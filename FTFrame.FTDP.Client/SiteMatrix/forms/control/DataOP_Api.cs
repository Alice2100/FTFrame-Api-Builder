using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FTDPClient.consts;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using FTDPClient.forms.control;

namespace FTDPClient.forms
{
    public class DataOP_Api : System.Windows.Forms.Form
	{
        public string partId = null;
        public string restr = "";
        public List<string[]> nameCapList;
        public bool IsCancel = true;
        private Button Close;
        private Button OK;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private Button button1;
        private Button button2;
        private Label label2;
        private ArrayList al;
        private TextBox luyou;
        private TextBox cfg;
        private Button button3;
        private Button button4;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem delToolStripMenuItem;
        private ToolStripMenuItem upToolStripMenuItem;
        private ToolStripMenuItem downToolStripMenuItem;
        private Label label1;
        private TextBox miaosu;
        private Label label9;
        private Label label10;
        private Label label11;
        private Panel pl;
        private RadioButton inputForm;
        private RadioButton inputJson;
        private CheckBox SelAllCheck;
        private IContainer components;

        public DataOP_Api()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();
            ApplyLanguage();
            //
            // TODO: 在 InitializeComponent 调用后添加任何构造函数代码
            //
            /*
             {$$GetList[##]得到客户列表[##]orderby,ordertype,keyvalue.fid[##]des1[#]des2[##]说明1[#]说明2
             {$$}ClientDel[##]客户删除[##]keyvalue.fid,status1.stat,status2.modtime[##]des1[#]des2[#]des3[##]说明1[#]说明2[#]说明3
             */
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.Close = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.delToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.luyou = new System.Windows.Forms.TextBox();
            this.cfg = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.miaosu = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pl = new System.Windows.Forms.Panel();
            this.inputForm = new System.Windows.Forms.RadioButton();
            this.inputJson = new System.Windows.Forms.RadioButton();
            this.SelAllCheck = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Close
            // 
            this.Close.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Close.Location = new System.Drawing.Point(637, 496);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 30);
            this.Close.TabIndex = 5;
            this.Close.Text = "&Close";
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // OK
            // 
            this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OK.Location = new System.Drawing.Point(524, 496);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 30);
            this.OK.TabIndex = 6;
            this.OK.Text = "&OK";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(156, 312);
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Tile;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 200;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.delToolStripMenuItem,
            this.upToolStripMenuItem,
            this.downToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(110, 92);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // delToolStripMenuItem
            // 
            this.delToolStripMenuItem.Name = "delToolStripMenuItem";
            this.delToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.delToolStripMenuItem.Text = "Del";
            this.delToolStripMenuItem.Click += new System.EventHandler(this.delToolStripMenuItem_Click);
            // 
            // upToolStripMenuItem
            // 
            this.upToolStripMenuItem.Name = "upToolStripMenuItem";
            this.upToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.upToolStripMenuItem.Text = "Up";
            this.upToolStripMenuItem.Click += new System.EventHandler(this.upToolStripMenuItem_Click);
            // 
            // downToolStripMenuItem
            // 
            this.downToolStripMenuItem.Name = "downToolStripMenuItem";
            this.downToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.downToolStripMenuItem.Text = "Down";
            this.downToolStripMenuItem.Click += new System.EventHandler(this.downToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(12, 330);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 24);
            this.button1.TabIndex = 8;
            this.button1.Text = "&Add";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(52, 330);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(36, 24);
            this.button2.TabIndex = 9;
            this.button2.Text = "&Del";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "接口路由";
            // 
            // luyou
            // 
            this.luyou.Font = new System.Drawing.Font("宋体", 12F);
            this.luyou.Location = new System.Drawing.Point(244, 9);
            this.luyou.Name = "luyou";
            this.luyou.Size = new System.Drawing.Size(157, 26);
            this.luyou.TabIndex = 22;
            this.luyou.Leave += new System.EventHandler(this.TextBox_Leave);
            // 
            // cfg
            // 
            this.cfg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.cfg.Location = new System.Drawing.Point(12, 360);
            this.cfg.Multiline = true;
            this.cfg.Name = "cfg";
            this.cfg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cfg.Size = new System.Drawing.Size(156, 132);
            this.cfg.TabIndex = 30;
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(92, 330);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(36, 24);
            this.button3.TabIndex = 34;
            this.button3.Text = "&Up";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Location = new System.Drawing.Point(132, 330);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(36, 24);
            this.button4.TabIndex = 35;
            this.button4.Text = "Do&w";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(408, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 40;
            this.label1.Text = "接口描述";
            // 
            // miaosu
            // 
            this.miaosu.Font = new System.Drawing.Font("宋体", 12F);
            this.miaosu.Location = new System.Drawing.Point(467, 9);
            this.miaosu.Name = "miaosu";
            this.miaosu.Size = new System.Drawing.Size(225, 26);
            this.miaosu.TabIndex = 45;
            this.miaosu.DoubleClick += new System.EventHandler(this.miaosu_DoubleClick);
            this.miaosu.Leave += new System.EventHandler(this.TextBox_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label9.Location = new System.Drawing.Point(185, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(119, 12);
            this.label9.TabIndex = 47;
            this.label9.Text = "输入（_开头不显示）";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label10.Location = new System.Drawing.Point(363, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 48;
            this.label10.Text = "Name列";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label11.Location = new System.Drawing.Point(510, 45);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 49;
            this.label11.Text = "说明";
            // 
            // pl
            // 
            this.pl.AutoScroll = true;
            this.pl.Location = new System.Drawing.Point(187, 72);
            this.pl.Name = "pl";
            this.pl.Size = new System.Drawing.Size(525, 420);
            this.pl.TabIndex = 51;
            // 
            // inputForm
            // 
            this.inputForm.AutoSize = true;
            this.inputForm.Location = new System.Drawing.Point(299, 503);
            this.inputForm.Name = "inputForm";
            this.inputForm.Size = new System.Drawing.Size(71, 16);
            this.inputForm.TabIndex = 94;
            this.inputForm.Text = "Form输入";
            this.inputForm.UseVisualStyleBackColor = true;
            this.inputForm.Click += new System.EventHandler(this.inputForm_Click);
            // 
            // inputJson
            // 
            this.inputJson.AutoSize = true;
            this.inputJson.Checked = true;
            this.inputJson.Location = new System.Drawing.Point(187, 503);
            this.inputJson.Name = "inputJson";
            this.inputJson.Size = new System.Drawing.Size(71, 16);
            this.inputJson.TabIndex = 93;
            this.inputJson.TabStop = true;
            this.inputJson.Text = "Json输入";
            this.inputJson.UseVisualStyleBackColor = true;
            this.inputJson.Click += new System.EventHandler(this.inputJson_Click);
            // 
            // SelAllCheck
            // 
            this.SelAllCheck.AutoSize = true;
            this.SelAllCheck.Font = new System.Drawing.Font("宋体", 10F);
            this.SelAllCheck.Location = new System.Drawing.Point(425, 502);
            this.SelAllCheck.Name = "SelAllCheck";
            this.SelAllCheck.Size = new System.Drawing.Size(54, 18);
            this.SelAllCheck.TabIndex = 95;
            this.SelAllCheck.Text = "全选";
            this.SelAllCheck.UseVisualStyleBackColor = true;
            this.SelAllCheck.CheckedChanged += new System.EventHandler(this.SelAllCheck_CheckedChanged);
            // 
            // DataOP_Api
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(719, 529);
            this.Controls.Add(this.SelAllCheck);
            this.Controls.Add(this.inputForm);
            this.Controls.Add(this.inputJson);
            this.Controls.Add(this.pl);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.miaosu);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.cfg);
            this.Controls.Add(this.luyou);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataOP_Api";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据操作 Api";
            this.Load += new System.EventHandler(this.BackValue_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RowAll_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
#endregion
        private void ApplyLanguage()
        {
            //OK.Text = res.About.GetString("String1");
            //Close.Text = res.About.GetString("String2");
           
        }
        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            al.Clear();
            restr = cfg.Text;
            IsCancel = false;
            this.Close();
        }
        private void InitCfgStr()
        {
            string str = "";
            foreach (string[] item in al)
            {
                if (!item[0].Trim().Equals(""))
                {
                    str += "{$$}" + item[0].Replace("\r\n", "") + "[##]" + item[1].Replace("\r\n", "") + "[##]" + item[2].Replace("\r\n", "") + "[##]" + item[3].Replace("\r\n", "") + "[##]" + item[4].Replace("\r\n", "") + "[##]" + item[5];
                }
            }
            if (!str.Equals("")) str = str.Substring(4);
            cfg.Text = str;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string NewName = "Add_" + (this.listView1.Items.Count + 1);
            string NewNameCap = res.ctl.str("DataOP_Api.1") +"_" + (this.listView1.Items.Count + 1);
            ListViewItem listview = new ListViewItem(new string[] {NewName}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))));
            listview.ImageIndex = 26;
            this.listView1.Items.Add(listview);
            string ApiNames = "";
            string ApiIds = "";
            string ApiDecs = "";
            foreach(string[] item in nameCapList)
            {
                ApiNames += "[#]" + item[0];
                ApiIds += "[#]" + item[0];
                ApiDecs += "[#]" + item[1];
            }
            if(nameCapList.Count>0)
            {
                ApiNames = ApiNames.Substring(3); ApiIds = ApiIds.Substring(3); ApiDecs = ApiDecs.Substring(3); 
            }

            al.Add(new string[] { NewName, NewNameCap, ApiNames, ApiIds, ApiDecs,"json" });
            listView1.SelectedItems.Clear();
            InitCfgStr();
            listview.Selected = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                al.RemoveAt(listView1.SelectedItems[i].Index);
                listView1.SelectedItems[i].Remove();
            }
            listView1.SelectedItems.Clear();
            InitCfgStr();
        }

        private void BackValue_Load(object sender, EventArgs e)
        {
            this.Text = res.ctl.str("DataOP_Api.text");         //数据操作 Api
            label2.Text = res.ctl.str("DataOP_Api.label2");         //接口路由
            label1.Text = res.ctl.str("DataOP_Api.label1");         //接口描述
            label9.Text = res.ctl.str("DataOP_Api.label9");         //输入（_开头不显示）
            label10.Text = res.ctl.str("DataOP_Api.label10");           //Name列
            label11.Text = res.ctl.str("DataOP_Api.label11");			//说明
            inputJson.Text = res.ctl.str("JsonInput");
            inputForm.Text = res.ctl.str("FormInput");
            SelAllCheck.Text = res.ctl.str("RawSelCols.selall");    //全选
            cfg.Text = restr;
            this.listView1.SmallImageList = globalConst.Imgs;
            this.listView1.LargeImageList = globalConst.Imgs;
            setInitFromSetString(restr);
        }

        private void setInitFromSetString(string setstr)
        {
            listView1.Items.Clear();
            string[] items = setstr.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
            al = new ArrayList();
            foreach (string item in items)
            {
                string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                if (colcfg.Length == 5)
                {
                    var list = new List<string>();
                    list.AddRange(colcfg);
                    list.Add("form");
                    colcfg = list.ToArray();
                }
                al.Add(colcfg);
            }
            foreach (string[] item in al)
            {
                ListViewItem listview = new ListViewItem(new string[] {
            (item[0].Trim().Equals("")?"Add_"+(this.listView1.Items.Count+1):(item[0].Trim()))}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))));
                listview.ImageIndex = 26;
                this.listView1.Items.Add(listview);
            }
            if (listView1.Items.Count > 0) listView1.Items[0].Selected = true;
            listView1_SelectedIndexChanged(null, null);
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                label1.Visible = false;
                label2.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                label11.Visible = false;
                luyou.Visible = false;
                miaosu.Visible = false;
                inputJson.Visible = false; inputForm.Visible = false;
                return;
            }
            foreach (ListViewItem listitem in listView1.Items)
            {
                if (listitem.ForeColor.Equals(Color.LightGray))
                {
                    listitem.ForeColor = Color.LightGray;
                }
                else
                {
                    listitem.ForeColor = System.Drawing.Color.Black;
                }
                listitem.BackColor = System.Drawing.Color.White;
                
                listitem.Font = new Font("宋体", 12F, System.Drawing.FontStyle.Regular);
            }
            listView1.SelectedItems[0].BackColor = System.Drawing.SystemColors.Highlight;
            if (listView1.SelectedItems[0].ForeColor.Equals(Color.LightGray))
            {
                listView1.SelectedItems[0].ForeColor = Color.LightGray;
            }
            else
            {
                listView1.SelectedItems[0].ForeColor = System.Drawing.Color.White;
            }
            listView1.SelectedItems[0].Font = new Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            label1.Visible = true;
            label2.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
            luyou.Visible = true;
            miaosu.Visible = true;
            inputJson.Visible = true; inputForm.Visible = true;

            string[] item = ((string[])al[listView1.SelectedItems[0].Index]);
            luyou.Text = item[0];
            miaosu.Text = item[1];
            string inputType = item[5];
            if (inputType == "form") inputForm.Checked = true;
            else inputJson.Checked = true;
            TextBoxInit(item[2], item[3], item[4]);
        }
        List<Control[]> ctlList = new List<Control[]>();
       private void TextBoxInit(string ApiNames,string ApiIDs,string ApiDecs)
        {
            foreach(Control[] ctls in ctlList)
            {
                pl.Controls.Remove(ctls[0]);
                pl.Controls.Remove(ctls[1]);
                pl.Controls.Remove(ctls[2]);
                pl.Controls.Remove(ctls[3]);
            }
            ctlList.Clear();
            string[] ApiNamesItem = ApiNames.Split(new string[] { "[#]" }, StringSplitOptions.None);
            string[] ApiIDsItem = ApiIDs.Split(new string[] { "[#]" }, StringSplitOptions.None);
            string[] ApiDecsItem = ApiDecs.Split(new string[] { "[#]" }, StringSplitOptions.None);
            for(int i=0;i< nameCapList.Count;i++)
            {
                string ApiId = nameCapList[i][0];
                string ApiDesc = nameCapList[i][1];
                int index = -1;
                for(int j=0;j< ApiIDsItem.Length;j++)
                {
                    if(ApiIDsItem[j]==ApiId)
                    {
                        index = j;
                        break;
                    }
                }
                CheckBox ckBox = new CheckBox()
                {
                    AutoSize = true,
                    Font = new Font("宋体", 12F),
                    Size = new Size(83, 20),
                    Location = new Point(0, 2 + 36 * i),
                    Tag = i
                };
                TextBox textBox1 = new TextBox()
                {
                    Font = new Font("宋体", 12F),
                    Size = new Size(155, 26),
                    Location = new Point(16, 0 + 36 * i),
                    Tag = i
                };
                Label label2 = new Label()
                {
                    Font = new Font("宋体", 12F),
                    Location = new Point(178, 0 + 36 * i),
                    Tag = i
                };
                TextBox textBox3 = new TextBox()
                {
                    Font = new Font("宋体", 12F),
                    Size = new Size(180, 26),
                    Location = new Point(325, 0 + 36 * i),
                    Tag = i
                };
                ckBox.Click += CkBox_Click;
                label2.Click += Label2_Click;
                textBox1.Leave += TextBox_Leave;
                label2.Leave += TextBox_Leave;
                textBox3.Leave += TextBox_Leave;
                if (index>=0)
                {
                    ckBox.Checked = true;
                    textBox1.Text = ApiNamesItem[index];
                    label2.Text = ApiIDsItem[index];
                    textBox3.Text = ApiDecsItem[index];
                }
                else
                {
                    ckBox.Checked = false;
                    textBox1.Text = ApiId;
                    label2.Text = ApiId;
                    textBox3.Text = ApiDesc;
                }
                pl.Controls.Add(ckBox); pl.Controls.Add(textBox1); pl.Controls.Add(label2); pl.Controls.Add(textBox3);
                ctlList.Add(new Control[] { ckBox, textBox1, label2, textBox3 });
                CkBox_Click(ckBox, null);
            }
        }

        private void TextBox4_Click(object sender, EventArgs e)
        {
            Api_List cl = new Api_List();
            cl.ShowDialog();
            if (!cl.IsCancel)
            {
                ((TextBox)sender).Text = cl.SetVal;
            }
        }

        private void Label2_Click(object sender, EventArgs e)
        {
            int index = (int)((Label)sender).Tag;
            ((CheckBox)ctlList[index][0]).Checked = !((CheckBox)ctlList[index][0]).Checked;
            CkBox_Click((CheckBox)ctlList[index][0],null);
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                al[listView1.SelectedItems[0].Index] = Set2Array();
                InitCfgStr();
                listView1.SelectedItems[0].Text = (luyou.Text.Trim().Equals("") ? "GetInfo_" + (listView1.SelectedItems[0].Index + 1) : (luyou.Text.Trim()));
            }
        }
        private void CkBox_Click(object sender, EventArgs e)
        {
            int index = (int)((CheckBox)sender).Tag;
            ctlList[index][1].Enabled = ((CheckBox)sender).Checked;
            //ctlList[index][2].Enabled = ((CheckBox)sender).Checked;
            ctlList[index][3].Enabled = ((CheckBox)sender).Checked;
            TextBox_Leave(null, null);
        }

        private void test_Click(object sender, EventArgs e)
        {
            SQL sql = new SQL();
            sql.restr = "";
            sql.ShowDialog();
        }
        string[] Set2Array()
        {
            string apiNames = "";
            string apiIds = "";
            string apiDecs = "";
            foreach(Control[] ctrls in ctlList)
            {
                if(((CheckBox)ctrls[0]).Checked)
                {
                    apiNames += "[#]"+((TextBox)ctrls[1]).Text.Trim();
                    apiIds += "[#]" + ((Label)ctrls[2]).Text.Trim();
                    apiDecs += "[#]" + ((TextBox)ctrls[3]).Text.Trim();
                }
            }
            if (apiNames != "") apiNames = apiNames.Substring(3);
            if (apiIds != "") apiIds = apiIds.Substring(3);
            if (apiDecs != "") apiDecs = apiDecs.Substring(3);

            return new string[] { luyou.Text.Trim(),miaosu.Text.Trim(), apiNames, apiIds, apiDecs, inputForm.Checked ? "form" : "json" };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int curpos = listView1.SelectedItems[0].Index;
                if (curpos > 0)
                {
                    string[] o1 = (string[])al[curpos];
                    string[] o0 = (string[])al[curpos - 1];
                    al[curpos] = o0;
                    al[curpos-1] = o1;
                    Color c1 = listView1.Items[curpos - 1].ForeColor;
                    Color c2 = listView1.Items[curpos].ForeColor;
                    listView1.Items[curpos - 1].ForeColor = c2;
                    listView1.Items[curpos].ForeColor = c1;
                    listView1.Items[curpos - 1].Text = (o1[0].Trim().Equals("") ? "Add_" + curpos : (o1[0].Trim() )) ;
                    listView1.Items[curpos].Text = (o0[0].Trim().Equals("") ? "Add_" + (curpos + 1) : (o0[0].Trim())) ;
                    listView1.SelectedItems.Clear();
                    listView1.Items[curpos - 1].Selected=true;
                    InitCfgStr();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int curpos = listView1.SelectedItems[0].Index;
                if (curpos < al.Count-1)
                {
                    string[] o1 = (string[])al[curpos];
                    string[] o0 = (string[])al[curpos + 1];
                    al[curpos] = o0;
                    al[curpos + 1] = o1;
                    Color c1 = listView1.Items[curpos + 1].ForeColor;
                    Color c2 = listView1.Items[curpos].ForeColor;
                    listView1.Items[curpos + 1].ForeColor = c2;
                    listView1.Items[curpos].ForeColor = c1;
                    listView1.Items[curpos + 1].Text = (o1[0].Trim().Equals("") ? "Add_" + (curpos + 2) : (o1[0].Trim() )) ;
                    listView1.Items[curpos].Text = (o0[0].Trim().Equals("") ? "Add_" + (curpos + 1) : (o0[0].Trim() )) ;
                    listView1.SelectedItems.Clear();
                    listView1.Items[curpos + 1].Selected = true;
                    InitCfgStr();
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (FormData.TheFormData == null)
            {
                FormData.TheFormData = new FormData();
            }
            FormData.TheFormData.Show();
        }

        private void lianjie_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
        }

        private void downToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button4_Click(sender, e);
        }

        private void RowAll_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string MainTable = classes.PageAsist.GetPartSetValue(partId, "MainTable");
            string CusSQL = classes.PageAsist.GetPartSetValue(partId, "CusSQL");
            if (MainTable == null || CusSQL == null)
            {
                functions.MsgBox.Warning(res.ctl.str("DataOP_Api.2"));
            }
            else
            {
                if (MainTable.StartsWith("@")) MainTable = MainTable.Substring(1);
                else
                {
                    if(!string.IsNullOrWhiteSpace(MainTable))MainTable = "ft_" + consts.globalConst.CurSite.ID + "_f_" + MainTable;
                }
                control.RowAll_SetCols rs = new control.RowAll_SetCols();
                rs.MainTable = MainTable;
                rs.SelectSql = CusSQL;
                rs.ShowDialog();
                if (rs.SetString != null && rs.SetString != "")
                {
                    cfg.Text = rs.SetString;
                    setInitFromSetString(rs.SetString);
                }
            }
        }

        private void inputJson_Click(object sender, EventArgs e)
        {
            TextBox_Leave(sender, e);
        }

        private void inputForm_Click(object sender, EventArgs e)
        {
            TextBox_Leave(sender, e);
        }

        private void SelAllCheck_CheckedChanged(object sender, EventArgs e)
        {
            bool seled = SelAllCheck.Checked;
            foreach (Control[] ctls in ctlList)
            {
                var ck = (CheckBox)ctls[0];
                ck.Checked = seled;
                ctls[1].Enabled = seled;
                ctls[3].Enabled = seled;
            }
            TextBox_Leave(null, null);
        }

        private void miaosu_DoubleClick(object sender, EventArgs e)
        {
            TextEditor editor = new TextEditor();
            editor.basetext = miaosu.Text;
            editor.ShowDialog();
            if (!editor.cancel) miaosu.Text = editor.basetext;
        }
    }

       
}
