using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SiteMatrix.SiteClass;
using SiteMatrix.consts;
using SiteMatrix.functions;
using mshtml;
namespace SiteMatrix.forms
{
	/// <summary>
	/// Workspace 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
	public class Property : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl WorkTab;
		private System.Windows.Forms.TabPage tabWare;
		private System.Windows.Forms.TabPage tabPage;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.TabPage tabProp;
		private System.ComponentModel.IContainer components;
		public System.Windows.Forms.ComboBox ProOthers;
		public System.Windows.Forms.ComboBox CurPropTag;
		public mshtml.IHTMLElement CurEle=null;
		public bool HasJustSelectChange=false;
		public bool HasJustFromAdapter=false;
		public PropertyGrid propertyGrid1 = new PropertyGrid();
		public Property()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
			InitializeComponent();
			
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Property));
			this.WorkTab = new System.Windows.Forms.TabControl();
			this.tabProp = new System.Windows.Forms.TabPage();
			this.ProOthers = new System.Windows.Forms.ComboBox();
			this.CurPropTag = new System.Windows.Forms.ComboBox();
			this.tabWare = new System.Windows.Forms.TabPage();
			this.tabPage = new System.Windows.Forms.TabPage();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.WorkTab.SuspendLayout();
			this.tabProp.SuspendLayout();
			this.SuspendLayout();
			// 
			// WorkTab
			// 
			this.WorkTab.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.WorkTab.Controls.Add(this.tabProp);
			this.WorkTab.Controls.Add(this.tabWare);
			this.WorkTab.Controls.Add(this.tabPage);
			this.WorkTab.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
			this.WorkTab.ImageList = this.imageList1;
			this.WorkTab.Location = new System.Drawing.Point(0, 8);
			this.WorkTab.Name = "WorkTab";
			this.WorkTab.SelectedIndex = 0;
			this.WorkTab.Size = new System.Drawing.Size(256, 312);
			this.WorkTab.TabIndex = 0;
			// 
			// tabProp
			// 
			this.tabProp.Controls.Add(this.ProOthers);
			this.tabProp.Controls.Add(this.CurPropTag);
			this.tabProp.ImageIndex = 0;
			this.tabProp.Location = new System.Drawing.Point(4, 4);
			this.tabProp.Name = "tabProp";
			this.tabProp.Size = new System.Drawing.Size(248, 285);
			this.tabProp.TabIndex = 0;
			this.tabProp.Text = "Property";
			this.tabProp.Click += new System.EventHandler(this.tabProp_Click);
			// 
			// ProOthers
			// 
			this.ProOthers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ProOthers.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
			this.ProOthers.Location = new System.Drawing.Point(8, 32);
			this.ProOthers.Name = "ProOthers";
			this.ProOthers.Size = new System.Drawing.Size(232, 20);
			this.ProOthers.TabIndex = 1;
			this.ProOthers.SelectedIndexChanged += new System.EventHandler(this.ProOthers_SelectedIndexChanged);
			// 
			// CurPropTag
			// 
			this.CurPropTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CurPropTag.Location = new System.Drawing.Point(8, 8);
			this.CurPropTag.Name = "CurPropTag";
			this.CurPropTag.Size = new System.Drawing.Size(232, 20);
			this.CurPropTag.TabIndex = 0;
			this.CurPropTag.SelectedIndexChanged += new System.EventHandler(this.CurPropTag_SelectedIndexChanged);
			// 
			// tabWare
			// 
			this.tabWare.ImageIndex = 0;
			this.tabWare.Location = new System.Drawing.Point(4, 4);
			this.tabWare.Name = "tabWare";
			this.tabWare.Size = new System.Drawing.Size(248, 285);
			this.tabWare.TabIndex = 1;
			this.tabWare.Text = "Opened";
			// 
			// tabPage
			// 
			this.tabPage.ImageIndex = 0;
			this.tabPage.Location = new System.Drawing.Point(4, 4);
			this.tabPage.Name = "tabPage";
			this.tabPage.Size = new System.Drawing.Size(248, 285);
			this.tabPage.TabIndex = 2;
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// Property
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(258, 328);
			this.Controls.Add(this.WorkTab);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Property";
			this.ShowInTaskbar = false;
			this.Text = "Properties";
			this.Resize += new System.EventHandler(this.Property_Resize);
			this.Load += new System.EventHandler(this.Property_Load);
			this.WorkTab.ResumeLayout(false);
			this.tabProp.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void tabProp_Click(object sender, System.EventArgs e)
		{
		
		}

		private void Property_Load(object sender, System.EventArgs e)
		{
			propertyGrid1.CommandsVisibleIfAvailable = true;
			propertyGrid1.Location = new Point(10, 48);
			propertyGrid1.Size = new System.Drawing.Size(275, 234);
			propertyGrid1.TabIndex = 1;
			propertyGrid1.Text = "Property Grid";
			propertyGrid1.LargeButtons = false;
			propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
			propertyGrid1.Name = "propertyGrid1";
			propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;

			this.tabProp.Controls.Add(propertyGrid1);
			globalConst.PropForm=this;
			this.CurPropTag.Enabled=false;
			this.ProOthers.Enabled=false;
			doResize();
		}

		private void Property_Resize(object sender, System.EventArgs e)
		{
			doResize();
		}
		private void doResize()
		{
			this.WorkTab.Width=this.Width;
			propertyGrid1.Width=this.Width-25;
		}

		private void ProOthers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		if(CurEle==null)return;
		mshtml.IHTMLElement PEle=CurEle;
		int pCount=ProOthers.SelectedIndex;
		if(pCount==0)return;
		int i;
			for(i=0;i<pCount;i++)
			{
			if(PEle!=null)PEle=PEle.parentElement;
			}
			if(PEle!=null)
			{
			mshtml.IHTMLTxtRange it=form.getEditor().INITxtRange;
			CurEle=PEle;
			it.moveToElementText(PEle);
			
//			//it.scrollIntoView(false);
			HasJustSelectChange=true;
			it.select();
			HasJustSelectChange=false;
			//Page.PageWare.doHtmlAdapter(PEle);
			}
		}

		private void CurPropTag_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(HasJustFromAdapter)
			{
			HasJustFromAdapter=false;
				return;
			}
			if(CurPropTag.SelectedItem.ToString().StartsWith("[Control]"))
			{
				string partid=((imageComboBoxItem)CurPropTag.SelectedItem).ImageIndex;
				Editor edr=form.getEditor();
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
								CurEle=ihe;
								IHTMLTxtRange it=edr.INITxtRange;
								it.moveToElementText(ihe);
								HasJustSelectChange=true;
								it.select();
								HasJustSelectChange=false;
								goto LoopOut;
							}
						}
					}
					//}
				}
			LoopOut:
				;
			}
		}
	}
	public class imageComboBoxItem 
	{ 
		//定义文本属性 
		private string _text; 
		public string Text 
		{ 
			get {return _text;} 
			set {_text = value;} 
		} 
		//定义图象索引属性 
		private string _imageIndex; 
		public string ImageIndex 
		{ 
			get {return _imageIndex;} 
			set {_imageIndex = value;} 
		} 
		//初始化函数之一：即没有图象也没有文本 
		public imageComboBoxItem():this("","") 
		{ 
		} 
		//初始化函数之二：没有图象，只有文本（针对不知属性哪一年级学生） 
		public imageComboBoxItem(string text): this(text, "") 
		{ 
		} 
		//初始化函数之三：文本与图象都有 
		public imageComboBoxItem(string text, string imageIndex) 
		{ 
			_text = text; 
			_imageIndex = imageIndex; 
		} 

		public override string ToString() 
		{ 
			return _text; 
		} 
	} 

}
