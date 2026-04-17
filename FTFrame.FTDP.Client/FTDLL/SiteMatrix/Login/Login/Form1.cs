using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;
namespace Login
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string username = "风娜江雪" + name + "山厉小雨";
            byte[] dataToHash = (new ASCIIEncoding()).GetBytes(username);
            byte[] hashvalue = ((System.Security.Cryptography.HashAlgorithm)System.Security.Cryptography.CryptoConfig.CreateFromName("MD5")).ComputeHash(dataToHash);
            string md5str = "";
            for (int i = 0; i < 16; i++)
            {
                md5str += Conversion.Hex(hashvalue[i]).ToUpper();
            }
            textBox2.Text=md5str;

            if (textBox3.Text.Trim().Equals(""))
            {
                textBox3.Text = getDecode(textBox4.Text);
            }
            else
            {
                textBox4.Text = getEncode(textBox3.Text);
            }
        }
        public static string getDecode(string str)
        {
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "maobb^#@$FVSD#$%SDF@#DSft234efwe";
            String sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);
            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength);
            }

            byte[] kkkk = ASCIIEncoding.ASCII.GetBytes(sTemp);
            sTemp = "#$ASDDASasdklasfui234978asdasdkf0maobb";
            mobjCryptoService.GenerateIV();
            bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength);
            }
            byte[] vvvv = ASCIIEncoding.ASCII.GetBytes(sTemp);

            byte[] bytIn = Convert.FromBase64String(str);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = kkkk;
            mobjCryptoService.IV = vvvv;
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
        public static string getEncode(string str)
        {
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "maobb^#@$FVSD#$%SDF@#DSft234efwe";

            String sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);

            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength);
            }
            byte[] kkkk = ASCIIEncoding.ASCII.GetBytes(sTemp);


            sTemp = "#$ASDDASasdklasfui234978asdasdkf0maobb";
            mobjCryptoService.GenerateIV();
            bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength);
            }
            byte[] vvvv = ASCIIEncoding.ASCII.GetBytes(sTemp);

            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(str.ToString());
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = kkkk;
            mobjCryptoService.IV = vvvv;
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void regbutton_Click(object sender, EventArgs e)
        {
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                string fileName = @"c:\Program Files\SWFTools\pdf2swf.exe";
                string str28 = @"C:\Syslive\DotForSite\v4.0.1\dcs2.0\userfiles\doc\b9c3ab0a_41b6_4080_8332_6030e8d22187_ctrl\f36969d6_bcbf_4a95_888d_a1e300f60df8.pdf.hid";
                string str29 = @"C:\Syslive\DotForSite\v4.0.1\dcs2.0\userfiles\doc\b9c3ab0a_41b6_4080_8332_6030e8d22187_ctrl\f36969d6_bcbf_4a95_888d_a1e300f60df8.pdf.hid.swf";
                string arguments = "  -t " + str28 + @" -s languagedir=c:\xpdf-chinese-simplified -s flashversion=9 -o " + str29;
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(fileName, arguments);
                process.StartInfo = info;
                process.Start();
            }

            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox5.Text = Func.GetEncode(textBox5.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox6.Text = Func.GetDecode(textBox6.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox7.Text = Func.GetEncode_L2(textBox7.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox8.Text = Func.GetDecode_L2(textBox8.Text);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            if (ofd.FileName != null && File.Exists(ofd.FileName))
            {
                new FileInfo(ofd.FileName).LastWriteTime = DateTime.Parse(textBox9.Text.Trim());
            }
        }
    }
}