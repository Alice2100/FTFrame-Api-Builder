using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SiteMatrix.functions;
using SiteMatrix.consts;
namespace SiteMatrix.forms
{
	/// <summary>
	/// AddSite 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
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
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AddSite()
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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Url:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "User:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(186, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "PassWD:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Key:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "Domin:";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(16, 151);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 24);
            this.label7.TabIndex = 6;
            this.label7.Text = "Caption:";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(16, 178);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 24);
            this.label8.TabIndex = 7;
            this.label8.Text = "Group:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(85, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 24);
            this.button1.TabIndex = 8;
            this.button1.Text = "OK!";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(216, 205);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 24);
            this.button2.TabIndex = 9;
            this.button2.Text = "Cancel";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // _id
            // 
            this._id.Location = new System.Drawing.Point(72, 16);
            this._id.Name = "_id";
            this._id.Size = new System.Drawing.Size(264, 21);
            this._id.TabIndex = 10;
            // 
            // _passwd
            // 
            this._passwd.Location = new System.Drawing.Point(240, 70);
            this._passwd.Name = "_passwd";
            this._passwd.PasswordChar = '*';
            this._passwd.Size = new System.Drawing.Size(96, 21);
            this._passwd.TabIndex = 11;
            // 
            // _url
            // 
            this._url.Location = new System.Drawing.Point(72, 43);
            this._url.Name = "_url";
            this._url.Size = new System.Drawing.Size(264, 21);
            this._url.TabIndex = 12;
            this._url.Text = "http://";
            // 
            // _user
            // 
            this._user.Location = new System.Drawing.Point(72, 70);
            this._user.Name = "_user";
            this._user.Size = new System.Drawing.Size(88, 21);
            this._user.TabIndex = 13;
            // 
            // _key
            // 
            this._key.Location = new System.Drawing.Point(72, 97);
            this._key.Name = "_key";
            this._key.Size = new System.Drawing.Size(264, 21);
            this._key.TabIndex = 14;
            // 
            // _domin
            // 
            this._domin.Location = new System.Drawing.Point(72, 124);
            this._domin.Name = "_domin";
            this._domin.Size = new System.Drawing.Size(264, 21);
            this._domin.TabIndex = 15;
            // 
            // _caption
            // 
            this._caption.Location = new System.Drawing.Point(72, 151);
            this._caption.Name = "_caption";
            this._caption.Size = new System.Drawing.Size(264, 21);
            this._caption.TabIndex = 16;
            // 
            // _group
            // 
            this._group.Location = new System.Drawing.Point(72, 178);
            this._group.Name = "_group";
            this._group.Size = new System.Drawing.Size(264, 21);
            this._group.TabIndex = 17;
            // 
            // AddSite
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(362, 241);
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
	}
}
