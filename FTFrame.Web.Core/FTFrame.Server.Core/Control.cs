using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FTFrame.Server.Core
{
    public class Control
    {
        private static Hashtable Paras_HT;
        public static List<string[]> Paras(string ControlName)
        {
            ControlName = ControlName.ToLower();
            if (Paras_HT == null) Paras_HT = new Hashtable();
            if (Paras_HT.ContainsKey(ControlName)) return (List<string[]>)Paras_HT[ControlName];
            string List_Para = "string CurPage, int SelectType, string LoadingImg, int RateNumType, string SelectOnClick, int NumsPerPage, string FirstPage, string PrePage, string NextPage, string LastPage, string TurnPage, string CountZero, int Cellspacing, int Cellpadding, string ColorHover, string ColorDefault, string ColorSelect, int HeadIsShow, int TurnIsShow, string OrderBy, string NewSiteID, string FormStartTime, string FormEndTime, string FormPID, string MainTable, string RowAll, string APISet, int IsAutoShow, string CusCondition, string RateEle, string CusSQL, string CusSQLHalf, string RerationTree, string BlockDataDefine, string AppendTitle, string LoadEndJS, string CustomConnection, string CustomTurnPageBottom, string CustomTurnPageTop, string CacuRowData, int IsSearchShow, int IsCusCdnShow, string SchAreaApdHtml, string AdvSearch, string AdvSearchCaption, string SearchDefine, string SearchDefineTip, string StrictSearch, int IsRefreshShow, int IsColDefineShow, string CusCdnCols, string Const1, string Const2, string Const3, string Const4, string Const5, string Const6, string Const7, string Const8, string Const9, string Const10, string Const11, string Const12, string Const13, string Const14, string Const15, string Const16, string Const17, string Const18, string Const19, string Const20, string Const21, string Const22, string Const23, string Const24, string List_OPID, string List_Code, string MemBind, string EleCondition, string RoleBindData, string RoleBindSession, string AuthRule, string FlowStat, string Del_Code, string Del_MemBind, string Del_EleCondition, string Del_RoleBindData, string Del_RoleBindSession, string Del_AuthRule, string Del_FlowStat, string Del_OPID, string Flow_Code, string Flow_MemBind, string Flow_EleCondition, string Flow_RoleBindData, string Flow_RoleBindSession, string Flow_AuthRule, string Flow_FlowStat, string Flow_OPID, string Copy_Code, string Copy_MemBind, string Copy_EleCondition, string Copy_RoleBindData, string Copy_RoleBindSession, string Copy_AuthRule, string Copy_FlowStat, string Copy_OPID, int ExportMax, int MenuIsShow, string MenuButtonSet,string ExecBefore,string ExecAfter";
            string DyValue_Para = "string Define, string FuncName, string AppendJS0, string AppendJS1, string NewSiteID, string YBAppendJS0, string YBAppendJS1, string DefaultFID, string APISet, string FidCol, int OpDefaultCol,string ExecBefore,string ExecAfter,string CustomConnection";
            string DataOp_Para = "int OPType, string Define, string NewSiteID, string JSBefore, string JSSuccess, string CodeBefore, string CodeAfter, string OPContidionSql, string OPContidionCode, string OPContidionJs, string BeforeSql, string AfterSql, string OPID, string CheckRule, string DefaultFID, int IsMultiMod, string MultiFidName, string MultiCondition, string CloseButton, string CloseButtonCap, string ButtonCap, string ButtonImg, int FlowType, string Tabletag, int FlowStat, string FlowDesign, string FlowDesignBaranch, string FlowDesignPos, string APISet, string FidCol, int OpDefaultCol,string CustomConnection";
            string DyNum_Para = "string CurPage,string Special,string Patten,string TableName,string ColName,string LockLike";
            List<string[]> list = new List<string[]>();
            string paraStr = "";
            switch (ControlName)
            {
                case "list": paraStr= List_Para;break;
                case "dyvalue": paraStr = DyValue_Para; break;
                case "dataop": paraStr = DataOp_Para; break;
                case "dynum": paraStr = DyNum_Para; break;
            }
            string[] item = paraStr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string itemstr in item)
            {
                list.Add(itemstr.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            }
            Paras_HT.Add(ControlName, list);
            return list;
        }
        public static string Name(string ControlName)
        {
            switch(ControlName.ToLower())
            {
                case "list":return "List";
                case "dyvalue": return "DyValue";
                case "dataop": return "DataOp";
                case "dynum": return "DyNum";
            }
            return ControlName;
            //return "(NoControlName)";
        }
    }
}
