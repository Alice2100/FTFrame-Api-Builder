using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SiteMatrix.consts;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using ICSharpCode.TextEditor.Document;
using SiteMatrix.database;
namespace SiteMatrix.forms
{
	/// <summary>
	/// ErrorReport 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
	public class SQL : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button test;
        public static string sql = null;
        public string restr = "";
        public bool IsCancel = false;
        private Button Close;
        private Button OK;
        private TextBox textsql;
        private ICSharpCode.TextEditor.TextEditorControl textEditorControl1;
        private ComboBox comboBox1;

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;

        public SQL()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQL));
            this.test = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.textsql = new System.Windows.Forms.TextBox();
            this.textEditorControl1 = new ICSharpCode.TextEditor.TextEditorControl();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // test
            // 
            this.test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.test.Location = new System.Drawing.Point(901, 475);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(75, 24);
            this.test.TabIndex = 2;
            this.test.Text = "&Test";
            this.test.Click += new System.EventHandler(this.cancel_Click);
            // 
            // Close
            // 
            this.Close.Location = new System.Drawing.Point(1063, 475);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 24);
            this.Close.TabIndex = 5;
            this.Close.Text = "&Close";
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(982, 475);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 24);
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
            this.textsql.Text = "//变量替换，用来测试SQL。变量间用分号或换行隔开，键值对用 : 隔开\r\n";
            // 
            // textEditorControl1
            // 
            this.textEditorControl1.Encoding = ((System.Text.Encoding)(resources.GetObject("textEditorControl1.Encoding")));
            this.textEditorControl1.Location = new System.Drawing.Point(4, 4);
            this.textEditorControl1.Name = "textEditorControl1";
            this.textEditorControl1.ShowEOLMarkers = true;
            this.textEditorControl1.ShowSpaces = true;
            this.textEditorControl1.ShowTabs = true;
            this.textEditorControl1.ShowVRuler = true;
            this.textEditorControl1.Size = new System.Drawing.Size(1136, 306);
            this.textEditorControl1.TabIndex = 9;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("宋体", 11F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "SQL模板",
            "字典选择1",
            "字典选择2"});
            this.comboBox1.Location = new System.Drawing.Point(781, 476);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(114, 23);
            this.comboBox1.TabIndex = 10;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // SQL
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(1144, 508);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textEditorControl1);
            this.Controls.Add(this.textsql);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.test);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SQL";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SQL";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SQL_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SQL_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
        private void ApplyLanguage()
        {
            OK.Text = "确定"; res.About.GetString("String1");
            Close.Text = "取消"; //res.About.GetString("String2");
            test.Text = "测试"; res.About.GetString("String3");
        }
		private void cancel_Click(object sender, System.EventArgs e)
		{
            test.Enabled = false;
            string sql = textEditorControl1.Text.Trim();
            string[] item = textsql.Text.Split(new string[] { "\r\n"},StringSplitOptions.RemoveEmptyEntries);
            foreach(string _item in item)
            {
                if (!_item.Trim().StartsWith("//"))
                {
                    string[] item2=_item.Trim().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string _item2 in item2)
                    {
                        string[] item3 = _item2.Trim().Split(':');
                        if(item3.Length==2)
                        {
                            sql = sql.Replace(item3[0],item3[1]);
                        }
                    }
                }
            }
            string connstr = Options.GetSystemDBSetConnStr();
            string dbtype = Options.GetSystemDBSetType();
            if (connstr.Equals(""))
            {
                functions.MsgBox.Error(res.About.GetString("String4"));
                return;
            }
            if (dbtype.ToLower() == "mysql")
            {
                MySqlConnection conn = null;
                try
                {
                    conn = new MySqlConnection(connstr);
                    conn.Open();
                    MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader();
                    BindingSource bs = new BindingSource();
                    bs.DataSource = dr;
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
            else if (dbtype.ToLower() == "sqlserver")
            {
                SqlConnection conn = null;
                try
                {
                    conn = new SqlConnection(connstr);
                    conn.Open();
                    SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader();
                    BindingSource bs = new BindingSource();
                    bs.DataSource = dr;
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
            restr = textEditorControl1.Text;
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
            //restr=restr.Replace("{","\\{").Replace("}","\\}");
            //texttext.Text = restr;
            //textsql.Text = restr;
            //HighLightSQL();
            textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("TSQL");
            textEditorControl1.Font = new Font("微软雅黑", 13);
            //textEditorControl1.BackColor = Color.Red;
            //textEditorControl1.ActiveTextAreaControl.BackColor = Color.Blue;
            //textEditorControl1.ActiveTextAreaControl.TextArea.BackColor = Color.Green;
            textEditorControl1.Text = restr;
            textEditorControl1.ShowEOLMarkers = false;
            textEditorControl1.ShowHRuler = false;
            textEditorControl1.ShowInvalidLines = false;
            textEditorControl1.ShowMatchingBracket = false;
            textEditorControl1.ShowSpaces = false;
            textEditorControl1.ShowTabs = false;
            textEditorControl1.ShowVRuler = false;
            textEditorControl1.IsIconBarVisible = false;

            comboBox1.Items.Clear();
            comboBox1.Items.Add(new Obj.ComboItem() { Id = "-1", Name = "SQL模板" });
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
            isformload = true;
        }
        bool Highting = false;
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
    }

       
}
