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
using Microsoft.AspNetCore.Http;

namespace FTFrame.Base.Core
{
    public class View
    {
        public enum UserSetList
        {
            CP1, CP2, CP3, CP4, CP5, CP6, CP7, CP8, CP9, CP10, CP11, CP12, FavorStyle
        }
        public class Frame
        {
            public static string IconUrl()
            {
                string s = "../_pro/image/logo.png";
                return s;
            }
            public static string _LeftTree()
            {
                string s = "";
                try
                {
                    using (DB db = new DB())
                    {
                        db.Open();
                        string sql = "select * from TB_CATA where (ParentID is null or ParentID='' or ParentID='0') and STAT=1 order by `Rank`";
                        ArrayList al = new ArrayList();
                        ArrayList tempal = new ArrayList();
                        using (DR dr = new DR(db, sql))
                        {
                            while (dr.Read())
                            {
                                string CataId = dr.GetString("CATAID");
                                string CateName = dr.GetStringNoNULL("CATANAME");
                                bool IsFolder = dr.GetInt16("ISFOLDER") == 1;
                                string Url = dr.GetStringNoNULL("URL");
                                string Icon = dr.GetStringNoNULL("Icon");
                                al.Add(new object[] { CataId, CateName, IsFolder, Url, Icon });
                            }
                        }
                        foreach (object[] item in al)
                        {
                            string CataId = (string)item[0];
                            string CateName = (string)item[1];
                            bool IsFolder = (bool)item[2];
                            string Url = (string)item[3];
                            string Icon = (string)item[4];
                            string iconOpen = null; string iconClose = null;
                            if (IsFolder)
                            {
                                iconOpen = Icon == "" ? "../_pro/js/pub/img/cate2.gif" : Icon;
                                iconClose = Icon == "" ? "../_pro/js/pub/img/cate.gif" : Icon;
                            }
                            else
                            {
                                iconOpen = Icon == "" ? "../_pro/js/pub/img/page.gif" : Icon;
                                iconClose = Icon == "" ? "../_pro/js/pub/img/page.gif" : Icon;
                            }
                            string linkAttr = "";
                            if (Url != "") linkAttr = " onclick=\"addTab('" + CateName + "','" + Url + "')\"";
                            s += "<li icon1=\"" + iconOpen + "\" icon2=\"" + iconClose + "\"><a href=\"javascript:void(0)\"" + linkAttr + ">" + CateName + "</a>";
                            string ss = _LeftTreeLoop(db, CataId, tempal);
                            if (!ss.Equals("")) s += "<ul>" + ss + "</ul>";
                            s += "</li>";
                        }
                        al.Clear();
                        al = null;
                        tempal.Clear();
                        tempal = null;
                        s += "<li icon1=\"../_pro/js/pub/img/cate2_sys.gif\" icon2=\"../_pro/js/pub/img/cate_sys.gif\"><a href=\"javascript:void(0)\">系统</a>";
                        string ss2 = _LeftTreeSysLoop(db, "01");
                        if (!ss2.Equals("")) s += "<ul>" + ss2 + "</ul>";
                        s += "</li>";
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                return s;
            }
            private static string _LeftTreeLoop(DB db, string ParentID, ArrayList tempal)
            {
                if (tempal.Contains(ParentID)) return "";//防止死循环
                tempal.Add(ParentID);
                string s = "";
                string sql = "select * from TB_CATA where ParentID='" + str.D2DD(ParentID) + "' and STAT=1 order by `Rank`";
                ArrayList al = new ArrayList();
                using (DR dr = new DR(db, sql))
                {
                    while (dr.Read())
                    {
                        string CataId = dr.GetString("CATAID");
                        string CateName = dr.GetStringNoNULL("CATANAME");
                        bool IsFolder = dr.GetInt16("ISFOLDER") == 1;
                        string Url = dr.GetStringNoNULL("URL");
                        string Icon = dr.GetStringNoNULL("Icon");
                        al.Add(new object[] { CataId, CateName, IsFolder, Url, Icon });
                    }
                }
                foreach (object[] item in al)
                {
                    string CataId = (string)item[0];
                    string CateName = (string)item[1];
                    bool IsFolder = (bool)item[2];
                    string Url = (string)item[3];
                    string Icon = (string)item[4];
                    string iconOpen = null; string iconClose = null;
                    if (IsFolder)
                    {
                        iconOpen = Icon == "" ? "../_pro/js/pub/img/cate2.gif" : Icon;
                        iconClose = Icon == "" ? "../_pro/js/pub/img/cate.gif" : Icon;
                    }
                    else
                    {
                        iconOpen = Icon == "" ? "../_pro/js/pub/img/page.gif" : Icon;
                        iconClose = Icon == "" ? "../_pro/js/pub/img/page.gif" : Icon;
                    }
                    string linkAttr = "";
                    if (Url != "") linkAttr = " onclick=\"addTab('" + CateName + "','" + Url + "')\"";
                    s += "<li icon1=\"" + iconOpen + "\" icon2=\"" + iconClose + "\"><a href=\"javascript:void(0)\"" + linkAttr + ">" + CateName + "</a>";
                    string ss = _LeftTreeLoop(db, CataId, tempal);
                    if (!ss.Equals("")) s += "<ul>" + ss + "</ul>";
                    s += "</li>";
                }
                al.Clear();
                al = null;
                return s;
            }
            private static string _LeftTreeSysLoop(DB db, string ParentID)
            {
                string restr = "";
                string sql = "select RESID,RESNAME,RESTYPE,ISFOLDER,URL from TB_RESINFO where PARENTID='" + str.D2DD(ParentID) + "' and STAT=1 order by `Rank`";
                ArrayList Al = new ArrayList();
                using (DR dr = new DR(db, sql))
                {
                    while (dr.Read())
                    {
                        Al.Add(new object[]{
                        dr.GetString("RESID"),
                        dr.GetStringNoNULL("RESNAME"),
                        dr.GetInt16("RESTYPE"),
                        dr.GetInt16("ISFOLDER"),
                        dr.GetStringNoNULL("URL")
                    });
                    }
                }
                foreach (object[] item in Al)
                {
                    string CateName = (string)item[1];
                    if ((int)item[3] == 1)
                    {
                        restr += "<li icon1=\"../_pro/js/pub/img/cate2_sys.gif\" icon2=\"../_pro/js/pub/img/cate_sys.gif\"><a href=\"javascript:void(0)\">" + CateName + "</a>";
                        string ss = _LeftTreeSysLoop(db, (string)item[0]);
                        if (!ss.Equals("")) restr += "<ul>" + ss + "</ul>";
                        restr += "</li>";
                    }
                    else if ((int)item[2] == 0)
                    {

                        restr += "<li icon1=\"../_pro/js/pub/img/page_sys.gif\" icon2=\"../_pro/js/pub/img/page_sys.gif\"><a href=\"javascript:void(0)\" onclick=\"addTab('" + CateName + "','" + item[4] + "')\">" + CateName + "</a></li>";
                    }
                }
                Al.Clear();
                Al = null;
                return restr;
            }
            public static string LeftTree()
            {
                string s = "";
                try
                {
                    using (DB db = new DB())
                    {
                        db.Open();

                        string sql = null;
                        bool IsAdmin = UserTool.IsAdmin();
                        string UserID = Tool.UserTool.CurUserID();
                        if (!IsAdmin && UserID == null) return "";
                        ArrayList AuthPageUrl = new ArrayList();
                        if (!IsAdmin)
                        {
                            sql = "select a.CATAID,a.CATANAME,a.RESTYPE,a.URL,a.OPID";
                            sql += "  from TB_CATA a,BD_RES_ROLE b,TB_USERINFO c,BD_USER_ROLE d";
                            sql += "   where a.STAT=1 and c.STAT=1 and a.CATAID=b.RESID and b.ROLEID=d.ROLEID and c.USERID=d.USERID";
                            sql += "    and c.USERID='" + str.D2DD(UserID) + "' and a.ISFOLDER=0";
                            sql += " 	union all ";
                            sql += " 	select a.RESID CATAID,a.RESNAME CATANAME,a.RESTYPE,a.URL,a.OPID ";
                            sql += "  from TB_RESINFO a,BD_RES_ROLE b,TB_USERINFO c,BD_USER_ROLE d ";
                            sql += "   where a.STAT=1 and c.STAT=1 and a.RESID=b.RESID and b.ROLEID=d.ROLEID and c.USERID=d.USERID ";
                            sql += "     and c.USERID='" + str.D2DD(UserID) + "' and a.ISFOLDER=0 ";
                            using (DR dr = new DR(db, sql))
                            {
                                while (dr.Read())
                                {
                                    int ResType = dr.IsDBNull("RESTYPE") ? 0 : dr.GetInt16("RESTYPE");
                                    if (ResType == 0 || ResType == 2)
                                    {
                                        AuthPageUrl.Add(dr.IsDBNull("URL") ? "[NULL]" : dr.GetString("URL").ToLower());
                                    }
                                }
                            }
                        }

                        sql = "select * from TB_CATA where (ParentID is null or ParentID='' or ParentID='0') and STAT=1 order by `Rank`";
                        ArrayList al = new ArrayList();
                        ArrayList tempal = new ArrayList();
                        using (DR dr = new DR(db, sql))
                        {
                            while (dr.Read())
                            {
                                string CataId = dr.GetString("CATAID");
                                string CateName = dr.GetStringNoNULL("CATANAME");
                                bool IsFolder = dr.GetInt16("ISFOLDER") == 1;
                                int ResType = dr.IsDBNull("RESTYPE") ? 0 : dr.GetInt16("RESTYPE");
                                bool NoMenu = !dr.IsDBNull("NoMenu") && dr.GetInt16("NoMenu") == 1;
                                string Url = dr.GetStringNoNULL("URL");
                                string Icon = dr.GetStringNoNULL("Icon");
                                al.Add(new object[] { CataId, CateName, IsFolder, Url, Icon, ResType, NoMenu });
                            }
                        }
                        foreach (object[] item in al)
                        {
                            string CataId = (string)item[0];
                            string CateName = (string)item[1];
                            bool IsFolder = (bool)item[2];
                            string Url = (string)item[3];
                            string Icon = (string)item[4];
                            int ResType = (int)item[5];
                            if (ResType == 1) continue;//OpId
                            bool IsNoMenu = (bool)item[6];
                            if (IsNoMenu) continue;
                            string iconOpen = null; string iconClose = null;
                            if (IsFolder)
                            {
                                iconOpen = Icon == "" ? "../_pro/js/pub/img/cate2.gif" : Icon;
                                iconClose = Icon == "" ? "../_pro/js/pub/img/cate.gif" : Icon;
                            }
                            else
                            {
                                iconOpen = Icon == "" ? "../_pro/js/pub/img/page.gif" : Icon;
                                iconClose = Icon == "" ? "../_pro/js/pub/img/page.gif" : Icon;
                            }
                            int rightok = 0;
                            if (IsAdmin || AuthPageUrl.Contains(Url.Trim().ToLower())) rightok = 1;
                            string linkAttr = "";
                            if (Url != "") linkAttr = " onclick=\"addTab('" + CateName + "','" + Url + "')\"";
                            s += "<li icon1=\"" + iconOpen + "\" icon2=\"" + iconClose + "\" rightok=\"" + rightok + "\"><a href=\"javascript:void(0)\"" + linkAttr + ">" + CateName + "</a>";
                            string ss = LeftTreeLoop(db, CataId, tempal, IsAdmin, AuthPageUrl);
                            if (!ss.Equals("")) s += "<ul>" + ss + "</ul>";
                            s += "</li>";
                        }
                        al.Clear();
                        al = null;
                        tempal.Clear();
                        tempal = null;
                        s += "<li icon1=\"../_pro/js/pub/img/cate2_sys.gif\" icon2=\"../_pro/js/pub/img/cate_sys.gif\"><a href=\"javascript:void(0)\">系统</a>";
                        string ss2 = LeftTreeSysLoop(db, "01", IsAdmin, AuthPageUrl);
                        if (!ss2.Equals("")) s += "<ul>" + ss2 + "</ul>";
                        s += "</li>";
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                return s;
            }
            private static string LeftTreeLoop(DB db, string ParentID, ArrayList tempal, bool IsAdmin, ArrayList AuthPageUrl)
            {
                if (tempal.Contains(ParentID)) return "";//防止死循环
                tempal.Add(ParentID);
                string s = "";
                string sql = "select * from TB_CATA where ParentID='" + str.D2DD(ParentID) + "' and STAT=1 order by `Rank`";
                ArrayList al = new ArrayList();
                using (DR dr = new DR(db, sql))
                {
                    while (dr.Read())
                    {
                        string CataId = dr.GetString("CATAID");
                        string CateName = dr.GetStringNoNULL("CATANAME");
                        bool IsFolder = dr.GetInt16("ISFOLDER") == 1;
                        int ResType = dr.IsDBNull("RESTYPE") ? 0 : dr.GetInt16("RESTYPE");
                        bool NoMenu = !dr.IsDBNull("NoMenu") && dr.GetInt16("NoMenu") == 1;
                        string Url = dr.GetStringNoNULL("URL");
                        string Icon = dr.GetStringNoNULL("Icon");
                        al.Add(new object[] { CataId, CateName, IsFolder, Url, Icon, ResType, NoMenu });
                    }
                }
                foreach (object[] item in al)
                {
                    string CataId = (string)item[0];
                    string CateName = (string)item[1];
                    bool IsFolder = (bool)item[2];
                    string Url = (string)item[3];
                    string Icon = (string)item[4];
                    int ResType = (int)item[5];
                    if (ResType == 1) continue;//OpId
                    bool IsNoMenu = (bool)item[6];
                    if (IsNoMenu) continue;
                    string iconOpen = null; string iconClose = null;
                    if (IsFolder)
                    {
                        iconOpen = Icon == "" ? "../_pro/js/pub/img/cate2.gif" : Icon;
                        iconClose = Icon == "" ? "../_pro/js/pub/img/cate.gif" : Icon;
                    }
                    else
                    {
                        iconOpen = Icon == "" ? "../_pro/js/pub/img/page.gif" : Icon;
                        iconClose = Icon == "" ? "../_pro/js/pub/img/page.gif" : Icon;
                    }
                    int rightok = 0;
                    if (IsAdmin || AuthPageUrl.Contains(Url.Trim().ToLower())) rightok = 1;
                    string linkAttr = "";
                    if (Url != "") linkAttr = " onclick=\"addTab('" + CateName + "','" + Url + "')\"";
                    s += "<li icon1=\"" + iconOpen + "\" icon2=\"" + iconClose + "\" rightok=\"" + rightok + "\"><a href=\"javascript:void(0)\"" + linkAttr + ">" + CateName + "</a>";
                    string ss = LeftTreeLoop(db, CataId, tempal, IsAdmin, AuthPageUrl);
                    if (!ss.Equals("")) s += "<ul>" + ss + "</ul>";
                    s += "</li>";
                }
                al.Clear();
                al = null;
                return s;
            }
            private static string LeftTreeSysLoop(DB db, string ParentID, bool IsAdmin, ArrayList AuthPageUrl)
            {
                string restr = "";
                string sql = "select RESID,RESNAME,RESTYPE,ISFOLDER,URL from TB_RESINFO where PARENTID='" + str.D2DD(ParentID) + "' and STAT=1 order by `Rank`";
                ArrayList Al = new ArrayList();
                using (DR dr = new DR(db, sql))
                {
                    while (dr.Read())
                    {
                        Al.Add(new object[]{
                        dr.GetString("RESID"),
                        dr.GetStringNoNULL("RESNAME"),
                        dr.GetInt16("RESTYPE"),
                        dr.GetInt16("ISFOLDER"),
                        dr.GetStringNoNULL("URL")
                    });
                    }
                }
                foreach (object[] item in Al)
                {
                    string CateName = (string)item[1];
                    int rightok = 0;
                    if (IsAdmin || AuthPageUrl.Contains(item[4].ToString().Trim().ToLower())) rightok = 1;
                    if ((int)item[3] == 1)
                    {
                        restr += "<li icon1=\"../_pro/js/pub/img/cate2_sys.gif\" icon2=\"../_pro/js/pub/img/cate_sys.gif\" rightok=\"" + rightok + "\"><a href=\"javascript:void(0)\">" + CateName + "</a>";
                        string ss = LeftTreeSysLoop(db, (string)item[0], IsAdmin, AuthPageUrl);
                        if (!ss.Equals("")) restr += "<ul>" + ss + "</ul>";
                        restr += "</li>";
                    }
                    else if ((int)item[2] == 0)
                    {
                        restr += "<li icon1=\"../_pro/js/pub/img/page_sys.gif\" icon2=\"../_pro/js/pub/img/page_sys.gif\" rightok=\"" + rightok + "\"><a href=\"javascript:void(0)\" onclick=\"addTab('" + CateName + "','" + item[4] + "')\">" + CateName + "</a></li>";
                    }
                }
                Al.Clear();
                Al = null;
                return restr;
            }
            public static string MenuTree()
            {
                string UserID = UserTool.CurUserID();
                if (UserID == null) return SysConst.NotLogin;
                string s = "";
                try
                {
                    using (DB db = new DB())
                    {
                        db.Open();
                        string sql = "select MenuID from TB_SET_MENU where UserID='" + str.D2DD(UserID) + "'";
                        ArrayList SetMenuAl = new ArrayList();
                        using (DR dr = new DR(db, sql))
                        {
                            while (dr.Read())
                            {
                                SetMenuAl.Add(dr.GetString(0));
                            }
                        }
                        ArrayList tempAl = new ArrayList();
                        s += MenuTreeLoop(db, null, SetMenuAl, tempAl);
                        tempAl.Clear(); tempAl = null;
                        s += ",{ id: '0_01', pId: '0_0', name: '系统', appid:null,open: false, icon: '/_pro/js/pub/img/cate_sys.gif',iconOpen: '/_pro/js/pub/img/cate2_sys.gif', iconClose: '/_pro/js/pub/img/cate_sys.gif'}";
                        s += MenuTreeSysLoop(db, "0_01", SetMenuAl);
                        SetMenuAl.Clear();
                        SetMenuAl = null;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                if (s.StartsWith(",")) s = "[" + s.Substring(1) + "]";
                else s = "[]";
                return s;
            }
            private static string MenuTreeSysLoop(DB db, string ParentID, ArrayList SetMenuAl)
            {
                string restr = "";
                string sql = "select RESID,RESNAME,RESTYPE,ISFOLDER,URL from TB_RESINFO where PARENTID='" + str.D2DD(ParentID.Substring(2)) + "' and STAT=1 order by `Rank`";
                ArrayList Al = new ArrayList();
                using (DR dr = new DR(db, sql))
                {
                    while (dr.Read())
                    {
                        Al.Add(new object[]{
                        dr.GetString("RESID"),
                        dr.GetStringNoNULL("RESNAME"),
                        dr.GetInt16("RESTYPE"),
                        dr.GetInt16("ISFOLDER"),
                        dr.GetStringNoNULL("URL")
                    });
                    }
                }
                foreach (object[] item in Al)
                {
                    string CateName = (string)item[1];
                    if ((int)item[3] == 1)
                    {
                        restr += ",{ id: '0_" + item[0] + "', pId: '" + ParentID + "', name: '" + CateName + "', appid:null,open: false, icon: '/_pro/js/pub/img/cate_sys.gif',iconOpen: '/_pro/js/pub/img/cate2_sys.gif', iconClose: '/_pro/js/pub/img/cate_sys.gif'}";
                        restr += MenuTreeSysLoop(db, "0_" + item[0], SetMenuAl);
                    }
                    else if ((int)item[2] == 0)
                    {
                        if (SetMenuAl.Contains(item[0].ToString())) CateName = "<font color=red>" + CateName + "</font>";
                        restr += ",{ id: '6_" + item[0] + "', pId: '" + ParentID + "', name: '" + CateName + "',urltag:'" + item[4] + "', appid:null,open: false, icon: '/_pro/js/pub/img/page_sys.gif',iconOpen: '/_pro/js/pub/img/page_sys.gif', iconClose: '/_pro/js/pub/img/page_sys.gif'}";
                    }
                }
                Al.Clear();
                Al = null;
                return restr;
            }
            private static string MenuTreeLoop(DB db, string ParentID, ArrayList SetMenuAl, ArrayList tempAl)
            {
                if (tempAl.Contains(ParentID)) return "";
                else tempAl.Add(ParentID);
                string s = "";
                string sql = null;
                if (ParentID == null)
                    sql = "select * from TB_CATA where (ParentID is null or ParentID='' or ParentID='0') and STAT=1 order by `Rank`";
                else
                    sql = "select * from TB_CATA where ParentID='" + str.D2DD(ParentID.Substring(2)) + "' and STAT=1 order by `Rank`";
                ArrayList al = new ArrayList();
                using (DR dr = new DR(db, sql))
                {
                    while (dr.Read())
                    {
                        string CataId = dr.GetString("CATAID");
                        string CateName = dr.GetStringNoNULL("CATANAME");
                        bool IsFolder = dr.GetInt16("ISFOLDER") == 1;
                        string Url = dr.GetStringNoNULL("URL");
                        string Icon = dr.GetStringNoNULL("Icon");
                        al.Add(new object[] { CataId, CateName, IsFolder, Url, Icon });
                    }
                }
                foreach (object[] item in al)
                {
                    string CataId = (string)item[0];
                    string CateName = (string)item[1];
                    bool IsFolder = (bool)item[2];
                    string Url = (string)item[3];
                    string Icon = (string)item[4];
                    string iconOpen = null; string iconClose = null;
                    if (IsFolder)
                    {
                        iconOpen = Icon == "" ? "../../_pro/js/pub/img/cate2.gif" : Icon;
                        iconClose = Icon == "" ? "../../_pro/js/pub/img/cate.gif" : Icon;
                    }
                    else
                    {
                        iconOpen = Icon == "" ? "../../_pro/js/pub/img/page.gif" : Icon;
                        iconClose = Icon == "" ? "../../_pro/js/pub/img/page.gif" : Icon;
                    }
                    if (IsFolder)
                    {
                        s += ",{ id: '0_" + CataId + "', pId: '" + ParentID + "', name: '" + CateName + "', appid:null,open: false, icon: '" + iconClose + "',iconOpen: '" + iconOpen + "', iconClose: '" + iconClose + "'}";
                        s += MenuTreeLoop(db, "0_" + CataId, SetMenuAl, tempAl);
                    }
                    else
                    {
                        if (SetMenuAl.Contains(CataId)) CateName = "<font color=red>" + CateName + "</font>";
                        s += ",{ id: '3_" + CataId + "', pId: '" + ParentID + "', name: '" + CateName + "',urltag:'" + Url + "', appid:null,open: false, icon: '" + iconClose + "',iconOpen: '" + iconOpen + "', iconClose: '" + iconClose + "'}";
                    }
                }
                al.Clear();
                al = null;
                return s;
            }
            public static void RoleResTree(HttpResponse Res, string RoleID)
            {
                Res.WriteAsync("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                Res.WriteAsync("<tree id=\"0\">");
                Res.WriteAsync("<item text=\"" + SysConst.Company.SystemName + "\" id=\"1_0_0\" open=\"1\" im0=\"res_tombs.gif\" im1=\"res_tombs.gif\" im2=\"res_tombs.gif\">");
                try
                {
                    using (DB db = new DB())
                    {
                        db.Open();
                        string sql = "select RESID from BD_RES_ROLE where ROLEID='" + str.D2DD(RoleID) + "'";
                        ArrayList ResAl = new ArrayList();
                        using (DR dr = new DR(db, sql))
                        {
                            while (dr.Read())
                            {
                                ResAl.Add(dr.GetString(0));
                            }
                        }
                        ArrayList tempAl = new ArrayList();
                        Res.WriteAsync(RoleResTreeLoop(db, null, ResAl, tempAl));
                        tempAl.Clear(); tempAl = null;
                        Res.WriteAsync("<item text=\"系统\" id=\"1_1_01\" im0=\"admin_folderClosed.gif\" im1=\"admin_folderOpen.gif\" im2=\"admin_folderClosed.gif\">");
                        Res.WriteAsync(RoleResTreeSysLoop(db, "01", ResAl));
                        Res.WriteAsync("</item>");
                        ResAl.Clear();
                        ResAl = null;
                    }
                    Res.WriteAsync("</item></tree>");
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            private static string RoleResTreeSysLoop(DB db, string ParentID, ArrayList ResAl)
            {
                string restr = "";
                string sql = "select RESID,RESNAME,RESTYPE,ISFOLDER,URL from TB_RESINFO where PARENTID='" + str.D2DD(ParentID) + "' and STAT=1 order by `Rank`";
                ArrayList Al = new ArrayList();
                using (DR dr = new DR(db, sql))
                {
                    while (dr.Read())
                    {
                        Al.Add(new object[]{
                        dr.GetString("RESID"),
                        dr.GetStringNoNULL("RESNAME"),
                        dr.GetInt16("RESTYPE"),
                        dr.GetInt16("ISFOLDER"),
                        dr.GetStringNoNULL("URL")
                    });
                    }
                }
                foreach (object[] item in Al)
                {
                    string CateName = (string)item[1];
                    if ((int)item[3] == 1)
                    {
                        restr += "<item text=\"" + item[1] + "\" id=\"1_1_" + item[0] + "\" im0=\"admin_folderClosed.gif\" im1=\"admin_folderOpen.gif\" im2=\"admin_folderClosed.gif\">";
                        restr += RoleResTreeSysLoop(db, item[0].ToString(), ResAl);
                        restr += "</item>";
                    }
                    else if ((int)item[2] == 0)
                    {
                        restr += "<item text=\"" + item[1] + "\" id=\"0_0_" + item[0] + "\" " + (!ResAl.Contains(item[0].ToString()) ? "" : "checked=\"1\" ") + " im0=\"admin_page.gif\" im1=\"admin_page.gif\" im2=\"admin_page.gif\">";
                        restr += "</item>";
                    }
                    else if ((int)item[2] == 1)
                    {
                        restr += "<item text=\"" + item[1] + "\" id=\"0_1_" + item[0] + "\" " + (!ResAl.Contains(item[0].ToString()) ? "" : "checked=\"1\" ") + " im0=\"admin_item.gif\" im1=\"admin_item.gif\" im2=\"admin_item.gif\">";
                        restr += "</item>";
                    }
                }
                Al.Clear();
                Al = null;
                return restr;
            }
            private static string RoleResTreeLoop(DB db, string ParentID, ArrayList ResAl, ArrayList tempAl)
            {
                if (tempAl.Contains(ParentID)) return "";
                else tempAl.Add(ParentID);
                string s = "";
                string sql = null;
                if (ParentID == null)
                    sql = "select CATAID,CATANAME,ISFOLDER from TB_CATA where (ParentID is null or ParentID='' or ParentID='0') and STAT=1 order by `Rank`";
                else
                    sql = "select CATAID,CATANAME,ISFOLDER from TB_CATA where ParentID='" + str.D2DD(ParentID) + "' and STAT=1 order by `Rank`";
                ArrayList al = new ArrayList();
                using (DR dr = new DR(db, sql))
                {
                    while (dr.Read())
                    {
                        al.Add(new object[] { dr.GetString(0), dr.GetStringNoNULL(1), dr.GetInt16(2) });
                    }
                }
                foreach (object[] item in al)
                {
                    if ((int)item[2] == 1)
                    {
                        s += "<item text=\"" + item[1] + "\" open=\"1\" id=\"1_2_" + item[0] + "\" im0=\"res_folderClosed.gif\" im1=\"res_folderOpen.gif\" im2=\"res_folderClosed.gif\">";
                        s += RoleResTreeLoop(db, item[0].ToString(), ResAl, tempAl);
                        s += "</item>";
                    }
                    else if ((int)item[2] == 0)
                    {
                        s += "<item text=\"" + item[1] + "\" id=\"0_2_" + item[0] + "\" " + (!ResAl.Contains(item[0].ToString()) ? "" : "checked=\"1\" ") + "im0=\"page.gif\" im1=\"page.gif\" im2=\"page.gif\">";
                        s += "</item>";
                    }
                }
                al.Clear();
                al = null;
                return s;
            }
        }
        public class Favor
        {
            private static string UserSetDefault(string tag)
            {
                switch (tag)
                {
                    case "CP1": return "#000000";
                    case "CP2": return "#212121";
                    case "CP3": return "#353535";
                    case "CP4": return "#ffffff";
                    case "CP5": return "#ffffff";
                    case "CP6": return "#183152";
                    case "CP7": return "#f1f1f1";
                    case "CP8": return "#e8edf3";
                    case "CP9": return "#f0f0f0";
                    case "CP10": return "#f2ffff";
                    case "CP11": return "#ffffcc";
                    case "CP12": return "#0b0b0b";
                    case "FavorStyle": return "ui-lightness";
                }
                return "";
            }
            public static string UserSetGet(string set)
            {
                if (UserBase.UserSetGet().ContainsKey(set)) return UserBase.UserSetGet()[set].ToString();
                else return UserSetDefault(set);
            }
            public static string CssAppend()
            {
                string s = "";
                string index = UserSetGet("FavorStyle");
                s += "<link href=\"/_pro/css/favor/" + index + "/jquery-ui.min.css\" type=\"text/css\" rel=\"stylesheet\" />";
                return s;
            }
            public static string StyleList()
            {
                string s = "";
                string index = UserSetGet("FavorStyle");
                ArrayList al = new ArrayList();
                al.Add(new string[] { "UI lightness", "ui-lightness" });
                al.Add(new string[] { "UI darkness", "ui-darkness" });
                al.Add(new string[] { "Smoothness", "smoothness" });
                al.Add(new string[] { "Start", "start" });
                al.Add(new string[] { "Redmond", "redmond" });
                al.Add(new string[] { "Sunny", "sunny" });
                al.Add(new string[] { "Overcast", "overcast" });
                al.Add(new string[] { "Le Frog", "le-frog" });
                al.Add(new string[] { "Flick", "flick" });
                al.Add(new string[] { "Pepper Grinder", "pepper-grinder" });
                al.Add(new string[] { "Eggplant", "eggplant" });
                al.Add(new string[] { "Dark Hive", "dark-hive" });
                al.Add(new string[] { "Cupertino", "cupertino" });
                al.Add(new string[] { "South Street", "south-street" });
                al.Add(new string[] { "Blitzer", "blitzer" });
                al.Add(new string[] { "Humanity", "humanity" });
                al.Add(new string[] { "Hot Sneaks", "hot-sneaks" });
                al.Add(new string[] { "Excite Bike", "excite-bike" });
                al.Add(new string[] { "Vader", "vader" });
                al.Add(new string[] { "Dot Luv", "dot-luv" });
                al.Add(new string[] { "Mint Choc", "mint-choc" });
                al.Add(new string[] { "Black Tie", "black-tie" });
                al.Add(new string[] { "Trontastic", "trontastic" });
                al.Add(new string[] { "Swanky Purse", "swanky-purse" });
                foreach (string[] url in al)
                {
                    s += "<img title='" + url[0] + "' class='styleselimg' src='/_pro/css/favor/" + url[1] + "/1.gif' onclick=\"$('.styleselimg').css('border-color','transparent');this.style.borderColor='blue';SV('styleseltag','" + url[1] + "')\" style=\"float:left;width:48px;height:48px;margin:4px;cursor:pointer;border:solid 1px transparent;padding:6px;" + (index.Equals(url[1]) ? "border-color:blue;" : "") + "\" onmouseover=\"if(this.style.borderColor!='blue')this.style.borderColor='#aaaaaa'\" onmouseout=\"if(this.style.borderColor!='blue')this.style.borderColor='transparent'\">";
                }
                s += "<input type='hidden' id='styleseltag' value='" + index + "'>";
                for (int i = 1; i <= 12; i++)
                {
                    s += "<input type='hidden' id='cp" + i + "val' value='" + UserSetGet("CP" + i) + "'>";
                }
                al.Clear();
                al = null;
                return s;
            }
            public static string FavorSave(string[] vals)
            {
                string UserID = UserTool.CurUserID();
                if (UserID == null) return SysConst.NotLogin;
                using (DB db = new DB())
                {
                    db.Open();
                    ST st = db.GetTransaction();
                    try
                    {
                        string sql = null;
                        for (int i = 1; i <= 12; i++)
                        {
                            sql = "delete from TB_USERSET where USERID='" + str.D2DD(UserID) + "' and SetTag='CP" + i + "'";
                            db.ExecSql(sql, st);
                            sql = "insert into TB_USERSET(USERID,SetTag,SetVal)values('" + str.D2DD(UserID) + "','CP" + i + "','" + str.D2DD(vals[i - 1]) + "')";
                            db.ExecSql(sql, st);
                        }
                        sql = "delete from TB_USERSET where USERID='" + str.D2DD(UserID) + "' and SetTag='FavorStyle'";
                        db.ExecSql(sql, st);
                        sql = "insert into TB_USERSET(USERID,SetTag,SetVal)values('" + str.D2DD(UserID) + "','FavorStyle','" + str.D2DD(vals[12]) + "')";
                        db.ExecSql(sql, st);
                        st.Commit();
                        UserBase.UserSetInit(db);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        st.Rollback();
                        log.Error(ex);
                        return ex.Message;
                    }
                }
            }
            public static string FavorDefault()
            {
                string UserID = UserTool.CurUserID();
                if (UserID == null) return SysConst.NotLogin;
                using (DB db = new DB())
                {
                    db.Open();
                    ST st = db.GetTransaction();
                    try
                    {
                        string sql = null;
                        for (int i = 1; i <= 12; i++)
                        {
                            sql = "delete from TB_USERSET where USERID='" + str.D2DD(UserID) + "' and SetTag='CP" + i + "'";
                            db.ExecSql(sql, st);
                        }
                        sql = "delete from TB_USERSET where USERID='" + str.D2DD(UserID) + "' and SetTag='FavorStyle'";
                        db.ExecSql(sql, st);
                        st.Commit();
                        UserBase.UserSetInit(db);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        st.Rollback();
                        log.Error(ex);
                        return ex.Message;
                    }
                }
            }
        }
        public class Page
        {
            public static string[] UserInfo()
            {
                string UserID = UserTool.CurUserID();
                try
                {
                    string[] item = new string[6];
                    using (DB db = new DB())
                    {
                        db.Open();
                        string sql = "select * from TB_USERINFO where USERID='" + str.D2DD(UserID) + "'";
                        using (DR dr = new DR(db, sql))
                        {
                            if (dr.Read())
                            {
                                item[0] = dr.GetString("USERNAME");
                                item[1] = dr.GetStringNoNULL("REALNAME");
                                item[2] = dr.GetStringNoNULL("Code");
                                //item[3] = str.GetDecode(dr.GetString("PASSWORD")).Substring(item[0].Length);
                                item[4] = dr.GetStringNoNULL("PHONENUM");
                                item[5] = dr.GetStringNoNULL("EMAIL");
                            }
                        }
                    }
                    return item;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return null;
                }
            }
            public static string UserInfoMod(string[] item)
            {
                string UserID = UserTool.CurUserID();
                if (UserID == null) return SysConst.NotLogin;
                try
                {
                    using (DB db = new DB())
                    {
                        db.Open();
                        string sql = "select USERNAME from TB_USERINFO where USERID='" + str.D2DD(UserID) + "'";
                        string username = db.GetString(sql);
                        sql = "update TB_USERINFO set  PASSWORD='" + str.D2DD(str.GetEncode(username + item[0])) + "',PHONENUM='" + str.D2DD(item[1]) + "',EMAIL='" + str.D2DD(item[2]) + "' where USERID='" + str.D2DD(UserID) + "'";
                        db.ExecSql(sql);
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return ex.Message;
                }
            }
            public static object[] SetMenuList()
            {
                string UserID = UserTool.CurUserID();
                if (UserID == null) return new string[] { SysConst.NotLogin };
                try
                {
                    using (DB db = new DB())
                    {
                        db.Open();
                        string sql = "select MenuID,Icon,Name,URL from TB_SET_MENU where USERID='" + str.D2DD(UserID) + "' order by `Rank`";
                        ArrayList al = new ArrayList();
                        using (DR dr = new DR(db, sql))
                        {
                            while (dr.Read())
                            {
                                al.Add(dr.GetString(0) + "|||" + dr.GetStringNoNULL(1) + "|||" + dr.GetStringNoNULL(2) + "|||" + dr.GetStringNoNULL(3));
                            }
                        }
                        return al.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return new string[] { ex.Message };
                }
            }
            public static string SetMenuSave(string[] item)
            {
                string UserID = UserTool.CurUserID();
                if (UserID == null) return SysConst.NotLogin;
                using (DB db = new DB())
                {
                    db.Open();
                    ST st = db.GetTransaction();
                    try
                    {
                        string sql = "delete from TB_SET_MENU where USERID='" + str.D2DD(UserID) + "'";
                        db.ExecSql(sql, st);
                        int Rank = 1;
                        foreach (string s in item)
                        {
                            //string[] v = s.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                            string[] v = s.Split(new string[] { "|||" }, StringSplitOptions.None);
                            sql = "insert into TB_SET_MENU(UserID,MenuID,`Rank`,Icon,Name,URL)";
                            sql += "values('" + str.D2DD(UserID) + "','" + str.D2DD(v[0]) + "'," + Rank + ",'" + str.D2DD(v[1]) + "','" + str.D2DD(v[2]) + "','" + str.D2DD(v[3]) + "')";
                            db.ExecSql(sql, st);
                            Rank++;
                        }
                        st.Commit();
                        return null;
                    }
                    catch (Exception ex)
                    {
                        st.Rollback();
                        log.Error(ex);
                        return ex.Message;
                    }
                }
            }
            public static string IconList()
            {
                string s = "";
                ArrayList al = new ArrayList();
                al.Add("/_pro/img/pro/icons/system.png");
                al.Add("/_pro/img/pro/icons/analysis.png");
                al.Add("/_pro/img/pro/icons/apply.gif");
                al.Add("/_pro/img/pro/icons/calendar.png");
                al.Add("/_pro/img/pro/icons/certapply.png");
                al.Add("/_pro/img/pro/icons/clock.png");
                al.Add("/_pro/img/pro/icons/clock_48.png");
                al.Add("/_pro/img/pro/icons/cms.png");
                al.Add("/_pro/img/pro/icons/comment_48.png");
                al.Add("/_pro/img/pro/icons/con1.png");
                al.Add("/_pro/img/pro/icons/con2.png");
                al.Add("/_pro/img/pro/icons/con3.png");
                al.Add("/_pro/img/pro/icons/cost.png");
                al.Add("/_pro/img/pro/icons/doc.png");
                al.Add("/_pro/img/pro/icons/finance.png");
                al.Add("/_pro/img/pro/icons/groupperson.png");
                al.Add("/_pro/img/pro/icons/house.png");
                al.Add("/_pro/img/pro/icons/house1.png");
                al.Add("/_pro/img/pro/icons/house2.png");
                al.Add("/_pro/img/pro/icons/income.png");
                al.Add("/_pro/img/pro/icons/manage.png");
                al.Add("/_pro/img/pro/icons/panel.png");
                al.Add("/_pro/img/pro/icons/paper_content_pencil_48.png");
                al.Add("/_pro/img/pro/icons/pencil_48.png");
                al.Add("/_pro/img/pro/icons/prolist.png");
                al.Add("/_pro/img/pro/icons/prolist2.png");
                al.Add("/_pro/img/pro/icons/report.png");
                al.Add("/_pro/img/pro/icons/res.png");
                al.Add("/_pro/img/pro/icons/role.png");
                al.Add("/_pro/img/pro/icons/service.png");
                al.Add("/_pro/img/pro/icons/target.png");
                al.Add("/_pro/img/pro/icons/tasklist.png");
                al.Add("/_pro/img/pro/icons/tasksubmit.png");
                al.Add("/_pro/img/pro/icons/usermanage.gif");
                foreach (string url in al)
                {
                    s += "<img src='" + url + "' onclick=\"iconsel('" + url + "')\" style=\"float:left;width:48px;height:48px;margin:4px;cursor:pointer;border:solid 1px transparent;padding:6px;\" onmouseover=\"this.style.borderColor='#aaaaaa'\" onmouseout=\"this.style.borderColor='transparent'\">";
                }
                al.Clear();
                al = null;
                return s;
            }
            public static string QuickMenuList()
            {
                string UserID = UserTool.CurUserID();
                if (UserID == null) return SysConst.NotLogin;
                DB db = new DB();
                db.Open();
                try
                {
                    string sql = null;
                    sql = "select MenuID,Icon,Name,URL from TB_SET_MENU where UserID='" + str.D2DD(UserID) + "' order by `Rank`";
                    string s = "";
                    DR dr = db.OpenRecord(sql);
                    while (dr.Read())
                    {
                        s += "<LI><A class=shortcut-button href='javascript:void(0)' onclick=\"parent.addTab('" + dr.GetString(2) + "','" + dr.GetString(3) + "')\"><SPAN><IMG src=\"" + dr.GetString(1) + "\"><BR>" + dr.GetString(2) + " </SPAN></A></LI>";
                    }
                    s += "<LI><A class=shortcut-button href='javascript:void(0)' onclick=\"parent.addTab('个人设置','/_base/user_set/mine.aspx?tab=3')\"><SPAN style=\"COLOR: red\"><IMG src=\"/_pro/img/pro/icons/panel.png\"><BR>快捷设置 </SPAN></A></LI>";
                    dr.Close();
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
            public static string OnlineUser()
            {
                DB db = new DB();
                db.Open();
                try
                {
                    string sql = null;
                    string s = "";
                    s += "<div class='fttable' style='height:400px;overflow-y:auto;overflow-x:hidden;background-color:#ffffff'>";
                    s += "<table width=100% cellspacing=1 cellpadding=1><tbody>";
                    IDictionaryEnumerator enumerator = (Stat.OnlineUserStat).GetEnumerator();
                    ArrayList ReAL = new ArrayList();
                    int i = 1;
                    while (enumerator.MoveNext())
                    {
                        string UserID = enumerator.Key.ToString();
                        DateTime LoginTime = ((DateTime[])enumerator.Value)[1];

                        sql = "select USERNAME,REALNAME from TB_USERINFO where USERID='" + str.D2DD(UserID) + "'";
                        DR dr = db.OpenRecord(sql);
                        if (dr.Read())
                        {
                            s += "<tr>";
                            s += "<td width=16>" + i++ + "</td>";
                            s += "<td>" + (dr.GetString(1).Equals("") ? dr.GetString(0) : dr.GetString(1)) + "</td>";
                            s += "<td>" + str.GetDateTimeHold(DateTime.Now, LoginTime) + "</td>";
                            s += "</tr>";
                        }
                        dr.Close();
                    }
                    s += "</tbody></table></div>";
                    enumerator = null;
                    return s;
                }
                catch (Exception ex) { log.Error(ex); return ex.Message; }
                finally { db.Close(); }
            }
        }
        public class Com
        {
            public static string Set(HttpRequest req)
            {
                if (req.QueryString("name") == null || req.QueryString("tag") == null) return "非法访问";
                session.SetString("ComViewSetJust_" + req.QueryString("name").Trim(), "1");
                string ComName = req.QueryString("name").Trim();
                string TagName = req.QueryString("tag").Trim();
                StringBuilder sb = new StringBuilder();
                string keys = "";
                string valis = "";
                string titles = "";
                string names = "";
                sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\"><tbody>");
                using (DB db = new DB())
                {
                    db.Open();
                    using (DB userDB = new DB())
                    {
                        userDB.Open();
                        string sql = "select * from TB_Set_Config where APPNAME='" + str.D2DD(ComName) + "' and Tag='" + str.D2DD(TagName) + "' order by `Rank`";
                        using (DR dr = new DR(db, sql))
                        {
                            while (dr.Read())
                            {
                                string SetKey = dr.GetString("SetKey");
                                string ValiType = dr.GetStringNoNULL("ValiType");
                                string Caption = dr.GetStringNoNULL("Caption");
                                names += "|" + SetKey;
                                valis += "|" + ValiType;
                                titles += "|" + Caption;
                                string DefaultVal = dr.IsDBNull("DefaultVal") ? null : dr.GetString("DefaultVal");
                                string Style = dr.GetStringNoNULL("Style");
                                string[] SelVals = dr.GetStringNoNULL("SelVals").Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                                string Mimo = dr.GetStringNoNULL("Mimo");
                                sql = "select SetVal from TB_SET where AppName='" + str.D2DD(ComName) + "' and SetKey='" + str.D2DD(SetKey) + "'";
                                string SetVal = userDB.GetString(sql);
                                if (SetVal == null && DefaultVal != null) SetVal = DefaultVal;
                                sb.Append("<tr>");
                                sb.Append("<td>" + Caption + "</td>");
                                sb.Append("<td>");
                                switch (dr.GetStringNoNULL("SetType"))
                                {
                                    case "Text":
                                        keys += "|" + SetKey;
                                        sb.Append("<input type='text' style=\"" + (Style.Equals("") ? "width:200px" : Style) + "\" id=\"setkey_" + SetKey + "\" name=\"setkey_" + SetKey + "\" " + (SetVal == null ? "" : (" value=\"" + SetVal + "\"")) + "/>");
                                        break;
                                    case "Pass":
                                        keys += "|" + SetKey;
                                        sb.Append("<input type='password' style=\"" + (Style.Equals("") ? "width:200px" : Style) + "\" id=\"setkey_" + SetKey + "\" name=\"setkey_" + SetKey + "\" " + (SetVal == null ? "" : (" value=\"" + SetVal + "\"")) + "/>");
                                        break;
                                    case "Date":
                                        keys += "|" + SetKey;
                                        sb.Append("<input type='text' class=dateget style=\"" + (Style.Equals("") ? "width:150px" : Style) + "\" id=\"setkey_" + SetKey + "\" name=\"setkey_" + SetKey + "\" " + (SetVal == null ? "" : (" value=\"" + SetVal + "\"")) + "/>");
                                        break;
                                    case "Check":
                                        keys += "|" + SetKey + "_1";
                                        int Loop = 1;
                                        foreach (string vals in SelVals)
                                        {
                                            sb.Append("<input type=checkbox " + (Style.Equals("") ? "" : (" style=\"" + Style + "\" ")) + " name=\"setkey_" + SetKey + "\" id=\"setkey_" + SetKey + "_" + Loop + "\" value='" + vals.Split('|')[1] + "'" + ((SetVal != null && ("," + SetVal + ",").IndexOf("," + vals.Split('|')[1] + ",") >= 0) ? " checked" : "") + "> <label for='setkey_" + SetKey + "_" + Loop + "'>" + vals.Split('|')[0] + "</label>&nbsp;&nbsp;");
                                            Loop++;
                                        }
                                        break;
                                    case "Radio":
                                        keys += "|" + SetKey + "_1";
                                        Loop = 1;
                                        foreach (string vals in SelVals)
                                        {
                                            sb.Append("<input type=radio " + (Style.Equals("") ? "" : (" style=\"" + Style + "\" ")) + " name=\"setkey_" + SetKey + "\" id=\"setkey_" + SetKey + "_" + Loop + "\" value='" + vals.Split('|')[1] + "'" + ((SetVal != null && SetVal.Equals(vals.Split('|')[1])) ? " checked" : "") + "> <label for='setkey_" + SetKey + "_" + Loop + "'>" + vals.Split('|')[0] + "</label>&nbsp;&nbsp;");
                                            Loop++;
                                        }
                                        break;
                                    case "Select":
                                        keys += "|" + SetKey;
                                        sb.Append("<select id=\"setkey_" + SetKey + "\" name=\"setkey_" + SetKey + "\" " + (Style.Equals("") ? "" : (" style=\"" + Style + "\" ")) + ">");
                                        foreach (string vals in SelVals)
                                        {
                                            sb.Append("<option value='" + vals.Split('|')[1] + "'" + ((SetVal != null && SetVal.Equals(vals.Split('|')[1])) ? " selected" : "") + ">" + vals.Split('|')[0] + "</option>");
                                        }
                                        sb.Append("</select>");
                                        break;
                                }
                                if (Mimo != null) sb.Append("<span class=fttip>" + Mimo + "</span>");
                                sb.Append("</td>");
                                sb.Append("</tr>");
                            }
                        }
                    }
                }
                sb.Append("<tr><td colspan=2 style='text-align:center'><button class='_button' tag='ui-icon-check' onclick='Save()'>保存设置</button></td></tr>");
                sb.Append("</tbody></table>");
                sb.Append("<input type='hidden' name='names' id='names' value='" + names + "'>");
                sb.Append("<input type='hidden' name='keys' id='keys' value='" + keys + "'>");
                sb.Append("<input type='hidden' name='valis' id='valis' value='" + valis + "'>");
                sb.Append("<input type='hidden' name='titles' id='titles' value='" + titles + "'>");
                sb.Append("<input type='hidden' name='comname' id='comname' value='" + ComName + "'>");
                return sb.ToString();
            }
            public static string SetSave(HttpRequest req)
            {
                if (!UserTool.IsLogin()) return str.FormPostResultJs(SysConst.NotLogin, null);
                if (session.GetString("ComViewSetJust_" + req.FormString("comname").Trim()) == null || !session.GetString("ComViewSetJust_" + req.FormString("comname").Trim()).ToString().Equals("1")) return str.FormPostResultJs("非法访问", null);
                using (DB db = new DB())
                {
                    db.Open();
                    ST st = db.GetTransaction();
                    try
                    {
                        string comname = req.FormString("comname");
                        string[] names = req.FormString("names").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string name in names)
                        {
                            string sql = "delete from TB_SET where AppName='" + str.D2DD(comname) + "' and SetKey='" + str.D2DD(name) + "'";
                            db.ExecSql(sql, st);
                            sql = "insert into TB_SET(AppName,SetKey,SetVal)";
                            sql += "values('" + str.D2DD(comname) + "','" + str.D2DD(name) + "','" + str.D2DD(req.Form["setkey_" + name]) + "')";
                            db.ExecSql(sql, st);
                        }
                        st.Commit();
                        return str.FormPostResultJs(null, null);
                    }
                    catch (Exception ex)
                    {
                        st.Rollback();
                        log.Error(ex);
                        return str.FormPostResultJs(ex.Message, null);
                    }
                }
            }
            public static string Dic(HttpRequest req)
            {
                if (req.QueryString("name") == null || req.QueryString("tag") == null) return "非法访问";
                session.SetString("ComViewDicJust_" + req.QueryString("name").Trim(), "1");
                string ComName = req.QueryString("name").Trim();
                string TagName = req.QueryString("tag").Trim();
                StringBuilder sb = new StringBuilder();
                sb.Append("<select class=_select onchange='loaddic(this.value)' name='dicsel' id='dicsel'><option value=''>*请选择字典*</option>");
                using (DB db = new DB())
                {
                    db.Open();
                    string sql = "select * from TB_Dic_Config where APPNAME='" + str.D2DD(ComName) + "' and Tag='" + str.D2DD(TagName) + "' order by DicRank";
                    using (DR dr = new DR(db, sql))
                    {
                        while (dr.Read())
                        {
                            sb.Append("<option value='" + ComName + "|" + dr.GetString("SetKey") + "'>" + dr.GetStringNoNULL("Caption") + "</option>");
                        }
                    }
                }
                sb.Append("</select>");
                return sb.ToString();
            }
            public static string DicLoad(string val)
            {
                StringBuilder sb = new StringBuilder();
                string appname = val.Split('|')[0];
                string setkey = val.Split('|')[1];
                using (DB db = new DB())
                {
                    db.Open();
                    string sql = "select Mimo from TB_Dic_Config where AppName='" + str.D2DD(appname) + "' and SetKey='" + str.D2DD(setkey) + "'";
                    string mimo = db.GetStringForceNoNull(sql);
                    sb.Append("$('#dictip').html(\"" + mimo + "\");");
                    sql = "select DicId,DicCode,DicName,DicRank from TB_DIC where AppName='" + str.D2DD(appname) + "' and SetKey='" + str.D2DD(setkey) + "' order by DicRank";
                    using (DR dr = new DR(db, sql))
                    {
                        int Loop = 0;
                        while (dr.Read())
                        {
                            sb.Append("eleVal('dicfid','" + dr.GetString(0) + "'," + Loop + ");");
                            sb.Append("eleVal('diccode',\"" + dr.GetStringNoNULL(1) + "\"," + Loop + ");");
                            sb.Append("eleVal('dicname',\"" + dr.GetStringNoNULL(2) + "\"," + Loop + ");");
                            sb.Append("eleVal('dicorder','" + dr.GetInt32(3) + "'," + Loop + ");");
                            Loop++;
                        }
                    }
                }
                return sb.ToString();
            }
            public static string DicSave(HttpRequest req)
            {
                if (!UserTool.IsLogin()) return str.FormPostResultJs(SysConst.NotLogin, null);
                string dicsel = req.FormString("dicsel");
                string comname = dicsel.Split('|')[0];
                string setkey = dicsel.Split('|')[1];
                if (session.GetString("ComViewDicJust_" + comname) == null || !session.GetString("ComViewDicJust_" + comname).Equals("1")) return str.FormPostResultJs("非法访问", null);
                using (DB db = new DB())
                {
                    db.Open();
                    ST st = db.GetTransaction();
                    try
                    {
                        string dicfid = req.FormString("dicfid");
                        if (dicfid == null || dicfid.Equals("")) dicfid = str.GetCombID();
                        string diccode = req.FormString("diccode").Trim();
                        string dicname = req.FormString("dicname").Trim();
                        int dicorder = int.Parse(req.FormString("dicorder").Trim());

                        string sql = "delete from TB_DIC where AppName='" + str.D2DD(comname) + "' and SetKey='" + str.D2DD(setkey) + "'";
                        db.ExecSql(sql, st);

                        sql = "insert into TB_DIC(AppName,SetKey,DicId,DicCode,DicName,DicRank)";
                        sql += "values('" + str.D2DD(comname) + "','" + str.D2DD(setkey) + "','" + str.D2DD(dicfid) + "','" + str.D2DD(diccode) + "','" + str.D2DD(dicname) + "'," + dicorder + ")";
                        db.ExecSql(sql, st);

                        int RowRate = 1;
                        while (req.FormString("diccode_rowrate" + RowRate) != null)
                        {
                            dicfid = req.FormString("dicfid_rowrate" + RowRate);
                            if (dicfid == null || dicfid.Equals("")) dicfid = str.GetCombID();
                            diccode = req.FormString("diccode_rowrate" + RowRate).Trim();
                            dicname = req.FormString("dicname_rowrate" + RowRate).Trim();
                            dicorder = int.Parse(req.FormString("dicorder_rowrate" + RowRate).Trim());

                            sql = "insert into TB_DIC(AppName,SetKey,DicId,DicCode,DicName,DicRank)";
                            sql += "values('" + str.D2DD(comname) + "','" + str.D2DD(setkey) + "','" + str.D2DD(dicfid) + "','" + str.D2DD(diccode) + "','" + str.D2DD(dicname) + "'," + dicorder + ")";
                            db.ExecSql(sql, st);

                            RowRate++;
                        }
                        st.Commit();
                        return str.FormPostResultJs(null, null);
                    }
                    catch (Exception ex)
                    {
                        st.Rollback();
                        log.Error(ex);
                        return str.FormPostResultJs(ex.Message, null);
                    }
                }
            }
        }
    }
}
