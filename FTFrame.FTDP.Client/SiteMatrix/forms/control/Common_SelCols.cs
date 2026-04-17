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
    public partial class Common_SelCols : Form
    {
        public string MainTable = null;
        public string SelectSql = null;
        public string SelValue = null;
        public int SelType = 0;//1 多选字段 2 排序 3 api-schStrict 
        public bool IsCancel=true;
        List<string> cols = new List<string>();
        public Common_SelCols()
        {
            InitializeComponent();
        }

        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            this.Text = res.ctl.str("Common_SelCols.Title");			//列绑定字段
            button1.Text = res.ctl.str("RawSelCols.button1");
            textBox1.Text = SelValue;
            if (SelType == 2)
            {
                checkBox1.Visible = true;
                checkBox1.Checked = true;
                var _item = SelValue.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (_item.Length > 3 && _item[3].ToLower() == "desc") checkBox1.Checked = false;
                if (_item.Length > 2) cols.Add(_item[2].ToLower());
            }
            else if (SelType == 1)
            {
                checkBox1.Visible = false;
                cols.AddRange(SelValue.ToLower().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
            }
            else if (SelType == 3)
            {
                checkBox1.Visible = false;
                //cols.AddRange(SelValue.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
            }
            string connstr = Options.GetSystemDBSetConnStr();
            var dbtype = Options.GetSystemDBSetType();
            if (connstr == null || connstr.Trim().Equals(""))
            {
                MsgBox.Warning(res.ctl.str("RawSelCol.1"));
                this.Close();
                return;
            }
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
            if (MainTable.StartsWith("@")) MainTable = MainTable.Substring(1);
            else
            {
                if (!string.IsNullOrWhiteSpace(MainTable)) MainTable = "ft_" + consts.globalConst.CurSite.ID + "_f_" + MainTable;
            }
            if (string.IsNullOrWhiteSpace(SelectSql)) SelectSql = "select * from " + MainTable;
            if (dbtype == globalConst.DBType.SqlServer)
            {
                if ((SelectSql != null && SelectSql != ""))
                {
                    using (SqlConnection conn = new SqlConnection(connstr))
                    {
                        conn.Open();
                        string sql = "select top 0 * from (" + SelectSql + ") t123456";
                        using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                        {
                            DataGridViewCheckBoxColumn colXuanZe = new DataGridViewCheckBoxColumn();
                            colXuanZe.Name = "";
                            dataGridView1.Columns.Add(colXuanZe);
                            colXuanZe.Width = 40;

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
                                dataGridView1.Rows.Add(new object[] {cols.Contains(dr.GetName(i).ToLower()),
                                dr.GetName(i),"",dr.GetDataTypeName(i)
                                });
                            }
                            Adv.GetColumnDescription(dbtype, connstr, ref descDic);
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (var item in descDic)
                                {
                                    if (item.Key.ToLower() == row.Cells[1].Value.ToString().ToLower())
                                    {
                                        row.Cells[2].Value = item.Value;
                                        break;
                                    }
                                }
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
                        string sql = SelectSql + " limit 1";
                        using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                        {
                            DataGridViewCheckBoxColumn colXuanZe = new DataGridViewCheckBoxColumn();
                            colXuanZe.Name = "";
                            dataGridView1.Columns.Add(colXuanZe);
                            colXuanZe.Width = 40;

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
                                dataGridView1.Rows.Add(new object[] {cols.Contains(dr.GetName(i).ToLower()),
                                dr.GetName(i),"",dr.GetDataTypeName(i)
                                });
                            }
                            Adv.GetColumnDescription(dbtype, connstr, ref descDic);
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (var item in descDic)
                                {
                                    if (item.Key.ToLower() == row.Cells[1].Value.ToString().ToLower())
                                    {
                                        row.Cells[2].Value = item.Value;
                                        break;
                                    }
                                }
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
                        string sql = SelectSql + " limit 1";
                        using (var dr = conn.OpenRecord(sql))
                        {
                            DataGridViewCheckBoxColumn colXuanZe = new DataGridViewCheckBoxColumn();
                            colXuanZe.Name = "";
                            dataGridView1.Columns.Add(colXuanZe);
                            colXuanZe.Width = 40;

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
                                dataGridView1.Rows.Add(new object[] {cols.Contains(dr.GetName(i).ToLower()),
                                dr.GetName(i),"",dr.GetDataTypeName(i)
                                });
                            }
                            Adv.GetColumnDescription(dbtype, connstr, ref descDic);
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (var item in descDic)
                                {
                                    if (item.Key.ToLower() == row.Cells[1].Value.ToString().ToLower())
                                    {
                                        row.Cells[2].Value = item.Value;
                                        break;
                                    }
                                }
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
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)//复选框
            {
                
            }
        }

        private void SelCol_SqlServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string selcol = "";
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if ((bool)dr.Cells[0].Value)
                {
                    selcol=dr.Cells[1].Value.ToString();
                    break;
                }
            }
            if (SelType == 2)
            {
                if (selcol != "") textBox1.Text = "order by " + selcol + " " + (checkBox1.Checked ? "asc" : "desc");
            }
        }
        private void TxtRefresh()
        {
            List<string> list = new List<string>();
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (dr.Cells[0].Value != null && (bool)dr.Cells[0].Value)
                {
                    list.Add(dr.Cells[1].Value.ToString());
                }
            }
            if (SelType == 1)
            {
                string txt = "";
                foreach (string s in list)
                {
                    txt += ";" + s;
                }
                if (txt != "") txt = txt.Substring(1);
                textBox1.Text = txt;
            }
            else if (SelType == 2)
            {
                if (list.Count > 0) textBox1.Text = "order by " + list[0] + " " + (checkBox1.Checked ? "asc" : "desc");
            }
            else if (SelType == 3)
            {
                string txt = "";
                foreach (string s in list)
                {
                    txt += ";" + s + ":txt";
                }
                if (txt != "") txt = txt.Substring(1);
                textBox1.Text = txt;
            }
        }
        bool txtRefresh = true;
        private void button1_Click(object sender, EventArgs e)
        {
            if(txtRefresh)TxtRefresh();
            SelValue = textBox1.Text;
            IsCancel = false;
            this.Close();
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            txtRefresh = !txtRefresh;
        }
    }
}
