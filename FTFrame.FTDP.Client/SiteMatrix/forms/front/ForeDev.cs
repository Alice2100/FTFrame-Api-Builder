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
using CefSharp.DevTools.Accessibility;
using System.Data;
using AngleSharp.Text;
using System.Threading;
using System.Windows;
//using DocumentFormat.OpenXml.Drawing;
//using DocumentFormat.OpenXml.Spreadsheet;
using System.Web.UI.WebControls.WebParts;

namespace FTDPClient.forms
{
    public partial class ForeDev : Form
    {
        public string SiteID;
        public static bool SaveIsExport = false;
        public static string SaveIsExportPath = "";
        public static string EslintPath = "";
        public ForeDev()
        {
            InitializeComponent();
            SiteID = globalConst.CurSite.ID;
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        void tabPageShowList()
        {
            tabControl1.TabPages.AddRange(new TabPage[] {
                tp1,tp2,tp3,tp4,tp5,tp6
            });
        }
        void tabPageShowForm()
        {
            tabControl1.TabPages.AddRange(new TabPage[] {
                tt1,tt2,tt3,tt4,tt5,tt6,tt7
            });
        }
        private void ForeDev_Load(object sender, EventArgs e)
        {
            this.Text = res.com.str("ToolMenu.QianDuan");
            #region language
            button6.Text = res.front.str("Temp.button6");
            button7.Text = res.front.str("Temp.button7");
            button12.Text = res.front.str("Temp.button12");
            tp1.Text = res.ftform.str("tp1"); 
            tp2.Text = res.ftform.str("tp2");
            tp3.Text = res.ftform.str("tp3"); 
            tp4.Text = res.ftform.str("tp4");
            tp5.Text = res.ftform.str("tp5"); 
            tp6.Text = res.ftform.str("tp6"); 
            tt1.Text = res.ftform.str("tt1");
            tt2.Text = res.ftform.str("tt2"); 
            tt3.Text = res.ftform.str("tt3");
            tt4.Text = res.ftform.str("tt4"); 
            tt5.Text = res.ftform.str("tt5");
            tt6.Text = res.ftform.str("tt6"); 
            tt7.Text = res.ftform.str("tt7"); 
            label1.Text = res.ftform.str("label1");
                        label6.Text = res.ftform.str("label1");  
            label2.Text = res.ftform.str("label2");
                        label5.Text = res.ftform.str("label2"); 
            label8.Text = res.ftform.str("label8");
                        label3.Text = res.ftform.str("label8");  
            label7.Text = res.ftform.str("label7"); 
            label4.Text = res.ftform.str("label4"); 
            label9.Text = res.ftform.str("label9");
            button11.Text = res.ftform.str("button11");
                        button8.Text = res.ftform.str("button11");
                        button14.Text = res.ftform.str("button11"); 
            button29.Text = res.ftform.str("button29");
                        button30.Text = res.ftform.str("button29");
                        button31.Text = res.ftform.str("button29"); 
            button3.Text = res.ftform.str("button3");
                        button26.Text = res.ftform.str("button3"); 
            button4.Text = res.ftform.str("button4");
                        button25.Text = res.ftform.str("button4");  
            button5.Text = res.ftform.str("button5");
                        button24.Text = res.ftform.str("button5");  
            button21.Text = res.ftform.str("button21");
                        button23.Text = res.ftform.str("button21");  
            button27.Text = res.ftform.str("button27");
                        button28.Text = res.ftform.str("button27");  
            label23.Text = res.ftform.str("label23");
                        label25.Text = res.ftform.str("label23"); 
                        button15.Text = res.ftform.str("button15"); 
            button16.Text = res.ftform.str("button16");
                        button17.Text = res.ftform.str("button16"); 
            checkBox3.Text = res.ftform.str("checkbox3");
                        checkBox4.Text = res.ftform.str("checkbox3");
                        dataGridView1.Columns[0].HeaderText = res.ftform.str("dataGridView1.col.0"); 
            dataGridView1.Columns[1].HeaderText = res.ftform.str("dataGridView1.col.1");
            dataGridView1.Columns[2].HeaderText = res.ftform.str("dataGridView1.col.2"); 
            dataGridView1.Columns[3].HeaderText = res.ftform.str("dataGridView1.col.3"); 
            dataGridView1.Columns[4].HeaderText = res.ftform.str("dataGridView1.col.4"); 
            dataGridView1.Columns[5].HeaderText = res.ftform.str("dataGridView1.col.5"); 
            dataGridView1.Columns[6].HeaderText = res.ftform.str("dataGridView1.col.6"); 
            dataGridView1.Columns[7].HeaderText = res.ftform.str("dataGridView1.col.7"); 
            dataGridView1.Columns[8].HeaderText = res.ftform.str("dataGridView1.col.8"); 
            dgv2.Columns[0].HeaderText = res.ftform.str("dgv2.col.0"); 
            dgv2.Columns[1].HeaderText = res.ftform.str("dgv2.col.1"); 
            dgv2.Columns[2].HeaderText = res.ftform.str("dgv2.col.2"); 
            dgv2.Columns[3].HeaderText = res.ftform.str("dgv2.col.3"); 
            dgv2.Columns[4].HeaderText = res.ftform.str("dgv2.col.4"); 
            dgv2.Columns[5].HeaderText = res.ftform.str("dgv2.col.5"); 
            dgv2.Columns[6].HeaderText = res.ftform.str("dgv2.col.6"); 
            dgv3.Columns[0].HeaderText = res.ftform.str("dgv3.col.0");
            dgv3.Columns[1].HeaderText = res.ftform.str("dgv3.col.1");
            dgv3.Columns[2].HeaderText = res.ftform.str("dgv3.col.2");
            dgv3.Columns[3].HeaderText = res.ftform.str("dgv3.col.3"); 
            dgv3.Columns[4].HeaderText = res.ftform.str("dgv3.col.4"); 
            dgv3.Columns[5].HeaderText = res.ftform.str("dgv3.col.5"); 
            dgv3.Columns[6].HeaderText = res.ftform.str("dgv3.col.6"); 
            dgv3.Columns[7].HeaderText = res.ftform.str("dgv3.col.7"); 
            dgv3.Columns[8].HeaderText = res.ftform.str("dgv3.col.8"); 
            dgv3.Columns[9].HeaderText = res.ftform.str("dgv3.col.9"); 
            dgv3.Columns[10].HeaderText = res.ftform.str("dgv3.col.10"); 
            dgv3.Columns[11].HeaderText = res.ftform.str("dgv3.col.11");
                        dgv5.Columns[0].HeaderText = res.ftform.str("dgv3.col.0");
                        dgv5.Columns[1].HeaderText = res.ftform.str("dgv3.col.1");
                        dgv5.Columns[2].HeaderText = res.ftform.str("dgv3.col.2");
                        dgv5.Columns[3].HeaderText = res.ftform.str("dgv3.col.3");
                        dgv5.Columns[4].HeaderText = res.ftform.str("dgv3.col.4");
                        dgv5.Columns[5].HeaderText = res.ftform.str("dgv3.col.5");
                        dgv5.Columns[6].HeaderText = res.ftform.str("dgv3.col.6");
                        dgv5.Columns[7].HeaderText = res.ftform.str("dgv3.col.7");
                        dgv5.Columns[8].HeaderText = res.ftform.str("dgv3.col.8");
                        dgv5.Columns[9].HeaderText = res.ftform.str("dgv3.col.9");
                        dgv5.Columns[10].HeaderText = res.ftform.str("dgv3.col.10");
                        dgv5.Columns[11].HeaderText = res.ftform.str("dgv3.col.11");
                        tabPage1.Text = res.ftform.str("tab2.page1");
            tabPage2.Text = res.ftform.str("tab2.page2");
            tabPage3.Text = res.ftform.str("tab2.page3");
            tabPage4.Text = res.ftform.str("tab2.page4"); 
            tabPage5.Text = res.ftform.str("tab2.page5");
            tabPage6.Text = res.ftform.str("tab2.page6");
            label10.Text = res.ftform.str("label10");
            List_Stripe.Text = res.ftform.str("list_stripe"); 
            List_Border.Text = res.ftform.str("list_border"); 
            List_Loading.Text = res.ftform.str("list_loading");
            Form_FormLoading.Text = res.ftform.str("list_loading");
            label11.Text = res.ftform.str("label11"); 
            label12.Text = res.ftform.str("label12"); 
            List_Style.Text = res.ftform.str("list_style"); 
            label13.Text = res.ftform.str("label13"); 
            List_PagerHidden.Text = res.ftform.str("list_pagerhidden"); 
            List_PagerBackground.Text = res.ftform.str("list_pagerbackground"); 
            List_PagerSmall.Text = res.ftform.str("list_pagersmall"); 
            List_PagerTotal.Text = res.ftform.str("list_pagertotal");
            List_PagerJumper.Text = res.ftform.str("list_pagerjumper");
            List111.Text = res.ftform.str("List111"); 
            label22.Text = res.ftform.str("label22"); 
            label24.Text = res.ftform.str("label24"); 
            button18.Text = res.ftform.str("button18"); 
            button19.Text = res.ftform.str("button19"); 
            dgv4.Columns[0].HeaderText = res.ftform.str("dgv4.col.0"); 
            dgv4.Columns[1].HeaderText = res.ftform.str("dgv4.col.1"); 
            dgv4.Columns[2].HeaderText = res.ftform.str("dgv4.col.2"); 
            dgv4.Columns[3].HeaderText = res.ftform.str("dgv4.col.3"); 
            dgv4.Columns[4].HeaderText = res.ftform.str("dgv4.col.4"); 
            dgv4.Columns[5].HeaderText = res.ftform.str("dgv4.col.5"); 
            dgv4.Columns[6].HeaderText = res.ftform.str("dgv4.col.6"); 
            dgv4.Columns[7].HeaderText = res.ftform.str("dgv4.col.7"); 
            dgv4.Columns[8].HeaderText = res.ftform.str("dgv4.col.8"); 
            dgv4.Columns[9].HeaderText = res.ftform.str("dgv4.col.9"); 
            dgv4.Columns[10].HeaderText = res.ftform.str("dgv4.col.10");
            dgv4.Columns[11].HeaderText = res.ftform.str("dgv4.col.11"); 
            button22.Text = res.ftform.str("button22"); 
            BindGetCheckBox.Text = res.ftform.str("bindgetcheckbox"); 
            BindSetCheckBox.Text = res.ftform.str("bindgetcheckbox"); 
            button20.Text = res.ftform.str("button20"); 
            tabPage7.Text = res.ftform.str("tab3.page7"); 
            tabPage8.Text = res.ftform.str("tab3.page8"); 
            tabPage9.Text = res.ftform.str("tab3.page9"); 
            tabPage10.Text = res.ftform.str("tab3.page10"); 
            tabPage13.Text = res.ftform.str("tab3.page13"); 
            tabPage14.Text = res.ftform.str("tab3.page14"); 
            tabPage11.Text = res.ftform.str("tab3.page11"); 
            tabPage15.Text = res.ftform.str("tab3.page15"); 
            tabPage12.Text = res.ftform.str("tab3.page12"); 
            label14.Text = res.ftform.str("label14"); 
            label16.Text = res.ftform.str("label16"); 
            label17.Text = res.ftform.str("label17");
            label15.Text = res.ftform.str("label15"); 
            label18.Text = res.ftform.str("label18"); 
            label19.Text = res.ftform.str("label19"); 

            button9.Text = res.ftform.str("button9");
            button10.Text = res.ftform.str("button10"); 
            button13.Text = res.ftform.str("button13");
            label21.Text = res.ftform.str("label21");
            label20.Text = res.ftform.str("label20"); 
            #endregion

            splitContainer2.SplitterDistance = 724;
            Form_LayoutTempRow.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("XML");
            Form_LayoutTempButton.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("XML");
            //listView1.Items.Clear();
            //listView1.SmallImageList = imageList1;
            tabControl1.TabPages.Clear();
            if (!Directory.Exists(globalConst.CurSite.Path + @"\__front"))
            {
                Directory.CreateDirectory(globalConst.CurSite.Path + @"\__front");
                Directory.CreateDirectory(globalConst.CurSite.Path + @"\__front\com");
                Directory.CreateDirectory(globalConst.CurSite.Path + @"\__front\preview");
                dir.Copy(new DirectoryInfo(globalConst.AppPath + @"\front\lib"), new DirectoryInfo(globalConst.CurSite.Path + @"\__front\lib"), null, null, true);
            }
            //init_ListView();
            init_TreeView();

            foreach (var control in tp6.Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).TextChanged += (object sender2, EventArgs e2) =>
                    {
                        ChangedForPreviewBrowser();
                    };
                }
                else if (control is CheckBox)
                {
                    ((CheckBox)control).Click += (object sender2, EventArgs e2) =>
                    {
                        ChangedForPreviewBrowser();
                    };
                }
            }
            foreach (var control in tt7.Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).TextChanged += (object sender2, EventArgs e2) =>
                    {
                        ChangedForPreviewBrowser();
                    };
                }
                else if (control is CheckBox)
                {
                    ((CheckBox)control).Click += (object sender2, EventArgs e2) =>
                    {
                        ChangedForPreviewBrowser();
                    };
                }
                else if (control is ComboBox)
                {
                    ((ComboBox)control).SelectedIndexChanged += (object sender2, EventArgs e2) =>
                    {
                        ChangedForPreviewBrowser();
                    };
                }
                else if (control is ICSharpCode.TextEditor.TextEditorControl)
                {
                    ((ICSharpCode.TextEditor.TextEditorControl)control).Leave += (object sender2, EventArgs e2) =>
                    {
                        ChangedForPreviewBrowser();
                    };
                }
            }
            foreach (var control in new Control[] { Form_FormInitJs, Form_JsBeforeGet, Form_JsBeforeSet, Form_JsAfterSet, Form_JsBeforeSubmit, Form_JsAfterSubmit, Form_CustomJs, Form_CusDataDefine, Form_CssText })
            {
                ((ICSharpCode.TextEditor.TextEditorControl)control).Leave += (object sender2, EventArgs e2) =>
                {
                    ChangedForPreviewBrowser();
                };
            }
        }
        private TreeNode CurNode= null;
        private void init_TreeView()
        {
            EdiItem = null;
            tree.Nodes.Clear();
            string sql = "select * from front_list";
            using (DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql)))
            {
                while (dr.Read())
                {
                    var obj = GetListColsObj(dr);
                    var names = obj.ComName.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    var findNode = tree_find_and_add(null, names.Take(names.Length - 1).ToArray(), 0);
                    TreeNode node = new TreeNode() { Text = names[names.Length - 1] + " - " + obj.Caption, ImageIndex = 3, SelectedImageIndex = 3 };
                    node.ToolTipText = names[names.Length - 1] + " - " + obj.Caption;
                    node.Tag = new object[] { "list", obj };
                    if (findNode == null) tree.Nodes.Add(node);
                    else findNode.Nodes.Add(node);
                }
            }
            sql = "select * from front_form";
            using (DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql)))
            {
                while (dr.Read())
                {
                    var obj = GetFormColsObj(dr);
                    var names = obj.ComName.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    var findNode = tree_find_and_add(null, names.Take(names.Length - 1).ToArray(), 0);
                    TreeNode node = new TreeNode() { Text = names[names.Length - 1] + " - " + obj.Caption, ImageIndex = 2, SelectedImageIndex = 2 };
                    node.ToolTipText = names[names.Length - 1] + " - " + obj.Caption;
                    node.Tag = new object[] { "form", obj };
                    if (findNode == null) tree.Nodes.Add(node);
                    else findNode.Nodes.Add(node);
                }
            }
            tree.TreeViewNodeSorter = new TreeViewNodesComparer(2);
            tree.Sort();
        }
        void tree_add_item(TreeNode addNode, string text, string caption)
        {
            var names = addNode.Text.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var findNode = tree_find_and_add(null, names.Take(names.Length - 1).ToArray(), 0);
            addNode.Text = names[names.Length - 1] + " - " + caption;
            addNode.ToolTipText = names[names.Length - 1] + " - " + caption;
            if (findNode == null) tree.Nodes.Add(addNode);
            else findNode.Nodes.Add(addNode);
        }
        void tree_move_item(TreeNode moveNode,string text,string caption)
        {
            var names = text.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var findNode = tree_find_and_add(null, names.Take(names.Length - 1).ToArray(), 0);
            moveNode.Text = names[names.Length - 1] + " - " + caption;
            moveNode.ToolTipText = names[names.Length - 1] + " - " + caption;
            if (findNode == null && moveNode.Parent == null) return;
            if (findNode != null && moveNode.Parent == findNode) return;
            TreeNode newNode = (TreeNode)moveNode.Clone();
            if (findNode == null) tree.Nodes.Add(newNode);
            else { findNode.Nodes.Add(newNode); findNode.Expand(); }
            tree_AfterSelect_notrigger = true;
            moveNode.Remove();
            EdiItem = null;
            tabControl1.TabPages.Clear();
            tree_AfterSelect_notrigger = false;
            //EdiItem = newNode;
            //tree.SelectedNode = newNode;
            //ItemInit();
        }
        TreeNode tree_find_and_add(TreeNode rootnode, string[] _paths, int index)
        {
            if (index > _paths.Length - 1) return null;
            var curpath = _paths[index];
            TreeNode findNode = null;
            foreach (TreeNode node in (index == 0 ? tree.Nodes : rootnode.Nodes))
            {
                if (node.Text == _paths[index])
                {
                    findNode = node;
                    break;
                }
            }
            if (findNode == null)
            {
                findNode = new TreeNode() { Text = curpath, ImageIndex = 0, SelectedImageIndex = 0 };
                findNode.Tag = new object[] { "dir", null };
                (index == 0 ? tree.Nodes : rootnode.Nodes).Add(findNode);
            }
            if (index == _paths.Length - 1) return findNode;
            else return tree_find_and_add(findNode, _paths, index + 1);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ListCols listCols = new ListCols()
            {
                ApiBase = "ftdpConfig.apiBase",
                InitSet = @"config.orderBy="""";
config.orderType="""";
config.schText="""";
config.schStrict="""";
config.pageSize=12;
config.pageNum=1;",
                JsBeforeLoad = @"vm.$emit('beforeLoad', vm);
return true;",
                JsBeforeSet = @"//resData is json object return from Api
vm.$emit('beforeSet', vm);
return true;",
                JsAfterSet = @"vm.$emit('afterLoad', vm);
//resData is json object return from Api",
                CustomJs = "",
                CssText = @".el-header, .el-footer {
	text-align: right;
  }",
            };
            TreeNode item = new TreeNode() { Text = "No Name", ImageIndex = 3, SelectedImageIndex = 3 };
            item.Tag = new object[] { "list", listCols };
            tree_add_item(item, "No Name", "");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormCols formCols = new FormCols()
            {
                ApiBase = "ftdpConfig.apiBase",
                JsBeforeGet = @"vm.$emit('beforeGet', vm);
return true;",
                JsBeforeSet = @"	//resData is json object return from Api
vm.$emit('beforeSet', vm)
return true;",
                JsAfterSet = @"//resData is json object return from Api
vm.$emit('afterSet', vm);",
                JsBeforeSubmit = @"vm.$emit('beforeSubmit', vm);
return true;",
                JsAfterSubmit = "vm.$emit('afterSubmit', vm)",
                CustomJs = "",
                CssText = "",
                CusDataDefine = "",
                BindGet = "/* Auto Generate */",
                BindSet = "/* Auto Generate */",
            };
            TreeNode item = new TreeNode() { Text = "No Name", ImageIndex = 2, SelectedImageIndex = 2 };
            item.Tag = new object[] { "form", formCols };
            tree_add_item(item, "No Name", "");
        }

        private void downToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        TreeNode EdiItem = null;
        bool tree_AfterSelect_notrigger = false;
        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tree_AfterSelect_notrigger) return;
            if (CurNode != null)
            {
                CurNode.BackColor = Color.White;
                CurNode.ForeColor = Color.Black;
            }
            tree.SelectedNode.BackColor = SystemColors.Highlight;
            tree.SelectedNode.ForeColor = Color.White;
            CurNode = tree.SelectedNode;

            if (tabControl1.SelectedTab != null)
            {
                if (tabControl1.SelectedTab.Name.StartsWith("tp")) TabListIndex = tabControl1.SelectedIndex;
                else if (tabControl1.SelectedTab.Name.StartsWith("tt")) TabFormIndex = tabControl1.SelectedIndex;
            }
            tabControl1.TabPages.Clear();
            ItemSave();
            if (tree.SelectedNode != null && (tree.SelectedNode.Tag as object[])[0].ToString() != "dir")
            {
                ItemInit();
                ChangedForPreviewBrowser();
            }
            else EdiItem = null;
        }

        void ItemSave()
        {
            if (EdiItem != null)
            {
                string t = ((object[])EdiItem.Tag)[0].ToString();
                if (t == "list")
                {
                    var obj = ((object[])EdiItem.Tag)[1] as ListCols;
                    obj.ComName = List_ComName.Text.Trim();
                    obj.Caption = List_Caption.Text.Trim();
                    obj.ApiBase = List_ApiBase.Text.Trim();
                    obj.ApiUrl = List_ApiUrl.Text.Trim();
                    obj.InitSet = List_InitJs.Text.Trim();
                    obj.JsBeforeLoad = List_JsBeforeLoad.Text.Trim();
                    obj.JsBeforeSet = List_JsBeforeSet.Text.Trim();
                    obj.JsAfterSet = List_JsAfterSet.Text.Trim();
                    obj.CustomJs = List_CustomJs.Text.Trim();
                    obj.CssText = List_CssText.Text.Trim();
                    obj.RowsList = new List<ListColsColumn>();
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        ListColsColumn listColsColumn = new ListColsColumn()
                        {
                            Caption = row.Cells[0].Value?.ToString() ?? "",
                            Binding = row.Cells[1].Value?.ToString() ?? "",
                            Width = row.Cells[2].Value?.ToString() ?? "",
                            Freezon = row.Cells[3].Value?.ToString() ?? "",
                            IsSort = (bool)row.Cells[4].EditedFormattedValue,
                            IsSelection = (bool)row.Cells[5].EditedFormattedValue,
                            Template = row.Cells[6].Value?.ToString() ?? "",
                        };
                        if (listColsColumn.Caption != "" || listColsColumn.Binding != "" || listColsColumn.Template != "") obj.RowsList.Add(listColsColumn);
                    }
                    obj.SearchList = new List<ListColsSearch>();
                    foreach (DataGridViewRow row in dgv2.Rows)
                    {
                        ListColsSearch listColsSon = new ListColsSearch()
                        {
                            Type = row.Cells[0].Value?.ToString() ?? "",
                            Binding = row.Cells[1].Value?.ToString() ?? "",
                            PlaceHolder = row.Cells[2].Value?.ToString() ?? "",
                            Style = row.Cells[3].Value?.ToString() ?? "",
                            InitData = row.Cells[4].Value?.ToString() ?? "",
                        };
                        if (listColsSon.Type == "html" || (listColsSon.Binding != "" && !listColsSon.Binding.EndsWith("."))) obj.SearchList.Add(listColsSon);
                    }
                    obj.ButtonList = new List<ListColsButton>();
                    foreach (DataGridViewRow row in dgv3.Rows)
                    {
                        ListColsButton listColsSon = new ListColsButton()
                        {
                            Type = row.Cells[0].Value?.ToString() ?? "",
                            Caption = row.Cells[1].Value?.ToString() ?? "",
                            Icon = row.Cells[2].Value?.ToString() ?? "",
                            IsPlain = (bool)row.Cells[3].EditedFormattedValue,
                            IsRound = (bool)row.Cells[4].EditedFormattedValue,
                            Size = row.Cells[5].Value?.ToString() ?? "",
                            IsCircle = (bool)row.Cells[6].EditedFormattedValue,
                            IsGroupEnd = (bool)row.Cells[7].EditedFormattedValue,
                            IsGroupStart = (bool)row.Cells[8].EditedFormattedValue,
                            Click = row.Cells[9].Value?.ToString() ?? "",
                            Js = row.Tag == null ? "" : (string)(((object[])row.Tag)[0]),
                        };
                        if (listColsSon.Click != "") obj.ButtonList.Add(listColsSon);
                    }
                    obj.OtherSetDic.Clear();
                    obj.OtherSetDic.Add("List_Stripe", List_Stripe.Checked.ToString());
                    obj.OtherSetDic.Add("List_Border", List_Border.Checked.ToString());
                    obj.OtherSetDic.Add("List_Loading", List_Loading.Checked.ToString());
                    obj.OtherSetDic.Add("List_Height", List_Height.Text.Trim());
                    obj.OtherSetDic.Add("List_MaxHeight", List_MaxHeight.Text.Trim());
                    obj.OtherSetDic.Add("List_TableStyle", List_TableStyle.Text.Trim());
                    obj.PagerDic.Clear();
                    obj.PagerDic.Add("List_PagerHidden", List_PagerHidden.Checked.ToString());
                    obj.PagerDic.Add("List_PagerBackground", List_PagerBackground.Checked.ToString());
                    obj.PagerDic.Add("List_PagerSmall", List_PagerSmall.Checked.ToString());
                    obj.PagerDic.Add("List_PagerTotal", List_PagerTotal.Checked.ToString());
                    obj.PagerDic.Add("List_PagerJumper", List_PagerJumper.Checked.ToString());
                    obj.PagerDic.Add("List_PagerPageSize", List_PagerPageSize.Text.Trim());
                    obj.PagerDic.Add("List_PagerLayout", List_PagerLayout.Text.Trim());
                    if (string.IsNullOrEmpty(obj.ComName)) EdiItem.Text = "No Name";
                    else
                    {
                        EdiItem.Text = obj.ComName + " - " + obj.Caption;
                        EdiItem.ToolTipText = obj.ComName + " - " + obj.Caption;
                        tree_move_item(EdiItem, obj.ComName, obj.Caption);
                    }
                }
                else if (t == "form")
                {
                    var obj = ((object[])EdiItem.Tag)[1] as FormCols;
                    obj.ComName = Form_ComName.Text.Trim();
                    obj.Caption = Form_Caption.Text.Trim();
                    obj.ApiBase = Form_ApiBase.Text.Trim();
                    obj.ApiGet = Form_ApiGet.Text.Trim();
                    obj.ApiSet = Form_ApiSet.Text.Trim();
                    obj.BindGet = Form_BindGet.Text.Trim();
                    obj.BindSet = Form_BindSet.Text.Trim();
                    obj.JsBeforeSubmit = Form_JsBeforeSubmit.Text.Trim();
                    obj.JsAfterSubmit = Form_JsAfterSubmit.Text.Trim();
                    obj.JsBeforeGet = Form_JsBeforeGet.Text.Trim();
                    obj.JsBeforeSet = Form_JsBeforeSet.Text.Trim();
                    obj.JsAfterSet = Form_JsAfterSet.Text.Trim();
                    obj.CustomJs = Form_CustomJs.Text.Trim();
                    obj.CssText = Form_CssText.Text.Trim();
                    obj.CusDataDefine = Form_CusDataDefine.Text.Trim();
                    obj.RowsList = new List<FormColsColumn>();
                    foreach (DataGridViewRow row in dgv4.Rows)
                    {
                        FormColsColumn son = new FormColsColumn()
                        {
                            Caption = row.Cells[0].Value?.ToString() ?? "",
                            Binding = row.Cells[1].Value?.ToString() ?? "",
                            Type = row.Cells[2].Value?.ToString() ?? "",
                            PlaceHolder = row.Cells[3].Value?.ToString() ?? "",
                            Style = row.Cells[4].Value?.ToString() ?? "",
                            Disable = (bool)row.Cells[5].EditedFormattedValue,
                            InitData = row.Cells[6].Value?.ToString() ?? "",
                            Template = row.Cells[7].Value?.ToString() ?? "",
                            ValidateType = row.Cells[8].Value?.ToString() ?? "",
                            ValidateCustomJs = row.Tag == null ? "" : ((object[])row.Tag)[0].ToString(),
                            LayoutSpan = (row.Cells[9].Value?.ToString() ?? "").Trim(),
                        };
                        if (son.Caption != "" || (son.Binding != "" && !son.Binding.EndsWith(".")) || son.Template != "") obj.RowsList.Add(son);
                    }
                    obj.ButtonList = new List<FormColsButton>();
                    foreach (DataGridViewRow row in dgv5.Rows)
                    {
                        FormColsButton son = new FormColsButton()
                        {
                            Type = row.Cells[0].Value?.ToString() ?? "",
                            Caption = row.Cells[1].Value?.ToString() ?? "",
                            Icon = row.Cells[2].Value?.ToString() ?? "",
                            IsPlain = (bool)row.Cells[3].EditedFormattedValue,
                            IsRound = (bool)row.Cells[4].EditedFormattedValue,
                            Size = row.Cells[5].Value?.ToString() ?? "",
                            IsCircle = (bool)row.Cells[6].EditedFormattedValue,
                            IsGroupEnd = (bool)row.Cells[7].EditedFormattedValue,
                            IsGroupStart = (bool)row.Cells[8].EditedFormattedValue,
                            Click = row.Cells[9].Value?.ToString() ?? "",
                            Js = row.Tag == null ? "" : (string)(((object[])row.Tag)[0]),
                        };
                        if (son.Click != "") obj.ButtonList.Add(son);
                    }
                    obj.OtherSetDic.Clear();
                    obj.OtherSetDic.Add("Form_LabelWidth", Form_LabelWidth.Text.Trim());
                    obj.OtherSetDic.Add("Form_FormLoading", Form_FormLoading.Checked.ToString());
                    obj.OtherSetDic.Add("Form_FormStyle", Form_FormStyle.Text.Trim());
                    obj.OtherSetDic.Add("Form_FormInitJs", Form_FormInitJs.Text.Trim());
                    obj.OtherSetDic.Add("Form_LabelPosition", Form_LabelPosition.Text.Trim());
                    obj.OtherSetDic.Add("Form_LayoutTempRow", Form_LayoutTempRow.Text.Trim());
                    obj.OtherSetDic.Add("Form_LayoutTempButton", Form_LayoutTempButton.Text.Trim());
                    if (string.IsNullOrEmpty(obj.ComName)) EdiItem.Text = "No Name";
                    else
                    {
                        EdiItem.Text = obj.ComName + " - " + obj.Caption;
                        EdiItem.ToolTipText = obj.ComName + " - " + obj.Caption;
                        tree_move_item(EdiItem, obj.ComName, obj.Caption);
                    }
                }
            }
        }
        void ItemInit()
        {
            EdiItem = tree.SelectedNode;
            string type = ((object[])EdiItem.Tag)[0].ToString();
            if (type == "list")
            {
                tabPageShowList();
                tabControl1.SelectedIndex = TabListIndex;
                var obj = ((object[])EdiItem.Tag)[1] as ListCols;
                List_ComName.Text = obj.ComName;
                List_Caption.Text = obj.Caption;
                List_ApiBase.Text = obj.ApiBase;
                List_ApiUrl.Text = obj.ApiUrl;
                List_InitJs.ResetText(); List_InitJs.Text = obj.InitSet;
                List_JsBeforeLoad.ResetText(); List_JsBeforeLoad.Text = obj.JsBeforeLoad;
                List_JsBeforeSet.ResetText(); List_JsBeforeSet.Text = obj.JsBeforeSet;
                List_JsAfterSet.ResetText(); List_JsAfterSet.Text = obj.JsAfterSet;
                List_CustomJs.ResetText(); List_CustomJs.Text = obj.CustomJs;
                List_CssText.ResetText(); List_CssText.Text = obj.CssText;
                if (obj.OtherSetDic.ContainsKey("List_Stripe")) List_Stripe.Checked = bool.Parse(obj.OtherSetDic["List_Stripe"]);
                if (obj.OtherSetDic.ContainsKey("List_Border")) List_Border.Checked = bool.Parse(obj.OtherSetDic["List_Border"]);
                if (obj.OtherSetDic.ContainsKey("List_Loading")) List_Loading.Checked = bool.Parse(obj.OtherSetDic["List_Loading"]);
                if (obj.OtherSetDic.ContainsKey("List_Height")) List_Height.Text = obj.OtherSetDic["List_Height"];
                if (obj.OtherSetDic.ContainsKey("List_MaxHeight")) List_MaxHeight.Text = obj.OtherSetDic["List_MaxHeight"];
                if (obj.OtherSetDic.ContainsKey("List_TableStyle")) List_TableStyle.Text = obj.OtherSetDic["List_TableStyle"];
                if (obj.PagerDic.ContainsKey("List_PagerHidden")) List_PagerHidden.Checked = bool.Parse(obj.PagerDic["List_PagerHidden"]);
                if (obj.PagerDic.ContainsKey("List_PagerBackground")) List_PagerBackground.Checked = bool.Parse(obj.PagerDic["List_PagerBackground"]);
                if (obj.PagerDic.ContainsKey("List_PagerSmall")) List_PagerSmall.Checked = bool.Parse(obj.PagerDic["List_PagerSmall"]);
                if (obj.PagerDic.ContainsKey("List_PagerTotal")) List_PagerTotal.Checked = bool.Parse(obj.PagerDic["List_PagerTotal"]);
                if (obj.PagerDic.ContainsKey("List_PagerJumper")) List_PagerJumper.Checked = bool.Parse(obj.PagerDic["List_PagerJumper"]);
                if (obj.PagerDic.ContainsKey("List_PagerPageSize")) List_PagerPageSize.Text = obj.PagerDic["List_PagerPageSize"];
                if (obj.PagerDic.ContainsKey("List_PagerLayout")) List_PagerLayout.Text = obj.PagerDic["List_PagerLayout"];
                dataGridView1.Rows.Clear();
                foreach (var item in obj.RowsList)
                {
                    dataGridView1.Rows.Add(new object[] { item.Caption, item.Binding, item.Width, item.Freezon, item.IsSort, item.IsSelection, item.Template, "Cut", "Insert" });
                }
                dgv2.Rows.Clear();
                foreach (var item in obj.SearchList)
                {
                    dgv2.Rows.Add(new object[] { item.Type, item.Binding, item.PlaceHolder, item.Style, item.InitData, "Cut", "Insert" });
                }
                dgv3.Rows.Clear();
                foreach (var item in obj.ButtonList)
                {
                    int dgvI = dgv3.Rows.Add(new object[] { item.Type, item.Caption, item.Icon, item.IsPlain, item.IsRound, item.Size, item.IsCircle, item.IsGroupEnd, item.IsGroupStart, item.Click, "Cut", "Insert" });
                    dgv3.Rows[dgvI].Tag = new object[] { item.Js };
                }
            }
            else if (type == "form")
            {
                tabPageShowForm();
                tabControl1.SelectedIndex = TabFormIndex;
                var obj = ((object[])EdiItem.Tag)[1] as FormCols;
                Form_ComName.Text = obj.ComName;
                Form_Caption.Text = obj.Caption;
                Form_ApiBase.Text = obj.ApiBase;
                Form_ApiGet.Text = obj.ApiGet;
                Form_ApiSet.Text = obj.ApiSet;
                Form_BindGet.Text = obj.BindGet;
                Form_BindSet.Text = obj.BindSet;
                BindGetCheckBox.Checked = !Form_BindGet.Text.StartsWith("/* Auto Generate */");
                BindSetCheckBox.Checked = !Form_BindSet.Text.StartsWith("/* Auto Generate */");
                Form_BindGet.Enabled = BindGetCheckBox.Checked;
                Form_BindSet.Enabled = BindSetCheckBox.Checked;
                button22.Enabled = BindGetCheckBox.Checked;
                button20.Enabled = BindSetCheckBox.Checked;
                Form_JsBeforeSubmit.ResetText(); Form_JsBeforeSubmit.Text = obj.JsBeforeSubmit;
                Form_JsAfterSubmit.ResetText(); Form_JsAfterSubmit.Text = obj.JsAfterSubmit;
                Form_JsBeforeGet.ResetText(); Form_JsBeforeGet.Text = obj.JsBeforeGet;
                Form_JsBeforeSet.ResetText(); Form_JsBeforeSet.Text = obj.JsBeforeSet;
                Form_JsAfterSet.ResetText(); Form_JsAfterSet.Text = obj.JsAfterSet;
                Form_CustomJs.ResetText(); Form_CustomJs.Text = obj.CustomJs;
                Form_CssText.ResetText(); Form_CssText.Text = obj.CssText;
                Form_CusDataDefine.ResetText(); Form_CusDataDefine.Text = obj.CusDataDefine;
                if (obj.OtherSetDic.ContainsKey("Form_LabelWidth")) Form_LabelWidth.Text = obj.OtherSetDic["Form_LabelWidth"];
                if (obj.OtherSetDic.ContainsKey("Form_FormLoading")) Form_FormLoading.Checked = bool.Parse(obj.OtherSetDic["Form_FormLoading"]);
                if (obj.OtherSetDic.ContainsKey("Form_FormStyle")) Form_FormStyle.Text = obj.OtherSetDic["Form_FormStyle"];
                if (obj.OtherSetDic.ContainsKey("Form_FormInitJs"))
                {
                    Form_FormInitJs.ResetText();
                    Form_FormInitJs.Text = obj.OtherSetDic["Form_FormInitJs"];
                }
                if (obj.OtherSetDic.ContainsKey("Form_LabelPosition")) Form_LabelPosition.Text = obj.OtherSetDic["Form_LabelPosition"];
                if (obj.OtherSetDic.ContainsKey("Form_LayoutTempRow"))
                {
                    Form_LayoutTempRow.ResetText();
                    Form_LayoutTempRow.Text = obj.OtherSetDic["Form_LayoutTempRow"];
                }
                if (obj.OtherSetDic.ContainsKey("Form_LayoutTempButton"))
                {
                    Form_LayoutTempButton.ResetText();
                    Form_LayoutTempButton.Text = obj.OtherSetDic["Form_LayoutTempButton"];
                }
                dgv4.Rows.Clear();
                foreach (var item in obj.RowsList)
                {
                    int dgvI = dgv4.Rows.Add(new object[] { item.Caption, item.Binding, item.Type, item.PlaceHolder, item.Style, item.Disable, item.InitData, item.Template, item.ValidateType, item.LayoutSpan, "Cut", "Insert" });
                    dgv4.Rows[dgvI].Tag = new object[] { item.ValidateCustomJs };
                }
                dgv5.Rows.Clear();
                foreach (var item in obj.ButtonList)
                {
                    int dgvI = dgv5.Rows.Add(new object[] { item.Type, item.Caption, item.Icon, item.IsPlain, item.IsRound, item.Size, item.IsCircle, item.IsGroupEnd, item.IsGroupStart, item.Click, "Cut", "Insert" });
                    dgv5.Rows[dgvI].Tag = new object[] { item.Js };
                }
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            ForeConfig foreConfig = new ForeConfig();
            foreConfig.TopMost = true;
            foreConfig.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            yulanToolStripMenuItem_Click(sender, e);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ForeSelectApi foreSelectApi = new ForeSelectApi();
            foreSelectApi.type = "List";
            foreSelectApi.TopMost = true;
            foreSelectApi.ShowDialog();
            if (!foreSelectApi.IsCancel) List_ApiUrl.Text = foreSelectApi.SetVal;
        }
        bool ComNameValidate(string str)
        { return Regex.IsMatch(str.Replace("/", ""), @"^[A-Za-z0-9_]+$"); }
        string ComNameNoPath(string str)
        { return str.Replace("/", "-d-"); }
        string ComNameJustName(string str)
        {
            var names = str.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            return names[names.Length - 1];
        }
        string ComNameToPath(string str)
        { return str.Replace("-d-", "/"); }
        //save()
        public List<TreeNode> tree_getAllNodes(TreeNode rootnode = null)
        {
            List<TreeNode> lst = new List<TreeNode>();
            foreach (TreeNode n in (rootnode == null ? tree.Nodes : rootnode.Nodes))
            {
                lst.Add(n);
                lst.AddRange(tree_getAllNodes(n));
            }
            return lst;
        }
        private bool LayoutSpanCheck(FormCols form)
        {
            var layoutMsg = form.ComName + " - " + form.Caption + Environment.NewLine + res.ftform.str("z001") + Environment.NewLine + res.ftform.str("z002");
            var spanList = form.RowsList.Select(r => r.LayoutSpan).ToList();
            if (spanList.Where(r => r != "").Count() > 0)
            {
                if (spanList.Where(r => r == "").Count() > 0)
                {
                    MsgBox.Error(layoutMsg); return false;
                }
                for (int i = 0; i < spanList.Count; i++)
                {
                    int sums = 0;
                    for (int j = i; j < spanList.Count; j++)
                    {
                        int cur = 0;
                        if (!int.TryParse(spanList[j], out cur))
                        {
                            MsgBox.Error(layoutMsg); return false;
                        }
                        if (cur < 1 || cur > 24)
                        {
                            MsgBox.Error(layoutMsg); return false;
                        }
                        sums += cur;
                        if (sums == 24)
                        {
                            i = j;
                            break;
                        }
                        else if (sums > 24)
                        {
                            MsgBox.Error(layoutMsg); return false;
                        }
                        else if ((j == spanList.Count - 1) && sums < 24)
                        {
                            MsgBox.Error(layoutMsg); return false;
                        }
                    }
                }
            }
            return true;
        }
        private void button12_Click(object sender, EventArgs e)
        {
            ItemSave();
            List<string> comNames = new List<string>();

            if (!LoopNodes(tree.Nodes)) return;

            var st = globalConst.CurSite.SiteConn.db.BeginTransaction();
            try
            {
                string sql = "delete  from front_list";
                globalConst.CurSite.SiteConn.execSql(sql, st);
                sql = "delete  from front_form";
                globalConst.CurSite.SiteConn.execSql(sql, st);
                foreach (TreeNode item in tree_getAllNodes())
                {
                    var obj = item.Tag as object[];
                    if (obj[0].ToString() == "list")
                    {
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
                    else if (obj[0].ToString() == "form")
                    {
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
                }
                st.Commit();
            }
            catch (Exception ex)
            {
                st.Rollback();
                MsgBox.Error(ex.Message);
                return;
            }
            //保存既导出
            if(SaveIsExport && !string.IsNullOrEmpty(SaveIsExportPath))
            {
                if (tree.SelectedNode != null && (tree.SelectedNode.Tag as object[])[0].ToString() != "dir")
                {
                    var item = tree.SelectedNode;
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
                    var filePath = SaveIsExportPath + "\\" + ComName;
                    filePath = filePath.Replace("/","\\").Replace("\\\\", "\\");
                    filePath += ".vue";
                    var dir = filePath.Substring(0, filePath.LastIndexOf('\\'));
                    Directory.CreateDirectory(dir);
                    using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
                    {
                        sw.Write(g.Item4);
                        sw.Flush();
                    }
                    if(!string.IsNullOrEmpty(ForeDev.EslintPath) && File.Exists(ForeDev.EslintPath))
                    {
                        var consoleStr = Adv.ConsoleOutput(ForeDev.EslintPath, new string[] {
                        filePath,
                        "--fix"
                    });
                        if(!string.IsNullOrEmpty(consoleStr))MsgBox.Warning(consoleStr);
                    }
                    
                }
            }

            OpInfo("Save successfully");

            bool LoopNodes(TreeNodeCollection nodes)
            {
                foreach (TreeNode item in nodes)
                {
                    var obj = item.Tag as object[];
                    if (obj[0].ToString() == "list")
                    {
                        var list = obj[1] as ListCols;
                        if (string.IsNullOrWhiteSpace(list.ComName))
                        {
                            MsgBox.Error(res.ftform.str("z003"));
                            return false;
                        }
                        if (comNames.Contains(list.ComName.Trim()))
                        {
                            MsgBox.Error(res.ftform.str("z004") + list.ComName);
                            return false;
                        }
                        if (!ComNameValidate(list.ComName.Trim()))
                        {
                            MsgBox.Error(res.ftform.str("z005"));
                            return false;
                        }
                        if (list.CustomJs != "" && list.CustomJs.IndexOf("//") < 0)
                        {
                            MsgBox.Error(list.ComName + " - " + list.Caption + Environment.NewLine + res.ftform.str("z006") + Environment.NewLine + res.ftform.str("z007"));
                            return false;
                        }
                        comNames.Add(list.ComName.Trim());
                    }
                    else if (obj[0].ToString() == "form")
                    {
                        var form = obj[1] as FormCols;
                        if (string.IsNullOrWhiteSpace(form.ComName))
                        {
                            MsgBox.Error(res.ftform.str("z003"));
                            return false;
                        }
                        if (comNames.Contains(form.ComName.Trim()))
                        {
                            MsgBox.Error(res.ftform.str("z004") + form.ComName);
                            return false;
                        }
                        if (form.CustomJs != "" && form.CustomJs.IndexOf("//") < 0)
                        {
                            MsgBox.Error(form.ComName + " - " + form.Caption + Environment.NewLine + res.ftform.str("z006") + Environment.NewLine + res.ftform.str("z007"));
                            return false;
                        }
                        if (!LayoutSpanCheck(form)) return false;
                        comNames.Add(form.ComName.Trim());
                    }
                    return LoopNodes(item.Nodes);
                }
                return true;
            }
        }
        System.Timers.Timer OpInfoTimer;
        void OpInfo(string msg)
        {
            this.Text = res.ftform.str("z008") + "     --      " + msg;
            OpInfoTimer = new System.Timers.Timer();
            OpInfoTimer.AutoReset = false;
            OpInfoTimer.Interval = 2000;
            OpInfoTimer.Elapsed += (sender, e) =>
            {
                this.Text = res.ftform.str("z008");
                OpInfoTimer.Enabled = false;
            };
            OpInfoTimer.Enabled = true;
        }

        private bool SaveActive()
        {
            if (tree.SelectedNode!=null && (tree.SelectedNode.Tag as object[])[0].ToString() != "dir")
            {
                ItemSave();
                var item = tree.SelectedNode;
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
                    MsgBox.Error(res.ftform.str("z009"));
                    return false;
                }
                if (!ComNameValidate(ComName))
                {
                    MsgBox.Error(res.ftform.str("z005"));
                    return false;
                }
                if (type == "form")
                {
                    var form = (item.Tag as object[])[1] as FormCols;
                    if (form.CustomJs != "" && form.CustomJs.IndexOf("//") < 0)
                    {
                        MsgBox.Error(form.ComName + " - " + form.Caption + Environment.NewLine + res.ftform.str("z006") + Environment.NewLine + res.ftform.str("z007"));
                        return false;
                    }
                    if (!LayoutSpanCheck(form)) return false;
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
        public static ListCols GetListColsObj(DR dr)
        {
            var obj = Adv.DRToType<ListCols>(dr);
            obj.RowsList = new List<ListColsColumn>();
            string[] rows0 = dr.getString("Rows").Split(new string[] { "{&&&&}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rows1 in rows0)
            {
                string[] rows2 = rows1.Split(new string[] { "{||||}" }, StringSplitOptions.None);
                obj.RowsList.Add(new ListColsColumn()
                {
                    Caption = rows2[0],
                    Binding = rows2[1],
                    Width = rows2[2],
                    Freezon = rows2[3],
                    IsSort = bool.Parse(rows2[4]),
                    IsSelection = bool.Parse(rows2[5]),
                    Template = rows2[6],
                });
            }
            obj.SearchList = new List<ListColsSearch>();
            string[] rows0a = dr.getString("Search").Split(new string[] { "{&&&&}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rows1 in rows0a)
            {
                string[] rows2 = rows1.Split(new string[] { "{||||}" }, StringSplitOptions.None);
                obj.SearchList.Add(new ListColsSearch()
                {
                    Type = rows2[0],
                    Binding = rows2[1],
                    PlaceHolder = rows2[2],
                    Style = rows2[3],
                    InitData = rows2[4],
                });
            }
            obj.ButtonList = new List<ListColsButton>();
            string[] rows0b = dr.getString("Buttons").Split(new string[] { "{&&&&}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rows1 in rows0b)
            {
                string[] rows2 = rows1.Split(new string[] { "{||||}" }, StringSplitOptions.None);
                obj.ButtonList.Add(new ListColsButton()
                {
                    Type = rows2[0],
                    Caption = rows2[1],
                    Icon = rows2[2],
                    IsPlain = bool.Parse(rows2[3]),
                    IsRound = bool.Parse(rows2[4]),
                    Size = rows2[5],
                    IsCircle = bool.Parse(rows2[6]),
                    IsGroupEnd = bool.Parse(rows2[7]),
                    IsGroupStart = bool.Parse(rows2[8]),
                    Click = rows2[9],
                    Js = rows2[10],

                });
            }
            obj.PagerDic = new Dictionary<string, string>();
            string[] rows0c = dr.getString("Pager").Split(new string[] { "{;;}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rows1 in rows0c)
            {
                string[] rows2 = rows1.Split(new string[] { "{::}" }, StringSplitOptions.None);
                obj.PagerDic.Add(rows2[0], rows2[1]);
            }
            obj.OtherSetDic = new Dictionary<string, string>();
            string[] rows0d = dr.getString("OtherSet").Split(new string[] { "{;;}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rows1 in rows0d)
            {
                string[] rows2 = rows1.Split(new string[] { "{::}" }, StringSplitOptions.None);
                obj.OtherSetDic.Add(rows2[0], rows2[1]);
            }
            return obj;
        }
        public static FormCols GetFormColsObj(DR dr)
        {
            var obj = Adv.DRToType<FormCols>(dr);
            obj.RowsList = new List<FormColsColumn>();
            string[] rows0 = dr.getString("Rows").Split(new string[] { "{&&&&}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rows1 in rows0)
            {
                string[] rows2 = rows1.Split(new string[] { "{||||}" }, StringSplitOptions.None);
                obj.RowsList.Add(new FormColsColumn()
                {
                    Caption = rows2[0],
                    Binding = rows2[1],
                    Type = rows2[2],
                    PlaceHolder = rows2[3],
                    Style = rows2[4],
                    Disable = bool.Parse(rows2[5]),
                    InitData = rows2[6],
                    Template = rows2[7],
                    ValidateType = rows2.Length > 8 ? rows2[8] : "",
                    ValidateCustomJs = rows2.Length > 9 ? rows2[9] : "",
                    LayoutSpan = (rows2.Length > 10 ? rows2[10] : "").Trim(),
                });
            }
            obj.ButtonList = new List<FormColsButton>();
            string[] rows0b = dr.getString("Buttons").Split(new string[] { "{&&&&}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rows1 in rows0b)
            {
                string[] rows2 = rows1.Split(new string[] { "{||||}" }, StringSplitOptions.None);
                obj.ButtonList.Add(new FormColsButton()
                {
                    Type = rows2[0],
                    Caption = rows2[1],
                    Icon = rows2[2],
                    IsPlain = bool.Parse(rows2[3]),
                    IsRound = bool.Parse(rows2[4]),
                    Size = rows2[5],
                    IsCircle = bool.Parse(rows2[6]),
                    IsGroupEnd = bool.Parse(rows2[7]),
                    IsGroupStart = bool.Parse(rows2[8]),
                    Click = rows2[9],
                    Js = rows2[10],

                });
            }
            obj.OtherSetDic = new Dictionary<string, string>();
            string[] rows0d = dr.getString("OtherSet").Split(new string[] { "{;;}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rows1 in rows0d)
            {
                string[] rows2 = rows1.Split(new string[] { "{::}" }, StringSplitOptions.None);
                obj.OtherSetDic.Add(rows2[0], rows2[1]);
            }
            return obj;
        }
        //void init_ListView()
        //{
        //    listView1.Items.Clear();
        //    string sql = "select * from front_list";
        //    using (DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql)))
        //    {
        //        while (dr.Read())
        //        {
        //            var obj = GetListColsObj(dr);
        //            var item = listView1.Items.Add(obj.ComName + " - " + obj.Caption, 2);
        //            item.ToolTipText = obj.ComName + " - " + obj.Caption;
        //            item.Tag = new object[] { "list", obj };
        //        }
        //    }
        //    sql = "select * from front_form";
        //    using (DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql)))
        //    {
        //        while (dr.Read())
        //        {
        //            var obj = GetFormColsObj(dr);
        //            var item = listView1.Items.Add(obj.ComName + " - " + obj.Caption, 0);
        //            item.ToolTipText = obj.ComName + " - " + obj.Caption;
        //            item.Tag = new object[] { "form", obj };
        //        }
        //    }
        //    listView1.ListViewItemSorter = new ListViewItemComparer(2);
        //    listView1.Sort();
        //}

        private void button8_Click(object sender, EventArgs e)
        {
            ForeSelectApi foreSelectApi = new ForeSelectApi();
            foreSelectApi.type = "Form_Get";
            foreSelectApi.TopMost = true;
            foreSelectApi.ShowDialog();
            if (!foreSelectApi.IsCancel) Form_ApiGet.Text = foreSelectApi.SetVal;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ForeSelectApi foreSelectApi = new ForeSelectApi();
            foreSelectApi.type = "Form_Set";
            foreSelectApi.TopMost = true;
            foreSelectApi.ShowDialog();
            if (!foreSelectApi.IsCancel) Form_ApiSet.Text = foreSelectApi.SetVal;
        }
        #region Menu操作
        private void sortByNameAscToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tree.TreeViewNodeSorter = new TreeViewNodesComparer(2);
            tree.Sort();
            //listView1.ListViewItemSorter = new ListViewItemComparer(2);
            //listView1.Sort();
        }

        private void sortByNameDescToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tree.TreeViewNodeSorter = new TreeViewNodesComparer(1);
            tree.Sort();
            //listView1.ListViewItemSorter = new ListViewItemComparer(1);
            //listView1.Sort();
        }

        private void sortByCaptionAscToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tree.TreeViewNodeSorter = new TreeViewNodesComparer(4);
            tree.Sort();
            //listView1.ListViewItemSorter = new ListViewItemComparer(4);
            //listView1.Sort();
        }

        private void sortByCaptionDescToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tree.TreeViewNodeSorter = new TreeViewNodesComparer(3);
            tree.Sort();
            //listView1.ListViewItemSorter = new ListViewItemComparer(3);
            //listView1.Sort();
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
            if (tree.SelectedNode!=null && (tree.SelectedNode.Tag as object[])[0].ToString() != "dir")
            {
                if (MsgBox.YesNoCancel("Delete this items ?") == DialogResult.Yes)
                {
                    string t = ((object[])tree.SelectedNode.Tag)[0].ToString();
                    var sql = "";
                    if (t == "list")
                    {
                        var obj = ((object[])EdiItem.Tag)[1] as ListCols;
                        sql = "delete from front_list where ComName='"+str.D2DD(obj.ComName) +"'";
                    }
                    else if (t == "form")
                    {
                        var obj = ((object[])EdiItem.Tag)[1] as FormCols;
                        sql = "delete from front_form where ComName='" + str.D2DD(obj.ComName) + "'";
                    }
                    if(sql!="") globalConst.CurSite.SiteConn.execSql(sql);
                    EdiItem = null;
                    tree.SelectedNode.Remove();
                }
            }
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MsgBox.YesNoCancel("Delete all items ?") == DialogResult.Yes)
            {
                EdiItem = null;
                tree.Nodes.Clear();
            }
            tabControl1.TabPages.Clear();
        }
        #endregion
        #region DataGridView1
        int selectionIdx = -1;
        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            dataGridView1.EndEdit();
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Clicks < 2) && (e.Button == MouseButtons.Left))

            {
                if ((e.ColumnIndex == -1) && (e.RowIndex > -1))

                    dataGridView1.DoDragDrop(dataGridView1.Rows[e.RowIndex], DragDropEffects.Move);

            }
        }
        private int GetRowFromPoint(int x, int y)

        {
            for (int i = 0; i < dataGridView1.RowCount; i++)

            {
                Rectangle rec = dataGridView1.GetRowDisplayRectangle(i, false);

                if (dataGridView1.RectangleToScreen(rec).Contains(x, y))

                    return i;

            }
            return -1;

        }
        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            int idx = GetRowFromPoint(e.X, e.Y);

            if (idx < 0) return;

            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))

            {
                DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));

                dataGridView1.Rows.Remove(row);

                selectionIdx = idx;

                dataGridView1.Rows.Insert(idx, row);
                ChangedForPreviewBrowser();
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (selectionIdx > -1)

            {
                dataGridView1.Rows[selectionIdx].Selected = true;

                dataGridView1.CurrentCell = dataGridView1.Rows[selectionIdx].Cells[0];
                selectionIdx = -1;

            }
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[7].Value = "Cut";
            e.Row.Cells[8].Value = "Insert";
        }
        DataGridViewRow cutSaveRow1 = null;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4 || e.ColumnIndex == 5)
            {
                ChangedForPreviewBrowser();
            }
            else if (e.ColumnIndex == 7)//Cut
            {
                cutSaveRow1 = dataGridView1.Rows[e.RowIndex];
                dataGridView1.Rows.Remove(cutSaveRow1);
                ChangedForPreviewBrowser();
            }
            else if (e.ColumnIndex == 8)//Insert
            {
                if (cutSaveRow1 != null)
                {
                    dataGridView1.Rows.Insert(e.RowIndex, cutSaveRow1); cutSaveRow1 = null;
                    ChangedForPreviewBrowser();
                }
            }
            else if (e.ColumnIndex == 6)//Template
            {
                HTMLText text = new HTMLText();
                text.SnippetDefine = (SnippetTag: "[Front]", ComboShowText: "[Select Snippet]");
                text.SetVal = (dataGridView1.Rows[e.RowIndex].Cells[6].Value ?? "").ToString();
                text.LabelShow.Text = res.ftform.str("z010") + "$(Bind),$(Bind.L),$(Label),$(PlaceHolder),@para{}";
                text.TopMost = true;
                text.ShowDialog();
                if (text.IsOK)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[6].Value = text.SetVal;
                    dataGridView1.EndEdit();
                    ChangedForPreviewBrowser();
                }
            }
        }


        #endregion
        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                string apiurl = List_ApiUrl.Text.Trim();
                if (apiurl == "")
                {
                    MsgBox.Warning(res.ftform.str("z011"));
                    return;
                }
                var suitUrl = apiurl;
                if (suitUrl.IndexOf('?') > 0 && suitUrl.IndexOf('/', suitUrl.IndexOf('?')) > 0)
                {
                    suitUrl = suitUrl.Substring(0, suitUrl.IndexOf('/', suitUrl.IndexOf('?')));
                }
                string keyDesc = "";
                string sql = "select KeyDesc from ft_ftdp_apidoc where ApiPath='" + str.Dot2DotDot(suitUrl) + "'";
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
                string[] item0 = keyDesc.Split(new string[] { "{;;}" }, StringSplitOptions.RemoveEmptyEntries);
                List<string> keysApi = new List<string>();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                foreach (string item1 in item0)
                {
                    string[] item2 = item1.Split(new string[] { "{::}" }, StringSplitOptions.None);
                    if (!keysApi.Contains(item2[0].ToLower())) keysApi.Add(item2[0].ToLower());
                    bool haveAdded = false;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[1].Value != null && row.Cells[1].Value.ToString().ToLower() == item2[0].ToLower())
                        {
                            haveAdded = true;
                            break;
                        }
                    }
                    if (!haveAdded)
                    {
                        var rI = dataGridView1.Rows.Add(new object[] { item2[1], item2[0], "", "", false, false, "", "Cut", "Insert" });
                        dataGridView1.Rows[rI].DefaultCellStyle.ForeColor = Color.Blue;
                    }
                }
                //检测存在不在接口的配置
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // row.DefaultCellStyle.ForeColor = Color.Black;
                    if (row.Cells[1].Value != null && !keysApi.Contains(row.Cells[1].Value.ToString().ToLower()))
                    {
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }

                ChangedForPreviewBrowser();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        int TabListIndex = 0;
        int TabFormIndex = 0;

        private void button16_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();
        }
        private bool PreviewBrowserTaskRuning = false;
        private void ChangedForPreviewBrowser()
        {
            if (ForeBrowser.FB == null) return;
            if (PreviewBrowserTaskRuning) return;
            if (tree.SelectedNode != null && (tree.SelectedNode.Tag as object[])[0].ToString() != "dir")
            {
                ItemSave();
                new Task(() =>
                {
                    PreviewBrowserTaskRuning = true;
                    //Adv.paraDic = null;//每次操作实时取ParaDic
                    //ItemSave();//线程安全移到外层
                    try
                    {
                        var item = tree.SelectedNode;
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
                            MsgBox.Error(res.ftform.str("z012"));
                            return;
                        }
                        string preview_filename = ComNameNoPath(ComName)+ "_preview.html";
                        preview_filename = globalConst.CurSite.Path + @"\__front\preview\" + preview_filename;
                        using (StreamWriter sw = new StreamWriter(preview_filename, false, Encoding.UTF8))
                        {
                            sw.Write(g.Item3);
                            sw.Flush();
                        }
                        PreviewBrowserLoad(preview_filename);
                    }
                    catch (Exception ex)
                    {
                        //new error(ex);
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
        #region DataGridView 2
        int selectionIdx2 = -1;
        private void dataGridView2_DragEnter(object sender, DragEventArgs e)
        {
            dgv2.EndEdit();
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView2_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Clicks < 2) && (e.Button == MouseButtons.Left))

            {
                if ((e.ColumnIndex == -1) && (e.RowIndex > -1))

                    dgv2.DoDragDrop(dgv2.Rows[e.RowIndex], DragDropEffects.Move);

            }
        }
        private int GetRowFromPoint2(int x, int y)

        {
            for (int i = 0; i < dgv2.RowCount; i++)

            {
                Rectangle rec = dgv2.GetRowDisplayRectangle(i, false);

                if (dgv2.RectangleToScreen(rec).Contains(x, y))

                    return i;

            }
            return -1;

        }
        private void dataGridView2_DragDrop(object sender, DragEventArgs e)
        {
            int idx = GetRowFromPoint2(e.X, e.Y);

            if (idx < 0) return;

            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))

            {
                DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));

                dgv2.Rows.Remove(row);

                selectionIdx2 = idx;

                dgv2.Rows.Insert(idx, row);
                ChangedForPreviewBrowser();

            }
        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (selectionIdx2 > -1)

            {
                dgv2.Rows[selectionIdx2].Selected = true;

                dgv2.CurrentCell = dgv2.Rows[selectionIdx2].Cells[0];
                selectionIdx2 = -1;

            }
        }

        private void dataGridView2_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[0].Value = "input";
            e.Row.Cells[1].Value = "sdata.";
            e.Row.Cells[3].Value = "width:200px;margin-right:10px";
            e.Row.Cells[5].Value = "Cut";
            e.Row.Cells[6].Value = "Insert";
        }
        DataGridViewRow cutSaveRow2 = null;

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)//Cut
            {
                cutSaveRow2 = dgv2.Rows[e.RowIndex];
                dgv2.Rows.Remove(cutSaveRow2);
                ChangedForPreviewBrowser();
            }
            else if (e.ColumnIndex == 6)//Insert
            {
                if (cutSaveRow2 != null)
                { dgv2.Rows.Insert(e.RowIndex, cutSaveRow2); cutSaveRow2 = null; ChangedForPreviewBrowser(); }
            }
            else if (e.ColumnIndex == 4)//InitData/HTML
            {
                var type = (dgv2.Rows[e.RowIndex].Cells[0].Value ?? "").ToString();
                if (type == "html")
                {
                    HTMLText hTMLText = new HTMLText();
                    hTMLText.SnippetDefine = (SnippetTag: "[Front]", ComboShowText: "[Select Snippet]");
                    hTMLText.SetVal = (dgv2.Rows[e.RowIndex].Cells[4].Value ?? "").ToString();
                    hTMLText.LabelShow.Text = res.ftform.str("z010") + "$(Bind),$(Bind.L),$(Label),$(PlaceHolder),@para{}";
                    hTMLText.TopMost = true;
                    hTMLText.ShowDialog();
                    if (hTMLText.IsOK)
                    {
                        dgv2.Rows[e.RowIndex].Cells[4].Value = hTMLText.SetVal;
                        dgv2.EndEdit();
                        ChangedForPreviewBrowser();
                    }
                }
                else
                {
                    ForeSData foreSData = new ForeSData();
                    foreSData.SetVal = (dgv2.Rows[e.RowIndex].Cells[4].Value ?? "").ToString();
                    foreSData.TopMost = true;
                    foreSData.ShowDialog();
                    if (foreSData.IsOK)
                    {
                        dgv2.Rows[e.RowIndex].Cells[4].Value = foreSData.SetVal;
                        dgv2.EndEdit();
                        ChangedForPreviewBrowser();
                    }
                }
            }
        }


        #endregion

        #region DataGridView 3
        int selectionIdx3 = -1;
        private void dataGridView3_DragEnter(object sender, DragEventArgs e)
        {
            dgv3.EndEdit();
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView3_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Clicks < 2) && (e.Button == MouseButtons.Left))

            {
                if ((e.ColumnIndex == -1) && (e.RowIndex > -1))

                    dgv3.DoDragDrop(dgv3.Rows[e.RowIndex], DragDropEffects.Move);

            }
        }
        private int GetRowFromPoint3(int x, int y)

        {
            for (int i = 0; i < dgv3.RowCount; i++)

            {
                Rectangle rec = dgv3.GetRowDisplayRectangle(i, false);

                if (dgv3.RectangleToScreen(rec).Contains(x, y))

                    return i;

            }
            return -1;

        }
        private void dataGridView3_DragDrop(object sender, DragEventArgs e)
        {
            int idx = GetRowFromPoint3(e.X, e.Y);

            if (idx < 0) return;

            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))

            {
                DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));

                dgv3.Rows.Remove(row);

                selectionIdx3 = idx;

                dgv3.Rows.Insert(idx, row);
                ChangedForPreviewBrowser();
            }
        }

        private void dataGridView3_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (selectionIdx3 > -1)

            {
                dgv3.Rows[selectionIdx3].Selected = true;

                dgv3.CurrentCell = dgv3.Rows[selectionIdx3].Cells[0];
                selectionIdx3 = -1;

            }
        }

        private void dataGridView3_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[10].Value = "Cut";
            e.Row.Cells[11].Value = "Insert";
        }
        DataGridViewRow cutSaveRow3 = null;

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((new int[] { 3, 4, 6, 7, 8 }).Contains(e.ColumnIndex))
            {
                ChangedForPreviewBrowser();
            }
            else if (e.ColumnIndex == 10)//Cut
            {
                cutSaveRow3 = dgv3.Rows[e.RowIndex];
                dgv3.Rows.Remove(cutSaveRow3);
                ChangedForPreviewBrowser();
            }
            else if (e.ColumnIndex == 11)//Insert
            {
                if (cutSaveRow3 != null)
                { dgv3.Rows.Insert(e.RowIndex, cutSaveRow3); cutSaveRow3 = null; ChangedForPreviewBrowser(); }
            }
            else if (e.ColumnIndex == 9)//Event
            {
                List<(string type, string data)> schDefineData = new List<(string type, string data)>();
                foreach (DataGridViewRow row in dgv2.Rows)
                {
                    string type = row.Cells[0].Value?.ToString() ?? "";
                    string data = row.Cells[1].Value?.ToString() ?? "";
                    if (data != "")
                    {
                        schDefineData.Add((type, data));
                    }
                }
                ForeButtonEvent foreButtonEvent = new ForeButtonEvent();
                foreButtonEvent.Click = (dgv3.Rows[e.RowIndex].Cells[9].Value ?? "").ToString();
                foreButtonEvent.JsCode = dgv3.Rows[e.RowIndex].Tag == null ? "" : (string)(((object[])dgv3.Rows[e.RowIndex].Tag)[0]);
                foreButtonEvent.FromIndex = 1;
                foreButtonEvent.schDefineData = schDefineData;
                foreButtonEvent.TopMost = true;
                //foreButtonEvent.TopLevelControl = this;
                foreButtonEvent.Row = dgv3.Rows[e.RowIndex];
                foreButtonEvent.Show();
                //if (foreButtonEvent.IsOK)
                //{
                //    dgv3.Rows[e.RowIndex].Cells[9].Value = foreButtonEvent.Click;
                //    ((object[])dgv3.Rows[e.RowIndex].Tag)[0] = foreButtonEvent.JsCode;
                //}
            }
        }


        #endregion

        #region DataGridView 4
        int selectionIdx4 = -1;
        private void dataGridView4_DragEnter(object sender, DragEventArgs e)
        {
            dgv4.EndEdit();
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView4_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Clicks < 2) && (e.Button == MouseButtons.Left))

            {
                if ((e.ColumnIndex == -1) && (e.RowIndex > -1))

                    dgv4.DoDragDrop(dgv4.Rows[e.RowIndex], DragDropEffects.Move);

            }
        }
        private int GetRowFromPoint4(int x, int y)

        {
            for (int i = 0; i < dgv4.RowCount; i++)

            {
                Rectangle rec = dgv4.GetRowDisplayRectangle(i, false);

                if (dgv4.RectangleToScreen(rec).Contains(x, y))

                    return i;

            }
            return -1;

        }
        private void dataGridView4_DragDrop(object sender, DragEventArgs e)
        {
            int idx = GetRowFromPoint4(e.X, e.Y);

            if (idx < 0) return;

            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))

            {
                DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));

                dgv4.Rows.Remove(row);

                selectionIdx4 = idx;

                dgv4.Rows.Insert(idx, row);
                ChangedForPreviewBrowser();
            }
        }

        private void dataGridView4_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (selectionIdx4 > -1)

            {
                dgv4.Rows[selectionIdx4].Selected = true;

                dgv4.CurrentCell = dgv4.Rows[selectionIdx4].Cells[0];
                selectionIdx4 = -1;

            }
        }

        private void dataGridView4_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[1].Value = "form.";
            e.Row.Cells[2].Value = "input";
            e.Row.Cells[10].Value = "Cut";
            e.Row.Cells[11].Value = "Insert";
        }
        DataGridViewRow cutSaveRow4 = null;

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                ChangedForPreviewBrowser();
            }
            else if (e.ColumnIndex == 10)//Cut
            {
                cutSaveRow4 = dgv4.Rows[e.RowIndex];
                dgv4.Rows.Remove(cutSaveRow4);
                ChangedForPreviewBrowser();
            }
            else if (e.ColumnIndex == 11)//Insert
            {
                if (cutSaveRow4 != null)
                { dgv4.Rows.Insert(e.RowIndex, cutSaveRow4); cutSaveRow4 = null; ChangedForPreviewBrowser(); }
            }
            else if (e.ColumnIndex == 6)//InitData
            {
                ForeSData foreSData = new ForeSData();
                foreSData.SetVal = (dgv4.Rows[e.RowIndex].Cells[6].Value ?? "").ToString();
                foreSData.TopMost = true;
                foreSData.ShowDialog();
                if (foreSData.IsOK)
                {
                    dgv4.Rows[e.RowIndex].Cells[6].Value = foreSData.SetVal;
                    dgv4.EndEdit();
                    ChangedForPreviewBrowser();
                }
            }
            else if (e.ColumnIndex == 7)//Template
            {
                HTMLText text = new HTMLText();
                text.SnippetDefine = (SnippetTag: "[Front]", ComboShowText: "[Select Snippet]");
                text.SetVal = (dgv4.Rows[e.RowIndex].Cells[7].Value ?? "").ToString();
                text.LabelShow.Text = res.ftform.str("z013");
                text.TopMost = true;
                text.ShowDialog();
                if (text.IsOK)
                {
                    dgv4.Rows[e.RowIndex].Cells[7].Value = text.SetVal;
                    dgv4.EndEdit();
                    ChangedForPreviewBrowser();
                }
            }
            else if (e.ColumnIndex == 8)//Validation
            {
                ForeFormCheck foreFormCheck = new ForeFormCheck();
                foreFormCheck.ValidType = (dgv4.Rows[e.RowIndex].Cells[8].Value ?? "").ToString();
                foreFormCheck.ValidCustomJs = dgv4.Rows[e.RowIndex].Tag == null ? "" : (((object[])(dgv4.Rows[e.RowIndex].Tag))[0].ToString());
                foreFormCheck.TopMost = true;
                foreFormCheck.ShowDialog();
                if (!foreFormCheck.IsCancel)
                {
                    dgv4.Rows[e.RowIndex].Cells[8].Value = foreFormCheck.ValidCustomJs.StartsWith("#")? foreFormCheck.ValidCustomJs:foreFormCheck.ValidType;
                    if (dgv4.Rows[e.RowIndex].Tag == null) dgv4.Rows[e.RowIndex].Tag = new object[] { foreFormCheck.ValidCustomJs };
                    else ((object[])(dgv4.Rows[e.RowIndex].Tag))[0] = foreFormCheck.ValidCustomJs;
                    dgv4.EndEdit();
                    ChangedForPreviewBrowser();
                }
            }
        }


        #endregion

        #region DataGridView 5
        int selectionIdx5 = -1;
        private void dataGridView5_DragEnter(object sender, DragEventArgs e)
        {
            dgv5.EndEdit();
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView5_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Clicks < 2) && (e.Button == MouseButtons.Left))

            {
                if ((e.ColumnIndex == -1) && (e.RowIndex > -1))

                    dgv5.DoDragDrop(dgv5.Rows[e.RowIndex], DragDropEffects.Move);

            }
        }
        private int GetRowFromPoint5(int x, int y)

        {
            for (int i = 0; i < dgv5.RowCount; i++)

            {
                Rectangle rec = dgv5.GetRowDisplayRectangle(i, false);

                if (dgv5.RectangleToScreen(rec).Contains(x, y))

                    return i;

            }
            return -1;

        }
        private void dataGridView5_DragDrop(object sender, DragEventArgs e)
        {
            int idx = GetRowFromPoint5(e.X, e.Y);

            if (idx < 0) return;

            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))

            {
                DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));

                dgv5.Rows.Remove(row);

                selectionIdx5 = idx;

                dgv5.Rows.Insert(idx, row);
                ChangedForPreviewBrowser();
            }
        }

        private void dataGridView5_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (selectionIdx5 > -1)

            {
                dgv5.Rows[selectionIdx5].Selected = true;

                dgv5.CurrentCell = dgv5.Rows[selectionIdx5].Cells[0];
                selectionIdx5 = -1;

            }
        }

        private void dataGridView5_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[10].Value = "Cut";
            e.Row.Cells[11].Value = "Insert";
        }
        DataGridViewRow cutSaveRow5 = null;

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((new int[] { 3, 4, 6, 7, 8 }).Contains(e.ColumnIndex))
            {
                ChangedForPreviewBrowser();
            }
            else if (e.ColumnIndex == 10)//Cut
            {
                cutSaveRow5 = dgv5.Rows[e.RowIndex];
                dgv5.Rows.Remove(cutSaveRow5);
                ChangedForPreviewBrowser();
            }
            else if (e.ColumnIndex == 11)//Insert
            {
                if (cutSaveRow5 != null)
                { dgv5.Rows.Insert(e.RowIndex, cutSaveRow5); cutSaveRow5 = null; ChangedForPreviewBrowser(); }
            }
            else if (e.ColumnIndex == 9)//Event
            {
                ForeButtonEvent foreButtonEvent = new ForeButtonEvent();
                foreButtonEvent.Click = (dgv5.Rows[e.RowIndex].Cells[9].Value ?? "").ToString();
                foreButtonEvent.JsCode = dgv5.Rows[e.RowIndex].Tag == null ? "" : (string)(((object[])dgv5.Rows[e.RowIndex].Tag)[0]);
                foreButtonEvent.FromIndex = 2;
                foreButtonEvent.TopMost = true;
                //foreButtonEvent.TopLevelControl = this;
                foreButtonEvent.Row = dgv5.Rows[e.RowIndex];
                foreButtonEvent.Show();
                //if (foreButtonEvent.IsOK)
                //{
                //    dgv3.Rows[e.RowIndex].Cells[9].Value = foreButtonEvent.Click;
                //    ((object[])dgv3.Rows[e.RowIndex].Tag)[0] = foreButtonEvent.JsCode;
                //}
            }
        }


        #endregion

        private void yulanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode != null && (tree.SelectedNode.Tag as object[])[0].ToString() != "dir")
            {
                //Adv.paraDic = null;//每次操作实时取ParaDic
                ItemSave();
                var item = tree.SelectedNode;
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
                    MsgBox.Error(res.ftform.str("z012"));
                    return;
                }
                string com_filename = ComNameNoPath(ComName) + ".vue";
                string html_filename = ComNameNoPath(ComName) + "_test.html";
                string preview_filename = ComNameNoPath(ComName) + "_preview.html";
                com_filename = globalConst.CurSite.Path + @"\__front\com\" + com_filename;
                html_filename = globalConst.CurSite.Path + @"\__front\" + html_filename;
                preview_filename = globalConst.CurSite.Path + @"\__front\preview\" + preview_filename;
                using (StreamWriter sw = new StreamWriter(com_filename, false, Encoding.UTF8))
                {
                    sw.Write(g.Item1);
                    sw.Flush();
                }
                using (StreamWriter sw = new StreamWriter(html_filename, false, Encoding.UTF8))
                {
                    sw.Write(g.Item2);
                    sw.Flush();
                }
                using (StreamWriter sw = new StreamWriter(preview_filename, false, Encoding.UTF8))
                {
                    sw.Write(g.Item3);
                    sw.Flush();
                }
                sheel.ExeSheel(preview_filename);
            }
        }

        private void daochuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode != null && (tree.SelectedNode.Tag as object[])[0].ToString() != "dir")
            {
                //Adv.paraDic = null;//每次操作实时取ParaDic
                ItemSave();
                var item = tree.SelectedNode;
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
                    MsgBox.Error(res.ftform.str("z012"));
                    return;
                }
                string com_filename = ComNameNoPath(ComName) + ".vue";
                string html_filename = ComNameNoPath(ComName) + "_test.html";
                string preview_filename = ComNameNoPath(ComName) + "_preview.html";
                var names = ComName.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Vue Component(*.vue)|*.vue";
                sfd.Title = "Vue Component";
                sfd.FileName = names[names.Length-1] + ".vue";
                sfd.ShowDialog();
                if (!string.IsNullOrWhiteSpace(sfd.FileName) && sfd.FileName != (names[names.Length - 1] + ".vue"))
                {
                    var writeText = g.Item4;
                    if (file.Exists(sfd.FileName))
                    {
                        var fileText = File.ReadAllText(sfd.FileName);
                        var comParts = Generator.ComTextSplit(fileText);
                        if(comParts.templateText!="" || comParts.scriptText!="" || comParts.styleText!="")
                        {
                            ForePartWrite forePartWrite = new ForePartWrite();
                            forePartWrite.ShowDialog();
                            if(!forePartWrite.IsCancel)
                            {
                                if(!forePartWrite.IsWriteTemplate || !forePartWrite.IsWriteScript || !forePartWrite.IsWriteStyle)
                                {
                                    var comPartsNew = Generator.ComTextSplit(writeText);
                                    writeText = "";
                                    if (!forePartWrite.IsWriteScript) writeText += comParts.docText;
                                    else writeText += comPartsNew.docText;
                                    if (!forePartWrite.IsWriteTemplate) writeText += comParts.templateText;
                                    else writeText += comPartsNew.templateText;
                                    if (!forePartWrite.IsWriteScript) writeText += comParts.scriptText;
                                    else writeText += comPartsNew.scriptText;
                                    if (!forePartWrite.IsWriteStyle) writeText += comParts.styleText;
                                    else writeText += comPartsNew.styleText;
                                }
                            }
                            else
                            {
                                writeText = null;
                            }
                        }
                    }
                    if (writeText != null)
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                        {
                            sw.Write(writeText);
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
                //sfd = new SaveFileDialog();
                //sfd.Filter = "HTML文件(*.html)|*.html";
                //sfd.Title = "Vue组件的HTML示例文件";
                //sfd.FileName = ComName + "_test.html";
                //sfd.ShowDialog();
                //if (!string.IsNullOrWhiteSpace(sfd.FileName) && sfd.FileName != (ComName + "_test.html"))
                //{
                //    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                //    {
                //        sw.Write(g.Item2);
                //        sw.Flush();
                //    }
                //}
            }
        }

        private void fabuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode != null && (tree.SelectedNode.Tag as object[])[0].ToString() != "dir")
            {
                //Adv.paraDic = null;//每次操作实时取ParaDic
                //ItemSave();
                if (!SaveActive()) return;
                var item = tree.SelectedNode;
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
                    MsgBox.Error(res.ftform.str("z014"));
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
                    paras.Add((0, "comHtml", g.Item3));
                    paras.Add((0, "comText2", g.Item4));
                    string reStr = net.HttpPostForm(url, paras).Trim();
                    if (reStr.StartsWith("error:")) MsgBox.Error(reStr);
                    else
                    {
                        MsgBox.Information(res.ftform.str("z015"));
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
            if (type == "dir") return;
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
        private void button17_Click(object sender, EventArgs e)
        {
            dgv4.Rows.Clear();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            formRowFromApi(Form_ApiGet.Text.Trim());
        }

        private void button19_Click(object sender, EventArgs e)
        {
            formRowFromApi(Form_ApiSet.Text.Trim());
        }
        
        void formRowFromApi(string apiurl)
        {
            try
            {
                if (apiurl == "")
                {
                    MsgBox.Warning(res.ftform.str("z011"));
                    return;
                }
                string keyDesc = FrontFunc.KeyDescGet(apiurl);
                string[] item0 = keyDesc.Split(new string[] { "{;;}" }, StringSplitOptions.RemoveEmptyEntries);
                List<string> keysApi = new List<string>();
                foreach (DataGridViewRow row in dgv4.Rows)
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                foreach (string item1 in item0)
                {
                    string[] item2 = item1.Split(new string[] { "{::}" }, StringSplitOptions.None);
                    if (!keysApi.Contains(item2[0].ToLower())) keysApi.Add(item2[0].ToLower());
                    bool haveAdded = false;
                    foreach (DataGridViewRow row in dgv4.Rows)
                    {
                        if (row.Cells[1].Value != null && row.Cells[1].Value.ToString().ToLower().EndsWith("." + item2[0].ToLower()))
                        {
                            haveAdded = true;
                            break;
                        }
                    }
                    if (!haveAdded)
                    {
                        var rI = dgv4.Rows.Add(new object[] { item2[1], "form." + item2[0], formAutoTypeByName(item2[0]), "", "", false, "", "", "", "", "Cut", "Insert" });
                        dgv4.Rows[rI].DefaultCellStyle.ForeColor = Color.Blue;
                    }
                }
                //检测存在不在接口的配置
                foreach (DataGridViewRow row in dgv4.Rows)
                {
                    string _key = "";
                    if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() != "")
                    {
                        string[] ss = row.Cells[1].Value.ToString().ToLower().Trim().Split('.');
                        _key = ss[ss.Length - 1];
                    }
                    if (_key == "")
                    {
                        row.DefaultCellStyle.ForeColor = Color.Green;
                    }
                    else if (!keysApi.Contains(_key))
                    {
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
                ChangedForPreviewBrowser();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        string formAutoTypeByName(string name)
        {
            name = name.ToLower();
            switch(name)
            {
                case string _ when name.EndsWith("type"):
                case string _ when name.EndsWith("status"):
                case string _ when name.EndsWith("stat"):
                case string _ when name.EndsWith("code"):
                    return "select";
                case string _ when name.EndsWith("date"):
                case string _ when name.EndsWith("day"):
                case string _ when name.EndsWith("time"):
                    return "date";
                case string _ when name.EndsWith("desc"):
                case string _ when name.EndsWith("description"):
                    return "textarea";
                default:return "input";
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            try
            {
                string apiurl = Form_ApiGet.Text.Trim();
                if (apiurl == "")
                {
                    MsgBox.Warning(res.ftform.str("z011"));
                    return;
                }
                var bindGenerate = FrontFunc.BindGetGenerate(apiurl, dgv4,null);
                //Form_BindGet.ResetText();
                //Form_BindGet.Refresh();
                //Form_BindGet.Text = sb.ToString();
                Form_BindGet.Append((Form_BindGet.Text.EndsWith("\r\n\r\n") ? "" : "\r\n\r\n") + bindGenerate);
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                string apiurl = Form_ApiSet.Text.Trim();
                if (apiurl == "")
                {
                    MsgBox.Warning(res.ftform.str("z011"));
                    return;
                }
                var bindGenerate = FrontFunc.BindSetGenerate(apiurl, dgv4, null);
                //Form_BindSet.ResetText();
                //Form_BindSet.Refresh();
                //Form_BindSet.Text = sb.ToString();
                Form_BindSet.Append((Form_BindSet.Text.EndsWith("\r\n\r\n") ? "" : "\r\n\r\n") + bindGenerate);
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void backUpListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode != null && (tree.SelectedNode.Tag as object[])[0].ToString() != "dir")
            {
                var item = tree.SelectedNode;
                string type = ((object[])item.Tag)[0].ToString();
                object obj = ((object[])EdiItem.Tag)[1];
                ForeBackUp foreBackUp = new ForeBackUp();
                foreBackUp.type = type;
                foreBackUp.obj = obj;
                foreBackUp.TopMost = true;
                foreBackUp.ShowDialog();
                if (foreBackUp.IsUpdateFromBackUp)
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
                    string sql = "select * from front_" + type + " where ComName='" + comName + "'";
                    using (DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql)))
                    {
                        if (dr.Read())
                        {
                            if (type == "list")
                            {
                                var colObj = GetListColsObj(dr);
                                item.Text = ComNameJustName(colObj.ComName) + " - " + colObj.Caption;
                                item.ToolTipText = ComNameJustName(colObj.ComName) + " - " + colObj.Caption;
                                item.Tag = new object[] { "list", colObj };
                            }
                            else if (type == "form")
                            {
                                var colObj = GetFormColsObj(dr);
                                item.Text = ComNameJustName(colObj.ComName) + " - " + colObj.Caption;
                                item.ToolTipText = ComNameJustName(colObj.ComName) + " - " + colObj.Caption;
                                item.Tag = new object[] { "form", colObj };
                            }
                            ItemInit();
                            tree.SelectedNode = item;
                        }
                    }

                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            else dataGridView1.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) dgv4.EditMode = DataGridViewEditMode.EditOnEnter;
            else dgv4.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
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
            url += "_ft/_base/frontlist";
            sheel.ExeSheel(url);
        }

        private void updateAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MsgBox.OKCancel(res.ftform.str("z016")) != DialogResult.OK) return;
            string connstr = Options.GetSystemDBSetConnStr_Plat();
            var conntype = Options.GetSystemDBSetType_Plat();
            List<object[]> ListArrays = new List<object[]>();
            List<object[]> FormArrays = new List<object[]>();
            string sql = "select * from ft_ftdp_front_list where IsNewest=1";
            if (conntype ==globalConst.DBType.SqlServer)
            {
                using (SqlConnection db = new SqlConnection(connstr))
                {
                    db.Open();
                    using (SqlDataReader dr = new SqlCommand(sql, db).ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ListArrays.Add(new object[] {
                                dr.GetString(dr.GetOrdinal("ComName")),
                                dr.GetString(dr.GetOrdinal("Caption")),
                                dr.GetString(dr.GetOrdinal("ApiBase")),
                                dr.GetString(dr.GetOrdinal("ApiUrl")),
                                dr.GetString(dr.GetOrdinal("ColRows")),
                                dr.GetString(dr.GetOrdinal("Search")),
                                dr.GetString(dr.GetOrdinal("Buttons")),
                                dr.GetString(dr.GetOrdinal("Pager")),
                                dr.GetString(dr.GetOrdinal("InitSet")),
                                dr.GetString(dr.GetOrdinal("JsBeforeLoad")),
                                dr.GetString(dr.GetOrdinal("JsBeforeSet")),
                                dr.GetString(dr.GetOrdinal("JsAfterSet")),
                                dr.GetString(dr.GetOrdinal("CustomJs")),
                                dr.GetString(dr.GetOrdinal("CssText")),
                                dr.GetString(dr.GetOrdinal("OtherSet")),
                            });
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
                        while (dr.Read())
                        {
                            ListArrays.Add(new object[] {
                                dr.GetString(dr.GetOrdinal("ComName")),
                                dr.GetString(dr.GetOrdinal("Caption")),
                                dr.GetString(dr.GetOrdinal("ApiBase")),
                                dr.GetString(dr.GetOrdinal("ApiUrl")),
                                dr.GetString(dr.GetOrdinal("ColRows")),
                                dr.GetString(dr.GetOrdinal("Search")),
                                dr.GetString(dr.GetOrdinal("Buttons")),
                                dr.GetString(dr.GetOrdinal("Pager")),
                                dr.GetString(dr.GetOrdinal("InitSet")),
                                dr.GetString(dr.GetOrdinal("JsBeforeLoad")),
                                dr.GetString(dr.GetOrdinal("JsBeforeSet")),
                                dr.GetString(dr.GetOrdinal("JsAfterSet")),
                                dr.GetString(dr.GetOrdinal("CustomJs")),
                                dr.GetString(dr.GetOrdinal("CssText")),
                                dr.GetString(dr.GetOrdinal("OtherSet")),
                            });
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
                        while (dr.Read())
                        {
                            ListArrays.Add(new object[] {
                                dr.GetString(dr.GetOrdinal("ComName")),
                                dr.GetString(dr.GetOrdinal("Caption")),
                                dr.GetString(dr.GetOrdinal("ApiBase")),
                                dr.GetString(dr.GetOrdinal("ApiUrl")),
                                dr.GetString(dr.GetOrdinal("ColRows")),
                                dr.GetString(dr.GetOrdinal("Search")),
                                dr.GetString(dr.GetOrdinal("Buttons")),
                                dr.GetString(dr.GetOrdinal("Pager")),
                                dr.GetString(dr.GetOrdinal("InitSet")),
                                dr.GetString(dr.GetOrdinal("JsBeforeLoad")),
                                dr.GetString(dr.GetOrdinal("JsBeforeSet")),
                                dr.GetString(dr.GetOrdinal("JsAfterSet")),
                                dr.GetString(dr.GetOrdinal("CustomJs")),
                                dr.GetString(dr.GetOrdinal("CssText")),
                                dr.GetString(dr.GetOrdinal("OtherSet")),
                            });
                        }
                    }
                }
            }
            sql = "select * from ft_ftdp_front_form where IsNewest=1";
            if (conntype==globalConst.DBType.SqlServer)
            {
                using (SqlConnection db = new SqlConnection(connstr))
                {
                    db.Open();
                    using (SqlDataReader dr = new SqlCommand(sql, db).ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            FormArrays.Add(new object[] {
                                dr.GetString(dr.GetOrdinal("ComName")),
                                dr.GetString(dr.GetOrdinal("Caption")),
                                dr.GetString(dr.GetOrdinal("ApiBase")),
                                dr.GetString(dr.GetOrdinal("ApiGet")),
                                dr.GetString(dr.GetOrdinal("ApiSet")),
                                dr.GetString(dr.GetOrdinal("ColRows")),
                                dr.GetString(dr.GetOrdinal("Buttons")),
                                dr.GetString(dr.GetOrdinal("BindGet")),
                                dr.GetString(dr.GetOrdinal("BindSet")),
                                dr.GetString(dr.GetOrdinal("JsBeforeSubmit")),
                                dr.GetString(dr.GetOrdinal("JsAfterSubmit")),
                                dr.GetString(dr.GetOrdinal("JsBeforeGet")),
                                dr.GetString(dr.GetOrdinal("JsBeforeSet")),
                                dr.GetString(dr.GetOrdinal("JsAfterSet")),
                                dr.GetString(dr.GetOrdinal("CustomJs")),
                                dr.GetString(dr.GetOrdinal("CssText")),
                                dr.GetString(dr.GetOrdinal("CusDataDefine")),
                                dr.GetString(dr.GetOrdinal("OtherSet")),
                            });
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
                        while (dr.Read())
                        {
                            FormArrays.Add(new object[] {
                                dr.GetString(dr.GetOrdinal("ComName")),
                                dr.GetString(dr.GetOrdinal("Caption")),
                                dr.GetString(dr.GetOrdinal("ApiBase")),
                                dr.GetString(dr.GetOrdinal("ApiGet")),
                                dr.GetString(dr.GetOrdinal("ApiSet")),
                                dr.GetString(dr.GetOrdinal("ColRows")),
                                dr.GetString(dr.GetOrdinal("Buttons")),
                                dr.GetString(dr.GetOrdinal("BindGet")),
                                dr.GetString(dr.GetOrdinal("BindSet")),
                                dr.GetString(dr.GetOrdinal("JsBeforeSubmit")),
                                dr.GetString(dr.GetOrdinal("JsAfterSubmit")),
                                dr.GetString(dr.GetOrdinal("JsBeforeGet")),
                                dr.GetString(dr.GetOrdinal("JsBeforeSet")),
                                dr.GetString(dr.GetOrdinal("JsAfterSet")),
                                dr.GetString(dr.GetOrdinal("CustomJs")),
                                dr.GetString(dr.GetOrdinal("CssText")),
                                dr.GetString(dr.GetOrdinal("CusDataDefine")),
                                dr.GetString(dr.GetOrdinal("OtherSet")),
                            });
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
                        while (dr.Read())
                        {
                            FormArrays.Add(new object[] {
                                dr.GetString(dr.GetOrdinal("ComName")),
                                dr.GetString(dr.GetOrdinal("Caption")),
                                dr.GetString(dr.GetOrdinal("ApiBase")),
                                dr.GetString(dr.GetOrdinal("ApiGet")),
                                dr.GetString(dr.GetOrdinal("ApiSet")),
                                dr.GetString(dr.GetOrdinal("ColRows")),
                                dr.GetString(dr.GetOrdinal("Buttons")),
                                dr.GetString(dr.GetOrdinal("BindGet")),
                                dr.GetString(dr.GetOrdinal("BindSet")),
                                dr.GetString(dr.GetOrdinal("JsBeforeSubmit")),
                                dr.GetString(dr.GetOrdinal("JsAfterSubmit")),
                                dr.GetString(dr.GetOrdinal("JsBeforeGet")),
                                dr.GetString(dr.GetOrdinal("JsBeforeSet")),
                                dr.GetString(dr.GetOrdinal("JsAfterSet")),
                                dr.GetString(dr.GetOrdinal("CustomJs")),
                                dr.GetString(dr.GetOrdinal("CssText")),
                                dr.GetString(dr.GetOrdinal("CusDataDefine")),
                                dr.GetString(dr.GetOrdinal("OtherSet")),
                            });
                        }
                    }
                }
            }

            foreach (var obj in ListArrays)
            {
                sql = "delete from front_list where ComName='" + obj[0] + "'";
                globalConst.CurSite.SiteConn.execSql(sql);
                sql = "insert into front_list(ComName,Caption,ApiBase,ApiUrl,Rows,Search,Buttons,Pager,InitSet,JsBeforeLoad,JsBeforeSet,JsAfterSet,CustomJs,CssText,OtherSet)";
                sql += "values(";
                sql += "'" + str.Dot2DotDot(obj[0].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[1].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[2].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[3].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[4].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[5].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[6].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[7].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[8].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[9].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[10].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[11].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[12].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[13].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[14].ToString().Trim()) + "'";
                sql += ")";
                globalConst.CurSite.SiteConn.execSql(sql);
            }
            foreach (var obj in FormArrays)
            {
                sql = "delete from front_form where ComName='" + obj[0] + "'";
                globalConst.CurSite.SiteConn.execSql(sql);
                sql = "insert into front_form(ComName,Caption,ApiBase,ApiGet,ApiSet,Rows,Buttons,BindGet,BindSet,JsBeforeSubmit,JsAfterSubmit,JsBeforeGet,JsBeforeSet,JsAfterSet,CustomJs,CssText,CusDataDefine,OtherSet)";
                sql += "values(";
                sql += "'" + str.Dot2DotDot(obj[0].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[1].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[2].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[3].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[4].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[5].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[6].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[7].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[8].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[9].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[10].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[11].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[12].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[13].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[14].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[15].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[16].ToString().Trim()) + "'";
                sql += ",'" + str.Dot2DotDot(obj[17].ToString().Trim()) + "'";
                sql += ")";
                globalConst.CurSite.SiteConn.execSql(sql);
            }

            tabControl1.TabPages.Clear();
            tree.TreeViewNodeSorter = null;
            init_TreeView();
            MsgBox.Information(res.ftform.str("z017"));
        }

        private void ForeDev_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.E)
            {
                yulanToolStripMenuItem_Click(sender, e);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.W)
            {
                daochuToolStripMenuItem_Click(sender, e);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Q)
            {
                fabuToolStripMenuItem_Click(sender, e);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                saveToolStripMenuItem_Click(sender, e);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.P)
            {
                button27_Click(sender, e);
            }
        }

        private void dgv2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)//BindData
            {
                List<string[]> initdata = new List<string[]>();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    string caption = row.Cells[0].Value?.ToString() ?? "";
                    string data = row.Cells[1].Value?.ToString() ?? "";
                    if (data != "")
                    {
                        initdata.Add(new string[] { caption, data });
                    }
                }
                ForeSchBindData foreSchBindData = new ForeSchBindData();
                foreSchBindData.InitData = initdata;
                foreSchBindData.ShowDialog();
                if (foreSchBindData.ReturnVal != null)
                {
                    dgv2.EndEdit();
                    dgv2.Rows[e.RowIndex].Cells[1].Value = "sdata." + foreSchBindData.ReturnVal[1];
                    dgv2.EndEdit();
                }
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode != null && (tree.SelectedNode.Tag as object[])[0].ToString() != "dir")
            {
                ItemSave();
                var item = tree.SelectedNode;
                var obj = item.Tag as object[];
                string type = obj[0].ToString();
                string sql;
                if (type == "list")
                {
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
                    sql += "'" + str.Dot2DotDot(list.ComName.Trim() + "_copy") + "'";
                    sql += ",'" + str.Dot2DotDot(list.Caption.Trim() + "_copy") + "'";
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
                    globalConst.CurSite.SiteConn.execSql(sql);
                }
                else if (type == "form")
                {
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
                    sql += "'" + str.Dot2DotDot(form.ComName.Trim() + "_copy") + "'";
                    sql += ",'" + str.Dot2DotDot(form.Caption.Trim() + "_copy") + "'";
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
                    globalConst.CurSite.SiteConn.execSql(sql);
                }
                tabControl1.TabPages.Clear();
                tree.TreeViewNodeSorter = null;
                init_TreeView();
                EdiItem = null;
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

        private void dgv3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                ForeIcon foreIcon = new ForeIcon();
                foreIcon.TopMost = true;
                foreIcon.IconString = dgv3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";
                foreIcon.ShowDialog();
                if (!foreIcon.IsCancel)
                {
                    dgv3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = foreIcon.IconString;
                    dgv3.EndEdit();
                }
            }
        }

        private void dgv5_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                ForeIcon foreIcon = new ForeIcon();
                foreIcon.TopMost = true;
                foreIcon.IconString = dgv5.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";
                foreIcon.ShowDialog();
                if (!foreIcon.IsCancel)
                {
                    dgv5.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = foreIcon.IconString;
                    dgv5.EndEdit();
                }
            }
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            Form_LayoutTempRow.Text = "<el-row>" + Environment.NewLine + "<el-col :span=\"12\">" + Environment.NewLine + "@ITEM@" + Environment.NewLine + "</el-col>" + Environment.NewLine + "<el-col :span=\"12\">" + Environment.NewLine + "@ITEM@" + Environment.NewLine + "</el-col>" + Environment.NewLine + "</el-row>";
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            Form_LayoutTempRow.Text = "<el-row>" + Environment.NewLine + "<el-col :span=\"8\">" + Environment.NewLine + "@ITEM@" + Environment.NewLine + "</el-col>" + Environment.NewLine + "<el-col :span=\"8\">" + Environment.NewLine + "@ITEM@" + Environment.NewLine + "</el-col>" + Environment.NewLine + "<el-col :span=\"8\">" + Environment.NewLine + "@ITEM@" + Environment.NewLine + "</el-col>" + Environment.NewLine + "</el-row>";
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            Form_LayoutTempRow.Text = "<el-row>" + Environment.NewLine + "<el-col :span=\"6\">" + Environment.NewLine + "@ITEM@" + Environment.NewLine + "</el-col>" + Environment.NewLine + "<el-col :span=\"6\">" + Environment.NewLine + "@ITEM@" + Environment.NewLine + "</el-col>" + Environment.NewLine + "<el-col :span=\"6\">" + Environment.NewLine + "@ITEM@" + Environment.NewLine + "</el-col>" + Environment.NewLine + "<el-col :span=\"6\">" + Environment.NewLine + "@ITEM@" + Environment.NewLine + "</el-col>" + Environment.NewLine + "</el-row>";
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
        private void dgv4_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            return;
            foreach (DataGridViewRow row in dgv4.Rows)
            {
                if (row.DefaultCellStyle.ForeColor != Color.Red) row.DefaultCellStyle.ForeColor = Color.Black;
                if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() != "")
                {
                    if (row.Cells[1].Value.ToString().Trim().EndsWith("."))
                    {
                        row.DefaultCellStyle.ForeColor = Color.Blue;
                    }
                }
            }
        }
        private void dgv4_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgv4.CurrentCell.OwningColumn.Index == 2 && dgv4.CurrentCell.RowIndex != -1)
            {
                (e.Control as ComboBox).SelectedIndexChanged += new EventHandler(cbo_SelectedIndexChanged);
            }
        }
        void cbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combox = sender as ComboBox;
            //combox.Leave += (sender2, e2) =>
            //{
            //    (sender2 as ComboBox).SelectedIndexChanged -= new EventHandler(cbo_SelectedIndexChanged);
            //};
            try
            {
                if (combox.SelectedItem != null)
                {
                    if (combox.Text == "(Integration)")
                    {
                        ForeIntegration foreIntegration = new ForeIntegration();
                        foreIntegration.TopMost = true;
                        foreIntegration.ShowDialog();
                        if (!foreIntegration.IsCancel)
                        {
                            dgv4.CurrentRow.Cells[7].Value = foreIntegration.SetString;
                        }
                    }
                    else
                    {
                        dgv4.CurrentRow.Cells[7].Value = "";
                    }
                    dgv4.CurrentCell.Value = combox.Text;
                    dgv4.EndEdit();
                    ChangedForPreviewBrowser();
                }
                //dgv4.Focus();
                //BeginInvoke(new Action(() => {
                //    dgv4.Focus();
                //}));
            }
            catch (Exception ex)
            {
                new error(ex);
            }
            finally
            {
                combox.SelectedIndexChanged -= new EventHandler(cbo_SelectedIndexChanged);
                //dgv4.Focus();
                //BeginInvoke(new Action(() => {
                //    dgv4.Focus();
                //}));
            }
            BeginInvoke(new Action(() => {
                
            }));
            
        }

        private void checkBox4_Click(object sender, EventArgs e)
        {
            checkBox3.Checked = checkBox4.Checked;
            ShowHiddenPreviewBrowser(checkBox4.Checked);
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            checkBox4.Checked = checkBox3.Checked;
            ShowHiddenPreviewBrowser(checkBox3.Checked);
        }
        bool PreviewBrowserFirstOpen = true;
        void ShowHiddenPreviewBrowser(bool isShow)
        {
            if (isShow && ForeBrowser.FB == null)
            {
                if(PreviewBrowserFirstOpen)
                {
                    PreviewBrowserLoad("about:blank");
                    //ChangedForPreviewBrowser();
                    Task.Delay(350).Wait();
                    ForeBrowser.FB.Close();
                    Task.Delay(350).Wait();
                    PreviewBrowserLoad("about:blank");
                    ChangedForPreviewBrowser();
                    PreviewBrowserFirstOpen = false;
                }
                else
                {
                    PreviewBrowserLoad("about:blank");
                    ChangedForPreviewBrowser();
                }
            }
            else if (!isShow && ForeBrowser.FB != null)
            {
                ForeBrowser.FB.Close();
            }
        }


        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ChangedForPreviewBrowser();
        }

        private void DataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((DataGridViewComboBoxEditingControl)e.Control).SelectedIndexChanged += dgv_combo_selectedchanged;
            }
            void dgv_combo_selectedchanged(object sender2, EventArgs e2)
            {
                ComboBox combox = sender2 as ComboBox;
                combox.Leave += (sender3, e3) =>
                {
                    (sender3 as ComboBox).SelectedIndexChanged -= new EventHandler(dgv_combo_selectedchanged);
                };
                if (combox.SelectedItem != null)
                {
                    ((DataGridView)(combox.Parent.Parent)).CurrentCell.Value = combox.Text;
                    ChangedForPreviewBrowser();
                }
            }
        }

        private void dgv2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ChangedForPreviewBrowser();
        }

        private void dgv3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ChangedForPreviewBrowser();
        }


        private void List_CssText_Leave(object sender, EventArgs e)
        {
            ChangedForPreviewBrowser();
        }

        private void dgv4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ChangedForPreviewBrowser();
        }

        private void List_InitJs_Leave(object sender, EventArgs e)
        {
            ChangedForPreviewBrowser();
        }

        private void List_JsBeforeLoad_Leave(object sender, EventArgs e)
        {
            ChangedForPreviewBrowser();
        }

        private void List_JsBeforeSet_Leave(object sender, EventArgs e)
        {
            ChangedForPreviewBrowser();
        }

        private void List_JsAfterSet_Leave(object sender, EventArgs e)
        {
            ChangedForPreviewBrowser();
        }

        private void List_CustomJs_Leave(object sender, EventArgs e)
        {
            ChangedForPreviewBrowser();
        }

        private void dgv5_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ChangedForPreviewBrowser();
        }

        private void Form_BindGet_Leave(object sender, EventArgs e)
        {
            ChangedForPreviewBrowser();
        }

        private void Form_BindSet_Leave(object sender, EventArgs e)
        {
            ChangedForPreviewBrowser();
        }

        private void tree_MouseDown(object sender, MouseEventArgs e)
        {
            System.Drawing.Point p = new System.Drawing.Point(e.X, e.Y);
            TreeNode nd = tree.GetNodeAt(p);
            if(nd!=null)
            {
                tree.SelectedNode = nd;
            }
        }

        private void ForeDev_FormClosing(object sender, FormClosingEventArgs e)
        {
            globalConst.MdiForm.foreDevForm = null;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            GeneratePageMenu.DropDownItems.Clear();
            var sql = "select * from ft_ftdp_front_temp where Caption like '@(%'  order by Caption";
            var dt = Adv.RemoteSqlQuery(sql);
            foreach (DataRow dr in dt.Rows)
            {
                var obj = Generator.GetTempObj(dr);
                ToolStripMenuItem toolStripMenuItem=new ToolStripMenuItem();
                toolStripMenuItem.Text = obj.Caption.Substring(4);
                if(obj.Caption.StartsWith("@(2)"))
                {
                    toolStripMenuItem.ForeColor = Color.Blue;
                }
                toolStripMenuItem.Tag = new object[] { obj };
                GeneratePageMenu.DropDownItems.Add(toolStripMenuItem);
                toolStripMenuItem.Click += (object sender2, EventArgs e2) => { 
                    if(tree.SelectedNode!=null)
                    {
                        if (((object[])tree.SelectedNode.Tag)[0].ToString()=="dir")
                        {
                            var list = new List<string>();
                            string dir = null;
                            foreach(TreeNode node in tree.SelectedNode.Nodes)
                            {
                                string comName = null;
                                if (((object[])node.Tag)[0].ToString() == "list")
                                {
                                    comName = ((ListCols)(((object[])node.Tag)[1])).ComName;
                                    if(dir==null)
                                    {
                                        dir = comName.Substring(0, comName.LastIndexOf("/"));
                                    }
                                    comName= comName.Substring(comName.LastIndexOf("/")+1);
                                    list.Add(comName);
                                }
                                else if (((object[])node.Tag)[0].ToString() == "form")
                                {
                                    comName = ((FormCols)(((object[])node.Tag)[1])).ComName;
                                    if (dir == null)
                                    {
                                        dir = comName.Substring(0, comName.LastIndexOf("/"));
                                    }
                                    comName = comName.Substring(comName.LastIndexOf("/")+1);
                                    list.Add(comName);
                                }
                            }
                            if (dir!=null&& list.Count>0)
                            {
                                bool matchOK = true;
                                var matchCom = new List<(string comKey, string comName)>();
                                var comList=obj.ComDefine.Keys.ToList();
                                foreach(var com in comList)
                                {
                                    var comItem = com.Split('|');
                                    var comKey="@"+comItem[0]+"@";
                                    string matchedComName = null;
                                    foreach(var comName in list)
                                    {
                                        if (comItem.Contains(comName))
                                        {
                                            matchedComName= comName;
                                            break;
                                        }
                                    }
                                    if (matchedComName == null)
                                    {
                                        MsgBox.Error("No Com Matched as " + com);
                                        matchOK = false;
                                        break;
                                    }
                                    else matchCom.Add((comKey, matchedComName));
                                }
                                if(matchOK)
                                {
                                    var paras = new string[0];
                                    if(obj.Caption.StartsWith("@(2)"))
                                    {
                                        TextEditor editor = new TextEditor();
                                        editor.Text = res.ftform.str("z018");
                                        editor.ShowDialog();
                                        if(!editor.cancel)paras=editor.basetext.Trim().Split('|');
                                    }
                                    var tempCode=obj.TempCode.Replace("@path@",dir);
                                    var randomStr = new Random(DateTime.Now.Millisecond).Next(1000, 9999) + "" + new Random(DateTime.Now.Millisecond + 9).Next(1000, 9999);
                                    tempCode = tempCode.Replace("@random@", randomStr);
                                    matchCom.ForEach(r =>
                                    {
                                        tempCode = tempCode.Replace(r.comKey, r.comName);
                                    });
                                    for (var i= 0;i<paras.Length;i++) tempCode = tempCode.Replace("@para"+(i+1)+"@", paras[i]);
                                    SaveFileDialog sfd = new SaveFileDialog();
                                    sfd.Filter = "Vue Page(*.vue)|*.vue";
                                    sfd.Title = "Vue Page";
                                    if(dir!="")
                                    {
                                        sfd.FileName = dir.LastIndexOf("/")<0?dir:dir.Substring(dir.LastIndexOf("/")+1);
                                    }
                                    sfd.ShowDialog();
                                    if (!string.IsNullOrWhiteSpace(sfd.FileName) && sfd.FileName.IndexOf("\\")>0)
                                    {
                                        using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                                        {
                                            sw.Write(tempCode);
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
                        }
                    }
                };
            }
        }

        public void findComAndActive(string type,string comName,string api)
        {
            TreeNode findNode = null;
            foreach(TreeNode treeNode in tree.Nodes)
            {
                if (type == "[List]" && (treeNode.Tag as object[])[0].ToString() == "list" && ((treeNode.Tag as object[])[1] as ListCols).ComName == comName)
                {
                    findNode = treeNode;
                    break;
                }
                if (type != "[List]" && (treeNode.Tag as object[])[0].ToString() == "form" && ((treeNode.Tag as object[])[1] as FormCols).ComName == comName)
                {
                    findNode = treeNode;
                    break;
                }
                var n = findN(treeNode);
                if (n != null)
                {
                    findNode = n;
                    break;
                }
            }
            if(findNode == null)
            {
                if(MsgBox.YesNoCancel(res.ftform.str("z019")) ==DialogResult.Yes)
                {
                    if (type == "[List]")
                    {
                        ListCols listCols = new ListCols()
                        {
                            ComName = comName,
                            ApiUrl = api,
                            ApiBase = "ftdpConfig.apiBase",
                            InitSet = @"config.orderBy="""";
config.orderType="""";
config.schText="""";
config.schStrict="""";
config.pageSize=12;
config.pageNum=1;",
                            JsBeforeLoad = @"vm.$emit('beforeLoad', vm);
return true;",
                            JsBeforeSet = @"//resData is json object return from Api
vm.$emit('beforeSet', vm);
return true;",
                            JsAfterSet = @"vm.$emit('afterLoad', vm);
//resData is json object return from Api",
                            CustomJs = "",
                            CssText = @".el-header, .el-footer {
	text-align: right;
  }",
                        };
                        TreeNode item = new TreeNode() { Text = comName, ImageIndex = 3, SelectedImageIndex = 3 };
                        item.Tag = new object[] { "list", listCols };
                        tree_add_item(item, comName, "");
                    }
                    else if (type == "[Detail]")
                    {
                        FormCols formCols = new FormCols()
                        {
                            ComName = comName,
                            ApiGet = api,
                            ApiBase = "ftdpConfig.apiBase",
                            JsBeforeGet = @"vm.$emit('beforeGet', vm);
return true;",
                            JsBeforeSet = @"	//resData is json object return from Api
vm.$emit('beforeSet', vm)
return true;",
                            JsAfterSet = @"//resData is json object return from Api
vm.$emit('afterSet', vm);",
                            JsBeforeSubmit = @"vm.$emit('beforeSubmit', vm);
return true;",
                            JsAfterSubmit = "vm.$emit('afterSubmit', vm)",
                            CustomJs = "",
                            CssText = "",
                            CusDataDefine = "",
                            BindGet = "/* Auto Generate */",
                            BindSet = "/* Auto Generate */",
                        };
                        TreeNode item = new TreeNode() { Text = comName, ImageIndex = 2, SelectedImageIndex = 2 };
                        item.Tag = new object[] { "form", formCols };
                        tree_add_item(item, comName, "");
                    }
                    else if (type == "[DataOP]")
                    {
                        FormCols formCols = new FormCols()
                        {
                            ComName = comName,
                            ApiSet = api,
                            ApiBase = "ftdpConfig.apiBase",
                            JsBeforeGet = @"vm.$emit('beforeGet', vm);
return true;",
                            JsBeforeSet = @"	//resData is json object return from Api
vm.$emit('beforeSet', vm)
return true;",
                            JsAfterSet = @"//resData is json object return from Api
vm.$emit('afterSet', vm);",
                            JsBeforeSubmit = @"vm.$emit('beforeSubmit', vm);
return true;",
                            JsAfterSubmit = "vm.$emit('afterSubmit', vm)",
                            CustomJs = "",
                            CssText = "",
                            CusDataDefine = "",
                            BindGet = "/* Auto Generate */",
                            BindSet = "/* Auto Generate */",
                        };
                        TreeNode item = new TreeNode() { Text = comName, ImageIndex = 2, SelectedImageIndex = 2 };
                        item.Tag = new object[] { "form", formCols };
                        tree_add_item(item, comName, "");
                    }
                }
            }
            else
            {
                var ptn = findNode.Parent;
                while (ptn != null)
                {
                    ptn.Expand();
                    ptn = ptn.Parent;
                }
                tree.SelectedNode= findNode;
                tree_AfterSelect(null, null);
            }
            TreeNode findN(TreeNode node)
            {
                foreach (TreeNode treeNode in node.Nodes)
                {
                    if (type == "[List]" && (treeNode.Tag as object[])[0].ToString() == "list" && ((treeNode.Tag as object[])[1] as ListCols).ComName == comName)
                    {
                        findNode = treeNode;
                        break;
                    }
                    if (type != "[List]" && (treeNode.Tag as object[])[0].ToString() == "form" && ((treeNode.Tag as object[])[1] as FormCols).ComName == comName)
                    {
                        findNode = treeNode;
                        break;
                    }
                    var n = findN(treeNode);
                    if (n != null) return n;
                }
                return null;
            }
        }
        public void tabActive(int tabIndex)
        {
            tabControl1.SelectedIndex = tabIndex;
        }
        public void activeShowForBugResult(string type,int tabIndex,string keyText)
        {
            DataGridView dgv = null; 
            if(type=="list")
            {
                if(tabIndex==1)
                {
                    dgv = dataGridView1;
                }
                else if (tabIndex == 2)
                {
                    dgv = dgv2;
                }
            }
            else if (type == "form")
            {
                if (tabIndex == 1)
                {
                    dgv = dgv4;
                }
            }
            if (dgv == null) return;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells[1].Value != null && row.Cells[1].Value.ToString().ToLower() == keyText.ToLower())
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }

        private void ConfigApi(string api)
        {
            var rootNode = globalConst.MdiForm.SiteTree.Nodes[0];
            if (api.IndexOf('?') > 0)
            {
                var paths = api.Substring(0, api.IndexOf('?')).Split(new string[] { "/"},StringSplitOptions.RemoveEmptyEntries) ;
                foreach (var path in paths)
                {
                    TreeNode findNode = null;
                    foreach(TreeNode node in rootNode.Nodes)
                    {
                        if(path.ToLower()== ((string[])node.Tag)[0].ToLower())
                        {
                            findNode = node;
                            break;
                        }
                    }
                    if(findNode==null)
                    {
                        MsgBox.Error(res.ftform.str("z020"));
                        return;
                    }
                    rootNode = findNode;
                }
                globalConst.MdiForm.SiteTree.SelectedNode = rootNode;
                globalConst.MdiForm.openPage();
            }
        }
        private void button29_Click(object sender, EventArgs e)
        {
            ConfigApi(List_ApiUrl.Text.Trim());
        }

        private void button30_Click(object sender, EventArgs e)
        {
            ConfigApi(Form_ApiGet.Text.Trim());
        }

        private void button31_Click(object sender, EventArgs e)
        {
            ConfigApi(Form_ApiSet.Text.Trim());
        }

        private void DeleteServertoolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode != null && (tree.SelectedNode.Tag as object[])[0].ToString() != "dir")
            {
                if (MsgBox.YesNoCancel("Delete Local and Server ?") == DialogResult.Yes)
                {
                    string t = ((object[])tree.SelectedNode.Tag)[0].ToString();
                    var sql = "";
                    var sqlRemote = "";
                    if (t == "list")
                    {
                        var obj = ((object[])EdiItem.Tag)[1] as ListCols;
                        sql = "delete from front_list where ComName='" + str.D2DD(obj.ComName) + "'";
                        sqlRemote = "update ft_ftdp_front_list set IsNewest=0 where ComName='" + str.D2DD(obj.ComName) + "' and IsNewest=1";
                    }
                    else if (t == "form")
                    {
                        var obj = ((object[])EdiItem.Tag)[1] as FormCols;
                        sql = "delete from front_form where ComName='" + str.D2DD(obj.ComName) + "'";
                        sqlRemote = "update ft_ftdp_front_form set IsNewest=0 where ComName='" + str.D2DD(obj.ComName) + "' and IsNewest=1";
                    }
                    if (sql != "") globalConst.CurSite.SiteConn.execSql(sql);
                    if (sqlRemote != "") Adv.RemoteSqlExec(sqlRemote);
                    EdiItem = null;
                    tree.SelectedNode.Remove();

                }
            }
        }
        string dgv4MenuForWitchDgv = "";
        private void dgv4_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgv4.Rows.Count)
                {
                    dgv4MenuForWitchDgv = "dgv4";
                    dgv4.ClearSelection();
                    dgv4.Rows[e.RowIndex].Selected = true;
                    dgv4Menu.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void dgv4Menu_Cut_Click(object sender, EventArgs e)
        {
            if (dgv4MenuForWitchDgv == "dgv4")
            {
                if (dgv4.SelectedRows.Count == 1)
                {
                    cutSaveRow4 = dgv4.Rows[dgv4.SelectedRows[0].Index];
                    dgv4.Rows.Remove(cutSaveRow4);
                    ChangedForPreviewBrowser();
                }
            }
            else if (dgv4MenuForWitchDgv == "dgv1")
            {
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    cutSaveRow1 = dataGridView1.Rows[dataGridView1.SelectedRows[0].Index];
                    dataGridView1.Rows.Remove(cutSaveRow1);
                    ChangedForPreviewBrowser();
                }
            }
        }

        private void dgv4Menu_Insert_Click(object sender, EventArgs e)
        {
            if (dgv4MenuForWitchDgv == "dgv4")
            {
                if (dgv4.SelectedRows.Count == 1)
                {
                    if (cutSaveRow4 != null)
                    { dgv4.Rows.Insert(dgv4.SelectedRows[0].Index, cutSaveRow4); cutSaveRow4 = null; ChangedForPreviewBrowser(); }
                }
            }
            else if (dgv4MenuForWitchDgv == "dgv1")
            {
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    if (cutSaveRow1 != null)
                    {
                        dataGridView1.Rows.Insert(dataGridView1.SelectedRows[0].Index, cutSaveRow1); cutSaveRow1 = null;
                        ChangedForPreviewBrowser();
                    }
                }
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgv4.Rows.Count)
                {
                    dgv4MenuForWitchDgv = "dgv1";
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dgv4Menu.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void BindGetCheckBox_Click(object sender, EventArgs e)
        {
            Form_BindGet.Enabled = BindGetCheckBox.Checked;
            button22.Enabled = BindGetCheckBox.Checked;
            string newText = null;
            if(BindGetCheckBox.Checked && Form_BindGet.Text.StartsWith("/* Auto Generate */"))
            {
                if (Form_BindGet.Text.StartsWith("/* Auto Generate */" + Environment.NewLine))
                {
                    newText = Form_BindGet.Text.Substring(("/* Auto Generate */" + Environment.NewLine).Length);
                }
                else newText = Form_BindGet.Text.Substring("/* Auto Generate */".Length);
            }
            if(!BindGetCheckBox.Checked && !Form_BindGet.Text.StartsWith("/* Auto Generate */"))
            {
                newText = "/* Auto Generate */";// +Environment.NewLine +Form_BindGet.Text;
            }
            if(newText!=null)
            {
                Form_BindGet.ResetText();
                Form_BindGet.Refresh();
                Form_BindGet.Text = newText;
            }
        }

        private void BindSetCheckBox_Click(object sender, EventArgs e)
        {
            Form_BindSet.Enabled = BindSetCheckBox.Checked;
            button20.Enabled = BindSetCheckBox.Checked;
            string newText = null;
            if (BindSetCheckBox.Checked && Form_BindSet.Text.StartsWith("/* Auto Generate */"))
            {
                if(Form_BindSet.Text.StartsWith("/* Auto Generate */" + Environment.NewLine))
                {
                    newText = Form_BindSet.Text.Substring(("/* Auto Generate */" + Environment.NewLine).Length);
                }
                else newText = Form_BindSet.Text.Substring("/* Auto Generate */".Length);
            }
            if (!BindSetCheckBox.Checked && !Form_BindSet.Text.StartsWith("/* Auto Generate */"))
            {
                newText = "/* Auto Generate */";// + Environment.NewLine + Form_BindSet.Text;
            }
            if (newText != null)
            {
                Form_BindSet.ResetText();
                Form_BindSet.Refresh();
                Form_BindSet.Text = newText;
            }
        }

        private void dgv4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
    public class TreeViewNodesComparer : IComparer
    {
        private int stype = 0;
        public TreeViewNodesComparer(int _stype)
        {
            stype = _stype;
        }
        public int Compare(object x, object y)
        {
            string cx = null;
            string cy = null;
            int sortType = stype % 2;
            string type = ((object[])((TreeNode)x).Tag)[0].ToString();
            if (type == "list")
            {
                var obj = ((object[])((TreeNode)x).Tag)[1] as ListCols;
                if (stype == 1 || stype == 2) cx = obj.ComName ?? "";
                else if (stype == 3 || stype == 4) cx = obj.Caption ?? "";
            }
            else if (type == "form")
            {
                var obj = ((object[])((TreeNode)x).Tag)[1] as FormCols;
                if (stype == 1 || stype == 2) cx = obj.ComName ?? "";
                else if (stype == 3 || stype == 4) cx = obj.Caption ?? "";
            }
            else if (type == "dir")
            {
                if (stype == 1 || stype == 2) cx = ((TreeNode)x).Text;
                else if (stype == 3 || stype == 4) cx = ((TreeNode)x).Text;
            }
            type = ((object[])((TreeNode)y).Tag)[0].ToString();
            if (type == "list")
            {
                var obj = ((object[])((TreeNode)y).Tag)[1] as ListCols;
                if (stype == 1 || stype == 2) cy = obj.ComName ?? "";
                else if (stype == 3 || stype == 4) cy = obj.Caption ?? "";
            }
            else if (type == "form")
            {
                var obj = ((object[])((TreeNode)y).Tag)[1] as FormCols;
                if (stype == 1 || stype == 2) cy = obj.ComName ?? "";
                else if (stype == 3 || stype == 4) cy = obj.Caption ?? "";
            }
            else if (type == "dir")
            {
                if (stype == 1 || stype == 2) cy = ((TreeNode)x).Text;
                else if (stype == 3 || stype == 4) cy = ((TreeNode)x).Text;
            }
            if (sortType == 0) return String.Compare(cx, cy);
            else return String.Compare(cy, cx);
        }
    }
}
