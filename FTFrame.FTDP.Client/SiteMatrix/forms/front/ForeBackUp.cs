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
    public partial class ForeBackUp : Form
    {
        public string type = null;
        public object obj = null;
        public bool IsOK = false;
        public bool IsUpdateFromBackUp = false;
        public ForeBackUp()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ForeConfig_Load(object sender, EventArgs e)
        {
            if (type == "list")
            {
                this.Text += " - " + ((Front.ListCols)obj).ComName + " - " + ((Front.ListCols)obj).Caption;
            }
            else if (type == "form")
            {
                this.Text += " - " + ((Front.FormCols)obj).ComName + " - " + ((Front.FormCols)obj).Caption;
            }
            dgv.Columns[0].HeaderText = res.front.str("TempPageBackUp.dgv.0");
            dgv.Columns[1].HeaderText = res.front.str("TempPageBackUp.dgv.1");
            dgv.Columns[2].HeaderText = res.front.str("TempPageBackUp.dgv.2");
            dgv.Columns[3].HeaderText = res.front.str("TempPageBackUp.dgv.3");
            dgv.Columns[4].HeaderText = res.front.str("TempPageBackUp.dgv.4");
            InitGrid();
        }
        string getComName()
        {
            string comName = null;
            if (type == "list")
            {
                comName = ((Front.ListCols)obj).ComName;
            }
            else if (type == "form")
            {
                comName = ((Front.FormCols)obj).ComName;
            }
            return comName;
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
                    string sql = "select Id,Caption,CreateTime,Developer,IsNewest from ft_ftdp_front_"+ type + " where ComName='" + comName + "' order by CreateTime desc";
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
                    string sql = "select Id,Caption,CreateTime,Developer,IsNewest from ft_ftdp_front_" + type + " where ComName='" + comName + "' order by CreateTime desc";
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
                    string sql = "select Id,Caption,CreateTime,Developer,IsNewest from ft_ftdp_front_" + type + " where ComName='" + comName + "' order by CreateTime desc";
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
                    string sql = "delete from ft_ftdp_front_" + type + " where id="+Id;
                    if (conntype == globalConst.DBType.SqlServer)
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
                    if (type == "list")
                    {
                        string sql = "select * from ft_ftdp_front_" + type + " where id=" + Id;
                        bool hasValue = false;
                        string ComName = null;
                        string Caption = null;
                        string ApiBase = null;
                        string ApiUrl = null;
                        string Rows = null;
                        string Search = null;
                        string Buttons = null;
                        string Pager = null;
                        string InitSet = null;
                        string JsBeforeLoad = null;
                        string JsBeforeSet = null;
                        string JsAfterSet = null;
                        string CustomJs = null;
                        string CssText = null;
                        string OtherSet = null;
                        if (conntype == globalConst.DBType.SqlServer)
                        {
                            using (SqlConnection db = new SqlConnection(connstr))
                            {
                                db.Open();
                                using (SqlDataReader dr = new SqlCommand(sql, db).ExecuteReader())
                                {
                                    if(dr.Read())
                                    {
                                        hasValue = true;
                                        ComName = dr.GetString(dr.GetOrdinal("ComName"));
                                        Caption = dr.GetString(dr.GetOrdinal("Caption"));
                                        ApiBase = dr.GetString(dr.GetOrdinal("ApiBase"));
                                        ApiUrl = dr.GetString(dr.GetOrdinal("ApiUrl"));
                                        Rows = dr.GetString(dr.GetOrdinal("ColRows"));
                                        Search = dr.GetString(dr.GetOrdinal("Search"));
                                        Buttons = dr.GetString(dr.GetOrdinal("Buttons"));
                                        Pager = dr.GetString(dr.GetOrdinal("Pager"));
                                        InitSet = dr.GetString(dr.GetOrdinal("InitSet"));
                                        JsBeforeLoad = dr.GetString(dr.GetOrdinal("JsBeforeLoad"));
                                        JsBeforeSet = dr.GetString(dr.GetOrdinal("JsBeforeSet"));
                                        JsAfterSet = dr.GetString(dr.GetOrdinal("JsAfterSet"));
                                        CustomJs = dr.GetString(dr.GetOrdinal("CustomJs"));
                                        CssText = dr.GetString(dr.GetOrdinal("CssText"));
                                        OtherSet = dr.GetString(dr.GetOrdinal("OtherSet"));
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
                                        ComName = dr.GetString(dr.GetOrdinal("ComName"));
                                        Caption = dr.GetString(dr.GetOrdinal("Caption"));
                                        ApiBase = dr.GetString(dr.GetOrdinal("ApiBase"));
                                        ApiUrl = dr.GetString(dr.GetOrdinal("ApiUrl"));
                                        Rows = dr.GetString(dr.GetOrdinal("ColRows"));
                                        Search = dr.GetString(dr.GetOrdinal("Search"));
                                        Buttons = dr.GetString(dr.GetOrdinal("Buttons"));
                                        Pager = dr.GetString(dr.GetOrdinal("Pager"));
                                        InitSet = dr.GetString(dr.GetOrdinal("InitSet"));
                                        JsBeforeLoad = dr.GetString(dr.GetOrdinal("JsBeforeLoad"));
                                        JsBeforeSet = dr.GetString(dr.GetOrdinal("JsBeforeSet"));
                                        JsAfterSet = dr.GetString(dr.GetOrdinal("JsAfterSet"));
                                        CustomJs = dr.GetString(dr.GetOrdinal("CustomJs"));
                                        CssText = dr.GetString(dr.GetOrdinal("CssText"));
                                        OtherSet = dr.GetString(dr.GetOrdinal("OtherSet"));
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
                                        ComName = dr.GetString(dr.GetOrdinal("ComName"));
                                        Caption = dr.GetString(dr.GetOrdinal("Caption"));
                                        ApiBase = dr.GetString(dr.GetOrdinal("ApiBase"));
                                        ApiUrl = dr.GetString(dr.GetOrdinal("ApiUrl"));
                                        Rows = dr.GetString(dr.GetOrdinal("ColRows"));
                                        Search = dr.GetString(dr.GetOrdinal("Search"));
                                        Buttons = dr.GetString(dr.GetOrdinal("Buttons"));
                                        Pager = dr.GetString(dr.GetOrdinal("Pager"));
                                        InitSet = dr.GetString(dr.GetOrdinal("InitSet"));
                                        JsBeforeLoad = dr.GetString(dr.GetOrdinal("JsBeforeLoad"));
                                        JsBeforeSet = dr.GetString(dr.GetOrdinal("JsBeforeSet"));
                                        JsAfterSet = dr.GetString(dr.GetOrdinal("JsAfterSet"));
                                        CustomJs = dr.GetString(dr.GetOrdinal("CustomJs"));
                                        CssText = dr.GetString(dr.GetOrdinal("CssText"));
                                        OtherSet = dr.GetString(dr.GetOrdinal("OtherSet"));
                                    }
                                }
                            }
                        }
                        if (hasValue)
                        {
                            sql = "delete from front_list where ComName='" + ComName + "'";
                            globalConst.CurSite.SiteConn.execSql(sql);
                            sql = "insert into front_list(ComName,Caption,ApiBase,ApiUrl,Rows,Search,Buttons,Pager,InitSet,JsBeforeLoad,JsBeforeSet,JsAfterSet,CustomJs,CssText,OtherSet)";
                            sql += "values(";
                            sql += "'" + str.Dot2DotDot(ComName.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(Caption.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(ApiBase.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(ApiUrl.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(Rows.ToString().Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(Search.ToString().Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(Buttons.ToString().Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(Pager.ToString().Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(InitSet.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(JsBeforeLoad.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(JsBeforeSet.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(JsAfterSet.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(CustomJs.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(CssText.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(OtherSet.ToString().Trim()) + "'";
                            sql += ")";
                            globalConst.CurSite.SiteConn.execSql(sql);
                            IsUpdateFromBackUp = true;
                        }
                    }
                    else if (type == "form")
                    {
                        string sql = "select * from ft_ftdp_front_" + type + " where id=" + Id;
                        bool hasValue = false;
                        string ComName = null;
                        string Caption = null;
                        string ApiBase = null;
                        string ApiGet = null;
                        string ApiSet = null;
                        string Rows = null;
                        string Buttons = null;
                        string BindGet = null;
                        string BindSet = null;
                        string JsBeforeSubmit = null;
                        string JsAfterSubmit = null;
                        string JsBeforeGet = null;
                        string JsBeforeSet = null;
                        string JsAfterSet = null;
                        string CustomJs = null;
                        string CssText = null;
                        string CusDataDefine = null;
                        string OtherSet = null;
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
                                        ComName = dr.GetString(dr.GetOrdinal("ComName"));
                                        Caption = dr.GetString(dr.GetOrdinal("Caption"));
                                        ApiBase = dr.GetString(dr.GetOrdinal("ApiBase"));
                                        ApiGet = dr.GetString(dr.GetOrdinal("ApiGet"));
                                        ApiSet = dr.GetString(dr.GetOrdinal("ApiSet"));
                                        Rows = dr.GetString(dr.GetOrdinal("ColRows"));
                                        Buttons = dr.GetString(dr.GetOrdinal("Buttons"));
                                        BindGet = dr.GetString(dr.GetOrdinal("BindGet"));
                                        BindSet = dr.GetString(dr.GetOrdinal("BindSet"));
                                        JsBeforeSubmit = dr.GetString(dr.GetOrdinal("JsBeforeSubmit"));
                                        JsAfterSubmit = dr.GetString(dr.GetOrdinal("JsAfterSubmit"));
                                        JsBeforeGet = dr.GetString(dr.GetOrdinal("JsBeforeGet"));
                                        JsBeforeSet = dr.GetString(dr.GetOrdinal("JsBeforeSet"));
                                        JsAfterSet = dr.GetString(dr.GetOrdinal("JsAfterSet"));
                                        CustomJs = dr.GetString(dr.GetOrdinal("CustomJs"));
                                        CssText = dr.GetString(dr.GetOrdinal("CssText"));
                                        CusDataDefine = dr.GetString(dr.GetOrdinal("CusDataDefine"));
                                        OtherSet = dr.GetString(dr.GetOrdinal("OtherSet"));
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
                                        ComName = dr.GetString(dr.GetOrdinal("ComName"));
                                        Caption = dr.GetString(dr.GetOrdinal("Caption"));
                                        ApiBase = dr.GetString(dr.GetOrdinal("ApiBase"));
                                        ApiGet = dr.GetString(dr.GetOrdinal("ApiGet"));
                                        ApiSet = dr.GetString(dr.GetOrdinal("ApiSet"));
                                        Rows = dr.GetString(dr.GetOrdinal("ColRows"));
                                        Buttons = dr.GetString(dr.GetOrdinal("Buttons"));
                                        BindGet = dr.GetString(dr.GetOrdinal("BindGet"));
                                        BindSet = dr.GetString(dr.GetOrdinal("BindSet"));
                                        JsBeforeSubmit = dr.GetString(dr.GetOrdinal("JsBeforeSubmit"));
                                        JsAfterSubmit = dr.GetString(dr.GetOrdinal("JsAfterSubmit"));
                                        JsBeforeGet = dr.GetString(dr.GetOrdinal("JsBeforeGet"));
                                        JsBeforeSet = dr.GetString(dr.GetOrdinal("JsBeforeSet"));
                                        JsAfterSet = dr.GetString(dr.GetOrdinal("JsAfterSet"));
                                        CustomJs = dr.GetString(dr.GetOrdinal("CustomJs"));
                                        CssText = dr.GetString(dr.GetOrdinal("CssText"));
                                        CusDataDefine = dr.GetString(dr.GetOrdinal("CusDataDefine"));
                                        OtherSet = dr.GetString(dr.GetOrdinal("OtherSet"));
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
                                        ComName = dr.GetString(dr.GetOrdinal("ComName"));
                                        Caption = dr.GetString(dr.GetOrdinal("Caption"));
                                        ApiBase = dr.GetString(dr.GetOrdinal("ApiBase"));
                                        ApiGet = dr.GetString(dr.GetOrdinal("ApiGet"));
                                        ApiSet = dr.GetString(dr.GetOrdinal("ApiSet"));
                                        Rows = dr.GetString(dr.GetOrdinal("ColRows"));
                                        Buttons = dr.GetString(dr.GetOrdinal("Buttons"));
                                        BindGet = dr.GetString(dr.GetOrdinal("BindGet"));
                                        BindSet = dr.GetString(dr.GetOrdinal("BindSet"));
                                        JsBeforeSubmit = dr.GetString(dr.GetOrdinal("JsBeforeSubmit"));
                                        JsAfterSubmit = dr.GetString(dr.GetOrdinal("JsAfterSubmit"));
                                        JsBeforeGet = dr.GetString(dr.GetOrdinal("JsBeforeGet"));
                                        JsBeforeSet = dr.GetString(dr.GetOrdinal("JsBeforeSet"));
                                        JsAfterSet = dr.GetString(dr.GetOrdinal("JsAfterSet"));
                                        CustomJs = dr.GetString(dr.GetOrdinal("CustomJs"));
                                        CssText = dr.GetString(dr.GetOrdinal("CssText"));
                                        CusDataDefine = dr.GetString(dr.GetOrdinal("CusDataDefine"));
                                        OtherSet = dr.GetString(dr.GetOrdinal("OtherSet"));
                                    }
                                }
                            }
                        }
                        if (hasValue)
                        {
                            sql = "delete from front_form where ComName='" + ComName + "'";
                            globalConst.CurSite.SiteConn.execSql(sql);
                            sql = "insert into front_form(ComName,Caption,ApiBase,ApiGet,ApiSet,Rows,Buttons,BindGet,BindSet,JsBeforeSubmit,JsAfterSubmit,JsBeforeGet,JsBeforeSet,JsAfterSet,CustomJs,CssText,CusDataDefine,OtherSet)";
                            sql += "values(";
                            sql += "'" + str.Dot2DotDot(ComName.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(Caption.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(ApiBase.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(ApiGet.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(ApiSet.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(Rows.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(Buttons.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(BindGet.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(BindSet.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(JsBeforeSubmit.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(JsAfterSubmit.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(JsBeforeGet.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(JsBeforeSet.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(JsAfterSet.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(CustomJs.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(CssText.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(CusDataDefine.Trim()) + "'";
                            sql += ",'" + str.Dot2DotDot(OtherSet.Trim()) + "'";
                            sql += ")";
                            globalConst.CurSite.SiteConn.execSql(sql);
                            IsUpdateFromBackUp = true;
                        }
                    }
                    if(IsUpdateFromBackUp)
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