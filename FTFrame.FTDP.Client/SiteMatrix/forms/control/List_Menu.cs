using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FTDPClient.consts;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
namespace FTDPClient.forms
{
	/// <summary>
	/// ErrorReport 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class List_Menu : System.Windows.Forms.Form
	{
        public string partId = null;
        public string restr = "";
        public bool IsCancel = false;
        private Button Close;
        private Button OK;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private Button button1;
        private Button button2;
        private Label label2;
        private ArrayList al;
        private Label label5;
        private Label label6;
        private Label label7;
        private TextBox anniu;
        private TextBox cfg;
        private Button button3;
        private Button button4;
        private Button button6;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem delToolStripMenuItem;
        private ToolStripMenuItem upToolStripMenuItem;
        private ToolStripMenuItem downToolStripMenuItem;
        private ComboBox gongneng;
        private ComboBox xuanxiang;
        private Label label1;
        private ComboBox tubiao;
        private Label label3;
        private Label label4;
        private Label label8;
        private ICSharpCode.TextEditor.TextEditorControl jiaoben;
        private IContainer components;

        public List_Menu()
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
            this.Close = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.delToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.anniu = new System.Windows.Forms.TextBox();
            this.cfg = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.gongneng = new System.Windows.Forms.ComboBox();
            this.xuanxiang = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tubiao = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.jiaoben = new ICSharpCode.TextEditor.TextEditorControl();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Close
            // 
            this.Close.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Close.Location = new System.Drawing.Point(586, 390);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 30);
            this.Close.TabIndex = 5;
            this.Close.Text = "&Close";
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // OK
            // 
            this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OK.Location = new System.Drawing.Point(493, 390);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 30);
            this.OK.TabIndex = 6;
            this.OK.Text = "&OK";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(156, 204);
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Tile;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 200;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.delToolStripMenuItem,
            this.upToolStripMenuItem,
            this.downToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(110, 92);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // delToolStripMenuItem
            // 
            this.delToolStripMenuItem.Name = "delToolStripMenuItem";
            this.delToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.delToolStripMenuItem.Text = "Del";
            this.delToolStripMenuItem.Click += new System.EventHandler(this.delToolStripMenuItem_Click);
            // 
            // upToolStripMenuItem
            // 
            this.upToolStripMenuItem.Name = "upToolStripMenuItem";
            this.upToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.upToolStripMenuItem.Text = "Up";
            this.upToolStripMenuItem.Click += new System.EventHandler(this.upToolStripMenuItem_Click);
            // 
            // downToolStripMenuItem
            // 
            this.downToolStripMenuItem.Name = "downToolStripMenuItem";
            this.downToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.downToolStripMenuItem.Text = "Down";
            this.downToolStripMenuItem.Click += new System.EventHandler(this.downToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(12, 222);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 24);
            this.button1.TabIndex = 8;
            this.button1.Text = "&Add";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(52, 222);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(36, 24);
            this.button2.TabIndex = 9;
            this.button2.Text = "&Del";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "按钮文字";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(185, 254);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "功能标识";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(185, 299);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "选项控制";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(185, 346);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "图标定义";
            // 
            // anniu
            // 
            this.anniu.Font = new System.Drawing.Font("宋体", 11F);
            this.anniu.Location = new System.Drawing.Point(187, 27);
            this.anniu.Name = "anniu";
            this.anniu.Size = new System.Drawing.Size(193, 24);
            this.anniu.TabIndex = 22;
            this.anniu.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // cfg
            // 
            this.cfg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.cfg.Location = new System.Drawing.Point(12, 252);
            this.cfg.Multiline = true;
            this.cfg.Name = "cfg";
            this.cfg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cfg.Size = new System.Drawing.Size(156, 134);
            this.cfg.TabIndex = 30;
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(92, 222);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(36, 24);
            this.button3.TabIndex = 34;
            this.button3.Text = "&Up";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Location = new System.Drawing.Point(132, 222);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(36, 24);
            this.button4.TabIndex = 35;
            this.button4.Text = "Do&w";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button6.Location = new System.Drawing.Point(558, 35);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(103, 30);
            this.button6.TabIndex = 38;
            this.button6.Text = "Text &Editor";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // gongneng
            // 
            this.gongneng.Font = new System.Drawing.Font("宋体", 11F);
            this.gongneng.FormattingEnabled = true;
            this.gongneng.Items.AddRange(new object[] {
            "del",
            "del([idcol],[statcol])",
            "refresh",
            "export_direct",
            "export",
            "copy",
            "saveto([table])",
            "flow[num]"});
            this.gongneng.Location = new System.Drawing.Point(187, 271);
            this.gongneng.Name = "gongneng";
            this.gongneng.Size = new System.Drawing.Size(193, 23);
            this.gongneng.TabIndex = 26;
            this.gongneng.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // xuanxiang
            // 
            this.xuanxiang.Font = new System.Drawing.Font("宋体", 11F);
            this.xuanxiang.FormattingEnabled = true;
            this.xuanxiang.Items.AddRange(new object[] {
            "none",
            "one",
            "more",
            "common"});
            this.xuanxiang.Location = new System.Drawing.Point(187, 318);
            this.xuanxiang.Name = "xuanxiang";
            this.xuanxiang.Size = new System.Drawing.Size(193, 23);
            this.xuanxiang.TabIndex = 39;
            this.xuanxiang.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(185, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 40;
            this.label1.Text = "操作脚本";
            // 
            // tubiao
            // 
            this.tubiao.Font = new System.Drawing.Font("宋体", 11F);
            this.tubiao.FormattingEnabled = true;
            this.tubiao.Items.AddRange(new object[] {
            "ui-icon-plus",
            "ui-icon-pencil",
            "ui-icon-document",
            "ui-icon-trash"});
            this.tubiao.Location = new System.Drawing.Point(187, 363);
            this.tubiao.Name = "tubiao";
            this.tubiao.Size = new System.Drawing.Size(193, 23);
            this.tubiao.TabIndex = 41;
            this.tubiao.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label3.Location = new System.Drawing.Point(397, 277);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 12);
            this.label3.TabIndex = 42;
            this.label3.Text = "选择内置功能或自定义标识";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label4.Location = new System.Drawing.Point(397, 324);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 12);
            this.label4.TabIndex = 43;
            this.label4.Text = "列表页复选框验证方式";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label8.Location = new System.Drawing.Point(395, 369);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(173, 12);
            this.label8.TabIndex = 44;
            this.label8.Text = "按钮图标定义，与前端框架相关";
            // 
            // jiaoben
            // 
            this.jiaoben.BackColor = System.Drawing.SystemColors.Control;
            this.jiaoben.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.jiaoben.Font = new System.Drawing.Font("宋体", 11F);
            this.jiaoben.Highlighting = "FTDP";
            this.jiaoben.Location = new System.Drawing.Point(187, 73);
            this.jiaoben.Name = "jiaoben";
            this.jiaoben.ShowLineNumbers = false;
            this.jiaoben.ShowVRuler = false;
            this.jiaoben.Size = new System.Drawing.Size(474, 173);
            this.jiaoben.TabIndent = 2;
            this.jiaoben.TabIndex = 45;
            this.jiaoben.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // List_Menu
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(673, 429);
            this.Controls.Add(this.jiaoben);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tubiao);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.xuanxiang);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.cfg);
            this.Controls.Add(this.gongneng);
            this.Controls.Add(this.anniu);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "List_Menu";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "List Menu";
            this.Load += new System.EventHandler(this.BackValue_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RowAll_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
#endregion
        private void ApplyLanguage()
        {
            OK.Text = res.About.GetString("String1");
            Close.Text = res.About.GetString("String2");
           
        }
        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            al.Clear();
            restr = cfg.Text;
            this.Close();
        }
        private void InitCfgStr()
        {
            string str = "";
            foreach (string[] item in al)
            {
                if (!item[0].Trim().Equals(""))
                {
                    str += "{$$}" + item[0].Replace("\r\n", "") + "[##]" + item[1].Replace("\r\n", "") + "[##]" + item[2].Replace("\r\n", "") + "[##]" + item[3].Replace("\r\n", "") + "[##]" + item[4].Replace("\r\n", "") ;
                }
            }
            if (!str.Equals("")) str = str.Substring(4);
            cfg.Text = str;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ListViewItem listview = new ListViewItem(new string[] {
            "Button "+(this.listView1.Items.Count+1)}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))));
            listview.ImageIndex = 26;
            this.listView1.Items.Add(listview);
            al.Add(new string[] { "Button", "", "custom", "", "none" });
            listView1.SelectedItems.Clear();
            listview.Selected = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                al.RemoveAt(listView1.SelectedItems[i].Index);
                listView1.SelectedItems[i].Remove();
            }
            listView1.SelectedItems.Clear();
            InitCfgStr();
        }

        private void BackValue_Load(object sender, EventArgs e)
        {
            label2.Text = res.ctl.str("List_Api.label2x");           //按钮文字
            label1.Text = res.ctl.str("List_Api.label1x");           //操作脚本
            label5.Text = res.ctl.str("List_Api.label5");           //功能标识
            label6.Text = res.ctl.str("List_Api.label6");           //选项控制
            label7.Text = res.ctl.str("List_Api.label7");           //图标定义
            label3.Text = res.ctl.str("List_Api.label3");           //选择内置功能或自定义标识
            label4.Text = res.ctl.str("List_Api.label4");           //列表页复选框验证方式
            label8.Text = res.ctl.str("List_Api.label8");			//按钮图标定义，与前端框架相关
            cfg.Text = restr;
            this.listView1.SmallImageList = globalConst.Imgs;
            this.listView1.LargeImageList = globalConst.Imgs;
            setInitFromSetString(restr);
            new FTDP.Util.ICSharpTextEditor().Init(this, jiaoben, false,null);
        }

        private void setInitFromSetString(string setstr)
        {
            listView1.Items.Clear();
            string[] items = setstr.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
            al = new ArrayList();
            foreach (string item in items)
            {
                string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                al.Add(colcfg);
            }
            foreach (string[] item in al)
            {
                ListViewItem listview = new ListViewItem(new string[] {
            (item[0].Trim().Equals("")?"Button ":(item[0].Trim()+" "))+(this.listView1.Items.Count+1)}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))));
                listview.ImageIndex = 26;
                this.listView1.Items.Add(listview);
            }
            if (listView1.Items.Count > 0) listView1.Items[0].Selected = true;
            listView1_SelectedIndexChanged(null, null);
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                label2.Visible = false;
                label3.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label4.Visible = false;
                label1.Visible = false;
                button6.Visible = false;
                jiaoben.Visible = false;
                anniu.Visible = false;
                gongneng.Visible = false;
                xuanxiang.Visible = false;
                tubiao.Visible = false;
                return;
            }
            foreach (ListViewItem listitem in listView1.Items)
            {
                if (listitem.ForeColor.Equals(Color.LightGray))
                {
                    listitem.ForeColor = Color.LightGray;
                }
                else
                {
                    listitem.ForeColor = System.Drawing.Color.Black;
                }
                listitem.BackColor = System.Drawing.Color.White;
                
                listitem.Font = new Font("宋体", 12F, System.Drawing.FontStyle.Regular);
            }
            listView1.SelectedItems[0].BackColor = System.Drawing.SystemColors.Highlight;
            if (listView1.SelectedItems[0].ForeColor.Equals(Color.LightGray))
            {
                listView1.SelectedItems[0].ForeColor = Color.LightGray;
            }
            else
            {
                listView1.SelectedItems[0].ForeColor = System.Drawing.Color.White;
            }
            listView1.SelectedItems[0].Font = new Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            label2.Visible = true;
            label3.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label4.Visible = true;
            label1.Visible = true;
            button6.Visible = true;
            jiaoben.Visible = true;
            anniu.Visible = true;
            gongneng.Visible = true;
            xuanxiang.Visible = true;
            tubiao.Visible = true;
            anniu.Text = ((string[])al[listView1.SelectedItems[0].Index])[0];
            jiaoben.Text = ((string[])al[listView1.SelectedItems[0].Index])[1];
            gongneng.Text = ((string[])al[listView1.SelectedItems[0].Index])[2];
            tubiao.Text = ((string[])al[listView1.SelectedItems[0].Index])[3];
            xuanxiang.Text = ((string[])al[listView1.SelectedItems[0].Index])[4];
        }

        private void test_Click(object sender, EventArgs e)
        {
            SQL sql = new SQL();
            sql.restr = "";
            sql.ShowDialog();
        }

        private void lieming_Leave(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                al[listView1.SelectedItems[0].Index] = new string[] { anniu.Text, jiaoben.Text, gongneng.Text, tubiao.Text, xuanxiang.Text };
                InitCfgStr();
                listView1.SelectedItems[0].Text = (anniu.Text.Trim().Equals("") ? "Button " : (anniu.Text.Trim() + " ")) + (listView1.SelectedItems[0].Index+1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int curpos = listView1.SelectedItems[0].Index;
                if (curpos > 0)
                {
                    string[] o1 = (string[])al[curpos];
                    string[] o0 = (string[])al[curpos - 1];
                    al[curpos] = o0;
                    al[curpos-1] = o1;
                    Color c1 = listView1.Items[curpos - 1].ForeColor;
                    Color c2 = listView1.Items[curpos].ForeColor;
                    listView1.Items[curpos - 1].ForeColor = c2;
                    listView1.Items[curpos].ForeColor = c1;
                    listView1.Items[curpos - 1].Text = (o1[0].Trim().Equals("") ? "Button " : (o1[0].Trim() + " ")) + curpos;
                    listView1.Items[curpos].Text = (o0[0].Trim().Equals("") ? "Button " : (o0[0].Trim() + " ")) + (curpos+1);
                    listView1.SelectedItems.Clear();
                    listView1.Items[curpos - 1].Selected=true;
                    InitCfgStr();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int curpos = listView1.SelectedItems[0].Index;
                if (curpos < al.Count-1)
                {
                    string[] o1 = (string[])al[curpos];
                    string[] o0 = (string[])al[curpos + 1];
                    al[curpos] = o0;
                    al[curpos + 1] = o1;
                    Color c1 = listView1.Items[curpos + 1].ForeColor;
                    Color c2 = listView1.Items[curpos].ForeColor;
                    listView1.Items[curpos + 1].ForeColor = c2;
                    listView1.Items[curpos].ForeColor = c1;
                    listView1.Items[curpos + 1].Text = (o1[0].Trim().Equals("") ? "Button " : (o1[0].Trim() + " ")) + (curpos+2);
                    listView1.Items[curpos].Text = (o0[0].Trim().Equals("") ? "Button " : (o0[0].Trim() + " ")) + (curpos + 1);
                    listView1.SelectedItems.Clear();
                    listView1.Items[curpos + 1].Selected = true;
                    InitCfgStr();
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (FormData.TheFormData == null)
            {
                FormData.TheFormData = new FormData();
            }
            FormData.TheFormData.Show();
        }

        private void lianjie_MouseDown(object sender, MouseEventArgs e)
        {
            TextEditor te = new TextEditor();
            te.basetext = jiaoben.Text;
            te.ShowDialog();
            jiaoben.Text = te.basetext;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TextEditor te = new TextEditor();
            te.basetext = jiaoben.Text;
            te.ShowDialog();
            jiaoben.Text = te.basetext;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
        }

        private void downToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button4_Click(sender, e);
        }

        private void RowAll_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string MainTable = classes.PageAsist.GetPartSetValue(partId, "MainTable");
            string CusSQL = classes.PageAsist.GetPartSetValue(partId, "CusSQL");
            if(MainTable==null || CusSQL==null)
            {
                functions.MsgBox.Warning(res.ctl.str("List_Api.5"));
            }
            else
            {
                if (MainTable.StartsWith("@")) MainTable = MainTable.Substring(1);
                else
                {
                    MainTable = "ft_" + consts.globalConst.CurSite.ID + "_f_" + MainTable;
                }
                control.RowAll_SelCol rs = new control.RowAll_SelCol();
                rs.MainTable = MainTable;
                rs.SelectSql = CusSQL;
                rs.ShowDialog();
                if(rs.SelColName!=null && rs.SelColName!="")
                {
                    jiaoben.Text = rs.SelColName;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string MainTable = classes.PageAsist.GetPartSetValue(partId, "MainTable");
            string CusSQL = classes.PageAsist.GetPartSetValue(partId, "CusSQL");
            if (MainTable == null || CusSQL == null)
            {
                functions.MsgBox.Warning("组件设置错误");
            }
            else
            {
                if (MainTable.StartsWith("@")) MainTable = MainTable.Substring(1);
                else
                {
                    if (!string.IsNullOrWhiteSpace(MainTable)) MainTable = "ft_" + consts.globalConst.CurSite.ID + "_f_" + MainTable;
                }
                control.RowAll_SetCols rs = new control.RowAll_SetCols();
                rs.MainTable = MainTable;
                rs.SelectSql = CusSQL;
                rs.ShowDialog();
                if (rs.SetString != null && rs.SetString != "")
                {
                    cfg.Text = rs.SetString;
                    setInitFromSetString(rs.SetString);
                }
            }
        }
    }

       
}
