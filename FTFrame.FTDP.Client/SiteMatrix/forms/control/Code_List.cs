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
using FTDPClient.consts;
using Microsoft.Data.Sqlite;
using FTDPClient.database;

namespace FTDPClient.forms.control
{
    public partial class Code_List : Form
    {
        public bool IsCancel = true;
        public string SetVal;
        public string ReturnType = "";
        public Code_List()
        {
            InitializeComponent();
        }
        string QianMing;
        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            this.Text = res.ctl.str("Code_List.text");          //程序集方法调用
            button1.Text = res.ctl.str("Code_List.button1");            //保存(&S)
            button2.Text = res.ctl.str("Code_List.button2");            //新增(&A)
            button3.Text = res.ctl.str("Code_List.button3");			//同步其他开发者配置
            QianMing = Options.QianMing;
            label1.Text = res.ctl.str("Code_List.1") + QianMing;

            DataGridViewColumn ColQianMing = new DataGridViewTextBoxColumn();
            ColQianMing.Name = res.ctl.str("Code_List.2");
            dataGridView1.Columns.Add(ColQianMing);
            ColQianMing.Width = 120;

            DataGridViewButtonColumn ColSelect = new DataGridViewButtonColumn();
            ColSelect.Name = "";
            dataGridView1.Columns.Add(ColSelect);
            ColSelect.Width = 60;

            DataGridViewColumn ColCodeKey = new DataGridViewTextBoxColumn();
            ColCodeKey.Name = res.ctl.str("Code_List.3");
            dataGridView1.Columns.Add(ColCodeKey);
            ColCodeKey.Width = 150;

            DataGridViewColumn ColCodeVal = new DataGridViewTextBoxColumn();
            ColCodeVal.Name = res.ctl.str("Code_List.4");
            dataGridView1.Columns.Add(ColCodeVal);
            ColCodeVal.Width = 350;

            DataGridViewColumn ColReturnType = new DataGridViewTextBoxColumn();
            ColReturnType.Name = res.ctl.str("Code_List.5");
            dataGridView1.Columns.Add(ColReturnType);
            ColReturnType.Width = 60;

            DataGridViewColumn ColDllName = new DataGridViewTextBoxColumn();
            ColDllName.Name = res.ctl.str("Code_List.6");
            dataGridView1.Columns.Add(ColDllName);
            ColDllName.Width = 120;

            DataGridViewColumn ColDel = new DataGridViewTextBoxColumn();
            ColDel.Name = "";
            dataGridView1.Columns.Add(ColDel);
            ColDel.Width = 50;
            ColDel.DefaultCellStyle.ForeColor = Color.Pink;

            DataGridViewColumn ColMimo = new DataGridViewTextBoxColumn();
            ColMimo.Name = res.ctl.str("Code_List.7");
            dataGridView1.Columns.Add(ColMimo);
            ColMimo.Width = 150;

            string sql = "select devuser,codekey,codeval,returntype,dllname,mimo from codeset where devuser='"+str.Dot2DotDot(QianMing)+"'  order by id";
            using (SqliteDataReader rdr = consts.globalConst.CurSite.SiteConn.OpenRecord(sql))
            {
                while (rdr.Read())
                {
                    dataGridView1.Rows.Add(new string[] {
               (rdr.GetValue(0)??"").ToString(), res.ctl.str("Code_List.8"),rdr.GetString(1),rdr.GetString(2),rdr.GetString(3),rdr.GetString(4),res.ctl.str("Code_List.9"),(rdr.GetValue(5)??"").ToString()
            });
                }
            }
            sql = "select devuser,codekey,codeval,returntype,dllname,mimo from codeset where devuser<>'" + str.Dot2DotDot(QianMing) + "'  order by devuser,id";
            using (SqliteDataReader rdr = consts.globalConst.CurSite.SiteConn.OpenRecord(sql))
            {
                while (rdr.Read())
                {
                    int index=dataGridView1.Rows.Add(new string[] {
              (rdr.GetValue(0)??"").ToString(), res.ctl.str("Code_List.8"),rdr.GetString(1),rdr.GetString(2),rdr.GetString(3),rdr.GetString(4),"",(rdr.GetValue(5)??"").ToString()
            });
                    dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
            CodeStaticGet();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                Save();
                string codeKey = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                if (string.IsNullOrWhiteSpace(dataGridView1.Rows[e.RowIndex].Cells[5].Value?.ToString() ?? ""))
                {
                    if(dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor==Color.LightBlue)
                        SetVal = "@enum(" + codeKey + ")";
                    else SetVal = "@code(" + codeKey + ")";
                }
                else
                {
                    SetVal = "@code($" + codeKey + ",)";
                }
                ReturnType= dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                IsCancel = false;
                this.Close();
            }
          else  if (e.ColumnIndex==6)
            {
                dataGridView1.Rows.Remove(dataGridView1.Rows[e.RowIndex]);
            }
        }
        private void SelCol_SqlServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            if(Save())MsgBox.Information(res.ctl.str("Code_List.10"));
        }
        bool Save()
        {
            List<string> listKey = new List<string>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (listKey.Contains(row.Cells[2].Value.ToString()))
                {
                    MsgBox.Warning(row.Cells[2].Value.ToString() + " "+ res.ctl.str("Code_List.11"));
                    return false;
                }
                listKey.Add(row.Cells[2].Value.ToString());
            }
           string sql = "delete from codeset";
            consts.globalConst.CurSite.SiteConn.execSql(sql);
            int loop = 1;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if ((row.Cells[0].Value?.ToString() ?? "")=="[Plat]") continue;
                sql = "insert into codeset(id,devuser,dllname,codekey,codeval,returntype,modtime,mimo)";
                sql += "values(" + loop + ",'" + functions.str.Dot2DotDot((row.Cells[0].Value??"").ToString()) + "','" + functions.str.Dot2DotDot(row.Cells[5].Value?.ToString()??"") + "','" + functions.str.Dot2DotDot(row.Cells[2].Value?.ToString() ?? "") + "','" + functions.str.Dot2DotDot(row.Cells[3].Value?.ToString() ?? "") + "','" + functions.str.Dot2DotDot(row.Cells[4].Value?.ToString() ?? "") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + str.Dot2DotDot((row.Cells[7].Value??"").ToString()) + "')";
                consts.globalConst.CurSite.SiteConn.execSql(sql);
                loop++;
            }
            globalConst.CurSite.CodeSetChanged = true;
            return true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DllList dlllist = new DllList();
            dlllist.ShowDialog();
            string ReflectStr = dlllist.ReflectString;
            if (ReflectStr != null)
            {
                Code_Select cs = new Code_Select();
                cs.ReflectString = ReflectStr;
                cs.ShowDialog();
                if (!cs.IsCancel)
                {
                    string[] item = cs.SetVal.Split(':');
                    //string key = item[0];
                    //if(key.IndexOf('(')>0)
                    //{
                    //    key = key.Substring(0, key.IndexOf('('));
                    //}
                    int mycount = MyCount();
                    string key = QianMing + (mycount + 1);
                    dataGridView1.Rows.Insert(mycount, new string[] {
                QianMing,res.ctl.str("Code_List.8"),key,item[0],item[1],item[2],res.ctl.str("Code_List.9"),"" });
                    Save();
                }
            }
        }
        private int MyCount()
        {
            int loop = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value.ToString() != QianMing) break;
                else loop++;
            }
            return loop;
        }
        private void CodeStaticGet()
        {
            string connstr = Options.GetSystemDBSetConnStr_Plat();
            var dbtype = Options.GetSystemDBSetType_Plat();
            string sql;
            if (!string.IsNullOrWhiteSpace(connstr))
            {
                if (dbtype==globalConst.DBType.SqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(connstr))
                    {
                        conn.Open();
                        sql = "select codetype,codekey,codeval,returntype from ft_ftdp_codestatic";
                        using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                int index = dataGridView1.Rows.Add(new string[] {"[Plat]",
               res.ctl.str("Code_List.8"),dr.GetString(1),dr.GetString(2),dr.GetString(3),"","","Static"
            });
                                dataGridView1.Rows[index].DefaultCellStyle.BackColor = dr.GetInt16(0)==0?Color.LightGray:Color.LightBlue;
                            }
                        }
                    }
                }
                else if (dbtype==globalConst.DBType.MySql)
                {
                    using (MySqlConnection conn = new MySqlConnection(connstr))
                    {
                        conn.Open();
                        sql = "select codetype,codekey,codeval,returntype from ft_ftdp_codestatic";
                        using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                int index = dataGridView1.Rows.Add(new string[] {"[Plat]",
               res.ctl.str("Code_List.8"),dr.GetString(1),dr.GetString(2),dr.GetString(3),"","","Static"
            });
                                dataGridView1.Rows[index].DefaultCellStyle.BackColor = dr.GetInt16(0) == 0 ? Color.LightGray : Color.LightBlue;
                            }
                        }
                    }
                }
                else if (dbtype == globalConst.DBType.Sqlite)
                {
                    using (var conn = new DB(connstr))
                    {
                        conn.Open();
                        sql = "select codetype,codekey,codeval,returntype from ft_ftdp_codestatic";
                        using (var dr = conn.OpenRecord(sql))
                        {
                            while (dr.Read())
                            {
                                int index = dataGridView1.Rows.Add(new string[] {"[Plat]",
               res.ctl.str("Code_List.8"),dr.GetString(1),dr.GetString(2),dr.GetString(3),"","","Static"
            });
                                dataGridView1.Rows[index].DefaultCellStyle.BackColor = dr.GetInt16(0) == 0 ? Color.LightGray : Color.LightBlue;
                            }
                        }
                    }
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string connstr = Options.GetSystemDBSetConnStr_Plat();
            var dbtype = Options.GetSystemDBSetType_Plat();
            string sql;
            if (!string.IsNullOrWhiteSpace(connstr))
            {
                List<DataGridViewRow> list=new List<DataGridViewRow> ();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value.ToString() != QianMing) list.Add(row);
                }
                list.ForEach(row => dataGridView1.Rows.Remove(row));
                if (dbtype==globalConst.DBType.SqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(connstr))
                    {
                        conn.Open();
                        sql = "select devuser,codekey,codeval,returntype,dllname,mimo from ft_ftdp_codeset where devuser!='" + str.Dot2DotDot(QianMing) + "'  order by devuser";
                        using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                int index = dataGridView1.Rows.Add(new string[] {
               (dr.GetValue(0)??"").ToString(), res.ctl.str("Code_List.8"),dr.GetString(1),dr.GetString(2),dr.GetString(3),dr.GetString(4),"",(dr.GetValue(5)??"").ToString()
            });
                                dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.LightGray;
                            }
                        }
                    }
                }
                else if (dbtype==globalConst.DBType.MySql)
                {
                    using (MySqlConnection conn = new MySqlConnection(connstr))
                    {
                        conn.Open();
                        sql = "select devuser,codekey,codeval,returntype,dllname,mimo from ft_ftdp_codeset where devuser!='" + str.Dot2DotDot(QianMing) + "'  order by devuser";
                        using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                int index = dataGridView1.Rows.Add(new string[] {
              (dr.GetValue(0)??"").ToString(), res.ctl.str("Code_List.8"),dr.GetString(1),dr.GetString(2),dr.GetString(3),dr.GetString(4),"",(dr.GetValue(5)??"").ToString()
            });
                                dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.LightGray;
                            }
                        }
                    }
                }
                else if (dbtype == globalConst.DBType.Sqlite)
                {
                    using (var conn = new DB(connstr))
                    {
                        conn.Open();
                        sql = "select devuser,codekey,codeval,returntype,dllname,mimo from ft_ftdp_codeset where devuser!='" + str.Dot2DotDot(QianMing) + "'  order by devuser";
                        using (var dr = conn.OpenRecord(sql))
                        {
                            while (dr.Read())
                            {
                                int index = dataGridView1.Rows.Add(new string[] {
              (dr.GetValue(0)??"").ToString(), res.ctl.str("Code_List.8"),dr.GetString(1),dr.GetString(2),dr.GetString(3),dr.GetString(4),"",(dr.GetValue(5)??"").ToString()
            });
                                dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.LightGray;
                            }
                        }
                    }
                }
            }
            CodeStaticGet();
        }
    }
}
