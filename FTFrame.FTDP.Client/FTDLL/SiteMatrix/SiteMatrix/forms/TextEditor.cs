using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SiteMatrix.consts;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
namespace SiteMatrix.forms
{
    public partial class TextEditor : Form
    {
        public string basetext="";
        public bool cancel = true;
        public string HighlightingStrategy = "JavaScript";
        public TextEditor()
        {
            InitializeComponent();
        }
        private void TextEditor_Load(object sender, EventArgs e)
        {
            button1.Text = "确定"; res.form.GetString("String33");
            button2.Text = "取消"; res.form.GetString("String34");

            textEditorControl1.Text = basetext;
            textEditorControl1.Font = new Font("微软雅黑",13);
            textEditorControl1.Document.HighlightingStrategy =HighlightingStrategyFactory.CreateHighlightingStrategy(HighlightingStrategy);
            textEditorControl1.ShowEOLMarkers = false;
            textEditorControl1.ShowHRuler = false;
            textEditorControl1.ShowInvalidLines = false;
            textEditorControl1.ShowMatchingBracket = false;
            textEditorControl1.ShowSpaces = false;
            textEditorControl1.ShowTabs = false;
            textEditorControl1.ShowVRuler = false;
            textEditorControl1.IsIconBarVisible = false;
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
            functions.sheel.ExeSheel("http://www.ftframe.com/doc/helper.mht");
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
    }
}