using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTFrame.Tool;
using FTFrame;
using FTFrame.DBClient;
using FTFrame.Base;
using System.Web;
using System.Collections;
namespace FT.Com.WorkFlow
{
    public class Busi
    {
        public static string AfterAdd(string fid,HttpContext Context)
        {
            DB db = new DB();
            db.Open();
            try
            {
                string sql = "select max(FlowNum) from "+Const.Table.FC_WorkFlow_List+"";
                int NewFlowNum = 1;
                DR dr = db.OpenRecord(sql);
                if (dr.Read())
                {
                    if (!dr.IsDBNull(0)) NewFlowNum = dr.GetInt32(0) + 1;
                }
                dr.Close();
                sql = "update " + Const.Table.FC_WorkFlow_List + " set FlowNum=" + NewFlowNum + " where fid='"+str.D2DD(fid)+"'";
                db.execSql(sql);

                int StepID = 0;
                string rolename = Context.Request.Form["add_rolename"].Trim();
                string departid = Context.Request.Form["add_departid"].Trim();
                string[] userids = Context.Request.Form["add_userids"].Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] expectids = Context.Request.Form["add_expectids"].Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                while (!rolename.Equals("") || !departid.Equals("") || userids.Length > 0)
                {
                    sql = "insert into " + Const.Table.FC_WorkFlow_Step + "(FlowFID,StepID,RoleName,DepartID)";
                    sql += "values('" + str.D2DD(fid) + "'," + StepID + ",'" + str.D2DD(rolename) + "','" + str.D2DD(departid) + "')";
                    db.execSql(sql);
                    foreach (string userid in userids)
                    {
                        sql = "insert into " + Const.Table.FC_WorkFlow_User + "(FlowFID,StepID,UserID)";
                        sql += "values('" + str.D2DD(fid) + "'," + StepID + ",'" + str.D2DD(userid) + "')";
                        db.execSql(sql);
                    }
                    foreach (string userid in expectids)
                    {
                        sql = "insert into " + Const.Table.FC_WorkFlow_User_Except + "(FlowFID,StepID,UserID)";
                        sql += "values('" + str.D2DD(fid) + "'," + StepID + ",'" + str.D2DD(userid) + "')";
                        db.execSql(sql);
                    }
                    StepID++;
                    if (Context.Request.Form["add_rolename_rowrate" + StepID] == null) break;
                    rolename = Context.Request.Form["add_rolename_rowrate" + StepID].Trim();
                    departid = Context.Request.Form["add_departid_rowrate" + StepID].Trim();
                    userids = Context.Request.Form["add_userids_rowrate" + StepID].Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    expectids = Context.Request.Form["add_expectids_rowrate" + StepID].Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                return null;
            }
            finally
            {
                db.Close();
            }
        }
        public static string AfterMod(string fid, HttpContext Context)
        {
            DB db = new DB();
            db.Open();
            try
            {
                string sql = null;
                sql = "delete from " + Const.Table.FC_WorkFlow_Step + " where FlowFID='" + str.D2DD(fid) + "'";
                db.execSql(sql);
                sql = "delete from " + Const.Table.FC_WorkFlow_User + " where FlowFID='" + str.D2DD(fid) + "'";
                db.execSql(sql);
                sql = "delete from " + Const.Table.FC_WorkFlow_User_Except + " where FlowFID='" + str.D2DD(fid) + "'";
                db.execSql(sql);

                int StepID = 0;
                string rolename = Context.Request.Form["mod_rolename"].Trim();
                string departid = Context.Request.Form["mod_departid"].Trim();
                string[] userids = Context.Request.Form["mod_userids"].Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] expectids = Context.Request.Form["mod_expectids"].Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                while (!rolename.Equals("") || !departid.Equals("") || userids.Length > 0)
                {
                    sql = "insert into " + Const.Table.FC_WorkFlow_Step + "(FlowFID,StepID,RoleName,DepartID)";
                    sql += "values('" + str.D2DD(fid) + "'," + StepID + ",'" + str.D2DD(rolename) + "','" + str.D2DD(departid) + "')";
                    db.execSql(sql);
                    foreach (string userid in userids)
                    {
                        sql = "insert into " + Const.Table.FC_WorkFlow_User + "(FlowFID,StepID,UserID)";
                        sql += "values('" + str.D2DD(fid) + "'," + StepID + ",'" + str.D2DD(userid) + "')";
                        db.execSql(sql);
                    }
                    foreach (string userid in expectids)
                    {
                        sql = "insert into " + Const.Table.FC_WorkFlow_User_Except + "(FlowFID,StepID,UserID)";
                        sql += "values('" + str.D2DD(fid) + "'," + StepID + ",'" + str.D2DD(userid) + "')";
                        db.execSql(sql);
                    }
                    StepID++;
                    if (Context.Request.Form["mod_rolename_rowrate" + StepID] == null) break;
                    rolename = Context.Request.Form["mod_rolename_rowrate" + StepID].Trim();
                    departid = Context.Request.Form["mod_departid_rowrate" + StepID].Trim();
                    userids = Context.Request.Form["mod_userids_rowrate" + StepID].Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    expectids = Context.Request.Form["mod_expectids_rowrate" + StepID].Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                return null;
            }
            finally
            {
                db.Close();
            }
        }
        public static string FlowGet(string fid, HttpContext Context)
        {
            DB db = new DB();
            db.Open();
            DB db2 = new DB();
            db2.Open();
            try
            {
                string sql = null;
                string js = "";
                sql = "select * from " + Const.Table.FC_WorkFlow_Step + " where FlowFID='" + str.D2DD(fid) + "' order by StepID";
                DR dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    int StepID = dr.GetInt32("StepID");
                    string RoleName = dr.GetStringNoNULL("RoleName");
                    string DepartID = dr.GetStringNoNULL("DepartID");
                    string DepartName = DepartID.Equals("")?"":UserClass.GetDepartNameByDepartID(DepartID);
                    string UserCaps = "";
                    string UserIds = "";
                    sql = "select UserID from " + Const.Table.FC_WorkFlow_User + " where FlowFID='" + str.D2DD(fid) + "' and StepID=" + StepID;
                    DR dr2 = db2.OpenRecord(sql);
                    while (dr2.Read())
                    {
                        UserIds += ","+dr2.GetStringNoNULL(0);
                        UserCaps += "," +UserClass.GetUserALinkName(dr2.GetStringNoNULL(0));
                    }
                    dr2.Close();
                    if (!UserIds.Equals("")) UserIds = UserIds.Substring(1);
                    if (!UserCaps.Equals("")) UserCaps = UserCaps.Substring(1);
                    string ExpectUserCaps = "";
                    string ExpectUserIds = "";
                    sql = "select UserID from " + Const.Table.FC_WorkFlow_User_Except + " where FlowFID='" + str.D2DD(fid) + "' and StepID=" + StepID;
                    dr2 = db2.OpenRecord(sql);
                    while (dr2.Read())
                    {
                        ExpectUserIds += "," + dr2.GetStringNoNULL(0);
                        ExpectUserCaps += "," + UserClass.GetUserALinkName(dr2.GetStringNoNULL(0));
                    }
                    dr2.Close();
                    if (!ExpectUserIds.Equals("")) ExpectUserIds = ExpectUserIds.Substring(1);
                    if (!ExpectUserCaps.Equals("")) ExpectUserCaps = ExpectUserCaps.Substring(1);

                    js += "eleVal('mod_rolename','" + RoleName + "'," + StepID + ");";
                    js += "eleVal('mod_departid','" + DepartID + "'," + StepID + ");";
                    js += "eleVal('mod_departname','" + DepartName + "'," + StepID + ");";
                    js += "eleVal('mod_userids','" + UserIds + "'," + StepID + ");";
                    js += "eleVal('mod_usernames','" + UserCaps + "'," + StepID + ");";
                    js += "eleVal('mod_expectids','" + ExpectUserIds + "'," + StepID + ");";
                    js += "eleVal('mod_expectnames','" + ExpectUserCaps + "'," + StepID + ");";
                }
                dr.Close();
                return js;
            }
            finally
            {
                db.Close(); db2.Close();
            }
        }
    }
}
