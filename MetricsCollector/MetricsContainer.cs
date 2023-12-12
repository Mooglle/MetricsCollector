using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsCollector
{
	internal class MetricsContainer
	{
		public List<Metric> CS { get; set; } = new List<Metric>();
		public List<Metric> NOO { get; set; } = new List<Metric>();
		public List<Metric> NOA { get; set; } = new List<Metric>();
		public List<Metric> SI { get; set; } = new List<Metric>();
		public List<Metric> AOS { get; set; } = new List<Metric>();
		public List<Metric> OC { get; set; } = new List<Metric>();
		public List<Metric> ANP { get; set; } = new List<Metric>();
		public MetricsContainer() { }
	}
}
