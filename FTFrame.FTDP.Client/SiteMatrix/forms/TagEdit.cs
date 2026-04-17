using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FTDPClient.functions;
using System.IO;
using FTDPClient.consts;
using mshtml;
using FTDPClient.Style;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using AngleSharp.Html.Parser;
using AngleSharp.Html;

namespace FTDPClient.forms
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
            //button2.Text = "取消";//res.TagEdit.GetString("button2");
            CodeCut.Text = res.TagEdit.GetString("CodeCut");
            CodeCopy.Text = res.TagEdit.GetString("CodeCopy");
            CodePaste.Text = res.TagEdit.GetString("CodePaste");
            CodeDelete.Text = res.TagEdit.GetString("CodeDelete");
            CodeUndo.Text = res.TagEdit.GetString("CodeUndo");
            CodeRedo.Text = res.TagEdit.GetString("CodeRedo");
            button6.Text = res.com.str("TagEdit.button6");          //往前插入
            button5.Text = res.com.str("TagEdit.button5");          //往后插入
            button4.Text = res.com.str("TagEdit.button4");          //删除元素
            button3.Text = res.com.str("TagEdit.button3");          //更新内部
            button1.Text = res.com.str("TagEdit.button1");          //更新外部
            button2.Text = res.com.str("TagEdit.button2");			//取 消
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //iCSharpTextEditor.Find("A");
            //iCSharpTextEditor.Replace("A","BB");
            //return;
            //var parser = new HtmlParser();
            //var document = parser.ParseDocument(textEditorControl1.Text);
            //using (var writer = new StringWriter())
            //{
            //    document.ToHtml(writer, new PrettyMarkupFormatter
            //    {
            //        Indentation = "\t",
            //        NewLine = "\n"
            //    });
            //    textEditorControl1.Text = writer.ToString();
            //}
            this.Close();
        }
        FTDP.Util.ICSharpTextEditor iCSharpTextEditor;
        private void ModifyDNS_Load(object sender, EventArgs e)
        {
            textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("HTML");
            textEditorControl1.Font = new Font("微软雅黑", 13);
            //textEditorControl1.BackColor = Color.Red;
            //textEditorControl1.ActiveTextAreaControl.BackColor = Color.Blue;
            //textEditorControl1.ActiveTextAreaControl.TextArea.BackColor = Color.Green;
            textEditorControl1.Text = SiteClass.Site.getTextFromEdit(ele.outerHTML);
            iCSharpTextEditor=new FTDP.Util.ICSharpTextEditor();
            iCSharpTextEditor.Init(this, textEditorControl1, false, null);
            //textEditorControl1.ShowEOLMarkers = false;
            //textEditorControl1.ShowHRuler = false;
            //textEditorControl1.ShowInvalidLines = false;
            //textEditorControl1.ShowMatchingBracket = false;
            //textEditorControl1.ShowSpaces = false;
            //textEditorControl1.ShowTabs = false;
            //textEditorControl1.ShowVRuler = false;
            //textEditorControl1.IsIconBarVisible = false;
            label1.Text = res.TagEdit.GetString("itemlabel");

            comboBox1.Items.AddRange(new object[] {
                res.TagEdit.GetString("item0"),
            res.TagEdit.GetString("item1"),
            res.TagEdit.GetString("item2"),
            res.TagEdit.GetString("item3"),
            res.TagEdit.GetString("item4"),
            res.TagEdit.GetString("item5"),
            res.TagEdit.GetString("item6"),
            res.TagEdit.GetString("item7"),});
            comboBox1.SelectedIndex = 0;
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tags = ",input,select,textarea,label,";
            if (tags.IndexOf("," + ele.tagName.ToLower() + ",") < 0) return;
            string id = ele.getAttribute("id")?.ToString();
            string name = ele.getAttribute("name")?.ToString();
            string style =ele.style?.cssText;
            string html = null;
            id = id == null ? "" : " id=\""+id+"\"";
            name = name == null ? "" : " name=\"" + name + "\"";
            style = style == null ? "" : " style=\"" + style + "\"";
            string apdStr = id + name + style;
            switch (comboBox1.SelectedIndex)
            {
                case 1:
                    html = "<input type=text"+ apdStr + "/>";
                    break;
                case 2:
                    html = "<select" + apdStr + "></select>";
                    break;
                case 3:
                    html = "<textarea" + apdStr + "></textarea>";
                    break;
                case 4:
                    html = "<label" + apdStr + ">Label</label>";
                    break;
                case 5:
                    html = "<input type=checkbox" + apdStr + "/> Label";
                    break;
                case 6:
                    html = "<input type=file" + apdStr + "/>";
                    break;
                case 7:
                    html = "<input type=hidden" + apdStr + "/>";
                    break;
            }
            if(html!=null)
            {
                ele.outerHTML = html;
                this.Close();
            }
        }
    }
}