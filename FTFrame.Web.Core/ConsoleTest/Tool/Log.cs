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
            await File.AppendAllTextAsync(path, message, Encoding.Default);
        }
        private static void AppendFile(string path, string message)
        {
            File.AppendAllText(path, message, Encoding.Default);
        }
        public static void Debug(string str)
        {
            Debug(str, "");
        }
        public static void Debug(string str, string other)
        {
            try
            {
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
                
            }
            catch { }
        }
    }
}
