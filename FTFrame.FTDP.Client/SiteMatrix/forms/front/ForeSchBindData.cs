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
namespace FTDPClient.forms
{
    public partial class ForeSchBindData : Form
    {
        public List<string[]> InitData = null;
        public string[] ReturnVal = null;
        public ForeSchBindData()
        {
            InitializeComponent();
        }
        private void OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ForeConfig_Load(object sender, EventArgs e)
        {
            dgv.Columns[0].HeaderText = res.ft.str("FIInter.dgv.col.0");
            dgv.Columns[1].HeaderText = res.ft.str("FIInter.dgv.col.2");
            foreach (var item in InitData)
            {
                dgv.Rows.Add(item[0], item[1]);
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Rows[e.RowIndex].Cells[0].Value == null || dgv.Rows[e.RowIndex].Cells[1].Value == null) return;
            ReturnVal = new string[] {
            dgv.Rows[e.RowIndex].Cells[0].Value.ToString(),
            dgv.Rows[e.RowIndex].Cells[1].Value.ToString()
            };
            this.Close();
        }
    }
}