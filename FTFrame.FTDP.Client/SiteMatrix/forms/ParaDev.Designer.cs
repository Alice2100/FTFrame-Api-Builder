namespace FTDPClient.forms
{
    partial class ParaDev
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("111112111111111 - 计量单位 1", 0);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("111112111111111 - 库存数量", 1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParaDev));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByNameAscToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByNameDescToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByCaptionAscToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByCaptionDescToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.delToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SC = new System.Windows.Forms.SplitContainer();
            this.SCC = new System.Windows.Forms.SplitContainer();
            this.mimoBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.paracaptionBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.paranameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SC)).BeginInit();
            this.SC.Panel1.SuspendLayout();
            this.SC.Panel2.SuspendLayout();
            this.SC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCC)).BeginInit();
            this.SCC.Panel1.SuspendLayout();
            this.SCC.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.SC);
            this.splitContainer1.Panel2.Font = new System.Drawing.Font("宋体", 12F);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(0, 2, 2, 2);
            this.splitContainer1.Size = new System.Drawing.Size(1534, 768);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listView1);
            this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(4, 4, 0, 0);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.button4);
            this.splitContainer2.Panel2.Controls.Add(this.button3);
            this.splitContainer2.Panel2.Controls.Add(this.button12);
            this.splitContainer2.Panel2.Controls.Add(this.button1);
            this.splitContainer2.Size = new System.Drawing.Size(400, 768);
            this.splitContainer2.SplitterDistance = 740;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.TabStop = false;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("宋体", 12F);
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.HideSelection = false;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.listView1.Location = new System.Drawing.Point(4, 4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(396, 736);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 8;
            this.listView1.TileSize = new System.Drawing.Size(280, 28);
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 275;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripSeparator2,
            this.addToolStripMenuItem,
            this.toolStripSeparator1,
            this.delToolStripMenuItem,
            this.deleteAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(156, 104);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(155, 22);
            this.toolStripMenuItem3.Text = "&Add Site Para";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(152, 6);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortByNameAscToolStripMenuItem,
            this.sortByNameDescToolStripMenuItem1,
            this.sortByCaptionAscToolStripMenuItem1,
            this.sortByCaptionDescToolStripMenuItem1});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.addToolStripMenuItem.Text = "&Sort";
            // 
            // sortByNameAscToolStripMenuItem
            // 
            this.sortByNameAscToolStripMenuItem.Name = "sortByNameAscToolStripMenuItem";
            this.sortByNameAscToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.sortByNameAscToolStripMenuItem.Text = "Sort by name asc";
            this.sortByNameAscToolStripMenuItem.Click += new System.EventHandler(this.sortByNameAscToolStripMenuItem_Click);
            // 
            // sortByNameDescToolStripMenuItem1
            // 
            this.sortByNameDescToolStripMenuItem1.Name = "sortByNameDescToolStripMenuItem1";
            this.sortByNameDescToolStripMenuItem1.Size = new System.Drawing.Size(196, 22);
            this.sortByNameDescToolStripMenuItem1.Text = "Sort by name desc";
            this.sortByNameDescToolStripMenuItem1.Click += new System.EventHandler(this.sortByNameDescToolStripMenuItem1_Click);
            // 
            // sortByCaptionAscToolStripMenuItem1
            // 
            this.sortByCaptionAscToolStripMenuItem1.Name = "sortByCaptionAscToolStripMenuItem1";
            this.sortByCaptionAscToolStripMenuItem1.Size = new System.Drawing.Size(196, 22);
            this.sortByCaptionAscToolStripMenuItem1.Text = "Sort by caption asc";
            this.sortByCaptionAscToolStripMenuItem1.Click += new System.EventHandler(this.sortByCaptionAscToolStripMenuItem1_Click);
            // 
            // sortByCaptionDescToolStripMenuItem1
            // 
            this.sortByCaptionDescToolStripMenuItem1.Name = "sortByCaptionDescToolStripMenuItem1";
            this.sortByCaptionDescToolStripMenuItem1.Size = new System.Drawing.Size(196, 22);
            this.sortByCaptionDescToolStripMenuItem1.Text = "Sort by caption desc";
            this.sortByCaptionDescToolStripMenuItem1.Click += new System.EventHandler(this.sortByCaptionDescToolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(152, 6);
            // 
            // delToolStripMenuItem
            // 
            this.delToolStripMenuItem.Name = "delToolStripMenuItem";
            this.delToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.delToolStripMenuItem.Text = "&Del";
            this.delToolStripMenuItem.Click += new System.EventHandler(this.delToolStripMenuItem_Click);
            // 
            // deleteAllToolStripMenuItem
            // 
            this.deleteAllToolStripMenuItem.Name = "deleteAllToolStripMenuItem";
            this.deleteAllToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.deleteAllToolStripMenuItem.Text = "Delete All";
            this.deleteAllToolStripMenuItem.Click += new System.EventHandler(this.deleteAllToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "14.gif");
            this.imageList1.Images.SetKeyName(1, "4.gif");
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Location = new System.Drawing.Point(207, 0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(90, 30);
            this.button4.TabIndex = 6;
            this.button4.Text = "选择接口";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(308, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 30);
            this.button3.TabIndex = 5;
            this.button3.Text = "使用说明";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button12
            // 
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button12.Location = new System.Drawing.Point(106, 0);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(90, 30);
            this.button12.TabIndex = 4;
            this.button12.Text = "Save";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(5, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Add Para";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SC
            // 
            this.SC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SC.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SC.IsSplitterFixed = true;
            this.SC.Location = new System.Drawing.Point(0, 2);
            this.SC.Name = "SC";
            this.SC.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SC.Panel1
            // 
            this.SC.Panel1.Controls.Add(this.SCC);
            // 
            // SC.Panel2
            // 
            this.SC.Panel2.Controls.Add(this.button2);
            this.SC.Size = new System.Drawing.Size(1128, 764);
            this.SC.SplitterDistance = 738;
            this.SC.SplitterWidth = 1;
            this.SC.TabIndex = 60;
            // 
            // SCC
            // 
            this.SCC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCC.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SCC.IsSplitterFixed = true;
            this.SCC.Location = new System.Drawing.Point(0, 0);
            this.SCC.Name = "SCC";
            this.SCC.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SCC.Panel1
            // 
            this.SCC.Panel1.Controls.Add(this.mimoBox);
            this.SCC.Panel1.Controls.Add(this.label3);
            this.SCC.Panel1.Controls.Add(this.paracaptionBox);
            this.SCC.Panel1.Controls.Add(this.label2);
            this.SCC.Panel1.Controls.Add(this.paranameBox);
            this.SCC.Panel1.Controls.Add(this.label1);
            this.SCC.Size = new System.Drawing.Size(1128, 738);
            this.SCC.SplitterDistance = 95;
            this.SCC.SplitterWidth = 1;
            this.SCC.TabIndex = 60;
            // 
            // mimoBox
            // 
            this.mimoBox.Location = new System.Drawing.Point(75, 62);
            this.mimoBox.Name = "mimoBox";
            this.mimoBox.Size = new System.Drawing.Size(1032, 26);
            this.mimoBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "说明";
            // 
            // paracaptionBox
            // 
            this.paracaptionBox.Location = new System.Drawing.Point(526, 21);
            this.paracaptionBox.Name = "paracaptionBox";
            this.paracaptionBox.Size = new System.Drawing.Size(581, 26);
            this.paracaptionBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(463, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "标题";
            // 
            // paranameBox
            // 
            this.paranameBox.Location = new System.Drawing.Point(75, 21);
            this.paranameBox.Name = "paranameBox";
            this.paranameBox.Size = new System.Drawing.Size(353, 26);
            this.paranameBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "标识";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Font = new System.Drawing.Font("宋体", 9F);
            this.button2.Location = new System.Drawing.Point(1005, -1);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 30);
            this.button2.TabIndex = 5;
            this.button2.Text = "增加判断条件";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 100;
            this.toolTip.ReshowDelay = 10;
            // 
            // ParaDev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1534, 768);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ParaDev";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ParaDev";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ParaDev_FormClosing);
            this.Load += new System.EventHandler(this.ForeDev_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ParaDev_KeyDown);
            this.Resize += new System.EventHandler(this.ParaDev_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.SC.Panel1.ResumeLayout(false);
            this.SC.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SC)).EndInit();
            this.SC.ResumeLayout(false);
            this.SCC.Panel1.ResumeLayout(false);
            this.SCC.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCC)).EndInit();
            this.SCC.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem delToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem sortByNameAscToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortByNameDescToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sortByCaptionAscToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sortByCaptionDescToolStripMenuItem1;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.SplitContainer SC;
        private System.Windows.Forms.SplitContainer SCC;
        private System.Windows.Forms.TextBox mimoBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox paracaptionBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox paranameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}