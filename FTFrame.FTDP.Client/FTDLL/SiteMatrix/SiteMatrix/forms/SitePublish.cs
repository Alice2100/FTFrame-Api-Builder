using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SiteMatrix.SiteClass;
using SiteMatrix.consts;
using System.Net;
using System.Xml;

namespace SiteMatrix.forms
{
	/// <summary>
	/// SitePublish 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
	public class SitePublish : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label publishstat;
		private System.Windows.Forms.CheckBox republish;
		private System.Windows.Forms.RadioButton radioFull;
		private System.Windows.Forms.RadioButton radioUpdate;
		private System.Windows.Forms.Timer pubTimer;
		private System.ComponentModel.IContainer components;
private WebClient wc=new WebClient();
		private XmlDocument xmldom=new XmlDocument();
        private Button button4;
        private MaskedTextBox maskedTextBox1;
        private Label label1;
        private Label label2;
        private Button pageselect;
		public static string wcurl="";
        public ArrayList UpdatePages = new ArrayList();
        public bool PublishSinglePage = false;
        public string[] PublishSinglePageTag = null;
        public bool PublishForSplit = false;
        public string PublishSplitDir = null;
        public string PublishSplitFile = null;
        public string PublishSplitNewSubName = null;
		public SitePublish()
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
            this.components = new System.ComponentModel.Container();
            this.radioFull = new System.Windows.Forms.RadioButton();
            this.radioUpdate = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.publishstat = new System.Windows.Forms.Label();
            this.republish = new System.Windows.Forms.CheckBox();
            this.pubTimer = new System.Windows.Forms.Timer(this.components);
            this.button4 = new System.Windows.Forms.Button();
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pageselect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radioFull
            // 
            this.radioFull.Location = new System.Drawing.Point(12, 48);
            this.radioFull.Name = "radioFull";
            this.radioFull.Size = new System.Drawing.Size(107, 24);
            this.radioFull.TabIndex = 0;
            this.radioFull.Text = "Full Publish";
            // 
            // radioUpdate
            // 
            this.radioUpdate.Checked = true;
            this.radioUpdate.Location = new System.Drawing.Point(156, 47);
            this.radioUpdate.Name = "radioUpdate";
            this.radioUpdate.Size = new System.Drawing.Size(122, 24);
            this.radioUpdate.TabIndex = 1;
            this.radioUpdate.TabStop = true;
            this.radioUpdate.Text = "Update Publish";
            this.radioUpdate.CheckedChanged += new System.EventHandler(this.radioUpdate_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 137);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 24);
            this.button1.TabIndex = 2;
            this.button1.Text = "Publish";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(248, 178);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 24);
            this.button2.TabIndex = 3;
            this.button2.Text = "Check";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(336, 178);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 24);
            this.button3.TabIndex = 4;
            this.button3.Text = "Cansel";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // publishstat
            // 
            this.publishstat.Location = new System.Drawing.Point(51, 102);
            this.publishstat.Name = "publishstat";
            this.publishstat.Size = new System.Drawing.Size(272, 24);
            this.publishstat.TabIndex = 5;
            // 
            // republish
            // 
            this.republish.Location = new System.Drawing.Point(299, 48);
            this.republish.Name = "republish";
            this.republish.Size = new System.Drawing.Size(112, 24);
            this.republish.TabIndex = 6;
            this.republish.Text = "republish last";
            // 
            // pubTimer
            // 
            this.pubTimer.Tick += new System.EventHandler(this.pubTimer_Tick);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(156, 138);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(138, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "ftp upload";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.Location = new System.Drawing.Point(346, 140);
            this.maskedTextBox1.Mask = "9999";
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(30, 21);
            this.maskedTextBox1.TabIndex = 8;
            this.maskedTextBox1.Text = "5120";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(299, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "Package";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(382, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "Byte";
            // 
            // pageselect
            // 
            this.pageselect.BackColor = System.Drawing.Color.White;
            this.pageselect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.pageselect.ForeColor = System.Drawing.Color.Red;
            this.pageselect.Location = new System.Drawing.Point(155, 68);
            this.pageselect.Name = "pageselect";
            this.pageselect.Size = new System.Drawing.Size(154, 23);
            this.pageselect.TabIndex = 11;
            this.pageselect.Text = "pageselect";
            this.pageselect.UseVisualStyleBackColor = false;
            this.pageselect.Click += new System.EventHandler(this.pageselect_Click);
            // 
            // SitePublish
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(433, 214);
            this.ControlBox = false;
            this.Controls.Add(this.pageselect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.maskedTextBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.republish);
            this.Controls.Add(this.publishstat);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.radioUpdate);
            this.Controls.Add(this.radioFull);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SitePublish";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SitePublish";
            this.Load += new System.EventHandler(this.SitePublish_Load);
            this.Shown += new System.EventHandler(this.SitePublish_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
        private void ApplyLanguage()
        {
            this.Text=res.SitePublish.GetString("_this");
            radioFull.Text = res.SitePublish.GetString("radioFull");
            radioUpdate.Text = res.SitePublish.GetString("radioUpdate");
            republish.Text = res.SitePublish.GetString("republish");
            button1.Text = res.SitePublish.GetString("button1");
            button2.Text = res.SitePublish.GetString("button2");
            button3.Text = res.SitePublish.GetString("button3");
            button4.Text = res.SitePublish.GetString("button4");
            pageselect.Text = res.SitePublish.GetString("type1");
        }
		private void button3_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			bool isFullPublish=radioFull.Checked;
			radioFull.Enabled=false;
			radioUpdate.Enabled=false;
			button1.Enabled=false;
			button2.Enabled=false;
			button3.Enabled=false;
            button4.Enabled = false;
			republish.Enabled=false;
            maskedTextBox1.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            bool sb = SiteClass.Site.PublishSite(globalConst.CurSite.ID, isFullPublish, publishstat, republish.Checked, pubTimer, false, int.Parse(maskedTextBox1.Text), UpdatePages, PublishSinglePage, PublishForSplit, PublishSplitDir, PublishSplitFile, PublishSplitNewSubName);
			if(!sb)
			{
				radioFull.Enabled=true;
				radioUpdate.Enabled=true;
				button1.Enabled=true;
				button2.Enabled=true;
				button3.Enabled=true;
                button4.Enabled = true;
				republish.Enabled=true;
                maskedTextBox1.Enabled = true;
                label1.Enabled = true;
                label2.Enabled = true;
				publishstat.Text="";
			}
			else
			{
                consts.globalConst.MdiForm.MainStatus.Text = "Publish Successfuly!";
			    this.Close();
			}
		}

		private void pubTimer_Tick(object sender, System.EventArgs e)
		{
		pubTimer.Enabled=false;
        wc.DownloadFile(wcurl, globalConst.ConfigPath + "\\state.xml");
		Application.DoEvents();
        xmldom.Load(globalConst.ConfigPath + "\\state.xml");
		Application.DoEvents();
			if(xmldom.InnerXml==null)
			{
			pubTimer.Enabled=true;
				return;
			}
			if(xmldom.InnerXml.Trim().Equals(""))
			{
				pubTimer.Enabled=true;
				return;
			}
			if(xmldom.SelectSingleNode("//publishstate/step").InnerText.Equals("10"))
			{
				pubTimer.Enabled=true;
				return;
			}
			string nowtext=xmldom.SelectSingleNode("//publishstate/label").InnerText;
			if(!publishstat.Text.Equals(nowtext))
			{
			publishstat.Text=nowtext;
			}
			pubTimer.Enabled=true;
		}

        private void button2_Click(object sender, EventArgs e)
        {
            SiteReport sr=new SiteReport();
            sr.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text.Length < 3) maskedTextBox1.Text = "5120";
            bool isFullPublish = radioFull.Checked;
            radioFull.Enabled = false;
            radioUpdate.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            republish.Enabled = false;
            maskedTextBox1.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            bool sb = SiteClass.Site.PublishSite(globalConst.CurSite.ID, isFullPublish, publishstat, republish.Checked, pubTimer, true, int.Parse(maskedTextBox1.Text), UpdatePages, PublishSinglePage, PublishForSplit, PublishSplitDir, PublishSplitFile, PublishSplitNewSubName);
            if (!sb)
            {
                radioFull.Enabled = true;
                radioUpdate.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                republish.Enabled = true;
                maskedTextBox1.Enabled = true;
                label1.Enabled = true;
                label2.Enabled = true;
                publishstat.Text = "";
            }
            else
            {
                this.Close();
            }
        }

        private void pageselect_Click(object sender, EventArgs e)
        {
            PublishSel ps = new PublishSel();
            ps.SelFiles = UpdatePages;
            ps.ShowDialog();
            UpdatePages = ps.SelFiles;
            if (UpdatePages.Count == 0) pageselect.Text = res.SitePublish.GetString("type1");
            else pageselect.Text = res.SitePublish.GetString("type2").Replace("#", UpdatePages.Count.ToString());
        }

        private void radioUpdate_CheckedChanged(object sender, EventArgs e)
        {
            pageselect.Enabled = radioUpdate.Checked&&!republish.Checked;
        }

        private void SitePublish_Load(object sender, EventArgs e)
        {
            
        }

        private void SitePublish_Shown(object sender, EventArgs e)
        {
            if (PublishSinglePage)
            {
                radioUpdate.Checked = true;
                republish.Checked = false;
                pageselect.Text = "1 file selected";
                UpdatePages = new ArrayList();
                UpdatePages.Add(PublishSinglePageTag);
                button1_Click(sender, null);
            }
            else if (PublishForSplit)
            {
                radioUpdate.Checked = true;
                republish.Checked = false;
                pageselect.Text = "split file selected";
                button1_Click(sender, null);
            }
        }
	}
}
