using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using CoreHttp = Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;
using System.Xml;
using System.Collections;
using Microsoft.AspNetCore.Http;
using FTFrame.Dynamic.Core;
using System.Reflection;

namespace FTFrame.Project.Core
{
    /// <summary>
    /// Define @code rules and call them using @code syntax in client configuration
    /// Example:@code(str.SnowId),@code(sql.GetUser,[userId]),@code(sql.IdAge,@p1@|32)
    /// </summary>
    public partial class Code
    {
        public static object Get(string CodeDefine, Dictionary<string, object> reqDic, Dictionary<string, string> paraDic, HttpContext Context = null)
        {
            if (Context == null) Context = FTFrame.FTHttpContext.Current;
            if (!CodeDefine.StartsWith("@code(")) return CodeDefine;
            try
            {
                CodeDefine = CodeDefine.Substring(CodeDefine.IndexOf('(') + 1, CodeDefine.LastIndexOf(')') - CodeDefine.IndexOf('(') - 1);
                if (CodeDefine.IndexOf(',') < 0)
                {
                    CodeDefine += ",";
                }
                int index = CodeDefine.IndexOf(',');
                string left = CodeDefine.Substring(0, index).Trim();
                string right = CodeDefine.Substring(index + 1).Trim();
                string[] _para = right.Split('|');
                if (paraDic != null)
                {
                    for (var i = 0; i < _para.Length; i++)
                    {
                        foreach (var paraKey in paraDic.Keys)
                        {
                            _para[i] = _para[i].Replace(paraKey, paraDic[paraKey]??"");
                        }
                    }
                }
                string _tag = left.Substring(0, left.IndexOf('.'));
                string _code = left.Substring(left.IndexOf('.') + 1);
                return GetValue(_tag, _code, _para, reqDic, Context);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
        
        public static object Get(string CodeDefine, Dictionary<string, object> reqDic, HttpContext Context = null)
        {
            if (Context == null) Context = FTFrame.FTHttpContext.Current;
            if (!CodeDefine.StartsWith("@code(")) return CodeDefine;
            try
            {
                CodeDefine = CodeDefine.Substring(CodeDefine.IndexOf('(') + 1, CodeDefine.LastIndexOf(')') - CodeDefine.IndexOf('(') - 1);
                if (CodeDefine.IndexOf(',') < 0)
                {
                    CodeDefine += ",";
                }
                int index = CodeDefine.IndexOf(',');
                string left = CodeDefine.Substring(0, index).Trim();
                string right = CodeDefine.Substring(index + 1).Trim();
                string[] _para = right.Split('|');
                if (left.StartsWith("$"))
                {
                    if (SysConst.CodeGetCompile) return FTFrame.Dynamic.Core.Code.Get(left.Substring(1), _para, Context);
                    else return Reflect(left.Substring(1), _para, Context);
                }

                string _tag = left.Substring(0, left.IndexOf('.'));
                string _code = left.Substring(left.IndexOf('.') + 1);
                return GetValue(_tag, _code, _para, reqDic, Context);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
        private static object GetValue(string _tag, string _code, string[] _para, Dictionary<string, object> reqDic, HttpContext Context)
        {
            switch (_tag)
            {
                //Basic String 
                case "str": return _str(_code, _para, reqDic, Context);
                //Sql String 
                case "sql": return _sql(_code, _para, reqDic, Context);
                //Business
                case "pro": return _pro(_code, _para, reqDic, Context);
                //case "obj": return _obj(_code, _para, reqDic, Context);
                ////Flow
                //case "WF": return WorkFlow.Code._str(_code, _para, Context);
                ////Flow
                //case "WFObj": return WorkFlow.Code._obj(_code, _para, Context);
                //case "oa": return _oa(_code, _para, reqDic, Context);
            }
            return "(nocode)";
        }
        public static object GetEnum(string EnumDefine)
        {
            if (!EnumDefine.StartsWith("@enum(")) return EnumDefine;
            try
            {
                var item = EnumDefine.Split(new char[] { '(', '.', ')' });
                if (item.Length < 3) return EnumDefine;
                return Dic.Get(item[1], item[2]) ?? "";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
        private static object Reflect(string code, string[] para, HttpContext Context)
        {
            try
            {
                var codeSet = (dllname: "", codeval: "", returntype: "");
                string sql = "select * from ft_ftdp_codeset where codekey='" + code.D2() + "'";
                using (DB db = new DB(SysConst.ConnectionStr_FTDP))
                {
                    db.Open();
                    using (DR dr = db.OpenRecord(sql))
                    {
                        if (dr.Read())
                        {
                            codeSet.dllname = dr.GetString("dllname").Trim();
                            codeSet.codeval = dr.GetString("codeval").Trim();
                            codeSet.returntype = dr.GetString("returntype").Trim();
                        }
                    }
                }
                if (codeSet.dllname == "")
                {
                    log.Error("Code Not Find " + code);
                    return "CodeNotFind" + code;
                }
                string _codeval = codeSet.codeval;
                bool IsVoid = (codeSet.returntype == "Void");
                bool IsNew = codeSet.codeval.StartsWith("new ");
                if (IsNew)
                {
                    _codeval = _codeval.Substring(4);
                }
                int lastLeft = _codeval.LastIndexOf('(');
                string item0 = _codeval.Substring(0, lastLeft);
                string item1 = _codeval.Substring(lastLeft + 1);
                string[] ParasType = item1.Replace(")", "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                lastLeft = item0.LastIndexOf('.');
                string TypeName = item0.Substring(0, item0.IndexOf('(') < 0 ? lastLeft : item0.IndexOf('('));
                string FuncName = item0.Substring(lastLeft + 1);
                byte[] filedata = File.ReadAllBytes(SysConst.RootPath + "\\" + codeSet.dllname + ".dll");
                var assembly = Assembly.Load(filedata);
                Type type = assembly.GetType(TypeName);
                Type[] params_type = new Type[ParasType.Length];
                object[] params_obj = new object[ParasType.Length];
                int paraIndex = 0;
                for (int i = 0; i < ParasType.Length; i++)
                {//String,Int16,Int32,Int64,Decimal,Single,Double,Boolean,HttpContext
                    switch (ParasType[i])
                    {
                        case "String":
                            params_type[i] = typeof(string);
                            params_obj[i] = para[paraIndex++];
                            break;
                        case "Int16":
                            params_type[i] = typeof(Int16);
                            params_obj[i] = int.Parse(para[paraIndex++]);
                            break;
                        case "Int32":
                            params_type[i] = typeof(int);
                            params_obj[i] = int.Parse(para[paraIndex++]);
                            break;
                        case "Int64":
                            params_type[i] = typeof(Int64);
                            params_obj[i] = int.Parse(para[paraIndex++]);
                            break;
                        case "Decimal":
                            params_type[i] = typeof(decimal);
                            params_obj[i] = decimal.Parse(para[paraIndex++]);
                            break;
                        case "Single":
                            params_type[i] = typeof(float);
                            params_obj[i] = float.Parse(para[paraIndex++]);
                            break;
                        case "Double":
                            params_type[i] = typeof(double);
                            params_obj[i] = double.Parse(para[paraIndex++]);
                            break;
                        case "Boolean":
                            params_type[i] = typeof(bool);
                            params_obj[i] = bool.Parse(para[paraIndex++]);
                            break;
                        case "HttpContext":
                            params_type[i] = typeof(HttpContext);
                            params_obj[i] = Context;
                            break;
                    }
                }
                if (IsNew)
                {
                    object instance = assembly.CreateInstance(TypeName);
                    //var func = InstanceMethodBuilder<int, int>.CreateInstanceMethod(instance, method);
                    if (IsVoid)
                    {
                        type.GetMethod(FuncName, params_type).Invoke(instance, params_obj);
                        return null;
                    }
                    else
                    {
                        return (object)(type.GetMethod(FuncName, params_type).Invoke(instance, params_obj));
                    }
                }
                else
                {
                    if (IsVoid)
                    {
                        type.GetMethod(FuncName, params_type).Invoke(null, params_obj);
                        return null;
                    }
                    else
                    {
                        return (object)(type.GetMethod(FuncName, params_type).Invoke(null, params_obj));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
    }
    /// <summary>
    /// Just For Page Development
    /// </summary>
    public class Right
    {
        public static string ListShowUserRight()
        {
            return null;
            //return UserTool.IsLogin() ? null : "No login or login timeout";
        }
        public static bool HavePageRight(string PageUrl, HttpRequest req)
        {
            return true;
            //return UserClass.HasePageRight(PageUrl, req);
        }
        public static bool HaveOPRight(string OPID)
        {
            return true;
            //OPID = OPID.Trim();
            //if (OPID.Equals("")) return true;
            //return UserClass.HaseOPRight(OPID);
        }
        public static bool HaveRoleNameRight(string RoleName)
        {
            return true;
            //RoleName = RoleName.Trim();
            //if (RoleName.Equals("")) return true;
            //return UserClass.HaseRoleNameRight(RoleName);
        }
        public static bool PageAllFilter(HttpContext context, string path, string norighturl)
        {
            if (Right.HaveRoleNameRight("*Administrator")) return true;
            return true;
        }
        /// <summary>
        /// Just For DataGetting on Page Development
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string DyValue(HttpContext context, ArrayList Define, string SiteID)
        {
            return null;
        }
        /// <summary>
        /// Just For DataOperation on Page Development
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string DataOP(HttpContext context)
        {
            return null;
        }
    }
    public class Action
    {
        public static void ActionSave(string TableName, HttpContext Context, int ActionType, int flow)
        {
            //0 add 1 mod 2 del 3 flow
            TableName = TableName.Trim();
            if (ActionType == 0)
            {

            }
            else if (ActionType == 1)
            {

            }
            else if (ActionType == 2)
            {

            }
            else if (ActionType == 3)
            {

            }
        }
        public static void ActionSave(string TableName, HttpContext Context, int ActionType)
        {
            ActionSave(TableName, Context, ActionType, 0);
        }
    }
}
