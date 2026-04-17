using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SiteMatrix.functions;
using SiteMatrix.consts;
using System.Data.OleDb;
namespace SiteMatrix.forms
{
    public partial class RulesEditor : Form
    {
        public string pageid = null; 
        public RulesEditor()
        {
            InitializeComponent();
            ApplyLanguage();

        }
        private void ApplyLanguage()
        {
            button1.Text = res.form.GetString("String33");
            button3.Text = res.form.GetString("String34");
            label1.Text = res.form.GetString("String35").Replace("\\r\\n","\r\n");
            this.rule.HeaderText = res.form.GetString("String36");
            this.alertinfo.HeaderText = res.form.GetString("String37");
        }
        private void RulesEditor_Load(object sender, EventArgs e)
        {
            string sql = "select rules,alertinfo from formrules where id='" + pageid + "'";
            OleDbDataReader rdr= globalConst.CurSite.SiteConn.OpenRecord(sql);
            while (rdr.Read())
            {
                dgv.Rows.Add(new string[] { rdr.GetString(0), rdr.GetString(1) });
            }
            rdr.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "delete from formrules where id='" + pageid + "'";
            globalConst.CurSite.SiteConn.execSql(sql);
            foreach (DataGridViewRow dgvr in dgv.Rows)
            {
                if (dgvr.Cells[0].Value == null && dgvr.Cells[1].Value == null) continue;
                sql = "insert into formrules(id,rules,alertinfo)values('" + pageid + "','" + (dgvr.Cells[0].Value == null ? "" : dgvr.Cells[0].Value.ToString().Replace("'", "''")) + "','" + (dgvr.Cells[1].Value == null ? "" : dgvr.Cells[1].Value.ToString().Replace("'", "''")) + "')";
                globalConst.CurSite.SiteConn.execSql(sql);
            }
            this.Close();
        }
    }
}