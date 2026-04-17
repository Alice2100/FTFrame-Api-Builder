<%@ WebHandler Language="C#" Class="uploadsave" %>

using System;
using System.Web;
using System.IO;
using FTDP.Tool;
public class uploadsave : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

    try
		{
        context.Response.ContentType = "text/plain";   
    context.Response.Charset = "utf-8";   

    HttpPostedFile file = context.Request.Files["Filedata"];   

    if (file != null)  
    {  
	string dir=str.GetYearMonth();
	string uploadPath = AppDomain.CurrentDomain.BaseDirectory + "\\_ftfiles\\" +dir;
	if (!Directory.Exists(uploadPath))
	{
	    Directory.CreateDirectory(uploadPath);
	}
	string newfilename=str.GetDateTime().Replace("-", "").Replace(" ", "").Replace(":", "") + "_" + file.FileName;
	string savefilename = "/_ftfiles/" + dir +"/"+ newfilename;
	
       file.SaveAs(uploadPath +"\\"+ newfilename);  

       context.Response.Write(savefilename);  
    }   
    else  
    {   
        context.Response.Write("0");   
    }  
		}
		catch(Exception ex){log.Exception(ex);}
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}