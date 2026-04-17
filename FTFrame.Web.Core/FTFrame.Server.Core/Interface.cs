using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;
using FTFrame.Server.Core.Tool;
using System.Xml;
using System.Collections;
using FTFrame.Project.Core;
using CoreHttp = Microsoft.AspNetCore.Http;
namespace FTFrame.Server.Core.Interface
{
    public class Code
    {
        public static string Get(string CodeDefine)
        {
            return Get(CodeDefine, null, null);
        }
        public static string Get(string CodeDefine, Dictionary<string, object> reqDic)
        {
            return Get(CodeDefine, reqDic, null);
        }
        public static string Get(string CodeDefine, HttpContext Context)
        {
            return Get(CodeDefine, null, Context);
        }
        public static string Get(string CodeDefine, Dictionary<string, object> reqDic, HttpContext Context)
        {
            object v = FTFrame.Project.Core.Code.Get(CodeDefine, reqDic, Context);
            return v == null ? null : v.ToString();
        }
        public static string Get(string CodeDefine, Dictionary<string, object> reqDic, Dictionary<string, string> paraDic, HttpContext Context)
        {
            object v = FTFrame.Project.Core.Code.Get(CodeDefine, reqDic, paraDic, Context);
            return v == null ? null : v.ToString();
        }
        public static string GetEnum(string EnumDefine)
        {
            object v = FTFrame.Project.Core.Code.GetEnum(EnumDefine);
            return v == null ? null : v.ToString();
        }
        public static object GetObject(string CodeDefine, HttpContext Context)
        {
            return GetObject(CodeDefine, null, Context);
        }
        public static object GetObject(string CodeDefine, Dictionary<string, object> reqDic, HttpContext Context)
        {
            return FTFrame.Project.Core.Code.Get(CodeDefine, reqDic, Context);
        }
    }
    public class Info
    {
        public static string Get(string InfoDefine, Dictionary<string, object> reqDic, HttpContext Context)
        {
            return FTFrame.Project.Core.Info.Get(InfoDefine, reqDic, Context);
        }
        public static string Get(string InfoDefine, HttpContext Context)
        {
            return Get(InfoDefine, null, Context);
        }
    }
    public class User
    {
        public static bool IsLogin()
        {
            return UserTool.IsLogin();
        }
        public static bool IsAdmin()
        {
            return UserTool.IsAdmin();
        }
        public static string UserID()
        {
            if (UserTool.IsLogin())
            {
                return UserTool.CurUserID();
            }
            return "";
        }
        public static string UserName()
        {
            if (UserTool.IsLogin())
            {
                return UserTool.CurUser().UserName;
            }
            return "";
        }
        public static string UserRealName()
        {
            if (UserTool.IsLogin())
            {
                return UserTool.CurUser().RealName;
            }
            return "";
        }
    }
    public class Right
    {
        public static bool HavePageRight(string pageurl)
        {
            return FTFrame.Project.Core.Right.HavePageRight(pageurl, FTHttpContext.Current.Request);
        }
        public static bool HaveOPRight(string opid)
        {
            return FTFrame.Project.Core.Right.HaveOPRight(opid);
        }
        public static (bool result, string noRightTip) HaveOPRight(string opid, string[] para, Dictionary<string, object> reqDic, HttpContext context)
        {
            return FTFrame.Project.Core.Right.HaveOPRight(opid, para, reqDic, context);
        }
        public static bool HaveRoleNameRight(string rolename)
        {
            return FTFrame.Project.Core.Right.HaveRoleNameRight(rolename);
        }
        public static string DyValue(HttpContext context, ArrayList Define, string SiteID)
        {
            return FTFrame.Project.Core.Right.DyValue(context, Define, SiteID);
        }
        public static string DataOP(HttpContext context)
        {
            return FTFrame.Project.Core.Right.DataOP(context);
        }
    }
    public class Timer
    {
    }
    public class Auth
    {
        public static bool HostReferOK(HttpContext context)
        {
            if (SysConst.HostReferrer == "") return true;
            string urlReferrer = context.Request.Headers["Referer"].FirstOrDefault();
            if (urlReferrer != null && urlReferrer != "" && SysConst.HostReferrer != "" && SysConst.HostReferrer.IndexOf(urlReferrer.ToLower()) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool AuthOK(HttpContext context)
        {
            return true;
        }
    }
}