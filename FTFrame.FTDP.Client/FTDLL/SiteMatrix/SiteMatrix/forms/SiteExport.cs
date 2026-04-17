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
namespace SiteMatrix.forms
{
    public partial class SiteExport : Form
    {
        public SiteExport()
        {
            InitializeComponent();
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            this.Text = res.SiteExport.GetString("_this");
        }
        public bool AsTemplate = false;
        private string filename;
        private void SiteImport_Load(object sender, EventArgs e)
        {
            if (AsTemplate) this.Text = "Export As Template";
            else
                this.Text = "Export Site";
            SaveFileDialog sfd = new SaveFileDialog();

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
            if (file.Exists(sfd.FileName))
            {
                if (MsgBox.OKCancel(sfd.FileName + res.SiteExport.GetString("m1")).Equals(DialogResult.Cancel))
                {
                    this.Close();
                    return;
                }
            }
            filename = sfd.FileName;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            if (!SiteClass.Site.Export(Stat, filename, AsTemplate))
                MsgBox.Error(res.SiteExport.GetString("m2"));
            else
                MsgBox.Information(res.SiteExport.GetString("m3"));
            this.Close();
        }
    }
}