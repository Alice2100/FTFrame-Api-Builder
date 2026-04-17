namespace FTDPClient.forms
{
    partial class TemplateManage
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
            this.tv = new System.Windows.Forms.TreeView();
            this.AddSort = new System.Windows.Forms.Button();
            this.AddTemplate = new System.Windows.Forms.Button();
            this.Rename = new System.Windows.Forms.Button();
            this.Delete = new System.Windows.Forms.Button();
            this.RefreshTV = new System.Windows.Forms.Button();
            this.CloseForm = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tv
            // 
            this.tv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tv.Location = new System.Drawing.Point(12, 12);
            this.tv.Name = "tv";
            this.tv.Size = new System.Drawing.Size(328, 319);
            this.tv.TabIndex = 0;
            // 
            // AddSort
            // 
            this.AddSort.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.AddSort.Location = new System.Drawing.Point(355, 12);
            this.AddSort.Name = "AddSort";
            this.AddSort.Size = new System.Drawing.Size(85, 23);
            this.AddSort.TabIndex = 1;
            this.AddSort.Text = "Add Sort";
            this.AddSort.UseVisualStyleBackColor = true;
            this.AddSort.Click += new System.EventHandler(this.AddSort_Click);
            // 
            // AddTemplate
            // 
            this.AddTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.AddTemplate.Location = new System.Drawing.Point(355, 41);
            this.AddTemplate.Name = "AddTemplate";
            this.AddTemplate.Size = new System.Drawing.Size(85, 23);
            this.AddTemplate.TabIndex = 2;
            this.AddTemplate.Text = "Add Template";
            this.AddTemplate.UseVisualStyleBackColor = true;
            this.AddTemplate.Click += new System.EventHandler(this.AddTemplate_Click);
            // 
            // Rename
            // 
            this.Rename.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Rename.Location = new System.Drawing.Point(355, 70);
            this.Rename.Name = "Rename";
            this.Rename.Size = new System.Drawing.Size(85, 23);
            this.Rename.TabIndex = 3;
            this.Rename.Text = "Rename";
            this.Rename.UseVisualStyleBackColor = true;
            this.Rename.Click += new System.EventHandler(this.Rename_Click);
            // 
            // Delete
            // 
            this.Delete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Delete.Location = new System.Drawing.Point(355, 99);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(85, 23);
            this.Delete.TabIndex = 4;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // RefreshTV
            // 
            this.RefreshTV.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.RefreshTV.Location = new System.Drawing.Point(355, 128);
            this.RefreshTV.Name = "RefreshTV";
            this.RefreshTV.Size = new System.Drawing.Size(85, 23);
            this.RefreshTV.TabIndex = 5;
            this.RefreshTV.Text = "Refresh";
            this.RefreshTV.UseVisualStyleBackColor = true;
            this.RefreshTV.Click += new System.EventHandler(this.RefreshTV_Click);
            // 
            // CloseForm
            // 
            this.CloseForm.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CloseForm.Location = new System.Drawing.Point(355, 308);
            this.CloseForm.Name = "CloseForm";
            this.CloseForm.Size = new System.Drawing.Size(85, 23);
            this.CloseForm.TabIndex = 6;
            this.CloseForm.Text = "Close";
            this.CloseForm.UseVisualStyleBackColor = true;
            this.CloseForm.Click += new System.EventHandler(this.CloseForm_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(355, 279);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Import...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TemplateManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 343);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CloseForm);
            this.Controls.Add(this.RefreshTV);
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.Rename);
            this.Controls.Add(this.AddTemplate);
            this.Controls.Add(this.AddSort);
            this.Controls.Add(this.tv);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TemplateManage";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TemplateManage";
            this.Load += new System.EventHandler(this.TemplateManage_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tv;
        private System.Windows.Forms.Button AddSort;
        private System.Windows.Forms.Button AddTemplate;
        private System.Windows.Forms.Button Rename;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.Button RefreshTV;
        private System.Windows.Forms.Button CloseForm;
        private System.Windows.Forms.Button button1;

    }
}