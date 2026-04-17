namespace FTDPClient.forms
{
    partial class TagEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagEdit));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.CMCode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CodeCut = new System.Windows.Forms.ToolStripMenuItem();
            this.CodeCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.CodePaste = new System.Windows.Forms.ToolStripMenuItem();
            this.CodeDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.CodeUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.CodeRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.textEditorControl1 = new ICSharpCode.TextEditor.TextEditorControl();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.CMCode.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(996, 433);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "更新外部";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(2, 433);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 30);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // CMCode
            // 
            this.CMCode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CodeCut,
            this.CodeCopy,
            this.CodePaste,
            this.CodeDelete,
            this.toolStripSeparator2,
            this.CodeUndo,
            this.CodeRedo});
            this.CMCode.Name = "CMPage";
            this.CMCode.Size = new System.Drawing.Size(144, 154);
            this.CMCode.Opening += new System.ComponentModel.CancelEventHandler(this.CMCode_Opening);
            // 
            // CodeCut
            // 
            this.CodeCut.Image = ((System.Drawing.Image)(resources.GetObject("CodeCut.Image")));
            this.CodeCut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CodeCut.Name = "CodeCut";
            this.CodeCut.Size = new System.Drawing.Size(143, 24);
            this.CodeCut.Text = "PageCut";
            this.CodeCut.Click += new System.EventHandler(this.CodeCut_Click);
            // 
            // CodeCopy
            // 
            this.CodeCopy.Image = ((System.Drawing.Image)(resources.GetObject("CodeCopy.Image")));
            this.CodeCopy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CodeCopy.Name = "CodeCopy";
            this.CodeCopy.Size = new System.Drawing.Size(143, 24);
            this.CodeCopy.Text = "PageCopy";
            this.CodeCopy.Click += new System.EventHandler(this.CodeCopy_Click);
            // 
            // CodePaste
            // 
            this.CodePaste.Image = ((System.Drawing.Image)(resources.GetObject("CodePaste.Image")));
            this.CodePaste.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CodePaste.Name = "CodePaste";
            this.CodePaste.Size = new System.Drawing.Size(143, 24);
            this.CodePaste.Text = "PagePaste";
            this.CodePaste.Click += new System.EventHandler(this.CodePaste_Click);
            // 
            // CodeDelete
            // 
            this.CodeDelete.Image = ((System.Drawing.Image)(resources.GetObject("CodeDelete.Image")));
            this.CodeDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CodeDelete.Name = "CodeDelete";
            this.CodeDelete.Size = new System.Drawing.Size(143, 24);
            this.CodeDelete.Text = "PageDelete";
            this.CodeDelete.Click += new System.EventHandler(this.CodeDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(140, 6);
            // 
            // CodeUndo
            // 
            this.CodeUndo.Image = ((System.Drawing.Image)(resources.GetObject("CodeUndo.Image")));
            this.CodeUndo.Name = "CodeUndo";
            this.CodeUndo.Size = new System.Drawing.Size(143, 24);
            this.CodeUndo.Text = "PageUndo";
            this.CodeUndo.Click += new System.EventHandler(this.CodeUndo_Click);
            // 
            // CodeRedo
            // 
            this.CodeRedo.Image = ((System.Drawing.Image)(resources.GetObject("CodeRedo.Image")));
            this.CodeRedo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CodeRedo.Name = "CodeRedo";
            this.CodeRedo.Size = new System.Drawing.Size(143, 24);
            this.CodeRedo.Text = "PageRedo";
            this.CodeRedo.Click += new System.EventHandler(this.CodeRedo_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(911, 433);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 30);
            this.button3.TabIndex = 9;
            this.button3.Text = "更新内部";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Location = new System.Drawing.Point(826, 433);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 30);
            this.button4.TabIndex = 10;
            this.button4.Text = "删除元素";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button5.Location = new System.Drawing.Point(741, 433);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 30);
            this.button5.TabIndex = 11;
            this.button5.Text = "往后插入";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button6.Location = new System.Drawing.Point(656, 433);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 30);
            this.button6.TabIndex = 12;
            this.button6.Text = "往前插入";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Button6_Click);
            // 
            // textEditorControl1
            // 
            this.textEditorControl1.BackColor = System.Drawing.SystemColors.Control;
            this.textEditorControl1.ContextMenuStrip = this.CMCode;
            this.textEditorControl1.Highlighting = null;
            this.textEditorControl1.Location = new System.Drawing.Point(2, 0);
            this.textEditorControl1.Name = "textEditorControl1";
            this.textEditorControl1.ShowVRuler = false;
            this.textEditorControl1.Size = new System.Drawing.Size(1070, 429);
            this.textEditorControl1.TabIndex = 13;
            this.textEditorControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextEditorControl1_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F);
            this.label1.Location = new System.Drawing.Point(451, 442);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "元素转换";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("宋体", 11F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(520, 436);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 15;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // TagEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 466);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textEditorControl1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TagEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TagEdit";
            this.Load += new System.EventHandler(this.ModifyDNS_Load);
            this.Shown += new System.EventHandler(this.TagEdit_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TagEdit_KeyDown);
            this.CMCode.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ContextMenuStrip CMCode;
        private System.Windows.Forms.ToolStripMenuItem CodeCut;
        private System.Windows.Forms.ToolStripMenuItem CodeCopy;
        private System.Windows.Forms.ToolStripMenuItem CodePaste;
        private System.Windows.Forms.ToolStripMenuItem CodeDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem CodeUndo;
        private System.Windows.Forms.ToolStripMenuItem CodeRedo;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private ICSharpCode.TextEditor.TextEditorControl textEditorControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}