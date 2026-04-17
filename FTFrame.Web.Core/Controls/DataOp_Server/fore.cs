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

namespace FT.DataOp.Fore
{
    public class client
    {
        public static string thisControlID = "";
        public static object[] thisStyleObject;
    }
    public class DataOp
    {
        public static void Output(HttpContext Context,TextWriter output,string SiteID, string ControlName, string ControlID, string PartID, string DataSource, string SetStyle, string ControlPara, int OPType, string Define, string NewSiteID, string JSBefore, string JSSuccess, string CodeBefore, string CodeAfter, string OPContidionSql, string OPContidionCode, string OPContidionJs, string BeforeSql, string AfterSql, string OPID, string CheckRule, string DefaultFID, int IsMultiMod, string MultiFidName, string MultiCondition, string CloseButton, string CloseButtonCap, string ButtonCap, string ButtonImg, int FlowType, string Tabletag, int FlowStat, string FlowDesign, string FlowDesignBaranch, string FlowDesignPos, string APISet, string FidCol, int OpDefaultCol, string CustomConnection = "")
        {
            ControlPara = ControlPara.Replace("{dsqt}", "\"");
            Define = Define.Replace("{dsqt}", "\"");
            NewSiteID = NewSiteID.Replace("{dsqt}", "\"");
            JSBefore = JSBefore.Replace("{dsqt}", "\"");
            JSSuccess = JSSuccess.Replace("{dsqt}", "\"");
            CodeBefore = CodeBefore.Replace("{dsqt}", "\"");
            CodeAfter = CodeAfter.Replace("{dsqt}", "\"");
            OPContidionSql = OPContidionSql.Replace("{dsqt}", "\"");
            OPContidionCode = OPContidionCode.Replace("{dsqt}", "\"");
            OPContidionJs = OPContidionJs.Replace("{dsqt}", "\"");
            BeforeSql = BeforeSql.Replace("{dsqt}", "\"");
            AfterSql = AfterSql.Replace("{dsqt}", "\"");
            OPID = OPID.Replace("{dsqt}", "\"");
            CheckRule = CheckRule.Replace("{dsqt}", "\"");
            DefaultFID = DefaultFID.Replace("{dsqt}", "\"");
            MultiFidName = MultiFidName.Replace("{dsqt}", "\"");
            MultiCondition = MultiCondition.Replace("{dsqt}", "\"");
            CloseButton = CloseButton.Replace("{dsqt}", "\"");
            CloseButtonCap = CloseButtonCap.Replace("{dsqt}", "\"");
            ButtonCap = ButtonCap.Replace("{dsqt}", "\"");
            ButtonImg = ButtonImg.Replace("{dsqt}", "\"");
            Tabletag = Tabletag.Replace("{dsqt}", "\"");
            FlowDesign = FlowDesign.Replace("{dsqt}", "\"");
            FlowDesignBaranch = FlowDesignBaranch.Replace("{dsqt}", "\"");
            FlowDesignPos = FlowDesignPos.Replace("{dsqt}", "\"");
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
                if (ForeFrame == ForeFrameType.JQueryUI)
                {
                    s += "<button class='_button' type='button' id='bs_" + PartID + "' onclick=\"ftdataset_" + PartID + "(this)\">" + ButtonCap + "</button>";
                    js += "$('#bs_" + PartID + "').button({icons: {primary: '" + ButtonImg + "'}});";
                }
                else if (ForeFrame == ForeFrameType.LayUI)
                {
                    s += "<button class='layui-btn' type='button' id='bs_" + PartID + "' onclick=\"ftdataset_" + PartID + "(this)\">" + ButtonCap + "</button>";
                }
                if (!CloseButtonCap.Equals(""))
                {
                    if (ForeFrame == ForeFrameType.JQueryUI)
                    {
                        s += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button class='_button' type='button' id='bc_" + PartID + "' onclick=\"" + CloseButton + "\">" + CloseButtonCap + "</button>";
                        js += "$('#bc_" + PartID + "').button({icons: {primary: 'ui-icon-close'}});";
                    }
                    else if (ForeFrame == ForeFrameType.LayUI)
                    {
                        s += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button class='layui-btn' type='button' id='bc_" + PartID + "' onclick=\"" + CloseButton + "\">" + CloseButtonCap + "</button>";
                    }
                }
                string _SiteID = (NewSiteID.Equals("") ? SiteID : NewSiteID);
                if (string.IsNullOrWhiteSpace(FidCol)) FidCol = "";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_datatype' value='" + Str.Encode(OPType) + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_siteid' value='" + _SiteID + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_fidcol' value='" + Str.Encode(FidCol) + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_defaultcols' value='" + OpDefaultCol + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_jssuc' value=\"" + adv.GetSpecialBase(Context, JSSuccess, _SiteID).Replace("\"", "\\&quot;") + "\"/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_opid' value='" + Str.Encode(OPID) + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_define' id='ftdataop_" + PartID + "_define' value=''/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_tabletag' value='" + Str.Encode(Tabletag) + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_defaultfid' id='ftdataop_" + PartID + "_defaultfid' value=''/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_flowtype' value='" + Str.Encode(FlowType) + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_flowstat' value='" + Str.Encode(FlowStat) + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_flowdesign' value='" + Str.Encode(FlowDesign) + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_flowdesignbaranch' value='" + Str.Encode(FlowDesignBaranch) + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_flowdesignpos' value='" + Str.Encode(FlowDesignPos) + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_codebefore' id='ftdataop_" + PartID + "_codebefore' value=''/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_codeafter' id='ftdataop_" + PartID + "_codeafter' value=''/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_cdnsql' value='" + Str.Encode(adv.GetSpecialBase(Context, OPContidionSql, _SiteID, true)) + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_cdnsqlevals' id='ftdataop_" + PartID + "_cdnsqlevals' value=''/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_cdncode' id='ftdataop_" + PartID + "_cdncode' value=''/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_cdnjs' value=\"" + adv.GetSpecialBase(Context, OPContidionJs, _SiteID).Replace("\"", "&quot;") + "\"/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_ismultimod' value=\"" + IsMultiMod + "\"/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_multifidname' value=\"" + MultiFidName + "\"/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_multicdn' value='" + Str.Encode(adv.GetSpecialBase(Context, MultiCondition, _SiteID, true)) + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_multicdnevals' id='ftdataop_" + PartID + "_multicdnevals' value=''/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_execsqlbefore' value='" + Str.Encode(adv.GetSpecialBase(Context, BeforeSql, _SiteID, true)) + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_execsqlbeforeevals' id='ftdataop_" + PartID + "_execsqlbeforeevals' value=''/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_execsqlafter' value='" + Str.Encode(adv.GetSpecialBase(Context, AfterSql, _SiteID, true)) + "'/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_execsqlafterevals' id='ftdataop_" + PartID + "_execsqlafterevals' value=''/>";
                s += "<input type='hidden' name='ftdataop_" + PartID + "_advsetevals' id='ftdataop_" + PartID + "_advsetevals' value=''/>";
                s += "<iframe width=0 height=0 scrolling=no frameborder=0 id='ftdataop_" + PartID + "_frame' name='ftdataop_" + PartID + "_frame'></iframe>";
                js = "$(function(){" + js + "});";
                js += "function ftdataset_" + PartID + "(obj){";
                js += adv.GetSpecialBase(Context, CheckRule, _SiteID) + ";";
                js += "var uploaddefine=\"\";";
                js += "eleValidateEmptyReverse='';";
                string advsetevals = "";
                string[] rows = Define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < rows.Length; i++)
                {
                    string[] item = rows[i].Split(new string[] { "##" }, StringSplitOptions.None);
                    if (i > 0)
                    {
                        js += "uploaddefine+=\"|\";";
                    }
                    if (!item[4].Equals(""))
                    {
                        string checkalertinfo = "";
                        bool checkisbase = false;
                        if (item[4].Equals("noempty")) { checkalertinfo = " 不能为空"; checkisbase = true; }
                        else if (item[4].Equals("int")) { checkalertinfo = " 必须为整数"; checkisbase = true; }
                        else if (item[4].Equals("decimal")) { checkalertinfo = " 必须为数字"; checkisbase = true; }
                        else if (item[4].Equals("date")) { checkalertinfo = " 必须为日期格式"; checkisbase = true; }
                        else if (item[4].Equals("int_empty") || item[4].Equals("int_0") || item[4].Equals("int_1")) { checkalertinfo = " 若不为空时，必须为整数"; checkisbase = false; }
                        else if (item[4].Equals("decimal_empty") || item[4].Equals("decimal_0") || item[4].Equals("decimal_1")) { checkalertinfo = " 若不为空时，必须为数字"; checkisbase = false; }
                        else if (item[4].Equals("null_nocheck")) { checkisbase = false; }
                        else if (item[4].Equals("null_int")) { checkalertinfo = " 若不为空时，必须为整数"; checkisbase = false; }
                        else if (item[4].Equals("null_decimal")) { checkalertinfo = " 若不为空时，必须为数字"; checkisbase = false; }
                        else if (item[4].Equals("null_date")) { checkalertinfo = " 若不为空时，必须为日期格式"; checkisbase = false; }
                        if (checkisbase) js += "if(!eleValidate(\"" + item[6] + "\",\"" + item[4] + "\",\"" + item[0] + checkalertinfo + "\")){eleValiEmpRev();return false;}";
                        else
                        {
                            if (item[4].Equals("int_empty"))
                            {
                                js += "if(!eleValidateEmpty(\"" + item[6] + "\",\"int\",\"" + item[0] + checkalertinfo + "\",null)){eleValiEmpRev();return false;}";
                            }
                            else if (item[4].Equals("int_0"))
                            {
                                js += "if(!eleValidateEmpty(\"" + item[6] + "\",\"int\",\"" + item[0] + checkalertinfo + "\",0)){eleValiEmpRev();return false;}";
                            }
                            else if (item[4].Equals("int_1"))
                            {
                                js += "if(!eleValidateEmpty(\"" + item[6] + "\",\"int\",\"" + item[0] + checkalertinfo + "\",-1)){eleValiEmpRev();return false;}";
                            }
                            else if (item[4].Equals("decimal_empty"))
                            {
                                js += "if(!eleValidateEmpty(\"" + item[6] + "\",\"decimal\",\"" + item[0] + checkalertinfo + "\",null)){eleValiEmpRev();return false;}";
                            }
                            else if (item[4].Equals("decimal_0"))
                            {
                                js += "if(!eleValidateEmpty(\"" + item[6] + "\",\"decimal\",\"" + item[0] + checkalertinfo + "\",0)){eleValiEmpRev();return false;}";
                            }
                            else if (item[4].Equals("decimal_1"))
                            {
                                js += "if(!eleValidateEmpty(\"" + item[6] + "\",\"decimal\",\"" + item[0] + checkalertinfo + "\",-1)){eleValiEmpRev();return false;}";
                            }
                            else if (item[4].Equals("null_nocheck"))
                            {
                                js += "eleValidateEmpty(\"" + item[6] + "\",\"null_nocheck\",\"\",\"[FTNULL]\");";
                            }
                            else if (item[4].Equals("null_int"))
                            {
                                js += "if(!eleValidateEmpty(\"" + item[6] + "\",\"int\",\"" + item[0] + checkalertinfo + "\",\"[FTNULL]\")){eleValiEmpRev();return false;}";
                            }
                            else if (item[4].Equals("null_decimal"))
                            {
                                js += "if(!eleValidateEmpty(\"" + item[6] + "\",\"decimal\",\"" + item[0] + checkalertinfo + "\",\"[FTNULL]\")){eleValiEmpRev();return false;}";
                            }
                            else if (item[4].Equals("null_date"))
                            {
                                js += "if(!eleValidateEmpty(\"" + item[6] + "\",\"date\",\"" + item[0] + checkalertinfo + "\",\"[FTNULL]\")){eleValiEmpRev();return false;}";
                            }
                        }
                        /*
                         if(item[4].Equals("noempty"))
                         {
                           js += "if(!eleCheck(\"" + item[6] + "\",\"noempty\")){info(\"" + item[0] + " 不能为空\",2,null,\"document.getElementById('" + item[6] + "').focus();\");return false;};";
                           js+="var rateIndex=1;var rateObj=document.getElementById('" + item[6] + "_rowrate'+rateIndex);while(rateObj!=null){if(!eleCheck(rateObj.id,\"noempty\")){info(\"" + item[0] + " 不能为空\",2,null,\"document.getElementById('\"+rateObj.id+\"').focus();\");return false;};rateIndex++;rateObj=document.getElementById('" + item[6] + "_rowrate'+rateIndex);}";
                         }
                         else if(item[4].Equals("int"))
                         {
                           js += "if(!eleCheck(\"" + item[6] + "\",\"int\")){info(\"" + item[0] + " 必须为整数\",2,null,\"document.getElementById('" + item[6] + "').focus();\");return false;};";
                           js+="var rateIndex=1;var rateObj=document.getElementById('" + item[6] + "_rowrate'+rateIndex);while(rateObj!=null){if(!eleCheck(rateObj.id,\"int\")){info(\"" + item[0] + " 必须为整数\",2,null,\"document.getElementById('\"+rateObj.id+\"').focus();\");return false;};rateIndex++;rateObj=document.getElementById('" + item[6] + "_rowrate'+rateIndex);}";
                         }
                         else if(item[4].Equals("decimal"))
                         {
                           js += "if(!eleCheck(\"" + item[6] + "\",\"decimal\")){info(\"" + item[0] + " 必须为数字\",2,null,\"document.getElementById('" + item[6] + "').focus();\");return false;};";
                           js+="var rateIndex=1;var rateObj=document.getElementById('" + item[6] + "_rowrate'+rateIndex);while(rateObj!=null){if(!eleCheck(rateObj.id,\"decimal\")){info(\"" + item[0] + " 必须为数字\",2,null,\"document.getElementById('\"+rateObj.id+\"').focus();\");return false;};rateIndex++;rateObj=document.getElementById('" + item[6] + "_rowrate'+rateIndex);}";
                         }
                         else if(item[4].Equals("date"))
                         {
                           js += "if(!eleCheck(\"" + item[6] + "\",\"date\")){info(\"" + item[0] + " 必须为日期格式\",2,null,\"document.getElementById('" + item[6] + "').focus();\");return false;};";
                           js+="var rateIndex=1;var rateObj=document.getElementById('" + item[6] + "_rowrate'+rateIndex);while(rateObj!=null){if(!eleCheck(rateObj.id,\"date\")){info(\"" + item[0] + " 必须为日期格式\",2,null,\"document.getElementById('\"+rateObj.id+\"').focus();\");return false;};rateIndex++;rateObj=document.getElementById('" + item[6] + "_rowrate'+rateIndex);}";
                         }
                         */
                    }
                    if (i > 0) advsetevals += "\"|||\"+";
                    if (item.Length > 7)
                    {
                        Regex r2 = new Regex(@"{[^}]*}");
                        MatchCollection mc2 = r2.Matches(str.GetDecode(item[7]));
                        foreach (Match m2 in mc2)
                        {
                            advsetevals += m2.Value.Replace("{", "").Replace("}", "") + "+\"##\"+";
                        }
                    }
                    string _fid = adv.GetSpecialBase(Context, item[5], _SiteID);
                    if (_fid.StartsWith("{"))
                    {
                        _fid = _fid.Replace("{", "").Replace("}", "");
                        js += "uploaddefine+=\"" + item[1] + "#" + item[2] + "#" + item[3] + "#" + "\"+" + _fid + "+\"" + (item.Length > 7 ? item[7] : "") + "\"";
                    }
                    else
                    {
                        js += "uploaddefine+=\"" + item[1] + "#" + item[2] + "#" + item[3] + "#" + _fid + "#" + (item.Length > 7 ? item[7] : "") + "\";";
                    }
                }
                js += "$('#ftdataop_" + PartID + "_define').val(uploaddefine);";
                string _DefaultFID = adv.GetSpecialBase(Context, DefaultFID, _SiteID);
                if (_DefaultFID.StartsWith("{"))
                {
                    _DefaultFID = _DefaultFID.Replace("{", "").Replace("}", "");
                    js += "$('#ftdataop_" + PartID + "_defaultfid').val(" + _DefaultFID + ");";
                }
                else
                {
                    js += "$('#ftdataop_" + PartID + "_defaultfid').val(\"" + _DefaultFID + "\");";
                }
                string codebefore = adv.GetSpecialBase(Context, CodeBefore, _SiteID);
                string codeafter = adv.GetSpecialBase(Context, CodeAfter, _SiteID);
                Regex r = new Regex(@"{[^}]*}");
                MatchCollection mc = r.Matches(codebefore);
                foreach (Match m in mc)
                {
                    codebefore = codebefore.Replace(m.Value, "\"+" + m.Value.Replace("{", "").Replace("}", "") + "+\"");
                }
                r = new Regex(@"{[^}]*}");
                mc = r.Matches(codeafter);
                foreach (Match m in mc)
                {
                    codeafter = codeafter.Replace(m.Value, "\"+" + m.Value.Replace("{", "").Replace("}", "") + "+\"");
                }
                js += "$('#ftdataop_" + PartID + "_codebefore').val(\"" + codebefore + "\");";
                js += "$('#ftdataop_" + PartID + "_codeafter').val(\"" + codeafter + "\");";
                string cdnsqlevalvals = "";
                r = new Regex(@"{[^}]*}");
                mc = r.Matches(OPContidionSql);
                foreach (Match m in mc)
                {
                    cdnsqlevalvals += m.Value.Replace("{", "").Replace("}", "") + "+\"##\"+";
                }
                js += "$('#ftdataop_" + PartID + "_cdnsqlevals').val(" + cdnsqlevalvals + "\"\");";
                string opcdncode = adv.GetSpecialBase(Context, OPContidionCode, _SiteID);
                r = new Regex(@"{[^}]*}");
                mc = r.Matches(opcdncode);
                foreach (Match m in mc)
                {
                    opcdncode = opcdncode.Replace(m.Value, "\"+" + m.Value.Replace("{", "").Replace("}", "") + "+\"");
                }
                js += "$('#ftdataop_" + PartID + "_cdncode').val(\"" + opcdncode + "\");";
                string multicdnevalvals = "";
                r = new Regex(@"{[^}]*}");
                mc = r.Matches(MultiCondition);
                foreach (Match m in mc)
                {
                    multicdnevalvals += m.Value.Replace("{", "").Replace("}", "") + "+\"##\"+";
                }
                js += "$('#ftdataop_" + PartID + "_multicdnevals').val(" + multicdnevalvals + "\"\");";

                string execsqlbeforeevals = "";
                r = new Regex(@"{[^}]*}");
                mc = r.Matches(BeforeSql);
                foreach (Match m in mc)
                {
                    execsqlbeforeevals += m.Value.Replace("{", "").Replace("}", "") + "+\"##\"+";
                }
                js += "$('#ftdataop_" + PartID + "_execsqlbeforeevals').val(" + execsqlbeforeevals + "\"\");";

                string execsqlafterevals = "";
                r = new Regex(@"{[^}]*}");
                mc = r.Matches(AfterSql);
                foreach (Match m in mc)
                {
                    execsqlafterevals += m.Value.Replace("{", "").Replace("}", "") + "+\"##\"+";
                }
                js += "$('#ftdataop_" + PartID + "_execsqlafterevals').val(" + execsqlafterevals + "\"\");";
                js += "$('#ftdataop_" + PartID + "_advsetevals').val(" + advsetevals + "\"\");";
                js += "var _form=$(obj).closest('form')[0];";
                js += "if(_form==null){alert('This button must be put in a form.');return false;}";
                js += "_form.encding='multipart/form-data';";
                js += "$(_form).attr('enctype','multipart/form-data');";
                js += "_form.method='post';";
                js += "_form.target='ftdataop_" + PartID + "_frame';";
                js += "_form.action='/_ftpub/ftform?ftformtype=dataop&partid=" + PartID + "&rmd='+Math.random();";
                js += adv.GetSpecialBase(Context, JSBefore, _SiteID) + ";";
                js += "newDialog('loading','loading');";
                js += "_form.submit();";
                js += "eleValiEmpRev();";
                js += "}";
                s += "<script type=\"text/javascript\">" + js + "</script>";
                output.Write(s);
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
