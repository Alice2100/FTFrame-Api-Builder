using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FTDPClient.functions;
using FTDPClient.consts;
using System.IO;
using System.Xml;

namespace FTDPClient.forms
{
	/// <summary>
	/// AddSite 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class AddSite : System.Windows.Forms.Form
	{
		public bool AddSuc=false;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox _id;
		private System.Windows.Forms.TextBox _passwd;
		private System.Windows.Forms.TextBox _url;
		private System.Windows.Forms.TextBox _user;
		private System.Windows.Forms.TextBox _key;
		private System.Windows.Forms.TextBox _domin;
		private System.Windows.Forms.TextBox _caption;
		private System.Windows.Forms.TextBox _group;
        private Button button3;

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;

		public AddSite()
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
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this._id = new System.Windows.Forms.TextBox();
            this._passwd = new System.Windows.Forms.TextBox();
            this._url = new System.Windows.Forms.TextBox();
            this._user = new System.Windows.Forms.TextBox();
            this._key = new System.Windows.Forms.TextBox();
            this._domin = new System.Windows.Forms.TextBox();
            this._caption = new System.Windows.Forms.TextBox();
            this._group = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(21, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 31);
            this.label2.TabIndex = 1;
            this.label2.Text = "Url:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(21, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "User:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(228, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 21);
            this.label4.TabIndex = 3;
            this.label4.Text = "PassWD:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(21, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Key:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(21, 159);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 21);
            this.label6.TabIndex = 5;
            this.label6.Text = "Domin:";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(21, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 31);
            this.label7.TabIndex = 6;
            this.label7.Text = "Caption:";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(21, 229);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 31);
            this.label8.TabIndex = 7;
            this.label8.Text = "Group:";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(221, 264);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 38);
            this.button1.TabIndex = 8;
            this.button1.Text = "OK!";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(348, 264);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 38);
            this.button2.TabIndex = 9;
            this.button2.Text = "Cancel";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // _id
            // 
            this._id.Location = new System.Drawing.Point(96, 21);
            this._id.Name = "_id";
            this._id.Size = new System.Drawing.Size(352, 25);
            this._id.TabIndex = 10;
            // 
            // _passwd
            // 
            this._passwd.Location = new System.Drawing.Point(320, 90);
            this._passwd.Name = "_passwd";
            this._passwd.PasswordChar = '*';
            this._passwd.Size = new System.Drawing.Size(128, 25);
            this._passwd.TabIndex = 11;
            // 
            // _url
            // 
            this._url.Location = new System.Drawing.Point(96, 55);
            this._url.Name = "_url";
            this._url.Size = new System.Drawing.Size(352, 25);
            this._url.TabIndex = 12;
            this._url.Text = "http://";
            // 
            // _user
            // 
            this._user.Location = new System.Drawing.Point(96, 90);
            this._user.Name = "_user";
            this._user.Size = new System.Drawing.Size(117, 25);
            this._user.TabIndex = 13;
            // 
            // _key
            // 
            this._key.Location = new System.Drawing.Point(96, 125);
            this._key.Name = "_key";
            this._key.Size = new System.Drawing.Size(352, 25);
            this._key.TabIndex = 14;
            // 
            // _domin
            // 
            this._domin.Location = new System.Drawing.Point(96, 159);
            this._domin.Name = "_domin";
            this._domin.Size = new System.Drawing.Size(352, 25);
            this._domin.TabIndex = 15;
            // 
            // _caption
            // 
            this._caption.Location = new System.Drawing.Point(96, 194);
            this._caption.Name = "_caption";
            this._caption.Size = new System.Drawing.Size(352, 25);
            this._caption.TabIndex = 16;
            // 
            // _group
            // 
            this._group.Location = new System.Drawing.Point(96, 229);
            this._group.Name = "_group";
            this._group.Size = new System.Drawing.Size(352, 25);
            this._group.TabIndex = 17;
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(24, 264);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(172, 38);
            this.button3.TabIndex = 18;
            this.button3.Text = "导入配置";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // AddSite
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 18);
            this.ClientSize = new System.Drawing.Size(480, 311);
            this.Controls.Add(this.button3);
            this.Controls.Add(this._group);
            this.Controls.Add(this._caption);
            this.Controls.Add(this._domin);
            this.Controls.Add(this._key);
            this.Controls.Add(this._user);
            this.Controls.Add(this._url);
            this.Controls.Add(this._passwd);
            this.Controls.Add(this._id);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddSite";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddSite";
            this.Load += new System.EventHandler(this.AddSite_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
        private void ApplyLanguage()
        {
            this.Text = res.AddSite.GetString("_this");
            label1.Text = res.AddSite.GetString("label1");
            label2.Text = res.AddSite.GetString("label2");
            label3.Text = res.AddSite.GetString("label3");
            label4.Text = res.AddSite.GetString("label4");
            label5.Text = res.AddSite.GetString("label5");
            label6.Text = res.AddSite.GetString("label6");
            label7.Text = res.AddSite.GetString("label7");
            label8.Text = res.AddSite.GetString("label8");
            button1.Text = res.AddSite.GetString("button1");
            button2.Text = res.AddSite.GetString("button2");
            button3.Text = res.AddSite.GetString("importcfg");
        }
		private void button2_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			string id=_id.Text.Trim();
			string url=_url.Text;
			string user=_user.Text;
			string passwd=_passwd.Text;
			string key=_key.Text;
			string domin=_domin.Text;
			string caption=_caption.Text;
			string groupname=_group.Text;
			if(id.Equals(""))
			{
                MsgBox.Warning(res.AddSite.GetString("m1"));
				_id.Focus();
				return;
			}
			string sql="select count(id) as c from sites where id='" + id + "'";
			if(globalConst.ConfigConn.GetInt32(sql)>0)
			{

                MsgBox.Warning(id + res.AddSite.GetString("m2"));
				_id.Focus();
				return;
			}
			dir.CreateDirectory(globalConst.SitesPath + @"\" + id);
            file.Copy(globalConst.defaultCssFile, globalConst.SitesPath + @"\" + id + @"\default.css");
			file.Copy(globalConst.ConfigPath + @"\" + "empty.db",globalConst.ConfigPath + @"\" + "site_" + id + ".db",true);
			sql="insert into sites(id,domin,caption,username,passwd,cdkey,version,url,homepage,groupname)values('" + id.Replace("'","''") + "','"+ domin.Replace("'","''") + "','"+ caption.Replace("'","''") + "','" + user.Replace("'","''") + "','" + passwd.Replace("'","''") + "','" + key.Replace("'","''") + "',0,'" + url.Replace("'","''") + "','','"+ groupname.Replace("'","''") + "')";
			globalConst.ConfigConn.execSql(sql);
			MsgBox.Information(res.AddSite.GetString("m3"));
			AddSuc=true;
			this.Close();
		}

        private void AddSite_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Site Config File(*.sitecfg)|*.sitecfg";
            openFileDialog.ShowDialog();
            if (!string.IsNullOrWhiteSpace(openFileDialog.FileName))
            {
                string filename = openFileDialog.FileName;
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                string siteid = doc.SelectSingleNode("site/id").InnerText;
                string sql = "select count(*) from sites where id='" + str.Dot2DotDot(siteid) + "'";
                if(globalConst.ConfigConn.GetInt32(sql)>0)
                {
                    MsgBox.Warning(siteid + res.AddSite.GetString("m2"));
                }
                else
                {
                    string domin = doc.SelectSingleNode("site/domin").InnerText;
                    string caption = doc.SelectSingleNode("site/caption").InnerText;
                    string username = doc.SelectSingleNode("site/username").InnerText;
                    string passwd = doc.SelectSingleNode("site/passwd").InnerText;
                    string cdkey = doc.SelectSingleNode("site/cdkey").InnerText;
                    string version = doc.SelectSingleNode("site/version").InnerText;
                    string url = doc.SelectSingleNode("site/url").InnerText;
                    string homepage = doc.SelectSingleNode("site/homepage").InnerText;
                    string groupname = doc.SelectSingleNode("site/groupname").InnerText;
                    string conntype = doc.SelectSingleNode("site/conntype").InnerText;
                    string conntypeplat = doc.SelectSingleNode("site/conntypeplat").InnerText;
                    string conn = doc.SelectSingleNode("site/conn").InnerText;
                    string connplat = doc.SelectSingleNode("site/connplat").InnerText;
                    sql = "insert into sites(id,domin,caption,username,passwd,cdkey,version,url,homepage,groupname)";
                    sql += "values('"+ siteid + "','"+ domin + "','" + caption + "','" + username + "','" + passwd + "','" + cdkey + "','" + version + "','" + url + "','" + homepage + "','" + groupname + "')";
                    globalConst.ConfigConn.execSql(sql);
                    if(Options.GetSystemDBSetConnStr(siteid)==null)
                    {
                        string s = "";
                        if(!string.IsNullOrWhiteSpace(conntype) && !string.IsNullOrWhiteSpace(conn))
                        {
                            s += "###" + siteid + "|||" + conntype + "|||" + conn;
                        }
                        if (!string.IsNullOrWhiteSpace(conntypeplat) && !string.IsNullOrWhiteSpace(connplat))
                        {
                            s += "###" + siteid + "_PLAT|||" + conntypeplat + "|||" + connplat;
                        }
                        if(s!="")
                        {
                            sql = "update system set thevalue=thevalue || '"+str.Dot2DotDot(s)+"' where name='mysql'";
                            globalConst.ConfigConn.execSql(sql);
                        }
                    }
                    dir.CreateDirectory(globalConst.SitesPath + @"\" + siteid);
                    file.Copy(globalConst.defaultCssFile, globalConst.SitesPath + @"\" + siteid + @"\default.css");
                    file.Copy(globalConst.ConfigPath + @"\" + "empty.db", globalConst.ConfigPath + @"\" + "site_" + siteid + ".db", true);
                    MsgBox.Information(res.AddSite.GetString("m3"));
                    AddSuc = true;
                    this.Close();
                }
            }
        }
    }
}
