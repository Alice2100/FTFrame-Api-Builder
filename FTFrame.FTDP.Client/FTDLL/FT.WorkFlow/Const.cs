using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT.Com.WorkFlow
{
    public class Const
    {
        //审批流程类型字典ID 字典类型固定，用户只能开关不能增删改类型
        public const int DicTypeID = 42;
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
    }
}
