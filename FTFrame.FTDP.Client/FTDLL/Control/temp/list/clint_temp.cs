/*FTDP Control,This Code Created By FTDP Deploy Tool
Build By Maobb 2007-6-10
Code Deploy Time is:2017-12-27 15:58:14*/
using System;
namespace ft_list
{
public class ftdpcontroltemp
{
private static string setStyle(string name)
{
return setStyleClass(name) + setStyleCssTxt(name);
}
private static string setStyleClass(string name)
{
int i;
for(i=0;i<client.thisStyleObject.Length;i++)
{
	if(((string[])(client.thisStyleObject[i]))[0].Equals(name))
	{
		if(!((string[])(client.thisStyleObject[i]))[1].Equals(""))
		{
			return "class=\"" + ((string[])(client.thisStyleObject[i]))[1] + "\" ";
		}
		return "";
	}
}
return "";
}
private static string setStyleCssTxt(string name)
{
	int i;
	for(i=0;i<client.thisStyleObject.Length;i++)
	{
		if(((string[])(client.thisStyleObject[i]))[0].Equals(name))
	{
		if(!((string[])(client.thisStyleObject[i]))[2].Equals(""))
		{
			return "style=\"" + ((string[])(client.thisStyleObject[i]))[2] + "\" ";
		}
		return "";
	}
}
return "";
}
public static string getStyleClass(string name)
{
int i;
for(i=0;i<client.thisStyleObject.Length;i++)
{
	if(((string[])(client.thisStyleObject[i]))[0].Equals(name))
	{
		if(!((string[])(client.thisStyleObject[i]))[1].Equals(""))
		{
			return ((string[])(client.thisStyleObject[i]))[1];
		}
		return "";
	}
}
return "";
}
public static string getStyleCssTxt(string name)
{
	int i;
	for(i=0;i<client.thisStyleObject.Length;i++)
	{
		if(((string[])(client.thisStyleObject[i]))[0].Equals(name))
	{
		if(!((string[])(client.thisStyleObject[i]))[2].Equals(""))
		{
			return ((string[])(client.thisStyleObject[i]))[2];
		}
		return "";
	}
}
return "";
}
public static string ListHead(int p0,int p1)
{
return ("  <table cellspacing=" + p0 + " cellpadding=" + p1 + " " + setStyle("csslisthead") + " width=\"100%\">  ").Trim();
}
public static string ListTail()
{
return ("  </table>  ").Trim();
}
public static string ListTitleTr(string p0)
{
return ("  <tr " + setStyle("csslisttitletr") + ">" + p0 + "</tr>  ").Trim();
}
public static string ListTitleTr_JS(string p0)
{
return ("      function listtitletr_" + p0 + "(s) { return '<tr " + setStyle("csslisttitletr") + ">'+s+'</tr>'}  ").Trim();
}
public static string ListTitleTd0(string p0)
{
return ("  <th name=\"listth\" " + setStyle("cssselecttitletd") + " width=1px>" + p0 + "</th>  ").Trim();
}
public static string ListTitleTd0_JS(string p0)
{
return ("        function listtitletd0_" + p0 + "(s) { return '<th name=\"listth\" " + setStyle("cssselecttitletd") + " width=1px>'+s+'</th>'}  ").Trim();
}
public static string ListTitleMSelect(string p0,string p1)
{
return ("  <input type=\"checkbox\" name=\"dl_allselect\" onclick=\"dl_SelectAll(this,'" + p0 + "','" + p1 + "')\">  ").Trim();
}
public static string ListTitleMSelect_JS(string p0)
{
return ("     function listtitlemselect_" + p0 + "(s1,s2) { return '<input type=\"checkbox\" name=\"dl_allselect\" onclick=\"dl_SelectAll(this,\\''+s1+'\\',\\''+s2+'\\')\">' }  ").Trim();
}
public static string ListMSelect(string p0,string p1)
{
return ("  <input type=\"checkbox\" name=\"dlcheckradio\" value=\"" + p0 + "\" onclick=\"" + p1 + "\">  ").Trim();
}
public static string ListSelect(string p0)
{
return ("  <input type=\"radio\" name=\"dlcheckradio\" value=\"" + p0 + "\">  ").Trim();
}
public static string SearchText(string p0,string p1,string p2,string p3)
{
return ("  <input type=\"text\" " + setStyle("csslistsearchtext") + " id=\"scht_" + p0 + "\" onKeyDown=\"if(event.keyCode==13){dl_search('" + p1 + "')}\"/> <input type=\"hidden\" id=\"schtc_" + p2 + "\" value=\"" + p3 + "\">  ").Trim();
}
public static string SearchTextTip(string p0,string p1,string p2,string p3,string p4,string p5)
{
return ("  <input type=\"text\" " + setStyle("csslistsearchtext") + " id=\"scht_" + p0 + "\" title=\"" + p1 + "\" onKeyDown=\"if(event.keyCode==13){dl_search('" + p2 + "')}\"/> <input type=\"hidden\" id=\"schtc_" + p3 + "\" value=\"" + p4 + "\"><script type=\"text/javascript\" language=\"javascript\">$(\"#scht_" + p5 + "\").tooltip();</script>  ").Trim();
}
public static string SearchText2(string p0)
{
return ("  <input type=\"text\" " + setStyle("csslistsearchtext") + " id=\"scht_" + p0 + "\"/>  ").Trim();
}
public static string SearchSelectStart(string p0,string p1)
{
return ("  <select " + setStyle("csslistsearchselect") + " name=ftlistsearch id=\"" + p0 + "__" + p1 + "\">  ").Trim();
}
public static string SearchSelectEnd()
{
return ("  </select>  ").Trim();
}
public static string ListTitleTd1(string p0,string p1,string p2,string p3)
{
return ("  <th name=\"listth\" " + setStyle("csslisttitletd1") + " style=\"width:" + p0 + "\" width=" + p1 + " align=" + p2 + ">" + p3 + "</th>  ").Trim();
}
public static string ListTitleTd1_JS(string p0)
{
return ("        function listtitletd1_" + p0 + "(s1,s2,s3,s4) { return '<th name=\"listth\" " + setStyle("csslisttitletd1") + " style=\"width:'+s1+'\" width=\"'+s2+'\" align=\"'+s3+'\">'+s4+'</th>'}  ").Trim();
}
public static string ListTitleTd2(string p0,string p1,string p2,string p3)
{
return ("  <th name=\"listth\" " + setStyle("csslisttitletd2") + " style=\"width:" + p0 + "\" width=" + p1 + " align=" + p2 + ">" + p3 + "</th>  ").Trim();
}
public static string ListTitleTd2_JS(string p0)
{
return ("        function listtitletd2_" + p0 + "(s1,s2,s3,s4) { return '<th name=\"listth\" " + setStyle("csslisttitletd2") + " style=\"width:'+s1+'\" width=\"'+s2+'\" align=\"'+s3+'\">'+s4+'</th>'}  ").Trim();
}
public static string ListTitleTd3(string p0,string p1,string p2,string p3)
{
return ("  <th name=\"listth\" " + setStyle("csslisttitletd3") + " style=\"width:" + p0 + "\" width=" + p1 + " align=" + p2 + ">" + p3 + "</th>  ").Trim();
}
public static string ListTitleTd3_JS(string p0)
{
return ("        function listtitletd3_" + p0 + "(s1,s2,s3,s4) { return '<th name=\"listth\" " + setStyle("csslisttitletd3") + " style=\"width:'+s1+'\" width=\"'+s2+'\" align=\"'+s3+'\">'+s4+'</th>'}  ").Trim();
}
public static string ListTitleTd4(string p0,string p1,string p2,string p3)
{
return ("  <th name=\"listth\" " + setStyle("csslisttitletd4") + " style=\"width:" + p0 + "\" width=" + p1 + " align=" + p2 + ">" + p3 + "</th>  ").Trim();
}
public static string ListTitleTd4_JS(string p0)
{
return ("        function listtitletd4_" + p0 + "(s1,s2,s3,s4) { return '<th name=\"listth\" " + setStyle("csslisttitletd4") + " style=\"width:'+s1+'\" width=\"'+s2+'\" align=\"'+s3+'\">'+s4+'</th>'}  ").Trim();
}
public static string ListTitleTd5(string p0,string p1,string p2,string p3)
{
return ("  <th name=\"listth\" " + setStyle("csslisttitletd5") + " style=\"width:" + p0 + "\" width=" + p1 + " align=" + p2 + ">" + p3 + "</th>  ").Trim();
}
public static string ListTitleTd5_JS(string p0)
{
return ("        function listtitletd5_" + p0 + "(s1,s2,s3,s4) { return '<th name=\"listth\" " + setStyle("csslisttitletd5") + " style=\"width:'+s1+'\" width=\"'+s2+'\" align=\"'+s3+'\">'+s4+'</th>'}  ").Trim();
}
public static string ListTitleTd6(string p0,string p1,string p2,string p3)
{
return ("  <th name=\"listth\" " + setStyle("csslisttitletd6") + " style=\"width:" + p0 + "\" width=" + p1 + " align=" + p2 + ">" + p3 + "</th>  ").Trim();
}
public static string ListTitleTd6_JS(string p0)
{
return ("        function listtitletd6_" + p0 + "(s1,s2,s3,s4) { return '<th name=\"listth\" " + setStyle("csslisttitletd6") + " style=\"width:'+s1+'\" width=\"'+s2+'\" align=\"'+s3+'\">'+s4+'</th>'}  ").Trim();
}
public static string ListTitleTd7(string p0,string p1,string p2,string p3)
{
return ("  <th name=\"listth\" " + setStyle("csslisttitletd7") + " style=\"width:" + p0 + "\" width=" + p1 + " align=" + p2 + ">" + p3 + "</th>  ").Trim();
}
public static string ListTitleTd7_JS(string p0)
{
return ("        function listtitletd7_" + p0 + "(s1,s2,s3,s4) { return '<th name=\"listth\" " + setStyle("csslisttitletd7") + " style=\"width:'+s1+'\" width=\"'+s2+'\" align=\"'+s3+'\">'+s4+'</th>'}  ").Trim();
}
public static string ListTitleTd8(string p0,string p1,string p2,string p3)
{
return ("  <th name=\"listth\" " + setStyle("csslisttitletd8") + " style=\"width:" + p0 + "\" width=" + p1 + " align=" + p2 + ">" + p3 + "</th>  ").Trim();
}
public static string ListTitleTd8_JS(string p0)
{
return ("        function listtitletd8_" + p0 + "(s1,s2,s3,s4) { return '<th name=\"listth\" " + setStyle("csslisttitletd8") + " style=\"width:'+s1+'\" width=\"'+s2+'\" align=\"'+s3+'\">'+s4+'</th>'}  ").Trim();
}
public static string ListTitleTd9(string p0,string p1,string p2,string p3)
{
return ("  <th name=\"listth\" " + setStyle("csslisttitletd9") + " style=\"width:" + p0 + "\" width=" + p1 + " align=" + p2 + ">" + p3 + "</th>  ").Trim();
}
public static string ListTitleTd9_JS(string p0)
{
return ("        function listtitletd9_" + p0 + "(s1,s2,s3,s4) { return '<th name=\"listth\" " + setStyle("csslisttitletd9") + " style=\"width:'+s1+'\" width=\"'+s2+'\" align=\"'+s3+'\">'+s4+'</th>'}  ").Trim();
}
public static string ListTitleTd10(string p0,string p1,string p2,string p3)
{
return ("  <th name=\"listth\" " + setStyle("csslisttitletd10") + " style=\"width:" + p0 + "\" width=" + p1 + " align=" + p2 + ">" + p3 + "</th>  ").Trim();
}
public static string ListTitleTd10_JS(string p0)
{
return ("        function listtitletd10_" + p0 + "(s1,s2,s3,s4) { return '<th name=\"listth\" " + setStyle("csslisttitletd10") + " style=\"width:'+s1+'\" width=\"'+s2+'\" align=\"'+s3+'\">'+s4+'</th>'}  ").Trim();
}
public static string ListTitleTd11(string p0,string p1,string p2,string p3)
{
return ("  <th name=\"listth\" " + setStyle("csslisttitletd11") + " style=\"width:" + p0 + "\" width=" + p1 + " align=" + p2 + ">" + p3 + "</th>  ").Trim();
}
public static string ListTitleTd11_JS(string p0)
{
return ("        function listtitletd11_" + p0 + "(s1,s2,s3,s4) { return '<th name=\"listth\" " + setStyle("csslisttitletd11") + " style=\"width:'+s1+'\" width=\"'+s2+'\" align=\"'+s3+'\">'+s4+'</th>'}  ").Trim();
}
public static string ListTitleTd12(string p0,string p1,string p2,string p3)
{
return ("  <th name=\"listth\" " + setStyle("csslisttitletd12") + " style=\"width:" + p0 + "\" width=" + p1 + " align=" + p2 + ">" + p3 + "</th>  ").Trim();
}
public static string ListTitleTd12_JS(string p0)
{
return ("        function listtitletd12_" + p0 + "(s1,s2,s3,s4) { return '<th name=\"listth\" " + setStyle("csslisttitletd12") + " style=\"width:'+s1+'\" width=\"'+s2+'\" align=\"'+s3+'\">'+s4+'</th>'}  ").Trim();
}
public static string ListTitleTdOther(string p0,string p1,string p2,string p3)
{
return ("  <th name=\"listth\" " + setStyle("csslisttitletdother") + " style=\"width:" + p0 + "\" width=" + p1 + " align=" + p2 + ">" + p3 + "</th>  ").Trim();
}
public static string ListTitleTdOther_JS(string p0)
{
return ("        function listtitletdother_" + p0 + "(s1,s2,s3,s4) { return '<th name=\"listth\" " + setStyle("csslisttitletdother") + " style=\"width:'+s1+'\" width=\"'+s2+'\" align=\"'+s3+'\">'+s4+'</th>'}  ").Trim();
}
public static string ListTr0(string p0,string p1,string p2)
{
return ("  <tr " + setStyle("csslisttr0") + " onmouseover=\"if(this.children[0].children[0]==null||!this.children[0].children[0].checked)this.style.background='" + p0 + "'\" onmouseout=\"if(this.children[0].children[0]==null||!this.children[0].children[0].checked)this.style.background='" + p1 + "'\">" + p2 + "</tr>  ").Trim();
}
public static string ListTr0_JS(string p0,string p1,string p2)
{
return ("  function listtr0_" + p0 + "(s) { return '<tr " + setStyle("csslisttr0") + " onmouseover=\"if(this.children[0].children[0]==null||!this.children[0].children[0].checked)this.style.background=" + p1 + "\" onmouseout=\"if(this.children[0].children[0]==null||!this.children[0].children[0].checked)this.style.background=" + p2 + "\">'+s+'</tr>'; }  ").Trim();
}
public static string ListTr1(string p0,string p1,string p2)
{
return ("  <tr " + setStyle("csslisttr1") + " onmouseover=\"if(this.children[0].children[0]==null||!this.children[0].children[0].checked)this.style.background='" + p0 + "'\" onmouseout=\"if(this.children[0].children[0]==null||!this.children[0].children[0].checked)this.style.background='" + p1 + "'\">" + p2 + "</tr>  ").Trim();
}
public static string ListTr1_JS(string p0,string p1,string p2)
{
return ("  function listtr1_" + p0 + "(s) { return '<tr " + setStyle("csslisttr1") + " onmouseover=\"if(this.children[0].children[0]==null||!this.children[0].children[0].checked)this.style.background=" + p1 + "\" onmouseout=\"if(this.children[0].children[0]==null||!this.children[0].children[0].checked)this.style.background=" + p2 + "\">'+s+'</tr>'; }  ").Trim();
}
public static string ListTrTdDataBlock(string p0,string p1,int p2,string p3)
{
return ("  <tr " + setStyle("csslisttr1") + " onmouseover=\"if(this.children[0].children[0]==null||!this.children[0].children[0].checked)this.style.background=" + p0 + "\" onmouseout=\"if(this.children[0].children[0]==null||!this.children[0].children[0].checked)this.style.background=" + p1 + "\"><td " + setStyle("csslisttd1") + " colspan=\"" + p2 + "\">" + p3 + "</td></tr>  ").Trim();
}
public static string ListTrTdDataBlock_JS(string p0,string p1,string p2)
{
return ("        function listtrtddatablock_" + p0 + "(s1,s2) { return '<tr " + setStyle("csslisttr1") + " onmouseover=\"if(this.children[0].children[0]==null||!this.children[0].children[0].checked)this.style.background=" + p1 + "\" onmouseout=\"if(this.children[0].children[0]==null||!this.children[0].children[0].checked)this.style.background=" + p2 + "\"><td " + setStyle("csslisttd1") + " colspan=\"'+s1+'\">'+s2+'</td></tr>'; }  ").Trim();
}
public static string ListTrCacu(string p0)
{
return ("  <tr " + setStyle("csslisttrcacu") + ">" + p0 + "</tr>  ").Trim();
}
public static string ListTrCacu_JS(string p0)
{
return ("  function listtrcacu_" + p0 + "(s) { return '<tr " + setStyle("csslisttrcacu") + ">'+s+'</tr>'; }  ").Trim();
}
public static string ListTd0(string p0)
{
return ("  <td " + setStyle("cssselecttd") + " width=14px align=center>" + p0 + "</td>  ").Trim();
}
public static string ListTd0num_JS_None(string p0)
{
return ("  function listtd0num_" + p0 + "(num) {   return ''; }  ").Trim();
}
public static string ListTd0num_JS_Num(string p0)
{
return ("  function listtd0num_" + p0 + "(num) {   return '<td " + setStyle("cssselecttd") + " width=14px align=center>'+num+'</td>'; }  ").Trim();
}
public static string ListTd0_JS_None_Inner(string p0)
{
return ("  function listtd0inner_" + p0 + "(id) {   return ''; }  ").Trim();
}
public static string ListTd0_JS_Check_Inner(string p0,string p1)
{
return ("  function listtd0inner_" + p0 + "(id) {   return '<input type=\"checkbox\" name=\"dlcheckradio\" value=\"'+id+'\" onclick=\"" + p1 + "\">'; }  ").Trim();
}
public static string ListTd0_JS_Radio_Inner(string p0)
{
return ("  function listtd0inner_" + p0 + "(id) {   return '<input type=\"radio\" name=\"dlcheckradio\" value=\"'+id+'\">'; }  ").Trim();
}
public static string ListTd0_JS_None(string p0)
{
return ("  function listtd0_" + p0 + "(id) {   return ''; }  ").Trim();
}
public static string ListTd0_JS_Check(string p0,string p1)
{
return ("  function listtd0_" + p0 + "(id) {   return '<td " + setStyle("cssselecttd") + " width=14px align=center><input type=\"checkbox\" name=\"dlcheckradio\" value=\"'+id+'\" onclick=\"" + p1 + "\"></td>'; }  ").Trim();
}
public static string ListTd0_JS_Radio(string p0)
{
return ("  function listtd0_" + p0 + "(id) {   return '<td " + setStyle("cssselecttd") + " width=14px align=center><input type=\"radio\" name=\"dlcheckradio\" value=\"'+id+'\"></td>'; }  ").Trim();
}
public static string ListTd1(string p0,string p1)
{
return ("  <td " + setStyle("csslisttd1") + " align=" + p0 + ">" + p1 + "</td>  ").Trim();
}
public static string ListTd1_JS(string p0)
{
return ("  function listtd1_" + p0 + "(a,s) {   return '<td " + setStyle("csslisttd1") + " align='+a+'>'+s+'</td>'; }  ").Trim();
}
public static string ListTd2(string p0,string p1)
{
return ("  <td " + setStyle("csslisttd2") + " align=" + p0 + ">" + p1 + "</td>  ").Trim();
}
public static string ListTd2_JS(string p0)
{
return ("  function listtd2_" + p0 + "(a,s) {   return '<td " + setStyle("csslisttd2") + " align='+a+'>'+s+'</td>'; }  ").Trim();
}
public static string ListTd3(string p0,string p1)
{
return ("  <td " + setStyle("csslisttd3") + " align=" + p0 + ">" + p1 + "</td>  ").Trim();
}
public static string ListTd3_JS(string p0)
{
return ("  function listtd3_" + p0 + "(a,s) {   return '<td " + setStyle("csslisttd3") + " align='+a+'>'+s+'</td>'; }  ").Trim();
}
public static string ListTd4(string p0,string p1)
{
return ("  <td " + setStyle("csslisttd4") + " align=" + p0 + ">" + p1 + "</td>  ").Trim();
}
public static string ListTd4_JS(string p0)
{
return ("  function listtd4_" + p0 + "(a,s) {   return '<td " + setStyle("csslisttd4") + " align='+a+'>'+s+'</td>'; }  ").Trim();
}
public static string ListTd5(string p0,string p1)
{
return ("  <td " + setStyle("csslisttd5") + " align=" + p0 + ">" + p1 + "</td>  ").Trim();
}
public static string ListTd5_JS(string p0)
{
return ("  function listtd5_" + p0 + "(a,s) {   return '<td " + setStyle("csslisttd5") + " align='+a+'>'+s+'</td>'; }  ").Trim();
}
public static string ListTd6(string p0,string p1)
{
return ("  <td " + setStyle("csslisttd6") + " align=" + p0 + ">" + p1 + "</td>  ").Trim();
}
public static string ListTd6_JS(string p0)
{
return ("  function listtd6_" + p0 + "(a,s) {   return '<td " + setStyle("csslisttd6") + " align='+a+'>'+s+'</td>'; }  ").Trim();
}
public static string ListTd7(string p0,string p1)
{
return ("  <td " + setStyle("csslisttd7") + " align=" + p0 + ">" + p1 + "</td>  ").Trim();
}
public static string ListTd7_JS(string p0)
{
return ("  function listtd7_" + p0 + "(a,s) {   return '<td " + setStyle("csslisttd7") + " align='+a+'>'+s+'</td>'; }  ").Trim();
}
public static string ListTd8(string p0,string p1)
{
return ("  <td " + setStyle("csslisttd8") + " align=" + p0 + ">" + p1 + "</td>  ").Trim();
}
public static string ListTd8_JS(string p0)
{
return ("  function listtd8_" + p0 + "(a,s) {   return '<td " + setStyle("csslisttd8") + " align='+a+'>'+s+'</td>'; }  ").Trim();
}
public static string ListTd9(string p0,string p1)
{
return ("  <td " + setStyle("csslisttd9") + " align=" + p0 + ">" + p1 + "</td>  ").Trim();
}
public static string ListTd9_JS(string p0)
{
return ("  function listtd9_" + p0 + "(a,s) {   return '<td " + setStyle("csslisttd9") + " align='+a+'>'+s+'</td>'; }  ").Trim();
}
public static string ListTd10(string p0,string p1)
{
return ("  <td " + setStyle("csslisttd10") + " align=" + p0 + ">" + p1 + "</td>  ").Trim();
}
public static string ListTd10_JS(string p0)
{
return ("  function listtd10_" + p0 + "(a,s) {   return '<td " + setStyle("csslisttd10") + " align='+a+'>'+s+'</td>'; }  ").Trim();
}
public static string ListTd11(string p0,string p1)
{
return ("  <td " + setStyle("csslisttd11") + " align=" + p0 + ">" + p1 + "</td>  ").Trim();
}
public static string ListTd11_JS(string p0)
{
return ("  function listtd11_" + p0 + "(a,s) {   return '<td " + setStyle("csslisttd11") + " align='+a+'>'+s+'</td>'; }  ").Trim();
}
public static string ListTd12(string p0,string p1)
{
return ("  <td " + setStyle("csslisttd12") + " align=" + p0 + ">" + p1 + "</td>  ").Trim();
}
public static string ListTd12_JS(string p0)
{
return ("  function listtd12_" + p0 + "(a,s) {   return '<td " + setStyle("csslisttd12") + " align='+a+'>'+s+'</td>'; }  ").Trim();
}
public static string ListTdother(string p0,string p1)
{
return ("  <td " + setStyle("csslisttdother") + " align=" + p0 + ">" + p1 + "</td>  ").Trim();
}
public static string ListTdother_JS(string p0)
{
return ("  function listtdother_" + p0 + "(a,s) {   return '<td " + setStyle("csslisttdother") + " align='+a+'>'+s+'</td>'; }  ").Trim();
}
public static string TurnPageTb(int p0,int p1,string p2)
{
return ("  <table cellspacing=" + p0 + " cellpadding=" + p1 + " " + setStyle("cssturnpagetb") + " width=\"100%\"> <tr " + setStyle("cssturnpagetr") + "><td align=right " + setStyle("cssturnpagetd") + ">" + p2 + "</td></tr> </table>  ").Trim();
}
public static string TurnPageTb_JS(string p0,int p1,int p2)
{
return ("  function listturnpage_" + p0 + "(s) {   var val='<table cellspacing=" + p1 + " cellpadding=" + p2 + " " + setStyle("cssturnpagetb") + " width=\"100%\">';   val+='<tr " + setStyle("cssturnpagetr") + "><td align=right " + setStyle("cssturnpagetd") + ">'+s+'</td></tr>';   val+=\"</table>\";return val; }  ").Trim();
}
public static string TurnPageTbTop(int p0,int p1,string p2)
{
return ("  <table cellspacing=" + p0 + " cellpadding=" + p1 + " " + setStyle("cssturnpagetbtop") + " width=\"100%\"> <tr " + setStyle("cssturnpagetrtop") + "><td align=right " + setStyle("cssturnpagetdtop") + ">" + p2 + "</td></tr> </table>  ").Trim();
}
public static string TurnPageTbTop_JS(string p0,int p1,int p2)
{
return ("  function listturnpagetop_" + p0 + "(s) {   var val='<table cellspacing=" + p1 + " cellpadding=" + p2 + " " + setStyle("cssturnpagetbtop") + " width=\"100%\">';   val+='<tr " + setStyle("cssturnpagetrtop") + "><td align=right " + setStyle("cssturnpagetdtop") + ">'+s+'</td></tr>';   val+=\"</table>\";return val; }  ").Trim();
}
public static string TurnPageTb0(int p0,int p1,string p2)
{
return ("  <table cellspacing=" + p0 + " cellpadding=" + p1 + " " + setStyle("cssturnpagetb") + " width=\"100%\"> <tr " + setStyle("cssturnpagetr") + "><td align=center " + setStyle("cssturnpagetd") + ">" + p2 + "</td></tr> </table>  ").Trim();
}
public static string TurnPageInput(int p0,string p1)
{
return ("  <input type=text value=\"" + p0 + "\" id=\"t_" + p1 + "\" " + setStyle("cssturnpageinput") + "/>  ").Trim();
}
public static string TurnPageInput_JS(string p0,string p1)
{
return ("  function listturnpageinput_" + p0 + "(s) {   return '<input type=text value=\"'+s+'\" id=\"t_" + p1 + "\" " + setStyle("cssturnpageinput") + "/>'; }  ").Trim();
}
public static string TurnPageSelect(string p0)
{
return ("  <select id=\"ts_" + p0 + "\" " + setStyle("cssturnpageselect") + "><option value='-1'>默认</option><option value='20'>20</option><option value='30'>30</option><option value='50'>50</option><option value='100'>100</option><option value='200'>200</option><option value='500'>500</option><option value='1000'>1000</option><!--option value='0'>全部</option--></select>  ").Trim();
}
public static string TurnPageSelect_JS(string p0,string p1,string p2)
{
return ("   function listturnpageselect_" + p0 + "() { return '<select id=\"ts_" + p1 + "\" onchange=\"dl_search(\\'" + p2 + "\\')\" " + setStyle("cssturnpageselect") + "><option value=\"-1\">默认</option><option value=\"20\">20</option><option value=\"30\">30</option><option value=\"50\">50</option><option value=\"100\">100</option><option value=\"200\">200</option><option value=\"500\">500</option><option value=\"1000\">1000</option><!--option value=\"0\">全部</option--></select>'; }  ").Trim();
}
public static string TurnPageButton(string p0,string p1,string p2)
{
return ("  <input type=button value=\"" + p0 + "\" onclick=\"dl_gotopage('" + p1 + "',$('#t_" + p2 + "').val())\"/>  ").Trim();
}
public static string TextLinkDemoStyle2(string p0,string p1,string p2)
{
return (" <button class='_button' type='button' id=\"" + p0 + "\" " + p1 + ">" + p2 + "</button> ").Trim();
}
public static string ButtonStyleInitJs(string p0,string p1)
{
return (" $(\"#\"+\"" + p0 + "\").button({icons: {primary: \"" + p1 + "\"}}).parent().buttonset(); ").Trim();
}
public static string ButtonStyleInitJs_Main(string p0)
{
return (" <script language=\"javascript\">$(function(){" + p0 + "});</script> ").Trim();
}
public static string menujsframe(string p0,string p1,string p2,string p3,string p4,string p5,string p6,string p7,string p8,string p9,string p10,string p11,string p12,string p13,string p14,string p15,string p16,string p17,string p18,string p19,string p20,string p21,string p22,string p23,string p24,string p25,string p26)
{
return ("  <span style=\"display:none\"><iframe name=\"dhf" + p0 + "\" scrolling=\"no\" frameborder=\"0\" width=\"0\" height=\"0\"></iframe><form id=\"dlf" + p1 + "\" method=post><input type=hidden name=\"d_membind\" value=\"" + p2 + "\"/><input type=hidden name=\"d_elecdt\" value=\"" + p3 + "\"/><input type=hidden name=\"d_roledata\" value=\"" + p4 + "\"/><input type=hidden name=\"d_rolesession\" value=\"" + p5 + "\"/><input type=hidden name=\"d_authrule\" value=\"" + p6 + "\"/><input type=hidden name=\"d_formstat\" value=\"" + p7 + "\"/><input type=hidden name=\"d_opid\" value=\"" + p8 + "\"/><input type=hidden name=\"c_membind\" value=\"" + p9 + "\"/><input type=hidden name=\"c_elecdt\" value=\"" + p10 + "\"/><input type=hidden name=\"c_roledata\" value=\"" + p11 + "\"/><input type=hidden name=\"c_rolesession\" value=\"" + p12 + "\"/><input type=hidden name=\"c_authrule\" value=\"" + p13 + "\"/><input type=hidden name=\"c_formstat\" value=\"" + p14 + "\"/><input type=hidden name=\"c_opid\" value=\"" + p15 + "\"/><input type=hidden name=\"f_membind\" value=\"" + p16 + "\"/><input type=hidden name=\"f_elecdt\" value=\"" + p17 + "\"/><input type=hidden name=\"f_roledata\" value=\"" + p18 + "\"/><input type=hidden name=\"f_rolesession\" value=\"" + p19 + "\"/><input type=hidden name=\"f_authrule\" value=\"" + p20 + "\"/><input type=hidden name=\"f_formstat\" value=\"" + p21 + "\"/><input type=hidden name=\"f_opid\" value=\"" + p22 + "\"/><input type=hidden name=\"siteid\"/><input type=hidden name=\"ftformstat\"/><input type=hidden name=\"ftformflow\"/><input type=hidden name=\"optype\"/><input type=hidden name=\"ftformid\"/><input type=hidden name=\"tabletag\" value=\"" + p23 + "\"/><input type=hidden name=\"curpartid\" value=\"\"/><input type=hidden name=\"exportstr\" value=\"\"/><input type=hidden name=\"d_code\" value=\"" + p24 + "\"/><input type=hidden name=\"c_code\" value=\"" + p25 + "\"/><input type=hidden name=\"f_code\" value=\"" + p26 + "\"/></form></span> 		 ").Trim();
}
public static string Tableframe(string p0,string p1)
{
return (" <table " + setStyle("menutablestyle") + " width=\"100%\"><tr " + setStyle("menurowstyle") + "><td " + setStyle("menucolstyle") + ">" + p0 + "</td><td " + setStyle("menucolsearchstyle") + ">" + p1 + "</td></tr></table> ").Trim();
}
public static string SaveColInput(string p0,string p1)
{
return ("  <input type=text name=\"" + p0 + "\" value=\"" + p1 + "\" " + setStyle("savecolstyle") + "/>  ").Trim();
}
public static string SaveColInput_JS(string p0)
{
return ("   function savecolinput_" + p0 + "(s,v) { return '<input type=text name=\"'+s+'\" value=\"'+v+'\" " + setStyle("savecolstyle") + "/>'; }  ").Trim();
}
public static string SaveColIds(string p0)
{
return ("  <input type=hidden name=\"dlidsall\" value=\"" + p0 + "\"/>  ").Trim();
}
public static string FormStart(string p0,string p1)
{
return ("  <form id=\"dli" + p0 + "\" method=\"post\" style=\"margin:0px;padding:0px\"> <input type=\"hidden\" name=\"tabletag\"/> <input type=\"hidden\" name=\"siteid\"/> <input type=\"hidden\" name=\"optype\"/> <input type=hidden name=\"savetabletag\" value=\"\"/> <span id=\"data_" + p1 + "\">  ").Trim();
}
public static string FormEnd()
{
return ("  </span></form>  ").Trim();
}
public static string ajax(string p0,string p1,string p2,string p3,string p4,string p5,string p6,string p7,string p8,string p9,string p10,string p11,string p12,string p13,string p14,string p15,string p16,string p17,string p18,string p19,string p20,string p21,string p22,int p23,int p24,string p25,string p26,int p27,string p28,string p29,string p30,string p31,string p32,string p33,string p34,int p35,string p36,string p37,string p38,string p39,string p40,string p41,string p42,int p43,string p44)
{
return ("  var list_title_apd_" + p0 + "=\"" + p1 + "\";var list_adv_s_" + p2 + "=\"\";var list_adv_c_" + p3 + "=\"\";var list_curpage_" + p4 + "=1; function load_" + p5 + "(page,orderby,ordertype,isexport,isexportsave,esfilecap,_cq) {  dl_load(page,orderby,ordertype,isexport,isexportsave,esfilecap,_cq,\"" + p6 + "\",\"" + p7 + "\",\"" + p8 + "\",\"" + p9 + "\"," + p10 + "\"\"," + p11 + "\"\",\"" + p12 + "\",\"" + p13 + "\",\"" + p14 + "\",\"" + p15 + "\",\"" + p16 + "\",\"" + p17 + "\",\"" + p18 + "\"," + p19 + "," + p20 + "," + p21 + ",\"" + p22 + "\"," + p23 + "," + p24 + ",\"" + p25 + "\",\"" + p26 + "\"," + p27 + ",\"" + p28 + "\",\"" + p29 + "\",\"" + p30 + "\",\"" + p31 + "\",\"" + p32 + "\",\"" + p33 + "\",\"" + p34 + "\"," + p35 + "," + p36 + "," + p37 + ",\"" + p38 + "\",\"" + p39 + "\",\"" + p40 + "\",\"" + p41 + "\",\"" + p42 + "\"); } if(" + p43 + "==1)load_" + p44 + "(1); 		 ").Trim();
}
}
}
