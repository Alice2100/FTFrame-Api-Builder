namespace FTDPClient.forms
{
    partial class ForePartWrite
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.stylecheckBox3 = new System.Windows.Forms.CheckBox();
            this.scriptcheckBox2 = new System.Windows.Forms.CheckBox();
            this.templatecheckBox1 = new System.Windows.Forms.CheckBox();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.stylecheckBox3);
            this.splitContainer1.Panel1.Controls.Add(this.scriptcheckBox2);
            this.splitContainer1.Panel1.Controls.Add(this.templatecheckBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.OK);
            this.splitContainer1.Panel2.Controls.Add(this.Cancel);
            this.splitContainer1.Size = new System.Drawing.Size(868, 255);
            this.splitContainer1.SplitterDistance = 212;
            this.splitContainer1.TabIndex = 3;
            // 
            // stylecheckBox3
            // 
            this.stylecheckBox3.AutoSize = true;
            this.stylecheckBox3.Checked = true;
            this.stylecheckBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stylecheckBox3.Font = new System.Drawing.Font("ËÎÌå", 14F);
            this.stylecheckBox3.Location = new System.Drawing.Point(95, 146);
            this.stylecheckBox3.Name = "stylecheckBox3";
            this.stylecheckBox3.Size = new System.Drawing.Size(152, 28);
            this.stylecheckBox3.TabIndex = 6;
            this.stylecheckBox3.Text = "style ¸²¸Ç";
            this.stylecheckBox3.UseVisualStyleBackColor = true;
            // 
            // scriptcheckBox2
            // 
            this.scriptcheckBox2.AutoSize = true;
            this.scriptcheckBox2.Checked = true;
            this.scriptcheckBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scriptcheckBox2.Font = new System.Drawing.Font("ËÎÌå", 14F);
            this.scriptcheckBox2.Location = new System.Drawing.Point(95, 94);
            this.scriptcheckBox2.Name = "scriptcheckBox2";
            this.scriptcheckBox2.Size = new System.Drawing.Size(164, 28);
            this.scriptcheckBox2.TabIndex = 5;
            this.scriptcheckBox2.Text = "script ¸²¸Ç";
            this.scriptcheckBox2.UseVisualStyleBackColor = true;
            // 
            // templatecheckBox1
            // 
            this.templatecheckBox1.AutoSize = true;
            this.templatecheckBox1.Checked = true;
            this.templatecheckBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.templatecheckBox1.Font = new System.Drawing.Font("ËÎÌå", 14F);
            this.templatecheckBox1.Location = new System.Drawing.Point(95, 39);
            this.templatecheckBox1.Name = "templatecheckBox1";
            this.templatecheckBox1.Size = new System.Drawing.Size(188, 28);
            this.templatecheckBox1.TabIndex = 4;
            this.templatecheckBox1.Text = "template ¸²¸Ç";
            this.templatecheckBox1.UseVisualStyleBackColor = true;
            // 
            // OK
            // 
            this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OK.Location = new System.Drawing.Point(641, 1);
            this.OK.Margin = new System.Windows.Forms.Padding(4);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(100, 38);
            this.OK.TabIndex = 1;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Cancel.Location = new System.Drawing.Point(757, 1);
            this.Cancel.Margin = new System.Windows.Forms.Padding(4);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(100, 38);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ForePartWrite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 255);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ForePartWrite";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Component Part Overwrite Select";
            this.Load += new System.EventHandler(this.ForeConfig_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.CheckBox stylecheckBox3;
        private System.Windows.Forms.CheckBox scriptcheckBox2;
        private System.Windows.Forms.CheckBox templatecheckBox1;
    }
}