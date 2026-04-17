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
using System.Collections;
using FTDPClient.consts;
using System.Xml;

namespace FTDPClient.forms.control
{
    public partial class Part_QuickSet : Form
    {
        public string MainTable = null;
        public string SelectSql = null;
        public string SetString = null;
        public string OpType = "";
        public List<string[]> OpSetList = new List<string[]>();
        public ArrayList al = new ArrayList();
        public Part_QuickSet()
        {
            InitializeComponent();
        }
        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = res.ctl.str("PartQuick.Title");
                comboBoxControl.Items.Clear();
                comboBoxPart.Items.Clear();
                string sql = "select caption,name from controls order by caption";
                comboBoxControl.Items.Add(new Obj.ComboItem() { Id = "", Name = "Please Select" });
                using (var dr = globalConst.ConfigConn.OpenRecord(sql))
                {
                    while (dr.Read())
                    {
                        comboBoxControl.Items.Add(new Obj.ComboItem() { Id = dr.GetString(1), Name = dr.GetString(0) });
                    }
                }
                comboBoxControl.SelectedIndex= 0;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void dgvCheckState(bool check)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[0].Value = check;
                if (check)
                {
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.Cells[4].Style.BackColor = Color.AliceBlue;
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)//复选框
            {
                if ((bool)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue)
                {
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Pink;
                }
                else
                {
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }

        }
        private void SelCol_SqlServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ShowSave();
            MsgBox.Information("Saved!");
        }


        private void comboBoxControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgv.Rows.Clear();
            comboBoxPart.Items.Clear();
            if (comboBoxControl.SelectedIndex == 0)
            {
                return;
            }
            string sql = "select caption,name from parts where controlname='" + ((Obj.ComboItem)comboBoxControl.SelectedItem).Id + "' order by caption ";
            using (var dr = globalConst.ConfigConn.OpenRecord(sql))
            {
                while (dr.Read())
                {
                    comboBoxPart.Items.Add(new Obj.ComboItem() { Id = dr.GetString(1), Name = dr.GetString(0) });
                }
            }
            if (comboBoxPart.Items.Count > 0) comboBoxPart.SelectedIndex = 0;
        }
        private void ShowSet()
        {
            dgv.Rows.Clear();
            if (comboBoxPart.Items.Count == 0) return;
            string controlname = ((Obj.ComboItem)comboBoxControl.SelectedItem).Id;
            string partname = ((Obj.ComboItem)comboBoxPart.SelectedItem).Id;
            string sql = "select partxml from parts where controlname='" + controlname + "' and name='" + partname + "'";
            XmlDocument doc = new XmlDocument();
            List<(string setName, string rank)> cfgList = new List<(string setName, string rank)>();
            string cfgValue = Options.GetSystemValue("partquickset_" + controlname + "_" + partname);
            if (!string.IsNullOrWhiteSpace(cfgValue))
            {
                foreach (var item in cfgValue.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    cfgList.Add((item.Split('|')[0], item.Split('|')[1]));
                }
            }
            List<(string className, string setName, string cateName, string typeName, string caption, string[] enums)> setList = new List<(string className, string setName, string cateName, string typeName, string caption, string[] enums)>();
            using (var dr = globalConst.ConfigConn.OpenRecord(sql))
            {
                if (dr.Read())
                {
                    doc.LoadXml(dr.GetString(0));
                }
            }
            foreach (XmlNode node in doc.SelectNodes("//partxml/public_params/param"))
            {
                string className = node.SelectSingleNode("class")?.InnerText;
                string typeName = node.SelectSingleNode("type")?.InnerText;
                string setName = node.SelectSingleNode("name")?.InnerText;
                string category = node.SelectSingleNode("category")?.InnerText;
                string caption = node.SelectSingleNode("caption")?.InnerText;
                if (className == "enum")
                {
                    List<string> list = new List<string>();
                    foreach (XmlNode node2 in node.SelectNodes("enums/enum"))
                    {
                        list.Add(node2.InnerText);
                    }
                    setList.Add((className, setName, category, typeName, caption, list.ToArray()));
                }
                else if (className == "string" || className == "int")
                {
                    setList.Add((className, setName, category, typeName, caption, null));
                }
            }
            setList = setList.OrderBy(r => r.cateName).ThenBy(r => r.caption).ToList();
            foreach (var item in setList)
            {
                int index = dgv.Rows.Add(new object[] { false, item.cateName, item.caption });
                dgv.Rows[index].Tag = item;
                if (cfgList.Where(r => r.setName == item.setName).Count() > 0)
                {
                    dgv.Rows[index].Cells[0].Value = true;
                    dgv.Rows[index].Cells[3].Value = cfgList.Where(r => r.setName == item.setName).First().rank;
                    dgv.Rows[index].DefaultCellStyle.BackColor = Color.Pink;
                }
            }
        }
        private void ShowSave()
        {
            if (comboBoxPart.Items.Count == 0) return;
            string controlname = ((Obj.ComboItem)comboBoxControl.SelectedItem).Id;
            string partname = ((Obj.ComboItem)comboBoxPart.SelectedItem).Id;
            string setVal = "";
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Tag != null && ((bool)row.Cells[0].Value))
                {
                    var obj = ((string className, string setName, string cateName, string typeName, string caption, string[] enums))row.Tag;
                    setVal += obj.setName + "|" + (row.Cells[3].Value??"").ToString().Replace(";","") + ";";
                }
            }
            Options.SetSystemValue("partquickset_" + controlname + "_" + partname, setVal);
        }
        private void comboBoxPart_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSet();
        }
    }
}
