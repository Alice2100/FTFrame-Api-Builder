using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
namespace FTFrame
{
    public class FTModule : IHttpModule
    {
        public void Init(HttpApplication application)
        {
            application.AcquireRequestState += (new EventHandler(this.Application_AcquireRequestState));
            application.BeginRequest += new EventHandler(application_BeginRequest);
        }

        void application_BeginRequest(object source, EventArgs e)
        {
            Regex regex = new Regex("(\\w+)_ft_(\\w+).aspx", RegexOptions.Compiled);
            Match match = regex.Match(System.IO.Path.GetFileName(((HttpApplication)source).Context.Request.Path));
            //FTFrame.Tool.log.Error(HttpContext.Current.Request.Url + "");
            //string url = HttpContext.Current.Request.Url + "";
            //string imgUrl = "http://erp.house.com/_ftfiles/";
            //FTFrame.Tool.log.Error("url:" + url);
            //FTFrame.Tool.log.Error("imgUrl:" + imgUrl);
            //FTFrame.Tool.log.Error(url.IndexOf("_ftfiles") + "");
            //url = url.Substring(url.IndexOf("_ftfiles"), url.Length - url.IndexOf("_ftfiles"));
            //FTFrame.Tool.log.Error(url);
            //FTFrame.Tool.log.Error(HttpContext.Current.Server.MapPath(System.IO.Path.GetFileName(((HttpApplication)source).Context.Request.Path)));
            //HttpContext.Current.Response.ContentType="img/jpeg";
            //HttpContext.Current.Response.WriteFile(HttpContext.Current.Server.MapPath(System.IO.Path.GetFileName(((HttpApplication)source).Context.Request.Path)));
            //if (url.IndexOf(imgUrl) > 0)
            //{
            //    FTFrame.Tool.log.Error("go");
            //}
            if (match.Success)
            {
                string FileName = match.Groups[1].Value;
                string ID = match.Groups[2].Value;
                string rewritePath = FileName + ".aspx?id=" + ID;
                ((HttpApplication)source).Context.RewritePath(rewritePath);
            }
        }
        private void Application_AcquireRequestState(Object source, EventArgs e)
        {
            string appendpath = "_pro/";
            string path = ((HttpApplication)source).Context.Request.Path.ToLower();
            if (!path.StartsWith("/__sys") && path.EndsWith(".aspx"))
            {
                if (!(path.EndsWith(appendpath + "index.aspx") && !path.EndsWith(appendpath + "main/index.aspx")) && path.IndexOf(appendpath + "res/") < 0 && path.IndexOf("_ftpub/") < 0 && path.IndexOf("_app_interface/") < 0)
                {
                    FTFrame.Base.UserClass.LoginInit(((HttpApplication)source).Context.Request, ((HttpApplication)source).Context.Response);
                    if ((!FTFrame.Tool.UserTool.IsLogin() && !FTFrame.Tool.UserTool.IsAdmin()))
                    {
                        ((HttpApplication)source).Context.Response.Redirect("~/" + appendpath + "index.aspx?path=" + HttpUtility.UrlEncode(((HttpApplication)source).Context.Request.Url.PathAndQuery), true);
                    }
                    else
                    {
                        string relativeurl = ((HttpApplication)source).Context.Request.AppRelativeCurrentExecutionFilePath.ToLower();
                        if (relativeurl.StartsWith("~")) relativeurl = relativeurl.Substring(1);

                        //ÌØÊâurl´¦Àí
                        if (path.StartsWith("/_base/comdic.aspx"))
                        {
                            relativeurl = "/_base/comdic.aspx?name=" + ((HttpApplication)source).Context.Request["name"] + "&tag=" + ((HttpApplication)source).Context.Request["tag"];
                        }
                        else if (path.StartsWith("/_base/comset.aspx"))
                        {
                            relativeurl = "/_base/comset.aspx?name=" + ((HttpApplication)source).Context.Request["name"] + "&tag=" + ((HttpApplication)source).Context.Request["tag"];
                        }

                        if (!FTFrame.Base.UserClass.HasePageRight(relativeurl, ((HttpApplication)source).Context.Request))
                        {
                            ((HttpApplication)source).Context.Response.Redirect("~/__sys/noright.aspx", true);
                        }
                        else
                        {
                            if (!Interface.Right.PageAllFilter(((HttpApplication)source).Context, path, "~/__sys/noright.aspx"))
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }
        public void Dispose()
        {
        }
    }
}
