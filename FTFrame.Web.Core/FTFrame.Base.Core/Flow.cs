using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using System.Linq;
using System.IO;
using System.Xml;
using System.Collections;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Web;
using FTFrame.Model;
using FTFrame.Project.Core;
using FTFrame.Project.Core.WorkFlow;
using System.Xml.Linq;
using Google.Protobuf.WellKnownTypes;

namespace FTFrame.Base.Core
{
    public class Flow
    {
        public Flow(DB ftDb, ST ftSt, string FlowID, string workID, string userID, string MainTable = null, string MainTableKey = null)
        {
            db = ftDb;
            st = ftSt;
            UserID = userID;
            WorkID = workID;
            FtFlow = db.SelectOne<FtFlowList>(r => r.FlowId == FlowID,null, "ft_flow_list");
            if (FtFlow == null) throw new Exception(Const.NoThisFlow);
            if (!string.IsNullOrEmpty(MainTable)) FtFlow.MainTable = MainTable;
            if (!string.IsNullOrEmpty(MainTableKey)) FtFlow.MainTableKey = MainTableKey;
            FlowDoc = new XmlDocument();
            FlowDoc.LoadXml(FtFlow.FlowDefine);
        }
        private XmlDocument FlowDoc;
        private FtFlowList FtFlow;
        private string WorkID;
        private DB db;
        private ST st;
        private string UserID;
        public string Join(string mimo = "", string para = "")
        {
            try
            {
                //var flowhis = db.SelectList<FtFlowHis>(r => r.FlowId == FlowID && r.WorkId == WorkID, r => r.OrderByDescending(r => r.AlterTime)).FirstOrDefault();
                var flowhis = db.SelectOneBySQL<FtFlowHis>("select * from ft_flow_his where FlowID='" + FtFlow.FlowId.D2() + "' and WorkID='" + WorkID.D2() + "' order by AlterTime desc");
                if (flowhis != null && flowhis.IsFinished == 0) return Const.FlowNotFinished;
                XmlNode StartNode = null;
                var nodes = FlowDoc.SelectNodes("//Rect/mxCell/mxGeometry[@isStart='1']");
                if (nodes.Count > 0)
                {
                    StartNode = nodes[0];
                }
                else
                {
                    nodes = FlowDoc.SelectNodes("//Roundrect/mxCell/mxGeometry[@isStart='1']");
                    if (nodes.Count > 0)
                    {
                        StartNode = nodes[0];
                    }
                    else
                    {
                        nodes = FlowDoc.SelectNodes("//Shape/mxCell/mxGeometry[@isStart='1']");
                        if (nodes.Count > 0)
                        {
                            StartNode = nodes[0];
                        }
                        else
                        {
                            nodes = FlowDoc.SelectNodes("//Image/mxCell/mxGeometry[@isStart='1']");
                            if (nodes.Count > 0)
                            {
                                StartNode = nodes[0];
                            }
                        }
                    }
                }
                if (StartNode == null) return Const.NoStartNode;
                string ret = NodeNext(StartNode.ParentNode.ParentNode, mimo, para);
                if (ret == null) Filter.Join(db, st, FtFlow, WorkID, UserID, FlowDoc);
                return ret;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
        public string Judge(Enums.FlowJudge judge, string mimo, string para = "")
        {
            try
            {
                string sql = null;
                var flowNext = db.SelectOneBySQL<FtFlowNext>("select * from ft_flow_next where WorkID='" + WorkID.D2() + "' and FlowID='" + FtFlow.FlowId.D2() + "'");
                if (flowNext == null) return Const.NoNodeFindInDB;
                var curNode = FlowDoc.SelectSingleNode("/mxGraphModel/root/*[@id='" + flowNext.StepId + "']");
                if (curNode == null) return Const.NoNodeFindFor + " " + flowNext.StepId + "";
                if (db.SelectCount("select count(*) from ft_flow_next_user where WorkID='" + WorkID.D2() + "' and FlowID='" + FtFlow.FlowId.D2() + "' and UserID='" + UserID.D2() + "'") == 0)
                {
                    return Const.YouCanNotJudgeThisNode;
                }
                if (judge == Enums.FlowJudge.Pass)
                {
                    string ret = NodeNext(curNode, mimo, para);
                    if (ret != null) return ret;
                }
                else if (judge == Enums.FlowJudge.RejectToFinish)
                {
                    sql = "delete from ft_flow_next where WorkID='" + WorkID.D2() + "' and FlowID='" + FtFlow.FlowId.D2() + "'";
                    db.ExecSql(sql, st);
                    sql = "delete from ft_flow_next_user where WorkID='" + WorkID.D2() + "' and FlowID='" + FtFlow.FlowId.D2() + "'";
                    db.ExecSql(sql, st);
                    sql = "insert into ft_flow_his(WorkID,FlowID,StepValue,StepID,AlterTime,NextStepID,UserID,Result,Mimo,StepCaption,IsEnd,IsStart,IsFinished)";
                    sql += "values(@WorkID,@FlowID,@StepValue,@StepID,@AlterTime,@NextStepID,@UserID,@Result,@Mimo,@StepCaption,@IsEnd,@IsStart,@IsFinished)";
                    db.ExecSql(sql, st, new PR[] {
                    new PR("@WorkID",WorkID),
                    new PR("@FlowID",FtFlow.FlowId),
                    new PR("@StepValue",FtFlow.RejectStatValue),
                    new PR("@StepID",-2),
                    new PR("@AlterTime",DateTime.Now),
                    new PR("@NextStepID",-30),
                    new PR("@UserID",UserID),
                    new PR("@Result",Const.FlowCaptionReject),
                    new PR("@Mimo",mimo),
                    new PR("@StepCaption",""),
                    new PR("@IsEnd",1),
                    new PR("@IsStart",0),
                    new PR("@IsFinished",1), });
                    //状态值写入
                    if (!string.IsNullOrEmpty(FtFlow.StatColumn))
                    {
                        sql = "update " + FtFlow.MainTable.D2() + " set " + FtFlow.StatColumn.D2() + "='" + FtFlow.RejectStatValue + "' where " + FtFlow.MainTableKey.D2() + "='" + WorkID.D2() + "'";
                        db.ExecSql(sql, st);
                    }
                    if (!string.IsNullOrEmpty(FtFlow.StatCaptionColumn))
                    {
                        string caption = GetCaptionByStat(FtFlow.StatCaptionEnum, (int)FtFlow.RejectStatValue);
                        if (caption != null)
                        {
                            sql = "update " + FtFlow.MainTable.D2() + " set " + FtFlow.StatCaptionColumn.D2() + "='" + caption.D2() + "' where " + FtFlow.MainTableKey.D2() + "='" + WorkID.D2() + "'";
                            db.ExecSql(sql, st);
                        }
                    }
                }
                else if (judge == Enums.FlowJudge.RejectToLastStep)//退回到上一步方案待成熟
                {
                    string ret = NodePrev(flowNext, mimo, para);
                    if (ret != null) return ret;
                }
                Filter.Judge(db, st, FtFlow, WorkID, UserID, judge, FlowDoc, curNode);
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
        public string Cancel()
        {
            try
            {
                string sql = null;
                sql = "delete from ft_flow_next where WorkID='" + WorkID.D2() + "' and FlowID='" + FtFlow.FlowId.D2() + "'";
                db.ExecSql(sql, st);
                sql = "delete from ft_flow_next_user where WorkID='" + WorkID.D2() + "' and FlowID='" + FtFlow.FlowId.D2() + "'";
                db.ExecSql(sql, st);
                sql = "insert into ft_flow_his(WorkID,FlowID,StepValue,StepID,AlterTime,NextStepID,UserID,Result,Mimo,StepCaption,IsEnd,IsStart,IsFinished)";
                sql += "values(@WorkID,@FlowID,@StepValue,@StepID,@AlterTime,@NextStepID,@UserID,@Result,@Mimo,@StepCaption,@IsEnd,@IsStart,@IsFinished)";
                db.ExecSql(sql, st, new PR[] {
                    new PR("@WorkID",WorkID),
                    new PR("@FlowID",FtFlow.FlowId),
                    new PR("@StepValue",FtFlow.CancelStatValue),
                    new PR("@StepID",-1),
                    new PR("@AlterTime",DateTime.Now),
                    new PR("@NextStepID",-20),
                    new PR("@UserID",UserID),
                    new PR("@Result",Const.FlowCaptionCancel),
                    new PR("@Mimo",""),
                    new PR("@StepCaption",""),
                    new PR("@IsEnd",1),
                    new PR("@IsStart",0),
                    new PR("@IsFinished",1), });
                //状态值写入
                if (!string.IsNullOrEmpty(FtFlow.StatColumn))
                {
                    sql = "update " + FtFlow.MainTable.D2() + " set " + FtFlow.StatColumn.D2() + "='" + FtFlow.CancelStatValue + "' where " + FtFlow.MainTableKey.D2() + "='" + WorkID.D2() + "'";
                    db.ExecSql(sql, st);
                }
                if (!string.IsNullOrEmpty(FtFlow.StatCaptionColumn))
                {
                    string caption = GetCaptionByStat(FtFlow.StatCaptionEnum, (int)FtFlow.CancelStatValue);
                    if (caption != null)
                    {
                        sql = "update " + FtFlow.MainTable.D2() + " set " + FtFlow.StatCaptionColumn.D2() + "='" + caption.D2() + "' where " + FtFlow.MainTableKey.D2() + "='" + WorkID.D2() + "'";
                        db.ExecSql(sql, st);
                    }
                }
                Filter.Cancel(db, st, FtFlow, WorkID, UserID);
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
        private string NodeNext(XmlNode outNode, string mimo = "", string para = "")
        {
            try
            {
                //判断是下一步是满足条件的唯一节点
                string nodeId = outNode.Attributes["id"].Value;
                string stepCaption = outNode.Attributes["label"].Value;
                var nodes = FlowDoc.SelectNodes("//Connector/mxCell[@source='" + nodeId + "']");
                if (nodes.Count == 0) return Const.NextStepsCountNotBe0;
                string nextNodeId = null;
                foreach (XmlNode node in nodes)
                {
                    string nextId = node.SelectSingleNode("mxCell").Attributes["target"].Value;
                    var CdnNode = node.SelectSingleNode("mxCell/mxGeometry");
                    string codectn = CdnNode.Attributes["codectn"]?.Value;
                    string sqlct = CdnNode.Attributes["sqlct"]?.Value;
                    int sqllr = CdnNode.Attributes["sqllr"] == null ? 0 : int.Parse(CdnNode.Attributes["sqllr"].Value);
                    string sqlval = CdnNode.Attributes["sqlval"]?.Value;
                    if (!string.IsNullOrWhiteSpace(codectn))
                    {
                        var ret = Project.Core.Code.Get(codectn.Replace("{workid}", WorkID).Replace("{flowid}", FtFlow.FlowId).Replace("{para}", para),null);
                        if (ret == null)
                        {
                            if (nextNodeId != null) return Const.MoreThenOneNextStepsSatisfied;
                            nextNodeId = nextId;
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(sqlct))
                    {
                        var ret = SQLConditionOK(sqlct, sqllr, sqlval);
                        if (ret)
                        {
                            if (nextNodeId != null) return Const.MoreThenOneNextStepsSatisfied;
                            nextNodeId = nextId;
                        }
                    }
                    else
                    {
                        if (nextNodeId != null) return Const.MoreThenOneNextStepsSatisfied;
                        nextNodeId = nextId;
                    }
                }
                var nextNode = FlowDoc.SelectSingleNode("/mxGraphModel/root/*[@id='" + nextNodeId + "']");
                if (nextNode == null) return Const.NoNodeFindFor + " " + nextNodeId + "";
                if ("|Rect|Roundrect|Shape|Image|".IndexOf("|" + nextNode.Name + "|") < 0)
                {
                    return Const.NoNodeFindFor + " " + nextNodeId + "";
                }
                string nextStepCaption = nextNode.Attributes["label"].Value;
                //operation out
                string operateOut = outNode.SelectSingleNode("mxCell/mxGeometry").Attributes["operateOut"]?.Value;
                if (operateOut != null)
                {
                    var ret = Operate(operateOut);
                    if (ret != null) return ret;
                }
                //数据表处理
                var souNode = outNode.SelectSingleNode("mxCell/mxGeometry");
                int isStart = souNode.Attributes["isStart"] == null ? 0 : int.Parse(souNode.Attributes["isStart"].Value);
                string statValueSource = (souNode.Attributes["statValue"] == null || string.IsNullOrEmpty(souNode.Attributes["statValue"].Value)) ? nodeId : souNode.Attributes["statValue"].Value;
                string sql = "insert into ft_flow_his(WorkID,FlowID,StepValue,StepID,AlterTime,NextStepID,UserID,Result,Mimo,StepCaption,IsEnd,IsStart,IsFinished)";
                sql += "values(@WorkID,@FlowID,@StepValue,@StepID,@AlterTime,@NextStepID,@UserID,@Result,@Mimo,@StepCaption,@IsEnd,@IsStart,@IsFinished)";
                db.ExecSql(sql, st, new PR[] {
                    new PR("@WorkID",WorkID),
                    new PR("@FlowID",FtFlow.FlowId),
                    new PR("@StepValue",int.Parse(statValueSource)),
                    new PR("@StepID",int.Parse(nodeId)),
                    new PR("@AlterTime",DateTime.Now),
                    new PR("@NextStepID",int.Parse(nextNodeId)),
                    new PR("@UserID",UserID),
                    new PR("@Result",isStart==1?Const.FlowCaptionSubmit:Const.FlowCaptionPass),
                    new PR("@Mimo",mimo),
                    new PR("@StepCaption",stepCaption),
                    new PR("@IsEnd",0),
                    new PR("@IsStart",isStart),
                    new PR("@IsFinished",0),
                });
                sql = "delete from ft_flow_next where WorkID='" + WorkID.D2() + "' and FlowID='" + FtFlow.FlowId.D2() + "'";
                db.ExecSql(sql, st);
                sql = "delete from ft_flow_next_user where WorkID='" + WorkID.D2() + "' and FlowID='" + FtFlow.FlowId.D2() + "'";
                db.ExecSql(sql, st);
                //operetion In
                var tarNode = nextNode.SelectSingleNode("mxCell/mxGeometry");
                int isEnd = tarNode.Attributes["isEnd"] == null ? 0 : int.Parse(tarNode.Attributes["isEnd"].Value);
                string statValue = (tarNode.Attributes["statValue"] == null || string.IsNullOrEmpty(tarNode.Attributes["statValue"].Value)) ? nextNodeId : tarNode.Attributes["statValue"].Value;
                string operateIn = tarNode.Attributes["operateIn"]?.Value;
                string stepRole = tarNode.Attributes["stepRole"]?.Value;
                string stepDepart = tarNode.Attributes["stepDepart"]?.Value;
                string stepUser = tarNode.Attributes["stepUser"]?.Value;
                string stepCode = tarNode.Attributes["stepCode"]?.Value;
                if (operateIn != null)
                {
                    var ret = Operate(operateIn);
                    if (ret != null) return ret;
                }
                //状态值写入
                if (!string.IsNullOrEmpty(FtFlow.StatColumn))
                {
                    sql = "update " + FtFlow.MainTable.D2() + " set " + FtFlow.StatColumn.D2() + "='" + statValue.D2() + "' where " + FtFlow.MainTableKey.D2() + "='" + WorkID.D2() + "'";
                    db.ExecSql(sql, st);
                }
                if (!string.IsNullOrEmpty(FtFlow.StatCaptionColumn))
                {
                    string caption = GetCaptionByStat(FtFlow.StatCaptionEnum, int.Parse(statValue));
                    if (caption != null)
                    {
                        sql = "update " + FtFlow.MainTable.D2() + " set " + FtFlow.StatCaptionColumn.D2() + "='" + caption.D2() + "' where " + FtFlow.MainTableKey.D2() + "='" + WorkID.D2() + "'";
                        db.ExecSql(sql, st);
                    }
                }
                //End节点处理 也要处理数据表
                if (isEnd == 1)
                {
                    sql = "insert into ft_flow_his(WorkID,FlowID,StepValue,StepID,AlterTime,NextStepID,UserID,Result,Mimo,StepCaption,IsEnd,IsStart,IsFinished)";
                    sql += "values(@WorkID,@FlowID,@StepValue,@StepID,@AlterTime,@NextStepID,@UserID,@Result,@Mimo,@StepCaption,@IsEnd,@IsStart,@IsFinished)";
                    db.ExecSql(sql, st, new PR[] {
                    new PR("@WorkID",WorkID),
                    new PR("@FlowID",FtFlow.FlowId),
                    new PR("@StepValue",int.Parse(statValue)),
                    new PR("@StepID",int.Parse(nextNodeId)),
                    new PR("@AlterTime",DateTime.Now),
                    new PR("@NextStepID",-10),
                    new PR("@UserID",UserID),
                    new PR("@Result",Const.FlowCaptionFinish),
                    new PR("@Mimo",""),
                    new PR("@StepCaption",nextStepCaption),
                    new PR("@IsEnd",1),
                    new PR("@IsStart",0),
                    new PR("@IsFinished",1),
                });
                    string ret = Operate(FtFlow.EndOperation);
                    if (ret != null) return ret;
                    Filter.End(db, st, FtFlow, WorkID, UserID, FlowDoc);
                }
                else
                {
                    //取下一步审批人并插入
                    sql = "insert into ft_flow_next(WorkID,FlowID,StepValue,StepID,AddTime,StepCaption)";
                    sql += "values(@WorkID,@FlowID,@StepValue,@StepID,@AddTime,@StepCaption)";
                    db.ExecSql(sql, st, new PR[] {
                    new PR("@WorkID",WorkID),
                    new PR("@FlowID",FtFlow.FlowId),
                    new PR("@StepValue",int.Parse(statValue)),
                    new PR("@StepID",int.Parse(nextNodeId)),
                    new PR("@AddTime",DateTime.Now),
                    new PR("@StepCaption",nextStepCaption),
                });
                    List<string> RoleUsers = null;
                    List<string> DepartUsers = null;
                    List<string> UserUsers = null;
                    List<string> CodeUsers = null;
                    if (!string.IsNullOrWhiteSpace(stepRole))
                    {
                        List<string> list = new();
                        var items = stepRole.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim());
                        foreach (var item in items)
                        {
                            list = list.Union(Project.Core.WorkFlow.User.UserFromRole(db, item)).ToList();
                        }
                        RoleUsers = list.Distinct().ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(stepDepart))
                    {
                        List<string> list = new();
                        var items = stepDepart.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim());
                        foreach (var item in items)
                        {
                            list = list.Union(Project.Core.WorkFlow.User.UserFromDepart(db, item)).ToList();
                        }
                        DepartUsers = list.Distinct().ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(stepUser))
                    {
                        List<string> list = new();
                        if (stepUser == "{para}")//手动指派，通过传参指定下一步审批人
                        {
                            stepUser = para;
                            list.AddRange(stepUser.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()));
                        }
                        else
                        {
                            var items = stepUser.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim());
                            foreach (var item in items)
                            {
                                var userId = Project.Core.WorkFlow.User.UserFromUser(db, item);
                                if (!string.IsNullOrEmpty(userId)) list.Add(userId);
                            }
                        }
                        UserUsers = list.Distinct().ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(stepCode))
                    {
                        CodeUsers = (List<string>)Project.Core.Code.Get(stepCode.Replace("{workid}", WorkID).Replace("{flowid}", FtFlow.FlowId).Replace("{nodeid}", nextNodeId).Replace("{para}", para), null);
                    }
                    List<string> nextUsers = RoleUsers ?? DepartUsers ?? UserUsers ?? CodeUsers;
                    if (nextUsers != null)
                    {
                        if (RoleUsers != null) nextUsers = nextUsers.Intersect(RoleUsers).ToList();
                        if (DepartUsers != null) nextUsers = nextUsers.Intersect(DepartUsers).ToList();
                        if (UserUsers != null) nextUsers = nextUsers.Intersect(UserUsers).ToList();
                        if (CodeUsers != null) nextUsers = nextUsers.Intersect(CodeUsers).ToList();
                        foreach (string userid in nextUsers)
                        {
                            sql = "insert into ft_flow_next_user(WorkID,FlowID,UserID)";
                            sql += "values(@WorkID,@FlowID,@UserID)";
                            db.ExecSql(sql, st, new PR[] {
                    new PR("@WorkID",WorkID),
                    new PR("@FlowID",FtFlow.FlowId),
                    new PR("@UserID",userid)
                });
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
        private string NodePrev(FtFlowNext ftFlowNext, string mimo = "", string para = "")
        {
            try
            {
                //找出上一步
                var flowHis = db.SelectOneBySQL<FtFlowHis>("select * from ft_flow_his where WorkID='" + WorkID.D2() + "' and FlowID='" + FtFlow.FlowId.D2() + "' and NextStepID='" + ftFlowNext.StepId + "' order by AlterTime desc");
                if (flowHis == null)
                {
                    return Const.NoPrevNodeFind;
                }
                else if (flowHis.IsStart == 1)
                {
                    return Const.FirstNodeCanNotBack;
                }
                string nodeId = flowHis.StepId.ToString();
                var curNode = FlowDoc.SelectSingleNode("/mxGraphModel/root/*[@id='" + nodeId + "']");
                if (curNode == null) return Const.NoNodeFindFor+" " + nodeId + "";
                string stepCaption = curNode.Attributes["label"].Value;

                //数据表处理
                //var souNode = curNode.SelectSingleNode("mxCell/mxGeometry");
                //string statValueSource = (souNode.Attributes["statValue"] == null || string.IsNullOrEmpty(souNode.Attributes["statValue"].Value)) ? nodeId : souNode.Attributes["statValue"].Value;

                string sql = "insert into ft_flow_his(WorkID,FlowID,StepValue,StepID,AlterTime,NextStepID,UserID,Result,Mimo,StepCaption,IsEnd,IsStart,IsFinished)";
                sql += "values(@WorkID,@FlowID,@StepValue,@StepID,@AlterTime,@NextStepID,@UserID,@Result,@Mimo,@StepCaption,@IsEnd,@IsStart,@IsFinished)";
                db.ExecSql(sql, st, new PR[] {
                    new PR("@WorkID",WorkID),
                    new PR("@FlowID",FtFlow.FlowId),
                    new PR("@StepValue",ftFlowNext.StepValue),
                    new PR("@StepID",ftFlowNext.StepId),
                    new PR("@AlterTime",DateTime.Now),
                    new PR("@NextStepID",-40),
                    new PR("@UserID",UserID),
                    new PR("@Result",Const.FlowCaptionPrev),
                    new PR("@Mimo",mimo),
                    new PR("@StepCaption",ftFlowNext.StepCaption),
                    new PR("@IsEnd",0),
                    new PR("@IsStart",0),
                    new PR("@IsFinished",0),
                });
                sql = "delete from ft_flow_next where WorkID='" + WorkID.D2() + "' and FlowID='" + FtFlow.FlowId.D2() + "'";
                db.ExecSql(sql, st);
                sql = "delete from ft_flow_next_user where WorkID='" + WorkID.D2() + "' and FlowID='" + FtFlow.FlowId.D2() + "'";
                db.ExecSql(sql, st);
                //operetion In
                var tarNode = curNode.SelectSingleNode("mxCell/mxGeometry");
                int isEnd = tarNode.Attributes["isEnd"] == null ? 0 : int.Parse(tarNode.Attributes["isEnd"].Value);
                string statValue = (tarNode.Attributes["statValue"] == null || string.IsNullOrEmpty(tarNode.Attributes["statValue"].Value)) ? nodeId : tarNode.Attributes["statValue"].Value;
                string operateIn = tarNode.Attributes["operateIn"]?.Value;
                string stepRole = tarNode.Attributes["stepRole"]?.Value;
                string stepDepart = tarNode.Attributes["stepDepart"]?.Value;
                string stepUser = tarNode.Attributes["stepUser"]?.Value;
                string stepCode = tarNode.Attributes["stepCode"]?.Value;
                if (operateIn != null)
                {
                    var ret = Operate(operateIn);
                    if (ret != null) return ret;
                }
                //状态值写入
                if (!string.IsNullOrEmpty(FtFlow.StatColumn))
                {
                    sql = "update " + FtFlow.MainTable.D2() + " set " + FtFlow.StatColumn.D2() + "='" + statValue.D2() + "' where " + FtFlow.MainTableKey.D2() + "='" + WorkID.D2() + "'";
                    db.ExecSql(sql, st);
                }
                if (!string.IsNullOrEmpty(FtFlow.StatCaptionColumn))
                {
                    string caption = GetCaptionByStat(FtFlow.StatCaptionEnum, int.Parse(statValue));
                    if (caption != null)
                    {
                        sql = "update " + FtFlow.MainTable.D2() + " set " + FtFlow.StatCaptionColumn.D2() + "='" + caption.D2() + "' where " + FtFlow.MainTableKey.D2() + "='" + WorkID.D2() + "'";
                        db.ExecSql(sql, st);
                    }
                }
                //取下一步审批人并插入
                sql = "insert into ft_flow_next(WorkID,FlowID,StepValue,StepID,AddTime,StepCaption)";
                sql += "values(@WorkID,@FlowID,@StepValue,@StepID,@AddTime,@StepCaption)";
                db.ExecSql(sql, st, new PR[] {
                    new PR("@WorkID",WorkID),
                    new PR("@FlowID",FtFlow.FlowId),
                    new PR("@StepValue",int.Parse(statValue)),
                    new PR("@StepID",int.Parse(nodeId)),
                    new PR("@AddTime",DateTime.Now),
                    new PR("@StepCaption",stepCaption),
                });
                List<string> RoleUsers = null;
                List<string> DepartUsers = null;
                List<string> UserUsers = null;
                List<string> CodeUsers = null;
                if (!string.IsNullOrWhiteSpace(stepRole))
                {
                    List<string> list = new();
                    var items = stepRole.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim());
                    foreach (var item in items)
                    {
                        list = list.Union(Project.Core.WorkFlow.User.UserFromRole(db, item)).ToList();
                    }
                    RoleUsers = list.Distinct().ToList();
                }
                if (!string.IsNullOrWhiteSpace(stepDepart))
                {
                    List<string> list = new();
                    var items = stepDepart.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim());
                    foreach (var item in items)
                    {
                        list = list.Union(Project.Core.WorkFlow.User.UserFromDepart(db, item)).ToList();
                    }
                    DepartUsers = list.Distinct().ToList();
                }
                if (!string.IsNullOrWhiteSpace(stepUser))
                {
                    List<string> list = new();
                    if (stepUser == "{para}")//手动指派，通过传参指定下一步审批人
                    {
                        stepUser = para;
                        list.AddRange(stepUser.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()));
                    }
                    else
                    {
                        var items = stepUser.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim());
                        foreach (var item in items)
                        {
                            var userId = Project.Core.WorkFlow.User.UserFromUser(db, item);
                            if (!string.IsNullOrEmpty(userId)) list.Add(userId);
                        }
                    }
                    UserUsers = list.Distinct().ToList();
                }
                if (!string.IsNullOrWhiteSpace(stepCode))
                {
                    CodeUsers = (List<string>)Project.Core.Code.Get(stepCode.Replace("{workid}", WorkID).Replace("{flowid}", FtFlow.FlowId).Replace("{nodeid}", nodeId).Replace("{para}", para), null);
                }
                List<string> nextUsers = RoleUsers ?? DepartUsers ?? UserUsers ?? CodeUsers;
                if (nextUsers != null)
                {
                    if (RoleUsers != null) nextUsers = nextUsers.Intersect(RoleUsers).ToList();
                    if (DepartUsers != null) nextUsers = nextUsers.Intersect(DepartUsers).ToList();
                    if (UserUsers != null) nextUsers = nextUsers.Intersect(UserUsers).ToList();
                    if (CodeUsers != null) nextUsers = nextUsers.Intersect(CodeUsers).ToList();
                    foreach (string userid in nextUsers)
                    {
                        sql = "insert into ft_flow_next_user(WorkID,FlowID,UserID)";
                        sql += "values(@WorkID,@FlowID,@UserID)";
                        db.ExecSql(sql, st, new PR[] {
                    new PR("@WorkID",WorkID),
                    new PR("@FlowID",FtFlow.FlowId),
                    new PR("@UserID",userid)
                });
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
        /*
        private List<string> UserFromRole(DB db, string RoleName)
        {
            try
            {
                List<string> al = new();
                string sql = "select a.USERID from BD_USER_ROLE a,TB_ROLE b where a.ROLEID=b.ROLEID and b.ROLENAME='" + str.D2DD(RoleName) + "'";
                using (DR dr = db.OpenRecord(sql))
                {
                    while (dr.Read())
                    {
                        al.Add(dr.GetString(0));
                    }
                }
                return al;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new();
            }
        }
        private List<string> UserFromDepart(DB db, string DepartName)
        {
            try
            {
                List<string> al = new();
                string sql = "select a.USERID from BD_USER_DEPART a,TB_DEPARTINFO b where a.DEPARTID=b.DEPARTID and b.DEPARTNAME='" + str.D2DD(DepartName) + "'";
                using (DR dr = db.OpenRecord(sql))
                {
                    while (dr.Read())
                    {
                        al.Add(dr.GetString(0));
                    }
                }
                return al;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new();
            }
        }
        private string UserFromUser(DB db, string RealName)
        {
            try
            {
                string sql = "select USERID from TB_USERINFO where REALNAME='" + str.D2DD(RealName) + "'";
                return db.GetStringForceNoNull(sql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }*/
        public string Operate(string setStr)
        {
            try
            {
                var items = setStr.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim());
                foreach (var item in items)
                {
                    if (item.StartsWith("@code("))
                    {
                        var ret = Project.Core.Code.Get(item.Replace("{workid}", WorkID).Replace("{flowid}", FtFlow.FlowId), null);
                        if (ret != null) return ret.ToString();
                    }
                    else if (item.IndexOf('=') > 0)
                    {
                        var colSet = item.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()).ToArray();
                        string sql = "update " + FtFlow.MainTable.D2() + " set " + colSet[0].D2() + "='" + colSet[1].D2() + "' where " + FtFlow.MainTableKey.D2() + "='" + WorkID.D2() + "'";
                        db.ExecSql(sql, st);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
        private string GetCaptionByStat(string setStr, int stat)
        {
            var items = setStr.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim());
            foreach (var item in items)
            {
                int lastI = item.LastIndexOf('=');
                if (lastI > 0)
                {
                    string caption = item.Substring(lastI + 1).Trim();
                    bool cdnOK = false;
                    string setS = item.Substring(0, lastI).Trim();
                    var setItems = setS.Split(new char[] { '&', '|' }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim());
                    if (setS.IndexOf('&') > 0)
                    {
                        foreach (var s in setItems)
                        {
                            if (!CdnOK(s, stat))
                            {
                                cdnOK = false;
                                break;
                            }
                        }
                    }
                    else if (setS.IndexOf('|') > 0)
                    {
                        foreach (var s in setItems)
                        {
                            if (CdnOK(s, stat))
                            {
                                cdnOK = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        cdnOK = CdnOK(setS, stat);
                    }
                    if (cdnOK) return caption;
                }
            }
            return null;
            bool CdnOK(string set, int stat)
            {
                if (set.StartsWith(">=")) return stat >= int.Parse(set.Substring(2));
                else if (set.StartsWith("<=")) return stat <= int.Parse(set.Substring(2));
                else if (set.StartsWith("!=")) return stat != int.Parse(set.Substring(2));
                else if (set.StartsWith(">")) return stat > int.Parse(set.Substring(1));
                else if (set.StartsWith("<")) return stat < int.Parse(set.Substring(1));
                else if (set.StartsWith("=")) return stat == int.Parse(set.Substring(1));
                else return stat == int.Parse(set);
            }
        }
        private bool SQLConditionOK(string sqlctn, int sqllr, string sqlval)
        {
            if (string.IsNullOrWhiteSpace(sqlctn) || sqllr == 0 || sqlval == null) return true;
            string v = db.GetStringForceNoNull(sqlctn);
            int val = 0;
            bool isIntValue = int.TryParse(sqlval, out val);
            if (sqllr == 0)//<
            {
                return (v != null) && (isIntValue ? int.Parse(v) < val : false);
            }
            else if (sqllr == 1)//<=
            {
                return (v != null) && (isIntValue ? int.Parse(v) <= val : false);
            }
            else if (sqllr == 2)//=
            {
                return ((v == null) && (sqlval.Equals("[NULL]"))) || ((v != null) && (isIntValue ? int.Parse(v) == val : v.Equals(sqlval)));
            }
            else if (sqllr == 3)//>=
            {
                return (v != null) && (isIntValue ? int.Parse(v.ToString()) >= val : false);
            }
            else if (sqllr == 4)//>
            {
                return (v != null) && (isIntValue ? int.Parse(v.ToString()) > val : false);
            }
            else if (sqllr == 5)//!=
            {
                return ((v == null) && (!sqlval.Equals("[NULL]"))) || ((v != null) && (isIntValue ? int.Parse(v) != val : !v.Equals(sqlval)));
            }
            return true;
        }
    }
    public class FlowOld
    {
        private static int MaxActionAutoLimited = 0;
        public static Hashtable DesignHT = new Hashtable();
        public static XmlDocument DesignDoc(string SiteID, string DataSource)
        {
            string TableName = "ft_" + SiteID + "_flowdesign_" + DataSource;
            if (DesignHT.ContainsKey(TableName)) return (XmlDocument)DesignHT[TableName];
            DB db = new DB();
            db.Open();
            try
            {
                XmlDocument doc = new XmlDocument();
                string sql = "select content from " + TableName + "";
                DR dr = db.OpenRecord(sql);
                if (dr.Read())
                {
                    doc.LoadXml(dr.GetString(0));
                }
                dr.Close();
                DesignHT.Add(TableName, doc);
                return doc;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DesignHT.Add(TableName, new XmlDocument());
                return new XmlDocument();
            }
            finally
            {
                db.Close();
            }
        }
        public static string Action(string SiteID, DB db, ST st, string fid, int FlowStatType, int lockflow, string FlowLockPos, string FlowDesignData, string FlowSubFlowID, string BindTable, HttpContext Context, string TableFlog, int _Type, string MemName)
        {
            MaxActionAutoLimited = 0;
            string sql = null;
            if (FlowStatType == 0)//固定状态值
            {
                int lockpos = 0;
                int.TryParse(FlowLockPos, out lockpos);
                sql = "update " + str.D2DD(BindTable) + " set flow=" + lockflow + ",flowpos=" + lockpos + " where fid='" + str.D2DD(fid) + "'";
                db.ExecSql(sql, st);
                //Form.NewFLLogV4(db, st, Context, TableFlog, fid, _Type, BindTable, MemName, lockflow, lockpos);
            }
            else if (FlowStatType == 1)//退回到上一步
            {
                sql = "select flow from " + str.D2DD(BindTable) + " where fid='" + str.D2DD(fid) + "'";
                DR dr = db.OpenRecord(sql, st);
                int curflow = 0;
                if (dr.Read())
                {
                    curflow = dr.GetInt32(0);
                }
                dr.Close();
                if (curflow == 0)
                {
                    return "该流程状态为初始状态，不能退回到上一步";
                }
                sql = "select fvalue,fpos,addtime,ftype from " + str.D2DD(TableFlog) + " where fid='" + str.D2DD(fid) + "' and binddata='" + str.D2DD(BindTable) + "' order by addtime desc";

                dr = db.OpenRecord(sql, st);
                if (dr.Read())
                {
                    int _fvalue = int.Parse(dr.GetValue(0).ToString());
                    string _addtime = dr.GetValue(2).ToString();
                    int _ftype = int.Parse(dr.GetValue(3).ToString());
                    int __fvalue = 0;
                    int __fpos = 0;
                    if (dr.Read())
                    {
                        __fvalue = int.Parse(dr.GetValue(0).ToString());
                        __fpos = int.Parse(dr.GetValue(1).ToString());
                    }
                    dr.Close();
                    sql = "update " + str.D2DD(BindTable) + " set flow=" + __fvalue + ",flowpos=" + __fpos + " where fid='" + str.D2DD(fid) + "'";
                    db.ExecSql(sql, st);
                    sql = "delete from " + str.D2DD(TableFlog) + " where fid='" + str.D2DD(fid) + "' and binddata='" + str.D2DD(BindTable) + "' and ftype=" + _ftype + " and addtime='" + str.D2DD(_addtime) + "' and fvalue=" + _fvalue;
                    db.ExecSql(sql, st);
                }
                else
                {
                    dr.Close();
                    return "该流程没有上一步的状态日志";
                }
            }
            else if (FlowStatType == 2)//流程构件
            {
                XmlDocument doc = DesignDoc(SiteID, FlowDesignData);
                if (doc.InnerXml == null || doc.InnerXml.Equals("")) return "数据源为'" + FlowDesignData + "'的流程设计并不存在或还未设计";
                sql = "select flow,flowpos from " + str.D2DD(BindTable) + " where fid='" + str.D2DD(fid) + "'";
                DR dr = db.OpenRecord(sql, st);
                if (dr.Read())
                {
                    int curflow = int.Parse(dr.GetValue(0).ToString());
                    int curpos = int.Parse(dr.GetValue(1).ToString());
                    dr.Close();
                    if (curpos == 0)
                    {
                        XmlNode node = doc.SelectSingleNode("//mxGeometry[@stattype='0' and @statvalue='0']");
                        if (node == null)
                        {
                            return "未能找到流程起始节点（固定状态值为0）";
                        }
                        curpos = int.Parse(node.ParentNode.ParentNode.Attributes["id"].Value);
                    }
                    XmlNode PosNode = doc.SelectSingleNode("/mxGraphModel/root/*[@id='" + curpos + "']");
                    if (PosNode == null) return "未能找到位置为'" + curpos + "'的流程节点";
                    if ("|Rect|Roundrect|Shape|Image|".IndexOf("|" + PosNode.Name + "|") < 0)
                    {
                        return "流程位置不正确，必须为对象类型";
                    }
                    XmlNodeList ArrawNodes = doc.SelectNodes("//Connector/mxCell[@source='" + curpos + "']");
                    if (ArrawNodes.Count == 0) return "没有下一步流程分支，无法继续";
                    int subflowid = 0;
                    if (ArrawNodes.Count == 1 || int.TryParse(FlowSubFlowID, out subflowid))//固定分支
                    {
                        XmlNode ArrawNode = null;
                        if (ArrawNodes.Count > 1)
                        {
                            ArrawNode = doc.SelectSingleNode("//Connector[@id='" + subflowid + "']/mxCell[@source='" + curpos + "']");
                            if (ArrawNode == null) return "设定的固定分支不存在";
                        }
                        else
                        {
                            ArrawNode = ArrawNodes[0];
                        }
                        int target = 0;
                        if (ArrawNode.Attributes["target"] == null) return "指定的固定分支不存在目标节点";
                        target = int.Parse(ArrawNode.Attributes["target"].Value);
                        int mustlogin = 1;
                        int flowtype = 0;
                        XmlNode ArrawDataNode = ArrawNode.SelectSingleNode("mxGeometry");
                        if (ArrawDataNode.Attributes["mustlogin"] != null)
                        {
                            mustlogin = int.Parse(ArrawDataNode.Attributes["mustlogin"].Value);
                        }
                        if (ArrawDataNode.Attributes["flowtype"] != null)
                        {
                            flowtype = int.Parse(ArrawDataNode.Attributes["flowtype"].Value);
                        }
                        string sqlctn = null;
                        string sqllr = null;
                        string sqlval = null;
                        string ctncap = "";
                        if (ArrawDataNode.Attributes["sqlctn"] != null)
                        {
                            sqlctn = ArrawDataNode.Attributes["sqlctn"].Value;
                        }
                        if (ArrawDataNode.Attributes["sqllr"] != null)
                        {
                            sqllr = ArrawDataNode.Attributes["sqllr"].Value;
                        }
                        if (ArrawDataNode.Attributes["sqlval"] != null)
                        {
                            sqlval = ArrawDataNode.Attributes["sqlval"].Value;
                        }
                        if (ArrawDataNode.Attributes["ctncap"] != null)
                        {
                            ctncap = ArrawDataNode.Attributes["ctncap"].Value;
                        }
                        if (mustlogin == 1 && !IsMemberLogin(Context, MemName)) return "必须登录才能进行此操作";
                        if (!SQLConditionOK(db, st, Context, SiteID, sqlctn, sqllr, sqlval, fid, curflow))
                        {
                            return "没有满足流转条件：" + ctncap;
                        }
                        PosNode = doc.SelectSingleNode("/mxGraphModel/root/*[@id='" + target + "']");
                        if (PosNode == null) return "未能找到位置为'" + curpos + "'的目标流程节点";
                        if ("|Rect|Roundrect|Shape|Image|".IndexOf("|" + PosNode.Name + "|") < 0)
                        {
                            return "目标流程位置不正确，必须为对象类型";
                        }
                        XmlNode Node = PosNode.SelectSingleNode("mxCell/mxGeometry");
                        if (Node == null) return "mxGeometry 节点不存在 " + PosNode.Name;
                        int stattype = 0;
                        int statvalue = 0;
                        if (Node.Attributes["stattype"] != null) stattype = int.Parse(Node.Attributes["stattype"].Value);
                        if (Node.Attributes["statvalue"] == null || !int.TryParse(Node.Attributes["statvalue"].Value, out statvalue))
                        {
                            return "节点状态值未设置正确";
                        }
                        if (stattype == 0)
                        {
                            sql = "update " + str.D2DD(BindTable) + " set flow=" + statvalue + ",flowpos=" + target + " where fid='" + str.D2DD(fid) + "'";
                            db.ExecSql(sql, st);
                            //Form.NewFLLogV4(db, st, Context, TableFlog, fid, _Type, BindTable, MemName, statvalue, target);
                            ActionAuto(SiteID, db, st, fid, doc, target, BindTable, Context, TableFlog, _Type, MemName, statvalue);
                        }
                        else if (stattype == 1)
                        {
                            if (HaveAdded4Pos(db, st, MemName, TableFlog, BindTable, fid, curpos, target))
                            {
                                return "你已经操作过该累加状态了";
                            }
                            else
                            {
                                sql = "update " + str.D2DD(BindTable) + " set flow=" + (curflow + statvalue) + ",flowpos=" + target + " where fid='" + str.D2DD(fid) + "'";
                                db.ExecSql(sql, st);
                                //Form.NewFLLogV4(db, st, Context, TableFlog, fid, _Type, BindTable, MemName, curflow + statvalue, target);
                                ActionAuto(SiteID, db, st, fid, doc, target, BindTable, Context, TableFlog, _Type, MemName, curflow + statvalue);
                            }
                        }
                    }
                    else//自动分支
                    {
                        string PassArrawID = null;
                        int _target = 0;
                        int _statvalue = 0;
                        int _stattype = 0;
                        foreach (XmlNode ArrawNode in ArrawNodes)
                        {
                            int target = 0;
                            if (ArrawNode.Attributes["target"] == null)
                            {
                                log.Debug("指定的固定分支不存在目标节点", fid);
                                continue;
                            }
                            target = int.Parse(ArrawNode.Attributes["target"].Value);
                            int mustlogin = 1;
                            int flowtype = 0;
                            XmlNode ArrawDataNode = ArrawNode.SelectSingleNode("mxGeometry");
                            if (ArrawDataNode.Attributes["mustlogin"] != null)
                            {
                                mustlogin = int.Parse(ArrawDataNode.Attributes["mustlogin"].Value);
                            }
                            if (ArrawDataNode.Attributes["flowtype"] != null)
                            {
                                flowtype = int.Parse(ArrawDataNode.Attributes["flowtype"].Value);
                            }
                            string sqlctn = null;
                            string sqllr = null;
                            string sqlval = null;
                            string ctncap = "";
                            if (ArrawDataNode.Attributes["sqlctn"] != null)
                            {
                                sqlctn = ArrawDataNode.Attributes["sqlctn"].Value;
                            }
                            if (ArrawDataNode.Attributes["sqllr"] != null)
                            {
                                sqllr = ArrawDataNode.Attributes["sqllr"].Value;
                            }
                            if (ArrawDataNode.Attributes["sqlval"] != null)
                            {
                                sqlval = ArrawDataNode.Attributes["sqlval"].Value;
                            }
                            if (ArrawDataNode.Attributes["ctncap"] != null)
                            {
                                ctncap = ArrawDataNode.Attributes["ctncap"].Value;
                            }
                            if (mustlogin == 1 && !IsMemberLogin(Context, MemName))
                            {
                                log.Debug("必须登录才能进行此操作", fid);
                                continue;
                            }
                            if (!SQLConditionOK(db, st, Context, SiteID, sqlctn, sqllr, sqlval, fid, curflow))
                            {
                                log.Debug("没有满足流转条件：" + ctncap, fid);
                                continue;
                            }
                            PosNode = doc.SelectSingleNode("/mxGraphModel/root/*[@id='" + target + "']");
                            if (PosNode == null)
                            {
                                log.Debug("未能找到位置为'" + curpos + "'的目标流程节点", fid);
                                continue;
                            }
                            if ("|Rect|Roundrect|Shape|Image|".IndexOf("|" + PosNode.Name + "|") < 0)
                            {
                                log.Debug("目标流程位置不正确，必须为对象类型", fid);
                                continue;
                            }
                            XmlNode Node = PosNode.SelectSingleNode("mxCell/mxGeometry");
                            if (Node == null)
                            {
                                log.Debug("mxGeometry 节点不存在 " + PosNode.Name, fid);
                                continue;
                            }
                            int stattype = 0;
                            int statvalue = 0;
                            if (Node.Attributes["stattype"] != null) stattype = int.Parse(Node.Attributes["stattype"].Value);
                            if (Node.Attributes["statvalue"] == null || !int.TryParse(Node.Attributes["statvalue"].Value, out statvalue))
                            {
                                log.Debug("节点状态值未设置正确", fid);
                                continue;
                            }
                            if (PassArrawID != null)
                            {
                                return "至少有两个分支同时满足条件，请确认条件正确或设置固定分支";
                            }
                            else
                            {
                                PassArrawID = ArrawNode.ParentNode.Attributes["id"].Value;
                                _target = target;
                                _stattype = stattype;
                                _statvalue = statvalue;
                            }
                        }
                        if (PassArrawID == null) return "没有找到符合条件的分支，不能继续";
                        if (_stattype == 0)
                        {
                            sql = "update " + str.D2DD(BindTable) + " set flow=" + _statvalue + ",flowpos=" + _target + " where fid='" + str.D2DD(fid) + "'";
                            db.ExecSql(sql, st);
                            //Form.NewFLLogV4(db, st, Context, TableFlog, fid, _Type, BindTable, MemName, _statvalue, _target);
                            ActionAuto(SiteID, db, st, fid, doc, _target, BindTable, Context, TableFlog, _Type, MemName, _statvalue);
                        }
                        else if (_stattype == 1)
                        {
                            if (HaveAdded4Pos(db, st, MemName, TableFlog, BindTable, fid, curpos, _target))
                            {
                                return "你已经操作过该累加状态了";
                            }
                            else
                            {
                                sql = "update " + str.D2DD(BindTable) + " set flow=" + (curflow + _statvalue) + ",flowpos=" + _target + " where fid='" + str.D2DD(fid) + "'";
                                db.ExecSql(sql, st);
                                //Form.NewFLLogV4(db, st, Context, TableFlog, fid, _Type, BindTable, MemName, curflow + _statvalue, _target);
                                ActionAuto(SiteID, db, st, fid, doc, _target, BindTable, Context, TableFlog, _Type, MemName, curflow + _statvalue);
                            }
                        }
                    }
                }
                else
                {
                    dr.Close();
                    return "该条记录并不存在";
                }
            }
            return null;
        }
        private static void ActionAuto(string SiteID, DB db, ST st, string fid, XmlDocument doc, int curpos, string BindTable, HttpContext Context, string TableFlog, int _Type, string MemName, int curflow)
        {
            if (MaxActionAutoLimited > 99)
            {
                log.Debug("流程自动流转次数超过99次，确认没有构成死循环", MaxActionAutoLimited.ToString());
                return;
            }
            MaxActionAutoLimited++;
            string sql = null;
            XmlNodeList ArrawNodes = doc.SelectNodes("//Connector/mxCell[@source='" + curpos + "']");
            if (ArrawNodes.Count == 0) return;
            string PassArrawID = null;
            int _target = 0;
            int _statvalue = 0;
            int _stattype = 0;
            foreach (XmlNode ArrawNode in ArrawNodes)
            {
                int target = 0;
                if (ArrawNode.Attributes["target"] == null)
                {
                    continue;
                }
                target = int.Parse(ArrawNode.Attributes["target"].Value);
                int mustlogin = 1;
                int flowtype = 0;
                XmlNode ArrawDataNode = ArrawNode.SelectSingleNode("mxGeometry");
                if (ArrawDataNode.Attributes["flowtype"] != null)
                {
                    flowtype = int.Parse(ArrawDataNode.Attributes["flowtype"].Value);
                }
                if (flowtype == 0) continue;//只考虑自动流转
                if (ArrawDataNode.Attributes["mustlogin"] != null)
                {
                    mustlogin = int.Parse(ArrawDataNode.Attributes["mustlogin"].Value);
                }
                string sqlctn = null;
                string sqllr = null;
                string sqlval = null;
                string ctncap = "";
                if (ArrawDataNode.Attributes["sqlctn"] != null)
                {
                    sqlctn = ArrawDataNode.Attributes["sqlctn"].Value;
                }
                if (ArrawDataNode.Attributes["sqllr"] != null)
                {
                    sqllr = ArrawDataNode.Attributes["sqllr"].Value;
                }
                if (ArrawDataNode.Attributes["sqlval"] != null)
                {
                    sqlval = ArrawDataNode.Attributes["sqlval"].Value;
                }
                if (ArrawDataNode.Attributes["ctncap"] != null)
                {
                    ctncap = ArrawDataNode.Attributes["ctncap"].Value;
                }
                if (mustlogin == 1 && !IsMemberLogin(Context, MemName))
                {
                    continue;
                }
                if (!SQLConditionOK(db, st, Context, SiteID, sqlctn, sqllr, sqlval, fid, curflow))
                {
                    continue;
                }
                XmlNode PosNode = doc.SelectSingleNode("/mxGraphModel/root/*[@id='" + target + "']");
                if (PosNode == null)
                {
                    continue;
                }
                if ("|Rect|Roundrect|Shape|Image|".IndexOf("|" + PosNode.Name + "|") < 0)
                {
                    continue;
                }
                XmlNode Node = PosNode.SelectSingleNode("mxCell/mxGeometry");
                if (Node == null)
                {
                    continue;
                }
                int stattype = 0;
                int statvalue = 0;
                if (Node.Attributes["stattype"] != null) stattype = int.Parse(Node.Attributes["stattype"].Value);
                if (Node.Attributes["statvalue"] == null || !int.TryParse(Node.Attributes["statvalue"].Value, out statvalue))
                {
                    continue;
                }
                if (PassArrawID != null)
                {
                    log.Debug("至少有两个分支同时满足条件，请确认条件正确或设置固定分支", fid);
                    return;
                }
                else
                {
                    PassArrawID = ArrawNode.ParentNode.Attributes["id"].Value;
                    _target = target;
                    _stattype = stattype;
                    _statvalue = statvalue;
                }
            }
            if (PassArrawID == null)
            {
                log.Debug("没有找到符合条件的分支，不能继续", fid);
                return;
            }
            if (_stattype == 0)
            {
                sql = "update " + str.D2DD(BindTable) + " set flow=" + _statvalue + ",flowpos=" + _target + " where fid='" + str.D2DD(fid) + "'";
                db.ExecSql(sql, st);
                //Form.NewFLLogV4(db, st, Context, TableFlog, fid, _Type, BindTable, MemName, _statvalue, _target);
                ActionAuto(SiteID, db, st, fid, doc, _target, BindTable, Context, TableFlog, _Type, MemName, _statvalue);
            }
            else if (_stattype == 1)
            {
                if (HaveAdded4Pos(db, st, MemName, TableFlog, BindTable, fid, curpos, _target))
                {
                    log.Debug("你已经操作过该累加状态了", MemName);
                }
                else
                {
                    sql = "update " + str.D2DD(BindTable) + " set flow=" + (curflow + _statvalue) + ",flowpos=" + _target + " where fid='" + str.D2DD(fid) + "'";
                    db.ExecSql(sql, st);
                    //Form.NewFLLogV4(db, st, Context, TableFlog, fid, _Type, BindTable, MemName, curflow + _statvalue, _target);
                    ActionAuto(SiteID, db, st, fid, doc, _target, BindTable, Context, TableFlog, _Type, MemName, curflow + _statvalue);
                }
            }
        }
        private static bool IsMemberLogin(HttpContext context, string MemName)
        {
            return MemName != null && !MemName.Equals("");
            //object memname = context.Session["asso_name"];
            //return memname != null && !memname.Equals("");
        }
        private static bool HaveAdded4Pos(DB db, ST st, string memname, string TableFlog, string BindTable, string fid, int curpos, int fpos)
        {
            string sql = "select fmem,fpos from " + str.D2DD(TableFlog) + " where fid='" + str.D2DD(fid) + "' and binddata='" + str.D2DD(BindTable) + "' order by addtime desc";
            DR dr = db.OpenRecord(sql, st);
            while (dr.Read())
            {
                //if (int.Parse(dr.GetValue(1).ToString()) != fpos)
                if (int.Parse(dr.GetValue(1).ToString()) != curpos)
                {
                    dr.Close();
                    return false;
                }
                if (dr.GetString(0).Equals(memname))
                {
                    dr.Close();
                    return true;
                }
            }
            dr.Close();
            return false;
        }
        private static bool SQLConditionOK(DB db, ST st, HttpContext context, string SiteID, string sqlctn, string _sqllr, string sqlval, string fid, int flow)
        {
            if (sqlctn == null || _sqllr == null || sqlval == null) return true;
            if (sqlctn.Trim().Equals("") || _sqllr.Trim().Equals("") || sqlval.Trim().Equals("")) return true;
            //sqlctn = adv.GetSpecialBase(context, sqlctn, SiteID).Replace("[fid]", fid).Replace("[flow]", flow.ToString()).Trim();
            object v = null;
            DR dr = db.OpenRecord(sqlctn, st);
            if (dr.Read())
            {
                v = dr.GetValue(0);
            }
            dr.Close();
            int sqllr = int.Parse(_sqllr);
            int val = 0;
            bool isIntValue = int.TryParse(sqlval, out val);
            if (sqllr == 0)//<
            {
                return (v != null) && (isIntValue ? int.Parse(v.ToString()) < val : false);
            }
            else if (sqllr == 1)//<=
            {
                return (v != null) && (isIntValue ? int.Parse(v.ToString()) <= val : false);
            }
            else if (sqllr == 2)//=
            {
                return ((v == null) && (sqlval.Equals("[NULL]"))) || ((v != null) && (isIntValue ? int.Parse(v.ToString()) == val : v.ToString().Equals(sqlval)));
            }
            else if (sqllr == 3)//>=
            {
                return (v != null) && (isIntValue ? int.Parse(v.ToString()) >= val : false);
            }
            else if (sqllr == 4)//>
            {
                return (v != null) && (isIntValue ? int.Parse(v.ToString()) > val : false);
            }
            return true;
        }
    }
}
