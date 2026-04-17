using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FTDPClient.functions;
using FTDPClient.consts;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FTDPClient.forms
{
	/// <summary>
	/// addDir 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class AddDir : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.TextBox filename;
		private System.Windows.Forms.TextBox caption;
		private string path="";
		private string pid="";
		public bool isEdit=false;
		public string _filename="";
		public string _caption="";
		public bool cancel=true;
        private Button button1;
        private TextBox textBox1;
        private Label label3;

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;

		public AddDir()
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
            this.filename = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.caption = new System.Windows.Forms.TextBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // filename
            // 
            this.filename.Font = new System.Drawing.Font("宋体", 12F);
            this.filename.Location = new System.Drawing.Point(101, 32);
            this.filename.Name = "filename";
            this.filename.Size = new System.Drawing.Size(236, 26);
            this.filename.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(34, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "filename";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(34, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "caption";
            // 
            // caption
            // 
            this.caption.Font = new System.Drawing.Font("宋体", 12F);
            this.caption.Location = new System.Drawing.Point(101, 77);
            this.caption.Name = "caption";
            this.caption.Size = new System.Drawing.Size(236, 26);
            this.caption.TabIndex = 3;
            // 
            // Cancel
            // 
            this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Cancel.Location = new System.Drawing.Point(262, 163);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 30);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "button1";
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // OK
            // 
            this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OK.Location = new System.Drawing.Point(101, 163);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 30);
            this.OK.TabIndex = 5;
            this.OK.Text = "确定";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(262, 114);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 30);
            this.button1.TabIndex = 11;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox1.Location = new System.Drawing.Point(101, 119);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(158, 26);
            this.textBox1.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "label3";
            // 
            // AddDir
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(393, 223);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.caption);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.filename);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddDir";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddDir";
            this.Load += new System.EventHandler(this.AddDir_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
        private void ApplyLanguage()
        {
            this.Text = res.AddDir.GetString("_this");
            label1.Text = res.AddDir.GetString("label1");
            label2.Text = res.AddDir.GetString("label2");
            OK.Text = res.AddDir.GetString("OK");
            Cancel.Text = res.AddDir.GetString("Cancel");
            label3.Text = res.AddPage.GetString("String1");
            button1.Text = res.AddPage.GetString("String2");
        }
		private void Cancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void AddDir_Load(object sender, System.EventArgs e)
		{
			if(!isEdit)
			{
				if(globalConst.MdiForm.SiteTree.SelectedNode==null)
				{
					globalConst.MdiForm.SiteTree.SelectedNode=globalConst.MdiForm.SiteTree.Nodes[0];
				}
				if(tree.getTypeFromID(globalConst.MdiForm.SiteTree.SelectedNode).Equals("page"))
				{
					globalConst.MdiForm.SiteTree.SelectedNode=globalConst.MdiForm.SiteTree.SelectedNode.Parent;
				}
                if (tree.getTypeFromID(globalConst.MdiForm.SiteTree.SelectedNode).Equals("part"))
                {
                    globalConst.MdiForm.SiteTree.SelectedNode = globalConst.MdiForm.SiteTree.SelectedNode.Parent.Parent;
                }
				pid=((string[])globalConst.MdiForm.SiteTree.SelectedNode.Tag)[2];	
				path=globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode);
				int i=1;
				while(dir.Exists(path + @"\new_dir_" + i))i++;
				filename.Text="new_dir_" + i;
                caption.Text = res.AddDir.GetString("c1") + i;
			}
			else
			{
				path=globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode);
				filename.Text=_filename;
				caption.Text=_caption;
			}
		}

		private void OK_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(filename.Text.Trim().Equals(""))
				{
					new MsgBox(res.AddDir.GetString("m1"));
					return;
				}
				if(caption.Text.Trim().Equals(""))
				{
					new MsgBox(res.AddDir.GetString("m2"));
					return;
				}
				if(!isEdit)
				{
					string Filename=path + @"\" + filename.Text.Trim();
					if(dir.Exists(Filename))
					{
						new MsgBox(res.AddDir.GetString("m3"));
						return;
                    }
                    new dir(Filename);
                    string Filecaption=caption.Text.Trim();
					string rdmid=rdm.getID() + "_drct";
					string sql="insert into directory(id,pid,name,caption,updatetime)values('" + rdmid + "','" + pid + "','" + filename.Text.Trim() + "','" + Filecaption + "','" + DateTime.Now + "')";
					globalConst.CurSite.SiteConn.execSql(sql);
					TreeNode nnd=new TreeNode();
					string[] tag=new string[3];
					tag[0]=filename.Text.Trim();
					tag[1]=Filecaption;
					tag[2]=rdmid;
					nnd.Tag=tag;
					nnd.ImageIndex=17;
					nnd.SelectedImageIndex=17;
					if(globalConst.siteTreeShowColName.EndsWith("name"))
					{
						nnd.Text=tag[0];
					}
					else
					{
						nnd.Text=tag[1];
					}
					//复制整个目录结构及关联组件
					if (!string.IsNullOrEmpty(CopyedDirId))
					{
                        var dirS = new System.IO.DirectoryInfo((globalConst.CurSite.Path + textBox1.Text.Trim()).Replace("/","\\"));
                        var dirT = new System.IO.DirectoryInfo(Filename.Replace("/", "\\"));
                        if (dirS.FullName.StartsWith(dirT.FullName) || dirT.FullName.StartsWith(dirS.FullName))
                        {
                            new MsgBox("Directory Not Correct");
                            return;
                        }

                        LoopCopy(rdmid, CopyedDirId);
                        //文件夹复制
                        dir.Copy(dirS, dirT, null, null, true);
                        //Tree刷新
                        globalConst.MdiForm.refreshSiteTree();
                        MsgBox.Information("Copy Directory,Page,Control,Part Successfully !");
                    }
					else
                    {
                        globalConst.MdiForm.SiteTree.SelectedNode.Nodes.Add(nnd);
						globalConst.MdiForm.SiteTree.SelectedNode.Expand();
						globalConst.MdiForm.SiteTree.SelectedNode = nnd;
					}
				}
				else
				{
					string id=tree.getID(globalConst.MdiForm.SiteTree.SelectedNode);
					string Filename=globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode.Parent) + @"\" + filename.Text.Trim();
					string Filecaption=caption.Text.Trim();
					if(!path.Equals(Filename))
					{
						if(dir.Exists(Filename))
						{
                            MsgBox.Warning(res.AddDir.GetString("m4"));
							return;
						}
						dir.Move(path,Filename);
					}
					string sql="update directory set name='" + filename.Text.Trim() + "',caption='" + Filecaption + "',updatetime='" +  DateTime.Now + "' where id='" + id + "'";
					globalConst.CurSite.SiteConn.execSql(sql);
					string[] tag=new string[3];
					tag[0]=filename.Text.Trim();
					tag[1]=Filecaption;
					tag[2]=id;
					globalConst.MdiForm.SiteTree.SelectedNode.Tag=tag;
					if(globalConst.siteTreeShowColName.EndsWith("name"))
					{
						globalConst.MdiForm.SiteTree.SelectedNode.Text=tag[0];
					}
					else
					{
						globalConst.MdiForm.SiteTree.SelectedNode.Text=tag[1];
					}
				}
				this.Close();
			}
			catch(Exception ex)
			{
	new error(ex);
			}
		}
		string CopyedDirId=null;
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            GetNode gn = new GetNode();
            gn.IsJustSelectDir = true;
            gn.ReturnURL = textBox1.Text;
            gn.ShowDialog();
            CopyedDirId = gn.PageId;
            textBox1.Text = gn.ReturnURL;
            gn = null;
            button1.Enabled = true;
        }
		private void LoopCopy(string NewDirId,string OriDirId)
		{
            string sql = "select id,pid,name,caption,updatetime,homepage from directory where pid='" + str.D2DD(OriDirId) + "'";
            var listDir = new List<string[]>();
            using (var dr = globalConst.CurSite.SiteConn.OpenRecord(sql))
            {
                while (dr.Read())
                {
                    var oiL = dr.FieldCount;
                    var item = new string[oiL];
                    for (var i = 0; i < oiL; i++) item[i] = dr.IsDBNull(i) ? "" : dr.GetString(i);
                    listDir.Add(item);
                }
            }
            sql = "select id,pid,name,caption,updatetime,ptype,mtype,jinfo,jurl,fid,modopen,viewopen,datastr,paraname,membind,elecdt,roledata,rolesession,authrule,flowstat,norightinfo,norighturl,a_ip_s,a_ip_c,a_ip_o,a_se_s,a_se_c,a_se_o,a_jp_s,a_jp_u,a_tp_s,a_tp_c,pagejs,fore_frame,out_type from pages where pid='" + str.D2DD(OriDirId) + "'";
            var listPage = new List<string[]>();
            using (var dr = globalConst.CurSite.SiteConn.OpenRecord(sql))
            {
                while (dr.Read())
                {
                    var oiL = dr.FieldCount;
                    var item = new string[oiL];
                    for (var i = 0; i < oiL; i++) item[i] = dr.IsDBNull(i) ? "" : dr.GetValue(i).ToString();
                    listPage.Add(item);
                }
            }
            foreach (var item in listDir)
            {
                var newDirId = rdm.getID() + "_drct";
                StringBuilder sb = new StringBuilder();
                for (var i = 2; i < item.Length; i++) sb.Append(",'" + str.D2DD(item[i]) + "'");
                sql = "insert into directory(id,pid,name,caption,updatetime,homepage)values('" + newDirId + "','" + NewDirId + "'" + sb.ToString() + ")";
                globalConst.CurSite.SiteConn.execSql(sql);
				LoopCopy(newDirId, item[0]);
            }
            foreach (var item in listPage)
            {
                var newPageId = rdm.getID() + "_page";
                StringBuilder sb = new StringBuilder();
                for (var i = 2; i < item.Length; i++) sb.Append(",'" + str.D2DD(item[i]) + "'");
                sql = "insert into pages(id,pid,name,caption,updatetime,ptype,mtype,jinfo,jurl,fid,modopen,viewopen,datastr,paraname,membind,elecdt,roledata,rolesession,authrule,flowstat,norightinfo,norighturl,a_ip_s,a_ip_c,a_ip_o,a_se_s,a_se_c,a_se_o,a_jp_s,a_jp_u,a_tp_s,a_tp_c,pagejs,fore_frame,out_type)values('" + newPageId + "','" + NewDirId + "'" + sb.ToString() + ")";
                globalConst.CurSite.SiteConn.execSql(sql);
                CopyParts(newPageId, item[0]);
            }
        }
        private void CopyParts(string NewPageId,string OriPageId)
        {
            string sql = "select partid from part_in_page where pageid='" + str.D2DD(OriPageId) + "'";
            var Parts = new List<string>();
            using (var dr = globalConst.CurSite.SiteConn.OpenRecord(sql))
            {
                while (dr.Read())
                {
                    Parts.Add(dr.GetString(0));
                }
            }
            var listParts = new List<string[]>();
            var listControlIds = new List<string>();
            foreach (var partid in Parts)
            {
                sql = "select id,name,controlid,partxml,asportal,a_ip_s,a_ip_c,a_ip_o,a_se_s,a_se_c,a_se_o,a_jp_s,a_jp_u,a_tp_s,a_tp_c from parts where id='" + str.D2DD(partid) + "'";
                using (var dr = globalConst.CurSite.SiteConn.OpenRecord(sql))
                {
                    while (dr.Read())
                    {

                        var oiL = dr.FieldCount;
                        var item = new string[oiL];
                        for (var i = 0; i < oiL; i++) item[i] = dr.IsDBNull(i) ? "" : dr.GetValue(i).ToString();
                        listParts.Add(item);
                        if (!listControlIds.Contains(item[2])) listControlIds.Add(item[2]);
                    }
                }
            }
            var listControls = new List<string[]>();
            foreach (var controlid in listControlIds)
            {
                sql = "select id,name,caption,datasource,shared,paras from controls where id='" + str.D2DD(controlid) + "'";
                using (var dr = globalConst.CurSite.SiteConn.OpenRecord(sql))
                {
                    while (dr.Read())
                    {
                        var oiL = dr.FieldCount;
                        var item = new string[oiL];
                        for (var i = 0; i < oiL; i++) item[i] = dr.IsDBNull(i) ? "" : dr.GetString(i);
                        listControls.Add(item);
                    }
                }
            }
            foreach (var item in listControls)
            {
                var newCtrlId = rdm.getID() + "_ctrl";
                StringBuilder sb = new StringBuilder();
                for (var i = 1; i < item.Length; i++) sb.Append(",'" + str.D2DD(item[i]) + "'");
                sql = "insert into controls(id,name,caption,datasource,shared,paras)values('" + newCtrlId + "'" + sb.ToString() + ")";
                globalConst.CurSite.SiteConn.execSql(sql);
                var oldCtrlId = item[0];
                foreach(var partItem in listParts.Where(r => r[2]== oldCtrlId))
                {
                    var newPartId = rdm.getID() + "_part";
                    StringBuilder sb2 = new StringBuilder();
                    for (var i = 3; i < partItem.Length; i++) sb2.Append(",'" + str.D2DD(partItem[i]) + "'");
                    sql = "insert into parts(id,name,controlid,partxml,asportal,a_ip_s,a_ip_c,a_ip_o,a_se_s,a_se_c,a_se_o,a_jp_s,a_jp_u,a_tp_s,a_tp_c)values('" + newPartId + "','" + str.D2DD(partItem[1]) + "','" + newCtrlId + "'" + sb2.ToString() + ")";
                    globalConst.CurSite.SiteConn.execSql(sql);
                    var oldPartId = partItem[0];
                    if(Parts.Contains(oldPartId))
                    {
                        sql = "insert into part_in_page(pageid,partid)values('"+NewPageId+"','"+ newPartId + "')";
                        globalConst.CurSite.SiteConn.execSql(sql);
                    }
                }
            }
        }
    }
}
