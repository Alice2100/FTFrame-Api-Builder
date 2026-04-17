using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SiteMatrix.consts;
using SiteMatrix.functions;
using SiteMatrix.Page;

namespace SiteMatrix.forms
{
	/// <summary>
	/// AddWare 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
	public class AddControl2Page : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox ControlCaption;
		private System.Windows.Forms.Label label1;
		public string controlName;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AddControl2Page()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
			InitializeComponent();
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
            this.ControlCaption = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(168, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "取消";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(51, 76);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 24);
            this.button2.TabIndex = 1;
            this.button2.Text = "确定";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ControlCaption
            // 
            this.ControlCaption.Location = new System.Drawing.Point(102, 28);
            this.ControlCaption.Name = "ControlCaption";
            this.ControlCaption.Size = new System.Drawing.Size(152, 21);
            this.ControlCaption.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "新实例名称";
            // 
            // AddControl2Page
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(292, 119);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ControlCaption);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddControl2Page";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddControl";
            this.Load += new System.EventHandler(this.AddWare_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
        private void ApplyLanguage()
        {
            this.Text = res.AddControl.GetString("_this");
            label1.Text = res.AddControl.GetString("label1");
            button2.Text = res.AddControl.GetString("button2");
            button1.Text = res.AddControl.GetString("button1");
        }
		private void AddWare_Load(object sender, System.EventArgs e)
		{
            Editor ed = form.getEditor();
            if (ed != null)
            {
                if (ControlCaption.Text.IndexOf("__") < 0)
                {
                    ControlCaption.Text = ed.thisTitle + "__" + ControlCaption.Text;
                }
                else
                {
                    ControlCaption.Text = ed.thisTitle + ControlCaption.Text.Substring(ControlCaption.Text.IndexOf("__"));
                }
            }
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
            if (ControlCaption.Text.Trim().Equals(""))
			{
			MsgBox.Warning(res.AddControl.GetString("m1"));
			return;
			}
			if(!PageWare.AddControl(ControlCaption.Text.Trim(),controlName,true))
			{
                MsgBox.Error(res.AddControl.GetString("m2"));
			}
			else
			{
			this.Close();
			}
		}
	}
}
