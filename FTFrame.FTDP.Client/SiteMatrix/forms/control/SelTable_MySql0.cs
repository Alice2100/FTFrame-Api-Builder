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
using FTDPClient.consts;

namespace FTDPClient.forms.control
{
    public partial class SelTable_MySql0 : Form
    {
        public string connstr = null;
        public string tablename = null;
        public SelTable_MySql0()
        {
            InitializeComponent();
        }

        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = null; 
            try
            {
                DataGridViewColumn col0 = new DataGridViewColumn();
                col0.CellTemplate = new DataGridViewTextBoxCell();
                col0.Name = res.ctl.str("SelCol.2");
                col0.Width = 300;
                col0.DefaultCellStyle.BackColor = Color.AliceBlue;
                dataGridView1.Columns.Add(col0);
                conn = new MySqlConnection(connstr);
                conn.Open();
                string sql = "show tables";
                using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                {
                    while(dr.Read())
                    {
                        dataGridView1.Rows.Add(new object[] { dr.GetString(0)});
                    }
                }
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
            this.Close();
        }

        private void SelCol_MySql_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
    }
}
