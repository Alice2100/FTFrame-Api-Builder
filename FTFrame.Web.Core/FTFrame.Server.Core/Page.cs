using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using System.Linq;
using System.IO;
using System.Xml;
using System.Collections;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Web;
using FTFrame.Project.Core;
namespace FTFrame.Server.Core
{
    public class Page
    {
        public static List<string> IFormFileKeys(HttpRequest Request)
        {
            List<string> IFormFileNames = new List<string>();
            if (!Request.HasFormContentType) return IFormFileNames;
            IEnumerator<IFormFile> IFormFileEnumerator = Request.Form.Files.GetEnumerator();
            while (IFormFileEnumerator.MoveNext())
            {
                if (!string.IsNullOrEmpty(IFormFileEnumerator.Current.Name) && !IFormFileNames.Contains(IFormFileEnumerator.Current.Name))
                {
                    IFormFileNames.Add(IFormFileEnumerator.Current.Name);
                }
            }
            IFormFileEnumerator.Dispose();
            return IFormFileNames;
        }
        public static void FormSqlExec(DB db, ST st, string SqlBase, string SqlEvals, string newfid, HttpContext Context)
        {
            if (SqlBase == null || SqlEvals == null) return;
            SqlBase = SqlBase.Trim();
            if (SqlBase.Equals("")) return;
            string[] sqlparaitem = SqlEvals.Split(new string[] { "##" }, StringSplitOptions.None);
            int IndexI = 0;
            Regex r = new Regex(@"{[^}]*}");
            MatchCollection mc = r.Matches(SqlBase);
            foreach (Match m in mc)
            {
                if (sqlparaitem.Length > IndexI)
                {
                    SqlBase = SqlBase.Replace(m.Value, str.D2DD(sqlparaitem[IndexI]));
                }
                IndexI++;
            }
            string[] Sqls = SqlBase.Trim().Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string sql in Sqls)
            {
                string _sql = sql.Replace("@newfid@", newfid).Replace("_newfid_", newfid);
                FTFrame.Project.Core.Api.LogDebug(_sql, "[Sql Exec]");
                db.ExecSql(_sql, st);
            }
        }
        public static string ConditionReturn(string sqls, string codes, string sqlparas, HttpContext Context, string cdnjss, out string rtnCdnjs)//返回0或null为通过，返回1为默认错误信息，其他为自定义错误信息
        {
            rtnCdnjs = "";
            sqls = sqls.Trim();
            codes = codes.Trim();
            if (string.IsNullOrEmpty(sqls) && string.IsNullOrEmpty(codes)) return null;
            if (sqlparas != null)
            {
                string[] sqlparaitem = sqlparas.Split(new string[] { "##" }, StringSplitOptions.None);
                int IndexI = 0;
                Regex r = new Regex(@"{[^}]*}");
                MatchCollection mc = r.Matches(sqls);
                foreach (Match m in mc)
                {
                    if (sqlparaitem.Length > IndexI)
                    {
                        sqls = sqls.Replace(m.Value, str.D2DD(sqlparaitem[IndexI]));
                    }
                    IndexI++;
                }
            }
            var sqlsA = sqls.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
            var codesA = codes.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
            var cdnjssA = cdnjss.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < sqlsA.Length; i++)
            {
                string valiVal = null;
                string cdnJsFromSql = null;
                string sql = sqlsA[i];
                DB db = new DB();
                db.Open();
                try
                {
                    DR dr = db.OpenRecord(sql);
                    if (dr.Read())
                    {
                        if (int.Parse(dr.GetValue(0).ToString()) != 0)
                        {
                            if (dr.FieldCount >= 2)
                            {
                                cdnJsFromSql = dr.GetValue(1).ToString();
                            }
                            dr.Close();
                            valiVal = "1";
                        }
                        else
                        {
                            dr.Close();
                        }
                    }
                    else
                    {
                        dr.Close();
                        valiVal = "1";
                    }
                }
                catch (Exception ex)
                {
                    FTFrame.Project.Core.Api.LogError(ex);
                    valiVal = ex.Message;
                }
                finally
                {
                    db.Close();
                }
                if (valiVal != null)
                {
                    if (cdnJsFromSql != null) rtnCdnjs = cdnJsFromSql;
                    else
                    {
                        if (i < cdnjssA.Length) rtnCdnjs = cdnjssA[i];
                        else if (cdnjssA.Length > 0) rtnCdnjs = cdnjssA[0];
                        else rtnCdnjs = cdnjss;
                    }
                    return valiVal;
                }
            }
            for (int i = 0; i < codesA.Length; i++)
            {
                string code = codesA[i];
                if (code.StartsWith("@code("))
                {
                    string cdnreturn = Interface.Code.Get(code, Context);
                    if (cdnreturn != null && !cdnreturn.Equals("0"))
                    {
                        rtnCdnjs = "[STR]"+cdnreturn;
                        return cdnreturn;
                    }
                    //string[] codeitem = code.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                    //foreach (string codei in codeitem)
                    //{
                    //    string cdnreturn = Interface.Code.Get(codei, Context);
                    //    if (cdnreturn != null && !cdnreturn.Equals("0")) return cdnreturn;
                    //}
                }
                else if (code.StartsWith("@para{"))
                {
                    string cdnreturn = Tool.adv.ParaPattern(Context,code);
                    if (cdnreturn != null && !cdnreturn.Equals("0"))
                    {
                        rtnCdnjs = "[STR]" + cdnreturn;
                        return cdnreturn;
                    }
                    //string[] codeitem = code.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                    //foreach (string codei in codeitem)
                    //{
                    //    string cdnreturn = Interface.Code.Get(codei, Context);
                    //    if (cdnreturn != null && !cdnreturn.Equals("0")) return cdnreturn;
                    //}
                }
            }
            return null;
        }
        public static string TableNameByTableTag(string tableTag)
        {
            if (tableTag.StartsWith("@")) tableTag = tableTag.Substring(1);
            if (tableTag.IndexOf('.') > 0) tableTag = tableTag.Substring(0, tableTag.IndexOf('.'));
            return tableTag;
        }
    }
    public class Fore
    {
        private static string[] GetSplitArray(string OriginalString)
        {
            if (OriginalString == null || OriginalString.Equals(""))
            {
                return new string[0];
            }
            else
            {
                return OriginalString.Split(';');
            }
        }
        private static string ReplaceRequest(string oristring, HttpRequest request)
        {
            //@Request["id"]
            //@RequestForm["id"]
            string result = oristring;
            int indexLeft;
            int indexRight;
            string willrepstr = null;
            string parastr = null;
            while (result.IndexOf("@Request[") >= 0)
            {
                indexLeft = result.IndexOf("@Request[");
                indexRight = result.IndexOf("]@");
                willrepstr = result.Substring(indexLeft, indexRight - indexLeft + 2);
                parastr = willrepstr.Replace("@Request[", "").Replace("]@", "");
                if (request.Query[parastr].FirstOrDefault<string>() != null)
                {
                    result = result.Replace(willrepstr, request.Query[parastr].FirstOrDefault<string>());
                }
            }
            while (result.IndexOf("@RequestForm[") >= 0)
            {
                indexLeft = result.IndexOf("@RequestForm[");
                indexRight = result.IndexOf("]@");
                willrepstr = result.Substring(indexLeft, indexRight - indexLeft + 2);
                parastr = willrepstr.Replace("@RequestForm[", "").Replace("]@", "");
                if (request.HasFormContentType && request.Form[parastr].FirstOrDefault<string>() != null)
                {
                    result = result.Replace(willrepstr, request.Form[parastr]);
                }
            }
            return result;

        }
        private static string ReplaceSession(string oristring, HttpContext context)
        {
            string result = oristring;
            int indexLeft;
            int indexRight;
            string willrepstr = null;
            string parastr = null;
            while (result.IndexOf("@Session[") >= 0)
            {
                indexLeft = result.IndexOf("@Session[");
                indexRight = result.IndexOf("]@");
                willrepstr = result.Substring(indexLeft, indexRight - indexLeft + 2);
                parastr = willrepstr.Replace("@Session[", "").Replace("]@", "");
                result = result.Replace(willrepstr, session.Get(parastr) == null ? "" : session.Get(parastr).ToString());
            }
            return result;

        }
        public static string GetBackValue(HttpContext Context, DB db, bool sonpage, string dsbackvalueappendjs0, string dsbackvalueappendjs, string[] dsbackvaluesqls)
        {
            string restr = "var eleobj;";
            if (sonpage)
            {
                restr += dsbackvalueappendjs0.Replace("document.", "parent.document.").Replace("{dsbvqt}", "\"") + ";";
            }
            else
            {
                restr += dsbackvalueappendjs0.Replace("{dsbvqt}", "\"") + ";";
            }
            DR rdr = null;
            int index = 1;
            string dsbackvaluesql = dsbackvaluesqls[index - 1];
            while (dsbackvaluesql != null)
            {
                dsbackvaluesql = dsbackvaluesql.Trim().Replace("{dsbvqt}", "\"");
                if (!dsbackvaluesql.Equals(""))
                {
                    string[] bvsqls = dsbackvaluesql.Split('&');
                    foreach (string _bvsql in bvsqls)
                    {
                        string bvsql = _bvsql.Trim();
                        if (!bvsql.Equals(""))
                        {
                            string[] bvitems = bvsql.Split('|');
                            bvitems[0] = bvitems[0].Trim();
                            bvitems[1] = bvitems[1].Trim();
                            string sql = bvitems[0].Replace("[#]", "|").Replace("[$]", "&");
                            if (bvitems[1].StartsWith("dimele("))//维表
                            {
                                string[] dimitems = bvitems[1].Split('=');
                                if (sonpage)
                                {
                                    restr += "eleobj=" + dimitems[0].Replace("dimele(", "parent.document.getElementById(\"").Replace(")", "\");");
                                    restr += "if(eleobj.name.indexOf('_rowrate')>=0 || parent.document.getElementsByName(eleobj.name+'_rowrate1').length>0)";
                                    restr += "{";
                                    restr += "var rowid='';var srcid=parent.BVSrcElement.id;if(srcid.indexOf('_rowrate')>=0)rowid=srcid.substring(srcid.indexOf('_rowrate'));";
                                    restr += "eleobj=parent.document.getElementById(eleobj.id+rowid);";
                                    restr += "}";
                                }
                                else
                                {
                                    restr += "eleobj=" + dimitems[0].Replace("dimele(", "document.getElementById(\"").Replace(")", "\");");
                                }
                                restr += "eleobj.innerHTML=\"\";";
                                dimitems[1] = dimitems[1].Replace("val(", "").Replace(")", "");
                                string sqlcol1 = dimitems[1].Split(',')[0];
                                string sqlcol2 = dimitems[1].Split(',')[1];
                                rdr = db.OpenRecord(sql);

                                while (rdr.Read())
                                {
                                    string val1 = "";
                                    string val2 = "";
                                    int getval2int = -1;
                                    if (int.TryParse(sqlcol1, out getval2int))
                                    {
                                        val1 = rdr.GetValue(getval2int) == null ? "" : rdr.GetValue(getval2int).ToString();
                                    }
                                    else
                                    {
                                        val1 = rdr.GetValue(rdr.GetOrdinal(sqlcol1)) == null ? "" : rdr.GetValue(rdr.GetOrdinal(sqlcol1)).ToString();
                                    }
                                    getval2int = -1;
                                    if (int.TryParse(sqlcol2, out getval2int))
                                    {
                                        val2 = rdr.GetValue(getval2int) == null ? "" : rdr.GetValue(getval2int).ToString();
                                    }
                                    else
                                    {
                                        val2 = rdr.GetValue(rdr.GetOrdinal(sqlcol2)) == null ? "" : rdr.GetValue(rdr.GetOrdinal(sqlcol2)).ToString();
                                    }
                                    restr += "eleobj.options[eleobj.length]=new Option(\"" + val1.Replace("\"", "\\\"") + "\",\"" + val2.Replace("\"", "\\\"") + "\");";
                                }
                                rdr.Close();
                            }
                            else if (bvitems[1].StartsWith("checkboxele("))//复选框
                            {
                                string[] dimitems = bvitems[1].Split('=');
                                string dimeleidname = dimitems[0].Replace("checkboxele(", "").Replace(")", "");
                                if (sonpage)
                                {
                                    restr += "var bvd_box_objs=parent.document.getElementsByName(\"" + dimeleidname + "\");";
                                    restr += "if(bvd_box_objs.length>0 && (bvd_box_objs[0].name.indexOf('_rowrate')>=0 || parent.document.getElementsByName(bvd_box_objs[0].name+'_rowrate1').length>0))";
                                    restr += "{";
                                    restr += "var rowid='';var srcid=parent.BVSrcElement.id;if(srcid.indexOf('_rowrate')>=0)rowid=srcid.substring(srcid.indexOf('_rowrate'));";
                                    restr += "bvd_box_objs=parent.document.getElementsByName(bvd_box_objs[0].name+rowid);";
                                    restr += "}";
                                }
                                else
                                {
                                    restr += "var bvd_box_objs=document.getElementsByName(\"" + dimeleidname + "\");";
                                }
                                restr += "var bvd_box_objlength=bvd_box_objs.length;";
                                restr += "for(i=0;i<bvd_box_objlength-1;i++)";
                                restr += "{bvd_box_objs[0].outerHTML=\"\";}";
                                restr += "var bvd_box_html=\"";
                                dimitems[1] = dimitems[1].Replace("val(", "").Replace(")", "");
                                string sqlcol1 = dimitems[1].Split(',')[0];
                                string sqlcol2 = dimitems[1].Split(',')[1];
                                rdr = db.OpenRecord(sql);

                                while (rdr.Read())
                                {
                                    string val1 = "";
                                    string val2 = "";
                                    int getval2int = -1;
                                    if (int.TryParse(sqlcol1, out getval2int))
                                    {
                                        val1 = rdr.GetValue(getval2int) == null ? "" : rdr.GetValue(getval2int).ToString();
                                    }
                                    else
                                    {
                                        val1 = rdr.GetValue(rdr.GetOrdinal(sqlcol1)) == null ? "" : rdr.GetValue(rdr.GetOrdinal(sqlcol1)).ToString();
                                    }
                                    getval2int = -1;
                                    if (int.TryParse(sqlcol2, out getval2int))
                                    {
                                        val2 = rdr.GetValue(getval2int) == null ? "" : rdr.GetValue(getval2int).ToString();
                                    }
                                    else
                                    {
                                        val2 = rdr.GetValue(rdr.GetOrdinal(sqlcol2)) == null ? "" : rdr.GetValue(rdr.GetOrdinal(sqlcol2)).ToString();
                                    }
                                    restr += "<INPUT id=" + dimeleidname + " type=checkbox value=" + val1.Replace("\"", "\\\"") + " name=" + dimeleidname + ">" + val2.Replace("\"", "\\\"") + " ";
                                }
                                rdr.Close();
                                restr += "\";";
                                restr += "bvd_box_objs[0].outerHTML=bvd_box_html;";
                            }
                            else if (bvitems[1].StartsWith("radioele("))//单选框
                            {
                                string[] dimitems = bvitems[1].Split('=');
                                string dimeleidname = dimitems[0].Replace("radioele(", "").Replace(")", "");
                                if (sonpage)
                                {
                                    restr += "var bvd_box_objs=parent.document.getElementsByName(\"" + dimeleidname + "\");";
                                    restr += "if(bvd_box_objs.length>0 && (bvd_box_objs[0].name.indexOf('_rowrate')>=0 || parent.document.getElementsByName(bvd_box_objs[0].name+'_rowrate1').length>0))";
                                    restr += "{";
                                    restr += "var rowid='';var srcid=parent.BVSrcElement.id;if(srcid.indexOf('_rowrate')>=0)rowid=srcid.substring(srcid.indexOf('_rowrate'));";
                                    restr += "bvd_box_objs=parent.document.getElementsByName(bvd_box_objs[0].name+rowid);";
                                    restr += "}";
                                }
                                else
                                {
                                    restr += "var bvd_box_objs=document.getElementsByName(\"" + dimeleidname + "\");";
                                }
                                restr += "var bvd_box_objlength=bvd_box_objs.length;";
                                restr += "for(i=0;i<bvd_box_objlength-1;i++)";
                                restr += "{bvd_box_objs[0].outerHTML=\"\";}";
                                restr += "var bvd_box_html=\"";
                                dimitems[1] = dimitems[1].Replace("val(", "").Replace(")", "");
                                string sqlcol1 = dimitems[1].Split(',')[0];
                                string sqlcol2 = dimitems[1].Split(',')[1];
                                rdr = db.OpenRecord(sql);

                                while (rdr.Read())
                                {
                                    string val1 = "";
                                    string val2 = "";
                                    int getval2int = -1;
                                    if (int.TryParse(sqlcol1, out getval2int))
                                    {
                                        val1 = rdr.GetValue(getval2int) == null ? "" : rdr.GetValue(getval2int).ToString();
                                    }
                                    else
                                    {
                                        val1 = rdr.GetValue(rdr.GetOrdinal(sqlcol1)) == null ? "" : rdr.GetValue(rdr.GetOrdinal(sqlcol1)).ToString();
                                    }
                                    getval2int = -1;
                                    if (int.TryParse(sqlcol2, out getval2int))
                                    {
                                        val2 = rdr.GetValue(getval2int) == null ? "" : rdr.GetValue(getval2int).ToString();
                                    }
                                    else
                                    {
                                        val2 = rdr.GetValue(rdr.GetOrdinal(sqlcol2)) == null ? "" : rdr.GetValue(rdr.GetOrdinal(sqlcol2)).ToString();
                                    }
                                    restr += "<INPUT id=" + dimeleidname + " type=radio value=" + val1.Replace("\"", "\\\"") + " name=" + dimeleidname + ">" + val2.Replace("\"", "\\\"") + " ";
                                }
                                rdr.Close();
                                restr += "\";";
                                restr += "bvd_box_objs[0].outerHTML=bvd_box_html;";
                            }
                            //else if (bvitems[1].StartsWith("eles("))//多条记录赋值
                            //{
                            //    string[] getids = bvitems[1].Replace("eles(", "").Replace(")", "").Split(',');
                            //    rdr = db.OpenRecord(sql);
                            //    int idsrant = 0;
                            //    while (rdr.Read()&&idsrant<getids.Length)
                            //    {
                            //        string elegetvalue = (rdr.GetValue(0) == null ? "" : rdr.GetValue(0).ToString().Replace("\"", "\\\"").Replace("\r\n", "\\r\\n"));
                            //        string getelefunc = null;
                            //        if (sonpage)
                            //        {
                            //            getelefunc = "parent.document.getElementById";
                            //            restr += "var eleobj=" + getelefunc + "(\"" + getids[idsrant] + "\");";
                            //            restr += "if(eleobj.name.indexOf('_rowrate')>=0 || parent.document.getElementsByName(eleobj.name+'_rowrate1').length>0)";
                            //            restr += "{";
                            //            restr += "var rowid='';var srcid=parent.BVSrcElement.id;if(srcid.indexOf('_rowrate')>=0)rowid=srcid.substring(srcid.indexOf('_rowrate'));";
                            //            restr += "eleobj=parent.document.getElementById(eleobj.id+rowid);";
                            //            restr += "}";
                            //        }
                            //        else
                            //        {
                            //            getelefunc = "document.getElementById";
                            //            restr += "var eleobj=" + getelefunc + "(\"" + getids[idsrant] + "\");";
                            //        }
                            //        restr += "if(eleobj.tagName!=\"LABEL\")";
                            //        restr += "{";
                            //        restr += "eleobj";
                            //        restr += ".value=\"" + elegetvalue + "\";";
                            //        restr += "}";
                            //        restr += "else{";
                            //        restr += "eleobj";
                            //        restr += ".innerText=\"" + elegetvalue + "\";";
                            //        restr += "}";
                            //        idsrant++;
                            //    }
                            //    rdr.Close();
                            //}
                            else if (bvitems[1].StartsWith("eles("))//多条记录赋值
                            {
                                string[] getids = bvitems[1].Replace("eles(", "").Replace(")", "").Split(',');
                                rdr = db.OpenRecord(sql);
                                int idsrant = 0;
                                if (rdr.Read() && idsrant < getids.Length)
                                {
                                    for (idsrant = 0; idsrant < getids.Length; idsrant++)
                                    {
                                        string elegetvalue = (rdr.GetValue(idsrant) == null ? "" : rdr.GetValue(idsrant).ToString().Replace("\"", "\\\"").Replace("\r\n", "\\r\\n"));
                                        string getelefunc = null;
                                        if (sonpage)
                                        {
                                            getelefunc = "parent.document.getElementById";
                                            restr += "var eleobj=" + getelefunc + "(\"" + getids[idsrant] + "\");";
                                            restr += "if(eleobj.name.indexOf('_rowrate')>=0 || parent.document.getElementsByName(eleobj.name+'_rowrate1').length>0)";
                                            restr += "{";
                                            restr += "var rowid='';var srcid=parent.BVSrcElement.id;if(srcid.indexOf('_rowrate')>=0)rowid=srcid.substring(srcid.indexOf('_rowrate'));";
                                            restr += "eleobj=parent.document.getElementById(eleobj.id+rowid);";
                                            restr += "}";
                                        }
                                        else
                                        {
                                            getelefunc = "document.getElementById";
                                            restr += "var eleobj=" + getelefunc + "(\"" + getids[idsrant] + "\");";
                                        }
                                        restr += "if(eleobj.tagName!=\"LABEL\")";
                                        restr += "{";
                                        restr += "eleobj";
                                        restr += ".value=\"" + elegetvalue + "\";";
                                        restr += "}";
                                        restr += "else{";
                                        restr += "eleobj";
                                        restr += ".innerText=\"" + elegetvalue + "\";";
                                        restr += "}";
                                    }
                                }
                                rdr.Close();
                            }
                            else//一般
                            {
                                string[] getvals = bvitems[1].Replace("[#]", "|").Replace("[$]", "&").Split(';');
                                rdr = db.OpenRecord(sql);
                                if (rdr.Read())
                                {
                                    foreach (string _getval in getvals)
                                    {
                                        string getval = _getval.Trim();
                                        if (!getval.Equals(""))
                                        {
                                            string getval1 = getval.Split('=')[0];
                                            string getval2 = getval.Split('=')[1];

                                            string elegetvalue = null;

                                            getval2 = getval2.Replace("val(", "").Replace(")", "");
                                            int getval2int = -1;
                                            if (int.TryParse(getval2, out getval2int))
                                            {
                                                elegetvalue = rdr.GetValue(getval2int) == null ? "" : rdr.GetValue(getval2int).ToString().Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                                            }
                                            else
                                            {
                                                elegetvalue = rdr.GetValue(rdr.GetOrdinal(getval2)) == null ? "" : rdr.GetValue(rdr.GetOrdinal(getval2)).ToString().Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                                            }

                                            string getelefunc = null;
                                            string geteleid = getval1.Replace("ele(", "").Replace(")", "");
                                            if (sonpage)
                                            {
                                                getelefunc = "parent.document.getElementById";
                                                restr += "var eleobj=" + getelefunc + "(\"" + geteleid + "\");";
                                                restr += "if(eleobj.name.indexOf('_rowrate')>=0 || parent.document.getElementsByName(eleobj.name+'_rowrate1').length>0)";
                                                restr += "{";
                                                restr += "var rowid='';var srcid=parent.BVSrcElement.id;if(srcid.indexOf('_rowrate')>=0)rowid=srcid.substring(srcid.indexOf('_rowrate'));";
                                                restr += "eleobj=parent.document.getElementById(eleobj.id+rowid);";
                                                restr += "}";
                                            }
                                            else
                                            {
                                                getelefunc = "document.getElementById";
                                                restr += "var eleobj=" + getelefunc + "(\"" + geteleid + "\");";
                                            }


                                            restr += "if(eleobj.tagName!=\"LABEL\")";
                                            restr += "{";
                                            restr += "eleobj";
                                            restr += ".value=\"" + elegetvalue + "\";";
                                            restr += "}";
                                            restr += "else{";
                                            restr += "eleobj";
                                            restr += ".innerText=\"" + elegetvalue + "\";";
                                            restr += "}";

                                        }
                                    }
                                }
                                rdr.Close();
                            }
                        }
                    }
                }
                index++;
                dsbackvaluesql = (index <= dsbackvaluesqls.Length) ? dsbackvaluesqls[index - 1] : null;
            }
            rdr = null;
            if (sonpage)
            {
                restr += dsbackvalueappendjs.Replace("document.", "parent.document.").Replace("{dsbvqt}", "\"") + ";";
            }
            else
            {
                restr += dsbackvalueappendjs.Replace("{dsbvqt}", "\"") + ";";
            }
            if (sonpage)
            {
                restr += "parent.backValueLoadDn();";
            }
            else
            {
                restr += "backValueLoadDn();";
            }
            return restr;
        }
        public static string GetSelectValue(HttpContext Context, DB db, string TagID, string RowSQL, string RowData, int RowSelect)
        {
            string restr = "var newtr;";
            restr += "var firsttr=parent.document.getElementById('t_" + TagID + "');";
            restr += "$(firsttr)[0].style.display='';";
            restr += "$(firsttr).parent().children(\":gt(1)\").remove();";
            restr += "var strtag=firsttr.innerHTML;var str;";
            DR dr = db.OpenRecord(RowSQL);
            while (dr.Read())
            {
                restr += "newtr=parent.document.createElement('TR');";
                restr += "newtr.id='r_" + dr.GetValue(0).ToString() + "';";
                restr += "str=strtag;";
                string[] items = RowData.Split(';');
                foreach (string s in items)
                {
                    if (s.Trim().Equals("")) continue;
                    restr += "str=str.replace(/\\{" + s.Trim() + "\\}/g,\"" + dr.GetValue(dr.GetOrdinal(s.Trim())).ToString() + "\");";
                }
                restr += "$(newtr).html(str);";
                restr += "$(newtr).attr('style',$(firsttr).attr('style'));";
                //restr += "$(newtr).attr('onmouseover',$(firsttr).attr('onmouseover'));";
                //restr += "$(newtr).attr('onmouseout',$(firsttr).attr('onmouseout'));";
                restr += "$(newtr).on('mouseover',function(){eval($(firsttr).attr('onmouseover'))});";
                restr += "$(newtr).on('mouseout',function(){eval($(firsttr).attr('onmouseout'))});";
                if (RowSelect == 0)
                {
                    restr += "$(newtr)[0].style.cursor='pointer';";
                    //restr += "$(newtr).attr('onclick','');";
                    //restr += "$(newtr).unbind('click');";
                    //restr += "$(newtr).bind('click',function(){parent.sl_RowSelected(this,'s_" + TagID + "','f_" + TagID + "')});";
                    restr += "if(newtr.children[0].children[0]!=null&&newtr.children[0].children[0].tagName=='INPUT'){";
                    restr += "$(newtr).children(':gt(0)').on('click',function(){parent.sl_RowSelected(this,'s_" + TagID + "','f_" + TagID + "')});";
                    restr += "}else{";
                    restr += "$(newtr).on('click',function(){parent.sl_RowSelected(this,'s_" + TagID + "','f_" + TagID + "')});";
                    restr += "}";
                    //restr += "$(newtr).bind('click',function(){alert(1)});";
                }
                restr += "if(newtr.children[0].children[0]!=null&&newtr.children[0].children[0].tagName=='INPUT'){";
                restr += "if(parent.CurSelectIDObj!=null){if((','+parent.CurSelectIDObj.value+',').indexOf('," + dr.GetValue(0).ToString() + ",')>=0)newtr.children[0].children[0].click();}";
                restr += "}";
                restr += "$(firsttr).parent().append($(newtr));";
            }
            dr.Close();
            restr += "parent.document.getElementById('l_" + TagID + "').style.display='none';";
            restr += "$(firsttr).parents('table')[0].style.display='';";
            restr += "$(firsttr)[0].style.display='none';";
            return restr;
        }
        public static string GetCusColAddValue(HttpContext Context, DB db)
        {
            ST st = null;
            try
            {
                string s = "";
                string tablename = Context.Request.Form["tablename"].FirstOrDefault<string>();
                string siteid = Context.Request.Form["siteid"].FirstOrDefault<string>();
                string cus_xsmc = Context.Request.Form["cus_xsmc"].FirstOrDefault<string>().Trim();
                string cus_sjlx = Context.Request.Form["cus_sjlx"].FirstOrDefault<string>().Trim();
                string cus_sjbg = Context.Request.Form["cus_sjbg"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_sjbg"].FirstOrDefault<string>().Trim();
                int cus_pxh = int.Parse(Context.Request.Form["cus_pxh"].FirstOrDefault<string>().Trim());
                int cus_isshow = int.Parse(Context.Request.Form["cus_isshow"].FirstOrDefault<string>().Trim());
                string cus_zdmc = Context.Request.Form["cus_zdmc"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_zdmc"].FirstOrDefault<string>().Trim();
                int cus_xtsc = (Context.Request.Form["cus_xtsc"].FirstOrDefault<string>() != null && Context.Request.Form["cus_xtsc"].FirstOrDefault<string>().Equals("1")) ? 1 : 0;
                int cus_sjcd = (Context.Request.Form["cus_sjcd"].FirstOrDefault<string>() == null || Context.Request.Form["cus_sjcd"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_sjcd"].FirstOrDefault<string>().Trim());
                int cus_xsws = (Context.Request.Form["cus_xsws"].FirstOrDefault<string>() == null || Context.Request.Form["cus_xsws"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_xsws"].FirstOrDefault<string>().Trim());
                int cus_kwnull = (Context.Request.Form["cus_kwnull"].FirstOrDefault<string>() == null || Context.Request.Form["cus_kwnull"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_kwnull"].FirstOrDefault<string>());
                string cus_mrz = Context.Request.Form["cus_mrz"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_mrz"].FirstOrDefault<string>().Trim();
                int cus_srlx = (Context.Request.Form["cus_srlx"].FirstOrDefault<string>() == null || Context.Request.Form["cus_srlx"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_srlx"].FirstOrDefault<string>());
                int cus_bt = (Context.Request.Form["cus_bt"].FirstOrDefault<string>() == null || Context.Request.Form["cus_bt"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_bt"].FirstOrDefault<string>());
                string cus_wd = Context.Request.Form["cus_wd"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_wd"].FirstOrDefault<string>().Trim();
                string cus_gd = Context.Request.Form["cus_gd"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_gd"].FirstOrDefault<string>().Trim();
                int cus_xldx = (Context.Request.Form["cus_xldx"].FirstOrDefault<string>() == null || Context.Request.Form["cus_xldx"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_xldx"].FirstOrDefault<string>());
                int cus_dxhs = (Context.Request.Form["cus_dxhs"].FirstOrDefault<string>() == null || Context.Request.Form["cus_dxhs"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_dxhs"].FirstOrDefault<string>());
                string cus_kxx = Context.Request.Form["cus_kxx"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_kxx"].FirstOrDefault<string>().Trim();
                string cus_yxx = Context.Request.Form["cus_yxx"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_yxx"].FirstOrDefault<string>().Trim();
                int cus_dtxz = (Context.Request.Form["cus_dtxz"].FirstOrDefault<string>() == null || Context.Request.Form["cus_dtxz"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_dtxz"].FirstOrDefault<string>());
                string cus_dtxzcap = Context.Request.Form["cus_dtxzcap"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_dtxzcap"].FirstOrDefault<string>().Trim();
                string cus_paraname = Context.Request.Form["cus_paraname"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_paraname"].FirstOrDefault<string>().Trim();

                if (cus_sjbg.IndexOf("_") >= 0 || cus_zdmc.IndexOf("_") >= 0 || cus_paraname.IndexOf("_") >= 0)
                {
                    return "alert(\"数据表格、字段名称和自定义参数不能包含下划线'_'\");parent.SubmitingDn();";
                }

                string sqlalter = null;
                string eid = null;
                if (!cus_sjlx.Equals("line"))
                {
                    string altertable = cus_sjbg.StartsWith("@") ? cus_sjbg.Substring(1) : ("ft_" + siteid + "_f_" + cus_sjbg);
                    if (SysConst.DataBaseType.Equals(DataBase.MySql))
                    {
                        //以下更改表结构需手动完成：更改索引、主键、删除列、非常规数据类型不更改
                        sqlalter = Sql.TableStructSql(altertable);
                        DR dr = db.OpenRecord(sqlalter);
                        Hashtable curDataTable = new Hashtable();
                        while (dr.Read())
                        {
                            curDataTable.Add(dr.GetString("Field").ToLower(), new string[] { dr.GetString("Type"), dr.GetString("Null"), (dr.GetString("Default") == null ? "NULL" : dr.GetString("Default")) });
                        }
                        dr.Close();
                        if (cus_srlx == 8)
                        {
                            eid = cus_zdmc;
                            if (eid.Equals(""))
                            {
                                return "alert(\"标签显示必须手动指定字段名称\");parent.SubmitingDn();";
                            }
                            if (!curDataTable.ContainsKey(eid))
                            {
                                return "alert(\"列名" + eid + "在指定数据表中不存在\");parent.SubmitingDn();";
                            }
                        }
                        else
                        {
                            if (cus_xtsc == 1)//自动列名
                            {
                                int i = 0;
                                for (i = 0; i < 26 * 26; i++)
                                {
                                    int l = i / 26;
                                    int r = i % 26;
                                    eid = ((char)(l + 97)).ToString() + ((char)(r + 97)).ToString();
                                    if (eid.Equals("as") || eid.Equals("by")) continue;
                                    if (!curDataTable.ContainsKey(eid))
                                    {
                                        break;
                                    }
                                }
                                if (curDataTable.ContainsKey(eid))
                                {
                                    return "alert(\"已经达到自动生成列的最大值" + (26 * 26) + "列\");parent.SubmitingDn();";
                                }
                            }
                            else
                            {
                                eid = cus_zdmc;
                                if (curDataTable.ContainsKey(eid))
                                {
                                    return "alert(\"列名" + eid + "在指定数据表中已经存在\");parent.SubmitingDn();";
                                }
                            }
                        }
                        if (cus_srlx != 8)
                        {
                            sqlalter = "ALTER TABLE " + altertable + " ADD " + eid + " " + Sql.DataTypeConvert(Sql.DataTypeConvertBase(cus_sjlx), cus_sjcd, cus_xsws) + " " + ((cus_kwnull == 0) ? "NOT NULL" : "");
                            if (!Sql.DataTypeConvertBase(cus_sjlx).Equals("text") && !cus_mrz.Equals(""))
                            {
                                sqlalter += " default '" + cus_mrz + "'";
                            }
                        }
                        else
                        {
                            sqlalter = null;
                        }
                        curDataTable.Clear();
                        curDataTable = null;
                    }
                    else if (SysConst.DataBaseType.Equals(DataBase.SqlServer))
                    {
                        //以下更改表结构需手动完成：更改索引、主键、删除列、默认值、非常规数据类型不更改
                        sqlalter = Sql.TableStructSql(altertable);
                        DR dr = db.OpenRecord(sqlalter);
                        Hashtable curDataTable = new Hashtable();
                        while (dr.Read())
                        {
                            curDataTable.Add(dr.GetString("ColumnName").ToLower(), new string[] { dr.GetString("Type"), dr.GetStringForce("length"), dr.GetStringForce("xprec"), dr.GetStringForce("xscale"), dr.GetStringForce("isnullable"), dr.GetStringForce("cdefault") });
                        }
                        dr.Close();
                        if (cus_srlx == 8)
                        {
                            eid = cus_zdmc;
                            if (eid.Equals(""))
                            {
                                return "alert(\"标签显示必须手动指定字段名称\");parent.SubmitingDn();";
                            }
                            if (!curDataTable.ContainsKey(eid))
                            {
                                return "alert(\"列名" + eid + "在指定数据表中不存在\");parent.SubmitingDn();";
                            }
                        }
                        else
                        {
                            if (cus_xtsc == 1)//自动列名
                            {
                                int i = 0;
                                for (i = 0; i < 26 * 26; i++)
                                {
                                    int l = i / 26;
                                    int r = i % 26;
                                    eid = ((char)(l + 97)).ToString() + ((char)(r + 97)).ToString();
                                    if (eid.Equals("as") || eid.Equals("by")) continue;
                                    if (!curDataTable.ContainsKey(eid))
                                    {
                                        break;
                                    }
                                }
                                if (curDataTable.ContainsKey(eid))
                                {
                                    return "alert(\"已经达到自动生成列的最大值" + (26 * 26) + "列\");parent.SubmitingDn();";
                                }
                            }
                            else
                            {
                                eid = cus_zdmc;
                                if (curDataTable.ContainsKey(eid))
                                {
                                    return "alert(\"列名" + eid + "在指定数据表中已经存在\");parent.SubmitingDn();";
                                }
                            }
                        }
                        if (cus_srlx != 8)
                        {
                            sqlalter = "ALTER TABLE " + altertable + " ADD " + eid + " " + Sql.DataTypeConvert(Sql.DataTypeConvertBase(cus_sjlx), cus_sjcd, cus_xsws) + " " + ((cus_kwnull == 0) ? "NOT NULL" : "NULL");
                            if (!cus_mrz.Equals(""))
                            {
                                sqlalter += " default '" + cus_mrz + "'";
                            }
                        }
                        else
                        {
                            sqlalter = null;
                        }
                        curDataTable.Clear();
                        curDataTable = null;
                    }
                }

                string sqlnormal = null;
                sqlnormal = "insert into " + tablename + "(pxh,xsmc,sjlx,sjbg,zdmc,xtsc,sjcd,xsws,kwnull,mrz,srlx,bt,wd,gd,xldx,dxhs,kxx,yxx,dtxz,dtxzcap,paraname,isshow,modify_time,publish_time)";
                sqlnormal += "values(" + cus_pxh + ",'" + str.D2DD(cus_xsmc) + "','" + str.D2DD(cus_sjlx) + "','" + str.D2DD(cus_sjbg) + "','" + str.D2DD((eid == null ? "" : eid)) + "'," + cus_xtsc + "," + cus_sjcd + "," + cus_xsws + "," + cus_kwnull + ",'" + str.D2DD(cus_mrz) + "'," + cus_srlx + "," + cus_bt + ",'" + str.D2DD(cus_wd) + "','" + str.D2DD(cus_gd) + "'," + cus_xldx + "," + cus_dxhs + ",'" + str.D2DD(cus_kxx) + "','" + str.D2DD(cus_yxx) + "'," + cus_dtxz + ",'" + str.D2DD(cus_dtxzcap) + "','" + str.D2DD(cus_paraname) + "'," + cus_isshow + ",'" + str.GetDateTime() + "','" + str.GetDateTime() + "')";


                st = db.GetTransaction();
                FTFrame.Project.Core.Api.LogDebug(sqlnormal, "CusCol Nomal");
                db.ExecSql(sqlnormal, st);
                if (sqlalter != null)
                {
                    FTFrame.Project.Core.Api.LogDebug(sqlalter, "CusCol Add");
                    db.ExecSql(sqlalter, st);
                }
                st.Commit();
                s = "parent.SubmitingDn();alert('Add Successfully!');parent.location.reload();";
                return s;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (st != null) st.Rollback();
                return "alert(\"" + ex.Message + "\");parent.SubmitingDn();";
            }
        }
        public static string GetCusColModValue(HttpContext Context, DB db, int id)
        {
            ST st = null;
            try
            {
                string s = "";
                string tablename = Context.Request.Form["tablename"].FirstOrDefault<string>();
                string siteid = Context.Request.Form["siteid"].FirstOrDefault<string>();
                string cus_xsmc = Context.Request.Form["cus_xsmc"].FirstOrDefault<string>().Trim();
                string cus_sjlx = Context.Request.Form["cus_sjlx"].FirstOrDefault<string>().Trim();
                string cus_sjbg = Context.Request.Form["cus_sjbg"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_sjbg"].FirstOrDefault<string>().Trim();
                int cus_pxh = int.Parse(Context.Request.Form["cus_pxh"].FirstOrDefault<string>().Trim());
                int cus_isshow = int.Parse(Context.Request.Form["cus_isshow"].FirstOrDefault<string>().Trim());
                string cus_zdmc = Context.Request.Form["cus_zdmc"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_zdmc"].FirstOrDefault<string>().Trim();
                //int cus_xtsc = (Context.Request.Form["cus_xtsc"].FirstOrDefault<string>() != null && Context.Request.Form["cus_xtsc"].FirstOrDefault<string>().Equals("1")) ? 1 : 0;
                int cus_sjcd = (Context.Request.Form["cus_sjcd"].FirstOrDefault<string>() == null || Context.Request.Form["cus_sjcd"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_sjcd"].FirstOrDefault<string>().Trim());
                int cus_xsws = (Context.Request.Form["cus_xsws"].FirstOrDefault<string>() == null || Context.Request.Form["cus_xsws"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_xsws"].FirstOrDefault<string>().Trim());
                int cus_kwnull = (Context.Request.Form["cus_kwnull"].FirstOrDefault<string>() == null || Context.Request.Form["cus_kwnull"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_kwnull"].FirstOrDefault<string>());
                string cus_mrz = Context.Request.Form["cus_mrz"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_mrz"].FirstOrDefault<string>().Trim();
                int cus_srlx = (Context.Request.Form["cus_srlx"].FirstOrDefault<string>() == null || Context.Request.Form["cus_srlx"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_srlx"].FirstOrDefault<string>());
                int cus_bt = (Context.Request.Form["cus_bt"].FirstOrDefault<string>() == null || Context.Request.Form["cus_bt"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_bt"].FirstOrDefault<string>());
                string cus_wd = Context.Request.Form["cus_wd"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_wd"].FirstOrDefault<string>().Trim();
                string cus_gd = Context.Request.Form["cus_gd"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_gd"].FirstOrDefault<string>().Trim();
                int cus_xldx = (Context.Request.Form["cus_xldx"].FirstOrDefault<string>() == null || Context.Request.Form["cus_xldx"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_xldx"].FirstOrDefault<string>());
                int cus_dxhs = (Context.Request.Form["cus_dxhs"].FirstOrDefault<string>() == null || Context.Request.Form["cus_dxhs"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_dxhs"].FirstOrDefault<string>());
                string cus_kxx = Context.Request.Form["cus_kxx"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_kxx"].FirstOrDefault<string>().Trim();
                string cus_yxx = Context.Request.Form["cus_yxx"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_yxx"].FirstOrDefault<string>().Trim();
                int cus_dtxz = (Context.Request.Form["cus_dtxz"].FirstOrDefault<string>() == null || Context.Request.Form["cus_dtxz"].FirstOrDefault<string>().Equals("")) ? 0 : int.Parse(Context.Request.Form["cus_dtxz"].FirstOrDefault<string>());
                string cus_dtxzcap = Context.Request.Form["cus_dtxzcap"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_dtxzcap"].FirstOrDefault<string>().Trim();
                string cus_paraname = Context.Request.Form["cus_paraname"].FirstOrDefault<string>() == null ? "" : Context.Request.Form["cus_paraname"].FirstOrDefault<string>().Trim();

                if (cus_sjbg.IndexOf("_") >= 0 || cus_zdmc.IndexOf("_") >= 0 || cus_paraname.IndexOf("_") >= 0)
                {
                    return "alert(\"数据表格、字段名称和自定义参数不能包含下划线'_'\");parent.SubmitingDn();";
                }

                string sqlnormal = null;
                sqlnormal = "update " + tablename + " set ";
                sqlnormal += "pxh=" + cus_pxh + ",";
                sqlnormal += "xsmc='" + str.D2DD(cus_xsmc) + "',";
                sqlnormal += "sjlx='" + str.D2DD(cus_sjlx) + "',";
                sqlnormal += "sjbg='" + str.D2DD(cus_sjbg) + "',";
                sqlnormal += "zdmc='" + str.D2DD(cus_zdmc) + "',";
                //sqlnormal += "xtsc=" + cus_xtsc + ",";
                sqlnormal += "sjcd=" + cus_sjcd + ",";
                sqlnormal += "xsws=" + cus_xsws + ",";
                sqlnormal += "kwnull=" + cus_kwnull + ",";
                sqlnormal += "mrz='" + str.D2DD(cus_mrz) + "',";
                sqlnormal += "srlx=" + cus_srlx + ",";
                sqlnormal += "bt=" + cus_bt + ",";
                sqlnormal += "wd='" + str.D2DD(cus_wd) + "',";
                sqlnormal += "gd='" + str.D2DD(cus_gd) + "',";
                sqlnormal += "xldx=" + cus_xldx + ",";
                sqlnormal += "dxhs=" + cus_dxhs + ",";
                sqlnormal += "kxx='" + str.D2DD(cus_kxx) + "',";
                sqlnormal += "yxx='" + str.D2DD(cus_yxx) + "',";
                sqlnormal += "dtxz=" + cus_dtxz + ",";
                sqlnormal += "dtxzcap='" + str.D2DD(cus_dtxzcap) + "',";
                sqlnormal += "paraname='" + str.D2DD(cus_paraname) + "',";
                sqlnormal += "isshow=" + cus_isshow + ",";
                sqlnormal += "modify_time='" + str.GetDateTime() + "',";
                sqlnormal += "publish_time='" + str.GetDateTime() + "'";
                sqlnormal += " where id=" + id;

                string sqlalter = null;
                if (!cus_sjlx.Equals("line"))
                {
                    string altertable = cus_sjbg.StartsWith("@") ? cus_sjbg.Substring(1) : ("ft_" + siteid + "_f_" + cus_sjbg);
                    if (SysConst.DataBaseType.Equals(DataBase.MySql))
                    {
                        //以下更改表结构需手动完成：更改索引、主键、删除列、非常规数据类型不更改
                        sqlalter = Sql.TableStructSql(altertable);
                        DR dr = db.OpenRecord(sqlalter);
                        Hashtable curDataTable = new Hashtable();
                        while (dr.Read())
                        {
                            curDataTable.Add(dr.GetString("Field").ToLower(), new string[] { dr.GetString("Type"), dr.GetString("Null"), (dr.GetString("Default") == null ? "NULL" : dr.GetString("Default")) });
                        }
                        dr.Close();
                        string eid = null;
                        eid = cus_zdmc;
                        if (cus_srlx == 8)
                        {
                            if (!curDataTable.ContainsKey(eid))
                            {
                                return "alert(\"列名" + eid + "在指定数据表中不存在\");parent.SubmitingDn();";
                            }
                            sqlalter = null;
                        }
                        else
                        {
                            if (curDataTable.ContainsKey(eid))
                            {
                                bool IsColumnSame = false;
                                string Type = ((string[])(curDataTable[eid]))[0];
                                string Null = ((string[])(curDataTable[eid]))[1];
                                string Default = ((string[])(curDataTable[eid]))[2];
                                if (Sql.IsBaseDataType(Type))
                                {
                                    if (Type.Equals(Sql.DataTypeConvert(Sql.DataTypeConvertBase(cus_sjlx), cus_sjcd, cus_xsws)))
                                    {
                                        if ((Null.Equals("NO") && (cus_kwnull == 0)) || (Null.Equals("YES") && (cus_kwnull == 1)))
                                        {
                                            if ((Default.Equals("NULL") && cus_mrz.Equals("")) || Default.Equals(cus_mrz))
                                            {
                                                IsColumnSame = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    IsColumnSame = true;//非常规类型字段为用户自定义字段，不做变更
                                }
                                if (!IsColumnSame)
                                {
                                    sqlalter = "ALTER TABLE " + altertable + " CHANGE " + eid + " " + eid + " " + Sql.DataTypeConvert(Sql.DataTypeConvertBase(cus_sjlx), cus_sjcd, cus_xsws) + " " + ((cus_kwnull == 0) ? "NOT NULL" : "");
                                    if (!Sql.DataTypeConvertBase(cus_sjlx).Equals("text"))
                                    {
                                        if (cus_kwnull == 1)
                                        {
                                            sqlalter += " default " + (cus_mrz.Equals("") ? "NULL" : "'" + cus_mrz + "'");
                                        }
                                        else
                                        {
                                            if (!cus_mrz.Equals(""))
                                            {
                                                sqlalter += " default  '" + cus_mrz + "'";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    sqlalter = null;
                                }
                            }
                            else
                            {
                                sqlalter = "ALTER TABLE " + altertable + " ADD " + eid + " " + Sql.DataTypeConvert(Sql.DataTypeConvertBase(cus_sjlx), cus_sjcd, cus_xsws) + " " + ((cus_kwnull == 0) ? "NOT NULL" : "");
                                if (!Sql.DataTypeConvertBase(cus_sjlx).Equals("text"))
                                {
                                    sqlalter += " default " + (cus_mrz.Equals("") ? "NULL" : "'" + cus_mrz + "'");
                                }
                            }
                        }
                        curDataTable.Clear();
                        curDataTable = null;
                    }
                    else if (SysConst.DataBaseType.Equals(DataBase.SqlServer))
                    {
                        //以下更改表结构需手动完成：更改索引、主键、删除列、默认值、非常规数据类型不更改
                        sqlalter = Sql.TableStructSql(altertable);
                        DR dr = db.OpenRecord(sqlalter);
                        Hashtable curDataTable = new Hashtable();
                        while (dr.Read())
                        {
                            curDataTable.Add(dr.GetString("ColumnName").ToLower(), new string[] { dr.GetString("Type"), dr.GetStringForce("length"), dr.GetStringForce("xprec"), dr.GetStringForce("xscale"), dr.GetStringForce("isnullable"), dr.GetStringForce("cdefault") });
                        }
                        dr.Close();
                        string eid = null;
                        eid = cus_zdmc;
                        if (cus_srlx == 8)
                        {
                            if (!curDataTable.ContainsKey(eid))
                            {
                                return "alert(\"列名" + eid + "在指定数据表中不存在\");parent.SubmitingDn();";
                            }
                            sqlalter = null;
                        }
                        else
                        {
                            if (curDataTable.ContainsKey(eid))
                            {
                                string Type = ((string[])(curDataTable[eid]))[0];
                                int Length = int.Parse(((string[])(curDataTable[eid]))[1]);
                                int Xprec = int.Parse(((string[])(curDataTable[eid]))[2]);
                                int Xscale = int.Parse(((string[])(curDataTable[eid]))[3]);
                                bool IsNull = int.Parse(((string[])(curDataTable[eid]))[4]) == 1;
                                string Default = ((string[])(curDataTable[eid]))[5];
                                bool IsColumnSame = false;
                                if (Sql.IsBaseDataTypeSqlServer(Type))
                                {
                                    IsColumnSame = Sql.IsColumnSameSqlServer(Type, Length, Xprec, Xscale, IsNull, Default, cus_sjlx, cus_sjcd, cus_xsws, cus_mrz, cus_kwnull == 1);
                                }
                                else
                                {
                                    IsColumnSame = true;//非常规类型字段为用户自定义字段，不做变更
                                }
                                if (!IsColumnSame)
                                {
                                    sqlalter = "ALTER TABLE  " + altertable + " ALTER COLUMN " + eid + " " + Sql.DataTypeConvert(Sql.DataTypeConvertBase(cus_sjlx), cus_sjcd, cus_xsws) + " " + ((cus_kwnull == 0) ? "NOT NULL" : "NULL");
                                }
                                else
                                {
                                    sqlalter = null;
                                }
                            }
                            else
                            {
                                sqlalter = "ALTER TABLE " + altertable + " ADD " + eid + " " + Sql.DataTypeConvert(Sql.DataTypeConvertBase(cus_sjlx), cus_sjcd, cus_xsws) + " " + ((cus_kwnull == 0) ? "NOT NULL" : "NULL");
                                if (!cus_mrz.Equals(""))
                                {
                                    sqlalter += " default '" + cus_mrz + "'";
                                }
                            }
                        }
                        curDataTable.Clear();
                        curDataTable = null;
                    }
                }

                st = db.GetTransaction();
                log.Debug(sqlnormal, "CusCol Normal");
                db.ExecSql(sqlnormal, st);
                if (sqlalter != null)
                {
                    log.Debug(sqlalter, "CusCol Mod");
                    db.ExecSql(sqlalter, st);
                }
                st.Commit();
                s = "parent.SubmitingDn();alert('Mod Successfully!');parent.location.reload();";
                return s;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (st != null) st.Rollback();
                return "alert(\"" + ex.Message + "\");parent.SubmitingDn();";
            }
        }
        public static string GetCusColDelValue(HttpContext Context, DB db, int id)
        {
            try
            {
                string tablename = Context.Request.Form["tablename"];
                string sql = "delete from " + tablename + " where id=" + id;
                db.ExecSql(sql);
                string s = "parent.SubmitingDn();alert('Delete Successfully!');parent.location.reload();";
                return s;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "alert(\"" + ex.Message + "\");parent.SubmitingDn();";
            }
        }
        public static string GetAutoCompleteValue(HttpContext Context, DB db, string AcValue, string SiteID, int MaxRow, string Sql, string JS)
        {
            if (AcValue.Equals("")) return "";
            Sql = Tool.adv.GetSpecialBase(Context, Sql.Replace("@value@", AcValue), SiteID).Trim();
            if (Sql.Equals("")) return "alert('No Sql Defined!')";
            string restr = "";
            restr += "var datalinehtml=$(parent.AutoCompleteObj).parent().next()[0].outerHTML;";
            restr += "$(parent.AutoCompleteObj).next().html('');";
            int i = 0;
            DR dr = db.OpenRecord(Sql);
            while (dr.Read() && i < MaxRow)
            {
                i++;
                string val = dr.GetValue(0).ToString();
                restr += "var newele=document.createElement('SPAN');$(newele).html(datalinehtml);$(newele).children(':eq(0)').html(\"" + val + "\");";
                restr += "$(newele).children().show();$(newele).children().on(\"click\",function(){parent.autocompleteclose('" + val + "');" + JS + "});";
                restr += "$(parent.AutoCompleteObj).next().append(newele);";
            }
            dr.Close();
            restr += "$(parent.AutoCompleteObj).next().show();";
            return restr;
        }
        public static string GetDyValue(DB db, ArrayList al, string SiteID, bool tongbu, string _FidCol, int OpDefaultCol)
        {
            string s = "";
            foreach (string[] item in al)
            {
                string id = item[0].Trim();
                string tabletag = item[1].Trim().Replace("'", "''");
                string fid = item[2].Trim();
                bool isdy = int.Parse(item[3]) == 1;
                bool isdim = int.Parse(item[4]) == 1;
                string _sql = item[5].Trim();
                if (string.IsNullOrWhiteSpace(_sql) || _sql.StartsWith("str@code("))
                {
                    bool IsCodePara = false;
                    if (_sql.StartsWith("str@code("))
                    {
                        IsCodePara = true;
                    }
                    if (!tabletag.Equals("") && !fid.Equals(""))
                    {
                        string table = tabletag.Split('.')[0];
                        table = table.StartsWith("@") ? table.Substring(1) : ("ft_" + SiteID + "_f_" + table);
                        string FidCol = Project.Core.DBSuit.Key(table, _FidCol).KeyName;
                        if (fid.IndexOf(',') >= 0)
                        {
                            FidCol = fid.Split(',')[1];
                            fid = fid.Split(',')[0];
                        }
                        string col = tabletag.Split('.')[1];
                        string sql;
                        OpDefaultCol = 0;//DyValue的保留列_dy目前取消
                        if (OpDefaultCol == 1) sql = "select " + col + ",dydata from " + table + " where " + str.D2DD(FidCol) + "='" + str.D2DD(fid) + "'";
                        else sql = "select " + col + ",'' dydata from " + table + " where " + str.D2DD(FidCol) + "='" + str.D2DD(fid) + "'";
                        DR rdr = db.OpenRecord(sql);
                        string dydata = "";
                        bool HaveData = false;
                        if (rdr.Read())
                        {
                            string val = rdr.CommonValue(null, table, 0).val;//.GetStringForceNoNULL(0);
                            if (IsCodePara)
                            {
                                val = Interface.Code.Get(_sql.Replace("@val@", val).Substring(3));
                            }
                            s += "eleVal(\"" + id + "\",\"" + val.Replace("\"", "\\\"").Replace("\r\n", "\\r\\n").Replace("\r", "\\r").Replace("\n", "\\n") + "\");";
                            dydata = rdr.GetStringNoNULL("dydata");
                            HaveData = true;
                        }
                        rdr.Close();
                        if (isdy)
                        {
                            //包含动态行数据
                            if (("," + dydata + ",").IndexOf("," + col + ",") >= 0)
                            {
                                sql = "select evalue from " + table + "_dy where fid='" + str.D2DD(fid) + "' and eid='" + str.D2DD(col) + "' order by erate";
                                rdr = db.OpenRecord(sql);
                                int LoopI = 1;
                                while (rdr.Read())
                                {
                                    string val = rdr.GetStringForceNoNULL(0);
                                    if (IsCodePara)
                                    {
                                        val = Interface.Code.Get(_sql.Replace("@val@", val).Substring(3));
                                    }
                                    s += "eleVal(\"" + id + "\",\"" + val.Replace("\"", "\\\"").Replace("\r\n", "\\r\\n").Replace("\r", "\\r").Replace("\n", "\\n") + "\"," + LoopI + ");";
                                    HaveData = true;
                                    LoopI++;
                                }
                                rdr.Close();
                            }
                        }
                        if (!HaveData)
                        {
                            s += "eleVal(\"" + id + "\",\"" + SysConst.NoResultValue + "\");";
                        }
                    }
                }
                else if (_sql.StartsWith("@code("))
                {
                    s += "eleVal(\"" + id + "\",\"" + Interface.Code.Get(_sql).Replace("\"", "\\\"").Replace("\r\n", "\\r\\n").Replace("\r", "\\r").Replace("\n", "\\n") + "\");";
                }
                else if (_sql.StartsWith("all@code("))
                {
                    s += Interface.Code.Get(_sql.Substring(3));
                }
                else
                {
                    if (_sql.StartsWith("sql@code("))
                    {
                        _sql = Interface.Code.Get(_sql.Substring(3));
                    }
                    _sql = Sql.GetSqlForRemoveSameCols(_sql);
                    DR rdr = db.OpenRecord(_sql);
                    if (!isdim)
                    {
                        string[] ids = id.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (!isdy)
                        {
                            if (rdr.Read())
                            {
                                for (int i = 0; i < ids.Length; i++)
                                {
                                    s += "eleVal(\"" + ids[i] + "\",\"" + rdr.CommonValue(null, null, i).val.Replace("\"", "\\\"").Replace("\r\n", "\\r\\n").Replace("\r", "\\r").Replace("\n", "\\n") + "\");";
                                }
                            }
                            else
                            {
                                for (int i = 0; i < ids.Length; i++)
                                {
                                    s += "eleVal(\"" + ids[i] + "\",\"" + SysConst.NoResultValue + "\");";
                                }
                            }
                            rdr.Close();
                        }
                        else
                        {
                            int LoopI = 0;
                            StringBuilder sb = new StringBuilder(256);
                            while (rdr.Read())
                            {
                                for (int i = 0; i < ids.Length; i++)
                                {
                                    sb.Append("eleVal(\"" + ids[i] + "\",\"" + rdr.CommonValue(null, null, i).val.Replace("\"", "\\\"").Replace("\r\n", "\\r\\n").Replace("\r", "\\r").Replace("\n", "\\n") + "\"," + LoopI + ");");
                                }
                                LoopI++;
                            }
                            rdr.Close();
                            if (LoopI == 0)
                            {
                                for (int i = 0; i < ids.Length; i++)
                                {
                                    sb.Append("eleVal(\"" + ids[i] + "\",\"" + SysConst.NoResultValue + "\");");
                                }
                            }
                            s += sb.ToString();
                            sb.Clear();
                            sb = null;
                        }
                    }
                    else
                    {
                        string ids = "";
                        string names = "";
                        int loopi = 0;
                        while (rdr.Read())
                        {
                            loopi++;
                            if (loopi > 1)
                            {
                                ids += "##";
                                names += "##";
                            }
                            ids += rdr.CommonValue(null, null, 0).val;
                            names += rdr.CommonValue(null, null, 1).val;
                        }
                        rdr.Close();
                        s += "eleDim(\"" + id + "\",\"" + ids + "\",\"" + names + "\");";
                    }
                }
            }
            return tongbu ? s : Tool.func.escape(s);
        }
        public static string ConditionReturn(string sql, string code, string sqlparas, HttpContext Context)//返回0或null为通过，返回1为默认错误信息，其他为自定义错误信息
        {
            sql = sql.Trim();
            code = code.Trim();
            if (sql.Equals("") && code.Equals("")) return null;

            if (!sql.Equals(""))
            {
                DB db = new DB();
                db.Open();
                try
                {
                    if (sqlparas != null)
                    {
                        string[] sqlparaitem = sqlparas.Split(new string[] { "##" }, StringSplitOptions.None);
                        int IndexI = 0;
                        Regex r = new Regex(@"{[^}]*}");
                        MatchCollection mc = r.Matches(sql);
                        foreach (Match m in mc)
                        {
                            if (sqlparaitem.Length > IndexI)
                            {
                                sql = sql.Replace(m.Value, str.D2DD(sqlparaitem[IndexI]));
                            }
                            IndexI++;
                        }
                    }
                    DR dr = db.OpenRecord(sql);
                    if (dr.Read())
                    {
                        if (int.Parse(dr.GetValue(0).ToString()) != 0)
                        {
                            dr.Close();
                            return "1";
                        }
                        else
                        {
                            dr.Close();
                        }
                    }
                    else
                    {
                        dr.Close();
                        return "1";
                    }
                }
                catch (Exception ex)
                {
                    FTFrame.Project.Core.Api.LogError(ex);
                    return ex.Message;
                }
                finally
                {
                    db.Close();
                }
            }
            if (!code.Equals(""))
            {
                if (code.StartsWith("@code("))
                {
                    string[] codeitem = code.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string codei in codeitem)
                    {
                        string cdnreturn = Interface.Code.Get(codei, Context);
                        if (cdnreturn != null && !cdnreturn.Equals("0")) return cdnreturn;
                    }
                }
            }
            return null;
        }
        public static void FormSqlExec(DB db, ST st, string SqlBase, string SqlEvals, string newfid, HttpContext Context)
        {
            if (SqlBase == null || SqlEvals == null) return;
            SqlBase = SqlBase.Trim();
            if (SqlBase.Equals("")) return;
            string[] sqlparaitem = SqlEvals.Split(new string[] { "##" }, StringSplitOptions.None);
            int IndexI = 0;
            Regex r = new Regex(@"{[^}]*}");
            MatchCollection mc = r.Matches(SqlBase);
            foreach (Match m in mc)
            {
                if (sqlparaitem.Length > IndexI)
                {
                    SqlBase = SqlBase.Replace(m.Value, str.D2DD(sqlparaitem[IndexI]));
                }
                IndexI++;
            }
            string[] Sqls = SqlBase.Trim().Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string sql in Sqls)
            {
                string _sql = sql.Replace("@newfid@", newfid).Replace("_newfid_", newfid);
                log.Debug(_sql, "[Sql Exec]");
                db.ExecSql(_sql, st);
            }
        }
    }
    public class JsCss
    {
        public static void GetAppend(HttpContext context)
        {
            string s = "";
            //通过模板去定义 /_pro/template/HEAD.ascx
            //s += "<link href=\"/_ftres/js/jquery-ui-1.10.3.custom.min.css\" rel=\"stylesheet\" type=\"text/css\" />";
            //s += "<script type=\"text/javascript\" src=\"/_ftres/js/jquery-1.9.1.min.js\"></script>";
            //s += "<script type=\"text/javascript\" src=\"/_ftres/js/jquery-ui-1.10.3.custom.min.js\"></script>";
            //s += "<script type=\"text/javascript\" src=\"/_ftres/js/ftmain.js\"></script>";
            //s += "<script type=\"text/javascript\" src=\"/_ftres/swfobject.js\"></script>";
            context.Response.WriteAsync(s);
        }
    }
    public class AccessControl
    {
        public static bool Accessed(HttpContext context, int AccessIPControl, string AccessIPCondition, int AccessIPConditionSide, int AccessSessionControl, string AccessSessionCondition, int AccessSessionConditionSide, int AccessJumpControl, string AccessJumpAddress, int AccessTipControl, string AccessTipContent)
        {
            bool CanAccess = true;
            if (AccessSessionControl == 1 && CanAccess)
            {
                string[] Conditions = AccessSessionCondition.Split(';');
                bool NormalSide = (AccessSessionConditionSide == 0);
                foreach (string Condition in Conditions)
                {
                    if (!CanAccess) break;
                    string _Condition = Condition;
                    if (_Condition.IndexOf('=') > 0)
                    {
                        if (_Condition.IndexOf('!') < 0)
                        {
                            CanAccess = CanAccess && (NormalSide == (session.Get(_Condition.Split('=')[0]) == null ? "" : session.Get(_Condition.Split('=')[0]).ToString()).Equals(_Condition.Split('=')[1]));
                        }
                        else
                        {
                            _Condition = _Condition.Replace("!", "");
                            CanAccess = CanAccess && (NormalSide == !(session.Get(_Condition.Split('=')[0]) == null ? "" : session.Get(_Condition.Split('=')[0]).ToString()).Equals(_Condition.Split('=')[1]));
                        }
                    }
                }
            }
            if (AccessIPControl == 1 && CanAccess)
            {
                string[] Conditions = AccessIPCondition.Split(';');
                bool NormalSide = (AccessIPConditionSide == 0);
                string RemoteIP = str.GetIP();
                foreach (string Condition in Conditions)
                {
                    if (!CanAccess) break;
                    string _Condition = Condition;
                    if (_Condition.IndexOf('=') > 0)
                    {
                        if (_Condition.IndexOf('-') < 0)
                        {
                            CanAccess = CanAccess && (NormalSide == (_Condition.Equals(RemoteIP)));
                        }
                        else
                        {
                            string[] IPS = _Condition.Split('-');
                            string[] IP1 = IPS[0].Split('.');
                            string[] IP2 = IPS[1].Split('.');
                            string ip1 = "";
                            string ip2 = "";
                            for (int i = 0; i < IP1.Length; i++)
                            {
                                if (IP1[i].Length == 1)
                                {
                                    IP1[i] = "00" + IP1[i];
                                }
                                else if (IP1[i].Length == 2)
                                {
                                    IP1[i] = "0" + IP1[i];
                                }
                                ip1 += IP1[i];
                            }
                            for (int i = 0; i < IP2.Length; i++)
                            {
                                if (IP2[i].Length == 1)
                                {
                                    IP2[i] = "00" + IP2[i];
                                }
                                else if (IP2[i].Length == 2)
                                {
                                    IP2[i] = "0" + IP2[i];
                                }
                                ip2 += IP2[i];
                            }

                            CanAccess = CanAccess && (NormalSide == (string.Compare(RemoteIP, ip1) >= 0 && string.Compare(ip2, RemoteIP) >= 0));
                        }
                    }
                }
            }
            return CanAccess;
        }
        public static string AccesseInfo(int AccessJumpControl, string AccessJumpAddress, int AccessTipControl, string AccessTipContent)
        {
            string info = "";
            if (AccessTipControl == 1)
            {
                if (AccessTipContent.StartsWith("@"))
                    info += "<script language='javascript'>" + AccessTipContent.Substring(1) + "</script>";
                else
                    info += "<script language='javascript'>alert('" + AccessTipContent + "');</script>";
            }
            if (AccessJumpControl == 1)
            {
                info += "<script language='javascript'>location.href='" + AccessJumpAddress + "';</script>";
            }
            return info;
        }
    }
}
