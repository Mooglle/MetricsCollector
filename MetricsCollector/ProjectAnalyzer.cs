using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Configuration.Assemblies;

using BF = System.Reflection.BindingFlags;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MetricsCollector
{
	internal class ProjectAnalyzer
	{
		public List<ClassInfo> infoList { get; set; }
		public (List<ClassInfo>, MetricsContainer) Analyze(string assemblyPath, string projectPath) 
		{
			if (!File.Exists(assemblyPath))
			{
				MessageBox.Show("File not exists in assembly path");
				return (null, null);
			}
			if (!Directory.Exists(projectPath))
			{
				MessageBox.Show("Directory not exists in project path");
				return (null, null);
			}

			List<ClassInfo> resultClassInfo = new List<ClassInfo>();
			MetricsContainer resultMetrics = new MetricsContainer();

			Assembly assembly = Assembly.LoadFrom(assemblyPath);
			Type[] types = assembly.GetTypes();

			var aos = CalculateAOS(projectPath);
			var oc = CalculateOC(projectPath);

			foreach (Type type in types)
			{
				if (type.IsClass) // Check if the type is a class
				{
					var methodsInfo = type.GetMethods(BF.Public | BF.Instance | BF.NonPublic | BF.Static);
					var propertiesInfo = type.GetProperties(BF.Public | BF.Instance | BF.NonPublic | BF.Static);
					var fieldsInfo = type.GetFields(BF.Public | BF.Instance | BF.NonPublic | BF.Static);
					var overriddenMethods = methodsInfo.Where(m => IsOverride(m)).ToArray();
					var notInheritedMethods = type.GetMethods(BF.Public | BF.Instance | BF.NonPublic | BF.Static).Where(m => m.DeclaringType == type && !IsOverride(m)).ToArray();

					resultClassInfo.Add(new ClassInfo { ClassName = type.FullName,
						MethodsInfo = methodsInfo,
						PropertiesInfo = propertiesInfo,
						FieldsInfo = fieldsInfo,
						PublicMethodsInfo = methodsInfo.Where(m => m.IsPublic).ToArray(),
						PrivateMethodsInfo = methodsInfo.Where(m => m.IsPrivate).ToArray(),
						OverriddenMethodsInfo = overriddenMethods,
						NotInheritedMethodsInfo = notInheritedMethods,
					});

					resultMetrics.CS.Add(new Metric()
					{
						Name = "Class Size",
						Description = type.FullName,
						Value = methodsInfo.Length + propertiesInfo.Length
					});

					resultMetrics.NOO.Add(new Metric()
					{
						Name = "Number of overridden operations",
						Description = type.FullName,
						Value = overriddenMethods.Count()
					});

					resultMetrics.NOA.Add(new Metric()
					{
						Name = "Number of added methods",
						Description = type.FullName,
						Value = notInheritedMethods.Count()
					});

					resultMetrics.SI.Add(new Metric()
					{
						Name = "Specialization index",
						Description = type.FullName,
						Value = (double)(overriddenMethods.Count() * GetInheritanceLevel(type)) / methodsInfo.Count(), 
					});

					resultMetrics.AOS.Add(new Metric()
					{
						Name = "Average operation size",
						Description = type.FullName,
						Value = aos.ContainsKey(type.FullName) ? aos[type.FullName] : 0,
					});

					resultMetrics.OC.Add(new Metric()
					{
						Name = "Operation complexity",
						Description = type.FullName,
						Value = oc.ContainsKey(type.FullName) ? oc[type.FullName] : 0,
					});

					resultMetrics.ANP.Add(new Metric()
					{
						Name = "Average number of parameters",
						Description = type.FullName,
						Value = new Func<MethodInfo[], double>(x => {
							int prms = 0;
							foreach (var mi in x) {
								prms += mi.GetParameters().Count();
							}
							return (double)prms / x.Count();
						})(methodsInfo),
					});

					resultMetrics.Jilb.Add(new Metric()
					{
						Name = "Jilb",
						Description = type.FullName,
						Value = CalculateJilb(projectPath),
					});
				}
			}
			return (resultClassInfo, resultMetrics);
		}
		private double CalculateJilb(string projectPath)
		{
			double result = 0;
			return result;
		}
		/*
 * 
Действие                                             Вес      Учитывается
Определение (описание) переменной-параметра          0,3          Да
Определение (описание) временной переменной          0,5          Да
Присваивание значения                                0,5          Да
Вложенное выражение                                  0,5          Нет
Сообщение без параметров                             1            Да
Арифметическая операция                              2            Да
Сообщение с параметрами                              3            Да
Вызов стандартной функции интерфейса (API)           5            Нет
Вызов пользовательской функции (простой вызов)       7            Нет
 * 
 */
		private Dictionary<string, double> CalculateOC(string projectPath)
		{
			Dictionary<string, double> result = new Dictionary<string, double>();
			int numVarParams = 0;
			int numTempParams = 0;
			int numAssignments = 0;
			int numMessages = 0;
			int numParamMessages = 0;
			int numMathOps = 0;

			foreach (var file in Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories))
			{
				if (file.Contains("Designer.cs"))
					continue;
				using (StreamReader sr = new StreamReader(file))
				{
					string prevInput = "";
					string input = "";
					string[] className = new string[2];
					while (!sr.EndOfStream)
					{
						prevInput = input;
						input = sr.ReadLine();
						if (input.Contains("namespace"))
						{
							string[] s = input.Trim().Split(' ');
							className[0] = s[1] + '.';
							continue;
						}
						if (input.Contains("class"))
						{
							string[] s = input.Trim().Split(' ');
							for (int i = 0; i < s.Length; i++)
							{
								if (s[i] == "class")
									className[1] = s[i + 1];
							}
							continue;
						}
						if (Regex.Matches(input, @"^\ {8}\w").Count > 0)
						{
							continue;
						}
						numAssignments += Regex.Matches(input, @" = ").Count;
						numAssignments += Regex.Matches(input, @"\+\=").Count;
						numAssignments += Regex.Matches(input, @"\-\=").Count;
						numAssignments += Regex.Matches(input, @"\*\=").Count;
						numAssignments += Regex.Matches(input, @"\/\=").Count;
						numMathOps += Regex.Matches(input, @" \- ").Count;
						numMathOps += Regex.Matches(input, @" \+ ").Count;
						numMathOps += Regex.Matches(input, @" \/ ").Count;
						numMathOps += Regex.Matches(input, @" \* ").Count;
						numMathOps += Regex.Matches(input, @" \% ").Count;
						numMathOps += Regex.Matches(input, @"\+\+").Count;
						numMathOps += Regex.Matches(input, @"\-\-").Count;
						numMathOps += Regex.Matches(input, @"\-\=").Count;
						numMathOps += Regex.Matches(input, @"\+\=").Count;
						numMathOps += Regex.Matches(input, @"\*\=").Count;
						numMathOps += Regex.Matches(input, @"\/\=").Count;
						numMessages += Regex.Matches(input, @"\w+\(\)").Count;
						numParamMessages += Regex.Matches(input, @"\w+\(\w").Count;
					}
					var name = className[0] + className[1];
					double oc = 0.3 * numVarParams + 0.5 * numTempParams + 0.5 * numAssignments + numMessages + 2 * numMathOps + 3 * numParamMessages;
					if (!result.ContainsKey(name))
					{
						result.Add(name, oc);
					}
					else
					{
						result[name] += oc;
					}
					numVarParams = 0;
					numTempParams = 0;
					numAssignments = 0;
					numMessages = 0;
					numParamMessages = 0;
					numMathOps = 0;
				}
			}
			return result;
		}
		private Dictionary<string, double> CalculateAOS(string projectPath)
		{
			Dictionary<string, double[]> subResult = new Dictionary<string, double[]>();
			Dictionary<string, double> result = new Dictionary<string, double>();
			int numMethods = 0;
			int numLines = 0;
			bool startCounting = false;
			foreach (var file in Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories))
			{
				if (file.Contains("Designer.cs"))
					continue;
				using (StreamReader sr = new StreamReader(file))
				{
					string input;
					string[] className = new string[2];
					while (!sr.EndOfStream)
					{
						input = sr.ReadLine();
						if (input.Contains("namespace")){
							string[] s = input.Trim().Split(' ');
							className[0] = s[1] + '.';
							continue;
						}
						if (input.Contains("class"))
						{
							string[] s = input.Trim().Split(' ');
							for(int i = 0; i < s.Length; i++)
							{
								if (s[i] == "class")
									className[1] = s[i + 1];
							}
							continue;
						}
						if (input.Contains(" " + className[1] + "("))
						{
							numMethods--;
						}
						if (input == "        {" )
						{
							startCounting= true;
							numMethods++;
							continue;
						}
						if (input.Contains("        p") && input.Contains("=>"))
						{
							numLines++;
							numMethods++;
							continue;
						}
						if (input == "        }")
						{
							startCounting = false;
							continue;
						}
						if (input.Replace(" ", "") != "" && startCounting && input.Replace(" ", "") != "{" && input.Replace(" ", "") != "}")
						{
							numLines++;
						}
					}
					var name = className[0] + className[1];
					if (!subResult.ContainsKey(name))
					{
						subResult.Add(name, new double[] { numLines, numMethods });
					}
					else
					{
						subResult[name][0] += numLines;
						subResult[name][1] += numMethods;
					}
					numMethods = 0;
					numLines = 0;
				}
			}
			foreach(var pair in subResult)
			{
				var aos = (double)pair.Value[0] / pair.Value[1];
				result.Add(pair.Key, aos);
			}
			return result;
		}
		private int GetInheritanceLevel(Type t)
		{
			int inheritanceLevel = 0;
			Type curType = t;
			while (curType.BaseType != null)
			{
				inheritanceLevel++;
				curType = curType.BaseType;
			}
			return inheritanceLevel;
		}
		private bool IsOverride(MethodInfo m)
		{
			return m.GetBaseDefinition().DeclaringType != m.DeclaringType;
		}
	}
}
