using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using FTFrame;
using FTFrame.Tool;
using FTFrame.DBClient;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FTFrame.BLL
{
    public class Base
    {
        /// <summary>
        /// 生成编号
        /// </summary>
        public static string CodeGenerate(string Tag)
        {
            try
            {
                string DateStr = DateTime.Now.ToString("yyyyMMdd");
                Tag = Tag.Trim();
                using (DB db = new DB())
                {
                    db.Open();
                    string sql = "select CurValue from Sys_Code where CodeBase='" + (Tag + DateStr).D2() + "'";
                    int CurValue = db.GetInt(sql);
                    if (CurValue > 0)
                    {
                        sql = "update Sys_Code set CurValue=CurValue+1 where  CodeBase='" + (Tag + DateStr).D2() + "'";
                        db.ExecSql(sql);
                        return Tag + DateStr + ((1000 + CurValue + 1).ToString().Substring(1));
                    }
                    else
                    {
                        sql = "delete from Sys_Code where Tag='" + Tag.D2() + "'";
                        db.ExecSql(sql);
                        sql = "insert into Sys_Code(Tag,CodeBase,CurValue)values(@Tag,@CodeBase,@CurValue)";
                        db.ExecSql(sql, new PR[] {
                        new PR("@Tag",Tag),
                        new PR("@CodeBase",Tag + DateStr),
                        new PR("@CurValue",1)
                    });
                        return Tag + DateStr + "001";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// 测试TestList
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> TestList(HttpContext context)
        {
            return new List<Dictionary<string, object>>() {
                new Dictionary<string, object>() { { "a", "1" }, { "b", "2" } },
                new Dictionary<string, object>() { { "a", "11" }, { "b", "22" } }
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Dictionary<string, object> TestDyvalue1(HttpContext context)
        {
            return new Dictionary<string, object>() { { "a", "1" }, { "b", "2" } };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Dictionary<string, object> TestDyvalue2(HttpContext context)
        {
            return new Dictionary<string, object>() { { "a", "11" }, { "b", "22" } };
        }
    }
}
