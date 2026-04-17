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
using Microsoft.Data.Sqlite;

namespace FTDPClient.SiteClass
{
    public class Bug
    {
        public static ArrayList Init(bool IsAllSite, ArrayList SelFiles, string SinglePageID)
        {
            ArrayList al = new ArrayList();
            if (IsAllSite)
            {
                string sql = "select id from pages";
                SqliteDataReader dr = globalConst.CurSite.SiteConn2.OpenRecord(sql);
                while (dr.Read())
                {
                    string pageid = dr.GetString(0);
                    string pagepath = getPathByPageId(pageid);
                    ArrayList bugpage = BugPage(pageid,pagepath);
                    foreach (string[] item in bugpage)
                    {
                        al.Add(item);
                    }
                }
                dr.Close();
            }
            else if (SelFiles != null)
            {
                foreach (string[] files in SelFiles)
                {
                    string pageid = files[0];
                    string pagepath = files[1];
                    ArrayList bugpage = BugPage(pageid, pagepath);
                    foreach (string[] item in bugpage)
                    {
                        al.Add(item);
                    }
                }
            }
            else if (SinglePageID != null)
            {
                string pagepath = getPathByPageId(SinglePageID);
                ArrayList bugpage = BugPage(SinglePageID, pagepath);
                foreach (string[] item in bugpage)
                {
                    al.Add(item);
                }
            }
            return al;
        }
        private static ArrayList BugPage(string pageid, string pagepath)
        {
            string pageurl = globalConst.CurSite.Path + "\\" + pagepath.Replace("/", "\\");
            string sql = "select caption from pages where id='" + str.Dot2DotDot(pageid) + "'";
            string caption = globalConst.CurSite.SiteConn.GetString(sql);
            ArrayList al = new ArrayList();
            //HTML元素标识
            //重名id
            HTMLDocumentClass hc = new HTMLDocumentClass();
            IHTMLDocument2 doc2 = hc;
            doc2.write("");
            doc2.close();
            IHTMLDocument4 doc4 = hc;
            IHTMLDocument2 doc = doc4.createDocumentFromUrl(pageurl, "null");
            while (doc.readyState != "complete")
            {
                System.Windows.Forms.Application.DoEvents();
            }
            ArrayList eleidal = new ArrayList();
            foreach (IHTMLElement ele in doc.all)
            {
                string id = ele.id;
                if (id != null && !id.Equals(""))
                {
                    if (!eleidal.Contains(id))
                    {
                        eleidal.Add(id);
                    }
                    else if(!id.StartsWith("ftform_"))
                    {
                        al.Add(new string[]{
                        res.@class.str("Class.0"),
                        pagepath,
                        caption,
                        "id\""+id+"\""+res.@class.str("Class.1")+""+ele.tagName,
                        res.@class.str("Class.2"),
                        res.@class.str("Class.3"),
                        pageid
                        });
                    }
                }
            }
            doc.close();
            //数据操作设置
            //数据获取设置
            //其他
            return al;
        }
        public static string getPathByPageId(string pageid)
        {
            string sql = "select name from pages where id='"+str.Dot2DotDot(pageid)+"'";
            string path = globalConst.CurSite.SiteConn.GetString(sql);
            sql="select pid from pages where id='" + str.Dot2DotDot(pageid) + "'";
            string pid = globalConst.CurSite.SiteConn.GetString(sql);
            while (!pid.Equals("root"))
            {
                sql = "select name from directory where id='" + str.Dot2DotDot(pid) + "'";
                string dir = globalConst.CurSite.SiteConn.GetString(sql);
                path = dir + "/" + path;
                sql = "select pid from directory where id='" + str.Dot2DotDot(pid) + "'";
                pid = globalConst.CurSite.SiteConn.GetString(sql);
            }
            return path;
        }
    }
}