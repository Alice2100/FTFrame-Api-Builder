using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FTDPClient.functions;
using FTDPClient.Page;
using mshtml;
using FTDPClient.consts;

namespace FTDPClient.forms
{
	/// <summary>
	/// NewControl 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class NewControl : System.Windows.Forms.Form
	{
		public bool isCancel=true;
		public string controlName;
        public string addType = "";
        public string oldpartid = "";
        public string curpageid = "";
        public bool cloneAndReplace = false;
        public bool cloneAndNew = false;
        public TextBox textBox1;
        public IHTMLElement curSelectPartEle = null;
        public string clonePartName = null;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NewControl()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
		System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl),this);
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox1.Location = new System.Drawing.Point(120, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(331, 26);
            this.textBox1.TabIndex = 0;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(376, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cancel";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(279, 72);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 30);
            this.button2.TabIndex = 2;
            this.button2.Text = "OK";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(40, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "新实例名称：";
            // 
            // NewControl
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(506, 118);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewControl";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Control";
            this.Load += new System.EventHandler(this.NewControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(textBox1.Text.Trim().Equals(""))
				{
					MsgBox.Warning("caption can not empty!");
					return;
				}
				string newcaption=textBox1.Text.Trim();
				if(addType.Equals("new"))
				{
					if(!PageWare.AddControl(newcaption,controlName,false))
					{
						MsgBox.Error(res.com.str("ImportSel.insertfailed"));
					}
					else
					{
                        classes.PageAsist.RefreshToolBoxPages();
						isCancel=false;
						this.Close();
					}
				}
				if(addType.Equals("clone"))
				{
                    button2.Enabled = false;
                    string NewControlID = PageWare.CloneControl(newcaption, controlName);
                    if (NewControlID == null)
					{
						MsgBox.Error(res.com.str("ImportSel.kelongfailed"));
					}
					else
					{
                        if (cloneAndReplace)
                        {
                            string newpartid = null;
                            int loopi=0;
                            string sql = null;
                            while (newpartid == null && loopi < 10)
                            {
                                System.Threading.Thread.Sleep(200);
                                loopi++;
                                sql = "select id from parts where controlid='" + NewControlID + "' and name='" + clonePartName + "'";
                                newpartid = consts.globalConst.CurSite.SiteConn.GetString(sql);
                            }
                            while (newpartid == null)
                            {
                                if (MsgBox.YesNo(res.com.str("ImportSel.waid2second")).Equals(DialogResult.Yes))
                                {
                                    System.Threading.Thread.Sleep(2000);
                                    sql = "select id from parts where controlid='" + NewControlID + "' and name='" + clonePartName + "'";
                                    newpartid = consts.globalConst.CurSite.SiteConn.GetString(sql);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (newpartid != null)
                            {
                                try
                                {
                                    MsgBox.Information(res.com.str("ImportSel.waid5second"));

                                    string innerhtml = "{getPartHtml Error}";
                                    loopi = 0;
                                    while (innerhtml.Equals("{getPartHtml Error}") && loopi < 25)
                                    {
                                        System.Threading.Thread.Sleep(200);
                                        loopi++;
                                        innerhtml=PageWare.getPartHtml(newpartid);
                                    }
                                    curSelectPartEle.innerHTML = innerhtml;
                                    curSelectPartEle.setAttribute("idname", newpartid);

                                    sql = "delete from part_in_page where pageid='" + curpageid + "' and partid='" + oldpartid + "'";
                                    consts.globalConst.CurSite.SiteConn.execSql(sql);
                                    sql = "insert into part_in_page(pageid,partid)values('" + curpageid + "','" + newpartid + "')";
                                    consts.globalConst.CurSite.SiteConn.execSql(sql);

                                    Editor ed = functions.form.getEditor();
                                    if (ed != null)
                                    {
                                        classes.PageAsist.RefreshToolBoxPages();
                                        ed.editocx_onselectionchange(curSelectPartEle);
                                    }

                                    consts.globalConst.MdiForm.MainStatus.Text = "Colone and Replace Successfully！";
                                }
                                catch (Exception ex)
                                {
                                    MsgBox.Error(res.com.str("ImportSel.replacefailed"));
                                    new error(ex);
                                }
                            }
                            else
                            {
                                MsgBox.Warning(res.com.str("ImportSel.pleasemanul"));
                            }
                            //PageWare.getPartHtml();
                        }
                        else if(cloneAndNew)
                        {
                            string newpartid = null;
                            int loopi = 0;
                            string sql = null;
                            while (newpartid == null && loopi < 10)
                            {
                                System.Threading.Thread.Sleep(200);
                                loopi++;
                                sql = "select id from parts where controlid='" + NewControlID + "' and name='" + clonePartName + "'";
                                newpartid = consts.globalConst.CurSite.SiteConn.GetString(sql);
                            }
                            while (newpartid == null)
                            {
                                if (MsgBox.YesNo(res.com.str("ImportSel.waid2second")).Equals(DialogResult.Yes))
                                {
                                    System.Threading.Thread.Sleep(2000);
                                    sql = "select id from parts where controlid='" + NewControlID + "' and name='" + clonePartName + "'";
                                    newpartid = consts.globalConst.CurSite.SiteConn.GetString(sql);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (newpartid != null)
                            {
                                try
                                {
                                    string innerhtml = "{getPartHtml Error}";
                                    loopi = 0;
                                    while (innerhtml.Equals("{getPartHtml Error}") && loopi < 25)
                                    {
                                        System.Threading.Thread.Sleep(200);
                                        loopi++;
                                        innerhtml = PageWare.getPartHtml(newpartid);
                                    }
                                    string outerhtml = globalConst.PageWare.getControlEditHead(newpartid, clonePartName, "1px", "1px") + innerhtml + globalConst.PageWare.getControlEditTail();
                                    IHTMLElement pie = curSelectPartEle.parentElement;
                                    if (pie.tagName != "TD" || pie.getAttribute("tag") == null || pie.getAttribute("tag").ToString() != "FTDPPartTd")
                                    {
                                        curSelectPartEle.insertAdjacentHTML("afterEnd", Environment.NewLine+ "<br><br><br>" + Environment.NewLine+outerhtml + "<br><br>");
                                    }
                                    else
                                    {
                                        IHTMLTable it = null;
                                        int curRow = ((IHTMLTableRow)pie.parentElement).rowIndex;
                                        if (pie.parentElement.parentElement.tagName == "TBODY") it = (IHTMLTable)pie.parentElement.parentElement.parentElement;
                                        else it = (IHTMLTable)pie.parentElement.parentElement;
                                        it.insertRow(curRow+1);
                                        IHTMLTableRow newRow = ((IHTMLTableRow)it.rows.item(curRow + 1));
                                        newRow.insertCell(0);
                                        ((IHTMLElement)(newRow.cells.item(0))).innerHTML = newcaption;
                                        newRow.insertCell(1);
                                        ((IHTMLElement)(newRow.cells.item(1))).setAttribute("tag", "FTDPPartTd");
                                        ((IHTMLElement)(newRow.cells.item(1))).innerHTML = outerhtml;
                                    }

                                    sql = "insert into part_in_page(pageid,partid)values('" + curpageid + "','" + newpartid + "')";
                                    consts.globalConst.CurSite.SiteConn.execSql(sql);

                                    consts.globalConst.MdiForm.MainStatus.Text = "Colone and New Successfully！";

                                    //classes.PageAsist.RefreshToolBoxPages();
                                }
                                catch (Exception ex)
                                {
                                    MsgBox.Error(res.com.str("ImportSel.addfailed"));
                                    new error(ex);
                                }
                            }
                            else
                            {
                                MsgBox.Warning(res.com.str("ImportSel.pleasemanul2"));
                            }
                        }
						isCancel=false;
						this.Close();
					}
                    button2.Enabled = true;
				}
				
			}
			catch(Exception ex)
			{
			new error(ex);
			}
		}

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button2_Click(sender, null);
        }

        private void NewControl_Load(object sender, EventArgs e)
        {
            Editor ed = form.getEditor();
            if (ed != null)
            {
                if (textBox1.Text.IndexOf("__") < 0)
                {
                    textBox1.Text = ed.thisTitle + "__" + textBox1.Text;
                }
                else
                {
                    textBox1.Text = ed.thisTitle + textBox1.Text.Substring(textBox1.Text.IndexOf("__"));
                }
            }
            label1.Text = res.com.str("ImportSel.label1");//新实例名称：
            button2.Text = res.com.str("ImportSel.button2");//确定
            button1.Text = res.com.str("ImportSel.button1");//取消
            this.Text = res.com.str("ImportSel.newcontrol");//新组件实例
        }
	}
}
