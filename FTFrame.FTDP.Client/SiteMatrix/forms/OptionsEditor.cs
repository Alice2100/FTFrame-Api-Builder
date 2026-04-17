using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FTDPClient.functions;
using FTDPClient.consts;
using System.Data.OleDb;
using mshtml;
using System.Collections;
namespace FTDPClient.forms
{
    public partial class OptionsEditor : Form
    {
        public ArrayList options = null;
        public bool cancel = true;
        public OptionsEditor()
        {
            InitializeComponent();
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            button1.Text = res.form.GetString("String33");
            button3.Text = res.form.GetString("String34");
            label1.Text = res.form.GetString("String58");
            this.optiontext.HeaderText = res.form.GetString("String55");
            this.optionvalue.HeaderText = res.form.GetString("String56");
            this.optioncheck.HeaderText = res.form.GetString("String57");
        }
        private void RulesEditor_Load(object sender, EventArgs e)
        {
            foreach (object option in options)
            {
                string[] items = (string[])option;
                dgv.Rows.Add(new object[] { items[0], items[1], int.Parse(items[2])==1?true:false});
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ovalue = "";
            foreach (DataGridViewRow dgvr in dgv.Rows)
            {
                if (dgvr.Cells[0].Value == null && dgvr.Cells[1].Value == null) continue;
                if (dgvr.Cells[0].Value != null && (dgvr.Cells[1].Value == null || dgvr.Cells[1].Value.ToString().Trim().Equals("")))
                {
                    MsgBox.Warning(res.form.GetString("String59"));
                    return;
                }
                if (dgvr.Cells[1].Value.ToString().Trim().Equals(ovalue))
                {
                    MsgBox.Warning(res.form.GetString("String59"));
                    return;
                }
                ovalue = dgvr.Cells[1].Value.ToString().Trim();
            }
            options = new ArrayList();
            foreach (DataGridViewRow dgvr in dgv.Rows)
            {
                if (dgvr.Cells[0].Value == null && dgvr.Cells[1].Value == null) continue;
                object option = new string[] { dgvr.Cells[0].Value.ToString().Trim(), dgvr.Cells[1].Value.ToString().Trim(), bool.Parse(((DataGridViewCheckBoxCell)dgvr.Cells[2]).Value==null?"False":((DataGridViewCheckBoxCell)dgvr.Cells[2]).Value.ToString()) ? "1" : "0" };
                options.Add(option);
            }
            cancel = false;
            this.Close();
        }

    }
}