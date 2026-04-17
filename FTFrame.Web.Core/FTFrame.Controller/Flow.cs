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
using FTFrame.DBClient;
using FTFrame.Tool;
namespace FTFrame.WebApi
{
    [Route("api/Flow/[action]")]
    [ApiController]
    public class Flow : ControllerBase
    {
        [HttpPost]
        public IActionResult Join()
        {
            var userId = Project.Core.User.UserID();
            if (userId == null)
            {
                return new ContentResult { StatusCode = 401, Content = Api.AuthFailedJson(null, null, null, HttpContext), ContentType = "application/json" };
            }
            var jObject = Advance.GetJObject(this.Request);
            string FlowID = jObject["FlowID"]?.ToString();
            string WorkID = jObject["WorkID"]?.ToString();
            string MainTable = jObject["MainTable"]?.ToString();
            string MainTableKey = jObject["MainTableKey"]?.ToString();
            using (DB db = new DB())
            {
                ST st = db.GetTransaction();
                try
                {
                    Base.Core.Flow flow = new Base.Core.Flow(db, st, FlowID, WorkID, userId, MainTable, MainTableKey);
                    string ret = flow.Join();
                    if (ret == null)
                    {
                        st.Commit();
                        return new ContentResult { Content = Api.OperationSuccessJson(), ContentType = "application/json" };
                    }
                    else
                    {
                        st.Rollback();
                        return new ContentResult { Content = Api.ErrorJson(ret), ContentType = "application/json" };
                    }
                }
                catch (Exception ex)
                {
                    st.Rollback();
                    log.Error(ex);
                    return new ContentResult { Content = Api.ErrorJson(ex.Message), ContentType = "application/json" };
                }
            }
        }
        
        [HttpPost]
        public IActionResult Cancel()
        {
            var userId = Project.Core.User.UserID();
            if (userId == null)
            {
                return new ContentResult { StatusCode = 401, Content = Api.AuthFailedJson(null, null, null, HttpContext), ContentType = "application/json" };
            }
            var jObject = Advance.GetJObject(this.Request);
            string FlowID = jObject["FlowID"]?.ToString();
            string WorkID = jObject["WorkID"]?.ToString();
            string MainTable = jObject["MainTable"]?.ToString();
            string MainTableKey = jObject["MainTableKey"]?.ToString();
            using (DB db = new DB())
            {
                ST st = db.GetTransaction();
                try
                {
                    Base.Core.Flow flow = new Base.Core.Flow(db, st, FlowID, WorkID, userId, MainTable, MainTableKey);
                    string ret = flow.Cancel();
                    if (ret == null)
                    {
                        st.Commit();
                        return new ContentResult { Content = Api.OperationSuccessJson(), ContentType = "application/json" };
                    }
                    else
                    {
                        st.Rollback();
                        return new ContentResult { Content = Api.ErrorJson(ret), ContentType = "application/json" };
                    }
                }
                catch (Exception ex)
                {
                    st.Rollback();
                    log.Error(ex);
                    return new ContentResult { Content = Api.ErrorJson(ex.Message), ContentType = "application/json" };
                }
            }
        }
        
        [HttpPost]
        public IActionResult Judge()
        {
            var userId = Project.Core.User.UserID();
            if (userId == null)
            {
                return new ContentResult { StatusCode = 401, Content = Api.AuthFailedJson(null, null, null, HttpContext), ContentType = "application/json" };
            }
            var jObject = Advance.GetJObject(this.Request);
            string FlowID = jObject["FlowID"]?.ToString();
            string WorkID = jObject["WorkID"]?.ToString();
            string MainTable = jObject["MainTable"]?.ToString();
            string MainTableKey = jObject["MainTableKey"]?.ToString();
            int Judge = int.Parse(jObject["Judge"]?.ToString());
            string Mimo = jObject["Mimo"]?.ToString();
            string Para = jObject["Para"]?.ToString();
            using (DB db = new DB())
            {
                ST st = db.GetTransaction();
                try
                {
                    Base.Core.Flow flow = new Base.Core.Flow(db, st, FlowID, WorkID, userId, MainTable, MainTableKey);
                    string ret = flow.Judge((Enums.FlowJudge)Judge, Mimo, Para);
                    if (ret == null)
                    {
                        st.Commit();
                        return new ContentResult { Content = Api.OperationSuccessJson(), ContentType = "application/json" };
                    }
                    else
                    {
                        st.Rollback();
                        return new ContentResult { Content = Api.ErrorJson(ret), ContentType = "application/json" };
                    }
                }
                catch (Exception ex)
                {
                    st.Rollback();
                    log.Error(ex);
                    return new ContentResult { Content = Api.ErrorJson(ex.Message), ContentType = "application/json" };
                }
            }
        }
    }

    [Route("api/FlowForm/[action]")]
    [ApiController]
    public class FlowForm : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public IActionResult Join([FromForm] string FlowID, [FromForm] string WorkID, [FromForm] string MainTable, [FromForm] string MainTableKey)
        {
            using (DB db = new DB())
            {
                ST st = db.GetTransaction();
                try
                {
                    Base.Core.Flow flow = new Base.Core.Flow(db, st, FlowID, WorkID, Project.Core.User.UserID(), MainTable, MainTableKey);
                    string ret = flow.Join();
                    if (ret == null)
                    {
                        st.Commit();
                        return new ContentResult { Content = Api.OperationSuccessJson(), ContentType = "application/json" };
                    }
                    else
                    {
                        st.Rollback();
                        return new ContentResult { Content = Api.ErrorJson(ret), ContentType = "application/json" };
                    }
                }
                catch (Exception ex)
                {
                    st.Rollback();
                    log.Error(ex);
                    return new ContentResult { Content = Api.ErrorJson(ex.Message), ContentType = "application/json" };
                }
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult Cancel([FromForm] string FlowID, [FromForm] string WorkID, [FromForm] string MainTable, [FromForm] string MainTableKey)
        {
            using (DB db = new DB())
            {
                ST st = db.GetTransaction();
                try
                {
                    Base.Core.Flow flow = new Base.Core.Flow(db, st, FlowID, WorkID, Project.Core.User.UserID(), MainTable, MainTableKey);
                    string ret = flow.Cancel();
                    if (ret == null)
                    {
                        st.Commit();
                        return new ContentResult { Content = Api.OperationSuccessJson(), ContentType = "application/json" };
                    }
                    else
                    {
                        st.Rollback();
                        return new ContentResult { Content = Api.ErrorJson(ret), ContentType = "application/json" };
                    }
                }
                catch (Exception ex)
                {
                    st.Rollback();
                    log.Error(ex);
                    return new ContentResult { Content = Api.ErrorJson(ex.Message), ContentType = "application/json" };
                }
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult Judge([FromForm] string FlowID, [FromForm] string WorkID, [FromForm] int Judge, [FromForm] string Mimo, [FromForm] string Para, [FromForm] string MainTable, [FromForm] string MainTableKey)
        {
            using (DB db = new DB())
            {
                ST st = db.GetTransaction();
                try
                {
                    Base.Core.Flow flow = new Base.Core.Flow(db, st, FlowID, WorkID, Project.Core.User.UserID(), MainTable, MainTableKey);
                    string ret = flow.Judge((Enums.FlowJudge)Judge, Mimo, Para);
                    if (ret == null)
                    {
                        st.Commit();
                        return new ContentResult { Content = Api.OperationSuccessJson(), ContentType = "application/json" };
                    }
                    else
                    {
                        st.Rollback();
                        return new ContentResult { Content = Api.ErrorJson(ret), ContentType = "application/json" };
                    }
                }
                catch (Exception ex)
                {
                    st.Rollback();
                    log.Error(ex);
                    return new ContentResult { Content = Api.ErrorJson(ex.Message), ContentType = "application/json" };
                }
            }
        }
    }
}
