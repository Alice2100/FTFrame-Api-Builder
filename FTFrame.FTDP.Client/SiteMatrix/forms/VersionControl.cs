using FTDPClient.functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using FTDPClient.consts;
using FTDPClient.functions;
using System.Web.UI.WebControls;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Collections;

namespace FTDPClient.forms.control
{
    public partial class VersionControl : Form
    {
        public string PageID = null;
        public VersionControl()
        {
            InitializeComponent();
        }

        private void PageBackList_Load(object sender, EventArgs e)
        {
            this.Text = res.ctl.str("VersionControl.Text");
            this.label1.Text = res.ctl.str("VersionControl.Desc");
            this.label2.Text = res.ctl.str("VersionControl.version");
            button1.Text = res.ctl.str("VersionControl.btn1");
            button2.Text = res.ctl.str("VersionControl.btn2");
            VersionList();
        }
        private void VersionList()
        {
            dgv1.Rows.Clear();
            var cop = Adv.ClientOperationPost();
            string url = cop.url;
            string postStr = cop.postStr + "&type=VersionList";
            try
            {
                string reStr = functions.net.HttpPost(url, postStr).Trim();
                if (reStr.StartsWith("error:")) MsgBox.Error(reStr);
                else
                {
                    string[] item1 = reStr.Split(new string[] { "[##]" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string _item1 in item1)
                    {
                        string[] item = _item1.Split(new string[] { "(##)" }, StringSplitOptions.None);
                        var index = dgv1.Rows.Add(new string[] { item[0], item[1], item[2], res.ctl.str("VersionControl.Detail"), res.ctl.str("HisBack.RollBack"), res.ctl.str("HisBack.Download"), res.ctl.str("HisBack.Export") });
                        dgv1.Rows[index].Tag = item[3];
                    }
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void VersionAdd(string time)
        {
            var cop = Adv.ClientOperationPost();
            string url = cop.url;
            string postStr = cop.postStr + "&type=VersionAdd&Version=" + textBox2.Text.Trim() + "&Time=" + time + "&Desc=" + textBox1.Text.Trim();
            try
            {
                string reStr = functions.net.HttpPost(url, postStr).Trim();
                if (reStr.StartsWith("error:")) MsgBox.Error(reStr);
                else
                {
                    MsgBox.Information($"Version {textBox2.Text.Trim()} Added");
                    VersionList();
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void VersionRollBack(string version)
        {
            var cop = Adv.ClientOperationPost();
            string url = cop.url;
            string postStr = cop.postStr + "&type=VersionGet&GetType=site&Version=" + version; 
            try
            {
                this.Text = res.ctl.str("VersionControl.Text") + " Start Download";
                Application.DoEvents();

                string dict = globalConst.AppPath + "\\down";
                if (Directory.Exists(dict)) Directory.Delete(dict, true);
                Directory.CreateDirectory(dict);
                var downfile = dict + @"\site.site";
                if (File.Exists(downfile)) File.Delete(downfile);
                net.HttpPostDownload(url, postStr, downfile);
                if (dir.Exists(globalConst.AppPath + @"\temp"))
                {
                    dir.Delete(globalConst.AppPath + @"\temp", true);
                }
                dir.CreateDirectory(globalConst.AppPath + @"\temp");

                this.Text = res.ctl.str("VersionControl.Text") + " UnZip File";
                Application.DoEvents();

                new Compression.ZipClass().UnZip(dict + "\\site.site", globalConst.AppPath + @"\temp\site\");

                Application.DoEvents();
                var sucI = 0;
                var files = new DirectoryInfo(globalConst.AppPath + @"\temp\site").GetFiles();
                for(int i=0;i<files.Length;i++)
                {
                    var file = files[i];
                    var zippath = globalConst.AppPath + @"\temp\site\" + (i+"_"+file.Name) + @"\";
                    Directory.CreateDirectory(zippath);
                    new Compression.ZipClass().UnZip(file.FullName, zippath);
                    string pageid = SiteClass.Site.GetPageIdFromDb(zippath+ "site.db");
                    if (pageid == null) continue;
                    ArrayList al = new ArrayList();
                    string fullpath = SiteClass.Site.PageFullPath(pageid, zippath + "site.db");
                    string dirPath = "";
                    foreach (string[] item in SiteClass.Site.PageFullPathDic)
                    {
                        al.Add(new string[] { item[0], "\\" + item[1] + dirPath });
                        dirPath = "\\" + item[1] + dirPath;
                    }
                    al.Add(new string[] { pageid, fullpath });

                    this.Text = res.ctl.str("VersionControl.Text") + " Start Import Page "+ file.Name;
                    Application.DoEvents();

                    string restr = SiteClass.Site.ImportPlus(globalConst.CurSite.ID, null, true, al, zippath);
                    if (restr == null)
                    {
                        sucI++;
                    }
                    else
                    {
                        MsgBox.Error(restr);
                        break;
                    }
                }
                this.Text = res.ctl.str("VersionControl.Text");
                MsgBox.Information(res.ctl.str("HisBack.success")+",Updated "+ sucI + " Pages");
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void Download(string version)
        {
            var cop = Adv.ClientOperationPost();
            string url = cop.url;
            string postStr = cop.postStr + "&type=VersionGet&GetType=site&Version=" + version;
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Download as Zip File(*.zip)|*.zip";
                sfd.ShowDialog();
                if (sfd.FileName != null && sfd.FileName != "")
                {
                    net.HttpPostDownload(url, postStr, sfd.FileName);
                    MsgBox.Information(res.ctl.str("HisBack.3"));
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void Export(string version)
        {
            var cop = Adv.ClientOperationPost();
            string url = cop.url;
            string postStr = cop.postStr + "&type=VersionGet&GetType=page&Version=" + version;
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Export as Zip File(*.zip)|*.zip";
                sfd.ShowDialog();
                if (sfd.FileName != null && sfd.FileName != "")
                {
                    net.HttpPostDownload(url, postStr, sfd.FileName);
                    MsgBox.Information(res.ctl.str("HisBack.3"));
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void PageBackList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string version = dgv1.Rows[e.RowIndex].Cells[0].Value.ToString();
            if (e.ColumnIndex == 3)
            {
                HTMLText hTMLText = new HTMLText();
                hTMLText.Text = "Detail";
                hTMLText.SetVal = dgv1.Rows[e.RowIndex].Tag.ToString();
                hTMLText.ShowDialog();
            }
            else if (e.ColumnIndex == 4)
            {
                if (MsgBox.YesNoCancel(res.ctl.str("HisBack.IsRollSite")) == DialogResult.Yes)
                {
                    VersionRollBack(version);
                }
            }
            else if (e.ColumnIndex == 5)
            {
                Download(version);
            }
            else if (e.ColumnIndex == 6)
            {
                Export(version);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            VersionAdd("");
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            VersionAdd(dateTimePicker1.Text);
            button2.Enabled = true;
        }
    }

}
