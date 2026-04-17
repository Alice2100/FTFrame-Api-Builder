using System;
using FTDPClient.consts;
using FTDPClient.functions;
using System.Data.OleDb;
using FTDPClient.database;
using FTDPClient.forms;
using System.Windows.Forms;
using mshtml;
using FTDPClient.Page;
using System.Text;
using System.IO;
using FTDPClient.Compression;
using System.Net;
using System.Xml;
using FTDPClient.ftplib;
using System.Collections;
using System.Text.RegularExpressions;
using Silver.UI;
using Microsoft.Data.Sqlite;

namespace FTDPClient.classes
{
   public  class PageAsist
    {
        public static void  RefreshToolBoxPages()
        {
            Silver.UI.ToolBox TB = consts.globalConst.MdiForm.MainToolBox;
            TB.DeleteTab(5);
            TB.AddTab("5", 55);
            TB[5].View = ViewMode.List;
            TB.SelectedTabIndex = 5;
            //TB[5].DeleteAllItems();
            //TB.RefreshTabs();
            Editor ed = form.getEditor();
            if (ed == null)
            {
                TB[5].Caption = res.com.GetString("PageAsist.TB5Caption");//页面辅助
                return;
            }
            else
            {
                TB[5].Caption = ed.thisTitle;
            }
            string PageID = ed.thisID;
            IHTMLElementCollection eles = ed.editocx.getElementsByTagName("DIV"); 
            foreach (IHTMLElement ele in eles)
            {
                if (ele.className != null && (ele.className.ToLower().IndexOf("ftdiv") >= 0 || ele.className.ToLower().IndexOf("ftselect") >= 0 || ele.className.ToLower().IndexOf("divft") >= 0))
                {
                    if (ele.id != null && !ele.id.Equals(""))
                    {
                        string name = "";
                        IHTMLElement nameEle = ed.editocx.getElementById(ele.id + "_ftheader1");
                        if (nameEle != null)
                        {
                            name = nameEle.innerText;
                        }
                        Silver.UI.ToolBoxItem item = new Silver.UI.ToolBoxItem(ele.id+" "+ name, 26,false);
                        item.Object = new object[] { "div_1",ele, ele.id , PageID };
                        TB[5].AddItem(item);
                        item.Selected = false;
                    }
                }
                /*
                else if (ele.className != null && ele.className.ToLower().IndexOf("_tabdiv") >= 0)
                {
                    if (ele.id != null && !ele.id.Equals(""))
                    {
                        string name = "";
                        IHTMLElement nameEle = ed.editocx.getElementById(ele.id + "_ftheader1");
                        if (nameEle != null)
                        {
                            name = nameEle.innerText;
                        }
                        Silver.UI.ToolBoxItem item = new Silver.UI.ToolBoxItem(ele.id + " " + name, 20, false);
                        item.Object = new object[] { "div_2", ele, ele.id, PageID };
                        TB[5].AddItem(item);
                        item.Selected = false;
                    }
                }*/
            }
            eles = ed.editocx.getElementsByTagName("SPAN");
            foreach (IHTMLElement ele in eles)
            {
                if (ele.id != null && (ele.id.ToLower().Equals("ftdpcom") || ele.id.ToLower().Equals("dotforsitecom")))
                {
                    object partname = ele.getAttribute("partname");
                    object controlname = ele.getAttribute("controlname");
                    object idname = ele.getAttribute("idname");
                    string partcap = "";
                    try
                    {
                        partcap = ((IHTMLElement)(((IHTMLElementCollection)ele.children).item(0))).innerText;
                    }
                    catch
                    {
                        partcap = "(Error)";
                    }
                    if (partname == null) partname = "";
                    if (controlname == null) controlname = "";
                    if (idname == null) idname = "";
                    int imagreIndex = 27;
                    for (int i = 0; i < globalConst.ControlIcon.Length; i++)
                    {
                        if(globalConst.ControlIcon[i].ControlID== controlname.ToString())
                        {
                            imagreIndex = i;
                            break;
                        }
                    } 
                    Silver.UI.ToolBoxItem item = new Silver.UI.ToolBoxItem(partcap+"_"+ controlname+"_"+ partname, imagreIndex, false);
                    item.Object = new object[] { "control", ele, idname, PageID,controlname + "_" + partname };
                    TB[5].AddItem(item);
                    item.Selected = false;
                }
            }
            for (int i =0;i< TB[5].ItemCount; i++)
            {
                Silver.UI.ToolBoxItem item = TB[5][i];
                for (int j= 0;j< TB[5].ItemCount; j++)
                {
                    if(j!=i)
                    {
                        Silver.UI.ToolBoxItem item2 = TB[5][j];
                        if(((string)((object[])item.Object)[2])== ((string)((object[])item2.Object)[2]))
                        {
                            if(!item.Caption.StartsWith(res.com.GetString("PageAsist.chongfu")))item.Caption = res.com.GetString("PageAsist.chongfu")+" " + item.Caption;
                            item.SmallImageIndex = 72;
                        }
                    }
                }
            }
            //for (int i = TB[5].ItemCount - 1; i >= 0; i--)
            //{
            //    TB[5].SelectedItemIndex = i;
            //}
        }
        public static void CopyGetValue(IHTMLElement ele)
        {
            bool IsOK = false;
            if (Page.PageWare.isPartElement(ele))
            {
                string partid = Adapter.Property.PropertyAdapter.getEleAttr(ele, "idname");
                string sql = "select a.name,a.controlid,a.partxml,b.name,a.asportal from parts a,controls b where a.id='" + partid + "' and a.controlid=b.id";
                SqliteDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                if (rdr.Read())
                {
                    string PartName = rdr.GetString(0);
                    string WareID = rdr.GetString(1);
                    string PartXml = rdr.GetString(2);
                    string WareName = rdr.GetString(3);
                    int AsPortal = rdr.GetInt32(4);
                    rdr.Close();
                    if (WareName.Equals("dyvalue") && PartName.Equals("Interface"))
                    {
                        IsOK = true;
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.LoadXml(PartXml);
                        System.Xml.XmlNodeList nodes = doc.SelectNodes("/partxml/param");
                        System.Xml.XmlNode curnode = null;
                        foreach (System.Xml.XmlNode node in nodes)
                        {
                            if (node.SelectSingleNode("name").InnerText.Equals("FuncName"))
                            {
                                curnode = node;
                                break;
                            }
                        }
                        string FuncName = curnode.SelectSingleNode("value").InnerText+"()";
                        Clipboard.SetText(FuncName);
                        MsgBox.Information(FuncName+" "+ res.com.GetString("PageAsist.yifuzhi"));

                        curnode = null;
                        nodes = null;
                        doc = null;
                    }
                }
                else
                {
                    rdr.Close();
                }
            }
            if (!IsOK)
            {
                MsgBox.Warning(res.com.GetString("PageAsist.jkdddd"));
            }
        }
        public static string GetPartSetValue(string partId,string nodeName)
        {
            string sql = "select a.name,a.controlid,a.partxml,b.name,a.asportal from parts a,controls b where a.id='" + partId + "' and a.controlid=b.id";
            using (SqliteDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql))
            {
                if (rdr.Read())
                {
                    string PartName = rdr.GetString(0);
                    string WareID = rdr.GetString(1);
                    string PartXml = rdr.GetString(2);
                    string WareName = rdr.GetString(3);
                    int AsPortal = rdr.GetInt32(4);
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.LoadXml(PartXml);
                    System.Xml.XmlNodeList nodes = doc.SelectNodes("/partxml/param");
                    System.Xml.XmlNode curnode = null;
                    foreach (System.Xml.XmlNode node in nodes)
                    {
                        if (node.SelectSingleNode("name").InnerText.Equals(nodeName))
                        {
                            curnode = node;
                            break;
                        }
                    }
                    if (curnode != null)
                    {
                        return curnode.SelectSingleNode("value").InnerText;
                    }
                    else return null;
                }
            }
            return null;
        }
        public static void BindSetting(IHTMLElement ele)
        {
            bool IsOK = false;
            if (Page.PageWare.isPartElement(ele))
            {
                string partid = Adapter.Property.PropertyAdapter.getEleAttr(ele, "idname");
                string sql = "select a.name,a.controlid,a.partxml,b.name,a.asportal from parts a,controls b where a.id='" + partid + "' and a.controlid=b.id";
                SqliteDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                if (rdr.Read())
                {
                    string PartName = rdr.GetString(0);
                    string WareID = rdr.GetString(1);
                    string PartXml = rdr.GetString(2);
                    string WareName = rdr.GetString(3);
                    int AsPortal = rdr.GetInt32(4);
                    rdr.Close();
                    if (WareName.Equals("list") && PartName.Equals("List"))
                    {
                        IsOK = true;
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.LoadXml(PartXml);
                        System.Xml.XmlNodeList nodes = doc.SelectNodes("/partxml/param");
                        System.Xml.XmlNode curnode = null;
                        foreach (System.Xml.XmlNode node in nodes)
                        {
                            if (node.SelectSingleNode("name").InnerText.Equals("RowAll"))
                            {
                                curnode = node;
                                break;
                            }
                        }
                        RowAll gn = new RowAll();
                        gn.XmlDoc = doc;
                        gn.partId = partid;
                        gn.restr = curnode.SelectSingleNode("value").InnerText;
                        gn.ShowDialog();
                        if (!gn.IsCancel)
                        {
                            curnode.SelectSingleNode("value").InnerText = gn.restr;
                            sql = "update parts set partxml='" + str.Dot2DotDot(doc.OuterXml) + "' where id='" + partid + "'";
                            globalConst.CurSite.SiteConn.execSql(sql);
                            PageWare.ApplyPartHTML(ele, WareID, WareName, PartName, doc.OuterXml, AsPortal);
                        }
                        curnode = null;
                        nodes = null;
                        doc = null;
                    }
                    else if (WareName.Equals("dataop") && PartName.Equals("Interface"))
                    {
                        IsOK = true;
                        if (!DataOpDefine.DataOpFormShow)
                        {
                            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                            doc.LoadXml(PartXml);
                            System.Xml.XmlNodeList nodes = doc.SelectNodes("/partxml/param");
                            System.Xml.XmlNode curnode = null;
                            foreach (System.Xml.XmlNode node in nodes)
                            {
                                if (node.SelectSingleNode("name").InnerText.Equals("Define"))
                                {
                                    curnode = node;
                                    break;
                                }
                            }
                            DataOpDefine gn = new DataOpDefine();
                            gn.XmlDoc = doc;
                            gn.str = curnode.SelectSingleNode("value").InnerText;
                            gn.partid = partid;
                            gn.Show();

                            curnode = null;
                            nodes = null;
                            doc = null;
                        }
                        else
                        {
                            DataOpDefine.DataOpDefineForm.WindowState = FormWindowState.Normal;
                            DataOpDefine.DataOpDefineForm.Activate();
                            MsgBox.Warning("Last Data Operation Form Has Opened", DataOpDefine.DataOpDefineForm);
                            
                        }
                    }
                    else if (WareName.Equals("dyvalue") && PartName.Equals("Interface"))
                    {
                        IsOK = true;
                        if (!DyValueDefine.DyValueFormShow)
                        {
                            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                            doc.LoadXml(PartXml);
                            System.Xml.XmlNodeList nodes = doc.SelectNodes("/partxml/param");
                            System.Xml.XmlNode curnode = null;
                            foreach (System.Xml.XmlNode node in nodes)
                            {
                                if (node.SelectSingleNode("name").InnerText.Equals("Define"))
                                {
                                    curnode = node;
                                    break;
                                }
                            }
                            DyValueDefine gn = new DyValueDefine();
                            gn.XmlDoc = doc;
                            gn.str = curnode.SelectSingleNode("value").InnerText;
                            gn.partid = partid;
                            gn.Show();

                            curnode = null;
                            nodes = null;
                            doc = null;
                        }
                        else
                        {
                            DyValueDefine.DyValueDefineForm.WindowState = FormWindowState.Normal;
                            DyValueDefine.DyValueDefineForm.Activate();
                            MsgBox.Warning("Last Data Getting Form Has Opened", DyValueDefine.DyValueDefineForm);
                        }
                    }
                }
                else
                {
                    rdr.Close();
                }
            }
            if(!IsOK)
            {
                MsgBox.Warning(res.com.GetString("PageAsist.jkdddd2"));
            }
        }
        public static void SiteTreePartOpration(string partid)
        {
            string sql = "select a.name,a.controlid,a.partxml,b.name,a.asportal from parts a,controls b where a.id='" + partid + "' and a.controlid=b.id";
            SqliteDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
            if (rdr.Read())
            {
                string PartName = rdr.GetString(0);
                string WareID = rdr.GetString(1);
                string PartXml = rdr.GetString(2);
                string WareName = rdr.GetString(3);
                int AsPortal = rdr.GetInt32(4);
                rdr.Close();
                if (WareName.Equals("list") && PartName.Equals("List"))
                {
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.LoadXml(PartXml);
                    System.Xml.XmlNodeList nodes = doc.SelectNodes("/partxml/param");
                    System.Xml.XmlNode curnode = null;
                    foreach (System.Xml.XmlNode node in nodes)
                    {
                        if (node.SelectSingleNode("name").InnerText.Equals("RowAll"))
                        {
                            curnode = node;
                            break;
                        }
                    }
                    RowAll gn = new RowAll();
                    gn.XmlDoc = doc;
                    gn.partId = partid;
                    gn.restr = curnode.SelectSingleNode("value").InnerText;
                    gn.ShowDialog();
                    if (!gn.IsCancel)
                    {
                        curnode.SelectSingleNode("value").InnerText = gn.restr;
                        sql = "update parts set partxml='" + str.Dot2DotDot(doc.OuterXml) + "' where id='" + partid + "'";
                        globalConst.CurSite.SiteConn.execSql(sql);
                    }
                    curnode = null;
                    nodes = null;
                    doc = null;
                }
                else if (WareName.Equals("dataop") && PartName.Equals("Interface"))
                {
                    if (!DataOpDefine.DataOpFormShow)
                    {
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.LoadXml(PartXml);
                        System.Xml.XmlNodeList nodes = doc.SelectNodes("/partxml/param");
                        System.Xml.XmlNode curnode = null;
                        foreach (System.Xml.XmlNode node in nodes)
                        {
                            if (node.SelectSingleNode("name").InnerText.Equals("Define"))
                            {
                                curnode = node;
                                break;
                            }
                        }
                        DataOpDefine gn = new DataOpDefine();
                        gn.XmlDoc = doc;
                        gn.str = curnode.SelectSingleNode("value").InnerText;
                        gn.partid = partid;
                        gn.Show();

                        curnode = null;
                        nodes = null;
                        doc = null;
                    }
                    else
                    {
                        DataOpDefine.DataOpDefineForm.WindowState = FormWindowState.Normal;
                        DataOpDefine.DataOpDefineForm.Activate();
                        MsgBox.Warning("Last Data Operation Form Has Opened", DataOpDefine.DataOpDefineForm);
                    }
                }
                else if (WareName.Equals("dyvalue") && PartName.Equals("Interface"))
                {
                    if (!DyValueDefine.DyValueFormShow)
                    {
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.LoadXml(PartXml);
                        System.Xml.XmlNodeList nodes = doc.SelectNodes("/partxml/param");
                        System.Xml.XmlNode curnode = null;
                        foreach (System.Xml.XmlNode node in nodes)
                        {
                            if (node.SelectSingleNode("name").InnerText.Equals("Define"))
                            {
                                curnode = node;
                                break;
                            }
                        }
                        DyValueDefine gn = new DyValueDefine();
                        gn.XmlDoc = doc;
                        gn.str = curnode.SelectSingleNode("value").InnerText;
                        gn.partid = partid;
                        gn.Show();

                        curnode = null;
                        nodes = null;
                        doc = null;
                    }
                    else
                    {
                        DyValueDefine.DyValueDefineForm.WindowState = FormWindowState.Normal;
                        DyValueDefine.DyValueDefineForm.Activate();
                        MsgBox.Warning("Last Data Getting Form Has Opened", DyValueDefine.DyValueDefineForm);
                    }
                }
            }
            else
            {
                rdr.Close();
            }
        }
        public static void MouseClick(MouseEventArgs e)
        {
            Silver.UI.ToolBox TB = consts.globalConst.MdiForm.MainToolBox;
            if (TB.SelectedTabIndex == 5)
            {
                int hittest = TB[5].HitTestItem(e.X, e.Y);
                if (hittest >= 0)
                {
                    Silver.UI.ToolBoxItem item = TB[5][hittest];
                    IHTMLElement ie = ((IHTMLElement)((object[])item.Object)[1]);
                    Editor ed = form.getEditor();
                    if(ed!=null)
                    {
                        consts.globalConst.MdiForm.HasJustSelectChange = true;
                        consts.globalConst.MdiForm.CurEle = ie;
                        ed.doPropertyAdapter();
                        consts.globalConst.MdiForm.HasJustSelectChange = false;
                        //ed.editocx_onselectionchange(ie);
                    }
                }
            }
        }
        public static void MouseDoubleClick(MouseEventArgs e)
        {
            Silver.UI.ToolBox TB = consts.globalConst.MdiForm.MainToolBox;
            if (TB.SelectedTabIndex == 5)
            {
                int hittest = TB[5].HitTestItem(e.X, e.Y);
                if(hittest<0)
                {
                    RefreshToolBoxPages(); 
                    str.ShowStatus(res.com.GetString("PageAsist.yishuaxinye")); 
                }
                else
                {
                    Silver.UI.ToolBoxItem item = TB[5][hittest];
                    object[] tag = ((object[])item.Object);
                    if(((string)tag[0])=="control")
                    {
                        BindSetting(((IHTMLElement)tag[1]));
                    }
                    else if (((string)tag[0]).StartsWith("div_"))
                    {
                        DivMod(((IHTMLElement)tag[1]));
                    }
                }
            }
        }
        public static void DivDel(IHTMLElement ie)
        {
            Editor ed = form.getEditor();
            if (ed != null)
            {
                IHTMLElement ele = ed.editocx.getElementById(ie.id + "_ftheader");
                if (ele != null)
                {
                    ele.outerHTML="";
                }
                ie.outerHTML = "";
            }
            RefreshToolBoxPages();
        }
        public static void DivMod(IHTMLElement ie)
        {
            PA_Div pd = new PA_Div();
            pd.IsCopy = false;
            pd.DivEle = ie;
            pd.Text = res.com.GetString("PageAsist.genggai")+" Div[id=" + ie.id + "]";
            pd.textBox1.Text = ie.id;
            Editor ed = form.getEditor();
            if (ed != null)
            {
                IHTMLElement nameEle = ed.editocx.getElementById(ie.id+"_ftheader1");
                if(nameEle!=null)
                {
                    pd.textBox2.Text = nameEle.innerText;
                }
            }
            pd.ShowDialog();
            if(!pd.IsCancel)
            {
                RefreshToolBoxPages();
            }
        }
        public static void DivCopyNew(IHTMLElement ie)
        {
            PA_Div pd = new PA_Div();
            pd.IsCopy = true;
            pd.DivEle = ie;
            pd.Text = res.com.str("PageAsist.copynew")+" Div[id=" + ie.id + "]";
            pd.textBox1.Text = ie.id;
            Editor ed = form.getEditor();
            if (ed != null)
            {
                IHTMLElement nameEle = ed.editocx.getElementById(ie.id + "_ftheader1");
                if (nameEle != null)
                {
                    pd.textBox2.Text = nameEle.innerText;
                }
            }
            pd.ShowDialog();
            if (!pd.IsCancel)
            {
                RefreshToolBoxPages();
            }
        }
        public static void DivAddNew()
        {
            PA_Div pd = new PA_Div();
            pd.IsAddNew = true;
            pd.Text = res.com.str("PageAsist.addnew")+" Div";
            pd.ShowDialog();
            if (!pd.IsCancel)
            {
                RefreshToolBoxPages();
            }
        }
        public static void PartDel(IHTMLElement ie)
        {
            if (ie != null && PageWare.isPartElement(ie))
                {
                    IHTMLElement pie = ie.parentElement;
                    if (pie.tagName != "TD" || pie.getAttribute("tag") == null || pie.getAttribute("tag").ToString() != "FTDPPartTd")
                    {
                        ie.outerHTML = "";
                    }
                    else
                    {
                        IHTMLTable it = null;
                        if (pie.parentElement.parentElement.tagName == "TBODY") it = (IHTMLTable)pie.parentElement.parentElement.parentElement;
                        else it = (IHTMLTable)pie.parentElement.parentElement;
                        it.deleteRow(((IHTMLTableRow)pie.parentElement).rowIndex);
                    }
                    RefreshToolBoxPages();
                    str.ShowStatus(res.com.str("PageAsist.yishanchu"));
                }
        }
        public static void CopyPartLabel(IHTMLElement ie)
        {
            if (ie != null && PageWare.isPartElement(ie))
            {
                string partid = ie.getAttribute("idname").ToString();
                string nodeHTML = "<ftdp:control id=\"" + ie.getAttribute("idname", 0).ToString() + "\" name=\"" + ie.getAttribute("partname", 0).ToString() + "\"" + (ie.style.width == null ? "" : " width=\"" + ie.style.width.ToString() + "\"") + (ie.style.height == null ? "" : " height=\"" + ie.style.height.ToString() + "\"") + "></ftdp:control>";
                Clipboard.SetDataObject(nodeHTML);
                consts.globalConst.MdiForm.MainStatus.Text = "Copy Part Tag OK：" + partid;
            }
        }
        public static void CopyPartID(IHTMLElement ie)
        {
            if (ie != null && PageWare.isPartElement(ie))
            {
                string partid = ie.getAttribute("idname").ToString();
                Clipboard.SetDataObject(partid);
                consts.globalConst.MdiForm.MainStatus.Text = "Copy PartID OK：" + partid;
            }
        }
        public static void ColonePartReplace(IHTMLElement ie,string pageid)
        {
            if (ie == null || !PageWare.isPartElement(ie)) return;
            try
            {
                string partid = ie.getAttribute("idname").ToString();
                string partname = ie.getAttribute("partname").ToString();
                string sql = "select a.caption,a.name,a.id from controls a,parts b where a.id=b.controlid and b.id='" + partid + "'";
                DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql));
                string controlid = null;
                string controlcap = null;
                if (dr.Read())
                {
                    controlid = dr.getString(2);
                    controlcap = dr.getString(0);
                }
                dr.Close();
                if (controlid != null)
                {
                    NewControl nc = new NewControl();
                    nc.cloneAndReplace = true;
                    nc.controlName = controlid;
                    nc.curSelectPartEle = ie;
                    nc.clonePartName = partname;
                    nc.curpageid = pageid;
                    nc.oldpartid = partid;
                    nc.addType = "clone";
                    nc.Text = res.com.str("PageAsist.kelongtihuan");
                    nc.textBox1.Text = controlcap;
                    nc.ShowDialog();
                    if (!nc.isCancel)
                    {
                        globalConst.MdiForm.refreshControlTree();
                        //globalConst.MdiForm.ControlTree.Nodes[nodeindex].Expand();
                        RefreshToolBoxPages();
                    }
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void ColonePartNew(IHTMLElement ie, string pageid)
        {
            if (ie == null || !PageWare.isPartElement(ie)) return;
            try
            {
                string partid = ie.getAttribute("idname").ToString();
                string partname = ie.getAttribute("partname").ToString();
                string sql = "select a.caption,a.name,a.id from controls a,parts b where a.id=b.controlid and b.id='" + partid + "'";
                DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql));
                string controlid = null;
                string controlcap = null;
                if (dr.Read())
                {
                    controlid = dr.getString(2);
                    controlcap = dr.getString(0);
                }
                dr.Close();
                if (controlid != null)
                {
                    NewControl nc = new NewControl();
                    nc.cloneAndNew = true;
                    nc.controlName = controlid;
                    nc.curSelectPartEle = ie;
                    nc.clonePartName = partname;
                    nc.curpageid = pageid;
                    nc.oldpartid = partid;
                    nc.addType = "clone";
                    nc.Text = res.com.str("PageAsist.kelongnew");
                    nc.textBox1.Text = controlcap;
                    nc.ShowDialog();
                    if (!nc.isCancel)
                    {
                        globalConst.MdiForm.refreshControlTree();
                        //globalConst.MdiForm.ControlTree.Nodes[nodeindex].Expand();
                        RefreshToolBoxPages();
                    }
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
    }
}
