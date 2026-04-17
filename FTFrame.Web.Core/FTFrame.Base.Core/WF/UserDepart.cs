using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using System.Linq;
using System.IO;
using System.Xml;
using System.Collections;
using CoreHttp = Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
namespace FTFrame.WorkFlow.Core
{
    public class UserDepart
    {
        public ArrayList LoopDepartSave;
        public ArrayList UserFromDepart(string DepartID)
        {
            if (LoopDepartSave.Contains(DepartID)) return new ArrayList();
            LoopDepartSave.Add(DepartID);
            DB db = new DB();
            db.Open();
            try
            {
                ArrayList al = new ArrayList();
                string sql = "select USERID from BD_USER_DEPART where DEPARTID='" + str.D2DD(DepartID) + "'";
                DR dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    al.Add(dr.GetString(0));
                }
                dr.Close();
                ArrayList departal = new ArrayList();
                sql = "select DEPARTID from TB_DEPARTINFO where PARENTID='" + str.D2DD(DepartID) + "'";
                dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    departal.Add(dr.GetString(0));
                }
                dr.Close();
                foreach (string departId in departal)
                {
                    ArrayList al2 = UserFromDepart(departId);
                    foreach (string userid in al2)
                    {
                        if (!al.Contains(userid)) al.Add(userid);
                    }
                    al2.Clear(); al2 = null;
                }
                return al;
            }
            catch (Exception ex) { log.Error(ex); return new ArrayList(); }
            finally { db.Close(); }
        }
    }
}
