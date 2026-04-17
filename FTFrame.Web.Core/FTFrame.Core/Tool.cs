using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace FTFrame
{
    /// <summary>
    /// http上下文
    /// </summary>
    public class FTHttpContext
    {
        private static IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// 当前上下文
        /// </summary>
        public static Microsoft.AspNetCore.Http.HttpContext Current => _contextAccessor.HttpContext;


        public static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
    }
    public class CacheHelper
    {
        private static Dictionary<string, PropertyInfo[]> CacheDic = null;
        private static readonly object dicAddLocker = new object();
        public static PropertyInfo[] ObjectProperties(string name, Type type = null, string dllName = null)
        {
            if (CacheDic == null) CacheDic = new Dictionary<string, PropertyInfo[]>();
            if (!CacheDic.ContainsKey(name))
            {
                lock (dicAddLocker)
                {
                    string typeName = name;
                    if (dllName != null) typeName += "," + dllName;
                    else if (name.StartsWith("FTFrame.Project.Model")) typeName += "," + SysConst.ModelDllName;
                    else if (name.StartsWith("FTFrame.Model")) typeName += ",FTFrame.Model";
                    CacheDic.Add(name, (type ?? Type.GetType(typeName)).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public));
                }
            }
            return CacheDic[name];
        }
    }
    public class ConfigHelper
    {
        private static IConfiguration _configuration;

        public static string GetConfigValue(string key)
        {
            if (_configuration == null)
            {
                var fileName = "appsettings.json";
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(fileName);

                _configuration = builder.Build();
            }
            return _configuration[key];
        }
    }

}
