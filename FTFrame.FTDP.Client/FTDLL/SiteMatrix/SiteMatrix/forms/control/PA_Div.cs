using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using mshtml;
using SiteMatrix.consts;
using SiteMatrix.database;
using SiteMatrix.functions;
namespace SiteMatrix.forms
{
    public partial class PA_Div : Form
    {
        public bool IsCopy = false;
        public bool IsAddNew = false;
        public bool IsCancel = true;
        public string oriId = null;
        public  IHTMLElement DivEle = null;
        public PA_Div()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EleAssis_Load(object sender, EventArgs e)
        {
            if(DivEle!=null) oriId = DivEle.id;
            listBox1.Items.Clear();
            string sql = "select id,caption from snippets order by id";
            DR dr = new DR(globalConst.ConfigConn.OpenRecord(sql));
            while (dr.Read())
            {
                if(dr.getString("caption").StartsWith("[DIV]"))
                {
                    listBox1.Items.Add(dr.getInt32("id") + ":" + dr.getString("caption").Substring(5));
                }
            }
            dr.Close();
        }
        public static void EleSelected(IHTMLElement ele)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void PA_Div_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
            {
                this.Close();
            }
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            Editor ed = form.getEditor();
            if (ed == null) return;
            string newid = textBox1.Text.Trim();
            string name = textBox2.Text.Trim();
            if(newid=="")
            {
                MsgBox.Warning("必须输入ID");
                return;
            }

            string content = null;
            if(checkBox1.Checked)
            {
                if(listBox1.SelectedItem==null)
                {
                    MsgBox.Warning("必须选择一个模板");
                    return;
                }
                int mubanID = int.Parse(listBox1.SelectedItem.ToString().Split(':')[0]);
                string sql = "select content from snippets where id="+ mubanID;
                DR dr = new DR(globalConst.ConfigConn.OpenRecord(sql));
                dr.Read();
                content = dr.getString(0);
                dr.Close();
            }
            if(IsAddNew)
            {
                IHTMLElement oEle = ed.editocx.getElementById(newid);
                if (oEle != null)
                {
                    MsgBox.Warning("ID为 " + newid + " 的HTML元素已经存在！");
                    return;
                }
                string newhtml = "";
                string divHTML = "<div class=\"ftdiv\" id=\""+ newid + "\">";
                if (content != null)
                {
                    string innerhtml = content;
                    if (RcheckBox1.Checked)
                    {
                        string ori = R1textBox1.Text.Trim();
                        string tar = R2textBox2.Text.Trim();
                        Regex r = new Regex("(((?i)id|(?i)name)=(|\'|\")[\\w]*" + ori + ")");
                        MatchCollection mc = r.Matches(innerhtml);
                        foreach (Match m in mc)
                        {
                            innerhtml = innerhtml.Replace(m.Value, m.Value.Replace(ori, tar));
                        }
                    }
                    divHTML += innerhtml;
                }
                else
                {
                    divHTML += "空内容";
                }
                divHTML += "</div>";
                string headHTML = "<SPAN id="+ newid + "_ftheader>Head Start：【<FONT id=" + newid + "_ftheader1 color=blue>"+ name + "</FONT>&nbsp;&nbsp;&nbsp;&nbsp;<FONT id=" + newid + "_ftheader2 color=red>ID:&nbsp;" + newid + "</FONT>&nbsp;&nbsp;&nbsp;&nbsp;Added：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"】Head End</SPAN>";
                newhtml = headHTML + divHTML;
                IHTMLElement red_hdn_eld=ed.editocx.getElementById("ft_red_hdn_div");
                if(red_hdn_eld==null)
                {
                    ed.editocx.body.insertAdjacentHTML("beforeEnd", "<div id='ft_red_hdn_div' style=\"border-top: red 1px solid; border-right: red 1px solid; border-bottom: red 1px solid; border-left: red 1px solid; display: none\">" + newhtml + "</div>");
                }
                else
                {
                    red_hdn_eld.insertAdjacentHTML("beforeEnd",  newhtml);
                }
                IsCancel = false;
                this.Close();
                return;
            }
            if (content != null)
            {
                if (!IsCopy)
                {
                    if (newid != oriId)
                    {
                        IHTMLElement oEle = ed.editocx.getElementById(newid);
                        if (oEle != null)
                        {
                            MsgBox.Warning("ID为 " + newid + " 的HTML元素已经存在！");
                            return;
                        }
                        DivEle.id = newid;
                    }
                    string innerhtml = content;
                    if (RcheckBox1.Checked)
                    {
                        string ori = R1textBox1.Text.Trim();
                        string tar = R2textBox2.Text.Trim();
                        Regex r = new Regex("(((?i)id|(?i)name)=(|\'|\")[\\w]*" + ori + ")");
                        MatchCollection mc = r.Matches(innerhtml);
                        foreach (Match m in mc)
                        {
                            innerhtml = innerhtml.Replace(m.Value, m.Value.Replace(ori, tar));
                        }
                    }
                    DivEle.innerHTML = innerhtml;
                    IHTMLElement nameEle = ed.editocx.getElementById(oriId + "_ftheader1");
                    if (nameEle != null)
                    {
                        nameEle.id = newid + "_ftheader1";
                        nameEle.innerText = name;
                    }
                    IHTMLElement idEle = ed.editocx.getElementById(oriId + "_ftheader2");
                    if (idEle != null)
                    {
                        idEle.id = newid + "_ftheader2";
                        idEle.innerHTML = "ID:&nbsp;" + newid;
                    }
                    IHTMLElement spanEle = ed.editocx.getElementById(oriId + "_ftheader");
                    if (spanEle != null)
                    {
                        spanEle.id = newid + "_ftheader";
                    }
                }
                else
                {
                    IHTMLElement oEle = ed.editocx.getElementById(newid);
                    if (oEle != null)
                    {
                        MsgBox.Warning("ID为 " + newid + " 的HTML元素已经存在！");
                        return;
                    }
                    string innerhtml = content;
                    if (RcheckBox1.Checked)
                    {
                        string ori = R1textBox1.Text.Trim();
                        string tar = R2textBox2.Text.Trim();
                        Regex r = new Regex("(((?i)id|(?i)name)=(|\'|\")[\\w]*" + ori + ")");
                        MatchCollection mc = r.Matches(innerhtml);
                        foreach (Match m in mc)
                        {
                            innerhtml = innerhtml.Replace(m.Value, m.Value.Replace(ori, tar));
                        }
                    }
                    string html = "<DIV class=\"ftdiv\" id=\"" + newid + "\">" + innerhtml + "</DIV>";
                    string head = "<SPAN  id=" + newid + "_ftheader>Head Start：【<FONT color=blue id=" + newid + "_ftheader1>" + name + "</FONT>&nbsp;&nbsp;&nbsp;&nbsp;<FONT color=red id=" + newid + "_ftheader2>ID:&nbsp;" + newid + "</FONT>&nbsp;&nbsp;&nbsp;&nbsp;Added：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】Head End</span>";
                    DivEle.insertAdjacentHTML("afterEnd", head + "\r\n" + html);
                }
            }
            else
            {
                if (!IsCopy)
                {
                    if (newid != oriId)
                    {
                        IHTMLElement oEle = ed.editocx.getElementById(newid);
                        if (oEle != null)
                        {
                            MsgBox.Warning("ID为 " + newid + " 的HTML元素已经存在！");
                            return;
                        }
                        DivEle.id = newid;
                    }
                    if (RcheckBox1.Checked)
                    {
                        string html = DivEle.outerHTML;
                        string ori = R1textBox1.Text.Trim();
                        string tar = R2textBox2.Text.Trim();
                        Regex r = new Regex("(((?i)id|(?i)name)=(|\'|\")[\\w]*" + ori + ")");
                        MatchCollection mc = r.Matches(html);
                        foreach (Match m in mc)
                        {
                            html = html.Replace(m.Value, m.Value.Replace(ori, tar));
                        }
                        DivEle.outerHTML = html;
                    }
                    IHTMLElement nameEle = ed.editocx.getElementById(oriId + "_ftheader1");
                    if (nameEle != null)
                    {
                        nameEle.id = newid + "_ftheader1";
                        nameEle.innerText = name;
                    }
                    IHTMLElement idEle = ed.editocx.getElementById(oriId + "_ftheader2");
                    if (idEle != null)
                    {
                        idEle.id = newid + "_ftheader2";
                        idEle.innerHTML = "ID:&nbsp;" + newid;
                    }
                    IHTMLElement spanEle = ed.editocx.getElementById(oriId + "_ftheader");
                    if (spanEle != null)
                    {
                        spanEle.id = newid + "_ftheader";
                    }
                }
                else
                {
                    IHTMLElement oEle = ed.editocx.getElementById(newid);
                    if (oEle != null)
                    {
                        MsgBox.Warning("ID为 " + newid + " 的HTML元素已经存在！");
                        return;
                    }
                    DivEle.id = newid;
                    string html = DivEle.outerHTML;
                    DivEle.id = oriId;
                    if (RcheckBox1.Checked)
                    {
                        string ori = R1textBox1.Text.Trim();
                        string tar = R2textBox2.Text.Trim();
                        Regex r = new Regex("(((?i)id|(?i)name)=(|\'|\")[\\w]*" + ori + ")");
                        MatchCollection mc = r.Matches(html);
                        foreach (Match m in mc)
                        {
                            html = html.Replace(m.Value, m.Value.Replace(ori, tar));
                        }
                    }
                    string head = "<SPAN  id=" + newid + "_ftheader>Head Start：【<FONT color=blue id=" + newid + "_ftheader1>" + name + "</FONT>&nbsp;&nbsp;&nbsp;&nbsp;<FONT color=red id=" + newid + "_ftheader2>ID:&nbsp;" + newid + "</FONT>&nbsp;&nbsp;&nbsp;&nbsp;Added：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】Head End</span>";
                    DivEle.insertAdjacentHTML("afterEnd", head + "\r\n" + html);
                }
            }
            IsCancel = false;
            this.Close();
        }

        private void CheckBox1_Click(object sender, EventArgs e)
        {
            listBox1.Enabled = checkBox1.Checked;
        }
    }
}
