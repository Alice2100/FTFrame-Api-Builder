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
using FTFrame.Server.Core.Model;
using FTFrame.Project.Core.Utils;

namespace FTFrame.Server.Core
{
    public class Form
    {
        public static void Post(HttpContext Context)
        {
            if (!Interface.Auth.HostReferOK(Context)) return;
            string ftformtype = Context.Request.Query["ftformtype"];
            if ((ftformtype != null && ftformtype.Equals("dataop")))
            {
                DataOP(Context);
                return;
            }
        }
        private static string PostStrSafe(string oriStr, string key)
        {
            if (key.EndsWith("_ishtml") || oriStr == null) return oriStr;
            else return FTFrame.Tool.str.GetSafeCode(oriStr);
        }
        private static void DataOP(HttpContext Context)
        {
            HttpResponse Response = Context.Response;
            HttpRequest Request = Context.Request;
            #region Init
            string RightCheck = Interface.Right.DataOP(Context);
            if (RightCheck != null)
            {
                Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('" + RightCheck + "!');"));
                return;
            }
            string memid = UserTool.CurUserID();
            string PartID = Request.Query["partid"];
            string opid = Str.Decode(Context.Request.Form["ftdataop_" + PartID + "_opid"]);
            if (!Interface.Right.HaveOPRight(opid))
            {
                Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('你没有操作该项的权限');"));
                return;
            }
            int datatype = int.Parse(Str.Decode(Context.Request.Form["ftdataop_" + PartID + "_datatype"]));
            string siteid = Context.Request.Form["ftdataop_" + PartID + "_siteid"].FirstOrDefault<string>().Replace("'", "''");
            string FidCol = Str.Decode(Context.Request.Form["ftdataop_" + PartID + "_fidcol"]);
            int OpDefaultCol = int.Parse(Context.Request.Form["ftdataop_" + PartID + "_defaultcols"]);
            string jssuc = Context.Request.Form["ftdataop_" + PartID + "_jssuc"];
            string define = Context.Request.Form["ftdataop_" + PartID + "_define"];
            string tabletag = Str.Decode(Context.Request.Form["ftdataop_" + PartID + "_tabletag"]).Replace("'", "''");
            string defaultfid = Context.Request.Form["ftdataop_" + PartID + "_defaultfid"];
            int flowtype = int.Parse(Str.Decode(Context.Request.Form["ftdataop_" + PartID + "_flowtype"]));
            int flowstat = int.Parse(Str.Decode(Context.Request.Form["ftdataop_" + PartID + "_flowstat"]));
            string flowdesign = Str.Decode(Context.Request.Form["ftdataop_" + PartID + "_flowdesign"]);
            string flowdesignbaranch = Str.Decode(Context.Request.Form["ftdataop_" + PartID + "_flowdesignbaranch"]);
            string flowdesignpos = Str.Decode(Context.Request.Form["ftdataop_" + PartID + "_flowdesignpos"]);
            string codebefore = Context.Request.Form["ftdataop_" + PartID + "_codebefore"];
            string codeafter = Context.Request.Form["ftdataop_" + PartID + "_codeafter"];
            string cdnsql = Str.Decode(Context.Request.Form["ftdataop_" + PartID + "_cdnsql"]);
            string cdnsqlevals = Context.Request.Form["ftdataop_" + PartID + "_cdnsqlevals"];
            string cdncode = Context.Request.Form["ftdataop_" + PartID + "_cdncode"];
            string cdnjs = Context.Request.Form["ftdataop_" + PartID + "_cdnjs"];
            bool IsMultiMod = int.Parse(Context.Request.Form["ftdataop_" + PartID + "_ismultimod"]) == 1;
            string MultiFidName = Context.Request.Form["ftdataop_" + PartID + "_multifidname"];
            string MultiCondition = Str.Decode(Context.Request.Form["ftdataop_" + PartID + "_multicdn"]);
            string MultiCdnEvals = Context.Request.Form["ftdataop_" + PartID + "_multicdnevals"];
            string execsqlbefore = Str.Decode(Context.Request.Form["ftdataop_" + PartID + "_execsqlbefore"]);
            string execsqlbeforeevals = Context.Request.Form["ftdataop_" + PartID + "_execsqlbeforeevals"];
            string execsqlafter = Str.Decode(Context.Request.Form["ftdataop_" + PartID + "_execsqlafter"]);
            string execsqlafterevals = Context.Request.Form["ftdataop_" + PartID + "_execsqlafterevals"];
            string advsetevals = Context.Request.Form["ftdataop_" + PartID + "_advsetevals"];
            if (advsetevals == null) advsetevals = "";
            string[] advsetevalsrows = advsetevals.Split(new string[] { "|||" }, StringSplitOptions.None);
            string tableflog = "ft_ftdp_formflog";// "ft_" + siteid + "_formflog";
            string tableolog = "ft_ftdp_formolog";// "ft_" + siteid + "_formolog";
            string FidCol2 = "";
            if (FidCol.IndexOf(',') > 0)
            {
                FidCol2 = FidCol.Split(',')[1];
                FidCol = FidCol.Split(',')[0];
            }
            codebefore = DataOPRootValue(codebefore,Context.Request);
            codeafter = DataOPRootValue(codeafter, Context.Request);
            cdnsql = DataOPRootValue(cdnsql, Context.Request);
            cdncode = DataOPRootValue(cdncode, Context.Request);
            execsqlbefore = DataOPRootValue(execsqlbefore, Context.Request);
            execsqlafter = DataOPRootValue(execsqlafter, Context.Request);
            Dictionary<string, string> JsonkeyValue = new Dictionary<string, string>();
            Dictionary<string, string> keyValue = new Dictionary<string, string>();
            List<DPData> DataList = new List<DPData>();
            /* 页面操作暂不支持Json ，以下有Bug
            string[] defineitemsJ = define.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < defineitemsJ.Length; i++)
            {
                string defineitem = defineitemsJ[i];
                string[] defineevalrowsitem = new string[0];
                if (advsetevalsrows.Length > i)
                {
                    defineevalrowsitem = advsetevalsrows[i].Split(new string[] { "##" }, StringSplitOptions.None);
                }
                string[] item = defineitem.Split(new char[] { '#' }, StringSplitOptions.None);
                string name = item[0].Trim();
                string Jsontabletag = str.GetDecode(item[1].Trim());
                string optype = str.GetDecode(item[2].Trim());
                string advset = "";
                if (item.Length >= 5) advset = item[4];
                if (advset != "") advset = str.GetDecode(advset);
                #region 处理Json
                string json = null;
                if (item.Length >= 9) json = item[8].Trim();
                int leixing = int.Parse(optype);
                List<string> FileKeys = Page.IFormFileKeys(Context.Request);
                string jsonOpResult = Api.DataOPKeyValueJson(false,Context,null, json, DataList, name, leixing, tabletag, advset, FileKeys, new string[0]);
                if (jsonOpResult != null)
                {
                    keyValue.Clear();
                    Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('"+ jsonOpResult+"');"));
                    return;
                }
                #endregion
            }*/
            #region 操作条件
            //支持##多条件和多条件返回 Sql多余一列则第二列为返回的cdnjs
            string cdnreturn = FTFrame.Server.Core.Page.ConditionReturn(cdnsql, cdncode, cdnsqlevals, Context, cdnjs, out string rtnCdnjs);
            if (cdnreturn != null && !cdnreturn.Equals("0"))
            {
                if (cdnreturn.Equals("1"))
                {
                    Response.WriteAsync(str.JavascriptLabel(rtnCdnjs + ";"));
                    return;
                }
                else
                {
                    Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('" + rtnCdnjs + "',null);"));
                    return;
                }
            }
            #endregion
            #region 流水号初始化
            ArrayList LiquidAL = new ArrayList();
            foreach (string liqkey in Request.Form.Keys)
            {
                if (liqkey.StartsWith("ftform_liquid"))
                {
                    string liquid_all = Request.Form[liqkey];
                    if (liquid_all != null && !liquid_all.Equals(""))
                    {
                        string[] liquid_alls = liquid_all.Split(new string[] { "&&" }, StringSplitOptions.None);
                        string liquid_patten = liquid_alls[0];
                        string liquid_table = liquid_alls[1];
                        string liquid_col = liquid_alls[2];
                        string liquid_locklike = liquid_alls[3];
                        if (liquid_table.StartsWith("@")) liquid_table = liquid_table.Substring(1);
                        else
                            liquid_table = "ft_" + siteid + "_f_" + liquid_table;
                        LiquidAL.Add(new string[] {
                                liquid_table,liquid_col,liquid_patten,liquid_locklike
                            });
                    }
                }
            }
            #endregion
            string BindTable = null;
            if (tabletag.StartsWith("@")) BindTable = tabletag.Substring(1);
            else
                BindTable = "ft_" + siteid + "_f_" + tabletag;
            string FlowActionResult = null;
            string _newfid_ = "";
            #endregion
            DB db = new DB();
            db.Open();
            ST st = db.GetTransaction();
            try
            {
                ///执行操作前SQL
                FTFrame.Server.Core.Fore.FormSqlExec(db, st, execsqlbefore, execsqlbeforeevals, "", Context);

                #region DataOP
                if (datatype == 0 || datatype == 2)//add,mix
                {
                    int flow = 0;//默认流程状态
                    if (datatype == 2) flow = flowstat;
                    Hashtable TableAdd = new Hashtable();//Add的信息存储
                    Hashtable TableMod = new Hashtable();//Mod的信息存储
                    Hashtable TableAddDy = new Hashtable();//Add的动态新增信息存储
                    Hashtable TableModDy = new Hashtable();//Mod的动态新增信息存储
                    ArrayList StayedPostedFiles = new ArrayList();//保留上传文件的存储
                    Hashtable TableAddFid = new Hashtable();//Add的Fid存储
                    string FirstAddFid = null;//str.GetCombID();
                    Dictionary<string, Dictionary<int, Dictionary<string, string>>> TableMultiCol = new Dictionary<string, Dictionary<int, Dictionary<string, string>>>();//多行存储的表、RowRate、列、数据
                    Dictionary<string, object[]> TableMuiltiSet = new Dictionary<string, object[]>();//多行存储的表、配置(类型、存储Fid元素Name、过滤条件)，通过fid配置实现

                    #region base
                    ArrayList DataOP_FileKey = new ArrayList();
                    Hashtable DataOP_Names = new Hashtable();
                    Dictionary<string, string> RootKeyValue = new Dictionary<string, string>();
                    Dictionary<string, bool> ModeAutoRecordDIc = new Dictionary<string, bool>();
                    ///数据操作相关的配置，值中数组内容为绑定表列、操作类型、fid设置、高级设置
                    ///除了多行的fid值特殊外，其他的值获取adv设置高于name值
                    #region 初始化 DataOP_Names 
                    string[] defineitems = define.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < defineitems.Length; i++)
                    {
                        string defineitem = defineitems[i];
                        string[] defineevalrowsitem = new string[0];
                        if (advsetevalsrows.Length > i)
                        {
                            defineevalrowsitem = advsetevalsrows[i].Split(new string[] { "##" }, StringSplitOptions.None);
                        }
                        string[] item = defineitem.Split(new char[] { '#' }, StringSplitOptions.None);
                        string dataop_name = item[0];
                        string dataop_tabletag = str.GetDecode(item[1]);
                        if (dataop_tabletag.Trim() != "")//数据绑定的设置为空，则不处理，后续支持@code 2019/11/14
                        {
                            if (FirstAddFid == null) FirstAddFid = DBSuit.KeyGenerate(Page.TableNameByTableTag(dataop_tabletag)).ToString();
                            int dataop_type = int.Parse(str.GetDecode(item[2]));
                            string dataop_fid = item[3];
                            dataop_fid = DataOPRootValue(dataop_fid, Context.Request,true);
                            if (dataop_fid == "_newfid_" || dataop_fid == "@newfid@") dataop_fid = FirstAddFid;//强制绑定同一个fid
                            string advset = "";
                            if (item.Length >= 5) advset = item[4];
                            if (advset != "") advset = str.GetDecode(advset);
                            advset = DataOPRootValue(advset, Context.Request);
                            int IndexI = 0;
                            Regex r = new Regex(@"{[^}]*}");
                            MatchCollection mc = r.Matches(advset);
                            foreach (Match m in mc)
                            {
                                if (defineevalrowsitem.Length > IndexI)
                                {
                                    advset = advset.Replace(m.Value, str.D2DD(defineevalrowsitem[IndexI]));
                                }
                                IndexI++;
                            }
                            if (dataop_name != "")
                            {
                                if (!DataOP_Names.ContainsKey(dataop_name))
                                {
                                    DataOP_Names.Add(dataop_name, new string[] { dataop_tabletag, dataop_type.ToString(), dataop_fid, advset.Trim() });
                                }
                            }
                            else if (advset != "" || (dataop_type >= 3 && dataop_type <= 6 && dataop_tabletag.ToLower().EndsWith("." + DBSuit.Key(Page.TableNameByTableTag(dataop_tabletag), FidCol2).KeyName.ToLower())))
                            {
                                DataOP_Names.Add("adv:" + i, new string[] { dataop_tabletag, dataop_type.ToString(), dataop_fid, advset.Trim() });
                            }
                        }
                    }
                    #endregion
                    ArrayList OPedKey = new ArrayList();
                    #region 处理文件上传
                    foreach (string key in Page.IFormFileKeys(Request))
                    {
                        if (!OPedKey.Contains(key)) OPedKey.Add(key);
                        string revalue = null;
                        if (DataOP_Names.ContainsKey(key))
                        {
                            DataOP_FileKey.Add(key);
                            if (key.IndexOf("_rowrate") < 0)
                            {
                                string[] dataop_para = (string[])DataOP_Names[key];
                                string dataop_tabletag = dataop_para[0];
                                int dataop_type = int.Parse(dataop_para[1]);
                                string dataop_fid = dataop_para[2];
                                string dataop_adv = dataop_para[3];
                                string dataop_table = dataop_tabletag.Split('.')[0];
                                dataop_table = dataop_table.StartsWith("@") ? dataop_table.Substring(1) : ("ft_" + siteid + "_f_" + dataop_table);
                                string dataop_col = dataop_tabletag.Split('.')[1];
                                string dataop_table_dy = dataop_table + "_dy";

                                if (dataop_adv == "")
                                    revalue = adv.UploadFile(Context, key);
                                else revalue = Ajax.Control.DataOPAdvVal(Context, db, st, dataop_adv, FirstAddFid, 0,TableAdd, TableMod);

                                //选中删除要删除的文件，才删除
                                if ((string.IsNullOrEmpty(Context.Request.Form["filedel_" + key].FirstOrDefault<string>()) || !Context.Request.Form["filedel_" + key].FirstOrDefault<string>().Equals("1")))
                                {
                                    if (!StayedPostedFiles.Contains(dataop_table + "." + dataop_col)) StayedPostedFiles.Add(dataop_table + "." + dataop_col);
                                }

                                if ((Context.Request.Form["delfile_" + key].FirstOrDefault<string>() != null && Context.Request.Form["delfile_" + key].FirstOrDefault<string>().Equals("1")))
                                {
                                    revalue = "(delfile)";
                                }

                                if (revalue == null) revalue = "";

                                bool IsLockValue = key.StartsWith("_lock_");
                                string LockValue = null;
                                if (IsLockValue) LockValue = revalue;

                                //处理Auto
                                if (dataop_type == 13)//Auto
                                {
                                    string autofid = defaultfid;
                                    if (!dataop_fid.Equals(""))
                                    {
                                        autofid = dataop_fid;
                                    }
                                    string CurFidCol = DBSuit.Key(dataop_table, FidCol).KeyName;
                                    string CusWhere = null;
                                    bool noRecord = true;
                                    if (ModeAutoRecordDIc.ContainsKey(dataop_table + "|" + autofid))
                                    {
                                        noRecord = ModeAutoRecordDIc[dataop_table + "|" + autofid];
                                    }
                                    else
                                    {
                                        if (autofid.Trim().StartsWith("where ", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            CusWhere = autofid.Trim();
                                        }
                                        else if (autofid.IndexOf(',') >= 0)
                                        {
                                            CurFidCol = autofid.Split(',')[1];
                                            autofid = autofid.Split(',')[0];
                                        }
                                        string sql = "select count(*) as ca from " + dataop_table.D2();
                                        if (CusWhere != null) sql += " " + CusWhere;
                                        else
                                        {
                                            sql += " where " + str.D2DD(CurFidCol) + "='" + str.D2DD(autofid) + "'";
                                        }
                                        using (DR dr = db.OpenRecord(sql, st))
                                        {
                                            if (dr.Read())
                                            {
                                                if (dr.GetInt32(0) > 0) noRecord = false;
                                            }
                                        }
                                        ModeAutoRecordDIc.Add(dataop_table + "|" + autofid, noRecord);
                                    }
                                    if (noRecord)
                                    {
                                        dataop_type = 0;
                                        dataop_fid = "";
                                    }
                                    else
                                    {
                                        dataop_type = 1;
                                    }
                                }
                                if (dataop_type == 0)//Add
                                {
                                    if (!TableAdd.ContainsKey(dataop_table))
                                    {
                                        TableAdd.Add(dataop_table, new Hashtable());
                                        if (!dataop_fid.Equals(""))
                                        {
                                            TableAddFid.Add(dataop_table, dataop_fid);
                                        }
                                        else
                                        {
                                            if (TableAddFid.Count == 0) TableAddFid.Add(dataop_table, FirstAddFid);
                                            else TableAddFid.Add(dataop_table, DBSuit.KeyGenerate(dataop_table).ToString());
                                        }
                                    }
                                    if (!((Hashtable)TableAdd[dataop_table]).Contains(dataop_col))
                                    {
                                        ((Hashtable)TableAdd[dataop_table]).Add(dataop_col, revalue.Equals("(delfile)") ? "" : revalue);
                                    }
                                    int rateindex = 1;
                                    IFormFile pfile = Context.Request.Form.Files[key + "_rowrate" + rateindex];
                                    int FindDeep = 0;
                                    int SaveRateIndex = 1;
                                    while (pfile == null && FindDeep < SysConst.RateRowFindDeep)
                                    {
                                        FindDeep++;
                                        rateindex++;
                                        pfile = Context.Request.Form.Files[key + "_rowrate" + rateindex];
                                    }
                                    while (pfile != null)
                                    {
                                        if ((Context.Request.Form["delfile_" + key + "_rowrate" + rateindex].FirstOrDefault<string>() != null && Context.Request.Form["delfile_" + key + "_rowrate" + rateindex].FirstOrDefault<string>().Equals("1")))
                                        {
                                            revalue = "";
                                        }
                                        else
                                        {
                                            if (dataop_adv == "")
                                                revalue = adv.UploadFile(Context, key + "_rowrate" + rateindex);
                                            else revalue = Ajax.Control.DataOPAdvVal(Context, db, st, dataop_adv, FirstAddFid, SaveRateIndex, TableAdd, TableMod);
                                        }
                                        if (!TableAddDy.Contains(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString() }))
                                        {
                                            TableAddDy.Add(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString() }, IsLockValue ? LockValue : revalue);
                                        }
                                        SaveRateIndex++;
                                        rateindex++;
                                        pfile = Context.Request.Form.Files[key + "_rowrate" + rateindex];
                                        FindDeep = 0;
                                        while (pfile == null && FindDeep < SysConst.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;
                                            pfile = Context.Request.Form.Files[key + "_rowrate" + rateindex];
                                        }
                                    }
                                }
                                else if (dataop_type == 1)//Mod
                                {
                                    string elefid = defaultfid;
                                    if (!dataop_fid.Equals(""))
                                    {
                                        elefid = dataop_fid;
                                    }
                                    if (elefid == null || elefid.Equals("")) continue;
                                    if (!TableMod.ContainsKey(dataop_table))
                                    {
                                        TableMod.Add(dataop_table, new Hashtable());
                                    }
                                    if (!((Hashtable)TableMod[dataop_table]).Contains(elefid))
                                    {
                                        ((Hashtable)TableMod[dataop_table]).Add(elefid, new Hashtable());
                                    }
                                    if (!((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).Contains(dataop_col))
                                    {
                                        ((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).Add(dataop_col, revalue);
                                    }
                                    int rateindex = 1;
                                    IFormFile pfile = Context.Request.Form.Files[key + "_rowrate" + rateindex];
                                    int FindDeep = 0;
                                    int SaveRateIndex = 1;
                                    while (pfile == null && FindDeep < SysConst.RateRowFindDeep)
                                    {
                                        FindDeep++;
                                        rateindex++;
                                        pfile = Context.Request.Form.Files[key + "_rowrate" + rateindex];
                                    }
                                    while (pfile != null)
                                    {
                                        if ((Context.Request.Form["delfile_" + key + "_rowrate" + rateindex].FirstOrDefault<string>() != null && Context.Request.Form["delfile_" + key + "_rowrate" + rateindex].FirstOrDefault<string>().Equals("1")))
                                        {
                                            revalue = "(delfile)";
                                        }
                                        else
                                        {
                                            if (dataop_adv == "")
                                                revalue = adv.UploadFile(Context, key + "_rowrate" + rateindex);
                                            else revalue = Ajax.Control.DataOPAdvVal(Context, db, st, dataop_adv, FirstAddFid, SaveRateIndex, TableAdd, TableMod);
                                        }
                                        if (!TableModDy.Contains(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString(), elefid }))
                                        {
                                            TableModDy.Add(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString(), elefid }, new string[] { IsLockValue ? LockValue : revalue, "FileBox" });
                                        }
                                        SaveRateIndex++;
                                        rateindex++;
                                        pfile = Context.Request.Form.Files[key + "_rowrate" + rateindex];
                                        FindDeep = 0;
                                        while (pfile == null && FindDeep < SysConst.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;
                                            pfile = Context.Request.Form.Files[key + "_rowrate" + rateindex];
                                        }
                                    }
                                }
                                else if (dataop_type >= 2 && dataop_type <= 6)//多行 FID设置认为不会在file上传中出现
                                {
                                    if (!TableMultiCol.ContainsKey(dataop_table))
                                    {
                                        TableMultiCol.Add(dataop_table, new Dictionary<int, Dictionary<string, string>>());
                                    }
                                    if (!TableMultiCol[dataop_table].ContainsKey(0))
                                    {
                                        TableMultiCol[dataop_table].Add(0, new Dictionary<string, string>());
                                    }
                                    if (!TableMultiCol[dataop_table][0].ContainsKey(dataop_col))
                                    {
                                        TableMultiCol[dataop_table][0].Add(dataop_col, revalue.Equals("(delfile)") ? "" : revalue);
                                    }
                                    int rateindex = 1;
                                    IFormFile pfile = Context.Request.Form.Files[key + "_rowrate" + rateindex];
                                    int FindDeep = 0;
                                    int SaveRateIndex = 1;
                                    while (pfile == null && FindDeep < SysConst.RateRowFindDeep)
                                    {
                                        FindDeep++;
                                        rateindex++;
                                        pfile = Context.Request.Form.Files[key + "_rowrate" + rateindex];
                                    }
                                    while (pfile != null)
                                    {
                                        if ((Context.Request.Form["delfile_" + key + "_rowrate" + rateindex].FirstOrDefault<string>() != null && Context.Request.Form["delfile_" + key + "_rowrate" + rateindex].FirstOrDefault<string>().Equals("1")))
                                        {
                                            revalue = "";
                                        }
                                        else
                                        {
                                            if (dataop_adv == "")
                                                revalue = adv.UploadFile(Context, key + "_rowrate" + rateindex);
                                            else revalue = Ajax.Control.DataOPAdvVal(Context, db, st, dataop_adv, FirstAddFid, SaveRateIndex, TableAdd, TableMod);
                                        }
                                        if (!TableMultiCol[dataop_table].ContainsKey(SaveRateIndex))
                                        {
                                            TableMultiCol[dataop_table].Add(SaveRateIndex, new Dictionary<string, string>());
                                        }
                                        if (!TableMultiCol[dataop_table][SaveRateIndex].ContainsKey(dataop_col))
                                        {
                                            TableMultiCol[dataop_table][SaveRateIndex].Add(dataop_col, IsLockValue ? LockValue : revalue);
                                        }
                                        SaveRateIndex++;
                                        rateindex++;
                                        pfile = Context.Request.Form.Files[key + "_rowrate" + rateindex];
                                        FindDeep = 0;
                                        while (pfile == null && FindDeep < SysConst.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;
                                            pfile = Context.Request.Form.Files[key + "_rowrate" + rateindex];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region 处理字段
                    foreach (string key in Context.Request.Form.Keys)
                    {
                        if (!OPedKey.Contains(key)) OPedKey.Add(key);
                        string revalue = null;
                        if (DataOP_Names.ContainsKey(key) && !DataOP_FileKey.Contains(key))
                        {
                            if (key.IndexOf("_rowrate") < 0)
                            {
                                string[] dataop_para = (string[])DataOP_Names[key];
                                string dataop_tabletag = dataop_para[0];
                                int dataop_type = int.Parse(dataop_para[1]);
                                string dataop_fid = dataop_para[2];
                                string dataop_adv = dataop_para[3];
                                string dataop_table = dataop_tabletag.Split('.')[0];
                                dataop_table = dataop_table.StartsWith("@") ? dataop_table.Substring(1) : ("ft_" + siteid + "_f_" + dataop_table);
                                string dataop_col = dataop_tabletag.Split('.')[1];
                                string dataop_table_dy = dataop_table + "_dy";
                                if (key.StartsWith("_newfid_"))
                                {
                                    //该字段的值来自于指定table的新newfid
                                    string newfid_table = key.Replace("_newfid_", "");
                                    if (TableAddFid.ContainsKey(newfid_table))
                                    {
                                        revalue = TableAddFid[newfid_table].ToString();
                                    }
                                    else
                                    {
                                        revalue = "";
                                    }
                                }
                                else
                                {
                                    if (dataop_adv == "")
                                        revalue = PostStrSafe(Context.Request.Form[key].FirstOrDefault<string>(), key);
                                    else revalue = Ajax.Control.DataOPAdvVal(Context, db, st, dataop_adv, FirstAddFid, 0, TableAdd, TableMod);
                                }
                                if (revalue == null) revalue = "";

                                if (revalue.StartsWith("@code("))
                                {
                                    revalue = Interface.Code.Get(adv.GetSpecialBase(Context, revalue, siteid), Context);
                                }
                                if (!RootKeyValue.ContainsKey(key.ToLower().Trim())) RootKeyValue.Add(key.ToLower().Trim(), revalue);
                                bool IsLockValue = key.StartsWith("_lock_");
                                string LockValue = null;
                                if (IsLockValue) LockValue = revalue;

                                //处理Auto
                                if (dataop_type == 13)//Auto
                                {
                                    string autofid = defaultfid;
                                    if (!dataop_fid.Equals(""))
                                    {
                                        autofid = dataop_fid;
                                    }
                                    string CurFidCol = DBSuit.Key(dataop_table, FidCol).KeyName;
                                    string CusWhere = null;
                                    bool noRecord = true;
                                    if (ModeAutoRecordDIc.ContainsKey(dataop_table + "|" + autofid))
                                    {
                                        noRecord = ModeAutoRecordDIc[dataop_table + "|" + autofid];
                                    }
                                    else
                                    {
                                        if (autofid.Trim().StartsWith("where ", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            CusWhere = autofid.Trim();
                                        }
                                        else if (autofid.IndexOf(',') >= 0)
                                        {
                                            CurFidCol = autofid.Split(',')[1];
                                            autofid = autofid.Split(',')[0];
                                        }
                                        string sql = "select count(*) as ca from " + dataop_table.D2();
                                        if (CusWhere != null) sql += " " + CusWhere;
                                        else
                                        {
                                            sql += " where " + str.D2DD(CurFidCol) + "='" + str.D2DD(autofid) + "'";
                                        }
                                        using (DR dr = db.OpenRecord(sql, st))
                                        {
                                            if (dr.Read())
                                            {
                                                if (dr.GetInt32(0) > 0) noRecord = false;
                                            }
                                        }
                                        ModeAutoRecordDIc.Add(dataop_table + "|" + autofid, noRecord);
                                    }
                                    if (noRecord)
                                    {
                                        dataop_type = 0;
                                        dataop_fid = "";
                                    }
                                    else
                                    {
                                        dataop_type = 1;
                                    }
                                }
                                if (dataop_type == 0)
                                {
                                    if (!TableAdd.ContainsKey(dataop_table))
                                    {
                                        TableAdd.Add(dataop_table, new Hashtable());
                                        if (!dataop_fid.Equals(""))
                                        {
                                            TableAddFid.Add(dataop_table, dataop_fid);
                                        }
                                        else
                                        {
                                            if (TableAddFid.Count == 0) TableAddFid.Add(dataop_table, FirstAddFid);
                                            else TableAddFid.Add(dataop_table, DBSuit.KeyGenerate(dataop_table).ToString());
                                        }
                                    }
                                    if (!((Hashtable)TableAdd[dataop_table]).Contains(dataop_col))
                                    {
                                        ((Hashtable)TableAdd[dataop_table]).Add(dataop_col, revalue);
                                    }
                                    int rateindex = 1;
                                    revalue = PostStrSafe(Context.Request.Form[key + "_rowrate" + rateindex].FirstOrDefault<string>(), key);
                                    int FindDeep = 0;
                                    int SaveRateIndex = 1;
                                    while (revalue == null && FindDeep < SysConst.RateRowFindDeep)
                                    {
                                        FindDeep++;
                                        rateindex++;
                                        revalue = PostStrSafe(Context.Request.Form[key + "_rowrate" + rateindex].FirstOrDefault<string>(), key);
                                    }
                                    while (revalue != null && revalue != "[FTNULL]")
                                    {
                                        if (dataop_adv != "") revalue = Ajax.Control.DataOPAdvVal(Context, db, st, dataop_adv, FirstAddFid, SaveRateIndex, TableAdd, TableMod);
                                        if (revalue.StartsWith("@code("))
                                        {
                                            revalue = Interface.Code.Get(adv.GetSpecialBase(Context, revalue, siteid), Context);
                                        }
                                        if (!TableAddDy.Contains(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString() }))
                                        {
                                            TableAddDy.Add(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString() }, IsLockValue ? LockValue : revalue);
                                        }
                                        SaveRateIndex++;
                                        rateindex++;
                                        revalue = PostStrSafe(Context.Request.Form[key + "_rowrate" + rateindex].FirstOrDefault<string>(), key);
                                        FindDeep = 0;
                                        while (revalue == null && FindDeep < SysConst.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;
                                            revalue = PostStrSafe(Context.Request.Form[key + "_rowrate" + rateindex].FirstOrDefault<string>(), key);
                                        }
                                    }
                                }
                                else if (dataop_type == 1)
                                {
                                    string elefid = defaultfid;
                                    if (!dataop_fid.Equals(""))
                                    {
                                        elefid = dataop_fid;
                                    }
                                    if (!TableMod.ContainsKey(dataop_table))
                                    {
                                        TableMod.Add(dataop_table, new Hashtable());
                                    }
                                    if (!((Hashtable)TableMod[dataop_table]).Contains(elefid))
                                    {
                                        ((Hashtable)TableMod[dataop_table]).Add(elefid, new Hashtable());
                                    }
                                    if (!((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).Contains(dataop_col))
                                    {
                                        ((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).Add(dataop_col, revalue);
                                    }
                                    int rateindex = 1;
                                    revalue = PostStrSafe(Context.Request.Form[key + "_rowrate" + rateindex].FirstOrDefault<string>(), key);
                                    int FindDeep = 0;
                                    int SaveRateIndex = 1;
                                    while (revalue == null && FindDeep < SysConst.RateRowFindDeep)
                                    {
                                        FindDeep++;
                                        rateindex++;
                                        revalue = PostStrSafe(Context.Request.Form[key + "_rowrate" + rateindex].FirstOrDefault<string>(), key);
                                    }
                                    while (revalue != null && revalue != "[FTNULL]")
                                    {
                                        if (dataop_adv != "") revalue = Ajax.Control.DataOPAdvVal(Context, db, st, dataop_adv, FirstAddFid, SaveRateIndex, TableAdd, TableMod);
                                        if (revalue.StartsWith("@code("))
                                        {
                                            revalue = Interface.Code.Get(adv.GetSpecialBase(Context, revalue, siteid), Context);
                                        }
                                        if (!TableModDy.Contains(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString(), elefid }))
                                        {
                                            TableModDy.Add(new string[] { dataop_table_dy, dataop_col, SaveRateIndex.ToString(), elefid }, new string[] { IsLockValue ? LockValue : revalue, "TextBox" });
                                        }
                                        SaveRateIndex++;
                                        rateindex++;
                                        revalue = PostStrSafe(Context.Request.Form[key + "_rowrate" + rateindex].FirstOrDefault<string>(), key);
                                        FindDeep = 0;
                                        while (revalue == null && FindDeep < SysConst.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;
                                            revalue = PostStrSafe(Context.Request.Form[key + "_rowrate" + rateindex].FirstOrDefault<string>(), key);
                                        }
                                    }
                                }
                                else if (dataop_type >= 2 && dataop_type <= 6)//多行 
                                {
                                    if (dataop_col.ToLower() == DBSuit.Key(dataop_table, FidCol2).KeyName.ToLower())//处理多行配置
                                    {
                                        //3 多行新增 不需要 name值
                                        //4 多行重置 不需要 name值
                                        //5 多行重置保留FID  需要 name值
                                        //6 多行仅更新 需要name值
                                        //在此全部处理，若未添加name值导致处理不到的，则在无name值处理
                                        if (dataop_type >= 3 && dataop_type <= 6)
                                        {
                                            if (!TableMuiltiSet.ContainsKey(dataop_table))
                                            {
                                                TableMuiltiSet.Add(dataop_table, new object[] {
                                                    dataop_type,key
                                                    ,Ajax.Control.DataOPAdvVal(Context, db, st, dataop_adv, FirstAddFid, 0,TableAdd, TableMod)
                                                }); ;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!TableMultiCol.ContainsKey(dataop_table))
                                        {
                                            TableMultiCol.Add(dataop_table, new Dictionary<int, Dictionary<string, string>>());
                                        }
                                        if (!TableMultiCol[dataop_table].ContainsKey(0))
                                        {
                                            TableMultiCol[dataop_table].Add(0, new Dictionary<string, string>());
                                        }
                                        if (!TableMultiCol[dataop_table][0].ContainsKey(dataop_col))
                                        {
                                            TableMultiCol[dataop_table][0].Add(dataop_col, revalue);
                                        }
                                        int rateindex = 1;
                                        revalue = PostStrSafe(Context.Request.Form[key + "_rowrate" + rateindex].FirstOrDefault<string>(), key);
                                        int FindDeep = 0;
                                        int SaveRateIndex = 1;
                                        while (revalue == null && FindDeep < SysConst.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;
                                            revalue = PostStrSafe(Context.Request.Form[key + "_rowrate" + rateindex].FirstOrDefault<string>(), key);
                                        }
                                        while (revalue != null && revalue != "[FTNULL]")
                                        {
                                            if (dataop_adv != "") revalue = Ajax.Control.DataOPAdvVal(Context, db, st, dataop_adv, FirstAddFid, SaveRateIndex, TableAdd, TableMod);
                                            if (revalue.StartsWith("@code("))
                                            {
                                                revalue = Interface.Code.Get(adv.GetSpecialBase(Context, revalue, siteid), Context);
                                            }
                                            if (!TableMultiCol[dataop_table].ContainsKey(SaveRateIndex))
                                            {
                                                TableMultiCol[dataop_table].Add(SaveRateIndex, new Dictionary<string, string>());
                                            }
                                            if (!TableMultiCol[dataop_table][SaveRateIndex].ContainsKey(dataop_col))
                                            {
                                                TableMultiCol[dataop_table][SaveRateIndex].Add(dataop_col, IsLockValue ? LockValue : revalue);
                                            }
                                            SaveRateIndex++;
                                            rateindex++;
                                            revalue = PostStrSafe(Context.Request.Form[key + "_rowrate" + rateindex].FirstOrDefault<string>(), key);
                                            FindDeep = 0;
                                            while (revalue == null && FindDeep < SysConst.RateRowFindDeep)
                                            {
                                                FindDeep++;
                                                rateindex++;
                                                revalue = PostStrSafe(Context.Request.Form[key + "_rowrate" + rateindex].FirstOrDefault<string>(), key);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    //处理没有name值的DataOP_Names
                    #region 处理NoName
                    IDictionaryEnumerator _Names = DataOP_Names.GetEnumerator();
                    while (_Names.MoveNext())
                    {
                        if (_Names.Key.ToString().StartsWith("adv:"))
                        {
                            string[] dataop_para = (string[])(_Names.Value);
                            string dataop_tabletag = dataop_para[0];
                            int dataop_type = int.Parse(dataop_para[1]);
                            string dataop_fid = dataop_para[2];
                            string dataop_adv = dataop_para[3];
                            string dataop_table = dataop_tabletag.Split('.')[0];
                            dataop_table = dataop_table.StartsWith("@") ? dataop_table.Substring(1) : ("ft_" + siteid + "_f_" + dataop_table);
                            string dataop_col = dataop_tabletag.Split('.')[1];
                            string revalue = Ajax.Control.DataOPAdvVal(Context, db, st, dataop_adv, FirstAddFid, 0, TableAdd, TableMod);

                            //处理Auto
                            if (dataop_type == 13)//Auto
                            {
                                string autofid = defaultfid;
                                if (!dataop_fid.Equals(""))
                                {
                                    autofid = dataop_fid;
                                }
                                string CurFidCol = DBSuit.Key(dataop_table, FidCol).KeyName;
                                string CusWhere = null;
                                bool noRecord = true;
                                if (ModeAutoRecordDIc.ContainsKey(dataop_table + "|" + autofid))
                                {
                                    noRecord = ModeAutoRecordDIc[dataop_table + "|" + autofid];
                                }
                                else
                                {
                                    if (autofid.Trim().StartsWith("where ", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        CusWhere = autofid.Trim();
                                    }
                                    else if (autofid.IndexOf(',') >= 0)
                                    {
                                        CurFidCol = autofid.Split(',')[1];
                                        autofid = autofid.Split(',')[0];
                                    }
                                    string sql = "select count(*) as ca from " + dataop_table.D2();
                                    if (CusWhere != null) sql += " " + CusWhere;
                                    else
                                    {
                                        sql += " where " + str.D2DD(CurFidCol) + "='" + str.D2DD(autofid) + "'";
                                    }
                                    using (DR dr = db.OpenRecord(sql, st))
                                    {
                                        if (dr.Read())
                                        {
                                            if (dr.GetInt32(0) > 0) noRecord = false;
                                        }
                                    }
                                    ModeAutoRecordDIc.Add(dataop_table + "|" + autofid, noRecord);
                                }
                                if (noRecord)
                                {
                                    dataop_type = 0;
                                    dataop_fid = "";
                                }
                                else
                                {
                                    dataop_type = 1;
                                }
                            }
                            if (dataop_type == 0)
                            {
                                if (!TableAdd.ContainsKey(dataop_table))
                                {
                                    TableAdd.Add(dataop_table, new Hashtable());
                                    if (!dataop_fid.Equals(""))
                                    {
                                        TableAddFid.Add(dataop_table, dataop_fid);
                                    }
                                    else
                                    {
                                        if (TableAddFid.Count == 0) TableAddFid.Add(dataop_table, FirstAddFid);
                                        else TableAddFid.Add(dataop_table, DBSuit.KeyGenerate(dataop_table).ToString());
                                    }
                                }
                                if (!((Hashtable)TableAdd[dataop_table]).Contains(dataop_col))
                                {
                                    ((Hashtable)TableAdd[dataop_table]).Add(dataop_col, revalue);
                                }
                            }
                            else if (dataop_type == 1)
                            {
                                string elefid = defaultfid;
                                if (!dataop_fid.Equals(""))
                                {
                                    elefid = dataop_fid;
                                }
                                if (!TableMod.ContainsKey(dataop_table))
                                {
                                    TableMod.Add(dataop_table, new Hashtable());
                                }
                                if (!((Hashtable)TableMod[dataop_table]).Contains(elefid))
                                {
                                    ((Hashtable)TableMod[dataop_table]).Add(elefid, new Hashtable());
                                }
                                if (!((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).Contains(dataop_col))
                                {
                                    ((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).Add(dataop_col, revalue);
                                }
                            }
                            else if (dataop_type >= 2 && dataop_type <= 6)//多行 
                            {
                                if (dataop_col.ToLower() == DBSuit.Key(dataop_table, FidCol2).KeyName.ToLower())//处理多行配置
                                {
                                    //3 多行新增 不需要 name值
                                    //4 多行重置 不需要 name值
                                    //5 多行重置保留FID  需要 name值
                                    //6 多行仅更新 需要name值
                                    if (dataop_type >= 3 && dataop_type <= 6)
                                    {
                                        if (!TableMuiltiSet.ContainsKey(dataop_table))
                                        {
                                            TableMuiltiSet.Add(dataop_table, new object[] {
                                                    dataop_type,""
                                                    ,Ajax.Control.DataOPAdvVal(Context, db, st, dataop_adv, FirstAddFid, 0,TableAdd, TableMod)
                                                }); ;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!TableMultiCol.ContainsKey(dataop_table))
                                    {
                                        TableMultiCol.Add(dataop_table, new Dictionary<int, Dictionary<string, string>>());
                                    }
                                    if (!TableMultiCol[dataop_table].ContainsKey(0))
                                    {
                                        TableMultiCol[dataop_table].Add(0, new Dictionary<string, string>());
                                    }
                                    if (!TableMultiCol[dataop_table][0].ContainsKey(dataop_col))
                                    {
                                        TableMultiCol[dataop_table][0].Add(dataop_col, revalue);
                                    }
                                }
                            }

                            //处理过的无Name存到已经处理过的Keys，防止被当选项框未选中而置空
                            OPedKey.Add(_Names.Key.ToString());
                        }
                    }
                    #endregion
                    //复选框、单选框未选中时，得不到值，则默认为空 ，动态新增行时可能有Bug，不建议在动态新增行使用单选、复选
                    _Names = DataOP_Names.GetEnumerator();
                    while (_Names.MoveNext())
                    {
                        if (!OPedKey.Contains(_Names.Key.ToString()) && !_Names.Key.ToString().StartsWith("adv:"))
                        {
                            string[] dataop_para = (string[])_Names.Value;
                            string dataop_tabletag = dataop_para[0];
                            int dataop_type = int.Parse(dataop_para[1]);
                            string dataop_fid = dataop_para[2];
                            string dataop_table = dataop_tabletag.Split('.')[0];
                            dataop_table = dataop_table.StartsWith("@") ? dataop_table.Substring(1) : ("ft_" + siteid + "_f_" + dataop_table);
                            string dataop_col = dataop_tabletag.Split('.')[1];
                            string dataop_table_dy = dataop_table + "_dy";

                            //处理Auto
                            if (dataop_type == 13)//Auto
                            {
                                string autofid = defaultfid;
                                if (!dataop_fid.Equals(""))
                                {
                                    autofid = dataop_fid;
                                }
                                string CurFidCol = DBSuit.Key(dataop_table, FidCol).KeyName;
                                string CusWhere = null;
                                bool noRecord = true;
                                if (ModeAutoRecordDIc.ContainsKey(dataop_table + "|" + autofid))
                                {
                                    noRecord = ModeAutoRecordDIc[dataop_table + "|" + autofid];
                                }
                                else
                                {
                                    if (autofid.Trim().StartsWith("where ", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        CusWhere = autofid.Trim();
                                    }
                                    else if (autofid.IndexOf(',') >= 0)
                                    {
                                        CurFidCol = autofid.Split(',')[1];
                                        autofid = autofid.Split(',')[0];
                                    }
                                    string sql = "select count(*) as ca from " + dataop_table.D2();
                                    if (CusWhere != null) sql += " " + CusWhere;
                                    else
                                    {
                                        sql += " where " + str.D2DD(CurFidCol) + "='" + str.D2DD(autofid) + "'";
                                    }
                                    using (DR dr = db.OpenRecord(sql, st))
                                    {
                                        if (dr.Read())
                                        {
                                            if (dr.GetInt32(0) > 0) noRecord = false;
                                        }
                                    }
                                    ModeAutoRecordDIc.Add(dataop_table + "|" + autofid, noRecord);
                                }
                                if (noRecord)
                                {
                                    dataop_type = 0;
                                    dataop_fid = "";
                                }
                                else
                                {
                                    dataop_type = 1;
                                }
                            }
                            if (dataop_type == 0)
                            {
                                if (!TableAdd.ContainsKey(dataop_table))
                                {
                                    TableAdd.Add(dataop_table, new Hashtable());
                                    if (TableAddFid.Count == 0) TableAddFid.Add(dataop_table, FirstAddFid);
                                    else TableAddFid.Add(dataop_table, DBSuit.KeyGenerate(dataop_table).ToString());
                                }
                                if (!((Hashtable)TableAdd[dataop_table]).Contains(dataop_col))
                                {
                                    ((Hashtable)TableAdd[dataop_table]).Add(dataop_col, "");
                                }
                            }
                            else if (dataop_type == 1)
                            {
                                string elefid = defaultfid;
                                if (!dataop_fid.Equals(""))
                                {
                                    elefid = dataop_fid;
                                }
                                if (!TableMod.ContainsKey(dataop_table))
                                {
                                    TableMod.Add(dataop_table, new Hashtable());
                                }
                                if (!((Hashtable)TableMod[dataop_table]).Contains(elefid))
                                {
                                    ((Hashtable)TableMod[dataop_table]).Add(elefid, new Hashtable());
                                }
                                if (!((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).Contains(dataop_col))
                                {
                                    ((Hashtable)(((Hashtable)TableMod[dataop_table]))[elefid]).Add(dataop_col, "");
                                }
                            }
                            else if (dataop_type >= 2 && dataop_type <= 6)//多行
                            {
                                if (!TableMultiCol.ContainsKey(dataop_table))
                                {
                                    TableMultiCol.Add(dataop_table, new Dictionary<int, Dictionary<string, string>>());
                                }
                                if (!TableMultiCol[dataop_table].ContainsKey(0))
                                {
                                    TableMultiCol[dataop_table].Add(0, new Dictionary<string, string>());
                                }
                                if (!TableMultiCol[dataop_table][0].ContainsKey(dataop_col))
                                {
                                    TableMultiCol[dataop_table][0].Add(dataop_col, "");
                                }
                            }
                        }
                    }
                    _Names = null;
                    OPedKey.Clear();
                    OPedKey = null;
                    DataOP_FileKey.Clear();
                    DataOP_FileKey = null;
                    DataOP_Names.Clear();
                    DataOP_Names = null;
                    ModeAutoRecordDIc.Clear();
                    ModeAutoRecordDIc = null;
                    #endregion
                    #region 自定义字段组件
                    /*
                    ArrayList CusCol_FileKey = new ArrayList();
                    foreach (string key in Context.Request.Files.AllKeys)
                    {
                        string revalue = null;
                        if (key.StartsWith("FTElement_cuscol_"))
                        {
                            CusCol_FileKey.Add(key);
                            if (key.IndexOf("_rowrate") < 0)
                            {
                                string cuscol_table = key.Split('_')[3];
                                cuscol_table = cuscol_table.StartsWith("@") ? cuscol_table.Substring(1) : ("ft_" + siteid + "_f_" + cuscol_table);
                                string cuscol_table_dy = cuscol_table + "_dy";
                                string cuscol_column = key.Split('_')[4];
                                string cuscol_paraname = key.Split('_')[5];
                                int cuscol_actiontype = int.Parse(key.Split('_')[2]);
                                revalue = UploadFile(Context, key);
                                //选中删除要删除的文件，才删除
                                if ((this.Context.Request.Form["filedel_" + key] == null || !this.Context.Request.Form["filedel_" + key].Equals("1")))
                                {
                                    if (!StayedPostedFiles.Contains(cuscol_table + "." + cuscol_column)) StayedPostedFiles.Add(cuscol_table + "." + cuscol_column);
                                }
                                if (revalue == null) revalue = "";
                                if (cuscol_actiontype == 0)
                                {
                                    if (!TableAdd.ContainsKey(cuscol_table))
                                    {
                                        TableAdd.Add(cuscol_table, new Hashtable());
                                        if (TableAddFid.Count == 0) TableAddFid.Add(cuscol_table, FirstAddFid);
                                        else TableAddFid.Add(cuscol_table, str.GetCombID());

                                    }
                                    if (!((Hashtable)TableAdd[cuscol_table]).Contains(cuscol_column))
                                    {
                                        ((Hashtable)TableAdd[cuscol_table]).Add(cuscol_column, revalue);
                                    }
                                    int rateindex = 1;
                                    HttpPostedFile pfile = this.Context.Request.Files[key + "_rowrate" + rateindex];
                                    int FindDeep = 0;
                                    int SaveRateIndex = 1;
                                    while (pfile == null && FindDeep < ConstStr.RateRowFindDeep)
                                    {
                                        FindDeep++;
                                        rateindex++;
                                        pfile = this.Context.Request.Files[key + "_rowrate" + rateindex];
                                    }
                                    while (pfile != null)
                                    {
                                        revalue = UploadFile(this.Context, key + "_rowrate" + rateindex);
                                        if (!TableAddDy.Contains(new string[] { cuscol_table_dy, cuscol_column, SaveRateIndex.ToString() }))
                                        {
                                            TableAddDy.Add(new string[] { cuscol_table_dy, cuscol_column, SaveRateIndex.ToString() }, revalue);
                                        }
                                        SaveRateIndex++;
                                        rateindex++;
                                        pfile = this.Context.Request.Files[key + "_rowrate" + rateindex];
                                        FindDeep = 0;
                                        while (pfile == null && FindDeep < ConstStr.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;
                                            pfile = this.Context.Request.Files[key + "_rowrate" + rateindex];
                                        }
                                    }
                                }
                                else if (cuscol_actiontype == 1)
                                {
                                    string elefid = defaultfid;
                                    if (!cuscol_paraname.Equals(""))
                                    {
                                        elefid = this.Context.Request.Form["para_" + cuscol_paraname];
                                    }
                                    if (elefid == null || elefid.Equals("")) continue;
                                    if (!TableMod.ContainsKey(cuscol_table))
                                    {
                                        TableMod.Add(cuscol_table, new Hashtable());
                                    }
                                    if (!((Hashtable)TableMod[cuscol_table]).Contains(elefid))
                                    {
                                        ((Hashtable)TableMod[cuscol_table]).Add(elefid, new Hashtable());
                                    }
                                    if (!((Hashtable)(((Hashtable)TableMod[cuscol_table]))[elefid]).Contains(cuscol_column))
                                    {
                                        ((Hashtable)(((Hashtable)TableMod[cuscol_table]))[elefid]).Add(cuscol_column, revalue);
                                    }
                                    int rateindex = 1;
                                    HttpPostedFile pfile = this.Context.Request.Files[key + "_rowrate" + rateindex];
                                    int FindDeep = 0;
                                    int SaveRateIndex = 1;
                                    while (pfile == null && FindDeep < ConstStr.RateRowFindDeep)
                                    {
                                        FindDeep++;
                                        rateindex++;
                                        pfile = this.Context.Request.Files[key + "_rowrate" + rateindex];
                                    }
                                    while (pfile != null)
                                    {
                                        revalue = UploadFile(this.Context, key + "_rowrate" + rateindex);
                                        if (!TableModDy.Contains(new string[] { cuscol_table_dy, cuscol_column, SaveRateIndex.ToString(), elefid }))
                                        {
                                            TableModDy.Add(new string[] { cuscol_table_dy, cuscol_column, SaveRateIndex.ToString(), elefid }, new string[] { revalue, "FileBox" });
                                        }
                                        SaveRateIndex++;
                                        rateindex++;
                                        pfile = this.Context.Request.Files[key + "_rowrate" + rateindex];
                                        FindDeep = 0;
                                        while (pfile == null && FindDeep < ConstStr.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;
                                            pfile = this.Context.Request.Files[key + "_rowrate" + rateindex];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (string key in this.Context.Request.Form.AllKeys)
                    {
                        string revalue = null;
                        if (key.StartsWith("FTElement_cuscol_") && !CusCol_FileKey.Contains(key))
                        {
                            if (key.IndexOf("_rowrate") < 0)
                            {
                                string cuscol_table = key.Split('_')[3];
                                cuscol_table = cuscol_table.StartsWith("@") ? cuscol_table.Substring(1) : ("ft_" + siteid + "_f_" + cuscol_table);
                                string cuscol_table_dy = cuscol_table + "_dy";
                                string cuscol_column = key.Split('_')[4];
                                string cuscol_paraname = key.Split('_')[5];
                                int cuscol_actiontype = int.Parse(key.Split('_')[2]);
                                revalue = this.Context.Request.Form[key];
                                if (revalue == null) revalue = "";
                                if (cuscol_actiontype == 0)
                                {
                                    if (!TableAdd.ContainsKey(cuscol_table))
                                    {
                                        TableAdd.Add(cuscol_table, new Hashtable());
                                        if (TableAddFid.Count == 0) TableAddFid.Add(cuscol_table, FirstAddFid);
                                        else TableAddFid.Add(cuscol_table, str.GetCombID());

                                    }
                                    if (!((Hashtable)TableAdd[cuscol_table]).Contains(cuscol_column))
                                    {
                                        ((Hashtable)TableAdd[cuscol_table]).Add(cuscol_column, revalue);
                                    }
                                    int rateindex = 1;
                                    revalue = this.Context.Request.Form[key + "_rowrate" + rateindex];
                                    int FindDeep = 0;
                                    int SaveRateIndex = 1;
                                    while (revalue == null && FindDeep < ConstStr.RateRowFindDeep)
                                    {
                                        FindDeep++;
                                        rateindex++;
                                        revalue = this.Context.Request.Form[key + "_rowrate" + rateindex];
                                    }
                                    while (revalue != null)
                                    {
                                        if (!TableAddDy.Contains(new string[] { cuscol_table_dy, cuscol_column, SaveRateIndex.ToString() }))
                                        {
                                            TableAddDy.Add(new string[] { cuscol_table_dy, cuscol_column, SaveRateIndex.ToString() }, revalue);
                                        }
                                        SaveRateIndex++;
                                        rateindex++;
                                        revalue = this.Context.Request.Form[key + "_rowrate" + rateindex];
                                        FindDeep = 0;
                                        while (revalue == null && FindDeep < ConstStr.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;
                                            revalue = this.Context.Request.Form[key + "_rowrate" + rateindex];
                                        }
                                    }
                                }
                                else if (cuscol_actiontype == 1)
                                {
                                    string elefid = defaultfid;
                                    if (!cuscol_paraname.Equals(""))
                                    {
                                        elefid = this.Context.Request.Form["para_" + cuscol_paraname];
                                    }
                                    if (!TableMod.ContainsKey(cuscol_table))
                                    {
                                        TableMod.Add(cuscol_table, new Hashtable());
                                    }
                                    if (!((Hashtable)TableMod[cuscol_table]).Contains(elefid))
                                    {
                                        ((Hashtable)TableMod[cuscol_table]).Add(elefid, new Hashtable());
                                    }
                                    if (!((Hashtable)(((Hashtable)TableMod[cuscol_table]))[elefid]).Contains(cuscol_column))
                                    {
                                        ((Hashtable)(((Hashtable)TableMod[cuscol_table]))[elefid]).Add(cuscol_column, revalue);
                                    }
                                    int rateindex = 1;
                                    revalue = this.Context.Request.Form[key + "_rowrate" + rateindex];
                                    int FindDeep = 0;
                                    int SaveRateIndex = 1;
                                    while (revalue == null && FindDeep < ConstStr.RateRowFindDeep)
                                    {
                                        FindDeep++;
                                        rateindex++;
                                        revalue = this.Context.Request.Form[key + "_rowrate" + rateindex];
                                    }
                                    while (revalue != null)
                                    {
                                        if (!TableModDy.Contains(new string[] { cuscol_table_dy, cuscol_column, SaveRateIndex.ToString(), elefid }))
                                        {
                                            TableModDy.Add(new string[] { cuscol_table_dy, cuscol_column, SaveRateIndex.ToString(), elefid }, new string[] { revalue, "TextBox" });
                                        }
                                        SaveRateIndex++;
                                        rateindex++;
                                        revalue = this.Context.Request.Form[key + "_rowrate" + rateindex];
                                        FindDeep = 0;
                                        while (revalue == null && FindDeep < ConstStr.RateRowFindDeep)
                                        {
                                            FindDeep++;
                                            rateindex++;
                                            revalue = this.Context.Request.Form[key + "_rowrate" + rateindex];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    CusCol_FileKey.Clear();
                    CusCol_FileKey = null;
                    */
                    #endregion
                    #region 操作前代码
                    if (codebefore.StartsWith("@code("))
                    {
                        string[] codebeforeitem = codebefore.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string codei in codebeforeitem)
                        {
                            string codebeforereval = Interface.Code.Get(codei, Context);
                            if (codebeforereval != null)
                            {
                                Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('" + codebeforereval + "');"));
                                st.Rollback();
                                return;
                            }
                        }
                    }
                    #endregion
                    if (0 > 1)//IsMultiMod
                    {
                        /*
                        #region 组件级别多行更新 可废弃
                        if (MultiFidName.Equals(""))
                        {
                            TableAdd.Clear();
                            TableAdd = null;
                            TableAddDy.Clear();
                            TableAddDy = null;
                            TableAddFid.Clear();
                            TableAddFid = null;
                            TableMod.Clear();
                            TableMod = null;
                            TableModDy.Clear();
                            TableModDy = null;
                            StayedPostedFiles.Clear();
                            StayedPostedFiles = null;
                            Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('未指定存储fid的元素名称');"));
                            return;
                        }
                        string TableName = null;
                        IDictionaryEnumerator HashEnum = null;
                        HashEnum = TableAdd.GetEnumerator();
                        if (HashEnum.MoveNext())
                        {
                            TableName = HashEnum.Key.ToString();
                        }
                        if (TableName == null)
                        {
                            HashEnum = TableMod.GetEnumerator();
                            if (HashEnum.MoveNext())
                            {
                                TableName = HashEnum.Key.ToString();
                            }
                        }
                        if (TableName == null)
                        {
                            TableAdd.Clear();
                            TableAdd = null;
                            TableAddDy.Clear();
                            TableAddDy = null;
                            TableAddFid.Clear();
                            TableAddFid = null;
                            TableMod.Clear();
                            TableMod = null;
                            TableModDy.Clear();
                            TableModDy = null;
                            StayedPostedFiles.Clear();
                            StayedPostedFiles = null;
                            HashEnum = null;
                            Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('未指定数据表格');"));
                            return;
                        }
                        ArrayList NewFids = new ArrayList();
                        Hashtable AllValues = new Hashtable();
                        if (!MultiCondition.Equals(""))
                        {
                            if (MultiCdnEvals != null)
                            {
                                string[] cdntem = MultiCdnEvals.Split(new string[] { "##" }, StringSplitOptions.None);
                                int IndexI = 0;
                                Regex r = new Regex(@"{[^}]*}");
                                MatchCollection mc = r.Matches(MultiCondition);
                                foreach (Match m in mc)
                                {
                                    if (cdntem.Length > IndexI)
                                    {
                                        MultiCondition = MultiCondition.Replace(m.Value, str.D2DD(cdntem[IndexI]));
                                    }
                                    IndexI++;
                                }
                            }
                            MultiCondition = " and (" + MultiCondition + ") ";
                        }
                        bool ForUpdate = false;
                        if (MultiFidName.StartsWith("[update]"))
                        {
                            ForUpdate = true;
                            MultiFidName = MultiFidName.Substring("[update]".Length);
                        }
                        string fid = this.Context.Request.Form[MultiFidName];
                        bool IsDelRow1 = (this.Context.Request.Form["Row1DeleteHdn"] != null && this.Context.Request.Form["Row1DeleteHdn"] == "yes");//删除首行标记
                        if (!IsDelRow1)
                        {
                            if (NewFids.Contains(fid))
                            {
                                NewFids.Add(str.GetCombID());
                            }
                            else
                            {
                                NewFids.Add(fid);
                            }
                        }
                        int rateindex = 1;
                        fid = this.Context.Request.Form[MultiFidName + "_rowrate" + rateindex];
                        int FindDeep = 0;
                        int SaveRateIndex = 1;
                        while (fid == null && FindDeep < ConstStr.RateRowFindDeep)
                        {
                            FindDeep++;
                            rateindex++;
                            fid = this.Context.Request.Form[MultiFidName + "_rowrate" + rateindex];
                        }
                        while (fid != null)
                        {
                            if (NewFids.Contains(fid))
                            {
                                NewFids.Add(str.GetCombID());
                            }
                            else
                            {
                                NewFids.Add(fid);
                            }
                            SaveRateIndex++;
                            rateindex++;
                            fid = this.Context.Request.Form[MultiFidName + "_rowrate" + rateindex];
                            FindDeep = 0;
                            while (fid == null && FindDeep < ConstStr.RateRowFindDeep)
                            {
                                FindDeep++;
                                rateindex++;
                                fid = this.Context.Request.Form[MultiFidName + "_rowrate" + rateindex];
                            }
                        }

                        IDictionaryEnumerator HashEnum_2 = null;
                        IDictionaryEnumerator HashEnum_3 = null;
                        ArrayList ValueItem = new ArrayList();
                        HashEnum = TableAdd.GetEnumerator();
                        while (HashEnum.MoveNext())
                        {
                            Hashtable RowItem = ((Hashtable)(HashEnum.Value));
                            HashEnum_2 = RowItem.GetEnumerator();
                            while (HashEnum_2.MoveNext())
                            {
                                ValueItem.Add(new string[] { HashEnum_2.Key.ToString(), HashEnum_2.Value.ToString() });
                            }
                            RowItem.Clear();
                            RowItem = null;
                        }
                        HashEnum = TableMod.GetEnumerator();
                        while (HashEnum.MoveNext())
                        {
                            Hashtable RowItem = ((Hashtable)(HashEnum.Value));
                            HashEnum_2 = RowItem.GetEnumerator();
                            while (HashEnum_2.MoveNext())
                            {
                                Hashtable ColItem = ((Hashtable)(HashEnum_2.Value));
                                HashEnum_3 = ColItem.GetEnumerator();
                                while (HashEnum_3.MoveNext())
                                {
                                    ValueItem.Add(new string[] { HashEnum_3.Key.ToString(), HashEnum_3.Value.ToString() });
                                }
                            }
                            RowItem.Clear();
                            RowItem = null;
                        }

                        AllValues.Add(0, ValueItem);

                        HashEnum = TableAddDy.GetEnumerator();
                        while (HashEnum.MoveNext())
                        {
                            int erate = int.Parse(((string[])HashEnum.Key)[2]);
                            string col = ((string[])HashEnum.Key)[1];
                            string evalue = HashEnum.Value.ToString();
                            if (!AllValues.ContainsKey(erate)) AllValues.Add(erate, new ArrayList());
                            ((ArrayList)AllValues[erate]).Add(new string[] { col, evalue });
                        }
                        HashEnum = TableModDy.GetEnumerator();
                        while (HashEnum.MoveNext())
                        {
                            int erate = int.Parse(((string[])HashEnum.Key)[2]);
                            string col = ((string[])HashEnum.Key)[1];
                            string evalue = ((string[])HashEnum.Value)[0];
                            if (!AllValues.ContainsKey(erate)) AllValues.Add(erate, new ArrayList());
                            ((ArrayList)AllValues[erate]).Add(new string[] { col, evalue });
                        }

                        //先删除
                        if (!ForUpdate)
                        {
                            string sql = "delete from " + str.D2DD(TableName) + " where 1=1 " + MultiCondition;
                            log.Debug(sql, "[Form Data Multiple]");
                            db.ExecSql(sql, st);
                            for (var erateI = 0; erateI < NewFids.Count; erateI++)
                            {
                                int DataIndex = (IsDelRow1 ? (erateI + 1) : erateI);//如果首行被删除，则数据从第二行数据开始
                                if (AllValues.ContainsKey(DataIndex))
                                {
                                    string newfid = NewFids[erateI].ToString().Trim();
                                    if (newfid.Equals("")) newfid = str.GetCombID();
                                    sql = "insert into " + str.D2DD(TableName) + "(" + str.D2DD(FidCol) + "";
                                    if (OpDefaultCol == 1) sql += ",fmem,modfmem,addtime,updatetime,dydata,stat,flow,flowpos";
                                    string colstr = "";
                                    string colvalue = "";
                                    foreach (string[] colval in (ArrayList)(AllValues[DataIndex]))
                                    {
                                        colstr += "," + str.D2DD(colval[0]);
                                        colvalue += "," + (colval[1].Equals("[FTNULL]") ? "null" : ("'" + str.D2DD(colval[1]) + "'"));
                                    }
                                    sql = sql + colstr + ")values('" + str.D2DD(newfid) + "'";
                                    if (OpDefaultCol == 1) sql += ",'" + str.D2DD(memid) + "','" + str.D2DD(memid) + "','" + str.GetDateTime() + "','" + str.GetDateTime() + "','',1,0,0";
                                    sql += colvalue + ")";
                                    log.Debug(sql, "[Form Data Multiple]");
                                    db.ExecSql(sql, st);
                                }
                            }
                        }
                        else
                        {
                            for (var erateI = 0; erateI < NewFids.Count; erateI++)
                            {
                                if (AllValues.ContainsKey(erateI))
                                {
                                    string curfid = NewFids[erateI].ToString().Trim();
                                    if (curfid != "")
                                    {
                                        string sql = "";
                                        if (OpDefaultCol == 1)
                                        {
                                            sql = "update " + str.D2DD(TableName) + " set modfmem='" + str.D2DD(memid) + "'";
                                            sql += ",updatetime='" + str.GetDateTime() + "'";
                                            foreach (string[] colval in (ArrayList)(AllValues[erateI]))
                                            {
                                                sql += "," + str.D2DD(colval[0]) + "=" + (colval[1].Equals("[FTNULL]") ? "null" : ("'" + str.D2DD(colval[1]) + "'"));
                                            }
                                        }
                                        else
                                        {
                                            sql = "update " + str.D2DD(TableName) + " set ";
                                            for (int i2 = 0; i2 < ((ArrayList)(AllValues[erateI])).Count; i2++)
                                            {
                                                if (i2 > 0) sql += ",";
                                                string[] colval = (string[])(((ArrayList)(AllValues[erateI]))[i2]);
                                                sql += str.D2DD(colval[0]) + "=" + (colval[1].Equals("[FTNULL]") ? "null" : ("'" + str.D2DD(colval[1]) + "'"));
                                            }
                                        }
                                        sql += " where " + str.D2DD(FidCol) + "='" + str.D2DD(curfid) + "' " + MultiCondition;
                                        log.Debug(sql, "[Form Data Multiple]");
                                        db.ExecSql(sql, st);
                                    }
                                }
                            }
                        }
                        FTFrame.Interface.Action.ActionSave(TableName, Context, 1);
                        NewOPLogV4(db, st, Context, tableolog, "datamultiple", 9, TableName, memid);

                        HashEnum = null;
                        HashEnum_2 = null;
                        HashEnum_3 = null;
                        NewFids.Clear();
                        NewFids = null;
                        AllValues.Clear();
                        AllValues = null; */
                        #endregion
                    }
                    else
                    {
                        #region 数据操作
                        //action start
                        string fid = null;
                        string FirstModFid = null;
                        #region Add
                        if (TableAdd.Count > 0)
                        {
                            //新增动态新增行
                            //IDictionaryEnumerator TableAddDyEnum = TableAddDy.GetEnumerator();
                            //while (TableAddDyEnum.MoveNext())
                            //{
                            //    string tablenamedy = ((string[])TableAddDyEnum.Key)[0];
                            //    fid = TableAddFid[tablenamedy.Substring(0, tablenamedy.Length - 3)].ToString();
                            //    if (fid == null || fid.Equals("")) continue;
                            //    string sql = "insert into " + tablenamedy + "(fid,eid,evalue,erate)";
                            //    sql += "values('" + fid + "','" + str.D2DD(((string[])TableAddDyEnum.Key)[1]) + "'," + (TableAddDyEnum.Value.ToString().Equals("[FTNULL]") ? "null" : ("'" + str.D2DD(TableAddDyEnum.Value.ToString()) + "'")) + "," + int.Parse(((string[])TableAddDyEnum.Key)[2]) + ")";
                            //    db.ExecSql(sql, st);
                            //}

                            //TableAddDyEnum = null;
                            //新增主表
                            IDictionaryEnumerator TableAddEnum = TableAdd.GetEnumerator();
                            while (TableAddEnum.MoveNext())
                            {
                                string tablename = TableAddEnum.Key.ToString();
                                fid = TableAddFid[tablename].ToString();
                                //fidval,fidcol
                                string CurFidCol = DBSuit.Key(tablename, FidCol).KeyName;
                                if (fid.IndexOf(',') >= 0)
                                {
                                    CurFidCol = fid.Split(',')[1];
                                    fid = fid.Split(',')[0];
                                }
                                //返回第一张新增表的fid
                                if (_newfid_.Equals("")) _newfid_ = fid;
                                //得到DyData
                                //DR drte = db.OpenRecord(Sql.TableExists(tablename + "_dy"), st);
                                //bool TableExists = drte.HasRows;
                                //drte.Close();
                                //string dyDataStr = "";
                                string sql = null;
                                //if (TableExists)
                                //{
                                //    sql = "select distinct eid from " + tablename + "_dy where fid='" + fid + "'";
                                //    DR dydr = db.OpenRecord(sql, st);
                                //    while (dydr.Read())
                                //    {
                                //        dyDataStr += "," + dydr.GetString(0);
                                //    }
                                //    dydr.Close();
                                //    if (!dyDataStr.Equals("")) dyDataStr = dyDataStr.Substring(0);
                                //}
                                var IsKeyColInSql = DBSuit.Key(tablename, FidCol).KeyType != Enums.KeyType.AutoIncrement;
                                if(IsKeyColInSql)sql = "insert into " + str.D2DD(tablename).Normal() + "(" + str.D2DD(CurFidCol) + "";
                                else sql = "insert into " + str.D2DD(tablename).Normal() + "(";
                                string OpDefaultCol_col = "";
                                string OpDefaultCol_val = "";
                                if (OpDefaultCol == 1)
                                {
                                    var dic = DBSuit.DefaultColsWhenAddForPage(tablename);
                                    foreach (var dicItem in dic)
                                    {
                                        OpDefaultCol_col += "," + dicItem.Key;
                                        OpDefaultCol_val += "," + (dicItem.Value == null ? "null" : ("'" + dicItem.Value.ToString().D2() + "'"));
                                    }
                                }
                                //sql += OpDefaultCol_col;
                                string colstr = "";
                                string colvalue = "";
                                //流水号
                                foreach (string[] liquiditem in LiquidAL)
                                {
                                    if (tablename.Equals(liquiditem[0]))
                                    {
                                        string liquid_id = Advance.GetLiquidID(liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                        int liquid_loop = 0;
                                        while (!Advance.IsLiquidIDOK(liquiditem[0], liquiditem[1], liquid_id))
                                        {
                                            if (liquid_loop > 99)
                                            {
                                                liquid_id = "[error]loop99";
                                                break;
                                            }
                                            liquid_loop++;
                                            liquid_id = Advance.GetLiquidID(liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                        }
                                        colstr += "," + func.SQLColSafe(liquiditem[1]).Normal();
                                        colvalue += "," + "'" + str.D2DD(liquid_id) + "'";
                                    }
                                }
                                IDictionaryEnumerator TableAddRowEnum = ((Hashtable)TableAddEnum.Value).GetEnumerator();
                                while (TableAddRowEnum.MoveNext())
                                {
                                    colstr += "," + func.SQLColSafe(TableAddRowEnum.Key.ToString()).Normal();
                                    colvalue += "," + (TableAddRowEnum.Value.ToString().Equals("[FTNULL]") ? "null" : ("'" + str.D2DD(TableAddRowEnum.Value.ToString()).Trim() + "'"));
                                }

                                int _type = 0;

                                if (datatype == 2 && tablename.Equals(BindTable)) { _type = 2; }
                                if (IsKeyColInSql)
                                {
                                    sql = sql + OpDefaultCol_col + colstr + ")values('" + fid.D2() + "'";
                                    sql += OpDefaultCol_val;
                                    sql += colvalue + ")";
                                }
                                else
                                {
                                    sql = sql + (OpDefaultCol_col + colstr).Substring(1) + ")values(";
                                    sql += (OpDefaultCol_val + colvalue).Substring(1);
                                    sql += ")";
                                }
                                //if (IsKeyColInSql) sql = sql + colstr + ")values('" + fid + "'";
                                //else sql = sql + colstr + ")values(";
                                //sql += OpDefaultCol_val;
                                ////if (OpDefaultCol == 1) sql += ",'" + str.D2DD(memid) + "','" + str.D2DD(memid) + "','" + str.GetDateTime() + "','" + str.GetDateTime() + "','" + str.D2DD(dyDataStr) + "',1,0,0";
                                //sql += colvalue + ")";
                                log.Debug(sql, "[Form Data Add]");
                                db.ExecSql(sql, st);

                                FTFrame.Project.Core.Action.ActionSave(tablename, Context, 0);


                                NewOPLogV4(db, st, Context, tableolog, fid, _type, tablename, memid);
                                if (datatype == 2)
                                {
                                    FlowActionResult = Base.Core.FlowOld.Action(siteid, db, st, fid, flowtype, flow, flowdesignpos, flowdesign, flowdesignbaranch, BindTable, Context, tableflog, _type, memid);
                                }

                                TableAddRowEnum = null;
                            }
                            TableAddEnum = null;
                        }
                        #endregion
                        #region Mod
                        if (TableMod.Count > 0)
                        {
                            IDictionaryEnumerator TableModEnum = TableMod.GetEnumerator();
                            while (TableModEnum.MoveNext())
                            {
                                string tablename = TableModEnum.Key.ToString();
                                //DR drte = db.OpenRecord(Sql.TableExists(tablename + "_dy"), st);
                                //bool TableExists = drte.HasRows;
                                //drte.Close();
                                IDictionaryEnumerator TableModFIDEnum = ((Hashtable)TableModEnum.Value).GetEnumerator();
                                while (TableModFIDEnum.MoveNext())
                                {

                                    string _flowstr = "";
                                    int _type = 1;
                                    if (datatype == 2 && tablename.Equals(BindTable)) { _type = 3; }

                                    string modfid = TableModFIDEnum.Key.ToString();
                                    if (FirstModFid == null) FirstModFid = modfid;
                                    //fidval,fidcol
                                    string CurFidCol = DBSuit.Key(tablename, FidCol).KeyName;
                                    string CusWhere = null;
                                    if (modfid.Trim().StartsWith("where ", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        CusWhere = modfid.Trim();
                                    }
                                    else if (modfid.IndexOf(',') >= 0)
                                    {
                                        CurFidCol = modfid.Split(',')[1];
                                        modfid = modfid.Split(',')[0];
                                    }
                                    string sql = null;
                                    bool ColHasAdded = false;
                                    string OpDefaultCol_colval = "";
                                    if (OpDefaultCol == 1)
                                    {
                                        var dic = DBSuit.DefaultColsWhenModForPage(tablename);
                                        int loopi = 0;
                                        foreach (var dicItem in dic)
                                        {
                                            if (loopi > 0) OpDefaultCol_colval += ",";
                                            OpDefaultCol_colval += dicItem.Key + "=" + (dicItem.Value == null ? "null" : ("'" + dicItem.Value.ToString().D2() + "'"));
                                            loopi++;
                                        }
                                    }
                                    if (OpDefaultCol == 1 && OpDefaultCol_colval != "")
                                    {
                                        sql = "update " + tablename.Normal() + " set " + OpDefaultCol_colval + _flowstr;
                                        ColHasAdded = true;
                                    }
                                    else
                                    {
                                        sql = "update " + tablename.Normal() + " set ";
                                        ColHasAdded = false;
                                    }
                                    IDictionaryEnumerator TableModRowEnum = ((Hashtable)TableModFIDEnum.Value).GetEnumerator();
                                    while (TableModRowEnum.MoveNext())
                                    {
                                        if (!StayedPostedFiles.Contains(tablename + "." + TableModRowEnum.Key.ToString()) || !TableModRowEnum.Value.ToString().Equals(""))
                                        {
                                            if (ColHasAdded) sql += ",";
                                            if (TableModRowEnum.Value.ToString().Equals("[FTNULL]"))
                                                sql += func.SQLColSafe(TableModRowEnum.Key.ToString()).Normal() + "=null";
                                            else
                                                sql += func.SQLColSafe(TableModRowEnum.Key.ToString()).Normal() + "='" + str.D2DD(TableModRowEnum.Value.ToString().Equals("(delfile)") ? "" : TableModRowEnum.Value.ToString()).Trim() + "'";
                                            ColHasAdded = true;
                                        }
                                        if (!StayedPostedFiles.Contains(tablename + "." + TableModRowEnum.Key.ToString()))
                                        {
                                            //if (TableExists)
                                            //{
                                            //    string sqlele = "delete from " + tablename + "_dy where fid='" + str.D2DD(modfid) + "' and eid='" + str.D2DD(TableModRowEnum.Key.ToString()) + "'";
                                            //    db.ExecSql(sqlele, st);
                                            //}
                                        }
                                    }
                                    if (CusWhere != null) sql += " " + CusWhere;
                                    else
                                    {
                                        sql += " where " + str.D2DD(CurFidCol).Normal() + "='" + str.D2DD(modfid) + "'";
                                    }
                                    log.Debug(sql, "[Form Data Mod]");
                                    db.ExecSql(sql, st);

                                    FTFrame.Project.Core.Action.ActionSave(tablename, Context, 1);

                                    NewOPLogV4(db, st, Context, tableolog, modfid, _type, tablename, memid);
                                    if (datatype == 2)
                                    {
                                        FlowActionResult = Base.Core.FlowOld.Action(siteid, db, st, fid, flowtype, flow, flowdesignpos, flowdesign, flowdesignbaranch, BindTable, Context, tableflog, _type, memid);
                                    }
                                    TableModRowEnum = null;
                                }
                                TableModFIDEnum = null;
                            }
                            //修改动态新增行

                            //IDictionaryEnumerator TableModDyEnum = TableModDy.GetEnumerator();
                            //while (TableModDyEnum.MoveNext())
                            //{
                            //    string sql = null;
                            //    string tablenamedy = ((string[])TableModDyEnum.Key)[0];
                            //    //FileBox要保留的话，更新
                            //    string _eletype = ((string[])TableModDyEnum.Value)[1];
                            //    if (_eletype.Equals("FileBox"))
                            //    {
                            //        if (StayedPostedFiles.Contains(tablenamedy.Substring(0, tablenamedy.Length - 3) + "." + ((string[])TableModDyEnum.Key)[1]))
                            //        {
                            //            if (!((string[])TableModDyEnum.Value)[0].Equals(""))//如果有新文件上传
                            //            {
                            //                string sqlele = "delete from " + tablenamedy + " where fid='" + str.D2DD(((string[])TableModDyEnum.Key)[3]) + "' and eid='" + str.D2DD(((string[])TableModDyEnum.Key)[1]) + "' and erate=" + int.Parse(((string[])TableModDyEnum.Key)[2]);
                            //                db.ExecSql(sqlele, st);
                            //                sql = "insert into " + tablenamedy + "(fid,eid,evalue,erate)";
                            //                sql += "values('" + str.D2DD(((string[])TableModDyEnum.Key)[3]) + "','" + str.D2DD(((string[])TableModDyEnum.Key)[1]) + "','" + str.D2DD(((string[])TableModDyEnum.Value)[0]) + "'," + int.Parse(((string[])TableModDyEnum.Key)[2]) + ")";
                            //                db.ExecSql(sql, st);
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        sql = "insert into " + tablenamedy + "(fid,eid,evalue,erate)";
                            //        sql += "values('" + str.D2DD(((string[])TableModDyEnum.Key)[3]) + "','" + str.D2DD(((string[])TableModDyEnum.Key)[1]) + "'," + (((string[])TableModDyEnum.Value)[0].Equals("[FTNULL]") ? "null" : ("'" + str.D2DD(((string[])TableModDyEnum.Value)[0]) + "'")) + "," + int.Parse(((string[])TableModDyEnum.Key)[2]) + ")";
                            //        db.ExecSql(sql, st);
                            //    }
                            //}
                            //处理dy表中的(delfile)且重新对erate排序
                            //List<string> DyDelFileTagList = new List<string>();
                            //TableModDyEnum = TableModDy.GetEnumerator();
                            //while (TableModDyEnum.MoveNext())
                            //{
                            //    string sql = null;
                            //    string tag_tablenamedy = ((string[])TableModDyEnum.Key)[0];
                            //    string tag_fid = ((string[])TableModDyEnum.Key)[3];
                            //    string tag_eid = ((string[])TableModDyEnum.Key)[1];
                            //    string dytag = tag_tablenamedy + "_" + tag_fid + "_" + tag_eid;
                            //    if (!DyDelFileTagList.Contains(dytag))
                            //    {
                            //        DyDelFileTagList.Add(dytag);
                            //        sql = "delete from " + tag_tablenamedy + " where fid='" + str.D2DD(tag_fid) + "' and  eid='" + str.D2DD(tag_eid) + "' and evalue='(delfile)'";
                            //        db.ExecSql(sql, st);
                            //        List<int[]> DyDelFileErateList = new List<int[]>();
                            //        sql = "select erate from " + tag_tablenamedy + " where fid='" + str.D2DD(tag_fid) + "' and  eid='" + str.D2DD(tag_eid) + "' and evalue!='(delfile)' order by erate";
                            //        int newrate = 1;
                            //        using (DR dyDr = db.OpenRecord(sql, st))
                            //        {
                            //            while (dyDr.Read())
                            //            {
                            //                DyDelFileErateList.Add(new int[] { dyDr.GetInt32(0), newrate++ });
                            //            }
                            //        }
                            //        foreach (int[] dyDelRate in DyDelFileErateList)
                            //        {
                            //            sql = "update " + tag_tablenamedy + " set erate=" + dyDelRate[1] + " where fid='" + str.D2DD(tag_fid) + "'  and erate=" + dyDelRate[0] + " and  eid='" + str.D2DD(tag_eid) + "'";
                            //            db.ExecSql(sql, st);
                            //        }
                            //        DyDelFileErateList.Clear();
                            //    }
                            //}
                            //DyDelFileTagList.Clear();
                            //_dy表更新取消
                            //if (OpDefaultCol == 1)
                            //{
                            //    //更新dydata
                            //    TableModEnum = TableMod.GetEnumerator();
                            //    while (TableModEnum.MoveNext())
                            //    {
                            //        string tablename = TableModEnum.Key.ToString();
                            //        IDictionaryEnumerator TableModFIDEnum = ((Hashtable)TableModEnum.Value).GetEnumerator();
                            //        while (TableModFIDEnum.MoveNext())
                            //        {
                            //            string modfid = TableModFIDEnum.Key.ToString();
                            //            //得到DyData
                            //            DR drte = db.OpenRecord(Sql.TableExists(tablename + "_dy"), st);
                            //            bool TableExists = drte.HasRows;
                            //            drte.Close();
                            //            string dyDataStr = "";
                            //            string sql = null;
                            //            if (TableExists)
                            //            {
                            //                sql = "select distinct eid from " + tablename + "_dy where fid='" + str.D2DD(modfid) + "'";
                            //                DR dydr = db.OpenRecord(sql, st);
                            //                while (dydr.Read())
                            //                {
                            //                    dyDataStr += "," + dydr.GetString(0);
                            //                }
                            //                dydr.Close();
                            //                if (!dyDataStr.Equals("")) dyDataStr = dyDataStr.Substring(0);
                            //            }

                            //            sql = "update " + tablename + " set dydata='" + str.D2DD(dyDataStr) + "' where " + str.D2DD(FidCol) + "='" + str.D2DD(modfid) + "'";
                            //            db.ExecSql(sql, st);
                            //        }
                            //        TableModFIDEnum = null;
                            //    }
                            //}
                            TableModEnum = null;
                        }
                        #endregion
                        #region 多行
                        if (TableMuiltiSet.Count > 0)
                        {
                            foreach (string TableName in TableMuiltiSet.Keys)
                            {
                                if (!TableMultiCol.ContainsKey(TableName)) continue;
                                object[] MuiltiSet = TableMuiltiSet[TableName];
                                int MuitiType = (int)MuiltiSet[0];
                                string FidKey = (string)MuiltiSet[1];
                                string FilterCdn = (string)MuiltiSet[2];
                                //3 多行新增 不需要 name值 不需要cdn
                                //4 多行重置 不需要 name值  需要cdn
                                //5 多行重置保留FID  需要 name值 需要cdn
                                //6 多行仅更新 需要name值 不需要cdn
                                int RowRate;
                                string sql;
                                string sqlCol;
                                string sqlVal;
                                if (MuitiType == 3)//多行新增 
                                {
                                    bool IsDelRow1 = (Context.Request.Form["Row1DeleteHdn"].FirstOrDefault<string>() != null && Context.Request.Form["Row1DeleteHdn"].FirstOrDefault<string>() == "yes");//删除首行标记
                                    RowRate = IsDelRow1 ? 1 : 0;
                                    while (TableMultiCol[TableName].ContainsKey(RowRate))
                                    {
                                        bool colAdded = false;
                                        sqlCol = "insert into " + func.SQLColSafe(TableName).Normal() + "(";
                                        sqlVal = "values(";
                                        var IsKeyColInSql = DBSuit.Key(TableName, FidCol2).KeyType != Enums.KeyType.AutoIncrement;
                                        if (IsKeyColInSql)
                                        {
                                            sqlCol += str.D2DD(DBSuit.Key(TableName, FidCol2).KeyName);
                                            sqlVal += "'" + DBSuit.KeyGenerate(TableName).ToString() + "'";
                                            colAdded = true;
                                        }
                                        //流水号
                                        foreach (string[] liquiditem in LiquidAL)
                                        {
                                            if (TableName.Equals(liquiditem[0]))
                                            {
                                                string liquid_id = Advance.GetLiquidID(db, st, liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                                int liquid_loop = 0;
                                                while (!Advance.IsLiquidIDOK(db, st, liquiditem[0], liquiditem[1], liquid_id))
                                                {
                                                    if (liquid_loop > 99)
                                                    {
                                                        liquid_id = "[error]loop99";
                                                        break;
                                                    }
                                                    liquid_loop++;
                                                    liquid_id = Advance.GetLiquidID(db, st, liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                                }
                                                if (colAdded)
                                                {
                                                    sqlCol += "," + func.SQLColSafe(liquiditem[1]).Normal();
                                                    sqlVal += "," + "'" + str.D2DD(liquid_id) + "'";
                                                }
                                                else
                                                {
                                                    sqlCol +=   func.SQLColSafe(liquiditem[1]).Normal();
                                                    sqlVal +=   "'" + str.D2DD(liquid_id) + "'";
                                                    colAdded = true;
                                                }
                                            }
                                        }
                                        //col value
                                        foreach (string col in TableMultiCol[TableName][RowRate].Keys)
                                        {
                                            string val = TableMultiCol[TableName][RowRate][col];
                                            if (colAdded)
                                            {
                                                sqlCol += "," + func.SQLColSafe(col).Normal();
                                                sqlVal += "," + ((val == null || val.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(val).Trim() + "'"));
                                            }
                                            else
                                            {
                                                sqlCol +=  func.SQLColSafe(col).Normal();
                                                sqlVal +=  ((val == null || val.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(val).Trim() + "'"));
                                                colAdded = true;
                                            }
                                        }
                                        sqlCol += ")";
                                        sqlVal += ")";
                                        log.Debug(sqlCol + sqlVal, "[Form Data Muilti " + MuitiType + "]");
                                        db.ExecSql(sqlCol + sqlVal, st);
                                        RowRate++;
                                    }
                                }
                                else if (MuitiType == 4 && FilterCdn != "")//多行重置 
                                {
                                    sql = "delete from " + func.SQLColSafe(TableName).Normal() + " where " + FilterCdn;
                                    log.Debug(sql, "[Form Data Muilti " + MuitiType + "]");
                                    db.ExecSql(sql, st);
                                    bool IsDelRow1 = (Context.Request.Form["Row1DeleteHdn"].FirstOrDefault<string>() != null && Context.Request.Form["Row1DeleteHdn"].FirstOrDefault<string>() == "yes");//删除首行标记
                                    RowRate = IsDelRow1 ? 1 : 0;
                                    while (TableMultiCol[TableName].ContainsKey(RowRate))
                                    {
                                        bool colAdded = false;
                                        sqlCol = "insert into " + func.SQLColSafe(TableName).Normal() + "(";
                                        sqlVal = "values(";
                                        var IsKeyColInSql = DBSuit.Key(TableName, FidCol2).KeyType != Enums.KeyType.AutoIncrement;
                                        if (IsKeyColInSql)
                                        {
                                            sqlCol += str.D2DD(DBSuit.Key(TableName, FidCol2).KeyName);
                                            sqlVal += "'" + DBSuit.KeyGenerate(TableName).ToString() + "'";
                                            colAdded = true;
                                        }
                                        //流水号
                                        foreach (string[] liquiditem in LiquidAL)
                                        {
                                            if (TableName.Equals(liquiditem[0]))
                                            {
                                                string liquid_id = Advance.GetLiquidID(db, st, liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                                int liquid_loop = 0;
                                                while (!Advance.IsLiquidIDOK(db, st, liquiditem[0], liquiditem[1], liquid_id))
                                                {
                                                    if (liquid_loop > 99)
                                                    {
                                                        liquid_id = "[error]loop99";
                                                        break;
                                                    }
                                                    liquid_loop++;
                                                    liquid_id = Advance.GetLiquidID(db, st, liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                                }
                                                if (colAdded)
                                                {
                                                    sqlCol += "," + func.SQLColSafe(liquiditem[1]).Normal();
                                                    sqlVal += "," + "'" + str.D2DD(liquid_id) + "'";
                                                }
                                                else
                                                {
                                                    sqlCol +=   func.SQLColSafe(liquiditem[1]).Normal();
                                                    sqlVal +=   "'" + str.D2DD(liquid_id) + "'";
                                                    colAdded = true;
                                                }
                                            }
                                        }
                                        //col value
                                        foreach (string col in TableMultiCol[TableName][RowRate].Keys)
                                        {
                                            string val = TableMultiCol[TableName][RowRate][col];
                                            if (colAdded)
                                            {
                                                sqlCol += "," + func.SQLColSafe(col).Normal();
                                                sqlVal += "," + ((val == null || val.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(val).Trim() + "'"));
                                            }
                                            else
                                            {
                                                sqlCol +=   func.SQLColSafe(col).Normal();
                                                sqlVal +=   ((val == null || val.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(val).Trim() + "'"));
                                                colAdded = true;
                                            }
                                        }
                                        sqlCol += ")";
                                        sqlVal += ")";
                                        log.Debug(sqlCol + sqlVal, "[Form Data Muilti " + MuitiType + "]");
                                        db.ExecSql(sqlCol + sqlVal, st);
                                        RowRate++;
                                    }
                                }
                                else if (MuitiType == 5 && FidKey != "" && FilterCdn != "")//多行重置保留FID 
                                {
                                    sql = "delete from " + func.SQLColSafe(TableName).Normal() + " where " + FilterCdn;
                                    log.Debug(sql, "[Form Data Muilti " + MuitiType + "]");
                                    db.ExecSql(sql, st);
                                    bool IsDelRow1 = (Context.Request.Form["Row1DeleteHdn"].FirstOrDefault<string>() != null && Context.Request.Form["Row1DeleteHdn"].FirstOrDefault<string>() == "yes");//删除首行标记
                                    RowRate = IsDelRow1 ? 1 : 0;
                                    while (TableMultiCol[TableName].ContainsKey(RowRate))
                                    {
                                        bool colAdded = false;
                                        sqlCol = "insert into " + func.SQLColSafe(TableName).Normal() + "(";
                                        sqlVal = "values(";
                                        string oriFid = Context.Request.Form[FidKey + (RowRate == 0 ? "" : ("_rowrate" + RowRate))];
                                        if (!string.IsNullOrWhiteSpace(oriFid))
                                        {
                                            sqlCol += str.D2DD(DBSuit.Key(TableName, FidCol2).KeyName);
                                            sqlVal += "'" + str.D2DD(oriFid) + "'";
                                            colAdded = true;
                                        }
                                        else
                                        {
                                            var IsKeyColInSql = DBSuit.Key(TableName, FidCol2).KeyType != Enums.KeyType.AutoIncrement;
                                            if(IsKeyColInSql)
                                            {
                                                sqlCol += str.D2DD(DBSuit.Key(TableName, FidCol2).KeyName);
                                                sqlVal += "'" + DBSuit.KeyGenerate(TableName).ToString() + "'";
                                                colAdded = true;
                                            }
                                        }

                                        //流水号
                                        foreach (string[] liquiditem in LiquidAL)
                                        {
                                            if (TableName.Equals(liquiditem[0]))
                                            {
                                                string liquid_id = Advance.GetLiquidID(db, st, liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                                int liquid_loop = 0;
                                                while (!Advance.IsLiquidIDOK(db, st, liquiditem[0], liquiditem[1], liquid_id))
                                                {
                                                    if (liquid_loop > 99)
                                                    {
                                                        liquid_id = "[error]loop99";
                                                        break;
                                                    }
                                                    liquid_loop++;
                                                    liquid_id = Advance.GetLiquidID(db, st, liquiditem[0], liquiditem[1], liquiditem[2], liquiditem[3]);
                                                }
                                                if (colAdded)
                                                {
                                                    sqlCol += "," + func.SQLColSafe(liquiditem[1]).Normal();
                                                    sqlVal += "," + "'" + str.D2DD(liquid_id) + "'";
                                                }
                                                else
                                                {
                                                    sqlCol += func.SQLColSafe(liquiditem[1]).Normal();
                                                    sqlVal +=  "'" + str.D2DD(liquid_id) + "'";
                                                    colAdded = true;
                                                }
                                            }
                                        }
                                        //col value
                                        foreach (string col in TableMultiCol[TableName][RowRate].Keys)
                                        {
                                            string val = TableMultiCol[TableName][RowRate][col];
                                            if (colAdded)
                                            {
                                                sqlCol += "," + func.SQLColSafe(col).Normal();
                                                sqlVal += "," + ((val == null || val.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(val).Trim() + "'"));
                                            }
                                            else
                                            {
                                                sqlCol +=   func.SQLColSafe(col).Normal();
                                                sqlVal +=   ((val == null || val.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(val).Trim() + "'"));
                                                colAdded = true;
                                            }
                                        }
                                        sqlCol += ")";
                                        sqlVal += ")";
                                        log.Debug(sqlCol + sqlVal, "[Form Data Muilti " + MuitiType + "]");
                                        db.ExecSql(sqlCol + sqlVal, st);
                                        RowRate++;
                                    }
                                }
                                else if (MuitiType == 6 && FidKey != "")//多行仅更新 
                                {
                                    RowRate = 0;
                                    while (TableMultiCol[TableName].ContainsKey(RowRate))
                                    {
                                        string oriFid = Context.Request.Form[FidKey + (RowRate == 0 ? "" : ("_rowrate" + RowRate))];
                                        sql = "update " + func.SQLColSafe(TableName).Normal() + " set ";
                                        //col value
                                        bool IsFirst = true;
                                        foreach (string col in TableMultiCol[TableName][RowRate].Keys)
                                        {
                                            string val = TableMultiCol[TableName][RowRate][col];
                                            if (!IsFirst) sql += ",";
                                            sql += func.SQLColSafe(col).Normal() + "=" + ((val == null || val.Equals("[FTNULL]")) ? "null" : ("'" + str.D2DD(val).Trim() + "'"));
                                            IsFirst = false;
                                        }
                                        sql += " where " + str.D2DD(DBSuit.Key(TableName, FidCol2).KeyName) + "='" + str.D2DD(oriFid) + "'";
                                        log.Debug(sql, "[Form Data Muilti " + MuitiType + "]");
                                        db.ExecSql(sql, st);
                                        RowRate++;
                                    }
                                }
                            }
                        }
                        #endregion
                        #region Json
                        foreach (Model.DPData Data in DataList)
                        {
                            string jsondataopResult = Api.DataOPJsonDataOP(Context, Data, db, st, FirstModFid ?? _newfid_, siteid, RootKeyValue, TableAdd, null,null,(true,true,false));
                            if (jsondataopResult != null)
                            {
                                st.Rollback();
                                Response.WriteAsync(str.JavascriptLabel("parent._loading2fai(\"" + jsondataopResult + "\");"));
                                return;
                            }
                        }
                        #endregion
                        #endregion
                    }

                    TableAdd.Clear();
                    TableAdd = null;
                    TableAddDy.Clear();
                    TableAddDy = null;
                    TableAddFid.Clear();
                    TableAddFid = null;
                    TableMod.Clear();
                    TableMod = null;
                    TableModDy.Clear();
                    TableModDy = null;
                    TableMultiCol.Clear();
                    TableMultiCol = null;
                    TableMuiltiSet.Clear();
                    TableMuiltiSet = null;
                    StayedPostedFiles.Clear();
                    StayedPostedFiles = null;
                    LiquidAL.Clear();
                    LiquidAL = null;
                    RootKeyValue.Clear();
                    RootKeyValue = null;
                }
                else if (datatype == 1)//flow
                {
                    FlowActionResult = Base.Core.FlowOld.Action(siteid, db, st, defaultfid, flowtype, flowstat, flowdesignpos, flowdesign, flowdesignbaranch, BindTable, Context, tableflog, 4, memid);
                    FTFrame.Project.Core.Action.ActionSave(BindTable, Context, 3, flowstat);
                }
                #region Result
                if (FlowActionResult != null)
                {
                    st.Rollback();
                    Response.WriteAsync(str.JavascriptLabel("parent._loading2fai(\"" + FlowActionResult + "\");"));
                }
                else
                {
                    if (!jssuc.Equals(""))
                    {
                        if (jssuc.StartsWith("js:"))
                        {
                            Response.WriteAsync(str.JavascriptLabel(jssuc.Substring(3).Replace("@newfid@", _newfid_).Replace("_newfid_", _newfid_)));
                        }
                        else
                        {
                            Response.WriteAsync(str.JavascriptLabel("parent._loading2suc(\"" + jssuc.Replace("@newfid@", _newfid_).Replace("_newfid_", _newfid_).Replace("\r\n", ";").Replace("\"", "\\\"") + "\");"));
                            //Response.WriteAsync(str.JavascriptLabel("parent._loading2suc();" + jssuc.Replace("@newfid@", _newfid_)));
                        }
                    }
                    else
                    {
                        Response.WriteAsync(str.JavascriptLabel("parent._loading2suc();"));
                    }

                    FTFrame.Server.Core.Page.FormSqlExec(db, st, execsqlafter, execsqlafterevals, _newfid_, Context);

                    st.Commit();
                    if (codeafter.StartsWith("@code("))
                    {
                        codeafter = codeafter.Replace("_newfid_", _newfid_).Replace("@newfid@", _newfid_);
                        string[] codeafteritem = codeafter.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string codei in codeafteritem)
                        {
                            string codeafterreval = Interface.Code.Get(codei, Context);
                            if (codeafterreval != null)
                            {
                                Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('" + codeafterreval + "');"));
                                return;
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                st.Rollback();
                Response.WriteAsync(str.JavascriptLabel("parent._loading2fai(\"" + ex.Message + "\");"));
                log.Error(ex);
            }
            finally
            {
                db.Close();
            }
        }
        private static string DataOPRootValue(string text,  HttpRequest req, bool noDot = false)
        {
            if (req.HasFormContentType)
            {
                Regex r = new Regex(@"@keyValue\([\w\.]*\)");
                MatchCollection mc = r.Matches(text);
                foreach (Match m in mc)
                {
                    string key = m.Value.Replace("@keyValue(", "").Replace(")", "");
                    string val = req.Form[key].FirstOrDefault<string>()?.Trim() ?? "";
                    text = text.Replace(m.Value, noDot ? val.Replace("'", "") : val);
                }
            }
            return text;
        }
        public static void NewFLLogV4(DB db, ST st, HttpContext context, string tablelog, string fid, int ftype, string binddata, string fmem, int fvalue, int fpos)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fid) || fid == "NullKeyValue") fid = FTFrame.Tool.str.GetCombID();
                using (DB dbFtdp = new DB(SysConst.ConnectionStr_FTDP))
                {
                    string sql = "insert into " + tablelog + "(fid,ftype,binddata,fmem,addtime,fvalue,fpos)";
                    sql += "values('" + fid + "'," + ftype + ",'" + str.D2DD(binddata) + "','" + str.D2DD(fmem) + "','" + str.GetDateTime() + "'," + fvalue + "," + fpos + ")";
                    //log.Debug(sql, "[Form Flow Log]");
                    dbFtdp.ExecSql(sql);
                }
            }
            catch (Exception ex) { log.Error(ex); }
        }
        public static void NewOPLogV4(DB db, ST st, HttpContext context, string tablelog, string fid, int ftype, string binddata, string fmem)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fid) || fid == "NullKeyValue") fid = FTFrame.Tool.str.GetCombID();
                using(DB dbFtdp=new DB(SysConst.ConnectionStr_FTDP))
                {
                    string sql = "insert into " + tablelog + "(fid,ftype,binddata,fmem,addtime)";
                    sql += "values('" + (fid) + "'," + ftype + ",'" + str.D2DD((string.IsNullOrWhiteSpace(binddata) ? "-1" : binddata)) + "','" + str.D2DD((string.IsNullOrWhiteSpace(fmem) ? "-1" : fmem)) + "','" + str.GetDateTime() + "')";
                    //log.Debug(sql, "[Form Operation Log]");
                    dbFtdp.ExecSql(sql);
                }
            }
            catch (Exception ex) { log.Error(ex); }
        }
    }
}
