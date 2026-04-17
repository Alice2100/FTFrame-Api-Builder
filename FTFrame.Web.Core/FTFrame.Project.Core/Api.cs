using Aop.Api.Domain;
using FTFrame.DBClient;
using FTFrame.Project.Core.Service;
using FTFrame.Tool;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Tls;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using static Aliyun.Api.LogService.Infrastructure.Serialization.Protobuf.Log.Types;
using static Mysqlx.Notice.Warning.Types;
using CoreHttp = Microsoft.AspNetCore.Http;

namespace FTFrame.Project.Core
{
    public class Api
    {
        /// <summary>
        /// Api内部日志的输出控制
        /// </summary>
        /// <param name="businessType"></param>
        /// <param name="paras"></param>
        /// <param name="context"></param>
        /// <param name="reqDic"></param>
        /// <returns></returns>
        public static (bool ShowBaseLog,bool ShowInputLog,bool ShowOutputLog) IsOutputLog(string businessType, string[] paras, HttpContext context, Dictionary<string, object> reqDic = null)
        {
            switch (businessType)
            {
                case "List":
                    return (true, false, false);
                case "DyValue":
                    return (true, false, false);
                case "DataOP":
                    return (true, true, false);
                default:
                    return (true, false, false);
            }
        }
        public static void LogBase(string level, string content, string other, HttpContext context, LoginInfo loginInfo)
        {
            try
            {
                var ip = context?.Request?.Headers["x-forwarded-for"].ToString();
                if (string.IsNullOrEmpty(ip))
                {
                    ip = (context?.Connection.RemoteIpAddress?.ToString()) ?? "";
                }
                AliyunLogService.WriteLog(new Dictionary<string, string>() {
                { "level", level },
                { "ip", ip},
                { "userId", loginInfo?.userId ?? "" },
                {"url", context?.Request?.GetEncodedPathAndQuery() ?? ""},
                { "content", (string.IsNullOrWhiteSpace(other)?"":("["+other+"]:"))+content }
            });
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            //存本地文件
            //if(level== "Debug")
            //{
            //    log.Debug(content, other);
            //}
            //else if (level == "Error")
            //{
            //    log.Error(content, other);
            //}
        }
        public static void LogBase(string level, string content, string other, HttpContext context, Dictionary<string, object> reqDic)
        {
            LogBase(level, content, other, context, User.UserInfo(reqDic));
        }
        #region Log
        public static void LogDebug(string content, string other = "", HttpContext context = null, Dictionary<string, object> reqDic = null)
        {
            LogBase("Debug", content, other, context, reqDic);
        }
        public static void LogDebug(string content, HttpContext context = null, Dictionary<string, object> reqDic = null)
        {
            LogBase("Debug", content, "", context, reqDic);
        }
        public static void LogError(Exception ex,HttpContext context = null, Dictionary<string, object> reqDic = null)
        {
            LogBase("Error", ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite.ToString(), "Exception", context, reqDic);
        }
        public static void LogError(string content, string other = "", HttpContext context = null, Dictionary<string, object> reqDic = null)
        {
            LogBase("Error", content, other, context, reqDic);
        }
        public static void LogError(string content, HttpContext context = null, Dictionary<string, object> reqDic = null)
        {
            LogBase("Error", content, "", context, reqDic);
        }
        #endregion
        public static ConcurrentDictionary<(string url, string userid, string[] paras, string jobjstr, string fid, string fidformod), DateTime> OPDuplicateBag = new ConcurrentDictionary<(string url, string userid, string[] paras, string jobjstr, string fid, string fidformod), DateTime>();

        public static (string url, string userid, string[] paras, DateTime dttime, string jobjstr, string fid, string fidformod) OPDuplicateObj(HttpContext context, string UserId, string[] paras, Dictionary<string, object> reqDic, JObject jObject, string defaultfid, string defaultfidForMod)
        {
            return (url: context?.Request?.GetEncodedPathAndQuery() ?? "", userid: UserId, paras: paras, dttime: DateTime.Now, jobjstr: jObject == null ? "" : JsonConvert.SerializeObject(jObject), fid: defaultfid, fidformod: defaultfidForMod);
        }
        public static string OPDuplicateCheck((string url, string userid, string[] paras, DateTime dttime, string jobjstr, string fid, string fidformod) dupobj, int seconds = 5)
        {
            var dps = OPDuplicateBag.Where(r => (dupobj.dttime - r.Value).TotalSeconds <= seconds);
            if (dps.Count() == 0) return null;
            var dpss = dps.Where(r => r.Key.url == dupobj.url && r.Key.userid == dupobj.userid && r.Key.paras.SequenceEqual(dupobj.paras) && r.Key.fid == dupobj.fid && r.Key.fidformod == dupobj.fidformod && r.Key.jobjstr == dupobj.jobjstr);
            if (dpss.Count() > 0) return "操作太频繁";
            else return null;
        }
        public static void OPDuplicateEnter((string url, string userid, string[] paras, DateTime dttime, string jobjstr, string fid, string fidformod) dupobj)
        {
            OPDuplicateBag.AddOrUpdate((dupobj.url, dupobj.userid, dupobj.paras, dupobj.jobjstr, dupobj.fid, dupobj.fidformod), dupobj.dttime, (key, oldValue) => dupobj.dttime);
        }
        public static void OPDuplicateOut((string url, string userid, string[] paras, DateTime dttime, string jobjstr, string fid, string fidformod) dupobj, int seconds = 5)
        {
            var dps = OPDuplicateBag.Where(r => (DateTime.Now - r.Value).TotalSeconds > seconds).ToArray();
            foreach (var dp in dps)
            {
                OPDuplicateBag.TryRemove(dp);
            }
        }
        /// <summary>
        /// Api authorization
        /// </summary>
        /// <param name="businessType">List,DyValue,DataOP</param>
        /// <param name="path">Api Path</param>
        /// <param name="paras">Sequence Parameters</param>
        /// <param name="context">HttpContext</param>
        /// <param name="reqDic"></param>
        /// <returns></returns>
        public static bool Auth(string businessType, string path, string[] paras, HttpContext context, Dictionary<string, object> reqDic = null)
        {
            //if (!User.IsLogin()) return false;
            string key = paras[0];
            return true;
        }
        public static string AuthFailedJson(string businessType, string path, string[] paras, HttpContext context)
        {
            return "{\"code\":401,\"" + MessageStr + "\":\"Authentication failed\"}";
        }
        public static string ExceptionJson(Exception ex)
        {
            //return "{\"code\":203,\"" + MessageStr + "\":\"" + NoWrap(ex.Message) + "\"}";
            LogError(ex);
            return "{\"code\":203,\"" + MessageStr + "\":\"error\"}";
        }
        public static string ErrorJson(string str)
        {
            return "{\"code\":203,\"" + MessageStr + "\":\"" + NoWrap(str) + "\"}";
        }
        public static string OperationSuccessJson(string tip = "Operation successful", Dictionary<string, string> AdvKeyValue = null, string NewId = "")
        {
            StringBuilder sb = new StringBuilder();
            if (AdvKeyValue != null)
            {
                foreach (KeyValuePair<string, string> kv in AdvKeyValue)
                {
                    sb.Append(",\"" + kv.Key + "\":\"" + NoWrap(kv.Value) + "\"");
                }
            }
            string json = "{\"code\":200,\"" + MessageStr + "\":\"success\",\"tip\":\"" + tip + "\",\"data\":{\"newId\":\"" + NewId + "\"" + sb.ToString() + "}}";
            sb.Clear();
            sb = null;
            AdvKeyValue.Clear();
            AdvKeyValue = null;
            return json;
        }
        public static string OperationBatchSuccessJson(int rowsAffected, string tip = "Operation successful")
        {
            return "{\"code\":200,\"" + MessageStr + "\":\"success\",\"tip\":\"" + tip + "\",\"data\":{\"rowsAffected\":" + rowsAffected + "}}";
        }
        public static string ResultJson(string resultJson)
        {
            return "{\"code\":200,\"" + MessageStr + "\":\"success\",\"data\":" + resultJson + "}";
        }
        public static string ResultJsonHead()
        {
            return "{\"code\":200,\"" + MessageStr + "\":\"success\",\"data\":";
        }
        private static string NoWrap(string s) => s.Replace("\r", "").Replace("\n", "");


        public readonly static string MessageStr = "msg";
        /// <summary>
        /// Obtain an array of parameters, with the first one being the key
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string[] Paras(HttpContext context)
        {
            string url = context.Request.GetDisplayUrl().Trim();
            log.Debug(url,"[URL]");
            int index = url.IndexOf("?");
            if (index > 0)
            {
                string paras = url.Substring(index + 1);
                return paras.Split('/').Select(r => HttpUtility.UrlDecode(r)).ToArray();
            }
            return null;
        }
        /// <summary>
        /// Return Url without parameters as http://domin/api?list
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string BaseUrl(HttpContext context)
        {
            string url = context.Request.GetDisplayUrl().Trim();
            int index = url.IndexOf("?");
            if (index > 0)
            {
                string paras = url.Substring(index + 1);
                var ps = paras.Split('/').Select(r => HttpUtility.UrlDecode(r)).ToArray();
                return url.Substring(0, index) + "?" + ps[0];
            }
            return "";
        }
    }
}
