using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SiteMatrix.consts;
using SiteMatrix.functions;

namespace SiteMatrix.forms
{
    public partial class OpenWebPage : Form
    {
        public OpenWebPage()
        {
            InitializeComponent();
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            this.Text = res.OpenWebPage.GetString("_this");
            button1.Text = res.OpenWebPage.GetString("button1");
            button2.Text = res.OpenWebPage.GetString("button2");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fid = rdm.getID() + "_free.html";
            form.addFreeFileEditor(textBox1.Text, "Static Page", fid, fid, "Static Page", false);
            this.Close();
        }

        private void OpenWebPage_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                button1_Click(null, null);
            }
        }
    }
}