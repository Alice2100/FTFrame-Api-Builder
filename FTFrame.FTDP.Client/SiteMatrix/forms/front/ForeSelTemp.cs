using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using FTDPClient.consts;
using FTDPClient.functions;
using ICSharpCode.TextEditor.Document;
using MySql.Data.MySqlClient;

namespace FTDPClient.forms
{
    public partial class ForeSelTemp : Form
    {
        public bool IsOK = false;
        public int TempId = 0;
        public ForeSelTemp()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ForeConfig_Load(object sender, EventArgs e)
        {
            dgv.Columns[0].HeaderText= res.front.str("TempSelTemp.dgv.0");
            dgv.Columns[1].HeaderText= res.front.str("TempSelTemp.dgv.1");
            dgv.Columns[2].HeaderText= res.front.str("TempSelTemp.dgv.2");
            InitGrid();
        }
        void InitGrid()
        {
            dgv.Rows.Clear();
            string connstr = Options.GetSystemDBSetConnStr_Plat();
            var conntype = Options.GetSystemDBSetType_Plat();
            string sql = "select Id,Caption,TempDesc from ft_ftdp_front_temp order by Caption";
            var dt = Adv.RemoteSqlQuery(sql, conntype, connstr);
            foreach(DataRow dr in dt.Rows)
            {
                int index=dgv.Rows.Add(new string[] { dr["Caption"].ToString(), dr["TempDesc"].ToString(), res.front.str("TempSelTemp.001") });
                dgv.Rows[index].Tag = dr["Id"];
            }
        }
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==2)
            {
                TempId = int.Parse(dgv.Rows[e.RowIndex].Tag.ToString());
                IsOK = true;
                this.Close();
            }
        }

        private void ForeBackUp_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}