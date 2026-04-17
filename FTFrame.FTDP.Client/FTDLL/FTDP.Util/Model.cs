using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTDP.Util.Model
{
    public class Bug
    {
        public string LeiXing { get; set; }
        public string Path { get; set; }
        public string PageCaption { get; set; }
        public string ControlCaption { get; set; }
        public string BugDesc { get; set; }
        public string Level { get; set; }
        public string OpenFile { get; set; }
        public string PageID { get; set; }
    }
    public class Find
    {
        public string LeiXing { get; set; }
        public string Position { get; set; }
        public string Path { get; set; }
        public string PageCaption { get; set; }
        public string ControlCaption { get; set; }
        public string Desc { get; set; }
        public string OpenFile { get; set; }
        public string PageID { get; set; }
        public string SetValue { get; set; }
        public string SetKey { get; set; } = null;
        public (string partname, string controlname, string partid, string partxml, string controlcaption) PartObj { get; set; }
    }
}
