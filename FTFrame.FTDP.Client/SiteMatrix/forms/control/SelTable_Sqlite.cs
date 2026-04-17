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
using System.Data.SqlClient;
using FTDPClient.consts;
using FTDPClient.database;

namespace FTDPClient.forms.control
{
    public partial class SelTable_Sqlite : Form
    {
        public string connstr = null;
        public string tablename = null;
        public string tabledesc = null;
        public string[] TopTable=new string[0];
        public SelTable_Sqlite()
        {
            InitializeComponent();
        }

        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            DB conn = null;
            try
            {
                DataGridViewColumn col0 = new DataGridViewColumn();
                col0.CellTemplate = new DataGridViewTextBoxCell();
                col0.Name = res.ctl.str("SelCol.2");
                col0.Width = 300;
                col0.DefaultCellStyle.BackColor = Color.AliceBlue;
                dataGridView1.Columns.Add(col0);
                DataGridViewColumn col1 = new DataGridViewColumn();
                col1.CellTemplate = new DataGridViewTextBoxCell();
                col1.Name = "Description";
                col1.Width = 320;
                col1.DefaultCellStyle.BackColor = Color.AliceBlue;
                dataGridView1.Columns.Add(col1);

                foreach (var table in TopTable)
                {
                    var i = dataGridView1.Rows.Add(new object[] { table, "[quick set]" });
                    dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                    var font = dataGridView1.Rows[i].DefaultCellStyle.Font;
                    dataGridView1.Rows[i].DefaultCellStyle.Font = new Font(Font.FontFamily, Font.Size*1.5f, FontStyle.Bold);
                }
                conn = new DB();
                conn.Open(connstr);
                //string sql = "show tables";
                string sql = "SELECT name FROM sqlite_master WHERE type='table' order by name";
                using (var dr = conn.OpenRecord(sql))
                {
                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add(new object[] { dr.GetString(0), "" });
                    }
                }
                for(var i=0;i< TopTable.Length;i++)
                {
                    var tableName = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    for (var j = TopTable.Length; j < dataGridView1.Rows.Count; j++)
                    {
                        if(tableName.ToLower()== dataGridView1.Rows[j].Cells[0].Value.ToString().ToLower())
                        {
                            dataGridView1.Rows[i].Cells[1].Value = dataGridView1.Rows[j].Cells[1].Value;
                            break;
                        }
                    }
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                functions.MsgBox.Error(ex.Message);
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                    conn = null;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            tablename = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            tabledesc = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            this.Close();
        }

        private void SelCol_MySql_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            string ss = textBox1.Text.Trim().ToLower();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Visible = (ss == "" || dataGridView1.Rows[i].Cells[0].Value.ToString().ToLower().IndexOf(ss) >= 0 || dataGridView1.Rows[i].Cells[1].Value.ToString().ToLower().IndexOf(ss) >= 0);
            }
        }
    }
}
