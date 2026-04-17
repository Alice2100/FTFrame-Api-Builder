using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Silver.UI;
using SiteMatrix.Style;
using SiteMatrix.functions;
using SiteMatrix.consts;
using SiteMatrix.database;
using SiteMatrix.forms;
using SiteMatrix.SiteClass;
using mshtml;
using htmleditocx;
using SiteMatrix.Page;
using SiteMatrix.Common;
using System.Diagnostics;
using System.Collections;

namespace SiteMatrix.forms
{
    public partial class MainForm : Form
    {

        FileExplorer fe = new FileExplorer();
        public MainForm()
        {
            InitializeComponent();
            #region FontList
            T2Font.Items.Clear();
            FontFamily[] ff = FontFamily.Families;
            for (int x = 0; x < ff.Length; x++)
            {
                //System.Drawing.Font font = null;

                //// Create the font - based on the styles available.
                //if (ff[x].IsStyleAvailable(FontStyle.Regular))
                //    font = new System.Drawing.Font(
                //        ff[x].Name,
                //        T2Font.Font.Size
                //        );
                //else if (ff[x].IsStyleAvailable(FontStyle.Bold))
                //    font = new System.Drawing.Font(
                //        ff[x].Name,
                //        T2Font.Font.Size,
                //        FontStyle.Bold
                //        );
                //else if (ff[x].IsStyleAvailable(FontStyle.Italic))
                //    font = new System.Drawing.Font(
                //        ff[x].Name,
                //        T2Font.Font.Size,
                //        FontStyle.Italic
                //        );
                //else if (ff[x].IsStyleAvailable(FontStyle.Strikeout))
                //    font = new System.Drawing.Font(
                //        ff[x].Name,
                //        T2Font.Font.Size,
                //        FontStyle.Strikeout
                //        );
                //else if (ff[x].IsStyleAvailable(FontStyle.Underline))
                //    font = new System.Drawing.Font(
                //        ff[x].Name,
                //        T2Font.Font.Size,
                //        FontStyle.Underline
                //        );

                //// Should we add the item?
                //if (font != null)
                if (ff[x].IsStyleAvailable(FontStyle.Regular)&&ff[x].IsStyleAvailable(FontStyle.Bold)&&ff[x].IsStyleAvailable(FontStyle.Italic)&&ff[x].IsStyleAvailable(FontStyle.Strikeout)&&ff[x].IsStyleAvailable(FontStyle.Underline))
                T2Font.Items.Add(ff[x].Name);

            } // End for all the fonts.
            T2Font.SelectedIndex = 0;
            #endregion
            #region InitForm
            CustomProfessionalColors cfc = new CustomProfessionalColors();
            cfc.UseSystemColors = true;
            ToolStripProfessionalRenderer tsprdnr = new ToolStripProfessionalRenderer(cfc);
            menuStrip_main.Renderer = tsprdnr;
            toolStrip_Common.Renderer = tsprdnr;
            toolStrip_SiteManage.Renderer = tsprdnr;
            toolStrip_TextEdit.Renderer = tsprdnr;
            toolStripContainer1.TopToolStripPanel.Renderer = tsprdnr;
            toolStripContainer1.LeftToolStripPanel.Renderer = tsprdnr;
            toolStripContainer1.RightToolStripPanel.Renderer = tsprdnr;
            toolStripContainer1.BottomToolStripPanel.Renderer = tsprdnr;
            toolStripContainer2.TopToolStripPanel.Renderer = tsprdnr;
            toolStripContainer2.LeftToolStripPanel.Renderer = tsprdnr;
            toolStripContainer2.RightToolStripPanel.Renderer = tsprdnr;
            toolStripContainer2.BottomToolStripPanel.Renderer = tsprdnr;
            WorkSpaceSiteTool.Renderer = tsprdnr;
            WorkSpaceControlTool.Renderer = tsprdnr;
            PropertyFileTool.Renderer = tsprdnr;
            CMTool.Renderer = tsprdnr;
            CMSite.Renderer = tsprdnr;
            CMControl.Renderer = tsprdnr;
            this.toolStripContainer1.Height = this.toolStripContainer1.TopToolStripPanel.Height - 1;
            this.toolStripContainer2.Height = this.toolStripContainer2.BottomToolStripPanel.Height;
            if (mdifromConst.LoadDefault)
                LoadDefaultStyle();
            else
                LoadCustomStyle();
            MainToolBox.SmallImageList = globalConst.ControlsImages;
            MainToolBox.LargeImageList = globalConst.ControlsImages; 
            MainToolBox.AddTab("0", 0);
            MainToolBox.AddTab("1", 27);
            MainToolBox.AddTab("2", 72);
            MainToolBox.AddTab("3", 30);
            MainToolBox.AddTab("4", 28);
            MainToolBox.AddTab("5", 55);
            MainToolBox.AllowSwappingByDragDrop =true;
            InitToolBoxTads();
            ProOthers.Enabled = false;
            CurPropTag.Enabled = false;
            WorkSpaceSiteTool.Enabled = false;
            WorkSpaceControlTool.Enabled = false;
            RefreshLastListMenus();
            #region HTML
            MainToolBox[0].View = ViewMode.List;
            InitToolBoxHTMLElements();
            #endregion
            #region Controls
            MainToolBox[1].View = ViewMode.List;
            MainToolBox[2].View = ViewMode.List;
            InitToolBoxControls();
            #endregion
            #region Snippets
            MainToolBox[4].View = ViewMode.List;
            InitToolBoxSnippets();
            #endregion
            #region FORM
            MainToolBox[3].View = ViewMode.List;
            #endregion
            #region Page
            MainToolBox[5].View = ViewMode.List;
            #endregion
            MainToolBox.SelectedTabIndex = 5;
            ChangeTabPageActiveColor(WorkSpace);
            ChangeTabPageActiveColor(Property);
            #endregion
            #region ImageList Bound
            WorkSpace.ImageList = globalConst.Imgs;
            Property.ImageList = globalConst.Imgs;
            FileOpend.ImageList = globalConst.Imgs;
            #endregion
            #region Tree
            SiteTree.ImageList = globalConst.Imgs;
            ControlTree.ImageList = globalConst.Imgs;
            SiteTree.Nodes.Clear();
            ControlTree.Nodes.Clear();
            SiteTree.Enabled = false;
            ControlTree.Enabled = false;
            ControlTree.AllowDrop = true;
            //SiteTree.AllowDrop = true;
            
            SiteTree.MouseDown += new MouseEventHandler(SiteTree_MouseDown);
            SiteTree.MouseUp += new MouseEventHandler(SiteTree_MouseUp);
            SiteTree.DoubleClick += new EventHandler(SiteTree_DoubleClick);
            ControlTree.MouseDown += new MouseEventHandler(ControlTree_MouseDown);
            ControlTree.MouseUp += new MouseEventHandler(ControlTree_MouseUp);
            ControlTree.DoubleClick += new EventHandler(ControlTree_DoubleClick);
            ControlTree.ItemDrag += new ItemDragEventHandler(treeView_ItemDrag);
            ControlTree.DragOver += new DragEventHandler(treeView3_DragOver);
            ControlTree.DragDrop += new DragEventHandler(ControlTree_DragDrop);
            ControlTree.DragLeave += new EventHandler(treeView3_DragLeave);
            ControlTree.DragEnter += new DragEventHandler(treeView_DragEnter);
            ControlTree.GiveFeedback += new GiveFeedbackEventHandler(ControlTree_GiveFeedback);
            MainToolBox.DragDrop += new DragEventHandler(ControlTree_DragDrop);
            MainToolBox.DragEnter += new DragEventHandler(treeView_DragEnter);
            MainToolBox.DragLeave += new EventHandler(treeView3_DragLeave);
            MainToolBox.DragOver += new DragEventHandler(toolbox_DragOver);
            MainToolBox.GiveFeedback += new GiveFeedbackEventHandler(ControlTree_GiveFeedback);
            MainToolBox.OnBeginDragDrop += new PreDragDropHandler(MainToolBox_OnBeginDragDrop);
            MainToolBox.DragDropFinished += new DragDropFinishedHandler(MainToolBox_DragDropFinished);
            #endregion
            #region ImageForm
            imgform = new ImageForm.ImageForm();
            imgform.TopLevel = false;
            imgform.Visible = false;
            this.Controls.Add(imgform);
            #endregion
            #region InitFormTrees
            FilesList.ImageList = globalConst.Imgs;
            FilesList.Nodes.Clear();
            fe.CreateTree(FilesList);
            #endregion
            #region Language

            string sql = "select * from languages";
            DR dr = new DR(globalConst.ConfigConn.OpenRecord(sql));
            ToolStripMenuItem mi;
            while (dr.Read())
            {
                mi = new ToolStripMenuItem();
                MenuLanguage.DropDownItems.Add(mi);
                mi.Text = dr.getString("thename");
                mi.Click += new EventHandler(mi_Click);
            }
            dr.Close();
            LanguageApply();
            #endregion
            if(globalConst.FullVersion)
            {
                this.Text = "FTDP Full Version";
            }
            else
            {
                this.Text = "FTDP Developer Version";
            }
            KeyPreview = true;
        }


        void MainToolBox_DragDropFinished(ToolBoxItem sender, DragDropEffects e)
        {
            classes.EditorEvent.MoveEvents();
            HasToolBoxDraged = false;
            CloseImageForm();
            ToolBoxBeginDrag = false;
        }
        public bool ToolBoxBeginDrag = false;
        void MainToolBox_OnBeginDragDrop(ToolBoxItem sender, PreDragDropEventArgs e)
        {
            ToolBoxBeginDrag = true;
        }
        private void mi_Click(object sender, EventArgs e)
        {
            try
            {
                str.ShowStatus(res.ToolBox.GetString("Applying Language"));
                string sql = "select thevalue from languages where thename='" + ((ToolStripMenuItem)sender).Text + "'";
                string thevalue = globalConst.ConfigConn.GetString(sql);
                sql = "update system set thevalue='" + thevalue + "' where name='curlanguage'";
                globalConst.ConfigConn.execSql(sql);
                globalConst.Language = thevalue;
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(globalConst.Language);System.Threading.Thread.CurrentThread.CurrentUICulture=new System.Globalization.CultureInfo(globalConst.Language);
                LanguageApply();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
            finally
            {
                str.StatusClear();
            }
        }
        private void LanguageApply()
        {
            #region menu
            MenuEdit.Text = res.MainForm.GetString("MenuEdit");
            MenuEditAddSnippet.Text = res.MainForm.GetString("MenuEditAddSnippet");
            MenuEditClonePart.Text = res.MainForm.GetString("MenuEditClonePart");
            MenuEditCopy.Text = res.MainForm.GetString("MenuEditCopy");
            MenuEditCreatePart.Text = res.MainForm.GetString("MenuEditCreatePart");
            MenuEditCut.Text = res.MainForm.GetString("MenuEditCut");
            MenuEditDelete.Text = res.MainForm.GetString("MenuEditDelete");
            MenuEditEditTag.Text = res.MainForm.GetString("MenuEditEditTag");
            MenuEditFind.Text = res.MainForm.GetString("MenuEditFind");
            MenuEditPaste.Text = res.MainForm.GetString("MenuEditPaste");
            MenuEditRedo.Text = res.MainForm.GetString("MenuEditRedo");
            MenuEditReplace.Text = res.MainForm.GetString("MenuEditReplace");
            MenuEditSelectAll.Text = res.MainForm.GetString("MenuEditSelectAll");
            MenuEditUndo.Text = res.MainForm.GetString("MenuEditUndo");
            MenuEditAdvance.Text = res.MainForm.GetString("MenuEditAdvance");
            MenuEditAdvEle2Label.Text = res.MainForm.GetString("MenuEditAdvEle2Label");
            CreateGUIDMenuItem.Text = res.form.GetString("String97");
            toolStripMenuItem1.Text = res.form.GetString("String98");
            MenuFile.Text = res.MainForm.GetString("MenuFile");
            MenuFileAddSite.Text = res.MainForm.GetString("MenuFileAddSite");
            MenuFileCloseFile.Text = res.MainForm.GetString("MenuFileCloseFile");
            MenuFileCloseSite.Text = res.MainForm.GetString("MenuFileCloseSite");
            MenuFileExit.Text = res.MainForm.GetString("MenuFileExit");
            MenuFileFileSaveAs.Text = res.MainForm.GetString("MenuFileFileSaveAs");
            MenuFileLastFiles.Text = res.MainForm.GetString("MenuFileLastFiles");
            MenuFileLastSites.Text = res.MainForm.GetString("MenuFileLastSites");
            MenuFileNewFreeFile.Text = res.MainForm.GetString("MenuFileNewFreeFile");
            MenuFileOpenFreeFile.Text = res.MainForm.GetString("MenuFileOpenFreeFile");
            MenuFileOpenSite.Text = res.MainForm.GetString("MenuFileOpenSite");
            MenuFileRegisterSite.Text = res.MainForm.GetString("MenuFileRegisterSite");
            MenuFileSaveAll.Text = res.MainForm.GetString("MenuFileSaveAll");
            MenuFileSaveFile.Text = res.MainForm.GetString("MenuFileSaveFile");
            MenuFileOpenWebPage.Text = res.MainForm.GetString("MenuFileOpenWebPage");
            MenuFormat.Text = res.MainForm.GetString("MenuFormat");
            MenuFormatAlignCenter.Text = res.MainForm.GetString("MenuFormatAlignCenter");
            MenuFormatAlignDefault.Text = res.MainForm.GetString("MenuFormatAlignDefault");
            MenuFormatAlignLeft.Text = res.MainForm.GetString("MenuFormatAlignLeft");
            MenuFormatAlignRight.Text = res.MainForm.GetString("MenuFormatAlignRight");
            MenuFormatBackColor.Text = res.MainForm.GetString("MenuFormatBackColor");
            MenuFormatBlockFormat.Text = res.MainForm.GetString("MenuFormatBlockFormat");
            MenuFormatBlockAddress.Text = res.MainForm.GetString("MenuFormatBlockAddress");
            MenuFormatBlockFormatFormatted.Text = res.MainForm.GetString("MenuFormatBlockFormatFormatted");
            MenuFormatBlockFormatParagraph.Text = res.MainForm.GetString("MenuFormatBlockFormatParagraph"); 
            MenuFormatBlockFormatHeading1.Text = res.MainForm.GetString("MenuFormatBlockFormatHeading1");
            MenuFormatBlockFormatHeading2.Text = res.MainForm.GetString("MenuFormatBlockFormatHeading2");
            MenuFormatBlockFormatHeading3.Text = res.MainForm.GetString("MenuFormatBlockFormatHeading3");
            MenuFormatBlockFormatHeading4.Text = res.MainForm.GetString("MenuFormatBlockFormatHeading4");
            MenuFormatBlockFormatHeading5.Text = res.MainForm.GetString("MenuFormatBlockFormatHeading5");
            MenuFormatBlockFormatHeading6.Text = res.MainForm.GetString("MenuFormatBlockFormatHeading6");
            MenuFormatBlockFormatNormal.Text = res.MainForm.GetString("MenuFormatBlockFormatNormal");
            MenuFormatBold.Text = res.MainForm.GetString("MenuFormatBold");
            MenuFormatForeColor.Text = res.MainForm.GetString("MenuFormatForeColor");
            MenuFormatIndent.Text = res.MainForm.GetString("MenuFormatIndent");
            MenuFormatItalic.Text = res.MainForm.GetString("MenuFormatItalic");
            MenuFormatLayer.Text = res.MainForm.GetString("MenuFormatLayer");
            MenuFormatLayerBack.Text = res.MainForm.GetString("MenuFormatLayerBack");
            MenuFormatLayerCancel.Text = res.MainForm.GetString("MenuFormatLayerCancel");
            MenuFormatLayerFore.Text = res.MainForm.GetString("MenuFormatLayerFore");
            MenuFormatLayerSet.Text = res.MainForm.GetString("MenuFormatLayerSet");
            MenuFormatLinethrough.Text = res.MainForm.GetString("MenuFormatLinethrough");
            MenuFormatLTR.Text = res.MainForm.GetString("MenuFormatLTR");
            MenuFormatRTL.Text = res.MainForm.GetString("MenuFormatRTL");
            MenuFormatStyle.Text = res.MainForm.GetString("MenuFormatStyle");
            MenuFormatSubscript.Text = res.MainForm.GetString("MenuFormatSubscript");
            MenuFormatSuperscript.Text = res.MainForm.GetString("MenuFormatSuperscript");
            MenuFormatUnderline.Text = res.MainForm.GetString("MenuFormatUnderline");
            MenuFormatUnindent.Text = res.MainForm.GetString("MenuFormatUnindent");
            MenuHelp.Text = res.MainForm.GetString("MenuHelp");
            MenuHelpAbout.Text = res.MainForm.GetString("MenuHelpAbout");
            MenuHelpD4SoftHomePage.Text = res.MainForm.GetString("MenuHelpD4SoftHomePage");
            MenuHelpD4System.Text = res.MainForm.GetString("MenuHelpD4System");
            MenuHelpSendFeedback.Text = res.MainForm.GetString("MenuHelpSendFeedback");
            MenuHelpSiteMatrix.Text = res.MainForm.GetString("MenuHelpSiteMatrix");
            MenuLanguage.Text = res.MainForm.GetString("MenuLanguage");
            MenuPage.Text = res.MainForm.GetString("MenuPage");
            MenuPageCancelLink.Text = res.MainForm.GetString("MenuPageCancelLink");
            MenuPageClearTable.Text = res.MainForm.GetString("MenuPageClearTable");
            MenuPageDeleteColumn.Text = res.MainForm.GetString("MenuPageDeleteColumn");
            MenuPageDeleteRow.Text = res.MainForm.GetString("MenuPageDeleteRow");
            MenuPageDivWrap.Text = res.MainForm.GetString("MenuPageDivWrap");
            MenuPageEditTable.Text = res.MainForm.GetString("MenuPageEditTable");
            MenuPageElementButton.Text = res.MainForm.GetString("MenuPageElementButton");
            MenuPageElementCheckBox.Text = res.MainForm.GetString("MenuPageElementCheckBox");
            MenuPageElementFrame.Text = res.MainForm.GetString("MenuPageElementFrame");
            MenuPageElementHorizon.Text = res.MainForm.GetString("MenuPageElementHorizon");
            MenuPageElementImage.Text = res.MainForm.GetString("MenuPageElementImage");
            MenuPageElementInputFile.Text = res.MainForm.GetString("MenuPageElementInputFile");
            MenuPageElementInputHidden.Text = res.MainForm.GetString("MenuPageElementInputHidden");
            MenuPageElementInputPassword.Text = res.MainForm.GetString("MenuPageElementInputPassword");
            MenuPageElementInputReset.Text = res.MainForm.GetString("MenuPageElementInputReset");
            MenuPageElementInputText.Text = res.MainForm.GetString("MenuPageElementInputText");
            MenuPageElementMarquee.Text = res.MainForm.GetString("MenuPageElementMarquee");
            MenuPageElementMultipleSelect.Text = res.MainForm.GetString("MenuPageElementMultipleSelect");
            MenuPageElementPanel.Text = res.MainForm.GetString("MenuPageElementPanel");
            MenuPageElementParagraph.Text = res.MainForm.GetString("MenuPageElementParagraph");
            MenuPageElementRadio.Text = res.MainForm.GetString("MenuPageElementRadio");
            MenuPageElementSelect.Text = res.MainForm.GetString("MenuPageElementSelect");
            MenuPageElementSubmitButton.Text = res.MainForm.GetString("MenuPageElementSubmitButton");
            MenuPageElementSubmitImage.Text = res.MainForm.GetString("MenuPageElementSubmitImage");
            MenuPageElementTextArea.Text = res.MainForm.GetString("MenuPageElementTextArea");
            MenuPageInsertColumn.Text = res.MainForm.GetString("MenuPageInsertColumn");
            MenuPageInsertElement.Text = res.MainForm.GetString("MenuPageInsertElement");
            MenuPageInsertLink.Text = res.MainForm.GetString("MenuPageInsertLink");
            MenuPageInsertRow.Text = res.MainForm.GetString("MenuPageInsertRow");
            MenuPageInsertTable.Text = res.MainForm.GetString("MenuPageInsertTable");
            MenuPageMergeCellDown.Text = res.MainForm.GetString("MenuPageMergeCellDown");
            MenuPageMergeCellLeft.Text = res.MainForm.GetString("MenuPageMergeCellLeft");
            MenuPageMergeCellRight.Text = res.MainForm.GetString("MenuPageMergeCellRight");
            MenuPageMergeCellUp.Text = res.MainForm.GetString("MenuPageMergeCellUp");
            MenuPageSelectTable.Text = res.MainForm.GetString("MenuPageSelectTable");
            MenuPageSpanWrap.Text = res.MainForm.GetString("MenuPageSpanWrap");
            MenuPageSplit2Columns.Text = res.MainForm.GetString("MenuPageSplit2Columns");
            MenuPageSplit2Rows.Text = res.MainForm.GetString("MenuPageSplit2Rows");
            MenuSite.Text = res.MainForm.GetString("MenuSite");
            MenuSiteCheck.Text = res.MainForm.GetString("MenuSiteCheck");
            MenuSiteClearFiles.Text = res.MainForm.GetString("MenuSiteClearFiles");
            MenuSiteExportSite.Text = res.MainForm.GetString("MenuSiteExportSite");
            MenuSiteExportTemplate.Text = res.MainForm.GetString("MenuSiteExportTemplate");
            MenuSiteImportPage.Text = res.MainForm.GetString("MenuSiteImportPage");
            MenuSiteImportSite.Text = res.MainForm.GetString("MenuSiteImportSite");
            MenuSiteImportTemplate.Text = res.MainForm.GetString("MenuSiteImportTemplate");
            MenuSiteManage.Text = res.MainForm.GetString("MenuSiteManage");
            MenuSitePublish.Text = res.MainForm.GetString("MenuSitePublish");
            MenuSiteRefreshShare.Text = res.MainForm.GetString("MenuSiteRefreshShare");
            MenuSiteSave.Text = res.MainForm.GetString("MenuSiteSave");
            MenuSiteSynchro.Text = res.MainForm.GetString("MenuSiteSynchro");
            MenuSiteTamplateManage.Text = res.MainForm.GetString("MenuSiteTamplateManage");
            MenuSiteModifyCSS.Text = res.MainForm.GetString("MenuSiteMofidyCSS");
            MenuTool.Text = res.MainForm.GetString("MenuTool");
            MenuToolContentSet.Text = res.MainForm.GetString("MenuToolContentSet");
            MenuToolControlManage.Text = res.MainForm.GetString("MenuToolControlManage");
            MenuToolDNS.Text = res.MainForm.GetString("MenuToolDNS");
            MenuToolOption.Text = res.MainForm.GetString("MenuToolOption");
            MenuToolSystemSet.Text = res.MainForm.GetString("MenuToolSystemSet");
            MenuView.Text = res.MainForm.GetString("MenuView");
            MenuViewAlive.Text = res.MainForm.GetString("MenuViewAlive");
            MenuViewBorder.Text = res.MainForm.GetString("MenuViewBorder");
            MenuViewCode.Text = res.MainForm.GetString("MenuViewCode");
            MenuViewDesign.Text = res.MainForm.GetString("MenuViewDesign");
            MenuViewGrid.Text = res.MainForm.GetString("MenuViewGrid");
            MenuViewPreview.Text = res.MainForm.GetString("MenuViewPreview");
            MenuViewWebView.Text = res.MainForm.GetString("MenuViewWebView");
            MenuViewProperty.Text = res.MainForm.GetString("MenuViewProperty");
            MenuViewTag.Text = res.MainForm.GetString("MenuViewTag");
            MenuViewToolBox.Text = res.MainForm.GetString("MenuViewToolBox");
            MenuViewWorkSpace.Text = res.MainForm.GetString("MenuViewWorkSpace");
            MenuWindow.Text = res.MainForm.GetString("MenuWindow");
            MenuWindowAllClose.Text = res.MainForm.GetString("MenuWindowAllClose");
            MenuWindowCascade.Text = res.MainForm.GetString("MenuWindowCascade");
            MenuWindowTileHorizontal.Text = res.MainForm.GetString("MenuWindowTileHorizontal");
            MenuWindowTileVertical.Text = res.MainForm.GetString("MenuWindowTileVertical");
            #endregion
            #region toolbar
            T1OpenWebPage.ToolTipText = res.MainForm.GetString("T1OpenWebPage");
            T1Border.ToolTipText = res.MainForm.GetString("T1Border");
            T1Code.ToolTipText = res.MainForm.GetString("T1Code");
            T1Copy.ToolTipText = res.MainForm.GetString("T1Copy");
            T1Cut.ToolTipText = res.MainForm.GetString("T1Cut");
            T1Design.ToolTipText = res.MainForm.GetString("T1Design");
            T1Grid.ToolTipText = res.MainForm.GetString("T1Grid");
            T1Help.ToolTipText = res.MainForm.GetString("T1Help");
            T1Live.ToolTipText = res.MainForm.GetString("T1Live");
            T1NewFreeFile.ToolTipText = res.MainForm.GetString("T1NewFreeFile");
            T1OpenFreeFile.ToolTipText = res.MainForm.GetString("T1OpenFreeFile");
            T1Paste.ToolTipText = res.MainForm.GetString("T1Paste");
            T1Preview.ToolTipText = res.MainForm.GetString("T1Preview");
            T1Redo.ToolTipText = res.MainForm.GetString("T1Redo");
            T1SaveAllFile.ToolTipText = res.MainForm.GetString("T1SaveAllFile");
            T1SaveFile.ToolTipText = res.MainForm.GetString("T1SaveFile");
            T1Search.ToolTipText = res.MainForm.GetString("T1Search");
            T1Tag.ToolTipText = res.MainForm.GetString("T1Tag");
            T1Undo.ToolTipText = res.MainForm.GetString("T1Undo");
            T2AlignLeft.ToolTipText = res.MainForm.GetString("T2AlignLeft");
            T2AlignMiddle.ToolTipText = res.MainForm.GetString("T2AlignMiddle");
            T2AlignRight.ToolTipText = res.MainForm.GetString("T2AlignRight");
            T2BackColor.ToolTipText = res.MainForm.GetString("T2BackColor");
            T2Block.ToolTipText = res.MainForm.GetString("T2Block");
            T2Bold.ToolTipText = res.MainForm.GetString("T2Bold");
            T2EditTag.ToolTipText = res.MainForm.GetString("T2EditTag");
            T2Font.ToolTipText = res.MainForm.GetString("T2Font");
            T2ForeColor.ToolTipText = res.MainForm.GetString("T2ForeColor");
            T2Indent.ToolTipText = res.MainForm.GetString("T2Indent");
            T2Italic.ToolTipText = res.MainForm.GetString("T2Italic");
            T2Layer.ToolTipText = res.MainForm.GetString("T2Layer");
            T2Size.ToolTipText = res.MainForm.GetString("T2Size");
            T2Underline.ToolTipText = res.MainForm.GetString("T2Underline");
            T2UnIndent.ToolTipText = res.MainForm.GetString("T2UnIndent");
            T3CloseSite.ToolTipText = res.MainForm.GetString("T3CloseSite");
            T3ControlManage.ToolTipText = res.MainForm.GetString("T3ControlManage");
            T3OpenSite.ToolTipText = res.MainForm.GetString("T3OpenSite");
            T3PageImport.ToolTipText = res.MainForm.GetString("T3PageImport");
            T3SaveSite.ToolTipText = res.MainForm.GetString("T3SaveSite");
            T3SiteAdd.ToolTipText = res.MainForm.GetString("T3SiteAdd");
            T3SiteCheck.ToolTipText = res.MainForm.GetString("T3SiteCheck");
            T3SiteExport.ToolTipText = res.MainForm.GetString("T3SiteExport");
            T3SiteImport.ToolTipText = res.MainForm.GetString("T3SiteImport");
            T3SiteManage.ToolTipText = res.MainForm.GetString("T3SiteManage");
            T3SitePublish.ToolTipText = res.MainForm.GetString("T3SitePublish");
            T3SiteRegister.ToolTipText = res.MainForm.GetString("T3SiteRegister");
            T3SycronSite.ToolTipText = res.MainForm.GetString("T3SycronSite");
            T3TemplateExport.ToolTipText = res.MainForm.GetString("T3TemplateExport");
            T3TemplateImport.ToolTipText = res.MainForm.GetString("T3TemplateImport");
            T3TemplateManage.ToolTipText = res.MainForm.GetString("T3TemplateManage");
            T3UpdateData.ToolTipText = res.MainForm.GetString("T3UpdateData");
            toolbar_property.ToolTipText = res.MainForm.GetString("toolbar_property");
            toolbar_toolbox.ToolTipText = res.MainForm.GetString("toolbar_toolbox");
            toolbar_workpace.ToolTipText = res.MainForm.GetString("toolbar_workpace");
            ShowHiddenSiteTool.ToolTipText = res.MainForm.GetString("ShowHiddenSiteTool");
            ShowHiddenTextTool.ToolTipText = res.MainForm.GetString("ShowHiddenTextTool");
            ResetLayerOut.ToolTipText = res.MainForm.GetString("ResetLayerOut");
            int ItemSelect = T2Block.SelectedIndex;
            T2Block.Items.Clear();
            T2Block.Items.Add(res.MainForm.GetString("MenuFormatBlockFormatNormal").Substring(3));
            T2Block.Items.Add(res.MainForm.GetString("MenuFormatBlockAddress").Substring(3));
            T2Block.Items.Add(res.MainForm.GetString("MenuFormatBlockFormatFormatted").Substring(3));
            T2Block.Items.Add(res.MainForm.GetString("MenuFormatBlockFormatParagraph").Substring(3));
            T2Block.Items.Add(res.MainForm.GetString("MenuFormatBlockFormatHeading1").Substring(3));
            T2Block.Items.Add(res.MainForm.GetString("MenuFormatBlockFormatHeading2").Substring(3));
            T2Block.Items.Add(res.MainForm.GetString("MenuFormatBlockFormatHeading3").Substring(3));
            T2Block.Items.Add(res.MainForm.GetString("MenuFormatBlockFormatHeading4").Substring(3));
            T2Block.Items.Add(res.MainForm.GetString("MenuFormatBlockFormatHeading5").Substring(3));
            T2Block.Items.Add(res.MainForm.GetString("MenuFormatBlockFormatHeading6").Substring(3));
            if (ItemSelect < 0) ItemSelect = 0;
            T2Block.SelectedIndex = ItemSelect;
            #endregion
            #region toolbox
            MainToolBox[0].Caption = res.ToolBox.GetString("Caption0");
            MainToolBox[1].Caption = res.ToolBox.GetString("Caption1");
            MainToolBox[2].Caption = res.ToolBox.GetString("Caption2");
            MainToolBox[3].Caption = res.form.GetString("String4");
            MainToolBox[4].Caption = res.ToolBox.GetString("Caption3");
            AddSneppetsToolStripMenuItem.Text = res.ToolBox.GetString("AddSneppetsToolStripMenuItem");
            renameToolStripMenuItem.Text = res.ToolBox.GetString("renameToolStripMenuItem");
            removeToolStripMenuItem.Text = res.ToolBox.GetString("removeToolStripMenuItem");
            addControlToolStripMenuItem1.Text = res.ToolBox.GetString("addControlToolStripMenuItem1");
            removeControlToolStripMenuItem.Text = res.ToolBox.GetString("removeControlToolStripMenuItem");
            propertyToolStripMenuItem.Text = res.ToolBox.GetString("propertyToolStripMenuItem");
            largeItemToolStripMenuItem.Text = res.ToolBox.GetString("largeItemToolStripMenuItem");
            listItemToolStripMenuItem.Text = res.ToolBox.GetString("listItemToolStripMenuItem");
            smallItemToolStripMenuItem.Text = res.ToolBox.GetString("smallItemToolStripMenuItem");
            //MainToolBox[0][0].Caption = res.ToolBox.GetString("Pointer");
            //MainToolBox[1][0].Caption = res.ToolBox.GetString("Pointer");
            //MainToolBox[2][0].Caption = res.ToolBox.GetString("Pointer");
            //MainToolBox[3][0].Caption = res.ToolBox.GetString("Pointer");
            //MainToolBox[0][1].Caption = res.ToolBox.GetString("Label");
            //MainToolBox[0][2].Caption = res.ToolBox.GetString("HyperLink");
            //MainToolBox[0][3].Caption = res.ToolBox.GetString("TextBox");
            //MainToolBox[0][4].Caption = res.ToolBox.GetString("TextArea");
            //MainToolBox[0][5].Caption = res.ToolBox.GetString("Password");
            //MainToolBox[0][6].Caption = res.ToolBox.GetString("Button");
            //MainToolBox[0][7].Caption = res.ToolBox.GetString("Submit Button");
            //MainToolBox[0][8].Caption = res.ToolBox.GetString("Reset Button");
            //MainToolBox[0][9].Caption = res.ToolBox.GetString("Image Button");
            //MainToolBox[0][10].Caption = res.ToolBox.GetString("CheckBox");
            //MainToolBox[0][11].Caption = res.ToolBox.GetString("Redio Button");
            //MainToolBox[0][12].Caption = res.ToolBox.GetString("ComboBox");
            //MainToolBox[0][13].Caption = res.ToolBox.GetString("ListBox");
            //MainToolBox[0][14].Caption = res.ToolBox.GetString("Hidden Field");
            //MainToolBox[0][15].Caption = res.ToolBox.GetString("File Upload");
            //MainToolBox[0][16].Caption = res.ToolBox.GetString("GroupBox");
            //MainToolBox[0][17].Caption = res.ToolBox.GetString("Anchor");
            //MainToolBox[0][18].Caption = res.ToolBox.GetString("Image");
            //MainToolBox[0][19].Caption = res.ToolBox.GetString("Table");
            //MainToolBox[0][20].Caption = res.ToolBox.GetString("Span");
            //MainToolBox[0][21].Caption = res.ToolBox.GetString("Div");
            //MainToolBox[0][22].Caption = res.ToolBox.GetString("Panel");
            //MainToolBox[0][23].Caption = res.ToolBox.GetString("IFrame");
            //MainToolBox[0][24].Caption = res.ToolBox.GetString("Horizontal Rule");
            //MainToolBox[0][25].Caption = res.ToolBox.GetString("Form");
            MainToolBox[3].DeleteAllItems();
            MainToolBox[3].AddItem("Pointer", 1, 1, false, "Pointer");
            MainToolBox[3].AddItem(res.form.GetString("String5"), 63, 63, true, "Text Box");
            MainToolBox[3].AddItem(res.form.GetString("String40"), 59, 59, true, "Password");
            MainToolBox[3].AddItem(res.form.GetString("String6"), 66, 66, true, "Combo Box");
            MainToolBox[3].AddItem(res.form.GetString("String7"), 64, 64, true, "Text Area");
            MainToolBox[3].AddItem(res.form.GetString("String8"), 50, 50, true,"Radio Box");
            MainToolBox[3].AddItem(res.form.GetString("String9"), 52, 52, true, "Check Box");
            MainToolBox[3].AddItem(res.form.GetString("String10"), 65, 65, true, "File Box");
            MainToolBox[3].AddItem(res.form.GetString("String11"), 49, 49, true, "Label");
            MainToolBox[3].AddItem(res.form.GetString("String12"), 55, 55, true,"Category");
            MainToolBox[3].AddItem(res.form.GetString("String13"), 60, 60, true, "Date Year");
            MainToolBox[3].AddItem(res.form.GetString("String67"), 70, 70, true, "Date Month");
            MainToolBox[3].AddItem(res.form.GetString("String68"), 61, 61, true, "Date Day");
            MainToolBox[3].AddItem(res.form.GetString("String62"), 53, 53, true, "Filter Action");
            if (globalConst.FormDataMode)
            {
                MainToolBox[3].AddItem(res.form.GetString("String105"), 67, 67, true, "Data Action");
            }
            else
            {
                MainToolBox[3].AddItem(res.form.GetString("String14"), 67, 67, true, "New Action");
                MainToolBox[3].AddItem(res.form.GetString("String15"), 68, 68, true, "Mod Action");
            }
            MainToolBox[3].AddItem(res.form.GetString("String16"), 71, 71, true, "Reset Action");
            MainToolBox[3].AddItem(res.form.GetString("String19"), 62, 62, true, "Stat Filter");
            MainToolBox[3].AddItem(res.form.GetString("String71"), 54, 54, true, "Member Input");
            MainToolBox[3].AddItem(res.form.GetString("String79"), 51, 51, true, "Row Rate");
            //MainToolBox[3].AddItem("新增一行[Button]", 51, 51, true, "Row Rate2");
            MainToolBox[3].AddItem(res.form.GetString("String80"), 69, 69, true, "Row Index");
            if (globalConst.FormDataMode)
            {
                MainToolBox[3].AddItem(res.form.GetString("String106"), 57, 57, true, "Flow Data");
            }
            else
            {
                MainToolBox[3].AddItem(res.form.GetString("String72"), 57, 57, true, "Flow Add");
                MainToolBox[3].AddItem(res.form.GetString("String73"), 58, 58, true, "Flow Mod");
            }
            MainToolBox[3].AddItem(res.form.GetString("String74"), 56, 56, true, "Flow Flow");
            //用状态触发器来实现多级流程
            //MainToolBox[3].AddItem(res.form.GetString("String87"), 56, 56, true, "Flow Ext1");
            //MainToolBox[3].AddItem(res.form.GetString("String88"), 56, 56, true, "Flow Ext2");
            for (int i = MainToolBox[3].ItemCount - 1; i >= 0; i--)
            {
                MainToolBox[3].SelectedItemIndex = i;
            }

            MainToolBox[0].DeleteAllItems();
            MainToolBox[0].AddItem("Pointer", 1, 1, false, "Pointer");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Label"), 2, 2, true, "Label");
            MainToolBox[0].AddItem(res.ToolBox.GetString("HyperLink"), 3, 3, true, "HyperLink");
            MainToolBox[0].AddItem(res.ToolBox.GetString("TextBox"), 4, 4, true, "TextBox");
            MainToolBox[0].AddItem(res.ToolBox.GetString("TextArea"), 5, 5, true, "TextArea");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Password"), 6, 6, true, "Password");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Button"), 7, 7, true, "Button");
            MainToolBox[0].AddItem("Button", 7, 7, true, "Button2");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Submit Button"), 8, 8, true, "Submit Button");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Reset Button"), 9, 9, true, "Reset Button");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Image Button"), 10, 10, true, "Image Button");
            MainToolBox[0].AddItem(res.ToolBox.GetString("CheckBox"), 11, 11, true, "CheckBox");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Redio Button"), 12, 12, true, "Redio Button");
            MainToolBox[0].AddItem(res.ToolBox.GetString("ComboBox"), 13, 13, true, "ComboBox");
            MainToolBox[0].AddItem(res.ToolBox.GetString("ListBox"), 14, 14, true, "ListBox");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Hidden Field"), 15, 15, true, "Hidden Field");
            MainToolBox[0].AddItem(res.ToolBox.GetString("File Upload"), 16, 16, true, "File Upload");
            MainToolBox[0].AddItem(res.ToolBox.GetString("GroupBox"), 17, 17, true, "GroupBox");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Anchor"), 18, 18, true, "Anchor");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Image"), 19, 19, true, "Image");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Table"), 20, 20, true, "Table");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Span"), 21, 21, true, "Span");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Div"), 22, 22, true, "Div");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Panel"), 23, 23, true, "Panel");
            MainToolBox[0].AddItem(res.ToolBox.GetString("IFrame"), 24, 24, true, "IFrame");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Horizontal Rule"), 25, 25, true, "Horizontal Rule");
            MainToolBox[0].AddItem(res.ToolBox.GetString("Form"), 26, 26, true, "Form");
            for (int i = MainToolBox[0].ItemCount - 1; i >= 0; i--)
            {
                MainToolBox[0].SelectedItemIndex = i;
            }
            #endregion
            #region space
            SiteTabNewFolder.ToolTipText = res.Space.GetString("SiteTabNewFolder");
            SiteTabNewFile.ToolTipText = res.Space.GetString("SiteTabNewFile");
            SiteTabOpenFile.ToolTipText = res.Space.GetString("SiteTabOpenFile");
            SiteTabDelete.ToolTipText = res.Space.GetString("SiteTabDelete");
            SiteTabRefresh.ToolTipText = res.Space.GetString("SiteTabRefresh");
            ControlTabAddControl.ToolTipText = res.Space.GetString("ControlTabAddControl");
            ControlTabNewInstance.ToolTipText = res.Space.GetString("ControlTabNewInstance");
            ControlTabCloneInstance.ToolTipText = res.Space.GetString("ControlTabCloneInstance");
            ControlTabNewPart.ToolTipText = res.Space.GetString("ControlTabNewPart");
            ControlTabClonePart.ToolTipText = res.Space.GetString("ControlTabClonePart");
            ControlTabDelete.ToolTipText = res.Space.GetString("ControlTabDelete");
            ControlTabRefresh.ToolTipText = res.Space.GetString("SiteTabRefresh");
            openPageToolStripMenuItem.Text = res.Space.GetString("openPageToolStripMenuItem");
            newDirectoryToolStripMenuItem.Text = res.Space.GetString("newDirectoryToolStripMenuItem");
            newPageToolStripMenuItem.Text = res.Space.GetString("newPageToolStripMenuItem");
            deleteToolStripMenuItem.Text = res.Space.GetString("deleteToolStripMenuItem");
            refreshToolStripMenuItem.Text = res.Space.GetString("refreshToolStripMenuItem");
            pubpageMenuItem2.Text = res.Space.GetString("pubpagemenuitem");
            expandAllToolStripMenuItem.Text = res.Space.GetString("expandAllToolStripMenuItem");
            collapseAllToolStripMenuItem.Text = res.Space.GetString("collapseAllToolStripMenuItem");
            showFileNameToolStripMenuItem.Text = res.Space.GetString("showFileNameToolStripMenuItem");
            showCaptionToolStripMenuItem.Text = res.Space.GetString("showCaptionToolStripMenuItem");
            sortByToolStripMenuItem.Text = res.Space.GetString("sortByToolStripMenuItem");
            FileNameASC.Text = res.Space.GetString("FileNameASC");
            sortByFilenameDESCToolStripMenuItem1.Text = res.Space.GetString("sortByFilenameDESCToolStripMenuItem1");
            captionASCToolStripMenuItem.Text = res.Space.GetString("captionASCToolStripMenuItem");
            captionDESCToolStripMenuItem.Text = res.Space.GetString("captionDESCToolStripMenuItem");
            timeASCToolStripMenuItem.Text = res.Space.GetString("timeASCToolStripMenuItem");
            timeDESCToolStripMenuItem.Text = res.Space.GetString("timeDESCToolStripMenuItem");
            addControlToolStripMenuItem.Text = res.Space.GetString("addControlToolStripMenuItem");
            deleteControlToolStripMenuItem.Text = res.Space.GetString("deleteControlToolStripMenuItem");
            newControlInstance1ToolStripMenuItem.Text = res.Space.GetString("newControlInstance1ToolStripMenuItem");
            newControlInstance2ToolStripMenuItem.Text = res.Space.GetString("newControlInstance2ToolStripMenuItem");
            cloneControlInstanceToolStripMenuItem.Text = res.Space.GetString("cloneControlInstanceToolStripMenuItem");
            deleteControlInstanceToolStripMenuItem.Text = res.Space.GetString("deleteControlInstanceToolStripMenuItem");
            newPartInstanceToolStripMenuItem.Text = res.Space.GetString("newPartInstanceToolStripMenuItem");
            clonePartInstanceToolStripMenuItem.Text = res.Space.GetString("clonePartInstanceToolStripMenuItem");
            deleteToolStripMenuItem1.Text = res.Space.GetString("deleteToolStripMenuItem1");
            openPageToolStripMenuItem1.Text = res.Space.GetString("openPageToolStripMenuItem1");
            deletePageToolStripMenuItem.Text = res.Space.GetString("deletePageToolStripMenuItem");
            refreshToolStripMenuItem1.Text = res.Space.GetString("refreshToolStripMenuItem1");
            expandAllToolStripMenuItem1.Text = res.Space.GetString("expandAllToolStripMenuItem1");
            collapseAllToolStripMenuItem1.Text = res.Space.GetString("collapseAllToolStripMenuItem1");
            showNameToolStripMenuItem.Text = res.Space.GetString("showNameToolStripMenuItem");
            showCaptionToolStripMenuItem1.Text = res.Space.GetString("showCaptionToolStripMenuItem1");
            sortByToolStripMenuItem1.Text = res.Space.GetString("sortByToolStripMenuItem1");
            nameASCToolStripMenuItem.Text = res.Space.GetString("nameASCToolStripMenuItem");
            nameDESCToolStripMenuItem.Text = res.Space.GetString("nameDESCToolStripMenuItem");
            captionASCToolStripMenuItem1.Text = res.Space.GetString("captionASCToolStripMenuItem1");
            captionDESCToolStripMenuItem1.Text = res.Space.GetString("captionDESCToolStripMenuItem1");
            FilesListNewFolder.ToolTipText = res.Space.GetString("FilesListNewFolder");
            FilesListNewFile.ToolTipText = res.Space.GetString("FilesListNewFile");
            FilesListOpen.ToolTipText = res.Space.GetString("FilesListOpen");
            FilesListDelete.ToolTipText = res.Space.GetString("FilesListDelete");
            formNewPage.Text = res.form.GetString("String1");
            formNewPageUser.Text = res.form.GetString("String2");
            formNewPageManager.Text = res.form.GetString("String3");
            #endregion
            #region editors
            foreach (Form f in this.MdiChildren)
            {
                if(f.Name.Equals("Editor"))
                {
                    ((Editor)(f)).ApplyLanguage();
                }
            }
            #endregion
            #region propertygrid
            SiteMatrix.Page.PageWare.CategoryMain = res._pageware.GetString("c1");
            SiteMatrix.Page.PageWare.CategoryInfo = res._pageware.GetString("c2");
            SiteMatrix.Page.PageWare.CategoryControlInfo = res._pageware.GetString("c3");
            SiteMatrix.Page.PageWare.CategoryPartInfo = res._pageware.GetString("c4");
            SiteMatrix.Page.PageWare.CategoryForeInfo = res._pageware.GetString("c5");
            SiteMatrix.Page.PageWare.CategoryBackInfo = res._pageware.GetString("c6");
            SiteMatrix.Page.PageWare.CategoryControlData = res._pageware.GetString("c7");
            SiteMatrix.Page.PageWare.CategoryControlDataShare = res._pageware.GetString("c8");
            SiteMatrix.Page.PageWare.CategoryControlName = res._pageware.GetString("c9");
            SiteMatrix.Page.PageWare.CategoryPartHeight = res._pageware.GetString("c10");
            SiteMatrix.Page.PageWare.CategoryPartWidth = res._pageware.GetString("c11");
            SiteMatrix.Page.PageWare.CategoryPartPortal = res._pageware.GetString("c12");
            SiteMatrix.Page.PageWare.CategoryStyle = res._pageware.GetString("c13");


            SiteMatrix.PropertySpace.Site.PropertyPart.CategoryMain = res._pageware.GetString("c1");
            SiteMatrix.PropertySpace.Site.PropertyPart.CategoryInfo = res._pageware.GetString("c2");
            SiteMatrix.PropertySpace.Site.PropertyPart.CategoryControlInfo = res._pageware.GetString("c3");
            SiteMatrix.PropertySpace.Site.PropertyPart.CategoryPartInfo = res._pageware.GetString("c4");
            SiteMatrix.PropertySpace.Site.PropertyPart.CategoryForeInfo = res._pageware.GetString("c5");
            SiteMatrix.PropertySpace.Site.PropertyPart.CategoryBackInfo = res._pageware.GetString("c6");
            SiteMatrix.PropertySpace.Site.PropertyPart.CategoryControlData = res._pageware.GetString("c7");
            SiteMatrix.PropertySpace.Site.PropertyPart.CategoryControlDataShare = res._pageware.GetString("c8");
            SiteMatrix.PropertySpace.Site.PropertyPart.CategoryControlName = res._pageware.GetString("c9");
            SiteMatrix.PropertySpace.Site.PropertyPart.CategoryStyle = res._pageware.GetString("c13");

            SiteMatrix.PropertySpace.Site.PropertyControl.CategoryMain = res._pageware.GetString("c1");
            SiteMatrix.PropertySpace.Site.PropertyControl.CategoryInfo = res._pageware.GetString("c2");
            SiteMatrix.PropertySpace.Site.PropertyControl.CategoryControlInfo = res._pageware.GetString("c3");
            SiteMatrix.PropertySpace.Site.PropertyControl.CategoryForeInfo = res._pageware.GetString("c5");
            SiteMatrix.PropertySpace.Site.PropertyControl.CategoryBackInfo = res._pageware.GetString("c6");
            SiteMatrix.PropertySpace.Site.PropertyControl.CategoryControlData = res._pageware.GetString("c7");
            SiteMatrix.PropertySpace.Site.PropertyControl.CategoryControlDataShare = res._pageware.GetString("c8");
            SiteMatrix.PropertySpace.Site.PropertyControl.CategoryControlName = res._pageware.GetString("c9");
            SiteMatrix.PropertySpace.Site.PropertyControl.CategoryControlPara = res._pageware.GetString("ctlpara");

            SiteMatrix.PropertySpace.Site.PropertyControlMother.CategoryInfo = res._pageware.GetString("c2");
            SiteMatrix.PropertySpace.Site.PropertyControlMother.CategoryControlInfo = res._pageware.GetString("c3");
            SiteMatrix.PropertySpace.Site.PropertyControlMother.CategoryForeInfo = res._pageware.GetString("c5");
            SiteMatrix.PropertySpace.Site.PropertyControlMother.CategoryBackInfo = res._pageware.GetString("c6");


            SiteMatrix.Adapter.Property.NotSet = res._pageware.GetString("NotSet");
            SiteMatrix.StyleEditorAdapter.StyleEditorAdapter.NotSet = res._pageware.GetString("NotSet");


            SiteMatrix.PropertySpace.Site.PropertyPart.AccessMain = res._pageware.GetString("AccessMain");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessIP = res._pageware.GetString("AccessIP");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessIPControl = res._pageware.GetString("AccessIPControl");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessIPCondition = res._pageware.GetString("AccessIPCondition");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessIPConditionSide = res._pageware.GetString("AccessIPConditionSide");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessSession = res._pageware.GetString("AccessSession");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessSessionControl = res._pageware.GetString("AccessSessionControl");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessSessionCondition = res._pageware.GetString("AccessSessionCondition");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessSessionConditionSide = res._pageware.GetString("AccessSessionConditionSide");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessJump = res._pageware.GetString("AccessJump");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessJumpControl = res._pageware.GetString("AccessJumpControl");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessJumpAddress = res._pageware.GetString("AccessJumpAddress");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessTip = res._pageware.GetString("AccessTip");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessTipControl = res._pageware.GetString("AccessTipControl");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessTipContent = res._pageware.GetString("AccessTipContent");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessMain_Des = res._pageware.GetString("AccessMain_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessIP_Des = res._pageware.GetString("AccessIP_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessIPControl_Des = res._pageware.GetString("AccessIPControl_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessIPCondition_Des = res._pageware.GetString("AccessIPCondition_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessIPConditionSide_Des = res._pageware.GetString("AccessIPConditionSide_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessSession_Des = res._pageware.GetString("AccessSession_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessSessionControl_Des = res._pageware.GetString("AccessSessionControl_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessSessionCondition_Des = res._pageware.GetString("AccessSessionCondition_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessSessionConditionSide_Des = res._pageware.GetString("AccessSessionConditionSide_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessJump_Des = res._pageware.GetString("AccessJump_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessJumpControl_Des = res._pageware.GetString("AccessJumpControl_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessJumpAddress_Des = res._pageware.GetString("AccessJumpAddress_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessTip_Des = res._pageware.GetString("AccessTip_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessTipControl_Des = res._pageware.GetString("AccessTipControl_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessTipContent_Des = res._pageware.GetString("AccessTipContent_Des");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessNormal = res._pageware.GetString("AccessNormal");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessNormalNot = res._pageware.GetString("AccessNormalNot");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessActive = res._pageware.GetString("AccessActive");
            SiteMatrix.PropertySpace.Site.PropertyPart.AccessActiveNot = res._pageware.GetString("AccessActiveNot");

            SiteMatrix.Page.PageWare.AccessMain = res._pageware.GetString("AccessMain");
            SiteMatrix.Page.PageWare.AccessIP = res._pageware.GetString("AccessIP");
            SiteMatrix.Page.PageWare.AccessIPControl = res._pageware.GetString("AccessIPControl");
            SiteMatrix.Page.PageWare.AccessIPCondition = res._pageware.GetString("AccessIPCondition");
            SiteMatrix.Page.PageWare.AccessIPConditionSide = res._pageware.GetString("AccessIPConditionSide");
            SiteMatrix.Page.PageWare.AccessSession = res._pageware.GetString("AccessSession");
            SiteMatrix.Page.PageWare.AccessSessionControl = res._pageware.GetString("AccessSessionControl");
            SiteMatrix.Page.PageWare.AccessSessionCondition = res._pageware.GetString("AccessSessionCondition");
            SiteMatrix.Page.PageWare.AccessSessionConditionSide = res._pageware.GetString("AccessSessionConditionSide");
            SiteMatrix.Page.PageWare.AccessJump = res._pageware.GetString("AccessJump");
            SiteMatrix.Page.PageWare.AccessJumpControl = res._pageware.GetString("AccessJumpControl");
            SiteMatrix.Page.PageWare.AccessJumpAddress = res._pageware.GetString("AccessJumpAddress");
            SiteMatrix.Page.PageWare.AccessTip = res._pageware.GetString("AccessTip");
            SiteMatrix.Page.PageWare.AccessTipControl = res._pageware.GetString("AccessTipControl");
            SiteMatrix.Page.PageWare.AccessTipContent = res._pageware.GetString("AccessTipContent");
            SiteMatrix.Page.PageWare.AccessMain_Des = res._pageware.GetString("AccessMain_Des");
            SiteMatrix.Page.PageWare.AccessIP_Des = res._pageware.GetString("AccessIP_Des");
            SiteMatrix.Page.PageWare.AccessIPControl_Des = res._pageware.GetString("AccessIPControl_Des");
            SiteMatrix.Page.PageWare.AccessIPCondition_Des = res._pageware.GetString("AccessIPCondition_Des");
            SiteMatrix.Page.PageWare.AccessIPConditionSide_Des = res._pageware.GetString("AccessIPConditionSide_Des");
            SiteMatrix.Page.PageWare.AccessSession_Des = res._pageware.GetString("AccessSession_Des");
            SiteMatrix.Page.PageWare.AccessSessionControl_Des = res._pageware.GetString("AccessSessionControl_Des");
            SiteMatrix.Page.PageWare.AccessSessionCondition_Des = res._pageware.GetString("AccessSessionCondition_Des");
            SiteMatrix.Page.PageWare.AccessSessionConditionSide_Des = res._pageware.GetString("AccessSessionConditionSide_Des");
            SiteMatrix.Page.PageWare.AccessJump_Des = res._pageware.GetString("AccessJump_Des");
            SiteMatrix.Page.PageWare.AccessJumpControl_Des = res._pageware.GetString("AccessJumpControl_Des");
            SiteMatrix.Page.PageWare.AccessJumpAddress_Des = res._pageware.GetString("AccessJumpAddress_Des");
            SiteMatrix.Page.PageWare.AccessTip_Des = res._pageware.GetString("AccessTip_Des");
            SiteMatrix.Page.PageWare.AccessTipControl_Des = res._pageware.GetString("AccessTipControl_Des");
            SiteMatrix.Page.PageWare.AccessTipContent_Des = res._pageware.GetString("AccessTipContent_Des");
            SiteMatrix.Page.PageWare.AccessNormal = res._pageware.GetString("AccessNormal");
            SiteMatrix.Page.PageWare.AccessNormalNot = res._pageware.GetString("AccessNormalNot");
            SiteMatrix.Page.PageWare.AccessActive = res._pageware.GetString("AccessActive");
            SiteMatrix.Page.PageWare.AccessActiveNot = res._pageware.GetString("AccessActiveNot");
#endregion
        }
        public void UpdateMenusAndToolBars4Site()
        {
            bool CurBool;
            if (globalConst.CurSite.ID == null)
            {
                CurBool = false;
            }
            else
            {
                CurBool = true;
            }
            MenuFileCloseSite.Enabled = CurBool;
            //MenuSiteManage.Enabled = CurBool;
            MenuSiteSave.Enabled = CurBool;
            MenuSiteSynchro.Enabled = CurBool;
            MenuSitePublish.Enabled = CurBool;
            MenuSiteCheck.Enabled = CurBool;
            MenuSiteExportSite.Enabled = CurBool;
            MenuSiteImportSite.Enabled = CurBool;
            //MenuSiteTamplateManage.Enabled = CurBool;
            MenuSiteImportTemplate.Enabled = CurBool;
            MenuSiteExportTemplate.Enabled = CurBool;
            MenuSiteImportPage.Enabled = CurBool;
            MenuSiteRefreshShare.Enabled = CurBool;
            MenuSiteClearFiles.Enabled = CurBool;
            MenuToolSystemSet.Enabled = CurBool;
            MenuToolContentSet.Enabled = CurBool;
            MenuSiteModifyCSS.Enabled = CurBool;
            T3CloseSite.Enabled = CurBool;
            T3SaveSite.Enabled = CurBool;
            T3SycronSite.Enabled = CurBool;
            T3SitePublish.Enabled = CurBool;
            T3SiteExport.Enabled = CurBool;
            T3SiteImport.Enabled = CurBool;
            T3TemplateManage.Enabled = CurBool;
            T3TemplateImport.Enabled = CurBool;
            T3TemplateExport.Enabled = CurBool;
            T3ControlManage.Enabled = CurBool;
            T3PageImport.Enabled = CurBool;
            T3SiteCheck.Enabled = CurBool;
            T3UpdateData.Enabled = CurBool;
        }
        public void UpdateMenusAndToolBars4Page()
        {
            bool CurBool;
            if (form.IsEditorCount0())
            {
                CurBool = false;
                MenuFileSaveFile.Enabled=CurBool;
                    T1SaveFile.Enabled=CurBool;
MenuFileSaveAll.Enabled=CurBool; 
    T1SaveAllFile.Enabled=CurBool;
MenuFileFileSaveAs.Enabled=CurBool;
MenuFileCloseFile.Enabled=CurBool;
MenuEditUndo.Enabled=CurBool;
    T1Undo.Enabled=CurBool;
MenuEditRedo.Enabled=CurBool;
    T1Redo.Enabled=CurBool;
MenuEditCut.Enabled=CurBool;
    T1Cut.Enabled=CurBool;
MenuEditCopy.Enabled=CurBool;
    T1Copy.Enabled=CurBool;
MenuEditPaste.Enabled=CurBool;
    T1Paste.Enabled=CurBool;
MenuEditDelete.Enabled=CurBool;
MenuEditSelectAll.Enabled=CurBool;
MenuEditFind.Enabled=CurBool;
    T1Search.Enabled=CurBool;
MenuEditReplace.Enabled=CurBool;
MenuEditEditTag.Enabled=CurBool;
MenuEditAdvEle2Label.Enabled = CurBool;
CreateGUIDMenuItem.Enabled = CurBool && !globalConst.FormDataMode;
MenuViewDesign.Enabled=CurBool;
    T1Design.Enabled=CurBool;
MenuViewCode.Enabled=CurBool;
    T1Code.Enabled=CurBool;
MenuViewPreview.Enabled=CurBool;
    T1Preview.Enabled=CurBool;
MenuViewWebView.Enabled=CurBool;
MenuViewTag.Enabled=CurBool;
T1Tag.Enabled = CurBool;
MenuViewBorder.Enabled = CurBool; T1Border.Enabled = CurBool;
MenuViewGrid.Enabled = CurBool; T1Grid.Enabled = CurBool;
MenuViewAlive.Enabled = CurBool; T1Live.Enabled = CurBool;
T2Block.Enabled = CurBool;
T2Font.Enabled = CurBool;
t2abc.Enabled = CurBool;
T2Size.Enabled = CurBool;
T2ForeColor.Enabled = CurBool;
T2BackColor.Enabled = CurBool;
T2Bold.Enabled = CurBool;
T2Italic.Enabled = CurBool;
T2Underline.Enabled = CurBool;
T2AlignLeft.Enabled = CurBool;
T2AlignMiddle.Enabled = CurBool;
T2AlignRight.Enabled = CurBool;
T2Indent.Enabled = CurBool;
T2UnIndent.Enabled = CurBool;
T2EditTag.Enabled = CurBool;
T2Layer.Enabled = CurBool;
foreach(ToolStripItem tsmi in MenuFormat.DropDownItems)
{
    tsmi.Enabled = CurBool;
}
foreach (ToolStripItem tsmi in MenuPage.DropDownItems)
{
    tsmi.Enabled = CurBool;
}
            }
            else
            {
                CurBool = true;
                if(form.getEditor().editmode.Equals("edit"))
                {
                    MenuFileSaveFile.Enabled = CurBool;
                    T1SaveFile.Enabled = CurBool;
                    MenuFileSaveAll.Enabled = CurBool;
                    T1SaveAllFile.Enabled = CurBool;
                    MenuFileFileSaveAs.Enabled = CurBool;
                    MenuFileCloseFile.Enabled = CurBool;
                    MenuEditUndo.Enabled = CurBool;
                    T1Undo.Enabled = CurBool;
                    MenuEditRedo.Enabled = CurBool;
                    T1Redo.Enabled = CurBool;
                    MenuEditCut.Enabled = CurBool;
                    T1Cut.Enabled = CurBool;
                    MenuEditCopy.Enabled = CurBool;
                    T1Copy.Enabled = CurBool;
                    MenuEditPaste.Enabled = CurBool;
                    T1Paste.Enabled = CurBool;
                    MenuEditDelete.Enabled = CurBool;
                    MenuEditSelectAll.Enabled = CurBool;
                    MenuEditFind.Enabled = CurBool;
                    T1Search.Enabled = CurBool;
                    MenuEditReplace.Enabled = CurBool;
                    MenuEditEditTag.Enabled = CurBool;
                    MenuEditAdvEle2Label.Enabled = CurBool;
                    CreateGUIDMenuItem.Enabled = CurBool && !globalConst.FormDataMode;
                    MenuViewDesign.Enabled = CurBool;
                    T1Design.Enabled = CurBool;
                    MenuViewCode.Enabled = CurBool;
                    T1Code.Enabled = CurBool;
                    MenuViewPreview.Enabled = CurBool;
                    T1Preview.Enabled = CurBool;
                    MenuViewWebView.Enabled = CurBool;
                    MenuViewTag.Enabled = CurBool;
                    T1Tag.Enabled = CurBool;
                    MenuViewBorder.Enabled = CurBool; T1Border.Enabled = CurBool;
                    MenuViewGrid.Enabled = CurBool; T1Grid.Enabled = CurBool;
                    MenuViewAlive.Enabled = CurBool; T1Live.Enabled = CurBool;
                    T2Block.Enabled = CurBool;
                    T2Font.Enabled = CurBool;
                    t2abc.Enabled = CurBool;
                    T2Size.Enabled = CurBool;
                    T2ForeColor.Enabled = CurBool;
                    T2BackColor.Enabled = CurBool;
                    T2Bold.Enabled = CurBool;
                    T2Italic.Enabled = CurBool;
                    T2Underline.Enabled = CurBool;
                    T2AlignLeft.Enabled = CurBool;
                    T2AlignMiddle.Enabled = CurBool;
                    T2AlignRight.Enabled = CurBool;
                    T2Indent.Enabled = CurBool;
                    T2UnIndent.Enabled = CurBool;
                    T2EditTag.Enabled = CurBool;
                    T2Layer.Enabled = CurBool;
                    foreach (ToolStripItem tsmi in MenuFormat.DropDownItems)
                    {
                        tsmi.Enabled = CurBool;
                    }
                    foreach (ToolStripItem tsmi in MenuPage.DropDownItems)
                    {
                        tsmi.Enabled = CurBool;
                    }
                }
                else if (form.getEditor().editmode.Equals("text"))
                {
                    MenuFileSaveFile.Enabled = CurBool;
                    T1SaveFile.Enabled = CurBool;
                    MenuFileSaveAll.Enabled = CurBool;
                    T1SaveAllFile.Enabled = CurBool;
                    MenuFileFileSaveAs.Enabled = CurBool;
                    MenuFileCloseFile.Enabled = CurBool;
                    MenuEditUndo.Enabled = CurBool;
                    T1Undo.Enabled = CurBool;
                    MenuEditRedo.Enabled = CurBool;
                    T1Redo.Enabled = CurBool;
                    MenuEditCut.Enabled = CurBool;
                    T1Cut.Enabled = CurBool;
                    MenuEditCopy.Enabled = CurBool;
                    T1Copy.Enabled = CurBool;
                    MenuEditPaste.Enabled = CurBool;
                    T1Paste.Enabled = CurBool;
                    MenuEditDelete.Enabled = CurBool;
                    MenuEditSelectAll.Enabled = CurBool;
                    MenuEditFind.Enabled = CurBool;
                    T1Search.Enabled = CurBool;
                    MenuEditReplace.Enabled = CurBool;
                    MenuEditEditTag.Enabled = !CurBool;
                    MenuViewDesign.Enabled = CurBool;
                    T1Design.Enabled = CurBool;
                    MenuViewCode.Enabled = CurBool;
                    T1Code.Enabled = CurBool;
                    MenuViewPreview.Enabled = CurBool;
                    T1Preview.Enabled = CurBool;
                    MenuViewWebView.Enabled = CurBool;
                    MenuViewTag.Enabled = !CurBool;
                    T1Tag.Enabled = !CurBool;
                    MenuViewBorder.Enabled = !CurBool; T1Border.Enabled = !CurBool;
                    MenuViewGrid.Enabled = !CurBool; T1Grid.Enabled = !CurBool;
                    MenuViewAlive.Enabled = !CurBool; T1Live.Enabled = !CurBool;
                    T2Block.Enabled = !CurBool;
                    T2Font.Enabled = !CurBool;
                    t2abc.Enabled = !CurBool;
                    T2Size.Enabled = !CurBool;
                    T2ForeColor.Enabled = !CurBool;
                    T2BackColor.Enabled = !CurBool;
                    T2Bold.Enabled = !CurBool;
                    T2Italic.Enabled = !CurBool;
                    T2Underline.Enabled = !CurBool;
                    T2AlignLeft.Enabled = !CurBool;
                    T2AlignMiddle.Enabled = !CurBool;
                    T2AlignRight.Enabled = !CurBool;
                    T2Indent.Enabled = !CurBool;
                    T2UnIndent.Enabled = !CurBool;
                    T2EditTag.Enabled = !CurBool;
                    T2Layer.Enabled = !CurBool;
                    foreach (ToolStripItem tsmi in MenuFormat.DropDownItems)
                    {
                        tsmi.Enabled = !CurBool;
                    }
                    foreach (ToolStripItem tsmi in MenuPage.DropDownItems)
                    {
                        tsmi.Enabled = !CurBool;
                    }
                }
                else if (form.getEditor().editmode.Equals("view"))
                {
                    MenuFileSaveFile.Enabled = CurBool;
                    T1SaveFile.Enabled = CurBool;
                    MenuFileSaveAll.Enabled = CurBool;
                    T1SaveAllFile.Enabled = CurBool;
                    MenuFileFileSaveAs.Enabled = CurBool;
                    MenuFileCloseFile.Enabled = CurBool;
                    MenuEditUndo.Enabled = !CurBool;
                    T1Undo.Enabled = !CurBool;
                    MenuEditRedo.Enabled = !CurBool;
                    T1Redo.Enabled = !CurBool;
                    MenuEditCut.Enabled = !CurBool;
                    T1Cut.Enabled = !CurBool;
                    MenuEditCopy.Enabled = !CurBool;
                    T1Copy.Enabled = !CurBool;
                    MenuEditPaste.Enabled = !CurBool;
                    T1Paste.Enabled = !CurBool;
                    MenuEditDelete.Enabled = !CurBool;
                    MenuEditSelectAll.Enabled = !CurBool;
                    MenuEditFind.Enabled = !CurBool;
                    T1Search.Enabled = !CurBool;
                    MenuEditReplace.Enabled = !CurBool;
                    MenuEditEditTag.Enabled = !CurBool;
                    MenuViewDesign.Enabled = CurBool;
                    T1Design.Enabled = CurBool;
                    MenuViewCode.Enabled = CurBool;
                    T1Code.Enabled = CurBool;
                    MenuViewPreview.Enabled = CurBool;
                    T1Preview.Enabled = CurBool;
                    MenuViewWebView.Enabled = CurBool;
                    MenuViewTag.Enabled = !CurBool;
                    T1Tag.Enabled = !CurBool;
                    MenuViewBorder.Enabled = !CurBool; T1Border.Enabled = !CurBool;
                    MenuViewGrid.Enabled = !CurBool; T1Grid.Enabled = !CurBool;
                    MenuViewAlive.Enabled = !CurBool; T1Live.Enabled = !CurBool;
                    T2Block.Enabled = !CurBool;
                    T2Font.Enabled = !CurBool;
                    t2abc.Enabled = !CurBool;
                    T2Size.Enabled = !CurBool;
                    T2ForeColor.Enabled = !CurBool;
                    T2BackColor.Enabled = !CurBool;
                    T2Bold.Enabled = !CurBool;
                    T2Italic.Enabled = !CurBool;
                    T2Underline.Enabled = !CurBool;
                    T2AlignLeft.Enabled = !CurBool;
                    T2AlignMiddle.Enabled = !CurBool;
                    T2AlignRight.Enabled = !CurBool;
                    T2Indent.Enabled = !CurBool;
                    T2UnIndent.Enabled = !CurBool;
                    T2EditTag.Enabled = !CurBool;
                    T2Layer.Enabled = !CurBool;
                    foreach (ToolStripItem tsmi in MenuFormat.DropDownItems)
                    {
                        tsmi.Enabled = !CurBool;
                    }
                    foreach (ToolStripItem tsmi in MenuPage.DropDownItems)
                    {
                        tsmi.Enabled = !CurBool;
                    }
                }
            }
        }
        private void Mainform_Load(object sender, EventArgs e)
        {
            classes.PageAsist.RefreshToolBoxPages();
            if(globalConst.FullVersion)
            {
                bool Registered = false;
                RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\F1D2T3P\HostKeys");
                if(rk!=null)
                {
                    object _name=rk.GetValue("name");
                    object _code = rk.GetValue("code");
                    if (_name != null && _code != null)
                    {
                        string name = _name.ToString();
                        string code = _code.ToString();
                        string username = SiteMatrix.consts.globalConst.FullLeft + name + SiteMatrix.consts.globalConst.FullRight;
                        byte[] dataToHash = (new ASCIIEncoding()).GetBytes(username);
                        byte[] hashvalue = ((System.Security.Cryptography.HashAlgorithm)System.Security.Cryptography.CryptoConfig.CreateFromName("MD5")).ComputeHash(dataToHash);
                        string md5str = "";
                        for (int i = 0; i < 16; i++)
                        {
                            md5str += Conversion.Hex(hashvalue[i]).ToUpper();
                        }
                        if (code.Equals(md5str))
                        {
                            Registered = true;
                        }
                    }
                }
                if(!Registered)
                {
                    (new Login()).ShowDialog();
                }
            }
        }
        public void InitToolBoxTads()
        {
            MainToolBox[0].Caption = "HTML Elements";
            MainToolBox[1].Caption = "Standard Controls";
            MainToolBox[2].Caption = "Custom Controls";
            MainToolBox[3].Caption = "My Snippets";
        }
        public void InitToolBoxControls()
        {
            int x = MainToolBox.SelectedTabIndex;
            if (MainToolBox[1].ItemCount > 0)
            {
                MainToolBox[1].SelectedItemIndex = 0;
            }
            if (MainToolBox[2].ItemCount > 0)
            {
                MainToolBox[2].SelectedItemIndex = 0;
            }
            MainToolBox[1].DeleteAllItems();
            MainToolBox[2].DeleteAllItems();
            MainToolBox[1].AddItem("Pointer", 1, 1, false, null);
            MainToolBox[2].AddItem("Pointer", 1, 1, false, null);
            for (int i = 0; i < globalConst.ControlIcon.Length; i++)
            {
                ToolBoxItem tb = new ToolBoxItem();
                tb.Caption = globalConst.ControlIcon[i].Caption;
                tb.SmallImageIndex = i;
                tb.LargeImageIndex = i;
                tb.AllowDrag = true;
                tb.Object = globalConst.ControlIcon[i].ControlID;
                tb.ToolTip = globalConst.ControlIcon[i].Description;
                string controlcorp = globalConst.ControlIcon[i].Company.ToLower();
                if (!globalConst.ControlIcon[i].ControlID.Equals(""))
                {
                    if (controlcorp.Equals("d4soft") || controlcorp.Equals("dotforsite") || controlcorp.Equals("syslive"))
                        MainToolBox[1].AddItem(tb);
                    else
                        MainToolBox[2].AddItem(tb);
                }
            }
            MainToolBox.SelectedTabIndex=x;
            for (int i = MainToolBox[1].ItemCount - 1; i >= 0; i--)
            {
                MainToolBox[1].SelectedItemIndex = i;
            }
            for (int i = MainToolBox[2].ItemCount - 1; i >= 0; i--)
            {
                MainToolBox[2].SelectedItemIndex = i;
            }
        }
        public void InitToolBoxHTMLElements()
        {
            //MainToolBox[0].DeleteAllItems();
            //MainToolBox[0].AddItem("Pointer", 1, 1, false, "Pointer");
            //MainToolBox[0].AddItem("Label", 2, 2, true, "Label");
            //MainToolBox[0].AddItem("HyperLink", 3, 3, true, "HyperLink");
            //MainToolBox[0].AddItem("TextBox", 4, 4, true, "TextBox");
            //MainToolBox[0].AddItem("TextArea", 5, 5, true, "TextArea");
            //MainToolBox[0].AddItem("Password", 6, 6, true, "Password");
            //MainToolBox[0].AddItem("Button", 7, 7, true, "Button");
            //MainToolBox[0].AddItem("Submit Button", 8, 8, true, "Submit Button");
            //MainToolBox[0].AddItem("Reset Button", 9, 9, true, "Reset Button");
            //MainToolBox[0].AddItem("Image Button", 10, 10, true, "Image Button");
            //MainToolBox[0].AddItem("CheckBox", 11, 11, true, "CheckBox");
            //MainToolBox[0].AddItem("Redio Button", 12, 12, true, "Redio Button");
            //MainToolBox[0].AddItem("ComboBox", 13, 13, true, "ComboBox");
            //MainToolBox[0].AddItem("ListBox", 14, 14, true, "ListBox");
            //MainToolBox[0].AddItem("Hidden Field", 15, 15, true, "Hidden Field");
            //MainToolBox[0].AddItem("File Upload", 16, 16, true, "File Upload");
            //MainToolBox[0].AddItem("GroupBox", 17, 17, true, "GroupBox");
            //MainToolBox[0].AddItem("Anchor", 18, 18, true, "Anchor");
            //MainToolBox[0].AddItem("Image", 19, 19, true, "Image");
            //MainToolBox[0].AddItem("Table", 20, 20, true, "Table");
            //MainToolBox[0].AddItem("Span", 21, 21, true, "Span");
            //MainToolBox[0].AddItem("Div", 22, 22, true, "Div");
            //MainToolBox[0].AddItem("Panel", 23, 23, true, "Panel");
            //MainToolBox[0].AddItem("IFrame", 24, 24, true, "IFrame");
            //MainToolBox[0].AddItem("Horizontal Rule", 25, 25, true, "Horizontal Rule");
            //MainToolBox[0].AddItem("Form", 26, 26, true, "Form");
            //for (int i = MainToolBox[0].ItemCount - 1; i >= 0; i--)
            //{
            //    MainToolBox[0].SelectedItemIndex = i;
            //}
        }
        public void InitToolBoxSnippets()
        {
            if (MainToolBox[4].ItemCount > 0)
            {
                MainToolBox[4].SelectedItemIndex = 0;
            }
            MainToolBox[4].DeleteAllItems();
            MainToolBox[4].AddItem("Pointer", 1, 1, false, "Pointer");
            string sql = "select * from snippets order by id";
            DR dr = new DR(globalConst.ConfigConn.OpenRecord(sql));
            while(dr.Read())
            {
                MainToolBox[4].AddItem(dr.getString("caption"), 29, 29, true, dr.getInt32("id").ToString());
            }
            dr.Close();
            for (int i = MainToolBox[4].ItemCount - 1; i >= 0; i--)
            {
                MainToolBox[4].SelectedItemIndex = i;
            }
        }
        private void MainMenu_File_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


       

      

        private void toolStripContainer1_TopToolStripPanel_SizeChanged(object sender, EventArgs e)
        {
            toolStripContainer1.Height = this.toolStripContainer1.TopToolStripPanel.Height-1;
            
        }

       



        

        

       
        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void toolStripContainer2_BottomToolStripPanel_SizeChanged(object sender, EventArgs e)
        {
            this.toolStripContainer2.Height = this.toolStripContainer2.BottomToolStripPanel.Height;
        }

        private void toolbar_toolbox_Click(object sender, EventArgs e)
        {
            MenuViewToolBox.Checked = toolbar_toolbox.Checked;
            this.MainToolBox.Visible = toolbar_toolbox.Checked;
            this.splitter1.Visible = toolbar_toolbox.Checked;
        }

        private void toolbar_property_Click(object sender, EventArgs e)
        {
            MenuViewProperty.Checked = toolbar_property.Checked;
            DoWorkSpaceAndProperty();
        }
        private void toolbar_workpace_Click(object sender, EventArgs e)
        {
            MenuViewWorkSpace.Checked = toolbar_workpace.Checked;
            DoWorkSpaceAndProperty();
        }
        private void DoWorkSpaceAndProperty()
        {
            if (toolbar_workpace.Checked && toolbar_property.Checked)
            {
                WorkSpace.Dock = DockStyle.Top;
                Property.Dock = DockStyle.Fill;
            }
            else
            {
                WorkSpace.Dock = DockStyle.Fill;
            }
            this.splitter3.Visible = toolbar_workpace.Checked && toolbar_property.Checked;
            this.WorkSpace.Visible = toolbar_workpace.Checked;
            this.Property.Visible = toolbar_property.Checked;
            this.panel1.Visible = toolbar_workpace.Checked || toolbar_property.Checked;
            this.splitter2.Visible = toolbar_workpace.Checked || toolbar_property.Checked;
            if (toolbar_workpace.Checked && !toolbar_property.Checked)
            {
                WorkSpace.Dock = DockStyle.Fill;
            }
            if (!toolbar_workpace.Checked && toolbar_property.Checked)
            {
                Property.Dock = DockStyle.Fill;
            }
        }
        public void ChangeTabPageActiveColor(TabControl TC)
        {
            foreach (TabPage TP in TC.TabPages)
            {
                TP.BackColor = Color.Empty;
                TP.ForeColor = Color.Gray;
            }
            TC.SelectedTab.BackColor = Color.FromArgb(255, 244, 242, 232);
            TC.SelectedTab.ForeColor = Color.Black;
        }

        private void WorkSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTabPageActiveColor(WorkSpace);
        }
        private void Property_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTabPageActiveColor(Property);
        }

        private void ResetLayerOut_Click(object sender, EventArgs e)
        {
            ReSetLayOut();
        }
        private void ReSetLayOut()
        {
            this.menuStrip_main.Location = new Point(0,0);
            this.toolStrip_Common.Location = new Point(3, 24);
            this.toolStrip_TextEdit.Visible = true;
            this.toolStrip_TextEdit.Location = new Point(3, 49);
            this.toolStrip_SiteManage.Visible = true;
            this.toolStrip_SiteManage.Location = new Point(664, 49);
            this.MainToolBox.Visible = true;
            this.splitter1.Visible = true;
            this.MainToolBox.Width = 208;
            this.toolbar_property.Checked = true;
            this.toolbar_toolbox.Checked = true;
            this.toolbar_workpace.Checked = true;
            DoWorkSpaceAndProperty();
            this.ShowHiddenTextTool.Checked = true;
            this.ShowHiddenSiteTool.Checked = true;
            this.panel1.Width = 208;
            this.WorkSpace.Height = this.panel1.Height / 2;
        }
        private void LoadDefaultStyle()
        {
            this.WindowState = FormWindowState.Maximized;
            ReSetLayOut();
        }
        public void BaseVisibility()
        {
            if (mdifromConst.tooltextvisible == 1)
            {
                this.toolStrip_TextEdit.Visible = true;
                this.ShowHiddenTextTool.Checked = true;
            }
            else
            {
                this.toolStrip_TextEdit.Visible = false;
                this.ShowHiddenTextTool.Checked = false;
            }
            if (mdifromConst.toolsitevisible == 1)
            {
                this.toolStrip_SiteManage.Visible = true;
                this.ShowHiddenSiteTool.Checked = true;
            }
            else
            {
                this.toolStrip_SiteManage.Visible = false;
                this.ShowHiddenSiteTool.Checked = false;
            }
            if (mdifromConst.toolboxvisible == 1)
            {
                this.MainToolBox.Visible = true;
                this.splitter1.Visible = true;
                this.toolbar_toolbox.Checked = true;
            }
            else
            {
                this.MainToolBox.Visible = false;
                this.splitter1.Visible = false;
                this.toolbar_toolbox.Checked = false;
            }
            if (mdifromConst.workspacevisible == 1)
                this.toolbar_workpace.Checked = true;
            else
                this.toolbar_workpace.Checked = false;
            if (mdifromConst.propertyvisible == 1)
                this.toolbar_property.Checked = true;
            else
                this.toolbar_property.Checked = false;
            DoWorkSpaceAndProperty();
        }
        private void LoadCustomStyle()
        {
            if (mdifromConst.windowstate.Trim().Equals("Maximized"))
                this.WindowState = FormWindowState.Maximized;
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Width = mdifromConst.windowwidth;
                this.Height = mdifromConst.windowheight;
                this.Top = mdifromConst.windowtop;
                this.Left = mdifromConst.windowleft;
            }
            if (mdifromConst.tooltextvisible == 1)
            {
                this.toolStrip_TextEdit.Visible = true;
                this.ShowHiddenTextTool.Checked=true;
            }
            else
            {
                this.toolStrip_TextEdit.Visible = false;
                this.ShowHiddenTextTool.Checked=false;
            }
            if (mdifromConst.toolsitevisible == 1)
            {
                this.toolStrip_SiteManage.Visible = true;
                this.ShowHiddenSiteTool.Checked=true;
            }
            else
            {
                this.toolStrip_SiteManage.Visible = false;
                this.ShowHiddenSiteTool.Checked=false;
            }
            this.MainMenuStrip.Location = new Point(mdifromConst.mainmenux, mdifromConst.mainmenuy);
            this.toolStrip_Common.Location = new Point(mdifromConst.toolcommonx, mdifromConst.toolcommony);
            this.toolStrip_TextEdit.Location = new Point(mdifromConst.tooltextx,mdifromConst.tooltexty);
            this.toolStrip_SiteManage.Location = new Point(mdifromConst.toolsitex, mdifromConst.toolsitey);
            if (mdifromConst.toolboxvisible == 1)
            {
                this.MainToolBox.Visible = true;
                this.splitter1.Visible = true;
                this.toolbar_toolbox.Checked = true;
                MenuViewToolBox.Checked = true;
            }
            else
            {
                this.MainToolBox.Visible = false;
                this.splitter1.Visible = false;
                this.toolbar_toolbox.Checked = false;
                MenuViewToolBox.Checked = false;
            } 
            this.MainToolBox.Width = mdifromConst.toolboxwidth;
            if (mdifromConst.workspacevisible == 1)
            {
                this.toolbar_workpace.Checked = true;
                MenuViewWorkSpace.Checked = true;
            }
            else
            {
                this.toolbar_workpace.Checked = false;
                MenuViewWorkSpace.Checked = false;
            }
            if (mdifromConst.propertyvisible == 1)
            {
                this.toolbar_property.Checked = true;
                MenuViewProperty.Checked = true;
            }
            else
            {
                this.toolbar_property.Checked = false;
                MenuViewProperty.Checked = false;
            }
            DoWorkSpaceAndProperty();
            this.panel1.Width = mdifromConst.panelwidth;
            this.WorkSpace.Height = mdifromConst.workspaceheight;
        }
       
        /// <summary>
        /// save system setting when close window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
			{
                if (globalConst.ConfigConn.State.Equals(ConnectionState.Closed)) return;
				//close site
				SiteClass.Site.close();
                //window states
            setSystemValue("windowstate", this.WindowState.ToString());
            setSystemValue("windowwidth", this.Width.ToString());
            setSystemValue("windowheight", this.Height.ToString());
            setSystemValue("windowtop", this.Top.ToString());
            setSystemValue("windowleft", this.Left.ToString());
            if (this.toolStrip_TextEdit.Visible)
                setSystemValue("tooltextvisible", "1");
            else
                setSystemValue("tooltextvisible", "0");
            if (this.toolStrip_SiteManage.Visible)
                setSystemValue("toolsitevisible", "1");
            else
                setSystemValue("toolsitevisible", "0");
            setSystemValue("mainmenux", this.MainMenuStrip.Location.X.ToString());
            setSystemValue("mainmenuy", this.MainMenuStrip.Location.Y.ToString());
            setSystemValue("toolcommonx", this.toolStrip_Common.Location.X.ToString());
            setSystemValue("toolcommony", this.toolStrip_Common.Location.Y.ToString());
            setSystemValue("tooltextx", this.toolStrip_TextEdit.Location.X.ToString());
            setSystemValue("tooltexty", this.toolStrip_TextEdit.Location.Y.ToString());
            setSystemValue("toolsitex", this.toolStrip_SiteManage.Location.X.ToString());
            setSystemValue("toolsitey", this.toolStrip_SiteManage.Location.Y.ToString());
            if (this.MainToolBox.Visible)
                setSystemValue("toolboxvisible", "1");
            else
                setSystemValue("toolboxvisible", "0");
            setSystemValue("toolboxwidth", this.MainToolBox.Width.ToString());
            if (this.WorkSpace.Visible)
                setSystemValue("workspacevisible", "1");
            else
                setSystemValue("workspacevisible", "0");
            if (this.Property.Visible)
                setSystemValue("propertyvisible", "1");
            else
                setSystemValue("propertyvisible", "0");
            setSystemValue("panelwidth", this.panel1.Width.ToString());
            setSystemValue("workspaceheight", this.WorkSpace.Height.ToString());

            globalConst.ConfigConn.Close();
            this.Owner.Close();

        }
        catch (Exception ex)
        {
            new error(ex);
        }
        }
        private void setSystemValue(string name,string thevalue)
        {
            globalConst.ConfigConn.execSql("update system set thevalue='" + thevalue + "' where name='" + name + "'");
        }

        private void ShowHiddenTextTool_Click(object sender, EventArgs e)
        {
            this.toolStrip_TextEdit.Visible = this.ShowHiddenTextTool.Checked;
        }

        private void ShowHiddenSiteTool_Click(object sender, EventArgs e)
        {
            this.toolStrip_SiteManage.Visible = this.ShowHiddenSiteTool.Checked;
        }

        private void T2Font_SelectedIndexChanged(object sender, EventArgs e)
        {
            t2abc.Font = new Font((string)T2Font.SelectedItem, t2abc.Font.Size);
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if (edr.EditSpace.SelectedIndex == 0)
                    {
                        edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_FontName, false, T2Font.Text);
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void t2abc_Click(object sender, EventArgs e)
        {
            T2Font.Focus();
        }

        private void MenuFileRegisterSite_Click(object sender, EventArgs e)
        {
            SiteAdd _SiteAdd = new SiteAdd();
            _SiteAdd.ShowDialog();
        }

        private void MenuFileAddSite_Click(object sender, EventArgs e)
        {
            AddSite _AddSite = new AddSite();
            _AddSite.ShowDialog();
        }

        private void MenuFileOpenSite_Click(object sender, EventArgs e)
        {
            SiteList _SiteList = new SiteList();
            _SiteList.ShowDialog();
        }

        private void MenuFileCloseSite_Click(object sender, EventArgs e)
        {
            SiteClass.Site.close();
        }

        private void SiteTree_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            TreeNode nd = SiteTree.GetNodeAt(p);
            if (nd != null)
            {
                SiteTree.SelectedNode = nd;
                if (tree.getTypeFromID(tree.getID(nd)).Equals("root"))
                {
                    CurPropTag.Enabled = true;
                    CurPropTag.Items.Clear();
                    CurPropTag.Items.Add(res._globalconst.GetString("tagSite"));
                    CurPropTag.SelectedIndex = 0;
                    ProOthers.Enabled = false;
                    PropGrid.SelectedObject = SiteMatrix.PropertySpace.Site.PropertySite.Bag();
                    return;
                }
                if (tree.getTypeFromID(tree.getID(nd)).Equals("drct"))
                {
                    CurPropTag.Enabled = true;
                    CurPropTag.Items.Clear();
                    CurPropTag.Items.Add(res._globalconst.GetString("tagDir"));
                    CurPropTag.SelectedIndex = 0;
                    ProOthers.Enabled = false;
                    SiteMatrix.PropertySpace.Site.PropertyDirectory.dirid = tree.getID(nd);
                    PropGrid.SelectedObject = SiteMatrix.PropertySpace.Site.PropertyDirectory.Bag();
                    return;
                }
                if (tree.getTypeFromID(tree.getID(nd)).Equals("page"))
                {

                    CurPropTag.Enabled = true;
                    CurPropTag.Items.Clear();
                    CurPropTag.Items.Add(res._globalconst.GetString("tagPage"));
                    CurPropTag.SelectedIndex = 0;
                    ProOthers.Enabled = false;
                    switch (int.Parse(((string[])nd.Tag)[3]))
                    {
                        case 0:
                            SiteMatrix.PropertySpace.Site.PropertyPage.pageid = tree.getID(nd);
                            PropGrid.SelectedObject = SiteMatrix.PropertySpace.Site.PropertyPage.Bag();
                            break;
                        case 1:
                            SiteMatrix.PropertySpace.Site.PropertyPageForm1.pageid = tree.getID(nd);
                            PropGrid.SelectedObject = SiteMatrix.PropertySpace.Site.PropertyPageForm1.Bag();
                            break;
                        case 2:
                            SiteMatrix.PropertySpace.Site.PropertyPageForm2.pageid = tree.getID(nd);
                            PropGrid.SelectedObject = SiteMatrix.PropertySpace.Site.PropertyPageForm2.Bag();
                            break;
                    }
                    return;
                }
                if (tree.getTypeFromID(tree.getID(nd)).Equals("part"))
                {
                    CurPropTag.Enabled = true;
                    CurPropTag.Items.Clear();
                    CurPropTag.Items.Add(res._globalconst.GetString("tagPart"));
                    CurPropTag.SelectedIndex = 0;
                    ProOthers.Enabled = false;
                    SiteMatrix.PropertySpace.Site.PropertyPart.doPartProperty(tree.getID(nd));
                    return;
                }
            }
        }

        private void ControlTree_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            TreeNode nd = ControlTree.GetNodeAt(p);
            if (nd != null)
            {
                ControlTree.SelectedNode = nd;

                if (tree.getTypeFromID(tree.getID(nd)).Equals("part"))
                {
                    CurPropTag.Enabled = true;
                    CurPropTag.Items.Clear();
                    CurPropTag.Items.Add(res._globalconst.GetString("tagPart"));
                    CurPropTag.SelectedIndex = 0;
                    ProOthers.Enabled = false;
                    SiteTree.SelectedNode = null;
                    SiteMatrix.PropertySpace.Site.PropertyPart.doPartProperty(tree.getID(nd));
                    return;
                }
                if (tree.getTypeFromID(tree.getID(nd)).Equals("ctrl"))
                {
                    CurPropTag.Enabled = true;
                    CurPropTag.Items.Clear();
                    CurPropTag.Items.Add(res._globalconst.GetString("tagInstance"));
                    CurPropTag.SelectedIndex = 0;
                    ProOthers.Enabled = false;
                    SiteTree.SelectedNode = null;
                    SiteMatrix.PropertySpace.Site.PropertyControl.doControlProperty(tree.getID(nd));
                    return;
                }
                if (tree.getTypeFromID(tree.getID(nd)).Equals("comp"))
                {
                    CurPropTag.Enabled = true;
                    CurPropTag.Items.Clear();
                    CurPropTag.Items.Add(res._globalconst.GetString("tagControl"));
                    CurPropTag.SelectedIndex = 0;
                    ProOthers.Enabled = false;
                    SiteTree.SelectedNode = null;
                    SiteMatrix.PropertySpace.Site.PropertyControlMother.doControlProperty(tree.getID(nd));
                    return;
                }
                if (tree.getTypeFromID(tree.getID(nd)).Equals("page"))
                {
                    CurPropTag.Enabled = true;
                    CurPropTag.Items.Clear();
                    CurPropTag.Items.Add(res._globalconst.GetString("tagPage"));
                    CurPropTag.SelectedIndex = 0;
                    ProOthers.Enabled = false;
                    TreeNode snd = tree.getSiteNodeByID(tree.getID(nd));
                    if (snd == null)
                    {
                        MsgBox.Warning(tree.getID(nd) + res.MainBox.GetString("m1"));
                        PropGrid.SelectedObject = null;
                    }
                    else
                    {
                        SiteTree.SelectedNode = snd;
                        SiteMatrix.PropertySpace.Site.PropertyPage.pageid = tree.getID(nd);
                        PropGrid.SelectedObject = SiteMatrix.PropertySpace.Site.PropertyPage.Bag();
                    }
                    return;
                }

            }
        }

        public static ImageForm.ImageForm imgform;
        public static void CloseImageForm()
        {
            try
            {
                if (imgform != null)
                {
                    imgform.Visible = false;
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void treeView_ItemDrag(object sender,
            System.Windows.Forms.ItemDragEventArgs e)
        {
            //DoDragDrop(e.Item, DragDropEffects.Move);
            if (!tree.getTypeFromID(tree.getID(ControlTree.SelectedNode)).Equals("part"))
            {
                return;
            }
            TreeNode dragNode = (TreeNode)e.Item;
            imgform.text = "{part}" + dragNode.Text;
            imgform.bitmap = (Bitmap)ControlTree.ImageList.Images[dragNode.ImageIndex];
            imgform.ReInit();
            //form.Load +=new EventHandler(new ImageForm().Form2_Load);
            //imgform.TopLevel = false;
            imgform.Visible = false;
            //imgform.Show();
            //this.Parent.Show().Controls(imgform);
            //globalConst.MdiForm.Controls.Add(imgform);
            //this.Controls.Add(imgform);
            imgform.BringToFront();
            classes.EditorEvent.AddEvents();
            ControlTree.DoDragDrop(e.Item, DragDropEffects.Move);
        }
        private void treeView_DragEnter(object sender,
            System.Windows.Forms.DragEventArgs e)
        {
            classes.EditorEvent.DragE = e;
            e.Effect = DragDropEffects.Move;
        }
        private bool HasToolBoxDraged = false;
        private void toolbox_DragOver(object sender, DragEventArgs e)
        {
            if (!HasToolBoxDraged)
            HasToolBoxDraged = (functions.form.getEditor() == null || functions.form.getEditor().EventAdded == true);
            if (!HasToolBoxDraged)
            {
                ToolBoxItem tbi = MainToolBox.SelectedTab.SelectedItem;
                imgform.text = "{control}" + tbi.Caption;
                imgform.bitmap = (Bitmap)MainToolBox.SmallImageList.Images[tbi.SmallImageIndex];
                imgform.ReInit();
                //form.Load +=new EventHandler(new ImageForm().Form2_Load);
                //imgform.TopLevel = false;
                imgform.Visible = false;
                //imgform.Show();
                //this.Parent.Show().Controls(imgform);
                //globalConst.MdiForm.Controls.Add(imgform);
                //this.Controls.Add(imgform);
                imgform.BringToFront();
                classes.EditorEvent.AddEvents();
                //MainToolBox.DoDragDrop(tbi, DragDropEffects.Move);
                HasToolBoxDraged = true; 
            }
            //Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            //imgform.Visible = true;
            ////imgform.Location = new Point(pt.X + 18,pt.Y + 6);
            //imgform.Location = new Point(e.X - globalConst.MdiForm.Left - 2, e.Y - globalConst.MdiForm.Top - 17);
            //imgform.BringToFront();
            //e.Effect = DragDropEffects.Move;
            //			TreeNode overNode = ((TreeView)sender).GetNodeAt(pt);
            ////			treeView1.SelectedNode = overNode;
            //			if(overNode == null || dragNode.Equals(overNode))
            //			{
            //				e.Effect = DragDropEffects.None;
            //			}
            //			else
            //			{
            //				e.Effect = DragDropEffects.Move;
            //				
            //    
            //			}
        }
        private void treeView3_DragOver(object sender, DragEventArgs e)
        {

            //Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            imgform.Visible = true;
            //imgform.Location = new Point(pt.X + 18,pt.Y + 6);
            imgform.Location = new Point(e.X - globalConst.MdiForm.Left-2, e.Y - globalConst.MdiForm.Top - 17);
            imgform.BringToFront();
            e.Effect = DragDropEffects.Move;
            //			TreeNode overNode = ((TreeView)sender).GetNodeAt(pt);
            ////			treeView1.SelectedNode = overNode;
            //			if(overNode == null || dragNode.Equals(overNode))
            //			{
            //				e.Effect = DragDropEffects.None;
            //			}
            //			else
            //			{
            //				e.Effect = DragDropEffects.Move;
            //				
            //    
            //			}
        }
        private void treeView3_DragLeave(object sender, System.EventArgs e)
        {
            imgform.Visible = false;
            HasToolBoxDraged = false;
        }



        private void ControlTree_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                // Show pointer cursor while dragging
                e.UseDefaultCursors = false;
                ControlTree.Cursor = Cursors.Default;
                MainToolBox.Cursor = Cursors.Default;
            }
            else e.UseDefaultCursors = false;
        }

        private void ControlTree_DragDrop(object sender, DragEventArgs e)
        {
            classes.EditorEvent.MoveEvents();
            HasToolBoxDraged = false;
            CloseImageForm();
        }
        private void SiteTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right))
            {
                Point p = new Point(e.X, e.Y);
                updateMenu4Site(SiteTree.SelectedNode);
                CMSite.Show(SiteTree, p);
            }
        }
        private void ControlTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right))
            {
                Point p = new Point(e.X, e.Y);
                updateMenu4Ctrl(ControlTree.SelectedNode);
                CMControl.Show(ControlTree, p);
            }
        }
        private void updateMenu4Site(TreeNode nd)
        {
            deleteToolStripMenuItem.Visible = true;
            openPageToolStripMenuItem.Visible = true;
            openwithvscode.Visible = true;
            pubpageMenuItem2.Visible = true;
            toolStripSeparator64.Visible = true;
            newDirectoryToolStripMenuItem.Visible = true;
            newPageToolStripMenuItem.Visible = true;
            if (nd == null) return;
            switch (tree.getTypeFromID(nd))
            {
                case "root": 
                    deleteToolStripMenuItem.Visible = false;
                    openPageToolStripMenuItem.Visible = false;
                    openwithvscode.Visible = false;
                    pubpageMenuItem2.Visible = false;
                    toolStripSeparator64.Visible = false;
                    break;
                case "drct":
                    openPageToolStripMenuItem.Visible = false;
                    openwithvscode.Visible = false;
                    pubpageMenuItem2.Visible = false;
                    toolStripSeparator64.Visible = false;
                    break;
                case "page": 
                    break;
                case "part": 
                    newDirectoryToolStripMenuItem.Visible = false;
                    newPageToolStripMenuItem.Visible = false;
                    break;
                default:break;
            }
        }
        private void updateMenu4Ctrl(TreeNode nd)
        {
            addControlToolStripMenuItem.Visible = false;
            deleteControlToolStripMenuItem.Visible = false;
            newControlInstance1ToolStripMenuItem.Visible = false;
            newControlInstance2ToolStripMenuItem.Visible = false;
            cloneControlInstanceToolStripMenuItem.Visible = false;
            deleteControlInstanceToolStripMenuItem.Visible = false;
            newPartInstanceToolStripMenuItem.Visible = false;
            clonePartInstanceToolStripMenuItem.Visible = false;
            deleteToolStripMenuItem1.Visible = false;
            openPageToolStripMenuItem1.Visible = false;
            deletePageToolStripMenuItem.Visible = false;
            toolStripSeparator69.Visible = true;
            if (nd == null)
            {
                addControlToolStripMenuItem.Visible = true;
                return;
            }
            switch (tree.getTypeFromID(nd))
            {
                case "comp":
                    addControlToolStripMenuItem.Visible = true;
                    deleteControlToolStripMenuItem.Visible = true;
                    newControlInstance1ToolStripMenuItem.Visible = true;
                    break;
                case "ctrl":
                    toolStripSeparator69.Visible = false;
                    newControlInstance2ToolStripMenuItem.Visible = true;
                    cloneControlInstanceToolStripMenuItem.Visible = true;
                    deleteControlInstanceToolStripMenuItem.Visible = true;
                    break;
                case "part":
                    toolStripSeparator69.Visible = false;
                    newPartInstanceToolStripMenuItem.Visible = true;
                    clonePartInstanceToolStripMenuItem.Visible = true;
                    deleteToolStripMenuItem1.Visible = true;
                    break;
                case "page":
                    toolStripSeparator69.Visible = false;
                    openPageToolStripMenuItem1.Visible = true;
                    pubpageMenuItem2.Visible = true;
                    deletePageToolStripMenuItem.Visible = true;
                    break;
                default:
                    break;
            }
        }
        private void SiteTree_DoubleClick(object sender, EventArgs e)
        {
            openPage();
        }
        private void ControlTree_DoubleClick(object sender, EventArgs e)
        {
            openPage2();
        }
        private void openPage()
        {
            if (SiteTree.SelectedNode == null) return;
            Cursor cr = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string id = tree.getID(SiteTree.SelectedNode);
                if (tree.getTypeFromID(id).Equals("page"))
                {
                    if (form.doActive(id))
                    {
                        Cursor.Current = cr;
                        return;
                    }
                    string path = globalConst.CurSite.Path + tree.getPath(SiteTree.SelectedNode);
                    //_edit.htm页面不存在则重新生成，暂时这样处理，不考虑其他人为因素造成文件的损坏,fixed
                    System.IO.FileInfo fio = new System.IO.FileInfo(path);
                    if (!file.Exists(path + "_edit.htm"))
                    {
                        SiteClass.Site.constructEditPageFromText(id, path);
                    }
                    else
                    {
                        //如果edit文件被手动修改过，重新生成
                        System.IO.FileInfo fioedit = new System.IO.FileInfo(path + "_edit.htm");
                        //System.IO.FileInfo fio = new System.IO.FileInfo(path);
                        if (!(fioedit.LastWriteTime.Equals(fio.LastWriteTime)))
                        {
                            SiteClass.Site.constructEditPageFromText(id, path);
                        }
                    }
                    Editor ed=form.addEditor(path, ((string[])SiteTree.SelectedNode.Tag)[1], id, ((string[])SiteTree.SelectedNode.Tag)[0], int.Parse(((string[])SiteTree.SelectedNode.Tag)[3]));
                    if (ed != null) ed.UrlLastModTime = fio.LastWriteTime;
                }
                if (tree.getTypeFromID(id).Equals("part"))
                {
                    string partid = tree.getID(SiteTree.SelectedNode);
                    string pageid = tree.getID(SiteTree.SelectedNode.Parent);
                    string sql = "select count(*) from part_in_page where pageid='" + pageid + "' and partid='" + id + "'";
                    if (globalConst.CurSite.SiteConn.GetInt32(sql) == 0)
                    {
                        SiteTree.SelectedNode.Remove();
                        goto LoopOut;
                    }
                    Editor edr = form.getEditor(pageid);
                    if (edr == null)
                    {
                        SiteTree.SelectedNode = SiteTree.SelectedNode.Parent;
                        if (tree.getTypeFromID(tree.getID(SiteTree.SelectedNode)).Equals("page"))
                        {
                            openPage();
                        }
                        else
                        {
                            log.Debug("SiteTree part node.parent not is a page!", "openPage");
                        }
                    }
                    else
                    {
                        IHTMLElementCollection iec = edr.editocx.getElementsByTagName("span");
                        //int i;
                        foreach (IHTMLElement ihe in iec)
                        {
                            //IHTMLElement ihe=(IHTMLElement)iec.item(i,i);
                            //if(ihe.tagName.ToLower().Equals("span"))
                            //{
                            if (ihe.getAttribute("id", 0) != null && ihe.getAttribute("name", 0) != null)
                            {
                                if (ihe.getAttribute("id", 0).ToString().Equals("dotforsitecom") && ihe.getAttribute("name", 0).ToString().Equals("dotforsitecom"))
                                {
                                    if (ihe.getAttribute("idname", 0).ToString().Equals(partid))
                                    {
                                        IHTMLTxtRange it = edr.INITxtRange;
                                        it.moveToElementText(ihe);
                                        goto LoopOut;
                                    }
                                }
                            }
                            //}
                        }

                    }
                }
            LoopOut:
                Cursor.Current = cr;
            }
            catch (Exception ex)
            {
                new error(ex);
                Cursor.Current = cr;
            }
        }
        private void openPage2()
        {
            Cursor cr = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string id = tree.getID(ControlTree.SelectedNode);
                if (tree.getTypeFromID(id).Equals("page"))
                {
                    if (form.doActive(id))
                    {
                        Cursor.Current = cr;
                        return;
                    }
                    string path = "";
                    TreeNode sitend = tree.getSiteNodeByID(id);
                    if (sitend == null)
                    {
                        MsgBox.Error(res.MainBox.GetString("m2"));
                        return;
                    }
                    path = globalConst.CurSite.Path + tree.getPath(sitend);
                    if (path.Equals(""))
                    {
                        MsgBox.Error(res.MainBox.GetString("m3"));
                        return;
                    }
                    //_edit.htm页面不存在则重新生成，暂时这样处理，不考虑其他人为因素造成文件的损坏
                    System.IO.FileInfo fio = new System.IO.FileInfo(path);
                    if (!file.Exists(path + "_edit.htm"))
                    {
                        SiteClass.Site.constructEditPageFromText(id, path);
                    }
                    else
                    {
                        //如果edit文件被手动修改过，重新生成
                        System.IO.FileInfo fioedit = new System.IO.FileInfo(path + "_edit.htm");
                        //System.IO.FileInfo fio = new System.IO.FileInfo(path);
                        if (!(fioedit.LastWriteTime.Equals(fio.LastWriteTime)))
                        {
                            SiteClass.Site.constructEditPageFromText(id, path);
                        }
                    }
                    Editor ed=form.addEditor(path, ((string[])ControlTree.SelectedNode.Tag)[1], id, ((string[])ControlTree.SelectedNode.Tag)[0], int.Parse(((string[])ControlTree.SelectedNode.Tag)[3]));
                    if (ed != null) ed.UrlLastModTime = fio.LastWriteTime;
                }
                Cursor.Current = cr;
            }
            catch (Exception ex)
            {
                new error(ex);
                Cursor.Current = cr;
            }
        }
        private void ProOthers_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (CurEle == null) return;
            mshtml.IHTMLElement PEle = CurEle;
            int pCount = ProOthers.SelectedIndex;
            if (pCount == 0) return;
            int i;
            for (i = 0; i < pCount; i++)
            {
                if (PEle != null) PEle = PEle.parentElement;
            }
            if (PEle != null)
            {
                mshtml.IHTMLTxtRange it = form.getEditor().INITxtRange;
                CurEle = PEle;
                it.moveToElementText(PEle);

                //			//it.scrollIntoView(false);
                HasJustSelectChange = true;
                it.select();
                HasJustSelectChange = false;
                //Page.PageWare.doHtmlAdapter(PEle);
            }
        }

        private void CurPropTag_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (HasJustFromAdapter)
            {
                HasJustFromAdapter = false;
                return;
            }
            if (CurPropTag.SelectedItem.ToString().StartsWith("[C]"))
            {
                string partid = ((imageComboBoxItem)CurPropTag.SelectedItem).ImageIndex;
                Editor edr = form.getEditor();
                IHTMLElementCollection iec = edr.editocx.getElementsByTagName("span");
                //int i;
                foreach (IHTMLElement ihe in iec)
                {
                    //IHTMLElement ihe=(IHTMLElement)iec.item(i,i);
                    //if(ihe.tagName.ToLower().Equals("span"))
                    //{
                    if (ihe.getAttribute("id", 0) != null && ihe.getAttribute("name", 0) != null)
                    {
                        if (ihe.getAttribute("id", 0).ToString().Equals("dotforsitecom") && ihe.getAttribute("name", 0).ToString().Equals("dotforsitecom"))
                        {
                            if (ihe.getAttribute("idname", 0).ToString().Equals(partid))
                            {
                                CurEle = ihe;
                                IHTMLTxtRange it = edr.INITxtRange;
                                it.moveToElementText(ihe);
                                HasJustSelectChange = true;
                                it.select();
                                HasJustSelectChange = false;
                                goto LoopOut;
                            }
                        }
                    }
                    //}
                }
            LoopOut:
                ;
            }
            else if (CurPropTag.Tag != null)
            {
                PageWare.doHtmlAdapter((IHTMLElement)CurPropTag.Tag);
            }
        }

        private void largeItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainToolBox.SelectedTab.View = ViewMode.LargeIcons;
            disableALLItemMenu();
            largeItemToolStripMenuItem.Checked = true;
        }

        private void listItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainToolBox.SelectedTab.View = ViewMode.List;
            disableALLItemMenu();
            listItemToolStripMenuItem.Checked = true;
        }

        private void smallItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainToolBox.SelectedTab.View = ViewMode.SmallIcons;
            disableALLItemMenu();
            smallItemToolStripMenuItem.Checked = true;
        }
        private void disableAllSiteSortMenu()
        {
            FileNameASC.Checked = false;
            sortByFilenameDESCToolStripMenuItem1.Checked = false;
            captionASCToolStripMenuItem.Checked = false;
            captionDESCToolStripMenuItem.Checked = false;
            timeASCToolStripMenuItem.Checked = false;
            timeDESCToolStripMenuItem.Checked = false;
        }
        private void disableAllControlSortMenu()
        {
            nameASCToolStripMenuItem.Checked = false;
            nameDESCToolStripMenuItem.Checked = false;
            captionASCToolStripMenuItem1.Checked = false;
            captionDESCToolStripMenuItem1.Checked = false;
        }
        private void disableALLItemMenu()
        {
            largeItemToolStripMenuItem.Checked = false;
            listItemToolStripMenuItem.Checked = false;
            smallItemToolStripMenuItem.Checked = false;
            removeToolStripMenuItem.Visible = false;
            removeControlToolStripMenuItem.Visible = false;
            addControlToolStripMenuItem1.Visible = false;
            propertyToolStripMenuItem.Visible = false;
            renameToolStripMenuItem.Visible = false;
            AddSneppetsToolStripMenuItem.Visible = false;
            toolStripSeparator78.Visible = false;

            pa_div_change.Visible = false;
            pa_div_del.Visible = false;
            pa_div_dingwei.Visible = false;
            pa_div_fuzhi.Visible = false;
            pa_part_clone_change.Visible = false;
            pa_part_clone_new.Visible = false;
            pa_part_copyGetvalue.Visible = false;
            pa_part_copyID.Visible = false;
            pa_part_copyLabel.Visible = false;
            pa_part_del.Visible = false;
            pa_part_dingwei.Visible = false;
            pa_part_setting.Visible = false;
            pa_refresh.Visible = false;
            pa_div_newdiv.Visible = false;
        }
        private void applyItemMenu()
        {
            disableALLItemMenu();
            //MsgBox.Information(MainToolBox[5].SelectedItemIndex.ToString());
            switch (MainToolBox.SelectedTabIndex)
            {
                case 0: break;
                case 3: break;
                case 4:
                    toolStripSeparator78.Visible = true;
                    renameToolStripMenuItem.Visible = true;
                    AddSneppetsToolStripMenuItem.Visible = true;
                    removeToolStripMenuItem.Visible = true;
                    break;
                case 5:
                    toolStripSeparator78.Visible = true;
                    if (MainToolBox[5].SelectedItem != null)
                    {
                        object[] tag = (object[])MainToolBox[5].SelectedItem.Object;
                        if (((string)tag[0]).StartsWith("div_"))
                        {
                            pa_div_change.Visible = true;
                            pa_div_del.Visible = true;
                            pa_div_dingwei.Visible = true;
                            pa_div_fuzhi.Visible = true;
                        }
                        else if (((string)tag[0]) == "control")
                        {
                            pa_part_clone_change.Visible = true;
                            pa_part_clone_new.Visible = true;
                            pa_part_copyGetvalue.Visible = true;
                            pa_part_copyID.Visible = true;
                            pa_part_copyLabel.Visible = true;
                            pa_part_del.Visible = true;
                            pa_part_dingwei.Visible = true;
                            pa_part_setting.Visible = true;
                        }
                    }
                    else
                    {

                    }
                    pa_refresh.Visible = true;
                    pa_div_newdiv.Visible = true;
                    break;
                default:
                    toolStripSeparator78.Visible = true;
                    removeControlToolStripMenuItem.Visible = true;
                    addControlToolStripMenuItem1.Visible = true;
                    propertyToolStripMenuItem.Visible = true;
                    break;
            }
            switch (MainToolBox.SelectedTab.View)
            {
                case ViewMode.List: listItemToolStripMenuItem.Checked = true; break;
                case ViewMode.SmallIcons: smallItemToolStripMenuItem.Checked = true; break;
                case ViewMode.LargeIcons: largeItemToolStripMenuItem.Checked = true; break;
                default: break;
            }
        }

        private void CMTool_Opening(object sender, CancelEventArgs e)
        {
            applyItemMenu();
        }
        public void refreshSiteTree()
        {
            try
            {
                SiteTree.Nodes.Clear();
                TreeNode rootnode;
                rootnode = new TreeNode(globalConst.CurSite.Domain, 16, 16);
                string[] tag = new string[3];
                tag[0] = globalConst.CurSite.Domain;
                tag[1] = globalConst.CurSite.Domain;
                tag[2] = "root";
                rootnode.Tag = tag;
                SiteTree.Nodes.Add(rootnode);
                SiteClass.Site.constructTree(SiteTree, rootnode, "root", globalConst.siteTreeOrderby, globalConst.siteTreeShowColName);
                SiteTree.CollapseAll();
                rootnode.Expand();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public void refreshControlTree()
        {
            try
            {
                if (!ControlTree.Visible || !ControlTree.Enabled) return;
                ControlTree.Nodes.Clear();
                SiteClass.Site.constructControlTree(ControlTree, globalConst.ctrlTreeOrderby, globalConst.ctrlTreeOrdertype, globalConst.ctrlTreeShowColName);
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void FileNameASC_Click(object sender, EventArgs e)
        {
            disableAllSiteSortMenu();
            globalConst.siteTreeOrderby = " order by name asc";
            refreshSiteTree();
            FileNameASC.Checked = true;
        }

        private void sortByFilenameDESCToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            disableAllSiteSortMenu();
            globalConst.siteTreeOrderby = " order by name desc"; 
            refreshSiteTree();
            sortByFilenameDESCToolStripMenuItem1.Checked = true;
        }

        private void captionASCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableAllSiteSortMenu();
            globalConst.siteTreeOrderby = " order by caption asc"; 
            refreshSiteTree();
            captionASCToolStripMenuItem.Checked = true;
        }

        private void captionDESCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableAllSiteSortMenu();
            globalConst.siteTreeOrderby = " order by caption desc";
            refreshSiteTree();
            captionDESCToolStripMenuItem.Checked = true;
        }

        private void timeASCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableAllSiteSortMenu();
            globalConst.siteTreeOrderby = " order by updatetime asc";
            refreshSiteTree();
            timeASCToolStripMenuItem.Checked = true;
        }

        private void timeDESCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableAllSiteSortMenu();
            globalConst.siteTreeOrderby = " order by updatetime desc";
            refreshSiteTree();
            timeDESCToolStripMenuItem.Checked = true;
        }

        private void nameASCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableAllControlSortMenu();
            globalConst.ctrlTreeOrderby = "name";
            globalConst.ctrlTreeOrdertype = "asc";
            refreshControlTree();
            nameASCToolStripMenuItem.Checked = true;
        }

        private void nameDESCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableAllControlSortMenu();
            globalConst.ctrlTreeOrderby = "name";
            globalConst.ctrlTreeOrdertype = "desc";
            refreshControlTree();
            nameDESCToolStripMenuItem.Checked = true;
        }

        private void captionASCToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            disableAllControlSortMenu();
            globalConst.ctrlTreeOrderby = "caption";
            globalConst.ctrlTreeOrdertype = "asc";
            refreshControlTree();
            captionASCToolStripMenuItem1.Checked = true;
        }

        private void captionDESCToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            disableAllControlSortMenu();
            globalConst.ctrlTreeOrderby = "caption";
            globalConst.ctrlTreeOrdertype = "desc";
            refreshControlTree();
            captionDESCToolStripMenuItem1.Checked = true;
        }

        private void openPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openPage();
        }

        private void newDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDir ad = new AddDir();
            ad.ShowDialog();
        }

        private void newPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPage ap = new AddPage();
            ap.PageType = 0;
            ap.ShowDialog();
        }
        private void SiteTab_Delete()
        {
            Cursor cr = Cursor.Current;
            try
            {
                if (SiteTree.SelectedNode == null)
                {
                    SiteTree.SelectedNode = SiteTree.Nodes[0];
                }
                if (tree.getTypeFromID(SiteTree.SelectedNode).Equals("root"))
                {
                    //delete site code add here
                    return;
                }

                if (tree.getTypeFromID(SiteTree.SelectedNode).Equals("drct"))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    //delete directory
                    if (MsgBox.OKCancel(res.MainBox.GetString("m4")).Equals(DialogResult.OK))
                    {
                        string path = globalConst.CurSite.Path + tree.getPath(SiteTree.SelectedNode);

                        dir.Delete(path, true);
                        if (dir.Exists(path))
                        {
                            MsgBox.Warning(res.MainBox.GetString("m6"));
                            goto DrctEnd;
                        }
                        string sql = "delete from directory where id='" + tree.getID(SiteTree.SelectedNode) + "'";
                        if (globalConst.CurSite.SiteConn.execSql(sql) == 0)
                        {
                            MsgBox.Warning(res.MainBox.GetString("m5"));
                        }
                        
                        //delete other
                        SiteClass.Site.loopTree4DeleteData(SiteTree.SelectedNode.Nodes, 2);
                        SiteTree.SelectedNode.Remove();
                        refreshControlTree();
                    }
                DrctEnd:
                    Cursor.Current = cr;
                    return;
                }

                if (tree.getTypeFromID(SiteTree.SelectedNode).Equals("page"))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    //delete page
                    if (MsgBox.OKCancel(res.MainBox.GetString("m16")).Equals(DialogResult.OK))
                    {
                        string path = globalConst.CurSite.Path + tree.getPath(SiteTree.SelectedNode);
                        file.Delete(path);
                        if (file.Exists(path))
                        {
                            MsgBox.Warning(res.MainBox.GetString("m19"));
                            goto PageEnd;
                        }
                        string sql = "delete from pages where id='" + tree.getID(SiteTree.SelectedNode) + "'";
                        if (globalConst.CurSite.SiteConn.execSql(sql) == 0)
                        {
                            MsgBox.Warning(res.MainBox.GetString("m8"));
                        }
                        sql = "delete from part_in_page where pageid='" + tree.getID(SiteTree.SelectedNode) + "'";
                        globalConst.CurSite.SiteConn.execSql(sql);
                        //close editor form
                        form.closeEditor(tree.getID(SiteTree.SelectedNode));
                        SiteTree.SelectedNode.Remove();
                        refreshControlTree();
                    }
                PageEnd:
                    Cursor.Current = cr;
                    return;
                }
                if (tree.getTypeFromID(SiteTree.SelectedNode).Equals("part"))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    //delete directory
                    if (MsgBox.OKCancel(res.MainBox.GetString("m9")).Equals(DialogResult.OK))
                    {
                        string id = tree.getID(SiteTree.SelectedNode);
                        SiteClass.Site.DeletePartInPage(id);
                        SiteTree.SelectedNode.Remove();
                        PageWare.refreshPagesInCtrlTree(id);
                    }
                    Cursor.Current = cr;
                    return;
                }
            }
            catch (Exception ex)
            {
                new error(ex);
                Cursor.Current = cr;
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiteTab_Delete();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshSiteTree();
        }
        

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiteTree.ExpandAll();
        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiteTree.CollapseAll();
        }

        private void showFileNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiteClass.Site.loopTree4TextChange(SiteTree.Nodes, 0);
            globalConst.siteTreeShowColName = "name";
        }

        private void showCaptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiteClass.Site.loopTree4TextChange(SiteTree.Nodes, 1);
            globalConst.siteTreeShowColName = "caption";
        }

        private void addControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddControl();
        }
        private void AddControl()
        {
            Cursor c = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (controls.controls.AddControl())
                {
                    refreshControlTree();
                    ToolBox.InitToolBoxControls();
                }
                Cursor.Current = c;
            }
            catch (Exception ex)
            {
                new error(ex);
                Cursor.Current = c;
            }
        }
        private void DeleteControl()
        {
            Cursor c = Cursor.Current;
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                if (ControlTree.SelectedNode != null)
                {
                    if (MsgBox.OKCancel(res.MainBox.GetString("m10")) == DialogResult.OK)
                    {
                        string ctrlid = ((string[])ControlTree.SelectedNode.Tag)[2];
                        string controlname = ctrlid.Substring(0, ctrlid.Length - 5);
                        controls.controls.DeleteControl(controlname);
                        refreshControlTree();
                        MainToolBox.SelectedTab.SelectedItemIndex = 0;
                        ToolBox.InitToolBoxControls();
                    }
                }
                Cursor.Current = c;
            }
            catch (Exception ex)
            {
                new error(ex);
                Cursor.Current = c;
            }
        }
        private void deleteControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteControl();
        }
        private void NewControlInstance1()
        {
            Cursor c = Cursor.Current;
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                if (ControlTree.SelectedNode != null)
                {
                    int nodeindex = ControlTree.SelectedNode.Index;
                    string ctrlid = ((string[])ControlTree.SelectedNode.Tag)[2];
                    string controlname = ctrlid.Substring(0, ctrlid.Length - 5);
                    NewControl nc = new NewControl();
                    nc.controlName = controlname;
                    nc.addType = "new";
                    nc.ShowDialog();
                    if (!nc.isCancel)
                    {
                        refreshControlTree();
                        ControlTree.Nodes[nodeindex].Expand();
                    }
                }
                Cursor.Current = c;
            }
            catch (Exception ex)
            {
                new error(ex);
                Cursor.Current = c;
            }
        }
        private void newControlInstance1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewControlInstance1();
        }
        private void NewControlInstance2()
        {
            Cursor c = Cursor.Current;
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                if (ControlTree.SelectedNode != null)
                {
                    int nodeindex = ControlTree.SelectedNode.Parent.Index;
                    string ctrlid = ((string[])ControlTree.SelectedNode.Parent.Tag)[2];
                    string controlname = ctrlid.Substring(0, ctrlid.Length - 5);
                    NewControl nc = new NewControl();
                    nc.controlName = controlname;
                    nc.addType = "new";
                    nc.ShowDialog();
                    if (!nc.isCancel)
                    {
                        refreshControlTree();
                        ControlTree.Nodes[nodeindex].Expand();
                    }
                }
                Cursor.Current = c;
            }
            catch (Exception ex)
            {
                new error(ex);
                Cursor.Current = c;
            }
        }
        private void newControlInstance2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewControlInstance2();
        }
        private void CloneControlInstance()
        {
            Cursor c = Cursor.Current;
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                if (ControlTree.SelectedNode != null)
                {
                    int nodeindex = ControlTree.SelectedNode.Parent.Index;
                    string ctrlid = ((string[])ControlTree.SelectedNode.Tag)[2];
                    NewControl nc = new NewControl();
                    nc.controlName = ctrlid;
                    nc.addType = "clone";
                    nc.textBox1.Text = ((string[])ControlTree.SelectedNode.Tag)[1];
                    nc.ShowDialog();
                    if (!nc.isCancel)
                    {
                        refreshControlTree();
                        ControlTree.Nodes[nodeindex].Expand();
                    }
                }
                Cursor.Current = c;
            }
            catch (Exception ex)
            {
                new error(ex);
                Cursor.Current = c;
            }
        }
        private void cloneControlInstanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloneControlInstance();
        }
        private void DeleteControlInstance()
        {
            try
            {
                if (ControlTree.SelectedNode != null)
                {
                    if (MsgBox.OKCancel(res.MainBox.GetString("m11")) == DialogResult.OK)
                    {
                        int nodeindex = ControlTree.SelectedNode.Parent.Index;
                        string ctrlid = ((string[])ControlTree.SelectedNode.Tag)[2];
                        string sql = "delete from controls where id='" + ctrlid + "'";
                        globalConst.CurSite.SiteConn.execSql(sql);
                        sql = "select id from parts where controlid='" + ctrlid + "'";
                        DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql));
                        string partids = "";
                        while (dr.Read())
                        {
                            partids += dr.getString(0) + "{";
                        }
                        dr.Close();
                        sql = "delete from parts where controlid='" + ctrlid + "'";
                        globalConst.CurSite.SiteConn.execSql(sql);
                        string[] partidsa = partids.Split('{');
                        foreach (string id in partidsa)
                        {
                            if (id != null && !id.Equals(""))
                            {
                                //
                                SiteClass.Site.DeletePartInPage(id);
                                //
                                sql = "delete from part_in_page where partid='" + id + "'";
                                globalConst.CurSite.SiteConn.execSql(sql);
                            }
                        }
                        refreshControlTree();
                        ControlTree.Nodes[nodeindex].Expand();
                        refreshSiteTree();
                    }
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void deleteControlInstanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteControlInstance();
        }
        private void NewPartInstance()
        {
            Cursor c = Cursor.Current;
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                if (ControlTree.SelectedNode != null)
                {
                    //int nodeindex=ctrlTree.SelectedNode.Parent.Index;
                    //int nodepindex=ctrlTree.SelectedNode.Parent.Parent.Index;
                    string partid = ((string[])ControlTree.SelectedNode.Tag)[2];
                    string[] AddPartSA = PageWare.AddPart(partid);
                    if (AddPartSA != null)
                    {
                        TreeNode nd;
                        if (globalConst.ctrlTreeShowColName.Equals("name"))
                            nd = new TreeNode(AddPartSA[0], 20, 20);
                        else
                            nd = new TreeNode(AddPartSA[1], 20, 20);
                        nd.Tag = AddPartSA;
                        ControlTree.SelectedNode.Parent.Nodes.Add(nd);
                        //						refreshControlTree();
                        //						//ctrlTree.Nodes[nodepindex].Expand();
                        //						ctrlTree.Nodes[nodeindex].Expand();
                    }
                    else
                    {
                        MsgBox.Error(res.MainBox.GetString("m12"));
                    }
                }
                Cursor.Current = c;
            }
            catch (Exception ex)
            {
                new error(ex);
                Cursor.Current = c;
            }
        }
        private void newPartInstanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewPartInstance();
        }
        private void ClonePartInstance()
        {
            Cursor c = Cursor.Current;
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                if (ControlTree.SelectedNode != null)
                {
                    //int nodeindex=ctrlTree.SelectedNode.Parent.Index;
                    //int nodepindex=ctrlTree.SelectedNode.Parent.Parent.Index;
                    string partid = ((string[])ControlTree.SelectedNode.Tag)[2];
                    string tag0 = ((string[])ControlTree.SelectedNode.Tag)[0];
                    string tag1 = ((string[])ControlTree.SelectedNode.Tag)[1];
                    string clonepart = PageWare.ClonePart(partid);
                    if (clonepart != null)
                    {
                        TreeNode nd;
                        string[] tag = new string[3];
                        tag[0] = tag0;
                        tag[1] = tag1;
                        tag[2] = clonepart;
                        if (globalConst.ctrlTreeShowColName.Equals("name"))
                            nd = new TreeNode(tag[0], 20, 20);
                        else
                            nd = new TreeNode(tag[1], 20, 20);
                        nd.Tag = tag;
                        ControlTree.SelectedNode.Parent.Nodes.Add(nd);
                        //						refreshControlTree();
                        //						//ctrlTree.Nodes[nodepindex].Expand();
                        //						ctrlTree.Nodes[nodeindex].Expand();
                    }
                    else
                    {
                        MsgBox.Error(res.MainBox.GetString("m13"));
                    }
                }
                Cursor.Current = c;
            }
            catch (Exception ex)
            {
                new error(ex);
                Cursor.Current = c;
            }
        }
        private void clonePartInstanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClonePartInstance();
        }
        private void DeletePart()
        {
            try
            {
                if (ControlTree.SelectedNode != null)
                {
                    string partid = ((string[])ControlTree.SelectedNode.Tag)[2];
                    string sql = "select name,controlid from parts where id='" + partid + "'";
                    string partname = "";
                    string controlid = "";
                    DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql));
                    if (dr.Read())
                    {
                        partname = dr.getString(0);
                        controlid = dr.getString(1);
                    }
                    else
                    {
                        dr.Close();
                        return;
                    }
                    dr.Close();
                    sql = "select count(id) from parts where name='" + partname + "' and controlid='" + controlid + "'";
                    if (globalConst.CurSite.SiteConn.GetInt32(sql) < 2)
                    {
                        MsgBox.Information(res.MainBox.GetString("m14"));
                        return;
                    }
                    if (MsgBox.OKCancel(res.MainBox.GetString("m15")) == DialogResult.OK)
                    {
                        sql = "delete from parts where id='" + partid + "'";
                        globalConst.CurSite.SiteConn.execSql(sql);
                        //
                        SiteClass.Site.DeletePartInPage(partid);
                        //
                        sql = "delete from part_in_page where partid='" + partid + "'";
                        globalConst.CurSite.SiteConn.execSql(sql);
                        ControlTree.SelectedNode.Remove();
                        ControlTree.SelectedNode = null;
                    }
                    refreshSiteTree();
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DeletePart();
        }

        private void openPageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openPage2();
        }
        private void ControlTabDeletePage()
        {
            try
            {
                if (ControlTree.SelectedNode != null)
                {
                    //delete page
                    if (MsgBox.OKCancel(res.MainBox.GetString("m16")).Equals(DialogResult.OK))
                    {
                        string path = "";
                        string id = ((string[])ControlTree.SelectedNode.Tag)[2];
                        TreeNode sitend = tree.getSiteNodeByID(id);
                        if (sitend == null)
                        {
                            MsgBox.Error(res.MainBox.GetString("m17"));
                            return;
                        }
                        path = globalConst.CurSite.Path + tree.getPath(sitend);
                        if (path.Equals(""))
                        {
                            MsgBox.Error(res.MainBox.GetString("m18"));
                            return;
                        }
                        file.Delete(path);
                        if (file.Exists(path))
                        {
                            MsgBox.Warning(res.MainBox.GetString("m19"));
                        }
                        string sql = "delete from pages where id='" + id + "'";
                        if (globalConst.CurSite.SiteConn.execSql(sql) == 0)
                        {
                            MsgBox.Warning(res.MainBox.GetString("m20"));
                        }
                        sql = "delete from part_in_page where pageid='" + id + "'";
                        globalConst.CurSite.SiteConn.execSql(sql);
                        sql = "delete from formrules where id='" + id + "'";
                        globalConst.CurSite.SiteConn.execSql(sql);
                        //close editor form
                        form.closeEditor(id);
                        //ctrlTree.SelectedNode.Remove();
                        refreshControlTree();
                        refreshSiteTree();
                    }
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void deletePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlTabDeletePage();
        }

        private void showNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiteClass.Site.loopTree4TextChange(ControlTree.Nodes, 0);
            globalConst.ctrlTreeShowColName = "name";
        }

        private void showCaptionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SiteClass.Site.loopTree4TextChange(ControlTree.Nodes, 1);
            globalConst.ctrlTreeShowColName = "caption";
        }

        private void expandAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ControlTree.ExpandAll();
        }

        private void collapseAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ControlTree.CollapseAll();
        }

        private void refreshToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            refreshControlTree();
        }

        private void MenuSitePublish_Click(object sender, EventArgs e)
        {
            SitePublish sp = new SitePublish();
            sp.ShowDialog();
        }

        private void MenuSiteSynchro_Click(object sender, EventArgs e)
        {
            if (MsgBox.YesNo("同步站点将覆盖本地站点，可能造成本地站点数据丢失，确定要继续吗？").Equals(DialogResult.Yes))
            {
                SiteUpdate su = new SiteUpdate();
                su.siteid = globalConst.CurSite.ID;
                su.ShowDialog();
            }
        }

        private void MenuSiteManage_Click(object sender, EventArgs e)
        {
            SiteList slt = new SiteList();
            slt.ShowDialog();
        }

        private void addControlToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddControl();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainToolBox.SelectedTab.SelectedItem.Object != null)
            {
                string cid = MainToolBox.SelectedTab.SelectedItem.Object.ToString();
                if (cid.Equals("Pointer")) return;
                string sql = "delete from snippets where id=" + cid;
                globalConst.ConfigConn.execSql(sql);
                this.InitToolBoxSnippets();
            }
        }

        private void removeControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor c = Cursor.Current;
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                if (MainToolBox.SelectedTab.SelectedItem.Object != null)
                {
                    if (MsgBox.OKCancel(res.MainBox.GetString("m21")) == DialogResult.OK)
                    {
                        string controlname = MainToolBox.SelectedTab.SelectedItem.Object.ToString();
                        controls.controls.DeleteControl(controlname);
                        refreshControlTree();
                        MainToolBox.SelectedTab.SelectedItemIndex = 0;
                        ToolBox.InitToolBoxControls();
                    }
                }
                Cursor.Current = c;
            }
            catch (Exception ex)
            {
                new error(ex);
                Cursor.Current = c;
            }
        }

        private void propertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainToolBox.SelectedTab.SelectedItem.Object != null)
            {
                CurPropTag.Enabled = true;
                CurPropTag.Items.Clear();
                CurPropTag.Items.Add(res._globalconst.GetString("tagControl"));
                CurPropTag.SelectedIndex = 0;
                ProOthers.Enabled = false;
                SiteTree.SelectedNode = null;
                SiteMatrix.PropertySpace.Site.PropertyControlMother.doControlProperty(MainToolBox.SelectedTab.SelectedItem.Object.ToString() + "_comp");
            }
        }

        private void CMPage_Opening(object sender, CancelEventArgs e)
        {

        }

        private void T1Cut_Click(object sender, EventArgs e)
        {
            DoCut();
        }
        private void DoCut()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if (edr.EditSpace.SelectedIndex == 0)
                    {
                        edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Cut, false, null);
                    }
                    else if (edr.EditSpace.SelectedIndex == 1)
                    {
                        edr.textocx.doCopy();
                        edr.textocx.SelectedText = "";
                    }
                    edr = null;
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void T1Copy_Click(object sender, EventArgs e)
        {
            DoCopy();
        }
        private void DoCopy()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if (edr.EditSpace.SelectedIndex == 0)
                    {
                        edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Copy, false, null);
                    }
                    else if (edr.EditSpace.SelectedIndex == 1)
                    {
                        edr.textocx.doCopy();
                    }
                    edr = null;
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void T1Paste_Click(object sender, EventArgs e)
        {
            DoPaste();
        }
        private void DoPaste()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if (edr.EditSpace.SelectedIndex == 0)
                    {
                        edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Paste, false, null);
                    }
                    else if (edr.EditSpace.SelectedIndex == 1)
                    {
                        edr.textocx.doPaste();
                    }
                    edr = null;
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void T1Undo_Click(object sender, EventArgs e)
        {
            DoUndo();
        }
        private void DoUndo()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if (edr.EditSpace.SelectedIndex == 0)
                    {
                        edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Undo, false, null);
                    }
                    else if (edr.EditSpace.SelectedIndex == 1)
                    {
                        if(edr.textocx.CanUndo)
                        edr.textocx.Undo();
                    }
                    edr = null;
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void T1Redo_Click(object sender, EventArgs e)
        {
            DoRedo();
        }
        private void DoRedo()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if (edr.EditSpace.SelectedIndex == 0)
                    {
                        edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Redo, false, null);
                    }
                    else if (edr.EditSpace.SelectedIndex == 1)
                    {
                        if (edr.textocx.CanRedo)
                        edr.textocx.Redo();
                    }
                    edr = null;
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void T1Design_Click(object sender, EventArgs e)
        {
            T1Design.Checked = true;
            T1Code.Checked = false;
            T1Preview.Checked = false;
            MenuViewDesign.Checked = T1Design.Checked;
            MenuViewCode.Checked=T1Code.Checked;
            MenuViewPreview.Checked=T1Preview.Checked;
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.EditSpace.SelectedIndex = 0;
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void T1Code_Click(object sender, EventArgs e)
        {
            T1Design.Checked = false;
            T1Code.Checked = true;
            T1Preview.Checked = false;
            MenuViewDesign.Checked = T1Design.Checked;
            MenuViewCode.Checked = T1Code.Checked;
            MenuViewPreview.Checked = T1Preview.Checked;
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.EditSpace.SelectedIndex = 1;
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void T1Preview_Click(object sender, EventArgs e)
        {
            T1Design.Checked = false;
            T1Code.Checked = false;
            T1Preview.Checked = true;
            MenuViewDesign.Checked = T1Design.Checked;
            MenuViewCode.Checked = T1Code.Checked;
            MenuViewPreview.Checked = T1Preview.Checked;
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.EditSpace.SelectedIndex = 2;
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void T1Tag_Click(object sender, EventArgs e)
        {
            MenuViewTag.Checked = T1Tag.Checked;
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if (edr.EditSpace.SelectedIndex == 0)
                    {
                        edr.editocx.showGlyphs = T1Tag.Checked;
                        edr.editocx.RefreshEditState4Glyphs();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void T1Border_Click(object sender, EventArgs e)
        {
            MenuViewBorder.Checked=T1Border.Checked;
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if (edr.EditSpace.SelectedIndex == 0)
                    {
                        edr.editocx.ShowTableZeroBorders(T1Border.Checked);
                    }
                } 
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void T1Grid_Click(object sender, EventArgs e)
        {
            MenuViewGrid.Checked=T1Grid.Checked;
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if (edr.EditSpace.SelectedIndex == 0)
                    {
                        edr.editocx.showBodyNetCells = T1Grid.Checked;
                        edr.editocx.RefreshEditState4NetCells();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void T1Live_Click(object sender, EventArgs e)
        {
            MenuViewAlive.Checked=T1Live.Checked;
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if (edr.EditSpace.SelectedIndex == 0)
                    {
                        edr.editocx.showLiveResizes = T1Live.Checked;
                        edr.editocx.RefreshEditState4LiveResize();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void T2Block_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if (edr.EditSpace.SelectedIndex == 0)
                    {
                        string tag = "<p>";
                        switch (T2Block.SelectedIndex)
                        {
                            case 0: tag = "<p>"; break;
                            case 1: tag = "<Address>"; break;
                            case 2: tag = "<PRE>"; break;
                            case 3: tag = "<p>"; break;
                            case 4: tag = "<H1>"; break;
                            case 5: tag = "<H2>"; break;
                            case 6: tag = "<H3>"; break;
                            case 7: tag = "<H4>"; break;
                            case 8: tag = "<H5>"; break;
                            case 9: tag = "<H6>"; break;
                        }
                        edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_FormatBlock, false, tag);
                        if(T2Block.SelectedIndex==0)
                            edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_RemoveParaFormat, false, null);
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void T2Size_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if (edr.EditSpace.SelectedIndex == 0)
                    {
                        edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_FontSize, false, T2Size.Text);
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void T2ForeColor_Click(object sender, EventArgs e)
        {
            DoForeColor();
        }
        private void DoForeColor()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    ColorDialog MyDialog = new ColorDialog();
                    try
                    {
                        MyDialog.Color = ColorTranslator.FromOle(int.Parse(edr.editocx.queryCMDValue(D4ENUM.D4HTMLCMDSTR.HTMLCMD_ForeColor).ToString()));
                    }
                    catch { }
                    MyDialog.FullOpen = true;
                    MyDialog.ShowDialog();
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_ForeColor, false, ColorTranslator.ToOle(MyDialog.Color));
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void T2BackColor_Click(object sender, EventArgs e)
        {
            DoBackColor();
        }
        private void DoBackColor()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    ColorDialog MyDialog = new ColorDialog();
                    try
                    {
                        MyDialog.Color = ColorTranslator.FromOle(int.Parse(edr.editocx.queryCMDValue(D4ENUM.D4HTMLCMDSTR.HTMLCMD_BackColor).ToString()));
                    }
                    catch { }
                    MyDialog.FullOpen = true;
                    MyDialog.ShowDialog();
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_BackColor, false, ColorTranslator.ToOle(MyDialog.Color));
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void T2Bold_Click(object sender, EventArgs e)
        {
            MenuFormatBold.Checked=  T2Bold.Checked;
            DoBold();
        }
        private void DoBold()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Bold, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void T2Italic_Click(object sender, EventArgs e)
        {
            MenuFormatItalic.Checked= T2Italic.Checked;
            DoItalic();
        }
        private void DoItalic()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Italic, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void T2Underline_Click(object sender, EventArgs e)
        {
            MenuFormatUnderline.Checked= T2Underline.Checked;
            DoUnderline();
        }
        private void DoUnderline()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Underline, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void MenuFormatLinethrough_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_StrikeThrough, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void T2AlignLeft_Click(object sender, EventArgs e)
        {
            MenuFormatAlignRight.Checked = false;
            T2AlignRight.Checked = false;
            MenuFormatAlignCenter.Checked = false;
            T2AlignMiddle.Checked = false;
            MenuFormatAlignLeft.Checked= T2AlignLeft.Checked;
            DoAlignLeft();
        }
        private void DoAlignLeft()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_JustifyLeft, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void T2AlignMiddle_Click(object sender, EventArgs e)
        {
            MenuFormatAlignRight.Checked = false;
            T2AlignRight.Checked = false;
            MenuFormatAlignLeft.Checked = false;
            T2AlignLeft.Checked = false;
            MenuFormatAlignCenter.Checked= T2AlignMiddle.Checked;
            DoAlignMiddle();
        }
        private void DoAlignMiddle()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_JustifyCenter, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void MenuFormatBlockFormatNormal_Click(object sender, EventArgs e)
        {
            T2Block.SelectedIndex = 0;
        }

        private void T2AlignRight_Click(object sender, EventArgs e)
        {
            MenuFormatAlignLeft.Checked = false;
            T2AlignLeft.Checked = false;
            MenuFormatAlignCenter.Checked = false;
            T2AlignMiddle.Checked = false;
            MenuFormatAlignRight.Checked= T2AlignRight.Checked;
            DoAlignRight();
        }
        private void DoAlignRight()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_JustifyRight, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void T2Indent_Click(object sender, EventArgs e)
        {
            DoIndent();
        }
        private void DoIndent()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Indent, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void T2UnIndent_Click(object sender, EventArgs e)
        {
            DoUnindent();
        }
        private void DoUnindent()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Outdent, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void MenuFormatRTL_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_DirRTL, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuFormatLTR_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_DirLTR, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuFormatAlignDefault_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_JustifyNone, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuFormatBold_Click(object sender, EventArgs e)
        {
              T2Bold.Checked=MenuFormatBold.Checked;
            DoBold();
        }

        private void MenuFormatItalic_Click(object sender, EventArgs e)
        {
            T2Italic.Checked=MenuFormatItalic.Checked;
            DoItalic();
        }

        private void MenuFormatUnderline_Click(object sender, EventArgs e)
        {
            T2Underline.Checked=MenuFormatUnderline.Checked;
            DoUnderline();
        }

        private void MenuFormatSuperscript_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Superscript, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuFormatSubscript_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Subscript, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuFormatForeColor_Click(object sender, EventArgs e)
        {
            DoForeColor();
        }

        private void MenuFormatBackColor_Click(object sender, EventArgs e)
        {
            DoBackColor();
        }

        private void MenuFormatAlignLeft_Click(object sender, EventArgs e)
        {
            MenuFormatAlignRight.Checked = false;
            T2AlignRight.Checked = false;
            MenuFormatAlignCenter.Checked = false;
            T2AlignMiddle.Checked = false;
            T2AlignLeft.Checked=MenuFormatAlignLeft.Checked;
            DoAlignLeft();
        }

        private void MenuFormatAlignRight_Click(object sender, EventArgs e)
        {
            MenuFormatAlignLeft.Checked = false;
            T2AlignLeft.Checked = false;
            MenuFormatAlignCenter.Checked = false;
            T2AlignMiddle.Checked = false;
            T2AlignRight.Checked=MenuFormatAlignRight.Checked;
            DoAlignRight();
        }

        private void MenuFormatAlignCenter_Click(object sender, EventArgs e)
        {
            MenuFormatAlignRight.Checked = false;
            T2AlignRight.Checked = false;
            MenuFormatAlignLeft.Checked = false;
            T2AlignLeft.Checked = false;
            T2AlignMiddle.Checked=MenuFormatAlignCenter.Checked;
            DoAlignMiddle();
        }

        private void MenuFormatIndent_Click(object sender, EventArgs e)
        {
            DoIndent();
        }

        private void MenuFormatUnindent_Click(object sender, EventArgs e)
        {
            DoUnindent();
        }

        private void MenuFormatBlockAddress_Click(object sender, EventArgs e)
        {
            T2Block.SelectedIndex = 1;
        }

        private void MenuFormatBlockFormatFormatted_Click(object sender, EventArgs e)
        {
            T2Block.SelectedIndex = 2;
        }

        private void MenuFormatBlockFormatParagraph_Click(object sender, EventArgs e)
        {
            T2Block.SelectedIndex = 3;
        }

        private void MenuFormatBlockFormatHeading1_Click(object sender, EventArgs e)
        {
            T2Block.SelectedIndex = 4;
        }

        private void MenuFormatBlockFormatHeading2_Click(object sender, EventArgs e)
        {
            T2Block.SelectedIndex = 5;
        }

        private void MenuFormatBlockFormatHeading3_Click(object sender, EventArgs e)
        {
            T2Block.SelectedIndex = 6;
        }

        private void MenuFormatBlockFormatHeading4_Click(object sender, EventArgs e)
        {
            T2Block.SelectedIndex = 7;
        }

        private void MenuFormatBlockFormatHeading5_Click(object sender, EventArgs e)
        {
            T2Block.SelectedIndex = 8;
        }

        private void MenuFormatBlockFormatHeading6_Click(object sender, EventArgs e)
        {
            T2Block.SelectedIndex = 9;
        }

        private void T2Layer_Click(object sender, EventArgs e)
        {
            if (T2Layer.Checked) DoLayerSet();
            else
                DoLayerCancel();
        }
        private void DoLayerSet()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if ((edr.editocx.queryCMDEnabled(D4ENUM.D4HTMLCMDSTR.HTMLCMD_AbsolutePosition)).ToString() == "True")
                        if ((edr.editocx.queryCMDValue(D4ENUM.D4HTMLCMDSTR.HTMLCMD_AbsolutePosition)).ToString() == "False")
                            edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_AbsolutePosition, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void DoLayerCancel()
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if ((edr.editocx.queryCMDEnabled(D4ENUM.D4HTMLCMDSTR.HTMLCMD_AbsolutePosition)).ToString() == "True")
                        if ((edr.editocx.queryCMDValue(D4ENUM.D4HTMLCMDSTR.HTMLCMD_AbsolutePosition)).ToString() == "True")
                            edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_AbsolutePosition, false, null);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuFormatLayerSet_Click(object sender, EventArgs e)
        {
            DoLayerSet();
        }

        private void MenuFormatLayerCancel_Click(object sender, EventArgs e)
        {
            DoLayerCancel();
        }

        private void MenuFormatLayerFore_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.setFront();
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuFormatLayerBack_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    edr.editocx.setBack();
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuFormatStyle_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    IHTMLElement ie=edr.editocx.getCurElement();
                    if (ie != null)
                    {
                        StyleBuilder sb = new StyleBuilder();
                        sb.ele = ie;
                        string originalCSS=sb.ele.style.cssText;
                        sb.ShowDialog();
                        if (!sb.isCanceled)
                        {
                            ie.style.cssText = originalCSS;
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

        private void MenuEditUndo_Click(object sender, EventArgs e)
        {
            DoUndo();
        }

        private void MenuEditDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();
                
                if (edr != null)
                {
                    switch (edr.editmode)
                    {
                        case "edit": edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Delete, false, null); break;
                        case "text": edr.textocx.SelectedText = ""; break;
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuEditRedo_Click(object sender, EventArgs e)
        {
            DoRedo();
        }

        private void MenuEditCut_Click(object sender, EventArgs e)
        {
            DoCut();
        }

        private void MenuEditCopy_Click(object sender, EventArgs e)
        {
            DoCopy();
        }

        private void MenuEditPaste_Click(object sender, EventArgs e)
        {
            DoPaste();
        }

        private void MenuSiteExportSite_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                SiteExport se = new SiteExport();
                se.AsTemplate = false;
                se.ShowDialog();
            }
        }

        private void MenuSiteImportSite_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                SiteImport se = new SiteImport();
                se.AsTemplate = false;
                se.ShowDialog();
            }
        }

        private void MenuSiteImportTemplate_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                SiteImport se = new SiteImport();
                se.AsTemplate = true;
                se.ShowDialog();
            }
        }

        private void MenuSiteExportTemplate_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                SiteExport se = new SiteExport();
                se.AsTemplate = true;
                se.ShowDialog();
            }
        }

        private void MenuFileNewFreeFile_Click(object sender, EventArgs e)
        {
            AddNewFreeFile();
        }
        private void AddNewFreeFile()
        { 
            string fid=rdm.getID()+"_free";
            file.Copy(globalConst.emptyFile, globalConst.FreeFilesPath + "/" + fid,true);
            form.addFreeFileEditor(globalConst.FreeFilesPath + "/" + fid, "Static Page", fid, fid, "Static Page", false);
        }

        private void T1NewFreeFile_Click(object sender, EventArgs e)
        {
            AddNewFreeFile();
        }
        private void OpenFreeFile()
        {
            string fid = rdm.getID() + "_free";
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "HTML Files(*.htm;*.html)|*.htm;*.html|Web Files(*.asp;*.aspx;*.jsp;*.php;*.php3;)|*.asp;*.aspx;*.jsp;*.php;*.php3;|All Files(*.*)|*.*";
            sfd.ShowDialog();
            if (!sfd.FileName.Equals(""))
            {
                form.addFreeFileEditor(sfd.FileName, "Free Page", fid, fid, "Free Page", true);
            }
        }
        private void T1OpenFreeFile_Click(object sender, EventArgs e)
        {
            OpenFreeFile();
        }
        private void SaveFile()
        {
            Editor ed = form.getEditor();
            if (ed != null) ed.savePage();
        }
        private void SaveAllFiles(bool saveFreeFile=true)
        {
            Form[] fms = globalConst.MdiForm.MdiChildren;
            foreach (Form fm in fms)
            {
                if (fm.Name.Equals("Editor"))
                {
                    if(saveFreeFile || !((Editor)fm).isFreeFile)
                    {
                        ((Editor)fm).savePage();
                    }
                }
            } 
        }
        private void T1SaveFile_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void T1SaveAllFile_Click(object sender, EventArgs e)
        {
            SaveAllFiles();
        }

        private void T1OpenWebPage_Click(object sender, EventArgs e)
        {
            OpenWebPage owp = new OpenWebPage();
            owp.ShowDialog();
        }

        private void MenuFileOpenWebPage_Click(object sender, EventArgs e)
        {
            OpenWebPage owp = new OpenWebPage();
            owp.ShowDialog();
        }

        private void MenuSiteImportPage_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                ImportPage ip = new ImportPage();
                ip.ShowDialog();
            }
        }
        private TreeNode CurNode1 = null;
        private TreeNode CurNode2 = null;
        private void SiteTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (CurNode1 != null)
            {
                CurNode1.BackColor = Color.White;
                CurNode1.ForeColor = Color.Black;
            }
            SiteTree.SelectedNode.BackColor = System.Drawing.SystemColors.Highlight;
            SiteTree.SelectedNode.ForeColor = Color.White;
            CurNode1 = SiteTree.SelectedNode;
        }

        private void ControlTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (CurNode2 != null)
            {
                CurNode2.BackColor = Color.White;
                CurNode2.ForeColor = Color.Black;
            }
            ControlTree.SelectedNode.BackColor = System.Drawing.SystemColors.Highlight;
            ControlTree.SelectedNode.ForeColor = Color.White;
            CurNode2 = ControlTree.SelectedNode;
            updateControlTabTool();
        }

        private void MenuSiteRefreshShare_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID == null) return;
            LoadStat ls = new LoadStat();
            ls.ShowDialog();
        }
        TreeNode FilesCurNode = null;
        private void FilesList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (FilesCurNode != null)
            {
                FilesCurNode.BackColor = Color.White;
                FilesCurNode.ForeColor = Color.Black;
            }
            FilesList.SelectedNode.BackColor = System.Drawing.SystemColors.Highlight;
            FilesList.SelectedNode.ForeColor = Color.White;
            FilesCurNode = FilesList.SelectedNode;
        }

        private void FilesList_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Text == "")
            {
                TreeNode node = fe.EnumerateDirectory(e.Node);
            }
            if (e.Node.ImageIndex == 3)
            {
                e.Node.ImageIndex = 7;
                e.Node.SelectedImageIndex = 7;
            }
        }

        private void MenuToolDNS_Click(object sender, EventArgs e)
        {
            ModifyDNS dns = new ModifyDNS();
            dns.ShowDialog();
        }

        private void MenuEditEditTag_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    IHTMLElement ie = edr.editocx.getCurElement();
                    if (ie != null)
                    {
                        if (ie.tagName.ToLower().Equals("body")||PageWare.isPartElement(ie)) return;
                        TagEdit te = new TagEdit();
                        te.ele = ie;
                        te.ShowDialog();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void T2EditTag_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    IHTMLElement ie = edr.editocx.getCurElement();
                    if (ie != null)
                    {
                        if (ie.tagName.ToLower().Equals("body") || PageWare.isPartElement(ie)) return;
                        TagEdit te = new TagEdit();
                        te.ele = ie;
                        te.ShowDialog();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void FileOpend_AfterSelect(object sender, TreeViewEventArgs e)
        {
            form.doActive(e.Node.Tag.ToString());
        }

        private void FilesList_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.ImageIndex == 7)
            {
                e.Node.ImageIndex = 3;
                e.Node.SelectedImageIndex = 3;
            }
        }

        private void FilesList_DoubleClick(object sender, EventArgs e)
        {
            FilesListOpenFile();
        }
        private void FilesListOpenFile()
        {
            try
            {
                if (FilesList.SelectedNode == null)
                {
                    return;
                }
                int II=FilesList.SelectedNode.ImageIndex;
                if (II == 0 || II == 1 || II == 3 || II == 4 || II == 5 || II == 7) return;
                string filename = FilesList.SelectedNode.FullPath.Substring(12);
                if (filename.EndsWith(".html") || filename.EndsWith(".htm"))
                {
                    string fid = rdm.getID() + "_free";
                    form.addFreeFileEditor(filename, "Free Page", fid, fid, "Free Page", true);
                }
                else
                {
                    sheel.ExeSheel(filename);
                }
            }
            catch(Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuViewWebView_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {   
                    switch(edr.editmode)
                    {
                        case "edit": SiteClass.Site.constructViewPageFromEdit(edr.thisID, edr.thisUrl); break;
                        case "text": SiteClass.Site.constructViewPageFromText(edr.thisID, edr.thisUrl); break;
                        case "view": break;
                    }
                    sheel.ExeSheel(edr.thisViewUrl);
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void FilesListOpen_Click(object sender, EventArgs e)
        {
            FilesListOpenFile();
        }

        private void FilesListNewFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (FilesList.SelectedNode == null)
                {
                    return;
                }
                int II = FilesList.SelectedNode.ImageIndex;
                if (II == 0 || II == 1 || II == 3 || II == 5 || II == 7)
                {
                    string FolderName = FilesList.SelectedNode.FullPath.Substring(12);
                    InputName iname=new InputName();
                    iname.Text = "Input a directory name";
                    iname.ShowDialog();
                    if (!iname.NameValue.Text.Trim().Equals(""))
                    {
                        try
                        {
                            if (!dir.Exists((FolderName + "\\" + iname.NameValue.Text.Trim()).Replace("\\\\", "\\")))
                            {
                                dir.CreateDirectory((FolderName + "\\" + iname.NameValue.Text.Trim()).Replace("\\\\", "\\"));
                                
                                FilesList.SelectedNode.Nodes.Add(iname.NameValue.Text.Trim(), iname.NameValue.Text.Trim(), 3, 3);
                                FilesList.SelectedNode.Nodes.Clear();
                                FilesList.SelectedNode.Nodes.Add("");
                                fe.EnumerateDirectory(FilesList.SelectedNode);
                                FilesList.SelectedNode.Expand();
                            }
                        }
                        catch(Exception ex)
                        {
                            MsgBox.Error(ex.Message);
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void FilesListNewFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (FilesList.SelectedNode == null)
                {
                    return;
                }
                int II = FilesList.SelectedNode.ImageIndex;
                if (II == 0 || II == 1 || II == 3 || II == 5 || II == 7)
                {
                    string FolderName = FilesList.SelectedNode.FullPath.Substring(12);
                    InputName iname = new InputName();
                    iname.Text = "Input a file name";
                    iname.ShowDialog();
                    if (!iname.NameValue.Text.Trim().Equals(""))
                    {
                        try
                        {
                            if (!file.Exists((FolderName + "\\" + iname.NameValue.Text.Trim()).Replace("\\\\", "\\") + ".html"))
                            {
                                file.Copy(globalConst.emptyFile, (FolderName + "\\" + iname.NameValue.Text.Trim()).Replace("\\\\", "\\") + ".html");

                                FilesList.SelectedNode.Nodes.Add(iname.NameValue.Text.Trim() + ".html", iname.NameValue.Text.Trim() + ".html", 8, 8);
                                FilesList.SelectedNode.Nodes.Clear();
                                FilesList.SelectedNode.Nodes.Add("");
                                fe.EnumerateDirectory(FilesList.SelectedNode);
                                FilesList.SelectedNode.Expand();
                            }
                        }
                        catch (Exception ex)
                        {
                            MsgBox.Error(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void FilesListDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (FilesList.SelectedNode == null)
                {
                    return;
                }
                int II = FilesList.SelectedNode.ImageIndex;
                if (II == 0 || II == 1 || II == 4 || II == 5) return;
                if (II == 3 || II == 7)
                {
                    string FolderName = FilesList.SelectedNode.FullPath.Substring(12);
                    if (MsgBox.OKCancel(res.MainBox.GetString("m22")).Equals(DialogResult.OK))
                    {
                        dir.Delete(FolderName,true);
                        FilesList.SelectedNode.Remove();
                    }
                }
                else
                {
                    string FileName = FilesList.SelectedNode.FullPath.Substring(12);
                    if (MsgBox.OKCancel(res.MainBox.GetString("m23")).Equals(DialogResult.OK))
                    {
                        file.Delete(FileName);
                        FilesList.SelectedNode.Remove();
                    }
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void SiteTabNewFolder_Click(object sender, EventArgs e)
        {
            AddDir ad = new AddDir();
            ad.ShowDialog();
        }

        private void SiteTabNewFile_Click(object sender, EventArgs e)
        {
            AddPage ap = new AddPage();
            ap.PageType = 0;
            ap.ShowDialog();
        }

        private void SiteTabOpenFile_Click(object sender, EventArgs e)
        {
            openPage();
        }

        private void SiteTabDelete_Click(object sender, EventArgs e)
        {
            SiteTab_Delete();
        }

        private void SiteTabRefresh_Click(object sender, EventArgs e)
        {
            refreshSiteTree();
        }

        private void ControlTabAddControl_Click(object sender, EventArgs e)
        {
            AddControl();
        }

        private void ControlTabNewInstance_Click(object sender, EventArgs e)
        {
            if(ControlTree.SelectedNode!=null)
            {

                if (tree.getTypeFromID(ControlTree.SelectedNode).Equals("comp"))
                {
                    NewControlInstance1();
                }
                if (tree.getTypeFromID(ControlTree.SelectedNode).Equals("ctrl"))
                {
                    NewControlInstance2();
                }
            }
        }

        private void ControlTabCloneInstance_Click(object sender, EventArgs e)
        {
            if (ControlTree.SelectedNode != null)
            {
                if (tree.getTypeFromID(ControlTree.SelectedNode).Equals("ctrl"))
                {
                    CloneControlInstance();
                }
            }
        }

        private void ControlTabNewPart_Click(object sender, EventArgs e)
        {
            if (ControlTree.SelectedNode != null)
            {
                if (tree.getTypeFromID(ControlTree.SelectedNode).Equals("part"))
                {
                    NewPartInstance();
                }
            }
        }

        private void ControlTabClonePart_Click(object sender, EventArgs e)
        {
            if (ControlTree.SelectedNode != null)
            {
                if (tree.getTypeFromID(ControlTree.SelectedNode).Equals("part"))
                {
                    ClonePartInstance();
                }
            }
        }

        private void ControlTabDelete_Click(object sender, EventArgs e)
        {
            if (ControlTree.SelectedNode != null)
            {

                if (tree.getTypeFromID(ControlTree.SelectedNode).Equals("comp"))
                {
                    DeleteControl();
                }
                else if (tree.getTypeFromID(ControlTree.SelectedNode).Equals("ctrl"))
                {
                    DeleteControlInstance();
                }
                else if (tree.getTypeFromID(ControlTree.SelectedNode).Equals("part"))
                {
                    DeletePart();
                }
                else if (tree.getTypeFromID(ControlTree.SelectedNode).Equals("page"))
                {
                    ControlTabDeletePage();
                }
            }
        }

        private void ControlTabRefresh_Click(object sender, EventArgs e)
        {
            refreshControlTree();
        }
        private void updateControlTabTool()
        {
            if (ControlTree.SelectedNode == null) return;
            switch (tree.getNodeType(ControlTree.SelectedNode))
            {
                case tree.NodeType.Control:
                    ControlTabNewInstance.Enabled = true;
                    ControlTabCloneInstance.Enabled = false;
                    ControlTabNewPart.Enabled = false;
                    ControlTabClonePart.Enabled = false;
                    ControlTabDelete.Enabled = true;
                    break;
                case tree.NodeType.Instance:
                    ControlTabNewInstance.Enabled = true;
                    ControlTabCloneInstance.Enabled = true;
                    ControlTabNewPart.Enabled = false;
                    ControlTabClonePart.Enabled = false;
                    ControlTabDelete.Enabled = true;
                    break;
                case tree.NodeType.Part:
                    ControlTabNewInstance.Enabled = false;
                    ControlTabCloneInstance.Enabled = false;
                    ControlTabNewPart.Enabled = true;
                    ControlTabClonePart.Enabled = true;
                    ControlTabDelete.Enabled = true;
                    break;
                case tree.NodeType.Page:
                    ControlTabNewInstance.Enabled = false;
                    ControlTabCloneInstance.Enabled = false;
                    ControlTabNewPart.Enabled = false;
                    ControlTabClonePart.Enabled = false;
                    ControlTabDelete.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void MenuEditFind_Click(object sender, EventArgs e)
        {
            if (FindReplace.FindReplaceShow) return;
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("text"))
                    {
                        FindReplace fr=new FindReplace();
                        fr.Show();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuEditReplace_Click(object sender, EventArgs e)
        {
            if (FindReplace.FindReplaceShow) return;
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("text"))
                    {
                        FindReplace fr = new FindReplace();
                        fr.Show();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void T1Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.KeyCode.Equals(Keys.Enter)) return;
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("text"))
                    {
                        edr.textocx.Find(T1Search.Text, edr.textocx.SelectionStart + edr.textocx.SelectionLength,RichTextBoxFinds.None, null);
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
            //try
            //{
            //    Editor edr = form.getEditor();

            //    if (edr != null)
            //    {
            //        if (edr.editmode.Equals("text"))
            //        {
            //            FindReplace fr = new FindReplace();
            //            fr.FindString = T1Search.Text;
            //            fr.ShowDialog();
            //        }
            //    }
            //    edr = null;
            //}
            //catch (Exception ex)
            //{
            //    new error(ex);
            //}
        }

        private void AddSneppetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Snippet st=new Snippet();
            st.ShowDialog();
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cid = MainToolBox.SelectedTab.SelectedItem.Object.ToString();
            if (cid.Equals("Pointer")) return;
            Snippet st = new Snippet();
            st.IsEdit = true;
            st.ID = cid;
            st.ShowDialog();
        }

        private void MenuSiteTamplateManage_Click(object sender, EventArgs e)
        {
            TemplateManage tm=new TemplateManage();
            tm.ShowDialog();
        }

        private void MenuSiteCheck_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                SiteReport sr = new SiteReport();
                sr.ShowDialog();
            }
        }

        private void MenuSiteClearFiles_Click(object sender, EventArgs e)
        {
            if(globalConst.CurSite.ID!=null)
            {
                SiteClear sc=new SiteClear();
                sc.ShowDialog();
            }
        }

        private void MenuToolOption_Click(object sender, EventArgs e)
        {
            Options ops=new Options();
            ops.ShowDialog();
        }

        private void MenuFileFileSaveAs_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "HTML Files(*.htm;*.html)|*.htm;*.html|Web Files(*.asp;*.aspx;*.jsp;*.php;*.php3;)|*.asp;*.aspx;*.jsp;*.php;*.php3;|All Files(*.*)|*.*";
                    sfd.ShowDialog();
                    if (!sfd.FileName.Equals(""))
                    {
                        string fid = rdm.getID() + "_free";
                        edr.thisUrl = sfd.FileName;
                        edr.thisEditUrl = sfd.FileName;
                        edr.thisViewUrl = sfd.FileName;
                        edr.thisTitle = "Free Page";
                        edr.isFreeFile = true;
                        edr.isFreeFileSaved = true;
                        edr.Text = "Free Page" + " - " + sfd.FileName;
                        edr.thisID = fid;
                        edr.thisName = fid;
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuFileOpenFreeFile_Click(object sender, EventArgs e)
        {
            OpenFreeFile();
        }
        private void LastSite_Click(object sender, EventArgs e)
        {
            try
            {
                string siteid = ((ToolStripMenuItem)sender).Text;
                siteid = siteid.Substring(siteid.IndexOf(":") + 1, siteid.IndexOf("[") - siteid.IndexOf(":") - 1);
                SiteClass.Site.close();
                SiteClass.Site.open(siteid);
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        private void LastFile_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = ((ToolStripMenuItem)sender).Text;
                filename = filename.Substring(filename.IndexOf(":") + 1);
                if (file.Exists(filename))
                {
                    string fid = rdm.getID() + "_free";
                    form.addFreeFileEditor(filename, "Free Page", fid, fid, "Free Page", true);
                }
                else
                {
                    MsgBox.Warning(filename + res.MainBox.GetString("m24"));
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public void RefreshLastListMenus()
        {
            MenuFileLastSites.DropDownItems.Clear();
            string sql = "select * from lastlist where thetype='site' and thevalue<>'' order by id";
            DR dr = new DR(globalConst.ConfigConn.OpenRecord(sql));
            int i = 1;
            while (dr.Read())
            {
                ToolStripMenuItem mi = new ToolStripMenuItem();
                MenuFileLastSites.DropDownItems.Add(mi);
                mi.Text = "&" + i + ":" + dr.getString("thevalue");
                mi.Click += new EventHandler(LastSite_Click);
                i++;
            }
            dr.Close();
            MenuFileLastFiles.DropDownItems.Clear();
            sql = "select * from lastlist where thetype='file' and thevalue<>'' order by id";
            dr = new DR(globalConst.ConfigConn.OpenRecord(sql));
            i = 1;
            while (dr.Read())
            {
                ToolStripMenuItem mi = new ToolStripMenuItem();
                MenuFileLastFiles.DropDownItems.Add(mi);
                mi.Text = "&" + i + ":" + dr.getString("thevalue");
                mi.Click += new EventHandler(LastFile_Click);
                i++;
            }
            dr.Close();
        }

        private void MenuFileSaveFile_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void MenuFileSaveAll_Click(object sender, EventArgs e)
        {
            SaveAllFiles();
        }

        private void MenuFileCloseFile_Click(object sender, EventArgs e)
        {
            Editor ed = form.getEditor();
            if (ed != null) ed.Close();
        }

        private void MenuEditSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();
                if (edr != null)
                {
                    if (edr.EditSpace.SelectedIndex == 0)
                    {
                        edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_SelectAll, false, null);
                    }
                    else if (edr.EditSpace.SelectedIndex == 1)
                    {
                        edr.textocx.selectAll();
                    }
                    edr = null;
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuEditAddSnippet_Click(object sender, EventArgs e)
        {
            Snippet spt=new Snippet();
            spt.IsEdit = false;
            spt.ShowDialog();
        }

        private void T1Search_Click(object sender, EventArgs e)
        {
            if(T1Search.Text.Equals("Type words to search"))
            {
                T1Search.Text = "";
            }
        }

        private void MenuViewDesign_Click(object sender, EventArgs e)
        {
            MenuViewDesign.Checked = true;
            MenuViewCode.Checked = false;
            MenuViewPreview.Checked = false;
            T1Design.Checked = MenuViewDesign.Checked;
            T1Code.Checked = MenuViewCode.Checked;
            T1Preview.Checked = MenuViewPreview.Checked;
            T1Design_Click(sender, e);
        }

        private void MenuViewCode_Click(object sender, EventArgs e)
        {
            MenuViewDesign.Checked = false;
            MenuViewCode.Checked = true;
            MenuViewPreview.Checked = false;
            T1Design.Checked = MenuViewDesign.Checked;
            T1Code.Checked = MenuViewCode.Checked;
            T1Preview.Checked = MenuViewPreview.Checked;
            T1Code_Click(sender, e);
        }

        private void MenuViewPreview_Click(object sender, EventArgs e)
        {
            MenuViewDesign.Checked = false;
            MenuViewCode.Checked = false;
            MenuViewPreview.Checked = true;
            T1Design.Checked = MenuViewDesign.Checked;
            T1Code.Checked = MenuViewCode.Checked;
            T1Preview.Checked = MenuViewPreview.Checked;
            T1Preview_Click(sender, e);
        }

        private void MenuViewToolBox_Click(object sender, EventArgs e)
        {
            toolbar_toolbox.Checked = MenuViewToolBox.Checked;
            this.MainToolBox.Visible = toolbar_toolbox.Checked;
            this.splitter1.Visible = toolbar_toolbox.Checked;
        }

        private void MenuViewWorkSpace_Click(object sender, EventArgs e)
        {
            toolbar_workpace.Checked=MenuViewWorkSpace.Checked;
            DoWorkSpaceAndProperty();
        }

        private void MenuViewProperty_Click(object sender, EventArgs e)
        {
            toolbar_property.Checked = MenuViewProperty.Checked;
            DoWorkSpaceAndProperty();
        }

        private void MenuViewTag_Click(object sender, EventArgs e)
        {
            T1Tag.Checked=MenuViewTag.Checked;
            T1Tag_Click(sender, e);
        }

        private void MenuViewBorder_Click(object sender, EventArgs e)
        {
            T1Border.Checked=MenuViewBorder.Checked;
            T1Border_Click(sender, e);
        }

        private void MenuViewGrid_Click(object sender, EventArgs e)
        {
            T1Grid.Checked=MenuViewGrid.Checked;
            T1Grid_Click(sender, e);
        }

        private void MenuViewAlive_Click(object sender, EventArgs e)
        {
            T1Live.Checked=MenuViewAlive.Checked;
            T1Live_Click(sender, e);
        }

        private void MenuSiteSave_Click(object sender, EventArgs e)
        {
            if(globalConst.CurSite.ID!=null)
            {
                SiteClass.Site.Save();
            }
        }

        private void MenuPageInsertLink_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<a href=\"\">HyperLink</a>");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageCancelLink_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_Unlink, false, null);
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageInsertTable_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<table><tbody><tr><td>Table</td><td></td><td></td></tr><tr><td></td><td></td><td></td></tr><tr><td></td><td></td><td></td></tr></tbody></table>");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageInsertRow_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.tableInsertRow();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageInsertColumn_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.tableInsertCol();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageDeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.tableInsertRow();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageDeleteColumn_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.tableDeleteCol();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageSplit2Rows_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.tableSplitCell2Rows();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageSplit2Columns_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.tableSplitCell2Cols();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageMergeCellLeft_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.tableMergeCellsLeft();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageMergeCellRight_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.tableMergeCellsRight();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageMergeCellUp_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.tableMergeCellsUp();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageMergeCellDown_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.tableMergeCellsDown();
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageClearTable_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        IHTMLTable t = (IHTMLTable)edr.editocx.getTableElement(edr.editocx.getCurElement());
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
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageSelectTable_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        IHTMLElement t = edr.editocx.getTableElement(edr.editocx.getCurElement());
                        if (t != null)
                        {
                            IHTMLTxtRange rg;
                            HTMLDocumentClass hc = new HTMLDocumentClass();
                            IHTMLDocument2 doc2 = hc;
                            doc2 = (IHTMLDocument2)edr.editocx.DOM;
                            IHTMLBodyElement ib = (IHTMLBodyElement)doc2.body;
                            rg = ib.createTextRange();
                            rg.moveToElementText(t);
                            //rg.scrollIntoView(true);
                            rg.select();
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

        private void MenuPageElementImage_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<img src=\"\">");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementHorizon_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<hr>");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementButton_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<input type=\"button\" value=\"Button\">");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementInputText_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<input type=\"text\">");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementTextArea_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<textarea></textarea>");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementInputPassword_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<input type=\"password\">");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementSelect_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<select></select>");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementRadio_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<input type=\"radio\">");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementCheckBox_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<input type=\"checkbox\">");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementPanel_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<div style=\"WIDTH: 100px; HEIGHT: 100px\">Div</div>");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementMarquee_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.execCommand(D4ENUM.D4HTMLCMDSTR.HTMLCMD_InsertMarquee, false, "Marquee");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementFrame_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<iframe style=\"WIDTH: 300px; HEIGHT: 138px\"></iframe>");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementInputFile_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<input type=\"file\">");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementInputHidden_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<input type=\"hidden\">");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementSubmitImage_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<input type=\"image\">");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementSubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<input type=\"submit\" value=\"Submit\">");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementInputReset_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<input type=\"reset\" value=\"Reset\">");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementMultipleSelect_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<select size=\"3\"></select>");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuPageElementParagraph_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    if (edr.editmode.Equals("edit"))
                    {
                        edr.editocx.pasteHtml("<p>Paragraph</p>");
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void MenuToolSystemSet_Click(object sender, EventArgs e)
        {
            if(globalConst.CurSite.ID!=null)
            {
                sheel.ExeSheel(globalConst.CurSite.URL + "/system.set");
            }
        }
        private void MenuHelpSiteMatrix_Click(object sender, EventArgs e)
        {
            //sheel.ExeSheel("http://www.syslive.com.cn");
        }
        private void MenuHelpD4System_Click(object sender, EventArgs e)
        {
            functions.sheel.ExeSheel("http://www.ftframe.com/doc/helper.mht");
        }
        private void MenuHelpD4SoftHomePage_Click(object sender, EventArgs e)
        {
            //sheel.ExeSheel("http://www.syslive.com.cn");
        }


        private void MenuToolContentSet_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                sheel.ExeSheel(globalConst.CurSite.URL + "/content.set");
            }
        }

        private void MenuWindowCascade_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void MenuWindowTileHorizontal_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void MenuWindowTileVertical_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void MenuWindowAllClose_Click(object sender, EventArgs e)
        {
            foreach(Form f in this.MdiChildren)
            {
                f.Close();
            }
        }

        private void MenuWindowFormList_Click(object sender, EventArgs e)
        {
            toolbar_workpace.Checked = true;
            MenuViewWorkSpace.Checked = toolbar_workpace.Checked;
            DoWorkSpaceAndProperty();
            WorkSpace.SelectedIndex = 2;
        }

        private void MenuHelpAbout_Click(object sender, EventArgs e)
        {
            About a=new About();
            a.ShowDialog();
        }

        private void T3SiteRegister_Click(object sender, EventArgs e)
        {
            SiteAdd _SiteAdd = new SiteAdd();
            _SiteAdd.ShowDialog();
        }

        private void T3SiteAdd_Click(object sender, EventArgs e)
        {
             AddSite _AddSite = new AddSite();
            _AddSite.ShowDialog();
        }

        private void T3SiteManage_Click(object sender, EventArgs e)
        {
            SiteList slt = new SiteList();
            slt.ShowDialog();
        }

        private void T3OpenSite_Click(object sender, EventArgs e)
        {
            SiteList slt = new SiteList();
            slt.ShowDialog();
        }

        private void T3CloseSite_Click(object sender, EventArgs e)
        {
            SiteClass.Site.close();
        }

        private void T3SaveSite_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                SiteClass.Site.Save();
            }
        }

        private void T3SycronSite_Click(object sender, EventArgs e)
        {
            if (MsgBox.YesNo("同步站点将覆盖本地站点，可能造成本地站点数据丢失，确定要继续吗？").Equals(DialogResult.Yes))
            {
                SiteUpdate su = new SiteUpdate();
                su.siteid = globalConst.CurSite.ID;
                su.ShowDialog();
            }
        }

        private void T3SitePublish_Click(object sender, EventArgs e)
        {
            SitePublish sp = new SitePublish();
            sp.ShowDialog();
        }

        private void T3SiteExport_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                SiteExport se = new SiteExport();
                se.AsTemplate = false;
                se.ShowDialog();
            }
        }

        private void T3SiteImport_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                SiteImport se = new SiteImport();
                se.AsTemplate = false;
                se.ShowDialog();
            }
        }

        private void T3TemplateManage_Click(object sender, EventArgs e)
        {

            TemplateManage tm = new TemplateManage();
            tm.ShowDialog();
        }

        private void T3TemplateImport_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                SiteImport se = new SiteImport();
                se.AsTemplate = true;
                se.ShowDialog();
            }
        }

        private void T3TemplateExport_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                SiteExport se = new SiteExport();
                se.AsTemplate = true;
                se.ShowDialog();
            }
        }

        private void T3PageImport_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                ImportPage ip = new ImportPage();
                ip.ShowDialog();
            }
        }

        private void T3SiteCheck_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                SiteReport sr = new SiteReport();
                sr.ShowDialog();
            }
        }

        private void T3UpdateData_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID == null) return;
            LoadStat ls = new LoadStat();
            ls.ShowDialog();
        }

        private void MenuSiteModifyCSS_Click(object sender, EventArgs e)
        {
            ModifyCSS mc=new ModifyCSS();
            mc.ShowDialog();
        }

        private void formNewPageUser_Click(object sender, EventArgs e)
        {
            AddPage ap = new AddPage();
            ap.PageType = 1;
            ap.ShowDialog();
        }

        private void formNewPageManager_Click(object sender, EventArgs e)
        {
            AddPage ap = new AddPage();
            ap.PageType = 2;
            ap.ShowDialog();
        }

        private void MenuEditAdvEle2Label_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    IHTMLElement ie = edr.editocx.getCurElement();
                    if (ie != null)
                    {
                        if (PageWare.isFormElement(ie))
                        {
                            string tagstr = "";
                            if (ie.getAttribute("tag", 0) != null)
                            {
                                tagstr = " tag=\"" + ie.getAttribute("tag", 0).ToString() + "\"";
                            }
                            ie.outerHTML = "<LABEL id=\"" + ie.id + "\"" + tagstr + ">" + res.form.GetString("String11") + "</LABEL>";
                        }
                        else
                        {
                            ie.outerHTML = "<LABEL id=\"" + ie.id + "\">(Label)</LABEL>";
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UpdateParts up = new UpdateParts();
            up.ShowDialog();
        }

        private void CreateGUIDMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Editor edr = form.getEditor();

                if (edr != null)
                {
                    IHTMLElement ie = edr.editocx.getCurElement();
                    if (ie != null)
                    {
                        if (PageWare.isFormElement(ie))
                        {
                            PropertySpace.FormElement.func.setNewID(ie,rdm.getCombID());
                        }
                        else
                        {
                            ie.id = rdm.getCombID();
                        }
                        Page.PageWare.doHtmlAdapter(ie);
                    }
                }
                edr = null;
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void pubpageMenuItem2_Click(object sender, EventArgs e)
        {
            if (SiteTree.SelectedNode == null) return;
            string id = tree.getID(SiteTree.SelectedNode);
            if (tree.getTypeFromID(id).Equals("page"))
            {
                SitePublish sp = new SitePublish();
                sp.PublishSinglePageTag = new string[] { id, tree.getPath(SiteTree.SelectedNode) ,"16"};
                sp.PublishSinglePage = true;
                sp.ShowDialog();
            }
        }

        private void eleasisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new EleAssis().Show();
        }

        private void sortaddtimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableAllControlSortMenu();
            globalConst.ctrlTreeOrderby = "addtime";
            globalConst.ctrlTreeOrdertype = "asc";
            refreshControlTree();
            nameASCToolStripMenuItem.Checked = true;
        }

        private void sortaddtime2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableAllControlSortMenu();
            globalConst.ctrlTreeOrderby = "addtime";
            globalConst.ctrlTreeOrdertype = "desc";
            refreshControlTree();
            nameASCToolStripMenuItem.Checked = true;
        }

        private void pageopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PageAssis.PageAssisShow) PageAssis.PageAssisForm.Activate();
            else
            {
                PageAssis pa = new PageAssis();
                pa.Show();
            }
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Editor ed = form.getEditor();
            if (ed != null)
            {
                SitePublish sp = new SitePublish();
                sp.PublishSinglePageTag = new string[] { ed.thisID, ed.thisUrl.Substring((globalConst.AppPath+@"\sites\"+globalConst.CurSite.ID).Length), "16" };
                sp.PublishSinglePage = true;
                sp.ShowDialog();
            }
        }

        private void quickpubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click(sender, e);
        }

        private void bUGcheckactiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BugCheck bc = new BugCheck();
            bc.IsQuick = true;
            bc.ShowDialog();
        }

        private void bUGcheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BugCheck bc = new BugCheck();
            bc.IsQuick = false;
            bc.ShowDialog();
        }
      
        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            SaveAllFiles(false);
            //if (DataOpDefine.DataOpFormShow) DataOpDefine.DataOpDefineForm.TopMost = false;
            //if (DyValueDefine.DyValueFormShow) DyValueDefine.DyValueDefineForm.TopMost = false;
        }
        private void MainForm_Activated(object sender, EventArgs e)
        {
            form.OutEdited();
            //if (DataOpDefine.DataOpFormShow) DataOpDefine.DataOpDefineForm.TopMost = true;
            //if (DyValueDefine.DyValueFormShow) DyValueDefine.DyValueDefineForm.TopMost = true;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SplitPublish sp = new SplitPublish();
            sp.ShowDialog();
        }

        private void Openwithvscode_Click(object sender, EventArgs e)
        {
            if (SiteTree.SelectedNode == null) return;
            string id = tree.getID(SiteTree.SelectedNode);
            if (!tree.getTypeFromID(id).Equals("page")) return;
            string path = globalConst.CurSite.Path + tree.getPath(SiteTree.SelectedNode);
            string vscodepath = Options.GetSystemValue("vscodepath");
            if(vscodepath==null || vscodepath=="")
            {
                MsgBox.Error("需要在选项中先配置VSCode地址");
                return;
            }
            Process myProcess = new Process();
            try
            {
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = vscodepath;
                myProcess.StartInfo.Arguments = "\""+ path + "\"";
                myProcess.StartInfo.CreateNoWindow = false;
                myProcess.Start();
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }

        private void Pa_div_fuzhi_Click(object sender, EventArgs e)
        {
            classes.PageAsist.DivCopyNew(((IHTMLElement)((object[])MainToolBox[5].SelectedItem.Object)[1]));
        }

        private void Pa_div_change_Click(object sender, EventArgs e)
        {
            classes.PageAsist.DivMod(((IHTMLElement)((object[])MainToolBox[5].SelectedItem.Object)[1]));
        }

        private void Pa_div_del_Click(object sender, EventArgs e)
        {
            classes.PageAsist.DivDel(((IHTMLElement)((object[])MainToolBox[5].SelectedItem.Object)[1]));
        }

        private void Pa_div_dingwei_Click(object sender, EventArgs e)
        {
            ((IHTMLElement)((object[])MainToolBox[5].SelectedItem.Object)[1]).scrollIntoView();
        }

        private void Pa_part_clone_new_Click(object sender, EventArgs e)
        {
            classes.PageAsist.ColonePartNew(((IHTMLElement)((object[])MainToolBox[5].SelectedItem.Object)[1]), ((string)((object[])MainToolBox[5].SelectedItem.Object)[3]));
        }

        private void Pa_part_clone_change_Click(object sender, EventArgs e)
        {
            classes.PageAsist.ColonePartReplace(((IHTMLElement)((object[])MainToolBox[5].SelectedItem.Object)[1]), ((string)((object[])MainToolBox[5].SelectedItem.Object)[3]));
        }

        private void Pa_part_copyID_Click(object sender, EventArgs e)
        {
            classes.PageAsist.CopyPartID(((IHTMLElement)((object[])MainToolBox[5].SelectedItem.Object)[1]));
        }

        private void Pa_part_setting_Click(object sender, EventArgs e)
        {
            classes.PageAsist.BindSetting(((IHTMLElement)((object[])MainToolBox[5].SelectedItem.Object)[1]));
        }

        private void Pa_part_copyGetvalue_Click(object sender, EventArgs e)
        {
            classes.PageAsist.CopyGetValue(((IHTMLElement)((object[])MainToolBox[5].SelectedItem.Object)[1]));
        }

        private void Pa_part_del_Click(object sender, EventArgs e)
        {
            classes.PageAsist.PartDel(((IHTMLElement)((object[])MainToolBox[5].SelectedItem.Object)[1]));
        }

        private void Pa_part_copyLabel_Click(object sender, EventArgs e)
        {
            classes.PageAsist.CopyPartLabel(((IHTMLElement)((object[])MainToolBox[5].SelectedItem.Object)[1]));
        }

        private void Pa_part_dingwei_Click(object sender, EventArgs e)
        {
            ((IHTMLElement)((object[])MainToolBox[5].SelectedItem.Object)[1]).scrollIntoView();
        }

        private void Pa_refresh_Click(object sender, EventArgs e)
        {
            classes.PageAsist.RefreshToolBoxPages();
        }

        private void MainToolBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            classes.PageAsist.MouseDoubleClick(e);
        }

        private void MainToolBox_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void MainToolBox_MouseClick(object sender, MouseEventArgs e)
        {
            classes.PageAsist.MouseClick(e);
        }

        private void ImportSinglePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                SiteImport se = new SiteImport();
                se.AsTemplate = false;
                se.ForSingle = true;
                se.ShowDialog();
            }
        }

        private void ExportSinglePage_Click(object sender, EventArgs e)
        {
            if (SiteTree.SelectedNode == null) return;
            string id = tree.getID(SiteTree.SelectedNode);
            if (tree.getTypeFromID(id).Equals("page"))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "站点文件(*.site)|*.site";
                sfd.ShowDialog();
                if(sfd.FileName!=null && sfd.FileName!="")
                {
                    ArrayList UpdatePages = new ArrayList();
                    UpdatePages.Add(new string[] { id, tree.getPath(SiteTree.SelectedNode), "16" });
                    string export=SiteClass.Site.ExportForFiles(globalConst.CurSite.ID, UpdatePages, sfd.FileName);
                    if(export!=null)
                    {
                        MsgBox.Error(export);
                    }
                    else
                    {
                        MsgBox.Information("导出成功！");
                    }
                }
            }
        }

        private void Pa_div_newdiv_Click(object sender, EventArgs e)
        {
            classes.PageAsist.DivAddNew();
        }
    }
}