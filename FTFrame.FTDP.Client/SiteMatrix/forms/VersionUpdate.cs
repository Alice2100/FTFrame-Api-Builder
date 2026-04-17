using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FTDPClient.functions;
using FTDPClient.consts;
using System.IO;
using System.Xml;
using FTDPClient.Compression;

namespace FTDPClient.forms
{
	public class VersionUpdate : System.Windows.Forms.Form
	{
		public bool AddSuc=false;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button2;
        private ProgressBar progressBar1;

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;

		public VersionUpdate()
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
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(32, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "            ";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(540, 89);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 30);
            this.button2.TabIndex = 9;
            this.button2.Text = "Close";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(35, 52);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(580, 23);
            this.progressBar1.TabIndex = 10;
            // 
            // VersionUpdate
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(627, 132);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VersionUpdate";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update Product";
            this.Load += new System.EventHandler(this.AddSite_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
        private void ApplyLanguage()
        {
        }
		private void button2_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
        int version = 0;
        private void AddSite_Load(object sender, EventArgs e)
        {
            version = update.version();
            System.Timers.Timer timer1 = new System.Timers.Timer();
            timer1.AutoReset = false;
            timer1.Interval = 1000;
            timer1.Elapsed += (sender2, e2) =>
            {
                DownloadFile("http://www.ftframe.com/ftdp_update/" + version + ".zip", globalConst.AppPath + @"\update\" + version + ".zip", progressBar1, label1);
            };
            timer1.Enabled = true;
        }

        public void DownloadFile(string URL, string filename, System.Windows.Forms.ProgressBar prog, System.Windows.Forms.Label label1)
        {
            float percent = 0;
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
                using (System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse())
                {
                    long totalBytes = myrp.ContentLength;
                    if (prog != null)
                    {
                        prog.Maximum = (int)totalBytes;
                    }
                    using (System.IO.Stream st = myrp.GetResponseStream())
                    {
                        using (System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create))
                        {

                            long totalDownloadedByte = 0;
                            byte[] by = new byte[1024];
                            int osize = st.Read(by, 0, (int)by.Length);
                            while (osize > 0)
                            {
                                totalDownloadedByte = osize + totalDownloadedByte;
                                System.Windows.Forms.Application.DoEvents();
                                so.Write(by, 0, osize);
                                if (prog != null)
                                {
                                    prog.Value = (int)totalDownloadedByte;
                                }
                                osize = st.Read(by, 0, (int)by.Length);

                                percent = Convert.ToInt32((float)totalDownloadedByte / (float)totalBytes * 100);
                                label1.Text = res.anew.GetString("String1") +" " + percent.ToString() + " %";
                                Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                            }
                            so.Close();
                        }
                        st.Close();
                    }
                }
                if(file.Exists(filename))
                {
                    new ZipClass().UnZip(filename, globalConst.AppPath + @"\update\" + version+@"\");
                    file.Delete(filename);
                    MsgBox.Information(res.anew.GetString("String2"));
                }
                else
                {
                    MsgBox.Error(res.anew.GetString("String3"));
                }
            }
            catch (System.Net.WebException ex)
            {
                if(ex.Message.IndexOf("404")>=0)
                {
                    label1.Text = res.anew.GetString("String4").Replace("{version}",version.ToString());
                }
                else new error(ex);
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
    }
}
