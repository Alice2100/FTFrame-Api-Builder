using System;
using FTDPClient.consts;
using FTDPClient.functions;
using System.Data.OleDb;
using FTDPClient.database;
using FTDPClient.forms;
using System.Windows.Forms;
using mshtml;
using FTDPClient.Page;
using System.Text;
using System.IO;
using FTDPClient.Compression;
using System.Net;
using System.Xml;
using FTDPClient.ftplib;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using FTDPClient.Obj;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;

namespace FTDPClient.RuleClass
{
    //通过反射调用

    //表 存在 列表
    public class Table_Exist_List : IRule
    {
        public RuleInformation Information()
        {
            return new RuleInformation()
            {
                Caption = res.anew.str("RuleC.cunzai"),
                Type = "Table",
                ApiType = ApiType.LIST,
                Rank = 1,
            };
        }

        public RuleResult Result(params object[] paras)
        {
            //Table_Exist_List[##]存在[##][##]
            //var remoteDBType = paras[1].ToString();
            //var remoteConnection = paras[2].ToString();
            var rulePara = paras[0] as RuleResult;
            var ruleItems = rulePara.RuleAtomValue.Split(new string[] { "[##]" }, StringSplitOptions.None);
            if (ruleItems[1] != Information().Caption) return null;
            string CusSQL = FTDP.Util.BugCheck.getPartNodeValue(rulePara.PartXml, "CusSQL");
            if (CusSQL.IndexOf("select", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                if (CusSQL.IndexOf(rulePara.RuleTableBind, StringComparison.CurrentCultureIgnoreCase) < 0)
                {
                    rulePara.Message = res.anew.str("RuleC.zidingyisql1") +" " + rulePara.RuleTableBind;
                    return rulePara;
                }
            }
            return null;
        }
    }
    //表 存在 获取
    public class Table_Exist_DyValue : IRule
    {
        public RuleInformation Information()
        {
            return new RuleInformation()
            {
                Caption = res.anew.str("RuleC.cunzai"),
                Type = "Table",
                ApiType = ApiType.DYVALUE,
                Rank = 1,
            };
        }

        public RuleResult Result(params object[] paras)
        {
            //Table_Exist_DyValue[##]存在[##][##]
            var rulePara = paras[0] as RuleResult;
            var ruleItems = rulePara.RuleAtomValue.Split(new string[] { "[##]" }, StringSplitOptions.None);
            if (ruleItems[1] != Information().Caption) return null;
            var rows = RuleFunction.RowAll_DyValue(rulePara.PartXml);
            var item = rows.Where(r => r.BindData != null && r.BindData.Trim().StartsWith("@" + rulePara.RuleTableBind, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (item.BindData == null)
            {
                rulePara.Message = res.anew.str("RuleC.peizhibucunzai") + " " + rulePara.RuleTableBind;
                return rulePara;
            }
            return null;
        }
    }
    //表 存在 操作
    public class Table_Exist_DataOP : IRule
    {
        public RuleInformation Information()
        {
            return new RuleInformation()
            {
                Caption = res.anew.str("RuleC.cunzai"),
                Type = "Table",
                ApiType = ApiType.DATAOP,
                Rank = 1,
            };
        }

        public RuleResult Result(params object[] paras)
        {
            //Table_Exist_DataOP[##]存在[##][##]
            var rulePara = paras[0] as RuleResult;
            var ruleItems = rulePara.RuleAtomValue.Split(new string[] { "[##]" }, StringSplitOptions.None);
            if (ruleItems[1] != Information().Caption) return null;
            var rows = RuleFunction.RowAll_DataOP(rulePara.PartXml);
            var item = rows.Where(r => r.BindData != null && r.BindData.Trim().StartsWith("@" + rulePara.RuleTableBind, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (item.BindData == null)
            {
                rulePara.Message = res.anew.str("RuleC.peizhibucunzai") + " " + rulePara.RuleTableBind;
                return rulePara;
            }
            return null;
        }
    }
    //表 新增 操作
    public class Table_Add_DataOP : IRule
    {
        public RuleInformation Information()
        {
            return new RuleInformation()
            {
                Caption = res.anew.str("RuleC.Addnew"),
                Type = "Table",
                ApiType = ApiType.DATAOP,
                Rank = 2,
            };
        }

        public RuleResult Result(params object[] paras)
        {
            //Table_Add_DataOP[##]新增[##][##]
            var rulePara = paras[0] as RuleResult;
            var ruleItems = rulePara.RuleAtomValue.Split(new string[] { "[##]" }, StringSplitOptions.None);
            if (ruleItems[1] != Information().Caption) return null;
            var rows = RuleFunction.RowAll_DataOP(rulePara.PartXml);
            var items = rows.Where(r => r.BindData != null && r.BindData.Trim().StartsWith("@" + rulePara.RuleTableBind, StringComparison.CurrentCultureIgnoreCase));
            if (items.Count() == 0)
            {
                rulePara.Message = res.anew.str("RuleC.peizhibucunzai")+ " " + rulePara.RuleTableBind;
                return rulePara;
            }
            if (items.Where(r => r.Type == "0").Count() == 0)
            {
                rulePara.Message = res.anew.str("RuleC.peizhizhongbucunzai2") + " " + rulePara.RuleTableBind;
                return rulePara;
            }
            return null;
        }
    }
    //表 修改 操作
    public class Table_Mod_DataOP : IRule
    {
        public RuleInformation Information()
        {
            return new RuleInformation()
            {
                Caption = res.anew.str("RuleC.xiugai"),
                Type = "Table",
                ApiType = ApiType.DATAOP,
                Rank = 3,
            };
        }

        public RuleResult Result(params object[] paras)
        {
            //Table_Mod_DataOP[##]修改[##][##]
            var rulePara = paras[0] as RuleResult;
            var ruleItems = rulePara.RuleAtomValue.Split(new string[] { "[##]" }, StringSplitOptions.None);
            if (ruleItems[1] != Information().Caption) return null;
            var rows = RuleFunction.RowAll_DataOP(rulePara.PartXml);
            var items = rows.Where(r => r.BindData != null && r.BindData.Trim().StartsWith("@" + rulePara.RuleTableBind, StringComparison.CurrentCultureIgnoreCase));
            if (items.Count() == 0)
            {
                rulePara.Message = res.anew.str("RuleC.peizhibucunzai") + " " + rulePara.RuleTableBind;
                return rulePara;
            }
            if (items.Where(r => r.Type == "1").Count() == 0)
            {
                rulePara.Message = res.anew.str("RuleC.peizhizhongbucunzai3") + " " + rulePara.RuleTableBind;
                return rulePara;
            }
            return null;
        }
    }
    //表 同步 操作
    public class Table_Auto_DataOP : IRule
    {
        public RuleInformation Information()
        {
            return new RuleInformation()
            {
                Caption = res.anew.str("RuleC.tongbu"),
                Type = "Table",
                ApiType = ApiType.DATAOP,
                Rank = 4,
            };
        }

        public RuleResult Result(params object[] paras)
        {
            //Table_Auto_DataOP[##]同步[##][##]
            var rulePara = paras[0] as RuleResult;
            var ruleItems = rulePara.RuleAtomValue.Split(new string[] { "[##]" }, StringSplitOptions.None);
            if (ruleItems[1] != Information().Caption) return null;
            var rows = RuleFunction.RowAll_DataOP(rulePara.PartXml);
            var items = rows.Where(r => r.BindData != null && r.BindData.Trim().StartsWith("@" + rulePara.RuleTableBind, StringComparison.CurrentCultureIgnoreCase));
            if (items.Count() == 0)
            {
                rulePara.Message = res.anew.str("RuleC.peizhibucunzai") + " " + rulePara.RuleTableBind;
                return rulePara;
            }
            if (items.Where(r => r.Type == "13").Count() == 0)
            {
                rulePara.Message = res.anew.str("Rule.peizhibucunzai4") +" " + rulePara.RuleTableBind;
                return rulePara;
            }
            return null;
        }
    }
    //字段 存在 列表
    public class Column_Exist_List : IRule
    {
        public RuleInformation Information()
        {
            return new RuleInformation()
            {
                Caption = res.anew.str("RuleC.cunzai"),
                Type = "Column",
                ApiType = ApiType.LIST,
                Rank = 1,
            };
        }

        public RuleResult Result(params object[] paras)
        {
            //Column_Exist_List[##]存在[##][##]
            var rulePara = paras[0] as RuleResult;
            var ruleItems = rulePara.RuleAtomValue.Split(new string[] { "[##]" }, StringSplitOptions.None);
            if (ruleItems[1] != Information().Caption) return null;
            var rows = RuleFunction.RowAll_List(rulePara.PartXml);
            var item = rows.Where(r => r.Data != null && r.Data.Trim().Equals(rulePara.RuleColumnBind, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (item.Data == null)
            {
                rulePara.Message = res.anew.str("RuleC.listnotdefine") + " " + rulePara.RuleColumnBind;
                return rulePara;
            }

            return null;
        }
    }
    //字段 存在 获取
    public class Column_Exist_DyValue : IRule
    {
        public RuleInformation Information()
        {
            return new RuleInformation()
            {
                Caption = res.anew.str("RuleC.cunzai"),
                Type = "Column",
                ApiType = ApiType.DYVALUE,
                Rank = 1,
            };
        }

        public RuleResult Result(params object[] paras)
        {
            //Column_Exist_DyValue[##]存在[##][##]
            var rulePara = paras[0] as RuleResult;
            var ruleItems = rulePara.RuleAtomValue.Split(new string[] { "[##]" }, StringSplitOptions.None);
            if (ruleItems[1] != Information().Caption) return null;
            var rows = RuleFunction.RowAll_DyValue(rulePara.PartXml);
            var items = rows.Where(r => r.BindData != null && r.BindData.Trim().StartsWith("@" + rulePara.RuleTableBind, StringComparison.CurrentCultureIgnoreCase));
            if (items.Count() == 0)
            {
                rulePara.Message = res.anew.str("RuleC.peizhibucunzai") + " " + rulePara.RuleTableBind;
                return rulePara;
            }
            if (items.Where(r => r.BindData.EndsWith("." + rulePara.RuleColumnBind, StringComparison.CurrentCultureIgnoreCase)).Count() == 0)
            {
                rulePara.Message = res.anew.str("Rule.peizhibucunzai5") + " " + rulePara.RuleColumnBind + " in " + rulePara.RuleTableBind;
                return rulePara;
            }
            return null;
        }
    }
    //字段 存在 操作
    public class Column_Exist_DataOP : IRule
    {
        public RuleInformation Information()
        {
            return new RuleInformation()
            {
                Caption = res.anew.str("RuleC.cunzai"),
                Type = "Column",
                ApiType = ApiType.DATAOP,
                Rank = 1,
            };
        }

        public RuleResult Result(params object[] paras)
        {
            //Column_Exist_DataOP[##]存在[##][##]
            var rulePara = paras[0] as RuleResult;
            var ruleItems = rulePara.RuleAtomValue.Split(new string[] { "[##]" }, StringSplitOptions.None);
            if (ruleItems[1] != Information().Caption) return null;
            var rows = RuleFunction.RowAll_DataOP(rulePara.PartXml);
            var items = rows.Where(r => r.BindData != null && r.BindData.Trim().StartsWith("@" + rulePara.RuleTableBind, StringComparison.CurrentCultureIgnoreCase));
            if (items.Count() == 0)
            {
                rulePara.Message = res.anew.str("RuleC.peizhibucunzai") + " " + rulePara.RuleTableBind;
                return rulePara;
            }
            if (items.Where(r => r.BindData.EndsWith("." + rulePara.RuleColumnBind, StringComparison.CurrentCultureIgnoreCase)).Count() == 0)
            {
                rulePara.Message = res.anew.str("Rule.peizhibucunzai5") + " " + rulePara.RuleColumnBind + " in " + rulePara.RuleTableBind;
                return rulePara;
            }
            return null;
        }
    }
    //字段 校验 操作
    public class Column_Check_DataOP : IRule
    {
        public RuleInformation Information()
        {
            return new RuleInformation()
            {
                Caption = res.anew.str("Rule.jiaoyan"),
                Type = "Column",
                ApiType = ApiType.DATAOP,
                Rank = 2,
                SubRules = new List<string[]>() {
                    new string[]{ "",res.anew.str("Rule.wu")},
                    new string[]{ "noempty", res.anew.str("Rule.jiaoyan1") },
                    new string[]{ "int", res.anew.str("Rule.jiaoyan2")},
                    new string[]{ "decimal", res.anew.str("Rule.jiaoyan3")},
                    new string[]{ "date", res.anew.str("Rule.jiaoyan4")},
                },
            };
        }

        public RuleResult Result(params object[] paras)
        {
            //Column_Check_DataOP[##]校验[##]noempty[##]
            var rulePara = paras[0] as RuleResult;
            var ruleItems = rulePara.RuleAtomValue.Split(new string[] { "[##]" }, StringSplitOptions.None);
            if (ruleItems[1] != Information().Caption) return null;
            var rows = RuleFunction.RowAll_DataOP(rulePara.PartXml);
            var items = rows.Where(r => r.Id != null && (r.Id.Trim() == rulePara.RuleColumnBind || r.Id.Trim() == "_" + rulePara.RuleColumnBind));
            if (items.Count() == 0)
            {
                rulePara.Message = res.anew.str("RuleC.peizhibucunzaizid") + " " + rulePara.RuleColumnBind;
                return rulePara;
            }
            if (ruleItems[2] != "")
            {
                if (items.Where(r => r.Validate != null && r.Validate.IndexOf(ruleItems[2]) >= 0).Count() == 0)
                {
                    rulePara.Message = res.anew.str("RuleC.myustvalidate") + " " + ruleItems[2] + " in " + rulePara.RuleColumnBind;
                    return rulePara;
                }
            }
            return null;
        }
    }
    //字段 定值 操作
    public class Column_Value_DataOP : IRule
    {
        public RuleInformation Information()
        {
            return new RuleInformation()
            {
                Caption = res.anew.str("RuleC.dingzhi"),
                Type = "Column",
                ApiType = ApiType.DATAOP,
                Rank = 3,
                IsText = true,
            };
        }

        public RuleResult Result(params object[] paras)
        {
            //Column_Value_DataOP[##]定值[##][##]111
            var rulePara = paras[0] as RuleResult;
            var ruleItems = rulePara.RuleAtomValue.Split(new string[] { "[##]" }, StringSplitOptions.None);
            if (ruleItems[1] != Information().Caption) return null;
            var rows = RuleFunction.RowAll_DataOP(rulePara.PartXml);
            var items = rows.Where(r => r.Id != null && (r.Id.Trim() == rulePara.RuleColumnBind || r.Id.Trim() == "_" + rulePara.RuleColumnBind));
            if (items.Count() == 0)
            {
                rulePara.Message = res.anew.str("RuleC.peizhibucunzaizid") + " " + rulePara.RuleColumnBind;
                return rulePara;
            }
            if (items.Where(r => r.Advance != null && r.Advance.Trim() == ruleItems[3]).Count() == 0)
            {
                rulePara.Message = res.anew.str("RuleC.mustdingzhi") + " " + ruleItems[3] + " in " + rulePara.RuleColumnBind;
                return rulePara;
            }
            return null;
        }
    }
    //字段 带计算 获取
    public class Column_Cacu_DyValue : IRule
    {
        public RuleInformation Information()
        {
            return new RuleInformation()
            {
                Caption = res.anew.str("RuleC.daijisuan"),
                Type = "Column",
                ApiType = ApiType.DYVALUE,
                Rank = 4,
            };
        }

        public RuleResult Result(params object[] paras)
        {
            //Column_Cacu_DyValue[##]带计算[##][##]
            var rulePara = paras[0] as RuleResult;
            var ruleItems = rulePara.RuleAtomValue.Split(new string[] { "[##]" }, StringSplitOptions.None);
            if (ruleItems[1] != Information().Caption) return null;
            var rows = RuleFunction.RowAll_DyValue(rulePara.PartXml);
            var items = rows.Where(r => r.Id != null && (r.Id.Trim() == rulePara.RuleColumnBind || r.Id.Trim() == "_" + rulePara.RuleColumnBind));
            if (items.Count() == 0)
            {
                rulePara.Message = res.anew.str("RuleC.peizhibucunzaizid") + " " + rulePara.RuleColumnBind;
                return rulePara;
            }
            if (items.Where(r => r.Advance != null && r.Advance.Trim() != "").Count() == 0)
            {
                rulePara.Message = res.anew.str("RuleC.mustjisuan") + " " + rulePara.RuleTableBind;
                return rulePara;
            }
            return null;
        }
    }
    //字段 带计算 操作
    public class Column_Cacu_DataOP : IRule
    {
        public RuleInformation Information()
        {
            return new RuleInformation()
            {
                Caption = res.anew.str("RuleC.daijisuan"),
                Type = "Column",
                ApiType = ApiType.DATAOP,
                Rank = 4,
            };
        }

        public RuleResult Result(params object[] paras)
        {
            //Column_Cacu_DataOP[##]带计算[##][##]
            var rulePara = paras[0] as RuleResult;
            var ruleItems = rulePara.RuleAtomValue.Split(new string[] { "[##]" }, StringSplitOptions.None);
            if (ruleItems[1] != Information().Caption) return null;
            var rows = RuleFunction.RowAll_DataOP(rulePara.PartXml);
            var items = rows.Where(r => r.Id != null && (r.Id.Trim() == rulePara.RuleColumnBind || r.Id.Trim() == "_" + rulePara.RuleColumnBind));
            if (items.Count() == 0)
            {
                rulePara.Message = res.anew.str("RuleC.peizhibucunzaizid") + " " + rulePara.RuleColumnBind;
                return rulePara;
            }
            if (items.Where(r => r.Advance != null && r.Advance.Trim() != "").Count() == 0)
            {
                rulePara.Message = res.anew.str("RuleC.mustjisuan") + " " + rulePara.RuleTableBind;
                return rulePara;
            }
            return null;
        }
    }
    public interface IRule
    {
        RuleInformation Information();
        RuleResult Result(params object[] paras);
    }
}

public class RuleFunction
{
    public static void Check(DataGridView dataGridView, Label label)
    {
        try
        {
            var labelOri = label.Text;
            var localSiteConnection = db.ConnStr_Site();
            var remoteDBType = Options.GetSystemDBSetType();
            var remoteConnection = Options.GetSystemDBSetConnStr();
            var remoteDBType_Plat = Options.GetSystemDBSetType_Plat();
            var remoteConnection_Plat = Options.GetSystemDBSetConnStr_Plat();
            string sql = null;
            sql = "select a.*,b.apipath,b.caption as dircaption from ft_rule_item a inner join ft_rule_dir b on a.pid=b.id where a.ruletype='table' and a.bindtable!='' and b.apipath!=''  and b.apipath is not null";
            var dtTable = Adv.RemoteSqlQuery(sql, remoteDBType_Plat, remoteConnection_Plat);
            var i = 0;
            foreach (DataRow rowTable in dtTable.Rows)
            {
                label.Text = labelOri + "     " + (++i) + "/" + dtTable.Rows.Count;
                Application.DoEvents();
                if (BugResult.@break) break;
                string apipath = rowTable["apipath"].ToString();
                string tableCaption = rowTable["caption"].ToString();
                string dirCaption = rowTable["dircaption"].ToString();
                string bindTable = rowTable["bindtable"].ToString();
                sql = "select PageID,PageCaption,ApiType,Mimo from ft_ftdp_apidoc where ApiPath='" + str.D2DD(apipath) + "'";
                string pageId = null;
                string pageCaption = null;
                string apiType = null;
                string apiCaption = null;
                var dtApiDoc = Adv.RemoteSqlQuery(sql, remoteDBType_Plat, remoteConnection_Plat);
                if (dtApiDoc.Rows.Count == 0)
                {
                    PutBug(new RuleResult()
                    {
                        ApiPath = apipath,
                        RuleDirCaption = dirCaption,
                        RuleTableCaption = tableCaption,
                        RuleTableBind = bindTable,
                        Message = res.anew.GetString("String16")
                    });
                }
                else
                {
                    pageId = dtApiDoc.Rows[0]["PageID"].ToString();
                    pageCaption = dtApiDoc.Rows[0]["PageCaption"].ToString();
                    apiType = dtApiDoc.Rows[0]["ApiType"].ToString();
                    apiCaption = dtApiDoc.Rows[0]["Mimo"].ToString();
                    string pagePath = FTDP.Util.BugCheck.getPathByPageId(pageId, localSiteConnection);
                    if (pagePath == null)
                    {
                        PutBug(new RuleResult()
                        {
                            PageId = pageId,
                            PageCaption = pageCaption,
                            PagePath = pagePath,
                            ApiType = apiType,
                            ApiCaption = apiCaption,
                            ApiPath = apipath,
                            RuleDirCaption = dirCaption,
                            RuleTableCaption = tableCaption,
                            RuleTableBind = bindTable,
                            Message = res.anew.GetString("String17")
                        });
                    }
                    else
                    {
                        //找出所有使用的组件
                        sql = "select b.name partname,c.name controlname,b.id partid,b.partxml,c.caption controlcaption from part_in_page a,parts b,controls c where a.partid=b.id and b.controlid=c.id and a.pageid='" + pageId + "'";
                        var partList = new List<(string partname, string controlname, string partid, string partxml, string controlcaption)>();
                        using (DB db = new DB())
                        {
                            db.Open(localSiteConnection);
                            using (DR dr = new DR(db.OpenRecord(sql)))
                            {
                                while (dr.Read())
                                {
                                    partList.Add((dr.getString(0), dr.getString(1), dr.getString(2), dr.getString(3), dr.getString(4)));
                                }
                            }
                        }
                        (string partname, string controlname, string partid, string partxml, string controlcaption, string apiname, string apicaption) partItem = (null, null, null, null, null, null, null);
                        var apiSet = new List<(string JsonKey, string SetKey, string Desc)>();
                        partList.ForEach(partitem =>
                        {
                            string APISet = FTDP.Util.BugCheck.getPartNodeValue(partitem.partxml, "APISet");
                            var items = APISet.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string item in items)
                            {
                                string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                                if (apipath == "/" + pagePath + "?" + colcfg[0])
                                {
                                    bool typeOK = false;
                                    if (apiType == "List" && partitem.controlname == "list" && partitem.partname == "List") typeOK = true;
                                    else if (apiType == "DyValue" && partitem.controlname == "dyvalue" && partitem.partname == "Interface") typeOK = true;
                                    else if (apiType == "DataOP" && partitem.controlname == "dataop" && partitem.partname == "Interface") typeOK = true;
                                    if (typeOK)
                                    {
                                        partItem = (partitem.partname, partitem.controlname, partitem.partid, partitem.partxml, partitem.controlcaption, colcfg[0], colcfg[1]);
                                        if (apiType == "DyValue" || apiType == "DataOP")
                                        {
                                            var jsonKeys = colcfg[2].Split(new string[] { "[#]" }, StringSplitOptions.None);
                                            var setKeys = colcfg[3].Split(new string[] { "[#]" }, StringSplitOptions.None);
                                            var descs = colcfg[4].Split(new string[] { "[#]" }, StringSplitOptions.None);
                                            for (int j = 0; j < jsonKeys.Length; j++)
                                            {
                                                apiSet.Add((jsonKeys[j], setKeys[j], descs[j]));
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        });
                        if (partItem.partname == null)
                        {
                            PutBug(new RuleResult()
                            {
                                PageId = pageId,
                                PageCaption = pageCaption,
                                PagePath = pagePath,
                                ApiType = apiType,
                                ApiCaption = apiCaption,
                                ApiPath = apipath,
                                PartId = partItem.partid,
                                PartName = partItem.partname,
                                PartXml = partItem.partxml,
                                ControlName = partItem.controlname,
                                ControlCaption = partItem.controlcaption,
                                RuleDirCaption = dirCaption,
                                RuleTableCaption = tableCaption,
                                RuleTableBind = bindTable,
                                Message = res.anew.GetString("String18")
                            });
                        }
                        else
                        {
                            var tempResult = new RuleResult()
                            {
                                PageId = pageId,
                                PageCaption = pageCaption,
                                PagePath = pagePath,
                                ApiType = apiType,
                                ApiCaption = apiCaption,
                                ApiPath = apipath,
                                PartId = partItem.partid,
                                PartName = partItem.partname,
                                PartXml = partItem.partxml,
                                ControlName = partItem.controlname,
                                ControlCaption = partItem.controlcaption,
                                RuleDirCaption = dirCaption,
                                RuleTableCaption = tableCaption,
                                RuleTableBind = bindTable,
                                ApiSet = apiSet,
                            };
                            sql = "select rule from ft_rule_atom where pid='" + rowTable["id"].ToString() + "'";
                            var dtTableRule = Adv.RemoteSqlQuery(sql, remoteDBType_Plat, remoteConnection_Plat);
                            foreach (DataRow row in dtTableRule.Rows)
                            {
                                if (BugResult.@break) break;
                                tempResult.RuleAtomValue = row["rule"].ToString();
                                CheckBug(tempResult, remoteDBType, remoteConnection);
                            }
                            sql = "select b.id,a.rule,b.caption,b.bindcolumn from ft_rule_atom a inner join ft_rule_item b on a.pid=b.id where b.ruletype='column' and b.bindcolumn!='' and b.pid='" + rowTable["id"].ToString() + "'";
                            var dtColumnRule = Adv.RemoteSqlQuery(sql, remoteDBType_Plat, remoteConnection_Plat);
                            foreach (DataRow row in dtColumnRule.Rows)
                            {
                                if (BugResult.@break) break;
                                tempResult.RuleColumnBind = row["bindcolumn"].ToString();
                                tempResult.RuleColumnCaption = row["caption"].ToString();
                                tempResult.RuleAtomValue = row["rule"].ToString();
                                CheckBug(tempResult, remoteDBType, remoteConnection);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            new error(ex);
        }
        void CheckBug(RuleResult ruleResult, globalConst.DBType remoteDBType, string remoteConnection)
        {
            var ruleItems = ruleResult.RuleAtomValue.Split(new string[] { "[##]" }, StringSplitOptions.None);
            var type = Type.GetType("FTDPClient.RuleClass." + ruleItems[0]);
            object obj = Activator.CreateInstance(type);
            MethodInfo mi = type.GetMethod("Result");
            var ret = mi.Invoke(obj, new object[] { new object[] { ruleResult, remoteDBType, remoteConnection } });
            if (ret != null)
            {
                PutBug(ret as RuleResult);
            }
        }
        void PutBug(RuleResult ruleResult)
        {
            var index = dataGridView.Rows.Add(new string[] {
                ruleResult.RuleDirCaption,
                ruleResult.PageCaption,
                ruleResult.ControlCaption+"_"+ruleResult.ApiCaption,
                ruleResult.ApiPath,
                ruleResult.Message,
                res.anew.GetString("String19"),
                res.anew.GetString("String20"),
            });
            dataGridView.Rows[index].Tag = ruleResult;
            dataGridView.Rows[index].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
        }
    }
    public static List<(string ColumnName, string Data, string Style, string OrderColumn, string Js, string Desc, string SaveTo, string OpenCondtion, string DefaultHidden)> RowAll_List(string partXml)
    {
        string RowAll = FTDP.Util.BugCheck.getPartNodeValue(partXml, "RowAll");
        string[] items = RowAll.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
        var rowAll = new List<(string ColumnName, string Data, string Style, string OrderColumn, string Js, string Desc, string SaveTo, string OpenCondtion, string DefaultHidden)>();
        foreach (string item in items)
        {
            if (item != null && item.Trim().IndexOf("&&&") >= 0)
            {
                string _item = item.Trim();
                string openclose = _item.Substring(_item.IndexOf("&&&") + 3);
                string[] colcfg = _item.Substring(0, _item.IndexOf("&&&")).Split('#');
                rowAll.Add((colcfg[0], FTDP.Util.BugCheck.getDecode(colcfg[1]), colcfg[2], colcfg[3], colcfg[4], colcfg[5], colcfg.Length < 7 ? "" : colcfg[6], openclose, colcfg.Length < 8 ? "0" : colcfg[7]));
            }
        }
        return rowAll;
    }
    public static List<(string Caption, string Id, string BindData, string KeyValue, string Async, string Multiple, string Dim, string Advance)> RowAll_DyValue(string partXml)
    {
        string Define = FTDP.Util.BugCheck.getPartNodeValue(partXml, "Define");
        var rowAll = new List<(string Caption, string Id, string BindData, string KeyValue, string Async, string Multiple, string Dim, string Advance)>();
        string[] items = Define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in items)
        {
            string[] row = item.Split(new string[] { "##" }, StringSplitOptions.None);
            rowAll.Add((
                row[0],//caption
                row[1],//id
                FTDP.Util.BugCheck.getDecode(row[2]),//binddata
                row[3],
                row[4],
                row[5],
                row[6],
                FTDP.Util.BugCheck.getDecode(row[7])
                ));
        }
        return rowAll;
    }
    public static List<(string Caption, string Name, string BindData, string Type, string Validate, string KeyValue, string Id, string Advance, string JsonForSon)> RowAll_DataOP(string partXml)
    {
        string Define = FTDP.Util.BugCheck.getPartNodeValue(partXml, "Define");
        string[] items = Define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
        var rowAll = new List<(string Caption, string Name, string BindData, string Type, string Validate, string KeyValue, string Id, string Advance, string JsonForSon)>();
        foreach (string item in items)
        {
            string[] row = item.Split(new string[] { "##" }, StringSplitOptions.None);
            rowAll.Add((
                row[0],//tip
                row[1],//name
                FTDP.Util.BugCheck.getDecode(row[2]),//binddata
                FTDP.Util.BugCheck.getDecode(row[3]),//0 Add 1 Mod
                row[4],//check
                row[5],//keyval
                row[6],//id
                row.Length < 8 ? "" : FTDP.Util.BugCheck.getDecode(row[7]),//adv
                row.Length < 9 ? "" : row[8].Replace("$#$#$", "##")//jsonForSon
                ));
        }
        return rowAll;
    }
}