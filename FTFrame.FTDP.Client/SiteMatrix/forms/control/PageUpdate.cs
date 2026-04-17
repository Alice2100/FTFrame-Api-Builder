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
using System.IO;
using System.Collections;
using Microsoft.Data.Sqlite;

namespace FTDPClient.forms.control
{
    public partial class PageUpdate : Form
    {
        public string ReflectString = null;
        public PageUpdate()
        {
            InitializeComponent();
        }
        string _lan_wu;
        string _lan_confirm;
        string _lan_success;
        private void PageBackList_Load(object sender, EventArgs e)
        {
            #region Language
            this.Text = res.ctl.str("PageUpdate.text");			//版本更新
            button1.Text = res.com.GetString("pageupdate.button1"); //更新
            button2.Text = res.com.GetString("pageupdate.button2"); //全选
            button4.Text = res.com.GetString("pageupdate.button4"); //选中需更新
            button3.Text = res.com.GetString("pageupdate_button1"); //刷新
            dataGridView1.Columns[1].HeaderText = res.com.GetString("pageupdate_col1"); //文件名称
            dataGridView1.Columns[2].HeaderText = res.com.GetString("pageupdate_col2"); //版本最新
            dataGridView1.Columns[3].HeaderText = res.com.GetString("pageupdate_col3"); //本地更新
            _lan_wu = res.com.GetString("pageupdate_wu");    //（无）
            _lan_confirm = res.com.GetString("pageupdate_confirm");  //更新后会替换选中的本地页面，是否继续？
            _lan_success = res.com.GetString("pageupdate_success");	//更新成功
            #endregion
            GetList();
            button4_Click(null, null);
        }

        string _url;
        string _id = globalConst.CurSite.ID;
        string _key;
        string _user;
        string _passwd;
        private  void GetList()
        {
            dataGridView1.Rows.Clear();
            string sql = "select * from sites where id='" + globalConst.CurSite.ID + "'";
            SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
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
            string postStr = "_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd + "&type=PageUpdateList";
            try
            {
                string reStr = functions.net.HttpPost(url, postStr).Trim();
                if (reStr.StartsWith("error:")) MsgBox.Error(reStr);
                else 
                {
                    string[] item1 = reStr.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string _item1 in item1)
                    {
                        string[] item = _item1.Split('#');
                        string pageId = item[0];
                        string name = item[1];
                        string serverTime = item[2];
                        string filename = item[3];
                        string localTime = _lan_wu;
                        System.Windows.Forms.TreeNode PageNode = tree.getTreeNodeByID(pageId, globalConst.MdiForm.SiteTree);
                        if (PageNode != null)
                        {
                            string localFile = globalConst.CurSite.Path + tree.getPath(PageNode);
                            if(file.Exists(localFile))
                            {
                                localTime = new FileInfo(localFile).LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                        int index=dataGridView1.Rows.Add(new object[] { false, name , serverTime , localTime });
                        dataGridView1.Rows[index].Tag = new string[] { pageId , filename };
                        if(localTime!= _lan_wu && DateTime.Parse(localTime) > DateTime.Parse(serverTime))
                        {
                            dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.Yellow;
                        }
                        else if (localTime == _lan_wu || DateTime.Parse(localTime) < DateTime.Parse(serverTime))
                        {
                            dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.LightBlue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void DllList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)//复选框
            {
                if ((bool)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
            else if (e.ColumnIndex == 1)
            {
                string pageId = ((string[])dataGridView1.Rows[e.RowIndex].Tag)[0];
                Clipboard.SetText(pageId);
                MsgBox.Information("page id copyed\r\n"+ pageId);
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[0].Value = true;
                row.DefaultCellStyle.BackColor = Color.LightBlue;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MsgBox.YesNoCancel(_lan_confirm) != DialogResult.Yes) return;
            button1.Enabled = false;
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if ((bool)row.Cells[0].EditedFormattedValue)
                    {
                        string[] item = (string[])row.Tag;
                        string restr=UpdateFromServer(item[0], item[1], DateTime.Parse(row.Cells[2].Value.ToString()));
                        row.Cells[3].Value = restr;
                        Application.DoEvents();
                    }
                }
                SiteClass.Site.InitSiteTree();
                if(Directory.Exists(globalConst.AppPath + "\\down"))Directory.Delete(globalConst.AppPath + "\\down",true);
                GetList();
            }
            catch(Exception ex)
            {
                new error(ex);
            }
            button1.Enabled = true;
        }
        private string UpdateFromServer(string pageid, string filename,DateTime serverTime)
        {
            string url = _url + "/_ftpub/clientop";
            string postStr = "_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd + "&type=BackupGet&PageID=" + pageid + "&FileName=" + filename;
            try
            {
                string dic = globalConst.AppPath + "\\down";
                Directory.CreateDirectory(dic);
                net.HttpPostDownload(url, postStr, dic+"\\"+filename);

                if (dir.Exists(globalConst.AppPath + @"\temp"))
                {
                    dir.Delete(globalConst.AppPath + @"\temp", true);
                }
                dir.CreateDirectory(globalConst.AppPath + @"\temp");

                new Compression.ZipClass().UnZip(dic + "\\" + filename, globalConst.AppPath + @"\temp\site\");

                Application.DoEvents();

                ArrayList al = new ArrayList();
                string fullpath = SiteClass.Site.PageFullPath(pageid, globalConst.AppPath + @"\temp\site\site.db");
                string dirPath = "";
                foreach(string[] item in SiteClass.Site.PageFullPathDic)
                {
                    al.Add(new string[] { item[0], "\\"+item[1] + dirPath });
                    dirPath = "\\" + item[1] + dirPath;
                }
                //MsgBox.Information(fullpath);
                al.Add(new string[] { pageid, fullpath });
                string restr=SiteClass.Site.ImportPlus(globalConst.CurSite.ID, null, true, al);
                if (restr == null)
                {
                    string newfilename = globalConst.SitesPath + @"\" + globalConst.CurSite.ID + fullpath;
                    if (file.Exists(newfilename)) new FileInfo(newfilename).LastWriteTime = serverTime;
                    return _lan_success;
                }
                else return restr;
            }
            catch (Exception ex)
            {
                new error(ex);
                return ex.Message;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string localTime = row.Cells[3].Value.ToString();
                string serverTime = row.Cells[2].Value.ToString();
                if (localTime == _lan_wu || DateTime.Parse(localTime) < DateTime.Parse(serverTime))
                {
                    row.Cells[0].Value = true;
                    //row.DefaultCellStyle.BackColor = Color.LightBlue;
                }
                else
                {
                    row.Cells[0].Value = false;
                    //row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }
    }

}
