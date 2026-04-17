using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FTFrame.Tool;
namespace FTFrame.Project.Core
{
    public class ProjectFilter
    {
        public static void Create(RazorPagesOptions options, IConfiguration config)
        {
            //This is page authorization, and Api authorization is available on Api Auth
            //options.Conventions.AddFolderApplicationModelConvention(
            //        "/_ft",
            //        model => model.Filters.Add(new ProjectPageFilter(config)));
            //options.Conventions.AddFolderApplicationModelConvention(
            //        "/demo",
            //        model => model.Filters.Add(new ProjectPageFilter(config)));
        }
        public static bool Page(HttpContext context)
        {
            //This is page authorization, and Api authorization is available on Api Auth
            if (!Tool.UserTool.IsLogin() && context.Request.Path.Value != "/_frame/login")
            {
                context.Response.Redirect("/_frame/login?path=" + HttpUtility.UrlEncode(context.Request.Path.Value), true);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Unified concatenation of SQL conditions for list page queries
        /// </summary>
        /// <param name="context"></param>
        /// <param name="paras"></param>
        /// <param name="reqDic"></param>
        /// <returns></returns>
        public static string ListSqlAppend(HttpContext context, string[] paras, Dictionary<string, object> reqDic)
        {
            //var baseUrl = Api.BaseUrl(context);// http://domin/test/api?list
            //var refererHost = context.Request.GetTypedHeaders().Referer?.Host; // www.domin.com
            //log.Debug(refererHost, "RefererHost");
            //if (refererHost == "www.domin.com")
            //{
            //    switch (baseUrl)
            //    {
            //        case var _ when baseUrl.EndsWith("/test1/api?list"):
            //        case var _ when baseUrl.EndsWith("/test2/api?list"):
            //        case var _ when baseUrl.EndsWith("/test3/api?list"):
            //            return " and publish_stat=1";
            //    }
            //}
            return "";
        }
    }
    public class ProjectAsyncActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var resultContext = await next();
        }
    }
    public class ProjectAsyncPageFilter : IAsyncPageFilter
    {
        private readonly IConfiguration _config;

        public ProjectAsyncPageFilter(IConfiguration config)
        {
            _config = config;
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context,
                                                      PageHandlerExecutionDelegate next)
        {
            var path = context.HttpContext.Request.Path;
            if (path.Value == "/")
            {
                context.HttpContext.Response.Redirect("/_frame/index", true);
                return;
            }
            else if (!Tool.UserTool.IsLogin() && path.Value != "/_frame/login")
            {
                context.HttpContext.Response.Redirect("/_frame/login?path=" + HttpUtility.UrlEncode(path.Value), true);
                return;
            }
            await next.Invoke();
        }
    }
    public class ProjectPageFilter : IPageFilter
    {
        private readonly IConfiguration _config;

        public ProjectPageFilter(IConfiguration config)
        {
            _config = config;
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            //var path = context.HttpContext.Request.Path;
            //if (path.Value == "/")
            //{
            //    context.HttpContext.Response.Redirect("/_frame/index", true);
            //}
            //else if (!Tool.UserTool.IsLogin() && path.Value != "/_frame/login")
            //{
            //    context.HttpContext.Response.Redirect("/_frame/login?path=" + HttpUtility.UrlEncode(path.Value), true);
            //}
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
        }
    }
}
