using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FTDPClient.functions;
using FTDPClient.consts;
using System.Text.RegularExpressions;
using System.Collections;
namespace FTDPClient.forms
{
    public partial class SplitPublish : Form
    {
        public SplitPublish()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = res.com.str("SplitPublish.selwenjianjia");
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MsgBox.Warning(res.com.str("SplitPublish.wenjianjabunengweik"));
                    return;
                }
                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = res.com.str("SplitPublish.plaseselfile");
            dialog.Filter = "html(*.html,*.htm)|*.html;*.htm";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string file = dialog.FileName;
                if (file != null && file != "")
                {
                    Publish(file);
                }
            }
        }
        private static string LastSplitFile = null;
        private void SplitPublish_Load(object sender, EventArgs e)
        {
            this.Text = res.com.str("SplitPublish.Text");           //分离发布
            label1.Text = res.com.str("SplitPublish.label1");           //本地根目录
            button1.Text = res.com.str("SplitPublish.button1");         //浏 览
            label2.Text = res.com.str("SplitPublish.label2");           //站点根路径
            checkBox1.Text = res.com.str("SplitPublish.checkBox1");         //发布后文件名跟当前相同
            label3.Text = res.com.str("SplitPublish.label3");           //注1：分离发布文件的本地相对于根目录的路径会映射在站点根路径。例如根目录为D:\abc，文件为D:\abc\x\1.html，站点根路径为main/则文件会被发布成main/x/1.aspx
            label5.Text = res.com.str("SplitPublish.label5");           //注2：分离文件中的实例片段示例：<ftdp:control id=abc_part name="List" height="1px" width="100%"></ftdp:control>。可右键复制片段标签获得。若要仅在发布时删除html元素，请在html元素加属性pubdel=true，则在发布时自动删除静态元素。
            button2.Text = res.com.str("SplitPublish.button2");         //分离发布最近一次
            label6.Text = res.com.str("SplitPublish.label6");           //最近一次分离发布
            label4.Text = res.com.str("SplitPublish.label4");           //最近一次分离发布
            label7.Text = res.com.str("SplitPublish.label7");           //前端框架
            button3.Text = res.com.str("SplitPublish.button3");         //选择本地文件发布
            button5.Text = res.com.str("SplitPublish.button5");         //服务器静态文件发布
            button4.Text = res.com.str("SplitPublish.button4");


            if (globalConst.CurSite.ID == null)
            {
                MsgBox.Error(res.com.str("SplitPublish.opensitefirst"));
                this.Close();
                return;
            }

           textBox1.Text= Options.GetSystemValue("splitrootpath");
           textBox2.Text = Options.GetSystemValue("splitdirpath");
           label4.Text = LastSplitFile==null?"":(res.com.str("SplitPublish.lastpublished") + LastSplitFile);
           label6.Text = "";
           textBox3.Enabled = false;
           checkBox1.Checked = true;

            comboBox1.Items.Clear();
            comboBox1.Items.Add(ForeFrameType.JQueryUI.ToString());
            comboBox1.Items.Add(ForeFrameType.LayUI.ToString());
            comboBox1.SelectedIndex = 0;
        }
        private void Publish(string filename)
        {
            string rootpath = textBox1.Text.Trim().ToLower();
            string dirpath = textBox2.Text.Trim().ToLower();
            if (rootpath == "")
            {
                MsgBox.Error(res.com.str("SplitPublish.zhidingmulufirst"));
                return;
            }
            if (!dirpath.EndsWith("/"))
            {
                MsgBox.Error(res.com.str("SplitPublish.zhidingmulufirst22"));
                return;
            }
            int index = filename.ToLower().IndexOf(rootpath);
            if (index != 0)
            {
                MsgBox.Error(res.com.str("SplitPublish.wenjianzaijx"));
                return;
            }
            Options.SetSystemValue("splitrootpath", rootpath);
            Options.SetSystemValue("splitdirpath", dirpath);

            if (!Validate(filename)) return;

            string subdir = filename.ToLower().Substring(rootpath.Length); 
            string subname = dirpath + "/" + subdir; 
            subname = subname.Replace(@"\", "/").Replace("//", ""); 
            subdir = "/";
            if (subname.LastIndexOf('/') > 0)
                subdir = subname.Substring(0, subname.LastIndexOf('/') + 1); 
            if (!checkBox1.Checked && textBox3.Text.Trim() != "")
            {
                subname = subdir + "/" + textBox3.Text.Trim().ToLower();
            }

            SitePublish sp = new SitePublish();
            sp.PublishForSplit = true;
            sp.PublishSplitDir = subdir;
            sp.PublishSplitFile = filename;
            sp.PublishSplitNewSubName = subname;
            sp.PublishSplitForeFrame = (ForeFrameType)Enum.Parse(typeof(ForeFrameType), comboBox1.SelectedItem.ToString());
            sp.ShowDialog();


            LastSplitFile = filename;
            label4.Text = res.com.str("SplitPublish.lastpublished") + LastSplitFile;
        }
        private bool Validate(string filename)
        {
            int left = 0; int right = 0;
            string filetext = file.getFileText(filename, System.Text.Encoding.UTF8);
            Regex r = new Regex(@"(<ftframe:control[^>]*></ftframe:control>)|(<ftdp:control[^>]*></ftdp:control>)");
            MatchCollection mc = r.Matches(filetext);
            ArrayList al = new ArrayList();
            foreach (Match m in mc)
            {
                string ControlNode = m.Value;
                left = ControlNode.IndexOf(" id=");
                right = ControlNode.IndexOf(" ", left + 1);
                string iid = ControlNode.Substring(left + 4, right - left - 4).Replace("\"", "");
                left = ControlNode.IndexOf(" name=");
                right = ControlNode.IndexOf(" ", left + 1);
                string iname = ControlNode.Substring(left + 6, right - left - 6).Replace("\"", "");
                string sql = "select count(*) from parts where id='"+iid+"' and name='"+iname+"'";
                if (al.Contains(iid))
                {
                    MsgBox.Error(res.com.str("SplitPublish.chongfujianca") + iid);
                    return false;
                }
                al.Add(iid);
                if (globalConst.CurSite.SiteConn.GetInt32(sql) == 0)
                {
                    MsgBox.Error(res.com.str("SplitPublish.pianduanbuunzai") + iid);
                    return false;
                }
            }
            if (al.Count == 0)
            {
                MsgBox.Information(res.com.str("SplitPublish.weifaxianshili"));
            }
            return true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (LastSplitFile == null)
            {
                MsgBox.Error(res.com.str("SplitPublish.meiyouzujinjili"));
            }
            else
            {
                if (LastSplitFile.StartsWith("Server:"))
                {
                    publish_server(LastSplitFile.Substring("Server:".Length));
                }
                else
                {
                    Publish(LastSplitFile);
                }
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            textBox3.Enabled = !checkBox1.Checked;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            publish_server(textBox4.Text.Trim());
        }
        private void publish_server(string serverfile)
        {
            if (serverfile == "")
            {
                MsgBox.Error(res.com.str("SplitPublish.qingxianzhidingjt"));
                return;
            }
            if (serverfile.IndexOf(':') < 0 || serverfile.IndexOf(@"\") < 0)
            {
                MsgBox.Error(res.com.str("SplitPublish.qingxianzhidingjt2"));
                return;
            }
            string dirpath = textBox2.Text.Trim().ToLower();

            if (!dirpath.EndsWith("/"))
            {
                MsgBox.Error(res.com.str("SplitPublish.qingxianzhidingjt23"));
                return;
            }
            Options.SetSystemValue("splitdirpath", dirpath);

            string subname = dirpath;
            if (!checkBox1.Checked && textBox3.Text.Trim() != "")
            {
                subname += textBox3.Text.Trim().ToLower();
            }
            else
            {
                subname += serverfile.Substring(serverfile.LastIndexOf('\\') + 1, serverfile.LastIndexOf('.') - serverfile.LastIndexOf('\\'));
            }


            SitePublish sp = new SitePublish();
            sp.PublishForSplit = true;
            sp.PublishSplitDir = "";
            sp.PublishSplitFile = "Server:" + serverfile;
            sp.PublishSplitNewSubName = subname;
            sp.ShowDialog();


            LastSplitFile = "Server:" + serverfile;
            label4.Text = res.com.str("SplitPublish.lastpublished") + LastSplitFile;
        }
    }
}
