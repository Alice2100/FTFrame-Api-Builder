using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;
namespace SiteMatrix.forms
{
    public partial class EleAssis : Form
    {
        public static bool EleAssisShow = false;
        public static EleAssis EleAssisForm = null;
        public static IHTMLElement EleAssisEle = null;
        public EleAssis()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EleAssisShow = false;
            this.Close();
        }

        private void EleAssis_Load(object sender, EventArgs e)
        {
            if (EleAssisShow) {
                this.Close();
            }
            EleAssisShow = true;
            EleAssisForm = this;
        }
        public static void EleSelected(IHTMLElement ele)
        {
            if (!EleAssisShow) return;
            EleAssisEle = ele;
            if (EleAssisForm.RcheckBox1.Checked)
            {
                if (ele.id != null)
                {
                    ele.id = ele.id.Replace(EleAssisForm.R1textBox1.Text, EleAssisForm.R2textBox2.Text);
                    ele.setAttribute("name", ele.id);
                }
            }
            if (EleAssisForm.WcheckBox2.Checked)
            {
                ele.style.width = EleAssisForm.WtextBox3.Text;
            }
            EleAssisForm.IDtextBox4.Text = ele.id == null ? "" : ele.id;
            EleAssisForm.NametextBox5.Text = ele.getAttribute("name") == null ? "" : ele.getAttribute("name").ToString();
            EleAssisForm.ClasstextBox6.Text = ele.className == null ? "" : ele.className;
            EleAssisForm.WidthtextBox7.Text = (ele.style==null||ele.style.width == null) ? "" : ele.style.width.ToString();
            EleAssisForm.CsstextBox8.Text = (ele.style == null || ele.style.cssText == null) ? "" : ele.style.cssText;
            EleAssisForm.HTMLtextBox9.Text = ele.outerHTML;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EleAssisEle.id = EleAssisForm.IDtextBox4.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EleAssisEle.setAttribute("name", EleAssisForm.NametextBox5.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EleAssisEle.className = EleAssisForm.ClasstextBox6.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            EleAssisEle.style.width = EleAssisForm.WidthtextBox7.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            EleAssisEle.style.cssText = EleAssisForm.CsstextBox8.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            EleAssisEle.outerHTML= EleAssisForm.HTMLtextBox9.Text;
        }

    }
}
