using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using FTDPClient.consts;
using FTDPClient.functions;

namespace FTDPClient.forms
{
    public partial class ForePartWrite : Form
    {
        public string SetString = "";
        public bool IsCancel = true;
        public bool IsWriteTemplate = true;
        public bool IsWriteScript = true;
        public bool IsWriteStyle = true;
        public ForePartWrite()
        {
            InitializeComponent();
        }
        private void OK_Click(object sender, EventArgs e)
        {
            IsCancel = false;
            IsWriteTemplate = templatecheckBox1.Checked;
            IsWriteScript = scriptcheckBox2.Checked;
            IsWriteStyle = stylecheckBox3.Checked;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ForeConfig_Load(object sender, EventArgs e)
        {
            templatecheckBox1.Text = "template " + res.ft.str("FIInter.fugai");
            scriptcheckBox2.Text = "script " + res.ft.str("FIInter.fugai");
            stylecheckBox3.Text = "style " + res.ft.str("FIInter.fugai");
        }
    }
}