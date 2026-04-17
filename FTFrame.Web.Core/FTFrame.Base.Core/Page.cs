using FTFrame.Base.Core;
using FTFrame.DBClient;
using FTFrame.Tool;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTFrame.Project.Core
{
    [Route("page/ajax/[action]")]
    [ApiController]
    public class Ajax : ControllerBase
    {
        [HttpGet]
        [HttpPost]
        public IActionResult get([FromForm] string pathEnc, [FromForm] string tag, [FromForm] string paras)
        {
            try
            {
                if (!UserTool.IsLogin()) return new ContentResult { Content = SysConst.NotLogin };
                //path|datetime
                //string path = null;
                //string[] paths = str.GetDecode(pathEnc).Split('|');
                //if (Math.Abs((DateTime.Parse(paths[1]) - DateTime.Now).TotalMinutes) <= 5)
                //{
                //    path = paths[0];
                //}
                //path 权限验证
                string text = "NoTag";
                switch (tag)
                {
                    case "DicLoad": text = View.Com.DicLoad(paras); break;
                }
                return new ContentResult { Content = text };
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new ContentResult { Content = ex.Message };
            }

        }

        [HttpPost]
        public IActionResult form([FromForm] string pathEnc, [FromForm] string tag, [FromForm] string afterjs)
        {
            try
            {
                if (!Request.HasFormContentType) return new ContentResult { Content = str.ContentHTML(str.JavascriptLabel("parent._loading2fai('not form post');")), ContentType = "text/html;charset=UTF-8" };
                if (!UserTool.IsLogin()) return new ContentResult { Content = str.ContentHTML(str.JavascriptLabel("parent._loading2fai('" + SysConst.NotLogin + "');")), ContentType = "text/html;charset=UTF-8" };
                //path|datetime
                //string path = null;
                //string[] paths = str.GetDecode(pathEnc).Split('|');
                //if (Math.Abs((DateTime.Parse(paths[1]) - DateTime.Now).TotalMinutes) <= 5)
                //{
                //    path = paths[0];
                //}
                //path 权限验证
                string text = str.JavascriptLabel("parent._loading2fai('NoTag');");
                switch (tag)
                {
                    case "DicSave": text = View.Com.DicSave(HttpContext.Request); break;
                }
                return new ContentResult { Content = str.ContentHTML(text), ContentType = "text/html;charset=UTF-8" };
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new ContentResult { Content = str.ContentHTML(str.JavascriptLabel("parent._loading2fai('" + ex.Message + "');")), ContentType = "text/html;charset=UTF-8" };
            }

        }

        [HttpPost]
        public IActionResult Join([FromForm] string FlowID, [FromForm] string WorkID, [FromForm] string MainTable, [FromForm] string MainTableKey)
        {
            if (!UserTool.IsLogin()) return new ContentResult { Content = SysConst.NotLogin };
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
                        return new ContentResult { Content = "" };
                    }
                    else
                    {
                        st.Rollback();
                        return new ContentResult { Content = ret };
                    }
                }
                catch (Exception ex)
                {
                    st.Rollback();
                    log.Error(ex);
                    return new ContentResult { Content = ex.Message };
                }
            }
        }
        [HttpPost]
        public IActionResult Cancel([FromForm] string FlowID, [FromForm] string WorkID, [FromForm] string MainTable, [FromForm] string MainTableKey)
        {
            if (!UserTool.IsLogin()) return new ContentResult { Content = SysConst.NotLogin };
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
                        return new ContentResult { Content = "" };
                    }
                    else
                    {
                        st.Rollback();
                        return new ContentResult { Content = ret };
                    }
                }
                catch (Exception ex)
                {
                    st.Rollback();
                    log.Error(ex);
                    return new ContentResult { Content = ex.Message };
                }
            }
        }
        
        [HttpPost]
        public IActionResult Judge([FromForm] string FlowID, [FromForm] string WorkID, [FromForm] int judge, [FromForm] string Mimo, [FromForm] string MainTable, [FromForm] string MainTableKey)
        {
            if (!UserTool.IsLogin()) return new ContentResult { Content = SysConst.NotLogin };
            using (DB db = new DB())
            {
                ST st = db.GetTransaction();
                try
                {
                    Base.Core.Flow flow = new Base.Core.Flow(db, st, FlowID, WorkID, Project.Core.User.UserID(), MainTable, MainTableKey);
                    string ret = flow.Judge((Enums.FlowJudge)judge, Mimo);
                    if (ret == null)
                    {
                        st.Commit();
                        return new ContentResult { Content = "" };
                    }
                    else
                    {
                        st.Rollback();
                        return new ContentResult { Content = ret };
                    }
                }
                catch (Exception ex)
                {
                    st.Rollback();
                    log.Error(ex);
                    return new ContentResult { Content = ex.Message };
                }
            }
        }
    }
}
