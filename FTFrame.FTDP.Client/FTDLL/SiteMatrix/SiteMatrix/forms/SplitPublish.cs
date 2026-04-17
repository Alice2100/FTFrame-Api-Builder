using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SiteMatrix.functions;
using SiteMatrix.consts;
using System.Text.RegularExpressions;
using System.Collections;
namespace SiteMatrix.forms
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
            dialog.Description = "请选择文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MsgBox.Warning("文件夹路径不能为空");
                    return;
                }
                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "请选择文件";
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
            if (globalConst.CurSite.ID == null)
            {
                MsgBox.Error("必须要先打开站点");
                this.Close();
                return;
            }

           textBox1.Text= Options.GetSystemValue("splitrootpath");
           textBox2.Text = Options.GetSystemValue("splitdirpath");
           label4.Text = LastSplitFile==null?"":("最近发布："+LastSplitFile);
           label6.Text = "";
           textBox3.Enabled = false;
           checkBox1.Checked = true;
        }
        private void Publish(string filename)
        {
            string rootpath = textBox1.Text.Trim().ToLower();
            string dirpath = textBox2.Text.Trim().ToLower();
            if (rootpath == "")
            {
                MsgBox.Error("请先指定本地根目录");
                return;
            }
            if (!dirpath.EndsWith("/"))
            {
                MsgBox.Error("请先指定站点根路径，以 / 结尾，顶级路径请输入  / ");
                return;
            }
            int index = filename.ToLower().IndexOf(rootpath);
            if (index != 0)
            {
                MsgBox.Error("文件必须在本地根目录下面");
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
            sp.ShowDialog();


            LastSplitFile = filename;
            label4.Text = "最近发布：" + LastSplitFile;
        }
        private bool Validate(string filename)
        {
            int left = 0; int right = 0;
            string filetext = file.getFileText(filename, System.Text.Encoding.UTF8);
            Regex r = new Regex(@"(<dotforsite:control[^>]*></dotforsite:control>)|(<ftdp:control[^>]*></ftdp:control>)");
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
                    MsgBox.Error("片段实例出现重复，请检查！" + iid);
                    return false;
                }
                al.Add(iid);
                if (globalConst.CurSite.SiteConn.GetInt32(sql) == 0)
                {
                    MsgBox.Error("片段实例在当前站点中不存在！" + iid);
                    return false;
                }
            }
            if (al.Count == 0)
            {
                MsgBox.Information("未发现任何片段实例");
            }
            return true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (LastSplitFile == null)
            {
                MsgBox.Error("没有最近发布分离文件的记录");
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
                MsgBox.Error("请先指定服务器静态文件");
                return;
            }
            if (serverfile.IndexOf(':') < 0 || serverfile.IndexOf(@"\") < 0)
            {
                MsgBox.Error("请先指定服务器静态文件，为磁盘绝对路径");
                return;
            }
            string dirpath = textBox2.Text.Trim().ToLower();

            if (!dirpath.EndsWith("/"))
            {
                MsgBox.Error("请先指定站点根路径，以 / 结尾，顶级路径请输入  / ");
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
            label4.Text = "最近发布：" + LastSplitFile;
        }
    }
}
