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
	/// addDir 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
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
        //0:normal;1:formpage;2:formpage manager
        public int PageType = 0;
		public AddPage()
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
            this.filename = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.caption = new System.Windows.Forms.TextBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // filename
            // 
            this.filename.Location = new System.Drawing.Point(100, 12);
            this.filename.Name = "filename";
            this.filename.Size = new System.Drawing.Size(232, 21);
            this.filename.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(34, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "File Name";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(34, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "caption";
            // 
            // caption
            // 
            this.caption.Location = new System.Drawing.Point(100, 44);
            this.caption.Name = "caption";
            this.caption.Size = new System.Drawing.Size(232, 21);
            this.caption.TabIndex = 3;
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(216, 117);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 24);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "button1";
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(100, 117);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 24);
            this.OK.TabIndex = 5;
            this.OK.Text = "确定";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "label3";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(100, 79);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(157, 21);
            this.textBox1.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(263, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 24);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AddPage
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(378, 159);
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
                    filetext = filetext.Replace("New Document", Filecaption+"\r\n").Replace("<!--STYLE-->", "<link href=\"" + subPath + "default.css\" rel=\"stylesheet\" type=\"text/css\">");
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
				string rdmid=rdm.getID() + "_page";
                string sql = "insert into pages(id,pid,name,caption,updatetime,ptype,mtype,jinfo,jurl,fid,modopen,viewopen,datastr,paraname,membind,elecdt,roledata,rolesession,authrule,flowstat,norightinfo,norighturl,a_ip_s,a_ip_c,a_ip_o,a_se_s,a_se_c,a_se_o,a_jp_s,a_jp_u,a_tp_s,a_tp_c)values('" + rdmid + "','" + pid + "','" + filename.Text.Trim() + "','" + Filecaption + "','" + DateTime.Now + "'," + PageType + ",0,'','','',0,0,'','ftformid','','','','asso_id','%m% and %e% and %f% and %s%','','','',0,'',0,0,'',0,0,'',0,'')";
				globalConst.CurSite.SiteConn.execSql(sql);
				TreeNode nnd=new TreeNode();
				string[] tag=new string[4];
				tag[0]=filename.Text.Trim();
				tag[1]=Filecaption;
				tag[2]=rdmid;
                tag[3] = PageType.ToString();
				nnd.Tag=tag;
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

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            GetNode gn = new GetNode();
            gn.IsJustSelectFile = true;
            gn.ReturnURL = textBox1.Text;
            gn.ShowDialog();
            textBox1.Text = gn.ReturnURL;
            gn = null;
            button1.Enabled = true;
        }

	}

}
