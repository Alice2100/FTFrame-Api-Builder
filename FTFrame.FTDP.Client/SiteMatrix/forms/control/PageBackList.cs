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
    public partial class PageBackList : Form
    {
        public string PageID = null;
        public PageBackList()
        {
            InitializeComponent();
        }

        private void PageBackList_Load(object sender, EventArgs e)
        {
            this.Text = res.ctl.str("HisBack.text");			//历史备份 - 点击下载
            BackupList(PageID);
        }
        private  void BackupList(string pageid)
        {
            dgv1.Rows.Clear();
            string sql = "select * from sites where id='" + globalConst.CurSite.ID + "'";
            SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
            string _url;
            string _id = globalConst.CurSite.ID;
            string _key;
            string _user;
            string _passwd;
            if (rdr.Read())
            {
                _url = rdr.GetString(rdr.GetOrdinal("url"));
                _key = rdr.GetString(rdr.GetOrdinal("cdkey"));
                _user = rdr.GetString(rdr.GetOrdinal("username"));
                _passwd = rdr.GetString(rdr.GetOrdinal("passwd"));
            }
            else
            {
                log.Error("siteid is " + globalConst.CurSite.ID + " not found while check server!");
                return;
            }
            rdr.Close(); rdr = null;
            if (_url.EndsWith("/")) _url = _url.Substring(0, _url.Length - 1);
            string url = _url + "/_ftpub/clientop";
            string postStr = "_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd + "&type=BackupList&PageID=" + pageid;
            try
            {
                string reStr = functions.net.HttpPost(url, postStr).Trim();
                if (reStr.StartsWith("error:")) MsgBox.Error(reStr);
                else {
                    string[] item1 = reStr.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string _item1 in item1)
                    {
                        string[] item = _item1.Split('#');
                        var index=dgv1.Rows.Add(new string[] { item[1] + " - " + item[0] , res.ctl.str("HisBack.RollBack") , res.ctl.str("HisBack.Download") , res.ctl.str("HisBack.Export") });
                        dgv1.Rows[index].Tag= item[2];
                    }
                }
                if(dgv1.Rows.Count==0)
                {
                    MsgBox.Information(res.ctl.str("HisBack.1")); 
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void BackupRoolBack(string pageid, string filename)
        {
            string sql = "select * from sites where id='" + globalConst.CurSite.ID + "'";
            SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
            string _url;
            string _id = globalConst.CurSite.ID;
            string _key;
            string _user;
            string _passwd;
            if (rdr.Read())
            {
                _url = rdr.GetString(rdr.GetOrdinal("url"));
                _key = rdr.GetString(rdr.GetOrdinal("cdkey"));
                _user = rdr.GetString(rdr.GetOrdinal("username"));
                _passwd = rdr.GetString(rdr.GetOrdinal("passwd"));
            }
            else
            {
                log.Error("siteid is " + globalConst.CurSite.ID + " not found while check server!");
                return;
            }
            rdr.Close(); rdr = null;
            if (_url.EndsWith("/")) _url = _url.Substring(0, _url.Length - 1);
            string url = _url + "/_ftpub/clientop";
            string postStr = "_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd + "&type=BackupGet&PageID=" + pageid + "&FileName=" + filename;
            try
            {
                string dict = globalConst.AppPath + "\\down";
                if (Directory.Exists(dict)) Directory.Delete(dict, true);
                Directory.CreateDirectory(dict);
                var downfile = dict + @"\page.site";
                if (File.Exists(downfile))File.Delete(downfile);
                net.HttpPostDownload(url, postStr, downfile);
                if (dir.Exists(globalConst.AppPath + @"\temp"))
                {
                    dir.Delete(globalConst.AppPath + @"\temp", true);
                }
                dir.CreateDirectory(globalConst.AppPath + @"\temp");

                new Compression.ZipClass().UnZip(dict + "\\page.site", globalConst.AppPath + @"\temp\site\");

                Application.DoEvents();

                ArrayList al = new ArrayList();
                string fullpath = SiteClass.Site.PageFullPath(pageid, globalConst.AppPath + @"\temp\site\site.db");
                string dirPath = "";
                foreach (string[] item in SiteClass.Site.PageFullPathDic)
                {
                    al.Add(new string[] { item[0], "\\" + item[1] + dirPath });
                    dirPath = "\\" + item[1] + dirPath;
                }
                //MsgBox.Information(fullpath);
                al.Add(new string[] { pageid, fullpath });
                string restr = SiteClass.Site.ImportPlus(globalConst.CurSite.ID, null, true, al);
                if (restr == null)
                {
                    MsgBox.Information(res.ctl.str("HisBack.success"));
                }
                else MsgBox.Error(restr);
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void BackupGet(string pageid,string filename)
        {
            string sql = "select * from sites where id='" + globalConst.CurSite.ID + "'";
            SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
            string _url;
            string _id = globalConst.CurSite.ID;
            string _key;
            string _user;
            string _passwd;
            if (rdr.Read())
            {
                _url = rdr.GetString(rdr.GetOrdinal("url"));
                _key = rdr.GetString(rdr.GetOrdinal("cdkey"));
                _user = rdr.GetString(rdr.GetOrdinal("username"));
                _passwd = rdr.GetString(rdr.GetOrdinal("passwd"));
            }
            else
            {
                log.Error("siteid is " + globalConst.CurSite.ID + " not found while check server!");
                return;
            }
            rdr.Close(); rdr = null;
            if (_url.EndsWith("/")) _url = _url.Substring(0, _url.Length - 1);
            string url = _url + "/_ftpub/clientop";
            string postStr = "_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd + "&type=BackupGet&PageID=" + pageid+ "&FileName=" + filename;
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = res.ctl.str("HisBack.2")+"(*.site)|*.site";
                sfd.ShowDialog();
                if(sfd.FileName!=null && sfd.FileName!="")
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
        private void BackupGetPublish(string pageid, string filename)
        {
            string sql = "select * from sites where id='" + globalConst.CurSite.ID + "'";
            SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
            string _url;
            string _id = globalConst.CurSite.ID;
            string _key;
            string _user;
            string _passwd;
            if (rdr.Read())
            {
                _url = rdr.GetString(rdr.GetOrdinal("url"));
                _key = rdr.GetString(rdr.GetOrdinal("cdkey"));
                _user = rdr.GetString(rdr.GetOrdinal("username"));
                _passwd = rdr.GetString(rdr.GetOrdinal("passwd"));
            }
            else
            {
                log.Error("siteid is " + globalConst.CurSite.ID + " not found while check server!");
                return;
            }
            rdr.Close(); rdr = null;
            if (_url.EndsWith("/")) _url = _url.Substring(0, _url.Length - 1);
            string url = _url + "/_ftpub/clientop";
            string postStr = "_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd + "&type=BackupGetPublish&PageID=" + pageid + "&FileName=" + filename;
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                var _filename = string.Join("_", filename.Split('_').Skip(2)).Replace(".site", ".cshtml");
                sfd.FileName = _filename;
                sfd.Filter = "published file(*.*)|*.*";
                sfd.ShowDialog();
                if (sfd.FileName != null && sfd.FileName != "" && sfd.FileName != _filename)
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
            var FileName = dgv1.Rows[e.RowIndex].Tag.ToString();
            if (e.ColumnIndex == 0)
            {
                Clipboard.SetText(FileName);
                MsgBox.Information("FileName Copyed");
            }
            else if(e.ColumnIndex==1)
            {
                if(MsgBox.YesNoCancel(res.ctl.str("HisBack.IsRoll"))==DialogResult.Yes)
                {
                    BackupRoolBack(PageID, FileName);
                }
            }
            else if (e.ColumnIndex == 2)
            {
                BackupGet(PageID, FileName);
            }
            else if (e.ColumnIndex == 3)
            {
                BackupGetPublish(PageID, FileName);
            }
        }
    }

}
