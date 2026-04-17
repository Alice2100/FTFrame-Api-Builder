using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using mshtml;
using SiteMatrix.consts;
using SiteMatrix.StyleEditorAdapter;

namespace SiteMatrix.forms
{
	/// <summary>
	/// StyleBuilder 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
	public class StyleBuilder : System.Windows.Forms.Form
	{
		public string ControlPartID;
		public string PartStyleName;
		public IHTMLElement PartElement;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;
		public bool isCanceled=true;
		public IHTMLElement ele;
		private PropertyGrid propertyGrid1 = new PropertyGrid();
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		private PropertyGrid propertyGrid2 = new PropertyGrid();
		private PropertyGrid propertyGrid3 = new PropertyGrid();
		private PropertyGrid propertyGrid4 = new PropertyGrid();
		private PropertyGrid propertyGrid5 = new PropertyGrid();
		
		public StyleBuilder()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
			InitializeComponent();
            propertyGrid1.CommandsVisibleIfAvailable = true;
            propertyGrid1.Location = new Point(200, 20);
            propertyGrid1.Size = new System.Drawing.Size(390, 300);
            propertyGrid1.TabIndex = 1;
            propertyGrid1.Text = "Property Grid";

            this.Controls.Add(propertyGrid1);


            propertyGrid2.CommandsVisibleIfAvailable = true;
            propertyGrid2.Location = new Point(200, 20);
            propertyGrid2.Size = new System.Drawing.Size(390, 300);
            propertyGrid2.TabIndex = 1;
            propertyGrid2.Text = "Property Grid";

            this.Controls.Add(propertyGrid2);
            propertyGrid2.Visible = false;

            propertyGrid3.CommandsVisibleIfAvailable = true;
            propertyGrid3.Location = new Point(200, 20);
            propertyGrid3.Size = new System.Drawing.Size(390, 300);
            propertyGrid3.TabIndex = 1;
            propertyGrid3.Text = "Property Grid";

            this.Controls.Add(propertyGrid3);
            propertyGrid3.Visible = false;

            propertyGrid4.CommandsVisibleIfAvailable = true;
            propertyGrid4.Location = new Point(200, 20);
            propertyGrid4.Size = new System.Drawing.Size(390, 300);
            propertyGrid4.TabIndex = 1;
            propertyGrid4.Text = "Property Grid";

            this.Controls.Add(propertyGrid4);
            propertyGrid4.Visible = false;

            propertyGrid5.CommandsVisibleIfAvailable = true;
            propertyGrid5.Location = new Point(200, 20);
            propertyGrid5.Size = new System.Drawing.Size(390, 300);
            propertyGrid5.TabIndex = 1;
            propertyGrid5.Text = "Property Grid";

            this.Controls.Add(propertyGrid5);
            propertyGrid5.Visible = false;
            ApplyLanguage();
			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(16, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "Font";
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(16, 88);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 32);
            this.button2.TabIndex = 1;
            this.button2.Text = "background";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(392, 312);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 24);
            this.button3.TabIndex = 2;
            this.button3.Text = "OK";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(496, 312);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(80, 24);
            this.button4.TabIndex = 3;
            this.button4.Text = "Cancel";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button5.Location = new System.Drawing.Point(16, 136);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(120, 32);
            this.button5.TabIndex = 4;
            this.button5.Text = "border";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button6.Location = new System.Drawing.Point(16, 184);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(120, 32);
            this.button6.TabIndex = 5;
            this.button6.Text = "Position";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button7.Location = new System.Drawing.Point(16, 232);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(120, 32);
            this.button7.TabIndex = 6;
            this.button7.Text = "other";
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // StyleBuilder
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(594, 352);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StyleBuilder";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StyleBuilder";
            this.Load += new System.EventHandler(this.StyleBuilder_Load);
            this.ResumeLayout(false);

		}
		#endregion
        private void ApplyLanguage()
        {
            this.Text=res.StyleBuilder.GetString("_this");
            button1.Text = res.StyleBuilder.GetString("button1");
            button2.Text = res.StyleBuilder.GetString("button2");
            button5.Text = res.StyleBuilder.GetString("button5");
            button6.Text = res.StyleBuilder.GetString("button6");
            button7.Text = res.StyleBuilder.GetString("button7");
            button3.Text = res.StyleBuilder.GetString("button3");
            button4.Text = res.StyleBuilder.GetString("button4");
        }
		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
		isCanceled=false;
		this.Close();
		}

		private void StyleBuilder_Load(object sender, System.EventArgs e)
		{
            propertyGrid1.SelectedObject = new StyleEditorAdapter.StyleEditorAdapter.FontEditAdapter(ele, ControlPartID, PartStyleName, PartElement);
            propertyGrid2.SelectedObject = new StyleEditorAdapter.StyleEditorAdapter.BackGroundEditAdapter(ele, ControlPartID, PartStyleName, PartElement);

            propertyGrid3.SelectedObject = new StyleEditorAdapter.StyleEditorAdapter.BorderEditAdapter(ele, ControlPartID, PartStyleName, PartElement);

            propertyGrid4.SelectedObject = new StyleEditorAdapter.StyleEditorAdapter.PadingEditAdapter(ele, ControlPartID, PartStyleName, PartElement);

            propertyGrid5.SelectedObject = new StyleEditorAdapter.StyleEditorAdapter.OtherEditAdapter(ele, ControlPartID, PartStyleName, PartElement);

		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void button1_Click_1(object sender, System.EventArgs e)
		{
			propertyGrid1.Visible=true;
			propertyGrid2.Visible=false;
			propertyGrid3.Visible=false;
			propertyGrid4.Visible=false;
			propertyGrid5.Visible=false;
            button1.Enabled = false;
            button2.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			propertyGrid1.Visible=false;
			propertyGrid2.Visible=true;
			propertyGrid3.Visible=false;
			propertyGrid4.Visible=false;
			propertyGrid5.Visible=false;
            button1.Enabled = true;
            button2.Enabled = false;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			propertyGrid1.Visible=false;
			propertyGrid2.Visible=false;
			propertyGrid3.Visible=true;
			propertyGrid4.Visible=false;
			propertyGrid5.Visible=false;
            button1.Enabled = true;
            button2.Enabled = true;
            button5.Enabled = false;
            button6.Enabled = true;
            button7.Enabled = true;
		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			propertyGrid1.Visible=false;
			propertyGrid2.Visible=false;
			propertyGrid3.Visible=false;
			propertyGrid4.Visible=true;
			propertyGrid5.Visible=false;
            button1.Enabled = true;
            button2.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = false;
            button7.Enabled = true;
		}

		private void button7_Click(object sender, System.EventArgs e)
		{
			propertyGrid1.Visible=false;
			propertyGrid2.Visible=false;
			propertyGrid3.Visible=false;
			propertyGrid4.Visible=false;
			propertyGrid5.Visible=true;
            button1.Enabled = true;
            button2.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = false;
		}
	
		
	
	}
}
