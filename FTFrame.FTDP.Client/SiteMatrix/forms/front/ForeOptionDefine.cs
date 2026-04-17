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
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FTDPClient.forms
{
    public partial class ForeOptionDefine : Form
    {
        public string InitData = "";
        public string ReturnVal = "";
        public bool IsCancel=false;
        public ForeOptionDefine()
        {
            InitializeComponent();
        }
        private void OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            IsCancel = true;
            this.Close();
        }

        private void ForeConfig_Load(object sender, EventArgs e)
        {
            dgv.Columns[0].HeaderText = res.ft.str("FIInter.dgv.col.0");
            dgv.Columns[1].HeaderText = res.ft.str("FIInter.dgv.col.1");
            if (InitData == "") return;
            try
            {
                var ja=JArray.Parse("["+InitData+"]");
                foreach(JObject jo in ja)
                {
                    string val = jo["value"].ToString();
                    string label = jo["label"].ToString();
                    dgv.Rows.Add(label,val);
                }
            }
            catch(Exception ex)
            {
                MsgBox.Error(ex.Message);
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach(DataGridViewRow row in dgv.Rows)
            {
                string val = (row.Cells[1].Value?.ToString() ?? "");
                string label = (row.Cells[0].Value?.ToString() ?? "");
                if (val=="" && label=="") continue;
                s += ",{value:'"+str.D2DD(val) + "',label:'" + str.D2DD(label) + "'}";
            }
            if (s != "") s = s.Substring(1);
            ReturnVal = s;
            this.Close();
        }
    }
}