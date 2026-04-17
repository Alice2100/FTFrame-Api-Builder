using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;
using FTFrame.Server.Core.Tool;
using System.Xml;
using System.Collections;
using FTFrame.Project.Core;
using CoreHttp = Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using FTFrame.Server.Core.Model;
using FTFrame.Project.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using FTFrame.Project.Core.WorkFlow;

namespace FTFrame.Server.Core
{
    public class Front
    {
        public static string FrontListMenu()
        {
            List<(string type, string ComName, string Caption, string Developer, string CreateTime)> list = new List<(string type, string ComName, string Caption, string Developer, string CreateTime)>();
            using (DB db = new DB(SysConst.ConnectionStr_FTDP))
            {
                string sql = null;
                sql = "select ComName,Caption,CreateTime,Developer from ft_ftdp_front_list where IsNewest=1";
                using (DR dr = db.OpenRecord(sql))
                {
                    while (dr.Read())
                    {
                        list.Add(("list", dr.GetString(0), dr.GetStringNoNULL(1), dr.GetStringNoNULL(3), str.GetDateTime(dr.GetDateTime(2))));
                    }
                }
                sql = "select ComName,Caption,CreateTime,Developer from ft_ftdp_front_form where IsNewest=1";
                using (DR dr = db.OpenRecord(sql))
                {
                    while (dr.Read())
                    {
                        list.Add(("form", dr.GetString(0), dr.GetStringNoNULL(1), dr.GetStringNoNULL(3), str.GetDateTime(dr.GetDateTime(2))));
                    }
                }
            }
            list = list.OrderBy(r => r.ComName).Select(r => r).ToList();
            StringBuilder sb = new StringBuilder();
            int loop = 1;
            foreach (var item in list)
            {
                sb.Append("<el-menu-item index=\"" + loop + "\" @click=\"changeurl('" + item.ComName.Replace("/", "_") + "')\">");
                sb.Append("<i class=\"" + (item.type == "list" ? "el-icon-menu" : "el-icon-setting") + "\"></i>");
                sb.Append("<span>" + item.ComName + " - " + item.Caption + "<el-dropdown @command=\"dropdownCmd\">");
                sb.Append("<span class=\"el-dropdown-link\">");
                sb.Append("<i class=\"el-icon-arrow-down el-icon--right\" style=\"color:white\"></i></span>");
                sb.Append("<el-dropdown-menu slot=\"dropdown\">");
                sb.Append("<el-dropdown-item icon=\"el-icon-download\" command=\"download1|" + item.type + "|" + item.ComName.Replace("/", "_") + "\">Vue Component</el-dropdown-item>");
                sb.Append("<el-dropdown-item icon=\"el-icon-download\" command=\"download2|" + item.type + "|" + item.ComName.Replace("/", "_") + "\">Test Page</el-dropdown-item>");
                sb.Append("<el-dropdown-item icon=\"el-icon-delete\" command=\"delete|" + item.type + "|" + item.ComName.Replace("/", "_") + "\">Delete</el-dropdown-item>");
                sb.Append("<el-dropdown-item>" + item.Developer + "</el-dropdown-item>");
                sb.Append("<el-dropdown-item>" + item.CreateTime + "</el-dropdown-item>");
                sb.Append("</el-dropdown-menu>");
                sb.Append("</el-dropdown></span>");
                sb.AppendLine("</el-menu-item>");
                loop++;
            }
            return sb.ToString();
        }
        public static string FrontDelete(string type, string comName)
        {
            using (DB db = new DB(SysConst.ConnectionStr_FTDP))
            {
                string sql = null;
                if (type == "list")
                {
                    sql = "update ft_ftdp_front_list set IsNewest=0 where ComName='" + comName.D2() + "' and IsNewest=1";
                }
                else if (type == "form")
                {
                    sql = "update ft_ftdp_front_form set IsNewest=0 where ComName='" + comName.D2() + "' and IsNewest=1";
                }
                db.ExecSql(sql);
                string comfilepath = AppDomain.CurrentDomain.BaseDirectory + "" + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "_ft" + Path.DirectorySeparatorChar + "_front" + Path.DirectorySeparatorChar + "com" + Path.DirectorySeparatorChar + "" + comName + ".vue";
                string testfilepath = AppDomain.CurrentDomain.BaseDirectory + "" + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "_ft" + Path.DirectorySeparatorChar + "_front" + Path.DirectorySeparatorChar + comName + "_test.html";
                comfilepath = comfilepath.Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString());
                testfilepath = testfilepath.Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString());
                try
                {
                    if (File.Exists(comfilepath)) File.Delete(comfilepath);
                    if (File.Exists(testfilepath)) File.Delete(testfilepath);
                }
                catch { }
                return null;
            }
        }
    }
    [ApiController]
    public class FrontController : ControllerBase
    {
        [Route("_ftfront/delete")]
        [HttpPost]
        public string delete([FromForm] string type, [FromForm] string comName)
        {
            if (!UserTool.IsLogin()) return SysConst.NotLogin;
            Front.FrontDelete(type, comName);
            return "";
        }
        [Route("_ftfront/download")]
        [HttpGet]
        public ActionResult download([FromQuery] string type, [FromQuery] string comName)
        {
            if (!UserTool.IsLogin()) return Content(SysConst.NotLogin);
            string filepath = null;
            string filename = null;
            string filefullpath = null;
            if (type == "1")
            {
                filepath = "/_ft/_front/com/" + comName + ".vue.com";
                filefullpath = AppDomain.CurrentDomain.BaseDirectory + "" + Path.DirectorySeparatorChar + "wwwroot" + filepath;
                filefullpath = filefullpath.Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString());
                filefullpath = filefullpath.Replace("/", Path.DirectorySeparatorChar.ToString());
                filename = comName + ".vue";
            }
            else if (type == "2")
            {
                filepath = "/_ft/_front/" + comName + "_test.html";
                filefullpath = AppDomain.CurrentDomain.BaseDirectory + "" + Path.DirectorySeparatorChar + "wwwroot" + filepath;
                filefullpath = filefullpath.Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString());
                filefullpath = filefullpath.Replace("/", Path.DirectorySeparatorChar.ToString());
                filename = comName + "_test.html";
            }
            if (!System.IO.File.Exists(filefullpath)) return Content("");
            return File(filepath, "application/force-download", filename);
        }
    }
}
