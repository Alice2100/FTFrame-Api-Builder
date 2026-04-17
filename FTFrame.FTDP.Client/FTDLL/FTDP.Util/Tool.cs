using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTDP.Util
{
    public class Tool
    {
        public static string AppPath = Application.StartupPath;
        public static string D2DD(string s)
        {
            return s.Replace("'","''");
        }
    }
}
