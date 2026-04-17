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

namespace FTDPClient.forms.control
{
    public partial class DllList : Form
    {
        public string ReflectString = null;
        public DllList()
        {
            InitializeComponent();
        }

        private void PageBackList_Load(object sender, EventArgs e)
        {
            this.Text = res.ctl.str("DllList.text");			//子选择dll文件
            BackupList();
        }
        private  void BackupList()
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
            string postStr = "_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd + "&type=DllList";
            try
            {
                string reStr = functions.net.HttpPost(url, postStr).Trim();
                if (reStr.StartsWith("error:")) MsgBox.Error(reStr);
                else {
                    string[] item1 = reStr.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string _item1 in item1)
                    {
                        listBox1.Items.Add(
                        new Obj.ComboItem() { Name = _item1, Tag = _item1 }
                        );
                    }
                }
                if(listBox1.Items.Count==0)
                {
                    MsgBox.Information(res.ctl.str("DllList.1"));
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void BackupGet(string filename)
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
            string postStr = "_id=" + _id + "&_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd + "&type=DllReflect&FileName=" + filename; 
            try
            {
                ReflectString = functions.net.HttpPost(url, postStr).Trim();
                this.Close();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            Obj.ComboItem obj =(Obj.ComboItem) listBox1.SelectedItem;
            BackupGet( obj.Tag.ToString());
        }

        private void DllList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
    }

}
