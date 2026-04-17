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

namespace FTFrame.WorkFlow.Core
{
    public class Const
    {
        //审批流程类型字典ID 字典类型固定，用户只能开关不能增删改类型
        public const string DicTypeCdn = "b.appname='sys' and b.setkey='workflow'";
        public enum Table
        {
            FC_WorkFlow_List,
            FC_WorkFlow_Step,
            FC_WorkFlow_User,
            FC_WorkFlow_User_Except,
            FC_WorkFlow_User_Dy,
            FC_WorkFlow_Detail
        }
        public enum FlowResultType
        {
            PASS = 1,
            LASTSTEP = 2,
            REJECT = 3,
        }

        [Serializable]
        public class WorkFlowInstance
        {
            public string FlowFid
            {
                get; set;
            }
            public int FlowNum
            {
                get; set;
            }
            public string FlowName
            {
                get;
                set;
            }
            public string FlowStartActionTip
            {
                get;
                set;
            }
            public string FlowCancelActionTip
            {
                get;
                set;
            }
            public string FlowResetActionTip
            {
                get;
                set;
            }
            public bool StrictAutoPass
            {
                get;
                set;
            }
            public string BaseTableName
            {
                get;
                set;
            }
            public string BasePrimaryColumn
            {
                get;
                set;
            }
            public string BaseKeyColumn
            {
                get;
                set;
            }
            public string BaseUserColumn
            {
                get;
                set;
            }
            public string BaseKeySql
            {
                get;
                set;
            }
            public string BaseUserSql
            {
                get;
                set;
            }
            public int CancelStat
            {
                get;
                set;
            }
            public int InitStat
            {
                get;
                set;
            }
            public int ProcessStat
            {
                get;
                set;
            }
            public int EndStat
            {
                get;
                set;
            }
            public int RejectStat
            {
                get;
                set;
            }
            public string RemindJudgeTitle
            {
                get;
                set;
            }
            public string RemindEndTitle
            {
                get;
                set;
            }
            public string RemindRejectTitle
            {
                get;
                set;
            }
            public string RemindJudgeContent
            {
                get;
                set;
            }
            public string RemindEndContent
            {
                get;
                set;
            }
            public string RemindRejectContent
            {
                get;
                set;
            }
        }
    }
}
