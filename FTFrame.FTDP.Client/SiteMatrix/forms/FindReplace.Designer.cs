namespace FTDPClient.forms
{
    partial class FindReplace
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
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.WholeWord = new System.Windows.Forms.CheckBox();
            this.MatchCase = new System.Windows.Forms.CheckBox();
            this.ReplaceTextBox = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.FindTextBox = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Find = new System.Windows.Forms.Button();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReplaceAll.Location = new System.Drawing.Point(252, 123);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(80, 30);
            this.btnReplaceAll.TabIndex = 15;
            this.btnReplaceAll.Text = "Replace &All";
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Location = new System.Drawing.Point(348, 123);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReplace.Location = new System.Drawing.Point(162, 123);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(75, 30);
            this.btnReplace.TabIndex = 13;
            this.btnReplace.Text = "&Replace";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.WholeWord);
            this.GroupBox1.Controls.Add(this.MatchCase);
            this.GroupBox1.Location = new System.Drawing.Point(92, 75);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(215, 41);
            this.GroupBox1.TabIndex = 12;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "&Find Options";
            // 
            // WholeWord
            // 
            this.WholeWord.AutoSize = true;
            this.WholeWord.Location = new System.Drawing.Point(112, 19);
            this.WholeWord.Name = "WholeWord";
            this.WholeWord.Size = new System.Drawing.Size(84, 16);
            this.WholeWord.TabIndex = 4;
            this.WholeWord.Text = "&Whole Word";
            this.WholeWord.UseVisualStyleBackColor = true;
            // 
            // MatchCase
            // 
            this.MatchCase.AutoSize = true;
            this.MatchCase.Location = new System.Drawing.Point(8, 20);
            this.MatchCase.Name = "MatchCase";
            this.MatchCase.Size = new System.Drawing.Size(84, 16);
            this.MatchCase.TabIndex = 3;
            this.MatchCase.Text = "&Match Case";
            this.MatchCase.UseVisualStyleBackColor = true;
            // 
            // ReplaceTextBox
            // 
            this.ReplaceTextBox.Font = new System.Drawing.Font("ËÎÌו", 12F);
            this.ReplaceTextBox.Location = new System.Drawing.Point(92, 43);
            this.ReplaceTextBox.Name = "ReplaceTextBox";
            this.ReplaceTextBox.Size = new System.Drawing.Size(331, 26);
            this.ReplaceTextBox.TabIndex = 10;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(10, 50);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(83, 12);
            this.Label2.TabIndex = 11;
            this.Label2.Text = "Replace with:";
            // 
            // FindTextBox
            // 
            this.FindTextBox.Font = new System.Drawing.Font("ËÎÌו", 12F);
            this.FindTextBox.Location = new System.Drawing.Point(92, 11);
            this.FindTextBox.Name = "FindTextBox";
            this.FindTextBox.Size = new System.Drawing.Size(331, 26);
            this.FindTextBox.TabIndex = 8;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(10, 18);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(65, 12);
            this.Label1.TabIndex = 9;
            this.Label1.Text = "Find what:";
            // 
            // Find
            // 
            this.Find.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Find.Location = new System.Drawing.Point(71, 123);
            this.Find.Name = "Find";
            this.Find.Size = new System.Drawing.Size(75, 30);
            this.Find.TabIndex = 16;
            this.Find.Text = "Find";
            this.Find.UseVisualStyleBackColor = true;
            this.Find.Click += new System.EventHandler(this.Find_Click);
            // 
            // FindReplace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 157);
            this.Controls.Add(this.Find);
            this.Controls.Add(this.btnReplaceAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.ReplaceTextBox);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.FindTextBox);
            this.Controls.Add(this.Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindReplace";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FindReplace";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FindReplace_FormClosed);
            this.Load += new System.EventHandler(this.FindReplace_Load);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnReplaceAll;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.CheckBox WholeWord;
        internal System.Windows.Forms.CheckBox MatchCase;
        internal System.Windows.Forms.TextBox ReplaceTextBox;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox FindTextBox;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Button Find;
    }
}