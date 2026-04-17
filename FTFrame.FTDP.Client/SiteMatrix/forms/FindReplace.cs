using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using htmleditocx;
using FTDPClient.consts;
using FTDPClient.forms;
using FTDPClient.functions;

namespace FTDPClient.forms
{
    public partial class FindReplace : Form
    {
        public string FindString = null;
        public static bool FindReplaceShow = false;
        public FindReplace()
        {
            InitializeComponent();
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            this.Text = res.FindReplace.GetString("_this");
            Label1.Text = res.FindReplace.GetString("Label1");
            Label2.Text = res.FindReplace.GetString("Label2");
            GroupBox1.Text = res.FindReplace.GetString("GroupBox1");
            MatchCase.Text = res.FindReplace.GetString("MatchCase");
            WholeWord.Text = res.FindReplace.GetString("WholeWord");
            Find.Text = res.FindReplace.GetString("Find");
            btnReplace.Text = res.FindReplace.GetString("btnReplace");
            btnReplaceAll.Text = res.FindReplace.GetString("btnReplaceAll");
            btnCancel.Text = res.FindReplace.GetString("btnCancel");
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private RichTextBoxFinds GetOptions()
        {
            if(this.WholeWord.Checked&&this.MatchCase.Checked)
            {
                return (RichTextBoxFinds)6;
            }
            else if(this.WholeWord.Checked&&!this.MatchCase.Checked)
            {
                return RichTextBoxFinds.WholeWord;
            }
            else if(!this.WholeWord.Checked&&this.MatchCase.Checked)
            {
                return RichTextBoxFinds.MatchCase;
            }
            else
            {
                return RichTextBoxFinds.None;
            }
        }
        private void StartSearch()
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("text"))
                    {
                        
                        if(!edr.textEditor.Find(FindTextBox.Text))
                        {
                            MsgBox.Information(res.FindReplace.GetString("a1"));
                        }
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void Find_Click(object sender, EventArgs e)
        {
            StartSearch();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("text"))
                    {
                        if (!edr.textEditor.Replace(FindTextBox.Text,this.ReplaceTextBox.Text))
                        {
                            MsgBox.Information(res.FindReplace.GetString("a1"));
                        }
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            if (ReplaceTextBox.Text.IndexOf(FindTextBox.Text) >= 0) return;
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("text"))
                    {
                        if (!edr.textEditor.Replace(FindTextBox.Text, this.ReplaceTextBox.Text))
                        {
                            MsgBox.Information(res.FindReplace.GetString("a1"));
                        }
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void FindReplace_Load(object sender, EventArgs e)
        {
            if(FindString!=null)
            {
                this.FindTextBox.Text = FindString;
                StartSearch();
            }
            FindReplaceShow = true;
        }

        private void FindReplace_FormClosed(object sender, FormClosedEventArgs e)
        {
            FindReplaceShow = false;
        }
    }
}