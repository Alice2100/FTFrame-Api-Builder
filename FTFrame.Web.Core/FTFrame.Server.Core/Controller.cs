using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace FTFrame.Server.Core
{
    [ApiController]
    public class Controller : ControllerBase
    {
        [Route("_ftpub/siteadd")]
        [HttpPost]
        [HttpGet]
        public string siteadd()
        {
            return Util.HTMLBody(Util.SiteAdd(Request));
        }
        [Route("_ftpub/siteupload")]
        [HttpPost]
        public string siteupload()
        {
            return Util.SiteUpload(Request);
        }
        [Route("_ftpub/ftpublish")]
        [HttpPost]
        [HttpGet]
        public string ftpublish()
        {
            //FTDP.Publish Publish    Util.HTMLBody
            return "";
        }
        [Route("_ftpub/ftpublishupdate")]
        [HttpPost]
        [HttpGet]
        public string ftpublishupdate()
        {
            var publish = new PublishUpdate();
            string restr = publish.Publish(Request);
            return restr;
        }
        [Route("_ftpub/state")]
        [HttpPost]
        [HttpGet]
        public string state()
        {
            return PublishUpdate.PublishStateXml;
        }
        [Route("_ftpub/share")]
        [HttpPost]
        [HttpGet]
        public string share()
        {
            return "";
        }
        //public string clientop()
        //{
        //    return Util.ClientOperation_Old(Request, Response);
        //}
        [Route("_ftpub/clientop")]
        [HttpPost]
        public  ActionResult ClientOP()
{
            return  Util.ClientOperation(Request, Response);
        }
        [Route("_ftpub/ftdllupload")]
        [HttpPost]
        public string ftdllupload()
        {
            return Util.DllUpload(Request);
        }
        [Route("_ftpub/hidden2zip")]
        [HttpPost]
        [HttpGet]
        public string hidden2zip()
        {
            return "";
        }
        [Route("_ftpub/zip2hidden")]
        [HttpPost]
        [HttpGet]
        public string zip2hidden()
        {
            return "";
        }
        [Route("_ftpub/ftform")]
        [HttpPost]
        public void ftform()
        {
            if (!Request.HasFormContentType) return;
            HttpContext.Response.ContentType = "text/html;charset=UTF-8";
            Util.HTMLBodyJquery_1(HttpContext);
            Form.Post(HttpContext);
            Util.HTMLBodyJquery_2(HttpContext);
        }
        [Route("_ftpub/ftformop")]
        [HttpPost]
        public void ftformop()
        {
            if (!Request.HasFormContentType) return;
            int.TryParse(Request.Form["optype"].FirstOrDefault<string>().Trim(),  out int optype);
            if(optype==10 || optype==11) Util.FormOP(HttpContext, Request);
            else
            {
                HttpContext.Response.ContentType = "text/html;charset=UTF-8";
                Util.HTMLBodyJquery_1(HttpContext);
                Util.FormOP(HttpContext, Request);
                Util.HTMLBodyJquery_2(HttpContext);
            }
        }
        [Route("_ftpub/ftajax")]
        [HttpPost]
        [HttpGet]
        public void ftajax()
        {
            Ajax.Init.Start(HttpContext);
        }
    }
}
