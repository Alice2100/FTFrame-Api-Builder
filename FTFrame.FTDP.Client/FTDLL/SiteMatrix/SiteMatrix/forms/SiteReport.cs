using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SiteMatrix.consts;
using SiteMatrix.database;
using SiteMatrix.functions;
namespace SiteMatrix.forms
{
    public partial class SiteReport : Form
    {
        public SiteReport()
        {
            InitializeComponent();
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            this.Text = res.SiteReport.GetString("_this");
            this.button1.Text = res.SiteReport.GetString("button1");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SiteReport_Load(object sender, EventArgs e)
        {
            try
            {
                string s = "";
                string n = "      ";
                string ln = "\r\n";
                string sql = "";
                string sql2 = "";
                string r = "";
                s += res.SiteReport.GetString("c1") + ln;
                s += n + globalConst.CurSite.ID + ln;
                sql = "select count(*) as count_all from directory";
                r = globalConst.CurSite.SiteConn.GetInt32(sql).ToString();
                s += res.SiteReport.GetString("c2") + ln;
                s += n + r + ln;
                sql = "select count(*) as count_all from pages";
                r = globalConst.CurSite.SiteConn.GetInt32(sql).ToString();
                s += res.SiteReport.GetString("c3") + ln;
                s += n + r + ln;
                sql = "select count(*) as count_all from (SELECT distinct name from controls)";
                r = globalConst.CurSite.SiteConn.GetInt32(sql).ToString();
                s += res.SiteReport.GetString("c4") + ln;
                s += n + r + ln;
                s += res.SiteReport.GetString("c5") + ln;
                sql = "SELECT distinct name from controls";
                DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql));
                int NotInSystem = 0;
                string rr="";
                while(dr.Read())
                {
                    r = dr.getString(0);
                    s += n + r + ln;
                    sql2 = "select count(*) from controls where name='" + r + "'";
                    if(globalConst.ConfigConn.GetInt32(sql2)==0)
                    {
                        NotInSystem++;
                        rr += n + r + ln;
                    }
                }
                dr.Close();
                s += res.SiteReport.GetString("c6") + ln;
                s += n + NotInSystem + ln;
                if (NotInSystem>0)
                {
                    s += res.SiteReport.GetString("c7") + ln;
                    s += n + rr;
                }
                s += res.SiteReport.GetString("c8") + ln;
                sql = "select count(*) as count_all from controls";
                r = globalConst.CurSite.SiteConn.GetInt32(sql).ToString();
                s += n + r + ln;
                s += res.SiteReport.GetString("c9") + ln;
                sql = "select * from controls order by name";
                dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql));
                while(dr.Read())
                {
                    r = "[" + dr.getString("name") + "][" + dr.getString("caption") + "][" + GetShareCaption(dr.getString("shared")) + "]";
                    if (dr.getString("datasource").Length>4)
                    {
                        r += "*";
                    }
                    s += n + r + ln;
                }
                dr.Close();
                s += res.SiteReport.GetString("c10") + ln;
                sql = "select count(*) as count_all from parts";
                r = globalConst.CurSite.SiteConn.GetInt32(sql).ToString();
                s += n + r + ln;
                s += res.SiteReport.GetString("c11") + ln;
                sql = "select count(*) as count_all from parts where asportal=1";
                r = globalConst.CurSite.SiteConn.GetInt32(sql).ToString();
                s += n + r + ln;
                s += res.SiteReport.GetString("c12") + ln;
                sql = "select count(*) as count_all from part_in_page";
                r = globalConst.CurSite.SiteConn.GetInt32(sql).ToString();
                s += n + r + ln;
                s += res.SiteReport.GetString("c13") + ln;
                sql = "select * from share_data order by site,name";
                dr=new DR(globalConst.CurSite.SiteConn.OpenRecord(sql));
                while(dr.Read())
                {
                    r = "[" + dr.getString("site") + "][" + dr.getString("name") + "][" + dr.getString("caption") + "][" + GetShareCaption(dr.getString("shared")) + "]";
                    s += n + r + ln;
                }
                dr.Close();
                ReportText.Text = s;
                ReportText.SelectionStart = 0;
                ReportText.SelectionLength = 0;
            }
            catch(Exception ex)
            {
                new error(ex);
            }
        }
        private string GetShareCaption(string i)
        {
            switch(i)
            {
                case "0": return res.SiteReport.GetString("e1");
                case "1": return res.SiteReport.GetString("e2");
                case "2": return res.SiteReport.GetString("e3");
            }
            return "";
        }
    }
}