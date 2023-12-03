using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MetricsCollector
{
	internal class ClassInfo
	{
		public string ClassName { get; set; }
		public MethodInfo[] MethodsInfo { get; set; }
		public PropertyInfo[] PropertiesInfo { get; set; }
		public FieldInfo[] FieldsInfo { get; set; }
	}
}
