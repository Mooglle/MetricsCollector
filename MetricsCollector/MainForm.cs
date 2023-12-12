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
		List<ClassInfo> classInfos;
		MetricsContainer metricsContainer;
		public MainForm()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			(classInfos, metricsContainer) = new ProjectAnalyzer().Analyze();

			listViewClassInfo.Items.Clear();
			listViewClassInfo.Columns.Clear();
			listViewMetrics.Items.Clear();
			listViewMetrics.Columns.Clear();

			SetListViewClassInfosColumns();
			SetListViewClassInfosRows();

			SetListViewMetricsColumns();
			SetListViewMetricsRows();
		}

		private void SetListViewClassInfosColumns()
		{
			List<ColumnHeader> classInfoColumns = new List<ColumnHeader>();
			foreach (var property in typeof(ClassInfo).GetProperties())
			{
				classInfoColumns.Add(new ColumnHeader() { Text = property.Name });
			}
			listViewClassInfo.Columns.AddRange(classInfoColumns.ToArray());
		}
		private void SetListViewClassInfosRows()
		{
			var classInfoProperties = typeof(ClassInfo).GetProperties().ToArray();
			for (int i = 0; i < classInfos.Count; i++)
			{
				List<string> classInfosRow = new List<string>();

				foreach (var property in classInfoProperties)
				{
					var p = property.GetValue(classInfos[i], null);
					if (p.GetType().IsArray)
					{
						classInfosRow.Add(((Array)p).Length.ToString());
					}
					else
					{
						classInfosRow.Add(p.ToString());
					}
				}
				var item = new ListViewItem(classInfosRow.ToArray());

				listViewClassInfo.Items.Add(item);
			}
		}
		private void SetListViewMetricsColumns()
		{
			List<ColumnHeader> metricsColumns = new List<ColumnHeader>() { new ColumnHeader() { Text = "ClassName" } };
			foreach (var property in typeof(MetricsContainer).GetProperties())
			{
				metricsColumns.Add(new ColumnHeader() { Text = property.Name });
			}
			listViewMetrics.Columns.AddRange(metricsColumns.ToArray());
		}
		private void SetListViewMetricsRows()
		{
			var properties = typeof(MetricsContainer).GetProperties().Where(p => p.PropertyType == typeof(List<Metric>)).ToArray();
			for (int i = 0; i < metricsContainer.CS.Count; i++)
			{
				List<string> metricsRow = new List<string>() { metricsContainer.CS[i].Description };

				foreach (var property in properties)
				{
					var p = (List<Metric>)property.GetValue(metricsContainer, null);
					if (p.Count > i)
						metricsRow.Add(p[i].Value.ToString());
				}
				var item = new ListViewItem(metricsRow.ToArray());

				listViewMetrics.Items.Add(item);
			}
		}
	}
}
