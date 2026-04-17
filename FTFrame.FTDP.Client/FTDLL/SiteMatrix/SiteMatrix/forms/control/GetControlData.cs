using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SiteMatrix.Common;
using SiteMatrix.consts;
using SiteMatrix.functions;
namespace SiteMatrix.forms
{
    public partial class GetControlData : Form
    {
        public string ReturnURL = "";
        public bool IsCancel = false;
        public static string ControlName = "";
        public GetControlData()
        {
            InitializeComponent();
            treeView2.ImageList = globalConst.Imgs;
            ApplyLanguage();
        }
        FileExplorer fe = new FileExplorer();
        private void button2_Click(object sender, EventArgs e)
        {
            IsCancel = true;
            this.Close();
        }
        private void ApplyLanguage()
        {
            this.Text = res.GetNode.GetString("String2");
            checkBox1.Text = res.GetNode.GetString("checkBox1");
            button3.Text = res.GetNode.GetString("button3");
            button1.Text = res.GetNode.GetString("button1");
            button2.Text = res.GetNode.GetString("button2");
        }
        private void ImportPage_Load(object sender, EventArgs e)
        {
            SiteClass.Site.buildControlExplorerTree(treeView2, ControlName);
            if(!ReturnURL.Equals(""))
            {
                textBox1.Text = ReturnURL;
                checkBox1.Checked = true;
            }
            else
            {
                textBox1.Text = "";
                checkBox1.Checked = false;
            }
        }
        private bool JustLoading = true;
        private TreeNode CurNode2 = null;


        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (CurNode2 != null)
            {
                CurNode2.BackColor = Color.White;
                CurNode2.ForeColor = Color.Black;
            }
            treeView2.SelectedNode.BackColor = System.Drawing.SystemColors.Highlight;
            treeView2.SelectedNode.ForeColor = Color.White;
            CurNode2 = treeView2.SelectedNode;
            if (!JustLoading)
            {
                checkBox1.Checked = false;
            }
            JustLoading = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled=checkBox1.Checked;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                ReturnURL=textBox1.Text;
            }
            else
            {
                if(treeView2.SelectedNode!=null)
                {
                    if (((string[])treeView2.SelectedNode.Tag)[2].EndsWith("_instance"))
                    {
                        ReturnURL = ((string[])treeView2.SelectedNode.Tag)[0];
                    }
                    else
                    {
                        ReturnURL = "";
                    }
                }
                else
                {
                    ReturnURL = "";
                }
            }
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReturnURL = "";
            this.Close();
        }

        private void treeView2_DoubleClick(object sender, EventArgs e)
        {
            button1_Click_1(sender, e);
        }

    }
}