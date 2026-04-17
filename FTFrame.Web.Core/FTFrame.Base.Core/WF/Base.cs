using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using System.Linq;
using System.IO;
using System.Xml;
using System.Collections;
using FTFrame.Project.Core;
using CoreHttp = Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
namespace FTFrame.WorkFlow.Core
{
    public class Base
    {
        public static object[] FlowMonitList(string WorkID, string FlowFID, string DepartInUser, string DepartID)
        {
            DB db = new DB();
            db.Open();
            DB db2 = new DB();
            db2.Open();
            try
            {
                ArrayList al = new ArrayList();
                string sql = null;
                int StepID = 0;
                sql = "select * from " + Const.Table.FC_WorkFlow_Detail + " where WORKID='" + str.D2DD(WorkID) + "' order by ALTERTIME asc";
                DR dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    StepID = dr.GetInt32("STEPID");
                    if (!dr.IsDBNull("USERID"))//除去UserID为Null不显示，表示发起审批
                        al.Add(new string[] { User.UserShowName(dr.GetString("USERID")), dr.GetString("FLRESULT"), dr.GetString("FLMIMO"), str.GetDateTime(dr.GetValue("ALTERTIME")) });
                }
                dr.Close();
                sql = "select StepID,RoleName,DepartID from " + Const.Table.FC_WorkFlow_Step + " where FlowFID='" + str.D2DD(FlowFID) + "' and STEPID>" + StepID + " order by STEPID";
                dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    ArrayList JudgeUser = GetJudgeUser(db2, FlowFID, dr.GetInt32(0), DepartInUser, DepartID);
                    string UserCaps = "";
                    foreach (string UserID in JudgeUser)
                    {
                        UserCaps += " " + User.UserShowName(UserID);
                    }
                    JudgeUser.Clear();
                    JudgeUser = null;
                    string GroupCaption = GetGroupCaption(db2, FlowFID, dr.GetInt32(0), DepartInUser, DepartID);
                    al.Add(new string[] { UserCaps, GroupCaption });
                }
                dr.Close();
                return al.ToArray();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new object[0];
            }
            finally
            {
                db.Close(); db2.Close();
            }
        }
        private static string GetGroupCaption(DB db, string FlowFid, int StepID, string UserID, string DepartID)
        {
            string sql = null;
            sql = "select UserID from " + Const.Table.FC_WorkFlow_User + " where FlowFID='" + str.D2DD(FlowFid) + "' and StepID=" + StepID;
            ArrayList User_List = new ArrayList();
            DR dr = db.OpenRecord(sql);
            while (dr.Read())
            {
                User_List.Add(dr.GetString(0));
            }
            dr.Close();
            if (User_List.Count > 0) return "";
            string Caption = "";
            string StepRoleName = null;
            string StepDepartID = null;
            sql = "select RoleName,DepartID from " + Const.Table.FC_WorkFlow_Step + " where FlowFID='" + str.D2DD(FlowFid) + "' and StepID=" + StepID;
            dr = db.OpenRecord(sql);
            if (dr.Read())
            {
                StepRoleName = dr.GetStringNoNULL(0).Trim();
                StepDepartID = dr.GetStringNoNULL(1).Trim();
            }
            dr.Close();

            if (!StepRoleName.Equals(""))
            {
                StepRoleName = StepRoleName.Replace("(DEPART)", DepartNameFromUser(1, UserID, DepartID));
                for (int i = 1; i < 10; i++)
                {
                    StepRoleName = StepRoleName.Replace("(DEPART." + i + ")", DepartNameFromUser(i, UserID, DepartID));
                }
                Caption += " " + StepRoleName;
            }
            if (!StepDepartID.Equals(""))
            {
                Caption += " " + User.GetDepartNameByDepartID(StepDepartID);
            }
            if (!Caption.Equals("")) Caption = Caption.Substring(1);
            return Caption;
        }
        private static ArrayList UserFromRole(string RoleName)
        {
            DB db = new DB();
            db.Open();
            try
            {
                ArrayList al = new ArrayList();
                string sql = null;
                sql = "select a.USERID from BD_USER_ROLE a,TB_ROLE b where a.ROLEID=b.ROLEID and b.ROLENAME='" + str.D2DD(RoleName) + "'";
                DR dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    al.Add(dr.GetString(0));
                }
                dr.Close();
                return al;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new ArrayList();
            }
            finally
            {
                db.Close();
            }
        }
        private static ArrayList UserFromDepartDirect(DB db, string DepartID)
        {
            ArrayList al = new ArrayList();
            string sql = "select USERID from BD_USER_DEPART where DEPARTID='" + str.D2DD(DepartID) + "'";
            DR dr = db.OpenRecord(sql);
            while (dr.Read())
            {
                al.Add(dr.GetString(0));
            }
            dr.Close();
            return al;
        }

        private static string DepartNameFromUser(int level, string UserID, string DepartID)
        {
            DB db = new DB();
            db.Open();
            try
            {
                string sql = null;
                DR dr = null;
                if (DepartID != null)
                {
                    sql = "select a.DEPARTID,a.PARENTID,a.DEPARTNAME from TB_DEPARTINFO a where a.DEPARTID='" + str.D2DD(DepartID) + "'";
                    dr = db.OpenRecord(sql);
                    if (dr.Read())
                    {
                        string DEPARTID = dr.GetString(0);
                        string PARENTID = dr.GetString(1);
                        string DEPARTNAME = dr.GetString(2);
                        dr.Close();
                        while (level > 1)
                        {
                            if (PARENTID.Equals("0")) return "(Corp)";
                            sql = "select PARENTID,DEPARTNAME from TB_DEPARTINFO where DEPARTID='" + str.D2DD(PARENTID) + "'";
                            dr = db.OpenRecord(sql);
                            if (dr.Read())
                            {
                                PARENTID = dr.GetString(0);
                                DEPARTNAME = dr.GetString(1);
                                dr.Close();
                                level--;
                            }
                            else
                            {
                                dr.Close();
                                return "(Corp)";
                            }
                        }
                        return DEPARTNAME;
                    }
                    else
                    {
                        dr.Close();
                        return "(Corp)";
                    }
                }
                else if (UserID != null)
                {
                    sql = "select a.DEPARTID,a.PARENTID,a.DEPARTNAME from TB_DEPARTINFO a,BD_USER_DEPART b where a.DEPARTID=b.DEPARTID and b.USERID='" + str.D2DD(UserID) + "'";
                    dr = db.OpenRecord(sql);
                    if (dr.Read())
                    {
                        string DEPARTID = dr.GetString(0);
                        string PARENTID = dr.GetString(1);
                        string DEPARTNAME = dr.GetString(2);
                        dr.Close();
                        while (level > 1)
                        {
                            if (PARENTID.Equals("0")) return "(Corp)";
                            sql = "select PARENTID,DEPARTNAME from TB_DEPARTINFO where DEPARTID='" + str.D2DD(PARENTID) + "'";
                            dr = db.OpenRecord(sql);
                            if (dr.Read())
                            {
                                PARENTID = dr.GetString(0);
                                DEPARTNAME = dr.GetString(1);
                                dr.Close();
                                level--;
                            }
                            else
                            {
                                dr.Close();
                                return "(Corp)";
                            }
                        }
                        return DEPARTNAME;
                    }
                    else
                    {
                        dr.Close();
                        return "(Corp)";
                    }
                }
                else return "(Corp)";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "(Exception)";
            }
            finally
            {
                db.Close();
            }
        }
        private static ArrayList GetJudgeUser(DB db, string FlowFid, int StepID, string UserID, string DepartID)
        {
            string sql = null;
            sql = "select UserID from " + Const.Table.FC_WorkFlow_User + " where FlowFID='" + str.D2DD(FlowFid) + "' and StepID=" + StepID;
            ArrayList User_List = new ArrayList();
            DR dr = db.OpenRecord(sql);
            while (dr.Read())
            {
                User_List.Add(dr.GetString(0));
            }
            dr.Close();
            if (User_List.Count > 0) return User_List;
            string StepRoleName = null;
            string StepDepartID = null;
            sql = "select RoleName,DepartID from " + Const.Table.FC_WorkFlow_Step + " where FlowFID='" + str.D2DD(FlowFid) + "' and StepID=" + StepID;
            dr = db.OpenRecord(sql);
            if (dr.Read())
            {
                StepRoleName = dr.GetStringNoNULL(0).Trim();
                StepDepartID = dr.GetStringNoNULL(1).Trim();
            }
            dr.Close();
            sql = "select UserID from " + Const.Table.FC_WorkFlow_User_Except + " where FlowFID='" + str.D2DD(FlowFid) + "' and StepID=" + StepID;
            ArrayList User_Except_List = new ArrayList();
            dr = db.OpenRecord(sql);
            while (dr.Read())
            {
                User_Except_List.Add(dr.GetString(0));
            }
            dr.Close();
            ArrayList RoleUsers = new ArrayList();
            if (!StepRoleName.Equals(""))
            {
                StepRoleName = StepRoleName.Replace("(DEPART)", DepartNameFromUser(1, UserID, DepartID));
                for (int i = 1; i < 10; i++)
                {
                    StepRoleName = StepRoleName.Replace("(DEPART." + i + ")", DepartNameFromUser(i, UserID, DepartID));
                }
                RoleUsers = UserFromRole(StepRoleName);
            }
            ArrayList DepartUsers = new ArrayList();
            if (!StepDepartID.Equals(""))
            {
                UserDepart userDepart = new UserDepart();
                userDepart.LoopDepartSave = new ArrayList();
                DepartUsers = userDepart.UserFromDepart(StepDepartID);
            }
            ArrayList JudgeUser = new ArrayList();
            if (StepRoleName.Equals(""))
            {
                JudgeUser = DepartUsers;
            }
            else if (StepDepartID.Equals(""))
            {
                JudgeUser = RoleUsers;
            }
            else
            {
                foreach (string userid in DepartUsers)
                {
                    if (RoleUsers.Contains(userid)) JudgeUser.Add(userid);
                }
            }
            foreach (string userid in JudgeUser)
            {
                if (User_Except_List.Contains(userid)) JudgeUser.Remove(userid);
            }
            User_Except_List.Clear(); User_Except_List = null;
            User_List.Clear(); User_List = null;
            return JudgeUser;
        }
        public static string GetFlowFid(int FlowNum, DB db)
        {
            return db.GetStringForceNoNull("select fid from " + Const.Table.FC_WorkFlow_List + " where flownum=" + FlowNum);
        }
        public static int GetFlowNum(string FlowFid, DB db)
        {
            return db.GetInt("select flownum from " + Const.Table.FC_WorkFlow_List + " where fid='" + str.D2DD(FlowFid) + "'");
        }
        public static string StartFlow(Const.WorkFlowInstance WFI, string WorkID, string DepartInUser, string DepartID)
        {
            string UserID = UserTool.CurUserID();
            if (UserID == null) return SysConst.NotLogin;
            if (DepartInUser != null && DepartInUser.Trim().Equals("")) DepartInUser = null;
            if (DepartID != null && DepartID.Trim().Equals("")) DepartID = null;
            WFC.Push(WFI, WorkID, UserID, DepartInUser, DepartID);
            return null;
        }
        public static string ResetFlow(Const.WorkFlowInstance WFI, string WorkID, string DepartInUser, string DepartID)
        {
            string UserID = UserTool.CurUserID();
            if (UserID == null) return SysConst.NotLogin;
            if (DepartInUser != null && DepartInUser.Trim().Equals("")) DepartInUser = null;
            if (DepartID != null && DepartID.Trim().Equals("")) DepartID = null;
            return WFC.Reset(WFI, WorkID, UserID, DepartInUser, DepartID);
        }
        public static string CancelFlow(Const.WorkFlowInstance WFI, string WorkID)
        {
            string UserID = UserTool.CurUserID();
            if (UserID == null) return SysConst.NotLogin;
            return WFC.Cancel(WFI, WorkID, UserID);
        }
        public static string JudgeFlow(Const.WorkFlowInstance WFI, string WorkID, Const.FlowResultType FlResultType, string FlMimo, string DepartInUser, string DepartID)
        {
            string UserID = UserTool.CurUserID();
            if (UserID == null) return SysConst.NotLogin;
            if (DepartInUser != null && DepartInUser.Trim().Equals("")) DepartInUser = null;
            if (DepartID != null && DepartID.Trim().Equals("")) DepartID = null;
            return WFC.Judge(WFI, WorkID, UserID, FlResultType, FlMimo, DepartInUser, DepartID);
        }
        public static string BatchFlowPass(Const.WorkFlowInstance WFI, string[] ids, string[] userids, string[] departids)
        {
            string UserID = UserTool.CurUserID();
            if (UserID == null) return SysConst.NotLogin;
            string restr = null;
            int count = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                string DepartInUser = (userids == null ? null : userids[i]);
                string DepartID = (departids == null ? null : departids[i]);
                if (DepartInUser != null && DepartInUser.Trim().Equals("")) DepartInUser = null;
                if (DepartID != null && DepartID.Trim().Equals("")) DepartID = null;
                restr = WFC.Judge(WFI, ids[i], UserID, Const.FlowResultType.PASS, "(批量审批通过)", DepartInUser, DepartID);
                if (restr != null) return "已经成功 " + count + " 个，在执行第 " + (count + 1) + " 个时发生错误：" + restr;
                count++;
            }
            return null;
        }
        public static string[] GetDepartInUserByIds_Base(Const.WorkFlowInstance WFI, string[] ids)
        {
            string[] userids = new string[ids.Length];
            DB db = new DB();
            db.Open();
            try
            {
                string sql = null;
                for (int i = 0; i < ids.Length; i++)
                {
                    sql = "select " + WFI.BaseUserColumn + " from " + WFI.BaseTableName + " where " + WFI.BasePrimaryColumn + "='" + str.D2DD(ids[i]) + "'";
                    userids[i] = db.GetString(sql);
                }
                return userids;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return userids;
            }
            finally
            {
                db.Close();
            }
        }
    }
}
