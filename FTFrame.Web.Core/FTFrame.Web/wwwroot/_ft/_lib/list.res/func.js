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
    var objs = $(obj).parent().parent().parent().parent().parent().find("input[name=dlcheckradio]");
    objs.each(function () {
        if ($(this)[0].checked) {
            ids += "," + $(this)[0].value;
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
    var objs = $(obj).parent().parent().parent().parent().parent().find("input[name=dlcheckradio]");
    objs.each(function () {
        if ($(this)[0].checked) {
            ids += "," + $(this)[0].value;
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
function dl_refresh(ftpartid) {
    eval("list_adv_s_" + ftpartid + "='';");
    if ($("#scht_" + ftpartid)[0] != null) {
        $("#scht_" + ftpartid).val("");
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

function dl_export_savefiledel(partid,fileid)
{
	newDialog('loading','loading');
	$.post("/_ftpub/ftajax.aspx", { "fileid":fileid,"ajaxtype": "List_ExportSaveFiles_Del" },function (data, textStatus) {
		if(data=='OK'){closeDialog('loading');dl_export_savefiles(partid+'_part');}
		else _loading2fai(data);
	}, "text");
}
var dl_export_saveuserbind_tree=null;
function dl_export_saveuserbind(fileid)
{
	var id = "dialog_listexportbind";
	if ($("#" + id)[0] != null) $("#" + id).remove();
	var s='<DIV id='+id+'>';
	s+='<DIV align=right>';
	s+='<button class="_button tip" tag="ui-icon-check" onclick="dl_export_saveuserbind_do(\''+fileid+'\')">确定</button>';
	s+='</DIV>';
	s+='<UL id='+id+'tree class=ztree style="OVERFLOW: auto; WIDTH: 95%"></UL></DIV>';
	$(s).appendTo($(document.body));
    StyleInit(O(id));
	$(O(id)).dialog({
        modal: true,
        resizable: true,
        width: 500,
        show: 200,
        title: "分享给指定用户"
    });
	newDialog("info", "loading","正在载入...",false);
    $.post("/_ftpub/ftajax.aspx",{ "ajaxtype": "List_ExportSaveFiles_Tree","fileid":fileid},function (data, textStatus) {
closeDialog("info");
dl_export_saveuserbind_tree = $.fn.zTree.init($("#"+id+'tree'), dl_export_save_bind_setting, eval(data));
}, "text");
}
function dl_export_saveuserbind_do(fileid)
{
	newDialog('loading','loading');
	var s="";
	var objs=dl_export_saveuserbind_tree.getCheckedNodes(true);
	for(var i=0;i<objs.length;i++)
	{
		if(objs[i].id.indexOf('2_')==0)s+=','+objs[i].id.substring(2);
	}
	if(s!='')s=s.substring(1);
	$.post("/_ftpub/ftajax.aspx", { "fileid":fileid,"userids":s,"ajaxtype": "List_ExportSaveFiles_Bind" },function (data, textStatus) {
		if(data=='OK'){closeDialog('loading');closeDialog('dialog_listexportbind');}
		else _loading2fai(data);
	}, "text");
}
function dl_export_savestart(partid)
{
	var id = "dialog_listexportfilecap";
    if ($("#" + id)[0] != null) $("#" + id).remove();
    var s = "<div id=\"" + id + "\" style=\"display:none;text-align:left;padding-top:30px\">";
	s+="显示的文件名称：<input type=text class=_input style='width:250px;margin-right:4px'/>";
	s+='<button class="_button tip" tag="ui-icon-clock" onclick="newDialog(\'loading\',\'loading\');load_'+partid+'(1,null,null,true,1,$(this).prev().val())">开始生成</button>';
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
function dl_export_saveok(partid)
{
	if ($("#dialog_listexportfilecap")[0] != null)closeDialog('dialog_listexportfilecap');
	_loading2suc("dl_export_savefiles('"+partid+"')");
}
function dl_export_savefiles(partid)
{
	$('#listexportspan'+partid).html("<div align='center' style='padding:20px 0px 20px 0px'><img border=0 src='/_ftres/progress.gif'/></div>");
	$.post("/_ftpub/ftajax.aspx", { "partid":partid,"ajaxtype": "ListExportSave" },function (data, textStatus) {
		$('#listexportspan'+partid).html(data);
	}, "text");
}
function dl_export_all(partid) {
    var id = "dialog_listexport";
    if ($("#" + id)[0] != null) $("#" + id).remove();
    var s = "<div id=\"" + id + "\" style=\"display:none;text-align:right\">";
	s+="<input type=hidden autofocus >";
	s+='<button class="_button tip" style="margin-right:10px" tag="ui-icon-extlink" title="数据量较少且网络条件较好时，可选择直接导出" onclick="load_'+partid+'(1,null,null,true)">直接导出</button>';
    s+='<button class="_button tip" tag="ui-icon-disk" title="在服务器生成数据文件，生成后可随时下载，生成的数据文件且可与指定用户分享<br>适用于导出大数据文件，或者相同的数据需多次导出时使用" onclick="dl_export_savestart(\''+partid+'\')">生成再下载</button>';
    s+='<div style="text-align:left" id="listexportspan'+partid+'" class="ftselect">';
	//s+="<table width=100% style='margin-top:6px' cellspacing=1 cellpadding=1>";
	//s+="<tr style='color:#888888'><td>文件</td><td>行数</td><td>时间</td><td>生成人</td><td>&nbsp;</td></tr>";
	//s+="<tr><td><a href=#>文件</a></td><td>数据行数</td><td>生成时间</td><td>生成人</td><td><img src='/_ftres/ui/user.gif'/><img src='/_ftres/ui/del.png'/></td></tr>";
	//s+="</table>";
	s+='</div>';
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
    $(checkobj).parent().parent().parent().find("tr:gt(0)").css("background", checkobj.checked ? c1 : c2);
    $(checkobj).parent().parent().parent().find("input[name=dlcheckradio]").each(function () {
        $(this)[0].checked = checkobj.checked;
    });
}
function dl_tr_bg(checkobj, c1, c2) {
    if (checkobj.checked) $(checkobj).parent().parent().css("background", c1);
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