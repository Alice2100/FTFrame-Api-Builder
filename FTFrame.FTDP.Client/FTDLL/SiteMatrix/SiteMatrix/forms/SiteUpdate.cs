using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SiteMatrix.consts;
using System.Data.OleDb;
using SiteMatrix.functions;
using mshtml;
using System.Net;
using System.Text;
using SiteMatrix.Compression;
using System.IO;
namespace SiteMatrix.forms
{
	/// <summary>
	/// SiteUpdate 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
	public class SiteUpdate : System.Windows.Forms.Form
	{
			public string siteid;
		private System.Windows.Forms.Label stat;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;

		public SiteUpdate()
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
			this.stat = new System.Windows.Forms.Label();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// stat
			// 
			this.stat.Location = new System.Drawing.Point(40, 24);
			this.stat.Name = "stat";
			this.stat.Size = new System.Drawing.Size(192, 24);
			this.stat.TabIndex = 0;
			// 
			// timer1
			// 
			this.timer1.Interval = 500;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// SiteUpdate
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(282, 80);
			this.ControlBox = false;
			this.Controls.Add(this.stat);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "SiteUpdate";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SiteUpdate";
			this.Load += new System.EventHandler(this.SiteUpdate_Load);
			this.ResumeLayout(false);

		}
		#endregion
        private void ApplyLanguage()
        {
            this.Text = res.SiteUpdate.GetString("_this");
        }
		private void SiteUpdate_Load(object sender, System.EventArgs e)
		{
            if (globalConst.CurSite.ID == null)
            {
                MsgBox.Warning(res.SiteUpdate.GetString("m1"));
                this.Close();
                return;
            }
			stat.Text=res.SiteUpdate.GetString("s1");
			Application.DoEvents();
            if (MsgBox.OKCancel(res.SiteUpdate.GetString("String1")).Equals(DialogResult.OK))
            {
                timer1.Enabled = true;
            }
            else
            {
                this.Close();
            }
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			timer1.Enabled=false;
			bool result=doupdate();
			if(result)
			{
				MsgBox.Information(res.SiteUpdate.GetString("m2"));
			}
			else
			{
				MsgBox.Warning(res.SiteUpdate.GetString("m3"));
			}
			this.Close();
            string siteid = globalConst.CurSite.ID;
            SiteClass.Site.close();
            SiteClass.Site.open(siteid);
		}
		private bool doupdate()
		{
		try
		{
			//check from server
			
			HTMLDocumentClass hc = new HTMLDocumentClass();
			IHTMLDocument2 doc2 = hc;
			doc2.write("");
			doc2.close();
			IHTMLDocument4 doc4 = hc;
			string sql="select * from sites where id='" + siteid + "'";
			OleDbDataReader rdr=globalConst.ConfigConn.OpenRecord(sql);
			string _url;
			string _id=siteid;
			string _key;
			string _user;
			string _passwd;
			int _version=0;
			if(rdr.Read())
			{
				_url=rdr.GetString(rdr.GetOrdinal("url"));
				_key=rdr.GetString(rdr.GetOrdinal("cdkey"));
				_user=rdr.GetString(rdr.GetOrdinal("username"));
				_passwd=rdr.GetString(rdr.GetOrdinal("passwd"));
			}
			else
			{
				log.Error("siteid is " + siteid + " not found while check server!");
				return false;
			}
			rdr.Close();
            string addurl = _url + "/_ftpub/siteadd.aspx?_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd;

			try
			{
				new WebClient().DownloadFile(addurl,globalConst.ConfigPath + "\\result.tmp");
			}
			catch
			{}

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
                MsgBox.Error(res.SiteUpdate.GetString("m4"));
			}
			else
			{
				returnstr=returnstr.Trim();
				returnstr=returnstr.Replace("{ftserver}","");
				if(returnstr.StartsWith("ok{domin"))
				{
					addSuccess=true;
					_version=int.Parse(returnstr.Substring(returnstr.IndexOf("{version")+8));
				}
				else
				{
					MsgBox.Warning(returnstr);
				}
			}
			if(!addSuccess)return false;

            stat.Text = res.SiteUpdate.GetString("s2");
			Application.DoEvents();

            string changeurl = _url + "/_ftpub/hidden2zip.aspx?_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd;
	
			byte[] responseArray = new WebClient().DownloadData(changeurl);
			if(!Encoding.ASCII.GetString(responseArray).Trim().Equals("{ftserver}ok"))
			{
				MsgBox.Error(Encoding.ASCII.GetString(responseArray).Replace("{ftserver}",""));
				return false;
			}

            stat.Text = res.SiteUpdate.GetString("s3");
			Application.DoEvents();
			
			if(dir.Exists(globalConst.AppPath + @"\temp"))
			{
				dir.Delete(globalConst.AppPath + @"\temp",true);
			}
			dir.CreateDirectory(globalConst.AppPath + @"\temp");
			try
			{
                new WebClient().DownloadFile(_url + "/_ftpub/site.zip", globalConst.AppPath + @"\temp\site.zip");
			}
			catch
			{}


            stat.Text = res.SiteUpdate.GetString("s4");
			Application.DoEvents();

            changeurl = _url + "/_ftpub/zip2hidden.aspx.aspx?_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd;
	
			responseArray = new WebClient().DownloadData(changeurl);
			if(!Encoding.ASCII.GetString(responseArray).Trim().Equals("{ftserver}ok"))
			{
				MsgBox.Error(Encoding.ASCII.GetString(responseArray).Replace("{ftserver}",""));
				return false;
			}

			//unzip
            stat.Text = res.SiteUpdate.GetString("s5");
			Application.DoEvents();
			new ZipClass().UnZip(globalConst.AppPath + @"\temp\site.zip",globalConst.AppPath + @"\temp\site\");


            stat.Text = res.SiteUpdate.GetString("s6");
			Application.DoEvents();
			file.Copy(globalConst.AppPath + @"\temp\site\site.db",globalConst.ConfigPath + @"\site_" + siteid + ".db",true);
            if (file.Exists(globalConst.AppPath + @"\temp\site\formtable.xml"))
            {
                file.Copy(globalConst.AppPath + @"\temp\site\formtable.xml", globalConst.ConfigPath + @"\" + siteid + "_formtable.xml", true);
            }

            stat.Text = res.SiteUpdate.GetString("s7");
			Application.DoEvents();
			FileInfo[] fis;
				if(dir.Exists(globalConst.AppPath + @"\temp\site\index"))
				{
					fis = new DirectoryInfo(globalConst.AppPath + @"\temp\site\index").GetFiles();
					foreach(FileInfo fi in fis)
					{
						fi.CopyTo(globalConst.LibPath + @"\" + fi.Name,true);
					}
				}

                stat.Text = res.SiteUpdate.GetString("s8");
			Application.DoEvents();

			if(dir.Exists(globalConst.AppPath + @"\temp\site\bin"))
			{
				fis = new DirectoryInfo(globalConst.AppPath + @"\temp\site\bin").GetFiles();
				foreach(FileInfo fi in fis)
				{
					fi.CopyTo(globalConst.LibPath + @"\" + fi.Name,true);
				}
			}

            stat.Text = res.SiteUpdate.GetString("s9");
			Application.DoEvents();
			

			if(dir.Exists(globalConst.AppPath + @"\temp\site\lib"))
			{
				DirectoryInfo[] dis = new DirectoryInfo(globalConst.AppPath + @"\temp\site\lib").GetDirectories();
				foreach(DirectoryInfo di in dis)
				{
					if(dir.Exists(globalConst.LibPath + @"\" + di.Name))
					{
					dir.Delete(globalConst.LibPath + @"\" + di.Name,true);
					}
                    try
                    {
					di.MoveTo(globalConst.LibPath + @"\" + di.Name);
                    }
                            catch { }
				}
			}

            stat.Text = res.SiteUpdate.GetString("s10");
			Application.DoEvents();

			if(dir.Exists(globalConst.SitesPath + @"\" + siteid))
			{
				dir.Delete(globalConst.SitesPath + @"\" + siteid,true);
			}

			stat.Text=res.SiteUpdate.GetString("s11");
			Application.DoEvents();

			if(dir.Exists(globalConst.AppPath + @"\temp\site\site"))
			{
				dir.Move(globalConst.AppPath + @"\temp\site\site",globalConst.SitesPath + @"\" + siteid);
			}
			if(!dir.Exists(globalConst.SitesPath + @"\" + siteid))
			{
				dir.CreateDirectory(globalConst.SitesPath + @"\" + siteid);
			}
            stat.Text = res.SiteUpdate.GetString("s12");
			Application.DoEvents();
			if(dir.Exists(globalConst.AppPath + @"\temp"))
			{
				dir.Delete(globalConst.AppPath + @"\temp",true);
			}
            stat.Text = res.SiteUpdate.GetString("s13");
			Application.DoEvents();

			globalConst.ConfigConn.execSql("update sites set version=" + _version + " where id='" + siteid + "'");

			return true;
		}
		catch(Exception ex)
	{
	new error(ex);
		return false;
	}
	}
	}
}
