$(function () {
		 V=function(eleid){
			var obj = O(eleid);
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
			return obj.value
		};
	 SV=function(eleid,objvalue){
		var obj = O(eleid);
		if(obj!=null)
		{
			var layuiForm=$(obj).closest('.layui-form');
			if(layuiForm[0]!=null)
			{
				var layFilter=layuiForm.attr('lay-filter');
				if(layFilter!=null&&layFilter!='')
				{
					var layObjVal=layui.form.val(layFilter)[eleid];
					if(layObjVal!=null)
					{
					    var keyval={};
						keyval[eleid]=objvalue;
						layui.form.val(layFilter,keyval);
						return;
					}
				}
			}
			obj.value=objvalue
		}
	}
});
