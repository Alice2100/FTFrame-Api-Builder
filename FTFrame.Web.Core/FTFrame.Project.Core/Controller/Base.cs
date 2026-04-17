using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTFrame.Project.Core;
using FTFrame.Project.Core.Utils;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using FTFrame.Tool;
using FTFrame;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Web;

namespace Project.WebApi
{
    [Route("api/Base/[action]")]
    [ApiController]
    public class Base : ControllerBase
    {
        [HttpPost]
        public IActionResult UploadFile()
        {
            if (!FTFrame.Project.Core.User.IsLogin())
            {
                return new ContentResult { StatusCode = 401, Content = Api.AuthFailedJson(null, null, null, HttpContext), ContentType = "application/json" };
            }
            if (!Request.HasFormContentType)
            {
                return new ContentResult { StatusCode = 400, Content = Api.ErrorJson("Must be submitted for Form"), ContentType = "application/json" };
            }
            try
            {
                var req = this.Request;
                StringBuilder sb = new StringBuilder();
                foreach (var file in req.Form.Files)
                {
                    if (file.Length > 0)
                    {
                        string subPath = "/_ftfiles/" + DateTime.Now.ToString("yyyyMM");
                        string uploadPath = SysConst.RootPath + Path.DirectorySeparatorChar + "wwwroot" + subPath.Replace('/', Path.DirectorySeparatorChar);
                        Directory.CreateDirectory(uploadPath);
                        string filename = (new Random().Next(10000, 99999)).ToString() + (new Random().Next(10000, 99999)).ToString() + "_" + file.FileName;
                        using (var inputStream = new FileStream(uploadPath + Path.DirectorySeparatorChar + filename, FileMode.Create))
                        {
                            file.CopyTo(inputStream);
                            byte[] array = new byte[inputStream.Length];
                            inputStream.Seek(0, SeekOrigin.Begin);
                            inputStream.Read(array, 0, array.Length);
                        }
                        sb.Append(",{\"name\":\"" + file.FileName + "\",\"path\":\"" + subPath + "/" + filename + "\"}");
                    }
                }
                string json = sb.ToString();
                if (json != "") json = "[" + json.Substring(1) + "]";
                else json = "[]";
                return new ContentResult { Content = Api.ResultJson(json), ContentType = "application/json" };
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new ContentResult { StatusCode = 400, Content = Api.ErrorJson(ex.Message), ContentType = "application/json" };
            }
        }
        [HttpPost]
        public void DownloadFile()
        {
            if (!FTFrame.Project.Core.User.IsLogin())
            {
                return;
            }
            try
            {
                JObject jObject = null;
                var req = this.Request;
                req.EnableBuffering();
                using (var ms = new MemoryStream())
                {
                    req.Body.Position = 0;
                    req.Body.CopyTo(ms);
                    var buffer = ms.ToArray();
                    string content = Encoding.UTF8.GetString(buffer);
                    if (string.IsNullOrWhiteSpace(content)) content = "{}";
                    jObject = JObject.Parse(content);
                }
                string filePath = jObject["file"].ToString();
                string diskPath = SysConst.RootPath + Path.DirectorySeparatorChar + "wwwroot" + filePath.Replace('/', Path.DirectorySeparatorChar);
                if (System.IO.File.Exists(diskPath))
                {
                    string filecaption = diskPath.Substring(diskPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                    if (filecaption.IndexOf('_') > 0) filecaption = filecaption.Substring(filecaption.IndexOf('_') + 1);
                    using (FileStream fs = new FileStream(diskPath, FileMode.Open))
                    {
                        byte[] bytes = new byte[(int)fs.Length];
                        fs.Read(bytes, 0, bytes.Length);
                        fs.Close();
                        Response.ContentType = "application/octet-stream;charset=UTF-8";
                        Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                        Response.Headers.Add("Content-Disposition", "attachment; filename=" + Func.escape(filecaption));
                        Response.Body.WriteAsync(bytes);
                        Response.Body.FlushAsync();
                    }
                }
                else
                {
                    //return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        [HttpPost]
        public IActionResult Upload()
        {
            if (!FTFrame.Project.Core.User.IsLogin())
            {
                return new ContentResult { StatusCode = 401, Content = Api.AuthFailedJson(null, null, null, HttpContext), ContentType = "application/json" };
            }
            if (!Request.HasFormContentType)
            {
                return new ContentResult { StatusCode = 400, Content = Api.ErrorJson("Must be submitted for Form"), ContentType = "application/json" };
            }
            try
            {
                var req = this.Request;
                StringBuilder sb = new StringBuilder();
                foreach (var file in req.Form.Files)
                {
                    if (file.Length > 0)
                    {
                        string subPath = "/_ftfiles/" + DateTime.Now.ToString("yyyyMM");
                        string uploadPath = SysConst.RootPath + Path.DirectorySeparatorChar + "wwwroot" + subPath.Replace('/', Path.DirectorySeparatorChar);
                        Directory.CreateDirectory(uploadPath);
                        string extension = "";
                        int index = file.FileName.LastIndexOf('.');
                        if (index > 0)
                        {
                            extension = file.FileName.Substring(index);
                        }
                        string filename = "u" + (new Random().Next(100, 999)).ToString() + "_" + str.GetCombID() + extension;
                        using (var inputStream = new FileStream(uploadPath + Path.DirectorySeparatorChar + filename, FileMode.Create))
                        {
                            file.CopyTo(inputStream);
                            byte[] array = new byte[inputStream.Length];
                            inputStream.Seek(0, SeekOrigin.Begin);
                            inputStream.Read(array, 0, array.Length);
                        }
                        sb.Append(",{\"name\":\"" + file.FileName + "\",\"path\":\"" + subPath + "/" + filename + "\"}");
                    }
                }
                string json = sb.ToString();
                if (json != "") json = "[" + json.Substring(1) + "]";
                else json = "[]";
                return new ContentResult { Content = Api.ResultJson(json), ContentType = "application/json" };
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new ContentResult { StatusCode = 400, Content = Api.ErrorJson(ex.Message), ContentType = "application/json" };
            }
        }
    }
}