namespace FTDPClient.forms
{
    partial class Rule
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Rule));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tv = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Menu_NewRootDir = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_NewDir = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_NewDir_Name = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_NewDir_Bind = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_NewTableRule = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_NewTableRule_Name = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_NewTableRule_Bind = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_NewColumnRule = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_NewColumnRule_Name = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_NewColumnRule_Bind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_Clone = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_Dell = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label_rule_desc = new System.Windows.Forms.Label();
            this.box_rule_text = new ICSharpCode.TextEditor.TextEditorControl();
            this.box_rule_sel2 = new System.Windows.Forms.ComboBox();
            this.box_rule_sel1 = new System.Windows.Forms.ComboBox();
            this.label_rule = new System.Windows.Forms.Label();
            this.btn_column_remove = new System.Windows.Forms.Button();
            this.box_column = new System.Windows.Forms.Label();
            this.btn_column = new System.Windows.Forms.Button();
            this.label_column = new System.Windows.Forms.Label();
            this.btn_table_remove = new System.Windows.Forms.Button();
            this.box_table = new System.Windows.Forms.Label();
            this.btn_table = new System.Windows.Forms.Button();
            this.label_table = new System.Windows.Forms.Label();
            this.btn_apipath_remove = new System.Windows.Forms.Button();
            this.box_apipath = new System.Windows.Forms.Label();
            this.btn_apipath = new System.Windows.Forms.Button();
            this.label_apipath = new System.Windows.Forms.Label();
            this.box_caption = new System.Windows.Forms.TextBox();
            this.label_caption = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tv);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label_rule_desc);
            this.splitContainer1.Panel2.Controls.Add(this.box_rule_text);
            this.splitContainer1.Panel2.Controls.Add(this.box_rule_sel2);
            this.splitContainer1.Panel2.Controls.Add(this.box_rule_sel1);
            this.splitContainer1.Panel2.Controls.Add(this.label_rule);
            this.splitContainer1.Panel2.Controls.Add(this.btn_column_remove);
            this.splitContainer1.Panel2.Controls.Add(this.box_column);
            this.splitContainer1.Panel2.Controls.Add(this.btn_column);
            this.splitContainer1.Panel2.Controls.Add(this.label_column);
            this.splitContainer1.Panel2.Controls.Add(this.btn_table_remove);
            this.splitContainer1.Panel2.Controls.Add(this.box_table);
            this.splitContainer1.Panel2.Controls.Add(this.btn_table);
            this.splitContainer1.Panel2.Controls.Add(this.label_table);
            this.splitContainer1.Panel2.Controls.Add(this.btn_apipath_remove);
            this.splitContainer1.Panel2.Controls.Add(this.box_apipath);
            this.splitContainer1.Panel2.Controls.Add(this.btn_apipath);
            this.splitContainer1.Panel2.Controls.Add(this.label_apipath);
            this.splitContainer1.Panel2.Controls.Add(this.box_caption);
            this.splitContainer1.Panel2.Controls.Add(this.label_caption);
            this.splitContainer1.Size = new System.Drawing.Size(1084, 761);
            this.splitContainer1.SplitterDistance = 450;
            this.splitContainer1.TabIndex = 1;
            // 
            // tv
            // 
            this.tv.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tv.ContextMenuStrip = this.contextMenuStrip1;
            this.tv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tv.Font = new System.Drawing.Font("宋体", 11F);
            this.tv.ImageIndex = 0;
            this.tv.ImageList = this.imageList1;
            this.tv.Location = new System.Drawing.Point(0, 0);
            this.tv.Name = "tv";
            this.tv.SelectedImageIndex = 0;
            this.tv.Size = new System.Drawing.Size(450, 761);
            this.tv.TabIndex = 0;
            this.tv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterSelect);
            this.tv.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tv_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_NewRootDir,
            this.Menu_NewDir,
            this.Menu_NewTableRule,
            this.Menu_NewColumnRule,
            this.toolStripSeparator1,
            this.Menu_Clone,
            this.toolStripSeparator3,
            this.Menu_Dell,
            this.toolStripSeparator2,
            this.Menu_Refresh});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(165, 176);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // Menu_NewRootDir
            // 
            this.Menu_NewRootDir.Image = ((System.Drawing.Image)(resources.GetObject("Menu_NewRootDir.Image")));
            this.Menu_NewRootDir.Name = "Menu_NewRootDir";
            this.Menu_NewRootDir.Size = new System.Drawing.Size(164, 22);
            this.Menu_NewRootDir.Text = "新建根目录(&R)";
            this.Menu_NewRootDir.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // Menu_NewDir
            // 
            this.Menu_NewDir.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_NewDir_Name,
            this.Menu_NewDir_Bind});
            this.Menu_NewDir.Image = ((System.Drawing.Image)(resources.GetObject("Menu_NewDir.Image")));
            this.Menu_NewDir.Name = "Menu_NewDir";
            this.Menu_NewDir.Size = new System.Drawing.Size(164, 22);
            this.Menu_NewDir.Text = "新建子目录(&N)";
            this.Menu_NewDir.Click += new System.EventHandler(this.Menu_NewDir_Click);
            // 
            // Menu_NewDir_Name
            // 
            this.Menu_NewDir_Name.Name = "Menu_NewDir_Name";
            this.Menu_NewDir_Name.Size = new System.Drawing.Size(166, 22);
            this.Menu_NewDir_Name.Text = "通过输入名称(&N)";
            this.Menu_NewDir_Name.Click += new System.EventHandler(this.Menu_NewDir_Name_Click);
            // 
            // Menu_NewDir_Bind
            // 
            this.Menu_NewDir_Bind.Name = "Menu_NewDir_Bind";
            this.Menu_NewDir_Bind.Size = new System.Drawing.Size(166, 22);
            this.Menu_NewDir_Bind.Text = "通过绑定接口(&B)";
            this.Menu_NewDir_Bind.Click += new System.EventHandler(this.Menu_NewDir_Bind_Click);
            // 
            // Menu_NewTableRule
            // 
            this.Menu_NewTableRule.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_NewTableRule_Name,
            this.Menu_NewTableRule_Bind});
            this.Menu_NewTableRule.Image = ((System.Drawing.Image)(resources.GetObject("Menu_NewTableRule.Image")));
            this.Menu_NewTableRule.Name = "Menu_NewTableRule";
            this.Menu_NewTableRule.Size = new System.Drawing.Size(164, 22);
            this.Menu_NewTableRule.Text = "新建表规则(&T)";
            this.Menu_NewTableRule.Click += new System.EventHandler(this.Menu_NewTableRule_Click);
            // 
            // Menu_NewTableRule_Name
            // 
            this.Menu_NewTableRule_Name.Name = "Menu_NewTableRule_Name";
            this.Menu_NewTableRule_Name.Size = new System.Drawing.Size(166, 22);
            this.Menu_NewTableRule_Name.Text = "通过输入名称(&N)";
            this.Menu_NewTableRule_Name.Click += new System.EventHandler(this.Menu_NewTableRule_Name_Click);
            // 
            // Menu_NewTableRule_Bind
            // 
            this.Menu_NewTableRule_Bind.Name = "Menu_NewTableRule_Bind";
            this.Menu_NewTableRule_Bind.Size = new System.Drawing.Size(166, 22);
            this.Menu_NewTableRule_Bind.Text = "通过绑定表格(&B)";
            this.Menu_NewTableRule_Bind.Click += new System.EventHandler(this.Menu_NewTableRule_Bind_Click);
            // 
            // Menu_NewColumnRule
            // 
            this.Menu_NewColumnRule.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_NewColumnRule_Name,
            this.Menu_NewColumnRule_Bind});
            this.Menu_NewColumnRule.Image = ((System.Drawing.Image)(resources.GetObject("Menu_NewColumnRule.Image")));
            this.Menu_NewColumnRule.Name = "Menu_NewColumnRule";
            this.Menu_NewColumnRule.Size = new System.Drawing.Size(164, 22);
            this.Menu_NewColumnRule.Text = "新建字段规则(&C)";
            this.Menu_NewColumnRule.Click += new System.EventHandler(this.Menu_NewColumnRule_Click);
            // 
            // Menu_NewColumnRule_Name
            // 
            this.Menu_NewColumnRule_Name.Name = "Menu_NewColumnRule_Name";
            this.Menu_NewColumnRule_Name.Size = new System.Drawing.Size(166, 22);
            this.Menu_NewColumnRule_Name.Text = "通过输入名称(&N)";
            this.Menu_NewColumnRule_Name.Click += new System.EventHandler(this.Menu_NewColumnRule_Name_Click);
            // 
            // Menu_NewColumnRule_Bind
            // 
            this.Menu_NewColumnRule_Bind.Name = "Menu_NewColumnRule_Bind";
            this.Menu_NewColumnRule_Bind.Size = new System.Drawing.Size(166, 22);
            this.Menu_NewColumnRule_Bind.Text = "通过绑定字段(&B)";
            this.Menu_NewColumnRule_Bind.Click += new System.EventHandler(this.Menu_NewColumnRule_Bind_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(161, 6);
            // 
            // Menu_Clone
            // 
            this.Menu_Clone.Image = ((System.Drawing.Image)(resources.GetObject("Menu_Clone.Image")));
            this.Menu_Clone.Name = "Menu_Clone";
            this.Menu_Clone.Size = new System.Drawing.Size(164, 22);
            this.Menu_Clone.Text = "克隆(&K)";
            this.Menu_Clone.Click += new System.EventHandler(this.Menu_Clone_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(161, 6);
            // 
            // Menu_Dell
            // 
            this.Menu_Dell.Image = ((System.Drawing.Image)(resources.GetObject("Menu_Dell.Image")));
            this.Menu_Dell.Name = "Menu_Dell";
            this.Menu_Dell.Size = new System.Drawing.Size(164, 22);
            this.Menu_Dell.Text = "删除(&D)";
            this.Menu_Dell.Click += new System.EventHandler(this.Menu_Dell_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(161, 6);
            // 
            // Menu_Refresh
            // 
            this.Menu_Refresh.Image = ((System.Drawing.Image)(resources.GetObject("Menu_Refresh.Image")));
            this.Menu_Refresh.Name = "Menu_Refresh";
            this.Menu_Refresh.Size = new System.Drawing.Size(164, 22);
            this.Menu_Refresh.Text = "刷新(&F)";
            this.Menu_Refresh.Click += new System.EventHandler(this.Menu_Refresh_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "0.gif");
            this.imageList1.Images.SetKeyName(1, "1.gif");
            this.imageList1.Images.SetKeyName(2, "2.gif");
            this.imageList1.Images.SetKeyName(3, "3.gif");
            this.imageList1.Images.SetKeyName(4, "4.gif");
            this.imageList1.Images.SetKeyName(5, "5.gif");
            this.imageList1.Images.SetKeyName(6, "6.gif");
            this.imageList1.Images.SetKeyName(7, "7.gif");
            this.imageList1.Images.SetKeyName(8, "8.gif");
            this.imageList1.Images.SetKeyName(9, "9.gif");
            this.imageList1.Images.SetKeyName(10, "10.gif");
            this.imageList1.Images.SetKeyName(11, "11.gif");
            this.imageList1.Images.SetKeyName(12, "12.gif");
            this.imageList1.Images.SetKeyName(13, "13.gif");
            this.imageList1.Images.SetKeyName(14, "14.gif");
            this.imageList1.Images.SetKeyName(15, "15.gif");
            this.imageList1.Images.SetKeyName(16, "16.gif");
            this.imageList1.Images.SetKeyName(17, "17.gif");
            this.imageList1.Images.SetKeyName(18, "18.gif");
            this.imageList1.Images.SetKeyName(19, "19.gif");
            this.imageList1.Images.SetKeyName(20, "20.gif");
            this.imageList1.Images.SetKeyName(21, "21.gif");
            this.imageList1.Images.SetKeyName(22, "22.gif");
            this.imageList1.Images.SetKeyName(23, "23.gif");
            this.imageList1.Images.SetKeyName(24, "24.gif");
            this.imageList1.Images.SetKeyName(25, "25.gif");
            this.imageList1.Images.SetKeyName(26, "26.gif");
            this.imageList1.Images.SetKeyName(27, "27.gif");
            this.imageList1.Images.SetKeyName(28, "28.gif");
            this.imageList1.Images.SetKeyName(29, "29.gif");
            this.imageList1.Images.SetKeyName(30, "30.gif");
            this.imageList1.Images.SetKeyName(31, "31.gif");
            this.imageList1.Images.SetKeyName(32, "32.gif");
            this.imageList1.Images.SetKeyName(33, "130.gif");
            this.imageList1.Images.SetKeyName(34, "collapse.gif");
            this.imageList1.Images.SetKeyName(35, "control.gif");
            this.imageList1.Images.SetKeyName(36, "delete.gif");
            this.imageList1.Images.SetKeyName(37, "edit.gif");
            this.imageList1.Images.SetKeyName(38, "tags.gif");
            // 
            // label_rule_desc
            // 
            this.label_rule_desc.AutoSize = true;
            this.label_rule_desc.Font = new System.Drawing.Font("宋体", 11F);
            this.label_rule_desc.Location = new System.Drawing.Point(115, 344);
            this.label_rule_desc.Margin = new System.Windows.Forms.Padding(3, 27, 3, 3);
            this.label_rule_desc.Name = "label_rule_desc";
            this.label_rule_desc.Size = new System.Drawing.Size(97, 15);
            this.label_rule_desc.TabIndex = 44;
            this.label_rule_desc.Text = "规则文字描述";
            // 
            // box_rule_text
            // 
            this.box_rule_text.BackColor = System.Drawing.SystemColors.Control;
            this.box_rule_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.box_rule_text.Font = new System.Drawing.Font("宋体", 15F);
            this.box_rule_text.Highlighting = "FTDP";
            this.box_rule_text.Location = new System.Drawing.Point(118, 265);
            this.box_rule_text.Name = "box_rule_text";
            this.box_rule_text.ShowLineNumbers = false;
            this.box_rule_text.ShowVRuler = false;
            this.box_rule_text.Size = new System.Drawing.Size(474, 49);
            this.box_rule_text.TabIndent = 2;
            this.box_rule_text.TabIndex = 43;
            this.box_rule_text.Leave += new System.EventHandler(this.box_rule_text_Leave);
            // 
            // box_rule_sel2
            // 
            this.box_rule_sel2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.box_rule_sel2.Font = new System.Drawing.Font("宋体", 15F);
            this.box_rule_sel2.FormattingEnabled = true;
            this.box_rule_sel2.Location = new System.Drawing.Point(391, 219);
            this.box_rule_sel2.Name = "box_rule_sel2";
            this.box_rule_sel2.Size = new System.Drawing.Size(201, 28);
            this.box_rule_sel2.TabIndex = 16;
            this.box_rule_sel2.SelectedIndexChanged += new System.EventHandler(this.box_rule_sel2_SelectedIndexChanged);
            // 
            // box_rule_sel1
            // 
            this.box_rule_sel1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.box_rule_sel1.Font = new System.Drawing.Font("宋体", 15F);
            this.box_rule_sel1.FormattingEnabled = true;
            this.box_rule_sel1.Location = new System.Drawing.Point(118, 219);
            this.box_rule_sel1.Name = "box_rule_sel1";
            this.box_rule_sel1.Size = new System.Drawing.Size(201, 28);
            this.box_rule_sel1.TabIndex = 15;
            this.box_rule_sel1.SelectedIndexChanged += new System.EventHandler(this.box_rule_sel1_SelectedIndexChanged);
            // 
            // label_rule
            // 
            this.label_rule.AutoSize = true;
            this.label_rule.Font = new System.Drawing.Font("宋体", 11F);
            this.label_rule.Location = new System.Drawing.Point(18, 225);
            this.label_rule.Margin = new System.Windows.Forms.Padding(3, 27, 3, 3);
            this.label_rule.Name = "label_rule";
            this.label_rule.Size = new System.Drawing.Size(67, 15);
            this.label_rule.TabIndex = 14;
            this.label_rule.Text = "规则定义";
            // 
            // btn_column_remove
            // 
            this.btn_column_remove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_column_remove.Location = new System.Drawing.Point(511, 553);
            this.btn_column_remove.Name = "btn_column_remove";
            this.btn_column_remove.Size = new System.Drawing.Size(75, 30);
            this.btn_column_remove.TabIndex = 13;
            this.btn_column_remove.Text = "解 除";
            this.btn_column_remove.Click += new System.EventHandler(this.btn_column_remove_Click);
            // 
            // box_column
            // 
            this.box_column.AutoSize = true;
            this.box_column.Font = new System.Drawing.Font("宋体", 11F);
            this.box_column.Location = new System.Drawing.Point(404, 598);
            this.box_column.Margin = new System.Windows.Forms.Padding(3, 27, 3, 3);
            this.box_column.Name = "box_column";
            this.box_column.Size = new System.Drawing.Size(97, 15);
            this.box_column.TabIndex = 12;
            this.box_column.Text = "字段名和描述";
            // 
            // btn_column
            // 
            this.btn_column.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_column.Location = new System.Drawing.Point(407, 553);
            this.btn_column.Name = "btn_column";
            this.btn_column.Size = new System.Drawing.Size(75, 30);
            this.btn_column.TabIndex = 11;
            this.btn_column.Text = "绑 定";
            this.btn_column.Click += new System.EventHandler(this.btn_column_Click);
            // 
            // label_column
            // 
            this.label_column.AutoSize = true;
            this.label_column.Font = new System.Drawing.Font("宋体", 11F);
            this.label_column.Location = new System.Drawing.Point(307, 559);
            this.label_column.Margin = new System.Windows.Forms.Padding(3, 27, 3, 3);
            this.label_column.Name = "label_column";
            this.label_column.Size = new System.Drawing.Size(67, 15);
            this.label_column.TabIndex = 10;
            this.label_column.Text = "绑定字段";
            // 
            // btn_table_remove
            // 
            this.btn_table_remove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_table_remove.Location = new System.Drawing.Point(511, 460);
            this.btn_table_remove.Name = "btn_table_remove";
            this.btn_table_remove.Size = new System.Drawing.Size(75, 30);
            this.btn_table_remove.TabIndex = 9;
            this.btn_table_remove.Text = "解 除";
            this.btn_table_remove.Click += new System.EventHandler(this.btn_table_remove_Click);
            // 
            // box_table
            // 
            this.box_table.AutoSize = true;
            this.box_table.Font = new System.Drawing.Font("宋体", 11F);
            this.box_table.Location = new System.Drawing.Point(404, 505);
            this.box_table.Margin = new System.Windows.Forms.Padding(3, 27, 3, 3);
            this.box_table.Name = "box_table";
            this.box_table.Size = new System.Drawing.Size(82, 15);
            this.box_table.TabIndex = 8;
            this.box_table.Text = "表名和描述";
            // 
            // btn_table
            // 
            this.btn_table.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_table.Location = new System.Drawing.Point(407, 460);
            this.btn_table.Name = "btn_table";
            this.btn_table.Size = new System.Drawing.Size(75, 30);
            this.btn_table.TabIndex = 7;
            this.btn_table.Text = "绑 定";
            this.btn_table.Click += new System.EventHandler(this.btn_table_Click);
            // 
            // label_table
            // 
            this.label_table.AutoSize = true;
            this.label_table.Font = new System.Drawing.Font("宋体", 11F);
            this.label_table.Location = new System.Drawing.Point(307, 466);
            this.label_table.Margin = new System.Windows.Forms.Padding(3, 27, 3, 3);
            this.label_table.Name = "label_table";
            this.label_table.Size = new System.Drawing.Size(52, 15);
            this.label_table.TabIndex = 6;
            this.label_table.Text = "绑定表";
            // 
            // btn_apipath_remove
            // 
            this.btn_apipath_remove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_apipath_remove.Location = new System.Drawing.Point(222, 85);
            this.btn_apipath_remove.Name = "btn_apipath_remove";
            this.btn_apipath_remove.Size = new System.Drawing.Size(75, 30);
            this.btn_apipath_remove.TabIndex = 5;
            this.btn_apipath_remove.Text = "解 除";
            this.btn_apipath_remove.Click += new System.EventHandler(this.button1_Click);
            // 
            // box_apipath
            // 
            this.box_apipath.AutoSize = true;
            this.box_apipath.Font = new System.Drawing.Font("宋体", 11F);
            this.box_apipath.Location = new System.Drawing.Point(115, 130);
            this.box_apipath.Margin = new System.Windows.Forms.Padding(3, 27, 3, 3);
            this.box_apipath.Name = "box_apipath";
            this.box_apipath.Size = new System.Drawing.Size(112, 15);
            this.box_apipath.TabIndex = 4;
            this.box_apipath.Text = "接口地址和描述";
            // 
            // btn_apipath
            // 
            this.btn_apipath.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_apipath.Location = new System.Drawing.Point(118, 85);
            this.btn_apipath.Name = "btn_apipath";
            this.btn_apipath.Size = new System.Drawing.Size(75, 30);
            this.btn_apipath.TabIndex = 3;
            this.btn_apipath.Text = "绑 定";
            this.btn_apipath.Click += new System.EventHandler(this.btn_apipath_Click);
            // 
            // label_apipath
            // 
            this.label_apipath.AutoSize = true;
            this.label_apipath.Font = new System.Drawing.Font("宋体", 11F);
            this.label_apipath.Location = new System.Drawing.Point(18, 91);
            this.label_apipath.Margin = new System.Windows.Forms.Padding(3, 27, 3, 3);
            this.label_apipath.Name = "label_apipath";
            this.label_apipath.Size = new System.Drawing.Size(67, 15);
            this.label_apipath.TabIndex = 2;
            this.label_apipath.Text = "绑定接口";
            // 
            // box_caption
            // 
            this.box_caption.Font = new System.Drawing.Font("宋体", 15F);
            this.box_caption.Location = new System.Drawing.Point(118, 20);
            this.box_caption.Margin = new System.Windows.Forms.Padding(3, 23, 3, 3);
            this.box_caption.Name = "box_caption";
            this.box_caption.Size = new System.Drawing.Size(355, 30);
            this.box_caption.TabIndex = 1;
            this.box_caption.Leave += new System.EventHandler(this.edit_leave);
            // 
            // label_caption
            // 
            this.label_caption.AutoSize = true;
            this.label_caption.Font = new System.Drawing.Font("宋体", 11F);
            this.label_caption.Location = new System.Drawing.Point(18, 27);
            this.label_caption.Margin = new System.Windows.Forms.Padding(3, 27, 3, 3);
            this.label_caption.Name = "label_caption";
            this.label_caption.Size = new System.Drawing.Size(61, 15);
            this.label_caption.TabIndex = 0;
            this.label_caption.Text = "名   称";
            // 
            // Rule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 761);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Rule";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rule Define";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Rule_FormClosing);
            this.Load += new System.EventHandler(this.Rule_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tv;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Menu_NewRootDir;
        private System.Windows.Forms.ToolStripMenuItem Menu_NewDir;
        private System.Windows.Forms.ToolStripMenuItem Menu_NewTableRule;
        private System.Windows.Forms.ToolStripMenuItem Menu_NewColumnRule;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Menu_Dell;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem Menu_Refresh;
        private System.Windows.Forms.Label label_caption;
        private System.Windows.Forms.TextBox box_caption;
        private System.Windows.Forms.Label label_apipath;
        private System.Windows.Forms.Label box_apipath;
        private System.Windows.Forms.Button btn_apipath;
        private System.Windows.Forms.Button btn_apipath_remove;
        private System.Windows.Forms.Button btn_column_remove;
        private System.Windows.Forms.Label box_column;
        private System.Windows.Forms.Button btn_column;
        private System.Windows.Forms.Label label_column;
        private System.Windows.Forms.Button btn_table_remove;
        private System.Windows.Forms.Label box_table;
        private System.Windows.Forms.Button btn_table;
        private System.Windows.Forms.Label label_table;
        private System.Windows.Forms.ToolStripMenuItem Menu_Clone;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Label label_rule;
        private System.Windows.Forms.ComboBox box_rule_sel2;
        private System.Windows.Forms.ComboBox box_rule_sel1;
        private ICSharpCode.TextEditor.TextEditorControl box_rule_text;
        private System.Windows.Forms.Label label_rule_desc;
        private System.Windows.Forms.ToolStripMenuItem Menu_NewDir_Name;
        private System.Windows.Forms.ToolStripMenuItem Menu_NewDir_Bind;
        private System.Windows.Forms.ToolStripMenuItem Menu_NewTableRule_Name;
        private System.Windows.Forms.ToolStripMenuItem Menu_NewTableRule_Bind;
        private System.Windows.Forms.ToolStripMenuItem Menu_NewColumnRule_Name;
        private System.Windows.Forms.ToolStripMenuItem Menu_NewColumnRule_Bind;
    }
}