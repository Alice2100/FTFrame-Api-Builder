using System;
using System.Collections.Generic;
using System.Text;
using FTFrame;
using FTFrame.Tool;
using FTFrame.DBClient;
using System.Linq;
using System.IO;
using System.Xml;
using System.Collections;
using CoreHttp = Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using FTFrame.Model;

namespace FTFrame.Project.Core.WorkFlow
{
    /// <summary>
    /// The flow of processes will call these methods
    /// The process function requires support from process components and process pages, which is not supported in the Api development version
    /// </summary>
    public class Filter
    {
        public static void Join(DB db,ST st,FtFlowList FtFlow,string WorkID,string UserID,XmlDocument FlowDoc)
        {

        }
        public static void End(DB db, ST st, FtFlowList FtFlow, string WorkID, string UserID, XmlDocument FlowDoc)
        {

        }
        public static void Judge(DB db, ST st, FtFlowList FtFlow, string WorkID, string UserID, Enums.FlowJudge judge, XmlDocument FlowDoc,XmlNode Node)
        {

        }
        public static void Cancel(DB db, ST st, FtFlowList FtFlow, string WorkID, string UserID)
        {

        }
    }
    public class Const
    {
        public static string NoThisFlow = "没有这个流程";//No This Flow
        public static string FlowNotFinished = "已有流程且未结束";//Flow Not Finished
        public static string NoStartNode = "没有起始节点";//No Start Node
        public static string NoNodeFindInDB = "配置中未找到该节点";//Can Not Find Current Node In DB
        public static string NoNodeFindFor = "该节点未找到";//Can Not Find Current Node For 
        public static string YouCanNotJudgeThisNode = "你不能审批该节点";//You Can Not Judge This Node
        public static string NextStepsCountNotBe0 = "下一步分支数量不能为零";//Next Steps Count Not Be 0
        public static string MoreThenOneNextStepsSatisfied = "大于一个的下一步分支被同时满足条件";//More Then One Next Steps Satisfied
        public static string FlowCaptionSubmit = "提交审批";
        public static string FlowCaptionFinish = "审批完成";
        public static string FlowCaptionPass = "审批通过";
        public static string FlowCaptionCancel = "审批取消";
        public static string FlowCaptionPrev = "退回到上一步";
        public static string FlowCaptionReject = "审批拒绝";
        public static string NoPrevNodeFind = "未找到上一个审批节点";//Can Not Find Prev Node
        public static string FirstNodeCanNotBack = "起始审批节点不能退回到上一步";//First Node Can not be Back
    }
    public class User
    {
        public static List<string> UserFromRole(DB db, string RoleName)
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
        public static List<string> UserFromDepart(DB db, string DepartName)
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
        public static string UserFromUser(DB db, string RealName)
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
        }
    }
    public class Obj
    {
        public enum WFCate
        {
            演示 = 1,
            采购需求审批 = 2,
            采购订单审批 = 3
        }

        public enum JudgeState
        {
            未发起审批 = 0,
            待审批 = 1,
            审批中 = 2,
            审批通过 = 10,
            审批未通过 = 99
        }
    }
    public class Code
    {
        public static string _str(string code, string[] para, HttpContext Context)
        {
            if (code == "XmlValidate")
            {
                return XmlValidate(Context, para[0]);
            }
            if (code == "FlowStat")
            {
                switch (int.Parse(para[0]))
                {
                    case 0:
                        return "未发起审批";
                    case 1:
                        return "待审批";
                    case 2:
                        return "审批中";
                    case 10:
                        return "审批通过";
                    case 99:
                        return "审批未通过";
                    default:
                        return "其他";
                }
            }
            if (code == "FlowSelect")
            {
                return "" + "<option value=0>未发起审批</option>" + "<option value=1>待审批</option>" + "<option value=2>审批中</option>" + "<option value=10>审批通过</option>" + "<option value=99>审批未通过</option>";
            }
            if (code == "StartFlowButton")
            {
                string s = "";
                int flowid = int.Parse(para[0]);
                int flow = int.Parse(para[1]);
                if (flow == 0)
                {
                    s = s + "<input type=button class='layui-btn _button' value='发起审批' onclick=\"startFlow(" + flowid + ",'" + para[2] + "')\"/>";
                }
                if (flow == 2)
                {
                    s = s + "<input type=button class='layui-btn _button' value='重置' onclick=\"resetFlow(" + flowid + ",'" + para[2] + "')\"/>";
                }
                if (flow == 1 || flow == 2 || flow == 99)
                {
                    if (!s.Equals(""))
                    {
                        s += "&nbsp;&nbsp;";
                    }
                    s = s + "<input type=button class='layui-btn _button' value='取消' onclick=\"cancelFlow(" + flowid + ",'" + para[2] + "')\"/>";
                }
                return s;
            }
            return "(nocode)";
        }
        public static object _obj(string code, string[] para, HttpContext Context)
        {
            if (code == "CustomJudgeUsers")
            {
                return CustomJudgeUsers(para[0], para[1], int.Parse(para[2]));
            }
            return "(nocode)";
        }


        public static string XmlValidate(HttpContext context, string key)
        {
            try
            {
                string xml = context.Request.FormString(key);
                if (string.IsNullOrEmpty(xml)) return "流程设置不能为空";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNodeList Rect_Start = doc.SelectNodes("//Rect/mxCell/mxGeometry[@isStart='1']");
                XmlNodeList Roundrect_Start = doc.SelectNodes("//Roundrect/mxCell/mxGeometry[@isStart='1']");
                XmlNodeList Shape_Start = doc.SelectNodes("//Shape/mxCell/mxGeometry[@isStart='1']");
                XmlNodeList Image_Start = doc.SelectNodes("//Image/mxCell/mxGeometry[@isStart='1']");
                int StartCount = Rect_Start.Count + Roundrect_Start.Count + Shape_Start.Count + Image_Start.Count;
                if (StartCount == 0) return "必须设置一个开始节点";
                else if (StartCount > 1) return "只能设置一个开始节点";
                XmlNodeList Rect_End = doc.SelectNodes("//Rect/mxCell/mxGeometry[@isEnd='1']");
                XmlNodeList Roundrect_End = doc.SelectNodes("//Roundrect/mxCell/mxGeometry[@isEnd='1']");
                XmlNodeList Shape_End = doc.SelectNodes("//Shape/mxCell/mxGeometry[@isEnd='1']");
                XmlNodeList Image_End = doc.SelectNodes("//Image/mxCell/mxGeometry[@isEnd='1']");
                int EndCount = Rect_End.Count + Roundrect_End.Count + Shape_End.Count + Image_End.Count;
                if (EndCount == 0) return "必须设置一个结束节点";
                else if (EndCount > 1) return "只能设置一个结束节点";
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
        
        /// <summary>
        /// 得到该审批事项所在的部门和关联用户
        /// </summary>
        /// <param name="FlowNum"></param>
        /// <param name="WorkID"></param>
        /// <returns></returns>
        public static string[] GetDepartInUserAndId(int FlowNum, string WorkID)
        {
            return new string[2];
        }
        /// <summary>
        /// 通过代码控制下一步审批人 审批设置里配置code的参数为$1|$2|$3
        /// </summary>
        /// <param name="FlowID"></param>
        /// <param name="WorkID"></param>
        /// <param name="StepID"></param>
        /// <returns></returns>
        private static ArrayList CustomJudgeUsers(string FlowID, string WorkID, int StepID)
        {
            ArrayList al = new ArrayList();
            using (DB db = new DB())
            {
                db.Open();
                string sql = "select userid from tb_test where workid='" + str.D2DD(WorkID) + "'";
                using (DR dr = db.OpenRecord(sql))
                {
                    while (dr.Read())
                    {
                        if (dr.GetStringNoNULL(0) != "")
                        {
                            al.Add(dr.GetString(0));
                        }
                    }
                    return al;
                }
            }
        }
    }
}
