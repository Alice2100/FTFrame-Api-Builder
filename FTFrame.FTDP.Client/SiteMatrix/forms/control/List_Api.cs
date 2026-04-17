using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FTDPClient.consts;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace FTDPClient.forms
{
	public class List_Api : System.Windows.Forms.Form
	{
        public string partId = null;
        public string restr = "";
        public bool IsCancel = true;
        private Button Close;
        private Button OK;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private Button button1;
        private Button button2;
        private Label label2;
        private ArrayList al;
        private TextBox luyou;
        private TextBox cfg;
        private Button button3;
        private Button button4;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem delToolStripMenuItem;
        private ToolStripMenuItem upToolStripMenuItem;
        private ToolStripMenuItem downToolStripMenuItem;
        private Label label1;
        private TextBox miaosu;
        private CheckBox orderByCK;
        private Label label9;
        private Label label10;
        private Label label11;
        private TextBox orderByC1;
        private TextBox orderByC2;
        private TextBox orderTypeC2;
        private TextBox orderTypeC1;
        private CheckBox orderTypeCK;
        private TextBox schStrictC2;
        private TextBox schStrictC1;
        private CheckBox schStrictCK;
        private TextBox schTextC2;
        private TextBox schTextC1;
        private CheckBox schTextCK;
        private TextBox pageNumC2;
        private TextBox pageNumC1;
        private CheckBox pageNumCK;
        private TextBox numTypeC2;
        private TextBox numTypeC1;
        private CheckBox numTypeCK;
        private TextBox pageSizeC2;
        private TextBox pageSizeC1;
        private CheckBox pageSizeCK;
        private TextBox schAdvC2;
        private TextBox schAdvC1;
        private CheckBox schAdvCK;
        private TextBox stat3C2;
        private CheckBox stat3CK;
        private TextBox stat2C2;
        private CheckBox stat2CK;
        private TextBox stat1C2;
        private CheckBox stat1CK;
        private TextBox keyValueC2;
        private TextBox keyValueC1;
        private CheckBox keyValueCK;
        private TextBox stat1C1;
        private TextBox stat2C1;
        private TextBox stat3C1;
        private TextBox stat1Para;
        private TextBox stat2Para;
        private TextBox stat3Para;
        private RadioButton inputJson;
        private RadioButton inputForm;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem deleteAllToolStripMenuItem;
        private Label label3;
        private TextBox ExportFileName;
        private IContainer components;

        public List_Api()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();
            ApplyLanguage();
            //
            // TODO: 在 InitializeComponent 调用后添加任何构造函数代码
            //
            /*
             {$$GetList[##]得到客户列表[##]orderby,ordertype,keyvalue.fid[##]des1[#]des2[##]说明1[#]说明2
             {$$}ClientDel[##]客户删除[##]keyvalue.fid,status1.stat,status2.modtime[##]des1[#]des2[#]des3[##]说明1[#]说明2[#]说明3
             */
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
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.luyou = new System.Windows.Forms.TextBox();
            this.cfg = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.miaosu = new System.Windows.Forms.TextBox();
            this.orderByCK = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.orderByC1 = new System.Windows.Forms.TextBox();
            this.orderByC2 = new System.Windows.Forms.TextBox();
            this.orderTypeC2 = new System.Windows.Forms.TextBox();
            this.orderTypeC1 = new System.Windows.Forms.TextBox();
            this.orderTypeCK = new System.Windows.Forms.CheckBox();
            this.schStrictC2 = new System.Windows.Forms.TextBox();
            this.schStrictC1 = new System.Windows.Forms.TextBox();
            this.schStrictCK = new System.Windows.Forms.CheckBox();
            this.schTextC2 = new System.Windows.Forms.TextBox();
            this.schTextC1 = new System.Windows.Forms.TextBox();
            this.schTextCK = new System.Windows.Forms.CheckBox();
            this.pageNumC2 = new System.Windows.Forms.TextBox();
            this.pageNumC1 = new System.Windows.Forms.TextBox();
            this.pageNumCK = new System.Windows.Forms.CheckBox();
            this.numTypeC2 = new System.Windows.Forms.TextBox();
            this.numTypeC1 = new System.Windows.Forms.TextBox();
            this.numTypeCK = new System.Windows.Forms.CheckBox();
            this.pageSizeC2 = new System.Windows.Forms.TextBox();
            this.pageSizeC1 = new System.Windows.Forms.TextBox();
            this.pageSizeCK = new System.Windows.Forms.CheckBox();
            this.schAdvC2 = new System.Windows.Forms.TextBox();
            this.schAdvC1 = new System.Windows.Forms.TextBox();
            this.schAdvCK = new System.Windows.Forms.CheckBox();
            this.stat3C2 = new System.Windows.Forms.TextBox();
            this.stat3CK = new System.Windows.Forms.CheckBox();
            this.stat2C2 = new System.Windows.Forms.TextBox();
            this.stat2CK = new System.Windows.Forms.CheckBox();
            this.stat1C2 = new System.Windows.Forms.TextBox();
            this.stat1CK = new System.Windows.Forms.CheckBox();
            this.keyValueC2 = new System.Windows.Forms.TextBox();
            this.keyValueC1 = new System.Windows.Forms.TextBox();
            this.keyValueCK = new System.Windows.Forms.CheckBox();
            this.stat1C1 = new System.Windows.Forms.TextBox();
            this.stat2C1 = new System.Windows.Forms.TextBox();
            this.stat3C1 = new System.Windows.Forms.TextBox();
            this.stat1Para = new System.Windows.Forms.TextBox();
            this.stat2Para = new System.Windows.Forms.TextBox();
            this.stat3Para = new System.Windows.Forms.TextBox();
            this.inputJson = new System.Windows.Forms.RadioButton();
            this.inputForm = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.ExportFileName = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Close
            // 
            this.Close.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Close.Location = new System.Drawing.Point(655, 498);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 30);
            this.Close.TabIndex = 5;
            this.Close.Text = "&Close";
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // OK
            // 
            this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OK.Location = new System.Drawing.Point(562, 498);
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
            this.listView1.Size = new System.Drawing.Size(156, 312);
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
            this.downToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(132, 120);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // delToolStripMenuItem
            // 
            this.delToolStripMenuItem.Name = "delToolStripMenuItem";
            this.delToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.delToolStripMenuItem.Text = "Del";
            this.delToolStripMenuItem.Click += new System.EventHandler(this.delToolStripMenuItem_Click);
            // 
            // upToolStripMenuItem
            // 
            this.upToolStripMenuItem.Name = "upToolStripMenuItem";
            this.upToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.upToolStripMenuItem.Text = "Up";
            this.upToolStripMenuItem.Click += new System.EventHandler(this.upToolStripMenuItem_Click);
            // 
            // downToolStripMenuItem
            // 
            this.downToolStripMenuItem.Name = "downToolStripMenuItem";
            this.downToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.downToolStripMenuItem.Text = "Down";
            this.downToolStripMenuItem.Click += new System.EventHandler(this.downToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(128, 6);
            // 
            // deleteAllToolStripMenuItem
            // 
            this.deleteAllToolStripMenuItem.Name = "deleteAllToolStripMenuItem";
            this.deleteAllToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.deleteAllToolStripMenuItem.Text = "Delete All";
            this.deleteAllToolStripMenuItem.Click += new System.EventHandler(this.deleteAllToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(12, 330);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 24);
            this.button1.TabIndex = 8;
            this.button1.Text = "&Add";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(52, 330);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(36, 24);
            this.button2.TabIndex = 9;
            this.button2.Text = "&Del";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "接口路由";
            // 
            // luyou
            // 
            this.luyou.Font = new System.Drawing.Font("宋体", 12F);
            this.luyou.Location = new System.Drawing.Point(244, 9);
            this.luyou.Name = "luyou";
            this.luyou.Size = new System.Drawing.Size(157, 26);
            this.luyou.TabIndex = 22;
            this.luyou.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // cfg
            // 
            this.cfg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.cfg.Location = new System.Drawing.Point(12, 360);
            this.cfg.Multiline = true;
            this.cfg.Name = "cfg";
            this.cfg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cfg.Size = new System.Drawing.Size(156, 132);
            this.cfg.TabIndex = 30;
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(92, 330);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(36, 24);
            this.button3.TabIndex = 34;
            this.button3.Text = "&Up";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Location = new System.Drawing.Point(132, 330);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(36, 24);
            this.button4.TabIndex = 35;
            this.button4.Text = "Do&w";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(410, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 40;
            this.label1.Text = "接口描述";
            // 
            // miaosu
            // 
            this.miaosu.Font = new System.Drawing.Font("宋体", 12F);
            this.miaosu.Location = new System.Drawing.Point(469, 9);
            this.miaosu.Name = "miaosu";
            this.miaosu.Size = new System.Drawing.Size(261, 26);
            this.miaosu.TabIndex = 45;
            this.miaosu.DoubleClick += new System.EventHandler(this.miaosu_DoubleClick);
            this.miaosu.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // orderByCK
            // 
            this.orderByCK.AutoSize = true;
            this.orderByCK.Font = new System.Drawing.Font("宋体", 12F);
            this.orderByCK.Location = new System.Drawing.Point(187, 71);
            this.orderByCK.Name = "orderByCK";
            this.orderByCK.Size = new System.Drawing.Size(82, 20);
            this.orderByCK.TabIndex = 46;
            this.orderByCK.Text = "orderBy";
            this.orderByCK.UseVisualStyleBackColor = true;
            this.orderByCK.Click += new System.EventHandler(this.orderByCK_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label9.Location = new System.Drawing.Point(185, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 47;
            this.label9.Text = "输入";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label10.Location = new System.Drawing.Point(283, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 12);
            this.label10.TabIndex = 48;
            this.label10.Text = "参考值/数据列";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label11.Location = new System.Drawing.Point(410, 45);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 49;
            this.label11.Text = "说明";
            // 
            // orderByC1
            // 
            this.orderByC1.Font = new System.Drawing.Font("宋体", 12F);
            this.orderByC1.Location = new System.Drawing.Point(285, 69);
            this.orderByC1.Name = "orderByC1";
            this.orderByC1.Size = new System.Drawing.Size(116, 26);
            this.orderByC1.TabIndex = 50;
            this.orderByC1.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // orderByC2
            // 
            this.orderByC2.Font = new System.Drawing.Font("宋体", 12F);
            this.orderByC2.Location = new System.Drawing.Point(412, 69);
            this.orderByC2.Name = "orderByC2";
            this.orderByC2.Size = new System.Drawing.Size(318, 26);
            this.orderByC2.TabIndex = 51;
            this.orderByC2.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // orderTypeC2
            // 
            this.orderTypeC2.Font = new System.Drawing.Font("宋体", 12F);
            this.orderTypeC2.Location = new System.Drawing.Point(412, 105);
            this.orderTypeC2.Name = "orderTypeC2";
            this.orderTypeC2.Size = new System.Drawing.Size(318, 26);
            this.orderTypeC2.TabIndex = 54;
            this.orderTypeC2.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // orderTypeC1
            // 
            this.orderTypeC1.Font = new System.Drawing.Font("宋体", 12F);
            this.orderTypeC1.Location = new System.Drawing.Point(285, 105);
            this.orderTypeC1.Name = "orderTypeC1";
            this.orderTypeC1.Size = new System.Drawing.Size(116, 26);
            this.orderTypeC1.TabIndex = 53;
            this.orderTypeC1.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // orderTypeCK
            // 
            this.orderTypeCK.AutoSize = true;
            this.orderTypeCK.Font = new System.Drawing.Font("宋体", 12F);
            this.orderTypeCK.Location = new System.Drawing.Point(187, 107);
            this.orderTypeCK.Name = "orderTypeCK";
            this.orderTypeCK.Size = new System.Drawing.Size(98, 20);
            this.orderTypeCK.TabIndex = 52;
            this.orderTypeCK.Text = "orderType";
            this.orderTypeCK.UseVisualStyleBackColor = true;
            this.orderTypeCK.Click += new System.EventHandler(this.orderTypeCK_Click);
            // 
            // schStrictC2
            // 
            this.schStrictC2.Font = new System.Drawing.Font("宋体", 12F);
            this.schStrictC2.Location = new System.Drawing.Point(412, 177);
            this.schStrictC2.Name = "schStrictC2";
            this.schStrictC2.Size = new System.Drawing.Size(318, 26);
            this.schStrictC2.TabIndex = 60;
            this.schStrictC2.DoubleClick += new System.EventHandler(this.schStrictC2_DoubleClick);
            this.schStrictC2.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // schStrictC1
            // 
            this.schStrictC1.Font = new System.Drawing.Font("宋体", 12F);
            this.schStrictC1.Location = new System.Drawing.Point(285, 177);
            this.schStrictC1.Name = "schStrictC1";
            this.schStrictC1.Size = new System.Drawing.Size(116, 26);
            this.schStrictC1.TabIndex = 59;
            this.schStrictC1.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // schStrictCK
            // 
            this.schStrictCK.AutoSize = true;
            this.schStrictCK.Font = new System.Drawing.Font("宋体", 12F);
            this.schStrictCK.Location = new System.Drawing.Point(187, 179);
            this.schStrictCK.Name = "schStrictCK";
            this.schStrictCK.Size = new System.Drawing.Size(98, 20);
            this.schStrictCK.TabIndex = 58;
            this.schStrictCK.Text = "schStrict";
            this.schStrictCK.UseVisualStyleBackColor = true;
            this.schStrictCK.Click += new System.EventHandler(this.schStrictCK_Click);
            // 
            // schTextC2
            // 
            this.schTextC2.Font = new System.Drawing.Font("宋体", 12F);
            this.schTextC2.Location = new System.Drawing.Point(412, 141);
            this.schTextC2.Name = "schTextC2";
            this.schTextC2.Size = new System.Drawing.Size(318, 26);
            this.schTextC2.TabIndex = 57;
            this.schTextC2.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // schTextC1
            // 
            this.schTextC1.Font = new System.Drawing.Font("宋体", 12F);
            this.schTextC1.Location = new System.Drawing.Point(285, 141);
            this.schTextC1.Name = "schTextC1";
            this.schTextC1.Size = new System.Drawing.Size(116, 26);
            this.schTextC1.TabIndex = 56;
            this.schTextC1.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // schTextCK
            // 
            this.schTextCK.AutoSize = true;
            this.schTextCK.Font = new System.Drawing.Font("宋体", 12F);
            this.schTextCK.Location = new System.Drawing.Point(187, 143);
            this.schTextCK.Name = "schTextCK";
            this.schTextCK.Size = new System.Drawing.Size(82, 20);
            this.schTextCK.TabIndex = 55;
            this.schTextCK.Text = "schText";
            this.schTextCK.UseVisualStyleBackColor = true;
            this.schTextCK.Click += new System.EventHandler(this.schTextCK_Click);
            // 
            // pageNumC2
            // 
            this.pageNumC2.Font = new System.Drawing.Font("宋体", 12F);
            this.pageNumC2.Location = new System.Drawing.Point(412, 321);
            this.pageNumC2.Name = "pageNumC2";
            this.pageNumC2.Size = new System.Drawing.Size(318, 26);
            this.pageNumC2.TabIndex = 72;
            this.pageNumC2.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // pageNumC1
            // 
            this.pageNumC1.Font = new System.Drawing.Font("宋体", 12F);
            this.pageNumC1.Location = new System.Drawing.Point(285, 321);
            this.pageNumC1.Name = "pageNumC1";
            this.pageNumC1.Size = new System.Drawing.Size(116, 26);
            this.pageNumC1.TabIndex = 71;
            this.pageNumC1.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // pageNumCK
            // 
            this.pageNumCK.AutoSize = true;
            this.pageNumCK.Font = new System.Drawing.Font("宋体", 12F);
            this.pageNumCK.Location = new System.Drawing.Point(187, 323);
            this.pageNumCK.Name = "pageNumCK";
            this.pageNumCK.Size = new System.Drawing.Size(82, 20);
            this.pageNumCK.TabIndex = 70;
            this.pageNumCK.Text = "pageNum";
            this.pageNumCK.UseVisualStyleBackColor = true;
            this.pageNumCK.Click += new System.EventHandler(this.pageNumCK_Click);
            // 
            // numTypeC2
            // 
            this.numTypeC2.Font = new System.Drawing.Font("宋体", 12F);
            this.numTypeC2.Location = new System.Drawing.Point(412, 285);
            this.numTypeC2.Name = "numTypeC2";
            this.numTypeC2.Size = new System.Drawing.Size(318, 26);
            this.numTypeC2.TabIndex = 69;
            this.numTypeC2.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // numTypeC1
            // 
            this.numTypeC1.Font = new System.Drawing.Font("宋体", 12F);
            this.numTypeC1.Location = new System.Drawing.Point(285, 285);
            this.numTypeC1.Name = "numTypeC1";
            this.numTypeC1.Size = new System.Drawing.Size(116, 26);
            this.numTypeC1.TabIndex = 68;
            this.numTypeC1.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // numTypeCK
            // 
            this.numTypeCK.AutoSize = true;
            this.numTypeCK.Font = new System.Drawing.Font("宋体", 12F);
            this.numTypeCK.Location = new System.Drawing.Point(187, 287);
            this.numTypeCK.Name = "numTypeCK";
            this.numTypeCK.Size = new System.Drawing.Size(82, 20);
            this.numTypeCK.TabIndex = 67;
            this.numTypeCK.Text = "numType";
            this.numTypeCK.UseVisualStyleBackColor = true;
            this.numTypeCK.Click += new System.EventHandler(this.numTypeCK_Click);
            // 
            // pageSizeC2
            // 
            this.pageSizeC2.Font = new System.Drawing.Font("宋体", 12F);
            this.pageSizeC2.Location = new System.Drawing.Point(412, 249);
            this.pageSizeC2.Name = "pageSizeC2";
            this.pageSizeC2.Size = new System.Drawing.Size(318, 26);
            this.pageSizeC2.TabIndex = 66;
            this.pageSizeC2.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // pageSizeC1
            // 
            this.pageSizeC1.Font = new System.Drawing.Font("宋体", 12F);
            this.pageSizeC1.Location = new System.Drawing.Point(285, 249);
            this.pageSizeC1.Name = "pageSizeC1";
            this.pageSizeC1.Size = new System.Drawing.Size(116, 26);
            this.pageSizeC1.TabIndex = 65;
            this.pageSizeC1.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // pageSizeCK
            // 
            this.pageSizeCK.AutoSize = true;
            this.pageSizeCK.Font = new System.Drawing.Font("宋体", 12F);
            this.pageSizeCK.Location = new System.Drawing.Point(187, 251);
            this.pageSizeCK.Name = "pageSizeCK";
            this.pageSizeCK.Size = new System.Drawing.Size(90, 20);
            this.pageSizeCK.TabIndex = 64;
            this.pageSizeCK.Text = "pageSize";
            this.pageSizeCK.UseVisualStyleBackColor = true;
            this.pageSizeCK.Click += new System.EventHandler(this.pageSizeCK_Click);
            // 
            // schAdvC2
            // 
            this.schAdvC2.Font = new System.Drawing.Font("宋体", 12F);
            this.schAdvC2.Location = new System.Drawing.Point(412, 213);
            this.schAdvC2.Name = "schAdvC2";
            this.schAdvC2.Size = new System.Drawing.Size(318, 26);
            this.schAdvC2.TabIndex = 63;
            this.schAdvC2.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // schAdvC1
            // 
            this.schAdvC1.Font = new System.Drawing.Font("宋体", 12F);
            this.schAdvC1.Location = new System.Drawing.Point(285, 213);
            this.schAdvC1.Name = "schAdvC1";
            this.schAdvC1.Size = new System.Drawing.Size(116, 26);
            this.schAdvC1.TabIndex = 62;
            this.schAdvC1.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // schAdvCK
            // 
            this.schAdvCK.AutoSize = true;
            this.schAdvCK.Font = new System.Drawing.Font("宋体", 12F);
            this.schAdvCK.Location = new System.Drawing.Point(187, 215);
            this.schAdvCK.Name = "schAdvCK";
            this.schAdvCK.Size = new System.Drawing.Size(74, 20);
            this.schAdvCK.TabIndex = 61;
            this.schAdvCK.Text = "schAdv";
            this.schAdvCK.UseVisualStyleBackColor = true;
            this.schAdvCK.Click += new System.EventHandler(this.schAdvCK_Click);
            // 
            // stat3C2
            // 
            this.stat3C2.Font = new System.Drawing.Font("宋体", 12F);
            this.stat3C2.Location = new System.Drawing.Point(412, 466);
            this.stat3C2.Name = "stat3C2";
            this.stat3C2.Size = new System.Drawing.Size(318, 26);
            this.stat3C2.TabIndex = 84;
            this.stat3C2.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // stat3CK
            // 
            this.stat3CK.AutoSize = true;
            this.stat3CK.Font = new System.Drawing.Font("宋体", 12F);
            this.stat3CK.Location = new System.Drawing.Point(187, 470);
            this.stat3CK.Name = "stat3CK";
            this.stat3CK.Size = new System.Drawing.Size(15, 14);
            this.stat3CK.TabIndex = 82;
            this.stat3CK.UseVisualStyleBackColor = true;
            this.stat3CK.Click += new System.EventHandler(this.stat3CK_Click);
            // 
            // stat2C2
            // 
            this.stat2C2.Font = new System.Drawing.Font("宋体", 12F);
            this.stat2C2.Location = new System.Drawing.Point(412, 430);
            this.stat2C2.Name = "stat2C2";
            this.stat2C2.Size = new System.Drawing.Size(318, 26);
            this.stat2C2.TabIndex = 81;
            this.stat2C2.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // stat2CK
            // 
            this.stat2CK.AutoSize = true;
            this.stat2CK.Font = new System.Drawing.Font("宋体", 12F);
            this.stat2CK.Location = new System.Drawing.Point(187, 434);
            this.stat2CK.Name = "stat2CK";
            this.stat2CK.Size = new System.Drawing.Size(15, 14);
            this.stat2CK.TabIndex = 79;
            this.stat2CK.UseVisualStyleBackColor = true;
            this.stat2CK.Click += new System.EventHandler(this.stat2CK_Click);
            // 
            // stat1C2
            // 
            this.stat1C2.Font = new System.Drawing.Font("宋体", 12F);
            this.stat1C2.Location = new System.Drawing.Point(412, 394);
            this.stat1C2.Name = "stat1C2";
            this.stat1C2.Size = new System.Drawing.Size(318, 26);
            this.stat1C2.TabIndex = 78;
            this.stat1C2.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // stat1CK
            // 
            this.stat1CK.AutoSize = true;
            this.stat1CK.Font = new System.Drawing.Font("宋体", 12F);
            this.stat1CK.Location = new System.Drawing.Point(187, 398);
            this.stat1CK.Name = "stat1CK";
            this.stat1CK.Size = new System.Drawing.Size(15, 14);
            this.stat1CK.TabIndex = 76;
            this.stat1CK.UseVisualStyleBackColor = true;
            this.stat1CK.Click += new System.EventHandler(this.stat1CK_Click);
            // 
            // keyValueC2
            // 
            this.keyValueC2.Font = new System.Drawing.Font("宋体", 12F);
            this.keyValueC2.Location = new System.Drawing.Point(412, 358);
            this.keyValueC2.Name = "keyValueC2";
            this.keyValueC2.Size = new System.Drawing.Size(318, 26);
            this.keyValueC2.TabIndex = 75;
            this.keyValueC2.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // keyValueC1
            // 
            this.keyValueC1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.keyValueC1.Font = new System.Drawing.Font("宋体", 12F);
            this.keyValueC1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.keyValueC1.Location = new System.Drawing.Point(285, 358);
            this.keyValueC1.Name = "keyValueC1";
            this.keyValueC1.Size = new System.Drawing.Size(116, 26);
            this.keyValueC1.TabIndex = 74;
            this.keyValueC1.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // keyValueCK
            // 
            this.keyValueCK.AutoSize = true;
            this.keyValueCK.Font = new System.Drawing.Font("宋体", 12F);
            this.keyValueCK.Location = new System.Drawing.Point(187, 360);
            this.keyValueCK.Name = "keyValueCK";
            this.keyValueCK.Size = new System.Drawing.Size(90, 20);
            this.keyValueCK.TabIndex = 73;
            this.keyValueCK.Text = "keyValue";
            this.keyValueCK.UseVisualStyleBackColor = true;
            this.keyValueCK.Click += new System.EventHandler(this.keyValueCK_Click);
            // 
            // stat1C1
            // 
            this.stat1C1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.stat1C1.Font = new System.Drawing.Font("宋体", 12F);
            this.stat1C1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.stat1C1.Location = new System.Drawing.Point(285, 394);
            this.stat1C1.Name = "stat1C1";
            this.stat1C1.Size = new System.Drawing.Size(116, 26);
            this.stat1C1.TabIndex = 85;
            this.stat1C1.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // stat2C1
            // 
            this.stat2C1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.stat2C1.Font = new System.Drawing.Font("宋体", 12F);
            this.stat2C1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.stat2C1.Location = new System.Drawing.Point(285, 430);
            this.stat2C1.Name = "stat2C1";
            this.stat2C1.Size = new System.Drawing.Size(116, 26);
            this.stat2C1.TabIndex = 86;
            this.stat2C1.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // stat3C1
            // 
            this.stat3C1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.stat3C1.Font = new System.Drawing.Font("宋体", 12F);
            this.stat3C1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.stat3C1.Location = new System.Drawing.Point(285, 466);
            this.stat3C1.Name = "stat3C1";
            this.stat3C1.Size = new System.Drawing.Size(116, 26);
            this.stat3C1.TabIndex = 87;
            this.stat3C1.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // stat1Para
            // 
            this.stat1Para.Font = new System.Drawing.Font("宋体", 12F);
            this.stat1Para.Location = new System.Drawing.Point(203, 394);
            this.stat1Para.Name = "stat1Para";
            this.stat1Para.Size = new System.Drawing.Size(75, 26);
            this.stat1Para.TabIndex = 88;
            this.stat1Para.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // stat2Para
            // 
            this.stat2Para.Font = new System.Drawing.Font("宋体", 12F);
            this.stat2Para.Location = new System.Drawing.Point(203, 430);
            this.stat2Para.Name = "stat2Para";
            this.stat2Para.Size = new System.Drawing.Size(75, 26);
            this.stat2Para.TabIndex = 89;
            this.stat2Para.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // stat3Para
            // 
            this.stat3Para.Font = new System.Drawing.Font("宋体", 12F);
            this.stat3Para.Location = new System.Drawing.Point(203, 466);
            this.stat3Para.Name = "stat3Para";
            this.stat3Para.Size = new System.Drawing.Size(75, 26);
            this.stat3Para.TabIndex = 90;
            this.stat3Para.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // inputJson
            // 
            this.inputJson.AutoSize = true;
            this.inputJson.Checked = true;
            this.inputJson.Location = new System.Drawing.Point(264, 505);
            this.inputJson.Name = "inputJson";
            this.inputJson.Size = new System.Drawing.Size(71, 16);
            this.inputJson.TabIndex = 91;
            this.inputJson.TabStop = true;
            this.inputJson.Text = "Json输入";
            this.inputJson.UseVisualStyleBackColor = true;
            this.inputJson.Click += new System.EventHandler(this.inputJson_Click);
            // 
            // inputForm
            // 
            this.inputForm.AutoSize = true;
            this.inputForm.Location = new System.Drawing.Point(376, 505);
            this.inputForm.Name = "inputForm";
            this.inputForm.Size = new System.Drawing.Size(71, 16);
            this.inputForm.TabIndex = 92;
            this.inputForm.Text = "Form输入";
            this.inputForm.UseVisualStyleBackColor = true;
            this.inputForm.Click += new System.EventHandler(this.inputForm_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 507);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 93;
            this.label3.Text = "导出文件名";
            // 
            // ExportFileName
            // 
            this.ExportFileName.Font = new System.Drawing.Font("宋体", 12F);
            this.ExportFileName.Location = new System.Drawing.Point(128, 499);
            this.ExportFileName.Name = "ExportFileName";
            this.ExportFileName.Size = new System.Drawing.Size(116, 26);
            this.ExportFileName.TabIndex = 94;
            this.ExportFileName.Leave += new System.EventHandler(this.lieming_Leave);
            // 
            // List_Api
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(738, 533);
            this.Controls.Add(this.ExportFileName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.inputForm);
            this.Controls.Add(this.inputJson);
            this.Controls.Add(this.stat3Para);
            this.Controls.Add(this.stat2Para);
            this.Controls.Add(this.stat1Para);
            this.Controls.Add(this.stat3C1);
            this.Controls.Add(this.stat2C1);
            this.Controls.Add(this.stat1C1);
            this.Controls.Add(this.stat3C2);
            this.Controls.Add(this.stat3CK);
            this.Controls.Add(this.stat2C2);
            this.Controls.Add(this.stat2CK);
            this.Controls.Add(this.stat1C2);
            this.Controls.Add(this.stat1CK);
            this.Controls.Add(this.keyValueC2);
            this.Controls.Add(this.keyValueC1);
            this.Controls.Add(this.keyValueCK);
            this.Controls.Add(this.pageNumC2);
            this.Controls.Add(this.pageNumC1);
            this.Controls.Add(this.pageNumCK);
            this.Controls.Add(this.numTypeC2);
            this.Controls.Add(this.numTypeC1);
            this.Controls.Add(this.numTypeCK);
            this.Controls.Add(this.pageSizeC2);
            this.Controls.Add(this.pageSizeC1);
            this.Controls.Add(this.pageSizeCK);
            this.Controls.Add(this.schAdvC2);
            this.Controls.Add(this.schAdvC1);
            this.Controls.Add(this.schAdvCK);
            this.Controls.Add(this.schStrictC2);
            this.Controls.Add(this.schStrictC1);
            this.Controls.Add(this.schStrictCK);
            this.Controls.Add(this.schTextC2);
            this.Controls.Add(this.schTextC1);
            this.Controls.Add(this.schTextCK);
            this.Controls.Add(this.orderTypeC2);
            this.Controls.Add(this.orderTypeC1);
            this.Controls.Add(this.orderTypeCK);
            this.Controls.Add(this.orderByC2);
            this.Controls.Add(this.orderByC1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.orderByCK);
            this.Controls.Add(this.miaosu);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.cfg);
            this.Controls.Add(this.luyou);
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
            this.Name = "List_Api";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "列表 Api";
            this.Load += new System.EventHandler(this.BackValue_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RowAll_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
#endregion
        private void ApplyLanguage()
        {
            //OK.Text = res.About.GetString("String1");
           // Close.Text = res.About.GetString("String2");
           
        }
        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            al.Clear();
            restr = cfg.Text;
            IsCancel = false;
            this.Close();
        }
        private void InitCfgStr()
        {
            string str = "";
            foreach (string[] item in al)
            {
                if (!item[0].Trim().Equals(""))
                {
                    str += "{$$}" + item[0].Replace("\r\n", "") + "[##]" + item[1].Replace("\r\n", "") + "[##]" + item[2].Replace("\r\n", "") + "[##]" + item[3].Replace("\r\n", "") + "[##]" + item[4].Replace("\r\n", "") +"[##]"+item[5] + "[##]" + item[6];
                }
            }
            if (!str.Equals("")) str = str.Substring(4);
            cfg.Text = str;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string NewName = "GetList_" + (this.listView1.Items.Count + 1);
            string NewNameCap = res.ctl.str("List_Api.1")+"_" + (this.listView1.Items.Count + 1);
            ListViewItem listview = new ListViewItem(new string[] {NewName}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))));
            listview.ImageIndex = 26;
            this.listView1.Items.Add(listview);
            al.Add(new string[] { NewName, NewNameCap, "orderBy,orderType,schText,schStrict,pageSize,numType,pageNum", "[#][#][#][#]12[#][#]", res.ctl.str("List_Api.2"),"json","" });
            listView1.SelectedItems.Clear();
            InitCfgStr();
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
            this.Text = res.ctl.str("List_Api.text");           //列表 Api
            label2.Text = res.ctl.str("List_Api.label2");           //接口路由
            label1.Text = res.ctl.str("List_Api.label1");           //接口描述
            label9.Text = res.ctl.str("List_Api.label9");           //输入
            label10.Text = res.ctl.str("List_Api.label10");         //参考值/数据列
            label11.Text = res.ctl.str("List_Api.label11");			//说明
            inputJson.Text = res.ctl.str("JsonInput");
            inputForm.Text = res.ctl.str("FormInput");
            label3.Text = res.ctl.str("List_Api_ExportFilename");
            cfg.Text = restr;
            this.listView1.SmallImageList = globalConst.Imgs;
            this.listView1.LargeImageList = globalConst.Imgs;
            setInitFromSetString(restr);
        }

        private void setInitFromSetString(string setstr)
        {
            listView1.Items.Clear();
            string[] items = setstr.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
            al = new ArrayList();
            foreach (string item in items)
            {
                string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                if(colcfg.Length==5)
                {
                    var list = new List<string>();
                    list.AddRange(colcfg);
                    list.Add("json");
                    list.Add("");//keyvalue [::] [;;]
                    colcfg = list.ToArray();
                }
                else if (colcfg.Length == 6)
                {
                    var list = new List<string>();
                    list.AddRange(colcfg);
                    list.Add("");//keyvalue [::] [;;]
                    colcfg = list.ToArray();
                }
                al.Add(colcfg);
            }
            foreach (string[] item in al)
            {
                ListViewItem listview = new ListViewItem(new string[] {
            (item[0].Trim().Equals("")?"GetList_"+(this.listView1.Items.Count+1):(item[0].Trim()))}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))));
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
                label1.Visible = false;
                label2.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                label11.Visible = false;
                luyou.Visible = false;
                miaosu.Visible = false;
                orderByCK.Visible = false; orderByC1.Visible = false; orderByC2.Visible = false;
                orderTypeCK.Visible = false; orderTypeC1.Visible = false; orderTypeC2.Visible = false;
                schTextCK.Visible = false; schTextC1.Visible = false; schTextC2.Visible = false;
                schStrictCK.Visible = false; schStrictC1.Visible = false; schStrictC2.Visible = false;
                schAdvCK.Visible = false; schAdvC1.Visible = false; schAdvC2.Visible = false;
                pageSizeCK.Visible = false; pageSizeC1.Visible = false; pageSizeC2.Visible = false;
                numTypeCK.Visible = false; numTypeC1.Visible = false; numTypeC2.Visible = false;
                pageNumCK.Visible = false; pageNumC1.Visible = false; pageNumC2.Visible = false;
                keyValueCK.Visible = false; keyValueC1.Visible = false; keyValueC2.Visible = false;
                stat1CK.Visible = false; stat1C1.Visible = false; stat1C2.Visible = false;stat1Para.Visible = false;
                stat2CK.Visible = false; stat2C1.Visible = false; stat2C2.Visible = false; stat2Para.Visible = false;
                stat3CK.Visible = false; stat3C1.Visible = false; stat3C2.Visible = false; stat3Para.Visible = false;
                inputJson.Visible = false;inputForm.Visible = false;
                ExportFileName.Visible = false;
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
            label1.Visible = true;
            label2.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
            luyou.Visible = true;
            miaosu.Visible = true;
            orderByCK.Visible = true; orderByC1.Visible = true; orderByC2.Visible = true;
            orderTypeCK.Visible = true; orderTypeC1.Visible = true; orderTypeC2.Visible = true;
            schTextCK.Visible = true; schTextC1.Visible = true; schTextC2.Visible = true;
            schStrictCK.Visible = true; schStrictC1.Visible = true; schStrictC2.Visible = true;
            schAdvCK.Visible = true; schAdvC1.Visible = true; schAdvC2.Visible = true;
            pageSizeCK.Visible = true; pageSizeC1.Visible = true; pageSizeC2.Visible = true;
            numTypeCK.Visible = true; numTypeC1.Visible = true; numTypeC2.Visible = true;
            pageNumCK.Visible = true; pageNumC1.Visible = true; pageNumC2.Visible = true;
            keyValueCK.Visible = true; keyValueC1.Visible = true; keyValueC2.Visible = true;
            stat1CK.Visible = true; stat1C1.Visible = true; stat1C2.Visible = true; stat1Para.Visible = true;
            stat2CK.Visible = true; stat2C1.Visible = true; stat2C2.Visible = true; stat2Para.Visible = true;
            stat3CK.Visible = true; stat3C1.Visible = true; stat3C2.Visible = true; stat3Para.Visible = true;
            inputJson.Visible = true; inputForm.Visible = true;
            ExportFileName.Visible = true;
            CK_orderBy(false); 
            CK_orderType(false);
            CK_schText(false);
            CK_schStrict(false);
            CK_schAdv(false);
            CK_pageSize(false);
            CK_numType(false);
            CK_pageNum(false);
            CK_keyValue(false);
            CK_stat1(false);
            CK_stat2(false);
            CK_stat3(false);
            string[] item = ((string[])al[listView1.SelectedItems[0].Index]);
            luyou.Text = item[0];
            miaosu.Text = item[1];
            string[] keys = item[2].Split(new string[] { ","},StringSplitOptions.None);
            string[] cankaos = item[3].Split(new string[] { "[#]" }, StringSplitOptions.None);
            string[] shuomings = item[4].Split(new string[] { "[#]" }, StringSplitOptions.None);
            string inputType = item[5];
            if (inputType == "form") inputForm.Checked = true;
            else inputJson.Checked = true;
            var dic = setDicFromStr(item[6]);
            ExportFileName.Text = dic.ContainsKey("ExportFileName") ? dic["ExportFileName"] : "";
            int StatCount = 0;
            for(int i = 0; i < keys.Length; i++) {
                if(keys[i]== "orderBy")
                {
                    orderByC1.Text = cankaos[i];
                    orderByC2.Text = shuomings[i];
                    CK_orderBy(true);
                }
                else if (keys[i] == "orderType")
                {
                    orderTypeC1.Text = cankaos[i];
                    orderTypeC2.Text = shuomings[i];
                    CK_orderType(true);
                }
                else if (keys[i] == "schText")
                {
                    schTextC1.Text = cankaos[i];
                    schTextC2.Text = shuomings[i];
                    CK_schText(true);
                }
                else if (keys[i] == "schStrict")
                {
                    schStrictC1.Text = cankaos[i];
                    schStrictC2.Text = shuomings[i];
                    CK_schStrict(true);
                }
                else if (keys[i] == "schAdv")
                {
                    schAdvC1.Text = cankaos[i];
                    schAdvC2.Text = shuomings[i];
                    CK_schAdv(true);
                }
                else if (keys[i] == "pageSize")
                {
                    pageSizeC1.Text = cankaos[i];
                    pageSizeC2.Text = shuomings[i];
                    CK_pageSize(true);
                }
                else if (keys[i] == "numType")
                {
                    numTypeC1.Text = cankaos[i];
                    numTypeC2.Text = shuomings[i];
                    CK_numType(true);
                }
                else if (keys[i] == "pageNum")
                {
                    pageNumC1.Text = cankaos[i];
                    pageNumC2.Text = shuomings[i];
                    CK_pageNum(true);
                }
                else if (keys[i].StartsWith("keyValue"))
                {
                    keyValueC1.Text = keys[i].Substring(keys[i].IndexOf('.')+1);
                    keyValueC2.Text = shuomings[i];
                    CK_keyValue(true);
                }
                else
                {
                    string[] statItem = keys[i].Split('.');
                    if(StatCount==0)
                    {
                        stat1Para.Text = statItem[0];
                        stat1C1.Text= statItem[1];
                        stat1C2.Text = shuomings[i];
                        CK_stat1(true);
                    }
                    else if (StatCount == 1)
                    {
                        stat2Para.Text = statItem[0];
                        stat2C1.Text = statItem[1];
                        stat2C2.Text = shuomings[i];
                        CK_stat2(true);
                    }
                    else if (StatCount == 2)
                    {
                        stat3Para.Text = statItem[0];
                        stat3C1.Text = statItem[1];
                        stat3C2.Text = shuomings[i];
                        CK_stat3(true);
                    }
                    StatCount++;
                }
            }

            if (!keyValueCK.Checked) keyValueC2.Text = res.ctl.str("List_Api.3");
            if (!stat1CK.Checked) stat1C2.Text = res.ctl.str("List_Api.4");
            if (!stat2CK.Checked) stat2C2.Text = res.ctl.str("List_Api.4");
            if (!stat3CK.Checked) stat3C2.Text = res.ctl.str("List_Api.4");
        }
        Dictionary<string,string> setDicFromStr(string setstr)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var items = setstr.Split(new string[] { "[;;]" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                var item2 = item.Split(new string[] { "[::]"},StringSplitOptions.None);
                dic.Add(item2[0], item2[1]);
            }
            return dic;
        }
        void CK_orderBy(bool IsCked){orderByCK.Checked = IsCked;orderByC1.Enabled = IsCked;orderByC2.Enabled = IsCked; }
        void CK_orderType(bool IsCked) { orderTypeCK.Checked = IsCked; orderTypeC1.Enabled = IsCked; orderTypeC2.Enabled = IsCked; }
        void CK_schText(bool IsCked) { schTextCK.Checked = IsCked; schTextC1.Enabled = IsCked; schTextC2.Enabled = IsCked; }
        void CK_schStrict(bool IsCked) { schStrictCK.Checked = IsCked; schStrictC1.Enabled = IsCked; schStrictC2.Enabled = IsCked; }
        void CK_schAdv(bool IsCked) { schAdvCK.Checked = IsCked; schAdvC1.Enabled = IsCked; schAdvC2.Enabled = IsCked; }
        void CK_pageSize(bool IsCked) { pageSizeCK.Checked = IsCked; pageSizeC1.Enabled = IsCked; pageSizeC2.Enabled = IsCked; }
        void CK_numType(bool IsCked) { numTypeCK.Checked = IsCked; numTypeC1.Enabled = IsCked; numTypeC2.Enabled = IsCked; }
        void CK_pageNum(bool IsCked) { pageNumCK.Checked = IsCked; pageNumC1.Enabled = IsCked; pageNumC2.Enabled = IsCked; }
        void CK_keyValue(bool IsCked) { keyValueCK.Checked = IsCked; keyValueC1.Enabled = IsCked; keyValueC2.Enabled = IsCked; }
        void CK_stat1(bool IsCked) { stat1CK.Checked = IsCked; stat1C1.Enabled = IsCked; stat1C2.Enabled = IsCked; stat1Para.Enabled = IsCked; }
        void CK_stat2(bool IsCked) { stat2CK.Checked = IsCked; stat2C1.Enabled = IsCked; stat2C2.Enabled = IsCked; stat2Para.Enabled = IsCked; }
        void CK_stat3(bool IsCked) { stat3CK.Checked = IsCked; stat3C1.Enabled = IsCked; stat3C2.Enabled = IsCked; stat3Para.Enabled = IsCked; }
        private void test_Click(object sender, EventArgs e)
        {
            SQL sql = new SQL();
            sql.restr = "";
            sql.ShowDialog();
        }
        string[] Set2Array()
        {
            string keys = "";
            string cankaos = "";
            string shuoming = "";
            if(orderByCK.Checked)
            {
                keys += ",orderBy";
                cankaos += "[#]" + orderByC1.Text.Trim();
                shuoming += "[#]" + orderByC2.Text.Trim();
            }
            if (orderTypeCK.Checked)
            {
                keys += ",orderType";
                cankaos += "[#]" + orderTypeC1.Text.Trim();
                shuoming += "[#]" + orderTypeC2.Text.Trim();
            }
            if (schTextCK.Checked)
            {
                keys += ",schText";
                cankaos += "[#]" + schTextC1.Text.Trim();
                shuoming += "[#]" + schTextC2.Text.Trim();
            }
            if (schStrictCK.Checked)
            {
                keys += ",schStrict";
                cankaos += "[#]" + schStrictC1.Text.Trim();
                shuoming += "[#]" + schStrictC2.Text.Trim();
            }
            if (schAdvCK.Checked)
            {
                keys += ",schAdv";
                cankaos += "[#]" + schAdvC1.Text.Trim();
                shuoming += "[#]" + schAdvC2.Text.Trim();
            }
            if (pageSizeCK.Checked)
            {
                keys += ",pageSize";
                cankaos += "[#]" + pageSizeC1.Text.Trim();
                shuoming += "[#]" + pageSizeC2.Text.Trim();
            }
            if (numTypeCK.Checked)
            {
                keys += ",numType";
                cankaos += "[#]" + numTypeC1.Text.Trim();
                shuoming += "[#]" + numTypeC2.Text.Trim();
            }
            if (pageNumCK.Checked)
            {
                keys += ",pageNum";
                cankaos += "[#]" + pageNumC1.Text.Trim();
                shuoming += "[#]" + pageNumC2.Text.Trim();
            }
            if (keyValueCK.Checked)
            {
                keys += ",keyValue." + keyValueC1.Text.Trim();
                cankaos += "[#]" ;
                shuoming += "[#]" + keyValueC2.Text.Trim();
            }
            if (stat1CK.Checked)
            {
                keys += ","+stat1Para.Text.Trim()+"."+ stat1C1.Text.Trim();
                cankaos += "[#]";
                shuoming += "[#]" + stat1C2.Text.Trim();
            }
            if (stat2CK.Checked)
            {
                keys += "," + stat2Para.Text.Trim() + "." + stat2C1.Text.Trim();
                cankaos += "[#]";
                shuoming += "[#]" + stat2C2.Text.Trim();
            }
            if (stat3CK.Checked)
            {
                keys += "," + stat3Para.Text.Trim() + "." + stat3C1.Text.Trim();
                cankaos += "[#]";
                shuoming += "[#]" + stat3C2.Text.Trim();
            }
            if (keys != "") keys = keys.Substring(1);
            if (cankaos != "") cankaos = cankaos.Substring(3);
            if (shuoming != "") shuoming = shuoming.Substring(3);
            string kvStr = "ExportFileName[::]"+ ExportFileName.Text+"[;;]";
            return new string[] { luyou.Text.Trim(),miaosu.Text.Trim(),keys,cankaos,shuoming,inputForm.Checked?"form":"json", kvStr };
        }
        private void lieming_Leave(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                al[listView1.SelectedItems[0].Index] = Set2Array();
                InitCfgStr();
                listView1.SelectedItems[0].Text = (luyou.Text.Trim().Equals("") ? "GetList_" + (listView1.SelectedItems[0].Index + 1) : (luyou.Text.Trim())) ;
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
                    listView1.Items[curpos - 1].Text = (o1[0].Trim().Equals("") ? "GetList_" + curpos : (o1[0].Trim() )) ;
                    listView1.Items[curpos].Text = (o0[0].Trim().Equals("") ? "GetList_" + (curpos + 1) : (o0[0].Trim())) ;
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
                    listView1.Items[curpos + 1].Text = (o1[0].Trim().Equals("") ? "GetList_" + (curpos + 2) : (o1[0].Trim() )) ;
                    listView1.Items[curpos].Text = (o0[0].Trim().Equals("") ? "GetList_" + (curpos + 1) : (o0[0].Trim() )) ;
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
        }

        private void button6_Click(object sender, EventArgs e)
        {
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
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string MainTable = classes.PageAsist.GetPartSetValue(partId, "MainTable");
            string CusSQL = classes.PageAsist.GetPartSetValue(partId, "CusSQL");
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
                rs.ShowDialog();
                if (rs.SetString != null && rs.SetString != "")
                {
                    cfg.Text = rs.SetString;
                    setInitFromSetString(rs.SetString);
                }
            }
        }

        private void orderByCK_Click(object sender, EventArgs e)
        {
            CK_orderBy(((CheckBox)sender).Checked);
            lieming_Leave(sender, e);
        }

        private void orderTypeCK_Click(object sender, EventArgs e)
        {
            CK_orderType(((CheckBox)sender).Checked);
            lieming_Leave(sender, e);
        }

        private void schTextCK_Click(object sender, EventArgs e)
        {
            CK_schText(((CheckBox)sender).Checked);
            lieming_Leave(sender, e);
        }

        private void schStrictCK_Click(object sender, EventArgs e)
        {
            CK_schStrict(((CheckBox)sender).Checked);
            lieming_Leave(sender, e);
        }

        private void schAdvCK_Click(object sender, EventArgs e)
        {
            CK_schAdv(((CheckBox)sender).Checked);
            lieming_Leave(sender, e);
        }

        private void pageSizeCK_Click(object sender, EventArgs e)
        {
            CK_pageSize(((CheckBox)sender).Checked);
            lieming_Leave(sender, e);
        }

        private void numTypeCK_Click(object sender, EventArgs e)
        {
            CK_numType(((CheckBox)sender).Checked);
            lieming_Leave(sender, e);
        }

        private void pageNumCK_Click(object sender, EventArgs e)
        {
            CK_pageNum(((CheckBox)sender).Checked);
            lieming_Leave(sender, e);
        }

        private void keyValueCK_Click(object sender, EventArgs e)
        {
            CK_keyValue(((CheckBox)sender).Checked);
            lieming_Leave(sender, e);
        }

        private void stat1CK_Click(object sender, EventArgs e)
        {
            CK_stat1(((CheckBox)sender).Checked);
            lieming_Leave(sender, e);
        }

        private void stat2CK_Click(object sender, EventArgs e)
        {
            CK_stat2(((CheckBox)sender).Checked);
            lieming_Leave(sender, e);
        }

        private void stat3CK_Click(object sender, EventArgs e)
        {
            CK_stat3(((CheckBox)sender).Checked);
            lieming_Leave(sender, e);
        }

        private void inputJson_Click(object sender, EventArgs e)
        {
            lieming_Leave(sender, e);
        }

        private void inputForm_Click(object sender, EventArgs e)
        {
            lieming_Leave(sender, e);
        }

        private void schStrictC2_DoubleClick(object sender, EventArgs e)
        {
            string MainTable = classes.PageAsist.GetPartSetValue(partId, "MainTable");
            string CusSQL = classes.PageAsist.GetPartSetValue(partId, "CusSQL");
            control.Common_SelCols cs = new control.Common_SelCols();
            cs.MainTable = MainTable;
            cs.SelectSql = CusSQL;
            cs.SelValue = "";
            cs.SelType = 3;
            cs.ShowDialog();
            if (!cs.IsCancel)
            {
                schStrictC2.Text=cs.SelValue;
            }
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            al.Clear();
            InitCfgStr();
        }

        private void miaosu_DoubleClick(object sender, EventArgs e)
        {
            TextEditor editor = new TextEditor();
            editor.basetext=miaosu.Text;
            editor.ShowDialog();
            if(!editor.cancel) miaosu.Text=editor.basetext;
        }
    }

       
}
