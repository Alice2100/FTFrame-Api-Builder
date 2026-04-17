// -----------------------------------------------------------------------
// Eros Fratini - eros@recoding.it
// jqprint 0.3
//
// - 19/06/2009 - some new implementations, added Opera support
// - 11/05/2009 - first sketch
//
// Printing plug-in for jQuery, evolution of jPrintArea: http://plugins.jquery.com/project/jPrintArea
// requires jQuery 1.3.x
//
// Licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php
//------------------------------------------------------------------------
(function($) {
var oldHTML = $.fn.html;
$.fn.formhtml =function() {
if (arguments.length) return oldHTML.apply(this,arguments);
$("input,textarea,button", this).each(function() {
this.setAttribute('value',this.value);
});
$(":radio,:checkbox", this).each(function() {
if (this.checked) this.setAttribute('checked', 'checked');
else this.removeAttribute('checked');
});
$("option", this).each(function() {
if (this.selected) this.setAttribute('selected', 'selected');
else this.removeAttribute('selected');
});
return oldHTML.apply(this);
};
})(jQuery);
(function($) {
    var opt;

    $.fn.jqprint = function (options) {
        opt = $.extend({}, $.fn.jqprint.defaults, options);

        var $element = (this instanceof jQuery) ? this : $(this);
        
        if (opt.operaSupport && navigator.userAgent.indexOf("Opera") > -1) 
        { 
            var tab = window.open("","jqPrint-preview");
            tab.document.open();

            var doc = tab.document;
        }
        else 
        {
            var $iframe = $("<iframe  />");
        
            if (!opt.debug) { $iframe.css({ position: "absolute", width: "0px", height: "0px", left: "-600px", top: "-600px" }); }

            $iframe.appendTo("body");
            var doc = $iframe[0].contentWindow.document;
        }
        
        if (opt.importCSS)
        {
            if ($("link[media=print]").length > 0) 
            {
                $("link[media=print]").each( function() {
                    doc.write("<link type='text/css' rel='stylesheet' href='" + $(this).attr("href") + "' media='print' />");
                });
            }
            else 
            {
                $("link").each( function() {
                    doc.write("<link type='text/css' rel='stylesheet' href='" + $(this).attr("href") + "' />");
                });
            }
        }
        
        if (opt.printContainer) { doc.write($element.outer()); }
        else { $element.each( function() { doc.write($(this).formhtml()); }); }
        //alert($(this).formhtml());
        doc.close();
        
        ((opt.operaSupport && navigator.userAgent.indexOf("Opera") > -1) ? tab : $iframe[0].contentWindow).focus();
        setTimeout( function() { ((opt.operaSupport && navigator.userAgent.indexOf("Opera") > -1) ? tab : $iframe[0].contentWindow).print(); if (tab) { tab.close(); } }, 1000);
    }
    
    $.fn.jqprint.defaults = {
		debug: false,
		importCSS: true, 
		printContainer: true,
		operaSupport: true
	};

    // Thanks to 9__, found at http://users.livejournal.com/9__/380664.html
	//onclick="$('#tab1 form table:eq(0)').jqprint();"
    jQuery.fn.outer = function() {
	  var rs='';
	  rs+='<link href="/_ftres/js/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" type="text/css" />';
	  rs+='<script type="text/javascript" src="/_ftres/js/jquery-1.9.1.min.js"></script>';
	  rs+='<script type="text/javascript" src="/_ftres/js/jquery-ui-1.10.3.custom.min.js"></script>';
	  rs+='<script type="text/javascript" src="/_ftres/js/ftmain.js"></script>';
	  rs+='<script type="text/javascript" src="/_ftres/swfobject.js"></script>';
	  rs+='<link href="/_pro/css/main.css" type="text/css" rel="stylesheet" />';
	  rs+='<link href="/_pro/css/ftdp.css" type="text/css" rel="stylesheet" />';
      return ''+$($('<div></div>').html($(this).formhtml())).formhtml();
    } 
})(jQuery);