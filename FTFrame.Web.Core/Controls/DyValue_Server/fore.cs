/*FTDP Control,This Code Created By FTDP Deploy Tool
Build By Maobb 2007-6-10
Code Deploy Time is:2020-06-05 09:54:53*/
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
using FTFrame.Project.Core.Utils;

namespace FT.DyValue.Fore
{
    public class client
    {
        public static string thisControlID = "";
        public static object[] thisStyleObject;
    }
    public class DyValue
    {
        public static void Output(HttpContext Context,TextWriter output,string SiteID, string ControlName, string ControlID, string PartID, string DataSource, string SetStyle, string ControlPara, string Define, string FuncName, string AppendJS0, string AppendJS1, string NewSiteID, string YBAppendJS0, string YBAppendJS1, string DefaultFID, string APISet, string FidCol, int OpDefaultCol, string ExecBefore = "", string ExecAfter = "", string CustomConnection="")
        {
            ControlPara = ControlPara.Replace("{dsqt}", "\"");
            Define = Define.Replace("{dsqt}", "\"");
            FuncName = FuncName.Replace("{dsqt}", "\"");
            AppendJS0 = AppendJS0.Replace("{dsqt}", "\"");
            AppendJS1 = AppendJS1.Replace("{dsqt}", "\"");
            NewSiteID = NewSiteID.Replace("{dsqt}", "\"");
            YBAppendJS0 = YBAppendJS0.Replace("{dsqt}", "\"");
            YBAppendJS1 = YBAppendJS1.Replace("{dsqt}", "\"");
            DefaultFID = DefaultFID.Replace("{dsqt}", "\"");
            if (string.IsNullOrWhiteSpace(DefaultFID)) DefaultFID = "@p1@";
            client.thisControlID = ControlID;
            string[] StyleStringArray = SetStyle.Split('{');
            int StyleStringArrayi;
            int StyleStringArrayLength = StyleStringArray.Length;
            client.thisStyleObject = new object[StyleStringArrayLength];
            for (StyleStringArrayi = 0; StyleStringArrayi < StyleStringArrayLength; StyleStringArrayi++)
            {
                string[] StyleStringArrayOne = StyleStringArray[StyleStringArrayi].Split('}');
                client.thisStyleObject[StyleStringArrayi] = StyleStringArrayOne;
            }

            try
            {
                ForeFrameType ForeFrame = (ForeFrameType)Enum.Parse(typeof(ForeFrameType), ControlPara);
                string s = "";
                string js = "";
                ArrayList TongBuAl = new ArrayList();
                ArrayList YiBuAl = new ArrayList();
                string[] rows = Define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                string _SiteID = (NewSiteID.Equals("") ? SiteID : NewSiteID);
                string _DefaultFID = adv.GetSpecialBase(Context, DefaultFID, NewSiteID);
                for (int i = 0; i < rows.Length; i++)
                {
                    string[] item = rows[i].Split(new string[] { "##" }, StringSplitOptions.None);
                    string fid = adv.GetSpecialBase(Context, item[3], _SiteID).Trim();
                    if (fid.Equals("")) fid = _DefaultFID;
                    string[] newitem = new string[]{
                item[1],str.GetDecode(item[2]),fid,item[5],item[6],adv.GetSpecialBase(Context,str.GetDecode(item[7]),_SiteID,true)
            };
                    if (int.Parse(item[4]) == 0) TongBuAl.Add(newitem);
                    else if (int.Parse(item[4]) == 1) YiBuAl.Add(newitem);
                }
                if (TongBuAl.Count > 0)
                {
                    DB db = new DB(SysConst.ConnString_ReadOnly);
                    db.Open();
                    try
                    {
                        output.Write("<script type=\"text/javascript\">$(function(){");
                        output.Write(adv.GetSpecialBase(Context, AppendJS0, SiteID) + ";");
                        if (string.IsNullOrWhiteSpace(FidCol)) FidCol = "fid";
                        output.Write(FTFrame.Server.Core.Fore.GetDyValue(db, TongBuAl, SiteID, true, FidCol, OpDefaultCol));
                        output.Write(adv.GetSpecialBase(Context, AppendJS1, SiteID) + ";");
                        output.Write("});</script>");
                    }
                    catch (Exception ex)
                    {
                        output.Write(ex.Message);
                        log.Error(ex);
                    }
                    finally
                    {
                        db.Close();
                    }
                }

                if (YiBuAl.Count > 0)
                {
                    output.Write("<script type=\"text/javascript\">");
                    output.WriteLine("function " + FuncName + "(p1,p2,p3,p4,p5,p6,p7,p8,p9,p10,p11,p12){");
                    output.Write(adv.GetSpecialBase(Context, YBAppendJS0, SiteID) + ";");
                    string definestr = "";
                    string defineeval = "";
                    string paravals = "(p1==null?'':p1)+\"##\"";
                    paravals += "+(p2==null?'':p2)+\"##\"";
                    paravals += "+(p3==null?'':p3)+\"##\"";
                    paravals += "+(p4==null?'':p4)+\"##\"";
                    paravals += "+(p5==null?'':p5)+\"##\"";
                    paravals += "+(p6==null?'':p6)+\"##\"";
                    paravals += "+(p7==null?'':p7)+\"##\"";
                    paravals += "+(p8==null?'':p8)+\"##\"";
                    paravals += "+(p9==null?'':p9)+\"##\"";
                    paravals += "+(p10==null?'':p10)+\"##\"";
                    paravals += "+(p11==null?'':p11)+\"##\"";
                    paravals += "+(p12==null?'':p12)";
                    int loopi = 0;
                    foreach (string[] item in YiBuAl)
                    {
                        loopi++;
                        string id = item[0].Trim();
                        string tabletag = item[1].Trim();
                        string fid = item[2].Trim();
                        int isdy = int.Parse(item[3]);
                        int isdim = int.Parse(item[4]);
                        string sql = item[5].Trim();
                        if (loopi > 1)
                        {
                            definestr += "|||";
                            defineeval += "\"|||\"+";
                        }
                        definestr += id + "##" + Str.Encode(tabletag) + "##";
                        if (fid.StartsWith("{"))
                        {
                            fid = fid.Replace("{", "").Replace("}", "");
                            definestr += "\"+" + fid + "+\"##";
                        }
                        else if (fid.StartsWith("@"))
                        {
                            fid = fid.Replace("@", "");
                            definestr += "\"+" + fid + "+\"##";
                        }
                        else
                        {
                            definestr += fid + "##";
                        }
                        definestr += isdy + "##" + isdim + "##" + Str.Encode(sql);
                        string evalvals = "";
                        Regex r = new Regex(@"{[^}]*}");
                        MatchCollection mc = r.Matches(sql);
                        foreach (Match m in mc)
                        {
                            evalvals += "" + m.Value.Replace("{", "").Replace("}", "") + "+\"##\"+";
                        }
                        defineeval += evalvals;
                    }
                    defineeval += "\"\"";
                    output.Write("$.post(\"/_ftpub/ftajax?rmd=\"+Math.random(), { \"ajaxtype\": \"Dyvalue\", \"definestr\":\"" + definestr + "\", \"defineeval\":" + defineeval + ", \"definepara\":" + paravals + ",\"siteid\":\"" + _SiteID + "\",\"fidcol\":\"" + Str.Encode(FidCol) + "\",\"opdefaultcol\":\"" + (OpDefaultCol) + "\"},");
                    output.WriteLine("function (data, textStatus) {eval(decodeURIComponent(data));");
                    output.Write(adv.GetSpecialBase(Context, YBAppendJS1, SiteID) + ";");
                    output.WriteLine("}, \"text\");");
                    output.WriteLine("}");
                    output.Write("</script>");
                }

                TongBuAl.Clear();
                TongBuAl = null;
                YiBuAl.Clear();
                YiBuAl = null;
            }
            catch (Exception ex)
            {
                output.Write(ex.Message);
                log.Error(ex);
            }
            finally
            {
            }

        }
    }
}
