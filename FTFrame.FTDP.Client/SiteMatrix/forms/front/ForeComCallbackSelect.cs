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
    public partial class ForeComCallbackSelect : Form
    {
        public string CallBack = null;
        public string CallBackDesc = null;
        public string ComName = null;
        public bool IsCancel = true;
        public ForeComCallbackSelect()
        {
            InitializeComponent();
        }
        private void OK_Click(object sender, EventArgs e)
        {
            IsCancel = false;
            CallBack = (comboBox1.SelectedItem as Obj.ComboItem).Id;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ForeConfig_Load(object sender, EventArgs e)
        {
            string connstr = Options.GetSystemDBSetConnStr_Plat();
            var conntype = Options.GetSystemDBSetType_Plat();
            bool IsList = false;
            string CustomJs = null;
            string sql = "select CustomJs from ft_ftdp_front_list where IsNewest=1 and ComName='"+ str.D2DD(ComName) + "'";
            var dt=Adv.RemoteSqlQuery(sql,conntype,connstr);
            if(dt.Rows.Count>0)
            {
                CustomJs = dt.Rows[0][0].ToString()+Environment.NewLine;
                IsList = true;
            }
            else
            {
                sql = "select CustomJs from ft_ftdp_front_form where IsNewest=1 and ComName='" + str.D2DD(ComName) + "'";
                dt = Adv.RemoteSqlQuery(sql, conntype, connstr);
                if (dt.Rows.Count > 0)
                {
                    CustomJs = dt.Rows[0][0].ToString() + Environment.NewLine;
                    IsList = false;
                }
            }
            if(CustomJs==null)
            {
                MsgBox.Warning(res.ft.str("FCCS.001"));
                this.Close();
                return;
            }
            if(IsList)
            {
                comboBox1.Items.Add(new Obj.ComboItem() { Id="", Name= res.ft.str("FCCS.002") });
                var doc = new Front.Generator().DocFromCustomJs(CustomJs,null,null);
                foreach (var item in doc.EmitList)
                {
                    comboBox1.Items.Add(new Obj.ComboItem() { Id = item.Emit, Name = $"{item.Emit} : {item.Desc}" });
                }
                comboBox1.Items.Add(new Obj.ComboItem() { Id= "@beforeLoad", Name= res.ft.str("FCCS.003") });
                comboBox1.Items.Add(new Obj.ComboItem() { Id= "@beforeSet", Name= res.ft.str("FCCS.004") });
                comboBox1.Items.Add(new Obj.ComboItem() { Id= "@afterLoad", Name= res.ft.str("FCCS.005") });
                comboBox1.SelectedIndex = 0;
                for (int i=0;i< comboBox1.Items.Count;i++)
                {
                    if (CallBackDesc.StartsWith((comboBox1.Items[i] as Obj.ComboItem).Id + ":"))
                    {
                        comboBox1.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                comboBox1.Items.Add(new Obj.ComboItem() { Id = "", Name = res.ft.str("FCCS.002")  });
                var doc = new Front.Generator().DocFromCustomJs(CustomJs, null, null);
                foreach (var item in doc.EmitList)
                {
                    comboBox1.Items.Add(new Obj.ComboItem() { Id = item.Emit, Name = $"{item.Emit} : {item.Desc}" });
                }
                comboBox1.Items.Add(new Obj.ComboItem() { Id = "@beforeGet", Name = res.ft.str("FCCS.006") });
                comboBox1.Items.Add(new Obj.ComboItem() { Id = "@beforeSet", Name = res.ft.str("FCCS.007") });
                comboBox1.Items.Add(new Obj.ComboItem() { Id = "@afterSet", Name = res.ft.str("FCCS.008") });
                comboBox1.Items.Add(new Obj.ComboItem() { Id = "@beforeSubmit", Name = res.ft.str("FCCS.009") });
                comboBox1.Items.Add(new Obj.ComboItem() { Id = "@afterSubmit", Name = res.ft.str("FCCS.010") });
                comboBox1.SelectedIndex = 0;
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    if (CallBackDesc.StartsWith((comboBox1.Items[i] as Obj.ComboItem).Id + ":"))
                    {
                        comboBox1.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
    }
}