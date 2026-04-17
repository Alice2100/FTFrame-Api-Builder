using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FTDPClient.consts;
using System.Data.OleDb;
using FTDPClient.functions;
using mshtml;
using System.Net;
using System.Text;
using FTDPClient.Compression;
using System.IO;
using Microsoft.Data.Sqlite;
using FTDPClient.classes;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Linq;
using FTDPClient.database;

namespace FTDPClient.forms
{
    public class DataStruct : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label stat;
        private System.ComponentModel.IContainer components;
        public string ExportType = null;

        public DataStruct()
        {
            //
            // Windows 窗体设计器支持所必需的
            //
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
            InitializeComponent();
            ApplyLanguage();
            //
            // TODO: 在 InitializeComponent 调用后添加任何构造函数代码
            //
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.stat = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // stat
            // 
            this.stat.Location = new System.Drawing.Point(40, 24);
            this.stat.Name = "stat";
            this.stat.Size = new System.Drawing.Size(388, 24);
            this.stat.TabIndex = 0;
            // 
            // DataStruct
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(466, 80);
            this.ControlBox = false;
            this.Controls.Add(this.stat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DataStruct";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exporting Structure";
            this.Load += new System.EventHandler(this.DataStruct_Load);
            this.Shown += new System.EventHandler(this.DataStruct_Shown);
            this.ResumeLayout(false);

        }
        #endregion
        private void ApplyLanguage()
        {

        }

        private void DataStruct_Shown(object sender, EventArgs e)
        {
            if (ExportType == "DataBase") ExportDataBase();
            if (ExportType == "ApiDoc") ExportApiDoc();
        }
        private void ExportApiDoc()
        {
            try
            {
                if (globalConst.CurSite.ID == null)
                {
                    MsgBox.Warning("Site Not Opend!");
                    Close();
                    return;
                }
                var dbtype = Options.GetSystemDBSetType_Plat();
                string dbconn = Options.GetSystemDBSetConnStr_Plat();
                if (dbtype==globalConst.DBType.UnKnown || string.IsNullOrWhiteSpace(dbconn))
                {
                    MsgBox.Warning("database type or connection string  not set!");
                    Close();
                    return;
                }

                var editor = new TextEditor();
                editor.Text = "Api Doc SQL";
                editor.basetext = " Select a.ApiPath,a.PageCaption,a.ApiType,a.Mimo,a.KeyDesc from ft_ftdp_apidoc a \r\n" +
                                $" where 1=1  \r\n" +
                                $" ORDER BY a.ApiPath  ";
                editor.ShowDialog();
                if (editor.cancel) return;
                var sql = editor.basetext;

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel文件(*.xlsx)|*.xlsx";
                saveFileDialog.ShowDialog();
                if (!string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                {
                    string filename = saveFileDialog.FileName;
                    var excel = new Excel();
                    bool excelCreateSuc = false;
                    try
                    {
                        if (excel.CreateSpreadsheet(filename) == true)
                        {
                            excelCreateSuc = true;
                            List<(string, string)> tables = new List<(string, string)>();
                            stat.Text = "Generating ...";
                            Application.DoEvents();
                            string oneSheet = excel.AddSheet("S001");
                            excel.CreateColumnWidth(oneSheet, 1, 1, 50);
                            excel.CreateColumnWidth(oneSheet, 2, 2, 50);
                            excel.CreateColumnWidth(oneSheet, 3, 3, 100);
                            var dataList = new List<(string ApiPath,string PageCaption,string ApiType,string Mimo,string KeyDesc)>();
                            //string sql = $"select a.*,b.Set_List_RowAll,b.Set_DyValue_ApiDefine,b.Set_DataOP_Dic from ft_ftdp_apidoc a " +
                            //    $" inner join ft_ftdp_apiset b  " +
                            //    $" on a.ApiPath=b.ApiPath  " +
                            //    $" where 1=1  " +
                            //    $" ORDER BY a.ApiPath  " +
                            //        $"";
                            //string sql = $"select a.ApiPath,a.PageCaption,a.ApiType,a.Mimo,a.KeyDesc from ft_ftdp_apidoc a " +
                            //    $" where 1=1  " +
                            //    $" ORDER BY a.ApiPath  " +
                            //        $" ";
                            if (dbtype==globalConst.DBType.SqlServer)
                            {
                                using (var conn = new SqlConnection(dbconn))
                                {
                                    conn.Open();
                                    using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                                    {
                                        while (dr.Read())
                                        {
                                            dataList.Add((dr.IsDBNull(0)?"":dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetString(1), dr.IsDBNull(2) ? "" : dr.GetString(2), dr.IsDBNull(3) ? "" : dr.GetString(3), dr.IsDBNull(4) ? "" : dr.GetString(4)));
                                        }
                                    }
                                }
                            }
                            else if (dbtype == globalConst.DBType.MySql)
                            {
                                using (var conn = new MySqlConnection(dbconn))
                                {
                                    conn.Open();
                                    using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                                    {
                                        while (dr.Read())
                                        {
                                            dataList.Add((dr.IsDBNull(0) ? "" : dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetString(1), dr.IsDBNull(2) ? "" : dr.GetString(2), dr.IsDBNull(3) ? "" : dr.GetString(3), dr.IsDBNull(4) ? "" : dr.GetString(4)));
                                        }
                                    }
                                }
                            }
                            else if (dbtype == globalConst.DBType.Sqlite)
                            {
                                using (var conn = new DB(dbconn))
                                {
                                    conn.Open();
                                    using (var dr = conn.OpenRecord(sql))
                                    {
                                        while (dr.Read())
                                        {
                                            dataList.Add((dr.IsDBNull(0) ? "" : dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetString(1), dr.IsDBNull(2) ? "" : dr.GetString(2), dr.IsDBNull(3) ? "" : dr.GetString(3), dr.IsDBNull(4) ? "" : dr.GetString(4)));
                                        }
                                    }
                                }
                            }
                            var loop = 0;
                            foreach(var data in dataList)
                            {
                                loop++;
                                if (loop % 20 == 0)
                                {
                                    stat.Text = "Generated "+ loop + "/"+ dataList.Count + " , Process is "+ (Convert.ToDecimal(100*loop)/dataList.Count).ToString("0.0")+" %";
                                    Application.DoEvents();
                                }
                                if(loop%100==0)
                                {
                                    oneSheet = excel.AddSheet("S" + (1000 + (loop/100)+1).ToString().Substring(1));
                                    excel.CreateColumnWidth(oneSheet, 1, 1, 50);
                                    excel.CreateColumnWidth(oneSheet, 2, 2, 50);
                                    excel.CreateColumnWidth(oneSheet, 3, 3, 100);
                                }
                                excel.AddRow(oneSheet, new List<string>());

                                var keyDesc = data.KeyDesc.Split(new string[] { "{;;}" }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Split(new string[] { "{::}" }, StringSplitOptions.None)).Select(r => (Key: r[0], Desc: r[1]));
                                
                                var headerCells = excel.AddHeader(oneSheet, new List<string>() { "",data.Mimo," " },r=>r==""?(uint)0: (uint)4);
                                excel.MergeCells(oneSheet, headerCells[1].CellReference, headerCells[2].CellReference);

                                excel.AddRow(oneSheet, new List<string>() { "", "所属功能",data.PageCaption }, r => r == "" ? (uint)0 : (uint)1);
                                excel.AddRow(oneSheet, new List<string>() { "", "接口描述", data.Mimo }, r => r == "" ? (uint)0 : (uint)1);
                                excel.AddRow(oneSheet, new List<string>() { "", "URL", data.ApiPath }, r => r == "" ? (uint)0 : (uint)1);
                                excel.AddRow(oneSheet, new List<string>() { "", "类型", LeiXing(data.ApiType) }, r => r == "" ? (uint)0 : (uint)1);
                                excel.AddRow(oneSheet, new List<string>() { "", "请求方式", data.ApiType== "DyValue"?"Get":"Post"}, r => r == "" ? (uint)0 : (uint)1);
                                excel.AddRow(oneSheet, new List<string>() { "", "请求类型", data.ApiType == "DyValue" ? "" : "Json"  }, r => r == "" ? (uint)0 : (uint)1);
                                excel.AddRow(oneSheet, new List<string>() { "", "返回类型", "Json" }, r => r == "" ? (uint)0 : (uint)1);
                                
                                if (data.ApiType=="List")
                                {
                                    headerCells = excel.AddHeader(oneSheet, new List<string>() { "", "请求参数", " " }, r => r == "" ? (uint)0 : (uint)4);
                                    excel.MergeCells(oneSheet, headerCells[1].CellReference, headerCells[2].CellReference);
                                    excel.AddRow(oneSheet, new List<string>() { "", "orderBy", "排序字段" }, r => r == "" ? (uint)0 : (uint)1);
                                    excel.AddRow(oneSheet, new List<string>() { "", "orderType", "排序方式" }, r => r == "" ? (uint)0 : (uint)1);
                                    //excel.AddRow(oneSheet, new List<string>() { "2", "schText", "模糊查询" }, r => r == "" ? (uint)0 : (uint)1);
                                    //excel.AddRow(oneSheet, new List<string>() { "2", "schStrict", "查询规则" }, r => r == "" ? (uint)0 : (uint)1);
                                    excel.AddRow(oneSheet, new List<string>() { "", "pageSize", "每页显示数量" }, r => r == "" ? (uint)0 : (uint)1);
                                    excel.AddRow(oneSheet, new List<string>() { "", "pageNum", "返回第几页" }, r => r == "" ? (uint)0 : (uint)1);
                                }
                                else if (data.ApiType == "DataOP")
                                {
                                    headerCells = excel.AddHeader(oneSheet, new List<string>() { "", "请求参数", " " }, r => r == "" ? (uint)0 : (uint)4);
                                    excel.MergeCells(oneSheet, headerCells[1].CellReference, headerCells[2].CellReference);
                                    foreach(var item in keyDesc)
                                    {
                                        excel.AddRow(oneSheet, new List<string>() { "", item.Key, item.Desc }, r => r == "" ? (uint)0 : (uint)1);
                                    }
                                }
                                if (data.ApiType != "DataOP")
                                {
                                    headerCells = excel.AddHeader(oneSheet, new List<string>() { "", "返回数据", " " }, r => r == "" ? (uint)0 : (uint)4);
                                    excel.MergeCells(oneSheet, headerCells[1].CellReference, headerCells[2].CellReference);
                                    foreach (var item in keyDesc)
                                    {
                                        excel.AddRow(oneSheet, new List<string>() { "", item.Key, item.Desc }, r => r == "" ? (uint)0 : (uint)1);
                                    }
                                }
                                headerCells = excel.AddHeader(oneSheet, new List<string>() { "", "状态码", " " }, r => r == "" ? (uint)0 : (uint)4);
                                excel.MergeCells(oneSheet, headerCells[1].CellReference, headerCells[2].CellReference);
                                excel.AddRow(oneSheet, new List<string>() { "", "200", "OK" }, r => r == "" ? (uint)0 : (uint)1);
                                excel.AddRow(oneSheet, new List<string>() { "", "401", "Unauthorized" }, r => r == "" ? (uint)0 : (uint)1);
                                
                            }
                            
                        }
                    }
                    finally
                    {
                        if (excelCreateSuc) excel.CloseSpreadsheet();
                    }
                }
                MsgBox.Information("Export Successfully");
            }
            catch (Exception ex)
            {
                new error(ex);
            }
            finally
            {
                Close();
            }
            string LeiXing(string apitype)
            {
                switch(apitype)
                {
                    case "List":return "列表";
                    case "DyValue": return "详情";
                    case "DataOP": return "操作";
                    default:return "";
                }
            }
        }
        private void ExportDataBase()
        {
            try
            {
                if (globalConst.CurSite.ID == null)
                {
                    MsgBox.Warning("Site Not Opend!");
                    Close();
                    return;
                }
                var dbtype = Options.GetSystemDBSetType();
                string dbconn = Options.GetSystemDBSetConnStr();
                if (dbtype == globalConst.DBType.UnKnown || string.IsNullOrWhiteSpace(dbconn))
                {
                    MsgBox.Warning("database type or connection string  not set!");
                    Close();
                    return;
                }
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel文件(*.xlsx)|*.xlsx";
                saveFileDialog.ShowDialog();
                if (!string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                {
                    string filename = saveFileDialog.FileName;
                    var excel = new Excel();
                    bool excelCreateSuc = false;
                    try
                    {
                        if (excel.CreateSpreadsheet(filename) == true)
                        {
                            excelCreateSuc = true;
                            List<(string, string)> tables = new List<(string, string)>();
                            stat.Text = "Geting Tables ...";
                            Application.DoEvents();
                            string oneSheet = excel.AddSheet("Tables");
                            excel.CreateColumnWidth(oneSheet, 1, 1, 50);
                            excel.CreateColumnWidth(oneSheet, 2, 2, 50);
                            excel.AddHeader(oneSheet, new List<string>() { "TableName", "Comment" },r=>4u);
                            if (dbtype==globalConst.DBType.SqlServer)
                            {
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
                                using (var conn = new SqlConnection(dbconn))
                                {
                                    conn.Open();
                                    using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                                    {
                                        while (dr.Read())
                                        {
                                            tables.Add((dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetValue(1).ToString()));
                                            excel.AddRow(oneSheet, new List<string>() { dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetValue(1).ToString() });
                                        }
                                    }
                                }
                            }
                            else if (dbtype == globalConst.DBType.MySql)
                            {
                                string sql = "SELECT TABLE_NAME,TABLE_COMMENT FROM information_schema.TABLES WHERE table_schema=(SELECT DATABASE() as dbname limit 1)";
                                using (var conn = new MySqlConnection(dbconn))
                                {
                                    conn.Open();
                                    using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                                    {
                                        while (dr.Read())
                                        {
                                            tables.Add((dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetValue(1).ToString()));
                                            excel.AddRow(oneSheet, new List<string>() { dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetValue(1).ToString() });
                                        }
                                    }
                                }
                            }
                            else if (dbtype == globalConst.DBType.Sqlite)
                            {
                                string sql = "SELECT name,'' note FROM sqlite_master as t1 WHERE type='table' order by name";
                                using (var conn = new DB(dbconn))
                                {
                                    conn.Open();
                                    using (var dr = conn.OpenRecord(sql))
                                    {
                                        while (dr.Read())
                                        {
                                            tables.Add((dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetValue(1).ToString()));
                                            excel.AddRow(oneSheet, new List<string>() { dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetValue(1).ToString() });
                                        }
                                    }
                                }
                            }
                            foreach (var table in tables)
                            {
                                stat.Text = "Geting " + table.Item1 + " ...";
                                Application.DoEvents();
                                string sheetName = table.Item1 + "_" + table.Item2;
                                if (sheetName.Length > 31) sheetName = sheetName.Substring(0, 31);
                                string dSheet = excel.AddSheet(sheetName);
                                if (dbtype == globalConst.DBType.SqlServer)
                                {
                                    excel.CreateColumnWidth(dSheet, 1, 1, 30);
                                    excel.CreateColumnWidth(dSheet, 2, 2, 20);
                                    excel.CreateColumnWidth(dSheet, 3, 3, 20);
                                    excel.CreateColumnWidth(dSheet, 4, 4, 50);
                                    excel.AddHeader(dSheet, new List<string>() { "name", "typename", "max_length", "description" }, r => 4u);
                                    string sql = @"(select sys.columns.name,sys.types.name as typename,sys.columns.max_length,sys.columns.precision,sys.columns.scale, sys.columns.is_nullable,
 (select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as is_identity ,
 sys.columns.column_id,(select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as description 
 from sys.columns, sys.tables, sys.types where sys.columns.object_id = sys.tables.object_id and sys.columns.user_type_id=sys.types.user_type_id and sys.tables.name='" + table.Item1 + "')";
                                    using (var conn = new SqlConnection(dbconn))
                                    {
                                        conn.Open();
                                        using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                                        {
                                            while (dr.Read())
                                            {
                                                excel.AddRow(dSheet, new List<string>() { dr.GetValue(dr.GetOrdinal("name"))?.ToString() ?? "", dr.GetValue(dr.GetOrdinal("typename"))?.ToString() ?? "", dr.GetValue(dr.GetOrdinal("max_length"))?.ToString() ?? "", dr.GetValue(dr.GetOrdinal("description"))?.ToString() ?? "" });
                                            }
                                        }
                                    }
                                }
                                else if (dbtype == globalConst.DBType.MySql)
                                {
                                    excel.CreateColumnWidth(dSheet, 1, 1, 30);
                                    excel.CreateColumnWidth(dSheet, 2, 2, 25);
                                    excel.CreateColumnWidth(dSheet, 4, 4, 50);
                                    excel.AddHeader(dSheet, new List<string>() { "Field", "Type", "Key", "Comment" }, r => 4u);
                                    string sql = "show full fields from " + table.Item1 + "";
                                    using (var conn = new MySqlConnection(dbconn))
                                    {
                                        conn.Open();
                                        using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                                        {
                                            while (dr.Read())
                                            {
                                                excel.AddRow(dSheet, new List<string>() { dr.GetValue(dr.GetOrdinal("Field"))?.ToString() ?? "", dr.GetValue(dr.GetOrdinal("Type"))?.ToString() ?? "", dr.GetValue(dr.GetOrdinal("Key"))?.ToString() ?? "", dr.GetValue(dr.GetOrdinal("Comment"))?.ToString() ?? "" });
                                            }
                                        }
                                    }
                                }
                                else if (dbtype == globalConst.DBType.Sqlite)
                                {
                                    excel.CreateColumnWidth(dSheet, 1, 1, 30);
                                    excel.CreateColumnWidth(dSheet, 2, 2, 25);
                                    excel.CreateColumnWidth(dSheet, 4, 4, 50);
                                    excel.AddHeader(dSheet, new List<string>() { "name", "type", "pk", "note" }, r => 4u);
                                    string sql = "PRAGMA TABLE_INFO(" + table.Item1 + ")";
                                    using (var conn = new DB(dbconn))
                                    {
                                        conn.Open();
                                        var zhuShiDic = DBFunc.GetColumnNoteForSqlite(conn, table.Item1);
                                        using (var dr = conn.OpenRecord(sql))
                                        {
                                            while (dr.Read())
                                            {
                                                var colName = dr.GetString(dr.GetOrdinal("name"));
                                                var colDesc = zhuShiDic.ContainsKey(colName.ToLower()) ? zhuShiDic[colName.ToLower()] : "";
                                                excel.AddRow(dSheet, new List<string>() { colName, dr.GetValue(dr.GetOrdinal("type"))?.ToString() ?? "", dr.GetValue(dr.GetOrdinal("pk"))?.ToString() ?? "", colDesc });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (excelCreateSuc) excel.CloseSpreadsheet();
                    }
                }
                MsgBox.Information("Export Successfully");
            }
            catch (Exception ex)
            {
                new error(ex);
            }
            finally
            {
                Close();
            }
        }
        private void DataStruct_Load(object sender, EventArgs e)
        {
        }
    }
}
