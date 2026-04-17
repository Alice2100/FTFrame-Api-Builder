using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using FTDPClient.functions;
using System.Collections;
using FTDPClient.consts;
using FTDPClient.database;

namespace FTDPClient.forms.control
{
    public partial class RowAll_SetCols : Form
    {
        public string MainTable=null;
        public string SelectSql = null;
        public string SetString = null;
        public string OpType = "";
        public List<string[]> OpSetList = new List<string[]>();
        public ArrayList al = new ArrayList();
        public Dictionary<string,List<string>> OriSet = new Dictionary<string,List<string>>();
        public bool IsOk=false;
        public RowAll_SetCols()
        {
            InitializeComponent();
        }

        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = res.ctl.str("RawSelCols.text");         //快速定义列
                button1.Text = res.ctl.str("RawSelCols.button1");           //确定(&O)
                checkBox1.Text = res.ctl.str("RawSelCols.selall");
                checkBox2.Text = res.ctl.str("RawSelCols.selall")+"(Ignore Default Columns)";
                string connstr = Options.GetSystemDBSetConnStr();
                var dbtype = Options.GetSystemDBSetType();
                if (connstr == null || connstr.Trim().Equals(""))
                {
                    MsgBox.Warning(res.ctl.str("RawSelCol.1"));
                    this.Close();
                    return;
                }
                List<string> defaultColumns = new List<string>();
                if (OpType == "DyValue" || OpType == "DataOp")
                {
                    if (MainTable == null)
                    {
                        if (dbtype == globalConst.DBType.MySql)
                        {
                            control.SelTable_MySql sel = new control.SelTable_MySql();
                            sel.TopTable = OriSet.Keys.ToArray();
                            sel.connstr = connstr;
                            sel.ShowDialog();
                            if (sel.tablename == null)
                            {
                                this.Close();
                                return;
                            }
                            MainTable = sel.tablename;
                        }
                        else if (dbtype == globalConst.DBType.SqlServer)
                        {
                            control.SelTable_SqlServer sel = new control.SelTable_SqlServer();
                            sel.TopTable = OriSet.Keys.ToArray();
                            sel.connstr = connstr;
                            sel.ShowDialog();
                            if (sel.tablename == null)
                            {
                                this.Close();
                                return;
                            }
                            MainTable = sel.tablename;
                        }
                        else if (dbtype == globalConst.DBType.Sqlite)
                        {
                            control.SelTable_Sqlite sel = new control.SelTable_Sqlite();
                            sel.TopTable = OriSet.Keys.ToArray();
                            sel.connstr = connstr;
                            sel.ShowDialog();
                            if (sel.tablename == null)
                            {
                                this.Close();
                                return;
                            }
                            MainTable = sel.tablename;
                        }
                    }
                    SelectSql = null;
                    defaultColumns = Adv.GetDefaultColumns(MainTable);
                }
                if (!string.IsNullOrEmpty(MainTable)) this.Text += " - " + MainTable;
                if ((MainTable == null || MainTable == "") && (SelectSql == null || SelectSql == ""))
                {
                    MsgBox.Warning(res.ctl.str("RawSelCol.2"));
                    this.Close();
                    return;
                }
                if (!string.IsNullOrWhiteSpace(SelectSql))
                {
                    SelectSql = functions.Adv.GetSqlForRemoveSameCols(dbtype, connstr, SelectSql);
                    SelectSql = functions.Adv.CodePattern(SelectSql);
                }
                List<string> SeledCols = new List<string>();
                Dictionary<string, string[]> SeledColsDesc = new Dictionary<string, string[]>();
                foreach (string[] item in al)
                {
                    string key = null;
                    if (item[0].ToLower().Trim() != "" && !SeledCols.Contains(item[0].ToLower().Trim())) key = item[0].ToLower().Trim();
                    if (key != null)
                    {
                        SeledCols.Add(key);
                        if (!SeledColsDesc.ContainsKey(key))
                        {
                            SeledColsDesc.Add(key, new string[] { item[5].Trim(), item[0].Trim() });
                        }
                    }
                    key = null;
                    if (item[5].ToLower().Trim() != "" && !SeledCols.Contains(item[5].ToLower().Trim())) key = item[5].ToLower().Trim();
                    if (key != null)
                    {
                        SeledCols.Add(key);
                        if (!SeledColsDesc.ContainsKey(key))
                        {
                            SeledColsDesc.Add(key, new string[] { item[5].Trim(), item[0].Trim() });
                        }
                    }
                }
                if (dbtype==globalConst.DBType.SqlServer)
                {
                    if ((SelectSql != null && SelectSql != ""))
                    {
                        using (SqlConnection conn = new SqlConnection(connstr))
                        {
                            conn.Open();

                            //                       Dictionary<string, string> colDesc = new Dictionary<string, string>();
                            //                       if(!string.IsNullOrWhiteSpace(MainTable))
                            //                       {
                            //                           string sqlMain = @"(select sys.columns.name,sys.types.name as typename,sys.columns.max_length,sys.columns.precision,sys.columns.scale, sys.columns.is_nullable,
                            //(select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as is_identity ,
                            //sys.columns.column_id,(select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as description 
                            //from sys.columns, sys.tables, sys.types where sys.columns.object_id = sys.tables.object_id and sys.columns.user_type_id=sys.types.user_type_id and sys.tables.name='" + MainTable + "')";
                            //                           using (SqlDataReader dr = new SqlCommand(sqlMain, conn).ExecuteReader())
                            //                           {
                            //                               while (dr.Read())
                            //                               {
                            //                                   colDesc.Add(dr.GetString(0).ToLower(), dr.IsDBNull(dr.GetOrdinal("description")) ? "" : dr.GetString(dr.GetOrdinal("description")));
                            //                               }
                            //                           }
                            //                       }

                            string sql = "select top 0 * from (" + SelectSql + ") t123456";
                            using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                            {
                                DataGridViewCheckBoxColumn colXuanZe = new DataGridViewCheckBoxColumn();
                                //colXuanZe.CellTemplate = new DataGridViewCheckBoxCell();
                                colXuanZe.Name = "";
                                dataGridView1.Columns.Add(colXuanZe);
                                colXuanZe.Width = 40;

                                DataGridViewColumn colXianShi = new DataGridViewColumn();
                                colXianShi.CellTemplate = new DataGridViewTextBoxCell();
                                colXianShi.Name = res.ctl.str("RawSelCols.1");
                                colXianShi.HeaderText = res.ctl.str("RawSelCols.2");
                                dataGridView1.Columns.Add(colXianShi);
                                colXianShi.Width = 120;

                                DataGridViewButtonColumn colShang = new DataGridViewButtonColumn();
                                //colShang.CellTemplate = new DataGridViewButtonCell();
                                colShang.Name = "";
                                colShang.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView1.Columns.Add(colShang);
                                colShang.Width = 50;

                                DataGridViewButtonColumn colXia = new DataGridViewButtonColumn();
                                //colXia.CellTemplate = new DataGridViewButtonCell();
                                colXia.Name = "";
                                colXia.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView1.Columns.Add(colXia);
                                colXia.Width = 50;

                                DataGridViewButtonColumn colDing = new DataGridViewButtonColumn();
                                //colXia.CellTemplate = new DataGridViewButtonCell();
                                colDing.Name = "";
                                colDing.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView1.Columns.Add(colDing);
                                colDing.Width = 50;

                                DataGridViewButtonColumn colDi = new DataGridViewButtonColumn();
                                //colXia.CellTemplate = new DataGridViewButtonCell();
                                colDi.Name = "";
                                colDi.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView1.Columns.Add(colDi);
                                colDi.Width = 50;


                                DataGridViewColumn col0 = new DataGridViewColumn();
                                col0.CellTemplate = new DataGridViewTextBoxCell();
                                col0.Name = res.ctl.str("RawSelCols.3");
                                col0.HeaderText = res.ctl.str("RawSelCols.4");
                                col0.DefaultCellStyle.BackColor = Color.AliceBlue;
                                col0.DefaultCellStyle.ForeColor = Color.Blue;
                                dataGridView1.Columns.Add(col0);
                                col0.Width = 160;



                                DataGridViewColumn col1 = new DataGridViewColumn();
                                col1.CellTemplate = new DataGridViewTextBoxCell();
                                col1.Name = res.ctl.str("RawSelCols.5");
                                col1.DefaultCellStyle.ForeColor = Color.Blue;
                                dataGridView1.Columns.Add(col1);
                                col1.Width = 80;

                                DataGridViewColumn colzs = new DataGridViewColumn();
                                colzs.CellTemplate = new DataGridViewTextBoxCell();
                                colzs.Name = res.ctl.str("RawSelCols.6");
                                colzs.DefaultCellStyle.ForeColor = Color.Blue;
                                dataGridView1.Columns.Add(colzs);
                                colzs.Width = 160;

                                DataGridViewColumn colzs2 = new DataGridViewColumn();
                                colzs2.CellTemplate = new DataGridViewTextBoxCell();
                                colzs2.Name = res.ctl.str("RawSelCols.7");
                                colzs2.HeaderText = res.ctl.str("RawSelCols.8");
                                dataGridView1.Columns.Add(colzs2);
                                colzs2.Width = 120;
                                var descDic = new Dictionary<string, string>();
                                for (int i = 0; i < dr.FieldCount; i++)
                                {
                                    descDic.Add(dr.GetName(i), null);
                                    string desc = "";// Adv.GetColumnDescription(dbtype, connstr, dr.GetName(i));
                                                     //string desc=(colDesc.ContainsKey(dr.GetName(i).ToLower())?colDesc[dr.GetName(i).ToLower()]:"");
                                    dataGridView1.Rows.Add(new object[] {
                                false,null,res.ctl.str("RawSelCols.9"),res.ctl.str("RawSelCols.10"),res.ctl.str("RawSelCols.11"),res.ctl.str("RawSelCols.12"),dr.GetName(i),dr.GetDataTypeName(i),desc,""
                                });
                                }
                                Adv.GetColumnDescription(dbtype, connstr, ref descDic);
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    foreach (var item in descDic)
                                    {
                                        if (item.Key.ToLower() == row.Cells[6].Value.ToString().ToLower())
                                        {
                                            row.Cells[8].Value = item.Value;
                                            break;
                                        }
                                    }
                                }
                            }
                            //colDesc.Clear();
                        }
                    }
                    else
                    {
                        using (SqlConnection conn = new SqlConnection(connstr))
                        {
                            conn.Open();
                            string sql = @"(select sys.columns.name,sys.types.name as typename,sys.columns.max_length,sys.columns.precision,sys.columns.scale, sys.columns.is_nullable,
 (select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as is_identity ,
 sys.columns.column_id,(select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as description 
 from sys.columns, sys.tables, sys.types where sys.columns.object_id = sys.tables.object_id and sys.columns.user_type_id=sys.types.user_type_id and sys.tables.name='" + MainTable + "')";

                            DataGridViewCheckBoxColumn colXuanZe = new DataGridViewCheckBoxColumn();
                            //colXuanZe.CellTemplate = new DataGridViewCheckBoxCell();
                            colXuanZe.Name = "";
                            dataGridView1.Columns.Add(colXuanZe);
                            colXuanZe.Width = 40;

                            DataGridViewColumn colXianShi = new DataGridViewColumn();
                            colXianShi.CellTemplate = new DataGridViewTextBoxCell();
                            colXianShi.Name = res.ctl.str("RawSelCols.1");
                            colXianShi.HeaderText = res.ctl.str("RawSelCols.2");
                            dataGridView1.Columns.Add(colXianShi);
                            colXianShi.Width = 120;

                            DataGridViewButtonColumn colShang = new DataGridViewButtonColumn();
                            //colShang.CellTemplate = new DataGridViewButtonCell();
                            colShang.Name = "";
                            colShang.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView1.Columns.Add(colShang);
                            colShang.Width = 50;

                            DataGridViewButtonColumn colXia = new DataGridViewButtonColumn();
                            //colXia.CellTemplate = new DataGridViewButtonCell();
                            colXia.Name = "";
                            colXia.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView1.Columns.Add(colXia);
                            colXia.Width = 50;

                            DataGridViewButtonColumn colDing = new DataGridViewButtonColumn();
                            //colXia.CellTemplate = new DataGridViewButtonCell();
                            colDing.Name = "";
                            colDing.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView1.Columns.Add(colDing);
                            colDing.Width = 50;

                            DataGridViewButtonColumn colDi = new DataGridViewButtonColumn();
                            //colXia.CellTemplate = new DataGridViewButtonCell();
                            colDi.Name = "";
                            colDi.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView1.Columns.Add(colDi);
                            colDi.Width = 50;


                            DataGridViewColumn col0 = new DataGridViewColumn();
                            col0.CellTemplate = new DataGridViewTextBoxCell();
                            col0.Name = res.ctl.str("RawSelCols.3");
                            col0.HeaderText = res.ctl.str("RawSelCols.4");
                            col0.DefaultCellStyle.BackColor = Color.AliceBlue;
                            col0.DefaultCellStyle.ForeColor = Color.Blue;
                            dataGridView1.Columns.Add(col0);
                            col0.Width = 160;



                            DataGridViewColumn col1 = new DataGridViewColumn();
                            col1.CellTemplate = new DataGridViewTextBoxCell();
                            col1.Name = res.ctl.str("RawSelCols.5");
                            col1.DefaultCellStyle.ForeColor = Color.Blue;
                            dataGridView1.Columns.Add(col1);
                            col1.Width = 80;

                            DataGridViewColumn colzs = new DataGridViewColumn();
                            colzs.CellTemplate = new DataGridViewTextBoxCell();
                            colzs.Name = res.ctl.str("RawSelCols.6");
                            colzs.DefaultCellStyle.ForeColor = Color.Blue;
                            dataGridView1.Columns.Add(colzs);
                            colzs.Width = 160;

                            DataGridViewColumn colzs2 = new DataGridViewColumn();
                            colzs2.CellTemplate = new DataGridViewTextBoxCell();
                            colzs2.Name = res.ctl.str("RawSelCols.7");
                            colzs2.HeaderText = res.ctl.str("RawSelCols.8");
                            dataGridView1.Columns.Add(colzs2);
                            colzs2.Width = 120;
                            using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    dataGridView1.Rows.Add(new object[] {
                                false,null,res.ctl.str("RawSelCols.9"),res.ctl.str("RawSelCols.10"),res.ctl.str("RawSelCols.11"),res.ctl.str("RawSelCols.12"),dr.GetString(0),dr.GetString(1),dr.IsDBNull(dr.GetOrdinal("description"))?null:dr.GetString(dr.GetOrdinal("description")),""
                                });
                                }
                            }
                        }
                    }
                }
                else if (dbtype == globalConst.DBType.MySql)
                {
                    if ((SelectSql != null && SelectSql != ""))
                    {
                        using (MySqlConnection conn = new MySqlConnection(connstr))
                        {
                            conn.Open();

                            //Dictionary<string, string> colDesc = new Dictionary<string, string>();
                            //if (!string.IsNullOrWhiteSpace(MainTable))
                            //{
                            //    string sqlMain = "show full fields from " + MainTable + "";
                            //    using (MySqlDataReader dr = new MySqlCommand(sqlMain, conn).ExecuteReader())
                            //    {
                            //        while (dr.Read())
                            //        {
                            //            colDesc.Add(dr.GetString(dr.GetOrdinal("Field")).ToLower(), dr.IsDBNull(dr.GetOrdinal("Comment")) ? "" : dr.GetString(dr.GetOrdinal("Comment")));
                            //        }
                            //    }
                            //}

                            string sql = SelectSql + " limit 1";
                            using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                            {
                                DataGridViewCheckBoxColumn colXuanZe = new DataGridViewCheckBoxColumn();
                                //colXuanZe.CellTemplate = new DataGridViewCheckBoxCell();
                                colXuanZe.Name = "";
                                dataGridView1.Columns.Add(colXuanZe);
                                colXuanZe.Width = 40;

                                DataGridViewColumn colXianShi = new DataGridViewColumn();
                                colXianShi.CellTemplate = new DataGridViewTextBoxCell();
                                colXianShi.Name = res.ctl.str("RawSelCols.1");
                                colXianShi.HeaderText = res.ctl.str("RawSelCols.2");
                                dataGridView1.Columns.Add(colXianShi);
                                colXianShi.Width = 120;

                                DataGridViewButtonColumn colShang = new DataGridViewButtonColumn();
                                //colShang.CellTemplate = new DataGridViewButtonCell();
                                colShang.Name = "";
                                colShang.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView1.Columns.Add(colShang);
                                colShang.Width = 50;

                                DataGridViewButtonColumn colXia = new DataGridViewButtonColumn();
                                //colXia.CellTemplate = new DataGridViewButtonCell();
                                colXia.Name = "";
                                colXia.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView1.Columns.Add(colXia);
                                colXia.Width = 50;

                                DataGridViewButtonColumn colDing = new DataGridViewButtonColumn();
                                //colXia.CellTemplate = new DataGridViewButtonCell();
                                colDing.Name = "";
                                colDing.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView1.Columns.Add(colDing);
                                colDing.Width = 50;

                                DataGridViewButtonColumn colDi = new DataGridViewButtonColumn();
                                //colXia.CellTemplate = new DataGridViewButtonCell();
                                colDi.Name = "";
                                colDi.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView1.Columns.Add(colDi);
                                colDi.Width = 70;


                                DataGridViewColumn col0 = new DataGridViewColumn();
                                col0.CellTemplate = new DataGridViewTextBoxCell();
                                col0.Name = res.ctl.str("RawSelCols.3");
                                col0.HeaderText = res.ctl.str("RawSelCols.4");
                                col0.DefaultCellStyle.BackColor = Color.AliceBlue;
                                col0.DefaultCellStyle.ForeColor = Color.Blue;
                                dataGridView1.Columns.Add(col0);
                                col0.Width = 160;



                                DataGridViewColumn col1 = new DataGridViewColumn();
                                col1.CellTemplate = new DataGridViewTextBoxCell();
                                col1.Name = res.ctl.str("RawSelCols.5");
                                col1.DefaultCellStyle.ForeColor = Color.Blue;
                                dataGridView1.Columns.Add(col1);
                                col1.Width = 80;

                                DataGridViewColumn colzs = new DataGridViewColumn();
                                colzs.CellTemplate = new DataGridViewTextBoxCell();
                                colzs.Name = res.ctl.str("RawSelCols.6");
                                colzs.DefaultCellStyle.ForeColor = Color.Blue;
                                dataGridView1.Columns.Add(colzs);
                                colzs.Width = 160;

                                DataGridViewColumn colzs2 = new DataGridViewColumn();
                                colzs2.CellTemplate = new DataGridViewTextBoxCell();
                                colzs2.Name = res.ctl.str("RawSelCols.7");
                                colzs2.HeaderText = res.ctl.str("RawSelCols.8");
                                dataGridView1.Columns.Add(colzs2);
                                colzs2.Width = 120;
                                var descDic = new Dictionary<string, string>();
                                for (int i = 0; i < dr.FieldCount; i++)
                                {
                                    descDic.Add(dr.GetName(i), null);
                                    string desc = "";// Adv.GetColumnDescription(dbtype, connstr, dr.GetName(i));
                                                     //string desc=(colDesc.ContainsKey(dr.GetName(i).ToLower())?colDesc[dr.GetName(i).ToLower()]:"");
                                    dataGridView1.Rows.Add(new object[] {
                                false,null,res.ctl.str("RawSelCols.9"),res.ctl.str("RawSelCols.10"),res.ctl.str("RawSelCols.11"),res.ctl.str("RawSelCols.12"),dr.GetName(i),dr.GetDataTypeName(i),desc,""
                                });
                                }
                                Adv.GetColumnDescription(dbtype, connstr, ref descDic);
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    foreach (var item in descDic)
                                    {
                                        if (item.Key.ToLower() == row.Cells[6].Value.ToString().ToLower())
                                        {
                                            row.Cells[8].Value = item.Value;
                                            break;
                                        }
                                    }
                                }
                            }
                            //colDesc.Clear();
                        }
                    }
                    else
                    {
                        using (MySqlConnection conn = new MySqlConnection(connstr))
                        {
                            conn.Open();
                            string sql = "show full fields from " + MainTable + "";
                            DataGridViewCheckBoxColumn colXuanZe = new DataGridViewCheckBoxColumn();
                            //colXuanZe.CellTemplate = new DataGridViewCheckBoxCell();
                            colXuanZe.Name = "";
                            dataGridView1.Columns.Add(colXuanZe);
                            colXuanZe.Width = 40;

                            DataGridViewColumn colXianShi = new DataGridViewColumn();
                            colXianShi.CellTemplate = new DataGridViewTextBoxCell();
                            colXianShi.Name = res.ctl.str("RawSelCols.1");
                            colXianShi.HeaderText = res.ctl.str("RawSelCols.2");
                            dataGridView1.Columns.Add(colXianShi);
                            colXianShi.Width = 120;

                            DataGridViewButtonColumn colShang = new DataGridViewButtonColumn();
                            //colShang.CellTemplate = new DataGridViewButtonCell();
                            colShang.Name = "";
                            colShang.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView1.Columns.Add(colShang);
                            colShang.Width = 50;

                            DataGridViewButtonColumn colXia = new DataGridViewButtonColumn();
                            //colXia.CellTemplate = new DataGridViewButtonCell();
                            colXia.Name = "";
                            colXia.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView1.Columns.Add(colXia);
                            colXia.Width = 50;

                            DataGridViewButtonColumn colDing = new DataGridViewButtonColumn();
                            //colXia.CellTemplate = new DataGridViewButtonCell();
                            colDing.Name = "";
                            colDing.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView1.Columns.Add(colDing);
                            colDing.Width = 50;

                            DataGridViewButtonColumn colDi = new DataGridViewButtonColumn();
                            //colXia.CellTemplate = new DataGridViewButtonCell();
                            colDi.Name = "";
                            colDi.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView1.Columns.Add(colDi);
                            colDi.Width = 80;


                            DataGridViewColumn col0 = new DataGridViewColumn();
                            col0.CellTemplate = new DataGridViewTextBoxCell();
                            col0.Name = res.ctl.str("RawSelCols.3");
                            col0.HeaderText = res.ctl.str("RawSelCols.4");
                            col0.DefaultCellStyle.BackColor = Color.AliceBlue;
                            col0.DefaultCellStyle.ForeColor = Color.Blue;
                            dataGridView1.Columns.Add(col0);
                            col0.Width = 160;



                            DataGridViewColumn col1 = new DataGridViewColumn();
                            col1.CellTemplate = new DataGridViewTextBoxCell();
                            col1.Name = res.ctl.str("RawSelCols.5");
                            col1.DefaultCellStyle.ForeColor = Color.Blue;
                            dataGridView1.Columns.Add(col1);
                            col1.Width = 110;

                            DataGridViewColumn colzs = new DataGridViewColumn();
                            colzs.CellTemplate = new DataGridViewTextBoxCell();
                            colzs.Name = res.ctl.str("RawSelCols.6");
                            colzs.DefaultCellStyle.ForeColor = Color.Blue;
                            dataGridView1.Columns.Add(colzs);
                            colzs.Width = 160;
                            DataGridViewColumn colzs2 = new DataGridViewColumn();
                            colzs2.CellTemplate = new DataGridViewTextBoxCell();
                            colzs2.Name = res.ctl.str("RawSelCols.7");
                            colzs2.HeaderText = res.ctl.str("RawSelCols.8");
                            dataGridView1.Columns.Add(colzs2);
                            colzs2.Width = 120;

                            using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    dataGridView1.Rows.Add(new object[] {
                                false,null,res.ctl.str("RawSelCols.9"),res.ctl.str("RawSelCols.10"),res.ctl.str("RawSelCols.11"),res.ctl.str("RawSelCols.12"),dr.GetString(dr.GetOrdinal("Field")),dr.GetString(dr.GetOrdinal("Type")),dr.IsDBNull(dr.GetOrdinal("Comment"))?null:dr.GetString(dr.GetOrdinal("Comment")),""
                                });
                                }
                            }
                        }
                    }
                }
                else if (dbtype == globalConst.DBType.Sqlite)
                {
                    if ((SelectSql != null && SelectSql != ""))
                    {
                        using (var conn = new DB(connstr))
                        {
                            conn.Open();
                            var zhuShiDic = new Dictionary<string, string>();
                            if (!string.IsNullOrWhiteSpace(MainTable))
                            {
                                zhuShiDic = DBFunc.GetColumnNoteForSqlite(conn, MainTable);
                            }
                            //Dictionary<string, string> colDesc = new Dictionary<string, string>();
                            //if (!string.IsNullOrWhiteSpace(MainTable))
                            //{
                            //    string sqlMain = "show full fields from " + MainTable + "";
                            //    using (MySqlDataReader dr = new MySqlCommand(sqlMain, conn).ExecuteReader())
                            //    {
                            //        while (dr.Read())
                            //        {
                            //            colDesc.Add(dr.GetString(dr.GetOrdinal("Field")).ToLower(), dr.IsDBNull(dr.GetOrdinal("Comment")) ? "" : dr.GetString(dr.GetOrdinal("Comment")));
                            //        }
                            //    }
                            //}

                            string sql = SelectSql + " limit 1";
                            using (var dr = conn.OpenRecord(sql))
                            {
                                DataGridViewCheckBoxColumn colXuanZe = new DataGridViewCheckBoxColumn();
                                //colXuanZe.CellTemplate = new DataGridViewCheckBoxCell();
                                colXuanZe.Name = "";
                                dataGridView1.Columns.Add(colXuanZe);
                                colXuanZe.Width = 40;

                                DataGridViewColumn colXianShi = new DataGridViewColumn();
                                colXianShi.CellTemplate = new DataGridViewTextBoxCell();
                                colXianShi.Name = res.ctl.str("RawSelCols.1");
                                colXianShi.HeaderText = res.ctl.str("RawSelCols.2");
                                dataGridView1.Columns.Add(colXianShi);
                                colXianShi.Width = 120;

                                DataGridViewButtonColumn colShang = new DataGridViewButtonColumn();
                                //colShang.CellTemplate = new DataGridViewButtonCell();
                                colShang.Name = "";
                                colShang.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView1.Columns.Add(colShang);
                                colShang.Width = 50;

                                DataGridViewButtonColumn colXia = new DataGridViewButtonColumn();
                                //colXia.CellTemplate = new DataGridViewButtonCell();
                                colXia.Name = "";
                                colXia.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView1.Columns.Add(colXia);
                                colXia.Width = 50;

                                DataGridViewButtonColumn colDing = new DataGridViewButtonColumn();
                                //colXia.CellTemplate = new DataGridViewButtonCell();
                                colDing.Name = "";
                                colDing.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView1.Columns.Add(colDing);
                                colDing.Width = 50;

                                DataGridViewButtonColumn colDi = new DataGridViewButtonColumn();
                                //colXia.CellTemplate = new DataGridViewButtonCell();
                                colDi.Name = "";
                                colDi.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView1.Columns.Add(colDi);
                                colDi.Width = 70;


                                DataGridViewColumn col0 = new DataGridViewColumn();
                                col0.CellTemplate = new DataGridViewTextBoxCell();
                                col0.Name = res.ctl.str("RawSelCols.3");
                                col0.HeaderText = res.ctl.str("RawSelCols.4");
                                col0.DefaultCellStyle.BackColor = Color.AliceBlue;
                                col0.DefaultCellStyle.ForeColor = Color.Blue;
                                dataGridView1.Columns.Add(col0);
                                col0.Width = 160;



                                DataGridViewColumn col1 = new DataGridViewColumn();
                                col1.CellTemplate = new DataGridViewTextBoxCell();
                                col1.Name = res.ctl.str("RawSelCols.5");
                                col1.DefaultCellStyle.ForeColor = Color.Blue;
                                dataGridView1.Columns.Add(col1);
                                col1.Width = 80;

                                DataGridViewColumn colzs = new DataGridViewColumn();
                                colzs.CellTemplate = new DataGridViewTextBoxCell();
                                colzs.Name = res.ctl.str("RawSelCols.6");
                                colzs.DefaultCellStyle.ForeColor = Color.Blue;
                                dataGridView1.Columns.Add(colzs);
                                colzs.Width = 160;

                                DataGridViewColumn colzs2 = new DataGridViewColumn();
                                colzs2.CellTemplate = new DataGridViewTextBoxCell();
                                colzs2.Name = res.ctl.str("RawSelCols.7");
                                colzs2.HeaderText = res.ctl.str("RawSelCols.8");
                                dataGridView1.Columns.Add(colzs2);
                                colzs2.Width = 120;
                                var descDic = new Dictionary<string, string>();
                                for (int i = 0; i < dr.FieldCount; i++)
                                {
                                    string desc = "";// Adv.GetColumnDescription(dbtype, connstr, dr.GetName(i));
                                                     //string desc=(colDesc.ContainsKey(dr.GetName(i).ToLower())?colDesc[dr.GetName(i).ToLower()]:"");
                                    //优先从主表取注释
                                    if (zhuShiDic.ContainsKey(dr.GetName(i).ToLower())) desc = zhuShiDic[dr.GetName(i).ToLower()];
                                    else descDic.Add(dr.GetName(i), null);
                                    dataGridView1.Rows.Add(new object[] {
                                false,null,res.ctl.str("RawSelCols.9"),res.ctl.str("RawSelCols.10"),res.ctl.str("RawSelCols.11"),res.ctl.str("RawSelCols.12"),dr.GetName(i),dr.GetDataTypeName(i),desc,""
                                });
                                }
                                Adv.GetColumnDescription(dbtype, connstr, ref descDic);
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    foreach (var item in descDic)
                                    {
                                        if (item.Key.ToLower() == row.Cells[6].Value.ToString().ToLower())
                                        {
                                            row.Cells[8].Value = item.Value;
                                            break;
                                        }
                                    }
                                }
                            }
                            //colDesc.Clear();
                        }
                    }
                    else
                    {
                        using (var conn = new DB(connstr))
                        {
                            conn.Open();
                            string sql = "PRAGMA TABLE_INFO(" + MainTable + ")";
                            var zhuShiDic = DBFunc.GetColumnNoteForSqlite(conn, MainTable);

                            DataGridViewCheckBoxColumn colXuanZe = new DataGridViewCheckBoxColumn();
                            //colXuanZe.CellTemplate = new DataGridViewCheckBoxCell();
                            colXuanZe.Name = "";
                            dataGridView1.Columns.Add(colXuanZe);
                            colXuanZe.Width = 40;

                            DataGridViewColumn colXianShi = new DataGridViewColumn();
                            colXianShi.CellTemplate = new DataGridViewTextBoxCell();
                            colXianShi.Name = res.ctl.str("RawSelCols.1");
                            colXianShi.HeaderText = res.ctl.str("RawSelCols.2");
                            dataGridView1.Columns.Add(colXianShi);
                            colXianShi.Width = 120;

                            DataGridViewButtonColumn colShang = new DataGridViewButtonColumn();
                            //colShang.CellTemplate = new DataGridViewButtonCell();
                            colShang.Name = "";
                            colShang.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView1.Columns.Add(colShang);
                            colShang.Width = 50;

                            DataGridViewButtonColumn colXia = new DataGridViewButtonColumn();
                            //colXia.CellTemplate = new DataGridViewButtonCell();
                            colXia.Name = "";
                            colXia.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView1.Columns.Add(colXia);
                            colXia.Width = 50;

                            DataGridViewButtonColumn colDing = new DataGridViewButtonColumn();
                            //colXia.CellTemplate = new DataGridViewButtonCell();
                            colDing.Name = "";
                            colDing.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView1.Columns.Add(colDing);
                            colDing.Width = 50;

                            DataGridViewButtonColumn colDi = new DataGridViewButtonColumn();
                            //colXia.CellTemplate = new DataGridViewButtonCell();
                            colDi.Name = "";
                            colDi.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridView1.Columns.Add(colDi);
                            colDi.Width = 80;


                            DataGridViewColumn col0 = new DataGridViewColumn();
                            col0.CellTemplate = new DataGridViewTextBoxCell();
                            col0.Name = res.ctl.str("RawSelCols.3");
                            col0.HeaderText = res.ctl.str("RawSelCols.4");
                            col0.DefaultCellStyle.BackColor = Color.AliceBlue;
                            col0.DefaultCellStyle.ForeColor = Color.Blue;
                            dataGridView1.Columns.Add(col0);
                            col0.Width = 160;



                            DataGridViewColumn col1 = new DataGridViewColumn();
                            col1.CellTemplate = new DataGridViewTextBoxCell();
                            col1.Name = res.ctl.str("RawSelCols.5");
                            col1.DefaultCellStyle.ForeColor = Color.Blue;
                            dataGridView1.Columns.Add(col1);
                            col1.Width = 110;

                            DataGridViewColumn colzs = new DataGridViewColumn();
                            colzs.CellTemplate = new DataGridViewTextBoxCell();
                            colzs.Name = res.ctl.str("RawSelCols.6");
                            colzs.DefaultCellStyle.ForeColor = Color.Blue;
                            dataGridView1.Columns.Add(colzs);
                            colzs.Width = 160;
                            DataGridViewColumn colzs2 = new DataGridViewColumn();
                            colzs2.CellTemplate = new DataGridViewTextBoxCell();
                            colzs2.Name = res.ctl.str("RawSelCols.7");
                            colzs2.HeaderText = res.ctl.str("RawSelCols.8");
                            dataGridView1.Columns.Add(colzs2);
                            colzs2.Width = 120;

                            using (var dr = conn.OpenRecord(sql))
                            {
                                while (dr.Read())
                                {
                                    dataGridView1.Rows.Add(new object[] {
                                false,null,res.ctl.str("RawSelCols.9"),res.ctl.str("RawSelCols.10"),res.ctl.str("RawSelCols.11"),res.ctl.str("RawSelCols.12"),dr.GetString(dr.GetOrdinal("name")),dr.GetString(dr.GetOrdinal("type")),zhuShiDic.ContainsKey(dr.GetString(dr.GetOrdinal("name")).ToLower())?zhuShiDic[dr.GetString(dr.GetOrdinal("name")).ToLower()]:"",""
                                });
                                }
                            }
                        }
                    }
                }
                else
                {
                    MsgBox.Warning(dbtype + " " + res.ctl.str("RawSelCol.6"));
                    this.Close();
                    return;
                }
                List<int> SelColRank = new List<int>();
                Dictionary<int, int> SelColRankRow = new Dictionary<int, int>();
                if(MainTable!=null && OriSet.ContainsKey(MainTable.ToLower()))
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[6].Value != null && OriSet[MainTable.ToLower()].Contains(row.Cells[6].Value.ToString().ToLower().Trim()))
                        {
                            row.Cells[0].Value = true;
                            row.DefaultCellStyle.BackColor = Color.LightBlue;
                        }
                    }
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[6].Value!=null && defaultColumns.Contains(row.Cells[6].Value.ToString().Trim()))
                    {
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                    }
                    if (SeledCols.Contains(row.Cells[res.ctl.str("RawSelCols.1")].Value?.ToString().ToLower() ?? "")
                        || SeledCols.Contains(row.Cells[res.ctl.str("RawSelCols.3")].Value?.ToString().ToLower() ?? "")
                        || SeledCols.Contains(row.Cells[res.ctl.str("RawSelCols.6")].Value?.ToString().ToLower() ?? ""))
                    {
                        row.Cells[0].Value = true;
                        row.DefaultCellStyle.BackColor = Color.LightBlue;
                        string matchText = "";
                        if (SeledCols.Contains(row.Cells[res.ctl.str("RawSelCols.1")].Value?.ToString().ToLower() ?? "")) matchText = row.Cells[res.ctl.str("RawSelCols.1")].Value.ToString().ToLower();
                        else if (SeledCols.Contains(row.Cells[res.ctl.str("RawSelCols.3")].Value?.ToString().ToLower() ?? "")) matchText = row.Cells[res.ctl.str("RawSelCols.3")].Value.ToString().ToLower();
                        else if (SeledCols.Contains(row.Cells[res.ctl.str("RawSelCols.6")].Value?.ToString().ToLower() ?? "")) matchText = row.Cells[res.ctl.str("RawSelCols.6")].Value.ToString().ToLower();
                        int matchIndex = SeledCols.IndexOf(matchText);
                        if (!SelColRank.Contains(matchIndex))
                        {
                            SelColRank.Add(matchIndex);
                            SelColRankRow.Add(matchIndex, row.Index);
                        }

                        if (SeledColsDesc.ContainsKey(row.Cells[res.ctl.str("RawSelCols.1")].Value?.ToString().ToLower() ?? "") || SeledColsDesc.ContainsKey(row.Cells[res.ctl.str("RawSelCols.3")].Value?.ToString().ToLower() ?? "") || SeledColsDesc.ContainsKey(row.Cells[res.ctl.str("RawSelCols.6")].Value?.ToString().ToLower() ?? ""))
                        {
                            if (SeledColsDesc.ContainsKey(row.Cells[res.ctl.str("RawSelCols.1")].Value?.ToString().ToLower() ?? ""))
                            {
                                row.Cells[res.ctl.str("RawSelCols.7")].Value = SeledColsDesc[row.Cells[res.ctl.str("RawSelCols.1")].Value.ToString().ToLower()][0];
                                row.Cells[res.ctl.str("RawSelCols.1")].Value = SeledColsDesc[row.Cells[res.ctl.str("RawSelCols.1")].Value.ToString().ToLower()][1];
                            }
                            else if (SeledColsDesc.ContainsKey(row.Cells[res.ctl.str("RawSelCols.3")].Value?.ToString().ToLower() ?? ""))
                            {
                                row.Cells[res.ctl.str("RawSelCols.7")].Value = SeledColsDesc[row.Cells[res.ctl.str("RawSelCols.3")].Value.ToString().ToLower()][0];
                                row.Cells[res.ctl.str("RawSelCols.1")].Value = SeledColsDesc[row.Cells[res.ctl.str("RawSelCols.3")].Value.ToString().ToLower()][1];
                            }
                            else if (SeledColsDesc.ContainsKey(row.Cells[res.ctl.str("RawSelCols.6")].Value?.ToString().ToLower() ?? ""))
                            {
                                row.Cells[res.ctl.str("RawSelCols.7")].Value = SeledColsDesc[row.Cells[res.ctl.str("RawSelCols.6")].Value.ToString().ToLower()][0];
                                row.Cells[res.ctl.str("RawSelCols.1")].Value = SeledColsDesc[row.Cells[res.ctl.str("RawSelCols.6")].Value.ToString().ToLower()][1];
                            }
                        }
                    }
                }
                SelColRank.Sort((left, right) =>
                {
                    if (left < right) return 1;
                    else if (left > right) return -1;
                    else return 0;
                });
                foreach (int key in SelColRank)
                {
                    if (SelColRankRow[key] < 0 || SelColRankRow[key] >= dataGridView1.Rows.Count) continue;
                    TopDataGridView(dataGridView1, dataGridView1.Rows[SelColRankRow[key]]);
                    foreach (int key2 in SelColRank)
                    {
                        if (SelColRankRow[key2] < SelColRankRow[key]) SelColRankRow[key2] += 1;
                        //if (key != key2 && SelColRankRow[key2] == SelColRankRow[key]) SelColRankRow[key2] = -1;
                    }
                }
            }
            catch(Exception ex)
            {
                new error(ex);
            }
        }
        private void dgvCheckState(bool check)
        {
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[0].Value = check;
                if (check)
                {
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.Cells[4].Style.BackColor = Color.AliceBlue;
                }
            }
        }
        private void dgvCheckState2(bool check)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if(!check)
                {
                    row.Cells[0].Value = check;
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.Cells[4].Style.BackColor = Color.AliceBlue;
                }
                else
                {
                    if (row.DefaultCellStyle.ForeColor != Color.Gray) {
                        row.Cells[0].Value = check;
                        row.DefaultCellStyle.BackColor = Color.LightBlue;
                    }
                    else
                    {
                        row.Cells[0].Value = !check;
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.Cells[4].Style.BackColor = Color.AliceBlue;
                    }
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==0)//复选框
            {
                if((bool)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[4].Style.BackColor = Color.AliceBlue;
                }
            }
            else if (e.ColumnIndex == 2)//上
            {
                UpDataGridView(dataGridView1, dataGridView1.Rows[e.RowIndex]);
            }
            else if (e.ColumnIndex == 3)//下
            {
                DnDataGridView(dataGridView1, dataGridView1.Rows[e.RowIndex]);
            }
            else if (e.ColumnIndex == 4)//顶
            {
                TopDataGridView(dataGridView1, dataGridView1.Rows[e.RowIndex]);
            }
            else if (e.ColumnIndex == 5)//底
            {
                BottomDataGridView(dataGridView1, dataGridView1.Rows[e.RowIndex]);
            }
        }
        private  void UpDataGridView(DataGridView dataGridView,DataGridViewRow row)
        {
            try
            {
                int index = row.Index;
                if (index > 0)//如果该行不是第一行
                {
                    DataGridViewRow dgvr = dataGridView.Rows[index - 1];//获取选中行的上一行
                    dataGridView.Rows.RemoveAt(index - 1);//删除原选中行的上一行
                    dataGridView.Rows.Insert((index), dgvr);//将选中行的上一行插入到选中行的后面
                    dataGridView.Rows[index - 1].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void DnDataGridView(DataGridView dataGridView, DataGridViewRow row)
        {
            try
            {
                int index = row.Index;
                if (index>=0 && index < dataGridView.Rows.Count-1)
                {
                    DataGridViewRow dgvr = dataGridView.Rows[index + 1];
                    dataGridView.Rows.RemoveAt(index + 1);
                    dataGridView.Rows.Insert((index), dgvr);
                    dataGridView.Rows[index + 1].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private  void TopDataGridView(DataGridView dataGridView, DataGridViewRow row)
        {
            try
            {
                int index = row.Index;
                if (index > 0)//如果该行不是第一行
                {
                    DataGridViewRow dgvr = dataGridView.Rows[index];//获取选中行的上一行
                    dataGridView.Rows.RemoveAt(index);//删除原选中行的上一行
                    dataGridView.Rows.Insert(0, dgvr);//将选中行的上一行插入到选中行的后面                       
                    for (int i = 0; i < dataGridView.Rows.Count; i++)//选中移动后的行
                    {
                        if (i != 0)
                        {
                            dataGridView.Rows[i].Selected = false;
                        }
                        else
                        {
                            dataGridView.Rows[i].Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private  void BottomDataGridView(DataGridView dataGridView, DataGridViewRow row)
        {
            try
            {
                int index = row.Index;
                if (index < dataGridView.Rows.Count - 1)
                {
                    DataGridViewRow dgvr = dataGridView.Rows[index];//获取选中行的上一行
                    dataGridView.Rows.RemoveAt(index);//删除原选中行的上一行
                    int nCount = dataGridView.Rows.Count;
                    dataGridView.Rows.Insert(nCount, dgvr);//将选中行的上一行插入到选中行的后面
                    for (int i = 0; i < dataGridView.Rows.Count; i++)//选中移动后的行
                    {
                        if (i != dataGridView.Rows.Count - 1)
                        {
                            dataGridView.Rows[i].Selected = false;
                        }
                        else
                        {
                            dataGridView.Rows[i].Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void SelCol_SqlServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (OpType == "DyValue" || OpType == "DataOp")
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if ((bool)row.Cells[0].EditedFormattedValue)
                    {
                        OpSetList.Add(new string[] {
                        "@"+MainTable+"."+row.Cells[6].Value.ToString(),row.Cells[6].Value.ToString(),row.Cells[8].Value?.ToString()??""
                            });
                    }
                }
                IsOk = true;
                this.Close();
                return;
            }
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            foreach (string[] item in al)
            {
                string key = item[0].ToLower().Trim();
                if (!dic.ContainsKey(key)) dic.Add(key,item);
            }
            string str = "";
            List<string> gbcols = new List<string>();
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                string col = row.Cells[6].Value.ToString();
                string lieming = col;
                if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() != "")
                {
                    lieming = row.Cells[1].Value.ToString();
                }
                if ((bool)row.Cells[0].EditedFormattedValue)
                {
                    if (dic.ContainsKey(lieming.ToLower()))
                    {
                        var item=dic[lieming.ToLower()];
                        str += "|||" + item[0].Replace("\r\n", "") + "#" + functions.str.getEncode(item[1].Replace("\r\n", "")) + "#" + item[2].Replace("\r\n", "") + "#" + item[3].Replace("\r\n", "") + "#" + item[4].Replace("\r\n", "") + "#" + item[5].Replace("\r\n", "") + "#" + item[6].Replace("\r\n", "") + "#" + item[8] + "&&&" + item[7].Replace("\r\n", "");
                    }
                    else
                    {
                        str += "|||" + lieming + "#" + functions.str.getEncode(col) + "#auto;left###" + ((row.Cells[9].Value == null || row.Cells[9].Value.ToString() == "") ? row.Cells[8].Value.ToString() : row.Cells[9].Value.ToString()) + "##0&&&";
                    }
                }
                gbcols.Add(lieming.ToLower());
            }
            foreach(string[] item in al)
            {
                //自定义的列，非在可选字段中
                if(!gbcols.Contains(item[0].ToLower().Trim()))
                {
                    str += "|||" + item[0].Replace("\r\n", "") + "#" + functions.str.getEncode(item[1].Replace("\r\n", "")) + "#" + item[2].Replace("\r\n", "") + "#" + item[3].Replace("\r\n", "") + "#" + item[4].Replace("\r\n", "") + "#" + item[5].Replace("\r\n", "") + "#" + item[6].Replace("\r\n", "") + "#" + item[8] + "&&&" + item[7].Replace("\r\n", "");
                }
            }
            if (!str.Equals("")) str = str.Substring(3);
            SetString = str;
            this.Close();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            dgvCheckState(checkBox1.Checked);
        }

        #region DataGridView1 Drag Drop
        int selectionIdx = -1;
        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            dataGridView1.EndEdit();
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Clicks < 2) && (e.Button == MouseButtons.Left))

            {
                if ((e.ColumnIndex == -1) && (e.RowIndex > -1))

                    dataGridView1.DoDragDrop(dataGridView1.Rows[e.RowIndex], DragDropEffects.Move);

            }
        }
        private int GetRowFromPoint(int x, int y)

        {
            for (int i = 0; i < dataGridView1.RowCount; i++)

            {
                Rectangle rec = dataGridView1.GetRowDisplayRectangle(i, false);

                if (dataGridView1.RectangleToScreen(rec).Contains(x, y))

                    return i;

            }
            return -1;

        }
        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            int idx = GetRowFromPoint(e.X, e.Y);

            if (idx < 0) return;

            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))

            {
                DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));

                dataGridView1.Rows.Remove(row);

                selectionIdx = idx;

                dataGridView1.Rows.Insert(idx, row);

            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (selectionIdx > -1)

            {
                dataGridView1.Rows[selectionIdx].Selected = true;

                dataGridView1.CurrentCell = dataGridView1.Rows[selectionIdx].Cells[0];
                selectionIdx = -1;

            }
        }

        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            dgvCheckState2(checkBox2.Checked);
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Visible = true;
            }
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Visible = ((bool)row.Cells[0].EditedFormattedValue);
            }
            
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Visible = !((bool)row.Cells[0].EditedFormattedValue);
            }
        }
    }
}
