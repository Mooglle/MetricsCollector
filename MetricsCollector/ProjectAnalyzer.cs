using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Configuration.Assemblies;

using BF = System.Reflection.BindingFlags;

namespace MetricsCollector
{
	internal class ProjectAnalyzer
	{
		public List<ClassInfo> infoList { get; set; }
		public List<ClassInfo> Analyze(string assemblyPath = @"C:\Users\Moogle\Downloads\Telegram Desktop\13 laba\13 laba\laba 8\bin\Debug\laba 8.dll") 
		{
			List<ClassInfo> result = new List<ClassInfo>();

			Assembly assembly = Assembly.LoadFrom(assemblyPath);
			Type[] types = assembly.GetTypes();

			foreach (Type type in types)
			{
				if (type.IsClass) // Check if the type is a class
				{
					result.Add(new ClassInfo { ClassName = type.FullName,
						MethodsInfo = type.GetMethods(BF.Public | BF.Instance | BF.NonPublic | BF.Static),
						PropertiesInfo = type.GetProperties(BF.Public | BF.Instance | BF.NonPublic | BF.Static),
						FieldsInfo = type.GetFields(BF.Public | BF.Instance | BF.NonPublic | BF.Static),
					});
				}
			}
			return result;
		}
	}
}
