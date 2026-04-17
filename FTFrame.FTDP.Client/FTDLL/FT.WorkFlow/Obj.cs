using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.Com.WorkFlow
{
    [Serializable]
    public class Instance
    {
        public Instance()
        {
        }
        public string FlowFid
        {
            get;set;
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
