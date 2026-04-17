using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;
namespace SiteMatrix.forms
{
    public partial class PageAssis : Form
    {
        public static bool PageAssisShow = false;
        public static PageAssis PageAssisForm = null;
        private static string CurEditorID = "";
        public PageAssis()
        {
            InitializeComponent();
        }

        private void PageAssis_Load(object sender, EventArgs e)
        {
            PageAssisShow = true;
            listView1.SmallImageList = consts.globalConst.Imgs;
            PageAssisForm = this;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Editor ed = functions.form.getEditor();
            if (ed == null) return;
            IHTMLElement ele = ed.editocx.getElementById(textBox1.Text);
            if (ele != null) ele.scrollIntoView();
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBox1.Text.Equals("输入ID")) textBox1.Text = "";
        }

        private void PageAssis_Shown(object sender, EventArgs e)
        {
            Analys();
        }

        public static void Analys()
        {
            Editor ed = functions.form.getEditor();
            if (ed == null) return;
            //if (ed == null || ed.thisID.Equals(CurEditorID)) return;
            PageAssisForm.listView1.Clear();
            PageAssisForm.listView1.Visible = false;
            PageAssisForm.label1.Visible = true;
            IHTMLElementCollection eles = ed.editocx.getElementsByTagName("DIV");
            foreach (IHTMLElement ele in eles)
            {
                if (ele.className != null && ele.className.ToLower().IndexOf("ftdiv") >= 0)
                {
                    if (ele.id != null && !ele.id.Equals(""))
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = ele.id;
                        item.ImageIndex = 14;
                        item.Tag = ele;
                        PageAssisForm.listView1.Items.Add(item);
                    }
                }
            }
            eles = ed.editocx.getElementsByTagName("DIV");
            foreach (IHTMLElement ele in eles)
            {
                if (ele.className != null && ele.className.ToLower().IndexOf("_tabdiv") >= 0)
                {
                    if (ele.id != null && !ele.id.Equals(""))
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = ele.id;
                        item.ImageIndex = 22;
                        item.Tag = ele;
                        PageAssisForm.listView1.Items.Add(item);
                    }
                }
            }
            eles = ed.editocx.getElementsByTagName("SPAN");
            foreach (IHTMLElement ele in eles)
            {
                if (ele.id != null && ele.id.ToLower().Equals("dotforsitecom"))
                {
                    object partname = ele.getAttribute("partname");
                    string partcap = "";
                    try
                    {
                        partcap = ((IHTMLElement)(((IHTMLElementCollection)ele.children).item(0))).innerText;
                    }
                    catch {
                        partcap = "(Error)";
                    }
                    if (partname == null) partname = "";
                    ListViewItem item = new ListViewItem();
                    item.Text = partcap + "_" + partname;
                    item.ImageIndex = 18;
                    item.Tag = ele;
                    PageAssisForm.listView1.Items.Add(item);
                }
            }
            eles = null;
            PageAssisForm.listView1.Visible = true;
            PageAssisForm.label1.Visible = false;
            CurEditorID = ed.thisID;
        }

        private void PageAssis_FormClosed(object sender, FormClosedEventArgs e)
        {
            PageAssisShow = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Analys();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            Editor ed=functions.form.getEditor();
            if(ed==null)return;
            ((IHTMLElement)e.Item.Tag).scrollIntoView();
            consts.globalConst.MdiForm.HasJustSelectChange = true;
            consts.globalConst.MdiForm.CurEle=(IHTMLElement)(e.Item.Tag);
            ed.doPropertyAdapter();
            consts.globalConst.MdiForm.HasJustSelectChange = false;
            //ed.editocx_onselectionchange(((IHTMLElement)(e.Item.Tag)));
            //Editor.CtlJustSelected = true;
            //ed.INICtlRange.add((IHTMLControlElement)((IHTMLElement)(e.Item.Tag)));
            //ed.INICtlRange.select();
            //ed.INICtlRange.remove(ed.INICtlRange.length - 1);
            //Editor.CtlJustSelected = false;
        }

        private void PageAssis_Resize(object sender, EventArgs e)
        {
            listView1.Width = this.Width - 196 + 157;
            if (this.Width - 196 >= 0)
            {
                button8.Left = this.Width - 196 + 107;
                button2.Left = this.Width - 196 + 94;
            }
            button1.Top = this.Height-598+ 527;
            button2.Top = this.Height - 598 + 527;
            listView1.Height = this.Height - 598 + 480;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button8_Click(sender, null);
            }
        }
    }
}
