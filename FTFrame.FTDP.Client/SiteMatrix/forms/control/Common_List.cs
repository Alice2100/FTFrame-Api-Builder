using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using FTDPClient.functions;
using System.Data.OleDb;
using System.Xml;
using FTDPClient.consts;

namespace FTDPClient.forms.control
{
    public partial class Common_List : Form
    {
        public bool IsCancel = true;
        public string SetVal;
        public Common_List()
        {
            InitializeComponent();
        }

        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            this.Text = res.ctl.str("Common_Select.text");			//常用配置
            Editor ed = form.getEditor();
            string pageid = null;
            if(ed==null || ed.thisID==null)
            {
                label1.Text = res.ctl.str("Common_Select.1");
            }
            else
            {
                pageid = ed.thisID;
                label1.Text = ed.thisName+":"+ ed.thisTitle;
            }
            DataGridViewColumn ColModule = new DataGridViewTextBoxColumn();
            ColModule.Name = res.ctl.str("Common_Select.2");
            dataGridView1.Columns.Add(ColModule);
            ColModule.Width =120;

            DataGridViewColumn ColJsName= new DataGridViewTextBoxColumn();
            ColJsName.Name = res.ctl.str("Common_Select.3");
            dataGridView1.Columns.Add(ColJsName);
            ColJsName.Width = 200;

            DataGridViewColumn ColJs = new DataGridViewTextBoxColumn();
            ColJs.Name = res.ctl.str("Common_Select.4");
            dataGridView1.Columns.Add(ColJs);
            ColJs.Width = 350;

            DataGridViewColumn ColDec = new DataGridViewTextBoxColumn();
            ColDec.Name = res.ctl.str("Common_Select.5");
            dataGridView1.Columns.Add(ColDec);
            ColDec.Width = 350;

            XmlDocument doc = new XmlDocument();
            doc.Load(consts.globalConst.ConfigPath+ "\\commonset.xml");
            XmlNodeList nodes = doc.SelectNodes("sets/set");
            foreach(XmlNode node in nodes)
            {
                dataGridView1.Rows.Add(new string[] { 
                node.Attributes["module"].Value,
                 node.Attributes["name"].Value,
                 node.SelectSingleNode("code").InnerText,
                 node.SelectSingleNode("desc").InnerText
                });
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SetVal = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            IsCancel = false;
            this.Close();
        }
        private void SelCol_SqlServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
    }
}
