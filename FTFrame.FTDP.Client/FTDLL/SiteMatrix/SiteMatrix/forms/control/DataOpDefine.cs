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
using SiteMatrix.functions;
namespace SiteMatrix.forms
{
    public partial class DataOpDefine : Form
    {
        public static bool DataOpFormShow = false;
        public static DataOpDefine DataOpDefineForm = null;
        public string str;
        public bool IsCancel = true;
        public string partid=null;
        public DataOpDefine()
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
            dgv.EndEdit();
            ArrayList al = new ArrayList();
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                DataGridViewRow r = dgv.Rows[i];
                bool IsFidCol = r.Cells[2].Value != null && r.Cells[2].Value.ToString().ToLower().EndsWith(".fid");
                if (IsFidCol || (r.Cells[7].Value != null && !r.Cells[7].Value.ToString().Trim().Equals("")) || (r.Cells[1].Value != null && !r.Cells[1].Value.ToString().Trim().Equals("") && !al.Contains(r.Cells[1].Value.ToString().Trim())))
                {
                    //al.Add(r.Cells[1].Value.ToString().Trim());
                    if (!str.Equals("")) str += "|||";
                    str +=( r.Cells[0].Value == null ? "" : r.Cells[0].Value.ToString().Trim()) + "##"
                        + (r.Cells[1].Value == null ? "" : r.Cells[1].Value.ToString().Trim()) + "##"
                        + functions.str.getEncode(r.Cells[2].Value == null ? "" : r.Cells[2].Value.ToString().Trim()) + "##"
                        + functions.str.getEncode(LeiXingCode(r.Cells[3].Value.ToString().Trim())) + "##"
                        + CheckVal(r.Cells[4].Value.ToString().Trim()) + "##"
                        + (r.Cells[5].Value == null ? "" : r.Cells[5].Value.ToString().Trim()) + "##"
                        + (r.Cells[6].Value == null ? "" : r.Cells[6].Value.ToString().Trim()) + "##"
                        + functions.str.getEncode(r.Cells[7].Value == null ? "" : r.Cells[7].Value.ToString().Trim());
                }
            }
            al.Clear();
            al = null;
        }
        private void RowColor()
        {
            foreach(DataGridViewRow row in dgv.Rows)
            {
                string leixing = row.Cells[3].Value == null ? "" : row.Cells[3].Value.ToString();
                Color color = Color.Black;
                if (leixing == "Add" || leixing == "Mod")
                {
                    color = Color.Black;
                }
                else
                {
                    string bindtag = row.Cells[2].Value == null ? "" : row.Cells[2].Value.ToString();
                    if (bindtag.IndexOf('.') > 0 && bindtag.Split('.')[1].ToLower() == "fid")
                    {
                        color = Color.Red;
                    }
                    else
                    {
                        color = Color.Blue;
                        row.Cells[3].Value = "多行";
                    }
                }
                row.DefaultCellStyle.ForeColor = color;
            }
        }
        public void InitDefine()
        {
            dgv.Rows.Clear();
            string[] items=str.Split(new string[]{"|||"},StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items)
            {
                //cap#name#col#01#noempty#fid#id
                string[] row = item.Split(new string[] { "##" }, StringSplitOptions.None);
                dgv.Rows.Add(new object[]{
                row[0],
                row[1],
                functions.str.getDecode(row[2]),
                LeiXingText(functions.str.getDecode(row[3])),
                CheckText(row[4]),
                row[5],
                row[6],
                row.Length<8?"":functions.str.getDecode(row[7]),
                "Delete"
                });
            }
        }
        private void DataOpDefine_Resize(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = this.Height - 445+369;
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
            DataOpDefine_Resize(sender, e);
            comboBox1.SelectedIndex = 0;
            DataOpFormShow = true;
            DataOpDefineForm = this;
            dgv.Rows.Clear();
            DataGridViewTextBoxColumn caption = new DataGridViewTextBoxColumn();
            caption.HeaderText = "名称";
            caption.ToolTipText = "作为备注和必填提醒时使用";
            caption.Width = 80;
            DataGridViewTextBoxColumn name = new DataGridViewTextBoxColumn();
            name.HeaderText = "Name值";
            name.Width = 180;
            DataGridViewCellStyle dgvstyle_name = new DataGridViewCellStyle();
            dgvstyle_name.BackColor = Color.Yellow;
            name.DefaultCellStyle = dgvstyle_name;
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = "绑定字段";
            col.Width = 210;
            DataGridViewComboBoxColumn optype = new DataGridViewComboBoxColumn();
            optype.HeaderText = "类型";
            optype.Width = 105;
            optype.Items.Add("Add");
            optype.Items.Add("Mod");
            optype.Items.Add("多行");
            optype.Items.Add("多行新增");
            optype.Items.Add("多行重置");
            optype.Items.Add("多行重置保留FID");
            optype.Items.Add("多行仅更新");
            DataGridViewComboBoxColumn mustfill = new DataGridViewComboBoxColumn();
            mustfill.HeaderText = "验证";
            mustfill.Width = 160;
            mustfill.Items.Add("无");
            mustfill.Items.Add("为空值null");
            mustfill.Items.Add("不能为空");
            mustfill.Items.Add("整数");
            mustfill.Items.Add("数字");
            mustfill.Items.Add("日期");
            mustfill.Items.Add("日期：为空值null");
            mustfill.Items.Add("整数：可以为空");
            mustfill.Items.Add("整数：为空值 0");
            mustfill.Items.Add("整数：为空值-1");
            mustfill.Items.Add("整数：为空值null");
            mustfill.Items.Add("数字：可以为空");
            mustfill.Items.Add("数字：为空值 0");
            mustfill.Items.Add("数字：为空值-1");
            mustfill.Items.Add("数字：为空值null");
            DataGridViewTextBoxColumn fid = new DataGridViewTextBoxColumn();
            fid.HeaderText = "fid";
            fid.ToolTipText = "支持@语法，{para}为js代码，为空则默认为DefaultFid值";
            fid.Width = 80;
            DataGridViewTextBoxColumn id = new DataGridViewTextBoxColumn();
            id.HeaderText = "ID值";
            id.ToolTipText = "验证时必须";
            id.Width = 150;
            DataGridViewTextBoxColumn sql = new DataGridViewTextBoxColumn();
            sql.HeaderText = "高级";
            sql.ToolTipText = "";
            sql.Width = 170;
            DataGridViewButtonColumn del = new DataGridViewButtonColumn();
            del.HeaderText = "操作";
            del.Width = 80;

            dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            caption,
            name,
            col,
            optype,
            mustfill,
            fid,
            id,
            sql,
            del
            });
            dgv.ClipboardCopyMode=DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dgv.CellClick += new DataGridViewCellEventHandler(dgv_CellClick);
            dgv.CellDoubleClick += new DataGridViewCellEventHandler(dgv_CellDoubleClick);
            dgv.DefaultValuesNeeded += new DataGridViewRowEventHandler(dgv_DefaultValuesNeeded);

            InitDefine();
        }

        void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int CIndex = e.ColumnIndex;
                if (CIndex == 7)
                {
                    object val = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    TextEditor te = new TextEditor();
                    te.basetext = (val == null ? "" : val.ToString());
                    te.TopMost = true;
                    te.ShowDialog();
                    if (!te.cancel)
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = te.basetext;
                    }
                }
                else if (CIndex == 2)
                {
                    object val = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    string connstr = Options.GetSystemDBSetConnStr();
                    string dbtype = Options.GetSystemDBSetType();
                    if (connstr == null || connstr.Trim().Equals(""))
                    {
                        MsgBox.Warning("必须先在工具-》选项中配置数据库连接串");
                        return;
                    }
                    if (val == null || val.ToString().Trim().IndexOf(".") < 0)
                    {
                        if (dbtype.ToLower() == "mysql")
                        {
                            control.SelTable_MySql sel = new control.SelTable_MySql();
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
                            MsgBox.Warning(dbtype + " 该数据库类型还没完成便捷性开发，请手动输入");
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
                        object nameVal = dgv.Rows[e.RowIndex].Cells[1].Value;
                        string tablename = val.ToString().Trim();
                        tablename = tablename.Substring(0, tablename.IndexOf('.'));
                        string otablename=tablename;
                        if (tablename.StartsWith("@")) tablename = tablename.Substring(1);
                        else
                        {
                            tablename = "ft_"+consts.globalConst.CurSite.ID+"_f_" + tablename;
                        }
                        if (dbtype.ToLower() == "sqlserver")
                        {
                            control.SelCol_SqlServer sel = new control.SelCol_SqlServer();
                            sel.connstr = connstr;
                            sel.tablename = tablename;
                            sel.colname = null;
                            sel.nameval = nameVal == null ? null : nameVal.ToString();
                            sel.ShowDialog();
                            if (sel.colname != null)
                            {
                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = otablename + "." + sel.colname;
                                dgv.EndEdit();
                            }
                        }
                        else if (dbtype.ToLower() == "mysql")
                        {
                            control.SelCol_MySql sel = new control.SelCol_MySql();
                            sel.connstr = connstr;
                            sel.tablename = tablename;
                            sel.colname = null;
                            sel.nameval = nameVal == null ? null : nameVal.ToString();
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
            e.Row.Cells[3].Value = "Add";
            e.Row.Cells[4].Value = "无";
            e.Row.Cells[5].Value = "";
            e.Row.Cells[6].Value = "";
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
            System.Data.OleDb.OleDbDataReader rdr = consts.globalConst.CurSite.SiteConn.OpenRecord(sql);
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
            DataOpFormShow = false;
            DataOpDefineForm = null;
        }

        public static void EleDyAdd(IHTMLElement ele)
        {
            string name=Adapter.Property.PropertyAdapter.getEleAttr(ele, "name");
            if (!name.Equals(""))
            {
                string type=Adapter.Property.PropertyAdapter.getEleAttr(ele, "type");
                if (functions.str.IsFormEleTag(ele.tagName, type))
                {
                    string id = Adapter.Property.PropertyAdapter.getEleAttr(ele, "id");
                    bool ishave = false;
                    for (int i = 0; i < DataOpDefineForm.dgv.Rows.Count; i++)
                    {
                        DataGridViewRow r = DataOpDefineForm.dgv.Rows[i];
                        if (r.Cells[1].Value != null && r.Cells[1].Value.ToString().Equals(name))
                        {
                            ishave = true;
                            break;
                        }
                    }
                    if (!ishave)
                    {
                        if (DataOpDefineForm.dgv.Rows.Count > 1)
                        {
                            DataGridViewRow row = DataOpDefineForm.dgv.Rows[DataOpDefineForm.dgv.Rows.Count - 2];
                            string bindcol = row.Cells[2].Value == null ? "" : row.Cells[2].Value.ToString();
                            if (bindcol.IndexOf(".") >= 0)
                            {
                                bindcol = bindcol.Substring(0, bindcol.IndexOf(".") + 1);
                            }
                            DataOpDefineForm.dgv.Rows.Add(new object[]{
                            "",
                            name,
                            bindcol,
                            row.Cells[3].Value == null ? "" : row.Cells[3].Value.ToString(),
                            "无",
                            "",
                                id,
                                "",
                            "Delete"
                            });
                        }
                        else
                        {
                                    DataOpDefineForm.dgv.Rows.Add(new object[]{
                                "",
                                name,
                                "",
                                "Add",
                                "无",
                                "",
                                id,
                                "",
                                "Delete"
                                });
                        }
                    }
                }
            }
        }
        private string LeiXingCode(string val)
        {
            switch (val)
            {
                case "Add": return "0";
                case "Mod": return "1";
                case "多行": return "2";
                case "多行新增": return "3";
                case "多行重置": return "4";
                case "多行重置保留FID": return "5";
                case "多行仅更新": return "6";
            }
            return null;
        }
        private string LeiXingText(string val)
        {
            switch (val)
            {
                case "0": return "Add";
                case "1": return "Mod";
                case "2": return "多行";
                case "3": return "多行新增";
                case "4": return "多行重置";
                case "5": return "多行重置保留FID";
                case "6": return "多行仅更新";
            }
            return null;
        }
        private string CheckText(string val)
        {
            switch (val)
            {
                case "": return "无";
                case "null_nocheck": return "为空值null"; 
                case "noempty": return "不能为空"; 
                case "int": return "整数"; 
                case "decimal": return "数字"; 
                case "date": return "日期";
                case "null_date": return "日期：为空值null"; 
                case "int_empty": return "整数：可以为空";
                case "int_0": return "整数：为空值 0";
                case "int_1": return "整数：为空值-1";
                case "null_int": return "整数：为空值null"; 
                case "decimal_empty": return "数字：可以为空";
                case "decimal_0": return "数字：为空值 0";
                case "decimal_1": return "数字：为空值-1";
                case "null_decimal": return "数字：为空值null"; 
            }
            return null;
        }
        private string CheckVal(string val)
        {
            switch (val)
            {
                case "无": return "";
                case "为空值null": return "null_nocheck";
                case "不能为空": return "noempty";
                case "整数": return "int";
                case "数字": return "decimal";
                case "日期": return "date";
                case "日期：为空值null": return "null_date";
                case "整数：可以为空": return "int_empty";
                case "整数：为空值 0": return "int_0";
                case "整数：为空值-1": return "int_1";
                case "整数：为空值null": return "null_int";
                case "数字：可以为空": return "decimal_empty";
                case "数字：为空值 0": return "decimal_0";
                case "数字：为空值-1": return "decimal_1";
                case "数字：为空值null": return "null_decimal";
            }
            return null;
        }
        public void DataGirdViewCellPaste()
        {
            try
            {
                // 获取剪切板的内容，并按行分割
                string pasteText = Clipboard.GetText();
                if (string.IsNullOrEmpty(pasteText))
                    return;
                string[] lines = pasteText.Split(new string[]{"\n"},StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line.Trim()))
                        continue;
                    string[] vals = line.Split(new string[] { "	" }, StringSplitOptions.None);
                    dgv.Rows.Add(new object[]{
                vals[0],
                vals[1],
                vals[2],
                vals[3],
                vals[4],
                vals[5],
                vals[6],
                vals[7],
                vals[8]
                });
                }
            }
            catch (Exception ex)
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
            if (comboBox1.SelectedIndex >0 )
            {
                val = comboBox1.SelectedItem.ToString();
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    DataGridViewRow r = dgv.Rows[i];
                   // if (r.Cells[1].Value != null && !r.Cells[1].Value.ToString().Trim().Equals(""))
                   // {
                        r.Cells[3].Value = val;
                    //}
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

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Cells[1].Value != null)
                {
                    dgv.Rows[i].Cells[6].Value = dgv.Rows[i].Cells[1].Value;
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
                    if (oldval.IndexOf('.')<0) newval = tablename + "."+oldval;
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

                                        IHTMLElementCollection eles = ed.editocx.getElementsByName(dgv.Rows[e.RowIndex].Cells[1].Value.ToString());
                                        foreach (IHTMLElement ele in eles)
                                        {
                                            tip += ele.parentElement.parentElement.outerText;
                                        }

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

        private void Button7_Click(object sender, EventArgs e)
        {
            functions.sheel.ExeSheel("http://www.ftframe.com/doc/dataop.mht");
        }

        private void DataOpDefine_TextChanged(object sender, EventArgs e)
        {
        }

        private void Dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            RowColor();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            string idname = textBox4.Text.Trim();
            if (idname.Equals("")) return;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Cells[5].Value = idname;
            }
        }
    }
}
