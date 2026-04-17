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
    public partial class Snippet : Form
    {
        public bool IsEdit = false;
        public string ID = "";
        public Snippet()
        {
            InitializeComponent();
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            this.Text=res.Snippet.GetString("_this");
            Caption.Text = res.Snippet.GetString("Caption");
            Content.Text = res.Snippet.GetString("Content");
            OK.Text = res.Snippet.GetString("OK");
            Cancel.Text = res.Snippet.GetString("Cancel");
        }
        private void Snippet_Load(object sender, EventArgs e)
        {
            if(IsEdit)
            {
                string sql = "select * from snippets where id=" + ID;
                DR dr=new DR(globalConst.ConfigConn.OpenRecord(sql));
                if(dr.Read())
                {
                    this.CaptionText.Text = dr.getString("caption");
                    this.ContentText.Text = dr.getString("content");
                }
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if(CaptionText.Text.Trim().Equals(""))
            {
                MsgBox.Warning(res.Snippet.GetString("m1"));
                return;
            }
            if (ContentText.Text.Trim().Equals(""))
            {
                MsgBox.Warning(res.Snippet.GetString("m2"));
                return;
            }
            string sql = "";
            if(IsEdit)
            {
                sql = "update snippets set caption='" + CaptionText.Text.Trim().Replace("'", "''") + "',content='" + ContentText.Text.Trim().Replace("'", "''") + "' where id=" + ID;
            }
            else
            {
                sql = "insert into snippets(caption,content)values('" + CaptionText.Text.Trim().Replace("'", "''") + "','" + ContentText.Text.Trim().Replace("'", "''") + "')";
            }
            globalConst.ConfigConn.execSql(sql);
            globalConst.MdiForm.InitToolBoxSnippets();
            this.Close();
        }
    }
}