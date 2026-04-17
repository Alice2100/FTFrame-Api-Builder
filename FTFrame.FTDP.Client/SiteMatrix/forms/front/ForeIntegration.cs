using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using FTDPClient.consts;
using FTDPClient.functions;

namespace FTDPClient.forms
{
    public partial class ForeIntegration : Form
    {
        public string SetString = "";
        public bool IsCancel = true;
        public ForeIntegration()
        {
            InitializeComponent();
        }
        private void OK_Click(object sender, EventArgs e)
        {
            SetString = ((Obj.ComboItem)comboBox1.SelectedItem).Id;
            if (SetString != "")
            {
                var paraStr = "";
                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    paraStr += textBox1.Text.Trim();
                }
                if (!string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    if (paraStr != "") paraStr += "|";
                    paraStr += textBox2.Text.Trim();
                }
                if (!string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    if (paraStr != "") paraStr += "|";
                    paraStr += textBox3.Text.Trim();
                }
                if (!string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    if (paraStr != "") paraStr += "|";
                    paraStr += textBox4.Text.Trim();
                }
                if (!string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    if (paraStr != "") paraStr += "|";
                    paraStr += textBox5.Text.Trim();
                }
                if (!string.IsNullOrWhiteSpace(paraStr))
                {
                    SetString = SetString.Replace("$(para)", "," + paraStr);
                }
                else SetString = SetString.Replace("$(para)", "");
                SetString = "" + ((Obj.ComboItem)comboBox1.SelectedItem).Tag.ToString().Replace(Environment.NewLine, " ") + Environment.NewLine + SetString;
            }
            IsCancel = false;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ForeConfig_Load(object sender, EventArgs e)
        {
            label2.Text = res.ft.str("FIInter.label2");
            comboBox1.Items.Clear();
            comboBox1.Items.Add(new Obj.ComboItem()
            {
                Id = "",
                Name = "( Please Select )",
                Tag=""
            });
            string sql = "select paracaption,paraname,mimo from ft_ftdp_para where stat=1 and paraname like '@Front_%' order by paraname";
            var dt=Adv.RemoteSqlQuery(sql);
            foreach(DataRow dr in dt.Rows)
            {
                Obj.ComboItem item = new Obj.ComboItem() { 
                  Id = "@para{"+ dr[1].ToString()+"$(para)}",
                  Name = dr[1].ToString()+" "+dr[0].ToString(),
                  Tag= dr[0].ToString() + Environment.NewLine + Environment.NewLine + dr[2].ToString(),
                };
                comboBox1.Items.Add(item);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label3.Text = ((Obj.ComboItem)comboBox1.SelectedItem).Tag.ToString();
            //SetString = ((Obj.ComboItem)comboBox1.SelectedItem).Id;
            //if (SetString != "")
            //{
            //    if(!string.IsNullOrWhiteSpace(textBox1.Text))
            //    {
            //        SetString = SetString.Replace("$(para)",","+ textBox1.Text.Trim());
            //    }
            //    IsCancel = false;
            //    this.Close();
            //}
        }
    }
}