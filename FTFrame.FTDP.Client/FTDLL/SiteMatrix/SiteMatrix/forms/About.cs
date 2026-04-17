using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SiteMatrix.consts;

namespace SiteMatrix.forms
{
	/// <summary>
	/// ErrorReport 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
	public class About : System.Windows.Forms.Form
	{
        private System.Windows.Forms.TextBox errorText;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Label label1;
		public string errorString;
		public string errorView;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

        public About()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.errorText = new System.Windows.Forms.TextBox();
            this.cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // errorText
            // 
            this.errorText.BackColor = System.Drawing.SystemColors.Control;
            this.errorText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.errorText.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.errorText.Location = new System.Drawing.Point(12, 36);
            this.errorText.Multiline = true;
            this.errorText.Name = "errorText";
            this.errorText.ReadOnly = true;
            this.errorText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.errorText.Size = new System.Drawing.Size(650, 318);
            this.errorText.TabIndex = 0;
            this.errorText.Text = resources.GetString("errorText.Text");
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(587, 360);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 24);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "确定";
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(485, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "版权所有  毛彬彬（MaoBinbin）个人所有，保留所有权利，2006-2032";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // About
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(674, 396);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.errorText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.Load += new System.EventHandler(this.About_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
        private void ApplyLanguage()
        {
            this.Text = res.About.GetString("_this");
            //label1.Text = res.About.GetString("label1");
           // errorText.Text = res.About.GetString("errorText") + "\r\nVersion:" + Application.ProductVersion;
           // errorText.Text += "\r\n4.0更新：";
cancel.Text = res.About.GetString("cancel");
            // if(errorString!=null)errorText.Text=errorString;
            label1.Text = "";
        }
		private void cancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void About_Load(object sender, EventArgs e)
        {
            label1.Text = "";
        }
    }
}
