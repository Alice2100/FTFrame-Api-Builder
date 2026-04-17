using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SiteMatrix.classes;
using mshtml;
using SiteMatrix.Adapter;
using System.Drawing.Design;
using SiteMatrix.functions;
using SiteMatrix.consts;
using System.IO;
using SiteMatrix.forms;

namespace SiteMatrix.Adapter
{
	/// <summary>
	/// PartImage 的摘要说明。
	/// </summary>
[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
	public class PartImage : System.Windows.Forms.Form
	{
		public static string thisURL;
		public static string thisName;
		public static string thisPartID;
		public string imagestring;
		public PropertyGrid propertyGrid1 = new PropertyGrid();
		public bool isCancel=true;
		public IHTMLElement Ele;
		public static string _text="";
		public static bool returnString=false;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private htmleditocx.D4HtmlEditOcx editocx;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PartImage()
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.editocx = new htmleditocx.D4HtmlEditOcx();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(200, 272);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 24);
            this.button1.TabIndex = 1;
            this.button1.Text = "返回图片";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(456, 272);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 24);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // editocx
            // 
            this.editocx.alinkColor = "#0000ff";
            this.editocx.bgColor = "#ffffff";
            this.editocx.cookie = null;
            this.editocx.isDesignMode = true;
            this.editocx.linkColor = "#0000ff";
            this.editocx.Location = new System.Drawing.Point(240, 5);
            this.editocx.Name = "editocx";
            this.editocx.ReadOnlyWhenApplyHTML = false;
            this.editocx.showBodyNetCells = false;
            this.editocx.showGlyphs = false;
            this.editocx.showLiveResizes = false;
            this.editocx.showTableBorders = true;
            this.editocx.Size = new System.Drawing.Size(295, 250);
            this.editocx.TabIndex = 3;
            this.editocx.url = "about:blank";
            this.editocx.onselectionchange += new htmleditocx.D4HtmlEditOcx.onselectionchangeEventHandler(this.editocx_onselectionchange);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(16, 272);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 24);
            this.button3.TabIndex = 4;
            this.button3.Text = "浏览";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(104, 272);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 24);
            this.button4.TabIndex = 5;
            this.button4.Text = "清除";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(296, 272);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 24);
            this.button5.TabIndex = 6;
            this.button5.Text = "返回字符串";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // PartImage
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(544, 304);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.editocx);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PartImage";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PartImage";
            this.Load += new System.EventHandler(this.PartImage_Load);
            this.ResumeLayout(false);

		}
		#endregion
        private void ApplyLanguage()
        {
            this.Text = res.PartImage.GetString("_this");
            button3.Text = res.PartImage.GetString("button3");
            button4.Text = res.PartImage.GetString("button4");
            button1.Text = res.PartImage.GetString("button1");
            button5.Text = res.PartImage.GetString("button5");
            button2.Text = res.PartImage.GetString("button2");
        }
		private void PartImage_Load(object sender, System.EventArgs e)
		{
			propertyGrid1.CommandsVisibleIfAvailable = true;
			propertyGrid1.Location = new Point(5, 5);
			propertyGrid1.Size = new System.Drawing.Size(235, 250);
			propertyGrid1.TabIndex = 1;
			propertyGrid1.Text = "Property Grid";
			propertyGrid1.LargeButtons = false;
			propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
			propertyGrid1.Name = "propertyGrid1";
			propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;

			this.Controls.Add(propertyGrid1);
//			editocx.Location = new System.Drawing.Point(305, 5);
//			editocx.Name = "editocx";
//			editocx.showBodyNetCells = false;
//			editocx.showGlyphs = false;
//			editocx.showLiveResizes = false;
//			editocx.showTableBorders = true;
//			editocx.Size = new System.Drawing.Size(310, 250);
//			editocx.TabIndex = 0;
//			this.Controls.Add(editocx);
			
			Application.DoEvents();
			//下面这段代码你能看懂吗，不懂请联系Alice
			if(!imagestring.ToLower().StartsWith("<img"))
			{
				imagestring=imagestring.Replace("\"","");
				PartImage._text=imagestring;
				imagestring="<img src=\"" + imagestring + "\">";
			}
			string s="";
			try
			{
				s=imagestring;
				int i=s.IndexOf(" src=\"");
				if(i<0)return;
				s=s.Substring(i+6,s.Length-i-6);
				i=s.IndexOf("\"");
				s=s.Substring(0,i);
			}
			catch
			{
				s="";
			}
			if(!s.Equals(""))
			{
				if(!s.ToLower().StartsWith("http://")&&!s.ToLower().StartsWith("ftp://"))
				{
					if(s.StartsWith("/lib") || s.StartsWith("\\lib"))
					{
						imagestring=imagestring.Replace("src=\"","src=\"" + globalConst.AppPath);
					}
					else if(s.StartsWith("/") || s.StartsWith("\\"))
					{
						imagestring=imagestring.Replace("src=\"","src=\"" + globalConst.CurSite.Path);
					}
					else
					{
						//if not page part property set , src replacing may be have a small bug
						//						Editor ed=form.getEditor();
						//						if(ed==null || ed.Focused==false)
						//						if(tree.getTypeFromID(tree.getID(Workspace.siteTree.SelectedNode)).Equals("part"))
						//						{
						//							if(form.getEditor(tree.getID(Workspace.siteTree.SelectedNode.Parent))==null)
						//							{
						//								
						//							}
						//						}
						string thisurl=thisURL;
						string thisname=thisName;
						if(!thisURL.Equals(""))
						{
							imagestring=imagestring.Replace("src=\"","src=\"" + thisurl.Substring(0,thisurl.Length-thisname.Length));
						}
					}
				}
			}
			editocx.LoadDocument(globalConst.ConfigPath + "\\empty.htm");
			//editocx.RefreshDocument();
			
			while(editocx.readyState().Equals("loading"))
			{
				Application.DoEvents();
			}
			IHTMLElement ie=null;
			//new MsgBox(editocx.body.outerHTML);
			//new MsgBox(((IHTMLDocument2)editocx.DOM).body.outerHTML);
			((IHTMLDocument2)editocx.DOM).body.innerHTML=imagestring;
			IHTMLElementCollection iec=(IHTMLElementCollection)(((IHTMLDocument2)editocx.DOM).all);
			foreach(IHTMLElement iee in iec)
			{
				if(iee.tagName.ToLower().Equals("img"))
				{
					ie=iee;
					goto ExitFor;
				}
			}
			ExitFor:
			Ele=ie;
			this.propertyGrid1.SelectedObject=new PropertyImage.ImageElement(Ele);
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			isCancel=false;
			PartImage.returnString=false;
			this.Close();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}




		private void editocx_onselectionchange()
		{
			this.propertyGrid1.Focus();
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog ofd=new OpenFileDialog();
			ofd.Filter="Images(*.gif;*.jpg;*.jpeg)|*.gif;*.jpg;*.jpeg|Images(*.bmp;*.png)|*.bmp;*.png|All Files(*.*)|*.*";
			ofd.ShowDialog();
			string FileName=ofd.FileName.Trim();
			if(!FileName.Equals(""))
			{
				if(file.Exists(FileName))
				{
//					string resdir=thisURL + ".res";
//					string thisname=thisName;
					string resdir=globalConst.CurSite.Path + @"\control.resource\" + thisPartID.Substring(0,thisPartID.Length-5);
					if(!dir.Exists(resdir))
					{
						dir.CreateDirectory(resdir);
					}
					FileInfo fi = new FileInfo(FileName);
					string newname=fi.Name;
					if(!FileName.ToLower().StartsWith(resdir.ToLower()))
					{
						if(file.Exists(resdir + @"\" + newname))
						{
							int i=1;
							while(file.Exists(resdir + @"\" + i + newname))
							{
							i++;
							}
							newname=i + newname;
						}
						file.Copy(FileName,resdir + @"\" + newname);
					}
					((IHTMLImgElement)Ele).src=resdir + @"\" + newname;
					this.propertyGrid1.SelectedObject=new PropertyImage.ImageElement(Ele);
				}
			}
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			//Ele.outerHTML="<img src=\"\">";
			((IHTMLImgElement)Ele).src="";
			Ele.removeAttribute("height",0);
			Ele.removeAttribute("width",0);
			this.propertyGrid1.SelectedObject=new PropertyImage.ImageElement(Ele);
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			isCancel=false;
			PartImage.returnString=true;
			this.Close();
		}


	}
	[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
	public class PropertyImage
	{
        public static string NotSet = res.PartImage.GetString("NotSet");	
		public static string _text="";
		public class PropertyAdapter
		{
		
			public PropertyAdapter()
			{
				System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
			}
			public static string getEleAttr(IHTMLElement e,string name)
			{
				System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
				if(e.getAttribute(name,0)==null)return "";
				return e.getAttribute(name,0).ToString();
			}
			public static void setEleAttr(IHTMLElement e,string name,string _value)
			{
				System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
				if(e.getAttribute(name,0)!=null)
				{
					if(_value.Equals("")||_value==null)
					{
						e.removeAttribute(name,0);
						return;
					}
					e.setAttribute(name,_value,0);
					return;
				}
				if(_value.Equals("")||_value==null)return;
				e.setAttribute(name,_value,0);
			}
		
		}
		public class ImageElement
		{
			private IHTMLElement e;
			public ImageElement(IHTMLElement ele)
			{
				e=ele;
				//PartImage._text=PropertyAdapter.getEleAttr(e,"src");
			}
			[DescriptionAttribute("HTML String"),
			CategoryAttribute("String Setting")]
			public string Text
			{
				get 
				{
					return PartImage._text;
				}
				set
				{
					PartImage._text=value;
				}
			}
            [DescriptionAttribute("Fires when the user clicks the left mouse button on the object."),
            CategoryAttribute("Events")]
			public string onClick
			{
				get 
				{
					return PropertyAdapter.getEleAttr(e,"onClick");
				}
				set
				{
					PropertyAdapter.setEleAttr(e,"onClick",value);
				}
			}
            [DescriptionAttribute("Fires when the user double-clicks the object."),
            CategoryAttribute("Events")]
			public string onDblClick
			{
				get 
				{
					return PropertyAdapter.getEleAttr(e,"onDblClick");
				}
				set
				{
					PropertyAdapter.setEleAttr(e,"onDblClick",value);
				}
			}
            [DescriptionAttribute("Fires when the object receives focus."),
            CategoryAttribute("Events")]
			public string onFocus
			{
				get 
				{
					return PropertyAdapter.getEleAttr(e,"onFocus");
				}
				set
				{
					PropertyAdapter.setEleAttr(e,"onFocus",value);
				}
			}
            [DescriptionAttribute("Fires when the user presses an alphanumeric key."),
            CategoryAttribute("Events")]
			public string onKeyPress
			{
				get 
				{
					return PropertyAdapter.getEleAttr(e,"onKeyPress");
				}
				set
				{
					PropertyAdapter.setEleAttr(e,"onKeyPress",value);
				}
			}
            [DescriptionAttribute("Fires when the user clicks the object with either mouse button."),
           CategoryAttribute("Events")]
			public string onMouseDown
			{
				get 
				{
					return PropertyAdapter.getEleAttr(e,"onMouseDown");
				}
				set
				{
					PropertyAdapter.setEleAttr(e,"onMouseDown",value);
				}
			}
            [DescriptionAttribute("Fires when the user moves the mouse pointer outside the boundaries of the object. "),
            CategoryAttribute("Events")]
			public string onMouseOut
			{
				get 
				{
					return PropertyAdapter.getEleAttr(e,"onMouseOut");
				}
				set
				{
					PropertyAdapter.setEleAttr(e,"onMouseOut",value);
				}
			}
            [DescriptionAttribute("Fires when the user moves the mouse pointer into the object."),
           CategoryAttribute("Events")]
			public string onMouseOver
			{
				get 
				{
					return PropertyAdapter.getEleAttr(e,"onMouseOver");
				}
				set
				{
					PropertyAdapter.setEleAttr(e,"onMouseOver",value);
				}
			}
            [DescriptionAttribute("Fires when the user releases a mouse button while the mouse is over the object."),
            CategoryAttribute("Events")]
			public string onMouseUp
			{
				get 
				{
					return PropertyAdapter.getEleAttr(e,"onMouseUp");
				}
				set
				{
					PropertyAdapter.setEleAttr(e,"onMouseUp",value);
				}
			}
            [DescriptionAttribute("Sets or retrieves the class of the object."),
            CategoryAttribute("Style")]
			public string @class
			{
				get 
				{
					return e.className;
				}
				set
				{
					e.className=value;
				}
			}
			[EditorAttribute(typeof(StyleEditor),typeof(UITypeEditor)), 
			TypeConverterAttribute(typeof(StyleEditorConverter)),
           DescriptionAttribute("Set style at Style Builder."),
           CategoryAttribute("Style")]
			public object @style
			{
				get 
				{   
					return e;
				}
				set
				{
				
				}
			}
            [DescriptionAttribute("Sets or retrieves the height of the object."),
            CategoryAttribute("Layout")]
			public int @height
			{
				get 
				{
					return ((HTMLImg)(e)).height;
				}
				set
				{
					((HTMLImg)(e)).height=value;
				}
			}
            [DescriptionAttribute("Sets or retrieves the calculated width of the object."),
            CategoryAttribute("Layout")]
			public int @width
			{
				get 
				{
					return ((HTMLImg)(e)).width;
				}
				set
				{
					((HTMLImg)(e)).width=value;
				}
			}
			[TypeConverter(typeof(Property.alignFieldSetConverter)),
           DescriptionAttribute("Sets or retrieves how the object is aligned with adjacent text."),
           CategoryAttribute("Appearance")]
			public string @align
			{
				get
				{
				
					if(((HTMLImg)(e)).align==null)return NotSet;
					return ((HTMLImg)(e)).align;
				
				}
				set
				{
					if(NotSet.Equals(value))value=null;
					((HTMLImg)(e)).align=value;
				}
			}
            [DescriptionAttribute("Sets or retrieves a text alternative to the graphic."),
            CategoryAttribute("Appearance")]
			public string @alt
			{
				get 
				{
					return ((HTMLImg)(e)).alt;
				}
				set
				{
					((HTMLImg)(e)).alt=value;
				}
			}
            [DescriptionAttribute("Sets or retrieves the width of the border to draw around the object."),
            CategoryAttribute("Appearance")]
			public string @border
			{
				get 
				{
					if(((HTMLImg)(e)).border==null)return "";
					return ((HTMLImg)(e)).border.ToString();
				}
				set
				{
					((HTMLImg)(e)).border=value;
				}
			}
            [DescriptionAttribute("Sets or retrieves the horizontal margin for the object."),
            CategoryAttribute("Appearance")]
			public int @hSpace
			{
				get 
				{
					return ((HTMLImg)(e)).hspace;
				}
				set
				{
					((HTMLImg)(e)).hspace=value;
				}
			}
            [DescriptionAttribute("Sets or retrieves the vertical margin for the object."),
            CategoryAttribute("Appearance")]
			public int @vSpace
			{
				get 
				{
					return ((HTMLImg)(e)).vspace;
				}
				set
				{
					((HTMLImg)(e)).vspace=value;
				}
			}
            [DescriptionAttribute("Sets or retrieves the accelerator key for the object."),
            CategoryAttribute("Behavior")]
			public string @accessKey
			{
				get 
				{
					return PropertyAdapter.getEleAttr(e,"accessKey");
				}
				set
				{
					PropertyAdapter.setEleAttr(e,"accessKey",value);
				}
			}
            [DescriptionAttribute("Sets or retrieves the value indicating whether the object visibly indicates that it has focus."),
            CategoryAttribute("Behavior")]
			public bool @hideFocus
			{
				get 
				{
					return ((HTMLImg)(e)).hideFocus;
				}
				set
				{
					((HTMLImg)(e)).hideFocus=value;
				}
			}

            [DescriptionAttribute("Sets or retrieves the index that defines the tab order for the object."),
            CategoryAttribute("Behavior")]
			public short @tabIndex
			{
				get 
				{
					return ((HTMLImg)(e)).tabIndex;
				}
				set
				{
					((HTMLImg)(e)).tabIndex=value;
				}
			}
            [DescriptionAttribute("Retrieves the string identifying the object."),
            CategoryAttribute("Misc")]
			public string @id
			{
				get 
				{
					return e.id;
				}
				set
				{
					e.id=value;
				}
			}
            [DescriptionAttribute("Sets or retrieves a URL to be loaded by the object."), CategoryAttribute("Misc")]
			public string @src
			{
				get 
				{
					return ((HTMLImg)(e)).src;
				}
				set
				{
					((HTMLImg)(e)).src=value;
				}
			}
            [DescriptionAttribute("Sets or retrieves advisory information (a ToolTip) for the object."), CategoryAttribute("Misc")]
            public string @title
			{
				get 
				{
					return PropertyAdapter.getEleAttr(e,"title");
				}
				set
				{
					PropertyAdapter.setEleAttr(e,"title",value);
				}
			}
		}
	}
}
