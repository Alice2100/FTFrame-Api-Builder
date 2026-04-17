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
using System.Xml;

namespace FTDP.Util
{
    public class ConfigSearch
    {
        private static string[] strItems = null;
        public static string[] StrItems
        {
            get { return strItems; }
            set { strItems = value; }
        }
        public static List<Find> FindPage(string searchText,string pageid, string pagepath, string apppath, string siteid, string LocalSiteConnection, string RemoteDBType = null, string RemoteConnection = null)
        {
            string pageurl = apppath + "\\sites\\" + siteid + "\\" + pagepath.Replace("/", "\\");
            string sql = "select caption from pages where id='" + Tool.D2DD(pageid) + "'";
            string caption = DB.GetStringValue(sql, LocalSiteConnection);
            var al = new List<Find>();
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
                XmlDocument doc= new XmlDocument();
                doc.LoadXml(partItem.partxml);
                var nodes = doc.SelectNodes("//partxml/param");
                foreach(XmlNode node in nodes)
                {
                    string name = node.SelectSingleNode("name")?.InnerText ?? "";
                    string value = node.SelectSingleNode("value")?.InnerText ?? "";
                    if (name == "RowAll")
                    {
                        string RowAll = value;
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
                        foreach(var strItem in rowAll)
                        {
                            if(NameEqualsForIsHtml(searchText, strItem[0]))//key匹配
                            {
                                al.Add(new Find()
                                {
                                    LeiXing = strItems[0],
                                    Position = "RowAll-" + strItem[0],
                                    Path = pagepath,
                                    PageCaption = caption,
                                    ControlCaption = partItem.controlcaption,
                                    Desc = "Data-"+ strItem[0],
                                    OpenFile = strItems[3],
                                    PageID = pageid,
                                    SetKey = strItem[0],
                                    SetValue = strItem[1],
                                    PartObj = partItem,
                                });
                            }
                            foreach(var str in strItem)
                            {
                                if (string.IsNullOrWhiteSpace(str)) continue;
                                int index = str.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase);
                                if (index >= 0)
                                {
                                    int leftIndex = (index - 10) < 0 ? 0 : (index - 10);
                                    int rightIndex = (index + 10) > (str.Length - 1) ? (str.Length - 1) : (index + 10);
                                    string desc = str.Substring(leftIndex, rightIndex - leftIndex + 1);
                                    al.Add(new Find()
                                    {
                                        LeiXing = strItems[0],
                                        Position = "RowAll-"+ strItem[0],
                                        Path = pagepath,
                                        PageCaption = caption,
                                        ControlCaption = partItem.controlcaption,
                                        Desc = desc,
                                        OpenFile = strItems[3],
                                        PageID = pageid,
                                    });
                                }
                            }
                        }
                    }
                    else if (name == "APISet")
                    {
                        string setstr = value;
                        string[] items = setstr.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                        List<string[]> apiAll = new List<string[]>();
                        foreach (string item in items)
                        {
                            string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                            if (colcfg.Length == 5)
                            {
                                var list = new List<string>();
                                list.AddRange(colcfg);
                                list.Add("json");
                                list.Add("");//keyvalue [::] [;;]
                                colcfg = list.ToArray();
                            }
                            else if (colcfg.Length == 6)
                            {
                                var list = new List<string>();
                                list.AddRange(colcfg);
                                list.Add("");//keyvalue [::] [;;]
                                colcfg = list.ToArray();
                            }
                            apiAll.Add(colcfg);
                        }
                        foreach (var strItem in apiAll)
                        {
                            foreach (var str in strItem)
                            {
                                if (string.IsNullOrWhiteSpace(str)) continue;
                                int index = str.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase);
                                if (index >= 0)
                                {
                                    int leftIndex = (index - 10) < 0 ? 0 : (index - 10);
                                    int rightIndex = (index + 10) > (str.Length - 1) ? (str.Length - 1) : (index + 10);
                                    string desc = str.Substring(leftIndex, rightIndex - leftIndex+1);
                                    al.Add(new Find()
                                    {
                                        LeiXing = strItems[0],
                                        Position = "APISet-" + strItem[0],
                                        Path = pagepath,
                                        PageCaption = caption,
                                        ControlCaption = partItem.controlcaption,
                                        Desc = desc,
                                        OpenFile = strItems[3],
                                        PageID = pageid,
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        int index = value.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase);
                        if(index>=0)
                        {
                            int leftIndex = (index - 10) < 0 ? 0 : (index - 10);
                            int rightIndex = (index + 10) > (value.Length-1) ? (value.Length - 1) : (index + 10);
                            string desc=value.Substring(leftIndex, rightIndex- leftIndex + 1);
                            al.Add(new Find()
                            {
                                LeiXing = strItems[0],
                                Position = name,
                                Path = pagepath,
                                PageCaption = caption,
                                ControlCaption = partItem.controlcaption,
                                Desc = desc,
                                OpenFile = strItems[3],
                                PageID = pageid,
                            });
                        }
                    }
                }
                doc = null;
            }
            #endregion
            #region 数据操作设置
            foreach (var partItem in partList.Where(r => r.controlname == "dataop" && r.partname == "Interface"))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(partItem.partxml);
                var nodes = doc.SelectNodes("//partxml/param");
                foreach (XmlNode node in nodes)
                {
                    string name = node.SelectSingleNode("name")?.InnerText ?? "";
                    string value = node.SelectSingleNode("value")?.InnerText ?? "";
                    if (name == "Define")
                    {
                        string Define = value;
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
                            row.Length<8?"":getDecode(row[7])//adv
                            });
                            if (row.Length >= 9)
                            {
                                //多行的Json设置暂时不考虑
                            }
                        }
                        foreach (var strItem in rowAll)
                        {
                            if (NameEqualsForIsHtml(searchText, strItem[1]))//key匹配
                            {
                                al.Add(new Find()
                                {
                                    LeiXing = strItems[1],
                                    Position = "Define-" + strItem[0] + "-" + strItem[1],
                                    Path = pagepath,
                                    PageCaption = caption,
                                    ControlCaption = partItem.controlcaption,
                                    Desc = "Adv-"+ strItem[1],
                                    OpenFile = strItems[3],
                                    PageID = pageid,
                                    SetKey = strItem[1],
                                    SetValue = strItem[7],
                                    PartObj = partItem,
                                });
                            }
                            foreach (var str in strItem)
                            {
                                if (string.IsNullOrWhiteSpace(str)) continue;
                                int index = str.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase);
                                if (index >= 0)
                                {
                                    int leftIndex = (index - 10) < 0 ? 0 : (index - 10);
                                    int rightIndex = (index + 10) > (str.Length - 1) ? (str.Length - 1) : (index + 10);
                                    string desc = str.Substring(leftIndex, rightIndex - leftIndex + 1);
                                    al.Add(new Find()
                                    {
                                        LeiXing = strItems[1],
                                        Position = "Define-" + strItem[0]+ "-" + strItem[1],
                                        Path = pagepath,
                                        PageCaption = caption,
                                        ControlCaption = partItem.controlcaption,
                                        Desc = desc,
                                        OpenFile = strItems[3],
                                        PageID = pageid,
                                    });
                                }
                            }
                        }
                    }
                    else if (name == "APISet")
                    {
                        string setstr = value;
                        string[] items = setstr.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                        List<string[]> apiAll = new List<string[]>();
                        foreach (string item in items)
                        {
                            string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                            if (colcfg.Length == 5)
                            {
                                var list = new List<string>();
                                list.AddRange(colcfg);
                                colcfg = list.ToArray();
                            }
                            apiAll.Add(colcfg);
                        }
                        foreach (var strItem in apiAll)
                        {
                            foreach (var str in strItem)
                            {
                                if (string.IsNullOrWhiteSpace(str)) continue;
                                int index = str.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase);
                                if (index >= 0)
                                {
                                    int leftIndex = (index - 10) < 0 ? 0 : (index - 10);
                                    int rightIndex = (index + 10) > (str.Length - 1) ? (str.Length - 1) : (index + 10);
                                    string desc = str.Substring(leftIndex, rightIndex - leftIndex + 1);
                                    al.Add(new Find()
                                    {
                                        LeiXing = strItems[1],
                                        Position = "APISet-" + strItem[0],
                                        Path = pagepath,
                                        PageCaption = caption,
                                        ControlCaption = partItem.controlcaption,
                                        Desc = desc,
                                        OpenFile = strItems[3],
                                        PageID = pageid,
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        int index = value.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase);
                        if (index >= 0)
                        {
                            int leftIndex = (index - 10) < 0 ? 0 : (index - 10);
                            int rightIndex = (index + 10) > (value.Length - 1) ? (value.Length - 1) : (index + 10);
                            string desc = value.Substring(leftIndex, rightIndex - leftIndex + 1);
                            al.Add(new Find()
                            {
                                LeiXing = strItems[1],
                                Position = name,
                                Path = pagepath,
                                PageCaption = caption,
                                ControlCaption = partItem.controlcaption,
                                Desc = desc,
                                OpenFile = strItems[3],
                                PageID = pageid,
                            });
                        }
                    }
                }
                doc = null;
            }
            #endregion
            #region 数据获取设置
            foreach (var partItem in partList.Where(r => r.controlname == "dyvalue" && r.partname == "Interface"))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(partItem.partxml);
                var nodes = doc.SelectNodes("//partxml/param");
                foreach (XmlNode node in nodes)
                {
                    string name = node.SelectSingleNode("name")?.InnerText ?? "";
                    string value = node.SelectSingleNode("value")?.InnerText ?? "";
                    if (name == "Define")
                    {
                        string Define = value;
                        string[] items = Define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                        List<string[]> rowAll = new List<string[]>();
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
                            getDecode(row[7])
                            });
                        }
                        foreach (var strItem in rowAll)
                        {
                            if (NameEqualsForIsHtml(searchText, strItem[1]))//key匹配
                            {
                                al.Add(new Find()
                                {
                                    LeiXing = strItems[2],
                                    Position = "Define-" + strItem[0] + "-" + strItem[1],
                                    Path = pagepath,
                                    PageCaption = caption,
                                    ControlCaption = partItem.controlcaption,
                                    Desc = "Adv-"+ strItem[1],
                                    OpenFile = strItems[3],
                                    PageID = pageid,
                                    SetKey  = strItem[1],
                                    SetValue = strItem[7],
                                    PartObj = partItem,
                                });
                            }
                            foreach (var str in strItem)
                            {
                                if (string.IsNullOrWhiteSpace(str)) continue;
                                int index = str.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase);
                                if (index >= 0)
                                {
                                    int leftIndex = (index - 10) < 0 ? 0 : (index - 10);
                                    int rightIndex = (index + 10) > (str.Length - 1) ? (str.Length - 1) : (index + 10);
                                    string desc = str.Substring(leftIndex, rightIndex - leftIndex + 1);
                                    al.Add(new Find()
                                    {
                                        LeiXing = strItems[2],
                                        Position = "Define-" + strItem[0] + "-" + strItem[1],
                                        Path = pagepath,
                                        PageCaption = caption,
                                        ControlCaption = partItem.controlcaption,
                                        Desc = desc,
                                        OpenFile = strItems[3],
                                        PageID = pageid,
                                    });
                                }
                            }
                        }
                    }
                    else if (name == "APISet")
                    {
                        string setstr = value;
                        string[] items = setstr.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                        List<string[]> apiAll = new List<string[]>();
                        foreach (string item in items)
                        {
                            string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                            apiAll.Add(colcfg);
                        }
                        foreach (var strItem in apiAll)
                        {
                            foreach (var str in strItem)
                            {
                                if (string.IsNullOrWhiteSpace(str)) continue;
                                int index = str.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase);
                                if (index >= 0)
                                {
                                    int leftIndex = (index - 10) < 0 ? 0 : (index - 10);
                                    int rightIndex = (index + 10) > (str.Length - 1) ? (str.Length - 1) : (index + 10);
                                    string desc = str.Substring(leftIndex, rightIndex - leftIndex + 1);
                                    al.Add(new Find()
                                    {
                                        LeiXing = strItems[2],
                                        Position = "APISet-" + strItem[0],
                                        Path = pagepath,
                                        PageCaption = caption,
                                        ControlCaption = partItem.controlcaption,
                                        Desc = desc,
                                        OpenFile = strItems[3],
                                        PageID = pageid,
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        int index = value.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase);
                        if (index >= 0)
                        {
                            int leftIndex = (index - 10) < 0 ? 0 : (index - 10);
                            int rightIndex = (index + 10) > (value.Length - 1) ? (value.Length - 1) : (index + 10);
                            string desc = value.Substring(leftIndex, rightIndex - leftIndex + 1);
                            al.Add(new Find()
                            {
                                LeiXing = strItems[2],
                                Position = name,
                                Path = pagepath,
                                PageCaption = caption,
                                ControlCaption = partItem.controlcaption,
                                Desc = desc,
                                OpenFile = strItems[3],
                                PageID = pageid,
                            });
                        }
                    }
                }
                doc = null;
            }
            #endregion
            //其他
            return al;
        }
        
        private static bool NameEqualsForIsHtml(string s1,string s2)
        {
            s1= s1.Trim().ToLower();
            s2= s2.Trim().ToLower();
            return s1==s2 || s1+"_ishtml" == s2 || s2 + "_ishtml" == s1;
        }
        private static string getDecode(string str)
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
    }
}
