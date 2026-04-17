using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SiteMatrix.consts;

namespace SiteMatrix.forms
{
    public partial class LoadStat : Form
    {
        public LoadStat()
        {
            InitializeComponent();
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            label1.Text = res.LoadStat.GetString("label1");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadStat_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            SiteClass.Site.UpdateShareXml();
            this.Close();
        }
    }
}