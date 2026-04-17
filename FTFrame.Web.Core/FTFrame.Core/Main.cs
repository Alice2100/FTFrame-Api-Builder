using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Collections;
namespace FTFrame
{
    public class SysConst
    {
        public static string RootPath = System.AppDomain.CurrentDomain.BaseDirectory;
        public static DataBase DataBaseType = ConfigHelper.GetConfigValue("DataBase:DataBaseType") == null ? DataBase.SqlServer : (DataBase)Enum.Parse(typeof(DataBase), ConfigHelper.GetConfigValue("DataBase:DataBaseType"));
        public static string ModelDllName = ConfigHelper.GetConfigValue("DataBase:ModelDllName") == null ? "FTFrame.Project.Model" : ConfigHelper.GetConfigValue("DataBase:ModelDllName");
        public static string ConnString = ConfigHelper.GetConfigValue("DataBase:ConnectionStr");
        public static string ConnString_ReadOnly = string.IsNullOrWhiteSpace(ConfigHelper.GetConfigValue("DataBase:ConnectionStr_ReadOnly"))? ConfigHelper.GetConfigValue("DataBase:ConnectionStr"): ConfigHelper.GetConfigValue("DataBase:ConnectionStr_ReadOnly");
        public static string ConnectionStr_FTDP = string.IsNullOrWhiteSpace(ConfigHelper.GetConfigValue("DataBase:ConnectionStr_FTDP"))? ConfigHelper.GetConfigValue("DataBase:ConnectionStr"): ConfigHelper.GetConfigValue("DataBase:ConnectionStr_FTDP");
        //public static string OLEDBProvider = ConfigHelper.GetConfigValue("DataBase:OLEDBProvider");
        public static bool IsDebug =  int.Parse(ConfigHelper.GetConfigValue("LogLevel")) >= 2;
        public static Obj.Company Company = null;
        public static ArrayList AuthPageUrls = null;
        public static int ValidateShowTime = 10;
        //public static int MonitUserTimer =  int.Parse(ConfigHelper.GetConfigValue("Monit:MonitUserTimer"));
        //public static int MonitNewMessageTimer =  int.Parse(ConfigHelper.GetConfigValue("Monit:MonitNewMessageTimer"));
        //public static int MonitRemindIntervalMinute = int.Parse(ConfigHelper.GetConfigValue("Monit:MonitRemindIntervalMinute"));
        public static string BlankStr = "——";
        public const string CopyRight = " ";
        public const string CopyRightAppend = " ";
        public const string NotLogin = "Please log in first to perform this operation";
        public const string NoRight = "(No Permission)";
        public const string NoRightOP = "You do not have permission to perform this operation";
        public static string SystemName = ConfigHelper.GetConfigValue("Site:SystemName");
        public static string SystemTitle = ConfigHelper.GetConfigValue("Site:SystemName");
        public static string SubPath = ConfigHelper.GetConfigValue("Site:SubPath");
        public static string HostReferrer = ConfigHelper.GetConfigValue("Site:HostReferrer");
        public static string RoutePathReplace = ConfigHelper.GetConfigValue("Site:RoutePathReplace");
        public static string SiteBak = Path.GetFullPath(ConfigHelper.GetConfigValue("Publish:SiteBak"));
        public static string LogPath = Path.GetFullPath(ConfigHelper.GetConfigValue("Site:LogPath"));
        public static string[] OriginAllowed = ConfigHelper.GetConfigValue("Site:OriginAllowed").Split(new char[] { ','},StringSplitOptions.RemoveEmptyEntries);
        public static bool CodeGetCompile = bool.Parse(ConfigHelper.GetConfigValue("Site:CodeGetCompile"));
        public static bool SettingEncode = bool.Parse(ConfigHelper.GetConfigValue("Site:SettingEncode"));
        public static bool IsApiCompile = bool.Parse(ConfigHelper.GetConfigValue("Publish:IsApiCompile"));
        public static string DllCompilePath = ConfigHelper.GetConfigValue("Publish:DllCompilePath");
        public static string RegisterNumber = ConfigHelper.GetConfigValue("Publish:RegisterNumber");
        public static string[] RegisterSiteKeys = ConfigHelper.GetConfigValue("Publish:RegisterSiteKey").Split('|'); 
        public const string CallBackUrl = "http://www.ftframe.com";
        public const bool DevelopMode = false;
        public static readonly string NoResult = "FT{NORESULT}";
        public static readonly string NoResultValue = "";
        public static readonly int RateRowFindDeep = 12;
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
}
