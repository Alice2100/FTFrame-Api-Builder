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
    public partial class Api_List : Form
    {
        public bool IsCancel = true;
        public string SetVal;
        public Api_List()
        {
            InitializeComponent();
        }

        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            this.Text = res.ctl.str("Api_List.text");			//Api调用
            label1.Text = res.ctl.str("Api_List.1");
            asDatasource.Text= res.ctl.str("AsDatasource");
            string sql;
            DataGridViewColumn ColModule = new DataGridViewTextBoxColumn();
            ColModule.Name = res.ctl.str("Api_List.2");
            dataGridView1.Columns.Add(ColModule);
            ColModule.Width =120;

            DataGridViewColumn ColJsName= new DataGridViewTextBoxColumn();
            ColJsName.Name = res.ctl.str("Api_List.3");
            dataGridView1.Columns.Add(ColJsName);
            ColJsName.Width = 120;

            DataGridViewColumn ColApi = new DataGridViewTextBoxColumn();
            ColApi.Name = "Api";
            dataGridView1.Columns.Add(ColApi);
            ColApi.Width = 250;

            DataGridViewColumn ColDec = new DataGridViewTextBoxColumn();
            ColDec.Name = res.ctl.str("Api_List.4");
            dataGridView1.Columns.Add(ColDec);
            ColDec.Width = 250;

            DataGridViewColumn ColPath = new DataGridViewTextBoxColumn();
            ColPath.Name = res.ctl.str("Api_List.5");
            dataGridView1.Columns.Add(ColPath);
            ColPath.Width = 250;

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
                        sql = "select ApiPath,PageCaption,ApiType,Mimo,DevUser from ft_ftdp_apidoc where (ApiType='List' or ApiType='DyValue') order by ApiPath";
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
                                    PageCaption,ApiType,ApiPath.Split('?')[1],Mimo,ApiPath.Split('?')[0],DevUser
                                });
                                }
                                else
                                {
                                    dataGridView1.Rows.Add(new string[] {
                                    PageCaption,ApiType,"[Custom]",Mimo,ApiPath,DevUser
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
                        sql = "select ApiPath,PageCaption,ApiType,Mimo,DevUser from ft_ftdp_apidoc  where (ApiType='List' or ApiType='DyValue')  order by ApiPath";
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
                                    PageCaption,ApiType,ApiPath.IndexOf('?')<0?"":ApiPath.Split('?')[1],Mimo,ApiPath.IndexOf('?')<0?ApiPath:ApiPath.Split('?')[0],DevUser
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
                        sql = "select ApiPath,PageCaption,ApiType,Mimo,DevUser from ft_ftdp_apidoc  where (ApiType='List' or ApiType='DyValue')  order by ApiPath";
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
                                    PageCaption,ApiType,ApiPath.IndexOf('?')<0?"":ApiPath.Split('?')[1],Mimo,ApiPath.IndexOf('?')<0?ApiPath:ApiPath.Split('?')[0],DevUser
                                });
                            }
                        }
                    }
                }
            }
            /*
            sql = "select c.name as controlname,c.caption,b.name as partname,a.partid,a.pageid,b.partxml from part_in_page a,parts b,controls c where a.partid=b.id and b.controlid=c.id and c.name in ('list','dyvalue')";
            using (OleDbDataReader dr = consts.globalConst.CurSite.SiteConn.OpenRecord(sql))
            {
                while (dr.Read())
                {
                    string ControlName = dr.GetString(0);
                    string ControlCaption = dr.GetString(1);
                    string PartName = dr.GetString(2);
                    string PartID = dr.GetString(3);
                    string PageID = dr.GetString(4);
                    string PartXml = dr.GetString(5);
                    if (ControlName == "list")
                    {
                        if (PartName == "List")
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(PartXml);
                            string APISet = Page.PageWare.getPartParamValue(doc, "APISet");
                            string[] items = APISet.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string item in items)
                            {
                                string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                                string apiname = colcfg[0];
                                string apicaption = colcfg[1];
                                TreeNode tnode = tree.getSiteNodeByID(PageID);
                                if (tnode != null)
                                {
                                    string path = tree.getPath(tnode).Replace("\\","/");
                                    dataGridView1.Rows.Add(new string[] {
                ControlCaption,"列表页",apiname,apicaption,path
            });
                                }
                            }
                        }
                    }
                    else if (ControlName == "dyvalue")
                    {
                        if (PartName == "Interface")
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(PartXml);
                            string APISet = Page.PageWare.getPartParamValue(doc, "APISet");
                            string[] items = APISet.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string item in items)
                            {
                                string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                                string apiname = colcfg[0];
                                string apicaption = colcfg[1];
                                TreeNode tnode = tree.getSiteNodeByID(PageID);
                                if (tnode != null)
                                {
                                    string path = tree.getPath(tnode).Replace("\\", "/");
                                    dataGridView1.Rows.Add(new string[] {
                ControlCaption,"数据获取",apiname,apicaption,path
            });
                                }
                            }
                        }
                    }
                }
            }
            */
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() == "[Custom]")
            {
                SetVal = "@api_" + dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() + "/[p1]/[p2]/...";

            }
            else
            {
                SetVal = "@api_" + dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() + "?" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "/[p1]/[p2]/...";
            }
            IsCancel = false;
            this.Close();
        }
        private void SelCol_SqlServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
    }
}
