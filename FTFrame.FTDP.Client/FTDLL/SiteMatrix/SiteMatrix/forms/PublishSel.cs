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
using System.Collections;
namespace SiteMatrix.forms
{
    public partial class PublishSel : Form
    {
        public ArrayList SelFiles=new ArrayList();
        public PublishSel()
        {
            InitializeComponent();
            treeView2.ImageList = globalConst.Imgs;
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            this.Text = res.SiteImport.GetString("String4");
            button1.Text = res.SiteImport.GetString("String2");
            button2.Text = res.SiteImport.GetString("m8");
            button3.Text = res.SiteImport.GetString("String1");
            label1.Text = res.SiteImport.GetString("String3");
        }

        private void ImportSel_Load(object sender, EventArgs e)
        {
            
        }
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ImportSel_Shown(object sender, EventArgs e)
        {


            Application.DoEvents();
            if (!SiteClass.Site.PageSelInit(treeView2, label2))
            {
                MsgBox.Error("Page Init Error!");
                this.Close();
            }

            loopnodes(treeView2.Nodes[0]);

            Application.DoEvents();

            label2.Visible = false;
            button1.Visible = true; 
            button3.Visible = true;
            label1.Visible = true;
            treeView2.Visible = true;
            treeView2.Nodes[0].Expand();
        }
        private void loopnodes(TreeNode pnode)
        {
            foreach (TreeNode node in pnode.Nodes)
            {
                string id = ((string[])node.Tag)[2];
                foreach (object obj in SelFiles)
                {
                    if (((string[])obj)[0].Equals(id))
                    {
                        node.Checked = true;
                        break;
                    }
                }
                loopnodes(node);
            }
        }
        private void treeView2_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!e.Action.Equals(TreeViewAction.Unknown))
            {
                TreeNodeParentsDo(e.Node, e.Node.Checked);
                TreeNodeChildsDo(e.Node, e.Node.Checked);
            }
        }
        private void TreeNodeParentsDo(TreeNode node, bool isChecked)
        {
            if (node.Parent != null)
            {
                if (!isChecked) node.Parent.Checked = false;
                else
                {
                    bool isAllChecked = true;
                    foreach (TreeNode cnode in node.Parent.Nodes)
                    {
                        if (!cnode.Checked)
                        {
                            isAllChecked = false;
                            break;
                        }
                    }
                    if (isAllChecked)
                    {
                        node.Parent.Checked = true;
                    }
                }

                TreeNodeParentsDo(node.Parent, node.Parent.Checked);
            }
        }
        private void TreeNodeChildsDo(TreeNode pnode, bool isChecked)
        {
            foreach (TreeNode node in pnode.Nodes)
            {
                node.Checked = isChecked;
                TreeNodeChildsDo(node, isChecked);
            }
        }
        private void Loop4GetFiles(TreeNode node)
        {
            if (node.Checked)
            {
                if (node.ImageIndex != 16)//root
                {
                    string[] obj = new string[] { ((string[])node.Tag)[2], GetFileFullPath(node), node.ImageIndex.ToString() };
                    if(!SelFiles.Contains(obj))SelFiles.Add(obj);
                    //if (node.Parent != null) Loop4GetFilesParents(node.Parent);//自动加目录
                }
            }
            foreach (TreeNode cnode in node.Nodes)
            {
                Loop4GetFiles(cnode);
            }
        }
        //private void Loop4GetFilesParents(TreeNode node)//自动加目录
        //{
        //    if (node.ImageIndex != 16)//root
        //    {
        //        string[] obj = new string[] { ((string[])node.Tag)[2], GetFileFullPath(node), node.ImageIndex.ToString() };
        //        if (!SelFiles.Contains(obj)) SelFiles.Add(obj);
        //    }
        //    if (node.Parent != null) Loop4GetFilesParents(node.Parent);
        //}
        private string GetFileFullPath(TreeNode node)
        {
            return (node.Parent.Equals(treeView2.Nodes[0])?"":GetFileFullPath(node.Parent)) + "\\" +(((string[])node.Tag)[0]);
        }
        private void GetSelFiles()
        {
            SelFiles = new ArrayList();
            Loop4GetFiles(treeView2.Nodes[0]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetSelFiles();
            if (SelFiles.Count == 0) { 
                MsgBox.Information(res.SiteImport.GetString("String5"));
                this.Close();
            }
            else
            {
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SelFiles.Clear();
            this.Close();
        }
    }
}