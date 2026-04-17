/*FTDP Control,This Code Created By FTDP Deploy Tool
Build By Maobb 2007-6-10
Code Deploy Time is:2017-12-27 15:58:14*/
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
namespace ft_list
{
public class client
{
public static string thisControlID="";
public static object[] thisStyleObject;
public string ControlXML()
{
return "<config><control_name>List</control_name><control_caption>数据列表</control_caption><control_company>FTDP</control_company><control_copyright>FTDP</control_copyright><control_category>data</control_category><control_version>1.001</control_version><control_description>数据列表展示构件</control_description><control_icon>icon.gif</control_icon><canpublish>no</canpublish><cancheck>no</cancheck><!--if yes then switch canpublish or cancheck--><paras></paras><fore_parts><part name=\"List\">数据列表</part></fore_parts><back_parts></back_parts></config>";
}
public string DeployTime()
{
return "2017-12-27 15:58:14";
}
}
public class List : Control, INamingContainer
{
public static string PartXML0="<partxml><public_params><param><name>CurPage</name><type>string</type><class>system[@CurPage]</class></param><param><name>SelectType</name><type>int</type><caption>选框方式</caption><description>定义列表的选框方式</description><class>enum</class><default>0</default><category>7.显示</category><enums><enum>多选框</enum><enum>单选框</enum><enum>无</enum></enums></param><param><name>LoadingImg</name><type>string</type><caption>载入时的图片</caption><description>定义载入时的图片，为空则不设置</description><class>string</class><default>/_ftres/progress.gif</default><category>7.显示</category></param><param><name>RateNumType</name><type>int</type><caption>序号</caption><description>定义序号展示方式</description><class>enum</class><default>0</default><category>7.显示</category><enums><enum>不显示</enum><enum>分页连续编号</enum><enum>分页重新编号</enum></enums></param><param><name>SelectOnClick</name><type>string</type><caption>复选框事件</caption><description>定义复选框Onclick事件</description><class>string</class><default>dl_tr_bg(this,'@color1','@color2')</default><category>7.显示</category></param><param><name>NumsPerPage</name><type>int</type><caption>显示几条/页</caption><description>每页显示几条记录，整数。为0则为显示全部，并关闭翻页</description><class>int</class><default>8</default><category>7.显示</category></param><param><name>FirstPage</name><type>string</type><caption>首页文字</caption><description>定义“首页”的文字</description><class>string</class><default>首页</default><category>7.显示</category></param><param><name>PrePage</name><type>string</type><caption>上页文字</caption><description>定义“上一页”的文字</description><class>string</class><default>上一页</default><category>7.显示</category></param><param><name>NextPage</name><type>string</type><caption>下页文字</caption><description>定义“下一页”的文字</description><class>string</class><default>下一页</default><category>7.显示</category></param><param><name>LastPage</name><type";
public static string PartXML1=">string</type><caption>末页文字</caption><description>定义“末页”的文字</description><class>string</class><default>末页</default><category>7.显示</category></param><param><name>TurnPage</name><type>string</type><caption>定义翻页</caption><description>定义翻页。%C记录总数,%T首页末页链接,%P总页数,%B()按钮文字,%I页码输入框,%N每页显示几条下拉框</description><class>string</class><default>记录总数%C %T 共%P页 %B(转到)第%I页 每页显示%N条</default><category>7.显示</category></param><param><name>CountZero</name><type>string</type><caption>记录总数0</caption><description>定义记录总数为0时显示的文字</description><class>string</class><default>所查询到的记录总数为 0</default><category>7.显示</category></param><param><name>Cellspacing</name><type>int</type><caption>CellSpacing</caption><description>设置主表格cellspacing值</description><class>int</class><default>3</default><category>7.显示</category></param><param><name>Cellpadding</name><type>int</type><caption>CellPadding</caption><description>设置主表格cellpadding值</description><class>int</class><default>1</default><category>7.显示</category></param><param><name>ColorHover</name><type>string</type><caption>数据行Hover颜色</caption><description>设置数据行Hover颜色</description><class>string</class><default>#ffffcc</default><category>7.显示</category></param><param><name>ColorDefault</name><type>string</type><caption>数据行默认颜色</caption><description>设置数据行默认颜色，逗号隔开表示奇偶行默认颜色</description><class>string</class><default>#ffffff</default><category>7.显示</category></param><param><name>ColorSelect</name><type>string</type><caption>数据行选中颜色</caption><description>设置数据行选中颜色</description><class>string</class><default>#ececff</default><category>7.显示</category></param><param><name>HeadIsShow</name><type>int</type><caption>是否显示列名</caption><description>设定是否显示列名</description><class>enum</class><default>1</default><category>7.显示</category><enums><enum>否</enum><enum>是</enum></enums></p";
public static string PartXML2="aram><param><name>TurnIsShow</name><type>int</type><caption>是否显示翻页</caption><description>设定是否显示翻页</description><class>enum</class><default>1</default><category>7.显示</category><enums><enum>否</enum><enum>是</enum></enums></param><param><name>OrderBy</name><type>string</type><caption>排序方式</caption><description>列表的排序方式，支持@语法。为空表示不设置。设置全自定义SQL时失效。</description><class>string</class><default>order by a.addtime desc</default><category>4.排序</category></param><param><name>NewSiteID</name><type>string</type><caption>站点标识</caption><description>站点标识，为空则为当前站点。</description><class>string</class><default /><category>5.条件</category></param><param><name>FormStartTime</name><type>string</type><caption>表单填写时间起始值</caption><description>填写表单时的时间所在区间起始值，支持@语法。为空表示不设置。设置全自定义SQL时失效。</description><class>string</class><default /><category>5.条件</category></param><param><name>FormEndTime</name><type>string</type><caption>表单填写时间结束值</caption><description>填写表单时的时间所在区间结束值，支持@语法。为空表示不设置。设置全自定义SQL时失效。</description><class>string</class><default /><category>5.条件</category></param><param><name>FormPID</name><type>string</type><caption>设置pid的筛选条件</caption><description>设置pid的筛选条件，支持@语法。为空表示不设置。设置全自定义SQL时失效。</description><class>string</class><default /><category>5.条件</category></param><param><name>MainTable</name><type>string</type><caption>3.主表标识</caption><description>设置数据主表标识，即设定的数据源标识（不包括真实表名的前半部分ft_@siteid@_f_），例如t1，若@开头则为全表名。设置自定义SQL时失效。</description><class>string</class><default /><category>1.基础</category></param><param><name>RowAll</name><type>string</type><caption>1.定义列</caption><description><![CDATA[定义该列的数据和格式，例如：标题#formid;fmem#30%;center#proname#view.ds#_self#savecol，其中@getValueAll(abc)表示元素ID为abc的动态新增行值，@getPValue(data,abc)表示获取该表单的指定父级表单的元素值(@getPValueAll(data,abc)表示父表单所有元素ID为abc的值，以动态新增行的序号排序)，@SQL{sq";
public static string PartXML3="lstr}表示sql取值(支持@语法),@getConst(index)表示获取图文信息，@str表示文字常量，最后一个参数表示该列文字的链接，为空表示无链接，$0|0|按钮文字|地址|_self,$表示链接文字或按钮，第一个数字表示仅仅为该状态值时才显示该链接文字或按钮，为空表示所有状态值，第二个数字为0表示为链接文字，否则表示为按钮，第四个字符表示打开的新窗口的地址，支持@和[]语法，第五个参数为打开方式。传递的关键参数为dsformid，dsformpid。一列里面需要显示多项可以用分号隔开。$0,1|0|多项匹配时可以用逗号隔开。~0|(0|a1)(1|a2)(other|a3)表示0级流程不同状态值时显示的图文。!abcd(null|x1)(|x2)(val0|x3)(other|x4)表示列abcd的值分别为不存在、为空、val0或其他时显示的图文。若列定义后加&&&即为定义该列的开关条件，支持@语法，例如@session[abc]==\"1\"、@session[abc]!=\"1\"、@session[abc]=?\",1,\"、@session[abc]!?\",1,\"（=?表示包含,!?表示不包含）。多列用|||隔开。用[rowid]来获取指定列的当前值。]]></description><class>string</class><default><![CDATA[项目名称#UN1btIeLgtvMSO4102QtPg==#30%;center#proname#alert('[fid]'+'[pid]')#_self#&amp;&amp;&amp;|||添加人#vNtbjABXfgQSymG2mTM6Rw==#20%;center####&amp;&amp;&amp;|||添加时间#7Fu4ag/rp4T5GGfQjcHxIw==#30%;center#addtime###&amp;&amp;&amp;|||#FUd2Fiu2dXpByB8TPQYnLLnXikHNiNmTZcyQx1d/few=#20%;center####&amp;&amp;&amp;]]></default><category>1.基础</category></param><param><name>IsAutoShow</name><type>int</type><caption>2.是否自动显示</caption><description>定义是否自动显示，手动显示需要执行load_[partid](1)方法</description><class>enum</class><default>1</default><category>1.基础</category><enums><enum>手动</enum><enum>自动</enum></enums></param><param><name>CusCondition</name><type>string</type><caption>3.自定义条件</caption><description>自定义查询SQL条件，支持@code接口，{para}写法，例如and a.flow!=8。支持@语法。为空表示不设置。设置全自定义SQL时失效。</description><class>string</class><default /><category>2.高级</category></param><param><name>RateEle</name><type>string</type><caption>4.动态新增行列名</caption><description>默认以fid为主标识显示，此处定义为以fid和动态新增行列名为主标识显示。输入值为动态新增行的列名。可用于动态新增行的列表页显示</description><class>string</class><default /><category>2.高级</category></param><param><name>CusSQL</name><type>string</type><caption>1.全自定义查询</caption><description>全自定义列表查询SQL，支持@code接口，{para}写法，应用此项其他设置条件";
public static string PartXML4="无效，支持@语法。为空表示不设置。例如：select  a.*,b.cusid from ft_@siteid@_f_t1 a,ft_@siteid@_f_t1 b where a.stat=1 and a.flow=0 and a.rowid=b.cusid order by a.addtime desc。</description><class>string</class><default /><category>2.高级</category></param><param><name>CusSQLHalf</name><type>string</type><caption>2.半自定义查询</caption><description>半自定义列表查询SQL，支持@code接口，{para}写法，应用此项其他设置条件依旧有效，支持@语法。为空表示不设置。主表必须取别名为a，必须以条件结尾，不能跟order by语句，例如：select  a.*,b.cusid from ft_@siteid@_f_t1 a,ft_@siteid@_f_t1 b where a.rowid=b.cusid。设置全自定义SQL时失效。</description><class>string</class><default /><category>2.高级</category></param><param><name>RerationTree</name><type>string</type><caption>5.级联显示</caption><description>设置为级联显示并指定参数，为空表示不设置。三个参数用分号隔开，例如，fid;pid;pid='@request[dsformid]'，分别表示主键字段、父级字段、第一级的筛选条件（支持@语法，{para}写法）。</description><class>string</class><default /><category>2.高级</category></param><param><name>BlockDataDefine</name><type>string</type><caption>7.数据渲染模板</caption><description><![CDATA[定义数据渲染模板，格式为HTML代码，支持@和[]语法，例如<div style=\"float:left;width:100px;height:100px\">{check}[proname]</div>，为空为不设置，{check}表示复选或单选框，支持code接口]]></description><class>string</class><default /><category>2.高级</category></param><param><name>AppendTitle</name><type>string</type><caption>8.附加表头</caption><description><![CDATA[定义附加表头，格式为HTML代码，支持@语法，例如<tr><th colspan=2>2015</th><th colspan=2>2016</th></tr>，为空为不设置，支持code接口]]></description><class>string</class><default /><category>2.高级</category></param><param><name>LoadEndJS</name><type>string</type><caption>9.载入后JS</caption><description><![CDATA[定义载入后JS，即load方法异步数据获取完毕后执行的JS，不能带双引号]]></description><class>string</class><default /><category>2.高级</category></param><param><name>CustomConnection</name><type>string</type><caption>A.自定义数据库连接</caption><description><![CDATA[指定该数据列表从非当前数据库中获";
public static string PartXML5="取数据，只支持code接口]]></description><class>string</class><default /><category>2.高级</category></param><param><name>CustomTurnPageBottom</name><type>string</type><caption>B.自定义下部翻页</caption><description><![CDATA[自定义下部翻页，为空不设置，只支持code接口，$1、$2、$3、$4、$5分别表示片段ID、总数、每页数量、总页数、当前页码]]></description><class>string</class><default /><category>2.高级</category></param><param><name>CustomTurnPageTop</name><type>string</type><caption>C.自定义上部翻页</caption><description><![CDATA[自定义上部翻页，为空不设置，只支持code接口，$1、$2、$3、$4、$5分别表示片段ID、总数、每页数量、总页数、当前页码]]></description><class>string</class><default /><category>2.高级</category></param><param><name>CacuRowData</name><type>string</type><caption>6.定义计算行</caption><description><![CDATA[定义计算行内容，例如<td colspan=3>合计：</td><td>{sum(col1)} {CONVERT(decimal(18,2),avg(col2))}</td>，根据数据库类型可使用sum,min,max,avg等，为空为不设置，使用code接口时查询条件可能无效]]></description><class>string</class><default /><category>2.高级</category></param><param><name>IsSearchShow</name><type>int</type><caption>8.是否显示查询</caption><description>定义是否显示查询</description><class>enum</class><default>1</default><category>3.查询、按钮</category><enums><enum>不显示</enum><enum>显示</enum></enums></param><param><name>IsCusCdnShow</name><type>int</type><caption>9.是否显示自定义查询</caption><description>定义是否显示自定义查询</description><class>enum</class><default>0</default><category>3.查询、按钮</category><enums><enum>不显示</enum><enum>显示</enum></enums></param><param><name>SchAreaApdHtml</name><type>string</type><caption>7.查询区域附加HTML</caption><description>定义查询区域附加HTML。</description><class>string</class><default /><category>3.查询、按钮</category></param><param><name>AdvSearch</name><type>string</type><caption>5.高级扩展查询</caption><description>定义高级查询js方法，显示高级查询按钮，为空为不生效。</description><class>string</class><default /><category>3.查询、按钮</category></param><param><name>AdvSearchCa";
public static string PartXML6="ption</name><type>string</type><caption>6.高级扩展查询文字</caption><description>定义高级扩展查询文字，为空显示为高级。</description><class>string</class><default /><category>3.查询、按钮</category></param><param><name>SearchDefine</name><type>string</type><caption>2.多字段模糊查询</caption><description>设置多字段模糊查询字段，用分号隔开，例如name;caption;title，生效后例如 and (name like '%val%' or caption like '%val%')。实际查询中，内容以分号隔开为或查询。</description><class>string</class><default /><category>3.查询、按钮</category></param><param><name>SearchDefineTip</name><type>string</type><caption>3.多字段模糊查询提示</caption><description>设置多字段模糊查询提示信息，为空为不设置。</description><class>string</class><default>多字段模糊查询，分号隔开为'或'查询</default><category>3.查询、按钮</category></param><param><name>StrictSearch</name><type>string</type><caption>1.单字段严格查询</caption><description><![CDATA[以下拉框形式显示，当值为空时，忽略该查询，为空为不生效，多个用#隔开，例如：corp;全部工厂|@SQL{select id,name from table}#stat;全部状态|通过:1;不通过:2。说明：值中null为is null,!null为is not null,加!为!=,及>,>=,<,<=,@SQL前亦可加5类比较符号,若比较符右边为()包围的，则为字段比较。支持@code接口]]></description><class>string</class><default /><category>3.查询、按钮</category></param><param><name>IsRefreshShow</name><type>int</type><caption>B.是否显示刷新</caption><description>定义是否显示刷新</description><class>enum</class><default>1</default><category>3.查询、按钮</category><enums><enum>不显示</enum><enum>显示</enum></enums></param><param><name>IsColDefineShow</name><type>int</type><caption>A.是否显示列定义</caption><description>定义是否显示列定义</description><class>enum</class><default>0</default><category>3.查询、按钮</category><enums><enum>不显示</enum><enum>显示</enum></enums></param><param><name>CusCdnCols</name><type>string</type><caption>4.自定义查询可选字段</caption><description>设置自定义查询可选字段。格式为字段名|显示名|说明，多个之间用;隔开</description><class>string</class><default /><category>3.查询、按钮</category></param><param><name>Const1</name><type>string</type><caption>图";
public static string PartXML7="文1</caption><description>定义图文常量，在列定义和菜单中用@c1@调用。</description><class>string|image</class><default /><category>常量</category></param><param><name>Const2</name><type>string</type><caption>图文2</caption><description>定义图文常量，在列定义和菜单中用@c2@调用。</description><class>string|image</class><default /><category>常量</category></param><param><name>Const3</name><type>string</type><caption>图文3</caption><description>定义图文常量，在列定义和菜单中用@c3@调用。</description><class>string|image</class><default /><category>常量</category></param><param><name>Const4</name><type>string</type><caption>图文4</caption><description>定义图文常量，在列定义和菜单中用@c4@调用。</description><class>string|image</class><default /><category>常量</category></param><param><name>Const5</name><type>string</type><caption>图文5</caption><description>定义图文常量，在列定义和菜单中用@c5@调用。</description><class>string|image</class><default /><category>常量</category></param><param><name>Const6</name><type>string</type><caption>图文6</caption><description>定义图文常量，在列定义和菜单中用@c6@调用。</description><class>string|image</class><default /><category>常量</category></param><param><name>Const7</name><type>string</type><caption>图文7</caption><description>定义图文常量，在列定义和菜单中用@c7@调用。</description><class>string|image</class><default /><category>常量</category></param><param><name>Const8</name><type>string</type><caption>图文8</caption><description>定义图文常量，在列定义和菜单中用@c8@调用。</description><class>string|image</class><default /><category>常量</category></param><param><name>Const9</name><type>string</type><caption>图文9</caption><description>定义图文常量，在列定义和菜单中用@c9@调用。</description><class>string|image</class><default /><category>常量</category></param><param><name>Const10</name><type>string</type><caption>图文10</caption><description>定义图文常量，在列定义和菜单中用@c10@调用。</description><class>string|image</class><default /><category>常量</category>";
public static string PartXML8="</param><param><name>Const11</name><type>string</type><caption>图文11</caption><description>定义图文常量，在列定义和菜单中用@c11@调用。</description><class>string|image</class><default /><category>常量</category></param><param><name>Const12</name><type>string</type><caption>图文12</caption><description>定义图文常量，在列定义和菜单中用@c12@调用。</description><class>string|image</class><default /><category>常量</category></param><param><name>Const13</name><type>string</type><caption>图文13</caption><description>定义图文常量，在列定义和菜单中用@c13@调用。</description><class>string|image</class><default /><category>常量</category></param><param><name>Const14</name><type>string</type><caption>图文14</caption><description>定义图文常量，在列定义和菜单中用@c14@调用。</description><class>string|image</class><default /><category>常量</category></param><param><name>Const15</name><type>string</type><caption>图文15</caption><description>定义图文常量，在列定义和菜单中用@c15@调用。</description><class>string|image</class><default /><category>常量</category></param><param><name>Const16</name><type>string</type><caption>图文16</caption><description>定义图文常量，在列定义和菜单中用@c16@调用。</description><class>string|image</class><default><![CDATA[]]></default><category>常量</category></param><param><name>Const17</name><type>string</type><caption>图文17</caption><description>定义图文常量，在列定义和菜单中用@c17@调用。</description><class>string|image</class><default><![CDATA[]]></default><category>常量</category></param><param><name>Const18</name><type>string</type><caption>图文18</caption><description>定义图文常量，在列定义和菜单中用@c18@调用。</description><class>string|image</class><default><![CDATA[]]></default><category>常量</category></param><param><name>Const19</name><type>string</type><caption>图文19</caption><description>定义图文常量，在列定义和菜单中用@c19@调用。</description><class>string|image</class><default><![CDATA[]]></default><category>常量</category></param><param><nam";
public static string PartXML9="e>Const20</name><type>string</type><caption>图文20</caption><description>定义图文常量，在列定义和菜单中用@c20@调用。</description><class>string|image</class><default><![CDATA[]]></default><category>常量</category></param><param><name>Const21</name><type>string</type><caption>图文21</caption><description>定义图文常量，在列定义和菜单中用@c21@调用。</description><class>string|image</class><default><![CDATA[]]></default><category>常量</category></param><param><name>Const22</name><type>string</type><caption>图文22</caption><description>定义图文常量，在列定义和菜单中用@c22@调用。</description><class>string|image</class><default><![CDATA[]]></default><category>常量</category></param><param><name>Const23</name><type>string</type><caption>图文23</caption><description>定义图文常量，在列定义和菜单中用@c23@调用。</description><class>string|image</class><default><![CDATA[]]></default><category>常量</category></param><param><name>Const24</name><type>string</type><caption>图文24</caption><description>定义图文常量，在列定义和菜单中用@c24@调用。</description><class>string|image</class><default><![CDATA[]]></default><category>常量</category></param><param><name>List_OPID</name><type>string</type><caption>操作标识_列表</caption><description>绑定操作标识，当前用户拥有该操作标识的权限才允许查看该列表页，为空为不设置。</description><class>string</class><default /><category>权限_列表显示</category></param><param><name>List_Code</name><type>string</type><caption>代码接口_列表</caption><description>@code接口，当返回不为null时，显示返回信息并停止操作，为空则不设置。</description><class>string</class><default></default><category>权限_列表显示</category></param><param><name>MemBind</name><type>string</type><caption>绑定用户_列表</caption><description>绑定新增者的用户名，支持@语法，为空表示不设置。在应用规则中应用。</description><class>string</class><default></default><category>权限_列表显示</category></param><param><name>EleCondition</name><type>string</type><caption>数据筛选_列表</caption><description><![CDATA[设置数据筛选条件，支持@语法，为空表示不设置。在应用规则中应用。例如： a";
public static string PartXML10=".cid='@request[a]' and a.did like '%@request[a]%' and (INSTR('@session[\"asso_roleresname\"]',CONCAT('[',a.fmem,']'))>0]]></description><class>string</class><default /><category>权限_列表显示</category></param><param><name>RoleBindData</name><type>string</type><caption>角色构件绑定_列表</caption><description>指定角色构件的数据源，并应用角色构件所指定的数据和状态进行修改、查看和删除权限的绑定，为空表示不设置。在应用规则中应用。</description><class>string</class><default /><category>权限_列表显示</category></param><param><name>RoleBindSession</name><type>string</type><caption>角色构件会话名_列表</caption><description>指定要过滤的会话名，与角色构件的\"获取用户的SQL\"对应，例如user_id获取ID,user_name获取账户名,user_realname获取真实用户名，user_userrole获取当前用户角色。在应用规则中应用。</description><class>string</class><default>user_id</default><category>权限_列表显示</category></param><param><name>AuthRule</name><type>string</type><caption>应用规则_列表</caption><description>对以上四种权限规则，进行自定义组合，可用and/or/()自由组合，设置全自定义SQL时失效。权限说明：%m%:绑定用户,%e%:数据筛选,%f%:角色绑定fid,%s%:角色绑定状态</description><class>string</class><default>%m% and %e% and %f% and %s%</default><category>权限_列表显示</category></param><param><name>FlowStat</name><type>string</type><caption>指定状态值_列表</caption><description>只显示指定状态值的列表，例如“0,1,2,3”，支持@语法。为空表示不设置。设置全自定义SQL时失效。</description><class>string</class><default /><category>权限_列表显示</category></param><param><name>Del_Code</name><type>string</type><caption>代码接口_删除</caption><description>@code接口，当返回不为null时，显示返回信息并停止操作，为空则不设置。</description><class>string</class><default></default><category>权限_允许删除</category></param><param><name>Del_MemBind</name><type>string</type><caption>绑定用户_删除</caption><description>绑定新增者的用户名，支持@语法，为空表示不设置。在应用规则中应用。</description><class>string</class><default></default><category>权限_允许删除</category></param><param><name>Del_EleCondition</name><type>string</type><caption>数据筛选_删除</caption><description><![CDATA[设置数据筛选条件，支持@语";
public static string PartXML11="法，为空表示不设置。在应用规则中应用。例如： a.cid='@request[a]' and a.did like '%@request[a]%' and (INSTR('@session[\"asso_roleresname\"]',CONCAT('[',a.fmem,']'))>0]]></description><class>string</class><default /><category>权限_允许删除</category></param><param><name>Del_RoleBindData</name><type>string</type><caption>角色构件绑定_删除</caption><description>指定角色构件的数据源，并应用角色构件所指定的数据和状态进行修改、查看和删除权限的绑定，为空表示不设置。在应用规则中应用。</description><class>string</class><default /><category>权限_允许删除</category></param><param><name>Del_RoleBindSession</name><type>string</type><caption>角色构件会话名_删除</caption><description>指定要过滤的会话名，与角色构件的\"获取用户的SQL\"对应，例如user_id获取ID,user_name获取账户名,user_realname获取真实用户名，user_userrole获取当前用户角色。在应用规则中应用。</description><class>string</class><default>user_id</default><category>权限_允许删除</category></param><param><name>Del_AuthRule</name><type>string</type><caption>应用规则_删除</caption><description>对以上四种权限规则，进行自定义组合，可用and/or/()自由组合，设置全自定义SQL时失效。权限说明：%m%:绑定用户,%e%:数据筛选,%f%:角色绑定fid,%s%:角色绑定状态</description><class>string</class><default>%m% and %e% and %f% and %s%</default><category>权限_允许删除</category></param><param><name>Del_FlowStat</name><type>string</type><caption>指定状态值_删除</caption><description>只允许删除指定状态值的列表，例如“0,1,2,3”，支持@语法。为空表示不设置。设置全自定义SQL时失效。</description><class>string</class><default /><category>权限_允许删除</category></param><param><name>Del_OPID</name><type>string</type><caption>操作标识_删除</caption><description>绑定操作标识，当前用户拥有该操作标识的权限才允许执行该操作，为空为不设置。</description><class>string</class><default /><category>权限_允许删除</category></param><param><name>Flow_Code</name><type>string</type><caption>代码接口_流程</caption><description>@code接口，当返回不为null时，显示返回信息并停止操作，为空则不设置，@flow@表示要设置的流程。</description><class>string</class><default></default><category>权限_批量流程</category></param><param><name>Flow_MemBind</name><type>string</type><caption>绑定用户_";
public static string PartXML12="流程</caption><description>绑定新增者的用户名，支持@语法，为空表示不设置。在应用规则中应用。</description><class>string</class><default></default><category>权限_批量流程</category></param><param><name>Flow_EleCondition</name><type>string</type><caption>数据筛选_流程</caption><description><![CDATA[设置数据筛选条件，支持@语法，为空表示不设置。在应用规则中应用。例如： a.cid='@request[a]' and a.did like '%@request[a]%' and (INSTR('@session[\"asso_roleresname\"]',CONCAT('[',a.fmem,']'))>0]]></description><class>string</class><default /><category>权限_批量流程</category></param><param><name>Flow_RoleBindData</name><type>string</type><caption>角色构件绑定_流程</caption><description>指定角色构件的数据源，并应用角色构件所指定的数据和状态进行修改、查看和删除权限的绑定，为空表示不设置。在应用规则中应用。</description><class>string</class><default /><category>权限_批量流程</category></param><param><name>Flow_RoleBindSession</name><type>string</type><caption>角色构件会话名_流程</caption><description>指定要过滤的会话名，与角色构件的\"获取用户的SQL\"对应，例如user_id获取ID,user_name获取账户名,user_realname获取真实用户名，user_userrole获取当前用户角色。在应用规则中应用。</description><class>string</class><default>user_id</default><category>权限_批量流程</category></param><param><name>Flow_AuthRule</name><type>string</type><caption>应用规则_流程</caption><description>对以上四种权限规则，进行自定义组合，可用and/or/()自由组合，设置全自定义SQL时失效。权限说明：%m%:绑定用户,%e%:数据筛选,%f%:角色绑定fid,%s%:角色绑定状态</description><class>string</class><default>%m% and %e% and %f% and %s%</default><category>权限_批量流程</category></param><param><name>Flow_FlowStat</name><type>string</type><caption>指定状态值_流程</caption><description>只允许操作指定状态值的列表，例如“0,1,2,3”，支持@语法。为空表示不设置。设置全自定义SQL时失效。</description><class>string</class><default /><category>权限_批量流程</category></param><param><name>Flow_OPID</name><type>string</type><caption>操作标识_流程</caption><description>绑定操作标识，当前用户拥有该操作标识的权限才允许执行该操作，为空为不设置。</description><class>string</class><default /><category>权限_批量流程</category></param><param><name>Copy_Code</n";
public static string PartXML13="ame><type>string</type><caption>代码接口_复制</caption><description>@code接口，当返回不为null时，显示返回信息并停止操作，为空则不设置。</description><class>string</class><default></default><category>权限_允许复制</category></param><param><name>Copy_MemBind</name><type>string</type><caption>绑定用户_复制</caption><description>绑定新增者的用户名，支持@语法，为空表示不设置。在应用规则中应用。</description><class>string</class><default></default><category>权限_允许复制</category></param><param><name>Copy_EleCondition</name><type>string</type><caption>数据筛选_复制</caption><description><![CDATA[设置数据筛选条件，支持@语法，为空表示不设置。在应用规则中应用。例如： a.cid='@request[a]' and a.did like '%@request[a]%' and (INSTR('@session[\"asso_roleresname\"]',CONCAT('[',a.fmem,']'))>0]]></description><class>string</class><default /><category>权限_允许复制</category></param><param><name>Copy_RoleBindData</name><type>string</type><caption>角色构件绑定_复制</caption><description>指定角色构件的数据源，并应用角色构件所指定的数据和状态进行修改、查看和删除权限的绑定，为空表示不设置。在应用规则中应用。</description><class>string</class><default /><category>权限_允许复制</category></param><param><name>Copy_RoleBindSession</name><type>string</type><caption>角色构件会话名_复制</caption><description>指定要过滤的会话名，与角色构件的\"获取用户的SQL\"对应，例如user_id获取ID,user_name获取账户名,user_realname获取真实用户名，user_userrole获取当前用户角色。在应用规则中应用。</description><class>string</class><default>user_id</default><category>权限_允许复制</category></param><param><name>Copy_AuthRule</name><type>string</type><caption>应用规则_复制</caption><description>对以上四种权限规则，进行自定义组合，可用and/or/()自由组合，设置全自定义SQL时失效。权限说明：%m%:绑定用户,%e%:数据筛选,%f%:角色绑定fid,%s%:角色绑定状态</description><class>string</class><default>%m% and %e% and %f% and %s%</default><category>权限_允许复制</category></param><param><name>Copy_FlowStat</name><type>string</type><caption>指定状态值_复制</caption><description>只允许复制指定状态值的列表，例如“0,1,2,3”，支持@语法。为空表示不设置。设置全自定义SQL时失效。</description><class>string</class><default /><category>权限";
public static string PartXML14="_允许复制</category></param><param><name>Copy_OPID</name><type>string</type><caption>操作标识_复制</caption><description>绑定操作标识，当前用户拥有该操作标识的权限才允许执行该操作，为空为不设置。</description><class>string</class><default /><category>权限_允许复制</category></param><param><name>ExportMax</name><type>int</type><caption>导出最大行数</caption><description>设定导出为CSV的最大行数</description><class>int</class><default>1000</default><category>8.其他</category></param><param><name>MenuIsShow</name><type>int</type><caption>是否显示菜单</caption><description>设定是否显示菜单</description><class>enum</class><default>1</default><category>6.菜单</category><enums><enum>否</enum><enum>是</enum></enums></param><param><name>MenuOprole</name><type>string</type><caption>菜单角色名称</caption><description>定义操作角色名称，用;隔开，系统角色包括del,refresh,export,copy,flow12,saveto(tabletag)</description><class>string</class><default>add;mod;view;del;export</default><category>6.菜单</category></param><param><name>MenuOpname</name><type>string</type><caption>菜单功能项名称</caption><description>定义操作功能项名称，用;隔开</description><class>string</class><default>新增;修改;查看;删除;导出</default><category>6.菜单</category></param><param><name>MenuOpcheck</name><type>string</type><caption>菜单选项类型</caption><description>定义菜单选项类型，用;隔开,common表示跟菜单角色名称对应,one表示必须且只能选中1条记录,none表示任何情况,more表示必须选择一条或多条</description><class>string</class><default>none;one;one;common;common</default><category>6.菜单</category></param><param><name>MenuOpendurer</name><type>string</type><caption>菜单角色方法</caption><description>表示菜单角色onclick方法。用##隔开。比如dl_openWindow('add.aspx'，''，500，400，'no')，[id]和[ids]分别表示已选框的id。使用数组[]，中间请加非英文字符，例如[0+1]。flow@code(a.a,)表示流程值由@code接口定义，flow1:confirm表示流程操作前的js确认。</description><class>string</class><default>alert('add')##alert('mod'+[id])##alert('view'+[id])##common##common</default><category>6.菜单</category></param><param><n";
public static string PartXML15="ame>MenuOppic</name><type>string</type><caption>菜单图片设置</caption><description>定义操作的图片提示，用;隔开,调用图片序列的图片使用@pic加序号的方式，比如@pic1</description><class>string</class><default>ui-icon-plus;ui-icon-pencil;ui-icon-document;ui-icon-trash;ui-icon-extlink</default><category>6.菜单</category></param></public_params><styles><style name=\"menutablestyle\" description=\"设置菜单表格样式\" caption=\"菜单表格样式\" class=\"\" csstext=\"\"/><style name=\"menurowstyle\" description=\"设置菜单表格行样式\" caption=\"菜单表格行样式\" class=\"\" csstext=\"\"/><style name=\"menucolstyle\" description=\"设置菜单按钮表格列样式\" caption=\"菜单按钮表格列样式\" class=\"\" csstext=\"\"/><style name=\"menucolsearchstyle\" description=\"设置菜单搜索表格列样式\" caption=\"菜单搜索表格列样式\" class=\"\" csstext=\"TEXT-ALIGN: right\"/><style name=\"csslisthead\" description=\"设置主体表格样式\" caption=\"主体表格样式\" class=\"\" csstext=\"\"/><style name=\"csslistsearchtext\" description=\"设置查询文本框样式\" caption=\"查询文本框样式\" class=\"_input\" csstext=\"\"/><style name=\"csslistsearchselect\" description=\"设置查询下拉框样式\" caption=\"查询下拉框样式\" class=\"_select\" csstext=\"\"/><style name=\"csslisttitletr\" description=\"设置标题行样式\" caption=\"标题行样式\" class=\"\" csstext=\"background:#c0c0ff\"/><style name=\"cssselecttitletd\" description=\"设置标题选框列样式\" caption=\"标题选框列样式\" class=\"list_b_rbl\" csstext=\"\"/><style name=\"csslisttitletd1\" description=\"设置标题第一列样式\" caption=\"标题第一列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttitletd2\" description=\"设置标题第二列样式\" caption=\"标题第二列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttitletd3\" description=\"设置标题第三列样式\" caption=\"标题第三列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttitletd4\" description=\"设置标题第四列样式\" caption=\"标题第四列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttitletd5\" description=\"设置标题第五列样式\" capti";
public static string PartXML16="on=\"标题第五列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttitletd6\" description=\"设置标题第六列样式\" caption=\"标题第六列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttitletd7\" description=\"设置标题第七列样式\" caption=\"标题第七列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttitletd8\" description=\"设置标题第八列样式\" caption=\"标题第八列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttitletd9\" description=\"设置标题第九列样式\" caption=\"标题第九列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttitletd10\" description=\"设置标题第十列样式\" caption=\"标题第十列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttitletd11\" description=\"设置标题第十一列样式\" caption=\"标题第十一列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttitletd12\" description=\"设置标题第十二列样式\" caption=\"标题第十二列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttitletdother\" description=\"设置标题其他列样式\" caption=\"标题其他列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttr0\" description=\"设置偶数行样式\" caption=\"偶数行样式\" class=\"ftlisttr0\" csstext=\"\"/><style name=\"csslisttr1\" description=\"设置奇数行样式\" caption=\"奇数行样式\" class=\"ftlisttr1\" csstext=\"\"/><style name=\"csslisttrcacu\" description=\"设置计算行样式\" caption=\"计算行样式\" class=\"ftlisttrcacu\" csstext=\"background:#EEEEEE\"/><style name=\"cssselecttd\" description=\"设置选框列和序号列样式\" caption=\"选框列和序号列样式\" class=\"list_b_rbl\" csstext=\"\"/><style name=\"csslisttd1\" description=\"设置第一列样式\" caption=\"第一列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttd2\" description=\"设置第二列样式\" caption=\"第二列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttd3\" description=\"设置第三列样式\" caption=\"第三列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttd4\" description=\"设置第四列样式\" caption=\"第四列样式\" class=\"list_b_rb\" ";
public static string PartXML17="csstext=\"\"/><style name=\"csslisttd5\" description=\"设置第五列样式\" caption=\"第五列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttd6\" description=\"设置第六列样式\" caption=\"第六列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttd7\" description=\"设置第七列样式\" caption=\"第七列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttd8\" description=\"设置第八列样式\" caption=\"第八列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttd9\" description=\"设置第九列样式\" caption=\"第九列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttd10\" description=\"设置第十列样式\" caption=\"第十列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttd11\" description=\"设置第十一列样式\" caption=\"第十一列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttd12\" description=\"设置第十二列样式\" caption=\"第十二列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"csslisttdother\" description=\"设置其他列样式\" caption=\"其他列样式\" class=\"list_b_rb\" csstext=\"\"/><style name=\"cssturnpagetb\" description=\"设置翻页主体表格样式\" caption=\"翻页主体表格样式\" class=\"\" csstext=\"\"/><style name=\"cssturnpagetr\" description=\"设置翻页行样式\" caption=\"翻页行样式\" class=\"\" csstext=\"\"/><style name=\"cssturnpagetd\" description=\"设置翻页单元格样式\" caption=\"翻页单元格样式\" class=\"list_b_rl\" csstext=\"\"/><style name=\"cssturnpagetbtop\" description=\"设置翻页主体表格样式_顶部\" caption=\"翻页主体表格样式_顶部\" class=\"\" csstext=\"\"/><style name=\"cssturnpagetrtop\" description=\"设置翻页行样式_顶部\" caption=\"翻页行样式_顶部\" class=\"\" csstext=\"\"/><style name=\"cssturnpagetdtop\" description=\"设置翻页单元格样式_顶部\" caption=\"翻页单元格样式_顶部\" class=\"list_b_rl\" csstext=\"\"/><style name=\"cssturnpageinput\" description=\"设置翻页转页输入样式\" caption=\"翻页转页输入样式\" class=\"\" csstext=\"width:40px\"/><style name=\"cssturnpageselect\" description=\"设置翻页每页显示几条样式\" caption=\"翻页每页显示几条样式\"";
public static string PartXML18=" class=\"_select\" csstext=\"\"/><style name=\"savecolstyle\" description=\"设置内容修改输入框样式\" caption=\"内容修改输入框\" class=\"\" csstext=\"width:90%\"/></styles></partxml>";
public static string PartXML19="";
public static string PartXML=PartXML0+PartXML1+PartXML2+PartXML3+PartXML4+PartXML5+PartXML6+PartXML7+PartXML8+PartXML9+PartXML10+PartXML11+PartXML12+PartXML13+PartXML14+PartXML15+PartXML16+PartXML17+PartXML18+PartXML19;
public string getPartXml()
{
return PartXML;
}
public string getHtml(string theControlID,string SetStyle,int SelectType,string LoadingImg,int RateNumType,string SelectOnClick,int NumsPerPage,string FirstPage,string PrePage,string NextPage,string LastPage,string TurnPage,string CountZero,int Cellspacing,int Cellpadding,string ColorHover,string ColorDefault,string ColorSelect,int HeadIsShow,int TurnIsShow,string OrderBy,string NewSiteID,string FormStartTime,string FormEndTime,string FormPID,string MainTable,string RowAll,int IsAutoShow,string CusCondition,string RateEle,string CusSQL,string CusSQLHalf,string RerationTree,string BlockDataDefine,string AppendTitle,string LoadEndJS,string CustomConnection,string CustomTurnPageBottom,string CustomTurnPageTop,string CacuRowData,int IsSearchShow,int IsCusCdnShow,string SchAreaApdHtml,string AdvSearch,string AdvSearchCaption,string SearchDefine,string SearchDefineTip,string StrictSearch,int IsRefreshShow,int IsColDefineShow,string CusCdnCols,string Const1,string Const2,string Const3,string Const4,string Const5,string Const6,string Const7,string Const8,string Const9,string Const10,string Const11,string Const12,string Const13,string Const14,string Const15,string Const16,string Const17,string Const18,string Const19,string Const20,string Const21,string Const22,string Const23,string Const24,string List_OPID,string List_Code,string MemBind,string EleCondition,string RoleBindData,string RoleBindSession,string AuthRule,string FlowStat,string Del_Code,string Del_MemBind,string Del_EleCondition,string Del_RoleBindData,string Del_RoleBindSession,string Del_AuthRule,string Del_FlowStat,string Del_OPID,string Flow_Code,string Flow_MemBind,string Flow_EleCondition,string Flow_RoleBindData,string Flow_RoleBindSession,string Flow_AuthRule,string Flow_FlowStat,string Flow_OPID,string Copy_Code,string Copy_MemBind,string Copy_EleCondition,string Copy_RoleBindData,string Copy_RoleBindSession,string Copy_AuthRule,string Copy_FlowStat,string Copy_OPID,int ExportMax,int MenuIsShow,string MenuOprole,string MenuOpname,string MenuOpcheck,string MenuOpendurer,string MenuOppic)
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
string temps="";
string temppara="";
temps=ftdpcontroltemp.Tableframe(temppara,temppara);
temps=ftdpcontroltemp.ListHead(Cellspacing,Cellpadding);
temps=ftdpcontroltemp.SearchText(temppara,temppara,temppara,temppara);
temps=ftdpcontroltemp.SearchSelectStart(temppara,temppara);
temps=ftdpcontroltemp.ListTitleTr(temppara);
temps=ftdpcontroltemp.ListTitleTd0(temppara);
temps=ftdpcontroltemp.ListTitleTd1(temppara,temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTitleTd2(temppara,temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTitleTd3(temppara,temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTitleTd4(temppara,temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTitleTd5(temppara,temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTitleTd6(temppara,temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTitleTd7(temppara,temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTitleTd8(temppara,temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTitleTd9(temppara,temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTitleTd10(temppara,temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTitleTd11(temppara,temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTitleTd12(temppara,temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTitleTdOther(temppara,temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTr0(temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTr1(temppara,temppara,temppara);
temps=ftdpcontroltemp.ListTrCacu(temppara);
temps=ftdpcontroltemp.ListTd0(temppara);
temps=ftdpcontroltemp.ListTd1(temppara,temppara);
temps=ftdpcontroltemp.ListTd2(temppara,temppara);
temps=ftdpcontroltemp.ListTd3(temppara,temppara);
temps=ftdpcontroltemp.ListTd4(temppara,temppara);
temps=ftdpcontroltemp.ListTd5(temppara,temppara);
temps=ftdpcontroltemp.ListTd6(temppara,temppara);
temps=ftdpcontroltemp.ListTd7(temppara,temppara);
temps=ftdpcontroltemp.ListTd8(temppara,temppara);
temps=ftdpcontroltemp.ListTd9(temppara,temppara);
temps=ftdpcontroltemp.ListTd10(temppara,temppara);
temps=ftdpcontroltemp.ListTd11(temppara,temppara);
temps=ftdpcontroltemp.ListTd12(temppara,temppara);
temps=ftdpcontroltemp.ListTdother(temppara,temppara);
temps=ftdpcontroltemp.TurnPageTb(Cellspacing,Cellpadding,temppara);
temps=ftdpcontroltemp.TurnPageTbTop(Cellspacing,Cellpadding,temppara);
temps=ftdpcontroltemp.TurnPageTb0(Cellspacing,Cellpadding,temppara);
temps=ftdpcontroltemp.TurnPageInput(Cellspacing,temppara);
temps=ftdpcontroltemp.TurnPageButton(temppara,temppara,temppara);
temps=ftdpcontroltemp.TurnPageSelect(temppara);
temps=ftdpcontroltemp.SaveColInput(temppara,temppara);

			string s = "";	
			string[] Consts=new string[24];
        Consts[0] = Const1;
        Consts[1] = Const2;
        Consts[2] = Const3;
        Consts[3] = Const4;
        Consts[4] = Const5;
        Consts[5] = Const6;
        Consts[6] = Const7;
        Consts[7] = Const8;
        Consts[8] = Const9;
        Consts[9] = Const10;
        Consts[10] = Const11;
        Consts[11] = Const12;
        Consts[12] = Const13;
        Consts[13] = Const14;
        Consts[14] = Const15;
        Consts[15] = Const16;
        Consts[16] = Const17;
        Consts[17] = Const18;
        Consts[18] = Const19;
        Consts[19] = Const20;
        Consts[20] = Const21;
        Consts[21] = Const22;
        Consts[22] = Const23;
        Consts[23] = Const24;
        
		if (MenuIsShow == 1)
		{
			string[] MenuOppics = MenuOppic.Split(';');
			for(int i = 0; i < MenuOppics.Length; i++)
			{
				if (MenuOppics[i].StartsWith("@c"))
				{
					MenuOppics[i] = Consts[Convert.ToInt32(MenuOppics[i].Substring(2)) - 1];
				}
			}

			string btns = "";

			if (MenuOpname != null && !MenuOpname.Trim().Equals(""))
			{
				string[] name = MenuOpname.Split(';');
				for(int i = 0  ; i< name.Length ; i++)
				{
                    btns += ftdpcontroltemp.TextLinkDemoStyle2("btn" + i, "", name[i]);
				}
			}

            string schs = "";
            if (IsSearchShow == 1)
            {
                if(!SearchDefine.Equals(""))
                {
                  schs += ftdpcontroltemp.SearchText2("");
                }
                string[] strictsearch = StrictSearch.Split(new string[]{"#"},StringSplitOptions.RemoveEmptyEntries);
                foreach (string strictsearch1 in strictsearch)
                {
                    string options = "";
                    options += "<option>" + strictsearch1.Split('|')[0].Split(';')[1] + "</option>";
                    /*
                    string[] items = strictsearch1.Split('|')[1].Split(';');
                    foreach (string _item in items)
                    {
                        options += "<option>" + _item.Split(':')[0] + "</option>";
                    }
                    */
                    schs += ftdpcontroltemp.SearchSelectStart("", "") + options + ftdpcontroltemp.SearchSelectEnd();
                }
                schs += ftdpcontroltemp.TextLinkDemoStyle2("","","查询");
                if (IsCusCdnShow==1)
                {
                    schs += ftdpcontroltemp.TextLinkDemoStyle2("", "", "自定义");
                }
                if (!AdvSearch.Equals(""))
                {
                    schs += ftdpcontroltemp.TextLinkDemoStyle2("", "", AdvSearchCaption==""?"高级":AdvSearchCaption);
                }
            }
            if(IsColDefineShow==1)
            {
                schs += ftdpcontroltemp.TextLinkDemoStyle2("", "", "列定义");
            }
            if(IsRefreshShow==1)
            {
                schs += ftdpcontroltemp.TextLinkDemoStyle2("", "", "刷新");
            }

            s = ftdpcontroltemp.Tableframe(btns, SchAreaApdHtml+"<span style='padding:0px;margin:0px'>"+schs+"</span>");
		}

		s += ftdpcontroltemp.ListHead(Cellspacing, Cellpadding);
        ArrayList ColumnTitle = new ArrayList();
        ArrayList ColumnWidth = new ArrayList();
        ArrayList ColumnAlign = new ArrayList();
        ArrayList ColumnData = new ArrayList();
        ArrayList ColumnLink = new ArrayList();
		ArrayList ColumnOrder = new ArrayList();
		ArrayList ColumnSaveCol = new ArrayList();
 int rowii=-1;
        string[] rows = RowAll.Split(new string[]{"|||"},StringSplitOptions.RemoveEmptyEntries);
        int ColLength = rows.Length;
        for (int rowi = 0; rowi < rows.Length; rowi++)
        {
            string row = rows[rowi].Substring(0,rows[rowi].IndexOf("&&&")).Trim();
            string[] rowcols = row.Split('#');
            
            if (rowcols.Length > 7 && rowcols[7].Equals("1"))continue;//默认隐藏
            rowii++;
            ColumnTitle.Add(rowcols[0]);
            ColumnWidth.Add(rowcols[2].Split(';')[0]);
            ColumnAlign.Add(rowcols[2].Split(';')[1]);
			if (rowcols.Length > 3 && !rowcols[3].Equals(""))
            {
                ColumnOrder.Add(rowcols[3]);
            }
            else
            {
                ColumnOrder.Add(null);
            }
            if (rowcols.Length > 4 && !(rowcols.Length > 6&&!rowcols[6].Equals("")))
            {
                ColumnLink.Add(rowcols[4]);
            }
            else
            {
                ColumnLink.Add(null);
            }
			if (rowcols.Length > 6)
            {
                ColumnSaveCol.Add(rowcols[6]);
            }
            else
            {
                ColumnSaveCol.Add(null);
            }
            
            string str = rowcols[1];


            System.Security.Cryptography.SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new System.Security.Cryptography.RijndaelManaged();
            Key = "^#@$FVSD#$%SDF@#maobb234efwe";
            String sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);
            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength);
            }

            byte[] kkkk = System.Text.ASCIIEncoding.ASCII.GetBytes(sTemp);
            sTemp = "Dmaobbasfui23497#$ASasdkf0";
            mobjCryptoService.GenerateIV();
            bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength);
            }
            byte[] vvvv = System.Text.ASCIIEncoding.ASCII.GetBytes(sTemp);

            byte[] bytIn = Convert.FromBase64String(str);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = kkkk;
            mobjCryptoService.IV = vvvv;
            System.Security.Cryptography.ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, encrypto, System.Security.Cryptography.CryptoStreamMode.Read);
            System.IO.StreamReader sr = new System.IO.StreamReader(cs);

            string coldata = sr.ReadToEnd();
            
            string[] coldatas = coldata.Split(';');
            coldata = "";
            for (int i = 0; i < coldatas.Length; i++)
            {
                if (coldatas[i].IndexOf("@str") == 0) coldatas[i] = coldatas[i].Replace("@str", "");
                //if (coldatas[i].IndexOf("@pval") == 0) coldatas[i] = "(pval" + i.ToString() + "(..." + coldatas[i].Substring(coldatas[i].Length - 5, 5) + ")";
                //if (coldatas[i].IndexOf("@getValue") == 0) coldatas[i] = "pval" + i.ToString() + "(..." + coldatas[i].Substring(coldatas[i].Length - 5, 5);
                if (coldatas[i].IndexOf("@getPValue(") == 0) coldatas[i] = coldatas[i].Replace("@getPValue(", "");
                if (coldatas[i].IndexOf("@getValueAll(") == 0) coldatas[i] = coldatas[i].Replace("@getValueAll(", "");
                if (coldatas[i].IndexOf("@getPValueAll(") == 0) coldatas[i] = coldatas[i].Replace("@getPValueAll(", "");
                if (coldatas[i].IndexOf("@SQL{") == 0) coldatas[i] = "(SQL)";
                //if (coldatas[i].IndexOf("@getMem") == 0) coldatas[i] = coldatas[i].Replace("@getMem", "");
                //if (coldatas[i].IndexOf("@getModtime") == 0) coldatas[i] = coldatas[i].Replace("@getModtime", "");
                if (coldatas[i].IndexOf("@getConst") == 0) coldatas[i] = coldatas[i].Replace(coldatas[i], Consts[int.Parse(coldatas[i].Substring(coldatas[i].IndexOf('(') + 1, coldatas[i].IndexOf(')') - coldatas[i].IndexOf('(')-1))-1]);
                if (coldatas[i].IndexOf("@code(") == 0)coldatas[i]=coldatas[i].Substring(coldatas[i].IndexOf('(')+1,coldatas[i].IndexOf(',')-coldatas[i].IndexOf('(')-1);
                //if (coldatas[i].IndexOf("@step") == 0) coldatas[i] = stepvalues[Convert.ToInt32(coldatas[i].Replace("@step", "")) - 1];
                if (coldatas[i].IndexOf("$") == 0)
                {
                    string[] coldatasbuts = coldatas[i].Split('|');

                    if (coldatasbuts[2].IndexOf("@getConst") == 0) coldatasbuts[2] = coldatasbuts[2].Replace(coldatasbuts[2], Consts[int.Parse(coldatasbuts[2].Substring(coldatasbuts[2].IndexOf('(') + 1, coldatasbuts[2].IndexOf(')') - coldatasbuts[2].IndexOf('(') - 1))-1]);
                    //if (coldatasbuts[2].IndexOf("@step") == 0) coldatasbuts[2] = stepvalues[Convert.ToInt32(coldatasbuts[2].Replace("@step", "")) - 1];

					if(ColumnSaveCol[rowii]==null||ColumnSaveCol[rowii].ToString().Equals(""))
					{
						if (int.Parse(coldatasbuts[1]) == 0)
						{
							coldatas[i] = "<a href=#mbb>" + coldatasbuts[2] + "</a>";
						}
						else if (int.Parse(coldatasbuts[1]) == 1)
						{
							coldatas[i] = "<input type=button value='" + coldatasbuts[2] + "'/>";
						}
					}
					else
					{
						coldatas[i]=coldatasbuts[2];
					}
                }
                if (coldatas[i].IndexOf("~") == 0 || coldatas[i].IndexOf("!") == 0)
                {
                    int extfirstp = coldatas[i].IndexOf('(');
                    int extlastp = coldatas[i].IndexOf(')');
                    coldatas[i] = coldatas[i].Substring(extfirstp + 1, extlastp - extfirstp - 1).Split('|')[1];
                }
                coldata += coldatas[i];
                if (i < coldatas.Length - 1) coldata += ";";
            }
			if(ColumnSaveCol[rowii]==null||ColumnSaveCol[rowii].ToString().Equals(""))
			{
				ColumnData.Add(coldata);
			}
			else
			{
				ColumnData.Add(ftdpcontroltemp.SaveColInput("",coldata.Replace("\"","")));
			}
        }

        string rowtitle = "";
        string rowdata = "";
        for(int i=0;i<ColumnTitle.Count;i++)
        {
			switch(i+1)
			{
				case 1:
					if(ColumnOrder[i]==null)
						rowtitle += ftdpcontroltemp.ListTitleTd1(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), ColumnTitle[i].ToString());
					else 
						rowtitle += ftdpcontroltemp.ListTitleTd1(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), "<a href='javascript:void(0)'>"+ColumnTitle[i].ToString()+"</a>");
					if (ColumnLink[i] != null && !ColumnLink[i].ToString().Trim().Equals(""))
					{
						rowdata += ftdpcontroltemp.ListTd1(ColumnAlign[i].ToString(), "<a href=#maobb>" + ColumnData[i].ToString() + "</a>");
					}
					else
					{
						rowdata += ftdpcontroltemp.ListTd1(ColumnAlign[i].ToString(), ColumnData[i].ToString());
					}
				break;
				case 2:
					if(ColumnOrder[i]==null)
						rowtitle += ftdpcontroltemp.ListTitleTd2(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), ColumnTitle[i].ToString());
					else 
						rowtitle += ftdpcontroltemp.ListTitleTd2(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), "<a href='javascript:void(0)'>"+ColumnTitle[i].ToString()+"</a>");
					if (ColumnLink[i] != null && !ColumnLink[i].ToString().Trim().Equals(""))
					{
						rowdata += ftdpcontroltemp.ListTd2(ColumnAlign[i].ToString(), "<a href=#maobb>" + ColumnData[i].ToString() + "</a>");
					}
					else
					{
						rowdata += ftdpcontroltemp.ListTd2(ColumnAlign[i].ToString(), ColumnData[i].ToString());
					}
				break;
				case 3:
					if(ColumnOrder[i]==null)
						rowtitle += ftdpcontroltemp.ListTitleTd3(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), ColumnTitle[i].ToString());
					else 
						rowtitle += ftdpcontroltemp.ListTitleTd3(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), "<a href='javascript:void(0)'>"+ColumnTitle[i].ToString()+"</a>");
					if (ColumnLink[i] != null && !ColumnLink[i].ToString().Trim().Equals(""))
					{
						rowdata += ftdpcontroltemp.ListTd3(ColumnAlign[i].ToString(), "<a href=#maobb>" + ColumnData[i].ToString() + "</a>");
					}
					else
					{
						rowdata += ftdpcontroltemp.ListTd3(ColumnAlign[i].ToString(), ColumnData[i].ToString());
					}
				break;
				case 4:
					if(ColumnOrder[i]==null)
						rowtitle += ftdpcontroltemp.ListTitleTd4(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), ColumnTitle[i].ToString());
					else 
						rowtitle += ftdpcontroltemp.ListTitleTd4(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), "<a href='javascript:void(0)'>"+ColumnTitle[i].ToString()+"</a>");
					if (ColumnLink[i] != null && !ColumnLink[i].ToString().Trim().Equals(""))
					{
						rowdata += ftdpcontroltemp.ListTd4(ColumnAlign[i].ToString(), "<a href=#maobb>" + ColumnData[i].ToString() + "</a>");
					}
					else
					{
						rowdata += ftdpcontroltemp.ListTd4(ColumnAlign[i].ToString(), ColumnData[i].ToString());
					}
				break;
				case 5:
					if(ColumnOrder[i]==null)
						rowtitle += ftdpcontroltemp.ListTitleTd5(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), ColumnTitle[i].ToString());
					else 
						rowtitle += ftdpcontroltemp.ListTitleTd5(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), "<a href='javascript:void(0)'>"+ColumnTitle[i].ToString()+"</a>");
					if (ColumnLink[i] != null && !ColumnLink[i].ToString().Trim().Equals(""))
					{
						rowdata += ftdpcontroltemp.ListTd5(ColumnAlign[i].ToString(), "<a href=#maobb>" + ColumnData[i].ToString() + "</a>");
					}
					else
					{
						rowdata += ftdpcontroltemp.ListTd5(ColumnAlign[i].ToString(), ColumnData[i].ToString());
					}
				break;
				case 6:
					if(ColumnOrder[i]==null)
						rowtitle += ftdpcontroltemp.ListTitleTd6(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), ColumnTitle[i].ToString());
					else 
						rowtitle += ftdpcontroltemp.ListTitleTd6(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), "<a href='javascript:void(0)'>"+ColumnTitle[i].ToString()+"</a>");
					if (ColumnLink[i] != null && !ColumnLink[i].ToString().Trim().Equals(""))
					{
						rowdata += ftdpcontroltemp.ListTd6(ColumnAlign[i].ToString(), "<a href=#maobb>" + ColumnData[i].ToString() + "</a>");
					}
					else
					{
						rowdata += ftdpcontroltemp.ListTd6(ColumnAlign[i].ToString(), ColumnData[i].ToString());
					}
				break;
				case 7:
					if(ColumnOrder[i]==null)
						rowtitle += ftdpcontroltemp.ListTitleTd7(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), ColumnTitle[i].ToString());
					else 
						rowtitle += ftdpcontroltemp.ListTitleTd7(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), "<a href='javascript:void(0)'>"+ColumnTitle[i].ToString()+"</a>");
					if (ColumnLink[i] != null && !ColumnLink[i].ToString().Trim().Equals(""))
					{
						rowdata += ftdpcontroltemp.ListTd7(ColumnAlign[i].ToString(), "<a href=#maobb>" + ColumnData[i].ToString() + "</a>");
					}
					else
					{
						rowdata += ftdpcontroltemp.ListTd7(ColumnAlign[i].ToString(), ColumnData[i].ToString());
					}
				break;
				case 8:
					if(ColumnOrder[i]==null)
						rowtitle += ftdpcontroltemp.ListTitleTd8(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), ColumnTitle[i].ToString());
					else 
						rowtitle += ftdpcontroltemp.ListTitleTd8(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), "<a href='javascript:void(0)'>"+ColumnTitle[i].ToString()+"</a>");
					if (ColumnLink[i] != null && !ColumnLink[i].ToString().Trim().Equals(""))
					{
						rowdata += ftdpcontroltemp.ListTd8(ColumnAlign[i].ToString(), "<a href=#maobb>" + ColumnData[i].ToString() + "</a>");
					}
					else
					{
						rowdata += ftdpcontroltemp.ListTd8(ColumnAlign[i].ToString(), ColumnData[i].ToString());
					}
				break;
				case 9:
					if(ColumnOrder[i]==null)
						rowtitle += ftdpcontroltemp.ListTitleTd9(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), ColumnTitle[i].ToString());
					else 
						rowtitle += ftdpcontroltemp.ListTitleTd9(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), "<a href='javascript:void(0)'>"+ColumnTitle[i].ToString()+"</a>");
					if (ColumnLink[i] != null && !ColumnLink[i].ToString().Trim().Equals(""))
					{
						rowdata += ftdpcontroltemp.ListTd9(ColumnAlign[i].ToString(), "<a href=#maobb>" + ColumnData[i].ToString() + "</a>");
					}
					else
					{
						rowdata += ftdpcontroltemp.ListTd9(ColumnAlign[i].ToString(), ColumnData[i].ToString());
					}
				break;
				case 10:
					if(ColumnOrder[i]==null)
						rowtitle += ftdpcontroltemp.ListTitleTd10(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), ColumnTitle[i].ToString());
					else 
						rowtitle += ftdpcontroltemp.ListTitleTd10(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), "<a href='javascript:void(0)'>"+ColumnTitle[i].ToString()+"</a>");
					if (ColumnLink[i] != null && !ColumnLink[i].ToString().Trim().Equals(""))
					{
						rowdata += ftdpcontroltemp.ListTd10(ColumnAlign[i].ToString(), "<a href=#maobb>" + ColumnData[i].ToString() + "</a>");
					}
					else
					{
						rowdata += ftdpcontroltemp.ListTd10(ColumnAlign[i].ToString(), ColumnData[i].ToString());
					}
				break;
				case 11:
					if(ColumnOrder[i]==null)
						rowtitle += ftdpcontroltemp.ListTitleTd11(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), ColumnTitle[i].ToString());
					else 
						rowtitle += ftdpcontroltemp.ListTitleTd11(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), "<a href='javascript:void(0)'>"+ColumnTitle[i].ToString()+"</a>");
					if (ColumnLink[i] != null && !ColumnLink[i].ToString().Trim().Equals(""))
					{
						rowdata += ftdpcontroltemp.ListTd11(ColumnAlign[i].ToString(), "<a href=#maobb>" + ColumnData[i].ToString() + "</a>");
					}
					else
					{
						rowdata += ftdpcontroltemp.ListTd11(ColumnAlign[i].ToString(), ColumnData[i].ToString());
					}
				break;
				case 12:
					if(ColumnOrder[i]==null)
						rowtitle += ftdpcontroltemp.ListTitleTd12(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), ColumnTitle[i].ToString());
					else 
						rowtitle += ftdpcontroltemp.ListTitleTd12(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), "<a href='javascript:void(0)'>"+ColumnTitle[i].ToString()+"</a>");
					if (ColumnLink[i] != null && !ColumnLink[i].ToString().Trim().Equals(""))
					{
						rowdata += ftdpcontroltemp.ListTd12(ColumnAlign[i].ToString(), "<a href=#maobb>" + ColumnData[i].ToString() + "</a>");
					}
					else
					{
						rowdata += ftdpcontroltemp.ListTd12(ColumnAlign[i].ToString(), ColumnData[i].ToString());
					}
				break;
				default:
					if(ColumnOrder[i]==null)
						rowtitle += ftdpcontroltemp.ListTitleTdOther(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), ColumnTitle[i].ToString());
					else 
						rowtitle += ftdpcontroltemp.ListTitleTdOther(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), "<a href='javascript:void(0)'>"+ColumnTitle[i].ToString()+"</a>");
					if (ColumnLink[i] != null && !ColumnLink[i].ToString().Trim().Equals(""))
					{
						rowdata += ftdpcontroltemp.ListTdother(ColumnAlign[i].ToString(), "<a href=#maobb>" + ColumnData[i].ToString() + "</a>");
					}
					else
					{
						rowdata += ftdpcontroltemp.ListTdother(ColumnAlign[i].ToString(), ColumnData[i].ToString());
					}
				break;
			}
        }
        ColumnTitle.Clear();
        ColumnWidth.Clear();
        ColumnAlign.Clear();
        ColumnData.Clear();
        ColumnLink.Clear();
		ColumnOrder.Clear();
        ColumnTitle = null;
        ColumnWidth = null;
        ColumnAlign = null;
        ColumnData = null;
        ColumnLink = null;
		ColumnOrder=null;

        if (RateNumType == 1 || RateNumType == 2)
        {
            rowtitle = ftdpcontroltemp.ListTitleTd0("No.") + rowtitle;
        }
        if (SelectType == 0)
        {
            rowtitle = ftdpcontroltemp.ListTitleTd0(ftdpcontroltemp.ListTitleMSelect(ColorSelect,ColorDefault)) + rowtitle;
        }
        else if (SelectType == 1)
        {
            rowtitle = ftdpcontroltemp.ListTitleTd0("") + rowtitle;
        }
		if(HeadIsShow==1)
		{
			s += ftdpcontroltemp.ListTitleTr(rowtitle);
		}
		
		if(NumsPerPage==0){
			NumsPerPage=12;
			TurnIsShow=0;
		}
       if(BlockDataDefine!=null&&!BlockDataDefine.Trim().Equals(""))
       {
            string DataBlockHtml = "";
            for (int i = 0; i < NumsPerPage; i++)
            {
                string checkboxhtml = "";
                if (SelectType == 0)
                {
                    checkboxhtml=ftdpcontroltemp.ListMSelect("", SelectOnClick.Replace("@color1",ColorSelect.StartsWith("@")?ColorSelect.Substring(1):ColorSelect).Replace("@color2",ColorDefault.StartsWith("@")?ColorDefault.Substring(1):ColorDefault));
                }
                else if (SelectType == 1)
                {
                    checkboxhtml=ftdpcontroltemp.ListSelect("");
                }
                DataBlockHtml += BlockDataDefine.Replace("{check}",checkboxhtml);
            }
            s += ftdpcontroltemp.ListTrTdDataBlock(ColorHover, ColorDefault, ColLength,DataBlockHtml);
       }
       else
       {
          for (int i = 0; i < NumsPerPage; i++)
          {
              string rowdata_num = "";

              if (RateNumType == 1 || RateNumType == 2)
              {
                  rowdata_num = ftdpcontroltemp.ListTd0((i + 1).ToString());
              }
              if (SelectType == 0)
              {
                  rowdata_num = ftdpcontroltemp.ListTd0(ftdpcontroltemp.ListMSelect("", SelectOnClick.Replace("@color1",ColorSelect.StartsWith("@")?ColorSelect.Substring(1):ColorSelect).Replace("@color2",ColorDefault.StartsWith("@")?ColorDefault.Substring(1):ColorDefault))) + rowdata_num;
              }
              else if (SelectType == 1)
              {
                  rowdata_num = ftdpcontroltemp.ListTd0(ftdpcontroltemp.ListSelect("")) + rowdata_num;
              }
			

              if (i % 2 == 0)
              {
                  s += ftdpcontroltemp.ListTr1(ColorHover,(ColorDefault.IndexOf(',')>0?ColorDefault.Split(',')[0]:ColorDefault),rowdata_num+rowdata);
              }
              else
              {
                  s += ftdpcontroltemp.ListTr0(ColorHover,(ColorDefault.IndexOf(',')>0?ColorDefault.Split(',')[1]:ColorDefault),rowdata_num+rowdata);
              }
          }
        }
		   if(CacuRowData!=null && !CacuRowData.Trim().Equals(""))
       {
          if(CacuRowData.StartsWith("@code("))s += ftdpcontroltemp.ListTrCacu("<td colspan=99>"+CacuRowData+"</td>");
          else s += ftdpcontroltemp.ListTrCacu(CacuRowData);
       }
        s += ftdpcontroltemp.ListTail();
		if(TurnIsShow==1)
		{
			string turnpagebtn = "";
			string turnpageori = "turnpageori";
			if (TurnPage.IndexOf("%B(") >= 0)
			{
				turnpagebtn = TurnPage.Substring(TurnPage.IndexOf("%B(") + 3, TurnPage.IndexOf(")", TurnPage.IndexOf("%B(")) - TurnPage.IndexOf("%B(") -3);
				turnpageori = "%B(" + turnpagebtn + ")";
				turnpagebtn = ftdpcontroltemp.TurnPageButton(turnpagebtn,"","");
			}
			string turnpageall = TurnPage.Replace("%C", NumsPerPage.ToString()).Replace("%T", FirstPage + "&nbsp;&nbsp;" + PrePage + "&nbsp;&nbsp;" + NextPage + "&nbsp;&nbsp;" + LastPage).Replace("%P", "1").Replace(" ", "&nbsp;").Replace("%I", ftdpcontroltemp.TurnPageInput(1, "")).Replace(turnpageori, turnpagebtn).Replace("%N", ftdpcontroltemp.TurnPageSelect(""));
            if(CustomTurnPageBottom.Trim()=="")
			s += ftdpcontroltemp.TurnPageTb(Cellspacing, Cellpadding, turnpageall);
			else s += ftdpcontroltemp.TurnPageTb(Cellspacing, Cellpadding, CustomTurnPageBottom);
			if(CustomTurnPageTop.Trim()!="")s=ftdpcontroltemp.TurnPageTbTop(Cellspacing, Cellpadding, CustomTurnPageTop)+s;
		}

		return s;
			
}
}
}
