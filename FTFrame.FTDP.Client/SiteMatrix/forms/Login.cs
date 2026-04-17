using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.VisualBasic;
using System.IO;
using FTDPClient.consts;
using FTDPClient.functions;
using DocumentFormat.OpenXml.Wordprocessing;

namespace FTDPClient.forms
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           this.Close();
           Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();
            string code=textBox2.Text.Trim();
            var Registered = false;
            try
            {
                var keyitems = str.getDecode(code).Split('|');
                if (keyitems[1] == "ftframe" && DateTime.TryParse(keyitems[2], out DateTime dt1))
                {
                    if (dt1.AddDays(1) >= DateTime.Now)
                    {
                        Registered = true;
                       globalConst.MdiForm.EndDate = dt1;
                    }
                }
            }
            catch { }
            if (!Registered)
            {
                MsgBox.Error("Key not Correct !");
            }
            else
            {
                var filename = Application.StartupPath + @"\update\user";
                if (File.Exists(filename)) File.Delete(filename);
                FileInfo fi = new FileInfo(filename);
                using (StreamWriter sw = fi.CreateText())
                {
                    sw.Write(code);
                    sw.Flush();
                    sw.Close();
                }
                globalConst.Key = code;
                this.Close();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}