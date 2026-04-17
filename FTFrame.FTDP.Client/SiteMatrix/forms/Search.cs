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
using System.Xml;

namespace FTDPClient.forms
{
    public partial class Search : Form
    {
        public Search()
        {
            InitializeComponent();
            FTDP.Util.ConfigSearch.StrItems = new string[] {
            res.form.GetString("shujuliebiaosz"),
            res.form.GetString("shujucaozuosz"),
            res.form.GetString("shujuhuoqusz"),
            res.form.GetString("dakaiwenjian"),
            };
        }

        private void BugResult_Load(object sender, EventArgs e)
        {
            #region Language
            checkBox1.Text = res.form.str("jinxianshipeizhi");
            dataGridView1.Columns[0].HeaderText = res.form.GetString("leixing");
            dataGridView1.Columns[1].HeaderText = res.form.GetString("position");
            dataGridView1.Columns[2].HeaderText = res.form.GetString("wenjian");
            dataGridView1.Columns[3].HeaderText = res.form.GetString("biaoti");
            dataGridView1.Columns[4].HeaderText = res.form.GetString("zujian");
            dataGridView1.Columns[5].HeaderText = res.form.GetString("miaosu");
            dataGridView1.Columns[6].HeaderText = res.form.GetString("kepeizhishujugaoji");
            dataGridView1.Columns[7].HeaderText = res.form.GetString("caozuo");
            dataGridView1.Columns[8].HeaderText = res.form.GetString("caozuo");
            #endregion
            SearchTextBox.Focus();
        }
        private void DoFind(string searchText)
        {
            FTDP.Util.BugCheck.TableColumnDic = new Dictionary<string, List<string>>();
            var pageList = new List<(string pageid, string pagepath)>();
            string LocalSiteConnection = db.ConnStr_Site();
            var RemoteDBType = Options.GetSystemDBSetType();
            string RemoteConnection = Options.GetSystemDBSetConnStr();
            string AppPath = globalConst.AppPath;
            string SiteID = globalConst.CurSite.ID;
            DB SiteDB = new DB();
            SiteDB.Open(LocalSiteConnection);
            try
            {
                string sql = "select id from pages";
                SqliteDataReader dr = SiteDB.OpenRecord(sql);
                while (dr.Read())
                {
                    string pageid = dr.GetString(0);
                    string pagepath = FTDP.Util.BugCheck.getPathByPageId(pageid, LocalSiteConnection);
                    if(pagepath!=null) pageList.Add((pageid, pagepath));
                }
                dr.Close();
            }
            finally
            {
                SiteDB.Close();
            }

            for (int i = 0; i < pageList.Count && !@break; i++)
            {
                label2.Text = " Searching " + (i + 1) + " / " + pageList.Count;
                Application.DoEvents();
                var result = FTDP.Util.ConfigSearch.FindPage(searchText,pageList[i].pageid, pageList[i].pagepath, AppPath, SiteID, LocalSiteConnection, DBFunc.DBTypeString(RemoteDBType), RemoteConnection);
                foreach (var item0 in result)
                {
                    string[] item = new string[] {
                     item0.LeiXing,
                     item0.Position,
                     item0.Path,
                     item0.PageCaption,
                     item0.ControlCaption,
                     item0.Desc,
                     item0.SetValue??"",
                     item0.SetKey==null?"":res.form.GetString("kuaisufabu"),
                     item0.OpenFile,
                    };
                    int a = dataGridView1.Rows.Add(item);
                    DataGridViewRow row = dataGridView1.Rows[a];
                    row.Tag = new object[] { item0.PageID, item0.PartObj,item0.SetKey, item0.LeiXing };
                    if(item0.LeiXing== FTDP.Util.ConfigSearch.StrItems[0])
                    {
                        row.DefaultCellStyle.BackColor = Color.AntiqueWhite;
                    }
                    else if (item0.LeiXing == FTDP.Util.ConfigSearch.StrItems[1])
                    {
                        row.DefaultCellStyle.BackColor = Color.Aqua;
                    }
                    else if (item0.LeiXing == FTDP.Util.ConfigSearch.StrItems[2])
                    {
                        row.DefaultCellStyle.BackColor = Color.Aquamarine;
                    }
                    if(item0.SetKey == null) row.Cells[6].Style.BackColor = Color.DarkGray;
                    else row.Cells[6].Style.BackColor = Color.White;
                }
                result.Clear();
                result = null;
                label2.Text = " Searched " + (i + 1) + " / " + pageList.Count;
            }

            label2.Text += "  Search Count : " + dataGridView1.Rows.Count;
        }
        private void BugResult_Shown(object sender, EventArgs e)
        {
            
        }
        bool IsSavePubing = false;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (e.ColumnIndex == 7)
                {
                    if (row.Cells[6].Style.BackColor != Color.White) return;
                    if (IsSavePubing || SiteClass.Site.IsPublishing)
                    {
                        MsgBox.Error(res.form.GetString("chulizhong"));
                        return;
                    }
                    try
                    {
                        IsSavePubing = true;
                        row.Cells[7].Value = res.form.GetString("chulizhong");
                        string pageId = (row.Tag as object[])[0].ToString();
                        string setKey = (row.Tag as object[])[2].ToString();
                        var partItem = ((string partname, string controlname, string partid, string partxml, string controlcaption))((row.Tag as object[])[1]);
                        string setValue = row.Cells[6].Value?.ToString()??"";
                        string leixing = (row.Tag as object[])[3].ToString();
                        //MsgBox.Information(setKey + ":::"+ partObj.partid+":::"+ row.Cells[6].Value.ToString());
                        if (leixing == FTDP.Util.ConfigSearch.StrItems[0])
                        {
                            #region List
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(partItem.partxml);
                            var nodes = doc.SelectNodes("//partxml/param");
                            StringBuilder sb = new StringBuilder();
                            foreach (XmlNode node in nodes)
                            {
                                string name = node.SelectSingleNode("name")?.InnerText ?? "";
                                string value = node.SelectSingleNode("value")?.InnerText ?? "";
                                if (name == "RowAll")
                                {
                                    string RowAll = value;
                                    string[] items = RowAll.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                                    for (var i = 0; i < items.Length; i++)
                                    {
                                        if (i > 0) sb.Append("|||");
                                        var item = items[i];
                                        string[] colcfg = item.Split('#');
                                        if (colcfg[0] == setKey)
                                        {
                                            for (var j = 0; j < colcfg.Length; j++)
                                            {
                                                if (j > 0) sb.Append("#");
                                                if (j == 1) sb.Append(str.getEncode(setValue));
                                                else sb.Append(colcfg[j]);
                                            }
                                        }
                                        else
                                        {
                                            sb.Append(item);
                                        }
                                    }
                                    break;
                                }
                            }
                            PageWare.setPartParamValue(ref doc, partItem.partid, "RowAll", sb.ToString());
                            doc = null;
                            #endregion
                        }
                        else if (leixing == FTDP.Util.ConfigSearch.StrItems[1])
                        {
                            #region DataOP
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(partItem.partxml);
                            var nodes = doc.SelectNodes("//partxml/param");
                            StringBuilder sb = new StringBuilder();
                            foreach (XmlNode node in nodes)
                            {
                                string name = node.SelectSingleNode("name")?.InnerText ?? "";
                                string value = node.SelectSingleNode("value")?.InnerText ?? "";
                                if (name == "Define")
                                {
                                    string Define = value;
                                    string[] items = Define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                                    for (var i = 0; i < items.Length; i++)
                                    {
                                        if (i > 0) sb.Append("|||");
                                        var item = items[i];
                                        string[] colcfg = item.Split(new string[] { "##" }, StringSplitOptions.None);
                                        if (colcfg[1] == setKey)
                                        {
                                            for (var j = 0; j < colcfg.Length; j++)
                                            {
                                                if (j > 0) sb.Append("##");
                                                if (j == 7) sb.Append(str.getEncode(setValue));
                                                else sb.Append(colcfg[j]);
                                            }
                                        }
                                        else
                                        {
                                            sb.Append(item);
                                        }
                                    }
                                    break;
                                }
                            }
                            PageWare.setPartParamValue(ref doc, partItem.partid, "Define", sb.ToString());
                            doc = null;
                            #endregion
                        }
                        else if (leixing == FTDP.Util.ConfigSearch.StrItems[2])
                        {
                            #region DyValue
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(partItem.partxml);
                            var nodes = doc.SelectNodes("//partxml/param");
                            StringBuilder sb = new StringBuilder();
                            foreach (XmlNode node in nodes)
                            {
                                string name = node.SelectSingleNode("name")?.InnerText ?? "";
                                string value = node.SelectSingleNode("value")?.InnerText ?? "";
                                if (name == "Define")
                                {
                                    string Define = value;
                                    string[] items = Define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                                    for (var i = 0; i < items.Length; i++)
                                    {
                                        if (i > 0) sb.Append("|||");
                                        var item = items[i];
                                        string[] colcfg = item.Split(new string[] { "##" }, StringSplitOptions.None);
                                        if (colcfg[1] == setKey)
                                        {
                                            for (var j = 0; j < colcfg.Length; j++)
                                            {
                                                if (j > 0) sb.Append("##");
                                                if (j == 7) sb.Append(str.getEncode(setValue));
                                                else sb.Append(colcfg[j]);
                                            }
                                        }
                                        else
                                        {
                                            sb.Append(item);
                                        }
                                    }
                                    break;
                                }
                            }
                            PageWare.setPartParamValue(ref doc, partItem.partid, "Define", sb.ToString());
                            doc = null;
                            #endregion
                        }

                        this.TopMost = false;
                        SitePublish sp = new SitePublish();
                        sp.PublishSinglePageTag = new string[] { pageId, "\\"+row.Cells[2].Value.ToString().Replace("/", "\\"), "16" };
                        sp.PublishSinglePage = true;
                        sp.TopMost = true;
                        sp.ShowDialog();
                        this.TopMost = true;
                    }
                    finally
                    {
                        IsSavePubing = false;
                        row.Cells[7].Value = res.form.GetString("kuaisufabu");
                    }
                }
                else if (e.ColumnIndex == 8)
                {
                    string id = (row.Tag as object[])[0].ToString();
                    if (form.doActive(id))
                    {
                        return;
                    }
                    string path = globalConst.CurSite.Path + "\\" + row.Cells[2].Value.ToString().Replace("/", "\\");

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
                    form.addEditor(path, row.Cells[3].Value.ToString(), id, name, ptype);
                    dataGridView1.ClearSelection();
                }
            }
            catch { }
        }

        bool @break = false;
        private void button1_Click(object sender, EventArgs e)
        {
            @break = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string str = SearchTextBox.Text.Trim();
            if(str!="") DoFind(str);
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button2_Click(sender,null);
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Visible = (row.Cells[6].Style.BackColor == Color.White) || !checkBox1.Checked;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            string leixing = (dataGridView1.Rows[e.RowIndex].Tag as object[])[3].ToString();
            if (e.ColumnIndex == 6 && cell.Style.BackColor == Color.White)
            {
                if (leixing == FTDP.Util.ConfigSearch.StrItems[0])
                {
                    TextEditor te = new TextEditor();
                    te.basetext = cell.Value.ToString();
                    te.StrictField = true;
                    te.TopMost = true;
                    te.ShowDialog();
                    if (!te.cancel)
                    {
                        cell.Value = te.basetext;
                        dataGridView1.EndEdit();
                    }
                }
                else if (leixing == FTDP.Util.ConfigSearch.StrItems[1])
                {
                    TextEditor te = new TextEditor();
                    te.basetext = cell.Value.ToString();
                    te.fromWhere = "dataop";
                    te.ShowInTaskbar = true;
                    te.TopMost = true;
                    te.ShowDialog();
                    if (!te.cancel)
                    {
                        cell.Value = te.basetext;
                        dataGridView1.EndEdit();
                    }
                }
                else if (leixing == FTDP.Util.ConfigSearch.StrItems[2])
                {
                    SQL fd = new SQL();
                    fd.restr = cell.Value.ToString();
                    fd.fromWhere = "dyvalue";
                    fd.ShowInTaskbar = true;
                    fd.TopMost = true;
                    fd.ShowDialog();
                    if (!fd.IsCancel)
                    {
                        cell.Value = fd.restr;
                        dataGridView1.EndEdit();
                    }
                }
            }
        }
    }
}
