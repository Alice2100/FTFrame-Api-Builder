using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTFrame.Tool;
using FTFrame;
using FTFrame.DBClient;
using FTFrame.Base;
using FTFrame.Project;
using System.Web;
using System.Collections;
using System.IO;
using System.Data;
using System.Data.OleDb;
namespace FTFrame.Project
{
    public class Import
    {
        public static string Start(string type, HttpContext context)
        {
            switch (type)
            {
            }
            return "No Import Type";
        }
        private static DataSet ExcelDS(string filenameurl, string sheet)
        {

            string strConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filenameurl + ";Extended Properties='Excel 12.0;HDR=YES; IMEX=1'";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataSet ds = new DataSet();
            try
            {
                OleDbDataAdapter odda = new OleDbDataAdapter("select * from [" + sheet + "$]", conn);
                odda.Fill(ds);
                odda.Dispose();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                conn.Close();
            }
            return ds;
        }
        private static string SaveFile(HttpPostedFile file)
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\_ftfiles\\import"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\_ftfiles\\import");
            }
            string filename = file.FileName.Replace("\\", "/");
            if (filename.LastIndexOf('/') >= 0)
            {
                filename = filename.Substring(filename.LastIndexOf('/') + 1);
            }
            filename = AppDomain.CurrentDomain.BaseDirectory + "\\_ftfiles\\import\\" + str.GetCombID() + "_" + filename;
            file.SaveAs(filename);
            return filename;
        }
    }
}
