using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Obj;
using FTFrame;
using FTFrame.DBClient;
using System.Web;
namespace FTFrame.Tool
{
    public class UserTool
    {
        public static string CurUserID()
        {
            User userinfo = CurUser();
            if (userinfo != null)
            {
                return userinfo.UserID;
            }
            return null;
        }
        public static User CurUser()
        {
            if (FTHttpContext.Current != null && FTHttpContext.Current.Session != null && session.GetString("UserInfo") != null)
            {
                return session.Get<User>("UserInfo");
            }
            else return null;
        }
        public static bool IsCorpLogin()
        {
            return CurCorp() != null;
        }
        public static BindingCorp CurCorp()
        {
            //不是SaaS模式，则锁定CurCorp
            if (FTHttpContext.Current != null && FTHttpContext.Current.Session != null && session.GetString("Corp") != null)
            {
                return session.Get<BindingCorp>("Corp");
            }
            else return null;
        }
        public static string UserLoginCorpPath()
        {
            if (FTHttpContext.Current != null && FTHttpContext.Current.Session != null && session.GetString("user_corppath") != null)
            {
                return session.GetString("user_corppath");
            }
            else return null;
        }
        public static string CurCorpPath()
        {
            BindingCorp Corp = CurCorp();
            if (Corp != null) return Corp.Path;
            return null;
        }
        public static bool IsAdmin()
        {
            User curUser = CurUser();
            if (curUser != null && curUser.UserType.Equals(Enums.UserType.Admin)) return true;
            else
            {
                if (session.GetString("AdminLogin") != null && session.GetString("AdminLogin").Equals("True"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool IsLogin()
        {
            return (session.GetString("UserInfo") != null);
        }
        public static void Action(string CAPSTR)
        {
            Action("", "", CAPSTR);
        }
        public static void Action(string TARGETID, string CAPSTR)
        {
            Action(TARGETID, "", CAPSTR);
        }
        public static void Action(string TARGETID, string ACTIONTIP, string CAPSTR)
        {
            if (FTHttpContext.Current == null || FTHttpContext.Current.Session == null) return;
            try
            {
                string UserID = null;
                if (session.GetString("UserInfo") != null)
                {
                    UserID = session.Get<User>("UserInfo").UserID;
                }
                if (UserID != null && !UserID.Equals("0") && !UserTool.IsAdmin())
                {
                    if (TARGETID == null) TARGETID = "";
                    if (TARGETID.Length > 36) TARGETID = TARGETID.Substring(0, 36);
                    if (ACTIONTIP == null) ACTIONTIP = "";
                    if (ACTIONTIP.Length > 50) ACTIONTIP = ACTIONTIP.Substring(0, 50);
                    if (CAPSTR == null) CAPSTR = "";
                    if (CAPSTR.Length > 50) CAPSTR = CAPSTR.Substring(0, 50);
                    string sql = "insert into UR_ACTION(USERID,ACTIONTIME,TARGETID,ACTIONTIP,CAPSTR)";
                    sql += "values('" + str.D2DD(UserID) + "','" + str.GetDateTimeMil() + "','" + str.D2DD(TARGETID) + "','" + str.D2DD(ACTIONTIP) + "','" + str.D2DD(CAPSTR) + "')";
                    DB.ExecSQL(sql);
                }
            }
            catch (Exception ex) { log.Error(ex); }
        }
        public static string Icon(string UserID)
        {
            DB db = new DB();
            db.Open();
            try
            {
                string sql = "select TOUXIANG from TB_USER_MORE where USERID='" + str.D2DD(UserID) + "'";
                string Icon = db.GetString(sql);
                if (Icon == null || Icon.Trim().Equals("")) Icon = "../image/icon/user_100-100.gif";
                return Icon;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
            finally
            {
                db.Close();
            }
        }
    }
}
