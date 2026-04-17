using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SiteMatrix.SiteClass;
using SiteMatrix.functions;
using SiteMatrix.consts;
using System.Collections;
namespace SiteMatrix.forms
{
    public partial class SiteImport : Form
    {
        public SiteImport()
        {
            InitializeComponent();
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            this.Text = res.SiteImport.GetString("_this");
        }
        public bool DirectImport = false;
        public bool AsTemplate = false;
        public bool ForSingle = false;
        public string filename;
        private void SiteImport_Load(object sender, EventArgs e)
        {
            if (DirectImport)
            {
                if (MsgBox.OKCancel(res.SiteImport.GetString("m1")).Equals(DialogResult.Cancel))
                {
                    this.Close();
                    return;
                }
                timer1.Enabled = true;
                return;
            }
            if (AsTemplate) this.Text = "Import Template";
            else
                this.Text = "Import Site";
            OpenFileDialog sfd = new OpenFileDialog();

            if (AsTemplate)
            {
                sfd.Filter = "FTDP Template(*.template)|*.template";
                sfd.InitialDirectory = globalConst.TemplatePath;
            }
            else
                sfd.Filter = "FTDP Site(*.site)|*.site";

            sfd.ShowDialog();
            if (sfd.FileName.Equals(""))
            {
                this.Close();
                return;
            }

            if (ForSingle)
            {
                ImportSel imports = new ImportSel();
                imports.FileName = sfd.FileName;
                imports.ForSingle = true;
                imports.ShowDialog();
                if (imports.IsCancel)
                {
                    this.Close();
                    return;
                }
                else
                {
                    if (MsgBox.OKCancel("若页面 '"+ imports.SingleFileCap+ "' 存在将被替换，继续吗？").Equals(DialogResult.OK))
                    {
                        ImportPlusOverWrite = true;
                        ImportPlusFiles = imports.SelFiles;
                        timer2.Enabled = true;
                    }
                    else
                    {
                        this.Close();
                        return;
                    }
                }
            }
            else
            {
                //是否全部覆盖更新
                if (MsgBox.YesNo(res.SiteImport.GetString("m10")).Equals(DialogResult.Yes))
                {
                    if (MsgBox.OKCancel(res.SiteImport.GetString("m2")).Equals(DialogResult.Cancel))
                    {
                        this.Close();
                        return;
                    }
                    filename = sfd.FileName;
                    timer1.Enabled = true;
                }
                else
                {
                    ImportSel imports = new ImportSel();
                    imports.FileName = sfd.FileName;
                    imports.ShowDialog();
                    if (imports.IsCancel)
                    {
                        this.Close();
                        return;
                    }
                    else
                    {
                        ImportPlusOverWrite = !imports.checkBox1.Checked;
                        ImportPlusFiles = imports.SelFiles;
                        timer2.Enabled = true;
                    }
                }
            }
        }
        private bool ImportPlusOverWrite = false;
        private ArrayList ImportPlusFiles = null;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            string siteid = globalConst.CurSite.ID;
            SiteClass.Site.close();

            if (!SiteClass.Site.Import(siteid, Stat, filename))
                MsgBox.Error(res.SiteImport.GetString("m3"));
            else
            {
                MsgBox.Information(res.SiteImport.GetString("m4"));
                SiteClass.Site.open(siteid);
            }
            this.Close();

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            string siteid = globalConst.CurSite.ID;

            //SiteClass.Site.close(); 部分导入不需要关闭站点，已在导入时处理关闭相关页面

            string result=SiteClass.Site.ImportPlus(siteid, Stat,ImportPlusOverWrite,ImportPlusFiles);
            if (result!=null)
                MsgBox.Error(res.SiteImport.GetString("m3") + "\r\n" + result);
            else
            {
                MsgBox.Information(res.SiteImport.GetString("m4"));
                SiteClass.Site.InitSiteTree();
                //SiteClass.Site.open(siteid); 
            }
            

            this.Close();
        }
    }
}