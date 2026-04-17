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
	public class ErrorReport : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox errorText;
		private System.Windows.Forms.Button send;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Label label1;
		public string errorString;
		public string errorView;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ErrorReport()
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
            this.errorText = new System.Windows.Forms.TextBox();
            this.send = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // errorText
            // 
            this.errorText.BackColor = System.Drawing.SystemColors.Control;
            this.errorText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.errorText.Location = new System.Drawing.Point(12, 35);
            this.errorText.Multiline = true;
            this.errorText.Name = "errorText";
            this.errorText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.errorText.Size = new System.Drawing.Size(368, 223);
            this.errorText.TabIndex = 0;
            this.errorText.Text = "errorText";
            // 
            // send
            // 
            this.send.Location = new System.Drawing.Point(215, 264);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(75, 24);
            this.send.TabIndex = 1;
            this.send.Text = "发送";
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(305, 264);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 24);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "取消";
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(320, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "以下为错误信息：";
            // 
            // ErrorReport
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(392, 294);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.send);
            this.Controls.Add(this.errorText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorReport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ErrorReport";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ErrorReport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
        private void ApplyLanguage()
        {
            this.Text = res.ErrorReport.GetString("_this");
            label1.Text = res.ErrorReport.GetString("label1");
            send.Text = res.ErrorReport.GetString("send");
            cancel.Text = res.ErrorReport.GetString("cancel");
        }
		private void cancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void ErrorReport_Load(object sender, System.EventArgs e)
		{
		errorText.Text=errorView;
		}
	}
}
