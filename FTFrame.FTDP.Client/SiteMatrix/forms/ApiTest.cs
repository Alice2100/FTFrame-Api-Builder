using FTDPClient.consts;
using FTDPClient.functions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTDPClient.forms
{
    public partial class ApiTest : Form
    {
        public string FId;
        public string ApiType;
        public string PostManJson;
        public string KeyDesc;
        public string apipath;
        public ApiTest()
        {
            InitializeComponent();
        }

        private void ApiTest_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void ApiTest_Load(object sender, EventArgs e)
        {
            label1.Text = ApiType == "DyValue" ? "Get" : "Post";
            textBox1.Text = apipath;
            textBox4.Text = "Authorization:ftdp;token:ftdp";
            if(ApiType != "DyValue")
            {
                var jo = Newtonsoft.Json.Linq.JObject.Parse(PostManJson);
                var inputText = jo["item"][0]["request"]["body"]["raw"].ToString();
                textBox2.Text = inputText;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                button3.Enabled = false;
                var headerDic = new Dictionary<string, string>();
                var hs1 = textBox4.Text.Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach(var item in hs1)
                {
                    var hs2 = item.Split(new char[] { ':' }, StringSplitOptions.None);
                    if(hs2.Length>=2)
                    {
                        headerDic.Add(hs2[0].Trim(), hs2[1].Trim());
                    }
                }
                string ret = "{}";
                if (ApiType == "DyValue")
                {
                    ret = functions.net.HttpGet(textBox1.Text.Trim(), headerDic);
                }
                else
                {
                    ret = functions.net.HttpPost(textBox1.Text.Trim(), textBox2.Text, headerDic);
                }
                textBox3.Text = ret;
                try
                {
                    JsonSerializer serializer = new JsonSerializer();
                    TextReader tr = new StringReader(textBox3.Text);
                    JsonTextReader jtr = new JsonTextReader(tr);
                    object obj = serializer.Deserialize(jtr);
                    if (obj != null)
                    {
                        StringWriter textWriter = new StringWriter();
                        JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                        {
                            Formatting = Formatting.Indented,
                            Indentation = 4,//缩进字符数
                            IndentChar = ' '//缩进字符
                        };
                        serializer.Serialize(jsonWriter, obj);
                        textBox3.Text = textWriter.ToString();
                    }

                }
                catch (Exception ex)
                {
                    MsgBox.Error(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MsgBox.Error(ex.Message);
            }
            finally
            {
                button3.Enabled = true;
            }
        }
    }
}
