using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FTFrame;
using System.Web;
using System.Threading.Tasks;

namespace FTFrame.Tool
{
    public class log
    {
        private static async Task AppendFileAsync(string path, string message)
        {
            using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 4096, useAsync: true))
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
                stream.Flush();
            }
            //await File.AppendAllTextAsync(path, message, Encoding.Default);
        }
        private static void AppendFile(string path, string message)
        {
            File.AppendAllText(path, message, Encoding.Default);
        }
        public static void Debug(string str)
        {
            Debug(str, "");
        }
        public static void DebugDirect(string str)
        {
            try
            {
                if (SysConst.IsDebug)
                {
                    string BaseDir = SysConst.LogPath;
                    if (!Directory.Exists(BaseDir))
                    {
                        Directory.CreateDirectory(BaseDir);
                    }
                    string path = BaseDir + Path.DirectorySeparatorChar + @"ftflog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".ftlog";
                    if (!File.Exists(path))
                    {
                        using (FileStream fs = File.Create(path)) { }
                    }
                    FileInfo fi = new FileInfo(path);
                    StreamWriter sw = fi.AppendText();
                    sw.WriteLine("[FTFrame.DebugDirect][" + DateTime.Now + "]"+ str);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch { }
        }
        public static void Debug(string str, string other)
        {
            try
            {
                if (SysConst.IsDebug)
                {
                    string BaseDir = SysConst.LogPath;
                    if (!Directory.Exists(BaseDir))
                    {
                        Directory.CreateDirectory(BaseDir);
                    }
                    string path = BaseDir + Path.DirectorySeparatorChar + @"ftflog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".ftlog";
                    AppendFileAsync(path, "[FTFrame.Debug][" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + other + ":" + str + Environment.NewLine).Start();
                    //if (!File.Exists(path))
                    //{
                    //    using (FileStream fs = File.Create(path)) { }
                    //}
                    //FileInfo fi = new FileInfo(path);
                    //StreamWriter sw = fi.AppendText();
                    //sw.WriteLine("[FTFrame.Debug][" + DateTime.Now + "]" + other + ":" + str);
                    //sw.Flush();
                    //sw.Close();
                }
            }
            catch { }
        }
        public static void DebugMonth(string str)
        {
            DebugMonth(str, "");
        }
        public static void DebugMonth(string str, string other)
        {
            try
            {
                if (SysConst.IsDebug)
                {
                    string BaseDir = SysConst.LogPath;
                    if (!Directory.Exists(BaseDir))
                    {
                        Directory.CreateDirectory(BaseDir);
                    }
                    string path = BaseDir + Path.DirectorySeparatorChar+@"ftflog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + ".ftlog";
                    AppendFileAsync(path, "[FTFrame.Debug][" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + other + ":" + str + Environment.NewLine).Start();
                    //if (!File.Exists(path))
                    //{
                    //    using (FileStream fs = File.Create(path)) { }
                    //}
                    //FileInfo fi = new FileInfo(path);
                    //StreamWriter sw = fi.AppendText();
                    //sw.WriteLine("[FTFrame.Debug][" + DateTime.Now + "]" + other + ":" + str);
                    //sw.Flush();
                    //sw.Close();
                }
            }
            catch { }
        }
        public static void Error(Exception ex)
        {
            Error(ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite.ToString(), "Exception");
        }
        public static void Error(string str)
        {
            Error(str, "");
        }
        public static void Error(string str, string other)
        {
            try
            {
                string BaseDir = SysConst.LogPath;
                if (!Directory.Exists(BaseDir))
                {
                    Directory.CreateDirectory(BaseDir);
                }
                string path = BaseDir + Path.DirectorySeparatorChar + @"error_" + DateTime.Now.Year + "_" + DateTime.Now.Month + ".ftlog";
                AppendFile(path, "[FTFrame.Error][" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + other + ":" + str + Environment.NewLine);
                path = BaseDir + Path.DirectorySeparatorChar + @"ftflog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".ftlog";
                AppendFileAsync(path, "[FTFrame.Error][" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + other + ":" + str + Environment.NewLine).Start();
                //if (!File.Exists(path))
                //{
                //    using (FileStream fs = File.Create(path)) { }
                //}
                //FileInfo fi = new FileInfo(path);
                //StreamWriter sw = fi.AppendText();
                //sw.WriteLine("[FTFrame.Error][" + DateTime.Now + "]" + other + ":" + str);
                //sw.Flush();
                //sw.Close();
                //if (!File.Exists(path))
                //{
                //    using (FileStream fs = File.Create(path)) { }
                //}
                //fi = new FileInfo(path);
                //sw = fi.AppendText();
                //sw.WriteLine("[FTFrame.Error][" + DateTime.Now + "]" + other + ":" + str);
                //sw.Flush();
                //sw.Close();
            }
            catch { }
        }
    }
}
