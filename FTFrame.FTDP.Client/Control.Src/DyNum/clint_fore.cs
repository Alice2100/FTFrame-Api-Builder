/*FTDP Control,This Code Created By FTDP Deploy Tool
Build By Maobb 2007-6-10
Code Deploy Time is:2020-06-05 09:49:47*/
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
namespace ft_dynum
{
    public class client
    {
        public static string thisControlID = "";
        public static object[] thisStyleObject;
        public string ControlXML()
        {
            return "<config><control_name>Dynum</control_name><control_caption>流水号</control_caption><control_company>FTDP</control_company><control_copyright>FTDP</control_copyright><control_category>information</control_category><control_version>1.001</control_version><control_description>生成流水号并保存，一次提交可支持多个流水号片段</control_description><control_icon>icon.gif</control_icon><canpublish>no</canpublish><cancheck>no</cancheck><paras></paras><fore_parts><part name=\"Input\">前台片段</part></fore_parts><back_parts></back_parts></config>";
        }
        public string DeployTime()
        {
            return "2021-04-09 21:49:47";
        }
    }
    public class Input : Control, INamingContainer
    {
        public static string PartXML0 = "<partxml><public_params><param><name>CurPage</name><type>string</type><class>system[@CurPage]</class></param>      <param><name>Special</name><type>string</type><caption>特殊类型</caption><description>定义特殊类型。[GUID]为生成新的GUID</description><class>string</class><default></default><category>设置</category>      </param><param><name>Patten</name><type>string</type><caption>流水号格式</caption><description>定义流水号格式。[Y][M][D][h][m][s]分别表示年月日时分秒，[N(6)]表示6位长度自增数字</description><class>string</class><default>FT-[Y][M][D][h][m][s]-G[N(6)]</default><category>设置</category></param><param><name>TableName</name><type>string</type><caption>存储表格</caption><description>定义目标存储表格标识,@开头为全表名</description><class>string</class><default></default><category>设置</category></param><param><name>ColName</name><type>string</type><caption>存储列名</caption><description>定义目标表格的存储列名</description><class>string</class><default></default><category>设置</category></param><param><name>LockLike</name><type>string</type><caption>增长方式</caption><description>定义流水号增长方式，不设置则为按全部自增，设置内容为like方式，例如按每月开始从头增长，可设为FT-[Y][M]%</description><class>string</class><default>FT-[Y][M][D]%</default><category>设置</category></param>		  </public_params><styles></styles></partxml>";
        public static string PartXML1 = "";
        public static string PartXML2 = "";
        public static string PartXML3 = "";
        public static string PartXML4 = "";
        public static string PartXML5 = "";
        public static string PartXML6 = "";
        public static string PartXML7 = "";
        public static string PartXML8 = "";
        public static string PartXML9 = "";
        public static string PartXML10 = "";
        public static string PartXML11 = "";
        public static string PartXML12 = "";
        public static string PartXML13 = "";
        public static string PartXML14 = "";
        public static string PartXML15 = "";
        public static string PartXML16 = "";
        public static string PartXML17 = "";
        public static string PartXML18 = "";
        public static string PartXML19 = "";
        public static string PartXML = PartXML0 + PartXML1 + PartXML2 + PartXML3 + PartXML4 + PartXML5 + PartXML6 + PartXML7 + PartXML8 + PartXML9 + PartXML10 + PartXML11 + PartXML12 + PartXML13 + PartXML14 + PartXML15 + PartXML16 + PartXML17 + PartXML18 + PartXML19;
        public string getPartXml()
        {
            return PartXML;
        }
        public string getHtml(string theControlID, string SetStyle, string CurPage, string Special, string Patten, string TableName, string ColName, string LockLike)
        {
            client.thisControlID = theControlID;
            string[] StyleStringArray = SetStyle.Split('{');
            int StyleStringArrayi;
            int StyleStringArrayLength = StyleStringArray.Length;
            client.thisStyleObject = new object[StyleStringArrayLength];
            for (StyleStringArrayi = 0; StyleStringArrayi < StyleStringArrayLength; StyleStringArrayi++)
            {
                string[] StyleStringArrayOne = StyleStringArray[StyleStringArrayi].Split('}');
                client.thisStyleObject[StyleStringArrayi] = StyleStringArrayOne;
            }
            string src = System.AppDomain.CurrentDomain.BaseDirectory + "\\lib\\dynum.res\\" + "icon.gif";
            string s = ftdpcontroltemp.ClientView(src);
            return s;
        }
    }
}
