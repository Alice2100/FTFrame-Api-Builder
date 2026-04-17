using System;
using System.Windows.Forms;
using SiteMatrix.database;
using System.Xml;
using SiteMatrix.forms;
using SiteMatrix.Resource;
using System.Collections;
using System.Data.OleDb;

namespace SiteMatrix.consts
{
	/// <summary>
	/// globalconst 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
	public class globalConst
	{
        public static System.Drawing.Image DefaultControlIcon;
        public static System.Drawing.Image TagsIcon;
        public static ImageList ControlsImages;
        public static ControlIcons[] ControlIcon;
		public static ImageList Imgs;
		public static MainForm MdiForm;
		public static Property PropForm;
		public static string AppPath;
		public static string AppFile;
		public static string SitesPath;
        public static string TemplatePath;
        public static string FreeFilesPath;
		public static string ImgsPath;
		public static string ConfigPath;
		public static string LibPath;
		public static string ConfigFile;
		public static string emptyFile;
		public static string emptyTxtFile;
        public static string defaultCssFile;
		public static DB ConfigConn;
        public static string CurSelectionPage;
		//public static XmlDocument CssEleDoc;
		//public static XmlDocument CssTypeDoc;
		public static Form curActiveForm;
		public static System.Drawing.Text.InstalledFontCollection SysFonts;
		public static string Language;
	    //Developer Version or FullVersion
        public const bool FullVersion = true;
        //public const bool FullVersion = false;
        public static string FullLeft = "风娜江雪";
        public static string FullRight = "山厉小雨";
        public static string OLEDBProvider = System.Configuration.ConfigurationManager.AppSettings["OLEDBProvider"];
        public static string FullAll = "8bGijaehruoiP+1WV9P87A==";
        public static bool FormDataMode = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["FormDataMode"]);
        public static ArrayList TextEditorControlProp = null;
        public static ArrayList SqlEditorControlProp = null;
		public struct CurSite
		{
		public static string ID=null;
		public static string Domain=null;
		public static string Caption=null;
		public static string Path=null;
        public static string URL = null;
		public static DB SiteConn;
            public static DB SiteConn2;
            public static XmlDocument FormDataXML = null;
		//add others here
		}
		public static string ProductName=System.Windows.Forms.Application.ProductName;
		public static string CompanyName=System.Windows.Forms.Application.CompanyName;
		public struct PageWare
		{
            public static string getPortalHead()
            {
                string s = "";
                s += "<table width=100% height=100% border=0 cellpadding=0 cellspacing=0><tr bgcolor=lightblue height=1px><td ";
                    s += "width=100%>&nbsp;</td><td align=right><img src=\"" + globalConst.ImgsPath + "\\edit.gif\" border=0 alt=\"edit\" ";
                    s += "style=\"cursor:hand\">&nbsp;<img src=\"" + globalConst.ImgsPath + "\\delete.gif\" border=0  style=\"cursor:hand\" ";
                    s += "alt=\"hidden\">&nbsp;<img src=\"" + globalConst.ImgsPath + "\\collapse.gif\" border=0  ";
                    s += "style=\"cursor:hand\" alt=\"collapse\" ";
                    s += ">&nbsp;</td></tr><tr ";
                    s += "><td colspan=2>";
                return s;
            }
            public static string getPortalTail()
            {
                string s = "";
                    s+= "</td></tr></table>";
                return s;
            }
			public static string getControlEditHead(string IdName,string PartName,string Width,string Height, string WareName=null)
			{
				System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
                if(WareName==null)
                {
                    string sql = "select b.name from parts a,controls b where a.id='" + IdName + "' and a.controlid=b.id";
                    OleDbDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                    if (rdr.Read())
                    {
                        WareName = rdr.GetString(0);
                    }
                    rdr.Close();
                }
                if (Width != null && Width.Equals("1px") && Height!=null&&Height.Equals("1px"))
				{
					Width="100%";
				}
                return "<span id=\"dotforsitecom\" name=\"dotforsitecom\" idname=\"" + IdName + "\" controlname=\""+ WareName + "\" partname=\"" + PartName + "\" style=\"BORDER-RIGHT: black 1px dotted; BORDER-TOP: black 1px dotted; BORDER-LEFT: black 1px dotted; " + (Width == null ? "" : "WIDTH: " + Width + ";") + " BORDER-BOTTOM: black 1px dotted; " + (Height == null ? "" : "HEIGHT: " + Height + ";") + "cursor:url(" + globalConst.AppPath + "\\img\\mycur.cur)\">";
			}
			public static string getControlEditTail()
			{
				return "</span>";
			}
			public static string getControlViewHead(string Width,string Height)
			{
				System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
                return "<span style=\"" + (Width == null ? "" : "WIDTH: " + Width + ";") + (Height == null ? "" : "HEIGHT: " + Height + ";") + "\">";
			}
			public static string getControlViewTail()
			{
				return "</span>";
			}
		}
		public static string siteTreeOrderby;
		public static string siteTreeShowColName;
		public static string ctrlTreeOrderby;
		public static string ctrlTreeOrdertype;
		public static string ctrlTreeShowColName;
		public static int LogLevel=3;//log level,3:error,debug,info
		public static string LogOutputPath;
		public globalConst()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
	}
    public class DefaultConst
    {
        //i==1,true;i==0,false
        public static string GetBoolCaption(int i)
        {
            if (i == 1) return res._globalconst.GetString("Yes");
            else return res._globalconst.GetString("No");
        }
        public static int GetBoolCaption(string caption)
        {
            if (caption.Equals(res._globalconst.GetString("Yes"))) return 1;
            else return 0;
        }
        public static string[] getFormTableRowDataType()
        {
            return new string[] { "varchar", "tinyint", "int", "decimal", "text", "datetime" };
        }
        public static string getFormTableDefaultRowsXML()
        {
            string s = "";
            s += "<row id=\"fid\" caption=\"表单ID\" lock=\"true\">\r\n";
            s += "			<datatype name=\"varchar\">\r\n";
            s += "				<length>36</length>\r\n";
            s += "				<numpoint>0</numpoint>\r\n";
            s += "			</datatype>\r\n";
            s += "			<allownull>false</allownull>\r\n";
            s += "			<default></default>\r\n";
            s += "			<primary>true</primary>\r\n";
            s += "			<index>true</index>\r\n";
            s += "			<bindinfo>\r\n";
            s += "				<isbind>false</isbind>\r\n";
            s += "				<page></page>\r\n";
            s += "			</bindinfo>\r\n";
            s += "		</row>\r\n";
            s += "		<row id=\"pid\" caption=\"父级表单ID\" lock=\"true\">\r\n";
            s += "			<datatype name=\"varchar\">\r\n";
            s += "				<length>36</length>\r\n";
            s += "				<numpoint>0</numpoint>\r\n";
            s += "			</datatype>\r\n";
            s += "			<allownull>true</allownull>\r\n";
            s += "			<default>NULL</default>\r\n";
            s += "			<primary>false</primary>\r\n";
            s += "			<index>true</index>\r\n";
            s += "			<bindinfo>\r\n";
            s += "				<isbind>false</isbind>\r\n";
            s += "				<page></page>\r\n";
            s += "			</bindinfo>\r\n";
            s += "		</row>\r\n";
            s += "		<row id=\"fmem\" caption=\"添加的用户\" lock=\"true\">\r\n";
            s += "			<datatype name=\"varchar\">\r\n";
            s += "				<length>36</length>\r\n";
            s += "				<numpoint>0</numpoint>\r\n";
            s += "			</datatype>\r\n";
            s += "			<allownull>true</allownull>\r\n";
            s += "			<default>NULL</default>\r\n";
            s += "			<primary>false</primary>\r\n";
            s += "			<index>true</index>\r\n";
            s += "			<bindinfo>\r\n";
            s += "				<isbind>false</isbind>\r\n";
            s += "				<page></page>\r\n";
            s += "			</bindinfo>\r\n";
            s += "		</row>\r\n";
            s += "		<row id=\"modfmem\" caption=\"修改的用户\" lock=\"true\">\r\n";
            s += "			<datatype name=\"varchar\">\r\n";
            s += "				<length>36</length>\r\n";
            s += "				<numpoint>0</numpoint>\r\n";
            s += "			</datatype>\r\n";
            s += "			<allownull>true</allownull>\r\n";
            s += "			<default>NULL</default>\r\n";
            s += "			<primary>false</primary>\r\n";
            s += "			<index>true</index>\r\n";
            s += "			<bindinfo>\r\n";
            s += "				<isbind>false</isbind>\r\n";
            s += "				<page></page>\r\n";
            s += "			</bindinfo>\r\n";
            s += "		</row>\r\n";
            s += "		<row id=\"addtime\" caption=\"添加时间\" lock=\"true\">\r\n";
            s += "			<datatype name=\"datetime\">\r\n";
            s += "				<length>0</length>\r\n";
            s += "				<numpoint>0</numpoint>\r\n";
            s += "			</datatype>\r\n";
            s += "			<allownull>true</allownull>\r\n";
            s += "			<default>NULL</default>\r\n";
            s += "			<primary>false</primary>\r\n";
            s += "			<index>true</index>\r\n";
            s += "			<bindinfo>\r\n";
            s += "				<isbind>false</isbind>\r\n";
            s += "				<page></page>\r\n";
            s += "			</bindinfo>\r\n";
            s += "		</row>\r\n";
            s += "		<row id=\"updatetime\" caption=\"更新时间\" lock=\"true\">\r\n";
            s += "			<datatype name=\"datetime\">\r\n";
            s += "				<length>0</length>\r\n";
            s += "				<numpoint>0</numpoint>\r\n";
            s += "			</datatype>\r\n";
            s += "			<allownull>true</allownull>\r\n";
            s += "			<default>NULL</default>\r\n";
            s += "			<primary>false</primary>\r\n";
            s += "			<index>true</index>\r\n";
            s += "			<bindinfo>\r\n";
            s += "				<isbind>false</isbind>\r\n";
            s += "				<page></page>\r\n";
            s += "			</bindinfo>\r\n";
            s += "		</row>\r\n";
            s += "		<row id=\"dydata\" caption=\"包含动态数据\" lock=\"true\">\r\n";
            s += "			<datatype name=\"varchar\">\r\n";
            s += "				<length>255</length>\r\n";
            s += "				<numpoint>0</numpoint>\r\n";
            s += "			</datatype>\r\n";
            s += "			<allownull>true</allownull>\r\n";
            s += "			<default></default>\r\n";
            s += "			<primary>false</primary>\r\n";
            s += "			<index>true</index>\r\n";
            s += "			<bindinfo>\r\n";
            s += "				<isbind>false</isbind>\r\n";
            s += "				<page></page>\r\n";
            s += "			</bindinfo>\r\n";
            s += "		</row>\r\n";
            s += "		<row id=\"stat\" caption=\"删除状态\" lock=\"true\">\r\n";
            s += "			<datatype name=\"tinyint\">\r\n";
            s += "				<length>1</length>\r\n";
            s += "				<numpoint>0</numpoint>\r\n";
            s += "			</datatype>\r\n";
            s += "			<allownull>true</allownull>\r\n";
            s += "			<default>NULL</default>\r\n";
            s += "			<primary>false</primary>\r\n";
            s += "			<index>true</index>\r\n";
            s += "			<bindinfo>\r\n";
            s += "				<isbind>false</isbind>\r\n";
            s += "				<page></page>\r\n";
            s += "			</bindinfo>\r\n";
            s += "		</row>\r\n";
            s += "		<row id=\"flow\" caption=\"流程状态\" lock=\"true\">\r\n";
            s += "			<datatype name=\"int\">\r\n";
            s += "				<length>8</length>\r\n";
            s += "				<numpoint>0</numpoint>\r\n";
            s += "			</datatype>\r\n";
            s += "			<allownull>true</allownull>\r\n";
            s += "			<default>NULL</default>\r\n";
            s += "			<primary>false</primary>\r\n";
            s += "			<index>true</index>\r\n";
            s += "			<bindinfo>\r\n";
            s += "				<isbind>false</isbind>\r\n";
            s += "				<page></page>\r\n";
            s += "			</bindinfo>\r\n";
            s += "		</row>";
            s += "		<row id=\"flowpos\" caption=\"流程位置\" lock=\"true\">\r\n";
            s += "			<datatype name=\"int\">\r\n";
            s += "				<length>8</length>\r\n";
            s += "				<numpoint>0</numpoint>\r\n";
            s += "			</datatype>\r\n";
            s += "			<allownull>true</allownull>\r\n";
            s += "			<default>NULL</default>\r\n";
            s += "			<primary>false</primary>\r\n";
            s += "			<index>true</index>\r\n";
            s += "			<bindinfo>\r\n";
            s += "				<isbind>false</isbind>\r\n";
            s += "				<page></page>\r\n";
            s += "			</bindinfo>\r\n";
            s += "		</row>";
            return s;
        }
    }
	public class mdifromConst
	{
	    public static int windowwidth;
        public static int windowheight;
        public static int windowleft;
        public static int windowtop;
        public static string windowstate;
        public static int tooltextx;
        public static int tooltexty;
        public static int toolsitex;
        public static int toolsitey;
        public static int toolboxvisible;
        public static int toolboxwidth;
        public static int workspacevisible;
        public static int workspaceheight;
        public static int propertyvisible;
        public static int panelwidth;
        public static int tooltextvisible;
        public static int toolsitevisible;
        public static int mainmenux;
        public static int mainmenuy;
        public static int toolcommonx;
        public static int toolcommony;
        public static string sceditbgcolor;
        public static int sceditwraped;
        public static int sceditsize;
        public static string sceditfont;
        public static string curlanguage;
        public static bool LoadDefault = false;
	}
	public class res
	{
	    public static rm MainForm=new rm("SiteMatrix.resource.mainform");
        public static rm ToolBox = new rm("SiteMatrix.resource.toolbox");
        public static rm Space = new rm("SiteMatrix.resource.workspace");
        public static rm MainBox = new rm("SiteMatrix.resource.mainbox");
        public static rm Editor = new rm("SiteMatrix.resource.editor");
        public static rm TemplateManage = new rm("SiteMatrix.resource.templatemanage");
        public static rm TagEdit = new rm("SiteMatrix.resource.tagedit");
        public static rm StyleBuilder = new rm("SiteMatrix.resource.stylebuilder");
        public static rm Snippet = new rm("SiteMatrix.resource.snippet");
        public static rm SiteUpdate = new rm("SiteMatrix.resource.siteupdate");
        public static rm SiteReport = new rm("SiteMatrix.resource.sitereport");
        public static rm SitePublish = new rm("SiteMatrix.resource.sitepublish");
        public static rm SiteList = new rm("SiteMatrix.resource.sitelist");
        public static rm SiteImport = new rm("SiteMatrix.resource.siteimport");
        public static rm SiteExport = new rm("SiteMatrix.resource.siteexport");
        public static rm SiteClear = new rm("SiteMatrix.resource.siteclear");
        public static rm SiteAdd = new rm("SiteMatrix.resource.siteadd");
        public static rm PartImage = new rm("SiteMatrix.resource.partimage");
        public static rm Option = new rm("SiteMatrix.resource.option");
        public static rm OpenWebPage = new rm("SiteMatrix.resource.openwebpage");
        public static rm ModifyDNS = new rm("SiteMatrix.resource.modifydns");
        public static rm LoadStat = new rm("SiteMatrix.resource.loadstat");
        public static rm Loading = new rm("SiteMatrix.resource.loading");
        public static rm InputName = new rm("SiteMatrix.resource.inputname");
        public static rm ImportPage = new rm("SiteMatrix.resource.importpage");
        public static rm GetNode = new rm("SiteMatrix.resource.getnode");
        public static rm FindReplace = new rm("SiteMatrix.resource.findreplace");
        public static rm ErrorReport = new rm("SiteMatrix.resource.errorreport");
        public static rm AddSite = new rm("SiteMatrix.resource.addsite");
        public static rm AddPage = new rm("SiteMatrix.resource.addpage");
        public static rm AddDir = new rm("SiteMatrix.resource.adddir");
        public static rm AddControl = new rm("SiteMatrix.resource.addcontrol");
        public static rm About = new rm("SiteMatrix.resource.about");
        public static rm _controls = new rm("SiteMatrix.resource._controls");
        public static rm _globalconst = new rm("SiteMatrix.resource._globalconst");
        public static rm _globalfunctions = new rm("SiteMatrix.resource._globalfunctions");
        public static rm _pageware = new rm("SiteMatrix.resource._pageware");
        public static rm _propertysite = new rm("SiteMatrix.resource._propertysite");
        public static rm _site = new rm("SiteMatrix.resource._site");
        public static rm form = new rm("SiteMatrix.resource.form");
	}
    public class ControlIcons
    {
        private string _controlid;
        private System.Drawing.Image _icon;
        private string _caption;
        private string _company;
        private string _description;
        public ControlIcons()
        {
        }
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
            }
        }
        public string Company
        {
            get
            {
                return _company;
            }
            set
            {
                _company = value;
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        public string ControlID
        {
            get
            {
                return _controlid;
            }
            set 
            {
                _controlid = value;
            }
        }
        public System.Drawing.Image Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
            }
        }
    }
}
