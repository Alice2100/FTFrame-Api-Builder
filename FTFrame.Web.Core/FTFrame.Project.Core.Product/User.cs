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
using System.Security.Claims;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Newtonsoft.Json.Linq;
using System.Net;
using Ubiety.Dns.Core;

namespace FTFrame.Project.Core
{
    /// <summary>
    /// Customize user related development based on user integration environment
    /// </summary>
    public class User
    {
        public static bool IsLogin() => UserID() is not null;
        public static bool IsLogin(Dictionary<string, object> reqDic) => UserID(reqDic) is not null;
        public static bool IsAdmin()
        {
            return false;
            //return FTHttpContext.Current.User.Claims.Where(r => r.Type == ClaimTypes.Role && r.Value == "Admin").Count() > 0;
        }
        public static string UserID()
        {
            var loginInfo = new LoginInfo();
            if (string.IsNullOrEmpty(loginInfo.Id)) return null;
            return loginInfo.Id;
            //if (IsLogin())
            //{
            //    return FTHttpContext.Current.User.Claims.Where(r => r.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            //    //return UserTool.CurUserID();
            //}
            //return null;
        }
        public static string UserID(Dictionary<string, object> reqDic)
{
            if (reqDic == null || !reqDic.ContainsKey("_userinfo_")) return UserID();
            else return reqDic["_userinfo_"] == null ? null : ((LoginInfo)reqDic["_userinfo_"]).Id;
        }
        public static LoginInfo UserInfo(Dictionary<string, object> reqDic)
        {
            if (reqDic == null || !reqDic.ContainsKey("_userinfo_")) return new LoginInfo();
            else return reqDic["_userinfo_"] == null ? null : ((LoginInfo)reqDic["_userinfo_"]);
        }
        public static LoginInfo GetLoginInfo()
        {
            return new LoginInfo();
        }
        public static string UserName()
        {
            var loginInfo = new LoginInfo();
            if (string.IsNullOrEmpty(loginInfo.Id)) return null;
            return loginInfo.UserName;
        }
        public static string UserShowName(string UserId)
        {
            var loginInfo = new LoginInfo();
            if (string.IsNullOrEmpty(loginInfo.Id)) return null;
            return loginInfo.DisplayName;
        }
        public static string GetDepartNameByDepartID(string DepartId)
        {
            return DepartId;
        }
        public static string UserRealName()
        {
            if (IsLogin())
            {
                return FTHttpContext.Current.User.Claims.Where(r => r.Type == "RealName").FirstOrDefault()?.Value;
                //return UserTool.CurUser().RealName;
            }
            return null;
        }
    }
    /// <summary>
    /// Important: When integrating Api with user management systems, define the construction of user objects here
    /// </summary>
    public class LoginInfo
    {
        private HttpContext context;
        public LoginInfo()
        {
            context = FTHttpContext.Current;
            init();
        }
        public LoginInfo(HttpContext _context)
        {
            context = _context;
            init();
        }
        private void init()
        {
            // Build user object based on the tokens contained in the header.
            // For example,jwt,Asp.Net Identity,etc.
            // The following is an example of a testing Api.
            this.Id = "0";
            this.UserName = "testUser";
            this.DisplayName = "testUser";
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string ErrorContent { get; set; }
        public JObject Json { get; set; }
    }
}
