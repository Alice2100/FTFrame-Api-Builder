using FTFrame.DBClient;
using FTFrame.Project.Core.Utils;
using FTFrame.Tool;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Xml;
using CoreHttp = Microsoft.AspNetCore.Http;

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
            if (string.IsNullOrEmpty(loginInfo.userId)) return null;
            return loginInfo.userId;
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
            else return reqDic["_userinfo_"] == null ? null : ((LoginInfo)reqDic["_userinfo_"]).userId;
        }
        public static LoginInfo UserInfo(Dictionary<string, object> reqDic)
        {
            if (reqDic == null || !reqDic.ContainsKey("_userinfo_")) return new LoginInfo();
            else return reqDic["_userinfo_"] == null ? null : ((LoginInfo)reqDic["_userinfo_"]);
        }
        public static UserInfo UserInfoObj()
        {
            var loginInfo = new LoginInfo();
            return loginInfo.userInfo;
        }
        public static UserInfo UserInfoObj(Dictionary<string, object> reqDic)
        {
            if (reqDic == null || !reqDic.ContainsKey("_userinfo_")) return UserInfoObj();
            else return reqDic["_userinfo_"] == null ? null : ((LoginInfo)reqDic["_userinfo_"]).userInfo;
        }
        public static IEnumerable<Claim> UserClaims()
        {
            return new LoginInfo().UserClaims;
        }
        public static IEnumerable<Claim> UserClaims(Dictionary<string, object> reqDic)
        {
            if (reqDic == null || !reqDic.ContainsKey("_userinfo_"))
            {
                return new LoginInfo().UserClaims;
            }
            else return reqDic["_userinfo_"] == null ? null : ((LoginInfo)reqDic["_userinfo_"]).UserClaims;
        }
        public static LoginInfo GetLoginInfo()
        {
            return new LoginInfo();
        }
        public static string UserName()
        {
            var loginInfo = new LoginInfo();
            if (string.IsNullOrEmpty(loginInfo.userId)) return null;
            return loginInfo.userInfo.UserName;
        }
        public static string UserShowName(string UserId)
        {
            var loginInfo = new LoginInfo();
            if (string.IsNullOrEmpty(loginInfo.userId)) return null;
            return loginInfo.userInfo.NickName;
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
        public IEnumerable<Claim> UserClaims { get; set; }

        public string userId { get; set; }
        public UserInfo userInfo { get; set; } = null;
        public List<String> permissionsList { get; set; }
        public List<String> rolesList { get; set; }
        public string error { get; set; }
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
            var token = context.Request.Headers["token"].FirstOrDefault() ?? context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(token))
            {
                error = "没有获取到token";
                return;
            }
            if (Consts.IsForDev)
            {
                //测试时可使用ftdp
                if (token == "ftdp")
                {
                    this.userId = "1";
                    this.permissionsList = new List<string>
                {
                    "*:*:*"
                };
                    this.rolesList = new List<string>
                {
                    "admin"
                };
                    this.UserClaims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim("userType","0"),
                };
                    this.userInfo = new UserInfo()
                    {
                        UserId = 1,
                        NickName = "admin",
                        UserName = "admin",
                        UserType = 0,
                    };
                    return;
                }
                else
                {
                    var userid = -1L;
                    if (long.TryParse(token, out userid))
                    {
                        using (DB db = DBSuit.DBReadOnly())
                        {
                            var sql = $"select * from act_user where fid={userid}";
                            using (var dr = db.OpenRecord(sql))
                            {
                                if (dr.Read())
                                {
                                    this.UserClaims = new[] {
                                        new Claim(ClaimTypes.NameIdentifier, userid.ToString()),
                                        new Claim(ClaimTypes.Name, dr.GetStringNoNULL("username")),
                                        new Claim("userType",dr.GetInt16("usertype").ToString()),
                                        new Claim("bindDemander", dr.GetInt64("bind_demander").ToString()),
                                        new Claim("bindSupplier", dr.GetInt64("bind_supplier").ToString()),
                                    };
                                    this.userId = userid.ToString();
                                    this.userInfo = new UserInfo()
                                    {
                                        UserId = userid,
                                        NickName = dr.GetStringNoNULL("nickname"),
                                        UserName = dr.GetStringNoNULL("username"),
                                        UserType = dr.GetInt16("usertype"),
                                    };
                                    this.permissionsList = new List<string>
                                    {
                                        "*:*:*"
                                    };
                                    return;
                                }
                                return;
                            }
                        }
                    }
                }
            }
            try
            {
                var ret = Jwt.Validate(token);
                if (ret.exception != null)
                {
                    error = ret.exception;
                }
                else
                {
                    if (ret.claimsPrincipal != null)
                    {
                        this.userId = ret.claimsPrincipal.Claims.Where(r => r.Type == ClaimTypes.NameIdentifier).First().Value;
                        this.UserClaims = ret.claimsPrincipal.Claims;
                        this.permissionsList = new List<string>
                        {
                            "*:*:*"
                        };
                        var userType = int.Parse(ret.claimsPrincipal.Claims.Where(r => r.Type == "userType").First().Value);
                        this.userInfo = new UserInfo()
                        {
                            UserId = long.Parse(this.userId),
                            NickName = ret.claimsPrincipal.Claims.Where(r => r.Type == "nickname").First().Value,
                            UserName = ret.claimsPrincipal.Claims.Where(r => r.Type == ClaimTypes.Name).First().Value,
                            UserType = userType,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }
    }

    public class UserInfo
    {
        public long UserId { get; set; } = -1;
        public string UserName { get; set; }
        public string NickName { get; set; }
        public int UserType { get; set; } = -1;
    }
}
