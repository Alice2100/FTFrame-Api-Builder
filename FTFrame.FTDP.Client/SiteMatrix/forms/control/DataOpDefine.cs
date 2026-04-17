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
    public partial class DataOpDefine : Form
    {
        public static bool DataOpFormShow = false;
        public static DataOpDefine DataOpDefineForm = null;
        public string str;
        public List<string[]> nameCapList;
        public bool IsCancel = true;
        public string partid = null;
        string FidCol2 = null;
        public System.Xml.XmlDocument XmlDoc;
        public Tuple<IHTMLElement, string  ,string , string ,int > EditorTuple=null;
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
        private bool ValidateRow()
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                string leixing = row.Cells[3].Value?.ToString() ?? "";
                if(leixing!="")
                {
                    int lx = int.Parse(LeiXingCode(leixing));
                    if(lx>=7 && lx<=12)
                    {
                        if((row.Cells[1].Value?.ToString() ?? "").Trim()=="")
                        {
                            MsgBox.Error(res.ctl.str("DataOPDefine.1"));
                            return false;
                        }
                        if ((row.Cells[2].Value?.ToString() ?? "").Trim() == ""|| (row.Cells[2].Value?.ToString() ?? "").Split(new string[] { "."}, StringSplitOptions.RemoveEmptyEntries).Length>1)
                        {
                            MsgBox.Error(res.ctl.str("DataOPDefine.2"));
                            return false;
                        }
                        if (row.Cells[2].Value.ToString().Trim().EndsWith(".")) row.Cells[2].Value = row.Cells[2].Value.ToString().Trim().Substring(0, row.Cells[2].Value.ToString().Trim().Length - 1);
                    }
                    if(lx==8||lx==11)
                    {
                        if ((row.Cells[7].Value?.ToString() ?? "").Trim() == "")
                        {
                            MsgBox.Error(res.ctl.str("DataOPDefine.3"));
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        private string JsonGet(DataGridViewRow row)
        {
            string leixing = row.Cells[3].Value?.ToString() ?? "";
            if (leixing != "")
            {
                int lx = int.Parse(LeiXingCode(leixing));
                if (lx >= 7 && lx <= 12)
                {
                    if (row.Tag != null)
                    {
                        return ((object[])row.Tag)[0].ToString().Replace("\r","").Replace("\n", "").Replace("\r\n", "").Replace(" ", "").Replace("  ", "").Replace("##", "$#$#$");
                    }
                }
            }
            return "[]";
        }
        private void JsonSet(DataGridViewRow row,string jsonstr)
        {
            string leixing = row.Cells[3].Value?.ToString() ?? "";
            if (leixing != "")
            {
                int lx = int.Parse(LeiXingCode(leixing));
                if (lx >= 7 && lx <= 12)
                {
                    row.Tag = new object[] { jsonstr.Replace("$#$#$", "##") };
                }
            }
        }
        public void EndDefine()
        {
            str = "";
            nameCapList = new List<string[]>();
            dgv.EndEdit();
            ArrayList al = new ArrayList();
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                DataGridViewRow r = dgv.Rows[i];
                bool IsFidCol = r.Cells[2].Value != null && r.Cells[2].Value.ToString().ToLower().EndsWith("."+ FidCol2.ToLower());
                if (IsFidCol || (r.Cells[7].Value != null && !r.Cells[7].Value.ToString().Trim().Equals("")) || (r.Cells[1].Value != null && !r.Cells[1].Value.ToString().Trim().Equals("") && !al.Contains(r.Cells[1].Value.ToString().Trim())))
                {
                    //al.Add(r.Cells[1].Value.ToString().Trim());
                    if (!str.Equals("")) str += "|||";
                    str += (r.Cells[0].Value == null ? "" : r.Cells[0].Value.ToString().Trim()) + "##"
                        + (r.Cells[1].Value == null ? "" : r.Cells[1].Value.ToString().Trim()) + "##"
                        + functions.str.getEncode(r.Cells[2].Value == null ? "" : r.Cells[2].Value.ToString().Trim()) + "##"
                        + functions.str.getEncode(LeiXingCode(r.Cells[3].Value.ToString().Trim())) + "##"
                        + CheckVal(r.Cells[4].Value.ToString().Trim()) + "##"
                        + (r.Cells[5].Value == null ? "" : r.Cells[5].Value.ToString().Trim()) + "##"
                        + (r.Cells[6].Value == null ? "" : r.Cells[6].Value.ToString().Trim()) + "##"
                        + functions.str.getEncode(r.Cells[7].Value == null ? "" : r.Cells[7].Value.ToString().Trim())
                        + "##" + JsonGet(r);
                    nameCapList.Add(new string[] {
                       (r.Cells[1].Value??"").ToString().Trim(),
                    (r.Cells[0].Value == null ? "" : r.Cells[0].Value.ToString().Trim())
                    });
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
                bool isIgnore = row.Cells[1].Value != null && row.Cells[1].Value.ToString().StartsWith("_");
                if (leixing == "Add" || leixing == "Mod")
                {
                    color = isIgnore?Color.Gray:Color.Black;
                }
                else if (leixing == "Auto")
                {
                    color = isIgnore ? Color.Gray : Color.SeaGreen;
                }
                else
                {

                    if (leixing.StartsWith(res.ctl.str("DataOPDefine.4")))
                    {
                        color = Color.Fuchsia;
                    }
                    else
                    {
                        string bindtag = row.Cells[2].Value == null ? "" : row.Cells[2].Value.ToString();
                        if (bindtag.IndexOf('.') > 0 && bindtag.Split('.')[1].ToLower() == FidCol2.ToLower())
                        {
                            color = Color.Red;
                        }
                        else
                        {
                            color = Color.Blue;
                            row.Cells[3].Value = res.ctl.str("DataOPDefine.5");
                        }
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
                int index= dgv.Rows.Add(new object[]{
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
                if(row.Length>=9)
                {
                    JsonSet(dgv.Rows[index], row[8]);
                }
            }
        }
        private void DataOpDefine_Resize(object sender, EventArgs e)
        {
            if (this.Height - 445 + 369 >= Math.Max(splitContainer1.Panel1MinSize, Width - splitContainer1.Panel1MinSize) || this.Height - 445 + 369 <= Math.Min(splitContainer1.Panel1MinSize, Width - splitContainer1.Panel1MinSize))
                return;
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
            #region Language
            this.Text = res.ctl.str("DataOPDefine.text");           //数据操作 设置
            button1.Text = res.ctl.str("DataOPDefine.button1");         //确定(&O)
            button3.Text = res.ctl.str("DataOPDefine.button3");         //粘贴(&P)
            button6.Text = res.ctl.str("DataOPDefine.button6");         //统一表名
            label1.Text = res.ctl.str("DataOPDefine.label1");           //第
            label2.Text = res.ctl.str("DataOPDefine.label2");           //列
            button4.Text = res.ctl.str("DataOPDefine.button4");         //替换
            button2.Text = res.ctl.str("DataOPDefine.button2");         //取消(&C)
            button11.Text = res.ctl.str("DataOPDefine.button11");           //Name=列名
            button9.Text = res.ctl.str("DataOPDefine.button9");         //API定义
            button10.Text = res.ctl.str("DataOPDefine.button10");           //快速定义
            button7.Text = res.ctl.str("DataOPDefine.button7");			//帮助文档
            button12.Text = res.ctl.str("DyValueDefine.button12");
            #endregion
            DataOpDefine_Resize(sender, e);
            this.Width = button1.Location.X + 110;
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new string[] { res.ctl.GetString("DataOPDefine.10"), "Add", "Mod", "Auto" });

            comboBox1.SelectedIndex = 0;
            DataOpFormShow = true;
            DataOpDefineForm = this;
            dgv.Rows.Clear();
            DataGridViewTextBoxColumn caption = new DataGridViewTextBoxColumn();
            caption.HeaderText = res.ctl.str("DataOPDefine.6");
            caption.ToolTipText = res.ctl.str("DataOPDefine.7");
            caption.Width = 140;
            DataGridViewTextBoxColumn name = new DataGridViewTextBoxColumn();
            name.HeaderText = res.ctl.str("DataOPDefine.8");
            name.Width = 180;
            DataGridViewCellStyle dgvstyle_name = new DataGridViewCellStyle();
            dgvstyle_name.BackColor = Color.Yellow;
            name.DefaultCellStyle = dgvstyle_name;
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = res.ctl.str("DataOPDefine.9");
            col.Width = 210;
            DataGridViewComboBoxColumn optype = new DataGridViewComboBoxColumn();
            optype.HeaderText = res.ctl.str("DataOPDefine.10");
            optype.Width = 183;
            optype.Items.Add("Add");
            optype.Items.Add("Mod");
            optype.Items.Add(res.ctl.str("DataOPDefine.5"));
            optype.Items.Add(res.ctl.str("DataOPDefine.5")+" "+ res.ctl.str("DataOPDefine.11"));
            optype.Items.Add(res.ctl.str("DataOPDefine.5")+ " " + res.ctl.str("DataOPDefine.12"));
            optype.Items.Add(res.ctl.str("DataOPDefine.5")+ " " + res.ctl.str("DataOPDefine.13"));
            optype.Items.Add(res.ctl.str("DataOPDefine.5")+ " " + res.ctl.str("DataOPDefine.14"));
            optype.Items.Add(res.ctl.str("DataOPDefine.4")+ " " + res.ctl.str("DataOPDefine.15"));
            optype.Items.Add(res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.16"));
            optype.Items.Add(res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.17"));
            optype.Items.Add(res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.18"));
            optype.Items.Add(res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.19"));
            optype.Items.Add(res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.20"));
            optype.Items.Add("Auto");
            DataGridViewComboBoxColumn mustfill = new DataGridViewComboBoxColumn();
            mustfill.HeaderText = res.ctl.str("DataOPDefine.21");
            mustfill.Width = 185;
            mustfill.Items.Add(res.ctl.str("DataOPDefine.22"));
            mustfill.Items.Add(res.ctl.str("DataOPDefine.23"));
            mustfill.Items.Add(res.ctl.str("DataOPDefine.24"));
            mustfill.Items.Add(res.ctl.str("DataOPDefine.25"));
            mustfill.Items.Add(res.ctl.str("DataOPDefine.26"));
            mustfill.Items.Add(res.ctl.str("DataOPDefine.27"));
            mustfill.Items.Add(res.ctl.str("DataOPDefine.28"));
            mustfill.Items.Add(res.ctl.str("DataOPDefine.29"));
            mustfill.Items.Add(res.ctl.str("DataOPDefine.30"));
            mustfill.Items.Add(res.ctl.str("DataOPDefine.31"));
            mustfill.Items.Add(res.ctl.str("DataOPDefine.32"));
            mustfill.Items.Add(res.ctl.str("DataOPDefine.33"));
            mustfill.Items.Add(res.ctl.str("DataOPDefine.34"));
            mustfill.Items.Add(res.ctl.str("DataOPDefine.35"));
            mustfill.Items.Add(res.ctl.str("DataOPDefine.36"));
            DataGridViewTextBoxColumn fid = new DataGridViewTextBoxColumn();
            fid.HeaderText = res.ctl.str("DataOPDefine.37");
            fid.ToolTipText = res.ctl.str("DataOPDefine.38");
            fid.Width = 130;
            DataGridViewTextBoxColumn id = new DataGridViewTextBoxColumn();
            id.HeaderText = res.ctl.str("DataOPDefine.39");
            id.ToolTipText = res.ctl.str("DataOPDefine.40");
            id.Width = 150;
            DataGridViewTextBoxColumn sql = new DataGridViewTextBoxColumn();
            sql.HeaderText = res.ctl.str("DataOPDefine.41");
            sql.ToolTipText = "";
            sql.Width = 260;
            DataGridViewButtonColumn del = new DataGridViewButtonColumn();
            del.HeaderText = res.ctl.str("DataOPDefine.42");
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
            foreach(DataGridViewColumn _col in dgv.Columns)
            {
                _col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dgv.ClipboardCopyMode=DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dgv.CellClick += new DataGridViewCellEventHandler(dgv_CellClick);
            dgv.CellDoubleClick += new DataGridViewCellEventHandler(dgv_CellDoubleClick);
            dgv.DefaultValuesNeeded += new DataGridViewRowEventHandler(dgv_DefaultValuesNeeded);

            string FidCol = Page.PageWare.getPartParamValue(partid,"dataop", "Interface", "FidCol");
            if (string.IsNullOrEmpty(FidCol) || FidCol.IndexOf(',') < 0) FidCol2 = "fid";
            else FidCol2 = FidCol.Split(',')[1].Trim();
            InitDefine();
        }

        void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int CIndex = e.ColumnIndex;
                if (CIndex == 5 || CIndex == 6 || CIndex == 7)
                {
                    object val = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    TextEditor te = new TextEditor();
                    te.basetext = (val == null ? "" : val.ToString());
                    this.TopMost = false;
                    if (CIndex == 7) te.fromWhere = "dataop";
                    else if (CIndex == 5) te.fromWhere = "filterrule";
                    te.ShowInTaskbar = true;
                    te.TopMost = true;
                    te.ShowDialog();
                    this.TopMost=true;
                    if (!te.cancel)
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = te.basetext;
                    }
                }
                else if (CIndex == 2)
                {
                    object val = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    string connstr = Options.GetSystemDBSetConnStr();
                    var dbtype = Options.GetSystemDBSetType();
                    if (connstr == null || connstr.Trim().Equals(""))
                    {
                        MsgBox.Warning(res.ctl.str("DataOPDefine.43"));
                        return;
                    }
                    //functions.MsgBox.Information("!!"+dbtype.ToLower()+"!!");
                    if (val == null || val.ToString().Trim().IndexOf(".") < 0)
                    {
                        if (dbtype == globalConst.DBType.MySql)
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
                            MsgBox.Warning(dbtype + " "+ res.ctl.str("DataOPDefine.44"));
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
                        if (dbtype == globalConst.DBType.SqlServer)
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
                        else if (dbtype == globalConst.DBType.MySql)
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
                        else if (dbtype == globalConst.DBType.Sqlite)
                        {
                            control.SelCol_Sqlite sel = new control.SelCol_Sqlite();
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
                else if(CIndex==0 || CIndex==1)
                {
                    string leixing =dgv.Rows[e.RowIndex].Cells[3].Value?.ToString() ?? "";
                    if (leixing != "")
                    {
                        int lx = int.Parse(LeiXingCode(leixing));
                        if (lx >= 7 && lx <= 12)
                        {
                            if ((dgv.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "").Trim() == "")
                            {
                                MsgBox.Error(res.ctl.str("DataOPDefine.45"));
                                return ;
                            }
                            if ((dgv.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "").Trim() == "" || (dgv.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "").Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
                            {
                                MsgBox.Error(res.ctl.str("DataOPDefine.46"));
                                return ;
                            }
                            if (dgv.Rows[e.RowIndex].Cells[2].Value.ToString().Trim().EndsWith(".")) dgv.Rows[e.RowIndex].Cells[2].Value = dgv.Rows[e.RowIndex].Cells[2].Value.ToString().Trim().Substring(0, dgv.Rows[e.RowIndex].Cells[2].Value.ToString().Trim().Length - 1);

                            DataOpDefineSon opson = new DataOpDefineSon();
                            opson.MainTable= dgv.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "";
                            opson.JsonString = JsonGet(dgv.Rows[e.RowIndex]).Replace("$#$#$", "##");
                            //MsgBox.Information(opson.JsonString);
                            opson.ParantName= dgv.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";
                            opson.ShowDialog();
                            if(!opson.IsCancel)
                            {
                                JsonSet(dgv.Rows[e.RowIndex], opson.str);
                                //MsgBox.Information(opson.str);
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
            e.Row.Cells[4].Value = res.ctl.str("DataOPDefine.22");
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
            if (!ValidateRow()) return;
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

                if(EditorTuple!=null)
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
                            res.ctl.str("DataOPDefine.22"),
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
                                res.ctl.str("DataOPDefine.22"),
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
                case "Auto": return "13";
                default:
                    if (val == res.ctl.str("DataOPDefine.5")) return "2";
                    else if (val == (res.ctl.str("DataOPDefine.5") + " " + res.ctl.str("DataOPDefine.11"))) return "3";
                    else if (val == (res.ctl.str("DataOPDefine.5") + " " + res.ctl.str("DataOPDefine.12"))) return "4";
                    else if (val == (res.ctl.str("DataOPDefine.5") + " " + res.ctl.str("DataOPDefine.13"))) return "5";
                    else if (val == (res.ctl.str("DataOPDefine.5") + " " + res.ctl.str("DataOPDefine.14"))) return "6";
                    else if (val == (res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.15"))) return "7";
                    else if (val == (res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.16"))) return "8";
                    else if (val == (res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.17"))) return "9";
                    else if (val == (res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.18"))) return "10";
                    else if (val == (res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.19"))) return "11";
                    else if (val == (res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.20"))) return "12";
                    break;
            }
            return null;
        }
        private string LeiXingText(string val)
        {
            switch (val)
            {
                case "0": return "Add";
                case "1": return "Mod";
                case "2": return res.ctl.str("DataOPDefine.5");
                case "3": return res.ctl.str("DataOPDefine.5") + " " + res.ctl.str("DataOPDefine.11");
                case "4": return res.ctl.str("DataOPDefine.5") + " " + res.ctl.str("DataOPDefine.12");
                case "5": return res.ctl.str("DataOPDefine.5") + " " + res.ctl.str("DataOPDefine.13");
                case "6": return res.ctl.str("DataOPDefine.5") + " " + res.ctl.str("DataOPDefine.14");
                case "7": return res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.15");
                case "8": return res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.16");
                case "9": return res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.17");
                case "10": return res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.18");
                case "11": return res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.19");
                case "12": return res.ctl.str("DataOPDefine.4") + " " + res.ctl.str("DataOPDefine.20");
                case "13": return "Auto";
            }
            return null;
        }
        private string CheckText(string val)
        {
            switch (val)
            {
                case "": return res.ctl.str("DataOPDefine.22");
                case "null_nocheck": return res.ctl.str("DataOPDefine.23"); 
                case "noempty": return res.ctl.str("DataOPDefine.24"); 
                case "int": return res.ctl.str("DataOPDefine.25"); 
                case "decimal": return res.ctl.str("DataOPDefine.26"); 
                case "date": return res.ctl.str("DataOPDefine.27");
                case "null_date": return res.ctl.str("DataOPDefine.28"); 
                case "int_empty": return res.ctl.str("DataOPDefine.29");
                case "int_0": return res.ctl.str("DataOPDefine.30");
                case "int_1": return res.ctl.str("DataOPDefine.31");
                case "null_int": return res.ctl.str("DataOPDefine.32"); 
                case "decimal_empty": return res.ctl.str("DataOPDefine.33");
                case "decimal_0": return res.ctl.str("DataOPDefine.34");
                case "decimal_1": return res.ctl.str("DataOPDefine.35");
                case "null_decimal": return res.ctl.str("DataOPDefine.36"); 
            }
            return null;
        }
        private string CheckVal(string val)
        {
            if(val== res.ctl.str("DataOPDefine.22")) return "";
            else if (val == res.ctl.str("DataOPDefine.23")) return "null_nocheck";
            else if (val == res.ctl.str("DataOPDefine.24")) return "noempty";
            else if (val == res.ctl.str("DataOPDefine.25")) return "int";
            else if (val == res.ctl.str("DataOPDefine.26")) return "decimal";
            else if (val == res.ctl.str("DataOPDefine.27")) return "date";
            else if (val == res.ctl.str("DataOPDefine.28")) return "null_date";
            else if (val == res.ctl.str("DataOPDefine.29")) return "int_empty";
            else if (val == res.ctl.str("DataOPDefine.30")) return "int_0";
            else if (val == res.ctl.str("DataOPDefine.31")) return "int_1";
            else if (val == res.ctl.str("DataOPDefine.32")) return "null_int";
            else if (val == res.ctl.str("DataOPDefine.33")) return "decimal_empty";
            else if (val == res.ctl.str("DataOPDefine.34")) return "decimal_0";
            else if (val == res.ctl.str("DataOPDefine.35")) return "decimal_1";
            else if (val == res.ctl.str("DataOPDefine.36")) return "null_decimal";
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
                string[] lines = pasteText.Split(new string[]{ "Delete\r\n","Delete\n", "Delete\r" },StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line.Trim()))
                        continue;
                    string[] vals = line.Split(new string[] { "	" }, StringSplitOptions.None);
                    string[] val2 = new string[9];
                    for (var i = 0; i <= 6;i++) val2[i] = vals[i];
                    val2[7] = "";
                    for (var i=7;i< vals.Length;i++)
                    {
                        if (i == vals.Length - 1 && vals[i] == "Delete") break;
                        if (i > 7) val2[7] += "	";
                        val2[7] += vals[i];
                    }
                    val2[8] = "Delete";
                    dgv.Rows.Add(val2);
                //    dgv.Rows.Add(new object[]{
                //vals[0],
                //vals[1],
                //vals[2],
                //vals[3],
                //vals[4],
                //vals[5],
                //vals[6],
                //vals[7],
                //vals[8]
                //});
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
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Cells[5].Value = idname;
            }
        }
        public static List<string[]> IdNameList(string partid)
        {
            string define = Page.PageWare.getPartParamValue(partid, "dataop", "Interface", "Define");
            List<string[]> list = new List<string[]>();
            string[] items = define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items)
            {
                string[] row = item.Split(new string[] { "##" }, StringSplitOptions.None);
                list.Add(new string[] { row[1], row[0] });
            }
            return list;
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
                    DataOP_Api list_Api = new DataOP_Api();
                    list_Api.restr = apiset;
                    list_Api.nameCapList = nameCapList;
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
                string apiset = Page.PageWare.getPartParamValue(partid, "dataop", "Interface", "APISet");
                if (apiset == null) functions.MsgBox.Error("api set error");
                else
                {
                    EndDefine();
                    DataOP_Api list_Api = new DataOP_Api();
                    list_Api.restr = apiset;
                    list_Api.nameCapList = nameCapList;
                    list_Api.TopMost = true;
                    list_Api.ShowDialog();
                    if (!list_Api.IsCancel)
                    {
                        Page.PageWare.setPartParamValue(partid, "dataop", "Interface", "APISet", list_Api.restr);
                    }
                }
            }
            PropertySpace.Site.PropertyPart.doPartProperty(partid);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            RowAll_SetCols rs = new RowAll_SetCols();
            rs.OpType = "DataOp";
            var OriSet = new Dictionary<string, List<string>>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells[2].Value != null && row.Cells[2].Value.ToString().IndexOf('.') > 0)
                {
                    var dataitem = row.Cells[2].Value.ToString().Split('.');
                    var table = dataitem[0].ToLower();
                    var col = dataitem[1].ToLower();
                    if (table.StartsWith("@")) table = table.Substring(1);
                    if (!OriSet.ContainsKey(table)) OriSet.Add(table, new List<string>());
                    OriSet[table].Add(col);
                }
            }
            rs.OriSet = OriSet;
            rs.ShowDialog();
            if (!rs.IsOk) return;
            var lastSimpleOpType = "";
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
                                if (item[0].ToLower().Trim() == row.Cells[2].Value.ToString().Trim().ToLower())
                                {
                                    isInOpSet = true;
                                    break;
                                }
                            }
                            if (!isInOpSet) delRows.Add(row);
                        }
                    }
                    if(row.Cells[3].Value!=null)
                    {
                        if("Add|Mod|Auto".IndexOf(row.Cells[3].Value.ToString())>=0)
                        {
                            lastSimpleOpType = row.Cells[3].Value.ToString();
                        }
                    }
                }
                foreach (var row in delRows)
                {
                    dgv.Rows.Remove(row);
                }
            }
            foreach (string[] item in rs.OpSetList)
            {
                if (!HasSet(item[0]))
                {
                    dgv.Rows.Add(new object[] {
                   item[2],item[1],item[0],lastSimpleOpType==""?"Add":lastSimpleOpType,res.ctl.str("DataOPDefine.22"),"",item[1],"","Delete"
                    });
                }
            }
            bool HasSet(string binddata)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells[2].Value != null && row.Cells[2].Value.ToString().ToLower().Trim() == binddata.ToLower())
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Cells[2].Value != null)
                {
                    string binddata = dgv.Rows[i].Cells[2].Value.ToString().Trim();
                    if (binddata.IndexOf('.') > 0)
                    {
                        string col = binddata.Split('.')[1];
                        dgv.Rows[i].Cells[1].Value = col;
                        dgv.Rows[i].Cells[6].Value = col;
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            EleGenarate eg = new EleGenarate();
            eg.TopMost = true;
            eg.ShowDialog();
            if(!eg.isCancel)
            {
                string apdStr = eg.appendStr.Text.Trim();
                int eleType = eg.comboBox1.SelectedIndex;
                string template = eg.layoutHTML;
                string divId = eg.comboBox3.Text;
                var idCaps = new List<(string idname, string caption)>();
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    string col1 = dgv.Rows[i].Cells[1].Value?.ToString() ?? "";
                    string col6 = dgv.Rows[i].Cells[6].Value?.ToString() ?? "";
                    if (apdStr != "")
                    {
                        if (col1 != "" && !col1.StartsWith(apdStr))
                        {
                            dgv.Rows[i].Cells[1].Value = apdStr + col1;
                        }
                        if (col6 != "" && !col6.StartsWith(apdStr))
                        {
                            dgv.Rows[i].Cells[6].Value = apdStr + col6;
                        }
                    }
                    col1 = dgv.Rows[i].Cells[1].Value?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(col1))
                    {
                        idCaps.Add((col1, dgv.Rows[i].Cells[0].Value?.ToString() ?? ""));
                    }
                }
                EleGenarate.FormGenarate(template,eleType,partid,idCaps, divId);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            dgv.Rows.Clear();
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
    }
}
