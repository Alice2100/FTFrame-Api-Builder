using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FTFrame.Dynamic.Core
{
    public class Code
    {
        public static object Get(string code, string[] para, HttpContext Context)
        {
            if (string.Compare(code, "$a1.b1", StringComparison.Ordinal) == 0)
            {
                if (code == "b1") return "111";
            }
            return "(nocode)";
        }
        private static string[] TagCode(string code)
        {
            int index = code.IndexOf('.');
            if (index < 0) return null;
            string tag = code.Substring(0, index);
            code = code.Substring(index + 1);
            return new string[] { tag, code };
        }
    }
}
