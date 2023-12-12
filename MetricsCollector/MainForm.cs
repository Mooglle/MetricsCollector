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

		}

		private void button1_Click(object sender, EventArgs e)
		{
			List<ClassInfo> classInfos;
			MetricsContainer metricsContainer;
			(classInfos, metricsContainer) = new ProjectAnalyzer().Analyze();

			listViewClassInfo.Items.Clear();
			listViewClassInfo.Columns.Clear();
			listViewMetrics.Items.Clear();
			listViewMetrics.Columns.Clear();

			listViewClassInfo.Columns.AddRange(new ColumnHeader[] {
				new ColumnHeader() { Text = "ClassName" },
				new ColumnHeader() { Text = "Methods" },
				new ColumnHeader() { Text = "Public methods" },
				new ColumnHeader() { Text = "Private methods" },
				new ColumnHeader() { Text = "User defined public methods" },
				new ColumnHeader() { Text = "User defined private methods" },
				new ColumnHeader() { Text = "Overridden methods" },
			});
			foreach (var info in classInfos)
			{
				var item = new ListViewItem(new string[] {
					info.ClassName,
					info.MethodsInfo.Count().ToString(),
					info.PublicMethodsInfo.Count().ToString(),
					info.PrivateMethodsInfo.Count().ToString(),
					info.MethodsInfo.Count(m => m.IsPublic && !m.IsVirtual).ToString(),
					info.MethodsInfo.Count(m => m.IsPrivate && !m.IsVirtual).ToString(),
					info.OverriddenMethodsInfo.Count().ToString(),
				});
				listViewClassInfo.Items.Add(item);
			}


			listViewMetrics.Columns.AddRange(new ColumnHeader[] {
				new ColumnHeader() { Text = "ClassName" },
				new ColumnHeader() { Text = "CS" },
				new ColumnHeader() { Text = "NOO" },
				new ColumnHeader() { Text = "NOA" },
				new ColumnHeader() { Text = "SI" },
			});
			for (int i = 0; i < metricsContainer.CS.Count; i++)
			{
				var item = new ListViewItem(new string[] {
					metricsContainer.CS[i].Description,
					metricsContainer.CS[i].Value.ToString(),
					metricsContainer.NOO[i].Value.ToString(),
					metricsContainer.NOA[i].Value.ToString(),
					metricsContainer.SI[i].Value.ToString(),
				});

				listViewMetrics.Items.Add(item);
			}
		}
	}
}
