$(function () {
		var V=function(eleid){
			var obj = O(eleid);
			var layuiForm=$(obj).parent('.layui-form');
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
			return obj.value
		};
	var SV=function(eleid,objvalue){
		var obj = O(eleid);
		if(obj!=null)
		{
			var layuiForm=$(obj).parent('.layui-form');
			if(layuiForm[0]!=null)
			{
				var layFilter=layuiForm.attr('lay-filter');
				if(layFilter!=null&&layFilter!='')
				{
					var layObjVal=layui.form.val(layFilter)[eleid];
					if(layObjVal!=null)
					{ 
						layui.form.val(layFilter,{eleid,objvalue});
						return;
					}
				}
			}
			obj.value=objvalue
		}
	}
});
