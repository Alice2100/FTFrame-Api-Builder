//builder by maobb,2005-7-9 - 10
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices;
using FTDPClient.forms;
using mshtml;
using FTDPClient.Page;
using FTDPClient.classes;
using FTDPClient.Adapter;
using FTDPClient.functions;
using FTDPClient.consts;
using FTDPClient.database;
using Microsoft.Data.Sqlite;
using FTDPClient.forms.control;

namespace FTDPClient.classes
{
	
	/// <summary>
	/// StyleBuilder 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class ImageEditorConverter:ExpandableObjectConverter
	{
		public ImageEditorConverter()
		{
			System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl),this);
		}
		public override object ConvertTo(ITypeDescriptorContext context,System.Globalization.CultureInfo culture,object vvalue,Type destinationType)
		{
			string _value=vvalue.ToString();
			if(!_value.ToLower().StartsWith("<img"))
			{
				return vvalue.ToString();
			}
			try
			{
				string s=_value;
				int i=s.IndexOf(" src=\"");
				if(i<0)return "";
				s=s.Substring(i+6,s.Length-i-6);
				i=s.IndexOf("\"");
				s=s.Substring(0,i);
				return s;
			}
			catch
			{
			return "";
			}
		}
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return false;
		}
	}
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class AccessUrlEditorConverter : ExpandableObjectConverter
    {
        public static string PartID;
        public static string Column;
        public static string v;
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object vvalue, Type destinationType)
        {
            if (!vvalue.ToString().Equals(v))
            {
                v = vvalue.ToString();
                string sql = "update parts set " + Column + "='" + v.Replace("'", "''") + "' where id='" + PartID + "'";
                globalConst.CurSite.SiteConn2.execSql(sql);
            }
            return vvalue;
        }
        public AccessUrlEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class AccessUrlEditorPageConverter : ExpandableObjectConverter
    {
        public static string PageID;
        public static string Column;
        public static string v;
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object vvalue, Type destinationType)
        {
            if (!vvalue.ToString().Equals(v))
            {
                v = vvalue.ToString();
                string sql = "update pages set " + Column + "='" + v.Replace("'", "''") + "' where id='" + PageID + "'";
                globalConst.CurSite.SiteConn2.execSql(sql);
            }
            return vvalue;
        }
        public AccessUrlEditorPageConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class HomePageEditorConverter : ExpandableObjectConverter
    { 
        public HomePageEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class DataSourceEditorConverter : ExpandableObjectConverter
    { 
        public DataSourceEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class BackValueEditorConverter : ExpandableObjectConverter
    {
        public BackValueEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class RowAllEditorConverter : ExpandableObjectConverter
    {
        public RowAllEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    public class CommonSelColsEditorConverter : ExpandableObjectConverter
    {
        public CommonSelColsEditorConverter()
        {
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class DataOPDefineEditorConverter : ExpandableObjectConverter
    {
        public DataOPDefineEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class DyValueDefineEditorConverter : ExpandableObjectConverter
    {
        public DyValueDefineEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    public class ListMenuEditorConverter : ExpandableObjectConverter
    {
        public ListMenuEditorConverter()
        {
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    public class DataOPAPIEditorConverter : ExpandableObjectConverter
    {
        public DataOPAPIEditorConverter()
        {
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    public class DyValueAPIEditorConverter : ExpandableObjectConverter
    {
        public DyValueAPIEditorConverter()
        {
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    public class ListAPIEditorConverter : ExpandableObjectConverter
    {
        public ListAPIEditorConverter()
        {
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class SQLEditorConverter : ExpandableObjectConverter
    {
        public SQLEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class TableTagEditorConverter : ExpandableObjectConverter
    {
        public TableTagEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    public class TableSelectEditorConverter : ExpandableObjectConverter
    {
        public TableSelectEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class FormDataEditorConverter : ExpandableObjectConverter
    {
        public FormDataEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    public class SelectFolderEditorConverter : ExpandableObjectConverter
    {
        public SelectFolderEditorConverter()
        {
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class HomePageEditor : UITypeEditor
    {
        public HomePageEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    GetNode gn = new GetNode();
                    gn.ReturnURL = vvalue.ToString();
                    gn.ShowDialog();
                    if (!gn.IsCancel)
                    {
                        return gn.ReturnURL;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class DataSourceEditor : UITypeEditor
    {
        public DataSourceEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    GetControlData gn=new GetControlData();
                    gn.ReturnURL = vvalue.ToString();
                    gn.ShowDialog();
                    if(!gn.IsCancel)
                    {
                        return gn.ReturnURL;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class BackValueEditor : UITypeEditor
    {
        public BackValueEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    BackValue gn = new BackValue();
                    gn.restr = vvalue.ToString();
                    gn.ShowDialog();
                    if (!gn.IsCancel)
                    {
                        return gn.restr;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class RowAllEditor : UITypeEditor
    {
        public RowAllEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public static string PartID;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    RowAll gn = new RowAll();
                    gn.partId = PartID;
                    gn.restr = vvalue.ToString();
                    gn.ShowDialog();
                    if (!gn.IsCancel)
                    {
                        return gn.restr;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }

    public class CommonSelColsEditorType1 : UITypeEditor
    {
        public CommonSelColsEditorType1()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public static string PartID;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    string MainTable = PageAsist.GetPartSetValue(PartID, "MainTable");
                    string CusSQL = PageAsist.GetPartSetValue(PartID, "CusSQL");
                    Common_SelCols cs = new Common_SelCols();
                    cs.MainTable = MainTable;
                    cs.SelectSql = CusSQL;
                    cs.SelValue = vvalue.ToString();
                    cs.SelType = 1;
                    cs.ShowDialog();
                    if (!cs.IsCancel)
                    {
                        return cs.SelValue;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    public class CommonSelColsEditorType2 : UITypeEditor
    {
        public CommonSelColsEditorType2()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public static string PartID;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    string MainTable = PageAsist.GetPartSetValue(PartID, "MainTable");
                    string CusSQL = PageAsist.GetPartSetValue(PartID, "CusSQL");
                    Common_SelCols cs = new Common_SelCols();
                    cs.MainTable = MainTable;
                    cs.SelectSql = CusSQL;
                    cs.SelValue = vvalue.ToString();
                    cs.SelType = 2;
                    cs.ShowDialog();
                    if (!cs.IsCancel)
                    {
                        return cs.SelValue;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    public class ListMenuEditor : UITypeEditor
    {
        public ListMenuEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public static string PartID;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    List_Menu gn = new List_Menu();
                    gn.partId = PartID;
                    gn.restr = vvalue.ToString();
                    gn.ShowDialog();
                    if (!gn.IsCancel)
                    {
                        return gn.restr;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    public class DataOPAPIEditor : UITypeEditor
    {
        public DataOPAPIEditor()
        {
        }
        private IWindowsFormsEditorService service;
        public static string PartID;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    DataOP_Api gn = new DataOP_Api();
                    gn.nameCapList = DataOpDefine.IdNameList(PartID);
                    gn.partId = PartID;
                    gn.restr = vvalue.ToString();
                    gn.ShowDialog();
                    if (!gn.IsCancel)
                    {
                        return gn.restr;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    public class DyValueAPIEditor : UITypeEditor
    {
        public DyValueAPIEditor()
        {
        }
        private IWindowsFormsEditorService service;
        public static string PartID;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    DyValue_Api gn = new DyValue_Api();
                    gn.idCapList = DyValueDefine.IdNameList(PartID);
                    gn.partId = PartID;
                    gn.restr = vvalue.ToString();
                    gn.ShowDialog();
                    if (!gn.IsCancel)
                    {
                        return gn.restr;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    public class ListAPIEditor : UITypeEditor
    {
        public ListAPIEditor()
        {
        }
        private IWindowsFormsEditorService service;
        public static string PartID;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    List_Api gn = new List_Api();
                    gn.partId = PartID;
                    gn.restr = vvalue.ToString();
                    gn.ShowDialog();
                    if (!gn.IsCancel)
                    {
                        return gn.restr;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class DataOPDefineEditor : UITypeEditor
    {
        public DataOPDefineEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public static string PartID;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    if (!DataOpDefine.DataOpFormShow)
                    {
                        DataOpDefine gn = new DataOpDefine();
                        gn.str = vvalue.ToString();
                        gn.partid = PartID;
                        gn.Show();
                    }
                    else
                    {
                        DataOpDefine.DataOpDefineForm.WindowState = FormWindowState.Normal;
                        DataOpDefine.DataOpDefineForm.Activate();
                        MsgBox.Warning("Last Data Operation Form Has Opened", DataOpDefine.DataOpDefineForm);
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class DyValueDefineEditor : UITypeEditor
    {
        public DyValueDefineEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public static string PartID;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    if (!DyValueDefine.DyValueFormShow)
                    {
                        DyValueDefine gn = new DyValueDefine();
                        gn.str = vvalue.ToString();
                        gn.partid = PartID;
                        gn.Show();
                    }
                    else
                    {
                        DyValueDefine.DyValueDefineForm.WindowState = FormWindowState.Normal;
                        DyValueDefine.DyValueDefineForm.Activate();
                        MsgBox.Warning("Last Data Getting Form Has Opened", DyValueDefine.DyValueDefineForm);
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class SQLEditor : UITypeEditor
    {
        public SQLEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    SQL gn = new SQL();
                    gn.restr = vvalue.ToString();
                    gn.ShowDialog();
                    if (!gn.IsCancel)
                    {
                        return gn.restr;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class TableTagEditor : UITypeEditor
    {
        public TableTagEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    TableTag gn = new TableTag();
                    gn.ReturnStr = vvalue.ToString();
                    gn.ShowDialog();
                    if (!gn.IsCancel)
                    {
                        return gn.ReturnStr;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    public class TableSelectEditor : UITypeEditor
    {
        public TableSelectEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    string connstr = Options.GetSystemDBSetConnStr();
                    var dbtype = Options.GetSystemDBSetType();
                    if (connstr == null || connstr.Trim().Equals(""))
                    {
                        MsgBox.Warning("You must first configure the database type and connection string in the Tools option");
                        return ""; 
                    }
                    if (dbtype==globalConst.DBType.MySql)
                    {
                        forms.control.SelTable_MySql sel = new forms.control.SelTable_MySql();
                        sel.connstr = connstr;
                        sel.ShowDialog();
                        if (sel.tablename != null)
                        {
                            return "@" + sel.tablename;
                        }
                        else return "";
                    }
                    else if (dbtype == globalConst.DBType.SqlServer)
                    {
                        forms.control.SelTable_SqlServer sel = new forms.control.SelTable_SqlServer();
                        sel.connstr = connstr;
                        sel.ShowDialog();
                        if (sel.tablename != null)
                        {
                            return "@" + sel.tablename;
                        }
                        else return "";
                    }
                    else if (dbtype == globalConst.DBType.Sqlite)
                    {
                        forms.control.SelTable_Sqlite sel = new forms.control.SelTable_Sqlite();
                        sel.connstr = connstr;
                        sel.ShowDialog();
                        if (sel.tablename != null)
                        {
                            return "@" + sel.tablename;
                        }
                        else return "";
                    }
                    else
                    {
                        MsgBox.Warning(dbtype + " The convenience development for this database type has not been completed yet");
                        return "";
                    }
                    //TableTag gn = new TableTag();
                    //gn.ReturnStr = vvalue.ToString();
                    //gn.ShowDialog();
                    //if (!gn.IsCancel)
                    //{
                    //    return gn.ReturnStr;
                    //}
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class FormDataEditor : UITypeEditor
    {
        public FormDataEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    if (FormData.TheFormData == null)
                    {
                        FormData.TheFormData = new FormData();
                    }

                    if (context.PropertyDescriptor.Name.Equals(res._propertysite.GetString("ad_formtable")))
                    {
                        FormData.TheFormData.fromSite = true;
                        FormData.FormDataShow();
                        if (!FormData.TheFormData.IsCancel)
                        {
                            return "Table Count : " + FormData.TheFormData.ReturnValue;
                        }
                    }
                    else
                    {
                        FormData.TheFormData.fromSite = false;
                        FormData.FormDataShow();
                        if (!FormData.TheFormData.IsCancel)
                        {
                            return FTDPClient.PropertySpace.FormElementData.func.getFormDataBindStr(FormData.ele);
                        }
                    }
                    
                }
            }
            return vvalue;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }

    }
    public class SelectFolderEditor : UITypeEditor
    {
        public SelectFolderEditor()
        {
        }
        private IWindowsFormsEditorService service;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    dialog.ShowDialog();
                    if(!string.IsNullOrEmpty(dialog.SelectedPath))return dialog.SelectedPath;
                }
            }
            return vvalue;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }

    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class TextEditorConverter : ExpandableObjectConverter
    {
        public TextEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class TextEditor : UITypeEditor
    {
            public TextEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    forms.TextEditor te = new forms.TextEditor();
                    te.basetext = vvalue.ToString();
                    te.ShowDialog();
                    if (!te.cancel)
                    {
                        return te.basetext;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class OptionEditorConverter : ExpandableObjectConverter
    {
        public OptionEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return "(Collection)";
            //return base.ConvertTo(context, culture, value, destinationType);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class OptionEditor : UITypeEditor
    {
        public OptionEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    forms.OptionsEditor oe = new forms.OptionsEditor();
                    oe.options = (System.Collections.ArrayList)vvalue;
                    oe.ShowDialog();
                    if (!oe.cancel)
                    {
                        return oe.options;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class SourceEditorConverter : ExpandableObjectConverter
    {
        public SourceEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value == null) return null;
            string CurPageURL = globalConst.CurSelectionPage.ToLower().Replace("\\", "/");
            string PageName = CurPageURL.Substring(CurPageURL.LastIndexOf('/') + 1);
            string filename = value.ToString();
            filename=System.Web.HttpUtility.UrlDecode(filename);
            filename = filename.ToLower().Replace("file:///","").Replace("\\", "/");
            if (globalConst.CurSite.Path!=null&&CurPageURL.StartsWith(globalConst.CurSite.Path.ToLower().Replace("\\", "/")) && filename.StartsWith(globalConst.CurSite.Path.ToLower().Replace("\\", "/")))
            {
                //page and image are both under site path.
                DirectoryInfo di = new DirectoryInfo(CurPageURL.Substring(0, CurPageURL.LastIndexOf('/')));
                string UpPath = "";
                while (!filename.StartsWith(di.FullName.ToLower().Replace("\\", "/") + "/"))
                {
                    di = di.Parent;
                    UpPath += "../";
                }
                filename = UpPath + filename.Replace(di.FullName.ToLower().Replace("\\", "/") + "/", "");
            }
            else
                if (filename.StartsWith(CurPageURL + ".resource/"))
                {
                    filename = filename.Replace(CurPageURL + ".resource/", PageName + ".resource/");
                }
            return filename;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class SourceEditor : UITypeEditor
    {
        public SourceEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    OpenFileDialog ofd=new OpenFileDialog();

                    string CurPageURL = globalConst.CurSelectionPage.ToLower().Replace("\\", "/");
                    string SrcPath="";
                    if (vvalue != null)
                    {
                        SrcPath = vvalue.ToString();
                    }
                    SrcPath = System.Web.HttpUtility.UrlDecode(SrcPath);
                    SrcPath = SrcPath.ToLower().Replace("file:///", "").Replace("/","\\");

                    if (File.Exists(SrcPath))
                    {
                        ofd.FileName = SrcPath;
                    }
  
                    ofd.ShowDialog();
                    string filename=ofd.FileName;
                    if (CurPageURL.Equals("")) return filename;
                    if (filename != null && !filename.Equals(""))
                    {
                        string PageName = CurPageURL.Substring(CurPageURL.LastIndexOf('/') + 1);
                        filename = filename.ToLower().Replace("\\", "/");
                        if (filename.StartsWith(CurPageURL + ".resource/"))
                        {
                            filename = filename.Replace(CurPageURL + ".resource/", PageName + ".resource/");
                        }
                        else if (!CurPageURL.StartsWith(globalConst.FreeFilesPath.ToLower().Replace("\\", "/")))
                        {
                            //free file or the image not in site path.
                            if (globalConst.CurSite.Path == null || !CurPageURL.StartsWith(globalConst.CurSite.Path.ToLower().Replace("\\", "/")) || !filename.StartsWith(globalConst.CurSite.Path.ToLower().Replace("\\", "/")))
                            {
                                if (!dir.Exists(CurPageURL + ".resource"))
                                {
                                    dir.CreateDirectory(CurPageURL + ".resource");
                                }
                                string fileshortname = filename.Substring(filename.LastIndexOf('/') + 1);
                                int i = 0;
                                while (file.Exists(CurPageURL + ".resource/" + i + fileshortname))
                                {
                                    i++;
                                }
                                file.Copy(filename, CurPageURL + ".resource/" + i + fileshortname);
                                filename = PageName + ".resource/" + i + fileshortname;
                            }
                            else
                            {
                                //page and image are both under site path.
                                DirectoryInfo di = new DirectoryInfo(CurPageURL.Substring(0, CurPageURL.LastIndexOf('/')));
                                string UpPath = "";
                                while (!filename.StartsWith(di.FullName.ToLower().Replace("\\", "/") + "/"))
                                {
                                    di = di.Parent;
                                    UpPath += "../";
                                }
                                filename = UpPath + filename.Replace(di.FullName.ToLower().Replace("\\", "/") + "/", "");
                            }
                        }
                        return filename;
                    }
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class ImageEditor:UITypeEditor
	{
		public ImageEditor()
		{
			System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl),this);
		}
		private IWindowsFormsEditorService service;
		private string thisurl="";
		private string thisname="";
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider , object vvalue)
		{
			if(context!=null && context.Instance!=null && provider!=null)
			{
				service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if(service!=null)
				{
PartImage pi=new PartImage();
pi.imagestring=vvalue.ToString();
pi.ShowDialog();
					if(pi.isCancel)
					{}
					else
					{
					if(PartImage.returnString)return PartImage._text;
					string imagestring=pi.Ele.outerHTML;
						string s="";
						try
						{
							s=imagestring;
							int i=s.IndexOf(" src=\"");
							if(i<0)return "";
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
								imagestring=imagestring.Replace("src=\"" + s,"src=\"" + s.Replace("file:///","").Replace("\\","/"));
								s=s.Replace("file:///","").Replace("\\","/");
								if(s.StartsWith(globalConst.AppPath.Replace("\\","/") + "/lib"))
								{
									imagestring=imagestring.Replace("src=\"" + globalConst.AppPath.Replace("\\","/"),"src=\"");
								}
								else if(s.StartsWith(globalConst.CurSite.Path.Replace("\\","/") + "/control.resource"))
								{
									imagestring=imagestring.Replace("src=\"" + globalConst.CurSite.Path.Replace("\\","/"),"src=\"");
								}
								else
								{
									if(thisurl.Equals(""))thisurl=PartImage.thisURL;
									if(thisname.Equals(""))thisname=PartImage.thisName;
									if(!thisurl.Equals(""))
									{
										imagestring=imagestring.Replace("src=\"" + thisurl.Substring(0,thisurl.Length-thisname.Length),"src=\"");
										imagestring=imagestring.Replace("src=\"file:///" + thisurl.Substring(0,thisurl.Length-thisname.Length).Replace("\\","/"),"src=\"");
									}
								}
							}
						}
						return imagestring;
					}
				}
			}
			return vvalue;
		}
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (context!=null && context.Instance!=null)
			{
				return UITypeEditorEditStyle.Modal;
			}
			return this.GetEditStyle(context);
		}
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}
		public override void PaintValue(PaintValueEventArgs e)
		{
			string img="";
			string s="";
			try
			{
				s=e.Value.ToString();
				int i=s.IndexOf(" src=\"");
				if(i<0)return;
				s=s.Substring(i+6,s.Length-i-6);
				i=s.IndexOf("\"");
				s=s.Substring(0,i);
				img=s;
			}
			catch
			{
			}
			if(img.Equals(""))return;
			s=img;
			//file just
			if(!s.ToLower().StartsWith("http://")&&!s.ToLower().StartsWith("ftp://"))
			{
				s=img.Replace("/","\\");
				if(s.StartsWith(@"\lib"))
				{
					img=globalConst.AppPath + img;
				}
				else if(s.StartsWith("\\control.resource"))
								{
					img=globalConst.CurSite.Path + img;			
								}
				else
				{
					if(thisurl.Equals(""))thisurl=PartImage.thisURL;
					if(thisname.Equals(""))thisname=PartImage.thisName;
					if(!thisurl.Equals(""))
					{
						img=thisurl.Substring(0,thisurl.Length-thisname.Length) + img;
					}
					img=img.Replace("src=\"file:///","");
				}
			}
			if(!file.Exists(img))return;
			Image im=Image.FromFile(img);
			e.Graphics.DrawImage(im,0,0,20,15);
		}

	}
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class StyleEditorConverter:ExpandableObjectConverter
	{
		public StyleEditorConverter()
		{
			System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl),this);
		}
//		public override bool CanConvertFrom(ITypeDescriptorContext contex , Type sourceType)
//		{
//			return this.CanConvertFrom(contex, sourceType);
//		}
//		public override bool CanConvertTo(ITypeDescriptorContext contex , Type destinationType)
//		{
//			if(destinationType.Equals(typeof(string)))return true;
//			return this.CanConvertFrom(contex, destinationType);
//		}
//		public override object ConvertFrom(ITypeDescriptorContext context,System.Globalization.CultureInfo culture,object vvalue)
//		{
//			if(vvalue.GetType().Equals(typeof(string)))
//			{
//				return vvalue;
//			}
//			return this.ConvertFrom(context, culture, vvalue);
//		}
		public override object ConvertTo(ITypeDescriptorContext context,System.Globalization.CultureInfo culture,object vvalue,Type destinationType)
		{
			return ((IHTMLElement)vvalue).style.cssText;
		}
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			//we don't want nested props
			return false;
		}
//		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context,object vvalue,Attribute[] attributes)
//		{
//			return null;
//		}
	}
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class StyleEditor:UITypeEditor
	{
		public StyleEditor()
		{
			System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl),this);
		}
		public static string ControlPartID;
		public static string PartStyleName;
		public static IHTMLElement PartElement;
		private IWindowsFormsEditorService service;
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider , object vvalue)
		{
			if(context!=null && context.Instance!=null && provider!=null)
			{
				service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if(service!=null)
				{
					// the Edit method gets the value ByRef, 
					// and changes it to the new value when user clicks ok
					// I don't know why ... but it took me ages to get this part of the project working 
					// I didn't find how i could change the property in the propertybag ... 
					// until i found out that i just needed to change "value" to the new version
					//MultiLineStringEditorForm.Edit(value, "Edit " & context.PropertyDescriptor.Name);
					StyleBuilder sb=new StyleBuilder();
					sb.ele=(IHTMLElement)vvalue;
					//part style 
					if(((IHTMLElement)vvalue).tagName.Equals("dscomstyle"))
					{
					sb.ControlPartID=ControlPartID;
					sb.PartElement=PartElement;
					sb.PartStyleName=PartStyleName;
					}
					string stylecss=((IHTMLElement)vvalue).style.cssText;
					//font 如果取消回复原貌 add by maobb,2005-7-10
//					object eocolor=sb.ele.style.color;
//					string eostyle=sb.ele.style.fontStyle;
//					string eoweight=sb.ele.style.fontWeight;
//					bool eolintt=sb.ele.style.textDecorationLineThrough;
//					bool eolinte=sb.ele.style.textDecorationUnderline;
//					bool eolinOverline=sb.ele.style.textDecorationOverline;
//					string styletextAlign=sb.ele.style.textAlign;
//					string stylefontVariant=sb.ele.style.fontVariant;
//					object stylefontSize=sb.ele.style.fontSize;
//					string stylefontFamily=sb.ele.style.fontFamily;
//					object styleletterSpacing=sb.ele.style.letterSpacing;
//					object stylewordSpacing=sb.ele.style.wordSpacing;
//					string stylewhiteSpace=sb.ele.style.whiteSpace;
//					object stylelineHeight=sb.ele.style.lineHeight;
//					//bg
//					object backgroundColor=sb.ele.style.backgroundColor;
//					string backgroundImage=sb.ele.style.backgroundImage;
//					string backgroundRepeat=sb.ele.style.backgroundRepeat;
//					object backgroundPositionX=sb.ele.style.backgroundPositionX;
//					object backgroundPositionY=sb.ele.style.backgroundPositionY;
//					string backgroundAttachment=sb.ele.style.backgroundAttachment;
//					//border
//					string styleborderWidth=sb.ele.style.borderWidth;
//					string styleborderColor=sb.ele.style.borderColor;
//					string styleborderStyle=sb.ele.style.borderStyle;
//					object styleborderLeftWidth=sb.ele.style.borderLeftWidth;
//					object styleborderLeftColor=sb.ele.style.borderLeftColor;
//					string styleborderLeftStyle=sb.ele.style.borderLeftStyle;
//					object styleborderRightWidth=sb.ele.style.borderRightWidth;
//					object styleborderRightColor=sb.ele.style.borderRightColor;
//					string styleborderRightStyle=sb.ele.style.borderRightStyle;
//					object styleborderTopWidth=sb.ele.style.borderTopWidth;
//					object styleborderTopColor=sb.ele.style.borderTopColor;
//					string styleborderTopStyle=sb.ele.style.borderTopStyle;
//					object styleborderBottomWidth=sb.ele.style.borderBottomWidth;
//					object styleborderBottomColor=sb.ele.style.borderBottomColor;
//					string styleborderBottomStyle=sb.ele.style.borderBottomStyle;
					sb.ShowDialog();
					if(sb.isCanceled)
					{
					//part style and element style not same
						if(((IHTMLElement)vvalue).tagName.Equals("dscomstyle"))
						{
							PageWare.updatePartStyle(ControlPartID,PartStyleName,stylecss);
							if(PartElement!=null)
							{
								try
								{
									PartElement.innerHTML=PageWare.getPartHtml(ControlPartID);
								}
								catch(Exception ex)
								{
									if(ex.Message.StartsWith("HRESULT"))
									{
										//Ele.parentElement.tagName="div";
										//Ele.innerHTML=getPartHtml("ctlid",CtlName,PartName,curPartSetXmlDom.OuterXml);
										//if parent element is <p> <td>...,span will accur error
										string Eleid=PartElement.getAttribute("idname",0).ToString();
										string Eleheight=PartElement.style.height.ToString();
										string Elewidth=PartElement.style.width.ToString();
										string Elename=PartElement.getAttribute("partname",0).ToString();
										//int ElesourceIndex=PartElement.sourceIndex;
										PartElement.innerHTML="";
										PartElement.style.cssText="";
										IHTMLElement itp=PartElement.parentElement;
										itp.outerHTML=itp.outerHTML.Replace(PartElement.outerHTML,globalConst.PageWare.getControlEditHead(Eleid,Elename,Elewidth,Eleheight) + PageWare.getPartHtml(ControlPartID) + globalConst.PageWare.getControlEditTail());
										//PartElement=(IHTMLElement)(((IHTMLDocument2)form.getEditor().editocx.DOM).all.item(ElesourceIndex,ElesourceIndex));
										//if(!PageWare.isPartElement(PartElement))MsgBox.Warning("Part Element Lost!");
										//new MsgBox(Ele.outerHTML);
										//					//new MsgBox(form.getEditor().editocx.getCurElement().outerHTML);
										//Ele=getPartElement(form.getEditor().editocx.getCurElement());
										//new MsgBox(Ele.outerHTML);
										//IHTMLDocument2 doc2=(IHTMLDocument2)itp.document;
										//new MsgBox(doc2.all.length.ToString());
				
										//					foreach(IHTMLElement ie in (IHTMLElementCollection)itp.all)
										//					{
										//						if(PageWare.isPartElement(ie))
										//						{
										//							if(ie.getAttribute("idname",0).ToString().Equals(Eleid))
										//							{
										//								Ele=ie;
										//							}
										//						}
										//					}
					
									}
								}
				
							}
						}
						else
						{
							((IHTMLElement)vvalue).style.cssText=stylecss;
						}
//					sb.ele.style.color=eocolor;
//					sb.ele.style.fontStyle=eostyle;
//					sb.ele.style.fontWeight=eoweight;
//					sb.ele.style.textDecorationLineThrough=eolintt;
//					sb.ele.style.textDecorationUnderline=eolinte;
//						sb.ele.style.textDecorationOverline=eolinOverline;
//						sb.ele.style.textAlign=styletextAlign;
//						sb.ele.style.fontVariant=stylefontVariant;
//						if(stylefontSize==null)sb.ele.style.fontSize="";
//						else sb.ele.style.fontSize=stylefontSize;
//						sb.ele.style.fontFamily=stylefontFamily;
//
//					if(styleletterSpacing==null)sb.ele.style.letterSpacing="";
//						else sb.ele.style.letterSpacing=styleletterSpacing;
//					if(stylewordSpacing==null)sb.ele.style.wordSpacing="";
//						else sb.ele.style.wordSpacing=stylewordSpacing;
//					sb.ele.style.whiteSpace=stylewhiteSpace;
//					if(stylelineHeight==null)sb.ele.style.lineHeight="";
//						else sb.ele.style.lineHeight=stylelineHeight;
//					sb.ele.style.backgroundColor=backgroundColor;
//					sb.ele.style.backgroundImage=backgroundImage;
//					sb.ele.style.backgroundRepeat=backgroundRepeat;
//						if(backgroundPositionX==null)sb.ele.style.backgroundPositionX="";
//						else sb.ele.style.backgroundPositionX=backgroundPositionX;
//						if(backgroundPositionY==null)sb.ele.style.backgroundPositionY="";
//						else sb.ele.style.backgroundPositionY=backgroundPositionY;
//					sb.ele.style.backgroundAttachment=backgroundAttachment;
//
//						//border
//
//						sb.ele.style.borderWidth=styleborderWidth;
//						sb.ele.style.borderColor=styleborderColor;
//						sb.ele.style.borderStyle=styleborderStyle;
//						if(styleborderLeftWidth==null)sb.ele.style.borderLeftWidth="";
//						else sb.ele.style.borderLeftWidth=styleborderLeftWidth;
//						sb.ele.style.borderLeftColor=styleborderLeftColor;
//						sb.ele.style.borderLeftStyle=styleborderLeftStyle;
//						if(styleborderRightWidth==null)sb.ele.style.borderRightWidth="";
//						else sb.ele.style.borderRightWidth=styleborderRightWidth;
//						sb.ele.style.borderRightColor=styleborderRightColor;
//						sb.ele.style.borderRightStyle=styleborderRightStyle;
//						if(styleborderTopWidth==null)sb.ele.style.borderTopWidth="";
//						else sb.ele.style.borderTopWidth=styleborderTopWidth;
//						sb.ele.style.borderTopColor=styleborderTopColor;
//						sb.ele.style.borderTopStyle=styleborderTopStyle;
//						if(styleborderBottomWidth==null)sb.ele.style.borderBottomWidth="";
//						else sb.ele.style.borderBottomWidth=styleborderBottomWidth;
//						sb.ele.style.borderBottomColor=styleborderBottomColor;
//						sb.ele.style.borderBottomStyle=styleborderBottomStyle;
					}
					return sb.ele;
				}
			}
			return vvalue;
		}
		
		//处理<未定义的值>
		private string AdapterString(string o)
		{
			try
			{
				if(o==null)return null;
				return o;
			}
			catch
			{
			return null;
			}
		}
		private object AdapterObject(object o)
		{
			try
			{
				if(o==null)return null;
				return o;
			}
			catch
			{
				return null;
			}
		}
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (context!=null && context.Instance!=null)
			{
				return UITypeEditorEditStyle.Modal;
			}
			return this.GetEditStyle(context);
		}
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return false;
		}



	}
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class AccessIPConditionEditorConverter : StringConverter
    {
        public AccessIPConditionEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public static string v;
        public static string PartID;
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object vvalue, Type destinationType)
        {
            if (!vvalue.ToString().Equals(v))
            {
                v = vvalue.ToString();
                string sql = "update parts set a_ip_c='" + v.Replace("'", "''") + "' where id='" + PartID + "'";
                globalConst.CurSite.SiteConn2.execSql(sql);
            }
            return vvalue;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class AccessIPConditionEditorPageConverter : StringConverter
    {
        public AccessIPConditionEditorPageConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public static string v;
        public static string PageID;
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object vvalue, Type destinationType)
        {
            if (!vvalue.ToString().Equals(v))
            {
                v = vvalue.ToString();
                string sql = "update pages set a_ip_c='" + v.Replace("'", "''") + "' where id='" + PageID + "'";
                globalConst.CurSite.SiteConn2.execSql(sql);
            }
            return vvalue;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class AccessSessionConditionEditorConverter : StringConverter
    {
        public AccessSessionConditionEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public static string v;
        public static string PartID;
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object vvalue, Type destinationType)
        {
            if (!vvalue.ToString().Equals(v))
            {
                v = vvalue.ToString();
                string sql = "update parts set a_se_c='" + v.Replace("'", "''") + "' where id='" + PartID + "'";
                globalConst.CurSite.SiteConn2.execSql(sql);
            }
            return vvalue;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class AccessSessionConditionEditorPageConverter : StringConverter
    {
        public AccessSessionConditionEditorPageConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public static string v;
        public static string PageID;
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object vvalue, Type destinationType)
        {
            if (!vvalue.ToString().Equals(v))
            {
                v = vvalue.ToString();
                string sql = "update pages set a_se_c='" + v.Replace("'", "''") + "' where id='" + PageID + "'";
                globalConst.CurSite.SiteConn2.execSql(sql);
            }
            return vvalue;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class AccessTipContentEditorConverter : StringConverter
    {
        public AccessTipContentEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public static string v;
        public static string PartID;
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object vvalue, Type destinationType)
        {
            if (!vvalue.ToString().Equals(v))
            {
                v = vvalue.ToString();
                string sql = "update parts set a_tp_c='" + v.Replace("'", "''") + "' where id='" + PartID + "'";
                globalConst.CurSite.SiteConn2.execSql(sql);
            }
            return vvalue;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class AccessTipContentEditorPageConverter : StringConverter
    {
        public AccessTipContentEditorPageConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public static string v;
        public static string PageID;
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object vvalue, Type destinationType)
        {
            if (!vvalue.ToString().Equals(v))
            {
                v = vvalue.ToString();
                string sql = "update pages set a_tp_c='" + v.Replace("'", "''") + "' where id='" + PageID + "'";
                globalConst.CurSite.SiteConn2.execSql(sql);
            }
            return vvalue;
        }
    }
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class ClassEditorConverter:StringConverter
	{
		public ClassEditorConverter()
		{
			System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl),this);
		}
		//		public override bool CanConvertFrom(ITypeDescriptorContext contex , Type sourceType)
		//		{
		//			return this.CanConvertFrom(contex, sourceType);
		//		}
		//		public override bool CanConvertTo(ITypeDescriptorContext contex , Type destinationType)
		//		{
		//			if(destinationType.Equals(typeof(string)))return true;
		//			return this.CanConvertFrom(contex, destinationType);
		//		}
		//		public override object ConvertFrom(ITypeDescriptorContext context,System.Globalization.CultureInfo culture,object vvalue)
		//		{
		//			if(vvalue.GetType().Equals(typeof(string)))
		//			{
		//				return vvalue;
		//			}
		//			return this.ConvertFrom(context, culture, vvalue);
		//		}
		public static string v;
		public static string ControlPartID;
		public static string PartStyleName;
		public static IHTMLElement PartElement;
		public override object ConvertTo(ITypeDescriptorContext context,System.Globalization.CultureInfo culture,object vvalue,Type destinationType)
		{
			if(!v.Equals(vvalue.ToString()))
			{
				v=vvalue.ToString();
				PageWare.updatePartClass(ControlPartID,PartStyleName,vvalue.ToString());
				if(PartElement!=null)
				{
					try
					{
						PartElement.innerHTML=PageWare.getPartHtml(ControlPartID);
					}
					catch(Exception ex)
					{
						if(ex.Message.StartsWith("HRESULT"))
						{
							//Ele.parentElement.tagName="div";
							//Ele.innerHTML=getPartHtml("ctlid",CtlName,PartName,curPartSetXmlDom.OuterXml);
							//if parent element is <p> <td>...,span will accur error
							string Eleid=PartElement.getAttribute("idname",0).ToString();
							string Eleheight=PartElement.style.height.ToString();
							string Elewidth=PartElement.style.width.ToString();
							string Elename=PartElement.getAttribute("partname",0).ToString();
							//int ElesourceIndex=PartElement.sourceIndex;
							PartElement.innerHTML="";
							PartElement.style.cssText="";
							IHTMLElement itp=PartElement.parentElement;
							itp.outerHTML=itp.outerHTML.Replace(PartElement.outerHTML,globalConst.PageWare.getControlEditHead(Eleid,Elename,Elewidth,Eleheight) + PageWare.getPartHtml(ControlPartID) + globalConst.PageWare.getControlEditTail());
						}
					}
				}
			}
			return vvalue;
		}
//		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
//		{
//			//we don't want nested props
//			return false;
//		}
		//		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context,object vvalue,Attribute[] attributes)
		//		{
		//			return null;
		//		}
	}
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class AsPortalConverter : StringConverter
    {
        public AsPortalConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { consts.DefaultConst.GetBoolCaption(0), consts.DefaultConst.GetBoolCaption(1) });
        }
        public override bool GetStandardValuesSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(
            ITypeDescriptorContext context)
        {
            return true;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class ForeFrameConverter : StringConverter
    {
        public ForeFrameConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { ForeFrameType.JQueryUI.ToString(), ForeFrameType.LayUI.ToString() });
        }
        public override bool GetStandardValuesSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(
            ITypeDescriptorContext context)
        {
            return true;
        }
    }
    public class OutTypeConverter : StringConverter
    {
        public OutTypeConverter()
        {
        }
        public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { PageOutType.Page.ToString(), PageOutType.Json.ToString() });
        }
        public override bool GetStandardValuesSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(
            ITypeDescriptorContext context)
        {
            return true;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class DataSourceShareConverter : StringConverter
    {
        public DataSourceShareConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { control.GetSharedCaptionByIndex("0"), control.GetSharedCaptionByIndex("1"), control.GetSharedCaptionByIndex("2") });
        }
        public override bool GetStandardValuesSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(
            ITypeDescriptorContext context)
        {
            return true;
        }
    } 
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class DataSourceIDConverter: StringConverter
	{  
		public DataSourceIDConverter()
		{
			System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl),this);
		}
		public static string defaultDSID;
        public static string currentCName;
		public override StandardValuesCollection
			GetStandardValues(ITypeDescriptorContext context) 
		{
            string sql = "select count(*) as countall from share_data where name='" + currentCName + "'";
            int countall = globalConst.CurSite.SiteConn2.GetInt32(sql);
            sql = "select * from share_data where name='" + currentCName + "' order by site";
            SqliteDataReader dr = globalConst.CurSite.SiteConn2.OpenRecord(sql);
            string[] Values = new string[countall + 2];
            Values[0] = defaultDSID;
            Values[1] = "New DataSource";
            int i = 2;
            while (dr.Read())
            {
                Values[i] = dr.GetString(3) + "[" + dr.GetString(2) + "][" + control.GetSharedCaptionByIndex(dr.GetString(4)) + "][" + dr.GetString(5) + "]";
                i++;
            }
            dr.Close();
            return new StandardValuesCollection(Values);
		}
		public override bool GetStandardValuesSupported(
			ITypeDescriptorContext context) 
		{
			return true;
		}
		public override bool GetStandardValuesExclusive(
			ITypeDescriptorContext context) 
		{
			return true;
		}
	} 
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class PartPropertyEnumConverter: StringConverter
	{  
		public PartPropertyEnumConverter()
		{
			System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl),this);
		}
		public static string[] Enums;
		public override StandardValuesCollection
			GetStandardValues(ITypeDescriptorContext context) 
		{
            return new StandardValuesCollection(Enums);
		}
		public override bool GetStandardValuesSupported(
			ITypeDescriptorContext context) 
		{
			return true;
		}
		public override bool GetStandardValuesExclusive(
			ITypeDescriptorContext context) 
		{
			return true;
		}
	}
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class PartPropertyEnumConverter2 : StringConverter
    {
        public PartPropertyEnumConverter2()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public static string[] Enums;
        public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(Enums);
        }
        public override bool GetStandardValuesSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(
            ITypeDescriptorContext context)
        {
            return true;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class AccessActiveDeactiveEnumEditor : StringConverter
    {
        public AccessActiveDeactiveEnumEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public static string[] Enums;
        public static string PartID;
        public static string Column;
        public static string v;
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object vvalue, Type destinationType)
        {
            if (!vvalue.ToString().Equals(v))
            {
                v = vvalue.ToString();
                int intv = v.Equals(Enums[1]) ? 1 : 0;
                string sql = "update parts set " + Column + "=" + intv + " where id='" + PartID + "'";
                globalConst.CurSite.SiteConn2.execSql(sql);
            }
            return vvalue;
        }
        public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(Enums);
        }
        public override bool GetStandardValuesSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(
            ITypeDescriptorContext context)
        {
            return true;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class AccessActiveDeactiveEnumEditorPage : StringConverter
    {
        public AccessActiveDeactiveEnumEditorPage()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public static string[] Enums;
        public static string PageID;
        public static string Column;
        public static string v;
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object vvalue, Type destinationType)
        {
            if (!vvalue.ToString().Equals(v))
            {
                v = vvalue.ToString();
                int intv = v.Equals(Enums[1]) ? 1 : 0;
                string sql = "update pages set " + Column + "=" + intv + " where id='" + PageID + "'";
                globalConst.CurSite.SiteConn2.execSql(sql);
            }
            return vvalue;
        }
        public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(Enums);
        }
        public override bool GetStandardValuesSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(
            ITypeDescriptorContext context)
        {
            return true;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class AccessActiveDeactiveEnumEditor2 : StringConverter
    {
        public AccessActiveDeactiveEnumEditor2()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public static string[] Enums;
        public static string PartID;
        public static string Column;
        public static string v;
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object vvalue, Type destinationType)
        {
            if (!vvalue.ToString().Equals(v))
            {
                v = vvalue.ToString();
                int intv = v.Equals(Enums[1]) ? 1 : 0;
                string sql = "update parts set " + Column + "=" + intv + " where id='" + PartID + "'";
                globalConst.CurSite.SiteConn2.execSql(sql);
            }
            return vvalue;
        }
        public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(Enums);
        }
        public override bool GetStandardValuesSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(
            ITypeDescriptorContext context)
        {
            return true;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class AccessActiveDeactiveEnumEditorPage2 : StringConverter
    {
        public AccessActiveDeactiveEnumEditorPage2()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public static string[] Enums;
        public static string PageID;
        public static string Column;
        public static string v;
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object vvalue, Type destinationType)
        {
            if (!vvalue.ToString().Equals(v))
            {
                v = vvalue.ToString();
                int intv = v.Equals(Enums[1]) ? 1 : 0;
                string sql = "update pages set " + Column + "=" + intv + " where id='" + PageID + "'";
                globalConst.CurSite.SiteConn2.execSql(sql);
            }
            return vvalue;
        }
        public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(Enums);
        }
        public override bool GetStandardValuesSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(
            ITypeDescriptorContext context)
        {
            return true;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class FormPageMemberType : StringConverter
    {
        public FormPageMemberType()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { res.form.GetString("String20"), res.form.GetString("String21"), res.form.GetString("String22") });
        }
        public override bool GetStandardValuesSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(
            ITypeDescriptorContext context)
        {
            return true;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class FormFlowStatType : StringConverter
    {
        public FormFlowStatType()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { res.form.GetString("String138"), res.form.GetString("String139"), res.form.GetString("String140") });
        }
        public override bool GetStandardValuesSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(
            ITypeDescriptorContext context)
        {
            return true;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class FormPageModViewType : StringConverter
    {
        public FormPageModViewType()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { res.form.GetString("String115"), res.form.GetString("String116")});
        }
        public override bool GetStandardValuesSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(
            ITypeDescriptorContext context)
        {
            return true;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class RuleEditorConverter : ExpandableObjectConverter
    {
        public RuleEditorConverter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class RuleEditor : UITypeEditor
    {
        public static string pageid=null;
        public RuleEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl), this);
        }
        private IWindowsFormsEditorService service;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object vvalue)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    RulesEditor re = new RulesEditor();
                    re.pageid = pageid;
                    re.ShowDialog();
                }
            }
            return vvalue;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return this.GetEditStyle(context);
        }

    }
}

