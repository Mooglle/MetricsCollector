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

namespace MetricsCollector
{
	internal class ProjectAnalyzer
	{
		public List<ClassInfo> infoList { get; set; }
		public (List<ClassInfo>, MetricsContainer) Analyze(string assemblyPath = @"C:\Users\Moogle\Downloads\Telegram Desktop\13 laba\13 laba\laba 8\bin\Debug\laba 8.dll") 
		{
			List<ClassInfo> resultClassInfo = new List<ClassInfo>();
			MetricsContainer resultMetrics = new MetricsContainer();

			Assembly assembly = Assembly.LoadFrom(assemblyPath);
			Type[] types = assembly.GetTypes();

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
				}
			}
			return (resultClassInfo, resultMetrics);
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
