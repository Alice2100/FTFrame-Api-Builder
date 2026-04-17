using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FTFrame.DBClient
{
    public class CacheHelper
    {
        private static Dictionary<string, PropertyInfo[]> CacheDic = null;
        private static readonly object dicAddLocker = new object();
        public static PropertyInfo[] ObjectProperties(string name)
        {
            if (CacheDic == null) CacheDic = new Dictionary<string, PropertyInfo[]>();
            if (!CacheDic.ContainsKey(name))
            {
                lock (dicAddLocker)
                {
                    string typeName = name;
                    if (name.StartsWith("FTFrame.Model")) typeName += ",FTFrame.Model";
                    else if (name.StartsWith("System.")) { }
                    else typeName += "," + DBConst.ModelDllName;
                    CacheDic.Add(name, Type.GetType(typeName).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public));
                }
            }
            return CacheDic[name];
        }
    }
}
