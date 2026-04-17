using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;
using System.Xml;
using System.Collections;
using CoreHttp = Microsoft.AspNetCore.Http;
using FTFrame.Project.Core.Utils;

namespace FTFrame.Project.Core
{
    /// <summary>
    /// Define @code rules and call them using @code syntax in client configuration
    /// Example:@code(str.SnowId),@code(sql.GetUser,[userId]),@code(sql.IdAge,@p1@|32)
    /// </summary>
    public partial class Code
    {
        private static object _obj(string code, string[] para, Dictionary<string, object> reqDic, HttpContext Context)
        {
            return null;
        }
        private static string _str(string code, string[] para, Dictionary<string, object> reqDic, HttpContext Context)
        {
            switch (code)
            {
                case "SnowId":// Snow Id
                    if (para.Length > 0 && para[0] == "static")
                    {
                        var staticKey = "_snowid_static_" + (para.Length > 1 ? para[1] : "");
                        if (!Context.Request.Headers.ContainsKey(staticKey))
                        {
                            Context.Request.Headers.Add(staticKey, Utils.Str.SnowId().ToString());
                        }
                        return Context.Request.Headers[staticKey];
                    }
                    else return Utils.Str.SnowId().ToString();
                case "Guid":// Guid
                    if (para.Length > 0 && para[0] == "static")
                    {
                        var staticKey = "_guid_static_" + (para.Length > 1 ? para[1] : "");
                        if (!Context.Request.Headers.ContainsKey(staticKey))
                        {
                            Context.Request.Headers.Add(staticKey, str.GetCombID());
                        }
                        return Context.Request.Headers[staticKey];
                    }
                    else return str.GetCombID();
                case "Now": return str.GetDateTime();//current time
                case "Decode": return Str.Decode(para[0]);//des decrypt
                case "Encode": return Str.Encode(para[0]);//des encrypt
                case "Html2Text": return str.GetSafeCode(para[0]);//html to text
                case "SerialNumber": //Random serial number,param 1 is length
                    var date = DateTime.Now;
                    int len = (para.Length > 0 && para[0].Trim() != "") ? int.Parse(para[0]) : 4;
                    int max = Math.Pow(10, len).ToInt32();
                    string apd = "";
                    if (para.Length > 1 && para[1].Trim() != "") apd = para[1].Trim();
                    return apd + date.ToString("yyyyMMddHHmmssfff") + (max * 10 - (new Random().Next(1, max - 1))).ToString().Substring(1);
                case "IdAge"://Age according to ID number
                    if (para[0].Length == 18)
                    {
                        if (int.TryParse(para[0].Substring(6, 4), out int year))
                        {
                            return (DateTime.Now.Year - year).ToString();
                        }
                    }
                    else if (para[0].Length == 15)
                    {
                        if (int.TryParse("19" + para[0].Substring(6, 2), out int year))
                        {
                            return (DateTime.Now.Year - year).ToString();
                        }
                    }
                    return "0";
                case "TurnPage": return Func.TurnPageHTML_LayUI(para);//page turning html for layui style
                case "TurnPageVue": return Func.TurnPageHTML_Vue(para);//page turning html for vue style
                case "DateFormat":// format date
                    DateTime dt = DateTime.Now;
                    if (DateTime.TryParse(para[0], out dt))
                    {
                        return str.GetDateTimeCustom(dt, para[1]);
                    }
                    else return para[0];

            }
            //others
            if (code.Equals("SetPay"))//Conversion amount
            {
                var result = Convert.ToDouble(para[0]);
                result = result / 100;
                return result.ToString();
            }
            else if (code.Equals("DateLeft"))//How many days are left until now
            {
                DateTime dt = DateTime.Now;
                if (DateTime.TryParse(para[0], out dt))
                {
                    return str.GetTimeSpanDays(dt, DateTime.Now).ToString();
                }
                else return "0";
            }
            else if (code.Equals("DateSpanDays"))//How many days do the dates differ
            {
                DateTime dt1 = DateTime.Now;
                DateTime dt2 = DateTime.Now;
                if (DateTime.TryParse(para[0], out dt1) && DateTime.TryParse(para[1], out dt2))
                {
                    return str.GetTimeSpanDays(dt1, dt2).ToString();
                }
                else return "0";
            }
            else if (code.Equals("DatePassYears"))//number of years
            {
                DateTime dt1 = DateTime.Now;
                DateTime dt2 = DateTime.Now;
                if (DateTime.TryParse(para[0], out dt1) && DateTime.TryParse(para[1], out dt2))
                {
                    decimal y = 0;
                    while (dt1.AddYears(1) <= dt2.AddDays(1))
                    {
                        y = y + 1;
                        dt1 = dt1.AddYears(1);
                    }
                    return (y + (Convert.ToDecimal(((TimeSpan)(dt2.AddDays(1) - dt1)).TotalDays) / 365)).ToString("0.00");
                }
                else return "0";
            }
            else if (code.Equals("Rate"))//Percentage growth rate
            {
                decimal d1 = 0;
                decimal d2 = 0;
                if (decimal.TryParse(para[0], out d1) && decimal.TryParse(para[1], out d2))
                {
                    return d2 == 0 ? "0" : ((100 * (d1 - d2) / d2).ToString("0.00") + "%");
                }
                else return "";
            }
            else if (code.Equals("RateDiv"))//Percentage rate
            {
                decimal d1 = 0;
                decimal d2 = 0;
                if (decimal.TryParse(para[0], out d1) && decimal.TryParse(para[1], out d2))
                {
                    return d2 == 0 ? "0" : ((100 * d1 / d2).ToString("0.00") + "%");
                }
                else return "";
            }
            else if (code.Equals("Minus"))//subtract
            {
                decimal d1 = 0;
                decimal d2 = 0;
                if (decimal.TryParse(para[0], out d1) && decimal.TryParse(para[1], out d2))
                {
                    return d2 == 0 ? "0" : ((d1 - d2).ToString("0.00"));
                }
                else return "";
            }
            else if (code.Equals("DecimalFormat"))//decimal format to string
            {
                return decimal.Parse(para[0]).ToString("0.00");
            }
            return "(nocode)";
        }
        private static string _sql(string code, string[] para, Dictionary<string, object> reqDic, HttpContext Context)
        {
            switch(code)
            {

            }
            return "(nocode)";
        }
        private static string _pro(string code, string[] para, Dictionary<string, object> reqDic, HttpContext Context)
        {
            switch (code)
            {
                case "ApiSchStrict":
                    string str = "";
                    str += "null;All|";
                    str += "Demo:@Cdn{" + Str.Encode("ApiPath like '/demo/%'") + "};";
                    str += "Flow:@Cdn{" + Str.Encode("ApiPath like '/api/Flow/%'") + "};";
                    str += "User:@Cdn{" + Str.Encode("ApiPath like '/api/User/%'") + "}";
                    return str;
                case "LiquidCode":
                    string liquid_table = para[0];
                    string liquid_col = para[1];
                    string liquid_patten = para[2];
                    string liquid_locklike = para[3];
                    return Advance.GetLiquidID(liquid_table, liquid_col, liquid_patten, liquid_locklike);
            }
            return "(nocode)";
        }
        private static string _oa(string code, string[] para, Dictionary<string, object> reqDic, HttpContext Context)
        {
            switch (code)
            {

            }
            return null;
        }
    }
    public class Dic
    {
        public static string Get(string name, string key)
        {
            return EnumDic.GetValueOrDefault(name)?.GetValueOrDefault(key);
        }
        private static Dictionary<string, Dictionary<string, string>> EnumDic = new Dictionary<string, Dictionary<string, string>>() {
            { "Test",new Dictionary<string, string>(){
                {"Level1","1" },{"Level2","2" },{"Level3","3" }
            }
          }
        };
    }
    public class Info
    {
        public static string Get(string InfoDefine, Dictionary<string, object> reqDic, HttpContext context)
        {
            if (InfoDefine.StartsWith("@code(")) return Code.Get(InfoDefine, reqDic, context).ToString();
            switch (InfoDefine)
            {
            }
            return "Undefined Info";
        }
    }
}
