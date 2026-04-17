using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTFrame.Tool;
using FTFrame;
using FTFrame.DBClient;
using FTFrame.Base;
using FTFrame.Project;
using System.Web;
using System.Collections;
namespace FTFrame.Interface
{
    public class Code
    {
        public static object Get(string CodeDefine, HttpContext Context)
        {
            if (!CodeDefine.StartsWith("@code(")) return CodeDefine;
            try
            {
                CodeDefine = CodeDefine.Substring(CodeDefine.IndexOf('(') + 1, CodeDefine.LastIndexOf(')') - CodeDefine.IndexOf('(') - 1);
                if (CodeDefine.IndexOf(',') < 0)
                {
                    CodeDefine += ",";
                }
                string[] item = CodeDefine.Split(',');
                string _tag = item[0].Substring(0, item[0].IndexOf('.'));
                string _code = item[0].Substring(item[0].IndexOf('.') + 1);
                string[] _para = item[1].Split('|');
                switch (_tag)
                {
                    case "com": return Base.Interface.com(_code, _para, Context);
                    case "str": return _str(_code, _para, Context);
                    case "sql": return _sql(_code, _para, Context);
                    case "pro": return _pro(_code, _para, Context);
                    case "obj": return _obj(_code, _para, Context);
                    case "html": return FTHTML._html(_code, _para, Context);
                }
                return "(nocode)";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
        private static object _obj(string code, string[] para, HttpContext Context)
        {
            return null;
        }
        private static string _str(string code, string[] para, HttpContext Context)
        {
            if (code.Equals("DateFormat"))//格式化日期
            {
                DateTime dt = DateTime.Now;
                if (DateTime.TryParse(para[0], out dt))
                {
                    return str.GetDateTimeCustom(dt, para[1]);
                }
                else return para[0];
            }
            else if (code.Equals("SetPay"))//转换金额
            {
                var result = Convert.ToDouble(para[0]);
                result = result / 100;
                return result.ToString();
            }
            else if (code.Equals("DateLeft"))//离当前还剩余几天
            {
                DateTime dt = DateTime.Now;
                if (DateTime.TryParse(para[0], out dt))
                {
                    return str.GetTimeSpanDays(dt, DateTime.Now).ToString();
                }
                else return "0";
            }
            else if (code.Equals("DateSpanDays"))//日期相差几天
            {
                DateTime dt1 = DateTime.Now;
                DateTime dt2 = DateTime.Now;
                if (DateTime.TryParse(para[0], out dt1) && DateTime.TryParse(para[1], out dt2))
                {
                    return str.GetTimeSpanDays(dt1, dt2).ToString();
                }
                else return "0";
            }
            else if (code.Equals("DatePassYears"))//年数
            {
                DateTime dt1 = DateTime.Now;
                DateTime dt2 = DateTime.Now;
                if (DateTime.TryParse(para[0], out dt1) && DateTime.TryParse(para[1], out dt2))
                {
                    decimal y = 0;
                    while (dt1.AddYears(1) <= dt2.AddDays(1))
                    {
                        y = y + 1;
                        dt1 = dt1.AddYears(1);
                    }
                    return (y + (Convert.ToDecimal(((TimeSpan)(dt2.AddDays(1) - dt1)).TotalDays) / 365)).ToString("0.00");
                }
                else return "0";
            }
            else if (code.Equals("Rate"))//百分比增长率
            {
                decimal d1 = 0;
                decimal d2 = 0;
                if (decimal.TryParse(para[0], out d1) && decimal.TryParse(para[1], out d2))
                {
                    return d2 == 0 ? "0" : ((100 * (d1 - d2) / d2).ToString("0.00") + "%");
                }
                else return "";
            }
            else if (code.Equals("RateDiv"))//百分比率
            {
                decimal d1 = 0;
                decimal d2 = 0;
                if (decimal.TryParse(para[0], out d1) && decimal.TryParse(para[1], out d2))
                {
                    return d2 == 0 ? "0" : ((100 * d1 / d2).ToString("0.00") + "%");
                }
                else return "";
            }
            else if (code.Equals("Minus"))//相减
            {
                decimal d1 = 0;
                decimal d2 = 0;
                if (decimal.TryParse(para[0], out d1) && decimal.TryParse(para[1], out d2))
                {
                    return d2 == 0 ? "0" : ((d1 - d2).ToString("0.00"));
                }
                else return "";
            }
            else if (code.Equals("Now"))//当前时间
            {
                return str.GetDateTime();
            }
            else if (code.Equals("Text2Html"))//文本html转换
            {
                return str.GetSafeCode(para[0]);
            }
            else if (code.Equals("DecimalFormat"))//字符串格式化
            {
                return decimal.Parse(para[0]).ToString("0.00");
            }
            else if (code.Equals("DateConcatColor"))//日期比较返回不同颜色
            {
                //date1,date2,color1,color2,color3,format
                DateTime dt1 = DateTime.Now;
                DateTime dt2 = DateTime.Now;
                if (DateTime.TryParse(para[0], out dt1) && DateTime.TryParse(para[1], out dt2))
                {
                    if (dt1 < dt2) return "<span style='color:" + para[2].Replace("@", "#") + "'>" + str.GetDateTimeCustom(dt1, para[5]) + "</span>";
                    else if (dt1 == dt2) return "<span style='color:" + para[3].Replace("@", "#") + "'>" + str.GetDateTimeCustom(dt1, para[5]) + "</span>";
                    else return "<span style='color:" + para[4].Replace("@", "#") + "'>" + str.GetDateTimeCustom(dt1, para[5]) + "</span>";
                }
                else return "NotDate";
            }
            else if (code.Equals("DateOutColor"))//超过月份指定颜色，并显示文字
            {
                //curdate,mindate,maxdate,color,format,text
                DateTime curdate = DateTime.Parse(para[0]);
                DateTime mindate = DateTime.Now;
                DateTime maxdate = DateTime.Now;
                bool NotIn = true;
                if (DateTime.TryParse(para[1], out mindate) && DateTime.TryParse(para[2], out maxdate))
                {
                    if (DateTime.Parse(str.GetYearMonth(curdate) + "-1") >= DateTime.Parse(str.GetYearMonth(mindate) + "-1") && DateTime.Parse(str.GetYearMonth(curdate) + "-1") <= DateTime.Parse(str.GetYearMonth(maxdate) + "-1"))
                    {
                        NotIn = false;
                    }
                }
                if (NotIn)
                {
                    return "<span style='color:" + para[3].Replace("@", "#") + "'>" + str.GetDateTimeCustom(curdate, para[4]) + para[5] + "</span>";
                }
                else return str.GetDateTimeCustom(curdate, para[4]);
            }
            else if (code.Equals("DecimalConcatColor"))//数字比较返回不同颜色
            {
                //d1,d2,color1,color2,color3
                decimal d1 = 0;
                decimal d2 = 0;
                if (decimal.TryParse(para[0], out d1) && decimal.TryParse(para[1], out d2))
                {
                    if (d1 < d2) return "<span style='color:" + para[2].Replace("@", "#") + "'>" + d1 + "</span>";
                    else if (d1 == d2) return "<span style='color:" + para[3].Replace("@", "#") + "'>" + d1 + "</span>";
                    else return "<span style='color:" + para[4].Replace("@", "#") + "'>" + d1 + "</span>";
                }
                else return "";
            }
            else if (code.Equals("DateMonthOptions"))//日期月份选择下拉框选项
            {
                //compare,pass1,pass2,curmonthcolor
                DateTime dtnow = DateTime.Parse(str.GetYearMonth() + "-1");
                string options = "";
                int curpass = int.Parse(para[2]);
                if (int.Parse(para[1]) >= int.Parse(para[2]))
                {
                    while (curpass >= int.Parse(para[2]) && curpass <= int.Parse(para[1]))
                    {
                        if (para[0].Equals("<"))
                        {
                            options += "<option value='" + para[0] + str.GetYearMonth(dtnow.AddMonths(curpass + 1)) + "-1'" + (curpass == 0 ? (" style='background:" + para[3] + "'") : "") + ">" + str.GetDateTimeCustom(dtnow.AddMonths(curpass), "yyyy年MM月") + "</option>";
                        }
                        else
                        {
                            options += "<option value='" + para[0] + str.GetYearMonth(dtnow.AddMonths(curpass)) + "-1'" + (curpass == 0 ? (" style='background:" + para[3] + "'") : "") + ">" + str.GetDateTimeCustom(dtnow.AddMonths(curpass), "yyyy年MM月") + "</option>";
                        }
                        curpass++;
                    }
                }
                else
                {
                    while (curpass <= int.Parse(para[2]) && curpass >= int.Parse(para[1]))
                    {
                        if (para[0].Equals("<"))
                        {
                            options += "<option value='" + para[0] + str.GetYearMonth(dtnow.AddMonths(curpass + 1)) + "-1'" + (curpass == 0 ? (" style='background:" + para[3] + "'") : "") + ">" + str.GetDateTimeCustom(dtnow.AddMonths(curpass), "yyyy年MM月") + "</option>";
                        }
                        else
                        {
                            options += "<option value='" + para[0] + str.GetYearMonth(dtnow.AddMonths(curpass)) + "-1'" + (curpass == 0 ? (" style='background:" + para[3] + "'") : "") + ">" + str.GetDateTimeCustom(dtnow.AddMonths(curpass), "yyyy年MM月") + "</option>";
                        }
                        curpass--;
                    }
                }
                return options;
            }
            else if (code.Equals("UserCap"))//用户ID得到用户姓名
            {
                string UserID = para[0].Trim();
                if (UserID.Equals("")) return "";
                DB db = new DB();
                db.Open();
                try
                {
                    string UserCap = "";
                    string sql = "select REALNAME,USERNAME from TB_USERINFO where USERID='" + str.D2DD(UserID) + "'";
                    DR dr = db.OpenRecord(sql);
                    if (dr.Read())
                    {
                        UserCap = dr.GetStringNoNULL(0).Equals("") ? dr.GetStringNoNULL(1) : dr.GetStringNoNULL(0);
                    }
                    dr.Close();
                    return UserCap;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    db.Close();
                }
            }
            else if (code.Equals("Progress"))//进度
            {
                int prog = int.Parse(para[0]);
                if (prog < 100)
                {
                    return "<div style='height:18px;padding:2px 0px 0px 0px;margin:0px;width:100px;background:url(/_pro/res/images/bluebar.jpg) no-repeat " + (prog - 800) + "px 0px;text-align:center;VERTICAL-ALIGN: middle'>" + prog + "&nbsp;%</div>";
                }
                else
                {
                    return "<div style='height:18px;padding:2px 0px 0px 0px;margin:0px;width:100px;background:url(/_pro/res/images/greenbar.jpg) no-repeat 0px 0px;text-align:center;VERTICAL-ALIGN: middle'>" + prog + "&nbsp;%</div>";
                }
            }
            else if (code.Equals("ProgressDiv"))//进度
            {
                int prog1 = int.Parse(para[0]);
                int prog2 = int.Parse(para[1]);
                int prog = 0;
                if (prog2 == 0)
                {
                    prog = 100;
                    if (prog1 == 0)
                    {
                        return "<div style='height:18px;padding:2px 0px 0px 0px;margin:0px;width:100px;background:url(/_pro/res/images/yellowbar.jpg) no-repeat " + (prog - 800) + "px 0px;text-align:center;VERTICAL-ALIGN: middle'>" + prog1 + "&nbsp;/&nbsp;" + prog2 + "</div>";
                    }
                }
                else prog = 100 * prog1 / prog2;
                if (prog > 100) prog = 100;
                if (prog < 100)
                {
                    return "<div style='height:18px;padding:2px 0px 0px 0px;margin:0px;width:100px;background:url(/_pro/res/images/bluebar.jpg) no-repeat " + (prog - 800) + "px 0px;text-align:center;VERTICAL-ALIGN: middle'>" + prog1 + "&nbsp;/&nbsp;" + prog2 + "</div>";
                }
                else
                {
                    return "<div style='height:18px;padding:2px 0px 0px 0px;margin:0px;width:100px;background:url(/_pro/res/images/greenbar.jpg) no-repeat 0px 0px;text-align:center;VERTICAL-ALIGN: middle'>" + prog1 + "&nbsp;/&nbsp;" + prog2 + "</div>";
                }
            }
            else if (code.Equals("ProgressClick"))//进度
            {
                int prog = int.Parse(para[0]);
                if (prog < 100)
                {
                    return "<div onclick=\"getview_task('" + para[1] + "');O('tasksubdetaila').click()\" style='cursor:pointer;height:18px;padding:2px 0px 0px 0px;margin:0px;width:100px;background:url(/_pro/res/images/bluebar.jpg) no-repeat " + (prog - 800) + "px 0px;text-align:center;VERTICAL-ALIGN: middle'>" + prog + "&nbsp;%</div>";
                }
                else
                {
                    return "<div  onclick=\"getview_task('" + para[1] + "');O('tasksubdetaila').click()\" style='cursor:pointer;height:18px;padding:2px 0px 0px 0px;margin:0px;width:100px;background:url(/_pro/res/images/greenbar.jpg) no-repeat 0px 0px;text-align:center;VERTICAL-ALIGN: middle'>" + prog + "&nbsp;%</div>";
                }
            }
            return "(nocode)";
        }
        private static string _sql(string code, string[] para, HttpContext Context)
        {
            if (code.Equals("SumDecimalAll"))//获值：获得包含动态新增行在内的decimal类型和
            {
                string tablename = para[0];
                string colname = para[1];
                string fid = para[2];
                string cdn1 = para[3];
                string cdn2 = para[4];
                string sql = "select isnull(SUM(" + colname + "),0) from (";
                sql += "select " + colname + " from " + tablename + " where fid='" + str.D2DD(fid) + "' " + cdn1;
                sql += " union all ";
                sql += "select CAST(cast(evalue as varchar(18)) as decimal(18,2)) as " + colname + " from " + tablename + "_dy where eid='" + colname + "' and fid='" + str.D2DD(fid) + "' " + cdn2;
                sql += ") as t1";
                DB db = new DB();
                db.Open();
                try
                {
                    return db.GetDecimal(sql).ToString("0.00");
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return "0";
                }
                finally { db.Close(); }
            }
            else if (code.Equals("DyDataAdd"))//插入一条动态新增行数据
            {
                string tablename = para[0];
                string colnames = para[1];
                string fid = para[2];
                string colvalues = para[3];
                string firstcdn = para[4];
                string sql = null;
                DB db = new DB();
                db.Open();
                try
                {
                    string[] colnames_ = colnames.Split(new char[] { '#' }, StringSplitOptions.None);
                    string[] colvalues_ = colvalues.Split(new char[] { '#' }, StringSplitOptions.None);
                    sql = "select count(*) as ca from " + tablename + " where fid='" + str.D2DD(fid) + "' and " + firstcdn;

                    if (db.GetInt(sql) > 0)
                    {
                        sql = "update " + tablename + " set ";
                        for (int i = 0; i < colnames_.Length; i++)
                        {
                            sql += " " + colnames_[i] + "='" + str.D2DD(colvalues_[i]) + "' ";
                            if (i < colnames_.Length - 1) sql += ",";
                        }
                        sql += " where fid='" + str.D2DD(fid) + "'";
                        db.execSql(sql);
                    }
                    else
                    {
                        sql = "select max(erate) from (select 0 as erate union select erate from " + tablename + "_dy where fid='" + str.D2DD(fid) + "') as t1";

                        int newerate = db.GetInt(sql) + 1;
                        for (int i = 0; i < colnames_.Length; i++)
                        {
                            sql = "insert into " + tablename + "_dy(fid,eid,evalue,erate)";
                            sql += "values('" + str.D2DD(fid) + "','" + str.D2DD(colnames_[i]) + "','" + str.D2DD(colvalues_[i]) + "'," + newerate + ")";
                            db.execSql(sql);
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return ex.Message;
                }
                finally { db.Close(); }
            }
            return "(nocode)";
        }
        private static string _pro(string code, string[] para, HttpContext Context)
        {
            return "(nocode)";
        }
        private static string _oa(string code, string[] para, HttpContext Context)
        {
            switch (code)
            {
                #region 通知公告
                case "Board_Update": return Base.Board.UpdateAfter(para[0], para[1], Context);
                case "Board_GetDepartsId": return Base.Board.GetDeparts(para[0])[0];
                case "Board_GetDepartsName": return Base.Board.GetDeparts(para[0])[1];
                #endregion
                case "messageadd": return Base.Message.Add(Context);
                case "MessageTitleShow": return Base.Message.TitleShow(para[0], int.Parse(para[1]), int.Parse(para[2]));
                case "MessageContentShow": return Base.Message.ContentShow(para[0], int.Parse(para[1]));
            }
            return null;
        }
    }
    public class Info
    {
        public static string Get(string InfoDefine, HttpContext context)
        {
            if (InfoDefine.StartsWith("@code(")) return Code.Get(InfoDefine, context).ToString();
            switch (InfoDefine)
            {
                case "UserTree": return UserClass.CreateZTree();
                case "UserTreeCheck": return UserClass.CreateUserTreeCheck(context);
                case "DepartTree": return UserClass.CreateZTree_Depart();
                case "DepartTreeCheck": return UserClass.CreateDepartTreeCheck(context);
                case "ListExportSave": return Control.List_ExportSaveFiles(context);
                case "List_ExportSaveFiles_Del": return Control.List_ExportSaveFiles_Del(context);
                case "List_ExportSaveFiles_Bind": return Control.List_ExportSaveFiles_Bind(context);
                case "List_ExportSaveFiles_Tree": return Control.List_ExportSaveFiles_Tree(context);
                case "HaveNewMessage": return Message.HaveNewMessage();
                case "ComSetSave": return Base.View.Com.SetSave(context.Request);
                case "ComDicSave": return Base.View.Com.DicSave(context.Request);
            }
            return "Undefined Info";
        }
        public static string LogOther()
        {
            return "";
        }
    }
    public class Right
    {
        public static string ListShowUserRight()
        {
            return UserTool.IsLogin() ? null : "没有登录或登录已超时";
        }
        public static bool HavePageRight(string PageUrl, HttpRequest req)
        {
            return UserClass.HasePageRight(PageUrl, req);
        }
        public static bool HaveOPRight(string OPID)
        {
            OPID = OPID.Trim();
            if (OPID.Equals("")) return true;
            return UserClass.HaseOPRight(OPID);
        }
        public static bool HaveRoleNameRight(string RoleName)
        {
            RoleName = RoleName.Trim();
            if (RoleName.Equals("")) return true;
            return UserClass.HaseRoleNameRight(RoleName);
        }
        public static bool PageAllFilter(HttpContext context, string path, string norighturl)
        {
            if (Right.HaveRoleNameRight("*系统管理员")) return true;
            return true;
        }
        /// <summary>
        /// 后台验证取值权限，返回null为被授权，其他则为错误信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string DyValue(HttpContext context, ArrayList Define, string SiteID)
        {
            //Define结构：     string[]{
            //        id,tabletag,fid,isdy.ToString(),isdim.ToString(),sql
            //    }
            return null;
        }
        /// <summary>
        /// 后台验证数据操作权限，返回null为被授权，其他则为错误信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string DataOP(HttpContext context)
        {
            //string memid = User.UserID();
            //string PartID = Context.Request["partid"];
            //string opid = str.GetDecode(Context.Request.Form["ftdataop_" + PartID + "_opid"]);
            //if (!Interface.Right.HaveOPRight(opid))
            //{
            //    output.Write(str.JavascriptLabel("parent._loading2fai('You have no right!');"));
            //    return;
            //}
            //int datatype = int.Parse(str.GetDecode(Context.Request.Form["ftdataop_" + PartID + "_datatype"]));
            //string siteid = Context.Request.Form["ftdataop_" + PartID + "_siteid"].Replace("'", "''");
            //string jssuc = Context.Request.Form["ftdataop_" + PartID + "_jssuc"];
            //string define = Context.Request.Form["ftdataop_" + PartID + "_define"];
            //string tabletag = str.GetDecode(Context.Request.Form["ftdataop_" + PartID + "_tabletag"]).Replace("'", "''");
            //string defaultfid = Context.Request.Form["ftdataop_" + PartID + "_defaultfid"];
            //int flowtype = int.Parse(str.GetDecode(Context.Request.Form["ftdataop_" + PartID + "_flowtype"]));
            //int flowstat = int.Parse(str.GetDecode(Context.Request.Form["ftdataop_" + PartID + "_flowstat"]));
            //string flowdesign = str.GetDecode(Context.Request.Form["ftdataop_" + PartID + "_flowdesign"]);
            //string flowdesignbaranch = str.GetDecode(Context.Request.Form["ftdataop_" + PartID + "_flowdesignbaranch"]);
            //string flowdesignpos = str.GetDecode(Context.Request.Form["ftdataop_" + PartID + "_flowdesignpos"]);
            //string codebefore = Context.Request.Form["ftdataop_" + PartID + "_codebefore"];
            //string codeafter = Context.Request.Form["ftdataop_" + PartID + "_codeafter"];
            //string cdnsql = str.GetDecode(Context.Request.Form["ftdataop_" + PartID + "_cdnsql"]);
            //string cdnsqlevals = Context.Request.Form["ftdataop_" + PartID + "_cdnsqlevals"];
            //string cdncode = Context.Request.Form["ftdataop_" + PartID + "_cdncode"];
            //string cdnjs = Context.Request.Form["ftdataop_" + PartID + "_cdnjs"];
            //bool IsMultiMod = int.Parse(Context.Request.Form["ftdataop_" + PartID + "_ismultimod"]) == 1;
            //string MultiFidName = Context.Request.Form["ftdataop_" + PartID + "_multifidname"];
            //string MultiCondition = str.GetDecode(Context.Request.Form["ftdataop_" + PartID + "_multicdn"]);
            //string MultiCdnEvals = Context.Request.Form["ftdataop_" + PartID + "_multicdnevals"];
            //string execsqlbefore = str.GetDecode(Context.Request.Form["ftdataop_" + PartID + "_execsqlbefore"]);
            //string execsqlbeforeevals = Context.Request.Form["ftdataop_" + PartID + "_execsqlbeforeevals"];
            //string execsqlafter = str.GetDecode(Context.Request.Form["ftdataop_" + PartID + "_execsqlafter"]);
            //string execsqlafterevals = Context.Request.Form["ftdataop_" + PartID + "_execsqlafterevals"];
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
