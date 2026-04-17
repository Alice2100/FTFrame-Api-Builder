using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTFrame
{
    public class Enums
    {
        public enum UserType
        {
            Admin,
            Normal
        }
        public enum FlowJudge
        {
            Pass = 0,
            RejectToFinish = 1,
            RejectToLastStep = 2,
        }
        public enum FlowResultCaption
        {
            提交审批 = 1,
            审批通过 = 2,
            退回到上一步 = 3,
            审批拒绝=4,
            取消审批=5,
        }
        public enum UserStatType
        {
            LOADING = 1,
            ONLINE = 2,
            OFFLINE = 3,
            AWAY = 4,
            BUSY = 5
        }
        public enum TaskState
        {
            HOLD = 1,
            NOTSTART = 2,
            EXECUTE = 3,
            PAUSE = 4,
            FINISHED = 5,
            FINISHED_JUDGE = 6
        }
        public enum TaskStateCN
        {
            挂起 = 1,
            未开始 = 2,
            执行中 = 3,
            暂停 = 4,
            已完成 = 5,
            待审核 = 6
        }
        public enum MessageType
        {
            NORMAL = 0,//一般性消息,FRUSERID为发送人ID，以下都为system
            TASK_REMIND = 1,//任务提醒
            TASK_ALERT = 2,//任务预警
            ALL = 10//全部
        }
        public enum RemindMessageType
        {
            PLANPASS = 1,//计划前几天
            REALPASS = 2,//前置后几天
            ENDPASS = 3,//完成前几天
            FINISH = 4//完成后
        }
        public enum FinanceApplyState
        {
            CASHED = 1,//已付款
            NOJUDGE = 2,//未审批
            JUDGEING = 3,//审批中
            WAITCASH = 4//待付款
        }
        public enum FinanceApplyStateCN
        {
            已付款 = 1,
            未审批 = 2,
            审批中 = 3,
            待付款 = 4
        }
        public enum FlowResultType
        {
            PASS = 1,//同意
            LASTSTEP = 2,//退回给上级
            REJECT = 3//退回给本人
        }
        public enum ActionType
        {

        }
        public enum CertApplyState
        {
            OUTED = 1,//已借出
            NOJUDGE = 2,//未审批
            JUDGEING = 3,//审批中
            JUDGEEND = 4//待借出
        }
        public enum CertApplyStateCN
        {
            已借出 = 1,
            未审批 = 2,
            审批中 = 3,
            待借出 = 4
        }
        public enum ForeFrameType
        {
            JQueryUI,
            LayUI
        }
        public enum ApiBusinessType
        {
            List,
            DataOp,
            DyValue
        }
        public enum KeyType
        {
            Guid,
            AutoIncrement,
            SnowId,
            Custom
        }
    }
}
