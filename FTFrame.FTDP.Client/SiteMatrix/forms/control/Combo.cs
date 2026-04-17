using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FTDPClient.consts;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using ICSharpCode.TextEditor.Document;
using FTDPClient.database;
namespace FTDPClient.forms
{
	public class Combo : System.Windows.Forms.Form
	{
        public string[] array;
        public string reStr=null;
        public int? reInt=null;
        public bool IsCancel = true;
        private Button Close;
        private Button OK;
        private ComboBox comboBox1;

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;

        public Combo()
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
            this.Close = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Close
            // 
            this.Close.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Close.Location = new System.Drawing.Point(200, 87);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 30);
            this.Close.TabIndex = 5;
            this.Close.Text = "&Close";
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // OK
            // 
            this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OK.Location = new System.Drawing.Point(108, 87);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 30);
            this.OK.TabIndex = 6;
            this.OK.Text = "&OK";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("宋体", 12F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(32, 29);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(245, 24);
            this.comboBox1.TabIndex = 7;
            // 
            // Combo
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(312, 141);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Combo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select";
            this.Load += new System.EventHandler(this.SQL_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SQL_KeyDown);
            this.ResumeLayout(false);

		}

		#endregion
        private void ApplyLanguage()
        {
            OK.Text = res.ctl.str("SQL.OK");            //确定(&O)
            Close.Text = res.ctl.str("SQL.Close");          //关闭(&C)
          
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            reInt = comboBox1.SelectedIndex;
            reStr = comboBox1.Text;
            IsCancel = false;
            this.Close();
        }
        private void SQL_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach(var str in array)
            {
                comboBox1.Items.Add(str);
            }
            if(reInt!=null) comboBox1.SelectedIndex = reInt.Value;
            else if (reStr != null) comboBox1.Text = reStr;
        }
        private void SQL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }

       
}
