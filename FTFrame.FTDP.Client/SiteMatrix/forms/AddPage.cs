using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FTDPClient.functions;
using FTDPClient.consts;
using Microsoft.Data.Sqlite;
using DocumentFormat.OpenXml.Wordprocessing;
using FTDPClient.Page;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;

namespace FTDPClient.forms
{
	/// <summary>
	/// addDir 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class AddPage : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.TextBox filename;
		private System.Windows.Forms.TextBox caption;
        public TreeView CustomTree = null;
        public string CustomFileName = null;
		private string path="";
		private string pid="";
		public bool isEdit=false;
		public string _filename="";
		public string _caption="";
		public bool cancel=true;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;
        private Label label3;
        private TextBox textBox1;
        private Button button1;
        private Button button2;
        private Label label4;

        //0:normal;1:formpage;2:formpage manager
        public int PageType = 0;
        //public PageOutType pageOutType=PageOutType.Page;
		public AddPage()
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
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // filename
            // 
            this.filename.Font = new System.Drawing.Font("宋体", 12F);
            this.filename.Location = new System.Drawing.Point(100, 12);
            this.filename.Name = "filename";
            this.filename.Size = new System.Drawing.Size(238, 26);
            this.filename.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(34, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "File Name";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(34, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "caption";
            // 
            // caption
            // 
            this.caption.Font = new System.Drawing.Font("宋体", 12F);
            this.caption.Location = new System.Drawing.Point(100, 44);
            this.caption.Name = "caption";
            this.caption.Size = new System.Drawing.Size(238, 26);
            this.caption.TabIndex = 3;
            // 
            // Cancel
            // 
            this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Cancel.Location = new System.Drawing.Point(263, 149);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 30);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "button1";
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // OK
            // 
            this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OK.Location = new System.Drawing.Point(100, 149);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 30);
            this.OK.TabIndex = 5;
            this.OK.Text = "确定";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "label3";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox1.Location = new System.Drawing.Point(100, 79);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(157, 26);
            this.textBox1.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(263, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 30);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(100, 112);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(238, 30);
            this.button2.TabIndex = 11;
            this.button2.Text = "Build Single Table Components";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(98, 187);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "label4";
            // 
            // AddPage
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(403, 207);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
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
            this.Name = "AddPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddPage";
            this.Load += new System.EventHandler(this.AddDir_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
        private void ApplyLanguage()
        {
            this.Text = res.AddPage.GetString("_this");
            label1.Text = res.AddPage.GetString("_this");
            label2.Text = res.AddPage.GetString("label2");
            OK.Text = res.AddPage.GetString("OK");
            Cancel.Text = res.AddPage.GetString("Cancel");
            label3.Text = res.AddPage.GetString("String1");
            button1.Text = res.AddPage.GetString("String2");
            button2.Text = res.AddPage.GetString("String4");
        }

		private void Cancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void AddDir_Load(object sender, System.EventArgs e)
		{
            if (CustomTree == null)
            {
                if (!isEdit)
                {
                    if (globalConst.MdiForm.SiteTree.SelectedNode == null)
                    {
                        globalConst.MdiForm.SiteTree.SelectedNode = globalConst.MdiForm.SiteTree.Nodes[0];
                    }
                    if (tree.getTypeFromID(globalConst.MdiForm.SiteTree.SelectedNode).Equals("page"))
                    {
                        globalConst.MdiForm.SiteTree.SelectedNode = globalConst.MdiForm.SiteTree.SelectedNode.Parent;
                    }
                    if (tree.getTypeFromID(globalConst.MdiForm.SiteTree.SelectedNode).Equals("part"))
                    {
                        globalConst.MdiForm.SiteTree.SelectedNode = globalConst.MdiForm.SiteTree.SelectedNode.Parent.Parent;
                    }
                    pid = ((string[])globalConst.MdiForm.SiteTree.SelectedNode.Tag)[2];
                    path = globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode);
                    int i = 1;
                    while (file.Exists(path + @"\new_page_" + i)) i++;
                    filename.Text = "new_page_" + i;
                    caption.Text = res.AddPage.GetString("c1") + i;
                }
                else
                {
                    path = globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode);
                    filename.Text = _filename;
                    caption.Text = _caption;
                }
            }
            else
            {
                pid = ((string[])CustomTree.SelectedNode.Tag)[2];
                path = globalConst.CurSite.Path + tree.getPath(CustomTree.SelectedNode);
                int i = 1;
                while (file.Exists(path + @"\new_page_" + i)) i++;
                filename.Text = "new_page_" + i;
                caption.Text = res.AddPage.GetString("c1") + i;
            }
            label4.Text = "";// "gadad_dadaf_dadadad"+Environment.NewLine+"发大大大大发大大大";
		}

		private void OK_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(filename.Text.Trim().Equals(""))
				{
					new MsgBox(res.AddPage.GetString("m1"));
					return;
				}
				if(caption.Text.Trim().Equals(""))
				{
					new MsgBox(res.AddPage.GetString("m2"));
					return;
				}
				if(!isEdit)
				{
				string Filename=path + @"\" + filename.Text.Trim();
				if(file.Exists(Filename))
				{
					new MsgBox(res.AddPage.GetString("m3"));
					return;
				}
				string Filecaption=caption.Text.Trim();

                //复制现有页面
                string srcfilename = textBox1.Text.Trim();
                System.Text.Encoding CopyFileTextEncoding = System.Text.Encoding.Default;
                if (!srcfilename.Equals(""))
                {
                    srcfilename=srcfilename.Replace("/",@"\");
                    if (srcfilename.StartsWith(@"\"))
                    {
                        srcfilename = globalConst.CurSite.Path + srcfilename;
                    }
                    if (file.Exists(srcfilename))
                    {
                        CustomFileName = srcfilename;
                        CopyFileTextEncoding = System.Text.Encoding.UTF8;
                    }
                    else
                    {
                        MsgBox.Error(res.AddPage.GetString("String3"));
                        return;
                    }
                    }
               string rdmid = rdm.getID() + "_page";
                    //file.Copy(globalConst.emptyFile,Filename);
                    //next 2 line add 2005-7-7,alice,转换为utf-8文档
                    string filetext;
                if (CustomFileName == null)
                {
                    filetext = file.getFileText(globalConst.emptyFile, System.Text.Encoding.Default);
                    string subPath = "";
                    if (globalConst.MdiForm.SiteTree.SelectedNode != null)
                    {
                        subPath = tree.getSubPath(globalConst.MdiForm.SiteTree.SelectedNode);
                    }
                    filetext = filetext.Replace("New Document", Filecaption+"\r\n").Replace("<!--STYLE-->", "<link id=edit_style_file href=\"" + subPath + "../../style/default.css\" rel=\"stylesheet\" type=\"text/css\">");
                    
                    if(SelTableName!=null)
                        {
                            var controlsHTML = PageWare.SingleTableQuickControl(SelTableName, string.IsNullOrWhiteSpace(SelTableDesc) ? SelTableName : SelTableDesc, rdmid);
                            filetext = filetext.Replace("<BODY></BODY>", "<BODY>"+ controlsHTML + "</BODY>");
                        }

                    if (PageType == 1||PageType==2)
                    {
                        filetext = filetext.Replace("<BODY></BODY>", "<BODY style=\"MARGIN: 0px\"><FORM style=\"MARGIN: 0px; WIDTH: 100%; HEIGHT: 100%\" method=\"post\" id=\"ftform_form\" name=\"ftform_form\" encType=\"multipart/form-data\"></FORM></BODY>");
                    }
                }
                else
                {
                    filetext = file.getFileText(CustomFileName, CopyFileTextEncoding);
                }
				file.CreateText(Filename,filetext);
                    string sql, formFrameType="", pageOutType="",pagejs="";
                if (string.IsNullOrEmpty(CopyedPageId))
                    {
                        formFrameType = Options.GetSystemValue("ForeFrameType");
                        pageOutType = Options.GetSystemValue("PageOutType");
                        if (formFrameType == null) formFrameType = ForeFrameType.JQueryUI.ToString();
                        if (pageOutType == null) pageOutType = PageOutType.Page.ToString();
                    }
                else
                    {
                        sql = "select pagejs,fore_frame,out_type from pages where id='"+CopyedPageId+"'";
                        using(SqliteDataReader dr = globalConst.CurSite.SiteConn.OpenRecord(sql))
                        {
                            if(dr.Read())
                            {
                                pagejs = dr.IsDBNull(0) ? "" : dr.GetString(0);
                                formFrameType = dr.IsDBNull(1) ? "" : dr.GetString(1);
                                pageOutType = dr.IsDBNull(2) ? "" : dr.GetString(2);
                            }
                        }
                    }
                sql = "insert into pages(id,pid,name,caption,updatetime,ptype,mtype,jinfo,jurl,fid,modopen,viewopen,datastr,paraname,membind,elecdt,roledata,rolesession,authrule,flowstat,norightinfo,norighturl,a_ip_s,a_ip_c,a_ip_o,a_se_s,a_se_c,a_se_o,a_jp_s,a_jp_u,a_tp_s,a_tp_c,pagejs,fore_frame,out_type)values('" + rdmid + "','" + pid + "','" + filename.Text.Trim() + "','" + Filecaption + "','" + DateTime.Now + "'," + PageType + ",0,'','','',0,0,'','ftformid','','','','asso_id','%m% and %e% and %f% and %s%','','','',0,'',0,0,'',0,0,'',0,'','"+functions.str.Dot2DotDot(pagejs)+"','"+ formFrameType + "','"+ pageOutType + "')";
				globalConst.CurSite.SiteConn.execSql(sql);
				TreeNode nnd=new TreeNode();
				string[] tag=new string[4];
				tag[0]=filename.Text.Trim();
				tag[1]=Filecaption;
				tag[2]=rdmid;
                tag[3] = PageType.ToString();
				nnd.Tag=tag;
                if (pageOutType == PageOutType.Json.ToString())
                {
                    nnd.ImageIndex = 11;
                        nnd.SelectedImageIndex = 11;
                }
                else
                {
                    switch (PageType)
                    {
                        case 0:
                            nnd.ImageIndex = 19;
                            break;
                        case 1:
                            nnd.ImageIndex = 26;
                            break;
                        case 2:
                            nnd.ImageIndex = 27;
                            break;
                    }
                }
                nnd.SelectedImageIndex = nnd.ImageIndex;
				if(globalConst.siteTreeShowColName.EndsWith("name"))
				{
					nnd.Text=tag[0];
				}
				else
				{
					nnd.Text=tag[1];
				}
                    if (CustomTree != null)
                    {
                        TreeNode tn = tree.getSiteNodeByID(tree.getID(CustomTree.SelectedNode));
                        if (tn != null) globalConst.MdiForm.SiteTree.SelectedNode = tn;
                        CustomTree.SelectedNode.Nodes.Add(nnd);
                        CustomTree.SelectedNode.Expand();
                    }
                    if (globalConst.MdiForm.SiteTree.SelectedNode != null)
                    {
                        TreeNode nnd2 = (TreeNode)nnd.Clone();
                        globalConst.MdiForm.SiteTree.SelectedNode.Nodes.Add(nnd2);
                        globalConst.MdiForm.SiteTree.SelectedNode.Expand();
                        globalConst.MdiForm.SiteTree.SelectedNode = nnd2;
                    }
                    FTDPClient.PropertySpace.Site.PropertyPage.pageid =rdmid ;
                    globalConst.MdiForm.PropGrid.SelectedObject = FTDPClient.PropertySpace.Site.PropertyPage.Bag();

                    PageWare.openPage(rdmid, Filecaption);
                }
				else
				{
					string id=tree.getID(globalConst.MdiForm.SiteTree.SelectedNode);
					string Filename=globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode.Parent) + @"\" + filename.Text.Trim();
					string Filecaption=caption.Text.Trim();
					if(!path.Equals(Filename))
					{
						if(file.Exists(Filename))
						{
							MsgBox.Warning(res.AddPage.GetString("m4"));
							return;
						}
						file.Move(path,Filename);
					}
					string sql="update pages set name='" + filename.Text.Trim() + "',caption='" + Filecaption + "',updatetime='" +  DateTime.Now + "' where id='" + id + "'";
					globalConst.CurSite.SiteConn.execSql(sql);
					string[] tag=new string[4];
					tag[0]=filename.Text.Trim();
					tag[1]=Filecaption;
					tag[2]=id;
                    tag[3] = ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[3];
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
        string CopyedPageId = "";
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            GetNode gn = new GetNode();
            gn.IsJustSelectFile = true;
            gn.ReturnURL = textBox1.Text;
            gn.ShowDialog();
            CopyedPageId = gn.PageId;
            textBox1.Text = gn.ReturnURL;
            gn = null;
            button1.Enabled = true;
        }
        string SelTableName = null;
        string SelTableDesc = null;

        private void button2_Click(object sender, EventArgs e)
        {
            string connstr = Options.GetSystemDBSetConnStr();
            var dbtype = Options.GetSystemDBSetType();
            if (dbtype==globalConst.DBType.MySql)
            {
                control.SelTable_MySql sel = new control.SelTable_MySql();
                sel.connstr = connstr;
                sel.ShowDialog();
                if (sel.tablename != null)
                {
                    SelTableName = sel.tablename;
                    SelTableDesc = sel.tabledesc;
                    label4.Text= SelTableName+" "+SelTableDesc;
                }
            }
            else if (dbtype == globalConst.DBType.SqlServer)
            {
                control.SelTable_SqlServer sel = new control.SelTable_SqlServer();
                sel.connstr = connstr;
                sel.ShowDialog();
                if (sel.tablename != null)
                {
                    SelTableName = sel.tablename;
                    SelTableDesc = sel.tabledesc;
                    label4.Text = SelTableName + " " + SelTableDesc;
                }
            }
            else if (dbtype == globalConst.DBType.Sqlite)
            {
                control.SelTable_Sqlite sel = new control.SelTable_Sqlite();
                sel.connstr = connstr;
                sel.ShowDialog();
                if (sel.tablename != null)
                {
                    SelTableName = sel.tablename;
                    SelTableDesc = sel.tabledesc;
                    label4.Text = SelTableName + " " + SelTableDesc;
                }
            }
        }
    }

}
