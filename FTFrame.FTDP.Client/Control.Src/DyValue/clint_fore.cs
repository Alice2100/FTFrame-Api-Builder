/*FTDP Control,This Code Created By FTDP Deploy Tool
Build By Maobb 2007-6-10
Code Deploy Time is:2020-06-05 09:55:17*/
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ft_dyvalue
{
public class client
{
public static string thisControlID="";
public static object[] thisStyleObject;
public string ControlXML()
{
return "<config><control_name>Dyvalue</control_name><control_caption>数据获取</control_caption><control_company>FTDP</control_company><control_copyright>FTDP</control_copyright><control_category>adv</control_category><control_version>1.001</control_version><control_description>FTDP数据获取</control_description><control_icon>icon.gif</control_icon><canpublish>no</canpublish><cancheck>no</cancheck><paras></paras><fore_parts><part name=\"Interface\">前台接口片段</part></fore_parts><back_parts></back_parts></config>";
}
public string DeployTime()
{
return "2020-06-05 09:55:17";
}
}
public class Interface : Control, INamingContainer
{
public static string PartXML0= "<partxml><public_params><param><name>Define</name><type>string</type><caption>0.规则定义</caption><description><![CDATA[定义数据取值的规则]]></description><class>string</class><default></default><category>设置</category></param><param><name>FuncName</name><type>string</type><caption>1.异步取值方法</caption><description><![CDATA[定义异步取值方法，同一个页面不能重复，可使用1到12个参数，分别与fid和自定义sql的@p1@到@p2@对应]]></description><class>string</class><default>getvalue_0</default><category>设置</category></param><param><name>AppendJS0</name><type>string</type><caption>2.同步取值前脚本</caption><description>定义取值之前执行的脚本，支持@语法。</description><class>string</class><default></default><category>设置</category></param><param><name>AppendJS1</name><type>string</type><caption>3.同步取值后脚本</caption><description>定义取值之后执行的脚本，支持@语法。</description><class>string</class><default></default><category>设置</category></param><param><name>NewSiteID</name><type>string</type><caption>9.站点标识</caption><description><![CDATA[默认为当前站点]]></description><class>string</class><default></default><category>设置</category></param><param><name>YBAppendJS0</name><type>string</type><caption>4.异步取值前脚本</caption><description>定义取值之前执行的脚本，支持@语法和@p1@到@p12@变量。</description><class>string</class><default>newDialog('_ybloading', 'loading', '正在载入...',false)</default><category>设置</category></param><param><name>YBAppendJS1</name><type>string</type><caption>5.异步取值后脚本</caption><description>定义取值之后执行的脚本，支持@语法和@p1@到@p12@变量。</description><class>string</class><default>closeDialog('_ybloading')</default><category>设置</category></param><param><name>DefaultFID</name><type>string</type><caption>6.默认主键值</caption><description><![CDATA[{para}为js代码,支持@语法和@p1@到@p12@，应用于如下场景：当非自定义SQL时且元素没有指定fid时]]></description><class>string</class><default></default><category>设置</category></param><param><name>APISet</name><type>string</type><caption>10.API设置</caption><description>定义数据获取输出API时的设置</description><class>string</class><default></default><category>设置</category></param><param><name>FidCol</name><type>string</type><caption>7.主键列名</caption><description>定义数据获取主键列名,为空时根据Project项目的DBSuit定义</description><class>string</class><default></default><category>设置</category></param><param><name>ExecBefore</name><type>string</type><caption>11.获取前执行</caption><description></description><class>string</class><default /><category>设置</category></param><param><name>ExecAfter</name><type>string</type><caption>12.获取后执行</caption><description></description><class>string</class><default /><category>设置</category></param><param><name>OpDefaultCol</name><type>int</type><caption>8.存在保留列</caption><description>配置是否存在保留列：保留列在Project项目的DBSuit定义</description><class>enum</class><default>1</default><category>设置</category><enums><enum>否</enum><enum>是</enum></enums></param></public_params><styles></styles";
public static string PartXML1="></partxml>";
public static string PartXML2="";
public static string PartXML3="";
public static string PartXML4="";
public static string PartXML5="";
public static string PartXML6="";
public static string PartXML7="";
public static string PartXML8="";
public static string PartXML9="";
public static string PartXML10="";
public static string PartXML11="";
public static string PartXML12="";
public static string PartXML13="";
public static string PartXML14="";
public static string PartXML15="";
public static string PartXML16="";
public static string PartXML17="";
public static string PartXML18="";
public static string PartXML19="";
public static string PartXML=PartXML0+PartXML1+PartXML2+PartXML3+PartXML4+PartXML5+PartXML6+PartXML7+PartXML8+PartXML9+PartXML10+PartXML11+PartXML12+PartXML13+PartXML14+PartXML15+PartXML16+PartXML17+PartXML18+PartXML19;
public string getPartXml()
{
return PartXML;
}
public string getHtml(string theControlID,string SetStyle,string Define,string FuncName,string AppendJS0,string AppendJS1,string NewSiteID,string YBAppendJS0,string YBAppendJS1,string DefaultFID,string APISet,string FidCol,int OpDefaultCol,string ExecBefore,string ExecAfter,string CustomConnection)
{
client.thisControlID=theControlID;
string[] StyleStringArray=SetStyle.Split('{');
int StyleStringArrayi;
int StyleStringArrayLength=StyleStringArray.Length;
client.thisStyleObject=new object[StyleStringArrayLength];
for(StyleStringArrayi=0;StyleStringArrayi<StyleStringArrayLength;StyleStringArrayi++)
{
	string[] StyleStringArrayOne=StyleStringArray[StyleStringArrayi].Split('}');
	client.thisStyleObject[StyleStringArrayi]=StyleStringArrayOne;
}

        string s = "";
            int tongbuc = 0;
int yibuc = 0;
string[] rows = Define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
for (int i = 0; i < rows.Length; i++)
{
    string[] item = rows[i].Split(new string[] { "##" }, StringSplitOptions.None);
    if (int.Parse(item[4]) == 0) tongbuc++;
    else if (int.Parse(item[4]) == 1) yibuc++;
}
            s += "<span class='parthead'>";
if (yibuc > 0)
{
    s += "&nbsp;<img border=0 src=\""+System.AppDomain.CurrentDomain.BaseDirectory + "\\lib\\dyvalue.res\\blueflag.gif\"/>&nbsp;" + yibuc;
}
if (tongbuc > 0)
{
    s += "&nbsp;<img border=0 src=\""+System.AppDomain.CurrentDomain.BaseDirectory + "\\lib\\dyvalue.res\\greenflag.gif\"/>&nbsp;" + tongbuc;
}
if (s.Equals(""))
{
    s = "&nbsp;<img border=0 src=\""+System.AppDomain.CurrentDomain.BaseDirectory + "\\lib\\dyvalue.res\\yellowflag.png\"/>&nbsp;";
}
            s += "</span>";
            List<string[]> list = new List<string[]>();
            string[] items2 = Define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items2)
            {
                string[] row = item.Split(new string[] { "##" }, StringSplitOptions.None);
                list.Add(new string[] { row[0], row[1], func.getDecode(row[2]), (row[5].Equals("0")&& row[6].Equals("0"))?ftdpcontroltemp.TextShow.Zhi: ftdpcontroltemp.TextShow.ShuZu, row[3] });
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<TABLE cellSpacing=0 cellPadding=0 width=\"100%\">");
            sb.Append("<TBODY>");
            sb.Append("<TR class=ftlisttr1><TD class=list_b_rb align=left style=\"WHITE-SPACE: nowrap;background-color:#e3f1ff;width:60px\"><FONT style=\"WHITE-SPACE: nowrap\">"+ ftdpcontroltemp.TextShow.MingCheng+ "</font></TD>");
            foreach (var item in list)
            {
                sb.Append("<TD class=list_b_rb align=left>" + item[0] + "</TD>");
            }
            sb.Append("</TR>");
            sb.Append("<TR class=ftlisttr1><TD class=list_b_rb align=left style=\"WHITE-SPACE: nowrap;background-color:#e3f1ff;width:60px\"><FONT style=\"WHITE-SPACE: nowrap\">Key</font></TD>");
            foreach (var item in list)
            {
                sb.Append("<TD class=list_b_rb align=left>" + item[1] + "</TD>");
            }
            sb.Append("</TR>");
            sb.Append("<TR class=ftlisttr1><TD class=list_b_rb align=left style=\"WHITE-SPACE: nowrap;background-color:#e3f1ff;width:60px\"><FONT style=\"WHITE-SPACE: nowrap\">"+ ftdpcontroltemp.TextShow.BangDingZiDuan+ "</font></TD>");
            foreach (var item in list)
            {
                sb.Append("<TD class=list_b_rb align=left>" + item[2] + "</TD>");
            }
            sb.Append("</TR>");
            sb.Append("<TR class=ftlisttr1><TD class=list_b_rb align=left style=\"WHITE-SPACE: nowrap;background-color:#e3f1ff;width:60px\"><FONT style=\"WHITE-SPACE: nowrap\">"+ ftdpcontroltemp.TextShow.LeiXing+ "</font></TD>");
            foreach (var item in list)
            {
                sb.Append("<TD class=list_b_rb align=left>" + item[3] + "</TD>");
            }
            sb.Append("</TR>");
            sb.Append("<TR class=ftlisttr1><TD class=list_b_rb align=left style=\"WHITE-SPACE: nowrap;background-color:#e3f1ff;width:60px\"><FONT style=\"WHITE-SPACE: nowrap\">"+ ftdpcontroltemp.TextShow.FilterRule+ "</font></TD>");
            foreach (var item in list)
            {
                sb.Append("<TD class=list_b_rb align=left>" + item[4] + "</TD>");
            }
            sb.Append("</TR>"); sb.Append("</TBODY></TABLE>");
            s += sb.ToString();
            if (!string.IsNullOrWhiteSpace(APISet))
            {
                s += "<table class='apitable' width='100%' cellpadding=0 cellspacing=0>";
                string[] items = APISet.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in items)
                {
                    string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                    s += "<tr><td style='color:#cc3366;padding:4px'>" + colcfg[0] + "</td><td style='color:#cc3366;padding:4px'>" + colcfg[1] + "</td></tr>";
                }
                s += "</table>";
            }

            return s;
        
}
}
}
