using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTFrame.DBClient
{
    public class DBConst
    {
        public static DataBase DataBaseType = ConfigHelper.GetConfigValue("DataBase:DataBaseType")==null?DataBase.SqlServer: (DataBase)Enum.Parse(typeof(DataBase), ConfigHelper.GetConfigValue("DataBase:DataBaseType"));
        public static string DBConnString = ConfigHelper.GetConfigValue("DataBase:ConnectionStr");
        public static string ModelDllName = ConfigHelper.GetConfigValue("DataBase:ModelDllName") == null ? "FTFrame.Project.Model" : ConfigHelper.GetConfigValue("DataBase:ModelDllName");
    }
    public enum DataBase
    {
        SqlServer,
        MySql,
        Oracle,
        DB2,
        Sybase,
        Sqlite
    }
    public class ConfigHelper
    {
        private static IConfiguration _configuration;

        public static string GetConfigValue(string key)
        {
            if (_configuration == null)
            {
                var fileName = "appsettings.json";
                if (!File.Exists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + fileName)) return null;
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(fileName);

                _configuration = builder.Build();
            }
            return _configuration[key];
        }
    }
}
