using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;
using FTFrame.Server.Core.Tool;
using System.Xml;
using System.Collections;
using FTFrame.Project.Core;
using CoreHttp = Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using FTFrame.Project.Core.Utils;

namespace FTFrame.Server.Core.Ajax
{
    public class Init
    {
        public static void Start(HttpContext context)
        {
            if (!Interface.Auth.HostReferOK(context)) return;
            string PageActionType = context.Request.Query["pageaction"];
            if (PageActionType != null)
            {
                if (PageActionType.Equals("download"))
                {
                    string filename = context.Request.Query["filename"];
                    string filecap = filename.Substring(filename.LastIndexOf("/") + 1);
                    filecap = filecap.Substring(filecap.IndexOf("_") + 1);
                    filename = SysConst.RootPath +"\\wwwroot\\"+ filename.Replace("/", "\\");
                    if (System.IO.File.Exists(filename))
                    {
                        using (FileStream fs = new FileStream(filename, FileMode.Open))
                        {
                            byte[] bytes = new byte[(int)fs.Length];
                            fs.Read(bytes, 0, bytes.Length);
                            fs.Close();
                            context.Response.ContentType = "application/octet-stream;charset=utf-8";
                            context.Response.Headers.Add("Content-Disposition", "attachment;filename=" + func.escape(filecap));
                            context.Response.Body.WriteAsync(bytes);
                            context.Response.Body.FlushAsync();
                        }
                    }
                    else
                    {
                        context.Response.WriteAsync(str.JavascriptLabel("alert('该文件已不存在！');history.go(-1);"));
                    }
                    return;
                }
            }
            switch (context.Request.Form["ajaxtype"].FirstOrDefault<string>())
            {
                case "List":
                    Control.List(context);
                    break;
                case "Dyvalue":
                    Control.Dyvalue(context);
                    break;
                case "Chart":
                    Control.Chart.Json(context);
                    break;
                case "Info":
                    context.Response.WriteAsync(Interface.Info.Get(context.Request.Form["infodefine"], context));
                    break;
                case "ListExportSave":
                    context.Response.WriteAsync(Interface.Info.Get("ListExportSave", context));
                    break;
                case "List_ExportSaveFiles_Del":
                    context.Response.WriteAsync(Interface.Info.Get("List_ExportSaveFiles_Del", context));
                    break;
                case "List_ExportSaveFiles_Bind":
                    context.Response.WriteAsync(Interface.Info.Get("List_ExportSaveFiles_Bind", context));
                    break;
                case "List_ExportSaveFiles_Tree":
                    context.Response.WriteAsync(Interface.Info.Get("List_ExportSaveFiles_Tree", context));
                    break;
            }
        }
    }
    public class Control
    {
        public static void List(HttpContext context)
        {
            HttpRequest req = context.Request;
            HttpResponse res = context.Response;
            if (!req.HasFormContentType) return;
            bool PageExport = (req.Form["optype"].FirstOrDefault<string>() != null && req.Form["optype"].FirstOrDefault<string>().Equals("11"));

            string ListShowUserRight = FTFrame.Project.Core.Right.ListShowUserRight();
            if (ListShowUserRight != null)
            {
                res.Clear();
                res.ContentType = "text/html;charset=utf-8";
                res.WriteAsync("#fail:" + func.escape(ListShowUserRight));
                return;
            }
            string List_OPID = Str.Decode(req.Form["List_OPID"].FirstOrDefault<string>().Trim());
            if (!Interface.Right.HaveOPRight(List_OPID))
            {
                res.Clear();
                res.ContentType = "text/html;charset=utf-8";
                res.WriteAsync("#fail:" + func.escape("没有权限查看该列表数据"));
                return;
            }
            string List_Code = Str.Decode(req.Form["List_Code"].FirstOrDefault<string>().Trim());
            if (List_Code.StartsWith("@code("))
            {
                string CodeResult = Interface.Code.Get(List_Code, context);
                if (CodeResult != null)
                {
                    res.Clear();
                    res.ContentType = "text/html;charset=utf-8";
                    res.WriteAsync("#fail:" +func.escape(CodeResult));
                    return;
                }
            }

            string s = null;
            DataTable ExportDT = null;
            string DefaultOrder = Str.Decode(req.Form["Order"].FirstOrDefault<string>());
            string partid = req.Form["partid"].FirstOrDefault<string>().Trim();
            if (partid.Length > 36) partid = partid.Substring(0, 36);
            string orderby = req.Form["orderby"].FirstOrDefault<string>().Trim();
            string ordertype = req.Form["ordertype"].FirstOrDefault<string>().Trim();
            string schdefine = req.Form["schdefine"].FirstOrDefault<string>().Trim();
            string schtext = req.Form["schtext"].FirstOrDefault<string>().Trim();
            string schstrict = req.Form["schstrict"].FirstOrDefault<string>().Trim();
            string schadv = req.Form["schadv"].FirstOrDefault<string>().Trim();
            int cuspagesize = int.Parse(req.Form["cuspagesize"].FirstOrDefault<string>().Trim());
            string RerationTreeEvals = req.Form["RerationTreeEvals"];
            string SqlEvals = req.Form["SqlEvals"];
            bool IsTree = bool.Parse(req.Form["IsTree"]);
            string RerationTree = Str.Decode(req.Form["RerationTree"]);
            string SiteID = req.Form["SiteID"];
            string sql = Str.Decode(req.Form["sql"]);
            string SqlCount = Str.Decode(req.Form["SqlCount"]);
            string SqlCountEvals = req.Form["SqlCountEvals"];
            string CusCondition = Str.Decode(req.Form["CusCondition"]);
            string CusConditionEvals = req.Form["CusConditionEvals"];
            string RowAll = req.Form["RowAll"].FirstOrDefault<string>().Replace("@ftquoat@", "\"");
            string[] Consts = req.Form["Consts"].FirstOrDefault<string>().Replace("@ftquoat@", "\"").Split(new string[] { "##" }, StringSplitOptions.None);
            string BlockDataDefine = req.Form["BlockDataDefine"].FirstOrDefault<string>().Replace("@ftquoat@", "\"");
            //BlockDataDefine = Interface.Code.Get(BlockDataDefine,context);
            bool IsBlockData = !BlockDataDefine.Trim().Equals("");
            string UserCusCdn = req.Form["UserCusCdn"].FirstOrDefault<string>().Replace("@ftquoat@", "\"");
            //string UserCusSql = req.Form["UserCusSql"].Replace("@ftquoat@", "\"");
            string CacuRowData = req.Form["CacuRowData"].FirstOrDefault<string>().Replace("@ftquoat@", "\"");
            bool IsCacuRow = !CacuRowData.Trim().Equals("");
            string CustomConnection = req.Form["CustomConnection"];
            if (CustomConnection.Trim().ToLower() == "ftdp") CustomConnection = SysConst.ConnectionStr_FTDP;
            string MainTable = Str.Decode(req.Form["MainTable"]).Replace("'", "''");
            bool NeedExport = bool.Parse(req.Form["NeedExport"]);
            int ExportMax = int.Parse(req.Form["ExportMax"]);
            int RateNumType = int.Parse(req.Form["RateNumType"]);
            int CurPageNum = int.Parse(req.Form["CurPageNum"]);
            int NumsPerPage = int.Parse(req.Form["NumsPerPage"]);
            string ColDefine_Cur = req.Form["ColDefine_Cur"];
            string CusTurnBtm = req.Form["CusTurnBtm"];
            if (string.IsNullOrWhiteSpace(CusTurnBtm)) CusTurnBtm = "@code(str.TurnPage,$1|$2|$3|$4|$5)";
            string CusTurnTop = req.Form["CusTurnTop"];
            bool ExportSave = false;
            if (req.Form["ExportSave"].FirstOrDefault<string>() != null && req.Form["ExportSave"].Equals("1"))
            {
                ExportSave = true;
            }
            string ExportSaveFileCap = req.Form["ExportSaveFileCap"];

            if (cuspagesize >= 0) NumsPerPage = cuspagesize;

            if (ExportSave) { ExportMax = 0; NumsPerPage = 0; }//生成下载不限制条数

            string[] RerationTreeEvalsitem = RerationTreeEvals.Split(new string[] { "##" }, StringSplitOptions.None);
            int IndexI = 0;
            Regex r = new Regex(@"{[^}]*}");
            MatchCollection mc = r.Matches(RerationTree);
            foreach (Match m in mc)
            {
                if (RerationTreeEvalsitem.Length > IndexI)
                {
                    RerationTree = RerationTree.Replace(m.Value, str.D2DD(RerationTreeEvalsitem[IndexI]));
                }
                IndexI++;
            }

            string[] SqlEvalsitem = SqlEvals.Split(new string[] { "##" }, StringSplitOptions.None);
            IndexI = 0;
            r = new Regex(@"{[^}]*}");
            mc = r.Matches(sql);
            //if (DateTime.Now > DateTime.Parse(str.GetDecode("zPZdq" + "OwkbE+PGvM" + "PeyUOwA==")) && new Random().Next(100)>85) return;
            foreach (Match m in mc)
            {
                if (SqlEvalsitem.Length > IndexI)
                {
                    sql = sql.Replace(m.Value, str.D2DD(SqlEvalsitem[IndexI]));
                }
                IndexI++;
            }
            sql = Interface.Code.Get(sql, context);


            string[] CusConditionEvalsItem = CusConditionEvals.Split(new string[] { "##" }, StringSplitOptions.None);
            IndexI = 0;
            r = new Regex(@"{[^}]*}");
            mc = r.Matches(CusCondition);
            foreach (Match m in mc)
            {
                if (CusConditionEvalsItem.Length > IndexI)
                {
                    CusCondition = CusCondition.Replace(m.Value, str.D2DD(CusConditionEvalsItem[IndexI]));
                }
                IndexI++;
            }

            CusCondition = Interface.Code.Get(CusCondition, context);


            int? CodeSqlCount = null;
            string[] SqlCountEvalsitem = SqlCountEvals.Split(new string[] { "##" }, StringSplitOptions.None);
            IndexI = 0;
            r = new Regex(@"{[^}]*}");
            mc = r.Matches(SqlCount);
            foreach (Match m in mc)
            {
                if (SqlCountEvalsitem.Length > IndexI)
                {
                    SqlCount = SqlCount.Replace(m.Value, str.D2DD(SqlCountEvalsitem[IndexI]));
                }
                IndexI++;
            }

            
            sql = adv.CodePattern(context,sql);
            SqlCount = adv.CodePattern(context, SqlCount);
            UserCusCdn = adv.CodePattern(context, UserCusCdn);
            sql = adv.ParaPattern(context, sql);
            SqlCount = adv.ParaPattern(context, SqlCount);
            UserCusCdn = adv.ParaPattern(context, UserCusCdn);

            if (!string.IsNullOrWhiteSpace(SqlCount) && !SqlCount.StartsWith("sql@code") && !SqlCount.StartsWith("@code"))
            {
                if (!string.IsNullOrWhiteSpace(CusCondition)) SqlCount += " " + CusCondition;
            }
            else if (SqlCount.StartsWith("sql@code"))
            {
                SqlCount = Interface.Code.Get(SqlCount.Substring(3), context);
            }
            else if (SqlCount.StartsWith("@code"))
            {
                CodeSqlCount = int.Parse(Interface.Code.Get(SqlCount, context));
            }

            if (!string.IsNullOrWhiteSpace(CusCondition)) sql += " " + CusCondition;

            bool ISCusSQL = false;// sql.StartsWith("@CUS:");
            string CusSqlList = null;
            string CusSqlCount = null;
            if (ISCusSQL)//@CUS:@SEARCH@####@SEARCH@ @LIMIT@ @ORDER@ @BETWEEN@
            {
                string[] CusSqlItem = sql.Substring(5).Split(new string[] { "####" }, StringSplitOptions.None);
                CusSqlList = CusSqlItem[0];
                CusSqlCount = CusSqlItem[1];
            }

            string seach_sql = "";
            if (!schdefine.Equals("") && !schtext.Equals(""))
            {
                string[] cols = schdefine.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < cols.Length; i++)
                {
                    if (i > 0) seach_sql += " or ";
                    string[] schtextitem = schtext.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    seach_sql += "(";
                    for (int j = 0; j < schtextitem.Length; j++)
                    {
                        if (j > 0) seach_sql += " or ";
                        seach_sql += str.D2DD(cols[i]) + " like '%" + str.D2DD(schtextitem[j]) + "%'";
                    }
                    seach_sql += ")";
                }
                if (!seach_sql.Equals(""))
                {
                    seach_sql = " and(" + seach_sql + ")";
                }
            }
            if (!schstrict.Equals(""))
            {
                string[] items = schstrict.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in items)
                {
                    string sval = item.Split(':')[1];
                    if (sval.StartsWith("@Cdn{"))
                    {
                        seach_sql += " and " + Str.Decode(sval.Substring(5, sval.Length - 6)) + " ";
                    }
                    else
                    {
                        if (!sval.Equals(""))
                        {
                            if (sval.Equals("null"))
                            {
                                seach_sql += " and " + str.D2DD(item.Split(':')[0]) + " is null";
                            }
                            else if (sval.Equals("!null"))
                            {
                                seach_sql += " and " + str.D2DD(item.Split(':')[0]) + " is not null";
                            }
                            else
                            {
                                string pat = "=";
                                if (sval.StartsWith("!"))
                                {
                                    pat = "!=";
                                    sval = sval.Substring(1);
                                }
                                else if (sval.StartsWith(">="))
                                {
                                    pat = ">=";
                                    sval = sval.Substring(2);
                                }
                                else if (sval.StartsWith("<="))
                                {
                                    pat = "<=";
                                    sval = sval.Substring(2);
                                }
                                else if (sval.StartsWith(">"))
                                {
                                    pat = ">";
                                    sval = sval.Substring(1);
                                }
                                else if (sval.StartsWith("<"))
                                {
                                    pat = "<";
                                    sval = sval.Substring(1);
                                }
                                if (sval.Equals("empty")) sval = "";
                                seach_sql += " and " + str.D2DD(item.Split(':')[0]) + pat + (sval.StartsWith("(") ? sval.Replace("(", "").Replace(")", "") : ("'" + str.D2DD(sval) + "'"));
                            }
                        }
                    }
                }
            }
            if (!schadv.Equals("") && str.SQLSelectSafe(schadv))
            {
                seach_sql += " and " + schadv;
            }

            string UserCusAllSql = null;
            string UserCusCdnSql = "";
            UserCusCdn = UserCusCdn.Trim().Replace(";", "");
            if (UserCusCdn.ToUpper().StartsWith("AND") && str.SQLSelectSafe(UserCusCdn))
            {
                UserCusCdnSql = UserCusCdn;
            }
            //if (Interface.User.IsAdmin())
            //{
            //    if (UserCusCdn.ToUpper().StartsWith("SELECT") && str.SQLSelectSafe(UserCusCdn))
            //    {
            //        UserCusAllSql = UserCusCdn;
            //    }
            //}
            if (!UserCusCdnSql.Equals(""))
            {
                seach_sql += " " + UserCusCdnSql;
            }

            string sqlall = null;
            string SqlSelectAll = null;
            if (!ISCusSQL)
            {
                sql = sql + seach_sql;
                sql = sql.Replace(";", "");
                //sql = Sql.GetSqlForRemoveSameCols(sql);
                if (!string.IsNullOrWhiteSpace(SqlCount)) sqlall = SqlCount;
                else sqlall = "select count(*) as ca from (" + sql + ") tb1";
                SqlSelectAll = sql;
            }
            else
            {
                CusSqlList = CusSqlList.Replace("@SEARCH@", seach_sql).Replace(";", "");
                CusSqlCount = CusSqlCount.Replace("@SEARCH@", seach_sql).Replace(";", "");
                //CusSqlList = Sql.GetSqlForRemoveSameCols(CusSqlList);
                //CusSqlCount = Sql.GetSqlForRemoveSameCols(CusSqlCount);
                SqlSelectAll = CusSqlList;
            }

            string[] TreeParas = new string[3];
            if (IsTree)
            {
                RerationTree = adv.SQLSelectSafe(adv.GetSpecialBase(context, RerationTree, SiteID, true));
                TreeParas = RerationTree.Split(';');
                NumsPerPage = 0;
            }
            if (SysConst.DataBaseType.Equals(DataBase.MySql))
            {
                //log.Error("Demo Version Just Support Sqlite Database");
                if (!ISCusSQL)
                {
                    if (orderby.Equals(""))
                    {
                        sql += " " + DefaultOrder;
                    }
                    else
                    {
                        sql += "  order by " + orderby + " " + ordertype;
                    }
                    if (!NeedExport)
                    {
                        if (NumsPerPage > 0)
                        {
                            sql += " limit " + NumsPerPage * (CurPageNum - 1) + "," + NumsPerPage;
                        }
                    }
                    else
                    {
                        if (ExportMax > 0)
                        {
                            sql += " limit 0," + ExportMax;
                        }
                    }
                }
                else
                {
                    if (orderby.Equals(""))
                    {
                        CusSqlList = CusSqlList.Replace("@ORDER@", DefaultOrder);
                    }
                    else
                    {
                        CusSqlList = CusSqlList.Replace("@ORDER@", "order by " + orderby + " " + ordertype);
                    }
                    if (!NeedExport)
                    {
                        if (NumsPerPage > 0)
                        {
                            CusSqlList = CusSqlList.Replace("@LIMIT@", "limit " + NumsPerPage * (CurPageNum - 1) + "," + NumsPerPage);
                        }
                    }
                    else
                    {
                        if (ExportMax > 0)
                        {
                            CusSqlList = CusSqlList.Replace("@LIMIT@", "limit 0," + ExportMax);
                        }
                    }
                }
            }
            else if (SysConst.DataBaseType.Equals(DataBase.SqlServer))
            {
                //log.Error("Demo Version Just Support Sqlite Database");
                string orderbystr = "";
                if (orderby.Equals(""))
                {
                    orderbystr = DefaultOrder;
                }
                else
                {
                    orderbystr = "order by " + orderby + " " + ordertype;
                }
                if (!ISCusSQL)
                {
                    if (!NeedExport)
                    {
                        if (NumsPerPage > 0)
                        {
                            sql = "select * from (select Row_Number() over (" + orderbystr + ") as ft_rownum,* from (" + sql + ") as a) as ftlistt where ft_rownum between " + (NumsPerPage * (CurPageNum - 1) + 1) + " and " + (NumsPerPage * CurPageNum) + "";
                        }
                        else
                        {
                            sql += " " + orderbystr;
                        }
                    }
                    else
                    {
                        if (ExportMax > 0)
                        {
                            sql = "select * from (select Row_Number() over (" + orderbystr + ") as ft_rownum,* from (" + sql + ") as a) as ftlistt where ft_rownum between 1 and " + ExportMax + "";
                        }
                        else
                        {
                            sql += " " + orderbystr;
                        }
                    }
                }
                else
                {
                    if (!NeedExport)
                    {
                        if (NumsPerPage > 0)
                        {
                            CusSqlList = CusSqlList.Replace("@ORDER@", orderbystr).Replace("@BETWEEN@", "between " + (NumsPerPage * (CurPageNum - 1) + 1) + " and " + (NumsPerPage * CurPageNum));
                        }
                        else
                        {
                            CusSqlList = CusSqlList.Replace("@ORDER@", orderbystr);
                        }
                    }
                    else
                    {
                        if (ExportMax > 0)
                        {
                            CusSqlList = CusSqlList.Replace("@ORDER@", orderbystr).Replace("@BETWEEN@", "between 1 and " + ExportMax);
                        }
                        else
                        {
                            CusSqlList = CusSqlList.Replace("@ORDER@", orderbystr).Replace("@BETWEEN@", ">-1");
                        }
                    }
                }
            }
            else if (SysConst.DataBaseType.Equals(DataBase.Sqlite))
            {
                if (!ISCusSQL)
                {
                    if (orderby.Equals(""))
                    {
                        sql += " " + DefaultOrder;
                    }
                    else
                    {
                        sql += "  order by " + orderby + " " + ordertype;
                    }
                    if (!NeedExport)
                    {
                        if (NumsPerPage > 0)
                        {
                            sql += " limit " + NumsPerPage + " offset " + (NumsPerPage * (CurPageNum - 1));
                        }
                    }
                    else
                    {
                        if (ExportMax > 0)
                        {
                            sql += " limit " + ExportMax + " offset 0";
                        }
                    }
                }
                else
                {
                    if (orderby.Equals(""))
                    {
                        CusSqlList = CusSqlList.Replace("@ORDER@", DefaultOrder);
                    }
                    else
                    {
                        CusSqlList = CusSqlList.Replace("@ORDER@", "order by " + orderby + " " + ordertype);
                    }
                    if (!NeedExport)
                    {
                        if (NumsPerPage > 0)
                        {
                            CusSqlList = CusSqlList.Replace("@LIMIT@", "limit " + NumsPerPage + " offset " + (NumsPerPage * (CurPageNum - 1)));
                        }
                    }
                    else
                    {
                        if (ExportMax > 0)
                        {
                            CusSqlList = CusSqlList.Replace("@LIMIT@", "limit " + ExportMax + " offset 0");
                        }
                    }
                }
            }

            if (!ISCusSQL)
            {
                log.Debug(sql, "[List SQL]");
                log.Debug(sqlall, "[List SQL Count]");
            }
            else
            {
                log.Debug(CusSqlList, "[Custom Sql List]");
                log.Debug(CusSqlCount, "[Custom Sql Count]");
            }
            if (IsCacuRow)
            {
                log.Debug(SqlSelectAll, "[Cacu List All]");
            }

            ArrayList ColumnAlign = new ArrayList();
            ArrayList ColumnData = new ArrayList();
            ArrayList ColumnLink = new ArrayList();
            ArrayList ColumnTarget = new ArrayList();
            ArrayList ColumnOpen = new ArrayList();
            ArrayList ColumnSaveCol = new ArrayList();
            string[] rows = RowAll.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            ArrayList a0 = new ArrayList();
            Hashtable a0hash = new Hashtable();
            foreach (string rowstr in rows)
            {
                string row = rowstr.Substring(0, rowstr.IndexOf("&&&")).Trim();
                string[] rowcols = row.Split('#');
                string title = rowcols[0];
                if (a0.Contains(title)) title += "_sametitle_" + str.GetCombID();
                a0.Add(title);
                a0hash.Add(title, new object[] { ((rowcols.Length > 7 && rowcols[7].Equals("1")) ? 1 : 0), rowcols[2].Split(';')[0], rowcols[2].Split(';')[1], rowstr });
            }
            ArrayList b0 = new ArrayList();
            Hashtable b0hash = new Hashtable();
            string[] rows_cuscol = ColDefine_Cur.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rowstr in rows_cuscol)
            {
                string[] rowcols = rowstr.Split(new string[] { "##" }, StringSplitOptions.None);
                b0.Add(rowcols[0]);
                b0hash.Add(rowcols[0], new object[] { rowcols[1], rowcols[2], rowcols[3] });
            }
            List<string> TitleList = new List<string>();
            foreach (string title in b0)
            {
                if (a0.Contains(title))
                {
                    if (((object[])b0hash[title])[0].ToString().Equals("0"))
                    {
                        if (NeedExport) TitleList.Add(title);
                        string rowstr = ((object[])a0hash[title])[3].ToString();
                        string width = ((object[])b0hash[title])[1].ToString().Trim().Equals("") ? ((object[])a0hash[title])[1].ToString() : ((object[])b0hash[title])[1].ToString().Trim();
                        string align = ((object[])b0hash[title])[2].ToString().Trim().Equals("") ? ((object[])a0hash[title])[2].ToString() : ((object[])b0hash[title])[2].ToString().Trim();
                        ColumnOpen.Add(List_IsColumnOpen(context, rowstr.Substring(rowstr.IndexOf("&&&") + 3).Trim()).ToString());
                        string row = rowstr.Substring(0, rowstr.IndexOf("&&&")).Trim();
                        string[] rowcols = row.Split('#');
                        ColumnData.Add(str.GetDecode(rowcols[1]));
                        ColumnAlign.Add(align);
                        if (rowcols.Length > 4 && !(rowcols.Length > 6 && !rowcols[6].Equals("")))
                        {
                            ColumnLink.Add(rowcols[4]);
                        }
                        else
                        {
                            ColumnLink.Add(null);
                        }
                        if (rowcols.Length > 5)
                        {
                            ColumnTarget.Add(rowcols[5]);
                        }
                        else
                        {
                            ColumnTarget.Add("");
                        }
                        if (rowcols.Length > 6)
                        {
                            ColumnSaveCol.Add(rowcols[6]);
                        }
                        else
                        {
                            ColumnSaveCol.Add(null);
                        }
                    }
                }
            }
            foreach (string title in a0)
            {
                if (!b0.Contains(title))
                {
                    if (((object[])a0hash[title])[0].ToString().Equals("0"))
                    {
                        if (NeedExport) TitleList.Add((title.IndexOf("_sametitle_") >= 0) ? title.Substring(0, title.IndexOf("_sametitle_")) : title);
                        string rowstr = ((object[])a0hash[title])[3].ToString();
                        string width = ((object[])a0hash[title])[1].ToString();
                        string align = ((object[])a0hash[title])[2].ToString();
                        ColumnOpen.Add(List_IsColumnOpen(context, rowstr.Substring(rowstr.IndexOf("&&&") + 3).Trim()).ToString());
                        string row = rowstr.Substring(0, rowstr.IndexOf("&&&")).Trim();
                        string[] rowcols = row.Split('#');
                        ColumnData.Add(str.GetDecode(rowcols[1]));
                        ColumnAlign.Add(align);
                        if (rowcols.Length > 4 && !(rowcols.Length > 6 && !rowcols[6].Equals("")))
                        {
                            ColumnLink.Add(rowcols[4]);
                        }
                        else
                        {
                            ColumnLink.Add(null);
                        }
                        if (rowcols.Length > 5)
                        {
                            ColumnTarget.Add(rowcols[5]);
                        }
                        else
                        {
                            ColumnTarget.Add("");
                        }
                        if (rowcols.Length > 6)
                        {
                            ColumnSaveCol.Add(rowcols[6]);
                        }
                        else
                        {
                            ColumnSaveCol.Add(null);
                        }
                    }
                }
            }
            //if (!CSVTitle.Equals("")) CSVTitle = CSVTitle.Substring(1);
            //for (int rowi = 0; rowi < rows.Length; rowi++)
            //{
            //    ColumnOpen.Add(List_IsColumnOpen(context, rows[rowi].Substring(rows[rowi].IndexOf("&&&") + 3).Trim()).ToString());
            //    string row = rows[rowi].Substring(0, rows[rowi].IndexOf("&&&")).Trim();
            //    string[] rowcols = row.Split('#');
            //    ColumnData.Add(str.GetDecode(rowcols[1]));
            //    ColumnAlign.Add(rowcols[2].Split(';')[1]);
            //    if (rowcols.Length > 4)
            //    {
            //        ColumnLink.Add(rowcols[4]);
            //    }
            //    else
            //    {
            //        ColumnLink.Add(null);
            //    }
            //    if (rowcols.Length > 5)
            //    {
            //        ColumnTarget.Add(rowcols[5]);
            //    }
            //    else
            //    {
            //        ColumnTarget.Add("");
            //    }
            //    if (rowcols.Length > 6)
            //    {
            //        ColumnSaveCol.Add(rowcols[6]);
            //    }
            //    else
            //    {
            //        ColumnSaveCol.Add(null);
            //    }
            //    if (ColumnSaveCol[rowi] != null && !ColumnSaveCol[rowi].ToString().Equals(""))
            //    {
            //        ColumnLink[rowi] = null;
            //    }
            //}
            DateTime OPStart = DateTime.Now;
            s = "{\"data\":[";
            if (NeedExport) ExportDT = new DataTable();
            DB db = null; DB db2 = null;
            if (CustomConnection == null || CustomConnection.Trim() == "")
            {
                db = new DB(SysConst.ConnString_ReadOnly);
                db.Open();
                db2 = new DB(SysConst.ConnString_ReadOnly);
                db2.Open();
            }
            else
            {
                CustomConnection = Interface.Code.Get(CustomConnection, context);
                db = new DB(CustomConnection); ;
                db.Open();
                db2 = new DB(CustomConnection);
                db2.Open();
            }
            int CountAll = 0;
            try
            {
                if (NeedExport)
                {
                    foreach (string col in TitleList)
                    {
                        ExportDT.Columns.Add(col);
                    }
                }
                string[] restrs = List_Loop(db, db2, 0, IsTree, TreeParas, null, 0, "", NeedExport, ISCusSQL ? CusSqlList : sql, ColumnOpen, ColumnAlign, ColumnData, ColumnSaveCol, ColumnLink, ColumnTarget, context, SiteID, Consts, MainTable, RateNumType, CurPageNum, NumsPerPage, IsBlockData, BlockDataDefine, ExportDT);
                s += restrs[0];
                //if (NeedExport)
                //{
                //    exportstr += CSVTitle + "\r\n" + restrs[1];
                //}
                CountAll = CodeSqlCount ?? db.GetInt(ISCusSQL ? CusSqlCount : sqlall);
                string cacurow = "";
                if (IsCacuRow)
                {
                    if (CacuRowData.StartsWith("@code(")) cacurow = Interface.Code.Get(CacuRowData, context);
                    else
                    {
                        cacurow = CacuRowData;
                        r = new Regex(@"{[^}]*}");
                        mc = r.Matches(CacuRowData);
                        foreach (Match m in mc)
                        {
                            string pattern = m.Value.Replace("{", "").Replace("}", "");
                            object cacuval = "";
                            sql = "select " + pattern + " as cacuval from (" + SqlSelectAll + ") as cacttb";
                            try
                            {
                                cacuval = db.GetObject(sql);
                                if (cacuval == null) cacuval = "";
                            }
                            catch (Exception ex)
                            {
                                cacuval = ex.Message;
                                log.Error(ex);
                            }
                            cacurow = cacurow.Replace(m.Value, cacuval.ToString());
                        }
                    }
                }
                if (NumsPerPage > 0 && CusTurnBtm != null && CusTurnBtm.Trim() != "")
                {
                    CusTurnBtm = Interface.Code.Get(CusTurnBtm.Replace("$1", partid).Replace("$2", CountAll.ToString()).Replace("$3", NumsPerPage.ToString()).Replace("$4", (((CountAll - 1) / NumsPerPage) + 1).ToString()).Replace("$5", CurPageNum.ToString()), context);
                }
                else CusTurnBtm = "";
                if (NumsPerPage > 0 && CusTurnTop != null && CusTurnTop.Trim() != "")
                {
                    CusTurnTop = Interface.Code.Get(CusTurnTop.Replace("$1", partid).Replace("$2", CountAll.ToString()).Replace("$3", NumsPerPage.ToString()).Replace("$4", (((CountAll - 1) / NumsPerPage) + 1).ToString()).Replace("$5", CurPageNum.ToString()), context);
                }
                else CusTurnTop = "";
                s += "],\"cacu\":\"" + func.escape(cacurow) + "\",\"turnbtm\":\"" + func.escape(CusTurnBtm) + "\",\"turntop\":\"" + func.escape(CusTurnTop) + "\",\"para\":{\"count\":\"" + CountAll + "\",\"pagesize\":\"" + NumsPerPage + "\",\"pagenum\":\"" + CurPageNum + "\",\"ratenumtype\":\"" + RateNumType + "\"}}";
            }
            catch (Exception ex)
            {
                //s = ex.Message;
                log.Error(ex);
                res.WriteAsync("#fail:" + func.escape(ex.Message));
                return;
            }
            finally
            {
                db.Close();
                db2.Close();
            }
            res.Clear();
            //res.Buffer = true;
            //res.Charset = Encoding.UTF8.ToString();
            //res.ContentEncoding = System.Text.Encoding.UTF8;
            try
            {
                if (NeedExport && ExportSave)
                {
                    string FileID = str.GetCombID();
                    if (ExportSaveFileCap == null || ExportSaveFileCap.Equals("")) ExportSaveFileCap = FileID;
                    string dir = str.GetYearMonth();
                    string alldir = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar +"_ftfiles" + Path.DirectorySeparatorChar + dir;
                    if (!Directory.Exists(alldir))
                    {
                        Directory.CreateDirectory(alldir);
                    }
                    string FilePath = "/_ftfiles/" + dir + "/" + FileID + ".xlsx";
                    export.ExcelSave(ExportDT, FilePath);
                    //FileStream FS = new FileStream(alldir + "\\" + FileID + ".csv", FileMode.Create);
                    //StreamWriter SW = new StreamWriter(FS,Encoding.Default);
                    //SW.Write(exportstr);
                    //SW.Close();
                    //FS.Close();
                    int CostMinutes = Convert.ToInt32(((TimeSpan)(DateTime.Now - OPStart)).TotalMinutes);
                    sql = "insert into ft_ctrl_list_files(partid,fileid,filepath,filecap,userid,addtime,rows,minutes)";
                    sql += "values('" + str.D2DD(partid) + "','" + FileID + "','" + str.D2DD(FilePath) + "','" + str.D2DD(ExportSaveFileCap.Length > 36 ? ExportSaveFileCap.Substring(0, 36) : ExportSaveFileCap) + "','" + str.D2DD(UserTool.CurUserID()??"") + "','" + str.GetDateTime() + "'," + CountAll + "," + CostMinutes + ")";
                    DB.ExecSQL(sql,SysConst.ConnectionStr_FTDP);
                    res.WriteAsync("OK");
                }
                else
                {
                    if (!NeedExport)
                    {
                        res.ContentType = "application/Json;charset=utf-8";
                        res.WriteAsync(s);
                    }
                    else
                    {
                        if (!PageExport)
                        {
                            res.ContentType = "application/Json;charset=utf-8";
                            res.WriteAsync(func.escape(ExportDT.ToString()));
                        }
                        else
                        {
                            export.Excel(context, ExportDT);
                            //return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                res.WriteAsync("#fail:" + func.escape(ex.Message));
                return;
            }
            //res.End();
        }
        private static string[] List_Loop(DB db, DB db2, int ItemRate, bool IsTree, string[] TreeParas, string OldTreeColVal, int TreeLevel, string TreePID, bool NeedExport, string sql, ArrayList ColumnOpen, ArrayList ColumnAlign, ArrayList ColumnData, ArrayList ColumnSaveCol, ArrayList ColumnLink, ArrayList ColumnTarget, HttpContext Context, string SiteID, string[] Consts, string MainTable, int RateNumType, int CurPageNum, int NumsPerPage, bool IsBlockData, string BlockDataDefine, DataTable ExportDT)
        {
            string s = "";
            string exportstr = "";
            string TreeColVal = null;
            string TreeSQL = sql;
            string TreeRowAppendHTML = "";
            if (IsTree)
            {
                for (int i = 0; i < TreeLevel; i++)
                {
                    TreeRowAppendHTML += "&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                TreeLevel++;
                if (OldTreeColVal == null)
                {
                    TreeSQL = "select * from (" + sql + ") a where " + TreeParas[2];
                }
                else
                {
                    TreeSQL = "select * from (" + sql + ") a where " + TreeParas[1] + "='" + str.D2DD(OldTreeColVal) + "'";
                }
            }
            DR rdr = db.OpenRecord(TreeSQL);
            DR dr2 = null;
            BlockDataDefine = adv.GetSpecialBase(Context, BlockDataDefine, SiteID);
            int LoopI = 0;
            while (rdr.Read())
            {
                if (IsBlockData)
                {
                    if (LoopI > 0) s += ",";
                    s += "{\"fid\":\"" + rdr.GetString(rdr.Key(MainTable).KeyName) + "\",\"row\":";
                    LoopI++;
                    string BlockDataDefine_T = BlockDataDefine;
                    Regex r = new Regex(@"\[[\w\.]*\]");
                    MatchCollection mc = r.Matches(BlockDataDefine_T);
                    foreach (Match m in mc)
                    {
                        //rdr.GetValue(rdr.GetOrdinal(m.Value.Replace("[", "").Replace("]", ""))) == null ? "" : rdr.GetValue(rdr.GetOrdinal(m.Value.Replace("[", "").Replace("]", ""))).ToString()
                        BlockDataDefine_T = BlockDataDefine_T.Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""),MainTable).val);
                    }
                    BlockDataDefine_T = Interface.Code.Get(BlockDataDefine_T, Context);
                    s += "\"" + func.escape(BlockDataDefine_T) + "\"}";
                    continue;
                }
                ItemRate++;
                string rowdata = "";
                if (LoopI > 0) s += ",";
                s += "{\"fid\":\"" + rdr.GetString(rdr.Key(MainTable).KeyName) + "\",\"row\":[";
                LoopI++;
                for (int i = 0; i < ColumnOpen.Count; i++)
                {
                    if (!bool.Parse(ColumnOpen[i].ToString())) continue;
                    string rowtddata = "";
                    string[] rowdatas = ColumnData[i].ToString().Split(';');
                    for (int j = 0; j < rowdatas.Length; j++)
                    {
                        if (rowdatas[j].Equals("")) continue;
                        rowdatas[j] = rowdatas[j].Trim();
                        string rowwilladddata = "";
                        if (rowdatas[j].StartsWith("@str("))
                        {
                            //string dataitem = rowdatas[j].Replace("@str(", "").Replace(")", "");
                            string dataitem = rowdatas[j].Replace("#)#", "#*1*#").Replace("@str(", "").Replace(")", "").Replace("#*1*#", ")");
                            dataitem = adv.GetSpecialBase(Context, dataitem, SiteID);
                            Regex r = new Regex(@"\[[\w\.]*\]");
                            MatchCollection mc = r.Matches(dataitem);
                            foreach (Match m in mc)
                            {
                                dataitem = dataitem.Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                            }
                            rowwilladddata = dataitem;
                        }
                        //else if (rowdatas[j].StartsWith("@getConst("))
                        //{
                        //    rowwilladddata = Consts[Convert.ToInt32(rowdatas[j].Replace("@getConst(", "").Replace(")", "")) - 1];
                        //}
                        //else if (rowdatas[j].StartsWith("@getValueAll("))
                        //{
                        //    string eleid = rowdatas[j].Replace("@getValueAll(", "").Replace(")", "");
                        //    try
                        //    {
                        //        string elevalue = "/" + rdr.GetValue(rdr.GetOrdinal(eleid)).ToString();
                        //        string sqlele = "select evalue from " + MainTable + "_dy where fid='" + str.D2DD(rdr.GetString("fid")) + "' and eid='" + str.D2DD(eleid) + "' order by erate";
                        //        dr2 = db2.OpenRecord(sqlele);
                        //        while (dr2.Read())
                        //        {
                        //            elevalue += "/" + dr2.GetValue(0).ToString();
                        //        }
                        //        dr2.Close();
                        //        if (!elevalue.Equals("")) elevalue = elevalue.Substring(1);
                        //        rowwilladddata = elevalue;
                        //    }
                        //    catch (Exception e) { rowwilladddata = e.Message; }
                        //}
                        //else if (rowdatas[j].StartsWith("@getPValue("))
                        //{
                        //    string dataitem = rowdatas[j].Replace("@getPValue(", "").Replace(")", "");
                        //    string pdata = dataitem.Split(',')[0];
                        //    string peleid = dataitem.Split(',')[1];
                        //    string sqlele = "select " + peleid + " from ft_" + SiteID + "_f_" + pdata + " where fid='" + str.D2DD(rdr.GetString("pid")) + "'";
                        //    try
                        //    {
                        //        string elevalue = "";
                        //        dr2 = db2.OpenRecord(sqlele);
                        //        if (dr2.Read())
                        //        {
                        //            elevalue = dr2.GetValue(0).ToString();
                        //        }
                        //        dr2.Close();
                        //        rowwilladddata = elevalue;
                        //    }
                        //    catch (Exception e) { rowwilladddata = e.Message; }
                        //}
                        //else if (rowdatas[j].StartsWith("@getPValueAll("))
                        //{
                        //    string dataitem = rowdatas[j].Replace("@getPValueAll(", "").Replace(")", "");
                        //    string pdata = dataitem.Split(',')[0];
                        //    string peleid = dataitem.Split(',')[1];
                        //    string sqlele = "select " + peleid + " from ft_" + SiteID + "_f_" + pdata + " where fid='" + str.D2DD(rdr.GetString("pid")) + "'";
                        //    try
                        //    {
                        //        string elevalue = "";
                        //        dr2 = db2.OpenRecord(sqlele);
                        //        elevalue = "";
                        //        if (dr2.Read())
                        //        {
                        //            elevalue += "/" + dr2.GetValue(0).ToString();
                        //        }
                        //        dr2.Close();
                        //        sqlele = "select evalue from ft_" + SiteID + "_f_" + pdata + "_dy where fid='" + str.D2DD(rdr.GetString("pid")) + "' and eid='" + str.D2DD(peleid) + "' order by erate";
                        //        dr2 = db2.OpenRecord(sqlele);
                        //        while (dr2.Read())
                        //        {
                        //            elevalue += "/" + dr2.GetValue(0).ToString();
                        //        }
                        //        dr2.Close();
                        //        if (!elevalue.Equals("")) elevalue = elevalue.Substring(1);
                        //        rowwilladddata = elevalue;
                        //    }
                        //    catch (Exception e) { rowwilladddata = e.Message; }
                        //}
                        else if (rowdatas[j].StartsWith("@SQL{"))
                        {
                            int firstIndex = rowdatas[j].IndexOf('{');
                            int lastIndex = rowdatas[j].LastIndexOf('}');
                            string itemsql = rowdatas[j].Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                            itemsql = adv.GetSpecialBase(Context, itemsql, SiteID, true);
                            itemsql = adv.CodePattern(Context, itemsql);
                            itemsql = adv.ParaPattern(Context, itemsql);
                            Regex r = new Regex(@"\[[\w\.]*\]");
                            MatchCollection mc = r.Matches(itemsql);
                            foreach (Match m in mc)
                            {
                                itemsql = itemsql.Replace(m.Value, str.D2DD(rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val));
                            }
                            try
                            {
                                string elevalue = "";
                                log.Debug(itemsql, "[List Loop ItemSql]");
                                dr2 = db2.OpenRecord(itemsql);
                                if (dr2.Read())
                                {
                                    elevalue = dr2.GetValue(0).ToString();
                                }
                                dr2.Close();
                                rowwilladddata = (elevalue == null ? "" : elevalue);
                            }
                            catch (Exception e)
                            {
                                rowwilladddata = e.Message;
                                log.Error(e.Message + ".itemsql:" + itemsql, "[List @SQL]");
                            }
                        }
                        else if (rowdatas[j].StartsWith("@api_"))
                        {
                            rowwilladddata = "@api";
                        }
                        else if (rowdatas[j].StartsWith("$"))
                        {
                            string[] coldatasbuts = rowdatas[j].Replace("$", "").Split('|');
                            if (coldatasbuts[2].StartsWith("@getConst("))
                            {
                                coldatasbuts[2] = Consts[Convert.ToInt32(coldatasbuts[2].Replace("@getConst(", "").Replace(")", "")) - 1];
                            }

                            if (coldatasbuts[0].Trim().Equals("") || ("," + coldatasbuts[0].Trim() + ",").IndexOf("," + rdr.GetInt32(rdr.GetOrdinal("flow")).ToString() + ",") >= 0)
                            {
                                if (int.Parse(coldatasbuts[1]) == 0)
                                {
                                    if (coldatasbuts[3] != "")
                                    {
                                        coldatasbuts[3] = adv.GetSpecialBase(Context, coldatasbuts[3], SiteID);
                                        Regex r = new Regex(@"\[[\w\.]*\]");
                                        MatchCollection mc = r.Matches(coldatasbuts[3]);
                                        foreach (Match m in mc)
                                        {
                                            coldatasbuts[3] = coldatasbuts[3].Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                                        }
                                        if (ColumnSaveCol[i] == null || ColumnSaveCol[i].ToString().Equals(""))
                                        {
                                            rowwilladddata = "<a href=\"javascript:void(0)\" onclick=\"" + coldatasbuts[3] + "\">" + coldatasbuts[2] + "</a>";
                                            //if (coldatasbuts[3].IndexOf('?') < 0)
                                            //{
                                            //    rowwilladddata = "<a href=\"javascript:void(0)\" onclick=\"dl_openWindow('" + coldatasbuts[3] + "?rmd='+Math.random(),'" + coldatasbuts[4] + "')\">" + coldatasbuts[2] + "</a>";
                                            //}
                                            //else
                                            //{
                                            //    rowwilladddata = "<a href=\"javascript:void(0)\" onclick=\"dl_openWindow('" + coldatasbuts[3] + "&rmd='+Math.random(),'" + coldatasbuts[4] + "')\">" + coldatasbuts[2] + "</a>";
                                            //}
                                        }
                                        else
                                        {
                                            rowwilladddata = coldatasbuts[2];
                                        }

                                    }
                                    else
                                    {
                                        rowwilladddata = coldatasbuts[2];
                                    }
                                }
                                else if (int.Parse(coldatasbuts[1]) == 1)
                                {
                                    coldatasbuts[3] = adv.GetSpecialBase(Context, coldatasbuts[3], SiteID);
                                    Regex r = new Regex(@"\[[\w\.]*\]");
                                    MatchCollection mc = r.Matches(coldatasbuts[3]);
                                    foreach (Match m in mc)
                                    {
                                        coldatasbuts[3] = coldatasbuts[3].Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                                    }
                                    if (ColumnSaveCol[i] == null || ColumnSaveCol[i].ToString().Equals(""))
                                    {
                                        rowwilladddata = "<button type=button onclick=\"" + coldatasbuts[3] + "\" class=\"layui-btn _button\">" + coldatasbuts[2] + "</button>";
                                        //if (coldatasbuts[3].IndexOf('?') < 0)
                                        //{
                                        //    rowwilladddata = "<input type=button value='" + coldatasbuts[2] + "' onclick=\"dl_openWindow('" + coldatasbuts[3] + "?rmd='+Math.random(),'" + coldatasbuts[4] + "')\"/>";
                                        //}
                                        //else
                                        //{
                                        //    rowwilladddata = "<input type=button value='" + coldatasbuts[2] + "' onclick=\"dl_openWindow('" + coldatasbuts[3] + "&rmd='+Math.random(),'" + coldatasbuts[4] + "')\"/>";
                                        //}
                                    }
                                    else
                                    {
                                        rowwilladddata = coldatasbuts[2];
                                    }
                                }
                            }
                        }
                        else if (rowdatas[j].StartsWith("~"))
                        {
                            //~0|(0|a1)(1|@getConst(7))(other|[a.fid])~
                            //~1|abc(null|xx)(0|a1)(1|a2)(other|a3)~
                            //~2|abc(null|xx)(0|a1)(1|a2)(other|a3)~
                            int extpfirst = 0;
                            int extplast = 0;
                            extpfirst = rowdatas[j].IndexOf('|');
                            int extlevel = int.Parse(rowdatas[j].Substring(1, extpfirst - 1));
                            string extflowvalue = "null";
                            if (extlevel == 0)
                            {
                                extflowvalue = rdr.GetInt32("flow").ToString();
                            }
                            else
                            {

                            }
                            int _pass1 = rowdatas[j].IndexOf("(" + extflowvalue + "|");
                            int _pass2 = rowdatas[j].IndexOf("(" + extflowvalue + ",");
                            int _pass3 = rowdatas[j].IndexOf("," + extflowvalue + "|");
                            int _pass4 = rowdatas[j].IndexOf("," + extflowvalue + ",");
                            if (_pass1 < 0 && _pass2 < 0 && _pass3 < 0 && _pass4 < 0)//多状态支持 (2,3,4|abc)
                            {
                                extflowvalue = "other";
                                _pass1 = rowdatas[j].IndexOf("(" + extflowvalue + "|");
                                _pass2 = rowdatas[j].IndexOf("(" + extflowvalue + ",");
                                _pass3 = rowdatas[j].IndexOf("," + extflowvalue + "|");
                                _pass4 = rowdatas[j].IndexOf("," + extflowvalue + ",");
                            }

                            extpfirst = (_pass1 >= 0 ? _pass1 : ((_pass2 >= 0 ? _pass2 : ((_pass3 >= 0 ? _pass3 : _pass4)))));
                            if (extpfirst >= 0)
                            {
                                extplast = rowdatas[j].IndexOf(')', extpfirst);
                                string dataitem = rowdatas[j].Substring(extpfirst + 1, extplast - extpfirst - 1).Split('|')[1];
                                dataitem = dataitem.Replace("#}", ")");
                                if (dataitem.StartsWith("@getConst("))
                                {
                                    rowwilladddata = Consts[int.Parse(dataitem.Replace("@getConst(", "")) - 1];
                                }
                                else
                                {
                                    Regex r = new Regex(@"\[[\w\.]*\]");
                                    MatchCollection mc = r.Matches(dataitem);
                                    foreach (Match m in mc)
                                    {
                                        dataitem = dataitem.Replace(m.Value, rdr.GetValue(rdr.GetOrdinal(m.Value.Replace("[", "").Replace("]", ""))) == null ? "" : rdr.GetValue(rdr.GetOrdinal(m.Value.Replace("[", "").Replace("]", ""))).ToString());
                                    }
                                    rowwilladddata = dataitem;
                                }
                            }
                        }
                        else if (rowdatas[j].StartsWith("!"))
                        {
                            //!abcd(null|x1)(|x2)(val0|x3)(other|x4)!
                            string passeleid = rowdatas[j].Substring(1, rowdatas[j].IndexOf('(') - 1);
                            string passelevalue = rdr.CommonValue(passeleid, MainTable).val;

                            int extpfirst = 0;
                            int extplast = 0;

                            int _pass1 = rowdatas[j].IndexOf("(" + passelevalue + "|");
                            int _pass2 = rowdatas[j].IndexOf("(" + passelevalue + ",");
                            int _pass3 = rowdatas[j].IndexOf("," + passelevalue + "|");
                            int _pass4 = rowdatas[j].IndexOf("," + passelevalue + ",");
                            if (_pass1 < 0 && _pass2 < 0 && _pass3 < 0 && _pass4 < 0)//多值支持 (2,3,4|abc)
                            {
                                passelevalue = "other";
                                _pass1 = rowdatas[j].IndexOf("(" + passelevalue + "|");
                                _pass2 = rowdatas[j].IndexOf("(" + passelevalue + ",");
                                _pass3 = rowdatas[j].IndexOf("," + passelevalue + "|");
                                _pass4 = rowdatas[j].IndexOf("," + passelevalue + ",");
                            }

                            extpfirst = (_pass1 >= 0 ? _pass1 : ((_pass2 >= 0 ? _pass2 : ((_pass3 >= 0 ? _pass3 : _pass4)))));
                            if (extpfirst >= 0)
                            {
                                extplast = rowdatas[j].IndexOf(')', extpfirst);
                                string dataitem = rowdatas[j].Substring(extpfirst + 1, extplast - extpfirst - 1).Split('|')[1];
                                dataitem = dataitem.Replace("#}", ")");
                                if (dataitem.StartsWith("@getConst("))
                                {
                                    rowwilladddata = Consts[int.Parse(dataitem.Replace("@getConst(", "")) - 1];
                                }
                                else
                                {
                                    Regex r = new Regex(@"\[[\w\.]*\]");
                                    MatchCollection mc = r.Matches(dataitem);
                                    foreach (Match m in mc)
                                    {
                                        dataitem = dataitem.Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                                    }
                                    rowwilladddata = dataitem;
                                }
                            }
                        }
                        else if (rowdatas[j].StartsWith("@code("))
                        {
                            string dataitem = adv.GetSpecialBase(Context, rowdatas[j], SiteID);
                            Regex r = new Regex(@"\[[\w\.]*\]");
                            MatchCollection mc = r.Matches(dataitem);
                            foreach (Match m in mc)
                            {
                                dataitem = dataitem.Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                            }
                            rowwilladddata = Interface.Code.Get(dataitem, Context);
                        }
                        else if (rowdatas[j].StartsWith("@para{"))
                        {
                            string dataitem = adv.GetSpecialBase(Context, rowdatas[j], SiteID);
                            Regex r = new Regex(@"\[[\w\.]*\]");
                            MatchCollection mc = r.Matches(dataitem);
                            foreach (Match m in mc)
                            {
                                dataitem = dataitem.Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                            }
                            rowwilladddata = adv.ParaPattern(Context, adv.CodePattern(Context, dataitem));
                        }
                        else if (rowdatas[j].StartsWith("@enum("))
                        {
                            string dataitem = adv.GetSpecialBase(Context, rowdatas[j], SiteID);
                            Regex r = new Regex(@"\[[\w\.]*\]");
                            MatchCollection mc = r.Matches(dataitem);
                            foreach (Match m in mc)
                            {
                                dataitem = dataitem.Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                            }
                            rowwilladddata = adv.EnumPattern(adv.CodePattern(Context, dataitem));
                        }
                        else
                        {
                            rowwilladddata = rdr.CommonValue(rowdatas[j],MainTable).val;
                        }
                        if (!rowtddata.Equals(""))
                        {
                            rowtddata += " ";
                        }
                        rowtddata += rowwilladddata;

                    }
                    if (ColumnLink[i] != null && !ColumnLink[i].ToString().Trim().Equals(""))
                    {
                        string ColLink = ColumnLink[i].ToString();
                        ColLink = adv.GetSpecialBase(Context, ColLink, SiteID);
                        Regex r = new Regex(@"\[[\w\.]*\]");
                        MatchCollection mc = r.Matches(ColLink);
                        foreach (Match m in mc)
                        {
                            ColLink = ColLink.Replace(m.Value, rdr.CommonValue(m.Value.Replace("[", "").Replace("]", ""), MainTable).val);
                        }
                        rowtddata = "<a href=\"javascript:void(0)\" onclick=\"" + ColLink + "\">" + rowtddata + "</a>";
                        //if (ColLink.IndexOf('?') < 0)
                        //{
                        //    rowtddata = "<a href=\"javascript:void(0)\" onclick=\"dl_openWindow('" + ColLink + "?rmd='+Math.random(),'" + ColumnTarget[i].ToString() + "')\">" + rowtddata + "</a>";
                        //}
                        //else
                        //{
                        //    rowtddata = "<a href=\"javascript:void(0)\" onclick=\"dl_openWindow('" + ColLink + "&rmd='+Math.random(),'" + ColumnTarget[i].ToString() + "')\">" + rowtddata + "</a>";
                        //}
                    }
                    string SaveVal = "";
                    if (ColumnSaveCol[i] != null && !ColumnSaveCol[i].ToString().Equals(""))
                    {
                        SaveVal = ColumnSaveCol[i].ToString();
                    }
                    if (i == 0 && IsTree)
                    {
                        rowdata = (TreeRowAppendHTML + "<img src='/lib/list.res/minus.gif' style='cursor:pointer' onclick='doTree(this)' class='_treeimg' id='" + TreePID + "_" + ItemRate + "_'/> ") + rowdata;
                    }
                    if (i > 0) s += ",";
                    s += "[\"" + func.escape(rowtddata) + "\",\"" + ColumnAlign[i].ToString() + "\",\"" + SaveVal + "\"]";

                    if (NeedExport)
                    {
                        //if (i > 0) exportstr += ",";
                        //exportstr += str.CSVConvert(rowtddata);
                        if (i == 0)
                        {
                            ExportDT.Rows.Add(new string[] { FTFrame.Tool.str.HTML2Text(rowtddata) });
                        }
                        else
                        {
                            ExportDT.Rows[ExportDT.Rows.Count - 1][i] = FTFrame.Tool.str.HTML2Text(rowtddata);
                        }
                    }
                }
                s += "]}";
                //if (NeedExport)
                //{
                //    exportstr += "\r\n";
                //}

                if (IsTree)
                {
                    TreeColVal = rdr.GetString(TreeParas[0]); if (TreeColVal == null) TreeColVal = "";
                    DB _db = new DB(); _db.Open();
                    DB _db2 = new DB(); _db2.Open();
                    try
                    {
                        string[] restrs = List_Loop(_db, _db2, ItemRate, IsTree, TreeParas, TreeColVal, TreeLevel, TreePID + "_" + ItemRate + "_", NeedExport, sql, ColumnOpen, ColumnAlign, ColumnData, ColumnSaveCol, ColumnLink, ColumnTarget, Context, SiteID, Consts, MainTable, RateNumType, CurPageNum, NumsPerPage, IsBlockData, BlockDataDefine, ExportDT);
                        s += restrs[0];
                        if (NeedExport)
                        {
                            exportstr += restrs[1];
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    finally
                    {
                        _db.Close();
                        _db2.Close();
                    }
                };
            }
            rdr.Close();
            return new string[] { s, exportstr };
        }
        public static bool List_IsColumnOpen(HttpContext context, string rowcdt)
        {
            if (rowcdt == null || rowcdt.Trim().Equals("")) return true;
            if (rowcdt.StartsWith("@code("))
            {
                string val = Interface.Code.GetObject(rowcdt, context).ToString();
                return val == null || val == "1";
            }
            string paraleft = "";
            string pararight = "";
            int index = 0;
            index = rowcdt.IndexOf("==");
            if (index > 0)
            {
                paraleft = rowcdt.Substring(0, index).Replace("\"", "");
                pararight = rowcdt.Substring(index + 2).Replace("\"", "");
                paraleft = adv.GetSpecialBase(context, paraleft, null);
                pararight = adv.GetSpecialBase(context, pararight, null);
                return paraleft.Equals(pararight);
            }
            else
            {
                index = rowcdt.IndexOf("!=");
                if (index > 0)
                {
                    paraleft = rowcdt.Substring(0, index).Replace("\"", "");
                    pararight = rowcdt.Substring(index + 2).Replace("\"", "");
                    paraleft = adv.GetSpecialBase(context, paraleft, null);
                    pararight = adv.GetSpecialBase(context, pararight, null);
                    return !paraleft.Equals(pararight);
                }
                else
                {
                    index = rowcdt.IndexOf("=?");
                    if (index > 0)
                    {
                        paraleft = rowcdt.Substring(0, index).Replace("\"", "");
                        pararight = rowcdt.Substring(index + 2).Replace("\"", "");
                        paraleft = adv.GetSpecialBase(context, paraleft, null);
                        pararight = adv.GetSpecialBase(context, pararight, null);
                        return paraleft.Contains(pararight);
                    }
                    else
                    {
                        index = rowcdt.IndexOf("!?");
                        if (index > 0)
                        {
                            paraleft = rowcdt.Substring(0, index).Replace("\"", "");
                            pararight = rowcdt.Substring(index + 2).Replace("\"", "");
                            paraleft = adv.GetSpecialBase(context, paraleft, null);
                            pararight = adv.GetSpecialBase(context, pararight, null);
                            return !paraleft.Contains(pararight);
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }
        public static void Dyvalue(HttpContext context)
        {
            string siteid = context.Request.Form["siteid"];
            string definestr = context.Request.Form["definestr"];
            string defineeval = context.Request.Form["defineeval"];
            string definepara = context.Request.Form["definepara"];
            string fidcol = Str.Decode(context.Request.Form["fidcol"]);
            //if (string.IsNullOrWhiteSpace(fidcol)) fidcol = "fid";
            int opdefaultcol = int.Parse(context.Request.Form["opdefaultcol"]);
            ArrayList al = new ArrayList();
            string[] defineevalrows = defineeval.Split(new string[] { "|||" }, StringSplitOptions.None);
            string[] definepararows = definepara.Split(new string[] { "|||" }, StringSplitOptions.None);
            string[] rows = definestr.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            string[] definepararowsrowsitem = definepara.Split(new string[] { "##" }, StringSplitOptions.None);
            for (int i = 0; i < rows.Length; i++)
            {
                string[] defineevalrowsitem = new string[0];
                if (defineevalrows.Length > i)
                {
                    defineevalrowsitem = defineevalrows[i].Split(new string[] { "##" }, StringSplitOptions.None);
                }
                string[] item = rows[i].Split(new string[] { "##" }, StringSplitOptions.None);
                string id = item[0].Trim();
                string tabletag = Str.Decode(item[1].Trim());
                string fid = item[2].Trim();
                int isdy = int.Parse(item[3]);
                int isdim = int.Parse(item[4]);
                string sql = Str.Decode(item[5].Trim());

                int IndexI = 0;
                Regex r = new Regex(@"{[^}]*}");
                MatchCollection mc = r.Matches(sql);
                foreach (Match m in mc)
                {
                    if (defineevalrowsitem.Length > IndexI)
                    {
                        sql = sql.Replace(m.Value, str.D2DD(defineevalrowsitem[IndexI]));
                    }
                    IndexI++;
                }
                for (IndexI = 1; IndexI <= 12; IndexI++)
                {
                    sql = sql.Replace("@p" + IndexI + "@", str.D2DD(definepararowsrowsitem[IndexI - 1]));
                    id = id.Replace("@p" + IndexI + "@", str.D2DD(definepararowsrowsitem[IndexI - 1]));
                }
                if (id.IndexOf(':') < 0 || id.Substring(0, id.IndexOf(':')) == "1")
                {
                    if (id.IndexOf(':') > 0) id = id.Substring(id.IndexOf(':') + 1);
                    al.Add(new string[]{
                    id,tabletag,fid,isdy.ToString(),isdim.ToString(),sql
                });
                }
            }
            string s = "";

            string RightCheck = Interface.Right.DyValue(context, al, siteid);
            if (RightCheck != null) s = s = "alert(\"" + RightCheck + "\")";
            else
            {
                DB db = new DB(SysConst.ConnString_ReadOnly);
                db.Open();
                try
                {
                    s = Fore.GetDyValue(db, al, siteid, false, fidcol, opdefaultcol);
                }
                catch (Exception ex)
                {
                    s = "alert(\"" + ex.Message + "\")";
                    log.Error(ex);
                }
                finally
                {
                    db.Close();
                }
            }
            al.Clear();
            al = null;
            HttpResponse res = context.Response;
            res.Clear();
            //res.Charset = Encoding.Default.ToString();
            //res.ContentEncoding = System.Text.Encoding.Default;
            res.ContentType = "text/html;charset=utf-8";
            res.WriteAsync(s);
        }
        public static string DataOPAdvVal(HttpContext context, DB db, ST st, string adv, string newfid, int rowindex, Hashtable AddHT, Hashtable ModHT)
        {
            //@sql:    @code()      _newfid_   @newfid@  其他val
            try
            {
                adv = adv.Replace("_newfid_", newfid).Replace("@newfid@", newfid);
                adv = adv.Replace("_rowindex_", rowindex.ToString()).Replace("@rowindex@", rowindex.ToString());
                if (adv.StartsWith("@sql:"))
                {
                    string sql = adv.Substring(5);
                    using (DR dr = db.OpenRecord(sql, st))
                    {
                        if (dr.Read())
                        {
                            return dr.GetStringForceNoNULL(0);
                        }
                        else return "";
                    }
                }
                else if (adv.StartsWith("@code("))
                {
                    return Interface.Code.GetObject(adv, context).ToString();
                }
                else if (adv.StartsWith("@from("))
                {
                    //@from(@ef_meeting_content_info.should_arrive_number)
                    string dataop_tabletag = adv.Trim().Replace("@from(", "").Replace(")", "");
                    string dataop_table = dataop_tabletag.Split('.')[0];
                    dataop_table = dataop_table.StartsWith("@") ? dataop_table.Substring(1) : ("ft_site_f_" + dataop_table);
                    string dataop_col = dataop_tabletag.Split('.')[1];
                    if (AddHT != null && AddHT.ContainsKey(dataop_table) && ((Hashtable)AddHT[dataop_table]).ContainsKey(dataop_col))
                    {
                        return ((Hashtable)AddHT[dataop_table])[dataop_col].ToString();
                    }
                    else if (ModHT != null && ModHT.ContainsKey(dataop_table) && ((Hashtable)ModHT[dataop_table]).ContainsKey(dataop_col))
                    {
                        return ((Hashtable)ModHT[dataop_table])[dataop_col].ToString();
                    }
                    else return "";
                }
                else return adv;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
        public class Chart
        {
            public static void Json(HttpContext context)
            {
                HttpRequest req = context.Request;
                string SiteID = req.Form["SiteID"];
                string PartID = req.Form["PartID"];
                string Chart0SQL = req.Form["Chart0SQL"];
                int Chart0_Limit = int.Parse(req.Form["Chart0_Limit"]);
                string Chart0_testdata = req.Form["Chart0_testdata"];
                string Chart0_Null = req.Form["Chart0_Null"];
                string Chart0_Tip = req.Form["Chart0_Tip"];
                int Chart0Type = int.Parse(req.Form["Chart0Type"]);
                string Chart1SQL = req.Form["Chart1SQL"];
                int Chart1_Limit = int.Parse(req.Form["Chart1_Limit"]);
                string Chart1_testdata = req.Form["Chart1_testdata"];
                string Chart2SQL = req.Form["Chart2SQL"];
                int Chart2_Limit = int.Parse(req.Form["Chart2_Limit"]);
                string Chart2_testdata = req.Form["Chart2_testdata"];
                string Chart3SQL = req.Form["Chart3SQL"];
                int Chart3_Limit = int.Parse(req.Form["Chart3_Limit"]);
                string Chart3_testdata = req.Form["Chart3_testdata"];
                string Chart4SQL = req.Form["Chart4SQL"];
                int Chart4_Limit = int.Parse(req.Form["Chart4_Limit"]);
                string Chart4_testdata = req.Form["Chart4_testdata"];
                string Chart5SQL = req.Form["Chart5SQL"];
                int Chart5_Limit = int.Parse(req.Form["Chart5_Limit"]);
                string Chart5_testdata = req.Form["Chart5_testdata"];
                string TitleName = req.Form["TitleName"];
                string TitleStyle = req.Form["TitleStyle"];
                string BGColor = req.Form["BGColor"];
                int Pie_Legend = int.Parse(req.Form["Pie_Legend"]);
                string Pie_Legend_bgcolor = req.Form["Pie_Legend_bgcolor"];
                string Pie_Legend_position = req.Form["Pie_Legend_position"];
                int Pie_Legend_border = int.Parse(req.Form["Pie_Legend_border"]);
                int Pie_Legend_shadow = int.Parse(req.Form["Pie_Legend_shadow"]);
                int Pie_radius = int.Parse(req.Form["Pie_radius"]);
                int Pie_nolabels = int.Parse(req.Form["Pie_nolabels"]);
                int Pie_animate = int.Parse(req.Form["Pie_animate"]);
                string Pie_highlight = req.Form["Pie_highlight"];
                int Pie_gradient = int.Parse(req.Form["Pie_gradient"]);
                string Pie_Alpha = req.Form["Pie_Alpha"];
                string Pie_Colors = req.Form["Pie_Colors"];
                string X_LegendName = req.Form["X_LegendName"];
                string X_LegendStyle = req.Form["X_LegendStyle"];
                string Y_LegendName = req.Form["Y_LegendName"];
                string Y_LegendStyle = req.Form["Y_LegendStyle"];
                string X_Axis_color = req.Form["X_Axis_color"];
                string X_Axis_grid_color = req.Form["X_Axis_grid_color"];
                string X_Axis_label_color = req.Form["X_Axis_label_color"];
                string X_Axis_labels = req.Form["X_Axis_labels"];
                string X_Axis_labels_sql = req.Form["X_Axis_labels_sql"];
                string X_Axis_steps = req.Form["X_Axis_steps"];
                string X_Axis_max = req.Form["X_Axis_max"];
                string X_Axis_min = req.Form["X_Axis_min"];
                string Y_Axis_grid_color = req.Form["Y_Axis_grid_color"];
                string Y_Axis_steps = req.Form["Y_Axis_steps"];
                string Y_Axis_min = req.Form["Y_Axis_min"];
                string Y_Axis_max = req.Form["Y_Axis_max"];
                string Y_Axis_labels = req.Form["Y_Axis_labels"];
                string Y_Axis_labels_sql = req.Form["Y_Axis_labels_sql"];
                string Y_Axis_color = req.Form["Y_Axis_color"];
                string Y_Right_Axis_grid_color = req.Form["Y_Right_Axis_grid_color"];
                string Y_Right_Axis_steps = req.Form["Y_Right_Axis_steps"];
                string Y_Right_Axis_min = req.Form["Y_Right_Axis_min"];
                string Y_Right_Axis_max = req.Form["Y_Right_Axis_max"];
                string Y_Right_Axis_color = req.Form["Y_Right_Axis_color"];
                string Chart0_Color = req.Form["Chart0_Color"];
                string Chart0_Color2 = req.Form["Chart0_Color2"];
                string Chart0_Alpha = req.Form["Chart0_Alpha"];
                string Chart0_fillalpha = req.Form["Chart0_fillalpha"];
                string Chart0_fill = req.Form["Chart0_fill"];
                string Chart1_Tip = req.Form["Chart1_Tip"];
                string Chart1_Color = req.Form["Chart1_Color"];
                string Chart1_Color2 = req.Form["Chart1_Color2"];
                string Chart1_Alpha = req.Form["Chart1_Alpha"];
                string Chart1_fillalpha = req.Form["Chart1_fillalpha"];
                string Chart1_fill = req.Form["Chart1_fill"];

                int X_Axis_offset = int.Parse(req.Form["X_Axis_offset"]);
                int X_Axis_3d = int.Parse(req.Form["X_Axis_3d"]);
                int X_Axis_tick = int.Parse(req.Form["X_Axis_tick"]);
                int X_Axis_stroke = int.Parse(req.Form["X_Axis_stroke"]);
                int Y_Axis_open = int.Parse(req.Form["Y_Axis_open"]);
                int Y_Axis_3d = int.Parse(req.Form["Y_Axis_3d"]);
                int Y_Axis_offset = int.Parse(req.Form["Y_Axis_offset"]);
                int Y_Axis_tick = int.Parse(req.Form["Y_Axis_tick"]);
                int Y_Axis_stroke = int.Parse(req.Form["Y_Axis_stroke"]);
                int Y_Right_Axis_open = int.Parse(req.Form["Y_Right_Axis_open"]);
                int Y_Right_Axis_3d = int.Parse(req.Form["Y_Right_Axis_3d"]);
                int Y_Right_Axis_offset = int.Parse(req.Form["Y_Right_Axis_offset"]);
                int Y_Right_Axis_tick = int.Parse(req.Form["Y_Right_Axis_tick"]);
                int Y_Right_Axis_stroke = int.Parse(req.Form["Y_Right_Axis_stroke"]);
                int Chart0_Position = int.Parse(req.Form["Chart0_Position"]);
                int Chart1Open = int.Parse(req.Form["Chart1Open"]);
                int Chart1Type = int.Parse(req.Form["Chart1Type"]);
                int Chart1_Position = int.Parse(req.Form["Chart1_Position"]);
                string Chart2_Tip = req.Form["Chart2_Tip"];
                string Chart2_Color = req.Form["Chart2_Color"];
                string Chart2_Color2 = req.Form["Chart2_Color2"];
                string Chart2_Alpha = req.Form["Chart2_Alpha"];
                string Chart2_fillalpha = req.Form["Chart2_fillalpha"];
                string Chart2_fill = req.Form["Chart2_fill"];
                int Chart2Open = int.Parse(req.Form["Chart2Open"]);
                int Chart2Type = int.Parse(req.Form["Chart2Type"]);
                int Chart2_Position = int.Parse(req.Form["Chart2_Position"]);
                string Chart3_Tip = req.Form["Chart3_Tip"];
                string Chart3_Color = req.Form["Chart3_Color"];
                string Chart3_Color2 = req.Form["Chart3_Color2"];
                string Chart3_Alpha = req.Form["Chart3_Alpha"];
                string Chart3_fillalpha = req.Form["Chart3_fillalpha"];
                string Chart3_fill = req.Form["Chart3_fill"];
                int Chart3Open = int.Parse(req.Form["Chart3Open"]);
                int Chart3Type = int.Parse(req.Form["Chart3Type"]);
                int Chart3_Position = int.Parse(req.Form["Chart3_Position"]);
                string Chart4_Tip = req.Form["Chart4_Tip"];
                string Chart4_Color = req.Form["Chart4_Color"];
                string Chart4_Color2 = req.Form["Chart4_Color2"];
                string Chart4_Alpha = req.Form["Chart4_Alpha"];
                string Chart4_fillalpha = req.Form["Chart4_fillalpha"];
                string Chart4_fill = req.Form["Chart4_fill"];
                int Chart4Open = int.Parse(req.Form["Chart4Open"]);
                int Chart4Type = int.Parse(req.Form["Chart4Type"]);
                int Chart4_Position = int.Parse(req.Form["Chart4_Position"]);
                string Chart5_Tip = req.Form["Chart5_Tip"];
                string Chart5_Color = req.Form["Chart5_Color"];
                string Chart5_Color2 = req.Form["Chart5_Color2"];
                string Chart5_Alpha = req.Form["Chart5_Alpha"];
                string Chart5_fillalpha = req.Form["Chart5_fillalpha"];
                string Chart5_fill = req.Form["Chart5_fill"];
                int Chart5Open = int.Parse(req.Form["Chart5Open"]);
                int Chart5Type = int.Parse(req.Form["Chart5Type"]);
                int Chart5_Position = int.Parse(req.Form["Chart5_Position"]);

                string Rador_color = req.Form["Rador_color"];
                string Rador_gridcolor = req.Form["Rador_gridcolor"];
                string Rador_max = req.Form["Rador_max"];
                string Rador_labels = req.Form["Rador_labels"];
                string Rador_linecolor = req.Form["Rador_linecolor"];
                string Rador_fillalpha = req.Form["Rador_fillalpha"];
                string Rador_fill = req.Form["Rador_fill"];
                int Rador_stroke = int.Parse(req.Form["Rador_stroke"]);
                int Rador_loop = int.Parse(req.Form["Rador_loop"]);
                string HBar_Colors = req.Form["HBar_Colors"];

                string str = "";

                ArrayList al = new ArrayList();
                decimal maxnum = 0;
                decimal minnum = 0;
                ArrayList al1 = new ArrayList();
                decimal maxnum1 = 0;
                decimal minnum1 = 0;
                ArrayList al2 = new ArrayList();
                decimal maxnum2 = 0;
                decimal minnum2 = 0;
                ArrayList al3 = new ArrayList();
                decimal maxnum3 = 0;
                decimal minnum3 = 0;
                ArrayList al4 = new ArrayList();
                decimal maxnum4 = 0;
                decimal minnum4 = 0;
                ArrayList al5 = new ArrayList();
                decimal maxnum5 = 0;
                decimal minnum5 = 0;

                DB db = new DB();
                db.Open();
                try
                {
                    object[] objs = InitData(context, db, Chart0SQL, SiteID, Chart0_Limit, Chart0_testdata);
                    if (objs == null)
                    {
                        str = InfoString("no chart sql defined!");
                        goto StrEnd;
                    }
                    al = (ArrayList)objs[0];
                    maxnum = decimal.Parse(objs[1].ToString());
                    minnum = decimal.Parse(objs[2].ToString());

                    if (al.Count <= 0)
                    {
                        str = InfoString(Chart0_Null);
                    }
                    else
                    {
                        objs = InitData(context, db, Chart1SQL, SiteID, Chart1_Limit, Chart1_testdata);
                        if (objs != null)
                        {
                            al1 = (ArrayList)objs[0];
                            maxnum1 = decimal.Parse(objs[1].ToString());
                            minnum1 = decimal.Parse(objs[2].ToString());
                        }
                        objs = InitData(context, db, Chart2SQL, SiteID, Chart2_Limit, Chart2_testdata);
                        if (objs != null)
                        {
                            al2 = (ArrayList)objs[0];
                            maxnum2 = decimal.Parse(objs[1].ToString());
                            minnum2 = decimal.Parse(objs[2].ToString());
                        }
                        objs = InitData(context, db, Chart3SQL, SiteID, Chart3_Limit, Chart3_testdata);
                        if (objs != null)
                        {
                            al3 = (ArrayList)objs[0];
                            maxnum3 = decimal.Parse(objs[1].ToString());
                            minnum3 = decimal.Parse(objs[2].ToString());
                        }
                        objs = InitData(context, db, Chart4SQL, SiteID, Chart4_Limit, Chart4_testdata);
                        if (objs != null)
                        {
                            al4 = (ArrayList)objs[0];
                            maxnum4 = decimal.Parse(objs[1].ToString());
                            minnum4 = decimal.Parse(objs[2].ToString());
                        }
                        objs = InitData(context, db, Chart5SQL, SiteID, Chart5_Limit, Chart5_testdata);
                        if (objs != null)
                        {
                            al5 = (ArrayList)objs[0];
                            maxnum5 = decimal.Parse(objs[1].ToString());
                            minnum5 = decimal.Parse(objs[2].ToString());
                        }

                        if (Chart0Type == 0 || Chart0Type == 1)//饼状图类型
                        {
                            str += "{";
                            str += TitleBgColor(context, TitleName, TitleStyle, BGColor);
                            if (Pie_Legend == 0)
                            {
                                str += "\"legend\":{\"visible\":" + (Pie_Legend == 0 ? "true" : "false") + ", \"bg_colour\":\"" + Pie_Legend_bgcolor + "\", \"position\":\"" + Pie_Legend_position + "\", \"border\":" + (Pie_Legend_border == 0 ? "true" : "false") + ", \"shadow\":" + (Pie_Legend_shadow == 0 ? "true" : "false") + "},";
                            }
                            str += "\"elements\":[{\"type\":\"pie\", \"tip\":\"" + Chart0_Tip + "\",";
                            str += "\"values\":[";
                            for (int i = 0; i < al.Count; i++)
                            {
                                string[] strs = (string[])al[i];
                                str += "{\"value\":" + strs[0] + ", \"label\":\"" + strs[1] + "\"";
                                if (strs.Length > 2)
                                {
                                    str += ",\"text\":\"" + strs[2] + "\"";
                                }
                                if (strs.Length > 3)
                                {
                                    str += ",\"on-click\":\"" + strs[3] + "\"";
                                }
                                str += "}";
                                if (i < al.Count - 1)
                                {
                                    str += ",";
                                }
                            }
                            str += "],";
                            if (Pie_radius > 0)
                            {
                                str += "\"radius\":" + Pie_radius + ",";
                            }
                            str += "\"no-labels\":" + (Pie_nolabels == 0 ? "true" : "false") + ",";
                            str += "\"animate\":" + (Pie_animate == 0 ? "true" : "false") + ",";
                            if (Chart0Type == 0)//不设置highlight，就变为类型2
                            {
                                str += "\"highlight\":\"" + Pie_highlight + "\",";
                            }
                            str += "\"gradient-fill\":" + (Pie_gradient == 0 ? "true" : "false") + ",";
                            str += "\"alpha\":" + Pie_Alpha + ", ";
                            str += "\"colours\":[\"" + Pie_Colors.Replace(",", "\",\"") + "\"]}]";
                            str += "}";
                        }
                        else if (Chart0Type == 2)//柱状图基本型
                        {
                            str += "{";
                            str += TitleBgColor(context, TitleName, TitleStyle, BGColor);

                            str += XYLegend(context, X_LegendName, X_LegendStyle, Y_LegendName, Y_LegendStyle);

                            str += XAxisNormal(X_Axis_color, X_Axis_grid_color, X_Axis_offset, X_Axis_label_color, X_Axis_labels, X_Axis_labels_sql, context, SiteID, db, X_Axis_3d, X_Axis_tick, X_Axis_steps, X_Axis_max, X_Axis_min, maxnum, minnum, X_Axis_stroke);

                            str += YAxisNormal(context, Y_Axis_open, Y_Axis_grid_color, Y_Axis_steps, Y_Axis_min, Y_Axis_max, Y_Axis_labels, Y_Axis_labels_sql, SiteID, db, Y_Axis_3d, Y_Axis_offset, Y_Axis_tick, Y_Axis_stroke, Y_Axis_color, Y_Right_Axis_open, Y_Right_Axis_grid_color, Y_Right_Axis_steps, Y_Right_Axis_min, Y_Right_Axis_max, Y_Right_Axis_3d, Y_Right_Axis_offset, Y_Right_Axis_tick, Y_Right_Axis_stroke, Y_Right_Axis_color, maxnum, minnum, maxnum1, minnum1, maxnum2, minnum2, maxnum3, minnum3, maxnum4, minnum4, maxnum5, minnum5);

                            str += "\"elements\":[";

                            str += AppendChart(0, al, Chart0_Position, Chart0_Tip, Chart0_Color, Chart0_Color2, Chart0_Alpha, Chart0_fillalpha, Chart0_fill);

                            if (Chart1Open == 0)
                            {
                                str += "," + AppendChart(Chart1Type, al1, Chart1_Position, Chart1_Tip, Chart1_Color, Chart1_Color2, Chart1_Alpha, Chart1_fillalpha, Chart1_fill);
                            }
                            if (Chart2Open == 0)
                            {
                                str += "," + AppendChart(Chart2Type, al2, Chart2_Position, Chart2_Tip, Chart2_Color, Chart2_Color2, Chart2_Alpha, Chart2_fillalpha, Chart2_fill);
                            }
                            if (Chart3Open == 0)
                            {
                                str += "," + AppendChart(Chart3Type, al3, Chart3_Position, Chart3_Tip, Chart3_Color, Chart3_Color2, Chart3_Alpha, Chart3_fillalpha, Chart3_fill);
                            }
                            if (Chart4Open == 0)
                            {
                                str += "," + AppendChart(Chart4Type, al4, Chart4_Position, Chart4_Tip, Chart4_Color, Chart4_Color2, Chart4_Alpha, Chart4_fillalpha, Chart4_fill);
                            }
                            if (Chart5Open == 0)
                            {
                                str += "," + AppendChart(Chart5Type, al5, Chart5_Position, Chart5_Tip, Chart5_Color, Chart5_Color2, Chart5_Alpha, Chart5_fillalpha, Chart5_fill);
                            }

                            str += "]";

                            str += "}";
                        }
                        else if (Chart0Type == 4)//柱状图立体型
                        {
                            str += "{";
                            str += TitleBgColor(context, TitleName, TitleStyle, BGColor);

                            str += XYLegend(context, X_LegendName, X_LegendStyle, Y_LegendName, Y_LegendStyle);

                            str += XAxisNormal(X_Axis_color, X_Axis_grid_color, X_Axis_offset, X_Axis_label_color, X_Axis_labels, X_Axis_labels_sql, context, SiteID, db, X_Axis_3d, X_Axis_tick, X_Axis_steps, X_Axis_max, X_Axis_min, maxnum, minnum, X_Axis_stroke);

                            str += YAxisNormal(context, Y_Axis_open, Y_Axis_grid_color, Y_Axis_steps, Y_Axis_min, Y_Axis_max, Y_Axis_labels, Y_Axis_labels_sql, SiteID, db, Y_Axis_3d, Y_Axis_offset, Y_Axis_tick, Y_Axis_stroke, Y_Axis_color, Y_Right_Axis_open, Y_Right_Axis_grid_color, Y_Right_Axis_steps, Y_Right_Axis_min, Y_Right_Axis_max, Y_Right_Axis_3d, Y_Right_Axis_offset, Y_Right_Axis_tick, Y_Right_Axis_stroke, Y_Right_Axis_color, maxnum, minnum, maxnum1, minnum1, maxnum2, minnum2, maxnum3, minnum3, maxnum4, minnum4, maxnum5, minnum5);
                            str += "\"elements\":[";
                            str += AppendChart(2, al, Chart0_Position, Chart0_Tip, Chart0_Color, Chart0_Color2, Chart0_Alpha, Chart0_fillalpha, Chart0_fill);
                            if (Chart1Open == 0)
                            {
                                str += "," + AppendChart(Chart1Type, al1, Chart1_Position, Chart1_Tip, Chart1_Color, Chart1_Color2, Chart1_Alpha, Chart1_fillalpha, Chart1_fill);
                            }
                            if (Chart2Open == 0)
                            {
                                str += "," + AppendChart(Chart2Type, al2, Chart2_Position, Chart2_Tip, Chart2_Color, Chart2_Color2, Chart2_Alpha, Chart2_fillalpha, Chart2_fill);
                            }
                            if (Chart3Open == 0)
                            {
                                str += "," + AppendChart(Chart3Type, al3, Chart3_Position, Chart3_Tip, Chart3_Color, Chart3_Color2, Chart3_Alpha, Chart3_fillalpha, Chart3_fill);
                            }
                            if (Chart4Open == 0)
                            {
                                str += "," + AppendChart(Chart4Type, al4, Chart4_Position, Chart4_Tip, Chart4_Color, Chart4_Color2, Chart4_Alpha, Chart4_fillalpha, Chart4_fill);
                            }
                            if (Chart5Open == 0)
                            {
                                str += "," + AppendChart(Chart5Type, al5, Chart5_Position, Chart5_Tip, Chart5_Color, Chart5_Color2, Chart5_Alpha, Chart5_fillalpha, Chart5_fill);
                            }
                            str += "]";

                            str += "}";
                        }
                        else if (Chart0Type == 5)//折线图基本型
                        {
                            str += "{";
                            str += TitleBgColor(context, TitleName, TitleStyle, BGColor);

                            str += XYLegend(context, X_LegendName, X_LegendStyle, Y_LegendName, Y_LegendStyle);

                            str += XAxisNormal(X_Axis_color, X_Axis_grid_color, X_Axis_offset, X_Axis_label_color, X_Axis_labels, X_Axis_labels_sql, context, SiteID, db, X_Axis_3d, X_Axis_tick, X_Axis_steps, X_Axis_max, X_Axis_min, maxnum, minnum, X_Axis_stroke);

                            str += YAxisNormal(context, Y_Axis_open, Y_Axis_grid_color, Y_Axis_steps, Y_Axis_min, Y_Axis_max, Y_Axis_labels, Y_Axis_labels_sql, SiteID, db, Y_Axis_3d, Y_Axis_offset, Y_Axis_tick, Y_Axis_stroke, Y_Axis_color, Y_Right_Axis_open, Y_Right_Axis_grid_color, Y_Right_Axis_steps, Y_Right_Axis_min, Y_Right_Axis_max, Y_Right_Axis_3d, Y_Right_Axis_offset, Y_Right_Axis_tick, Y_Right_Axis_stroke, Y_Right_Axis_color, maxnum, minnum, maxnum1, minnum1, maxnum2, minnum2, maxnum3, minnum3, maxnum4, minnum4, maxnum5, minnum5);
                            str += "\"elements\":[";
                            str += AppendChart(3, al, Chart0_Position, Chart0_Tip, Chart0_Color, Chart0_Color2, Chart0_Alpha, Chart0_fillalpha, Chart0_fill);
                            if (Chart1Open == 0)
                            {
                                str += "," + AppendChart(Chart1Type, al1, Chart1_Position, Chart1_Tip, Chart1_Color, Chart1_Color2, Chart1_Alpha, Chart1_fillalpha, Chart1_fill);
                            }
                            if (Chart2Open == 0)
                            {
                                str += "," + AppendChart(Chart2Type, al2, Chart2_Position, Chart2_Tip, Chart2_Color, Chart2_Color2, Chart2_Alpha, Chart2_fillalpha, Chart2_fill);
                            }
                            if (Chart3Open == 0)
                            {
                                str += "," + AppendChart(Chart3Type, al3, Chart3_Position, Chart3_Tip, Chart3_Color, Chart3_Color2, Chart3_Alpha, Chart3_fillalpha, Chart3_fill);
                            }
                            if (Chart4Open == 0)
                            {
                                str += "," + AppendChart(Chart4Type, al4, Chart4_Position, Chart4_Tip, Chart4_Color, Chart4_Color2, Chart4_Alpha, Chart4_fillalpha, Chart4_fill);
                            }
                            if (Chart5Open == 0)
                            {
                                str += "," + AppendChart(Chart5Type, al5, Chart5_Position, Chart5_Tip, Chart5_Color, Chart5_Color2, Chart5_Alpha, Chart5_fillalpha, Chart5_fill);
                            }
                            str += "]";

                            str += "}";
                        }
                        else if (Chart0Type == 6)//柱状图圆柱型
                        {
                            str += "{";
                            str += TitleBgColor(context, TitleName, TitleStyle, BGColor);

                            str += XYLegend(context, X_LegendName, X_LegendStyle, Y_LegendName, Y_LegendStyle);

                            str += XAxisNormal(X_Axis_color, X_Axis_grid_color, X_Axis_offset, X_Axis_label_color, X_Axis_labels, X_Axis_labels_sql, context, SiteID, db, X_Axis_3d, X_Axis_tick, X_Axis_steps, X_Axis_max, X_Axis_min, maxnum, minnum, X_Axis_stroke);

                            str += YAxisNormal(context, Y_Axis_open, Y_Axis_grid_color, Y_Axis_steps, Y_Axis_min, Y_Axis_max, Y_Axis_labels, Y_Axis_labels_sql, SiteID, db, Y_Axis_3d, Y_Axis_offset, Y_Axis_tick, Y_Axis_stroke, Y_Axis_color, Y_Right_Axis_open, Y_Right_Axis_grid_color, Y_Right_Axis_steps, Y_Right_Axis_min, Y_Right_Axis_max, Y_Right_Axis_3d, Y_Right_Axis_offset, Y_Right_Axis_tick, Y_Right_Axis_stroke, Y_Right_Axis_color, maxnum, minnum, maxnum1, minnum1, maxnum2, minnum2, maxnum3, minnum3, maxnum4, minnum4, maxnum5, minnum5);
                            str += "\"elements\":[";
                            str += AppendChart(4, al, Chart0_Position, Chart0_Tip, Chart0_Color, Chart0_Color2, Chart0_Alpha, Chart0_fillalpha, Chart0_fill);
                            if (Chart1Open == 0)
                            {
                                str += "," + AppendChart(Chart1Type, al1, Chart1_Position, Chart1_Tip, Chart1_Color, Chart1_Color2, Chart1_Alpha, Chart1_fillalpha, Chart1_fill);
                            }
                            if (Chart2Open == 0)
                            {
                                str += "," + AppendChart(Chart2Type, al2, Chart2_Position, Chart2_Tip, Chart2_Color, Chart2_Color2, Chart2_Alpha, Chart2_fillalpha, Chart2_fill);
                            }
                            if (Chart3Open == 0)
                            {
                                str += "," + AppendChart(Chart3Type, al3, Chart3_Position, Chart3_Tip, Chart3_Color, Chart3_Color2, Chart3_Alpha, Chart3_fillalpha, Chart3_fill);
                            }
                            if (Chart4Open == 0)
                            {
                                str += "," + AppendChart(Chart4Type, al4, Chart4_Position, Chart4_Tip, Chart4_Color, Chart4_Color2, Chart4_Alpha, Chart4_fillalpha, Chart4_fill);
                            }
                            if (Chart5Open == 0)
                            {
                                str += "," + AppendChart(Chart5Type, al5, Chart5_Position, Chart5_Tip, Chart5_Color, Chart5_Color2, Chart5_Alpha, Chart5_fillalpha, Chart5_fill);
                            }
                            str += "]";

                            str += "}";
                        }
                        else if (Chart0Type == 7)//柱状图玻璃型二
                        {
                            str += "{";
                            str += TitleBgColor(context, TitleName, TitleStyle, BGColor);

                            str += XYLegend(context, X_LegendName, X_LegendStyle, Y_LegendName, Y_LegendStyle);

                            str += XAxisNormal(X_Axis_color, X_Axis_grid_color, X_Axis_offset, X_Axis_label_color, X_Axis_labels, X_Axis_labels_sql, context, SiteID, db, X_Axis_3d, X_Axis_tick, X_Axis_steps, X_Axis_max, X_Axis_min, maxnum, minnum, X_Axis_stroke);

                            str += YAxisNormal(context, Y_Axis_open, Y_Axis_grid_color, Y_Axis_steps, Y_Axis_min, Y_Axis_max, Y_Axis_labels, Y_Axis_labels_sql, SiteID, db, Y_Axis_3d, Y_Axis_offset, Y_Axis_tick, Y_Axis_stroke, Y_Axis_color, Y_Right_Axis_open, Y_Right_Axis_grid_color, Y_Right_Axis_steps, Y_Right_Axis_min, Y_Right_Axis_max, Y_Right_Axis_3d, Y_Right_Axis_offset, Y_Right_Axis_tick, Y_Right_Axis_stroke, Y_Right_Axis_color, maxnum, minnum, maxnum1, minnum1, maxnum2, minnum2, maxnum3, minnum3, maxnum4, minnum4, maxnum5, minnum5);
                            str += "\"elements\":[";
                            str += AppendChart(5, al, Chart0_Position, Chart0_Tip, Chart0_Color, Chart0_Color2, Chart0_Alpha, Chart0_fillalpha, Chart0_fill);
                            if (Chart1Open == 0)
                            {
                                str += "," + AppendChart(Chart1Type, al1, Chart1_Position, Chart1_Tip, Chart1_Color, Chart1_Color2, Chart1_Alpha, Chart1_fillalpha, Chart1_fill);
                            }
                            if (Chart2Open == 0)
                            {
                                str += "," + AppendChart(Chart2Type, al2, Chart2_Position, Chart2_Tip, Chart2_Color, Chart2_Color2, Chart2_Alpha, Chart2_fillalpha, Chart2_fill);
                            }
                            if (Chart3Open == 0)
                            {
                                str += "," + AppendChart(Chart3Type, al3, Chart3_Position, Chart3_Tip, Chart3_Color, Chart3_Color2, Chart3_Alpha, Chart3_fillalpha, Chart3_fill);
                            }
                            if (Chart4Open == 0)
                            {
                                str += "," + AppendChart(Chart4Type, al4, Chart4_Position, Chart4_Tip, Chart4_Color, Chart4_Color2, Chart4_Alpha, Chart4_fillalpha, Chart4_fill);
                            }
                            if (Chart5Open == 0)
                            {
                                str += "," + AppendChart(Chart5Type, al5, Chart5_Position, Chart5_Tip, Chart5_Color, Chart5_Color2, Chart5_Alpha, Chart5_fillalpha, Chart5_fill);
                            }
                            str += "]";

                            str += "}";
                        }
                        else if (Chart0Type == 8)//雷达图
                        {
                            str += "{";
                            str += TitleBgColor(context, TitleName, TitleStyle, BGColor);

                            str += "\"radar_axis\":{";
                            str += "\"stroke\":" + Rador_stroke + ",";
                            str += "\"colour\":\"" + Rador_color + "\",";
                            str += "\"grid-colour\":\"" + Rador_gridcolor + "\",";
                            str += "\"max\":" + Rador_max.Replace("@max@", maxnum.ToString()) + ",";
                            str += "\"spoke-labels\":{\"labels\":[\"" + Rador_labels.Replace(",", "\",\"") + "\"]}},";

                            str += "\"elements\":[{\"type\":\"area_line\",";
                            str += "\"values\":[" + EleValue(al) + "],";
                            str += "\"colour\":\"" + Rador_linecolor + "\",";
                            str += "\"fill-alpha\":" + Rador_fillalpha + ",";
                            str += "\"fill\":\"" + Rador_fill + "\",";
                            str += "\"loop\":" + (Rador_loop == 0 ? "true" : "false") + "}]";
                            str += "}";
                        }
                        else if (Chart0Type == 9)//柱状图水平型
                        {
                            str += "{";
                            str += TitleBgColor(context, TitleName, TitleStyle, BGColor);

                            str += XYLegend(context, X_LegendName, X_LegendStyle, Y_LegendName, Y_LegendStyle);

                            str += XAxisNormal(X_Axis_color, X_Axis_grid_color, X_Axis_offset, X_Axis_label_color, X_Axis_labels, X_Axis_labels_sql, context, SiteID, db, X_Axis_3d, X_Axis_tick, X_Axis_steps, X_Axis_max, X_Axis_min, maxnum, minnum, X_Axis_stroke);

                            str += YAxisNormal(context, Y_Axis_open, Y_Axis_grid_color, Y_Axis_steps, Y_Axis_min, Y_Axis_max, Y_Axis_labels, Y_Axis_labels_sql, SiteID, db, Y_Axis_3d, Y_Axis_offset, Y_Axis_tick, Y_Axis_stroke, Y_Axis_color, Y_Right_Axis_open, Y_Right_Axis_grid_color, Y_Right_Axis_steps, Y_Right_Axis_min, Y_Right_Axis_max, Y_Right_Axis_3d, Y_Right_Axis_offset, Y_Right_Axis_tick, Y_Right_Axis_stroke, Y_Right_Axis_color, maxnum, minnum, maxnum1, minnum1, maxnum2, minnum2, maxnum3, minnum3, maxnum4, minnum4, maxnum5, minnum5);
                            str += "\"elements\":[{\"type\":\"hbar\", \"tip\":\"" + Chart0_Tip + "\",";
                            str += "\"values\":[";
                            str += EleValue4HBar(al, HBar_Colors);
                            str += "], \"colour\":\"" + Chart0_Color + "\", \"alpha\":" + Chart0_Alpha + "}]";

                            str += "}";
                        }
                        else if (Chart0Type == 10)//折线图区域型
                        {
                            str += "{";
                            str += TitleBgColor(context, TitleName, TitleStyle, BGColor);

                            str += XYLegend(context, X_LegendName, X_LegendStyle, Y_LegendName, Y_LegendStyle);

                            str += XAxisNormal(X_Axis_color, X_Axis_grid_color, X_Axis_offset, X_Axis_label_color, X_Axis_labels, X_Axis_labels_sql, context, SiteID, db, X_Axis_3d, X_Axis_tick, X_Axis_steps, X_Axis_max, X_Axis_min, maxnum, minnum, X_Axis_stroke);

                            str += YAxisNormal(context, Y_Axis_open, Y_Axis_grid_color, Y_Axis_steps, Y_Axis_min, Y_Axis_max, Y_Axis_labels, Y_Axis_labels_sql, SiteID, db, Y_Axis_3d, Y_Axis_offset, Y_Axis_tick, Y_Axis_stroke, Y_Axis_color, Y_Right_Axis_open, Y_Right_Axis_grid_color, Y_Right_Axis_steps, Y_Right_Axis_min, Y_Right_Axis_max, Y_Right_Axis_3d, Y_Right_Axis_offset, Y_Right_Axis_tick, Y_Right_Axis_stroke, Y_Right_Axis_color, maxnum, minnum, maxnum1, minnum1, maxnum2, minnum2, maxnum3, minnum3, maxnum4, minnum4, maxnum5, minnum5);
                            str += "\"elements\":[";
                            str += AppendChart(6, al, Chart0_Position, Chart0_Tip, Chart0_Color, Chart0_Color2, Chart0_Alpha, Chart0_fillalpha, Chart0_fill);
                            if (Chart1Open == 0)
                            {
                                str += "," + AppendChart(Chart1Type, al1, Chart1_Position, Chart1_Tip, Chart1_Color, Chart1_Color2, Chart1_Alpha, Chart1_fillalpha, Chart1_fill);
                            }
                            if (Chart2Open == 0)
                            {
                                str += "," + AppendChart(Chart2Type, al2, Chart2_Position, Chart2_Tip, Chart2_Color, Chart2_Color2, Chart2_Alpha, Chart2_fillalpha, Chart2_fill);
                            }
                            if (Chart3Open == 0)
                            {
                                str += "," + AppendChart(Chart3Type, al3, Chart3_Position, Chart3_Tip, Chart3_Color, Chart3_Color2, Chart3_Alpha, Chart3_fillalpha, Chart3_fill);
                            }
                            if (Chart4Open == 0)
                            {
                                str += "," + AppendChart(Chart4Type, al4, Chart4_Position, Chart4_Tip, Chart4_Color, Chart4_Color2, Chart4_Alpha, Chart4_fillalpha, Chart4_fill);
                            }
                            if (Chart5Open == 0)
                            {
                                str += "," + AppendChart(Chart5Type, al5, Chart5_Position, Chart5_Tip, Chart5_Color, Chart5_Color2, Chart5_Alpha, Chart5_fillalpha, Chart5_fill);
                            }
                            str += "]";

                            str += "}";
                        }
                        else if (Chart0Type == 11)//堆积图
                        {
                            str = InfoString("请使用堆积图构件");
                        }
                        else if (Chart0Type == 12)//折线图热点型
                        {
                            str += "{";
                            str += TitleBgColor(context, TitleName, TitleStyle, BGColor);

                            str += XYLegend(context, X_LegendName, X_LegendStyle, Y_LegendName, Y_LegendStyle);

                            str += XAxisNormal(X_Axis_color, X_Axis_grid_color, X_Axis_offset, X_Axis_label_color, X_Axis_labels, X_Axis_labels_sql, context, SiteID, db, X_Axis_3d, X_Axis_tick, X_Axis_steps, X_Axis_max, X_Axis_min, maxnum, minnum, X_Axis_stroke);

                            str += YAxisNormal(context, Y_Axis_open, Y_Axis_grid_color, Y_Axis_steps, Y_Axis_min, Y_Axis_max, Y_Axis_labels, Y_Axis_labels_sql, SiteID, db, Y_Axis_3d, Y_Axis_offset, Y_Axis_tick, Y_Axis_stroke, Y_Axis_color, Y_Right_Axis_open, Y_Right_Axis_grid_color, Y_Right_Axis_steps, Y_Right_Axis_min, Y_Right_Axis_max, Y_Right_Axis_3d, Y_Right_Axis_offset, Y_Right_Axis_tick, Y_Right_Axis_stroke, Y_Right_Axis_color, maxnum, minnum, maxnum1, minnum1, maxnum2, minnum2, maxnum3, minnum3, maxnum4, minnum4, maxnum5, minnum5);
                            str += "\"elements\":[";
                            str += AppendChart(7, al, Chart0_Position, Chart0_Tip, Chart0_Color, Chart0_Color2, Chart0_Alpha, Chart0_fillalpha, Chart0_fill);
                            if (Chart1Open == 0)
                            {
                                str += "," + AppendChart(Chart1Type, al1, Chart1_Position, Chart1_Tip, Chart1_Color, Chart1_Color2, Chart1_Alpha, Chart1_fillalpha, Chart1_fill);
                            }
                            if (Chart2Open == 0)
                            {
                                str += "," + AppendChart(Chart2Type, al2, Chart2_Position, Chart2_Tip, Chart2_Color, Chart2_Color2, Chart2_Alpha, Chart2_fillalpha, Chart2_fill);
                            }
                            if (Chart3Open == 0)
                            {
                                str += "," + AppendChart(Chart3Type, al3, Chart3_Position, Chart3_Tip, Chart3_Color, Chart3_Color2, Chart3_Alpha, Chart3_fillalpha, Chart3_fill);
                            }
                            if (Chart4Open == 0)
                            {
                                str += "," + AppendChart(Chart4Type, al4, Chart4_Position, Chart4_Tip, Chart4_Color, Chart4_Color2, Chart4_Alpha, Chart4_fillalpha, Chart4_fill);
                            }
                            if (Chart5Open == 0)
                            {
                                str += "," + AppendChart(Chart5Type, al5, Chart5_Position, Chart5_Tip, Chart5_Color, Chart5_Color2, Chart5_Alpha, Chart5_fillalpha, Chart5_fill);
                            }
                            str += "]";

                            str += "}";
                        }
                        else if (Chart0Type == 13)//折线图空心型
                        {
                            str += "{";
                            str += TitleBgColor(context, TitleName, TitleStyle, BGColor);

                            str += XYLegend(context, X_LegendName, X_LegendStyle, Y_LegendName, Y_LegendStyle);

                            str += XAxisNormal(X_Axis_color, X_Axis_grid_color, X_Axis_offset, X_Axis_label_color, X_Axis_labels, X_Axis_labels_sql, context, SiteID, db, X_Axis_3d, X_Axis_tick, X_Axis_steps, X_Axis_max, X_Axis_min, maxnum, minnum, X_Axis_stroke);

                            str += YAxisNormal(context, Y_Axis_open, Y_Axis_grid_color, Y_Axis_steps, Y_Axis_min, Y_Axis_max, Y_Axis_labels, Y_Axis_labels_sql, SiteID, db, Y_Axis_3d, Y_Axis_offset, Y_Axis_tick, Y_Axis_stroke, Y_Axis_color, Y_Right_Axis_open, Y_Right_Axis_grid_color, Y_Right_Axis_steps, Y_Right_Axis_min, Y_Right_Axis_max, Y_Right_Axis_3d, Y_Right_Axis_offset, Y_Right_Axis_tick, Y_Right_Axis_stroke, Y_Right_Axis_color, maxnum, minnum, maxnum1, minnum1, maxnum2, minnum2, maxnum3, minnum3, maxnum4, minnum4, maxnum5, minnum5);
                            str += "\"elements\":[";
                            str += AppendChart(8, al, Chart0_Position, Chart0_Tip, Chart0_Color, Chart0_Color2, Chart0_Alpha, Chart0_fillalpha, Chart0_fill);
                            if (Chart1Open == 0)
                            {
                                str += "," + AppendChart(Chart1Type, al1, Chart1_Position, Chart1_Tip, Chart1_Color, Chart1_Color2, Chart1_Alpha, Chart1_fillalpha, Chart1_fill);
                            }
                            if (Chart2Open == 0)
                            {
                                str += "," + AppendChart(Chart2Type, al2, Chart2_Position, Chart2_Tip, Chart2_Color, Chart2_Color2, Chart2_Alpha, Chart2_fillalpha, Chart2_fill);
                            }
                            if (Chart3Open == 0)
                            {
                                str += "," + AppendChart(Chart3Type, al3, Chart3_Position, Chart3_Tip, Chart3_Color, Chart3_Color2, Chart3_Alpha, Chart3_fillalpha, Chart3_fill);
                            }
                            if (Chart4Open == 0)
                            {
                                str += "," + AppendChart(Chart4Type, al4, Chart4_Position, Chart4_Tip, Chart4_Color, Chart4_Color2, Chart4_Alpha, Chart4_fillalpha, Chart4_fill);
                            }
                            if (Chart5Open == 0)
                            {
                                str += "," + AppendChart(Chart5Type, al5, Chart5_Position, Chart5_Tip, Chart5_Color, Chart5_Color2, Chart5_Alpha, Chart5_fillalpha, Chart5_fill);
                            }
                            str += "]";

                            str += "}";
                        }
                    }
                    al = null;
                    al1 = null;
                    al2 = null;
                    al3 = null;
                    al4 = null;
                    al5 = null;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    str = InfoString(ex.Message);
                }
                finally
                {
                    db.Close();
                }

            StrEnd:
                //string s= "{\"value\":1, \"label\":\"1\",\"text\":\"5460\"},{\"value\":2, \"label\":\"2\",\"text\":\"6460\"},{\"value\":3, \"label\":\"3\",\"text\":\"8460\"}";

                HttpResponse res = context.Response;
                res.Clear();
                //res.Charset = Encoding..ToString();
                //res.ContentEncoding = System.Text.Encoding.Default;
                res.ContentType = "text/html;charset=utf-8";
                res.WriteAsync(str);
                //res.Write(Microsoft.JScript.GlobalObject.escape(str));
            }
            private static object[] InitData(HttpContext context, DB db, string ChartSQL, string SiteID, int Chart_Limit, string Chart_testdata)
            {
                object[] objs = new object[3];
                if (Chart_testdata != null && !Chart_testdata.Equals(""))
                {
                    ArrayList al = new ArrayList();
                    decimal maxnum = 0;
                    decimal minnum = 0;
                    string[] datas = Chart_testdata.Split(',');
                    foreach (string data in datas)
                    {
                        if (data != null && !data.Equals(""))
                        {
                            al.Add(new string[] { data.Trim() });
                            decimal parseval = 0;
                            if (decimal.TryParse(data.Trim(), out parseval))
                            {
                                if (maxnum < parseval)
                                {
                                    maxnum = parseval;
                                }
                                if (minnum > parseval)
                                {
                                    minnum = parseval;
                                }
                            }
                        }
                    }
                    objs[0] = al;
                    objs[1] = maxnum;
                    objs[2] = minnum;
                }
                else
                {
                    if (ChartSQL == null || ChartSQL.Trim().Equals("")) return null;
                    if (ChartSQL.StartsWith("@code(")) return (object[])Interface.Code.GetObject(adv.GetSpecialBase(context, ChartSQL, SiteID), context);
                    string sql = null;
                    if (ChartSQL.StartsWith("sql@code(")) sql = Interface.Code.Get(adv.GetSpecialBase(context, ChartSQL.Substring(3), SiteID), context);
                    else sql = adv.SQLSelectSafe(adv.GetSpecialBase(context, ChartSQL, SiteID));
                    DR dr = db.OpenRecord(sql);
                    ArrayList al = new ArrayList();
                    decimal maxnum = 0;
                    decimal minnum = 0;
                    int fieldcount = dr.FieldCount;
                    int maxcount = (Chart_Limit == 0 ? 48 : Chart_Limit);
                    int curcount = 0;
                    while (dr.Read() && curcount < maxcount)
                    {
                        string[] alstrs = new string[fieldcount];
                        for (int i = 0; i < fieldcount; i++)
                        {
                            alstrs[i] = dr.GetValue(i).ToString();
                        }
                        al.Add(alstrs);

                        //if (fieldcount == 1)//仅为数字时，取最大最小值
                        //{

                        //}
                        decimal parseval = 0;
                        if (decimal.TryParse(alstrs[0], out parseval))
                        {
                            if (maxnum < parseval)
                            {
                                maxnum = parseval;
                            }
                            if (minnum > parseval)
                            {
                                minnum = parseval;
                            }
                        }

                        curcount++;
                    }
                    dr.Close();
                    objs[0] = al;
                    objs[1] = maxnum;
                    objs[2] = minnum;
                }
                return objs;
            }
            private static string InfoString(string info)
            {
                return "{\"title\":{\"text\":\"" + info + "\", \"style\":\"font-size: 14px; font-family: 微软雅黑,宋体, Verdana; text-align: center;\"}, \"bg_colour\":\"#ffffff\", \"elements\":[{\"type\":\"pie\"}]}";
            }
            private static string TitleBgColor(HttpContext context, string TitleName, string TitleStyle, string BGColor)
            {
                string str = "\"title\":{\"text\":\"" + adv.GetSpecialBase(context, TitleName, "") + "\", \"style\":\"" + TitleStyle + "\"},";
                str += "\"bg_colour\":\"" + BGColor + "\",";
                return str;
            }
            private static string XAxisNormal(string X_Axis_color, string X_Axis_grid_color, int X_Axis_offset, string X_Axis_label_color, string X_Axis_labels, string X_Axis_labels_sql, HttpContext context, string SiteID, DB db, int X_Axis_3d, int X_Axis_tick, string X_Axis_steps, string X_Axis_max, string X_Axis_min, decimal max, decimal min, int X_Axis_stroke)
            {
                string str = "";
                str += "\"x_axis\":{";
                str += "\"grid-colour\":\"" + X_Axis_grid_color + "\",";
                if (X_Axis_offset != -1)
                {
                    str += "\"offset\":" + X_Axis_offset + ",";
                }
                if (X_Axis_3d != -1)
                {
                    str += "\"3d\":" + X_Axis_3d + ",";
                }
                if (X_Axis_tick != -1)
                {
                    str += "\"tick-height\":" + X_Axis_tick + ",";
                }
                if (X_Axis_labels != null && !X_Axis_labels.Equals(""))
                {
                    str += "\"labels\":{";
                    str += "\"colour\":\"" + X_Axis_label_color + "\",";
                    str += "\"labels\":[";
                    str += "\"" + X_Axis_labels.Replace(",", "\",\"") + "\"";
                    str += "]},";
                }
                else if (X_Axis_labels_sql != null && !X_Axis_labels_sql.Equals(""))
                {
                    str += "\"labels\":{";
                    str += "\"colour\":\"" + X_Axis_label_color + "\",";
                    str += "\"labels\":[";
                    string tempstr = "";
                    if (X_Axis_labels_sql.StartsWith("@code(")) tempstr = Interface.Code.Get(adv.GetSpecialBase(context, X_Axis_labels_sql, SiteID), context);
                    else
                    {
                        string sql = null;
                        if (X_Axis_labels_sql.StartsWith("sql@code("))
                        {
                            sql = Interface.Code.Get(adv.GetSpecialBase(context, X_Axis_labels_sql.Substring(3), SiteID), context);
                        }
                        else sql = adv.SQLSelectSafe(adv.GetSpecialBase(context, X_Axis_labels_sql, SiteID));
                        DR dr = db.OpenRecord(sql);
                        int curcount = 0;
                        while (dr.Read() && curcount < 48)
                        {
                            tempstr += "\"" + dr.GetValue(0).ToString() + "\",";
                            curcount++;
                        }
                        dr.Close();
                        if (!tempstr.Equals("")) tempstr = tempstr.Substring(0, tempstr.Length - 1);
                    }
                    str += tempstr;
                    str += "]},";
                }
                if (X_Axis_steps != null && !X_Axis_steps.Equals(""))
                {
                    str += "\"steps\":" + X_Axis_steps.Replace("@max@", max.ToString()).Replace("@min@", min.ToString()) + ",";
                }
                if (X_Axis_max != null && !X_Axis_max.Equals(""))
                {
                    str += "\"max\":" + X_Axis_max.Replace("@max@", max.ToString()).Replace("@min@", min.ToString()) + ",";
                }
                if (X_Axis_min != null && !X_Axis_min.Equals(""))
                {
                    str += "\"min\":" + X_Axis_min.Replace("@max@", max.ToString()).Replace("@min@", min.ToString()) + ",";
                }
                if (X_Axis_tick != -1)
                {
                    str += "\"tick-height\":" + X_Axis_tick + ",";
                }
                if (X_Axis_stroke != -1)
                {
                    str += "\"stroke\":" + X_Axis_stroke + ",";
                }
                str += "\"colour\":\"" + X_Axis_color + "\"";
                str += "},";
                return str;
            }
            private static string XYLegend(HttpContext context, string X_LegendName, string X_LegendStyle, string Y_LegendName, string Y_LegendStyle)
            {
                string str = "";
                if (X_LegendName != null && !X_LegendName.Equals(""))
                {
                    str += "\"x_legend\":{\"text\":\"" + adv.GetSpecialBase(context, X_LegendName, "") + "\", \"style\":\"" + X_LegendStyle + "\"},";
                }

                if (Y_LegendName != null && !Y_LegendName.Equals(""))
                {
                    str += "\"y_legend\":{\"text\":\"" + adv.GetSpecialBase(context, Y_LegendName, "") + "\", \"style\":\"" + Y_LegendStyle + "\"},";
                }
                return str;
            }
            private static string YAxisNormal(HttpContext context, int Y_Axis_open, string Y_Axis_grid_color, string Y_Axis_steps, string Y_Axis_min, string Y_Axis_max, string Y_Axis_labels, string Y_Axis_labels_sql, string SiteID, DB db, int Y_Axis_3d, int Y_Axis_offset, int Y_Axis_tick, int Y_Axis_stroke, string Y_Axis_color, int Y_Right_Axis_open, string Y_Right_Axis_grid_color, string Y_Right_Axis_steps, string Y_Right_Axis_min, string Y_Right_Axis_max, int Y_Right_Axis_3d, int Y_Right_Axis_offset, int Y_Right_Axis_tick, int Y_Right_Axis_stroke, string Y_Right_Axis_color, decimal max, decimal min, decimal max1, decimal min1, decimal max2, decimal min2, decimal max3, decimal min3, decimal max4, decimal min4, decimal max5, decimal min5)
            {
                string str = "";
                if (Y_Axis_open == 1)
                {
                    str += "\"y_axis\":{";
                    str += "\"grid-colour\":\"" + Y_Axis_grid_color + "\",";
                    if (Y_Axis_steps != null && !Y_Axis_steps.Equals(""))
                    {
                        str += "\"steps\":" + ReplaceMaxMin(Y_Axis_steps, max, min, max1, min1, max2, min2, max3, min3, max4, min4, max5, min5) + ",";
                    }
                    if (Y_Axis_min != null && !Y_Axis_min.Equals(""))
                    {
                        str += "\"min\":" + ReplaceMaxMin(Y_Axis_min, max, min, max1, min1, max2, min2, max3, min3, max4, min4, max5, min5) + ",";
                    }
                    if (Y_Axis_max != null && !Y_Axis_max.Equals(""))
                    {
                        str += "\"max\":" + ReplaceMaxMin(Y_Axis_max, max, min, max1, min1, max2, min2, max3, min3, max4, min4, max5, min5) + ",";
                    }

                    if (Y_Axis_labels != null && !Y_Axis_labels.Equals(""))
                    {
                        str += "\"labels\":[\"" + Y_Axis_labels.Replace(",", "\",\"") + "\"],";
                    }
                    else if (Y_Axis_labels_sql != null && !Y_Axis_labels_sql.Equals(""))
                    {
                        string sql0 = adv.SQLSelectSafe(adv.GetSpecialBase(context, Y_Axis_labels_sql, SiteID));
                        DR dr = db.OpenRecord(sql0);
                        int curcount = 0;
                        string tempstr = "";
                        while (dr.Read() && curcount < 48)
                        {
                            tempstr += "\"" + dr.GetValue(0).ToString() + "\",";
                            curcount++;
                        }
                        dr.Close();
                        tempstr = tempstr.Substring(0, tempstr.Length - 1);
                        str += "\"labels\":[" + tempstr + "],";
                    }

                    if (Y_Axis_3d != -1)
                    {
                        str += "\"3d\":" + Y_Axis_3d + ",";
                    }
                    if (Y_Axis_offset != -1)
                    {
                        str += "\"offset\":" + Y_Axis_offset + ",";
                    }
                    if (Y_Axis_tick != -1)
                    {
                        str += "\"tick-height\":" + Y_Axis_tick + ",";
                    }
                    if (Y_Axis_stroke != -1)
                    {
                        str += "\"stroke\":" + Y_Axis_stroke + ",";
                    }

                    str += "\"colour\":\"" + Y_Axis_color + "\"},";
                }
                if (Y_Right_Axis_open == 1)
                {
                    str += "\"y_axis_right\":{";
                    str += "\"grid-colour\":\"" + Y_Right_Axis_grid_color + "\",";
                    if (Y_Right_Axis_steps != null && !Y_Right_Axis_steps.Equals(""))
                    {
                        str += "\"steps\":" + ReplaceMaxMin(Y_Right_Axis_steps, max, min, max1, min1, max2, min2, max3, min3, max4, min4, max5, min5) + ",";
                    }
                    if (Y_Right_Axis_min != null && !Y_Right_Axis_min.Equals(""))
                    {
                        str += "\"min\":" + ReplaceMaxMin(Y_Right_Axis_min, max, min, max1, min1, max2, min2, max3, min3, max4, min4, max5, min5) + ",";
                    }
                    if (Y_Right_Axis_max != null && !Y_Right_Axis_max.Equals(""))
                    {
                        str += "\"max\":" + ReplaceMaxMin(Y_Right_Axis_max, max, min, max1, min1, max2, min2, max3, min3, max4, min4, max5, min5) + ",";
                    }

                    if (Y_Right_Axis_3d != -1)
                    {
                        str += "\"3d\":" + Y_Right_Axis_3d + ",";
                    }
                    if (Y_Right_Axis_offset != -1)
                    {
                        str += "\"offset\":" + Y_Right_Axis_offset + ",";
                    }
                    if (Y_Right_Axis_tick != -1)
                    {
                        str += "\"tick-height\":" + Y_Right_Axis_tick + ",";
                    }
                    if (Y_Right_Axis_stroke != -1)
                    {
                        str += "\"stroke\":" + Y_Right_Axis_stroke + ",";
                    }

                    str += "\"colour\":\"" + Y_Right_Axis_color + "\"},";
                }
                return str;
            }
            private static string EleValue(ArrayList al)
            {
                string str = "";
                for (int i = 0; i < al.Count; i++)
                {
                    str += "" + ((string[])al[i])[0] + "";
                    if (i < al.Count - 1)
                    {
                        str += ",";
                    }
                }
                return str;
            }
            private static string EleValue4Bar_Round_Glass(ArrayList al, string Chart_Color2)
            {
                string str = "";
                for (int i = 0; i < al.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        str += "" + ((string[])al[i])[0] + "";
                    }
                    else
                    {
                        str += "{\"top\":" + ((string[])al[i])[0] + ", \"colour\":\"" + Chart_Color2 + "\"}";
                    }
                    if (i < al.Count - 1)
                    {
                        str += ",";
                    }
                }
                return str;
            }
            private static string EleValue4HBar(ArrayList al, string HBar_Colors)
            {
                string str = "";
                string[] colors = HBar_Colors.Split(',');
                for (int i = 0; (i < al.Count) && (i < HBar_Colors.Length); i++)
                {
                    str += "{\"right\":" + ((string[])al[i])[0] + ", \"colour\":\"" + HBar_Colors[i] + "\"},";
                }
                if (str.Length > 0) str = str.Substring(0, str.Length - 1);
                return str;
            }
            private static string ReplaceMaxMin(string str, decimal max, decimal min, decimal max1, decimal min1, decimal max2, decimal min2, decimal max3, decimal min3, decimal max4, decimal min4, decimal max5, decimal min5)
            {
                return adv.CaculateMath(str.Replace("@max@", max.ToString()).Replace("@min@", min.ToString()).Replace("@max1@", max1.ToString()).Replace("@min1@", min1.ToString()).Replace("@max2@", max2.ToString()).Replace("@min2@", min2.ToString()).Replace("@max3@", max3.ToString()).Replace("@min3@", min3.ToString()).Replace("@max4@", max4.ToString()).Replace("@min4@", min4.ToString()).Replace("@max5@", max5.ToString()).Replace("@min5@", min5.ToString())).ToString("0");
            }
            private static string AppendChart(int ChartType, ArrayList al, int Chart_Position, string Chart_Tip, string Chart_Color, string Chart_Color2, string Chart_Alpha, string Chart_fillalpha, string Chart_fill)
            {
                string str = "";
                if (ChartType == 0)//柱状图基本型
                {
                    str += "{\"type\":\"bar\", \"tip\":\"" + Chart_Tip + "\",";
                    str += "\"values\":[";
                    str += EleValue(al);
                    str += "], \"colour\":\"" + Chart_Color + "\", \"axis\":\"" + (Chart_Position == 0 ? "left" : "right") + "\",\"alpha\":" + Chart_Alpha + "}";
                }
                else if (ChartType == 1)//柱状图玻璃型一
                {
                    str += "{\"type\":\"bar_glass\", \"tip\":\"" + Chart_Tip + "\",";
                    str += "\"values\":[";
                    str += EleValue(al);
                    str += "], \"colour\":\"" + Chart_Color + "\", \"axis\":\"" + (Chart_Position == 0 ? "left" : "right") + "\",\"alpha\":" + Chart_Alpha + "}";
                }
                else if (ChartType == 2)//柱状图立体型
                {
                    str += "{\"type\":\"bar_3d\", \"tip\":\"" + Chart_Tip + "\",";
                    str += "\"values\":[";
                    str += EleValue(al);
                    str += "], \"colour\":\"" + Chart_Color + "\", \"axis\":\"" + (Chart_Position == 0 ? "left" : "right") + "\",\"alpha\":" + Chart_Alpha + "}";
                }
                else if (ChartType == 3)//折线图基本型
                {
                    str += "{\"type\":\"line\", \"tip\":\"" + Chart_Tip + "\",";
                    str += "\"values\":[";
                    str += EleValue(al);
                    str += "], \"colour\":\"" + Chart_Color + "\", \"axis\":\"" + (Chart_Position == 0 ? "left" : "right") + "\",\"alpha\":" + Chart_Alpha + "}";

                }
                else if (ChartType == 4)//柱状图圆柱型
                {
                    str += "{\"type\":\"bar_cylinder\", \"tip\":\"" + Chart_Tip + "\",";
                    str += "\"values\":[";
                    str += EleValue(al);
                    str += "], \"colour\":\"" + Chart_Color + "\", \"axis\":\"" + (Chart_Position == 0 ? "left" : "right") + "\",\"alpha\":" + Chart_Alpha + "}";
                }
                else if (ChartType == 5)//柱状图玻璃型二
                {
                    str += "{\"type\":\"bar_round_glass\", \"tip\":\"" + Chart_Tip + "\",";
                    str += "\"values\":[";
                    str += EleValue4Bar_Round_Glass(al, Chart_Color2);
                    str += "], \"colour\":\"" + Chart_Color + "\", \"axis\":\"" + (Chart_Position == 0 ? "left" : "right") + "\",\"alpha\":" + Chart_Alpha + "}";
                }
                else if (ChartType == 6)//折线图区域型
                {
                    str += "{\"type\":\"area_line\", \"tip\":\"" + Chart_Tip + "\",";
                    str += "\"values\":[";
                    str += EleValue(al);
                    str += "], \"colour\":\"" + Chart_Color + "\",\"fill-alpha\":" + Chart_fillalpha + ",\"fill\":\"" + Chart_fill + "\", \"axis\":\"" + (Chart_Position == 0 ? "left" : "right") + "\",\"alpha\":" + Chart_Alpha + "}";
                }
                else if (ChartType == 7)//折线图热点型
                {
                    str += "{\"type\":\"line_dot\", \"tip\":\"" + Chart_Tip + "\",";
                    str += "\"values\":[";
                    str += EleValue(al);
                    str += "], \"colour\":\"" + Chart_Color + "\", \"axis\":\"" + (Chart_Position == 0 ? "left" : "right") + "\",\"alpha\":" + Chart_Alpha + "}";
                }
                else if (ChartType == 8)//折线图空心型
                {
                    str += "{\"type\":\"line_hollow\", \"tip\":\"" + Chart_Tip + "\",";
                    str += "\"values\":[";
                    str += EleValue(al);
                    str += "], \"colour\":\"" + Chart_Color + "\", \"axis\":\"" + (Chart_Position == 0 ? "left" : "right") + "\",\"alpha\":" + Chart_Alpha + "}";
                }
                return str;
            }
        }
    }
}
