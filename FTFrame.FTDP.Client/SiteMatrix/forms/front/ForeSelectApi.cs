using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using FTDPClient.consts;
using FTDPClient.database;
using FTDPClient.functions;
using MySql.Data.MySqlClient;

namespace FTDPClient.forms
{
    public partial class ForeSelectApi : Form
    {
        public string type = null;
        public bool IsCancel = true;
        public string SetVal = null;
        public ForeSelectApi()
        {
            InitializeComponent();
        }
        private void OK_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MsgBox.Error(ex.Message);
            }
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ForeConfig_Load(object sender, EventArgs e)
        {
            this.Text += " " + type;
            string subsql = "";
            if (type == "List") subsql = "(ApiType='List')";
            else if (type == "Form_Get") subsql = "(ApiType='DyValue')";
            else if (type == "Form_Set") subsql = "(ApiType='DataOP')";
            string sql;
            DataGridViewColumn ColModule = new DataGridViewTextBoxColumn();
            ColModule.Name = res.ctl.str("Api_List.2");
            dataGridView1.Columns.Add(ColModule);
            ColModule.Width = 120;

            DataGridViewColumn ColJsName = new DataGridViewTextBoxColumn();
            ColJsName.Name = res.ctl.str("Api_List.3");
            dataGridView1.Columns.Add(ColJsName);
            ColJsName.Width = 120;

            DataGridViewColumn ColPath = new DataGridViewTextBoxColumn();
            ColPath.Name = res.ctl.str("Api_List.5");
            dataGridView1.Columns.Add(ColPath);
            ColPath.Width = 500;

            //DataGridViewColumn ColApi = new DataGridViewTextBoxColumn();
            //ColApi.Name = "Api";
            //dataGridView1.Columns.Add(ColApi);
            //ColApi.Width = 250;

            DataGridViewColumn ColDec = new DataGridViewTextBoxColumn();
            ColDec.Name = res.ctl.str("Api_List.4");
            dataGridView1.Columns.Add(ColDec);
            ColDec.Width = 250;

            DataGridViewColumn ColDevUser = new DataGridViewTextBoxColumn();
            ColDevUser.Name = res.ctl.str("Api_List.6");
            dataGridView1.Columns.Add(ColDevUser);
            ColDevUser.Width = 120;

            string connstr = Options.GetSystemDBSetConnStr_Plat();
            var dbtype = Options.GetSystemDBSetType_Plat();
            if (!string.IsNullOrWhiteSpace(connstr))
            {
                if (dbtype==globalConst.DBType.SqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(connstr))
                    {
                        conn.Open();
                        sql = "select ApiPath,PageCaption,ApiType,Mimo,DevUser from ft_ftdp_apidoc where " + subsql + " order by ApiPath";
                        using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string ApiPath = dr.GetString(dr.GetOrdinal("ApiPath"));
                                string PageCaption = dr.GetString(dr.GetOrdinal("PageCaption"));
                                string _ApiType = dr.GetString(dr.GetOrdinal("ApiType"));
                                string Mimo = dr.GetString(dr.GetOrdinal("Mimo"));
                                string DevUser = dr.GetString(dr.GetOrdinal("DevUser"));
                                string ApiType = null;
                                if (_ApiType == "List") ApiType = res.ctl.str("Api_List.7");
                                else if (_ApiType == "DyValue") ApiType = res.ctl.str("Api_List.8");
                                else if (_ApiType == "DataOP") ApiType = res.ctl.str("Api_List.9");
                                if (ApiPath.IndexOf('?') > 0)
                                {
                                    dataGridView1.Rows.Add(new string[] {
                                    PageCaption,ApiType,ApiPath,Mimo,DevUser
                                });
                                }
                                else
                                {
                                    dataGridView1.Rows.Add(new string[] {
                                    PageCaption,ApiType,ApiPath,Mimo,DevUser
                                });
                                }
                            }
                        }
                    }
                }
                else if (dbtype == globalConst.DBType.MySql)
                {
                    using (MySqlConnection conn = new MySqlConnection(connstr))
                    {
                        conn.Open();
                        sql = "select ApiPath,PageCaption,ApiType,Mimo,DevUser from ft_ftdp_apidoc  where " + subsql + "  order by ApiPath";
                        using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string ApiPath = dr.GetString(dr.GetOrdinal("ApiPath"));
                                string PageCaption = dr.GetString(dr.GetOrdinal("PageCaption"));
                                string _ApiType = dr.GetString(dr.GetOrdinal("ApiType"));
                                string Mimo = dr.GetString(dr.GetOrdinal("Mimo"));
                                string DevUser = dr.GetString(dr.GetOrdinal("DevUser"));
                                string ApiType = null;
                                if (_ApiType == "List") ApiType = res.ctl.str("Api_List.7");
                                else if (_ApiType == "DyValue") ApiType = res.ctl.str("Api_List.8");
                                else if (_ApiType == "DataOP") ApiType = res.ctl.str("Api_List.9");
                                dataGridView1.Rows.Add(new string[] {
                                    PageCaption,ApiType,ApiPath,Mimo,DevUser
                                });
                            }
                        }
                    }
                }
                else if (dbtype == globalConst.DBType.Sqlite)
                {
                    using (var conn = new DB(connstr))
                    {
                        conn.Open();
                        sql = "select ApiPath,PageCaption,ApiType,Mimo,DevUser from ft_ftdp_apidoc  where " + subsql + "  order by ApiPath";
                        using (var dr = conn.OpenRecord(sql))
                        {
                            while (dr.Read())
                            {
                                string ApiPath = dr.GetString(dr.GetOrdinal("ApiPath"));
                                string PageCaption = dr.GetString(dr.GetOrdinal("PageCaption"));
                                string _ApiType = dr.GetString(dr.GetOrdinal("ApiType"));
                                string Mimo = dr.GetString(dr.GetOrdinal("Mimo"));
                                string DevUser = dr.GetString(dr.GetOrdinal("DevUser"));
                                string ApiType = null;
                                if (_ApiType == "List") ApiType = res.ctl.str("Api_List.7");
                                else if (_ApiType == "DyValue") ApiType = res.ctl.str("Api_List.8");
                                else if (_ApiType == "DataOP") ApiType = res.ctl.str("Api_List.9");
                                dataGridView1.Rows.Add(new string[] {
                                    PageCaption,ApiType,ApiPath,Mimo,DevUser
                                });
                            }
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SetVal = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            IsCancel = false;
            this.Close();
        }

        private void ForeSelectApi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var str = textBox1.Text.Trim().ToLower();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Visible = str == ""
                    || (row.Cells[0].Value?.ToString() ?? "").ToLower().Contains(str)
                    || (row.Cells[2].Value?.ToString() ?? "").ToLower().Contains(str)
                    || (row.Cells[3].Value?.ToString() ?? "").ToLower().Contains(str)
                    || (row.Cells[4].Value?.ToString() ?? "").ToLower().Contains(str);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }
    }
}