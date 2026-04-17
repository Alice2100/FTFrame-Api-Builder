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
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using FTDPClient.functions;
using ICSharpCode.TextEditor;

namespace FTDPClient.forms
{
	/// <summary>
	/// ErrorReport 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class SQL : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button test;
        public static string sql = null;
        public static bool FormSQLShow = false;
        public string restr = "";
        public bool IsCancel = true;
        private Button Close;
        private Button OK;
        private TextBox textsql;
        private ICSharpCode.TextEditor.TextEditorControl textEditorControl1;
        private ComboBox comboBox1;
        private Button button1;
        private ToolTip toolTip;
        private IContainer components;
        private ComboBox comboBox2;
        private Button button2;
        private Button button3;
        private CheckBox checkBox1;
        public string fromWhere = "";
        public SQL()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
			InitializeComponent();
            ApplyLanguage();
			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.test = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.textsql = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textEditorControl1 = new ICSharpCode.TextEditor.TextEditorControl();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // test
            // 
            this.test.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.test.Location = new System.Drawing.Point(879, 475);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(75, 30);
            this.test.TabIndex = 2;
            this.test.Text = "&Test";
            this.test.Click += new System.EventHandler(this.cancel_Click);
            // 
            // Close
            // 
            this.Close.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Close.Location = new System.Drawing.Point(1065, 475);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 30);
            this.Close.TabIndex = 5;
            this.Close.Text = "&Close";
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // OK
            // 
            this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OK.Location = new System.Drawing.Point(973, 475);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 30);
            this.OK.TabIndex = 6;
            this.OK.Text = "&OK";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // textsql
            // 
            this.textsql.BackColor = System.Drawing.Color.LightGray;
            this.textsql.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textsql.Location = new System.Drawing.Point(4, 317);
            this.textsql.Multiline = true;
            this.textsql.Name = "textsql";
            this.textsql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textsql.Size = new System.Drawing.Size(1136, 151);
            this.textsql.TabIndex = 8;
            this.textsql.Text = "//变量替换，用来测试SQL。变量间用分号或换行隔开，键值对用 : 隔开；多表查询用Select * 时，*替换为 @*@，则自动去除重复列";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox1.Font = new System.Drawing.Font("宋体", 11F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "SQL模板",
            "字典选择1",
            "字典选择2"});
            this.comboBox1.Location = new System.Drawing.Point(584, 478);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(277, 23);
            this.comboBox1.TabIndex = 10;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(371, 475);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 30);
            this.button1.TabIndex = 11;
            this.button1.Text = "Text Editor";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textEditorControl1
            // 
            this.textEditorControl1.Font = new System.Drawing.Font("微软雅黑", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEditorControl1.Highlighting = null;
            this.textEditorControl1.Location = new System.Drawing.Point(4, 4);
            this.textEditorControl1.Name = "textEditorControl1";
            this.textEditorControl1.ShowVRuler = false;
            this.textEditorControl1.Size = new System.Drawing.Size(1136, 306);
            this.textEditorControl1.TabIndex = 9;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 100;
            this.toolTip.ReshowDelay = 10;
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox2.Font = new System.Drawing.Font("宋体", 11F);
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "SQL模板",
            "字典选择1",
            "字典选择2"});
            this.comboBox2.Location = new System.Drawing.Point(94, 478);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(177, 23);
            this.comboBox2.TabIndex = 12;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(278, 475);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 30);
            this.button2.TabIndex = 13;
            this.button2.Text = "&Para";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(480, 475);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(93, 30);
            this.button3.TabIndex = 14;
            this.button3.Text = "Field Editor";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(4, 483);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(90, 16);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "&Force Comit";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // SQL
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(1144, 511);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.test);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textEditorControl1);
            this.Controls.Add(this.textsql);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Close);
            this.KeyPreview = true;
            this.Name = "SQL";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SQL";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SQL_FormClosed);
            this.Load += new System.EventHandler(this.SQL_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SQL_KeyDown);
            this.Resize += new System.EventHandler(this.SQL_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
        private void ApplyLanguage()
        {
            OK.Text = res.ctl.str("SQL.OK");            //确定(&O)
            Close.Text = res.ctl.str("SQL.Close");          //关闭(&C)
            test.Text = res.ctl.str("SQL.test");            //测试(&T)
            textsql.Text = res.ctl.str("SQL.textsql");			////变量替换，用来测试SQL。变量间用分号或换行隔开，键值对用 : 隔开
        }
		private void cancel_Click(object sender, System.EventArgs e)
		{
            test.Enabled = false;
            var selText = textEditorControl1.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText;
            string sql = string.IsNullOrWhiteSpace(selText)?textEditorControl1.Text.Trim(): selText.Trim();
            if(sql.StartsWith("@SQL{"))
            {
                int firstIndex = sql.IndexOf('{');
                int lastIndex = sql.LastIndexOf('}');
                sql = sql.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
            }
            sql = functions.Adv.SQLPatternOP(sql);
            string[] item = textsql.Text.Split(new string[] { Environment.NewLine},StringSplitOptions.RemoveEmptyEntries);
            if (item.Length <= 12)
            {
                foreach (string _item in item)
                {
                    if (!_item.Trim().StartsWith("//"))
                    {
                        string[] item2 = _item.Trim().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string _item2 in item2)
                        {
                            string[] item3 = _item2.Trim().Split(':');
                            if (item3.Length == 2)
                            {
                                sql = sql.Replace(item3[0], item3[1]);
                            }
                        }
                    }
                }
            }
            string connstr = Options.GetSystemDBSetConnStr();
            var dbtype = Options.GetSystemDBSetType();
            if (connstr.Equals(""))
            {
                functions.MsgBox.Error(res.About.GetString("String4"));
                return;
            }
            sql = functions.Adv.GetSqlForRemoveSameCols(dbtype, connstr, sql);
            sql = functions.Adv.CodePattern(sql);
            if (dbtype==globalConst.DBType.MySql)
            {
                MySqlConnection conn = null;
                try
                {
                    conn = new MySqlConnection(connstr);
                    conn.Open();
                    MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader();
                    BindingSource bs = new BindingSource();
                    if (dr.HasRows)
                    {
                        bs.DataSource = dr;
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        for(int i=0;i<dr.FieldCount;i++)
                        {
                            dt.Columns.Add(dr.GetName(i));
                        }
                        bs.DataSource = dt;
                    }
                    DataBind db = new DataBind();
                    db.grid.DataSource = bs;
                    db.ShowDialog();
                    dr.Close();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    functions.MsgBox.Error(ex.Message);
                }
                finally
                {
                    if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                        conn = null;
                    }
                }
            }
            else if (dbtype==globalConst.DBType.SqlServer)
            {
                SqlConnection conn = null;
                try
                {
                    conn = new SqlConnection(connstr);
                    conn.Open();
                    SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader();
                    BindingSource bs = new BindingSource();
                    if (dr.HasRows)
                    {
                        bs.DataSource = dr;
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            dt.Columns.Add(dr.GetName(i));
                        }
                        bs.DataSource = dt;
                    }
                    DataBind db = new DataBind();
                    db.grid.DataSource = bs;
                    db.ShowDialog();
                    dr.Close();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    functions.MsgBox.Error(ex.Message);
                }
                finally
                {
                    if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                        conn = null;
                    }
                }
            }
            else if (dbtype == globalConst.DBType.Sqlite)
            {
                DB conn = null;
                try
                {
                    conn = new DB(connstr);
                    conn.Open();
                    var dr = conn.OpenRecord(sql);
                    BindingSource bs = new BindingSource();
                    if (dr.HasRows)
                    {
                        bs.DataSource = dr;
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            dt.Columns.Add(dr.GetName(i));
                        }
                        bs.DataSource = dt;
                    }
                    DataBind db = new DataBind();
                    db.grid.DataSource = bs;
                    db.ShowDialog();
                    dr.Close();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    functions.MsgBox.Error(ex.Message);
                }
                finally
                {
                    if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                        conn = null;
                    }
                }
            }
            test.Enabled = true;
		}

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            IsCancel = false;
            restr = textEditorControl1.Text.Replace(" \r\n", "\r\n").Replace("\r\n","\n").Replace("\r", "\n").Replace("\n", " \r\n");
            this.Close();
        }
        private void rtfsql_TextChanged(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                //textsql.Text = texttext.Text.Replace("@siteid@", globalConst.CurSite.ID);
            }
        }
        private void SQL_Load(object sender, EventArgs e)
        {
            FormSQLShow = true;
            //restr=restr.Replace("{","\\{").Replace("}","\\}");
            //texttext.Text = restr;
            //textsql.Text = restr;
            //HighLightSQL();
            textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("SQL");
            textEditorControl1.Font = new Font("微软雅黑", 13);
            //textEditorControl1.BackColor = Color.Red;
            //textEditorControl1.ActiveTextAreaControl.BackColor = Color.Blue;
            //textEditorControl1.ActiveTextAreaControl.TextArea.BackColor = Color.Green;
            textEditorControl1.Text = restr;
            new FTDP.Util.ICSharpTextEditor().Init(this, textEditorControl1, true, null);
            //textEditorControl1.ShowEOLMarkers = false;
            //textEditorControl1.ShowHRuler = false;
            //textEditorControl1.ShowInvalidLines = false;
            //textEditorControl1.ShowMatchingBracket = false;
            //textEditorControl1.ShowSpaces = false;
            //textEditorControl1.ShowTabs = false;
            //textEditorControl1.ShowVRuler = false;
            //textEditorControl1.IsIconBarVisible = false;

            comboBox1.Items.Clear();
            comboBox1.Items.Add(new Obj.ComboItem() { Id = "-1", Name = res.ctl.str("SQL.1") });
            string sql = "select id,caption from snippets order by id";
            DR dr = new DR(globalConst.ConfigConn.OpenRecord(sql));
            while (dr.Read())
            {
                if (dr.getString("caption").StartsWith("[SQL]"))
                {
                    comboBox1.Items.Add(new Obj.ComboItem() {  Id= dr.getInt32("id").ToString(), Name= dr.getString("caption").Substring(5) }  );
                }
            }
            dr.Close();
            comboBox1.SelectedIndex = 0;

            textEditorControl1.ActiveTextAreaControl.TextArea.MouseClick += ActiveTextAreaControl_MouseClick;

            if (fromWhere == "dyvalue")
            {
                textsql.AppendText(Environment.NewLine+ res.com.str("TextEditor.label.dyvalue"));
            }

            comboBox2.Items.Clear();
            Regex r = new Regex(@"@para\{[^\}]*\}");
            var mc = r.Matches(restr);
            List<string> mValue = new List<string>();
            foreach (Match m in mc)
            {
                string p = m.Value;
                if(p.IndexOf(',')>0)
                {
                    p = p.Substring(0, p.IndexOf(',')).Trim() + "}";
                }
                if (mValue.Contains(p)) continue;
                mValue.Add(p);
            }
            mValue.ForEach(m => {
                comboBox2.Items.Add(m);
            });
            mValue.Clear();
            if (comboBox2.Items.Count > 0) comboBox2.SelectedIndex = 0;
            comboBox2.Visible = comboBox2.Items.Count > 0;

            checkBox1.Checked = restr.StartsWith("@ForceComit#");

            isformload = true;
        }
        char[] chars = new char[] { '\n', '\t', ' ', '\"', '\'', ',', '|', '{', '[', '(', '}', ']', ')', ';', ':', '/', '*', '=', '!', '>', '<' };
        private void ActiveTextAreaControl_MouseClick(object sender, EventArgs e)
        {
            toolTip.Hide(textEditorControl1.ActiveTextAreaControl.TextArea);
            var textArea = textEditorControl1.ActiveTextAreaControl;
            string lineStr = textArea.Document.GetText(textArea.Document.GetLineSegment(textArea.Caret.Position.Line))+ " ";
            string leftStr = lineStr.Substring(0, lineStr.IndexOfAny(chars, textArea.Caret.Position.X));
            string[] steItems = leftStr.Split(chars);
            string ptnstr = "";
            if (steItems.Length > 0) ptnstr = steItems[steItems.Length - 1];
            if(ptnstr.IndexOf('.')<0)
            {
                var item=FTDP.Util.ICSharpTextEditor.completionData_Table.Where(r=>r[0].ToString().Equals(ptnstr,StringComparison.CurrentCultureIgnoreCase));
                if(item.Count()>0)
                {
                    toolTip.ToolTipTitle = " "+item.First()[1];
                    toolTip.Show(" " + ptnstr, textEditorControl1.ActiveTextAreaControl.TextArea);
                }
            }
            else
            {
                string aliasName = ptnstr.Split('.')[0];
                string colName = ptnstr.Split('.')[1];
                var dic = FTDP.Util.ICSharpTextEditor.TableAlias(textEditorControl1.Text);
                string tablename = null;
                if (dic.ContainsKey(aliasName)) tablename = dic[aliasName];
                if(tablename!=null)
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
                        toolTip.ToolTipTitle = " " + item.First()[1]+" ["+ tablename + "]";
                        var item2 = StrictFields.Where(r => r[0].ToString().Equals(colName, StringComparison.CurrentCultureIgnoreCase));
                        if(item2.Count()>0)
                        {
                            toolTip.Show(" " + item2.First()[1] + " [" + colName + "]", textEditorControl1.ActiveTextAreaControl.TextArea);
                        }
                        else
                        {
                            toolTip.Show(" " + ("Not Exist ") + " [" + colName + "]", textEditorControl1.ActiveTextAreaControl.TextArea);
                        }
                    }
                }
            }
            return;
        }

        bool isformload = false;
        private void SQL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (Close.Focused) this.Close();
                else Close.Focus();
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isformload)
            {
                int mubanID = int.Parse(((Obj.ComboItem)comboBox1.SelectedItem).Id);
                if (mubanID >= 0)
                {
                    string sql = "select content from snippets where id=" + mubanID;
                    DR dr = new DR(globalConst.ConfigConn.OpenRecord(sql));
                    dr.Read();
                    textEditorControl1.Visible = false;
                    textEditorControl1.Text = dr.getString(0);
                    textEditorControl1.Visible = true;
                    dr.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //textEditorControl1.Text = NSQLFormatter.Formatter.Format(textEditorControl1.Text);
            //return;
            TextEditor te = new TextEditor();
            te.basetext = textEditorControl1.Text;
            te.TopMost = true;
            te.ShowDialog();
            if(!te.cancel)
            {
                textEditorControl1.Text = te.basetext;
            }
        }

        private void SQL_Resize(object sender, EventArgs e)
        {
            textEditorControl1.Width = 1136 + this.Width - 1160;
            textEditorControl1.Height = 306 + this.Height - 550;
            textsql.Width = 1136 + this.Width - 1160;
            textsql.Top = 317 + this.Height - 550;
            button1.Location = new Point(371 + this.Width - 1160, 475 + this.Height - 550);
            button2.Location = new Point(278 + this.Width - 1160, 475 + this.Height - 550);
            button3.Location = new Point(480 + this.Width - 1160, 475 + this.Height - 550);
            comboBox1.Location = new Point(584 + this.Width - 1160, 478 + this.Height - 550);
            test.Location = new Point(879 + this.Width - 1160, 475 + this.Height - 550);
            OK.Location = new Point(973 + this.Width - 1160, 475 + this.Height - 550);
            Close.Location = new Point(1065 + this.Width - 1160, 475 + this.Height - 550);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ParaDev paraDev = new ParaDev();
            paraDev.initParaSel = comboBox2.Text;
            paraDev.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            try
            {
                var sqlAll = textEditorControl1.Text;
                var dic = FTDP.Util.ICSharpTextEditor.TableAlias(sqlAll);
                var selText = textEditorControl1.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText;
                string sqlColumns = "";
                var sql_qian = "";
                var sql_hou = "";
                var selTextMode = false;
                if(string.IsNullOrWhiteSpace(selText))
                {
                    var startIndex = sqlAll.IndexOf("select ", StringComparison.OrdinalIgnoreCase);
                    if(startIndex<0) startIndex = sqlAll.IndexOf("select\t", StringComparison.OrdinalIgnoreCase);
                    if(startIndex<0) startIndex = sqlAll.IndexOf("select\n", StringComparison.OrdinalIgnoreCase);
                    if(startIndex<0) startIndex = sqlAll.IndexOf("select\r", StringComparison.OrdinalIgnoreCase);
                    if(startIndex<0)
                    {
                        MsgBox.Warning("Must have 'select' include");
                        return;
                    }
                    var endIndex = sqlAll.IndexOf(" from", StringComparison.OrdinalIgnoreCase);
                    if (endIndex < 0) endIndex = sqlAll.IndexOf("\tfrom", StringComparison.OrdinalIgnoreCase);
                    if (endIndex < 0) endIndex = sqlAll.IndexOf("\nfrom", StringComparison.OrdinalIgnoreCase);
                    if (endIndex < 0) endIndex = sqlAll.IndexOf("\rfrom", StringComparison.OrdinalIgnoreCase);
                    if (endIndex < 0)
                    {
                        MsgBox.Warning("Must have 'from' include");
                        return;
                    }
                    if(endIndex - startIndex - 7<0)
                    { sqlColumns = ""; }
                    else sqlColumns = sqlAll.Substring(startIndex+7, endIndex- startIndex-7);
                    sql_qian = "select";
                    sql_hou = sqlAll.Substring(endIndex);
                    sql_hou = sql_hou.Substring(sql_hou.IndexOf("from", StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    selTextMode = true;
                    sqlColumns = selText.Trim();
                    sql_qian = sqlAll.Substring(0, sqlAll.IndexOf(selText));
                    sql_hou= sqlAll.Substring(sqlAll.IndexOf(selText)+selText.Length);
                }
                //MsgBox.Information(sqlColumns+"\r\n"+ sql_qian+"\r\n"+sql_hou);
                //foreach(var item in dic)
                //{
                //    MsgBox.Information(item.Key+":"+item.Value);//a:tablename
                //}
                var fieldsOri= sqlColumns.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries);
                var fields = new List<(string prefix,string field,string alias)>();
                foreach(var item in fieldsOri)
                {
                    var column = item.Trim();
                    if(column.IndexOf(" ")>0)
                    {
                        var prefixField = column.Substring(0, column.IndexOf(" ")).Trim();
                        var alias = column.Substring(column.LastIndexOf(" ")).Trim();
                        if(prefixField.IndexOf('.')<0)
                        {
                            MsgBox.Warning("Field must have '.'");
                            return;
                        }
                        fields.Add((prefixField.Split('.')[0], prefixField.Split('.')[1], alias));
                    }
                    else
                    {
                        var prefixField = column;
                        if (prefixField.IndexOf('.') < 0)
                        {
                            MsgBox.Warning("Field must have '.'");
                            return;
                        }
                        fields.Add((prefixField.Split('.')[0], prefixField.Split('.')[1], ""));
                    }
                }
                var sqlFieldEditor = new control.SQL_FieldEditor();
                sqlFieldEditor.Fields = fields;
                sqlFieldEditor.TableAlias = dic;
                sqlFieldEditor.comboBox1.Items.Clear();
                sqlFieldEditor.comboBox1.Items.Add("Select Table");
                foreach (var item in dic)
                {
                    sqlFieldEditor.comboBox1.Items.Add(item.Value + " " + item.Key);
                }
                sqlFieldEditor.comboBox1.SelectedIndex = 0;
                sqlFieldEditor.ShowDialog();
                if(sqlFieldEditor.IsOk)
                {
                    if(selTextMode)
                        textEditorControl1.Text = sql_qian+" "+sqlFieldEditor.FieldsStr+" " +sql_hou;
                    else
                        textEditorControl1.Text = sql_qian + " " + sqlFieldEditor.FieldsStr + Environment.NewLine + sql_hou;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            finally
            {
                button3.Enabled = true;
            }
        }

        private void SQL_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormSQLShow = false;
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked && !textEditorControl1.Text.StartsWith("@ForceComit#"))
            {
                textEditorControl1.Text = "@ForceComit#" + textEditorControl1.Text;
            }
            else if(!checkBox1.Checked && textEditorControl1.Text.StartsWith("@ForceComit#"))
            {
                textEditorControl1.Text = textEditorControl1.Text.Substring(12);
            }
        }
    }

       
}
