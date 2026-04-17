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
    public partial class ImportSel : Form
    {
        public bool IsCancel = true;
        public string FileName = null;
        public bool ForSingle = false;
        public string SingleFileCap = null;
        public ArrayList SelFiles = new ArrayList();
        public ImportSel()
        {
            InitializeComponent();
            treeView2.ImageList = globalConst.Imgs;
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            this.Text = res.SiteImport.GetString("m5");
            button1.Text = res.SiteImport.GetString("m7");
            button2.Text = res.SiteImport.GetString("m8");
            checkBox1.Text= res.SiteImport.GetString("m11");
            label1.Text = res.SiteImport.GetString("m6");
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
            if (!SiteClass.Site.ImportPlusInit(FileName, treeView2, label2))
            {
                MsgBox.Error("Import Init Error!");
                this.Close();
            }
            Application.DoEvents();

            label2.Visible = false;
            button1.Visible = true;
            checkBox1.Visible = true;
            label1.Visible = true;
            treeView2.Visible = true;
            treeView2.Nodes[0].Expand();

            if(ForSingle)
            {
                
                foreach (TreeNode node in treeView2.Nodes)
                {
                    node.Checked = true;
                    SetTreeNodeChecked(node);
                }
                GetSelFiles();
                int filesCount = 0;
                foreach(string[] item in SelFiles)
                {
                    int imageIndex = int.Parse(item[2]);
                    if(imageIndex!=16 && imageIndex!=17)
                    {
                        filesCount++;
                        SingleFileCap = item[3];
                    }
                }
                if (filesCount == 0) { MsgBox.Error("没有任何页面"); }
                else if (filesCount > 1) { MsgBox.Error("只能导入单个页面"); }
                else 
                {
                    IsCancel = false;
                }

                this.Close();
            }
        }
        private void SetTreeNodeChecked(TreeNode tn)
        {
            if (tn == null) return; // 若为空，则返回
            if (tn.Nodes.Count > 0)
            {
                foreach (TreeNode item in tn.Nodes)
                {
                    item.Checked = tn.Checked;
                    SetTreeNodeChecked(item);
                }
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
                    string[] obj = new string[] { ((string[])node.Tag)[2], GetFileFullPath(node), node.ImageIndex.ToString(),((string[])node.Tag)[1] };
                    if(!SelFiles.Contains(obj))SelFiles.Add(obj);
                    if (node.Parent != null) Loop4GetFilesParents(node.Parent);//自动加目录
                }
            }
            foreach (TreeNode cnode in node.Nodes)
            {
                Loop4GetFiles(cnode);
            }
        }
        private void Loop4GetFilesParents(TreeNode node)//自动加目录
        {
            if (node.ImageIndex != 16)//root
            {
                string[] obj = new string[] { ((string[])node.Tag)[2], GetFileFullPath(node), node.ImageIndex.ToString(), ((string[])node.Tag)[1] };
                if (!SelFiles.Contains(obj)) SelFiles.Add(obj);
            }
            if (node.Parent != null) Loop4GetFilesParents(node.Parent);
        }
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
            if (SelFiles.Count == 0) { MsgBox.Information(res.SiteImport.GetString("m9")); }
            else
            {
                IsCancel = false;
                this.Close();
            }
        }
    }
}