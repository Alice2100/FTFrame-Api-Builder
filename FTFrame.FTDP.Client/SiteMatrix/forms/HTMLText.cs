using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using FTDPClient.consts;
using FTDPClient.database;
using FTDPClient.functions;
using ICSharpCode.TextEditor.Document;

namespace FTDPClient.forms
{
    public partial class HTMLText : Form
    {
        public string SetVal = null;
        public bool IsOK = false;
        public string Strategy = "HTML";
        public (string SnippetTag, string ComboShowText) SnippetDefine = (null, null);
        public HTMLText()
        {
            InitializeComponent();
        }
        private void OK_Click(object sender, EventArgs e)
        {
            SetVal = html_editor.Text;
            IsOK = true;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        bool loaded = false;
        private void ForeConfig_Load(object sender, EventArgs e)
        {
            comboBox1.Visible = SnippetDefine.SnippetTag != null;
            //new FTDP.Util.ICSharpTextEditor().Init(this, html_editor, null);
            html_editor.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(Strategy);
            html_editor.Text = SetVal;

            if (SnippetDefine.SnippetTag != null)
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add(new Obj.ComboItem() { Id = "", Name = SnippetDefine.ComboShowText, Tag = "" });
                string sql = "select caption,content from snippets where caption like '"+ SnippetDefine.SnippetTag+ "%' order by caption";
                DR dr = new DR(globalConst.ConfigConn.OpenRecord(sql));
                while (dr.Read())
                {
                    comboBox1.Items.Add(new Obj.ComboItem() { Id = "", Name = dr.getString("caption").Substring(SnippetDefine.SnippetTag.Length), Tag = dr.getString("content") });
                }
                dr.Close();
                comboBox1.SelectedIndex = 0;
            }
            loaded = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                html_editor.ResetText();
                html_editor.Text = "";
                html_editor.Refresh();
                html_editor.Text = (comboBox1.SelectedItem as Obj.ComboItem).Tag.ToString();
            }
        }
    }
}