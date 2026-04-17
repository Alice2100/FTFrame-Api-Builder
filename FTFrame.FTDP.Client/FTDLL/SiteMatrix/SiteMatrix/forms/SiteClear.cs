using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SiteMatrix.SiteClass;
using SiteMatrix.functions;
using SiteMatrix.consts;
namespace SiteMatrix.forms
{
    public partial class SiteClear : Form
    {
        public SiteClear()
        {
            InitializeComponent();
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            this.Text = res.SiteClear.GetString("_this");
        }
        private void SiteImport_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            int DFCount=0;
            if (dir.Exists(globalConst.CurSite.Path + "\\control.resource"))
            {
                foreach (DirectoryInfo di in new DirectoryInfo(globalConst.CurSite.Path + "\\control.resource").GetDirectories())
                {
                    foreach(FileInfo fi in di.GetFiles())
                    {
                        string filename = "/control.resource/" + di.Name + "/" + fi.Name;
                        string sql = "select count(*) as countall from parts where id='" + di.Name + "_part' and partxml like '%" + filename + "%'";
                        if(globalConst.CurSite.SiteConn.GetInt32(sql)==0)
                        {
                            fi.Delete();
                            Application.DoEvents();
                            Stat.Text = filename + " deleted.";
                            Application.DoEvents();
                            DFCount++;
                        }
                    }
                }
            }
            Stat.Text = "";
            MsgBox.Information(DFCount.ToString() + res.SiteClear.GetString("m1"));
            this.Close();
        }
    }
}