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

namespace FTDPClient.forms
{
    public partial class ForeConfig : Form
    {
        
        public ForeConfig()
        {
            InitializeComponent();
        }
        private void OK_Click(object sender, EventArgs e)
        {
            try
            {
                string jsfile = globalConst.CurSite.Path + @"\__front\lib\config.js";
                file.Delete(jsfile);
                file.CreateText(jsfile, config_editor.Text);

                Options.SetSystemValue("FrontEsLintPath", eslint_path.Text.Trim());
                ForeDev.EslintPath = eslint_path.Text.Trim();
            }
            catch (Exception ex)
            {
                MsgBox.Error(ex.Message);
            }
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ForeConfig_Load(object sender, EventArgs e)
        {
            checkBox1.Text= res.front.str("TempConfig.checkBox1");
            button1.Text= res.front.str("TempConfig.button1");
            label2.Text= res.front.str("TempConfig.label2");


            checkBox1.Checked = ForeDev.SaveIsExport;
            label1.Text = ForeDev.SaveIsExportPath;
            eslint_path.Text = ForeDev.EslintPath;
            new FTDP.Util.ICSharpTextEditor().Init(this, config_editor, false, null);
            try
            {
                string jsfile = globalConst.CurSite.Path + @"\__front\lib\config.js";
                if (!file.Exists(jsfile))
                {
                    MsgBox.Warning("File not exist: "+ globalConst.CurSite.Path + @"\__front\lib\config.js");
                    this.Close();
                }
                else
                {
                    StreamReader sr = file.OpenText(jsfile);
                    config_editor.Text = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.Error(ex.Message);
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            ForeDev.SaveIsExport = checkBox1.Checked;
            if (ForeDev.SaveIsExport && !string.IsNullOrEmpty(ForeDev.SaveIsExportPath))
            {
                Options.SetSystemValue("FrontSaveIsExport", ForeDev.SaveIsExportPath);
            }
            else
            {
                Options.SetSystemValue("FrontSaveIsExport", "");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            if(path.SelectedPath!=null)
            {
                label1.Text= path.SelectedPath;
                ForeDev.SaveIsExportPath= path.SelectedPath;
                if (ForeDev.SaveIsExport && !string.IsNullOrEmpty(ForeDev.SaveIsExportPath))
                {
                    Options.SetSystemValue("FrontSaveIsExport", ForeDev.SaveIsExportPath);
                }
                else
                {
                    Options.SetSystemValue("FrontSaveIsExport", "");
                }
            }
        }
    }
}