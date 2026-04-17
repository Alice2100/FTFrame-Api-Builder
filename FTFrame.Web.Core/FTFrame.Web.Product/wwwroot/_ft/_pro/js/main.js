$(function () {
    $.datepicker.setDefaults({
        changeMonth: false,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true,
        dateFormat: "yy-mm-dd",
        showButtonPanel: false,
        monthNames: ['一月', '二月', '三月', '四月', '五月', '六月',
                '七月', '八月', '九月', '十月', '十一月', '十二月'],
        monthNamesShort: ['一', '二', '三', '四', '五', '六',
                '七', '八', '九', '十', '十一', '十二'],
        dayNames: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
        dayNamesShort: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
        dayNamesMin: ['日', '一', '二', '三', '四', '五', '六'],
        firstDay: 1,
        prevText: '&#x3c;上月', prevStatus: '',
        prevJumpText: '&#x3c;&#x3c;', prevJumpStatus: '',
        nextText: '下月&#x3e;', nextStatus: '',
        nextJumpText: '&#x3e;&#x3e;', nextJumpStatus: '',
        currentText: '今天', currentStatus: '',
        todayText: '今天', todayStatus: '',
        clearText: '-', clearStatus: '',
        closeText: '关闭', closeStatus: '',
        yearStatus: '', monthStatus: '',
        weekText: '周', weekStatus: '',
        dayStatus: 'DD d MM',
        defaultStatus: '',
        isRTL: false
    });
});
function newdialog(id, type, title,ismodel) {
    if (title == null) title = "";
    if ($("#" + id)[0] != null) $("#" + id).remove();
    var s = null;
    if (type == null) {
        s = "<div id=\"" + id + "\" title=\"" + title + "\"></div>";
    }
    else if (type == "loading") {
        s = "<div id=\"" + id + "\" title=\"" + title + "\" style=\"padding-top:40px;text-align:center\"><img src=\"../css/img/all/progress.gif\"/></div>";
    }
    $(s).appendTo($(document.body));

    if (type == "loading") {
        $("#" + id).dialog({
            modal: (ismodel == null || ismodel),
            resizable: false,
            show: 200,
            title:(title==null?"正在提交中...":title),
            beforeClose: function (event, ui) { alert("正在提交中不能关闭"); return false; }
        });
        setTimeout("if($('#" + id + "').dialog('isOpen'))$('#" + id + "').dialog({beforeClose: function (event, ui) { return true; }});", 10000);
    }
    return $("#" + id)[0];
}
function info(val,type,el,evaljs) {
    var ele = (el == null ? newdialog("_custominfo", null) : el);
    $(ele).css({"padding-top":"20px","text-align":"left"});
    $(ele).html("<img src='../css/img/ui/" + (type == 1 ? "alert1.gif" : (type == 2 ? "alert0.gif" : (type == 3 ? "error.gif" : (type == 4 ? "success.gif" : "failed.png")))) + "'>&nbsp;" + val);
    $(ele).dialog({
        modal: true,
        resizable: false,
        show: 200,
        title: (type == 1 ? "信息" : (type == 2 ? "警告" : (type == 3 ? "错误" : (type == 4 ? "操作成功" : "操作失败")))),
        buttons: {
            "确定": function () {
                $(this).dialog("close");
            }
        },
        beforeClose: function (event, ui) {return true;
        },
        close: function (event, ui) {
            if (evaljs != null) eval(evaljs);
        }
    });
}
function closeDialog(eid) {
    if ($('#' + eid).dialog('isOpen')) {
        $('#' + eid).dialog({ beforeClose: function (event, ui) { return true; } });
        $('#' + eid).dialog("close");
    }
}
function RowModel(colname, inputid, tip, inputattr,noempty,topbottom,cushtml) {
    var spacestyle = "";
    if (colname.length > 4) {
        spacestyle = "letter-spacing:0px;";
    }
    if (colname.length == 4) {
        spacestyle = "letter-spacing:2px;";
    }
    else if (colname.length == 3) {
        spacestyle = "letter-spacing:10px;";
    }
    else if (colname.length == 2) {
        spacestyle = "letter-spacing:33px;";
    }
    var radius = "";
    if (topbottom != null) {
        if (topbottom == 0) radius = " topradius";
        else if (topbottom == 1) radius = " bottomradius";
    }
    return '<div class="txt-fld' + radius + '"><label class="column" style="font-size:100%;' + spacestyle + '">' + colname + '</label>' + (cushtml == null ? ('<input class="nomal_input" type="text" id="' + inputid + '"' + (inputattr == null ? '' : inputattr) + '/>') : cushtml) +(noempty ? '<font color="red" style="color:red">*</font>' : '') + '<span class="info" style="font-size:80%;">' + (tip == null ? '' : tip) + '</span></div>';
}
function ButtonModel(bts,notbottom) {
  var s="";
 for(var i=0;i<bts.length;i++) {
     s += '<button type="button"' + (bts[i][2] == null ? '' : ' id="' + bts[i][2] + '"') + ' class="nomal_button' + (i > 0 ? ' leftmar' : '') + '"  onclick="' + bts[i][1] + '">' + bts[i][0] + '</button>';
 }
 return '<div class="btn-fld' + ((notbottom==null||!notbottom)?' bottomradius':'') + '">' + s + '</div>';
}
function eleVal(eleid) {
    var obj = document.getElementById(eleid);
    var val = "";
    if ((obj.tagName == "INPUT" && obj.type == "checkbox") || (obj.tagName == "INPUT" && obj.type == "radio")) {
        var objs = document.getElementsByName(eleid);
        for (var i = 0; i < objs.length; i++) {
            if (objs[i].checked) val += "," + objs[i].value;
        }
        if (val != "") val = val.substring(1);
    }
    else if (obj.tagName == "SELECT" && obj.multiple) {
        for (var i = 0; i < obj.options.length; i++) {
            if (obj.options[i].selected) val += "," + obj.options[i].value;
        }
        if (val != "") val = val.substring(1);
    }
    else val = obj.value;
    return val;
}
function eleCheck(eles) {
    for (var ei = 0; ei < eles.length; ei++) {
        if (eles[ei].length > 2) {
            var eval=$.trim($("#"+eles[ei][0]).val());
            $("#"+eles[ei][0]).val(eval);
            if (eles[ei][1] == "noempty") {
                if (eval == "") {
                    info(eles[ei][2], 2, null, "$('#" + eles[ei][0] + "')[0].focus();");
                    return false;
                }
            }
            else if (eles[ei][1] == "int") {
                if (eval == "" || !(/^(\+|-)?\d+$/.test(eval))) {
                    info(eles[ei][2], 2, null, "$('#" + eles[ei][0] + "')[0].focus();");
                    return false;
                }
            }
            else if (eles[ei][1] == "decimal") {
                if (eval == "" || isNaN(eval)) {
                    info(eles[ei][2], 2, null, "$('#" + eles[ei][0] + "')[0].focus();");
                    return false;
                }
            }
        }
    }
    return true;
}
function listLoading(eleid) {
    $("#" + eleid).html("<div align=center style='padding-top:100px'><img src='/css/img/all/progress.gif'/></div>");
}
function ft_SelectAll(checkobj, c1, c2) {
    $(checkobj).parent().parent().parent().find("tr:gt(0)").css("background", checkobj.checked ? c1 : c2);
    $(checkobj).parent().parent().parent().find("tr:last").css("background", c2);
    $(checkobj).parent().parent().parent().find("input[name=ft_check]").each(function () {
        $(this)[0].checked = checkobj.checked;
    });
}
function ft_tr_bg(checkobj, c1, c2) {
    if (checkobj.checked) $(checkobj).parent().parent().css("background", c1);
    else $(checkobj).parent().parent().css("background", c2);
}
function ft_tt_over(obj, c1) {
    if (obj.children[0].children[0] != null && obj.children[0].children[0].tagName == "INPUT" && obj.children[0].children[0].checked) {
    } else
        $(obj).css("background-color", c1);
}
function ft_tt_out(obj, c2) {
    if (obj.children[0].children[0] != null && obj.children[0].children[0].tagName == "INPUT" && obj.children[0].children[0].checked) {
    } else
        $(obj).css("background-color", c2);
}
function ft_SelectOne(obj) {
    j = 0;
    var ids = "";
    var objs = $(obj).find("input[name=ft_check]");
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
        alert("你最多只能选择一个项");
        return null;
    }
    return ids.substring(1);
}

function ft_SelectMore(obj, showmsg) {
    j = 0;
    var ids = "";
    var objs = $(obj).find("input[name=ft_check]");
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