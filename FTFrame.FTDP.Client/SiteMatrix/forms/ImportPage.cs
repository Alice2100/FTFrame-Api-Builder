using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FTDPClient.Common;
using FTDPClient.consts;
using FTDPClient.functions;
namespace FTDPClient.forms
{
    public partial class ImportPage : Form
    {
        public ImportPage()
        {
            InitializeComponent();
            treeView1.ImageList = globalConst.Imgs;
            treeView2.ImageList = globalConst.Imgs;
            ApplyLanguage();
        }
        FileExplorer fe = new FileExplorer();
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ApplyLanguage()
        {
            this.Text = res.ImportPage.GetString("_this");
            button1.Text = res.ImportPage.GetString("button1");
            button2.Text = res.ImportPage.GetString("button2");
        }
        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Text == "")
            {
                TreeNode node = fe.EnumerateDirectory(e.Node);
            }
            if (e.Node.ImageIndex==3)
            {
                e.Node.ImageIndex = 7;
                e.Node.SelectedImageIndex = 7;
            }
        }

        private void ImportPage_Load(object sender, EventArgs e)
        {
            fe.CreateTree(treeView1);
            SiteClass.Site.buildSiteExplorerTree(treeView2,false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string notSelectCorrectFile = res.ImportPage.GetString("m1");
            string notSelectCorrectSiteNode = res.ImportPage.GetString("m2");
            if (treeView1.SelectedNode == null)
            {
                MsgBox.Warning(notSelectCorrectFile);
                return;
            }
            string filename = treeView1.SelectedNode.Text.ToLower();
            if (!filename.EndsWith(".html") && !filename.EndsWith(".htm"))
            {
                MsgBox.Warning(notSelectCorrectFile);
                return;
            }
            filename = treeView1.SelectedNode.FullPath.Substring(12);
            if (treeView2.SelectedNode == null || (!tree.getTypeFromID(tree.getID(treeView2.SelectedNode)).Equals("drct") && !tree.getTypeFromID(tree.getID(treeView2.SelectedNode)).Equals("root")))
            {
                MsgBox.Warning(notSelectCorrectSiteNode);
                return;
            }
            AddPage ap = new AddPage();
            ap.PageType = 0;
            ap.isEdit = false;
            ap.CustomTree = treeView2;
            filename = filename.Replace("\\\\", "\\").Replace("\\\\", "\\");
            ap.CustomFileName = filename;
            ap.ShowDialog();
        }
        private TreeNode CurNode1 = null;
        private TreeNode CurNode2 = null;

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (CurNode1 != null)
            {
                CurNode1.BackColor = Color.White;
                CurNode1.ForeColor = Color.Black;
            }
            treeView1.SelectedNode.BackColor = System.Drawing.SystemColors.Highlight;
            treeView1.SelectedNode.ForeColor = Color.White;
            CurNode1 = treeView1.SelectedNode;
        }

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

        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.ImageIndex == 7)
            {
                e.Node.ImageIndex = 3;
                e.Node.SelectedImageIndex = 3;
            }
        }
    }
}