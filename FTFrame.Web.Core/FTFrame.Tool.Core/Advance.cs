using FTFrame.DBClient;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FTFrame.Tool
{
    public class Advance
    {
        public static object DRChangeType(object value, Type convertsionType)
        {
            //判断convertsionType类型是否为泛型，因为nullable是泛型类,
            if (convertsionType.IsGenericType &&
                //判断convertsionType是否为nullable泛型类
                convertsionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return null;
                }
                else
                {
                    return Convert.ChangeType(value, convertsionType.GetGenericArguments()[0]);
                }
            }
            return Convert.ChangeType(value, convertsionType);
        }

        public static JObject GetJObject(HttpRequest req)
        {
            try
            {
                JObject jObject = null;
                req.EnableBuffering();
                using (var ms = new MemoryStream())
                {
                    req.Body.Position = 0;
                    req.Body.CopyTo(ms);
                    var buffer = ms.ToArray();
                    string content = Encoding.UTF8.GetString(buffer);
                    if (string.IsNullOrWhiteSpace(content)) content = "{}";
                    jObject = JObject.Parse(content);
                }
                return jObject;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                return JObject.Parse("{}");
            }
        }
        public static string GetLiquidID(string TableName, string ColName, string PattenStr, string LockLikeStr)
        {
            PattenStr = PattenStr.Trim();
            LockLikeStr = LockLikeStr.Trim();
            DateTime dt = DateTime.Now;
            PattenStr = str.GetDateTimeCustom(dt, PattenStr.Replace("[y]", "yy").Replace("[Y]", "yyyy").Replace("[M]", "MM").Replace("[D]", "dd").Replace("[h]", "HH").Replace("[m]", "mm").Replace("[s]", "ss"));
            if (!LockLikeStr.Equals(""))
            {
                LockLikeStr = str.GetDateTimeCustom(dt, LockLikeStr.Replace("[y]", "yy").Replace("[Y]", "yyyy").Replace("[M]", "MM").Replace("[D]", "dd").Replace("[h]", "HH").Replace("[m]", "mm").Replace("[s]", "ss").Replace("%", "&&")).Replace("&&", "%");
            }
            Regex r = new Regex(@"\[N\(\d\)\]");
            Match m = r.Match(PattenStr);
            if (!m.Success) return "[error]no[N]";
            string RateN = m.Value;
            int RateI = m.Index;
            int RateLengh = int.Parse(RateN.Substring(RateN.IndexOf('(') + 1, RateN.IndexOf(')') - RateN.IndexOf('(') - 1));
            DB db = new DB();
            db.Open();
            try
            {
                string sql = null;
                switch (SysConst.DataBaseType)
                {
                    case DataBase.MySql:
                        if (LockLikeStr.Equals(""))
                        {
                            sql = "select SUBSTRING(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  order by  rate desc";
                        }
                        else
                        {
                            sql = "select SUBSTRING(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  where " + str.D2DD(ColName) + " like '" + str.D2DD(LockLikeStr) + "' order by  rate desc";
                        }
                        break;
                    case DataBase.SqlServer:
                        if (LockLikeStr.Equals(""))
                        {
                            sql = "select SUBSTRING(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  order by  rate desc";
                        }
                        else
                        {
                            sql = "select SUBSTRING(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  where " + str.D2DD(ColName) + " like '" + str.D2DD(LockLikeStr) + "' order by  rate desc";
                        }
                        break;
                    case DataBase.Sqlite:
                        if (LockLikeStr.Equals(""))
                        {
                            sql = "select substr(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  order by  rate desc";
                        }
                        else
                        {
                            sql = "select substr(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  where " + str.D2DD(ColName) + " like '" + str.D2DD(LockLikeStr) + "' order by  rate desc";
                        }
                        break;
                }
                int MaxID = 1;
                object v = db.GetObject(sql);
                if (v != null)
                {
                    if (int.TryParse(v.ToString(), out MaxID))
                    {
                        MaxID += 1;
                    }
                    else
                    {
                        MaxID = 1;
                    }
                }
                if (MaxID >= Math.Pow(10, RateLengh))
                {
                    return "[error]max" + RateLengh;
                }
                string NewID = MaxID.ToString();
                for (int i = 0; i < RateLengh - MaxID.ToString().Length; i++)
                {
                    NewID = "0" + NewID;
                }
                NewID = PattenStr.Replace(RateN, NewID);
                return NewID;
            }
            catch
            {
                return "[error]ex";
            }
            finally { db.Close(); }
        }
        public static bool IsLiquidIDOK(string TableName, string ColName, string LiquidID)
        {
            if (LiquidID.StartsWith("[error]")) return true;
            DB db = new DB();
            db.Open();
            try
            {
                string sql = "select count(*) as ca from " + str.D2DD(TableName) + " where " + str.D2DD(ColName) + "='" + str.D2DD(LiquidID) + "'";
                return db.GetInt(sql) == 0;
            }
            catch
            {
                return true;
            }
            finally { db.Close(); }
        }
        public static string GetLiquidID(DB db, ST st, string TableName, string ColName, string PattenStr, string LockLikeStr)
        {
            PattenStr = PattenStr.Trim();
            LockLikeStr = LockLikeStr.Trim();
            DateTime dt = DateTime.Now;
            PattenStr = str.GetDateTimeCustom(dt, PattenStr.Replace("[y]", "yy").Replace("[Y]", "yyyy").Replace("[M]", "MM").Replace("[D]", "dd").Replace("[h]", "HH").Replace("[m]", "mm").Replace("[s]", "ss"));
            if (!LockLikeStr.Equals(""))
            {
                LockLikeStr = str.GetDateTimeCustom(dt, LockLikeStr.Replace("[y]", "yy").Replace("[Y]", "yyyy").Replace("[M]", "MM").Replace("[D]", "dd").Replace("[h]", "HH").Replace("[m]", "mm").Replace("[s]", "ss").Replace("%", "&&")).Replace("&&", "%");
            }
            Regex r = new Regex(@"\[N\(\d\)\]");
            Match m = r.Match(PattenStr);
            if (!m.Success) return "[error]no[N]";
            string RateN = m.Value;
            int RateI = m.Index;
            int RateLengh = int.Parse(RateN.Substring(RateN.IndexOf('(') + 1, RateN.IndexOf(')') - RateN.IndexOf('(') - 1));
            try
            {
                string sql = null;
                switch (SysConst.DataBaseType)
                {
                    case DataBase.MySql:
                        if (LockLikeStr.Equals(""))
                        {
                            sql = "select SUBSTRING(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  order by  rate desc";
                        }
                        else
                        {
                            sql = "select SUBSTRING(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  where " + str.D2DD(ColName) + " like '" + str.D2DD(LockLikeStr) + "' order by  rate desc";
                        }
                        break;
                    case DataBase.SqlServer:
                        if (LockLikeStr.Equals(""))
                        {
                            sql = "select SUBSTRING(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  order by  rate desc";
                        }
                        else
                        {
                            sql = "select SUBSTRING(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  where " + str.D2DD(ColName) + " like '" + str.D2DD(LockLikeStr) + "' order by  rate desc";
                        }
                        break;
                    case DataBase.Sqlite:
                        if (LockLikeStr.Equals(""))
                        {
                            sql = "select substr(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  order by  rate desc";
                        }
                        else
                        {
                            sql = "select substr(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  where " + str.D2DD(ColName) + " like '" + str.D2DD(LockLikeStr) + "' order by  rate desc";
                        }
                        break;
                }
                int MaxID = 1;
                object v = null;
                using (DR dr = db.OpenRecord(sql, st))
                {
                    if (dr.Read())
                    {
                        v = dr.GetValue(0);
                    }
                }
                if (v != null)
                {
                    if (int.TryParse(v.ToString(), out MaxID))
                    {
                        MaxID += 1;
                    }
                    else
                    {
                        MaxID = 1;
                    }
                }
                if (MaxID >= Math.Pow(10, RateLengh))
                {
                    return "[error]max" + RateLengh;
                }
                string NewID = MaxID.ToString();
                for (int i = 0; i < RateLengh - MaxID.ToString().Length; i++)
                {
                    NewID = "0" + NewID;
                }
                NewID = PattenStr.Replace(RateN, NewID);
                return NewID;
            }
            catch
            {
                return "[error]ex";
            }
        }
        public static bool IsLiquidIDOK(DB db, ST st, string TableName, string ColName, string LiquidID)
        {
            if (LiquidID.StartsWith("[error]")) return true;
            try
            {
                string sql = "select count(*) as ca from " + str.D2DD(TableName) + " where " + str.D2DD(ColName) + "='" + str.D2DD(LiquidID) + "'";
                int count = 0;
                using (DR dr = db.OpenRecord(sql, st))
                {
                    if (dr.Read())
                    {
                        count = int.Parse(dr.GetValue(0).ToString());
                    }
                }
                return count == 0;
            }
            catch
            {
                return true;
            }
        }
    }
}
