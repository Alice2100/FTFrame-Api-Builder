using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FTDPClient.consts;
using FTDPClient.database;
using FTDPClient.functions;
using mshtml;

namespace FTDPClient.forms
{
    public partial class EleGenarate : Form
    {
        public bool isCancel = true;
        public string layoutHTML = "";
        public EleGenarate()
        {
            InitializeComponent();
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            //this.Text = res.InputName.GetString("_this");
            OK.Text = res.InputName.GetString("OK");
            Cancel.Text = res.InputName.GetString("Cancel");
            label1.Text = res.InputName.GetString("label1");
            label2.Text = res.InputName.GetString("label2");
            label3.Text = res.InputName.GetString("label3");
            label4.Text = res.InputName.GetString("label4");
            comboBox1.Items.AddRange(new object[] {
            res.TagEdit.GetString("item1"),
            res.TagEdit.GetString("item2"),
            res.TagEdit.GetString("item3"),
            res.TagEdit.GetString("item4"),
            res.TagEdit.GetString("item5"),
            res.TagEdit.GetString("item6"),
            res.TagEdit.GetString("item7"),});
            comboBox1.SelectedIndex = 0;
        }
        public static void FormGenarate(string template, int eleType, string partid, List<(string idname, string caption)> idCaps,string divId)
        {
            try
            {
                var editor = form.getEditor();
                if (editor == null) return;
                StringBuilder sb = new StringBuilder();
                if (template.IndexOf("{NAME}") >= 0)//2ап
                {
                    foreach (var item in idCaps)
                    {
                        sb.Append(template.Replace("{NAME}", item.caption).Replace("{ELEMENT}", getEleHtml(eleType, item.idname)));
                    }
                }
                else if (template.IndexOf("{NAME2}") >= 0 && template.IndexOf("{NAME3}") < 0)//4ап
                {
                    for (int i = 0; i < idCaps.Count; i++)
                    {
                        string cap1 = idCaps[i].caption;
                        string id1 = idCaps[i].idname;
                        string cap2 = "";
                        string id2 = "";
                        if (i + 1 < idCaps.Count)
                        {
                            i++;
                            cap2 = idCaps[i].caption;
                            id2 = idCaps[i].idname;
                        }
                        sb.Append(template.Replace("{NAME1}", cap1).Replace("{ELEMENT1}", getEleHtml(eleType, id1)).Replace("{NAME2}", cap2).Replace("{ELEMENT2}", getEleHtml(eleType, id2)));
                    }
                }
                else if (template.IndexOf("{NAME3}") >= 0 && template.IndexOf("{NAME4}") < 0)//6ап
                {
                    for (int i = 0; i < idCaps.Count; i++)
                    {
                        string cap1 = idCaps[i].caption;
                        string id1 = idCaps[i].idname;
                        string cap2 = "";
                        string id2 = "";
                        if (i + 1 < idCaps.Count)
                        {
                            i++;
                            cap2 = idCaps[i].caption;
                            id2 = idCaps[i].idname;
                        }
                        string cap3 = "";
                        string id3 = "";
                        if (i + 1 < idCaps.Count)
                        {
                            i++;
                            cap3 = idCaps[i].caption;
                            id3 = idCaps[i].idname;
                        }
                        sb.Append(template.Replace("{NAME1}", cap1).Replace("{ELEMENT1}", getEleHtml(eleType, id1)).Replace("{NAME2}", cap2).Replace("{ELEMENT2}", getEleHtml(eleType, id2)).Replace("{NAME3}", cap3).Replace("{ELEMENT3}", getEleHtml(eleType, id3)));
                    }
                }
                string html = sb.ToString();
                if (string.IsNullOrEmpty(divId) || divId=="(Auto)")
                {
                    var ctrols = editor.editocx.getElementsByName("ftdpcom");
                    foreach (IHTMLElement ele in ctrols)
                    {
                        var attr = ele.getAttribute("idname");
                        if (attr != null && attr.ToString() == partid)
                        {
                            string partTrHTML = ele.parentElement.parentElement.outerHTML;
                            var tableEle = ele.parentElement.parentElement.parentElement.parentElement;
                            if (tableEle.tagName.ToUpper() == "TABLE")
                            {
                                IHTMLDOMNode node = ele as IHTMLDOMNode;
                                var nodeTr = node.parentNode.parentNode;
                                while (nodeTr.previousSibling != null)
                                {
                                    nodeTr.parentNode.removeChild(nodeTr.previousSibling);
                                }
                                nodeTr.removeNode();
                                string tableHTML = tableEle.outerHTML.Trim();
                                if (tableHTML.StartsWith("<TABLE", StringComparison.OrdinalIgnoreCase))
                                {
                                    tableHTML = tableHTML.Substring(0, tableHTML.IndexOf("<TBODY>", StringComparison.OrdinalIgnoreCase)) + html + partTrHTML + "</TBODY></TABLE>";
                                }
                                tableEle.outerHTML = tableHTML;
                            }
                        }
                    }
                }
                else
                {
                    var divEle = editor.editocx.getElementById(divId) as IHTMLDOMNode;
                    var tableNode = divEle.firstChild;
                    if (tableNode.nodeName.ToUpper() != "TABLE")
                    {
                        tableNode = tableNode.firstChild;
                    }
                    if (tableNode.nodeName.ToUpper()=="TABLE")
                    {
                        string partTrHTML = "";
                        var trNode = tableNode.firstChild.lastChild;
                        if(trNode!=null)
                        {
                            partTrHTML = (trNode as IHTMLElement).outerHTML;
                            while (trNode.previousSibling != null)
                            {
                                trNode.parentNode.removeChild(trNode.previousSibling);
                            }
                            trNode.removeNode();
                        }
                        string tableHTML = (tableNode as IHTMLElement).outerHTML.Trim();
                        if (tableHTML.StartsWith("<TABLE", StringComparison.OrdinalIgnoreCase))
                        {
                            tableHTML = tableHTML.Substring(0, tableHTML.IndexOf("<TBODY>", StringComparison.OrdinalIgnoreCase)) + html + partTrHTML + "</TBODY></TABLE>";
                        }
                        (tableNode as IHTMLElement).outerHTML = tableHTML;
                    }
                }
                string getEleHtml(int eletype, string idName)
                {
                    if (string.IsNullOrEmpty(idName)) return "";
                    string id = " id=\"" + idName + "\"";
                    string name = " name=\"" + idName + "\"";
                    string restr = "";
                    switch (eletype + 1)
                    {
                        case 1:
                            restr = "<input type=text" + id + name + "/>";
                            break;
                        case 2:
                            restr = "<select" + id + name + "></select>";
                            break;
                        case 3:
                            restr = "<textarea" + id + name + "></textarea>";
                            break;
                        case 4:
                            restr = "<label" + id + ">Label</label>";
                            break;
                        case 5:
                            restr = "<input type=checkbox" + id + name + "/> Label";
                            break;
                        case 6:
                            restr = "<input type=file" + id + name + "/>";
                            break;
                        case 7:
                            restr = "<input type=hidden" + id + name + "/>";
                            break;
                    }
                    return restr;
                }
            }
            catch(Exception ex)
            {
                new error(ex);
            }
        }
        private void OK_Click(object sender, EventArgs e)
        {
            isCancel = false;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NameValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                this.Close();
            }
        }

        List<string> list = new List<string>();
        private void EleGenarate_Load(object sender, EventArgs e)
        {
            string sql = "select id,caption,content from snippets order by id";
            DR dr = new DR(globalConst.ConfigConn.OpenRecord(sql));
            while (dr.Read())
            {
                if (dr.getString("caption").StartsWith("[TEMP]"))
                {
                    comboBox2.Items.Add(dr.getString("caption"));
                    list.Add(dr.getString("content"));
                }
            }
            dr.Close();
            if (comboBox2.Items.Count > 0) comboBox2.SelectedIndex = 0;
            if (list.Count > comboBox2.SelectedIndex)
            {
                layoutHTML = list[comboBox2.SelectedIndex];
            }

            comboBox3.Items.Clear();
            comboBox3.Items.Add("(Auto)");
            var ed = form.getEditor();
            if (ed != null)
            {
                IHTMLElementCollection eles = ed.editocx.getElementsByTagName("DIV");
                foreach (IHTMLElement ele in eles)
                {
                    if (!string.IsNullOrEmpty(ele.id) && ele.className != null && (ele.className.ToLower().IndexOf("ftdiv") >= 0))
                    {
                        comboBox3.Items.Add(ele.id);
                    }
                }
            }
            comboBox3.SelectedIndex = 0;

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (list.Count > comboBox2.SelectedIndex)
            {
                layoutHTML = list[comboBox2.SelectedIndex];
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            appendStr.Text = "add_";
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            appendStr.Text = "mod_";
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            appendStr.Text = "view_";
        }
    }
}