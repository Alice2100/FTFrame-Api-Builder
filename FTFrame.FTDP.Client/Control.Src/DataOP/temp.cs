/*FTDP Control,This Code Created By FTDP Deploy Tool
Build By Maobb 2005-7-21
Code Deploy Time is:2020-06-05 09:49:47*/
using System;
using FT.Dataop.Fore;
namespace FT.Dataop
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
public static string ClientView(string p0,string p1)
{
return ("  		<img src=\"" + p0 + "\" border=\"0\"/>&nbsp;<button>" + p1 + "</button> 		 ").Trim();
}
public static string ClientView2(string p0)
{
return ("  		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button>" + p0 + "</button> 		 ").Trim();
}
}
}
