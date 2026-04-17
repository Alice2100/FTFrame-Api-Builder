using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SiteMatrix.Style;
using SiteMatrix.functions;
using SiteMatrix.consts;
using SiteMatrix.database;
using SiteMatrix.forms;
using SiteMatrix.SiteClass;
using mshtml;
using htmleditocx;
using SiteMatrix.Page;
using SiteMatrix.Common;
using System.Collections;
namespace SiteMatrix.forms
{
    public partial class BugResult : Form
    {
        public bool IsAllSite = false;
        public ArrayList SelFiles = null;
        public string SinglePageID = null;
        public BugResult()
        {
            InitializeComponent();
            this.comboBox1.Items.AddRange(FTDP.Util.BugCheck.BugTypes);
        }

        private void BugResult_Load(object sender, EventArgs e)
        {

        }

        private void BugResult_Shown(object sender, EventArgs e)
        {
            //do
            //ArrayList result = Bug.Init(IsAllSite,SelFiles,SinglePageID);
            ArrayList result = FTDP.Util.BugCheck.Init(IsAllSite, SelFiles, SinglePageID, "Provider=" + globalConst.OLEDBProvider + ";Data Source=" + globalConst.ConfigFile, "Provider=" + globalConst.OLEDBProvider + ";Data Source=" + globalConst.AppPath + @"\cfg\site_" + globalConst.CurSite.ID + ".db", Options.GetSystemValue("mysql"), globalConst.AppPath, globalConst.CurSite.ID);
            label2.Text = "";
            foreach (string[] item in result)
            {
                int a=dataGridView1.Rows.Add(item);
                DataGridViewRow row = dataGridView1.Rows[a];
                row.Tag = item[6];
                if (item[4].Equals("严重"))
                {
                    row.DefaultCellStyle.BackColor = Color.DarkRed;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else if (item[4].Equals("重要"))
                {
                    row.DefaultCellStyle.BackColor = Color.Chocolate;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else if (item[4].Equals("一般"))
                {
                    row.DefaultCellStyle.BackColor = Color.LightSalmon;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (item[4].Equals("警告"))
                {
                    row.DefaultCellStyle.BackColor = Color.PeachPuff;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (item[4].Equals("可略"))
                {
                    row.DefaultCellStyle.BackColor = Color.MintCream;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
            result.Clear();
            result = null;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (e.ColumnIndex == 5)
                {
                    string id = row.Tag.ToString();
                    if (form.doActive(id))
                    {
                        return;
                    }
                    string path = globalConst.CurSite.Path + "\\" + row.Cells[1].Value.ToString().Replace("/", "\\");

                    if (!file.Exists(path + "_edit.htm"))
                    {
                        SiteClass.Site.constructEditPageFromText(id, path);
                    }
                    else
                    {
                        //如果edit文件被手动修改过，重新生成
                        System.IO.FileInfo fioedit = new System.IO.FileInfo(path + "_edit.htm");
                        System.IO.FileInfo fio = new System.IO.FileInfo(path);
                        if (!(fioedit.LastWriteTime.Equals(fio.LastWriteTime)))
                        {
                            SiteClass.Site.constructEditPageFromText(id, path);
                        }
                    }
                    string sql = "select name from pages where id='" + str.Dot2DotDot(id) + "'";
                    string name = globalConst.CurSite.SiteConn.GetString(sql);
                    sql = "select ptype from pages where id='" + str.Dot2DotDot(id) + "'";
                    int ptype = globalConst.CurSite.SiteConn.GetInt32(sql);
                    form.addEditor(path, row.Cells[2].Value.ToString(), id, name, ptype);
                }
            }
            catch { }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = comboBox1.Text;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    if (val.Equals("全部") || val.Equals(""))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        string type = row.Cells[0].Value == null ? "" : row.Cells[0].Value.ToString();
                        row.Visible = type.Equals(val);
                    }
                }
                catch { }
            }
        }
    }
}
