using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FTFrame;
using FTFrame.DBClient;
using FTFrame.Tool;
using FTFrame.Base;
using FTFrame.Project;
namespace FTFrame
{
    public class Init
    {
        public static void Initialize()
        {
            BaseInit.CompanyInfo();
            AuthPageUrlsInit();
        }
        public static void AuthPageUrlsInit()
        {
            SysConst.AuthPageUrls = new ArrayList();
            DB db = new DB();
            db.Open();
            try
            {
                string sql = "select URL from TB_RESINFO where stat=1";
                DR dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    if (!dr.IsDBNull(0) && dr.GetString(0) != "" && !SysConst.AuthPageUrls.Contains(dr.GetString(0))) SysConst.AuthPageUrls.Add(dr.GetString(0));
                }
                dr.Close();
                sql = "select URL from TB_CATA where stat=1";
                dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    if (!dr.IsDBNull(0) && dr.GetString(0) != "" && !SysConst.AuthPageUrls.Contains(dr.GetString(0))) SysConst.AuthPageUrls.Add(dr.GetString(0));
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                db.Close();
            }


        }
    }
}
