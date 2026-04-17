using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FTDPClient.consts;
using FTDPClient.functions;
using System.IO;
namespace FTDPClient.forms
{
    public partial class TemplateManage : Form
    {
        public TemplateManage()
        {
            InitializeComponent();
            tv.ImageList = globalConst.Imgs;
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            this.Text=res.TemplateManage.GetString("_this");
            AddSort.Text = res.TemplateManage.GetString("AddSort");
            AddTemplate.Text = res.TemplateManage.GetString("AddTemplate");
            Rename.Text = res.TemplateManage.GetString("Rename");
            Delete.Text = res.TemplateManage.GetString("Delete");
            RefreshTV.Text = res.TemplateManage.GetString("RefreshTV");
            button1.Text = res.TemplateManage.GetString("button1");
            CloseForm.Text = res.TemplateManage.GetString("CloseForm");
        }
        private void TemplateManage_Load(object sender, EventArgs e)
        {
            InitTV();
        }
        private void InitTV()
        {
            tv.Nodes.Clear();
            DirectoryInfo di = new DirectoryInfo(globalConst.TemplatePath);
            foreach (DirectoryInfo d in di.GetDirectories())
            {
                TreeNode node = new TreeNode(d.Name, 24, 24);
                node.Tag = d.FullName;
                tv.Nodes.Add(node);
                foreach (FileInfo f in d.GetFiles())
                {
                    if (f.Extension.Equals(".template"))
                    {
                        TreeNode nodef = new TreeNode(f.Name.Substring(0, f.Name.Length-9), 25, 25);
                        nodef.Tag = f.FullName;
                        node.Nodes.Add(nodef);
                    }
                }
            }
        }
        private void CloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddSort_Click(object sender, EventArgs e)
        {
            InputName iname=new InputName();
            iname.ShowDialog();
            try
            {
                if (!iname.NameValue.Text.Trim().Equals(""))
                {
                    dir.CreateDirectory(globalConst.TemplatePath + @"\" + iname.NameValue.Text.Trim());
                    TreeNode node = new TreeNode(iname.NameValue.Text.Trim(), 24, 24);
                    node.Tag = globalConst.TemplatePath + "\\" + iname.NameValue.Text.Trim();
                    tv.Nodes.Add(node);
                }
            }
            catch(Exception ex)
            {
                MsgBox.Error(ex.Message);
            }
        }

        private void AddTemplate_Click(object sender, EventArgs e)
        {
            if(tv.SelectedNode!=null)
            {
                OpenFileDialog ofd=new OpenFileDialog();
                ofd.Filter = "FTDP Site File(*.site;*.template)|*.site;*.template";
                ofd.ShowDialog();
                if (!ofd.FileName.Equals(""))
                {
                    InputName iname = new InputName();
                    iname.ShowDialog();
                    if (!iname.NameValue.Text.Trim().Equals(""))
                    {
                        if (tv.SelectedNode.ImageIndex == 25)
                        {
                            tv.SelectedNode = tv.SelectedNode.Parent;
                        }
                        if (File.Exists(tv.SelectedNode.Tag.ToString() + "\\" + iname.NameValue.Text.Trim() + ".template"))
                        {
                            MsgBox.Warning(iname.NameValue.Text.Trim() + res.TemplateManage.GetString("m1"));
                        }
                        else
                        {
                            try
                            {
                                file.Copy(ofd.FileName, tv.SelectedNode.Tag.ToString() + "\\" + iname.NameValue.Text.Trim() + ".template");
                                TreeNode nodef = new TreeNode(iname.NameValue.Text.Trim(), 25, 25);
                                nodef.Tag = tv.SelectedNode.Tag.ToString() + "\\" + iname.NameValue.Text.Trim() + ".template";
                                tv.SelectedNode.Nodes.Add(nodef);
                            }
                            catch(Exception ex)
                            {
                                MsgBox.Error(ex.Message);
                            }
                        }
                    }
                }     
            }
        }

        private void Rename_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode != null)
            {
                if (tv.SelectedNode.ImageIndex == 24)
                {
                    try
                    {
                        InputName iname = new InputName();
                        iname.NameValue.Text = tv.SelectedNode.Text;
                        iname.ShowDialog();
                        if (!iname.NameValue.Text.Trim().Equals(""))
                        {
                            try
                            {
                                dir.Move(tv.SelectedNode.Tag.ToString(), globalConst.TemplatePath + "\\" + iname.NameValue.Text.Trim());
                                tv.SelectedNode.Text = iname.NameValue.Text.Trim();
                                tv.SelectedNode.Tag = globalConst.TemplatePath + "\\" + iname.NameValue.Text.Trim();
                            }
                            catch (Exception ex)
                            {
                                MsgBox.Error(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error(ex.Message);
                    }
                }
                else if (tv.SelectedNode.ImageIndex == 25)
                {
                    try
                    {
                        InputName iname = new InputName();
                        iname.NameValue.Text = tv.SelectedNode.Text;
                        iname.ShowDialog();
                        if (!iname.NameValue.Text.Trim().Equals(""))
                        {
                            try
                            {
                                file.Move(tv.SelectedNode.Tag.ToString(), tv.SelectedNode.Parent.Tag.ToString() + "\\" + iname.NameValue.Text.Trim() + ".template");
                                tv.SelectedNode.Text = iname.NameValue.Text.Trim();
                                tv.SelectedNode.Tag = tv.SelectedNode.Parent.Tag.ToString() + "\\" + iname.NameValue.Text.Trim() + ".template";
                            }
                            catch (Exception ex)
                            {
                                MsgBox.Error(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error(ex.Message);
                    }
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode != null)
            {
                if (tv.SelectedNode.ImageIndex == 24)
                {
                    try
                    {
                        if (MsgBox.OKCancel(res.TemplateManage.GetString("m2")).Equals(DialogResult.OK))
                        {
                            dir.Delete(tv.SelectedNode.Tag.ToString(), true);
                            tv.SelectedNode.Remove();
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error(ex.Message);
                    }
                }
                else if (tv.SelectedNode.ImageIndex == 25)
                {
                    try
                    {
                        if (MsgBox.OKCancel(res.TemplateManage.GetString("m3")).Equals(DialogResult.OK))
                        {
                            file.Delete(tv.SelectedNode.Tag.ToString());
                            tv.SelectedNode.Remove();
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error(ex.Message);
                    }
                }
            }
        }

        private void RefreshTV_Click(object sender, EventArgs e)
        {
            InitTV();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(globalConst.CurSite.ID!=null&&tv.SelectedNode != null&&tv.SelectedNode.ImageIndex == 25)
            {
                SiteImport si=new SiteImport();
                si.DirectImport = true;
                si.filename = tv.SelectedNode.Tag.ToString();
                si.ShowDialog();
            }
        }
    }
}