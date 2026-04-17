using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SiteMatrix.functions;
using SiteMatrix.consts;
using SiteMatrix.Adapter;
using mshtml;
using htmleditocx;
using SiteMatrix.Page;
using SiteMatrix.database;
using SiteMatrix.Style;
using System.Drawing.Text;

namespace SiteMatrix.forms
{
    /// <summary>
    /// Editor 的摘要说明。
    /// </summary>
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class Editor : System.Windows.Forms.Form
    {
        public string thisUrl = "";
        public string thisTitle = "";
        public string thisID = "";
        public string thisName = "";
        public string thisEditUrl = "";
        public string thisViewUrl = "";
        public bool isFreeFile = false;
        public bool isFreeFileSaved = false;
        public DateTime UrlLastModTime = DateTime.Now;
        //0,normal.1.form page1.2.form page2
        public int pagetype = 0;
        public string editmode = "edit";
        public string lastSavedHTML = "";
        public sceditocx.ScEditOcx textocx;
        private bool ScEditSheelEdit4Margin=false;
        private bool ScEditSheelEdit4BgColor = false;
        private bool ScEditSheelEdit4Wraped = false;
        private bool ScEditSheelEdit4FontSize = false;
        private bool ScEditSheelEdit4FontName = false;
        private bool HasViewed = false;
        private bool textChanged = false;
        private System.Windows.Forms.Timer editor_timer;
        private System.ComponentModel.IContainer components;
        public IHTMLTxtRange INITxtRange;
        public IHTMLControlRange INICtlRange;
        public static bool CtlJustSelected = false;
        private bool LoadComplete = false;
        public HTMLTextContainerEvents2_Event iEvent;
        public FlatTabControl.FlatTabControl EditSpace;
        private TabPage tabDesign;
        private TabPage tabCode;
        private TabPage tabPreview;
        public htmleditocx.D4HtmlEditOcx editocx;
        private htmleditocx.D4HtmlEditOcx viewocx;
        private ToolStrip CodeEditTool;
        private ToolStripButton BGColor;
        private ToolStripComboBox TheFontName;
        private ToolStripComboBox TheFontSize;
        private ToolStripLabel toolStripLabel1;
        private ToolStripLabel toolStripLabel2;
        private Panel panel1;
        private ContextMenuStrip CMPage;
        private ToolStripMenuItem pageCutToolStripMenuItem;
        private ToolStripMenuItem pageCopyToolStripMenuItem;
        private ToolStripMenuItem pagePasteToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator79;
        private ToolStripMenuItem pageUndoToolStripMenuItem;
        private ToolStripMenuItem pageRedoToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator80;
        private ToolStripMenuItem pageTableToolStripMenuItem;
        private ToolStripMenuItem pageInsertTableToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator82;
        private ToolStripMenuItem pageInsertRowToolStripMenuItem;
        private ToolStripMenuItem pageInsertColumnToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator83;
        private ToolStripMenuItem pageDeleteRowToolStripMenuItem;
        private ToolStripMenuItem pageDeleteColumnToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator84;
        private ToolStripMenuItem pageSplitRowToolStripMenuItem;
        private ToolStripMenuItem pageSplitColumnToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator85;
        private ToolStripMenuItem pageMergeCellToolStripMenuItem;
        private ToolStripMenuItem pageMergeCellRightToolStripMenuItem;
        private ToolStripMenuItem pageMergeCellUpToolStripMenuItem;
        private ToolStripMenuItem pageMergeCellDownToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator86;
        private ToolStripMenuItem pageSelectTableToolStripMenuItem;
        private ToolStripMenuItem pageClearContentToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator81;
        private ToolStripMenuItem pageEditTagToolStripMenuItem;
        int ex=0;
		int	ey=0;
        private ToolStripMenuItem pageNewPartToolStripMenuItem;
        private ToolStripMenuItem pageClonePartToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem PageDelete;
        private ToolStripButton SelectionMargin;
        private ToolStripButton WordWrap;
        private ToolStripLabel CurLine;
        private ToolStripLabel curCol;
        private ToolStripLabel toolStripLabel4;
        private ToolStripLabel toolStripLabel3;
        private ContextMenuStrip CMCode;
        private ToolStripMenuItem CodeCut;
        private ToolStripMenuItem CodeCopy;
        private ToolStripMenuItem CodePaste;
        private ToolStripMenuItem CodeDelete;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem CodeUndo;
        private ToolStripMenuItem CodeRedo;
        private ToolStripMenuItem EditFormDataMenuItem1;
        private ToolStripMenuItem cloneToolStripMenuItem;
        private ToolStripMenuItem copypartidToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem cloneandreplaceToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem ToolStdadadaripMenuItem;
        public bool EventAdded = false;
        public Editor()
        {
            //
            // Windows 窗体设计器支持所必需的
            //
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            InitializeComponent();
            InitCustomFonts();
            CustomProfessionalColors cfc = new CustomProfessionalColors();
            cfc.UseSystemColors = true;
            ToolStripProfessionalRenderer tsprr= new ToolStripProfessionalRenderer(cfc);
            CodeEditTool.Renderer=tsprr;
            CMPage.Renderer = tsprr;
            CMCode.Renderer = tsprr;
            globalConst.MdiForm.ChangeTabPageActiveColor(EditSpace);
            EditSpace.ImageList = globalConst.Imgs;
            tabDesign.ImageIndex = 13;
            tabCode.ImageIndex = 14;
            tabPreview.ImageIndex = 15;
            ApplyLanguage();
            //KeyPreview = true;
            //
            // TODO: 在 InitializeComponent 调用后添加任何构造函数代码
            //
        }



        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.textocx = new sceditocx.ScEditOcx();
            this.editor_timer = new System.Windows.Forms.Timer(this.components);
            this.EditSpace = new FlatTabControl.FlatTabControl();
            this.tabDesign = new System.Windows.Forms.TabPage();
            this.editocx = new htmleditocx.D4HtmlEditOcx();
            this.tabCode = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CodeEditTool = new System.Windows.Forms.ToolStrip();
            this.SelectionMargin = new System.Windows.Forms.ToolStripButton();
            this.WordWrap = new System.Windows.Forms.ToolStripButton();
            this.BGColor = new System.Windows.Forms.ToolStripButton();
            this.TheFontName = new System.Windows.Forms.ToolStripComboBox();
            this.TheFontSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.CurLine = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.curCol = new System.Windows.Forms.ToolStripLabel();
            this.tabPreview = new System.Windows.Forms.TabPage();
            this.viewocx = new htmleditocx.D4HtmlEditOcx();
            this.CMPage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pageCutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pagePasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PageDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator79 = new System.Windows.Forms.ToolStripSeparator();
            this.pageUndoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageRedoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator80 = new System.Windows.Forms.ToolStripSeparator();
            this.pageTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageInsertTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator82 = new System.Windows.Forms.ToolStripSeparator();
            this.pageInsertRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageInsertColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator83 = new System.Windows.Forms.ToolStripSeparator();
            this.pageDeleteRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageDeleteColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator84 = new System.Windows.Forms.ToolStripSeparator();
            this.pageSplitRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageSplitColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator85 = new System.Windows.Forms.ToolStripSeparator();
            this.pageMergeCellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageMergeCellRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageMergeCellUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageMergeCellDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator86 = new System.Windows.Forms.ToolStripSeparator();
            this.pageSelectTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageClearContentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator81 = new System.Windows.Forms.ToolStripSeparator();
            this.pageNewPartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageClonePartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.EditFormDataMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pageEditTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cloneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cloneandreplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.copypartidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CMCode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CodeCut = new System.Windows.Forms.ToolStripMenuItem();
            this.CodeCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.CodePaste = new System.Windows.Forms.ToolStripMenuItem();
            this.CodeDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.CodeUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.CodeRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStdadadaripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditSpace.SuspendLayout();
            this.tabDesign.SuspendLayout();
            this.tabCode.SuspendLayout();
            this.panel1.SuspendLayout();
            this.CodeEditTool.SuspendLayout();
            this.tabPreview.SuspendLayout();
            this.CMPage.SuspendLayout();
            this.CMCode.SuspendLayout();
            this.SuspendLayout();
            // 
            // textocx
            // 
            this.textocx.BgColor = System.Drawing.SystemColors.Window;
            this.textocx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textocx.HideSelection = false;
            this.textocx.Location = new System.Drawing.Point(0, 0);
            this.textocx.Name = "textocx";
            this.textocx.RightMargin = 0;
            this.textocx.SelectedText = "";
            this.textocx.SelectionLength = 0;
            this.textocx.SelectionStart = 0;
            this.textocx.ShowSelectionMargin = false;
            this.textocx.Size = new System.Drawing.Size(632, 296);
            this.textocx.TabIndex = 8;
            this.textocx.theAllowDrop = true;
            this.textocx.theFont = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textocx.theFontName = "宋体";
            this.textocx.theFontSize = 18F;
            this.textocx.theRtf = "{\\rtf1\\ansi\\ansicpg936\\deff0\\deflang1033\\deflangfe2052{\\fonttbl{\\f0\\fnil\\fcharset" +
    "134 \\\'cb\\\'ce\\\'cc\\\'e5;}}\r\n\\viewkind4\\uc1\\pard\\lang2052\\f0\\fs36\\par\r\n}\r\n";
            this.textocx.theText = "";
            this.textocx.WordWrap = true;
            this.textocx.TextChange += new sceditocx.ScEditOcx.TextChangeEventHandler(this.textocx_TextChange);
            this.textocx.SelectionChange += new sceditocx.ScEditOcx.SelectionChangeEventHandler(this.textocx_SelectionChange);
            this.textocx.theMouseDown += new sceditocx.ScEditOcx.theMouseDownEventHandler(this.textocx_theMouseDown);
            this.textocx.OnContextMenu += new sceditocx.ScEditOcx.OnContextMenuEventHandler(this.textocx_OnContextMenu);
            this.textocx.theDragEnter += new sceditocx.ScEditOcx.theDragEnterEventHandler(this.textocx_theDragEnter);
            this.textocx.theDragDrop += new sceditocx.ScEditOcx.theDragDropEventHandler(this.textocx_theDragDrop);
            // 
            // editor_timer
            // 
            this.editor_timer.Interval = 10;
            this.editor_timer.Tick += new System.EventHandler(this.editor_timer_Tick);
            // 
            // EditSpace
            // 
            this.EditSpace.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.EditSpace.Controls.Add(this.tabDesign);
            this.EditSpace.Controls.Add(this.tabCode);
            this.EditSpace.Controls.Add(this.tabPreview);
            this.EditSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditSpace.Location = new System.Drawing.Point(0, 0);
            this.EditSpace.Margin = new System.Windows.Forms.Padding(0);
            this.EditSpace.myBackColor = System.Drawing.SystemColors.Control;
            this.EditSpace.Name = "EditSpace";
            this.EditSpace.SelectedIndex = 0;
            this.EditSpace.Size = new System.Drawing.Size(640, 535);
            this.EditSpace.TabIndex = 59;
            this.EditSpace.SelectedIndexChanged += new System.EventHandler(this.EditSpace_SelectedIndexChanged);
            this.EditSpace.Resize += new System.EventHandler(this.EditSpace_Resize);
            // 
            // tabDesign
            // 
            this.tabDesign.BackColor = System.Drawing.SystemColors.Control;
            this.tabDesign.Controls.Add(this.editocx);
            this.tabDesign.ImageIndex = 0;
            this.tabDesign.Location = new System.Drawing.Point(4, 4);
            this.tabDesign.Margin = new System.Windows.Forms.Padding(0);
            this.tabDesign.Name = "tabDesign";
            this.tabDesign.Size = new System.Drawing.Size(632, 505);
            this.tabDesign.TabIndex = 0;
            this.tabDesign.Text = "Design";
            // 
            // editocx
            // 
            this.editocx.alinkColor = "#0000ff";
            this.editocx.AllowDrop = true;
            this.editocx.bgColor = "#ffffff";
            this.editocx.cookie = null;
            this.editocx.isDesignMode = true;
            this.editocx.linkColor = "#0000ff";
            this.editocx.Location = new System.Drawing.Point(0, 0);
            this.editocx.Margin = new System.Windows.Forms.Padding(0);
            this.editocx.Name = "editocx";
            this.editocx.ReadOnlyWhenApplyHTML = false;
            this.editocx.showBodyNetCells = false;
            this.editocx.showGlyphs = false;
            this.editocx.showLiveResizes = false;
            this.editocx.showTableBorders = true;
            this.editocx.Size = new System.Drawing.Size(380, 321);
            this.editocx.TabIndex = 17;
            this.editocx.url = "about:blank";
            this.editocx.onDocumentdblclick += new htmleditocx.D4HtmlEditOcx.onDocumentdblclickEventHandler(this.editocx_onDocumentdblclick);
            this.editocx.onDocumentmousemove += new htmleditocx.D4HtmlEditOcx.onDocumentmousemoveEventHandler(this.editocx_onDocumentmousemove);
            this.editocx.oncontextmenu += new htmleditocx.D4HtmlEditOcx.oncontextmenuEventHandler(this.editocx_oncontextmenu);
            this.editocx.ondragstart += new htmleditocx.D4HtmlEditOcx.ondragstartEventHandler(this.editocx_ondragstart);
            this.editocx.onselectionchange += new htmleditocx.D4HtmlEditOcx.onselectionchangeEventHandler(this.editocx_onselectionchange);
            // 
            // tabCode
            // 
            this.tabCode.Controls.Add(this.panel1);
            this.tabCode.Controls.Add(this.CodeEditTool);
            this.tabCode.ImageIndex = 1;
            this.tabCode.Location = new System.Drawing.Point(4, 4);
            this.tabCode.Name = "tabCode";
            this.tabCode.Size = new System.Drawing.Size(192, 70);
            this.tabCode.TabIndex = 1;
            this.tabCode.Text = "Code";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textocx);
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 296);
            this.panel1.TabIndex = 10;
            // 
            // CodeEditTool
            // 
            this.CodeEditTool.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.CodeEditTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectionMargin,
            this.WordWrap,
            this.BGColor,
            this.TheFontName,
            this.TheFontSize,
            this.toolStripLabel3,
            this.toolStripLabel1,
            this.CurLine,
            this.toolStripLabel4,
            this.toolStripLabel2,
            this.curCol});
            this.CodeEditTool.Location = new System.Drawing.Point(0, 0);
            this.CodeEditTool.Name = "CodeEditTool";
            this.CodeEditTool.Size = new System.Drawing.Size(192, 29);
            this.CodeEditTool.TabIndex = 9;
            this.CodeEditTool.Text = "CodeEditTool";
            // 
            // SelectionMargin
            // 
            this.SelectionMargin.CheckOnClick = true;
            this.SelectionMargin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SelectionMargin.Image = ((System.Drawing.Image)(resources.GetObject("SelectionMargin.Image")));
            this.SelectionMargin.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.SelectionMargin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SelectionMargin.Name = "SelectionMargin";
            this.SelectionMargin.Size = new System.Drawing.Size(23, 26);
            this.SelectionMargin.Text = "SelectionMargin";
            this.SelectionMargin.Click += new System.EventHandler(this.SelectionMargin_Click);
            // 
            // WordWrap
            // 
            this.WordWrap.CheckOnClick = true;
            this.WordWrap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.WordWrap.Image = ((System.Drawing.Image)(resources.GetObject("WordWrap.Image")));
            this.WordWrap.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.WordWrap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.WordWrap.Name = "WordWrap";
            this.WordWrap.Size = new System.Drawing.Size(23, 26);
            this.WordWrap.Text = "WordWrap";
            this.WordWrap.Click += new System.EventHandler(this.WordWrap_Click);
            // 
            // BGColor
            // 
            this.BGColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BGColor.Image = ((System.Drawing.Image)(resources.GetObject("BGColor.Image")));
            this.BGColor.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BGColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BGColor.Name = "BGColor";
            this.BGColor.Size = new System.Drawing.Size(23, 26);
            this.BGColor.Text = "BGColor";
            this.BGColor.Click += new System.EventHandler(this.BGColor_Click);
            // 
            // TheFontName
            // 
            this.TheFontName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TheFontName.Name = "TheFontName";
            this.TheFontName.Size = new System.Drawing.Size(75, 29);
            this.TheFontName.ToolTipText = "FontName";
            this.TheFontName.SelectedIndexChanged += new System.EventHandler(this.TheFontName_SelectedIndexChanged);
            // 
            // TheFontSize
            // 
            this.TheFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TheFontSize.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "14",
            "16",
            "18",
            "20",
            "22",
            "24",
            "26",
            "28",
            "30",
            "32"});
            this.TheFontSize.Name = "TheFontSize";
            this.TheFontSize.Size = new System.Drawing.Size(75, 25);
            this.TheFontSize.ToolTipText = "FontSize";
            this.TheFontSize.SelectedIndexChanged += new System.EventHandler(this.TheFontSize_SelectedIndexChanged);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(16, 17);
            this.toolStripLabel3.Text = "  ";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(42, 17);
            this.toolStripLabel1.Text = "Rows:";
            // 
            // CurLine
            // 
            this.CurLine.Name = "CurLine";
            this.CurLine.Size = new System.Drawing.Size(15, 17);
            this.CurLine.Text = "0";
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(16, 17);
            this.toolStripLabel4.Text = "  ";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(36, 17);
            this.toolStripLabel2.Text = "Cols:";
            // 
            // curCol
            // 
            this.curCol.Name = "curCol";
            this.curCol.Size = new System.Drawing.Size(15, 17);
            this.curCol.Text = "0";
            // 
            // tabPreview
            // 
            this.tabPreview.Controls.Add(this.viewocx);
            this.tabPreview.ImageIndex = 2;
            this.tabPreview.Location = new System.Drawing.Point(4, 4);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.Size = new System.Drawing.Size(192, 70);
            this.tabPreview.TabIndex = 2;
            this.tabPreview.Text = "Preview";
            // 
            // viewocx
            // 
            this.viewocx.alinkColor = "#0000ff";
            this.viewocx.bgColor = "#ffffff";
            this.viewocx.cookie = null;
            this.viewocx.isDesignMode = true;
            this.viewocx.linkColor = "#0000ff";
            this.viewocx.Location = new System.Drawing.Point(0, 0);
            this.viewocx.Name = "viewocx";
            this.viewocx.ReadOnlyWhenApplyHTML = false;
            this.viewocx.showBodyNetCells = false;
            this.viewocx.showGlyphs = false;
            this.viewocx.showLiveResizes = false;
            this.viewocx.showTableBorders = true;
            this.viewocx.Size = new System.Drawing.Size(632, 321);
            this.viewocx.TabIndex = 6;
            this.viewocx.url = "about:blank";
            // 
            // CMPage
            // 
            this.CMPage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pageCutToolStripMenuItem,
            this.pageCopyToolStripMenuItem,
            this.pagePasteToolStripMenuItem,
            this.PageDelete,
            this.toolStripSeparator79,
            this.pageUndoToolStripMenuItem,
            this.pageRedoToolStripMenuItem,
            this.toolStripSeparator80,
            this.pageTableToolStripMenuItem,
            this.toolStripSeparator81,
            this.pageNewPartToolStripMenuItem,
            this.pageClonePartToolStripMenuItem,
            this.toolStripSeparator3,
            this.EditFormDataMenuItem1,
            this.pageEditTagToolStripMenuItem,
            this.toolStripSeparator1,
            this.cloneToolStripMenuItem,
            this.cloneandreplaceToolStripMenuItem,
            this.toolStripSeparator4,
            this.copypartidToolStripMenuItem,
            this.ToolStdadadaripMenuItem});
            this.CMPage.Name = "CMPage";
            this.CMPage.Size = new System.Drawing.Size(186, 422);
            this.CMPage.Opening += new System.ComponentModel.CancelEventHandler(this.CMPage_Opening);
            // 
            // pageCutToolStripMenuItem
            // 
            this.pageCutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageCutToolStripMenuItem.Image")));
            this.pageCutToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageCutToolStripMenuItem.Name = "pageCutToolStripMenuItem";
            this.pageCutToolStripMenuItem.Size = new System.Drawing.Size(185, 24);
            this.pageCutToolStripMenuItem.Text = "PageCut";
            this.pageCutToolStripMenuItem.Click += new System.EventHandler(this.pageCutToolStripMenuItem_Click);
            // 
            // pageCopyToolStripMenuItem
            // 
            this.pageCopyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageCopyToolStripMenuItem.Image")));
            this.pageCopyToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageCopyToolStripMenuItem.Name = "pageCopyToolStripMenuItem";
            this.pageCopyToolStripMenuItem.Size = new System.Drawing.Size(185, 24);
            this.pageCopyToolStripMenuItem.Text = "PageCopy";
            this.pageCopyToolStripMenuItem.Click += new System.EventHandler(this.pageCopyToolStripMenuItem_Click);
            // 
            // pagePasteToolStripMenuItem
            // 
            this.pagePasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pagePasteToolStripMenuItem.Image")));
            this.pagePasteToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pagePasteToolStripMenuItem.Name = "pagePasteToolStripMenuItem";
            this.pagePasteToolStripMenuItem.Size = new System.Drawing.Size(185, 24);
            this.pagePasteToolStripMenuItem.Text = "PagePaste";
            this.pagePasteToolStripMenuItem.Click += new System.EventHandler(this.pagePasteToolStripMenuItem_Click);
            // 
            // PageDelete
            // 
            this.PageDelete.Image = ((System.Drawing.Image)(resources.GetObject("PageDelete.Image")));
            this.PageDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.PageDelete.Name = "PageDelete";
            this.PageDelete.Size = new System.Drawing.Size(185, 24);
            this.PageDelete.Text = "PageDelete";
            this.PageDelete.Click += new System.EventHandler(this.PageDelete_Click);
            // 
            // toolStripSeparator79
            // 
            this.toolStripSeparator79.Name = "toolStripSeparator79";
            this.toolStripSeparator79.Size = new System.Drawing.Size(182, 6);
            // 
            // pageUndoToolStripMenuItem
            // 
            this.pageUndoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageUndoToolStripMenuItem.Image")));
            this.pageUndoToolStripMenuItem.Name = "pageUndoToolStripMenuItem";
            this.pageUndoToolStripMenuItem.Size = new System.Drawing.Size(185, 24);
            this.pageUndoToolStripMenuItem.Text = "PageUndo";
            this.pageUndoToolStripMenuItem.Click += new System.EventHandler(this.pageUndoToolStripMenuItem_Click);
            // 
            // pageRedoToolStripMenuItem
            // 
            this.pageRedoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageRedoToolStripMenuItem.Image")));
            this.pageRedoToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageRedoToolStripMenuItem.Name = "pageRedoToolStripMenuItem";
            this.pageRedoToolStripMenuItem.Size = new System.Drawing.Size(185, 24);
            this.pageRedoToolStripMenuItem.Text = "PageRedo";
            this.pageRedoToolStripMenuItem.Click += new System.EventHandler(this.pageRedoToolStripMenuItem_Click);
            // 
            // toolStripSeparator80
            // 
            this.toolStripSeparator80.Name = "toolStripSeparator80";
            this.toolStripSeparator80.Size = new System.Drawing.Size(182, 6);
            // 
            // pageTableToolStripMenuItem
            // 
            this.pageTableToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pageInsertTableToolStripMenuItem,
            this.toolStripSeparator82,
            this.pageInsertRowToolStripMenuItem,
            this.pageInsertColumnToolStripMenuItem,
            this.toolStripSeparator83,
            this.pageDeleteRowToolStripMenuItem,
            this.pageDeleteColumnToolStripMenuItem,
            this.toolStripSeparator84,
            this.pageSplitRowToolStripMenuItem,
            this.pageSplitColumnToolStripMenuItem,
            this.toolStripSeparator85,
            this.pageMergeCellToolStripMenuItem,
            this.pageMergeCellRightToolStripMenuItem,
            this.pageMergeCellUpToolStripMenuItem,
            this.pageMergeCellDownToolStripMenuItem,
            this.toolStripSeparator86,
            this.pageSelectTableToolStripMenuItem,
            this.pageClearContentToolStripMenuItem});
            this.pageTableToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageTableToolStripMenuItem.Image")));
            this.pageTableToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageTableToolStripMenuItem.Name = "pageTableToolStripMenuItem";
            this.pageTableToolStripMenuItem.Size = new System.Drawing.Size(185, 24);
            this.pageTableToolStripMenuItem.Text = "Table";
            // 
            // pageInsertTableToolStripMenuItem
            // 
            this.pageInsertTableToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageInsertTableToolStripMenuItem.Image")));
            this.pageInsertTableToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageInsertTableToolStripMenuItem.Name = "pageInsertTableToolStripMenuItem";
            this.pageInsertTableToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.pageInsertTableToolStripMenuItem.Text = "PageInsertTable";
            this.pageInsertTableToolStripMenuItem.Click += new System.EventHandler(this.pageInsertTableToolStripMenuItem_Click);
            // 
            // toolStripSeparator82
            // 
            this.toolStripSeparator82.Name = "toolStripSeparator82";
            this.toolStripSeparator82.Size = new System.Drawing.Size(196, 6);
            // 
            // pageInsertRowToolStripMenuItem
            // 
            this.pageInsertRowToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageInsertRowToolStripMenuItem.Image")));
            this.pageInsertRowToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageInsertRowToolStripMenuItem.Name = "pageInsertRowToolStripMenuItem";
            this.pageInsertRowToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.pageInsertRowToolStripMenuItem.Text = "PageInsertRow";
            this.pageInsertRowToolStripMenuItem.Click += new System.EventHandler(this.pageInsertRowToolStripMenuItem_Click);
            // 
            // pageInsertColumnToolStripMenuItem
            // 
            this.pageInsertColumnToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageInsertColumnToolStripMenuItem.Image")));
            this.pageInsertColumnToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageInsertColumnToolStripMenuItem.Name = "pageInsertColumnToolStripMenuItem";
            this.pageInsertColumnToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.pageInsertColumnToolStripMenuItem.Text = "PageInsertColumn";
            this.pageInsertColumnToolStripMenuItem.Click += new System.EventHandler(this.pageInsertColumnToolStripMenuItem_Click);
            // 
            // toolStripSeparator83
            // 
            this.toolStripSeparator83.Name = "toolStripSeparator83";
            this.toolStripSeparator83.Size = new System.Drawing.Size(196, 6);
            // 
            // pageDeleteRowToolStripMenuItem
            // 
            this.pageDeleteRowToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageDeleteRowToolStripMenuItem.Image")));
            this.pageDeleteRowToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageDeleteRowToolStripMenuItem.Name = "pageDeleteRowToolStripMenuItem";
            this.pageDeleteRowToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.pageDeleteRowToolStripMenuItem.Text = "PageDeleteRow";
            this.pageDeleteRowToolStripMenuItem.Click += new System.EventHandler(this.pageDeleteRowToolStripMenuItem_Click);
            // 
            // pageDeleteColumnToolStripMenuItem
            // 
            this.pageDeleteColumnToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageDeleteColumnToolStripMenuItem.Image")));
            this.pageDeleteColumnToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageDeleteColumnToolStripMenuItem.Name = "pageDeleteColumnToolStripMenuItem";
            this.pageDeleteColumnToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.pageDeleteColumnToolStripMenuItem.Text = "PageDeleteColumn";
            this.pageDeleteColumnToolStripMenuItem.Click += new System.EventHandler(this.pageDeleteColumnToolStripMenuItem_Click);
            // 
            // toolStripSeparator84
            // 
            this.toolStripSeparator84.Name = "toolStripSeparator84";
            this.toolStripSeparator84.Size = new System.Drawing.Size(196, 6);
            // 
            // pageSplitRowToolStripMenuItem
            // 
            this.pageSplitRowToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageSplitRowToolStripMenuItem.Image")));
            this.pageSplitRowToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageSplitRowToolStripMenuItem.Name = "pageSplitRowToolStripMenuItem";
            this.pageSplitRowToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.pageSplitRowToolStripMenuItem.Text = "PageSplitRow";
            this.pageSplitRowToolStripMenuItem.Click += new System.EventHandler(this.pageSplitRowToolStripMenuItem_Click);
            // 
            // pageSplitColumnToolStripMenuItem
            // 
            this.pageSplitColumnToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageSplitColumnToolStripMenuItem.Image")));
            this.pageSplitColumnToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageSplitColumnToolStripMenuItem.Name = "pageSplitColumnToolStripMenuItem";
            this.pageSplitColumnToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.pageSplitColumnToolStripMenuItem.Text = "PageSplitColumn";
            this.pageSplitColumnToolStripMenuItem.Click += new System.EventHandler(this.pageSplitColumnToolStripMenuItem_Click);
            // 
            // toolStripSeparator85
            // 
            this.toolStripSeparator85.Name = "toolStripSeparator85";
            this.toolStripSeparator85.Size = new System.Drawing.Size(196, 6);
            // 
            // pageMergeCellToolStripMenuItem
            // 
            this.pageMergeCellToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageMergeCellToolStripMenuItem.Image")));
            this.pageMergeCellToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageMergeCellToolStripMenuItem.Name = "pageMergeCellToolStripMenuItem";
            this.pageMergeCellToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.pageMergeCellToolStripMenuItem.Text = "PageMergeCellLeft";
            this.pageMergeCellToolStripMenuItem.Click += new System.EventHandler(this.pageMergeCellToolStripMenuItem_Click);
            // 
            // pageMergeCellRightToolStripMenuItem
            // 
            this.pageMergeCellRightToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageMergeCellRightToolStripMenuItem.Image")));
            this.pageMergeCellRightToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageMergeCellRightToolStripMenuItem.Name = "pageMergeCellRightToolStripMenuItem";
            this.pageMergeCellRightToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.pageMergeCellRightToolStripMenuItem.Text = "PageMergeCellRight";
            this.pageMergeCellRightToolStripMenuItem.Click += new System.EventHandler(this.pageMergeCellRightToolStripMenuItem_Click);
            // 
            // pageMergeCellUpToolStripMenuItem
            // 
            this.pageMergeCellUpToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageMergeCellUpToolStripMenuItem.Image")));
            this.pageMergeCellUpToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageMergeCellUpToolStripMenuItem.Name = "pageMergeCellUpToolStripMenuItem";
            this.pageMergeCellUpToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.pageMergeCellUpToolStripMenuItem.Text = "PageMergeCellUp";
            this.pageMergeCellUpToolStripMenuItem.Click += new System.EventHandler(this.pageMergeCellUpToolStripMenuItem_Click);
            // 
            // pageMergeCellDownToolStripMenuItem
            // 
            this.pageMergeCellDownToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageMergeCellDownToolStripMenuItem.Image")));
            this.pageMergeCellDownToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageMergeCellDownToolStripMenuItem.Name = "pageMergeCellDownToolStripMenuItem";
            this.pageMergeCellDownToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.pageMergeCellDownToolStripMenuItem.Text = "PageMergeCellDown";
            this.pageMergeCellDownToolStripMenuItem.Click += new System.EventHandler(this.pageMergeCellDownToolStripMenuItem_Click);
            // 
            // toolStripSeparator86
            // 
            this.toolStripSeparator86.Name = "toolStripSeparator86";
            this.toolStripSeparator86.Size = new System.Drawing.Size(196, 6);
            // 
            // pageSelectTableToolStripMenuItem
            // 
            this.pageSelectTableToolStripMenuItem.Name = "pageSelectTableToolStripMenuItem";
            this.pageSelectTableToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.pageSelectTableToolStripMenuItem.Text = "PageSelectTable";
            this.pageSelectTableToolStripMenuItem.Click += new System.EventHandler(this.pageSelectTableToolStripMenuItem_Click);
            // 
            // pageClearContentToolStripMenuItem
            // 
            this.pageClearContentToolStripMenuItem.Name = "pageClearContentToolStripMenuItem";
            this.pageClearContentToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.pageClearContentToolStripMenuItem.Text = "PageClearContent";
            this.pageClearContentToolStripMenuItem.Click += new System.EventHandler(this.pageClearContentToolStripMenuItem_Click);
            // 
            // toolStripSeparator81
            // 
            this.toolStripSeparator81.Name = "toolStripSeparator81";
            this.toolStripSeparator81.Size = new System.Drawing.Size(182, 6);
            this.toolStripSeparator81.Visible = false;
            // 
            // pageNewPartToolStripMenuItem
            // 
            this.pageNewPartToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageNewPartToolStripMenuItem.Image")));
            this.pageNewPartToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageNewPartToolStripMenuItem.Name = "pageNewPartToolStripMenuItem";
            this.pageNewPartToolStripMenuItem.Size = new System.Drawing.Size(185, 24);
            this.pageNewPartToolStripMenuItem.Text = "PageNewPart";
            this.pageNewPartToolStripMenuItem.Visible = false;
            // 
            // pageClonePartToolStripMenuItem
            // 
            this.pageClonePartToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageClonePartToolStripMenuItem.Image")));
            this.pageClonePartToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageClonePartToolStripMenuItem.Name = "pageClonePartToolStripMenuItem";
            this.pageClonePartToolStripMenuItem.Size = new System.Drawing.Size(185, 24);
            this.pageClonePartToolStripMenuItem.Text = "PageClonePart";
            this.pageClonePartToolStripMenuItem.Visible = false;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(182, 6);
            // 
            // EditFormDataMenuItem1
            // 
            this.EditFormDataMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("EditFormDataMenuItem1.Image")));
            this.EditFormDataMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.EditFormDataMenuItem1.Name = "EditFormDataMenuItem1";
            this.EditFormDataMenuItem1.Size = new System.Drawing.Size(185, 24);
            this.EditFormDataMenuItem1.Text = "EditFormData";
            this.EditFormDataMenuItem1.Click += new System.EventHandler(this.EditFormDataMenuItem1_Click);
            // 
            // pageEditTagToolStripMenuItem
            // 
            this.pageEditTagToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pageEditTagToolStripMenuItem.Image")));
            this.pageEditTagToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pageEditTagToolStripMenuItem.Name = "pageEditTagToolStripMenuItem";
            this.pageEditTagToolStripMenuItem.Size = new System.Drawing.Size(185, 24);
            this.pageEditTagToolStripMenuItem.Text = "PageEditTag";
            this.pageEditTagToolStripMenuItem.Click += new System.EventHandler(this.pageEditTagToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
            // 
            // cloneToolStripMenuItem
            // 
            this.cloneToolStripMenuItem.Image = global::SiteMatrix.main.克隆实例;
            this.cloneToolStripMenuItem.Name = "cloneToolStripMenuItem";
            this.cloneToolStripMenuItem.Size = new System.Drawing.Size(185, 24);
            this.cloneToolStripMenuItem.Text = "克隆构件实例";
            this.cloneToolStripMenuItem.Click += new System.EventHandler(this.cloneToolStripMenuItem_Click);
            // 
            // cloneandreplaceToolStripMenuItem
            // 
            this.cloneandreplaceToolStripMenuItem.Image = global::SiteMatrix.main.xjsl;
            this.cloneandreplaceToolStripMenuItem.Name = "cloneandreplaceToolStripMenuItem";
            this.cloneandreplaceToolStripMenuItem.Size = new System.Drawing.Size(185, 24);
            this.cloneandreplaceToolStripMenuItem.Text = "克隆构件实例并替换";
            this.cloneandreplaceToolStripMenuItem.Click += new System.EventHandler(this.cloneandreplaceToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(182, 6);
            // 
            // copypartidToolStripMenuItem
            // 
            this.copypartidToolStripMenuItem.Image = global::SiteMatrix.main.克隆片段;
            this.copypartidToolStripMenuItem.Name = "copypartidToolStripMenuItem";
            this.copypartidToolStripMenuItem.Size = new System.Drawing.Size(185, 24);
            this.copypartidToolStripMenuItem.Text = "复制片段标识";
            this.copypartidToolStripMenuItem.Click += new System.EventHandler(this.copypartidToolStripMenuItem_Click);
            // 
            // CMCode
            // 
            this.CMCode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CodeCut,
            this.CodeCopy,
            this.CodePaste,
            this.CodeDelete,
            this.toolStripSeparator2,
            this.CodeUndo,
            this.CodeRedo});
            this.CMCode.Name = "CMPage";
            this.CMCode.Size = new System.Drawing.Size(144, 154);
            this.CMCode.Opening += new System.ComponentModel.CancelEventHandler(this.CMCode_Opening);
            // 
            // CodeCut
            // 
            this.CodeCut.Image = ((System.Drawing.Image)(resources.GetObject("CodeCut.Image")));
            this.CodeCut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CodeCut.Name = "CodeCut";
            this.CodeCut.Size = new System.Drawing.Size(143, 24);
            this.CodeCut.Text = "PageCut";
            this.CodeCut.Click += new System.EventHandler(this.CodeCut_Click);
            // 
            // CodeCopy
            // 
            this.CodeCopy.Image = ((System.Drawing.Image)(resources.GetObject("CodeCopy.Image")));
            this.CodeCopy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CodeCopy.Name = "CodeCopy";
            this.CodeCopy.Size = new System.Drawing.Size(143, 24);
            this.CodeCopy.Text = "PageCopy";
            this.CodeCopy.Click += new System.EventHandler(this.CodeCopy_Click);
            // 
            // CodePaste
            // 
            this.CodePaste.Image = ((System.Drawing.Image)(resources.GetObject("CodePaste.Image")));
            this.CodePaste.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CodePaste.Name = "CodePaste";
            this.CodePaste.Size = new System.Drawing.Size(143, 24);
            this.CodePaste.Text = "PagePaste";
            this.CodePaste.Click += new System.EventHandler(this.CodePaste_Click);
            // 
            // CodeDelete
            // 
            this.CodeDelete.Image = ((System.Drawing.Image)(resources.GetObject("CodeDelete.Image")));
            this.CodeDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CodeDelete.Name = "CodeDelete";
            this.CodeDelete.Size = new System.Drawing.Size(143, 24);
            this.CodeDelete.Text = "PageDelete";
            this.CodeDelete.Click += new System.EventHandler(this.CodeDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(140, 6);
            // 
            // CodeUndo
            // 
            this.CodeUndo.Image = ((System.Drawing.Image)(resources.GetObject("CodeUndo.Image")));
            this.CodeUndo.Name = "CodeUndo";
            this.CodeUndo.Size = new System.Drawing.Size(143, 24);
            this.CodeUndo.Text = "PageUndo";
            this.CodeUndo.Click += new System.EventHandler(this.CodeUndo_Click);
            // 
            // CodeRedo
            // 
            this.CodeRedo.Image = ((System.Drawing.Image)(resources.GetObject("CodeRedo.Image")));
            this.CodeRedo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CodeRedo.Name = "CodeRedo";
            this.CodeRedo.Size = new System.Drawing.Size(143, 24);
            this.CodeRedo.Text = "PageRedo";
            this.CodeRedo.Click += new System.EventHandler(this.CodeRedo_Click);
            // 
            // ToolStdadadaripMenuItem
            // 
            this.ToolStdadadaripMenuItem.Image = global::SiteMatrix.main.xjpd;
            this.ToolStdadadaripMenuItem.Name = "ToolStdadadaripMenuItem";
            this.ToolStdadadaripMenuItem.Size = new System.Drawing.Size(185, 24);
            this.ToolStdadadaripMenuItem.Text = "复制片段标签";
            this.ToolStdadadaripMenuItem.Click += new System.EventHandler(this.ToolStdadadaripMenuItem_Click);
            // 
            // Editor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(640, 535);
            this.Controls.Add(this.EditSpace);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Editor";
            this.ShowInTaskbar = false;
            this.Text = "Editor";
            this.Activated += new System.EventHandler(this.Editor_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Editor_Closing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Editor_FormClosed);
            this.Load += new System.EventHandler(this.Editor_Load);
            this.LostFocus += new System.EventHandler(this.Editor_LostFocus);
            this.Resize += new System.EventHandler(this.Editor_Resize);
            this.EditSpace.ResumeLayout(false);
            this.tabDesign.ResumeLayout(false);
            this.tabCode.ResumeLayout(false);
            this.tabCode.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.CodeEditTool.ResumeLayout(false);
            this.CodeEditTool.PerformLayout();
            this.tabPreview.ResumeLayout(false);
            this.CMPage.ResumeLayout(false);
            this.CMCode.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private bool SelectedAfterFocus = false;
        private void Editor_LostFocus(object sender, EventArgs e)
        {
            SelectedAfterFocus=false;
        }
        private void textocx_theDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void textocx_theDragDrop(object sender, DragEventArgs e)
        {
            if (!globalConst.MdiForm.ToolBoxBeginDrag) return;
            if (MainForm.imgform.text.StartsWith("{control}"))
            {
                Silver.UI.ToolBoxItem dragNode = (Silver.UI.ToolBoxItem)classes.EditorEvent.DragE.Data.GetData(typeof(Silver.UI.ToolBoxItem));

                MainForm.CloseImageForm();
                switch (globalConst.MdiForm.MainToolBox.SelectedTabIndex)
                {
                    case 0:
                        string WillPasteText = "";
                        switch (dragNode.Object.ToString())
                        {
                            case "Label":
                                WillPasteText = "<label>" + dragNode.Caption + "</label>";
                                break;
                            case "HyperLink":
                                WillPasteText = "<a href=\"\">" + dragNode.Caption + "</a>";
                                break;
                            case "TextBox":
                                WillPasteText="<input type=\"text\">";
                                break;
                            case "TextArea":
                                WillPasteText="<textarea></textarea>";
                                break;
                            case "Password":
                                WillPasteText="<input type=\"password\">";
                                break;
                            case "Button":
                                WillPasteText = "<input type=\"button\" value=\"Button\">";
                                break;
                            case "Button2":
                                WillPasteText = "<button type=\"button\" class=\"_button\">Button</button>";
                                break;
                            case "Submit Button":
                                WillPasteText = "<input type=\"submit\" value=\"" + dragNode.Caption + "\">";
                                break;
                            case "Reset Button":
                                WillPasteText = "<input type=\"reset\" value=\"" + dragNode.Caption + "\">";
                                break;
                            case "Image Button":
                                WillPasteText="<input type=\"image\">";
                                break;
                            case "CheckBox":
                                WillPasteText="<input type=\"checkbox\">";
                                break;
                            case "Redio Button":
                                WillPasteText="<input type=\"radio\">";
                                break;
                            case "ComboBox":
                                WillPasteText="<select></select>";
                                break;
                            case "ListBox":
                                WillPasteText="<select size=\"3\"></select>";
                                break;
                            case "Hidden Field":
                                WillPasteText="<input type=\"hidden\">";
                                break;
                            case "File Upload":
                                WillPasteText="<input type=\"file\">";
                                break;
                            case "GroupBox":
                                WillPasteText = "<fieldset><legend>" + dragNode.Caption + "<legend></legend></legend></fieldset>";
                                break;
                            case "Anchor":
                                WillPasteText = "<a href=\"\">" + dragNode.Caption + "</a>";
                                break;
                            case "Image":
                                WillPasteText="<img src=\"\">";
                                break;
                            case "Table":
                                WillPasteText = "<table><tbody><tr><td>" + dragNode.Caption + "</td><td></td><td></td></tr><tr><td></td><td></td><td></td></tr><tr><td></td><td></td><td></td></tr></tbody></table>";
                                break;
                            case "Span":
                                WillPasteText = "<span>" + dragNode.Caption + "</span>";
                                break;
                            case "Div":
                                WillPasteText = "<div>" + dragNode.Caption + "</div>";
                                break;
                            case "Panel":
                                WillPasteText = "<div style=\"WIDTH: 100px; HEIGHT: 100px\">" + dragNode.Caption + "</div>";
                                break;
                            case "IFrame":
                                WillPasteText="<iframe style=\"WIDTH: 300px; HEIGHT: 138px\"></iframe>";
                                break;
                            case "Horizontal Rule":
                                WillPasteText="<hr>";
                                break;
                            case "Form":
                                WillPasteText="<form></form>";
                                break;
                            default: break;
                        }
                        Clipboard.SetText(WillPasteText);
                        textocx.doPaste();
                        break;
                    case 1: break;
                    case 2: break;
                    case 3: break;
                    case 4:
                        string sql = "select content from snippets where id=" + dragNode.Object.ToString();
                        editocx.pasteHtml(globalConst.ConfigConn.GetString(sql));
                        break;
                }
                //if (!PageWare.AddPart2Editor(tree.getID(dragNode), this))
                //{
                //    MsgBox.Error("add part to page failed!");
                //}
            }
            //classes.EditorEvent.MoveEvents();
            globalConst.MdiForm.ToolBoxBeginDrag = false;
        }

        void editocx_ondragstart()
        {
            try
            {
                if (EventAdded)
                {
                    EventAdded = false;
                    iEvent.ondrop -= new HTMLTextContainerEvents2_ondropEventHandler(iEvent_ondrop);
                    iEvent.ondragover -= new HTMLTextContainerEvents2_ondragoverEventHandler(iEvent_ondragover);
                    iEvent.ondragenter -= new HTMLTextContainerEvents2_ondragenterEventHandler(iEvent_ondragenter);
                    iEvent.ondragleave -= new HTMLTextContainerEvents2_ondragleaveEventHandler(iEvent_ondragleave);
                }
            }
            catch { }
        }




        #endregion
        private void InitCustomFonts()
        {
            TheFontName.Items.Clear();
            for (int i = 1; i <= 18; i++)
            {
               TheFontName.Items.Add("Font " + i);
            }
        }
        public void ApplyLanguage()
        {
            SelectionMargin.ToolTipText=res.Editor.GetString("SelectionMargin");
            WordWrap.ToolTipText=res.Editor.GetString("WordWrap");
            BGColor.ToolTipText=res.Editor.GetString("BGColor");
            TheFontName.ToolTipText=res.Editor.GetString("TheFontName");
            TheFontSize.ToolTipText=res.Editor.GetString("TheFontSize");
            toolStripLabel1.Text=res.Editor.GetString("toolStripLabel1");
            toolStripLabel2.Text=res.Editor.GetString("toolStripLabel2");

            pageCutToolStripMenuItem.Text = res.Editor.GetString("pageCutToolStripMenuItem");
            pageCopyToolStripMenuItem.Text = res.Editor.GetString("pageCopyToolStripMenuItem");
            pagePasteToolStripMenuItem.Text = res.Editor.GetString("pagePasteToolStripMenuItem");
            PageDelete.Text = res.Editor.GetString("PageDelete");
            pageUndoToolStripMenuItem.Text = res.Editor.GetString("pageUndoToolStripMenuItem");
            pageRedoToolStripMenuItem.Text = res.Editor.GetString("pageRedoToolStripMenuItem");
            pageTableToolStripMenuItem.Text = res.Editor.GetString("pageTableToolStripMenuItem");
            pageInsertTableToolStripMenuItem.Text = res.Editor.GetString("pageInsertTableToolStripMenuItem");
            pageInsertRowToolStripMenuItem.Text = res.Editor.GetString("pageInsertRowToolStripMenuItem");
            pageInsertColumnToolStripMenuItem.Text = res.Editor.GetString("pageInsertColumnToolStripMenuItem");
            pageDeleteRowToolStripMenuItem.Text = res.Editor.GetString("pageDeleteRowToolStripMenuItem");
            pageDeleteColumnToolStripMenuItem.Text = res.Editor.GetString("pageDeleteColumnToolStripMenuItem");
            pageSplitRowToolStripMenuItem.Text = res.Editor.GetString("pageSplitRowToolStripMenuItem");
            pageSplitColumnToolStripMenuItem.Text = res.Editor.GetString("pageSplitColumnToolStripMenuItem");
            pageMergeCellToolStripMenuItem.Text = res.Editor.GetString("pageMergeCellToolStripMenuItem");
            pageMergeCellRightToolStripMenuItem.Text = res.Editor.GetString("pageMergeCellRightToolStripMenuItem");
            pageMergeCellUpToolStripMenuItem.Text = res.Editor.GetString("pageMergeCellUpToolStripMenuItem");
            pageMergeCellDownToolStripMenuItem.Text = res.Editor.GetString("pageMergeCellDownToolStripMenuItem");
            pageSelectTableToolStripMenuItem.Text = res.Editor.GetString("pageSelectTableToolStripMenuItem");
            pageClearContentToolStripMenuItem.Text = res.Editor.GetString("pageClearContentToolStripMenuItem");
            pageEditTagToolStripMenuItem.Text = res.Editor.GetString("pageEditTagToolStripMenuItem");
            EditFormDataMenuItem1.Text = res.Editor.GetString("editformdatamenu");
            

            CodeCut.Text = res.Editor.GetString("CodeCut");
            CodeCopy.Text = res.Editor.GetString("CodeCopy");
            CodePaste.Text = res.Editor.GetString("CodePaste");
            CodeDelete.Text = res.Editor.GetString("CodeDelete");
            CodeUndo.Text = res.Editor.GetString("CodeUndo");
            CodeRedo.Text = res.Editor.GetString("CodeRedo");

            tabDesign.Text = res.Editor.GetString("tabDesign");
            tabCode.Text = res.Editor.GetString("tabCode");
            tabPreview.Text = res.Editor.GetString("tabPreview");
        }
        public static string GetFontName(int SelectIndex)
        {
            switch (SelectIndex)
            {
                case 0: return "宋体"; //return "仿宋_GB2312";
                case 1: return "微软雅黑";
                case 2: return "宋体";
                case 3: return "宋体";
                case 4: return "宋体";
                case 5: return "宋体";
                case 6: return "宋体";
                case 7: return "宋体";
                case 8: return "宋体"; //return "宋体-18030";
                case 9: return "宋体"; //return "宋体-方正超大字符集";
                case 10: return "宋体";
                case 11: return "新宋体";
                case 12: return "宋体"; //return "新宋体-18030";
                case 13: return "宋体";
                case 14: return "宋体";
                case 15: return "宋体"; //return "楷体_GB2312";
                case 16: return "宋体";
                case 17: return "黑体";
            }
            return "";
        }
        private void Editor_Load(object sender, System.EventArgs e)
        {
            doResize();
            editocx.Visible = false;
            textocx.Visible = false;
            viewocx.Visible = false;
            editor_timer.Enabled = true;
        }


        private void getSystemFonts()
        {
            try
            {
                int i;
                for (i = 0; i < globalConst.SysFonts.Families.Length; i++)
                {
                    //FontsCom.Items.Add(globalConst.SysFonts.Families[i].Name);
                }

            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public void OutEdited()
        {
            switch (editmode)
            {
                case "edit":
                    //next line add 2005-7-7,alice,转换为utf-8文档
                    //if (!SiteClass.Site.constructTextPageFromText(thisID, thisUrl)) { MsgBox.Warning("Construct Failed,Please Retry"); return; };
                    if (!isFreeFile)
                    {
                        if (!SiteClass.Site.constructEditPageFromText(thisID, thisUrl)) { MsgBox.Warning("Construct Failed,Please Retry"); return; };
                        PageWare.refreshPartsInSiteTree(thisID);
                    }
                    //text改变才需要刷新
                    editocx.RefreshDocument();
                    while (editocx.readyState().Equals("loading"))
                    {
                        Application.DoEvents();
                    }
                    if(form.getEditor().thisID==thisID)classes.PageAsist.RefreshToolBoxPages();
                    INITxtRange = ((mshtml.HTMLBody)editocx.body).createTextRange();
                    INICtlRange = (IHTMLControlRange)((mshtml.HTMLBody)editocx.body).createControlRange();
                    iEvent = (HTMLTextContainerEvents2_Event)(HTMLBody)(((HTMLDocument)(editocx.DOM)).body);
                    lastSavedHTML = editocx.DocumentHTML; 
                    break;
                case "text":
                    textocx.Visible = false;
                    CodeEditTool.Visible = false;
                    textocx.OpenFile(ref thisUrl);
                    textocx.Color_All();
                    //selectmation 必须在打开之后处理，要不然会对wordwrap有问题
                    CodeEditTool.Visible = true;
                    textocx.Visible = true;
                    textChanged = true;
                    textocx.ShowSelectionMargin = this.SelectionMargin.Checked;
                    break;
            }
        }
        public void change_editmode(string mode)//mode must be:one of in edit/text/view
        {
            try
            {
                if (editmode.Equals(mode)) return;
                switch (mode)
                {
                    case "edit":
                        //这里添加由于部件编辑和状态切换带来的判断以及文件转换等代码,add by maobb,2005-5-27 0:58:11
                        str.ShowStatus(res.Editor.GetString("s4"));
                        if (editmode.Equals("text") && textChanged)
                        {
                            lastSavedHTML = "";
                            textocx.SaveFile(ref thisUrl);
                            textChanged = false;
                            //next line add 2005-7-7,alice,转换为utf-8文档
                            if (!SiteClass.Site.constructTextPageFromText(thisID, thisUrl)) { MsgBox.Warning("Construct Failed,Please Retry"); return; };
                            if (!isFreeFile)
                            {
                                if (!SiteClass.Site.constructEditPageFromText(thisID, thisUrl)) { MsgBox.Warning("Construct Failed,Please Retry"); return; };
                                PageWare.refreshPartsInSiteTree(thisID);
                            }
                            //text改变才需要刷新
                            editocx.RefreshDocument();
                            while (editocx.readyState().Equals("loading"))
                            {
                                Application.DoEvents();
                            }
                            if (form.getEditor().thisID == thisID) classes.PageAsist.RefreshToolBoxPages();
                            INITxtRange = ((mshtml.HTMLBody)editocx.body).createTextRange();
                            INICtlRange = (IHTMLControlRange)((mshtml.HTMLBody)editocx.body).createControlRange();
                            iEvent = (HTMLTextContainerEvents2_Event)(HTMLBody)(((HTMLDocument)(editocx.DOM)).body);
                            lastSavedHTML = editocx.DocumentHTML;
                        }
                        editocx.Visible = true;
                        globalConst.MdiForm.CurPropTag.Enabled = true;
                        globalConst.MdiForm.ProOthers.Enabled = true;
                        //textocx.Visible = false;
                        //viewocx.Visible = false;
                        break;
                    case "text":
                        //这里添加由于部件编辑和状态切换带来的判断以及文件转换等代码,add by maobb,2005-5-27 0:58:11
                        str.ShowStatus(res.Editor.GetString("s2"));
                        textocx.Visible = false;
                        CodeEditTool.Visible = false;
                        globalConst.MdiForm.CurPropTag.Text = "";
                        globalConst.MdiForm.ProOthers.Text = "";
                        globalConst.MdiForm.CurPropTag.Enabled = false;
                        globalConst.MdiForm.ProOthers.Enabled = false;
                        globalConst.MdiForm.PropGrid.SelectedObject = null;
                        if (editmode.Equals("edit"))
                        {
                            //有改动才需要重新生成code
                            if (!lastSavedHTML.Equals(editocx.DocumentHTML))
                            {
                                editocx.SaveDocument(thisEditUrl, false);
                                if (!isFreeFile)
                                {
                                    if (!SiteClass.Site.constructTextPageFromEdit(thisID, thisUrl)) { MsgBox.Warning("Construct Failed,Please Retry"); return; };
                                    PageWare.refreshPartsInSiteTree(thisID);
                                }
                                lastSavedHTML = editocx.DocumentHTML;
                            }
                        }
                        //editocx.Visible = false;
                        //viewocx.Visible = false;
                        //7-5 修改了这个方法，能打开unicode和utf8格式文件
                        textocx.OpenFile(ref thisUrl);
                        //7-3 增加了这个方法，color html tag，用正则表达式和内存指针操作快速进行字符串替换，
                        //比普通字符串替换快 100 倍以上
                        textocx.Color_All();
                        //selectmation 必须在打开之后处理，要不然会对wordwrap有问题
                        CodeEditTool.Visible = true;
                        textocx.Visible = true;
                        textChanged = false;
                        textocx.ShowSelectionMargin = this.SelectionMargin.Checked;
                        break;
                    case "view":
                        //这里添加由于部件编辑和状态切换带来的判断以及文件转换等代码,add by maobb,2005-5-27 0:58:11
                        str.ShowStatus(res.Editor.GetString("s3"));
                        globalConst.MdiForm.CurPropTag.Text = "";
                        globalConst.MdiForm.ProOthers.Text = "";
                        globalConst.MdiForm.CurPropTag.Enabled = false;
                        globalConst.MdiForm.ProOthers.Enabled = false;
                        globalConst.MdiForm.PropGrid.SelectedObject = null;
                        if (editmode.Equals("text"))
                        {
                            textocx.SaveFile(ref thisUrl);
                            textChanged = false;
                            //next line add 2005-7-7,alice,转换为utf-8文档
                            if (!SiteClass.Site.constructTextPageFromText(thisID, thisUrl)) { MsgBox.Warning("Construct Failed,Please Retry"); return; };
                            if (!isFreeFile)
                            {
                                if (!SiteClass.Site.constructViewPageFromText(thisID, thisUrl)) { MsgBox.Warning("Construct Failed,Please Retry"); return; };
                                if (!SiteClass.Site.constructEditPageFromText(thisID, thisUrl)) { MsgBox.Warning("Construct Failed,Please Retry"); return; };
                                PageWare.refreshPartsInSiteTree(thisID);
                            }
                        }
                        if (editmode.Equals("edit"))
                        {
                            
                            editocx.SaveDocument(thisEditUrl, false);
                            if (!isFreeFile)
                            {
                                if (!SiteClass.Site.constructViewPageFromEdit(thisID, thisUrl)) { MsgBox.Warning("Construct Failed,Please Retry"); return; };
                                if (!SiteClass.Site.constructTextPageFromEdit(thisID, thisUrl)) { MsgBox.Warning("Construct Failed,Please Retry"); return; };
                                PageWare.refreshPartsInSiteTree(thisID);
                            }
                            lastSavedHTML = editocx.DocumentHTML;
                        }
                        //editocx.Visible = false;
                        //textocx.Visible = false;
   
                        if (HasViewed)
                        {
                            viewocx.Visible = true;
                            viewocx.LoadDocumentAsIE(thisViewUrl);                 
                        }
                        else
                        {
                            viewocx.Visible = false;
                            viewocx.LoadDocumentAsIE(thisViewUrl);
                            viewocx.Visible = true;
                            viewocx.LoadDocumentAsIE(thisViewUrl);
                            HasViewed = true;
                        }
                        break;
                }
                editmode = mode;
                globalConst.MdiForm.UpdateMenusAndToolBars4Page();
            }
            catch (Exception exx)
            {
                new error(exx);
            }
            finally
            {
                str.StatusClear();
            }
        }



        private void Editor_Activated(object sender, System.EventArgs e)
        {
            globalConst.curActiveForm = this;
            if (EventAdded)
            {
                iEvent.ondrop -= new HTMLTextContainerEvents2_ondropEventHandler(iEvent_ondrop);
                iEvent.ondragover -= new HTMLTextContainerEvents2_ondragoverEventHandler(iEvent_ondragover);
                EventAdded = false;
            }
            PageWare.refreshPartsInSiteTreeNoCtrl(thisID);
            globalConst.MdiForm.T1Design.Checked = false;
            globalConst.MdiForm.T1Code.Checked = false;
            globalConst.MdiForm.T1Preview.Checked = false;
            globalConst.MdiForm.MenuViewDesign.Checked = false;
            globalConst.MdiForm.MenuViewCode.Checked = false;
            globalConst.MdiForm.MenuViewPreview.Checked = false;
            switch (EditSpace.SelectedIndex)
            {
                case 0: change_editmode("edit"); globalConst.MdiForm.T1Design.Checked = true; globalConst.MdiForm.MenuViewDesign.Checked = true; break;
                case 1: change_editmode("text"); globalConst.MdiForm.T1Code.Checked = true; globalConst.MdiForm.MenuViewCode.Checked = true; break;
                case 2: change_editmode("view"); globalConst.MdiForm.T1Preview.Checked = true; globalConst.MdiForm.MenuViewPreview.Checked = true; break;
                default: break;
            }
            globalConst.MdiForm.UpdateMenusAndToolBars4Page();
            if (PageAssis.PageAssisShow) PageAssis.Analys();
            classes.PageAsist.RefreshToolBoxPages();
        }



        private void textocx_TextChange()
        {
            this.textChanged = true;
        }







        private bool JustClosing = false;
        private void Editor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!LoadComplete)
                {
                    e.Cancel = true;
                    return;
                }
                //save change
                JustClosing = true;
                if (!savePage())
                {
                    e.Cancel = true;
                    JustClosing = false;
                    return;
                }
                JustClosing = false;
                //save edit shell change
                if (ScEditSheelEdit4Margin)
                {
                    string sceditmargin;
                    if (textocx.ShowSelectionMargin) sceditmargin = "1";
                    else sceditmargin = "0";
                    string sql = "update system set thevalue='" + sceditmargin + "' where name='sceditmargin'";
                    globalConst.ConfigConn.execSql(sql);
                }
                if (ScEditSheelEdit4BgColor)
                {
                    string sceditbgcolor = ColorTranslator.ToHtml(textocx.BgColor);
                    string sql = "update system set thevalue='" + sceditbgcolor + "' where name='sceditbgcolor'";
                    globalConst.ConfigConn.execSql(sql);
                }
                if (ScEditSheelEdit4Wraped)
                {
                    string sceditwraped;
                    if (textocx.WordWrap) sceditwraped = "1";
                    else sceditwraped = "0";
                    string sql = "update system set thevalue='" + sceditwraped + "' where name='sceditwraped'";
                    globalConst.ConfigConn.execSql(sql);
                }
                if (ScEditSheelEdit4FontSize)
                {
                    int sceditsize = int.Parse(textocx.theFontSize.ToString());
                    string sql = "update system set thevalue='" + sceditsize + "' where name='sceditsize'";
                    globalConst.ConfigConn.execSql(sql);
                }
                if (ScEditSheelEdit4FontName)
                {
                    string sceditfont = this.TheFontName.SelectedIndex.ToString();
                    string sql = "update system set thevalue='" + sceditfont + "' where name='sceditfont'";
                    globalConst.ConfigConn.execSql(sql);
                }
                if (form.getEditorCount() <= 1)
                {
                    globalConst.MdiForm.CurPropTag.Text = "";
                    globalConst.MdiForm.CurPropTag.Enabled = false;
                    globalConst.MdiForm.ProOthers.Text = "";
                    globalConst.MdiForm.ProOthers.Enabled = false;
                    globalConst.MdiForm.PropGrid.SelectedObject = null;
                }
                form.UpdateFileOpend(thisID, false);
                    globalConst.MdiForm.ProOthers.Enabled = false;
                    globalConst.MdiForm.CurPropTag.Enabled = false;
                if(isFreeFile&&isFreeFileSaved)
                {
                    SiteClass.Site.InsertLastList("file",this.thisUrl);
                }
            }
            catch (Exception exx)
            {
                new error(exx);
            }
        }
        public bool savePage()
        {
            try
            {
                if (!isFreeFile)
                {
                    str.ShowStatus(res.Editor.GetString("s1") + this.Text);
                    if (editmode.Equals("text"))
                    {
                        if (textChanged)
                        {
                            textocx.SaveFile(ref thisUrl);
                            //textChanged = false;
                            //next line add 2005-7-7,alice,转换为utf-8文档
                            if(!SiteClass.Site.constructTextPageFromText(thisID, thisUrl)){ MsgBox.Warning("Construct Failed,Please Retry"); return false; };
                            if(!SiteClass.Site.constructEditPageFromText(thisID, thisUrl)) { MsgBox.Warning("Construct Failed,Please Retry"); return false; };
                            //设置修改时间，设置为一样的时间，在打开时可快速打开
                            DateTime curTime = DateTime.Now;
                            (new System.IO.FileInfo(thisUrl)).LastWriteTime = curTime;
                            (new System.IO.FileInfo(thisUrl + "_edit.htm")).LastWriteTime = curTime;
                            UrlLastModTime = curTime;
                        }
                    }
                    else if (editmode.Equals("edit"))
                    {
                        if (!lastSavedHTML.Equals(editocx.DocumentHTML))
                        {
                            editocx.SaveDocument(thisEditUrl, false);
                            if(!SiteClass.Site.constructTextPageFromEdit(thisID, thisUrl)) { MsgBox.Warning("Construct Failed,Please Retry"); return false; };
                            DateTime curTime = DateTime.Now;
                            (new System.IO.FileInfo(thisUrl)).LastWriteTime = curTime;
                            (new System.IO.FileInfo(thisUrl + "_edit.htm")).LastWriteTime = curTime;
                            UrlLastModTime = curTime;
                            lastSavedHTML = editocx.DocumentHTML;
                        }
                    }
                    PageWare.refreshPartsInSiteTree(thisID);
                }
                else
                {
                    if (!isFreeFileSaved)
                    {
                        if (JustClosing)
                        {
                            DialogResult dr = MsgBox.YesNoCancel(res.Editor.GetString("m1"));
                            if (dr.Equals(DialogResult.Yes))
                            {
                                SaveFileDialog sfd = new SaveFileDialog();
                                sfd.Filter = "Html Files(*.htm;*.html)|*.htm;*.html|Web Files(*.asp;*.aspx;*.jsp;*.php;*.php3;)|*.asp;*.aspx;*.jsp;*.php;*.php3;|All Files(*.*)|*.*";
                                sfd.ShowDialog();
                                if (!sfd.FileName.Equals(""))
                                {
                                    thisUrl = sfd.FileName;
                                    isFreeFileSaved = true;
                                    this.Text = "Free Page - " + thisUrl;
                                    form.UpdateFileOpend(thisID,true);
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (dr.Equals(DialogResult.Cancel))
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.Filter = "Html Files(*.htm;*.html)|*.htm;*.html|Web Files(*.asp;*.aspx;*.jsp;*.php;*.php3;)|*.asp;*.aspx;*.jsp;*.php;*.php3;|All Files(*.*)|*.*";
                            sfd.ShowDialog();
                            if (!sfd.FileName.Equals(""))
                            {
                                thisUrl = sfd.FileName;
                                isFreeFileSaved = true;
                                this.Text = "Free Page - " + thisUrl;
                                form.UpdateFileOpend(thisID, true);
                            }
                            else
                            {
                                return false;
                            }
                        }
                        
                    }
                    if (isFreeFileSaved)
                    {
                        str.ShowStatus(res.Editor.GetString("s1") + this.thisUrl);
                        if (editmode.Equals("text"))
                        {
                            if (textChanged)
                            {
                                try
                                {
                                    textocx.SaveFile(ref thisUrl);
                                }
                                catch
                                {
                                    MsgBox.Error(res.Editor.GetString("m2"));
                                }
                                textChanged = false;
                            }
                        }
                        else if (editmode.Equals("edit"))
                        {
                            editocx.SaveDocument(thisUrl, false);
                        }
                    }
                }
                return true;
            }
            catch (Exception exx)
            {
                new error(exx);
                return false;
            }
            finally
            {
                str.StatusClear();
            }
        }
        private void editor_timer_Tick(object sender, System.EventArgs e)
        {
            editor_timer.Enabled = false;
            thisUrl = thisUrl.Replace(@"\\", @"\");
            form.UpdateFileOpend(thisID, true);
            try
            {
                if (!isFreeFile)
                {
                    thisEditUrl = thisUrl + "_edit.htm";
                    thisViewUrl = thisUrl + "_view.htm";
                }
                else
                {
                    thisEditUrl = thisUrl;
                    thisViewUrl = thisUrl;
                }
                editocx.isShowTableZeroBorders = true;
                editocx.Visible = true;
                editocx.LoadDocument(thisEditUrl);
                if (globalConst.MdiForm.T1Tag.Checked)
                {
                    editocx.showGlyphs = true;
                    editocx.RefreshEditState4Glyphs();
                }
                if (!globalConst.MdiForm.T1Border.Checked)
                {
                    editocx.ShowTableZeroBorders(false);
                }
                if (globalConst.MdiForm.T1Grid.Checked)
                {
                    editocx.showBodyNetCells = true;
                    editocx.RefreshEditState4NetCells();
                }
                if (globalConst.MdiForm.T1Live.Checked)
                {
                    editocx.showLiveResizes = true;
                    editocx.RefreshEditState4LiveResize();
                }
                //editocx.LoadDocument(globalConst.ConfigPath + "\\empty.htm");
                //editocx.RefreshDocument();

                //				while(editocx.readyState().Equals("loading"))
                //				{
                //					Application.DoEvents();
                //				}

                globalConst.MdiForm.CurPropTag.Enabled = true;
                globalConst.MdiForm.ProOthers.Enabled = true;

                //editocx.RefreshDocument();
                while (editocx.readyState().Equals("loading"))
                {
                    Application.DoEvents();
                }
                
                editocx.Visible = true;

                
                getSystemFonts();
                //get EditScShell from table
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
                this.WordWrap.Checked = textocx.WordWrap;

                sql = "select thevalue from system where name='sceditmargin'";
                if (globalConst.ConfigConn.GetString(sql).ToLower().Equals("1"))
                {
                    this.SelectionMargin.Checked = true;
                }
                else
                {
                    this.SelectionMargin.Checked = false;
                }
                sql = "select thevalue from system where name='sceditbgcolor'";
                textocx.BgColor = ColorTranslator.FromHtml(globalConst.ConfigConn.GetString(sql));
                sql = "select thevalue from system where name='sceditsize'";
                textocx.theFontSize = int.Parse(globalConst.ConfigConn.GetString(sql));
                this.TheFontSize.Text = textocx.theFontSize.ToString();
                sql = "select thevalue from system where name='sceditfont'";
                int FontID = int.Parse(globalConst.ConfigConn.GetString(sql));
                textocx.theFontName = Editor.GetFontName(FontID);
                this.TheFontName.SelectedIndex = FontID;
                textocx.theText = "Loading...";
                //textocx.Color_All();
                //end
                ////////
                if (thisUrl.ToLower().Trim().StartsWith("http"))
                {
                    thisUrl = globalConst.FreeFilesPath + "/" + thisID;
                    thisEditUrl = thisUrl;
                    thisViewUrl = thisUrl;
                    Application.DoEvents();
                    editocx.SaveDocument(thisUrl, false);
                    editocx.LoadDocument(thisUrl);
                    while (editocx.readyState().Equals("loading"))
                    {
                        Application.DoEvents();
                    }
                    iEvent = (HTMLTextContainerEvents2_Event)(HTMLBody)(((HTMLDocument)(editocx.DOM)).body);
                }
                //////////
                ScEditSheelEdit4Margin = false;
                ScEditSheelEdit4BgColor = false;
                ScEditSheelEdit4Wraped = false;
                ScEditSheelEdit4FontSize = false;
                ScEditSheelEdit4FontName = false;

                globalConst.MdiForm.UpdateMenusAndToolBars4Page();
                //解决win7，xp编辑器兼容性问题
                if (SiteClass.Site.JustEdit(this.thisID))
                {
                    ((IHTMLDocument2)editocx.DOM).designMode = "On";
                    while (editocx.readyState().Equals("loading"))
                    {
                        Application.DoEvents();
                    }
                }
                try
                {
                    iEvent = (HTMLTextContainerEvents2_Event)(HTMLBody)(((HTMLDocument)(editocx.DOM)).body);
                    INITxtRange = ((mshtml.HTMLBody)editocx.body).createTextRange();
                    INICtlRange = (IHTMLControlRange)((mshtml.HTMLBody)editocx.body).createControlRange();
                }
                catch
                {
                    MsgBox.Error(res.Editor.GetString("m3"));
                    LoadComplete = true;
                    this.Close();
                    return;
                }
                LoadComplete = true;
                //切换导致频繁保存问题
                lastSavedHTML = editocx.DocumentHTML;
            }
            catch (Exception exx)
            {
                new error(exx);
                LoadComplete = true;
            }
        }

        public void doPropertyAdapter()
        {
            mshtml.IHTMLElement ele=null;
            //解决getcurelement 方法的bug
            if (globalConst.MdiForm.HasJustSelectChange)
            {
                ele = globalConst.MdiForm.CurEle;
            }
            else
            {
                try
                {
                    ele = editocx.getCurElement();
                }
                catch { }
            }
            if (ele == null) return;

            if (DataOpDefine.DataOpFormShow)
            {
                DataOpDefine.EleDyAdd(ele);
            }
            if (DyValueDefine.DyValueFormShow)
            {
                DyValueDefine.EleDyAdd(ele);
            }
            EleAssis.EleSelected(ele);

            globalConst.MdiForm.CurPropTag.Items.Clear();
            string partid = "";
            if (Page.PageWare.isPartElement(ele))
            {
                partid = Adapter.Property.PropertyAdapter.getEleAttr(ele, "idname");
            }
            else
            {
                if (Page.PageWare.isFormElement(ele))
                {
                    if (ele.tagName.ToLower().Equals("input"))
                    {
                        globalConst.MdiForm.CurPropTag.Items.Add(res.form.GetString("String4") + "<" + ele.tagName + " type=\"" + Adapter.Property.PropertyAdapter.getEleAttr(ele, "type").ToLower() + "\">");
                    }
                    else
                    {
                        globalConst.MdiForm.CurPropTag.Items.Add(res.form.GetString("String47") + "<" + ele.tagName + ">");
                    }
                    if (ele.tagName.ToLower().Equals("input"))
                    {
                        globalConst.MdiForm.CurPropTag.Items.Add("<" + ele.tagName + " type=\"" + Adapter.Property.PropertyAdapter.getEleAttr(ele, "type").ToLower() + "\">");
                    }
                    else
                    {
                        globalConst.MdiForm.CurPropTag.Items.Add("<" + ele.tagName + ">");
                    }
                }
                else
                {
                    if (ele.tagName.ToLower().Equals("input"))
                    {
                        globalConst.MdiForm.CurPropTag.Items.Add("<" + ele.tagName + " type=\"" + Adapter.Property.PropertyAdapter.getEleAttr(ele, "type").ToLower() + "\">");
                    }
                    else
                    {
                        globalConst.MdiForm.CurPropTag.Items.Add("<" + ele.tagName + ">");
                    }
                }
            }
            //
            try
            {
                if (!isFreeFile&&!thisID.Equals(""))
                {
                    string psql = "select a.partid as partid,b.name as name,c.caption as caption,c.name as controlname from part_in_page a,parts b,controls c where a.pageid='" + thisID + "' and a.partid=b.id and c.id=b.controlid";
                    DR pdr = new DR(globalConst.CurSite.SiteConn.OpenRecord(psql));
                    while (pdr.Read())
                    {
                        string thisctrlcaption = pdr.getString("caption");
                        string controlname = pdr.getString("controlname");
                        string partname = pdr.getString("name");
                        string controlcaption = "{NoCtrl}";
                        string partcaption = "{NoPart}";
                        string csql = "select a.caption as controlcaption,b.caption as partcaption from controls a,parts b where a.name='" + controlname + "' and a.name=b.controlname and b.name='" + partname + "'";
                        DR cpdr = new DR(globalConst.ConfigConn.OpenRecord(csql));
                        if (cpdr.Read())
                        {
                            controlcaption = cpdr.getString("controlcaption");
                            partcaption = cpdr.getString("partcaption");

                            globalConst.MdiForm.CurPropTag.Items.Add(new imageComboBoxItem("[C]" + thisctrlcaption + "_" + controlcaption + "_" + partcaption, pdr.getString("partid")));
                        }
                        cpdr.Close();

                    }
                    pdr.Close();
                }
                //
                if (!partid.Equals(""))
                {
                    int i;
                    for (i = 1; i < globalConst.MdiForm.CurPropTag.Items.Count; i++)
                    {
                        if (((imageComboBoxItem)globalConst.MdiForm.CurPropTag.Items[i]).ImageIndex.Equals(partid))
                        {
                            globalConst.MdiForm.HasJustFromAdapter = true;
                            globalConst.MdiForm.CurPropTag.SelectedIndex = i;
                            goto LoopOut;
                        }
                    }
                }
                globalConst.MdiForm.HasJustFromAdapter = true;
                if (globalConst.MdiForm.CurPropTag.Items.Count > 0)
                {
                    globalConst.MdiForm.CurPropTag.SelectedIndex = 0;
                }

            LoopOut:
                ;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
            //
            mshtml.IHTMLElement elep = ele;
            globalConst.MdiForm.ProOthers.Enabled = true;
            globalConst.MdiForm.ProOthers.Items.Clear();
            globalConst.MdiForm.ProOthers.Items.Add(res._globalconst.GetString("pe"));
            elep = elep.parentElement;
            while (elep != null && !elep.tagName.Equals("HTML"))
            {
                globalConst.MdiForm.ProOthers.Items.Add("<" + elep.tagName + ">");
                elep = elep.parentElement;
            }
            globalConst.MdiForm.CurEle = ele;
            globalConst.MdiForm.ProOthers.SelectedIndex = 0;

            Page.PageWare.doHtmlAdapter(ele);
        }
        private void editocx_onselectionchange()
        {
            editocx_onselectionchange(editocx.getCurElement());
        }
        public void editocx_onselectionchange(IHTMLElement ele)
        {
            if (!globalConst.MdiForm.ProOthers.Enabled)
            globalConst.MdiForm.ProOthers.Enabled = true;
            if (!globalConst.MdiForm.CurPropTag.Enabled)
            globalConst.MdiForm.CurPropTag.Enabled = true;
            try
            {
                if (CtlJustSelected)
                {
                    CtlJustSelected = !CtlJustSelected;
                    return;
                }
                if (!Page.PageWare.isPartElement(ele))
                {
                    IHTMLElement PartEle = Page.PageWare.getPartElement(ele);
                    if (PartEle != null)
                    {
                        CtlJustSelected = true;
                        INICtlRange.add((IHTMLControlElement)PartEle);
                        INICtlRange.select();
                        INICtlRange.remove(INICtlRange.length - 1);
                        CtlJustSelected = false;
                    }
                }
                else
                {
                    CtlJustSelected = true;
                    INICtlRange.add((IHTMLControlElement)ele);
                    INICtlRange.select();
                    INICtlRange.remove(INICtlRange.length - 1);
                    CtlJustSelected = false;
                }
            }
            catch(Exception ex)
            {

            }
            if (!SelectedAfterFocus)
            {
                SelectedAfterFocus = true;
                globalConst.CurSelectionPage = this.thisUrl;
            }
            doPropertyAdapter();
            //////query stat
            try
            {
                if (editocx.queryCMDState(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Bold))
                {
                    globalConst.MdiForm.T2Bold.Checked = true;
                    globalConst.MdiForm.MenuFormatBold.Checked = true;
                }
                else
                {
                    globalConst.MdiForm.T2Bold.Checked = false;
                    globalConst.MdiForm.MenuFormatBold.Checked = false;
                }
                if (editocx.queryCMDState(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Italic))
                {
                    globalConst.MdiForm.T2Italic.Checked = true;
                    globalConst.MdiForm.MenuFormatItalic.Checked = true;
                }
                else
                {
                    globalConst.MdiForm.T2Italic.Checked = false;
                    globalConst.MdiForm.MenuFormatItalic.Checked = false;
                }
                if (editocx.queryCMDState(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Underline))
                {
                    globalConst.MdiForm.T2Underline.Checked = true;
                    globalConst.MdiForm.MenuFormatUnderline.Checked = true;
                }
                else
                {
                    globalConst.MdiForm.T2Underline.Checked = false;
                    globalConst.MdiForm.MenuFormatUnderline.Checked = false;
                }
                if (editocx.queryCMDState(D4ENUM.D4HTMLCMDSTR.HTMLCMD_JustifyLeft))
                {
                    globalConst.MdiForm.T2AlignLeft.Checked = true;
                    globalConst.MdiForm.MenuFormatAlignLeft.Checked = true;
                }
                else
                {
                    globalConst.MdiForm.T2AlignLeft.Checked = false;
                    globalConst.MdiForm.MenuFormatAlignLeft.Checked = false;
                }
                if (editocx.queryCMDState(D4ENUM.D4HTMLCMDSTR.HTMLCMD_JustifyRight))
                {
                    globalConst.MdiForm.T2AlignRight.Checked = true;
                    globalConst.MdiForm.MenuFormatAlignRight.Checked = true;
                }
                else
                {
                    globalConst.MdiForm.T2AlignRight.Checked = false;
                    globalConst.MdiForm.MenuFormatAlignRight.Checked = false;
                }
                if (editocx.queryCMDState(D4ENUM.D4HTMLCMDSTR.HTMLCMD_JustifyCenter))
                {
                    globalConst.MdiForm.T2AlignMiddle.Checked = true;
                    globalConst.MdiForm.MenuFormatAlignCenter.Checked = true;
                }
                else
                {
                    globalConst.MdiForm.T2AlignMiddle.Checked = false;
                    globalConst.MdiForm.MenuFormatAlignCenter.Checked = false;
                }
            }
            catch (Exception ex) { }
        }
        public bool iEvent_ondrop(IHTMLEventObj pEvtObj)
        {
            if (PageWare.getPartElement(editocx.elementFromPoint(pEvtObj.x, pEvtObj.y)) != null)
            {
                this.editocx.Cursor = Cursors.No;
                classes.EditorEvent.MoveEvents();
                MainForm.CloseImageForm();
                return false;
            }
            mshtml.IHTMLTxtRange it = INITxtRange;
            //it.moveToElementText(ie);
            it.moveToPoint(pEvtObj.x, pEvtObj.y);
            it.select();
            if (MainForm.imgform.text.StartsWith("{part}"))
            {
                TreeNode dragNode = (TreeNode)classes.EditorEvent.DragE.Data.GetData(typeof(TreeNode));
                //new MsgBox(tree.getID(dragNode));
                MainForm.CloseImageForm();
                if (!PageWare.AddPart2Editor(tree.getID(dragNode), this))
                {
                    MsgBox.Error(res.Editor.GetString("m4"));
                }
                classes.PageAsist.RefreshToolBoxPages();
            }
            else if (MainForm.imgform.text.StartsWith("{control}"))
            {
                Silver.UI.ToolBoxItem dragNode = (Silver.UI.ToolBoxItem)classes.EditorEvent.DragE.Data.GetData(typeof(Silver.UI.ToolBoxItem));

                MainForm.CloseImageForm();
                switch (globalConst.MdiForm.MainToolBox.SelectedTabIndex)
                { 
                    case 0:
                        switch (dragNode.Object.ToString())
                        {
                            case "Label":
                                editocx.pasteHtml("<label>" + dragNode.Caption + "</label>");
                                break;
                            case "HyperLink":
                                editocx.pasteHtml("<a href=\"\">" + dragNode.Caption + "</a>");
                                break;
                            case "TextBox":
                                editocx.pasteHtml("<input type=\"text\">");
                                break;
                            case "TextArea":
                                editocx.pasteHtml("<textarea></textarea>");
                                break;
                            case "Password":
                                editocx.pasteHtml("<input type=\"password\">");
                                break;
                            case "Button":
                                editocx.pasteHtml("<input type=\"button\" value=\"Button\">");
                                break;
                            case "Button2":
                                editocx.pasteHtml("<button type=\"button\" class=\"_button\">Button</button>");
                                break;
                            case "Submit Button":
                                editocx.pasteHtml("<input type=\"submit\" value=\"" + dragNode.Caption + "\">");
                                break;
                            case "Reset Button":
                                editocx.pasteHtml("<input type=\"reset\" value=\"" + dragNode.Caption + "\">");
                                break;
                            case "Image Button":
                                editocx.pasteHtml("<input type=\"image\">");
                                break;
                            case "CheckBox":
                                editocx.pasteHtml("<input type=\"checkbox\">");
                                break;
                            case "Redio Button":
                                editocx.pasteHtml("<input type=\"radio\">");
                                break;
                            case "ComboBox":
                                editocx.pasteHtml("<select></select>");
                                break;
                            case "ListBox":
                                editocx.pasteHtml("<select size=\"3\"></select>");
                                break;
                            case "Hidden Field":
                                editocx.pasteHtml("<input type=\"hidden\">");
                                break;
                            case "File Upload":
                                editocx.pasteHtml("<input type=\"file\">");
                                break;
                            case "GroupBox":
                                editocx.pasteHtml("<fieldset><legend>" + dragNode.Caption + "<legend></legend></legend></fieldset>");
                                break;
                            case "Anchor":
                                editocx.pasteHtml("<a href=\"\">" + dragNode.Caption + "</a>");
                                break;
                            case "Image":
                                editocx.pasteHtml("<img src=\"\">");
                                break;
                            case "Table":
                                editocx.pasteHtml("<table><tbody><tr><td>" + dragNode.Caption + "</td><td></td><td></td></tr><tr><td></td><td></td><td></td></tr><tr><td></td><td></td><td></td></tr></tbody></table>");
                                break;
                            case "Span":
                                editocx.pasteHtml("<span>" + dragNode.Caption + "</span>");
                                break;
                            case "Div":
                                editocx.pasteHtml("<div>" + dragNode.Caption + "</div>");
                                break;
                            case "Panel":
                                editocx.pasteHtml("<div style=\"WIDTH: 100px; HEIGHT: 100px\">" + dragNode.Caption + "</div>");
                                break;
                            case "IFrame":
                                editocx.pasteHtml("<iframe style=\"WIDTH: 300px; HEIGHT: 138px\"></iframe>");
                                break;
                            case "Horizontal Rule":
                                editocx.pasteHtml("<hr>");
                                break;
                            case "Form":
                                editocx.pasteHtml("<form></form>");
                                break;
                            default: break;
                        }
                        break;
                    case 3:
                        //MsgBox.Information(dragNode.Object.ToString());
                        if (!isFreeFile)
                        {
                            if (pagetype == 0 && "|Row Rate|Row Index|".IndexOf(dragNode.Object.ToString())<0)
                            {
                                MsgBox.Information(res.form.GetString("String38"));
                            }
                            else if (pagetype == 1 && (dragNode.Object.ToString().Equals("Stat Filter") || dragNode.Object.ToString().Equals("Member Input")))
                            {
                                MsgBox.Information(res.form.GetString("String39"));
                            }
                            else
                            {
                                FormName fn = new FormName();
                                if (globalConst.FormDataMode)
                                {
                                    switch (dragNode.Object.ToString())
                                    {
                                        case "Text Box":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<input type=\"text\" id=\"ftform_" + str.getEncode("maobb:0:" + fn.EleName + "|") + "\" tag=\""+str.SetSplitValue(null,2,0,fn.EleName)+"\"/>");
                                                Page.PageWare.UpdateFormBindPage(fn.EleName);
                                            }
                                            break;
                                        case "Password":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<input type=\"password\" id=\"ftform_" + str.getEncode("maobb:0:" + fn.EleName + "|") + "\" tag=\"" + str.SetSplitValue(null, 2, 0, fn.EleName) + "\"/>");
                                                Page.PageWare.UpdateFormBindPage(fn.EleName);
                                            }
                                            break;
                                        case "Combo Box":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<select id=\"ftform_" + str.getEncode("maobb:0:" + fn.EleName + "|") + "\" tag=\"" + str.SetSplitValue(null, 2, 0, fn.EleName) + "\"><option>" + dragNode.Caption + "</option></select>");
                                                Page.PageWare.UpdateFormBindPage(fn.EleName);
                                            }
                                            break;
                                        case "Text Area":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<textarea id=\"ftform_" + str.getEncode("maobb:0:" + fn.EleName + "|") + "\" tag=\"" + str.SetSplitValue(null, 2, 0, fn.EleName) + "\"></textarea>");
                                                Page.PageWare.UpdateFormBindPage(fn.EleName);
                                            }
                                            break;
                                        case "Radio Box":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<input type=\"radio\" id=\"ftform_" + str.getEncode("maobb:0:" + fn.EleName + "|") + "\" tag=\"" + str.SetSplitValue(null, 2, 0, fn.EleName) + "\"/>");
                                                Page.PageWare.UpdateFormBindPage(fn.EleName);
                                            }
                                            break;
                                        case "Check Box":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<input type=\"checkbox\" id=\"ftform_" + str.getEncode("maobb:0:" + fn.EleName + "|") + "\" tag=\"" + str.SetSplitValue(null, 2, 0, fn.EleName) + "\"/>");
                                                Page.PageWare.UpdateFormBindPage(fn.EleName);
                                            }
                                            break;
                                        case "File Box":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<input type=\"file\" id=\"ftform_" + str.getEncode("maobb:0:" + fn.EleName + "|") + "\" tag=\"" + str.SetSplitValue(null, 2, 0, fn.EleName) + "\"/>");
                                                Page.PageWare.UpdateFormBindPage(fn.EleName);
                                            }
                                            break;
                                        case "Label":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<label id=\"ftform_" + str.getEncode("maobb:2:" + fn.EleName + "|") + "\" tag=\"" + str.SetSplitValue(null, 2, 0, fn.EleName) + "\">" + dragNode.Caption + "</label>");
                                                Page.PageWare.UpdateFormBindPage(fn.EleName);
                                            }
                                            break;
                                        case "Category":
                                            editocx.pasteHtml("<select id=\"ftform_" + str.getEncode("maobb:1:ftform_category|") + "\"><option>" + dragNode.Caption + "</option></select>");
                                            break;
                                        case "Date Year":
                                            editocx.pasteHtml("<select id=\"ftform_" + str.getEncode("maobb:1:ftform_dateyear|") + "\"><option>" + dragNode.Caption + "</option></select>");
                                            break;
                                        case "Date Month":
                                            editocx.pasteHtml("<select id=\"ftform_" + str.getEncode("maobb:1:ftform_datemonth|") + "\"><option>" + dragNode.Caption + "</option></select>");
                                            break;
                                        case "Date Day":
                                            editocx.pasteHtml("<select id=\"ftform_" + str.getEncode("maobb:1:ftform_dateday|") + "\"><option>" + dragNode.Caption + "</option></select>");
                                            break;
                                        case "Filter Action":
                                            editocx.pasteHtml("<input type=\"button\" id=\"ftform_" + str.getEncode("maobb:1:ftform_filteraction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Data Action":
                                            editocx.pasteHtml("<input type=\"button\" id=\"ftform_" + str.getEncode("maobb:1:ftform_dataaction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "New Action":
                                            editocx.pasteHtml("<input type=\"button\" id=\"ftform_" + str.getEncode("maobb:1:ftform_newaction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Mod Action":
                                            editocx.pasteHtml("<input type=\"button\" id=\"ftform_" + str.getEncode("maobb:1:ftform_modaction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Reset Action":
                                            editocx.pasteHtml("<input type=\"button\" id=\"ftform_" + str.getEncode("maobb:1:ftform_resetaction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Stat Filter":
                                            editocx.pasteHtml("<select id=\"ftform_" + str.getEncode("maobb:1:ftform_statfilter|0") + "\"><option>" + dragNode.Caption + "</option></select>");
                                            break;
                                        case "Member Input":
                                            editocx.pasteHtml("<input type=\"text\" id=\"ftform_" + str.getEncode("maobb:1:ftform_memberinput|") + "\"/>");
                                            break;
                                        case "Row Rate":
                                            editocx.pasteHtml("<input type=\"button\" name=\"rowrate_0_" + rdm.getCombID().Replace("_", "") + "\" id=\"ftform_" + str.getEncode("maobb:1:ftform_rowrate|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        //case "Row Rate2":
                                        //    editocx.pasteHtml("<button onclick=\"ftformaddrow(this, 'add')\" class='_button' type='button'>新增一行</button>");
                                        //    break;
                                        case "Row Index":
                                            editocx.pasteHtml("<span name=\"row_index_1\" id=\"ftform_" + str.getEncode("maobb:1:ftform_rowindex|") + "\"/>" + dragNode.Caption + "</span>");
                                            break;
                                        case "Flow Data":
                                            editocx.pasteHtml("<input type=\"button\" name=\"flow_btn_val_0\" id=\"ftform_" + str.getEncode("maobb:1:ftform_flowdataaction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Flow Add":
                                            editocx.pasteHtml("<input type=\"button\" name=\"flow_btn_val_0\" id=\"ftform_" + str.getEncode("maobb:1:ftform_flowaddaction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Flow Mod":
                                            editocx.pasteHtml("<input type=\"button\" name=\"flow_btn_val_0\" id=\"ftform_" + str.getEncode("maobb:1:ftform_flowmodaction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Flow Flow":
                                            editocx.pasteHtml("<input type=\"button\" name=\"flow_btn_val_0\" id=\"ftform_" + str.getEncode("maobb:1:ftform_flowflowaction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Flow Ext1"://level,flow,ismod,name,pname
                                            editocx.pasteHtml("<input type=\"button\" name=\"flow_btn_val_1_0_0_\" id=\"ftform_" + str.getEncode("maobb:1:ftform_flowext1action|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Flow Ext2":
                                            editocx.pasteHtml("<input type=\"button\" name=\"flow_btn_val_2_0_0__\" id=\"ftform_" + str.getEncode("maobb:1:ftform_flowext2action|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        default: break;
                                    }
                                }
                                else
                                {
                                    switch (dragNode.Object.ToString())
                                    {
                                        case "Text Box":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<input type=\"text\" id=\"ftform_" + str.getEncode("maobb:0:" + fn.EleName + "|") + "\"/>");
                                            }
                                            break;
                                        case "Password":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<input type=\"password\" id=\"ftform_" + str.getEncode("maobb:0:" + fn.EleName + "|") + "\"/>");
                                            }
                                            break;
                                        case "Combo Box":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<select id=\"ftform_" + str.getEncode("maobb:0:" + fn.EleName + "|") + "\"><option>" + dragNode.Caption + "</option></select>");
                                            }
                                            break;
                                        case "Text Area":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<textarea id=\"ftform_" + str.getEncode("maobb:0:" + fn.EleName + "|") + "\"></textarea>");
                                            }
                                            break;
                                        case "Radio Box":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<input type=\"radio\" id=\"ftform_" + str.getEncode("maobb:0:" + fn.EleName + "|") + "\"/>");
                                            }
                                            break;
                                        case "Check Box":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<input type=\"checkbox\" id=\"ftform_" + str.getEncode("maobb:0:" + fn.EleName + "|") + "\"/>");
                                            }
                                            break;
                                        case "File Box":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<input type=\"file\" id=\"ftform_" + str.getEncode("maobb:0:" + fn.EleName + "|") + "\"/>");
                                            }
                                            break;
                                        case "Label":
                                            fn.EleCaption = dragNode.Caption;
                                            fn.ShowDialog();
                                            if (!fn.cancel)
                                            {
                                                editocx.pasteHtml("<label id=\"ftform_" + str.getEncode("maobb:2:" + fn.EleName + "|") + "\">" + dragNode.Caption + "</label>");
                                            }
                                            break;
                                        case "Category":
                                            editocx.pasteHtml("<select id=\"ftform_" + str.getEncode("maobb:1:ftform_category|") + "\"><option>" + dragNode.Caption + "</option></select>");
                                            break;
                                        case "Date Year":
                                            editocx.pasteHtml("<select id=\"ftform_" + str.getEncode("maobb:1:ftform_dateyear|") + "\"><option>" + dragNode.Caption + "</option></select>");
                                            break;
                                        case "Date Month":
                                            editocx.pasteHtml("<select id=\"ftform_" + str.getEncode("maobb:1:ftform_datemonth|") + "\"><option>" + dragNode.Caption + "</option></select>");
                                            break;
                                        case "Date Day":
                                            editocx.pasteHtml("<select id=\"ftform_" + str.getEncode("maobb:1:ftform_dateday|") + "\"><option>" + dragNode.Caption + "</option></select>");
                                            break;
                                        case "Filter Action":
                                            editocx.pasteHtml("<input type=\"button\" id=\"ftform_" + str.getEncode("maobb:1:ftform_filteraction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "New Action":
                                            editocx.pasteHtml("<input type=\"button\" id=\"ftform_" + str.getEncode("maobb:1:ftform_newaction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Mod Action":
                                            editocx.pasteHtml("<input type=\"button\" id=\"ftform_" + str.getEncode("maobb:1:ftform_modaction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Reset Action":
                                            editocx.pasteHtml("<input type=\"button\" id=\"ftform_" + str.getEncode("maobb:1:ftform_resetaction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Stat Filter":
                                            editocx.pasteHtml("<select id=\"ftform_" + str.getEncode("maobb:1:ftform_statfilter|0") + "\"><option>" + dragNode.Caption + "</option></select>");
                                            break;
                                        case "Member Input":
                                            editocx.pasteHtml("<input type=\"text\" id=\"ftform_" + str.getEncode("maobb:1:ftform_memberinput|") + "\"/>");
                                            break;
                                        case "Row Rate":
                                            editocx.pasteHtml("<input type=\"button\" name=\"rowrate_0_" + rdm.getCombID().Replace("_", "") + "\" id=\"ftform_" + str.getEncode("maobb:1:ftform_rowrate|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Row Index":
                                            editocx.pasteHtml("<span name=\"row_index_1\" id=\"ftform_" + str.getEncode("maobb:1:ftform_rowindex|") + "\"/>" + dragNode.Caption + "</span>");
                                            break;
                                        case "Flow Add":
                                            editocx.pasteHtml("<input type=\"button\" name=\"flow_btn_val_0\" id=\"ftform_" + str.getEncode("maobb:1:ftform_flowaddaction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Flow Mod":
                                            editocx.pasteHtml("<input type=\"button\" name=\"flow_btn_val_0\" id=\"ftform_" + str.getEncode("maobb:1:ftform_flowmodaction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Flow Flow":
                                            editocx.pasteHtml("<input type=\"button\" name=\"flow_btn_val_0\" id=\"ftform_" + str.getEncode("maobb:1:ftform_flowflowaction|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Flow Ext1"://level,flow,ismod,name,pname
                                            editocx.pasteHtml("<input type=\"button\" name=\"flow_btn_val_1_0_0_\" id=\"ftform_" + str.getEncode("maobb:1:ftform_flowext1action|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        case "Flow Ext2":
                                            editocx.pasteHtml("<input type=\"button\" name=\"flow_btn_val_2_0_0__\" id=\"ftform_" + str.getEncode("maobb:1:ftform_flowext2action|") + "\" value=\"" + dragNode.Caption + "\"/>");
                                            break;
                                        default: break;
                                    }
                                }
                                fn = null;
                            }
                        }
                        break;
                    case 4:
                        string sql = "select content from snippets where id=" + dragNode.Object.ToString();
                        editocx.pasteHtml(globalConst.ConfigConn.GetString(sql));
                        break;
                    default:
                        if (!isFreeFile)
                        {
                            AddControl2Page ACP = new AddControl2Page();
                            ACP.controlName = dragNode.Object.ToString();
                            ACP.ShowDialog();
                        }
                        break;
                }
                //if (!PageWare.AddPart2Editor(tree.getID(dragNode), this))
                //{
                //    MsgBox.Error("add part to page failed!");
                //}
            }
            classes.EditorEvent.MoveEvents();
            return false;
        }

        public bool iEvent_ondragover(IHTMLEventObj pEvtObj)
        {
            MainForm.imgform.Visible = true;
            MainForm.imgform.Location = new Point(pEvtObj.screenX - globalConst.MdiForm.Left-2, pEvtObj.screenY - globalConst.MdiForm.Top - 17);
            MainForm.imgform.BringToFront();

            return false;
        }

        public bool iEvent_ondragenter(IHTMLEventObj pEvtObj)
        {
            this.editocx.Cursor = Cursors.Default;
            return false;
        }

        public void iEvent_ondragleave(IHTMLEventObj pEvtObj)
        {
            MainForm.imgform.Visible = false;
        }

        private void textocx_theMouseDown(object sender, MouseEventArgs e)
        {

            globalConst.MdiForm.MainStatus.Text="(" + this.textocx.NowPointX + "," + this.textocx.NowPointY + ")";
        }

        private void EditSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            globalConst.MdiForm.ChangeTabPageActiveColor(EditSpace);
            globalConst.MdiForm.T1Design.Checked = false;
            globalConst.MdiForm.T1Code.Checked = false;
            globalConst.MdiForm.T1Preview.Checked = false;
            globalConst.MdiForm.MenuViewDesign.Checked = false;
            globalConst.MdiForm.MenuViewCode.Checked = false;
            globalConst.MdiForm.MenuViewPreview.Checked = false;
            switch (EditSpace.SelectedIndex)
            {
                case 0: change_editmode("edit"); globalConst.MdiForm.T1Design.Checked = true; globalConst.MdiForm.MenuViewDesign.Checked = true; break;
                case 1: change_editmode("text"); globalConst.MdiForm.T1Code.Checked = true; globalConst.MdiForm.MenuViewCode.Checked = true; break;
                case 2: change_editmode("view"); globalConst.MdiForm.T1Preview.Checked = true; globalConst.MdiForm.MenuViewPreview.Checked = true; break;
                default: break;
            }
        }

        private void Editor_Resize(object sender, EventArgs e)
        {
            doResize();
        }
        private void doResize()
        {
            editocx.Location = new Point(-2, -2);
            editocx.Width = EditSpace.Width - 3; 
            //editocx.Width = this.Width - 19;
            //editocx.Width = this.Width - 12;38
            editocx.Height = EditSpace.Height - 25;
            panel1.Location = new Point(-2, 23);
            panel1.Width = EditSpace.Width - 3;
            //panel1.Width = this.Width - 19;
            //panel1.Width = this.Width - 12;
            panel1.Height = EditSpace.Height - 50;
            viewocx.Location = new Point(-2, -2);
            viewocx.Width = EditSpace.Width - 3;
            //viewocx.Width = this.Width - 19;
            //viewocx.Width = this.Width - 12;
            viewocx.Height = EditSpace.Height - 25;
            //panel1.Dock = DockStyle.Fill;
            //new MsgBox(panel1.Location.ToString() + ":" + panel1.Width.ToString() + ":" + panel1.Height.ToString() + ":nnn" + this.Width.ToString() + ":" + this.Height.ToString());
        }

        private void editocx_onDocumentmousemove()
        {
            ex = Cursor.Position.X - this.Location.X - (globalConst.MdiForm.MainToolBox.Visible?(globalConst.MdiForm.MainToolBox.Width+20):20) - globalConst.MdiForm.Left;
            ey = Cursor.Position.Y - this.Location.Y - 137 - globalConst.MdiForm.Top + 73 - globalConst.MdiForm.toolStripContainer1.Height;
            globalConst.MdiForm.MainStatus.Text = "(" + ex + "," + ey + ")";
        }

        private void editocx_oncontextmenu()
        {
            CMPage.Show(this.editocx, new Point(ex, ey));
        }

        private void CMPage_Opening(object sender, CancelEventArgs e)
        {
            IHTMLElement curie = editocx.getCurElement();

            this.pagePasteToolStripMenuItem.Enabled=editocx.queryCMDEnabled(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Paste);
            this.pageCutToolStripMenuItem.Enabled=editocx.queryCMDEnabled(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Cut);
            this.PageDelete.Enabled=editocx.queryCMDEnabled(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Delete);
            this.pageCopyToolStripMenuItem.Enabled=editocx.queryCMDEnabled(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Copy);
            this.pageUndoToolStripMenuItem.Enabled=editocx.queryCMDEnabled(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Undo);
            this.pageRedoToolStripMenuItem.Enabled=editocx.queryCMDEnabled(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Redo);

            bool isControl = editocx.getSelectionType().ToLower().Equals("control");
            this.pageInsertTableToolStripMenuItem.Enabled = !isControl;


            this.EditFormDataMenuItem1.Enabled = false;
            if (globalConst.FormDataMode)
            {
                if (PageWare.isFormElementDataShouldBind(curie))
                {
                    this.EditFormDataMenuItem1.Enabled = true;
                }
            }


            if (editocx.getTableElement(curie) != null)
            {
                this.pageInsertRowToolStripMenuItem.Enabled = true;
                this.pageInsertColumnToolStripMenuItem.Enabled = true;
                this.pageDeleteRowToolStripMenuItem.Enabled = true;
                this.pageDeleteColumnToolStripMenuItem.Enabled = true;
                this.pageSplitColumnToolStripMenuItem.Enabled = true;
                this.pageSplitRowToolStripMenuItem.Enabled = true;
                this.pageMergeCellDownToolStripMenuItem.Enabled = true;
                this.pageMergeCellRightToolStripMenuItem.Enabled = true;
                this.pageMergeCellToolStripMenuItem.Enabled = true;
                this.pageMergeCellUpToolStripMenuItem.Enabled = true;
                this.pageSelectTableToolStripMenuItem.Enabled = true;
                this.pageClearContentToolStripMenuItem.Enabled = true;
            }
            else
            {
                this.pageInsertRowToolStripMenuItem.Enabled = false;
                this.pageInsertColumnToolStripMenuItem.Enabled = false;
                this.pageDeleteRowToolStripMenuItem.Enabled = false;
                this.pageDeleteColumnToolStripMenuItem.Enabled = false;
                this.pageSplitColumnToolStripMenuItem.Enabled = false;
                this.pageSplitRowToolStripMenuItem.Enabled = false;
                this.pageMergeCellDownToolStripMenuItem.Enabled = false;
                this.pageMergeCellRightToolStripMenuItem.Enabled = false;
                this.pageMergeCellToolStripMenuItem.Enabled = false;
                this.pageMergeCellUpToolStripMenuItem.Enabled = false;
                this.pageSelectTableToolStripMenuItem.Enabled = false;
                this.pageClearContentToolStripMenuItem.Enabled = false;
                return;
            }
            if (isControl)
            {
                this.pageInsertRowToolStripMenuItem.Enabled = false;
                this.pageInsertColumnToolStripMenuItem.Enabled = false;
                this.pageDeleteRowToolStripMenuItem.Enabled = false;
                this.pageDeleteColumnToolStripMenuItem.Enabled = false;
                this.pageSplitColumnToolStripMenuItem.Enabled = false;
                this.pageSplitRowToolStripMenuItem.Enabled = false;
                this.pageMergeCellDownToolStripMenuItem.Enabled = false;
                this.pageMergeCellRightToolStripMenuItem.Enabled = false;
                this.pageMergeCellToolStripMenuItem.Enabled = false;
                this.pageMergeCellUpToolStripMenuItem.Enabled = false;
                return;
            }

            IHTMLElement ie = curie;
            if (ie == null) return;
            IHTMLTableCell iec = (IHTMLTableCell)editocx.getCellElement(ie);
            if (iec == null) return;
            IHTMLTableRow ier = (IHTMLTableRow)editocx.getRowElement(ie);
            if (ier == null) return;
            IHTMLTable iet = (IHTMLTable)editocx.getTableElement(ie);
            if (iet == null) return;
            this.pageMergeCellDownToolStripMenuItem.Enabled = (ier.rowIndex < iet.rows.length - iec.rowSpan);
            this.pageMergeCellRightToolStripMenuItem.Enabled = (ier.cells.length > iec.cellIndex + 1);
            this.pageMergeCellToolStripMenuItem.Enabled = (iec.cellIndex>0);
            this.pageMergeCellUpToolStripMenuItem.Enabled = (ier.rowIndex>0);


        }

        private void pageCutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Cut, false, null);
        }

        private void pageCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Copy, false, null);
        }

        private void pagePasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Paste, false, null);
        }

        private void PageDelete_Click(object sender, EventArgs e)
        {
            editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Delete, false, null);
        }

        private void pageUndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Undo, false, null);
        }

        private void pageRedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Redo, false, null);
        }

        private void pageInsertRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.tableInsertRow();
        }

        private void pageInsertColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.tableInsertCol();
        }

        private void pageDeleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.tableDeleteRow();
        }

        private void pageDeleteColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.tableDeleteCol();
        }

        private void pageSplitRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.tableSplitCell2Rows();
        }

        private void pageSplitColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.tableSplitCell2Cols();
        }

        private void pageMergeCellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.tableMergeCellsLeft();
        }

        private void pageMergeCellRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.tableMergeCellsRight();
        }

        private void pageMergeCellUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.tableMergeCellsUp();
        }

        private void pageMergeCellDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.tableMergeCellsDown();
        }

        private void pageSelectTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IHTMLElement t = editocx.getTableElement(editocx.getCurElement());
            if (t != null)
            {
                IHTMLTxtRange rg;
                HTMLDocumentClass hc = new HTMLDocumentClass();
                IHTMLDocument2 doc2 = hc;
                doc2 = (IHTMLDocument2)editocx.DOM;
                IHTMLBodyElement ib = (IHTMLBodyElement)doc2.body;
                rg = ib.createTextRange();
                rg.moveToElementText(t);
                //rg.scrollIntoView(true);
                rg.select();
            }
        }

        private void pageClearContentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IHTMLTable t = (IHTMLTable)editocx.getTableElement(editocx.getCurElement());
            IHTMLTableRow r;
            IHTMLElement c;
            if (t != null)
            {
                int i;
                int j;
                for (i = 0; i < t.rows.length; i++)
                {
                    r = (IHTMLTableRow)t.rows.item(i, i);
                    for (j = 0; j < r.cells.length; j++)
                    {
                        c = (IHTMLElement)r.cells.item(j, j);
                        c.innerHTML = "&nbsp;";
                    }
                }
            }
        }

        private void WordWrap_Click(object sender, EventArgs e)
        {
            this.textocx.WordWrap = WordWrap.Checked;
            ScEditSheelEdit4Wraped = true;
        }

        private void SelectionMargin_Click(object sender, EventArgs e)
        {
            this.textocx.ShowSelectionMargin = SelectionMargin.Checked;
            ScEditSheelEdit4Margin = true;
        }

        private void BGColor_Click(object sender, EventArgs e)
        {
            ColorDialog dc = new ColorDialog();
            dc.Color = this.textocx.BgColor;
            dc.ShowDialog();
            this.textocx.BgColor = dc.Color;
            ScEditSheelEdit4BgColor = true;
        }

        private void TheFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textocx.theFontSize = int.Parse(TheFontSize.SelectedItem.ToString());
            ScEditSheelEdit4FontSize = true;
        }

        private void TheFontName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textocx.theFontName = Editor.GetFontName(TheFontName.SelectedIndex);
            ScEditSheelEdit4FontName = true;
        }

        private void textocx_SelectionChange()
        {
            try
            {
                CurLine.Text = this.textocx.curLine.ToString();
                curCol.Text = this.textocx.curCol.ToString();
            }
            catch { }
        }

        private void CodeCut_Click(object sender, EventArgs e)
        {
            textocx.doCopy();
            textocx.SelectedText = "";
        }

        private void CodeCopy_Click(object sender, EventArgs e)
        {
            textocx.doCopy();
        }

        private void CodePaste_Click(object sender, EventArgs e)
        {
            textocx.doPaste();
        }

        private void CodeDelete_Click(object sender, EventArgs e)
        {
            textocx.SelectedText = "";
        }

        private void CodeUndo_Click(object sender, EventArgs e)
        {
            textocx.Undo();
        }

        private void CodeRedo_Click(object sender, EventArgs e)
        {
            textocx.Redo();
        }

        private void CMCode_Opening(object sender, CancelEventArgs e)
        {
            CodeCopy.Enabled = textocx.canCopy;
            CodeCut.Enabled = textocx.canCopy;
            CodeDelete.Enabled = textocx.canDelete;
            CodePaste.Enabled = textocx.canPaste;
            CodeUndo.Enabled = textocx.CanUndo;
            CodeRedo.Enabled = textocx.CanRedo;
        }

        private void textocx_OnContextMenu(object sender, MouseEventArgs e)
        {
            CMCode.Show(textocx, new Point(e.X, e.Y));
        }

        private void pageEditTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IHTMLElement ie = this.editocx.getCurElement();
            if (ie != null)
            {
                if (ie.tagName.ToLower().Equals("body") || PageWare.isPartElement(ie)) return;
                TagEdit te = new TagEdit();
                te.ele = ie;
                try
                {
                    te.ShowDialog();
                }
                catch (Exception ex) { new error(ex); }
            }
        }

        private void pageInsertTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editocx.pasteHtml("<table><tbody><tr><td>Table</td><td></td><td></td></tr><tr><td></td><td></td><td></td></tr><tr><td></td><td></td><td></td></tr></tbody></table>");
        }
        public bool IsClosing = false;
        private void Editor_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsClosing = true;
            globalConst.MdiForm.UpdateMenusAndToolBars4Page();
        }

        private void EditFormDataMenuItem1_Click(object sender, EventArgs e)
        {
            if (FormData.TheFormData == null)
            {
                FormData.TheFormData = new FormData();
            }
            FormData.TheFormData.fromSite = false;
            FormData.ele = editocx.getCurElement();
            FormData.FormDataShow();
        }

        private void EditSpace_Resize(object sender, EventArgs e)
        {
            doResize();
        }

        private void editocx_onDocumentdblclick()
        {
            IHTMLElement ele=editocx.getCurElement();
            if (Page.PageWare.isPartElement(ele))
            {
                string partid = Adapter.Property.PropertyAdapter.getEleAttr(ele, "idname");
                string sql = "select a.name,a.controlid,a.partxml,b.name,a.asportal from parts a,controls b where a.id='" + partid + "' and a.controlid=b.id";
                System.Data.OleDb.OleDbDataReader rdr = globalConst.CurSite.SiteConn.OpenRecord(sql);
                if (rdr.Read())
                {
                    string PartName = rdr.GetString(0);
                    string WareID = rdr.GetString(1);
                    string PartXml = rdr.GetString(2);
                    string WareName = rdr.GetString(3);
                    int AsPortal = rdr.GetInt32(4);
                    rdr.Close();
                    if (WareName.Equals("list") && PartName.Equals("List"))
                    {
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.LoadXml(PartXml);
                        System.Xml.XmlNodeList nodes = doc.SelectNodes("/partxml/param");
                        System.Xml.XmlNode curnode = null;
                        foreach (System.Xml.XmlNode node in nodes)
                        {
                            if (node.SelectSingleNode("name").InnerText.Equals("RowAll"))
                            {
                                curnode = node;
                                break;
                            }
                        }
                        RowAll gn = new RowAll();
                        gn.restr = curnode.SelectSingleNode("value").InnerText;
                        gn.ShowDialog();
                        if (!gn.IsCancel)
                        {
                            curnode.SelectSingleNode("value").InnerText = gn.restr;
                            sql = "update parts set partxml='" + str.Dot2DotDot(doc.OuterXml) + "' where id='" + partid + "'";
                            globalConst.CurSite.SiteConn.execSql(sql);
                            PageWare.ApplyPartHTML(ele, WareID, WareName, PartName, doc.OuterXml, AsPortal);
                        }
                        curnode = null;
                        nodes = null;
                        doc = null;
                    }
                    else if (WareName.Equals("dataop") && PartName.Equals("Interface"))
                    {
                        if (!DataOpDefine.DataOpFormShow)
                        {
                            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                            doc.LoadXml(PartXml);
                            System.Xml.XmlNodeList nodes = doc.SelectNodes("/partxml/param");
                            System.Xml.XmlNode curnode = null;
                            foreach (System.Xml.XmlNode node in nodes)
                            {
                                if (node.SelectSingleNode("name").InnerText.Equals("Define"))
                                {
                                    curnode = node;
                                    break;
                                }
                            }
                            DataOpDefine gn = new DataOpDefine();
                            gn.str = curnode.SelectSingleNode("value").InnerText;
                            gn.partid = partid;
                            gn.Show();

                            curnode = null;
                            nodes = null;
                            doc = null;
                        }
                    }
                    else if (WareName.Equals("dyvalue") && PartName.Equals("Interface"))
                    {
                        if (!DyValueDefine.DyValueFormShow)
                        {
                            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                            doc.LoadXml(PartXml);
                            System.Xml.XmlNodeList nodes = doc.SelectNodes("/partxml/param");
                            System.Xml.XmlNode curnode = null;
                            foreach (System.Xml.XmlNode node in nodes)
                            {
                                if (node.SelectSingleNode("name").InnerText.Equals("Define"))
                                {
                                    curnode = node;
                                    break;
                                }
                            }
                            DyValueDefine gn = new DyValueDefine();
                            gn.str = curnode.SelectSingleNode("value").InnerText;
                            gn.partid = partid;
                            gn.Show();

                            curnode = null;
                            nodes = null;
                            doc = null;
                        }
                    }
                }
                else
                {
                    rdr.Close();
                }
            }
            
        }
        public void coloneControl(bool replace)
        {
            IHTMLElement ie = this.editocx.getCurElement();
            if (ie == null || !PageWare.isPartElement(ie)) return;
            try
            {
                string partid = ie.getAttribute("idname").ToString();
                string partname = ie.getAttribute("partname").ToString();
                string sql = "select a.caption,a.name,a.id from controls a,parts b where a.id=b.controlid and b.id='" + partid + "'";
                DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql));
                string controlid = null;
                string controlcap = null;
                if (dr.Read())
                {
                    controlid = dr.getString(2);
                    controlcap = dr.getString(0);
                }
                dr.Close();
                if (controlid != null)
                {
                    NewControl nc = new NewControl();
                    nc.cloneAndReplace = replace;
                    nc.controlName = controlid;
                    nc.curSelectPartEle = ie;
                    nc.clonePartName = partname;
                    nc.curpageid = this.thisID;
                    nc.oldpartid = partid;
                    nc.addType = "clone";
                    nc.textBox1.Text = controlcap;
                    nc.ShowDialog();
                    if (!nc.isCancel)
                    {
                        globalConst.MdiForm.refreshControlTree();
                        //globalConst.MdiForm.ControlTree.Nodes[nodeindex].Expand();
                    }
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void cloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            coloneControl(false);
        }

        private void copypartidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IHTMLElement ie = this.editocx.getCurElement();
            if (ie != null && PageWare.isPartElement(ie))
            {
                string partid = ie.getAttribute("idname").ToString();
                Clipboard.SetDataObject(partid);
                consts.globalConst.MdiForm.MainStatus.Text = "Copy PartID OK：" + partid;
            }
        }

        private void cloneandreplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            coloneControl(true);
        }

        private void ToolStdadadaripMenuItem_Click(object sender, EventArgs e)
        {
            IHTMLElement ie = this.editocx.getCurElement();
            if (ie != null && PageWare.isPartElement(ie))
            {
                string partid = ie.getAttribute("idname").ToString();
                string nodeHTML = "<ftdp:control id=\"" + ie.getAttribute("idname", 0).ToString() + "\" name=\"" + ie.getAttribute("partname", 0).ToString() + "\"" + (ie.style.width == null ? "" : " width=\"" + ie.style.width.ToString() + "\"") + (ie.style.height == null ? "" : " height=\"" + ie.style.height.ToString() + "\"") + "></ftdp:control>";
                Clipboard.SetDataObject(nodeHTML);
                consts.globalConst.MdiForm.MainStatus.Text = "Copy Part Tag OK：" + partid;
            }
        }


    }
}
