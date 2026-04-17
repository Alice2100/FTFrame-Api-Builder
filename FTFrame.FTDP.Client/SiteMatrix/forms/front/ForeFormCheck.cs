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
    public partial class ForeFormCheck : Form
    {
        public string ValidType = "";
        public string ValidCustomJs = "";
        public bool IsCancel = true;
        bool inited = false;
        public ForeFormCheck()
        {
            InitializeComponent();
        }
        private void OK_Click(object sender, EventArgs e)
        {
            ValidType = comboBox1.Text == res.ft.str("FFCVali.001") ? "" : comboBox1.Text;
            ValidCustomJs = Custom_Js_Box.Enabled ? Custom_Js_Box.Text.Trim() : "";
            IsCancel = false;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ForeConfig_Load(object sender, EventArgs e)
        {
            label1.Text = res.ft.str("FFCVali.label1");
            label2.Text = res.ft.str("FFCVali.label2");
            label3.Text = res.ft.str("FFCVali.label3");
            label1.Text = res.ft.str("FFCVali.label1");
            comboBox1.Items.Clear();
            comboBox1.Items.Add(res.ft.str("FFCVali.001"));
            comboBox1.Items.Add(res.ft.str("FFCVali.002"));
            comboBox1.Items.Add(res.ft.str("FFCVali.003"));
            comboBox1.Items.Add(res.ft.str("FFCVali.004"));
            comboBox1.Items.Add(res.ft.str("FFCVali.005"));
            comboBox1.Items.Add(res.ft.str("FFCVali.006"));
            comboBox1.Items.Add(res.ft.str("FFCVali.007"));
            comboBox1.Items.Add(res.ft.str("FFCVali.008"));
            comboBox1.Items.Add(res.ft.str("FFCVali.009"));
            comboBox1.Items.Add(res.ft.str("FFCVali.010"));
            comboBox1.Items.Add(res.ft.str("FFCVali.011"));
            comboBox1.Items.Add(res.ft.str("FFCVali.012"));
            comboBox1.SelectedIndex = 0;
            comboBox1.Text = ValidType == "" ? res.ft.str("FFCVali.001") : ValidType;
            if (ValidType.StartsWith("#")) comboBox1.Text = res.ft.str("FFCVali.012");
            Custom_Js_Box.Text = ValidCustomJs;
            Custom_Js_Box.Enabled = (comboBox1.Text == res.ft.str("FFCVali.012"));
            inited = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!inited) return;
            if (comboBox1.Text == res.ft.str("FFCVali.011"))
            {
                comboBox1.Text = res.ft.str("FFCVali.012");
                Custom_Js_Box.Text = "#LengthRange:0,100";
                Custom_Js_Box.Enabled = true;
            }
            else if (comboBox1.Text != res.ft.str("FFCVali.012"))
            {
                Custom_Js_Box.Enabled = false;
                OK_Click(sender, e);
            }
            else
            {
                Custom_Js_Box.Enabled = true;
            }
        }
    }
}