using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using mshtml;
using System.Xml;
using FTDPClient.functions;
using FTDPClient.forms.control;
using FTDPClient.Page;
using Microsoft.Data.Sqlite;
using FTDPClient.consts;

namespace FTDPClient.forms
{
    public partial class DyValueDefine : Form
    {
        public static bool DyValueFormShow = false;
        public static DyValueDefine DyValueDefineForm = null;
        public string str;
        public List<string[]> idCapList;
        public bool IsCancel = true;
        public string partid=null;
        public System.Xml.XmlDocument XmlDoc;
        public Tuple<IHTMLElement, string, string, string, int> EditorTuple = null;
        public DyValueDefine()
        {
            InitializeComponent();
            this.dgv.ShowCellToolTips = false;
            this.toolTip1.AutomaticDelay = 0;
            this.toolTip1.OwnerDraw = true;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.ToolTipTitle = " ";
            this.toolTip1.UseAnimation = true;
            this.toolTip1.UseFading = true;
        }
        public void EndDefine()
        {
            str = "";
            idCapList = new List<string[]>();
            dgv.EndEdit();
            ArrayList al = new ArrayList();
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                DataGridViewRow r = dgv.Rows[i];
                if (r.Cells[1].Value != null && !r.Cells[1].Value.ToString().Trim().Equals("") && !al.Contains(r.Cells[1].Value.ToString().Trim()))
                {
                    //al.Add(r.Cells[1].Value.ToString().Trim());
                    if (!str.Equals("")) str += "|||";
                    str += (r.Cells[0].Value == null ? "" : r.Cells[0].Value.ToString().Trim()) + "##"
                        + r.Cells[1].Value.ToString().Trim() + "##"
                        + functions.str.getEncode(r.Cells[2].Value == null ? "" : r.Cells[2].Value.ToString().Trim()) + "##"
                        + (r.Cells[3].Value == null ? "" : r.Cells[3].Value.ToString().Trim()) + "##"
                        + (r.Cells[4].Value.ToString().Trim().Equals(res.ctl.str("DyValueDefine.0")) ? "0" : "1") + "##"
                        + (r.Cells[5].Value.ToString().Trim().Equals(res.ctl.str("DyValueDefine.1")) ? "0" : "1") + "##"
                        + (r.Cells[6].Value.ToString().Trim().Equals(res.ctl.str("DyValueDefine.2")) ? "0" : "1") + "##"
                        + functions.str.getEncode(r.Cells[7].Value == null ? "" : r.Cells[7].Value.ToString().Trim());
                    idCapList.Add(new string[] {
                        r.Cells[1].Value.ToString().Trim(),
                    (r.Cells[0].Value == null ? "" : r.Cells[0].Value.ToString().Trim())
                    });
                }
            }
            al.Clear();
            al = null;
        }
        public static List<string[]> IdNameList(string partid)
        {
            string define=Page.PageWare.getPartParamValue(partid, "dyvalue", "Interface", "Define");
            List<string[]> list = new List<string[]>();
            string[] items = define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items)
            {
                string[] row = item.Split(new string[] { "##" }, StringSplitOptions.None);
                list.Add(new string[] { row[1], row[0] });
            }
            return list;
        }
        public void InitDefine()
        {
            dgv.Rows.Clear();
            string[] items=str.Split(new string[]{"|||"},StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items)
            {
                string[] row = item.Split(new string[] { "##" }, StringSplitOptions.None);
                dgv.Rows.Add(new object[]{
                row[0],
                row[1],
                functions.str.getDecode(row[2]),
                row[3],
                row[4].Equals("0")?res.ctl.str("DyValueDefine.0"):res.ctl.str("DyValueDefine.3"),
                row[5].Equals("0")?res.ctl.str("DyValueDefine.1"):res.ctl.str("DyValueDefine.4"),
                row[6].Equals("0")?res.ctl.str("DyValueDefine.2"):res.ctl.str("DyValueDefine.5"),
                functions.str.getDecode(row[7]),
                "Delete"
                });
            }
        }
        private void DataOpDefine_Resize(object sender, EventArgs e)
        {
            var newH = this.Height - 445 + 369;
            if (newH >= Math.Max(splitContainer1.Panel1MinSize, Width - splitContainer1.Panel1MinSize) || newH <= Math.Min(splitContainer1.Panel1MinSize, Width - splitContainer1.Panel1MinSize))
            return;
            splitContainer1.SplitterDistance = newH;
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
                        this.Close();
                        break;
                }

            }
            return false;
        }

        private void DataOpDefine_Load(object sender, EventArgs e)
        {
            #region language
            this.Text = res.ctl.str("DyValueDefine.text");          //数据获取 设置
            button1.Text = res.ctl.str("DyValueDefine.button1");            //确定(&O)
            button3.Text = res.ctl.str("DyValueDefine.button3");            //粘贴(&P)
            button6.Text = res.ctl.str("DyValueDefine.button6");            //统一表名
            label1.Text = res.ctl.str("DyValueDefine.label1");          //第
            label2.Text = res.ctl.str("DyValueDefine.label2");          //列
            button4.Text = res.ctl.str("DyValueDefine.button4");            //替换
            button8.Text = res.ctl.str("DyValueDefine.button8");            //ID=列名
            button2.Text = res.ctl.str("DyValueDefine.button2");            //取消(&C)
            button9.Text = res.ctl.str("DyValueDefine.button9");            //API定义
            button7.Text = res.ctl.str("DyValueDefine.button7");            //快速定义
            button5.Text = res.ctl.str("DyValueDefine.button5");			//帮助文档
            button12.Text = res.ctl.str("DyValueDefine.button12");			//帮助文档
            #endregion
            DataOpDefine_Resize(sender, e);
            this.Width = button1.Location.X + 110 ;
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(new string[] { res.ctl.GetString("DyValueDefine.14"), res.ctl.GetString("DyValueDefine.1"), res.ctl.GetString("DyValueDefine.4"), });
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            DyValueFormShow = true;
            DyValueDefineForm = this;
            dgv.Rows.Clear();
            DataGridViewTextBoxColumn caption = new DataGridViewTextBoxColumn();
            caption.SortMode = DataGridViewColumnSortMode.NotSortable;
            caption.HeaderText = res.ctl.str("DyValueDefine.6");
            caption.ToolTipText = res.ctl.str("DyValueDefine.7");
            caption.Width = 140;
            DataGridViewTextBoxColumn name = new DataGridViewTextBoxColumn();
            name.SortMode = DataGridViewColumnSortMode.NotSortable;
            name.HeaderText = res.ctl.str("DyValueDefine.8");
            caption.ToolTipText = res.ctl.str("DyValueDefine.9");
            name.Width = 210;
            DataGridViewCellStyle dgvstyle_name = new DataGridViewCellStyle();
            dgvstyle_name.BackColor = Color.Yellow;
            name.DefaultCellStyle = dgvstyle_name;
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            col.HeaderText = res.ctl.str("DyValueDefine.10");
            col.Width = 210;
            DataGridViewTextBoxColumn fid = new DataGridViewTextBoxColumn();
            fid.SortMode = DataGridViewColumnSortMode.NotSortable;
            fid.HeaderText = res.ctl.str("DyValueDefine.11");
            fid.ToolTipText = res.ctl.str("DyValueDefine.12");
            fid.Width = 130;
            DataGridViewComboBoxColumn dytype = new DataGridViewComboBoxColumn();
            dytype.SortMode = DataGridViewColumnSortMode.NotSortable;
            dytype.HeaderText = res.ctl.str("DyValueDefine.13");
            dytype.Width = 80;
            dytype.Items.Add(res.ctl.str("DyValueDefine.0"));
            dytype.Items.Add(res.ctl.str("DyValueDefine.3"));
            DataGridViewComboBoxColumn isdy = new DataGridViewComboBoxColumn();
            isdy.SortMode = DataGridViewColumnSortMode.NotSortable;
            isdy.HeaderText = res.ctl.str("DyValueDefine.14");
            isdy.Width = 120;
            isdy.Items.Add(res.ctl.str("DyValueDefine.1"));
            isdy.Items.Add(res.ctl.str("DyValueDefine.4"));
            DataGridViewComboBoxColumn isdim = new DataGridViewComboBoxColumn();
            isdim.SortMode = DataGridViewColumnSortMode.NotSortable;
            isdim.HeaderText = res.ctl.str("DyValueDefine.2");
            isdim.Width = 80;
            isdim.Items.Add(res.ctl.str("DyValueDefine.2"));
            isdim.Items.Add(res.ctl.str("DyValueDefine.5"));
            DataGridViewTextBoxColumn sql = new DataGridViewTextBoxColumn();
            sql.SortMode = DataGridViewColumnSortMode.NotSortable;
            sql.HeaderText = res.ctl.str("DyValueDefine.15");
            sql.ToolTipText = res.ctl.str("DyValueDefine.16");
            sql.Width = 275;
            DataGridViewButtonColumn del = new DataGridViewButtonColumn();
            del.SortMode = DataGridViewColumnSortMode.NotSortable;
            del.HeaderText = res.ctl.str("DyValueDefine.17");
            del.Width = 80;

            dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            caption,
            name,
            col,
		fid,
		dytype,
		isdy,
		isdim,
		sql,
            del
            });
            dgv.CellClick += new DataGridViewCellEventHandler(dgv_CellClick);
            dgv.CellDoubleClick += new DataGridViewCellEventHandler(dgv_CellDoubleClick);
            dgv.DefaultValuesNeeded += new DataGridViewRowEventHandler(dgv_DefaultValuesNeeded);

            dytype.Visible= false;
            isdim.Visible = false;
            fid.Width += 80;
            sql.Width += 80;

            InitDefine();
        }

        void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int CIndex = e.ColumnIndex;
                if (CIndex == 1|| CIndex == 3)
                {
                    object val = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    TextEditor te = new TextEditor();
                    te.basetext = (val == null ? "" : val.ToString());
                    this.TopMost = false;
                    if (CIndex == 3) te.fromWhere = "filterrule";
                    te.ShowInTaskbar = true;
                    te.TopMost = true;
                    te.ShowDialog();
                    this.TopMost = true;
                    if (!te.cancel)
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = te.basetext;
                    }
                }
                 else   if (CIndex == 2)
                {
                    object val = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    string connstr = Options.GetSystemDBSetConnStr();
                    var dbtype = Options.GetSystemDBSetType();
                    if(connstr == null || connstr.Trim().Equals(""))
                    {
                        MsgBox.Warning(res.ctl.str("DyValueDefine.18"));
                        return;
                    }
                    if (val == null || val.ToString().Trim().IndexOf(".") < 0)
                    {
                        if (dbtype == globalConst.DBType.MySql)
                        {
                            control.SelTable_MySql sel = new control.SelTable_MySql();
                            sel.connstr = connstr;
                            sel.ShowDialog();
                            if (sel.tablename != null)
                            {
                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "@"+ sel.tablename +".";
                                dgv.EndEdit();
                            }
                        }
                        else if (dbtype == globalConst.DBType.SqlServer)
                        {
                            control.SelTable_SqlServer sel = new control.SelTable_SqlServer();
                            sel.connstr = connstr;
                            sel.ShowDialog();
                            if (sel.tablename != null)
                            {
                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "@" + sel.tablename + ".";
                                dgv.EndEdit();
                            }
                        }
                        else if (dbtype == globalConst.DBType.Sqlite)
                        {
                            control.SelTable_Sqlite sel = new control.SelTable_Sqlite();
                            sel.connstr = connstr;
                            sel.ShowDialog();
                            if (sel.tablename != null)
                            {
                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "@" + sel.tablename + ".";
                                dgv.EndEdit();
                            }
                        }
                        else
                        {
                            MsgBox.Warning(dbtype+" "+ res.ctl.str("DyValueDefine.19"));
                        }
                        //if (FormData.TheFormData == null)
                        //{
                        //    FormData.TheFormData = new FormData();
                        //}
                        //FormData.TheFormData.fromSite = false;
                        //FormData.ele = null;
                        //FormData.FormDataShow();
                        //if (!FormData.TheFormData.IsCancel)
                        //{
                        //    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = FormData.TheFormData.ReturnRow;
                        //    dgv.EndEdit();
                        //}
                    }
                    else
                    {
                        object idVal = dgv.Rows[e.RowIndex].Cells[1].Value;
                        string tablename = val.ToString().Trim();
                        tablename = tablename.Substring(0, tablename.IndexOf('.'));
                        string otablename = tablename;
                        if (tablename.StartsWith("@")) tablename = tablename.Substring(1);
                        else
                        {
                            tablename = "ft_" + consts.globalConst.CurSite.ID + "_f_" + tablename;
                        }
                        if (dbtype == globalConst.DBType.SqlServer)
                        {
                            control.SelCol_SqlServer sel = new control.SelCol_SqlServer();
                            sel.connstr = connstr;
                            sel.tablename = tablename;
                            sel.colname = null;
                            sel.nameval = idVal == null ? null : idVal.ToString();
                            sel.ShowDialog();
                            if (sel.colname != null)
                            {
                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = otablename + "." + sel.colname;
                                dgv.EndEdit();
                            }
                        }
                        else if (dbtype == globalConst.DBType.MySql)
                        {
                            control.SelCol_MySql sel = new control.SelCol_MySql();
                            sel.connstr = connstr;
                            sel.tablename = tablename;
                            sel.colname = null;
                            sel.nameval = idVal == null ? null : idVal.ToString();
                            sel.ShowDialog();
                            if (sel.colname != null)
                            {
                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = otablename + "." + sel.colname;
                                dgv.EndEdit();
                            }
                        }
                        else if (dbtype == globalConst.DBType.Sqlite)
                        {
                            control.SelCol_Sqlite sel = new control.SelCol_Sqlite();
                            sel.connstr = connstr;
                            sel.tablename = tablename;
                            sel.colname = null;
                            sel.nameval = idVal == null ? null : idVal.ToString();
                            sel.ShowDialog();
                            if (sel.colname != null)
                            {
                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = otablename + "." + sel.colname;
                                dgv.EndEdit();
                            }
                        }
                    }
                }
            }
            catch { }
        }

        void dgv_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[0].Value = "";
            e.Row.Cells[1].Value = "";
            e.Row.Cells[2].Value = "";
            e.Row.Cells[3].Value = "";
            e.Row.Cells[4].Value = res.ctl.str("DyValueDefine.0");
            e.Row.Cells[5].Value = res.ctl.str("DyValueDefine.1");
            e.Row.Cells[6].Value = res.ctl.str("DyValueDefine.2");
            e.Row.Cells[7].Value = "";
            e.Row.Cells[8].Value = "Delete";
        }

        void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int CIndex = e.ColumnIndex;
                if (CIndex == 8)
                {
                        dgv.Rows.RemoveAt(e.RowIndex);
                }
                else if (CIndex == 7)
                {

                    SQL fd = new SQL();
                    fd.restr = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    this.TopMost = false;
                    fd.fromWhere = "dyvalue";
                    fd.ShowInTaskbar = true;
                    fd.TopMost = true;
                    fd.ShowDialog();
                    this.TopMost = true;
                    if (!fd.IsCancel)
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value= fd.restr;
                    }
                }
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EndDefine();
            IsCancel = false;
            string sql = "select partxml from parts where id='" + partid + "'";
            SqliteDataReader rdr = consts.globalConst.CurSite.SiteConn.OpenRecord(sql);
            if (rdr.Read())
            {
                string PartXml = rdr.GetString(0);
                rdr.Close();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(PartXml);
                XmlNodeList nodes = doc.SelectNodes("/partxml/param");
                XmlNode curnode = null;
                foreach (XmlNode node in nodes)
                {
                    if (node.SelectSingleNode("name").InnerText.Equals("Define"))
                    {
                        curnode = node;
                        break;
                    }
                }
                curnode.SelectSingleNode("value").InnerText = str;
                sql = "update parts set partxml='" + functions.str.Dot2DotDot(doc.OuterXml) + "' where id='" + partid + "'";
                consts.globalConst.CurSite.SiteConn.execSql(sql);

                if (EditorTuple != null)
                {
                    PageWare.ApplyPartHTML(EditorTuple.Item1, EditorTuple.Item2, EditorTuple.Item3, EditorTuple.Item4, doc.OuterXml, EditorTuple.Item5);
                }

                curnode = null;
                nodes = null;
                doc = null;
            }
            else
            {
                rdr.Close();
            }
            try
            {
                Page.PageWare.doHtmlAdapter((IHTMLElement)(consts.globalConst.MdiForm.CurPropTag.Tag));
            }
            catch { }
            this.Close();
        }

        private void DataOpDefine_FormClosed(object sender, FormClosedEventArgs e)
        {
            DyValueFormShow = false;
            DyValueDefineForm = null;
        }

        public static void EleDyAdd(IHTMLElement ele)
        {
            string id=Adapter.Property.PropertyAdapter.getEleAttr(ele, "id");
            if (!id.Equals(""))
            {
                string type=Adapter.Property.PropertyAdapter.getEleAttr(ele, "type");
                if (functions.str.IsFormEleTag(ele.tagName, type))
                {
                    bool ishave = false;
                    for (int i = 0; i < DyValueDefineForm.dgv.Rows.Count; i++)
                    {
                        DataGridViewRow r = DyValueDefineForm.dgv.Rows[i];
                        if (r.Cells[1].Value != null && r.Cells[1].Value.ToString().Equals(id))
                        {
                            ishave = true;
                            break;
                        }
                    }
                    if (!ishave)
                    {
                        if (DyValueDefineForm.dgv.Rows.Count > 1)
                        {
                            DataGridViewRow row = DyValueDefineForm.dgv.Rows[DyValueDefineForm.dgv.Rows.Count - 2];
                            string bindcol = row.Cells[2].Value == null ? "" : row.Cells[2].Value.ToString();
                            if (bindcol.IndexOf(".") >= 0)
                            {
                                bindcol = bindcol.Substring(0, bindcol.IndexOf(".") + 1);
                            } 
                            DyValueDefineForm.dgv.Rows.Add(new object[]{
                            "",
                            id,
                            bindcol,
                            row.Cells[3].Value == null ? "" : row.Cells[3].Value.ToString(),
                            row.Cells[4].Value == null ? "" : row.Cells[4].Value.ToString(),
                            row.Cells[5].Value == null ? "" : row.Cells[5].Value.ToString(),
                            res.ctl.str("DyValueDefine.2"),
                            "",
                            "Delete"
                            });
                        }
                        else
                        {
                                DyValueDefineForm.dgv.Rows.Add(new object[]{
                            "",
                            id,
                            "",
                            "",
                            res.ctl.str("DyValueDefine.3"),
                            res.ctl.str("DyValueDefine.1"),
                            res.ctl.str("DyValueDefine.2"),
                            "",
                            "Delete"
                            });
                        }
                    }
                }
            }
        }
        public void DataGirdViewCellPaste()
        {
            try
            {
                // 获取剪切板的内容，并按行分割
                string pasteText = Clipboard.GetText();
                if (string.IsNullOrEmpty(pasteText))
                    return;
                pasteText = pasteText.Replace("Multiple Rows", "[MultipleRows]").Replace("Single Row", "[SingleRow]");
                string[] lines = pasteText.Split(new string[] { "Delete\r\n", "Delete\n", "Delete\r" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line.Trim()))
                        continue;
                    string[] vals = line.Split(new string[] { "	" }, StringSplitOptions.None);
                    string[] val2 = new string[9];
                    for (var i = 0; i <= 4; i++)
                    {
                        if (vals[i + 1] == "[MultipleRows]") vals[i + 1] = "Multiple Rows";
                        else if (vals[i + 1] == "[SingleRow]") vals[i + 1] = "Single Row";
                        if (i < 4)
                        {
                            val2[i] = vals[i + 1];
                        }
                        else if (i == 4)
                        {
                            val2[i] = res.ctl.str("DyValueDefine.3");
                            val2[i+1] = vals[i + 1];
                            val2[i + 2] = res.ctl.str("DyValueDefine.2");
                        }
                        //else if (i >=5)
                        //{
                        //    val2[i + 2] = vals[i + 1];
                        //}
                    }
                    val2[7] = "";
                    for (var i = 6; i < vals.Length; i++)
                    {
                        if (i == vals.Length - 1 && vals[i] == "Delete") break;
                        if (i > 6) val2[7] += "	";
                        val2[7] += vals[i];
                    }
                    val2[8] = "Delete";
                    dgv.Rows.Add(val2);
                    //    dgv.Rows.Add(new object[]{
                    //vals[1],
                    //vals[2],
                    //vals[3],
                    //vals[4],
                    //vals[5],
                    //vals[6],
                    //vals[7],
                    //vals[8],
                    //vals[9]
                    //});
                }
            }
            catch(Exception ex)
            {
                new error(ex);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DataGirdViewCellPaste();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = null;
            if (comboBox1.SelectedIndex > 0)
            {
                val = comboBox1.SelectedItem.ToString();
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    DataGridViewRow r = dgv.Rows[i];
                    if (r.Cells[1].Value != null && !r.Cells[1].Value.ToString().Trim().Equals(""))
                    {
                        r.Cells[4].Value = val;
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = null;
            if (comboBox2.SelectedIndex > 0)
            {
                val = comboBox2.SelectedItem.ToString();
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    DataGridViewRow r = dgv.Rows[i];
                    if (r.Cells[1].Value != null && !r.Cells[1].Value.ToString().Trim().Equals(""))
                    {
                        r.Cells[5].Value = val;
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int index = int.Parse(textBox3.Text);
            if (textBox1.Text != "")
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (dgv.Rows[i].Cells[index].Value != null && !dgv.Rows[i].Cells[index].Value.ToString().Equals(""))
                    {
                        dgv.Rows[i].Cells[index].Value = dgv.Rows[i].Cells[index].Value.ToString().Replace(textBox1.Text, textBox2.Text);
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string tablename = textBox4.Text.Trim();
            if (tablename.Equals("")) return;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Cells[2].Value != null)
                {
                    string newval = "";
                    string oldval = dgv.Rows[i].Cells[2].Value.ToString().Trim();
                    if (oldval.IndexOf('.') < 0) newval = tablename + "." + oldval;
                    else
                    {
                        newval = tablename + oldval.Substring(oldval.LastIndexOf('.'));
                    }
                    dgv.Rows[i].Cells[2].Value = newval;
                }
            }
        }

        private void dgv_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 2)
            {
                Editor ed = form.getEditor();
                if (ed != null)
                {
                    try
                    {
                        if (dgv.Rows[e.RowIndex].Cells[1].Value != null && !dgv.Rows[e.RowIndex].Cells[1].Value.ToString().Equals(""))
                        {
                            this.toolTip1.Hide(this.dgv);
                            Point mousePos = PointToClient(MousePosition);

                            string tip = "";

                            IHTMLElement ele = ed.editocx.getElementById(dgv.Rows[e.RowIndex].Cells[1].Value.ToString());
                            tip += ele.parentElement.parentElement.outerText;

                            this.toolTip1.Show(tip, this.dgv, mousePos);//在指定位置显示提示工具
                        }
                    }
                    catch { }
                }
            }
        }

        private void dgv_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            this.toolTip1.Hide(this.dgv);
        }

        private void toolTip1_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.AliceBlue, e.Bounds);
            e.Graphics.DrawRectangle(Pens.Chocolate, new Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height - 1));
            e.Graphics.DrawString(this.toolTip1.ToolTipTitle + e.ToolTipText, e.Font, Brushes.Red, e.Bounds);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            functions.sheel.ExeSheel("http://www.ftframe.com/doc/dyvalue.mht");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (XmlDoc != null)
            {
                string apiset = Page.PageWare.getPartParamValue(XmlDoc, "APISet");
                if (apiset == null) functions.MsgBox.Error("api set error");
                else
                {
                    EndDefine();
                    DyValue_Api list_Api = new DyValue_Api();
                    list_Api.restr = apiset;
                    list_Api.idCapList = idCapList;
                    list_Api.TopMost = true;
                    list_Api.ShowDialog();
                    if (!list_Api.IsCancel)
                    {
                        Page.PageWare.setPartParamValue(ref XmlDoc, partid, "APISet", list_Api.restr);
                    }
                }
            }
            else
            {
                string apiset = Page.PageWare.getPartParamValue(partid, "dyvalue", "Interface", "APISet");
                if (apiset == null) functions.MsgBox.Error("api set error");
                else
                {
                    EndDefine();
                    DyValue_Api list_Api = new DyValue_Api();
                    list_Api.restr = apiset;
                    list_Api.idCapList = idCapList;
                    list_Api.TopMost = true;
                    list_Api.ShowDialog();
                    if (!list_Api.IsCancel)
                    {
                        Page.PageWare.setPartParamValue(partid, "dyvalue", "Interface", "APISet", list_Api.restr);
                    }
                }
            }
            PropertySpace.Site.PropertyPart.doPartProperty(partid);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            RowAll_SetCols rs = new RowAll_SetCols();
            rs.OpType = "DyValue";
            var OriSet = new Dictionary<string, List<string>>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells[2].Value != null && row.Cells[2].Value.ToString().IndexOf('.')>0)
                {
                    var dataitem = row.Cells[2].Value.ToString().Split('.');
                    var table = dataitem[0].ToLower();
                    var col= dataitem[1].ToLower();
                    if (table.StartsWith("@"))table= table.Substring(1);
                    if (!OriSet.ContainsKey(table)) OriSet.Add(table, new List<string>());
                    OriSet[table].Add(col);
                }
            }
            rs.OriSet= OriSet;
            rs.ShowDialog();
            if (!rs.IsOk) return;
            if (rs.MainTable != null)
            {
                var delRows = new List<DataGridViewRow>();
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells[2].Value != null && row.Cells[2].Value.ToString().IndexOf('.') > 0)
                    {
                        var dataitem = row.Cells[2].Value.ToString().Split('.');
                        var table = dataitem[0];
                        if (table.StartsWith("@")) table = table.Substring(1);
                        if (table.ToLower() == rs.MainTable.ToLower())
                        {
                            bool isInOpSet = false;
                            foreach (string[] item in rs.OpSetList)
                            {
                                if(item[0].ToLower().Trim()== row.Cells[2].Value.ToString().Trim().ToLower())
                                {
                                    isInOpSet = true;
                                    break;
                                }
                            }
                            if (!isInOpSet) delRows.Add(row);
                        }
                    }
                }
                foreach(var row in delRows)
                {
                    dgv.Rows.Remove(row);
                }
            }
            foreach (string[] item in rs.OpSetList)
            {
                if(!HasSet(item[0]))
                {
                    dgv.Rows.Add(new object[] {
                    item[2],item[1],item[0],"",res.ctl.str("DyValueDefine.3"),res.ctl.str("DyValueDefine.1"),res.ctl.str("DyValueDefine.2"),"","Delete"
                    });
                }
            }
            bool HasSet(string binddata)
            {
                foreach(DataGridViewRow row in dgv.Rows)
                {
                    if(row.Cells[2].Value!=null && row.Cells[2].Value.ToString().ToLower().Trim()==binddata.ToLower())
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Cells[2].Value != null)
                {
                    string binddata = dgv.Rows[i].Cells[2].Value.ToString().Trim();
                    if(binddata.IndexOf('.')>0)
                    {
                        string col = binddata.Split('.')[1];
                        dgv.Rows[i].Cells[1].Value = col;
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            EleGenarate eg = new EleGenarate();
            eg.TopMost = true;
            eg.ShowDialog();
            if (!eg.isCancel)
            {
                string apdStr = eg.appendStr.Text.Trim();
                int eleType = eg.comboBox1.SelectedIndex;
                string template = eg.layoutHTML;
                string divId = eg.comboBox3.Text;
                var idCaps = new List<(string idname, string caption)>();
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    string col1 = dgv.Rows[i].Cells[1].Value?.ToString() ?? "";
                    if (apdStr != "")
                    {
                        if (col1 != "" && !col1.StartsWith(apdStr))
                        {
                            dgv.Rows[i].Cells[1].Value = apdStr + col1;
                        }
                    }
                    col1 = dgv.Rows[i].Cells[1].Value?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(col1))
                    {
                        idCaps.Add((col1, dgv.Rows[i].Cells[0].Value?.ToString() ?? ""));
                    }
                }
                EleGenarate.FormGenarate(template, eleType, partid, idCaps, divId);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            dgv.Rows.Clear();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string idname = textBox4.Text.Trim();
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Cells[3].Value = idname;
            }
        }

        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            RowColor();
        }
        private void RowColor()
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                bool col5Default = row.Cells[5].Value==null || (row.Cells[5].Value.ToString() == res.ctl.str("DyValueDefine.1"));
                bool col6Default = row.Cells[6].Value == null || (row.Cells[6].Value.ToString() == res.ctl.str("DyValueDefine.2"));
                Color color = Color.Black;
                if (!col5Default || !col6Default)
                {
                    color = Color.Blue;
                }
                row.DefaultCellStyle.ForeColor = color;
            }
        }

        #region DataGridView1 Drag Drop
        int selectionIdx = -1;
        private void dgv_DragEnter(object sender, DragEventArgs e)
        {
            dgv.EndEdit();
            e.Effect = DragDropEffects.Move;
        }

        private void dgv_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Clicks < 2) && (e.Button == MouseButtons.Left))

            {
                if ((e.ColumnIndex == -1) && (e.RowIndex > -1))

                    dgv.DoDragDrop(dgv.Rows[e.RowIndex], DragDropEffects.Move);

            }
        }
        private int GetRowFromPoint(int x, int y)

        {
            for (int i = 0; i < dgv.RowCount; i++)

            {
                Rectangle rec = dgv.GetRowDisplayRectangle(i, false);

                if (dgv.RectangleToScreen(rec).Contains(x, y))

                    return i;

            }
            return -1;

        }
        private void dgv_DragDrop(object sender, DragEventArgs e)
        {
            int idx = GetRowFromPoint(e.X, e.Y);

            if (idx < 0) return;

            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))

            {
                DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));

                dgv.Rows.Remove(row);

                selectionIdx = idx;

                dgv.Rows.Insert(idx, row);

            }
        }

        private void dgv_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (selectionIdx > -1)

            {
                dgv.Rows[selectionIdx].Selected = true;

                dgv.CurrentCell = dgv.Rows[selectionIdx].Cells[0];
                selectionIdx = -1;

            }
        }

        #endregion

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
