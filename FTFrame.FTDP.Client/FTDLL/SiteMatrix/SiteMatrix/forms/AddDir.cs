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
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AddDir()
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
            this.SuspendLayout();
            // 
            // filename
            // 
            this.filename.Location = new System.Drawing.Point(101, 32);
            this.filename.Name = "filename";
            this.filename.Size = new System.Drawing.Size(236, 21);
            this.filename.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(34, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "filename";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(34, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "caption";
            // 
            // caption
            // 
            this.caption.Location = new System.Drawing.Point(101, 77);
            this.caption.Name = "caption";
            this.caption.Size = new System.Drawing.Size(236, 21);
            this.caption.TabIndex = 3;
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(215, 114);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 24);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "button1";
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(92, 114);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 24);
            this.OK.TabIndex = 5;
            this.OK.Text = "确定";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // AddDir
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(378, 159);
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
					string Filecaption=caption.Text.Trim();
					new dir(Filename);
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
					globalConst.MdiForm.SiteTree.SelectedNode.Nodes.Add(nnd);
					globalConst.MdiForm.SiteTree.SelectedNode.Expand();
					globalConst.MdiForm.SiteTree.SelectedNode=nnd;
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
		
	}
}
