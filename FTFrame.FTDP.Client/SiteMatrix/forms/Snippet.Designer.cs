namespace FTDPClient.forms
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
            this.Caption = new System.Windows.Forms.Label();
            this.Content = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.textEditorControl1 = new ICSharpCode.TextEditor.TextEditorControl();
            this.SuspendLayout();
            // 
            // CaptionText
            // 
            this.CaptionText.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CaptionText.Location = new System.Drawing.Point(71, 6);
            this.CaptionText.Name = "CaptionText";
            this.CaptionText.Size = new System.Drawing.Size(869, 30);
            this.CaptionText.TabIndex = 0;
            // 
            // Caption
            // 
            this.Caption.AutoSize = true;
            this.Caption.Location = new System.Drawing.Point(12, 9);
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
            this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OK.Location = new System.Drawing.Point(770, 479);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 30);
            this.OK.TabIndex = 4;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Cancel.Location = new System.Drawing.Point(865, 479);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 30);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // textEditorControl1
            // 
            this.textEditorControl1.BackColor = System.Drawing.SystemColors.Control;
            this.textEditorControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textEditorControl1.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEditorControl1.Highlighting = null;
            this.textEditorControl1.Location = new System.Drawing.Point(71, 42);
            this.textEditorControl1.Name = "textEditorControl1";
            this.textEditorControl1.ShowVRuler = false;
            this.textEditorControl1.Size = new System.Drawing.Size(869, 431);
            this.textEditorControl1.TabIndex = 14;
            // 
            // Snippet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 514);
            this.Controls.Add(this.textEditorControl1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Content);
            this.Controls.Add(this.Caption);
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
        private System.Windows.Forms.Label Caption;
        private System.Windows.Forms.Label Content;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private ICSharpCode.TextEditor.TextEditorControl textEditorControl1;
    }
}