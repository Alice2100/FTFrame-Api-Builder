using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FTDPClient.consts;
using FTDPClient.functions;
using System.Xml;
using Microsoft.Data.Sqlite;
using System.IO;
using FTDPClient.database;

namespace FTDPClient.forms
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
            label9.Text = res.form.GetString("vscodepath");
            label11.Text = res.form.GetString("morenqianduan");
            label13.Text = res.form.GetString("bianjiqiys");
            label12.Text = res.form.GetString("morenshuchu");
            label10.Text = res.form.GetString("kaifaqianm");
            button5.Text = res.form.GetString("jiushujushj");
            button6.Text = res.form.GetString("zhandianlbhb");
            button7.Text = res.form.GetString("shujukushengji");
            tabPage4.Text = res.form.GetString("shujukulj");
            tabPage5.Text = res.form.GetString("waibuchengxu");
            dgv.Columns[0].HeaderText= res.form.GetString("zhandian");
            dgv.Columns[1].HeaderText = res.form.GetString("leixing");
            dgv.Columns[2].HeaderText = res.form.GetString("lianjiechuan");
            dgv2.Columns[0].HeaderText = res.form.GetString("yuming");
            dgv2.Columns[1].HeaderText = res.form.GetString("leixing");
            dgv2.Columns[4].HeaderText = res.form.GetString("wenjian");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public static string QianMing
        {
            get
            {
                return GetSystemValue("qianming") ?? "";
            }
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
            vscodepath.Text = GetSystemValue("vscodepath");
            qianming.Text = GetSystemValue("qianming");
            defaultSiteBox.Text = GetSystemValue("DefaultSite")??"";


            using (var rdr = globalConst.ConfigConn.OpenRecord("select id from sites order by id"))
            {
                while (rdr.Read())
                {
                    ((DataGridViewComboBoxColumn)dgv.Columns[0]).Items.Add(rdr.GetString(0));
                    ((DataGridViewComboBoxColumn)dgv.Columns[0]).Items.Add(rdr.GetString(0)+"_PLAT");
                }
            }
            //mysql.Text = GetSystemValue("mysql");
            //dbtype.Text = GetSystemValue("dbtype");
            dgv.Rows.Clear();
            string dbsetstr = GetSystemValue("mysql");
            if (dbsetstr != null)
            {
                string[] item1 = dbsetstr.Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string _item in item1)
                {
                    string[] item = _item.Split(new string[] { "|||" }, StringSplitOptions.None);
                    if (((DataGridViewComboBoxColumn)dgv.Columns[0]).Items.Contains(item[0]))
                    {
                        dgv.Rows.Add(new string[]{
                    item[0],
                   item[1],
                   item[2],res.com.str("Option.delete")
                    });
                    }
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
                   res.com.str("Option.delete"),
                   res.com.str("Option.upload"),
                   item[2]
                    });
                }
            }

            QianDuanCombo.Items.Clear();
            QianDuanCombo.Items.Add(ForeFrameType.JQueryUI.ToString());
            QianDuanCombo.Items.Add(ForeFrameType.LayUI.ToString());
            QianDuanCombo.SelectedIndex = 0;
            ShuChuCombo.Items.Clear();
            ShuChuCombo.Items.Add(PageOutType.Page.ToString());
            ShuChuCombo.Items.Add(PageOutType.Json.ToString());
            ShuChuCombo.SelectedIndex = 0;
            string formFrameType = GetSystemValue("ForeFrameType");
            string pageOutType = GetSystemValue("PageOutType");
            if (formFrameType != null)
            {
                QianDuanCombo.SelectedItem = formFrameType;
            }
            if (pageOutType != null)
            {
                ShuChuCombo.SelectedItem = pageOutType;
            }

            bianjiCSS.Items.Clear();
            bianjiCSS.Items.Add(res.com.str("Option.current"));
            System.IO.FileInfo[] cssFiles = new System.IO.DirectoryInfo(globalConst.AppPath + "\\style").GetFiles();
            foreach (System.IO.FileInfo file in cssFiles)
            {
                if (file.Name != "default.css")
                {
                    bianjiCSS.Items.Add(file.Name);
                }
            }
            bianjiCSS.SelectedIndex = 0;

            upgrade.Items.Clear();
            upgrade.Items.Add(new Obj.ComboItem() { Id="", Name="(请选择)" });
            upgrade.Items.Add(new Obj.ComboItem() { Id="1", Name="DataOP,DyValue增加配置:自定义数据库链接" });
            upgrade.SelectedIndex = 0;
        }
        public static string GetSystemValue(string name)
        {
            return globalConst.ConfigConn.GetString("select thevalue from system where name='" + name + "'");
        }
        public static globalConst.DBType GetSystemDBSetType(string siteID = null)
        {
            if (siteID == null) siteID = globalConst.CurSite.ID;
            if (siteID == null) return globalConst.DBType.UnKnown;
            string dbsetstr = GetSystemValue("mysql");
            if (dbsetstr != null)
            {
                string[] item1 = dbsetstr.Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string _item in item1)
                {
                    string[] item = _item.Split(new string[] { "|||" }, StringSplitOptions.None);
                    if (item[0] == siteID)
                    {
                        switch(item[1].ToLower())
                        {
                            case "mysql":return globalConst.DBType.MySql;
                            case "sqlserver":return globalConst.DBType.SqlServer;
                            case "sqlite":return globalConst.DBType.Sqlite;
                            default:return globalConst.DBType.UnKnown;
                        }
                    }
                }
            }
            return globalConst.DBType.UnKnown;
        }
        public static string GetSystemDBSetConnStr(string siteID = null)
        {
            if (siteID == null) siteID = globalConst.CurSite.ID;
            if (siteID == null) return null;
            string dbsetstr = GetSystemValue("mysql");
            if (dbsetstr != null)
            {
                string[] item1 = dbsetstr.Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string _item in item1)
                {
                    string[] item = _item.Split(new string[] { "|||" }, StringSplitOptions.None);
                    if (item[0] == siteID) return item[2];
                }
            }
            return null;
        }
        public static globalConst.DBType GetSystemDBSetType_Plat(string siteID = null)
        {
            if (siteID == null) siteID = globalConst.CurSite.ID;
            if (siteID == null) return globalConst.DBType.UnKnown;
            string dbsetstr = GetSystemValue("mysql");
            if (dbsetstr != null)
            {
                string[] item1 = dbsetstr.Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string _item in item1)
                {
                    string[] item = _item.Split(new string[] { "|||" }, StringSplitOptions.None);
                    if (item[0] == (siteID + "_PLAT"))
                    {
                        switch (item[1].ToLower())
                        {
                            case "mysql": return globalConst.DBType.MySql;
                            case "sqlserver": return globalConst.DBType.SqlServer;
                            case "sqlite": return globalConst.DBType.Sqlite;
                            default: return globalConst.DBType.UnKnown;
                        }
                    }
                }
            }
            return GetSystemDBSetType(siteID);
        }
        public static string GetSystemDBSetConnStr_Plat(string siteID = null)
        {
            if (siteID == null) siteID = globalConst.CurSite.ID;
            if (siteID == null) return null;
            string dbsetstr = GetSystemValue("mysql");
            if (dbsetstr != null)
            {
                string[] item1 = dbsetstr.Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string _item in item1)
                {
                    string[] item = _item.Split(new string[] { "|||" }, StringSplitOptions.None);
                    if (item[0] == (siteID + "_PLAT")) return item[2];
                }
            }
            return GetSystemDBSetConnStr(siteID);
        }
        public static void SetSystemValue(string name, string value)
        {
            if (GetSystemValue(name) != null)
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
            SetSystemValue("toolboxvisible", toolboxvisible.Checked ? "1" : "0");
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
            SetSystemValue("ForeFrameType", QianDuanCombo.Text);
            SetSystemValue("PageOutType", ShuChuCombo.Text);
            SetSystemValue("qianming", qianming.Text);
            SetSystemValue("DefaultSite", defaultSiteBox.Text.Trim());
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

            if (bianjiCSS.SelectedIndex > 0)
            {
                System.IO.File.Delete(globalConst.AppPath + "\\style\\default.css");
                System.IO.File.Copy(globalConst.AppPath + "\\style\\" + bianjiCSS.SelectedItem, globalConst.AppPath + "\\style\\default.css");
            }

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
                        MsgBox.Error(res.com.str("Option.mustopensite"));
                        return;
                    }
                    if (dgv2.Rows[e.RowIndex].Cells[4].Value == null || !System.IO.File.Exists(dgv2.Rows[e.RowIndex].Cells[4].Value.ToString().Trim()))
                    {
                        MsgBox.Error(res.com.str("Option.filenotexist"));
                        return;
                    }
                    if (dgv2.Rows[e.RowIndex].Cells[0].Value == null || dgv2.Rows[e.RowIndex].Cells[0].Value.ToString() == "")
                    {
                        MsgBox.Error(res.com.str("Option.domin123"));
                        return;
                    }
                    string sql = "select * from sites where id='" + globalConst.CurSite.ID + "'";
                    SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
                    string _key = null;
                    string _user = null;
                    string _passwd = null;
                    if (rdr.Read())
                    {
                        _key = rdr.GetString(rdr.GetOrdinal("cdkey"));
                        _user = rdr.GetString(rdr.GetOrdinal("username"));
                        _passwd = rdr.GetString(rdr.GetOrdinal("passwd"));
                    }
                    rdr.Close();

                    dgv2.Rows[e.RowIndex].Cells[3].Value = res.com.str("Option.uploading");
                    Application.DoEvents();

                    string uploadurl = dgv2.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "/_ftpub/ftdllupload?_key=" + _key + "&_user=" + _user + "&_passwd=" + _passwd;
                    System.Net.WebClient wc = new System.Net.WebClient();
                    byte[] responseArray = wc.UploadFile(uploadurl, "POST", dgv2.Rows[e.RowIndex].Cells[4].Value.ToString().Trim());
                    if (!Encoding.UTF8.GetString(responseArray).Trim().Equals("{ftserver}ok"))
                    {
                        MsgBox.Error(Encoding.UTF8.GetString(responseArray).Replace("{ftserver}", ""));
                    }
                    else
                    {
                        MsgBox.Information(res.com.str("Option.uploadsuccess"));
                    }
                    dgv2.Rows[e.RowIndex].Cells[3].Value = res.com.str("Option.upload");
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
            e.Row.Cells[2].Value = res.com.str("Option.delete");
            e.Row.Cells[3].Value = res.com.str("Option.upload");
        }

        private void dgv_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[3].Value = res.com.str("Option.delete");
        }

        private void dgv2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = res.com.str("Option.pleaseselectfile");
                ofd.Multiselect = false;
                ofd.ShowDialog();
                if (ofd.FileName != null)
                {
                    dgv2.Rows[e.RowIndex].Cells[4].Value = ofd.FileName;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MsgBox.Information("Not Supported");
            return;
            if (MsgBox.OKCancel("确定要升级该站点到最新数据结构吗？") == DialogResult.OK)
            {
                if (globalConst.CurSite.ID == null)
                {
                    MsgBox.Error("请先打开站点");
                }
                else
                {
                    string sql = null;
                    try
                    {
                        sql = "alter table pages add COLUMN fore_frame varchar(36)";
                        globalConst.CurSite.SiteConn.execSql(sql);
                    }
                    catch { }
                    try
                    {
                        sql = "alter table pages add COLUMN out_type varchar(36)";
                        globalConst.CurSite.SiteConn.execSql(sql);
                    }
                    catch { }
                    try
                    {
                        sql = "create table codeset(id int PRIMARY KEY,dllname varchar(150),devuser varchar(50),codekey varchar(150),codeval varchar(255),returntype varchar(50),modtime datetime,mimo varchar(255))";
                        globalConst.CurSite.SiteConn.execSql(sql);
                    }
                    catch { }
                    try
                    {
                        string xmlNew = null;
                        sql = "select partxml from parts where controlname='list' and name='List'";
                        SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
                        if (rdr.Read())
                        {
                            string PartXml = rdr["partxml"].ToString();
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(PartXml);
                            XmlNodeList nodes = doc.SelectNodes("partxml/public_params/param");
                            bool MenuButtonSeted = false;
                            XmlNode nodeMenuOprole = null;
                            XmlNode nodeMenuOpname = null;
                            XmlNode nodeMenuOpcheck = null;
                            XmlNode nodeMenuOpendurer = null;
                            XmlNode nodeMenuOppic = null;
                            foreach (XmlNode node in nodes)
                            {
                                if (node.SelectSingleNode("name").InnerText == "MenuButtonSet")
                                {
                                    MenuButtonSeted = true;
                                }
                                if (node.SelectSingleNode("name").InnerText == "MenuOprole")
                                {
                                    nodeMenuOprole = node;
                                }
                                else if (node.SelectSingleNode("name").InnerText == "MenuOpname")
                                {
                                    nodeMenuOpname = node;
                                }
                                else if (node.SelectSingleNode("name").InnerText == "MenuOpcheck")
                                {
                                    nodeMenuOpcheck = node;
                                }
                                else if (node.SelectSingleNode("name").InnerText == "MenuOpendurer")
                                {
                                    nodeMenuOpendurer = node;
                                }
                                else if (node.SelectSingleNode("name").InnerText == "MenuOppic")
                                {
                                    nodeMenuOppic = node;
                                }
                            }
                            if (nodeMenuOprole != null) nodeMenuOprole.ParentNode.RemoveChild(nodeMenuOprole);
                            if (nodeMenuOpname != null) nodeMenuOpname.ParentNode.RemoveChild(nodeMenuOpname);
                            if (nodeMenuOpcheck != null) nodeMenuOpcheck.ParentNode.RemoveChild(nodeMenuOpcheck);
                            if (nodeMenuOpendurer != null) nodeMenuOpendurer.ParentNode.RemoveChild(nodeMenuOpendurer);
                            if (nodeMenuOppic != null) nodeMenuOppic.ParentNode.RemoveChild(nodeMenuOppic);
                            if (!MenuButtonSeted)
                            {
                                XmlNode newNode = doc.CreateNode("element", "param", "");
                                string newXml = "<name>MenuButtonSet</name>";
                                newXml += "<type>string</type>";
                                newXml += "<caption>菜单按钮设置</caption>";
                                newXml += "<description>定义列表页菜单按钮</description>";
                                newXml += "<class>string</class>";
                                newXml += "<default></default>";
                                newXml += "<category>6.菜单</category>";
                                newNode.InnerXml = newXml;
                                doc.SelectSingleNode("partxml/public_params").AppendChild(newNode);
                            }
                            xmlNew = doc.OuterXml;
                        }
                        rdr.Close();
                        if (xmlNew != null)
                        {
                            sql = "update parts set partxml='" + functions.str.Dot2DotDot(xmlNew) + "' where  controlname='list' and name='List'";
                            globalConst.ConfigConn.execSql(sql);
                        }
                    }
                    catch { }
                    try
                    {
                        string xmlNew = null;
                        sql = "select partxml from parts where controlname='list' and name='List'";
                        SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
                        if (rdr.Read())
                        {
                            string PartXml = rdr["partxml"].ToString();
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(PartXml);
                            XmlNodeList nodes = doc.SelectNodes("partxml/public_params/param");
                            bool APISeted = false;
                            foreach (XmlNode node in nodes)
                            {
                                if (node.SelectSingleNode("name").InnerText == "APISet")
                                {
                                    APISeted = true;
                                    break;
                                }
                            }
                            if (!APISeted)
                            {
                                XmlNode newNode = doc.CreateNode("element", "param", "");
                                string newXml = "<name>APISet</name>";
                                newXml += "<type>string</type>";
                                newXml += "<caption>API设置</caption>";
                                newXml += "<description>定义列表页输出API时的设置</description>";
                                newXml += "<class>string</class>";
                                newXml += "<default></default>";
                                newXml += "<category>8.其他</category>";
                                newNode.InnerXml = newXml;
                                doc.SelectSingleNode("partxml/public_params").AppendChild(newNode);
                            }
                            xmlNew = doc.OuterXml;
                        }
                        rdr.Close();
                        if (xmlNew != null)
                        {
                            sql = "update parts set partxml='" + functions.str.Dot2DotDot(xmlNew) + "' where  controlname='list' and name='List'";
                            globalConst.ConfigConn.execSql(sql);
                        }
                    }
                    catch { }
                    //DyValue增加APISet节点
                    try
                    {
                        string xmlNew = null;
                        sql = "select partxml from parts where controlname='dyvalue' and name='Interface'";
                        SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
                        if (rdr.Read())
                        {
                            string PartXml = rdr["partxml"].ToString();
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(PartXml);
                            XmlNodeList nodes = doc.SelectNodes("partxml/public_params/param");
                            bool APISeted = false;
                            foreach (XmlNode node in nodes)
                            {
                                if (node.SelectSingleNode("name").InnerText == "APISet")
                                {
                                    APISeted = true;
                                    break;
                                }
                            }
                            if (!APISeted)
                            {
                                XmlNode newNode = doc.CreateNode("element", "param", "");
                                string newXml = "<name>APISet</name>";
                                newXml += "<type>string</type>";
                                newXml += "<caption>10.API设置</caption>";
                                newXml += "<description>定义数据获取输出API时的设置</description>";
                                newXml += "<class>string</class>";
                                newXml += "<default></default>";
                                newXml += "<category>设置</category>";
                                newNode.InnerXml = newXml;
                                doc.SelectSingleNode("partxml/public_params").AppendChild(newNode);
                            }
                            xmlNew = doc.OuterXml;
                        }
                        rdr.Close();
                        if (xmlNew != null)
                        {
                            sql = "update parts set partxml='" + functions.str.Dot2DotDot(xmlNew) + "' where  controlname='dyvalue' and name='Interface'";
                            globalConst.ConfigConn.execSql(sql);
                        }
                    }
                    catch { }
                    //DataOP增加APISet节点
                    try
                    {
                        string xmlNew = null;
                        sql = "select partxml from parts where controlname='dataop' and name='Interface'";
                        SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
                        if (rdr.Read())
                        {
                            string PartXml = rdr["partxml"].ToString();
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(PartXml);
                            XmlNodeList nodes = doc.SelectNodes("partxml/public_params/param");
                            bool APISeted = false;
                            foreach (XmlNode node in nodes)
                            {
                                if (node.SelectSingleNode("name").InnerText == "APISet")
                                {
                                    APISeted = true;
                                    break;
                                }
                            }
                            if (!APISeted)
                            {
                                XmlNode newNode = doc.CreateNode("element", "param", "");
                                string newXml = "<name>APISet</name>";
                                newXml += "<type>string</type>";
                                newXml += "<caption>20.API设置</caption>";
                                newXml += "<description>定义数据操作API时的设置</description>";
                                newXml += "<class>string</class>";
                                newXml += "<default></default>";
                                newXml += "<category>设置</category>";
                                newNode.InnerXml = newXml;
                                doc.SelectSingleNode("partxml/public_params").AppendChild(newNode);
                            }
                            xmlNew = doc.OuterXml;
                        }
                        rdr.Close();
                        if (xmlNew != null)
                        {
                            sql = "update parts set partxml='" + functions.str.Dot2DotDot(xmlNew) + "' where  controlname='dataop' and name='Interface'";
                            globalConst.ConfigConn.execSql(sql);
                        }
                    }
                    catch { }
                    //DyValue增加FidCol节点，OpDefaultCol节点
                    try
                    {
                        string xmlNew = null;
                        sql = "select partxml from parts where controlname='dyvalue' and name='Interface'";
                        SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
                        if (rdr.Read())
                        {
                            string PartXml = rdr["partxml"].ToString();
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(PartXml);
                            XmlNodeList nodes = doc.SelectNodes("partxml/public_params/param");
                            bool FidColSeted = false;
                            foreach (XmlNode node in nodes)
                            {
                                if (node.SelectSingleNode("name").InnerText == "FidCol")
                                {
                                    FidColSeted = true;
                                    break;
                                }
                            }
                            if (!FidColSeted)
                            {
                                XmlNode newNode = doc.CreateNode("element", "param", "");
                                string newXml = "<name>FidCol</name>";
                                newXml += "<type>string</type>";
                                newXml += "<caption>7.主键列名</caption>";
                                newXml += "<description>定义数据获取主键列名,默认为fid</description>";
                                newXml += "<class>string</class>";
                                newXml += "<default></default>";
                                newXml += "<category>设置</category>";
                                newNode.InnerXml = newXml;
                                doc.SelectSingleNode("partxml/public_params").AppendChild(newNode);
                            }
                            bool OpDefaultColSeted = false;
                            foreach (XmlNode node in nodes)
                            {
                                if (node.SelectSingleNode("name").InnerText == "OpDefaultCol")
                                {
                                    OpDefaultColSeted = true;
                                    break;
                                }
                            }
                            if (!OpDefaultColSeted)
                            {
                                XmlNode newNode = doc.CreateNode("element", "param", "");
                                string newXml = "<name>OpDefaultCol</name>";
                                newXml += "<type>int</type>";
                                newXml += "<caption>8.存在保留列</caption>";
                                newXml += "<description>配置是否存在保留列：fmem,modfmem,addtime,updatetime,dydata,stat,flow,flowpos</description>";
                                newXml += "<class>enum</class>";
                                newXml += "<default>1</default>";
                                newXml += "<category>设置</category>";
                                newXml += "<enums><enum>否</enum><enum>是</enum></enums>";
                                newNode.InnerXml = newXml;
                                doc.SelectSingleNode("partxml/public_params").AppendChild(newNode);
                            }
                            xmlNew = doc.OuterXml;
                        }
                        rdr.Close();
                        if (xmlNew != null)
                        {
                            sql = "update parts set partxml='" + functions.str.Dot2DotDot(xmlNew) + "' where  controlname='dyvalue' and name='Interface'";
                            globalConst.ConfigConn.execSql(sql);
                        }
                    }
                    catch { }
                    //DataOP增加FidCol，OpDefaultCol节点
                    try
                    {
                        string xmlNew = null;
                        sql = "select partxml from parts where controlname='dataop' and name='Interface'";
                        SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql);
                        if (rdr.Read())
                        {
                            string PartXml = rdr["partxml"].ToString();
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(PartXml);
                            XmlNodeList nodes = doc.SelectNodes("partxml/public_params/param");
                            bool FidColSeted = false;
                            foreach (XmlNode node in nodes)
                            {
                                if (node.SelectSingleNode("name").InnerText == "FidCol")
                                {
                                    FidColSeted = true;
                                    break;
                                }
                            }
                            if (!FidColSeted)
                            {
                                XmlNode newNode = doc.CreateNode("element", "param", "");
                                string newXml = "<name>FidCol</name>";
                                newXml += "<type>string</type>";
                                newXml += "<caption>15.主键列名</caption>";
                                newXml += "<description>定义数据操作主键列名,默认为fid</description>";
                                newXml += "<class>string</class>";
                                newXml += "<default></default>";
                                newXml += "<category>设置</category>";
                                newNode.InnerXml = newXml;
                                doc.SelectSingleNode("partxml/public_params").AppendChild(newNode);
                            }
                            bool OpDefaultColSeted = false;
                            foreach (XmlNode node in nodes)
                            {
                                if (node.SelectSingleNode("name").InnerText == "OpDefaultCol")
                                {
                                    OpDefaultColSeted = true;
                                    break;
                                }
                            }
                            if (!OpDefaultColSeted)
                            {
                                XmlNode newNode = doc.CreateNode("element", "param", "");
                                string newXml = "<name>OpDefaultCol</name>";
                                newXml += "<type>int</type>";
                                newXml += "<caption>16.更新保留列</caption>";
                                newXml += "<description>配置是否自动更新保留列：fmem,modfmem,addtime,updatetime,dydata,stat,flow,flowpos</description>";
                                newXml += "<class>enum</class>";
                                newXml += "<default>1</default>";
                                newXml += "<category>设置</category>";
                                newXml += "<enums><enum>否</enum><enum>是</enum></enums>";
                                newNode.InnerXml = newXml;
                                doc.SelectSingleNode("partxml/public_params").AppendChild(newNode);
                            }
                            xmlNew = doc.OuterXml;
                        }
                        rdr.Close();
                        if (xmlNew != null)
                        {
                            sql = "update parts set partxml='" + functions.str.Dot2DotDot(xmlNew) + "' where  controlname='dataop' and name='Interface'";
                            globalConst.ConfigConn.execSql(sql);
                        }
                    }
                    catch { }
                    MsgBox.Information("升级成功！");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID == null)
            {
                MsgBox.Error("请先打开站点");
            }
            else
            {
                string sql = null;
                sql = "select a.id,a.partxml from parts a,controls b where a.controlid=b.id and b.name='List' and a.name='list'";
                List<string> SqlList = new List<string>();
                SqliteDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                while (rdr.Read())
                {
                    string partid = rdr.GetString(0);
                    string partxml = rdr.GetString(1);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(partxml);
                    XmlNodeList nodes = doc.SelectNodes("partxml/param");
                    bool MenuButtonSeted = false;
                    XmlNode nodeMenuOprole = null;
                    XmlNode nodeMenuOpname = null;
                    XmlNode nodeMenuOpcheck = null;
                    XmlNode nodeMenuOpendurer = null;
                    XmlNode nodeMenuOppic = null;
                    foreach (XmlNode node in nodes)
                    {
                        if (node.SelectSingleNode("name").InnerText == "MenuButtonSet")
                        {
                            MenuButtonSeted = true;
                        }
                        if (node.SelectSingleNode("name").InnerText == "MenuOprole")
                        {
                            nodeMenuOprole = node;
                        }
                        else if (node.SelectSingleNode("name").InnerText == "MenuOpname")
                        {
                            nodeMenuOpname = node;
                        }
                        else if (node.SelectSingleNode("name").InnerText == "MenuOpcheck")
                        {
                            nodeMenuOpcheck = node;
                        }
                        else if (node.SelectSingleNode("name").InnerText == "MenuOpendurer")
                        {
                            nodeMenuOpendurer = node;
                        }
                        else if (node.SelectSingleNode("name").InnerText == "MenuOppic")
                        {
                            nodeMenuOppic = node;
                        }
                    }
                    if (nodeMenuOpname != null && nodeMenuOprole != null)
                    {
                        string[] Opnameval = nodeMenuOpname.SelectSingleNode("value").InnerText.Split(';');
                        string[] Oproleval = nodeMenuOprole.SelectSingleNode("value").InnerText.Split(';');
                        string[] Opendureval = nodeMenuOpendurer.SelectSingleNode("value").InnerText.Split(new string[] { "##" }, StringSplitOptions.None);
                        string[] Opcheckval = nodeMenuOpcheck.SelectSingleNode("value").InnerText.Split(';');
                        string[] Oppicval = nodeMenuOppic.SelectSingleNode("value").InnerText.Split(';');
                        nodeMenuOpname.ParentNode.RemoveChild(nodeMenuOpname);
                        nodeMenuOprole.ParentNode.RemoveChild(nodeMenuOprole);
                        nodeMenuOpendurer.ParentNode.RemoveChild(nodeMenuOpendurer);
                        nodeMenuOpcheck.ParentNode.RemoveChild(nodeMenuOpcheck);
                        nodeMenuOppic.ParentNode.RemoveChild(nodeMenuOppic);
                        string s = "";
                        for (int i = 0; i < Opnameval.Length; i++)
                        {
                            s += "{$$}" + Opnameval[i] + "[##]" + Opendureval[i] + "[##]" + Oproleval[i] + "[##]" + Oppicval[i] + "[##]" + Opcheckval[i];
                        }
                        if (s != "") s = s.Substring(4);
                        if (!MenuButtonSeted)
                        {
                            XmlNode newNode = doc.CreateNode("element", "param", "");
                            string newXml = "<name>MenuButtonSet</name>";
                            newXml += "<type>string</type>";
                            newXml += "<value>" + s + "</value>";
                            newNode.InnerXml = newXml;
                            doc.SelectSingleNode("partxml").AppendChild(newNode);
                        }
                        string xmlNew = doc.OuterXml;
                        sql = "update parts set partxml='" + functions.str.Dot2DotDot(xmlNew) + "' where  id='" + partid + "'";
                        SqlList.Add(sql);
                    }
                }
                rdr.Close();
                foreach (string sqlstr in SqlList)
                {
                    globalConst.CurSite.SiteConn.execSql(sqlstr);
                }
                MsgBox.Information("升级成功！");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Enabled = false;
            string oriText = button7.Text;
            button7.Text = "转换中...";
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "选择包含旧数据库类型的config.db,site_*.db的目录";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string dir = dialog.SelectedPath;
                    if (Directory.Exists(dir + @"\newdb")) Directory.Delete(dir + @"\newdb", true);
                    Directory.CreateDirectory(dir + @"\newdb");
                    foreach (string filepath in Directory.GetFiles(dir))
                    {
                        if (filepath.EndsWith(".db"))
                        {
                            FileInfo file = new FileInfo(filepath);
                            if (file.Name == "config.db")
                            {
                                File.Copy(Application.StartupPath + @"\cfg\template\config.db", dir + @"\newdb\config.db");
                                using (DB db = new DB())
                                {
                                    db.Open(functions.db.ConnStr(dir + @"\newdb\config.db"));
                                    using (DBOle dBOle = new DBOle())
                                    {
                                        dBOle.Open("Provider=" + globalConst.OLEDBProvider + ";Data Source=" + filepath);
                                        tableData(dBOle, db, "controls");
                                        tableData(dBOle, db, "languages");
                                        tableData(dBOle, db, "lastlist");
                                        tableData(dBOle, db, "parts");
                                        tableData(dBOle, db, "sites");
                                        tableData(dBOle, db, "snippets");
                                        tableData(dBOle, db, "system");
                                    }
                                }
                            }
                            else if (file.Name.StartsWith("site_"))
                            {
                                File.Copy(Application.StartupPath + @"\cfg\template\empty.db", dir + @"\newdb\" + file.Name);
                                using (DB db = new DB())
                                {
                                    db.Open(functions.db.ConnStr(dir + @"\newdb\" + file.Name));
                                    using (DBOle dBOle = new DBOle())
                                    {
                                        dBOle.Open("Provider=" + globalConst.OLEDBProvider + ";Data Source=" + filepath);
                                        tableData(dBOle, db, "codeset");
                                        tableData(dBOle, db, "controls");
                                        tableData(dBOle, db, "deledds");
                                        tableData(dBOle, db, "directory");
                                        tableData(dBOle, db, "formrules");
                                        tableData(dBOle, db, "pages");
                                        tableData(dBOle, db, "part_in_page");
                                        tableData(dBOle, db, "parts");
                                        tableData(dBOle, db, "share_data");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
            button7.Enabled = true;
            button7.Text = oriText;
            MsgBox.Information("转换成功，新数据文件在 newdb 目录下");
            void tableData(DBOle dBOle, DB db, string table)
            {
                using (DROle dROle = new DROle(dBOle.OpenRecord("select * from " + table)))
                {
                    while (dROle.Read())
                    {
                        string sql = "insert into " + table + "(";
                        string val = "values(";
                        for (int i = 0; i < dROle.FieldCount; i++)
                        {
                            sql += dROle.GetName(i);
                            val += "'" + str.Dot2DotDot(dROle.getValue(i).ToString()) + "'";
                            if (i < dROle.FieldCount - 1)
                            {
                                sql += ",";
                                val += ",";
                            }
                        }
                        sql = sql + ")" + val + ")";
                        db.execSql(sql);
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var selItem = upgrade.SelectedItem as Obj.ComboItem;
            switch(selItem.Id)
            {
                case "":
                    MsgBox.Warning("Please Select Upgrade Item !");
                    break;
                case "1"://DataOP,DyValue增加配置:自定义数据库链接
                    string sql = null;
                    //list 更改注释
                    try
                    {
                        string xmlNew = null;
                        sql = "select partxml from parts where controlname='list' and name='List'";
                        using (SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql))
                        {
                            if (rdr.Read())
                            {
                                string PartXml = rdr["partxml"].ToString();
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(PartXml);
                                XmlNodeList nodes = doc.SelectNodes("partxml/public_params/param");
                                XmlNode setNode = null;
                                foreach (XmlNode node in nodes)
                                {
                                    if (node.SelectSingleNode("name").InnerText == "CustomConnection")
                                    {
                                        setNode = node;
                                        break;
                                    }
                                }
                                if (setNode != null)
                                {
                                    setNode.SelectSingleNode("description").InnerText = "数据库连接串，从非当前数据库中获取数据，@code开始为从后台代码获取，@para开始为从站点参数获取，其他为数据库连接串。若要指定默认数据库类型，用##隔开，后面跟数据库类型，例如：MySql、SqlServer、Oracle、Sqlite";
                                }
                                xmlNew = doc.OuterXml;
                            }
                        }
                        if (xmlNew != null)
                        {
                            sql = "update parts set partxml='" + functions.str.Dot2DotDot(xmlNew) + "' where controlname='list' and name='List'";
                            globalConst.ConfigConn.execSql(sql);
                        }
                    }
                    catch (Exception ex) { new error(ex); }
                    //DyValue
                    try
                    {
                        string xmlNew = null;
                        sql = "select partxml from parts where controlname='dyvalue' and name='Interface'";
                        using (SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql))
                        {
                            if (rdr.Read())
                            {
                                string PartXml = rdr["partxml"].ToString();
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(PartXml);
                                XmlNodeList nodes = doc.SelectNodes("partxml/public_params/param");
                                bool Seted = false;
                                foreach (XmlNode node in nodes)
                                {
                                    if (node.SelectSingleNode("name").InnerText == "CustomConnection")
                                    {
                                        Seted = true;
                                        break;
                                    }
                                }
                                if (!Seted)
                                {
                                    XmlNode newNode = doc.CreateNode("element", "param", "");
                                    string newXml = "<name>CustomConnection</name>";
                                    newXml += "<type>string</type>";
                                    newXml += "<caption>13.自定义数据库连接</caption>";
                                    newXml += "<description>数据库连接串，从非当前数据库中获取数据，@code开始为从后台代码获取，@para开始为从站点参数获取，其他为数据库连接串。若要指定默认数据库类型，用##隔开，后面跟数据库类型，例如：MySql、SqlServer、Oracle、Sqlite</description>";
                                    newXml += "<class>string</class>";
                                    newXml += "<default></default>";
                                    newXml += "<category>设置</category>";
                                    newNode.InnerXml = newXml;
                                    doc.SelectSingleNode("partxml/public_params").AppendChild(newNode);
                                }
                                xmlNew = doc.OuterXml;
                            }
                        }
                        if (xmlNew != null)
                        {
                            sql = "update parts set partxml='" + functions.str.Dot2DotDot(xmlNew) + "' where  controlname='dyvalue' and name='Interface'";
                            globalConst.ConfigConn.execSql(sql);
                        }
                    }
                    catch (Exception ex) { new error(ex); }
                    //DataOP
                    try
                    {
                        string xmlNew = null;
                        sql = "select partxml from parts where controlname='dataop' and name='Interface'";
                        using (SqliteDataReader rdr = globalConst.ConfigConn.OpenRecord(sql))
                        {
                            if (rdr.Read())
                            {
                                string PartXml = rdr["partxml"].ToString();
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(PartXml);
                                XmlNodeList nodes = doc.SelectNodes("partxml/public_params/param");
                                bool Seted = false;
                                foreach (XmlNode node in nodes)
                                {
                                    if (node.SelectSingleNode("name").InnerText == "CustomConnection")
                                    {
                                        Seted = true;
                                        break;
                                    }
                                }
                                if (!Seted)
                                {
                                    XmlNode newNode = doc.CreateNode("element", "param", "");
                                    string newXml = "<name>CustomConnection</name>";
                                    newXml += "<type>string</type>";
                                    newXml += "<caption>21.自定义数据库连接</caption>";
                                    newXml += "<description>数据库连接串，从非当前数据库中获取数据，@code开始为从后台代码获取，@para开始为从站点参数获取，其他为数据库连接串。若要指定默认数据库类型，用##隔开，后面跟数据库类型，例如：MySql、SqlServer、Oracle、Sqlite</description>";
                                    newXml += "<class>string</class>";
                                    newXml += "<default></default>";
                                    newXml += "<category>设置</category>";
                                    newNode.InnerXml = newXml;
                                    doc.SelectSingleNode("partxml/public_params").AppendChild(newNode);
                                }
                                xmlNew = doc.OuterXml;
                            }
                        }
                        if (xmlNew != null)
                        {
                            sql = "update parts set partxml='" + functions.str.Dot2DotDot(xmlNew) + "' where  controlname='dataop' and name='Interface'";
                            globalConst.ConfigConn.execSql(sql);
                        }
                    }
                    catch(Exception ex) { new error(ex); }
                    MsgBox.Information("完成!"+Environment.NewLine+ "还需在 工具->更新站点片段，对站点组件（list、dataop、dyvalue）配置进行更新");
                    break;
            }
        }
    }
}