using System;
using FTDPClient.consts;
using FTDPClient.functions;
using System.Data.OleDb;
using FTDPClient.database;
using FTDPClient.forms;
using System.Windows.Forms;
using FTDPClient.Page;
using System.Text;
using System.IO;
using FTDPClient.Compression;
using System.Net;
using System.Xml;
using FTDPClient.ftplib;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Data;
using System.Security.Cryptography;
using System.Windows;
using FTDPClient.Obj;
using System.Reflection;
using DocumentFormat.OpenXml.Spreadsheet;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace FTDPClient.Front
{
    /// <summary>
    /// 前端
    /// </summary>
    public class Generator
    {
        public (string ComText, string HTMLText, string PreviewHTML, string ComTextPack) List(ListCols obj)
        {
            #region ParaPattern
            foreach (var item in obj.RowsList)
            {
                item.Template = Adv.ParaPattern(item.Template);
            }
            foreach (var item in obj.SearchList)
            {
                item.InitData = Adv.ParaPattern(item.InitData);
            }
            foreach (var item in obj.ButtonList)
            {
                item.Js = Adv.ParaPattern(item.Js);
            }
            obj.InitSet= Adv.ParaPattern(obj.InitSet);
            obj.JsBeforeLoad= Adv.ParaPattern(obj.JsBeforeLoad);
            obj.JsBeforeSet= Adv.ParaPattern(obj.JsBeforeSet);
            obj.JsAfterSet= Adv.ParaPattern(obj.JsAfterSet);
            obj.CustomJs= Adv.ParaPattern(obj.CustomJs);
            obj.CssText= Adv.ParaPattern(obj.CssText);
            #endregion
            string HTMLHeader = @"<!doctype html>
<html lang='en'>
  <head>
  <meta charset='UTF-8'>
  <link rel='stylesheet' href='lib/element-ui/index.css'>
    <script src='lib/vue.js'></script>
	<script src='lib/httpVueLoader.js'></script>
	<script src='lib/element-ui/index.js'></script>
	<script src='lib/axios.min.js'></script>
	<script src='lib/config.js'></script>
	<script src='lib/base.js'></script>
    <script src='lib/en.js'></script>
	<script>ELEMENT.locale(ELEMENT.lang.en)</script>
  </head>
<body>";
            string HTMLHeaderPreview = @"<!doctype html>
<html lang='en'>
  <head>
  <meta charset='UTF-8'>
  <link rel='stylesheet' href='../lib/element-ui/index.css'>
    <script src='../lib/vue.js'></script>
	<script src='../lib/httpVueLoader.js'></script>
	<script src='../lib/element-ui/index.js'></script>
	<script src='../lib/axios.min.js'></script>
	<script src='../lib/config.js'></script>
	<script src='../lib/base.js'></script>
    <script src='../lib/en.js'></script>
	<script>ELEMENT.locale(ELEMENT.lang.en)</script>
  </head>
<body>";
            StringBuilder sbCom = new StringBuilder();
            StringBuilder sbHtml = new StringBuilder();
            StringBuilder sbPreview = new StringBuilder();
            StringBuilder sbInitData = new StringBuilder();
            StringBuilder sbOptionData = new StringBuilder();
            StringBuilder sbDocument = new StringBuilder();

            //父方法
            List<string> parentFuncList = new List<string>();
            string SearchJs = "";
            string ResetJs = "";
            sbPreview.AppendLine(HTMLHeaderPreview);
            #region Document
            var ComNames = obj.ComName.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var ComNameShort = ComNames[ComNames.Length - 1];
            var doc = DocFromCustomJs(obj.CustomJs,obj,null);
            sbDocument.AppendLine(res.ftclass.str("z021"));
            sbDocument.AppendLine("1、<"+ ComNameShort + " ref=\"" + ComNameShort + "\"/>");
            sbDocument.AppendLine("2、import " + ComNameShort + " from '@/ftdp/components/" + obj.ComName + "?'");
            sbDocument.AppendLine("3、components: { " + ComNameShort + " }");
            sbDocument.AppendLine("*/");
            sbDocument.AppendLine(res.ftclass.str("z022"));
            sbDocument.AppendLine(res.ftclass.str("z023"));
            foreach(var item in doc.EmitList)
            {
                sbDocument.AppendLine("\t"+$"{item.Emit} : {item.Desc}");
            }
            sbDocument.AppendLine("\t" + res.ftclass.str("z024"));
            sbDocument.AppendLine("\t" + res.ftclass.str("z025"));
            sbDocument.AppendLine("\t" + res.ftclass.str("z026"));
            if(doc.FuncList.Count>0)
            {
                sbDocument.AppendLine(res.ftclass.str("z027"));
                foreach (var item in doc.FuncList)
                {
                    sbDocument.AppendLine("\t" + $"{item.Func} : {item.Desc}");
                }
            }
            sbDocument.AppendLine(res.ftclass.str("z028"));
            sbDocument.AppendLine("\tload(paras)"+ res.ftclass.str("z029"));
            sbDocument.AppendLine("\tsearch()"+ res.ftclass.str("z030"));
            sbDocument.AppendLine("\treset()" + res.ftclass.str("z031"));
            sbDocument.AppendLine("\texcel(maxRow,exportName)" + res.ftclass.str("z032"));
            sbDocument.AppendLine("\tselection()" + res.ftclass.str("z033"));
            sbDocument.AppendLine("\tfileDownload(fileInfo)" + res.ftclass.str("z034"));
            sbDocument.AppendLine("\tget(apiPath, paras, callback)" + res.ftclass.str("z035"));
            sbDocument.AppendLine("\tpost(apiPath, paras, json, callback)" + res.ftclass.str("z036"));
            sbDocument.AppendLine("\tbatch(keyName, updateName, updateVal)" + res.ftclass.str("z037"));
            sbDocument.AppendLine("\tloadauto" + res.ftclass.str("z038"));
            sbDocument.Append("*/");
            #endregion
            #region Com
            StringBuilder sbTemplate = new StringBuilder();
            sbTemplate.AppendLine("<template>");
            sbTemplate.AppendLine(S(1) + "<el-container>");
            if (obj.SearchList.Count > 0 || obj.ButtonList.Count > 0)
            {
                sbTemplate.AppendLine(S(2) + "<el-header>");
                foreach (var sch in obj.SearchList)
                {
                    if(sch.InitData != null)
                    {
                        sch.InitData = FrontFunc.TempKeyPara(sch.InitData, "", sch.PlaceHolder, sch.Binding);
                    }
                    if (sch.Type == "input")
                    {
                        sbTemplate.Append(S(3) + "<el-input clearable");
                        if (!string.IsNullOrEmpty(sch.Binding)) sbTemplate.Append(" v-model=\"" + sch.Binding + "\"");
                        if (!string.IsNullOrEmpty(sch.PlaceHolder)) sbTemplate.Append(" placeholder=\"" + sch.PlaceHolder + "\"");
                        if (!string.IsNullOrEmpty(sch.Style)) sbTemplate.Append(" style=\"" + sch.Style + "\"");
                        sbTemplate.Append(">");
                        sbTemplate.AppendLine("</el-input>");

                        if (!string.IsNullOrEmpty(sch.Binding))
                        {
                            var item = sch.Binding.Split('.');
                            string key = item[item.Length - 1];
                            sbInitData.AppendLine(S(1) + "" + key + ":\"" + (sch.InitData ?? "") + "\",");
                        }

                    }
                    else if (sch.Type == "select")
                    {
                        sbTemplate.Append(S(3) + "<el-select clearable");
                        if (!string.IsNullOrEmpty(sch.Binding)) sbTemplate.Append(" v-model=\"" + sch.Binding + ".selValue\"");
                        if (!string.IsNullOrEmpty(sch.PlaceHolder)) sbTemplate.Append(" placeholder=\"" + sch.PlaceHolder + "\"");
                        if (!string.IsNullOrEmpty(sch.Style)) sbTemplate.Append(" style=\"" + sch.Style + "\"");
                        sbTemplate.AppendLine(">");
                        if (!string.IsNullOrEmpty(sch.Binding))
                        {
                            sbTemplate.AppendLine(S(4) + "<el-option v-for=\"item in " + sch.Binding + ".options\" :key=\"item.value\" :label=\"item.label\" :value=\"item.value\"></el-option>");
                        }
                        sbTemplate.AppendLine(S(3) + "</el-select>");

                        if (!string.IsNullOrEmpty(sch.Binding))
                        {
                            var item = sch.Binding.Split('.');
                            string key = item[item.Length - 1];
                            string staticdata = "";
                            string apipath = "";
                            if (sch.InitData.IndexOf("[#OPTION#]") >= 0)
                            {
                                staticdata = sch.InitData.Split(new string[] { "[#OPTION#]" }, StringSplitOptions.None)[0];
                                apipath = sch.InitData.Split(new string[] { "[#OPTION#]" }, StringSplitOptions.None)[1];
                            }
                            else
                            {
                                if (sch.InitData.StartsWith("/"))
                                {
                                    apipath = sch.InitData;
                                }
                                else
                                {
                                    staticdata = sch.InitData;
                                }
                            }
                            sbInitData.AppendLine(S(1) + "" + key + ":{");
                            sbInitData.AppendLine(S(2) + "options:[" + (staticdata) + "],");
                            sbInitData.AppendLine(S(2) + "selValue:''");
                            sbInitData.AppendLine(S(1) + "},");
                            if (apipath != "")
                            {
                                sbOptionData.AppendLine(S(1) + "ftdpBase.optionsJson(config,vm,apiBase+'" + apipath + "',function(json){" + sch.Binding + ".options=" + sch.Binding + ".options.concat(json);});");
                            }
                        }
                    }
                    if (sch.Type == "date")
                    {
                        sbTemplate.Append(S(3) + "<el-date-picker clearable value-format=\"yyyy-MM-dd\"");
                        if (!string.IsNullOrEmpty(sch.Binding)) sbTemplate.Append(" v-model=\"" + sch.Binding + "\"");
                        if (!string.IsNullOrEmpty(sch.PlaceHolder)) sbTemplate.Append(" placeholder=\"" + sch.PlaceHolder + "\"");
                        if (!string.IsNullOrEmpty(sch.Style)) sbTemplate.Append(" style=\"" + sch.Style + "\"");
                        sbTemplate.Append(">");
                        sbTemplate.AppendLine("</el-date-picker>");

                        if (!string.IsNullOrEmpty(sch.Binding))
                        {
                            var item = sch.Binding.Split('.');
                            string key = item[item.Length - 1];
                            sbInitData.AppendLine(S(1) + "" + key + ":\"" + (sch.InitData ?? "") + "\",");
                        }

                    }
                    if (sch.Type == "dater")
                    {
                        var s1 = res.ftclass.str("z039");
                        var s2 = res.ftclass.str("z040");
                        var s3 = res.ftclass.str("z041");
                        if (!string.IsNullOrEmpty(sch.PlaceHolder))
                        {
                            var ss0 = sch.PlaceHolder.Split(',');
                            if(ss0.Length>=3)
                            {
                                s2 = ss0[0];
                                s1 = ss0[1];
                                s3 = ss0[2];
                            }
                        }
                            sbTemplate.Append(S(3) + "<el-date-picker clearable value-format=\"yyyy-MM-dd\" type=\"daterange\" range-separator=\""+s1+"\" start-placeholder=\""+s2+"\" end-placeholder=\""+s3+"\"");
                        if (!string.IsNullOrEmpty(sch.Binding)) sbTemplate.Append(" v-model=\"" + sch.Binding + "\"");
                        if (!string.IsNullOrEmpty(sch.Style)) sbTemplate.Append(" style=\"" + sch.Style + "\"");
                        sbTemplate.Append(">");
                        sbTemplate.AppendLine("</el-date-picker>");

                        if (!string.IsNullOrEmpty(sch.Binding))
                        {
                            var item = sch.Binding.Split('.');
                            string key = item[item.Length - 1];
                            sbInitData.AppendLine(S(1) + "" + key + ":null,");
                        }

                    }
                    else if(sch.Type == "html")
                    {
                        sbTemplate.Append(S(3) + sch.InitData);
                    }
                }
                foreach (var btn in obj.ButtonList)
                {
                    if (btn.IsGroupStart) sbTemplate.AppendLine(S(3) + "<el-button-group>");
                    sbTemplate.Append(S(3) + "<el-button");
                    if (!string.IsNullOrEmpty(btn.Type)) sbTemplate.Append(" type=\"" + btn.Type + "\"");
                    if (!string.IsNullOrEmpty(btn.Icon)) sbTemplate.Append(" icon=\"" + btn.Icon + "\"");
                    if (!string.IsNullOrEmpty(btn.Size)) sbTemplate.Append(" size=\"" + btn.Size + "\"");
                    if (btn.IsPlain) sbTemplate.Append(" plain");
                    if (btn.IsRound) sbTemplate.Append(" round");
                    if (btn.IsCircle) sbTemplate.Append(" circle");
                    if (!string.IsNullOrEmpty(btn.Click))
                    {
                        var btnClicks = btn.Click.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        if(btnClicks.Length > 1) btn.Click=btnClicks[btnClicks.Length-1];
                    }
                    if (!string.IsNullOrEmpty(btn.Click)) sbTemplate.Append(" @click=\"" + btn.Click + "\"");
                    sbTemplate.Append(">");
                    sbTemplate.Append(btn.Caption);
                    sbTemplate.AppendLine("</el-button>");
                    if (btn.IsGroupEnd) sbTemplate.AppendLine(S(3) + "</el-button-group>");
                    switch (btn.Click)
                    {
                        case string s when s.StartsWith("search("):
                            SearchJs = btn.Js;
                            break;
                        case string s when s.StartsWith("reset("):
                            ResetJs = btn.Js;
                            break;
                        case string s when s.StartsWith("excel("):
                            break;
                        default:
                            if (!string.IsNullOrEmpty(btn.Click) && !btn.Click.StartsWith("func(")) parentFuncList.Add(btn.Click);
                            break;
                    }
                }
                sbTemplate.AppendLine(S(2) + "</el-header>");
            }
            sbTemplate.AppendLine(S(2) + "<el-main>");
            sbTemplate.Append(S(3) + "<el-table :data=\"gridData.list\" @selection-change=\"handleSelectionChange\" @sort-change=\"handleSortChange\"");
            if (obj.OtherSetDic.ContainsKey("List_Stripe") && bool.Parse(obj.OtherSetDic["List_Stripe"])) sbTemplate.Append(" stripe");
            if (obj.OtherSetDic.ContainsKey("List_Border") && bool.Parse(obj.OtherSetDic["List_Border"])) sbTemplate.Append(" border");
            if (obj.OtherSetDic.ContainsKey("List_Loading") && bool.Parse(obj.OtherSetDic["List_Loading"])) sbTemplate.Append(" v-loading=\"page.loading\"");
            if (obj.OtherSetDic.ContainsKey("List_Height") && !string.IsNullOrWhiteSpace(obj.OtherSetDic["List_Height"])) sbTemplate.Append(" height=\"" + obj.OtherSetDic["List_Height"] + "\"");
            if (obj.OtherSetDic.ContainsKey("List_MaxHeight") && !string.IsNullOrWhiteSpace(obj.OtherSetDic["List_MaxHeight"])) sbTemplate.Append(" max-height=\"" + obj.OtherSetDic["List_MaxHeight"] + "\"");
            if (obj.OtherSetDic.ContainsKey("List_TableStyle") && !string.IsNullOrWhiteSpace(obj.OtherSetDic["List_TableStyle"])) sbTemplate.Append(" style=\"" + obj.OtherSetDic["List_TableStyle"] + "\"");
            sbTemplate.AppendLine(">");
            foreach (var col in obj.RowsList)
            {
                sbTemplate.Append(S(4) + "<el-table-column");
                if (!string.IsNullOrEmpty(col.Caption)) sbTemplate.Append(" label=\"" + col.Caption + "\"");
                if (!string.IsNullOrEmpty(col.Binding)) sbTemplate.Append(" property=\"" + col.Binding + "\"");
                if (!string.IsNullOrEmpty(col.Width)) sbTemplate.Append(" width=\"" + col.Width + "\"");
                if (!string.IsNullOrEmpty(col.Freezon)) sbTemplate.Append(" fixed=\"" + col.Freezon + "\"");
                if (col.IsSort) sbTemplate.Append(" sortable=\"custom\"");
                if (col.IsSelection) sbTemplate.Append(" type=\"selection\"");
                sbTemplate.Append(">");
                if (!string.IsNullOrWhiteSpace(col.Template))
                {
                    col.Template = FrontFunc.TempKeyPara(col.Template, col.Caption, "", col.Binding);
                    sbTemplate.Append(Environment.NewLine + col.Template + Environment.NewLine + S(4));
                }
                sbTemplate.AppendLine("</el-table-column>");
            }
            sbTemplate.AppendLine(S(3) + "</el-table>");
            sbTemplate.AppendLine(S(2) + "</el-main>");
            if (!obj.PagerDic.ContainsKey("List_PagerHidden") || !bool.Parse(obj.PagerDic["List_PagerHidden"]))
            {
                sbTemplate.AppendLine(S(2) + "<el-footer>");
                obj.PagerDic.TryGetValue("List_PagerLayout", out string pageLayout);
                obj.PagerDic.TryGetValue("List_PagerPageSize", out string pagePagesize);
                bool pageTotal = (obj.PagerDic.ContainsKey("List_PagerTotal") && bool.Parse(obj.PagerDic["List_PagerTotal"]));
                bool pageJumper = (obj.PagerDic.ContainsKey("List_PagerJumper") && bool.Parse(obj.PagerDic["List_PagerJumper"]));
                if (string.IsNullOrEmpty(pageLayout))
                {
                    pageLayout = "prev, pager, next";
                    if (!string.IsNullOrEmpty(pagePagesize)) pageLayout = "sizes," + pageLayout;
                    if (pageTotal) pageLayout = "total," + pageLayout;
                    if (pageJumper) pageLayout = pageLayout + ", jumper";
                }
                if (string.IsNullOrEmpty(pagePagesize)) pagePagesize = "";
                else pagePagesize = ":page-sizes=\"[" + pagePagesize + "]\"";

                sbTemplate.Append(S(3) + "<el-pagination layout=\"" + pageLayout + "\" @size-change=\"handleSizeChange\" " + pagePagesize + " @current-change=\"handleCurrentChange\" :total=\"page.total\" :page-size=\"page.size\"");
                if (obj.PagerDic.ContainsKey("List_PagerBackground") && bool.Parse(obj.PagerDic["List_PagerBackground"])) sbTemplate.Append(" background");
                if (obj.PagerDic.ContainsKey("List_PagerSmall") && bool.Parse(obj.PagerDic["List_PagerSmall"])) sbTemplate.Append(" small");
                sbTemplate.AppendLine(" />");
                sbTemplate.AppendLine(S(2) + "</el-footer>");
            }
            sbTemplate.AppendLine(S(1) + "</el-container>");
            sbTemplate.AppendLine("</template>");
            sbPreview.AppendLine("<div id=\"app1\">");
            sbPreview.AppendLine("<el-container>");
            sbPreview.AppendLine("<el-header style=\"text-align: center;\">");
            sbPreview.AppendLine("<el-row style=\"\">");
            sbPreview.AppendLine(S(1) + "<el-col :span=\"12\"><el-alert title=\""+ res.ftclass.str("z042") + "" + obj.ComName + " -- " + obj.Caption + "\"  type=\"warning\" center effect=\"dark\"></el-alert></el-col>");
            sbPreview.AppendLine(S(1) + "<el-col :span=\"12\"><el-input v-model=\"fillPara\" style=\"width: 300px\" placeholder=\""+ res.ftclass.str("z043") + "\"></el-input> <el-button @click=\"load(fillPara)\">"+ res.ftclass.str("z044") + "</el-button></el-col>");
            sbPreview.AppendLine("</el-row>");
            sbPreview.AppendLine("</el-header>");
            //sbPreview.AppendLine("<el-header style=\"text-align: center;\"><el-alert title=\"列表组件：" + obj.ComName + " -- " + obj.Caption + "\"  type=\"warning\" center effect=\"dark\"></el-alert></el-header>");
            sbPreview.AppendLine("<el-main>");
            sbPreview.Append(sbTemplate.ToString());
            sbPreview.AppendLine("</el-main>");
            sbPreview.AppendLine("</el-container>");
            sbPreview.AppendLine("</div>");
            //sbCom.AppendLine(sbDocument.ToString());
            sbCom.AppendLine(sbTemplate.ToString());
            #region 组件脚本和样式定义
            sbCom.AppendLine("<script>");
            sbCom.AppendLine("##TAG_01##//"+ res.ftclass.str("z045"));
            sbCom.AppendLine("var apiBase=" + obj.ApiBase + "");
            sbCom.AppendLine("var config={");
            sbCom.AppendLine(S(1) + "apiUrl:apiBase+\"" + obj.ApiUrl + "\",");
            sbCom.AppendLine(S(1) + "apiPara: '',");
            sbCom.AppendLine(S(1) + "tokenKey:ftdpConfig.tokenKey,");
            sbCom.AppendLine(S(1) + "tokenVal:ftdpConfig.getTokenLocal(),");
            sbCom.AppendLine(S(1) + "orderBy:'',");
            sbCom.AppendLine(S(1) + "orderType:'',");
            sbCom.AppendLine(S(1) + "schText:'',");
            sbCom.AppendLine(S(1) + "schStrict:'',");
            sbCom.AppendLine(S(1) + "pageSize:12,");
            sbCom.AppendLine(S(1) + "pageNum:1,");
            sbCom.AppendLine("}");
            sbCom.AppendLine("//"+ res.ftclass.str("z046"));
            sbCom.AppendLine("var sdata={");
            sbCom.Append(sbInitData.ToString());
            sbCom.AppendLine("}");
            string exports = @"
var orisdata = '{}'
var inited = false
##TAG_02## {
    data: function() {
        return {
          gridData:{list: [] },
          multipleSelection: [],
          page:{count: 0, size: config.pageSize,loading:false },
        ftdpConfig: ftdpConfig,
            fillPara:'',
          sdata:sdata
        }
    },
	props: {
    loadauto: {
      type: String,
      default: '1'
    }
  },
	methods: {
		load(paras) {
            if (paras != null)config.apiPara = paras
			list_data_module(this);
		},
		search() {
			search_build(this);
			list_data_module(this);
		},
		reset() {
			ftdpBase.list_reset(sdata, orisdata);
			reset_build(this);
			list_data_module(this);
		},
        func(f) {
        const Fn = Function
        new Fn(f + '(this)')()
        },
		excel(maxRow,exportName){
			ftdpBase.list_excel(config,this,maxRow,exportName);
		},
        selection(){
			return this.multipleSelection;
        },
		handleSelectionChange(val) {
			this.multipleSelection = val;
       },
		handleSortChange(val) {
			if(val.order==null)
			{
				config.orderBy='';
				config.orderType='';
			}else if(val.order##TAG_03##'ascending')
			{
				config.orderBy=val.prop;
				config.orderType='asc';
			}
			else {
				config.orderBy=val.prop;
				config.orderType='desc';
			}
			list_data_module(this);
       },
		handleCurrentChange(val) {
         config.pageNum=val;
		list_data_module(this);
      },
    handleSizeChange(val) {
    config.pageSize=val;
    list_data_module(this);
    },
get(apiPath, paras, callback) {
      ftdpBase.normal_get(this, ftdpConfig, apiPath, paras, callback)
    },
    post(apiPath, paras, json, callback) {
      ftdpBase.normal_post(this, ftdpConfig, apiPath, paras, json, callback)
    },
batch(keyName, updateName, updateVal,paras) {
      var keyValues = ''
      for (let i = 0; i < this.selection().length; i++) {
        if (i > 0)keyValues += ','
        keyValues += this.selection()[i][keyName]
      }
      if (keyValues === '') {
        this.$message.warning('Please Select Item')
      } else {
        this.post(config.apiUrl,paras, JSON.parse('{""keyValue"":""' + keyValues + '"",""' + updateName + '"":""' + updateVal + '""}'), () => { list_data_module(this) })
      }
    },
fileDownload(fileInfo) {
    ftdpBase.file_download(this, ftdpConfig, fileInfo)
    }##TAG_04##
   },
	mounted:function(){
        if (!inited) {
		init(this);
		orisdata = JSON.stringify(sdata);}
		if(this.loadauto##TAG_03##'1')list_data_module(this);
        inited = true
	}
}
//Component internal encapsulation loading method
function list_data_module(vm)
{
	ftdpBase.list_data(config,vm,()=>{return js_beforeload(vm)},(resData)=>{return js_beforeset(vm,resData)},(resData)=>{js_afterset(vm,resData)});
}";
            sbCom.AppendLine(exports);
            sbCom.AppendLine(res.ftclass.str("z047"));
            sbCom.AppendLine("function init(vm)");
            sbCom.AppendLine("{");
            sbCom.AppendLine(obj.InitSet);
            sbCom.Append(sbOptionData.ToString());
            sbCom.AppendLine("}");
            sbCom.AppendLine(res.ftclass.str("z048"));
            sbCom.AppendLine("function search_build(vm)");
            sbCom.AppendLine("{");
            sbCom.AppendLine(SearchJs);
            sbCom.AppendLine("}");
            sbCom.AppendLine(res.ftclass.str("z049"));
            sbCom.AppendLine("function reset_build(vm)");
            sbCom.AppendLine("{");
            sbCom.AppendLine(ResetJs);
            sbCom.AppendLine("}");
            sbCom.AppendLine(res.ftclass.str("z050"));
            sbCom.AppendLine("function js_beforeload(vm)");
            sbCom.AppendLine("{");
            sbCom.AppendLine(obj.JsBeforeLoad);
            sbCom.AppendLine("}");
            sbCom.AppendLine(res.ftclass.str("z051"));
            sbCom.AppendLine("function js_beforeset(vm,resData)");
            sbCom.AppendLine("{");
            sbCom.AppendLine(obj.JsBeforeSet);
            sbCom.AppendLine("}");
            sbCom.AppendLine(res.ftclass.str("z052"));
            sbCom.AppendLine("function js_afterset(vm,resData)");
            sbCom.AppendLine("{");
            sbCom.AppendLine(obj.JsAfterSet);
            sbCom.AppendLine("}");
            //sbCom.AppendLine("//自定义脚本");
            //sbCom.AppendLine(obj.CustomJs);
            sbCom.AppendLine("</script>");
            sbCom.AppendLine("<style scoped>");
            sbCom.AppendLine(obj.CssText);
            sbCom.AppendLine("</style>");
            #endregion
            #region 组件脚本和样式定义
            sbPreview.AppendLine("<script>");
            sbPreview.AppendLine("//"+ res.ftclass.str("z045"));
            sbPreview.AppendLine("var apiBase=" + obj.ApiBase + ";");
            sbPreview.AppendLine("var config={");
            sbPreview.AppendLine(S(1) + "apiUrl:apiBase+\"" + obj.ApiUrl + "\",");
            sbPreview.AppendLine(S(1) + "apiPara: '',");
            sbPreview.AppendLine(S(1) + "tokenKey:ftdpConfig.tokenKey,");
            sbPreview.AppendLine(S(1) + "tokenVal:ftdpConfig.getTokenLocal(),");
            sbPreview.AppendLine(S(1) + "orderBy:'',");
            sbPreview.AppendLine(S(1) + "orderType:'',");
            sbPreview.AppendLine(S(1) + "schText:'',");
            sbPreview.AppendLine(S(1) + "schStrict:'',");
            sbPreview.AppendLine(S(1) + "pageSize:12,");
            sbPreview.AppendLine(S(1) + "pageNum:1,");
            sbPreview.AppendLine("}");
            sbPreview.AppendLine("//"+ res.ftclass.str("z046"));
            sbPreview.AppendLine("var sdata={");
            sbPreview.Append(sbInitData.ToString());
            sbPreview.AppendLine("}");
            string exportsPreview = @"//
var orisdata = '{}';
var inited = false
var Main = {
    data: function() {
        return {
          gridData:{list: [] },
		  multipleSelection: [],
			page:{count: 0, size: config.pageSize,loading:false },
        ftdpConfig: ftdpConfig,
            fillPara:'',
			sdata:sdata
        }
    },
	props: {
    loadauto: {
      type: Number,
      default: 1
    }
  },
	methods: {
		load(paras) {
            if (paras != null)config.apiPara = paras
			list_data_module(this);
		},
		search() {
			search_build(this);
			list_data_module(this);
		},
		reset() {
			ftdpBase.list_reset(sdata, orisdata);
			reset_build(this);
			list_data_module(this);
		},
        func(f)
		{
			eval(f+'(this)');
        },
		excel(maxRow,exportName){
			ftdpBase.list_excel(config,this,maxRow,exportName);
		},
        selection(){
			return this.multipleSelection;
        },
		handleSelectionChange(val) {
			this.multipleSelection = val;
       },
        handleSortChange(val) {
			if(val.order==null)
			{
				config.orderBy='';
				config.orderType='';
			}else if(val.order=='ascending')
			{
				config.orderBy=val.prop;
				config.orderType='asc';
			}
			else {
				config.orderBy=val.prop;
				config.orderType='desc';
			}
			list_data_module(this);
       },
        handleCurrentChange(val) {
         config.pageNum=val;
        list_data_module(this);
      },
    handleSizeChange(val) {
    config.pageSize=val;
    list_data_module(this);
    },
get(apiPath, paras, callback) {
      ftdpBase.normal_get(this, ftdpConfig, apiPath, paras, callback)
    },
    post(apiPath, paras, json, callback) {
      ftdpBase.normal_post(this, ftdpConfig, apiPath, paras, json, callback)
    },
batch(keyName, updateName, updateVal,paras) {
      var keyValues = ''
      for (let i = 0; i < this.selection().length; i++) {
        if (i > 0)keyValues += ','
        keyValues += this.selection()[i][keyName]
      }
      if (keyValues === '') {
        this.$message.warning('Please Select Item')
      } else {
        this.post(config.apiUrl, paras, JSON.parse('{""keyValue"":""' + keyValues + '"",""' + updateName + '"":""' + updateVal + '""}'), () => { list_data_module(this) })
      }
    },
fileDownload(fileInfo) {
    ftdpBase.file_download(this, ftdpConfig, fileInfo)
    }##TAG_04##
   },
	mounted:function(){
        if (!inited) {
		init(this);
		orisdata = JSON.stringify(sdata);}
		if('1'=='1')list_data_module(this);
        inited = true
	}
}
//Component internal encapsulation loading method
function list_data_module(vm)
{
	ftdpBase.list_data(config,vm,()=>{return js_beforeload(vm)},(resData)=>{return js_beforeset(vm,resData)},(resData)=>{js_afterset(vm,resData)});
}";
            sbPreview.AppendLine(exportsPreview);
            sbPreview.AppendLine(res.ftclass.str("z047"));
            sbPreview.AppendLine("function init(vm)");
            sbPreview.AppendLine("{");
            sbPreview.AppendLine(obj.InitSet);
            sbPreview.AppendLine(sbOptionData.ToString());
            sbPreview.AppendLine("}");
            sbPreview.AppendLine(res.ftclass.str("z048"));
            sbPreview.AppendLine("function search_build(vm)");
            sbPreview.AppendLine("{");
            sbPreview.AppendLine(SearchJs);
            sbPreview.AppendLine("}");
            sbPreview.AppendLine(res.ftclass.str("z049"));
            sbPreview.AppendLine("function reset_build(vm)");
            sbPreview.AppendLine("{");
            sbPreview.AppendLine(ResetJs);
            sbPreview.AppendLine("}");
            sbPreview.AppendLine(res.ftclass.str("z050"));
            sbPreview.AppendLine("function js_beforeload(vm)");
            sbPreview.AppendLine("{");
            sbPreview.AppendLine(obj.JsBeforeLoad);
            sbPreview.AppendLine("}");
            sbPreview.AppendLine(res.ftclass.str("z051"));
            sbPreview.AppendLine("function js_beforeset(vm,resData)");
            sbPreview.AppendLine("{");
            sbPreview.AppendLine(obj.JsBeforeSet);
            sbPreview.AppendLine("}");
            sbPreview.AppendLine(res.ftclass.str("z052"));
            sbPreview.AppendLine("function js_afterset(vm,resData)");
            sbPreview.AppendLine("{");
            sbPreview.AppendLine(obj.JsAfterSet);
            sbPreview.AppendLine("}");
            sbPreview.AppendLine("var Ctor = Vue.extend(Main);");
            sbPreview.AppendLine("new Ctor().$mount('#app1');");
            //sbPreview.AppendLine("//自定义脚本");
            //sbPreview.AppendLine(obj.CustomJs);
            sbPreview.AppendLine("</script>");
            sbPreview.AppendLine("<style>");
            sbPreview.AppendLine(obj.CssText);
            sbPreview.AppendLine("</style>");
            sbPreview.AppendLine("</body>");
            sbPreview.AppendLine("</html>");
            #endregion
            #endregion
            #region Html
            sbHtml.AppendLine(HTMLHeader);
            sbHtml.AppendLine("<script type='text/javascript'>");
            sbHtml.AppendLine("/*");
            sbHtml.AppendLine(S(1) + res.ftclass.str("z053") + obj.Caption);
            sbHtml.AppendLine(S(1) + res.ftclass.str("z054"));
            sbHtml.AppendLine(S(1) + res.ftclass.str("z055") + string.Join(",", parentFuncList));
            sbHtml.AppendLine("*/");
            sbHtml.AppendLine("httpVueLoader.register(Vue,'com/" + obj.ComName.Replace("/", "_") + ".vue?rdn='+(new Date()).valueOf());");
            sbHtml.AppendLine("</script>");
            sbHtml.AppendLine("<div id=\"app1\">");
            sbHtml.AppendLine("<el-container>");
            sbHtml.AppendLine("<el-header style=\"text-align: center;\">");
            sbHtml.AppendLine("<el-row style=\"\">");
            sbHtml.AppendLine(S(1) + "<el-col :span=\"12\"><el-alert title=\""+ res.ftclass.str("z042") + "" + obj.ComName + " -- " + obj.Caption + "\"  type=\"warning\" center effect=\"dark\"></el-alert></el-col>");
            sbHtml.AppendLine(S(1) + "<el-col :span=\"12\"><el-input v-model=\"fillPara\" style=\"width: 300px\" placeholder=\""+ res.ftclass.str("z043") + "\"></el-input> <el-button @click=\"load(fillPara)\">"+ res.ftclass.str("z044") + "</el-button></el-col>");
            sbHtml.AppendLine("</el-row>");
            sbHtml.AppendLine("</el-header>");
            //sbHtml.AppendLine("<el-header style=\"text-align: center;\"><el-alert title=\"列表组件：" + obj.ComName + " -- " + obj.Caption + "\"  type=\"warning\" center effect=\"dark\"></el-alert></el-header>");
            sbHtml.AppendLine("<el-main>");
            sbHtml.AppendLine(S(1) + "<" + obj.ComName.Replace("/", "_") + " desc=\"" + obj.Caption + "\" loadauto=\"1\" ref=\"" + obj.ComName.Replace("/", "_") + "\"></" + obj.ComName.Replace("/", "_") + ">");
            sbHtml.AppendLine("</el-main>");
            sbHtml.AppendLine("</el-container>");
            sbHtml.AppendLine("</div>");
            sbHtml.AppendLine(@"<script>
var Main = {
	data: function() {
        return {
            fillPara:'',
        }
    },
	methods: {
      load(para) {
        this.$refs." + obj.ComName.Replace("/", "_") + @".load(para);
      }
    },
};
var Ctor = Vue.extend(Main);
new Ctor().$mount('#app1');
</script>");
            sbHtml.Append(@"</body>
</html>");
            #endregion
            string comText = sbCom.ToString();
            string comTextPack = sbCom.ToString();
            string sbPreviewText = sbPreview.ToString();
            comText = comText.Replace("##TAG_01##", "").Replace("##TAG_02##", "module.exports =").Replace("##TAG_03##", "==");
            comTextPack = comTextPack.Replace("##TAG_01##", "import { ftdpConfig } from '@/ftdp/utils/config.js'" + Environment.NewLine + "import { ftdpBase } from '@/ftdp/utils/base.js'" + Environment.NewLine).Replace("##TAG_02##", "export default ").Replace("##TAG_03##", "===");
            comTextPack= sbDocument.ToString()+Environment.NewLine+ comTextPack;
            if (string.IsNullOrWhiteSpace(obj.CustomJs))
            {
                comText = comText.Replace("##TAG_04##", "");
                comTextPack = comTextPack.Replace("##TAG_04##", "");
                sbPreviewText = sbPreviewText.Replace("##TAG_04##", "");
            }
            else
            {
                comText = comText.Replace("##TAG_04##", "," + Environment.NewLine + res.ftclass.str("z056") + Environment.NewLine + obj.CustomJs);
                comTextPack = comTextPack.Replace("##TAG_04##", "," + Environment.NewLine + res.ftclass.str("z056") + Environment.NewLine + obj.CustomJs);
                sbPreviewText = sbPreviewText.Replace("##TAG_04##", "," + Environment.NewLine + res.ftclass.str("z056") + Environment.NewLine + obj.CustomJs);
            }
            return (comText, sbHtml.ToString(), sbPreviewText, comTextPack);
        }
        public (string ComText, string HTMLText, string PreviewHTML, string ComTextPack) Form(FormCols obj)
        {
            var Custom_Js_All = "";
            var Components = new List<string>();
            var SingleSetList = new List<string>();
            #region Auto Generate
            if(!string.IsNullOrWhiteSpace(obj.BindGet) && obj.BindGet.StartsWith("/* Auto Generate */"))
            {
                obj.BindGet = "/* Auto Generate */" + Environment.NewLine + FrontFunc.BindGetGenerate(obj.ApiGet, null, obj.RowsList);
            }
            if (!string.IsNullOrWhiteSpace(obj.BindSet) && obj.BindSet.StartsWith("/* Auto Generate */"))
            {
                obj.BindSet = "/* Auto Generate */" + Environment.NewLine + FrontFunc.BindSetGenerate(obj.ApiSet, null, obj.RowsList);
            }
            #endregion
            #region Integration
            var dic_inter =Form_Integration(obj);
            var css_inter = new List<string>();
            foreach (var item in obj.RowsList)
            {
                if(dic_inter.Keys.Contains(item.Binding))
                {
                    item.Template = dic_inter[item.Binding].Keys.Contains("Form_Template") ? dic_inter[item.Binding]["Form_Template"] : "";
                    if (dic_inter[item.Binding].Keys.Contains("Form_BindGet"))
                    {
                        obj.BindGet += (string.IsNullOrWhiteSpace(obj.BindGet)?"": Environment.NewLine) + dic_inter[item.Binding]["Form_BindGet"];
                    }
                    if (dic_inter[item.Binding].Keys.Contains("Form_BindSet"))
                    {
                        obj.BindSet += (string.IsNullOrWhiteSpace(obj.BindSet) ? "" : Environment.NewLine) + dic_inter[item.Binding]["Form_BindSet"];
                    }
                    if (dic_inter[item.Binding].Keys.Contains("Form_CustomMethod"))
                    {
                        if (string.IsNullOrWhiteSpace(obj.CustomJs)) obj.CustomJs = dic_inter[item.Binding]["Form_CustomMethod"];
                        else obj.CustomJs += Environment.NewLine + "," + Environment.NewLine + dic_inter[item.Binding]["Form_CustomMethod"];
                    }
                    if (dic_inter[item.Binding].Keys.Contains("Form_AppendData"))
                    {
                        obj.CusDataDefine += (string.IsNullOrWhiteSpace(obj.CusDataDefine) ? "" : Environment.NewLine) + dic_inter[item.Binding]["Form_AppendData"];
                    }
                    if (dic_inter[item.Binding].Keys.Contains("Form_JsBeforeGet"))
                    {
                        obj.JsBeforeGet = dic_inter[item.Binding]["Form_JsBeforeGet"] + (string.IsNullOrWhiteSpace(obj.JsBeforeGet) ? "" : Environment.NewLine + obj.JsBeforeGet);
                        // obj.JsBeforeGet += (string.IsNullOrWhiteSpace(obj.JsBeforeGet) ? "" : Environment.NewLine) + dic_inter[item.Binding]["Form_JsBeforeGet"];
                    }
                    if (dic_inter[item.Binding].Keys.Contains("Form_JsBeforeSet"))
                    {
                        obj.JsBeforeSet = dic_inter[item.Binding]["Form_JsBeforeSet"] + (string.IsNullOrWhiteSpace(obj.JsBeforeSet) ? "" : Environment.NewLine + obj.JsBeforeSet);
                        // obj.JsBeforeSet += (string.IsNullOrWhiteSpace(obj.JsBeforeSet) ? "" : Environment.NewLine) + dic_inter[item.Binding]["Form_JsBeforeSet"];
                    }
                    if (dic_inter[item.Binding].Keys.Contains("Form_JsAfterSet"))
                    {
                        obj.JsAfterSet += (string.IsNullOrWhiteSpace(obj.JsAfterSet) ? "" : Environment.NewLine) + dic_inter[item.Binding]["Form_JsAfterSet"];
                    }
                    if (dic_inter[item.Binding].Keys.Contains("Form_JsBeforeSubmit"))
                    {
                        obj.JsBeforeSubmit = dic_inter[item.Binding]["Form_JsBeforeSubmit"] + (string.IsNullOrWhiteSpace(obj.JsBeforeSubmit) ? "" : Environment.NewLine + obj.JsBeforeSubmit);
                        // obj.JsBeforeSubmit += (string.IsNullOrWhiteSpace(obj.JsBeforeSubmit) ? "" : Environment.NewLine) + dic_inter[item.Binding]["Form_JsBeforeSubmit"];
                    }
                    if (dic_inter[item.Binding].Keys.Contains("Form_JsAfterSubmit"))
                    {
                        obj.JsAfterSubmit += (string.IsNullOrWhiteSpace(obj.JsAfterSubmit) ? "" : Environment.NewLine) + dic_inter[item.Binding]["Form_JsAfterSubmit"];
                    }
                    if (dic_inter[item.Binding].Keys.Contains("Form_FormInitJs"))
                    {
                        obj.OtherSetDic["Form_FormInitJs"] += (string.IsNullOrWhiteSpace(obj.OtherSetDic["Form_FormInitJs"]) ? "" : Environment.NewLine) + dic_inter[item.Binding]["Form_FormInitJs"];
                    }
                    if (dic_inter[item.Binding].Keys.Contains("Form_AppendCss"))
                    {
                        if(!css_inter.Contains(dic_inter[item.Binding]["Form_AppendCss"]))
                        {
                            obj.CssText += (string.IsNullOrWhiteSpace(obj.CssText) ? "" : Environment.NewLine) + dic_inter[item.Binding]["Form_AppendCss"];
                            css_inter.Add(dic_inter[item.Binding]["Form_AppendCss"]);
                        }
                    }
                    if (dic_inter[item.Binding].Keys.Contains("Custom_Js_All"))
                    {
                        var _set = (string.IsNullOrWhiteSpace(Custom_Js_All) ? "" : Environment.NewLine) + dic_inter[item.Binding]["Custom_Js_All"];
                        if (!_set.StartsWith("[S]") && !_set.StartsWith(Environment.NewLine+"[S]")) Custom_Js_All += _set;
                        else
                        {
                            if(_set.StartsWith("[S]")) _set = _set.Substring(3);
                            else if (_set.StartsWith(Environment.NewLine + "[S]")) _set = Environment.NewLine+_set.Substring((Environment.NewLine + "[S]").Length);
                            if (!SingleSetList.Contains(_set))
                            {
                                SingleSetList.Add(_set);
                                Custom_Js_All += _set;
                            }
                        }
                    }
                    if (dic_inter[item.Binding].Keys.Contains("Components"))
                    {
                        var theComponent = dic_inter[item.Binding]["Components"].Trim();
                        if (!Components.Contains(theComponent)) Components.Add(theComponent);
                    }
                }
            }
            #endregion
            #region ParaPattern
            foreach (var item in obj.RowsList)
            {
                item.Style = Adv.ParaPattern(item.Style);
                item.InitData = Adv.ParaPattern(item.InitData);
                item.Template = Adv.ParaPattern(item.Template);
            }
            foreach (var item in obj.ButtonList)
            {
                item.Js = Adv.ParaPattern(item.Js);
            }
            obj.BindGet = Adv.ParaPattern(obj.BindGet);
            obj.BindSet = Adv.ParaPattern(obj.BindSet);
            obj.OtherSetDic["Form_FormInitJs"]= Adv.ParaPattern(obj.OtherSetDic["Form_FormInitJs"]);
            obj.JsBeforeGet = Adv.ParaPattern(obj.JsBeforeGet);
            obj.JsBeforeSet = Adv.ParaPattern(obj.JsBeforeSet);
            obj.JsAfterSet = Adv.ParaPattern(obj.JsAfterSet);
            obj.JsBeforeSubmit = Adv.ParaPattern(obj.JsBeforeSubmit);
            obj.JsAfterSubmit = Adv.ParaPattern(obj.JsAfterSubmit);
            obj.CustomJs = Adv.ParaPattern(obj.CustomJs);
            obj.CusDataDefine = Adv.ParaPattern(obj.CusDataDefine);
            obj.CssText = Adv.ParaPattern(obj.CssText);
            Custom_Js_All = Adv.ParaPattern(Custom_Js_All);
            obj.OtherSetDic["Form_FormStyle"] = Adv.ParaPattern(obj.OtherSetDic["Form_FormStyle"]);
            obj.OtherSetDic["Form_LayoutTempRow"] = Adv.ParaPattern(obj.OtherSetDic["Form_LayoutTempRow"]);
            obj.OtherSetDic["Form_LayoutTempButton"] = Adv.ParaPattern(obj.OtherSetDic["Form_LayoutTempButton"]);
            #endregion
            string HTMLHeader = @"<!doctype html>
<html lang='en'>
  <head>
  <meta charset='UTF-8'>
  <link rel='stylesheet' href='lib/element-ui/index.css'>
    <script src='lib/vue.js'></script>
	<script src='lib/httpVueLoader.js'></script>
	<script src='lib/element-ui/index.js'></script>
	<script src='lib/axios.min.js'></script>
	<script src='lib/config.js'></script>
	<script src='lib/base.js'></script>
    <script src='lib/en.js'></script>
	<script>ELEMENT.locale(ELEMENT.lang.en)</script>
  </head>
<body>";
            string HTMLHeaderPreview = @"<!doctype html>
<html lang='en'>
  <head>
  <meta charset='UTF-8'>
  <link rel='stylesheet' href='../lib/element-ui/index.css'>
    <script src='../lib/vue.js'></script>
	<script src='../lib/httpVueLoader.js'></script>
	<script src='../lib/element-ui/index.js'></script>
	<script src='../lib/axios.min.js'></script>
	<script src='../lib/config.js'></script>
	<script src='../lib/base.js'></script>
    <script src='../lib/en.js'></script>
	<script>ELEMENT.locale(ELEMENT.lang.en)</script>
  </head>
<body>";
            StringBuilder sbCom = new StringBuilder();
            StringBuilder sbHtml = new StringBuilder();
            StringBuilder sbPreview = new StringBuilder();
            StringBuilder sbInitData = new StringBuilder();
            StringBuilder sbOptionData = new StringBuilder();
            StringBuilder sbDocument = new StringBuilder();
            Dictionary<string,string> dicRules = new Dictionary<string,string>();

            //父方法
            List<string> parentFuncList = new List<string>();
            sbPreview.AppendLine(HTMLHeaderPreview);
            #region Document
            var ComNames = obj.ComName.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var ComNameShort = ComNames[ComNames.Length - 1];
            var doc = DocFromCustomJs(obj.CustomJs,null,obj);
            sbDocument.AppendLine(res.ftclass.str("z021"));
            sbDocument.AppendLine("1、<" + ComNameShort + " ref=\"" + ComNameShort + "\"/>");
            sbDocument.AppendLine("2、import " + ComNameShort + " from '@/ftdp/components/" + obj.ComName + "?'");
            sbDocument.AppendLine("3、components: { " + ComNameShort + " }");
            sbDocument.AppendLine("*/");
            sbDocument.AppendLine(res.ftclass.str("z057"));
            sbDocument.AppendLine(res.ftclass.str("z023"));
            foreach (var item in doc.EmitList)
            {
                sbDocument.AppendLine("\t" + $"{item.Emit} : {item.Desc}");
            }
            sbDocument.AppendLine("\t" + res.ftclass.str("z058"));
            sbDocument.AppendLine("\t" + res.ftclass.str("z059"));
            sbDocument.AppendLine("\t" + res.ftclass.str("z060"));
            sbDocument.AppendLine("\t" + res.ftclass.str("z061"));
            sbDocument.AppendLine("\t" + res.ftclass.str("z062"));
            if (doc.FuncList.Count > 0)
            {
                sbDocument.AppendLine(res.ftclass.str("z027"));
                foreach (var item in doc.FuncList)
                {
                    sbDocument.AppendLine("\t" + $"{item.Func} : {item.Desc}");
                }
            }
            sbDocument.AppendLine(res.ftclass.str("z028"));
            sbDocument.AppendLine("\tfill(paras)" + res.ftclass.str("z063"));
            sbDocument.AppendLine("\treset(allEmpty)" + res.ftclass.str("z064"));
            sbDocument.AppendLine("\tsubmit()" + res.ftclass.str("z065"));
            sbDocument.AppendLine("\tfileDownload(fileInfo)" + res.ftclass.str("z066"));
            sbDocument.AppendLine("\tget(apiPath, paras, callback)" + res.ftclass.str("z067"));
            sbDocument.AppendLine("\tpost(apiPath, paras, json, callback)"+ res.ftclass.str("z068"));
            sbDocument.Append("*/");
            #endregion
            #region Com
            StringBuilder sbTemplate = new StringBuilder();
            sbTemplate.AppendLine("<template>");
            sbTemplate.Append(S(1) + "<el-form ref=\"form\" :model=\"form\"");
            if (obj.OtherSetDic.ContainsKey("Form_LabelWidth") && !string.IsNullOrWhiteSpace(obj.OtherSetDic["Form_LabelWidth"])) sbTemplate.Append(" label-width=\"" + obj.OtherSetDic["Form_LabelWidth"] + "\"");
            if (obj.OtherSetDic.ContainsKey("Form_LabelPosition") && !string.IsNullOrWhiteSpace(obj.OtherSetDic["Form_LabelPosition"])) sbTemplate.Append(" label-position=\"" + obj.OtherSetDic["Form_LabelPosition"] + "\"");
            if (obj.OtherSetDic.ContainsKey("Form_FormStyle") && !string.IsNullOrWhiteSpace(obj.OtherSetDic["Form_FormStyle"])) sbTemplate.Append(" style=\"" + obj.OtherSetDic["Form_FormStyle"] + "\"");
            if (obj.OtherSetDic.ContainsKey("Form_FormLoading") && bool.Parse(obj.OtherSetDic["Form_FormLoading"])) sbTemplate.Append(" v-loading=\"page.loading\"");
            sbTemplate.AppendLine(" :rules=\"formRules\">");
            List<string> FormItemList = new List<string>();
            foreach (var row in obj.RowsList)
            {
                if(row.Template !=null)
                {
                    row.Template = FrontFunc.TempKeyPara(row.Template, row.Caption, row.PlaceHolder, row.Binding);
                }
                StringBuilder sbItem = new StringBuilder();
                string modelBinding = null;
                string _template = row.Template.Trim();
                string template = _template.StartsWith("<") ? _template : null;
                string attribute = !_template.StartsWith("<") ? _template : null;
                // 使用temp后，初始化和数据定义依然有效
                if (row.Type == "selectMore")
                {
                    row.Type = "select";
                    if (attribute == null) attribute = "";
                    if (attribute.IndexOf("multiple") < 0) attribute += " multiple";
                }
                switch (row.Type)
                {
                    case "label":
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            if (row.Binding.EndsWith("_ishtml"))
                            {
                                sbItem.AppendLine(S(3) + "<div v-html=\"" + row.Binding + "\" />");
                            }
                            else
                            {
                                sbItem.AppendLine(S(3) + "{{" + row.Binding + "}}");
                            }
                        }
                        break;
                    case "input":
                        sbItem.Append(S(3) + "<el-input clearable");
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + "\"");
                            modelBinding = row.Binding;
                        }
                        if (!string.IsNullOrWhiteSpace(row.PlaceHolder)) sbItem.Append(" placeholder=\"" + row.PlaceHolder + "\"");
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        sbItem.AppendLine(S(3) + "</el-input>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            sbInitData.AppendLine(S(1) + "" + key + ":\"" + (row.InitData ?? "") + "\",");
                        }
                        break;
                    case "select":
                        sbItem.Append(S(3) + "<el-select clearable");
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + ".selValue\"");
                            modelBinding = (row.Binding + ".selValue");
                        }
                        if (!string.IsNullOrWhiteSpace(row.PlaceHolder)) sbItem.Append(" placeholder=\"" + row.PlaceHolder + "\"");
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            sbItem.AppendLine(S(4) + "<el-option v-for=\"item in " + row.Binding + ".options\" :key=\"item.value\" :label=\"item.label\" :value=\"item.value\"></el-option>");
                        }
                        sbItem.AppendLine(S(3) + "</el-select>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            string staticdata = "";
                            string apipath = "";
                            if (row.InitData.IndexOf("[#OPTION#]") >= 0)
                            {
                                staticdata = row.InitData.Split(new string[] { "[#OPTION#]" }, StringSplitOptions.None)[0];
                                apipath = row.InitData.Split(new string[] { "[#OPTION#]" }, StringSplitOptions.None)[1];
                            }
                            else
                            {
                                if (row.InitData.StartsWith("/"))
                                {
                                    apipath = row.InitData;
                                }
                                else
                                {
                                    staticdata = row.InitData;
                                }
                            }
                            sbInitData.AppendLine(S(1) + "" + key + ":{");
                            sbInitData.AppendLine(S(2) + "options:[" + (staticdata) + "],");
                            if (attribute != null && attribute.ToLower().IndexOf("multiple") >= 0)
                            {
                                sbInitData.AppendLine(S(2) + "selValue:[]");
                            }
                            else
                            {
                                sbInitData.AppendLine(S(2) + "selValue:''");
                            }
                            sbInitData.AppendLine(S(1) + "},");
                            if (apipath != "")
                            {
                                sbOptionData.AppendLine(S(1) + "ftdpBase.optionsJson(config,vm,apiBase+'" + apipath + "',(json)=>{" + row.Binding + ".options=" + row.Binding + ".options.concat(json);callback();});");
                            }
                        }
                        break;
                    case "switch":
                        sbItem.Append(S(3) + "<el-switch :active-value=\"1\" :inactive-value=\"0\"");
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + "\"");
                            modelBinding = (row.Binding);
                        }
                        if (!string.IsNullOrWhiteSpace(row.PlaceHolder)) sbItem.Append(" placeholder=\"" + row.PlaceHolder + "\"");
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        sbItem.AppendLine(S(3) + "</el-switch>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            sbInitData.AppendLine(S(1) + "" + key + ":" + (row.InitData ?? "") + ",");
                        }
                        break;
                    case "checkbox":
                        sbItem.Append(S(3) + "<el-checkbox-group");
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + ".selValue\"");
                            modelBinding = (row.Binding + ".selValue");
                        }
                        if (!string.IsNullOrWhiteSpace(row.PlaceHolder)) sbItem.Append(" placeholder=\"" + row.PlaceHolder + "\"");
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            sbItem.AppendLine(S(4) + "<el-checkbox v-for=\"item in " + row.Binding + ".options\" :key=\"item.value\" :label=\"item.value\">{{item.label}}</el-checkbox>");
                        }
                        sbItem.AppendLine(S(3) + "</el-checkbox-group>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            string staticdata = "";
                            string apipath = "";
                            if (row.InitData.IndexOf("[#OPTION#]") >= 0)
                            {
                                staticdata = row.InitData.Split(new string[] { "[#OPTION#]" }, StringSplitOptions.None)[0];
                                apipath = row.InitData.Split(new string[] { "[#OPTION#]" }, StringSplitOptions.None)[1];
                            }
                            else
                            {
                                if (row.InitData.StartsWith("/"))
                                {
                                    apipath = row.InitData;
                                }
                                else
                                {
                                    staticdata = row.InitData;
                                }
                            }
                            sbInitData.AppendLine(S(1) + "" + key + ":{");
                            sbInitData.AppendLine(S(2) + "options:[" + (staticdata) + "],");
                            sbInitData.AppendLine(S(2) + "selValue:[]");
                            sbInitData.AppendLine(S(1) + "},");
                            if (apipath != "")
                            {
                                sbOptionData.AppendLine(S(1) + "ftdpBase.optionsJson(config,vm,apiBase+'" + apipath + "',(json)=>{" + row.Binding + ".options=" + row.Binding + ".options.concat(json);callback();});");
                            }
                        }
                        break;
                    case "radiobox":
                        sbItem.Append(S(3) + "<el-radio-group");
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + ".selValue\"");
                            modelBinding = (row.Binding + ".selValue");
                        }
                        if (!string.IsNullOrWhiteSpace(row.PlaceHolder)) sbItem.Append(" placeholder=\"" + row.PlaceHolder + "\"");
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            sbItem.AppendLine(S(4) + "<el-radio v-for=\"item in " + row.Binding + ".options\" :key=\"item.value\" :label=\"item.value\">{{item.label}}</el-radio>");
                        }
                        sbItem.AppendLine(S(3) + "</el-radio-group>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            string staticdata = "";
                            string apipath = "";
                            if (row.InitData.IndexOf("[#OPTION#]") >= 0)
                            {
                                staticdata = row.InitData.Split(new string[] { "[#OPTION#]" }, StringSplitOptions.None)[0];
                                apipath = row.InitData.Split(new string[] { "[#OPTION#]" }, StringSplitOptions.None)[1];
                            }
                            else
                            {
                                if (row.InitData.StartsWith("/"))
                                {
                                    apipath = row.InitData;
                                }
                                else
                                {
                                    staticdata = row.InitData;
                                }
                            }
                            sbInitData.AppendLine(S(1) + "" + key + ":{");
                            sbInitData.AppendLine(S(2) + "options:[" + (staticdata) + "],");
                            sbInitData.AppendLine(S(2) + "selValue:''");
                            sbInitData.AppendLine(S(1) + "},");
                            if (apipath != "")
                            {
                                sbOptionData.AppendLine(S(1) + "ftdpBase.optionsJson(config,vm,apiBase+'" + apipath + "',(json)=>{" + row.Binding + ".options=" + row.Binding + ".options.concat(json);callback();});");
                            }
                        }
                        break;
                    case "textarea":
                        sbItem.Append(S(3) + "<el-input type=\"textarea\"");
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + "\"");
                            modelBinding = (row.Binding);
                        }
                        if (!string.IsNullOrWhiteSpace(row.PlaceHolder)) sbItem.Append(" placeholder=\"" + row.PlaceHolder + "\"");
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        sbItem.AppendLine(S(3) + "</el-input>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            sbInitData.AppendLine(S(1) + "" + key + ":\"" + (row.InitData ?? "") + "\",");
                        }
                        break;
                    case "date":
                        if (!string.IsNullOrWhiteSpace(attribute) && attribute.IndexOf("value-format", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            sbItem.Append(S(3) + "<el-date-picker clearable");
                        }
                        else
                        {
                            sbItem.Append(S(3) + "<el-date-picker clearable value-format=\"yyyy-MM-dd\"");
                        }
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + "\"");
                            modelBinding = (row.Binding);
                        }
                        if (!string.IsNullOrWhiteSpace(row.PlaceHolder)) sbItem.Append(" placeholder=\"" + row.PlaceHolder + "\"");
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        sbItem.AppendLine(S(3) + "</el-date-picker>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            sbInitData.AppendLine(S(1) + "" + key + ":\"" + (row.InitData ?? "") + "\",");
                        }
                        break;
                    case "time":
                        sbItem.Append(S(3) + "<el-time-picker clearable value-format=\"yyyy-MM-dd HH:mm:ss\"");
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + "\"");
                            modelBinding = (row.Binding);
                        }
                        if (!string.IsNullOrWhiteSpace(row.PlaceHolder)) sbItem.Append(" placeholder=\"" + row.PlaceHolder + "\"");
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        sbItem.AppendLine(S(3) + "</el-time-picker>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            sbInitData.AppendLine(S(1) + "" + key + ":\"" + (row.InitData ?? "") + "\",");
                        }
                        break;
                    case "timeSel":
                        sbItem.Append(S(3) + "<el-time-select clearable");
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + "\"");
                            modelBinding = (row.Binding);
                        }
                        if (!string.IsNullOrWhiteSpace(row.PlaceHolder)) sbItem.Append(" placeholder=\"" + row.PlaceHolder + "\"");
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        sbItem.AppendLine(S(3) + "</el-time-select>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            sbInitData.AppendLine(S(1) + "" + key + ":\"" + (row.InitData ?? "") + "\",");
                        }
                        break;
                    case "number":
                        sbItem.Append(S(3) + "<el-input-number");
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + "\"");
                            modelBinding = (row.Binding);
                        }
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        sbItem.AppendLine(S(3) + "</el-input-number>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            sbInitData.AppendLine(S(1) + "" + key + ":\"" + (row.InitData ?? "") + "\",");
                        }
                        break;
                    case "slider":
                        sbItem.Append(S(3) + "<el-slider");
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + "\"");
                            modelBinding = (row.Binding);
                        }
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        sbItem.AppendLine(S(3) + "</el-slider>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            sbInitData.AppendLine(S(1) + "" + key + ":" + (string.IsNullOrWhiteSpace(row.InitData) ? "null" : row.InitData) + ",");
                        }
                        break;
                    case "rate":
                        sbItem.Append(S(3) + "<el-rate");
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + "\"");
                            modelBinding = (row.Binding);
                        }
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        sbItem.AppendLine(S(3) + "</el-rate>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            sbInitData.AppendLine(S(1) + "" + key + ":" + (string.IsNullOrWhiteSpace(row.InitData) ? "null" : row.InitData) + ",");
                        }
                        break;
                    case "color":
                        sbItem.Append(S(3) + "<el-color-picker");
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + "\"");
                            modelBinding = (row.Binding);
                        }
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        sbItem.AppendLine(S(3) + "</el-color-picker>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            sbInitData.AppendLine(S(1) + "" + key + ":\"" + (row.InitData ?? "") + "\",");
                        }
                        break;
                    case "transfer":
                        sbItem.Append(S(3) + "<el-transfer");
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + ".selValue\"");
                            sbItem.Append(" :data=\"" + row.Binding + ".options\"");
                            modelBinding = (row.Binding + ".selValue");
                        }
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        sbItem.AppendLine(S(3) + "</el-transfer>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            string staticdata = "";
                            string apipath = "";
                            if (row.InitData.IndexOf("[#OPTION#]") >= 0)
                            {
                                staticdata = row.InitData.Split(new string[] { "[#OPTION#]" }, StringSplitOptions.None)[0];
                                apipath = row.InitData.Split(new string[] { "[#OPTION#]" }, StringSplitOptions.None)[1];
                            }
                            else
                            {
                                if (row.InitData.StartsWith("/"))
                                {
                                    apipath = row.InitData;
                                }
                                else
                                {
                                    staticdata = row.InitData;
                                }
                            }
                            //穿梭框需要把value替换成key
                            staticdata = staticdata.Replace("value", "key");
                            sbInitData.AppendLine(S(1) + "" + key + ":{");
                            sbInitData.AppendLine(S(2) + "options:[" + (staticdata) + "],");
                            sbInitData.AppendLine(S(2) + "selValue:[]");
                            sbInitData.AppendLine(S(1) + "},");
                            if (apipath != "")
                            {
                                sbOptionData.AppendLine(S(1) + "ftdpBase.optionsJson(config,vm,apiBase+'" + apipath + "',(json)=>{json.forEach(function(obj){obj['key'] = obj['value'];});" + row.Binding + ".options=" + row.Binding + ".options.concat(json);callback();});");
                            }
                        }
                        break;
                    case "cascader":
                        sbItem.Append(S(3) + "<el-cascader clearable");
                        if (!string.IsNullOrWhiteSpace(row.Binding))
                        {
                            sbItem.Append(" v-model=\"" + row.Binding + ".selValue\"");
                            sbItem.Append(" :options=\"" + row.Binding + ".options\"");
                            modelBinding = (row.Binding + ".selValue");
                        }
                        if (!string.IsNullOrWhiteSpace(row.PlaceHolder)) sbItem.Append(" placeholder=\"" + row.PlaceHolder + "\"");
                        if (!string.IsNullOrWhiteSpace(row.Style)) sbItem.Append(" style=\"" + row.Style + "\"");
                        if (row.Disable) sbItem.Append(" disabled");
                        if (!string.IsNullOrWhiteSpace(attribute)) sbItem.Append(" " + attribute);
                        sbItem.AppendLine(">");
                        sbItem.AppendLine(S(3) + "</el-cascader>");
                        if (!string.IsNullOrEmpty(row.Binding))
                        {
                            var item = row.Binding.Split('.');
                            string key = item[item.Length - 1];
                            string staticdata = "";
                            string apipath = "";
                            if (row.InitData.IndexOf("[#OPTION#]") >= 0)
                            {
                                staticdata = row.InitData.Split(new string[] { "[#OPTION#]" }, StringSplitOptions.None)[0];
                                apipath = row.InitData.Split(new string[] { "[#OPTION#]" }, StringSplitOptions.None)[1];
                            }
                            else
                            {
                                if (row.InitData.StartsWith("/"))
                                {
                                    apipath = row.InitData;
                                }
                                else
                                {
                                    staticdata = row.InitData;
                                }
                            }
                            sbInitData.AppendLine(S(1) + "" + key + ":{");
                            sbInitData.AppendLine(S(2) + "options:[" + (staticdata) + "],");
                            sbInitData.AppendLine(S(2) + "selValue:[]");
                            sbInitData.AppendLine(S(1) + "},");
                            if (apipath != "")
                            {
                                //初始化InitJs调用post获取
                                //sbOptionData.AppendLine(S(1) + "ftdpBase.optionsJson(config,vm,apiBase+'" + apipath + "',(json)=>{json.forEach(function(obj){obj['key'] = obj['value'];});" + row.Binding + ".options=" + row.Binding + ".options.concat(json);callback();});");
                            }
                        }
                        break;
                }
                // 使用temp后，初始化和数据定义依然有效
                if (!string.IsNullOrWhiteSpace(template))
                {
                    sbItem.Clear();
                    sbItem.Append(template);
                }
                StringBuilder sbItemMain=new StringBuilder();
                if (!string.IsNullOrWhiteSpace(template))
                {
                    sbItemMain.Append(sbItem.ToString());
                }
                else
                {
                    sbItemMain.Append(S(2) + "<el-form-item");
                    if (!string.IsNullOrWhiteSpace(row.Caption)) sbItemMain.Append(" label=\"" + row.Caption + "\"");
                    if (!string.IsNullOrWhiteSpace(modelBinding)) sbItemMain.Append(" prop=\"" + modelBinding.Substring(modelBinding.IndexOf('.') + 1) + "\"");
                    sbItemMain.AppendLine(">");
                    sbItemMain.Append(sbItem.ToString());
                    sbItemMain.Append(S(2) + "</el-form-item>");
                }
                if(!string.IsNullOrWhiteSpace(modelBinding) && row.ValidateType!="")
                {
                    var prop = modelBinding.Substring(modelBinding.IndexOf('.') + 1);
                    if (!dicRules.ContainsKey(prop))
                    {
                        var trigger = "blur";
                        if (row.Type == "select" || row.Type == "radiobox" || row.Type == "checkbox" || row.Type == "switch" || row.Type == "number" || row.Type == "slider" || row.Type == "rate" || row.Type == "color" || row.Type == "transfer" || row.Type == "cascader") trigger = "change";
                        switch (row.ValidateType)
                        {
                            case string s when s == res.ft.str("FFCVali.002"):
                                dicRules.Add(prop, @"[ { required: true, pattern:/\S/, message: '" + row.Caption + " "+ res.ftclass.str("z069") + "', trigger: '" + trigger + "' } ]");
                                break;
                            case string s when s == res.ft.str("FFCVali.003"):
                                dicRules.Add(prop, @"[ { required: true, pattern:/^-?\d+$/, message: '" + row.Caption + " "+ res.ftclass.str("z070") + "', trigger: '" + trigger + "' } ]");
                                break;
                            case string s when s == res.ft.str("FFCVali.004"):
                                dicRules.Add(prop, @"[ { required: true, pattern:/^-?\d*\.?\d+$/, message: '" + row.Caption + " "+ res.ftclass.str("z071") + "', trigger: '" + trigger + "' } ]");
                                break;
                            case string s when s == res.ft.str("FFCVali.005"):
                                dicRules.Add(prop, @"[ { required: true, pattern:/^\d{4}(\-)\d{1,2}\1\d{1,2}$/, message: '" + row.Caption + " "+ res.ftclass.str("z072") + "', trigger: '" + trigger + "' } ]");
                                break;
                            case string s when s == res.ft.str("FFCVali.006"):
                                dicRules.Add(prop, @"[ { required: true, pattern:/^1[3456789]\d{9}$/, message: '" + row.Caption + " "+ res.ftclass.str("z073") + "', trigger: '" + trigger + "' } ]");
                                break;
                            case string s when s == res.ft.str("FFCVali.007"):
                                dicRules.Add(prop, @"[ { required: true, pattern:/^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/, message: '" + row.Caption + " "+ res.ftclass.str("z074") + "', trigger: '" + trigger + "' } ]");
                                break;
                            case string s when s == res.ft.str("FFCVali.008"):
                                dicRules.Add(prop, @"[ { required: true, pattern:/^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$/, message: '" + row.Caption + " "+ res.ftclass.str("z075") + "', trigger: '" + trigger + "' } ]");
                                break;
                            case string s when s == res.ft.str("FFCVali.009"):
                                dicRules.Add(prop, @"[ { required: true, pattern:/^((https?|ftp|file):\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/, message: '" + row.Caption + " "+ res.ftclass.str("z076") + "', trigger: '" + trigger + "' } ]");
                                break;
                            case string s when s == res.ft.str("FFCVali.010"):
                                dicRules.Add(prop, @"[ { required: true, pattern:/[\u4E00-\u9FA5]/, message: '" + row.Caption + " "+ res.ftclass.str("z077") + "', trigger: '" + trigger + "' } ]");
                                break;
                            case string s when s.StartsWith("#LengthRange:"):
                                var vItem = s.Split(new char[] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                                var vMsg = "";
                                if (int.Parse(vItem[1]) > 0 && int.Parse(vItem[2]) < 9998) vMsg = res.ftclass.str("z078").Replace("{1}", vItem[1]).Replace("{2}", vItem[2]); 
                                else if (int.Parse(vItem[1]) > 0) vMsg = res.ftclass.str("z079")+$" {vItem[1]}";
                                else if (int.Parse(vItem[2]) < 9998) vMsg = res.ftclass.str("z080")+  $" {vItem[2]}";
                                dicRules.Add(prop, @"[ { required: "+(int.Parse(vItem[1]) > 0?"true":"false") +@", pattern:/^[\S\s]{"+ vItem[1] + ","+ vItem[2] + "}$/, message: '" + row.Caption + " "+ vMsg + "', trigger: '" + trigger + "' } ]");
                                break;
                            case string s when s == res.ft.str("FFCVali.012"):
                                if(!string.IsNullOrEmpty(row.ValidateCustomJs)) dicRules.Add(prop, row.ValidateCustomJs);
                                break;
                        }
                    }
                }
                FormItemList.Add(sbItemMain.ToString());
            }
            var spanList = obj.RowsList.Select(r => r.LayoutSpan).ToList();
            //通过字段的Layout布局
            if (spanList.Where(r => r != "").Count() > 0)
            {
                for (int i = 0; i < spanList.Count; i++)
                {
                    int sums = 0;
                    sbTemplate.AppendLine("<el-row>");
                    for (int j = i; j < spanList.Count; j++)
                    {
                        int cur = int.Parse(spanList[j]);
                        sbTemplate.AppendLine(S(1) + "<el-col :span=\"" + cur + "\">");
                        sbTemplate.AppendLine(S(2) + FormItemList[j]);
                        sbTemplate.AppendLine(S(1) + "</el-col>");
                        sums += cur;
                        if (sums == 24)
                        {
                            i = j;
                            sbTemplate.AppendLine("</el-row>");
                            break;
                        }
                        else if (sums > 24)
                        {
                            
                        }
                        else if ((j == spanList.Count - 1))
                        {
                            sbTemplate.AppendLine("</el-row>");
                        }
                    }
                }
            }
            else
            {
                string LayoutTempRow = "@ITEM@";
                if (obj.OtherSetDic.ContainsKey("Form_LayoutTempRow") && !string.IsNullOrWhiteSpace(obj.OtherSetDic["Form_LayoutTempRow"]))
                {
                    LayoutTempRow = obj.OtherSetDic["Form_LayoutTempRow"];
                }
                int index = LayoutTempRow.IndexOf("@ITEM@");
                int loop = 0;
                while (index >= 0)
                {
                    loop++;
                    LayoutTempRow = LayoutTempRow.Insert(index + 1, loop.ToString());
                    index = LayoutTempRow.IndexOf("@ITEM@");
                }
                if (loop > 0)
                {
                    for (int j = 0; j < FormItemList.Count / loop; j++)
                    {
                        string tempRow = LayoutTempRow;
                        for (int k = 1; k <= loop; k++)
                        {
                            tempRow = tempRow.Replace("@" + k + "ITEM@", FormItemList[j * loop + k - 1]);
                        }
                        sbTemplate.AppendLine(tempRow);
                    }
                    if (FormItemList.Count % loop > 0)
                    {
                        string tempRow = LayoutTempRow;
                        for (int k = 1; k <= FormItemList.Count % loop; k++)
                        {
                            tempRow = tempRow.Replace("@" + k + "ITEM@", FormItemList[FormItemList.Count - (FormItemList.Count % loop) + k - 1]);
                        }
                        for (int k = (FormItemList.Count % loop) + 1; k <= loop; k++)
                        {
                            tempRow = tempRow.Replace("@" + k + "ITEM@", "");
                        }
                        sbTemplate.AppendLine(tempRow);
                    }
                }
            }

            if (obj.ButtonList.Count > 0)
            {
                StringBuilder sbButton = new StringBuilder();
                foreach (var btn in obj.ButtonList)
                {
                    if (btn.IsGroupStart) sbButton.AppendLine(S(3) + "<el-button-group>");
                    sbButton.Append(S(3) + "<el-button");
                    if (!string.IsNullOrEmpty(btn.Type)) sbButton.Append(" type=\"" + btn.Type + "\"");
                    if (!string.IsNullOrEmpty(btn.Icon)) sbButton.Append(" icon=\"" + btn.Icon + "\"");
                    if (!string.IsNullOrEmpty(btn.Size)) sbButton.Append(" size=\"" + btn.Size + "\"");
                    if (btn.IsPlain) sbButton.Append(" plain");
                    if (btn.IsRound) sbButton.Append(" round");
                    if (btn.IsCircle) sbButton.Append(" circle");
                    if (!string.IsNullOrEmpty(btn.Click))
                    {
                        var btnClicks = btn.Click.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        if (btnClicks.Length > 1) btn.Click = btnClicks[btnClicks.Length - 1];
                    }
                    if (btn.Click.StartsWith("submit("))
                    {
                        sbButton.Append(" @click=\"" + btn.Click + "\"");
                        //sbButton.Append(" @click=\"submit(()=>{" + btn.Js + "})\"");
                        //if (!string.IsNullOrEmpty(btn.Js)) parentFuncList.Add(btn.Js);
                    }
                    else if (btn.Click.StartsWith("reset("))
                    {
                        sbButton.Append(" @click=\"" + btn.Click + "\"");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(btn.Click))
                        {
                            sbButton.Append(" @click=\"" + btn.Click + "\"");
                            if (!btn.Click.StartsWith("func(")) parentFuncList.Add(btn.Click);
                        }
                    }
                    sbButton.Append(">");
                    sbButton.Append(btn.Caption);
                    sbButton.AppendLine("</el-button>");
                    if (btn.IsGroupEnd) sbButton.AppendLine(S(3) + "</el-button-group>");
                }
                if (obj.OtherSetDic.ContainsKey("Form_LayoutTempButton") && !string.IsNullOrWhiteSpace(obj.OtherSetDic["Form_LayoutTempButton"]))
                {
                    sbTemplate.AppendLine(obj.OtherSetDic["Form_LayoutTempButton"].Replace("@BUTTON@", sbButton.ToString()));
                }
                else
                {
                    sbTemplate.AppendLine(S(2) + "<el-row><el-col style=\"text-align:center\">");
                    sbTemplate.Append(S(3) + sbButton.ToString());
                    sbTemplate.AppendLine(S(2) + "</el-col></el-row>");
                }
            }
            sbTemplate.AppendLine(S(1) + "</el-form>");
            sbTemplate.AppendLine("</template>");
            sbPreview.AppendLine("<div id=\"app1\">");
            sbPreview.AppendLine("<el-container>");
            sbPreview.AppendLine("<el-header style=\"text-align: center;\">");
            sbPreview.AppendLine("<el-row style=\"\">");
            sbPreview.AppendLine(S(1) + "<el-col :span=\"12\"><el-alert title=\""+ res.ftclass.str("z081") + "" + obj.ComName + " -- " + obj.Caption + "\"  type=\"warning\" center effect=\"dark\"></el-alert></el-col>");
            sbPreview.AppendLine(S(1) + "<el-col :span=\"12\"><el-input v-model=\"fillPara\" style=\"width: 300px\" placeholder=\""+ res.ftclass.str("z043") + "\"></el-input> <el-button @click=\"fill(fillPara)\">"+ res.ftclass.str("z082") + "</el-button></el-col>");
            sbPreview.AppendLine("</el-row>");
            sbPreview.AppendLine("</el-header>");
            sbPreview.AppendLine("<el-main>");
            sbPreview.Append(sbTemplate.ToString());
            sbPreview.AppendLine("</el-main>");
            sbPreview.AppendLine("</el-container>");
            sbPreview.AppendLine("</div>");
            //sbCom.AppendLine(sbDocument.ToString());
            sbCom.AppendLine(sbTemplate.ToString());
            #region 组件脚本和样式定义
            sbCom.AppendLine("<script>");
            sbCom.AppendLine("##TAG_01##//"+ res.ftclass.str("z045"));
            if (Custom_Js_All != "")
            {
                sbCom.AppendLine(res.ftclass.str("z083"));
                sbCom.AppendLine(Custom_Js_All);
            }
            sbCom.AppendLine("var apiBase=" + obj.ApiBase + ";");
            sbCom.AppendLine("var config={");
            sbCom.AppendLine(S(1) + "apiGet:apiBase+\"" + obj.ApiGet + "\",");
            sbCom.AppendLine(S(1) + "apiSet:apiBase+\"" + obj.ApiSet + "\",");
            sbCom.AppendLine(S(1) + "tokenKey:ftdpConfig.tokenKey,");
            sbCom.AppendLine(S(1) + "tokenVal:ftdpConfig.getTokenLocal(),");
            sbCom.AppendLine("}");
            sbCom.AppendLine(res.ftclass.str("z084"));
            sbCom.AppendLine("var form={");
            sbCom.Append(sbInitData.ToString());
            if (!string.IsNullOrWhiteSpace(obj.CusDataDefine)) sbCom.AppendLine(obj.CusDataDefine);
            sbCom.AppendLine("}");
            sbCom.AppendLine("var oriform='{}';");
            sbCom.AppendLine("var inited = false;");
            string exports = @"
##TAG_02## { 
##TAG_Components##    data: function() {
        return {
			form:form,
			page: { loading: false },
            ftdpConfig: ftdpConfig,
            fillPara:'',
##TAG_Rules##
        }
    },
	methods: {
		fill(paras) {
			form_paras=paras;
			ftdpBase.form_fill(config,this,form_paras,(obj)=>{form_fill_json(obj,this);oriform=JSON.stringify(form);},()=>{return js_beforeget(this)},(resData)=>{return js_beforeset(this,resData)},(resData)=>{js_afterset(this,resData)});
		},
		reset(allEmpty) {
    this.$refs.form.resetFields()
      ftdpBase.form_reset(form, oriform, allEmpty)
    },
        func(f) {
        const Fn = Function
        new Fn(f + '(this)')()
        },
		submit() {
this.$refs.form.validate((valid) => {
          if (valid) {
            if(submit_before(this))ftdpBase.form_submit(config,this,form_paras,form_submit_json(this),()=>{submit_after(this)});
          } else {
            return false;
          }
        });
		},
get(apiPath, paras, callback) {
      ftdpBase.normal_get(this, ftdpConfig, apiPath, paras, callback)
    },
    post(apiPath, paras, json, callback) {
      ftdpBase.normal_post(this, ftdpConfig, apiPath, paras, json, callback)
    },
fileDownload(fileInfo) {
        ftdpBase.file_download(this, ftdpConfig, fileInfo)
    }##TAG_04##
   },
	mounted:function(){
		if (!inited)init(this,()=>{oriform=JSON.stringify(form)});
        inited = true
	}
}
var form_paras='';";
            sbCom.AppendLine(exports);
            sbCom.AppendLine(res.ftclass.str("z085"));
            sbCom.AppendLine("function init(vm,callback)");
            sbCom.AppendLine("{");
            var initJs = (obj.OtherSetDic.ContainsKey("Form_FormInitJs") ? obj.OtherSetDic["Form_FormInitJs"] : "");
            sbCom.Append(initJs+(initJs==""?"":Environment.NewLine));
            sbCom.Append(sbOptionData.ToString());
            sbCom.AppendLine(S(1) + "callback();");
            sbCom.AppendLine("}");
            sbCom.AppendLine(res.ftclass.str("z086"));
            sbCom.AppendLine("function form_fill_json(detail,vm)");
            sbCom.AppendLine("{");
            sbCom.AppendLine(obj.BindGet);
            sbCom.AppendLine("}");
            sbCom.AppendLine(res.ftclass.str("z087"));
            sbCom.AppendLine("function form_submit_json(vm)");
            sbCom.AppendLine("{");
            sbCom.AppendLine("var json={};");
            sbCom.AppendLine(obj.BindSet);
            sbCom.AppendLine("return json;");
            sbCom.AppendLine("}");
            sbCom.AppendLine(res.ftclass.str("z088"));
            sbCom.AppendLine("function js_beforeget(vm)");
            sbCom.AppendLine("{");
            sbCom.AppendLine(obj.JsBeforeGet);
            sbCom.AppendLine("}");
            sbCom.AppendLine(res.ftclass.str("z089"));
            sbCom.AppendLine("function js_beforeset(vm,resData)");
            sbCom.AppendLine("{");
            sbCom.AppendLine(obj.JsBeforeSet);
            sbCom.AppendLine("}");
            sbCom.AppendLine(res.ftclass.str("z090"));
            sbCom.AppendLine("function js_afterset(vm,resData)");
            sbCom.AppendLine("{");
            sbCom.AppendLine(obj.JsAfterSet);
            sbCom.AppendLine("}");
            sbCom.AppendLine(res.ftclass.str("z091"));
            sbCom.AppendLine("function submit_before(vm)");
            sbCom.AppendLine("{");
            sbCom.AppendLine(obj.JsBeforeSubmit);
            sbCom.AppendLine("}");
            sbCom.AppendLine(res.ftclass.str("z092"));
            sbCom.AppendLine("function submit_after(vm)");
            sbCom.AppendLine("{");
            sbCom.AppendLine(obj.JsAfterSubmit);
            sbCom.AppendLine("}");
            //sbCom.AppendLine("//自定义脚本");
            //sbCom.AppendLine(obj.CustomJs);
            sbCom.AppendLine("</script>");
            sbCom.AppendLine("<style scoped>");
            sbCom.AppendLine(obj.CssText);
            sbCom.AppendLine("</style>");
            #endregion
            #region 组件脚本和样式定义
            sbPreview.AppendLine("<script>");
            sbPreview.AppendLine("//"+ res.ftclass.str("z045"));
            sbPreview.AppendLine("var apiBase=" + obj.ApiBase + ";");
            sbPreview.AppendLine("var config={");
            sbPreview.AppendLine(S(1) + "apiGet:apiBase+\"" + obj.ApiGet + "\",");
            sbPreview.AppendLine(S(1) + "apiSet:apiBase+\"" + obj.ApiSet + "\",");
            sbPreview.AppendLine(S(1) + "tokenKey:ftdpConfig.tokenKey,");
            sbPreview.AppendLine(S(1) + "tokenVal:ftdpConfig.getTokenLocal(),");
            sbPreview.AppendLine("}");
            sbPreview.AppendLine(res.ftclass.str("z084"));
            sbPreview.AppendLine("var form={");
            sbPreview.Append(sbInitData.ToString());
            if (!string.IsNullOrWhiteSpace(obj.CusDataDefine)) sbPreview.AppendLine(obj.CusDataDefine);
            sbPreview.AppendLine("}");
            sbPreview.AppendLine("var oriform='{}';");
            sbPreview.AppendLine("var inited = false;");
            string exportsPreview = @"//
var Main = {
    data: function() {
        return {
			form:form,
			page: { loading: false },
            ftdpConfig: ftdpConfig,
            fillPara:'',
##TAG_Rules##
        }
    },
	methods: {
		fill(paras) {
			form_paras=paras;
			ftdpBase.form_fill(config,this,form_paras,(obj)=>{form_fill_json(obj,this);oriform=JSON.stringify(form);},()=>{return js_beforeget(this)},(resData)=>{return js_beforeset(this,resData)},(resData)=>{js_afterset(this,resData)});
		},
		reset(allEmpty) {
    this.$refs.form.resetFields()
      ftdpBase.form_reset(form, oriform, allEmpty)
    },
		func(f)
		{
			eval(f+'(this)');
        },
		submit() {
this.$refs.form.validate((valid) => {
          if (valid) {
            if(submit_before(this))ftdpBase.form_submit(config,this,form_paras,form_submit_json(this),()=>{submit_after(this)});
          } else {
            return false;
          }
        });
		},
get(apiPath, paras, callback) {
      ftdpBase.normal_get(this, ftdpConfig, apiPath, paras, callback)
    },
    post(apiPath, paras, json, callback) {
      ftdpBase.normal_post(this, ftdpConfig, apiPath, paras, json, callback)
    },
fileDownload(fileInfo) {
    ftdpBase.file_download(this, ftdpConfig, fileInfo)
    }##TAG_04##
   },
	mounted:function(){
		if (!inited)init(this,()=>{oriform=JSON.stringify(form)});
        inited = true
	}
}
var form_paras='';";
            sbPreview.AppendLine(exportsPreview);
            sbPreview.AppendLine(res.ftclass.str("z085"));
            sbPreview.AppendLine("function init(vm,callback)");
            sbPreview.AppendLine("{");
            sbPreview.AppendLine(obj.OtherSetDic.ContainsKey("Form_FormInitJs") ? obj.OtherSetDic["Form_FormInitJs"] : "");
            sbPreview.AppendLine(sbOptionData.ToString());
            sbPreview.AppendLine(S(1) + "callback();");
            sbPreview.AppendLine("}");
            sbPreview.AppendLine(res.ftclass.str("z086"));
            sbPreview.AppendLine("function form_fill_json(detail,vm)");
            sbPreview.AppendLine("{");
            sbPreview.AppendLine(obj.BindGet);
            sbPreview.AppendLine("}");
            sbPreview.AppendLine(res.ftclass.str("z087"));
            sbPreview.AppendLine("function form_submit_json(vm)");
            sbPreview.AppendLine("{");
            sbPreview.AppendLine("var json={};");
            sbPreview.AppendLine(obj.BindSet);
            sbPreview.AppendLine("return json;");
            sbPreview.AppendLine("}");
            sbPreview.AppendLine(res.ftclass.str("z088"));
            sbPreview.AppendLine("function js_beforeget(vm)");
            sbPreview.AppendLine("{");
            sbPreview.AppendLine(obj.JsBeforeGet);
            sbPreview.AppendLine("}");
            sbPreview.AppendLine(res.ftclass.str("z089"));
            sbPreview.AppendLine("function js_beforeset(vm,resData)");
            sbPreview.AppendLine("{");
            sbPreview.AppendLine(obj.JsBeforeSet);
            sbPreview.AppendLine("}");
            sbPreview.AppendLine(res.ftclass.str("z090"));
            sbPreview.AppendLine("function js_afterset(vm,resData)");
            sbPreview.AppendLine("{");
            sbPreview.AppendLine(obj.JsAfterSet);
            sbPreview.AppendLine("}");
            sbPreview.AppendLine(res.ftclass.str("z091"));
            sbPreview.AppendLine("function submit_before(vm)");
            sbPreview.AppendLine("{");
            sbPreview.AppendLine(obj.JsBeforeSubmit);
            sbPreview.AppendLine("}");
            sbPreview.AppendLine(res.ftclass.str("z092"));
            sbPreview.AppendLine("function submit_after(vm)");
            sbPreview.AppendLine("{");
            sbPreview.AppendLine(obj.JsAfterSubmit);
            sbPreview.AppendLine("}");
            sbPreview.AppendLine("var Ctor = Vue.extend(Main);");
            sbPreview.AppendLine("new Ctor().$mount('#app1');");
            //sbPreview.AppendLine("//自定义脚本");
            //sbPreview.AppendLine(obj.CustomJs);
            sbPreview.AppendLine("</script>");
            sbPreview.AppendLine("<style>");
            sbPreview.AppendLine(obj.CssText);
            sbPreview.AppendLine("</style>");
            sbPreview.AppendLine("</body>");
            sbPreview.AppendLine("</html>");
            #endregion
            #endregion
            #region Html
            sbHtml.AppendLine(HTMLHeader);
            sbHtml.AppendLine("<script type='text/javascript'>");
            sbHtml.AppendLine("/*");
            sbHtml.AppendLine(S(1) + res.ftclass.str("z093") + obj.Caption);
            sbHtml.AppendLine(S(1) + res.ftclass.str("z094"));
            sbHtml.AppendLine(S(1) + res.ftclass.str("z095") + string.Join(",", parentFuncList));
            sbHtml.AppendLine("*/");
            sbHtml.AppendLine("httpVueLoader.register(Vue,'com/" + obj.ComName.Replace("/", "_") + ".vue?rdn='+(new Date()).valueOf());");
            sbHtml.AppendLine("</script>");
            sbHtml.AppendLine("<div id=\"app1\">");
            sbHtml.AppendLine("<el-container>");
            sbHtml.AppendLine("<el-header style=\"text-align: center;\">");
            sbHtml.AppendLine("<el-row style=\"\">");
            sbHtml.AppendLine(S(1) + "<el-col :span=\"12\"><el-alert title=\""+ res.ftclass.str("z081") + "" + obj.ComName + " -- " + obj.Caption + "\"  type=\"warning\" center effect=\"dark\"></el-alert></el-col>");
            sbHtml.AppendLine(S(1) + "<el-col :span=\"12\"><el-input v-model=\"fillPara\" style=\"width: 300px\" placeholder=\""+ res.ftclass.str("z043") + "\"></el-input> <el-button @click=\"fill(fillPara)\">"+ res.ftclass.str("z082") + "</el-button></el-col>");
            sbHtml.AppendLine("</el-row>");
            sbHtml.AppendLine("</el-header>");
            sbHtml.AppendLine("<el-main>");
            sbHtml.AppendLine(S(1) + "<" + obj.ComName.Replace("/", "_") + " desc=\"" + obj.Caption + "\" ref=\"" + obj.ComName.Replace("/", "_") + "\"></" + obj.ComName.Replace("/", "_") + ">");
            sbHtml.AppendLine("</el-main>");
            sbHtml.AppendLine("</el-container>");
            sbHtml.AppendLine("</div>");
            sbHtml.AppendLine(@"<script>
var Main = {
	data: function() {
        return {
            fillPara:'',
        }
    },
	methods: {
      fill(para) {
        this.$refs." + obj.ComName.Replace("/", "_") + @".fill(para);
      }
    },
};
var Ctor = Vue.extend(Main);
new Ctor().$mount('#app1');
</script>");
            sbHtml.Append(@"</body>
</html>");
            #endregion
            string comText = sbCom.ToString();
            string comTextPack = sbCom.ToString();
            string sbPreviewText = sbPreview.ToString();//##TAG_Components##
            var tag_com = "";
            if(Components.Count>0)
            {
                tag_com += "components: { ";
                for(int _i=0;_i<Components.Count;_i++)
                {
                    if (_i > 0) tag_com += ",";
                    tag_com += Components[_i];
                }
                tag_com += " },"+Environment.NewLine;
            }
            comText = comText.Replace("##TAG_01##", "").Replace("##TAG_Components##", "").Replace("##TAG_02##", "module.exports =").Replace("##TAG_03##", "==");
            comTextPack = comTextPack.Replace("##TAG_01##", "import { ftdpConfig } from '@/ftdp/utils/config.js'" + Environment.NewLine + "import { ftdpBase } from '@/ftdp/utils/base.js'" + Environment.NewLine).Replace("##TAG_Components##", tag_com).Replace("##TAG_02##", "export default ").Replace("##TAG_03##", "===");
            comTextPack = sbDocument.ToString() + Environment.NewLine + comTextPack;
            if (string.IsNullOrWhiteSpace(obj.CustomJs))
            {
                comText = comText.Replace("##TAG_04##", "");
                comTextPack = comTextPack.Replace("##TAG_04##", "");
                sbPreviewText = sbPreviewText.Replace("##TAG_04##", "");
            }
            else
            {
                comText = comText.Replace("##TAG_04##", "," + Environment.NewLine + res.ftclass.str("z056") + Environment.NewLine + obj.CustomJs);
                comTextPack = comTextPack.Replace("##TAG_04##", "," + Environment.NewLine + res.ftclass.str("z056") + Environment.NewLine + obj.CustomJs);
                sbPreviewText = sbPreviewText.Replace("##TAG_04##", "," + Environment.NewLine + res.ftclass.str("z056") + Environment.NewLine + obj.CustomJs);
            }
            StringBuilder sbRules=new StringBuilder();
            sbRules.AppendLine("formRules: {");
            foreach(var item in dicRules)
            {
                var prop=item.Key;
                var propItems = prop.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                sbRules.Append(propItems[0]+":");
                for(int i=1;i<propItems.Length;i++)
                {
                    sbRules.Append("{"+ propItems[i] + ":");
                }
                sbRules.Append(item.Value);
                for (int i = 1; i < propItems.Length; i++)
                {
                    sbRules.Append("}");
                }
                sbRules.AppendLine(",");
            }
            sbRules.Append("},");
            comText = comText.Replace("##TAG_Rules##", sbRules.ToString());
            comTextPack = comTextPack.Replace("##TAG_Rules##", sbRules.ToString());
            sbPreviewText = sbPreviewText.Replace("##TAG_Rules##", sbRules.ToString());
            return (comText, sbHtml.ToString(), sbPreviewText, comTextPack);
        }
        private Dictionary<string, Dictionary<string,string>> Form_Integration(FormCols obj)
        {
            var dic=new Dictionary<string,Dictionary<string,string>>();
            foreach(var col in obj.RowsList)
            {
                if(col.Type== "(Integration)")
                {
                    var trows = col.Template.Split(new string[] { Environment.NewLine},StringSplitOptions.None);
                    foreach(var trow in trows)
                    {
                        if(trow.Trim().StartsWith("@para{"))
                        {
                            col.Template = trow.Trim();
                            break;
                        }
                    }
                    var randomStr = new Random(DateTime.Now.Millisecond).Next(1000,9999)+""+ new Random(DateTime.Now.Millisecond+9).Next(1000, 9999);
                    var paraStr = Adv.ParaPattern(col.Template,null,false);
                    paraStr = paraStr.Replace("$(Form.Label)",col.Caption);
                    paraStr = paraStr.Replace("$(Form.PlaceHolder)", col.PlaceHolder);
                    paraStr = paraStr.Replace("$(Form.Bind.L)", col.Binding);
                    paraStr = paraStr.Replace("$(Form.Bind)", col.Binding.Split('.')[col.Binding.Split('.').Length-1]);
                    paraStr = paraStr.Replace("$(Form.Disabled)", col.Disable? " disabled":"");
                    paraStr = paraStr.Replace("$(Random)", randomStr);
                    var lines = paraStr.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    var dic2 = new Dictionary<string, string>();
                    string key = null;
                    StringBuilder sb = new StringBuilder();
                    for(int i=0;i<lines.Length;i++)
                    {
                        var curLine = lines[i];
                        if (curLine.Trim().StartsWith("##"))
                        {
                            if(key!=null)
                            {
                                var str = sb.ToString().Trim();
                                if(str!="") dic2.Add(key, str);
                            }
                            key = curLine.Trim().Substring(2);
                            sb = new StringBuilder();
                        }
                        else
                        {
                            sb.AppendLine(curLine);
                        }
                    }
                    var str2 = sb.ToString().Trim();
                    if (str2 != "") dic2.Add(key, str2);
                    dic.Add(col.Binding,dic2);
                }
            }
            return dic;
        }
        public (List<(string Emit, string Desc)> EmitList, List<(string Func, string Desc)> FuncList) DocFromCustomJs(string customJs, ListCols listObj, FormCols formObj)
        {
            var emitList = new List<(string Emit, string Desc)>();
            var funcList = new List<(string Func, string Desc)>();
            if (listObj != null)
            {
                foreach(var btn in listObj.ButtonList)
                {
                    if (!string.IsNullOrEmpty(btn.Click))
                    {
                        var btnClicks = btn.Click.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        var btnMimo = "";
                        var emitClick = btn.Click.Trim();
                        if (btnClicks.Length > 1)
                        {
                            btnMimo = btnClicks[0].Trim();
                            if (btnMimo.StartsWith("//")) btnMimo = btnMimo.Substring(2);
                            emitClick = btnClicks[btnClicks.Length - 1].Trim();
                        }
                        var emitIndex = emitClick.IndexOf("$emit(");
                        if (emitIndex >= 0)
                        {
                            var emitIndex2 = emitClick.IndexOf(',', emitIndex);
                            if (emitIndex2 < 0) emitIndex2 = emitClick.IndexOf(')', emitIndex);
                            if (emitIndex2 > 0)
                            {
                                var emitName = emitClick.Substring(emitIndex + 6, emitIndex2 - emitIndex - 6).Replace("'", "").Replace("\"", "");
                                emitList.Add(("@" + emitName, btnMimo));
                            }
                        }
                    }
                }
            }
            if (formObj != null)
            {
                foreach (var btn in formObj.ButtonList)
                {
                    if (!string.IsNullOrEmpty(btn.Click))
                    {
                        var btnClicks = btn.Click.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        var btnMimo = "";
                        var emitClick = btn.Click.Trim();
                        if (btnClicks.Length > 1)
                        {
                            btnMimo = btnClicks[0].Trim();
                            if (btnMimo.StartsWith("//")) btnMimo = btnMimo.Substring(2);
                            emitClick = btnClicks[btnClicks.Length - 1].Trim();
                        }
                        var emitIndex = emitClick.IndexOf("$emit(");
                        if (emitIndex >= 0)
                        {
                            var emitIndex2 = emitClick.IndexOf(',', emitIndex);
                            if (emitIndex2 < 0) emitIndex2 = emitClick.IndexOf(')', emitIndex);
                            if (emitIndex2 > 0)
                            {
                                var emitName = emitClick.Substring(emitIndex + 6, emitIndex2 - emitIndex - 6).Replace("'", "").Replace("\"", "");
                                emitList.Add(("@" + emitName, btnMimo));
                            }
                        }
                    }
                }
            }
            var lines = customJs.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (line.StartsWith("//") && i < lines.Length - 1)
                {
                    var desc = line.Substring(2);
                    var lineNext = lines[i + 1].Trim();
                    var emitIndex = lineNext.IndexOf("$emit(");
                    var funcIndex = lineNext.IndexOf(")");
                    if (emitIndex >= 0)
                    {
                        var emitIndex2 = lineNext.IndexOf(',', emitIndex);
                        if (emitIndex2 < 0) emitIndex2 = lineNext.IndexOf(')', emitIndex);
                        if (emitIndex2 > 0)
                        {
                            var emitName = lineNext.Substring(emitIndex + 6, emitIndex2 - emitIndex - 6).Replace("'", "").Replace("\"", "");
                            emitList.Add(("@"+ emitName,desc));
                        }
                    }
                    else if(funcIndex>0)
                    {
                        var funcName = lineNext.Substring(0, funcIndex + 1).Trim();
                        funcList.Add((funcName, desc));
                    }
                }
            }
            return (emitList, funcList);
        }
        public static string FrontPageCode(int PageId)
        {
            var pageObj = GetPageObj(Adv.RemoteSqlQuery("select * from ft_ftdp_front_page where id=" + PageId).Rows[0]);
            if (pageObj.TempId == 0) return pageObj.PageCode;
            var tempObj = GetTempObj(Adv.RemoteSqlQuery("select * from ft_ftdp_front_temp where id=" + pageObj.TempId).Rows[0]);
            var code = tempObj.TempCode;
            foreach (var key in tempObj.ComDefine.Keys)
            {
                var comCaption = tempObj.ComDefine[key].Caption;
                var comBind = "";
                if (pageObj.ComDefine.ContainsKey(key))
                {
                    comBind = pageObj.ComDefine[key].NewCom.Trim();
                }
                if(comBind!="")
                {
                    var cbKeyBind = new Dictionary<string, string>();
                    foreach (var cbKey in tempObj.ComDefine[key].CallBack.Keys)
                    {
                        var cbCaption = tempObj.ComDefine[key].CallBack[cbKey];
                        var cbBind = "";
                        if (pageObj.ComDefine.ContainsKey(key) && pageObj.ComDefine[key].CallBack.ContainsKey(cbKey))
                        {
                            cbBind = pageObj.ComDefine[key].CallBack[cbKey].Trim();
                        }
                        if(cbBind!="")
                        {
                            cbKeyBind.Add(cbKey,cbBind);
                        }
                    }
                    Regex r = new Regex(@"(<" + key + @"[^>]*>)");
                    MatchCollection mc = r.Matches(code);
                    foreach (Match m in mc)
                    {
                        var orips = m.Value;
                        foreach (var dk in cbKeyBind.Keys)
                        {
                            orips = orips.Replace(dk + "=", cbKeyBind[dk] + "=");
                        }
                        code = code.Replace(m.Value, orips);
                    }
                    r = new Regex(@"[^a-zA-Z0-9_]"+key+"[^a-zA-Z0-9_]");
                    mc = r.Matches(code);
                    foreach (Match m in mc)
                    {
                        var orips = m.Value;
                        orips = orips.Replace(key,comBind);
                        code = code.Replace(m.Value, orips);
                    }
                }
            }
            foreach (var key in tempObj.ParaDefine.Keys)
            {
                var paraDesc = tempObj.ParaDefine[key].Desc;
                var paraDefaultVal = tempObj.ParaDefine[key].DefaultVal;
                var paraSetVal = "";
                if (pageObj.ParaDefine.ContainsKey(key))
                {
                    paraSetVal = pageObj.ParaDefine[key];
                }
                if (paraSetVal == "") paraSetVal = paraDefaultVal;
                code = code.Replace(key, paraSetVal);
            }
            code = code.Replace("@PageName@", pageObj.PageName);
            return code;
        }

        public static FrontPage GetPageObj(DataRow dr)
        {
            var obj = new FrontPage();
            obj.Id = int.Parse(dr["Id"].ToString());
            obj.PageName = dr["PageName"].ToString();
            obj.Caption = dr["Caption"].ToString();
            obj.TempId = int.Parse(dr["TempId"].ToString());
            obj.PageCode = dr["PageCode"].ToString();
            Dictionary<string, (string, Dictionary<string, string>)> dic1 = new Dictionary<string, (string, Dictionary<string, string>)>();
            string[] rows0 = dr["ComDefine"].ToString().Split(new string[] { "{;;;}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rows1 in rows0)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string[] rows2 = rows1.Split(new string[] { "{:::}" }, StringSplitOptions.None);
                var key = rows2[0];
                var newCom = rows2[1];
                string[] rows3 = rows2[2].Split(new string[] { "{;;}" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string rows4 in rows3)
                {
                    string[] rows5 = rows4.Split(new string[] { "{::}" }, StringSplitOptions.None);
                    dic.Add(rows5[0], rows5[1]);
                }
                dic1.Add(key, (newCom, dic));
            }
            obj.ComDefine = dic1;
            Dictionary<string, string> dic2 = new Dictionary<string, string>();
            rows0 = dr["ParaDefine"].ToString().Split(new string[] { "{;;;}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rows1 in rows0)
            {
                string[] rows2 = rows1.Split(new string[] { "{:::}" }, StringSplitOptions.None);
                var key = rows2[0];
                dic2.Add(key, rows2[1]);
            }
            obj.ParaDefine = dic2;
            return obj;
        }
        public static FrontTemp GetTempObj(DataRow dr)
        {
            var obj = new FrontTemp();
            obj.Id = int.Parse(dr["Id"].ToString());
            obj.Caption = dr["Caption"].ToString();
            obj.TempCode = dr["TempCode"].ToString();
            obj.TempDesc = dr["TempDesc"].ToString();
            Dictionary<string, (string, Dictionary<string, string>)> dic1 = new Dictionary<string, (string, Dictionary<string, string>)>();
            string[] rows0 = dr["ComDefine"].ToString().Split(new string[] { "{;;;}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rows1 in rows0)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string[] rows2 = rows1.Split(new string[] { "{:::}" }, StringSplitOptions.None);
                var key = rows2[0];
                var caption = rows2[1];
                string[] rows3 = rows2[2].Split(new string[] { "{;;}" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string rows4 in rows3)
                {
                    string[] rows5 = rows4.Split(new string[] { "{::}" }, StringSplitOptions.None);
                    dic.Add(rows5[0], rows5[1]);
                }
                dic1.Add(key, (caption, dic));
            }
            obj.ComDefine = dic1;
            Dictionary<string, (string, string)> dic2 = new Dictionary<string, (string, string)>();
            rows0 = dr["ParaDefine"].ToString().Split(new string[] { "{;;;}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rows1 in rows0)
            {
                string[] rows2 = rows1.Split(new string[] { "{:::}" }, StringSplitOptions.None);
                var key = rows2[0];
                dic2.Add(key, (rows2[1], rows2[2]));
            }
            obj.ParaDefine = dic2;
            return obj;
        }

        public static (string docText,string templateText,string scriptText,string styleText) ComTextSplit(string comText)
        {
            string docText = "";
            string templateText = "";
            string scriptText = "";
            string styleText = "";
            var templateIndex=comText.IndexOf("<template>");
            if(templateIndex<0) templateIndex = comText.IndexOf("<template ");
            if(templateIndex>=0)
            {
                docText=comText.Substring(0,templateIndex);
                var scriptIndex = comText.IndexOf("<script>", templateIndex);
                if (scriptIndex < 0) scriptIndex = comText.IndexOf("<script ");
                if(scriptIndex>0)
                {
                    templateText=comText.Substring(templateIndex, scriptIndex- templateIndex);
                    var styleIndex = comText.IndexOf("<style>", scriptIndex);
                    if (styleIndex < 0) styleIndex = comText.IndexOf("<style ");
                    if (styleIndex > 0)
                    {
                        scriptText = comText.Substring(scriptIndex, styleIndex - scriptIndex);
                        styleText = comText.Substring(styleIndex);
                    }
                    else
                    {
                        scriptText = comText.Substring(scriptIndex);
                    }
                }
                else
                {
                    templateText = comText.Substring(templateIndex);
                }
            }
            return (docText, templateText, scriptText, styleText);
        }
        private string S(int i)
        {
            string s = "";
            for (int j = 0; j < i; j++) s += "\t";
            return s;
        }
    }
    /// <summary>
    /// 定义front_list表的列。没有表，则新增表，没有列新增列。
    /// </summary>
    public class ListCols
    {
        /// <summary>
        /// vue组件名称，根据类型会前面加list_或form_,在组件注释生成时会加上其他信息，例如表头、翻页情况，查询或设置情况
        /// </summary>
        public string ComName { get; set; }
        /// <summary>
        /// 显示的文本文字
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// var apiBase=ftdpConfig.apiBase;
        /// </summary>
        public string ApiBase { get; set; }
        /// <summary>
        /// /demo/xxx?list
        /// </summary>
        public string ApiUrl { get; set; }
        /// <summary>
        /// {&&&&}隔开,参数间用{||||}隔开
        /// </summary>
        public List<ListColsColumn> RowsList { get; set; } = new List<ListColsColumn>();
        /// <summary>
        /// {&&&&}隔开,参数间用{||||}隔开
        /// </summary>
        public List<ListColsSearch> SearchList { get; set; } = new List<ListColsSearch>();
        /// <summary>
        /// {&&&&}隔开 按钮的参数组织用{||||}隔开
        /// </summary>
        public List<ListColsButton> ButtonList { get; set; } = new List<ListColsButton>();
        /// <summary>
        /// 分页定义 用{::}和{;;}隔开
        /// </summary>
        public Dictionary<string, string> PagerDic { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 初始化设置orderBy orderType schText schStrict pageSize pageNum 及其他init方法执行的脚本
        /// </summary>
        public string InitSet { get; set; }
        /// <summary>
        /// js_beforeload() 
        /// </summary>
        public string JsBeforeLoad { get; set; }
        /// <summary>
        /// js_beforeset(resData)
        /// </summary>
        public string JsBeforeSet { get; set; }
        /// <summary>
        /// js_afterset(resData)
        /// </summary>
        public string JsAfterSet { get; set; }
        /// <summary>
        /// CustomJs
        /// </summary>
        public string CustomJs { get; set; }
        /// <summary>
        /// CssText
        /// </summary>
        public string CssText { get; set; }
        /// <summary>
        /// 其他配置，用{::}和{;;}隔开
        /// </summary>
        public Dictionary<string, string> OtherSetDic { get; set; } = new Dictionary<string, string>();
    }
    /// <summary>
    /// 定义front_form表的列。没有表，则新增表，没有列新增列。
    /// </summary>
    public class FormCols
    {
        /// <summary>
        /// vue组件名称，根据类型会前面加list_或form_,在组件注释生成时会加上其他信息，例如表头、翻页情况，查询或设置情况
        /// </summary>
        public string ComName { get; set; }
        /// <summary>
        /// 显示的文本文字
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// var apiBase=ftdpConfig.apiBase;
        /// </summary>
        public string ApiBase { get; set; }
        /// <summary>
        /// /demo/xxx?detail
        /// </summary>
        public string ApiGet { get; set; }
        /// <summary>
        /// /demo/xxx?mod
        /// </summary>
        public string ApiSet { get; set; }
        /// <summary>
        /// {&&&&}隔开
        /// </summary>
        public List<FormColsColumn> RowsList { get; set; } = new List<FormColsColumn>();
        /// <summary>
        /// {&&&&}隔开 按钮的参数组织用{||||}隔开
        /// </summary>
        public List<FormColsButton> ButtonList { get; set; } = new List<FormColsButton>();
        /// <summary>
        /// Get的Json绑定
        /// </summary>
        public string BindGet { get; set; }
        /// <summary>
        /// Set的Json绑定
        /// </summary>
        public string BindSet { get; set; }
        /// <summary>
        /// 数据提交之前的脚本
        /// </summary>
        public string JsBeforeSubmit { get; set; }
        /// <summary>
        /// 数据提交之后的脚本
        /// </summary>
        public string JsAfterSubmit { get; set; }
        /// <summary>
        /// fill赋值之前的脚本,还未获取到对象
        /// </summary>
        public string JsBeforeGet { get; set; }
        /// <summary>
        /// fill赋值之前的脚本,已获取到对象
        /// </summary>
        public string JsBeforeSet { get; set; }
        /// <summary>
        /// fill赋值之后的脚本
        /// </summary>
        public string JsAfterSet { get; set; }
        /// <summary>
        /// 自定义脚本
        /// </summary>
        public string CustomJs { get; set; }
        /// <summary>
        /// 样式
        /// </summary>
        public string CssText { get; set; }
        /// <summary>
        /// 自定义数据对象
        /// </summary>
        public string CusDataDefine { get; set; }
        /// <summary>
        /// 其他配置，用{::}和{;;}隔开
        /// </summary>
        public Dictionary<string, string> OtherSetDic { get; set; } = new Dictionary<string, string>();
    }
    public class ListColsColumn
    {
        public string Caption { set; get; }
        public string Binding { set; get; }
        public string Width { set; get; }
        public string Freezon { set; get; }
        public bool IsSort { set; get; }
        public bool IsSelection { set; get; }
        public string Template { set; get; }
    }
    public class ListColsSearch
    {
        public string Type { set; get; }
        public string Binding { set; get; }
        public string PlaceHolder { set; get; }
        public string Style { set; get; }
        public string InitData { set; get; }
    }
    public class ListColsButton
    {
        public string Type { set; get; }
        public string Caption { set; get; }
        public string Icon { set; get; }
        public bool IsPlain { set; get; }
        public bool IsRound { set; get; }
        public string Size { set; get; }
        public bool IsCircle { set; get; }
        public bool IsGroupEnd { set; get; }
        public bool IsGroupStart { set; get; }
        public string Click { set; get; }
        public string Js { set; get; }
    }
    public class FormColsColumn
    {
        public string Caption { get; set; }
        public string Binding { get; set; }
        public string Type { get; set; }
        public string PlaceHolder { get; set; }
        public string Style { get; set; }
        public bool Disable { get; set; }
        public string InitData { set; get; }
        public string Template { get; set; }
        public string LayoutSpan { get; set; }
        public string ValidateType { get; set; }
        public string ValidateCustomJs { get; set; }
    }
    public class FormColsButton
    {
        public string Type { set; get; }
        public string Caption { set; get; }
        public string Icon { set; get; }
        public bool IsPlain { set; get; }
        public bool IsRound { set; get; }
        public string Size { set; get; }
        public bool IsCircle { set; get; }
        public bool IsGroupEnd { set; get; }
        public bool IsGroupStart { set; get; }
        public string Click { set; get; }
        public string Js { set; get; }
    }
    public class FormValidation
    {
        public string Key { get; set; }
        public string Caption { get; set; }
        public string Js { get; set; }
    }
    public class FrontPage
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string Caption { get; set; }
        public int TempId { get; set; }
        public string PageCode { get; set; }
        public Dictionary<string, (string NewCom,Dictionary<string, string> CallBack)> ComDefine { get; set; }
        public Dictionary<string, string> ParaDefine { get; set; }
    }
    public class FrontTemp
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string TempCode { get; set; }
        public string TempDesc { get; set; }
        public Dictionary<string,(string Caption, Dictionary<string, string> CallBack)> ComDefine { get; set; }
        public Dictionary<string, (string Desc, string DefaultVal)> ParaDefine { get; set; }
    }

    public class FrontFunc
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
                var loopI = 0;
                sql = "select * from front_list";
                using (DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql)))
                {
                    while (dr.Read() && !BugResult.@break)
                    {
                        label.Text = labelOri + "   " + (++loopI);
                        Application.DoEvents();
                        var obj = ForeDev.GetListColsObj(dr);
                        if (string.IsNullOrWhiteSpace(obj.ApiUrl))
                        {
                            PutBug("list",obj, res.anew.GetString("String5"), 2, 0,"");
                        }
                        else
                        {
                            var suitUrl = obj.ApiUrl;
                            if (suitUrl.IndexOf('?') > 0 && suitUrl.IndexOf('/', suitUrl.IndexOf('?')) > 0)
                            {
                                suitUrl = suitUrl.Substring(0, suitUrl.IndexOf('/', suitUrl.IndexOf('?')));
                            }
                            sql = "select KeyDesc from ft_ftdp_apidoc where ApiPath='" + str.Dot2DotDot(suitUrl) + "'";
                            var dt = Adv.RemoteSqlQuery(sql, remoteDBType_Plat, remoteConnection_Plat);
                            if(dt.Rows.Count == 0)
                            {
                                PutBug("list", obj, res.anew.GetString("String6")+" " + suitUrl, 2, 0, "");
                            }
                            else
                            {
                                string keyDesc = dt.Rows[0][0].ToString();
                                string[] item0 = keyDesc.Split(new string[] { "{;;}" }, StringSplitOptions.RemoveEmptyEntries);
                                List<string> keysApi = new List<string>();
                                foreach (string item1 in item0)
                                {
                                    string[] item2 = item1.Split(new string[] { "{::}" }, StringSplitOptions.None);
                                    if (!keysApi.Contains(item2[0].ToLower())) keysApi.Add(item2[0].ToLower());
                                }
                                foreach(var row in obj.RowsList)
                                {
                                    if(!keysApi.Contains(row.Binding.ToLower()) && string.IsNullOrWhiteSpace(row.Template) && !string.IsNullOrWhiteSpace(row.Binding))
                                    {
                                        PutBug("list", obj, $"{row.Binding} "+ res.anew.GetString("String7"), 2, 1, row.Binding);
                                    }
                                }
                                foreach (var row in obj.SearchList)
                                {
                                    var sKey = row.Binding.ToLower();
                                    if (sKey.IndexOf('.') > 0) sKey = sKey.Split('.')[sKey.Split('.').Length - 1].ToLower();
                                    if(!obj.RowsList.Select(r=>r.Binding.ToLower()).Contains(sKey))
                                    {
                                        PutBug("list", obj, res.anew.GetString("String8").Replace("{row.Binding}", row.Binding), 2, 2, row.Binding);
                                    }
                                }
                            }
                        }
                    }
                }
                sql = "select * from front_form";
                using (DR dr = new DR(globalConst.CurSite.SiteConn.OpenRecord(sql)))
                {
                    while (dr.Read() && !BugResult.@break)
                    {
                        label.Text = labelOri + "   " + (++loopI);
                        Application.DoEvents();
                        var obj = ForeDev.GetFormColsObj(dr);
                        var isGet = !string.IsNullOrWhiteSpace(obj.ApiGet);
                        var isSet = !string.IsNullOrWhiteSpace(obj.ApiSet);
                        if (!isGet && !isSet)
                        {
                            PutBug("form", obj, res.anew.GetString("String9"), 2, 0, "");
                        }
                        else
                        {
                            if(isGet)
                            {
                                var suitUrl = obj.ApiGet;
                                if (suitUrl.IndexOf('?') > 0 && suitUrl.IndexOf('/', suitUrl.IndexOf('?')) > 0)
                                {
                                    suitUrl = suitUrl.Substring(0, suitUrl.IndexOf('/', suitUrl.IndexOf('?')));
                                }
                                sql = "select KeyDesc from ft_ftdp_apidoc where ApiPath='" + str.Dot2DotDot(suitUrl) + "'";
                                var dt = Adv.RemoteSqlQuery(sql, remoteDBType_Plat, remoteConnection_Plat);
                                if (dt.Rows.Count == 0)
                                {
                                    PutBug("form", obj, res.anew.GetString("String10") +" " + suitUrl, 2, 0, "");
                                }
                                else
                                {
                                    string keyDesc = dt.Rows[0][0].ToString();
                                    string[] item0 = keyDesc.Split(new string[] { "{;;}" }, StringSplitOptions.RemoveEmptyEntries);
                                    List<string> keysApi = new List<string>();
                                    foreach (string item1 in item0)
                                    {
                                        string[] item2 = item1.Split(new string[] { "{::}" }, StringSplitOptions.None);
                                        if (!keysApi.Contains("form." + item2[0].ToLower())) keysApi.Add("form." + item2[0].ToLower());
                                    }
                                    foreach (var row in obj.RowsList)
                                    {
                                        if (!keysApi.Contains(row.Binding.ToLower()) && string.IsNullOrWhiteSpace(row.Template) && !string.IsNullOrWhiteSpace(row.Binding))
                                        {
                                            PutBug("form", obj, $"{row.Binding} "+ res.anew.GetString("String11"), 2, 1, row.Binding);
                                        }
                                    }
                                    if(string.IsNullOrWhiteSpace(obj.BindGet))
                                    {
                                        PutBug("form", obj, res.anew.GetString("String12"), 2, 3,"");
                                    }
                                }
                            }
                            if (isSet)
                            {
                                var suitUrl = obj.ApiSet;
                                if (suitUrl.IndexOf('?') > 0 && suitUrl.IndexOf('/', suitUrl.IndexOf('?')) > 0)
                                {
                                    suitUrl = suitUrl.Substring(0, suitUrl.IndexOf('/', suitUrl.IndexOf('?')));
                                }
                                sql = "select KeyDesc from ft_ftdp_apidoc where ApiPath='" + str.Dot2DotDot(suitUrl) + "'";
                                var dt = Adv.RemoteSqlQuery(sql, remoteDBType_Plat, remoteConnection_Plat);
                                if (dt.Rows.Count == 0)
                                {
                                    PutBug("form", obj, res.anew.GetString("String13") + " " + suitUrl, 2, 0,"");
                                }
                                else
                                {
                                    string keyDesc = dt.Rows[0][0].ToString();
                                    string[] item0 = keyDesc.Split(new string[] { "{;;}" }, StringSplitOptions.RemoveEmptyEntries);
                                    List<string> keysApi = new List<string>();
                                    foreach (string item1 in item0)
                                    {
                                        string[] item2 = item1.Split(new string[] { "{::}" }, StringSplitOptions.None);
                                        if (!keysApi.Contains("form." + item2[0].ToLower())) keysApi.Add("form." + item2[0].ToLower());
                                    }
                                    foreach (var row in obj.RowsList)
                                    {
                                        if (!keysApi.Contains(row.Binding.ToLower()) && string.IsNullOrWhiteSpace(row.Template) && !string.IsNullOrWhiteSpace(row.Binding))
                                        {
                                            PutBug("form", obj, $"{row.Binding} "+ res.anew.GetString("String14"), 2, 1, row.Binding);
                                        }
                                    }
                                    if (string.IsNullOrWhiteSpace(obj.BindSet))
                                    {
                                        PutBug("form", obj, res.anew.GetString("String15"), 2, 4,"");
                                    }
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
            void PutBug(string type,object obj,string msg,int level,int tabIndex,string keyText)
            {
                var caption = type=="list"? ((ListCols)obj).Caption : ((FormCols)obj).Caption;
                var comname = type=="list"? ((ListCols)obj).ComName : ((FormCols)obj).ComName;

                var index = dataGridView.Rows.Add(new string[] {
                    type=="list"?"List":"Form",
                caption,
                comname,
                msg,
                res.form.GetString("position"),
            });
                dataGridView.Rows[index].Tag = new object[] { type, comname,obj, tabIndex, keyText };
                dataGridView.Rows[index].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridView.Rows[index].DefaultCellStyle.BackColor = level==3 ? System.Drawing.Color.Red:System.Drawing.Color.Yellow;
            }
        }
        public static string KeyDescGet(string apiurl)
        {
            string keyDesc = "";
            var suitUrl = apiurl;
            if (suitUrl.IndexOf('?') > 0 && suitUrl.IndexOf('/', suitUrl.IndexOf('?')) > 0)
            {
                suitUrl = suitUrl.Substring(0, suitUrl.IndexOf('/', suitUrl.IndexOf('?')));
            }
            string sql = "select KeyDesc from ft_ftdp_apidoc where ApiPath='" + str.Dot2DotDot(suitUrl) + "'";
            string connstr = Options.GetSystemDBSetConnStr_Plat();
            var conntype = Options.GetSystemDBSetType_Plat();
            if (conntype == globalConst.DBType.SqlServer)
            {
                using (SqlConnection db = new SqlConnection(connstr))
                {
                    db.Open();
                    using (SqlDataReader dr = new SqlCommand(sql, db).ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            keyDesc = dr.IsDBNull(0) ? "" : dr.GetString(0);
                        }
                    }
                }
            }
            else if (conntype == globalConst.DBType.MySql)
            {
                using (MySqlConnection db = new MySqlConnection(connstr))
                {
                    db.Open();
                    using (MySqlDataReader dr = new MySqlCommand(sql, db).ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            keyDesc = dr.IsDBNull(0) ? "" : dr.GetString(0);
                        }
                    }
                }
            }
            else if (conntype == globalConst.DBType.Sqlite)
            {
                using (var db = new DB(connstr))
                {
                    db.Open();
                    using (var dr = db.OpenRecord(sql))
                    {
                        if (dr.Read())
                        {
                            keyDesc = dr.IsDBNull(0) ? "" : dr.GetString(0);
                        }
                    }
                }
            }
            return keyDesc;
        }
        public static string BindGetGenerate(string apiurl,DataGridView dgv, List<FormColsColumn> cols)
        {
            if (string.IsNullOrWhiteSpace(apiurl)) return "";
            string keyDesc = KeyDescGet(apiurl);
            string[] item0 = keyDesc.Split(new string[] { "{;;}" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> keysApi = new List<string>();
            foreach (string item1 in item0)
            {
                string[] item2 = item1.Split(new string[] { "{::}" }, StringSplitOptions.None);
                if (!keysApi.Contains(item2[0].ToLower())) keysApi.Add(item2[0]);
            }
            StringBuilder sb = new StringBuilder();
            if(cols == null)
            {
                cols = new List<FormColsColumn>();
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    string caption = row.Cells[0].Value?.ToString() ?? "";
                    string binding = row.Cells[1].Value?.ToString() ?? "";
                    string datatype = row.Cells[2].Value?.ToString() ?? "";
                    string templateOrAttr = row.Cells[7].Value?.ToString() ?? "";
                    cols.Add(new FormColsColumn() {
                        Caption = caption,
                        Binding = binding,
                        Type = datatype,
                        Template = templateOrAttr
                    });
                }
            }
            foreach (var col in cols)
            {
                string caption = col.Caption;
                string binding = col.Binding;
                string datatype = col.Type;
                string templateOrAttr = col.Template;
                string attribute = !templateOrAttr.StartsWith("<") ? templateOrAttr : "";
                if (binding != "")
                {
                    string[] ss = binding.Split('.');
                    string key = ss[ss.Length - 1];
                    string matchedKey = null;
                    foreach (string s in keysApi)
                    {
                        if (s.Trim().ToLower() == key.ToLower())
                        {
                            matchedKey = s;
                            break;
                        }
                    }
                    bool isValueArr = false;
                    string appendStr = "";
                    if (datatype == "slider" || datatype == "rate")
                    {
                        appendStr = " "+res.ftclass.str("z096");
                    }
                    else if (datatype == "checkbox" || datatype == "transfer" || datatype == "cascader" || datatype == "selectMore" || (datatype == "select" && attribute.ToLower().IndexOf("multiple") >= 0))
                    {
                        appendStr = " "+ res.ftclass.str("z097");
                        isValueArr = true;
                    }
                    bool isSelValue = (datatype == "select" || datatype == "selectMore" || datatype == "radiobox" || datatype == "checkbox" || datatype == "transfer" || datatype == "cascader");
                    if (matchedKey != null)
                    {
                        if (isSelValue)
                        {
                            if (isValueArr)
                            {
                                sb.Append(binding + ".selValue = ftdpBase.toValueArray(detail." + matchedKey + "); //" + caption + appendStr + "" + Environment.NewLine);
                            }
                            else
                            {
                                sb.Append(binding + ".selValue = detail." + matchedKey + "; //" + caption + appendStr + "" + Environment.NewLine);
                            }
                        }
                        else if (datatype != "(Integration)" || matchedKey.EndsWith("_ishtml"))
                        {
                            sb.Append(binding + " = detail." + matchedKey + "; //" + caption + appendStr + "" + Environment.NewLine);
                        }
                        else
                        {
                            sb.Append("// " + binding + " " + caption + " "+ res.ftclass.str("z098") + Environment.NewLine);
                        }
                    }
                    else
                    {
                        sb.Append(binding + " = \"\"; //" + caption + appendStr + res.ftclass.str("z099") + Environment.NewLine);
                    }
                }
            }
            return sb.ToString();
        }
        public static string BindSetGenerate(string apiurl, DataGridView dgv, List<FormColsColumn> cols)
        {
            if (string.IsNullOrWhiteSpace(apiurl)) return "";
            string keyDesc = KeyDescGet(apiurl);
            string[] item0 = keyDesc.Split(new string[] { "{;;}" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> keysApi = new List<string>();
            foreach (string item1 in item0)
            {
                string[] item2 = item1.Split(new string[] { "{::}" }, StringSplitOptions.None);
                if (!keysApi.Contains(item2[0].ToLower())) keysApi.Add(item2[0]);
            }
            StringBuilder sb = new StringBuilder();
            if (cols == null)
            {
                cols = new List<FormColsColumn>();
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    string caption = row.Cells[0].Value?.ToString() ?? "";
                    string binding = row.Cells[1].Value?.ToString() ?? "";
                    string datatype = row.Cells[2].Value?.ToString() ?? "";
                    string templateOrAttr = row.Cells[7].Value?.ToString() ?? "";
                    cols.Add(new FormColsColumn()
                    {
                        Caption = caption,
                        Binding = binding,
                        Type = datatype,
                        Template = templateOrAttr
                    });
                }
            }
            foreach (var col in cols)
            {
                string caption = col.Caption;
                string binding = col.Binding;
                string datatype = col.Type;
                string templateOrAttr = col.Template;
                string attribute = !templateOrAttr.StartsWith("<") ? templateOrAttr : "";
                if (binding != "")
                {
                    string[] ss = binding.Split('.');
                    string key = ss[ss.Length - 1];
                    string matchedKey = null;
                    foreach (string s in keysApi)
                    {
                        if (s.Trim().ToLower() == key.ToLower())
                        {
                            matchedKey = s;
                            break;
                        }
                    }
                    bool isValueArr = false;
                    string appendStr = "";
                    if (datatype == "slider" || datatype == "rate")
                    {
                        appendStr = " "+ res.ftclass.str("z100");
                    }
                    else if (datatype == "checkbox" || datatype == "transfer" || datatype == "cascader" || datatype == "selectMore" || (datatype == "select" && attribute.ToLower().IndexOf("multiple") >= 0))
                    {
                        appendStr = " "+ res.ftclass.str("z101");
                        isValueArr = true;
                    }
                    bool isSelValue = (datatype == "select" || datatype == "selectMore" || datatype == "radiobox" || datatype == "checkbox" || datatype == "transfer" || datatype == "cascader");
                    if (matchedKey != null)
                    {
                        if (isSelValue)
                        {
                            if (isValueArr)
                            {
                                sb.Append("json['" + matchedKey + "'] = ftdpBase.toObjectArray(" + binding + ".selValue,'_AutoKey_'); //" + caption + appendStr + " "+ res.ftclass.str("z102") + Environment.NewLine);
                            }
                            else
                            {
                                sb.Append("json['" + matchedKey + "'] = " + binding + ".selValue; //" + caption + appendStr + "" + Environment.NewLine);
                            }
                        }
                        else if (datatype != "(Integration)" || matchedKey.EndsWith("_ishtml"))
                        {
                            sb.Append("json['" + matchedKey + "'] = " + binding + "; //" + caption + "" + Environment.NewLine);
                        }
                        else
                        {
                            sb.Append("// " + binding + " " + caption + " "+ res.ftclass.str("z098") + Environment.NewLine);
                        }
                    }
                    else
                    {
                        sb.Append("//json[''] = " + binding + "; //" + caption + appendStr + res.ftclass.str("z099") + Environment.NewLine);
                    }
                }
            }
            return sb.ToString();
        }

        public static string TempKeyPara(string oriStr,string caption,string placeholder,string binding)
        {
            oriStr = oriStr.Replace("$(Label)", caption);
            oriStr = oriStr.Replace("$(PlaceHolder)", placeholder);
            oriStr = oriStr.Replace("$(Bind.L)", binding);
            var bindingS = binding;
            if(binding.IndexOf('.')>0)
            {
                bindingS = binding.Split('.')[binding.Split('.').Length - 1];
            }
            oriStr = oriStr.Replace("$(Bind)", bindingS);
            return oriStr;
        }
    }
}

