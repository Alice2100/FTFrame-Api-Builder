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
using Microsoft.Data.Sqlite;
using FTDPClient.consts;

namespace FTDPClient.forms.control
{
    public partial class Js_List : Form
    {
        public bool IsCancel = true;
        public string SetVal;
        public Js_List()
        {
            InitializeComponent();
        }

        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            this.Text = res.ctl.str("Js_List.text");			//JS方法调用
            Editor ed = form.getEditor();
            string pageid = null;
            if(ed==null || ed.thisID==null)
            {
                label1.Text = res.ctl.str("Js_List.1");
            }
            else
            {
                pageid = ed.thisID;
                label1.Text = ed.thisName+":"+ ed.thisTitle;
            }
            string sql;
            List<Tuple<int, string>> FLowList = new List<Tuple<int, string>>();
            string connstr = Options.GetSystemDBSetConnStr_Plat();
            var dbtype = Options.GetSystemDBSetType_Plat();
            if (!string.IsNullOrWhiteSpace(connstr))
            {
                //if (dbtype.ToLower() == "sqlserver")
                //{
                //    using (SqlConnection conn = new SqlConnection(connstr))
                //    {
                //        conn.Open();
                //        sql = "select FlowNum,FlowName from fc_workflow_list where stat=1 order by OrderRank";
                //        using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                //        {
                //            while(dr.Read())FLowList.Add(new Tuple<int, string>(dr.GetInt32(0), dr.GetString(1)));
                //        }
                //    }
                //}
                //else if (dbtype.ToLower() == "mysql")
                //{
                //    using (MySqlConnection conn = new MySqlConnection(connstr))
                //    {
                //        conn.Open();
                //        sql = "select FlowNum,FlowName from fc_workflow_list where stat=1 order by OrderRank";
                //        using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                //        {
                //            while (dr.Read()) FLowList.Add(new Tuple<int, string>(dr.GetInt32(0), dr.GetString(1)));
                //        }
                //    }
                //}
            }
            DataGridViewColumn ColModule = new DataGridViewTextBoxColumn();
            ColModule.Name = res.ctl.str("Js_List.2");
            dataGridView1.Columns.Add(ColModule);
            ColModule.Width =120;

            DataGridViewColumn ColJsName= new DataGridViewTextBoxColumn();
            ColJsName.Name = res.ctl.str("Js_List.3");
            dataGridView1.Columns.Add(ColJsName);
            ColJsName.Width = 200;

            DataGridViewColumn ColJs = new DataGridViewTextBoxColumn();
            ColJs.Name = res.ctl.str("Js_List.4");
            dataGridView1.Columns.Add(ColJs);
            ColJs.Width = 350;

            DataGridViewColumn ColDec = new DataGridViewTextBoxColumn();
            ColDec.Name = res.ctl.str("Js_List.5");
            dataGridView1.Columns.Add(ColDec);
            ColDec.Width = 350;

            if(pageid != null)
            {
                sql = "select c.name as controlname,c.caption,b.name as partname,a.partid,b.partxml from part_in_page a,parts b,controls c where a.partid=b.id and b.controlid=c.id and c.name in ('list','dyvalue','dataop') and a.pageid='" + pageid+"'";
                using(SqliteDataReader dr = consts.globalConst.CurSite.SiteConn.OpenRecord(sql))
                {
                    while(dr.Read())
                    {
                        string ControlName = dr.GetString(0);
                        string ControlCaption = dr.GetString(1);
                        string PartName = dr.GetString(2);
                        string PartID = dr.GetString(3);
                        string PartXml = dr.GetString(4);
                        if (ControlName== "list")
                        {
                            if(PartName== "List")
                            {
                                dataGridView1.Rows.Add(new string[] {
                ControlCaption,res.ctl.str("Js_List.6"),"load_"+PartID+"(1)",res.ctl.str("Js_List.7")
            });
                            }
                        }
                        else if (ControlName == "dyvalue")
                        {
                            if (PartName == "Interface")
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(PartXml);
                                string funcname = Page.PageWare.getPartParamValue(doc, "FuncName");
                                dataGridView1.Rows.Add(new string[] {
                ControlCaption,res.ctl.str("Js_List.8"),funcname+"([p1])",res.ctl.str("Js_List.9")
            });
                            }
                        }
                    }
                }
            }

            foreach(var  item in FLowList)
            {
                string FlowName = item.Item2;
                int FLowNum = item.Item1;
                dataGridView1.Rows.Add(new string[] {
                FlowName,"StartFlow","startFlow("+FLowNum+",'[workid]')",""
            });
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Blue;
                dataGridView1.Rows.Add(new string[] {
                FlowName,"ResetFlow","resetFlow("+FLowNum+",'[workid]')",""
            });
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Blue;
                dataGridView1.Rows.Add(new string[] {
                FlowName,"CancelFlow","cancelFlow("+FLowNum+",'[workid]')",""
            });
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Blue;
                dataGridView1.Rows.Add(new string[] {
                FlowName,"ShowFlowList","showFlowList("+FLowNum+",'[workid]','flowlistdiv','Flow')",""
            });
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Blue;
                dataGridView1.Rows.Add(new string[] {
                FlowName,"FlowJudge","flowJudge("+FLowNum+",'[workid]','[title]','judgediv')",""
            });
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Blue;
                dataGridView1.Rows.Add(new string[] {
                FlowName,"Judge","judge([type],'judgediv')","type 1 pass,2 go back,3 reject"
            });
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Blue;
                dataGridView1.Rows.Add(new string[] {
                FlowName,"DoAllPass","doPassAll([object])","object is html dom"
            });
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Blue;
                dataGridView1.Rows.Add(new string[] {
                FlowName,"DoAllPass","doAllPass("+FLowNum+",[object])","special flow,object is html dom"
            });
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Blue;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SetVal = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            IsCancel = false;
            this.Close();
        }
        private void SelCol_SqlServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            Save();
            MsgBox.Information(res.ctl.str("Js_List.10"));
        }
        void Save()
        {
            string sql = "delete from codeset";
            consts.globalConst.CurSite.SiteConn.execSql(sql);
            int loop = 1;
            List<string> listKey = new List<string>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (listKey.Contains(row.Cells[1].Value.ToString()))
                {
                    MsgBox.Warning(row.Cells[1].Value.ToString() + " "+ res.ctl.str("Js_List.11"));
                    return;
                }
                listKey.Add(row.Cells[1].Value.ToString());
                sql = "insert into codeset(id,dllname,codekey,codeval,returntype,modtime)";
                sql += "values(" + loop + ",'" + functions.str.Dot2DotDot(row.Cells[4].Value.ToString()) + "','" + functions.str.Dot2DotDot(row.Cells[1].Value.ToString()) + "','" + functions.str.Dot2DotDot(row.Cells[2].Value.ToString()) + "','" + functions.str.Dot2DotDot(row.Cells[3].Value.ToString()) + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                consts.globalConst.CurSite.SiteConn.execSql(sql);
                loop++;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Code_Select cs = new Code_Select();
            cs.ShowDialog();
            if (!cs.IsCancel)
            {
                string[] item = cs.SetVal.Split(':');
                string key = item[0];
                if(key.IndexOf('(')>0)
                {
                    key = key.Substring(0, key.IndexOf('('));
                }
                dataGridView1.Rows.Add(new string[] {
                res.ctl.str("Js_List.12"),key,item[0],item[1],item[2],res.ctl.str("Js_List.13")
            });
                Save();
            }
            
        }
    }
}
