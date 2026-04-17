using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using System.Linq;
using System.IO;
using System.Xml;
using System.Collections;
using CoreHttp = Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace FTFrame.Project.Core
{
    public class Api
    {
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
            log.Error(ex);
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
