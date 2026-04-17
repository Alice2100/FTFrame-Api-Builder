using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using mshtml;
using System.Windows.Forms;
using FTDPClient.database;
using Microsoft.Data.Sqlite;
using FTDP.Util.Model;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;

namespace FTDP.Util
{
    public class BugCheck
    {
        public static object[] BugTypes
        {
            get
            {
                return new object[]
                {
                    strItems[0],strItems[1],strItems[2],strItems[3],strItems[4],strItems[5]
                };
            }
        }
        public static Dictionary<string, List<string>> TableColumnDic = null;
        private static string[] strItems = null;
        public static string[] StrItems
        {
            get { return strItems; }
            set { strItems = value; }
        }

        public static List<Bug> Init(bool IsAllSite, ArrayList SelFiles, string SinglePageID, string LocalConfigConnection, string LocalSiteConnection, string RemoteConnection, string AppPath, string SiteID)
        {
            var al = new List<Bug>();
            DB SiteDB = new DB();
            SiteDB.Open(LocalSiteConnection);
            try
            {
                if (IsAllSite)
                {
                    string sql = "select id from pages";
                    SqliteDataReader dr = SiteDB.OpenRecord(sql);
                    while (dr.Read())
                    {
                        string pageid = dr.GetString(0);
                        string pagepath = getPathByPageId(pageid, LocalSiteConnection);
                        var bugpage = BugPage(pageid, pagepath, AppPath, SiteID, LocalSiteConnection);
                        foreach (var item in bugpage)
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
                        var bugpage = BugPage(pageid, pagepath, AppPath, SiteID, LocalSiteConnection);
                        foreach (var item in bugpage)
                        {
                            al.Add(item);
                        }
                    }
                }
                else if (SinglePageID != null)
                {
                    string pagepath = getPathByPageId(SinglePageID, LocalSiteConnection);
                    var bugpage = BugPage(SinglePageID, pagepath, AppPath, SiteID, LocalSiteConnection);
                    foreach (var item in bugpage)
                    {
                        al.Add(item);
                    }
                }
            }
            finally
            {
                SiteDB.Close();
            }
            return al;
        }
        public static List<Bug> BugPage(string pageid, string pagepath, string apppath, string siteid, string LocalSiteConnection, string RemoteDBType = null, string RemoteConnection = null)
        {
            string pageurl = apppath + "\\sites\\" + siteid + "\\" + pagepath.Replace("/", "\\");
            string sql = "select caption from pages where id='" + Tool.D2DD(pageid) + "'";
            string caption = DB.GetStringValue(sql, LocalSiteConnection);
            var al = new List<Bug>();
            #region HTML元素标识
            //重名id
            HTMLDocumentClass hc = new HTMLDocumentClass();
            IHTMLDocument2 doc2 = hc;
            doc2.write("");
            doc2.close();
            IHTMLDocument4 doc4 = hc;
            IHTMLDocument2 doc = doc4.createDocumentFromUrl(pageurl, "null");
            while (doc.readyState != "complete")
            {
                Application.DoEvents();
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
                    else if (!id.StartsWith("ftform_"))
                    {
                        al.Add(new Bug()
                        {
                            LeiXing = strItems[1],
                            Path = pagepath,
                            PageCaption = caption,
                            ControlCaption="",
                            BugDesc = "id\"" + id + "\"" + strItems[12] + "：" + ele.tagName,
                            Level = strItems[6],
                            OpenFile = strItems[11],
                            PageID = pageid,
                        });
                    }
                }
            }
            doc.close();
            #endregion
            //找出所有使用的组件
            sql = "select b.name partname,c.name controlname,b.id partid,b.partxml,c.caption controlcaption from part_in_page a,parts b,controls c where a.partid=b.id and b.controlid=c.id and a.pageid='" + pageid + "'";
            var partList = new List<(string partname, string controlname, string partid, string partxml,string controlcaption)>();
            using (DB db = new DB())
            {
                db.Open(LocalSiteConnection);
                using (DR dr = new DR(db.OpenRecord(sql)))
                {
                    while (dr.Read())
                    {
                        partList.Add((dr.getString(0), dr.getString(1), dr.getString(2), dr.getString(3), dr.getString(4)));
                    }
                }
            }
            #region 数据列表设置
            foreach (var partItem in partList.Where(r => r.controlname == "list" && r.partname == "List"))
            {
                string MainTable = getPartNodeValue(partItem.partxml, "MainTable");
                if (string.IsNullOrWhiteSpace(MainTable))
                {
                    al.Add(new Bug()
                    {
                        LeiXing = strItems[2],
                        Path = pagepath,
                        PageCaption = caption,
                        ControlCaption = partItem.controlcaption,
                        BugDesc = "Main Table Not Set",
                        Level = strItems[7],
                        OpenFile = strItems[11],
                        PageID = pageid,
                    });
                }
                string CusSQL = getPartNodeValue(partItem.partxml, "CusSQL");
                if (string.IsNullOrWhiteSpace(CusSQL))
                {
                    al.Add(new Bug()
                    {
                        LeiXing = strItems[2],
                        Path = pagepath,
                        PageCaption = caption,
                        ControlCaption = partItem.controlcaption,
                        BugDesc = "Custom SQL Is Empty",
                        Level = strItems[8],
                        OpenFile = strItems[11],
                        PageID = pageid,
                    });
                }
                string OrderBy = getPartNodeValue(partItem.partxml, "OrderBy");
                if (string.IsNullOrWhiteSpace(OrderBy))
                {
                    al.Add(new Bug()
                    {
                        LeiXing = strItems[2],
                        Path = pagepath,
                        PageCaption = caption,
                        ControlCaption = partItem.controlcaption,
                        BugDesc = "OrderBy Sub SQL Is Empty",
                        Level = strItems[7],
                        OpenFile = strItems[11],
                        PageID = pageid,
                    });
                }
                var columns = sqlColumns(RemoteDBType, RemoteConnection, MainTable, CusSQL, OrderBy);
                if (columns.exception != null)
                {
                    al.Add(new Bug()
                    {
                        LeiXing = strItems[2],
                        Path = pagepath,
                        PageCaption = caption,
                        ControlCaption = partItem.controlcaption,
                        BugDesc = "Query SQL Exception:" + columns.exception,
                        Level = strItems[9],
                        OpenFile = strItems[11],
                        PageID = pageid,
                    });
                }
                string SearchDefine = getPartNodeValue(partItem.partxml, "SearchDefine");
                string notInCols = "";
                foreach (var col in SearchDefine.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim().ToLower()))
                {
                    if (columns.Columns.Count > 0 && !columns.Columns.Contains(col)) notInCols += col + ";";
                }
                if (notInCols != "")
                {
                    al.Add(new Bug()
                    {
                        LeiXing = strItems[2],
                        Path = pagepath,
                        PageCaption = caption,
                        ControlCaption = partItem.controlcaption,
                        BugDesc = "Search Column Not in SQL:" + notInCols,
                        Level = strItems[9],
                        OpenFile = strItems[11],
                        PageID = pageid,
                    });
                }
                string RowAll = getPartNodeValue(partItem.partxml, "RowAll");
                string[] items = RowAll.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                List<string[]> rowAll = new List<string[]>();
                foreach (string item in items)
                {
                    if (item != null && item.Trim().IndexOf("&&&") >= 0)
                    {
                        string _item = item.Trim();
                        string openclose = _item.Substring(_item.IndexOf("&&&") + 3);
                        string[] colcfg = _item.Substring(0, _item.IndexOf("&&&")).Split('#');
                        rowAll.Add(new string[] { colcfg[0], getDecode(colcfg[1]), colcfg[2], colcfg[3], colcfg[4], colcfg[5], colcfg.Length < 7 ? "" : colcfg[6], openclose, colcfg.Length < 8 ? "0" : colcfg[7] });
                    }
                }
                if (rowAll.Count == 0)
                {
                    al.Add(new Bug()
                    {
                        LeiXing = strItems[2],
                        Path = pagepath,
                        PageCaption = caption,
                        ControlCaption = partItem.controlcaption,
                        BugDesc = "Row Define Is Empty",
                        Level = strItems[7],
                        OpenFile = strItems[11],
                        PageID = pageid,
                    });
                }
                notInCols = "";
                foreach (var item in rowAll)
                {
                    string shuju = item[1].ToLower().Trim();
                    if (shuju != "" && !shuju.StartsWith("@") && !shuju.StartsWith("!") && !shuju.StartsWith("$") && shuju.IndexOf(';') < 0)
                    {
                        if (columns.Columns.Count > 0 && !columns.Columns.Contains(shuju)) notInCols += shuju + ";";
                    }
                }
                if (notInCols != "")
                {
                    al.Add(new Bug()
                    {
                        LeiXing = strItems[2],
                        Path = pagepath,
                        PageCaption = caption,
                        ControlCaption = partItem.controlcaption,
                        BugDesc = "Data Not in SQL:" + notInCols,
                        Level = strItems[7],
                        OpenFile = strItems[11],
                        PageID = pageid,
                    });
                }
            }
            #endregion
            #region 数据操作设置
            foreach (var partItem in partList.Where(r => r.controlname == "dataop" && r.partname == "Interface"))
            {
                string Define = getPartNodeValue(partItem.partxml, "Define");
                string[] items = Define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                List<string[]> rowAll = new List<string[]>();
                foreach (string item in items)
                {
                    string[] row = item.Split(new string[] { "##" }, StringSplitOptions.None);
                    rowAll.Add(new string[]{
                    row[0],//tip
                    row[1],//name
                    getDecode(row[2]),//binddata
                    getDecode(row[3]),//0 Add 1 Mod
                    row[4],//check
                    row[5],//keyval
                    row[6],//id
                    row.Length<8?"":getDecode(row[7]),//adv
                    "Delete"
                    });
                    if (row.Length >= 9)
                    {
                        //多行的Json设置暂时不考虑
                    }
                }
                if (rowAll.Count == 0)
                {
                    al.Add(new Bug()
                    {
                        LeiXing = strItems[3],
                        Path = pagepath,
                        PageCaption = caption,
                        ControlCaption = partItem.controlcaption,
                        BugDesc = "Row Define Is Empty",
                        Level = strItems[7],
                        OpenFile = strItems[11],
                        PageID = pageid,
                    });
                }
                if (rowAll.Where(r => r[3] == "0").Count() > 0 && rowAll.Where(r => r[3] == "1").Count() > 0)
                {
                    al.Add(new Bug()
                    {
                        LeiXing = strItems[3],
                        Path = pagepath,
                        PageCaption = caption,
                        ControlCaption = partItem.controlcaption,
                        BugDesc = "Add and Mod Mixed",
                        Level = strItems[9],
                        OpenFile = strItems[11],
                        PageID = pageid,
                    });
                }
                string validateAlert = "";
                foreach (var item in rowAll)
                {
                    if (!string.IsNullOrWhiteSpace(item[4]) && string.IsNullOrWhiteSpace(item[0]))
                    {
                        validateAlert += item[1] + ",";
                    }
                }
                if (validateAlert != "")
                {
                    al.Add(new Bug()
                    {
                        LeiXing = strItems[3],
                        Path = pagepath,
                        PageCaption = caption,
                        ControlCaption = partItem.controlcaption,
                        BugDesc = "No Caption When Validate Set:" + validateAlert,
                        Level = strItems[7],
                        OpenFile = strItems[11],
                        PageID = pageid,
                    });
                }
                string dataBindError = "";
                foreach (var item in rowAll)
                {
                    if (!string.IsNullOrWhiteSpace(item[2]) && item[2].StartsWith("@"))
                    {
                        if (item[2].IndexOf('.') < 0)
                        {
                            if (!TableExist(item[2].Substring(1), RemoteDBType, RemoteConnection)) dataBindError += item[2] + ",";
                        }
                        else
                        {
                            string tablename = item[2].Substring(1).Split('.')[0];
                            string columnname = item[2].Substring(1).Split('.')[1];
                            if (!ColumnExist(tablename, columnname, RemoteDBType, RemoteConnection)) dataBindError += columnname + ",";
                        }
                    }
                }
                if (dataBindError != "")
                {
                    al.Add(new Bug()
                    {
                        LeiXing = strItems[3],
                        Path = pagepath,
                        PageCaption = caption,
                        ControlCaption = partItem.controlcaption,
                        BugDesc = "DataBind Error:" + dataBindError,
                        Level = strItems[7],
                        OpenFile = strItems[11],
                        PageID = pageid,
                    });
                }
                string APISet = getPartNodeValue(partItem.partxml, "APISet");
                items = APISet.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                List<string[]> apiList = new List<string[]>();
                foreach (string item in items)
                {
                    string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                    if (colcfg.Length == 5)
                    {
                        var list = new List<string>();
                        list.AddRange(colcfg);
                        list.Add("form");
                        colcfg = list.ToArray();
                    }
                    apiList.Add(colcfg);
                }
                foreach (string[] item in apiList)
                {
                    string apiName = item[0];
                    string apiNameError = "";
                    var names = item[3].Split(new string[] { "[#]" }, StringSplitOptions.None);
                    foreach (var name in names)
                    {
                        if (rowAll.Where(r => r[1] == name).Count() == 0)
                        {
                            apiNameError += name + ",";
                        }
                    }
                    if (apiNameError != "")
                    {
                        al.Add(new Bug()
                        {
                            LeiXing = strItems[3],
                            Path = pagepath,
                            PageCaption = caption,
                            ControlCaption = partItem.controlcaption,
                            BugDesc = apiName + " Name Not Match:" + apiNameError,
                            Level = strItems[7],
                            OpenFile = strItems[11],
                            PageID = pageid,
                        });
                    }
                }
            }
            #endregion
            #region 数据获取设置
            foreach (var partItem in partList.Where(r => r.controlname == "dyvalue" && r.partname == "Interface"))
            {
                string Define = getPartNodeValue(partItem.partxml, "Define");
                List<string[]> rowAll = new List<string[]>();
                string[] items = Define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in items)
                {
                    string[] row = item.Split(new string[] { "##" }, StringSplitOptions.None);
                    rowAll.Add(new string[]{
                    row[0],//caption
                    row[1],//id
                    getDecode(row[2]),//binddata
                    row[3],
                    row[4],
                    row[5],
                    row[6],
                    getDecode(row[7]),
                    "Delete"
                    });
                }
                if (rowAll.Count == 0)
                {
                    al.Add(new Bug()
                    {
                        LeiXing = strItems[4],
                        Path = pagepath,
                        PageCaption = caption,
                        ControlCaption = partItem.controlcaption,
                        BugDesc = "Row Define Is Empty",
                        Level = strItems[7],
                        OpenFile = strItems[11],
                        PageID = pageid,
                    });
                }
                string dataBindError = "";
                foreach (var item in rowAll)
                {
                    if (!string.IsNullOrWhiteSpace(item[2]) && item[2].StartsWith("@"))
                    {
                        if (item[2].IndexOf('.') < 0)
                        {
                            if (!TableExist(item[2].Substring(1), RemoteDBType, RemoteConnection)) dataBindError += item[2] + ",";
                        }
                        else
                        {
                            string tablename = item[2].Substring(1).Split('.')[0];
                            string columnname = item[2].Substring(1).Split('.')[1];
                            if (!ColumnExist(tablename, columnname, RemoteDBType, RemoteConnection)) dataBindError += columnname + ",";
                        }
                    }
                }
                if (dataBindError != "")
                {
                    al.Add(new Bug()
                    {
                        LeiXing = strItems[4],
                        Path = pagepath,
                        PageCaption = caption,
                        ControlCaption = partItem.controlcaption,
                        BugDesc = "DataBind Error:" + dataBindError,
                        Level = strItems[7],
                        OpenFile = strItems[11],
                        PageID = pageid,
                    });
                }
                string APISet = getPartNodeValue(partItem.partxml, "APISet");
                items = APISet.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                List<string[]> apiList = new List<string[]>();
                foreach (string item in items)
                {
                    string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                    apiList.Add(colcfg);
                }
                foreach (string[] item in apiList)
                {
                    string apiName = item[0];
                    string apiNameError = "";
                    var names = item[3].Split(new string[] { "[#]" }, StringSplitOptions.None);
                    foreach (var name in names)
                    {
                        if (rowAll.Where(r => r[1] == name).Count() == 0)
                        {
                            apiNameError += name + ",";
                        }
                    }
                    if (apiNameError != "")
                    {
                        al.Add(new Bug()
                        {
                            LeiXing = strItems[4],
                            Path = pagepath,
                            PageCaption = caption,
                            ControlCaption = partItem.controlcaption,
                            BugDesc = apiName + " ID Not Match:" + apiNameError,
                            Level = strItems[7],
                            OpenFile = strItems[11],
                            PageID = pageid,
                        });
                    }
                }
            }
            #endregion
            //其他
            return al;
        }
        public static string getPathByPageId(string pageid, string LocalSiteConnection)
        {
            string sql = "select name from pages where id='" + Tool.D2DD(pageid) + "'";
            string path = DB.GetStringValue(sql, LocalSiteConnection);
            if (path == null) return null;
            sql = "select pid from pages where id='" + Tool.D2DD(pageid) + "'";
            string pid = DB.GetStringValue(sql, LocalSiteConnection);
            if (pid == null) return null;
            while (!pid.Equals("root"))
            {
                sql = "select name from directory where id='" + Tool.D2DD(pid) + "'";
                string dir = DB.GetStringValue(sql, LocalSiteConnection);
                path = dir + "/" + path;
                sql = "select pid from directory where id='" + Tool.D2DD(pid) + "'";
                pid = DB.GetStringValue(sql, LocalSiteConnection);
                if (pid == null) return null;
            }
            return path;
        }
        public static string getPartNodeValue(string partxml, string nodeName)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(partxml);
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
        static string codePattern(string oriInput)
        {
            if (oriInput == null) return null;
            Regex r = new Regex(@"@code\([^(\)@)]*\)");
            MatchCollection mc = r.Matches(oriInput);
            foreach (Match m in mc)
            {
                string pattern = m.Value.Substring(0, m.Value.Length - 1);
                oriInput = oriInput.Replace(m.Value, "");
            }
            r = new Regex(@"@enum\([^(\)@)]*\)");
            mc = r.Matches(oriInput);
            foreach (Match m in mc)
            {
                string pattern = m.Value.Substring(0, m.Value.Length - 1);
                oriInput = oriInput.Replace(m.Value, "");
            }
            return oriInput;
        }
        public static (List<string> Columns, string exception) sqlColumns(string dbtype, string connstr, string maintable, string cussql, string orderby)
        {
            try
            {
                List<string> list = new List<string>();
                string baseSql = "";
                if (string.IsNullOrWhiteSpace(cussql))
                {
                    baseSql = "select * from " + maintable.Substring(1);
                }
                else
                {
                    cussql = codePattern(cussql);
                    baseSql = GetSqlForRemoveSameCols(dbtype, connstr, cussql);
                }
                if (dbtype.ToLower() == "sqlserver")
                {
                    using (SqlConnection conn = new SqlConnection(connstr))
                    {
                        conn.Open();
                        string sql = "select top 0 * from (" + baseSql + ") t1 " + orderby;
                        using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                        {
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                list.Add(dr.GetName(i).ToLower());
                            }
                        }
                    }
                }
                else if (dbtype.ToLower() == "mysql")
                {
                    using (MySqlConnection conn = new MySqlConnection(connstr))
                    {
                        conn.Open();
                        string sql = baseSql + " " + orderby + " limit 1";
                        using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                        {
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                list.Add(dr.GetName(i).ToLower());
                            }
                        }
                    }
                }
                else if (dbtype.ToLower() == "sqlite")
                {
                    using (var conn = new DB(connstr))
                    {
                        conn.Open();
                        string sql = baseSql + " " + orderby + " limit 1";
                        using (var dr = conn.OpenRecord(sql))
                        {
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                list.Add(dr.GetName(i).ToLower());
                            }
                        }
                    }
                }
                return (list, null);
            }
            catch (Exception ex)
            {
                return (new List<string>(), ex.Message);
            }
        }
        public static string getDecode(string str)
        {
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "^#@$FVSD#$%SDF@#maobb234efwe";
            String sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);
            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength);
            }

            byte[] kkkk = ASCIIEncoding.ASCII.GetBytes(sTemp);
            sTemp = "Dmaobbasfui23497#$ASasdkf0";
            mobjCryptoService.GenerateIV();
            bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength);
            }
            byte[] vvvv = ASCIIEncoding.ASCII.GetBytes(sTemp);

            byte[] bytIn = Convert.FromBase64String(str);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = kkkk;
            mobjCryptoService.IV = vvvv;
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
        private static bool TableExist(string tablename, string dbtype, string connstr)
        {
            if (TableColumnDic.Count == 0)
            {
                if (dbtype.ToLower() == "sqlserver")
                {
                    using (SqlConnection conn = new SqlConnection(connstr))
                    {
                        conn.Open();
                        string sql = "select * from (select name from sys.tables where type='U' union SELECT Name FROM sys.sql_modules AS m  INNER JOIN sys.all_objects AS o ON m.object_id = o.object_id WHERE o.[type] = 'v') t1";
                        using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                TableColumnDic.Add(dr.GetString(0).ToLower(), null);
                            }
                        }
                    }
                }
                else if (dbtype.ToLower() == "mysql")
                {
                    using (MySqlConnection conn = new MySqlConnection(connstr))
                    {
                        conn.Open();
                        string sql = "show tables";
                        using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                TableColumnDic.Add(dr.GetString(0).ToLower(), null);
                            }
                        }
                    }
                }
                else if (dbtype.ToLower() == "sqlite")
                {
                    using (var conn = new DB(connstr))
                    {
                        conn.Open();
                        string sql = "SELECT name,'' note FROM sqlite_master as t1 WHERE type='table' order by name";
                        using (var dr = conn.OpenRecord(sql))
                        {
                            while (dr.Read())
                            {
                                TableColumnDic.Add(dr.GetString(0).ToLower(), null);
                            }
                        }
                    }
                }
            }
            return TableColumnDic.ContainsKey(tablename.ToLower());
        }
        private static bool ColumnExist(string tablename, string colname, string dbtype, string connstr)
        {
            if (!TableExist(tablename, dbtype, connstr)) return false;
            if (TableColumnDic[tablename.ToLower()] == null)
            {
                TableColumnDic[tablename.ToLower()] = new List<string>();
                if (dbtype.ToLower() == "sqlserver")
                {
                    using (SqlConnection conn = new SqlConnection(connstr))
                    {
                        conn.Open();
                        bool isView = false;
                        string sql = "select name,xtype from sysobjects where name<>'dtproperties' and (xtype='U' or xtype='V') and name='" + tablename + "'";
                        using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                isView = dr.GetString(1).Trim().ToLower() == "v";
                            }
                        }
                        if (isView)
                        {
                            sql = @"(select sys.columns.name,sys.types.name as typename,sys.columns.max_length,sys.columns.precision,sys.columns.scale, sys.columns.is_nullable,
 (select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as is_identity ,
 sys.columns.column_id,(select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as description 
 from sys.columns, sys.views, sys.types where sys.columns.object_id = sys.views.object_id and sys.columns.user_type_id=sys.types.user_type_id and sys.views.name='" + tablename + "')";
                        }
                        else
                        {
                            sql = @"(select sys.columns.name,sys.types.name as typename,sys.columns.max_length,sys.columns.precision,sys.columns.scale, sys.columns.is_nullable,
 (select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as is_identity ,
 sys.columns.column_id,(select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as description 
 from sys.columns, sys.tables, sys.types where sys.columns.object_id = sys.tables.object_id and sys.columns.user_type_id=sys.types.user_type_id and sys.tables.name='" + tablename + "')";
                        }
                        using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                TableColumnDic[tablename.ToLower()].Add(dr.GetString(0).ToLower());
                            }
                        }
                    }
                }
                else if (dbtype.ToLower() == "mysql")
                {
                    using (MySqlConnection conn = new MySqlConnection(connstr))
                    {
                        conn.Open();
                        string sql = "show full fields from " + tablename;
                        using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                TableColumnDic[tablename.ToLower()].Add(dr.GetString(0).ToLower());
                            }
                        }
                    }
                }
                else if (dbtype.ToLower() == "sqlite")
                {
                    using (var conn = new DB(connstr))
                    {
                        conn.Open();
                        string sql = "PRAGMA TABLE_INFO(" + tablename + ")";
                        using (var dr = conn.OpenRecord(sql))
                        {
                            while (dr.Read())
                            {
                                TableColumnDic[tablename.ToLower()].Add(dr.GetString(1).ToLower());
                            }
                        }
                    }
                }
            }
            return TableColumnDic[tablename.ToLower()].Contains(colname.ToLower());
        }
        private static string GetSqlForRemoveSameCols(string dbtype, string connstr, string oriSql)
        {
            //@*@
            if (oriSql.IndexOf("@*@") < 0) return oriSql;
            string _oriSql = oriSql.Replace("@*@", "*");
            StringBuilder selCols = new StringBuilder(100);
            if (dbtype.ToLower() == "sqlserver")
            {
                string sql = "select top 0 * from (" + _oriSql + ") t123456";
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                    {
                        Dictionary<string, int> dic = new Dictionary<string, int>();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            string key = dr.GetName(i).ToLower();
                            if (!dic.ContainsKey(key)) dic.Add(key, 1);
                            else dic[key] += 1;
                        }
                        var dicList = dic.Where(r => r.Value == 1).Select(r => r.Key).ToList();
                        for (int i = 0; i < dicList.Count; i++)
                        {
                            if (i > 0) selCols.Append(",");
                            selCols.Append(dicList[i]);
                        }
                    }
                }
            }
            else if (dbtype.ToLower() == "mysql")
            {
                string sql = _oriSql + " limit 0";
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                    {
                        Dictionary<string, int> dic = new Dictionary<string, int>();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            string key = dr.GetName(i).ToLower();
                            if (!dic.ContainsKey(key)) dic.Add(key, 1);
                            else dic[key] += 1;
                        }
                        var dicList = dic.Where(r => r.Value == 1).Select(r => r.Key).ToList();
                        for (int i = 0; i < dicList.Count; i++)
                        {
                            if (i > 0) selCols.Append(",");
                            selCols.Append(dicList[i]);
                        }
                    }
                }
            }
            else if (dbtype.ToLower() == "sqlite")
            {
                string sql = _oriSql + " limit 0";
                using (var conn = new DB(connstr))
                {
                    conn.Open();
                    using (var dr = conn.OpenRecord(sql))
                    {
                        Dictionary<string, int> dic = new Dictionary<string, int>();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            string key = dr.GetName(i).ToLower();
                            if (!dic.ContainsKey(key)) dic.Add(key, 1);
                            else dic[key] += 1;
                        }
                        var dicList = dic.Where(r => r.Value == 1).Select(r => r.Key).ToList();
                        for (int i = 0; i < dicList.Count; i++)
                        {
                            if (i > 0) selCols.Append(",");
                            selCols.Append(dicList[i]);
                        }
                    }
                }
            }
            return oriSql.Replace("@*@", selCols.ToString());
        }
    }
}
