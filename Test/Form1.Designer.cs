namespace Test
{
	partial class Form1
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && (components != null) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.customCompleteTextBox1 = new CustomCompleteTextBox.CustomCompleteTextBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(260, 31);
			this.button1.Margin = new System.Windows.Forms.Padding(4);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(100, 29);
			this.button1.TabIndex = 6;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(15, 112);
			this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(129, 23);
			this.comboBox1.TabIndex = 1;
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 15;
			this.listBox1.Location = new System.Drawing.Point(16, 15);
			this.listBox1.Margin = new System.Windows.Forms.Padding(4);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(159, 79);
			this.listBox1.TabIndex = 8;
			// 
			// customCompleteTextBox1
			// 
			this.customCompleteTextBox1.AutoDropWidth = true;
			this.customCompleteTextBox1.DisplayMember = "";
			this.customCompleteTextBox1.DropHeight = 100;
			this.customCompleteTextBox1.ItemFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.customCompleteTextBox1.ItemForeColor = System.Drawing.SystemColors.WindowText;
			this.customCompleteTextBox1.Location = new System.Drawing.Point(260, 111);
			this.customCompleteTextBox1.Margin = new System.Windows.Forms.Padding(4);
			this.customCompleteTextBox1.Name = "customCompleteTextBox1";
			this.customCompleteTextBox1.SelectedItem = null;
			this.customCompleteTextBox1.SelectedValue = null;
			this.customCompleteTextBox1.Size = new System.Drawing.Size(132, 25);
			this.customCompleteTextBox1.TabIndex = 7;
			this.customCompleteTextBox1.ValueMember = "";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(424, 160);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.customCompleteTextBox1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.button1);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox comboBox1;
        private CustomCompleteTextBox.CustomCompleteTextBox customCompleteTextBox1;
        private System.Windows.Forms.ListBox listBox1;
    }
}

