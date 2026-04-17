/*FTDP Control,This Code Created By FTDP Deploy Tool
Build By Maobb 2007-6-10
Code Deploy Time is:2020-06-05 09:49:47*/
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Text;
using System.Collections.Generic;

namespace ft_dataop
{
public class client
{
public static string thisControlID="";
public static object[] thisStyleObject;
public string ControlXML()
{
return "<config><control_name>Dataop</control_name><control_caption>数据操作</control_caption><control_company>FTDP</control_company><control_copyright>FTDP</control_copyright><control_category>adv</control_category><control_version>1.001</control_version><control_description>FTDP数据操作</control_description><control_icon>icon.gif</control_icon><canpublish>no</canpublish><cancheck>no</cancheck><paras></paras><fore_parts><part name=\"Interface\">前台接口片段</part></fore_parts><back_parts></back_parts></config>";
}
public string DeployTime()
{
return "2020-06-05 09:49:47";
}
}
public class Interface : Control, INamingContainer
{
public static string PartXML0="<partxml><public_params><param><name>OPType</name><type>int</type><caption>00.使用类型</caption><description>定义数据操作使用类型。数据操作：仅对数据进行增改操作；流程操作：仅对流程指定表格的默认fid数据进行流程操作；混合操作：先进行数据操作，再对被操作数据指定流程状态</description><class>enum</class><default>0</default><category>设置</category><enums><enum>数据操作</enum><enum>流程操作</enum><enum>混合操作</enum></enums></param><param><name>Define</name><type>string</type><caption>01.规则定义</caption><description><![CDATA[定义数据操作的规则]]></description><class>string</class><default></default><category>设置</category></param><param><name>NewSiteID</name><type>string</type><caption>12.站点标识</caption><description><![CDATA[默认为当前站点]]></description><class>string</class><default></default><category>设置</category></param><param><name>JSBefore</name><type>string</type><caption>02.脚本：操作前</caption><description>定义操作前脚本，支持@语法。</description><class>string</class><default></default><category>设置</category></param><param><name>JSSuccess</name><type>string</type><caption>03.脚本：操作成功后</caption><description>定义操作成功后脚本，支持@语法。</description><class>string</class><default></default><category>设置</category></param><param><name>CodeBefore</name><type>string</type><caption>04.代码：数据操作前</caption><description>@code代码接口，支持@语法和js变量{para}，多个代码以#隔开。</description><class>string</class><default></default><category>设置</category></param><param><name>CodeAfter</name><type>string</type><caption>05.代码：数据操作后</caption><description>@code代码接口，支持@语法和js变量{para}，@newfid@或_newfid_表示新纪录的第一个fid，多个代码以#隔开。</description><class>string</class><default></default><category>设置</category></param><param><name>OPContidionSql</name><type>string</type><caption>08.操作前提：Sql</caption><description>Sql语句，返回0为通过，其他为不通过且执行操作前提js，支持@语法和js变量{para}。为空为不设置</description><class>string</class><default></default><category>设置</category></param><param><name>OPC";
public static string PartXML1="ontidionCode</name><type>string</type><caption>09.操作前提：代码</caption><description>@code代码接口，返回0或null为通过，返回1且执行操作前提js，其他为不通过且显示返回值js提示，支持@语法和js变量{para}。为空为不设置</description><class>string</class><default></default><category>设置</category></param><param><name>OPContidionJs</name><type>string</type><caption>10.操作前提：js执行</caption><description>前提条件不满足时执行的js，支持@语法</description><class>string</class><default>parent._loading2fai(\"验证不通过\",null)</default><category>设置</category></param><param><name>BeforeSql</name><type>string</type><caption>06.Sql执行：操作前</caption><description>数据操作前执行的Sql语句，支持@语法和js变量{para}。为空为不设置，多条语句以##隔开。</description><class>string</class><default></default><category>设置</category></param><param><name>AfterSql</name><type>string</type><caption>07.Sql执行：操作后</caption><description>数据操作后执行的Sql语句，支持@语法和js变量{para}，@newfid@表示新纪录的第一个的fid。为空为不设置，多条语句以##隔开。</description><class>string</class><default></default><category>设置</category></param><param><name>OPID</name><type>string</type><caption>13.权限标识</caption><description>定义权限标识，通过标识和接口确定当前用户是否有该操作的权限。</description><class>string</class><default></default><category>设置</category></param><param><name>CheckRule</name><type>string</type><caption>11.数据验证</caption><description>定义数据验证JS。支持@语法</description><class>string</class><default></default><category>设置</category></param><param><name>DefaultFID</name><type>string</type><caption>14.默认fid</caption><description><![CDATA[{para}为js代码,支持@语法，应用于如下场景：当修改的元素没有指定fid时、自定义字段修改时、流程操作时]]></description><class>string</class><default></default><category>设置</category></param><param><name>IsMultiMod</name><type>int</type><caption>17.多行更新</caption><description>开启多行更新后，直接更新多行数据，只能操作一张数据表,对设置条件内的数据若fid不存在则删除，若fid存在则更新，若fid为空则为新插入，开启后动态新增行失效</description><class>enum</class><default>0</default><category>设置<";
public static string PartXML2="/category><enums><enum>否</enum><enum>是</enum></enums></param><param><name>MultiFidName</name><type>string</type><caption>18.多行更新存储fid的元素名</caption><description><![CDATA[必须指定，且该元素名存在，否则忽略多行操作]]></description><class>string</class><default></default><category>设置</category></param><param><name>MultiCondition</name><type>string</type><caption>19.多行更新筛选条件</caption><description><![CDATA[不指定可能面临删除不必要的记录，支持@和{para}语法]]></description><class>string</class><default></default><category>设置</category></param><param><name>CloseButton</name><type>string</type><caption>关闭按钮事件</caption><description>定义关闭按钮JS事件。</description><class>string</class><default></default><category>显示</category></param><param><name>CloseButtonCap</name><type>string</type><caption>关闭按钮文字</caption><description>定义关闭按钮文字，为空为不显示关闭按钮。</description><class>string</class><default>取消</default><category>显示</category></param><param><name>ButtonCap</name><type>string</type><caption>操作按钮文字</caption><description>定义操作按钮文字。</description><class>string</class><default>提交</default><category>显示</category></param><param><name>ButtonImg</name><type>string</type><caption>操作按钮图标</caption><description>定义操作按钮文字。</description><class>string</class><default>ui-icon-check</default><category>显示</category></param><param><name>FlowType</name><type>int</type><caption>0.状态类型</caption><description>定义状态类型</description><class>enum</class><default>0</default><category>流程</category><enums><enum>固定状态</enum><enum>退回上一步</enum><enum>绑定流程设计</enum></enums></param><param><name>Tabletag</name><type>string</type><caption>1.绑定数据表</caption><description>设置该流程绑定的数据表标识，@开头为全表名，为空或输入错误将导致该流程操作不生效</description><class>string</class><default></default><category>流程</category></param><param><name>FlowStat</name><type>int</type><caption>2.固定状态值</caption><description>定义该动作发生后，";
public static string PartXML3= "表单的状态值。默认新增和修改动作会使该表单状态值置为0</description><class>int</class><default>0</default><category>流程</category></param><param><name>FlowDesign</name><type>string</type><caption>3.流程设计构件</caption><description>由绑定的流程设计构件指定操作后的行为</description><class>string</class><default></default><category>流程</category></param><param><name>FlowDesignBaranch</name><type>string</type><caption>4.固定分支</caption><description>绑定流程构件时，若下一步有分支流程，且当分支流程可能同时满足流转条件时，需设定固定分支流程ID值，该值为流程设计构件的分支箭头的ID。</description><class>string</class><default></default><category>流程</category></param><param><name>FlowDesignPos</name><type>string</type><caption>5.固定状态值位置</caption><description>在实现流程构件与固定状态值混用时，若当前类型为固定状态值，则需设定该固定状态值在流程设计中所对应的位置，即流程设计构件的对象的ID。</description><class>string</class><default></default><category>流程</category></param><param><name>APISet</name><type>string</type><caption>20.API设置</caption><description>定义数据操作API时的设置</description><class>string</class><default></default><category>设置</category></param><param><name>FidCol</name><type>string</type><caption>15.主键列名</caption><description>定义数据操作主键列名,为空时根据Project项目的DBSuit定义</description><class>string</class><default></default><category>设置</category></param><param><name>OpDefaultCol</name><type>int</type><caption>16.更新保留列</caption><description>配置是否自动更新保留列：保留列在Project项目的DBSuit定义</description><class>enum</class><default>1</default><category>设置</category><enums><enum>否</enum><enum>是</enum></enums></param></public_params><styles></styles></partxml>";
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
public string getHtml(string theControlID,string SetStyle,int OPType,string Define,string NewSiteID,string JSBefore,string JSSuccess,string CodeBefore,string CodeAfter,string OPContidionSql,string OPContidionCode,string OPContidionJs,string BeforeSql,string AfterSql,string OPID,string CheckRule,string DefaultFID,int IsMultiMod,string MultiFidName,string MultiCondition,string CloseButton,string CloseButtonCap,string ButtonCap,string ButtonImg,int FlowType,string Tabletag,int FlowStat,string FlowDesign,string FlowDesignBaranch,string FlowDesignPos,string APISet,string FidCol,int OpDefaultCol,string CustomConnection)
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
            var rowCount = Define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries).Length;
            string src=System.AppDomain.CurrentDomain.BaseDirectory + "\\lib\\dataop.res\\" + "redflag.gif";
            s += "<span class='parthead'>";
 s +=ftdpcontroltemp.ClientView(src,ButtonCap,rowCount);

        if(!CloseButtonCap.Equals(""))
        {
          s+=ftdpcontroltemp.ClientView2(CloseButtonCap);
        }
            s += "</span>";
            List<string[]> list = new List<string[]>();
            string[] items2 = Define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items2)
            {
                //cap#name#col#01#noempty#fid#id
                string[] row = item.Split(new string[] { "##" }, StringSplitOptions.None);
                list.Add(new string[] { row[0], row[1] , func.getDecode(row[2]), func.LeiXingText(func.getDecode(row[3])), row[5] });
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<TABLE cellSpacing=0 cellPadding=0 width=\"100%\">");
            sb.Append("<TBODY>");
            sb.Append("<TR class=ftlisttr1><TD class=list_b_rb align=left style=\"WHITE-SPACE: nowrap;background-color:#fff2ec;width:60px\"><FONT style=\"WHITE-SPACE: nowrap\">"+ftdpcontroltemp.TextShow.MingCheng+"</font></TD>");
            foreach (var item in list)
            {
                sb.Append("<TD class=list_b_rb align=left>" + item[0] +"</TD>");
            }
            sb.Append("</TR>");
            sb.Append("<TR class=ftlisttr1><TD class=list_b_rb align=left style=\"WHITE-SPACE: nowrap;background-color:#fff2ec;width:60px\"><FONT style=\"WHITE-SPACE: nowrap\">Key</font></TD>");
            foreach (var item in list)
            {
                sb.Append("<TD class=list_b_rb align=left>" + item[1] + "</TD>");
            }
            sb.Append("</TR>");
            sb.Append("<TR class=ftlisttr1><TD class=list_b_rb align=left style=\"WHITE-SPACE: nowrap;background-color:#fff2ec;width:60px\"><FONT style=\"WHITE-SPACE: nowrap\">"+ftdpcontroltemp.TextShow.BangDingZiDuan+"</font></TD>");
            foreach (var item in list)
            {
                sb.Append("<TD class=list_b_rb align=left>" + item[2] + "</TD>");
            }
            sb.Append("</TR>");
            sb.Append("<TR class=ftlisttr1><TD class=list_b_rb align=left style=\"WHITE-SPACE: nowrap;background-color:#fff2ec;width:60px\"><FONT style=\"WHITE-SPACE: nowrap\">"+ftdpcontroltemp.TextShow.LeiXing+"</font></TD>");
            foreach (var item in list)
            {
                sb.Append("<TD class=list_b_rb align=left>" + item[3] + "</TD>");
            }
            sb.Append("</TR>");
            sb.Append("<TR class=ftlisttr1><TD class=list_b_rb align=left style=\"WHITE-SPACE: nowrap;background-color:#fff2ec;width:60px\"><FONT style=\"WHITE-SPACE: nowrap\">"+ftdpcontroltemp.TextShow.FilterRule+"</font></TD>");
            foreach (var item in list)
            {
                sb.Append("<TD class=list_b_rb align=left>" + item[4] + "</TD>");
            }
            sb.Append("</TR>");sb.Append("</TBODY></TABLE>");
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
