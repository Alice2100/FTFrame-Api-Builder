using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FTDPClient.Style;
using FTDPClient.functions;
using FTDPClient.consts;
using FTDPClient.database;
using FTDPClient.forms;
using FTDPClient.SiteClass;
using mshtml;
using htmleditocx;
using FTDPClient.Page;
using FTDPClient.Common;
using System.Collections;
using Microsoft.Data.Sqlite;
using FTDPClient.Obj;
using FTDPClient.Front;

namespace FTDPClient.forms
{
    public partial class BugResult : Form
    {
        public bool IsAllSite = false;
        public ArrayList SelFiles = null;
        public string SinglePageID = null;
        public bool For_Back = false;
        public bool For_Front = false;
        public bool For_Rule = false;
        public BugResult()
        {
            InitializeComponent();
            FTDP.Util.BugCheck.StrItems = new string[] {
            res.form.GetString("quanbu"),
            res.form.GetString("htmlyuansubiaoz"),
            res.form.GetString("shujuliebiaosz"),
            res.form.GetString("shujucaozuosz"),
            res.form.GetString("shujuhuoqusz"),
            res.form.GetString("qita"),
            res.form.GetString("yanzhong"),
            res.form.GetString("zhongyao"),
            res.form.GetString("yiban"),
            res.form.GetString("jinggao"),
            res.form.GetString("kelue"),
            res.form.GetString("dakaiwenjian"),
            res.form.GetString("chongfutagwei"),
            };
            this.comboBox1.Items.AddRange(FTDP.Util.BugCheck.BugTypes);
            this.comboBox1.SelectedIndex = 0;
        }

        private void BugResult_Load(object sender, EventArgs e)
        {
            #region Language
            label1.Text = res.form.GetString("leixing");
            dataGridView1.Columns[0].HeaderText = res.form.GetString("leixing");
            dataGridView1.Columns[1].HeaderText = res.form.GetString("wenjian");
            dataGridView1.Columns[2].HeaderText = res.form.GetString("biaoti");
            dataGridView1.Columns[3].HeaderText = res.form.GetString("zujian");
            dataGridView1.Columns[4].HeaderText = res.form.GetString("miaosu");
            dataGridView1.Columns[5].HeaderText = res.form.GetString("jibie");
            dataGridView1.Columns[6].HeaderText = res.form.GetString("caozuo");
            dataGridView3.Columns[0].HeaderText = res.form.GetString("leixing");
            dataGridView3.Columns[1].HeaderText = res.form.GetString("biaoti");
            dataGridView3.Columns[2].HeaderText = res.form.GetString("zujian");
            dataGridView3.Columns[3].HeaderText = res.form.GetString("miaosu");
            dataGridView3.Columns[4].HeaderText = res.form.GetString("caozuo");
            dataGridView2.Columns[0].HeaderText = "Module";
            dataGridView2.Columns[1].HeaderText = "Page";
            dataGridView2.Columns[2].HeaderText = res.form.GetString("zujian");
            dataGridView2.Columns[3].HeaderText = "API";
            dataGridView2.Columns[4].HeaderText = res.form.GetString("miaosu");
            dataGridView2.Columns[5].HeaderText = res.form.GetString("caozuo");
            dataGridView2.Columns[6].HeaderText = res.form.GetString("caozuo");
            #endregion
            @break = false;
        }

        private void BugResult_Shown(object sender, EventArgs e)
        {
            tabPage1.Text = "Checking";
            tabPage3.Text = "Checking";
            tabPage2.Text = "Checking";
            FTDP.Util.BugCheck.TableColumnDic = new Dictionary<string, List<string>>();
            var pageList = new List<(string pageid, string pagepath)>();
            string LocalSiteConnection = db.ConnStr_Site();
            var RemoteDBType = Options.GetSystemDBSetType();
            string RemoteConnection = Options.GetSystemDBSetConnStr();
            string AppPath = globalConst.AppPath;
            string SiteID = globalConst.CurSite.ID;
            DB SiteDB = new DB();
            SiteDB.Open(LocalSiteConnection);
            if (For_Back)
            {
                try
                {
                    if (IsAllSite)
                    {
                        string sql = "select id from pages";
                        SqliteDataReader dr = SiteDB.OpenRecord(sql);
                        while (dr.Read())
                        {
                            string pageid = dr.GetString(0);
                            string pagepath = FTDP.Util.BugCheck.getPathByPageId(pageid, LocalSiteConnection);
                            pageList.Add((pageid, pagepath));
                        }
                        dr.Close();
                    }
                    else if (SelFiles != null)
                    {
                        foreach (string[] files in SelFiles)
                        {
                            string pageid = files[0];
                            string pagepath = files[1];
                            pageList.Add((pageid, pagepath));
                        }
                    }
                    else if (SinglePageID != null)
                    {
                        string pagepath = FTDP.Util.BugCheck.getPathByPageId(SinglePageID, LocalSiteConnection);
                        pageList.Add((SinglePageID, pagepath));
                    }
                }
                finally
                {
                    SiteDB.Close();
                }
                for (int i = 0; i < pageList.Count && !@break; i++)
                {
                    label2.Text = " Checking " + (i + 1) + " / " + pageList.Count;
                    Application.DoEvents();
                    var result = FTDP.Util.BugCheck.BugPage(pageList[i].pageid, pageList[i].pagepath, AppPath, SiteID, LocalSiteConnection, DBFunc.DBTypeString(RemoteDBType), RemoteConnection);
                    foreach (var item0 in result)
                    {
                        string[] item = new string[] {
                 item0.LeiXing,
                 item0.Path,
                 item0.PageCaption,
                 item0.ControlCaption,
                 item0.BugDesc,
                 item0.Level,
                 item0.OpenFile,
                 item0.PageID,
                };
                        int a = dataGridView1.Rows.Add(item);
                        DataGridViewRow row = dataGridView1.Rows[a];
                        row.Tag = item[7];
                        if (item[5].Equals(res.form.GetString("yanzhong")))
                        {
                            row.DefaultCellStyle.BackColor = Color.DarkRed;
                            row.DefaultCellStyle.ForeColor = Color.White;
                        }
                        else if (item[5].Equals(res.form.GetString("zhongyao")))
                        {
                            row.DefaultCellStyle.BackColor = Color.Chocolate;
                            row.DefaultCellStyle.ForeColor = Color.White;
                        }
                        else if (item[5].Equals(res.form.GetString("yiban")))
                        {
                            row.DefaultCellStyle.BackColor = Color.LightSalmon;
                            row.DefaultCellStyle.ForeColor = Color.Black;
                        }
                        else if (item[5].Equals(res.form.GetString("jinggao")))
                        {
                            row.DefaultCellStyle.BackColor = Color.PeachPuff;
                            row.DefaultCellStyle.ForeColor = Color.Black;
                        }
                        else if (item[5].Equals(res.form.GetString("kelue")))
                        {
                            row.DefaultCellStyle.BackColor = Color.MintCream;
                            row.DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                    result.Clear();
                    result = null;
                    label2.Text = " Checked     " + (i + 1) + "/" + pageList.Count;
                }
            }
            else
            {
                tabControl1.SelectedIndex= 1;
            }
            tabPage1.Text = "Base (" + dataGridView1.Rows.Count + ")";
            if (For_Front)
            {
                FrontFunc.Check(dataGridView3, label2);
            }
            tabPage3.Text = "Front (" + dataGridView3.Rows.Count + ")";
            if (For_Rule)
            {
                RuleFunction.Check(dataGridView2, label2);
            }
            tabPage2.Text = "Rule (" + dataGridView2.Rows.Count + ")";
            label2.Text += "     Bug Count : " + (dataGridView1.Rows.Count + dataGridView3.Rows.Count + dataGridView2.Rows.Count);
        }

        private void BugResult_Shown_All(object sender, EventArgs e)
        {
            //do
            //ArrayList result = Bug.Init(IsAllSite,SelFiles,SinglePageID);
            var result = FTDP.Util.BugCheck.Init(IsAllSite, SelFiles, SinglePageID, db.ConnStr_Cfg(), db.ConnStr_Site(), Options.GetSystemValue("mysql"), globalConst.AppPath, globalConst.CurSite.ID);
            label2.Text = "";
            foreach (var item0 in result)
            {
                string[] item = new string[] {
                 item0.LeiXing,
                 item0.Path,
                 item0.PageCaption,
                 item0.BugDesc,
                 item0.Level,
                 item0.OpenFile,
                 item0.PageID,
                };
                int a = dataGridView1.Rows.Add(item);
                DataGridViewRow row = dataGridView1.Rows[a];
                row.Tag = item[6];
                if (item[4].Equals(res.form.GetString("yanzhong")))
                {
                    row.DefaultCellStyle.BackColor = Color.DarkRed;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else if (item[4].Equals(res.form.GetString("zhongyao")))
                {
                    row.DefaultCellStyle.BackColor = Color.Chocolate;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else if (item[4].Equals(res.form.GetString("yiban")))
                {
                    row.DefaultCellStyle.BackColor = Color.LightSalmon;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (item[4].Equals(res.form.GetString("jinggao")))
                {
                    row.DefaultCellStyle.BackColor = Color.PeachPuff;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (item[4].Equals(res.form.GetString("kelue")))
                {
                    row.DefaultCellStyle.BackColor = Color.MintCream;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
            result.Clear();
            result = null;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (e.ColumnIndex == 6)
                {
                    string id = row.Tag.ToString();
                    if (form.doActive(id))
                    {
                        return;
                    }
                    string path = globalConst.CurSite.Path + "\\" + row.Cells[1].Value.ToString().Replace("/", "\\");

                    if (!file.Exists(path + "_edit.htm"))
                    {
                        SiteClass.Site.constructEditPageFromText(id, path);
                    }
                    else
                    {
                        //如果edit文件被手动修改过，重新生成
                        System.IO.FileInfo fioedit = new System.IO.FileInfo(path + "_edit.htm");
                        System.IO.FileInfo fio = new System.IO.FileInfo(path);
                        if (!(fioedit.LastWriteTime.Equals(fio.LastWriteTime)))
                        {
                            SiteClass.Site.constructEditPageFromText(id, path);
                        }
                    }
                    string sql = "select name from pages where id='" + str.Dot2DotDot(id) + "'";
                    string name = globalConst.CurSite.SiteConn.GetString(sql);
                    sql = "select ptype from pages where id='" + str.Dot2DotDot(id) + "'";
                    int ptype = globalConst.CurSite.SiteConn.GetInt32(sql);
                    form.addEditor(path, row.Cells[2].Value.ToString(), id, name, ptype);
                }
            }
            catch { }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = comboBox1.Text;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    if (val.Equals(res.form.GetString("quanbu")) || val.Equals(""))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        string type = row.Cells[0].Value == null ? "" : row.Cells[0].Value.ToString();
                        row.Visible = type.Equals(val);
                    }
                }
                catch { }
            }
        }
        public static bool @break = false;
        private void button1_Click(object sender, EventArgs e)
        {
            @break = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            @break = false;
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            BugResult_Shown(sender,e);
            @break = false;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
            var ruleResult = row.Tag as RuleResult;
            if (e.ColumnIndex == 5)
            {
                HTMLText hTMLText = new HTMLText();
                hTMLText.Text = res.anew.GetString("String19");
                hTMLText.SetVal = "PageId = " + ruleResult.PageId + Environment.NewLine
                    + "PagePath = " + ruleResult.PagePath + Environment.NewLine
                    + "PageCaption = " + ruleResult.PageCaption + Environment.NewLine
                    + "ControlId = " + ruleResult.ControlId + Environment.NewLine
                    + "ControlName = " + ruleResult.ControlName + Environment.NewLine
                    + "ControlCaption = " + ruleResult.ControlCaption + Environment.NewLine
                    + "PartName = " + ruleResult.PartName + Environment.NewLine
                    + "ApiType = " + ruleResult.ApiType + Environment.NewLine
                    + "ApiPath = " + ruleResult.ApiPath + Environment.NewLine
                    + "ApiCaption = " + ruleResult.ApiCaption + Environment.NewLine
                    + "RuleDirId = " + ruleResult.RuleDirId + Environment.NewLine
                    + "RuleDirCaption = " + ruleResult.RuleDirCaption + Environment.NewLine
                    + "RuleTableId = " + ruleResult.RuleTableId + Environment.NewLine
                    + "RuleTableBind = " + ruleResult.RuleTableBind + Environment.NewLine
                    + "RuleTableCaption = " + ruleResult.RuleTableCaption + Environment.NewLine
                    + "RuleColumnId = " + ruleResult.RuleColumnId + Environment.NewLine
                    + "RuleColumnBind = " + ruleResult.RuleColumnBind + Environment.NewLine
                    + "RuleColumnCaption = " + ruleResult.RuleColumnCaption + Environment.NewLine
                    + "RuleAtomId = " + ruleResult.RuleAtomId + Environment.NewLine
                    + "RuleAtomValue = " + ruleResult.RuleAtomValue + Environment.NewLine
                    + "Message = " + ruleResult.Message + Environment.NewLine;
                hTMLText.TopMost = true;
                hTMLText.ShowDialog();
            }
            else if (e.ColumnIndex==6)
            {
                try
                {
                    if (!string.IsNullOrEmpty(ruleResult.PagePath))
                    {
                        string id = ruleResult.PageId;
                        if (form.doActive(id))
                        {
                            return;
                        }
                        string path = globalConst.CurSite.Path + "\\" + ruleResult.PagePath.Replace("/", "\\");

                        if (!file.Exists(path + "_edit.htm"))
                        {
                            SiteClass.Site.constructEditPageFromText(id, path);
                        }
                        else
                        {
                            //如果edit文件被手动修改过，重新生成
                            System.IO.FileInfo fioedit = new System.IO.FileInfo(path + "_edit.htm");
                            System.IO.FileInfo fio = new System.IO.FileInfo(path);
                            if (!(fioedit.LastWriteTime.Equals(fio.LastWriteTime)))
                            {
                                SiteClass.Site.constructEditPageFromText(id, path);
                            }
                        }
                        string sql = "select name from pages where id='" + str.Dot2DotDot(id) + "'";
                        string name = globalConst.CurSite.SiteConn.GetString(sql);
                        sql = "select ptype from pages where id='" + str.Dot2DotDot(id) + "'";
                        int ptype = globalConst.CurSite.SiteConn.GetInt32(sql);
                        form.addEditor(path, ruleResult.PageCaption, id, name, ptype);
                    }
                }
                catch { }
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                var rowTag = dataGridView3.Rows[e.RowIndex].Tag as object[];
                var type = rowTag[0] as string;
                var comname = rowTag[1] as string;
                var tabIndex = (int)rowTag[3];
                var keyText = rowTag[4] as string;
                if (globalConst.MdiForm.foreDevForm != null)
                {
                    globalConst.MdiForm.foreDevForm.Activate();
                }
                else
                {
                    ForeDev foreDev = new ForeDev();
                    foreDev.ShowInTaskbar = true;
                    globalConst.MdiForm.foreDevForm = foreDev;
                    foreDev.Show();
                }
                globalConst.MdiForm.foreDevForm.findComAndActive(type=="list"? "[List]":"[Form]", comname, "");
                globalConst.MdiForm.foreDevForm.tabActive(type == "list" ? tabIndex : tabIndex);
                globalConst.MdiForm.foreDevForm.activeShowForBugResult(type,tabIndex ,keyText);
                
            }
        }
    }
}
