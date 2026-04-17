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
using System.Data.OleDb;
using System.Xml;
using FTDPClient.consts;
using FTDPClient.database;

namespace FTDPClient.forms.control
{
    public partial class Api_List_DataOP : Form
    {
        public bool IsCancel = true;
        public string SetVal;
        public Api_List_DataOP()
        {
            InitializeComponent();
        }

        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            label1.Text = res.anew.str("ApiList.label1");
            string sql;
            DataGridViewColumn ColModule = new DataGridViewTextBoxColumn();
            ColModule.Name = res.ctl.str("Api_List.2");
            dataGridView1.Columns.Add(ColModule);
            ColModule.Width =160;

            DataGridViewColumn ColJsName= new DataGridViewTextBoxColumn();
            ColJsName.Name = res.ctl.str("Api_List.3");
            dataGridView1.Columns.Add(ColJsName);
            ColJsName.Width = 110;

            DataGridViewColumn ColApi = new DataGridViewTextBoxColumn();
            ColApi.Name = "Api";
            dataGridView1.Columns.Add(ColApi);
            ColApi.Width = 440;

            DataGridViewColumn ColDec = new DataGridViewTextBoxColumn();
            ColDec.Name = res.ctl.str("Api_List.4");
            dataGridView1.Columns.Add(ColDec);
            ColDec.Width = 250;

            DataGridViewColumn ColDevUser = new DataGridViewTextBoxColumn();
            ColDevUser.Name = res.ctl.str("Api_List.6");
            dataGridView1.Columns.Add(ColDevUser);
            ColDevUser.Width = 120;

            DataGridViewButtonColumn ColBtn = new DataGridViewButtonColumn();
            ColBtn.Name = res.anew.str("ApiList.ColBtn");
            dataGridView1.Columns.Add(ColBtn);
            ColBtn.Width = 80;

            string connstr = Options.GetSystemDBSetConnStr_Plat();
            var dbtype = Options.GetSystemDBSetType_Plat();
            if (!string.IsNullOrWhiteSpace(connstr))
            {
                if (dbtype==globalConst.DBType.SqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(connstr))
                    {
                        conn.Open();
                        sql = "select ApiPath,PageCaption,ApiType,Mimo,DevUser from ft_ftdp_apidoc where (ApiType='DataOP') order by ApiPath";
                        using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string ApiPath = dr.GetString(dr.GetOrdinal("ApiPath"));
                                string PageCaption = dr.GetString(dr.GetOrdinal("PageCaption"));
                                string _ApiType = dr.GetString(dr.GetOrdinal("ApiType"));
                                string Mimo = dr.GetString(dr.GetOrdinal("Mimo"));
                                string DevUser = dr.GetString(dr.GetOrdinal("DevUser"));
                                string ApiType = res.anew.str("ApiList.DataOP");
                                if (ApiPath.IndexOf('?') > 0)
                                {
                                    dataGridView1.Rows.Add(new string[] {
                                    PageCaption,ApiType,"@api_"+ApiPath,Mimo,DevUser,res.anew.str("ApiList.Copy")
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
                        sql = "select ApiPath,PageCaption,ApiType,Mimo,DevUser from ft_ftdp_apidoc  where (ApiType='DataOP')  order by ApiPath";
                        using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string ApiPath = dr.GetString(dr.GetOrdinal("ApiPath"));
                                string PageCaption = dr.GetString(dr.GetOrdinal("PageCaption"));
                                string Mimo = dr.GetString(dr.GetOrdinal("Mimo"));
                                string DevUser = dr.GetString(dr.GetOrdinal("DevUser"));
                                string ApiType = res.anew.str("ApiList.DataOP");
                                if (ApiPath.IndexOf('?') > 0)
                                {
                                    dataGridView1.Rows.Add(new string[] {
                                    PageCaption,ApiType,"@api_"+ApiPath,Mimo,DevUser,res.anew.str("ApiList.Copy")
                                });
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
                        sql = "select ApiPath,PageCaption,ApiType,Mimo,DevUser from ft_ftdp_apidoc  where (ApiType='DataOP')  order by ApiPath";
                        using (var dr = conn.OpenRecord(sql))
                        {
                            while (dr.Read())
                            {
                                string ApiPath = dr.GetString(dr.GetOrdinal("ApiPath"));
                                string PageCaption = dr.GetString(dr.GetOrdinal("PageCaption"));
                                string Mimo = dr.GetString(dr.GetOrdinal("Mimo"));
                                string DevUser = dr.GetString(dr.GetOrdinal("DevUser"));
                                string ApiType = res.anew.str("ApiList.DataOP");
                                if (ApiPath.IndexOf('?') > 0)
                                {
                                    dataGridView1.Rows.Add(new string[] {
                                    PageCaption,ApiType,"@api_"+ApiPath,Mimo,DevUser,res.anew.str("ApiList.Copy")
                                });
                                }
                            }
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==5)
            {
                Clipboard.SetText(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
            }
        }
        private void SelCol_SqlServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
    }
}
