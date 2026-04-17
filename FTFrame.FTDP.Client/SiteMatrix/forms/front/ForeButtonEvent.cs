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
using ICSharpCode.TextEditor.Document;

namespace FTDPClient.forms
{
    public partial class ForeButtonEvent : Form
    {
        public string Click = null;
        public string JsCode = null;
        public bool IsOK = false;
        public DataGridViewRow Row = null;
        public int FromIndex = 0;//1 从列表，2从表单
        public List<(string type, string data)> schDefineData = null; 
        public ForeButtonEvent()
        {
            InitializeComponent();
            label1.Text = res.ft.str("FBE.label1");
            label3.Text = res.ft.str("FBE.label3");
            label2.Text = res.ft.str("FBE.label2");
            button1.Text = res.ft.str("FBE.button1");
            button2.Text = res.ft.str("FBE.button2");
        }
        private void OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (FromIndex == 1)
                {
                    if (comboBox1.SelectedIndex == 4)
                    {
                        Click = js_editor.Text.Trim();
                        JsCode = "";
                    }
                    else if (comboBox1.SelectedIndex == 0)
                    {
                        Click = "search()";
                        JsCode = js_editor.Text.Trim();
                    }
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        Click = "reset()";
                        JsCode = js_editor.Text.Trim();
                    }
                    else if (comboBox1.SelectedIndex == 2)
                    {
                        Click = js_editor.Text.Trim();
                        JsCode = "";
                    }
                    else if (comboBox1.SelectedIndex == 3)
                    {
                        Click = "excel(" + js_editor.Text.Trim() + ")";
                        JsCode = "";
                    }
                }
                else if (FromIndex == 2)
                {
                    if (comboBox1.SelectedIndex == 2)
                    {
                        Click = js_editor.Text.Trim();
                        JsCode = "";
                    }
                    else if (comboBox1.SelectedIndex == 0)
                    {
                        Click = "submit()";
                        JsCode = js_editor.Text.Trim();
                    }
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        Click = "reset()";
                        JsCode = js_editor.Text.Trim();
                    }
                }
                IsOK = true;
                Row.Cells[9].Value = Click;
                if (Row.Tag == null) Row.Tag = new object[] { "" };
                 ((object[])Row.Tag)[0] = JsCode;
                Row.DataGridView.EndEdit();
                this.Close();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string jsCode = "";
            bool schStrcitDefine = false;
            if (FromIndex == 1)
            {
                if (comboBox1.SelectedIndex == 4)
                {
                    jsCode = "";
                }
                else if (comboBox1.SelectedIndex == 0)
                {
                    jsCode += "config.schText=\"\";" + Environment.NewLine;
                    //jsCode += "config.schStrict=\"\";" + Environment.NewLine;
                    schStrcitDefine = true;
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    jsCode += "config.orderBy=\"\";" + Environment.NewLine;
                    jsCode += "config.orderType=\"\";" + Environment.NewLine;
                    jsCode += "config.schText=\"\";" + Environment.NewLine;
                    jsCode += "config.schStrict=\"\";" + Environment.NewLine;
                    jsCode += "config.pageNum=1;" + Environment.NewLine;
                    jsCode += "vm.$set(vm.page,'count',config.pageNum);" + Environment.NewLine;
                }
                else if (comboBox1.SelectedIndex == 2)
                {
                    jsCode = "batch(keyName, updateName, updateVal, paras)";
                }
                else if (comboBox1.SelectedIndex == 3)
                {
                    jsCode = "0";
                }
            }
            else if (FromIndex == 2)
            {
                if (comboBox1.SelectedIndex == 2)
                {
                    jsCode = "";
                }
                else if (comboBox1.SelectedIndex == 0)
                {
                    jsCode = "";
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    jsCode = "";
                }
            }
            js_editor.ResetText();
            js_editor.Text="";
            js_editor.Refresh();
            js_editor.Text = jsCode;
            ShowHiddenSchBtn();
            if(schStrcitDefine)
            {
                button1_Click(sender, e);
            }
        }
        void ShowHiddenSchBtn()
        {
            bool vi = comboBox1.Text == res.ft.str("FBE.001");
            button1.Visible = vi;
            button2.Visible = vi;
        }
        private void ForeButtonEvent_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            if(FromIndex==1)
            {
                comboBox1.Items.AddRange(new string[] { res.ft.str("FBE.001"), res.ft.str("FBE.002"), res.ft.str("FBE.003"), res.ft.str("FBE.004"), res.ft.str("FBE.005") });
                if (Click.StartsWith("search("))
                {
                    comboBox1.SelectedIndex = 0;
                    js_editor.Text = JsCode;
                }
                else if (Click.StartsWith("reset("))
                {
                    comboBox1.SelectedIndex = 1;
                    js_editor.Text = JsCode;
                }
                else if (Click.StartsWith("batch("))
                {
                    comboBox1.SelectedIndex = 2;
                    js_editor.Text = Click;
                }
                else if (Click.StartsWith("excel("))
                {
                    comboBox1.SelectedIndex = 3;
                    js_editor.Text = Click.Replace("excel(", "").Replace(")", "");
                }
                else
                {
                    comboBox1.SelectedIndex = 4;
                    js_editor.Text = Click;
                }
            }
            else if (FromIndex == 2)
            {
                comboBox1.Items.AddRange(new string[] { res.ft.str("FBE.006"), res.ft.str("FBE.002"), res.ft.str("FBE.005") });
                if (Click.StartsWith("submit("))
                {
                    comboBox1.SelectedIndex = 0;
                    js_editor.Text = JsCode;
                }
                else if (Click.StartsWith("reset("))
                {
                    comboBox1.SelectedIndex = 1;
                    js_editor.Text = JsCode;
                }
                else 
                {
                    comboBox1.SelectedIndex = 2;
                    js_editor.Text = Click;
                }
            }
            ShowHiddenSchBtn();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HTMLText hTMLText = new HTMLText();
            hTMLText.SetVal = res.ft.str("FBE.007");
            hTMLText.Text = res.ft.str("FBE.008");
            hTMLText.Strategy = "BAT";
            hTMLText.TopMost = true;
            hTMLText.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("config.schStrict=\"\"");
            foreach(var item in schDefineData)
            {
                if(!string.IsNullOrWhiteSpace(item.data))
                {
                    var ss = item.data.Split('.');
                    if (item.type == "input")
                    {
                        sb.AppendLine("\t\t+ \";" + ss[ss.Length - 1] + ":%\" + " + item.data + " + \"%\" ");
                    }
                    else if (item.type == "select")
                    {
                        sb.AppendLine("\t\t+ \";" + ss[ss.Length - 1] + ":\" + " + item.data+ ".selValue");
                    }
                    else if (item.type == "date")
                    {
                        sb.AppendLine("\t\t+ \";" + ss[ss.Length - 1] + ":\" + " + "(("+ item.data + "===''||"+ item.data + "==null)?'':('>='+"+ item.data + "))");
                    }
                    else if (item.type == "dater")
                    {
                        sb.AppendLine("\t\t+ \";" + ss[ss.Length - 1] + ":\" + " + "((" + item.data + "===''||" + item.data + "==null)?'':('>='+" + item.data + "[0]))");
                        sb.AppendLine("\t\t+ \";" + ss[ss.Length - 1] + ":\" + " + "((" + item.data + "===''||" + item.data + "==null)?'':('<='+" + item.data + "[1]))");
                    }
                }
            }
            sb.AppendLine(";");
            js_editor.Append((js_editor.Text.EndsWith("\r\n")?"":"\r\n") + sb.ToString());
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}