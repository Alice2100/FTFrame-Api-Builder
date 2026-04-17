using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FTDPClient.consts;
using FTDPClient.functions;
using mshtml;
namespace FTDPClient.forms
{
    public partial class FormName : Form
    {
        public string EleCaption;
        public bool cancel = true;
        public string EleName;
        public FormName()
        {
            InitializeComponent();
        }

        private void FormName_Load(object sender, EventArgs e)
        {

            label2.Text = EleCaption;
            label1.Text = res.form.GetString("String41");
            textBox1.Focus();
            button1.Text = res.form.GetString("String33");
            button2.Text = res.form.GetString("String34");
            if (globalConst.FormDataMode)
            {
                label3.Text = res.form.GetString("String99");
                label4.Text = res.form.GetString("String101");
                button3.Text = res.form.GetString("String102");
            }
            else
            {
                label3.Text = res.form.GetString("String42");
                label4.Text = res.form.GetString("String44");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (globalConst.FormDataMode)
            {
                if (FormData.TheFormData == null)
                {
                    FormData.TheFormData = new FormData();
                }
                FormData.TheFormData.fromSite = false;
                FormData.ele = null;
                FormData.FormDataShow();
                if (FormData.TheFormData.ReturnRow != null) textBox1.Text = FormData.TheFormData.ReturnRow;
            }
            else
            {
                textBox1.Text = rdm.getCombID();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (globalConst.FormDataMode)
            {
                if (!textBox1.Text.Trim().Equals("") && !str.IsNatural(textBox1.Text.Trim().Replace(".","")))
                {
                    MsgBox.Information(res.form.GetString("String43"));
                    return;
                }
                else
                {
                    EleName = textBox1.Text.Trim();
                    cancel = false;
                    this.Close();
                }
            }
            else
            {
                if (!str.IsNatural(textBox1.Text.Trim()))
                {
                    MsgBox.Information(res.form.GetString("String43"));
                    return;
                }
                else
                {
                    EleName = textBox1.Text.Trim();
                    cancel = false;
                    this.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}