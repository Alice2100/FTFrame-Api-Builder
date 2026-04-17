function ftframe_ui_load()
{
newDialog('pageloading','loading','页面正在载入中...',true);
$('#pageloading').html("根据网络情况载入速度有所不同。<br>若要立即使用请<a href='javascript:void(0)' onclick='ftframe_ui_init();StyleInit();closeDialog(\"pageloading\")' style='color:red;text-decoration:underline'>点击</a>。<br><img src=\"/_ftres/progress.gif\"/>");
$('#pageloading').css({'line-height':'150%','padding-top':'10px'});
}
function ftframe_ui_init()
{
    //Minimize Content Box
		$(".content-box-header .content-box-updown img").css({ "cursor":"pointer" });
		$(".content-box-header .content-box-updown img").click( // When the h3 is clicked...
			function () {
			  if(this.src.indexOf("down.gif")>0){
			  this.src="/_ft/_pro/img/pro/up.gif";
			  $($(this).parent().parent().next()).slideUp(400);
			  $($(this).parent().parent().find(".content-box-tabs")).slideUp(400);
			  $($(this).parent().parent().find(".content-box-icon")).slideUp(400);
			  }
			  else {
			  this.src="/_ft/_pro/img/pro/down.gif";
			  $($(this).parent().parent().next()).slideDown(400);
			  $($(this).parent().parent().find(".content-box-tabs")).slideDown(400);
			  $($(this).parent().parent().find(".content-box-icon")).slideDown(400);
			  }
			  
			  
			}
		);
		
		//$(".content-box-header h3").css({ "cursor":"s-resize" }); // Give the h3 in Content Box Header a different cursor
		$(".closed-box .content-box-content").hide(); // Hide the content of the header if it has the class "closed"
		$(".closed-box .content-box-tabs").hide(); // Hide the tabs in the header if it has the class "closed"

		
		$('.content-box .content-box-content div.tab-content').hide(); // Hide the content divs
		$('ul.content-box-tabs li a.default-tab').each(function(){
			$(this).addClass('current');
			if($(this).parent().parent().hasClass("lottabs"))
			{
				$(this).addClass('currentlottabs');
			}
		});// Add the class "current" to the default tab
		$('.content-box-content div.default-tab').show(); // Show the div with class "default-tab"
		
		$('.content-box ul.content-box-tabs li a').click( // When a tab is clicked...
			function() { 
				$(this).parent().siblings().find("a").each(function(){
					$(this).removeClass('current');
					$(this).removeClass('currentlottabs');
				});// Remove "current" class from all tabs
				$(this).parent().siblings().find("a").removeClass('currentlottabs');
				$(this).addClass('current'); // Add class "current" to clicked tab
				if($(this).parent().parent().hasClass("lottabs"))
				{
					$(this).addClass('currentlottabs');
				}
				var currentTab = $(this).attr('href'); // Set variable "currentTab" to the value of href of clicked tab
				$(currentTab).siblings().hide(); // Hide all content divs
				    if(currentTab.indexOf("_normal")>=0)
				    {
				        $(currentTab).show();
				    }
				    else
				    {
				        $(currentTab).fadeTo(0,0);
				        $(currentTab).show(); // Show the content div with the id equal to the id of clicked tab
				        $(currentTab).fadeTo(400,1,function () {
				        $(currentTab)[0].style.display="none";
				        $(currentTab)[0].style.cssText="";
				        $(currentTab)[0].style.display="";
				        });
				    }
				return false; 
			}
		);

    //Close button:
		
		$(".close").click(
			function () {
				$(this).parent().fadeTo(400, 0, function () { // Links with the class "close" will close parent
					$(this).slideUp(400);
				});
				return false;
			}
		);
		
		$('.check-all').click(
			function(){
				$(this).parent().parent().parent().parent().find("input[type='checkbox']").attr('checked', $(this).is(':checked'));   
			}
		);
		
		if(O('pageloading')!=null)closeDialog('pageloading');

}
$(document).ready(function(){
	ftframe_ui_init();
	
});