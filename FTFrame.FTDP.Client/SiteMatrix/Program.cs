using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FTDPClient.consts;
using FTDPClient.forms;
using FTDPClient.functions;

namespace FTDPClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            #region 更新
            int version = update.version();
            var updateFile = Application.StartupPath + @"\update\" + version + @"\FTDP_Update.exe";
            if (file.Exists(updateFile))
            {
                System.Diagnostics.ProcessStartInfo cp = new System.Diagnostics.ProcessStartInfo();
                cp.FileName = updateFile;
                cp.Arguments = version.ToString();
                cp.UseShellExecute = true;
                System.Diagnostics.Process.Start(cp);
                System.Threading.Thread.Sleep(2 * 1000);
                Application.Exit();
                return;
            }
            #endregion
            Application.Run(new Loading());
        }
    }
}