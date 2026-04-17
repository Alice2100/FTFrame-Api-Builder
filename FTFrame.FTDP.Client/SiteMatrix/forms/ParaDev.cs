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
using System.Data;
using ICSharpCode.TextEditor;

namespace FTDPClient.forms
{
    public partial class ParaDev : Form
    {
        public string initParaSel = null;
        public ParaDev()
        {
            InitializeComponent();
        }

        private void ForeDev_Load(object sender, EventArgs e)
        {
            this.Text = res.com.str("ToolMenu.Para");
            button4.Text = res.anew.str("SitePara.button4");
            button3.Text = res.anew.str("SitePara.button3");
            label1.Text = res.anew.str("SitePara.label1");
            label2.Text = res.anew.str("SitePara.label2");
            label3.Text = res.anew.str("SitePara.label3");
            button2.Text = res.anew.str("SitePara.button2");
            listView1.Items.Clear();
            listView1.SmallImageList = imageList1;
            splitContainer2.SplitterDistance = 732;
            SC.SplitterDistance = 730;
            FormResize();
            init_ListView();
        }
        List<Control> EditAreaControlList = new List<Control>();
        void InitEditArea(string val)
        {
            //[#IF#]@code('1')[##]==[##]1
            //[#ELSE#]
            foreach (var ctl in EditAreaControlList) SCC.Panel2.Controls.Remove(ctl);
            EditAreaControlList.Clear();
            if (!val.StartsWith("[#IF#]"))
            {
                TextEditorControl textEditorControl1 = new TextEditorControl();
                textEditorControl1.BackColor = SystemColors.Control;
                textEditorControl1.BorderStyle = BorderStyle.FixedSingle;
                textEditorControl1.Font = new Font(Font.FontFamily, 12F);
                textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("SQL");
                textEditorControl1.ShowLineNumbers = false;
                textEditorControl1.ShowVRuler = false;
                new FTDP.Util.ICSharpTextEditor().Init(this, textEditorControl1, true, null);
                textEditorControl1.ActiveTextAreaControl.TextArea.MouseClick += ActiveTextAreaControl_MouseClick;
                SCC.Panel2.Controls.Add(textEditorControl1);
                textEditorControl1.ResetText();
                textEditorControl1.Text = val;
                textEditorControl1.Refresh();
                EditAreaControlList.Add(textEditorControl1);
            }
            else
            {
                string[] lines = val.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                string curval = "";
                List<string> vals = new List<string>();
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("[#IF#]") || lines[i].StartsWith("[#ELSE#]"))
                    {
                        if (i > 0)
                        {
                            vals.Add(curval.Trim());
                            curval = "";
                        }
                        vals.Add(lines[i].Trim());
                    }
                    else
                    {
                        curval += lines[i] + Environment.NewLine;
                    }
                }
                vals.Add(curval.Trim());
                int scWidth = SCC.Panel2.ClientSize.Width;
                int scHeight = SCC.Panel2.ClientSize.Height - 2;
                int baseH0 = scHeight / (vals.Count / 2);
                int curTop = 0;
                for (int i = 0; i < (vals.Count / 2); i++)
                {
                    int cdnH = 35;
                    int valH = 0;
                    if (i < (vals.Count / 2) - 1) valH = baseH0 - cdnH;
                    else valH = scHeight - (((vals.Count / 2) - 1) * (baseH0)) - cdnH;
                    List<Control> tagControls = new List<Control>();
                    if (vals[2 * i].StartsWith("[#IF#]"))
                    {
                        var cdnItem = vals[2 * i].Substring("[#IF#]".Length).Split(new string[] { "[##]" }, StringSplitOptions.None);
                        TextBox leftBox = new TextBox();
                        leftBox.Text = cdnItem[0];
                        leftBox.Location = new Point(2, curTop + 4);
                        leftBox.Width = 400;
                        leftBox.BackColor = Color.Gainsboro;
                        SCC.Panel2.Controls.Add(leftBox);
                        EditAreaControlList.Add(leftBox);
                        ComboBox comBox = new ComboBox(); 
                        comBox.Items.AddRange(new string[] {
                            "==",">","<",">=","<=","!=","Start","End","Contain","!Start","!End","!Contain"
                        });
                        comBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        comBox.Location = new Point(410, curTop + 5);
                        comBox.Width = 70;
                        comBox.Text = cdnItem[1];
                        SCC.Panel2.Controls.Add(comBox);
                        EditAreaControlList.Add(comBox);
                        TextBox rightBox = new TextBox();//[null]
                        rightBox.Text = cdnItem[2];
                        rightBox.Location = new Point(490, curTop + 4);
                        rightBox.Width = 400;
                        rightBox.BackColor = Color.Gainsboro;
                        SCC.Panel2.Controls.Add(rightBox);
                        EditAreaControlList.Add(rightBox);
                        tagControls.AddRange(new Control[] { leftBox, comBox, rightBox });
                    }
                    else if (vals[2 * i].StartsWith("[#ELSE#]"))
                    {
                        Label label = new Label();
                        label.Text = res.anew.str("SitePara.label.Text");
                        label.Location = new Point(2, curTop + 8);
                        SCC.Panel2.Controls.Add(label);
                        EditAreaControlList.Add(label);
                        tagControls.AddRange(new Control[] { label });
                    }

                    TextEditorControl textEditorControl1 = new TextEditorControl();


                    LinkLabel linkLabel = new LinkLabel();
                    tagControls.Add(textEditorControl1);
                    tagControls.Add(linkLabel);
                    linkLabel.Text = res.anew.str("SitePara.linkLabel.Text");
                    linkLabel.Location = new Point(900, curTop + 8);
                    linkLabel.Tag = tagControls;
                    linkLabel.Click += (object sender, EventArgs e) =>
                    {
                        foreach (var ctl in linkLabel.Tag as List<Control>)
                        {
                            SCC.Panel2.Controls.Remove(ctl);
                            EditAreaControlList.Remove(ctl);
                        }
                        InitEditArea(EditTextVal());
                    };
                    SCC.Panel2.Controls.Add(linkLabel);
                    EditAreaControlList.Add(linkLabel);

                    curTop += cdnH;
                    textEditorControl1.BackColor = SystemColors.Control;
                    textEditorControl1.BorderStyle = BorderStyle.FixedSingle;
                    textEditorControl1.Font = new Font(Font.FontFamily, 12F);
                    textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("SQL");
                    textEditorControl1.ShowLineNumbers = false;
                    textEditorControl1.ShowVRuler = false;
                    new FTDP.Util.ICSharpTextEditor().Init(this, textEditorControl1, true, null);
                    textEditorControl1.ActiveTextAreaControl.TextArea.MouseClick += ActiveTextAreaControl_MouseClick;
                    SCC.Panel2.Controls.Add(textEditorControl1);
                    textEditorControl1.ResetText();
                    textEditorControl1.Text = vals[2 * i + 1];
                    textEditorControl1.Refresh();
                    textEditorControl1.Location = new Point(2, curTop);
                    textEditorControl1.Size = new Size(scWidth, valH);
                    curTop += valH;
                    EditAreaControlList.Add(textEditorControl1);
                }
            }
            FormResize(false);
        }


        void FormResize(bool initEditArea = true)
        {
            if (EditAreaControlList.Count == 1)
            {
                EditAreaControlList[0].Location = new Point(0, 2);
                EditAreaControlList[0].Size = new Size(SCC.Panel2.ClientSize.Width, SCC.Panel2.ClientSize.Height - 2);
            }
            else if (EditAreaControlList.Count > 1)
            {
                if (initEditArea) InitEditArea(EditTextVal());
            }
            button2.Location = new Point(SC.Panel2.Width - 122, 0);
        }
        string EditTextVal()
        {
            if (EditAreaControlList.Count == 1)
            {
                return EditAreaControlList[0].Text;
            }
            else if (EditAreaControlList.Count > 1)
            {
                StringBuilder val = new StringBuilder();
                for (int i = 0; i < EditAreaControlList.Count; i++)
                {
                    if (EditAreaControlList[i].GetType().Name == "LinkLabel")
                    {
                        var controls = EditAreaControlList[i].Tag as List<Control>;
                        if (controls[0].GetType().Name == "Label")
                        {
                            val.AppendLine("[#ELSE#]");
                            val.AppendLine(controls[1].Text);
                        }
                        else
                        {
                            val.AppendLine("[#IF#]" + controls[0].Text + "[##]" + controls[1].Text + "[##]" + controls[2].Text);
                            val.AppendLine(controls[3].Text);
                        }
                    }
                }
                return val.ToString();
            }
            return "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ListParaItem listItem = new ListParaItem()
            {
                FId = rdm.getCombID(),
                ParaName = "",
                ParaCaption = "",
                Mimo = "",
                ParaValue = "",
                IsNew = true,
            };
            ListViewItem item = new ListViewItem("No Name", 0);
            item.ToolTipText = "No Name";
            item.Tag = listItem;
            listView1.Items.Add(item);
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            button1_Click(null, null);
        }

        ListViewItem EdiItem = null;
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ItemSave();
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count == 1)
            {
                ItemInit();
            }
            else EdiItem = null;
        }
        bool ItemSave()
        {
            if (EdiItem != null)
            {
                var obj = EdiItem.Tag as ListParaItem;
                obj.ParaName = paranameBox.Text.Trim();
                obj.ParaCaption = paracaptionBox.Text.Trim();
                obj.Mimo = mimoBox.Text.Trim();
                obj.ParaValue = EditTextVal().Trim();
                if (string.IsNullOrEmpty(obj.ParaName)) EdiItem.Text = "No Name";
                else
                {
                    obj.ParaName = obj.ParaName.Trim();
                    if (!ComNameValidate(obj.ParaName))
                    {
                        MsgBox.Error(res.anew.str("SitePara.Msg1"));
                        return false;
                    }
                    EdiItem.Text = obj.ParaName + " - " + obj.ParaCaption;
                    EdiItem.ToolTipText = obj.Mimo;
                    string sql = null;
                    if (obj.IsNew)
                    {
                        sql = "select count(*) from ft_ftdp_para where paraname='" + str.D2DD(obj.ParaName) + "'";
                        if (int.Parse(Adv.RemoteSqlQuery(sql).Rows[0][0].ToString()) > 0)
                        {
                            MsgBox.Error(res.anew.str("SitePara.Msg2").Replace("{p}", obj.ParaName));
                            return false;
                        }
                        sql = "insert into ft_ftdp_para(fid,paraname,paracaption,mimo,paravalue,stat)";
                        sql += "values('" + obj.FId + "','" + str.D2DD(obj.ParaName) + "','" + str.D2DD(obj.ParaCaption) + "','" + str.D2DD(obj.Mimo) + "','" + str.D2DD(obj.ParaValue) + "',1)";
                        Adv.RemoteSqlExec(sql);
                        obj.IsNew = false;
                    }
                    else
                    {
                        sql = "select count(*) from ft_ftdp_para where paraname='" + str.D2DD(obj.ParaName) + "' and fid!='" + obj.FId + "'";
                        if (int.Parse(Adv.RemoteSqlQuery(sql).Rows[0][0].ToString()) > 0)
                        {
                            MsgBox.Error(res.anew.str("SitePara.Msg2").Replace("{p}", obj.ParaName));
                            return false;
                        }
                        sql = "update ft_ftdp_para set paraname='" + str.D2DD(obj.ParaName) + "',paracaption='" + str.D2DD(obj.ParaCaption) + "',mimo='" + str.D2DD(obj.Mimo) + "',paravalue='" + str.D2DD(obj.ParaValue) + "' where fid='" + obj.FId + "'";
                        Adv.RemoteSqlExec(sql);
                    }
                }
            }
            return true;
        }
        void ItemInit()
        {
            EdiItem = listView1.SelectedItems[0];
            var obj = EdiItem.Tag as ListParaItem;
            string sql = "select paravalue from ft_ftdp_para where fid='" + obj.FId + "'";
            var dt = Adv.RemoteSqlQuery(sql);
            string val = "";
            if (dt.Rows.Count > 0)
            {
                val = dt.Rows[0][0].ToString();
            }
            paranameBox.Text = obj.ParaName;
            paracaptionBox.Text = obj.ParaCaption;
            mimoBox.Text = obj.Mimo;
            InitEditArea(val);
        }

        bool ComNameValidate(string str)
        { 
            if(str.StartsWith("@")) str= str.Substring(1);
            return Regex.IsMatch(str, @"^[A-Za-z0-9_\.]+$"); }
        //save()
        private async void button12_Click(object sender, EventArgs e)
        {
            if (ItemSave())
            {
                OpInfo("Save successfully");
                await System.Threading.Tasks.Task.Run(() => { functions.Adv.CompletionData_Para(); functions.Adv.CompletionData_Enum(); Adv.paraDic = null; _ = Adv.ParaDic(); });
            }
        }
        System.Timers.Timer OpInfoTimer;
        void OpInfo(string msg)
        {
            this.Text = res.com.str("ToolMenu.Para")+"     --      " + msg;
            OpInfoTimer = new System.Timers.Timer();
            OpInfoTimer.AutoReset = false;
            OpInfoTimer.Interval = 2000;
            OpInfoTimer.Elapsed += (sender, e) => {
                this.Text = res.com.str("ToolMenu.Para");
                OpInfoTimer.Enabled = false;
            };
            OpInfoTimer.Enabled = true;
        }
        void init_ListView()
        {
            listView1.Items.Clear();
            string sql = "select * from ft_ftdp_para where stat=1";
            var dt = Adv.RemoteSqlQuery(sql);
            foreach (DataRow row in dt.Rows)
            {
                ListParaItem paraItem = new ListParaItem()
                {
                    FId = row["fid"].ToString(),
                    ParaName = row["paraname"].ToString(),
                    ParaCaption = row["paracaption"].ToString(),
                    Mimo = row["mimo"].ToString(),
                    ParaValue = "",
                    IsNew = false
                };
                ListViewItem item = new ListViewItem(paraItem.ParaName + " - " + paraItem.ParaCaption, 0);
                item.ToolTipText = paraItem.Mimo;
                item.Tag = paraItem;
                if (paraItem.ParaName.StartsWith("Front_")) item.ForeColor = Color.Blue;
                else if (paraItem.ParaName.StartsWith("@Front_")) item.ForeColor = Color.DarkGreen;
                else if (paraItem.ParaName.StartsWith("Enum_")) item.ForeColor = Color.Chocolate;
                listView1.Items.Add(item);
            }
            listView1.ListViewItemSorter = new ListViewItemComparer_Para(2);
            listView1.Sort();
            if(!string.IsNullOrEmpty(initParaSel))
            {
                foreach(ListViewItem item in listView1.Items)
                {
                    if("@para{"+(item.Tag as ListParaItem).ParaName+"}"== initParaSel)
                    {
                        item.Selected = true;
                        listView1_ItemSelectionChanged(null, null);
                        break;
                    }
                }
            }
        }

        #region Menu操作
        private void sortByNameAscToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.ListViewItemSorter = new ListViewItemComparer_Para(2);
            listView1.Sort();
        }

        private void sortByNameDescToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listView1.ListViewItemSorter = new ListViewItemComparer_Para(1);
            listView1.Sort();
        }

        private void sortByCaptionAscToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listView1.ListViewItemSorter = new ListViewItemComparer_Para(4);
            listView1.Sort();
        }

        private void sortByCaptionDescToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listView1.ListViewItemSorter = new ListViewItemComparer_Para(3);
            listView1.Sort();
        }


        private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count == 1)
            {
                var obj = EdiItem.Tag as ListParaItem;
                string sql = "delete from ft_ftdp_para where fid='" + str.D2DD(obj.FId) + "'";
                Adv.RemoteSqlExec(sql);
                EdiItem = null;
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MsgBox.YesNoCancel("Delete all items ?") == DialogResult.Yes)
            {
                string sql = "delete from ft_ftdp_para";
                Adv.RemoteSqlExec(sql);
                EdiItem = null;
                listView1.Items.Clear();
            }
        }
        #endregion

        private void ParaDev_Resize(object sender, EventArgs e)
        {
            FormResize();
        }

        private async void ParaDev_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ItemSave()) e.Cancel = true;
            else
            {
                await System.Threading.Tasks.Task.Run(() => { Adv.CompletionData_Para(); Adv.CompletionData_Enum();Adv.paraDic = null;_ = Adv.ParaDic(); });
                await System.Threading.Tasks.Task.Run(() => { Adv.ClientOperation("ParaDicRefresh",new List<(int Type, string Prop, string Value)>()); });
            }
        }
        char[] chars = new char[] { '\n', '\t', ' ', '\"', '\'', ',', '|', '{', '[', '(', '}', ']', ')', ';', ':', '/', '*', '=', '!', '>', '<' };

        private void ActiveTextAreaControl_MouseClick(object sender, EventArgs e)
        {
            toolTip.Hide(((TextArea)sender).MotherTextEditorControl.ActiveTextAreaControl.TextArea);
            var textArea = ((TextArea)sender).MotherTextEditorControl.ActiveTextAreaControl;
            string lineStr = textArea.Document.GetText(textArea.Document.GetLineSegment(textArea.Caret.Position.Line)) + " ";
            string leftStr = lineStr.Substring(0, lineStr.IndexOfAny(chars, textArea.Caret.Position.X));
            string[] steItems = leftStr.Split(chars);
            string ptnstr = "";
            if (steItems.Length > 0) ptnstr = steItems[steItems.Length - 1];
            if (ptnstr.IndexOf('.') < 0)
            {
                var item = FTDP.Util.ICSharpTextEditor.completionData_Table.Where(r => r[0].ToString().Equals(ptnstr, StringComparison.CurrentCultureIgnoreCase));
                if (item.Count() > 0)
                {
                    toolTip.ToolTipTitle = " " + item.First()[1];
                    toolTip.Show(" " + ptnstr, ((TextArea)sender).MotherTextEditorControl.ActiveTextAreaControl.TextArea);
                }
            }
            else
            {
                string aliasName = ptnstr.Split('.')[0];
                string colName = ptnstr.Split('.')[1];
                var dic = FTDP.Util.ICSharpTextEditor.TableAlias(((TextArea)sender).MotherTextEditorControl.Text);
                string tablename = null;
                if (dic.ContainsKey(aliasName)) tablename = dic[aliasName];
                if (tablename != null)
                {
                    var item = FTDP.Util.ICSharpTextEditor.completionData_Table.Where(r => r[0].ToString().Equals(tablename, StringComparison.CurrentCultureIgnoreCase));
                    if (item.Count() > 0)
                    {
                        string connstr = Options.GetSystemDBSetConnStr();
                        var dbtype = Options.GetSystemDBSetType();
                        var StrictFields = new List<object[]>();
                        try
                        {
                            StrictFields = FTDP.Util.ICSharpTextEditor.GetStrictFields(DBFunc.DBTypeString(dbtype), connstr, tablename);
                        }
                        catch { }
                        toolTip.ToolTipTitle = " " + item.First()[1] + " [" + tablename + "]";
                        var item2 = StrictFields.Where(r => r[0].ToString().Equals(colName, StringComparison.CurrentCultureIgnoreCase));
                        if (item2.Count() > 0)
                        {
                            toolTip.Show(" " + item2.First()[1] + " [" + colName + "]", ((TextArea)sender).MotherTextEditorControl.ActiveTextAreaControl.TextArea);
                        }
                        else
                        {
                            toolTip.Show(" " + ("Not Exist ") + " [" + colName + "]", ((TextArea)sender).MotherTextEditorControl.ActiveTextAreaControl.TextArea);
                        }
                    }
                }
            }
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string val = EditTextVal();
            if (val.IndexOf("[#ELSE#]") < 0)//(!val.StartsWith("[#IF#]"))
            {
                val = "[#IF#]1[##]==[##]1" + Environment.NewLine + val.Trim() + Environment.NewLine + "[#ELSE#]";
                InitEditArea(val);
            }
            else
            {
                val = val.Replace("[#ELSE#]", "[#IF#]1[##]==[##]1" + Environment.NewLine + "" + Environment.NewLine+"[#ELSE#]");
                InitEditArea(val);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HTMLText hTMLText = new HTMLText();
            hTMLText.Text = "Help";
            hTMLText.SetVal = res.anew.str("SitePara.Help1") + Environment.NewLine
                + res.anew.str("SitePara.Help2") + Environment.NewLine
                + res.anew.str("SitePara.Help3") + Environment.NewLine
                + res.anew.str("SitePara.Help4") + Environment.NewLine
                + res.anew.str("SitePara.Help5") + Environment.NewLine
                + res.anew.str("SitePara.Help6") + Environment.NewLine
                + res.anew.str("SitePara.Help7") + Environment.NewLine
                + res.anew.str("SitePara.Help8") + Environment.NewLine
                + res.anew.str("SitePara.Help9") + Environment.NewLine
                + res.anew.str("SitePara.Help10") + Environment.NewLine
                ;
            hTMLText.TopMost = true;
            hTMLText.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            control.Api_List_DataOP api_List_DataOP = new control.Api_List_DataOP();
            api_List_DataOP.TopMost = true;
            api_List_DataOP.ShowDialog();
        }

        private void ParaDev_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                button12_Click(sender, e);
            }
        }
    }
    public class ListParaItem
    {
        public string FId { get; set; }
        public string ParaName { get; set; }
        /// <summary>
        /// 显示的文本文字
        /// </summary>
        public string ParaCaption { get; set; }
        public string Mimo { get; set; }

        public string ParaValue { get; set; }
        public bool IsNew { get; set; }
    }
    public class ListViewItemComparer_Para : IComparer
    {
        private int stype = 0;
        public ListViewItemComparer_Para(int _stype)
        {
            stype = _stype;
        }
        public int Compare(object x, object y)
        {
            string cx = null;
            string cy = null;
            int sortType = stype % 2;
            var objX = ((ListViewItem)x).Tag as ListParaItem;
            var objY = ((ListViewItem)y).Tag as ListParaItem;
            if (stype == 1 || stype == 2) cx = objX.ParaName ?? "";
            else if (stype == 3 || stype == 4) cx = objX.ParaCaption ?? "";
            if (stype == 1 || stype == 2) cy = objY.ParaName ?? "";
            else if (stype == 3 || stype == 4) cy = objY.ParaCaption ?? "";
            if (sortType == 0) return String.Compare(cx, cy);
            else return String.Compare(cy, cx);
        }
    }
}
