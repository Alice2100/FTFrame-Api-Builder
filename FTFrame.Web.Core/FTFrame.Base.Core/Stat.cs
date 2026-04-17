using System;
using System.Collections.Generic;
using System.Text;
using FTFrame;
using FTFrame.DBClient;
using FTFrame.Tool;
using System.Collections;
namespace FTFrame.Base.Core
{
    public class Stat
    {
        private static Hashtable ActiveUserStat = new Hashtable();
        public static void SetUserStat(string UserID, Enums.UserStatType UserStat)
        {
            if (UserID == null) return;
            if (!ActiveUserStat.ContainsKey(UserID))
                ActiveUserStat.Add(UserID, UserStat);
            else
                ActiveUserStat[UserID] = UserStat;
        }
        public static Enums.UserStatType GetUserStat(string UserID)
        {
            if (UserID == null || !ActiveUserStat.ContainsKey(UserID))
                return Enums.UserStatType.OFFLINE;
            else
                return (Enums.UserStatType)ActiveUserStat[UserID];
        }
        public static void UpdateStat(string UserID)
        {
            if (IsUserOnline(UserID)) { }
            else
                SetUserStat(UserID, Enums.UserStatType.OFFLINE);
        }

        public static Hashtable OnlineUserStat = new Hashtable();
        public static bool IsUserOnline(string UserID)
        {
            return OnlineUserStat.ContainsKey(UserID);
        }
        public static void PutUser(string UserID, object logindt)
        {
            if (UserID == null || UserID.Equals("0") || UserTool.IsAdmin()) return;
            if (!OnlineUserStat.ContainsKey(UserID))
            {
                if (logindt != null)
                    OnlineUserStat.Add(UserID, new DateTime[] { DateTime.Now, (DateTime)logindt });
                else
                    OnlineUserStat.Add(UserID, new DateTime[] { DateTime.Now, DateTime.Now });
            }
            else
            {
                if (logindt == null)
                    OnlineUserStat[UserID] = new DateTime[] { DateTime.Now, ((DateTime[])OnlineUserStat[UserID])[1] };
                else
                    OnlineUserStat[UserID] = new DateTime[] { DateTime.Now, (DateTime)logindt };
            }
            //设置状态
            if (GetUserStat(UserID).Equals(Enums.UserStatType.OFFLINE))
            {
                Stat.SetUserStat(UserID, Enums.UserStatType.ONLINE);
            }
        }
        public static void GetUser(string UserID)
        {
            if (UserID == null || UserID.Equals("0")) return;
            if (OnlineUserStat.ContainsKey(UserID)) OnlineUserStat.Remove(UserID);
            //设置状态
            Stat.SetUserStat(UserID, Enums.UserStatType.OFFLINE);
        }
        public static void MoniteOnlineUser()
        {
            IDictionaryEnumerator enumerator = OnlineUserStat.GetEnumerator();
            ArrayList NeedGetUsers = new ArrayList();
            while (enumerator.MoveNext())
            {
                string ID = enumerator.Key.ToString();
                try
                {
                    DateTime[] DTS = (DateTime[])enumerator.Value;
                    TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
                    TimeSpan ts2 = new TimeSpan(DTS[0].Ticks);
                    TimeSpan ts = ts1.Subtract(ts2).Duration();
                    if (Convert.ToInt32(ts.TotalMinutes) > 9)
                    {
                        NeedGetUsers.Add(ID);
                        ts1 = new TimeSpan(DateTime.Now.Ticks);
                        ts2 = new TimeSpan(DTS[1].Ticks);
                        ts = ts1.Subtract(ts2).Duration();
                        int passMinutes = Convert.ToInt32(ts.TotalMinutes);
                        string sql = "insert into UR_LOGIN_STAT(USERID,LOGINTIME,LOGINTYPE,LOGINDES,COMMENT,STAYTIME,ACTIONIP)";
                        sql += "values('" + ID + "','" + str.GetDateTime() + "',2,'Session Timeout[s]','Session Timeout[s]'," + passMinutes + ",'')";
                        DB.ExecSQL(sql);
                    }
                }
                catch (Exception ex)
                {
                    NeedGetUsers.Add(ID);
                    log.Error(ex);
                }
            }
            foreach (string UserID in NeedGetUsers)
            {
                GetUser(UserID);
            }
            NeedGetUsers = null;
            enumerator = null;
        }
        public static void GlobalSessionEnd(object UserID, object LoginTime)
        {
            if (UserID == null || UserID.ToString().Equals("")) return;
            string userID = UserID.ToString();
            DateTime loginTime = (DateTime)LoginTime;
            TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts2 = new TimeSpan(loginTime.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            int passMinutes = Convert.ToInt32(ts.TotalMinutes);
            string sql = "insert into UR_LOGIN_STAT(USERID,LOGINTIME,LOGINTYPE,LOGINDES,COMMENT,STAYTIME,ACTIONIP)";
            sql += "values('" + userID + "','" + str.GetDateTime() + "',2,'Session Timeout[a]','Session Timeout[a]'," + passMinutes + ",'')";
            DB.ExecSQL(sql);
            GetUser(userID);
        }
        public static void LoginOutAllUser()
        {
            IDictionaryEnumerator enumerator = OnlineUserStat.GetEnumerator();
            ArrayList NeedGetUsers = new ArrayList();
            while (enumerator.MoveNext())
            {
                string ID = enumerator.Key.ToString();
                try
                {
                    DateTime[] DTS = (DateTime[])enumerator.Value;
                    TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
                    TimeSpan ts2 = new TimeSpan(DTS[1].Ticks);
                    TimeSpan ts = ts1.Subtract(ts2).Duration();
                    int passMinutes = Convert.ToInt32(ts.TotalMinutes);
                    string sql = "insert into UR_LOGIN_STAT(USERID,LOGINTIME,LOGINTYPE,LOGINDES,COMMENT,STAYTIME,ACTIONIP)";
                    sql += "values('" + ID + "','" + str.GetDateTime() + "',2,'System Shutdown','System Shutdown'," + passMinutes + ",'')";
                    DB.ExecSQL(sql);
                }
                catch (Exception ex)
                {
                    NeedGetUsers.Add(ID);
                    log.Error(ex);
                }
            }
            foreach (string UserID in NeedGetUsers)
            {
                GetUser(UserID);
            }
            NeedGetUsers = null;
            enumerator = null;
        }
    }
}
