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

namespace FTDPClient.forms
{
    public partial class Rule_Api : Form
    {
        public bool IsCancel = true;
        public string SetVal;
        public string SetDesc;
        public string SetApiType;
        public string PatternStr = "";
        public Rule_Api()
        {
            InitializeComponent();
        }

        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            label1.Text = res.anew.str("Rule.label1");
            button1.Text = res.anew.str("Rule.button1");
            btn_apipath.Text = res.anew.str("Rule.btn_apipath");
            string sql;
            DataGridViewColumn ColModule = new DataGridViewTextBoxColumn();
            ColModule.Name = res.ctl.str("Api_List.2");
            dataGridView1.Columns.Add(ColModule);
            ColModule.Width = 170;

            DataGridViewColumn ColJsName = new DataGridViewTextBoxColumn();
            ColJsName.Name = res.ctl.str("Api_List.3");
            dataGridView1.Columns.Add(ColJsName);
            ColJsName.Width = 110;

            DataGridViewColumn ColDec = new DataGridViewTextBoxColumn();
            ColDec.Name = res.ctl.str("Api_List.4");
            dataGridView1.Columns.Add(ColDec);
            ColDec.Width = 330;

            DataGridViewColumn ColPath = new DataGridViewTextBoxColumn();
            ColPath.Name = res.ctl.str("Api_List.5");
            dataGridView1.Columns.Add(ColPath);
            ColPath.Width = 600;

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
                        sql = "select ApiPath,PageCaption,ApiType,Mimo,DevUser from ft_ftdp_apidoc where (ApiType='List' or ApiType='DyValue' or ApiType='DataOP') order by ApiPath";
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
                                    int index = dataGridView1.Rows.Add(new string[] {
                                    PageCaption,ApiType,Mimo,ApiPath,DevUser
                                });
                                    if (PatternStr != "")
                                    {
                                        if (Mimo.IndexOf(PatternStr, StringComparison.CurrentCultureIgnoreCase) >= 0 || PatternStr.IndexOf(Mimo, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                        {
                                            dataGridView1.Rows[index].DefaultCellStyle.ForeColor = Color.Blue;
                                        }
                                    }
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
                        sql = "select ApiPath,PageCaption,ApiType,Mimo,DevUser from ft_ftdp_apidoc  where (ApiType='List' or ApiType='DyValue' or ApiType='DataOP')  order by ApiPath";
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
                                if (ApiPath.IndexOf('?') > 0)
                                {
                                    int index = dataGridView1.Rows.Add(new string[] {
                                    PageCaption,ApiType,Mimo,ApiPath,DevUser
                                });
                                    if (PatternStr != "")
                                    {
                                        if (Mimo.IndexOf(PatternStr, StringComparison.CurrentCultureIgnoreCase) >= 0 || PatternStr.IndexOf(Mimo, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                        {
                                            dataGridView1.Rows[index].DefaultCellStyle.ForeColor = Color.Blue;
                                        }
                                    }
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
                        sql = "select ApiPath,PageCaption,ApiType,Mimo,DevUser from ft_ftdp_apidoc  where (ApiType='List' or ApiType='DyValue' or ApiType='DataOP')  order by ApiPath";
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
                                if (ApiPath.IndexOf('?') > 0)
                                {
                                    int index = dataGridView1.Rows.Add(new string[] {
                                    PageCaption,ApiType,Mimo,ApiPath,DevUser
                                });
                                    if (PatternStr != "")
                                    {
                                        if (Mimo.IndexOf(PatternStr, StringComparison.CurrentCultureIgnoreCase) >= 0 || PatternStr.IndexOf(Mimo, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                        {
                                            dataGridView1.Rows[index].DefaultCellStyle.ForeColor = Color.Blue;
                                        }
                                    }
                                }
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
            SetVal = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            SetDesc = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            SetApiType = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            IsCancel = false;
            this.Close();
        }
        private void SelCol_SqlServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void btn_apipath_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.DefaultCellStyle.ForeColor != Color.Blue)
                {
                    row.Visible = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text.Trim();
            if (str != "")
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value.ToString().IndexOf(str) >= 0 || row.Cells[2].Value.ToString().IndexOf(str) >= 0 || row.Cells[3].Value.ToString().IndexOf(str) >= 0)
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Visible = true;
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)button1_Click(null, null);
        }
    }
}
