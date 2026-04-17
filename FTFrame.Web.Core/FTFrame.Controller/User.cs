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
namespace FTFrame.WebApi
{
    //[Route("api/User/[action]")]
    //[ApiController]
    public class User : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromForm] string userName, [FromForm] string passWord)
        {
            FTFrame.Project.Model.Demo.Demo_Fitting a = new FTFrame.Project.Model.Demo.Demo_Fitting()
            {
                id = "123",
                FittingName = "顶顶顶",
                Unit = "xx"
            };
            (string x1, string x2, string x3) b;
            b.x1 = "1";
            b.x2 = "2";
            b.x3 = "3";
            var d = new { userId = "123", token = Jwt.Generate("123", "张三", "corpid") };
            var c = new Dictionary<string, string>() { { "userId", "123" }, { "token", Jwt.Generate("123", "张三", "corpid") } };

            string strJson = JsonConvert.SerializeObject(d);
            return new ContentResult { Content = Api.ResultJson(strJson), ContentType = "application/json" };
        }
        [Authorize]
        [HttpGet]
        public IActionResult RefreshToken()
        {
            string userId = Project.Core.User.UserID();
            string userName = Project.Core.User.UserName();
            string strJson = JsonConvert.SerializeObject(new { token = Jwt.Generate(userId, userName, "corpid") });
            return new ContentResult { Content = Api.ResultJson(strJson), ContentType = "application/json" };
        }
        [Authorize]
        [HttpGet]
        public IActionResult OP()
        {
            //Authorization: Bearer <token>
            FTFrame.Project.Model.Demo.Demo_Fitting a = new FTFrame.Project.Model.Demo.Demo_Fitting()
            {
                id = "123",
                FittingName = "顶顶顶",
                Unit = "xx"
            };
            //FTHttpContext.Current.Items.Add("1","2");
            //var a1 = FTHttpContext.Current.User.Claims;
            string strJson = JsonConvert.SerializeObject(a);
            return new ContentResult { Content = Api.ResultJson(strJson), ContentType = "application/json" };
        }
    }
}
