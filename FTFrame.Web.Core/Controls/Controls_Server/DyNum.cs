using System;
using System.Web;
using System.Data;
using System.Xml;
using System.Collections;
using FTFrame;
using FTFrame.Tool;
using FTFrame.DBClient;
using FTFrame.Server.Core;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using static FTFrame.Enums;
using System.IO;
using FTFrame.Server.Core.Tool;

namespace FT.DyNum.Back
{
}
namespace FT.DyNum
{
    public class ftdpcontrolsql
    {
    }
	public class ftdpcontroltemp
	{
		public static string input(string p0, string p1, string p2, string p3, string p4, string p5, string p6)
		{
			return ("<input type=hidden name=\"ftform_liquid_" + p0 + "\" id=\"ftform_liquid_" + p1 + "\" value=\"" + p2 + "&&" + p3 + "&&" + p4 + "&&" + p5 + "&&" + p6 + "\"/>").Trim();
		}

		public static string ClientView(string p0)
		{
			return ("<img src=\"" + p0 + "\" border=\"0\">").Trim();
		}
	}

}
namespace FT.DyNum.Fore
{
    public class client
    {
        public static string thisControlID = "";
        public static object[] thisStyleObject;
    }
    public class Func
    {
    }
    public class DyNum
	{
        public static void Output(HttpContext Context, TextWriter output, string SiteID, string ControlName, string ControlID, string PartID, string DataSource, string SetStyle, string ControlPara,string CurPage,string Special,string Patten,string TableName,string ColName,string LockLike)
        {
			ControlPara = ControlPara.Replace("{dsqt}", "\"");
			CurPage = CurPage.Replace("{dsqt}", "\"");
			Special = Special.Replace("{dsqt}", "\"");
			Patten = Patten.Replace("{dsqt}", "\"");
			TableName = TableName.Replace("{dsqt}", "\"");
			ColName = ColName.Replace("{dsqt}", "\"");
			LockLike = LockLike.Replace("{dsqt}", "\"");
			client.thisControlID = ControlID;
			string[] array = SetStyle.Split('{');
			int num = array.Length;
			client.thisStyleObject = new object[num];
			for (int i = 0; i < num; i++)
			{
				string[] array2 = array[i].Split('}');
				client.thisStyleObject[i] = array2;
			}
			output.Write(ftdpcontroltemp.input(PartID, PartID, Patten, TableName, ColName, LockLike, Special));
		}
    }
}
