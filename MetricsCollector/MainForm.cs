using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MetricsCollector
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			int a = 1;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			List<ClassInfo> list = new ProjectAnalyzer().Analyze();
			listViewClassInfo.Items.Clear();
			foreach (var info in list)
			{
				var test = info.MethodsInfo.Where(m => !m.IsVirtual).ToArray();
				var item = new ListViewItem(new string[] {
					info.ClassName,
					info.MethodsInfo.Count().ToString(),
					info.MethodsInfo.Count(m => m.IsPublic).ToString(),
					info.MethodsInfo.Count(m => m.IsPrivate).ToString(),
					info.MethodsInfo.Count(m => m.IsPublic && !m.IsVirtual).ToString(),
					info.MethodsInfo.Count(m => m.IsPrivate && !m.IsVirtual).ToString(),
				});
				listViewClassInfo.Items.Add(item);
			}

		}
	}
}
