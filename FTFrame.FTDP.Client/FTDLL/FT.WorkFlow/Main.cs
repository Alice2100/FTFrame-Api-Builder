using System;
using System.Collections.Generic;
using System.Text;
using FTFrame;
using FTFrame.Obj;
using FTFrame.DBClient;
using FTFrame.Tool;
using FTFrame.Base;
using System.Web;
using System.Collections;

namespace FT.Com.WorkFlow
{
    public abstract class WorkFlowJob
    {
        public void Push(Instance WFI, string WorkID, string SubmitUser, string DepartInUser, string DepartID)
        {
            DB db = new DB();
            db.Open();
            ST st = null;
            try
            {
                string sql = null;
                sql = "select count(*) from "+Const.Table.FC_WorkFlow_Step+" where FlowFID='" + str.D2DD(WFI.FlowFid) + "' and StepID=1";
                if (db.GetInt(sql)==0)//没有设置流程，则直接通过
                {
                    st = db.GetTransaction();
                    WorkFlow_UpdateStat(WorkID, db, st, WFI.BaseTableName, WFI.BasePrimaryColumn, WFI.EndStat, WFI);
                    st.Commit();
                    return;
                }
                ArrayList JudgeUser = GetJudgeUser(db,WFI.FlowFid,1,DepartInUser, DepartID);
                st = db.GetTransaction();
                sql = "insert into " + Const.Table.FC_WorkFlow_Detail + "(WORKID,FLOWFID,STEPID,ALTERTIME)";
                sql += "values('" + str.D2DD(WorkID) + "','" + str.D2DD(WFI.FlowFid) + "',0,'" + str.GetDateTimeMil() + "')";
                db.execSql(sql, st);
                foreach (string UserID in JudgeUser)
                {
                    sql = "insert into " + Const.Table.FC_WorkFlow_User_Dy + "(WORKID,USERID,ADDTIME,FLOWFID)";
                    sql += "values('" + str.D2DD(WorkID) + "','" + str.D2DD(UserID) + "','" + str.GetDateTime() + "','" + str.D2DD(WFI.FlowFid) + "')";
                    db.execSql(sql, st);
                }
                WorkFlow_UpdateStat(WorkID, db, st, WFI.BaseTableName, WFI.BasePrimaryColumn, WFI.InitStat, WFI);
                st.Commit();
                st = null;

                if (SubmitUser != null && (JudgeUser.Count == 1 || !WFI.StrictAutoPass) && JudgeUser.Contains(SubmitUser))
                {
                    Judge(WFI, WorkID, SubmitUser, Const.FlowResultType.PASS, "(自己审批，自动通过)", DepartInUser, DepartID);
                }
                else
                {
                    WorkFlowJudge_Remind(WFI, WorkID);
                }

                JudgeUser.Clear(); JudgeUser = null;

                if (WFI.FlowStartActionTip != null) UserTool.Action(WorkID, WFI.FlowStartActionTip);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (st != null) st.Rollback();
            }
            finally
            {
                db.Close();
            }
        }
        public string Judge(Instance WFI, string WorkID, string UserID, Const.FlowResultType FlResultType, string FlMimo, string DepartInUser, string DepartID)
        {
            DB db = new DB();
            db.Open();
            ST st = null;
            try
            {
                string sql = null;
                string FlowFID = WFI.FlowFid;
                sql = "select count(*) from "+Const.Table.FC_WorkFlow_User_Dy+" where WORKID='" + str.D2DD(WorkID) + "' and USERID='" + str.D2DD(UserID) + "'";
                if (db.GetInt(sql) == 0) return "您不能审批当前阶段";//当前用户还没有权限审批该一步,或者审批已经结束
                if (FlResultType.Equals(Const.FlowResultType.PASS))//同意
                {
                    sql = "select STEPID from " + Const.Table.FC_WorkFlow_Detail + " where WORKID='" + str.D2DD(WorkID) + "' order by ALTERTIME desc";//最近一次的状况
                    DR dr = db.OpenRecord(sql);
                    dr.Read();
                    int StepID = dr.GetInt32(0);
                    dr.Close();

                    sql = "select count(*) from " + Const.Table.FC_WorkFlow_Step + " where FlowFID='" + str.D2DD(FlowFID) + "' and StepID=" + (StepID + 2);
                    if (db.GetInt(sql)>0)//前进一步
                    {
                        ArrayList JudgeUser = GetJudgeUser(db,WFI.FlowFid,StepID + 2, DepartInUser, DepartID);
                        st = db.GetTransaction();
                        sql = "delete from " + Const.Table.FC_WorkFlow_User_Dy + " where WORKID='" + str.D2DD(WorkID) + "'";
                        db.execSql(sql, st);
                        sql = "insert into " + Const.Table.FC_WorkFlow_Detail + "(WORKID,FLOWFID,STEPID,ALTERTIME,USERID,FLRESULT,FLMIMO)";
                        sql += "values('" + str.D2DD(WorkID) + "','" + str.D2DD(FlowFID) + "'," + (StepID + 1) + ",'" + str.GetDateTimeMil() + "','" + str.D2DD(UserID) + "','同意','" + str.D2DD(FlMimo) + "')";
                        db.execSql(sql, st);
                        foreach (string NewUserID in JudgeUser)
                        {
                            sql = "insert into " + Const.Table.FC_WorkFlow_User_Dy + "(WORKID,USERID,ADDTIME,FLOWFID)";
                            sql += "values('" + str.D2DD(WorkID) + "','" + str.D2DD(NewUserID) + "','" + str.GetDateTime() + "','" + str.D2DD(FlowFID) + "')";
                            db.execSql(sql, st);
                        }
                        WorkFlow_UpdateStat(WorkID, db, st, WFI.BaseTableName, WFI.BasePrimaryColumn, WFI.ProcessStat, WFI);
                        st.Commit();
                        st = null;

                        if ((JudgeUser.Count == 1 || !WFI.StrictAutoPass) && JudgeUser.Contains(UserID))
                        {
                            Judge(WFI, WorkID, UserID, Const.FlowResultType.PASS, "(自己审批，自动通过)", DepartInUser, DepartID);
                        }
                        else
                        {
                            WorkFlowJudge_Remind(WFI, WorkID);
                        }
                    }
                    else//流程结束
                    {
                        st = db.GetTransaction();
                        sql = "delete from " + Const.Table.FC_WorkFlow_User_Dy + " where WORKID='" + str.D2DD(WorkID) + "'";
                        db.execSql(sql, st);
                        sql = "insert into " + Const.Table.FC_WorkFlow_Detail + "(WORKID,FLOWFID,STEPID,ALTERTIME,USERID,FLRESULT,FLMIMO)";
                        sql += "values('" + str.D2DD(WorkID) + "','" + str.D2DD(FlowFID) + "'," + (StepID + 1) + ",'" + str.GetDateTimeMil() + "','" + str.D2DD(UserID) + "','同意','" + str.D2DD(FlMimo) + "')";
                        db.execSql(sql, st);
                        WorkFlow_UpdateStat(WorkID, db, st, WFI.BaseTableName, WFI.BasePrimaryColumn, WFI.EndStat, WFI);
                        st.Commit();
                        st = null;

                        WorkFlowEnd_Remind(WFI, WorkID);
                    }
                }
                else if (FlResultType.Equals(Const.FlowResultType.LASTSTEP))//退回给上级
                {
                    sql = "select STEPID from " + Const.Table.FC_WorkFlow_Detail + " where WORKID='" + str.D2DD(WorkID) + "' order by ALTERTIME desc";//最近一次的状况
                    DR dr = db.OpenRecord(sql);
                    dr.Read();
                    int StepID = dr.GetInt32(0);
                    dr.Close();
                    if (StepID == 0)//上级就是本人，无法退还，不操作
                    {

                    }
                    else//上级审批的用户重新载入
                    {
                        ArrayList JudgeUser = GetJudgeUser(db, FlowFID, StepID, DepartInUser, DepartID);
                        st = db.GetTransaction();
                        sql = "delete from " + Const.Table.FC_WorkFlow_User_Dy + " where WORKID='" + str.D2DD(WorkID) + "'";
                        db.execSql(sql, st);
                        sql = "insert into " + Const.Table.FC_WorkFlow_Detail + "(WORKID,FLOWFID,STEPID,ALTERTIME,USERID,FLRESULT,FLMIMO)";
                        sql += "values('" + str.D2DD(WorkID) + "','" + str.D2DD(FlowFID) + "'," + (StepID - 1) + ",'" + str.GetDateTimeMil() + "','" + str.D2DD(UserID) + "','退回给上级','" + str.D2DD(FlMimo) + "')";
                        db.execSql(sql, st);
                        foreach (string NewUserID in JudgeUser)
                        {
                            sql = "insert into " + Const.Table.FC_WorkFlow_User_Dy + "(WORKID,USERID,ADDTIME,FLOWFID)";
                            sql += "values('" + str.D2DD(WorkID) + "','" + str.D2DD(NewUserID) + "','" + str.GetDateTime() + "','" + str.D2DD(FlowFID) + "')";
                            db.execSql(sql, st);
                        }
                        if (StepID - 1 == 0)//退到底
                        {
                            WorkFlow_UpdateStat(WorkID, db, st, WFI.BaseTableName, WFI.BasePrimaryColumn, WFI.RejectStat, WFI);
                            st.Commit();
                            st = null;
                            WorkFlowReject_Remind(WFI, WorkID);
                        }
                        else
                        {
                            WorkFlow_UpdateStat(WorkID, db, st, WFI.BaseTableName, WFI.BasePrimaryColumn, WFI.ProcessStat, WFI);
                            st.Commit();
                            st = null;
                        }
                    }
                }
                else if (FlResultType.Equals(Enums.FlowResultType.REJECT))//退回给本人
                {
                    ArrayList JudgeUser = GetJudgeUser(db, FlowFID, 1, DepartInUser, DepartID);
                    st = db.GetTransaction();
                    sql = "delete from " + Const.Table.FC_WorkFlow_User_Dy + " where WORKID='" + str.D2DD(WorkID) + "'";
                    db.execSql(sql, st);
                    sql = "insert into " + Const.Table.FC_WorkFlow_Detail + "(WORKID,FLOWFID,STEPID,ALTERTIME,USERID,FLRESULT,FLMIMO)";
                    sql += "values('" + str.D2DD(WorkID) + "','" + str.D2DD(FlowFID) + "',0,'" + str.GetDateTimeMil() + "','" + str.D2DD(UserID) + "','退回给本人','" + str.D2DD(FlMimo) + "')";
                    db.execSql(sql, st);
                    foreach (string NewUserID in JudgeUser)
                    {
                        sql = "insert into " + Const.Table.FC_WorkFlow_User_Dy + "(WORKID,USERID,ADDTIME,FLOWFID)";
                        sql += "values('" + str.D2DD(WorkID) + "','" + str.D2DD(NewUserID) + "','" + str.GetDateTime() + "','" + str.D2DD(FlowFID) + "')";
                        db.execSql(sql, st);
                    }
                    WorkFlow_UpdateStat(WorkID, db, st, WFI.BaseTableName, WFI.BasePrimaryColumn, WFI.RejectStat, WFI);

                    st.Commit();
                    st = null;

                    WorkFlowReject_Remind(WFI, WorkID);
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (st != null) st.Rollback();
                return ex.Message;
            }
            finally
            {
                db.Close();
            }
        }
        public string Reset(Instance WFI, string WorkID, string UserID, string DepartInUser, string DepartID)
        {
            DB db = new DB();
            db.Open();
            ST st = null;
            try
            {
                string sql = null;
                ArrayList JudgeUser = GetJudgeUser(db, WFI.FlowFid, 1, DepartInUser, DepartID);
                st = db.GetTransaction();
                sql = "delete from " + Const.Table.FC_WorkFlow_User_Dy + " where WORKID='" + str.D2DD(WorkID) + "'";
                db.execSql(sql, st);
                sql = "insert into " + Const.Table.FC_WorkFlow_Detail + "(WORKID,FLOWFID,STEPID,ALTERTIME,USERID,FLRESULT,FLMIMO)";
                sql += "values('" + str.D2DD(WorkID) + "','" + str.D2DD(WFI.FlowFid) + "',0,'" + str.GetDateTimeMil() + "','" + str.D2DD(UserID) + "','重置审批','')";
                db.execSql(sql, st);
                foreach (string NewUserID in JudgeUser)
                {
                    sql = "insert into " + Const.Table.FC_WorkFlow_User_Dy + "(WORKID,USERID,ADDTIME,FLOWFID)";
                    sql += "values('" + str.D2DD(WorkID) + "','" + str.D2DD(NewUserID) + "','" + str.GetDateTime() + "','" + str.D2DD(WFI.FlowFid) + "')";
                    db.execSql(sql, st);
                }
                WorkFlow_UpdateStat(WorkID, db, st, WFI.BaseTableName, WFI.BasePrimaryColumn, WFI.InitStat, WFI);

                st.Commit();
                st = null;

                if (WFI.FlowResetActionTip != null) UserTool.Action(WorkID, WFI.FlowResetActionTip);

                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (st != null) st.Rollback();
                return ex.Message;
            }
            finally
            {
                db.Close();
            }
        }
        public string Cancel(Instance WFI, string WorkID, string UserID)
        {
            DB db = new DB();
            db.Open();
            ST st = null;
            try
            {
                st = db.GetTransaction();
                string sql = "delete from " + Const.Table.FC_WorkFlow_User_Dy + " where WORKID='" + str.D2DD(WorkID) + "'";
                db.execSql(sql, st);
                sql = "insert into " + Const.Table.FC_WorkFlow_Detail + "(WORKID,FLOWFID,STEPID,ALTERTIME,USERID,FLRESULT,FLMIMO)";
                sql += "values('" + str.D2DD(WorkID) + "','" + str.D2DD(WFI.FlowFid) + "',0,'" + str.GetDateTimeMil() + "','" + str.D2DD(UserID) + "','取消审批','')";
                db.execSql(sql, st);
                WorkFlow_UpdateStat(WorkID, db, st, WFI.BaseTableName, WFI.BasePrimaryColumn, WFI.CancelStat, WFI);

                st.Commit();
                st = null;

                if (WFI.FlowCancelActionTip != null) UserTool.Action(WorkID, WFI.FlowCancelActionTip);

                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (st != null) st.Rollback();
                return ex.Message;
            }
            finally
            {
                db.Close();
            }
        }
        public object[] FlowMonitList(string WorkID, string FlowFID, string DepartInUser, string DepartID)
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
                sql = "select * from "+Const.Table.FC_WorkFlow_Detail+" where WORKID='" + str.D2DD(WorkID) + "' order by ALTERTIME asc";
                DR dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    StepID = dr.GetInt32("STEPID");
                    if (!dr.IsDBNull("USERID"))//除去UserID为Null不显示，表示发起审批
                        al.Add(new string[] { UserClass.GetUserALinkName(dr.GetString("USERID")), dr.GetString("FLRESULT"), dr.GetString("FLMIMO"), str.GetDateTime(dr.GetValue("ALTERTIME")) });
                }
                dr.Close();
                sql = "select StepID,RoleName,DepartID from " + Const.Table.FC_WorkFlow_Step + " where FlowFID='" + str.D2DD(FlowFID) + "' and STEPID>" + StepID + " order by STEPID";
                dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    ArrayList JudgeUser = GetJudgeUser(db2,FlowFID,dr.GetInt32(0), DepartInUser, DepartID);
                    string UserCaps = "";
                    foreach (string UserID in JudgeUser)
                    {
                        UserCaps += " " + UserClass.GetUserALinkName(UserID);
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
        private ArrayList GetJudgeUser(DB db, string FlowFid,int StepID,string UserID, string DepartID)
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
            sql = "select RoleName,DepartID from " + Const.Table.FC_WorkFlow_Step + " where FlowFID='" + str.D2DD(FlowFid) + "' and StepID="+StepID;
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
            ArrayList DepartUsers=new ArrayList();
            if (!StepDepartID.Equals(""))
            {
                LoopDepartSave=new ArrayList();
                DepartUsers = UserFromDepart(StepDepartID);
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
        private string GetGroupCaption(DB db, string FlowFid, int StepID, string UserID, string DepartID)
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
                Caption += " " + UserClass.GetDepartNameByDepartID(StepDepartID);
            }
            if (!Caption.Equals("")) Caption = Caption.Substring(1);
            return Caption;
        }
        private string DepartNameFromUser(int level, string UserID, string DepartID)
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
        private ArrayList UserFromRole(string RoleName)
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
        private ArrayList UserFromDepartDirect(DB db,string DepartID)
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
        private ArrayList LoopDepartSave = null;
        private ArrayList UserFromDepart(string DepartID)
        {
            if (LoopDepartSave.Contains(DepartID)) return new ArrayList();
            LoopDepartSave.Add(DepartID);
            DB db = new DB();
            db.Open();
            try
            {
                ArrayList al = new ArrayList();
                string sql = "select USERID from BD_USER_DEPART where DEPARTID='" + str.D2DD(DepartID) + "'";
                DR dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    al.Add(dr.GetString(0));
                }
                dr.Close();
                ArrayList departal = new ArrayList();
                sql = "select DEPARTID from TB_DEPARTINFO where PARENTID='" + str.D2DD(DepartID) + "'";
                dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    departal.Add(dr.GetString(0));
                }
                dr.Close();
                foreach (string departId in departal)
                {
                    ArrayList al2 = UserFromDepart(departId);
                    foreach (string userid in al2)
                    {
                        if (!al.Contains(userid)) al.Add(userid);
                    }
                    al2.Clear(); al2 = null;
                }
                return al;
            }
            catch (Exception ex) { log.Error(ex); return new ArrayList(); }
            finally { db.Close(); }
        }
        public abstract void FlowStatUpdatedOperation(DB db, ST st, Instance WFI, string WorkID, int StatValue);
        private  void WorkFlow_UpdateStat(string WorkID, DB db2, ST st, string TableName, string PrimaryColumn, int StatValue, Instance WFI)
        {
            string sql = "update " + TableName + " set flow=" + StatValue + " where " + PrimaryColumn + "='" + str.D2DD(WorkID) + "'";
            db2.execSql(sql, st);
            FlowStatUpdatedOperation(db2, st, WFI, WorkID, StatValue);
        }
        private void WorkFlowJudge_Remind(Instance WFI, string WorkID)
        {
            DB db = new DB();
            db.Open();
            try
            {
                string FromUserID = null;
                string KeyCap = null;
                bool CusSql = false;
                if (WFI.BaseKeySql != null && WFI.BaseUserSql != null)
                {
                    FromUserID = db.GetStringForceNoNull(WFI.BaseUserSql.Replace("%WORKID%", str.D2DD(WorkID)));
                    KeyCap = db.GetStringForceNoNull(WFI.BaseKeySql.Replace("%WORKID%", str.D2DD(WorkID)));
                    CusSql = true;
                }
                string sql = null;
                sql = "select a.USERID,b." + WFI.BaseUserColumn + " as FROMID,b." + WFI.BaseKeyColumn + " as KEYCOLUMN from " + Const.Table.FC_WorkFlow_User_Dy + " a," + WFI.BaseTableName + " b where a.WORKID='" + str.D2DD(WorkID) + "' and a.WORKID=b." + WFI.BasePrimaryColumn + "";
                DR dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    if (!CusSql)
                    {
                        FromUserID = dr.GetString("FROMID");
                        KeyCap = dr.GetStringForceNoNULL("KEYCOLUMN");
                    }
                    string UserCap = UserClass.GetUserALinkName(FromUserID);
                    Message.Add("system", dr.GetString("UserID"), WFI.RemindJudgeTitle.Replace("%USER%", UserCap).Replace("%KEY%", KeyCap), WFI.RemindJudgeContent.Replace("%USER%", UserCap).Replace("%KEY%", KeyCap).Replace("%WORKID%", WorkID), 1);
                }
                dr.Close();
            }
            catch (Exception ex) { log.Error(ex); }
            finally { db.Close(); }
        }
        private void WorkFlowEnd_Remind(Instance WFI, string WorkID)
        {
            DB db = new DB();
            db.Open();
            try
            {
                string UserID = null;
                string KeyCap = null;
                if (WFI.BaseKeySql != null && WFI.BaseUserSql != null)
                {
                    UserID = db.GetStringForceNoNull(WFI.BaseUserSql.Replace("%WORKID%", str.D2DD(WorkID)));
                    KeyCap = db.GetStringForceNoNull(WFI.BaseKeySql.Replace("%WORKID%", str.D2DD(WorkID)));
                }
                else
                {
                    string sql = "select " + WFI.BaseKeyColumn + "," + WFI.BaseUserColumn + " from " + WFI.BaseTableName + " where " + WFI.BasePrimaryColumn + "='" + str.D2DD(WorkID) + "'";
                    DR dr = db.OpenRecord(sql);
                    if (dr.Read())
                    {
                        UserID = dr.GetString(1);
                        KeyCap = dr.GetStringForceNoNULL(0);
                    }
                    dr.Close();
                }
                Message.Add("system", UserID, WFI.RemindEndTitle.Replace("%KEY%", KeyCap), WFI.RemindEndContent.Replace("%KEY%", KeyCap).Replace("%WORKID%", WorkID), 2);
            }
            catch (Exception ex) { log.Error(ex); }
            finally { db.Close(); }
        }
        private void WorkFlowReject_Remind(Instance WFI, string WorkID)
        {
            DB db = new DB();
            db.Open();
            try
            {
                string UserID = null;
                string KeyCap = null;
                if (WFI.BaseKeySql != null && WFI.BaseUserSql != null)
                {
                    UserID = db.GetStringForceNoNull(WFI.BaseUserSql.Replace("%WORKID%", str.D2DD(WorkID)));
                    KeyCap = db.GetStringForceNoNull(WFI.BaseKeySql.Replace("%WORKID%", str.D2DD(WorkID)));
                }
                else
                {
                    string sql = "select " + WFI.BaseKeyColumn + "," + WFI.BaseUserColumn + " from " + WFI.BaseTableName + " where " + WFI.BasePrimaryColumn + "='" + str.D2DD(WorkID) + "'";
                    DR dr = db.OpenRecord(sql);
                    if (dr.Read())
                    {
                        UserID = dr.GetString(1);
                        KeyCap = dr.GetStringForceNoNULL(0);
                    }
                    dr.Close();
                }
                Message.Add("system", UserID, WFI.RemindRejectTitle.Replace("%KEY%", KeyCap), WFI.RemindRejectContent.Replace("%KEY%", KeyCap).Replace("%WORKID%", WorkID), 2);
            }
            catch (Exception ex) { log.Error(ex); }
            finally { db.Close(); }
        }
    }
}
