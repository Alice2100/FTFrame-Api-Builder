/**
 * @author Roger Wu
 * @version 1.0
 * added extend property oncheck
 * moded by Maobinbin @version 2.0
 */
 (function($){
 	$.extend($.fn, {
		isTag:function(tn) {
			if(!tn) return false;
			return $(this)[0].tagName.toLowerCase() == tn?true:false;
		},
		jTree:function(options) {$(this).find('li').each(function(){
				var isrightok=false;
				if($(this).attr("rightok")=='1')isrightok=true;
				else {
					$(this).find('li').each(function(){
						if($(this).attr("rightok")=='1')
						{
							isrightok=true;
							return false;
						}
					});
				}
				if(!isrightok)$(this).remove();
			});		var op = $.extend({checkFn:null, selected:"selected", exp:"expandable", coll:"collapsable", firstExp:"first_expandable", firstColl:"first_collapsable", lastExp:"last_expandable", lastColl:"last_collapsable", folderExp:"folder_expandable", folderColl:"folder_collapsable", endExp:"end_expandable", endColl:"end_collapsable",file:"file",ck:"checked", unck:"unchecked"}, options);
			return this.each(function(){
				var $this = $(this);
				var cnum = $this.children().length;
				$(">li", $this).each(function(){
					var $li = $(this);
					
					var first = $li.prev()[0]?false:true;
					var last = $li.next()[0]?false:true;
					$li.genTree({
						icon1:$li.attr("icon1"),//自加，用于判断节点是否添加了iconCls  
						icon2:$li.attr("icon2"),
						icon:$this.hasClass("treeFolder"),
						ckbox:$this.hasClass("treeCheck"),
						options: op,
						level: 0,
						exp:(cnum>1?(first?op.firstExp:(last?op.lastExp:op.exp)):op.endExp),
						coll:(cnum>1?(first?op.firstColl:(last?op.lastColl:op.coll)):op.endColl),
						showSub:(!$this.hasClass("collapse") && ($this.hasClass("expand") || (cnum>1?(first?true:false):true))),
						isLast:(cnum>1?(last?true:false):true)
					});
				});
				setTimeout(function(){
					if($this.hasClass("treeCheck")){
						var checkFn = eval($this.attr("oncheck"));
						if(checkFn && $.isFunction(checkFn)) {
							$("div.ckbox", $this).each(function(){
								var ckbox = $(this);
								ckbox.click(function(){
									var checked = $(ckbox).hasClass("checked");
									var items = [];
									if(checked){
										var tnode = $(ckbox).parent().parent();
										var boxes = $("input", tnode);
										if(boxes.size() > 1) {
											$(boxes).each(function(){
												items[items.length] = {name:$(this).attr("name"), value:$(this).val(), text:$(this).attr("text")};
											});
										} else {
											items = {name:boxes.attr("name"), value:boxes.val(), text:boxes.attr("text")};
										}		
									}								
									checkFn({checked:checked, items:items});														
								});
							});
						}
					}
					$("a", $this).click(function(event){
						$("div." + op.selected, $this).removeClass(op.selected);
						var parent = $(this).parent().addClass(op.selected);
						var $li = $(this).parents("li:first"), sTarget = $li.attr("target");
						if (sTarget) {
							if ($("#"+sTarget, $this).size() == 0) {
								$this.prepend('<input id="'+sTarget+'" type="hidden" />');
							}
							$("#"+sTarget, $this).val($li.attr("rel"));
						}
						
						$(".ckbox",parent).trigger("click");
						event.stopPropagation();
						$(document).trigger("click");
						if (!$(this).attr("target")) return false;
					});
				},1);
			});
		},
		subTree:function(op, level) {
			return this.each(function(){
				$(">li", this).each(function(){
					var $this = $(this);
					
					var isLast = ($this.next()[0]?false:true);
					$this.genTree({
						icon1:$this.attr("icon1"),//自加，用于判断节点是否添加了iconCls  
						icon2:$this.attr("icon2"),
						icon:op.icon,
						ckbox:op.ckbox,
						exp:isLast?op.options.lastExp:op.options.exp,
						coll:isLast?op.options.lastColl:op.options.coll,
						options:op.options,
						level:level,
						space:isLast?null:op.space,
						showSub:op.showSub,
						isLast:isLast
					});
					
				});
			});
		},
		genTree:function(options) {
			var op = $.extend({icon1:options.icon1,icon2:options.icon2,icon:options.icon,ckbox:options.ckbox,exp:"", coll:"", showSub:false, level:0, options:null, isLast:false}, options);
			return this.each(function(){
				var node = $(this);
				var tree = $(">ul", node);
				var parent = node.parent().prev();
				var checked = 'unchecked';
				if(op.ckbox) {
					if($(">.checked",parent).size() > 0) checked = 'checked';
				}
				if (tree.size()>0) {
					node.children(":first").wrap("<div></div>");
					// 设置class，设置的是具有子节点的node。  
                    var imgdivhtml = ''; 
                    if(op.icon) {  
                        if(op.showSub) {  
                            if(op.icon1 != null && op.icon1 !='') {imgdivhtml="<div style=\"background:url("+op.icon1+") no-repeat;background-position:0 2px;\"></div>";}  
                            else {imgdivhtml="<div class='"+ op.options.folderColl +"'></div>";}  
                        } else {  
                            if(op.icon2 != null && op.icon2 !='') {imgdivhtml="<div style=\"background:url("+op.icon2+") no-repeat;background-position:0 2px;\"></div>";}  
							else {imgdivhtml="<div class='"+ op.options.folderExp +"'></div>";}  
                        }  
                    }
					$(">div", node).prepend("<div class='" + (op.showSub ? op.coll : op.exp) + "'></div>"+(op.ckbox ?"<div class='ckbox " + checked + "'></div>":"")+imgdivhtml);
					op.showSub ? tree.show() : tree.hide();
					$(">div>div:first,>div>a", node).click(function(){
						var $fnode = $(">li:first",tree);
						if($fnode.children(":first").isTag('a')) tree.subTree(op, op.level + 1);
						var $this = $(this);
						var isA = $this.isTag('a');
						var $this = isA?$(">div>div", node).eq(op.level):$this;
						//if (!isA || tree.is(":hidden")) {
						if (true) {
							$this.toggleClass(op.exp).toggleClass(op.coll);
							if (op.icon) {
								var imgdiv=$(">div>div:last", node)[0];
								if(op.icon1 != null && op.icon1 !='' && op.icon2 != null && op.icon2 !='')
								{
									if(imgdiv.style.cssText!=null && imgdiv.style.cssText.indexOf(op.icon1)>0)
									{
										imgdiv.style.cssText="background:url("+op.icon2+") no-repeat;background-position:0 2px;";
									}
									else
									{
										imgdiv.style.cssText="background:url("+op.icon1+") no-repeat;background-position:0 2px;";
									}
								}
								else
								{
									$(imgdiv).toggleClass(op.options.folderExp).toggleClass(op.options.folderColl);
								}
							}
						}
						//(tree.is(":hidden"))?tree.slideDown("fast"):(isA?"":tree.slideUp("fast"));
						(tree.is(":hidden"))?tree.slideDown("fast"):tree.slideUp("fast");
						return false;
					});
					addSpace(op.level, node);
					if(op.showSub) tree.subTree(op, op.level + 1);
				} else {
					 // 具有父亲节点的节点的属性，在subTree中已经处理过，这个地方队没有父亲节点的节点做一个补充处理  
                    var imgdivhtml = '';  
					if(op.icon1 != null && op.icon1 !='') {imgdivhtml="<div style=\"background:url("+op.icon1+") no-repeat;background-position:0 3px;\"></div>";} 
					else  {imgdivhtml="<div class='file'></div>";}  
					node.children().wrap("<div></div>");			
					$(">div", node).prepend("<div class='node'></div>"+(op.ckbox?"<div class='ckbox "+checked+"'></div>":"")+imgdivhtml);
					addSpace(op.level, node);
					if(op.isLast)$(node).addClass("last");
				}
				if (op.ckbox) node._check(op);
				$(">div",node).mouseover(function(){
					$(this).addClass("hover");
				}).mouseout(function(){
					$(this).removeClass("hover");
				});
				if(/msie/.test(navigator.userAgent.toLowerCase()))
					$(">div",node).click(function(){
						$("a", this).trigger("click");
						return false;
					});
			});
			function addSpace(level,node) {
				if (level > 0) {					
					var parent = node.parent().parent();
					var space = !parent.next()[0]?"indent":"line";
					var plist = "<div class='" + space + "'></div>";
					if (level > 1) {
						var next = $(">div>div", parent).filter(":first");
						var prev = "";
						while(level > 1){
							prev = prev + "<div class='" + next.attr("class") + "'></div>";
							next = next.next();
							level--;
						}
						plist = prev + plist;
					}
					$(">div", node).prepend(plist);
				}
			}
		},
		_check:function(op) {
			var node = $(this);
			var ckbox = $(">div>.ckbox", node);
			var $input = node.find("a");
			var tname = $input.attr("tname"), tvalue = $input.attr("tvalue");
			var attrs = "text='"+$input.text()+"' ";
			if (tname) attrs += "name='"+tname+"' ";
			if (tvalue) attrs += "value='"+tvalue+"' ";
			
			ckbox.append("<input type='checkbox' style='display:none;' " + attrs + "/>").click(function(){
				var cked = ckbox.hasClass("checked");
				var aClass = cked?"unchecked":"checked";
				var rClass = cked?"checked":"unchecked";
				ckbox.removeClass(rClass).removeClass(!cked?"indeterminate":"").addClass(aClass);
				$("input", ckbox).attr("checked", !cked);
				$(">ul", node).find("li").each(function(){
					var box = $("div.ckbox", this);
					box.removeClass(rClass).removeClass(!cked?"indeterminate":"").addClass(aClass)
					   .find("input").attr("checked", !cked);
				});
				$(node)._checkParent();
				return false;
			});
			var cAttr = $input.attr("checked") || false;
			if (cAttr) {
				ckbox.find("input").attr("checked", true);
				ckbox.removeClass("unchecked").addClass("checked");
				$(node)._checkParent();
			}
		},
		_checkParent:function(){
			if($(this).parent().hasClass("tree")) return;
			var parent = $(this).parent().parent();
			var stree = $(">ul", parent);
			var ckbox = stree.find(">li>a").size()+stree.find("div.ckbox").size();
			var ckboxed = stree.find("div.checked").size();
			var aClass = (ckboxed==ckbox?"checked":(ckboxed!=0?"indeterminate":"unchecked"));
			var rClass = (ckboxed==ckbox?"indeterminate":(ckboxed!=0?"checked":"indeterminate"));
			$(">div>.ckbox", parent).removeClass("unchecked").removeClass("checked").removeClass(rClass).addClass(aClass);
			
			var $checkbox = $(":checkbox", parent);
			if (aClass == "checked") $checkbox.attr("checked","checked");
			else $checkbox.removeAttr("checked");
			
			parent._checkParent();
		}
	});
})(jQuery);