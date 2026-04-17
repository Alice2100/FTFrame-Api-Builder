using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FTDPClient.Common;
using FTDPClient.consts;
using FTDPClient.functions;
using System.Text.RegularExpressions;
using System.Xml;
using mshtml;
namespace FTDPClient.forms
{
    public partial class FormData : Form
    {
        public static FormData TheFormData=null;
        public int ReturnValue = 0;
        public bool IsCancel = false;
        public static IHTMLElement ele = null;
        public static string defaultShowTab = "";
        public bool fromSite = true;
        private bool ShouldSave = false;
        public string ReturnRow = null;
        public FormData()
        {
            InitializeComponent();
            ApplyLanguage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TheFormData.Hide();
        }
        private void ApplyLanguage()
        {
            button3.Text = res._propertysite.GetString("fd_add");
            button4.Text = res._propertysite.GetString("fd_mod");
            button5.Text = res._propertysite.GetString("fd_del");
            button1.Text = res._propertysite.GetString("fd_cancel");
            button2.Text = res._propertysite.GetString("fd_ok");
            selbutton.Text = res._propertysite.GetString("String1");
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        TheFormData.Hide();
                        break;
                }

            }
            return false;
        }
        private string filename = null;
        private void initTabs()
        {
            tabControl1.TabPages.Clear();
            if (globalConst.CurSite.FormDataXML != null)
            {
                foreach (XmlNode TableNode in globalConst.CurSite.FormDataXML.SelectNodes("/formdata/table"))
                {
                    string TableName = TableNode.Attributes["id"].Value;
                    string TableCaption = TableNode.Attributes["caption"].Value;
                    addTableTab(TableCaption + "[" + TableName + "]");
                    DataGridView dg = (DataGridView)tabControl1.SelectedTab.Controls[0];
                    dg.Rows.Clear();
                    int i=0;
                    foreach (XmlNode RowNode in TableNode.SelectNodes("row"))
                    {
                        string rw_caption = RowNode.Attributes["caption"].Value;
                        string rw_id = RowNode.Attributes["id"].Value;
                        string rw_type = RowNode.SelectSingleNode("datatype").Attributes["name"].Value;
                        string rw_length = RowNode.SelectSingleNode("datatype/length").InnerText;
                        string rw_numpoint = RowNode.SelectSingleNode("datatype/numpoint").InnerText;
                        bool rw_null = bool.Parse(RowNode.SelectSingleNode("allownull").InnerText);
                        string rw_default = RowNode.SelectSingleNode("default").InnerText;
                        bool rw_mainkey = bool.Parse(RowNode.SelectSingleNode("primary").InnerText);
                        bool rw_index = bool.Parse(RowNode.SelectSingleNode("index").InnerText);
                        bool IsLock = bool.Parse(RowNode.Attributes["lock"].Value);
                        bool isbind = bool.Parse(RowNode.SelectSingleNode("bindinfo/isbind").InnerText);
                        string page = RowNode.SelectSingleNode("bindinfo/page").InnerText;
                        dg.Rows.Add(new object[]{
                        rw_caption,
                        rw_id,
                        rw_type,
                        rw_length,
                        rw_numpoint,
                        rw_null,
                        rw_default,
                        rw_mainkey,
                        rw_index,
                        page
                        }); 
                        if (IsLock)
                        {
                            dg.Rows[i].ReadOnly = true;
                            dg.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            dg.Rows[i].Cells[0].Style.ForeColor = Color.Blue;
                        }
                        dg.Rows[i].Cells[1].Style.BackColor = Color.AliceBlue;
                        if (page.Trim().Equals("") && !IsLock)
                        {
                            dg.Rows[i].Cells[9].Style.BackColor = Color.Yellow;
                        }
                        else
                        {
                            dg.Rows[i].Cells[9].Style.BackColor = Color.White;
                        }
                        i++;
                    }
                }
            }
            for (int i = 0; i < tabControl1.TabCount; i++)
            {
                if (tabControl1.TabPages[i].Text.Equals(defaultShowTab))
                {
                    tabControl1.SelectedIndex = i;
                    break;
                }
            }
        }
        public static void FormDataShow()
        {
            TheFormData.selbutton.Visible = !TheFormData.fromSite;
            TheFormData.ShowDialog();
            TheFormData.Focus();
        }
        private bool loaded = false;
        private void FormData_Load(object sender, EventArgs e)
        {
            if (loaded) return; loaded = true;
            if (globalConst.CurSite.FormDataXML!=null)
            {
                initTabs();
            }
            ShouldSave = false;
            button2.Enabled = false;

            if (fromSite) selbutton.Visible = false;
            else selbutton.Visible = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            TableName tn = new TableName();
            tn.ShowDialog();
            string tablename = tn.NameValue.Text;
            tablename=tablename.Trim().ToLower();
            if (tablename.Equals("")) return;
            if (!IsNameOK(tablename))
            {
                MsgBox.Warning("Table name must be no more than 16 chars,and with 'a-z','0-9' and '_'");
                return;
            }
            string tablecaption = tn.Caption.Text.Trim();
            if (tn.checkBox1.Checked) tablename = "@" + tablename;

            if (globalConst.CurSite.FormDataXML == null)
            {
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<formdata version=\"1.0\"><table id=\"" + tablename + "\" caption=\"" + tablecaption + "\">" + DefaultConst.getFormTableDefaultRowsXML() + "</table></formdata>";
                globalConst.CurSite.FormDataXML = new XmlDocument();
                globalConst.CurSite.FormDataXML.LoadXml(xml);
                initTabs();
            }
            else
            {
                if (tabTableNameExist(tablename,false))
                {
                    MsgBox.Warning("Table name '" + tablename + "' has exist");
                    return;
                }
                else
                {
                    addTableTab(tablecaption + "[" + tablename + "]");
                    string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<formdata version=\"1.0\"><table id=\"" + tablename + "\" caption=\"" + tablecaption + "\">" + DefaultConst.getFormTableDefaultRowsXML() + "</table></formdata>";
                    XmlDocument doc2 = new XmlDocument();
                    doc2.LoadXml(xml);
                    DataGridView dg = (DataGridView)tabControl1.SelectedTab.Controls[0];
                    dg.Rows.Clear();
                    XmlNode TableNode = doc2.SelectSingleNode("/formdata/table");
                    int i = 0;
                    foreach (XmlNode RowNode in TableNode.SelectNodes("row"))
                    {
                        string rw_caption = RowNode.Attributes["caption"].Value;
                        string rw_id = RowNode.Attributes["id"].Value;
                        string rw_type = RowNode.SelectSingleNode("datatype").Attributes["name"].Value;
                        string rw_length = RowNode.SelectSingleNode("datatype/length").InnerText;
                        string rw_numpoint = RowNode.SelectSingleNode("datatype/numpoint").InnerText;
                        bool rw_null = bool.Parse(RowNode.SelectSingleNode("allownull").InnerText);
                        string rw_default = RowNode.SelectSingleNode("default").InnerText;
                        bool rw_mainkey = bool.Parse(RowNode.SelectSingleNode("primary").InnerText);
                        bool rw_index = bool.Parse(RowNode.SelectSingleNode("index").InnerText);
                        bool IsLock = bool.Parse(RowNode.Attributes["lock"].Value);
                        bool isbind = bool.Parse(RowNode.SelectSingleNode("bindinfo/isbind").InnerText);
                        string page = RowNode.SelectSingleNode("bindinfo/page").InnerText;
                        dg.Rows.Add(new object[]{
                        rw_caption,
                        rw_id,
                        rw_type,
                        rw_length,
                        rw_numpoint,
                        rw_null,
                        rw_default,
                        rw_mainkey,
                        rw_index,
                        page
                        });
                        if (IsLock)
                        {
                            dg.Rows[i].ReadOnly = true;
                            dg.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            dg.Rows[i].Cells[0].Style.ForeColor = Color.Blue;
                        }
                        i++;
                    }
                }
            }
        }
        private void addTableTab(string tablenamecap)
        {
            TabPage tabPage = new TabPage();
            DataGridView dataGridView = new DataGridView();
            DataGridViewTextBoxColumn caption = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn name = new DataGridViewTextBoxColumn();
            DataGridViewComboBoxColumn datatype = new DataGridViewComboBoxColumn();
            DataGridViewTextBoxColumn length = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn numpoint = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn allownull = new DataGridViewCheckBoxColumn();
            DataGridViewTextBoxColumn defaultval = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn primary = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn index = new DataGridViewCheckBoxColumn();
            DataGridViewTextBoxColumn page = new DataGridViewTextBoxColumn();
            this.tabControl1.Controls.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
            dataGridView.CausesValidation = false;
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            //dataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridView.UserDeletingRow += new DataGridViewRowCancelEventHandler(dataGridView_UserDeletingRow);
            dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView_CellValidating);
            dataGridView.DefaultValuesNeeded += new DataGridViewRowEventHandler(dataGridView_DefaultValuesNeeded);
            dataGridView.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
            dataGridView.UserDeletedRow += new DataGridViewRowEventHandler(dataGridView_UserDeletedRow);
            dataGridView.UserAddedRow += new DataGridViewRowEventHandler(dataGridView_UserAddedRow);
            dataGridView.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView_CellDoubleClick);
            // tabPage1
            // 
            tabPage.Controls.Add(dataGridView);
            tabPage.Location = new System.Drawing.Point(4, 22);
            tabPage.Padding = new System.Windows.Forms.Padding(3);
            tabPage.Size = new System.Drawing.Size(920, 248);
            tabPage.TabIndex = 0;
            tabPage.Text = tablenamecap;
            tabPage.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            caption,
            name,
            datatype,
            length,
            numpoint,
            allownull,
            defaultval,
            primary,
            index,
            page
            });
            dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView.Location = new System.Drawing.Point(3, 3);
            dataGridView.RowTemplate.Height = 23;
            dataGridView.Size = new System.Drawing.Size(914, 242);
            dataGridView.TabIndex = 0;
            // 
            // caption
            // 
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle_Caption = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle_Caption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle_Caption.ForeColor = System.Drawing.Color.Blue;
            caption.DefaultCellStyle = dataGridViewCellStyle_Caption;
            caption.HeaderText = res._propertysite.GetString("rw_caption");
            caption.Name = "caption";
            caption.Width = 120;
            // 
            // name
            // 
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle_Name= new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle_Name.BackColor = System.Drawing.Color.AliceBlue;
            name.DefaultCellStyle = dataGridViewCellStyle_Name;
            name.HeaderText = res._propertysite.GetString("rw_id");
            name.Name = "name";
            name.Width = 110;
            // 
            // datatype
            // 
            datatype.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            datatype.FlatStyle = FlatStyle.Flat;
            datatype.HeaderText = res._propertysite.GetString("rw_type");
            datatype.Name = "datatype";
            foreach (string s in DefaultConst.getFormTableRowDataType())
            {
                datatype.Items.Add(s);
            }
            datatype.Width = 100;
            // 
            // length
            // 
            length.HeaderText = res._propertysite.GetString("rw_length");
            length.Name = "length";
            length.Width = 80;
            // 
            // numpoint
            // 
            numpoint.HeaderText = res._propertysite.GetString("rw_numpoint");
            numpoint.Name = "numpoint";
            numpoint.Width = 70;
            // 
            // allownull
            // 
            allownull.HeaderText = res._propertysite.GetString("rw_null");
            allownull.Name = "allownull";
            allownull.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            allownull.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            allownull.Width = 70;
            // 
            // defaultval
            // 
            defaultval.HeaderText = res._propertysite.GetString("rw_default");
            defaultval.Name = "defaultval";
            defaultval.Width = 70;
            // 
            // primary
            // 
            primary.HeaderText = res._propertysite.GetString("rw_mainkey");
            primary.Name = "primary";
            primary.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            primary.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            primary.Width = 70;
            // 
            // index
            // 
            index.HeaderText = res._propertysite.GetString("rw_index");
            index.Name = "index";
            index.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            index.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            index.Width = 70;
            // 
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle_Page = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle_Page.BackColor = Color.Yellow;
            page.DefaultCellStyle = dataGridViewCellStyle_Page;
            page.HeaderText = res._propertysite.GetString("rw_page");
            page.Name = "page";
            page.Width = 110;
        }

        void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!fromSite)
            {
                selbutton_Click(sender, null);
            }
        }

        void dataGridView_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            ShouldSave = true;
            button2.Enabled = true;
        }

        void dataGridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            ShouldSave = true;
            button2.Enabled = true;
        }

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab!=null && !tabControl1.SelectedTab.Text.Equals(""))
            {
                defaultShowTab = tabControl1.SelectedTab.Text;
            }
        }

        void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ShouldSave = true;
            button2.Enabled = true;
        }

        private void dataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[0].Value = "";
            e.Row.Cells[1].Value = AZMax();
            e.Row.Cells[2].Value = "varchar";
            e.Row.Cells[3].Value = "36";
            e.Row.Cells[4].Value = "0";
            e.Row.Cells[5].Value = true;
            e.Row.Cells[6].Value = "NULL";
            e.Row.Cells[7].Value = false;
            e.Row.Cells[8].Value = false;
            e.Row.Cells[9].Value = "";
        }
        private string AZ(int code)
        {
            //code 0——26*26-1
            int l = code / 26;
            int r = code % 26;
            return ((char)(l + 97)).ToString() + ((char)(r + 97)).ToString();
        }
        private string AZMax()
        {
            int i = 0;
            DataGridView dg = (DataGridView)tabControl1.SelectedTab.Controls[0];
            for (i = 0; i < 26 * 26; i++)
            {
                string eid = AZ(i);
                if (eid.Equals("as") || eid.Equals("by")) continue;
                bool hasin = false;
                foreach (DataGridViewRow row in dg.Rows)
                {
                    if (row.Cells[1].Value == null) continue;
                    if (eid.Equals(row.Cells[1].Value.ToString()))
                    {
                        hasin = true;
                        break;
                    }
                }
                if (!hasin) return eid;
            }
            return "";
        }
        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.ReadOnly)
            {
                MsgBox.Warning("Can not delete locked row");
                e.Cancel = true;
            }
        }

        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if(!IsNameOK(e.FormattedValue.ToString()))
                {
                    MsgBox.Warning("Column name must be no more than 16 chars,and with 'a-z','0-9' and '_'");
                    e.Cancel = true;
                }
                DataGridView dg = (DataGridView)tabControl1.SelectedTab.Controls[0];
                foreach (DataGridViewRow row in dg.Rows)
                {
                    if (e.RowIndex == row.Index) continue;
                    if (row.Cells[1].Value==null) continue;
                    if (e.FormattedValue.ToString().ToLower().Equals(row.Cells[1].Value.ToString().ToLower()))
                    {
                        MsgBox.Warning("The column name has exist!");
                        e.Cancel = true;
                        break;
                    }
                }
            }
            else if (e.ColumnIndex == 2)
            {
                if (e.FormattedValue == null || e.FormattedValue.ToString().Equals(""))
                {
                    MsgBox.Warning("Must select datatype");
                    e.Cancel = true;
                }
            }
            else if (e.ColumnIndex == 3)
            {
                int x = -1;
                int.TryParse(e.FormattedValue.ToString(), out x);
                if (x < 0)
                {
                    MsgBox.Warning("Must be integer and no less than 0");
                    e.Cancel = true;
                }
            }
            else if (e.ColumnIndex == 4)
            {
                int x = -1;
                int.TryParse(e.FormattedValue.ToString(), out x);
                if (x < 0)
                {
                    MsgBox.Warning("Must be integer and no less than 0");
                    e.Cancel = true;
                }
            }
        }
        private bool IsNameOK(string s)
        {
            if (s.Length == 0 || s.Length > 16) return false;
            Regex rex = new Regex("^[a-z0-9A-Z_]+$"); 
            Match ma = rex.Match(s);
            if (ma.Success) return true;
            else return false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TableName tn = new TableName();
            string tabtext=tabControl1.SelectedTab.Text;
            string tablename = tabtext.Substring(tabtext.IndexOf('[') + 1, tabtext.IndexOf(']') - tabtext.IndexOf('[')-1);
            string tablecaption = tabtext.Substring(0, tabtext.IndexOf('['));
            bool isalltable = tablename.StartsWith("@");
            if (isalltable) tablename = tablename.Substring(1);
            tn.NameValue.Text = tablename;
            tn.Caption.Text = tablecaption;
            tn.checkBox1.Checked = isalltable;
            tn.ShowDialog();
            tablename = tn.NameValue.Text.Trim().ToLower();
            if (tablename.Equals("")) return;
            if (!IsNameOK(tablename))
            {
                MsgBox.Warning("Table name must be no more than 16 chars,and with 'a-z','0-9' and '_'");
                return;
            }
            tablecaption = tn.Caption.Text.Trim();
            if (tn.checkBox1.Checked) tablename = "@" + tablename;
            if (tabTableNameExist(tablename, true))
            {
                MsgBox.Warning("Table name '" + tablename + "' has exist");
                return;
            }
            else
            {
                tabControl1.SelectedTab.Text=tablecaption + "[" + tablename + "]";
            }
        }
        private bool tabTableNameExist(string tablename,bool ismod)
        {
            foreach(TabPage tab in this.tabControl1.TabPages)
            {
                if(ismod)
                {
                    if(tab.Text.Equals(tabControl1.SelectedTab.Text))
                    {
                        continue;
                    }
                }
                if(tab.Text.EndsWith("["+tablename+"]"))return true;
            }
            return false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MsgBox.OKCancel("Confirm delete this table?").Equals(DialogResult.OK))
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filename = globalConst.ConfigPath + "\\" + globalConst.CurSite.ID + "_formtable.xml";
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            xml += "<formdata version=\"1.0\">";
            foreach (TabPage tp in tabControl1.TabPages)
            {
                string tabtext = tp.Text;
                string tablename = tabtext.Substring(tabtext.IndexOf('[') + 1, tabtext.IndexOf(']') - tabtext.IndexOf('[') - 1);
                string tablecaption = tabtext.Substring(0, tabtext.IndexOf('['));
                xml += "<table id=\"" + tablename + "\" caption=\"" + tablecaption + "\">";
                DataGridView dg = (DataGridView)tp.Controls[0];
                dg.EndEdit();
                foreach (DataGridViewRow row in dg.Rows)
                {
                    if (row.Cells[1].Value != null && !row.Cells[1].Value.ToString().Equals(""))
                    {
                        string id = row.Cells[1].Value.ToString();
                        string caption = row.Cells[0].Value.ToString();
                        string islock = row.ReadOnly.ToString();
                        string datatype = row.Cells[2].Value.ToString();
                        string datalength = row.Cells[3].Value.ToString();
                        string numpoint = row.Cells[4].Value.ToString();
                        string allownull = row.Cells[5].Value.ToString();
                        string defaultval = row.Cells[6].Value.ToString();
                        string mainkey = row.Cells[7].Value.ToString();
                        string index = row.Cells[8].Value.ToString();
                        string isbind = (row.Cells[9].Value != null && !row.Cells[9].Value.ToString().Equals("")).ToString();
                        string page = row.Cells[9].Value == null ? "" : row.Cells[9].Value.ToString();
                        xml += "<row id=\"" + id + "\" caption=\"" + caption + "\" lock=\"" + islock + "\">\r\n";
                        xml += "			<datatype name=\"" + datatype + "\">\r\n";
                        xml += "				<length>" + datalength + "</length>\r\n";
                        xml += "				<numpoint>" + numpoint + "</numpoint>\r\n";
                        xml += "			</datatype>\r\n";
                        xml += "			<allownull>" + allownull + "</allownull>\r\n";
                        xml += "			<default>" + defaultval + "</default>\r\n";
                        xml += "			<primary>" + mainkey + "</primary>\r\n";
                        xml += "			<index>" + index + "</index>\r\n";
                        xml += "			<bindinfo>\r\n";
                        xml += "				<isbind>" + isbind + "</isbind>\r\n";
                        xml += "				<page>" + page + "</page>\r\n";
                        xml += "			</bindinfo>\r\n";
                        xml += "		</row>\r\n";
                    }
                }
                xml += "</table>";
            }
            xml += "</formdata>";
            globalConst.CurSite.FormDataXML = new XmlDocument();
            globalConst.CurSite.FormDataXML.LoadXml(xml);
            globalConst.CurSite.FormDataXML.Save(filename);
            MsgBox.Information("Saved Successfully!");
            ShouldSave = false;
            button2.Enabled = false;
            if (fromSite) TheFormData.Hide();
        }

        private void selbutton_Click(object sender, EventArgs e)
        {
            DataGridView dg = (DataGridView)tabControl1.SelectedTab.Controls[0];
            dg.EndEdit();
            if (ShouldSave)
            {
                MsgBox.Information("Please save first before select");
                return;
            }
            string rowid = null;
            if (dg.SelectedRows != null && dg.SelectedRows.Count > 1)
            {
                MsgBox.Warning("Please select only one datarow or datacell");
                return;
            }
            if (dg.SelectedRows != null && dg.SelectedRows.Count == 1)
            {
                rowid = dg.SelectedRows[0].Cells[1].Value.ToString();
            }
            else
            {
                if (dg.SelectedCells.Count == null || dg.SelectedCells.Count > 1)
                {
                    MsgBox.Warning("Please select only one datarow or datacell");
                    return;
                }
                else
                {
                    rowid = dg.Rows[dg.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
                }
            }
            string tabtext = tabControl1.SelectedTab.Text;
            string tablename = tabtext.Substring(tabtext.IndexOf('[') + 1, tabtext.IndexOf(']') - tabtext.IndexOf('[') - 1);
            if (ele != null)
            {
                string para = ele.getAttribute("tag", 0) == null ? null : ele.getAttribute("tag", 0).ToString();
                para = str.SetSplitValue(para, 2, 0, tablename + "." + rowid);
                ele.setAttribute("tag", para, 0);
                PropertySpace.FormElement.func.setNewID(ele, tablename + "." + rowid);
                Page.PageWare.UpdateFormBindPage(tablename + "." + rowid);
            }
            else
            {
                ReturnRow = tablename + "." + rowid;
            }
            TheFormData.Hide();
        }

        private void FormData_FormClosing(object sender, FormClosingEventArgs e)
        {
            //TheFormData.Visible = false;
            //e.Cancel = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取剪切板的内容，并按行分割
                string pasteText = Clipboard.GetText();
                if (string.IsNullOrEmpty(pasteText))
                    return;
                string[] lines = pasteText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line.Trim()))
                        continue;
                    string[] vals = line.Split(new string[] { "	" }, StringSplitOptions.None);
                    
                    DataGridView dgv = (DataGridView)this.tabControl1.SelectedTab.Controls[0];
                    dgv.Rows.Add(new object[]{
                vals[1],
                vals[2],
                vals[3],
                vals[4],
                vals[5],
                vals[6],
                vals[7],
                vals[8],
                vals[9]
                });
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
            button2.Enabled = true;
        }
    }
}
