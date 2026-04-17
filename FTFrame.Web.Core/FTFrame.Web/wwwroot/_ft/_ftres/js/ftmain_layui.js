$(function () {
	/*if(/msie 7|msie 6|msie 5/.test(navigator.userAgent.toLowerCase()) && !/msie 8|msie 9|msie 1/.test(navigator.userAgent.toLowerCase()))
	{
		alert("请使用IE8或以上浏览器，或其他标准浏览器访问");
	}*/
});
function StyleInit(elet) {
}
function InitSel(obj)
{
	return;
	if(typeof(obj)!='object')obj=O(obj);
	var oncg=$(obj).attr("onchange");
	$(obj).selectmenu({
       change: function( event, data ) {
		   if(oncg!=null&&oncg!='')eval(oncg.replace('this.value','\"'+data.item.value+'\"'));
               },
		open: function( event, ui ) {
				   var zix=111;
			$(".ui-dialog").each(function()
				   {
				     if($(this).zIndex()>zix)zix=$(this).zIndex();
				   });
				   console.log("max zindex "+zix);
				   $('.ui-selectmenu-open').zIndex(zix+4);
		}
     });
}
function StyleRow(obj)
{
	
}
function newDialog(id, type, title,ismodel)
{
	if (title == null) title = "";
	if ($("#" + id)[0] != null) $("#" + id).remove();
	var s = null;
	if (type == null) {
        layer.alert('"'+title+'"', {
			skin: 'layui-layer-molv' //样式类名
		});
	}
	else if (type == "loading") {
		var text=((title==null||title=="")?"正在处理中...":title);
		var load = layer.msg('"'+text+'"', {
            icon:16,
            shade:[0.1, '#fff'],
			time:false,  //false取消自动关闭
			cancel:function(){
				alert("请稍后关闭");
				return false;
			}
        });
		layuiDialogIndex[id]=load;
       setTimeout(() => {
		layer.close(load);
	   }, 10000); 
	}
	return $("#" + id)[0];
}
/*function newDialog(id, type, title,ismodel) {
    if (title == null) title = "";
    if ($("#" + id)[0] != null) $("#" + id).remove();
    var s = null;
    if (type == null) {
        s = "<div id=\"" + id + "\" title=\"" + title + "\"></div>";
    }
    else if (type == "loading") {
        s = "<div id=\"" + id + "\" title=\"" + title + "\" style=\"padding-top:40px;text-align:center\"><img src=\"/_ftres/progress.gif\"/></div>";
    }
    $(s).appendTo($(document.body));

    if (type == "loading") {
        $("#" + id).dialog({
            modal: (ismodel == null || ismodel),
            resizable: false,
            show: 200,
            title:((title==null||title=="")?"正在处理中...":title),
            beforeClose: function (event, ui) { alert("请稍后再关闭"); return false; }
        });
        setTimeout("if($('#" + id + "').dialog('isOpen'))$('#" + id + "').dialog({beforeClose: function (event, ui) { return true; }});", 10000);
    }
    return $("#" + id)[0];
}*/
function eleForm(id)
{
	var ele=O(id);
	//if($(ele).find("form")[0]==null)$(ele).html("<form style='margin:0px;padding:0px'>"+$(ele).html()+"</form>");
	//if($(ele).find("form")[0]==null && $(ele).parent()[0].tagName!='FORM')$(ele).wrapAll("<form style='margin:0px;padding:0px'></form>");
	if($(ele).find("form")[0]==null && $(ele).parent()[0].tagName!='FORM')$(ele).html("<form style='margin:0px;padding:0px'>"+$(ele).html()+"</form>");
}
var layuiDialogIndex=[];
function showDialog(id,title,width,isaddform,hight,idadd,ismodal,isresizeable)
{
	var ele=O(id);
	if(isaddform!=null&&isaddform)eleForm(id);
	layui.use('layer',function(){
		var form = layui.form; 
            form.render();
		var layer = layui.layer;
		//var laydate =layui.laydate;
		var ii=layer.open({
			 type: 1 //此处以iframe举例
			  ,title: title
			  ,area: [width,hight]
			  ,shade: 0
			  ,maxmin: true
			  ,content: $("#"+id+"")
			  /*,success:function(){
				laydate.render({
					elem:'#add_date'
				  });
			  }*/
			});  
		layuiDialogIndex[id]=ii;
		}); 

		layui.use('laydate', function () {
            var laydate = layui.laydate; 
			$(ele).find(".layui-date").each(function(){
	        	laydate.render({
	              elem:'#'+this.id
	            });
	        });
        });
}
/*function showDialog(id,title,width,isaddform,idadd,ismodal,isresizeable)
{
	var ele=O(id);
	if(isaddform!=null&&isaddform)eleForm(id);
if(idadd!=null&&idadd!="")
{
	$(ele).find("*[id]").each(function(){
	this.id=idadd+"_"+this.id;
	});
}

StyleInit(ele);
$(ele).find("._button").each(function(){
	if($(this).find("span:eq(1)").find("span")[0]!=null)
	{
		$(this).find("span:eq(1)").removeAttr("class");
		$(this).find("span:eq(0)").remove();
	}
});
$(ele).dialog({
            modal:(ismodal!=null&&ismodal),
            resizable: (isresizeable==null||!isresizeable),
			width:width,
            show: 200,
			title:title,
				open: function (event, ui) {
                    var $dialog = $(this);
                    $dialog.parent().find('.ui-dialog-title').css('width','50%');
					$dialog.parent().find('.ui-dialog-titlebar-min,.ui-dialog-titlebar-max').remove();
                    var atext = $dialog.parent().find(".ui-dialog-titlebar-close").before('<a href="javascript:void(0)" class="ui-dialog-titlebar-min ui-corner-all" role="button"><span class="ui-icon ui-icon-minus">minus</span></a><a href="javascript:void(0)" class="ui-dialog-titlebar-max ui-corner-all" role="button"><span class="ui-icon ui-icon-plus">plus</span></a><span class="_lastwidth" style="display:none"></span>');
                    
					$dialog.parent().find('.ui-dialog-titlebar').dblclick(function(){
						 if($(this).find(".ui-icon-plus")[0]!=null)$(this).find(".ui-icon-plus").trigger("click");
						 else if($(this).find(".ui-icon-newwin")[0]!=null)$(this).find(".ui-icon-newwin").trigger("click");
					});
					$dialog.parent().find(".ui-dialog-titlebar .ui-icon").click(function () {
                        var spantext = $(this).text();
                        if (spantext == "plus") {
						if(dialogbusing)return false;dialogbusing=true;setTimeout("dialogbusing=false",500);
							if($dialog.dialog( "option", "height" )!=null && $dialog.dialog( "option", "height" )!=20)
							{
								$dialog.parent().find('._lastwidth').html($dialog.dialog( "option", "width" )+";"+$dialog.dialog( "option", "height" ));
							}
							$dialog.dialog({
                                    position: { my: "left top", at: "left top", of: window },
                                    width: $(window).width()-12,
                                    height: 'auto'
                                });
							/*
                            if (window.screen) {              
                                var myw = screen.availWidth;   
                                var myh = screen.availHeight; 
                                $dialog.dialog({
                                    position: ['left', 'top'],
                                    width: myw * 1-28,
                                    height: 'auto'
                                });
								

                            } else {
                                $dialog.dialog({
                                    position: 'center',
                                    width: 640,
                                    height: 480
                                });
                            }
							$(this).removeClass("ui-icon-plus").addClass("ui-icon-newwin").html("newwin");
							$(this.parentElement).prev().find("span").removeClass("ui-icon-newwin").addClass("ui-icon-minus").html("minus");
                        } else if (spantext == "minus") {
                            $dialog.dialog({
                                position: 'top',
                                width: 590,
                                height:20
                            });
							$(this).removeClass("ui-icon-minus").addClass("ui-icon-newwin").html("newwin");
							$(this.parentElement).next().find("span").removeClass("ui-icon-newwin").addClass("ui-icon-plus").html("plus");
                        }else if (spantext == "newwin") {
						if(dialogbusing)return false;dialogbusing=true;setTimeout("dialogbusing=false",500);
							if($(this).parent().hasClass("ui-dialog-titlebar-max"))
							{
								$(this).removeClass("ui-icon-newwin").addClass("ui-icon-plus").html("plus");
							}
							else
							{
								$(this).removeClass("ui-icon-newwin").addClass("ui-icon-minus").html("minus");
							}
							var _pospara=$dialog.parent().find('._lastwidth').html().split(";");
                            $dialog.dialog({
                                    position: { my: "center", at: "center", of: window },
                                    width: _pospara[0],
                                    height: _pospara[1]
                                });
                        }else {
                           
                        }
                    });}
        });
	return ele;
}*/
var dialogbusing=false;
function _loading2suc(evaljs,val)
{
	val=(val==null)?"":(":"+val);
	info(val,4,$("#loading")[0],evaljs);
}
function _loading2fai(evaljs,val)
{
	val=(val==null)?"":(":"+val);
	info(val,5,$("#loading")[0],evaljs);
}
function info(val,type,el,evaljs)
{
   var title=(type == 1 ? "信息" : (type == 2 ? "警告" : (type == 3 ? "错误" : (type == 4 ? "操作成功" : "操作失败"))));
   var icon=(type == 1 ? 3 : (type == 2 ? 0 : (type == 3 ? 2 : (type == 4 ? 1 : 5))));
   console.log(type);
   console.log(title);
   if(val!=null&&val!='')title+='&nbsp;' + val;
  var tealert= layer.alert('"'+title+'"', {
	skin: 'layui-layer-molv' //样式类名
	, closeBtn: 0
	, icon: icon
	,yes:function(index,layero){
		layer.close(index); 
		if (evaljs != null) eval(evaljs);
	}
},function () {
	//layer.close(layer.index);
});
}
/*function info(val,type,el,evaljs) {
    var ele = (el == null ? newDialog("_custominfo", null) : el);
    $(ele).css({"padding-top":"20px","text-align":"left"});
    $(ele).html("<img src='/_ftres/ui/" + (type == 1 ? "alert1.gif" : (type == 2 ? "alert0.gif" : (type == 3 ? "error.gif" : (type == 4 ? "success.gif" : "failed.png")))) + "'>&nbsp;" + val);
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
}*/
function closeDialog(id){
	if(id==null)layer.closeAll();
	else layer.close(layuiDialogIndex[id]);
}
/*function closeDialog(eid) {
	try{
		if(typeof(eid)=='object')eid=$(eid).parents('.ftselect,.ftdiv')[0].id;
    if ($('#' + eid).dialog('isOpen')) {
        $('#' + eid).dialog({ beforeClose: function (event, ui) { return true; } });
        $('#' + eid).dialog("close");
    }
	}catch(ex){}
}*/
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
function eleVal(eleid,eleval,rowrate) {
	if(eleid==null||eleid=='')return;
	if(eleid.indexOf(',')>=0)
	{
		var _ids=eleid.split(',');
		for(var i=0;i<_ids.length;i++){eleVal(_ids[i],eleval,rowrate)}
		return false;
	}
if(eleid.indexOf('$')==0){eval(eleid.substring(1)+"=eleval");return;}
	var obj = O(eleid);
	if(obj==null){alert("对象为空，id:"+eleid);return false;};
	var objtag=obj.tagName.toUpperCase();
	var objtype=(obj.type==null?"":obj.type.toUpperCase());
	if(eleval==null)
	{
//layui
		var layuiForm=$(obj).closest('.layui-form');
		if(layuiForm[0]!=null)
		{
			var layFilter=layuiForm.attr('lay-filter');
			if(layFilter!=null&&layFilter!='')
			{
				var layObjVal=layui.form.val(layFilter)[eleid];
				if(layObjVal!=null)
				{ 
					return layObjVal;
				}
			}
		}
		var val = "";
		if ((objtag == "INPUT" && objtype == "CHECKBOX") || (objtag == "INPUT" && objtype == "RADIO")) {
			var objs = document.getElementsByName(obj.name);
			for (var i = 0; i < objs.length; i++) {
				if (objs[i].checked) val += "," + objs[i].value;
			}
			if (val != "") val = val.substring(1);
		}
		else if (objtag == "SELECT" && obj.multiple) {
			for (var i = 0; i < obj.options.length; i++) {
				if (obj.options[i].selected) val += "," + obj.options[i].value;
			}
			if (val != "") val = val.substring(1);
		}
		else if(objtag=="LABEL"||objtag=="SPAN"||objtag=="DIV")
		{
			val=$(obj).html();
		}
		else val = obj.value;
		return val;
	}
	else
	{
		eleval=eleval+"";
		if(rowrate==null||rowrate==0)
		{
			eleValBase(obj,objtag,objtype,eleval,eleid);
		}
		else
		{
			if(O(eleid+"_rowrate"+rowrate)==null)
			{
				if(rowrate==1)
				{
					ftformaddrow(getRowRateElement(O(eleid)));
				}
				else
				{
					ftformaddrow(getRowRateElement(O(eleid+"_rowrate"+(rowrate-1))));
				}
			}
			eleValBase(O(eleid+"_rowrate"+rowrate),objtag,objtype,eleval);
		}
	}
}
function eleValBase(obj,objtag,objtype,val,eleid)
{
	var layuiForm=$(obj).closest('.layui-form');
		if(layuiForm[0]!=null && val.indexOf("/_ftfiles/")<0)
		{
			var layFilter=layuiForm.attr('lay-filter');
			if(layFilter!=null&&layFilter!='')
			{
				var layObjVal=layui.form.val(layFilter)[eleid];
				if(layObjVal!=null)
				{
					var keyval={};
					keyval[eleid]=val;
					layui.form.val(layFilter,keyval);
					return;
				}
			}
		}

	if(val=="FT{NORESULT}")
	{
		if(objtag=="LABEL"||objtag=="SPAN"||objtag=="DIV"){$(obj).html("");return;};
		val="";
	}
	if(val.indexOf("/_ftfiles/")>=0)
	{
		var filename=val.substring(val.lastIndexOf("/"));
		filename=filename.substring(filename.indexOf("_")+1);
		var filehtml="<a href=\"javascript:void(0)\" onclick=\"location.href='/_ftpub/ftajax?pageaction=download&filename="+encodeURI(val)+"&rdm="+Math.random()+"'\" class=\"_ftfilealink\">" + filename + "</a>";
		if(objtag=="LABEL"||objtag=="SPAN")$(obj).html(filehtml);
		else $(obj).after(filehtml);
		return;
	}
	if ((objtag == "INPUT" && objtype == "CHECKBOX") || (objtag == "INPUT" && objtype == "RADIO")) {
			var objs = document.getElementsByName(obj.name);
			for (var i = 0; i < objs.length; i++) {
				if((","+val+",").indexOf(","+objs[i].value+",")>=0)objs[i].checked=true;
			}
		}
		else if (objtag == "SELECT" && obj.multiple) {
			for (var i = 0; i < obj.options.length; i++) {
				if((","+val+",").indexOf(","+obj.options[i].value+",")>=0)obj.options[i].selected=true;
			}
		}
		else if(objtag=="LABEL"||objtag=="SPAN"||objtag=="DIV")
		{
			$(obj).html(val);
		}
		else obj.value=val;
}
function eleCheck(eleid,checktype,_val)
{
	var val=(_val==null?$.trim(eleVal(eleid)):_val);
	if(val=='[FTNULL]')return true;
	if(checktype=="noempty")
	{
		if(val=="")return false;
	}
	else if(checktype=="int")
	{
		if (val == "" || !(/^(\+|-)?\d+$/.test(val)))return false;
	}
	else if(checktype=="decimal")
	{
		if (val == "" || isNaN(val))return false;
	}
	else if(checktype=="date")
	{
		if (val == "")return false;
		if(val.indexOf(' ')>0)val=val.split(' ')[0];
		var reg =/^(\d{4})-(\d{1,2})-(\d{1,2})$/;
		  if(!reg.test(val)||RegExp.$2>12||RegExp.$3>31){  
			  return false;
		  }
	}
	return true;
}
function eleValidate(eleid,checktype,alertinfo)
{
	if(!eleCheck(eleid,checktype)){info(alertinfo,2,null,"document.getElementById('"+eleid+"').focus();");return false;};var rateIndex=1;var rateObj=document.getElementById(eleid+'_rowrate'+rateIndex);while(rateObj!=null){if(!eleCheck(rateObj.id,checktype)){info(alertinfo,2,null,"document.getElementById('"+rateObj.id+"').focus();");return false;};rateIndex++;rateObj=document.getElementById(eleid+'_rowrate'+rateIndex);}
	return true;
}
function eleValidateEmpty(eleid,checktype,alertinfo,emptyvalue)
{
	if(emptyvalue==null)
	{
		if(eleCheck(eleid,'noempty')&&!eleCheck(eleid,checktype)){info(alertinfo,2,null,"document.getElementById('"+eleid+"').focus();");return false;};var rateIndex=1;var rateObj=document.getElementById(eleid+'_rowrate'+rateIndex);while(rateObj!=null){if(eleCheck(rateObj.id,'noempty')&&!eleCheck(rateObj.id,checktype)){info(alertinfo,2,null,"document.getElementById('"+rateObj.id+"').focus();");return false;};rateIndex++;rateObj=document.getElementById(eleid+'_rowrate'+rateIndex);}
	}
	else
	{
		if(!eleCheck(eleid,'noempty')){eleVal(eleid,emptyvalue);eleValidateEmptyReverse+="eleVal('"+eleid+"','');";}
		if(!eleCheck(eleid,checktype)){info(alertinfo,2,null,"document.getElementById('"+eleid+"').focus();");return false;};var rateIndex=1;var rateObj=document.getElementById(eleid+'_rowrate'+rateIndex);while(rateObj!=null){if(!eleCheck(rateObj.id,'noempty')){eleVal(rateObj.id,emptyvalue);eleValidateEmptyReverse+="eleVal('"+rateObj.id+"','');";}if(!eleCheck(rateObj.id,checktype)){info(alertinfo,2,null,"document.getElementById('"+rateObj.id+"').focus();");return false;};rateIndex++;rateObj=document.getElementById(eleid+'_rowrate'+rateIndex);}
	}
	return true;
}
var eleValidateEmptyReverse=null;
function eleValiEmpRev()
{
	if(eleValidateEmptyReverse!='')eval(eleValidateEmptyReverse);eleValidateEmptyReverse='';
}
function eleDim(eleid,ids,names)
{
	if(eleid.indexOf(',')>=0)
	{
		var _ids=eleid.split(',');
		for(var i=0;i<_ids.length;i++){eleDim(_ids[i],ids,names)}
		return false;
	}
	var obj = O(eleid);
	var objtag=obj.tagName.toUpperCase();
	var objtype=(obj.type==null?"":obj.type.toUpperCase());
	var _ids=ids.split("##");
	var _names=names.split("##");
	if (objtag == "SELECT")
	{
		$(obj).html("");
		for(var i=0;i<_ids.length;i++)
		{
			if(_ids[i]!=""||_names[i]!="")obj.options[i]=new Option(_names[i],_ids[i]);
		}

		var layuiForm=$(obj).closest('.layui-form');
		if(layuiForm[0]!=null)
		{
			var layFilter=layuiForm.attr('lay-filter');
			if(layFilter!=null&&layFilter!='')
			{
				var layObjVal=layui.form.val(layFilter)[eleid];
				if(layObjVal!=null)
				{ 
					layui.form.render('select',layFilter);
				}
			}
		}
	}
	else if ((objtag == "INPUT" && objtype == "CHECKBOX") || (objtag == "INPUT" && objtype == "RADIO"))
	{
		var objs = document.getElementsByName(obj.name);
		for(i=1;i<objs.length;i++)objs[i].outerHTML="";
		var checkhtml="";
		for(var i=0;i<_ids.length;i++)
		{
			if(_ids[i]!=""||_names[i]!="")checkhtml+="<input type=\""+objtype+"\" value=\""+_ids[i]+"\" name=\""+obj.name+"\"/>&nbsp;"+_names[i]+"&nbsp;";
		}
		objs[0].outerHTML=checkhtml;
	}
}
function eleClear(eleid,expectids,forcehiddenids)
{
	$("#"+eleid).find("*").each(function(){
		if(this==null)return true;
		if(this.name!=null && (this.name=='dl_allselect' || this.name=='dlcheckradio'))return true;
		if(this.id!=null)
		{
			if(expectids!=null && (","+expectids+",").indexOf(","+this.id+",")>=0)return true;
			//if(this.id.indexOf("ftform_liquid")==0)return true;
		}
		var objtag=this.tagName.toUpperCase();
		var objtype=(this.type==null?"":this.type.toUpperCase());
		if(objtag=="INPUT"&&(objtype==""||objtype=="TEXT"||objtype=="PASSWORD"))
		{
			eleVal(this.id,'');
			//this.value="";
		}
		else if(objtag=="INPUT"&&objtype=="HIDDEN")
		{
if(this.name!=null&&this.name=='Row1DeleteHdn')this.value="";
			if(forcehiddenids!=null && (","+forcehiddenids+",").indexOf(","+this.id+",")>=0)eleVal(this.id,'');
		}
		else if(objtag=="INPUT"&&objtype=="FILE")
		{
			eleVal(this.id,'');
			try
			{
				this.outerHTML+="";
			}
			catch (ex)
			{
			}
		}
		else if(objtag=="TEXTAREA")
		{
			eleVal(this.id,'');
		}
		else if (objtag == "SELECT") {
			for (var i = 0; i < this.options.length; i++) {
				this.options[i].selected=false;
			}
		}
		else if ((objtag == "INPUT" && objtype == "CHECKBOX") || (objtag == "INPUT" && objtype == "RADIO")) {
			var objs = document.getElementsByName(this.name);
			for (var i = 0; i < objs.length; i++) {
				objs[i].checked=false;
			}
		}
		if(this.name!=null && this.name.indexOf("rowrate_")==0)
		{
			var obj=$(this).parent().parent().prev()[0];
			while(obj!=null && obj.tagName=="TR" && obj.id!=null && obj.id!="")
			{
				$(obj).remove();
				obj=$(this).parent().parent().prev()[0];
			}
		}
	});
	$("#"+eleid).find("._ftfilealink,.noaddrow").remove();
}
function isImage(s)
{
	var reg = /^[^.]+\.(jpg|gif|png|jpeg)$/ig;
	return reg.test(s);
}
function showImage(id,wid)
{
	if(O(id).tagName=="INPUT")
	{
		var obj=$('#'+id).next()[0];
		if(obj!=null&&obj.tagName=='A'&&$(obj).attr('onclick')!=null)
		{
		var filelink=$(obj).attr('onclick');filelink=filelink.substring(filelink.indexOf('&filename=')+10,filelink.indexOf('&rdm='));
		$(obj).parent().next().html("<img class='noaddrow' src='"+filelink+"' border=0 style='cursor:pointer;width:"+wid+"px' title='点击查看原图' onclick=\"window.open('"+filelink+"')\"/>");
		}
		var ratei=1;
		obj=O(id+'_rowrate'+ratei);
		while(obj!=null)
		{
		obj=$(obj).next()[0];
		if(obj!=null&&obj.tagName=='A'&&$(obj).attr('onclick')!=null)
		{
		var filelink=$(obj).attr('onclick');filelink=filelink.substring(filelink.indexOf('&filename=')+10,filelink.indexOf('&rdm='));
		$(obj).parent().next().html("<img class='noaddrow'  src='"+filelink+"' border=0 style='cursor:pointer;width:"+wid+"px' title='点击查看原图' onclick=\"window.open('"+filelink+"')\"/>");
		}ratei++;obj=O(id+'_rowrate'+ratei);
		}
	}
	else if(O(id).tagName=="LABEL")
	{
		var obj=$('#'+id).children(0)[0];
		if(obj!=null&&obj.tagName=='A'&&$(obj).attr('onclick')!=null)
		{
		var filelink=$(obj).attr('onclick');filelink=filelink.substring(filelink.indexOf('&filename=')+10,filelink.indexOf('&rdm='));
		$(obj).parent().after("<img class='noaddrow' src='"+filelink+"' border=0 style='cursor:pointer;width:"+wid+"px' title='点击查看原图' onclick=\"window.open('"+filelink+"')\"/>");
		}
		var ratei=1;
		obj=O(id+'_rowrate'+ratei);
		while(obj!=null)
		{
		obj=$(obj).children(0)[0];
		if(obj!=null&&obj.tagName=='A'&&$(obj).attr('onclick')!=null)
		{
		var filelink=$(obj).attr('onclick');filelink=filelink.substring(filelink.indexOf('&filename=')+10,filelink.indexOf('&rdm='));
		$(obj).parent().after("<img class='noaddrow'  src='"+filelink+"' border=0 style='cursor:pointer;width:"+wid+"px' title='点击查看原图' onclick=\"window.open('"+filelink+"')\"/>");
		}ratei++;obj=O(id+'_rowrate'+ratei);
		}
	}
}
function eleFromDim(addBtnId,isFromRow0,selID,splitStr,eleIds)
{
	ftformdelrowall(O(addBtnId));
	var ids=eleIds.split(',');
	if(isFromRow0)$("#"+ids[0]).parent().parent().remove();
	var optionobjs=O(selID).options;
	for(var i=0;i<optionobjs.length;i++)
	{
		var val=optionobjs[i].text.split(splitStr);
		if(i==0)
		{
			if(O(ids[0])==null)
			{
				if(isFromRow0)O(addBtnId).click();
				else{alert('Row0 not exsit!');return false;}
			}
			for(var j=0;j<ids.length;j++)
			{
				eleVal(ids[j],val[j]);
			}
		}
		else
		{
			if(O(ids[0]+'_rowrate'+i)==null)
			{
				O(addBtnId).click();
			}
			for(var j=0;j<ids.length;j++)
			{
				eleVal(ids[j]+'_rowrate'+i,val[j]);
			}
		}
	}
}
function listLoading(eleid) {
    $("#" + eleid).html("<div align=center style='padding:50px'><img src='/_ftres/progress.gif'/></div>");
}
function getTree(define,id,setting,objpara,otherpara) {
                    var ele = newDialog("info", "loading","正在载入...",false);
                    $.post("/_ftpub/ftajax",{ "ajaxtype": "Info","infodefine":define,"otherpara":otherpara},function (data, textStatus) {
closeDialog("info");
var t = $.fn.zTree.init($("#"+id), setting, eval(data));
if(objpara!=null)eval(objpara+"=t");
}, "text");
}
function getTreeVal(treenode,indexpt,idid,nameid,dialogid)
{
	if(treenode!=null && (indexpt==null || treenode.id.indexOf(indexpt)==0))
	{
		if(indexpt==null){
		O(idid).value=treenode.id;
		}
		else
		{
		O(idid).value=treenode.id.substring(indexpt.length);
		}
		O(nameid).value=treenode.name;
		if(dialogid!=null)closeDialog(dialogid);
	}
}
var O = function(objName){return document.getElementById(objName)}
var modRate=new Array();
var modRateItemMax=new Array();
var RowRateMax=1;
var ReplaceTag="|INPUT|TEXTAREA|LABEL|SELECT|SPAN|";
var FTEleStartTag="";
function ftformdelrowall(obj)
{
	var obj=$(obj).parent().parent();
	var prevtr=obj.prev()[0];
	while(prevtr!=null && prevtr.id!=null&&prevtr.id.indexOf("tr_")==0)
	{
		$(prevtr).remove();
		prevtr=obj.prev()[0];
	}
}
function ftformdelrow(obj,delfirst)
{
	var oobj=obj;
	var obj=$(obj).parent().parent()[0];
	if(obj.tagName!="TR"){alert("删除行操作必须放在TD标签下！");return false;}
	if(obj.id==null||obj.id==""){
		if(delfirst==null || !delfirst){
        if($(oobj).next()[0]==null||$(oobj).next()[0].name!='Row1DeleteHdn'||$(oobj).next()[0].value!='yes')
			{
			     if($(oobj).next()[0]==null||$(oobj).next()[0].name!='Row1DeleteHdn')$("<input type=hidden name='Row1DeleteHdn' value='yes'>").insertAfter(oobj);
				 else if($(oobj).next()[0].value!='yes')$(oobj).next()[0].value='yes';
				 info("首行已被标记为删除状态",2);return false;
			}
			else 
			{
				$(oobj).next()[0].value='';info("首行取消删除状态",1);return false;
			}
		//alert("首行不能被删除！");return false;
		
		}
		else {
			if($(obj).next()[0]!=null && $(obj).next()[0].tagName=="TR" && $(obj).next()[0].id!=null && $(obj).next()[0].id=="tr_2")
			{
				alert("当仅有首行时，首行才能被删除！");return false;
			}
			else
			{
				$(obj).remove();
				return true;
			}
		}
	}
	var trindex=parseInt(obj.id.substring(3),10);
	var rowindex=trindex-1;
	var nexttr=$(obj).next()[0];
	$(obj).find("td").fadeTo(100,0,function(){
		$(obj).remove();
		while(nexttr!=null&&nexttr.id!=null&&nexttr.id.indexOf("tr_")==0)
		{
			nexttr.id="tr_"+trindex;
			$(nexttr).find("*").each(function(){
				if(this.id!=null && this.id.indexOf("_rowrate")>0)
				{
					this.id=this.id.replace(new RegExp("_rowrate"+(rowindex+1),"g"),"_rowrate"+(rowindex));
				}
				if($(this).attr('for')!=null && $(this).attr('for').indexOf("_rowrate")>0)
				{
					$(this).attr('for',$(this).attr('for').replace(new RegExp("_rowrate"+(rowindex+1),"g"),"_rowrate"+(rowindex)));
				}
				if(this.name!=null && this.name.indexOf("_rowrate")>0)
				{
					if(/msie 7|msie 6|msie 5/.test(navigator.userAgent.toLowerCase()) && !/msie 8|msie 9|msie 1/.test(navigator.userAgent.toLowerCase())){
						var newname=this.name.replace("_rowrate"+(rowindex+1),"_rowrate"+(rowindex));
						var newhtml=this.outerHTML.replace(new RegExp("name="+this.name,"g"),"name="+newname).replace(new RegExp("name=\""+this.name,"g"),"name=\""+newname);
						var newele=null;
						if(this.tagName=="INPUT")newele=document.createElement("<input name=\"" + newname + "\"/>");
						else if(this.tagName=="SELECT")newele=document.createElement("<select name=\"" + newname + "\"/>");
						else if(this.tagName=="TEXTAREA")newele=document.createElement("<textarea name=\"" + newname + "\"/>");
						else newele=document.createElement(this.tagName);
						newele.name=newname;
						$(newele).attr("name",newname);
						for(var i=0;i<this.attributes.length;i++)
						{
							var att=this.attributes[i];
							if(att.specified)
							{
								if(att.value!=null)
								{
									$(newele).attr(att.name,att.value);
								}
							}
						}
						try
						{
							$(newele).html($(this).html());
						}
						catch (e)
						{
						}
						newele.value=this.value;
						if(this.className!=null && this.className!="")newele.className=this.className;
						if(this.style.cssText!=null && this.style.cssText!="")newele.style.cssText=this.style.cssText;
						if(this.tagName=="INPUT")
						{
							if(this.type.toLowerCase()=="radio")newele.checked=this.checked;
							else if(this.type.toLowerCase()=="checkbox")newele.checked=this.checked;
							else if(this.type.toLowerCase()=="file"){};
						}
						$(newele).insertAfter($(this));
						/*
						var newele=$(this).clone()[0];
						$(newele).attr("name",newname);
						alert(newele.outerHTML);
						$(newele).insertAfter($(this));*/
						$(this).remove();
					}
					else
					{
						$(this).attr("name",this.name.replace(new RegExp("_rowrate"+(rowindex+1),"g"),"_rowrate"+(rowindex)));
					}
				}
				if(this.tagName=='SPAN'&&this.id.indexOf('ftformrateindex')==0)$(this).html(parseInt($(this).text(),10)-1);
			});
			trindex++;
			rowindex++;
			nexttr=$(nexttr).next()[0];
		}
	})
}
function ftformaddrow(obj,addtype,isfade)
{
	$(".layui-date").removeAttr("lay-key");
	if(event!=null&&event.shiftKey)
	{
		var obj=$(obj).parent().parent().prev()[0];
		if(obj.tagName=="TR" && obj.id!=null && obj.id!="")$(obj).remove();
		return;
	}
	addObj=$(obj).parent().parent();
	newObj=$(addObj).prev()[0];
	newEle=document.createElement("TR");

	newRowID=2;
	if(newObj.id!=null&&newObj.id!="")newRowID=Number(newObj.id.replace(/tr_/g,""))+1;
	newEle.id="tr_"+newRowID;
	newEle.className=newObj.className;
	for(var i=0;i<newObj.children.length;i++)
	{
		newEleChild=document.createElement("TD");
		if(newObj.children[i].className!=null && newObj.children[i].className!="")newEleChild.className=newObj.children[i].className;
		if(newObj.children[i].style.cssText!=null && newObj.children[i].style.cssText!="")newEleChild.style.cssText=newObj.children[i].style.cssText;
		if(newObj.children[i].colSpan!=null)newEleChild.colSpan=newObj.children[i].colSpan;
		if($(newObj.children[i]).attr('align')!=null)$(newEleChild).attr('align',$(newObj.children[i]).attr('align'));
		//eval("newEleChild.innerHTML=newObj.children[i].innerHTML.replace(/_"+(newRowID-1)+"__/g,\"_"+newRowID+"__\")");
		newEleChild.innerHTML=newObj.children[i].innerHTML;
		var passrate=-1;
		var tdeleidname=null;
		var newtdeleidname=null;
		var newEleTempHTML=newEleChild.innerHTML;
		var eleIdNameAllTemp="|"
		for(var ii=0;ii<newEleChild.children.length;ii++)
		{
			tdeleidname=newEleChild.children[ii].id;
			if(eleIdNameAllTemp.indexOf("|"+tdeleidname+"|")<0)//解决相同idname引起的问题
			{
				eleIdNameAllTemp+=tdeleidname+"|";
				if(tdeleidname!=null&&tdeleidname!="")
				{
					if(ReplaceTag.indexOf(newEleChild.children[ii].tagName)>=0&&tdeleidname.indexOf(FTEleStartTag)>=0)
					{
						passrate=tdeleidname.indexOf("_rowrate");
						if(passrate<0)
							{newtdeleidname=tdeleidname+"_rowrate1";
						  
						}
						else
							{newtdeleidname=tdeleidname.substring(0,passrate+8)+ (Number(tdeleidname.substring(passrate+8))+1);}
					newEleTempHTML=newEleTempHTML.replace(new RegExp("id="+tdeleidname+" ","g"),"id="+newtdeleidname+" ").replace(new RegExp("name="+tdeleidname+" ","g"),"name="+newtdeleidname+" ");
					newEleTempHTML=newEleTempHTML.replace(new RegExp("id="+tdeleidname+">","g"),"id="+newtdeleidname+">").replace(new RegExp("name="+tdeleidname+">","g"),"name="+newtdeleidname+">");
					newEleTempHTML=newEleTempHTML.replace(new RegExp("id="+tdeleidname+"/","g"),"id="+newtdeleidname+"/").replace(new RegExp("name="+tdeleidname+"/","g"),"name="+newtdeleidname+"/");
					newEleTempHTML=newEleTempHTML.replace(new RegExp("id='"+tdeleidname+"'","g"),"id='"+newtdeleidname+"'").replace(new RegExp("name='"+tdeleidname+"'","g"),"name='"+newtdeleidname+"'");
					newEleTempHTML=newEleTempHTML.replace(new RegExp("id='"+tdeleidname+"'","g"),"id='"+newtdeleidname+"'").replace(new RegExp("name='"+tdeleidname+"'","g"),"name='"+newtdeleidname+"'");
					newEleTempHTML=newEleTempHTML.replace(new RegExp("id='"+tdeleidname+"'","g"),"id='"+newtdeleidname+"'").replace(new RegExp("name='"+tdeleidname+"'","g"),"name='"+newtdeleidname+"'");
					newEleTempHTML=newEleTempHTML.replace(new RegExp("id=\""+tdeleidname+"\"","g"),"id=\""+newtdeleidname+"\"").replace(new RegExp("name=\""+tdeleidname+"\"","g"),"name=\""+newtdeleidname+"\"");
					newEleTempHTML=newEleTempHTML.replace(new RegExp("id=\""+tdeleidname+"\"","g"),"id=\""+newtdeleidname+"\"").replace(new RegExp("name=\""+tdeleidname+"\"","g"),"name=\""+newtdeleidname+"\"");
					newEleTempHTML=newEleTempHTML.replace(new RegExp("id=\""+tdeleidname+"\"","g"),"id=\""+newtdeleidname+"\"").replace(new RegExp("name=\""+tdeleidname+"\"","g"),"name=\""+newtdeleidname+"\"");
					newEleTempHTML=newEleTempHTML.replace(new RegExp("for=\""+tdeleidname+"\"","g"),"for=\""+newtdeleidname+"\"");
					//newEleTempHTML=newEleTempHTML.replace(new RegExp("hasDatepicker","g"),"");
					newEleTempHTML=newEleTempHTML.replace(new RegExp("<script[\\s\\S]*?>[\\s\\S]*?<\\/script>","ig"), '');
		
					}
				}
			}
			for(var jj=0;jj<newEleChild.children[ii].children.length;jj++)
			{
				tdeleidname=newEleChild.children[ii].children[jj].id;
				if(eleIdNameAllTemp.indexOf("|"+tdeleidname+"|")<0)//解决相同idname引起的问题
				{
					eleIdNameAllTemp+=tdeleidname+"|";
					if(tdeleidname!=null&&tdeleidname!="")
					{
						if(ReplaceTag.indexOf(newEleChild.children[ii].children[jj].tagName)>=0&&tdeleidname.indexOf(FTEleStartTag)>=0)
						{
							passrate=tdeleidname.indexOf("_rowrate");
							if(passrate<0)
								newtdeleidname=tdeleidname+"_rowrate1";
							else
								newtdeleidname=tdeleidname.substring(0,passrate+8)+ (Number(tdeleidname.substring(passrate+8))+1);
						newEleTempHTML=newEleTempHTML.replace(new RegExp("id="+tdeleidname+" ","g"),"id="+newtdeleidname+" ").replace(new RegExp("name="+tdeleidname+" ","g"),"name="+newtdeleidname+" ");
						newEleTempHTML=newEleTempHTML.replace(new RegExp("id="+tdeleidname+">","g"),"id="+newtdeleidname+">").replace(new RegExp("name="+tdeleidname+">","g"),"name="+newtdeleidname+">");
						newEleTempHTML=newEleTempHTML.replace(new RegExp("id="+tdeleidname+"/","g"),"id="+newtdeleidname+"/").replace(new RegExp("name="+tdeleidname+"/","g"),"name="+newtdeleidname+"/");
						newEleTempHTML=newEleTempHTML.replace(new RegExp("id='"+tdeleidname+"'","g"),"id='"+newtdeleidname+"'").replace(new RegExp("name='"+tdeleidname+"'","g"),"name='"+newtdeleidname+"'");
						newEleTempHTML=newEleTempHTML.replace(new RegExp("id='"+tdeleidname+"'","g"),"id='"+newtdeleidname+"'").replace(new RegExp("name='"+tdeleidname+"'","g"),"name='"+newtdeleidname+"'");
						newEleTempHTML=newEleTempHTML.replace(new RegExp("id='"+tdeleidname+"'","g"),"id='"+newtdeleidname+"'").replace(new RegExp("name='"+tdeleidname+"'","g"),"name='"+newtdeleidname+"'");
						newEleTempHTML=newEleTempHTML.replace(new RegExp("id=\""+tdeleidname+"\"","g"),"id=\""+newtdeleidname+"\"").replace(new RegExp("name=\""+tdeleidname+"\"","g"),"name=\""+newtdeleidname+"\"");
						newEleTempHTML=newEleTempHTML.replace(new RegExp("id=\""+tdeleidname+"\"","g"),"id=\""+newtdeleidname+"\"").replace(new RegExp("name=\""+tdeleidname+"\"","g"),"name=\""+newtdeleidname+"\"");
						newEleTempHTML=newEleTempHTML.replace(new RegExp("id=\""+tdeleidname+"\"","g"),"id=\""+newtdeleidname+"\"").replace(new RegExp("name=\""+tdeleidname+"\"","g"),"name=\""+newtdeleidname+"\"");
						newEleTempHTML=newEleTempHTML.replace(new RegExp("for=\""+tdeleidname+"\"","g"),"for=\""+newtdeleidname+"\"");
						//newEleTempHTML=newEleTempHTML.replace(new RegExp("hasDatepicker","g"),"");
						newEleTempHTML=newEleTempHTML.replace(new RegExp("<script[\\s\\S]*?>[\\s\\S]*?<\\/script>","ig"), '');
						}
					}
				}
			}
			//alert(newEleTempHTML);
			/*
			tdeleidname=newEleChild.children[ii].name;
			if(tdeleidname!=null&&tdeleidname!="")
			{
				if(ReplaceTag.indexOf(newEleChild.children[ii].tagName)>=0&&tdeleidname.indexOf(FTEleStartTag)>=0)
				{
					passrate=tdeleidname.indexOf("_rowrate");
					if(passrate<0)
						newEleChild.children[ii].name=tdeleidname+"_rowrate1";
					else
						newEleChild.children[ii].name=tdeleidname.substring(0,passrate+8)+ (Number(tdeleidname.substring(passrate+8))+1);
				}
			}*/
		}
		newEleChild.innerHTML=newEleTempHTML;//if(newEleChild.children.length>3){alert(newEleChild.children[3].outerHTML);alert(newEleChild.children[3].id.indexOf("filedel_")==0);newEleChild.children[3].outerHTML="";}
		for(var ii=0;ii<newEleChild.children.length;ii++)
		{
			if($(newEleChild.children[ii]).hasClass('noaddrow'))
			{
				$(newEleChild.children[ii]).remove();
				continue;
			}
			if(newEleChild.children[ii].id!=null&&newEleChild.children[ii].id.indexOf("filedel_")==0)
			{
				newEleChild.children[ii].outerHTML="<span></span>";
			}
			else
			{
				if(newEleChild.children[ii].id.indexOf("_lock_")<0)
				{
					if(newEleChild.children[ii].tagName=="INPUT"&&(newEleChild.children[ii].type=="text"||newEleChild.children[ii].type=="password"||newEleChild.children[ii].type=="hidden"))newEleChild.children[ii].value="";
					if(newEleChild.children[ii].tagName=="TEXTAREA")newEleChild.children[ii].value="";
					if(newEleChild.children[ii].tagName=="A")newEleChild.children[ii].innerText="";
					if((newEleChild.children[ii].tagName=="LABEL"||newEleChild.children[ii].tagName=="SPAN")&&$(newEleChild.children[ii]).hasClass('noadd'))newEleChild.children[ii].innerText="";
					if(newEleChild.children[ii].tagName=="SPAN"&&newEleChild.children[ii].id.indexOf("ftformrateindex")==0)
					newEleChild.children[ii].innerText=Number(newEleChild.children[ii].innerText)+1;
				}

				var newnewEleChild=newEleChild.children[ii];
				for(var jj=0;jj<newnewEleChild.children.length;jj++)
				{
					if(newnewEleChild.children[jj].id!=null&&newnewEleChild.children[jj].id.indexOf("filedel_")==0)
					{
						newnewEleChild.children[jj].outerHTML="<span></span>";
						continue;
					}
					else
					{
						if(newnewEleChild.children[jj].id.indexOf("_lock_")<0)
						{
							if(newnewEleChild.children[jj].tagName=="INPUT"&&(newnewEleChild.children[jj].type=="text"||newnewEleChild.children[jj].type=="password"||newnewEleChild.children[jj].type=="hidden"))newnewEleChild.children[jj].value="";
							if(newnewEleChild.children[jj].tagName=="TEXTAREA")newnewEleChild.children[jj].value="";
							if(newnewEleChild.children[jj].tagName=="A")newnewEleChild.children[jj].innerText="";
							if((newEleChild.children[ii].tagName=="LABEL"||newEleChild.children[ii].tagName=="SPAN")&&$(newEleChild.children[ii]).hasClass('noadd'))newEleChild.children[ii].innerText="";
							if(newnewEleChild.children[jj].tagName=="SPAN"&&newnewEleChild.children[jj].id.indexOf("ftformrateindex")==0)
							newnewEleChild.children[jj].innerText=Number(newnewEleChild.children[jj].innerText)+1;
						}
					}
				}
			}
		}

		newEle.appendChild(newEleChild);
		$(newEleChild).fadeTo(400,1,function(){
		});
	}
	//alert(newEle.outerHTML);
	//newEle.style="display:none";
	$(newEle).find("[onclick*='ftformdelrow']").show();
	var tdeleid = '';
	for(a=0;a<newEle.children.length;a++)
	{
		var newEleChild2=newEle.children[a];
		for(var b=0;b<newEleChild2.children.length;b++)
		{	
			var tdeleclassname=newEleChild2.children[b].className;	
			var tdeleidname2=newEleChild2.children[b].id;
			
			//$(''+tdeleid+'').removeAttr("lay-key");	
			if(tdeleclassname.indexOf("layui-date")>=0)
			{
				//$(''+tdeleid+'').removeAttr("lay-key");
				if(tdeleidname2.indexOf("_rowrate")>=0)
				{
					//console.log(tdeleclassname.indexOf("layui-date"));
					//console.log(tdeleidname2.indexOf("_rowrate"));
					tdeleid="#"+tdeleidname2;
					//console.log(tdeleid);
					//$(''+tdeleid+'').removeAttr("lay-key");
					// document.querySelector(tdeleid).removeAttribute("lay-key");
					//console.log("121");
					// console.log(document.getElementById(tdeleid));	
				
					setTimeout(function () {
						layui.use('laydate', function () {
							var laydate = layui.laydate; 
						laydate.render({
							elem:''+tdeleid+'' 
							
							});	
							console.log(tdeleid);
						});
					},500)
				}
	
			}
		}
	}
	$(newEle).insertBefore(addObj);
	StyleRow(newEle);
	//$(newEle).fadeTo(300,0.5);
	//addObj.parentElement.insertBefore(newEle,addObj);
	layui.use('form', function () {
		var form = layui.form; 
		form.render();
	   
	});
}
function getModRowMax(id)
{
	ModRowMax=1;
	newObj=$(O(id)).parent().parent().prev()[0];
	for(var i=0;i<newObj.children.length;i++)
	{
		for(var k=0;k<newObj.children[i].children.length;k++)
			{
				ele=newObj.children[i].children[k];
				if(ele.id.indexOf("r_")==0)
				{
					if(modRateItemMax[ele.id]!=null&&modRateItemMax[ele.id]!="")
					{
						if(Number(modRateItemMax[ele.id])>ModRowMax)
						ModRowMax=Number(modRateItemMax[ele.id]);
					}
				}
			}
	}
	return ModRowMax;
}
function getRowRateElement(obj)
{
	while(obj!=null)
	{	
		if(obj.tagName=="TR")
		{
			obj=$(obj).next()[0];
			for(var i=0;i<obj.children.length;i++)
			{
				for(var j=0;j<obj.children[i].children.length;j++)
				{
					if(obj.children[i].children[j].name!=null&&obj.children[i].children[j].name.indexOf("rowrate_")==0)
					{
						return obj.children[i].children[j];
					}
				}
			}
			break;
		}
		else
		{
			obj=obj.parentElement;
		}
	}
	return null;
}

jQuery.cookie = function(name, value, options) { 
          if (typeof value != 'undefined') { 
                    options = options || {}; 
                    if (value === null) { 
                              value = ''; 
                              options = $.extend({}, options); 
                              options.expires = -1; 
                    } 
                    var expires = ''; 
                    if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) { 
                              var date; 
                              if (typeof options.expires == 'number') { 
                                        date = new Date(); 
                                        date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000)); 
                              } else { 
                                        date = options.expires; 
                              } 
                              expires = '; expires=' + date.toUTCString(); 
                    } 
                    var path = options.path ? '; path=' + (options.path) : ''; 
                    var domain = options.domain ? '; domain=' + (options.domain) : ''; 
                    var secure = options.secure ? '; secure' : ''; 
                    document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join(''); 
          } else { 
                    var cookieValue = null; 
                    if (document.cookie && document.cookie != '') { 
                              var cookies = document.cookie.split(';'); 
                              for (var i = 0; i < cookies.length; i++) { 
                                        var cookie = jQuery.trim(cookies[i]); 
                                        if (cookie.substring(0, name.length + 1) == (name + '=')) { 
                                                  cookieValue = decodeURIComponent(cookie.substring(name.length + 1)); 
                                                  break; 
                                        } 
                              } 
                    } 
                    return cookieValue; 
          } 
};
