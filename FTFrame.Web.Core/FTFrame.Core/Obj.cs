using System;
using System.Collections.Generic;
using System.Text;
using FTFrame;
namespace FTFrame.Obj
{
    [Serializable]
    public class Company
    {
        private string _ShortName;
        private string _FullName;
        private string _EnglishName;
        private string _Address;
        private string _Phone;
        private string _Fax;
        private DateTime _TargetStart;
        private DateTime _TargetEnd;
        public Company()
        {
        }
        public string SystemName
        {
            get
            {
                return SysConst.SystemName == null ? "FTDP Site" : SysConst.SystemName;
            }
            set { }
        }
        public string SystemTitle
        {
            get { return SysConst.SystemTitle == null ? "FTDP System" : SysConst.SystemTitle; }
            set { }
        }
        public string ShortName
        {
            get { return _ShortName; }
            set { _ShortName = value; }
        }
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }
        public string EnglishName
        {
            get { return _EnglishName; }
            set { _EnglishName = value; }
        }
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }
        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }
        public DateTime TargetStart
        {
            get { return _TargetStart; }
            set { _TargetStart = value; }
        }
        public DateTime TargetEnd
        {
            get { return _TargetEnd; }
            set { _TargetEnd = value; }
        }
    }
    [Serializable]
    public class User
    {
        private string _UserID;
        private string _UserName;
        private string _RealName;
        private Enums.UserType _UserType;
        private DateTime _LoginTime;
        private int _LoginCount;
        private int _FCount;
        private decimal _Progress;
        private string _Icon;
        public User()
        {
        }
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        public string RealName
        {
            get { return _RealName; }
            set { _RealName = value; }
        }
        public Enums.UserType UserType
        {
            get { return _UserType; }
            set { _UserType = value; }
        }
        public DateTime LoginTime
        {
            get { return _LoginTime; }
            set { _LoginTime = value; }
        }
        public int LoginCount
        {
            get { return _LoginCount; }
            set { _LoginCount = value; }
        }
        public int FCount
        {
            get { return _FCount; }
            set { _FCount = value; }
        }
        public decimal Progress
        {
            get { return _Progress; }
            set { _Progress = value; }
        }
        public string Icon
        {
            get { return _Icon; }
            set { _Icon = value; }
        }
    }
    [Serializable]
    public class Right_Page
    {
        private string _ResID;
        private string _ResName;
        private string _URL;
        public Right_Page(string resid, string resname, string url)
        {
            _ResID = resid;
            _ResName = resname;
            _URL = url;
        }
        public string ResID
        {
            get { return _ResID; }
            set { _ResID = value; }
        }
        public string ResName
        {
            get { return _ResName; }
            set { _ResName = value; }
        }
        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }
    }
    [Serializable]
    public class Right_OP
    {
        private string _ResID;
        private string _ResName;
        private string _OPID;
        public Right_OP(string resid, string resname, string opid)
        {
            _ResID = resid;
            _ResName = resname;
            _OPID = opid;
        }
        public string ResID
        {
            get { return _ResID; }
            set { _ResID = value; }
        }
        public string ResName
        {
            get { return _ResName; }
            set { _ResName = value; }
        }
        public string OPID
        {
            get { return _OPID; }
            set { _OPID = value; }
        }
    }
    [Serializable]
    public class BindingCorp
    {
        private string _Path;
        private string _ShortName;
        private string _FullName;
        private string _EnglishName;
        private string _Address;
        private string _Phone;
        private string _Fax;
        private string _DBUserID;
        private string _DBPassWord;
        private string _ConnStr;
        private string _Token;
        public BindingCorp()
        {
        }
        public string ID
        {
            get;
            set;
        }
        public int MaxUsers { set; get; }
        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }
        public string ShortName
        {
            get { return _ShortName; }
            set { _ShortName = value; }
        }
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }
        public string EnglishName
        {
            get { return _EnglishName; }
            set { _EnglishName = value; }
        }
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }
        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }
        public string DBServer
        { get; set; }
        public string DBBase
        { get; set; }
        public string DBPort
        { get; set; }
        public string DBUserID
        {
            get { return _DBUserID; }
            set { _DBUserID = value; }
        }
        public string DBPassWord
        {
            get { return _DBPassWord; }
            set { _DBPassWord = value; }
        }
        public string ConnStr
        {
            get { return _ConnStr; }
            set { _ConnStr = value; }
        }
        public string Token
        {
            get { return _Token; }
            set { _Token = value; }
        }
        public string Icon
        {
            get;
            set;
        }
        public int Stat
        {
            get;
            set;
        }
    }
    [Serializable]
    public class WorkFlowInstance
    {
        private string _FlowStartActionTip;
        private string _FlowResetActionTip;
        private bool _StrictAutoPass;
        private string _BaseTableName;
        private string _BasePrimaryColumn;
        private string _BaseKeyColumn;
        private string _BaseUserColumn;
        private int _InitStat;
        private int _ProcessStat;
        private int _EndStat;
        private string _RemindJudgeTitle;
        private string _RemindEndTitle;
        private string _RemindInitTitle;
        private string _RemindJudgeContent;
        private string _RemindEndContent;
        private string _RemindInitContent;
        public WorkFlowInstance()
        {
        }
        public string FlowStartActionTip
        {
            get { return _FlowStartActionTip; }
            set { _FlowStartActionTip = value; }
        }
        public string FlowResetActionTip
        {
            get { return _FlowResetActionTip; }
            set { _FlowResetActionTip = value; }
        }
        public bool StrictAutoPass
        {
            get { return _StrictAutoPass; }
            set { _StrictAutoPass = value; }
        }
        public string BaseTableName
        {
            get { return _BaseTableName; }
            set { _BaseTableName = value; }
        }
        public string BasePrimaryColumn
        {
            get { return _BasePrimaryColumn; }
            set { _BasePrimaryColumn = value; }
        }
        public string BaseKeyColumn
        {
            get { return _BaseKeyColumn; }
            set { _BaseKeyColumn = value; }
        }
        public string BaseUserColumn
        {
            get { return _BaseUserColumn; }
            set { _BaseUserColumn = value; }
        }
        public int InitStat
        {
            get { return _InitStat; }
            set { _InitStat = value; }
        }
        public int ProcessStat
        {
            get { return _ProcessStat; }
            set { _ProcessStat = value; }
        }
        public int EndStat
        {
            get { return _EndStat; }
            set { _EndStat = value; }
        }
        public string RemindJudgeTitle
        {
            get { return _RemindJudgeTitle; }
            set { _RemindJudgeTitle = value; }
        }
        public string RemindEndTitle
        {
            get { return _RemindEndTitle; }
            set { _RemindEndTitle = value; }
        }
        public string RemindInitTitle
        {
            get { return _RemindInitTitle; }
            set { _RemindInitTitle = value; }
        }
        public string RemindJudgeContent
        {
            get { return _RemindJudgeContent; }
            set { _RemindJudgeContent = value; }
        }
        public string RemindEndContent
        {
            get { return _RemindEndContent; }
            set { _RemindEndContent = value; }
        }
        public string RemindInitContent
        {
            get { return _RemindInitContent; }
            set { _RemindInitContent = value; }
        }
    }
}
