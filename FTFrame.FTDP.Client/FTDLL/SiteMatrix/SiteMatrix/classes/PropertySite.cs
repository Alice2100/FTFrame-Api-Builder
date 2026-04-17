using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using SiteMatrix.PropertyBagNameSpace;
using SiteMatrix.consts;
using SiteMatrix.functions;
using SiteMatrix.forms;
using SiteMatrix.PropertySpace.ControlInfo;
using mshtml;
using SiteMatrix.database;
using System.Data.OleDb;
using System.Xml;
using SiteMatrix.classes;
using System.Drawing;
using SiteMatrix.Page;
using System.Windows.Forms;
using SiteMatrix.controls;
namespace SiteMatrix.PropertySpace.Site
{
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class PropertySite
    {
        public PropertySite()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
        }
        public static string getSiteString(string name)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "select " + name + " from sites where id='" + globalConst.CurSite.ID + "'";
            return globalConst.ConfigConn.GetString(sql);
        }
        public static void setSiteString(string name, string _value)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "update sites set " + name + "='" + _value.Replace("'", "''") + "' where id='" + globalConst.CurSite.ID + "'";
            globalConst.ConfigConn.execSql(sql);
        }
        public static int getSiteInt32(string name)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "select " + name + " from sites where id='" + globalConst.CurSite.ID + "'";
            return globalConst.ConfigConn.GetInt32(sql);
        }
        public static void setSiteInt32(string name, int _value)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "update sites set " + name + "=" + _value + " where id='" + globalConst.CurSite.ID + "'";
            globalConst.ConfigConn.execSql(sql);
        }
        //		[DescriptionAttribute("Description..."),
        //		CategoryAttribute("SiteProperty"),ReadOnlyAttribute(true)]
        //		public string ID
        //		{
        //			get 
        //			{
        //					return siteid;
        //			}
        //		}
        //		[DescriptionAttribute("Description..."),
        //		CategoryAttribute("SiteProperty")]
        //		public string Domin
        //		{
        //			get 
        //			{
        //				return getSiteString("domin");
        //			}
        //			set 
        //			{
        //				setSiteString("domin",value);
        //			}
        //		}
        //		[DescriptionAttribute("Description..."),
        //		CategoryAttribute("SiteProperty")]
        //		public string Caption
        //		{
        //			get 
        //			{
        //				return getSiteString("caption");
        //			}
        //			set 
        //			{
        //				setSiteString("caption",value);
        //			}
        //		}
        //		[DescriptionAttribute("Description..."),
        //		CategoryAttribute("SiteProperty")]
        //		public string UserName
        //		{
        //			get 
        //			{
        //				return getSiteString("username");
        //			}
        //			set 
        //			{
        //				setSiteString("username",value);
        //			}
        //		}
        //		[DescriptionAttribute("Description..."),
        //		CategoryAttribute("SiteProperty")]
        //		public string PassWord
        //		{
        //			get 
        //			{
        //				return getSiteString("passwd");
        //			}
        //			set 
        //			{
        //				setSiteString("passwd",value);
        //			}
        //		}
        //		[DescriptionAttribute("Description..."),
        //		CategoryAttribute("SiteProperty")]
        //		public string CDKey
        //		{
        //			get 
        //			{
        //				return getSiteString("cdkey");
        //			}
        //			set 
        //			{
        //				setSiteString("cdkey",value);
        //			}
        //		}
        //		[DescriptionAttribute("Description..."),
        //		CategoryAttribute("SiteProperty"),ReadOnlyAttribute(true)]
        //		public string Version
        //		{
        //			get 
        //			{
        //				return getSiteString("version");
        //			}
        //		}
        //		[DescriptionAttribute("Description..."),
        //		CategoryAttribute("SiteProperty")]
        //		public string URL
        //		{
        //			get 
        //			{
        //				return getSiteString("url");
        //			}
        //			set 
        //			{
        //				setSiteString("url",value);
        //			}
        //		}
        //		[DescriptionAttribute("Description..."),
        //		CategoryAttribute("SiteProperty")]
        //		public string HomePage
        //		{
        //			get 
        //			{
        //				return getSiteString("homepage");
        //			}
        //			set 
        //			{
        //				setSiteString("homepage",value);
        //			}
        //		}
        //		[DescriptionAttribute("Description..."),
        //		CategoryAttribute("SiteProperty")]
        //		public string GroupName
        //		{
        //			get 
        //			{
        //				return getSiteString("groupname");
        //			}
        //			set 
        //			{
        //				setSiteString("groupname",value);
        //			}
        //		}
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(site_GetValue);
            bag.SetValue += new PropertySpecEventHandler(site_SetValue);
            PropertySpec ps;
            string curcate = res._propertysite.GetString("c1");
            string ftpcate = res._propertysite.GetString("ftpcate");
            string adcate = res._propertysite.GetString("ad");
            ps = new PropertySpec("ID", typeof(string), curcate, res._propertysite.GetString("d1"), globalConst.CurSite.ID);
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);

            ps = new PropertySpec("Version", typeof(string), curcate, res._propertysite.GetString("d2"), getSiteString("version"));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);

            bag.Properties.Add(new PropertySpec("Domin", typeof(string), curcate, res._propertysite.GetString("d3"), getSiteString("domin")));
            bag.Properties.Add(new PropertySpec("Caption", typeof(string), curcate, res._propertysite.GetString("d4"), getSiteString("caption")));
            bag.Properties.Add(new PropertySpec("UserName", typeof(string), curcate, res._propertysite.GetString("d5"), getSiteString("username")));
            bag.Properties.Add(new PropertySpec("PassWord", typeof(string), curcate, res._propertysite.GetString("d6"), getSiteString("passwd")));
            bag.Properties.Add(new PropertySpec("CDKey", typeof(string), curcate, res._propertysite.GetString("d7"), getSiteString("cdkey")));
            bag.Properties.Add(new PropertySpec("URL", typeof(string), curcate, res._propertysite.GetString("d8"), getSiteString("url")));
            bag.Properties.Add(new PropertySpec("HomePage", typeof(string), curcate, res._propertysite.GetString("d9"), getSiteString("homepage"), typeof(HomePageEditor), typeof(HomePageEditorConverter)));
            bag.Properties.Add(new PropertySpec("GroupName", typeof(string), curcate, res._propertysite.GetString("d10"), getSiteString("groupname")));

            bag.Properties.Add(new PropertySpec(res._propertysite.GetString("ftpurl"), typeof(string), ftpcate, res._propertysite.GetString("ftpurl_des"), getSiteString("ftpurl")));
            bag.Properties.Add(new PropertySpec(res._propertysite.GetString("ftpname"), typeof(string), ftpcate, res._propertysite.GetString("ftpname_des"), getSiteString("ftpname")));
            bag.Properties.Add(new PropertySpec(res._propertysite.GetString("ftppswd"), typeof(string), ftpcate, res._propertysite.GetString("ftppswd_des"), getSiteString("ftppswd")));
            bag.Properties.Add(new PropertySpec(res._propertysite.GetString("ftpport"), typeof(int), ftpcate, res._propertysite.GetString("ftpport_des"), getSiteInt32("ftpport")));

            bag.Properties.Add(new PropertySpec(res._propertysite.GetString("ad_formtable"), typeof(string), adcate, res._propertysite.GetString("ad_formtable_des"), getFormDataCount(), typeof(FormDataEditor), typeof(FormDataEditorConverter)));
            
            return bag;
        }
        private static string getFormDataCount()
        {
            string filename=globalConst.ConfigPath+"\\"+globalConst.CurSite.ID+"_formtable.xml";
            if(!file.Exists(filename))return "Not any form table be found";
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            return "Table Count : "+doc.SelectNodes("/formdata/table").Count;
        }
        private static void site_GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            switch (pName)
            {
                case "ID":
                    e.Value = globalConst.CurSite.ID;
                    break;
                case "Version":
                    e.Value = getSiteString("version");
                    break;
                case "Domin":
                    e.Value = getSiteString("domin");
                    break;
                case "Caption":
                    e.Value = getSiteString("caption");
                    break;
                case "UserName":
                    e.Value = getSiteString("username");
                    break;
                case "PassWord":
                    e.Value = getSiteString("passwd");
                    break;
                case "CDKey":
                    e.Value = getSiteString("cdkey");
                    break;
                case "URL":
                    e.Value = getSiteString("url");
                    break;
                case "HomePage":
                    e.Value = getSiteString("homepage");
                    break;
                case "GroupName":
                    e.Value = getSiteString("groupname");
                    break;
                default:
                    if (pName.Equals(res._propertysite.GetString("ftpurl")))
                    {
                        e.Value = getSiteString("ftpurl");
                        return;
                    }
                    if (pName.Equals(res._propertysite.GetString("ftpname")))
                    {
                        e.Value = getSiteString("ftpname");
                        return;
                    }
                    if (pName.Equals(res._propertysite.GetString("ftppswd")))
                    {
                        e.Value = getSiteString("ftppswd");
                        return;
                    }
                    if (pName.Equals(res._propertysite.GetString("ftpport")))
                    {
                        e.Value = getSiteInt32("ftpport");
                        return;
                    }
                    if (pName.Equals(res._propertysite.GetString("ad_formtable")))
                    {
                        e.Value =getFormDataCount();
                        return;
                    }
                    break;
            }
        }
        private static void site_SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            switch (pName)
            {
                case "Domin":
                    setSiteString("domin", pValue);
                    if (tree.getTypeFromID(tree.getID(globalConst.MdiForm.SiteTree.SelectedNode)).Equals("root"))
                    {
                        globalConst.MdiForm.SiteTree.SelectedNode.Text = pValue;
                        ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[0] = pValue;
                        ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[1] = pValue;
                    }
                    globalConst.CurSite.Domain = pValue;
                    break;
                case "Caption":
                    setSiteString("caption", pValue);
                    globalConst.CurSite.Caption = pValue;
                    break;
                case "UserName":
                    setSiteString("username", pValue);
                    break;
                case "PassWord":
                    setSiteString("passwd", pValue);
                    break;
                case "CDKey":
                    setSiteString("cdkey", pValue);
                    break;
                case "URL":
                    setSiteString("url", pValue);
                    globalConst.CurSite.URL = pValue;
                    break;
                case "HomePage":
                    if (SiteClass.Site.setHomePage(globalConst.CurSite.Path,pValue))
                    {
                        setSiteString("homepage", pValue);
                    }
                    break;
                case "GroupName":
                    setSiteString("groupname", pValue);
                    break;
                default:
                    if (pName.Equals(res._propertysite.GetString("ftpurl")))
                    {
                        setSiteString("ftpurl", pValue);
                        return;
                    }
                    if (pName.Equals(res._propertysite.GetString("ftpname")))
                    {
                        setSiteString("ftpname", pValue);
                        return;
                    }
                    if (pName.Equals(res._propertysite.GetString("ftppswd")))
                    {
                        setSiteString("ftppswd", pValue);
                        return;
                    }
                    if (pName.Equals(res._propertysite.GetString("ftpport")))
                    {
                        setSiteInt32("ftpport", int.Parse(pValue));
                        return;
                    }
                    if (pName.Equals(res._propertysite.GetString("ad_formtable")))
                    {
                        return;
                    }
                    break;
            }
        }
    }

    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class PropertyDirectory
    {
        public static string dirid;
        public PropertyDirectory()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
        }
        private static string getDirString(string name)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "select " + name + " from directory where id='" + dirid + "'";
            return globalConst.CurSite.SiteConn.GetString(sql);
        }
        private static void setDirString(string name, string _value)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "update directory set " + name + "='" + _value.Replace("'", "''") + "' where id='" + dirid + "'";
            globalConst.CurSite.SiteConn.execSql(sql);
        }
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(dir_GetValue);
            bag.SetValue += new PropertySpecEventHandler(dir_SetValue);
            PropertySpec ps;
            string curcate = res._propertysite.GetString("c2");
            ps = new PropertySpec("ID", typeof(string), curcate, res._propertysite.GetString("e1"), dirid);
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);

            ps = new PropertySpec("UpdateTime", typeof(string), curcate, res._propertysite.GetString("e2"), getDirString("updatetime"));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec("Path", typeof(string), curcate, res._propertysite.GetString("e6"), globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            bag.Properties.Add(new PropertySpec("Name", typeof(string), curcate, res._propertysite.GetString("e3"), getDirString("name")));
            bag.Properties.Add(new PropertySpec("Caption", typeof(string), curcate, res._propertysite.GetString("e4"), getDirString("caption")));
            bag.Properties.Add(new PropertySpec("HomePage", typeof(string), curcate, res._propertysite.GetString("e5"), getDirString("homepage"), typeof(HomePageEditor), typeof(HomePageEditorConverter)));

            return bag;
        }
        private static void dir_GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            switch (pName)
            {
                case "ID":
                    e.Value = dirid;
                    break;
                case "Name":
                    e.Value = getDirString("name");
                    break;
                case "Caption":
                    e.Value = getDirString("caption");
                    break;
                case "UpdateTime":
                    e.Value = getDirString("updatetime");
                    break;
                case "Path":
                    e.Value = globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode);
                    break;
                case "HomePage":
                    e.Value = getDirString("homepage");
                    break;
                default:
                    break;
            }
        }
        private static void dir_SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            switch (pName)
            {
                case "Name":
                    pValue = pValue.Trim();
                    if (pValue.IndexOf("/") >= 0 || pValue.IndexOf("*") >= 0 || pValue.IndexOf("?") >= 0 || pValue.IndexOf("\\") >= 0)
                    {
                        MsgBox.Warning(res._propertysite.GetString("m1"));
                        return;
                    }
                    string path = globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode);
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
                    if (!di.Exists)
                    {
                        MsgBox.Warning(path + res._propertysite.GetString("m2"));
                        return;
                    }
                    if (dir.Exists(di.Parent.FullName + @"\" + pValue))
                    {
                        MsgBox.Warning(di.Parent.FullName + @"\" + pValue + res._propertysite.GetString("m3"));
                        return;
                    }
                    try
                    {
                        di.MoveTo(di.Parent.FullName + @"\" + pValue);
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error(ex.Message);
                        return;
                    }
                    setDirString("name", pValue);
                    setDirString("updatetime", DateTime.Now.ToString());
                    ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[0] = pValue;
                    if (globalConst.siteTreeShowColName.Equals("name"))
                    {
                        globalConst.MdiForm.SiteTree.SelectedNode.Text = pValue;
                    }
                    //change opend editor'url property
                    foreach (System.Windows.Forms.Form fm in globalConst.MdiForm.MdiChildren)
                    {
                        if (fm.Name.Equals("Editor"))
                        {
                            Editor edr = (Editor)fm;
                            if (edr.thisUrl.StartsWith(path))
                            {
                                edr.thisUrl = edr.thisUrl.Replace(path, di.Parent.FullName + @"\" + pValue);
                                edr.thisEditUrl = edr.thisUrl + "_edit.htm";
                                edr.thisViewUrl = edr.thisUrl + "_view.htm";
                            }
                            edr = null;
                        }
                    }

                    //					int i;
                    //					Form[] fms=globalConst.MdiForm.MdiChildren;
                    //					for(i=0;i<fms.Length;i++)
                    //					{
                    //						if(fms[i].Name.Equals("Editor"))
                    //						{
                    //							j++;
                    //						}
                    //					}
                    //					fms=null;
                    break;
                case "Caption":
                    setDirString("caption", pValue);
                    ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[1] = pValue;
                    if (globalConst.siteTreeShowColName.Equals("caption"))
                    {
                        globalConst.MdiForm.SiteTree.SelectedNode.Text = pValue;
                    }
                    //just 4 page
                    //					Editor ed=form.getEditor(((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[2]);
                    //					if(ed!=null)
                    //					{
                    //						ed.Text=pValue;
                    //					}
                    //					ed=null;
                    break;
                case "HomePage":
                    if (SiteClass.Site.setHomePage(globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode), pValue))
                    {
                        setDirString("homepage", pValue);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class PropertyPage
    {
        private static AccessObj accessObj;
        public static string pageid;
        public PropertyPage()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
        }
        private static string getPageString(string name)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "select " + name + " from pages where id='" + pageid + "'";
            return globalConst.CurSite.SiteConn.GetString(sql);
        }
        private static void setPageString(string name, string _value)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "update pages set " + name + "='" + _value.Replace("'", "''") + "' where id='" + pageid + "'";
            globalConst.CurSite.SiteConn.execSql(sql);
        }
        private static int getPageInt(string name)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "select " + name + " from pages where id='" + pageid + "'";
            return globalConst.CurSite.SiteConn.GetInt32(sql);
        }
        private static void setPageInt(string name, int _value)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "update pages set " + name + "='" + _value + "' where id='" + pageid + "'";
            globalConst.CurSite.SiteConn.execSql(sql);
        }
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(page_GetValue);
            bag.SetValue += new PropertySpecEventHandler(page_SetValue);
            PropertySpec ps;
            string curcate =res._propertysite.GetString("c3");
            ps = new PropertySpec("ID", typeof(string), curcate, res._propertysite.GetString("f1"), pageid);
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);

            ps = new PropertySpec("UpdateTime", typeof(string), curcate, res._propertysite.GetString("f2"), getPageString("updatetime"));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);

            ps = new PropertySpec("Path", typeof(string), curcate, res._propertysite.GetString("f3"), globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);


            bag.Properties.Add(new PropertySpec("Name", typeof(string), curcate, res._propertysite.GetString("f4"), getPageString("name")));
            bag.Properties.Add(new PropertySpec("Caption", typeof(string), curcate, res._propertysite.GetString("f5"), getPageString("caption")));

            accessObj = new AccessObj();
            accessObj.setA_ip_s(getPageInt("a_ip_s"));
            accessObj.setA_ip_c(getPageString("a_ip_c"));
            accessObj.setA_ip_o(getPageInt("a_ip_o"));
            accessObj.setA_se_s(getPageInt("a_se_s"));
            accessObj.setA_se_c(getPageString("a_se_c"));
            accessObj.setA_se_o(getPageInt("a_se_o"));
            accessObj.setA_jp_s(getPageInt("a_jp_s"));
            accessObj.setA_jp_u(getPageString("a_jp_u"));
            accessObj.setA_tp_s(getPageInt("a_tp_s"));
            accessObj.setA_tp_c(getPageString("a_tp_c"));

            ps = new PropertySpec(PropertyPart.AccessIP, typeof(PropertyTable), PropertyPart.AccessMain, PropertyPart.AccessIP_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(PropertyPart.AccessSession, typeof(PropertyTable), PropertyPart.AccessMain, PropertyPart.AccessSession_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(PropertyPart.AccessJump, typeof(PropertyTable), PropertyPart.AccessMain, PropertyPart.AccessJump_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(PropertyPart.AccessTip, typeof(PropertyTable), PropertyPart.AccessMain, PropertyPart.AccessTip_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);

            bag.Properties.Add(new PropertySpec("页面JS",typeof(string), "高级", "设置页面级JS脚本，嵌入到页面指定JS代码区域", getPageJS(), typeof(classes.TextEditor), typeof(TextEditorConverter)));

            return bag;
        }
        private static string getPageJS()
        {
            //Editor ed = form.getEditor(pageid);
            //if (ed == null) return "须打开页面";
            //IHTMLElement ele = ed.editocx.getElementById("ftdp_page_js");
            //if (ele == null) return "";
            //IHTMLScriptElement eleS = (IHTMLScriptElement)ele;
            //string val = eleS.text;
            //if (val.Length > 20) val = val.Substring(0, 17) + "...";
            string val = getPageString("pagejs");
            if (val.Length > 20) val = val.Substring(0, 17) + "...";
            return val;
        }
        private static void page_GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (e.Property.Category.Equals(PropertyPart.AccessMain))
            {
                if (pName.Equals(PropertyPart.AccessIP))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { PropertyPart.AccessActiveNot, PropertyPart.AccessActive };
                    string[] ActiveNormal = new string[] { PropertyPart.AccessNormal, PropertyPart.AccessNormalNot };
                    //取值
                    string AccessIPControl_Value = (1 == accessObj.getA_ip_s() ? PropertyPart.AccessActive : PropertyPart.AccessActiveNot);
                    string AccessIPCondition_Value = accessObj.getA_ip_c();
                    string AccessIPConditionSide_Value = (1 == accessObj.getA_ip_o() ? PropertyPart.AccessNormalNot : PropertyPart.AccessNormal);
                    PropertyTable bagAccess = new PropertyTable();
                    //IP控制
                    AccessActiveDeactiveEnumEditorPage.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditorPage.v = AccessIPControl_Value;
                    AccessActiveDeactiveEnumEditorPage.Column = "a_ip_s";
                    psAccess = new PropertySpec(PropertyPart.AccessIPControl, typeof(string), "_AccessIPControl", PropertyPart.AccessIPControl_Des, AccessIPControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessIPControl] = AccessIPControl_Value;
                    //IP条件
                    AccessIPConditionEditorPageConverter.PageID = pageid;
                    AccessIPConditionEditorPageConverter.v = AccessIPCondition_Value;
                    psAccess = new PropertySpec(PropertyPart.AccessIPCondition, typeof(string), "_AccessIPCondition", PropertyPart.AccessIPCondition_Des, AccessIPCondition_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessIPConditionEditorPageConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessIPCondition] = AccessIPCondition_Value;
                    //IP条件置反
                    AccessActiveDeactiveEnumEditorPage2.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage2.Enums = ActiveNormal;
                    AccessActiveDeactiveEnumEditorPage2.v = AccessIPConditionSide_Value;
                    AccessActiveDeactiveEnumEditorPage2.Column = "a_ip_o";
                    psAccess = new PropertySpec(PropertyPart.AccessIPConditionSide, typeof(string), "_AccessIPConditionSide", PropertyPart.AccessIPConditionSide_Des, AccessIPConditionSide_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage2));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessIPConditionSide] = AccessIPConditionSide_Value;
                    e.Value = bagAccess;
                    return;
                }
                if (pName.Equals(PropertyPart.AccessSession))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { PropertyPart.AccessActiveNot, PropertyPart.AccessActive };
                    string[] ActiveNormal = new string[] { PropertyPart.AccessNormal, PropertyPart.AccessNormalNot };
                    string AccessSessionControl_Value = (1 == accessObj.getA_se_s() ? PropertyPart.AccessActive : PropertyPart.AccessActiveNot);
                    string AccessSessionCondition_Value = accessObj.getA_se_c();
                    string AccessSessionConditionSide_Value = (1 == accessObj.getA_se_o() ? PropertyPart.AccessNormalNot : PropertyPart.AccessNormal);
                    PropertyTable bagAccess = new PropertyTable();
                    //会话控制
                    AccessActiveDeactiveEnumEditorPage.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditorPage.v = AccessSessionControl_Value;
                    AccessActiveDeactiveEnumEditorPage.Column = "a_se_s";
                    psAccess = new PropertySpec(PropertyPart.AccessSessionControl, typeof(string), "_AccessSessionControl", PropertyPart.AccessSessionControl_Des, AccessSessionControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessSessionControl] = AccessSessionControl_Value;
                    //会话条件
                    AccessSessionConditionEditorPageConverter.PageID = pageid;
                    AccessSessionConditionEditorPageConverter.v = AccessSessionCondition_Value;
                    psAccess = new PropertySpec(PropertyPart.AccessSessionCondition, typeof(string), "_AccessSessionCondition", PropertyPart.AccessSessionCondition_Des, AccessSessionCondition_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessSessionConditionEditorPageConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessSessionCondition] = AccessSessionCondition_Value;
                    //会话条件置反
                    AccessActiveDeactiveEnumEditorPage2.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage2.Enums = ActiveNormal;
                    AccessActiveDeactiveEnumEditorPage2.v = AccessSessionConditionSide_Value;
                    AccessActiveDeactiveEnumEditorPage2.Column = "a_se_o";
                    psAccess = new PropertySpec(PropertyPart.AccessSessionConditionSide, typeof(string), "_AccessSessionConditionSide", PropertyPart.AccessSessionConditionSide_Des, AccessSessionConditionSide_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage2));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessSessionConditionSide] = AccessSessionConditionSide_Value;
                    e.Value = bagAccess;
                    return;
                }
                if (pName.Equals(PropertyPart.AccessJump))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { PropertyPart.AccessActiveNot, PropertyPart.AccessActive };
                    string[] ActiveNormal = new string[] { PropertyPart.AccessNormal, PropertyPart.AccessNormalNot };
                    string AccessJumpControl_Value = (1 == accessObj.getA_jp_s() ? PropertyPart.AccessActive : PropertyPart.AccessActiveNot);
                    string AccessJumpAddress_Value = accessObj.getA_jp_u();
                    PropertyTable bagAccess = new PropertyTable();
                    //跳转设置
                    AccessActiveDeactiveEnumEditorPage.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditorPage.v = AccessJumpControl_Value;
                    AccessActiveDeactiveEnumEditorPage.Column = "a_jp_s";
                    psAccess = new PropertySpec(PropertyPart.AccessJumpControl, typeof(string), "_AccessJumpControl", PropertyPart.AccessJumpControl_Des, AccessJumpControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessJumpControl] = AccessJumpControl_Value;
                    //跳转地址
                    AccessUrlEditorPageConverter.PageID = pageid;
                    AccessUrlEditorPageConverter.v = AccessJumpAddress_Value;
                    AccessUrlEditorPageConverter.Column = "a_jp_u";
                    psAccess = new PropertySpec(PropertyPart.AccessJumpAddress, typeof(string), "_AccessJumpAddress", PropertyPart.AccessJumpAddress_Des, AccessJumpAddress_Value, typeof(HomePageEditor), typeof(AccessUrlEditorPageConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessJumpAddress] = AccessJumpAddress_Value;

                    e.Value = bagAccess;
                    return;
                }
                if (pName.Equals(PropertyPart.AccessTip))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { PropertyPart.AccessActiveNot, PropertyPart.AccessActive };
                    string[] ActiveNormal = new string[] { PropertyPart.AccessNormal, PropertyPart.AccessNormalNot };
                    string AccessTipControl_Value = (1 == accessObj.getA_tp_s() ? PropertyPart.AccessActive : PropertyPart.AccessActiveNot);
                    string AccessTipContent_Value = accessObj.getA_tp_c();
                    PropertyTable bagAccess = new PropertyTable();
                    //提示设置
                    AccessActiveDeactiveEnumEditorPage.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditorPage.v = AccessTipControl_Value;
                    AccessActiveDeactiveEnumEditorPage.Column = "a_tp_s";
                    psAccess = new PropertySpec(PropertyPart.AccessTipControl, typeof(string), "_AccessTipControl", PropertyPart.AccessTipControl_Des, AccessTipControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessTipControl] = AccessTipControl_Value;
                    //提示内容
                    AccessTipContentEditorPageConverter.PageID = pageid;
                    AccessTipContentEditorPageConverter.v = AccessTipContent_Value;
                    psAccess = new PropertySpec(PropertyPart.AccessTipContent, typeof(string), "_AccessTipContent", PropertyPart.AccessTipContent_Des, AccessTipContent_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessTipContentEditorPageConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessTipContent] = AccessTipContent_Value;

                    e.Value = bagAccess;
                    return;
                }
                return;
            }
            switch (pName)
            {
                case "ID":
                    e.Value = pageid;
                    break;
                case "Name":
                    e.Value = getPageString("name");
                    break;
                case "Path":
                    e.Value = globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode);
                    break;
                case "Caption":
                    e.Value = getPageString("caption");
                    break;
                case "UpdateTime":
                    e.Value = getPageString("updatetime");
                    break;
                default:
                    if (pName.Equals("页面JS"))
                    {
                        e.Value = getPageString("pagejs");
                        return;
                    }
                    break;
            }
        }
        private static void page_SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            switch (pName)
            {
                case "Name":
                    pValue = pValue.Trim();
                    if (pValue.IndexOf("/") >= 0 || pValue.IndexOf("*") >= 0 || pValue.IndexOf("?") >= 0 || pValue.IndexOf("\\") >= 0)
                    {
                        MsgBox.Warning(res._propertysite.GetString("m4"));
                        return;
                    }
                    string path = globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode);
                    System.IO.FileInfo fi = new System.IO.FileInfo(path);
                    if (!fi.Exists)
                    {
                        MsgBox.Warning(path + res._propertysite.GetString("m5"));
                        return;
                    }
                    if (file.Exists(fi.DirectoryName + @"\" + pValue))
                    {
                        MsgBox.Warning(fi.DirectoryName + @"\" + pValue + res._propertysite.GetString("m6"));
                        return;
                    }
                    try
                    {
                        fi.MoveTo(fi.DirectoryName + @"\" + pValue);
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error(ex.Message);
                        return;
                    }
                    setPageString("name", pValue);
                    setPageString("updatetime", DateTime.Now.ToString());
                    ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[0] = pValue;
                    if (globalConst.siteTreeShowColName.Equals("name"))
                    {
                        globalConst.MdiForm.SiteTree.SelectedNode.Text = pValue;
                    }
                    //change opend editor'url property
                    Editor edr = form.getEditor(((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[2]);
                    if (edr != null && edr.thisUrl.Equals(path))
                    {
                        edr.thisUrl = fi.DirectoryName + @"\" + pValue;
                        edr.thisEditUrl = edr.thisUrl + "_edit.htm";
                        edr.thisViewUrl = edr.thisUrl + "_view.htm";
                    }
                    edr = null;
                    //update name in ctrltree
                    System.Windows.Forms.TreeNode td = tree.getCtrlNodeByID(((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[2]);
                    if (td != null)
                    {
                        ((string[])(td.Tag))[0] = pValue;
                        if (globalConst.ctrlTreeShowColName.Equals("name"))
                        {
                            td.Text = pValue;
                        }
                    }
                    break;
                case "Caption":
                    setPageString("caption", pValue);
                    ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[1] = pValue;
                    if (globalConst.siteTreeShowColName.Equals("caption"))
                    {
                        globalConst.MdiForm.SiteTree.SelectedNode.Text = pValue;
                    }
                    //just 4 page
                    string thisID = ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[2];
                    Editor ed = form.getEditor(thisID);
                    if (ed != null)
                    {
                        ed.Text = pValue;
                        form.UpdateFileOpend(thisID, true);
                    }
                    ed = null;
                    System.Windows.Forms.TreeNode tdr = tree.getCtrlNodeByID(((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[2]);
                    if (tdr != null)
                    {
                        ((string[])(tdr.Tag))[1] = pValue;
                        if (globalConst.ctrlTreeShowColName.Equals("caption"))
                        {
                            tdr.Text = pValue;
                        }
                    }
                    break;
                case "页面JS":
                    setPageString("pagejs", pValue);
                    /*
                    Editor ed2 = form.getEditor(pageid);
                    if (ed2 == null || ed2.editmode!= "edit") MsgBox.Warning("须打开页面并在编辑模式");
                    else
                    {
                        IHTMLElement ele = ed2.editocx.getElementById("ftdp_page_js");
                        if (ele == null)
                        {
                            //IHTMLElement ele2 = (IHTMLElement)ed2.editocx.getElementsByTagName("head").item(0);
                            //ele2.insertAdjacentHTML("beforeEnd", "<div>111<script language='javascript' id='ftdp_page_js'></script></div>"); MsgBox.Information(ele2.outerHTML);
                            // IHTMLElement aaa = ed2.editocx.createElement("script");
                            //aaa.id = "ftdp_page_js";
                            //aaa.setAttribute("type", "javascript");
                            //aaa.innerText = pValue;
                            //ed2.editocx_onselectionchange(ele2);
                            while(true)
                            {
                                ed2.editocx.pasteHtml("<input type=hidden id='ftdp_page_js_hdn'><script language='javascript' id='ftdp_page_js'>" + pValue + "</script>");
                                ele = ed2.editocx.getElementById("ftdp_page_js");
                                if (ele == null)
                                {
                                    Clipboard.SetText(pValue);
                                    if (MsgBox.YesNo("未成功添加，请手动添加JS标签或在页面上指定插入位置（内容已复制）。\r\n是否重试？") == DialogResult.No)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    MsgBox.Information(ed2.editocx.DocumentHTML);
                                    ed2.editocx.getElementById("ftdp_page_js_hdn").outerHTML = "";
                                    break;
                                }
                            }
                            //ed2.editocx.pasteHtml("<script language='javascript' id='ftdp_page_js'>"+ pValue + "</script>");
                            //ed2.editocx.document
                            //ed2.editocx.appendChild((IHTMLDOMNode)aaa);
                            //MsgBox.Information(ed2.editocx.DocumentHTML);
                            //ed2.savePage();
                        }
                        else
                        {
                            while (true)
                            {
                                ele.outerHTML = "<input type=hidden id='ftdp_page_js_hdn'><script language='javascript' id='ftdp_page_js'>" + pValue + "</script>";
                                ele = ed2.editocx.getElementById("ftdp_page_js");
                                string jstext = ((IHTMLScriptElement)ele).text;
                                MsgBox.Information(pValue + "\r\n" + jstext);
                                if (pValue != jstext)
                                {
                                    Clipboard.SetText(pValue);
                                    if (MsgBox.YesNo("未成功更新（内容已复制）。\r\n是否重试？") == DialogResult.No)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    MsgBox.Information(ed2.editocx.DocumentHTML);
                                    ed2.editocx.getElementById("ftdp_page_js_hdn").outerHTML = "";
                                    break;
                                }
                            }
                        }

                    }*/
                    break;
                default:
                    break;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class PropertyPageForm1
    {
        private static AccessObj accessObj;
        public static string pageid;
        public PropertyPageForm1()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
        }
        private static string getPageString(string name)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "select " + name + " from pages where id='" + pageid + "'";
            return globalConst.CurSite.SiteConn.GetString(sql);
        }
        private static void setPageString(string name, string _value)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "update pages set " + name + "='" + _value.Replace("'", "''") + "' where id='" + pageid + "'";
            globalConst.CurSite.SiteConn.execSql(sql);
        }
        private static int getPageInt(string name)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "select " + name + " from pages where id='" + pageid + "'";
            return globalConst.CurSite.SiteConn.GetInt32(sql);
        }
        private static void setPageInt(string name, int _value)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "update pages set " + name + "='" + _value + "' where id='" + pageid + "'";
            globalConst.CurSite.SiteConn.execSql(sql);
        }
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(page_GetValue);
            bag.SetValue += new PropertySpecEventHandler(page_SetValue);
            PropertySpec ps;
            string curcate = res._propertysite.GetString("c3");
            ps = new PropertySpec("ID", typeof(string), curcate, res._propertysite.GetString("f1"), pageid);
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);

            ps = new PropertySpec("UpdateTime", typeof(string), curcate, res._propertysite.GetString("f2"), getPageString("updatetime"));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);

            ps = new PropertySpec("Path", typeof(string), curcate, res._propertysite.GetString("f3"), globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);


            bag.Properties.Add(new PropertySpec("Name", typeof(string), curcate, res._propertysite.GetString("f4"), getPageString("name")));
            bag.Properties.Add(new PropertySpec("Caption", typeof(string), curcate, res._propertysite.GetString("f5"), getPageString("caption")));

            curcate = res.form.GetString("String17");
            ps = new PropertySpec(res.form.GetString("String30"), typeof(string), curcate, res.form.GetString("String30"), res.form.GetString("String31"));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            bag.Properties.Add(new PropertySpec(res.form.GetString("String18"), typeof(string), curcate, res.form.GetString("String24"), (getPageInt("mtype") == 0 ? res.form.GetString("String20") : (getPageInt("mtype") == 1 ? res.form.GetString("String21") : res.form.GetString("String22"))), typeof(System.Drawing.Design.UITypeEditor), typeof(FormPageMemberType)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String26"), typeof(string), curcate, res.form.GetString("String27"), getPageString("jinfo")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String77"), typeof(string), curcate, res.form.GetString("String78"), getPageString("fid")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String28"), typeof(string), curcate, res.form.GetString("String29"), getPageString("jurl"), typeof(HomePageEditor), typeof(HomePageEditorConverter)));
            RuleEditor.pageid = pageid;
            bag.Properties.Add(new PropertySpec(res.form.GetString("String23"), typeof(string), curcate, res.form.GetString("String25"), "", typeof(RuleEditor), typeof(RuleEditorConverter)));

            curcate = res.form.GetString("String107");
            bag.Properties.Add(new PropertySpec(res.form.GetString("String113"), typeof(string), curcate, res.form.GetString("String114"), (getPageInt("viewopen") == 0 ? res.form.GetString("String115") : res.form.GetString("String116")), typeof(System.Drawing.Design.UITypeEditor), typeof(FormPageModViewType)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String111"), typeof(string), curcate, res.form.GetString("String112"), (getPageInt("modopen") == 0 ? res.form.GetString("String115") : res.form.GetString("String116")), typeof(System.Drawing.Design.UITypeEditor), typeof(FormPageModViewType)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String117"), typeof(string), curcate, res.form.GetString("String118"), getPageString("datastr"), typeof(TableTagEditor), typeof(TableTagEditorConverter)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String119"), typeof(string), curcate, res.form.GetString("String120"), getPageString("paraname")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String121"), typeof(string), curcate, res.form.GetString("String122"), getPageString("membind")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String123"), typeof(string), curcate, res.form.GetString("String124"), getPageString("elecdt")));
            GetControlData.ControlName = "role";
            bag.Properties.Add(new PropertySpec(res.form.GetString("String125"), typeof(string), curcate, res.form.GetString("String126"), getPageString("roledata"), typeof(DataSourceEditor), typeof(DataSourceEditorConverter)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String127"), typeof(string), curcate, res.form.GetString("String128"), getPageString("rolesession")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String129"), typeof(string), curcate, res.form.GetString("String130"), getPageString("authrule")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String131"), typeof(string), curcate, res.form.GetString("String132"), getPageString("flowstat")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String133"), typeof(string), curcate, res.form.GetString("String134"), getPageString("norightinfo")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String135"), typeof(string), curcate, res.form.GetString("String136"), getPageString("norighturl"), typeof(HomePageEditor), typeof(HomePageEditorConverter)));
            accessObj = new AccessObj();
            accessObj.setA_ip_s(getPageInt("a_ip_s"));
            accessObj.setA_ip_c(getPageString("a_ip_c"));
            accessObj.setA_ip_o(getPageInt("a_ip_o"));
            accessObj.setA_se_s(getPageInt("a_se_s"));
            accessObj.setA_se_c(getPageString("a_se_c"));
            accessObj.setA_se_o(getPageInt("a_se_o"));
            accessObj.setA_jp_s(getPageInt("a_jp_s"));
            accessObj.setA_jp_u(getPageString("a_jp_u"));
            accessObj.setA_tp_s(getPageInt("a_tp_s"));
            accessObj.setA_tp_c(getPageString("a_tp_c"));

            ps = new PropertySpec(PropertyPart.AccessIP, typeof(PropertyTable), PropertyPart.AccessMain, PropertyPart.AccessIP_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(PropertyPart.AccessSession, typeof(PropertyTable), PropertyPart.AccessMain, PropertyPart.AccessSession_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(PropertyPart.AccessJump, typeof(PropertyTable), PropertyPart.AccessMain, PropertyPart.AccessJump_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(PropertyPart.AccessTip, typeof(PropertyTable), PropertyPart.AccessMain, PropertyPart.AccessTip_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);

            return bag;
        }
        private static void page_GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (e.Property.Category.Equals(PropertyPart.AccessMain))
            {
                if (pName.Equals(PropertyPart.AccessIP))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { PropertyPart.AccessActiveNot, PropertyPart.AccessActive };
                    string[] ActiveNormal = new string[] { PropertyPart.AccessNormal, PropertyPart.AccessNormalNot };
                    //取值
                    string AccessIPControl_Value = (1 == accessObj.getA_ip_s() ? PropertyPart.AccessActive : PropertyPart.AccessActiveNot);
                    string AccessIPCondition_Value = accessObj.getA_ip_c();
                    string AccessIPConditionSide_Value = (1 == accessObj.getA_ip_o() ? PropertyPart.AccessNormalNot : PropertyPart.AccessNormal);
                    PropertyTable bagAccess = new PropertyTable();
                    //IP控制
                    AccessActiveDeactiveEnumEditorPage.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditorPage.v = AccessIPControl_Value;
                    AccessActiveDeactiveEnumEditorPage.Column = "a_ip_s";
                    psAccess = new PropertySpec(PropertyPart.AccessIPControl, typeof(string), "_AccessIPControl", PropertyPart.AccessIPControl_Des, AccessIPControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessIPControl] = AccessIPControl_Value;
                    //IP条件
                    AccessIPConditionEditorPageConverter.PageID = pageid;
                    AccessIPConditionEditorPageConverter.v = AccessIPCondition_Value;
                    psAccess = new PropertySpec(PropertyPart.AccessIPCondition, typeof(string), "_AccessIPCondition", PropertyPart.AccessIPCondition_Des, AccessIPCondition_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessIPConditionEditorPageConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessIPCondition] = AccessIPCondition_Value;
                    //IP条件置反
                    AccessActiveDeactiveEnumEditorPage2.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage2.Enums = ActiveNormal;
                    AccessActiveDeactiveEnumEditorPage2.v = AccessIPConditionSide_Value;
                    AccessActiveDeactiveEnumEditorPage2.Column = "a_ip_o";
                    psAccess = new PropertySpec(PropertyPart.AccessIPConditionSide, typeof(string), "_AccessIPConditionSide", PropertyPart.AccessIPConditionSide_Des, AccessIPConditionSide_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage2));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessIPConditionSide] = AccessIPConditionSide_Value;
                    e.Value = bagAccess;
                    return;
                }
                if (pName.Equals(PropertyPart.AccessSession))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { PropertyPart.AccessActiveNot, PropertyPart.AccessActive };
                    string[] ActiveNormal = new string[] { PropertyPart.AccessNormal, PropertyPart.AccessNormalNot };
                    string AccessSessionControl_Value = (1 == accessObj.getA_se_s() ? PropertyPart.AccessActive : PropertyPart.AccessActiveNot);
                    string AccessSessionCondition_Value = accessObj.getA_se_c();
                    string AccessSessionConditionSide_Value = (1 == accessObj.getA_se_o() ? PropertyPart.AccessNormalNot : PropertyPart.AccessNormal);
                    PropertyTable bagAccess = new PropertyTable();
                    //会话控制
                    AccessActiveDeactiveEnumEditorPage.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditorPage.v = AccessSessionControl_Value;
                    AccessActiveDeactiveEnumEditorPage.Column = "a_se_s";
                    psAccess = new PropertySpec(PropertyPart.AccessSessionControl, typeof(string), "_AccessSessionControl", PropertyPart.AccessSessionControl_Des, AccessSessionControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessSessionControl] = AccessSessionControl_Value;
                    //会话条件
                    AccessSessionConditionEditorPageConverter.PageID = pageid;
                    AccessSessionConditionEditorPageConverter.v = AccessSessionCondition_Value;
                    psAccess = new PropertySpec(PropertyPart.AccessSessionCondition, typeof(string), "_AccessSessionCondition", PropertyPart.AccessSessionCondition_Des, AccessSessionCondition_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessSessionConditionEditorPageConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessSessionCondition] = AccessSessionCondition_Value;
                    //会话条件置反
                    AccessActiveDeactiveEnumEditorPage2.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage2.Enums = ActiveNormal;
                    AccessActiveDeactiveEnumEditorPage2.v = AccessSessionConditionSide_Value;
                    AccessActiveDeactiveEnumEditorPage2.Column = "a_se_o";
                    psAccess = new PropertySpec(PropertyPart.AccessSessionConditionSide, typeof(string), "_AccessSessionConditionSide", PropertyPart.AccessSessionConditionSide_Des, AccessSessionConditionSide_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage2));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessSessionConditionSide] = AccessSessionConditionSide_Value;
                    e.Value = bagAccess;
                    return;
                }
                if (pName.Equals(PropertyPart.AccessJump))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { PropertyPart.AccessActiveNot, PropertyPart.AccessActive };
                    string[] ActiveNormal = new string[] { PropertyPart.AccessNormal, PropertyPart.AccessNormalNot };
                    string AccessJumpControl_Value = (1 == accessObj.getA_jp_s() ? PropertyPart.AccessActive : PropertyPart.AccessActiveNot);
                    string AccessJumpAddress_Value = accessObj.getA_jp_u();
                    PropertyTable bagAccess = new PropertyTable();
                    //跳转设置
                    AccessActiveDeactiveEnumEditorPage.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditorPage.v = AccessJumpControl_Value;
                    AccessActiveDeactiveEnumEditorPage.Column = "a_jp_s";
                    psAccess = new PropertySpec(PropertyPart.AccessJumpControl, typeof(string), "_AccessJumpControl", PropertyPart.AccessJumpControl_Des, AccessJumpControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessJumpControl] = AccessJumpControl_Value;
                    //跳转地址
                    AccessUrlEditorPageConverter.PageID = pageid;
                    AccessUrlEditorPageConverter.v = AccessJumpAddress_Value;
                    AccessUrlEditorPageConverter.Column = "a_jp_u";
                    psAccess = new PropertySpec(PropertyPart.AccessJumpAddress, typeof(string), "_AccessJumpAddress", PropertyPart.AccessJumpAddress_Des, AccessJumpAddress_Value, typeof(HomePageEditor), typeof(AccessUrlEditorPageConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessJumpAddress] = AccessJumpAddress_Value;

                    e.Value = bagAccess;
                    return;
                }
                if (pName.Equals(PropertyPart.AccessTip))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { PropertyPart.AccessActiveNot, PropertyPart.AccessActive };
                    string[] ActiveNormal = new string[] { PropertyPart.AccessNormal, PropertyPart.AccessNormalNot };
                    string AccessTipControl_Value = (1 == accessObj.getA_tp_s() ? PropertyPart.AccessActive : PropertyPart.AccessActiveNot);
                    string AccessTipContent_Value = accessObj.getA_tp_c();
                    PropertyTable bagAccess = new PropertyTable();
                    //提示设置
                    AccessActiveDeactiveEnumEditorPage.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditorPage.v = AccessTipControl_Value;
                    AccessActiveDeactiveEnumEditorPage.Column = "a_tp_s";
                    psAccess = new PropertySpec(PropertyPart.AccessTipControl, typeof(string), "_AccessTipControl", PropertyPart.AccessTipControl_Des, AccessTipControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessTipControl] = AccessTipControl_Value;
                    //提示内容
                    AccessTipContentEditorPageConverter.PageID = pageid;
                    AccessTipContentEditorPageConverter.v = AccessTipContent_Value;
                    psAccess = new PropertySpec(PropertyPart.AccessTipContent, typeof(string), "_AccessTipContent", PropertyPart.AccessTipContent_Des, AccessTipContent_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessTipContentEditorPageConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessTipContent] = AccessTipContent_Value;

                    e.Value = bagAccess;
                    return;
                }
                return;
            }
            switch (pName)
            {
                case "ID":
                    e.Value = pageid;
                    break;
                case "Name":
                    e.Value = getPageString("name");
                    break;
                case "Path":
                    e.Value = globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode);
                    break;
                case "Caption":
                    e.Value = getPageString("caption");
                    break;
                case "UpdateTime":
                    e.Value = getPageString("updatetime");
                    break;
                default:
                    if (pName.Equals(res.form.GetString("String30")))
                    {
                        e.Value = res.form.GetString("String31");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String18")))
                    {
                        e.Value = (getPageInt("mtype") == 0 ? res.form.GetString("String20") : (getPageInt("mtype") == 1 ? res.form.GetString("String21") : res.form.GetString("String22")));
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String26")))
                    {
                        e.Value = getPageString("jinfo");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String77")))
                    {
                        e.Value = getPageString("fid");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String28")))
                    {
                        e.Value = getPageString("jurl");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String113")))
                    {
                        e.Value = getPageInt("viewopen") == 0 ? res.form.GetString("String115") : res.form.GetString("String116");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String111")))
                    {
                        e.Value = getPageInt("modopen") == 0 ? res.form.GetString("String115") : res.form.GetString("String116");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String117")))
                    {
                        e.Value = getPageString("datastr");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String119")))
                    {
                        e.Value = getPageString("paraname");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String121")))
                    {
                        e.Value = getPageString("membind");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String123")))
                    {
                        e.Value = getPageString("elecdt");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String125")))
                    {
                        e.Value = getPageString("roledata");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String127")))
                    {
                        e.Value = getPageString("rolesession");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String129")))
                    {
                        e.Value = getPageString("authrule");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String131")))
                    {
                        e.Value = getPageString("flowstat");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String133")))
                    {
                        e.Value = getPageString("norightinfo");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String135")))
                    {
                        e.Value = getPageString("norighturl");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String23")))
                    {
                        e.Value = "";
                        return;
                    }
                    break;
            }
        }
        private static void page_SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            switch (pName)
            {
                case "Name":
                    pValue = pValue.Trim();
                    if (pValue.IndexOf("/") >= 0 || pValue.IndexOf("*") >= 0 || pValue.IndexOf("?") >= 0 || pValue.IndexOf("\\") >= 0)
                    {
                        MsgBox.Warning(res._propertysite.GetString("m4"));
                        return;
                    }
                    string path = globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode);
                    System.IO.FileInfo fi = new System.IO.FileInfo(path);
                    if (!fi.Exists)
                    {
                        MsgBox.Warning(path + res._propertysite.GetString("m5"));
                        return;
                    }
                    if (file.Exists(fi.DirectoryName + @"\" + pValue))
                    {
                        MsgBox.Warning(fi.DirectoryName + @"\" + pValue + res._propertysite.GetString("m6"));
                        return;
                    }
                    try
                    {
                        fi.MoveTo(fi.DirectoryName + @"\" + pValue);
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error(ex.Message);
                        return;
                    }
                    setPageString("name", pValue);
                    setPageString("updatetime", DateTime.Now.ToString());
                    ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[0] = pValue;
                    if (globalConst.siteTreeShowColName.Equals("name"))
                    {
                        globalConst.MdiForm.SiteTree.SelectedNode.Text = pValue;
                    }
                    //change opend editor'url property
                    Editor edr = form.getEditor(((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[2]);
                    if (edr != null && edr.thisUrl.Equals(path))
                    {
                        edr.thisUrl = fi.DirectoryName + @"\" + pValue;
                        edr.thisEditUrl = edr.thisUrl + "_edit.htm";
                        edr.thisViewUrl = edr.thisUrl + "_view.htm";
                    }
                    edr = null;
                    //update name in ctrltree
                    System.Windows.Forms.TreeNode td = tree.getCtrlNodeByID(((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[2]);
                    if (td != null)
                    {
                        ((string[])(td.Tag))[0] = pValue;
                        if (globalConst.ctrlTreeShowColName.Equals("name"))
                        {
                            td.Text = pValue;
                        }
                    }
                    break;
                case "Caption":
                    setPageString("caption", pValue);
                    ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[1] = pValue;
                    if (globalConst.siteTreeShowColName.Equals("caption"))
                    {
                        globalConst.MdiForm.SiteTree.SelectedNode.Text = pValue;
                    }
                    //just 4 page
                    string thisID = ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[2];
                    Editor ed = form.getEditor(thisID);
                    if (ed != null)
                    {
                        ed.Text = pValue;
                        form.UpdateFileOpend(thisID, true);
                    }
                    ed = null;
                    System.Windows.Forms.TreeNode tdr = tree.getCtrlNodeByID(((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[2]);
                    if (tdr != null)
                    {
                        ((string[])(tdr.Tag))[1] = pValue;
                        if (globalConst.ctrlTreeShowColName.Equals("caption"))
                        {
                            tdr.Text = pValue;
                        }
                    }
                    break;
                default:
                    if (pName.Equals(res.form.GetString("String18")))
                    {
                        int setvalue = (pValue.Equals(res.form.GetString("String20")) ? 0 : (pValue.Equals(res.form.GetString("String21")) ? 1 : 2));
                        setPageInt("mtype", setvalue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String26")))
                    {
                        setPageString("jinfo", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String77")))
                    {
                        setPageString("fid", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String28")))
                    {
                        setPageString("jurl", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String113")))
                    {
                        int setvalue = (pValue.Equals(res.form.GetString("String115")) ? 0 : 1);
                        setPageInt("viewopen", setvalue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String111")))
                    {
                        int setvalue = (pValue.Equals(res.form.GetString("String115")) ? 0 : 1);
                        setPageInt("modopen", setvalue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String117")))
                    {
                        setPageString("datastr", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String119")))
                    {
                        setPageString("paraname", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String121")))
                    {
                        setPageString("membind", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String123")))
                    {
                        setPageString("elecdt", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String125")))
                    {
                        setPageString("roledata", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String127")))
                    {
                        setPageString("rolesession", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String129")))
                    {
                        setPageString("authrule", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String131")))
                    {
                        setPageString("flowstat", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String133")))
                    {
                        setPageString("norightinfo", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String135")))
                    {
                        setPageString("norighturl", pValue);
                        return;
                    }
                    break;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class PropertyPageForm2
    {
        private static AccessObj accessObj;
        public static string pageid;
        public PropertyPageForm2()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
        }
        private static string getPageString(string name)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "select " + name + " from pages where id='" + pageid + "'";
            return globalConst.CurSite.SiteConn.GetString(sql);
        }
        private static void setPageString(string name, string _value)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "update pages set " + name + "='" + _value.Replace("'", "''") + "' where id='" + pageid + "'";
            globalConst.CurSite.SiteConn.execSql(sql);
        }
        private static int getPageInt(string name)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "select " + name + " from pages where id='" + pageid + "'";
            return globalConst.CurSite.SiteConn.GetInt32(sql);
        }
        private static void setPageInt(string name, int _value)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string sql = "update pages set " + name + "='" + _value + "' where id='" + pageid + "'";
            globalConst.CurSite.SiteConn.execSql(sql);
        }
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(page_GetValue);
            bag.SetValue += new PropertySpecEventHandler(page_SetValue);
            PropertySpec ps;
            string curcate = res._propertysite.GetString("c3");
            ps = new PropertySpec("ID", typeof(string), curcate, res._propertysite.GetString("f1"), pageid);
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);

            ps = new PropertySpec("UpdateTime", typeof(string), curcate, res._propertysite.GetString("f2"), getPageString("updatetime"));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);

            ps = new PropertySpec("Path", typeof(string), curcate, res._propertysite.GetString("f3"), globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);


            bag.Properties.Add(new PropertySpec("Name", typeof(string), curcate, res._propertysite.GetString("f4"), getPageString("name")));
            bag.Properties.Add(new PropertySpec("Caption", typeof(string), curcate, res._propertysite.GetString("f5"), getPageString("caption")));

            curcate = res.form.GetString("String17");
            ps = new PropertySpec(res.form.GetString("String30"), typeof(string), curcate, res.form.GetString("String30"), res.form.GetString("String32"));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);bag.Properties.Add(new PropertySpec(res.form.GetString("String26"), typeof(string), curcate, res.form.GetString("String27"), getPageString("jinfo")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String77"), typeof(string), curcate, res.form.GetString("String78"), getPageString("fid")));

            bag.Properties.Add(new PropertySpec(res.form.GetString("String28"), typeof(string), curcate, res.form.GetString("String29"), getPageString("jurl"), typeof(HomePageEditor), typeof(HomePageEditorConverter)));
            RuleEditor.pageid = pageid;
            bag.Properties.Add(new PropertySpec(res.form.GetString("String23"), typeof(string), curcate, res.form.GetString("String25"), "", typeof(RuleEditor), typeof(RuleEditorConverter)));

            curcate = res.form.GetString("String107");
            bag.Properties.Add(new PropertySpec(res.form.GetString("String113"), typeof(string), curcate, res.form.GetString("String114"), (getPageInt("viewopen") == 0 ? res.form.GetString("String115") : res.form.GetString("String116")), typeof(System.Drawing.Design.UITypeEditor), typeof(FormPageModViewType)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String111"), typeof(string), curcate, res.form.GetString("String112"), (getPageInt("modopen") == 0 ? res.form.GetString("String115") : res.form.GetString("String116")), typeof(System.Drawing.Design.UITypeEditor), typeof(FormPageModViewType)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String117"), typeof(string), curcate, res.form.GetString("String118"), getPageString("datastr")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String119"), typeof(string), curcate, res.form.GetString("String120"), getPageString("paraname")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String121"), typeof(string), curcate, res.form.GetString("String122"), getPageString("membind")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String123"), typeof(string), curcate, res.form.GetString("String124"), getPageString("elecdt")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String125"), typeof(string), curcate, res.form.GetString("String126"), getPageString("roledata")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String127"), typeof(string), curcate, res.form.GetString("String128"), getPageString("rolesession")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String129"), typeof(string), curcate, res.form.GetString("String130"), getPageString("authrule")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String131"), typeof(string), curcate, res.form.GetString("String132"), getPageString("flowstat")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String133"), typeof(string), curcate, res.form.GetString("String134"), getPageString("norightinfo")));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String135"), typeof(string), curcate, res.form.GetString("String136"), getPageString("norighturl"), typeof(HomePageEditor), typeof(HomePageEditorConverter)));
            accessObj = new AccessObj();
            accessObj.setA_ip_s(getPageInt("a_ip_s"));
            accessObj.setA_ip_c(getPageString("a_ip_c"));
            accessObj.setA_ip_o(getPageInt("a_ip_o"));
            accessObj.setA_se_s(getPageInt("a_se_s"));
            accessObj.setA_se_c(getPageString("a_se_c"));
            accessObj.setA_se_o(getPageInt("a_se_o"));
            accessObj.setA_jp_s(getPageInt("a_jp_s"));
            accessObj.setA_jp_u(getPageString("a_jp_u"));
            accessObj.setA_tp_s(getPageInt("a_tp_s"));
            accessObj.setA_tp_c(getPageString("a_tp_c"));

            ps = new PropertySpec(PropertyPart.AccessIP, typeof(PropertyTable), PropertyPart.AccessMain, PropertyPart.AccessIP_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(PropertyPart.AccessSession, typeof(PropertyTable), PropertyPart.AccessMain, PropertyPart.AccessSession_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(PropertyPart.AccessJump, typeof(PropertyTable), PropertyPart.AccessMain, PropertyPart.AccessJump_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(PropertyPart.AccessTip, typeof(PropertyTable), PropertyPart.AccessMain, PropertyPart.AccessTip_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);

            return bag;
        }
        private static void page_GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (e.Property.Category.Equals(PropertyPart.AccessMain))
            {
                if (pName.Equals(PropertyPart.AccessIP))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { PropertyPart.AccessActiveNot, PropertyPart.AccessActive };
                    string[] ActiveNormal = new string[] { PropertyPart.AccessNormal, PropertyPart.AccessNormalNot };
                    //取值
                    string AccessIPControl_Value = (1 == accessObj.getA_ip_s() ? PropertyPart.AccessActive : PropertyPart.AccessActiveNot);
                    string AccessIPCondition_Value = accessObj.getA_ip_c();
                    string AccessIPConditionSide_Value = (1 == accessObj.getA_ip_o() ? PropertyPart.AccessNormalNot : PropertyPart.AccessNormal);
                    PropertyTable bagAccess = new PropertyTable();
                    //IP控制
                    AccessActiveDeactiveEnumEditorPage.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditorPage.v = AccessIPControl_Value;
                    AccessActiveDeactiveEnumEditorPage.Column = "a_ip_s";
                    psAccess = new PropertySpec(PropertyPart.AccessIPControl, typeof(string), "_AccessIPControl", PropertyPart.AccessIPControl_Des, AccessIPControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessIPControl] = AccessIPControl_Value;
                    //IP条件
                    AccessIPConditionEditorPageConverter.PageID = pageid;
                    AccessIPConditionEditorPageConverter.v = AccessIPCondition_Value;
                    psAccess = new PropertySpec(PropertyPart.AccessIPCondition, typeof(string), "_AccessIPCondition", PropertyPart.AccessIPCondition_Des, AccessIPCondition_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessIPConditionEditorPageConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessIPCondition] = AccessIPCondition_Value;
                    //IP条件置反
                    AccessActiveDeactiveEnumEditorPage2.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage2.Enums = ActiveNormal;
                    AccessActiveDeactiveEnumEditorPage2.v = AccessIPConditionSide_Value;
                    AccessActiveDeactiveEnumEditorPage2.Column = "a_ip_o";
                    psAccess = new PropertySpec(PropertyPart.AccessIPConditionSide, typeof(string), "_AccessIPConditionSide", PropertyPart.AccessIPConditionSide_Des, AccessIPConditionSide_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage2));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessIPConditionSide] = AccessIPConditionSide_Value;
                    e.Value = bagAccess;
                    return;
                }
                if (pName.Equals(PropertyPart.AccessSession))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { PropertyPart.AccessActiveNot, PropertyPart.AccessActive };
                    string[] ActiveNormal = new string[] { PropertyPart.AccessNormal, PropertyPart.AccessNormalNot };
                    string AccessSessionControl_Value = (1 == accessObj.getA_se_s() ? PropertyPart.AccessActive : PropertyPart.AccessActiveNot);
                    string AccessSessionCondition_Value = accessObj.getA_se_c();
                    string AccessSessionConditionSide_Value = (1 == accessObj.getA_se_o() ? PropertyPart.AccessNormalNot : PropertyPart.AccessNormal);
                    PropertyTable bagAccess = new PropertyTable();
                    //会话控制
                    AccessActiveDeactiveEnumEditorPage.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditorPage.v = AccessSessionControl_Value;
                    AccessActiveDeactiveEnumEditorPage.Column = "a_se_s";
                    psAccess = new PropertySpec(PropertyPart.AccessSessionControl, typeof(string), "_AccessSessionControl", PropertyPart.AccessSessionControl_Des, AccessSessionControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessSessionControl] = AccessSessionControl_Value;
                    //会话条件
                    AccessSessionConditionEditorPageConverter.PageID = pageid;
                    AccessSessionConditionEditorPageConverter.v = AccessSessionCondition_Value;
                    psAccess = new PropertySpec(PropertyPart.AccessSessionCondition, typeof(string), "_AccessSessionCondition", PropertyPart.AccessSessionCondition_Des, AccessSessionCondition_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessSessionConditionEditorPageConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessSessionCondition] = AccessSessionCondition_Value;
                    //会话条件置反
                    AccessActiveDeactiveEnumEditorPage2.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage2.Enums = ActiveNormal;
                    AccessActiveDeactiveEnumEditorPage2.v = AccessSessionConditionSide_Value;
                    AccessActiveDeactiveEnumEditorPage2.Column = "a_se_o";
                    psAccess = new PropertySpec(PropertyPart.AccessSessionConditionSide, typeof(string), "_AccessSessionConditionSide", PropertyPart.AccessSessionConditionSide_Des, AccessSessionConditionSide_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage2));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessSessionConditionSide] = AccessSessionConditionSide_Value;
                    e.Value = bagAccess;
                    return;
                }
                if (pName.Equals(PropertyPart.AccessJump))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { PropertyPart.AccessActiveNot, PropertyPart.AccessActive };
                    string[] ActiveNormal = new string[] { PropertyPart.AccessNormal, PropertyPart.AccessNormalNot };
                    string AccessJumpControl_Value = (1 == accessObj.getA_jp_s() ? PropertyPart.AccessActive : PropertyPart.AccessActiveNot);
                    string AccessJumpAddress_Value = accessObj.getA_jp_u();
                    PropertyTable bagAccess = new PropertyTable();
                    //跳转设置
                    AccessActiveDeactiveEnumEditorPage.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditorPage.v = AccessJumpControl_Value;
                    AccessActiveDeactiveEnumEditorPage.Column = "a_jp_s";
                    psAccess = new PropertySpec(PropertyPart.AccessJumpControl, typeof(string), "_AccessJumpControl", PropertyPart.AccessJumpControl_Des, AccessJumpControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessJumpControl] = AccessJumpControl_Value;
                    //跳转地址
                    AccessUrlEditorPageConverter.PageID = pageid;
                    AccessUrlEditorPageConverter.v = AccessJumpAddress_Value;
                    AccessUrlEditorPageConverter.Column = "a_jp_u";
                    psAccess = new PropertySpec(PropertyPart.AccessJumpAddress, typeof(string), "_AccessJumpAddress", PropertyPart.AccessJumpAddress_Des, AccessJumpAddress_Value, typeof(HomePageEditor), typeof(AccessUrlEditorPageConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessJumpAddress] = AccessJumpAddress_Value;

                    e.Value = bagAccess;
                    return;
                }
                if (pName.Equals(PropertyPart.AccessTip))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { PropertyPart.AccessActiveNot, PropertyPart.AccessActive };
                    string[] ActiveNormal = new string[] { PropertyPart.AccessNormal, PropertyPart.AccessNormalNot };
                    string AccessTipControl_Value = (1 == accessObj.getA_tp_s() ? PropertyPart.AccessActive : PropertyPart.AccessActiveNot);
                    string AccessTipContent_Value = accessObj.getA_tp_c();
                    PropertyTable bagAccess = new PropertyTable();
                    //提示设置
                    AccessActiveDeactiveEnumEditorPage.PageID = pageid;
                    AccessActiveDeactiveEnumEditorPage.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditorPage.v = AccessTipControl_Value;
                    AccessActiveDeactiveEnumEditorPage.Column = "a_tp_s";
                    psAccess = new PropertySpec(PropertyPart.AccessTipControl, typeof(string), "_AccessTipControl", PropertyPart.AccessTipControl_Des, AccessTipControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditorPage));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessTipControl] = AccessTipControl_Value;
                    //提示内容
                    AccessTipContentEditorPageConverter.PageID = pageid;
                    AccessTipContentEditorPageConverter.v = AccessTipContent_Value;
                    psAccess = new PropertySpec(PropertyPart.AccessTipContent, typeof(string), "_AccessTipContent", PropertyPart.AccessTipContent_Des, AccessTipContent_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessTipContentEditorPageConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[PropertyPart.AccessTipContent] = AccessTipContent_Value;

                    e.Value = bagAccess;
                    return;
                }
                return;
            }
            switch (pName)
            {
                case "ID":
                    e.Value = pageid;
                    break;
                case "Name":
                    e.Value = getPageString("name");
                    break;
                case "Path":
                    e.Value = globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode);
                    break;
                case "Caption":
                    e.Value = getPageString("caption");
                    break;
                case "UpdateTime":
                    e.Value = getPageString("updatetime");
                    break;
                default:
                    if (pName.Equals(res.form.GetString("String30")))
                    {
                        e.Value = res.form.GetString("String32");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String26")))
                    {
                        e.Value = getPageString("jinfo");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String77")))
                    {
                        e.Value = getPageString("fid");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String28")))
                    {
                        e.Value = getPageString("jurl");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String113")))
                    {
                        e.Value = getPageInt("viewopen") == 0 ? res.form.GetString("String115") : res.form.GetString("String116");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String111")))
                    {
                        e.Value = getPageInt("modopen") == 0 ? res.form.GetString("String115") : res.form.GetString("String116");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String117")))
                    {
                        e.Value = getPageString("datastr");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String119")))
                    {
                        e.Value = getPageString("paraname");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String121")))
                    {
                        e.Value = getPageString("membind");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String123")))
                    {
                        e.Value = getPageString("elecdt");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String125")))
                    {
                        e.Value = getPageString("roledata");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String127")))
                    {
                        e.Value = getPageString("rolesession");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String129")))
                    {
                        e.Value = getPageString("authrule");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String131")))
                    {
                        e.Value = getPageString("flowstat");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String133")))
                    {
                        e.Value = getPageString("norightinfo");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String135")))
                    {
                        e.Value = getPageString("norighturl");
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String23")))
                    {
                        e.Value = "";
                        return;
                    }
                    break;
            }
        }
        private static void page_SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            switch (pName)
            {
                case "Name":
                    pValue = pValue.Trim();
                    if (pValue.IndexOf("/") >= 0 || pValue.IndexOf("*") >= 0 || pValue.IndexOf("?") >= 0 || pValue.IndexOf("\\") >= 0)
                    {
                        MsgBox.Warning(res._propertysite.GetString("m4"));
                        return;
                    }
                    string path = globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode);
                    System.IO.FileInfo fi = new System.IO.FileInfo(path);
                    if (!fi.Exists)
                    {
                        MsgBox.Warning(path + res._propertysite.GetString("m5"));
                        return;
                    }
                    if (file.Exists(fi.DirectoryName + @"\" + pValue))
                    {
                        MsgBox.Warning(fi.DirectoryName + @"\" + pValue + res._propertysite.GetString("m6"));
                        return;
                    }
                    try
                    {
                        fi.MoveTo(fi.DirectoryName + @"\" + pValue);
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error(ex.Message);
                        return;
                    }
                    setPageString("name", pValue);
                    setPageString("updatetime", DateTime.Now.ToString());
                    ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[0] = pValue;
                    if (globalConst.siteTreeShowColName.Equals("name"))
                    {
                        globalConst.MdiForm.SiteTree.SelectedNode.Text = pValue;
                    }
                    //change opend editor'url property
                    Editor edr = form.getEditor(((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[2]);
                    if (edr != null && edr.thisUrl.Equals(path))
                    {
                        edr.thisUrl = fi.DirectoryName + @"\" + pValue;
                        edr.thisEditUrl = edr.thisUrl + "_edit.htm";
                        edr.thisViewUrl = edr.thisUrl + "_view.htm";
                    }
                    edr = null;
                    //update name in ctrltree
                    System.Windows.Forms.TreeNode td = tree.getCtrlNodeByID(((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[2]);
                    if (td != null)
                    {
                        ((string[])(td.Tag))[0] = pValue;
                        if (globalConst.ctrlTreeShowColName.Equals("name"))
                        {
                            td.Text = pValue;
                        }
                    }
                    break;
                case "Caption":
                    setPageString("caption", pValue);
                    ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[1] = pValue;
                    if (globalConst.siteTreeShowColName.Equals("caption"))
                    {
                        globalConst.MdiForm.SiteTree.SelectedNode.Text = pValue;
                    }
                    //just 4 page
                    string thisID = ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[2];
                    Editor ed = form.getEditor(thisID);
                    if (ed != null)
                    {
                        ed.Text = pValue;
                        form.UpdateFileOpend(thisID, true);
                    }
                    ed = null;
                    System.Windows.Forms.TreeNode tdr = tree.getCtrlNodeByID(((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Tag))[2]);
                    if (tdr != null)
                    {
                        ((string[])(tdr.Tag))[1] = pValue;
                        if (globalConst.ctrlTreeShowColName.Equals("caption"))
                        {
                            tdr.Text = pValue;
                        }
                    }
                    break;
                default:
                    if (pName.Equals(res.form.GetString("String26")))
                    {
                        setPageString("jinfo", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String77")))
                    {
                        setPageString("fid", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String28")))
                    {
                        setPageString("jurl", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String113")))
                    {
                        int setvalue = (pValue.Equals(res.form.GetString("String115")) ? 0 : 1);
                        setPageInt("viewopen", setvalue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String111")))
                    {
                        int setvalue = (pValue.Equals(res.form.GetString("String115")) ? 0 : 1);
                        setPageInt("modopen", setvalue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String117")))
                    {
                        setPageString("datastr", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String119")))
                    {
                        setPageString("paraname", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String121")))
                    {
                        setPageString("membind", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String123")))
                    {
                        setPageString("elecdt", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String125")))
                    {
                        setPageString("roledata", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String127")))
                    {
                        setPageString("rolesession", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String129")))
                    {
                        setPageString("authrule", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String131")))
                    {
                        setPageString("flowstat", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String133")))
                    {
                        setPageString("norightinfo", pValue);
                        return;
                    }
                    if (pName.Equals(res.form.GetString("String135")))
                    {
                        setPageString("norighturl", pValue);
                        return;
                    }
                    break;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class PropertyPart
    {
        private static XmlDocument curPartSysXmlDom;
        private static XmlDocument curPartSetXmlDom;
        private static string partid;
        private static AccessObj accessObj;
        private static string CtlName;
        private static string PartName;
        private static string ControlName;
        private static string ControlID;
        private static string ControlCaption;
        private static string ControlCompany;
        private static string ControlCopyright;
        private static string ControlCategory;
        private static string ControlVersion;
        private static string ControlDescription;
        private static string ControlCanPublish;
        private static string ControlCanCheck;
        private static string PartCaption;
        private static string DeployTime;
        private static XmlDocument ForePartXml;
        private static XmlDocument BackPartXml;
        public static string CategoryMain = res._pageware.GetString("c1");
        public static string CategoryInfo = res._pageware.GetString("c2");
        public static string CategoryControlInfo = res._pageware.GetString("c3");
        public static string CategoryPartInfo = res._pageware.GetString("c4");
        public static string CategoryForeInfo = res._pageware.GetString("c5");
        public static string CategoryBackInfo = res._pageware.GetString("c6");
        public static string CategoryControlData = res._pageware.GetString("c7");
        public static string CategoryControlDataShare = res._pageware.GetString("c8");
        public static string CategoryControlName = res._pageware.GetString("c9");
        public static string CategoryStyle = res._pageware.GetString("c13");
        public static string AccessMain = res._pageware.GetString("AccessMain");
        public static string AccessIP = res._pageware.GetString("AccessIP");
        public static string AccessIPControl = res._pageware.GetString("AccessIPControl");
        public static string AccessIPCondition = res._pageware.GetString("AccessIPCondition");
        public static string AccessIPConditionSide = res._pageware.GetString("AccessIPConditionSide");
        public static string AccessSession = res._pageware.GetString("AccessSession");
        public static string AccessSessionControl = res._pageware.GetString("AccessSessionControl");
        public static string AccessSessionCondition = res._pageware.GetString("AccessSessionCondition");
        public static string AccessSessionConditionSide = res._pageware.GetString("AccessSessionConditionSide");
        public static string AccessJump = res._pageware.GetString("AccessJump");
        public static string AccessJumpControl = res._pageware.GetString("AccessJumpControl");
        public static string AccessJumpAddress = res._pageware.GetString("AccessJumpAddress");
        public static string AccessTip = res._pageware.GetString("AccessTip");
        public static string AccessTipControl = res._pageware.GetString("AccessTipControl");
        public static string AccessTipContent = res._pageware.GetString("AccessTipContent");
        public static string AccessMain_Des = res._pageware.GetString("AccessMain_Des");
        public static string AccessIP_Des = res._pageware.GetString("AccessIP_Des");
        public static string AccessIPControl_Des = res._pageware.GetString("AccessIPControl_Des");
        public static string AccessIPCondition_Des = res._pageware.GetString("AccessIPCondition_Des");
        public static string AccessIPConditionSide_Des = res._pageware.GetString("AccessIPConditionSide_Des");
        public static string AccessSession_Des = res._pageware.GetString("AccessSession_Des");
        public static string AccessSessionControl_Des = res._pageware.GetString("AccessSessionControl_Des");
        public static string AccessSessionCondition_Des = res._pageware.GetString("AccessSessionCondition_Des");
        public static string AccessSessionConditionSide_Des = res._pageware.GetString("AccessSessionConditionSide_Des");
        public static string AccessJump_Des = res._pageware.GetString("AccessJump_Des");
        public static string AccessJumpControl_Des = res._pageware.GetString("AccessJumpControl_Des");
        public static string AccessJumpAddress_Des = res._pageware.GetString("AccessJumpAddress_Des");
        public static string AccessTip_Des = res._pageware.GetString("AccessTip_Des");
        public static string AccessTipControl_Des = res._pageware.GetString("AccessTipControl_Des");
        public static string AccessTipContent_Des = res._pageware.GetString("AccessTipContent_Des");
        public static string AccessNormal = res._pageware.GetString("AccessNormal");
        public static string AccessNormalNot = res._pageware.GetString("AccessNormalNot");
        public static string AccessActive = res._pageware.GetString("AccessActive");
        public static string AccessActiveNot = res._pageware.GetString("AccessActiveNot");
        public PropertyPart()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
        }
        public static void doPartProperty(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            partid = id;
            string sql = "select a.name as ctlname,b.partxml as partxml,b.name as partname ,a.id as ctlid,b.a_ip_s,b.a_ip_c,b.a_ip_o,b.a_se_s,b.a_se_c,b.a_se_o,b.a_jp_s,b.a_jp_u,b.a_tp_s,b.a_tp_c from controls a,parts b where a.id=b.controlid and b.id='" + partid + "'";
            string controlname = "";
            string partname = "";
            //add by maobb 20071031
            accessObj = new AccessObj();
            OleDbDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
            if (rdr.Read())
            {
                curPartSetXmlDom = new XmlDocument();
                curPartSetXmlDom.LoadXml(rdr.GetString(1));
                if (curPartSetXmlDom.OuterXml.Equals(""))
                {
                    log.Error("Part Set XmlDom is empty");
                    rdr.Close();
                    return;
                }
                controlname = rdr.GetString(0); 
                partname = rdr.GetString(2);
                ControlID = rdr.GetString(3);
                CtlName = controlname;
                PartName = partname;
                accessObj.setA_ip_s(int.Parse(rdr.GetValue(4).ToString()));
                accessObj.setA_ip_c(rdr.GetValue(5).ToString());
                accessObj.setA_ip_o(int.Parse(rdr.GetValue(6).ToString()));
                accessObj.setA_se_s(int.Parse(rdr.GetValue(7).ToString()));
                accessObj.setA_se_c(rdr.GetValue(8).ToString());
                accessObj.setA_se_o(int.Parse(rdr.GetValue(9).ToString()));
                accessObj.setA_jp_s(int.Parse(rdr.GetValue(10).ToString()));
                accessObj.setA_jp_u(rdr.GetValue(11).ToString());
                accessObj.setA_tp_s(int.Parse(rdr.GetValue(12).ToString()));
                accessObj.setA_tp_c(rdr.GetValue(13).ToString());
            }
            else
            {
                log.Error("partid is " + partid + "  part not exist!");
                rdr.Close();
                return;
            }
            rdr.Close();
            
            sql = "select partxml from parts where name='" + partname + "' and controlname='" + controlname + "'";
            OleDbDataReader rdr2 = globalConst.ConfigConn.OpenRecord(sql);
            if (rdr2.Read())
            {
                curPartSysXmlDom = new XmlDocument();
                curPartSysXmlDom.LoadXml(rdr2.GetString(0));
                if (curPartSysXmlDom.OuterXml.Equals(""))
                {
                    log.Error("Part Sys XmlDom is empty");
                    rdr2.Close();
                    return;
                }
            }
            else
            {
                log.Error("partname is " + partname + "  controlname is " + controlname + " part not exist!");
                rdr2.Close();
                return;
            }
            rdr2.Close();

            string sql1 = "select * from controls where name='" + CtlName + "'";
            OleDbDataReader rdr3 = globalConst.ConfigConn.OpenRecord(sql1);
            if (rdr3.Read())
            {
                ControlName = rdr3.GetString(0);
                ControlCaption = rdr3.GetString(1);
                ControlCompany = rdr3.GetString(2);
                ControlCopyright = rdr3.GetString(3);
                ControlCategory = rdr3.GetString(4);
                ControlVersion = rdr3.GetString(5);
                ControlDescription = rdr3.GetString(6);
                ControlCanPublish = rdr3.GetString(9);
                ControlCanCheck = rdr3.GetString(9);
                ForePartXml = new XmlDocument();
                ForePartXml.LoadXml(rdr3.GetString(10));
                BackPartXml = new XmlDocument();
                BackPartXml.LoadXml(rdr3.GetString(11));
                PartCaption = ForePartXml.SelectSingleNode("//fore_parts/part[@name='" + PartName + "']").InnerText;
                DeployTime = rdr3.GetString(12);
            }
            else
            {
                log.Error("Control name " + CtlName + " not found 1");
                rdr3.Close();
                return;
            }
            rdr3.Close();
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(bag_GetValue);
            bag.SetValue += new PropertySpecEventHandler(bag_SetValue);

            //other
            OleDbDataReader rdr4 = globalConst.CurSite.SiteConn.OpenRecord("select * from controls where id='" + ControlID + "'");
            if (rdr4.Read())
            {
                bag.Properties.Add(new PropertySpec(CategoryControlName, typeof(string), CategoryMain, res._pageware.GetString("b4"), rdr4.GetString(2)));
                DataSourceIDConverter.defaultDSID = rdr4.GetString(3).Replace("[1]", "[" + control.GetSharedCaptionByIndex("1") + "]").Replace("[2]", "[" + control.GetSharedCaptionByIndex("2") + "]");
                DataSourceIDConverter.currentCName = rdr4.GetString(1);
                bag.Properties.Add(new PropertySpec(CategoryControlData, typeof(string), CategoryMain, res._pageware.GetString("b5"), rdr4.GetString(3).Replace("[1]", "[" + control.GetSharedCaptionByIndex("1") + "]").Replace("[2]", "[" + control.GetSharedCaptionByIndex("2") + "]"), typeof(System.Drawing.Design.UITypeEditor), typeof(DataSourceIDConverter)));
                bag.Properties.Add(new PropertySpec(CategoryControlDataShare, typeof(string), CategoryMain, res._pageware.GetString("b6"), control.GetSharedCaptionByIndex(rdr4.GetString(4).ToString()), typeof(System.Drawing.Design.UITypeEditor), typeof(DataSourceShareConverter)));
            }
            rdr4.Close();
            //control part info
            PropertySpec ps;
            //			PropertySpec ps=new PropertySpec(CategoryControlName, typeof(string), CategoryInfo,"the Control Name",ControlName);
            //			ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
            //			bag.Properties.Add(ps);
            //			//bag.Properties.Add(new PropertySpec(CategoryControlName, typeof(string), CategoryInfo,"the Control Name")); 
            //			ps=new PropertySpec("Control Caption", typeof(string), CategoryInfo,"the Control Caption",ControlCaption);
            //			ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
            //			bag.Properties.Add(ps);
            //			ps=new PropertySpec("Control Company", typeof(string), CategoryInfo,"the Control Company",ControlCompany);
            //			ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
            //			bag.Properties.Add(ps);
            //			ps=new PropertySpec("Control Copyright", typeof(string), CategoryInfo,"the Control Copyright",ControlCopyright);
            //			ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
            //			bag.Properties.Add(ps);
            //			ps=new PropertySpec("Control Category", typeof(string), CategoryInfo,"the Control Category",ControlCategory);
            //			ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
            //			bag.Properties.Add(ps);
            //			ps=new PropertySpec("Control Version", typeof(string), CategoryInfo,"the Control Version",ControlVersion);
            //			ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
            //			bag.Properties.Add(ps);
            //			ps=new PropertySpec("Control Description", typeof(string), CategoryInfo,"the Control Description",ControlDescription);
            //			ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
            //			bag.Properties.Add(ps);
            //			ps=new PropertySpec("Control CanPublish", typeof(string), CategoryInfo,"the Control CanPublish",ControlCanPublish);
            //			ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
            //			bag.Properties.Add(ps);
            //			ps=new PropertySpec("Control CanCheck", typeof(string), CategoryInfo,"the Control CanCheck",ControlCanCheck);
            //			ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
            //			bag.Properties.Add(ps);
            //			ps=new PropertySpec("Part Name", typeof(string), CategoryInfo,"the Part Name",PartName);
            //			ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
            //			bag.Properties.Add(ps);
            //			ps=new PropertySpec("Part Caption", typeof(string), CategoryInfo,"the Part Caption",PartCaption);
            //			ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
            //			bag.Properties.Add(ps);
            //			ps=new PropertySpec("Part ID", typeof(string), CategoryInfo,"the Part ID",partid);
            //			ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
            //			bag.Properties.Add(ps);
            //			ps=new PropertySpec("Deploy Time", typeof(string), CategoryInfo,"the Deploy Time",DeployTime);
            //			ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
            //			bag.Properties.Add(ps);
            ps = new PropertySpec(CategoryControlInfo, typeof(ControlInfo.ControlInfo), CategoryInfo, res._pageware.GetString("b7"), null, typeof(System.Drawing.Design.UITypeEditor), typeof(ControlInfoConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            //ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
            ps = new PropertySpec(CategoryPartInfo, typeof(PartInfo), CategoryInfo, res._pageware.GetString("b8"), null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartInfoConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);

            ps = new PropertySpec(CategoryForeInfo, typeof(PropertyTable), CategoryInfo, res._pageware.GetString("b9"), null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(CategoryBackInfo, typeof(PropertyTable), CategoryInfo, res._pageware.GetString("b10"), null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            //访问权限控制
            //PartPropertyEnumConverter.Enums = new string[] { AccessActiveNot, AccessActive };
            //bag.Properties.Add(new PropertySpec(AccessIPControl, typeof(string), AccessMain, AccessIPControl_Des, AccessActiveNot, typeof(System.Drawing.Design.UITypeEditor), typeof(PartPropertyEnumConverter)));
            //bag.Properties.Add(new PropertySpec(AccessIPCondition, typeof(string), AccessMain, AccessIPCondition_Des, ""));
            ps = new PropertySpec(AccessIP, typeof(PropertyTable), AccessMain, AccessIP_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(AccessSession, typeof(PropertyTable), AccessMain, AccessSession_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(AccessJump, typeof(PropertyTable), AccessMain, AccessJump_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(AccessTip, typeof(PropertyTable), AccessMain, AccessTip_Des, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            //
            //			globalConst.MdiForm.PropGrid.SelectedObject=new Organization();
            //			return;
            //			bag.Properties.Add(new PropertySpec("test", typeof(Organization), CategoryInfo,"xxzxzx")); 
            XmlNodeList nds = curPartSysXmlDom.SelectNodes("//partxml/public_params/param");
            foreach (XmlNode nd in nds)
            {
                if (!nd.SelectSingleNode("class").InnerText.StartsWith("system"))
                {
                    string _name = nd.SelectSingleNode("name").InnerText;
                    string _caption = nd.SelectSingleNode("caption").InnerText;
                    string _description = nd.SelectSingleNode("description").InnerText;
                    string _class = nd.SelectSingleNode("class").InnerText;
                    //string _default=nd.SelectSingleNode("default").InnerText;
                    string _category = nd.SelectSingleNode("category").InnerText;
                    //set default value,flag bold if changed
                    XmlNodeList nds2 = curPartSetXmlDom.SelectNodes("//partxml/param");
                    string _value = null;
                    foreach (XmlNode nd2 in nds2)
                    {
                        if (nd2.SelectSingleNode("name").InnerText.Equals(_name))
                        {
                            _value = nd2.SelectSingleNode("value").InnerText;
                            goto ExitFor;
                        }
                    }
                ExitFor:
                    if (_value == null)
                    {
                        log.Error("Property " + _name + " not found in control.");
                        return;
                    } 
                    string CusEditorTag = ControlName + "-" + _name;
                    if (CusEditorTag.Equals("menu-RoleBindData") || CusEditorTag.Equals("list-RoleBindData") || CusEditorTag.Equals("list-Del_RoleBindData") || CusEditorTag.Equals("list-Flow_RoleBindData") || CusEditorTag.Equals("list-Copy_RoleBindData"))
                    {
                        GetControlData.ControlName = "role";
                        bag.Properties.Add(new PropertySpec(_caption, typeof(string), _category, _description, _value, typeof(DataSourceEditor), typeof(DataSourceEditorConverter)));
                    }
                    else if (CusEditorTag.Equals("dataop-FlowDesign"))
                    {
                        GetControlData.ControlName = "flow";
                        bag.Properties.Add(new PropertySpec(_caption, typeof(string), _category, _description, _value, typeof(DataSourceEditor), typeof(DataSourceEditorConverter)));
                    }
                    else if (CusEditorTag.StartsWith("list-MainTable") || CusEditorTag.StartsWith("dynum-TableName") || CusEditorTag.StartsWith("dataop-Tabletag"))
                    {
                        bag.Properties.Add(new PropertySpec(_caption, typeof(string), _category, _description, _value, typeof(TableTagEditor), typeof(TableTagEditorConverter)));
                    }
                    else if (globalConst.SqlEditorControlProp.Contains(CusEditorTag))
                    {
                        bag.Properties.Add(new PropertySpec(_caption, typeof(string), _category, _description, _value, typeof(SQLEditor), typeof(SQLEditorConverter)));
                    }
                    else if (CusEditorTag.StartsWith("dyvalue-SQLS_"))
                    {
                        bag.Properties.Add(new PropertySpec(_caption, typeof(string), _category, _description, _value, typeof(BackValueEditor), typeof(BackValueEditorConverter)));
                    }
                    else if (CusEditorTag.StartsWith("list-RowAll"))
                    {
                        bag.Properties.Add(new PropertySpec(_caption, typeof(string), _category, _description, _value, typeof(RowAllEditor), typeof(RowAllEditorConverter)));
                    }
                    else if (CusEditorTag.Equals("dataop-Define"))
                    {
                        DataOPDefineEditor.PartID = partid;
                        bag.Properties.Add(new PropertySpec(_caption, typeof(string), _category, _description, _value, typeof(DataOPDefineEditor), typeof(DataOPDefineEditorConverter)));
                    }
                    else if (CusEditorTag.Equals("dyvalue-Define"))
                    {
                        DyValueDefineEditor.PartID = partid;
                        bag.Properties.Add(new PropertySpec(_caption, typeof(string), _category, _description, _value, typeof(DyValueDefineEditor), typeof(DyValueDefineEditorConverter)));
                    }
                    else if (globalConst.TextEditorControlProp.Contains(CusEditorTag))
                    {
                        bag.Properties.Add(new PropertySpec(_caption, typeof(string), _category, _description, _value, typeof(classes.TextEditor), typeof(TextEditorConverter)));
                    }
                    else
                    {
                        switch (_class)
                        {
                            case "string":
                                bag.Properties.Add(new PropertySpec(_caption, typeof(string), _category, _description, _value));
                                break;
                            case "string|image":
                                bag.Properties.Add(new PropertySpec(_caption, typeof(string), _category, _description, _value, typeof(ImageEditor), typeof(ImageEditorConverter)));
                                break;
                            case "color":
                                bag.Properties.Add(new PropertySpec(_caption, typeof(Color), _category, _description, ColorTranslator.FromHtml(_value)));
                                break;
                            case "int":
                                bag.Properties.Add(new PropertySpec(_caption, typeof(int), _category, _description, int.Parse(_value)));
                                break;
                            case "enum":
                                XmlNodeList enumnds = nd.SelectNodes("enums/enum");
                                int _default = int.Parse(_value);
                                string _defaultvalue = "";
                                string[] enumsstring = new string[enumnds.Count];
                                int enumi;
                                for (enumi = 0; enumi < enumnds.Count; enumi++)
                                {
                                    enumsstring[enumi] = enumnds[enumi].InnerText;
                                    if (_default == enumi)
                                    {
                                        _defaultvalue = enumsstring[enumi];
                                    }
                                }
                                PartPropertyEnumConverter.Enums = enumsstring;
                                bag.Properties.Add(new PropertySpec(_caption, typeof(string), _category, _description, _defaultvalue, typeof(System.Drawing.Design.UITypeEditor), typeof(PartPropertyEnumConverter)));
                                break;
                            case "image":
                                bag.Properties.Add(new PropertySpec(_caption, typeof(string), _category, _description, _value, typeof(ImageEditor), typeof(ImageEditorConverter)));
                                break;
                            case "double":
                                bag.Properties.Add(new PropertySpec(_caption, typeof(double), _category, _description, double.Parse(_value)));
                                break;
                            case "page":
                                bag.Properties.Add(new PropertySpec(_caption, typeof(string), _category, _description, _value, typeof(HomePageEditor), typeof(HomePageEditorConverter)));
                                break;
                            default:
                                log.Error("Unknown Control Property! is " + _class);
                                break;
                        }
                    }
                }
            }
            //style
            nds = curPartSysXmlDom.SelectNodes("//partxml/styles/style");
            foreach (XmlNode nd in nds)
            {
                ps = new PropertySpec(nd.Attributes["caption"].Value, typeof(PropertyTable), CategoryStyle, nd.Attributes["description"].Value, null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
                ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                bag.Properties.Add(ps);
            }
            //			bag.Properties.Add(new PropertySpec("Picture", typeof(int), "Some Category",
            //				"This is a sample description."));
            //			bag.Properties.Add(new PropertySpec("Picture2", typeof(string), "",
            //				"This is a sample description."));
            if (globalConst.MdiForm.SiteTree.SelectedNode == null)
            {
                if (globalConst.MdiForm.ControlTree.SelectedNode.Nodes.Count == 0)
                {
                    SiteMatrix.Adapter.PartImage.thisURL = "";
                    SiteMatrix.Adapter.PartImage.thisName = "";
                    SiteMatrix.Adapter.PartImage.thisPartID = partid;
                }
                else
                {
                    TreeNode cnd = tree.getSiteNodeByID(tree.getID(globalConst.MdiForm.ControlTree.SelectedNode));
                    SiteMatrix.Adapter.PartImage.thisURL = globalConst.CurSite.Path + tree.getPath(cnd.Parent);
                    SiteMatrix.Adapter.PartImage.thisName = ((string[])(cnd.Parent.Tag))[0];
                    SiteMatrix.Adapter.PartImage.thisPartID = partid;
                }
            }
            else
            {
                SiteMatrix.Adapter.PartImage.thisURL = globalConst.CurSite.Path + tree.getPath(globalConst.MdiForm.SiteTree.SelectedNode.Parent);
                SiteMatrix.Adapter.PartImage.thisName = ((string[])(globalConst.MdiForm.SiteTree.SelectedNode.Parent.Tag))[0];
                SiteMatrix.Adapter.PartImage.thisPartID = partid;
            }
            globalConst.MdiForm.PropGrid.SelectedObject = bag;
        }
        private static void bag_GetValue(object sender, PropertySpecEventArgs e)
        {
            //Info
            string pName = e.Property.Name;
            if (e.Property.Category.Equals(CategoryInfo))
            {
                if (pName.Equals(CategoryPartInfo))
                {
                    PartInfo emp0 = new PartInfo();
                    emp0.Caption = PartCaption;
                    emp0.ID = partid;
                    emp0.Name = PartName;
                    e.Value = emp0;
                    return;
                }
                if (pName.Equals(CategoryControlInfo))
                {
                    ControlInfo.ControlInfo ci = new ControlInfo.ControlInfo();
                    ci.CanPublish = ControlCanPublish;
                    ci.CanCheck = ControlCanCheck;
                    ci.Caption = ControlCaption;
                    ci.Category = ControlCategory;
                    ci.Company = ControlCompany;
                    ci.Copyright = ControlCopyright;
                    ci.Description = ControlDescription;
                    ci.Name = ControlName;
                    ci.Version = ControlVersion;
                    ci.DeployTime = DeployTime;
                    ci.ControlID = ControlID;
                    e.Value = ci;
                    return;
                }
                if (pName.Equals(CategoryForeInfo))
                {
                    PropertyTable bagPart = new PropertyTable();
                    PropertySpec ps;
                    XmlNodeList tnds = ForePartXml.SelectNodes("//fore_parts/part");
                    foreach (XmlNode nd in tnds)
                    {
                        ps = new PropertySpec(nd.Attributes["name"].Value, typeof(string), "forpart", "A Fore Part ,Name is " + nd.Attributes["name"].Value + " and Caption is " + nd.InnerText, nd.InnerText);
                        ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                        bagPart.Properties.Add(ps);
                        bagPart[nd.Attributes["name"].Value] = nd.InnerText;
                    }
                    e.Value = bagPart;
                    return;
                }
                if (pName.Equals(CategoryBackInfo))
                {
                    PropertyTable bagPart2 = new PropertyTable();
                    PropertySpec ps2;
                    XmlNodeList tnds2 = BackPartXml.SelectNodes("//back_parts/part");
                    foreach (XmlNode nd in tnds2)
                    {
                        ps2 = new PropertySpec(nd.Attributes["name"].Value, typeof(string), "backpart", "A Back Part ,Name is " + nd.Attributes["name"].Value + " and Caption is " + nd.InnerText, nd.InnerText);
                        ps2.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                        bagPart2.Properties.Add(ps2);
                        bagPart2[nd.Attributes["name"].Value] = nd.InnerText;
                    }
                    e.Value = bagPart2;
                    return;
                }
                //					case CategoryControlName:
                //						e.Value=ControlName;
                //						return;
                //					case "Control Caption":
                //						e.Value=ControlCaption;
                //						return;
                //					case "Control Company":
                //						e.Value=ControlCompany;
                //						return;
                //					case "Control Copyright":
                //						e.Value=ControlCopyright;
                //						return;
                //					case "Control Category":
                //						e.Value=ControlCategory;
                //						return;
                //					case "Control Version":
                //						e.Value=ControlVersion;
                //						return;
                //					case "Control Description":
                //						e.Value=ControlDescription;
                //						return;
                //					case "Control CanPublish":
                //						e.Value=ControlCanPublish;
                //						return;
                //					case "Control CanCheck":
                //						e.Value=ControlCanCheck;
                //						return;
                //					case "Part Name":
                //						e.Value=PartName;
                //						return;
                //					case "Part Caption":
                //						e.Value=PartCaption;
                //						return;
                //					case "Part ID":
                //						e.Value=partid;
                //						return;
                //					case "Deploy Time":
                //						e.Value=DeployTime;
                //						return;


                return;
            }
            if (e.Property.Category.Equals(CategoryMain))
            {

                if (e.Property.Name.Equals(CategoryControlName))
                {
                    e.Value = globalConst.CurSite.SiteConn.GetString("select caption from controls where id='" + ControlID + "'");
                    return;
                }
                if (e.Property.Name.Equals(CategoryControlData))
                {
                    e.Value = globalConst.CurSite.SiteConn.GetString("select datasource from controls where id='" + ControlID + "'").Replace("[1]", "[" + control.GetSharedCaptionByIndex("1") + "]").Replace("[2]", "[" + control.GetSharedCaptionByIndex("2") + "]");
                    return;
                }
                if (e.Property.Name.Equals(CategoryControlDataShare))
                {
                    e.Value = control.GetSharedCaptionByIndex(globalConst.CurSite.SiteConn.GetString("select shared from controls where id='" + ControlID + "'"));
                    return;
                }
                return;
            }
            if (e.Property.Category.Equals(AccessMain))
            {
                if (pName.Equals(AccessIP))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { AccessActiveNot, AccessActive };
                    string[] ActiveNormal = new string[] { AccessNormal, AccessNormalNot };
                    //取值
                    string AccessIPControl_Value = (1==accessObj.getA_ip_s() ? AccessActive : AccessActiveNot);
                    string AccessIPCondition_Value = accessObj.getA_ip_c();
                    string AccessIPConditionSide_Value = (1==accessObj.getA_ip_o() ? AccessNormalNot : AccessNormal);
                    PropertyTable bagAccess = new PropertyTable();
                    //IP控制
                    AccessActiveDeactiveEnumEditor.PartID = partid;
                    AccessActiveDeactiveEnumEditor.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditor.v = AccessIPControl_Value;
                    AccessActiveDeactiveEnumEditor.Column = "a_ip_s";
                    psAccess = new PropertySpec(AccessIPControl, typeof(string), "_AccessIPControl", AccessIPControl_Des, AccessIPControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditor));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[AccessIPControl] = AccessIPControl_Value;
                    //IP条件
                    AccessIPConditionEditorConverter.PartID = partid;
                    AccessIPConditionEditorConverter.v = AccessIPCondition_Value;
                    psAccess = new PropertySpec(AccessIPCondition, typeof(string), "_AccessIPCondition", AccessIPCondition_Des, AccessIPCondition_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessIPConditionEditorConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[AccessIPCondition] = AccessIPCondition_Value;
                    //IP条件置反
                    AccessActiveDeactiveEnumEditor2.PartID = partid;
                    AccessActiveDeactiveEnumEditor2.Enums = ActiveNormal;
                    AccessActiveDeactiveEnumEditor2.v = AccessIPConditionSide_Value;
                    AccessActiveDeactiveEnumEditor2.Column = "a_ip_o";
                    psAccess = new PropertySpec(AccessIPConditionSide, typeof(string), "_AccessIPConditionSide", AccessIPConditionSide_Des, AccessIPConditionSide_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditor2));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[AccessIPConditionSide] = AccessIPConditionSide_Value;
                    e.Value = bagAccess;
                    return;
                }
                if (pName.Equals(AccessSession))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { AccessActiveNot, AccessActive };
                    string[] ActiveNormal = new string[] { AccessNormal, AccessNormalNot };
                    string AccessSessionControl_Value = (1==accessObj.getA_se_s() ? AccessActive : AccessActiveNot);
                    string AccessSessionCondition_Value = accessObj.getA_se_c();
                    string AccessSessionConditionSide_Value = (1==accessObj.getA_se_o() ? AccessNormalNot : AccessNormal);
                    PropertyTable bagAccess = new PropertyTable();
                    //会话控制
                    AccessActiveDeactiveEnumEditor.PartID = partid;
                    AccessActiveDeactiveEnumEditor.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditor.v = AccessSessionControl_Value;
                    AccessActiveDeactiveEnumEditor.Column = "a_se_s";
                    psAccess = new PropertySpec(AccessSessionControl, typeof(string), "_AccessSessionControl", AccessSessionControl_Des, AccessSessionControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditor));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[AccessSessionControl] = AccessSessionControl_Value;
                    //会话条件
                    AccessSessionConditionEditorConverter.PartID = partid;
                    AccessSessionConditionEditorConverter.v = AccessSessionCondition_Value;
                    psAccess = new PropertySpec(AccessSessionCondition, typeof(string), "_AccessSessionCondition", AccessSessionCondition_Des, AccessSessionCondition_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessSessionConditionEditorConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[AccessSessionCondition] = AccessSessionCondition_Value;
                    //会话条件置反
                    AccessActiveDeactiveEnumEditor2.PartID = partid;
                    AccessActiveDeactiveEnumEditor2.Enums = ActiveNormal;
                    AccessActiveDeactiveEnumEditor2.v = AccessSessionConditionSide_Value;
                    AccessActiveDeactiveEnumEditor2.Column = "a_se_o";
                    psAccess = new PropertySpec(AccessSessionConditionSide, typeof(string), "_AccessSessionConditionSide", AccessSessionConditionSide_Des, AccessSessionConditionSide_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditor2));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[AccessSessionConditionSide] = AccessSessionConditionSide_Value;
                    e.Value = bagAccess;
                    return;
                }
                if (pName.Equals(AccessJump))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { AccessActiveNot, AccessActive };
                    string[] ActiveNormal = new string[] { AccessNormal, AccessNormalNot };
                    string AccessJumpControl_Value = (1==accessObj.getA_jp_s() ? AccessActive : AccessActiveNot);
                    string AccessJumpAddress_Value = accessObj.getA_jp_u();
                    PropertyTable bagAccess = new PropertyTable();
                    //跳转设置
                    AccessActiveDeactiveEnumEditor.PartID = partid;
                    AccessActiveDeactiveEnumEditor.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditor.v = AccessJumpControl_Value;
                    AccessActiveDeactiveEnumEditor.Column = "a_jp_s";
                    psAccess = new PropertySpec(AccessJumpControl, typeof(string), "_AccessJumpControl", AccessJumpControl_Des, AccessJumpControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditor));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[AccessJumpControl] = AccessJumpControl_Value;
                    //跳转地址
                    AccessUrlEditorConverter.PartID = partid;
                    AccessUrlEditorConverter.v = AccessJumpAddress_Value;
                    AccessUrlEditorConverter.Column = "a_jp_u";
                    psAccess = new PropertySpec(AccessJumpAddress, typeof(string), "_AccessJumpAddress", AccessJumpAddress_Des, AccessJumpAddress_Value, typeof(HomePageEditor), typeof(AccessUrlEditorConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[AccessJumpAddress] = AccessJumpAddress_Value;
                    
                    e.Value = bagAccess;
                    return;
                }
                if (pName.Equals(AccessTip))
                {
                    PropertySpec psAccess;
                    string[] ActiveEnum = new string[] { AccessActiveNot, AccessActive };
                    string[] ActiveNormal = new string[] { AccessNormal, AccessNormalNot };
                    string AccessTipControl_Value = (1==accessObj.getA_tp_s() ? AccessActive : AccessActiveNot);
                    string AccessTipContent_Value = accessObj.getA_tp_c();
                    PropertyTable bagAccess = new PropertyTable();
                    //提示设置
                    AccessActiveDeactiveEnumEditor.PartID = partid;
                    AccessActiveDeactiveEnumEditor.Enums = ActiveEnum;
                    AccessActiveDeactiveEnumEditor.v = AccessTipControl_Value;
                    AccessActiveDeactiveEnumEditor.Column = "a_tp_s";
                    psAccess = new PropertySpec(AccessTipControl, typeof(string), "_AccessTipControl", AccessTipControl_Des, AccessTipControl_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessActiveDeactiveEnumEditor));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[AccessTipControl] = AccessTipControl_Value;
                    //提示内容
                    AccessTipContentEditorConverter.PartID = partid;
                    AccessTipContentEditorConverter.v = AccessTipContent_Value;
                    psAccess = new PropertySpec(AccessTipContent, typeof(string), "_AccessTipContent", AccessTipContent_Des, AccessTipContent_Value, typeof(System.Drawing.Design.UITypeEditor), typeof(AccessTipContentEditorConverter));
                    bagAccess.Properties.Add(psAccess);
                    bagAccess[AccessTipContent] = AccessTipContent_Value;

                    e.Value = bagAccess;
                    return;
                }
                return;
            }
            if (e.Property.Category.Equals(CategoryStyle))
            {
                //style 
                XmlNode nd2 = curPartSysXmlDom.SelectSingleNode("//partxml/styles/style[@caption='" + pName + "']");
                XmlNode nd = curPartSetXmlDom.SelectSingleNode("//partxml/styles/style[@name='" + nd2.Attributes["name"].Value + "']");
                if (nd == null)
                {
                    log.Error("Property Style" + pName + " not found in control.");
                    return;
                }
                PropertyTable bagPart3 = new PropertyTable();
                PropertySpec ps3;

                IHTMLElement ie = Loading.editocx.createElement("dscomstyle");
                //new MsgBox("111" + form.getEditor().editocx.body.outerHTML);
                ie.style.cssText = nd.Attributes["csstext"].Value;
                //new MsgBox("222" + form.getEditor().editocx.body.outerHTML);
                StyleEditor.ControlPartID = partid;
                StyleEditor.PartStyleName = nd2.Attributes["name"].Value;
                StyleEditor.PartElement = null;
                ps3 = new PropertySpec("style", typeof(string), "_style", nd2.Attributes["description"].Value + " style", ie, typeof(StyleEditor), typeof(StyleEditorConverter));
                //ps3.Attributes=new Attribute[]{EditorAttribute(typeof(StyleEditor),typeof(UITypeEditor)), 
                //								  TypeConverterAttribute(typeof(StyleEditorConverter))};
                bagPart3.Properties.Add(ps3);

                //new MsgBox(ie.style.cssText);
                bagPart3["style"] = ie;

                ClassEditorConverter.ControlPartID = partid;
                ClassEditorConverter.PartStyleName = nd2.Attributes["name"].Value;
                ClassEditorConverter.PartElement = null;
                ClassEditorConverter.v = nd.Attributes["class"].Value;
                ps3 = new PropertySpec("class", typeof(string), "_class", nd2.Attributes["description"].Value + " class", nd.Attributes["class"].Value, typeof(System.Drawing.Design.UITypeEditor), typeof(ClassEditorConverter));
                bagPart3.Properties.Add(ps3);
                bagPart3["class"] = nd.Attributes["class"].Value;
                e.Value = bagPart3;
                return;
            }
            //param


            XmlNodeList nds2 = curPartSysXmlDom.SelectNodes("//partxml/public_params/param");
            string _class = null;
            string _name = null;
            XmlNode curSysSetNode = null;
            XmlNode curSetSetNode = null;
            foreach (XmlNode nd in nds2)
            {
                if (nd.SelectSingleNode("caption") != null)
                {
                    if (nd.SelectSingleNode("caption").InnerText.Equals(pName))
                    {
                        _class = nd.SelectSingleNode("class").InnerText;
                        _name = nd.SelectSingleNode("name").InnerText;
                        curSysSetNode = nd;
                        goto ExitFor2;
                    }
                }
            }
        ExitFor2:
            XmlNodeList nds = curPartSetXmlDom.SelectNodes("//partxml/param");
            //new MsgBox(curPartSetXmlDom.OuterXml);
            string _value = null;
            foreach (XmlNode nd in nds)
            {
                if (nd.SelectSingleNode("name").InnerText.Equals(_name))
                {
                    _value = nd.SelectSingleNode("value").InnerText;
                    curSetSetNode = nd;
                    goto ExitFor;
                }
            }
        ExitFor:

            if (_value == null)
            {
                log.Error("Param " + _name + " not found in Control Part!");
                return;
            }
            switch (_class)
            {
                case "string":
                    e.Value = _value;
                    break;
                case "string|image":
                    e.Value = _value;
                    break;
                case "color":
                    e.Value = ColorTranslator.FromHtml(_value);
                    break;
                case "int":
                    e.Value = int.Parse(_value);
                    break;
                case "enum":
                    XmlNodeList enumnds = curSysSetNode.SelectNodes("enums/enum");
                    PageWare.ResetConverterEnums(enumnds);
                    int _default = int.Parse(_value);
                    string _defaultvalue = "";
                    int enumi;
                    for (enumi = 0; enumi < enumnds.Count; enumi++)
                    {
                        if (_default == enumi)
                        {
                            _defaultvalue = enumnds[enumi].InnerText;
                        }
                    }
                    e.Value = _defaultvalue;
                    break;
                case "image":
                    e.Value = _value;
                    break;
                case "double":
                    e.Value = double.Parse(_value);
                    break;
                case "page":
                    e.Value = _value;
                    break;
                default:
                    log.Error("Unknown Control Property! is " + _class);
                    break;
            }

        }
        private static void bag_SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            object pValue = e.Value;
            if (e.Property.Category.Equals(CategoryMain))
            {
                if (pName.Equals(CategoryControlName))
                {
                    globalConst.CurSite.SiteConn.execSql("update controls set caption='" + pValue.ToString().Replace("'", "''") + "'  where id='" + ControlID + "'");
                    if (globalConst.MdiForm.SiteTree.SelectedNode == null)
                    {
                        if (tree.getTypeFromID(tree.getID(globalConst.MdiForm.ControlTree.SelectedNode)).Equals("part"))
                        {
                            ((string[])(globalConst.MdiForm.ControlTree.SelectedNode.Parent.Tag))[0] = pValue.ToString();
                            ((string[])(globalConst.MdiForm.ControlTree.SelectedNode.Parent.Tag))[1] = pValue.ToString();
                            globalConst.MdiForm.ControlTree.SelectedNode.Parent.Text = pValue.ToString();
                            TreeNode nd = tree.getSiteNodeByID(tree.getID(globalConst.MdiForm.ControlTree.SelectedNode));
                            if (nd != null)
                            {
                                PageWare.refreshPartsInSiteTreeNoCtrl(tree.getID(nd.Parent));
                            }
                        }
                        return;
                    }
                    else
                    {
                        if (tree.getTypeFromID(tree.getID(globalConst.MdiForm.SiteTree.SelectedNode)).Equals("part"))
                        {
                            TreeNode nd = tree.getCtrlNodeByID(tree.getID(globalConst.MdiForm.SiteTree.SelectedNode));
                            PageWare.refreshPartsInSiteTreeNoCtrl(tree.getID(globalConst.MdiForm.SiteTree.SelectedNode.Parent));

                            if (nd != null)
                            {
                                ((string[])(nd.Parent.Tag))[0] = pValue.ToString();
                                ((string[])(nd.Parent.Tag))[1] = pValue.ToString();
                                nd.Parent.Text = pValue.ToString();
                            }
                        }
                        return;
                    }
                }
                if (pName.Equals(CategoryControlData))
                {
                    if (pValue.ToString().Equals("New DataSource"))
                    {
                        string DataSourceID = rdm.getDataSourceID();
                        string sql = "select count(id) from controls where datasource='" + DataSourceID + "'";
                        while (globalConst.CurSite.SiteConn.GetInt32(sql) > 0)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            DataSourceID = rdm.getDataSourceID();
                            sql = "select count(id) from controls where datasource='" + DataSourceID + "'";
                        }
                        pValue = DataSourceID;
                    }
                    pValue = pValue.ToString().Replace("[" + control.GetSharedCaptionByIndex("1") + "]", "[1]").Replace("[" + control.GetSharedCaptionByIndex("2") + "]", "[2]");
                    globalConst.CurSite.SiteConn.execSql("update controls set datasource='" + pValue + "'  where id='" + ControlID + "'");
                    return;
                }
                if (pName.Equals(CategoryControlDataShare))
                {
                    globalConst.CurSite.SiteConn.execSql("update controls set shared=" + control.GetSharedCaptionByIndex(pValue.ToString()) + "  where id='" + ControlID + "'");
                    return;
                }
                return;
            }
            XmlNodeList nds2 = curPartSysXmlDom.SelectNodes("//partxml/public_params/param");
            string _class = "";
            string _name = "";
            XmlNode curSysSetNode = null;
            foreach (XmlNode nd in nds2)
            {
                if (nd.SelectSingleNode("caption") != null)
                {
                    if (nd.SelectSingleNode("caption").InnerText.Equals(pName))
                    {
                        _class = nd.SelectSingleNode("class").InnerText;
                        _name = nd.SelectSingleNode("name").InnerText;
                        curSysSetNode = nd;
                        goto ExitFor2;
                    }
                }
            }
        ExitFor2:

            XmlNodeList nds = curPartSetXmlDom.SelectNodes("//partxml/param");
            XmlNode setNode = null;
            foreach (XmlNode nd in nds)
            {
                if (nd.SelectSingleNode("name").InnerText.Equals(_name))
                {
                    setNode = nd;
                    goto ExitFor;
                }
            }
        ExitFor:
            if (setNode == null)
            {
                log.Error("Property " + pName + " not found in control.");
                return;
            }

            switch (_class)
            {
                case "string":
                    setNode.SelectSingleNode("value").InnerText = pValue.ToString();

                    break;
                case "string|image":
                    setNode.SelectSingleNode("value").InnerText = pValue.ToString();
                    break;
                case "color":
                    setNode.SelectSingleNode("value").InnerText = ColorTranslator.ToHtml((Color)pValue);

                    break;
                case "int":
                    setNode.SelectSingleNode("value").InnerText = pValue.ToString();

                    break;
                case "enum":
                    XmlNodeList enumnds = curSysSetNode.SelectNodes("enums/enum");
                    PageWare.ResetConverterEnums(enumnds);
                    int enumi;
                    for (enumi = 0; enumi < enumnds.Count; enumi++)
                    {
                        if (enumnds[enumi].InnerText == pValue.ToString())
                        {
                            goto ExitFor4Enum;
                        }
                    }
                ExitFor4Enum:
                    pValue = enumi;
                    setNode.SelectSingleNode("value").InnerText = pValue.ToString();

                    break;
                case "image":
                    setNode.SelectSingleNode("value").InnerText = pValue.ToString();
                    break;
                case "double":
                    setNode.SelectSingleNode("value").InnerText = pValue.ToString();

                    break;
                case "page":
                    setNode.SelectSingleNode("value").InnerText = pValue.ToString();
                    break;
                default:
                    log.Error("Unknown Control Property! is " + _class);
                    break;
            }
            /*	try
                {
                    Ele.innerHTML=getPartHtml("ctlid",CtlName,PartName,curPartSetXmlDom.OuterXml);
                }
                catch(Exception ex)
                {
                    if(ex.Message.StartsWith("HRESULT"))
                    {
                        //Ele.parentElement.tagName="div";
                        //Ele.innerHTML=getPartHtml("ctlid",CtlName,PartName,curPartSetXmlDom.OuterXml);
                        //if parent element is <p> <td>...,span will accur error
                        string Eleid=Ele.getAttribute("idname",0).ToString();
                        string Eleheight=Ele.style.height.ToString();
                        string Elewidth=Ele.style.width.ToString();
                        string Elename=Ele.getAttribute("partname",0).ToString();
                        int ElesourceIndex=Ele.sourceIndex;
                        Ele.innerHTML="";
                        Ele.style.cssText="";
                        IHTMLElement itp=Ele.parentElement;
                        itp.outerHTML=itp.outerHTML.Replace(Ele.outerHTML,globalConst.PageWare.getControlEditHead(Eleid,Elename,Elewidth,Eleheight) + getPartHtml("ctlid",CtlName,PartName,curPartSetXmlDom.OuterXml)+ globalConst.PageWare.getControlEditTail());
                        Ele=(IHTMLElement)(((IHTMLDocument2)form.getEditor().editocx.DOM).all.item(ElesourceIndex,ElesourceIndex));
                        if(!PageWare.isPartElement(Ele))MsgBox.Warning("Part Element Lost!");
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
				
                }*/
            System.Windows.Forms.Application.DoEvents();
            if (SiteClass.Site.JustEdit(partid))globalConst.CurSite.SiteConn.execSql("update parts set partxml='" + curPartSetXmlDom.OuterXml.Replace("'", "''") + "'where id='" + partid + "'");

        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class PropertyControl
    {

        private static string CtlName;
        private static string ControlName;
        private static string ControlID;
        private static string ControlCaption;
        private static string ControlCompany;
        private static string ControlCopyright;
        private static string ControlCategory;
        private static string ControlVersion;
        private static string ControlDescription;
        private static string ControlCanPublish;
        private static string ControlCanCheck;
        private static string DeployTime;
        private static string ControlPara;
        private static XmlDocument ForePartXml;
        private static XmlDocument BackPartXml;
        public static string CategoryMain = res._pageware.GetString("c1");
        public static string CategoryInfo = res._pageware.GetString("c2");
        public static string CategoryControlInfo = res._pageware.GetString("c3");
        public static string CategoryForeInfo = res._pageware.GetString("c5");
        public static string CategoryBackInfo = res._pageware.GetString("c6");
        public static string CategoryControlData = res._pageware.GetString("c7");
        public static string CategoryControlDataShare = res._pageware.GetString("c8");
        public static string CategoryControlName = res._pageware.GetString("c9");
        public static string CategoryControlPara = res._pageware.GetString("ctlpara");
        public PropertyControl()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
        }
        public static void doControlProperty(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            ControlID = id;
            string sql = "select name from controls where id='" + ControlID + "'";
            string controlname = "";
            OleDbDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
            if (rdr.Read())
            {
                controlname = rdr.GetString(0);
                CtlName = controlname;
            }
            else
            {
                log.Error("ControlID is " + ControlID + "  control not exist!");
                rdr.Close();
                return;
            }
            rdr.Close();


            string sql1 = "select * from controls where name='" + CtlName + "'";
            OleDbDataReader rdr3 = globalConst.ConfigConn.OpenRecord(sql1);
            if (rdr3.Read())
            {
                ControlName = rdr3.GetString(0);
                ControlCaption = rdr3.GetString(1);
                ControlCompany = rdr3.GetString(2);
                ControlCopyright = rdr3.GetString(3);
                ControlCategory = rdr3.GetString(4);
                ControlVersion = rdr3.GetString(5);
                ControlDescription = rdr3.GetString(6);
                ControlCanPublish = rdr3.GetString(9);
                ControlCanCheck = rdr3.GetString(9);
                ForePartXml = new XmlDocument();
                ForePartXml.LoadXml(rdr3.GetString(10));
                BackPartXml = new XmlDocument();
                BackPartXml.LoadXml(rdr3.GetString(11));
                DeployTime = rdr3.GetString(12);
            }
            else
            {
                log.Error("Control name " + CtlName + " not found 1");
                rdr3.Close();
                return;
            }
            rdr3.Close();
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(bag_GetValue);
            bag.SetValue += new PropertySpecEventHandler(bag_SetValue);

            //other
            OleDbDataReader rdr4 = globalConst.CurSite.SiteConn.OpenRecord("select * from controls where id='" + ControlID + "'");
            if (rdr4.Read())
            {
                bag.Properties.Add(new PropertySpec(CategoryControlName, typeof(string), CategoryMain, res._pageware.GetString("b4"), rdr4.GetString(2)));
                DataSourceIDConverter.defaultDSID = rdr4.GetString(3).Replace("[1]", "[" + control.GetSharedCaptionByIndex("1") + "]").Replace("[2]", "[" + control.GetSharedCaptionByIndex("2") + "]");
                DataSourceIDConverter.currentCName = rdr4.GetString(1);
                bag.Properties.Add(new PropertySpec(CategoryControlData, typeof(string), CategoryMain, res._pageware.GetString("b5"), rdr4.GetString(3).Replace("[1]", "[" + control.GetSharedCaptionByIndex("1") + "]").Replace("[2]", "[" + control.GetSharedCaptionByIndex("2") + "]"), typeof(System.Drawing.Design.UITypeEditor), typeof(DataSourceIDConverter)));
                bag.Properties.Add(new PropertySpec(CategoryControlDataShare, typeof(string), CategoryMain, res._pageware.GetString("b6"), control.GetSharedCaptionByIndex(rdr4.GetString(4).ToString()), typeof(System.Drawing.Design.UITypeEditor), typeof(DataSourceShareConverter)));

                //control paras
                //bag.Properties.Add(new PropertySpec("参数1", typeof(string), res._pageware.GetString("ctlpara"), "参数1desc", "参数1默认值"));
                string Control_Para = null;
                if (rdr4.IsDBNull(5)) Control_Para = "";
                else Control_Para = rdr4.GetString(5);
                if (Control_Para.Trim().Equals("")) Control_Para = "<paras></paras>";
                ControlPara = Control_Para;
                XmlDocument ctlparaxmldoc = new XmlDocument();
                ctlparaxmldoc.LoadXml(Control_Para);
                XmlNodeList ctlparas = ctlparaxmldoc.SelectNodes("//paras/para");
                int parai=0;
                foreach (XmlNode ctlpara in ctlparas)
                {
                    if (controlname.Equals("role") && parai==0)
                    {
                        GetControlData.ControlName = "member";
                        bag.Properties.Add(new PropertySpec(ctlpara.Attributes.GetNamedItem("caption").Value, typeof(string), CategoryControlPara, ctlpara.Attributes.GetNamedItem("desc").Value, ctlpara.Attributes.GetNamedItem("value").Value, typeof(DataSourceEditor), typeof(DataSourceEditorConverter)));
                    }
                    else if (controlname.Equals("role") && (parai == 1 || parai == 2))
                    {
                        bag.Properties.Add(new PropertySpec(ctlpara.Attributes.GetNamedItem("caption").Value, typeof(string), CategoryControlPara, ctlpara.Attributes.GetNamedItem("desc").Value, ctlpara.Attributes.GetNamedItem("value").Value, typeof(SQLEditor), typeof(SQLEditorConverter)));
                    }
                    else if (controlname.Equals("role") && parai==3)
                    {
                        bag.Properties.Add(new PropertySpec(ctlpara.Attributes.GetNamedItem("caption").Value, typeof(string), CategoryControlPara, ctlpara.Attributes.GetNamedItem("desc").Value, ctlpara.Attributes.GetNamedItem("value").Value, typeof(TableTagEditor), typeof(TableTagEditorConverter)));
                    }
                    else if (controlname.Equals("cuscol") && parai == 0)
                    {
                        bag.Properties.Add(new PropertySpec(ctlpara.Attributes.GetNamedItem("caption").Value, typeof(string), CategoryControlPara, ctlpara.Attributes.GetNamedItem("desc").Value, ctlpara.Attributes.GetNamedItem("value").Value, typeof(TableTagEditor), typeof(TableTagEditorConverter)));
                    }
                    else
                    {
                        bag.Properties.Add(new PropertySpec(ctlpara.Attributes.GetNamedItem("caption").Value, typeof(string), CategoryControlPara, ctlpara.Attributes.GetNamedItem("desc").Value, ctlpara.Attributes.GetNamedItem("value").Value));
                    }
                    parai++;
                }
                ctlparas = null;
                ctlparaxmldoc = null;
            }
            rdr4.Close();

            

            //control part info
            PropertySpec ps;

            ps = new PropertySpec(CategoryControlInfo, typeof(ControlInfo.ControlInfo), CategoryInfo, res._pageware.GetString("b7"), null, typeof(System.Drawing.Design.UITypeEditor), typeof(ControlInfoConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            //ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};


            ps = new PropertySpec(CategoryForeInfo, typeof(PropertyTable), CategoryInfo, res._pageware.GetString("b9"), null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(CategoryBackInfo, typeof(PropertyTable), CategoryInfo, res._pageware.GetString("b10"), null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            //			globalConst.MdiForm.PropGrid.SelectedObject=new Organization();
            //			return;
            //			bag.Properties.Add(new PropertySpec("test", typeof(Organization), CategoryInfo,"xxzxzx")); 

            globalConst.MdiForm.PropGrid.SelectedObject = bag;
        }
        private static void bag_GetValue(object sender, PropertySpecEventArgs e)
        {
            //Info
            string pName = e.Property.Name;
            if (e.Property.Category.Equals(CategoryInfo))
            {

                if (pName.Equals(CategoryControlInfo))
                {
                    ControlInfo.ControlInfo ci = new ControlInfo.ControlInfo();
                    ci.CanPublish = ControlCanPublish;
                    ci.CanCheck = ControlCanCheck;
                    ci.Caption = ControlCaption;
                    ci.Category = ControlCategory;
                    ci.Company = ControlCompany;
                    ci.Copyright = ControlCopyright;
                    ci.Description = ControlDescription;
                    ci.Name = ControlName;
                    ci.Version = ControlVersion;
                    ci.DeployTime = DeployTime;
                    ci.ControlID = ControlID;
                    e.Value = ci;
                    return;
                }
                if (pName.Equals(CategoryForeInfo))
                {
                    PropertyTable bagPart = new PropertyTable();
                    PropertySpec ps;
                    XmlNodeList tnds = ForePartXml.SelectNodes("//fore_parts/part");
                    foreach (XmlNode nd in tnds)
                    {
                        ps = new PropertySpec(nd.Attributes["name"].Value, typeof(string), "forpart", "A Fore Part ,Name is " + nd.Attributes["name"].Value + " and Caption is " + nd.InnerText, nd.InnerText);
                        ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                        bagPart.Properties.Add(ps);
                        bagPart[nd.Attributes["name"].Value] = nd.InnerText;
                    }
                    e.Value = bagPart;
                    return;
                }
                if (pName.Equals(CategoryBackInfo))
                {
                    PropertyTable bagPart2 = new PropertyTable();
                    PropertySpec ps2;
                    XmlNodeList tnds2 = BackPartXml.SelectNodes("//back_parts/part");
                    foreach (XmlNode nd in tnds2)
                    {
                        ps2 = new PropertySpec(nd.Attributes["name"].Value, typeof(string), "backpart", "A Back Part ,Name is " + nd.Attributes["name"].Value + " and Caption is " + nd.InnerText, nd.InnerText);
                        ps2.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                        bagPart2.Properties.Add(ps2);
                        bagPart2[nd.Attributes["name"].Value] = nd.InnerText;
                    }
                    e.Value = bagPart2;
                    return;
                }

                return;
            }
            if (e.Property.Category.Equals(CategoryMain))
            {

                if (e.Property.Name.Equals(CategoryControlName))
                {
                    e.Value = globalConst.CurSite.SiteConn.GetString("select caption from controls where id='" + ControlID + "'");
                    return;
                }
                if (e.Property.Name.Equals(CategoryControlData))
                {
                    e.Value = globalConst.CurSite.SiteConn.GetString("select datasource from controls where id='" + ControlID + "'").Replace("[1]", "[" + control.GetSharedCaptionByIndex("1") + "]").Replace("[2]", "[" + control.GetSharedCaptionByIndex("2") + "]");
                    return;
                }
                if (e.Property.Name.Equals(CategoryControlDataShare))
                {
                    e.Value = control.GetSharedCaptionByIndex(globalConst.CurSite.SiteConn.GetString("select shared from controls where id='" + ControlID + "'"));
                    return;
                }

                return;
            }
            if (e.Property.Category.Equals(CategoryControlPara))
            {
                XmlDocument ctlparaxmldoc = new XmlDocument();
                ctlparaxmldoc.LoadXml(ControlPara);
                e.Value=ctlparaxmldoc.SelectSingleNode("//paras/para[@caption='" + e.Property.Name + "']").Attributes.GetNamedItem("value").Value ;
                ctlparaxmldoc = null;
                return;
            }

        }
        private static void bag_SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            object pValue = e.Value;
            if (e.Property.Category.Equals(CategoryMain))
            {
                if (pName.Equals(CategoryControlName))
                {
                    globalConst.CurSite.SiteConn.execSql("update controls set caption='" + pValue.ToString().Replace("'", "''") + "'  where id='" + ControlID + "'");
                    ((string[])(globalConst.MdiForm.ControlTree.SelectedNode.Tag))[0] = pValue.ToString();
                    ((string[])(globalConst.MdiForm.ControlTree.SelectedNode.Tag))[1] = pValue.ToString();
                    globalConst.MdiForm.ControlTree.SelectedNode.Text = pValue.ToString();
                    foreach (TreeNode cpnd in globalConst.MdiForm.ControlTree.SelectedNode.Nodes)
                    {
                        foreach (TreeNode cpnd2 in cpnd.Nodes)
                        {
                            if (tree.getTypeFromID(tree.getID(cpnd2)).Equals("page"))
                            {
                                PageWare.refreshPartsInSiteTreeNoCtrl(tree.getID(cpnd2));
                            }
                        }
                    }

                }
                if (pName.Equals(CategoryControlData))
                {
                    if (pValue.ToString().Equals("New DataSource"))
                    {
                        string DataSourceID = rdm.getDataSourceID();
                        string sql = "select count(id) from controls where datasource='" + DataSourceID + "'";
                        while (globalConst.CurSite.SiteConn.GetInt32(sql) > 0)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            DataSourceID = rdm.getDataSourceID();
                            sql = "select count(id) from controls where datasource='" + DataSourceID + "'";
                        }
                        pValue = DataSourceID;
                    }
                    pValue = pValue.ToString().Replace("[" + control.GetSharedCaptionByIndex("1") + "]", "[1]").Replace("[" + control.GetSharedCaptionByIndex("2") + "]", "[2]");
                    globalConst.CurSite.SiteConn.execSql("update controls set datasource='" + pValue + "'  where id='" + ControlID + "'");
                    return;
                }
                if (pName.Equals(CategoryControlDataShare))
                {
                    globalConst.CurSite.SiteConn.execSql("update controls set shared=" + control.GetSharedCaptionByIndex(pValue.ToString()) + "  where id='" + ControlID + "'");
                    return;
                }
                return;
            }
            if (e.Property.Category.Equals(CategoryControlPara))
            {
                XmlDocument ctlparaxmldoc = new XmlDocument();
                ctlparaxmldoc.LoadXml(ControlPara);
                ctlparaxmldoc.SelectSingleNode("//paras/para[@caption='" + pName + "']").Attributes.GetNamedItem("value").Value = pValue.ToString();
                globalConst.CurSite.SiteConn.execSql("update controls set paras='" + ctlparaxmldoc.OuterXml.Replace("'","''") + "'  where id='" + ControlID + "'");
                ControlPara = ctlparaxmldoc.OuterXml; 
                ctlparaxmldoc = null;
                return;
            }
        }
    }

    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class PropertyControlMother
    {

        private static string CtlName;
        private static string ControlName;
        private static string ControlCaption;
        private static string ControlCompany;
        private static string ControlCopyright;
        private static string ControlCategory;
        private static string ControlVersion;
        private static string ControlDescription;
        private static string ControlCanPublish;
        private static string ControlCanCheck;
        private static string DeployTime;
        private static XmlDocument ForePartXml;
        private static XmlDocument BackPartXml;
        public static string CategoryInfo =  res._pageware.GetString("c2");
        public static string CategoryControlInfo =  res._pageware.GetString("c3");
        public static string CategoryForeInfo =  res._pageware.GetString("c5");
        public static string CategoryBackInfo =  res._pageware.GetString("c6");
        public PropertyControlMother()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
        }
        public static void doControlProperty(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            if (!id.EndsWith("_comp")) return;
            CtlName = id.Substring(0, id.Length - 5);
            string sql1 = "select * from controls where name='" + CtlName + "'";
            OleDbDataReader rdr3 = globalConst.ConfigConn.OpenRecord(sql1);
            if (rdr3.Read())
            {
                ControlName = rdr3.GetString(0);
                ControlCaption = rdr3.GetString(1);
                ControlCompany = rdr3.GetString(2);
                ControlCopyright = rdr3.GetString(3);
                ControlCategory = rdr3.GetString(4);
                ControlVersion = rdr3.GetString(5);
                ControlDescription = rdr3.GetString(6);
                ControlCanPublish = rdr3.GetString(9);
                ControlCanCheck = rdr3.GetString(9);
                ForePartXml = new XmlDocument();
                ForePartXml.LoadXml(rdr3.GetString(10));
                BackPartXml = new XmlDocument();
                BackPartXml.LoadXml(rdr3.GetString(11));
                DeployTime = rdr3.GetString(12);
            }
            else
            {
                log.Error("Control name " + CtlName + " not found 1");
                rdr3.Close();
                return;
            }
            rdr3.Close();
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(bag_GetValue);
            //bag.SetValue += new PropertySpecEventHandler(bag_SetValue);

            //control part info
            PropertySpec ps;

            ps = new PropertySpec(CategoryControlInfo, typeof(ControlInfo.ControlInfo), CategoryInfo,  res._pageware.GetString("b7"), null, typeof(System.Drawing.Design.UITypeEditor), typeof(ControlInfoConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            //ps.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};


            ps = new PropertySpec(CategoryForeInfo, typeof(PropertyTable), CategoryInfo, res._pageware.GetString("b9"), null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            ps = new PropertySpec(CategoryBackInfo, typeof(PropertyTable), CategoryInfo,  res._pageware.GetString("b10"), null, typeof(System.Drawing.Design.UITypeEditor), typeof(PartsConverter));
            ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            bag.Properties.Add(ps);
            //			globalConst.MdiForm.PropGrid.SelectedObject=new Organization();
            //			return;
            //			bag.Properties.Add(new PropertySpec("test", typeof(Organization), CategoryInfo,"xxzxzx")); 

            globalConst.MdiForm.PropGrid.SelectedObject = bag;
        }
        private static void bag_GetValue(object sender, PropertySpecEventArgs e)
        {
            //Info
            string pName = e.Property.Name;
            if (e.Property.Category.Equals(CategoryInfo))
            {

                if (pName.Equals(CategoryControlInfo))
                {
                    ControlInfo.ControlInfo ci = new ControlInfo.ControlInfo();
                    ci.CanPublish = ControlCanPublish;
                    ci.CanCheck = ControlCanCheck;
                    ci.Caption = ControlCaption;
                    ci.Category = ControlCategory;
                    ci.Company = ControlCompany;
                    ci.Copyright = ControlCopyright;
                    ci.Description = ControlDescription;
                    ci.Name = ControlName;
                    ci.Version = ControlVersion;
                    ci.DeployTime = DeployTime;
                    ci.ControlID = "(Invalid)";
                    e.Value = ci;
                    return;
                }
                if (pName.Equals(CategoryForeInfo))
                {
                    PropertyTable bagPart = new PropertyTable();
                    PropertySpec ps;
                    XmlNodeList tnds = ForePartXml.SelectNodes("//fore_parts/part");
                    foreach (XmlNode nd in tnds)
                    {
                        ps = new PropertySpec(nd.Attributes["name"].Value, typeof(string), "forpart", "A Fore Part ,Name is " + nd.Attributes["name"].Value + " and Caption is " + nd.InnerText, nd.InnerText);
                        ps.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                        bagPart.Properties.Add(ps);
                        bagPart[nd.Attributes["name"].Value] = nd.InnerText;
                    }
                    e.Value = bagPart;
                    return;
                }
                if (pName.Equals(CategoryBackInfo))
                {
                    PropertyTable bagPart2 = new PropertyTable();
                    PropertySpec ps2;
                    XmlNodeList tnds2 = BackPartXml.SelectNodes("//back_parts/part");
                    foreach (XmlNode nd in tnds2)
                    {
                        ps2 = new PropertySpec(nd.Attributes["name"].Value, typeof(string), "backpart", "A Back Part ,Name is " + nd.Attributes["name"].Value + " and Caption is " + nd.InnerText, nd.InnerText);
                        ps2.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                        bagPart2.Properties.Add(ps2);
                        bagPart2[nd.Attributes["name"].Value] = nd.InnerText;
                    }
                    e.Value = bagPart2;
                    return;
                }

                return;
            }
        }

    }

}