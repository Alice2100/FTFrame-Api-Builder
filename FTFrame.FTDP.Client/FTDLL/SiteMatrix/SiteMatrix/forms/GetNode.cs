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
    public partial class GetNode : Form
    {
        public string ReturnURL = "";
        public bool IsCancel = false;
        public bool IsJustSelectFile = false;
        public GetNode()
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
            this.Text = res.GetNode.GetString("_this");
            checkBox1.Text = res.GetNode.GetString("checkBox1");
            button3.Text = res.GetNode.GetString("button3");
            label1.Text = res.GetNode.GetString("label1");
            button1.Text = res.GetNode.GetString("button1");
            button2.Text = res.GetNode.GetString("button2");
        }
        private void ImportPage_Load(object sender, EventArgs e)
        {
            SiteClass.Site.buildSiteExplorerTree(treeView2,true);
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
            comboBox1.Enabled = !checkBox1.Checked;
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
                    ReturnURL = tree.getPath(treeView2.SelectedNode);
                        ReturnURL = ReturnURL.Replace("\\","/");
                        if(tree.getNodeType(treeView2.SelectedNode).Equals(tree.NodeType.Page))
                        {
                            if (!IsJustSelectFile)
                            {
                                ReturnURL = ReturnURL + comboBox1.Text;
                            }
                        }
                        else
                        {
                            if (!IsJustSelectFile)
                            {
                                ReturnURL = ReturnURL + "/";
                            }
                            else
                            {
                                MsgBox.Warning(res.GetNode.GetString("String1"));
                                return;
                            }
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

    }
}