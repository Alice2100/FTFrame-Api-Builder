using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FTFrame.Base.Core;
using FTFrame.DBClient;
using FTFrame.Tool;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FTFrame.Web.Pages._frame
{
    public class loginModel : PageModel
    {
        public void OnGet([FromQuery] string logout)
        {
            if (!string.IsNullOrWhiteSpace(logout) && logout == "true")
            {
                LoginOut(Response);
                Response.Redirect("login", true);
                return;
            }
            if (UserTool.IsLogin())
            {
                Response.Redirect("index", true);
                return;
            }
        }
        public string ResultMsg = null;
        public void OnPost([FromQuery] string path)
        {
            if (Request.HasFormContentType)
            {
                string loginStat = Login(Request, Response);
                if (loginStat == null)
                {
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        Response.Redirect("index", true);
                    }
                    else
                    {
                        Response.Redirect(path, true);
                    }
                }
                else
                {
                    if (path == null) path = "";
                    ResultMsg = loginStat;
                }
            }
        }
        public string GetPath()
        {
            string path = Request.Query["path"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(path)) return "";
            else return "?path=" + HttpUtility.UrlEncode(path);
        }
        private string Login(HttpRequest request, HttpResponse response)
        {
            string username = request.Form["username"];
            string password = request.Form["password"];
            username = username.Trim();
            using (DB db = new DB(SysConst.ConnectionStr_FTDP))
            {
                db.Open();
                string sql = "select count(*) ca from ft_sites where sysuser='" + str.D2DD(username) + "' and syspasswd='" + str.D2DD(password) + "' and locked='0'";
                if (db.GetInt(sql) == 0)
                {
                    return "Incorrect username or password";
                }
                else
                {
                    Obj.User userinfo = new Obj.User();
                    userinfo.UserID = username;
                    userinfo.UserName = username;
                    userinfo.RealName = username;
                    userinfo.LoginTime = DateTime.Now;
                    userinfo.UserType = Enums.UserType.Normal;
                    session.Set<Obj.User>("UserInfo", userinfo);
                    return null;
                }
            }

        }
        private void LoginOut(HttpResponse response)
        {
            try
            {
                if (UserTool.IsLogin())
                {
                    session.Remove("UserInfo");
                }
            }
            catch (Exception ex) { log.Error(ex); }
        }
    }
}
