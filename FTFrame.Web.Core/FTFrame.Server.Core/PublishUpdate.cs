using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;
using FTFrame.Server.Core.Tool;
using System.Xml;
using System.Threading.Tasks;
using System.Threading;
using AngleSharp.Html.Parser;
using AngleSharp.Html.Dom;
using AngleSharp.Dom;
using Microsoft.Data.Sqlite;
using DocumentFormat.OpenXml.InkML;
using System.Security.Policy;
using Microsoft.Extensions.Primitives;

namespace FTFrame.Server.Core
{
    public class PublishUpdate
    {
        string Step1 = "正在验证合法性(Checking)";
        string Step2 = "正在上传站点实例(Uploading Site)";
        string Step3 = "正在处理实例资源(Checking Resource)";
        string Step4 = "正在发布数据源(Publishing Data Source)";
        string Step5 = "正在发布资源文件(Publishing Resource Files)";
        string Step6 = "正在生成控件和映象文件(Publishing Control and Mirror Files)";
        string Step7 = "正在发布前台页面(Publishing Fore Part Pages)";
        string Step8 = "正在发布后台页面(Publishing Back Part Pages)";
        string Step9 = "正在发布系统文件(Publishing System Pages)";
        string Step10 = "发布结束、删除临时文件(Publish Over,Delete Template Files)";
        int Persent1 = 1;
        int Persent2 = 5;
        int Persent3 = 20;
        int Persent4 = 30;
        int Persent5 = 40;
        int Persent6 = 50;
        int Persent7 = 60;
        int Persent8 = 75;
        int Persent9 = 90;
        int Persent10 = 100;
        string PublishError = "Publish Error,Please Look Logs";
        string successLabel = "{ftserver}ok";
        string failedLabel = "{ftserver}publish failed!";
        private string siteid;
        private string directPath;
        private DB db = new DB(SysConst.ConnectionStr_FTDP);
        private DB db44 = new DB(SysConst.ConnectionStr_FTDP);
        private string basedir;
        private string[] ControlArray;
        private int version = 0;
        private string uploadname = "";
        private bool IsPublishSplit = false;
        private string PublishSplitFile = null;
        private string PublishSplitServerFile = null;
        private string PublishSplitForeFrame = null;
        private string QianMing = "";
        private DateTime pubDateNow = DateTime.Now;
        bool CodeSetChanged = true;
        public string Publish(HttpRequest Request)
        {
            //DeleteUploadAll();
            uploadname = "upload_" + str.GetCombID();
            db.Open();
            db44.Open();
            try
            {
                log.Debug("Publish Site Start:");
                log.Debug("Upload Folder Name is: " + uploadname);
                string b = System.AppDomain.CurrentDomain.BaseDirectory;
                basedir = b;
                string f = b +"_ftpub"+Path.DirectorySeparatorChar;
                Directory.CreateDirectory(f);
                if (!PublishStep1(Request))
                {
                    log.Error("Publish Role", "Publish Site Step 1 Faild!");
                    PublishState(0, 0, PublishError);
                    DeleteUpload();
                    return(failedLabel);
                }

                if (!PublishStep2())
                {
                    log.Error("Publish Role", "Publish Site Step 2 Faild!");
                    PublishState(0, 0, PublishError);
                    DeleteUpload();
                    return (failedLabel);
                }
                if (!PublishStep3(f))
                {
                    log.Error("Publish Role", "Publish Site Step 3 Faild!");
                    PublishState(0, 0, PublishError);
                    DeleteUpload();
                    return(failedLabel);
                }
                if (!PublishStep4(f))
                {
                    log.Error("Publish Role", "Publish Site Step 4 Faild!");
                    PublishState(0, 0, PublishError);
                    DeleteUpload();
                    return (failedLabel + ":PublishStep4,please view log!");
                }
                if (!IsPublishSplit)
                {
                    if (!PublishStep5(f, b))
                    {
                        log.Error("Publish Role", "Publish Site Step 5 Faild!");
                        PublishState(0, 0, PublishError);
                        DeleteUpload();
                        return(failedLabel);
                    }
                }
                if (!PublishStep6(f, b))
                {
                    log.Error("Publish Role", "Publish Site Step 6 Faild!");
                    PublishState(0, 0, PublishError);
                    DeleteUpload();
                    return(failedLabel);
                }

                if (!IsPublishSplit)
                {
                    if (!PublishStep7(f, b))
                    {
                        log.Error("Publish Role", "Publish Site Step 7 Faild!");
                        PublishState(0, 0, PublishError);
                        DeleteUpload();
                        return(failedLabel);
                    }
                }
                else
                {
                    if (!PublishStep7ForSplit(f, b))
                    {
                        log.Error("Publish Role", "Publish Site Step 7 ForSplit Faild!");
                        PublishState(0, 0, PublishError);
                        DeleteUpload();
                        return(failedLabel);
                    }
                }
                if (!PublishStep8(f))
                {
                    log.Error("Publish Role", "Publish Site Step 8 Faild!");
                    PublishState(0, 0, PublishError);
                    DeleteUpload();
                    return(failedLabel);
                }
                if (!PublishStep9(f, b))
                {
                    log.Error("Publish Role", "Publish Site Step 9 Faild!");
                    PublishState(0, 0, PublishError);
                    DeleteUpload();
                    return(failedLabel);
                }
                if (!PublishStep10(f))
                {
                    log.Error("Publish Role", "Publish Site Step 10 Faild!");
                    PublishState(0, 0, PublishError);
                    DeleteUpload();
                    return(failedLabel);
                }
                db.Close();
                db44.Close();
                log.Debug("Publish Role", "Publish Site Success!version " + version.ToString());

                if (CodeSetChanged)//更改过配置 并且 不是通过反射  //始终编译 && SysConst.CodeGetCompile
                {
                    if (!PublishCodeDynamic(f, b))
                    {
                        log.Error("Publish Role", "Publish Code Dynamic Faild!");
                        PublishState(0, 0, PublishError);
                        DeleteUpload();
                        return (failedLabel);
                    }
                }
                else if (CodeSetChanged)
                {
                    PublishCodeSetUpdate(f, b);
                }

                return ("{version" + version + successLabel);
            }
            catch (Exception ex)
            {
                log.Error("PublishStepAll:" + ex.Message);
                log.Error(ex);
                db.Close();
                db44.Close();
                return(failedLabel);
            }
            finally
            {
                DeleteUpload();
            }
        }
        /// <summary>
        /// 验证合法性
        /// </summary>
        /// <returns></returns>
        private bool PublishStep1(HttpRequest Request)
        {
            PublishState(1, Persent1, Step1);
            try
            {
                Model.SiteInfo SiteInfo = Request.ToType<Model.SiteInfo>(typeof(Model.SiteInfo));
                string _id = SiteInfo._id;
                string _user = SiteInfo._user;
                string _passwd = SiteInfo._passwd;
                string _key = SiteInfo._key;
                if (_id == null) _id = "";
                if (_user == null) _user = "";
                if (_passwd == null) _passwd = "";
                if (_key == null) _key = "";

                log.Debug("site ID is : " + _id);

                string _sql = "select * from ft_sites where siteid='" + _id + "'";

                DR rdr = db.OpenRecord(_sql);
                if (rdr.Read())
                {
                    bool sitekeyok = false;
                    foreach (string RegisterNumber in SysConst.RegisterSiteKeys)
                    {
                        if (string.Compare(_key, RegisterNumber, false) == 0)
                        {
                            sitekeyok = true;
                        }
                    }
                    if (!sitekeyok)
                    {
                        log.Error("key error", "siteupload");
                        return false;
                    }
                    if (!_user.Equals(rdr.GetString("sysuser")))
                    {
                        log.Error("user error", "siteupload");
                        return false;
                    }
                    if (!_passwd.Equals(rdr.GetString("syspasswd")))
                    {
                        log.Error("passwd error", "siteupload");
                        return false;
                    }
                }
                else
                {
                    log.Error("no site error", "siteupload");
                    return false;
                }
                rdr.Close();
                siteid = _id;

                IsPublishSplit = (SiteInfo._split != null && SiteInfo._split == "1");
                if (SiteInfo._splitfile != null) PublishSplitFile = SiteInfo._splitfile.Trim();
                if (SiteInfo._splitserver != null) PublishSplitServerFile = SiteInfo._splitserver.Trim();
                if (SiteInfo._splitforeframe != null) PublishSplitForeFrame = SiteInfo._splitforeframe.Trim();
                if (SiteInfo._datenow != null) pubDateNow = SiteInfo._datenow.Value;
                if (string.IsNullOrEmpty(PublishSplitForeFrame)) PublishSplitForeFrame = "JQueryUI";

                QianMing = SiteInfo._qianming ?? "";
                CodeSetChanged = (SiteInfo._codesetchange != null && SiteInfo._codesetchange == "1"); 
                return true;
            }
            catch (Exception ex)
            {
                log.Error("PublishStep1:" + ex.Message);
                log.Error(ex);
                return false;
            }
        }
        /// <summary>
        /// 上传站点实例
        /// </summary>
        /// <returns></returns>
        private bool PublishStep2() {
            PublishState(2, Persent2, Step2);
            try
            {
                return true;
                string xxxx;
                string md5;
                //Enterprise Version End
                xxxx = str.UrlHost();
                md5 = "&*&^#$%sdfsdfsdaldoa!@#$^$$5" + xxxx + "d4sof谁tL)#$$DD你知道我是？";

                md5 = str.GetMD5(md5);
                foreach (string RegisterNumber in SysConst.RegisterSiteKeys)
                {
                    if (string.Compare(md5, RegisterNumber.Split("-")[0], false) == 0)
                    {
                        goto HOSTOK;
                    }
                }
                log.Error("This Web Site Not Register! <br>Web URL Host is :" + xxxx + "<br>Copyright Maobinbin!");
                return false;
            HOSTOK:
                return true;
            }
            catch (Exception ex)
            {
                log.Error("PublishStep2:" + ex.Message);
                log.Error(ex);
                return false;
            }
        }
        /// <summary>
        /// 处理实例资源
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private bool PublishStep3(string f) {
            PublishState(3, Persent3, Step3);
            try
            {
                if (!File.Exists(f + "site.fthidden"))
                {
                    log.Error("site.fthidden" + " not exist!");
                    return false;
                }
                zip zc = new zip();
                log.Debug("start UnZip site.fthidden");
                zc.UnZip(f + "site.fthidden", f + @"" + uploadname + @""+ Path.DirectorySeparatorChar);
                log.Debug("UnZip End");
                //index
                if (Directory.Exists(f + @"" + uploadname + Path.DirectorySeparatorChar +  @"index"))
                {
                    DirectoryInfo di = new DirectoryInfo(f + @"" + uploadname + Path.DirectorySeparatorChar+ @"index");
                    FileInfo[] fis = di.GetFiles();
                    if (!Directory.Exists(f + @"index"))
                    {
                        Directory.CreateDirectory(f + @"index");
                    }
                    foreach (FileInfo fi in fis)
                    {
                        if (!isFileSame(fi, f + @"index" + Path.DirectorySeparatorChar + fi.Name + ".fthidden"))
                        {
                            fi.CopyTo(f + @"index" + Path.DirectorySeparatorChar + fi.Name + ".fthidden", true);
                            log.Debug(fi.Name + ".fthidden copyed", "index file");
                        }
                    }
                }
                //lib
                if (Directory.Exists(f + @"" + uploadname+ Path.DirectorySeparatorChar + @"lib"))
                {
                    CopyPlus(new DirectoryInfo(f + @"" + uploadname + Path.DirectorySeparatorChar + @"lib"), new DirectoryInfo(basedir + @"" + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "lib"));
                    log.Debug("new lib files copyed");
}
//bin
                if (Directory.Exists(f + @"" + uploadname + @"" + Path.DirectorySeparatorChar + "bin"))
                {
                    DirectoryInfo di = new DirectoryInfo(f + @"" + uploadname + @"" + Path.DirectorySeparatorChar + "bin");
                    FileInfo[] fis = di.GetFiles();
                    if (!Directory.Exists(basedir))
                    {
                        Directory.CreateDirectory(basedir);
                    }
                    foreach (FileInfo fi in fis)
                    {
                        try
                        {
                            if (!isFileSame(fi, basedir + Path.DirectorySeparatorChar + fi.Name))
                            {
                                fi.CopyTo(basedir + Path.DirectorySeparatorChar + fi.Name, true);
                                log.Debug(fi.Name + " copyed", "bin file");
                            }
                        }
                        catch
                        { }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("PublishStep3:" + ex.Message);
                log.Error(ex);
                return false;
            }
        }
        /// <summary>
        /// 发布数据源
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private bool PublishStep4(string f)
        {
            PublishState(4, Persent4, Step4);
            //发布数据源已取消
            return true;
            //发布表单数据表 add by maobb,2013-4-1
            //已取消
            //if (!Sql.PublishDataTable(db, siteid, f + uploadname)) return false;

            string connstr = "Data Source=" + f + "" + uploadname + Path.DirectorySeparatorChar  + "site.db";
            SqliteConnection tempconn = new SqliteConnection(connstr);
            tempconn.Open();
            try
            {
                string strSql = "select * from deledds";
                SqliteCommand myCommand = new SqliteCommand(strSql, tempconn);
                SqliteDataReader rdr = myCommand.ExecuteReader();
                log.Debug("operate deled d.s start:");
                while (rdr.Read())
                {
                    string warename = rdr["name"].ToString();
                    string datasource = rdr["datasource"].ToString();
                    string basename = "ft_" + siteid + "_" + warename + "_" + datasource;
                    log.Debug(basename, "the deled F.T. basename is");
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(f + "" + Path.DirectorySeparatorChar + "index" + Path.DirectorySeparatorChar + "" + warename + ".dll.fthidden");
                    XmlNodeList dropnodes = xmldoc.SelectNodes("//configuration/datasource/" + SysConst.DataBaseType.ToString() + "/drop");
                    int i;
                    for (i = 0; i < dropnodes.Count; i++)
                    {
                        string sql = dropnodes.Item(i).InnerText;
                        log.Debug(sql, "Delete F.T. Sql String from xml is:");
                        string temp = sql.Substring(sql.IndexOf("@") + 1, sql.LastIndexOf("]") - sql.IndexOf("@"));
                        sql = sql.Replace("@[tablename]", basename).Replace("[", "_").Replace("]", "");
                        log.Debug(sql, "Delete F.T. Sql String final is:");
                        try
                        {
                            log.Debug("now exec " + sql);
                            db44.ExecSql(sql);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message + ":maybe this table not Exists!");
                            log.Error(ex);
                        }
                    }
                }
                log.Debug("operate deled d.s end");
                rdr.Close();

                strSql = "select * from controls";
                myCommand = new SqliteCommand(strSql, tempconn);
                rdr = myCommand.ExecuteReader();
                log.Debug("Add d.s start:");
                //asume site controls 类别<1000
                ControlArray = new string[999];
                //Trial Version Start
               // int trialcontrols = 0;
                //if (ConstStr.RuntimeType == 0)
                //{
                //    debug("Trial Version 20 controls Limit!");
                //}
                //Trial Version End
                string SharedItems = "";
            NextControlOut:
                while (rdr.Read())
                {
                    //if (ConstStr.RuntimeType == 0)
                    //{
                    //    if (trialcontrols >= 20) break;
                    //    trialcontrols++;
                    //}
                    string datasource = rdr["datasource"].ToString();

                    string warename = rdr["name"].ToString();
                    ///////UpdateShareXML/////////add by maobb,2006-4-21 start
                    if (datasource.Length == 4 && !rdr["shared"].ToString().Equals("0"))
                    {
                        SharedItems += "<item name=\"" + warename + "\" id=\"" + rdr["id"].ToString() + "\" caption=\"" + rdr["caption"].ToString() + "\" datasource=\"" + datasource + "\" shared=\"" + rdr["shared"].ToString() + "\"/>";
                    }
                    ///////UpdateShareXML/////////add by maobb,2006-4-21 end
                    string basename = "ft_" + siteid + "_" + warename + "_" + datasource;
                    log.Debug(basename, "the add F.T. basename is");
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(f + "" + Path.DirectorySeparatorChar + "index" + Path.DirectorySeparatorChar + "" + warename + ".dll.fthidden");
                    int ControlI;
                    for (ControlI = 0; ControlI < 999; ControlI++)
                    {
                        string ControlName = xmldoc.SelectSingleNode("//configuration/config/control_name").InnerText;
                        if (ControlArray[ControlI] == null)
                        {
                            ControlArray[ControlI] = ControlName;
                            goto ControlIEnd;
                        }
                        else
                        {
                            if (ControlArray[ControlI].ToLower().Equals(ControlName.ToLower()))
                            {
                                goto ControlIEnd;
                            }
                        }
                    }
                ControlIEnd:
                    //当为共享数据源时候，不创建数据源。add by maobb,2006-4-24
                    if (datasource.Length > 4)
                    {
                        goto NextControlOut;
                    }
                    XmlNodeList dropnodes = xmldoc.SelectNodes("//configuration/datasource/" + SysConst.DataBaseType.ToString() + "/create");
                    int i;
                    for (i = 0; i < dropnodes.Count; i++)
                    {
                        string sql = dropnodes.Item(i).InnerText;
                        log.Debug(sql, "Add F.T. Sql String from xml is:");
                        string temp = sql.Substring(sql.IndexOf("@") + 1, sql.LastIndexOf("]") - sql.IndexOf("@"));
                        sql = sql.Replace("@[tablename]", basename).Replace("[", "_").Replace("]", "");
                        log.Debug(sql, "Add F.T. Sql String final is:");
                        try
                        {
                            log.Debug("now exec " + sql);
                            db44.ExecSql(sql);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message + ":maybe this table has Exists!");
                            log.Error(ex);
                        }
                    }
                }
                log.Debug("Add d.s end");
                rdr.Close();
                //PublishUtil.UpdateShareXml(f, siteid, SharedItems);    

                tempconn.Close();
                return true;
            }
            catch (Exception ex)
            {
                log.Error("PublishStep4:" + ex.Message);
                log.Error(ex);
                tempconn.Close();
                return false;
            }
        }
        /// <summary>
        /// 发布资源文件
        /// </summary>
        /// <param name="f"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool PublishStep5(string f, string b)
        {
            PublishState(5, Persent5, Step5);

            string connstr = "Data Source=" + f + "" + uploadname + Path.DirectorySeparatorChar + "site.db";

            SqliteConnection tempconn = new SqliteConnection(connstr);

            tempconn.Open();
try
{
                DirectoryInfo d = new DirectoryInfo(f + "" + uploadname + @"" + Path.DirectorySeparatorChar + "site" + Path.DirectorySeparatorChar + "");
                if (!d.Exists)
                {
                    log.Debug("Directory " + f + "" + uploadname + @"/site/ not exists!");
                    return true;
                }
                FileInfo[] files = d.GetFiles();
                int i;
                //放置在wwwroot下
                for (i = 0; i < files.Length; i++)
                {
                    if (!files[i].Name.ToLower().Equals("web.config"))
                    {
                        if (!isFileSame(files[i], b + "" + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "" + files[i].Name))
                        {
                            File.Delete(b + "" + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "" + files[i].Name);
                            files[i].MoveTo(b + "" + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "" + files[i].Name);
                            log.Debug("Moved File :" + b + "/wwwroot/" + files[i].Name);
                        }
                    }
                }
                DirectoryInfo[] direcs = d.GetDirectories();
                for (i = 0; i < direcs.Length; i++)
                {
                    CopyPlus(direcs[i], new DirectoryInfo(b + "/wwwroot/" + direcs[i].Name));
                    log.Debug("Moved Directory :" + b + "/wwwroot/" + direcs[i].Name);
                }
                string strSql = "select * from pages";
                SqliteCommand myCommand = new SqliteCommand(strSql, tempconn);
                SqliteDataReader rdr = myCommand.ExecuteReader();
                log.Debug("Split FT File Start:");
                while (rdr.Read())
                {
                    string filename = rdr["name"].ToString();
                    string direct;
                    directPath = "/";
                    if (rdr["pid"].ToString().Equals("root"))
                    {
                        direct = b + "/wwwroot/";
                    }
                    else
                    {
                        getDirect(rdr["pid"].ToString(), f);
                        direct = b + "/wwwroot/" + directPath;
                    }
                    log.Debug(direct + filename + " found in cfg.,move to " + f + "" + uploadname + @"/site" + directPath + filename);
                    Directory.CreateDirectory(f + "" + uploadname + @"/site" + directPath);
                    try
                    {
                        if (File.Exists(direct + filename))
                        {
                            if (File.Exists(f + "" + uploadname + @"/site" + directPath + filename)) File.Delete(f + "" + uploadname + @"/site" + directPath + filename);
                            File.Move(direct + filename, f + "" + uploadname + @"/site" + directPath + filename);
                        }
                        else
                        {
                            log.Debug(direct + filename + " not Exsits!");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug(ex.Message);
                        log.Error(ex);
                    }
                }
                log.Debug("Split FT File End");
                rdr.Close();
                tempconn.Close();
                return true;
            }
            catch (Exception ex)
            {
                log.Error("PublishStep5:" + ex.Message);
                log.Error(ex);
                tempconn.Close();
                return false;
            }
        }
        /// <summary>
        /// 生成控件和映象文件
        /// </summary>
        /// <param name="f"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool PublishStep6(string f, string b)
        {
            PublishState(6, Persent6, Step6);
            //已取消
            return true;
            string connstr = "Data Source=" + f + "" + uploadname + @"/" + "site.db";
            SqliteConnection tempconn = new SqliteConnection(connstr);
            tempconn.Open();
            try
            {
                string sql = "select * from ft_" + siteid + "_ctrls";
                DB db2 = new DB();
                db2.Open();
                DB db3 = new DB();
                db3.Open();
                DR rdr2 = db3.OpenRecord(sql);
                while (rdr2.Read())
                {
                    string id = rdr2.GetStringForce("id");
                    string wareid = rdr2.GetString("ctrlid");
                    string warename = rdr2.GetString("ctrlname");
                    string waredata = rdr2.GetString("ctrldata");
                    string sqlmdb = "select * from controls where id='" + wareid + "'";
                    SqliteCommand myCommand = new SqliteCommand(sqlmdb, tempconn);
                    SqliteDataReader rdrmdb = myCommand.ExecuteReader();

                    //当共享数据源为只读数据源时候，不在后台发布构件。add by maobb,2006-4-24
                    if (rdrmdb.Read() && rdrmdb["datasource"].ToString().IndexOf("[1]") <= 0)
                    {
                        sql = "update ft_" + siteid + "_ctrls set ctrlcaption='" + rdrmdb["caption"] + "',ctrldata='" + rdrmdb["datasource"] + "' where id=" + id;
                        db2.ExecSql(sql);
                        log.Debug("New CtrlCaption or DataSource " + sql);
                    }
                    rdrmdb.Close();
                }
                rdr2.Close();
                db2.Close();
                string sqlmdb2 = "select * from controls";
                SqliteCommand myCommand2 = new SqliteCommand(sqlmdb2, tempconn);
                SqliteDataReader rdrmdb2 = myCommand2.ExecuteReader();
                while (rdrmdb2.Read())
                {
                    string id = rdrmdb2["id"].ToString();
                    string name = rdrmdb2["name"].ToString();
                    string caption = rdrmdb2["caption"].ToString();
                    string datasource = rdrmdb2["datasource"].ToString();
                    sql = "select count(*) as counta from ft_" + siteid + "_ctrls where ctrlid='" + id + "'";
                    //当共享数据源为只读数据源时候，不在后台发布构件。add by maobb,2006-4-24
                    if (db3.GetInt(sql) == 0 && datasource.IndexOf("[1]") <= 0)
                    {
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.Load(f + "/index/" + name + ".dll.fthidden");
                        XmlNode node = xmldoc.SelectSingleNode("//configuration/config/canpublish");
                        if (node.InnerText.Equals("yes"))
                        {
                            node = xmldoc.SelectSingleNode("//configuration/publish_node/homepage");
                            sql = "insert into ft_" + siteid + "_ctrls(ctrlid,ctrlname,ctrlcaption,ctrldata,roletype,homepage,autopublish)values('" + id + "','" + name + "','" + caption + "','" + datasource + "','publish','" + node.InnerText + ".cs',1)";
                            log.Debug(sql, "Add new control publish part");
                            db.ExecSql(sql);
                        }
                        node = xmldoc.SelectSingleNode("//configuration/config/cancheck");
                        if (node.InnerText.Equals("yes"))
                        {
                            node = xmldoc.SelectSingleNode("//configuration/check_node/homepage");
                            sql = "insert into ft_" + siteid + "_ctrls(ctrlid,ctrlname,ctrlcaption,ctrldata,roletype,homepage,autopublish)values('" + id + "','" + name + "','" + caption + "','" + datasource + "','check','" + node.InnerText + ".cs',1)";
                            log.Debug(sql, "Add new control check part");
                            db.ExecSql(sql);
                        }
                    }
                }
                db3.Close();
                rdrmdb2.Close();
                tempconn.Close();
                return true;
            }
            catch (Exception ex)
            {
                log.Error("PublishStep6:" + ex.Message);
                log.Error(ex);
                tempconn.Close();
                return false;
            }
        }
        /// <summary>
        /// 发布前台页面 分离发布
        /// </summary>
        /// <param name="f"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool PublishStep7ForSplit(string f, string b) {
            PublishState(7, Persent7, Step7);
            string connstr = "Data Source=" + f + "" + uploadname + @"/" + "site.db";
            SqliteConnection conn = new SqliteConnection(connstr);
            conn.Open();
            SqliteConnection conn2 = new SqliteConnection(connstr);
            conn2.Open();
            try
            {
                string SiteRootPath = f + "" + uploadname + @"/site";
                string SplitFile = null;


                if (PublishSplitServerFile != null && PublishSplitServerFile != "")
                {
                    SplitFile = PublishSplitServerFile;//服务器静态文件直接指向
                }
                else
                {
                    SplitFile = f + "" + uploadname + @"/site/" + PublishSplitFile;//本地静态文件的服务器上传目录
                }
                log.Debug(SplitFile, "Split File");

                string filename = null; string curPath = "/";
                filename = PublishSplitFile.Replace(@"\", "/").Replace("//", "/");
                if (filename.IndexOf('/') >= 0) filename = filename.Substring(filename.LastIndexOf('/') + 1);
                if (filename.IndexOf('.') > 0) filename = filename.Substring(0, filename.LastIndexOf('.'));

                if (PublishSplitFile.LastIndexOf('/') > 0) curPath = PublishSplitFile.Substring(0, PublishSplitFile.LastIndexOf('/') + 1);

                if (!File.Exists(SplitFile))
                {
                    log.Error(SplitFile + " not exsits!");
                    return false;
                }
                string[] waresinpage = new string[999];
                string warestop;
                int i = 0;
                int j;
                var parser = new HtmlParser();
                IHtmlDocument doc;
                using (FileStream fs= new FileStream(SplitFile, FileMode.Open))
                {
                    doc=parser.ParseDocument(fs);
                }
                string doctype = "";
                if(doc.Doctype!=null && doc.Doctype.PublicIdentifier!=null)
                {
                    doctype = "<!DOCTYPE HTML PUBLIC \" "+ doc.Doctype.PublicIdentifier + "\""+(string.IsNullOrEmpty(doc.Doctype.SystemIdentifier)?"":(" \""+ doc.Doctype.SystemIdentifier + "\""))+">";
                }
                int PartPageCount = 0;
                //form about
                string form_formid = "";
                string formpageappendjs = "";
                foreach (IElement ihe in doc.All)
                {
                    //if(ihe.tagName.ToLower().Equals("head") && headString.Equals(""))
                    //{
                    //    headString = "<head>" + ihe.innerHTML + "<script language=\"javascript\" src=\"/_ftres/portalstop.js\"></script></head>";
                    //}
                    //publish control
                    if (ihe.GetAttribute("pubdel") != null && ihe.GetAttribute("pubdel") == "true")
                    {
                        ihe.OuterHtml = "";
                        continue;
                    }
                    if (ihe.NodeName.ToLower() == "link" && ihe.GetAttribute("id") != null && ihe.GetAttribute("id") == "edit_style_file")
                    {
                        ihe.OuterHtml = " ";
                    }
                    if (ihe.NodeName.ToLower().Equals("control") || ihe.NodeName.ToLower().Equals("ftdp:control"))
                    {
                        //get Wares in Page Start
                        bool have = false;
                        string sql = "select a.name,a.id from controls a,parts b where a.id=b.controlid and b.id='" + ihe.GetAttribute("id") + "'";
                        SqliteCommand myCommand8 = new SqliteCommand(sql, conn);
                        SqliteDataReader rdr8 = myCommand8.ExecuteReader();
                        string controlname = "";
                        string controlid = "";
                        if (rdr8.Read())
                        {
                            controlname = rdr8.GetString(0);
                            controlid = rdr8.GetString(1);
                            controlname = Control.Name(controlname);
                        }
                        else
                        {
                            log.Error("controlname is empty sql is:" + sql);
                            rdr8.Close();
                            ihe.OuterHtml = "";
                            goto controlnameempty;
                        }
                        rdr8.Close();
                        //int ControlI;
                        //for (ControlI = 0; ControlI < ControlArray.Length; ControlI++)
                        //{
                        //    if (ControlArray[ControlI] == null)
                        //    {
                        //        return false;
                        //    }

                        //    if (ControlArray[ControlI].ToLower().Equals(controlname.ToLower()))
                        //    {
                        //        controlname = ControlArray[ControlI];
                        //        goto controlnameEnd;
                        //    }
                        //}
                    controlnameEnd:
                        for (j = 0; j < i; j++)
                        {
                            if (waresinpage[j].Equals(controlname))
                                have = true;
                        }
                        if (!have)
                        {
                            waresinpage[i] = controlname;
                            i++;
                        }
                        //get Wares in Page End
                        string partName = ihe.GetAttribute("name");
                        string wareId = controlid;
                        string partId = ihe.GetAttribute("id");
                        //2006-9-15解决多个相同片断name而不能唯一的问题。
                        //mod 2010-6-7
                        sql = "select a.name as warename,a.datasource as waredata,b.partxml as partxml,a.caption as ctrlcaption,b.asportal as asportal,a.paras as paras,b.name as partname from controls a,parts b where a.id=b.controlid and a.id='" + wareId + "' and b.name='" + partName + "' and b.id='" + ihe.GetAttribute("id") + "'";
                        //log.Info("sql" + sql);
                        SqliteCommand myCommand2 = new SqliteCommand(sql, conn);
                        SqliteDataReader rdr2 = myCommand2.ExecuteReader();
                        StringBuilder configSb = new StringBuilder();
                        string fitControlName = Control.Name(controlname);
                        string configNode = "";
                        string ctrlcaption = "";
                        string partname = "";
                        int asportal = 0;
                        if (rdr2.Read())
                        {
                            ctrlcaption = rdr2["ctrlcaption"].ToString();
                            partname = rdr2["partname"].ToString();
                            asportal = int.Parse(rdr2["asportal"].ToString());
                            //mod 2010-6-7
                            XmlDocument controlparadoc = new XmlDocument();
                            string controlparaxml = rdr2["paras"] == null ? "" : rdr2["paras"].ToString();
                            if (controlparaxml.Trim().Equals("")) controlparaxml = "<paras></paras>";
                            controlparadoc.LoadXml(controlparaxml);
                            XmlNodeList controlparanodelist = controlparadoc.SelectNodes("//paras/para");
                            string controlparastr = "";
                            foreach (XmlNode controlparanode in controlparanodelist)
                            {
                                controlparastr += controlparanode.Attributes.GetNamedItem("value").Value + ";";
                            }
                            controlparastr = controlparastr.Replace("\"", "{dsqt}");
                            //=>fore_frame
                            controlparastr = PublishSplitForeFrame;
                            controlparanodelist = null;
                            controlparadoc = null;
                            XmlDocument doc22 = new XmlDocument();
                            doc22.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + rdr2["partxml"]);
                            XmlNodeList nodelist = doc22.SelectNodes("//partxml/param");
                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            for (j = 0; j < nodelist.Count; j++)
                            {
                                string dicKey = nodelist[j].SelectSingleNode("name").InnerText;
                                string dicValue = nodelist[j].SelectSingleNode("value").InnerText.Replace("\"", "{dsqt}").Replace(Environment.NewLine, "  ").Replace("&", "(ftdp)_amp;").Replace("<", "(ftdp)_lt;").Replace(">", "(ftdp)_gt;");
                                if (fitControlName == "List" && partname == "List" && dicKey == "CusSQL")
                                {
                                    dicValue = Sql.GetSqlForRemoveSameCols(dicValue);
                                }
                                paraDic.Add(dicKey, dicValue);
                            }
                           //SetStyle String
                           XmlNodeList stylends = doc22.SelectNodes("//partxml/styles/style");
                            string SetStyle = "";
                            int ij;
                            for (ij = 0; ij < stylends.Count; ij++)
                            {
                                XmlNode stylend = stylends[ij];
                                SetStyle += stylend.Attributes["name"].Value + "}" + stylend.Attributes["class"].Value + "}" + stylend.Attributes["csstext"].Value;
                                if (ij < stylends.Count - 1)
                                {
                                    SetStyle += "{";
                                }
                            }
                            configSb.Append("@{FT."+ fitControlName + ".Fore."+ fitControlName + ".Output(HttpContext, Output, ");
                            configSb.Append("\"" + siteid + "\",\"" + fitControlName + "\",\"" + wareId + "\",\"" + partId + "\",\"" + rdr2["waredata"].ToString() + "\",\"" + SetStyle + "\",\"" + controlparastr + "\"");
                            List<string[]> paras = Control.Paras(controlname);
                            foreach(string[] item in paras)
                            {
                                if(item[1]== "CurPage")
                                {
                                    configSb.Append(",\"" + filename + "\"");
                                }
                                else
                                {
                                    string paraVal = (item[0].Trim()=="string"?"\"\"":"0");
                                    if(paraDic.ContainsKey(item[1]))
                                    {
                                        if (item[0].Trim() == "string")
                                        {
                                            paraVal = "\""+ paraDic[item[1]].Replace("\r", "  ").Replace("\n", "  ").Replace(Environment.NewLine, "  ").Replace("\"", "'") + "\"";
                                        }
                                        else
                                        {
                                            paraVal = "" + paraDic[item[1]] + "";
                                        }
                                    }
                                    configSb.Append((paraVal.Length>100?Environment.NewLine:"") + "," + paraVal);
                                }
                            }
                            configSb.Append(");}");
                        }
                        rdr2.Close();
                        configNode = configSb.ToString();
                        string iheheight;
                        string ihewidth;
                        //if(ihe.getAttribute("height",0)==null)iheheight="1px";
                        //else
                        //    iheheight=ihe.getAttribute("height",0).ToString();
                        //if(ihe.getAttribute("width",0)==null)ihewidth="1px";
                        //else
                        //    ihewidth=ihe.getAttribute("width",0).ToString();
                        configNode = "<span style=\"" + (ihe.GetAttribute("height") == null ? "" : ("height:" + ihe.GetAttribute("height") + ";")) + (ihe.GetAttribute("width") == null ? "" : ("width:" + ihe.GetAttribute("width") + ";")) + "\" id=\"" +
                                     partId + "_" + PartPageCount + "_span\">"+ Environment.NewLine + configNode + Environment.NewLine+ "</span>";
                        //configNode = "<span style=\"height:" + iheheight + ";width:" + ihewidth + "\" id=\"" +
                        //             partId + "_" + PartPageCount + "_span\">" + configNode + "</span>";

                        ihe.OuterHtml = configNode;
                        PartPageCount++;
                    }
                controlnameempty:
                    ;

                }
                //form_formid;
                //warestop = "<%@ Page Language=\"C#\" AutoEventWireup=\"true\"%>\r\n";
                //for (j = 0; j < i; j++)
                //{
                //    warestop += "<%@ Register TagPrefix=\"FT_" + waresinpage[j] + "_Fore\" Namespace=\"FT." + waresinpage[j] + ".Fore\" Assembly=\"FT." + waresinpage[j] + "\"%>\r\n";
                //}
                //log.Debug("Controls in page:" + warestop);
                //warestop = "<%@FTDP%>" + warestop + "<%/FTDP%>";
                waresinpage = null;
                string cacheDir = b+ "" + Path.DirectorySeparatorChar + "Pages" + Path.DirectorySeparatorChar + "" + curPath.Replace("/", Path.DirectorySeparatorChar.ToString());
                Directory.CreateDirectory(cacheDir);
                cacheDir += filename + ".cshtml";
                File.Delete(cacheDir);
                string HtmlStr = doc.All[0].OuterHtml.Replace("(ftdp)_lt;", "<").Replace("(ftdp)_gt;", ">").Replace("(ftdp)_amp;", "&");
                //HtmlStr = HtmlStr.Replace("&lt;%", "<%").Replace("%&gt;", "%>");
                //HtmlStr = HtmlStr.Replace("(ftdp)&lt;", "<").Replace("(ftdp)&gt;", ">").Replace("(ftdp)&amp;", "&");
                //int PaseLeft = HtmlStr.Length < 88 ? -1 : HtmlStr.IndexOf("ftdp", 0, 88);
                //if (PaseLeft >= 0)
                //{
                //    string HtmlStrLeft = HtmlStr.Substring(0, PaseLeft);
                //    string HtmlStrRight = HtmlStr.Substring(PaseLeft + 10);
                //    HtmlStr = HtmlStrLeft + "ftdp" + HtmlStrRight;
                //}
                string htmlTop = "@page"+ Environment.NewLine+ "@using Microsoft.AspNetCore.Http.Extensions;" + Environment.NewLine + "@{Layout = null;}" + Environment.NewLine + "@{if (!FTFrame.Project.Core.ProjectFilter.Page(HttpContext)) return;}" + Environment.NewLine + "@{ if (!FTFrame.Server.Core.AccessControl.Accessed(HttpContext, 0, \"\", 0, 0, \"\", 0, 0, \"\", 0, \"\")) { Output.Write(FTFrame.Server.Core.AccessControl.AccesseInfo(0, \"\", 0, \"\")); return; }}";
                using (StreamWriter sw = new StreamWriter(cacheDir, true, System.Text.Encoding.UTF8))
                {
                    sw.Write(htmlTop + Environment.NewLine + doctype + Environment.NewLine + "@{ FTFrame.Server.Core.JsCss.GetAppend(HttpContext); }" + Environment.NewLine + HtmlStr + "" + "");
                    sw.Flush();
                    sw.Close();
                }
                //debug(doc.body.outerHTML,"doc.body.outerHTML");
                //new FileInfo(cacheDir).LastWriteTime = new FileInfo(sourcefilepath).LastWriteTime;
                Api.MapPageRouteAdd(curPath.Replace("\\", "/") + filename, "splitpage");
                log.Debug("End");
                return true;
            }
            catch (Exception ex)
            {
                log.Error("PublishStep7ForSplit:" + ex.Message);
                log.Error(ex);
                conn2.Close();
                conn.Close();
                return false;
            }
        }
        /// <summary>
        /// 发布动态代码
        /// </summary>
        /// <param name="f"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool PublishCodeDynamic(string f, string b) {
            //PublishState(7, Persent7, Step7);
            string connstr = "Data Source=" + f + "" + uploadname + @"/" + "site.db";
            SqliteConnection conn = new SqliteConnection(connstr);
            conn.Open();
            //SqliteConnection conn2 = new SqliteConnection(connstr);
            //conn2.Open();
            try
            {
                string restr = PublishUtil.CodeCompile(conn, QianMing); 
                return restr == null;
            }
            catch (Exception ex)
            {
                log.Error("PublishCodeDynamic:" + ex.Message);
                log.Error(ex);
                return false;
            }
            finally
            {
                //conn2.Close();
                conn.Close();
            }
        }
        private void PublishCodeSetUpdate(string f, string b)
        {
            //PublishState(7, Persent7, Step7);
            string connstr = "Data Source=" + f + "" + uploadname + @"/" + "site.db";
            SqliteConnection conn = new SqliteConnection(connstr);
            conn.Open();
            //SqliteConnection conn2 = new SqliteConnection(connstr);
            //conn2.Open();
            try
            {
                PublishUtil.CodeSetUpdate(conn, QianMing);
            }
            catch (Exception ex)
            {
                log.Error("PublishCodeSetUpdate:" + ex.Message);
                log.Error(ex);
            }
            finally
            {
                //conn2.Close();
                conn.Close();
            }
        }
        /// <summary>
        /// 发布前台页面
        /// </summary>
        /// <param name="f"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool PublishStep7(string f, string b)
        {
            PublishState(7, Persent7, Step7);
            string connstr = "Data Source=" + f + "" + uploadname + @"/" + "site.db";
            SqliteConnection tempconn = new SqliteConnection(connstr);
            tempconn.Open();
            SqliteConnection conn = new SqliteConnection(connstr);
            conn.Open();
            SqliteConnection conn2 = new SqliteConnection(connstr);
            conn2.Open();
            string SinglePubPageCaption = null;
            string SinglePubPageId = null;
            string SinglePubPageDirectPath = null;
            string SinglePubPageFileName = null;
            try
            {

                string strSql = "select * from pages";
                string curPath = "";
                SqliteCommand myCommand = new SqliteCommand(strSql, tempconn);
                SqliteDataReader rdr = myCommand.ExecuteReader();
                log.Debug("Publish Forpart Start:");
                while (rdr.Read())
                {
                    //form about
                    int ptype = int.Parse(rdr["ptype"].ToString());
                    int mtype = int.Parse(rdr["mtype"].ToString());
                    string pageid = rdr["id"].ToString();
                    SinglePubPageId = pageid;
                    string jinfo = rdr["jinfo"].ToString();
                    string jurl = rdr["jurl"].ToString();
                    //add by maobb 2013-4-10 for page right v4.0
                    int page_modopen = int.Parse(rdr["modopen"].ToString());
                    int page_viewopen = int.Parse(rdr["viewopen"].ToString());
                    string page_datastr = rdr["datastr"].ToString();
                    string page_paraname = rdr["paraname"].ToString();
                    string page_membind = rdr["membind"].ToString();
                    string page_elecdt = rdr["elecdt"].ToString();
                    string page_roledata = rdr["roledata"].ToString();
                    string page_rolesession = rdr["rolesession"].ToString();
                    string page_authrule = rdr["authrule"].ToString();
                    string page_flowstat = rdr["flowstat"].ToString();
                    string page_norightinfo = rdr["norightinfo"].ToString();
                    string page_norighturl = rdr["norighturl"].ToString();
                    int page_a_ip_s = int.Parse(rdr["a_ip_s"].ToString());
                    string page_a_ip_c = rdr["a_ip_c"].ToString();
                    int page_a_ip_o = int.Parse(rdr["a_ip_o"].ToString());
                    int page_a_se_s = int.Parse(rdr["a_se_s"].ToString());
                    string page_a_se_c = rdr["a_se_c"].ToString();
                    int page_a_se_o = int.Parse(rdr["a_se_o"].ToString());
                    int page_a_jp_s = int.Parse(rdr["a_jp_s"].ToString());
                    string page_a_jp_u = rdr["a_jp_u"].ToString();
                    int page_a_tp_s = int.Parse(rdr["a_tp_s"].ToString());
                    string page_a_tp_c = rdr["a_tp_c"].ToString();
                    //string allowstat = rdr["allowstat"] == null ? "" : rdr["allowstat"].ToString();
                    string formfid = (rdr["fid"] == null ? "" : rdr["fid"].ToString());
                    string filename = rdr["name"].ToString();
                    string Page_JS = (rdr["pagejs"] == null ? "" : rdr["pagejs"].ToString());
                    string _ForeFrame = (rdr["fore_frame"] == null ? "" : rdr["fore_frame"].ToString());
                    if (_ForeFrame == "") _ForeFrame = "JQueryUI";
                    string OutType = (rdr["out_type"] ?? "").ToString();
                    //ForeFrameType ForeFrame = (ForeFrameType)Enum.Parse(typeof(ForeFrameType), _ForeFrame);
                    string caption = rdr["caption"].ToString();
                    string direct;
                    directPath = "/";
                    int subCount = 0;
                    if (rdr["pid"].ToString().Equals("root"))
                    {
                        direct = b + "/Pages/";
                        subCount = 0;
                    }
                    else
                    {
                        getDirect(rdr["pid"].ToString(), f);
                        direct = b + "/Pages/" + directPath;
                        subCount = directPath.Length - directPath.Replace("/", "").Length-1;
                    }
                    curPath = directPath;

                    if (OutType == "Json")
                    {
                        PublishUtil.ApiPagePublish(conn, conn2, pageid, caption, directPath, filename, QianMing);
                        if (SinglePubPageCaption == null) SinglePubPageCaption = caption + "_" + filename;
                        else SinglePubPageCaption = "1";
                        SinglePubPageDirectPath = directPath;
                        SinglePubPageFileName = filename;
                        continue;
                    }
                    string sourcefilepath = f + "" + uploadname + @"/site" + directPath + filename;
                    if (!isFileTimeSame(new FileInfo(sourcefilepath), new FileInfo(b + "" + Path.DirectorySeparatorChar + "Pages" + Path.DirectorySeparatorChar + "" + curPath.Replace("/", Path.DirectorySeparatorChar.ToString()) + filename)))
                    {
                        if (File.Exists(sourcefilepath))
                        {
                            log.Debug("Start[" + direct + filename + "]");
                            string[] waresinpage = new string[999];
                            string warestop;
                            int i = 0;
                            int j;
                            var parser = new HtmlParser();
                            IHtmlDocument doc;
                            using (FileStream fs = new FileStream(f + "" + uploadname + @"/site" + directPath + filename, FileMode.Open))
                            {
                                doc = parser.ParseDocument(fs);
                            }
                            string doctype = "";
                            if (doc.Doctype != null && doc.Doctype.PublicIdentifier != null)
                            {
                                doctype = "<!DOCTYPE HTML PUBLIC \" " + doc.Doctype.PublicIdentifier + "\"" + (string.IsNullOrEmpty(doc.Doctype.SystemIdentifier) ? "" : (" \"" + doc.Doctype.SystemIdentifier + "\"")) + ">";
                            }
                            int PartPageCount = 0;
                            //form about
                            string form_formid = "";
                            string formpageappendjs = "";
                            foreach (IElement ihe in doc.All)
                            {
                                if(ihe.NodeName.ToLower() == "head")
                                {
                                    string _subpath = "";
                                    for(int i2=0;i2<subCount;i2++)
                                    {
                                        _subpath += "../";
                                    }
                                    string _template = "";
                                    if(_ForeFrame== "JQueryUI")
                                    {
                                        _template = "head_jqueryui";
                                    }
                                    else if (_ForeFrame == "LayUI")
                                    {
                                        _template = "head_layui";
                                    }
                                    ihe.InnerHtml = "@{await Html.RenderPartialAsync(\"" + _subpath + "_ft/_template/"+ _template + "\");}"+Environment.NewLine+ ihe.InnerHtml;
                                }
                                if (ihe.NodeName.ToLower() == "link" && ihe.GetAttribute("id") != null && ihe.GetAttribute("id") == "edit_style_file")
                                {
                                    ihe.OuterHtml = " ";
                                }
                                if (ihe.NodeName.ToLower() == "style" && ihe.GetAttribute("name") != null && ihe.GetAttribute("name") == "render")
                                {
                                    ihe.OuterHtml = ihe.InnerHtml;
                                }
                                if (ihe.NodeName.ToLower().Equals("control") || ihe.NodeName.ToLower().Equals("ftdp:control"))
                                {
                                    //get Wares in Page Start
                                    bool have = false;
                                    string sql = "select a.name,a.id from controls a,parts b where a.id=b.controlid and b.id='" + ihe.GetAttribute("id") + "'";
                                    SqliteCommand myCommand8 = new SqliteCommand(sql, conn);
                                    SqliteDataReader rdr8 = myCommand8.ExecuteReader();
                                    string controlname = "";
                                    string controlid = "";
                                    if (rdr8.Read())
                                    {
                                        controlname = rdr8.GetString(0);
                                        controlid = rdr8.GetString(1);
                                        controlname = Control.Name(controlname);
                                    }
                                    else
                                    {
                                        log.Error("controlname is empty sql is:" + sql);
                                        rdr8.Close();
                                        ihe.OuterHtml = "";
                                        goto controlnameempty;
                                    }
                                    rdr8.Close();
                                    //int ControlI;
                                    //for (ControlI = 0; ControlI < ControlArray.Length; ControlI++)
                                    //{
                                    //    if (ControlArray[ControlI] == null)
                                    //    {
                                    //        return false;
                                    //    }

                                    //    if (ControlArray[ControlI].ToLower().Equals(controlname.ToLower()))
                                    //    {
                                    //        controlname = ControlArray[ControlI];
                                    //        goto controlnameEnd;
                                    //    }
                                    //}
                                controlnameEnd:
                                    for (j = 0; j < i; j++)
                                    {
                                        if (waresinpage[j].Equals(controlname))
                                            have = true;
                                    }
                                    if (!have)
                                    {
                                        waresinpage[i] = controlname;
                                        i++;
                                    }
                                    //get Wares in Page End
                                    string partName = ihe.GetAttribute("name");
                                    string wareId = controlid;
                                    string partId = ihe.GetAttribute("id");
                                    //2006-9-15解决多个相同片断name而不能唯一的问题。
                                    //mod 2010-6-7
                                    sql = "select a.name as warename,a.datasource as waredata,b.partxml as partxml,a.caption as ctrlcaption,b.asportal as asportal,a.paras as paras,b.name as partname from controls a,parts b where a.id=b.controlid and a.id='" + wareId + "' and b.name='" + partName + "' and b.id='" + ihe.GetAttribute("id") + "'";
                                    //log.Info("sql" + sql);
                                    SqliteCommand myCommand2 = new SqliteCommand(sql, conn);
                                    SqliteDataReader rdr2 = myCommand2.ExecuteReader();
                                    StringBuilder configSb = new StringBuilder();
                                    string fitControlName = Control.Name(controlname);
                                    string configNode = "";
                                    string ctrlcaption = "";
                                    string partname = "";
                                    int asportal = 0;
                                    if (rdr2.Read())
                                    {
                                        ctrlcaption = rdr2["ctrlcaption"].ToString();
                                        partname = rdr2["partname"].ToString();
                                        asportal = int.Parse(rdr2["asportal"].ToString());
                                        //mod 2010-6-7
                                        XmlDocument controlparadoc = new XmlDocument();
                                        string controlparaxml = rdr2["paras"] == null ? "" : rdr2["paras"].ToString();
                                        if (controlparaxml.Trim().Equals("")) controlparaxml = "<paras></paras>";
                                        controlparadoc.LoadXml(controlparaxml);
                                        XmlNodeList controlparanodelist = controlparadoc.SelectNodes("//paras/para");
                                        string controlparastr = "";
                                        foreach (XmlNode controlparanode in controlparanodelist)
                                        {
                                            controlparastr += controlparanode.Attributes.GetNamedItem("value").Value + ";";
                                        }
                                        controlparastr = controlparastr.Replace("\"", "{dsqt}");
                                        //controlpara => fore_frame
                                        controlparastr = _ForeFrame;

                                        controlparanodelist = null;
                                        controlparadoc = null;

                                        XmlDocument doc22 = new XmlDocument();
                                        doc22.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + rdr2["partxml"]);
                                        XmlNodeList nodelist = doc22.SelectNodes("//partxml/param");
                                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                                        for (j = 0; j < nodelist.Count; j++)
                                        {
                                            string dicKey = nodelist[j].SelectSingleNode("name").InnerText;
                                            string dicValue = nodelist[j].SelectSingleNode("value").InnerText.Replace("\"", "{dsqt}").Replace(Environment.NewLine, "  ").Replace("&", "(ftdp)_amp;").Replace("<", "(ftdp)_lt;").Replace(">", "(ftdp)_gt;");
                                            if(fitControlName=="List" && partname=="List" && dicKey== "CusSQL")
                                            {
                                                dicValue = Sql.GetSqlForRemoveSameCols(dicValue);
                                            }
                                            paraDic.Add(dicKey, dicValue);
                                        }
                                        //SetStyle String
                                        XmlNodeList stylends = doc22.SelectNodes("//partxml/styles/style");
                                        string SetStyle = "";
                                        int ij;
                                        for (ij = 0; ij < stylends.Count; ij++)
                                        {
                                            XmlNode stylend = stylends[ij];
                                            SetStyle += stylend.Attributes["name"].Value + "}" + stylend.Attributes["class"].Value + "}" + stylend.Attributes["csstext"].Value;
                                            if (ij < stylends.Count - 1)
                                            {
                                                SetStyle += "{";
                                            }
                                        }
                                        configSb.Append("@{FT." + fitControlName + ".Fore." + fitControlName + ".Output(HttpContext, Output, ");
                                        configSb.Append("\"" + siteid + "\",\"" + fitControlName + "\",\"" + wareId + "\",\"" + partId + "\",\"" + rdr2["waredata"].ToString() + "\",\"" + SetStyle + "\",\"" + controlparastr + "\"");
                                        List<string[]> paras = Control.Paras(controlname);
                                        foreach (string[] item in paras)
                                        {
                                            if (item[1] == "CurPage")
                                            {
                                                configSb.Append(",\"" + filename + "\"");
                                            }
                                            else
                                            {
                                                string paraVal = (item[0].Trim() == "string" ? "\"\"" : "0");
                                                if (paraDic.ContainsKey(item[1]))
                                                {
                                                    if (item[0].Trim() == "string")
                                                    {
                                                        paraVal = "\"" + paraDic[item[1]].Replace("\r", "  ").Replace("\n", "  ").Replace(Environment.NewLine, "  ").Replace("\"", "'") + "\"";
                                                    }
                                                    else
                                                    {
                                                        paraVal = "" + paraDic[item[1]] + "";
                                                    }
                                                }
                                                configSb.Append((paraVal.Length > 100 ? Environment.NewLine : "") + "," + paraVal);
                                            }
                                        }
                                        configSb.Append(");}");
                                    }
                                    rdr2.Close();
                                    configNode = "<span style=\"" + (ihe.GetAttribute("height") == null ? "" : ("height:" + ihe.GetAttribute("height") + ";")) + (ihe.GetAttribute("width") == null ? "" : ("width:" + ihe.GetAttribute("width") + ";")) + "\" id=\"" +
                                     partId + "_" + PartPageCount + "_span\">" + Environment.NewLine + configSb.ToString() + Environment.NewLine + "</span>";
                                    //访问权限
                                    int a_ip_s = 0;
                                    string a_ip_c = "";
                                    int a_ip_o = 0;
                                    int a_se_s = 0;
                                    string a_se_c = "";
                                    int a_se_o = 0;
                                    int a_jp_s = 0;
                                    string a_jp_u = "";
                                    int a_tp_s = 0;
                                    string a_tp_c = "";
                                    string sql9 = "select a_ip_s,a_ip_c,a_ip_o,a_se_s,a_se_c,a_se_o,a_jp_s,a_jp_u,a_tp_s,a_tp_c from parts where id='" + partId + "'";
                                    //log.Info("sql3" + sql3);
                                    try
                                    {
                                        SqliteCommand myCommand10 = new SqliteCommand(sql9, conn2);
                                        SqliteDataReader rdr10 = myCommand10.ExecuteReader();
                                        if (rdr10.Read())
                                        {
                                            a_ip_s = int.Parse(rdr10.GetValue(0).ToString());
                                            a_ip_c = rdr10.GetValue(1).ToString();
                                            a_ip_o = int.Parse(rdr10.GetValue(2).ToString());
                                            a_se_s = int.Parse(rdr10.GetValue(3).ToString());
                                            a_se_c = rdr10.GetValue(4).ToString();
                                            a_se_o = int.Parse(rdr10.GetValue(5).ToString());
                                            a_jp_s = int.Parse(rdr10.GetValue(6).ToString());
                                            a_jp_u = rdr10.GetValue(7).ToString();
                                            a_tp_s = int.Parse(rdr10.GetValue(8).ToString());
                                            a_tp_c = rdr10.GetValue(9).ToString();
                                        }
                                        rdr10.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex.Message + ",ftdp.exe db not correct!");
                                    }
                                    configNode = "@{if (FTFrame.Server.Core.AccessControl.Accessed(HttpContext, " + a_ip_s + ",\"" + a_ip_c + "\"," + a_ip_o + "," + a_se_s + ",\"" + a_se_c + "\"," + a_se_o + "," + a_jp_s + ",\"" + a_jp_u + "\"," + a_tp_s + ",\"" + a_tp_c + "\")){" + Environment.NewLine
                                        + configNode + Environment.NewLine + "}else { Output.Write(FTFrame.Server.Core.AccessControl.AccesseInfo(" + a_jp_s + ",\"" + a_jp_u + "\"," + a_tp_s + ",\"" + a_tp_c + "\")); }}";
                                    ihe.OuterHtml = configNode;
                                    PartPageCount++;
                                }
                            controlnameempty:
                                ;
                            }
                            waresinpage = null;
                            string cacheDir = b + "" + Path.DirectorySeparatorChar +"Pages" + Path.DirectorySeparatorChar + "" + curPath.Replace("/", Path.DirectorySeparatorChar.ToString());
                            Directory.CreateDirectory(cacheDir);
                            cacheDir += filename+".cshtml";
                            File.Delete(cacheDir);
                            string HtmlStr = doc.All[0].OuterHtml.Replace("(ftdp)_lt;", "<").Replace("(ftdp)_gt;", ">").Replace("(ftdp)_amp;", "&");
                            string htmlTop = "@page" + Environment.NewLine + "@using Microsoft.AspNetCore.Http.Extensions;" + Environment.NewLine + "@{Layout = null;}" + Environment.NewLine + "@{if (!FTFrame.Project.Core.ProjectFilter.Page(HttpContext)) return;}" + Environment.NewLine + "@{ if (!FTFrame.Server.Core.AccessControl.Accessed(HttpContext, " + page_a_ip_s + ",\"" + page_a_ip_c + "\"," + page_a_ip_o + "," + page_a_se_s + ",\"" + page_a_se_c + "\"," + page_a_se_o + "," + page_a_jp_s + ",\"" + page_a_jp_u + "\"," + page_a_tp_s + ",\"" + page_a_tp_c + "\")) { Output.Write(FTFrame.Server.Core.AccessControl.AccesseInfo(" + page_a_jp_s + ",\"" + page_a_jp_u + "\"," + page_a_tp_s + ",\"" + page_a_tp_c + "\")); return; }}";
                            if (Page_JS != "") Page_JS = "\r\n<script language='javascript' id='ftdp_page_js'>" + Page_JS + "</script>";
                            using (StreamWriter sw = new StreamWriter(cacheDir, true, System.Text.Encoding.UTF8))
                            {
                                sw.Write(htmlTop + Environment.NewLine + doctype + Environment.NewLine + "@{ FTFrame.Server.Core.JsCss.GetAppend(HttpContext); }" + Environment.NewLine + HtmlStr + Page_JS+"" + "");
                                sw.Flush();
                                sw.Close();
                                new FileInfo(cacheDir).LastWriteTime = new FileInfo(sourcefilepath).LastWriteTime;
                                Api.MapPageRouteAdd(curPath.Replace("\\", "/") + filename, pageid);
                                log.Debug("End");
                                if (SinglePubPageCaption == null) SinglePubPageCaption = caption + "_" + filename;
                                else SinglePubPageCaption = "1";
                            }
                        }
                    }
                }
                log.Debug("Publish forpart End");
                rdr.Close();
                conn.Close();
                conn2.Close();
                tempconn.Close();
                if (SinglePubPageCaption != null && SinglePubPageCaption != "1")
                {
                    PublishUtil.SingleFileSiteBakSave(SinglePubPageCaption, pubDateNow, SinglePubPageId, SinglePubPageDirectPath, SinglePubPageFileName);
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("PublishStep7:" + ex.Message);
                log.Error(ex);
                conn2.Close();
                conn.Close();
                tempconn.Close();
                return false;
            }
        }
        /// <summary>
        /// 发布后台页面
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private bool PublishStep8(string f) {
            PublishState(8, Persent8, Step8);
            return true;
        }
        /// <summary>
        /// 发布系统文件
        /// </summary>
        /// <param name="f"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool PublishStep9(string f, string b) {
            PublishState(9, Persent9, Step9);
            return true;
        }
        /// <summary>
        /// 删除临时文件
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private bool PublishStep10(string f) {
            PublishState(10, Persent10, Step10);
            try
            {/*
				 * 向数据库写入此次发布记录和历史,另加
				 * 发布后续工作，升级站点版本
				 */
                DB db34 = new DB(SysConst.ConnectionStr_FTDP);
                db34.Open();
                db34.ExecSql("update ft_sites set siteversion=siteversion+1 where siteid='" + siteid + "'");

                //log.Info("get site version sql is:" + "select siteversion from ft_sites where siteid='" + siteid + "'");

                version = db34.GetInt("select siteversion from ft_sites where siteid='" + siteid + "'");

                db34.Close();
                log.Debug("new site version is:" + version);
                //if(Directory.Exists(f + "" + uploadname + @""))
                //{
                //    try
                //    {
                //        Directory.Delete(f + "" + uploadname + @"", true);
                //    }
                //    catch{}
                //}
                return true;
            }
            catch (Exception ex)
            {
                log.Error("PublishStep10:" + ex.Message);
                log.Error(ex);
                return true;
            }
        }
        private void DeleteUploadAll()
        {
            if(Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "/_ftpub"))
            {
                var list=new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory + "/_ftpub").GetDirectories().Select(r=>r.FullName);
                foreach(var dir in list)
                {
                    try
                    {
                        Directory.Delete(dir, true);
                    }
                    catch (Exception ex)
                    {
                        //log.Error(ex);
                    }
                }
            }
        }
        private void DeleteUpload()
        {
            try
            {
                if (Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "/_ftpub/" + uploadname + ""))
                {
                    Directory.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "/_ftpub/" + uploadname + "", true);
                }
            }
            catch (Exception ex)
            {
                //log.Error(ex);
            }
        }
        private void getDirect(string pid, string f)
        {
            string connstr = "Data Source=" + f + "" + uploadname + @"/" + "site.db";
            SqliteConnection tempconn = new SqliteConnection(connstr);
            tempconn.Open();
            string strSql = "select * from directory where id='" + pid + "'";
            SqliteCommand myCommand = new SqliteCommand(strSql, tempconn);
            SqliteDataReader rdr = myCommand.ExecuteReader();
            if (rdr.Read())
            {
                directPath = "/" + rdr["name"] + directPath;
                if (!rdr["pid"].ToString().Equals("root") && directPath.Length < 2008)
                {
                    getDirect(rdr["pid"].ToString(), f);
                }
            }
            else
            {
                log.Debug("id=" + pid + "'s directory not find,maybe the site.db have error!");
            }
            rdr.Close();
            tempconn.Close();
        }
        public static string PublishStateXml = "";
        private void PublishState(int step, int persent, string label)
        {
            string s = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            s += "<publishstate><step>" + step + "</step><persent>" + persent + "</persent><label>" + label + "</label></publishstate>";
            PublishStateXml = s;
        }
        private bool isFileSame(FileInfo fs, string d)
        {
            if (!fs.Exists || !File.Exists(d))
            {
                return false;
            }
            FileInfo fd = new FileInfo(d);
            if (!((fs.LastWriteTime).Equals(fd.LastWriteTime)))
            {
                return false;
            }
            if ((fs.Length) != (fd.Length))
            {
                return false;
            }
            return true;
        }
        private bool isFileTimeSame(FileInfo fs, FileInfo fd)
        {
            if (!fs.Exists || !fd.Exists)
            {
                return false;
            }
            if (!((fs.LastWriteTime).Equals(fd.LastWriteTime)))
            {
                return false;
            }
            return true;
        }
        private void CopyPlus(DirectoryInfo SourceDirectory,DirectoryInfo DestinationDirectory)
        {
            DirectoryInfo[] SourceSubDirectories;
            FileInfo[] SourceFiles;


            SourceFiles = SourceDirectory.GetFiles();


            SourceSubDirectories = SourceDirectory.GetDirectories();

            //Create the Destination Directory
            if (File.Exists(DestinationDirectory.FullName))
            {
                File.Delete(DestinationDirectory.FullName);
            }
            if (!DestinationDirectory.Exists) DestinationDirectory.Create();

            //Recursively Copy Every SubDirectory and it's 
            //Contents (according to folder filter)
            foreach (DirectoryInfo SourceSubDirectory in SourceSubDirectories)
                CopyPlus(SourceSubDirectory, new DirectoryInfo(
                    DestinationDirectory.FullName + Path.DirectorySeparatorChar + SourceSubDirectory.Name));

            //Copy Every File to Destination Directory (according to file filter)
            foreach (FileInfo SourceFile in SourceFiles)
            {
                string targetFileName = DestinationDirectory.FullName + Path.DirectorySeparatorChar + SourceFile.Name;

                if (!isFileSame(SourceFile, targetFileName))
                {
                    if (Directory.Exists(targetFileName))
                    {
                        Directory.Delete(targetFileName, true);
                    }
                    SourceFile.CopyTo(targetFileName, true);
                }
            }
        }
    }
}
