using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FTDPClient.consts;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using FTDPClient.forms.control;
using System.Linq;
using System.Text.RegularExpressions;
using FTDPClient.database;
using FTDPClient.functions;

namespace FTDPClient.forms
{
    public partial class TextEditor : Form
    {
        public static bool FormTextShow = false;
        public string basetext="";
        public bool cancel = true;
        public string HighlightingStrategy = "FTDP";
        public bool StrictField = false;
        public string fromWhere = "";
        public TextEditor()
        {
            InitializeComponent();
        }
        private void TextEditor_Load(object sender, EventArgs e)
        {
            FormTextShow = true;
        textEditorControl1.Text = basetext;
            textEditorControl1.Font = new Font("微软雅黑",13);
            textEditorControl1.Document.HighlightingStrategy =HighlightingStrategyFactory.CreateHighlightingStrategy(HighlightingStrategy);
            new FTDP.Util.ICSharpTextEditor().Init(this, textEditorControl1, StrictField, null);
            if(StrictField)
            {
                textEditorControl1.ActiveTextAreaControl.TextArea.MouseClick += ActiveTextAreaControl_MouseClick;
            }
            //textEditorControl1.ShowEOLMarkers = false;
            //textEditorControl1.ShowHRuler = false;
            //textEditorControl1.ShowInvalidLines = false;
            //textEditorControl1.ShowMatchingBracket = false;
            //textEditorControl1.ShowSpaces = false;
            //textEditorControl1.ShowTabs = false;
            //textEditorControl1.ShowVRuler = false;
            //textEditorControl1.IsIconBarVisible = false;
            button3.Text = res.com.str("TextEditor.button3");           //帮助文档
            button7.Text = res.com.str("TextEditor.button7");           //常用配置
            button4.Text = res.com.str("TextEditor.button4");           //程序集调用
            button5.Text = res.com.str("TextEditor.button5");           //脚本调用
            button6.Text = res.com.str("TextEditor.button6");			//API调用
            if(fromWhere=="")
            {
                textBox1.Visible = false;
                textEditorControl1.Location = new Point(2, 2);
                textEditorControl1.Width = 1225;
            }
            else if (fromWhere == "dataop")
            {
                textBox1.Visible = true;
                textEditorControl1.Location = new Point(402, 2);
                textEditorControl1.Width = 825;
                textBox1.Text= res.com.str("TextEditor.label.dataop");
                textBox1.Font = new Font(textBox1.Font.FontFamily, 12f);
            }
            else if (fromWhere == "filterrule")
            {
                textBox1.Visible = true;
                textEditorControl1.Location = new Point(402, 2);
                textEditorControl1.Width = 825;
                textBox1.Text = res.com.str("TextEditor.label.filterrule");
                textBox1.Font = new Font(textBox1.Font.FontFamily, 11f);
            }
            else if (fromWhere == "dyvalue")
            {
                textBox1.Visible = true;
                textEditorControl1.Location = new Point(402, 2);
                textEditorControl1.Width = 825;
                textBox1.Text = res.com.str("TextEditor.label.dyvalue");
                textBox1.Font = new Font(textBox1.Font.FontFamily, 12f);
            }
            comboBox2.Items.Clear();
            Regex r = new Regex(@"@para\{[^\}]*\}");
            var mc = r.Matches(basetext);
            List<string> mValue = new List<string>();
            foreach (Match m in mc)
            {
                string p = m.Value;
                if (p.IndexOf(',') > 0)
                {
                    p = p.Substring(0, p.IndexOf(',')).Trim() + "}";
                }
                if (mValue.Contains(p)) continue;
                mValue.Add(p);
            }
            mValue.ForEach(m => {
                comboBox2.Items.Add(m);
            });
            mValue.Clear();
            if (comboBox2.Items.Count > 0) comboBox2.SelectedIndex = 0;
            comboBox2.Visible = comboBox2.Items.Count > 0;

            checkBox1.Checked = basetext.StartsWith("@ForceComit#");
            checkBox2.Checked = basetext.StartsWith("@ForcePara#");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            basetext = textEditorControl1.Text; 
            cancel = false;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            net.GotoHelp();
            //JsHelper h = new JsHelper();
            //h.Show();
        }

        private void TextEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (button2.Focused) this.Close();
                else button2.Focus();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Code_List cl = new Code_List();
            cl.TopMost = this.TopMost;
            cl.ShowDialog();
            if(!cl.IsCancel)
            {
                if (cl.ReturnType == "List`1")
                { textEditorControl1.Clear(); textEditorControl1.Text = "@FromList:" + cl.SetVal; }
                else if (cl.ReturnType == "Dictionary`2")
                {
                    textEditorControl1.Clear(); textEditorControl1.Text = "@FromDic:" + cl.SetVal;
                }
                else
                {
                    Clipboard.SetText(cl.SetVal);
                    textEditorControl1.Focus();
                    SendKeys.Send("^(v)");
                    //textEditorControl1.Text += cl.SetVal;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Js_List cl = new Js_List();
            cl.TopMost = this.TopMost;
            cl.ShowDialog();
            if (!cl.IsCancel)
            {
                textEditorControl1.Text += cl.SetVal+";";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Api_List cl = new Api_List();
            cl.TopMost = this.TopMost;
            cl.ShowDialog();
            if (!cl.IsCancel)
            {
                if (cl.asDatasource.Checked)
                { textEditorControl1.Clear(); textEditorControl1.Text = "@FromApi:" + cl.SetVal; }
                else textEditorControl1.Text += cl.SetVal;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Common_List cl = new Common_List();
            cl.TopMost = this.TopMost;
            cl.ShowDialog();
            if (!cl.IsCancel)
            {
                textEditorControl1.Text += cl.SetVal ;
            }
        }
        char[] chars = new char[] { '\n', '\t', ' ', '\"', '\'', ',', '|', '{', '[', '(', '}', ']', ')', ';', ':', '/', '*', '=', '!', '>', '<' };

        private void ActiveTextAreaControl_MouseClick(object sender, EventArgs e)
        {
            toolTip.Hide(textEditorControl1.ActiveTextAreaControl.TextArea);
            var textArea = textEditorControl1.ActiveTextAreaControl;
            string lineStr = textArea.Document.GetText(textArea.Document.GetLineSegment(textArea.Caret.Position.Line)) + " ";
            string leftStr = lineStr.Substring(0, lineStr.IndexOfAny(chars, textArea.Caret.Position.X));
            string[] steItems = leftStr.Split(chars);
            string ptnstr = "";
            if (steItems.Length > 0) ptnstr = steItems[steItems.Length - 1];
            if (ptnstr.IndexOf('.') < 0)
            {
                var item = FTDP.Util.ICSharpTextEditor.completionData_Table.Where(r => r[0].ToString().Equals(ptnstr, StringComparison.CurrentCultureIgnoreCase));
                if (item.Count() > 0)
                {
                    toolTip.ToolTipTitle = " " + item.First()[1];
                    toolTip.Show(" " + ptnstr, textEditorControl1.ActiveTextAreaControl.TextArea);
                }
            }
            else
            {
                string aliasName = ptnstr.Split('.')[0];
                string colName = ptnstr.Split('.')[1];
                var dic = FTDP.Util.ICSharpTextEditor.TableAlias(textEditorControl1.Text);
                string tablename = null;
                if (dic.ContainsKey(aliasName)) tablename = dic[aliasName];
                if (tablename != null)
                {
                    var item = FTDP.Util.ICSharpTextEditor.completionData_Table.Where(r => r[0].ToString().Equals(tablename, StringComparison.CurrentCultureIgnoreCase));
                    if (item.Count() > 0)
                    {
                        string connstr = Options.GetSystemDBSetConnStr();
                        var dbtype = Options.GetSystemDBSetType();
                        var StrictFields = new List<object[]>();
                        try
                        {
                            StrictFields = FTDP.Util.ICSharpTextEditor.GetStrictFields(DBFunc.DBTypeString(dbtype), connstr, tablename);
                        }
                        catch { }
                        toolTip.ToolTipTitle = " " + item.First()[1] + " [" + tablename + "]";
                        var item2 = StrictFields.Where(r => r[0].ToString().Equals(colName, StringComparison.CurrentCultureIgnoreCase));
                        if (item2.Count() > 0)
                        {
                            toolTip.Show(" " + item2.First()[1] + " [" + colName + "]", textEditorControl1.ActiveTextAreaControl.TextArea);
                        }
                        else
                        {
                            toolTip.Show(" " + ("Not Exist ") + " [" + colName + "]", textEditorControl1.ActiveTextAreaControl.TextArea);
                        }
                    }
                }
            }
            return;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ftframe.com/doc/dataop.rtf");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ParaDev paraDev = new ParaDev();
            paraDev.initParaSel = comboBox2.Text;
            paraDev.TopMost = this.TopMost;
            paraDev.ShowDialog();
        }

        private void TextEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormTextShow = false;
    }

        private void button9_Click(object sender, EventArgs e)
        {
            var te = new SQL();
            te.restr = textEditorControl1.Text;
            te.TopMost = true;
            te.ShowDialog();
            if (!te.IsCancel)
            {
                textEditorControl1.Text = te.restr;
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked && !textEditorControl1.Text.StartsWith("@ForceComit#"))
            {
                textEditorControl1.Text = "@ForceComit#" + textEditorControl1.Text;
            }
            else if (!checkBox1.Checked && textEditorControl1.Text.StartsWith("@ForceComit#"))
            {
                textEditorControl1.Text = textEditorControl1.Text.Substring(12);
            }
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked && !textEditorControl1.Text.StartsWith("@ForcePara#"))
            {
                textEditorControl1.Text = "@ForcePara#" + textEditorControl1.Text;
            }
            else if (!checkBox2.Checked && textEditorControl1.Text.StartsWith("@ForcePara#"))
            {
                textEditorControl1.Text = textEditorControl1.Text.Substring(11);
            }
        }
    }
}