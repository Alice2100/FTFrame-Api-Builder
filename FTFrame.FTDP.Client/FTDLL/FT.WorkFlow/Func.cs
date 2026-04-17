using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTFrame.Tool;
using FTFrame;
using FTFrame.DBClient;
using FTFrame.Base;
using System.Web;
using System.Collections;
namespace FT.Com.WorkFlow
{
    public class Func
    {
        /// <summary>
        /// 得到流程选择的select元素option html
        /// </summary>
        /// <returns></returns>
        public static string FlowListOptionHTML()
        {
            DB db = new DB();
            db.Open();
            try
            {
                string s = "";
                string nodicoptions = "";
                string options="";
                string nowlabel = null;
                string sql = "select a.fid,a.flowname,a.dicfid,b.name dicname from "+Const.Table.FC_WorkFlow_List+" a left join TB_DIC b on a.dicfid=b.fid and b.typeid="+Const.DicTypeID+" where a.stat=1  order by b.ORDERNUM,a.OrderRank";
                DR dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    if (dr.IsDBNull("dicname") || dr.GetString("dicname").Equals(""))
                    {
                        nodicoptions += "<option value='" + dr.GetString("fid") + "'>" + dr.GetString("flowname") + "</option>";
                    }
                    else
                    {
                        if (nowlabel != null && !nowlabel.Equals(dr.GetString("dicname")))
                        {
                            options += "</optgroup>";
                            nowlabel = null;
                        }
                        if (nowlabel == null)
                        {
                            nowlabel=dr.GetString("dicname");
                            options += "<optgroup label='"+dr.GetString("dicname")+"'>";
                        }
                        options += "<option value='" + dr.GetString("fid") + "'>" + dr.GetString("flowname") + "</option>";
                    }
                }
                dr.Close();
                if (nowlabel != null)
                {
                    options += "</optgroup>";
                }
                if (options.Equals("")) s = nodicoptions;
                else s = options + "<optgroup label='未分类'>" + nodicoptions + "</optgroup>";
                return s;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
            finally
            {
                db.Close();
            }
        }
    }
}
