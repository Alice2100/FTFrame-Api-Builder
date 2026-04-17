using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SiteMatrix.SiteClass;
using SiteMatrix.consts;
using SiteMatrix.functions;
using SiteMatrix.database;
using SiteMatrix.Page;
using mshtml;
namespace SiteMatrix.forms
{
	/// <summary>
	/// Workspace 的摘要说明。
	/// </summary>
	/// 
	[LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
	public class Workspace : System.Windows.Forms.Form
	{
		public static System.Windows.Forms.TreeView siteTree;
		public static System.Windows.Forms.TreeView ctrlTree;
		private System.Windows.Forms.TabControl WorkTab;
		private System.Windows.Forms.TabPage tabWare;
		private System.Windows.Forms.TabPage tabPage;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.TabPage tabSite;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.TabPage tabOpen;
		private System.Windows.Forms.ContextMenu contextMenu2;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.MenuItem menuItem21;
		private System.Windows.Forms.MenuItem menuItem22;
		private System.Windows.Forms.MenuItem menuItem23;
		private System.Windows.Forms.MenuItem menuItem24;
		private System.Windows.Forms.MenuItem menuItem25;
		private System.Windows.Forms.MenuItem menuItem26;
		private System.Windows.Forms.MenuItem menuItem27;
		private System.Windows.Forms.MenuItem menuItem28;
		private System.Windows.Forms.MenuItem menuItem30;
		private System.Windows.Forms.MenuItem menuItem31;
		private System.Windows.Forms.MenuItem menuItem32;
		private System.Windows.Forms.MenuItem menuItem33;
		private System.Windows.Forms.MenuItem menuItem34;
		private System.Windows.Forms.MenuItem menuItem35;
		private System.Windows.Forms.MenuItem menuItem36;
		private System.Windows.Forms.MenuItem menuItem37;
		private System.Windows.Forms.MenuItem menuItem38;
		private System.Windows.Forms.MenuItem menuItem39;
		private System.Windows.Forms.MenuItem menuItem40;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem41;
		private System.ComponentModel.IContainer components;
		public Workspace()
		{
			System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl),this);
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();
			// siteTree
			// 
			Workspace.siteTree = new System.Windows.Forms.TreeView();
			Workspace.siteTree.ImageIndex = -1;
			Workspace.siteTree.Location = new System.Drawing.Point(8, 8);
			Workspace.siteTree.Name = "siteTree";
			Workspace.siteTree.SelectedImageIndex = -1;
			Workspace.siteTree.Size = new System.Drawing.Size(240, 288);
			Workspace.siteTree.TabIndex = 0;
			Workspace.siteTree.ImageList=globalConst.Imgs;
			Workspace.siteTree.Enabled=false;
			//ctrl tree
			Workspace.ctrlTree = new System.Windows.Forms.TreeView();
			Workspace.ctrlTree.ImageIndex = -1;
			Workspace.ctrlTree.Location = new System.Drawing.Point(8, 8);
			Workspace.ctrlTree.Name = "ctrlTree";
			Workspace.ctrlTree.SelectedImageIndex = -1;
			Workspace.ctrlTree.Size = new System.Drawing.Size(240, 288);
			Workspace.ctrlTree.TabIndex = 1;
			Workspace.ctrlTree.ImageList=globalConst.Imgs;
			Workspace.ctrlTree.Enabled=false;
			Workspace.ctrlTree.AllowDrop=true;
			
			this.tabSite.Controls.Add(Workspace.siteTree);
			this.tabWare.Controls.Add(Workspace.ctrlTree);
			
			Workspace.siteTree.MouseDown+=new MouseEventHandler(siteTree_MouseDown);
			Workspace.siteTree.MouseUp+=new MouseEventHandler(siteTree_MouseUp);
			Workspace.siteTree.DoubleClick+=new EventHandler(siteTree_DoubleClick);
			Workspace.ctrlTree.MouseDown+=new MouseEventHandler(ctrlTree_MouseDown);
			Workspace.ctrlTree.MouseUp+=new MouseEventHandler(ctrlTree_MouseUp);
			Workspace.ctrlTree.DoubleClick+=new EventHandler(ctrlTree_DoubleClick);
			Workspace.ctrlTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(treeView_ItemDrag);
			Workspace.ctrlTree.DragOver+=new DragEventHandler(treeView3_DragOver);
			Workspace.ctrlTree.DragDrop+=new DragEventHandler(ctrlTree_DragDrop);
			Workspace.ctrlTree.DragLeave+=new EventHandler(treeView3_DragLeave);
			//Workspace.ctrlTree.MouseMove+=new MouseEventHandler(treeView3_MouseMove);
			Workspace.ctrlTree.DragEnter += new System.Windows.Forms.DragEventHandler(treeView_DragEnter);
			Workspace.ctrlTree.GiveFeedback+=new GiveFeedbackEventHandler(this.ctrlTree_GiveFeedback);

			//
			//Workspace.siteTree.ContextMenu = this.contextMenu1;
			
			// 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Workspace));
			this.WorkTab = new System.Windows.Forms.TabControl();
			this.tabSite = new System.Windows.Forms.TabPage();
			this.tabWare = new System.Windows.Forms.TabPage();
			this.tabPage = new System.Windows.Forms.TabPage();
			this.tabOpen = new System.Windows.Forms.TabPage();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.menuItem18 = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.menuItem16 = new System.Windows.Forms.MenuItem();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.menuItem41 = new System.Windows.Forms.MenuItem();
			this.contextMenu2 = new System.Windows.Forms.ContextMenu();
			this.menuItem19 = new System.Windows.Forms.MenuItem();
			this.menuItem20 = new System.Windows.Forms.MenuItem();
			this.menuItem21 = new System.Windows.Forms.MenuItem();
			this.menuItem22 = new System.Windows.Forms.MenuItem();
			this.menuItem23 = new System.Windows.Forms.MenuItem();
			this.menuItem24 = new System.Windows.Forms.MenuItem();
			this.menuItem25 = new System.Windows.Forms.MenuItem();
			this.menuItem26 = new System.Windows.Forms.MenuItem();
			this.menuItem40 = new System.Windows.Forms.MenuItem();
			this.menuItem27 = new System.Windows.Forms.MenuItem();
			this.menuItem28 = new System.Windows.Forms.MenuItem();
			this.menuItem31 = new System.Windows.Forms.MenuItem();
			this.menuItem30 = new System.Windows.Forms.MenuItem();
			this.menuItem32 = new System.Windows.Forms.MenuItem();
			this.menuItem33 = new System.Windows.Forms.MenuItem();
			this.menuItem34 = new System.Windows.Forms.MenuItem();
			this.menuItem35 = new System.Windows.Forms.MenuItem();
			this.menuItem36 = new System.Windows.Forms.MenuItem();
			this.menuItem37 = new System.Windows.Forms.MenuItem();
			this.menuItem38 = new System.Windows.Forms.MenuItem();
			this.menuItem39 = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.WorkTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// WorkTab
			// 
			this.WorkTab.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.WorkTab.Controls.Add(this.tabSite);
			this.WorkTab.Controls.Add(this.tabWare);
			this.WorkTab.Controls.Add(this.tabPage);
			this.WorkTab.Controls.Add(this.tabOpen);
			this.WorkTab.ImageList = this.imageList1;
			this.WorkTab.Location = new System.Drawing.Point(0, 8);
			this.WorkTab.Name = "WorkTab";
			this.WorkTab.SelectedIndex = 0;
			this.WorkTab.Size = new System.Drawing.Size(256, 312);
			this.WorkTab.TabIndex = 0;
			// 
			// tabSite
			// 
			this.tabSite.ImageIndex = 0;
			this.tabSite.Location = new System.Drawing.Point(4, 4);
			this.tabSite.Name = "tabSite";
			this.tabSite.Size = new System.Drawing.Size(248, 285);
			this.tabSite.TabIndex = 0;
			this.tabSite.Text = "Site";
			// 
			// tabWare
			// 
			this.tabWare.ImageIndex = 0;
			this.tabWare.Location = new System.Drawing.Point(4, 4);
			this.tabWare.Name = "tabWare";
			this.tabWare.Size = new System.Drawing.Size(248, 285);
			this.tabWare.TabIndex = 1;
			this.tabWare.Text = "Controls";
			// 
			// tabPage
			// 
			this.tabPage.ImageIndex = 0;
			this.tabPage.Location = new System.Drawing.Point(4, 4);
			this.tabPage.Name = "tabPage";
			this.tabPage.Size = new System.Drawing.Size(248, 285);
			this.tabPage.TabIndex = 2;
			// 
			// tabOpen
			// 
			this.tabOpen.ImageIndex = 0;
			this.tabOpen.Location = new System.Drawing.Point(4, 4);
			this.tabOpen.Name = "tabOpen";
			this.tabOpen.Size = new System.Drawing.Size(248, 285);
			this.tabOpen.TabIndex = 3;
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "全部展开";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "全部闭合";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 2;
			this.menuItem3.Text = "显示文件名";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.Text = "显示中文名";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 4;
			this.menuItem5.Text = "按照文件名顺排";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 5;
			this.menuItem6.Text = "按照文件名逆排";
			this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 6;
			this.menuItem7.Text = "按照中文名顺排";
			this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 7;
			this.menuItem8.Text = "按照中文名逆排";
			this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 8;
			this.menuItem9.Text = "按照日期顺排";
			this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 9;
			this.menuItem10.Text = "按照日期逆排";
			this.menuItem10.Click += new System.EventHandler(this.menuItem10_Click);
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 10;
			this.menuItem17.Text = "-";
			// 
			// menuItem18
			// 
			this.menuItem18.Index = 11;
			this.menuItem18.Text = "打开页面";
			this.menuItem18.Click += new System.EventHandler(this.menuItem18_Click);
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 12;
			this.menuItem11.Text = "-";
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 13;
			this.menuItem12.Text = "新建目录";
			this.menuItem12.Click += new System.EventHandler(this.menuItem12_Click);
			// 
			// menuItem13
			// 
			this.menuItem13.Index = 14;
			this.menuItem13.Text = "新建页面";
			this.menuItem13.Click += new System.EventHandler(this.menuItem13_Click);
			// 
			// menuItem16
			// 
			this.menuItem16.Index = 15;
			this.menuItem16.Text = "删除";
			this.menuItem16.Click += new System.EventHandler(this.menuItem16_Click);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem1,
																						 this.menuItem2,
																						 this.menuItem3,
																						 this.menuItem4,
																						 this.menuItem5,
																						 this.menuItem6,
																						 this.menuItem7,
																						 this.menuItem8,
																						 this.menuItem9,
																						 this.menuItem10,
																						 this.menuItem17,
																						 this.menuItem18,
																						 this.menuItem11,
																						 this.menuItem12,
																						 this.menuItem13,
																						 this.menuItem16,
																						 this.menuItem15,
																						 this.menuItem41});

			// 
			// menuItem15
			// 
			this.menuItem15.Index = 16;
			this.menuItem15.Text = "-";
			// 
			// menuItem41
			// 
			this.menuItem41.Index = 17;
			this.menuItem41.Text = "刷新";
			this.menuItem41.Click += new System.EventHandler(this.menuItem41_Click);
			// 
			// contextMenu2
			// 
			this.contextMenu2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem19,
																						 this.menuItem20,
																						 this.menuItem21,
																						 this.menuItem22,
																						 this.menuItem23,
																						 this.menuItem24,
																						 this.menuItem25,
																						 this.menuItem26,
																						 this.menuItem40,
																						 this.menuItem27,
																						 this.menuItem28,
																						 this.menuItem31,
																						 this.menuItem30,
																						 this.menuItem32,
																						 this.menuItem33,
																						 this.menuItem34,
																						 this.menuItem35,
																						 this.menuItem36,
																						 this.menuItem37,
																						 this.menuItem38,
																						 this.menuItem39,
																						 this.menuItem14});
			// 
			// menuItem19
			// 
			this.menuItem19.Index = 0;
			this.menuItem19.Text = "添加控件";
			this.menuItem19.Click += new System.EventHandler(this.menuItem19_Click);
			// 
			// menuItem20
			// 
			this.menuItem20.Index = 1;
			this.menuItem20.Text = "删除控件";
			this.menuItem20.Click += new System.EventHandler(this.menuItem20_Click);
			// 
			// menuItem21
			// 
			this.menuItem21.Index = 2;
			this.menuItem21.Text = "生成控件实例";
			this.menuItem21.Click += new System.EventHandler(this.menuItem21_Click);
			// 
			// menuItem22
			// 
			this.menuItem22.Index = 3;
			this.menuItem22.Text = "生成控件实例";
			this.menuItem22.Click += new System.EventHandler(this.menuItem22_Click);
			// 
			// menuItem23
			// 
			this.menuItem23.Index = 4;
			this.menuItem23.Text = "克隆控件实例";
			this.menuItem23.Click += new System.EventHandler(this.menuItem23_Click);
			// 
			// menuItem24
			// 
			this.menuItem24.Index = 5;
			this.menuItem24.Text = "删除控件实例";
			this.menuItem24.Click += new System.EventHandler(this.menuItem24_Click);
			// 
			// menuItem25
			// 
			this.menuItem25.Index = 6;
			this.menuItem25.Text = "生成片断实例";
			this.menuItem25.Click += new System.EventHandler(this.menuItem25_Click);
			// 
			// menuItem26
			// 
			this.menuItem26.Index = 7;
			this.menuItem26.Text = "克隆片断实例";
			this.menuItem26.Click += new System.EventHandler(this.menuItem26_Click);
			// 
			// menuItem40
			// 
			this.menuItem40.Index = 8;
			this.menuItem40.Text = "删除片断实例";
			this.menuItem40.Click += new System.EventHandler(this.menuItem40_Click);
			// 
			// menuItem27
			// 
			this.menuItem27.Index = 9;
			this.menuItem27.Text = "打开页面";
			this.menuItem27.Click += new System.EventHandler(this.menuItem27_Click);
			// 
			// menuItem28
			// 
			this.menuItem28.Index = 10;
			this.menuItem28.Text = "删除页面";
			this.menuItem28.Click += new System.EventHandler(this.menuItem28_Click);
			// 
			// menuItem31
			// 
			this.menuItem31.Index = 11;
			this.menuItem31.Text = "-";
			// 
			// menuItem30
			// 
			this.menuItem30.Index = 12;
			this.menuItem30.Text = "显示中文名";
			this.menuItem30.Click += new System.EventHandler(this.menuItem30_Click);
			// 
			// menuItem32
			// 
			this.menuItem32.Index = 13;
			this.menuItem32.Text = "显示英文名";
			this.menuItem32.Click += new System.EventHandler(this.menuItem32_Click);
			// 
			// menuItem33
			// 
			this.menuItem33.Index = 14;
			this.menuItem33.Text = "按中文名顺排";
			this.menuItem33.Click += new System.EventHandler(this.menuItem33_Click);
			// 
			// menuItem34
			// 
			this.menuItem34.Index = 15;
			this.menuItem34.Text = "按中文名逆排";
			this.menuItem34.Click += new System.EventHandler(this.menuItem34_Click);
			// 
			// menuItem35
			// 
			this.menuItem35.Index = 16;
			this.menuItem35.Text = "按英文名顺排";
			this.menuItem35.Click += new System.EventHandler(this.menuItem35_Click);
			// 
			// menuItem36
			// 
			this.menuItem36.Index = 17;
			this.menuItem36.Text = "按英文名逆排";
			this.menuItem36.Click += new System.EventHandler(this.menuItem36_Click);
			// 
			// menuItem37
			// 
			this.menuItem37.Index = 18;
			this.menuItem37.Text = "-";
			// 
			// menuItem38
			// 
			this.menuItem38.Index = 19;
			this.menuItem38.Text = "全部展开";
			this.menuItem38.Click += new System.EventHandler(this.menuItem38_Click);
			// 
			// menuItem39
			// 
			this.menuItem39.Index = 20;
			this.menuItem39.Text = "全部闭合";
			this.menuItem39.Click += new System.EventHandler(this.menuItem39_Click);
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 21;
			this.menuItem14.Text = "刷新";
			this.menuItem14.Click += new System.EventHandler(this.menuItem14_Click);
			// 
			// Workspace
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(258, 328);
			this.Controls.Add(this.WorkTab);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Workspace";
			this.ShowInTaskbar = false;
			this.Text = "Workspace";
			this.Load += new System.EventHandler(this.Workspace_Load);
			this.WorkTab.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void Workspace_Load(object sender, System.EventArgs e)
		{
		}

		private void menuItem7_Click(object sender, System.EventArgs e)
		{
			Workspace.siteTree.Nodes.Clear();
			TreeNode rootnode;
			rootnode=new TreeNode(globalConst.CurSite.Domain,0,1);
			string[] tag=new string[3];
			tag[0]=globalConst.CurSite.Domain;
			tag[1]=globalConst.CurSite.Domain;
			tag[2]="root";
			rootnode.Tag=tag;
			Workspace.siteTree.Nodes.Add(rootnode);
			globalConst.siteTreeOrderby=" order by caption";//can add desc
			SiteClass.Site.constructTree(Workspace.siteTree,rootnode,"root",globalConst.siteTreeOrderby,globalConst.siteTreeShowColName);
			Workspace.siteTree.CollapseAll();
			rootnode.Expand();
		}
		public void refreshSiteTree()
		{
            globalConst.MdiForm.SiteTree.Nodes.Clear();
			TreeNode rootnode;
			rootnode=new TreeNode(globalConst.CurSite.Domain,0,1);
			string[] tag=new string[3];
			tag[0]=globalConst.CurSite.Domain;
			tag[1]=globalConst.CurSite.Domain;
			tag[2]="root";
			rootnode.Tag=tag;
            globalConst.MdiForm.SiteTree.Nodes.Add(rootnode);
            SiteClass.Site.constructTree(globalConst.MdiForm.SiteTree, rootnode, "root", globalConst.siteTreeOrderby, globalConst.siteTreeShowColName);
            globalConst.MdiForm.SiteTree.CollapseAll();
			rootnode.Expand();
		}
		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			Workspace.siteTree.Nodes.Clear();
			TreeNode rootnode;
			rootnode=new TreeNode(globalConst.CurSite.Domain,0,1);
			string[] tag=new string[3];
			tag[0]=globalConst.CurSite.Domain;
			tag[1]=globalConst.CurSite.Domain;
			tag[2]="root";
			rootnode.Tag=tag;
			Workspace.siteTree.Nodes.Add(rootnode);
			globalConst.siteTreeOrderby=" order by name";//can add desc
			SiteClass.Site.constructTree(Workspace.siteTree,rootnode,"root",globalConst.siteTreeOrderby,globalConst.siteTreeShowColName);
			Workspace.siteTree.CollapseAll();
			rootnode.Expand();
		}

		private void menuItem6_Click(object sender, System.EventArgs e)
		{
			Workspace.siteTree.Nodes.Clear();
			TreeNode rootnode;
			rootnode=new TreeNode(globalConst.CurSite.Domain,0,1);
			string[] tag=new string[3];
			tag[0]=globalConst.CurSite.Domain;
			tag[1]=globalConst.CurSite.Domain;
			tag[2]="root";
			rootnode.Tag=tag;
			Workspace.siteTree.Nodes.Add(rootnode);
			globalConst.siteTreeOrderby=" order by name desc";//can add desc
			SiteClass.Site.constructTree(Workspace.siteTree,rootnode,"root",globalConst.siteTreeOrderby,globalConst.siteTreeShowColName);
			Workspace.siteTree.CollapseAll();
			rootnode.Expand();
		}

		private void menuItem8_Click(object sender, System.EventArgs e)
		{
			Workspace.siteTree.Nodes.Clear();
			TreeNode rootnode;
			rootnode=new TreeNode(globalConst.CurSite.Domain,0,1);
			string[] tag=new string[3];
			tag[0]=globalConst.CurSite.Domain;
			tag[1]=globalConst.CurSite.Domain;
			tag[2]="root";
			rootnode.Tag=tag;
			Workspace.siteTree.Nodes.Add(rootnode);
			globalConst.siteTreeOrderby=" order by caption desc";//can add desc
			SiteClass.Site.constructTree(Workspace.siteTree,rootnode,"root",globalConst.siteTreeOrderby,globalConst.siteTreeShowColName);
			Workspace.siteTree.CollapseAll();
			rootnode.Expand();
		}

		private void menuItem9_Click(object sender, System.EventArgs e)
		{
			Workspace.siteTree.Nodes.Clear();
			TreeNode rootnode;
			rootnode=new TreeNode(globalConst.CurSite.Domain,0,1);
			string[] tag=new string[3];
			tag[0]=globalConst.CurSite.Domain;
			tag[1]=globalConst.CurSite.Domain;
			tag[2]="root";
			rootnode.Tag=tag;
			Workspace.siteTree.Nodes.Add(rootnode);
			globalConst.siteTreeOrderby=" order by updatetime";//can add desc
			SiteClass.Site.constructTree(Workspace.siteTree,rootnode,"root",globalConst.siteTreeOrderby,globalConst.siteTreeShowColName);
			Workspace.siteTree.CollapseAll();
			rootnode.Expand();
		}

		private void menuItem10_Click(object sender, System.EventArgs e)
		{
			Workspace.siteTree.Nodes.Clear();
			TreeNode rootnode;
			rootnode=new TreeNode(globalConst.CurSite.Domain,0,1);
			string[] tag=new string[3];
			tag[0]=globalConst.CurSite.Domain;
			tag[1]=globalConst.CurSite.Domain;
			tag[2]="root";
			rootnode.Tag=tag;
			Workspace.siteTree.Nodes.Add(rootnode);
			globalConst.siteTreeOrderby=" order by updatetime desc";//can add desc
			SiteClass.Site.constructTree(Workspace.siteTree,rootnode,"root",globalConst.siteTreeOrderby,globalConst.siteTreeShowColName);
			Workspace.siteTree.CollapseAll();
			rootnode.Expand();
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			SiteClass.Site.loopTree4TextChange(Workspace.siteTree.Nodes,0);
			globalConst.siteTreeShowColName="name";//caption or name
			/*
			Workspace.siteTree.Nodes.Clear();
			TreeNode rootnode;
			rootnode=new TreeNode(globalConst.CurSite.Domain);
			Workspace.siteTree.Nodes.Add(rootnode);
			globalConst.siteTreeShowColName="name";//caption or name
			SiteClass.Site.constructTree(Workspace.siteTree,rootnode,"root",globalConst.siteTreeOrderby,globalConst.siteTreeShowColName);
			Workspace.siteTree.CollapseAll();
			rootnode.Expand();
			*/
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			SiteClass.Site.loopTree4TextChange(Workspace.siteTree.Nodes,1);
			globalConst.siteTreeShowColName="caption";//caption or name
			/*
			Workspace.siteTree.Nodes.Clear();
			TreeNode rootnode;
			rootnode=new TreeNode(globalConst.CurSite.Domain);
			Workspace.siteTree.Nodes.Add(rootnode);
			globalConst.siteTreeShowColName="caption";//caption or name
			SiteClass.Site.constructTree(Workspace.siteTree,rootnode,"root",globalConst.siteTreeOrderby,globalConst.siteTreeShowColName);
			Workspace.siteTree.CollapseAll();
			rootnode.Expand();
			*/
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			Workspace.siteTree.CollapseAll();
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			Workspace.siteTree.ExpandAll();
		}




		private void menuItem12_Click(object sender, System.EventArgs e)
		{
			AddDir ad=new AddDir();
			ad.ShowDialog();
		}

		private void menuItem13_Click(object sender, System.EventArgs e)
		{
			AddPage ap=new AddPage();
            ap.PageType = 0;
			ap.ShowDialog();
		}

		private void siteTree_MouseUp(object sender, MouseEventArgs e)
		{
			if(e.Button.Equals(MouseButtons.Right))
			{
				Point p=new Point(e.X,e.Y);
			contextMenu1.Show(siteTree,p);
			}
		}
		private void ctrlTree_MouseUp(object sender, MouseEventArgs e)
		{
			if(e.Button.Equals(MouseButtons.Right))
			{
				Point p=new Point(e.X,e.Y);
				updateMenu4Ctrl(ctrlTree.SelectedNode);
				contextMenu2.Show(ctrlTree,p);
			}
		}
		private void updateMenu4Ctrl(TreeNode nd)
		{
			if(nd==null)
			{
				menuItem19.Visible=true;
				menuItem20.Visible=false;
				menuItem21.Visible=false;
				menuItem22.Visible=false;
				menuItem23.Visible=false;
				menuItem24.Visible=false;
				menuItem25.Visible=false;
				menuItem26.Visible=false;
				menuItem40.Visible=false;
				menuItem27.Visible=false;
				menuItem28.Visible=false;
				return;
			}
			switch(tree.getTypeFromID(nd))
			{
				case "comp":
					menuItem19.Visible=true;
					menuItem20.Visible=true;
					menuItem21.Visible=true;
					menuItem22.Visible=false;
					menuItem23.Visible=false;
					menuItem24.Visible=false;
					menuItem25.Visible=false;
					menuItem26.Visible=false;
					menuItem40.Visible=false;
					menuItem27.Visible=false;
					menuItem28.Visible=false;
					break;
				case "ctrl":
					menuItem19.Visible=false;
					menuItem20.Visible=false;
					menuItem21.Visible=false;
					menuItem22.Visible=true;
					menuItem23.Visible=true;
					menuItem24.Visible=true;
					menuItem25.Visible=false;
					menuItem26.Visible=false;
					menuItem40.Visible=false;
					menuItem27.Visible=false;
					menuItem28.Visible=false;
					break;
				case "part":
					menuItem19.Visible=false;
					menuItem20.Visible=false;
					menuItem21.Visible=false;
					menuItem22.Visible=false;
					menuItem23.Visible=false;
					menuItem24.Visible=false;
					menuItem25.Visible=true;
					menuItem26.Visible=true;
					menuItem40.Visible=true;
					menuItem27.Visible=false;
					menuItem28.Visible=false;
					break;
				case "page":
					menuItem19.Visible=false;
					menuItem20.Visible=false;
					menuItem21.Visible=false;
					menuItem22.Visible=false;
					menuItem23.Visible=false;
					menuItem24.Visible=false;
					menuItem25.Visible=false;
					menuItem26.Visible=false;
					menuItem40.Visible=false;
					menuItem27.Visible=true;
					menuItem28.Visible=true;
					break;
				default:
					break;
			}
		}
		private void menuItem16_Click(object sender, System.EventArgs e)
		{
			Cursor cr=Cursor.Current;
			try
			{
				if(siteTree.SelectedNode==null)
				{
					siteTree.SelectedNode=siteTree.Nodes[0];
				}
				if(tree.getTypeFromID(siteTree.SelectedNode).Equals("root"))
				{
					//delete site code add here
					return;
				}
			
				if(tree.getTypeFromID(siteTree.SelectedNode).Equals("drct"))
				{
					Cursor.Current=Cursors.WaitCursor;
					//delete directory
					if(MsgBox.OKCancel("delete directory ?").Equals(DialogResult.OK))
					{
						string path=globalConst.CurSite.Path + tree.getPath(siteTree.SelectedNode);
					
						string sql="delete from directory where id='" + tree.getID(siteTree.SelectedNode) + "'";
						if(globalConst.CurSite.SiteConn.execSql(sql)==0)
						{
							MsgBox.Warning("not delete data");
						}
						dir.Delete(path,true);
						if(dir.Exists(path))
						{
							MsgBox.Warning("delete directory failed");
						}
						//delete other
						SiteClass.Site.loopTree4DeleteData(siteTree.SelectedNode.Nodes,2);
						siteTree.SelectedNode.Remove();
						refreshControlTree();
					}
					Cursor.Current=cr;
					return;
				}
			
				if(tree.getTypeFromID(siteTree.SelectedNode).Equals("page"))
				{
					Cursor.Current=Cursors.WaitCursor;
					//delete page
					if(MsgBox.OKCancel("delete page ?").Equals(DialogResult.OK))
					{
						string path=globalConst.CurSite.Path + tree.getPath(siteTree.SelectedNode);
						file.Delete(path);
						if(file.Exists(path))
						{
							MsgBox.Warning("delete file failed");
						}
						string sql="delete from pages where id='" + tree.getID(siteTree.SelectedNode) + "'";
						if(globalConst.CurSite.SiteConn.execSql(sql)==0)
						{
							MsgBox.Warning("not delete data");
						}
						sql="delete from part_in_page where pageid='" + tree.getID(siteTree.SelectedNode) + "'";
						globalConst.CurSite.SiteConn.execSql(sql);
						//close editor form
						form.closeEditor(tree.getID(siteTree.SelectedNode));
						siteTree.SelectedNode.Remove();
						refreshControlTree();
					}
					Cursor.Current=cr;
					return;
				}
				if(tree.getTypeFromID(siteTree.SelectedNode).Equals("part"))
				{
					Cursor.Current=Cursors.WaitCursor;
					//delete directory
					if(MsgBox.OKCancel("delete this part in this page ?").Equals(DialogResult.OK))
					{
						string id=tree.getID(siteTree.SelectedNode);
						SiteClass.Site.DeletePartInPage(id);
						siteTree.SelectedNode.Remove();
						PageWare.refreshPagesInCtrlTree(id);
					}
					Cursor.Current=cr;
					return;
				}
			}
			catch(Exception ex)
			{
				new error(ex);
				Cursor.Current=cr;
			}
			
		}

		private void siteTree_DoubleClick(object sender, EventArgs e)
		{
			openPage();
		}
		private void ctrlTree_DoubleClick(object sender, EventArgs e)
		{
			openPage2();
		}

		private void menuItem15_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(siteTree.SelectedNode==null)
				{
					siteTree.SelectedNode=siteTree.Nodes[0];
				}
				if(tree.getTypeFromID(siteTree.SelectedNode).Equals("root"))
				{
					//delete site code add here
					return;
				}
			
				if(tree.getTypeFromID(siteTree.SelectedNode).Equals("drct"))
				{
					AddDir ad=new AddDir();
					ad.isEdit=true;
					ad._filename=((string[])siteTree.SelectedNode.Tag)[0];
					ad._caption=((string[])siteTree.SelectedNode.Tag)[1];
					ad.ShowDialog();
					return;
				}
			
				if(tree.getTypeFromID(siteTree.SelectedNode).Equals("page"))
				{
					AddPage ap=new AddPage();
                    ap.PageType = 0;
					ap.isEdit=true;
					ap._filename=((string[])siteTree.SelectedNode.Tag)[0];
					ap._caption=((string[])siteTree.SelectedNode.Tag)[1];
					ap.ShowDialog();
					return;
				}
			}
			catch(Exception ex)
			{
				new error(ex);
			}
			
		}
		private void openPage2()
		{
			Cursor cr=Cursor.Current;
			try
			{
				Cursor.Current=Cursors.WaitCursor;
				string id=tree.getID(ctrlTree.SelectedNode);
				if(tree.getTypeFromID(id).Equals("page"))
				{
					if(form.doActive(id))
					{
						Cursor.Current=cr;
						return;
					}
					string path="";
					TreeNode sitend=tree.getSiteNodeByID(id);
					if(sitend==null)
					{
						MsgBox.Error("can not found this page in site tree!");
						return;
					}
					path=globalConst.CurSite.Path + tree.getPath(sitend);
					if(path.Equals(""))
					{
						MsgBox.Error("can not found this page!");
						return;
					}
					//_edit.htm页面不存在则重新生成，暂时这样处理，不考虑其他人为因素造成文件的损坏
					if(!file.Exists(path + "_edit.htm"))
					{
						SiteClass.Site.constructEditPageFromText(id,path);
					}
					else
					{
						//如果edit文件被手动修改过，重新生成
						System.IO.FileInfo fioedit=new System.IO.FileInfo(path + "_edit.htm");
						System.IO.FileInfo fio=new System.IO.FileInfo(path);
						if(!(fioedit.LastWriteTime.Equals(fio.LastWriteTime)))
						{
							SiteClass.Site.constructEditPageFromText(id,path);
						}
					}
                    form.addEditor(path, ((string[])ctrlTree.SelectedNode.Tag)[1], id, ((string[])ctrlTree.SelectedNode.Tag)[0], int.Parse(((string[])ctrlTree.SelectedNode.Tag)[3]));
				}
				Cursor.Current=cr;
			}
			catch(Exception ex)
			{
				new error(ex);
				Cursor.Current=cr;
			}
		}
		private void openPage()
		{
			Cursor cr=Cursor.Current;
			try
			{
				Cursor.Current=Cursors.WaitCursor;
				string id=tree.getID(siteTree.SelectedNode);
				if(tree.getTypeFromID(id).Equals("page"))
				{
					if(form.doActive(id))
					{
					Cursor.Current=cr;
					return;
					}
					string path=globalConst.CurSite.Path + tree.getPath(siteTree.SelectedNode);
					//_edit.htm页面不存在则重新生成，暂时这样处理，不考虑其他人为因素造成文件的损坏,fixed
					if(!file.Exists(path + "_edit.htm"))
					{
						SiteClass.Site.constructEditPageFromText(id,path);
					}
					else
					{
						//如果edit文件被手动修改过，重新生成
						System.IO.FileInfo fioedit=new System.IO.FileInfo(path + "_edit.htm");
						System.IO.FileInfo fio=new System.IO.FileInfo(path);
						if(!(fioedit.LastWriteTime.Equals(fio.LastWriteTime)))
						{
						SiteClass.Site.constructEditPageFromText(id,path);
						}
					}
                    form.addEditor(path, ((string[])siteTree.SelectedNode.Tag)[1], id, ((string[])siteTree.SelectedNode.Tag)[0], int.Parse(((string[])siteTree.SelectedNode.Tag)[3]));
				}
				if(tree.getTypeFromID(id).Equals("part"))
				{
					string partid=tree.getID(siteTree.SelectedNode);
					string pageid=tree.getID(siteTree.SelectedNode.Parent);
					string sql="select count(*) from part_in_page where pageid='" + pageid + "' and partid='" + id + "'";
					if(globalConst.CurSite.SiteConn.GetInt32(sql)==0)
					{
						siteTree.SelectedNode.Remove();
						goto LoopOut;
					}
					Editor edr=form.getEditor(pageid);
					if(edr==null)
					{
						siteTree.SelectedNode=siteTree.SelectedNode.Parent;
						if(tree.getTypeFromID(tree.getID(siteTree.SelectedNode)).Equals("page"))
						{
							openPage();
						}
						else
						{
							log.Debug("siteTree part node.parent not is a page!","openPage");
						}
					}
					else
					{
						IHTMLElementCollection iec=edr.editocx.getElementsByTagName("span");
						//int i;
						foreach(IHTMLElement ihe in iec)
						{
							//IHTMLElement ihe=(IHTMLElement)iec.item(i,i);
							//if(ihe.tagName.ToLower().Equals("span"))
							//{
							if(ihe.getAttribute("id",0)!=null && ihe.getAttribute("name",0)!=null)
							{
                                if (ihe.getAttribute("id", 0).ToString().Equals("dotforsitecom") && ihe.getAttribute("name", 0).ToString().Equals("dotforsitecom"))
								{
									if(ihe.getAttribute("idname",0).ToString().Equals(partid))
									{
										IHTMLTxtRange it=edr.INITxtRange;
										it.moveToElementText(ihe);
										goto LoopOut;
									}
								}
							}
							//}
						}
						
					}
				}
			LoopOut:
				Cursor.Current=cr;
			}
			catch(Exception ex)
			{
				new error(ex);
				Cursor.Current=cr;
			}
		}

		private void menuItem18_Click(object sender, System.EventArgs e)
		{
			openPage();
		}

		private void menuItem38_Click(object sender, System.EventArgs e)
		{
			ctrlTree.ExpandAll();
		}

		private void menuItem39_Click(object sender, System.EventArgs e)
		{
			ctrlTree.CollapseAll();
		}

		private void menuItem33_Click(object sender, System.EventArgs e)
		{
		ctrlTree.Nodes.Clear();
			globalConst.ctrlTreeOrderby="caption";
			globalConst.ctrlTreeOrdertype="";
			refreshControlTree();
		}

		private void menuItem34_Click(object sender, System.EventArgs e)
		{
			ctrlTree.Nodes.Clear();
			globalConst.ctrlTreeOrderby="caption";
			globalConst.ctrlTreeOrdertype="desc";
			refreshControlTree();
		}

		private void menuItem35_Click(object sender, System.EventArgs e)
		{
			ctrlTree.Nodes.Clear();
			globalConst.ctrlTreeOrderby="name";
			globalConst.ctrlTreeOrdertype="";
			refreshControlTree();
		}

		private void menuItem36_Click(object sender, System.EventArgs e)
		{
			ctrlTree.Nodes.Clear();
			globalConst.ctrlTreeOrderby="name";
			globalConst.ctrlTreeOrdertype="desc";
			refreshControlTree();
			
		}

		private void menuItem30_Click(object sender, System.EventArgs e)
		{
			SiteClass.Site.loopTree4TextChange(Workspace.ctrlTree.Nodes,1);
			globalConst.ctrlTreeShowColName="caption";//caption or name
		}

		private void menuItem32_Click(object sender, System.EventArgs e)
		{
			SiteClass.Site.loopTree4TextChange(Workspace.ctrlTree.Nodes,0);
			globalConst.ctrlTreeShowColName="name";//caption or name
		}
		public static void refreshControlTree()
		{
			try
			{
                globalConst.MdiForm.ControlTree.Nodes.Clear();
                SiteClass.Site.constructControlTree(globalConst.MdiForm.ControlTree, globalConst.ctrlTreeOrderby, globalConst.ctrlTreeOrdertype, globalConst.ctrlTreeShowColName);
			}
			catch(Exception ex)
			{
			new error(ex);
			}
		}
		private void menuItem19_Click(object sender, System.EventArgs e)
		{
			Cursor c=Cursor.Current;
			try
			{
				Cursor.Current=Cursors.WaitCursor;
				controls.controls.AddControl();
				refreshControlTree();
				ToolBox.InitToolBoxControls();
				Cursor.Current=c;
			}
			catch(Exception ex)
			{
				new error(ex);
				Cursor.Current=c;
			}
		}

		private void menuItem14_Click(object sender, System.EventArgs e)
		{
		refreshControlTree();
		}

		private void menuItem20_Click(object sender, System.EventArgs e)
		{
			Cursor c=Cursor.Current;
			try
			{
				
				Cursor.Current=Cursors.WaitCursor;
				if(ctrlTree.SelectedNode!=null)
				{
					if(MsgBox.OKCancel("Delete this control?")==DialogResult.OK)
					{
						string ctrlid=((string[])ctrlTree.SelectedNode.Tag)[2];
						string controlname=ctrlid.Substring(0,ctrlid.Length-5);
						controls.controls.DeleteControl(controlname);
						refreshControlTree();
                        ToolBox.InitToolBoxControls();
					}
				}
				Cursor.Current=c;
			}
			catch(Exception ex)
			{
				new error(ex);
				Cursor.Current=c;
			}
		}

		private void menuItem21_Click(object sender, System.EventArgs e)
		{
			Cursor c=Cursor.Current;
			try
			{
				
				Cursor.Current=Cursors.WaitCursor;
				if(ctrlTree.SelectedNode!=null)
				{
					int nodeindex=ctrlTree.SelectedNode.Index;
						string ctrlid=((string[])ctrlTree.SelectedNode.Tag)[2];
						string controlname=ctrlid.Substring(0,ctrlid.Length-5);
						NewControl nc=new NewControl();
						nc.controlName=controlname;
					nc.addType="new";
						nc.ShowDialog();
					if(!nc.isCancel)
					{
						refreshControlTree();
						ctrlTree.Nodes[nodeindex].Expand();
					}
				}
				Cursor.Current=c;
			}
			catch(Exception ex)
			{
				new error(ex);
				Cursor.Current=c;
			}
		}

		private void menuItem22_Click(object sender, System.EventArgs e)
		{
			Cursor c=Cursor.Current;
			try
			{
				
				Cursor.Current=Cursors.WaitCursor;
				if(ctrlTree.SelectedNode!=null)
				{
					int nodeindex=ctrlTree.SelectedNode.Parent.Index;
					string ctrlid=((string[])ctrlTree.SelectedNode.Parent.Tag)[2];
					string controlname=ctrlid.Substring(0,ctrlid.Length-5);
					NewControl nc=new NewControl();
					nc.controlName=controlname;
					nc.addType="new";
					nc.ShowDialog();
					if(!nc.isCancel)
					{
						refreshControlTree();
						ctrlTree.Nodes[nodeindex].Expand();
					}
				}
				Cursor.Current=c;
			}
			catch(Exception ex)
			{
				new error(ex);
				Cursor.Current=c;
			}
		}

		private void menuItem24_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(ctrlTree.SelectedNode!=null)
				{
					if(MsgBox.OKCancel("Delete this control will delete all it's parts ,are you sure?")==DialogResult.OK)
					{
						int nodeindex=ctrlTree.SelectedNode.Parent.Index;
						string ctrlid=((string[])ctrlTree.SelectedNode.Tag)[2];
						string sql="delete from controls where id='" + ctrlid + "'";
						globalConst.CurSite.SiteConn.execSql(sql);
						sql="select id from parts where controlid='" + ctrlid + "'";
						DR dr=new DR(globalConst.CurSite.SiteConn.OpenRecord(sql));
						string partids="";
						while(dr.Read())
						{
							partids+=dr.getString(0) + "{";
						}
						dr.Close();
						sql="delete from parts where controlid='" + ctrlid + "'";
						globalConst.CurSite.SiteConn.execSql(sql);
						string[] partidsa=partids.Split('{');
						foreach(string id in partidsa)
						{
							if(id!=null && !id.Equals(""))
							{
								//
								SiteClass.Site.DeletePartInPage(id);
								//
							sql="delete from part_in_page where partid='" + id + "'";
							globalConst.CurSite.SiteConn.execSql(sql);
							}
						}
						refreshControlTree();
						ctrlTree.Nodes[nodeindex].Expand();
						refreshSiteTree();
					}
				}
			}
			catch(Exception ex)
			{
				new error(ex);
			}
		}

		private void menuItem23_Click(object sender, System.EventArgs e)
		{
			Cursor c=Cursor.Current;
			try
			{
				
				Cursor.Current=Cursors.WaitCursor;
				if(ctrlTree.SelectedNode!=null)
				{
					int nodeindex=ctrlTree.SelectedNode.Parent.Index;
					string ctrlid=((string[])ctrlTree.SelectedNode.Tag)[2];
					NewControl nc=new NewControl();
					nc.controlName=ctrlid;
					nc.addType="clone";
					nc.ShowDialog();
					if(!nc.isCancel)
					{
						refreshControlTree();
						ctrlTree.Nodes[nodeindex].Expand();
					}
				}
				Cursor.Current=c;
			}
			catch(Exception ex)
			{
				new error(ex);
				Cursor.Current=c;
			}
		}

		private void menuItem25_Click(object sender, System.EventArgs e)
		{
			Cursor c=Cursor.Current;
			try
			{
				
				Cursor.Current=Cursors.WaitCursor;
				if(ctrlTree.SelectedNode!=null)
				{
					//int nodeindex=ctrlTree.SelectedNode.Parent.Index;
					//int nodepindex=ctrlTree.SelectedNode.Parent.Parent.Index;
					string partid=((string[])ctrlTree.SelectedNode.Tag)[2];
					string[] AddPartSA=PageWare.AddPart(partid);
					if(AddPartSA!=null)
					{
						TreeNode nd;
						if(globalConst.ctrlTreeShowColName.Equals("name"))
							nd=new TreeNode(AddPartSA[0],12,13);
						else
							nd=new TreeNode(AddPartSA[1],12,13);
						nd.Tag=AddPartSA;
						ctrlTree.SelectedNode.Parent.Nodes.Add(nd);
//						refreshControlTree();
//						//ctrlTree.Nodes[nodepindex].Expand();
//						ctrlTree.Nodes[nodeindex].Expand();
					}
					else
					{
						MsgBox.Error("Add Part Error!");
					}
				}
				Cursor.Current=c;
			}
			catch(Exception ex)
			{
				new error(ex);
				Cursor.Current=c;
			}
		}

		private void menuItem26_Click(object sender, System.EventArgs e)
		{
			Cursor c=Cursor.Current;
			try
			{
				
				Cursor.Current=Cursors.WaitCursor;
				if(ctrlTree.SelectedNode!=null)
				{
					//int nodeindex=ctrlTree.SelectedNode.Parent.Index;
					//int nodepindex=ctrlTree.SelectedNode.Parent.Parent.Index;
					string partid=((string[])ctrlTree.SelectedNode.Tag)[2];
					string tag0=((string[])ctrlTree.SelectedNode.Tag)[0];
					string tag1=((string[])ctrlTree.SelectedNode.Tag)[1];
					string clonepart=PageWare.ClonePart(partid);
					if(clonepart!=null)
					{
						TreeNode nd;
						string[] tag=new string[3];
						tag[0]=tag0;
						tag[1]=tag1;
						tag[2]=clonepart;
						if(globalConst.ctrlTreeShowColName.Equals("name"))
							nd=new TreeNode(tag[0],12,13);
						else
							nd=new TreeNode(tag[1],12,13);
						nd.Tag=tag;
						ctrlTree.SelectedNode.Parent.Nodes.Add(nd);
						//						refreshControlTree();
						//						//ctrlTree.Nodes[nodepindex].Expand();
						//						ctrlTree.Nodes[nodeindex].Expand();
					}
					else
					{
						MsgBox.Error("Clone Part Error!");
					}
				}
				Cursor.Current=c;
			}
			catch(Exception ex)
			{
				new error(ex);
				Cursor.Current=c;
			}
		}

		private void menuItem40_Click(object sender, System.EventArgs e)
		{
			try
			{
			if(ctrlTree.SelectedNode!=null)
			{
				string partid=((string[])ctrlTree.SelectedNode.Tag)[2];
				string sql="select name,controlid from parts where id='" + partid + "'";
				string partname="";
				string controlid="";
				DR dr=new DR(globalConst.CurSite.SiteConn.OpenRecord(sql));
				if(dr.Read())
				{
					partname=dr.getString(0);
					controlid=dr.getString(1);
				}
				else
				{
				dr.Close();
				return;
				}
				dr.Close();
				sql="select count(id) from parts where name='" + partname + "' and controlid='" + controlid + "'";
				if(globalConst.CurSite.SiteConn.GetInt32(sql)<2)
				{
				MsgBox.Information("Only one part found in this control,can not delete!");
				return;
				}
				if(MsgBox.OKCancel("Delete this part ,are you sure?")==DialogResult.OK)
				{
					sql="delete from parts where id='" + partid + "'";
					globalConst.CurSite.SiteConn.execSql(sql);
					//
					SiteClass.Site.DeletePartInPage(partid);
					//
					sql="delete from part_in_page where partid='" + partid + "'";
					globalConst.CurSite.SiteConn.execSql(sql);
					ctrlTree.SelectedNode.Remove();
					ctrlTree.SelectedNode=null;
				}
				refreshSiteTree();
			}
		}
		catch(Exception ex)
	{
		new error(ex);
	}
		}

		private void menuItem27_Click(object sender, System.EventArgs e)
		{
			openPage2();
		}

		private void menuItem41_Click(object sender, System.EventArgs e)
		{
			refreshSiteTree();
		}

		private void menuItem28_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(ctrlTree.SelectedNode!=null)
				{
					//delete page
					if(MsgBox.OKCancel("delete page ?").Equals(DialogResult.OK))
					{
						string path="";
						string id=((string[])ctrlTree.SelectedNode.Tag)[2];
						TreeNode sitend=tree.getSiteNodeByID(id);
						if(sitend==null)
						{
							MsgBox.Error("can not found this page in site tree!");
							return;
						}
						path=globalConst.CurSite.Path + tree.getPath(sitend);
						if(path.Equals(""))
						{
							MsgBox.Error("can not found this page!");
							return;
						}
						file.Delete(path);
						if(file.Exists(path))
						{
							MsgBox.Warning("delete file failed");
						}
						string sql="delete from pages where id='" + id + "'";
						if(globalConst.CurSite.SiteConn.execSql(sql)==0)
						{
							MsgBox.Warning("not delete data");
						}
						sql="delete from part_in_page where pageid='" + id + "'";
						globalConst.CurSite.SiteConn.execSql(sql);
						//close editor form
						form.closeEditor(id);
						//ctrlTree.SelectedNode.Remove();
						refreshControlTree();
						refreshSiteTree();
					}
				}
			}
			catch(Exception ex)
			{
			new error(ex);
			}
		}

		private void menuItem29_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(ctrlTree.SelectedNode!=null)
				{
					//delete page
					if(MsgBox.OKCancel("Cancel relation will delete this part in this page,continue ?").Equals(DialogResult.OK))
					{
						string id=((string[])ctrlTree.SelectedNode.Tag)[2];
						
						string sql="delete from part_in_page where pageid='" + id + "'";
						globalConst.CurSite.SiteConn.execSql(sql);
						
						ctrlTree.SelectedNode.Remove();
						refreshSiteTree();
					}
				}
			}
			catch(Exception ex)
			{
				new error(ex);
			}
		}



		private void siteTree_MouseDown(object sender, MouseEventArgs e)
		{
			Point p=new Point(e.X,e.Y);
			TreeNode nd= siteTree.GetNodeAt(p);
			if(nd!=null)
			{
				siteTree.SelectedNode=nd;
				if(tree.getTypeFromID(tree.getID(nd)).Equals("root"))
				{
					globalConst.PropForm.CurPropTag.Enabled=true;
					globalConst.PropForm.CurPropTag.Items.Clear();
					globalConst.PropForm.CurPropTag.Items.Add("Site Property");
					globalConst.PropForm.CurPropTag.SelectedIndex=0;
					globalConst.PropForm.ProOthers.Enabled=false;
					globalConst.PropForm.propertyGrid1.SelectedObject=SiteMatrix.PropertySpace.Site.PropertySite.Bag();
					return;
				}
				if(tree.getTypeFromID(tree.getID(nd)).Equals("drct"))
				{
					globalConst.PropForm.CurPropTag.Enabled=true;
					globalConst.PropForm.CurPropTag.Items.Clear();
					globalConst.PropForm.CurPropTag.Items.Add("Directory Property");
					globalConst.PropForm.CurPropTag.SelectedIndex=0;
					globalConst.PropForm.ProOthers.Enabled=false;
					SiteMatrix.PropertySpace.Site.PropertyDirectory.dirid=tree.getID(nd);
					globalConst.PropForm.propertyGrid1.SelectedObject=SiteMatrix.PropertySpace.Site.PropertyDirectory.Bag();
					return;
				}
				if(tree.getTypeFromID(tree.getID(nd)).Equals("page"))
				{
					globalConst.PropForm.CurPropTag.Enabled=true;
					globalConst.PropForm.CurPropTag.Items.Clear();
					globalConst.PropForm.CurPropTag.Items.Add("Page Property");
					globalConst.PropForm.CurPropTag.SelectedIndex=0;
					globalConst.PropForm.ProOthers.Enabled=false;
					SiteMatrix.PropertySpace.Site.PropertyPage.pageid=tree.getID(nd);
					globalConst.PropForm.propertyGrid1.SelectedObject=SiteMatrix.PropertySpace.Site.PropertyPage.Bag();
					return;
				}

				if(tree.getTypeFromID(tree.getID(nd)).Equals("part"))
				{
					globalConst.PropForm.CurPropTag.Enabled=true;
					globalConst.PropForm.CurPropTag.Items.Clear();
					globalConst.PropForm.CurPropTag.Items.Add("Part Property");
					globalConst.PropForm.CurPropTag.SelectedIndex=0;
					globalConst.PropForm.ProOthers.Enabled=false;
					SiteMatrix.PropertySpace.Site.PropertyPart.doPartProperty(tree.getID(nd));
					return;
				}
			}
		}

		private void ctrlTree_MouseDown(object sender, MouseEventArgs e)
		{
			Point p=new Point(e.X,e.Y);
			TreeNode nd= ctrlTree.GetNodeAt(p);
			if(nd!=null)
			{
				ctrlTree.SelectedNode=nd;
				
				if(tree.getTypeFromID(tree.getID(nd)).Equals("part"))
				{
					globalConst.PropForm.CurPropTag.Enabled=true;
					globalConst.PropForm.CurPropTag.Items.Clear();
					globalConst.PropForm.CurPropTag.Items.Add("Part Property");
					globalConst.PropForm.CurPropTag.SelectedIndex=0;
					globalConst.PropForm.ProOthers.Enabled=false;
					siteTree.SelectedNode=null;
					SiteMatrix.PropertySpace.Site.PropertyPart.doPartProperty(tree.getID(nd));
					return;
				}
				if(tree.getTypeFromID(tree.getID(nd)).Equals("ctrl"))
				{
					globalConst.PropForm.CurPropTag.Enabled=true;
					globalConst.PropForm.CurPropTag.Items.Clear();
					globalConst.PropForm.CurPropTag.Items.Add("Control Property");
					globalConst.PropForm.CurPropTag.SelectedIndex=0;
					globalConst.PropForm.ProOthers.Enabled=false;
					siteTree.SelectedNode=null;
					SiteMatrix.PropertySpace.Site.PropertyControl.doControlProperty(tree.getID(nd));
					return;
				}
				if(tree.getTypeFromID(tree.getID(nd)).Equals("comp"))
				{
					globalConst.PropForm.CurPropTag.Enabled=true;
					globalConst.PropForm.CurPropTag.Items.Clear();
					globalConst.PropForm.CurPropTag.Items.Add("ControlMother Property");
					globalConst.PropForm.CurPropTag.SelectedIndex=0;
					globalConst.PropForm.ProOthers.Enabled=false;
					siteTree.SelectedNode=null;
					SiteMatrix.PropertySpace.Site.PropertyControlMother.doControlProperty(tree.getID(nd));
					return;
				}
				if(tree.getTypeFromID(tree.getID(nd)).Equals("page"))
				{
					globalConst.PropForm.CurPropTag.Enabled=true;
					globalConst.PropForm.CurPropTag.Items.Clear();
					globalConst.PropForm.CurPropTag.Items.Add("Page Property");
					globalConst.PropForm.CurPropTag.SelectedIndex=0;
					globalConst.PropForm.ProOthers.Enabled=false;
					TreeNode snd=tree.getSiteNodeByID(tree.getID(nd));
					if(snd==null)
					{
						MsgBox.Warning(tree.getID(nd) + " not found in sitetree!");
						globalConst.PropForm.propertyGrid1.SelectedObject=null;
					}
					else
					{
						siteTree.SelectedNode=snd;	
						SiteMatrix.PropertySpace.Site.PropertyPage.pageid=tree.getID(nd);
						globalConst.PropForm.propertyGrid1.SelectedObject=SiteMatrix.PropertySpace.Site.PropertyPage.Bag();
					}
					return;
				}

			}
		}

		public static ImageForm.ImageForm imgform;
		public static void CloseImageForm()
		{
			try
			{
				if(imgform != null)
				{
					globalConst.MdiForm.Controls.Remove(imgform);
					imgform = null;
				}
			}
			catch(Exception ex)
			{
			new error(ex);
			}
		}
		private void treeView_ItemDrag(object sender,
			System.Windows.Forms.ItemDragEventArgs e)
		{
			//DoDragDrop(e.Item, DragDropEffects.Move);
			if(!tree.getTypeFromID(tree.getID(ctrlTree.SelectedNode)).Equals("part"))
			{
			return;
			}
			TreeNode dragNode = (TreeNode)e.Item;
			CloseImageForm();
			//imgform = new ImageForm.ImageForm((Bitmap)ctrlTree.ImageList.Images[dragNode.ImageIndex],dragNode.Text);
			//form.Load +=new EventHandler(new ImageForm().Form2_Load);
			imgform.TopLevel = false;
			imgform.Visible = true;
			//imgform.Show();
			//this.Parent.Show().Controls(imgform);
			globalConst.MdiForm.Controls.Add(imgform);
			//this.Controls.Add(imgform);
			imgform.BringToFront();
			classes.EditorEvent.AddEvents();
			ctrlTree.DoDragDrop(e.Item,DragDropEffects.Move);
		} 
		private void treeView_DragEnter(object sender,
			System.Windows.Forms.DragEventArgs e)
		{
			classes.EditorEvent.DragE=e;
			e.Effect = DragDropEffects.Move;
		} 
		

		private void treeView3_DragOver(object sender, DragEventArgs e)
		{
			
			//Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
			imgform.Visible = true;
			//imgform.Location = new Point(pt.X + 18,pt.Y + 6);
			imgform.Location = new Point(e.X -globalConst.MdiForm.Left,e.Y -globalConst.MdiForm.Top-50);
			imgform.BringToFront();
			e.Effect = DragDropEffects.Move;
//			TreeNode overNode = ((TreeView)sender).GetNodeAt(pt);
////			treeView1.SelectedNode = overNode;
//			if(overNode == null || dragNode.Equals(overNode))
//			{
//				e.Effect = DragDropEffects.None;
//			}
//			else
//			{
//				e.Effect = DragDropEffects.Move;
//				
//    
//			}
		}
		private void treeView3_DragLeave(object sender, System.EventArgs e)
		{
			imgform.Visible = false;
		}



		private void ctrlTree_GiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			if(e.Effect == DragDropEffects.Move) 
			{
				// Show pointer cursor while dragging
				e.UseDefaultCursors = false;
				ctrlTree.Cursor = Cursors.Default;
			}
			else e.UseDefaultCursors = true;
		}

		private void ctrlTree_DragDrop(object sender, DragEventArgs e)
		{
			classes.EditorEvent.MoveEvents();
			CloseImageForm();
		}
	}
	}
