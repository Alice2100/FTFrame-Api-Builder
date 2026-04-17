using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using FTDPClient.consts;
using FTDPClient.functions;
using FTDPClient.database;
using FTDPClient.forms;
namespace FTDPClient.forms
{
	/// <summary>
	/// Loading 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class Loading : System.Windows.Forms.Form
	{
		public static  htmleditocx.D4HtmlEditOcx editocx;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private ProgressBar progressBar1;
        private Label label2;
        private PictureBox pictureBox1;
        private Label label3;
		private System.ComponentModel.IContainer components;

		public Loading()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
			InitializeComponent();
            ApplyLanguage();
			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Loading));
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(86, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(268, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "FTDPClient is loading,please wait...";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.Color.Moccasin;
            this.progressBar1.Location = new System.Drawing.Point(53, 193);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(225, 16);
            this.progressBar1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(164, 218);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-2, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(329, 245);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Silver;
            this.label3.Location = new System.Drawing.Point(49, 215);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 22);
            this.label3.TabIndex = 4;
            this.label3.Text = "FTDP 2.0";
            // 
            // Loading
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(325, 245);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Loading";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loading";
            this.TransparencyKey = System.Drawing.Color.Maroon;
            this.Load += new System.EventHandler(this.Loading_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private void ApplyLanguage()
        {
            //label1.Text = res.Loading.GetString("label1");
            label1.Text = "FTDPClient is loading,please wait...";
        }

		private void Loading_Load(object sender, System.EventArgs e)
		{
            progressBar1.Value = 10;
            timer1.Enabled = true;
            label2.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
            //timer1.Enabled = false;
            //try
            //{
            //    FolderBrowserDialog dialog = new FolderBrowserDialog();
            //    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    {
            //        string dir = dialog.SelectedPath;
            //        if (Directory.Exists(dir + @"\newdb")) Directory.Delete(dir + @"\newdb", true);
            //        Directory.CreateDirectory(dir + @"\newdb");
            //        foreach (string filepath in Directory.GetFiles(dir))
            //        {
            //            if (filepath.EndsWith(".db"))
            //            {
            //                FileInfo file = new FileInfo(filepath);
            //                if (file.Name == "config.db")
            //                {
            //                    File.Copy(Application.StartupPath + @"\cfg\template\config.db", dir + @"\newdb\config.db");
            //                    using (DB db = new DB())
            //                    {
            //                        db.Open(functions.db.ConnStr(dir + @"\newdb\config.db"));
            //                        using (DBOle dBOle = new DBOle())
            //                        {
            //                            dBOle.Open("Provider=" + globalConst.OLEDBProvider + ";Data Source=" + filepath);
            //                            tableData(dBOle,db,"controls");
            //                            tableData(dBOle, db, "languages");
            //                            tableData(dBOle, db, "lastlist");
            //                            tableData(dBOle, db, "parts");
            //                            tableData(dBOle, db, "sites");
            //                            tableData(dBOle, db, "snippets");
            //                            tableData(dBOle, db, "system");
            //                        }
            //                    }
            //                }
            //                else if(file.Name.StartsWith("site_"))
            //                {
            //                    File.Copy(Application.StartupPath + @"\cfg\template\empty.db", dir + @"\newdb\"+ file.Name);
            //                    using (DB db = new DB())
            //                    {
            //                        db.Open(functions.db.ConnStr(dir + @"\newdb\" + file.Name));
            //                        using (DBOle dBOle = new DBOle())
            //                        {
            //                            dBOle.Open("Provider=" + globalConst.OLEDBProvider + ";Data Source=" + filepath);
            //                            tableData(dBOle, db, "codeset");
            //                            tableData(dBOle, db, "controls");
            //                            tableData(dBOle, db, "deledds");
            //                            tableData(dBOle, db, "directory");
            //                            tableData(dBOle, db, "formrules");
            //                            tableData(dBOle, db, "pages");
            //                            tableData(dBOle, db, "part_in_page");
            //                            tableData(dBOle, db, "parts");
            //                            tableData(dBOle, db, "share_data");
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch(Exception ex)
            //{
            //    new error(ex);
            //}
            //void tableData(DBOle dBOle,DB db,string table)
            //{
            //    using (DROle dROle = new DROle(dBOle.OpenRecord("select * from "+ table)))
            //    {
            //        while (dROle.Read())
            //        {
            //            string sql = "insert into "+ table + "(";
            //            string val = "values(";
            //            for (int i = 0; i < dROle.FieldCount; i++)
            //            {
            //                sql += dROle.GetName(i);
            //                val += "'" + str.Dot2DotDot(dROle.getValue(i).ToString()) + "'";
            //                if (i < dROle.FieldCount - 1)
            //                {
            //                    sql += ",";
            //                    val += ",";
            //                }
            //            }
            //            sql = sql + ")" + val + ")";
            //            db.execSql(sql);
            //        }
            //    }
            //}
            //return;

                try
			{
                progressBar1.Value = 20;
				timer1.Enabled=false;
				//global const
				globalConst.AppPath=Application.StartupPath;
				globalConst.AppFile=Application.ExecutablePath;
				globalConst.SitesPath=globalConst.AppPath + @"\sites";
                globalConst.TemplatePath = globalConst.AppPath + @"\templates";
                globalConst.FreeFilesPath = globalConst.AppPath + @"\freefiles";
				globalConst.ConfigPath=globalConst.AppPath + @"\cfg";
				globalConst.LibPath=globalConst.AppPath + @"\lib";
				globalConst.ConfigFile=globalConst.AppPath + @"\cfg\config.db";
				globalConst.emptyFile=globalConst.AppPath + @"\cfg\empty.ds";
				globalConst.emptyTxtFile=globalConst.AppPath + @"\cfg\empty";
                globalConst.defaultCssFile = globalConst.AppPath + @"\cfg\default.css";
				globalConst.LogOutputPath=globalConst.AppPath + @"\log";
				globalConst.ImgsPath=globalConst.AppPath + @"\img\";
                globalConst.DefaultControlIcon=Image.FromFile(globalConst.ImgsPath + "\\control.gif");
                globalConst.TagsIcon = Image.FromFile(globalConst.ImgsPath + "\\tags.gif");
				//config data connection
				globalConst.ConfigConn=new DB();
                globalConst.ConfigConn.Open(db.ConnStr_Cfg());
				//Init Css Document
				//globalConst.CssEleDoc=new System.Xml.XmlDocument();
				//globalConst.CssTypeDoc=new System.Xml.XmlDocument();
				//globalConst.CssEleDoc.Load(globalConst.LibPath + @"\d4cs.dll");
				//globalConst.CssTypeDoc.Load(globalConst.LibPath + @"\d4st.dll");
				//Init sysFonts
                progressBar1.Value = 30;
				globalConst.SysFonts=new System.Drawing.Text.InstalledFontCollection();
				
				string sqlstr="";
				//language
				sqlstr="select thevalue from system where name='curlanguage'";
				globalConst.Language=globalConst.ConfigConn.GetString(sqlstr);
				System.Threading.Thread.CurrentThread.CurrentUICulture=new System.Globalization.CultureInfo(globalConst.Language);
				//mdiposition
                progressBar1.Value = 40;
                try
                {
                    mdifromConst.windowwidth = int.Parse(getSystemSetValue("windowwidth"));
                    mdifromConst.windowheight = int.Parse(getSystemSetValue("windowheight"));
                    mdifromConst.windowleft = int.Parse(getSystemSetValue("windowleft"));
                    mdifromConst.windowtop = int.Parse(getSystemSetValue("windowtop"));
                    mdifromConst.windowstate = getSystemSetValue("windowstate");
                    mdifromConst.curlanguage = getSystemSetValue("curlanguage");
                    mdifromConst.mainmenux = int.Parse(getSystemSetValue("mainmenux"));
                    mdifromConst.mainmenuy = int.Parse(getSystemSetValue("mainmenuy"));
                    mdifromConst.panelwidth = int.Parse(getSystemSetValue("panelwidth"));
                    mdifromConst.propertyvisible = int.Parse(getSystemSetValue("propertyvisible"));
                    mdifromConst.sceditbgcolor = getSystemSetValue("sceditbgcolor");
                    mdifromConst.sceditfont = getSystemSetValue("sceditfont");
                    mdifromConst.sceditsize = int.Parse(getSystemSetValue("sceditsize"));
                    mdifromConst.sceditwraped = int.Parse(getSystemSetValue("sceditwraped"));
                    mdifromConst.toolboxvisible = int.Parse(getSystemSetValue("toolboxvisible"));
                    mdifromConst.toolboxwidth = int.Parse(getSystemSetValue("toolboxwidth"));
                    mdifromConst.toolcommonx = int.Parse(getSystemSetValue("toolcommonx"));
                    mdifromConst.toolcommony = int.Parse(getSystemSetValue("toolcommony"));
                    mdifromConst.toolsitevisible = int.Parse(getSystemSetValue("toolsitevisible"));
                    mdifromConst.toolsitex = int.Parse(getSystemSetValue("toolsitex"));
                    mdifromConst.toolsitey = int.Parse(getSystemSetValue("toolsitey"));
                    mdifromConst.tooltextvisible = int.Parse(getSystemSetValue("tooltextvisible"));
                    mdifromConst.tooltextx = int.Parse(getSystemSetValue("tooltextx"));
                    mdifromConst.tooltexty = int.Parse(getSystemSetValue("tooltexty"));
                    mdifromConst.workspaceheight = int.Parse(getSystemSetValue("workspaceheight"));
                    mdifromConst.workspacevisible = int.Parse(getSystemSetValue("workspacevisible"));
                    mdifromConst.LoadDefault = false;
                }
                catch 
                {
                    mdifromConst.LoadDefault = true;
                }
				//
				//				//缓存Editor
				//				Editor ed=new Editor();
				//				ed.thisUrl=globalConst.emptyTxtFile;
				//				ed.thisTitle="";
				//				ed.thisID="";
				//				ed.thisName="";
				//				ed.Text=ed.thisTitle;
				//				ed.Visible=false;
				//				ed.Show();
				//				ed.Close();
				//删除temp目录临时dll
				//string sql="insert into sites(id,domin,caption,username,passwd,cdkey,version,url,homepage,enterprise)values('qq',' ',' ','qq','qq','qq',0,'qq','','')";
				//string sql="insert into sites(id,domin)values('qq','')";
                progressBar1.Value = 50;
                if(System.IO.Directory.Exists(globalConst.LibPath + "\\temp"))
                {
				    System.IO.Directory.Delete(globalConst.LibPath + "\\temp",true);
                }
				System.IO.Directory.CreateDirectory(globalConst.LibPath + "\\temp");

                if(Directory.Exists(globalConst.FreeFilesPath))System.IO.Directory.Delete(globalConst.FreeFilesPath, true);
                System.IO.Directory.CreateDirectory(globalConst.FreeFilesPath);	
                
                //control icons
                controls.controls.InitControlIcon();
				//imgs
				globalConst.Imgs=new ImageList();
				globalConst.Imgs.TransparentColor=Color.Transparent;
				int i;
				for(i=0;file.Exists(globalConst.ImgsPath + i + ".gif");i++)
				{
					globalConst.Imgs.Images.Add(Image.FromFile(globalConst.ImgsPath + i + ".gif"));
				}
                progressBar1.Value = 60;
                //Editor
                editocx = new htmleditocx.D4HtmlEditOcx();
                // editocx
                // 
                editocx.Location = new System.Drawing.Point(130, 70);
                editocx.Name = "editocx";
                editocx.showBodyNetCells = false;
                editocx.showGlyphs = false;
                editocx.showLiveResizes = false;
                editocx.showTableBorders = true;
                editocx.Size = new System.Drawing.Size(1, 1);
                editocx.TabIndex = 1;
                editocx.Visible = true;
                this.Controls.Add(editocx);
                progressBar1.Value = 70;
                Application.DoEvents();
                editocx.LoadDocument(globalConst.emptyFile);
                while (editocx.readyState().Equals("loading"))
                {
                    Application.DoEvents();
                }
                progressBar1.Value = 90;
				MainForm mf=new MainForm();
				mf.Owner=this;


                globalConst.TextEditorControlProp = new ArrayList();
                globalConst.TextEditorControlProp.AddRange(new string[]{
                "dataop-CheckRule",
                "dataop-JSBefore",
                "dataop-JSSuccess",
                "dataop-CloseButton",
                "dataop-DefaultFID",
                "dyvalue-AppendJS0",
                "dyvalue-AppendJS1",
                "dyvalue-YBAppendJS0",
                "dyvalue-YBAppendJS1",
                "dyvalue-ExecBefore",
                "dyvalue-ExecAfter",
                "dyvalue-CustomConnection",
                "dyvalue-DefaultFID",
                "list-MenuOpendurer",
                "list-AdvSearch",
                "dataop-OPContidionJs",
                "dataop-CodeBefore",
                "dataop-CodeAfter",
                "dataop-OPContidionCode",
                "dataop-CustomConnection",
                //"list-SearchDefine",
                "list-StrictSearch",
                "list-MenuOppic",
                //"list-CusCdnCols",
                "list-CacuRowData",
                "list-BlockDataDefine",
                "list-CusCondition",
                "list-AppendTitle",
                "list-LoadEndJS",
                "list-CustomConnection",
                "list-SchAreaApdHtml",
                "list-ExecBefore",
                "list-ExecAfter",
                "list-CacuRowData",
                });

                globalConst.SqlEditorControlProp = new ArrayList();
                globalConst.SqlEditorControlProp.AddRange(new string[]{
                "select-RowSQL",
                "select-TreeRootSQL",
                "select-TreeRowSQL",
                "list-CusSQL",
                "list-CusSQLHalf",
                "dataop-OPContidionSql",
                "dataop-BeforeSql",
                 "dataop-AfterSql"
                });

                progressBar1.Value = 100;
                this.Visible = false;
                globalConst.MdiForm = mf;
                globalConst.MdiForm.UpdateMenusAndToolBars4Site();
                globalConst.MdiForm.UpdateMenusAndToolBars4Page();
				mf.Show();
                form.UpdateFileOpend(null, false);
			    //copy resource files:
                //System.IO.DirectoryInfo di=new DirectoryInfo(@"D:\d4soft\bk\wang\work\FTDPClient\FTDPClient\resource");
                //FileInfo[] fis = di.GetFiles();
                //foreach(FileInfo f in fis)
                //{
                //    string n=f.Name.Substring(0, f.Name.IndexOf("."));
                //    if(!File.Exists(di.FullName + "\\" + n + ".ja.resx"))
                //    {
                //        f.CopyTo(di.FullName + "\\" + n + ".ja.resx");
                //        f.CopyTo(di.FullName + "\\" + n + ".zh-chs.resx");
                //        f.CopyTo(di.FullName + "\\" + n + ".zh-tw.resx");
                //    }
                //}
			}	
			catch(Exception ex)
			{
				new error(ex);
			}
		}
        private string getSystemSetValue(string s)
        {
            return globalConst.ConfigConn.GetString("select thevalue from system where name='" + s + "'");
        }
	}
}