namespace MetricsCollector
{
	partial class MainForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.listViewClassInfo = new System.Windows.Forms.ListView();
			this.listViewMetrics = new System.Windows.Forms.ListView();
			this.textBoxAssemblyPath = new System.Windows.Forms.TextBox();
			this.textBoxProjectPath = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(107, 509);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "ClassInfo";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// listViewClassInfo
			// 
			this.listViewClassInfo.HideSelection = false;
			this.listViewClassInfo.Location = new System.Drawing.Point(28, 12);
			this.listViewClassInfo.Name = "listViewClassInfo";
			this.listViewClassInfo.Size = new System.Drawing.Size(1070, 227);
			this.listViewClassInfo.TabIndex = 1;
			this.listViewClassInfo.UseCompatibleStateImageBehavior = false;
			this.listViewClassInfo.View = System.Windows.Forms.View.Details;
			// 
			// listViewMetrics
			// 
			this.listViewMetrics.HideSelection = false;
			this.listViewMetrics.Location = new System.Drawing.Point(283, 245);
			this.listViewMetrics.Name = "listViewMetrics";
			this.listViewMetrics.Size = new System.Drawing.Size(815, 327);
			this.listViewMetrics.TabIndex = 2;
			this.listViewMetrics.UseCompatibleStateImageBehavior = false;
			this.listViewMetrics.View = System.Windows.Forms.View.Details;
			// 
			// textBoxAssemblyPath
			// 
			this.textBoxAssemblyPath.Location = new System.Drawing.Point(47, 318);
			this.textBoxAssemblyPath.Name = "textBoxAssemblyPath";
			this.textBoxAssemblyPath.Size = new System.Drawing.Size(195, 22);
			this.textBoxAssemblyPath.TabIndex = 3;
			this.textBoxAssemblyPath.Text = "C:\\Users\\Moogle\\Downloads\\Telegram Desktop\\13 laba\\13 laba\\laba 8\\bin\\Debug\\laba " +
    "8.dll";
			// 
			// textBoxProjectPath
			// 
			this.textBoxProjectPath.Location = new System.Drawing.Point(47, 408);
			this.textBoxProjectPath.Name = "textBoxProjectPath";
			this.textBoxProjectPath.Size = new System.Drawing.Size(195, 22);
			this.textBoxProjectPath.TabIndex = 4;
			this.textBoxProjectPath.Text = "C:\\Users\\Moogle\\Downloads\\Telegram Desktop\\13 laba\\13 laba\\laba 8";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(47, 271);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Assembly Path";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(47, 367);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 16);
			this.label2.TabIndex = 6;
			this.label2.Text = "Project Path";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1110, 584);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxProjectPath);
			this.Controls.Add(this.textBoxAssemblyPath);
			this.Controls.Add(this.listViewMetrics);
			this.Controls.Add(this.listViewClassInfo);
			this.Controls.Add(this.button1);
			this.Name = "MainForm";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ListView listViewClassInfo;
		private System.Windows.Forms.ListView listViewMetrics;
		private System.Windows.Forms.TextBox textBoxAssemblyPath;
		private System.Windows.Forms.TextBox textBoxProjectPath;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}

