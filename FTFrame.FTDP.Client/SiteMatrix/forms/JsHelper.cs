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
    public partial class JsHelper : Form
    {
        public string basetext="";
        public bool cancel = true;
        public JsHelper()
        {
            InitializeComponent();
        }

        private void TextEditor_Load(object sender, EventArgs e)
        {
            //richTextBox1.LoadFile(globalConst.AppPath+@"\jshelper.rtf");
            //richTextBox1.LoadFile("http://www.ftframe.com/doc/helper.rtf");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}