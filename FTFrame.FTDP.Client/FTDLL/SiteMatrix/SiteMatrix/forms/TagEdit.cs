using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SiteMatrix.functions;
using System.IO;
using SiteMatrix.consts;
using mshtml;
using SiteMatrix.Style;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace SiteMatrix.forms
{
    public partial class TagEdit : Form
    {
        public IHTMLElement ele;
        public TagEdit()
        {
            InitializeComponent();
            CustomProfessionalColors cfc = new CustomProfessionalColors();
            cfc.UseSystemColors = true;
            ToolStripProfessionalRenderer tsprr = new ToolStripProfessionalRenderer(cfc);
            CMCode.Renderer = tsprr;
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            this.Text=res.TagEdit.GetString("_this");
            //button1.Text ="确定(&O)" ;//res.TagEdit.GetString("button1");
            button2.Text = "取消";//res.TagEdit.GetString("button2");
            CodeCut.Text = res.TagEdit.GetString("CodeCut");
            CodeCopy.Text = res.TagEdit.GetString("CodeCopy");
            CodePaste.Text = res.TagEdit.GetString("CodePaste");
            CodeDelete.Text = res.TagEdit.GetString("CodeDelete");
            CodeUndo.Text = res.TagEdit.GetString("CodeUndo");
            CodeRedo.Text = res.TagEdit.GetString("CodeRedo");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ModifyDNS_Load(object sender, EventArgs e)
        {
            textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("HTML");
            textEditorControl1.Font = new Font("微软雅黑", 13);
            //textEditorControl1.BackColor = Color.Red;
            //textEditorControl1.ActiveTextAreaControl.BackColor = Color.Blue;
            //textEditorControl1.ActiveTextAreaControl.TextArea.BackColor = Color.Green;
            textEditorControl1.Text = SiteClass.Site.getTextFromEdit(ele.outerHTML);
            textEditorControl1.ShowEOLMarkers = false;
            textEditorControl1.ShowHRuler = false;
            textEditorControl1.ShowInvalidLines = false;
            textEditorControl1.ShowMatchingBracket = false;
            textEditorControl1.ShowSpaces = false;
            textEditorControl1.ShowTabs = false;
            textEditorControl1.ShowVRuler = false;
            textEditorControl1.IsIconBarVisible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //ele.outerHTML = SiteClass.Site.getEditFromText(textocx.theText);
                ele.outerHTML = SiteClass.Site.getEditFromText(textEditorControl1.Text);
                this.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("HRESULT")>0)
                {
                    MsgBox.Warning(res.TagEdit.GetString("m1") + "(" + ele.tagName + ")");
                }
                else
                {
                    MsgBox.Error(ex.Message);
                }
            }
        }
        private bool HaveSelection()
        {
                return textEditorControl1.ActiveTextAreaControl.TextArea.SelectionManager.HasSomethingSelected;
        }
        private void CMCode_Opening(object sender, CancelEventArgs e)
        {
            CodeCopy.Enabled = HaveSelection();
            CodeCut.Enabled = HaveSelection();
            CodeDelete.Enabled = HaveSelection();
            CodePaste.Enabled = true;
            CodeUndo.Enabled = false;
            CodeRedo.Enabled = false;
        }
        private void DoEditAction(TextEditorControl editor, ICSharpCode.TextEditor.Actions.IEditAction action)
        {
            if (editor != null && action != null)
            {
                var area = editor.ActiveTextAreaControl.TextArea;
                editor.BeginUpdate();
                try
                {
                    lock (editor.Document)
                    {
                        action.Execute(area);
                        if (area.SelectionManager.HasSomethingSelected && area.AutoClearSelection /*&& caretchanged*/)
                        {
                            if (area.Document.TextEditorProperties.DocumentSelectionMode == DocumentSelectionMode.Normal)
                            {
                                area.SelectionManager.ClearSelection();
                            }
                        }
                    }
                }
                finally
                {
                    editor.EndUpdate();
                    area.Caret.UpdateCaretPosition();
                }
            }
        }
        private void CodeCut_Click(object sender, EventArgs e)
        {
            if (HaveSelection())
                DoEditAction(textEditorControl1, new ICSharpCode.TextEditor.Actions.Cut());
        }

        private void CodeCopy_Click(object sender, EventArgs e)
        {
            if (HaveSelection())
                DoEditAction(textEditorControl1, new ICSharpCode.TextEditor.Actions.Copy());
        }

        private void CodePaste_Click(object sender, EventArgs e)
        {
            DoEditAction(textEditorControl1, new ICSharpCode.TextEditor.Actions.Paste());
        }

        private void CodeDelete_Click(object sender, EventArgs e)
        {
            if (HaveSelection())
                DoEditAction(textEditorControl1, new ICSharpCode.TextEditor.Actions.Delete());
        }

        private void CodeUndo_Click(object sender, EventArgs e)
        {
            //textocx.Undo();
        }

        private void CodeRedo_Click(object sender, EventArgs e)
        {
            //textocx.Redo();
        }

        private void textocx_OnContextMenu(object sender, MouseEventArgs e)
        {
            //CMCode.Show(textocx, new Point(e.X, e.Y));
        }

        private void TagEdit_Shown(object sender, EventArgs e)
        {
            /*
            try
            {
                textocx.Visible = false;
                string sql;
                sql = "select thevalue from system where name='sceditwraped'";
                if (globalConst.ConfigConn.GetString(sql).ToLower().Equals("1"))
                {
                    textocx.WordWrap = true;
                }
                else
                {
                    textocx.WordWrap = false;
                }
                sql = "select thevalue from system where name='sceditbgcolor'";
                textocx.BgColor = ColorTranslator.FromHtml(globalConst.ConfigConn.GetString(sql));
                sql = "select thevalue from system where name='sceditsize'";
                textocx.theFontSize = int.Parse(globalConst.ConfigConn.GetString(sql));
                sql = "select thevalue from system where name='sceditfont'";
                int FontID = int.Parse(globalConst.ConfigConn.GetString(sql));
                textocx.theFontName = Editor.GetFontName(FontID);


                textocx.theText = SiteClass.Site.getTextFromEdit(ele.outerHTML);
                textocx.Color_All();
                textocx.Visible = true;
                textocx.TextHasChanged = false;
                sql = "select thevalue from system where name='sceditmargin'";
                if (globalConst.ConfigConn.GetString(sql).ToLower().Equals("1"))
                {
                    textocx.ShowSelectionMargin = true;
                }
                else
                {
                    textocx.ShowSelectionMargin = false;
                }
            }
            catch (Exception ex)
            {
                MsgBox.Error(ex.Message);
            }*/
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                string val = SiteClass.Site.getEditFromText(textEditorControl1.Text, false);
                if (val != null)
                {
                    ele.innerHTML = val;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("HRESULT") > 0)
                {
                    MsgBox.Warning(res.TagEdit.GetString("m1") + "(" + ele.tagName + ")");
                }
                else
                {
                    MsgBox.Error(ex.Message);
                }
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            //form.getEditor().editocx.execCommand(htmleditocx.D4ENUM.D4HTMLCMDSTR.HTMLCMD_Delete, false, null);
            //form.getEditor().editocx.removeChild((IHTMLDOMNode)ele);
            ele.outerHTML = "";
            this.Close();
        }
        private void TagEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (button2.Focused) this.Close();
                else button2.Focus();
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            try
            { 
            ele.insertAdjacentHTML("afterEnd", SiteClass.Site.getEditFromText(textEditorControl1.Text));
            this.Close();
            }
            catch(Exception ex)
            {
                new error(ex);
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            try
            {
                ele.insertAdjacentHTML("beforeBegin", SiteClass.Site.getEditFromText(textEditorControl1.Text));
                this.Close();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void TextEditorControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                button2.Focus();
            }
        }
    }
}