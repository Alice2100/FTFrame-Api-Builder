using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SiteMatrix.consts;
using SiteMatrix.functions;
namespace SiteMatrix.forms
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            this.Text = res.Option.GetString("_this");
            button2.Text = res.Option.GetString("button2");
            button1.Text = res.Option.GetString("button1");
            tabPage1.Text = res.Option.GetString("tabPage1");
            tabPage2.Text = res.Option.GetString("tabPage2");
            tabPage3.Text = res.Option.GetString("tabPage3");
            toolboxvisible.Text = res.Option.GetString("toolboxvisible");
            workspacevisible.Text = res.Option.GetString("workspacevisible");
            propertyvisible.Text = res.Option.GetString("propertyvisible");
            tooltextvisible.Text = res.Option.GetString("tooltextvisible");
            toolsitevisible.Text = res.Option.GetString("toolsitevisible");
            sceditmargin.Text = res.Option.GetString("sceditmargin");
            sceditwraped.Text = res.Option.GetString("sceditwraped");
            label1.Text = res.Option.GetString("label1");
            label2.Text = res.Option.GetString("label2");
            label3.Text = res.Option.GetString("label3");
            portaldefault.Text = res.Option.GetString("portaldefault");
            autogetshare.Text = res.Option.GetString("autogetshare");
            label4.Text = "Data Base Connection String";//res.Option.GetString("mysql");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SiteReport_Load(object sender, EventArgs e)
        {
            sceditfont.Items.Clear();
            for (int i = 1; i <= 18; i++)
            {
                sceditfont.Items.Add("Font " + i);
            }
            toolboxvisible.Checked = (GetSystemValue("toolboxvisible").Equals("1"));
            workspacevisible.Checked = (GetSystemValue("workspacevisible").Equals("1"));
            propertyvisible.Checked = (GetSystemValue("propertyvisible").Equals("1"));
            tooltextvisible.Checked = (GetSystemValue("tooltextvisible").Equals("1"));
            toolsitevisible.Checked = (GetSystemValue("toolsitevisible").Equals("1"));
            sceditmargin.Checked = (GetSystemValue("sceditmargin").Equals("1"));
            sceditwraped.Checked = (GetSystemValue("sceditwraped").Equals("1"));
            portaldefault.Checked = (GetSystemValue("portaldefault").Equals("1"));
            autogetshare.Checked = (GetSystemValue("autogetshare").Equals("1"));
            sceditbgcolor.BackColor = ColorTranslator.FromHtml(GetSystemValue("sceditbgcolor"));
            sceditfont.SelectedIndex = int.Parse(GetSystemValue("sceditfont"));
            sceditsize.Text = GetSystemValue("sceditsize");
            vscodepath.Text= GetSystemValue("vscodepath");
            //mysql.Text = GetSystemValue("mysql");
            //dbtype.Text = GetSystemValue("dbtype");
            dgv.Rows.Clear();
            string dbsetstr = GetSystemValue("mysql");
            if (dbsetstr != null)
            {
                string[] item1 = dbsetstr.Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string _item in item1)
                {
                    string[] item = _item.Split(new string[] { "|||" },StringSplitOptions.None);
                    dgv.Rows.Add(new string[]{
                    item[0],
                   item[1],
                   item[2],"删除"
                    });
                }
            }

            dgv2.Rows.Clear();
            string dllfilestr = GetSystemValue("dllfile");
            if (dllfilestr != null)
            {
                string[] item1 = dllfilestr.Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string _item in item1)
                {
                    string[] item = _item.Split(new string[] { "|||" }, StringSplitOptions.None);
                    dgv2.Rows.Add(new string[]{
                    item[0],
                   item[1],
                   "删除",
                   "上传",
                   item[2]
                    });
                }
            }
        }
        public static string GetSystemValue(string name)
        {
            return globalConst.ConfigConn.GetString("select thevalue from system where name='" + name + "'");
        }
        public static string GetSystemDBSetType()
        {
            if (globalConst.CurSite.ID == null) return null;
            string dbsetstr = GetSystemValue("mysql");
            if (dbsetstr != null)
            {
                string[] item1 = dbsetstr.Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string _item in item1)
                {
                    string[] item = _item.Split(new string[] { "|||" }, StringSplitOptions.None);
                    if (item[0] == globalConst.CurSite.ID) return item[1].ToLower();
                }
            }
            return null;
        }
        public static string GetSystemDBSetConnStr()
        {
            if (globalConst.CurSite.ID == null) return null;
            string dbsetstr = GetSystemValue("mysql");
            if (dbsetstr != null)
            {
                string[] item1 = dbsetstr.Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string _item in item1)
                {
                    string[] item = _item.Split(new string[] { "|||" }, StringSplitOptions.None);
                    if (item[0] == globalConst.CurSite.ID) return item[2];
                }
            }
            return null;
        }
        public static void SetSystemValue(string name,string value)
        {
            if(GetSystemValue(name)!=null)
            globalConst.ConfigConn.GetString("update system set thevalue='" + value + "' where name='" + name + "'");
            else
                globalConst.ConfigConn.GetString("insert into system(name,thevalue)values('" + name + "','" + value + "')");
        }
        private void sceditbgcolor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = sceditbgcolor.BackColor;
            cd.ShowDialog();
            if (cd.Color != null)
            {
                sceditbgcolor.BackColor = cd.Color;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetSystemValue("toolboxvisible", toolboxvisible.Checked?"1":"0");
            SetSystemValue("workspacevisible", workspacevisible.Checked ? "1" : "0");
            SetSystemValue("propertyvisible", propertyvisible.Checked ? "1" : "0");
            SetSystemValue("tooltextvisible", tooltextvisible.Checked ? "1" : "0");
            SetSystemValue("toolsitevisible", toolsitevisible.Checked ? "1" : "0");
            SetSystemValue("sceditmargin", sceditmargin.Checked ? "1" : "0");
            SetSystemValue("sceditwraped", sceditwraped.Checked ? "1" : "0");
            SetSystemValue("portaldefault", portaldefault.Checked ? "1" : "0");
            SetSystemValue("autogetshare", autogetshare.Checked ? "1" : "0");
            SetSystemValue("sceditbgcolor", ColorTranslator.ToHtml(sceditbgcolor.BackColor));
            SetSystemValue("sceditfont", sceditfont.SelectedIndex.ToString());
            SetSystemValue("sceditsize", sceditsize.Text);
            SetSystemValue("vscodepath", vscodepath.Text.Trim());
            //SetSystemValue("mysql", mysql.Text);
            //SetSystemValue("dbtype", dbtype.Text);

            dgv.EndEdit();
            string dbsetstr = "";
            foreach (DataGridViewRow row in dgv.Rows)
            {
                object siteid = row.Cells[0].Value;
                object dbtype = row.Cells[1].Value;
                object connstr = row.Cells[2].Value;
                if (siteid != null && dbtype != null && connstr != null)
                {
                    dbsetstr += "###" + siteid + "|||" + dbtype + "|||" + connstr;
                }
            }
            SetSystemValue("mysql", dbsetstr);

            dgv2.EndEdit();
            string dllfilestr = "";
            foreach (DataGridViewRow row in dgv2.Rows)
            {
                object siteid = row.Cells[0].Value;
                object dlltype = row.Cells[1].Value;
                object filestr = row.Cells[4].Value;
                if (siteid != null && filestr != null)
                {
                    dllfilestr += "###" + siteid + "|||" + (dlltype == null ? " " : dlltype.ToString()) + "|||" + filestr;
                }
            }
            SetSystemValue("dllfile", dllfilestr); 

            mdifromConst.tooltextvisible = tooltextvisible.Checked ? 1 : 0;
            mdifromConst.toolsitevisible = toolsitevisible.Checked ? 1 : 0;
            mdifromConst.toolboxvisible = toolboxvisible.Checked ? 1 : 0;
            mdifromConst.workspacevisible = workspacevisible.Checked ? 1 : 0;
            mdifromConst.propertyvisible = propertyvisible.Checked ? 1 : 0;
            globalConst.MdiForm.BaseVisibility();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = str.getEncode(textBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Text = str.getDecode(textBox1.Text);
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                dgv.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    dgv2.Rows.RemoveAt(e.RowIndex);
                }
                else if (e.ColumnIndex == 3)
                {
                    if (globalConst.CurSite.ID == null)
                    {
                        MsgBox.Error("必须打开站点");
                        return;
                    }
                    if (dgv2.Rows[e.RowIndex].Cells[4].Value == null || !System.IO.File.Exists(dgv2.Rows[e.RowIndex].Cells[4].Value.ToString().Trim()))
                    {
                        MsgBox.Error("文件不存在！");
                        return;
                    }
                    if (dgv2.Rows[e.RowIndex].Cells[0].Value == null || dgv2.Rows[e.RowIndex].Cells[0].Value.ToString()=="")
                    {
                        MsgBox.Error("请指定上传域名，例如 www.abc.com 或 x.abc.com:81");
                        return;
                    }
                    string sql = "select * from sites where id='" + globalConst.CurSite.ID + "'";
                    System.Data.OleDb.OleDbDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
                    string _key=null;
                    string _user = null;
                    string _passwd = null;
                    if (rdr.Read())
                    {
                        _key = rdr.GetString(rdr.GetOrdinal("cdkey"));
                        _user = rdr.GetString(rdr.GetOrdinal("username"));
                        _passwd = rdr.GetString(rdr.GetOrdinal("passwd"));
                    }
                    rdr.Close();

                    dgv2.Rows[e.RowIndex].Cells[3].Value = "正在上传...";
                    Application.DoEvents();

                    string uploadurl = "http://" + dgv2.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "/__sys/ftdllupload.aspx?_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd;
                    System.Net.WebClient wc = new System.Net.WebClient();
                    byte[] responseArray = wc.UploadFile(uploadurl, "POST", dgv2.Rows[e.RowIndex].Cells[4].Value.ToString().Trim());
                    if (!Encoding.UTF8.GetString(responseArray).Trim().Equals("{ftserver}ok"))
                    {
                        MsgBox.Error(Encoding.UTF8.GetString(responseArray).Replace("{ftserver}", ""));
                    }
                    else
                    {
                        MsgBox.Information("上传成功！");
                    }
                    dgv2.Rows[e.RowIndex].Cells[3].Value = "上传";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
            }
        }

        private void dgv2_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[2].Value = "删除";
            e.Row.Cells[3].Value = "上传";
        }

        private void dgv_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[3].Value = "删除";
        }

        private void dgv2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
                if (e.ColumnIndex == 4)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Title = "请选择要上传到bin目录下的文件";
                    ofd.Multiselect = false;
                    ofd.ShowDialog();
                    if (ofd.FileName != null)
                    {
                        dgv2.Rows[e.RowIndex].Cells[4].Value = ofd.FileName;
                    }
                }
        }
    }
}