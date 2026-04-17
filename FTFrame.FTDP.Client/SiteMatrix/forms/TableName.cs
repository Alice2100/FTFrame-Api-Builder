using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FTDPClient.consts;

namespace FTDPClient.forms
{
    public partial class TableName : Form
    {

        public TableName()
        {
            InitializeComponent();
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            OK.Text = res.InputName.GetString("OK");
            Cancel.Text = res.InputName.GetString("Cancel");
        }
        private void OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            NameValue.Text = "";
            this.Close();
        }

        private void NameValue_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode.Equals(Keys.Enter))
            {
                this.Close();
            }
        }

 
    }
}