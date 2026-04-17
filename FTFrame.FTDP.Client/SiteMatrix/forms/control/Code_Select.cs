using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using FTDPClient.functions;
using System.Data.OleDb;
using System.Reflection;
using System.IO;
using FTDPClient.consts;
using mshtml;

namespace FTDPClient.forms.control
{
    public partial class Code_Select : Form
    {
        public bool IsCancel = true;
        public string SetVal;
        public string ReflectString = null;
        public Code_Select()
        {
            InitializeComponent();
        }

        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            this.Text = res.ctl.str("Code_Select.text");			//选择公共方法
            string[] item0 = ReflectString.Split(new string[] { "[#0:]"},StringSplitOptions.RemoveEmptyEntries);
            var assemblyNode = new TreeNode(item0[0]);
            this.treeView1.Nodes.Add(assemblyNode);
            if (item0.Length > 1)
            {
                string[] item1 = item0[1].Split(new string[] { "[#1;]" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string _item1 in item1)
                {
                    string[] __item1 = _item1.Split(new string[] { "[#1:]" }, StringSplitOptions.RemoveEmptyEntries);
                    string text1 = __item1[0];
                    Color color1 = Color.Black;
                    object tag1 = null;
                    if (text1.StartsWith("(blue)")|| text1.StartsWith("(red2)"))
                    {
                        if (text1.StartsWith("(blue)")) color1 = Color.Blue;
                        else if (text1.StartsWith("(red2)")) color1 = Color.Red;
                        text1 = text1.Substring(6);
                        if (text1.StartsWith("(tag)"))
                        {
                            string[] _tag1 = text1.Split(new string[] { "(tag)" }, StringSplitOptions.None);
                            text1 = _tag1[2];
                            tag1 = _tag1[1];
                        }
                    }
                    var node1 = new TreeNode(text1);
                    node1.ForeColor = color1;
                    node1.Tag = tag1;
                    assemblyNode.Nodes.Add(node1);
                    if (__item1.Length > 1)
                    {
                        string[] item2 = __item1[1].Split(new string[] { "[#2;]" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string _item2 in item2)
                        {
                            string[] __item2 = _item2.Split(new string[] { "[#2:]" }, StringSplitOptions.RemoveEmptyEntries);
                            string text2 = __item2[0];
                            Color color2 = Color.Black;
                            object tag2 = null;
                            if (text2.StartsWith("(blue)") || text2.StartsWith("(red2)"))
                            {
                                if (text2.StartsWith("(blue)")) color2 = Color.Blue;
                                else if (text2.StartsWith("(red2)")) color2 = Color.Red;
                                text2 = text2.Substring(6);
                                if (text2.StartsWith("(tag)"))
                                {
                                    string[] _tag2 = text2.Split(new string[] { "(tag)" }, StringSplitOptions.None);
                                    text2 = _tag2[2];
                                    tag2 = _tag2[1];
                                }
                            }
                            var node2 = new TreeNode(text2);
                            node2.ForeColor = color2;
                            node2.Tag = tag2;
                            node1.Nodes.Add(node2);

                            if (__item2.Length > 1) { 
                            string[] item3 = __item2[1].Split(new string[] { "[#3;]" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string _item3 in item3)
                            {
                                string[] __item3 = _item3.Split(new string[] { "[#3:]" }, StringSplitOptions.RemoveEmptyEntries);
                                string text3 = __item3[0];
                                Color color3 = Color.Black;
                                    object tag3 = null;
                                    if (text3.StartsWith("(blue)") || text3.StartsWith("(red2)"))
                                    {
                                        if (text3.StartsWith("(blue)")) color3 = Color.Blue;
                                        else if (text3.StartsWith("(red2)")) color3 = Color.Red;
                                        text3 = text3.Substring(6);
                                        if (text3.StartsWith("(tag)"))
                                        {
                                            string[] _tag3 = text3.Split(new string[] { "(tag)" }, StringSplitOptions.None);
                                            text3 = _tag3[2];
                                            tag3 = _tag3[1];
                                        }
                                    }
                                var node3 = new TreeNode(text3);
                                    node3.ForeColor = color3;
                                    node3.Tag = tag3;
                                    node2.Nodes.Add(node3);
                                    if (__item3.Length > 1)
                                    {
                                        string[] item4 = __item3[1].Split(new string[] { "[#4;]" }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (string _item4 in item4)
                                        {
                                            string[] __item4 = _item4.Split(new string[] { "[#4:]" }, StringSplitOptions.RemoveEmptyEntries);
                                            string text4 = __item4[0];
                                            Color color4 = Color.Black;
                                            object tag4 = null;
                                            if (text4.StartsWith("(blue)") || text4.StartsWith("(red2)"))
                                            {
                                                if (text4.StartsWith("(blue)")) color4 = Color.Blue;
                                                else if (text4.StartsWith("(red2)")) color4 = Color.Red;
                                                text4 = text4.Substring(6);
                                                if (text4.StartsWith("(tag)"))
                                                {
                                                    string[] _tag4 = text4.Split(new string[] { "(tag)" }, StringSplitOptions.None);
                                                    text4 = _tag4[2];
                                                    tag4 = _tag4[1];
                                                }
                                            }
                                            var node4 = new TreeNode(text4);
                                            node4.ForeColor = color4;
                                            node4.Tag = tag4;
                                            node3.Nodes.Add(node4);
                                            if (__item4.Length > 1)
                                            {
                                                string[] item5 = __item4[1].Split(new string[] { "[#5;]" }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (string _item5 in item5)
                                                {
                                                    string[] __item5 = _item5.Split(new string[] { "[#5:]" }, StringSplitOptions.RemoveEmptyEntries);
                                                    string text5 = __item5[0];
                                                    Color color5 = Color.Black;
                                                    object tag5 = null;
                                                    if (text5.StartsWith("(blue)") || text5.StartsWith("(red2)"))
                                                    {
                                                        if (text5.StartsWith("(blue)")) color5 = Color.Blue;
                                                        else if (text5.StartsWith("(red2)")) color5 = Color.Red;
                                                        text5 = text5.Substring(6);
                                                        if (text5.StartsWith("(tag)"))
                                                        {
                                                            string[] _tag5 = text5.Split(new string[] { "(tag)" }, StringSplitOptions.None);
                                                            text5 = _tag5[2];
                                                            tag5 = _tag5[1];
                                                        }
                                                    }
                                                    var node5 = new TreeNode(text5);
                                                    node5.ForeColor = color5;
                                                    node5.Tag = tag5;
                                                    node4.Nodes.Add(node5);

                                                }
                                            }

                                        }
                                    }

                            }
                         }

                        }
                    }
                    node1.Expand();
                }
            }
            assemblyNode.Expand();
        }

        private void SelCol_SqlServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
             if(e.Node.ForeColor==Color.Blue || e.Node.ForeColor == Color.Red)
            {
                //Tuple<MethodInfo, string, string,string,bool> tuple = (Tuple<MethodInfo, string, string, string,bool>)e.Node.Tag;
                //MsgBox.Information(tuple.Item2+"."+ tuple.Item1.Name+"("+ tuple .Item3+ "):"+ tuple.Item1.ReturnType.Name);
                SetVal = e.Node.Tag.ToString();
                //if (tuple.Item5)
                //{
                //    SetVal = tuple.Item2 + "." + tuple.Item1.Name + "(" + tuple.Item3 + "):" + tuple.Item1.ReturnType.Name + ":" + tuple.Item4;
                //}
                //else
                //{
                //    SetVal = "new "+tuple.Item2 + "()." + tuple.Item1.Name + "(" + tuple.Item3 + "):" + tuple.Item1.ReturnType.Name + ":" + tuple.Item4;
                //}
                IsCancel = false;
                this.Close();
            }
        }
    }
}
