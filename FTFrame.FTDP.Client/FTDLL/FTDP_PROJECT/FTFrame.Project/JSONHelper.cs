using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Net;

/// <summary>
///JSONHelper 的摘要说明
/// </summary>
/// 
namespace FTFrame.Project
{
    public static class JSONHelper
    {
        /// <summary>
        /// 将对象序列化成JSON格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSONjss(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

    }
}