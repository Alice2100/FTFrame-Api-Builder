using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using FTFrame;
using FTFrame.DBClient;
using FTFrame.Tool;
using System.Collections;
using Microsoft.AspNetCore.Http;

namespace FTFrame.Base.Core
{
    public class UserBaseForApiDevelop
    {
        public string Login(HttpRequest request, HttpResponse response)
        {
            string username = request.Form["username"];
            string password = request.Form["password"];
            username = username.Trim();
            using (DB db = new DB(SysConst.ConnectionStr_FTDP))
            {
                db.Open();
                string sql = "select count(*) ca from ft_sites where sysuser='" + str.D2DD(username) + "' and syspasswd='" + str.D2DD(password) + "' and locked='0'";
                if(db.GetInt(sql)==0)
                {
                    return "Incorrect username or password";
                }
                else
                {
                    Obj.User userinfo = new FTFrame.Obj.User();
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
        public void LoginOut(HttpResponse response)
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
    public class UserBase
    {
        public void LoginInit(HttpRequest request, HttpResponse response)
        {
            //保持登录应用
            if (!UserTool.IsLogin())
            {
                try
                {
                    var usernameCookie = request.Cookies.ContainsKey("AHNBYU6H8M2GZ") ? request.Cookies["AHNBYU6H8M2GZ"] : null;
                    var passwordCookie = request.Cookies.ContainsKey("ZOPMNKJ784NBA") ? request.Cookies["ZOPMNKJ784NBA"] : null;
                    if (!string.IsNullOrEmpty(usernameCookie) && !string.IsNullOrEmpty(passwordCookie))
                    {
                        string username = str.GetDecode_L2(usernameCookie);
                        string password = str.GetDecode_L2(passwordCookie);
                        LoginBase(username, password, response);
                    }
                }
                catch { }
            }
        }
        public string Login(HttpRequest request, HttpResponse response)
        {
            //验证码，10分钟内两次或以上登录的将进行验证码验证
            if (!string.IsNullOrEmpty(session.GetString("LastEnterTime")))
            {
                TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime.Parse(session.GetString("LastEnterTime")).Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                int passMinutes = Convert.ToInt32(ts.TotalMinutes);
                if (passMinutes < SysConst.ValidateShowTime)
                {
                    string valicode = request.Form["valicode"];
                    if (valicode == null || !valicode.ToLower().Equals(session.GetString("Validate").ToLower()))
                        return "验证码输入错误，请重新登录";
                }
            }
            string username = request.Form["username"];
            string password = request.Form["password"];
            //以下在前端密码加密时会用到
            //password = Microsoft.JScript.GlobalObject.unescape(password);
            //string c = ((char)(((int)password[0]) - password.Length)).ToString();
            //for (int j = 1; j < password.Length; j++)
            //{
            //    c += ((char)(((int)password[j]) - ((int)c[j - 1]))).ToString();
            //}
            //password = c;
            username = username.Trim();
            string returnValue = LoginBase(username, password, response);
            if (returnValue == null)
            {
                //cookie，设置保持登录
                try
                {
                    response.Cookies.Delete("AHNBYU6H8M2GZ");
                    response.Cookies.Delete("ZOPMNKJ784NBA");
                    string staylogin = request.Form["staylogin"];
                    if (staylogin != null && staylogin.Equals("1"))
                    {
                        response.Cookies.Append("AHNBYU6H8M2GZ", str.GetEncode_L2(username), new CookieOptions() { Expires = DateTime.Now.AddYears(1) });
                        response.Cookies.Append("ZOPMNKJ784NBA", str.GetEncode_L2(password), new CookieOptions() { Expires = DateTime.Now.AddYears(1) });
                    }
                }
                catch (Exception ex) { log.Error(ex); }
            }
            return returnValue;
        }
        public string LoginBase(string username, string password, HttpResponse response)
        {
            if (username.Equals(""))
                return "用户名不能为空";
            if (username.Length > 32)
                return "用户名长度不能超过32个字符";
            if (password.Equals(""))
                return "密码不能为空";
            if (password.Length > 32)
                return "密码长度不能超过32个字符";
            string returnValue = null;
            string userID = null;
            string realName = null;
            DateTime regdate = DateTime.Now;
            int stat = 0;
            int logincount = 0;
            DB db = new DB(SysConst.ConnectionStr_FTDP);
            db.Open();
            string sql = "select userid,realname,stat,logincount from tb_userinfo where username='" + str.D2DD(username) + "' and password='" + str.D2DD(str.GetEncode(username + password)) + "' and stat>0";
            using (var dr = db.OpenRecord(sql))
            {
                if (!dr.HasRows) returnValue = "用户名或密码错误，请重新登录";
                else
                {
                    dr.Read();
                    stat = dr.GetInt16("STAT");
                    if (stat == 2) returnValue = "该账号已经被冻结，请与系统管理员联系";
                    else
                    {
                        userID = dr.GetString("USERID");
                        realName = dr.GetString("REALNAME");
                        logincount = dr.GetInt32("LOGINCOUNT");
                    }
                }
            }
            if (returnValue != null && stat == 0)//用户名、密码输错情况
            {
                //记录错误的用户名和密码
                sql = "insert into ur_login_error(username,password,logintime,ip)values('" + str.D2DD(username) + "','" + str.D2DD(password) + "','" + str.GetDateTimeMil() + "','" + str.GetIP() + "')";
                db.ExecSql(sql);
            }
            db.Close();
            if (returnValue != null) return returnValue;
            //登录
            if (returnValue == null)
            {
                db = new DB(SysConst.ConnectionStr_FTDP);
                db.Open();
                try
                {
                    //先注销
                    //LoginOut("重新登录而注销", "重新登录而注销", response);
                    //logintye:1为成功的登录，2为注销退出，3为失败的登录
                    //Icon
                    /*
                    sql = "select TOUXIANG from TB_USER_MORE where USERID='"+str.D2DD(userID)+"'";
                    string Icon = db.GetString(sql);
                    if (Icon == null || Icon.Trim().Equals("")) Icon = "../image/icon/user_100-100.gif";*/
                    Obj.User userinfo = new FTFrame.Obj.User();
                    userinfo.UserID = userID;
                    userinfo.UserName = username;
                    userinfo.RealName = realName;
                    userinfo.LoginTime = DateTime.Now;
                    userinfo.LoginCount = logincount + 1;
                    if (userID.Equals("0") && username.Equals("admin"))
                    {
                        userinfo.UserType = Enums.UserType.Admin;
                        session.SetString("AdminLogin", "True");
                    }
                    else
                    {
                        userinfo.UserType = Enums.UserType.Normal;
                    }
                    session.Set<Obj.User>("UserInfo", userinfo);
                    Stat.PutUser(userinfo.UserID, DateTime.Now);
                    sql = "insert into ur_login_stat(userid,logintime,logintype,logindes,comment,staytime,actionip)";
                    sql += "values('" + userID + "','" + str.GetDateTimeMil() + "',1,'Normal Login','Normal Login',-1,'" + str.GetIP() + "')";
                    db.ExecSql(sql);
                    sql = "update tb_userinfo set logincount=logincount+1 where userid='" + userID + "'";
                    db.ExecSql(sql);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    returnValue = "System Error，请重新操作";
                }
                finally
                {
                    db.Close();
                }
            }
            return returnValue;
        }
        public void LoginOut(HttpResponse response)
        {
            try
            {
                if (UserTool.IsLogin())
                {
                    string userID = UserTool.CurUser().UserID;
                    TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
                    TimeSpan ts2 = new TimeSpan(UserTool.CurUser().LoginTime.Ticks);
                    TimeSpan ts = ts1.Subtract(ts2).Duration();
                    int passMinutes = Convert.ToInt32(ts.TotalMinutes);
                    string sql = "insert into ur_login_stat(userid,logintime,logintype,logindes,comment,staytime,actionip)";
                    sql += "values('" + str.D2DD(userID) + "','" + str.GetDateTimeMil() + "',2,'Normal Logout','Normal Logout'," + passMinutes + ",'" + str.GetIP() + "')";
                    DB.ExecSQL(sql, SysConst.ConnectionStr_FTDP);
                    Stat.GetUser(UserTool.CurUserID());
                    session.Remove("UserInfo");
                    session.Remove("AdminLogin");
                    //session.Remove("Right_Page");
                    //session.Remove("Right_OP");
                    //session.Remove("UserSet");
                    //手动注销需要清除自动登录状态
                    response.Cookies.Delete("AHNBYU6H8M2GZ");
                    response.Cookies.Delete("ZOPMNKJ784NBA");
                }
            }
            catch (Exception ex) { log.Error(ex); }
        }
        public static Hashtable UserSetGet()
        {
            string UserID = UserTool.CurUserID();
            if (UserID == null) return new Hashtable();
            if (session.Get<Hashtable>("UserSet") == null) return new Hashtable();
            return session.Get<Hashtable>("UserSet");
        }
        public static void UserSetInit(DB db)
        {
            string UserID = UserTool.CurUserID();
            if (UserID == null) return;
            Hashtable ht = new Hashtable();
            bool IsDbNull = db == null;
            if (IsDbNull)
            {
                db = new DB(SysConst.ConnectionStr_FTDP);
                db.Open();
            }
            try
            {
                string sql = "select settag,setval from tb_userset where userid='" + str.D2DD(UserID) + "'";
                using (DR dr = new DR(db, sql))
                {
                    while (dr.Read())
                    {
                        ht.Add(dr.GetString(0), dr.GetStringNoNULL(1));
                    }
                }
                session.Set<Hashtable>("UserSet",ht);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                if (IsDbNull) db.Close();
            }
        }
    }
}
