function dl_load(page, orderby, ordertype, isexport, isexportsave, esfilecap, _cq, ftpartid, DefaultOrderBy, List_Code, List_OPID, RerationTreeevalvals, sqlevalvals, IsTree, RerationTree, SiteID, sql, RowAll, _Conststr, MainTable, ExportMax, RateNumType, NumsPerPage, JSTableHead, HeadIsShow, SelectType, ColorSelect, ColorDefault, RateNumType, JSTableTail, TurnPage, FirstPage, PrePage, NextPage, LastPage, CountZero, TurnIsShow, BlockDataDefine, CacuRowData,LoadEndJS,CustomConnection,loadingimg,custurnbtm,custurntop) {
    if (page == null) { eval("load_" + ftpartid + "(list_curpage_" + ftpartid + ");"); return false; }
    if (isexportsave == null) isexportsave = 0;
    eval("list_curpage_" + ftpartid + "=" + page + ";");
    if (orderby == null) orderby = ""; if (ordertype == null) ordertype = "";
    if (orderby == "") {
        var tdobj = $("#data_" + ftpartid).find("th[name=listth]").find("a:contains('△')")[0];
        if (tdobj != null) {
            var attr = $(tdobj).attr("onclick");
            orderby = attr.substring(attr.indexOf(",'") + 2, attr.indexOf("')"));
            ordertype = "asc";
        }
        else {
            tdobj = $("#data_" + ftpartid).find("th[name=listth]").find("a:contains('▽')")[0];
            if (tdobj != null) {
                var attr = $(tdobj).attr("onclick");
                orderby = attr.substring(attr.indexOf(",'") + 2, attr.indexOf("')"));
                ordertype = "desc";
            }
        }
    }
    else {
        if (ordertype == "") {
            var tdobj = $("#data_" + ftpartid).find("th[name=listth]").find("a[onclick*=\'" + orderby + "\']")[0];
            ordertype = "asc";
            if (tdobj != null) {
                if ($(tdobj).html().indexOf("△") >= 0) ordertype = "desc";
            }
        }
    }
    var schdefine = ""; var schtext = "";
    if ($("#scht_" + ftpartid)[0] != null) {
        schtext = $("#scht_" + ftpartid).val(); schdefine = $("#schtc_" + ftpartid).val();
    }
    var schstrict = "";
    $("#data_" + ftpartid).parent().prev().find("select[name=ftlistsearch]").each(function () {
        schstrict += this.id.split("__")[0] + ":" + this.value + ";";
    });
    var cuspagesize = -1;
    if ($("#ts_" + ftpartid)[0] != null) cuspagesize = parseInt($("#ts_" + ftpartid).val());
    if (cuspagesize == 0 && !confirm("若数据量较大（超过1000以上），全部显示会大大降低速度，并有可能导致超时，真的要全部显示吗？")) { $("#ts_" + ftpartid).val(-1); return false; }
    if (isexport == null || !isexport) {
        if (loadingimg != null && loadingimg != ''){
        $("#data_" + ftpartid).html("<div align='center' style='padding:20px 0px 20px 0px'><img border=0 src='"+loadingimg+"'/></div>");
        }
    }
    var cookiename = coldefine_cookiename(location.href, ftpartid);
    var curdefine = $.cookie(cookiename); if (curdefine == null) curdefine = "";
    var curdefine_width = $.cookie(cookiename + "_wd");
    if (isexport != null && isexport == true && isexportsave == 0) {
        var listtempform = document.getElementById("listexporttempform");
        if (listtempform == null) {
            var temformhtml = "";
            temformhtml += "<form id='listexporttempform' style='display:none' action='/_ftpub/ftformop.aspx' target='dhf" + ftpartid + "' method='post'>";
            temformhtml += "<input type=hidden name='optype' value='11'>";
            temformhtml += "<input type=hidden name='partid'>";
            temformhtml += "<input type=hidden name='ajaxtype'>";
            temformhtml += "<input type=hidden name='Order'>";
            temformhtml += "<input type=hidden name='orderby'>";
            temformhtml += "<input type=hidden name='ordertype'>";
            temformhtml += "<input type=hidden name='schdefine'>";
            temformhtml += "<input type=hidden name='schtext'>";
            temformhtml += "<input type=hidden name='schstrict'>";
            temformhtml += "<input type=hidden name='cuspagesize'>";
            temformhtml += "<input type=hidden name='schadv'>";
            temformhtml += "<input type=hidden name='UserCusCdn'>";
            temformhtml += "<input type=hidden name='List_Code'>";
            temformhtml += "<input type=hidden name='List_OPID'>";
            temformhtml += "<input type=hidden name='RerationTreeEvals'>";
            temformhtml += "<input type=hidden name='SqlEvals'>";
            temformhtml += "<input type=hidden name='IsTree'>";
            temformhtml += "<input type=hidden name='RerationTree'>";
            temformhtml += "<input type=hidden name='SiteID'>";
            temformhtml += "<input type=hidden name='sql'>";
            temformhtml += "<input type=hidden name='RowAll'>";
            temformhtml += "<input type=hidden name='Consts'>";
            temformhtml += "<input type=hidden name='MainTable'>";
            temformhtml += "<input type=hidden name='NeedExport'>";
            temformhtml += "<input type=hidden name='ExportMax'>";
            temformhtml += "<input type=hidden name='ExportSave'>";
            temformhtml += "<input type=hidden name='ExportSaveFileCap'>";
            temformhtml += "<input type=hidden name='RateNumType'>";
            temformhtml += "<input type=hidden name='CurPageNum'>";
            temformhtml += "<input type=hidden name='NumsPerPage'>";
            temformhtml += "<input type=hidden name='ColDefine_Cur'>";
            temformhtml += "<input type=hidden name='BlockDataDefine'>";
            temformhtml += "<input type=hidden name='UserCusSql'>";
            temformhtml += "<input type=hidden name='CacuRowData'>";
            temformhtml += "<input type=hidden name='CustomConnection'>";
            temformhtml += "</form>";
            $(temformhtml).appendTo($('body'));
        }
        listtempform = document.getElementById("listexporttempform");
        listtempform.partid.value = ftpartid;
        listtempform.ajaxtype.value = "List";
        listtempform.Order.value = DefaultOrderBy;
        listtempform.orderby.value = orderby;
        listtempform.ordertype.value = ordertype;
        listtempform.schdefine.value = schdefine;
        listtempform.schtext.value = schtext;
        listtempform.schstrict.value = schstrict;
        listtempform.cuspagesize.value = cuspagesize;
        listtempform.schadv.value = eval("list_adv_s_" + ftpartid);
        listtempform.UserCusCdn.value = eval("list_adv_c_" + ftpartid);
        listtempform.List_Code.value = List_Code;
        listtempform.List_OPID.value = List_OPID;
        listtempform.RerationTreeEvals.value = RerationTreeevalvals;
        listtempform.SqlEvals.value = sqlevalvals;
        listtempform.IsTree.value = IsTree;
        listtempform.RerationTree.value = RerationTree;
        listtempform.SiteID.value = SiteID;
        listtempform.sql.value = sql;
        listtempform.RowAll.value = RowAll;
        listtempform.Consts.value = _Conststr;
        listtempform.MainTable.value = MainTable;
        listtempform.NeedExport.value = (isexport == null ? "false" : isexport);
        listtempform.ExportMax.value = ExportMax;
        listtempform.ExportSave.value = isexportsave;
        listtempform.ExportSaveFileCap.value = esfilecap;
        listtempform.RateNumType.value = RateNumType;
        listtempform.CurPageNum.value = page;
        listtempform.NumsPerPage.value = NumsPerPage;
        listtempform.ColDefine_Cur.value = curdefine;
        listtempform.BlockDataDefine.value = BlockDataDefine;
        listtempform.UserCusSql.value = (_cq == null ? "" : _cq);
        listtempform.CacuRowData.value = CacuRowData;
        listtempform.CustomConnection.value = CustomConnection;
        listtempform.submit();
        return false;
    }
    $.post("/_ftpub/ftajax.aspx", { "partid": ftpartid, "ajaxtype": "List", "Order": DefaultOrderBy, "orderby": orderby, "ordertype": ordertype, "schdefine": schdefine, "schtext": schtext, "schstrict": schstrict, "cuspagesize": cuspagesize, "schadv": eval("list_adv_s_" + ftpartid), "UserCusCdn": eval("list_adv_c_" + ftpartid), "List_Code": List_Code, "List_OPID": List_OPID, "RerationTreeEvals": RerationTreeevalvals, "SqlEvals": sqlevalvals, "IsTree": IsTree, "RerationTree": RerationTree, "SiteID": SiteID, "sql": sql, "RowAll": RowAll, "Consts": _Conststr, "MainTable": MainTable, "NeedExport": (isexport == null ? "false" : isexport), "ExportMax": ExportMax, "ExportSave": isexportsave, "ExportSaveFileCap": esfilecap, "RateNumType": RateNumType, "CurPageNum": page, "NumsPerPage": NumsPerPage, "ColDefine_Cur": curdefine, "BlockDataDefine": BlockDataDefine, "UserCusSql": (_cq == null ? "" : _cq), "CacuRowData": CacuRowData,"CustomConnection":CustomConnection,"CusTurnBtm":custurnbtm,"CusTurnTop":custurntop },
   function (data, textStatus) {
       if (isexport != null && isexport == true) {
           if (isexportsave == 0) {
               var exportstr = unescape(data);
               dl_export(ftpartid, exportstr);
           }
           else {
               if (data == 'OK') dl_export_saveok(ftpartid);
           }
           return false;
       }
       if (data.indexOf("#fail:") == 0) {
           $("#data_" + ftpartid).html("<div align='center' style='padding:20px 0px 20px 0px'>" + unescape(data.substring(6)) + "</div>");
           return false;
       }
       var json = eval("(" + data + ")");
       var tablehead = JSTableHead;
       var _HeadIsShow = HeadIsShow;
       var _SelectType = SelectType;
       var _ColorSelect = ColorSelect;
       var _ColorDefault = ColorDefault;
       var _RateNumType = RateNumType;
       var tabletail = JSTableTail;
       var tablesavecolids = "";
       var tablebody = "";
       var count = parseInt(json["para"]["count"]);
       var pagesize = parseInt(json["para"]["pagesize"]);
       var pagenum = parseInt(json["para"]["pagenum"]);
       var ratenumtype = parseInt(json["para"]["ratenumtype"]);
       var turnhtml = TurnPage;
       var firstpage = FirstPage;
       var prepage = PrePage;
       var nextpage = NextPage;
       var lastpage = LastPage;
       var countzero = CountZero;
       var turnisshow = TurnIsShow;
       var tabletitle = "";
       var blockcolspan = 1;
       if (_HeadIsShow == 1) {
           if (_SelectType == 0 || _SelectType == 1) blockcolspan++; if (_RateNumType == 1 || _RateNumType == 2) blockcolspan++;
           var rowtitle = "";
           if (_SelectType == 0) {
               eval("rowtitle += listtitletd0_" + ftpartid + "(listtitlemselect_" + ftpartid + "(_ColorSelect,_ColorDefault))");
           }
           else if (_SelectType == 1) {
               eval("rowtitle += listtitletd0_" + ftpartid + "('')");
           }
           if (_RateNumType == 1 || _RateNumType == 2) {
               eval("rowtitle += listtitletd0_" + ftpartid + "('No.')");
           }
           var coltitles = coldefine_showext(eval('l_cd_' + ftpartid), curdefine);
           blockcolspan = blockcolspan + coltitles.length - 1;
           for (var coli = 0; coli < coltitles.length; coli++) {
               var dytitle = null;
               if (coltitles[coli][3] == '') dytitle = coltitles[coli][0];
               else dytitle = "<a href=\"javascript:void(0)\" title=\"点击排序\" onclick=\"load_" + ftpartid + "(1,'" + coltitles[coli][3] + "')\">" + coltitles[coli][0] + "</a>";
               switch (coli + 1) {
                   case 1: eval("rowtitle+=listtitletd1_" + ftpartid + "(coltitles[coli][1],coltitles[coli][1],coltitles[coli][2],dytitle)"); break;
                   case 2: eval("rowtitle+=listtitletd2_" + ftpartid + "(coltitles[coli][1],coltitles[coli][1],coltitles[coli][2],dytitle)"); break;
                   case 3: eval("rowtitle+=listtitletd3_" + ftpartid + "(coltitles[coli][1],coltitles[coli][1],coltitles[coli][2],dytitle)"); break;
                   case 4: eval("rowtitle+=listtitletd4_" + ftpartid + "(coltitles[coli][1],coltitles[coli][1],coltitles[coli][2],dytitle)"); break;
                   case 5: eval("rowtitle+=listtitletd5_" + ftpartid + "(coltitles[coli][1],coltitles[coli][1],coltitles[coli][2],dytitle)"); break;
                   case 6: eval("rowtitle+=listtitletd6_" + ftpartid + "(coltitles[coli][1],coltitles[coli][1],coltitles[coli][2],dytitle)"); break;
                   case 7: eval("rowtitle+=listtitletd7_" + ftpartid + "(coltitles[coli][1],coltitles[coli][1],coltitles[coli][2],dytitle)"); break;
                   case 8: eval("rowtitle+=listtitletd8_" + ftpartid + "(coltitles[coli][1],coltitles[coli][1],coltitles[coli][2],dytitle)"); break;
                   case 9: eval("rowtitle+=listtitletd9_" + ftpartid + "(coltitles[coli][1],coltitles[coli][1],coltitles[coli][2],dytitle)"); break;
                   case 10: eval("rowtitle+=listtitletd10_" + ftpartid + "(coltitles[coli][1],coltitles[coli][1],coltitles[coli][2],dytitle)"); break;
                   case 11: eval("rowtitle+=listtitletd11_" + ftpartid + "(coltitles[coli][1],coltitles[coli][1],coltitles[coli][2],dytitle)"); break;
                   case 12: eval("rowtitle+=listtitletd12_" + ftpartid + "(coltitles[coli][1],coltitles[coli][1],coltitles[coli][2],dytitle)"); break;
                   default: eval("rowtitle+=listtitletdother_" + ftpartid + "(coltitles[coli][1],coltitles[coli][1],coltitles[coli][2],dytitle)"); break;
               }
           }
           eval("tabletitle+=listtitletr_" + ftpartid + "(rowtitle)");
       }
       var tableturn = "";var turntop="";
       if (turnisshow == 1 && pagesize > 0) {
           if($.trim(custurntop)!="")eval("turntop=listturnpagetop_" + ftpartid + "(unescape(json['turntop']))");
           if($.trim(custurnbtm)!="")eval("tableturn=listturnpage_" + ftpartid + "(unescape(json['turnbtm']))");
           else{
           if (count == 0) turnhtml = countzero;
           else {
               if (pagenum > 1) {
                   prepage = "<a href='javascript:void(0)' onclick='load_" + ftpartid + "(" + (pagenum - 1) + ")'>" + prepage + "</a>";
                   firstpage = "<a href='javascript:void(0)' onclick='load_" + ftpartid + "(1)'>" + firstpage + "</a>";
               }
               if (pagenum < parseInt(((count - 1) / pagesize) + 1)) {
                   nextpage = "<a href='javascript:void(0)' onclick='load_" + ftpartid + "(" + (pagenum + 1) + ")'>" + nextpage + "</a>";
                   lastpage = "<a href='javascript:void(0)' onclick='load_" + ftpartid + "(" + parseInt(((count - 1) / pagesize) + 1) + ")'>" + lastpage + "</a>";
               }
               var turnpagebtn = "";
               var turnpageori = "turnpageori";
               if (turnhtml.indexOf("%B(") >= 0) {
                   turnpagebtn = turnhtml.substring(turnhtml.indexOf("%B(") + 3, turnhtml.indexOf(")", turnhtml.indexOf("%B(")));
                   turnpageori = "%B(" + turnpagebtn + ")";
                   turnpagebtn = "<BUTTON class='_button' type='button' onclick=\"dl_gotopage('" + ftpartid + "')\" id='tb_" + ftpartid + "'>" + turnpagebtn + "</BUTTON>";
               }
               var listturnpageinput_ = null; eval("listturnpageinput_=listturnpageinput_" + ftpartid + "(pagenum);");
               var listturnpageselect_ = null; eval("listturnpageselect_=listturnpageselect_" + ftpartid + "();");
               turnhtml = turnhtml.replace(" ", "&nbsp;").replace("%C", count).replace("%T", firstpage + "&nbsp;&nbsp;" + prepage + "&nbsp;&nbsp;" + nextpage + "&nbsp;&nbsp;" + lastpage).replace("%P", parseInt(((count - 1) / pagesize) + 1)).replace("%I", listturnpageinput_).replace(turnpageori, turnpagebtn).replace("%N", listturnpageselect_);

           }
           eval("tableturn=listturnpage_" + ftpartid + "(turnhtml)");
        }
       }
       
       
       var BlockHTML = "";
       for (var i = 0; i < json["data"].length; i++) {
           var item = json["data"][i];
           var fid = item["fid"];
           var row = item["row"];
           if (BlockDataDefine != null && BlockDataDefine != '') {
               BlockHTML += unescape(row).replace("{check}", eval("listtd0inner_" + ftpartid + "(fid)"));
               continue;
           }
           var rowdata = "";
           tablesavecolids += "," + fid;
           eval("rowdata+=listtd0_" + ftpartid + "(fid);");
           eval("if(ratenumtype==1)rowdata+=listtd0num_" + ftpartid + "(pagesize*(pagenum-1)+i+1);else rowdata+=listtd0num_" + ftpartid + "(i+1);");
           for (var j = 0; j < row.length; j++) {
               var col = row[j];
               if (col[2] != null && col[2] != "") {
                   eval("col[0]=savecolinput_" + ftpartid + "('f_" + fid + "_'+col[2],col[0]);");
               }
               switch (j) {
                   case 0: eval("rowdata+=listtd1_" + ftpartid + "(col[1],unescape(col[0]));"); break;
                   case 1: eval("rowdata+=listtd2_" + ftpartid + "(col[1],unescape(col[0]));"); break;
                   case 2: eval("rowdata+=listtd3_" + ftpartid + "(col[1],unescape(col[0]));"); break;
                   case 3: eval("rowdata+=listtd4_" + ftpartid + "(col[1],unescape(col[0]));"); break;
                   case 4: eval("rowdata+=listtd5_" + ftpartid + "(col[1],unescape(col[0]));"); break;
                   case 5: eval("rowdata+=listtd6_" + ftpartid + "(col[1],unescape(col[0]));"); break;
                   case 6: eval("rowdata+=listtd7_" + ftpartid + "(col[1],unescape(col[0]));"); break;
                   case 7: eval("rowdata+=listtd8_" + ftpartid + "(col[1],unescape(col[0]));"); break;
                   case 8: eval("rowdata+=listtd9_" + ftpartid + "(col[1],unescape(col[0]));"); break;
                   case 9: eval("rowdata+=listtd10_" + ftpartid + "(col[1],unescape(col[0]));"); break;
                   case 10: eval("rowdata+=listtd11_" + ftpartid + "(col[1],unescape(col[0]));"); break;
                   case 11: eval("rowdata+=listtd12_" + ftpartid + "(col[1],unescape(col[0]));"); break;
                   default: eval("rowdata+=listtdother_" + ftpartid + "(col[1],unescape(col[0]));");
               }
           }
           eval("if((i%2)==0)rowdata=listtr0_" + ftpartid + "(rowdata);else rowdata=listtr1_" + ftpartid + "(rowdata);");
           tablebody += rowdata;
       }
       if (BlockDataDefine != null && BlockDataDefine != '') {
           eval("tablebody+=listtrtddatablock_" + ftpartid + "(blockcolspan,BlockHTML)");
       }
       if (tablesavecolids != '') tablesavecolids = tablesavecolids.substring(1);
       tablesavecolids = "<input type=hidden name=\"dlidsall\" value=\"" + tablesavecolids + "\"/>";
       var tablecacu = '';
       eval("tablecacu=listtrcacu_" + ftpartid + "(unescape(json['cacu']))");
       var tabletitleapd = null;
       eval("tabletitleapd=list_title_apd_" + ftpartid);
       $("#data_" + ftpartid).html(turntop+tablehead +tabletitleapd+ tabletitle + tablebody + tablecacu + tabletail + tablesavecolids + tableturn);
       if (orderby != "") {
           $("#data_" + ftpartid).find("th[name=listth]").find("a").each(function () {
               $(this).html($(this).html().replace('&nbsp;△', '').replace('&nbsp;▽', ''));
           });
           var tdobj = $("#data_" + ftpartid).find("th[name=listth]").find("a[onclick*=\'" + orderby + "\']")[0];
           if (tdobj != null) {
               if (ordertype == "asc") $(tdobj).html($(tdobj).html() + '&nbsp;△');
               else if (ordertype == "desc") $(tdobj).html($(tdobj).html() + '&nbsp;▽');
           }
       }
       if ($("#ts_" + ftpartid)[0] != null) $("#ts_" + ftpartid).val(cuspagesize);
       StyleInit($("#data_" + ftpartid)[0]);
       if (curdefine_width != null && curdefine_width != "") { $(document.body).css('width', curdefine_width); }
       fttableslip();if(LoadEndJS!=null&&LoadEndJS!="")eval(LoadEndJS);
   }, "text");
    if (eval("list_adv_s_" + ftpartid + "==null||list_adv_s_" + ftpartid + "==''")) {
        $('#btna_' + ftpartid).removeAttr('style').removeAttr('title');
    }
    else {
        $('#btna_' + ftpartid).css('color', '#ff6600').attr('title', '高级条件已设置').tooltip();
    }
    if (eval("list_adv_c_" + ftpartid + "==null||list_adv_c_" + ftpartid + "==''")) {
        $('#btnc_' + ftpartid).removeAttr('style').removeAttr('title');
    }
    else {
        $('#btnc_' + ftpartid).css('color', '#ff6600').attr('title', '自定义条件已设置').tooltip();
    }
}
function dl_openWindow(url, winname, W, H, S) {
    if (url.indexOf("?") >= 0) url += "&rmd=" + Math.random();
    else url += "?rmd=" + Math.random();
    if (winname = null) {
        winname = "";
    }
    if (W == null) {
        window.open(url, winname);
    }
    else {
        var L = (screen.width - W) / 2;
        var T = (screen.height - H) / 2;
        window.open(url, winname, 'left=' + L + ',top=' + T + ',width=' + W + ',height=' + H + ',toolbar=no,menubar=no,scrollbars=' + S);
    }
}
function dl_doMore(obj, url, W, H, S) {
    var id = dl_SelectMore(obj, true);
    if (id == null) return;
    if (url.indexOf("?") >= 0) url = url + "&ids=" + id;
    else url = url + "?ids=" + id;
    dl_openWindow(url, W, H, S);
    return true;
}
function dl_SelectOne(obj) {
    j = 0;
    var ids = "";
    var objs = $(obj).parent().parent().parent().parent().parent().find("div.layui-form-checkbox");
    objs.each(function () {
        if ($(this).hasClass('listckbox') && dl_checked(this)) {
            ids += "," + $(this).attr('layid');
            j++;
        }
    })
    if (j == 0) {
        alert("请选择项");
        return null;
    }
    if (j > 1) {
        alert("您只能选中一个");
        return null;
    }
    return ids.substring(1);
}

function dl_SelectMore(obj, showmsg) {
    j = 0;
    var ids = "";
    var objs = $(obj).parent().parent().parent().parent().parent().find("div.layui-form-checkbox");
    objs.each(function () {
        if ($(this).hasClass('listckbox') && dl_checked(this)) {
            ids += "," + $(this).attr('layid');
            j++;
        }
    })
    if (j == 0) {
        if (showmsg) alert("请选择项");
        return null;
    }
    return ids.substring(1);
}
function dl_one(obj, url, Wname, W, H, S) {
    var id = dl_SelectOne(obj);
    if (id != null) {
        var patten = "";
        if (url.indexOf("?") >= 0) patten = "&";
        else patten = "?";
        dl_openWindow(url + patten + "fid=" + id, Wname, W, H, S);
    }
}
function dl_search(ftpartid) {
    eval("load_" + ftpartid + "(1)");
}
function dl_cuscdn(ftpartid, cuscdncols) {
    var id = "dialog_listcuscdn" + ftpartid;
    if ($("#" + id)[0] == null) {
        var s = "<div id=\"" + id + "\" style=\"display:none;text-align:left;padding-top:20px\" class='ftdiv'>";
        var cdns = cuscdncols.split(';');
        for (var i = 0; i < cdns.length; i++) {
            if (cdns[i] == '') continue;
            var item = cdns[i].split('|');
            s += "<span style='display:block;margin:5px;padding-bottom:3px;border-bottom:solid 1px #e1e1e1'><span style='float:left;width:80px;padding-top:6px'>" + item[1] + "</span><select id='cuscdn_p_" + ftpartid + i + "' onchange='dl_cuscdn_generate(\"" + ftpartid + "\",\"" + cuscdncols + "\")'><option value='like'>包含</option><option value='='>等于</option><option value='<'>小于</option><option value='<='>小于等于</option><option value='>'>大于</option><option value='>='>大于等于</option><option value='!='>不等于</option></select>&nbsp;<input id='cuscdn_t_" + ftpartid + i + "' type=text onKeyUp='if(event.keyCode==13){list_adv_c_" + ftpartid + "=eleVal(\"cuscdn_q_" + ftpartid + "\");dl_search(\"" + ftpartid + "\");closeDialog(\"" + id + "\")}' onchange='dl_cuscdn_generate(\"" + ftpartid + "\",\"" + cuscdncols + "\")' oninput='dl_cuscdn_generate(\"" + ftpartid + "\",\"" + cuscdncols + "\")' class=_input style='width:200px;margin-right:4px'/><span class=fttip>" + (item.length > 2 ? item[2] : "") + "</span></span>";
        }
        s += '<span style="display:block;text-align:right;margin:5px"><button class="_button" tag="ui-icon-check" onclick="if(event!=null&&event.shiftKey){$(\'#cuscdn_q_' + ftpartid + '\').show();return false;}list_adv_c_' + ftpartid + '=eleVal(\'cuscdn_q_' + ftpartid + '\');dl_search(\'' + ftpartid + '\');closeDialog(\'' + id + '\')">确定并查询</button></span>';
        s += '<span style="display:block;text-align:right;margin:5px"><textarea id="cuscdn_q_' + ftpartid + '" style="width:100%;height:100px;display:none"></textarea></span>';
        s += "</div>";
        $(s).appendTo($(document.body));
        StyleInit(O(id));
        $(O(id)).find("._button").each(function () {
            if ($(this).find("span:eq(1)").find("span")[0] != null) {
                $(this).find("span:eq(1)").removeAttr("class");
                $(this).find("span:eq(0)").remove();
            }
        });
    }
    $(O(id)).dialog({
        modal: true,
        resizable: true,
        width: 600,
        show: 200,
        title: "自定义查询条件"
    });
}
function dl_cuscdn_generate(ftpartid, cuscdncols) {
    var cdns = cuscdncols.split(';');
    var cdn = '';
    for (var i = 0; i < cdns.length; i++) {
        if (cdns[i] == '') continue;
        var item = cdns[i].split('|');
        var cdnval = $.trim(eleVal("cuscdn_t_" + ftpartid + i));
        if (cdnval != '') {
            cdnval = cdnval.replace(/'/g, "''");
            switch (eleVal("cuscdn_p_" + ftpartid + i)) {
                case 'like': cdn += " and " + item[0] + " like '%" + cdnval + "%'"; break;
                case '=': cdn += " and " + item[0] + "='" + cdnval + "'"; break;
                case '<': cdn += " and " + item[0] + "<'" + cdnval + "'"; break;
                case '<=': cdn += " and " + item[0] + "<='" + cdnval + "'"; break;
                case '>': cdn += " and " + item[0] + ">'" + cdnval + "'"; break;
                case '>=': cdn += " and " + item[0] + ">='" + cdnval + "'"; break;
                case '!=': cdn += " and " + item[0] + "!='" + cdnval + "'"; break;
            }
        }
    }
    eleVal("cuscdn_q_" + ftpartid, cdn);
}
function dl_refresh(ftpartid) {
    eval("list_adv_s_" + ftpartid + "='';");
    eval("list_adv_c_" + ftpartid + "='';");
    if ($("#scht_" + ftpartid)[0] != null) {
        $("#scht_" + ftpartid).val("");
    }
    if ($("#dialog_listcuscdn" + ftpartid)[0] != null) {
        $("#dialog_listcuscdn" + ftpartid).remove();
    }
    $("#data_" + ftpartid).parent().prev().find("select[name=ftlistsearch]").each(function () {
        this.value = "";
    });
    eval("load_" + ftpartid + "(1)");
}

function dl_delete(obj, siteid, partid) {
    var fid = dl_SelectMore(obj, true);
    if (fid == null) return;
    if (!confirm("您确定要删除已选的项吗?")) return;
    newDialog("loading", "loading");
    document.getElementById("dlf" + partid).action = "/_ftpub/ftformop.aspx";
    document.getElementById("dlf" + partid).target = "dhf" + partid;
    document.getElementById("dlf" + partid).siteid.value = siteid;
    document.getElementById("dlf" + partid).ftformstat.value = 0;
    document.getElementById("dlf" + partid).ftformflow.value = "";
    document.getElementById("dlf" + partid).optype.value = 0;
    document.getElementById("dlf" + partid).ftformid.value = fid;
    document.getElementById("dlf" + partid).curpartid.value = partid;
    document.getElementById("dlf" + partid).submit();
    return true;
}

function dl_saveto(obj, tabletag, siteid, partid) {
    var fid = dl_SelectMore(obj, false);
    var str = null;
    if (fid == null) str = "没有选中要保存的项，确认要全部保存吗？";
    else str = "确定要保存选中的项吗？";
    if (!confirm(str)) return;
    newDialog("loading", "loading");
    document.getElementById("dli" + partid).action = "/_ftpub/ftformop.aspx";
    document.getElementById("dli" + partid).target = "dhf" + partid;
    document.getElementById("dli" + partid).savetabletag.value = tabletag;
    document.getElementById("dli" + partid).siteid.value = siteid;
    document.getElementById("dli" + partid).optype.value = 2;
    document.getElementById("dli" + partid).submit();
    return true;
}

function dl_copy(obj, siteid, partid) {
    var fid = dl_SelectMore(obj, true);
    if (fid == null) return;
    newDialog("loading", "loading");
    document.getElementById("dlf" + partid).action = "/_ftpub/ftformop.aspx";
    document.getElementById("dlf" + partid).target = "dhf" + partid;
    document.getElementById("dlf" + partid).siteid.value = siteid;
    document.getElementById("dlf" + partid).ftformstat.value = 0;
    document.getElementById("dlf" + partid).ftformflow.value = "";
    document.getElementById("dlf" + partid).optype.value = 1;
    document.getElementById("dlf" + partid).ftformid.value = fid;
    document.getElementById("dlf" + partid).curpartid.value = partid;
    document.getElementById("dlf" + partid).submit();
    return true;
}

function dl_flow(obj, siteid, partid, newflow) {
    var fid = dl_SelectMore(obj, true);
    if (fid == null) return;
    newDialog("loading", "loading");
    document.getElementById("dlf" + partid).action = "/_ftpub/ftformop.aspx";
    document.getElementById("dlf" + partid).target = "dhf" + partid;
    document.getElementById("dlf" + partid).siteid.value = siteid;
    document.getElementById("dlf" + partid).ftformstat.value = "";
    document.getElementById("dlf" + partid).ftformflow.value = newflow;
    document.getElementById("dlf" + partid).optype.value = 0;
    document.getElementById("dlf" + partid).ftformid.value = fid;
    document.getElementById("dlf" + partid).curpartid.value = partid;
    document.getElementById("dlf" + partid).submit();
    return true;
}
var dl_export_save_bind_setting = {
    view: {
        dblClickExpand: true,
        showLine: true,
        selectedMulti: false,
        nameIsHTML: true
    },
    data: {
        simpleData: {
            enable: true,
            idKey: "id",
            pIdKey: "pId",
            rootPId: null
        }
    },
    check: {
        enable: true,
        autoCheckTrigger: true,
        chkboxType: { "Y": "ps", "N": "ps" }
    }
};

function dl_export_savefiledel(partid, fileid) {
    newDialog('loading', 'loading');
    $.post("/_ftpub/ftajax.aspx", { "fileid": fileid, "ajaxtype": "List_ExportSaveFiles_Del" }, function (data, textStatus) {
        if (data == 'OK') { closeDialog('loading'); dl_export_savefiles(partid + '_part'); }
        else _loading2fai(data);
    }, "text");
}
var dl_export_saveuserbind_tree = null;
function dl_export_saveuserbind(fileid) {
    var id = "dialog_listexportbind";
    if ($("#" + id)[0] != null) $("#" + id).remove();
    var s = '<DIV id=' + id + '>';
    s += '<DIV align=right>';
    s += '<button class="_button tip" tag="ui-icon-check" onclick="dl_export_saveuserbind_do(\'' + fileid + '\')">确定</button>';
    s += '</DIV>';
    s += '<UL id=' + id + 'tree class=ztree style="OVERFLOW: auto; WIDTH: 95%"></UL></DIV>';
    $(s).appendTo($(document.body));
    StyleInit(O(id));
    $(O(id)).dialog({
        modal: true,
        resizable: true,
        width: 500,
        show: 200,
        title: "分享给指定用户"
    });
    newDialog("info", "loading", "正在载入...", false);
    $.post("/_ftpub/ftajax.aspx", { "ajaxtype": "List_ExportSaveFiles_Tree", "fileid": fileid }, function (data, textStatus) {
        closeDialog("info");
        dl_export_saveuserbind_tree = $.fn.zTree.init($("#" + id + 'tree'), dl_export_save_bind_setting, eval(data));
    }, "text");
}
function dl_export_saveuserbind_do(fileid) {
    newDialog('loading', 'loading');
    var s = "";
    var objs = dl_export_saveuserbind_tree.getCheckedNodes(true);
    for (var i = 0; i < objs.length; i++) {
        if (objs[i].id.indexOf('2_') == 0) s += ',' + objs[i].id.substring(2);
    }
    if (s != '') s = s.substring(1);
    $.post("/_ftpub/ftajax.aspx", { "fileid": fileid, "userids": s, "ajaxtype": "List_ExportSaveFiles_Bind" }, function (data, textStatus) {
        if (data == 'OK') { closeDialog('loading'); closeDialog('dialog_listexportbind'); }
        else _loading2fai(data);
    }, "text");
}
function dl_export_savestart(partid) {
    var id = "dialog_listexportfilecap";
    if ($("#" + id)[0] != null) $("#" + id).remove();
    var s = "<div id=\"" + id + "\" style=\"display:none;text-align:left;padding-top:30px\" class='ftdiv'>";
    s += "显示的文件名称：<input type=text class=_input style='width:250px;margin-right:4px'/>";
    s += '<button class="_button tip" tag="ui-icon-clock" onclick="newDialog(\'loading\',\'loading\');load_' + partid + '(1,null,null,true,1,$(this).prev().val())">开始生成</button>';
    s += "</div>";
    $(s).appendTo($(document.body));
    StyleInit(O(id));
    $(O(id)).find("._button").each(function () {
        if ($(this).find("span:eq(1)").find("span")[0] != null) {
            $(this).find("span:eq(1)").removeAttr("class");
            $(this).find("span:eq(0)").remove();
        }
    });
    $(O(id)).dialog({
        modal: true,
        resizable: false,
        width: 500,
        show: 200,
        title: "文件名称："
    });
}
function dl_export_saveok(partid) {
    if ($("#dialog_listexportfilecap")[0] != null) closeDialog('dialog_listexportfilecap');
    _loading2suc("dl_export_savefiles('" + partid + "')");
}
function dl_export_savefiles(partid) {
    $('#listexportspan' + partid).html("<div align='center' style='padding:20px 0px 20px 0px'><img border=0 src='/_ftres/progress.gif'/></div>");
    $.post("/_ftpub/ftajax.aspx", { "partid": partid, "ajaxtype": "ListExportSave" }, function (data, textStatus) {
        $('#listexportspan' + partid).html(data);
    }, "text");
}
function dl_export_all(partid) {
    var id = "dialog_listexport";
    if ($("#" + id)[0] != null) $("#" + id).remove();
    var s = "<div id=\"" + id + "\" style=\"display:none;text-align:right\">";
    s += "<input type=hidden autofocus >";
    s += '<button class="_button tip" style="margin-right:10px" tag="ui-icon-extlink" title="数据量较少且网络条件较好时，可选择直接导出" onclick="load_' + partid + '(1,null,null,true)">直接导出</button>';
    s += '<button class="_button tip" tag="ui-icon-disk" title="在服务器生成数据文件，生成后可随时下载，生成的数据文件且可与指定用户分享<br>适用于导出大数据文件，或者相同的数据需多次导出时使用" onclick="dl_export_savestart(\'' + partid + '\')">生成再下载</button>';
    s += '<div style="text-align:left" id="listexportspan' + partid + '" class="ftselect">';
    //s+="<table width=100% style='margin-top:6px' cellspacing=1 cellpadding=1>";
    //s+="<tr style='color:#888888'><td>文件</td><td>行数</td><td>时间</td><td>生成人</td><td>&nbsp;</td></tr>";
    //s+="<tr><td><a href=#>文件</a></td><td>数据行数</td><td>生成时间</td><td>生成人</td><td><img src='/_ftres/ui/user.gif'/><img src='/_ftres/ui/del.png'/></td></tr>";
    //s+="</table>";
    s += '</div>';
    s += "</div>";
    $(s).appendTo($(document.body));
    StyleInit(O(id));
    $(O(id)).find("._button").each(function () {
        if ($(this).find("span:eq(1)").find("span")[0] != null) {
            $(this).find("span:eq(1)").removeAttr("class");
            $(this).find("span:eq(0)").remove();
        }
    });
    $(O(id)).dialog({
        modal: true,
        resizable: false,
        width: 700,
        show: 200,
        title: "数据导出选项"
    });
    dl_export_savefiles(partid);
}
function dl_export(partid, str) {
    document.getElementById("dlf" + partid).action = "/_ftpub/ftformop.aspx";
    document.getElementById("dlf" + partid).target = "dhf" + partid;
    document.getElementById("dlf" + partid).optype.value = 10;
    document.getElementById("dlf" + partid).exportstr.value = str;
    document.getElementById("dlf" + partid).submit();
    return true;
}
function dl_SelectAll(checkobj, c1, c2) {
    var loop = 1;
	var cked=dl_checked(checkobj);
    $(checkobj).parent().parent().parent().find("tr:gt(0)").each(
    function () {
        if (c2.indexOf(',') > 0) {
            if (loop % 2 == 1) $(this).css("background", cked ? c1 : c2.split(',')[0]);
            else $(this).css("background", cked ? c1 : c2.split(',')[1]);
        }
        else {
            $(this).css("background", cked ? c1 : c2);
        }
        loop++;
    }
    );
    $(checkobj).parent().parent().parent().find("div.layui-form-checkbox").each(function () {
		if($(this).hasClass('listckbox')){
        if(cked&&!dl_checked(this))$(this).addClass('layui-form-checked');
		else if(!cked&&dl_checked(this))$(this).removeClass('layui-form-checked');}
    });
}
function dl_checked(checkobj)
{
	return $(checkobj).hasClass('layui-form-checked');
}
function dl_tr_bg(checkobj, c1, c2) {
    if (dl_checked(checkobj)) $(checkobj).parent().parent().css("background", c1);
    else $(checkobj).parent().parent().css("background", c2);
}
function dl_gotopage(partid) {
    if (eleCheck("t_" + partid, "int") && parseInt($("#t_" + partid + "").val()) > 0) {
        eval("load_" + partid + "(" + (parseInt($("#t_" + partid + "").val())) + ")");
    }
}
function dl_coldefine(partid, coldefine_default) {
    var id = "dialog_coldefine";
    var s = null;
    var cookiename = coldefine_cookiename(location.href, partid);
    var curdefine = $.cookie(cookiename);
    var curdefine_width = $.cookie(cookiename + "_wd");
    var oo = coldefine_showarray(coldefine_default, curdefine);
    if ($("#" + id)[0] != null) $("#" + id).remove();
    s = "<div id=\"" + id + "\" style=\"display:none\">";
    s += '<div style="border:1px solid #ddd;float:left;margin-right: 10px;margin-bottom:5px; background: #eee; padding: 5px; width: 241px;color:#999999">未选列</div>';
    s += '<div style="border:1px solid #d3d3d3;float:left;margin-right: 10px;margin-bottom:5px; background: #dfdfec; padding: 5px; width: 241px;color:#999999">已选列</div>';
    s += '<div style="float:left;background: #eee; padding: 5px; width: 150px;color:#999999;">总体宽度&nbsp;<input type=text style="width:80px;height:13px;font-size:10px;color:#888888" class="_input tip" id="coldefine_wd" title="定义列表的总体宽度，例如1600px,1800,100% <br><br> 注：总体宽度恢复为默认值后需刷新页面生效" value="' + (curdefine_width == null ? '' : curdefine_width) + '"/></div>';
    s += '<br style="clear: both;" />';
    s += '<ul id="coldefine_hidden">';
    for (var o in oo[2]) {
        s += '<li class="ui-state-default"><span style="display:block;width:120px;float:left">' + coldefine_namefilext(o) + '</span><div style="width:90px;float:right;margin-top:-1px;"><input type=text  style="width:40px;height:13px;font-size:10px;color:#888888" class="_input tip" title="定义该列的宽度，例如auto,100,120px,12%" value="' + oo[2][o][2] + '"/>&nbsp;<select style="width:40px;height:18px;font-size:10px;color:#888888" class="_select tip" title="定义该列内容的对齐方式"><option value=""></option><option value="left"' + (oo[2][o][3] == 'left' ? ' selected' : '') + '>左</option><option value="center"' + (oo[2][o][3] == 'center' ? ' selected' : '') + '>中</option><option value="right"' + (oo[2][o][3] == 'right' ? ' selected' : '') + '>右</option></select><br style="clear: both;" /></li>';
    }
    for (var o in oo[0]) {
        s += '<li class="ui-state-default"><span style="display:block;width:120px;float:left">' + coldefine_namefilext(o) + '</span><div style="width:90px;float:right;margin-top:-1px;"><input type=text  style="width:40px;height:13px;font-size:10px;color:#888888" class="_input tip" title="定义该列的宽度，例如100,120px,12%" value=""/>&nbsp;<select style="width:40px;height:18px;font-size:10px;color:#888888" class="_select tip" title="定义该列内容的对齐方式"><option value=""></option><option value="left">左</option><option value="center">中</option><option value="right">右</option></select><br style="clear: both;" /></li>';
    }
    s += '</ul>';

    s += '<ul id="coldefine_show">';
    for (var o in oo[3]) {
        s += '<li class="ui-state-active"><span style="display:block;width:120px;float:left">' + coldefine_namefilext(o) + '</span><div style="width:90px;float:right;margin-top:-1px;"><input type=text  style="width:40px;height:13px;font-size:10px;color:#888888" class="_input tip" title="定义该列的宽度，例如100,120px,12%" value="' + oo[3][o][2] + '"/>&nbsp;<select style="width:40px;height:18px;font-size:10px;color:#888888" class="_select tip" title="定义该列内容的对齐方式"><option value=""></option><option value="left"' + (oo[3][o][3] == 'left' ? ' selected' : '') + '>左</option><option value="center"' + (oo[3][o][3] == 'center' ? ' selected' : '') + '>中</option><option value="right"' + (oo[3][o][3] == 'right' ? ' selected' : '') + '>右</option></select><br style="clear: both;" /></li>';
    }
    for (var o in oo[1]) {
        s += '<li class="ui-state-active"><span style="display:block;width:120px;float:left">' + coldefine_namefilext(o) + '</span><div style="width:90px;float:right;margin-top:-1px;"><input type=text  style="width:40px;height:13px;font-size:10px;color:#888888" class="_input tip" title="定义该列的宽度，例如100,120px,12%" value=""/>&nbsp;<select style="width:40px;height:18px;font-size:10px;color:#888888" class="_select tip" title="定义该列内容的对齐方式"><option value=""></option><option value="left">左</option><option value="center">中</option><option value="right">右</option></select><br style="clear: both;" /></li>';
    }
    s += '</ul>';
    s += '<div style="float:left;background: #eee; padding: 5px; width: 150px;"><button class="_button" tag="ui-icon-arrowreturn-1-w" onclick="$.cookie(\'' + cookiename + '_wd\',null);$.cookie(\'' + cookiename + '\',null);load_' + partid + '(1);closeDialog(\'' + id + '\');">默认</button>&nbsp;<button class="_button" autofocus tag="ui-icon-check" onclick="$.cookie(\'' + cookiename + '_wd\',eleVal(\'coldefine_wd\'));$.cookie(\'' + cookiename + '\',coldefine_create());load_' + partid + '(1);closeDialog(\'' + id + '\');">确定</button></div>';
    s += '<div style="float:left;background: #eee; padding: 5px; width: 150px;margin-top:2px;color:#cccccc">拖拽列以进行选择和排序</div>';
    s += '<br style="clear: both;" />';
    s += "</div>";
    $(s).appendTo($(document.body));
    $("ul#coldefine_hidden,ul#coldefine_show").sortable({
        connectWith: "ul", placeholder: "dialog_coldefine_holder",
        stop: function (event, ui) {
            if (ui.item.parent()[0].id == 'coldefine_hidden') ui.item[0].className = 'ui-state-default';
            else ui.item[0].className = 'ui-state-active';
        }
    });

    $("ul#coldefine_hidden,ul#coldefine_show").disableSelection();
    $("ul#coldefine_hidden,ul#coldefine_show").each(function () { this.style.cssText = "list-style-type: none; margin: 0; padding: 0; float: left; margin-right: 10px; background: #eee; padding: 5px; width: 243px;"; });
    $("ul#coldefine_hidden li,ul#coldefine_show li").each(function () { this.style.cssText = "margin: 5px; padding: 5px; min-height:16px;font-size: 100%; width: 220px;word-break:break-all;word-wrap:break-word"; });
    $("#" + id + " .ui-state-default,#" + id + " .ui-state-active").css({ cursor: 'move' });
    StyleInit(O(id));
    $(O(id)).find("._button").each(function () {
        if ($(this).find("span:eq(1)").find("span")[0] != null) {
            $(this).find("span:eq(1)").removeAttr("class");
            $(this).find("span:eq(0)").remove();
        }
    });
    $(O(id)).dialog({
        modal: true,
        resizable: false,
        width: 720,
        show: 200,
        title: "列自定义"
    });
}
function coldefine_cookiename(href, partid) {
    if (href.indexOf('?') >= 0) href = href.substring(0, href.indexOf('?'));
    href = href.replace("http:", "").replace(/\//g, "").replace(/\./g, "");
    return href + "_" + partid;
}
function coldefine_namefil(name) {
    if (eleCheck(null, 'decimal', name)) return '((' + name; else return name;
}
function coldefine_namefilext(name) {
    if (name.indexOf('((') == 0) return name.substring(2); else return name;
}
function coldefine_showarray(oridefine, curdefine) {
    var a0 = new Object();
    var d0 = oridefine.split("|||");
    for (var i = 0; i < d0.length; i++) {
        var d1 = d0[i].split("##");
        a0[coldefine_namefil(d1[0])] = new Array(i + 1, parseInt(d1[1]), d1[2], d1[3]);
    }
    var a1 = new Object();
    if (curdefine != null && curdefine != "") {
        var d0 = curdefine.split("|||");
        for (var i = 0; i < d0.length; i++) {
            var d1 = d0[i].split("##");
            a1[coldefine_namefil(d1[0])] = new Array(i + 1, parseInt(d1[1]), d1[2], d1[3]);
        }
    }
    var o0 = new Object(); var o1 = new Object(); var o2 = new Object(); var o3 = new Object();
    for (var c0 in a0) {
        if (a1[c0] == null) {
            if (a0[c0][1] == 1) o0[c0] = a0[c0];
            else o1[c0] = a0[c0];
        }
    }
    for (var c1 in a1) {
        if (a0[c1] != null) {
            if (a1[c1][1] == 1) o2[c1] = a1[c1];
            else o3[c1] = a1[c1];
        }
    }
    return new Array(o0, o1, o2, o3);
}
function coldefine_create() {
    var define = "";
    $("#coldefine_hidden>li.ui-state-default").each(function () {
        define += "|||";
        define += $(this).find("span:eq(0)").text() + "##1##" + $(this).find("input")[0].value + "##" + $(this).find("select")[0].value;
    });
    $("#coldefine_show>li.ui-state-active").each(function () {
        define += "|||";
        define += $(this).find("span:eq(0)").text() + "##0##" + $(this).find("input")[0].value + "##" + $(this).find("select")[0].value;
    });
    if (define == "") define = null;
    else define = define.substring(3);
    return define;
}
function coldefine_showext(oridefineext, curdefine) {
    var a0 = new Object();
    var d0 = oridefineext.split("|||");
    for (var i = 0; i < d0.length; i++) {
        var d1 = d0[i].split("##");
        var title = d1[0];
        if (a0[coldefine_namefil(title)] != null) title += "_sametitle_" + Math.random();
        a0[coldefine_namefil(title)] = new Array(parseInt(d1[1]), d1[2], d1[3], parseInt(d1[4]), d1[5]);
    }
    var a1 = new Object();
    if (curdefine != null && curdefine != "") {
        var d0 = curdefine.split("|||");
        for (var i = 0; i < d0.length; i++) {
            var d1 = d0[i].split("##");
            a1[coldefine_namefil(d1[0])] = new Array(parseInt(d1[1]), d1[2], d1[3]);
        }
    }
    var oo = new Array();
    for (var c1 in a1) {
        if (a0[c1] != null) {
            if (a1[c1][0] == 0 && a0[c1][3] == 1) { oo[oo.length] = new Array(coldefine_namefilext(c1), a1[c1][1] == '' ? a0[c1][1] : a1[c1][1], a1[c1][2] == '' ? a0[c1][2] : a1[c1][2], a0[c1][4]); }
        }
    }
    for (var c0 in a0) {
        if (a1[c0] == null) {
            if (a0[c0][0] == 0 && a0[c0][3] == 1) { oo[oo.length] = new Array(c0.indexOf('_sametitle_') >= 0 ? c0.substring(0, c0.indexOf('_sametitle_')) : coldefine_namefilext(c0), a0[c0][1], a0[c0][2], a0[c0][4]); }
        }
    }
    return oo;
}
var lineMove = false;
var currTh = null;
$(document).ready(function () {
    $("body").append("<div id=\"line\" style=\"width:1px;height:200px;border-left:1px solid #AAAAAA; position:absolute;display:none;\" ></div> ");
    $("body").bind("mousemove", function (event) {
        if (lineMove == true) {
            $("#line").css({ "left": event.clientX }).show();
        }
    });



    $("body").bind("mouseup", function (event) {
        if (lineMove == true) {
            $("#line").hide();
            lineMove = false;
            var pos = currTh.offset();
            var index = currTh.prevAll().length;
            currTh.width(event.clientX - pos.left);
            currTh.parent().parent().find("tr").each(function () {
                $(this).children().eq(index).width(event.clientX - pos.left);
            });
        }
    });

});
function fttableslip() {
    $("th[name=listth]").bind("mousemove", function (event) {
        var th = $(this);
        if (th.prevAll().length <= 1 || th.nextAll().length < 1) {
            return;
        }
        var left = th.offset().left;
        if (lineMove || (event.clientX - left < 4 || (th.width() - (event.clientX - left)) < 4)) {
            th.css({ 'cursor': 'url(/lib/list.res/splith.cur)' });
            th.css({ 'cursor': 'W-resize' });
        }
        else {
            th.css({ 'cursor': 'default' });
        } window.getSelection ? window.getSelection().removeAllRanges() : document.selection.empty();
    });

    $("th[name=listth]").bind("mousedown", function (event) {
        var th = $(this);
        if (th.prevAll().length <= 1 | th.nextAll().length < 1) {
            return;
        }
        var pos = th.offset();
        if (event.clientX - pos.left < 4 || (th.width() - (event.clientX - pos.left)) < 4) {
            var height = th.parent().parent().height();
            var top = pos.top;
            $("#line").css({ "height": height, "top": top, "left": event.clientX, "display": "" });
            lineMove = true;
            if (event.clientX - pos.left < th.width() / 2) {
                currTh = th.prev();
            }
            else {
                currTh = th;
            }
        }
    });
    $("th[name=listth]").bind("mouseup", function (event) {
        if (lineMove == true) {
            $("#line").hide();
            lineMove = false;
            var pos = currTh.offset();
            var index = currTh.prevAll().length;
            currTh.width(event.clientX - pos.left);
            currTh.parent().parent().find("tr").each(function () {
                $(this).children().eq(index).width(event.clientX - pos.left);
            }); window.getSelection ? window.getSelection().removeAllRanges() : document.selection.empty();
        }
    });
}
$(document).ready(function () {
    $("._treeimg").each(function () {
        var id = this.id;
        var haveson = false;
        $("._treeimg").each(function () {
            var id1 = this.id;
            if (id != id1 && id1.indexOf(id) == 0) {
                haveson = true;
                return false;
            }
        });
        if (!haveson) {
            $(this).hide();
        }
    });
});
function doTree(obj) {
    var id = obj.id;
    var ishide = (obj.src.indexOf("minus.gif") >= 0);
    $(obj).parent().parent().parent().find("._treeimg").each(function () {
        if (this.id != id) {
            if (ishide) {
                if (this.id.indexOf(id) == 0) {
                    $(this).parent().parent().fadeOut(400);
                }
            }
            else {
                if (this.id.indexOf(id) == 0) {
                    var id1 = this.id.substring(id.length);
                    if (id1.lastIndexOf("_") == id1.indexOf("_")) {
                        if (this.style.display != "none") this.src = "/lib/list.res/plus.gif";
                        $(this).parent().parent().fadeIn(400);
                    }
                }
            }
        }
    });
    if (!ishide) obj.src = "/lib/list.res/minus.gif";
    else obj.src = "/lib/list.res/plus.gif";
}