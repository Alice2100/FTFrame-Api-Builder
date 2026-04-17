using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FTDPClient.consts;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using ICSharpCode.TextEditor.Document;
using FTDPClient.database;
using System.Web;
using FTDPClient.functions;
using FTDPClient.Front;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using System.Data;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.InkML;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace FTDPClient.forms
{
    public partial class ForePage : Form
    {
        public string SiteID;
        public ForePage()
        {
            InitializeComponent();
            SiteID = globalConst.CurSite.ID;
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }
        void tabPageShowPage()
        {
            tabControl1.TabPages.AddRange(new TabPage[] {
                tp2
            });
        }
        void tabPageShowTemp()
        {
            tabControl1.TabPages.AddRange(new TabPage[] {
                tp1
            });
        }
        private void ForeDev_Load(object sender, EventArgs e)
        {
            this.Text = res.com.str("ToolMenu.QianDuanPage");
            #region language
            tabPage16.Text = res.front.str("Temp.tabPage16");
            tabPage17.Text = res.front.str("Temp.tabPage17");
            button6.Text = res.front.str("Temp.button6");
            button7.Text = res.front.str("Temp.button7");
            button12.Text = res.front.str("Temp.button12");
            tp1.Text = res.front.str("Temp.tp1");
            tp2.Text = res.front.str("Temp.tp2");
            label1.Text = res.front.str("Temp.label1");
            label2.Text = res.front.str("Temp.label2");
            label8.Text = res.front.str("Temp.label8");
            linkLabel1.Text = res.front.str("Temp.linklabel1");
            label7.Text = res.front.str("Temp.label7");
            linkLabel2.Text = res.front.str("Temp.linklabel2");
            label23.Text = res.front.str("Temp.label23");
            button21.Text = res.front.str("Temp.button21");
            button8.Text = res.front.str("Temp.button8");
            label29.Text = res.front.str("Temp.label29");
            label30.Text = res.front.str("Temp.label30");
            label27.Text = res.front.str("Temp.label27");
            BindOrCancel_TempId.Text = res.front.str("Temp.bindorcancel");
            label32.Text = res.front.str("Temp.label32");
            linkLabel4.Text = res.front.str("Temp.linklabel4");
            linkLabel3.Text = res.front.str("Temp.linklabel3");
            label26.Text = res.front.str("Temp.label26");
            label33.Text = res.front.str("Temp.label33");
            button3.Text = res.front.str("Temp.button3");
            button5.Text = res.front.str("Temp.button5");
            button11.Text = res.front.str("Temp.button11");
            button4.Text = res.front.str("Temp.button4");
            Temp_ComDefine.Columns[0].HeaderText= res.front.str("Temp.dgv.comdefine.col.1");
            Temp_ComDefine.Columns[1].HeaderText= res.front.str("Temp.dgv.comdefine.col.2");
            Temp_ComDefine.Columns[2].HeaderText= res.front.str("Temp.dgv.comdefine.col.3");
            Temp_ComDefine.Columns[3].HeaderText= res.front.str("Temp.dgv.comdefine.col.4");
            Temp_ParaDefine.Columns[0].HeaderText = res.front.str("Temp.dgv.paradefine.col.1");
            Temp_ParaDefine.Columns[1].HeaderText = res.front.str("Temp.dgv.paradefine.col.2");
            Temp_ParaDefine.Columns[2].HeaderText = res.front.str("Temp.dgv.paradefine.col.3");
            Page_ComDefine.Columns[0].HeaderText = res.front.str("Temp.dgvpage.comdefine.col.1");
            Page_ComDefine.Columns[1].HeaderText = res.front.str("Temp.dgvpage.comdefine.col.2");
            Page_ComDefine.Columns[2].HeaderText = res.front.str("Temp.dgvpage.comdefine.col.3");
            Page_ComDefine.Columns[3].HeaderText = res.front.str("Temp.dgvpage.comdefine.col.4");
            Page_ParaDefine.Columns[0].HeaderText = res.front.str("Temp.dgvpage.paradefine.col.1");
            Page_ParaDefine.Columns[1].HeaderText = res.front.str("Temp.dgvpage.paradefine.col.2");
            Page_ParaDefine.Columns[2].HeaderText = res.front.str("Temp.dgvpage.paradefine.col.3");
            #endregion
            splitContainer2.SplitterDistance = 824;
            Page_Code.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("HTML");
            Temp_Code.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("HTML");
            listView1.Items.Clear();
            listView1.SmallImageList = imageList1;
            listView2.Items.Clear();
            listView2.SmallImageList = imageList1;
            tabControl1.TabPages.Clear();
            if (!Directory.Exists(globalConst.CurSite.Path + @"\__front"))
            {
                Directory.CreateDirectory(globalConst.CurSite.Path + @"\__front");
                Directory.CreateDirectory(globalConst.CurSite.Path + @"\__front\com");
                Directory.CreateDirectory(globalConst.CurSite.Path + @"\__front\preview");
                dir.Copy(new DirectoryInfo(globalConst.AppPath + @"\front\lib"), new DirectoryInfo(globalConst.CurSite.Path + @"\__front\lib"), null, null, true);
            }
            init_ListView();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "insert into ft_ftdp_front_page(PageName,Caption,TempId,PageCode,ComDefine,ParaDefine,UpdateTime,Developer,IsNewest)";
            sql += "values('NoName','NoName',0,'','','','" + str.GetDateTime() + "','" + (Options.GetSystemValue("qianming") ?? "") + "',1)";
            Adv.RemoteSqlExec(sql);
            sql = "select * from ft_ftdp_front_page where PageName='NoName' order by id desc limit 0,1";

            System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem("NoName - NoName", 3);
            item.ToolTipText = "NoName";
            item.Tag = new object[] { Generator.GetPageObj(Adv.RemoteSqlQuery(sql).Rows[0]) };
            listView1.Items.Add(item);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = "insert into ft_ftdp_front_temp(Caption,TempCode,TempDesc,ComDefine,ParaDefine,UpdateTime,Developer)";
            sql += "values('NoName','','','','','" + str.GetDateTime() + "','" + (Options.GetSystemValue("qianming") ?? "") + "')";
            Adv.RemoteSqlExec(sql);
            sql = "select * from ft_ftdp_front_temp where Caption='NoName' order by id desc limit 0,1";

            System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem("NoName", 4);
            item.ToolTipText = "NoName";
            item.Tag = new object[] { Generator.GetTempObj(Adv.RemoteSqlQuery(sql).Rows[0]) };
            listView2.Items.Add(item);
        }

        private void downToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        System.Windows.Forms.ListViewItem EdiItem = null;
        string EditItemType = "";
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            tabControl1.TabPages.Clear();
            ItemSave();
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count == 1)
            {
                tabPageShowPage();
                ItemInit("Page");
            }
            else EdiItem = null;
        }
        private void listView2_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            tabControl1.TabPages.Clear();
            ItemSave();
            if (listView2.SelectedItems != null && listView2.SelectedItems.Count == 1)
            {
                tabPageShowTemp();
                ItemInit("Temp");
            }
            else EdiItem = null;
        }
        void ItemSave()
        {
            if (EdiItem != null)
            {
                this.Text = res.com.str("ToolMenu.QianDuanPage") + " Saving";
                var developer = Options.GetSystemValue("qianming") ?? "";
                if (EditItemType == "Page")
                {
                    var obj = ((object[])EdiItem.Tag)[0] as FrontPage;
                    int id = obj.Id;
                    var PageName = Page_Name.Text.Trim();
                    var PageCaption = Page_Caption.Text.Trim();
                    var TempId = obj.TempId;
                    var PageCode = TempId > 0 ? "" : Page_Code.Text.Trim();
                    var ComDefine = "";
                    var ComKey = "";
                    var CbStr = "";
                    var NewCom = "";
                    foreach (DataGridViewRow row in Page_ComDefine.Rows)
                    {
                        var tag = row.Tag as object[];
                        var comKey = tag[0].ToString();
                        var cbKey = tag[1].ToString();
                        var newCom = row.Cells[1].Value?.ToString().Trim() ?? "";
                        var newCb = row.Cells[3].Value?.ToString().Trim() ?? "";
                        if (ComKey != "" && ComKey != comKey)
                        {
                            ComDefine += ComKey + "{:::}" + NewCom + "{:::}";
                            ComDefine += CbStr;
                            ComDefine += "{;;;}";
                            CbStr = "";
                            NewCom = "";
                        }
                        if (!string.IsNullOrEmpty(newCom)) NewCom = newCom;
                        CbStr += cbKey + "{::}" + newCb + "{;;}";
                        ComKey = comKey;
                    }
                    if (ComKey != "")
                    {
                        ComDefine += ComKey + "{:::}" + NewCom + "{:::}";
                        ComDefine += CbStr;
                        ComDefine += "{;;;}";
                        CbStr = "";
                    }
                    var ParaDefine = "";
                    foreach (DataGridViewRow row in Page_ParaDefine.Rows)
                    {
                        var tag = row.Tag as object[];
                        var paraKey = tag[0].ToString();
                        var setVal = row.Cells[1].Value?.ToString().Trim() ?? "";
                        ParaDefine += paraKey + "{:::}" + setVal + "{;;;}";
                    }

                    string sql = "update ft_ftdp_front_page set PageName='" + str.D2DD(PageName) + "',Caption='" + str.D2DD(PageCaption) + "',TempId='" + TempId + "',PageCode='" + str.D2DD(PageCode) + "',ComDefine='" + str.D2DD(ComDefine) + "',ParaDefine='" + str.D2DD(ParaDefine) + "',UpdateTime='" + str.GetDateTime() + "',Developer='" + str.D2DD(developer) + "' where id=" + id;
                    Adv.RemoteSqlExec(sql);
                    EdiItem.Tag = new object[] { Generator.GetPageObj(Adv.RemoteSqlQuery("select * from ft_ftdp_front_page where id=" + id).Rows[0]) };
                    EdiItem.Text = PageName + " - " + PageCaption;
                }
                else if (EditItemType == "Temp")
                {
                    var obj = ((object[])EdiItem.Tag)[0] as FrontTemp;
                    int id = obj.Id;
                    var Caption = Temp_Caption.Text.Trim();
                    var TempCode = Temp_Code.Text.Trim();
                    var TempDesc = Temp_Desc.Text.Trim();
                    var ComDefine = "";
                    var ComKey = "";
                    var CbStr = "";
                    var ComDesc = "";
                    foreach (DataGridViewRow row in Temp_ComDefine.Rows)
                    {
                        var comKey = row.Cells[0].Value?.ToString().Trim() ?? "";
                        var comDesc = row.Cells[1].Value?.ToString().Trim() ?? "";
                        var cbKey = row.Cells[2].Value?.ToString().Trim() ?? "";
                        var cbDesc = row.Cells[3].Value?.ToString().Trim() ?? "";
                        if (string.IsNullOrEmpty(comKey)) comKey = ComKey;
                        if (ComKey != "" && ComKey != comKey)
                        {
                            ComDefine += ComKey + "{:::}" + ComDesc + "{:::}";
                            ComDefine += CbStr;
                            ComDefine += "{;;;}";
                            CbStr = "";
                        }
                        if (!string.IsNullOrEmpty(comDesc)) ComDesc = comDesc;
                        if (!string.IsNullOrEmpty(comKey) &&!string.IsNullOrEmpty(cbKey)) CbStr += cbKey + "{::}" + cbDesc + "{;;}";
                        ComKey = comKey;
                    }
                    if (ComKey != "")
                    {
                        ComDefine += ComKey + "{:::}" + ComDesc + "{:::}";
                        ComDefine += CbStr;
                        ComDefine += "{;;;}";
                        CbStr = "";
                    }
                    var ParaDefine = "";
                    foreach (DataGridViewRow row in Temp_ParaDefine.Rows)
                    {
                        var paraKey = row.Cells[0].Value?.ToString().Trim() ?? "";
                        if (paraKey == "") continue;
                        var paraDesc = row.Cells[1].Value?.ToString().Trim() ?? "";
                        var paraDefaultVal = row.Cells[2].Value?.ToString().Trim() ?? "";
                        ParaDefine += paraKey + "{:::}" + paraDesc + "{:::}" + paraDefaultVal + "{;;;}";
                    }
                    string sql = "update ft_ftdp_front_temp set Caption='" + str.D2DD(Caption) + "',TempCode='" + str.D2DD(TempCode) + "',TempDesc='" + str.D2DD(TempDesc) + "',ComDefine='" + str.D2DD(ComDefine) + "',ParaDefine='" + str.D2DD(ParaDefine) + "',UpdateTime='" + str.GetDateTime() + "',Developer='" + str.D2DD(developer) + "' where id=" + id;
                    Adv.RemoteSqlExec(sql);
                    EdiItem.Tag = new object[] { Generator.GetTempObj(Adv.RemoteSqlQuery("select * from ft_ftdp_front_temp where id=" + id).Rows[0]) };
                    EdiItem.Text = Caption;
                }
                this.Text = res.com.str("ToolMenu.QianDuanPage");
            }
        }
        void init_ListView()
        {
            listView1.Items.Clear();
            string sql = "select * from ft_ftdp_front_page where IsNewest=1";
            var dt = Adv.RemoteSqlQuery(sql);
            foreach (DataRow dr in dt.Rows)
            {
                var obj = Generator.GetPageObj(dr);
                var item = listView1.Items.Add(obj.PageName + " - " + obj.Caption, 3);
                item.ToolTipText = obj.PageName + " - " + obj.Caption;
                item.Tag = new object[] { obj };
            }
            listView1.ListViewItemSorter = new ListViewItemComparerPage(2);
            listView1.Sort();
            listView2.Items.Clear();
            sql = "select * from ft_ftdp_front_temp";
            dt = Adv.RemoteSqlQuery(sql);
            foreach (DataRow dr in dt.Rows)
            {
                var obj = Generator.GetTempObj(dr);
                var item = listView2.Items.Add(obj.Caption, 4);
                item.ToolTipText = obj.Caption;
                item.Tag = new object[] { obj };
            }
            listView2.ListViewItemSorter = new ListViewItemComparerTemp(4);
            listView2.Sort();
        }
        void ItemInit(string listType)
        {
            EditItemType = listType;
            if (listType == "Page")
            {
                EdiItem = listView1.SelectedItems[0];
                var obj = ((object[])EdiItem.Tag)[0] as FrontPage;
                Page_Name.Text = obj.PageName;
                Page_Caption.Text = obj.Caption;
                if (obj.TempId == 0)
                {
                    label31.Text = res.front.str("Temp.001");
                    BindOrCancel_TempId.Text = res.front.str("Temp.label27");
                }
                else
                {
                    string sql = "select Caption from ft_ftdp_front_temp where id=" + obj.TempId;
                    var dt = Adv.RemoteSqlQuery(sql);
                    if (dt.Rows.Count == 0)
                    {
                        obj.TempId = 0;
                        label31.Text = res.front.str("Temp.002");
                        BindOrCancel_TempId.Text = res.front.str("Temp.label27");
                    }
                    else
                    {
                        label31.Text = dt.Rows[0][0].ToString();
                        BindOrCancel_TempId.Text = res.front.str("Temp.003");
                    }
                }

                Page_Code.ResetText();
                Page_Code.Text = obj.PageCode;
                Page_ComDefine.Rows.Clear();
                Page_ParaDefine.Rows.Clear();
                if (obj.TempId > 0)
                {
                    string sql = "select * from ft_ftdp_front_temp where id=" + obj.TempId;
                    var tempObj = Generator.GetTempObj(Adv.RemoteSqlQuery(sql).Rows[0]);
                    foreach (var key in tempObj.ComDefine.Keys)
                    {
                        var comCaption = tempObj.ComDefine[key].Caption;
                        var comBind = "";
                        if (obj.ComDefine.ContainsKey(key))
                        {
                            comBind = obj.ComDefine[key].NewCom;
                        }
                        if (tempObj.ComDefine[key].CallBack.Count == 0)
                        {
                            var index = Page_ComDefine.Rows.Add(new string[] { comCaption, comBind, "", "" });
                            Page_ComDefine.Rows[index].Tag = new string[] { key, "" };
                        }
                        else
                        {
                            foreach (var cbKey in tempObj.ComDefine[key].CallBack.Keys)
                            {
                                var cbCaption = tempObj.ComDefine[key].CallBack[cbKey];
                                var cbBind = "";
                                if (obj.ComDefine.ContainsKey(key) && obj.ComDefine[key].CallBack.ContainsKey(cbKey))
                                {
                                    cbBind = obj.ComDefine[key].CallBack[cbKey];
                                }
                                var index = Page_ComDefine.Rows.Add(new string[] { comCaption, comBind, cbKey + ":" + cbCaption, cbBind });
                                Page_ComDefine.Rows[index].Tag = new string[] { key, cbKey };
                                if (index > 0)
                                {
                                    var lastKey = (Page_ComDefine.Rows[index - 1].Tag as string[])[0];
                                    if (lastKey == key)
                                    {
                                        Page_ComDefine.Rows[index].Cells[0].Value = "";
                                        Page_ComDefine.Rows[index].Cells[1].Value = "";
                                    }
                                }
                            }
                        }
                    }
                    foreach (var key in tempObj.ParaDefine.Keys)
                    {
                        var paraDesc = tempObj.ParaDefine[key].Desc;
                        var paraDefaultVal = tempObj.ParaDefine[key].DefaultVal;
                        var paraSetVal = "";
                        if (obj.ParaDefine.ContainsKey(key))
                        {
                            paraSetVal = obj.ParaDefine[key];
                        }
                        var index = Page_ParaDefine.Rows.Add(new string[] { paraDesc, paraSetVal, paraDefaultVal });
                        Page_ParaDefine.Rows[index].Tag = new string[] { key };
                    }
                }
            }
            else if (listType == "Temp")
            {
                EdiItem = listView2.SelectedItems[0];
                var obj = ((object[])EdiItem.Tag)[0] as FrontTemp;
                Temp_Caption.Text = obj.Caption;
                Temp_Desc.Text = obj.TempDesc;
                Temp_Code.ResetText();
                Temp_Code.Text = obj.TempCode;
                Temp_ComDefine.Rows.Clear();
                Temp_ParaDefine.Rows.Clear();
                foreach (var key in obj.ComDefine.Keys)
                {
                    var comCaption = obj.ComDefine[key].Caption;
                    if (obj.ComDefine[key].CallBack.Count == 0)
                    {
                        int index = Temp_ComDefine.Rows.Add(new string[] { key, comCaption, "", "" });
                        Temp_ComDefine.Rows[index].Tag = new string[] { key, "" };
                    }
                    else
                    {
                        foreach (var cbKey in obj.ComDefine[key].CallBack.Keys)
                        {
                            var cbCaption = obj.ComDefine[key].CallBack[cbKey];
                            int index = Temp_ComDefine.Rows.Add(new string[] { key, comCaption, cbKey, cbCaption });
                            Temp_ComDefine.Rows[index].Tag = new string[] { key, cbKey };
                            if (index > 0)
                            {
                                var lastKey = (Temp_ComDefine.Rows[index - 1].Tag as string[])[0];
                                if (lastKey == key)
                                {
                                    Temp_ComDefine.Rows[index].Cells[0].Value = "";
                                    Temp_ComDefine.Rows[index].Cells[1].Value = "";
                                }
                            }
                        }
                    }
                }
                foreach (var key in obj.ParaDefine.Keys)
                {
                    var paraDesc = obj.ParaDefine[key].Desc;
                    var paraDefaultVal = obj.ParaDefine[key].DefaultVal;
                    Temp_ParaDefine.Rows.Add(new string[] { key, paraDesc, paraDefaultVal });
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            yulanToolStripMenuItem_Click(sender, e);
        }

        bool ComNameValidate(string str)
        { return Regex.IsMatch(str, @"^[A-Za-z0-9_]+$"); }
        //save()
        private void button12_Click(object sender, EventArgs e)
        {
            ItemSave();
            if (EdiItem != null)
            {
                if (EditItemType == "Page")
                {
                    if (string.IsNullOrWhiteSpace(Page_Name.Text.Trim()))
                    {
                        MsgBox.Error(res.front.str("Temp.004"));
                        return;
                    }
                    if (!ComNameValidate(Page_Name.Text.Trim()))
                    {
                        MsgBox.Error(res.front.str("Temp.005"));
                        return;
                    }
                }
                else if (EditItemType == "Temp")
                {
                    if (string.IsNullOrWhiteSpace(Temp_Caption.Text.Trim()))
                    {
                        MsgBox.Error(res.front.str("Temp.006"));
                        return;
                    }
                }
            }

            OpInfo("Save successfully");
        }
        System.Timers.Timer OpInfoTimer;
        void OpInfo(string msg)
        {
            this.Text = res.front.str("Temp.007") + "     --      " + msg;
            OpInfoTimer = new System.Timers.Timer();
            OpInfoTimer.AutoReset = false;
            OpInfoTimer.Interval = 2000;
            OpInfoTimer.Elapsed += (sender, e) =>
            {
                new Action(() =>
                {
                    this.Text = res.front.str("Temp.007");
                    OpInfoTimer.Enabled = false;
                }).BeginInvoke(null, null);

            };
            OpInfoTimer.Enabled = true;
        }

        private bool SaveActive()
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count == 1)
            {
                ItemSave();
                var item = listView1.SelectedItems[0];
                string type = ((object[])item.Tag)[0].ToString();
                string ComName = "";
                (string, string, string) g = ("", "", "");
                if (type == "list")
                {
                    var obj = ((object[])item.Tag)[1] as ListCols;
                    ComName = obj.ComName;
                }
                else if (type == "form")
                {
                    var obj = ((object[])item.Tag)[1] as FormCols;
                    ComName = obj.ComName;
                }
                if (string.IsNullOrWhiteSpace(ComName))
                {
                    MsgBox.Error(res.front.str("Temp.008"));
                    return false;
                }
                if (!ComNameValidate(ComName))
                {
                    MsgBox.Error(res.front.str("Temp.009"));
                    return false;
                }
                if (type == "form")
                {
                    var form = (item.Tag as object[])[1] as FormCols;
                    if (form.CustomJs != "" && form.CustomJs.IndexOf("//") < 0)
                    {
                        MsgBox.Error(form.ComName + " - " + form.Caption + Environment.NewLine + res.front.str("Temp.010") + Environment.NewLine + res.front.str("Temp.011"));
                        return false;
                    }
                }
                var st = globalConst.CurSite.SiteConn.db.BeginTransaction();
                try
                {
                    string sql = null;
                    var obj = item.Tag as object[];
                    if (type == "list")
                    {
                        sql = "delete  from front_list where ComName='" + ComName + "'";
                        globalConst.CurSite.SiteConn.execSql(sql, st);
                        var list = obj[1] as ListCols;
                        StringBuilder Rows = new StringBuilder();
                        foreach (var row in list.RowsList)
                        {
                            Rows.Append(row.Caption + "{||||}" + row.Binding + "{||||}" + row.Width + "{||||}" + row.Freezon + "{||||}" + row.IsSort + "{||||}" + row.IsSelection + "{||||}" + row.Template + "{&&&&}");
                        }
                        StringBuilder Search = new StringBuilder();
                        foreach (var row in list.SearchList)
                        {
                            Search.Append(row.Type + "{||||}" + row.Binding + "{||||}" + row.PlaceHolder + "{||||}" + row.Style + "{||||}" + row.InitData + "{&&&&}");
                        }
                        StringBuilder Buttons = new StringBuilder();
                        foreach (var row in list.ButtonList)
                        {
                            Buttons.Append(row.Type + "{||||}" + row.Caption + "{||||}" + row.Icon + "{||||}" + row.IsPlain + "{||||}" + row.IsRound + "{||||}" + row.Size + "{||||}" + row.IsCircle + "{||||}" + row.IsGroupEnd + "{||||}" + row.IsGroupStart + "{||||}" + row.Click + "{||||}" + row.Js + "{&&&&}");
                        }
                        StringBuilder Pager = new StringBuilder();
                        foreach (var row in list.PagerDic)
                        {
                            Pager.Append(row.Key + "{::}" + row.Value + "{;;}");
                        }
                        StringBuilder OtherSet = new StringBuilder();
                        foreach (var row in list.OtherSetDic)
                        {
                            OtherSet.Append(row.Key + "{::}" + row.Value + "{;;}");
                        }
                        sql = "insert into front_list(ComName,Caption,ApiBase,ApiUrl,Rows,Search,Buttons,Pager,InitSet,JsBeforeLoad,JsBeforeSet,JsAfterSet,CustomJs,CssText,OtherSet)";
                        sql += "values(";
                        sql += "'" + str.Dot2DotDot(list.ComName.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(list.Caption.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(list.ApiBase.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(list.ApiUrl.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(Rows.ToString().Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(Search.ToString().Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(Buttons.ToString().Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(Pager.ToString().Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(list.InitSet.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(list.JsBeforeLoad.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(list.JsBeforeSet.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(list.JsAfterSet.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(list.CustomJs.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(list.CssText.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(OtherSet.ToString().Trim()) + "'";
                        sql += ")";
                        globalConst.CurSite.SiteConn.execSql(sql, st);
                    }
                    else if (type == "form")
                    {
                        sql = "delete  from front_form where ComName='" + ComName + "'";
                        globalConst.CurSite.SiteConn.execSql(sql, st);
                        var form = obj[1] as FormCols;
                        StringBuilder Rows = new StringBuilder();
                        foreach (var row in form.RowsList)
                        {
                            Rows.Append(row.Caption + "{||||}" + row.Binding + "{||||}" + row.Type + "{||||}" + row.PlaceHolder + "{||||}" + row.Style + "{||||}" + row.Disable + "{||||}" + row.InitData + "{||||}" + row.Template + "{||||}" + row.ValidateType + "{||||}" + row.ValidateCustomJs + "{||||}" + row.LayoutSpan + "{&&&&}");
                        }
                        StringBuilder Buttons = new StringBuilder();
                        foreach (var row in form.ButtonList)
                        {
                            Buttons.Append(row.Type + "{||||}" + row.Caption + "{||||}" + row.Icon + "{||||}" + row.IsPlain + "{||||}" + row.IsRound + "{||||}" + row.Size + "{||||}" + row.IsCircle + "{||||}" + row.IsGroupEnd + "{||||}" + row.IsGroupStart + "{||||}" + row.Click + "{||||}" + row.Js + "{&&&&}");
                        }
                        StringBuilder OtherSet = new StringBuilder();
                        foreach (var row in form.OtherSetDic)
                        {
                            OtherSet.Append(row.Key + "{::}" + row.Value + "{;;}");
                        }
                        sql = "insert into front_form(ComName,Caption,ApiBase,ApiGet,ApiSet,Rows,Buttons,BindGet,BindSet,JsBeforeSubmit,JsAfterSubmit,JsBeforeGet,JsBeforeSet,JsAfterSet,CustomJs,CssText,CusDataDefine,OtherSet)";
                        sql += "values(";
                        sql += "'" + str.Dot2DotDot(form.ComName.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(form.Caption.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(form.ApiBase.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(form.ApiGet.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(form.ApiSet.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(Rows.ToString().Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(Buttons.ToString().Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(form.BindGet.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(form.BindSet.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(form.JsBeforeSubmit.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(form.JsAfterSubmit.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(form.JsBeforeGet.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(form.JsBeforeSet.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(form.JsAfterSet.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(form.CustomJs.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(form.CssText.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(form.CusDataDefine.Trim()) + "'";
                        sql += ",'" + str.Dot2DotDot(OtherSet.ToString().Trim()) + "'";
                        sql += ")";
                        globalConst.CurSite.SiteConn.execSql(sql, st);
                    }
                    st.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    st.Rollback();
                    MsgBox.Error(ex.Message);
                    return false;
                }
            }
            return false;
        }

        #region Menu操作
        private void sortByNameAscToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.ListViewItemSorter = new ListViewItemComparerPage(2);
            listView1.Sort();
        }

        private void sortByNameDescToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listView1.ListViewItemSorter = new ListViewItemComparerPage(1);
            listView1.Sort();
        }

        private void sortByCaptionAscToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listView1.ListViewItemSorter = new ListViewItemComparerPage(4);
            listView1.Sort();
        }

        private void sortByCaptionDescToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listView1.ListViewItemSorter = new ListViewItemComparerPage(3);
            listView1.Sort();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            button1_Click(null, null);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            button2_Click(null, null);
        }

        private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MsgBox.YesNoCancel("Delete item ?") == DialogResult.Yes)
            {
                if (listView1.SelectedItems != null && listView1.SelectedItems.Count == 1)
                {
                    EdiItem = null;
                    EditItemType = "";
                    int id = ((listView1.SelectedItems[0].Tag as object[])[0] as FrontPage).Id;
                    Adv.RemoteSqlExec("delete from ft_ftdp_front_page where id=" + id);
                    listView1.Items.Remove(listView1.SelectedItems[0]);
                }
            }
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MsgBox.YesNoCancel("Delete all items ?") == DialogResult.Yes)
            {
                EdiItem = null;
                listView1.Items.Clear();
            }
            tabControl1.TabPages.Clear();
        }
        #endregion
        private void button15_Click(object sender, EventArgs e)
        {
        }
        int TabListIndex = 0;
        int TabFormIndex = 0;

        private void button16_Click(object sender, EventArgs e)
        {

            Page_ParaDefine.Rows.Clear();
        }
        private bool PreviewBrowserTaskRuning = false;
        private void ChangedForPreviewBrowser()
        {
            if (ForeBrowser.FB == null) return;
            if (PreviewBrowserTaskRuning) return;
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count == 1)
            {
                ItemSave();
                new Task(() =>
                {
                    PreviewBrowserTaskRuning = true;
                    //Adv.paraDic = null;//每次操作实时取ParaDic
                    //ItemSave();
                    try
                    {
                        var item = listView1.SelectedItems[0];
                        string type = ((object[])item.Tag)[0].ToString();
                        string ComName = "";
                        (string, string, string, string) g = ("", "", "", "");
                        if (type == "list")
                        {
                            var obj = ((object[])item.Tag)[1] as ListCols;
                            ComName = obj.ComName;
                            g = new Generator().List(obj);
                        }
                        else if (type == "form")
                        {
                            var obj = ((object[])item.Tag)[1] as FormCols;
                            ComName = obj.ComName;
                            g = new Generator().Form(obj);
                        }
                        if (!ComNameValidate(ComName))
                        {
                            MsgBox.Error(res.front.str("Temp.012"));
                            return;
                        }
                        string preview_filename = ComName + "_preview.html";
                        preview_filename = globalConst.CurSite.Path + @"\__front\preview\" + preview_filename;
                        using (StreamWriter sw = new StreamWriter(preview_filename, false, Encoding.UTF8))
                        {
                            sw.Write(g.Item3);
                            sw.Flush();
                        }
                        PreviewBrowserLoad(preview_filename);
                    }
                    finally
                    {
                        PreviewBrowserTaskRuning = false;
                    }
                }).Start();
            }
        }
        private void PreviewBrowserLoad(string filepath)
        {
            try
            {
                if (ForeBrowser.FB == null)
                {
                    ForeBrowser fb = new ForeBrowser();
                    fb.TopMost = true;
                    fb.Top = 0;
                    fb.Width = 800;
                    fb.Left = Screen.PrimaryScreen.WorkingArea.Width - fb.Width + 10;
                    fb.Height = Screen.PrimaryScreen.WorkingArea.Height;
                    fb.Show();
                    fb.LoadUrl(filepath + "?" + (DateTime.Now - new DateTime(2000, 1, 1)).TotalMilliseconds);
                }
                else
                {
                    ForeBrowser.FB.LoadUrl(filepath + "?" + (DateTime.Now - new DateTime(2000, 1, 1)).TotalMilliseconds);
                }
            }
            catch (Exception ex)
            {
                //new error(ex);
            }
        }

        private void yulanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EdiItem != null && EditItemType == "Page")
            {
                ItemSave();
                var obj = ((EdiItem.Tag as object[])[0] as FrontPage);
                if (!ComNameValidate(obj.PageName))
                {
                    MsgBox.Error(res.front.str("Temp.013"));
                    return;
                }
                Page_Code.ResetText();
                Page_Code.Text = Generator.FrontPageCode(obj.Id);
                string page_filename = obj.PageName + ".vue";
                string html_filename = obj.PageName + "_test.html";
                string preview_filename = obj.PageName + "_preview.html";
                page_filename = globalConst.CurSite.Path + @"\__front\page\" + page_filename;
                html_filename = globalConst.CurSite.Path + @"\__front\" + html_filename;
                preview_filename = globalConst.CurSite.Path + @"\__front\preview\" + preview_filename;
                //using (StreamWriter sw = new StreamWriter(page_filename, false, Encoding.UTF8))
                //{
                //    sw.Write(g.Item1);
                //    sw.Flush();
                //}
                //using (StreamWriter sw = new StreamWriter(html_filename, false, Encoding.UTF8))
                //{
                //    sw.Write(g.Item2);
                //    sw.Flush();
                //}
                //using (StreamWriter sw = new StreamWriter(preview_filename, false, Encoding.UTF8))
                //{
                //    sw.Write(g.Item3);
                //    sw.Flush();
                //}
                //sheel.ExeSheel(preview_filename);
            }
        }

        private void daochuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EdiItem != null && EditItemType == "Page")
            {
                ItemSave();
                var obj = ((EdiItem.Tag as object[])[0] as FrontPage);
                if (!ComNameValidate(obj.PageName))
                {
                    MsgBox.Error(res.front.str("Temp.013"));
                    return;
                }
                Page_Code.ResetText();
                Page_Code.Text = Generator.FrontPageCode(obj.Id);
                string page_filename = obj.PageName + ".vue";
                string html_filename = obj.PageName + "_test.html";
                string preview_filename = obj.PageName + "_preview.html";
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Vue Component(*.vue)|*.vue";
                sfd.Title = "Vue Component";
                sfd.FileName = obj.PageName + ".vue";
                sfd.ShowDialog();
                if (!string.IsNullOrWhiteSpace(sfd.FileName) && sfd.FileName != (obj.PageName + ".vue"))
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                    {
                        sw.Write(Page_Code.Text);
                        sw.Flush();
                    }
                    if (!string.IsNullOrEmpty(ForeDev.EslintPath) && File.Exists(ForeDev.EslintPath))
                    {
                        var consoleStr = Adv.ConsoleOutput(ForeDev.EslintPath, new string[] {
                        sfd.FileName,
                        "--fix"
                    });
                        if (!string.IsNullOrEmpty(consoleStr)) MsgBox.Warning(consoleStr);
                    }
                }
            }
        }

        private void fabuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count == 1)
            {
                //Adv.paraDic = null;//每次操作实时取ParaDic
                //ItemSave();
                if (!SaveActive()) return;
                var item = listView1.SelectedItems[0];
                string type = ((object[])item.Tag)[0].ToString();
                string ComName = "";
                (string, string, string, string) g = ("", "", "", "");
                if (type == "list")
                {
                    var obj = ((object[])item.Tag)[1] as ListCols;
                    ComName = obj.ComName;
                    g = new Generator().List(obj);
                }
                else if (type == "form")
                {
                    var obj = ((object[])item.Tag)[1] as FormCols;
                    ComName = obj.ComName;
                    g = new Generator().Form(obj);
                }
                if (!ComNameValidate(ComName))
                {
                    MsgBox.Error(res.front.str("Temp.012"));
                    return;
                }
                //string com_filename = ComName + ".vue";
                //string html_filename = ComName + "_test.html";
                //string preview_filename = ComName + "_preview.html";
                //com_filename = globalConst.AppPath + @"\front\com\" + com_filename;
                //html_filename = globalConst.AppPath + @"\front\" + html_filename;
                //using (StreamWriter sw = new StreamWriter(com_filename, false, Encoding.UTF8))
                //{
                //    sw.Write(g.Item1);
                //    sw.Flush();
                //}
                //using (StreamWriter sw = new StreamWriter(html_filename, false, Encoding.UTF8))
                //{
                //    sw.Write(g.Item2);
                //    sw.Flush();
                //}
                string sql = "select * from sites where id='" + globalConst.CurSite.ID + "'";
                SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
                string _url;
                string _id = globalConst.CurSite.ID;
                string _key;
                string _user;
                string _passwd;
                if (rdr.Read())
                {
                    _url = rdr.GetString(rdr.GetOrdinal("url"));
                    _key = rdr.GetString(rdr.GetOrdinal("cdkey"));
                    _user = rdr.GetString(rdr.GetOrdinal("username"));
                    _passwd = rdr.GetString(rdr.GetOrdinal("passwd"));
                }
                else
                {
                    log.Error("siteid is " + globalConst.CurSite.ID + " not found while check server!");
                    return;
                }
                rdr.Close(); rdr = null;
                if (_url.EndsWith("/")) _url = _url.Substring(0, _url.Length - 1);
                string url = _url + "/_ftpub/clientop";
                try
                {
                    List<(int Type, string Prop, string Value)> paras = new List<(int Type, string Prop, string Value)>();
                    paras.Add((0, "_id", _id));
                    paras.Add((0, "_key", _key));
                    paras.Add((0, "_user", _user));
                    paras.Add((0, "_passwd", _passwd));
                    paras.Add((0, "type", "FrontComPublish"));
                    paras.Add((0, "comName", ComName));
                    paras.Add((0, "comText", g.Item1));
                    paras.Add((0, "comHtml", g.Item2));
                    paras.Add((0, "comText2", g.Item4));
                    string reStr = net.HttpPostForm(url, paras).Trim();
                    if (reStr.StartsWith("error:")) MsgBox.Error(reStr);
                    else
                    {
                        MsgBox.Information(res.front.str("Temp.014"));
                        //if (SaveActive())
                        //{
                        SaveToServer(type, ComName);
                        //}
                    }
                }
                catch (Exception ex)
                {
                    new error(ex);
                }
            }
        }
        void SaveToServer(string type, string comName)
        {
            List<string> Sqls = new List<string>();
            if (type == "list")
            {
                string sql = "select * from front_list where ComName='" + comName + "'";
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
                using (DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql)))
                {
                    if (dr.Read())
                    {
                        hasValue = true;
                        ComName = dr.getString("ComName");
                        Caption = dr.getString("Caption");
                        ApiBase = dr.getString("ApiBase");
                        ApiUrl = dr.getString("ApiUrl");
                        Rows = dr.getString("Rows");
                        Search = dr.getString("Search");
                        Buttons = dr.getString("Buttons");
                        Pager = dr.getString("Pager");
                        InitSet = dr.getString("InitSet");
                        JsBeforeLoad = dr.getString("JsBeforeLoad");
                        JsBeforeSet = dr.getString("JsBeforeSet");
                        JsAfterSet = dr.getString("JsAfterSet");
                        CustomJs = dr.getString("CustomJs");
                        CssText = dr.getString("CssText");
                        OtherSet = dr.getString("OtherSet");
                    }
                }
                if (hasValue)
                {
                    Sqls.Add("update ft_ftdp_front_list set IsNewest=0 where ComName='" + str.Dot2DotDot(ComName) + "'");
                    string QianMing = Options.GetSystemValue("qianming") ?? "";
                    sql = "insert into ft_ftdp_front_list(ComName,Caption,ApiBase,ApiUrl,ColRows,Search,Buttons,Pager,InitSet,JsBeforeLoad,JsBeforeSet,JsAfterSet,CustomJs,CssText,OtherSet,CreateTime,Developer,IsNewest)";
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
                    sql += ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    sql += ",'" + str.Dot2DotDot(QianMing) + "'";
                    sql += ",1";
                    sql += ")";
                    Sqls.Add(sql);
                }
            }
            else if (type == "form")
            {
                string sql = "select * from front_form where ComName='" + comName + "'";
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
                using (DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql)))
                {
                    if (dr.Read())
                    {
                        hasValue = true;
                        ComName = dr.getString("ComName");
                        Caption = dr.getString("Caption");
                        ApiBase = dr.getString("ApiBase");
                        ApiGet = dr.getString("ApiGet");
                        ApiSet = dr.getString("ApiSet");
                        Rows = dr.getString("Rows");
                        Buttons = dr.getString("Buttons");
                        BindGet = dr.getString("BindGet");
                        BindSet = dr.getString("BindSet");
                        JsBeforeSubmit = dr.getString("JsBeforeSubmit");
                        JsAfterSubmit = dr.getString("JsAfterSubmit");
                        JsBeforeGet = dr.getString("JsBeforeGet");
                        JsBeforeSet = dr.getString("JsBeforeSet");
                        JsAfterSet = dr.getString("JsAfterSet");
                        CustomJs = dr.getString("CustomJs");
                        CssText = dr.getString("CssText");
                        CusDataDefine = dr.getString("CusDataDefine");
                        OtherSet = dr.getString("OtherSet");
                    }
                }
                if (hasValue)
                {
                    Sqls.Add("update ft_ftdp_front_form set IsNewest=0 where ComName='" + str.Dot2DotDot(ComName) + "'");
                    string QianMing = Options.GetSystemValue("qianming") ?? "";
                    sql = "insert into ft_ftdp_front_form(ComName,Caption,ApiBase,ApiGet,ApiSet,ColRows,Buttons,BindGet,BindSet,JsBeforeSubmit,JsAfterSubmit,JsBeforeGet,JsBeforeSet,JsAfterSet,CustomJs,CssText,CusDataDefine,OtherSet,CreateTime,Developer,IsNewest)";
                    sql += "values(";
                    sql += "'" + str.Dot2DotDot(ComName.Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(Caption.Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(ApiBase.Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(ApiGet.Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(ApiSet.Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(Rows.ToString().Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(Buttons.ToString().Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(BindGet.ToString().Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(BindSet.Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(JsBeforeSubmit.Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(JsAfterSubmit.Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(JsBeforeGet.Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(JsBeforeSet.Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(JsAfterSet.Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(CustomJs.Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(CssText.Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(CusDataDefine.Trim()) + "'";
                    sql += ",'" + str.Dot2DotDot(OtherSet.ToString().Trim()) + "'";
                    sql += ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    sql += ",'" + str.Dot2DotDot(QianMing) + "'";
                    sql += ",1";
                    sql += ")";
                    Sqls.Add(sql);
                }
            }
            string connstr = Options.GetSystemDBSetConnStr_Plat();
            var conntype = Options.GetSystemDBSetType_Plat();
            if (conntype == globalConst.DBType.SqlServer)
            {
                using (SqlConnection db = new SqlConnection(connstr))
                {
                    db.Open();
                    foreach (string sql in Sqls) new SqlCommand(sql, db).ExecuteNonQuery();
                }
            }
            else if (conntype == globalConst.DBType.MySql)
            {
                using (MySqlConnection db = new MySqlConnection(connstr))
                {
                    db.Open();
                    foreach (string sql in Sqls) new MySqlCommand(sql, db).ExecuteNonQuery();
                }
            }
            else if (conntype == globalConst.DBType.Sqlite)
            {
                using (var db = new DB(connstr))
                {
                    db.Open();
                    foreach (string sql in Sqls) db.execSql(sql);
                }
            }
        }

        string KeyDescGet(string apiurl)
        {
            string keyDesc = "";
            string sql = "select KeyDesc from ft_ftdp_apidoc where ApiPath='" + str.Dot2DotDot(apiurl) + "'";
            string connstr = Options.GetSystemDBSetConnStr_Plat();
            var conntype = Options.GetSystemDBSetType_Plat();
            if (conntype == globalConst.DBType.SqlServer)
            {
                using (SqlConnection db = new SqlConnection(connstr))
                {
                    db.Open();
                    using (SqlDataReader dr = new SqlCommand(sql, db).ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            keyDesc = dr.IsDBNull(0) ? "" : dr.GetString(0);
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
                            keyDesc = dr.IsDBNull(0) ? "" : dr.GetString(0);
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
                            keyDesc = dr.IsDBNull(0) ? "" : dr.GetString(0);
                        }
                    }
                }
            }
            return keyDesc;
        }
        void formRowFromApi(string apiurl)
        {
        }

        private void backUpListToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }



        private void button13_Click(object sender, EventArgs e)
        {
            yulanToolStripMenuItem_Click(sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            daochuToolStripMenuItem_Click(sender, e);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            daochuToolStripMenuItem_Click(sender, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fabuToolStripMenuItem_Click(sender, e);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            fabuToolStripMenuItem_Click(sender, e);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string url = globalConst.CurSite.URL.Trim();
            if (!url.EndsWith("/")) url += "/";
            url += "_ft/_base/frontpagelist";
            sheel.ExeSheel(url);
        }


        private void ForeDev_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.E)
            {
                //yulanToolStripMenuItem_Click(sender, e);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.W)
            {
                daochuToolStripMenuItem_Click(sender, e);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Q)
            {
                //fabuToolStripMenuItem_Click(sender, e);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                saveToolStripMenuItem_Click(sender, e);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.P)
            {
                button8_Click(sender, e);
            }
        }


        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count == 1)
            {
                ItemSave();
                var item = listView1.SelectedItems[0];
                var id = ((item.Tag as object[])[0] as FrontPage).Id;
                string sql = "select PageName,Caption,TempId,PageCode,ComDefine,ParaDefine from ft_ftdp_front_page where id=" + id;
                DataRow dr = Adv.RemoteSqlQuery(sql).Rows[0];
                sql = "insert into ft_ftdp_front_page(PageName,Caption,TempId,PageCode,ComDefine,ParaDefine,UpdateTime,Developer,IsNewest)";
                sql += "values('" + str.D2DD(dr["PageName"].ToString()) + "_copy','" + str.D2DD(dr["Caption"].ToString()) + "_copy','" + dr["TempId"].ToString() + "','" + str.D2DD(dr["PageCode"].ToString()) + "','" + str.D2DD(dr["ComDefine"].ToString()) + "','" + str.D2DD(dr["ParaDefine"].ToString()) + "','" + str.GetDateTime() + "','" + (Options.GetSystemValue("qianming") ?? "") + "',1)";
                Adv.RemoteSqlExec(sql);
                tabControl1.TabPages.Clear();
                listView1.ListViewItemSorter = null;
                listView2.ListViewItemSorter = null;
                init_ListView();
                EdiItem = null;
                EditItemType = "";
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button12_Click(sender, e);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            button12_Click(sender, e);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            yulanToolStripMenuItem_Click(sender, e);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            daochuToolStripMenuItem_Click(sender, e);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            fabuToolStripMenuItem_Click(sender, e);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            button12_Click(sender, e);
        }


        private void button27_Click(object sender, EventArgs e)
        {
            ParaDev paraDev = new ParaDev();
            //paraDev.TopMost = true;
            paraDev.ShowDialog();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            button27_Click(sender, e);
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            listView2.ListViewItemSorter = new ListViewItemComparerTemp(4);
            listView2.Sort();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            listView2.ListViewItemSorter = new ListViewItemComparerTemp(3);
            listView2.Sort();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ForeConfig foreConfig = new ForeConfig();
            foreConfig.TopMost = true;
            foreConfig.ShowDialog();
        }

        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            button12_Click(sender, e);
        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            if (MsgBox.YesNoCancel("Delete item ?") == DialogResult.Yes)
            {
                if (listView2.SelectedItems != null && listView2.SelectedItems.Count == 1)
                {
                    EdiItem = null;
                    EditItemType = "";
                    int id = ((listView2.SelectedItems[0].Tag as object[])[0] as FrontTemp).Id;
                    Adv.RemoteSqlExec("delete from ft_ftdp_front_temp where id=" + id);
                    listView2.Items.Remove(listView2.SelectedItems[0]);
                }
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            button2_Click(null, null);
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems != null && listView2.SelectedItems.Count == 1)
            {
                ItemSave();
                var item = listView2.SelectedItems[0];
                var id = ((item.Tag as object[])[0] as FrontTemp).Id;
                string sql = "select Caption,TempCode,TempDesc,ComDefine,ParaDefine from ft_ftdp_front_temp where id=" + id;
                DataRow dr = Adv.RemoteSqlQuery(sql).Rows[0];
                sql = "insert into ft_ftdp_front_temp(Caption,TempCode,TempDesc,ComDefine,ParaDefine,UpdateTime,Developer)";
                sql += "values('" + str.D2DD(dr["Caption"].ToString()) + "_copy','" + str.D2DD(dr["TempCode"].ToString()) + "','" + str.D2DD(dr["TempDesc"].ToString()) + "','" + str.D2DD(dr["ComDefine"].ToString()) + "','" + str.D2DD(dr["ParaDefine"].ToString()) + "','" + str.GetDateTime() + "','" + (Options.GetSystemValue("qianming") ?? "") + "')";
                Adv.RemoteSqlExec(sql);
                tabControl1.TabPages.Clear();
                listView1.ListViewItemSorter = null;
                listView2.ListViewItemSorter = null;
                init_ListView();
                EdiItem = null;
                EditItemType = "";
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count == 1)
            {
                var item = listView1.SelectedItems[0];
                object obj = ((object[])item.Tag)[0];
                ForePageBackUp foreBackUp = new ForePageBackUp();
                foreBackUp.obj = obj;
                foreBackUp.TopMost = true;
                foreBackUp.ShowDialog();
                if (foreBackUp.IsUpdateFromBackUp)
                {
                    tabControl1.TabPages.Clear();
                    listView1.ListViewItemSorter = null;
                    init_ListView();
                    EdiItem = null;
                    EditItemType = "";
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            yulanToolStripMenuItem_Click(sender, e);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            button12_Click(sender, e);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            daochuToolStripMenuItem_Click(sender, e);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            fabuToolStripMenuItem_Click(sender, e);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var code = Temp_Code.Text.Trim();
            var codes=code.Split(new string[] { Environment.NewLine},StringSplitOptions.RemoveEmptyEntries).Select(r=>r.Trim()).ToList();
            var coms = new List<string>();
            foreach(var row in codes)
            {
                if (row.StartsWith("components:"))
                {
                    var str=row.Replace("components:","").Replace("{", "").Replace("}", "");
                    coms= str.Split(new string[] { "," },StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()).ToList();
                    break;
                }
            }
            //[^a-zA-Z0-9_]test_list[^a-zA-Z0-9_]
            //(<test_list[^>]*>)
            var dic = new Dictionary<string, string[]>();
            foreach(string com in coms)
            {
                var cbs = new List<string>();
                Regex r = new Regex(@"(<"+com+@"[^>]*>)");
                MatchCollection mc = r.Matches(code);
                foreach (Match m in mc)
                {
                    var ps=m.Value.Split(new string[] { Environment.NewLine," ","\t".ToString(),"="},StringSplitOptions.RemoveEmptyEntries);
                    foreach(var p in ps)
                    {
                        if(p.Trim().StartsWith("@") && !cbs.Contains(p.Trim()))
                        {
                            cbs.Add(p.Trim());
                        }
                    }
                }
                if (cbs.Count == 0) cbs.Add("");
                dic.Add(com, cbs.ToArray());
            }
            var dicCom = new Dictionary<string, string>();
            var dicCb = new Dictionary<string, string>();
            var lastComId = "";
            foreach(DataGridViewRow row in Temp_ComDefine.Rows)
            {
                var comId = row.Cells[0].Value?.ToString().Trim() ?? "";
                var comDesc = row.Cells[1].Value?.ToString().Trim() ?? "";
                var cbId = row.Cells[2].Value?.ToString().Trim() ?? "";
                var cbDesc = row.Cells[3].Value?.ToString().Trim() ?? "";
                if (comId != "") lastComId = comId;
                if (comId!="" && comDesc!="")
                {
                    if (!dicCom.ContainsKey(comId)) dicCom.Add(comId, comDesc);
                }
                if (cbId != "" && cbDesc != "")
                {
                    if (!dicCb.ContainsKey(lastComId + "_"+cbId)) dicCb.Add(lastComId + "_" + cbId, cbDesc);
                }
            }
            Temp_ComDefine.Rows.Clear();
            foreach(string key in dic.Keys)
            {
                for(int i = 0; i < dic[key].Length;i++)
                {
                    if(i==0)
                    {
                        Temp_ComDefine.Rows.Add(new string[] { key,dicCom.ContainsKey(key)? dicCom[key]:"", dic[key][i],dicCb.ContainsKey(key+"_"+dic[key][i])? dicCb[key + "_" + dic[key][i]]:"" });
                    }
                    else
                    {
                        Temp_ComDefine.Rows.Add(new string[] { "", "", dic[key][i], dicCb.ContainsKey(key + "_" + dic[key][i]) ? dicCb[key + "_" + dic[key][i]] : "" });
                    }
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HTMLText hTMLText = new HTMLText();
            hTMLText.SetVal = Temp_Code.Text;
            hTMLText.TopMost = true;
            hTMLText.ShowDialog();
            if(hTMLText.IsOK)
            {
                Temp_Code.ResetText();
                Temp_Code.Text = hTMLText.SetVal;
            }
        }

        private void BindOrCancel_TempId_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(EdiItem!=null && EditItemType=="Page")
            {
                var obj = (EdiItem.Tag as object[])[0] as FrontPage;
                if(obj.TempId>0)
                {
                    if (MsgBox.YesNoCancel(res.front.str("Temp.015")) == DialogResult.Yes)
                    {
                        string sql = "update ft_ftdp_front_page set TempId=0 where id=" + obj.Id;
                        Adv.RemoteSqlExec(sql);
                        obj.TempId = 0;
                        Page_ComDefine.Rows.Clear();
                        Page_ParaDefine.Rows.Clear();
                        label31.Text = res.front.str("Temp.001");
                        BindOrCancel_TempId.Text = res.front.str("Temp.label27");
                    }
                }
                else
                {
                    ForeSelTemp foreSelTemp = new ForeSelTemp();
                    foreSelTemp.TopMost = true;
                    foreSelTemp.ShowDialog();
                    if(foreSelTemp.IsOK)
                    {
                        string sql = "update ft_ftdp_front_page set TempId="+ foreSelTemp.TempId+ " where id=" + obj.Id;
                        Adv.RemoteSqlExec(sql);
                        obj.TempId = foreSelTemp.TempId;
                        var TempObj = Generator.GetTempObj(Adv.RemoteSqlQuery("select * from ft_ftdp_front_temp where id="+ obj.TempId).Rows[0]);
                        label31.Text = TempObj.Caption;
                        BindOrCancel_TempId.Text = res.front.str("Temp.bindorcancel");
                        ItemInit("Page");
                    }
                    
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(EdiItem!=null && EditItemType=="Temp")
            {
                int tempId = ((EdiItem.Tag as object[])[0] as FrontTemp).Id;
                string sql = "insert into ft_ftdp_front_page(PageName,Caption,TempId,PageCode,ComDefine,ParaDefine,UpdateTime,Developer,IsNewest)";
                sql += "values('NoName_FromTemp','NoName_FromTemp'," + tempId + ",'','','','" + str.GetDateTime() + "','" + (Options.GetSystemValue("qianming") ?? "") + "',1)";
                Adv.RemoteSqlExec(sql);
                sql = "select * from ft_ftdp_front_page where PageName='NoName_FromTemp' order by id desc limit 0,1";
                System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem("NoName_FromTemp - NoName_FromTemp", 3);
                item.ToolTipText = "NoName_FromTemp";
                item.Tag = new object[] { Generator.GetPageObj(Adv.RemoteSqlQuery(sql).Rows[0]) };
                listView1.Items.Add(item);
                MsgBox.Information(res.front.str("Temp.016"));
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(EdiItem!=null && EditItemType=="Page")
            {
                ItemSave();
                int id = ((EdiItem.Tag as object[])[0] as FrontPage).Id;
                Page_Code.ResetText();
                Page_Code.Text = Generator.FrontPageCode(id);
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HTMLText hTMLText = new HTMLText();
            hTMLText.SetVal = Page_Code.Text;
            hTMLText.TopMost = true;
            hTMLText.ShowDialog();
            if (hTMLText.IsOK)
            {
                Page_Code.ResetText();
                Page_Code.Text = hTMLText.SetVal;
            }
        }

        private void Page_ComDefine_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void Page_ComDefine_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                ForeComSelect foreComSelect = new ForeComSelect();
                foreComSelect.TopMost = true;
                foreComSelect.ShowDialog();
                if (foreComSelect.IsOK)
                {
                    Page_ComDefine.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = foreComSelect.component;
                    Page_ComDefine.EndEdit();
                }
            }
            else if (e.ColumnIndex == 3)
            {
                ForeComCallbackSelect foreComCallbackSelect = new ForeComCallbackSelect();
                foreComCallbackSelect.TopMost = true;
                foreComCallbackSelect.ComName = (Page_ComDefine.Rows[e.RowIndex].Tag as object[])[0].ToString();
                foreComCallbackSelect.CallBackDesc = Page_ComDefine.Rows[e.RowIndex].Cells[2].Value?.ToString()??"";
                foreComCallbackSelect.ShowDialog();
                if(!foreComCallbackSelect.IsCancel)
                {
                    Page_ComDefine.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = foreComCallbackSelect.CallBack;
                    Page_ComDefine.EndEdit();
                }
            }
        }
    }
    public class ListViewItemComparerPage : IComparer
    {
        private int stype = 0;
        public ListViewItemComparerPage(int _stype)
        {
            stype = _stype;
        }
        public int Compare(object x, object y)
        {
            string cx = null;
            string cy = null;
            int sortType = stype % 2;
            var obj = ((object[])((System.Windows.Forms.ListViewItem)x).Tag)[0] as FrontPage;
            if (stype == 1 || stype == 2) cx = obj.PageName ?? "";
            else if (stype == 3 || stype == 4) cx = obj.Caption ?? "";
            var obj2 = ((object[])((System.Windows.Forms.ListViewItem)y).Tag)[0] as FrontPage;
            if (stype == 1 || stype == 2) cy = obj2.PageName ?? "";
            else if (stype == 3 || stype == 4) cy = obj2.Caption ?? "";
            if (sortType == 0) return String.Compare(cx, cy);
            else return String.Compare(cy, cx);
        }
    }
    public class ListViewItemComparerTemp : IComparer
    {
        private int stype = 0;
        public ListViewItemComparerTemp(int _stype)
        {
            stype = _stype;
        }
        public int Compare(object x, object y)
        {
            string cx = null;
            string cy = null;
            int sortType = stype % 2;
            var obj = ((object[])((System.Windows.Forms.ListViewItem)x).Tag)[0] as FrontTemp;
            if (stype == 3 || stype == 4) cx = obj.Caption ?? "";
            var obj2 = ((object[])((System.Windows.Forms.ListViewItem)y).Tag)[0] as FrontTemp;
            if (stype == 3 || stype == 4) cy = obj2.Caption ?? "";
            if (sortType == 0) return String.Compare(cx, cy);
            else return String.Compare(cy, cx);
        }
    }
}
