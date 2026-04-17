using System;
using SiteMatrix.functions;
using SiteMatrix.consts;
using SiteMatrix.database;
using System.Data.OleDb;
using System.Xml;
using SiteMatrix.forms;
using System.Reflection;
using SiteMatrix.Adapter;
using SiteMatrix.PropertyBagNameSpace;
using mshtml;
using System.Collections;
using System.ComponentModel;
using SiteMatrix.PropertySpace.ControlInfo;
using SiteMatrix.classes;
using System.Drawing;
using System.Windows.Forms;
using SiteMatrix.controls;
namespace SiteMatrix.Page
{
    /// <summary>
    /// PageWare 的摘要说明。
    /// </summary>
    /// 
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class PageWare
    {
        public PageWare()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
        }
        /*
         * Add 
         * 
         * 
         * 
         * 
        */
        private static XmlDocument curPartSysXmlDom;
        private static XmlDocument curPartSetXmlDom;
        private static AccessObj accessObj;
        private static string CtlName;
        private static string PartName;
        private static string partid;
        private static string ControlName;
        private static string ControlID;
        private static int AsPortal;
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
        private static IHTMLElement Ele;
        public static string CategoryMain = res._pageware.GetString("c1");
        public static string CategoryInfo = res._pageware.GetString("c2");
        public static string CategoryControlInfo = res._pageware.GetString("c3");
        public static string CategoryPartInfo = res._pageware.GetString("c4");
        public static string CategoryForeInfo = res._pageware.GetString("c5");
        public static string CategoryBackInfo = res._pageware.GetString("c6");
        public static string CategoryControlData = res._pageware.GetString("c7");
        public static string CategoryControlDataShare = res._pageware.GetString("c8");
        public static string CategoryControlName = res._pageware.GetString("c9");
        public static string CategoryPartHeight = res._pageware.GetString("c10");
        public static string CategoryPartWidth = res._pageware.GetString("c11");
        public static string CategoryPartPortal = res._pageware.GetString("c12");
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
        public static string CloneControl(string ControlCaption, string ControlID)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            DB db3 = new DB();
            db3.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db");
            try
            {
                string sql = "select name from controls where id='" + ControlID + "'";
                string ControlName = globalConst.CurSite.SiteConn.GetString(sql);
                if (ControlName == null)
                {
                    MsgBox.Error(res._pageware.GetString("m1"));
                    return null;
                }
                sql = "select paras from controls where id='" + ControlID + "'";
                string paras = globalConst.CurSite.SiteConn.GetString(sql);
                string DataSourceID = rdm.getDataSourceID();
                sql = "select count(id) from controls where datasource='" + DataSourceID + "'";
                while (globalConst.CurSite.SiteConn.GetInt32(sql) > 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    DataSourceID = rdm.getDataSourceID();
                    sql = "select count(id) from controls where datasource='" + DataSourceID + "'";
                }
                string NewControlID = rdm.getID() + "_ctrl";
                sql = "select count(id) from controls where id='" + NewControlID + "'";
                while (globalConst.CurSite.SiteConn.GetInt32(sql) > 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    NewControlID = rdm.getID() + "_ctrl";
                    sql = "select count(id) from controls where id='" + NewControlID + "'";
                }
                sql = "insert into controls(id,name,caption,datasource,shared,paras,addtime)values('" + NewControlID + "','" + ControlName + "','" + ControlCaption + "','" + DataSourceID + "',0,'"+str.Dot2DotDot(paras)+"','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "')";
                globalConst.CurSite.SiteConn.execSql(sql);
                //
                sql = "select name,partxml,asportal from parts where controlid='" + ControlID + "'";
                DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql));
                while (dr.Read())
                {
                    string partname = dr.getString(0);
                    string partxml = dr.getString(1);
                    int asportal = dr.getInt32(2);
                    //
                    string newpartid = rdm.getID() + "_part";
                    sql = "select count(id) from parts where id='" + newpartid + "'";
                    while (db3.GetInt32(sql) > 0)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        newpartid = rdm.getID() + "_part";
                        sql = "select count(id) from parts where id='" + newpartid + "'";
                    }
                    //
                    string clonesql = "insert into parts(id,name,controlid,partxml,asportal)values('" + newpartid + "','" + partname + "','" + NewControlID + "','" + partxml.Replace("'", "''") + "'," + asportal + ")";
                    db3.execSql(clonesql);
                }
                dr.Close();
                return NewControlID;
            }
            catch (Exception ex)
            {
                new error(ex);
                return null;
            }
            finally
            {
                db3.Close();
            }
        }
        public static string[] AddPart(string partid)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            string[] returnSA = new string[3];
            DB db3 = new DB();
            db3.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db");
            try
            {
                string sql = "select a.name as partname,b.name as controlname,b.id as controlid from parts a,controls b where a.id='" + partid + "' and a.controlid=b.id";
                OleDbDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                string partname = "";
                string controlname = "";
                string controlid = "";
                if (rdr.Read())
                {
                    partname = rdr.GetString(0);
                    controlname = rdr.GetString(1);
                    controlid = rdr.GetString(2);
                }
                else
                {
                    rdr.Close();
                    return null;
                }
                rdr.Close();
                returnSA[0] = partname;
                sql = "select * from parts where name='" + partname + "' and controlname='" + controlname + "'";
                rdr = globalConst.ConfigConn.OpenRecord(sql);
                if (rdr.Read())
                {
                    returnSA[1] = rdr["caption"].ToString();
                    string PartName = partname;
                    string PartXml = rdr["partxml"].ToString();
                    string partXml = ConvertPartXML(PartXml);
                    string PartID = rdm.getID() + "_part";
                    sql = "select count(id) from parts where id='" + PartID + "'";
                    while (db3.GetInt32(sql) > 0)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        PartID = rdm.getID() + "_part";
                        sql = "select count(id) from parts where id='" + PartID + "'";
                    }
                    returnSA[2] = PartID;
                    sql = "select thevalue from system where name='portaldefault'";
                    int _AsPortal = globalConst.ConfigConn.GetInt32(sql);
                    sql = "insert into parts(id,name,controlid,partxml,asportal)values('" + PartID + "','" + PartName + "','" + controlid + "','" + partXml.Replace("'", "''") + "'," + _AsPortal + ")";
                    globalConst.CurSite.SiteConn.execSql(sql);
                }
                else
                {
                    rdr.Close();
                    return null;
                }
                rdr.Close();
                return returnSA;
            }
            catch (Exception ex)
            {
                new error(ex);
                return null;
            }
            finally
            {
                db3.Close();
            }
        }
        public static string ConvertPartXML(string PartXml)
        {
            string partXml = "<partxml>";
            XmlDocument PartXmlDoc = new XmlDocument();
            PartXmlDoc.LoadXml(PartXml);
            XmlNodeList NdList = PartXmlDoc.SelectNodes("//partxml/public_params/param");
            XmlNode Nd;
            int i;
            //				int startO;
            //				int endO;
            //				//int k;
            //				string classString;
            //string cssTypeName;
            //XmlNodeList cssNds;
            //XmlNode cssNd;
            for (i = 0; i < NdList.Count; i++)
            {
                Nd = NdList[i];
                if (!Nd.SelectSingleNode("class").InnerText.StartsWith("system[@"))
                {
                    partXml += "<param>";
                    partXml += Nd.SelectSingleNode("name").OuterXml;
                    partXml += Nd.SelectSingleNode("type").OuterXml;
                    partXml += "<value>";
                    //							classString=Nd.SelectSingleNode("class").InnerText;
                    //
                    //							endO=classString.IndexOf(",");
                    //								if(endO>0)
                    //								{
                    //								startO=classString.IndexOf("[");
                    //								partXml+=classString.Substring(startO+1,endO-startO-1);
                    //								}
                    partXml += Nd.SelectSingleNode("default").InnerText;
                    partXml += "</value>";
                    partXml += "</param>";
                }
            }
            NdList = PartXmlDoc.SelectNodes("//partxml/styles/style");
            partXml += "<styles>";
            for (i = 0; i < NdList.Count; i++)
            {
                Nd = NdList[i];
                partXml += "<style name=\"" + Nd.Attributes.GetNamedItem("name").Value + "\" class=\"" + Nd.Attributes.GetNamedItem("class").Value + "\" csstext=\"" + Nd.Attributes.GetNamedItem("csstext").Value + "\">";
                //					//add style from style element;removed 2005-7-18
                //					cssNd=globalConst.CssTypeDoc.SelectSingleNode("//configuration/csstype[@name='" + cssTypeName + "']");
                //					cssNds=cssNd.SelectNodes("ele");
                //						for(k=0;k<cssNds.Count;k++)
                //						{
                //						partXml+="<ele name=\"" + cssNds[k].InnerText + "\"></ele>";
                //						}
                partXml += "</style>";
            }
            partXml += "</styles>";
            partXml += "</partxml>";
            PartXmlDoc = null;
            return partXml;
        }
        public static string ClonePart(string partid)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                string sql = "select * from parts where id='" + partid + "'";
                DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql));
                string partname = "";
                string controlid = "";
                string partxml = "";
                int asportal = 0;
                if (dr.Read())
                {
                    partname = dr.getString("name");
                    controlid = dr.getString("controlid");
                    partxml = dr.getString("partxml");
                    asportal = dr.getInt32("asportal");
                }
                else
                {
                    dr.Close();
                    return null;
                }
                dr.Close();
                string PartID = rdm.getID() + "_part";
                sql = "select count(id) from parts where id='" + PartID + "'";
                while (globalConst.CurSite.SiteConn.GetInt32(sql) > 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    PartID = rdm.getID() + "_part";
                    sql = "select count(id) from parts where id='" + PartID + "'";
                }
                sql = "insert into parts(id,name,controlid,partxml,asportal)values('" + PartID + "','" + partname + "','" + controlid + "','" + partxml.Replace("'", "''") + "'," + asportal + ")";
                globalConst.CurSite.SiteConn.execSql(sql);
                return PartID;
            }
            catch (Exception ex)
            {
                new error(ex);
                return null;
            }
        }
        public static bool AddPart2Editor(string partid, Editor edr)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                if (edr != null && partid != null)
                {
                    string pageid = edr.thisID;
                    //
                    string WareID;
                    string WareName;
                    string PartName;
                    string PartXml;
                    int AsPortal;
                    string sql = "select a.name,a.controlid,a.partxml,b.name,a.asportal from parts a,controls b where a.id='" + partid + "' and a.controlid=b.id";
                    OleDbDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                    if (rdr.Read())
                    {
                        PartName = rdr.GetString(0);
                        WareID = rdr.GetString(1);
                        PartXml = rdr.GetString(2);
                        WareName = rdr.GetString(3);
                        AsPortal = rdr.GetInt32(4);
                    }
                    else
                    {
                        log.Error("partid is " + partid + ": Part not found!", "getPartHtml");
                        rdr.Close();
                        MsgBox.Error(res._pageware.GetString("m2"));
                        return false;
                    }
                    rdr.Close();
                    //
                    if (edr.editocx.pasteHtml(" " + globalConst.PageWare.getControlEditHead(partid, PartName, "1px", "1px", WareName) + getPartHtml(WareID, WareName, PartName, PartXml, AsPortal) + globalConst.PageWare.getControlEditTail() + " "))
                    {
                        sql = "insert into part_in_page(pageid,partid)values('" + pageid + "','" + partid + "')";
                        globalConst.CurSite.SiteConn.execSql(sql);
                        //
                        refreshPartsInSiteTreeNoCtrl(pageid);
                        refreshPagesInCtrlTree(partid);
                    }
                }
                else
                {
                    MsgBox.Error(res._pageware.GetString("m3"));
                    return false;
                }
                return true;
                //刷新
            }
            catch (Exception ex)
            {
                new error(ex);
                return false;
            }
        }
        public static bool AddControl(string ControlCaption, string ControlName, bool IsNowAdd2Page)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            DB db3 = new DB();
            db3.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db");
            //添加一个组件到页面
            string WareCaption = ControlCaption;
            string WareName = ControlName;
            try
            {
                if (IsNowAdd2Page)
                {
                    if (form.getEditor() == null)
                    {
                        return false;
                    }
                }
                //数据库处理
                string WareID = rdm.getID() + "_ctrl";
                string sql = "select count(id) from controls where id='" + WareID + "'";
                while (globalConst.CurSite.SiteConn.GetInt32(sql) > 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    WareID = rdm.getID() + "_ctrl";
                    sql = "select count(id) from controls where id='" + WareID + "'";
                }
                int AsPortal = 1;
                string PartID = "";
                string DataSourceID = rdm.getDataSourceID();
                sql = "select count(id) from controls where datasource='" + DataSourceID + "'";
                while (globalConst.CurSite.SiteConn.GetInt32(sql) > 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    DataSourceID = rdm.getDataSourceID();
                    sql = "select count(id) from controls where datasource='" + DataSourceID + "'";
                }
                string FirstPartName = "";
                string FirstPartID = "";
                string FirstPartXml = "";
                sql = "select paras from controls where name='" + WareName + "'";
                string Control_Paras =globalConst.ConfigConn.GetString(sql);
                if (Control_Paras.Trim().Equals("")) Control_Paras = "<paras></paras>";
                sql = "insert into controls(id,name,caption,datasource,paras,addtime)values('" + WareID + "','" + WareName + "','" + WareCaption + "','" + DataSourceID + "','" + Control_Paras.Replace("'", "''") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "')";
                globalConst.CurSite.SiteConn.execSql(sql);
                //////////////
                sql = "select * from parts where controlname='" + WareName + "'";
                OleDbDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
                while (rdr.Read())
                {
                    string PartName = rdr["name"].ToString();
                    //get FirstPartName
                    if (FirstPartName.Equals("")) FirstPartName = PartName;
                    string PartXml = rdr["partxml"].ToString();
                    string partXml = "<partxml>";
                    XmlDocument PartXmlDoc = new XmlDocument();
                    PartXmlDoc.LoadXml(PartXml);
                    XmlNodeList NdList = PartXmlDoc.SelectNodes("//partxml/public_params/param");
                    XmlNode Nd;
                    int i;
                    //				int startO;
                    //				int endO;
                    //				//int k;
                    //				string classString;
                    //string cssTypeName;
                    //XmlNodeList cssNds;
                    //XmlNode cssNd;
                    for (i = 0; i < NdList.Count; i++)
                    {
                        Nd = NdList[i];
                        if (!Nd.SelectSingleNode("class").InnerText.StartsWith("system[@"))
                        {
                            partXml += "<param>";
                            partXml += Nd.SelectSingleNode("name").OuterXml;
                            partXml += Nd.SelectSingleNode("type").OuterXml;
                            partXml += "<value>";
                            //							classString=Nd.SelectSingleNode("class").InnerText;
                            //
                            //							endO=classString.IndexOf(",");
                            //								if(endO>0)
                            //								{
                            //								startO=classString.IndexOf("[");
                            //								partXml+=classString.Substring(startO+1,endO-startO-1);
                            //								}
                            partXml += Nd.SelectSingleNode("default").InnerText;
                            partXml += "</value>";
                            partXml += "</param>";
                        }
                    }
                    NdList = PartXmlDoc.SelectNodes("//partxml/styles/style");
                    partXml += "<styles>";
                    for (i = 0; i < NdList.Count; i++)
                    {
                        Nd = NdList[i];
                        partXml += "<style name=\"" + Nd.Attributes.GetNamedItem("name").Value + "\" class=\"" + Nd.Attributes.GetNamedItem("class").Value + "\" csstext=\"" + Nd.Attributes.GetNamedItem("csstext").Value + "\">";
                        //					//add style from style element;removed 2005-7-18
                        //					cssNd=globalConst.CssTypeDoc.SelectSingleNode("//configuration/csstype[@name='" + cssTypeName + "']");
                        //					cssNds=cssNd.SelectNodes("ele");
                        //						for(k=0;k<cssNds.Count;k++)
                        //						{
                        //						partXml+="<ele name=\"" + cssNds[k].InnerText + "\"></ele>";
                        //						}
                        partXml += "</style>";
                    }
                    partXml += "</styles>";
                    partXml += "</partxml>";
                    //get FirstPartXml
                    if (FirstPartXml.Equals("")) FirstPartXml = partXml;
                    PartID = rdm.getID() + "_part";
                    sql = "select count(id) from parts where id='" + PartID + "'";
                    while (db3.GetInt32(sql) > 0)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        PartID = rdm.getID() + "_part";
                        sql = "select count(id) from parts where id='" + PartID + "'";
                    }
                    sql = "select thevalue from system where name='portaldefault'";
                    AsPortal = globalConst.ConfigConn.GetInt32(sql);
                    if (FirstPartID.Equals("")) FirstPartID = PartID;
                    sql = "insert into parts(id,name,controlid,partxml,asportal)values('" + PartID + "','" + PartName + "','" + WareID + "','" + partXml.Replace("'", "''") + "'," + AsPortal + ")";
                    globalConst.CurSite.SiteConn.execSql(sql);
                }
                rdr.Close();

                if (IsNowAdd2Page)
                {
                    //页面处理	
                    Editor ed = form.getEditor();
                    if (ed != null)
                    {
                        //设置默认的class和csstext
                        //setPartDefaultStyle(WareID,WareName,FirstPartID,FirstPartName,FirstPartXml);
                        if (ed.editocx.pasteHtml(" " + globalConst.PageWare.getControlEditHead(FirstPartID, FirstPartName, "1px", "1px", WareName) + getPartHtml(WareID, WareName, FirstPartName, FirstPartXml, AsPortal) + globalConst.PageWare.getControlEditTail() + " "))
                        {
                            sql = "insert into part_in_page(pageid,partid)values('" + ed.thisID + "','" + FirstPartID + "')";
                            globalConst.CurSite.SiteConn.execSql(sql);
                            //
                            refreshPartsInSiteTree(ed.thisID);
                        }
                    }
                    //刷新
                }
                return true;
            }
            catch (Exception ex)
            {
                new error(ex);
                return false;
            }
            finally
            {
                db3.Close();
            }

        }
        public static void refreshPartsInSiteTreeNoCtrl(string pageid)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                TreeNode nd = tree.getSiteNodeByID(pageid);
                if (nd == null) return;
                nd.Nodes.Clear();
                //
                string psql = "select a.partid as partid,b.name as name,c.caption as caption,c.name as controlname from part_in_page a,parts b,controls c where a.pageid='" + pageid + "' and a.partid=b.id and c.id=b.controlid";
                DR pdr = new DR(globalConst.CurSite.SiteConn.OpenRecord(psql));
                while (pdr.Read())
                {
                    TreeNode ndp;
                    string thisctrlcaption = pdr.getString("caption");
                    string controlname = pdr.getString("controlname");
                    string partname = pdr.getString("name");
                    string controlcaption = "{NoCtrl}";
                    string partcaption = "{NoPart}";
                    string csql = "select a.caption as controlcaption,b.caption as partcaption from controls a,parts b where a.name='" + controlname + "' and a.name=b.controlname and b.name='" + partname + "'";
                    DR cpdr = new DR(globalConst.ConfigConn.OpenRecord(csql));
                    if (cpdr.Read())
                    {
                        controlcaption = cpdr.getString("controlcaption");
                        partcaption = cpdr.getString("partcaption"); 
                    }
                    cpdr.Close();
                    if (globalConst.siteTreeShowColName.Equals("name"))
                    {
                        ndp = new TreeNode(thisctrlcaption + "_" + controlname + "_" + partname, 20, 20);
                    }
                    else
                    {
                        ndp = new TreeNode(thisctrlcaption + "_" + controlcaption + "_" + partcaption, 20, 20);
                    }
                    string[] tagp = new string[3];
                    tagp[0] = thisctrlcaption + "_" + controlname + "_" + partname;
                    tagp[1] = thisctrlcaption + "_" + controlcaption + "_" + partcaption;
                    tagp[2] = pdr.getString("partid");
                    ndp.Tag = tagp;
                    nd.Nodes.Add(ndp);
                }
                nd.Expand();
                pdr.Close();
                //
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void refreshPartsInSiteTree(string pageid)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                TreeNode nd = tree.getSiteNodeByID(pageid);
                if (nd == null) return;
                nd.Nodes.Clear();
                //
                string psql = "select a.partid as partid,b.name as name,c.caption as caption,c.name as controlname from part_in_page a,parts b,controls c where a.pageid='" + pageid + "' and a.partid=b.id and c.id=b.controlid";
                DR pdr = new DR(globalConst.CurSite.SiteConn.OpenRecord(psql));
                while (pdr.Read())
                {
                    TreeNode ndp;
                    string thisctrlcaption = pdr.getString("caption");
                    string controlname = pdr.getString("controlname");
                    string partname = pdr.getString("name");
                    string controlcaption = "{NoCtrl}";
                    string partcaption = "{NoPart}";
                    string csql = "select a.caption as controlcaption,b.caption as partcaption from controls a,parts b where a.name='" + controlname + "' and a.name=b.controlname and b.name='" + partname + "'";
                    DR cpdr = new DR(globalConst.ConfigConn.OpenRecord(csql));
                    if (cpdr.Read())
                    {
                        controlcaption = cpdr.getString("controlcaption");
                        partcaption = cpdr.getString("partcaption");
                    }
                    cpdr.Close();
                    if (globalConst.siteTreeShowColName.Equals("name"))
                    {
                        ndp = new TreeNode(thisctrlcaption + "_" + controlname + "_" + partname, 20, 20);
                    }
                    else
                    {
                        ndp = new TreeNode(thisctrlcaption + "_" + controlcaption + "_" + partcaption, 20, 20);
                    }
                    string[] tagp = new string[3];
                    tagp[0] = thisctrlcaption + "_" + controlname + "_" + partname;
                    tagp[1] = thisctrlcaption + "_" + controlcaption + "_" + partcaption;
                    tagp[2] = pdr.getString("partid");
                    ndp.Tag = tagp;
                    nd.Nodes.Add(ndp);
                }
                nd.Expand();
                pdr.Close();
                //
                Workspace.refreshControlTree();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void refreshPagesInCtrlTree(string partid)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                TreeNode nd = tree.getCtrlNodeByID(partid);
                if (nd == null) return;
                nd.Nodes.Clear();
                //
                string orderbystring = "";
                string orderstring = "";
                if (globalConst.ctrlTreeOrderby.Equals("name")) orderbystring = " order by name ";
                if (globalConst.ctrlTreeOrderby.Equals("caption")) orderbystring = " order by caption ";
                if (globalConst.ctrlTreeOrderby.Equals("addtime")) orderbystring = " order by caption ";
                orderstring = orderbystring + globalConst.ctrlTreeOrdertype;
                string pcpsql = "select a.pageid as pageid,b.name as name,b.caption as caption from part_in_page a,pages b where a.partid='" + partid + "' and a.pageid=b.id" + orderstring;
                DR dr5 = new DR(globalConst.CurSite.SiteConn.OpenRecord(pcpsql));
                while (dr5.Read())
                {
                    TreeNode nd4 = new TreeNode(dr5.getString(globalConst.ctrlTreeShowColName), 19, 19);

                    string[] tag = new string[3];
                    tag[0] = dr5.getString("name");
                    tag[1] = dr5.getString("caption");
                    tag[2] = dr5.getString("pageid");
                    nd4.Tag = tag;
                    nd.Nodes.Add(nd4);

                }
                nd.Expand();
                dr5.Close();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        //move by alice.2005-7-25,style another way
        //		public static void setPartDefaultStyle(string WareID,string WareName,string PartID,string PartName,string PartXml)
        //		{
        //			try
        //			{
        //				XmlDocument PartXmlDoc=new XmlDocument();
        //				PartXmlDoc.LoadXml(PartXml);
        //				XmlNodeList nds=PartXmlDoc.SelectNodes("//partxml/param");
        //				object[] Params=new object[nds.Count+1];
        //				Params[0]=WareID;
        //				int i;
        //				XmlNode nd;
        //				for(i=0;i<nds.Count;i++)
        //				{
        //					nd=nds[i];
        //					switch(nd.SelectSingleNode("type").InnerText.ToLower())
        //					{
        //						case "int":
        //							Params[i+1]=int.Parse(nd.SelectSingleNode("value").InnerText);
        //							break;
        //						case "string":
        //							Params[i+1]=nd.SelectSingleNode("value").InnerText;
        //							break;
        //						case "datetime":
        //							Params[i+1]=nd.SelectSingleNode("value").InnerText;
        //							break;
        //						case "object":
        //							Params[i+1]=(object)(nd.SelectSingleNode("value").InnerText);
        //							break;
        //						default:Params[i+1]="Unkown Param Type!";break;
        //					}
        //				}
        //
        //				Assembly assem=Assembly.LoadFile(globalConst.LibPath + @"\d4_" + WareName + ".dll");
        //				Type MyAppType=assem.GetType("d4_" + WareName + "." + PartName);
        //				object obj = MyAppType.InvokeMember(null,BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null,null);
        //				string PartHtml = (string)MyAppType.InvokeMember("getHtml",BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance,null,obj,Params);
        //				MyAppType=null;
        //				obj=null;
        //				assem=null;
        //				//
        //				HTMLDocumentClass hc = new HTMLDocumentClass();
        //				IHTMLDocument2 doc2 = hc;
        //				doc2.write(PartHtml);
        //				foreach(IHTMLElement ie in doc2.all)
        //				{
        //					if(ie.getAttribute("setstyle",0)!=null)
        //					{
        //						if(!ie.getAttribute("setstyle",0).ToString().Trim().Equals(""))
        //						{
        //							new MsgBox(ie.outerHTML);
        //							string setstyle=ie.getAttribute("setstyle",0).ToString();
        //							string stylename=setstyle.Substring(2,setstyle.IndexOf("[")-2);
        //							string classname="";
        //							string csstext="";
        //							if(ie.className!=null)classname=ie.className;
        //							if(ie.style.cssText!=null)csstext=ie.style.cssText;
        //							XmlNode ndd=PartXmlDoc.SelectSingleNode("//partxml/styles/style[@name='" + stylename + "']");
        //							if(ndd!=null)
        //							{
        //								ndd.Attributes["class"].Value=classname;
        //								ndd.Attributes["csstext"].Value=csstext;
        //							}
        //						}
        //					}
        //				}
        //				doc2.close();
        //				string sql="update parts set partxml='" + PartXmlDoc.OuterXml.Replace("'","''") + "' where id='" + PartID + "'";
        //				globalConst.CurSite.SiteConn.execSql(sql);
        //			}
        //			catch(Exception ex)
        //			{
        //				new error(ex);
        //			}
        //		}
        public static void updatePartStyle(string PartID, string name, string csstext)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                string sql = "select partxml from parts where id='" + PartID + "'";
                string partxml = globalConst.CurSite.SiteConn.GetString(sql);
                if (partxml == null)
                {
                    log.Error("PartID is " + PartID + ": Part not found!", "updatePartStyle");
                    return;
                }
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(partxml);
                XmlNode nd = dom.SelectSingleNode("//partxml/styles/style[@name='" + name + "']");
                if (nd == null)
                {
                    log.Error("PartID is " + PartID + ": Part not found!", "updatePartStyle");
                    return;
                }
                nd.Attributes["csstext"].Value = csstext;
                sql = "update parts set partxml='" + dom.OuterXml.Replace("'", "''") + "' where id='" + PartID + "'";
                globalConst.CurSite.SiteConn.execSql(sql);
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void updatePartClass(string PartID, string name, string classname)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                string sql = "select partxml from parts where id='" + PartID + "'";
                string partxml = globalConst.CurSite.SiteConn.GetString(sql);
                if (partxml == null)
                {
                    log.Error("PartID is " + PartID + ": Part not found!", "updatePartClass");
                    return;
                }
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(partxml);
                XmlNode nd = dom.SelectSingleNode("//partxml/styles/style[@name='" + name + "']");
                if (nd == null)
                {
                    log.Error("PartID is " + PartID + ": Part not found!", "updatePartClass");
                    return;
                }
                nd.Attributes["class"].Value = classname;
                sql = "update parts set partxml='" + dom.OuterXml.Replace("'", "''") + "' where id='" + PartID + "'";
                globalConst.CurSite.SiteConn.execSql(sql);
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static string getPartHtml(string PartID)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                string WareID;
                string WareName;
                string PartName;
                string PartXml;
                int AsPortal;
                string sql = "select a.name,a.controlid,a.partxml,b.name,a.asportal from parts a,controls b where a.id='" + PartID + "' and a.controlid=b.id";
                OleDbDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                if (rdr.Read())
                {
                    PartName = rdr.GetString(0);
                    WareID = rdr.GetString(1);
                    PartXml = rdr.GetString(2);
                    WareName = rdr.GetString(3);
                    AsPortal = rdr.GetInt32(4);
                }
                else
                {
                    log.Error("PartID is " + PartID + ": Part not found!", "getPartHtml");
                    rdr.Close();
                    return "{getPartHtml Error}"; ;
                }
                rdr.Close();

                return getPartHtml(WareID, WareName, PartName, PartXml, AsPortal);
            }
            catch (Exception ex)
            {
                new error(ex);
                return "{getPartHtml Error}";
            }
        }
        public static string getPartHtml(string WareID, string WareName, string PartName, string PartXml, int AsPortal)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                XmlDocument PartXmlDoc = new XmlDocument();
                PartXmlDoc.LoadXml(PartXml);
                XmlNodeList nds;
                int i;
                XmlNode nd;
                nds = PartXmlDoc.SelectNodes("//partxml/param");
                object[] Params = new object[nds.Count + 2];
                Params[0] = WareID;

                for (i = 0; i < nds.Count; i++)
                {
                    nd = nds[i];
                    switch (nd.SelectSingleNode("type").InnerText.ToLower())
                    {
                        case "int":
                            Params[i + 2] = int.Parse(nd.SelectSingleNode("value").InnerText);
                            break;
                        case "string":
                            string mayimg = nd.SelectSingleNode("value").InnerText;
                            if (mayimg.StartsWith("<IMG") || mayimg.StartsWith("<img"))
                            {
                                if (mayimg.IndexOf("/control.resource") >= 0)
                                {
                                    mayimg = mayimg.Replace("src=\"/control.resource", "src=\"file:///" + globalConst.CurSite.Path.Replace("\\", "/") + "/control.resource");
                                }
                                else
                                {
                                    mayimg = mayimg.Replace("src=\"/lib", "src=\"file:///" + globalConst.AppPath.Replace("\\", "/") + "/lib");
                                }
                            }
                            Params[i + 2] = mayimg;
                            break;
                        case "datetime":
                            Params[i + 2] = nd.SelectSingleNode("value").InnerText;
                            break;
                        case "object":
                            Params[i + 2] = (object)(nd.SelectSingleNode("value").InnerText);
                            break;
                        case "double":
                            Params[i + 2] = double.Parse(nd.SelectSingleNode("value").InnerText);
                            break;
                        default: Params[i + 2] = "Unkown Param Type!"; break;
                    }
                }
                //SetStyle String
                nds = PartXmlDoc.SelectNodes("//partxml/styles/style");
                string SetStyle = "";
                for (i = 0; i < nds.Count; i++)
                {
                    nd = nds[i];
                    SetStyle += nd.Attributes["name"].Value + "}" + nd.Attributes["class"].Value + "}" + nd.Attributes["csstext"].Value;
                    if (i < nds.Count - 1)
                    {
                        SetStyle += "{";
                    }
                }
                Params[1] = SetStyle;
                Assembly assem = Assembly.LoadFile(globalConst.LibPath + @"\ft_" + WareName + ".dll");
                Type MyAppType = assem.GetType("ft_" + WareName + "." + PartName);
                object obj = MyAppType.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
                string PartHtml = (string)MyAppType.InvokeMember("getHtml", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, obj, Params);
                MyAppType = null;
                obj = null;
                assem = null;
                if (AsPortal == 1)
                {
                    PartHtml = globalConst.PageWare.getPortalHead() + PartHtml + globalConst.PageWare.getPortalTail();
                }

                string sql = "select caption from controls where id='"+WareID+"'";
                string InstanceName=globalConst.CurSite.SiteConn2.GetString(sql);
                PartHtml = "<label style='color:blue'>" + InstanceName + "</label>" + PartHtml;
                return PartHtml;
            }
            catch (Exception ex)
            {
                new error(ex);
                return "{getPartHtml Error}";
            }
        }
        public static void doHtmlAdapter(mshtml.IHTMLElement ele)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            globalConst.MdiForm.CurPropTag.Tag = ele;
            //如果是表单元素，先绑定表单属性
            if (PageWare.isFormElement(ele) && globalConst.MdiForm.CurPropTag.SelectedIndex==0)
            {
                if (globalConst.FormDataMode)
                {
                    FormData.ele = ele;
                    string eleid = null;
                    switch (ele.tagName.ToLower())
                    {
                        case "label":
                            SiteMatrix.PropertySpace.FormElementData.Label.curEle = ele;
                            globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.Label.Bag();
                            break;
                        case "input":
                            switch (Adapter.Property.PropertyAdapter.getEleAttr(ele, "type").ToLower())
                            {
                                case "text":
                                    eleid = SiteMatrix.PropertySpace.FormElementData.func.getID(ele);
                                    if (eleid.Equals("ftform_memberinput"))
                                    {
                                        AdapterProperty(null);
                                    }
                                    else
                                    {
                                        SiteMatrix.PropertySpace.FormElementData.TextBox.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.TextBox.Bag();
                                    }
                                    break;
                                case "password":
                                    SiteMatrix.PropertySpace.FormElementData.Password.curEle = ele;
                                    globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.Password.Bag();
                                    break;
                                case "button":
                                    eleid = SiteMatrix.PropertySpace.FormElementData.func.getID(ele);
                                    if (eleid.Equals("ftform_flowaddaction"))
                                    {
                                        SiteMatrix.PropertySpace.FormElementData.Flow.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.Flow.Bag();
                                    }
                                    else if (eleid.Equals("ftform_flowmodaction"))
                                    {
                                        SiteMatrix.PropertySpace.FormElementData.Flow.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.Flow.Bag();
                                    }
                                    else if (eleid.Equals("ftform_flowdataaction"))
                                    {
                                        SiteMatrix.PropertySpace.FormElementData.Flow.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.Flow.Bag();
                                    }
                                    else if (eleid.Equals("ftform_flowflowaction"))
                                    {
                                        SiteMatrix.PropertySpace.FormElementData.Flow.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.Flow.Bag();
                                    }
                                    else if (eleid.Equals("ftform_flowext1action"))
                                    {
                                        SiteMatrix.PropertySpace.FormElementData.FlowExt1.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.FlowExt1.Bag();
                                    }
                                    else if (eleid.Equals("ftform_flowext2action"))
                                    {
                                        SiteMatrix.PropertySpace.FormElementData.FlowExt2.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.FlowExt2.Bag();
                                    }
                                    else if (eleid.Equals("ftform_rowrate"))
                                    {
                                        SiteMatrix.PropertySpace.FormElementData.RowRate.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.RowRate.Bag();
                                    }
                                    else
                                    {
                                        AdapterProperty(null);
                                    }
                                    break;
                                case "submit":
                                    AdapterProperty(null);
                                    break;
                                case "reset":
                                    AdapterProperty(null);
                                    break;
                                case "checkbox":
                                    SiteMatrix.PropertySpace.FormElementData.Checkbox.curEle = ele;
                                    globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.Checkbox.Bag();
                                    break;
                                case "radio":
                                    SiteMatrix.PropertySpace.FormElementData.Radio.curEle = ele;
                                    globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.Radio.Bag();
                                    break;
                                case "file":
                                    SiteMatrix.PropertySpace.FormElementData.FileBox.curEle = ele;
                                    globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.FileBox.Bag();
                                    break;
                                default:
                                    AdapterProperty(null);
                                    break;
                            }
                            break;
                        case "textarea":
                            SiteMatrix.PropertySpace.FormElementData.TextArea.curEle = ele;
                            globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.TextArea.Bag();
                            break;
                        case "select":
                            eleid = SiteMatrix.PropertySpace.FormElementData.func.getID(ele);
                            if (eleid.Equals("ftform_category"))
                            {
                                SiteMatrix.PropertySpace.FormElementData.Category.curEle = ele;
                                globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.Category.Bag();
                            }
                            else if (eleid.Equals("ftform_dateyear"))
                            {
                                SiteMatrix.PropertySpace.FormElementData.DateAll.curEle = ele;
                                globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.DateAll.Bag();
                            }
                            else if (eleid.Equals("ftform_datemonth"))
                            {
                                SiteMatrix.PropertySpace.FormElementData.DateAll.curEle = ele;
                                globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.DateAll.Bag();
                            }
                            else if (eleid.Equals("ftform_dateday"))
                            {
                                SiteMatrix.PropertySpace.FormElementData.DateAll.curEle = ele;
                                globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.DateAll.Bag();
                            }
                            else if (eleid.Equals("ftform_statfilter"))
                            {
                                SiteMatrix.PropertySpace.FormElementData.StatFilter.curEle = ele;
                                globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.StatFilter.Bag();
                            }
                            else
                            {
                                SiteMatrix.PropertySpace.FormElementData.Select.curEle = ele;
                                globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.Select.Bag();
                            }
                            break;
                        case "span":
                            SiteMatrix.PropertySpace.FormElementData.RowIndex.curEle = ele;
                            globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElementData.RowIndex.Bag();
                            break;
                    }
                }
                else
                {
                    string eleid = null;
                    switch (ele.tagName.ToLower())
                    {
                        case "label":
                            SiteMatrix.PropertySpace.FormElement.Label.curEle = ele;
                            globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.Label.Bag();
                            break;
                        case "input":
                            switch (Adapter.Property.PropertyAdapter.getEleAttr(ele, "type").ToLower())
                            {
                                case "text":
                                    eleid = SiteMatrix.PropertySpace.FormElement.func.getID(ele);
                                    if (eleid.Equals("ftform_memberinput"))
                                    {
                                        AdapterProperty(null);
                                    }
                                    else
                                    {
                                        SiteMatrix.PropertySpace.FormElement.TextBox.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.TextBox.Bag();
                                    }
                                    break;
                                case "password":
                                    SiteMatrix.PropertySpace.FormElement.Password.curEle = ele;
                                    globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.Password.Bag();
                                    break;
                                case "button":
                                    eleid = SiteMatrix.PropertySpace.FormElement.func.getID(ele);
                                    if (eleid.Equals("ftform_flowaddaction"))
                                    {
                                        SiteMatrix.PropertySpace.FormElement.Flow.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.Flow.Bag();
                                    }
                                    else if (eleid.Equals("ftform_flowmodaction"))
                                    {
                                        SiteMatrix.PropertySpace.FormElement.Flow.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.Flow.Bag();
                                    }
                                    else if (eleid.Equals("ftform_flowflowaction"))
                                    {
                                        SiteMatrix.PropertySpace.FormElement.Flow.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.Flow.Bag();
                                    }
                                    else if (eleid.Equals("ftform_flowext1action"))
                                    {
                                        SiteMatrix.PropertySpace.FormElement.FlowExt1.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.FlowExt1.Bag();
                                    }
                                    else if (eleid.Equals("ftform_flowext2action"))
                                    {
                                        SiteMatrix.PropertySpace.FormElement.FlowExt2.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.FlowExt2.Bag();
                                    }
                                    else if (eleid.Equals("ftform_rowrate"))
                                    {
                                        SiteMatrix.PropertySpace.FormElement.RowRate.curEle = ele;
                                        globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.RowRate.Bag();
                                    }
                                    else
                                    {
                                        AdapterProperty(null);
                                    }
                                    break;
                                case "submit":
                                    AdapterProperty(null);
                                    break;
                                case "reset":
                                    AdapterProperty(null);
                                    break;
                                case "checkbox":
                                    SiteMatrix.PropertySpace.FormElement.Checkbox.curEle = ele;
                                    globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.Checkbox.Bag();
                                    break;
                                case "radio":
                                    SiteMatrix.PropertySpace.FormElement.Radio.curEle = ele;
                                    globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.Radio.Bag();
                                    break;
                                case "file":
                                    SiteMatrix.PropertySpace.FormElement.FileBox.curEle = ele;
                                    globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.FileBox.Bag();
                                    break;
                                default:
                                    AdapterProperty(null);
                                    break;
                            }
                            break;
                        case "textarea":
                            SiteMatrix.PropertySpace.FormElement.TextArea.curEle = ele;
                            globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.TextArea.Bag();
                            break;
                        case "select":
                            eleid = SiteMatrix.PropertySpace.FormElement.func.getID(ele);
                            if (eleid.Equals("ftform_category"))
                            {
                                SiteMatrix.PropertySpace.FormElement.Category.curEle = ele;
                                globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.Category.Bag();
                            }
                            else if (eleid.Equals("ftform_dateyear"))
                            {
                                SiteMatrix.PropertySpace.FormElement.DateAll.curEle = ele;
                                globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.DateAll.Bag();
                            }
                            else if (eleid.Equals("ftform_datemonth"))
                            {
                                SiteMatrix.PropertySpace.FormElement.DateAll.curEle = ele;
                                globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.DateAll.Bag();
                            }
                            else if (eleid.Equals("ftform_dateday"))
                            {
                                SiteMatrix.PropertySpace.FormElement.DateAll.curEle = ele;
                                globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.DateAll.Bag();
                            }
                            else if (eleid.Equals("ftform_statfilter"))
                            {
                                SiteMatrix.PropertySpace.FormElement.StatFilter.curEle = ele;
                                globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.StatFilter.Bag();
                            }
                            else
                            {
                                SiteMatrix.PropertySpace.FormElement.Select.curEle = ele;
                                globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.Select.Bag();
                            }
                            break;
                        case "span":
                            SiteMatrix.PropertySpace.FormElement.RowIndex.curEle = ele;
                            globalConst.MdiForm.PropGrid.SelectedObject = SiteMatrix.PropertySpace.FormElement.RowIndex.Bag();
                            break;
                    }
                }
                return;
            }
            switch (ele.tagName.ToLower())
            {
                case "label":
                    AdapterProperty(new Adapter.Property.LabelElement(ele));
                    break;
                case "input":
                    switch (Adapter.Property.PropertyAdapter.getEleAttr(ele, "type").ToLower())
                    {
                        case "text":
                            AdapterProperty(new Adapter.Property.InputTextElement(ele));
                            break;
                        case "password":
                            AdapterProperty(new Adapter.Property.PassWordElement(ele));
                            break;
                        case "button":
                            AdapterProperty(new Adapter.Property.InputButtonElement(ele));
                            break;
                        case "submit":
                            AdapterProperty(new Adapter.Property.InputSubmitElement(ele));
                            break;
                        case "reset":
                            AdapterProperty(new Adapter.Property.InputResetElement(ele));
                            break;
                        case "image":
                            AdapterProperty(new Adapter.Property.InputImageElement(ele));
                            break;
                        case "checkbox":
                            AdapterProperty(new Adapter.Property.InputCheckBoxElement(ele));
                            break;
                        case "radio":
                            AdapterProperty(new Adapter.Property.InputRadioElement(ele));
                            break;
                        case "hidden":
                            AdapterProperty(new Adapter.Property.InputHiddenElement(ele));
                            break;
                        case "file":
                            AdapterProperty(new Adapter.Property.InputFileElement(ele));
                            break;
                        default:
                            AdapterProperty(null);
                            break;
                    }
                    break;
                case "textarea":
                    AdapterProperty(new Adapter.Property.TextAreaElement(ele));
                    break;
                case "select":
                    AdapterProperty(new Adapter.Property.SelectElement(ele));
                    break;
                case "fieldset":
                    AdapterProperty(new Adapter.Property.FieldSetElement(ele));
                    break;
                case "legend":
                    AdapterProperty(new Adapter.Property.LegendElement(ele));
                    break;
                case "a":
                    AdapterProperty(new Adapter.Property.AnchorElement(ele));
                    break;
                case "img":
                    AdapterProperty(new Adapter.Property.ImageElement(ele));
                    break;
                case "table":
                    AdapterProperty(new Adapter.Property.TabelElement(ele));
                    break;
                case "tr":
                    AdapterProperty(new Adapter.Property.TableTrElement(ele));
                    break;
                case "td":
                    AdapterProperty(new Adapter.Property.TableTdElement(ele));
                    break;
                case "th":
                    AdapterProperty(new Adapter.Property.TableThElement(ele));
                    break;
                case "font":
                    AdapterProperty(new Adapter.Property.FontElement(ele));
                    break;
                case "span":
                    if (Page.PageWare.isPartElement(ele))
                    {
                        doPartProperty(ele);
                    }
                    else
                    {
                        AdapterProperty(new Adapter.Property.SpanElement(ele));
                    }
                    break;
                case "div":
                    AdapterProperty(new Adapter.Property.DivElement(ele));
                    break;
                case "iframe":
                    AdapterProperty(new Adapter.Property.IFrameElement(ele));
                    break;
                case "hr":
                    AdapterProperty(new Adapter.Property.HRElement(ele));
                    break;
                case "form":
                    AdapterProperty(new Adapter.Property.FormElement(ele));
                    break;
                case "body":
                    AdapterProperty(new Adapter.Property.BodyElement(ele));
                    break;
                case "button":
                    AdapterProperty(new Adapter.Property.ButtonElement(ele));
                    break;
                case "p":
                    AdapterProperty(new Adapter.Property.ParaElement(ele));
                    break;
                default:
                    AdapterProperty(null);
                    break;
            }
        }
        public static void AdapterProperty(object eleclass)
        {
            globalConst.MdiForm.PropGrid.SelectedObject = eleclass;
        }
        public static mshtml.IHTMLElement getPartElement(mshtml.IHTMLElement ele)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            while (ele != null)
            {
                if (ele.tagName.ToLower().Equals("body")) return null;
                if (isPartElement(ele)) return ele;
                ele = ele.parentElement;
            }
            return null;

        }
        public static bool isPartElement(mshtml.IHTMLElement ihe)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                if (ihe == null) return false;
                if (ihe.tagName.ToLower().Equals("span"))
                {
                    if (ihe.getAttribute("id", 0) != null && ihe.getAttribute("name", 0) != null)
                    {
                        if (ihe.getAttribute("id", 0).ToString().Equals("dotforsitecom") && ihe.getAttribute("name", 0).ToString().Equals("dotforsitecom"))
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                return false;
            }

            catch (Exception ex)
            {
                new error(ex);
                return false;
            }
        }
        public static bool isFormElement(mshtml.IHTMLElement ihe)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            if (ihe == null) return false;
            if (ihe.id == null) return false;
            if (ihe.id.StartsWith("ftform_")) return true;
            return false;
        }
        public static bool isFormElementDataShouldBind(IHTMLElement ele)
        {
            if(!isFormElement(ele))return false;
            if (globalConst.FormDataMode)
            {
                FormData.ele = ele;
                string eleid = null;
                switch (ele.tagName.ToLower())
                {
                    case "label":
                        return true;
                    case "input":
                        switch (Adapter.Property.PropertyAdapter.getEleAttr(ele, "type").ToLower())
                        {
                            case "text":
                                eleid = SiteMatrix.PropertySpace.FormElementData.func.getID(ele);
                                if (eleid.Equals("ftform_memberinput"))
                                {

                                }
                                else
                                {
                                    return true;
                                }
                                break;
                            case "password":
                                return true;
                            case "checkbox":
                                return true;
                            case "radio":
                                return true;
                            case "file":
                                return true;
                            default:
                                break;
                        }
                        break;
                    case "textarea":
                        return true;
                    case "select":
                        eleid = SiteMatrix.PropertySpace.FormElementData.func.getID(ele);
                        if (eleid.Equals("ftform_category"))
                        {
                        }
                        else if (eleid.Equals("ftform_dateyear"))
                        {
                        }
                        else if (eleid.Equals("ftform_datemonth"))
                        {
                        }
                        else if (eleid.Equals("ftform_dateday"))
                        {
                        }
                        else if (eleid.Equals("ftform_statfilter"))
                        {
                        }
                        else
                        {
                            return true;
                        }
                        break;
                    case "span":
                        return true;
                }
            }
           return false;
        }
        private static void doPartProperty(mshtml.IHTMLElement ele)
        {
            //根据ele 得到 参数类型
            //根据ele得到 参数值
            //构造属性
            //构造ele style
            partid = ele.getAttribute("idname", 0).ToString();
            string sql = "select a.name as ctlname,b.partxml as partxml,b.name as partname ,a.id as ctlid,b.asportal as asportal,b.a_ip_s,b.a_ip_c,b.a_ip_o,b.a_se_s,b.a_se_c,b.a_se_o,b.a_jp_s,b.a_jp_u,b.a_tp_s,b.a_tp_c from controls a,parts b where a.id=b.controlid and b.id='" + partid + "'";
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
                AsPortal = rdr.GetInt32(4);
                CtlName = controlname;
                PartName = partname;
                Ele = ele;
                accessObj.setA_ip_s(int.Parse(rdr.GetValue(5).ToString()));
                accessObj.setA_ip_c(rdr.GetValue(6).ToString());
                accessObj.setA_ip_o(int.Parse(rdr.GetValue(7).ToString()));
                accessObj.setA_se_s(int.Parse(rdr.GetValue(8).ToString()));
                accessObj.setA_se_c(rdr.GetValue(9).ToString());
                accessObj.setA_se_o(int.Parse(rdr.GetValue(10).ToString()));
                accessObj.setA_jp_s(int.Parse(rdr.GetValue(11).ToString()));
                accessObj.setA_jp_u(rdr.GetValue(12).ToString());
                accessObj.setA_tp_s(int.Parse(rdr.GetValue(13).ToString()));
                accessObj.setA_tp_c(rdr.GetValue(14).ToString());
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
                ControlCanPublish = rdr3.GetString(8);
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
            //Width and Height
            if (Ele.style.width == null) bag.Properties.Add(new PropertySpec(CategoryPartWidth, typeof(string), CategoryMain, res._pageware.GetString("b1"), ""));
            else
                bag.Properties.Add(new PropertySpec(CategoryPartWidth, typeof(string), CategoryMain, res._pageware.GetString("b1"), Ele.style.width.ToString()));
            if (Ele.style.height == null) bag.Properties.Add(new PropertySpec(CategoryPartHeight, typeof(string), CategoryMain, res._pageware.GetString("b2"), ""));
            else
                bag.Properties.Add(new PropertySpec(CategoryPartHeight, typeof(string), CategoryMain, res._pageware.GetString("b2"), Ele.style.height.ToString()));
            //other
            //as portal
            bag.Properties.Add(new PropertySpec(CategoryPartPortal, typeof(string), CategoryMain, res._pageware.GetString("b3"), consts.DefaultConst.GetBoolCaption(AsPortal), typeof(System.Drawing.Design.UITypeEditor), typeof(AsPortalConverter)));
            
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
            ps = new PropertySpec(CategoryControlInfo, typeof(ControlInfo), CategoryInfo, res._pageware.GetString("b7"), null, typeof(System.Drawing.Design.UITypeEditor), typeof(ControlInfoConverter));
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
                    else if(CusEditorTag.StartsWith("dyvalue-SQLS_"))
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
            if (form.getEditor() == null) return;
            SiteMatrix.Adapter.PartImage.thisURL = form.getEditor().thisUrl;
            SiteMatrix.Adapter.PartImage.thisName = form.getEditor().thisName;
            SiteMatrix.Adapter.PartImage.thisPartID = partid;
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
                    ControlInfo ci = new ControlInfo();
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
                //Main
                if (e.Property.Name.Equals(CategoryPartWidth))
                {
                    if (Ele.style.width == null) e.Value = "";
                    else
                        e.Value = Ele.style.width.ToString();
                    return;
                }
                if (e.Property.Name.Equals(CategoryPartHeight))
                {
                    if (Ele.style.height == null) e.Value = "";
                    else
                        e.Value = Ele.style.height.ToString();
                    return;
                }
                if (e.Property.Name.Equals(CategoryPartPortal))
                {
                    e.Value = consts.DefaultConst.GetBoolCaption(globalConst.CurSite.SiteConn.GetInt32("select asportal from parts where id='" + partid + "'"));
                    return;
                }
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
                    string AccessIPControl_Value = (1 == accessObj.getA_ip_s() ? AccessActive : AccessActiveNot);
                    string AccessIPCondition_Value = accessObj.getA_ip_c();
                    string AccessIPConditionSide_Value = (1 == accessObj.getA_ip_o() ? AccessNormalNot : AccessNormal);
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
                    string AccessSessionControl_Value = (1 == accessObj.getA_se_s() ? AccessActive : AccessActiveNot);
                    string AccessSessionCondition_Value = accessObj.getA_se_c();
                    string AccessSessionConditionSide_Value = (1 == accessObj.getA_se_o() ? AccessNormalNot : AccessNormal);
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
                    string AccessJumpControl_Value = (1 == accessObj.getA_jp_s() ? AccessActive : AccessActiveNot);
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
                    string AccessTipControl_Value = (1 == accessObj.getA_tp_s() ? AccessActive : AccessActiveNot);
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
                StyleEditor.PartElement = Ele;
                ps3 = new PropertySpec("style", typeof(string), "_style", nd2.Attributes["description"].Value + " style", ie, typeof(StyleEditor), typeof(StyleEditorConverter));
                //ps3.Attributes=new Attribute[]{EditorAttribute(typeof(StyleEditor),typeof(UITypeEditor)), 
                //								  TypeConverterAttribute(typeof(StyleEditorConverter))};
                bagPart3.Properties.Add(ps3);

                //new MsgBox(ie.style.cssText);
                bagPart3["style"] = ie;

                ClassEditorConverter.ControlPartID = partid;
                ClassEditorConverter.PartStyleName = nd2.Attributes["name"].Value;
                ClassEditorConverter.PartElement = Ele;
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
                    //解决多个Enums和一个静态变量带来的问题。
                    ResetConverterEnums(enumnds);
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
                if (pName.Equals(CategoryPartWidth))
                {
                    if (Ele.style.height == null && pValue.ToString().Trim().Equals(""))
                    {
                        MsgBox.Information("Width and Height must be set one of them!");
                        return;
                    }
                    Ele.style.width = pValue.ToString();
                    return;
                }
                if (pName.Equals(CategoryPartHeight))
                {
                    if (Ele.style.width == null && pValue.ToString().Trim().Equals(""))
                    {
                        MsgBox.Information("Width and Height must be set one of them!");
                        return;
                    }
                    Ele.style.height = pValue.ToString();
                    return;
                }
                if (pName.Equals(CategoryPartPortal))
                {
                    globalConst.CurSite.SiteConn.execSql("update parts set asportal=" + consts.DefaultConst.GetBoolCaption(pValue.ToString()) + "  where id='" + partid + "'");
                    ApplyPartHTML(Ele, ControlID, CtlName, PartName, curPartSetXmlDom.OuterXml, consts.DefaultConst.GetBoolCaption(pValue.ToString()));
                    return;
                }
                if (pName.Equals(CategoryControlName))
                {
                    globalConst.CurSite.SiteConn.execSql("update controls set caption='" + pValue.ToString() + "'  where id='" + ControlID + "'");
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
                    ResetConverterEnums(enumnds);
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
            ApplyPartHTML(Ele, ControlID, CtlName, PartName, curPartSetXmlDom.OuterXml, AsPortal);
            System.Windows.Forms.Application.DoEvents();
            if (SiteClass.Site.JustEdit(partid))globalConst.CurSite.SiteConn.execSql("update parts set partxml='" + curPartSetXmlDom.OuterXml.Replace("'", "''") + "'where id='" + partid + "'");

        }
        public static void ResetConverterEnums(XmlNodeList enumnds)
        {
            try
            {
                string[] enumsstring = new string[enumnds.Count];
                int enumi;
                for (enumi = 0; enumi < enumnds.Count; enumi++)
                {
                    enumsstring[enumi] = enumnds[enumi].InnerText;
                }
                PartPropertyEnumConverter.Enums = enumsstring;
            }
            catch(Exception ex)
            {
                new error(ex);
            }
        }
        public static void ApplyPartHTML(IHTMLElement Ele, string ControlID, string CtlName, string PartName, string PartXml, int AsPortal)
        {
            try
            {
                Ele.innerHTML = getPartHtml(ControlID, CtlName, PartName, PartXml, AsPortal);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("HRESULT"))
                {
                    //Ele.parentElement.tagName="div";
                    //Ele.innerHTML=getPartHtml("ctlid",CtlName,PartName,curPartSetXmlDom.OuterXml);
                    //if parent element is <p> <td>...,span will accur error
                    string Eleid = Ele.getAttribute("idname", 0).ToString();
                    string Eleheight = Ele.style.height.ToString();
                    string Elewidth = Ele.style.width.ToString();
                    string Elename = Ele.getAttribute("partname", 0).ToString();
                    int ElesourceIndex = Ele.sourceIndex;
                    Ele.innerHTML = "";
                    Ele.style.cssText = "";
                    IHTMLElement itp = Ele.parentElement;
                    itp.outerHTML = itp.outerHTML.Replace(Ele.outerHTML, globalConst.PageWare.getControlEditHead(Eleid, Elename, Elewidth, Eleheight, CtlName) + getPartHtml(ControlID, CtlName, PartName, curPartSetXmlDom.OuterXml, AsPortal) + globalConst.PageWare.getControlEditTail());
                    Ele = (IHTMLElement)(((IHTMLDocument2)form.getEditor().editocx.DOM).all.item(ElesourceIndex, ElesourceIndex));
                    if (!PageWare.isPartElement(Ele)) MsgBox.Warning(res._pageware.GetString("m4"));
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
        public static void UpdateFormBindPage(string tablerow)
        {
            try
            {
                if (tablerow.IndexOf('.') < 0) return;
                Editor ed = form.getEditor();
                if (ed != null)
                {
                    XmlNode node = globalConst.CurSite.FormDataXML.SelectSingleNode("/formdata/table[@id='" + tablerow.Split('.')[0] + "']/row[@id='" + tablerow.Split('.')[1] + "']");
                    if (node != null)
                    {
                        string subpath = "sites\\" + globalConst.CurSite.ID + "\\";
                        node.SelectSingleNode("bindinfo/page").InnerText = ed.thisUrl.IndexOf(subpath) < 0 ? ed.thisUrl : ed.thisUrl.Substring(ed.thisUrl.IndexOf(subpath) + subpath.Length);
                        globalConst.CurSite.FormDataXML.Save(globalConst.ConfigPath + "\\" + globalConst.CurSite.ID + "_formtable.xml");
                    }
                }
            }
            catch { }
        }
    }
}
