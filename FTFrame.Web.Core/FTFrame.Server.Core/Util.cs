using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using FTFrame.Server.Core.Tool;
using FTFrame.Project.Core.Utils;
using FTFrame.Project.Core;

namespace FTFrame.Server.Core
{
    public class Util
    {
        public static string SiteAdd(HttpRequest Request)
        {
            string successLabel = @"{ftserver}ok";
            string noThisSite = @"{ftserver}no this site!";
            string noThisUser = @"{ftserver}no this user!";
            string passwordError = @"{ftserver}password error";
            string errorCDKey = @"{ftserver}not correct CD Key!";
            string exceptionLabel = @"{ftserver}Exception occur!";
            DB db = new DB(SysConst.ConnectionStr_FTDP);
            try
            {
                Model.SiteInfo SiteInfo = Request.ToType<Model.SiteInfo>(typeof(Model.SiteInfo));
                if (SiteInfo._id == null) SiteInfo._id = "";
                if (SiteInfo._user == null) SiteInfo._user = "";
                if (SiteInfo._passwd == null) SiteInfo._passwd = "";
                if (SiteInfo._key == null) SiteInfo._key = "";
                string _sql = "select * from ft_sites where siteid='" + SiteInfo._id.D2() + "'";
                db.Open();
                using (var rdr = db.OpenRecord(_sql))
                {
                    if (rdr.Read())
                    {
                        bool sitekeyok = false;
                        foreach (string RegisterNumber in SysConst.RegisterSiteKeys)
                        {
                            if (string.Compare(SiteInfo._key, RegisterNumber, false) == 0)
                            {
                                sitekeyok = true;
                            }
                        }
                        if (!sitekeyok)
                        {
                            return errorCDKey;
                        }
                        if (!SiteInfo._user.Equals(rdr.GetString("sysuser")))
                        {
                            return noThisUser;
                        }
                        if (!SiteInfo._passwd.Equals(rdr.GetString("syspasswd")))
                        {
                            return passwordError;
                        }
                        return (successLabel + "{domin" + rdr.GetString("sitehost") + "{caption" + rdr.GetString("sitecaption") + "{group" + rdr.GetString("sitegroup") + "{version" + rdr.GetStringForce("siteversion"));
                    }
                    else
                    {
                        return noThisSite;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return (exceptionLabel + "\n" + ex.Message);
            }
            finally
            {
                db.Close();
            }
        }
        public static string SiteUpload(HttpRequest Request)
        {
            string successLabel = @"{ftserver}ok";
            string failedLabel = @"{ftserver}upload failed!";
            string ispublishing = @"{ftserver}site is publishing!";
            DB db = new DB(SysConst.ConnectionStr_FTDP);
            try
            {
                Model.SiteInfo SiteInfo = Request.ToType<Model.SiteInfo>(typeof(Model.SiteInfo));
                if (SiteInfo._id == null) SiteInfo._id = "";
                if (SiteInfo._user == null) SiteInfo._user = "";
                if (SiteInfo._passwd == null) SiteInfo._passwd = "";
                if (SiteInfo._key == null) SiteInfo._key = "";
                string _sql = "select * from ft_sites where siteid='" + SiteInfo._id.D2() + "'";
                db.Open();
                using (var rdr = db.OpenRecord(_sql))
                {
                    if (rdr.Read())
                    {
                        bool sitekeyok = false;
                        foreach (string RegisterNumber in SysConst.RegisterSiteKeys)
                        {
                            if (string.Compare(SiteInfo._key, RegisterNumber, false) == 0)
                            {
                                sitekeyok = true;
                            }
                        }
                        if (!sitekeyok)
                        {
                            log.Error("key error", "siteupload");
                            return (failedLabel);
                        }
                        if (!SiteInfo._user.Equals(rdr.GetString("sysuser")))
                        {
                            log.Error("user error", "siteupload");
                            return (failedLabel);
                        }
                        if (!SiteInfo._passwd.Equals(rdr.GetString("syspasswd")))
                        {
                            log.Error("passwd error", "siteupload");
                            return (failedLabel);
                        }
                    }
                    else
                    {
                        log.Error("no site error", "siteupload");
                        return (failedLabel);
                    }
                    //Has used guid directory.Can delete.
                    if (Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"" + Path.DirectorySeparatorChar + "_ftpub" + Path.DirectorySeparatorChar + "upload"))
                    {
                        log.Error("site is publishing", "siteupload");
                        return (ispublishing);
                    }
                    if (Request.Form.Files.Count != 1)
                    {
                        log.Error("file count!=1", "siteupload");
                        return (failedLabel);
                    }
                    string deploypath = System.AppDomain.CurrentDomain.BaseDirectory + @"" + Path.DirectorySeparatorChar + "_ftpub";
                    if (!Directory.Exists(deploypath))
                    {
                        Directory.CreateDirectory(deploypath);
                    }
                    if (!Directory.Exists(SysConst.SiteBak))
                    {
                        Directory.CreateDirectory(SysConst.SiteBak);
                    }
                    if (Directory.Exists(deploypath + @"" + Path.DirectorySeparatorChar + "upload")) Directory.Delete(deploypath + @"" + Path.DirectorySeparatorChar + "upload", true);
                    using (var inputStream = new FileStream(deploypath + @"" + Path.DirectorySeparatorChar + "site.fthidden", FileMode.Create))
                    {
                        // read file to stream
                        Request.Form.Files[0].CopyTo(inputStream);
                        // stream to byte array
                        byte[] array = new byte[inputStream.Length];
                        inputStream.Seek(0, SeekOrigin.Begin);
                        inputStream.Read(array, 0, array.Length);
                        // get file name
                        //string fName = Request.Form.Files[0].FileName;
                    }
                }
                return (successLabel);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return (failedLabel);
            }
            finally
            {
                db.Close();
            }
        }
        public static ActionResult ClientOperation(HttpRequest Request, HttpResponse Response)
        {
            string auth = SiteAuth(Request);
            if (auth != null)
            {
                return new ContentResult() { Content = "error:" + auth, ContentType = "text/plain" };
            }
            string _type = Request.Form["type"].FirstOrDefault<string>().Trim();
            if (_type == "PageDel")
            {
                string pageid = Request.Form["PageID"].FirstOrDefault<string>().Trim();
                using (DB db = new DB(SysConst.ConnectionStr_FTDP))
                {
                    db.Open();
                    string sql = "delete from ft_ftdp_apidoc where PageID='" + pageid.D2() + "'";
                    db.ExecSql(sql);
                    sql = "delete from ft_ftdp_apiset where PageID='" + pageid.D2() + "'";
                    db.ExecSql(sql);
                    sql = "delete from ft_ftdp_route where PageID='" + pageid.D2() + "'";
                    db.ExecSql(sql);
                }
            }
            else if (_type == "DefaultColumn")
            {
                var restr = "";
                string tableName = Request.Form["TableName"].FirstOrDefault<string>()?.Trim();
                if (string.IsNullOrWhiteSpace(tableName)) return new ContentResult() { Content = restr, ContentType = "text/plain" };
                restr+="|"+DBSuit.Key(tableName).KeyName;
                var reqDic = new Dictionary<string, object>();
                reqDic.Add("_userinfo_", null);
                var dic = DBSuit.DefaultColsWhenAddForApi(tableName, reqDic);
                restr += "|" + string.Join("|", dic.Keys.ToArray());
                dic.Clear();
                return new ContentResult() { Content = restr, ContentType = "text/plain" };
            }
            else if (_type == "BackupList")
            {
                string pageid = Request.Form["PageID"].FirstOrDefault<string>().Trim();
                if (!Directory.Exists(SysConst.SiteBak + "" + Path.DirectorySeparatorChar + "" + pageid))
                {
                    return new ContentResult() { Content = "", ContentType = "text/plain" };
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    string[] s = Directory.GetFiles(SysConst.SiteBak + "" + Path.DirectorySeparatorChar + "" + pageid);
                    List<string> ssss = s.OrderByDescending(ss => new FileInfo(ss).CreationTime).ToList();
                    foreach (string filename in ssss)
                    {
                        string _file = filename.Substring(filename.Replace("/", Path.DirectorySeparatorChar.ToString()).LastIndexOf(Path.DirectorySeparatorChar) + 1);
                        if (_file.ToLower().EndsWith(".site"))
                        {
                            string[] item = _file.ToLower().Split(new string[] { "_", "." }, StringSplitOptions.None);
                            DateTime dt = DateTime.ParseExact(item[0], "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                            sb.Append(item[1] + "_" + item[2] + "#" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "#" + _file + ";");
                        }
                    }
                    ssss.Clear();
                    string restr = (sb.ToString());
                    sb.Clear();
                    sb = null;
                    return new ContentResult() { Content = restr, ContentType = "text/plain" };
                }
            }
            else if (_type == "BackupGet")
            {
                try
                {
                    string pageid = Request.Form["PageID"].FirstOrDefault<string>().Trim();
                    string filename = Request.Form["FileName"].FirstOrDefault<string>().Trim();
                    if (!File.Exists(SysConst.SiteBak + "" + Path.DirectorySeparatorChar + "" + pageid + "" + Path.DirectorySeparatorChar + "" + filename))
                    {
                        return new ContentResult() { Content = "error:Backup is empty", ContentType = "text/plain" };
                    }
                    using (FileStream fs = new FileStream(SysConst.SiteBak + "" + Path.DirectorySeparatorChar + "" + pageid + "" + Path.DirectorySeparatorChar + "" + filename, FileMode.Open))
                    {
                        byte[] bytes = new byte[(int)fs.Length];
                        fs.Read(bytes, 0, bytes.Length);
                        fs.Close();

                        return new FileContentResult(bytes, "application/octet-stream");
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return new ContentResult() { Content = "error:" + ex.Message, ContentType = "text/plain" };
                }
            }
            else if (_type == "BackupGetPublish")
            {
                try
                {
                    string pageid = Request.Form["PageID"].FirstOrDefault<string>().Trim();
                    string filename = Request.Form["FileName"].FirstOrDefault<string>().Trim();
                    string publishedFile = null;
                    string dirPath = SysConst.SiteBak + "" + Path.DirectorySeparatorChar + "" + pageid + "" + Path.DirectorySeparatorChar + "" + filename.Substring(0, filename.Length - 5);
                    if (Directory.Exists(dirPath))
                    {
                        while (true)
                        {
                            var dir = new DirectoryInfo(dirPath);
                            if (dir.GetFiles().Length == 1)
                            {
                                publishedFile = dir.GetFiles()[0].FullName;
                                break;
                            }
                            else if (dir.GetDirectories().Length == 1)
                            {
                                dirPath = dir.GetDirectories()[0].FullName;
                            }
                            else break;
                        }
                    }
                    if (publishedFile == null)
                    {
                        return new ContentResult() { Content = "error:file does not exist", ContentType = "text/plain" };
                    }
                    using (FileStream fs = new FileStream(publishedFile, FileMode.Open))
                    {
                        byte[] bytes = new byte[(int)fs.Length];
                        fs.Read(bytes, 0, bytes.Length);
                        fs.Close();

                        return new FileContentResult(bytes, "application/octet-stream");
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return new ContentResult() { Content = "error:" + ex.Message, ContentType = "text/plain" };
                }
            }
            else if (_type == "FileGet")
            {
                try
                {
                    string getType = Request.Form["GetType"].FirstOrDefault<string>().Trim();
                    string getPath = Request.Form["GetPath"].FirstOrDefault<string>().Trim();
                    string basePath = AppDomain.CurrentDomain.BaseDirectory;
                    if (getType == "PublishNet")
                    {
                        string filepath = basePath + "" + Path.DirectorySeparatorChar + "Pages" + Path.DirectorySeparatorChar + "" + getPath + ".cshtml";
                        filepath = filepath.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("\\", Path.DirectorySeparatorChar.ToString());
                        if (File.Exists(filepath))
                        {
                            using (FileStream fs = new FileStream(filepath, FileMode.Open))
                            {
                                byte[] bytes = new byte[fs.Length];
                                fs.Read(bytes, 0, bytes.Length);
                                fs.Close();
                                return new FileContentResult(bytes, "application/octet-stream");
                            }
                        }
                    }
                    else if (getType == "PublishJava")
                    {
                        if (string.IsNullOrWhiteSpace(PublishUtil.JavaRootPath)) return new ContentResult() { Content = "", ContentType = "text/plain" }; ;
                        string pubPath = Path.GetFullPath(PublishUtil.JavaRootPath) + "/project/src/main/java/com/ftframe/api";
                        string filepath = pubPath + Path.DirectorySeparatorChar + "" + getPath + ".java";
                        filepath = filepath.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("\\", Path.DirectorySeparatorChar.ToString());
                        if (File.Exists(filepath))
                        {
                            using (FileStream fs = new FileStream(filepath, FileMode.Open))
                            {
                                byte[] bytes = new byte[fs.Length];
                                fs.Read(bytes, 0, bytes.Length);
                                fs.Close();
                                return new FileContentResult(bytes, "application/octet-stream");
                            }
                        }
                    }
                    return new ContentResult() { Content = "error:file not exists or no gettype", ContentType = "text/plain" };
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return new ContentResult() { Content = "error:" + ex.Message, ContentType = "text/plain" };
                }
            }
            else if (_type == "PageUpdateList")
            {
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(SysConst.SiteBak)) Directory.CreateDirectory(SysConst.SiteBak);
                string[] dicS = Directory.GetDirectories(SysConst.SiteBak).Where(ss => (ss.EndsWith("_page"))).ToArray();
                List<string> filenameS = new List<string>();
                foreach (string dic in dicS)
                {
                    string pageId = dic.Replace("/", Path.DirectorySeparatorChar.ToString()).Substring(dic.LastIndexOf(Path.DirectorySeparatorChar) + 1).Replace(Path.DirectorySeparatorChar.ToString(), "");
                    string[] s = Directory.GetFiles(dic);
                    string _file = s.OrderByDescending(ss => new FileInfo(ss).Name).ToList().FirstOrDefault<string>();
                    if (_file != null)
                    {
                        _file = _file.Replace("/", Path.DirectorySeparatorChar.ToString()).Substring(_file.LastIndexOf(Path.DirectorySeparatorChar) + 1).Replace(Path.DirectorySeparatorChar.ToString(), "");
                        if (_file.ToLower().EndsWith(".site"))
                        {
                            string[] item = _file.ToLower().Split(new string[] { "_", "." }, StringSplitOptions.None);
                            DateTime dt = DateTime.ParseExact(item[0], "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                            filenameS.Add(pageId + "#" + item[1] + "_" + item[2] + "#" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "#" + _file);
                        }
                    }
                }
                filenameS = filenameS.OrderBy(ss => ss.Split('#')[1]).ToList<string>();
                foreach (string s in filenameS)
                {
                    sb.Append(s + ";");
                }
                filenameS.Clear();
                string restr = (sb.ToString());
                sb.Clear();
                sb = null;
                return new ContentResult() { Content = restr, ContentType = "text/plain" };
            }
            else if (_type == "DllList")
            {
                if (!Directory.Exists(SysConst.RootPath))
                {
                    return new ContentResult() { Content = "", ContentType = "text/plain" };
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    string[] s = Directory.GetFiles(SysConst.RootPath, "*.dll", SearchOption.TopDirectoryOnly);
                    List<string> ssss = s.OrderBy(ss => new FileInfo(ss).Name).ToList();
                    foreach (string filename in ssss)
                    {
                        string _file = filename.Substring(filename.Replace("/", Path.DirectorySeparatorChar.ToString()).LastIndexOf(Path.DirectorySeparatorChar) + 1);
                        sb.Append(_file + ";");
                    }
                    ssss.Clear();
                    string restr = (sb.ToString());
                    sb.Clear();
                    sb = null;
                    return new ContentResult() { Content = restr, ContentType = "text/plain" };
                }
            }
            else if (_type == "DllReflect")
            {
                try
                {
                    string filename = Request.Form["FileName"].FirstOrDefault<string>().Trim();
                    if (!File.Exists(SysConst.RootPath + Path.DirectorySeparatorChar.ToString() + filename))
                    {
                        return new ContentResult() { Content = "error:file does not exist", ContentType = "text/plain" };
                    }
                    return new ContentResult() { Content = adv.CodeReflection(SysConst.RootPath + Path.DirectorySeparatorChar.ToString() + filename), ContentType = "text/plain" };
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return new ContentResult() { Content = "error:" + ex.Message, ContentType = "text/plain" };
                }
            }
            else if (_type == "FrontComPublish")
            {
                string comName = Request.Form["comName"].FirstOrDefault() ?? "";
                string comText = Request.Form["comText"].FirstOrDefault() ?? "";
                string comHtml = Request.Form["comHtml"].FirstOrDefault() ?? "";
                string comText2 = Request.Form["comText2"].FirstOrDefault() ?? "";
                string dir_com = AppDomain.CurrentDomain.BaseDirectory + "" + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "_ft" + Path.DirectorySeparatorChar + "_front" + Path.DirectorySeparatorChar + "com";
                string dir_html = AppDomain.CurrentDomain.BaseDirectory + "" + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "_ft" + Path.DirectorySeparatorChar + "_front";
                Directory.CreateDirectory(dir_com);
                Directory.CreateDirectory(dir_html);
                string com_filename = dir_com + Path.DirectorySeparatorChar + "" + comName.Replace("/", "_") + ".vue";
                string html_filename = dir_html + Path.DirectorySeparatorChar + comName.Replace("/", "_") + "_test.html";
                using (StreamWriter sw = new StreamWriter(com_filename, false, Encoding.UTF8))
                {
                    sw.Write(comText);
                    sw.Flush();
                }
                using (StreamWriter sw = new StreamWriter(html_filename, false, Encoding.UTF8))
                {
                    sw.Write(comHtml);
                    sw.Flush();
                }
                using (StreamWriter sw = new StreamWriter(com_filename + ".com", false, Encoding.UTF8))
                {
                    sw.Write(comText2);
                    sw.Flush();
                }
            }
            else if (_type == "ParaDicRefresh")
            {
                adv.paraDic = null;
                var _paraDic = adv.ParaDic();
                log.Debug("ParaDic Refreshed,Count Is " + _paraDic.Count());
                return new ContentResult() { Content = _paraDic.Count().ToString(), ContentType = "text/plain" };
            }
            else if (_type == "VersionAdd")
            {
                try
                {
                    string version = Request.Form["Version"].FirstOrDefault<string>().Trim();
                    if (version == "" || !decimal.TryParse(version, out decimal _version))
                    {
                        return new ContentResult() { Content = "error:VERSION must be a number", ContentType = "text/plain" };
                    }
                    var dir = SysConst.SiteBak + Path.DirectorySeparatorChar + "version" + Path.DirectorySeparatorChar + version;
                    dir = dir.Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString());
                    if (Directory.Exists(dir))
                    {
                        return new ContentResult() { Content = "error:VERSION " + version + " already exists", ContentType = "text/plain" };
                    }
                    string desc = Request.Form["Desc"].FirstOrDefault<string>().Trim();
                    string time = Request.Form["Time"].FirstOrDefault<string>().Trim();
                    string[] dicS = Directory.GetDirectories(SysConst.SiteBak).Where(ss => (ss.EndsWith("_page"))).ToArray();
                    List<(string pageId, string filePath)> filenameS = new List<(string pageId, string filePath)>();
                    List<(bool isDir, string path)> pubFiles = new List<(bool isDir, string path)>();
                    foreach (string dic in dicS)
                    {
                        string pageId = dic.Replace("/", Path.DirectorySeparatorChar.ToString()).Substring(dic.LastIndexOf(Path.DirectorySeparatorChar) + 1).Replace(Path.DirectorySeparatorChar.ToString(), "");
                        string[] s = Directory.GetFiles(dic);
                        string fileName = null;
                        if (time == "")
                        {
                            fileName = s.OrderByDescending(ss => new FileInfo(ss).Name).ToList().FirstOrDefault<string>();
                        }
                        else
                        {
                            string timeStr = str.GetDateTime(DateTime.Parse(time)).Replace(" ", "").Replace(":", "").Replace("-", "") + "_filename";
                            fileName = s.Where(r => string.Compare(new FileInfo(r).Name, timeStr) <= 0).OrderByDescending(ss => new FileInfo(ss).Name).ToList().FirstOrDefault<string>();
                        }
                        if (fileName != null)
                        {
                            filenameS.Add((pageId, fileName));
                            var dirt = fileName.Substring(0, fileName.Length - 5);
                            if (Directory.Exists(dirt))
                            {
                                var _dir = new DirectoryInfo(dirt);
                                if (_dir.GetDirectories().Length == 1)
                                {
                                    pubFiles.Add((true, _dir.GetDirectories()[0].FullName));
                                }
                                else if (_dir.GetDirectories().Length == 2)
                                {
                                    if (_dir.GetDirectories()[0].Name == "_java" || _dir.GetDirectories()[1].Name == "_java")
                                    {
                                        pubFiles.Add((true, _dir.GetDirectories()[0].FullName));
                                        pubFiles.Add((true, _dir.GetDirectories()[1].FullName));
                                    }
                                }
                                else if (_dir.GetFiles().Length == 1)
                                {
                                    pubFiles.Add((false, _dir.GetFiles()[0].FullName));
                                }
                            }
                        }
                    }
                    StringBuilder sbList = new StringBuilder();
                    var tempDir = SysConst.SiteBak + Path.DirectorySeparatorChar + "version" + Path.DirectorySeparatorChar + "temp";
                    if (Directory.Exists(tempDir))
                    {
                        Directory.Delete(tempDir, true);
                    }
                    Directory.CreateDirectory(tempDir + Path.DirectorySeparatorChar + "site");
                    Directory.CreateDirectory(tempDir + Path.DirectorySeparatorChar + "page");
                    foreach (var item in filenameS)
                    {
                        sbList.AppendLine(new FileInfo(item.filePath).Name + "    " + item.pageId);
                        File.Copy(item.filePath, tempDir + Path.DirectorySeparatorChar + "site" + Path.DirectorySeparatorChar + (new FileInfo(item.filePath).Name));
                    }
                    foreach (var item in pubFiles)
                    {
                        if (item.isDir)
                        {
                            func.CopyFolder(item.path, tempDir + Path.DirectorySeparatorChar + "page",true);
                        }
                        else
                        {
                            File.Copy(item.path, tempDir + Path.DirectorySeparatorChar + "page" + Path.DirectorySeparatorChar + (new FileInfo(item.path).Name));
                        }
                    }
                    Directory.CreateDirectory(dir);
                    using (StreamWriter sr = new StreamWriter(dir + Path.DirectorySeparatorChar + "desc", false, new System.Text.UTF8Encoding(true)))
                    {
                        sr.Write(desc);
                    }
                    using (StreamWriter sr = new StreamWriter(dir + Path.DirectorySeparatorChar + "list", false, new System.Text.UTF8Encoding(true)))
                    {
                        sr.Write(sbList.ToString());
                    }
                    using (StreamWriter sr = new StreamWriter(dir + Path.DirectorySeparatorChar + "time", false, new System.Text.UTF8Encoding(true)))
                    {
                        sr.Write(str.GetDateTime());
                    }
                    var zip = new zipext(dir + Path.DirectorySeparatorChar + "site.zip");
                    var dirSite = new DirectoryInfo(tempDir + Path.DirectorySeparatorChar + "site");
                    zip.ZipFilesAndFolders(dirSite.GetFiles().Select(r => r.FullName).ToArray(), dirSite.GetDirectories().Select(r => r.FullName).ToArray(), "");
                    zip.ZipEnd();
                    zip = new zipext(dir + Path.DirectorySeparatorChar + "page.zip");
                    var dirPage = new DirectoryInfo(tempDir + Path.DirectorySeparatorChar + "page");
                    zip.ZipFilesAndFolders(dirPage.GetFiles().Select(r => r.FullName).ToArray(), dirPage.GetDirectories().Select(r => r.FullName).ToArray(), "");
                    zip.ZipEnd();
                    Directory.Delete(tempDir + Path.DirectorySeparatorChar + "site", true);
                    Directory.Delete(tempDir + Path.DirectorySeparatorChar + "page", true);
                    Directory.Delete(tempDir, true);
                    return new ContentResult() { Content = version, ContentType = "text/plain" };
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return new ContentResult() { Content = "error:" + ex.Message, ContentType = "text/plain" };
                }
            }
            else if (_type == "VersionList")
            {
                var dir = SysConst.SiteBak + Path.DirectorySeparatorChar + "version";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                var versionS = new DirectoryInfo(dir).GetDirectories().Where(r => r.Name != "temp").OrderByDescending(r => decimal.Parse(r.Name));
                StringBuilder sb = new StringBuilder();
                foreach (var ver in versionS)
                {
                    sb.Append("[##]");
                    sb.Append(ver.Name + "(##)");
                    var time = "";
                    if (File.Exists(dir + Path.DirectorySeparatorChar + ver.Name + Path.DirectorySeparatorChar + "time"))
                    {
                        using (StreamReader sr = new StreamReader(dir + Path.DirectorySeparatorChar + ver.Name + Path.DirectorySeparatorChar + "time"))
                        {
                            time = sr.ReadToEnd();
                        }
                    }
                    sb.Append(time + "(##)");
                    var desc = "";
                    if (File.Exists(dir + Path.DirectorySeparatorChar + ver.Name + Path.DirectorySeparatorChar + "desc"))
                    {
                        using (StreamReader sr = new StreamReader(dir + Path.DirectorySeparatorChar + ver.Name + Path.DirectorySeparatorChar + "desc"))
                        {
                            desc = sr.ReadToEnd();
                        }
                    }
                    sb.Append(desc + "(##)");
                    var list = "";
                    if (File.Exists(dir + Path.DirectorySeparatorChar + ver.Name + Path.DirectorySeparatorChar + "list"))
                    {
                        using (StreamReader sr = new StreamReader(dir + Path.DirectorySeparatorChar + ver.Name + Path.DirectorySeparatorChar + "list"))
                        {
                            list = sr.ReadToEnd();
                        }
                    }
                    sb.Append(list);
                }
                return new ContentResult() { Content = sb.ToString(), ContentType = "text/plain" };
            }
            else if (_type == "VersionGet")
            {
                try
                {
                    string getType = Request.Form["GetType"].FirstOrDefault<string>().Trim();
                    string version = Request.Form["Version"].FirstOrDefault<string>().Trim();
                    var dir = SysConst.SiteBak + Path.DirectorySeparatorChar + "version"+ Path.DirectorySeparatorChar+ version;
                    var filepath = dir + Path.DirectorySeparatorChar + (getType == "site" ? "site.zip" : "page.zip");
                    if (File.Exists(filepath))
                    {
                        using (FileStream fs = new FileStream(filepath, FileMode.Open))
                        {
                            byte[] bytes = new byte[fs.Length];
                            fs.Read(bytes, 0, bytes.Length);
                            fs.Close();
                            return new FileContentResult(bytes, "application/octet-stream");
                        }
                    }
                    return new ContentResult() { Content = "error:file not exists or no gettype", ContentType = "text/plain" };
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return new ContentResult() { Content = "error:" + ex.Message, ContentType = "text/plain" };
                }
            }
            return new ContentResult() { Content = "", ContentType = "text/plain" };
        }
        public static string ClientOperation_Old(HttpRequest Request, HttpResponse Response)
        {
            string auth = SiteAuth(Request);
            if (auth != null)
            {
                return ("error:" + auth);
            }
            string _type = Request.Form["type"].FirstOrDefault<string>().Trim();
            if (_type == "PageDel")
            {
                string pageid = Request.Form["PageID"].FirstOrDefault<string>().Trim();
                using (DB db = new DB(SysConst.ConnectionStr_FTDP))
                {
                    db.Open();
                    string sql = "delete from ft_ftdp_apidoc where PageID='" + pageid.D2() + "'";
                    db.ExecSql(sql);
                    sql = "delete from ft_ftdp_apiset where PageID='" + pageid.D2() + "'";
                    db.ExecSql(sql);
                    sql = "delete from ft_ftdp_route where PageID='" + pageid.D2() + "'";
                    db.ExecSql(sql);
                }
            }
            else if (_type == "BackupList")
            {
                string pageid = Request.Form["PageID"].FirstOrDefault<string>().Trim();
                if (!Directory.Exists(SysConst.SiteBak + Path.DirectorySeparatorChar.ToString() + pageid))
                {
                    return "";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    string[] s = Directory.GetFiles(SysConst.SiteBak + Path.DirectorySeparatorChar.ToString() + pageid);
                    List<string> ssss = s.OrderByDescending(ss => new FileInfo(ss).CreationTime).ToList();
                    foreach (string filename in ssss)
                    {
                        string _file = filename.Substring(filename.Replace("/", Path.DirectorySeparatorChar.ToString()).LastIndexOf(Path.DirectorySeparatorChar) + 1);
                        if (_file.ToLower().EndsWith(".site"))
                        {
                            string[] item = _file.ToLower().Split(new string[] { "_", "." }, StringSplitOptions.None);
                            DateTime dt = DateTime.ParseExact(item[0], "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                            sb.Append(item[1] + "_" + item[2] + "#" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "#" + _file + ";");
                        }
                    }
                    ssss.Clear();
                    string restr = (sb.ToString());
                    sb.Clear();
                    sb = null;
                    return restr;
                }
            }
            else if (_type == "BackupGet")
            {
                try
                {
                    string pageid = Request.Form["PageID"].FirstOrDefault<string>().Trim();
                    string filename = Request.Form["FileName"].FirstOrDefault<string>().Trim();
                    if (!File.Exists(SysConst.SiteBak + Path.DirectorySeparatorChar.ToString() + pageid + Path.DirectorySeparatorChar.ToString() + filename))
                    {
                        return ("error:Backup is empty");
                    }
                    using (FileStream fs = new FileStream(SysConst.SiteBak + Path.DirectorySeparatorChar.ToString() + pageid + Path.DirectorySeparatorChar.ToString() + filename, FileMode.Open))
                    {
                        byte[] bytes = new byte[(int)fs.Length];
                        fs.Read(bytes, 0, bytes.Length);
                        fs.Close();
                        //Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                        //Response.ContentType = "application/octet-stream;charset=UTF-8"; ;
                        Response.Headers.Add("Content-Disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(filename));
                        Response.Body.WriteAsync(bytes);
                        Response.Body.FlushAsync();
                    }
                    return "";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return "error:" + ex.Message;
                }
            }
            return "";
        }
        public static string DllUpload(HttpRequest Request)
        {
            string successLabel = @"{ftserver}ok";
            string failedLabel = @"{ftserver}upload failed!";
            string norightLabel = @"{ftserver}no right!";
            try
            {
                string _user = Request.Query["_user"].FirstOrDefault<string>();
                string _passwd = Request.Query["_passwd"].FirstOrDefault<string>();
                string _key = Request.Query["_key"].FirstOrDefault<string>();
                if (string.IsNullOrEmpty(_user) || string.IsNullOrEmpty(_passwd) || string.IsNullOrEmpty(_key))
                {
                    return norightLabel;
                }
                if (Request.Form.Files.Count != 1)
                {
                    return (failedLabel);
                }
                string binpath = System.AppDomain.CurrentDomain.BaseDirectory;

                using (var inputStream = new FileStream(binpath + Path.DirectorySeparatorChar.ToString() + Request.Form.Files[0].FileName, FileMode.Create))
                {
                    // read file to stream
                    Request.Form.Files[0].CopyTo(inputStream);
                    // stream to byte array
                    byte[] array = new byte[inputStream.Length];
                    inputStream.Seek(0, SeekOrigin.Begin);
                    inputStream.Read(array, 0, array.Length);
                    // get file name
                    //string fName = Request.Form.Files[0].FileName;
                }
                //if succcess
                return (successLabel);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return (failedLabel);
            }
        }
        private static string SiteAuth(HttpRequest Request)
        {
            DB db = new DB(SysConst.ConnectionStr_FTDP);
            try
            {
                Model.SiteInfo SiteInfo = Request.ToType<Model.SiteInfo>(typeof(Model.SiteInfo));
                if (SiteInfo._id == null) SiteInfo._id = "";
                if (SiteInfo._user == null) SiteInfo._user = "";
                if (SiteInfo._passwd == null) SiteInfo._passwd = "";
                if (SiteInfo._key == null) SiteInfo._key = "";
                string _sql = "select * from ft_sites where siteid='" + SiteInfo._id.D2() + "'";
                db.Open();
                using (var rdr = db.OpenRecord(_sql))
                {
                    if (rdr.Read())
                    {
                        bool sitekeyok = false;
                        foreach (string RegisterNumber in SysConst.RegisterSiteKeys)
                        {
                            if (string.Compare(SiteInfo._key, RegisterNumber, false) == 0)
                            {
                                sitekeyok = true;
                            }
                        }
                        if (!sitekeyok)
                        {
                            return "SiteKeyNotOK";
                        }
                        if (!SiteInfo._user.Equals(rdr.GetString("sysuser")))
                        {
                            return "UserNotOK";
                        }
                        if (!SiteInfo._passwd.Equals(rdr.GetString("syspasswd")))
                        {
                            return "PasswordNotOK";
                        }
                    }
                    else
                    {
                        return "SiteNotExist";
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "SiteKeyNotOK";
            }
            finally
            {
                db.Close();
            }
        }
        public static void FormOP(HttpContext Context, HttpRequest Request)
        {
            if (!Request.HasFormContentType) return;
            if (!UserTool.IsLogin())
            {
                Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('" + SysConst.NotLogin + "');"));
                return;
            }
            string memname = UserTool.CurUserID();
            //if (!FTFrame.Project.Core.User.IsLogin())
            //{
            //    memname = "0";
            //}


            string siteid = Request.Form["siteid"].FirstOrDefault<string>().Trim();
            string partid = Request.Form["curpartid"].FirstOrDefault<string>();
            int optype = 0;//0,delete,stat;1,copy 2 save 10 csv
            int.TryParse(Request.Form["optype"].FirstOrDefault<string>().Trim(), out optype);

            string now = str.GetDateTime();

            DB db = new DB();
            db.Open();
            try
            {
                string sql = "";
                if (optype == 0)
                {
                    string IdCol = "fid";
                    string StatCol = "stat";
                    string idstatname = Request.FormString("idstatname");
                    if (!string.IsNullOrEmpty(idstatname))
                    {
                        string[] idstatnames = Str.Decode(idstatname).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        IdCol = idstatnames[0].Trim();
                        if (idstatnames.Length > 1) StatCol = idstatnames[1].Trim();
                    }
                    string tablename = Str.Decode(Request.Form["tabletag"].FirstOrDefault<string>());
                    string tablenamedy = tablename + "_dy";
                    string ftformid = Request.Form["ftformid"].FirstOrDefault<string>().Trim();

                    string[] ids = ftformid.Split(',');


                    int stat = -99;
                    if (Request.Form["ftformstat"].FirstOrDefault<string>() != null && !Request.Form["ftformstat"].FirstOrDefault<string>().Equals(""))
                    {
                        int.TryParse(Request.Form["ftformstat"].FirstOrDefault<string>(), out stat);
                    }
                    int flow = -99;
                    if (stat == 0)//先判断删除权限
                    {
                        string d_opid = Str.Decode(Request.Form["d_opid"].FirstOrDefault<string>());
                        string d_code = Str.Decode(Request.Form["d_code"].FirstOrDefault<string>());
                        if (!Interface.Right.HaveOPRight(d_opid))
                        {
                            Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('You do not have permission to operate this item');"));
                            return;
                        }
                        if (d_code.StartsWith("@code("))
                        {
                            string CodeResult = Interface.Code.Get(d_code, Context);
                            if (CodeResult != null)
                            {
                                Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('" + CodeResult + "');"));
                                return;
                            }
                        }
                    }
                    if (stat == -99)//先判断流程操作权限
                    {
                        string f_opid = Str.Decode(Context.Request.Form["f_opid"]);
                        string f_code = Str.Decode(Context.Request.Form["f_code"]);
                        if (!Interface.Right.HaveOPRight(f_opid))
                        {
                            Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('You do not have permission to operate this item');"));
                            return;
                        }
                        if (f_code.StartsWith("@code("))
                        {
                            string CodeResult = Interface.Code.Get(f_code.Replace("@flow@", flow.ToString()), Context);
                            if (CodeResult != null)
                            {
                                Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('" + CodeResult + "');"));
                                return;
                            }
                        }
                    }
                    string CodeFlow = Context.Request.Form["ftformflow"];
                    if (CodeFlow.StartsWith("@code("))
                    {
                        CodeFlow = Interface.Code.Get(CodeFlow, Context);
                        if (CodeFlow == null)
                        {
                            Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2suc('load_" + partid + "(list_curpage_" + partid + ")');"));
                            return;
                        }
                        else if (CodeFlow.StartsWith("EX:"))
                        {
                            Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('" + CodeFlow.Substring(3) + "')"));
                            return;
                        }
                    }
                    string cdn = "";
                    if (CodeFlow.StartsWith("@cdn{"))
                    {
                        CodeFlow = CodeFlow.Substring(5, CodeFlow.Length - 6);
                        CodeFlow = CodeFlow.Split(',')[0];
                        cdn = CodeFlow.Split(',')[1];
                    }
                    if (CodeFlow != null && !CodeFlow.Equals(""))
                    {
                        int.TryParse(CodeFlow, out flow);
                    }
                    string subsql = "";
                    if (stat != -99)
                    {
                        subsql += " stat=" + stat + " ";
                    }
                    if (flow != -99)
                    {
                        if (!subsql.Equals("")) subsql += " and ";
                        subsql += " flow=" + flow + " ";
                    }
                    if (stat == 0 && flow == -99)//删除
                    {
                        string d_membind = Str.Decode(Context.Request.Form["d_membind"]);
                        string d_elecdt = Str.Decode(Context.Request.Form["d_elecdt"]);
                        string d_roledata = Str.Decode(Context.Request.Form["d_roledata"]);
                        string d_rolesession = Str.Decode(Context.Request.Form["d_rolesession"]);
                        string d_authrule = Str.Decode(Context.Request.Form["d_authrule"]);
                        string d_formstat = Str.Decode(Context.Request.Form["d_formstat"]);

                        foreach (string id in ids)
                        {
                            if (id != null && !id.Trim().Equals(""))
                            {
                                string opright = adv.HaveDeleteRight(Context, "@" + tablename, id, d_membind, d_elecdt, d_roledata, d_rolesession, d_authrule, d_formstat, siteid, IdCol);
                                if (opright != null)
                                {
                                    Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai(\"" + opright + "\");"));
                                    return;
                                }
                            }
                        }
                    }
                    if (stat == -99 && flow >= 0)//改变状态
                    {
                        string f_membind = Str.Decode(Context.Request.Form["f_membind"]);
                        string f_elecdt = Str.Decode(Context.Request.Form["f_elecdt"]);
                        string f_roledata = Str.Decode(Context.Request.Form["f_roledata"]);
                        string f_rolesession = Str.Decode(Context.Request.Form["f_rolesession"]);
                        string f_authrule = Str.Decode(Context.Request.Form["f_authrule"]);
                        string f_formstat = Str.Decode(Context.Request.Form["f_formstat"]);

                        foreach (string id in ids)
                        {
                            if (id != null && !id.Trim().Equals(""))
                            {
                                string opright = adv.HaveFlowRight(Context, "@" + tablename, id, f_membind, f_elecdt, f_roledata, f_rolesession, f_authrule, f_formstat, siteid);
                                if (opright != null)
                                {
                                    Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai(\"" + opright + "\");"));
                                    return;
                                }
                            }
                        }
                    }

                    ST st = db.GetTransaction();
                    try
                    {

                        foreach (string id in ids)
                        {
                            if (id != null && !id.Trim().Equals(""))
                            {
                                if (stat == 0)//删除?move
                                {
                                    sql = "update " + tablename + " set " + StatCol + "=0 where " + IdCol + "='" + str.D2DD(id.Trim()) + "' " + cdn;
                                    log.Debug(sql, "[Form Data Del]");
                                    DB.ExecSQL(sql, SysConst.ConnectionStr_FTDP);
                                    sql = "insert into ft_ftdp_formolog(fid,ftype,binddata,fmem,addtime)";
                                    sql += "values('" + str.D2DD(id) + "',99,'" + str.D2DD(tablename) + "','" + str.D2DD(memname) + "','" + now + "')";
                                    DB.ExecSQL(sql, SysConst.ConnectionStr_FTDP);
                                }
                                else
                                {
                                    sql = "update " + tablename + " set " + subsql + " where fid='" + str.D2DD(id.Trim()) + "' " + cdn;
                                    DB.ExecSQL(sql, SysConst.ConnectionStr_FTDP);
                                    sql = "insert into ft_ftdp_formflog(fid,ftype,binddata,fmem,addtime,fvalue,fpos)";
                                    sql += "values('" + str.D2DD(id) + "',99,'" + str.D2DD(tablename) + "','" + str.D2DD(memname) + "','" + now + "'," + flow + ",0)";
                                    DB.ExecSQL(sql, SysConst.ConnectionStr_FTDP);
                                }
                            }
                        }
                        if (stat == 0)
                        {
                            FTFrame.Project.Core.Action.ActionSave(tablename, Context, 2);
                        }
                        else
                        {
                            FTFrame.Project.Core.Action.ActionSave(tablename, Context, 3, flow);
                        }
                        st.Commit();
                        Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2suc('load_" + partid + "(list_curpage_" + partid + ")');"));
                        return;
                    }
                    catch (Exception ex)
                    {
                        st.Rollback();
                        log.Error(ex);
                        Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai(\"" + ex.Message + "\")"));
                        return;
                    }
                }
                else if (optype == 1)//copy
                {
                    string tablename = Str.Decode(Context.Request.Form["tabletag"]);
                    string tablenamedy = tablename + "_dy";
                    string ftformid = Request.Form["ftformid"].FirstOrDefault<string>().Trim();

                    string[] ids = ftformid.Split(',');



                    string c_membind = Str.Decode(Context.Request.Form["c_membind"]);
                    string c_elecdt = Str.Decode(Context.Request.Form["c_elecdt"]);
                    string c_roledata = Str.Decode(Context.Request.Form["c_roledata"]);
                    string c_rolesession = Str.Decode(Context.Request.Form["c_rolesession"]);
                    string c_authrule = Str.Decode(Context.Request.Form["c_authrule"]);
                    string c_formstat = Str.Decode(Context.Request.Form["c_formstat"]);
                    string c_opid = Str.Decode(Context.Request.Form["c_opid"]);
                    string c_code = Str.Decode(Context.Request.Form["c_code"]);
                    if (!Interface.Right.HaveOPRight(c_opid))
                    {
                        Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('You do not have permission to operate this item');"));
                        return;
                    }
                    if (c_code.StartsWith("@code("))
                    {
                        string CodeResult = Interface.Code.Get(c_code, Context);
                        if (CodeResult != null)
                        {
                            Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai('" + CodeResult + "');"));
                            return;
                        }
                    }
                    foreach (string id in ids)
                    {
                        if (id != null && !id.Trim().Equals(""))
                        {
                            string opright = adv.HaveCopyRight(Context, "@" + tablename, id, c_membind, c_elecdt, c_roledata, c_rolesession, c_authrule, c_formstat, siteid);
                            if (opright != null)
                            {
                                Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai(\"" + opright + "\");"));
                                return;
                            }
                        }
                    }

                    DB db2 = new DB();
                    db2.Open();
                    ST st = db.GetTransaction();
                    try
                    {

                        int rows = 0;
                        foreach (string id in ids)
                        {
                            if (id != null && !id.Trim().Equals(""))
                            {
                                string sql1 = null;
                                string sql2 = null;
                                DR dr = null;
                                string newid = str.GetCombID();
                                switch (SysConst.DataBaseType)
                                {
                                    case DataBase.MySql:
                                        sql1 = Sql.TableStructSql(tablename);
                                        dr = db2.OpenRecord(sql1);
                                        sql1 = "";
                                        while (dr.Read())
                                        {
                                            if (!dr.GetString("Field").Trim().Equals("fid"))
                                            {
                                                if (dr.GetString("Field").Trim().Equals("flow"))
                                                {
                                                    sql1 += ",0";
                                                }
                                                else
                                                {
                                                    sql1 += "," + dr.GetString("Field");
                                                }
                                            }
                                        }
                                        dr.Close();
                                        sql1 = "insert into " + tablename + " select '" + newid + "'" + sql1 + " from " + tablename + "  where fid='" + str.D2DD(id) + "'";
                                        sql2 = "insert into " + tablenamedy + " select '" + newid + "',eid,etype,evalue,erate from " + tablenamedy + "  where fid='" + str.D2DD(id) + "'";
                                        break;
                                    case DataBase.SqlServer:
                                        sql1 = Sql.TableStructSql(tablename);
                                        dr = db2.OpenRecord(sql1);
                                        sql1 = "";
                                        while (dr.Read())
                                        {
                                            if (!dr.GetString("ColumnName").Trim().ToLower().Equals("fid"))
                                            {
                                                if (dr.GetString("ColumnName").Trim().ToLower().Equals("flow"))
                                                {
                                                    sql1 += ",0";
                                                }
                                                else
                                                {
                                                    sql1 += "," + dr.GetString("ColumnName");
                                                }
                                            }
                                        }
                                        dr.Close();
                                        sql1 = "insert into " + tablename + " select '" + newid + "'" + sql1 + " from " + tablename + "  where fid='" + str.D2DD(id) + "'";
                                        sql2 = "insert into " + tablenamedy + " select '" + newid + "',eid,etype,evalue,erate from " + tablenamedy + "  where fid='" + str.D2DD(id) + "'";
                                        break;
                                    default: break;
                                }
                                log.Debug(sql1, "[Form Data Copy]");
                                db.ExecSql(sql1, st);
                                db.ExecSql(sql2, st);
                                sql = "insert into ft_ftdp_formolog(fid,ftype,binddata,fmem,addtime)";
                                sql += "values('" + str.D2DD(id) + "',0,'" + str.D2DD(tablename) + "','" + str.D2DD(memname) + "','" + now + "')";
                                DB.ExecSQL(sql, SysConst.ConnectionStr_FTDP);

                                rows++;
                            }
                        }
                        Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2suc('load_" + partid + "(list_curpage_" + partid + ")','Copyed " + rows + " rows.');"));
                        return;
                    }
                    catch (Exception ex)
                    {
                        st.Rollback();
                        log.Error(ex);
                        Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai(\"" + ex.Message + "\")"));
                        return;
                    }
                    finally
                    {
                        db2.Close();
                    }
                }
                else if (optype == 2)
                {
                    string tablename = Context.Request.Form["savetabletag"];
                    if (tablename.StartsWith("@")) tablename = tablename.Substring(1);
                    else tablename = "ft_" + siteid + "_f_" + tablename;
                    string[] ids = null;
                    string checkids = Context.Request.Form["dlcheckradio"];
                    if (checkids == null || checkids.Equals(""))
                    {
                        checkids = Context.Request.Form["dlidsall"];
                    }
                    ids = checkids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] allkeys = Context.Request.Form.Keys.ToArray<string>();
                    ST st = db.GetTransaction();
                    try
                    {
                        foreach (string id in ids)
                        {
                            string cols = "";
                            string vals = "";
                            string sets = "";
                            foreach (string key in allkeys)
                            {
                                if (key.StartsWith("f_" + id + "_"))
                                {
                                    cols += "," + key.Substring(("f_" + id + "_").Length);
                                    vals += ",'" + str.D2DD(Context.Request.Form[key].FirstOrDefault<string>().Trim()) + "'";
                                    sets += "," + key.Substring(("f_" + id + "_").Length) + "='" + str.D2DD(Context.Request.Form[key].FirstOrDefault<string>().Trim()) + "'";
                                }
                            }
                            if (!cols.Equals("")) cols = cols.Substring(1);
                            if (!vals.Equals("")) vals = vals.Substring(1);
                            if (!sets.Equals("")) sets = sets.Substring(1);
                            sql = "select count(*) as ca from " + str.D2DD(tablename) + " where fid='" + str.D2DD(id) + "'";
                            int Count = 0;
                            DR dr = db.OpenRecord(sql, st);
                            if (dr.Read())
                            {
                                Count = int.Parse(dr.GetValue(0).ToString());
                            }
                            dr.Close();
                            sql = null;
                            if (Count == 0)
                            {
                                if (!cols.Equals("") && !vals.Equals(""))
                                {
                                    sql = "insert into " + str.D2DD(tablename) + "(fid," + cols + ")values('" + str.D2DD(id) + "'," + vals + ")";
                                }
                            }
                            else
                            {
                                if (!sets.Equals(""))
                                {
                                    sql = "update " + str.D2DD(tablename) + " set " + sets + " where fid='" + str.D2DD(id) + "'";
                                }
                            }
                            if (sql != null)
                            {
                                log.Debug(sql, "[Form SaveTo]");
                                db.ExecSql(sql, st);
                            }
                        }
                        st.Commit();
                        Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2suc()"));
                        return;
                    }
                    catch (Exception ex)
                    {
                        st.Rollback();
                        log.Error(ex);
                        Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai(\"" + ex.Message + "\")"));
                        return;
                    }
                }
                else if (optype == 10)
                {
                    string exportstr = Context.Request.Form["exportstr"];
                    export.CSV(Context, exportstr);
                }
                else if (optype == 11)
                {
                    Ajax.Control.List(Context);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Context.Response.WriteAsync(str.JavascriptLabel("parent._loading2fai(\"" + ex.Message + "\")"));
                return;
            }
            finally
            {
                db.Close();
            }
        }
        public static string HTMLBody(string body)
        {
            return "<HTML><BODY>" + body + "</BODY></HTML>";
        }
        public static string HTMLBodyJquery(string body)
        {
            return "<HTML><head><script src='" + SysConst.SubPath + "/_ft/_ftres/js/jquery-1.9.1.min.js'></script></head><BODY>" + body + "</BODY></HTML>";
        }
        public static void HTMLBodyJquery_1(HttpContext context)
        {
            context.Response.WriteAsync("<HTML><head><script src='" + SysConst.SubPath + "/_ft/_ftres/js/jquery-1.9.1.min.js'></script></head><BODY>");
        }
        public static void HTMLBodyJquery_2(HttpContext context)
        {
            context.Response.WriteAsync("</BODY></HTML>");
        }
    }
}
