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
using FTDPClient.consts;
using FTDPClient.database;

namespace FTDPClient.forms.control
{
    public partial class RowAll_SelCol : Form
    {
        public string MainTable=null;
        public string SelectSql = null;
        public string SelColName = null;
        public RowAll_SelCol()
        {
            InitializeComponent();
        }

        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            this.Text = res.ctl.str("RawSelCol.text");			//列绑定字段
            string connstr = Options.GetSystemDBSetConnStr();
            var dbtype = Options.GetSystemDBSetType();
            if (connstr == null || connstr.Trim().Equals(""))
            {
                MsgBox.Warning(res.ctl.str("RawSelCol.1"));
                this.Close();
                return;
            }
            if((MainTable==null || MainTable=="")&& (SelectSql == null || SelectSql == ""))
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
            if (dbtype == globalConst.DBType.SqlServer)
            {
                if ((SelectSql != null && SelectSql != ""))
                {
                    using (SqlConnection conn= new SqlConnection(connstr))
                    {
                        conn.Open();
                        string sql = "select top 0 * from ("+ SelectSql + ") t123456";
                        using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                        {
                            DataGridViewColumn colzs = new DataGridViewColumn();
                            colzs.CellTemplate = new DataGridViewTextBoxCell();
                            colzs.Name = res.ctl.str("RawSelCol.3");
                            dataGridView1.Columns.Add(colzs);
                            colzs.Width = 160;
                            DataGridViewColumn col0 = new DataGridViewColumn();
                            col0.CellTemplate = new DataGridViewTextBoxCell();
                            col0.Name = res.ctl.str("RawSelCol.4");
                            col0.DefaultCellStyle.BackColor = Color.AliceBlue;
                            dataGridView1.Columns.Add(col0);
                            col0.Width = 160;
                            DataGridViewColumn col1 = new DataGridViewColumn();
                            col1.CellTemplate = new DataGridViewTextBoxCell();
                            col1.Name = res.ctl.str("RawSelCol.5");
                            dataGridView1.Columns.Add(col1);
                            col1.Width = 160;
                            var descDic = new Dictionary<string, string>();
                            for (int i=0;i<dr.FieldCount;i++)
                            {
                                descDic.Add(dr.GetName(i), null);
                                dataGridView1.Rows.Add(new string[] {
                                "",dr.GetName(i),dr.GetDataTypeName(i)
                                });
                            }
                            Adv.GetColumnDescription(dbtype, connstr, ref descDic);
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (var item in descDic)
                                {
                                    if (item.Key.ToLower() == row.Cells[1].Value?.ToString().ToLower())
                                    {
                                        row.Cells[0].Value = item.Value;
                                        break;
                                    }
                                }
                            }
                        }
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
                        SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader();
                        BindingSource bs = new BindingSource();
                        bs.DataSource = dr;
                        dataGridView2.DataSource = bs;
                        dr.Close();
                        DataGridViewColumn col0 = new DataGridViewColumn();
                        col0.CellTemplate = new DataGridViewTextBoxCell();
                        col0.Name = dataGridView2.Columns[dataGridView2.Columns.Count - 1].Name;
                        dataGridView1.Columns.Add(col0);
                        for (int i = 0; i < dataGridView2.Columns.Count - 1; i++)
                        {
                            DataGridViewTextBoxColumn colx = new DataGridViewTextBoxColumn();
                            colx.Name = dataGridView2.Columns[i].Name;
                            if (i == 0) { colx.DefaultCellStyle.BackColor = Color.AliceBlue; colx.Width = 180; }
                            dataGridView1.Columns.Add(colx);
                        }
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            object[] item = new object[row.Cells.Count];
                            object comment = row.Cells[row.Cells.Count - 1].Value;
                            item[0] = comment;
                            for (int i = row.Cells.Count - 1; i > 0; i--)
                            {
                                item[i] = row.Cells[i - 1].Value;
                            }
                            int index = dataGridView1.Rows.Add(item);
                        }
                    }
                }
            }
            else if (dbtype==globalConst.DBType.MySql)
            {
                if ((SelectSql != null && SelectSql != ""))
                {
                    using (MySqlConnection conn = new MySqlConnection(connstr))
                    {
                        conn.Open();
                        string sql = SelectSql+" limit 1";
                        using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                        {
                            DataGridViewColumn colzs = new DataGridViewColumn();
                            colzs.CellTemplate = new DataGridViewTextBoxCell();
                            colzs.Name = res.ctl.str("RawSelCol.3");
                            dataGridView1.Columns.Add(colzs);
                            colzs.Width = 160;
                            DataGridViewColumn col0 = new DataGridViewColumn();
                            col0.CellTemplate = new DataGridViewTextBoxCell();
                            col0.Name = res.ctl.str("RawSelCol.4");
                            col0.DefaultCellStyle.BackColor = Color.AliceBlue;
                            dataGridView1.Columns.Add(col0);
                            col0.Width = 160;
                            DataGridViewColumn col1 = new DataGridViewColumn();
                            col1.CellTemplate = new DataGridViewTextBoxCell();
                            col1.Name = res.ctl.str("RawSelCol.5");
                            dataGridView1.Columns.Add(col1);
                            col1.Width = 160;
                            var descDic = new Dictionary<string, string>();
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                descDic.Add(dr.GetName(i), null);
                                dataGridView1.Rows.Add(new string[] {
                                "",dr.GetName(i),dr.GetDataTypeName(i)
                                });
                            }
                            Adv.GetColumnDescription(dbtype, connstr, ref descDic);
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (var item in descDic)
                                {
                                    if (item.Key.ToLower() == row.Cells[1].Value?.ToString().ToLower())
                                    {
                                        row.Cells[0].Value = item.Value;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    using (MySqlConnection conn = new MySqlConnection(connstr))
                    {
                        conn.Open();
                        string sql = "show full fields from " + MainTable + "";
                        MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader();
                        BindingSource bs = new BindingSource();
                        bs.DataSource = dr;
                        dataGridView2.DataSource = bs;
                        dr.Close();
                        DataGridViewColumn col0 = new DataGridViewColumn();
                        col0.CellTemplate = new DataGridViewTextBoxCell();
                        col0.Name = dataGridView2.Columns[dataGridView2.Columns.Count - 1].Name;
                        dataGridView1.Columns.Add(col0);
                        for (int i = 0; i < dataGridView2.Columns.Count - 1; i++)
                        {
                            DataGridViewTextBoxColumn colx = new DataGridViewTextBoxColumn();
                            colx.Name = dataGridView2.Columns[i].Name;
                            if (i == 0) { colx.DefaultCellStyle.BackColor = Color.AliceBlue; colx.Width = 180; }
                            dataGridView1.Columns.Add(colx);
                        }
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            object[] item = new object[row.Cells.Count];
                            object comment = row.Cells[row.Cells.Count - 1].Value;
                            item[0] = comment;
                            for (int i = row.Cells.Count - 1; i > 0; i--)
                            {
                                item[i] = row.Cells[i - 1].Value;
                            }
                            int index = dataGridView1.Rows.Add(item);
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
                        string sql = SelectSql + " limit 1";
                        using (var dr = conn.OpenRecord(sql))
                        {
                            DataGridViewColumn colzs = new DataGridViewColumn();
                            colzs.CellTemplate = new DataGridViewTextBoxCell();
                            colzs.Name = res.ctl.str("RawSelCol.3");
                            dataGridView1.Columns.Add(colzs);
                            colzs.Width = 160;
                            DataGridViewColumn col0 = new DataGridViewColumn();
                            col0.CellTemplate = new DataGridViewTextBoxCell();
                            col0.Name = res.ctl.str("RawSelCol.4");
                            col0.DefaultCellStyle.BackColor = Color.AliceBlue;
                            dataGridView1.Columns.Add(col0);
                            col0.Width = 160;
                            DataGridViewColumn col1 = new DataGridViewColumn();
                            col1.CellTemplate = new DataGridViewTextBoxCell();
                            col1.Name = res.ctl.str("RawSelCol.5");
                            dataGridView1.Columns.Add(col1);
                            col1.Width = 160;
                            var descDic = new Dictionary<string, string>();
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                descDic.Add(dr.GetName(i), null);
                                dataGridView1.Rows.Add(new string[] {
                                "",dr.GetName(i),dr.GetDataTypeName(i)
                                });
                            }
                            Adv.GetColumnDescription(dbtype, connstr, ref descDic);
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (var item in descDic)
                                {
                                    if (item.Key.ToLower() == row.Cells[1].Value?.ToString().ToLower())
                                    {
                                        row.Cells[0].Value = item.Value;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    using (var conn = new DB(connstr))
                    {
                        conn.Open();
                        string sql = "PRAGMA TABLE_INFO(" + MainTable + ")";
                        var zhuShiDic = DBFunc.GetColumnNoteForSqlite(conn, MainTable);
                        var dr = conn.OpenRecord(sql);
                        BindingSource bs = new BindingSource();
                        bs.DataSource = dr;
                        dataGridView2.DataSource = bs;
                        dr.Close();
                        DataGridViewColumn col0 = new DataGridViewColumn();
                        col0.CellTemplate = new DataGridViewTextBoxCell();
                        col0.Name = "Note";
                        col0.Width = 180;
                        for (int i = 1; i <= dataGridView2.Columns.Count - 1; i++)
                        {
                            DataGridViewTextBoxColumn colx = new DataGridViewTextBoxColumn();
                            colx.Name = dataGridView2.Columns[i].Name;
                            if (i == 1) { colx.DefaultCellStyle.BackColor = Color.AliceBlue; colx.Width = 180; }
                            dataGridView1.Columns.Add(colx);
                        }
                        DataGridViewTextBoxColumn colx0 = new DataGridViewTextBoxColumn();
                        colx0.Name = dataGridView2.Columns[0].Name;
                        dataGridView1.Columns.Add(colx0);
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            object[] item = new object[row.Cells.Count + 1];
                            var key = row.Cells[1].Value.ToString();
                            var comment = "";
                            if (zhuShiDic.ContainsKey(key.ToLower())) comment = zhuShiDic[key.ToLower()];
                            item[0] = comment;
                            for (var i = 1; i <= row.Cells.Count - 1; i++)
                            {
                                item[i] = row.Cells[i].Value;
                            }
                            item[row.Cells.Count] = row.Cells[0].Value;
                            int index = dataGridView1.Rows.Add(item);
                        }
                    }
                }
            }
            else
            {
                MsgBox.Warning(dbtype + " "+ res.ctl.str("RawSelCol.6"));
                this.Close();
                return;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SelColName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            this.Close();
        }

        private void SelCol_SqlServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
    }
}
