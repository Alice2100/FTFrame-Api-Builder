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
namespace SiteMatrix.forms
{
	/// <summary>
	/// SiteList 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
	public class SiteList : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.ListView lv;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label Domin;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label Caption;
        private Label URL;
        private Label Group;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SiteList()
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
            this.lv = new System.Windows.Forms.ListView();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Domin = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Caption = new System.Windows.Forms.Label();
            this.URL = new System.Windows.Forms.Label();
            this.Group = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lv
            // 
            this.lv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.lv.ContextMenu = this.contextMenu1;
            this.lv.ForeColor = System.Drawing.SystemColors.InfoText;
            this.lv.Location = new System.Drawing.Point(8, 8);
            this.lv.MultiSelect = false;
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(280, 240);
            this.lv.TabIndex = 0;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.DoubleClick += new System.EventHandler(this.lv_DoubleClick);
            this.lv.SelectedIndexChanged += new System.EventHandler(this.lv_SelectedIndexChanged);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem3,
            this.menuItem4});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "LargeIcon";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "SmallIcon";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "List";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(382, 262);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 24);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cancel";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(89, 262);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 24);
            this.button3.TabIndex = 3;
            this.button3.Text = "注册";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(294, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "[Domin]";
            this.label1.Visible = false;
            // 
            // Domin
            // 
            this.Domin.Location = new System.Drawing.Point(294, 24);
            this.Domin.Name = "Domin";
            this.Domin.Size = new System.Drawing.Size(169, 24);
            this.Domin.TabIndex = 5;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(251, 262);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 24);
            this.button4.TabIndex = 6;
            this.button4.Text = "删除";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(170, 262);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 24);
            this.button5.TabIndex = 7;
            this.button5.Text = "同步";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(8, 262);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 24);
            this.button6.TabIndex = 8;
            this.button6.Text = "添加";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(294, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "[Caption]";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(294, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "[URL]";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(294, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "[Group]";
            // 
            // Caption
            // 
            this.Caption.Location = new System.Drawing.Point(294, 80);
            this.Caption.Name = "Caption";
            this.Caption.Size = new System.Drawing.Size(169, 24);
            this.Caption.TabIndex = 12;
            // 
            // URL
            // 
            this.URL.Location = new System.Drawing.Point(294, 145);
            this.URL.Name = "URL";
            this.URL.Size = new System.Drawing.Size(169, 24);
            this.URL.TabIndex = 13;
            // 
            // Group
            // 
            this.Group.Location = new System.Drawing.Point(294, 207);
            this.Group.Name = "Group";
            this.Group.Size = new System.Drawing.Size(169, 24);
            this.Group.TabIndex = 14;
            // 
            // SiteList
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(469, 298);
            this.Controls.Add(this.Group);
            this.Controls.Add(this.URL);
            this.Controls.Add(this.Caption);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.Domin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lv);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SiteList";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SiteList";
            this.Load += new System.EventHandler(this.SiteList_Load);
            this.ResumeLayout(false);

		}
		#endregion
        private void ApplyLanguage()
        {
            this.Text=res.SiteList.GetString("_this");
            label1.Text = res.SiteList.GetString("label1");
            label2.Text = res.SiteList.GetString("label2");
            label3.Text = res.SiteList.GetString("label3");
            label4.Text = res.SiteList.GetString("label4");
            button6.Text = res.SiteList.GetString("button6");
            button3.Text = res.SiteList.GetString("button3");
            button5.Text = res.SiteList.GetString("button5");
            button4.Text = res.SiteList.GetString("button4");
            button1.Text = res.SiteList.GetString("button1");
        }
		private void SiteList_Load(object sender, System.EventArgs e)
		{
			init();
		}
		private void init()
		{
			lv.Items.Clear();
			label1.Visible=true;
			Domin.Text="";
			string sql="select * from sites";
			OleDbDataReader rdr= globalConst.ConfigConn.OpenRecord(sql);
			while(rdr.Read())
			{
				ListViewItem lvi=new ListViewItem(rdr["id"].ToString());
				string[] tagi=new string[9];
				tagi[0]=rdr["domin"].ToString();
				tagi[1]=rdr["caption"].ToString();
				tagi[2]=rdr["username"].ToString();
				tagi[3]=rdr["passwd"].ToString();
				tagi[4]=rdr["cdkey"].ToString();
				tagi[5]=rdr["version"].ToString();
				tagi[6]=rdr["url"].ToString();
				tagi[7]=rdr["homepage"].ToString();
				tagi[8]=rdr["groupname"].ToString();
				lvi.Tag=tagi;
				lv.Items.Add(lvi);
			}
			rdr.Close();
		}
		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void lv_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(lv.SelectedItems.Count<1)return;
			label1.Visible=true;
			string[] tagi=(string[])lv.SelectedItems[0].Tag;
			Domin.Text=tagi[0];
            Caption.Text = tagi[1];
            URL.Text = tagi[6];
            Group.Text = tagi[8];
		}

		private void lv_DoubleClick(object sender, System.EventArgs e)
		{
		string siteid=lv.SelectedItems[0].Text;
			if(globalConst.CurSite.ID!=null && globalConst.CurSite.ID.Equals(siteid))
			{
				MsgBox.Warning(siteid + res.SiteList.GetString("m1"));
				return;
			}
		Cursor.Current=Cursors.WaitCursor;
		SiteClass.Site.open(siteid);
		this.Close();
		}


		private void button3_Click(object sender, System.EventArgs e)
		{
			SiteAdd sa=new SiteAdd();
			sa.ShowDialog();
			if(sa.isAddSuccess)
			{
				init();
			}
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			lv.View=View.LargeIcon;
		}


		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			lv.View=View.SmallIcon;
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			lv.View=View.List;
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			if(lv.SelectedItems.Count<1)return;
			string siteid=lv.SelectedItems[0].Text;
            if (MsgBox.OKCancel(res.SiteList.GetString("m2") + " [" + siteid + "] ?").Equals(DialogResult.OK))
			{
				if(globalConst.CurSite.ID!=null && globalConst.CurSite.ID.Equals(siteid))
				{
                    MsgBox.Warning(siteid + res.SiteList.GetString("m3"));
				return;
				}
				dir.Delete(globalConst.SitesPath + @"\" + siteid,true);
				file.Delete(globalConst.ConfigPath + @"\" + "site_" + siteid + ".db");
				string sql="delete from sites where id='" + siteid + "'";
				globalConst.ConfigConn.execSql(sql);
				init();
			}
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			if(lv.SelectedItems.Count<1)return;
			string siteid=lv.SelectedItems[0].Text;
            if (MsgBox.OKCancel(res.SiteList.GetString("m4")).Equals(DialogResult.OK))
			{
				if(globalConst.CurSite.ID!=null && globalConst.CurSite.ID.Equals(siteid))
				{
                    MsgBox.Warning(siteid + res.SiteList.GetString("m5"));
					return;
				}
				SiteUpdate su=new SiteUpdate();
				su.siteid=siteid;
				su.ShowDialog();
			}
		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			AddSite sa=new AddSite();
			sa.ShowDialog();
			if(sa.AddSuc)
			{
				init();
			}
		}
	}
}
