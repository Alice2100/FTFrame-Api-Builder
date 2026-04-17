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
using CefSharp;
using CefSharp.WinForms;
namespace FTDPClient.forms
{
    public partial class ForeBrowser : Form
    {
        public string SetVal = null;
        public bool IsOK = false;
        ChromiumWebBrowser browser;
        public static double ZoomLevel = 0;
        public static ForeBrowser FB = null;
        public ForeBrowser()
        {
            InitializeComponent();
            InitBrowser();
        }
        public void InitBrowser()
        {
            CefSettings settings = new CefSettings();

            // Note that if you get an error or a white screen, you may be doing something wrong !
            // Try to load a local file that you're sure that exists and give the complete path instead to test
            // for example, replace page with a direct path instead :
            // String page = @"C:\Users\SDkCarlos\Desktop\afolder\index.html";

            // String page = string.Format(@"{0}\html-resources\html\index.html", Application.StartupPath);
            string url = "http://www.html5test.com";

            // Initialize cef with the provided settings
            if(!Cef.IsInitialized) Cef.Initialize(settings);
            // Create a browser component
            browser = new ChromiumWebBrowser();

            // Add it to the form and fill it to the form window.
            this.splitContainer1.Panel2.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            // Allow the use of local resources in the browser
            BrowserSettings browserSettings = new BrowserSettings();
            //browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            //browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            browser.BrowserSettings = browserSettings;
            browser.LoadingStateChanged += Browser_LoadingStateChanged;
        }

        public bool IsLoading = true;

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            IsLoading = e.IsLoading;
            //OnLoadingChange(this, new EventArgs());
            if (!e.IsLoading) browser.SetZoomLevel(ZoomLevel);
        }
        public delegate void LoadingChange(object sender, EventArgs e);
        public event LoadingChange OnLoadingChange;

        public void LoadUrl(string url)
        {
            browser.LoadUrl(url);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            browser.SetZoomLevel(0);
            ZoomLevel = 0;
            trackBar1.Value = 50;
        }

        private void ForeConfig_Load(object sender, EventArgs e)
        {
            FB = this;
            trackBar1.Value = Convert.ToInt32((ZoomLevel+2)*25);
        }


        private void ForeSData_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
            {
                this.Close();
            }
        }


        private void ForeBrowser_FormClosed(object sender, FormClosedEventArgs e)
        {
            FB = null;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ZoomLevel = (Convert.ToDouble(trackBar1.Value) / 25d) - 2;
            browser.SetZoomLevel(ZoomLevel);
        }
    }
}