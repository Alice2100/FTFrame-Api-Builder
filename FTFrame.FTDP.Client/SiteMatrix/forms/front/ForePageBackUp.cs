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
using ICSharpCode.TextEditor.Document;
using MySql.Data.MySqlClient;

namespace FTDPClient.forms
{
    public partial class ForePageBackUp : Form
    {
        public object obj = null;
        public bool IsOK = false;
        public bool IsUpdateFromBackUp = false;
        public ForePageBackUp()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ForeConfig_Load(object sender, EventArgs e)
        {
            this.Text += " - " + ((Front.FrontPage)obj).PageName + " - " + ((Front.FrontPage)obj).Caption;
            dgv.Columns[0].HeaderText = res.front.str("TempPageBackUp.dgv.0");
            dgv.Columns[1].HeaderText = res.front.str("TempPageBackUp.dgv.1");
            dgv.Columns[2].HeaderText = res.front.str("TempPageBackUp.dgv.2");
            dgv.Columns[3].HeaderText = res.front.str("TempPageBackUp.dgv.3");
            dgv.Columns[4].HeaderText = res.front.str("TempPageBackUp.dgv.4");
            InitGrid();
        }
        string getComName()
        {
            return ((Front.FrontPage)obj).PageName;
        }
        void InitGrid()
        {
            string comName = getComName();
            dgv.Rows.Clear();
            string connstr = Options.GetSystemDBSetConnStr_Plat();
            var conntype = Options.GetSystemDBSetType_Plat();
            if (conntype==globalConst.DBType.SqlServer)
            {
                using (SqlConnection db = new SqlConnection(connstr))
                {
                    db.Open();
                    string sql = "select Id,Caption,UpdateTime,Developer,IsNewest from ft_ftdp_front_page where PageName='" + comName + "' order by UpdateTime desc";
                    using (SqlDataReader dr = new SqlCommand(sql, db).ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var index = dgv.Rows.Add(comName, dr.GetString(1), dr.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss"), dr.GetString(3), res.front.str("TempPageBackUp.get"));
                            dgv.Rows[index].Tag = dr.GetInt32(0);
                        }
                    }
                }
            }
            else if (conntype == globalConst.DBType.MySql)
            {
                using (MySqlConnection db = new MySqlConnection(connstr))
                {
                    db.Open();
                    string sql = "select Id,Caption,UpdateTime,Developer,IsNewest from ft_ftdp_front_page where PageName='" + comName + "' order by UpdateTime desc";
                    using (MySqlDataReader dr = new MySqlCommand(sql, db).ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var index = dgv.Rows.Add(comName, dr.GetString(1), dr.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss"), dr.GetString(3), res.front.str("TempPageBackUp.get"));
                            dgv.Rows[index].Tag = dr.GetInt32(0);
                        }
                    }
                }
            }
            else if (conntype == globalConst.DBType.Sqlite)
            {
                using (var db = new DB(connstr))
                {
                    db.Open();
                    string sql = "select Id,Caption,UpdateTime,Developer,IsNewest from ft_ftdp_front_page where PageName='" + comName + "' order by UpdateTime desc";
                    using (var dr = db.OpenRecord(sql))
                    {
                        while (dr.Read())
                        {
                            var index = dgv.Rows.Add(comName, dr.GetString(1), dr.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss"), dr.GetString(3), res.front.str("TempPageBackUp.get"));
                            dgv.Rows[index].Tag = dr.GetInt32(0);
                        }
                    }
                }
            }
        }
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==4)
            {
                string comName = getComName();
                string connstr = Options.GetSystemDBSetConnStr_Plat();
                var conntype = Options.GetSystemDBSetType_Plat();
                int Id = int.Parse(dgv.Rows[e.RowIndex].Tag.ToString());
                if((Control.ModifierKeys & Keys.Control) == Keys.Control && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                {
                    string sql = "delete from ft_ftdp_front_page where id="+Id;
                    if (conntype==globalConst.DBType.SqlServer)
                    {
                        using (SqlConnection db = new SqlConnection(connstr))
                        {
                            db.Open();
                            new SqlCommand(sql, db).ExecuteNonQuery();
                        }
                    }
                    else if (conntype == globalConst.DBType.MySql)
                    {
                        using (MySqlConnection db = new MySqlConnection(connstr))
                        {
                            db.Open();
                            new MySqlCommand(sql, db).ExecuteNonQuery();
                        }
                    }
                    else if (conntype == globalConst.DBType.Sqlite)
                    {
                        using (var db = new DB(connstr))
                        {
                            db.Open();
                            db.execSql(sql);
                        }
                    }
                    InitGrid();
                }
                else
                {
                    string sql = "select * from ft_ftdp_front_page where id=" + Id;
                    bool hasValue = false;
                    string PageName = null;
                    string Caption = null;
                    int TempId = 0;
                    string PageCode = null;
                    string ComDefine = null;
                    string ParaDefine = null;
                    if (conntype == globalConst.DBType.SqlServer)
                    {
                        using (SqlConnection db = new SqlConnection(connstr))
                        {
                            db.Open();
                            using (SqlDataReader dr = new SqlCommand(sql, db).ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    hasValue = true;
                                    PageName = dr.GetString(dr.GetOrdinal("PageName"));
                                    Caption = dr.GetString(dr.GetOrdinal("Caption"));
                                    TempId = dr.GetInt32(dr.GetOrdinal("TempId"));
                                    PageCode = dr.GetString(dr.GetOrdinal("PageCode"));
                                    ComDefine = dr.GetString(dr.GetOrdinal("ComDefine"));
                                    ParaDefine = dr.GetString(dr.GetOrdinal("ParaDefine"));
                                }
                            }
                        }
                    }
                    else if (conntype == globalConst.DBType.MySql)
                    {
                        using (MySqlConnection db = new MySqlConnection(connstr))
                        {
                            db.Open();
                            using (MySqlDataReader dr = new MySqlCommand(sql, db).ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    hasValue = true;
                                    PageName = dr.GetString(dr.GetOrdinal("PageName"));
                                    Caption = dr.GetString(dr.GetOrdinal("Caption"));
                                    TempId = dr.GetInt32(dr.GetOrdinal("TempId"));
                                    PageCode = dr.GetString(dr.GetOrdinal("PageCode"));
                                    ComDefine = dr.GetString(dr.GetOrdinal("ComDefine"));
                                    ParaDefine = dr.GetString(dr.GetOrdinal("ParaDefine"));
                                }
                            }
                        }
                    }
                    else if (conntype == globalConst.DBType.Sqlite)
                    {
                        using (var db = new DB(connstr))
                        {
                            db.Open();
                            using (var dr = db.OpenRecord(sql))
                            {
                                if (dr.Read())
                                {
                                    hasValue = true;
                                    PageName = dr.GetString(dr.GetOrdinal("PageName"));
                                    Caption = dr.GetString(dr.GetOrdinal("Caption"));
                                    TempId = dr.GetInt32(dr.GetOrdinal("TempId"));
                                    PageCode = dr.GetString(dr.GetOrdinal("PageCode"));
                                    ComDefine = dr.GetString(dr.GetOrdinal("ComDefine"));
                                    ParaDefine = dr.GetString(dr.GetOrdinal("ParaDefine"));
                                }
                            }
                        }
                    }
                    if (hasValue)
                    {
                        sql = "delete from ft_ftdp_front_page where PageName='" + str.D2DD(PageName) + "' and IsNewest=1";
                        Adv.RemoteSqlExec(sql, conntype, connstr);
                        sql = "insert into ft_ftdp_front_page(PageName,Caption,TempId,PageCode,ComDefine,ParaDefine,UpdateTime,Developer,IsNewest)";
                        sql += "values('" + str.D2DD(PageName) + "','" + str.D2DD(Caption) + "','" + TempId + "','" + str.D2DD(PageCode) + "','" + str.D2DD(ComDefine) + "','" + str.D2DD(ParaDefine) + "','" + str.GetDateTime() + "','" + (Options.GetSystemValue("qianming") ?? "") + "',1)";
                        Adv.RemoteSqlExec(sql, conntype, connstr);
                        IsUpdateFromBackUp = true;
                    }
                    if (IsUpdateFromBackUp)
                    {
                        this.Close();
                    }
                }
            }
        }

        private void ForeBackUp_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}