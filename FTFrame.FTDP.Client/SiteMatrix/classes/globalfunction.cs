using System;
using System.Diagnostics;
using System.Windows.Forms;
using FTDPClient.forms;
using System.IO;
using FTDPClient.consts;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Drawing;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Linq;
using FTDPClient.database;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using FTDPClient.forms.control;

namespace FTDPClient.functions
{
    /// <summary>
    /// globalfunction 的摘要说明。
    /// </summary>
    public class Adv
    {
        public static string ConsoleOutput(string filepath, string[] args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            string arg = "";
            foreach (string s in args)
            {
                arg += "\"" + s + "\" ";
            }
            start.Arguments = arg;
            start.WorkingDirectory = new FileInfo(filepath).Directory.FullName;
            start.FileName = filepath;
            start.UseShellExecute = false;
            start.RedirectStandardInput = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.CreateNoWindow = true;
            Process process = Process.Start(start);
            Application.DoEvents();
            string restr = process.StandardOutput.ReadToEnd();
            Application.DoEvents();
            process.WaitForExit();//等待控制台程序执行完成
            Application.DoEvents();
            process.Close();//关闭该进程
            Application.DoEvents();
            return restr;
            //process.OutputDataReceived += (sender, e) =>
            //{
            //    if (!string.IsNullOrEmpty(e.Data))
            //    {

            //    }
            //    else
            //    {
            //        //控制台程序异常后的代码
            //    }
            //    process.CancelOutputRead();
            //    process.Close();
            //    process.Dispose();
            //};
            //process.BeginOutputReadLine();
        }
        public static string GetColumnDescription(globalConst.DBType dbtype, string connstr, string colName)
        {
            string des = "";
            if (dbtype==globalConst.DBType.SqlServer)
            {
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    string sql = $"select description from " +
                                $" (select sys.columns.name, sys.types.name as typename, sys.columns.max_length, sys.columns.precision, sys.columns.scale, sys.columns.is_nullable, " +
                                $" (select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as is_identity , " +
                                $" sys.columns.column_id,(select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as description " +
                                $" from sys.columns, sys.tables, sys.types where sys.columns.object_id = sys.tables.object_id and sys.columns.user_type_id=sys.types.user_type_id ) " +
                                $" t1 where name='" + colName + "' order by description desc";
                    using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            des = dr.IsDBNull(0) ? "" : dr.GetValue(0).ToString();
                            return des;
                        }
                    }
                }
            }
            else if (dbtype==globalConst.DBType.MySql)
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    string sql = $"select column_comment from INFORMATION_SCHEMA.Columns where table_schema=(SELECT DATABASE() as dbname limit 1)" +
                        $" and COLUMN_NAME='" + colName + "' order by column_comment desc";
                    using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            des = dr.IsDBNull(0) ? "" : dr.GetValue(0).ToString();
                            return des;
                        }
                    }
                }
            }
            return "";
        }
        public static void GetColumnDescription(globalConst.DBType dbtype, string connstr, ref Dictionary<string, string> dic)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("1=0");
            if (dbtype== globalConst.DBType.SqlServer)
            {
                foreach (var item in dic)
                {
                    sb.Append(" or name='" + item.Key + "'");
                }
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    string sql = $"select name,description from " +
                                $" (select sys.columns.name, sys.types.name as typename, sys.columns.max_length, sys.columns.precision, sys.columns.scale, sys.columns.is_nullable, " +
                                $" (select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as is_identity , " +
                                $" sys.columns.column_id,(select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as description " +
                                $" from sys.columns, sys.tables, sys.types where sys.columns.object_id = sys.tables.object_id and sys.columns.user_type_id=sys.types.user_type_id ) " +
                                $" t1 where (" + sb.ToString() + ") order by description desc";
                    using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string name = dr.GetString(0);
                            if (dic.ContainsKey(name) && dic[name] == null)
                            {
                                dic[name] = dr.IsDBNull(1) ? "" : dr.GetValue(1).ToString();
                            }
                        }
                    }
                }
            }
            else if (dbtype==globalConst.DBType.MySql)
            {
                foreach (var item in dic)
                {
                    sb.Append(" or COLUMN_NAME='" + item.Key + "'");
                }
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    string sql = $"select COLUMN_NAME,column_comment from INFORMATION_SCHEMA.Columns where table_schema=(SELECT DATABASE() as dbname limit 1)" +
                        $" and (" + sb.ToString() + ") order by column_comment desc";
                    using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string name = dr.GetString(0);
                            if (dic.ContainsKey(name) && dic[name] == null)
                            {
                                dic[name] = dr.IsDBNull(1) ? "" : dr.GetValue(1).ToString();
                            }
                        }
                    }
                }
            }

            foreach (var key in dic.Keys.ToArray())
            {
                if (dic[key] == null)
                {
                    dic[key] = "";
                }
            }
        }
        public static string GetSqlForRemoveSameCols(globalConst.DBType dbtype, string connstr, string oriSql)
        {
            //@*@
            if (oriSql.IndexOf("@*@") < 0) return oriSql;
            string _oriSql = oriSql.Replace("@*@", "*");
            StringBuilder selCols = new StringBuilder(100);
            if (dbtype==globalConst.DBType.SqlServer)
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
                            if (!key.StartsWith("alias_"))
                            {
                                if (!dic.ContainsKey(key)) dic.Add(key, 1);
                                else dic[key] += 1;
                            }
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
            else if (dbtype == globalConst.DBType.MySql)
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
                            if (!key.StartsWith("alias_"))
                            {
                                if (!dic.ContainsKey(key)) dic.Add(key, 1);
                                else dic[key] += 1;
                            }
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
        public static string CodePattern(string oriInput)
        {
            if (oriInput == null) return null;
            Regex r = new Regex(@"@code\([^(\)@)]*\)");
            var mc = r.Matches(oriInput);
            foreach (Match m in mc)
            {
                //string pattern = m.Value.Substring(0, m.Value.Length);
                oriInput = oriInput.Replace(m.Value, " ");
            }
            r = new Regex(@"@para\{[^(\})]*\}");
            mc = r.Matches(oriInput);
            foreach (Match m in mc)
            {
                //string pattern = m.Value.Substring(0, m.Value.Length);
                oriInput = oriInput.Replace(m.Value, " ");
            }
            r = new Regex(@"@enum\([^(\)@)]*\)");
            mc = r.Matches(oriInput);
            foreach (Match m in mc)
            {
                //string pattern = m.Value.Substring(0, m.Value.Length);
                oriInput = oriInput.Replace(m.Value, " ");
            }
            r = new Regex(@"@dic\([^(\)@)]*\)");
            mc = r.Matches(oriInput);
            foreach (Match m in mc)
            {
                oriInput = oriInput.Replace(m.Value, " ");
            }
            return oriInput;
        }
        public static int RemoteSqlExec(string sql, globalConst.DBType dbtype = globalConst.DBType.UnKnown, string connstr = null)
        {
            try
            {
                if (dbtype == globalConst.DBType.UnKnown) dbtype = Options.GetSystemDBSetType_Plat();
                connstr = connstr ?? Options.GetSystemDBSetConnStr_Plat();
                if (dbtype==globalConst.DBType.SqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(connstr))
                    {
                        conn.Open();
                        return new SqlCommand(sql, conn).ExecuteNonQuery();
                    }
                }
                else if (dbtype == globalConst.DBType.MySql)
                {
                    using (MySqlConnection conn = new MySqlConnection(connstr))
                    {
                        conn.Open();
                        return new MySqlCommand(sql, conn).ExecuteNonQuery();
                    }
                }
                else if (dbtype == globalConst.DBType.Sqlite)
                {
                    using (var conn = new DB(connstr))
                    {
                        conn.Open();
                        return conn.execSql(sql);
                    }
                }
                else
                {
                    MsgBox.Error("dbtype plat error");
                }
                return -1;
            }
            catch (Exception ex)
            {
                new error(ex);
                return -1;
            }
        }
        public static DataTable RemoteSqlQuery(string sql, globalConst.DBType dbtype = globalConst.DBType.UnKnown, string connstr = null, bool exceptionout = false)
        {
            try
            {
                if (dbtype == globalConst.DBType.UnKnown) dbtype = Options.GetSystemDBSetType_Plat();
                connstr = connstr ?? Options.GetSystemDBSetConnStr_Plat();
                DataTable datatable = new DataTable();
                if (dbtype != globalConst.DBType.UnKnown && !string.IsNullOrEmpty(connstr))
                {
                    if (dbtype==globalConst.DBType.SqlServer)
                    {
                        using (SqlConnection conn = new SqlConnection(connstr))
                        {
                            conn.Open();
                            using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                            {
                                for (int i = 0; i < dr.FieldCount; i++)
                                {
                                    DataColumn myDataColumn = new DataColumn();
                                    myDataColumn.DataType = dr.GetFieldType(i);
                                    myDataColumn.ColumnName = dr.GetName(i);
                                    datatable.Columns.Add(myDataColumn);
                                }
                                while (dr.Read())
                                {
                                    DataRow myDataRow = datatable.NewRow();
                                    for (int i = 0; i < dr.FieldCount; i++)
                                    {
                                        myDataRow[i] = dr.IsDBNull(i) ? "" : (dr[i].ToString());
                                    }
                                    datatable.Rows.Add(myDataRow);
                                    myDataRow = null;
                                }
                            }
                        }
                    }
                    else if (dbtype == globalConst.DBType.MySql)
                    {
                        using (MySqlConnection conn = new MySqlConnection(connstr))
                        {
                            conn.Open();
                            using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                            {
                                for (int i = 0; i < dr.FieldCount; i++)
                                {
                                    DataColumn myDataColumn = new DataColumn();
                                    myDataColumn.DataType = dr.GetFieldType(i);
                                    myDataColumn.ColumnName = dr.GetName(i);
                                    datatable.Columns.Add(myDataColumn);
                                }
                                while (dr.Read())
                                {
                                    DataRow myDataRow = datatable.NewRow();
                                    for (int i = 0; i < dr.FieldCount; i++)
                                    {
                                        myDataRow[i] = dr.IsDBNull(i) ? "" : (dr[i].ToString());
                                    }
                                    datatable.Rows.Add(myDataRow);
                                    myDataRow = null;
                                }
                            }
                        }
                    }
                    else if (dbtype == globalConst.DBType.Sqlite)
                    {
                        using (var conn = new DB(connstr))
                        {
                            conn.Open();
                            using (var dr = conn.OpenRecord(sql))
                            {
                                for (int i = 0; i < dr.FieldCount; i++)
                                {
                                    DataColumn myDataColumn = new DataColumn();
                                    myDataColumn.DataType = dr.GetFieldType(i);
                                    myDataColumn.ColumnName = dr.GetName(i);
                                    datatable.Columns.Add(myDataColumn);
                                }
                                while (dr.Read())
                                {
                                    DataRow myDataRow = datatable.NewRow();
                                    for (int i = 0; i < dr.FieldCount; i++)
                                    {
                                        myDataRow[i] = dr.IsDBNull(i) ? "" : (dr[i].ToString());
                                    }
                                    datatable.Rows.Add(myDataRow);
                                    myDataRow = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        MsgBox.Error("dbtype plat error");
                        return null;
                    }
                }
                return datatable;
            }
            catch (Exception ex)
            {
                if (exceptionout) throw ex;
                else
                {
                    new error(ex);
                    return null;
                }
            }
        }

        public static (string tablename, string tabledesc) SelectTable()
        {
            try
            {
                string connstr = Options.GetSystemDBSetConnStr();
                var dbtype = Options.GetSystemDBSetType();
                if (dbtype==globalConst.DBType.MySql)
                {
                    forms.control.SelTable_MySql sel = new forms.control.SelTable_MySql();
                    sel.connstr = connstr;
                    sel.ShowDialog();
                    if (sel.tablename != null)
                    {
                        return (sel.tablename, sel.tabledesc);
                    }
                }
                else if (dbtype == globalConst.DBType.SqlServer)
                {
                    forms.control.SelTable_SqlServer sel = new forms.control.SelTable_SqlServer();
                    sel.connstr = connstr;
                    sel.ShowDialog();
                    if (sel.tablename != null)
                    {
                        return (sel.tablename, sel.tabledesc);
                    }
                }
                else if (dbtype == globalConst.DBType.Sqlite)
                {
                    forms.control.SelTable_Sqlite sel = new forms.control.SelTable_Sqlite();
                    sel.connstr = connstr;
                    sel.ShowDialog();
                    if (sel.tablename != null)
                    {
                        return (sel.tablename, sel.tabledesc);
                    }
                }
                return (null, null);
            }
            catch (Exception ex)
            {
                new error(ex);
                return (null, null);
            }
        }
        public static (string columnname, string columndesc) SelectColumn(string tablename)
        {
            try
            {
                string connstr = Options.GetSystemDBSetConnStr();
                var dbtype = Options.GetSystemDBSetType();
                if (dbtype==globalConst.DBType.SqlServer)
                {
                    forms.control.SelCol_SqlServer sel = new forms.control.SelCol_SqlServer();
                    sel.connstr = connstr;
                    sel.tablename = tablename;
                    sel.ShowDialog();
                    if (sel.colname != null)
                    {
                        return (sel.colname, sel.coldesc);
                    }
                }
                else if (dbtype == globalConst.DBType.MySql)
                {
                    forms.control.SelCol_MySql sel = new forms.control.SelCol_MySql();
                    sel.connstr = connstr;
                    sel.tablename = tablename;
                    sel.ShowDialog();
                    if (sel.colname != null)
                    {
                        return (sel.colname, sel.coldesc);
                    }
                }
                else if (dbtype == globalConst.DBType.Sqlite)
                {
                    forms.control.SelCol_Sqlite sel = new forms.control.SelCol_Sqlite();
                    sel.connstr = connstr;
                    sel.tablename = tablename;
                    sel.ShowDialog();
                    if (sel.colname != null)
                    {
                        return (sel.colname, sel.coldesc);
                    }
                }
                return (null, null);
            }
            catch (Exception ex)
            {
                new error(ex);
                return (null, null);
            }
        }
        public static string TableDesc(string tablename)
        {
            var item = FTDP.Util.ICSharpTextEditor.completionData_Table.Where(r => r[0].ToString().Equals(tablename, StringComparison.CurrentCultureIgnoreCase));
            if (item.Count() > 0)
            {
                return item.First()[1].ToString();
            }
            return null;
        }
        public static string ColumnDesc(string tablename, string colname, globalConst.DBType dbtype = globalConst.DBType.UnKnown, string connstr = null)
        {
            if (dbtype == globalConst.DBType.UnKnown) dbtype = Options.GetSystemDBSetType();
            connstr = connstr ?? Options.GetSystemDBSetConnStr();
            var StrictFields = new List<object[]>();
            try
            {
                StrictFields = FTDP.Util.ICSharpTextEditor.GetStrictFields(DBFunc.DBTypeString(dbtype), connstr, tablename);
            }
            catch { }
            var item2 = StrictFields.Where(r => r[0].ToString().Equals(colname, StringComparison.CurrentCultureIgnoreCase));
            if (item2.Count() > 0)
            {
                return item2.First()[1].ToString();
            }
            return null;
        }
        public static Dictionary<string, List<(string[] Cdn, string ParaValue)>> paraDic = null;
        public static Dictionary<string, List<(string[] Cdn, string ParaValue)>> ParaDic()
        {
            if (paraDic != null) return paraDic;
            paraDic = new Dictionary<string, List<(string[] Cdn, string ParaValue)>>();
            try
            {
                var dt = RemoteSqlQuery("select paraname,paravalue from ft_ftdp_para where stat=1 order by paraname");
                foreach (DataRow row in dt.Rows)
                {
                    string paraname = row[0].ToString();
                    string paravalue = row[1].ToString();
                    if (!paravalue.StartsWith("[#IF#]"))
                    {
                        paraDic.Add(paraname, new List<(string[] Cdn, string ParaValue)>() { (null, paravalue) });
                    }
                    else
                    {
                        var list = new List<(string[] Cdn, string ParaValue)>();
                        string[] lines = paravalue.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                        string curval = "";
                        List<string> vals = new List<string>();
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i].StartsWith("[#IF#]") || lines[i].StartsWith("[#ELSE#]"))
                            {
                                if (i > 0)
                                {
                                    vals.Add(curval.Trim());
                                    curval = "";
                                }
                                vals.Add(lines[i].Trim());
                            }
                            else
                            {
                                curval += lines[i] + Environment.NewLine;
                            }
                        }
                        vals.Add(curval.Trim());
                        for (int i = 0; i < (vals.Count / 2); i++)
                        {
                            if (vals[2 * i].StartsWith("[#IF#]"))
                            {
                                var cdnItem = vals[2 * i].Substring("[#IF#]".Length).Split(new string[] { "[##]" }, StringSplitOptions.None);
                                list.Add((cdnItem, vals[2 * i + 1]));
                            }
                            else if (vals[2 * i].StartsWith("[#ELSE#]"))
                            {
                                list.Add((new string[] { "[#ELSE#]" }, vals[2 * i + 1]));
                            }
                        }
                        paraDic.Add(paraname, list);
                    }
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
            return paraDic;
        }
        public static string ParaPattern(string oriInput, int[] loopCount = null, bool loopSon = true)
        {
            if (loopCount == null) loopCount = new int[] { 1 };
            if (loopCount[0] > 999)
            {
                new error("ParaPattern {maybe endless loop} " + oriInput);
                return oriInput;
            }
            //@para{abc} @para{abc,1|2}
            if (oriInput == null) return null;
            Regex r = new Regex(@"@para\{[^(\})]*\}");
            var mc = r.Matches(oriInput);
            List<string> mValue = new List<string>();
            foreach (Match m in mc)
            {
                string p = m.Value;
                if (mValue.Contains(p)) continue;
                mValue.Add(p);
                //@para{abc,1|2}
                string pattern = p;
                pattern = pattern.Substring(pattern.IndexOf('{') + 1, pattern.LastIndexOf('}') - pattern.IndexOf('{') - 1);
                //abc,1|2   abc,
                if (pattern.IndexOf(',') < 0)
                {
                    pattern += ",";
                }
                int index = pattern.IndexOf(',');
                //abc
                string left = pattern.Substring(0, index).Trim();
                //1|2   
                string right = pattern.Substring(index + 1).Trim();
                string[] paras = new string[0];
                if (right != "") {
                    paras = right.Split('|');
                    for(var i=0;i<paras.Length;i++)
                    {
                        paras[i] = paras[i].Replace("[##]","|");
                    }
                }
                string val = "";
                if (ParaDic().ContainsKey(left))
                {
                    var paraSet = ParaDic()[left];
                    if (paraSet.Count == 1)
                    {
                        if (paraSet[0].Cdn == null)
                        {
                            val = paraSet[0].ParaValue;
                        }
                    }
                    else
                    {
                        Dictionary<string, string> pairValueDic = new Dictionary<string, string>();
                        foreach (var paraItem in paraSet)
                        {
                            if (paraItem.Cdn.Length == 3)
                            {
                                string lval = paraItem.Cdn[0];
                                string pair = paraItem.Cdn[1];
                                string rval = paraItem.Cdn[2];
                                for (int i = 0; i < paras.Length && i < 12; i++)
                                {
                                    lval = lval.Replace("@para" + (i + 1) + "@", paras[i]);
                                    rval = rval.Replace("@para" + (i + 1) + "@", paras[i]);
                                }
                                if (pairValueDic.ContainsKey(lval)) lval = pairValueDic[lval];
                                else
                                {
                                    pairValueDic.Add(lval, CodePattern(lval));
                                    lval = pairValueDic[lval];
                                }
                                if (pairValueDic.ContainsKey(rval)) rval = pairValueDic[rval];
                                else
                                {
                                    pairValueDic.Add(rval, CodePattern(rval));
                                    rval = pairValueDic[rval];
                                }
                                bool cdnOK = false;
                                switch (pair)
                                {
                                    case "==":
                                        cdnOK = lval.Equals(rval);
                                        break;
                                    case ">":
                                        if (decimal.TryParse(lval, out decimal l01) && decimal.TryParse(rval, out decimal l02))
                                        {
                                            cdnOK = l01 > l02;
                                        }
                                        break;
                                    case "<":
                                        if (decimal.TryParse(lval, out decimal l11) && decimal.TryParse(rval, out decimal l12))
                                        {
                                            cdnOK = l11 < l12;
                                        }
                                        break;
                                    case ">=":
                                        if (decimal.TryParse(lval, out decimal l21) && decimal.TryParse(rval, out decimal l22))
                                        {
                                            cdnOK = l21 >= l22;
                                        }
                                        break;
                                    case "<=":
                                        if (decimal.TryParse(lval, out decimal l31) && decimal.TryParse(rval, out decimal l32))
                                        {
                                            cdnOK = l31 <= l32;
                                        }
                                        break;
                                    case "!=":
                                        cdnOK = !lval.Equals(rval);
                                        break;
                                    case "Start":
                                        cdnOK = lval.StartsWith(rval);
                                        break;
                                    case "End":
                                        cdnOK = lval.EndsWith(rval);
                                        break;
                                    case "Contain":
                                        cdnOK = lval.Contains(rval);
                                        break;
                                    default:
                                        break;
                                }
                                if (cdnOK)
                                {
                                    val = paraItem.ParaValue;
                                    break;
                                }
                            }
                            else if (paraItem.Cdn.Length == 1)
                            {
                                val = paraItem.ParaValue;
                                break;
                            }
                        }
                        pairValueDic.Clear();
                        pairValueDic = null;
                    }
                }
                else val = "{no para key:" + left + "}";
                for (int i = 0; i < paras.Length && i < 12; i++)
                {
                    val = val.Replace("@para" + (i + 1) + "@", paras[i]);
                }
                val = CodePattern(val);
                loopCount[0]++;
                if (loopSon) val = ParaPattern(val, loopCount);
                oriInput = oriInput.Replace(p, val);
            }
            return oriInput;
        }
        public static string SQLPatternOP(string sql)
        {
            if (sql.Trim().StartsWith("@para{")) return ParaPattern(sql);
            return sql;
        }

        public static List<string> GetDefaultColumns(string tablename)
        {
            if (string.IsNullOrWhiteSpace(tablename)) return new List<string>();
            var cop = Adv.ClientOperationPost();
            string url = cop.url;
            string postStr = cop.postStr + "&type=DefaultColumn&TableName="+ tablename;
            try
            {
                string reStr = net.HttpPost(url, postStr).Trim();
                return reStr.Split(new string[] { "|"},StringSplitOptions.RemoveEmptyEntries).Select(r=>r.ToLower()).Where(r=>r.IndexOf("error:")<0).ToList();
            }
            catch (Exception ex)
            {
                new error(ex);
                return new List<string>();
            }
        }
        public static void MainFormLoad()
        {
            //MsgBox.Information(str.AesEncrypt(""));
            //MsgBox.Information(str.AesEncrypt("2r098ik的"));
            //MsgBox.Information(str.AesDecrypt_Fit(""));
            //MsgBox.Information(str.AesDecrypt_Fit("BMzoOSiVJnzyttZbZlK0Sg=="));
            //MsgBox.Information(str.AesDecrypt_Fit("fBLWPKRcAQ/MvgOeIvbJew=="));
            //SiteClass.Site.open("xijin");
            //new ForeDev().ShowDialog();
        }
        public static T DRToType<T>(DR dr) where T : new()
        {
            T t = new T();
            var ps = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (System.Reflection.PropertyInfo item in ps)
            {
                int index = dr.FieldIndex(item.Name);
                if (index >= 0)
                {
                    item.SetValue(t, dr.getValue(index), null);
                }
            }
            return t;
        }
        public static void CompletionData_Assembly()
        {
            try
            {
                FTDP.Util.ICSharpTextEditor.completionData_Assembly.Clear();
                string connstr = Options.GetSystemDBSetConnStr_Plat();
                var dbtype = Options.GetSystemDBSetType_Plat();
                FTDP.Util.ICSharpTextEditor.DBType_Plat = DBFunc.DBTypeString(dbtype);
                FTDP.Util.ICSharpTextEditor.DBConnStr_Plat = connstr;
                if (dbtype!=globalConst.DBType.UnKnown && !string.IsNullOrEmpty(connstr))
                {
                    if (dbtype==globalConst.DBType.MySql)
                    {
                        using (var conn = new MySqlConnection(connstr))
                        {
                            conn.Open();
                            string sql = "SELECT codetype,codekey,codeval from ft_ftdp_codestatic";
                            using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    string name = "";
                                    if (dr.GetInt16(0) == 0)
                                    {
                                        name = "@code(" + dr.GetString(1) + ")";
                                        //else if (dr.GetInt16(0) == 1) name = "@enum(" + dr.GetString(1) + ")";
                                        FTDP.Util.ICSharpTextEditor.completionData_Assembly.Add(new object[] { name, dr.IsDBNull(2) ? "" : dr.GetValue(2).ToString(), 4 });
                                    }
                                }
                            }
                        }
                    }
                    else if (dbtype == globalConst.DBType.SqlServer)
                    {
                        using (var conn = new SqlConnection(connstr))
                        {
                            conn.Open();
                            string sql = "SELECT codetype,codekey,codeval from ft_ftdp_codestatic";
                            using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    string name = "";
                                    if (dr.GetInt16(0) == 0)
                                    {
                                        name = "@code(" + dr.GetString(1) + ")";
                                        //else if (dr.GetInt16(0) == 1) name = "@enum(" + dr.GetString(1) + ")";
                                        FTDP.Util.ICSharpTextEditor.completionData_Assembly.Add(new object[] { name, dr.IsDBNull(2) ? "" : dr.GetValue(2).ToString(), 4 });
                                    }
                                }
                            }
                        }
                    }
                    else if (dbtype == globalConst.DBType.Sqlite)
                    {
                        using (var conn = new DB(connstr))
                        {
                            conn.Open();
                            string sql = "SELECT codetype,codekey,codeval from ft_ftdp_codestatic";
                            using (var dr = conn.OpenRecord(sql))
                            {
                                while (dr.Read())
                                {
                                    string name = "";
                                    if (dr.GetInt16(0) == 0)
                                    {
                                        name = "@code(" + dr.GetString(1) + ")";
                                        //else if (dr.GetInt16(0) == 1) name = "@enum(" + dr.GetString(1) + ")";
                                        FTDP.Util.ICSharpTextEditor.completionData_Assembly.Add(new object[] { name, dr.IsDBNull(2) ? "" : dr.GetValue(2).ToString(), 4 });
                                    }
                                }
                            }
                        }
                    }
                }

                string sql1 = "select devuser,codekey,codeval,returntype,dllname,mimo from codeset order by id";
                using (SqliteDataReader rdr = consts.globalConst.CurSite.SiteConn.OpenRecord(sql1))
                {
                    while (rdr.Read())
                    {
                        FTDP.Util.ICSharpTextEditor.completionData_Assembly.Add(new object[] { "@code($" + rdr.GetString(1) + ")", rdr.IsDBNull(2) ? "" : rdr.GetValue(2).ToString(), 4 });
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.Warning(ex.Message);
            }
        }
        public static void CompletionData_Table()
        {
            try
            {
                FTDP.Util.ICSharpTextEditor.completionData_Table.Clear();
                string connstr = Options.GetSystemDBSetConnStr();
                var dbtype = Options.GetSystemDBSetType();
                FTDP.Util.ICSharpTextEditor.DBType = DBFunc.DBTypeString(dbtype);
                FTDP.Util.ICSharpTextEditor.DBConnStr = connstr;
                if (dbtype!=globalConst.DBType.UnKnown && !string.IsNullOrEmpty(connstr))
                {
                    if (dbtype==globalConst.DBType.MySql)
                    {
                        using (var conn = new MySqlConnection(connstr))
                        {
                            conn.Open();
                            string sql = "SELECT TABLE_NAME,TABLE_COMMENT FROM information_schema.TABLES WHERE table_schema=(SELECT DATABASE() as dbname limit 1)";
                            using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    FTDP.Util.ICSharpTextEditor.completionData_Table.Add(new object[] { dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetValue(1).ToString(), 1 });
                                }
                            }
                        }
                    }
                    else if (dbtype == globalConst.DBType.SqlServer)
                    {
                        using (var conn = new SqlConnection(connstr))
                        {
                            conn.Open();
                            string sql = $"select t1.name,t2.value from (" +
                    $" select name from sys.tables where type='U' " +
                        $" union" +
                        $" SELECT Name FROM sys.sql_modules AS m  INNER JOIN sys.all_objects AS o ON m.object_id = o.object_id WHERE o.[type] = 'v' ) t1" +
                        $" left join " +
                        $"( " +
                        $" SELECT DISTINCT " +
                        $" d.name, " +
                        $" f.value " +
                        $" FROM " +
                        $" syscolumns a " +
                        $" LEFT JOIN systypes b ON a.xusertype = b.xusertype " +
                        $" INNER JOIN sysobjects d ON a.id = d.id " +
                        $" AND d.xtype = 'U' " +
                        $" AND d.name <> 'dtproperties' " +
                        $" LEFT JOIN syscomments e ON a.cdefault = e.id " +
                        $" LEFT JOIN sys.extended_properties g ON a.id = G.major_id " +
                        $" AND a.colid = g.minor_id " +
                        $" LEFT JOIN sys.extended_properties f ON d.id = f.major_id " +
                        $" AND f.minor_id = 0 ) t2 on t1.name = t2.name " +
                        $" order by t1.name" +
                        $"";
                            using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    FTDP.Util.ICSharpTextEditor.completionData_Table.Add(new object[] { dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetValue(1).ToString(), 1 });
                                }
                            }
                        }
                    }
                    else if (dbtype == globalConst.DBType.Sqlite)
                    {
                        using (var conn = new DB(connstr))
                        {
                            conn.Open();
                            string sql = "SELECT name,'' note FROM sqlite_master as t1 WHERE type='table' order by name";
                            using (var dr = conn.OpenRecord(sql))
                            {
                                while (dr.Read())
                                {
                                    FTDP.Util.ICSharpTextEditor.completionData_Table.Add(new object[] { dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetValue(1).ToString(), 1 });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.Warning(ex.Message);
            }
        }
        public static void CompletionData_Para()
        {
            try
            {
                FTDP.Util.ICSharpTextEditor.completionData_Para.Clear();
                string connstr = Options.GetSystemDBSetConnStr_Plat();
                var dbtype = Options.GetSystemDBSetType_Plat();
                if (dbtype!=globalConst.DBType.UnKnown && !string.IsNullOrEmpty(connstr))
                {
                    if (dbtype==globalConst.DBType.MySql)
                    {
                        using (var conn = new MySqlConnection(connstr))
                        {
                            conn.Open();
                            string sql = "select paraname,paracaption,mimo from ft_ftdp_para where stat=1 order by paraname";
                            using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    if (!dr.GetString(0).StartsWith("@") && !dr.GetString(0).StartsWith("Front_") && !dr.GetString(0).StartsWith("Enum_"))
                                    {
                                        FTDP.Util.ICSharpTextEditor.completionData_Para.Add(new object[] { "@para{" + dr.GetString(0) + "}", dr.GetString(1) + Environment.NewLine + dr.GetString(2), 9 });
                                    }
                                }
                            }
                        }
                    }
                    else if (dbtype == globalConst.DBType.SqlServer)
                    {
                        using (var conn = new SqlConnection(connstr))
                        {
                            conn.Open();
                            string sql = "select paraname,paracaption,mimo from ft_ftdp_para where stat=1 order by paraname";
                            using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    if (!dr.GetString(0).StartsWith("@") && !dr.GetString(0).StartsWith("Front_") && !dr.GetString(0).StartsWith("Enum_"))
                                    {
                                        FTDP.Util.ICSharpTextEditor.completionData_Para.Add(new object[] { "@para{" + dr.GetString(0) + "}", dr.GetString(1) + Environment.NewLine + dr.GetString(2), 9 });
                                    }
                                }
                            }
                        }
                    }
                    else if (dbtype == globalConst.DBType.Sqlite)
                    {
                        using (var conn = new DB(connstr))
                        {
                            conn.Open();
                            string sql = "select paraname,paracaption,mimo from ft_ftdp_para where stat=1 order by paraname";
                            using (var dr = conn.OpenRecord(sql))
                            {
                                while (dr.Read())
                                {
                                    if (!dr.GetString(0).StartsWith("@") && !dr.GetString(0).StartsWith("Front_") && !dr.GetString(0).StartsWith("Enum_"))
                                    {
                                        FTDP.Util.ICSharpTextEditor.completionData_Para.Add(new object[] { "@para{" + dr.GetString(0) + "}", dr.GetString(1) + Environment.NewLine + dr.GetString(2), 9 });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.Warning(ex.Message);
            }
        }
        public static void CompletionData_Dic()
        {
            try
            {
                FTDP.Util.ICSharpTextEditor.completionData_Dic.Clear();
                FTDP.Util.ICSharpTextEditor.completionData_Dic_Data.Clear();
                string sql = "select * from ft_dic";
                var dt = Adv.RemoteSqlQuery(sql, globalConst.DBType.UnKnown, null, true);
                var list = new List<(string dic_type, string dic_value, string dic_label, string dic_name, string dic_desc)>();
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        list.Add((row["dic_type"].ToString(), row["dic_value"].ToString(), row["dic_label"].ToString(), row["dic_name"].ToString(), row["dic_desc"].ToString()));
                    }
                }
                foreach (var dic_type in list.Select(r => r.dic_type).Distinct())
                {
                    var dicrow = list.Where(r => r.dic_type == dic_type).FirstOrDefault();
                    if (dicrow.dic_type != null)
                    {
                        FTDP.Util.ICSharpTextEditor.completionData_Dic.Add(new object[] { "@dic(" + dic_type + ")", dicrow.dic_name == "" ? "  " : dicrow.dic_name, 4 });
                        var datas = list.Where(r => r.dic_type == dic_type).Select(r => (r.dic_value + "." + r.dic_label.Replace(".", "").Replace("(", "").Replace(")", ""), r.dic_desc == "" ? "  " : r.dic_desc)).ToList();
                        FTDP.Util.ICSharpTextEditor.completionData_Dic_Data.Add(dic_type, datas);
                    }
                }
                dt.Clear();
                dt.Dispose();
                list.Clear();
                list = null;
            }
            catch (Exception ex)
            {
                FTDP.Util.ICSharpTextEditor.completionData_Dic.Add(new object[] { "@dic(error)", ex.Message, 4 });
                //MsgBox.Warning(ex.Message);
            }
        }
        public static void CompletionData_Enum()
        {
            try
            {
                FTDP.Util.ICSharpTextEditor.completionData_Enum.Clear();
                FTDP.Util.ICSharpTextEditor.completionData_Enum_Data.Clear();
                string sql = "select paraname,paravalue,paracaption,mimo from ft_ftdp_para where stat=1 order by paraname";
                var dt = Adv.RemoteSqlQuery(sql);
                foreach (DataRow row in dt.Rows)
                {
                    string paraname = row["paraname"].ToString();
                    string paravalue = row["paravalue"].ToString();
                    string paracaption = row["paracaption"].ToString();
                    string mimo = row["mimo"].ToString();
                    if (!paraname.StartsWith("Enum_")) continue;
                    paraname = paraname.Substring(5);
                    FTDP.Util.ICSharpTextEditor.completionData_Enum.Add(new object[] { "@enum(" + paraname + ")", paracaption + Environment.NewLine + mimo+" ", 9 });
                    var enumDatas = new List<(string,string)>();
                    if (paravalue.StartsWith("[#IF#]"))
                    {
                        //var list = new List<(string[] Cdn, string ParaValue)>();
                        string[] lines = paravalue.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                        string curval = "";
                        List<string> vals = new List<string>();
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i].StartsWith("[#IF#]") || lines[i].StartsWith("[#ELSE#]"))
                            {
                                if (i > 0)
                                {
                                    vals.Add(curval.Trim());
                                    curval = "";
                                }
                                vals.Add(lines[i].Trim());
                            }
                            else
                            {
                                curval += lines[i] + Environment.NewLine;
                            }
                        }
                        vals.Add(curval.Trim());
                        for (int i = 0; i < (vals.Count / 2); i++)
                        {
                            if (vals[2 * i].StartsWith("[#IF#]"))
                            {
                                var cdnItem = vals[2 * i].Substring("[#IF#]".Length).Split(new string[] { "[##]" }, StringSplitOptions.None);
                                enumDatas.Add((cdnItem[0], vals[2 * i + 1]+" "));
                                //list.Add((cdnItem, vals[2 * i + 1]));
                            }
                            else if (vals[2 * i].StartsWith("[#ELSE#]"))
                            {
                                //list.Add((new string[] { "[#ELSE#]" }, vals[2 * i + 1]));
                            }
                        }
                        FTDP.Util.ICSharpTextEditor.completionData_Enum_Data.Add(paraname, enumDatas);
                    }
                }
                dt.Clear();
                dt.Dispose();
            }
            catch (Exception ex)
            {
                MsgBox.Warning(ex.Message);
            }
        }
        public static string ClientOperation(string OpType, List<(int Type, string Prop, string Value)> BusiParas)
        {
            if (globalConst.CurSite.ID == null)
            {
                MsgBox.Error("Site Not Opened!");
                return "Site Not Opened!";
            }
            string sql = "select * from sites where id='" + globalConst.CurSite.ID + "'";
            SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
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
                return "siteid is " + globalConst.CurSite.ID + " not found";
            }
            rdr.Close(); rdr = null;
            if (_url.EndsWith("/")) _url = _url.Substring(0, _url.Length - 1);
            string url = _url + "/_ftpub/clientop";
            try
            {
                List<(int Type, string Prop, string Value)> paras = new List<(int Type, string Prop, string Value)>();
                paras.Add((0, "_id", _id));
                paras.Add((0, "_key", _key));
                paras.Add((0, "_user", _user));
                paras.Add((0, "_passwd", _passwd));
                paras.Add((0, "type", OpType));
                paras.AddRange(BusiParas);
                string reStr = net.HttpPostForm(url, paras).Trim();
                if (reStr.StartsWith("error:"))
                {
                    MsgBox.Error(reStr);
                }
                return reStr;
            }
            catch (Exception ex)
            {
                new error(ex);
                return ex.Message;
            }
        }
        public static (string url, string postStr) ClientOperationPost()
        {
            if (globalConst.CurSite.ID == null)
            {
                MsgBox.Error("Site Not Opened!");
                return ("", "");
            }
            string sql = "select * from sites where id='" + globalConst.CurSite.ID + "'";
            using (SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql))
            {
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
                    return ("", "");
                }

                if (_url.EndsWith("/")) _url = _url.Substring(0, _url.Length - 1);
                string url = _url + "/_ftpub/clientop";
                return (url, "_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd);
            }
        }
    }
    public class update
    {
        public static int version()
        {
            int version = 0;
            var filename = Application.StartupPath + @"\update\info.txt";
            if (file.Exists(filename))
            {
                var items = file.getFileText(filename, System.Text.Encoding.UTF8).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in items)
                {
                    if (item.StartsWith("version:"))
                    {
                        version = int.Parse(item.Split(':')[1].Trim());
                        break;
                    }
                }
            }
            return version;
        }
    }
    public class db
    {
        public static string ConnStr(string dbFilePath)
        {
            return "Data Source=" + dbFilePath + ";";
        }
        public static string ConnStr_Cfg()
        {
            return ConnStr(globalConst.ConfigFile);
        }
        public static string ConnStr_Site(string SiteId = null)
        {
            if (SiteId == null) return ConnStr(globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db");
            else return ConnStr(globalConst.AppPath + @"\cfg\site_" + SiteId + ".db");
        }
    }
    public class MsgBox
    {
        public MsgBox()
        {
        }
        public MsgBox(string text)
        {
            MessageBox.Show(text);
        }
        public static DialogResult msg(string text, string caption)
        {
            return MessageBox.Show(text, caption);
        }
        public static DialogResult msg(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(text, caption, buttons, icon);
        }
        public static DialogResult msg(string text, string caption, MessageBoxButtons buttons)
        {
            return MessageBox.Show(text, caption, buttons);
        }
        public static DialogResult msg(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return MessageBox.Show(text, caption, buttons, icon, defaultButton);
        }
        public static DialogResult msg(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return MessageBox.Show(text, caption, buttons, icon, defaultButton, options);
        }
        public static void Information(string text)
        {
            MessageBox.Show(text, res._globalfunctions.GetString("c1"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void Warning(string text,IWin32Window owner)
        {
            MessageBox.Show(owner, text, res._globalfunctions.GetString("c2"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //log.Debug(text, "Message Warning");
        }
        public static void Warning(string text)
        {
            MessageBox.Show(text, res._globalfunctions.GetString("c2"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //log.Debug(text, "Message Warning");
        }
        public static void Error(string text)
        {
            MessageBox.Show(text, res._globalfunctions.GetString("c3"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            log.Debug(text, "Message Error");
        }
        public static DialogResult OKCancel(string text)
        {
            return MessageBox.Show(text, res._globalfunctions.GetString("c4"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }
        public static DialogResult YesNo(string text)
        {
            return MessageBox.Show(text, res._globalfunctions.GetString("c4"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        public static DialogResult YesNoCancel(string text)
        {
            return MessageBox.Show(text, res._globalfunctions.GetString("c4"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }
        public static DialogResult RetryCancel(string text)
        {
            return MessageBox.Show(text, res._globalfunctions.GetString("c4"), MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
        }
        public static DialogResult AbortRetryIgnore(string text)
        {
            return MessageBox.Show(text, res._globalfunctions.GetString("c4"), MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question);
        }
    }
    public class error
    {
        public error()
        { }
        public error(string str)
        {
            ErrorReport er = new ErrorReport();
            er.errorView = str;
            er.ShowInTaskbar = true;
            er.Show();
        }
        public error(Exception ex)
        {
            ErrorReport er = new ErrorReport();
            er.errorString = "{Message}:\r\n" + ex.Message + "\r\n{Source}:\r\n" + ex.Source + "\r\n{InnerException}:\r\n" + ex.InnerException + "\r\n{StackTrace}:\r\n" + ex.StackTrace + "\r\n{TargetSite}:\r\n" + ex.TargetSite;
            er.errorView = "{Message}:\r\n" + ex.Message + "\r\n{Source}:\r\n" + ex.Source + "\r\n{InnerException}:\r\n" + ex.InnerException + "\r\n{StackTrace}:\r\n" + ex.StackTrace + "\r\n{TargetSite}:\r\n" + ex.TargetSite;
            log.Error(er.errorString+Environment.NewLine+ex.StackTrace.ToString()+Environment.NewLine+ex.Source.ToString(), "from class error");
            er.ShowInTaskbar = true;
            er.Show();
        }
    }
    public class file
    {
        public file()
        { }
        public file(string filename)
        {
            try
            {
                File.CreateText(filename);
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static FileStream Create(string filename)
        {
            try
            {
                return File.Create(filename);
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static void Copy(string sourceFileName, string destFileName)
        {
            try
            {
                File.Copy(sourceFileName, destFileName);
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            try
            {
                File.Copy(sourceFileName, destFileName, overwrite);
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static void AppenText(string FileName, string appenText)
        {
            try
            {
                FileInfo fi = new FileInfo(FileName);
                StreamWriter sw = fi.AppendText();
                sw.Write(appenText);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static void CreateText(string FileName, string createText)
        {
            try
            {
                FileInfo fi = new FileInfo(FileName);
                StreamWriter sw = fi.CreateText();
                sw.Write(createText);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static void Move(string sourceFileName, string destFileName)
        {
            try
            {
                File.Move(sourceFileName, destFileName);
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static string getFileText(string filename, System.Text.Encoding Encode)
        {
            try
            {
                //FileInfo fi = new FileInfo(filename);
                //fi.OpenRead();
                StreamReader sr = new StreamReader(filename, Encode);
                string rs = sr.ReadToEnd();
                sr.Close();
                return rs;

            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static FileStream Open(string filename, FileMode mode)
        {
            try
            {
                return File.Open(filename, mode);
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static FileStream Open(string filename, FileMode mode, FileAccess access)
        {
            try
            {
                return File.Open(filename, mode, access);
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static FileStream Open(string filename, FileMode mode, FileAccess access, FileShare share)
        {
            try
            {
                return File.Open(filename, mode, access, share);
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static FileStream OpenRead(string filename)
        {
            try
            {
                return File.OpenRead(filename);
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static StreamReader OpenText(string filename)
        {
            try
            {
                return File.OpenText(filename);
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static FileStream OpenWrite(string filename)
        {
            try
            {
                return File.OpenWrite(filename);
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static bool Exists(string filename)
        {
            return File.Exists(filename);
        }
        public static void Delete(string filename)
        {
            File.Delete(filename);
        }
    }
    public class net
    {
        public static void GotoHelp()
        {
            switch(System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower())
            {
                case "zh-chs":
                    sheel.ExeSheel("http://www.ftframe.com/ftdp/ftdoc_zh.html");
                    break;
                default:
                    sheel.ExeSheel("http://www.ftframe.com/ftdp/ftdoc.html");
                    break;
            }
        }
        public static string HttpPost(string Url, string postDataStr)
        {

            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            byte[] data = Encoding.UTF8.GetBytes(postDataStr);//把字符串转换为字节

            req.ContentLength = data.Length; //请求长度

            using (Stream reqStream = req.GetRequestStream()) //获取
            {
                reqStream.Write(data, 0, data.Length);//向当前流中写入字节
                reqStream.Close(); //关闭当前流
            }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse(); //响应结果
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        public static void HttpPostDownload(string Url, string postDataStr, string filename)
        {
            filename = filename.Replace("/", "\\").Replace("\\\\", "\\");
            try
            {
                Directory.CreateDirectory(filename.Substring(0, filename.LastIndexOf('\\')));
            }
            catch (Exception ex)
            {

            }
            if (!Directory.Exists(filename.Substring(0, filename.LastIndexOf('\\')))) return;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            byte[] data = Encoding.UTF8.GetBytes(postDataStr);//把字符串转换为字节

            req.ContentLength = data.Length; //请求长度

            using (Stream reqStream = req.GetRequestStream()) //获取
            {
                reqStream.Write(data, 0, data.Length);//向当前流中写入字节
                reqStream.Close(); //关闭当前流
            }


            HttpWebResponse resp = (HttpWebResponse)req.GetResponse(); //响应结果
            if (resp.ContentType == "text/plain") return;
            Stream stream = resp.GetResponseStream();
            Stream so = new FileStream(filename, FileMode.Create);
            long totalDownloadedByte = 0;
            byte[] by = new byte[1024];
            int osize = stream.Read(by, 0, (int)by.Length);
            while (osize > 0)
            {
                totalDownloadedByte = osize + totalDownloadedByte;
                Application.DoEvents();
                so.Write(by, 0, osize);
                osize = stream.Read(by, 0, (int)by.Length);
            }
            so.Close();
            stream.Close();
        }
        public static string HttpPostForm(string strUrl, List<(int Type, string Prop, string Value)> postParaList)
        {
            try
            {
                string responseContent = "";
                var memStream = new MemoryStream();
                var webRequest = (HttpWebRequest)WebRequest.Create(strUrl);
                // 边界符
                var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
                // 边界符
                var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
                // 最后的结束符
                var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");
                memStream.Write(beginBoundary, 0, beginBoundary.Length);
                // 设置属性
                webRequest.Method = "POST";
                webRequest.Timeout = 10000;
                webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                for (int i = 0; i < postParaList.Count; i++)
                {
                    var temp = postParaList[i];
                    if (temp.Type == 1)
                    {
                        var fileStream = new FileStream(temp.Value, FileMode.Open, FileAccess.Read);
                        // 写入文件
                        const string filePartHeader =
                            "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                            "Content-Type: application/octet-stream\r\n\r\n";
                        var header = string.Format(filePartHeader, temp.Prop, temp.Value);
                        var headerbytes = Encoding.UTF8.GetBytes(header);
                        memStream.Write(headerbytes, 0, headerbytes.Length);
                        var buffer = new byte[1024];
                        int bytesRead; // =0
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            memStream.Write(buffer, 0, bytesRead);
                        }
                        string end = "\r\n";
                        headerbytes = Encoding.UTF8.GetBytes(end);
                        memStream.Write(headerbytes, 0, headerbytes.Length);
                        fileStream.Close();
                    }
                    else if (temp.Type == 0)
                    {
                        // 写入字符串的Key
                        var stringKeyHeader = "Content-Disposition: form-data; name=\"{0}\"" +
                                       "\r\n\r\n{1}\r\n";
                        var header = string.Format(stringKeyHeader, temp.Prop, temp.Value);
                        var headerbytes = Encoding.UTF8.GetBytes(header);
                        memStream.Write(headerbytes, 0, headerbytes.Length);
                    }
                    if (i != postParaList.Count - 1)
                        memStream.Write(beginBoundary, 0, beginBoundary.Length);
                    else
                        // 写入最后的结束边界符
                        memStream.Write(endBoundary, 0, endBoundary.Length);
                }
                webRequest.ContentLength = memStream.Length;
                var requestStream = webRequest.GetRequestStream();
                memStream.Position = 0;
                var tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();
                using (HttpWebResponse res = (HttpWebResponse)webRequest.GetResponse())
                {


                    using (Stream resStream = res.GetResponseStream())
                    {
                        byte[] buffer = new byte[1024];
                        int read;
                        while ((read = resStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            responseContent += Encoding.UTF8.GetString(buffer, 0, read);
                        }
                    }
                    res.Close();
                }
                return responseContent;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
            return null;




        }
        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET"; //设置请求方式
            request.ContentType = "text/html;charset=UTF-8"; //设置内容类型

            HttpWebResponse response = (HttpWebResponse)request.GetResponse(); //返回响应

            Stream myResponseStream = response.GetResponseStream(); //获得响应流

            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);//以UTF8编码方式读取该流
            string retString = myStreamReader.ReadToEnd();//读取所有

            myStreamReader.Close();//关闭流
            myResponseStream.Close();
            return retString;
        }
        public static string HttpGet(string Url, Dictionary<string,string> Headers)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET"; //设置请求方式
            request.ContentType = "text/html;charset=UTF-8"; //设置内容类型
            request.UserAgent = "FTDP Client " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            foreach (var key in Headers.Keys)
            {
                request.Headers.Add(key, Headers[key]);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse(); //返回响应

            Stream myResponseStream = response.GetResponseStream(); //获得响应流

            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);//以UTF8编码方式读取该流
            string retString = myStreamReader.ReadToEnd();//读取所有

            myStreamReader.Close();//关闭流
            myResponseStream.Close();
            return retString;
        }
        public static string HttpPost(string Url, string postDataStr, Dictionary<string, string> Headers)
        {

            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Method = "POST";
            req.ContentType = "application/json";
            req.UserAgent = "FTDP Client " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            foreach (var key in Headers.Keys)
            {
                req.Headers.Add(key, Headers[key]);
            }

            byte[] data = Encoding.UTF8.GetBytes(postDataStr);//把字符串转换为字节

            req.ContentLength = data.Length; //请求长度

            using (Stream reqStream = req.GetRequestStream()) //获取
            {
                reqStream.Write(data, 0, data.Length);//向当前流中写入字节
                reqStream.Close(); //关闭当前流
            }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse(); //响应结果
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
    public class dir
    {
        public dir()
        { }
        public dir(string dirpath)
        {
            try
            {
                Directory.CreateDirectory(dirpath);
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static void Delete(string dirpath)
        {
            try
            {
                Directory.Delete(dirpath);
            }
            catch
            {
            }
        }
        public static void Delete(string dirpath, bool recursive)
        {
            try
            {
                Directory.Delete(dirpath, recursive);
            }
            catch
            {
            }
        }
        public static bool Exists(string dirpath)
        {
            return Directory.Exists(dirpath);
        }
        public static void Move(string sourceDirName, string destDirName)
        {
            Directory.Move(sourceDirName, destDirName);
        }
        /// <SUMMARY>
        /// Copy a Directory, SubDirectories and Files Given a Source and  
        /// Destination DirectoryInfo Object, Given a SubDirectory Filter
        /// and a File Filter.
        /// IMPORTANT: The search strings for SubDirectories and Files applies 
        /// to every Folder and File within the Source Directory.
        /// </SUMMARY>
        /// <PARAM name="SourceDirectory">A DirectoryInfo Object Pointing 
        /// to the Source Directory</PARAM>
        /// <PARAM name="DestinationDirectory">A DirectoryInfo Object Pointing 
        /// to the Destination Directory</PARAM>
        /// <PARAM name="SourceDirectoryFilter">Search String on  
        ///   SubDirectories (Example: "System*" will return all subdirectories
        ///   starting with "System")</PARAM>
        /// <PARAM name="SourceFileFilter">File Filter: Standard DOS-Style Format 
        ///    (Examples: "*.txt" or "*.exe")</PARAM>
        /// <PARAM name="Overwrite">Whether or not to Overwrite Copied Files in the
        ///     Destination Directory</PARAM>
        public static void Copy(DirectoryInfo SourceDirectory,
            DirectoryInfo DestinationDirectory, string SourceDirectoryFilter,
            string SourceFileFilter, bool Overwrite)
        {
            DirectoryInfo[] SourceSubDirectories;
            FileInfo[] SourceFiles;

            //Check for File Filter
            if (SourceFileFilter != null)
                SourceFiles = SourceDirectory.GetFiles(SourceFileFilter.Trim());
            else
                SourceFiles = SourceDirectory.GetFiles();

            //Check for Folder Filter
            if (SourceDirectoryFilter != null)
                SourceSubDirectories = SourceDirectory.GetDirectories(
                    SourceDirectoryFilter.Trim());
            else
                SourceSubDirectories = SourceDirectory.GetDirectories();

            //Create the Destination Directory
            if (!DestinationDirectory.Exists) DestinationDirectory.Create();

            //Recursively Copy Every SubDirectory and it's 
            //Contents (according to folder filter)
            foreach (DirectoryInfo SourceSubDirectory in SourceSubDirectories)
                Copy(SourceSubDirectory, new DirectoryInfo(
                    DestinationDirectory.FullName + @"\" + SourceSubDirectory.Name),
                    SourceDirectoryFilter, SourceFileFilter, Overwrite);

            //Copy Every File to Destination Directory (according to file filter)
            foreach (FileInfo SourceFile in SourceFiles)
                SourceFile.CopyTo(DestinationDirectory.FullName +
                    @"\" + SourceFile.Name, Overwrite);
        }
        public static void Copy(DirectoryInfo SourceDirectory,
            DirectoryInfo DestinationDirectory, System.Windows.Forms.Label stat, bool Overwrite)
        {
            DirectoryInfo[] SourceSubDirectories;
            FileInfo[] SourceFiles;

            //Check for File Filter

            SourceFiles = SourceDirectory.GetFiles();

            //Check for Folder Filter

            SourceSubDirectories = SourceDirectory.GetDirectories();

            //Create the Destination Directory
            if (!DestinationDirectory.Exists) DestinationDirectory.Create();

            //Recursively Copy Every SubDirectory and it's 
            //Contents (according to folder filter)
            foreach (DirectoryInfo SourceSubDirectory in SourceSubDirectories)
                Copy(SourceSubDirectory, new DirectoryInfo(
                    DestinationDirectory.FullName + @"\" + SourceSubDirectory.Name),
                    stat, Overwrite);

            //Copy Every File to Destination Directory (according to file filter)
            foreach (FileInfo SourceFile in SourceFiles)
            {
                System.Windows.Forms.Application.DoEvents();
                stat.Text = "Copying site file " + SourceFile.Name;
                System.Windows.Forms.Application.DoEvents();

                SourceFile.CopyTo(DestinationDirectory.FullName +
                    @"\" + SourceFile.Name, Overwrite);
            }
        }
        public static DirectoryInfo CreateDirectory(string DirName)
        {
            return Directory.CreateDirectory(DirName);
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class control
    {
        public static string ApiCaption(string apiType)
        {
            string ApiType = null;
            if (apiType.Trim() == "list") ApiType = res.ctl.str("Api_List.7");
            else if (apiType.Trim() == "dyvalue") ApiType = res.ctl.str("Api_List.8");
            else if (apiType.Trim() == "dataop") ApiType = res.ctl.str("Api_List.9");
            return ApiType;
        }
        public static string GetSharedCaptionByIndex(string i)
        {
            switch (i)
            {
                case "0": return res._globalfunctions.GetString("s1");
                case "1": return res._globalfunctions.GetString("s2");
                case "2": return res._globalfunctions.GetString("s3");
            }
            if (i.Equals(res._globalfunctions.GetString("s1"))) return "0";
            if (i.Equals(res._globalfunctions.GetString("s2"))) return "1";
            if (i.Equals(res._globalfunctions.GetString("s3"))) return "2";
            return "(Not Select)";
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class tree
    {
        public enum NodeType
        {
            Root = 0, Directory = 1, Page = 2, Control = 3, Instance = 4, Part = 5, Unknown = 6, Api = 9
        }
        public static NodeType getNodeType(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            switch (getTypeFromID(id))
            {
                case "root":
                    return NodeType.Root;
                case "drct":
                    return NodeType.Directory;
                case "page":
                    return NodeType.Page;
                case "comp":
                    return NodeType.Control;
                case "ctrl":
                    return NodeType.Instance;
                case "part":
                    return NodeType.Part;
                case "fapi":
                    return NodeType.Api;
            }
            return NodeType.Unknown;
        }
        public static NodeType getNodeType(TreeNode nd)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            switch (getTypeFromID(nd))
            {
                case "root":
                    return NodeType.Root;
                case "drct":
                    return NodeType.Directory;
                case "page":
                    return NodeType.Page;
                case "comp":
                    return NodeType.Control;
                case "ctrl":
                    return NodeType.Instance;
                case "part":
                    return NodeType.Part;
                case "fapi":
                    return NodeType.Api;
            }
            return NodeType.Unknown;
        }
        public static string getTypeFromID(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                return id.Substring(id.Length - 4);
            }
            catch (Exception e)
            {
                new error(e);
                return "unknown";
            }
        }
        public static string getTypeFromID(TreeNode nd)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                string id = ((string[])nd.Tag)[2];
                return id.Substring(id.Length - 4);
            }
            catch (Exception e)
            {
                new error(e);
                return "unknown";
            }
        }
        public static string getID(TreeNode nd)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                return ((string[])nd.Tag)[2];
            }
            catch (Exception e)
            {
                new error(e);
                return "unknown";
            }
        }
        public static TreeNode getTreeNodeByID(string id, TreeView tv)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                tree.NodeGetByValue = null;
                tree.getNodebyValue(tv.Nodes, 2, id);
                return tree.NodeGetByValue;
            }
            catch (Exception ex)
            {
                new error(ex);
                return null;
            }
        }
        static List<(string PageId,string PageCaption, string ApiPath, string PartId)> ApiFindInTreeList = null;
        public static void ApiFindInTree(TreeView treeView, string searchText)
        {
            ApiFindInTreeList = new List<(string PageId, string PageCaption, string ApiPath, string PartId)>();
            searchText = searchText.ToLower().Trim();
            var leftText = searchText.Split('?')[0];
            var rightText = searchText.Split('?')[1];
            var apiKey = rightText.Split('/')[0];
            var apiPath = leftText+"?"+ apiKey;
            if (apiPath.StartsWith("/")) apiPath = apiPath.Substring(1);
            //MsgBox.Information(((string[])treeView.Nodes[0].Tag)[2]);
            if(treeView.Nodes.Count<=0)return;
            TreeFind(treeView.Nodes[0], "");
            if(ApiFindInTreeList.Count==0)
            {
                MsgBox.Warning("Cannot find this Api!");
                return;
            }
            if (ApiFindInTreeList.Count == 1)
            {
                PagePositonToPart(ApiFindInTreeList[0].PageId, ApiFindInTreeList[0].PartId, ApiFindInTreeList[0].ApiPath, ApiFindInTreeList[0].PageCaption);
                return;
            }
            var apiPageSelect = new ApiPageSelect();
            apiPageSelect.listBox1.Items.Clear();
            foreach (var item in ApiFindInTreeList)
            {
                apiPageSelect.listBox1.Items.Add(new Obj.ComboItem() {
                 Name = item.ApiPath + " - " + item.PageCaption,
                  Tag = new string[] { item.PageId, item.PartId, item.ApiPath, item.PageCaption }
                });
            }
            apiPageSelect.ShowDialog();
            void TreeFind(TreeNode Node,string tempPath)
            {
                foreach (TreeNode node in Node.Nodes)
                {
                    string id = ((string[])node.Tag)[2];
                    var type = id.Substring(id.Length - 4);
                    if (type == "fapi")
                    {
                        var api = tempPath + "?" + ((string[])node.Tag)[5];
                        if (api.ToLower().EndsWith(apiPath))
                        {
                            ApiFindInTreeList.Add((((string[])node.Parent.Parent.Tag)[2], ((string[])node.Parent.Parent.Tag)[1], api, ((string[])node.Parent.Tag)[2]));
                        }
                    }
                    else if (type == "part")
                    {
                        TreeFind(node,tempPath);
                    }
                    else
                    {
                        var path = ((string[])node.Tag)[0];
                        TreeFind(node, tempPath+"/"+ path);
                    }
                }
            }
        }
        public static void PagePositonToPart(string PageId,string PartId,string ApiPath,string PageCaption)
        {
            Page.PageWare.openPage(PageId, PageCaption);
            var tb =globalConst.MdiForm.MainToolBox[5];
            for(var i =0;i<tb.ItemCount;i++)
            {
                var obj = tb[i].Object;
                if(obj!=null)
                {
                    var objArr = (object[])obj;
                    if (objArr[0].ToString()== "control")
                    {
                        var idname = objArr[2].ToString();
                        if(idname== PartId)
                        {
                            ((mshtml.IHTMLElement)objArr[1]).scrollIntoView();
                            PagePositonToPartShine(((mshtml.IHTMLElement)objArr[1]));
                            return;
                        }
                    }
                }
            }
            MsgBox.Warning("Cannot locate the specified element!");
        }
        static System.Timers.Timer PagePositonToPartShineTimer = null;
        static bool PagePositonToPartShineTimerRunning = false;
        static mshtml.IHTMLElement PagePositonToPartShineEle = null;
        static int PagePositonToPartShineCount = 0;
        private static void PagePositonToPartShine(mshtml.IHTMLElement ele)
        {
            if(PagePositonToPartShineTimer==null)
            {
                PagePositonToPartShineTimer = new System.Timers.Timer();
            }
            if(PagePositonToPartShineTimerRunning)
            {
                PagePositonToPartShineTimer.Enabled = false;
                PagePositonToPartShineTimer.Dispose();
                PagePositonToPartShineTimer = null;
                PagePositonToPartShineTimer = new System.Timers.Timer();
                try
                {
                    PagePositonToPartShineEle.style.backgroundColor = "white";
                }
                catch
                {
                    
                }
                finally
                {
                    PagePositonToPartShineTimerRunning = false;
                }
            }
            PagePositonToPartShineTimerRunning = true;
            PagePositonToPartShineCount = 0;
            PagePositonToPartShineEle = ele;
            PagePositonToPartShineTimer.Interval = 500;
            PagePositonToPartShineTimer.Elapsed += (object sender2, System.Timers.ElapsedEventArgs e2) => {
                //MsgBox.Information(PagePositonToPartShineCount.ToString());
                if (PagePositonToPartShineCount >= 8)
                {
                    PagePositonToPartShineEle.style.backgroundColor = "white";
                    PagePositonToPartShineTimer.Enabled = false;
                    PagePositonToPartShineTimer.Dispose();
                    PagePositonToPartShineTimer = null;
                    PagePositonToPartShineTimerRunning = false;
                }
                else
                {
                    PagePositonToPartShineEle.style.backgroundColor = (PagePositonToPartShineCount % 2 == 0) ? "orange" : "white";
                    PagePositonToPartShineCount++;
                }
            };
            PagePositonToPartShineTimer.Enabled = true;
        }


        static (TreeNode, int) TreeFindTreeNodes = (null, 0);
        public static void TreeFind(TreeView treeView, string searchText)
        {
            TreeFindTreeNodes = (null, 0);
            searchText = searchText.ToLower().Trim();
            if (searchText == "") return;
            treeView.CollapseAll();
            foreach (TreeNode node in treeView.Nodes)
            {
                bool IsSonPattern = TreeFindSonOK(node, searchText);
                if (IsSonPattern)
                {
                    node.Expand();
                }
            }
            if (TreeFindTreeNodes.Item2 == 1)
            {
                globalConst.MdiForm.SiteTree.SelectedNode = TreeFindTreeNodes.Item1;
                globalConst.MdiForm.openPage();
            }
        }
        private static bool TreeFindSonOK(TreeNode Node, string searchText)
        {
            bool needExpand = false;
            foreach (TreeNode node in Node.Nodes)
            {
                if (((string[])node.Tag)[0].ToLower().IndexOf(searchText) >= 0 || ((string[])node.Tag)[1].ToLower().IndexOf(searchText) >= 0)
                {
                    node.ForeColor = Color.Red;
                    TreeFindTreeNodes = (node, TreeFindTreeNodes.Item2 + 1);
                }
                else
                {
                    node.ForeColor = Color.Black;
                }
                if (TreeFindSonOK(node, searchText) || ((string[])node.Tag)[0].ToLower().IndexOf(searchText) >= 0 || ((string[])node.Tag)[1].ToLower().IndexOf(searchText) >= 0)
                {
                    needExpand = true;
                }
            }
            if (needExpand) Node.Expand();
            return needExpand;
        }
        public static TreeNode getSiteNodeByID(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                tree.NodeGetByValue = null;
                tree.getNodebyValue(globalConst.MdiForm.SiteTree.Nodes, 2, id);
                return tree.NodeGetByValue;
            }
            catch (Exception ex)
            {
                new error(ex);
                return null;
            }
        }
        public static TreeNode getCtrlNodeByID(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                tree.NodeGetByValue = null;
                tree.getNodebyValue(globalConst.MdiForm.ControlTree.Nodes, 2, id);
                return tree.NodeGetByValue;
            }
            catch (Exception ex)
            {
                new error(ex);
                return null;
            }
        }
        public static TreeNode NodeGetByValue;
        public static void getNodebyValue(TreeNodeCollection nds, int tagI, string _value)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                int i;
                for (i = 0; i < nds.Count; i++)
                {
                    if (_value.Equals(((string[])nds[i].Tag)[tagI]))
                    {
                        NodeGetByValue = nds[i];
                        return;
                    }
                    getNodebyValue(nds[i].Nodes, tagI, _value);
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static string getPath(TreeNode nd)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                string path = ((string[])nd.Tag)[0];
                string id = ((string[])nd.Tag)[2];
                if (id.Equals("root")) return ("");
                else
                    return getPath(nd.Parent) + @"\" + path;
            }
            catch (Exception e)
            {
                new error(e);
                return "unknown";
            }
        }
        public static string getSubPath(TreeNode nd)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                string id = ((string[])nd.Tag)[2];
                if (id.Equals("root")) return ("");
                else
                    return getSubPath(nd.Parent) + @"../";
            }
            catch (Exception e)
            {
                new error(e);
                return "";
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class rdm
    {
        public static string getDataSourceID()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            Random r = new Random();
            string s = "";
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            return s;
        }

        public static string getCombID()
        {
            return GenerateComb().ToString().Replace("-", "_");
        }
        public static string getID()
        {
            //"g5pi3_ps1fi_hy2d_223552";
            return System.Guid.NewGuid().ToString().Replace("-", "_");
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            Random r = new Random();
            string s = "";
            s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            s += "_";

            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();

            s += "_";

            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            s += "_";
            DateTime d = DateTime.Now;
            string s1 = d.Hour.ToString();
            string s2 = d.Minute.ToString();
            string s3 = d.Second.ToString();
            string f = (s1.Length == 1 ? s1 = "0" + s1 : s1 = s1) + (s2.Length == 1 ? s2 = "0" + s2 : s2 = s2) + (s3.Length == 1 ? s3 = "0" + s3 : s3 = s3);
            s += f;
            return s;
        }
        /**//// <summary>
        /**//// Generate a new <see cref="Guid"/> using the comb algorithm.
        /**//// </summary>
        private static Guid GenerateComb()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build 
            //the byte string 
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;

            // Convert to a byte array   
            // Note that SQL Server is accurate to 1/300th of a 
            // millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);
            return new Guid(guidArray);
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class log
    {
        public static void Info(string info)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            if (globalConst.LogLevel > 2)
            {
                string path = globalConst.LogOutputPath + @"\dssmlog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path)) { }
                }
                FileInfo fi = new FileInfo(path);
                StreamWriter sw = fi.AppendText();
                sw.WriteLine("[DSSM]Info[" + DateTime.Now + "]:" + info);
                sw.Flush();
                sw.Close();
            }
        }
        public static void Info(string info, string other)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            if (globalConst.LogLevel > 2)
            {
                string path = globalConst.LogOutputPath + @"\dssmlog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path)) { }
                }
                FileInfo fi = new FileInfo(path);
                StreamWriter sw = fi.AppendText();
                sw.WriteLine("[DSSM]Info[" + DateTime.Now + "]{" + other + "}:" + info);
                sw.Flush();
                sw.Close();
            }
        }
        public static void Debug(string debug)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            if (globalConst.LogLevel > 1)
            {
                string path = globalConst.LogOutputPath + @"\dssmlog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path)) { }
                }
                FileInfo fi = new FileInfo(path);
                StreamWriter sw = fi.AppendText();
                sw.WriteLine("[DSSM]Debug[" + DateTime.Now + "]:" + debug);
                sw.Flush();
                sw.Close();
            }
        }
        public static void Debug(string debug, string other)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            if (globalConst.LogLevel > 1)
            {
                string path = globalConst.LogOutputPath + @"\dssmlog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path)) { }
                }
                FileInfo fi = new FileInfo(path);
                StreamWriter sw = fi.AppendText();
                sw.WriteLine("[DSSM]Debug[" + DateTime.Now + "]{" + other + "}:" + debug);
                sw.Flush();
                sw.Close();
            }
        }
        public static void Error(string error)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            if (globalConst.LogLevel > 0)
            {
                string path = globalConst.LogOutputPath + @"\dssmlog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path)) { }
                }
                FileInfo fi = new FileInfo(path);
                StreamWriter sw = fi.AppendText();
                sw.WriteLine("[DSSM]Error[" + DateTime.Now + "]:" + error);
                sw.Flush();
                sw.Close();
            }
        }
        public static void Error(string error, string other)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            if (globalConst.LogLevel > 0)
            {
                string path = globalConst.LogOutputPath + @"\dssmlog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path)) { }
                }
                FileInfo fi = new FileInfo(path);
                StreamWriter sw = fi.AppendText();
                sw.WriteLine("[DSSM]Error[" + DateTime.Now + "]{" + other + "}:" + error);
                sw.Flush();
                sw.Close();
            }
        }

    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class str
    {
        public static string GetDate(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        public static string GetDateTime(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        public static string GetDateTime()
        {
            return GetDateTime(DateTime.Now);
        }
        public static bool IsFormEleTag(string tagName, string type)
        {
            if (tagName == null) return false;
            if (type != null && (type.ToLower().Equals("button") || type.ToLower().Equals("submit"))) return false;
            tagName = tagName.ToLower();
            return "|input|select|textarea|label|span".IndexOf("|" + tagName + "|") >= 0;
        }
        public static bool IsNatural(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9_]+$");
            return reg1.IsMatch(str);
        }
        public static void StatusClear()
        {
            Application.DoEvents();
            globalConst.MdiForm.MainStatus.Text = "";
            Application.DoEvents();
        }
        public static void ShowStatus(string s)
        {
            Application.DoEvents();
            globalConst.MdiForm.MainStatus.Text = s;
            Application.DoEvents();
        }
        public static string D2DD(string s)
        {
            if (s == null) return "";
            return s.Replace("'", "''");
        }
        public static string Dot2DotDot(string s)
        {
            if (s == null) return "";
            return s.Replace("'", "''");
        }
        public static string EncodingConvert(string ins, Encoding sce, Encoding des)
        {
            try
            {
                byte[] unicodeBytes = sce.GetBytes(ins);

                // Perform the conversion from one encoding to the other.
                byte[] beBytes = Encoding.Convert(sce, des, unicodeBytes);

                // Convert the new byte[] into a char[] and then into a string.
                // This is a slightly different approach to converting to illustrate
                // the use of GetCharCount/GetChars.
                char[] beChars = new char[des.GetCharCount(beBytes, 0, beBytes.Length)];
                des.GetChars(beBytes, 0, beBytes.Length, beChars, 0);
                return new string(beChars);
            }
            catch (Exception ex)
            {
                new error(ex);
                return "";
            }
        }
        public static bool AuthOK(string url)
        {
            return true;
        }
        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AesEncrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            String key = "^#@$Fmaobb$%SDF@#maobb234efwexYz";
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string AesDecrypt_Fit(string str)
        {
            try
            {
                return AesDecrypt(str);
            }
            catch
            {
                return getDecode(str);
            }
        }
        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AesDecrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            String key = "^#@$Fmaobb$%SDF@#maobb234efwexYz";
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
        public static string getEncode(string str)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
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

            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(str.ToString());
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = kkkk;
            mobjCryptoService.IV = vvvv;
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }
        public static string getDecode(string str)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
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
        public static string getEncode2(string str)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "hkNndjhgdDF@#maobb234efwe";

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


            sTemp = "hidnahfkandi23497#$ASasdkf0";
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

            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(str.ToString());
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = kkkk;
            mobjCryptoService.IV = vvvv;
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }
        public static string getDecode2(string str)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "hkNndjhgdDF@#maobb234efwe";
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
            sTemp = "hidnahfkandi23497#$ASasdkf0";
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
        public static string getMD5(string str)
        {
            byte[] sor = Encoding.UTF8.GetBytes(str);
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(sor);
            StringBuilder strbul = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

            }
            return strbul.ToString();
        }
        public static string GetSplitValue(string para, int index)
        {
            string stri = "";
            if (para != null && !para.Equals(""))
            {
                para = str.getDecode(para);
                para = para.Replace(";", "&&&").Replace("|||", ";");
                if (para.Split(';').Length < index + 1) return "";
                stri = para.Split(';')[index].Replace("&&&", ";").Trim();
            }
            return stri;
        }
        public static string SetSplitValue(string para, int paracount, int index, string newvalue)
        {
            if (para == null || para.Equals(""))
            {
                para = "";
                for (int i = 0; i < paracount; i++)
                {
                    if (i < paracount - 1) para += "|||";
                }
                para = str.getEncode(para);
            }
            para = str.getDecode(para);
            para = para.Replace(";", "&&&").Replace("|||", ";");
            for (int i = 0; i < paracount - para.Split(';').Length; i++)
            {
                para += ";";
            }
            string[] parastrs = para.Split(';');
            string newstring = "";
            for (int i = 0; i < parastrs.Length; i++)
            {
                if (i != index)
                {
                    newstring += parastrs[i].Replace("&&&", ";").Trim();
                }
                else
                {
                    newstring += newvalue;
                }
                if (i < parastrs.Length - 1) newstring += "|||";
            }
            return str.getEncode(newstring);
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class form
    {
        public static Editor getEditorByURL(string url)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                foreach (Form fm in globalConst.MdiForm.MdiChildren)
                {
                    if (fm.Name.Equals("Editor"))
                    {
                        if (((Editor)fm).thisUrl.ToLower().Equals(url.ToLower()))
                        {
                            return ((Editor)fm);
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static Editor getEditor(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                foreach (Form fm in globalConst.MdiForm.MdiChildren)
                {
                    if (fm.Name.Equals("Editor"))
                    {
                        if (((Editor)fm).thisID.Equals(id))
                        {
                            return ((Editor)fm);
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static void OutEdited()
        {
            try
            {
                foreach (Form fm in globalConst.MdiForm.MdiChildren)
                {
                    if (fm.Name.Equals("Editor"))
                    {
                        Editor ed = (Editor)fm;
                        if (!ed.isFreeFile)
                        {
                            FileInfo fio = new FileInfo(ed.thisUrl);
                            if (!fio.LastWriteTime.Equals(ed.UrlLastModTime))
                            {
                                str.ShowStatus("正在更新外部编辑内容   " + ed.Name);
                                ed.OutEdited();
                                ed.UrlLastModTime = fio.LastWriteTime;
                                str.StatusClear();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static List<Editor> getEditors()
        {
            try
            {
                List<Editor> list = new List<Editor>();
                foreach (Form fm in globalConst.MdiForm.MdiChildren)
                {
                    if (fm.Name.Equals("Editor"))
                    {
                        list.Add((Editor)fm);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                new error(e);
                return new List<Editor>();
            }
        }
        public static bool doActive(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                foreach (Form fm in globalConst.MdiForm.MdiChildren)
                {
                    if (fm.Name.Equals("Editor"))
                    {
                        if (((Editor)fm).thisID.Equals(id))
                        {
                            fm.Activate();
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                new error(e);
                return false;
            }
        }
        public static void addFreeFileEditor(string url, string title, string id, string name, string text, bool IsFreeSaved)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                url = url.Replace(@"\\", @"\");
                Editor edr = getEditorByURL(url);
                if (edr == null)
                {
                    Editor ed = new Editor();
                    ed.MdiParent = globalConst.MdiForm;
                    ed.thisUrl = url;
                    ed.thisTitle = title;
                    ed.thisID = id;
                    ed.thisName = name;
                    if (text.StartsWith("Static"))
                    {
                        ed.Text = "Free Page *";
                    }
                    else
                    {
                        ed.Text = text + " - " + url;
                    }
                    ed.isFreeFile = true;
                    ed.isFreeFileSaved = IsFreeSaved;
                    ed.Show();
                }
                else
                {
                    edr.Activate();
                }
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static Editor addEditor(string url, string title, string id, string name, int pagetype)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                Editor ed = new Editor();
                ed.MdiParent = globalConst.MdiForm;
                ed.thisUrl = url;
                ed.thisTitle = title;
                ed.thisID = id;
                ed.thisName = name;
                ed.Text = ed.thisTitle;
                ed.isFreeFile = false;
                ed.pagetype = pagetype;
                ed.WindowState= FormWindowState.Maximized;
                ed.Show();
                new Task(()=> globalConst.MdiForm.InitLastSiteFile_Add(id, title)).Start();
                return ed;
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static void UpdateFileOpend(string EditorID, bool IsOpened)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                //init all
                if (EditorID == null)
                {
                    globalConst.MdiForm.FileOpend.Nodes.Clear();
                    TreeNode tn = new TreeNode(res._globalfunctions.GetString("c5"), 22, 22);
                    tn.Tag = "root";
                    globalConst.MdiForm.FileOpend.Nodes.Add(tn);
                    foreach (Form fm in globalConst.MdiForm.MdiChildren)
                    {
                        if (fm.Name.Equals("Editor"))
                        {
                            UpdateFileOpend(((Editor)fm).thisID, true);
                        }
                    }
                }
                else
                {
                    if (IsOpened)
                    {
                        TreeNode cnd = null;
                        foreach (TreeNode nd in globalConst.MdiForm.FileOpend.Nodes[0].Nodes)
                        {
                            if (nd.Tag.ToString().Equals(EditorID))
                            {
                                cnd = nd;
                                break;
                            }
                        }
                        Editor edr = getEditor(EditorID);

                        TreeNode tn;
                        if (edr.isFreeFile)
                        {
                            if (edr.Text.EndsWith("*"))
                                tn = new TreeNode(edr.Text, 8, 8);
                            else
                            {
                                string edrText = edr.Text;
                                edrText.Replace("/", "\\");
                                if (edrText.LastIndexOf('\\') > 0)
                                    tn = new TreeNode(edrText.Substring(edrText.LastIndexOf('\\') + 1), 8, 8);
                                else
                                    tn = new TreeNode(edrText, 8, 8);
                            }
                        }
                        else
                        {
                            switch (edr.pagetype)
                            {
                                case 0:
                                    tn = new TreeNode(edr.Text, 19, 19);
                                    break;
                                case 1:
                                    tn = new TreeNode(edr.Text, 26, 26);
                                    break;
                                case 2:
                                    tn = new TreeNode(edr.Text, 27, 27);
                                    break;
                                default:
                                    tn = new TreeNode(edr.Text, 19, 19);
                                    break;
                            }

                        }
                        tn.Tag = EditorID;
                        if (cnd == null)
                            globalConst.MdiForm.FileOpend.Nodes[0].Nodes.Add(tn);
                        else
                        {
                            cnd.Text = tn.Text;
                            cnd.Tag = tn.Tag;
                        }
                        globalConst.MdiForm.FileOpend.Nodes[0].ExpandAll();
                    }
                    else
                    {
                        foreach (TreeNode nd in globalConst.MdiForm.FileOpend.Nodes[0].Nodes)
                        {
                            if (nd.Tag.ToString().Equals(EditorID))
                            {
                                nd.Remove();
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static void closeEditor(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                int i;
                Form[] fms = globalConst.MdiForm.MdiChildren;
                for (i = 0; i < fms.Length; i++)
                {
                    if (fms[i].Name.Equals("Editor"))
                    {
                        Editor edr = (Editor)fms[i];
                        if (edr.thisID.Equals(id))
                        {
                            edr.Close();
                            fms = null;
                            return;
                        }
                    }
                }
                fms = null;
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static Editor getEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                if (globalConst.MdiForm == null) return null;
                if (globalConst.MdiForm.ActiveMdiChild != null)
                {
                    if (globalConst.MdiForm.ActiveMdiChild.Name.Equals("Editor"))
                        return (Editor)globalConst.MdiForm.ActiveMdiChild;
                }
                if (globalConst.curActiveForm == null) return null;
                if (globalConst.curActiveForm.Name.Equals("Editor"))
                {
                    int i;
                    Editor er = (Editor)globalConst.curActiveForm;
                    Form[] fms = globalConst.MdiForm.MdiChildren;
                    for (i = 0; i < fms.Length; i++)
                    {
                        if (fms[i].Name.Equals("Editor"))
                        {
                            Editor edr = (Editor)fms[i];
                            if (edr.thisID.Equals(er.thisID))
                            {
                                return edr;
                            }
                        }
                    }
                    fms = null;
                }
                return null;
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static bool IsEditorCount0()
        {
            if (globalConst.MdiForm.MdiChildren.Length > 1) return false;
            if (globalConst.MdiForm.MdiChildren.Length == 0) return true;
            if (globalConst.MdiForm.MdiChildren.Length == 1)
            {
                if (((Editor)globalConst.MdiForm.MdiChildren[0]).IsClosing) return true;
                else
                    return false;
            }
            return false;
        }
        public static int getEditorCount()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            try
            {
                int j = 0;
                int i;
                Form[] fms = globalConst.MdiForm.MdiChildren;
                for (i = 0; i < fms.Length; i++)
                {
                    if (fms[i].Name.Equals("Editor"))
                    {
                        if (!((Editor)fms[i]).IsClosing)
                        {
                            j++;
                        }
                    }
                }
                fms = null;
                return j;
            }
            catch (Exception e)
            {
                new error(e);
                return 0;
            }
        }
    }
    public class sheel
    {
        public static string ExeCommand(string commandText)
        {

            Process p = new Process();

            p.StartInfo.FileName = "cmd.exe";

            p.StartInfo.UseShellExecute = false;

            p.StartInfo.RedirectStandardInput = true;

            p.StartInfo.RedirectStandardOutput = true;

            p.StartInfo.RedirectStandardError = true;

            p.StartInfo.CreateNoWindow = true;

            string strOutput = null;

            try
            {

                p.Start();

                p.StandardInput.WriteLine(commandText);

                p.StandardInput.WriteLine("exit");

                strOutput = p.StandardOutput.ReadToEnd();

                p.WaitForExit();

                p.Close();

            }

            catch (Exception e)
            {

                strOutput = e.Message;

            }

            return strOutput;

        }
        public static void ExeSheel(string sheelName)
        {
            try
            {
                Process p = new Process();

                p.StartInfo.FileName = sheelName;

                p.StartInfo.UseShellExecute = true;

                //p.StartInfo.RedirectStandardInput = true;

                //p.StartInfo.RedirectStandardOutput = true;

                //p.StartInfo.RedirectStandardError = true;

                //p.StartInfo.CreateNoWindow = true;

                //string strOutput = null;


                p.Start();

                //p.StandardInput.WriteLine(commandText);

                //p.StandardInput.WriteLine("exit");

                //strOutput = p.StandardOutput.ReadToEnd();

                //p.WaitForExit();

                //p.Close();
            }
            catch (Exception ex)
            {
                MsgBox.Warning(ex.Message);
            }

        }
    }
}
