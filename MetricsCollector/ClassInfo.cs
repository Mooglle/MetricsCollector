using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace MetricsCollector
{
	internal class ClassInfo
	{
		public string ClassName { get; set; }
		public MethodInfo[] MethodsInfo { get; set; }
		public PropertyInfo[] PropertiesInfo { get; set; }
		public FieldInfo[] FieldsInfo { get; set; }
		public MethodInfo[] OverriddenMethodsInfo { get; set; }
		public MethodInfo[] PublicMethodsInfo { get; set; }
		public MethodInfo[] PrivateMethodsInfo { get; set; }
		public MethodInfo[] NotInheritedMethodsInfo { get; set; }
	}
}
