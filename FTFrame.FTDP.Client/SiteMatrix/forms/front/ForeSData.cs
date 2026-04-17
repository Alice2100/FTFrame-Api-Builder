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
using ICSharpCode.TextEditor.Document;

namespace FTDPClient.forms
{
    public partial class ForeSData : Form
    {
        public string SetVal = null;
        public bool IsOK = false;
        public ForeSData()
        {
            InitializeComponent();
        }
        private void OK_Click(object sender, EventArgs e)
        {
            string staticdata = staticData.Text.Trim();
            string apipath = apiPath.Text.Trim();
            if(staticdata!=""&& apipath!="")
            {
                SetVal = staticdata + "[#OPTION#]" + apipath;
            }
            else
            {
                SetVal = staticdata != "" ? staticdata : apipath;
            }
            IsOK = true;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ForeConfig_Load(object sender, EventArgs e)
        {
            label1.Text = res.ft.str("FIInter2.label1");
            button1.Text = res.ft.str("FIInter2.button1");
            button11.Text = res.ft.str("FIInter2.button11");
            label2.Text = res.ft.str("FIInter2.label2");
            label3.Text = res.ft.str("FIInter2.label3");

            string staticdata = "";
            string apipath = "";
            if(SetVal.IndexOf("[#OPTION#]")>=0)
            {
                staticdata = SetVal.Split(new string[] { "[#OPTION#]" },StringSplitOptions.None)[0];
                apipath = SetVal.Split(new string[] { "[#OPTION#]" },StringSplitOptions.None)[1];
            }
            else
            {
                if (SetVal.StartsWith("/"))
                {
                    apipath = SetVal;
                }
                else
                {
                    staticdata = SetVal;
                }
            }
            staticData.Text = staticdata;
            apiPath.Text = apipath;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ForeSelectApi foreSelectApi = new ForeSelectApi();
            foreSelectApi.type = "List";
            foreSelectApi.TopMost = true;
            foreSelectApi.ShowDialog();
            if (!foreSelectApi.IsCancel) apiPath.Text = foreSelectApi.SetVal;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ForeOptionDefine foreOptionDefine = new ForeOptionDefine();
            foreOptionDefine.TopMost = true;
            foreOptionDefine.InitData = staticData.Text.Trim();
            foreOptionDefine.ShowDialog();
            if (!foreOptionDefine.IsCancel) staticData.Text = foreOptionDefine.ReturnVal;
        }

        private void ForeSData_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
            {
                this.Close();
            }
        }
    }
}