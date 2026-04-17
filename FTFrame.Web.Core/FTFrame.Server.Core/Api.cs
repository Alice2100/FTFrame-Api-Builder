using Aliyun.OSS;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using FTFrame.DBClient;
using FTFrame.DBClient.DataBaseType;
using FTFrame.Project.Core;
using FTFrame.Project.Core.Utils;
using FTFrame.Project.Core.WorkFlow;
using FTFrame.Server.Core.Model;
using FTFrame.Server.Core.Tool;
using FTFrame.Tool;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using CoreHttp = Microsoft.AspNetCore.Http;

namespace FTFrame.Server.Core
{
    public class Api
    {
        public static string Json(string businessType, HttpContext context, Dictionary<string, string> setDic, string[] paras, bool IsSon, Dictionary<string, int> OpedApiPathList = null, int level = 0, string CurApiPath = "", Dictionary<string, object> reqDic = null)
        {
            try
            {
                //log.DebugMonth(str.GetIP()+","+businessType+","+context.Request.GetDisplayUrl());
                if (OpedApiPathList == null) OpedApiPathList = new Dictionary<string, int>();
                if (OpedApiPathList.ContainsKey(CurApiPath) && OpedApiPathList[CurApiPath] < level) return ("\"Endless Loop\"");
                if (!OpedApiPathList.ContainsKey(CurApiPath)) OpedApiPathList.Add(CurApiPath, level);
                if (reqDic == null) reqDic = new Dictionary<string, object>();
                var isOutputLog = FTFrame.Project.Core.Api.IsOutputLog(businessType, paras, context, reqDic);
                switch (businessType)
                {
                    case "List":
                        return List(context, setDic, paras, IsSon, OpedApiPathList, level, reqDic, isOutputLog);
                    case "DyValue":
                        return DyValue(context, setDic, paras, IsSon, OpedApiPathList, level, reqDic, isOutputLog);
                    case "DataOP":
                        return DataOP(context, setDic, paras, reqDic, isOutputLog);
                    default:
                        return FTFrame.Project.Core.Api.ErrorJson("No Type");
                }
                //if (string.CompareOrdinal(businessType, "List") == 0) return List(context, setDic, paras, IsSon, OpedApiPathList, level);
                //else if (string.CompareOrdinal(businessType, "DyValue") == 0) return DyValue(context, setDic, paras, IsSon, OpedApiPathList, level);
                //else if (string.CompareOrdinal(businessType, "DataOP") == 0) return DataOP(context, setDic, paras);
                //ApiBusinessType BusinessType= (ApiBusinessType)Enum.Parse(typeof(ApiBusinessType), businessType);
                //switch (BusinessType)
                //{
                //    case ApiBusinessType.List:return List(context, setDic, paras,IsSon, OpedApiPathList,level);
                //    case ApiBusinessType.DyValue: return DyValue(context, setDic, paras, IsSon, OpedApiPathList, level);
                //}
                //return FTFrame.Project.Core.Api.ErrorJson("No Type");
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError(ex, context,reqDic);
                return FTFrame.Project.Core.Api.ErrorJson(ex.Message);
            }
        }
        private static string ReqFormOrJsonVal(bool isJson, HttpRequest req, JObject jObject, string key, string defaultValue = "",string nullvalue = null)
        {
            if (!isJson)
            {
                if (!req.HasFormContentType) return defaultValue;
                if (key == null) return defaultValue;
                else return req.Form[key].FirstOrDefault<string>()?.Trim() ?? defaultValue;
            }
            else
            {
                if (jObject.ContainsKey(key))
                {
                    return (jObject[key].Type == JTokenType.Null ? nullvalue : jObject[key].ToString());
                }
                else return defaultValue;
                    //return jObject?[key] == null ? defaultValue : (jObject[key].Type == JTokenType.Null ? null : jObject[key].ToString());
            }
        }
        private static string DataOP(CoreHttp.HttpContext context, Dictionary<string, string> setDic, string[] paras, Dictionary<string, object> reqDic, (bool ShowBaseLog, bool ShowInputLog, bool ShowOutputLog) IsOutputLog)
        {
            if (!Project.Core.User.IsLogin(reqDic))
            {
                return Project.Core.Api.ErrorJson("No Right");
            }
            (string url, string userid, string[] paras, DateTime dttime, string jobjstr, string fid, string fidformod) OPDuplicateObj = ("", "", [],DateTime.Now,"","","");
            try
            {
                bool isJson = setDic["InputType"] == "json";
                if (!isJson && !context.Request.HasFormContentType)
                {
                    return FTFrame.Project.Core.Api.ErrorJson("Need Form Post");
                }

                HttpRequest req = context.Request;
                JObject jObject = null;
                if (isJson)
                {
                    try
                    {
                        req.EnableBuffering();
                        using (var ms = new MemoryStream())
                        {
                            req.Body.Position = 0;
                            req.Body.CopyTo(ms);
                            var buffer = ms.ToArray();
                            string content = Encoding.UTF8.GetString(buffer);
                            if (string.IsNullOrWhiteSpace(content)) content = "{}";
                            if (content.TrimStart().StartsWith("{"))
                            {
                                jObject = JObject.Parse(content);
                                //输出json格式化日志
                                if (IsOutputLog.ShowInputLog)
                                {
                                    using StringWriter textWriter = new StringWriter();
                                    JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                                    {
                                        Formatting = Newtonsoft.Json.Formatting.Indented,
                                        Indentation = 4,//缩进字符数
                                        IndentChar = ' '//缩进字符
                                    };
                                    new JsonSerializer().Serialize(jsonWriter, jObject);
                                    FTFrame.Project.Core.Api.LogDebug(textWriter.ToString(), context, reqDic);
                                }
                            }
                            else
                            {
                                jObject = JObject.Parse("{}");
                                FTFrame.Project.Core.Api.LogDebug(content, context, reqDic);
                            }
                        }
                        //using (Stream stream = req.Body)
                        //{
                        //    byte[] buffer = new byte[req.ContentLength == null ? 0 : req.ContentLength.Value];
                        //    stream.Read(buffer, 0, buffer.Length);
                        //    string content = Encoding.UTF8.GetString(buffer);
                        //    req.Body.Position = 0;
                        //    if (string.IsNullOrWhiteSpace(content)) content = "{}";
                        //    jObject = JObject.Parse(content);
                        //}
                    }
                    catch (Exception ex)
                    {
                        FTFrame.Project.Core.Api.LogError(ex, context, reqDic);
                        return FTFrame.Project.Core.Api.ExceptionJson(ex);
                    }
                }
                string RightCheck = Interface.Right.DataOP(context);
                if (RightCheck != null)
                {
                    return FTFrame.Project.Core.Api.ErrorJson(RightCheck);
                }
                string memid = FTFrame.Project.Core.User.UserID();
                string opid = setDic["opid"];
                var opRight = Interface.Right.HaveOPRight(opid, paras, reqDic, context);
                if (!opRight.result)
                {
                    return FTFrame.Project.Core.Api.ErrorJson(opRight.noRightTip ?? "You do not have permission to operate this item");
                }
                string PartID = setDic["partid"];
                if (PartID.Length > 36) PartID = PartID.Substring(0, 36);
                string siteid = setDic["SiteID"];
                string jssuc = setDic["jssuc"];//操作成功后返回的消息
                string define = setDic["DefineStr"];
                string ApiDefine = setDic["ApiDefine"];
                string defaultfid = setDic["defaultfid"];

                //log.DebugDirect("aaa " + string.Join(',', paras));

                paras = ParasRewrite(context, reqDic, defaultfid, paras);

                //log.DebugDirect("bbb " + string.Join(',', paras));

                if (!string.IsNullOrEmpty(ReqFormOrJsonVal(isJson, req, jObject, "fid", null)))
                {
                    if (paras.Length < 2)//p1参数通过fid post过来 而不是 get
                    {
                        paras = new string[] { paras[0], ReqFormOrJsonVal(isJson, req, jObject, "fid", null) };
                    }
                    else if (paras.Length == 2 && paras.Last()== "noDefaultCols")
                    {
                        paras = new string[] { paras[0], ReqFormOrJsonVal(isJson, req, jObject, "fid", null), "noDefaultCols" };
                    }
                }

                //log.DebugDirect("ccc " + string.Join(',', paras));

                if (defaultfid.StartsWith("@ForcePara#")) defaultfid = "";//"@p1@";
                string defaultfidForMod = defaultfid;
                if (string.IsNullOrWhiteSpace(defaultfidForMod)) defaultfidForMod = "@p1@";
                string codebefore = setDic["codebefore"];
                string codeafter = setDic["codeafter"];
                string cdnsql = setDic["cdnsql"];
                string cdncode = setDic["cdncode"];
                string cdnjs = setDic["cdnjs"];//前提条件验证不通过时的消息
                string execsqlbefore = setDic["execsqlbefore"];
                string execsqlafter = setDic["execsqlafter"];
                string FidCol = setDic["FidCol"];
                //if (string.IsNullOrWhiteSpace(FidCol)) FidCol = "";
                int OpDefaultCol = int.Parse(setDic["OpDefaultCol"]);
                var DBSet = adv.CustomDBSet(setDic.GetValueOrDefault("CustomConnection"), paras, reqDic, context);
                string tableflog = "ft_ftdp_formflog";// "ft_" + siteid + "_formflog";
                string tableolog = "ft_ftdp_formolog";// "ft_" + siteid + "_formolog";
                string FidCol2 = "";
                if (FidCol.IndexOf(',') > 0)
                {
                    FidCol2 = FidCol.Split(',')[1];
                    FidCol = FidCol.Split(',')[0];
                }

                OPDuplicateObj = FTFrame.Project.Core.Api.OPDuplicateObj(context, memid, paras, reqDic, jObject, defaultfid, defaultfidForMod);
                var OPDuplicate = FTFrame.Project.Core.Api.OPDuplicateCheck(OPDuplicateObj);
                if (OPDuplicate != null)
                {
                    return FTFrame.Project.Core.Api.ErrorJson(OPDuplicate);
                }
                FTFrame.Project.Core.Api.OPDuplicateEnter(OPDuplicateObj);

                //if (!string.IsNullOrEmpty(ReqFormOrJsonVal(isJson, req, jObject, "fid", null)))
                //{
                //    if (paras.Length < 2)//p1参数通过fid post过来 而不是 get
                //    {
                //        paras = new string[] { paras[0], ReqFormOrJsonVal(isJson, req, jObject, "fid", null) };
                //    }
                //}
                var noDefaultCols = paras.Length>0 && paras.Last()== "noDefaultCols";
                if (noDefaultCols) OpDefaultCol = 0;
                for (int k = 1; k < paras.Length; k++)
                {
                    codebefore = codebefore.Replace("@p" + k + "@", paras[k]).Replace("{p" + k + "}", paras[k]);
                    codeafter = codeafter.Replace("@p" + k + "@", paras[k]).Replace("{p" + k + "}", paras[k]);
                    cdnsql = cdnsql.Replace("@p" + k + "@", paras[k].D0()).Replace("{p" + k + "}", paras[k]);
                    cdncode = cdncode.Replace("@p" + k + "@", paras[k]).Replace("{p" + k + "}", paras[k]);
                    execsqlbefore = execsqlbefore.Replace("@p" + k + "@", paras[k].D0()).Replace("{p" + k + "}", paras[k]);
                    execsqlafter = execsqlafter.Replace("@p" + k + "@", paras[k].D0()).Replace("{p" + k + "}", paras[k]);
                }
                codebefore = DataOPRootValue(codebefore, isJson, req, jObject);
                codeafter = DataOPRootValue(codeafter, isJson, req, jObject);
                cdnsql = DataOPRootValue(cdnsql, isJson, req, jObject, true);
                cdncode = DataOPRootValue(cdncode, isJson, req, jObject);
                execsqlbefore = DataOPRootValue(execsqlbefore, isJson, req, jObject, true);
                execsqlafter = DataOPRootValue(execsqlafter, isJson, req, jObject, true);

                cdnsql = adv.CodePattern(context, cdnsql, reqDic);
                execsqlbefore = adv.CodePattern(context, execsqlbefore, reqDic);
                execsqlafter = adv.CodePattern(context, execsqlafter, reqDic);
                cdnsql = adv.ParaPattern(context, cdnsql, reqDic);
                execsqlbefore = adv.ParaPattern(context, execsqlbefore, reqDic);
                execsqlafter = adv.ParaPattern(context, execsqlafter, reqDic);
                if (!string.IsNullOrWhiteSpace(execsqlbefore)) { if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(execsqlbefore, "[Form SqlBefore]", context, reqDic); }
                if (!string.IsNullOrWhiteSpace(execsqlafter)) { if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(execsqlafter, "[Form SqlAfter]", context, reqDic); }

                string[] apiItem = ApiDefine.Split(new string[] { "[##]" }, StringSplitOptions.None);
                string[] apiKeys = apiItem[2].Split(new string[] { "[#]" }, StringSplitOptions.None);
                string[] colKeys = apiItem[3].Split(new string[] { "[#]" }, StringSplitOptions.None);
                bool ValidateOK = true;
                string ErrorStr = null;
                Dictionary<string, string> keyValue = new Dictionary<string, string>();
                Dictionary<string, string[]> keyItem = new Dictionary<string, string[]>();
                Dictionary<string, string> JsonkeyValue = new Dictionary<string, string>();
                List<DPData> DataList = new List<DPData>();
                string[] rows = define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < rows.Length; i++)
                {
                    string[] item = rows[i].Split(new string[] { "##" }, StringSplitOptions.None);
                    string caption = item[0].Trim();
                    string name = item[1].Trim();
                    string jiaoyan = item[4].Trim();
                    string idValue = item[6].Trim();
                    if (idValue.StartsWith("@cdn:"))
                    {
                        string cdnSql = idValue.Substring(5);
                        cdnSql = DataOPRootValue(cdnSql, isJson, req, jObject, true);
                        cdnSql = adv.CodePattern(context, cdnSql, reqDic);
                        cdnSql = adv.ParaPattern(context, cdnSql, reqDic);
                        using (DB dbCdn = new DB(DBSet.DataBaseType, DBSet.ConnString))
                        {
                            dbCdn.Open();
                            if (dbCdn.GetInt(cdnSql) <= 0)
                            {
                                continue;
                            }
                        }
                    }
                    int matchIndex = -1;
                    for (int k = 0; k < colKeys.Length; k++)
                    {
                        if (colKeys[k].Equals(name))
                        {
                            matchIndex = k;
                            break;
                        }
                    }
                    if (matchIndex < 0) continue;



                    string apikey = apiKeys[matchIndex];
                    //string val = ReqFormOrJsonVal(isJson, req, jObject, apikey, null,"[FTNULL]");
                    string val = ReqFormOrJsonVal(isJson, req, jObject, apikey, null,"");
                    if (!apikey.StartsWith("_") && val == null) continue;//未入参的忽略，这样可以配置冗余字段而不影响
                    if (!keyValue.ContainsKey(apikey)) keyValue.Add(apikey, val);
                    bool valueChanged = false;
                    #region 校验
                    if (!string.IsNullOrWhiteSpace(jiaoyan))
                    {
                        if (jiaoyan.Equals("noempty"))
                        {
                            if (string.IsNullOrEmpty(val))
                            {
                                ValidateOK = false;
                                ErrorStr = caption + " 不能为空";
                                break;
                            }
                        }
                        else if (jiaoyan.Equals("int"))
                        {
                            if (string.IsNullOrWhiteSpace(val) || !int.TryParse(val, out int _i))
                            {
                                ValidateOK = false;
                                ErrorStr = caption + " 必须为整数";
                                break;
                            }
                        }
                        else if (jiaoyan.Equals("decimal"))
                        {
                            if (string.IsNullOrWhiteSpace(val) || !decimal.TryParse(val, out decimal _i))
                            {
                                ValidateOK = false;
                                ErrorStr = caption + " 必须为数字";
                                break;
                            }
                        }
                        else if (jiaoyan.Equals("date"))
                        {
                            if (string.IsNullOrWhiteSpace(val) || !DateTime.TryParse(val, out DateTime _i))
                            {
                                ValidateOK = false;
                                ErrorStr = caption + " 必须为日期格式";
                                break;
                            }
                        }
                        else if (jiaoyan.Equals("int_empty"))
                        {
                            if (string.IsNullOrWhiteSpace(val))
                            {
                                keyValue[apikey] = "";
                                valueChanged = true;
                            }
                            else if (!int.TryParse(val, out int _i))
                            {
                                ValidateOK = false;
                                ErrorStr = caption + " 若不为空时，必须为整数";
                                break;
                            }
                        }
                        else if (jiaoyan.Equals("int_0"))
                        {
                            if (string.IsNullOrWhiteSpace(val))
                            {
                                keyValue[apikey] = "0";
                                valueChanged = true;
                            }
                            else if (!int.TryParse(val, out int _i))
                            {
                                ValidateOK = false;
                                ErrorStr = caption + " 若不为空时，必须为整数";
                                break;
                            }
                        }
                        else if (jiaoyan.Equals("int_1"))
                        {
                            if (string.IsNullOrWhiteSpace(val))
                            {
                                keyValue[apikey] = "1";
                                valueChanged = true;
                            }
                            else if (!int.TryParse(val, out int _i))
                            {
                                ValidateOK = false;
                                ErrorStr = caption + " 若不为空时，必须为整数";
                                break;
                            }
                        }
                        else if (jiaoyan.Equals("decimal_empty"))
                        {
                            if (string.IsNullOrWhiteSpace(val))
                            {
                                keyValue[apikey] = "";
                                valueChanged = true;
                            }
                            if (!decimal.TryParse(val, out decimal _i))
                            {
                                ValidateOK = false;
                                ErrorStr = caption + " 若不为空时，必须为数字";
                                break;
                            }
                        }
                        else if (jiaoyan.Equals("decimal_0"))
                        {
                            if (string.IsNullOrWhiteSpace(val))
                            {
                                keyValue[apikey] = "0";
                                valueChanged = true;
                            }
                            else if (!decimal.TryParse(val, out decimal _i))
                            {
                                ValidateOK = false;
                                ErrorStr = caption + " 若不为空时，必须为数字";
                                break;
                            }
                        }
                        else if (jiaoyan.Equals("decimal_1"))
                        {
                            if (string.IsNullOrWhiteSpace(val))
                            {
                                keyValue[apikey] = "1";
                                valueChanged = true;
                            }
                            else if (!decimal.TryParse(val, out decimal _i))
                            {
                                ValidateOK = false;
                                ErrorStr = caption + " 若不为空时，必须为数字";
                                break;
                            }
                        }
                        else if (jiaoyan.Equals("null_nocheck"))
                        {
                            if (string.IsNullOrWhiteSpace(val))
                            {
                                keyValue[apikey] = "[FTNULL]";// null;
                                valueChanged = true;
                            }
                        }
                        else if (jiaoyan.Equals("null_int"))
                        {
                            if (string.IsNullOrWhiteSpace(val))
                            {
                                keyValue[apikey] = "[FTNULL]";// null;
                                valueChanged = true;
                            }
                            else if (!int.TryParse(val, out int _i))
                            {
                                ValidateOK = false;
                                ErrorStr = caption + " 若不为空时，必须为整数";
                                break;
                            }
                        }
                        else if (jiaoyan.Equals("null_decimal"))
                        {
                            if (string.IsNullOrWhiteSpace(val))
                            {
                                keyValue[apikey] = "[FTNULL]";// null;
                                valueChanged = true;
                            }
                            else if (!decimal.TryParse(val, out decimal _i))
                            {
                                ValidateOK = false;
                                ErrorStr = caption + " 若不为空时，必须为数字";
                                break;
                            }
                        }
                        else if (jiaoyan.Equals("null_date"))
                        {
                            if (string.IsNullOrWhiteSpace(val))
                            {
                                keyValue[apikey] = "[FTNULL]";// null;
                                valueChanged = true;
                            }
                            else if (!DateTime.TryParse(val, out DateTime _i))
                            {
                                ValidateOK = false;
                                ErrorStr = caption + " 若不为空时，必须为日期格式";
                                break;
                            }
                        }
                    }
                    #endregion

                    string tabletag = str.GetDecode(item[2].Trim());
                    string optype = str.GetDecode(item[3].Trim());
                    string advset = str.GetDecode(item[7].Trim());
                    string fid = item[5].Trim();
                    if (fid == "") fid = (optype == "1" ? defaultfidForMod : defaultfid);
                    for (int k = 1; k < paras.Length; k++)
                    {
                        fid = fid.Replace("@p" + k + "@", paras[k].Replace("'", "")).Replace("{p" + k + "}", paras[k].Replace("'", ""));
                        tabletag = tabletag.Replace("@p" + k + "@", paras[k]).Replace("{p" + k + "}", paras[k]);
                        advset = advset.Replace("@p" + k + "@", paras[k].D0()).Replace("{p" + k + "}", paras[k]);
                    }
                    if (fid == "@p1@") fid = "";//默认值@p1@并没有传值，则重新置为默认空
                    fid = DataOPRootValue(fid, isJson, req, jObject, true);
                    tabletag = DataOPRootValue(tabletag, isJson, req, jObject);
                    advset = DataOPRootValue(advset, isJson, req, jObject, true);

                    fid = adv.CodePattern(context, fid, reqDic);
                    advset = adv.CodePattern(context, advset, reqDic);
                    fid = adv.ParaPattern(context, fid, reqDic);
                    advset = adv.ParaPattern(context, advset, reqDic);

                    #region 处理Json
                    string json = null;
                    if (item.Length >= 9) json = item[8].Trim();
                    int leixing = int.Parse(optype);
                    List<string> FileKeys = Page.IFormFileKeys(context.Request);
                    string jsonOpResult = DataOPKeyValueJson(isJson, context, jObject, json, DataList, name, leixing, tabletag, advset, FileKeys, paras);

                    if (jsonOpResult != null)
                    {
                        keyValue.Clear();
                        keyValue = null;
                        keyItem.Clear();
                        keyItem = null;
                        return FTFrame.Project.Core.Api.ErrorJson(jsonOpResult);
                    }
                    #endregion

                    if (int.Parse(optype) >= 7 && int.Parse(optype) <= 12) continue;//通过Json处理了
                    keyItem.Add(apikey, new string[] { fid, tabletag, optype, advset, valueChanged ? "1" : "0", keyValue[apikey] });
                }

                if (!ValidateOK)
                {
                    keyValue.Clear();
                    keyValue = null;
                    keyItem.Clear();
                    keyItem = null;
                    return FTFrame.Project.Core.Api.ErrorJson(ErrorStr);
                }

                #region 操作条件
                //支持##多条件和多条件返回 Sql多余一列则第二列为返回的cdnjs
                string cdnreturn = Page.ConditionReturn(cdnsql, cdncode, null, context, cdnjs, out string rtnCdnjs);
                if (cdnreturn != null && !cdnreturn.Equals("0"))
                {
                    //return FTFrame.Project.Core.Api.ErrorJson(cdnjs.Replace("{msg}", cdnreturn));
                    if(rtnCdnjs.StartsWith("[STR]")) 
                        return FTFrame.Project.Core.Api.ErrorJson(rtnCdnjs.Substring("[STR]".Length));
                    else
                        return FTFrame.Project.Core.Api.ErrorJson(rtnCdnjs);
                        //return FTFrame.Project.Core.Api.ErrorJson(cdnjs.Replace("{msg}", rtnCdnjs));
                    //if (cdnreturn.Equals("1"))
                    //{
                    //    return FTFrame.Project.Core.Api.ErrorJson(cdnjs);
                    //}
                    //else
                    //{
                    //    return FTFrame.Project.Core.Api.ErrorJson(cdnreturn);
                    //}
                }
                #endregion
                #region 流水号初始化
                ArrayList LiquidAL = new ArrayList();
                foreach (string liqkey in (context.Request.HasFormContentType ? context.Request.Form.Keys : new string[0]))
                {
                    if (liqkey.StartsWith("ftform_liquid"))
                    {
                        string liquid_all = context.Request.Form[liqkey];
                        if (liquid_all != null && !liquid_all.Equals(""))
                        {
                            string[] liquid_alls = liquid_all.Split(new string[] { "&&" }, StringSplitOptions.None);
                            string liquid_patten = liquid_alls[0];
                            string liquid_table = liquid_alls[1];
                            string liquid_col = liquid_alls[2];
                            string liquid_locklike = liquid_alls[3];
                            if (liquid_table.StartsWith("@")) liquid_table = liquid_table.Substring(1);
                            else
                                liquid_table = "ft_" + siteid + "_f_" + liquid_table;
                            LiquidAL.Add(new string[] {
                                liquid_table,liquid_col,liquid_patten,liquid_locklike
                            });
                        }
                    }
                }
                #endregion
                string _newfid_ = "";
                DB db = new DB(DBSet.DataBaseType, DBSet.ConnString);
                db.Open();
                ST st = db.GetTransaction();
                try
                {
                    ///执行操作前SQL
                    Page.FormSqlExec(db, st, execsqlbefore, "", "", context);

                    Hashtable TableAdd = new Hashtable();//Add的信息存储
                    Hashtable TableMod = new Hashtable();//Mod的信息存储
                    Dictionary<string, Dictionary<string, string>> TableModDic = new();//Mod的信息存储，不区分fid
                    Hashtable TableAddDy = new Hashtable();//Add的动态新增信息存储
                    Hashtable TableModDy = new Hashtable();//Mod的动态新增信息存储
                    ArrayList StayedPostedFiles = new ArrayList();//保留上传文件的存储
                    Hashtable TableAddFid = new Hashtable();//Add的Fid存储
                    string FirstAddFid = null;// str.GetCombID();
                    Dictionary<string, Dictionary<int, Dictionary<string, string>>> TableMultiCol = new Dictionary<string, Dictionary<int, Dictionary<string, string>>>();//多行存储的表、RowRate、列、数据
                    Dictionary<string, object[]> TableMuiltiSet = new Dictionary<string, object[]>();//多行存储的表、配置(类型、存储Fid元素Name、过滤条件)，通过fid配置实现
                    #region base
                    ArrayList DataOP_FileKey = new ArrayList();
                    Dictionary<string, string[]> DataOP_Names = new Dictionary<string, string[]>();
                    Dictionary<string, string> RootKeyValue = new Dictionary<string, string>();
                    Dictionary<string, string> AdvKeyValue = new Dictionary<string, string>();
                    Dictionary<string, bool> ModeAutoRecordDIc = new Dictionary<string, bool>();
                    ///数据操作相关的配置，值中数组内容为绑定表列、操作类型、fid设置、高级设置
                    ///除了多行的fid值特殊外，其他的值获取adv设置高于name值
                    #region 初始化 DataOP_Names 
                    int loop1 = 0;
                    foreach (string key in keyItem.Keys)
                    {
                        string dataop_name = key;
                        string dataop_tabletag = keyItem[key][1];
                        if (dataop_tabletag.Trim() != "")//数据绑定的设置为空，则不处理，后续支持@code 2019/11/14
                        {
                            if (FirstAddFid == null) FirstAddFid = DBSuit.KeyGenerate(Page.TableNameByTableTag(dataop_tabletag)).ToString();
                            int dataop_type = int.Parse(keyItem[key][2]);
                            string dataop_fid = keyItem[key][0];
                            if (dataop_fid == "_newfid_" || dataop_fid == "@newfid@") dataop_fid = FirstAddFid;//强制绑定同一个fid
                            string advset = keyItem[key][3];
                            string valuechanged = keyItem[key][4];
                            string changedValue = keyItem[key][5];
                            for (int k = 1; k < paras.Length; k++)
                            {
                                advset = advset.Replace("@p" + k + "@", paras[k]).Replace("{p" + k + "}", paras[k]);
                            }
                            if (dataop_name != "" && !dataop_name.StartsWith("_"))
                            {
                                if (!DataOP_Names.ContainsKey(dataop_name))
                                {
                                    DataOP_Names.Add(dataop_name, new string[] { dataop_tabletag, dataop_type.ToString(), dataop_fid, advset.Trim(), valuechanged, changedValue });
                                }
                            }
                            else if (advset != "" || (dataop_type >= 3 && dataop_type <= 6 && dataop_tabletag.ToLower().EndsWith("." + DBSuit.Key(Page.TableNameByTableTag(dataop_tabletag), FidCol2).KeyName.ToLower())))
                            {
                                DataOP_Names.Add("adv:" + loop1++, new string[] { dataop_tabletag, dataop_type.ToString(), dataop_fid, advset.Trim(), valuechanged, changedValue, dataop_name });
                            }
                        }
                    }
                    #endregion
                    ArrayList OPedKey = new ArrayList();
                    #region 处理文件上传

                    foreach (string key in Page.IFormFileKeys(context.Request))
                    {
                        if (!OPedKey.Contains(key)) OPedKey.Add(key);
                        string revalue = null;
                        if (DataOP_Names.ContainsKey(key))
                        {
                            DataOP_FileKey.Add(key);
                            if (key.IndexOf("_rowrate") < 0)
                            {
                                string[] dataop_para = (string[])DataOP_Names[key];
                                string dataop_tabletag = dataop_para[0];
                                int dataop_type = int.Parse(dataop_para[1]);
                                string dataop_fid = dataop_para[2];
                                string dataop_adv = dataop_para[3];
                                string dataop_table = dataop_tabletag.Split('.')[0];
                                dataop_table = dataop_table.StartsWith("@") ? dataop_table.Substring(1) : ("ft_" + siteid + "_f_" + dataop_table);
                                string dataop_col = dataop_tabletag.Split('.')[1];
                                string dataop_table_dy = dataop_table + "_dy";

                                if (dataop_adv == "")
                                    revalue = adv.UploadFile(context, key);
                                else revalue = comp.DataOPAdvVal(context, db, st, dataop_adv, FirstAddFid, 0, TableAdd, TableModDic, reqDic);

                                //选中删除要删除的文件，才删除
                                if ((string.IsNullOrEmpty(context.Request.Form["filedel_" + key].FirstOrDefault<string>()) || !context.Request.Form["filedel_" + key].FirstOrDefault<string>().Equals("1")))
                                {
                                    if (!StayedPostedFiles.Contains(dataop_table + "." + dataop_col)) StayedPostedFiles.Add(dataop_table + "." + dataop_col);
                                }

                                if ((context.Request.Form["delfile_" + key].FirstOrDefault<string>() != null && context.Request.Form["delfile_" + key].FirstOrDefault<string>().Equals("1")))
                                {
                                    revalue = "(delfile)";
                                }

                                if (revalue == null) revalue = "";

                                bool IsLockValue = key.StartsWith("_lock_");
                                string LockValue = null;
                                if (IsLockValue) LockValue = revalue;

                                if (dataop_type == 0)//Add
                                {
                                    if (!TableAdd.ContainsKey(dataop_table))
                                    {
                                        TableAdd.Add(dataop_table, new Hashtable());
                                        if (!dataop_fid.Equals(""))
                                        {
                                            TableAddFid.Add(dataop_table, dataop_fid);
                                        }
                                        else
                                        {
                                            if (TableAddFid.Count == 0) TableAddFid.Add(dataop_table, FirstAddFid);
                                            else TableAddFid.Add(dataop_table, DBSuit.KeyGenerate(dataop_table).ToString());
                                        }
                                    }
                                    if (!((Hashtable)TableAdd[dataop_table]).ContainsKey(dataop_col))
                                    {
                                        ((Hashtable)TableAdd[dataop_table]).Add(dataop_col, revalue.Equals("(delfile)") ? "" : revalue);
                                    }
                                    int rateindex = 1;
                                    IFormFile pfile = context.Request.Form.Files[key + "_rowrate" + rateindex];
                                    int FindDeep = 0;
                                    int SaveRateIndex = 1;
                                    while (pfile == null && FindDeep < SysConst.RateRowFindDeep)
                                    {
                                        FindDeep++;
                                        rateindex++;
                                        pfile = context.Request.Form.Files[key + "_rowrate" + rateindex];
                                    }
                                    while (pfile != null)
                                    {
                                        if ((context.Request.Form["delfile_" + key + "_rowrate" + rateindex].FirstOrDefault<string>() != null && context.Request.Form["delfile_" + key + "_rowrate" + rateindex].FirstOrDefault<string>().Equals("1")))
                                        {
                                            revalue = "";
                                        }
                                        else
                                        {
                                            if (dataop_adv == "")
                                                revalue = adv.UploadFile(context, key + "_rowrate" + rateindex);
                                            else revalue = comp.DataOPAdvVal(context, db, st, dataop_adv, FirstAddFid, SaveRateIndex, TableAdd, TableModDic, reqDic);
                                        }
                                        if (!TableAddDy.Contains(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString() }))
                                        {
                                            TableAddDy.Add(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString() }, IsLockValue ? LockValue : revalue);
                                        }
                                        SaveRateIndex++;
                                        rateindex++;
                                        pfile = context.Request.Form.Files[key + "_rowrate" + rateindex];
                                        FindDeep = 0;
                                        while (pfile == null && FindDeep < SysConst.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;
                                            pfile = context.Request.Form.Files[key + "_rowrate" + rateindex];
                                        }
                                    }
                                }
                                else if (dataop_type == 1)//Mod
                                {
                                    string elefid = defaultfid;
                                    if (!dataop_fid.Equals(""))
                                    {
                                        elefid = dataop_fid;
                                    }
                                    if (elefid == null || elefid.Equals("")) continue;
                                    if (!TableMod.ContainsKey(dataop_table))
                                    {
                                        TableMod.Add(dataop_table, new Hashtable());
                                        TableModDic.Add(dataop_table, new Dictionary<string, string>());
                                    }
                                    if (!((Hashtable)TableMod[dataop_table]).ContainsKey(elefid))
                                    {
                                        ((Hashtable)TableMod[dataop_table]).Add(elefid, new Hashtable());
                                    }
                                    if (!((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).ContainsKey(dataop_col))
                                    {
                                        ((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).Add(dataop_col, revalue);
                                    }
                                    if (!TableModDic[dataop_table].ContainsKey(dataop_col))
                                    {
                                        TableModDic[dataop_table].Add(dataop_col, revalue);
                                    }
                                    int rateindex = 1;
                                    IFormFile pfile = context.Request.Form.Files[key + "_rowrate" + rateindex];
                                    int FindDeep = 0;
                                    int SaveRateIndex = 1;
                                    while (pfile == null && FindDeep < SysConst.RateRowFindDeep)
                                    {
                                        FindDeep++;
                                        rateindex++;
                                        pfile = context.Request.Form.Files[key + "_rowrate" + rateindex];
                                    }
                                    while (pfile != null)
                                    {
                                        if ((context.Request.Form["delfile_" + key + "_rowrate" + rateindex].FirstOrDefault<string>() != null && context.Request.Form["delfile_" + key + "_rowrate" + rateindex].FirstOrDefault<string>().Equals("1")))
                                        {
                                            revalue = "(delfile)";
                                        }
                                        else
                                        {
                                            if (dataop_adv == "")
                                                revalue = adv.UploadFile(context, key + "_rowrate" + rateindex);
                                            else revalue = comp.DataOPAdvVal(context, db, st, dataop_adv, FirstAddFid, SaveRateIndex, TableAdd, TableModDic, reqDic);
                                        }
                                        if (!TableModDy.Contains(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString(), elefid }))
                                        {
                                            TableModDy.Add(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString(), elefid }, new string[] { IsLockValue ? LockValue : revalue, "FileBox" });
                                        }
                                        SaveRateIndex++;
                                        rateindex++;
                                        pfile = context.Request.Form.Files[key + "_rowrate" + rateindex];
                                        FindDeep = 0;
                                        while (pfile == null && FindDeep < SysConst.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;
                                            pfile = context.Request.Form.Files[key + "_rowrate" + rateindex];
                                        }
                                    }
                                }
                                else if (dataop_type >= 2 && dataop_type <= 6)//多行 FID设置认为不会在file上传中出现
                                {
                                    if (!TableMultiCol.ContainsKey(dataop_table))
                                    {
                                        TableMultiCol.Add(dataop_table, new Dictionary<int, Dictionary<string, string>>());
                                    }
                                    if (!TableMultiCol[dataop_table].ContainsKey(0))
                                    {
                                        TableMultiCol[dataop_table].Add(0, new Dictionary<string, string>());
                                    }
                                    if (!TableMultiCol[dataop_table][0].ContainsKey(dataop_col))
                                    {
                                        TableMultiCol[dataop_table][0].Add(dataop_col, revalue.Equals("(delfile)") ? "" : revalue);
                                    }
                                    int rateindex = 1;
                                    IFormFile pfile = context.Request.Form.Files[key + "_rowrate" + rateindex];
                                    int FindDeep = 0;
                                    int SaveRateIndex = 1;
                                    while (pfile == null && FindDeep < SysConst.RateRowFindDeep)
                                    {
                                        FindDeep++;
                                        rateindex++;
                                        pfile = context.Request.Form.Files[key + "_rowrate" + rateindex];
                                    }
                                    while (pfile != null)
                                    {
                                        if ((context.Request.Form["delfile_" + key + "_rowrate" + rateindex].FirstOrDefault<string>() != null && context.Request.Form["delfile_" + key + "_rowrate" + rateindex].FirstOrDefault<string>().Equals("1")))
                                        {
                                            revalue = "";
                                        }
                                        else
                                        {
                                            if (dataop_adv == "")
                                                revalue = adv.UploadFile(context, key + "_rowrate" + rateindex);
                                            else revalue = comp.DataOPAdvVal(context, db, st, dataop_adv, FirstAddFid, SaveRateIndex, TableAdd, TableModDic, reqDic);
                                        }
                                        if (!TableMultiCol[dataop_table].ContainsKey(SaveRateIndex))
                                        {
                                            TableMultiCol[dataop_table].Add(SaveRateIndex, new Dictionary<string, string>());
                                        }
                                        if (!TableMultiCol[dataop_table][SaveRateIndex].ContainsKey(dataop_col))
                                        {
                                            TableMultiCol[dataop_table][SaveRateIndex].Add(dataop_col, IsLockValue ? LockValue : revalue);
                                        }
                                        SaveRateIndex++;
                                        rateindex++;
                                        pfile = context.Request.Form.Files[key + "_rowrate" + rateindex];
                                        FindDeep = 0;
                                        while (pfile == null && FindDeep < SysConst.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;
                                            pfile = context.Request.Form.Files[key + "_rowrate" + rateindex];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region 处理字段
                    var postKeys = (!isJson ? context.Request.Form.Keys : jObject.Properties().Select(r => r.Name));
                    //foreach (string key in (!isJson ? context.Request.Form.Keys : jObject.Properties().Select(r => r.Name)))
                    foreach (string key in DataOP_Names.Keys)
                    {
                        if (!postKeys.Contains(key)) continue;
                        if (!OPedKey.Contains(key)) OPedKey.Add(key);
                        string revalue = null;
                        //if (DataOP_Names.ContainsKey(key) && !DataOP_FileKey.Contains(key))
                        if (!DataOP_FileKey.Contains(key))
                        {
                            if (key.IndexOf("_rowrate") < 0)
                            {
                                string[] dataop_para = (string[])DataOP_Names[key];
                                string dataop_tabletag = dataop_para[0];
                                int dataop_type = int.Parse(dataop_para[1]);
                                string dataop_fid = dataop_para[2];
                                string dataop_adv = dataop_para[3];
                                string dataop_table = dataop_tabletag.Split('.')[0];
                                dataop_table = dataop_table.StartsWith("@") ? dataop_table.Substring(1) : ("ft_" + siteid + "_f_" + dataop_table);
                                string dataop_col = dataop_tabletag.Split('.')[1];
                                string dataop_table_dy = dataop_table + "_dy";
                                bool valuechanged = (dataop_para[4] == "1");
                                string changedValue = dataop_para[5];
                                if (key.StartsWith("_newfid_"))
                                {
                                    //该字段的值来自于指定table的新newfid
                                    string newfid_table = key.Replace("_newfid_", "");
                                    if (TableAddFid.ContainsKey(newfid_table))
                                    {
                                        revalue = TableAddFid[newfid_table].ToString();
                                    }
                                    else
                                    {
                                        revalue = "";
                                    }
                                }
                                else
                                {
                                    if (dataop_adv == "")
                                        revalue = adv.PostStrSafe(valuechanged ? changedValue : ReqFormOrJsonVal(isJson, req, jObject, key, "[FTNULL]", "[FTNULL]"), key);
                                    else
                                    {
                                        revalue = comp.DataOPAdvVal(context, db, st, dataop_adv, FirstAddFid, 0, TableAdd, TableModDic, reqDic, valuechanged ? changedValue : ReqFormOrJsonVal(isJson, req, jObject, key, "[FTNULL]", "[FTNULL]"));
                                        if (!AdvKeyValue.ContainsKey(key)) AdvKeyValue.Add(key, revalue);
                                    }
                                }
                                if (revalue == null) revalue = "";

                                if (revalue.StartsWith("@code("))
                                {
                                    revalue = Interface.Code.Get(adv.GetSpecialBase(context, revalue, siteid), reqDic, context);
                                }
                                string _key = key.ToLower().Trim();
                                if (_key.StartsWith('_')) _key = _key.Substring(1);
                                if (!RootKeyValue.ContainsKey(_key)) RootKeyValue.Add(_key, revalue);

                                bool IsLockValue = key.StartsWith("_lock_");
                                string LockValue = null;
                                if (IsLockValue) LockValue = revalue;

                                //处理Auto
                                if (dataop_type == 13)//Auto
                                {
                                    string autofid = defaultfid;
                                    if (!dataop_fid.Equals(""))
                                    {
                                        autofid = dataop_fid;
                                    }
                                    string CurFidCol = DBSuit.Key(dataop_table, FidCol).KeyName;
                                    string CusWhere = null;
                                    bool noRecord = true;
                                    string ori_autofid = autofid;
                                    if (ModeAutoRecordDIc.ContainsKey(dataop_table + "|" + ori_autofid))
                                    {
                                        noRecord = ModeAutoRecordDIc[dataop_table + "|" + ori_autofid];
                                    }
                                    else
                                    {
                                        if (autofid.Trim().StartsWith("where ", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            CusWhere = autofid.Trim();
                                        }
                                        else if (autofid.IndexOf(',') >= 0)
                                        {
                                            CurFidCol = autofid.Split(',')[1];
                                            autofid = autofid.Split(',')[0];
                                        }
                                        string sql = "select count(*) as ca from " + (dataop_table.IndexOf('#') > 0 ? (dataop_table.Split('#')[0]) : dataop_table).D2();//支持多次操作同表的情况
                                        if (CusWhere != null) sql += " " + CusWhere;
                                        else
                                        {
                                            sql += " where " + str.D2DD(CurFidCol) + "='" + str.D2DD(autofid) + "'";
                                        }
                                        using (DR dr = db.OpenRecord(sql, st))
                                        {
                                            if (dr.Read())
                                            {
                                                if (dr.GetInt32(0) > 0) noRecord = false;
                                            }
                                        }
                                        ModeAutoRecordDIc.Add(dataop_table + "|" + ori_autofid, noRecord);
                                    }
                                    if (noRecord)
                                    {
                                        dataop_type = 0;
                                        dataop_fid = "";
                                    }
                                    else
                                    {
                                        dataop_type = 1;
                                    }
                                }

                                if (dataop_type == 0)//Add
                                {
                                    if (!TableAdd.ContainsKey(dataop_table))
                                    {
                                        TableAdd.Add(dataop_table, new Hashtable());
                                        if (!dataop_fid.Equals(""))
                                        {
                                            TableAddFid.Add(dataop_table, dataop_fid);
                                        }
                                        else
                                        {
                                            if (TableAddFid.Count == 0) TableAddFid.Add(dataop_table, FirstAddFid);
                                            else TableAddFid.Add(dataop_table, DBSuit.KeyGenerate(dataop_table).ToString());
                                        }
                                    }
                                    if (!((Hashtable)TableAdd[dataop_table]).ContainsKey(dataop_col))
                                    {
                                        ((Hashtable)TableAdd[dataop_table]).Add(dataop_col, revalue);
                                    }
                                    int rateindex = 1;
                                    revalue = adv.PostStrSafe(valuechanged ? changedValue : ReqFormOrJsonVal(isJson, req, jObject, key + "_rowrate" + rateindex, null), key);
                                    int FindDeep = 0;
                                    int SaveRateIndex = 1;
                                    while (revalue == null && FindDeep < SysConst.RateRowFindDeep)
                                    {
                                        FindDeep++;
                                        rateindex++;//valuechanged ? changedValue : 
                                        revalue = adv.PostStrSafe(ReqFormOrJsonVal(isJson, req, jObject, key + "_rowrate" + rateindex, null), key);
                                    }
                                    while (revalue != null && revalue != "[FTNULL]")
                                    {
                                        if (dataop_adv != "") revalue = comp.DataOPAdvVal(context, db, st, dataop_adv, FirstAddFid, SaveRateIndex, TableAdd, TableModDic, reqDic, revalue);
                                        if (revalue.StartsWith("@code("))
                                        {
                                            revalue = Interface.Code.Get(adv.GetSpecialBase(context, revalue, siteid), reqDic, context);
                                        }
                                        if (!TableAddDy.Contains(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString() }))
                                        {
                                            TableAddDy.Add(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString() }, IsLockValue ? LockValue : revalue);
                                        }
                                        SaveRateIndex++;
                                        rateindex++;//valuechanged ? changedValue : 
                                        revalue = adv.PostStrSafe(ReqFormOrJsonVal(isJson, req, jObject, key + "_rowrate" + rateindex, null), key);
                                        FindDeep = 0;
                                        while (revalue == null && FindDeep < SysConst.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;//valuechanged ? changedValue : 
                                            revalue = adv.PostStrSafe(ReqFormOrJsonVal(isJson, req, jObject, key + "_rowrate" + rateindex, null), key);
                                        }
                                    }
                                }
                                else if (dataop_type == 1)//Mod
                                {
                                    string elefid = defaultfid;
                                    if (!dataop_fid.Equals(""))
                                    {
                                        elefid = dataop_fid;
                                    }
                                    if (!TableMod.ContainsKey(dataop_table))
                                    {
                                        TableMod.Add(dataop_table, new Hashtable());
                                    }
                                    if (!TableModDic.ContainsKey(dataop_table))
                                    {
                                        TableModDic.Add(dataop_table, new Dictionary<string, string>());
                                    }
                                    if (!((Hashtable)TableMod[dataop_table]).ContainsKey(elefid))
                                    {
                                        ((Hashtable)TableMod[dataop_table]).Add(elefid, new Hashtable());
                                    }
                                    if (!((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).ContainsKey(dataop_col))
                                    {
                                        ((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).Add(dataop_col, revalue);
                                    }
                                    if (!TableModDic[dataop_table].ContainsKey(dataop_col))
                                    {
                                        TableModDic[dataop_table].Add(dataop_col, revalue);
                                    }
                                    int rateindex = 1;
                                    revalue = adv.PostStrSafe(valuechanged ? changedValue : ReqFormOrJsonVal(isJson, req, jObject, key + "_rowrate" + rateindex, null), key);
                                    int FindDeep = 0;
                                    int SaveRateIndex = 1;
                                    while (revalue == null && FindDeep < SysConst.RateRowFindDeep)
                                    {
                                        FindDeep++;
                                        rateindex++;//valuechanged ? changedValue : 
                                        revalue = adv.PostStrSafe(ReqFormOrJsonVal(isJson, req, jObject, key + "_rowrate" + rateindex, null), key);
                                    }
                                    while (revalue != null && revalue != "[FTNULL]")
                                    {
                                        if (dataop_adv != "") revalue = comp.DataOPAdvVal(context, db, st, dataop_adv, FirstAddFid, SaveRateIndex, TableAdd, TableModDic, reqDic, revalue);
                                        if (revalue.StartsWith("@code("))
                                        {
                                            revalue = Interface.Code.Get(adv.GetSpecialBase(context, revalue, siteid), reqDic, context);
                                        }
                                        if (!TableModDy.Contains(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString(), elefid }))
                                        {
                                            TableModDy.Add(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString(), elefid }, new string[] { IsLockValue ? LockValue : revalue, "TextBox" });
                                        }
                                        SaveRateIndex++;
                                        rateindex++;//valuechanged ? changedValue : 
                                        revalue = adv.PostStrSafe(ReqFormOrJsonVal(isJson, req, jObject, key + "_rowrate" + rateindex, null), key);
                                        FindDeep = 0;
                                        while (revalue == null && FindDeep < SysConst.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;//valuechanged ? changedValue : 
                                            revalue = adv.PostStrSafe(ReqFormOrJsonVal(isJson, req, jObject, key + "_rowrate" + rateindex, null), key);
                                        }
                                    }
                                }
                                else if (dataop_type >= 2 && dataop_type <= 6)//多行 
                                {
                                    if (dataop_col.ToLower() == DBSuit.Key(dataop_table, FidCol2).KeyName.ToLower())//处理多行配置
                                    {
                                        //3 多行新增 不需要 name值
                                        //4 多行重置 不需要 name值
                                        //5 多行重置保留FID  需要 name值
                                        //6 多行仅更新 需要name值
                                        //在此全部处理，若未添加name值导致处理不到的，则在无name值处理
                                        if (dataop_type >= 3 && dataop_type <= 6)
                                        {
                                            if (!TableMuiltiSet.ContainsKey(dataop_table))
                                            {
                                                TableMuiltiSet.Add(dataop_table, new object[] {
                                                    dataop_type,key
                                                    ,comp.DataOPAdvVal(context, db, st, dataop_adv, FirstAddFid, 0,TableAdd,TableModDic,reqDic)
                                                }); ;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!TableMultiCol.ContainsKey(dataop_table))
                                        {
                                            TableMultiCol.Add(dataop_table, new Dictionary<int, Dictionary<string, string>>());
                                        }
                                        if (!TableMultiCol[dataop_table].ContainsKey(0))
                                        {
                                            TableMultiCol[dataop_table].Add(0, new Dictionary<string, string>());
                                        }
                                        if (!TableMultiCol[dataop_table][0].ContainsKey(dataop_col))
                                        {
                                            TableMultiCol[dataop_table][0].Add(dataop_col, revalue);
                                        }
                                        int rateindex = 1;
                                        revalue = adv.PostStrSafe(valuechanged ? changedValue : ReqFormOrJsonVal(isJson, req, jObject, key + "_rowrate" + rateindex, null), key);
                                        int FindDeep = 0;
                                        int SaveRateIndex = 1;
                                        while (revalue == null && FindDeep < SysConst.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;//valuechanged ? changedValue : 
                                            revalue = adv.PostStrSafe(ReqFormOrJsonVal(isJson, req, jObject, key + "_rowrate" + rateindex, null), key);
                                        }
                                        while (revalue != null && revalue != "[FTNULL]")
                                        {
                                            if (dataop_adv != "") revalue = comp.DataOPAdvVal(context, db, st, dataop_adv, FirstAddFid, SaveRateIndex, TableAdd, TableModDic, reqDic, revalue);
                                            if (revalue.StartsWith("@code("))
                                            {
                                                revalue = Interface.Code.Get(adv.GetSpecialBase(context, revalue, siteid), reqDic, context);
                                            }
                                            if (!TableMultiCol[dataop_table].ContainsKey(SaveRateIndex))
                                            {
                                                TableMultiCol[dataop_table].Add(SaveRateIndex, new Dictionary<string, string>());
                                            }
                                            if (!TableMultiCol[dataop_table][SaveRateIndex].ContainsKey(dataop_col))
                                            {
                                                TableMultiCol[dataop_table][SaveRateIndex].Add(dataop_col, IsLockValue ? LockValue : revalue);
                                            }
                                            SaveRateIndex++;
                                            rateindex++;//valuechanged ? changedValue : 
                                            revalue = adv.PostStrSafe(ReqFormOrJsonVal(isJson, req, jObject, key + "_rowrate" + rateindex, null), key);
                                            FindDeep = 0;
                                            while (revalue == null && FindDeep < SysConst.RateRowFindDeep)
                                            {
                                                FindDeep++;
                                                rateindex++;//valuechanged ? changedValue : 
                                                revalue = adv.PostStrSafe(ReqFormOrJsonVal(isJson, req, jObject, key + "_rowrate" + rateindex, null), key);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    //处理没有name值的DataOP_Names
                    #region 处理NoName
                    IDictionaryEnumerator _Names = DataOP_Names.GetEnumerator();
                    while (_Names.MoveNext())
                    {
                        if (_Names.Key.ToString().StartsWith("adv:"))
                        {
                            string[] dataop_para = (string[])(_Names.Value);
                            string dataop_tabletag = dataop_para[0];
                            int dataop_type = int.Parse(dataop_para[1]);
                            string dataop_fid = dataop_para[2];
                            string dataop_adv = dataop_para[3];
                            string dataop_table = dataop_tabletag.Split('.')[0];
                            dataop_table = dataop_table.StartsWith("@") ? dataop_table.Substring(1) : ("ft_" + siteid + "_f_" + dataop_table);
                            string dataop_col = dataop_tabletag.Split('.')[1];
                            string revalue = comp.DataOPAdvVal(context, db, st, dataop_adv, FirstAddFid, 0, TableAdd, TableModDic, reqDic);
                            string dataop_name = dataop_para[6];
                            if (dataop_name.StartsWith('_')) dataop_name = dataop_name.Substring(1);
                            if (!RootKeyValue.ContainsKey(dataop_name.ToLower().Trim())) RootKeyValue.Add(dataop_name.ToLower().Trim(), revalue);
                            if (!AdvKeyValue.ContainsKey(dataop_name)) AdvKeyValue.Add(dataop_name, revalue);
                            //处理Auto
                            if (dataop_type == 13)//Auto
                            {
                                string autofid = defaultfid;
                                if (!dataop_fid.Equals(""))
                                {
                                    autofid = dataop_fid;
                                }
                                string CurFidCol = DBSuit.Key(dataop_table, FidCol).KeyName;
                                string CusWhere = null;
                                bool noRecord = true;
                                string ori_autofid = autofid;
                                if (ModeAutoRecordDIc.ContainsKey(dataop_table + "|" + ori_autofid))
                                {
                                    noRecord = ModeAutoRecordDIc[dataop_table + "|" + ori_autofid];
                                }
                                else
                                {
                                    if (autofid.Trim().StartsWith("where ", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        CusWhere = autofid.Trim();
                                    }
                                    else if (autofid.IndexOf(',') >= 0)
                                    {
                                        CurFidCol = autofid.Split(',')[1];
                                        autofid = autofid.Split(',')[0];
                                    }
                                    string sql = "select count(*) as ca from " + dataop_table.D2();
                                    if (CusWhere != null) sql += " " + CusWhere;
                                    else
                                    {
                                        sql += " where " + str.D2DD(CurFidCol) + "='" + str.D2DD(autofid) + "'";
                                    }
                                    using (DR dr = db.OpenRecord(sql, st))
                                    {
                                        if (dr.Read())
                                        {
                                            if (dr.GetInt32(0) > 0) noRecord = false;
                                        }
                                    }
                                    ModeAutoRecordDIc.Add(dataop_table + "|" + ori_autofid, noRecord);
                                }
                                if (noRecord)
                                {
                                    dataop_type = 0;
                                    dataop_fid = "";
                                }
                                else
                                {
                                    dataop_type = 1;
                                }
                            }
                            if (dataop_type == 0)
                            {
                                if (!TableAdd.ContainsKey(dataop_table))
                                {
                                    TableAdd.Add(dataop_table, new Hashtable());
                                    if (!dataop_fid.Equals(""))
                                    {
                                        TableAddFid.Add(dataop_table, dataop_fid);
                                    }
                                    else
                                    {
                                        if (TableAddFid.Count == 0) TableAddFid.Add(dataop_table, FirstAddFid);
                                        else TableAddFid.Add(dataop_table, DBSuit.KeyGenerate(dataop_table).ToString());
                                    }
                                }
                                if (!((Hashtable)TableAdd[dataop_table]).ContainsKey(dataop_col))
                                {
                                    ((Hashtable)TableAdd[dataop_table]).Add(dataop_col, revalue);
                                }
                            }
                            else if (dataop_type == 1)
                            {
                                string elefid = defaultfid;
                                if (!dataop_fid.Equals(""))
                                {
                                    elefid = dataop_fid;
                                }
                                if (!TableMod.ContainsKey(dataop_table))
                                {
                                    TableMod.Add(dataop_table, new Hashtable());
                                }
                                if (!TableModDic.ContainsKey(dataop_table))
                                {
                                    TableModDic.Add(dataop_table, new Dictionary<string, string>());
                                }
                                if (!((Hashtable)TableMod[dataop_table]).ContainsKey(elefid))
                                {
                                    ((Hashtable)TableMod[dataop_table]).Add(elefid, new Hashtable());
                                }
                                if (!((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).ContainsKey(dataop_col))
                                {
                                    ((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).Add(dataop_col, revalue);
                                }
                                if (!TableModDic[dataop_table].ContainsKey(dataop_col))
                                {
                                    TableModDic[dataop_table].Add(dataop_col, revalue);
                                }
                            }
                            else if (dataop_type >= 2 && dataop_type <= 6)//多行 
                            {
                                if (dataop_col.ToLower() == DBSuit.Key(dataop_table, FidCol2).KeyName.ToLower())//处理多行配置
                                {
                                    //3 多行新增 不需要 name值
                                    //4 多行重置 不需要 name值
                                    //5 多行重置保留FID  需要 name值
                                    //6 多行仅更新 需要name值
                                    if (dataop_type >= 3 && dataop_type <= 6)
                                    {
                                        if (!TableMuiltiSet.ContainsKey(dataop_table))
                                        {
                                            TableMuiltiSet.Add(dataop_table, new object[] {
                                                    dataop_type,""
                                                    ,comp.DataOPAdvVal(context, db, st, dataop_adv, FirstAddFid, 0,TableAdd,TableModDic,reqDic)
                                                }); ;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!TableMultiCol.ContainsKey(dataop_table))
                                    {
                                        TableMultiCol.Add(dataop_table, new Dictionary<int, Dictionary<string, string>>());
                                    }
                                    if (!TableMultiCol[dataop_table].ContainsKey(0))
                                    {
                                        TableMultiCol[dataop_table].Add(0, new Dictionary<string, string>());
                                    }
                                    if (!TableMultiCol[dataop_table][0].ContainsKey(dataop_col))
                                    {
                                        TableMultiCol[dataop_table][0].Add(dataop_col, revalue);
                                    }
                                }
                            }

                            //处理过的无Name存到已经处理过的Keys，防止被当选项框未选中而置空
                            OPedKey.Add(_Names.Key.ToString());
                        }
                    }
                    #endregion
                    //复选框、单选框未选中时，得不到值，则默认为空 ，动态新增行时可能有Bug，不建议在动态新增行使用单选、复选
                    _Names = DataOP_Names.GetEnumerator();
                    while (_Names.MoveNext())
                    {
                        if (!OPedKey.Contains(_Names.Key.ToString()) && !_Names.Key.ToString().StartsWith("adv:"))
                        {
                            string[] dataop_para = (string[])_Names.Value;
                            string dataop_tabletag = dataop_para[0];
                            int dataop_type = int.Parse(dataop_para[1]);
                            string dataop_fid = dataop_para[2];
                            string dataop_table = dataop_tabletag.Split('.')[0];
                            dataop_table = dataop_table.StartsWith("@") ? dataop_table.Substring(1) : ("ft_" + siteid + "_f_" + dataop_table);
                            string dataop_col = dataop_tabletag.Split('.')[1];
                            string dataop_table_dy = dataop_table + "_dy";
                            //处理Auto
                            if (dataop_type == 13)//Auto
                            {
                                string autofid = defaultfid;
                                if (!dataop_fid.Equals(""))
                                {
                                    autofid = dataop_fid;
                                }
                                string CurFidCol = DBSuit.Key(dataop_table, FidCol).KeyName;
                                string CusWhere = null;
                                bool noRecord = true;
                                string ori_autofid = autofid;
                                if (ModeAutoRecordDIc.ContainsKey(dataop_table + "|" + ori_autofid))
                                {
                                    noRecord = ModeAutoRecordDIc[dataop_table + "|" + ori_autofid];
                                }
                                else
                                {
                                    if (autofid.Trim().StartsWith("where ", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        CusWhere = autofid.Trim();
                                    }
                                    else if (autofid.IndexOf(',') >= 0)
                                    {
                                        CurFidCol = autofid.Split(',')[1];
                                        autofid = autofid.Split(',')[0];
                                    }
                                    string sql = "select count(*) as ca from " + dataop_table.D2();
                                    if (CusWhere != null) sql += " " + CusWhere;
                                    else
                                    {
                                        sql += " where " + str.D2DD(CurFidCol) + "='" + str.D2DD(autofid) + "'";
                                    }
                                    using (DR dr = db.OpenRecord(sql, st))
                                    {
                                        if (dr.Read())
                                        {
                                            if (dr.GetInt32(0) > 0) noRecord = false;
                                        }
                                    }
                                    ModeAutoRecordDIc.Add(dataop_table + "|" + ori_autofid, noRecord);
                                }
                                if (noRecord)
                                {
                                    dataop_type = 0;
                                    dataop_fid = "";
                                }
                                else
                                {
                                    dataop_type = 1;
                                }
                            }
                            if (dataop_type == 0)
                            {
                                if (!TableAdd.ContainsKey(dataop_table))
                                {
                                    TableAdd.Add(dataop_table, new Hashtable());
                                    if (TableAddFid.Count == 0) TableAddFid.Add(dataop_table, FirstAddFid);
                                    else TableAddFid.Add(dataop_table, DBSuit.KeyGenerate(dataop_table).ToString());
                                }
                                if (!((Hashtable)TableAdd[dataop_table]).ContainsKey(dataop_col))
                                {
                                    ((Hashtable)TableAdd[dataop_table]).Add(dataop_col, "");
                                }
                            }
                            else if (dataop_type == 1)
                            {
                                string elefid = defaultfid;
                                if (!dataop_fid.Equals(""))
                                {
                                    elefid = dataop_fid;
                                }
                                if (!TableMod.ContainsKey(dataop_table))
                                {
                                    TableMod.Add(dataop_table, new Hashtable());
                                }
                                if (!TableModDic.ContainsKey(dataop_table))
                                {
                                    TableModDic.Add(dataop_table, new Dictionary<string, string>());
                                }

                                if (!((Hashtable)TableMod[dataop_table]).ContainsKey(elefid))
                                {
                                    ((Hashtable)TableMod[dataop_table]).Add(elefid, new Hashtable());
                                }
                                if (!((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).ContainsKey(dataop_col))
                                {
                                    ((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).Add(dataop_col, "");
                                }
                                if (!TableModDic[dataop_table].ContainsKey(dataop_col))
                                {
                                    TableModDic[dataop_table].Add(dataop_col, "");
                                }
                            }
                            else if (dataop_type >= 2 && dataop_type <= 6)//多行
                            {
                                if (!TableMultiCol.ContainsKey(dataop_table))
                                {
                                    TableMultiCol.Add(dataop_table, new Dictionary<int, Dictionary<string, string>>());
                                }
                                if (!TableMultiCol[dataop_table].ContainsKey(0))
                                {
                                    TableMultiCol[dataop_table].Add(0, new Dictionary<string, string>());
                                }
                                if (!TableMultiCol[dataop_table][0].ContainsKey(dataop_col))
                                {
                                    TableMultiCol[dataop_table][0].Add(dataop_col, "");
                                }
                            }
                        }
                    }
                    _Names = null;
                    OPedKey.Clear();
                    OPedKey = null;
                    DataOP_FileKey.Clear();
                    DataOP_FileKey = null;
                    DataOP_Names.Clear();
                    DataOP_Names = null;
                    ModeAutoRecordDIc.Clear();
                    ModeAutoRecordDIc = null;
                    #endregion
                    #region 操作前代码或参数
                    string[] codebeforeitem = codebefore.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string codei in codebeforeitem.Select(r=>r.Trim()))
                    {
                        if (codei.StartsWith("@code("))
                        {
                            string codebeforereval = Interface.Code.Get(codei, reqDic, context);
                            if (codebeforereval != null)
                            {
                                st.Rollback();
                                return FTFrame.Project.Core.Api.ErrorJson(codebeforereval);
                            }
                        }
                        else if (codei.StartsWith("@para{"))
                        {
                            string codebeforereval = adv.ExecOperation(context, db, st, adv.ParaPattern(context, codei, reqDic), reqDic);
                            if (codebeforereval != null)
                            {
                                st.Rollback();
                                return FTFrame.Project.Core.Api.ErrorJson(codebeforereval);
                            }
                        }
                    }
                    #endregion
                    if (0 > 1)
                    {
                    }
                    else
                    {
                        #region 数据操作
                        //action start
                        string fid = null;
                        string FirstModFid = null;
                        #region Add
                        if (TableAdd.Count > 0)
                        {
                            //新增动态新增行
                            //IDictionaryEnumerator TableAddDyEnum = TableAddDy.GetEnumerator();
                            //while (TableAddDyEnum.MoveNext())
                            //{
                            //    string tablenamedy = ((string[])TableAddDyEnum.Key)[0];
                            //    fid = TableAddFid[tablenamedy.Substring(0, tablenamedy.Length - 3)].ToString();
                            //    if (fid == null || fid.Equals("")) continue;
                            //    string sql = "insert into " + tablenamedy + "(fid,eid,evalue,erate)";
                            //    sql += "values('" + fid + "','" + str.D2DD(((string[])TableAddDyEnum.Key)[1]) + "'," + (TableAddDyEnum.Value.ToString().Equals("[FTNULL]") ? "null" : ("'" + str.D2DD(TableAddDyEnum.Value.ToString()) + "'")) + "," + int.Parse(((string[])TableAddDyEnum.Key)[2]) + ")";
                            //    db.ExecSql(sql, st);
                            //}

                            //TableAddDyEnum = null;
                            //新增主表
                            IDictionaryEnumerator TableAddEnum = TableAdd.GetEnumerator();
                            while (TableAddEnum.MoveNext())
                            {
                                string tablename = TableAddEnum.Key.ToString();
                                fid = TableAddFid[tablename].ToString();
                                //fidval,fidcol
                                string CurFidCol = DBSuit.Key(tablename, FidCol).KeyName;
                                if (fid.IndexOf(',') >= 0)
                                {
                                    CurFidCol = fid.Split(',')[1];
                                    fid = fid.Split(',')[0];
                                }
                                //返回第一张新增表的fid
                                if (_newfid_.Equals("")) _newfid_ = fid;
                                //得到DyData
                                //DR drte = db.OpenRecord(Sql.TableExists(tablename + "_dy"), st);
                                //bool TableExists = drte.HasRows;
                                //drte.Close();
                                //string dyDataStr = "";
                                string sql = null;
                                //if (TableExists)
                                //{
                                //    sql = "select distinct eid from " + tablename + "_dy where fid='" + fid + "'";
                                //    DR dydr = db.OpenRecord(sql, st);
                                //    while (dydr.Read())
                                //    {
                                //        dyDataStr += "," + dydr.GetString(0);
                                //    }
                                //    dydr.Close();
                                //    if (!dyDataStr.Equals("")) dyDataStr = dyDataStr.Substring(0);
                                //}
                                var IsKeyColInSql = DBSuit.Key(tablename, FidCol).KeyType != Enums.KeyType.AutoIncrement;
                                if (IsKeyColInSql) sql = "insert into " + str.D2DD(tablename).Normal() + "(" + str.D2DD(CurFidCol).Normal() + "";
                                else sql = "insert into " + str.D2DD(tablename).Normal() + "(";
                                string OpDefaultCol_col = "";
                                string OpDefaultCol_val = "";
                                if (OpDefaultCol == 1)
                                {
                                    var dic = DBSuit.DefaultColsWhenAddForApi(tablename, reqDic);
                                    foreach (var dicItem in dic)
                                    {
                                        if (((Hashtable)TableAddEnum.Value).ContainsKey(dicItem.Key)) continue;
                                        OpDefaultCol_col += "," + dicItem.Key;
                                        OpDefaultCol_val += "," + (dicItem.Value == null ? "null" : ("'" + dicItem.Value.ToString().D2() + "'"));
                                    }
                                }
                                //sql += OpDefaultCol_col;
                                string colstr = "";
                                string colvalue = "";
                                //流水号
                                foreach (string[] liquiditem in LiquidAL)
                                {
                                    if (tablename.Equals(liquiditem[0]))
                                    {
                                        string liquid_id = Advance.GetLiquidID(liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                        int liquid_loop = 0;
                                        while (!Advance.IsLiquidIDOK(liquiditem[0], liquiditem[1], liquid_id))
                                        {
                                            if (liquid_loop > 99)
                                            {
                                                liquid_id = "[error]loop99";
                                                break;
                                            }
                                            liquid_loop++;
                                            liquid_id = Advance.GetLiquidID(liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                        }
                                        colstr += "," + func.SQLColSafe(liquiditem[1]).Normal();
                                        colvalue += "," + "'" + str.D2DD(liquid_id) + "'";
                                    }
                                }
                                IDictionaryEnumerator TableAddRowEnum = ((Hashtable)TableAddEnum.Value).GetEnumerator();
                                while (TableAddRowEnum.MoveNext())
                                {
                                    colstr += "," + func.SQLColSafe(TableAddRowEnum.Key.ToString()).Normal();
                                    colvalue += "," + (TableAddRowEnum.Value.ToString().Equals("[FTNULL]") ? "null" : ("'" + str.D2DD(TableAddRowEnum.Value.ToString()) + "'"));
                                }
                                if (IsKeyColInSql)
                                {
                                    sql = sql + OpDefaultCol_col + colstr + ")values('" + fid.D2() + "'";
                                    sql += OpDefaultCol_val;
                                    sql += colvalue + ")";
                                }
                                else
                                {
                                    sql = sql + (OpDefaultCol_col + colstr).Substring(1) + ")values(";
                                    sql += (OpDefaultCol_val + colvalue).Substring(1);
                                    sql += ")";
                                }
                                //sql += OpDefaultCol_val;
                                //if (OpDefaultCol == 1) sql += ",'" + str.D2DD(memid) + "','" + str.D2DD(memid) + "','" + str.GetDateTime() + "','" + str.GetDateTime() + "','" + str.D2DD(dyDataStr) + "',1,0,0";
                                //sql += colvalue + ")";
                                if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(sql, "[Form Data Add]", context, reqDic);
                                db.ExecSql(sql, st);

                                FTFrame.Project.Core.Action.ActionSave(tablename, context, 0);
                                Form.NewOPLogV4(db, st, context, tableolog, fid, 1, tablename, memid);

                                TableAddRowEnum = null;
                            }
                            TableAddEnum = null;
                        }
                        #endregion
                        #region Mod
                        if (TableMod.Count > 0)
                        {
                            IDictionaryEnumerator TableModEnum = TableMod.GetEnumerator();
                            while (TableModEnum.MoveNext())
                            {
                                string tablename = TableModEnum.Key.ToString();
                                //DR drte = db.OpenRecord(Sql.TableExists(tablename + "_dy"), st);
                                //bool TableExists = drte.HasRows;
                                //drte.Close();
                                IDictionaryEnumerator TableModFIDEnum = ((Hashtable)TableModEnum.Value).GetEnumerator();
                                while (TableModFIDEnum.MoveNext())
                                {
                                    string _flowstr = "";
                                    string modfid = TableModFIDEnum.Key.ToString();
                                    if (FirstModFid == null) FirstModFid = modfid;
                                    //fidval,fidcol
                                    string CurFidCol = DBSuit.Key(tablename, FidCol).KeyName;
                                    string CusWhere = null;
                                    if (modfid.Trim().StartsWith("where ", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        CusWhere = modfid.Trim();
                                    }
                                    else if (modfid.IndexOf(',') >= 0)
                                    {
                                        CurFidCol = modfid.Split(',')[1];
                                        modfid = modfid.Split(',')[0];
                                    }
                                    string sql = null;
                                    bool ColHasAdded = false;
                                    string OpDefaultCol_colval = "";
                                    if (OpDefaultCol == 1)
                                    {
                                        var dic = DBSuit.DefaultColsWhenModForApi(tablename, reqDic);
                                        int loopi = 0;
                                        foreach (var dicItem in dic)
                                        {
                                            if (((Hashtable)TableModFIDEnum.Value).ContainsKey(dicItem.Key)) continue;
                                            if (loopi > 0) OpDefaultCol_colval += ",";
                                            OpDefaultCol_colval += dicItem.Key + "=" + (dicItem.Value == null ? "null" : ("'" + dicItem.Value.ToString().D2() + "'"));
                                            loopi++;
                                        }
                                    }
                                    if (OpDefaultCol == 1 && OpDefaultCol_colval != "")
                                    {
                                        sql = "update " + tablename.Normal() + " set " + OpDefaultCol_colval + _flowstr;
                                        ColHasAdded = true;
                                    }
                                    else
                                    {
                                        sql = "update " + tablename.Normal() + " set ";
                                        ColHasAdded = false;
                                    }
                                    IDictionaryEnumerator TableModRowEnum = ((Hashtable)TableModFIDEnum.Value).GetEnumerator();
                                    while (TableModRowEnum.MoveNext())
                                    {
                                        if (!StayedPostedFiles.Contains(tablename + "." + TableModRowEnum.Key.ToString()) || !TableModRowEnum.Value.ToString().Equals(""))
                                        {
                                            if (ColHasAdded) sql += ",";
                                            if (TableModRowEnum.Value.ToString().Equals("[FTNULL]"))
                                                sql += func.SQLColSafe(TableModRowEnum.Key.ToString()).Normal() + "=null";
                                            else
                                                sql += func.SQLColSafe(TableModRowEnum.Key.ToString()).Normal() + "='" + str.D2DD(TableModRowEnum.Value.ToString().Equals("(delfile)") ? "" : TableModRowEnum.Value.ToString()) + "'";
                                            ColHasAdded = true;
                                        }
                                        if (!StayedPostedFiles.Contains(tablename + "." + TableModRowEnum.Key.ToString()))
                                        {
                                            //if (TableExists)
                                            //{
                                            //    string sqlele = "delete from " + tablename + "_dy where fid='" + str.D2DD(modfid) + "' and eid='" + str.D2DD(TableModRowEnum.Key.ToString()) + "'";
                                            //    db.ExecSql(sqlele, st);
                                            //}
                                        }
                                    }
                                    if (CusWhere != null) sql += " " + CusWhere;
                                    else
                                    {
                                        sql += " where " + str.D2DD(CurFidCol).Normal() + "='" + str.D2DD(modfid) + "'";
                                    }
                                    if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(sql, "[Form Data Mod]", context, reqDic);
                                    db.ExecSql(sql, st);

                                    FTFrame.Project.Core.Action.ActionSave(tablename, context, 1);
                                    Form.NewOPLogV4(db, st, context, tableolog, fid, 2, tablename, memid);

                                    TableModRowEnum = null;
                                }
                                TableModFIDEnum = null;
                            }
                            //修改动态新增行

                            //IDictionaryEnumerator TableModDyEnum = TableModDy.GetEnumerator();
                            //while (TableModDyEnum.MoveNext())
                            //{
                            //    string sql = null;
                            //    string tablenamedy = ((string[])TableModDyEnum.Key)[0];
                            //    //FileBox要保留的话，更新
                            //    string _eletype = ((string[])TableModDyEnum.Value)[1];
                            //    if (_eletype.Equals("FileBox"))
                            //    {
                            //        if (StayedPostedFiles.Contains(tablenamedy.Substring(0, tablenamedy.Length - 3) + "." + ((string[])TableModDyEnum.Key)[1]))
                            //        {
                            //            if (!((string[])TableModDyEnum.Value)[0].Equals(""))//如果有新文件上传
                            //            {
                            //                string sqlele = "delete from " + tablenamedy + " where fid='" + str.D2DD(((string[])TableModDyEnum.Key)[3]) + "' and eid='" + str.D2DD(((string[])TableModDyEnum.Key)[1]) + "' and erate=" + int.Parse(((string[])TableModDyEnum.Key)[2]);
                            //                db.ExecSql(sqlele, st);
                            //                sql = "insert into " + tablenamedy + "(fid,eid,evalue,erate)";
                            //                sql += "values('" + str.D2DD(((string[])TableModDyEnum.Key)[3]) + "','" + str.D2DD(((string[])TableModDyEnum.Key)[1]) + "','" + str.D2DD(((string[])TableModDyEnum.Value)[0]) + "'," + int.Parse(((string[])TableModDyEnum.Key)[2]) + ")";
                            //                db.ExecSql(sql, st);
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        sql = "insert into " + tablenamedy + "(fid,eid,evalue,erate)";
                            //        sql += "values('" + str.D2DD(((string[])TableModDyEnum.Key)[3]) + "','" + str.D2DD(((string[])TableModDyEnum.Key)[1]) + "'," + (((string[])TableModDyEnum.Value)[0].Equals("[FTNULL]") ? "null" : ("'" + str.D2DD(((string[])TableModDyEnum.Value)[0]) + "'")) + "," + int.Parse(((string[])TableModDyEnum.Key)[2]) + ")";
                            //        db.ExecSql(sql, st);
                            //    }
                            //}
                            ////处理dy表中的(delfile)且重新对erate排序
                            //List<string> DyDelFileTagList = new List<string>();
                            //TableModDyEnum = TableModDy.GetEnumerator();
                            //while (TableModDyEnum.MoveNext())
                            //{
                            //    string sql = null;
                            //    string tag_tablenamedy = ((string[])TableModDyEnum.Key)[0];
                            //    string tag_fid = ((string[])TableModDyEnum.Key)[3];
                            //    string tag_eid = ((string[])TableModDyEnum.Key)[1];
                            //    string dytag = tag_tablenamedy + "_" + tag_fid + "_" + tag_eid;
                            //    if (!DyDelFileTagList.Contains(dytag))
                            //    {
                            //        DyDelFileTagList.Add(dytag);
                            //        sql = "delete from " + tag_tablenamedy + " where fid='" + str.D2DD(tag_fid) + "' and  eid='" + str.D2DD(tag_eid) + "' and evalue='(delfile)'";
                            //        db.ExecSql(sql, st);
                            //        List<int[]> DyDelFileErateList = new List<int[]>();
                            //        sql = "select erate from " + tag_tablenamedy + " where fid='" + str.D2DD(tag_fid) + "' and  eid='" + str.D2DD(tag_eid) + "' and evalue!='(delfile)' order by erate";
                            //        int newrate = 1;
                            //        using (DR dyDr = db.OpenRecord(sql, st))
                            //        {
                            //            while (dyDr.Read())
                            //            {
                            //                DyDelFileErateList.Add(new int[] { dyDr.GetInt32(0), newrate++ });
                            //            }
                            //        }
                            //        foreach (int[] dyDelRate in DyDelFileErateList)
                            //        {
                            //            sql = "update " + tag_tablenamedy + " set erate=" + dyDelRate[1] + " where fid='" + str.D2DD(tag_fid) + "'  and erate=" + dyDelRate[0] + " and  eid='" + str.D2DD(tag_eid) + "'";
                            //            db.ExecSql(sql, st);
                            //        }
                            //        DyDelFileErateList.Clear();
                            //    }
                            //}
                            //DyDelFileTagList.Clear();
                            //更新dydata
                            //if (OpDefaultCol == 1)
                            //{
                            //TableModEnum = TableMod.GetEnumerator();
                            //while (TableModEnum.MoveNext())
                            //{
                            //    string tablename = TableModEnum.Key.ToString();
                            //    IDictionaryEnumerator TableModFIDEnum = ((Hashtable)TableModEnum.Value).GetEnumerator();
                            //    while (TableModFIDEnum.MoveNext())
                            //    {
                            //        string modfid = TableModFIDEnum.Key.ToString();
                            //        //得到DyData
                            //        DR drte = db.OpenRecord(Sql.TableExists(tablename + "_dy"), st);
                            //        bool TableExists = drte.HasRows;
                            //        drte.Close();
                            //        string dyDataStr = "";
                            //        string sql = null;
                            //        if (TableExists)
                            //        {
                            //            sql = "select distinct eid from " + tablename + "_dy where fid='" + str.D2DD(modfid) + "'";
                            //            DR dydr = db.OpenRecord(sql, st);
                            //            while (dydr.Read())
                            //            {
                            //                dyDataStr += "," + dydr.GetString(0);
                            //            }
                            //            dydr.Close();
                            //            if (!dyDataStr.Equals("")) dyDataStr = dyDataStr.Substring(0);
                            //        }

                            //        sql = "update " + tablename + " set dydata='" + str.D2DD(dyDataStr) + "' where " + str.D2DD(FidCol) + "='" + str.D2DD(modfid) + "'";
                            //        db.ExecSql(sql, st);
                            //    }
                            //    TableModFIDEnum = null;
                            //}

                            //}
                            TableModEnum = null;
                        }
                        #endregion
                        #region 多行
                        if (TableMuiltiSet.Count > 0)
                        {
                            foreach (string TableName in TableMuiltiSet.Keys)
                            {
                                if (!TableMultiCol.ContainsKey(TableName)) continue;
                                object[] MuiltiSet = TableMuiltiSet[TableName];
                                int MuitiType = (int)MuiltiSet[0];
                                string FidKey = (string)MuiltiSet[1];
                                string FilterCdn = (string)MuiltiSet[2];
                                //3 多行新增 不需要 name值 不需要cdn
                                //4 多行重置 不需要 name值  需要cdn
                                //5 多行重置保留FID  需要 name值 需要cdn
                                //6 多行仅更新 需要name值 不需要cdn
                                int RowRate;
                                string sql;
                                string sqlCol;
                                string sqlVal;
                                if (MuitiType == 3)//多行新增 
                                {
                                    bool IsDelRow1 = (context.Request.Form["Row1DeleteHdn"].FirstOrDefault<string>() != null && context.Request.Form["Row1DeleteHdn"].FirstOrDefault<string>() == "yes");//删除首行标记
                                    RowRate = IsDelRow1 ? 1 : 0;
                                    while (TableMultiCol[TableName].ContainsKey(RowRate))
                                    {
                                        bool colAdded = false;
                                        sqlCol = "insert into " + func.SQLColSafe(TableName).Normal() + "(";
                                        sqlVal = "values(";
                                        var IsKeyColInSql = DBSuit.Key(TableName, FidCol2).KeyType != Enums.KeyType.AutoIncrement;
                                        if (IsKeyColInSql)
                                        {
                                            sqlCol += str.D2DD(DBSuit.Key(TableName, FidCol2).KeyName);
                                            sqlVal += "'" + DBSuit.KeyGenerate(TableName).ToString() + "'";
                                            colAdded = true;
                                        }
                                        //流水号
                                        foreach (string[] liquiditem in LiquidAL)
                                        {
                                            if (TableName.Equals(liquiditem[0]))
                                            {
                                                string liquid_id = Advance.GetLiquidID(db, st, liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                                int liquid_loop = 0;
                                                while (!Advance.IsLiquidIDOK(db, st, liquiditem[0], liquiditem[1], liquid_id))
                                                {
                                                    if (liquid_loop > 99)
                                                    {
                                                        liquid_id = "[error]loop99";
                                                        break;
                                                    }
                                                    liquid_loop++;
                                                    liquid_id = Advance.GetLiquidID(db, st, liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                                }
                                                if (colAdded)
                                                {
                                                    sqlCol += "," + func.SQLColSafe(liquiditem[1]).Normal();
                                                    sqlVal += "," + "'" + str.D2DD(liquid_id) + "'";
                                                }
                                                else
                                                {
                                                    sqlCol += func.SQLColSafe(liquiditem[1]).Normal();
                                                    sqlVal += "'" + str.D2DD(liquid_id) + "'";
                                                    colAdded = true;
                                                }
                                            }
                                        }
                                        //col value
                                        foreach (string col in TableMultiCol[TableName][RowRate].Keys)
                                        {
                                            string val = TableMultiCol[TableName][RowRate][col];
                                            if (colAdded)
                                            {
                                                sqlCol += "," + func.SQLColSafe(col).Normal();
                                                sqlVal += "," + ((val == null || val.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(val) + "'"));
                                            }
                                            else
                                            {
                                                sqlCol += func.SQLColSafe(col).Normal();
                                                sqlVal += ((val == null || val.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(val) + "'"));
                                                colAdded = true;
                                            }
                                        }
                                        sqlCol += ")";
                                        sqlVal += ")";
                                        if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(sqlCol + sqlVal, "[Form Data Muilti " + MuitiType + "]", context, reqDic);
                                        db.ExecSql(sqlCol + sqlVal, st);
                                        RowRate++;
                                    }
                                }
                                else if (MuitiType == 4 && FilterCdn != "")//多行重置 
                                {
                                    sql = "delete from " + func.SQLColSafe(TableName).Normal() + " where " + FilterCdn;
                                    if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(sql, "[Form Data Muilti " + MuitiType + "]", context, reqDic);
                                    db.ExecSql(sql, st);
                                    bool IsDelRow1 = (context.Request.Form["Row1DeleteHdn"].FirstOrDefault<string>() != null && context.Request.Form["Row1DeleteHdn"].FirstOrDefault<string>() == "yes");//删除首行标记
                                    RowRate = IsDelRow1 ? 1 : 0;
                                    while (TableMultiCol[TableName].ContainsKey(RowRate))
                                    {
                                        bool colAdded = false;
                                        sqlCol = "insert into " + func.SQLColSafe(TableName).Normal() + "(";
                                        sqlVal = "values(";
                                        var IsKeyColInSql = DBSuit.Key(TableName, FidCol2).KeyType != Enums.KeyType.AutoIncrement;
                                        if (IsKeyColInSql)
                                        {
                                            sqlCol += str.D2DD(DBSuit.Key(TableName, FidCol2).KeyName);
                                            sqlVal += "'" + DBSuit.KeyGenerate(TableName).ToString() + "'";
                                            colAdded = true;
                                        }
                                        //流水号
                                        foreach (string[] liquiditem in LiquidAL)
                                        {
                                            if (TableName.Equals(liquiditem[0]))
                                            {
                                                string liquid_id = Advance.GetLiquidID(db, st, liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                                int liquid_loop = 0;
                                                while (!Advance.IsLiquidIDOK(db, st, liquiditem[0], liquiditem[1], liquid_id))
                                                {
                                                    if (liquid_loop > 99)
                                                    {
                                                        liquid_id = "[error]loop99";
                                                        break;
                                                    }
                                                    liquid_loop++;
                                                    liquid_id = Advance.GetLiquidID(db, st, liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                                }
                                                if (colAdded)
                                                {
                                                    sqlCol += "," + func.SQLColSafe(liquiditem[1]).Normal();
                                                    sqlVal += "," + "'" + str.D2DD(liquid_id) + "'";
                                                }
                                                else
                                                {
                                                    sqlCol += func.SQLColSafe(liquiditem[1]).Normal();
                                                    sqlVal += "'" + str.D2DD(liquid_id) + "'";
                                                    colAdded = true;
                                                }
                                            }
                                        }
                                        //col value
                                        foreach (string col in TableMultiCol[TableName][RowRate].Keys)
                                        {
                                            string val = TableMultiCol[TableName][RowRate][col];
                                            if (colAdded)
                                            {
                                                sqlCol += "," + func.SQLColSafe(col).Normal();
                                                sqlVal += "," + ((val == null || val.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(val) + "'"));
                                            }
                                            else
                                            {
                                                sqlCol += func.SQLColSafe(col).Normal();
                                                sqlVal += ((val == null || val.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(val) + "'"));
                                                colAdded = true;
                                            }
                                        }
                                        sqlCol += ")";
                                        sqlVal += ")";
                                        if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(sqlCol + sqlVal, "[Form Data Muilti " + MuitiType + "]", context, reqDic);
                                        db.ExecSql(sqlCol + sqlVal, st);
                                        RowRate++;
                                    }
                                }
                                else if (MuitiType == 5 && FidKey != "" && FilterCdn != "")//多行重置保留FID 
                                {
                                    sql = "delete from " + func.SQLColSafe(TableName).Normal() + " where " + FilterCdn;
                                    if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(sql, "[Form Data Muilti " + MuitiType + "]", context, reqDic);
                                    db.ExecSql(sql, st);
                                    bool IsDelRow1 = (context.Request.Form["Row1DeleteHdn"].FirstOrDefault<string>() != null && context.Request.Form["Row1DeleteHdn"].FirstOrDefault<string>() == "yes");//删除首行标记
                                    RowRate = IsDelRow1 ? 1 : 0;
                                    while (TableMultiCol[TableName].ContainsKey(RowRate))
                                    {
                                        //sqlCol = "insert into " + func.SQLColSafe(TableName) + "(";
                                        //sqlVal = "values(";
                                        //sqlCol += str.D2DD(DBSuit.Key(TableName, FidCol2).KeyName);
                                        //string oriFid = context.Request.Form[FidKey + (RowRate == 0 ? "" : ("_rowrate" + RowRate))];
                                        //if (oriFid == null || oriFid.Trim() == "") sqlVal += "'" + DBSuit.KeyGenerate(TableName).ToString() + "'";
                                        //else sqlVal += "'" + str.D2DD(oriFid) + "'";
                                        bool colAdded = false;
                                        sqlCol = "insert into " + func.SQLColSafe(TableName).Normal() + "(";
                                        sqlVal = "values(";
                                        string oriFid = context.Request.Form[FidKey + (RowRate == 0 ? "" : ("_rowrate" + RowRate))];
                                        if (!string.IsNullOrWhiteSpace(oriFid))
                                        {
                                            sqlCol += str.D2DD(DBSuit.Key(TableName, FidCol2).KeyName);
                                            sqlVal += "'" + str.D2DD(oriFid) + "'";
                                            colAdded = true;
                                        }
                                        else
                                        {
                                            var IsKeyColInSql = DBSuit.Key(TableName, FidCol2).KeyType != Enums.KeyType.AutoIncrement;
                                            if (IsKeyColInSql)
                                            {
                                                sqlCol += str.D2DD(DBSuit.Key(TableName, FidCol2).KeyName);
                                                sqlVal += "'" + DBSuit.KeyGenerate(TableName).ToString() + "'";
                                                colAdded = true;
                                            }
                                        }

                                        //流水号
                                        foreach (string[] liquiditem in LiquidAL)
                                        {
                                            if (TableName.Equals(liquiditem[0]))
                                            {
                                                string liquid_id = Advance.GetLiquidID(db, st, liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                                int liquid_loop = 0;
                                                while (!Advance.IsLiquidIDOK(db, st, liquiditem[0], liquiditem[1], liquid_id))
                                                {
                                                    if (liquid_loop > 99)
                                                    {
                                                        liquid_id = "[error]loop99";
                                                        break;
                                                    }
                                                    liquid_loop++;
                                                    liquid_id = Advance.GetLiquidID(db, st, liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                                }
                                                if (colAdded)
                                                {
                                                    sqlCol += "," + func.SQLColSafe(liquiditem[1]).Normal();
                                                    sqlVal += "," + "'" + str.D2DD(liquid_id) + "'";
                                                }
                                                else
                                                {
                                                    sqlCol += func.SQLColSafe(liquiditem[1]).Normal();
                                                    sqlVal += "'" + str.D2DD(liquid_id) + "'";
                                                    colAdded = true;
                                                }
                                            }
                                        }
                                        //col value
                                        foreach (string col in TableMultiCol[TableName][RowRate].Keys)
                                        {
                                            string val = TableMultiCol[TableName][RowRate][col];
                                            if (colAdded)
                                            {
                                                sqlCol += "," + func.SQLColSafe(col).Normal();
                                                sqlVal += "," + ((val == null || val.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(val) + "'"));
                                            }
                                            else
                                            {
                                                sqlCol += func.SQLColSafe(col).Normal();
                                                sqlVal += ((val == null || val.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(val) + "'"));
                                                colAdded = true;
                                            }
                                        }
                                        sqlCol += ")";
                                        sqlVal += ")";
                                        if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(sqlCol + sqlVal, "[Form Data Muilti " + MuitiType + "]", context, reqDic);
                                        db.ExecSql(sqlCol + sqlVal, st);
                                        RowRate++;
                                    }
                                }
                                else if (MuitiType == 6 && FidKey != "")//多行仅更新 
                                {
                                    RowRate = 0;
                                    while (TableMultiCol[TableName].ContainsKey(RowRate))
                                    {
                                        string oriFid = context.Request.Form[FidKey + (RowRate == 0 ? "" : ("_rowrate" + RowRate))];
                                        sql = "update " + func.SQLColSafe(TableName).Normal() + " set ";
                                        //col value
                                        bool IsFirst = true;
                                        foreach (string col in TableMultiCol[TableName][RowRate].Keys)
                                        {
                                            string val = TableMultiCol[TableName][RowRate][col];
                                            if (!IsFirst) sql += ",";
                                            sql += func.SQLColSafe(col).Normal() + "=" + ((val == null || val.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(val) + "'"));
                                            IsFirst = false;
                                        }
                                        sql += " where " + str.D2DD(DBSuit.Key(TableName, FidCol2).KeyName) + "='" + str.D2DD(oriFid) + "'";
                                        if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(sql, "[Form Data Muilti " + MuitiType + "]", context, reqDic);
                                        db.ExecSql(sql, st);
                                        RowRate++;
                                    }
                                }
                            }
                        }
                        #endregion
                        #region Json
                        foreach (Model.DPData Data in DataList)
                        {
                            var parentFid = FirstModFid ?? _newfid_;
                            if(string.IsNullOrEmpty(parentFid))
                            {
                                parentFid = defaultfidForMod;
                                for (int k = 1; k < paras.Length; k++)
                                {
                                    parentFid = parentFid.Replace("@p" + k + "@", paras[k].Replace("'", "")).Replace("{p" + k + "}", paras[k].Replace("'", ""));
                                }
                            }
                            string jsondataopResult = DataOPJsonDataOP(context, Data, db, st, parentFid, siteid, RootKeyValue, TableAdd, TableModDic, reqDic,IsOutputLog);
                            if (jsondataopResult != null)
                            {
                                st.Rollback();
                                return FTFrame.Project.Core.Api.ErrorJson(jsondataopResult);
                            }
                        }
                        #endregion
                        #endregion
                    }
                    DataList.Clear();
                    DataList = null;
                    keyValue.Clear();
                    keyValue = null;
                    keyItem.Clear();
                    keyItem = null;
                    TableAdd.Clear();
                    TableAdd = null;
                    TableAddDy.Clear();
                    TableAddDy = null;
                    TableAddFid.Clear();
                    TableAddFid = null;
                    TableMod.Clear();
                    TableMod = null;
                    TableModDic.Clear();
                    TableModDic = null;
                    TableModDy.Clear();
                    TableModDy = null;
                    TableMultiCol.Clear();
                    TableMultiCol = null;
                    TableMuiltiSet.Clear();
                    TableMuiltiSet = null;
                    StayedPostedFiles.Clear();
                    StayedPostedFiles = null;
                    LiquidAL.Clear();
                    LiquidAL = null;
                    RootKeyValue.Clear();
                    RootKeyValue = null;


                    if (execsqlafter.StartsWith("@ForceComit#"))
                    {
                        st.Commit();
                        st = db.GetTransaction();
                        execsqlafter = execsqlafter.Substring("@ForceComit#".Length);
                    }
                    Page.FormSqlExec(db, st, execsqlafter, "", _newfid_, context);
                    codeafter = codeafter.Replace("_newfid_", _newfid_).Replace("@newfid@", _newfid_);

                    if (codeafter.StartsWith("@ForceComit#"))
                    {
                        st.Commit();
                        st=db.GetTransaction();
                        codeafter = codeafter.Substring("@ForceComit#".Length);
                    }

                    string[] codeafteritem = codeafter.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string codei in codeafteritem.Select(r => r.Trim()))
                    {
                        if (codei.StartsWith("@code("))
                        {
                            string codeafterreval = Interface.Code.Get(codei, reqDic, context);
                            if (codeafterreval != null)
                            {
                                if(codeafterreval.StartsWith("@AddKeyValue#"))
                                {
                                    var codeafterrevalitems = codeafterreval.Substring("@AddKeyValue#".Length).Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                    foreach(var codeafterrevalitem in codeafterrevalitems)
                                    {
                                        var codeafterrevalitemitems = codeafterrevalitem.Split(new string[] { "#" }, StringSplitOptions.None);
                                        if (!AdvKeyValue.ContainsKey(codeafterrevalitemitems[0])) AdvKeyValue.Add(codeafterrevalitemitems[0], codeafterrevalitemitems[1]);
                                    }
                                }
                                else
                                {
                                    st.Rollback();
                                    return FTFrame.Project.Core.Api.ErrorJson(codeafterreval);
                                }
                            }
                        }
                        else if (codei.StartsWith("@para{"))
                        {
                            string codeafterreval = adv.ExecOperation(context, db, st, adv.ParaPattern(context, codei, reqDic), reqDic);
                            if (codeafterreval != null)
                            {
                                if (codeafterreval.StartsWith("@AddKeyValue#"))
                                {
                                    var codeafterrevalitems = codeafterreval.Split(new string[] { "#" }, StringSplitOptions.None);
                                    if (!AdvKeyValue.ContainsKey(codeafterrevalitems[1])) AdvKeyValue.Add(codeafterrevalitems[1], codeafterrevalitems[2]);
                                }
                                else
                                {
                                    st.Rollback();
                                    return FTFrame.Project.Core.Api.ErrorJson(codeafterreval);
                                }
                            }
                        }
                    }
                    st.Commit();

                    var returnJsonStr = "{}";
                    if (!jssuc.Equals(""))
                    {
                        returnJsonStr = FTFrame.Project.Core.Api.OperationSuccessJson(jssuc, AdvKeyValue, _newfid_);
                    }
                    else
                    {
                        returnJsonStr = FTFrame.Project.Core.Api.OperationSuccessJson("Operation successful", AdvKeyValue, _newfid_);
                    }
                    if (IsOutputLog.ShowOutputLog)
                    {
                        //输出json格式化日志
                        using StringWriter textWriter = new StringWriter();
                        JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                        {
                            Formatting = Newtonsoft.Json.Formatting.Indented,
                            Indentation = 4,//缩进字符数
                            IndentChar = ' '//缩进字符
                        };
                        new JsonSerializer().Serialize(jsonWriter, JObject.Parse(returnJsonStr.ToString()));
                        FTFrame.Project.Core.Api.LogDebug(textWriter.ToString(), context, reqDic);
                    }
                    return returnJsonStr;
                }
                catch (Exception ex)
                {
                    st.Rollback();
                    FTFrame.Project.Core.Api.LogError(ex, context, reqDic);
                    return FTFrame.Project.Core.Api.ExceptionJson(ex);
                }
                finally
                {
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError(ex, context, reqDic);
                return FTFrame.Project.Core.Api.ExceptionJson(ex);
            }
            finally
            {
                FTFrame.Project.Core.Api.OPDuplicateOut(OPDuplicateObj);
            }
        }
        public static string DataOPJsonDataOP(HttpContext context, DPData Data, DB db, ST st, string ParentFid, string SiteId, Dictionary<string, string> RootKeyValue, Hashtable AddHT, Dictionary<string, Dictionary<string, string>> TableModDic, Dictionary<string, object> reqDic, (bool ShowBaseLog, bool ShowInputLog, bool ShowOutputLog) IsOutputLog)
        {
            string TableName = Data.TableName.StartsWith("@") ? Data.TableName.Substring(1) : ("ft_" + SiteId + "_f_" + Data.TableName);
            if (Data.LeiXing == 7 || Data.LeiXing == 10)//新增 需要唯一一个主键列定义
            {
                List<DPDataRow> DataRow = Data.DataRow;
                foreach (DPDataRow row in DataRow)
                {
                    string FidCol = null;
                    string colstr = "";
                    string sql = null;
                    string FidValue = DBSuit.KeyGenerate(TableName).ToString();
                    string colvalue = "";
                    var loop = 0;
                    var ignoreDic= new Dictionary<int, bool>();
                    foreach ((DPItem OpItem, string Value, DPData _Data) Item in row.ValueList)
                    {
                        string ColName = Item.OpItem.BindData.IndexOf('.') > 0 ? Item.OpItem.BindData.Split('.')[1] : Item.OpItem.BindData;
                        if (!Item.OpItem.IsJson && ColName != FidCol)
                        {
                            string revalue = null;
                            if (Item.OpItem.Special == 2)
                            {
                                revalue = ParentFid;
                            }
                            else if (Item.OpItem.Special == 3)
                            {
                                string tempName = Item.OpItem.Name.ToLower().Trim();
                                if (tempName.StartsWith("_")) tempName = tempName.Substring(1);
                                if (RootKeyValue.ContainsKey(tempName))
                                {
                                    revalue = RootKeyValue[tempName];
                                }
                            }
                            else
                            {
                                if (Item.OpItem.Advance.Trim() == "")
                                    revalue = adv.PostStrSafe(Item.Value, Item.OpItem.Name);
                                else revalue = comp.DataOPAdvVal(context, db, st, Item.OpItem.Advance, ParentFid, 0, AddHT, TableModDic, reqDic, Item.Value);
                            }
                            loop++;
                            ignoreDic.Add(loop, revalue != null); 
                            if (revalue != null)//不入参则忽略
                            {
                                colvalue += "," + ((revalue == null || revalue.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(revalue) + "'"));
                            }
                        }
                        else if (Item.OpItem.IsJson && Item._Data != null)
                        {
                            string result = DataOPJsonDataOP(context, Item._Data, db, st, FidValue, SiteId, RootKeyValue, AddHT, TableModDic, reqDic, IsOutputLog);
                            if (result != null) return result;
                        }
                    }
                    loop = 0;
                    foreach ((DPItem OpItem, string Value, DPData _Data) Item in row.ValueList)
                    {
                        if (!Item.OpItem.IsJson)
                        {
                            string ColName = Item.OpItem.BindData.IndexOf('.') > 0 ? Item.OpItem.BindData.Split('.')[1] : Item.OpItem.BindData;
                            if (Item.OpItem.Special == 1)//新增时 主键列前端不需要传
                            {
                                if (FidCol != null) return "主键列只能设置一项 " + ColName;
                                FidCol = ColName;
                            }
                            else
                            {
                                loop++;
                                if (ignoreDic[loop])
                                {
                                    colstr += "," + func.SQLColSafe(ColName).Normal();
                                }
                            }
                        }
                    }
                    
                    var KeySuit = DBSuit.Key(TableName, FidCol);
                    var IsKeyColInSql = KeySuit.KeyType != Enums.KeyType.AutoIncrement;
                    if (FidCol == null) FidCol = KeySuit.KeyName;
                    if (FidCol == null && IsKeyColInSql) return "没有设置主键列 ";
                    if (IsKeyColInSql)
                    {
                        sql = "insert into " + TableName.Normal() + "(" + FidCol + colstr + ")values('" + FidValue + "'" + colvalue + ")";
                    }
                    else
                    {
                        sql = "insert into " + TableName.Normal() + "(" + colstr.Substring(1) + ")values(" + colvalue.Substring(1) + ")";
                    }
                    ignoreDic.Clear();
                    if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(sql, "[SonTable Data Add]", context, reqDic);
                    db.ExecSql(sql, st);
                }
            }
            else if (Data.LeiXing == 8 || Data.LeiXing == 11)//重置 需要唯一一个主键列定义 
            {
                string Condition = Data.AdvSet ?? "";
                Condition = string.IsNullOrWhiteSpace(Data.AdvSet) ? "" : (" where " + Data.AdvSet);
                string sql = "delete from " + TableName.Normal() + Condition;
                if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(sql, "[SonTable Data Delete]", context, reqDic);
                db.ExecSql(sql, st);
                List<DPDataRow> DataRow = Data.DataRow;
                foreach (DPDataRow row in DataRow)
                {
                    string FidCol = null;
                    string colstr = "";
                    string FidValue = DBSuit.KeyGenerate(TableName).ToString();
                    string colvalue = "";
                    var loop = 0;
                    var ignoreDic = new Dictionary<int, bool>();
                    foreach ((DPItem OpItem, string Value, DPData _Data) Item in row.ValueList)
                    {
                        string ColName = Item.OpItem.BindData.IndexOf('.') > 0 ? Item.OpItem.BindData.Split('.')[1] : Item.OpItem.BindData;
                        if (!Item.OpItem.IsJson && ColName != FidCol)
                        {
                            string revalue = null;
                            if (Item.OpItem.Special == 2)
                            {
                                revalue = ParentFid;
                            }
                            else if (Item.OpItem.Special == 3)
                            {
                                string tempName = Item.OpItem.Name.ToLower().Trim();
                                if (tempName.StartsWith("_")) tempName = tempName.Substring(1);
                                if (RootKeyValue.ContainsKey(tempName))
                                {
                                    revalue = RootKeyValue[tempName];
                                }
                            }
                            else
                            {
                                if (Item.OpItem.Advance.Trim() == "")
                                    revalue = adv.PostStrSafe(Item.Value, Item.OpItem.Name);
                                else revalue = comp.DataOPAdvVal(context, db, st, Item.OpItem.Advance, ParentFid, 0, AddHT, TableModDic, reqDic, Item.Value);
                            }
                            loop++;
                            ignoreDic.Add(loop, revalue != null);
                            if (revalue != null)//不入参则忽略
                            {
                                colvalue += "," + ((revalue == null || revalue.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(revalue) + "'"));
                            }
                        }
                        else if (Item.OpItem.IsJson && Item._Data != null)
                        {
                            string result = DataOPJsonDataOP(context, Item._Data, db, st, FidValue, SiteId, RootKeyValue, AddHT, TableModDic, reqDic, IsOutputLog);
                            if (result != null) return result;
                        }
                    }
                    loop = 0;
                    foreach ((DPItem OpItem, string Value, DPData _Data) Item in row.ValueList)
                    {
                        if (!Item.OpItem.IsJson)
                        {
                            string ColName = Item.OpItem.BindData.IndexOf('.') > 0 ? Item.OpItem.BindData.Split('.')[1] : Item.OpItem.BindData;
                            if (Item.OpItem.Special == 1)//新增时 主键列前端不需要传
                            {
                                if (FidCol != null) return "主键列只能设置一项 " + ColName;
                                FidCol = ColName;
                            }
                            else
                            {
                                loop++;
                                if (ignoreDic[loop])
                                {
                                    colstr += "," + func.SQLColSafe(ColName).Normal();
                                }
                            }
                        }
                    }
                    
                    var KeySuit = DBSuit.Key(TableName, FidCol);
                    var IsKeyColInSql = KeySuit.KeyType != Enums.KeyType.AutoIncrement;
                    if (FidCol == null) FidCol = KeySuit.KeyName;
                    if (FidCol == null && IsKeyColInSql) return "没有设置主键列 ";
                    if (IsKeyColInSql)
                    {
                        sql = "insert into " + TableName.Normal() + "(" + FidCol + colstr + ")values('" + FidValue + "'" + colvalue + ")";
                    }
                    else
                    {
                        sql = "insert into " + TableName.Normal() + "(" + colstr.Substring(1) + ")values(" + colvalue.Substring(1) + ")";
                    }
                    ignoreDic.Clear();
                    if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(sql, "[SonTable Data Add]", context, reqDic);
                    db.ExecSql(sql, st);
                }
            }
            else if (Data.LeiXing == 9 || Data.LeiXing == 12)//更新 必须定义唯一的主键列，且该主键列必须有值传递
            {
                List<DPDataRow> DataRow = Data.DataRow;
                foreach (DPDataRow row in DataRow)
                {
                    string FidCol = null;
                    string FidValue = null;
                    string sql = null;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("update " + TableName.Normal() + " set ");
                    int loop = 0;
                    foreach ((DPItem OpItem, string Value, DPData _Data) Item in row.ValueList)
                    {
                        if (!Item.OpItem.IsJson)
                        {
                            string ColName = Item.OpItem.BindData.IndexOf('.') > 0 ? Item.OpItem.BindData.Split('.')[1] : Item.OpItem.BindData;
                            if (Item.OpItem.Special == 1)
                            {
                                if (FidCol != null) return "主键列只能设置一项 " + ColName;
                                FidCol = ColName;
                                if (string.IsNullOrWhiteSpace(Item.Value)) return "获取的主键值为空" + ColName;
                                FidValue = Item.Value;
                            }
                            else
                            {
                                string revalue = null;
                                if (Item.OpItem.Special == 2)
                                {
                                    revalue = ParentFid;
                                }
                                else if (Item.OpItem.Special == 3)
                                {
                                    string tempName = Item.OpItem.Name.ToLower().Trim();
                                    if (tempName.StartsWith("_")) tempName = tempName.Substring(1);
                                    if (RootKeyValue.ContainsKey(tempName))
                                    {
                                        revalue = RootKeyValue[tempName];
                                    }
                                }
                                else
                                {
                                    if (Item.OpItem.Advance.Trim() == "")
                                        revalue = adv.PostStrSafe(Item.Value, Item.OpItem.Name);
                                    else revalue = comp.DataOPAdvVal(context, db, st, Item.OpItem.Advance, ParentFid, 0, AddHT, TableModDic, reqDic, Item.Value);
                                }
                                if (revalue != null)
                                {
                                    if (loop > 0) sb.Append(",");
                                    sb.Append(func.SQLColSafe(ColName).Normal() + "=" + ((revalue == null || revalue.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(revalue) + "'")));
                                    loop++;
                                }
                            }
                        }
                        else if (Item.OpItem.IsJson && Item._Data != null)
                        {
                            string result = DataOPJsonDataOP(context, Item._Data, db, st, FidValue, SiteId, RootKeyValue, AddHT, TableModDic, reqDic,IsOutputLog);
                            if (result != null) return result;
                        }
                    }
                    var KeySuit = DBSuit.Key(TableName, FidCol);
                    if (FidCol == null) FidCol = KeySuit.KeyName;
                    if (FidCol == null) return "没有设置主键列 ";
                    sb.Append(" where " + FidCol.Normal() + "='" + FidValue.D2() + "'");
                    sql = sb.ToString();
                    if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(sql, "[SonTable Data Update]", context, reqDic);
                    db.ExecSql(sql, st);
                    sb.Clear();
                }
            }
            return null;
        }
        public static string DataOPKeyValueJson(bool isInputJson, HttpContext context, JObject mainJObject, string json, List<Model.DPData> DPDataList, string RootKeyName, int LeiXing, string TableName, string AdvSet, List<string> FormKeys, string[] paras)
        {
            //7,8,9 Form.10,11,12 Json
            if (string.IsNullOrWhiteSpace(json) || json == "[]") return null;
            json = json.Replace("$#$#$", "##");
            JArray ja = JArray.Parse(json);
            List<JToken> list = ja.ToList<JToken>();
            Dictionary<string, JArray> PostJson = new Dictionary<string, JArray>();
            Model.DPData Data = new Model.DPData() { TableName = TableName, LeiXing = LeiXing, AdvSet = AdvSet, DataRow = new List<Model.DPDataRow>() };
            foreach (JToken jt in list)
            {
                if (jt.Type.ToString() == "Object")
                {
                    List<JToken> list1 = ((JObject)jt).ToList<JToken>();
                    string item = ((JProperty)list1[0]).Value.ToString();
                    string[] row = item.Split(new string[] { "##" }, StringSplitOptions.None);
                    string caption = row[0].Trim();
                    string name = row[1].Trim();
                    string binddata = str.AesDecrypt_Fit(row[2].Trim());
                    int leixing = int.Parse(str.AesDecrypt_Fit(row[3].Trim()));
                    string jiaoyan = row[4].Trim();
                    int special = int.Parse(row[5].Trim());
                    string advset = str.AesDecrypt_Fit(row[7].Trim());
                    bool IsJson = (leixing >= 7 && leixing <= 12);
                    for (int k = 1; k < paras.Length; k++)
                    {
                        binddata = binddata.Replace("@p" + k + "@", paras[k]).Replace("{p" + k + "}", paras[k]);
                        advset = advset.Replace("@p" + k + "@", paras[k]).Replace("{p" + k + "}", paras[k]);
                    }
                    binddata = DataOPRootValue(binddata, true, context.Request, mainJObject);
                    advset = DataOPRootValue(advset, true, context.Request, mainJObject);
                    Model.DPItem OpItem = new Model.DPItem()
                    {
                        Name = name,
                        Caption = caption,
                        LeiXing = leixing,
                        Validate = jiaoyan,
                        BindData = binddata,
                        Special = special,
                        Advance = advset,
                        IsJson = IsJson
                    };
                    if (LeiXing >= 10 && LeiXing <= 12)
                    {
                        string postJson = ReqFormOrJsonVal(isInputJson, context.Request, mainJObject, RootKeyName, null);
                        if (postJson == null || postJson == "")
                        {
                            FTFrame.Project.Core.Api.LogError("No Post Json For Key " + RootKeyName, context);
                        }
                        else
                        {
                            JArray postJa;
                            if (PostJson.ContainsKey(RootKeyName)) postJa = PostJson[RootKeyName];
                            else
                            {
                                postJa = JArray.Parse(postJson);
                                PostJson.Add(RootKeyName, postJa);
                            }
                            int loop = 0;
                            foreach (JToken postJt in postJa)
                            {
                                var postJtObj = postJt[name] ?? postJt["_AutoKey_"];
                                if (postJtObj != null)
                                {
                                    if (postJtObj.Type.ToString() != "Array")
                                    {
                                        string val = postJtObj.Value<string>();
                                        string vali = DataOPValidate(ref val, jiaoyan, caption);
                                        if (vali != null) return vali;
                                        if (Data.DataRow.Count < (loop + 1)) Data.DataRow.Add(new DPDataRow()); Data.DataRow[loop].ValueList.Add((OpItem, val, null));
                                        loop++;
                                    }
                                    else
                                    {
                                        DPData _Data = new DPData() { TableName = binddata, LeiXing = leixing, AdvSet = advset, DataRow = new List<Model.DPDataRow>() };
                                        if (Data.DataRow.Count < (loop + 1)) Data.DataRow.Add(new DPDataRow()); Data.DataRow[loop].ValueList.Add((OpItem, null, _Data));
                                        string result = DataOPKeyValueJsonLoop(isInputJson, context, mainJObject, ((JProperty)list1[1]).Value.ToString(), _Data, postJtObj, leixing, FormKeys, paras);
                                        if (result != null) return result;
                                        loop++;
                                    }
                                }
                                else
                                {
                                    string val = null;
                                    string vali = DataOPValidate(ref val, jiaoyan, caption);
                                    if (vali != null) return vali;
                                    if (Data.DataRow.Count < (loop + 1)) Data.DataRow.Add(new DPDataRow()); Data.DataRow[loop].ValueList.Add((OpItem, val, null));
                                    loop++;
                                }
                            }
                        }
                    }
                    else if (((JProperty)list1[1]).Value.Type.ToString() != "Array")
                    {
                        if (LeiXing >= 7 && LeiXing <= 9)
                        {
                            string val;
                            if (FormKeys.Contains(name))
                            {
                                val = adv.UploadFile(context, name);
                            }
                            else val = ReqFormOrJsonVal(isInputJson, context.Request, mainJObject, name, null);
                            string vali = DataOPValidate(ref val, jiaoyan, caption);
                            if (vali != null) return vali;
                            if (Data.DataRow.Count < 1) Data.DataRow.Add(new DPDataRow()); Data.DataRow[0].ValueList.Add((OpItem, val, null));
                            int loop = 1;
                            while (ReqFormOrJsonVal(isInputJson, context.Request, mainJObject, name + "_rowrate" + loop, null) != null)
                            {
                                if (FormKeys.Contains(name + "_rowrate" + loop))
                                {
                                    val = adv.UploadFile(context, name + "_rowrate" + loop);
                                }
                                else val = ReqFormOrJsonVal(isInputJson, context.Request, mainJObject, name + "_rowrate" + loop, null);
                                vali = DataOPValidate(ref val, jiaoyan, caption);
                                if (vali != null) return vali;
                                if (Data.DataRow.Count < (loop + 1)) Data.DataRow.Add(new DPDataRow()); Data.DataRow[loop].ValueList.Add((OpItem, val, null));
                                loop++;
                            }
                        }
                    }
                }
            }
            DPDataList.Add(Data);
            ja.Clear();
            ja = null;
            return null;
        }
        public static string DataOPKeyValueJsonLoop(bool isInputJson, HttpContext context, JObject mainJObject, string JsonStr, DPData Data, JToken ParentJT, int LeiXing, List<string> FormKeys, string[] paras)
        {
            JArray ja = JArray.Parse(JsonStr);
            List<JToken> list = ja.ToList<JToken>();
            foreach (JToken jt in list)
            {
                if (jt.Type.ToString() == "Object")
                {
                    List<JToken> list1 = ((JObject)jt).ToList<JToken>();
                    string item = ((JProperty)list1[0]).Value.ToString();
                    string[] row = item.Split(new string[] { "##" }, StringSplitOptions.None);
                    string caption = row[0].Trim();
                    string name = row[1].Trim();
                    string binddata = str.AesDecrypt_Fit(row[2].Trim());
                    int leixing = int.Parse(str.AesDecrypt_Fit(row[3].Trim()));
                    string jiaoyan = row[4].Trim();
                    int special = int.Parse(row[5].Trim());
                    string advset = str.AesDecrypt_Fit(row[7].Trim());
                    bool IsJson = (leixing >= 7 && leixing <= 12);
                    for (int k = 1; k < paras.Length; k++)
                    {
                        binddata = binddata.Replace("@p" + k + "@", paras[k]).Replace("{p" + k + "}", paras[k]);
                        advset = advset.Replace("@p" + k + "@", paras[k]).Replace("{p" + k + "}", paras[k]);
                    }
                    binddata = DataOPRootValue(binddata, true, context.Request, mainJObject);
                    advset = DataOPRootValue(advset, true, context.Request, mainJObject);
                    Model.DPItem OpItem = new Model.DPItem()
                    {
                        Name = name,
                        Caption = caption,
                        LeiXing = leixing,
                        Validate = jiaoyan,
                        BindData = binddata,
                        Special = special,
                        Advance = advset,
                        IsJson = IsJson
                    };
                    if (LeiXing >= 10 && LeiXing <= 12)
                    {
                        int loop = 0;
                        foreach (JToken postJt in ParentJT)
                        {
                            var postJtObj = postJt[name] ?? postJt["_AutoKey_"];
                            if (postJtObj != null)
                            {
                                if (postJtObj.Type.ToString() != "Array")
                                {
                                    string val = postJtObj.Value<string>();
                                    string vali = DataOPValidate(ref val, jiaoyan, caption);
                                    if (vali != null) return vali;
                                    if (Data.DataRow.Count < (loop + 1)) Data.DataRow.Add(new DPDataRow()); Data.DataRow[loop].ValueList.Add((OpItem, val, null));
                                    loop++;
                                }
                                else
                                {
                                    DPData _Data = new DPData() { TableName = binddata, LeiXing = leixing, AdvSet = advset, DataRow = new List<Model.DPDataRow>() };
                                    if (Data.DataRow.Count < (loop + 1)) Data.DataRow.Add(new DPDataRow()); Data.DataRow[loop].ValueList.Add((OpItem, null, _Data));
                                    string result = DataOPKeyValueJsonLoop(isInputJson, context, mainJObject, ((JProperty)list1[1]).Value.ToString(), _Data, postJtObj, LeiXing, FormKeys, paras);
                                    if (result != null) return result;
                                    loop++;
                                }
                            }
                            else
                            {
                                string val = null;
                                string vali = DataOPValidate(ref val, jiaoyan, caption);
                                if (vali != null) return vali;
                                if (Data.DataRow.Count < (loop + 1)) Data.DataRow.Add(new DPDataRow()); Data.DataRow[loop].ValueList.Add((OpItem, val, null));
                                loop++;
                            }
                        }
                    }
                    else if (((JProperty)list1[1]).Value.Type.ToString() != "Array")
                    {
                        if (LeiXing >= 7 && LeiXing <= 9)
                        {
                            string val;
                            if (FormKeys.Contains(name))
                            {
                                val = adv.UploadFile(context, name);
                            }
                            else val = ReqFormOrJsonVal(isInputJson, context.Request, mainJObject, name, null);
                            string vali = DataOPValidate(ref val, jiaoyan, caption);
                            if (vali != null) return vali;
                            if (Data.DataRow.Count < 1) Data.DataRow.Add(new DPDataRow()); Data.DataRow[0].ValueList.Add((OpItem, val, null));
                            int loop = 1;
                            while (ReqFormOrJsonVal(isInputJson, context.Request, mainJObject, name + "_rowrate" + loop, null) != null)
                            {
                                if (FormKeys.Contains(name + "_rowrate" + loop))
                                {
                                    val = adv.UploadFile(context, name + "_rowrate" + loop);
                                }
                                else val = ReqFormOrJsonVal(isInputJson, context.Request, mainJObject, name + "_rowrate" + loop, null);
                                vali = DataOPValidate(ref val, jiaoyan, caption);
                                if (vali != null) return vali;
                                if (Data.DataRow.Count < (loop + 1)) Data.DataRow.Add(new DPDataRow()); Data.DataRow[loop].ValueList.Add((OpItem, val, null));
                                loop++;
                            }
                        }
                    }
                }
            }
            ja.Clear();
            ja = null;
            return null;
        }
        public static string DataOPValidate(ref string val, string jiaoyan, string caption)
        {
            if (jiaoyan.Equals("noempty"))
            {
                if (string.IsNullOrEmpty(val))
                {
                    return caption + " 不能为空";
                }
            }
            else if (jiaoyan.Equals("int"))
            {
                if (string.IsNullOrEmpty(val) || !int.TryParse(val, out int _i))
                {
                    return caption + " 必须为整数";
                }
            }
            else if (jiaoyan.Equals("decimal"))
            {
                if (string.IsNullOrEmpty(val) || !decimal.TryParse(val, out decimal _i))
                {
                    return caption + " 必须为数字";
                }
            }
            else if (jiaoyan.Equals("date"))
            {
                if (string.IsNullOrEmpty(val) || !DateTime.TryParse(val, out DateTime _i))
                {
                    return caption + " 必须为日期格式";
                }
            }
            else if (jiaoyan.Equals("int_empty"))
            {
                if (string.IsNullOrEmpty(val))
                {
                    val = "";
                }
                else if (!int.TryParse(val, out int _i))
                {
                    return caption + " 若不为空时，必须为整数";
                }
            }
            else if (jiaoyan.Equals("int_0"))
            {
                if (string.IsNullOrEmpty(val))
                {
                    val = "0";
                }
                else if (!int.TryParse(val, out int _i))
                {
                    return caption + " 若不为空时，必须为整数";
                }
            }
            else if (jiaoyan.Equals("int_1"))
            {
                if (string.IsNullOrEmpty(val))
                {
                    val = "1";
                }
                else if (!int.TryParse(val, out int _i))
                {
                    return caption + " 若不为空时，必须为整数";
                }
            }
            else if (jiaoyan.Equals("decimal_empty"))
            {
                if (string.IsNullOrEmpty(val))
                {
                    val = "";
                }
                if (!decimal.TryParse(val, out decimal _i))
                {
                    return caption + " 若不为空时，必须为数字";
                }
            }
            else if (jiaoyan.Equals("decimal_0"))
            {
                if (string.IsNullOrEmpty(val))
                {
                    val = "0";
                }
                else if (!decimal.TryParse(val, out decimal _i))
                {
                    return caption + " 若不为空时，必须为数字";
                }
            }
            else if (jiaoyan.Equals("decimal_1"))
            {
                if (string.IsNullOrEmpty(val))
                {
                    val = "1";
                }
                else if (!decimal.TryParse(val, out decimal _i))
                {
                    return caption + " 若不为空时，必须为数字";
                }
            }
            else if (jiaoyan.Equals("null_nocheck"))
            {
                if (string.IsNullOrEmpty(val))
                {
                    val = null;
                }
            }
            else if (jiaoyan.Equals("null_int"))
            {
                if (string.IsNullOrEmpty(val))
                {
                    val = null;
                }
                else if (!int.TryParse(val, out int _i))
                {
                    return caption + " 若不为空时，必须为整数";
                }
            }
            else if (jiaoyan.Equals("null_decimal"))
            {
                if (string.IsNullOrEmpty(val))
                {
                    val = null;
                }
                else if (!decimal.TryParse(val, out decimal _i))
                {
                    return caption + " 若不为空时，必须为数字";
                }
            }
            else if (jiaoyan.Equals("null_date"))
            {
                if (string.IsNullOrEmpty(val))
                {
                    val = null;
                }
                else if (!DateTime.TryParse(val, out DateTime _i))
                {
                    return caption + " 若不为空时，必须为日期格式";
                }
            }
            return null;
        }
        public static string DataOPExec(HttpContext context, string apiPath, string[] paras)
        {
            //  /test/new_page_2?add1/123
            string ApiPath = apiPath;
            int index = ApiPath.IndexOf("?");
            if (index > 0)
            {
                string[] newparas = ApiPath.Substring(index + 1).Split('/');
                string key = newparas[0];
                for (int i2 = 1; i2 < newparas.Length; i2++)
                {
                    for (int i3 = 1; i3 < paras.Length; i3++)
                    {
                        newparas[i2] = newparas[i2].Replace("@p" + i3 + "@", paras[i3]).Replace("{p" + i3 + "}", paras[i3]);
                    }
                }
                ApiPath = ApiPath.Substring(0, index) + "?" + key;
                string apisql = "select * from ft_ftdp_apiset where ApiPath='" + str.D2DD(ApiPath) + "' and ApiType='DataOP'";
                using (DB dbFTDP = new DB(SysConst.ConnectionStr_FTDP))
                {
                    using (DR dr = dbFTDP.OpenRecord(apisql))
                    {
                        if (dr.Read())
                        {
                            var setDic = func.StrToDic(dr.GetString("Set_DataOP_Dic"));
                            var ret = Api.Json("DataOP", context, setDic, newparas, true);
                            setDic.Clear(); setDic = null;
                            return ret;
                        }
                    }
                }
            }
            return "";
        }
        private static string DyValue(HttpContext context, Dictionary<string, string> setDic, string[] paras, bool IsSon, Dictionary<string, int> OpedApiPathList, int level, Dictionary<string, object> reqDic, (bool ShowBaseLog, bool ShowInputLog, bool ShowOutputLog) IsOutputLog)
        {
            try
            {
                HttpRequest req = context.Request;
                string partid = setDic["partid"];
                if (partid.Length > 36) partid = partid.Substring(0, 36);
                string DefaultFID = setDic["DefaultFID"];
                if (string.IsNullOrWhiteSpace(DefaultFID)) DefaultFID = "@p1@";
                paras = ParasRewrite(context,reqDic, DefaultFID,paras);
                if (DefaultFID.StartsWith("@ForcePara#")) DefaultFID = "@p1@";
                string SiteID = setDic["SiteID"];
                string DefineStr = setDic["DefineStr"];
                string ApiDefine = setDic["ApiDefine"];
                string FidCol = setDic["FidCol"];
                //if (string.IsNullOrWhiteSpace(FidCol)) FidCol = "fid";
                int OpDefaultCol = int.Parse(setDic["OpDefaultCol"]);
                string ExecBefore = setDic.GetValueOrDefault("ExecBefore") ?? "";
                string ExecAfter = setDic.GetValueOrDefault("ExecAfter") ?? "";
                var DBSet = adv.CustomDBSet(setDic.GetValueOrDefault("CustomConnection"), paras, reqDic, context);
                string[] apiItem = ApiDefine.Split(new string[] { "[##]" }, StringSplitOptions.None);
                string[] apiKeys = apiItem[2].Split(new string[] { "[#]" }, StringSplitOptions.None);
                string[] colKeys = apiItem[3].Split(new string[] { "[#]" }, StringSplitOptions.None);
                string[] linkKeys = apiItem[5].Split(new string[] { "[#]" }, StringSplitOptions.None);
                for (int k = 1; k < paras.Length; k++)
                {
                    ExecBefore = ExecBefore.Replace("@p" + k + "@", paras[k]).Replace("{p" + k + "}", paras[k]);
                    ExecAfter = ExecAfter.Replace("@p" + k + "@", paras[k]).Replace("{p" + k + "}", paras[k]);
                }
                List<string[]> list = new List<string[]>();
                string[] rows = DefineStr.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < rows.Length; i++)
                {
                    string[] item = rows[i].Split(new string[] { "##" }, StringSplitOptions.None);
                    string id = item[1].Trim();
                    int matchIndex = -1;
                    for (int k = 0; k < colKeys.Length; k++)
                    {
                        if (colKeys[k].Equals(id))
                        {
                            matchIndex = k;
                            break;
                        }
                    }
                    if (matchIndex < 0) continue;
                    string tabletag = str.GetDecode(item[2].Trim());
                    string fid = item[3].Trim();
                    if (fid == "") fid = DefaultFID;
                    int isdy = int.Parse(item[5]);
                    int isdim = int.Parse(item[6]);
                    //动态新增行、维表都按多行处理
                    //if (isdy == 1 || isdim == 1) continue;//动态新增行、维表不处理
                    string sql = str.GetDecode(item[7].Trim());
                    for (int k = 1; k < paras.Length; k++)//sql/高级列对参数进行替换
                    {
                        fid = fid.Replace("@p" + k + "@", paras[k].Replace("'", "")).Replace("{p" + k + "}", paras[k].Replace("'", ""));
                        sql = sql.Replace("@p" + k + "@", paras[k]).Replace("{p" + k + "}", paras[k]);
                        tabletag = tabletag.Replace("@p" + k + "@", paras[k]).Replace("{p" + k + "}", paras[k]);
                    }
                    fid=adv.CodePattern(context, fid, reqDic);
                    fid = adv.ParaPattern(context, fid, reqDic);
                    list.Add(new string[]{
                    id,tabletag,fid,sql,apiKeys[matchIndex],linkKeys[matchIndex],isdy.ToString(),isdim.ToString()
                });
                }

                bool fromDic = false;
                Dictionary<string, object> dic = null;
                //FromDic
                if (list[0][3].StartsWith("@FromDic:"))
                {
                    fromDic = true;
                    dic = Interface.Code.GetObject(list[0][3].Substring("@FromDic:".Length), reqDic, context) as Dictionary<string, object>;
                }

                StringBuilder json = new StringBuilder();
                if (IsSon) json.Append("{\"detail\":");
                else json.Append(FTFrame.Project.Core.Api.ResultJsonHead() + "{\"detail\":");
                Dictionary<string, string> keyValue = new Dictionary<string, string>();
                Dictionary<string, string> keyValueDirect = new Dictionary<string, string>();
                Dictionary<string, string> IDKeyValue = new Dictionary<string, string>();
                var directSelect = new Dictionary<string, List<(string apiKey, string column, bool isCodePara, string advSet)>>();
                bool BeSimple = false;
                var dbSetReadOnly = adv.CustomDBSet(setDic.GetValueOrDefault("CustomConnection"), paras, reqDic, context,true);
                using (DB db = new DB(dbSetReadOnly.DataBaseType, dbSetReadOnly.ConnString))
                {
                    db.Open();
                    //获取前执行
                    adv.ExecOperation(context, null, null, ExecBefore, reqDic);
                    //先处理Direct
                    foreach (string[] item in list)
                    {
                        string id = item[0].Trim();
                        string tabletag = item[1].Trim().Replace("'", "''");
                        string fid = item[2].Trim();
                        string _sql = item[3].Trim();
                        string apikey = item[4].Trim();
                        string linkkey = item[5].Trim();
                        bool isdy = item[6] == "1";
                        bool isdim = item[7] == "1";
                        if (!IDKeyValue.ContainsKey(id)) IDKeyValue.Add(id, apikey);
                        if (keyValue.ContainsKey(apikey) || keyValue.ContainsKey("_tagjson_" + apikey)) continue;
                        if (linkkey.StartsWith("@api_"))
                        {
                            
                        }
                        else if (isdy || isdim)
                        {
                            
                        }
                        else
                        {
                            //if (string.IsNullOrWhiteSpace(_sql) || _sql.StartsWith("str@code(") || _sql.StartsWith("sql:") || _sql.StartsWith("sql@code(") || _sql.StartsWith("@From"))
                            if (!string.IsNullOrWhiteSpace(tabletag))
                            {
                                bool IsCodePara = false;
                                if (!string.IsNullOrWhiteSpace(_sql))
                                {
                                    IsCodePara = true;
                                }
                                if (fromDic)
                                {
                                    keyValue.Add(apikey, dic.GetValueOrDefault(tabletag)?.ToString());
                                }
                                else
                                {
                                    if (!tabletag.Equals("") && !fid.Equals(""))
                                    {
                                        string table = tabletag.Split('.')[0];
                                        table = table.StartsWith("@") ? table.Substring(1) : ("ft_" + SiteID + "_f_" + table);
                                        string col = tabletag.Split('.')[1];
                                        string CurFidCol = DBSuit.Key(table, FidCol).KeyName;
                                        fid = DyValueFromValue(fid, IDKeyValue, keyValue, true);
                                        string CusWhere = null;
                                        if (fid.StartsWith("where ", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            CusWhere = fid;
                                        }
                                        else if (fid.IndexOf(',') >= 0)
                                        {
                                            CurFidCol = fid.Split(',')[1];
                                            fid = fid.Split(',')[0];
                                        }
                                        string tableWhere = (CusWhere == null) ? (table + " where " + str.D2DD(CurFidCol) + "='" + str.D2DD(fid) + "'") : (table + " " + CusWhere);
                                        if (!directSelect.ContainsKey(tableWhere))
                                        {
                                            directSelect.Add(tableWhere, new List<(string apiKey, string column, bool isCodePara, string advSet)>());
                                        }
                                        directSelect[tableWhere].Add((apikey, col, IsCodePara, _sql));
                                    }
                                }
                            }
                        }
                    }
                    //处理直接查
                    foreach (string tableWhere in directSelect.Keys)
                    {
                        var directCols = directSelect[tableWhere];
                        string cols = "";
                        foreach (var directCol in directCols)
                        {
                            cols += "," + directCol.column;
                        }
                        if (!cols.Equals("")) cols = cols.Substring(1);
                        string sql = "select " + cols + " from " + tableWhere;
                        if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(sql, "[Simple Select]", context, reqDic);
                        using (DR rdr = db.OpenRecord(sql))
                        {
                            if (rdr.Read())
                            {
                                for (int i = 0; i < directCols.Count; i++)
                                {
                                    if (keyValueDirect.ContainsKey(directCols[i].apiKey)) continue;
                                    var cval = rdr.CommonValue(null, tableWhere, i);
                                    if (directCols[i].isCodePara)
                                    {
                                        if (directCols[i].advSet.StartsWith("@code("))
                                        {
                                            string val = Interface.Code.Get(directCols[i].advSet, reqDic, new Dictionary<string, string>() {
                                                { "@val@", cval.val} }, context);

                                            keyValueDirect.Add(directCols[i].apiKey, val);
                                            keyValue.Add(directCols[i].apiKey, val);
                                        }
                                        else if (directCols[i].advSet.StartsWith("str@code("))
                                        {
                                            string val = Interface.Code.Get(directCols[i].advSet.Substring(3), reqDic, new Dictionary<string, string>() {
                                                { "@val@", cval.val} }, context);

                                            keyValueDirect.Add(directCols[i].apiKey, val);
                                            keyValue.Add(directCols[i].apiKey, val);
                                        }
                                        else if (!directCols[i].advSet.StartsWith("@From"))
                                        {
                                            var _sql = DyValueFromValue(directCols[i].advSet, IDKeyValue, keyValue, true);
                                            if (_sql.StartsWith("sql@code("))
                                            {
                                                _sql = Interface.Code.Get(_sql.Substring(3), reqDic, new Dictionary<string, string>() {
                                                { "@val@", cval.val} }, context);
                                            }
                                            _sql = _sql.Replace("@val@", cval.val);
                                            _sql = adv.CodePattern(context, _sql, reqDic);
                                            _sql = adv.ParaPattern(context, _sql, reqDic);
                                            if (_sql.StartsWith("@sql:")) _sql = _sql.Substring(5);
                                            using (DB db2 = new DB(dbSetReadOnly.DataBaseType, dbSetReadOnly.ConnString))
                                            {
                                                using (DR itemrdr = db2.OpenRecord(_sql))
                                                {
                                                    if (itemrdr.Read())
                                                    {
                                                        var itemcval = itemrdr.CommonValue(null, null, 0);
                                                        if (itemcval.quotation)
                                                        {
                                                            keyValueDirect.Add(directCols[i].apiKey, itemcval.val);
                                                            keyValue.Add(directCols[i].apiKey, itemcval.val);
                                                        }
                                                        else
                                                        {
                                                            keyValueDirect.Add("_tagnoqua_" + directCols[i].apiKey, itemcval.val);
                                                            keyValue.Add("_tagnoqua_" + directCols[i].apiKey, itemcval.val);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        keyValueDirect.Add(directCols[i].apiKey, SysConst.NoResult);
                                                        keyValue.Add(directCols[i].apiKey, SysConst.NoResult);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cval.quotation)
                                        {
                                            keyValueDirect.Add(directCols[i].apiKey, cval.val);
                                            keyValue.Add(directCols[i].apiKey, cval.val);
                                        }
                                        else
                                        {
                                            keyValueDirect.Add("_tagnoqua_" + directCols[i].apiKey, cval.val);
                                            keyValue.Add("_tagnoqua_" + directCols[i].apiKey, cval.val);
                                        }
                                        
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < directCols.Count; i++)
                                {
                                    if (keyValueDirect.ContainsKey(directCols[i].apiKey)) continue;
                                    keyValueDirect.Add(directCols[i].apiKey, SysConst.NoResult);
                                    keyValue.Add(directCols[i].apiKey, SysConst.NoResult);
                                }
                            }
                        }
                    }
                    //处理其他情况
                    //简易方式输出为，仅配置一项，且该项id为_开头，且该项的返回SQL为一行记录
                    bool MayBeSimple = list.Count == 1 && list[0][0].Trim().StartsWith("_");
                    foreach (string[] item in list)
                    {
                        string id = item[0].Trim();
                        string tabletag = item[1].Trim().Replace("'", "''");
                        string fid = item[2].Trim();
                        string _sql = item[3].Trim();
                        string apikey = item[4].Trim();
                        string linkkey = item[5].Trim();
                        bool isdy = item[6] == "1";
                        bool isdim = item[7] == "1";
                        if (!IDKeyValue.ContainsKey(id)) IDKeyValue.Add(id, apikey);
                        if (keyValue.ContainsKey(apikey) || keyValue.ContainsKey("_tagjson_" + apikey)) continue;
                        _sql = DyValueFromValue(_sql, IDKeyValue, keyValue, true);
                        if (linkkey.StartsWith("@api_"))
                        {
                            string ApiPath = linkkey.Substring(5);
                            int index = ApiPath.IndexOf("?");
                            if (index > 0)
                            {
                                string[] newparas = ApiPath.Substring(index + 1).Split('/');
                                string key = newparas[0];
                                for (int i2 = 1; i2 < newparas.Length; i2++)
                                {
                                    for (int i3 = 1; i3 < paras.Length; i3++)
                                    {
                                        newparas[i2] = newparas[i2].Replace("@p" + i3 + "@", paras[i3]).Replace("{p" + i3 + "}", paras[i3]);
                                    }
                                }
                                ApiPath = ApiPath.Substring(0, index) + "?" + key;
                                string apisql = "select * from ft_ftdp_apiset where ApiPath='" + str.D2DD(ApiPath) + "'";
                                using (DB dbFTDP = new DB(SysConst.ConnectionStr_FTDP))
                                {
                                    using (DR dr = dbFTDP.OpenRecord(apisql))
                                    {
                                        if (dr.Read())
                                        {
                                            string ApiType = dr.GetString("ApiType");
                                            if (ApiType == "List")
                                            {
                                                Dictionary<string, string> setDic2 = new Dictionary<string, string>();
                                                setDic2.Add("partid", dr.GetString("PartID"));
                                                setDic2.Add("Order", dr.GetString("Set_List_Order"));
                                                setDic2.Add("schdefine", "");
                                                setDic2.Add("SiteID", "ftdp");
                                                setDic2.Add("InputType", "json");
                                                setDic2.Add("sql", dr.GetString("Set_List_Sql"));//.Replace("\r\n", "\n").Replace("\n", "\\r\\n"));
                                                setDic2.Add("sqlCount", dr.GetString("Set_List_SqlCount"));//.Replace("\r\n", "\n").Replace("\n", "\\r\\n"));
                                                setDic2.Add("RowAll", dr.GetString("Set_List_RowAll"));
                                                setDic2.Add("Consts", "################################################");
                                                setDic2.Add("BlockDataDefine", "");
                                                setDic2.Add("UserCusCdn", dr.GetString("Set_List_CusCondition"));//.Replace("\r\n", "\n").Replace("\n", "\\r\\n"));
                                                setDic2.Add("CacuRowData", "");
                                                setDic2.Add("CustomConnection", dr.GetStringNoNULL("CustomConnection"));
                                                setDic2.Add("MainTable", "");
                                                setDic2.Add("NumsPerPage", "0");
                                                setDic2.Add("KeyName", "fid");
                                                setDic2.Add("Col1ReqName", "");
                                                setDic2.Add("Col2ReqName", "");
                                                setDic2.Add("Col3ReqName", "");
                                                setDic2.Add("Col1Name", "");
                                                setDic2.Add("Col2Name", "");
                                                setDic2.Add("Col3Name", "");
                                                keyValue.Add("_tagjson_" + apikey, Api.Json("List", context, setDic2, newparas, true, OpedApiPathList, level + 1, ApiPath));
                                                setDic2.Clear(); setDic2 = null;
                                            }
                                            else if (ApiType == "DyValue")
                                            {
                                                Dictionary<string, string> setDic2 = new Dictionary<string, string>();
                                                setDic2.Add("partid", dr.GetString("PartID"));
                                                setDic2.Add("DefaultFID", dr.GetString("Set_DyValue_DefaultFID"));
                                                setDic2.Add("SiteID", "ftdp");
                                                setDic2.Add("DefineStr", dr.GetString("Set_DyValue_DefineStr"));
                                                setDic2.Add("ApiDefine", dr.GetString("Set_DyValue_ApiDefine"));
                                                setDic2.Add("FidCol", dr.GetString("Set_DyValue_FidCol"));
                                                setDic2.Add("OpDefaultCol", dr.GetInt16("Set_DyValue_OpDefaultCol").ToString());
                                                setDic2.Add("CustomConnection", dr.GetStringNoNULL("CustomConnection"));
                                                keyValue.Add("_tagjson_" + apikey, Api.Json("DyValue", context, setDic2, newparas, true, OpedApiPathList, level + 1, ApiPath));
                                                setDic2.Clear(); setDic2 = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (_sql.StartsWith("@api_"))
                        {
                            string ApiPath = _sql.Substring(5);
                            int index = ApiPath.IndexOf("?");
                            if (index > 0)
                            {
                                string[] newparas = ApiPath.Substring(index + 1).Split('/');
                                string key = newparas[0];
                                for (int i2 = 1; i2 < newparas.Length; i2++)
                                {
                                    for (int i3 = 1; i3 < paras.Length; i3++)
                                    {
                                        newparas[i2] = newparas[i2].Replace("@p" + i3 + "@", paras[i3]).Replace("{p" + i3 + "}", paras[i3]);
                                    }
                                }
                                ApiPath = ApiPath.Substring(0, index) + "?" + key;
                                string apisql = "select * from ft_ftdp_apiset where ApiPath='" + str.D2DD(ApiPath) + "'";
                                using (DB dbFTDP = new DB(SysConst.ConnectionStr_FTDP))
                                {
                                    using (DR dr = dbFTDP.OpenRecord(apisql))
                                    {
                                        if (dr.Read())
                                        {
                                            string ApiType = dr.GetString("ApiType");
                                            if (ApiType == "List")
                                            {
                                                Dictionary<string, string> setDic2 = new Dictionary<string, string>();
                                                setDic2.Add("partid", dr.GetString("PartID"));
                                                setDic2.Add("Order", dr.GetString("Set_List_Order"));
                                                setDic2.Add("schdefine", "");
                                                setDic2.Add("SiteID", "ftdp");
                                                setDic2.Add("InputType", "json");
                                                setDic2.Add("sql", dr.GetString("Set_List_Sql"));//.Replace("\r\n", "\n").Replace("\n", "\\r\\n"));
                                                setDic2.Add("sqlCount", dr.GetString("Set_List_SqlCount"));//.Replace("\r\n", "\n").Replace("\n", "\\r\\n"));
                                                setDic2.Add("RowAll", dr.GetString("Set_List_RowAll"));
                                                setDic2.Add("Consts", "################################################");
                                                setDic2.Add("BlockDataDefine", "");
                                                setDic2.Add("UserCusCdn", dr.GetString("Set_List_CusCondition"));//.Replace("\r\n", "\n").Replace("\n", "\\r\\n"));
                                                setDic2.Add("CacuRowData", "");
                                                setDic2.Add("CustomConnection", dr.GetStringNoNULL("CustomConnection"));
                                                setDic2.Add("MainTable", "");
                                                setDic2.Add("NumsPerPage", "0");
                                                setDic2.Add("KeyName", "fid");
                                                setDic2.Add("Col1ReqName", "");
                                                setDic2.Add("Col2ReqName", "");
                                                setDic2.Add("Col3ReqName", "");
                                                setDic2.Add("Col1Name", "");
                                                setDic2.Add("Col2Name", "");
                                                setDic2.Add("Col3Name", "");
                                                keyValue.Add("_tagjson_" + apikey, Api.Json("List", context, setDic2, newparas, true, OpedApiPathList, level + 1, ApiPath));
                                                setDic2.Clear(); setDic2 = null;
                                            }
                                            else if (ApiType == "DyValue")
                                            {
                                                Dictionary<string, string> setDic2 = new Dictionary<string, string>();
                                                setDic2.Add("partid", dr.GetString("PartID"));
                                                setDic2.Add("DefaultFID", dr.GetString("Set_DyValue_DefaultFID"));
                                                setDic2.Add("SiteID", "ftdp");
                                                setDic2.Add("DefineStr", dr.GetString("Set_DyValue_DefineStr"));
                                                setDic2.Add("ApiDefine", dr.GetString("Set_DyValue_ApiDefine"));
                                                setDic2.Add("FidCol", dr.GetString("Set_DyValue_FidCol"));
                                                setDic2.Add("OpDefaultCol", dr.GetInt16("Set_DyValue_OpDefaultCol").ToString());
                                                setDic2.Add("CustomConnection", dr.GetStringNoNULL("CustomConnection"));
                                                keyValue.Add("_tagjson_" + apikey, Api.Json("DyValue", context, setDic2, newparas, true, OpedApiPathList, level + 1, ApiPath));
                                                setDic2.Clear(); setDic2 = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (isdy || isdim)
                        {
                            if (!string.IsNullOrWhiteSpace(_sql) && !_sql.StartsWith("@From"))
                            {
                                if (_sql.StartsWith("@code("))
                                {
                                    keyValue.Add("_tagjson_" + apikey, Interface.Code.Get(_sql, reqDic, context).NewLineReplace());
                                }
                                else
                                {
                                    if (_sql.StartsWith("sql@code("))
                                    {
                                        _sql = Interface.Code.Get(_sql.Substring(3), reqDic, context);
                                    }
                                    _sql = adv.CodePattern(context, _sql, reqDic);
                                    _sql = adv.ParaPattern(context, _sql, reqDic);
                                    _sql = Sql.GetSqlForRemoveSameCols(_sql);
                                    var IsForceOneRow = false;
                                    switch (db.CurDataBaseType)
                                    {
                                        case DBClient.DataBase.MySql:
                                            IsForceOneRow= _sql.Trim().ToLower().EndsWith("limit 1"); ;
                                            break;
                                        case DBClient.DataBase.SqlServer:
                                            IsForceOneRow = _sql.Trim().ToLower().StartsWith("select top 1"); ;
                                            break;
                                        case DBClient.DataBase.Sqlite:
                                            IsForceOneRow = _sql.Trim().ToLower().EndsWith("limit 1"); ;
                                            break;
                                        case DBClient.DataBase.Oracle:
                                            IsForceOneRow = _sql.Trim().ToLower().EndsWith("limit 1"); ;
                                            break;
                                        default:
                                            break;
                                    }
                                    
                                    StringBuilder sbMulti = new StringBuilder();
                                    //sbMulti.Append("[");
                                    int loopMulti = 0;
                                    using (DR rdr = db.OpenRecord(_sql))
                                    {
                                        while (rdr.Read())
                                        {
                                            if (loopMulti > 0) sbMulti.Append(",");
                                            sbMulti.Append("{");
                                            for (int fi = 0; fi < rdr.FieldCount; fi++)
                                            {
                                                if (fi > 0) sbMulti.Append(",");
                                                var cval = rdr.CommonValue(null, null, fi);
                                                if (cval.quotation) sbMulti.Append("\"" + rdr.GetName(fi) + "\":\"" + cval.val.NewLineReplace() + "\"");
                                                else sbMulti.Append("\"" + rdr.GetName(fi) + "\":" + cval.val + "");
                                            }
                                            sbMulti.Append("}");
                                            loopMulti++;
                                        }
                                    }
                                    //sbMulti.Append("]");
                                    if (IsForceOneRow)
                                    {
                                        if (loopMulti == 1)
                                        {
                                            keyValue.Add("_tagjson_" + apikey, sbMulti.ToString());
                                        }
                                        else
                                        {
                                            keyValue.Add("_tagnoqua_" + apikey, "null");
                                        }
                                    }
                                    else
                                    {
                                        if (MayBeSimple && loopMulti == 1)
                                        {
                                            BeSimple = true;
                                            keyValue.Add("_tagjson_" + apikey, sbMulti.ToString());
                                        }
                                        else
                                        {
                                            keyValue.Add("_tagjson_" + apikey, "[" + sbMulti.ToString() + "]");
                                        }
                                    }
                                    sbMulti.Clear();
                                    sbMulti = null;
                                }
                            }
                        }
                        else if (string.IsNullOrWhiteSpace(tabletag))
                        {
                            if (string.IsNullOrWhiteSpace(_sql) || _sql.StartsWith("str@code(") || _sql.StartsWith("@From"))
                            {
                                
                            }
                            else if (_sql.StartsWith("@code("))
                            {
                                keyValue.Add(apikey, Interface.Code.Get(_sql, reqDic, context));
                            }
                            else if (_sql.StartsWith("all@code("))
                            {
                                keyValue.Add(apikey, "all@code not support at api mode");
                            }
                            else if (!_sql.StartsWith("@From"))
                            {
                                _sql = DyValueFromValue(_sql, IDKeyValue, keyValue, true);
                                if (_sql.StartsWith("sql@code("))
                                {
                                    _sql = Interface.Code.Get(_sql.Substring(3), reqDic, context);
                                }
                                _sql = adv.CodePattern(context, _sql, reqDic);
                                _sql = adv.ParaPattern(context, _sql, reqDic);
                                if (_sql.StartsWith("@sql:")) _sql = _sql.Substring(5);
                                if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(_sql,"[DyValue Adv Sql]", context, reqDic);
                                DR rdr = db.OpenRecord(_sql);
                                if (rdr.Read())
                                {
                                    var cval = rdr.CommonValue(null, null, 0);
                                    if (cval.quotation) keyValue.Add(apikey, cval.val);//api 暂不支持 ，隔开的多id一次查询，暂时用视图解决，包括无fid的情况
                                    else keyValue.Add("_tagnoqua_" + apikey, cval.val);
                                }
                                else
                                {
                                    keyValue.Add(apikey, SysConst.NoResult);
                                }
                                rdr.Close();
                            }
                        }
                    }
                    
                    //获取后执行
                    adv.ExecOperation(context, null, null, ExecAfter, reqDic);
                }
                IDKeyValue.Clear();
                int loop = 0;
                bool hasValue = false;
                StringBuilder jsonSub = new StringBuilder(100);
                //foreach (string apikey in keyValueDirect.Keys)
                //{
                //    if (loop++ > 0) jsonSub.Append(",");
                //    if (apikey.StartsWith("_tagnoqua_"))
                //    {
                //        jsonSub.Append("\"" + apikey.Substring(10) + "\":" + keyValueDirect[apikey]);
                //        if (!hasValue) hasValue = true;
                //    }
                //    else
                //    {
                //        string keyVal = keyValueDirect[apikey].Replace("\"", "\\\"").NewLineReplace();
                //        jsonSub.Append("\"" + apikey + "\":\"" + (keyVal.Equals(SysConst.NoResult) ? SysConst.NoResultValue : keyVal) + "\"");
                //        if (!hasValue && keyVal != SysConst.NoResult) hasValue = true;
                //    }
                //}
                foreach (string apikey in keyValue.Keys)
                {
                    if (loop++ > 0) jsonSub.Append(",");
                    if (apikey.StartsWith("_tagjson_"))
                    {
                        if (BeSimple) jsonSub.Append(keyValue[apikey]);
                        else jsonSub.Append("\"" + apikey.Substring(9) + "\":" + keyValue[apikey]);
                        if (!hasValue) hasValue = true;
                    }
                    else if (apikey.StartsWith("_tagnoqua_"))
                    {
                        jsonSub.Append("\"" + apikey.Substring(10) + "\":" + keyValue[apikey]);
                        if (!hasValue) hasValue = true;
                    }
                    else if (keyValue[apikey].StartsWith("#NoQuata#"))
                    {
                        jsonSub.Append("\"" + apikey + "\":" + keyValue[apikey].Substring("#NoQuata#".Length));
                        if (!hasValue) hasValue = true;
                    }
                    else
                    {
                        string keyVal = keyValue[apikey].Replace("\"", "\\\"").NewLineReplace();
                        jsonSub.Append("\"" + apikey + "\":\"" + (keyVal.Equals(SysConst.NoResult) ? SysConst.NoResultValue : keyVal) + "\"");
                        if (!hasValue && keyValue[apikey] != SysConst.NoResult) hasValue = true;
                    }
                }
                directSelect.Clear();
                keyValue.Clear();
                keyValueDirect.Clear();
                if (hasValue)
                {
                    if (BeSimple) json.Append(jsonSub.ToString());
                    else json.Append("{" + jsonSub.ToString() + "}");
                }
                else
                {
                    json.Append("null");
                }
                if (IsSon) json.Append("}");
                else json.Append("}}");
                if (IsOutputLog.ShowOutputLog)
                {
                    //输出json格式化日志
                    using StringWriter textWriter = new StringWriter();
                    JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                    {
                        Formatting = Newtonsoft.Json.Formatting.Indented,
                        Indentation = 4,//缩进字符数
                        IndentChar = ' '//缩进字符
                    };
                    new JsonSerializer().Serialize(jsonWriter, JObject.Parse(json.ToString()));
                    FTFrame.Project.Core.Api.LogDebug(textWriter.ToString(), context, reqDic);
                }
                return json.ToString();
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError(ex, context, reqDic);
                return FTFrame.Project.Core.Api.ExceptionJson(ex);
            }
        }
        private static string DyValueFromValue(string val, Dictionary<string, string> IdApiKey, Dictionary<string, string> ApiKeyValue, bool NoDot = false)
        {
            Regex r = new Regex(@"@from\([\w\.]*\)");
            MatchCollection mc = r.Matches(val);
            foreach (Match m in mc)
            {
                string id = m.Value.Replace("@from(", "").Replace(")", "");
                if (IdApiKey.ContainsKey(id))
                {
                    if (ApiKeyValue.ContainsKey(IdApiKey[id]))
                    {
                        string idVal = ApiKeyValue[IdApiKey[id]];
                        if (idVal.Equals(SysConst.NoResult)) idVal = SysConst.NoResultValue;
                        val = val.Replace(m.Value, NoDot ? idVal.D0() : idVal);
                    }
                    else if (ApiKeyValue.ContainsKey("_tagnoqua_" + IdApiKey[id]))
                    {
                        string idVal = ApiKeyValue["_tagnoqua_" + IdApiKey[id]];
                        if (idVal.Equals(SysConst.NoResult)) idVal = SysConst.NoResultValue;
                        val = val.Replace(m.Value, NoDot ? idVal.D0() : idVal);
                    }
                }
            }
            r = new Regex(@"@key\([\w\.]*\)");
            mc = r.Matches(val);
            foreach (Match m in mc)
            {
                string id = m.Value.Replace("@key(", "").Replace(")", "");
                if (ApiKeyValue.ContainsKey(IdApiKey[id]))
                {
                    string idVal = ApiKeyValue[IdApiKey[id]];
                    if (idVal.Equals(SysConst.NoResult)) idVal = SysConst.NoResultValue;
                    val = val.Replace(m.Value, NoDot ? idVal.D0() : idVal);
                }
                else if (ApiKeyValue.ContainsKey("_tagnoqua_" + IdApiKey[id]))
                {
                    string idVal = ApiKeyValue["_tagnoqua_" + IdApiKey[id]];
                    if (idVal.Equals(SysConst.NoResult)) idVal = SysConst.NoResultValue;
                    val = val.Replace(m.Value, NoDot ? idVal.D0() : idVal);
                }
            }
            return val;
        }
        private static string DataOPRootValue(string text, bool isJson, HttpRequest req, JObject jObject, bool NoDot = false)
        {
            Regex r = new Regex(@"@keyValue\([\w\.]*\)");
            MatchCollection mc = r.Matches(text);
            foreach (Match m in mc)
            {
                string key = m.Value.Replace("@keyValue(", "").Replace(")", "");
                string val = ReqFormOrJsonVal(isJson, req, jObject, key)??"";
                text = text.Replace(m.Value, NoDot ? val.Replace("'", "") : val);
            }
            r = new Regex(@"@key\([\w\.]*\)");
            mc = r.Matches(text);
            foreach (Match m in mc)
            {
                string key = m.Value.Replace("@key(", "").Replace(")", "");
                string val = ReqFormOrJsonVal(isJson, req, jObject, key)??"";
                text = text.Replace(m.Value, NoDot ? val.Replace("'", "") : val);
            }
            return text;
        }
        private static string[] ParasRewrite(HttpContext context, Dictionary<string, object> reqDic, string setString, string[] paras)
        {
            if (setString.StartsWith("@ForcePara#"))
            {
                setString = setString.Substring("@ForcePara#".Length);
                string[] foreParas;
                if (setString.IndexOf(";")>=0)
                {
                    foreParas = setString.Split(new string[] { ";" }, StringSplitOptions.None);
                }
                else
                {
                    setString = adv.CodePattern(context, setString, reqDic);
                    setString = adv.ParaPattern(context, setString, reqDic);
                    if (string.IsNullOrEmpty(setString)) foreParas = new string[0];
                    else
                        foreParas = setString.Split(new string[] { ";" }, StringSplitOptions.None);
                }
                var foreParas2 = new string[foreParas.Length];
                var newparas = new string[foreParas.Length+1];
                newparas[0] = paras[0];
                for (var i = 0; i < foreParas.Length; i++)
                {
                    var forcePara = foreParas[i];
                    for (int k = 1; k < paras.Length; k++)
                    {
                        forcePara = forcePara.Replace("@p" + k + "@", paras[k].Replace("'", "")).Replace("{p" + k + "}", paras[k].Replace("'", ""));
                    }
                    for (int k = paras.Length; k < 12+1; k++)
                    {
                        forcePara = forcePara.Replace("@p" + k + "@", "").Replace("{p" + k + "}", "");
                    }
                    forcePara = adv.CodePattern(context, forcePara, reqDic);
                    forcePara = adv.ParaPattern(context, forcePara, reqDic);
                    foreParas2[i] = forcePara;
                }
                for (var i = 0; i < foreParas2.Length; i++)
                {
                    newparas[i+1]= foreParas2[i];
                }
                return newparas;
            }
            return paras;
        }
        private static string NormalKeyValue(string text, Dictionary<string, string> DataKeyValue)
        {
            Regex r = new Regex(@"@key\([\w\.]*\)");
            MatchCollection mc = r.Matches(text);
            foreach (Match m in mc)
            {
                string id = m.Value.Replace("@key(", "").Replace(")", "");
                if (DataKeyValue.ContainsKey(id))
                {
                    text = text.Replace(m.Value, DataKeyValue[id]);
                }
            }
            return text;
        }
        private static string List_Operation(DB db, string MainTable, string keyName, string keyValue, List<string[]> ColNameValue, HttpContext context, Dictionary<string, object> reqDic)
        {
            try
            {
                string subsql = " where 1=1 ";
                if (keyValue.IndexOf(',') < 0) subsql += " and " + str.D2DD(keyName) + "='" + str.D2DD(keyValue) + "'";
                else
                {
                    subsql += " and " + str.D2DD(keyName).Normal() + " in ('" + str.D2DD(keyValue).Replace(",", "','") + "')";
                }
                string sql = null;
                int count = 0;
                foreach (string[] item in ColNameValue)
                {
                    if (item[0] != "" && item[1] != null)
                    {
                        item[0] = adv.CodePattern(context, item[0], reqDic);
                        item[1] = adv.CodePattern(context, item[1], reqDic);
                        var cols = item[0].Split(",");
                        var vals = item[1].Split(",");
                        if (cols.Length == vals.Length)
                        {
                            sql = "update " + MainTable.Normal() + " set ";
                            for (var i = 0; i < cols.Length; i++)
                            {
                                if (i > 0) sql += ",";
                                sql += str.D2DD(cols[i]).Normal() + "='" + str.D2DD(vals[i]) + "' ";
                            }
                            sql += subsql;
                            //sql = "update " + MainTable.Normal() + " set " + str.D2DD(item[0]).Normal() + "='" + str.D2DD(item[1]) + "' " + subsql;
                            FTFrame.Project.Core.Api.LogDebug(sql, context, reqDic);
                            count += db.ExecSql(sql);
                        }
                    }
                }
                ColNameValue.Clear();
                ColNameValue = null;
                return FTFrame.Project.Core.Api.OperationBatchSuccessJson(count);
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError(ex, context, reqDic);
                return FTFrame.Project.Core.Api.ExceptionJson(ex);
            }
        }

        public static void PostManJson(HttpContext context)
        {
            string Fid = context.Request.Query["id"];
            if (string.IsNullOrEmpty(Fid)) return;
            string type = string.IsNullOrEmpty(context.Request.Query["type"]) ? "net" : context.Request.Query["type"];
            string Host = "";
            string Proto = "";
            if (type == "net")
            {
                Host = context.Request.Host.Host;
                if (context.Request.Host.Port != null && context.Request.Host.Port != 80 && context.Request.Host.Port != 443)
                {
                    Host += ":" + context.Request.Host.Port;
                }
                Proto = context.Request.GetDisplayUrl().Split(new string[] { "://" }, StringSplitOptions.None)[0];
            }
            else if (type == "java")
            {
                if (!string.IsNullOrEmpty(PublishUtil.JavaBaseUrl))
                {
                    Host = PublishUtil.JavaBaseUrl.Split(new string[] { "://" }, StringSplitOptions.None)[1];
                    Proto = PublishUtil.JavaBaseUrl.Split(new string[] { "://" }, StringSplitOptions.None)[0];
                }
            }
            using (DB db = new DB(SysConst.ConnectionStr_FTDP))
            {
                db.Open();
                string sql = "select PostManJson from ft_ftdp_apidoc where FId='" + str.D2DD(Fid) + "'";
                string json = db.GetStringForceNoNull(sql);
                json = json.Replace("[[[Proto]]]", Proto).Replace("[[[Host]]]", Host);
                context.Response.Clear();
                context.Response.ContentType = "application/Json";
                context.Response.WriteAsync(json);
            }
        }
        public static Dictionary<string, string> KeyValueSetDic(string setstr)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var items = setstr.Split(new string[] { "[;;]" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                var item2 = item.Split(new string[] { "[::]" }, StringSplitOptions.None);
                dic.Add(item2[0], item2[1]);
            }
            return dic;
        }
        private static string List(HttpContext context, Dictionary<string, string> setDic, string[] paras, bool IsSon, Dictionary<string, int> OpedApiPathList, int level, Dictionary<string, object> reqDic,(bool ShowBaseLog, bool ShowInputLog, bool ShowOutputLog) IsOutputLog)
        {
            //IsSon为true时，不传递req.Form任意参数，且输出为data下节点
            //根据paras个数，依次替换Order和Sql的 @p1@ {p1}
            HttpRequest req = context.Request;
            bool isJson = setDic["InputType"] == "json";
            JObject jObject = null;
            if (isJson)
            {
                try
                {
                    req.EnableBuffering();
                    using (var ms = new MemoryStream())
                    {
                        req.Body.Position = 0;
                        req.Body.CopyTo(ms);
                        var buffer = ms.ToArray();
                        string content = Encoding.UTF8.GetString(buffer);
                        if (string.IsNullOrWhiteSpace(content)) content = "{}";
                        jObject = JObject.Parse(content);
                        if (IsOutputLog.ShowInputLog)
                        {
                            //输出json格式化日志
                            using StringWriter textWriter = new StringWriter();
                            JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                            {
                                Formatting = Newtonsoft.Json.Formatting.Indented,
                                Indentation = 4,//缩进字符数
                                IndentChar = ' '//缩进字符
                            };
                            new JsonSerializer().Serialize(jsonWriter, jObject);
                            FTFrame.Project.Core.Api.LogDebug(textWriter.ToString(), context, reqDic);
                        }
                    }
                    //using (Stream stream = req.Body)
                    //{
                    //    byte[] buffer = new byte[req.ContentLength.Value];
                    //    stream.Read(buffer, 0, buffer.Length);
                    //    string content = Encoding.UTF8.GetString(buffer);
                    //    req.Body.Position = 0;
                    //    jObject = JObject.Parse(content);
                    //}
                }
                catch (Exception ex)
                {
                    return FTFrame.Project.Core.Api.ExceptionJson(ex);
                }
            }
            //是否为导出Excel
            //bool NeedExport = !isJson && ReqFormOrJsonVal(false, req, null, "isExport", "")=="1";
            bool NeedExport = ReqFormOrJsonVal(isJson, req, jObject, "isExport") == "1";
            //导出数据的最大行数限制 0 为不限制
            //int ExportMax = NeedExport?int.Parse(ReqFormOrJsonVal(false, req, null, "exportMax", "0")):0;
            int ExportMax = NeedExport ? int.Parse(ReqFormOrJsonVal(isJson, req, jObject, "exportMax", "0")) : 0;
            DataTable ExportDT = null;
            string DefaultOrder = setDic["Order"];//默认排序
            //aFezqkdpoDhM9udDF3FfWI6YSezasPDFkQp5zfkrDrY=
            string partid = setDic["partid"];//PartID   
            //20e8425d_4a2f_43ce_aebd_d616e064d59e_part
            if (partid.Length > 36) partid = partid.Substring(0, 36);
            string orderby = IsSon ? "" : ReqFormOrJsonVal(isJson, req, jObject, "orderBy");//排序字段 
            //""
            string ordertype = IsSon ? "" : ReqFormOrJsonVal(isJson, req, jObject, "orderType");//排序方式
            //""
            string schdefine = setDic["schdefine"];//模糊查询列定义
            //a.ClientName
            string schtext = IsSon ? "" : ReqFormOrJsonVal(isJson, req, jObject, "schText");//模糊查询内容 若有:则按单字段模糊查询 \: \;转义
            //""
            string schstrict = IsSon ? "" : ReqFormOrJsonVal(isJson, req, jObject, "schStrict");//严格查询定义 a:1;b:2;c:%3%;
            //""
            string schadv = IsSon ? "" : ReqFormOrJsonVal(isJson, req, jObject, "schAdv");//高级自定义查询Sql
            //""
            string keyName = setDic["KeyName"];//主键查询时的Name 
            string keyValue = IsSon ? "" : ReqFormOrJsonVal(isJson, req, jObject, "keyValue");//主键查询时的Value 逗号隔开为批量
            string col1ReqName = setDic["Col1ReqName"];
            string col2ReqName = setDic["Col2ReqName"];
            string col3ReqName = setDic["Col3ReqName"];
            string col1Name = setDic["Col1Name"];
            string col2Name = setDic["Col2Name"];
            string col3Name = setDic["Col3Name"];
            string col1Value = col1ReqName == "" ? null : ReqFormOrJsonVal(isJson, req, jObject, col1ReqName);
            string col2Value = col2ReqName == "" ? null : ReqFormOrJsonVal(isJson, req, jObject, col2ReqName);
            string col3Value = col3ReqName == "" ? null : ReqFormOrJsonVal(isJson, req, jObject, col3ReqName);

            var KvSetDic = KeyValueSetDic(setDic.GetValueOrDefault("KeyValueSet") ?? "");

            string ExecBefore = setDic.GetValueOrDefault("ExecBefore") ?? "";
            string ExecAfter = setDic.GetValueOrDefault("ExecAfter") ?? "";

            string _cuspagesize = IsSon ? "" : ReqFormOrJsonVal(isJson, req, jObject, "pageSize");
            int cuspagesize = (_cuspagesize == "" ? 999 : int.Parse(_cuspagesize));//每页显示的记录条数
            //8

            //string RerationTreeEvals = setDic["RerationTreeEvals"];//树形设置
            //""
            //string SqlEvals = setDic["SqlEvals"];//Sql的js
            //""
            //bool IsTree = bool.Parse(setDic["IsTree"]);//是否显示为树
            //False
            //string RerationTree = str.GetDecode(setDic["RerationTree"]);//树形设置
            //m1U8o6GpviGj96AwAELxMw==
            string SiteID = setDic["SiteID"];
            //house
            string sql = (setDic["sql"]);
            string sqlCount = (setDic["sqlCount"]);
            //rhRugfNdnv0YmZSGaNl207LsAcJURdFpSaIuGW31b+s=
            string RowAll = setDic["RowAll"].Replace("@ftquoat@", "\"");
            //"modfmem#WO3F8teLh6FbdtnI4AwYQg==#auto;left###_self##0&&&|||updatetime#pH5hf7p6jQ2XxK0q6atcdQ==#auto;left###_self##0&&&|||stat#scmvmxxJkJopsU0oHwGzWQ==#auto;left###_self##0&&&|||flow#w2THpCk21d0j8G5vTGJUTQ==#auto;left###_self##0&&&|||ClientName#r/1NiSk7oOZjgQVk111BdA==#auto;left###_self##0&&&"
            string[] Consts = setDic["Consts"].Replace("@ftquoat@", "\"").Split(new string[] { "##" }, StringSplitOptions.None);
            //"################################################"
            string BlockDataDefine = setDic["BlockDataDefine"].Replace("@ftquoat@", "\"");
            //""
            //BlockDataDefine = Interface.Code.Get(BlockDataDefine,context);
            bool IsBlockData = !BlockDataDefine.Trim().Equals("");

            string UserCusCdn = setDic["UserCusCdn"];//.Replace("@ftquoat@", "\"");
            //""
            //string UserCusSql = req.Form["UserCusSql"].Replace("@ftquoat@", "\"");
            string CacuRowData = setDic["CacuRowData"].Replace("@ftquoat@", "\"");
            //""
            bool IsCacuRow = !CacuRowData.Trim().Equals("");

            paras = ParasRewrite(context, reqDic, CacuRowData, paras);
            if (CacuRowData.StartsWith("@ForcePara#")) { CacuRowData = ""; IsCacuRow = false; }

            var DBSet = adv.CustomDBSet(setDic["CustomConnection"], paras, reqDic, context);
            //""
            string MainTable = setDic["MainTable"].Replace("'", "''");
            if (MainTable.StartsWith("@")) MainTable = MainTable.Substring(1);
            //VmRc59YVsIryr4DNCQrG3A==
            string rateNumType = IsSon ? "" : ReqFormOrJsonVal(isJson, req, jObject, "numType");
            int RateNumType = (rateNumType == "" ? 0 : int.Parse(rateNumType));//0 无序号 1当页序号 2总序号
            //2
            string _CurPageNum = IsSon ? "" : ReqFormOrJsonVal(isJson, req, jObject, "pageNum");//第几页
            int CurPageNum = (_CurPageNum == "" ? 1 : int.Parse(_CurPageNum));
            //1
            int NumsPerPage = int.Parse(setDic["NumsPerPage"]);
            //12
            if (cuspagesize >= 0) NumsPerPage = cuspagesize;
            if (NumsPerPage == 0) NumsPerPage = 999;
            //string ColDefine_Cur = setDic["ColDefine_Cur"];

                //替换传递的参数在排序和sql
            for (int i = 1; i < paras.Length; i++)
            {
                DefaultOrder = DefaultOrder.Replace("@p" + i + "@", paras[i]).Replace("{p" + i + "}", paras[i]);
                sql = sql.Replace("@p" + i + "@", paras[i]).Replace("{p" + i + "}", paras[i]);
                sqlCount = sqlCount.Replace("@p" + i + "@", paras[i]).Replace("{p" + i + "}", paras[i]);
                UserCusCdn = UserCusCdn.Replace("@p" + i + "@", paras[i]).Replace("{p" + i + "}", paras[i]);
                ExecBefore = ExecBefore.Replace("@p" + i + "@", paras[i]).Replace("{p" + i + "}", paras[i]);
                ExecAfter = ExecAfter.Replace("@p" + i + "@", paras[i]).Replace("{p" + i + "}", paras[i]);
            }
            //支持@key
            sql = DataOPRootValue(sql, isJson, req, jObject, true);
            sqlCount = DataOPRootValue(sqlCount, isJson, req, jObject, true);
            UserCusCdn = DataOPRootValue(UserCusCdn, isJson, req, jObject, true);
            ExecBefore = DataOPRootValue(ExecBefore, isJson, req, jObject, true);
            ExecAfter = DataOPRootValue(ExecAfter, isJson, req, jObject, true);
            sql = adv.CodePattern(context, sql, reqDic);
            sqlCount = adv.CodePattern(context, sqlCount, reqDic);
            UserCusCdn = adv.CodePattern(context, UserCusCdn, reqDic);
            sql = adv.ParaPattern(context, sql, reqDic);
            sqlCount = adv.ParaPattern(context, sqlCount, reqDic);
            UserCusCdn = adv.ParaPattern(context, UserCusCdn, reqDic);

            string initSql = sql;
            //数据源为List List_Loop_DataFromList实现
            //if(sql.StartsWith("@FromList:"))
            //{
            //    return ListDataFromCode(context, sql.Substring("@FromList:".Length).Trim());
            //}
            //数据源为@api ，待完善
            //if (sql.StartsWith("@FromApi:"))
            //{

            //}

            //列表页数据操作
            if (keyName != "" && keyValue != "" && ((col1Name != "" && col1Value != null) || (col2Name != "" && col2Value != null) || (col3Name != "" && col3Value != null)))
            {
                if (!Project.Core.User.IsLogin(reqDic))
                {
                    return Project.Core.Api.ErrorJson("No Right");
                }
                if (string.IsNullOrWhiteSpace(MainTable))
                {
                    return Project.Core.Api.ErrorJson("Batch operations must set the main table");
                }
                if (keyName == "(auto)") keyName = DBSuit.Key(MainTable).KeyName;
                for (int i = 1; i < paras.Length; i++)
                {
                    col1Name = col1Name.Replace("@p" + i + "@", paras[i]);
                    col2Name = col2Name.Replace("@p" + i + "@", paras[i]);
                    col3Name = col3Name.Replace("@p" + i + "@", paras[i]);
                }
                using (DB dbOP = new DB(DBSet.DataBaseType, DBSet.ConnString))
                {
                    List<string[]> listCols = new List<string[]>() {
                        new string[]{ col1Name, col1Value },new string[]{ col2Name, col2Value },new string[]{ col3Name, col3Value }
                    };
                    return List_Operation(dbOP, MainTable, keyName, keyValue, listCols, context, reqDic);
                }
            }

            //""
            //string CusTurnBtm = setDic["CusTurnBtm"];
            //CusTurnBtm = "@code(str.TurnPage,$1|$2|$3|$4|$5)";
            //string CusTurnTop = setDic["CusTurnTop"];
            sql = Interface.Code.Get(sql, reqDic, context) + " " + ProjectFilter.ListSqlAppend(context,paras,reqDic);
            UserCusCdn = Interface.Code.Get(UserCusCdn, reqDic, context);
            if (!string.IsNullOrWhiteSpace(UserCusCdn)) sql += " " + UserCusCdn;
            int? CodeSqlCount = null;
            if (!string.IsNullOrWhiteSpace(sqlCount) && !sqlCount.StartsWith("sql@code") && !sqlCount.StartsWith("@code"))
            {
                if (!string.IsNullOrWhiteSpace(UserCusCdn)) sqlCount += " " + UserCusCdn;
            }
            else if (sqlCount.StartsWith("sql@code"))
            {
                sqlCount = Interface.Code.Get(sqlCount.Substring(3), reqDic, context);
            }
            else if (sqlCount.StartsWith("@code"))
            {
                CodeSqlCount = int.Parse(Interface.Code.Get(sqlCount, reqDic, context));
            }
            bool ISCusSQL = false;// sql.StartsWith("@CUS:");
            string CusSqlList = null;
            string CusSqlCount = null;
            if (ISCusSQL)//@CUS:@SEARCH@####@SEARCH@ @LIMIT@ @ORDER@ @BETWEEN@
            {
                string[] CusSqlItem = sql.Substring(5).Split(new string[] { "####" }, StringSplitOptions.None);
                CusSqlList = CusSqlItem[0];
                CusSqlCount = CusSqlItem[1];
            }
            string seach_sql = "";
            if (!schtext.Equals(""))
            {
                schtext = schtext.Replace(@"\;", "[###]").Replace(@"\:", "[$$$]");
                if (schtext.IndexOf(":") < 0)//多字段模糊查询
                {
                    schtext = schtext.Replace("[###]", @";").Replace("[$$$]", ":");
                    if (!schdefine.Equals("") && !schtext.Equals(""))
                    {
                        string[] cols = schdefine.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < cols.Length; i++)
                        {
                            if (i > 0) seach_sql += " or ";
                            string[] schtextitem = schtext.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            seach_sql += "(";
                            for (int j = 0; j < schtextitem.Length; j++)
                            {
                                if (j > 0) seach_sql += " or ";
                                seach_sql += str.D2DD(cols[i]) + " like '%" + str.D2DD(schtextitem[j]) + "%'";
                            }
                            seach_sql += ")";
                        }
                        if (!seach_sql.Equals(""))
                        {
                            seach_sql = " and(" + seach_sql + ")";
                        }
                    }
                }
                else//单字段模糊查询 采用Or
                {
                    var list = new List<(string col, string val)>();
                    string[] itemMohu = schtext.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item1 in itemMohu)
                    {
                        string[] item = item1.Split(new string[] { ":" }, StringSplitOptions.None);
                        if (item.Length >= 2)
                        {
                            list.Add((item[0].Replace("[###]", @";").Replace("[$$$]", ":").D2(), item[1].Replace("[###]", @";").Replace("[$$$]", ":").D2()));
                            //seach_sql += " and " + item[0].Replace("[###]", @";").Replace("[$$$]", ":").D2() + " like '%" + item[1].Replace("[###]", @";").Replace("[$$$]", ":").D2() + "%' ";
                        }
                    }
                    if(list.Count>0)
                    {
                        seach_sql += " and (";
                        for(var i=0;i<list.Count;i++)
                        {
                            if(i>0) seach_sql += " or ";
                            seach_sql += " " + str.Nomal(list[i].col.Trim(),true) +" like '%"+ list[i].val.Trim() + "%' ";
                        }
                        seach_sql += " ) ";
                    }
                    list.Clear();
                }
            }

            if (keyName != "" && keyValue != "")
            {
                if (keyValue.IndexOf(',') < 0) seach_sql += " and " + str.D2DD(keyName) + "='" + str.D2DD(keyValue) + "'";
                else
                {
                    seach_sql += " and " + str.D2DD(keyName) + " in ('" + str.D2DD(keyValue).Replace(",", "','") + "')";
                }
            }
            if (!schstrict.Equals(""))
            {
                string[] items = schstrict.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in items)
                {
                    if (item.IndexOf(':') <= 0) continue;
                    string sval = item.Substring(item.IndexOf(':') + 1).Trim();
                    if (sval.StartsWith("@Cdn{"))
                    {
                        string sqlcuscdn = Project.Core.Utils.Str.Decode(sval.Substring(5, sval.Length - 6));
                        if (str.SQLSelectSafe(sqlcuscdn))
                        {
                            seach_sql += " and " + sqlcuscdn.Replace(";", "") + " ";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(sval) && sval!="%%")
                        {
                            if (sval.Equals("null"))
                            {
                                seach_sql += " and " + str.D2DD(item.Split(':')[0]).NormalAllowDot() + " is null";
                            }
                            else if (sval.Equals("!null"))
                            {
                                seach_sql += " and " + str.D2DD(item.Split(':')[0]).NormalAllowDot() + " is not null";
                            }
                            else if (sval.StartsWith("in"))
                            {
                                var inItems = sval.Substring(2).Replace("(", "").Replace(")", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                StringBuilder sbiT = new StringBuilder();
                                string iTcol = str.D2DD(item.Split(':')[0]).NormalAllowDot();
                                sbiT.Append(" and (");
                                for (int iTi = 0; iTi < inItems.Length; iTi++)
                                {
                                    if (iTi > 0) sbiT.Append(" or ");
                                    sbiT.Append(iTcol + "='" + inItems[iTi].D2() + "'");
                                }
                                sbiT.Append(" )");
                                seach_sql += sbiT.ToString();
                                sbiT.Clear();
                            }
                            else
                            {
                                string pat = "=";
                                if (sval.StartsWith("!"))
                                {
                                    pat = "!=";
                                    sval = sval.Substring(1);
                                }
                                else if (sval.StartsWith(">="))
                                {
                                    pat = ">=";
                                    sval = sval.Substring(2);
                                }
                                else if (sval.StartsWith("<="))
                                {
                                    pat = "<=";
                                    sval = sval.Substring(2);
                                }
                                else if (sval.StartsWith(">"))
                                {
                                    pat = ">";
                                    sval = sval.Substring(1);
                                }
                                else if (sval.StartsWith("<"))
                                {
                                    pat = "<";
                                    sval = sval.Substring(1);
                                }
                                if (sval.Equals("empty")) sval = "";
                                if (sval.IndexOf('%') >= 0)
                                {
                                    pat = " like ";
                                }
                                //seach_sql += " and " + str.D2DD(item.Split(':')[0]).Normal() + pat + (sval.StartsWith("(") ? sval.Replace("(", "").Replace(")", "") : ("'" + str.D2DD(sval) + "'"));
                                seach_sql += " and " + str.D2DD(item.Split(':')[0]).Trim().NormalAllowDot() + pat + (("'" + str.D2DD(sval) + "'"));
                            }
                        }
                    }
                }
            }
            if (!schadv.Equals("") && str.SQLSelectSafe(schadv))
            {
                seach_sql += " and " + schadv;
            }
            //string UserCusCdnSql = "";
            //UserCusCdn = UserCusCdn.Trim().Replace(";", "");
            //if (UserCusCdn.ToUpper().StartsWith("AND") && str.SQLSelectSafe(UserCusCdn))
            //{
            //    UserCusCdnSql = UserCusCdn;
            //}
            //if (!UserCusCdnSql.Equals(""))
            //{
            //    seach_sql += " " + UserCusCdnSql;
            //}
            string sqlall = null;
            string SqlSelectAll = null;
            if (!ISCusSQL)
            {
                sql = sql + seach_sql;
                sql = sql.Replace(";", "");
                //sql = Sql.GetSqlForRemoveSameCols(sql);
                if (!string.IsNullOrWhiteSpace(sqlCount)) sqlall = sqlCount;
                else sqlall = "select count(*) as ca from (" + sql + ") tbcount1";
                SqlSelectAll = sql;
            }
            else
            {
                CusSqlList = CusSqlList.Replace("@SEARCH@", seach_sql).Replace(";", "");
                CusSqlCount = CusSqlCount.Replace("@SEARCH@", seach_sql).Replace(";", "");
                //CusSqlList = Sql.GetSqlForRemoveSameCols(CusSqlList);
                //CusSqlCount = Sql.GetSqlForRemoveSameCols(CusSqlCount);
                SqlSelectAll = CusSqlList;
            }
            if (SysConst.DataBaseType.Equals(DataBase.MySql))
            {
                if (!ISCusSQL)
                {
                    if (orderby.Equals(""))
                    {
                        sql += " " + DefaultOrder;
                    }
                    else
                    {
                        sql += "  order by " + orderby + " " + ordertype;
                    }
                    if (!NeedExport)
                    {
                        if (NumsPerPage > 0)
                        {
                            sql += " limit " + NumsPerPage * (CurPageNum - 1) + "," + NumsPerPage;
                        }
                    }
                    else
                    {
                        if (ExportMax > 0)
                        {
                            sql += " limit 0," + ExportMax;
                        }
                    }
                }
                else
                {
                    if (orderby.Equals(""))
                    {
                        CusSqlList = CusSqlList.Replace("@ORDER@", DefaultOrder);
                    }
                    else
                    {
                        CusSqlList = CusSqlList.Replace("@ORDER@", "order by " + orderby + " " + ordertype);
                    }
                    if (!NeedExport)
                    {
                        if (NumsPerPage > 0)
                        {
                            CusSqlList = CusSqlList.Replace("@LIMIT@", "limit " + NumsPerPage * (CurPageNum - 1) + "," + NumsPerPage);
                        }
                    }
                    else
                    {
                        if (ExportMax > 0)
                        {
                            CusSqlList = CusSqlList.Replace("@LIMIT@", "limit 0," + ExportMax);
                        }
                    }
                }
            }
            else if (SysConst.DataBaseType.Equals(DataBase.SqlServer))
            {
                string orderbystr = "";
                if (orderby.Equals(""))
                {
                    orderbystr = DefaultOrder;
                }
                else
                {
                    orderbystr = "order by " + orderby + " " + ordertype;
                }
                if (!ISCusSQL)
                {
                    if (!NeedExport)
                    {
                        if (NumsPerPage > 0)
                        {
                            sql = "select * from (select Row_Number() over (" + orderbystr + ") as ft_rownum,* from (" + sql + ") as a) as ftlistt where ft_rownum between " + (NumsPerPage * (CurPageNum - 1) + 1) + " and " + (NumsPerPage * CurPageNum) + "";
                        }
                        else
                        {
                            sql += " " + orderbystr;
                        }
                    }
                    else
                    {
                        if (ExportMax > 0)
                        {
                            sql = "select * from (select Row_Number() over (" + orderbystr + ") as ft_rownum,* from (" + sql + ") as a) as ftlistt where ft_rownum between 1 and " + ExportMax + "";
                        }
                        else
                        {
                            sql += " " + orderbystr;
                        }
                    }
                }
                else
                {
                    if (!NeedExport)
                    {
                        if (NumsPerPage > 0)
                        {
                            CusSqlList = CusSqlList.Replace("@ORDER@", orderbystr).Replace("@BETWEEN@", "between " + (NumsPerPage * (CurPageNum - 1) + 1) + " and " + (NumsPerPage * CurPageNum));
                        }
                        else
                        {
                            CusSqlList = CusSqlList.Replace("@ORDER@", orderbystr);
                        }
                    }
                    else
                    {
                        if (ExportMax > 0)
                        {
                            CusSqlList = CusSqlList.Replace("@ORDER@", orderbystr).Replace("@BETWEEN@", "between 1 and " + ExportMax);
                        }
                        else
                        {
                            CusSqlList = CusSqlList.Replace("@ORDER@", orderbystr).Replace("@BETWEEN@", ">-1");
                        }
                    }
                }
            }
            else if (SysConst.DataBaseType.Equals(DataBase.Sqlite))
            {
                if (!ISCusSQL)
                {
                    if (orderby.Equals(""))
                    {
                        sql += " " + DefaultOrder;
                    }
                    else
                    {
                        sql += "  order by " + orderby + " " + ordertype;
                    }
                    if (!NeedExport)
                    {
                        if (NumsPerPage > 0)
                        {
                            sql += " limit " + NumsPerPage + " offset " + (NumsPerPage * (CurPageNum - 1));
                        }
                    }
                    else
                    {
                        if (ExportMax > 0)
                        {
                            sql += " limit " + ExportMax + " offset 0";
                        }
                    }
                }
                else
                {
                    if (orderby.Equals(""))
                    {
                        CusSqlList = CusSqlList.Replace("@ORDER@", DefaultOrder);
                    }
                    else
                    {
                        CusSqlList = CusSqlList.Replace("@ORDER@", "order by " + orderby + " " + ordertype);
                    }
                    if (!NeedExport)
                    {
                        if (NumsPerPage > 0)
                        {
                            CusSqlList = CusSqlList.Replace("@LIMIT@", "limit " + NumsPerPage + " offset " + (NumsPerPage * (CurPageNum - 1)));
                        }
                    }
                    else
                    {
                        if (ExportMax > 0)
                        {
                            CusSqlList = CusSqlList.Replace("@LIMIT@", "limit " + ExportMax + " offset 0");
                        }
                    }
                }
            }
            if (!ISCusSQL)
            {
                if (IsOutputLog.ShowBaseLog)
                {
                    FTFrame.Project.Core.Api.LogDebug(sql, "[Api List SQL]", context, reqDic);
                    if (!string.IsNullOrWhiteSpace(sqlCount)) FTFrame.Project.Core.Api.LogDebug(sqlall, "[List SQL Count]", context, reqDic);
                }
            }
            else
            {
                if (IsOutputLog.ShowBaseLog)
                {
                    FTFrame.Project.Core.Api.LogDebug(CusSqlList, "[Api Custom Sql List]", context, reqDic);
                    FTFrame.Project.Core.Api.LogDebug(CusSqlCount, "[Api Custom Sql Count]", context, reqDic);
                }
            }
            if (IsCacuRow)
            {
                if (IsOutputLog.ShowBaseLog)
                {
                    FTFrame.Project.Core.Api.LogDebug(SqlSelectAll, "[Api Cacu List All]", context, reqDic);
                }
            }

            ArrayList ColumnData = new ArrayList();
            string[] rows = RowAll.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            ArrayList ColumnOpen = new ArrayList();
            List<string> ColumnKey = new List<string>();
            ArrayList a0 = new ArrayList();
            Hashtable a0hash = new Hashtable();
            ArrayList b0 = new ArrayList();
            Hashtable b0hash = new Hashtable();
            Dictionary<string, string> titleCaption = new Dictionary<string, string>();
            foreach (string rowstr in rows)
            {
                string row = rowstr.Substring(0, rowstr.IndexOf("&&&")).Trim();
                string[] rowcols = row.Split('#');
                string title = rowcols[0];
                string caption = rowcols[5];
                if (a0.Contains(title)) title += "_sametitle_" + str.GetCombID();
                a0.Add(title);
                titleCaption.Add(title, string.IsNullOrWhiteSpace(caption) ? title : caption);
                a0hash.Add(title, new object[] { ((rowcols.Length > 7 && rowcols[7].Equals("1")) ? 1 : 0), rowcols[2].Split(';')[0], rowcols[2].Split(';')[1], rowstr });
                if (rowcols[7] == "0")//该列显示
                {
                    b0.Add(title);
                    b0hash.Add(title, new object[] { "0" });
                }
            }
            List<string> TitleList = new List<string>();
            foreach (string title in b0)
            {
                if (a0.Contains(title))
                {
                    if (((object[])b0hash[title])[0].ToString().Equals("0"))
                    {
                        if (NeedExport) TitleList.Add(titleCaption.GetValueOrDefault(title) ?? "");
                        string rowstr = ((object[])a0hash[title])[3].ToString();
                        ColumnOpen.Add(comp.List_IsColumnOpen(context, rowstr.Substring(rowstr.IndexOf("&&&") + 3).Trim(), reqDic).ToString());
                        string row = rowstr.Substring(0, rowstr.IndexOf("&&&")).Trim();
                        string[] rowcols = row.Split('#');
                        var colData = str.GetDecode(rowcols[1]);
                        for (int i = 1; i < paras.Length; i++)
                        {
                            colData = colData.Replace("@p" + i + "@", paras[i]);
                        }
                        ColumnData.Add(colData);
                        ColumnKey.Add(title);
                    }
                }
            }
            foreach (string title in a0)
            {
                if (!b0.Contains(title))
                {
                    if (((object[])a0hash[title])[0].ToString().Equals("0"))
                    {
                        //if (NeedExport) TitleList.Add((title.IndexOf("_sametitle_") >= 0) ? title.Substring(0, title.IndexOf("_sametitle_")) : title);
                        if (NeedExport) TitleList.Add(titleCaption.GetValueOrDefault(title) ?? "");
                        string rowstr = ((object[])a0hash[title])[3].ToString();
                        string row = rowstr.Substring(0, rowstr.IndexOf("&&&")).Trim();
                        string[] rowcols = row.Split('#');
                        var colData = str.GetDecode(rowcols[1]);
                        for (int i = 1; i < paras.Length; i++)
                        {
                            colData = colData.Replace("@p" + i + "@", paras[i]);
                        }
                        ColumnData.Add(colData);
                    }
                }
            }

            DateTime OPStart = DateTime.Now;
            if (NeedExport) ExportDT = new DataTable();
            var dbSetReadOnly = adv.CustomDBSet(setDic["CustomConnection"], paras, reqDic, context, true);
            DB db = new DB(dbSetReadOnly.DataBaseType, dbSetReadOnly.ConnString);
            DB db2 = new DB(dbSetReadOnly.DataBaseType, dbSetReadOnly.ConnString);
            //DB db = null; DB db2 = null;
            //if (CustomConnection == null || CustomConnection.Trim() == "")
            //{
            //    db = new DB(SysConst.ConnString_ReadOnly);
            //    db.Open();
            //    db2 = new DB(SysConst.ConnString_ReadOnly);
            //    db2.Open();
            //}
            //else
            //{
            //    CustomConnection = Interface.Code.Get(CustomConnection, context);
            //    db = new DB(CustomConnection); ;
            //    db.Open();
            //    db2 = new DB(CustomConnection);
            //    db2.Open();
            //}
            int CountAll = 0;
            StringBuilder json = new StringBuilder(200);
            try
            {
                if (NeedExport)
                {
                    foreach (string col in TitleList)
                    {
                        var _col = col.Trim().Split(new char[] { '：', ':', ' ', '\t', '\r', '\n', ',', '，', '.', ';', '；', '(', '（' });
                        ExportDT.Columns.Add(_col.Length == 0 ? " " : _col[0]);
                    }
                }
                if (IsSon) json.Append("{\"list\":[");
                else json.Append(FTFrame.Project.Core.Api.ResultJsonHead() + "{\"list\":[");

                //获取前执行
                adv.ExecOperation(context, null, null, ExecBefore, reqDic);

                if (initSql.StartsWith("@FromList:"))
                {
                    List_Loop_DataFromList(json, out CountAll, db, db2, NeedExport, ExportDT, initSql, paras, ColumnKey, ColumnOpen, ColumnData, context, SiteID, Consts, MainTable, RateNumType, NumsPerPage * (CurPageNum - 1), IsBlockData, BlockDataDefine, OpedApiPathList, level, reqDic);
                }
                else
                {
                    List_Loop(json, db, db2, NeedExport, ExportDT, ISCusSQL ? CusSqlList : sql, paras, ColumnKey, ColumnOpen, ColumnData, context, SiteID, Consts, MainTable, RateNumType, NumsPerPage * (CurPageNum - 1), IsBlockData, BlockDataDefine, OpedApiPathList, level, reqDic , isJson, jObject, IsOutputLog);
                    CountAll = CodeSqlCount ?? db.GetInt(ISCusSQL ? CusSqlCount : sqlall);
                }
                string cacurow = "";
                if (IsCacuRow)
                {
                    if (CacuRowData.StartsWith("@code(")) cacurow = Interface.Code.Get(CacuRowData, reqDic, context);
                    else
                    {
                        cacurow = CacuRowData;
                        Regex r = new Regex(@"{[^}]*}");
                        MatchCollection mc = r.Matches(CacuRowData);
                        foreach (Match m in mc)
                        {
                            string pattern = m.Value.Replace("{", "").Replace("}", "");
                            object cacuval = "";
                            sql = "select " + pattern + " as cacuval from (" + SqlSelectAll + ") as cacttb";
                            try
                            {
                                cacuval = db.GetObject(sql);
                                if (cacuval == null) cacuval = "";
                            }
                            catch (Exception ex)
                            {
                                cacuval = ex.Message;
                                FTFrame.Project.Core.Api.LogError(ex, context, reqDic);
                            }
                            cacurow = cacurow.Replace(m.Value, cacuval.ToString());
                        }
                    }
                }
                int PageCount = NumsPerPage <= 0 ? 1 : (((CountAll - 1) / NumsPerPage) + 1);
                json.Append("],\"cacu\":\"" + cacurow.Replace("\"", "").Replace("\r\n", "") + "\"");
                json.Append(",\"page\":{\"count\":" + CountAll + ",\"pageSize\":" + NumsPerPage + ",\"pageNum\":" + CurPageNum + ",\"pageCount\":" + PageCount + "}}");
                if (!IsSon) json.Append("}");

                //获取后执行
                adv.ExecOperation(context, null, null, ExecAfter, reqDic);

                if (NeedExport)
                {
                    json.Clear();
                    string ExportFileName = ReqFormOrJsonVal(isJson, req, jObject, "exportName");
                    if (string.IsNullOrEmpty(ExportFileName))
                    {
                        ExportFileName = KvSetDic.GetValueOrDefault("ExportFileName") ?? "";
                    }
                    export.Excel(context, ExportDT, ExportFileName);
                }
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError(ex, context, reqDic);
                return FTFrame.Project.Core.Api.ExceptionJson(ex);
            }
            finally
            {
                db.Close();
                db2.Close();
            }
            if (IsOutputLog.ShowOutputLog)
            {
                //输出json格式化日志
                using StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    Indentation = 4,//缩进字符数
                    IndentChar = ' '//缩进字符
                };
                new JsonSerializer().Serialize(jsonWriter, JObject.Parse(json.ToString()));
                FTFrame.Project.Core.Api.LogDebug(textWriter.ToString(), context, reqDic);
            }
            return json.ToString();
        }
        private static void List_Loop(StringBuilder json, DB db, DB db2, bool NeedExport, DataTable ExportDT, string sql, string[] paras, List<string> ColumnKey, ArrayList ColumnOpen, ArrayList ColumnData, HttpContext Context, string SiteID, string[] Consts, string MainTable, int RateNumType, int PassedNums, bool IsBlockData, string BlockDataDefine, Dictionary<string, int> OpedApiPathList, int level, Dictionary<string, object> reqDic,bool isJson,JObject jObject, (bool ShowBaseLog, bool ShowInputLog, bool ShowOutputLog) IsOutputLog)
        {
            DR rdr = db.OpenRecord(sql);
            DR dr2;
            BlockDataDefine = adv.GetSpecialBase(Context, BlockDataDefine, SiteID);
            int LoopI = 0;
            Dictionary<string, Dictionary<string, string>> KeyValueDic = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> CodeKVDic = new Dictionary<string, string>();
            var mtKey = rdr.Key(MainTable);
            while (rdr.Read())
            {
                if (IsBlockData)
                {
                    if (LoopI > 0) json.Append(",");
                    if(mtKey.KeyType==Enums.KeyType.SnowId || mtKey.KeyType == Enums.KeyType.AutoIncrement)
                    {
                        json.Append("{\"fid\":" + rdr.GetValue(mtKey.KeyName).ToString().Replace("\"", "\\\"").NewLineReplace() + ",\"row\":");
                    }
                    else
                    {
                        json.Append("{\"fid\":\"" + rdr.GetString(mtKey.KeyName).Replace("\"", "\\\"").NewLineReplace() + "\",\"row\":");
                    }
                    LoopI++;
                    string BlockDataDefine_T = BlockDataDefine;
                    Regex r = new Regex(@"\[[\w\.]*\]");
                    MatchCollection mc = r.Matches(BlockDataDefine_T);
                    foreach (Match m in mc)
                    {
                        BlockDataDefine_T = BlockDataDefine_T.Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                    }
                    BlockDataDefine_T = Interface.Code.Get(BlockDataDefine_T, reqDic, Context);
                    json.Append("\"" + func.escape(BlockDataDefine_T) + "\"}");
                    continue;
                }
                if (LoopI > 0) json.Append(",");
                //var keyName = rdr.Key(MainTable).KeyName;
                if (mtKey.KeyType == Enums.KeyType.SnowId || mtKey.KeyType == Enums.KeyType.AutoIncrement)
                {
                    json.Append("{\"fid\":" + (rdr.IsDBNull(mtKey.KeyName) ? "0" : rdr.GetValue(mtKey.KeyName).ToString().Replace("\"", "\\\"").NewLineReplace()) + "");
                }
                else
                {
                    json.Append("{\"fid\":\"" + (rdr.IsDBNull(mtKey.KeyName) ? "" : rdr.GetString(mtKey.KeyName).Replace("\"", "\\\"").NewLineReplace()) + "\"");
                }
                
                if (RateNumType > 0)
                {
                    if (RateNumType == 1)
                    {
                        json.Append(",\"num\":" + (LoopI + 1) + "");
                    }
                    else if (RateNumType == 2)
                    {
                        json.Append(",\"num\":" + (LoopI + 1 + PassedNums) + "");
                    }
                }
                LoopI++;
                var ColumnKeyValueDic = new Dictionary<string, string>();
                for (int i = 0; i < ColumnOpen.Count; i++)
                {
                    if (!bool.Parse(ColumnOpen[i].ToString())) continue;
                    string rowtddata = "";
                    bool IsJsonData = false;
                    bool IsQuata = true;
                    string[] rowdatas = NormalKeyValue(ColumnData[i].ToString(), ColumnKeyValueDic).Split(';');
                    #region RowValue
                    for (int j = 0; j < rowdatas.Length; j++)
                    {
                        if (rowdatas[j].Equals("")) continue;
                        rowdatas[j] = rowdatas[j].Trim();
                        string rowwilladddata = "";
                        if (rowdatas[j].StartsWith("@str("))
                        {
                            //string dataitem = rowdatas[j].Replace("@str(", "").Replace(")", "");
                            string dataitem = rowdatas[j].Replace("#)#", "#*1*#").Replace("@str(", "").Replace(")", "").Replace("#*1*#", ")");
                            dataitem = adv.GetSpecialBase(Context, dataitem, SiteID);
                            Regex r = new Regex(@"\[[\w\.]*\]");
                            MatchCollection mc = r.Matches(dataitem);
                            foreach (Match m in mc)
                            {
                                dataitem = dataitem.Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                            }
                            rowwilladddata = dataitem;
                        }
                        //else if (rowdatas[j].StartsWith("@getConst("))
                        //{
                        //    rowwilladddata = Consts[Convert.ToInt32(rowdatas[j].Replace("@getConst(", "").Replace(")", "")) - 1];
                        //}
                        //else if (rowdatas[j].StartsWith("@getValueAll("))
                        //{
                        //    string eleid = rowdatas[j].Replace("@getValueAll(", "").Replace(")", "");
                        //    try
                        //    {
                        //        string elevalue = "/" + rdr.GetValue(rdr.GetOrdinal(eleid)).ToString();
                        //        string sqlele = "select evalue from " + MainTable + "_dy where fid='" + str.D2DD(rdr.GetString("fid")) + "' and eid='" + str.D2DD(eleid) + "' order by erate";
                        //        dr2 = db2.OpenRecord(sqlele);
                        //        while (dr2.Read())
                        //        {
                        //            elevalue += "/" + dr2.GetValue(0).ToString();
                        //        }
                        //        dr2.Close();
                        //        if (!elevalue.Equals("")) elevalue = elevalue.Substring(1);
                        //        rowwilladddata = elevalue;
                        //    }
                        //    catch (Exception e) { rowwilladddata = e.Message; }
                        //}
                        //else if (rowdatas[j].StartsWith("@getPValue("))
                        //{
                        //    string dataitem = rowdatas[j].Replace("@getPValue(", "").Replace(")", "");
                        //    string pdata = dataitem.Split(',')[0];
                        //    string peleid = dataitem.Split(',')[1];
                        //    string sqlele = "select " + peleid + " from ft_" + SiteID + "_f_" + pdata + " where fid='" + str.D2DD(rdr.GetString("pid")) + "'";
                        //    try
                        //    {
                        //        string elevalue = "";
                        //        dr2 = db2.OpenRecord(sqlele);
                        //        if (dr2.Read())
                        //        {
                        //            elevalue = dr2.GetValue(0).ToString();
                        //        }
                        //        dr2.Close();
                        //        rowwilladddata = elevalue;
                        //    }
                        //    catch (Exception e) { rowwilladddata = e.Message; }
                        //}
                        //else if (rowdatas[j].StartsWith("@getPValueAll("))
                        //{
                        //    string dataitem = rowdatas[j].Replace("@getPValueAll(", "").Replace(")", "");
                        //    string pdata = dataitem.Split(',')[0];
                        //    string peleid = dataitem.Split(',')[1];
                        //    string sqlele = "select " + peleid + " from ft_" + SiteID + "_f_" + pdata + " where fid='" + str.D2DD(rdr.GetString("pid")) + "'";
                        //    try
                        //    {
                        //        string elevalue = "";
                        //        dr2 = db2.OpenRecord(sqlele);
                        //        elevalue = "";
                        //        if (dr2.Read())
                        //        {
                        //            elevalue += "/" + dr2.GetValue(0).ToString();
                        //        }
                        //        dr2.Close();
                        //        sqlele = "select evalue from ft_" + SiteID + "_f_" + pdata + "_dy where fid='" + str.D2DD(rdr.GetString("pid")) + "' and eid='" + str.D2DD(peleid) + "' order by erate";
                        //        dr2 = db2.OpenRecord(sqlele);
                        //        while (dr2.Read())
                        //        {
                        //            elevalue += "/" + dr2.GetValue(0).ToString();
                        //        }
                        //        dr2.Close();
                        //        if (!elevalue.Equals("")) elevalue = elevalue.Substring(1);
                        //        rowwilladddata = elevalue;
                        //    }
                        //    catch (Exception e) { rowwilladddata = e.Message; }
                        //}
                        else if (rowdatas[j].StartsWith("@SQL{"))
                        {
                            int firstIndex = rowdatas[j].IndexOf('{');
                            int lastIndex = rowdatas[j].LastIndexOf('}');
                            string itemsql = rowdatas[j].Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                            itemsql = adv.GetSpecialBase(Context, itemsql, SiteID, true);
                            Regex r = new Regex(@"\[[\w\.]*\]");
                            MatchCollection mc = r.Matches(itemsql);
                            foreach (Match m in mc)
                            {
                                itemsql = itemsql.Replace(m.Value, str.D2DD(rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val));
                            }
                            for (int ix = 1; ix < paras.Length; ix++) itemsql = itemsql.Replace("@p" + ix + "@", paras[ix]).Replace("{p" + ix + "}", paras[ix]);
                            itemsql = DataOPRootValue(itemsql, isJson, Context.Request, jObject, true);
                            itemsql = adv.CodePattern(Context, itemsql, reqDic, CodeKVDic);
                            itemsql = adv.ParaPattern(Context, itemsql, reqDic, null, CodeKVDic);
                            try
                            {
                                //非一行一列值则为json格式
                                string elevalue = null;
                                StringBuilder sbMulti = new StringBuilder();
                                sbMulti.Append("[");
                                int loopMulti = 0;
                                if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(itemsql, "[List Loop ItemSql]", Context, reqDic);
                                var quata = true;
                                dr2 = db2.OpenRecord(itemsql);
                                var isJsonArray = dr2.FieldCount > 1;
                                while (dr2.Read())
                                {
                                    if (!IsJsonData && (loopMulti > 0 || dr2.FieldCount > 1)) IsJsonData = true;
                                    if (!IsJsonData) {
                                        var elevalueObj = dr2.GetValue(0);
                                        if(elevalueObj == null)
                                        {
                                            quata = false;
                                            elevalue = "null";
                                        }
                                        else
                                        {
                                            var elevalueTuple = DBSuit.ValueQuotation(elevalueObj);
                                            elevalue = elevalueTuple.val;
                                            quata=  elevalueTuple.quotation;
                                        }
                                    }
                                    if (loopMulti > 0) sbMulti.Append(",");
                                    sbMulti.Append("{");
                                    for (int fi = 0; fi < dr2.FieldCount; fi++)
                                    {
                                        if (fi > 0) sbMulti.Append(",");
                                        var elevalueObj = dr2.GetValue(fi);
                                        var _quata = false;
                                        var _elevalue = "";
                                        if (elevalueObj == null)
                                        {
                                            _quata = false;
                                            _elevalue = "null";
                                        }
                                        else
                                        {
                                            var elevalueTuple = DBSuit.ValueQuotation(elevalueObj);
                                            _elevalue = elevalueTuple.val;
                                            _quata = elevalueTuple.quotation;
                                        }
                                        if(_quata)
                                        {
                                            sbMulti.Append("\"" + dr2.GetName(fi) + "\":\"" + _elevalue.NewLineReplace() + "\"");
                                        }
                                        else
                                        {
                                            sbMulti.Append("\"" + dr2.GetName(fi) + "\":" + _elevalue + "");
                                        }
                                    }
                                    sbMulti.Append("}");
                                    loopMulti++;
                                }
                                dr2.Close();
                                sbMulti.Append("]");
                                if (IsJsonData) rowwilladddata = sbMulti.ToString();
                                else rowwilladddata = elevalue ?? "";
                                IsQuata = quata;
                                if(isJsonArray && string.IsNullOrWhiteSpace(rowwilladddata))
                                {
                                    IsQuata = false;
                                    rowwilladddata = "[]";
                                }
                                sbMulti.Clear();
                                sbMulti = null;
                            }
                            catch (Exception e)
                            {
                                IsJsonData = false;
                                rowwilladddata = e.Message;
                                FTFrame.Project.Core.Api.LogError(e.Message + ".itemsql:" + itemsql, "[List @SQL]", Context, reqDic);
                            }
                        }
                        else if (rowdatas[j].StartsWith("@KeyValue{", StringComparison.CurrentCultureIgnoreCase))
                        {
                            //@KeyValue{colname,1,sql}
                            int firstI = rowdatas[j].IndexOf(',');
                            int secondI = rowdatas[j].IndexOf(',', firstI + 1);
                            string colname = rowdatas[j].Substring(0, firstI).Replace("@KeyValue{", "", StringComparison.CurrentCultureIgnoreCase).Replace(",", "").Trim();
                            //0全部转义，1仅导出转义，2仅列表转义
                            int kvType = int.Parse(rowdatas[j].Substring(firstI + 1, secondI - firstI).Replace(",", "").Trim());
                            var cval = rdr.CommonValue(colname, MainTable);
                            rowwilladddata = cval.val;
                            //0全部转义，1仅导出转义，2仅列表转义
                            if (kvType == 0 || (kvType == 1 && NeedExport) || (kvType == 2 && !NeedExport))
                            {
                                //Build KeyValueDic
                                if (!KeyValueDic.ContainsKey(ColumnKey[i] + "_" + colname))
                                {
                                    string itemsql = rowdatas[j].Substring(secondI + 1);
                                    itemsql = DataOPRootValue(itemsql, isJson, Context.Request, jObject, true);
                                    itemsql = adv.GetSpecialBase(Context, itemsql, SiteID, true);
                                    itemsql = adv.CodePattern(Context, itemsql, reqDic);
                                    itemsql = adv.ParaPattern(Context, itemsql, reqDic);
                                    itemsql = itemsql.Replace("#}#", "#*2*#").Replace("}", "").Replace("#*2*#", "}");
                                    Regex r = new Regex(@"\[[\w\.]*\]");
                                    MatchCollection mc = r.Matches(itemsql);
                                    foreach (Match m in mc)
                                    {
                                        itemsql = itemsql.Replace(m.Value, str.D2DD(rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val));
                                    }
                                    if (IsOutputLog.ShowBaseLog) FTFrame.Project.Core.Api.LogDebug(itemsql, "[List KeyValue]", Context, reqDic);
                                    Dictionary<string, string> dic = new Dictionary<string, string>();
                                    using (dr2 = db2.OpenRecord(itemsql))
                                    {
                                        while (dr2.Read())
                                        {
                                            if (!dic.ContainsKey(dr2.GetStringForceNoNULL(0))) dic.Add(dr2.GetStringForceNoNULL(0), dr2.GetStringForceNoNULL(1));
                                        }
                                    }
                                    KeyValueDic.Add(ColumnKey[i] + "_" + colname, dic);
                                }
                                rowwilladddata = KeyValueDic[ColumnKey[i] + "_" + colname].GetValueOrDefault(rowwilladddata) ?? "";
                            }
                        }
                        else if (rowdatas[j].StartsWith("@api_"))
                        {
                            string ApiPath = rowdatas[j].Substring(5);
                            int index = ApiPath.IndexOf("?");
                            if (index > 0)
                            {
                                string[] newparas = ApiPath.Substring(index + 1).Split('/');
                                string key = newparas[0];
                                for (int i2 = 1; i2 < newparas.Length; i2++)
                                {
                                    Regex r = new Regex(@"\[[\w\.]*\]");
                                    MatchCollection mc = r.Matches(newparas[i2]);
                                    foreach (Match m in mc)
                                    {
                                        newparas[i2] = newparas[i2].Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                                    }
                                    for (int i3 = 1; i3 < paras.Length; i3++)
                                    {
                                        newparas[i2] = newparas[i2].Replace("@p" + i3 + "@", paras[i3]).Replace("{p" + i3 + "}", paras[i3]);
                                    }
                                }
                                ApiPath = ApiPath.Substring(0, index) + "?" + key;
                                string apisql = "select * from ft_ftdp_apiset where ApiPath='" + str.D2DD(ApiPath) + "'";
                                using (DB dbFTDP = new DB(SysConst.ConnectionStr_FTDP))
                                {
                                    using (DR dr = dbFTDP.OpenRecord(apisql))
                                    {
                                        if (dr.Read())
                                        {
                                            string ApiType = dr.GetString("ApiType");
                                            if (ApiType == "List")
                                            {
                                                Dictionary<string, string> setDic = new Dictionary<string, string>();
                                                setDic.Add("partid", dr.GetString("PartID"));
                                                setDic.Add("Order", dr.GetString("Set_List_Order"));
                                                setDic.Add("schdefine", "");
                                                setDic.Add("SiteID", "ftdp");
                                                setDic.Add("InputType", "json");
                                                setDic.Add("sql", dr.GetString("Set_List_Sql"));//.Replace("\r\n", "\n").Replace("\n", "\\r\\n"));
                                                setDic.Add("sqlCount", dr.GetString("Set_List_SqlCount"));//.Replace("\r\n", "\n").Replace("\n", "\\r\\n"));
                                                setDic.Add("RowAll", dr.GetString("Set_List_RowAll"));
                                                setDic.Add("Consts", "################################################");
                                                setDic.Add("BlockDataDefine", "");
                                                setDic.Add("UserCusCdn", dr.GetString("Set_List_CusCondition"));//.Replace("\r\n", "\n").Replace("\n", "\\r\\n"));
                                                setDic.Add("CacuRowData", "");
                                                setDic.Add("CustomConnection", dr.GetStringNoNULL("CustomConnection"));
                                                setDic.Add("MainTable", "");
                                                setDic.Add("NumsPerPage", "0");
                                                //setDic.Add("ColDefine_Cur", dr.GetString("Set_List_ColDefine"));
                                                setDic.Add("KeyName", "fid");
                                                setDic.Add("Col1ReqName", "");
                                                setDic.Add("Col2ReqName", "");
                                                setDic.Add("Col3ReqName", "");
                                                setDic.Add("Col1Name", "");
                                                setDic.Add("Col2Name", "");
                                                setDic.Add("Col3Name", "");
                                                rowwilladddata = Api.Json("List", Context, setDic, newparas, true, OpedApiPathList, level + 1, ApiPath);
                                                IsJsonData = true;
                                                setDic.Clear(); setDic = null;
                                            }
                                            else if (ApiType == "DyValue")
                                            {
                                                Dictionary<string, string> setDic = new Dictionary<string, string>();
                                                setDic.Add("partid", dr.GetString("PartID"));
                                                setDic.Add("DefaultFID", dr.GetString("Set_DyValue_DefaultFID"));
                                                setDic.Add("SiteID", "ftdp");
                                                setDic.Add("DefineStr", dr.GetString("Set_DyValue_DefineStr"));
                                                setDic.Add("ApiDefine", dr.GetString("Set_DyValue_ApiDefine"));
                                                setDic.Add("FidCol", dr.GetString("Set_DyValue_FidCol"));
                                                setDic.Add("OpDefaultCol", dr.GetInt16("Set_DyValue_OpDefaultCol").ToString());
                                                setDic.Add("CustomConnection", dr.GetStringNoNULL("CustomConnection"));
                                                rowwilladddata = Api.Json("DyValue", Context, setDic, newparas, true, OpedApiPathList, level + 1, ApiPath);
                                                IsJsonData = true;
                                                setDic.Clear(); setDic = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (rowdatas[j].StartsWith("$"))
                        {
                            string[] coldatasbuts = rowdatas[j].Replace("$", "").Split('|');
                            if (coldatasbuts[2].StartsWith("@getConst("))
                            {
                                coldatasbuts[2] = Consts[Convert.ToInt32(coldatasbuts[2].Replace("@getConst(", "").Replace(")", "")) - 1];
                            }

                            if (coldatasbuts[0].Trim().Equals("") || ("," + coldatasbuts[0].Trim() + ",").IndexOf("," + rdr.GetInt32(rdr.GetOrdinal("flow")).ToString() + ",") >= 0)
                            {
                                if (int.Parse(coldatasbuts[1]) == 0)
                                {
                                    if (coldatasbuts[3] != "")
                                    {
                                        coldatasbuts[3] = adv.GetSpecialBase(Context, coldatasbuts[3], SiteID);
                                        Regex r = new Regex(@"\[[\w\.]*\]");
                                        MatchCollection mc = r.Matches(coldatasbuts[3]);
                                        foreach (Match m in mc)
                                        {
                                            coldatasbuts[3] = coldatasbuts[3].Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                                        }
                                        rowwilladddata = "<a href=\"javascript:void(0)\" onclick=\"" + coldatasbuts[3] + "\">" + coldatasbuts[2] + "</a>";

                                    }
                                    else
                                    {
                                        rowwilladddata = coldatasbuts[2];
                                    }
                                }
                                else if (int.Parse(coldatasbuts[1]) == 1)
                                {
                                    coldatasbuts[3] = adv.GetSpecialBase(Context, coldatasbuts[3], SiteID);
                                    Regex r = new Regex(@"\[[\w\.]*\]");
                                    MatchCollection mc = r.Matches(coldatasbuts[3]);
                                    foreach (Match m in mc)
                                    {
                                        coldatasbuts[3] = coldatasbuts[3].Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                                    }
                                    rowwilladddata = "<button type=button onclick=\"" + coldatasbuts[3] + "\" class=\"_button\">" + coldatasbuts[2] + "</button>";
                                }
                            }
                        }
                        else if (rowdatas[j].StartsWith("~"))
                        {
                            //~0|(0|a1)(1|@getConst(7))(other|[a.fid])~
                            //~1|abc(null|xx)(0|a1)(1|a2)(other|a3)~
                            //~2|abc(null|xx)(0|a1)(1|a2)(other|a3)~
                            int extpfirst = 0;
                            int extplast = 0;
                            extpfirst = rowdatas[j].IndexOf('|');
                            int extlevel = int.Parse(rowdatas[j].Substring(1, extpfirst - 1));
                            string extflowvalue = "null";
                            if (extlevel == 0)
                            {
                                extflowvalue = rdr.GetInt32("flow").ToString();
                            }
                            else
                            {

                            }
                            int _pass1 = rowdatas[j].IndexOf("(" + extflowvalue + "|");
                            int _pass2 = rowdatas[j].IndexOf("(" + extflowvalue + ",");
                            int _pass3 = rowdatas[j].IndexOf("," + extflowvalue + "|");
                            int _pass4 = rowdatas[j].IndexOf("," + extflowvalue + ",");
                            if (_pass1 < 0 && _pass2 < 0 && _pass3 < 0 && _pass4 < 0)//多状态支持 (2,3,4|abc)
                            {
                                extflowvalue = "other";
                                _pass1 = rowdatas[j].IndexOf("(" + extflowvalue + "|");
                                _pass2 = rowdatas[j].IndexOf("(" + extflowvalue + ",");
                                _pass3 = rowdatas[j].IndexOf("," + extflowvalue + "|");
                                _pass4 = rowdatas[j].IndexOf("," + extflowvalue + ",");
                            }

                            extpfirst = (_pass1 >= 0 ? _pass1 : ((_pass2 >= 0 ? _pass2 : ((_pass3 >= 0 ? _pass3 : _pass4)))));
                            if (extpfirst >= 0)
                            {
                                extplast = rowdatas[j].IndexOf(')', extpfirst);
                                string dataitem = rowdatas[j].Substring(extpfirst + 1, extplast - extpfirst - 1).Split('|')[1];
                                dataitem = dataitem.Replace("#}", ")");
                                if (dataitem.StartsWith("@getConst("))
                                {
                                    rowwilladddata = Consts[int.Parse(dataitem.Replace("@getConst(", "")) - 1];
                                }
                                else
                                {
                                    Regex r = new Regex(@"\[[\w\.]*\]");
                                    MatchCollection mc = r.Matches(dataitem);
                                    foreach (Match m in mc)
                                    {
                                        dataitem = dataitem.Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                                    }
                                    rowwilladddata = dataitem;
                                }
                            }
                        }
                        else if (rowdatas[j].StartsWith("!"))
                        {
                            //!abcd(null|x1)(|x2)(val0|x3)(other|x4)!
                            string passeleid = rowdatas[j].Substring(1, rowdatas[j].IndexOf('(') - 1);
                            string passelevalue = rdr.CommonValue(passeleid, MainTable).val;

                            int extpfirst = 0;
                            int extplast = 0;

                            int _pass1 = rowdatas[j].IndexOf("(" + passelevalue + "|");
                            int _pass2 = rowdatas[j].IndexOf("(" + passelevalue + ",");
                            int _pass3 = rowdatas[j].IndexOf("," + passelevalue + "|");
                            int _pass4 = rowdatas[j].IndexOf("," + passelevalue + ",");
                            if (_pass1 < 0 && _pass2 < 0 && _pass3 < 0 && _pass4 < 0)//多值支持 (2,3,4|abc)
                            {
                                passelevalue = "other";
                                _pass1 = rowdatas[j].IndexOf("(" + passelevalue + "|");
                                _pass2 = rowdatas[j].IndexOf("(" + passelevalue + ",");
                                _pass3 = rowdatas[j].IndexOf("," + passelevalue + "|");
                                _pass4 = rowdatas[j].IndexOf("," + passelevalue + ",");
                            }

                            extpfirst = (_pass1 >= 0 ? _pass1 : ((_pass2 >= 0 ? _pass2 : ((_pass3 >= 0 ? _pass3 : _pass4)))));
                            if (extpfirst >= 0)
                            {
                                extplast = rowdatas[j].IndexOf(')', extpfirst);
                                string dataitem = rowdatas[j].Substring(extpfirst + 1, extplast - extpfirst - 1).Split('|')[1];
                                dataitem = dataitem.Replace("#}", ")");
                                if (dataitem.StartsWith("@getConst("))
                                {
                                    rowwilladddata = Consts[int.Parse(dataitem.Replace("@getConst(", "")) - 1];
                                }
                                else
                                {
                                    Regex r = new Regex(@"\[[\w\.]*\]");
                                    MatchCollection mc = r.Matches(dataitem);
                                    foreach (Match m in mc)
                                    {
                                        dataitem = dataitem.Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                                    }
                                    rowwilladddata = dataitem;
                                }
                            }
                        }
                        else if (rowdatas[j].StartsWith("@code("))
                        {
                            string dataitem = adv.GetSpecialBase(Context, rowdatas[j], SiteID);
                            dataitem = DataOPRootValue(dataitem, isJson, Context.Request, jObject, false);
                            var codeParaDic = adv.CodeParaDic(rdr, dataitem);
                            //没有替换[] 存到缓存
                            if(codeParaDic.Count==0)
                            {
                                if (!CodeKVDic.ContainsKey(dataitem)) CodeKVDic.Add(dataitem, Interface.Code.Get(dataitem, reqDic, Context));
                                rowwilladddata = CodeKVDic[dataitem];
                            }
                            else // 有替换，使用后替换的参数注入
                            {
                                rowwilladddata = Interface.Code.Get(dataitem, reqDic, codeParaDic, Context);
                            }
                            //Regex r = new Regex(@"\[[\w\.]*\]");
                            //MatchCollection mc = r.Matches(dataitem);
                            //foreach (Match m in mc)
                            //{
                            //    dataitem = dataitem.Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                            //}
                            //if (!CodeKVDic.ContainsKey(dataitem)) CodeKVDic.Add(dataitem, Interface.Code.Get(dataitem, reqDic, Context));
                            //rowwilladddata = CodeKVDic[dataitem];
                        }
                        else if (rowdatas[j].StartsWith("@para{"))
                        {
                            string dataitem = adv.GetSpecialBase(Context, rowdatas[j], SiteID);
                            var codeParaDic = adv.CodeParaDic(rdr, dataitem);
                            dataitem = DataOPRootValue(dataitem, isJson, Context.Request, jObject, false);
                            rowwilladddata = adv.ParaPattern(Context, adv.CodePattern(Context, dataitem, reqDic,null, codeParaDic), reqDic, null, CodeKVDic);
                            //Regex r = new Regex(@"\[[\w\.]*\]");
                            //MatchCollection mc = r.Matches(dataitem);
                            //foreach (Match m in mc)
                            //{
                            //    dataitem = dataitem.Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                            //}
                            //rowwilladddata = adv.ParaPattern(Context, adv.CodePattern(Context, dataitem, reqDic), reqDic, null, CodeKVDic);
                        }
                        else if (rowdatas[j].StartsWith("@enum("))
                        {
                            string dataitem = adv.GetSpecialBase(Context, rowdatas[j], SiteID);
                            Regex r = new Regex(@"\[[\w\.]*\]");
                            MatchCollection mc = r.Matches(dataitem);
                            foreach (Match m in mc)
                            {
                                dataitem = dataitem.Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                            }
                            rowwilladddata = adv.EnumPattern(adv.CodePattern(Context, dataitem, reqDic));
                        }
                        else
                        {
                            var cval = rdr.CommonValue(rowdatas[j], MainTable);
                            rowwilladddata = cval.val;
                            IsQuata = cval.quotation;
                        }
                        if (!rowtddata.Equals(""))
                        {
                            rowtddata += " ";
                            IsQuata = true;
                        }
                        rowtddata += rowwilladddata;

                    }
                    #endregion
                    if(rowtddata.StartsWith("#NoQuata#"))
                    {
                        rowtddata = rowtddata.Substring("#NoQuata#".Length);
                        IsQuata = false;
                    }
                    if (!ColumnKeyValueDic.ContainsKey(ColumnKey[i])) ColumnKeyValueDic.Add(ColumnKey[i],rowtddata);
                    if (IsJsonData) json.Append(",\"" + ColumnKey[i] + "\":" + rowtddata + "");
                    else if (ColumnKey[i].ToLower() != "fid")
                    {
                        if (IsQuata) json.Append(",\"" + ColumnKey[i] + "\":\"" + rowtddata.Replace("\"", "\\\"").NewLineReplace() + "\"");
                        else json.Append(",\"" + ColumnKey[i] + "\":" + rowtddata + "");
                    }
                    if (NeedExport)
                    {
                        if (i == 0)
                        {
                            ExportDT.Rows.Add(new string[] { FTFrame.Tool.str.HTML2Text(rowtddata) });
                        }
                        else
                        {
                            ExportDT.Rows[ExportDT.Rows.Count - 1][i] = FTFrame.Tool.str.HTML2Text(rowtddata);
                        }
                    }
                }
                json.Append("}");
                ColumnKeyValueDic.Clear();
            }
            rdr.Close();
            KeyValueDic.Clear();
            CodeKVDic.Clear();
        }

        private static void List_Loop_DataFromList(StringBuilder json, out int count, DB db, DB db2, bool NeedExport, DataTable ExportDT, string sql, string[] paras, List<string> ColumnKey, ArrayList ColumnOpen, ArrayList ColumnData, HttpContext Context, string SiteID, string[] Consts, string MainTable, int RateNumType, int PassedNums, bool IsBlockData, string BlockDataDefine, Dictionary<string, int> OpedApiPathList, int level, Dictionary<string, object> reqDic)
        {
            string code = sql.Substring("@FromList:".Length).Trim();
            List<Dictionary<string, object>> datas = Interface.Code.GetObject(code, reqDic, Context) as List<Dictionary<string, object>>;
            count = datas.Count();
            DR dr2;
            BlockDataDefine = adv.GetSpecialBase(Context, BlockDataDefine, SiteID);
            int LoopI = 0;
            foreach (var dic in datas)
            {
                if (IsBlockData)
                {
                    if (LoopI > 0) json.Append(",");
                    json.Append("{\"fid\":\"" + dic.GetValueOrDefault(DBSuit.Key(MainTable).KeyName) + "\",\"row\":");
                    LoopI++;
                    string BlockDataDefine_T = BlockDataDefine;
                    Regex r = new Regex(@"\[[\w\.]*\]");
                    MatchCollection mc = r.Matches(BlockDataDefine_T);
                    foreach (Match m in mc)
                    {
                        BlockDataDefine_T = BlockDataDefine_T.Replace(m.Value, dic.GetValueOrDefault(m.Value.Replace("[", "").Replace("]", ""))?.ToString() == null ? "" : dic.GetValueOrDefault(m.Value.Replace("[", "").Replace("]", ""))?.ToString());
                    }
                    BlockDataDefine_T = Interface.Code.Get(BlockDataDefine_T, reqDic, Context);
                    json.Append("\"" + func.escape(BlockDataDefine_T) + "\"}");
                    continue;
                }
                if (LoopI > 0) json.Append(",");
                json.Append("{\"fid\":\"" + dic.GetValueOrDefault(DBSuit.Key(MainTable).KeyName) + "\"");
                if (RateNumType > 0)
                {
                    if (RateNumType == 1)
                    {
                        json.Append(",\"num\":" + (LoopI + 1) + "");
                    }
                    else if (RateNumType == 2)
                    {
                        json.Append(",\"num\":" + (LoopI + 1 + PassedNums) + "");
                    }
                }
                LoopI++;
                for (int i = 0; i < ColumnOpen.Count; i++)
                {
                    if (!bool.Parse(ColumnOpen[i].ToString())) continue;
                    string rowtddata = "";
                    bool IsJsonData = false;
                    string[] rowdatas = ColumnData[i].ToString().Split(';');
                    #region RowValue
                    for (int j = 0; j < rowdatas.Length; j++)
                    {
                        if (rowdatas[j].Equals("")) continue;
                        rowdatas[j] = rowdatas[j].Trim();
                        string rowwilladddata = "";
                        if (rowdatas[j].StartsWith("@str("))
                        {
                            //string dataitem = rowdatas[j].Replace("@str(", "").Replace(")", "");
                            string dataitem = rowdatas[j].Replace("#)#", "#*1*#").Replace("@str(", "").Replace(")", "").Replace("#*1*#", ")");
                            dataitem = adv.GetSpecialBase(Context, dataitem, SiteID);
                            Regex r = new Regex(@"\[[\w\.]*\]");
                            MatchCollection mc = r.Matches(dataitem);
                            foreach (Match m in mc)
                            {
                                dataitem = dataitem.Replace(m.Value, dic.GetValueOrDefault(m.Value.Replace("[", "").Replace("]", "")) == null ? "" : dic.GetValueOrDefault(m.Value.Replace("[", "").Replace("]", "")).ToString());
                            }
                            rowwilladddata = dataitem;
                        }
                        else if (rowdatas[j].StartsWith("@getConst("))
                        {
                            rowwilladddata = Consts[Convert.ToInt32(rowdatas[j].Replace("@getConst(", "").Replace(")", "")) - 1];
                        }
                        else if (rowdatas[j].StartsWith("@getValueAll("))
                        {
                            string eleid = rowdatas[j].Replace("@getValueAll(", "").Replace(")", "");
                            try
                            {
                                string elevalue = "/" + dic.GetValueOrDefault(eleid)?.ToString();
                                string sqlele = "select evalue from " + MainTable + "_dy where fid='" + str.D2DD(dic.GetValueOrDefault("fid")?.ToString() ?? "") + "' and eid='" + str.D2DD(eleid) + "' order by erate";
                                dr2 = db2.OpenRecord(sqlele);
                                while (dr2.Read())
                                {
                                    elevalue += "/" + dr2.GetValue(0).ToString();
                                }
                                dr2.Close();
                                if (!elevalue.Equals("")) elevalue = elevalue.Substring(1);
                                rowwilladddata = elevalue;
                            }
                            catch (Exception e) { rowwilladddata = e.Message; }
                        }
                        else if (rowdatas[j].StartsWith("@getPValue("))
                        {
                            string dataitem = rowdatas[j].Replace("@getPValue(", "").Replace(")", "");
                            string pdata = dataitem.Split(',')[0];
                            string peleid = dataitem.Split(',')[1];
                            string sqlele = "select " + peleid + " from ft_" + SiteID + "_f_" + pdata + " where fid='" + str.D2DD(dic.GetValueOrDefault("pid")?.ToString() ?? "") + "'";
                            try
                            {
                                string elevalue = "";
                                dr2 = db2.OpenRecord(sqlele);
                                if (dr2.Read())
                                {
                                    elevalue = dr2.GetValue(0).ToString();
                                }
                                dr2.Close();
                                rowwilladddata = elevalue;
                            }
                            catch (Exception e) { rowwilladddata = e.Message; }
                        }
                        else if (rowdatas[j].StartsWith("@getPValueAll("))
                        {
                            string dataitem = rowdatas[j].Replace("@getPValueAll(", "").Replace(")", "");
                            string pdata = dataitem.Split(',')[0];
                            string peleid = dataitem.Split(',')[1];
                            string sqlele = "select " + peleid + " from ft_" + SiteID + "_f_" + pdata + " where fid='" + str.D2DD(dic.GetValueOrDefault("pid")?.ToString() ?? "") + "'";
                            try
                            {
                                string elevalue = "";
                                dr2 = db2.OpenRecord(sqlele);
                                elevalue = "";
                                if (dr2.Read())
                                {
                                    elevalue += "/" + dr2.GetValue(0).ToString();
                                }
                                dr2.Close();
                                sqlele = "select evalue from ft_" + SiteID + "_f_" + pdata + "_dy where fid='" + str.D2DD(dic.GetValueOrDefault("pid")?.ToString() ?? "") + "' and eid='" + str.D2DD(peleid) + "' order by erate";
                                dr2 = db2.OpenRecord(sqlele);
                                while (dr2.Read())
                                {
                                    elevalue += "/" + dr2.GetValue(0).ToString();
                                }
                                dr2.Close();
                                if (!elevalue.Equals("")) elevalue = elevalue.Substring(1);
                                rowwilladddata = elevalue;
                            }
                            catch (Exception e) { rowwilladddata = e.Message; }
                        }
                        else if (rowdatas[j].StartsWith("@SQL{"))
                        {
                            string itemsql = rowdatas[j].Replace("#}#", "#*2*#").Replace("@SQL{", "").Replace("}", "").Replace("#*2*#", "}");
                            itemsql = adv.GetSpecialBase(Context, itemsql, SiteID, true);
                            Regex r = new Regex(@"\[[\w\.]*\]");
                            MatchCollection mc = r.Matches(itemsql);
                            foreach (Match m in mc)
                            {
                                itemsql = itemsql.Replace(m.Value, str.D2DD(dic.GetValueOrDefault(m.Value.Replace("[", "").Replace("]", "")) == null ? "" : dic.GetValueOrDefault(m.Value.Replace("[", "").Replace("]", ""))?.ToString()));
                            }
                            try
                            {
                                string elevalue = "";
                                dr2 = db2.OpenRecord(itemsql);
                                if (dr2.Read())
                                {
                                    elevalue = dr2.GetValue(0).ToString();
                                }
                                dr2.Close();
                                rowwilladddata = (elevalue == null ? "" : elevalue);
                            }
                            catch (Exception e)
                            {
                                rowwilladddata = e.Message;
                                FTFrame.Project.Core.Api.LogError(e.Message + ".itemsql:" + itemsql, "[List @SQL]", Context, reqDic);
                            }
                        }
                        else if (rowdatas[j].StartsWith("@api_"))
                        {
                            string ApiPath = rowdatas[j].Substring(5);
                            int index = ApiPath.IndexOf("?");
                            if (index > 0)
                            {
                                string[] newparas = ApiPath.Substring(index + 1).Split('/');
                                string key = newparas[0];
                                for (int i2 = 1; i2 < newparas.Length; i2++)
                                {
                                    Regex r = new Regex(@"\[[\w\.]*\]");
                                    MatchCollection mc = r.Matches(newparas[i2]);
                                    foreach (Match m in mc)
                                    {
                                        newparas[i2] = newparas[i2].Replace(m.Value, dic.GetValueOrDefault(m.Value.Replace("[", "").Replace("]", "")) == null ? "" : dic.GetValueOrDefault(m.Value.Replace("[", "").Replace("]", ""))?.ToString());
                                    }
                                    for (int i3 = 1; i3 < paras.Length; i3++)
                                    {
                                        newparas[i2] = newparas[i2].Replace("@p" + i3 + "@", paras[i3]).Replace("{p" + i3 + "}", paras[i3]);
                                    }
                                }
                                ApiPath = ApiPath.Substring(0, index) + "?" + key;
                                string apisql = "select * from ft_ftdp_apiset where ApiPath='" + str.D2DD(ApiPath) + "'";
                                using (DB dbFTDP = new DB(SysConst.ConnectionStr_FTDP))
                                {
                                    using (DR dr = dbFTDP.OpenRecord(apisql))
                                    {
                                        if (dr.Read())
                                        {
                                            string ApiType = dr.GetString("ApiType");
                                            if (ApiType == "List")
                                            {
                                                Dictionary<string, string> setDic = new Dictionary<string, string>();
                                                setDic.Add("partid", dr.GetString("PartID"));
                                                setDic.Add("Order", dr.GetString("Set_List_Order"));
                                                setDic.Add("schdefine", "");
                                                setDic.Add("SiteID", "ftdp");
                                                setDic.Add("sql", dr.GetString("Set_List_Sql").Replace("\r\n", "\n").Replace("\n", "\\r\\n"));
                                                setDic.Add("sqlCount", dr.GetString("Set_List_SqlCount").Replace("\r\n", "\n").Replace("\n", "\\r\\n"));
                                                setDic.Add("RowAll", dr.GetString("Set_List_RowAll"));
                                                setDic.Add("Consts", "################################################");
                                                setDic.Add("BlockDataDefine", "");
                                                setDic.Add("UserCusCdn", dr.GetString("Set_List_CusCondition").Replace("\r\n", "\n").Replace("\n", "\\r\\n"));
                                                setDic.Add("CacuRowData", "");
                                                setDic.Add("CustomConnection", dr.GetStringNoNULL("CustomConnection"));
                                                setDic.Add("MainTable", "");
                                                setDic.Add("NumsPerPage", "0");
                                                //setDic.Add("ColDefine_Cur", dr.GetString("Set_List_ColDefine"));
                                                setDic.Add("KeyName", "fid");
                                                setDic.Add("Col1ReqName", "");
                                                setDic.Add("Col2ReqName", "");
                                                setDic.Add("Col3ReqName", "");
                                                setDic.Add("Col1Name", "");
                                                setDic.Add("Col2Name", "");
                                                setDic.Add("Col3Name", "");
                                                rowwilladddata = Api.Json("List", Context, setDic, newparas, true, OpedApiPathList, level + 1, ApiPath);
                                                IsJsonData = true;
                                                setDic.Clear(); setDic = null;
                                            }
                                            else if (ApiType == "DyValue")
                                            {
                                                Dictionary<string, string> setDic = new Dictionary<string, string>();
                                                setDic.Add("partid", dr.GetString("PartID"));
                                                setDic.Add("DefaultFID", dr.GetString("Set_DyValue_DefaultFID"));
                                                setDic.Add("SiteID", "ftdp");
                                                setDic.Add("DefineStr", dr.GetString("Set_DyValue_DefineStr"));
                                                setDic.Add("ApiDefine", dr.GetString("Set_DyValue_ApiDefine"));
                                                setDic.Add("FidCol", dr.GetString("Set_DyValue_FidCol"));
                                                setDic.Add("OpDefaultCol", dr.GetInt16("Set_DyValue_OpDefaultCol").ToString());
                                                setDic.Add("CustomConnection", dr.GetStringNoNULL("CustomConnection"));
                                                rowwilladddata = Api.Json("DyValue", Context, setDic, newparas, true, OpedApiPathList, level + 1, ApiPath);
                                                IsJsonData = true;
                                                setDic.Clear(); setDic = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (rowdatas[j].StartsWith("!"))
                        {
                            //!abcd(null|x1)(|x2)(val0|x3)(other|x4)!
                            string passeleid = rowdatas[j].Substring(1, rowdatas[j].IndexOf('(') - 1);
                            string passelevalue = dic.GetValueOrDefault(passeleid) == null ? "null" : dic.GetValueOrDefault(passeleid)?.ToString() ?? "";

                            int extpfirst = 0;
                            int extplast = 0;

                            int _pass1 = rowdatas[j].IndexOf("(" + passelevalue + "|");
                            int _pass2 = rowdatas[j].IndexOf("(" + passelevalue + ",");
                            int _pass3 = rowdatas[j].IndexOf("," + passelevalue + "|");
                            int _pass4 = rowdatas[j].IndexOf("," + passelevalue + ",");
                            if (_pass1 < 0 && _pass2 < 0 && _pass3 < 0 && _pass4 < 0)//多值支持 (2,3,4|abc)
                            {
                                passelevalue = "other";
                                _pass1 = rowdatas[j].IndexOf("(" + passelevalue + "|");
                                _pass2 = rowdatas[j].IndexOf("(" + passelevalue + ",");
                                _pass3 = rowdatas[j].IndexOf("," + passelevalue + "|");
                                _pass4 = rowdatas[j].IndexOf("," + passelevalue + ",");
                            }

                            extpfirst = (_pass1 >= 0 ? _pass1 : ((_pass2 >= 0 ? _pass2 : ((_pass3 >= 0 ? _pass3 : _pass4)))));
                            if (extpfirst >= 0)
                            {
                                extplast = rowdatas[j].IndexOf(')', extpfirst);
                                string dataitem = rowdatas[j].Substring(extpfirst + 1, extplast - extpfirst - 1).Split('|')[1];
                                dataitem = dataitem.Replace("#}", ")");
                                if (dataitem.StartsWith("@getConst("))
                                {
                                    rowwilladddata = Consts[int.Parse(dataitem.Replace("@getConst(", "")) - 1];
                                }
                                else
                                {
                                    Regex r = new Regex(@"\[[\w\.]*\]");
                                    MatchCollection mc = r.Matches(dataitem);
                                    foreach (Match m in mc)
                                    {
                                        dataitem = dataitem.Replace(m.Value, dic.GetValueOrDefault(m.Value.Replace("[", "").Replace("]", "")) == null ? "" : dic.GetValueOrDefault(m.Value.Replace("[", "").Replace("]", ""))?.ToString() ?? "");
                                    }
                                    rowwilladddata = dataitem;
                                }
                            }
                        }
                        else if (rowdatas[j].StartsWith("@code("))
                        {
                            string dataitem = adv.GetSpecialBase(Context, rowdatas[j], SiteID);
                            Regex r = new Regex(@"\[[\w\.]*\]");
                            MatchCollection mc = r.Matches(dataitem);
                            foreach (Match m in mc)
                            {
                                dataitem = dataitem.Replace(m.Value, dic.GetValueOrDefault(m.Value.Replace("[", "").Replace("]", "")) == null ? "" : dic.GetValueOrDefault(m.Value.Replace("[", "").Replace("]", ""))?.ToString() ?? "");
                            }
                            rowwilladddata = Interface.Code.Get(dataitem, reqDic, Context);
                        }
                        else
                        {
                            rowwilladddata = dic.GetValueOrDefault(rowdatas[j]) == null ? "" : dic.GetValueOrDefault(rowdatas[j])?.ToString() ?? "";
                        }
                        if (!rowtddata.Equals(""))
                        {
                            rowtddata += " ";
                        }
                        rowtddata += rowwilladddata;

                    }
                    #endregion
                    if (IsJsonData) json.Append(",\"" + ColumnKey[i] + "\":" + rowtddata + "");
                    else if (ColumnKey[i].ToLower() != "fid") json.Append(",\"" + ColumnKey[i] + "\":\"" + rowtddata.Replace("\"", "\\\"").NewLineReplace() + "\"");
                    if (NeedExport)
                    {
                        if (i == 0)
                        {
                            ExportDT.Rows.Add(new string[] { FTFrame.Tool.str.HTML2Text(rowtddata) });
                        }
                        else
                        {
                            ExportDT.Rows[ExportDT.Rows.Count - 1][i] = FTFrame.Tool.str.HTML2Text(rowtddata);
                        }
                    }
                }
                json.Append("}");
            }
        }
        public static void MapPageRoute(Microsoft.AspNetCore.Mvc.RazorPages.RazorPagesOptions options)
        {
            using (DB db = new DB(SysConst.ConnectionStr_FTDP))
            {
                db.Open();
                string sql = "select route,page from ft_ftdp_route";
                using (DR dr = db.OpenRecord(sql))
                {
                    while (dr.Read())
                    {
                        string page = dr.GetString(1);
                        if (page.StartsWith('~')) page = page.Substring(1);
                        if (page.EndsWith(".aspx")) page = page.Substring(0, page.Length - 5);
                        options.Conventions.AddPageRoute(page, dr.GetString(0));
                    }
                }
            }
        }
        public static string ApiPathReplace(string ApiPath)
        {
            string[] items = SysConst.RoutePathReplace.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ss in items)
            {
                string[] item = ss.Split(',');
                if (item.Length == 2)
                {
                    string s1 = item[0].Trim();
                    string s2 = item[1].Trim();
                    if (ApiPath.StartsWith(s1))
                    {
                        ApiPath = ApiPath.Substring(s1.Length);
                        ApiPath = s2 + ApiPath;
                    }
                }
            }
            return ApiPath;
        }
        public static void MapPageRouteAdd(string route, string pageid)
        {
            if (route.StartsWith("/")) route = route.Substring(1);
            string[] items = SysConst.RoutePathReplace.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            string page = "~/" + route + "";
            bool routeChanged = false;
            foreach (string ss in items)
            {
                string[] item = ss.Split(',');
                if (item.Length == 2)
                {
                    string s1 = item[0].Trim();
                    string s2 = item[1].Trim();
                    if (s1.StartsWith("/")) s1 = s1.Substring(1);
                    if (s2.StartsWith("/")) s2 = s2.Substring(1);
                    if (route.StartsWith(s1))
                    {
                        route = route.Substring(s1.Length);
                        route = s2 + route;
                        routeChanged = true;
                    }
                }
            }
            if (routeChanged)
            {
                using (DB db = new DB(SysConst.ConnectionStr_FTDP))
                {
                    db.Open();
                    string sql = "delete from ft_ftdp_route where route='" + str.D2DD(route) + "'";
                    db.ExecSql(sql);
                    sql = "insert into ft_ftdp_route(route,page,PageID)values('" + str.D2DD(route) + "','" + str.D2DD(page) + "','" + pageid.D2() + "')";
                    db.ExecSql(sql);
                }
            }
        }
    }
}
