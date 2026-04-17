using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FTDPClient.functions;
using FTDPClient.consts;
using mshtml;
using FTDPClient.forms.control;

namespace FTDPClient.forms
{
	/// <summary>
	/// SiteAdd 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class SiteAdd : System.Windows.Forms.Form
	{
		public bool isAddSuccess=false;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox id;
		private System.Windows.Forms.TextBox url;
		private System.Windows.Forms.TextBox user;
		private System.Windows.Forms.TextBox passwd;
        private System.Windows.Forms.TextBox key;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SiteAdd()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.id = new System.Windows.Forms.TextBox();
            this.url = new System.Windows.Forms.TextBox();
            this.user = new System.Windows.Forms.TextBox();
            this.passwd = new System.Windows.Forms.TextBox();
            this.key = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(21, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 31);
            this.label2.TabIndex = 1;
            this.label2.Text = "Url:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(21, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 31);
            this.label3.TabIndex = 2;
            this.label3.Text = "User:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(21, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 31);
            this.label4.TabIndex = 3;
            this.label4.Text = "Key:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(232, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 31);
            this.label5.TabIndex = 4;
            this.label5.Text = "PassWD:";
            // 
            // id
            // 
            this.id.Location = new System.Drawing.Point(117, 15);
            this.id.Name = "id";
            this.id.Size = new System.Drawing.Size(345, 25);
            this.id.TabIndex = 5;
            // 
            // url
            // 
            this.url.Location = new System.Drawing.Point(117, 63);
            this.url.Name = "url";
            this.url.Size = new System.Drawing.Size(345, 25);
            this.url.TabIndex = 6;
            this.url.Text = "http://";
            // 
            // user
            // 
            this.user.Location = new System.Drawing.Point(117, 112);
            this.user.Name = "user";
            this.user.Size = new System.Drawing.Size(107, 25);
            this.user.TabIndex = 7;
            // 
            // passwd
            // 
            this.passwd.Location = new System.Drawing.Point(337, 112);
            this.passwd.Name = "passwd";
            this.passwd.PasswordChar = '*';
            this.passwd.Size = new System.Drawing.Size(125, 25);
            this.passwd.TabIndex = 8;
            // 
            // key
            // 
            this.key.Location = new System.Drawing.Point(117, 167);
            this.key.Name = "key";
            this.key.Size = new System.Drawing.Size(345, 25);
            this.key.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(117, 226);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 39);
            this.button1.TabIndex = 10;
            this.button1.Text = "注册";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(327, 226);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 39);
            this.button2.TabIndex = 11;
            this.button2.Text = "Cancel";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // SiteAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 18);
            this.ClientSize = new System.Drawing.Size(515, 295);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.key);
            this.Controls.Add(this.passwd);
            this.Controls.Add(this.user);
            this.Controls.Add(this.url);
            this.Controls.Add(this.id);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SiteAdd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SiteAdd";
            this.Load += new System.EventHandler(this.SiteAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
        private void ApplyLanguage()
        {
            this.Text = res.SiteAdd.GetString("_this");
            label1.Text = res.SiteAdd.GetString("label1");
            label2.Text = res.SiteAdd.GetString("label2");
            label3.Text = res.SiteAdd.GetString("label3");
            label4.Text = res.SiteAdd.GetString("label4");
            label5.Text = res.SiteAdd.GetString("label5");
            button1.Text = res.SiteAdd.GetString("button1");
            button2.Text = res.SiteAdd.GetString("button2");
        }
		private void button2_Click(object sender, System.EventArgs e)
        {
		this.Close();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			try
			{
				string _id=id.Text.Trim();
				string _url=url.Text;
				string _user=user.Text;
				string _passwd=passwd.Text;
				string _key=key.Text;
				string _domin="";
				string _caption="";
				string _groupname="";
				int _version=0;
				if(_id.Equals(""))
				{
                    MsgBox.Warning(res.SiteAdd.GetString("m1"));
					id.Focus();
					return;
				}
				if(_url.Equals(""))
				{
                    MsgBox.Warning(res.SiteAdd.GetString("m2"));
					url.Focus();
					return;
				}
				if(_key.Equals(""))
				{
                    MsgBox.Warning(res.SiteAdd.GetString("m3"));
					key.Focus();
					return;
				}
				string sql="select count(id) as c from sites where id='" + _id + "'";
				if(globalConst.ConfigConn.GetInt32(sql)>0)
				{

                    MsgBox.Warning(_id + res.SiteAdd.GetString("m4"));
					id.Focus();
					return;
				}
				id.Enabled=false;
				url.Enabled=false;
				user.Enabled=false;
				passwd.Enabled=false;
				key.Enabled=false;
				button1.Enabled=false;
				button2.Enabled=false;
				//check from server
				HTMLDocumentClass hc = new HTMLDocumentClass();
				IHTMLDocument2 doc2 = hc;
				doc2.write("");
				doc2.close();
				IHTMLDocument4 doc4 = hc;
				string addurl=_url + "/_ftpub/siteadd?_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd;

				new System.Net.WebClient().DownloadFile(addurl,globalConst.ConfigPath + "\\result.tmp");

				System.Windows.Forms.Application.DoEvents();

				IHTMLDocument2 doc = doc4.createDocumentFromUrl(globalConst.ConfigPath + "\\result.tmp","null");
				while(doc.readyState != "complete")
				{
					System.Windows.Forms.Application.DoEvents();
				}
				string returnstr=doc.body.innerText;
				bool addSuccess=false;
				if(!returnstr.StartsWith("{ftserver}"))
				{
                    MsgBox.Error(res.SiteAdd.GetString("m5"));
				}
				else
				{
					returnstr=returnstr.Trim();
					returnstr=returnstr.Replace("{ftserver}","");
					if(returnstr.StartsWith("ok{domin"))
					{
						addSuccess=true;
						int i1=returnstr.IndexOf("{domin");
						int i2=returnstr.IndexOf("{caption");
						int i3=returnstr.IndexOf("{group");
						int i4=returnstr.IndexOf("{version");
						_domin=returnstr.Substring(i1+6,i2-i1-6);
						_caption=returnstr.Substring(i2+8,i3-i2-8);
						_groupname=returnstr.Substring(i3+6,i4-i3-6);
						_version=int.Parse(returnstr.Substring(i4+8));

					}
					else
					{
						MsgBox.Warning(returnstr);
					}
				}
				if(!addSuccess)
				{
					id.Enabled=true;
					url.Enabled=true;
					user.Enabled=true;
					passwd.Enabled=true;
					key.Enabled=true;
					button1.Enabled=true;
					button2.Enabled=true;
					return;
				}

				dir.CreateDirectory(globalConst.SitesPath + @"\" + _id);
                file.Copy(globalConst.defaultCssFile, globalConst.SitesPath + @"\" + _id + @"\default.css");
				file.Copy(globalConst.ConfigPath + @"\" + "empty.db",globalConst.ConfigPath + @"\" + "site_" + _id + ".db",true);
				sql="insert into sites(id,domin,caption,username,passwd,cdkey,version,url,homepage,groupname)values('" + _id.Replace("'","''") + "','"+ _domin.Replace("'","''") + "','"+ _caption.Replace("'","''") + "','" + _user.Replace("'","''") + "','" + _passwd.Replace("'","''") + "','" + _key.Replace("'","''") + "',0,'" + _url.Replace("'","''") + "','','"+ _groupname.Replace("'","''") + "')";
				globalConst.ConfigConn.execSql(sql);
                MsgBox.Information(res.SiteAdd.GetString("m6"));
				this.isAddSuccess=true;
				if(_version>0)
				{
                    if (MsgBox.YesNo(res.SiteAdd.GetString("m7")).Equals(DialogResult.Yes))
					{
						//同步数据
						
						if(globalConst.CurSite.ID!=null && globalConst.CurSite.ID.Equals(_id))
						{
                            MsgBox.Warning(_id + res.SiteAdd.GetString("m8"));
						}
						else
						{
							SiteClass.Site.close();
							SiteClass.Site.open(_id);
							PageUpdate pu = new PageUpdate();
							pu.ShowDialog();
							//return;
							//SiteUpdate su=new SiteUpdate();
							//su.siteid=_id;
							//su.ShowDialog();
						}
					}
				}
				this.Close();
			}
			catch(Exception ex)
			{
			new error(ex);
			}
		}

        private void SiteAdd_Load(object sender, EventArgs e)
        {

        }
    }
}
