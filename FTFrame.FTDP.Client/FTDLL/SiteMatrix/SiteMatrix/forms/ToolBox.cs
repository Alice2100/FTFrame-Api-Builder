using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SiteMatrix.database;
using System.Data.OleDb;
using SiteMatrix.consts;
using SiteMatrix.functions;
using SiteMatrix.controls;

namespace SiteMatrix.forms
{
	/// <summary>
	/// ToolBox 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
	public class ToolBox : System.Windows.Forms.Form
	{
		public static System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ToolBox()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl),this);
			InitializeComponent();

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
			listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// listView1
			// 
			listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																				   this.columnHeader1});
			listView1.Location = new System.Drawing.Point(32, 32);
			listView1.Name = "listView1";
			listView1.Size = new System.Drawing.Size(224, 160);
			listView1.TabIndex = 0;
			listView1.View=View.LargeIcon ;
			listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(40, 208);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "双击插入";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(120, 208);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(64, 24);
			this.button1.TabIndex = 2;
			this.button1.Text = "删除控件";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(112, 256);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(88, 24);
			this.button2.TabIndex = 3;
			this.button2.Text = "添加控件";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// ToolBox
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(282, 320);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(listView1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ToolBox";
			this.ShowInTaskbar = false;
			this.Text = "ToolBox";
			this.Load += new System.EventHandler(this.ToolBox_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void ToolBox_Load(object sender, System.EventArgs e)
		{
			//初始化组件箱组件
            InitToolBoxControls();
		}
        public static void InitToolBoxControls()
		{
			try
			{
                controls.controls.InitControlIcon();
                globalConst.MdiForm.MainToolBox.SmallImageList = globalConst.ControlsImages;
                globalConst.MdiForm.MainToolBox.LargeImageList = globalConst.ControlsImages;
                globalConst.MdiForm.InitToolBoxControls();
                for (int i = globalConst.MdiForm.MainToolBox[1].ItemCount - 1; i >= 0; i--)
                {
                    globalConst.MdiForm.MainToolBox[1].SelectedItemIndex = i;
                }
                for (int i = globalConst.MdiForm.MainToolBox[2].ItemCount - 1; i >= 0; i--)
                {   
                    globalConst.MdiForm.MainToolBox[2].SelectedItemIndex = i;
                }
                
			}
			catch(Exception ex)
			{
				new error(ex);
			}
		}

		private void listView1_DoubleClick(object sender, System.EventArgs e)
		{
			AddControl2Page aw=new AddControl2Page();
			aw.controlName=((string[])listView1.SelectedItems[0].Tag)[0];
			aw.ShowDialog(this);
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			Cursor c=Cursor.Current;
			try
			{
				
				Cursor.Current=Cursors.WaitCursor;
				if(listView1.SelectedItems.Count>0)
				{
					if(MsgBox.OKCancel("Delete this control?")==DialogResult.OK)
					{
						string controlname=((string[])listView1.SelectedItems[0].Tag)[0];
						controls.controls.DeleteControl(controlname);
                        InitToolBoxControls();
						Workspace.refreshControlTree();
					}
				}
				Cursor.Current=c;
			}
			catch(Exception ex)
			{
				new error(ex);
				Cursor.Current=c;
			}
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			Cursor c=Cursor.Current;
			try
			{
				Cursor.Current=Cursors.WaitCursor;
				controls.controls.AddControl();
                InitToolBoxControls();
				Workspace.refreshControlTree();
				Cursor.Current=c;
			}
			catch(Exception ex)
			{
				new error(ex);
				Cursor.Current=c;
			}
		}
	}
}
