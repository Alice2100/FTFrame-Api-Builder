using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FTFrame;
using System.Web;
using System.Collections;
using System.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Net;
using FTFrame.DBClient;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;

public static class FTFrameExtensions
{
    /// <summary>
    /// 单引号转换为双单引号
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string D2(this string str)
    {
        return FTFrame.Tool.str.D2DD(str);
    }
    /// <summary>
    /// 单引号替换为空
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string D0(this string str)
    {
        return str.Replace("'", "");
    }
    public static string NoDotNoWrap(this string str)
    {
        return str.Replace("'", "").Replace("\"", "").Replace("\r", "").Replace("\n", "");
    }
    public static string NoWrap(this string str)
    {
        return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'");
    }
    /// <summary>
    /// 强制转换非常规字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Normal(this string str)
    {
        if (str.IndexOf('#') > 0) str = str.Split('#')[0];
        return FTFrame.Tool.str.Nomal(str);
    }
    public static string NormalAllowDot(this string str)
    {
        if (str.IndexOf('#') > 0) str = str.Split('#')[0];
        return FTFrame.Tool.str.Nomal(str,true);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string NewLineReplace(this string s)
    {
        return s==null?"":s.Replace("\t", "\\t").Replace("\r\n", "\n").Replace("\n", Environment.NewLine=="\n"?"\\n":"\\r\\n");
    }
    /// <summary>
    /// Reqeust 参数转换为实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="request"></param>
    /// <returns></returns>
    public static T ToType<T>(this HttpRequest request, Type type = null, string dllName = null) where T : new()
    {
        T t = new T();
        foreach (System.Reflection.PropertyInfo item in FTFrame.CacheHelper.ObjectProperties(t.GetType().FullName, type, dllName))
        {
            if (request.Query.ContainsKey(item.Name) && request.Query[item.Name].FirstOrDefault<object>() != null)
            {
                item.SetValue(t, FTFrame.Tool.Advance.DRChangeType(request.Query[item.Name].FirstOrDefault<object>(), item.PropertyType), null);
            }
            else if (request.HasFormContentType)
            {
                if (request.Form.ContainsKey(item.Name) && request.Form[item.Name].FirstOrDefault<object>() != null)
                {
                    item.SetValue(t, FTFrame.Tool.Advance.DRChangeType(request.Form[item.Name].FirstOrDefault<object>(), item.PropertyType), null);
                }
            }
        }
        return t;
    }
    public static string FormString(this HttpRequest request, string key)
    {
        return request.Form[key].FirstOrDefault();
    }
    public static string QueryString(this HttpRequest request, string key)
    {
        return request.Query[key].FirstOrDefault();
    }
    public static T ToType<T>(this DR dr, Type type = null, string dllName = null) where T : new()
    {
        //int FieldIndex(DR dr2, string FieldName)
        //{
        //    for (int i = 0; i < dr2.FieldCount; i++)
        //    {
        //        if (dr2.GetName(i).ToLower() == FieldName.ToLower()) return i;
        //    }
        //    return -1;
        //}
        T t = new T();
        foreach (System.Reflection.PropertyInfo item in FTFrame.CacheHelper.ObjectProperties(t.GetType().FullName, type, dllName))
        {
            int index = dr.FieldIndex(item.Name);//FieldIndex(dr, item.Name);
            if (index >= 0)
            {
                item.SetValue(t, FTFrame.Tool.Advance.DRChangeType(dr.GetValue(index), item.PropertyType), null);
            }
        }
        return t;
    }
}
namespace FTFrame.Tool
{
    public class str
    {
        public static string D2DD(string s)
        {
            return s.Replace("'", "''");
        }
        public static bool IsNomalStr(string str, bool allowDot = false)
        {
            if (string.IsNullOrEmpty(str)) return true;
            if (allowDot) str = str.Replace(".", "");
            return Regex.IsMatch(str, @"^[A-Za-z0-9_]+$"); 
        }
        public static string Nomal(string str, bool allowDot = false)
        {
            return IsNomalStr(str, allowDot) ? str : "NotCorrectNomalString";
        }
        public static string NewLineReplace(string s)
        {
            return s.Replace("\r\n", Environment.NewLine).Replace("\r", Environment.NewLine).Replace("\n", Environment.NewLine);
        }
        public static string JavascriptLabel(string s)
        {
            return "<script language=\"javascript\">" + s + "</script>";
        }
        public static string ContentHTML(string text)
        {
            return "<HTML><head><script src='" + SysConst.SubPath + "/_ft/_ftres/js/jquery-1.9.1.min.js'></script></head><BODY>" + text + "</BODY></HTML>";
        }
        public static string GetYearMonth()
        {
            return GetYearMonth(DateTime.Now);
        }
        public static string GetYearMonth(DateTime dt)
        {
            return dt.ToString("yyyy-MM", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        public static string GetDate(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        public static string GetDate(object dtstr)
        {
            if (dtstr == null) return "";
            return DateTime.Parse(dtstr.ToString()).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        public static DateTime GetDateTimeDate()
        {
            return GetDateTimeDate(DateTime.Now);
        }
        public static DateTime GetDateTimeDate(DateTime dt)
        {
            return DateTime.Parse(dt.Year + "-" + dt.Month + "-" + dt.Day);
        }
        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        public static string GetDateTime(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        public static string GetDateTime(object dtstr)
        {
            return DateTime.Parse(dtstr.ToString()).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        public static string GetDateTimeMil()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        public static string GetDateTimeCustom(DateTime dt, string pstring)
        {
            return dt.ToString(pstring, System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        public static string GetDateTimeCN(string datetime)
        {
            DateTime dt = Convert.ToDateTime(datetime);
            return dt.Year + "年" + dt.Month + "月" + dt.Day + "日";
        }
        public static int GetTimeSpanDays(DateTime dtbig, DateTime dtsmall)
        {
            TimeSpan ts = DateTime.Parse(GetDate(dtbig)) - DateTime.Parse(GetDate(dtsmall));
            return ts.Days;
        }
        public static string ToUnicodeString(string str)
        {
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    strResult.Append("\\u");
                    strResult.Append(((int)str[i]).ToString("x"));
                }
            }
            return strResult.ToString();
        }


        /// <summary>
        /// unicode解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FromUnicodeString(string str)
        {
            //最直接的方法Regex.Unescape(str);
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        int charCode = Convert.ToInt32(strlist[i], 16);
                        strResult.Append((char)charCode);
                    }
                }
                catch
                {
                    return str;
                }
                //catch (FormatException ex)
                //{
                //    return Regex.Unescape(str);
                //}
            }
            return strResult.ToString();
        }
        public static string ReplaceDotAll(string text)
        {
            return text.Replace("'", "").Replace("\"", "");
        }
        public static string GetSafeCode(string text, bool convertSpace = false)
        {
            return text.Replace("<", "[").Replace(">", "]");
            if (!convertSpace)
                return text.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br>").Replace("\n", "<br>");
            else
                return text.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br>").Replace("\n", "<br>").Replace(" ", "&nbsp;");
        }
        public static string GetSafeCodeReverse(string text)
        {
            return text.Replace("&lt;", "<").Replace("&gt;", ">").Replace("<br>", "\r\n").Replace("&nbsp;", " ");
        }
        public static string GetSafeCodeRemove(string text)
        {
            return text.Replace("<", "").Replace(">", "").Replace("\r\n", "").Replace("\n", "").Replace("&", "");
        }
        public static string GetSafeCodeRemoveAll(string text)
        {
            return text.Replace("'", "").Replace("\"", "").Replace("<", "").Replace(">", "").Replace("\r\n", "").Replace("\n", "").Replace("&", "").Replace(",", "").Replace(";", "").Replace(":", "");
        }
        public static string CSVConvert(string s)
        {
            Regex r = new Regex(@"<[^>]*>");
            MatchCollection mc = r.Matches(s);
            foreach (Match m in mc)
            {
                s = s.Replace(m.Value, "");
            }
            s = s.Replace("\r", " ").Replace("\n", " ").Replace("\"", "\"\"");
            s = "\"" + s + "\"";
            return s;
        }
        public static string HTML2Text(string html)
        {
            string regexstr = @"<[^>]*>";    //去除所有的标签

            //@"<script[^>]*?>.*?</script>" //去除所有脚本，中间部分也删除

            // string regexstr = @"<img[^>]*>";   //去除图片的正则

            // string regexstr = @"<(?!br).*?>";   //去除所有标签，只剩br

            // string regexstr = @"<table[^>]*?>.*?</table>";   //去除table里面的所有内容

            //string regexstr = @"<(?!img|br|p|/p).*?>";   //去除所有标签，只剩img,br,p

            return Regex.Replace(html, regexstr, string.Empty, RegexOptions.IgnoreCase).Replace("&nbsp;", "").Trim();
        }
        public static string FormPostResultJs(string str, string sucjs)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language='javascript'>");
            if (str == null) sb.Append("parent._loading2suc(" + (sucjs == null ? "" : ("\"" + sucjs + "\"")) + ")");
            else sb.Append("parent._loading2fai(\"" + str + "\")");
            sb.Append("</script>");
            return sb.ToString();
        }
        public static string UrlHost()
        {
            return FTFrame.FTHttpContext.Current.Request.Host.Host;
        }
        public static string UserALink(string UserLinkName, string UserID)
        {
            return "<a href=\"../main/user_bs_" + UserID + ".aspx\" target=\"_blank\">" + UserLinkName + "</a>";
        }
        public static string UserMLink(string UserLinkName, string UserID)
        {
            return "<a href=\"../main/user_more_bs_" + UserID + ".aspx\" target=\"_blank\">" + UserLinkName + "</a>";
        }
        public static string GroupALink(string GroupLinkName, string GroupID)
        {
            return "<a href=\"../main/group_view_bs_" + GroupID + ".aspx\" target=\"_blank\">" + GroupLinkName + "</a>";
        }
        public static string MessageTypeCnName(Enums.MessageType mtype)
        {
            switch (mtype)
            {
                case Enums.MessageType.ALL: return "全部";
                case Enums.MessageType.NORMAL: return "一般";
                case Enums.MessageType.TASK_ALERT: return "预警";
                case Enums.MessageType.TASK_REMIND: return "提醒";
            }
            return "未知";
        }
        public static string CusStr(string str, int length)
        {
            if (str.Length > length) str = str.Substring(0, length - 1) + "...";
            return str;
        }
        public static bool SQLSelectSafe(string s)
        {
            s = s.ToLower();
            return s.IndexOf("insert") < 0 && s.IndexOf("delete") < 0 && s.IndexOf("update") < 0 && s.IndexOf("drop") < 0 && s.IndexOf("alter") < 0 && s.IndexOf("create") < 0 && s.IndexOf("exec") < 0 && s.IndexOf("truncate") < 0;
        }
        public static string SqlBuilder_Insert(string TableName, object[] SetObj)
        {
            string sql = "insert into " + TableName + "(";
            string cols = "";
            string vals = "";
            foreach (object[] item in SetObj)
            {
                if (!cols.Equals("")) cols += ",";
                cols += item[0].ToString();
                if (!vals.Equals("")) vals += ",";
                if (item[1] == null) vals += "null";
                else
                {
                    switch (item[1].GetType().Name.ToLower())
                    {
                        case "int":
                        case "int16":
                        case "int32":
                        case "int64":
                        case "decimal":
                        case "float":
                        case "long":
                        case "double":
                            vals += item[1].ToString();
                            break;
                        default:
                            vals += "'" + str.D2DD(item[1].ToString()) + "'";
                            break;
                    }
                }
            }
            sql += cols + ")values(" + vals + ")";
            return sql;
        }
        public static string SqlBuilder_Update(string TableName, object[] SetObj, string WhereCdn)
        {
            string sql = "";
            foreach (object[] item in SetObj)
            {
                if (!sql.Equals("")) sql += ",";
                sql += item[0].ToString() + "=";
                if (item[1] == null) sql += "null";
                {
                    switch (item[1].GetType().Name.ToLower())
                    {
                        case "int":
                        case "int16":
                        case "int32":
                        case "int64":
                        case "decimal":
                        case "float":
                        case "long":
                        case "double":
                            sql += item[1].ToString();
                            break;
                        default:
                            sql += "'" + str.D2DD(item[1].ToString()) + "'";
                            break;
                    }
                }
            }
            sql = "update " + TableName + " set " + sql + " " + WhereCdn;
            return sql;
        }
        public static string GetDateTimeParse(DateTime now, DateTime tar)
        {
            string appendstr = "前";
            TimeSpan ts = now.Subtract(tar);
            if (ts.TotalSeconds < 0)
            {
                ts = tar.Subtract(now);
                appendstr = "后";
            }
            string restr = "";
            if (ts.Days > 1) restr = str.GetDateTime(tar);
            else
            {
                if (ts.Days > 0) restr += ts.Days + "天";
                if (ts.Days > 0 || ts.Hours > 0) restr += ts.Hours + "小时";
                if (ts.Days > 0 || ts.Hours > 0 || ts.Minutes > 0) restr += ts.Minutes + "分钟";
                restr += appendstr;
                if (ts.Days == 0 && ts.Hours == 0 && ts.Minutes == 0) restr = "1分钟内";
            }
            return restr;
        }
        public static string GetDateTimeHold(DateTime bigdate, DateTime smalldate)
        {
            TimeSpan ts = bigdate.Subtract(smalldate);
            string restr = "";
            if (ts.Days > 0) restr += ts.Days + "天";
            if (ts.Days > 0 || ts.Hours > 0) restr += ts.Hours + "小时";
            if (ts.Days > 0 || ts.Hours > 0 || ts.Minutes > 0) restr += ts.Minutes + "分钟";
            if (ts.Days == 0 && ts.Hours == 0 && ts.Minutes == 0) restr = "1分钟内";
            return restr;
        }
        /// <summary>
        /// 四个日期取区间共有部分
        /// </summary>
        /// <param name="dt1start"></param>
        /// <param name="dt1end"></param>
        /// <param name="dt2start"></param>
        /// <param name="dt2end"></param>
        /// <returns></returns>
        public static DateTime[] GetDateTimeSubMin(DateTime dt1start, DateTime dt1end, DateTime dt2start, DateTime dt2end)
        {
            if (dt1end < dt1start || dt2end < dt2start) return null;
            if (dt1start <= dt2start)
            {
                if (dt1end >= dt2start)
                {
                    if (dt1end <= dt2end)
                    {
                        return new DateTime[] { dt2start, dt1end };
                    }
                    else
                    {
                        return new DateTime[] { dt2start, dt2end };
                    }
                }
                else
                {
                    return null;
                }
            }
            else if (dt1start <= dt2end)
            {
                if (dt1end <= dt2end)
                {
                    return new DateTime[] { dt1start, dt1end };
                }
                else
                {
                    return new DateTime[] { dt1start, dt2end };
                }
            }
            else return null;
        }
        public static IPAddress GetIPAddress()
        {
            return FTFrame.FTHttpContext.Current?.Connection.RemoteIpAddress;
        }
        public static string GetIP()
        {
            return FTFrame.FTHttpContext.Current?.Connection.RemoteIpAddress.ToString();
        }

        public static string GetCombID()
        {
            return GenerateComb().ToString().Replace("-", "_");
        }
        public static string GetGuidID()
        {
            //"g5pi3_ps1fi_hy2d_223552";
            return System.Guid.NewGuid().ToString().Replace("-", "_");
        }
        /**/
        /// <summary>
        /**/
        /// Generate a new <see cref="Guid"/> using the comb algorithm.
        /**/
        /// </summary>
        private static Guid GenerateComb()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build 
            //the byte string 
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;

            // Convert to a byte array   
            // Note that SQL Server is accurate to 1/300th of a 
            // millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);
            return new Guid(guidArray);
        }

        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AesEncrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            String key = "^#@$Fmaobb$%SDF@#maobb234efwexYz";
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string AesDecrypt_Fit(string str)
        {
            try
            {
                return AesDecrypt(str)??"";
            }
            catch
            {
                return GetDecode(str)??"";
            }
        }
        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AesDecrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            String key = "^#@$Fmaobb$%SDF@#maobb234efwexYz";
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
        public static string GetEncode(object str)
        {
            return GetEncode(str.ToString());
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetEncode(string str)
        {
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "^#@$FVSD#$%SDF@#maobb234efwe";

            String sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);

            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength);
            }
            byte[] kkkk = ASCIIEncoding.ASCII.GetBytes(sTemp);


            sTemp = "Dmaobbasfui23497#$ASasdkf0";
            mobjCryptoService.GenerateIV();
            bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength);
            }
            byte[] vvvv = ASCIIEncoding.ASCII.GetBytes(sTemp);

            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(str.ToString());
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = kkkk;
            mobjCryptoService.IV = vvvv;
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetDecode(string str)
        {
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "^#@$FVSD#$%SDF@#maobb234efwe";
            String sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);
            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength);
            }

            byte[] kkkk = ASCIIEncoding.ASCII.GetBytes(sTemp);
            sTemp = "Dmaobbasfui23497#$ASasdkf0";
            mobjCryptoService.GenerateIV();
            bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength);
            }
            byte[] vvvv = ASCIIEncoding.ASCII.GetBytes(sTemp);

            byte[] bytIn = Convert.FromBase64String(str);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = kkkk;
            mobjCryptoService.IV = vvvv;
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetEncode_L2(string str)
        {
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "^#bgjodlp$332xbx34efwe1";

            String sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);

            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength);
            }
            byte[] kkkk = ASCIIEncoding.ASCII.GetBytes(sTemp);


            sTemp = "kfo$6fjf7&krn%339dNfd";
            mobjCryptoService.GenerateIV();
            bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength);
            }
            byte[] vvvv = ASCIIEncoding.ASCII.GetBytes(sTemp);

            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(str.ToString());
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = kkkk;
            mobjCryptoService.IV = vvvv;
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetDecode_L2(string str)
        {
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "^#bgjodlp$332xbx34efwe1";
            String sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);
            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength);
            }

            byte[] kkkk = ASCIIEncoding.ASCII.GetBytes(sTemp);
            sTemp = "kfo$6fjf7&krn%339dNfd";
            mobjCryptoService.GenerateIV();
            bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength);
            }
            byte[] vvvv = ASCIIEncoding.ASCII.GetBytes(sTemp);

            byte[] bytIn = Convert.FromBase64String(str);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = kkkk;
            mobjCryptoService.IV = vvvv;
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
        public static string GetMD5(string str)
        {
            byte[] dataToHash = new ASCIIEncoding().GetBytes(str);
            byte[] hashvalue = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(dataToHash);
            int i;
            string md5 = "";
            for (i = 0; i <= 15; i++)
            {
                md5 += Microsoft.VisualBasic.Conversion.Hex(hashvalue[i]).ToLower();
            }
            return md5;
        }
        public static string getMD5(string str)
        {
            byte[] sor = Encoding.UTF8.GetBytes(str);
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(sor);
            StringBuilder strbul = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

            }
            return strbul.ToString();
        }

        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str">明文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string AesEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str">密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string AesDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
        public static string CmycurD(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字 
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
            string str3 = "";    //从原num值中取出的值 
            string str4 = "";    //数字的字符串形式 
            string str5 = "";  //人民币大写金额形式 
            int i;    //循环变量 
            int j;    //num的值乘以100的字符串长度 
            string ch1 = "";    //数字的汉语读法 
            string ch2 = "";    //数字位的汉字读法 
            int nzero = 0;  //用来计算连续的零值是几个 
            int temp;            //从原num值中取出的值 

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数 
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式 
            j = str4.Length;      //找出最高位 
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以tr2=佰拾元角分 

            //循环取出每一位需要转换的值 
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值 
                temp = Convert.ToInt32(str3);      //转换为数字 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整” 
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }


    }
}
