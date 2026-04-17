using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FTDPClient.Style;
using FTDPClient.functions;
using FTDPClient.consts;
using FTDPClient.database;
using FTDPClient.forms;
using FTDPClient.SiteClass;
using mshtml;
using htmleditocx;
using FTDPClient.Page;
using FTDPClient.Common;
using System.Collections;
namespace FTDPClient.forms
{
    public partial class BugCheck : Form
    {
        public bool IsQuick = false;
        private string ActivePageID=null;
        public ArrayList UpdatePages = new ArrayList();
        public BugCheck()
        {
            InitializeComponent();
        }

        private void BugCheck_Load(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID == null)
            {
                MsgBox.Error("Site Not Opened!");
                this.Close();
            }
            Editor ed = form.getEditor();
            if (ed != null && !ed.isFreeFile)
            {
                radio1.Checked = true;
                label1.Text = ed.thisTitle;
                ActivePageID = ed.thisID;
                ed.savePage();
            }
            else
            {
                radio2.Checked = true;
                label1.Text = "";
            }
            if (IsQuick && ed != null && !ed.isFreeFile)
            {
                button1_Click(sender,e);
            }

            #region Language
            radio1.Text = res.form.GetString("dangqianjihuo");
            radio2.Text = res.form.GetString("xuanzezhiding");
            radio3.Text = res.form.GetString("zhenggezhandian");
            button1.Text = res.form.GetString("kaishi");
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BugResult br= new BugResult();
            if(checkBox1.Checked)
            {
                if (radio1.Checked)
                {
                    if (ActivePageID == null)
                    {
                        MsgBox.Error("No Active Page!");
                        return;
                    }
                    br.SinglePageID = ActivePageID;
                }
                else if (radio2.Checked)
                {
                    if (UpdatePages.Count == 0)
                    {
                        MsgBox.Error("No Page Selected!");
                        return;
                    }
                    br.SelFiles = UpdatePages;
                }
                else if (radio3.Checked)
                {
                br.IsAllSite = true;
                }
            }
            br.For_Back = checkBox1.Checked;
            br.For_Front = checkBox2.Checked;
            br.For_Rule = checkBox3.Checked;
            this.Close();
            br.Show();
        }

        private void pageselect_Click(object sender, EventArgs e)
        {
            PublishSel ps = new PublishSel();
            ps.SelFiles = UpdatePages;
            ps.ShowDialog();
            UpdatePages = ps.SelFiles;
            if (UpdatePages.Count == 0) pageselect.Text ="No Page Selected";
            else pageselect.Text = UpdatePages.Count+" Pages Selected";
        }
    }
}
