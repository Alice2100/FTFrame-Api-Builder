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
    public partial class ForeComSelect : Form
    {
        public string component = null;
        public bool IsOK = false;
        public ForeComSelect()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ForeConfig_Load(object sender, EventArgs e)
        {
            dgv.Columns[0].HeaderText = res.front.str("TempPageBackUp.dgv.0");
            dgv.Columns[1].HeaderText = res.front.str("TempPageBackUp.dgv.1");
            dgv.Columns[2].HeaderText = res.front.str("TempPageBackUp.dgv.2");
            dgv.Columns[3].HeaderText = res.front.str("TempPageBackUp.dgv.3");
            dgv.Columns[4].HeaderText = res.front.str("TempPageBackUp.dgv.4");
            InitGrid();
        }
        void InitGrid()
        {
            dgv.Rows.Clear();
            string connstr = Options.GetSystemDBSetConnStr_Plat();
            var conntype = Options.GetSystemDBSetType_Plat();
            string sql = "select ComName,Caption,CreateTime,Developer from ft_ftdp_front_list where IsNewest=1";
            sql += " UNION all";
            sql += " select ComName,Caption,CreateTime,Developer from ft_ftdp_front_form where IsNewest=1 order by ComName";
            var dt=Adv.RemoteSqlQuery(sql,conntype,connstr);
            foreach(DataRow dr in dt.Rows)
            {
                dgv.Rows.Add(dr["ComName"].ToString(), dr["Caption"].ToString(), (dr["CreateTime"] as DateTime?).Value.ToString("yyyy-MM-dd HH:mm:ss"), dr["Developer"].ToString(), res.front.str("TempSelTemp.001"));
            }
        }
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==4)
            {
                component = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                IsOK = true;
                this.Close();
            }
        }

        private void ForeBackUp_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}