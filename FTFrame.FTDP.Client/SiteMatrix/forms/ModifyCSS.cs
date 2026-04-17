using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FTDPClient.consts;
using FTDPClient.functions;
using System.IO;

namespace FTDPClient.forms
{
    public partial class ModifyCSS : Form
    {
        public ModifyCSS()
        {
            InitializeComponent();
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            this.Text = res.ModifyDNS.GetString("__this");
            //this.Text = "Modify Site Default Style";
            button1.Text = res.ModifyDNS.GetString("button1");
            button2.Text = res.ModifyDNS.GetString("button2");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ModifyDNS_Load(object sender, EventArgs e)
        {
            try
            {
                string hostsfile = globalConst.CurSite.Path + "\\default.css";
            if (!file.Exists(hostsfile))
            {
                MsgBox.Warning(res.ModifyDNS.GetString("m1"));
                this.Close();
            }
            else
            {
                StreamReader sr=file.OpenText(hostsfile);
                textBox1.Text = sr.ReadToEnd();            
                sr.Close();
                textBox1.SelectionStart = 0;
                textBox1.SelectionLength = 0;
            }
            }
            catch (Exception ex)
            {
                MsgBox.Error(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string hostsfile = globalConst.CurSite.Path + "\\default.css";
                file.Delete(hostsfile);
                file.CreateText(hostsfile, textBox1.Text);
                this.Close();
            }
            catch (Exception ex)
            {
                MsgBox.Error(ex.Message);
            }
        }
    }
}