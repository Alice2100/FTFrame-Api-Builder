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
using FTFrame.Project.Core;
namespace FTFrame.Base.Core
{
    public class Message
    {
        public static string Add(string UserID, string userids, string title, string content, int msgtype)
        {
            if (userids.Equals("")) return "必须选择用户";
            if (title.Equals("")) return "必须填写标题";
            if (title.Length > 80) return "标题不能超过80个字符";
            if (content.Length > 2000) return "内容不能超过2000个字符";
            DB db = new DB();
            db.Open();
            ST st = db.GetTransaction();
            try
            {
                string sql = null;
                string[] ids = userids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string id in ids)
                {
                    sql = "insert into TB_MESSAGE (USERID,FID,FRUSERID,MSGTYPE,SENDTIME,ISREAD,TITLE,DETAIL,STAT,FRSTAT,dydata)";
                    sql += "values('" + str.D2DD(id) + "','" + str.GetCombID() + "','" + str.D2DD(UserID) + "'," + msgtype + ",'" + str.GetDateTime() + "',0,'" + str.D2DD(title) + "','" + str.D2DD(content) + "',1,1,'')";
                    db.ExecSql(sql, st);
                }
                st.Commit();
                return null;
            }
            catch (Exception ex)
            {
                st.Rollback();
                log.Error(ex);
                return ex.Message;
            }
            finally
            {
                db.Close();
            }
        }
        public static string TitleShow(string fid, int msgtype, int readupdate)
        {
            DB db = new DB();
            db.Open();
            try
            {
                string sql = null;
                if (readupdate == 1)
                {
                    sql = "update TB_MESSAGE set ISREAD=1 where fid='" + str.D2DD(fid) + "'";
                    db.ExecSql(sql);
                }
                sql = "select title from TB_MESSAGE where fid='" + str.D2DD(fid) + "'";
                string title = db.GetStringForceNoNull(sql);
                return msgtype == 0 ? str.GetSafeCode(title) : title;
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
        public static string ContentShow(string fid, int msgtype)
        {
            DB db = new DB();
            db.Open();
            try
            {
                string sql = null;
                sql = "select DETAIL from TB_MESSAGE where fid='" + str.D2DD(fid) + "'";
                string content = db.GetStringForceNoNull(sql);
                return msgtype == 0 ? str.GetSafeCode(content) : content;
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
        public static string HaveNewMessage()
        {
            string UserID = UserTool.CurUserID();
            if (UserID == null) return "0";
            string sql = "select count(*) from TB_MESSAGE where STAT=1 and USERID='" + str.D2DD(UserID) + "' and ISREAD=0";
            DB db = new DB();
            db.Open();
            try
            {
                return db.GetInt(sql) > 0 ? "1" : "0";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "0";
            }
            finally
            {
                db.Close();
            }
        }
    }
}
