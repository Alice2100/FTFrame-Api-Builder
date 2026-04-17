namespace SiteMatrix.forms
{
    partial class Snippet
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
            this.CaptionText = new System.Windows.Forms.TextBox();
            this.ContentText = new System.Windows.Forms.TextBox();
            this.Caption = new System.Windows.Forms.Label();
            this.Content = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CaptionText
            // 
            this.CaptionText.Location = new System.Drawing.Point(71, 6);
            this.CaptionText.Name = "CaptionText";
            this.CaptionText.Size = new System.Drawing.Size(365, 21);
            this.CaptionText.TabIndex = 0;
            // 
            // ContentText
            // 
            this.ContentText.Location = new System.Drawing.Point(71, 38);
            this.ContentText.Multiline = true;
            this.ContentText.Name = "ContentText";
            this.ContentText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ContentText.Size = new System.Drawing.Size(365, 211);
            this.ContentText.TabIndex = 1;
            // 
            // Caption
            // 
            this.Caption.AutoSize = true;
            this.Caption.Location = new System.Drawing.Point(12, 6);
            this.Caption.Name = "Caption";
            this.Caption.Size = new System.Drawing.Size(53, 12);
            this.Caption.TabIndex = 2;
            this.Caption.Text = "Caption:";
            // 
            // Content
            // 
            this.Content.AutoSize = true;
            this.Content.Location = new System.Drawing.Point(12, 38);
            this.Content.Name = "Content";
            this.Content.Size = new System.Drawing.Size(53, 12);
            this.Content.TabIndex = 3;
            this.Content.Text = "Content:";
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(266, 255);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 4;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(361, 255);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Snippet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 284);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Content);
            this.Controls.Add(this.Caption);
            this.Controls.Add(this.ContentText);
            this.Controls.Add(this.CaptionText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Snippet";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Snippet";
            this.Load += new System.EventHandler(this.Snippet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox CaptionText;
        private System.Windows.Forms.TextBox ContentText;
        private System.Windows.Forms.Label Caption;
        private System.Windows.Forms.Label Content;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
    }
}