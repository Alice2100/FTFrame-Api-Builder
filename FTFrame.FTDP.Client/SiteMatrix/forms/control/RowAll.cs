using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FTDPClient.consts;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
using FTDPClient.functions;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace FTDPClient.forms
{
	/// <summary>
	/// ErrorReport 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class RowAll : System.Windows.Forms.Form
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
        private Button test;
        private Label label3;
        private ArrayList al;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private TextBox lieming;
        private TextBox kuandu;
        private TextBox paixu;
        private TextBox lianjie;
        private ComboBox target;
        private TextBox kaiguan;
        private Label label4;
        private TextBox cfg;
        private TextBox shujutip;
        private TextBox saveto;
        private Label label1;
        private Button button3;
        private Button button4;
        private Button button5;
        private CheckBox defaulthidden;
        private Button button6;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem delToolStripMenuItem;
        private ToolStripMenuItem upToolStripMenuItem;
        private ToolStripMenuItem downToolStripMenuItem;
        private Button button7;
        private Button button8;
        private Button button9;
        private IContainer components;
        private ImageList imageList1;
        private ICSharpCode.TextEditor.TextEditorControl shuju;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem deleteAllToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem copyKeysToolStripMenuItem;
        private ToolStripMenuItem copyItemToolStripMenuItem;
        private ToolStripMenuItem pasteItemToolStripMenuItem;
        private ToolStripMenuItem sortbykeytoolStripMenuItem1;
        public System.Xml.XmlDocument XmlDoc;
        public RowAll()
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("111112111111111 - 计量单位 1");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("111112111111111 - 库存数量", 1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RowAll));
            this.Close = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.delToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.test = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lieming = new System.Windows.Forms.TextBox();
            this.kuandu = new System.Windows.Forms.TextBox();
            this.paixu = new System.Windows.Forms.TextBox();
            this.lianjie = new System.Windows.Forms.TextBox();
            this.target = new System.Windows.Forms.ComboBox();
            this.kaiguan = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cfg = new System.Windows.Forms.TextBox();
            this.shujutip = new System.Windows.Forms.TextBox();
            this.saveto = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.defaulthidden = new System.Windows.Forms.CheckBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.shuju = new ICSharpCode.TextEditor.TextEditorControl();
            this.copyKeysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.sortbykeytoolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Close
            // 
            this.Close.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Close.Location = new System.Drawing.Point(694, 518);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(80, 30);
            this.Close.TabIndex = 5;
            this.Close.Text = "&Close";
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // OK
            // 
            this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OK.Location = new System.Drawing.Point(598, 518);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(80, 30);
            this.OK.TabIndex = 6;
            this.OK.Text = "&OK";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Font = new System.Drawing.Font("宋体", 12F);
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.HideSelection = false;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.listView1.Location = new System.Drawing.Point(11, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(283, 371);
            this.listView1.TabIndex = 7;
            this.listView1.TileSize = new System.Drawing.Size(280, 28);
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 275;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.delToolStripMenuItem,
            this.upToolStripMenuItem,
            this.downToolStripMenuItem,
            this.toolStripSeparator2,
            this.copyKeysToolStripMenuItem,
            this.copyItemToolStripMenuItem,
            this.pasteItemToolStripMenuItem,
            this.sortbykeytoolStripMenuItem1,
            this.toolStripSeparator1,
            this.deleteAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 236);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // delToolStripMenuItem
            // 
            this.delToolStripMenuItem.Name = "delToolStripMenuItem";
            this.delToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.delToolStripMenuItem.Text = "Del";
            this.delToolStripMenuItem.Click += new System.EventHandler(this.delToolStripMenuItem_Click);
            // 
            // upToolStripMenuItem
            // 
            this.upToolStripMenuItem.Name = "upToolStripMenuItem";
            this.upToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.upToolStripMenuItem.Text = "Up";
            this.upToolStripMenuItem.Click += new System.EventHandler(this.upToolStripMenuItem_Click);
            // 
            // downToolStripMenuItem
            // 
            this.downToolStripMenuItem.Name = "downToolStripMenuItem";
            this.downToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.downToolStripMenuItem.Text = "Down";
            this.downToolStripMenuItem.Click += new System.EventHandler(this.downToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // deleteAllToolStripMenuItem
            // 
            this.deleteAllToolStripMenuItem.Name = "deleteAllToolStripMenuItem";
            this.deleteAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deleteAllToolStripMenuItem.Text = "Delete All";
            this.deleteAllToolStripMenuItem.Click += new System.EventHandler(this.deleteAllToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(11, 389);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 24);
            this.button1.TabIndex = 8;
            this.button1.Text = "&Add";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(82, 389);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(69, 24);
            this.button2.TabIndex = 9;
            this.button2.Text = "&Del";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(298, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "lieming";
            // 
            // test
            // 
            this.test.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.test.Location = new System.Drawing.Point(500, 518);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(80, 30);
            this.test.TabIndex = 13;
            this.test.Text = "SQL &Test";
            this.test.Click += new System.EventHandler(this.test_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(300, 281);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "shuju";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(298, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "kuandu";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(298, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "paixu";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(298, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "lianjie";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(298, 181);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 20;
            this.label8.Text = "target";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(298, 224);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 12);
            this.label9.TabIndex = 21;
            this.label9.Text = "kaiguan";
            // 
            // lieming
            // 
            this.lieming.Font = new System.Drawing.Font("宋体", 11F);
            this.lieming.Location = new System.Drawing.Point(300, 27);
            this.lieming.Name = "lieming";
            this.lieming.Size = new System.Drawing.Size(193, 24);
            this.lieming.TabIndex = 22;
            this.lieming.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // kuandu
            // 
            this.kuandu.Font = new System.Drawing.Font("宋体", 11F);
            this.kuandu.Location = new System.Drawing.Point(300, 70);
            this.kuandu.Name = "kuandu";
            this.kuandu.Size = new System.Drawing.Size(193, 24);
            this.kuandu.TabIndex = 23;
            this.kuandu.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // paixu
            // 
            this.paixu.Font = new System.Drawing.Font("宋体", 11F);
            this.paixu.Location = new System.Drawing.Point(300, 111);
            this.paixu.Name = "paixu";
            this.paixu.Size = new System.Drawing.Size(193, 24);
            this.paixu.TabIndex = 24;
            this.paixu.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // lianjie
            // 
            this.lianjie.Font = new System.Drawing.Font("宋体", 11F);
            this.lianjie.Location = new System.Drawing.Point(300, 155);
            this.lianjie.Name = "lianjie";
            this.lianjie.Size = new System.Drawing.Size(474, 24);
            this.lianjie.TabIndex = 25;
            this.lianjie.Leave += new System.EventHandler(this.lieming_Leave);
            this.lianjie.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lianjie_MouseDown);
            // 
            // target
            // 
            this.target.Font = new System.Drawing.Font("宋体", 11F);
            this.target.FormattingEnabled = true;
            this.target.Location = new System.Drawing.Point(300, 197);
            this.target.Name = "target";
            this.target.Size = new System.Drawing.Size(193, 23);
            this.target.TabIndex = 26;
            this.target.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // kaiguan
            // 
            this.kaiguan.Font = new System.Drawing.Font("宋体", 11F);
            this.kaiguan.Location = new System.Drawing.Point(300, 239);
            this.kaiguan.Name = "kaiguan";
            this.kaiguan.Size = new System.Drawing.Size(193, 24);
            this.kaiguan.TabIndex = 27;
            this.kaiguan.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(499, 245);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 28;
            this.label4.Text = "kaiguantip";
            // 
            // cfg
            // 
            this.cfg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.cfg.Location = new System.Drawing.Point(11, 419);
            this.cfg.Multiline = true;
            this.cfg.Name = "cfg";
            this.cfg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cfg.Size = new System.Drawing.Size(283, 127);
            this.cfg.TabIndex = 30;
            // 
            // shujutip
            // 
            this.shujutip.BackColor = System.Drawing.SystemColors.Control;
            this.shujutip.Font = new System.Drawing.Font("宋体", 9F);
            this.shujutip.ForeColor = System.Drawing.Color.Red;
            this.shujutip.Location = new System.Drawing.Point(300, 389);
            this.shujutip.Multiline = true;
            this.shujutip.Name = "shujutip";
            this.shujutip.ReadOnly = true;
            this.shujutip.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.shujutip.Size = new System.Drawing.Size(474, 122);
            this.shujutip.TabIndex = 31;
            // 
            // saveto
            // 
            this.saveto.BackColor = System.Drawing.Color.NavajoWhite;
            this.saveto.Font = new System.Drawing.Font("宋体", 11F);
            this.saveto.Location = new System.Drawing.Point(637, 224);
            this.saveto.Name = "saveto";
            this.saveto.Size = new System.Drawing.Size(137, 24);
            this.saveto.TabIndex = 32;
            this.saveto.Visible = false;
            this.saveto.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label1.Location = new System.Drawing.Point(503, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 33;
            this.label1.Text = "saveto";
            this.label1.Visible = false;
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(153, 389);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(69, 24);
            this.button3.TabIndex = 34;
            this.button3.Text = "&Up";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Location = new System.Drawing.Point(224, 389);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(69, 24);
            this.button4.TabIndex = 35;
            this.button4.Text = "Do&wn";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Gainsboro;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button5.Location = new System.Drawing.Point(656, 185);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(103, 30);
            this.button5.TabIndex = 36;
            this.button5.Text = "&Show Tables";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // defaulthidden
            // 
            this.defaulthidden.AutoSize = true;
            this.defaulthidden.Location = new System.Drawing.Point(512, 27);
            this.defaulthidden.Name = "defaulthidden";
            this.defaulthidden.Size = new System.Drawing.Size(84, 16);
            this.defaulthidden.TabIndex = 37;
            this.defaulthidden.Text = "默认不显示";
            this.defaulthidden.UseVisualStyleBackColor = true;
            this.defaulthidden.Click += new System.EventHandler(this.lieming_Leave);
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button6.Location = new System.Drawing.Point(671, 353);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(103, 30);
            this.button6.TabIndex = 38;
            this.button6.Text = "Text &Editor";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button7.Location = new System.Drawing.Point(550, 353);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(103, 30);
            this.button7.TabIndex = 39;
            this.button7.Text = "选择数据列";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.SkyBlue;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button8.Location = new System.Drawing.Point(671, 12);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(103, 30);
            this.button8.TabIndex = 40;
            this.button8.Text = "快速定义";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.SkyBlue;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button9.Location = new System.Drawing.Point(671, 55);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(103, 30);
            this.button9.TabIndex = 41;
            this.button9.Text = "API定义";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "rowall.gif");
            // 
            // shuju
            // 
            this.shuju.BackColor = System.Drawing.SystemColors.Control;
            this.shuju.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shuju.Font = new System.Drawing.Font("宋体", 11F);
            this.shuju.Highlighting = "FTDP";
            this.shuju.Location = new System.Drawing.Point(300, 298);
            this.shuju.Name = "shuju";
            this.shuju.ShowLineNumbers = false;
            this.shuju.ShowVRuler = false;
            this.shuju.Size = new System.Drawing.Size(474, 49);
            this.shuju.TabIndent = 2;
            this.shuju.TabIndex = 42;
            this.shuju.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // copyKeysToolStripMenuItem
            // 
            this.copyKeysToolStripMenuItem.Name = "copyKeysToolStripMenuItem";
            this.copyKeysToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copyKeysToolStripMenuItem.Text = "Copy Keys";
            this.copyKeysToolStripMenuItem.Click += new System.EventHandler(this.copyKeysToolStripMenuItem_Click);
            // 
            // copyItemToolStripMenuItem
            // 
            this.copyItemToolStripMenuItem.Name = "copyItemToolStripMenuItem";
            this.copyItemToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copyItemToolStripMenuItem.Text = "Copy Items";
            this.copyItemToolStripMenuItem.Click += new System.EventHandler(this.copyItemToolStripMenuItem_Click);
            // 
            // pasteItemToolStripMenuItem
            // 
            this.pasteItemToolStripMenuItem.Name = "pasteItemToolStripMenuItem";
            this.pasteItemToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.pasteItemToolStripMenuItem.Text = "Paste Items";
            this.pasteItemToolStripMenuItem.Click += new System.EventHandler(this.pasteItemToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // sortbykeytoolStripMenuItem1
            // 
            this.sortbykeytoolStripMenuItem1.Name = "sortbykeytoolStripMenuItem1";
            this.sortbykeytoolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.sortbykeytoolStripMenuItem1.Text = "Sort By Key";
            this.sortbykeytoolStripMenuItem1.Click += new System.EventHandler(this.sortbykeytoolStripMenuItem1_Click);
            // 
            // RowAll
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(789, 558);
            this.Controls.Add(this.shuju);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.defaulthidden);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.saveto);
            this.Controls.Add(this.shujutip);
            this.Controls.Add(this.cfg);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.kaiguan);
            this.Controls.Add(this.target);
            this.Controls.Add(this.lianjie);
            this.Controls.Add(this.paixu);
            this.Controls.Add(this.kuandu);
            this.Controls.Add(this.lieming);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.test);
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
            this.Name = "RowAll";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rows Define";
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
            label2.Text = res.About.GetString("String8");
            label3.Text = res.About.GetString("String9");
            label5.Text = res.About.GetString("String10");
            label6.Text = res.About.GetString("String11");
            label9.Text = res.About.GetString("String12");
            label7.Text = res.About.GetString("String13");
            label8.Text = res.ctl.str("RawAll.label8");			//说明/描述
            label4.Text = res.About.GetString("String15");
            shujutip.Text = res.About.GetString("String16");
            label1.Text = res.form.GetString("String148");
            defaulthidden.Text = res.ctl.str("RawAll.defaulthidden");           //默认不显示
            button8.Text = res.ctl.str("RawAll.button8");           //快速定义
            button9.Text = res.ctl.str("RawAll.button9");           //API定义
            button7.Text = res.ctl.str("RawAll.button7");			//选择数据列
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
                if (!item[0].Trim().Equals("") || !item[1].Trim().Equals(""))
                {
                    str += "|||" + item[0].Replace("\r\n", "") + "#" + functions.str.getEncode(item[1]) + "#" + item[2].Replace("\r\n", "") + "#" + item[3].Replace("\r\n", "") + "#" + item[4].Replace("\r\n", "") + "#" + item[5].Replace("\r\n", "") + "#" + item[6].Replace("\r\n", "") + "#" + item[8] + "&&&" + item[7].Replace("\r\n", "");
                }
            }
            if (!str.Equals("")) str = str.Substring(3);
            cfg.Text = str;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ListViewItem listview = new ListViewItem(new string[] {
            "Column "+(this.listView1.Items.Count+1)}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))));
            listview.ImageIndex = 0;
            this.listView1.Items.Add(listview);
            al.Add(new string[] { "", "", "auto;left", "", "", "", "", "", "0" });
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
            listView1.Items.Clear();
            cfg.Text = restr;
            this.listView1.SmallImageList = imageList1;
            //this.listView1.SmallImageList.Images.Add(Image.FromFile(globalConst.AppPath+ "\\img\\rowall.gif"));//this.listView1.SmallImageList
            //this.listView1.LargeImageList = globalConst.Imgs;
            //this.listView1.SmallImageList.ImageSize = new Size(16, 26);
            setInitFromSetString(restr);
            new FTDP.Util.ICSharpTextEditor().Init(this, shuju, false, null);
        }

        private void setInitFromSetString(string setstr)
        {
            listView1.Items.Clear();
            string[] items = setstr.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            al = new ArrayList();
            foreach (string item in items)
            {
                if (item != null && item.Trim().IndexOf("&&&") >= 0)
                {
                    string _item = item.Trim();
                    string openclose = _item.Substring(_item.IndexOf("&&&") + 3);
                    string[] colcfg = _item.Substring(0, _item.IndexOf("&&&")).Split('#');
                    al.Add(new string[] { colcfg[0], functions.str.getDecode(colcfg[1]), colcfg[2], colcfg[3], colcfg[4], colcfg[5], colcfg.Length < 7 ? "" : colcfg[6], openclose, colcfg.Length < 8 ? "0" : colcfg[7] });
                }
            }
            foreach (string[] item in al)
            {
                ListViewItem listview = new ListViewItem(new string[] {
            (item[0].Trim().Equals("")?"Column":(item[0].Trim()))+(item[5].Trim()==""?"":(" - "+item[5].Trim()))+ " "+(this.listView1.Items.Count+1)}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))));
                listview.ImageIndex = 0;
                if (item[8].Equals("1"))
                {
                    listview.ForeColor = Color.LightGray;
                }
                else
                {
                    listview.ForeColor = Color.Black;
                }
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
                label9.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label4.Visible = false;
                //label1.Visible = false;
                shujutip.Visible = false;
                lieming.Visible = false;
                kuandu.Visible = false;
                paixu.Visible = false;
                lianjie.Visible = false;
                target.Visible = false;
                kaiguan.Visible = false;
                shuju.Visible = false;
                //saveto.Visible = false;
                defaulthidden.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
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
            label9.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label4.Visible = true;
            //label1.Visible = true;
            shujutip.Visible = true;
            lieming.Visible = true;
            kuandu.Visible = true;
            paixu.Visible = true;
            lianjie.Visible = true;
            target.Visible = true;
            kaiguan.Visible = true;
            shuju.Visible = true;
            //saveto.Visible = true;
            defaulthidden.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
            lieming.Text = ((string[])al[listView1.SelectedItems[0].Index])[0];
            shuju.Text = ((string[])al[listView1.SelectedItems[0].Index])[1];
            kuandu.Text = ((string[])al[listView1.SelectedItems[0].Index])[2];
            paixu.Text = ((string[])al[listView1.SelectedItems[0].Index])[3];
            lianjie.Text = ((string[])al[listView1.SelectedItems[0].Index])[4];
            target.Text = ((string[])al[listView1.SelectedItems[0].Index])[5];
            saveto.Text = ((string[])al[listView1.SelectedItems[0].Index])[6];
            kaiguan.Text = ((string[])al[listView1.SelectedItems[0].Index])[7];
            defaulthidden.Checked = ((string[])al[listView1.SelectedItems[0].Index])[8].Equals("1");
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
                al[listView1.SelectedItems[0].Index] = new string[] { lieming.Text, shuju.Text, kuandu.Text, paixu.Text, lianjie.Text, target.Text, saveto.Text, kaiguan.Text,defaulthidden.Checked?"1":"0" };
                InitCfgStr();
                listView1.SelectedItems[0].Text = (lieming.Text.Trim().Equals("") ? "Column" : (lieming.Text.Trim() )) + (target.Text.Trim() == "" ? "" : (" - " + target.Text.Trim())) + " " + (listView1.SelectedItems[0].Index+1);
                if (defaulthidden.Checked)
                {
                    listView1.SelectedItems[0].ForeColor = Color.LightGray;
                }
                else
                {
                    listView1.SelectedItems[0].ForeColor = System.Drawing.Color.White;
                }
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
                    listView1.Items[curpos - 1].Text = (o1[0].Trim().Equals("") ? "Column" : (o1[0].Trim() )) + (o1[5].Trim() == "" ? "" : (" - " + o1[5].Trim())) + " " + curpos;
                    listView1.Items[curpos].Text = (o0[0].Trim().Equals("") ? "Column" : (o0[0].Trim() ))+ (o0[5].Trim() == "" ? "" : (" - " + o0[5].Trim())) + " " + (curpos+1);
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
                    listView1.Items[curpos + 1].Text = (o1[0].Trim().Equals("") ? "Column" : (o1[0].Trim())) + (o1[5].Trim() == "" ? "" : (" - " + o1[5].Trim())) + " " + (curpos+2);
                    listView1.Items[curpos].Text = (o0[0].Trim().Equals("") ? "Column" : (o0[0].Trim() )) + (o0[5].Trim() == "" ? "" : (" - " + o0[5].Trim())) + " " + (curpos + 1);
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
            te.basetext = lianjie.Text;
            te.ShowDialog();
            lianjie.Text = te.basetext;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TextEditor te = new TextEditor();
            te.basetext = shuju.Text;
            te.StrictField = true;
            te.ShowDialog();
            shuju.Text = te.basetext;
            lieming_Leave(sender, e);
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
            CusSQL= functions.Adv.SQLPatternOP(CusSQL);
            if (CusSQL!=null&&CusSQL.StartsWith("@From"))
            {
                MsgBox.Information(res.ctl.str("fromNotSql"));
                return;
            }
            if (MainTable==null || CusSQL==null)
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
                    shuju.Text = rs.SelColName;
                    lieming_Leave(sender, e);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string MainTable = classes.PageAsist.GetPartSetValue(partId, "MainTable");
            string CusSQL = classes.PageAsist.GetPartSetValue(partId, "CusSQL");
            CusSQL= functions.Adv.SQLPatternOP(CusSQL);
            if (CusSQL != null && CusSQL.StartsWith("@From"))
            {
                MsgBox.Information(res.ctl.str("fromNotSql"));
                return;
            }
            if (MainTable == null || CusSQL == null)
            {
                functions.MsgBox.Warning(res.ctl.str("List_Api.5"));
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
                rs.al = al;
                rs.ShowDialog();
                if (rs.SetString != null && rs.SetString != "")
                {
                    cfg.Text = rs.SetString;
                    setInitFromSetString(rs.SetString);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (XmlDoc != null)
            {
                string apiset = Page.PageWare.getPartParamValue(XmlDoc, "APISet");
                if (apiset == null) functions.MsgBox.Error("api set error");
                else
                {
                    List_Api list_Api = new List_Api();
                    list_Api.partId =partId;
                    list_Api.restr = apiset;
                    list_Api.ShowDialog();
                    if (!list_Api.IsCancel)
                    {
                        Page.PageWare.setPartParamValue(ref XmlDoc, partId, "APISet", list_Api.restr);
                    }
                }
            }
            else
            {
                string apiset = Page.PageWare.getPartParamValue(partId, "list", "List", "APISet");
                if (apiset == null) functions.MsgBox.Error("api set error");
                else
                {
                    List_Api list_Api = new List_Api();
                    list_Api.partId = partId;
                    list_Api.restr = apiset;
                    list_Api.ShowDialog();
                    if (!list_Api.IsCancel)
                    {
                        Page.PageWare.setPartParamValue(partId, "list", "List", "APISet", list_Api.restr);
                    }
                }
            }
            PropertySpace.Site.PropertyPart.doPartProperty(partId);
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            al.Clear();
            listView1.SelectedItems.Clear();
            while (listView1.Items.Count > 0) listView1.Items.RemoveAt(0);
            InitCfgStr();
        }

        private void copyKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            foreach (string[] row in al)
            {
                sb.Append(row[0]+Environment.NewLine);
            }
            Clipboard.SetText(sb.ToString());
        }

        private void copyItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count>0)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    var ss = al[listView1.SelectedItems[i].Index] as string[];
                    foreach(var s in ss)
                    {
                        sb.Append(s+ "[##1##]");
                    }
                    sb.Append("[##2##]");
                }
                Clipboard.SetText(sb.ToString());
                MsgBox.Information("Copyed "+ listView1.SelectedItems.Count + " items");
            }
            else
            {
                MsgBox.Warning("Please select item");
            }
        }

        private void pasteItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var str = Clipboard.GetText();
            if(str.EndsWith("[##2##]"))
            {
                var ss = str.Split(new string[] { "[##2##]" }, StringSplitOptions.RemoveEmptyEntries);
                foreach(var s in ss)
                {
                    al.Add(s.Split(new string[] { "[##1##]" }, StringSplitOptions.None));
                }
                listView1.SelectedItems.Clear();
                listView1_SelectedIndexChanged(sender, e);
                InitCfgStr();
                listView1.Items.Clear();
                setInitFromSetString(cfg.Text);
                MsgBox.Information("Copyed " + ss.Length + " items");
            }
        }

        private void sortbykeytoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listView1.SelectedItems.Clear();
            listView1_SelectedIndexChanged(sender, e);
            var list = new List<string[]>();
            al = new ArrayList(al.ToArray().Select(r => (string[])r).OrderBy(r => r[0]).ToList());
            InitCfgStr();
            listView1.Items.Clear();
            setInitFromSetString(cfg.Text);
        }
    }

       
}
