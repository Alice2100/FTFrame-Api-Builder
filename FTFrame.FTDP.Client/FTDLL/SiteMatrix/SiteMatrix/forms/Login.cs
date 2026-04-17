using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.VisualBasic;
namespace SiteMatrix.forms
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
            string name = textBox1.Text;
            string code=textBox2.Text;

            string username = SiteMatrix.consts.globalConst.FullLeft + name + SiteMatrix.consts.globalConst.FullRight;
            byte[] dataToHash = (new ASCIIEncoding()).GetBytes(username);
            byte[] hashvalue=((System.Security.Cryptography.HashAlgorithm)System.Security.Cryptography.CryptoConfig.CreateFromName("MD5")).ComputeHash(dataToHash);
            string md5str = "";
            for(int i=0;i<16;i++)
            {
                md5str += Conversion.Hex(hashvalue[i]).ToUpper();
            }
            if(code.Equals(md5str))
            {

                //RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\D4soft\HostKeys");
                Registry.CurrentUser.CreateSubKey(@"Software\F1D2T3P\HostKeys").SetValue("name", name);
                Registry.CurrentUser.CreateSubKey(@"Software\F1D2T3P\HostKeys").SetValue("code", code);
                MessageBox.Show("Register successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //SiteMatrix.consts.globalConst.FullVersion = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Register failed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}