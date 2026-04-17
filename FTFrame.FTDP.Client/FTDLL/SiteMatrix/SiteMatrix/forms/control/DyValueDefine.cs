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
    public partial class DyValueDefine : Form
    {
        public static bool DyValueFormShow = false;
        public static DyValueDefine DyValueDefineForm = null;
        public string str;
        public bool IsCancel = true;
        public string partid=null;
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
                        + (r.Cells[4].Value.ToString().Trim().Equals("同步") ? "0" : "1") + "##"
                        + (r.Cells[5].Value.ToString().Trim().Equals("单行取值") ? "0" : "1") + "##"
                        + (r.Cells[6].Value.ToString().Trim().Equals("赋值") ? "0" : "1") + "##"
                        + functions.str.getEncode(r.Cells[7].Value == null ? "" : r.Cells[7].Value.ToString().Trim());
                }
            }
            al.Clear();
            al = null;
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
                row[4].Equals("0")?"同步":"异步",
                row[5].Equals("0")?"单行取值":"动态新增",
                row[6].Equals("0")?"赋值":"维表",
                functions.str.getDecode(row[7]),
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
            comboBox2.SelectedIndex = 0;
            DyValueFormShow = true;
            DyValueDefineForm = this;
            dgv.Rows.Clear();
            DataGridViewTextBoxColumn caption = new DataGridViewTextBoxColumn();
            caption.HeaderText = "名称";
            caption.ToolTipText = "作为备注和必填提醒时使用";
            caption.Width = 80;
            DataGridViewTextBoxColumn name = new DataGridViewTextBoxColumn();
            name.HeaderText = "Id值";
            caption.ToolTipText = "用,隔开表示多个元素赋值";
            name.Width = 210;
            DataGridViewCellStyle dgvstyle_name = new DataGridViewCellStyle();
            dgvstyle_name.BackColor = Color.Yellow;
            name.DefaultCellStyle = dgvstyle_name;
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = "绑定字段";
            col.Width = 210;
            DataGridViewTextBoxColumn fid = new DataGridViewTextBoxColumn();
            fid.HeaderText = "fid";
            fid.ToolTipText = "支持@语法，{para}为js代码，支持@p1@到@p12@赋值，为空则默认为DefaultFid值";
            fid.Width = 120;
            DataGridViewComboBoxColumn dytype = new DataGridViewComboBoxColumn();
            dytype.HeaderText = "方式";
            dytype.Width = 80;
            dytype.Items.Add("同步");
            dytype.Items.Add("异步");
            DataGridViewComboBoxColumn isdy = new DataGridViewComboBoxColumn();
            isdy.HeaderText = "取值";
            isdy.Width = 120;
            isdy.Items.Add("单行取值");
            isdy.Items.Add("动态新增");
            DataGridViewComboBoxColumn isdim = new DataGridViewComboBoxColumn();
            isdim.HeaderText = "赋值";
            isdim.Width = 80;
            isdim.Items.Add("赋值");
            isdim.Items.Add("维表");
            DataGridViewTextBoxColumn sql = new DataGridViewTextBoxColumn();
            sql.HeaderText = "高级";
            sql.ToolTipText = "为空不生效，@code为code接口，sql@code为code接口sql";
            sql.Width = 185;
            DataGridViewButtonColumn del = new DataGridViewButtonColumn();
            del.HeaderText = "操作";
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

            InitDefine();
        }

        void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int CIndex = e.ColumnIndex;
                if (CIndex == 1)
                {
                    object val = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    TextEditor te = new TextEditor();
                    te.basetext = (val == null ? "" : val.ToString());
                    te.TopMost = true;
                    te.ShowDialog();
                    if(!te.cancel)
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = te.basetext;
                    }
                }
                 else   if (CIndex == 2)
                {
                    object val = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    string connstr = Options.GetSystemDBSetConnStr();
                    string dbtype = Options.GetSystemDBSetType();
                    if(connstr == null || connstr.Trim().Equals(""))
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
                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "@"+ sel.tablename +".";
                                dgv.EndEdit();
                            }
                        }
                        else
                        {
                            MsgBox.Warning(dbtype+" 该数据库类型还没完成便捷性开发，请手动输入");
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
                        if (dbtype.ToLower() == "sqlserver")
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
                        else if (dbtype.ToLower() == "mysql")
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
            e.Row.Cells[4].Value = "同步";
            e.Row.Cells[5].Value = "单行取值";
            e.Row.Cells[6].Value = "赋值";
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
                    fd.ShowDialog();
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
                            "赋值",
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
                            "异步",
                            "单行取值",
                            "赋值",
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
                string[] lines = pasteText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line.Trim()))
                        continue;
                    string[] vals = line.Split(new string[] { "	" }, StringSplitOptions.None);
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
    }
}
