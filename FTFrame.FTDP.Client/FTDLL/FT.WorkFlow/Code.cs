using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTFrame.Tool;
using FTFrame;
using FTFrame.DBClient;
using FTFrame.Base;
using System.Web;
using System.Collections;
namespace FT.Com.WorkFlow
{
    public class Code
    {
        public static object Get(string Tag,string Code,string[] Para, HttpContext Context)
        {
            try
            {
                switch (Tag)
                {
                    case "op": return _op(Code, Para, Context);
                }
                return "(nocode)";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
        private static string _op(string Code, string[] Para, HttpContext Context)
        {
            switch (Code)
            {
                case "AfterAdd": return Busi.AfterAdd(Para[0], Context);
                case "AfterMod": return Busi.AfterMod(Para[0], Context);
                case "FlowGet": return Busi.FlowGet(Para[0], Context);
            }
            return "(nocode)";
        }
    }
}
