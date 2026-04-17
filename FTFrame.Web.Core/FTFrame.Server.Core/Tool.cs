using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using System.Linq;
using System.IO;
using System.Xml;
using System.Collections;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksum;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Drawing;
using System.Data;
using Newtonsoft.Json;
using System.Web;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Diagnostics;
using FTFrame.Project.Core;
using DocumentFormat.OpenXml.InkML;

namespace FTFrame.Server.Core.Tool
{
    public class export
    {
        public static void Excel(HttpContext context, DataTable dt, string filecaption = null)
        {
            //string filename = System.DateTime.Now.ToString("yyMMdd_hhmmss") + ".xlsx";
            //using (MemoryStream ms =new MemoryStream())
            //{
            //    using (var oxExt = new FTFrame.Server.Core.Office.Excel(ms, "List"))
            //    {
            //        oxExt.Write(dt);
            //    }
            //    byte[] bytes = new byte[ms.Length];
            //    ms.Read(bytes, 0, bytes.Length);
            //    context.Response.ContentType = "application/octet-stream;charset=UTF-8";
            //    context.Response.Headers.Add("Content-Disposition", "attachment; filename=" + func.escape(filename));
            //    context.Response.Body.WriteAsync(bytes);
            //    context.Response.Body.FlushAsync();
            //}
            string time = System.DateTime.Now.ToString("yyMMdd_hhmmss");
            string filename = time + ".xlsx";
            string filepath = SysConst.RootPath + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "_ft" + Path.DirectorySeparatorChar + "_tempfiles";
            Directory.CreateDirectory(filepath);
            using (var oxExt = new FTFrame.Server.Core.Office.Excel(filepath + Path.DirectorySeparatorChar + filename, string.IsNullOrEmpty(filecaption) ? "List" : filecaption))
            {
                oxExt.Write(dt);
            }
            adv.DownloadFile(context, filepath + Path.DirectorySeparatorChar + filename, string.IsNullOrEmpty(filecaption) ? time : filecaption);
            System.IO.File.Delete(filepath + Path.DirectorySeparatorChar + filename);
        }
        /*
        public static void Excel(HttpContext context, DataTable dt)
        {
            log.Error("11");
            string filename = System.DateTime.Now.ToString("yyyyMMdd_hhmm") + ".xlsx";
            FileInfo fi = new FileInfo(SysConst.RootPath+"\\wwwroot\\_ft\\"+ filename);
            using (ExcelPackage package = new ExcelPackage(fi))
            {
                log.Error("22");
                var worksheet = package.Workbook.Worksheets.Add("list");
                int colLen = dt.Columns.Count;
                for (int i = 0; i < colLen; i++)
                {
                    worksheet.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                }
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int i = 0; i < colLen; i++)
                    {
                        worksheet.Cell(row + 2, i + 1).Value = dt.Rows[row][i].ToString();
                    }
                }
                log.Error("33");
                worksheet.Column(1).Width = 200;
                //package.SaveAs(new FileInfo(@"E:\alice\01_erp\_ftfiles\aa.xlsx"));
                package.Save();
                log.Error("44");
                adv.DownloadFile(context, fi.FullName);
                log.Error("55");
                //byte[] bytes = new byte[fs.Length];
                //fs.Read(bytes, 0, bytes.Length);
                //// 设置当前流的位置为流的开始
                //fs.Seek(0, SeekOrigin.Begin);
                //log.Error("55");
                //context.Response.Clear();
                //context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //context.Response.Headers.Add("Content-Disposition", "attachment;filename=export" + System.DateTime.Now.ToString("_yyMMdd_hhmm") + ".xlsx");
                //context.Response.Body.Write(bytes);
                //log.Error("66");
            }
        }*/
        public static void ExcelSave(DataTable dt, string FileName)
        {
            using (var oxExt = new FTFrame.Server.Core.Office.Excel(FileName, "List"))
            {
                oxExt.Write(dt);
            }
        }
        public static void CSV(HttpContext context, string str)
        {
            context.Response.Clear();
            context.Response.ContentType = "application/ms-excel";
            context.Response.WriteAsync(str);
        }
        public static void File(HttpContext context, string filename, string filepath)
        {
            adv.DownloadFile(context, filepath, filename);
        }

    }
    public class func
    {
        public static string escape(string str)
        {
            return Uri.EscapeDataString(str);
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in str)
                {
                    sb.Append((Char.IsLetterOrDigit(c)
                    || c == '-' || c == '_' || c == '\\'
                    || c == '/' || c == '.') ? c.ToString() : Uri.HexEscape(c));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError("str is " + str,"");
                FTFrame.Project.Core.Api.LogError(ex);
                throw ex;
            }
        }
        public static string SQLColSafe(string s)
        {
            return s.Replace("'", "").Replace("\"", "").Replace(";", "").Replace(",", "");
        }
        public static bool SQLSelectSafe(string s)
        {
            s = s.ToLower();
            return s.IndexOf("insert") < 0 && s.IndexOf("delete") < 0 && s.IndexOf("update") < 0 && s.IndexOf("drop") < 0 && s.IndexOf("alter") < 0 && s.IndexOf("create") < 0 && s.IndexOf("exec") < 0 && s.IndexOf("truncate") < 0;
        }
        public static string DicToStr(Dictionary<string, string> dic)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in dic.Keys)
            {
                sb.Append(key + "[#KEY#]" + dic[key] + "[#ITEM#]");
            }
            return sb.ToString();
        }
        public static Dictionary<string, string> StrToDic(string str)
        {
            var dic = new Dictionary<string, string>();
            var items = str.Split(new String[] { "[#ITEM#]" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items)
            {
                var _item = item.Split("[#KEY#]");
                dic.Add(_item[0], _item[1]);
            }
            return dic;
        }
        public static void CopyFolder(string sourceFolder, string destFolder, bool isCopyRoot = false)
        {
            try
            {
                //如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }
                if (isCopyRoot)
                {
                    var newDestFolder = destFolder + Path.DirectorySeparatorChar + (new DirectoryInfo(sourceFolder).Name);
                    CopyFolder(sourceFolder, newDestFolder);
                    return;
                }
                //得到原文件根目录下的所有文件
                string[] files = System.IO.Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    System.IO.File.Copy(file, dest, true);//复制文件
                }
                //得到原文件根目录下的所有文件夹
                string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    CopyFolder(folder, dest);//构建目标路径,递归复制文件
                }
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError(ex);
            }

        }
        public static string ExecuteCommandFile(string FileName ,string Arguments ,string WorkingDirectory ,int milliseconds = 10000)
        {
            var rValue = "";
            using (System.Diagnostics.Process p = new System.Diagnostics.Process())
            {
                p.StartInfo.FileName = FileName;
                p.StartInfo.Arguments = Arguments;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.WorkingDirectory = WorkingDirectory;
                p.Start();
                p.WaitForExit(milliseconds);
                rValue = p.StandardOutput.ReadToEnd();
            }
            return rValue;
        }
        public static string ExecuteCommandBash(string Input, int milliseconds = 0)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo("/bin/bash", "")
            };
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.EnableRaisingEvents = true;
            process.Start();
            process.StandardInput.WriteLine(Input);
            process.StandardInput.Close();
            var cpuInfo = process.StandardOutput.ReadToEnd();
            var cpuInfo2 = process.StandardError.ReadToEnd();
            if(milliseconds==0)process.WaitForExit();
            else process.WaitForExit(milliseconds);
            process.Dispose();
            //var lines2 = cpuInfo.Split('\n');
            //foreach (var item in lines2)
            //{
            //    log.Debug("行记录A：" + item);
            //}
            var lines2 = cpuInfo2.Split(new string[] { "\n" , "\r" },StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in lines2)
            {
                FTFrame.Project.Core.Api.LogError("Bash Error：" + item, "");
            }
            return cpuInfo;
        }
    }
    public class comp
    {
        public static string DataOPAdvVal(HttpContext context, DB db, ST st, string adv, string newfid, int rowindex, Hashtable AddHT, Dictionary<string, Dictionary<string, string>> TableModDic, Dictionary<string, object> reqDic, string oriValue = null)
        {
            //@sql:    @code()      _newfid_   @newfid@  其他val
            try
            {
                if (adv == "_empty_"|| adv == "@empty@") return "";
                if (adv == "_null_"|| adv == "@null@") return "[FTNULL]";
                adv = adv.Replace("_newfid_", newfid).Replace("@newfid@", newfid);
                adv = adv.Replace("_rowindex_", rowindex.ToString()).Replace("@rowindex@", rowindex.ToString());
                if (adv.StartsWith("@code("))
                {
                    return Interface.Code.Get(adv, reqDic,new Dictionary<string, string>() {
                        { "_val_", oriValue ?? ""},{ "@val@", oriValue ?? ""}
                    }, context).ToString();
                }
                else
                {
                    adv = adv.Replace("_val_", oriValue ?? "").Replace("@val@", oriValue ?? "");
                    if (adv.StartsWith("@sql:"))
                    {
                        string sql = adv.Substring(5);
                        FTFrame.Project.Core.Api.LogDebug(sql, "[DataOP,@SQL]");
                        using (DR dr = db.OpenRecord(sql, st))
                        {
                            if (dr.Read())
                            {
                                return dr.GetStringForceNoNULL(0);
                            }
                            else return "";
                        }
                    }
                    else if (adv.StartsWith("@from("))
                    {
                        //@from(@ef_meeting_content_info.should_arrive_number)
                        string dataop_tabletag = adv.Trim().Replace("@from(", "").Replace(")", "");
                        string dataop_table = dataop_tabletag.Split('.')[0];
                        dataop_table = dataop_table.StartsWith("@") ? dataop_table.Substring(1) : ("ft_site_f_" + dataop_table);
                        string dataop_col = dataop_tabletag.Split('.')[1];
                        if (AddHT != null && AddHT.ContainsKey(dataop_table) && ((Hashtable)AddHT[dataop_table]).ContainsKey(dataop_col))
                        {
                            return ((Hashtable)AddHT[dataop_table])[dataop_col].ToString();
                        }
                        else if (TableModDic != null && TableModDic.ContainsKey(dataop_table) && TableModDic[dataop_table].ContainsKey(dataop_col))
                        {
                            return TableModDic[dataop_table][dataop_col];
                        }
                        else return "";
                    }
                    else return adv;
                }
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError(ex);
                return ex.Message;
            }
        }
        public static bool List_IsColumnOpen(HttpContext context, string rowcdt, Dictionary<string, object> reqDic)
        {
            if (rowcdt == null || rowcdt.Trim().Equals("")) return true;
            if (rowcdt.StartsWith("@code("))
            {
                string val = Interface.Code.GetObject(rowcdt, reqDic, context).ToString();
                return val == null || val == "1";
            }
            string paraleft = "";
            string pararight = "";
            int index = 0;
            index = rowcdt.IndexOf("==");
            if (index > 0)
            {
                paraleft = rowcdt.Substring(0, index).Replace("\"", "");
                pararight = rowcdt.Substring(index + 2).Replace("\"", "");
                paraleft = adv.GetSpecialBase(context, paraleft, null);
                pararight = adv.GetSpecialBase(context, pararight, null);
                return paraleft.Equals(pararight);
            }
            else
            {
                index = rowcdt.IndexOf("!=");
                if (index > 0)
                {
                    paraleft = rowcdt.Substring(0, index).Replace("\"", "");
                    pararight = rowcdt.Substring(index + 2).Replace("\"", "");
                    paraleft = adv.GetSpecialBase(context, paraleft, null);
                    pararight = adv.GetSpecialBase(context, pararight, null);
                    return !paraleft.Equals(pararight);
                }
                else
                {
                    index = rowcdt.IndexOf("=?");
                    if (index > 0)
                    {
                        paraleft = rowcdt.Substring(0, index).Replace("\"", "");
                        pararight = rowcdt.Substring(index + 2).Replace("\"", "");
                        paraleft = adv.GetSpecialBase(context, paraleft, null);
                        pararight = adv.GetSpecialBase(context, pararight, null);
                        return paraleft.Contains(pararight);
                    }
                    else
                    {
                        index = rowcdt.IndexOf("!?");
                        if (index > 0)
                        {
                            paraleft = rowcdt.Substring(0, index).Replace("\"", "");
                            pararight = rowcdt.Substring(index + 2).Replace("\"", "");
                            paraleft = adv.GetSpecialBase(context, paraleft, null);
                            pararight = adv.GetSpecialBase(context, pararight, null);
                            return !paraleft.Contains(pararight);
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }
    }
    public class zip
    {
        public zip()
        {
            ZipStrings.CodePage = System.Text.Encoding.UTF8.CodePage;
        }
        public void ZipFile(string FileToZip, string ZipedFile, int CompressionLevel, int BlockSize)
        {
            //如果文件没有找到，则报错
            if (!System.IO.File.Exists(FileToZip))
            {
                throw new System.IO.FileNotFoundException("The specified file " + FileToZip + " could not be found. Zipping aborderd");
            }

            System.IO.FileStream StreamToZip = new System.IO.FileStream(FileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.FileStream ZipFile = System.IO.File.Create(ZipedFile);
            ZipOutputStream ZipStream = new ZipOutputStream(ZipFile);
            ZipEntry ZipEntry = new ZipEntry("ZippedFile");
            ZipStream.PutNextEntry(ZipEntry);
            ZipStream.SetLevel(CompressionLevel);
            byte[] buffer = new byte[BlockSize];
            System.Int32 size = StreamToZip.Read(buffer, 0, buffer.Length);
            ZipStream.Write(buffer, 0, size);
            try
            {
                while (size < StreamToZip.Length)
                {
                    int sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                    ZipStream.Write(buffer, 0, sizeRead);
                    size += sizeRead;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            ZipStream.Finish();
            ZipStream.Close();
            StreamToZip.Close();
        }

        public void ZipFileMain(string inputpath, string zipfile)
        {
            string[] filenames = Directory.GetFiles(inputpath);

            Crc32 crc = new Crc32();
            ZipOutputStream s = new ZipOutputStream(File.Create(zipfile));

            s.SetLevel(6); // 0 - store only to 9 - means best compression

            foreach (string file in filenames)
            {
                //打开压缩文件
                FileStream fs = File.OpenRead(file);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                ZipEntry entry = new ZipEntry(file);

                entry.DateTime = DateTime.Now;

                // set Size and the crc, because the information
                // about the size and crc should be stored in the header
                // if it is not set it is automatically written in the footer.
                // (in this case size == crc == -1 in the header)
                // Some ZIP programs have problems with zip files that don't store
                // the size and crc in the header.
                entry.Size = fs.Length;
                fs.Close();

                crc.Reset();
                crc.Update(buffer);

                entry.Crc = crc.Value;

                s.PutNextEntry(entry);

                s.Write(buffer, 0, buffer.Length);

            }

            s.Finish();
            s.Close();
        }
        public void UnZip(string zipfile, string ouputpath)
        {
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipfile)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {

                    string directoryName = Path.GetDirectoryName(ouputpath);
                    string fileName = Path.GetFileName(theEntry.Name);
                    DateTime dt = theEntry.DateTime;

                    //生成解压目录
                    //Directory.CreateDirectory(directoryName);
                    if (fileName != String.Empty)
                    {
                        //解压文件到指定的目录
                        string filename = ouputpath + theEntry.Name;
                        filename = filename.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("\\", Path.DirectorySeparatorChar.ToString());

                        Directory.CreateDirectory(Path.GetDirectoryName(filename));
                        FileStream streamWriter = File.Create(filename);


                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            try
                            {
                                size = s.Read(data, 0, data.Length);
                            }
                            catch
                            {
                                break;
                            }
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }

                        streamWriter.Close();
                        new FileInfo(filename).LastWriteTime = dt;
                    }
                }
            }
        }

    }
    public class zipext
    {
        private ZipOutputStream s;
        private Crc32 crc;
        public zipext(string ZipFileName)
        {
            ZipStrings.CodePage = System.Text.Encoding.UTF8.CodePage;
            crc = new Crc32();
            s = new ZipOutputStream(File.Create(ZipFileName));
            s.SetLevel(6); // 0 - store only to 9 - means best compression
        }
        public void ZipEnd()
        {
            s.Finish();
            s.Close();
            s.Dispose();
        }
        public void ZipFilesAndFolders(string[] filenames, string[] folders, string headfolder)
        {

            foreach (string file in filenames)
            {
                if (file == null || file.Equals("")) continue;
                //打开压缩文件
                FileStream fs = File.OpenRead(file);

                FileInfo fi = new FileInfo(file);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                ZipEntry entry = new ZipEntry(headfolder + fi.Name);


                //entry.DateTime = DateTime.Now;
                entry.DateTime = fi.LastWriteTime;
                fi = null;

                // set Size and the crc, because the information
                // about the size and crc should be stored in the header
                // if it is not set it is automatically written in the footer.
                // (in this case size == crc == -1 in the header)
                // Some ZIP programs have problems with zip files that don't store
                // the size and crc in the header.
                entry.Size = fs.Length;
                fs.Close();

                crc.Reset();
                crc.Update(buffer);

                entry.Crc = crc.Value;

                s.PutNextEntry(entry);

                s.Write(buffer, 0, buffer.Length);

            }
            foreach (string resfolder in folders)
            {

                if (Directory.Exists(resfolder))
                {
                    DirectoryInfo dio = new DirectoryInfo(resfolder);

                    string[] filenamessub = Directory.GetFiles(resfolder);
                    string[] foldernamessub = Directory.GetDirectories(resfolder);

                    string headfolder2 = headfolder + dio.Name + Path.DirectorySeparatorChar;
                    ZipFilesAndFolders(filenamessub, foldernamessub, headfolder2);
                }
            }


        }
    }
    public class adv
    {
        /// <summary>
        /// Cdn为null则无条件，Cdn长度为1则为ELSE，其他为IF
        /// </summary>
        public static Dictionary<string, List<(string[] Cdn, string ParaValue)>> paraDic = null;
        public static Dictionary<string, List<(string[] Cdn, string ParaValue)>> ParaDic()
        {
            if (paraDic != null) return paraDic;
            paraDic = new Dictionary<string, List<(string[] Cdn, string ParaValue)>>();
            try
            {
                using (DB db = new DB(SysConst.ConnectionStr_FTDP))
                {
                    string sql = "select paraname,paravalue from ft_ftdp_para where stat=1 order by paraname";
                    using (DR dr = db.OpenRecord(sql))
                    {
                        while (dr.Read())
                        {
                            string paraname = dr.GetString(0);
                            string paravalue = dr.GetString(1);
                            if (paraname.StartsWith("Front_")) continue;
                            if (!paravalue.StartsWith("[#IF#]"))
                            {
                                paraDic.Add(paraname, new List<(string[] Cdn, string ParaValue)>() { (null, paravalue) });
                            }
                            else
                            {
                                var list = new List<(string[] Cdn, string ParaValue)>();
                                string[] lines = paravalue.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                                string curval = "";
                                List<string> vals = new List<string>();
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    if (lines[i].StartsWith("[#IF#]") || lines[i].StartsWith("[#ELSE#]"))
                                    {
                                        if (i > 0)
                                        {
                                            vals.Add(curval.Trim());
                                            curval = "";
                                        }
                                        vals.Add(lines[i].Trim());
                                    }
                                    else
                                    {
                                        curval += lines[i] + Environment.NewLine;
                                    }
                                }
                                vals.Add(curval.Trim());
                                for (int i = 0; i < (vals.Count / 2); i++)
                                {
                                    if (vals[2 * i].StartsWith("[#IF#]"))
                                    {
                                        var cdnItem = vals[2 * i].Substring("[#IF#]".Length).Split(new string[] { "[##]" }, StringSplitOptions.None);
                                        list.Add((cdnItem, vals[2 * i + 1]));
                                    }
                                    else if (vals[2 * i].StartsWith("[#ELSE#]"))
                                    {
                                        list.Add((new string[] { "[#ELSE#]" }, vals[2 * i + 1]));
                                    }
                                }
                                paraDic.Add(paraname, list);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError(ex);
            }
            return paraDic;
        }
        public static string UploadFile(HttpContext context, string elename)
        {
            if (!context.Request.HasFormContentType) return "";
            string revalue = "";
            IFormFile pfile = context.Request.Form.Files[elename];
            if (pfile == null) return revalue;
            else
            {
                string dir = str.GetYearMonth();
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "" + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "_ftfiles" + Path.DirectorySeparatorChar + "" + dir))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "" + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "_ftfiles" + Path.DirectorySeparatorChar + "" + dir);
                }
                string basefilename = (pfile.FileName.LastIndexOf("\\") >= 0) ? pfile.FileName.Substring(pfile.FileName.LastIndexOf("\\") + 1) : pfile.FileName.Substring(pfile.FileName.LastIndexOf("/") + 1);
                if (basefilename == null || basefilename.Trim().Equals(""))
                {
                    revalue = "";
                }
                else
                {
                    string newfilename = "" + Path.DirectorySeparatorChar + "_ftfiles" + Path.DirectorySeparatorChar + "" + dir + "" + Path.DirectorySeparatorChar + "" + str.GetDateTime().Replace("-", "").Replace(" ", "").Replace(":", "") + "_" + basefilename;
                    if (pfile.Length > 0)
                    {
                        using (var inputStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "" + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "" + newfilename, FileMode.Create))
                        {
                            // read file to stream
                            pfile.CopyTo(inputStream);
                            // stream to byte array
                            byte[] array = new byte[inputStream.Length];
                            inputStream.Seek(0, SeekOrigin.Begin);
                            inputStream.Read(array, 0, array.Length);
                            // get file name
                            //string fName = formFile.FileName;
                        }
                    }
                    revalue = newfilename;
                }
            }
            return revalue;
        }
        public static void DownloadFile(HttpContext context, string filepath, string filecaption)
        {
            filepath = filepath.Replace("/", Path.DirectorySeparatorChar.ToString());
            if (!File.Exists(filepath)) return;
            ///if(string.IsNullOrEmpty(filecaption)) filecaption = filepath.Substring(filepath.LastIndexOf(Path.DirectorySeparatorChar) +1);
            using (FileStream fs = new FileStream(filepath, FileMode.Open))
            {
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                context.Response.ContentType = "application/octet-stream;charset=UTF-8";
                context.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                context.Response.Headers.Add("Content-Disposition", "attachment; filename=" + func.escape(filecaption + ".xlsx"));
                context.Response.Body.WriteAsync(bytes);
                context.Response.Body.FlushAsync();
            }
        }
        public static string PostStrSafe(string oriStr, string key)
        {
            if (key.EndsWith("_ishtml") || oriStr == null) return oriStr;
            else return FTFrame.Tool.str.GetSafeCode(oriStr);
        }
        public static string CodeReflection(string filename)
        {
            StringBuilder sb = new StringBuilder();
            //filename = @"E:\alice\netcore\publish\FTFrame.DB.Core.dll";
            byte[] filedata = File.ReadAllBytes(filename);
            //var assembly = Assembly.LoadFrom(filename);
            var assembly = Assembly.Load(filedata);// Assembly.LoadFile(openFile.FileName);// 
            string asseblyName = assembly.GetName().Name;
            var assemblyNode = new Model.TreeNode(asseblyName);
            //module
            var modules = assembly.GetModules();
            //foreach (var module in modules)
            foreach (var module in modules)
            {   //module view 
                var moduleNode = new Model.TreeNode(module.Name);
                assemblyNode.Nodes.Add(moduleNode);
                //reference 
                var referenceNode = new Model.TreeNode("Reference");
                moduleNode.Nodes.Add(referenceNode);
                var refers = assembly.GetReferencedAssemblies();
                foreach (var refer in refers)
                {
                    var referNode = new Model.TreeNode(refer.Name);
                    referenceNode.Nodes.Add(referNode);
                }
                //types
                Type[] typearr = null;
                try
                {
                    typearr = module.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    typearr = ex.Types;
                }
                //字典 分离键值对key-value(mul)
                var typeGroup = new System.Collections.Generic.Dictionary<String, List<Type>>();
                foreach (var type in typearr)
                {
                    if (type == null) continue;
                    string np = null;
                    try
                    {
                        np = type.Namespace;
                    }
                    catch { }

                    if (String.IsNullOrEmpty(np))
                    {
                        np = "-";
                    }
                    if (typeGroup.ContainsKey(np))
                    {
                        typeGroup[np].Add(type);
                        continue;

                    }
                    var list = new List<Type>();
                    list.Add(type);
                    typeGroup.Add(np, list);
                }
                //显示namespace和class
                foreach (var typeItem in typeGroup)
                {
                    var nameSpaceNode = new Model.TreeNode(typeItem.Key);
                    moduleNode.Nodes.Add(nameSpaceNode);

                    foreach (var type in typeItem.Value)
                    {
                        var classNode = new Model.TreeNode(type.Name);
                        nameSpaceNode.Nodes.Add(classNode);

                        //construc
                        var constructInfos = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Instance);

                        ParameterInfo[] consParameters = null;
                        String parsList = String.Empty;
                        try
                        {


                            foreach (var cons in constructInfos)
                            {
                                consParameters = cons.GetParameters();
                                var conlist = new List<String>();
                                foreach (var consParameter in consParameters)
                                {
                                    conlist.Add(consParameter.ParameterType.Name);
                                }
                                parsList = String.Join(" , ", conlist.ToArray());

                                var constructNode = new Model.TreeNode(cons.Name + "(" + parsList + ")");
                                classNode.Nodes.Add(constructNode);
                            }
                        }
                        catch { }
                        //method
                        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                        foreach (var method in methods)
                        {
                            var methodName = method.Name;
                            if (method.IsSpecialName)
                            {
                                continue;
                            }
                            string paramStr = string.Empty;
                            String methodRT = null;
                            bool ParaOK = true;
                            try
                            {
                                var parameters = method.GetParameters();
                                var paraList = new List<String>();
                                foreach (var parameter in parameters)
                                {
                                    paraList.Add(parameter.ParameterType.Name);
                                    if (",String,Int16,Int32,Int64,Decimal,Single,Double,Boolean,HttpContext,".IndexOf("," + parameter.ParameterType.Name + ",") < 0)
                                    {
                                        ParaOK = false;
                                    }
                                }
                                paramStr = String.Join(",", paraList.ToArray());
                                methodRT = method.ReturnType.Name;
                            }
                            catch
                            {
                                paramStr = "UnKnow";
                                methodRT = "methodRT_UnKnow";
                            }

                            var methodNode = new Model.TreeNode(method.Name + "(" + paramStr + ")" + " : " + methodRT);
                            classNode.Nodes.Add(methodNode);
                            if (ParaOK && method.IsPublic)
                            {
                                bool returnBaseTypeOK = (method.ReturnType.Name == "String" || method.ReturnType.Name == "Void");
                                bool returnListTypeOK = (method.ReturnType.Name.StartsWith("List`1"));
                                bool returnDyvalueOK = (method.ReturnType.Name.StartsWith("Dictionary`2"));
                                if (returnBaseTypeOK || returnListTypeOK || returnDyvalueOK)
                                {
                                    //methodNode.Tag = new Tuple<string,string, string, string, string, bool>(method.Name,method.ReturnType.Name, typeItem.Key + "." + type.Name, paramStr, asseblyName, method.IsStatic);
                                    if (method.IsStatic)
                                    {
                                        methodNode.Tag = typeItem.Key + "." + type.Name + "." + method.Name + "(" + paramStr + "):" + method.ReturnType.Name + ":" + asseblyName;
                                    }
                                    else
                                    {
                                        methodNode.Tag = "new " + typeItem.Key + "." + type.Name + "()." + method.Name + "(" + paramStr + "):" + method.ReturnType.Name + ":" + asseblyName;
                                    }
                                    if (returnBaseTypeOK) methodNode.ForeColor = System.Drawing.Color.Blue;
                                    else if (returnListTypeOK || returnDyvalueOK) methodNode.ForeColor = System.Drawing.Color.Red;
                                }
                            }
                        }
                        //property
                        var proInfos = type.GetProperties();

                        foreach (var proinfo in proInfos)
                        {
                            Model.TreeNode proNode = null;
                            try
                            {
                                proNode = new Model.TreeNode(proinfo.Name + " : " + proinfo.PropertyType.Name);
                                classNode.Nodes.Add(proNode);
                            }
                            catch
                            {
                                proNode = new Model.TreeNode(proinfo.Name + " : " + "UnKnow");
                                classNode.Nodes.Add(proNode);
                            }

                            MethodInfo promethods = null;
                            ParameterInfo[] proParameters = null;
                            List<String> proList = null;
                            String parsStr2 = String.Empty;
                            String prt = null;
                            if (proinfo.CanRead)
                            {
                                try
                                {
                                    promethods = proinfo.GetGetMethod();
                                    proParameters = promethods.GetParameters();
                                    proList = new List<String>();
                                    foreach (var proParameter in proParameters)
                                    {
                                        proList.Add(proParameter.ParameterType.Name);
                                    }
                                    parsStr2 = string.Join(", ", proList.ToArray());
                                    prt = promethods.ReturnType.Name;
                                }
                                catch { }
                                var proInfoNode = new Model.TreeNode(promethods.Name + "(" + parsStr2 + ")" + prt);
                                proNode.Nodes.Add(proInfoNode);
                            }
                            if (proinfo.CanWrite)
                            {
                                try
                                {

                                    promethods = proinfo.GetGetMethod();
                                    proParameters = promethods.GetParameters();
                                    proList = new List<String>();
                                    foreach (var proParameter in proParameters)
                                    {
                                        proList.Add(proParameter.ParameterType.Name);
                                    }
                                }
                                catch { }
                                var proInfoNode = new Model.TreeNode(promethods.Name + "(" + string.Join(", ", proList.ToArray()) + ")" + promethods.ReturnType.Name);
                                proNode.Nodes.Add(proInfoNode);
                            }

                            //filds 
                            var filds = type.GetFields(BindingFlags.Public |
                                                       BindingFlags.Static |
                                                       BindingFlags.NonPublic |
                                                       BindingFlags.Instance |
                                                       BindingFlags.DeclaredOnly);
                            try
                            {
                                foreach (var fildsInfo in filds)
                                {
                                    var fildNode = new Model.TreeNode(fildsInfo.Name + ": " + fildsInfo.FieldType.Name);
                                    classNode.Nodes.Add(fildNode);
                                }
                            }
                            catch { }
                        }


                    }
                }
            }

            sb.Append(assemblyNode.Text + "[#0:]");
            foreach (Model.TreeNode node1 in assemblyNode.Nodes)
            {
                sb.Append((Node2ColorStr(node1)) + (node1.Tag == null ? "" : ("(tag)" + node1.Tag + "(tag)")) + node1.Text + "[#1:]");
                foreach (Model.TreeNode node2 in node1.Nodes)
                {
                    sb.Append((Node2ColorStr(node2)) + (node2.Tag == null ? "" : ("(tag)" + node2.Tag + "(tag)")) + node2.Text + "[#2:]");
                    foreach (Model.TreeNode node3 in node2.Nodes)
                    {
                        sb.Append((Node2ColorStr(node3)) + (node3.Tag == null ? "" : ("(tag)" + node3.Tag + "(tag)")) + node3.Text + "[#3:]");
                        foreach (Model.TreeNode node4 in node3.Nodes)
                        {
                            sb.Append((Node2ColorStr(node4)) + (node4.Tag == null ? "" : ("(tag)" + node4.Tag + "(tag)")) + node4.Text + "[#4:]");
                            foreach (Model.TreeNode node5 in node4.Nodes)
                            {
                                sb.Append((Node2ColorStr(node5)) + (node5.Tag == null ? "" : ("(tag)" + node5.Tag + "(tag)")) + node5.Text + "[#5:]");
                                foreach (Model.TreeNode node6 in node5.Nodes)
                                {

                                }
                                sb.Append("[#5;]");
                            }
                            sb.Append("[#4;]");
                        }
                        sb.Append("[#3;]");
                    }
                    sb.Append("[#2;]");
                }
                sb.Append("[#1;]");
            }
            string restr = sb.ToString();
            sb.Clear();
            return restr;
            string Node2ColorStr(Model.TreeNode node)
            {
                if (node.ForeColor == System.Drawing.Color.Blue) return "(blue)";
                else if (node.ForeColor == System.Drawing.Color.Red) return "(red2)";
                else return "";
            }
        }
        public static string CodePattern(HttpContext context, string oriInput, Dictionary<string, string> CodeKVDic = null, Dictionary<string, string> CodeParaDic=null)
        {
            return CodePattern(context, oriInput, null, CodeKVDic, CodeParaDic);
        }
        public static string CodePattern(HttpContext context, string oriInput, Dictionary<string, object> reqDic, Dictionary<string, string> CodeKVDic = null, Dictionary<string, string> CodeParaDic = null)
        {
            if (oriInput == null) return null;
            Regex r = new Regex(@"@code\([^(\)@)]*\)");
            var mc = r.Matches(oriInput);
            foreach (string p in mc.Select(x => x.Value).Distinct())
            {
                string pattern = p;
                string codeVal = "";
                if (CodeKVDic != null)
                {
                    if (!CodeKVDic.ContainsKey(p)) CodeKVDic.Add(p, (CodeParaDic==null|| CodeParaDic.Count==0)?Interface.Code.Get(pattern, reqDic, context): Interface.Code.Get(pattern, reqDic, CodeParaDic, context));
                    codeVal = CodeKVDic[p];
                }
                else codeVal = (CodeParaDic == null || CodeParaDic.Count == 0) ? Interface.Code.Get(pattern, reqDic, context) : Interface.Code.Get(pattern, reqDic, CodeParaDic, context);
                oriInput = oriInput.Replace(p, codeVal);
            }
            r = new Regex(@"@enum\([^(\)@)]*\)");
            mc = r.Matches(oriInput);
            foreach (string p in mc.Select(x => x.Value).Distinct())
            {
                string pattern = p;
                //oriInput = oriInput.Replace(p, Interface.Code.GetEnum(pattern));
                oriInput = oriInput.Replace(p, EnumPattern(pattern));
            }
            r = new Regex(@"@dic\([^(\)@)]*\)");
            mc = r.Matches(oriInput);
            foreach (string p in mc.Select(x => x.Value).Distinct())
            {
                string pattern = p;
                var item = pattern.Split(new char[] { '(', '.', ')' });
                if (item.Length > 2) oriInput = oriInput.Replace(p, item[2]);
            }
            return oriInput;
        }
        public static string SqlPattern(string oriInput, HttpContext context)
        {
            return SqlPattern(oriInput,null, context);
        }
        public static string SqlPattern(string oriInput, Dictionary<string, object> reqDic,HttpContext context)
        {
            if (oriInput == null) return null;
            if (oriInput.StartsWith("@sql:"))
            {
                string sql = oriInput.Substring(5);
                var dbSet = DBSuit.ReadOnlyConnection(context);
                using (DB db = new DB(dbSet.DataBaseType, dbSet.ConnString))
                {
                    return db.GetStringForceNoNull(sql);
                }
            }
            else return oriInput;
        }

        public static string ParaPattern(HttpContext context, string oriInput, int[] loopCount = null, Dictionary<string, string> CodeKVDic = null)
        {
            return ParaPattern(context, oriInput, null, loopCount, CodeKVDic);
        }
        public static string ParaPattern(HttpContext context, string oriInput, Dictionary<string, object> reqDic, int[] loopCount = null, Dictionary<string, string> CodeKVDic = null)
        {
            if (loopCount == null) loopCount = new int[] { 1 };
            if (loopCount[0] > 999)
            {
                FTFrame.Project.Core.Api.LogError("ParaPattern {maybe endless loop} " + oriInput, "");
                return oriInput;
            }
            //@para{abc} @para{abc,1|2}
            if (oriInput == null) return null;
            Regex r = new Regex(@"@para\{[^(\})]*\}");
            var mc = r.Matches(oriInput);
            foreach (string p in mc.Select(x => x.Value).Distinct())
            {
                //@para{abc,1|2}
                string pattern = p;

                string paraVal = "";
                if (CodeKVDic != null && CodeKVDic.ContainsKey(p))
                {
                    paraVal = CodeKVDic[p];
                }
                else
                {

                    pattern = pattern.Substring(pattern.IndexOf('{') + 1, pattern.LastIndexOf('}') - pattern.IndexOf('{') - 1);
                    //abc,1|2   abc,
                    if (pattern.IndexOf(',') < 0)
                    {
                        pattern += ",";
                    }
                    int index = pattern.IndexOf(',');
                    //abc
                    string left = pattern.Substring(0, index).Trim();
                    //1|2   
                    string right = pattern.Substring(index + 1).Trim();
                    string[] paras = new string[0];
                    if (right != "") paras = right.Split(new char[] { '|', ',' });
                    string val = "";
                    if (ParaDic().ContainsKey(left))
                    {
                        var paraSet = ParaDic()[left];
                        if (paraSet.Count == 1)
                        {
                            if (paraSet[0].Cdn == null)
                            {
                                val = paraSet[0].ParaValue;
                            }
                        }
                        else
                        {
                            Dictionary<string, string> pairValueDic = new Dictionary<string, string>();
                            foreach (var paraItem in paraSet)
                            {
                                if (paraItem.Cdn.Length == 3)
                                {
                                    string lval = paraItem.Cdn[0];
                                    string pair = paraItem.Cdn[1];
                                    string rval = paraItem.Cdn[2];
                                    for (int i = 0; i < paras.Length && i < 12; i++)
                                    {
                                        lval = lval.Replace("@para" + (i + 1) + "@", paras[i]);
                                        rval = rval.Replace("@para" + (i + 1) + "@", paras[i]);
                                    }
                                    if (pairValueDic.ContainsKey(lval)) lval = pairValueDic[lval];
                                    else
                                    {
                                        pairValueDic.Add(lval, SqlPattern(CodePattern(context, lval, reqDic), reqDic, context));
                                        lval = pairValueDic[lval];
                                    }
                                    if (pairValueDic.ContainsKey(rval)) rval = pairValueDic[rval];
                                    else
                                    {
                                        pairValueDic.Add(rval, SqlPattern(CodePattern(context, rval, reqDic), reqDic, context));
                                        rval = pairValueDic[rval];
                                    }
                                    bool cdnOK = false;
                                    switch (pair)
                                    {
                                        case "==":
                                            cdnOK = lval.Equals(rval);
                                            break;
                                        case ">":
                                            if (decimal.TryParse(lval, out decimal l01) && decimal.TryParse(rval, out decimal l02))
                                            {
                                                cdnOK = l01 > l02;
                                            }
                                            break;
                                        case "<":
                                            if (decimal.TryParse(lval, out decimal l11) && decimal.TryParse(rval, out decimal l12))
                                            {
                                                cdnOK = l11 < l12;
                                            }
                                            break;
                                        case ">=":
                                            if (decimal.TryParse(lval, out decimal l21) && decimal.TryParse(rval, out decimal l22))
                                            {
                                                cdnOK = l21 >= l22;
                                            }
                                            break;
                                        case "<=":
                                            if (decimal.TryParse(lval, out decimal l31) && decimal.TryParse(rval, out decimal l32))
                                            {
                                                cdnOK = l31 <= l32;
                                            }
                                            break;
                                        case "!=":
                                            cdnOK = !lval.Equals(rval);
                                            break;
                                        case "Start":
                                            cdnOK = lval.StartsWith(rval);
                                            break;
                                        case "End":
                                            cdnOK = lval.EndsWith(rval);
                                            break;
                                        case "Contain":
                                            cdnOK = lval.Contains(rval);
                                            break;
                                        case "!Start":
                                            cdnOK = !lval.StartsWith(rval);
                                            break;
                                        case "!End":
                                            cdnOK = !lval.EndsWith(rval);
                                            break;
                                        case "!Contain":
                                            cdnOK = !lval.Contains(rval);
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdnOK)
                                    {
                                        val = paraItem.ParaValue;
                                        break;
                                    }
                                }
                                else if (paraItem.Cdn.Length == 1)
                                {
                                    val = paraItem.ParaValue;
                                    break;
                                }
                            }
                            pairValueDic.Clear();
                            pairValueDic = null;
                        }
                    }
                    else val = "{no para key:" + left + "}";
                    for (int i = 0; i < paras.Length && i < 12; i++)
                    {
                        val = val.Replace("@para" + (i + 1) + "@", paras[i]);
                    }
                    //log.Debug(val, "1111");
                    val = CodePattern(context, val, reqDic);
                    loopCount[0]++;
                    //log.Debug(val, "2222");
                    val = ParaPattern(context, val, reqDic, loopCount);
                    //log.Debug(val, "3333");
                    val = SqlPattern(val, reqDic,context);
                    //log.Debug(val, "4444");
                    //log.Debug(oriInput, "aaaa");
                    //log.Debug(p, "bbbb");
                    paraVal = val;
                    if (CodeKVDic != null && !CodeKVDic.ContainsKey(p)) CodeKVDic.Add(p, paraVal);
                }
                oriInput = oriInput.Replace(p, paraVal);
                //log.Debug(oriInput, "cccc");
            }
            return oriInput;
        }
        public static string EnumPattern(string oriInput)
        {
            var item = oriInput.Split(new char[] { '(', '.', ')' });
            if (item.Length < 3) return oriInput;
            if (ParaDic().ContainsKey("Enum_" + item[1]))
            {
                var paraSet = ParaDic()["Enum_" + item[1]];
                foreach (var set in paraSet)
                {
                    if (set.Cdn[0] == item[2]) return set.Cdn[2];
                }
            }
            return oriInput;
        }

        public static Dictionary<string, string> CodeParaDic(DR dr,string CodeDefine)
        {
            var dic = new Dictionary<string, string>();
            Regex r = new Regex(@"\[[\w\.]*\]");
            MatchCollection mc = r.Matches(CodeDefine);
            foreach (Match m in mc)
            {
                if(!dic.ContainsKey(m.Value))
                {
                    dic.Add(m.Value, dr.CommonValue(m.Value.Replace("[", "").Replace("]", "")).val);
                }
            }
            return dic;
        }
        public static string ExecOperation(HttpContext context, DB db, ST st, string input, Dictionary<string, object> reqDic)
        {
            bool isDBNew = false;
            try
            {
                var items = input.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
                if(items.Length > 0)
                {
                    if(db == null)
                    {
                        db = new DB(DBConst.DataBaseType, DBConst.DBConnString);
                        isDBNew = true;
                    }
                }
                foreach (string item in items)
                {
                    var setVal = item.Trim();
                    if (setVal.StartsWith("@sql:"))
                    {
                        string sql = ParaPattern(context, CodePattern(context, setVal.Substring(5), reqDic), reqDic);
                        if (st != null) db.ExecSql(sql, st);
                        else db.ExecSql(sql);
                    }
                    else if (setVal.StartsWith("@code("))
                    {
                        string ret = Interface.Code.Get(setVal, reqDic, context);
                        if (ret != null) return ret;
                    }
                    else if (setVal.StartsWith("@para{"))
                    {
                        string val = ParaPattern(context, setVal, reqDic);
                        var ret = ExecOperation(context, db, st, val, reqDic);
                        if (ret != null) return ret;
                    }
                    else if (setVal.StartsWith("@api_"))
                    {
                        Api.DataOPExec(context, ParaPattern(context, CodePattern(context, setVal.Substring(5), reqDic), reqDic), new string[0]);
                    }
                    else
                    {
                        var errorStr = "Not Support In ExecOperation:" + item;
                        FTFrame.Project.Core.Api.LogError(errorStr, "");
                        return errorStr;
                    }
                }
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError(ex);
                return ex.Message;
            }
            finally
            {
                if (isDBNew) db.Close();
            }
            return null;
        }
        public static (DBClient.DataBase DataBaseType, string ConnString) CustomDBSet(string CustomConnection, string[] paras, Dictionary<string, object> reqDic, HttpContext Context,bool ForReadOnly = false)
        {
            if (string.IsNullOrWhiteSpace(CustomConnection)) return ForReadOnly ? DBSuit.ReadOnlyConnection(Context) : DBSuit.BaseConnection(Context);
            for (int k = 1; k < paras.Length; k++)
            {
                CustomConnection = CustomConnection.Replace("@p" + k + "@", paras[k]);
            }
            var items = CustomConnection.Split(new string[] { "##" }, StringSplitOptions.None);
            var dataBaseType = DBConst.DataBaseType;
            var connStr = items[0];
            if (items.Length > 1)
            {
                try
                {
                    dataBaseType = (DBClient.DataBase)Enum.Parse(typeof(DBClient.DataBase), items[1]);
                }
                catch (Exception ex)
                {
                    FTFrame.Project.Core.Api.LogError(ex.Message, "CustomDB Convert DataBaseType Error For " + items[1]);
                }
            }
            if (connStr.StartsWith("@code")) connStr = Interface.Code.Get(connStr, reqDic, Context);
            else if (connStr.StartsWith("@para")) connStr = adv.ParaPattern(Context,connStr);
            else if (connStr == "ftdp") connStr = SysConst.ConnectionStr_FTDP;
            return (dataBaseType, connStr);
        }
        public static string GetTableName(string s1, string s2, string s3)
        {
            if (s3.Length == 4)
            {
                return ("ft_" + s1 + "_" + s2 + "_" + s3).ToLower();
            }
            else
            {
                return ("ft_" + s3.Substring(s3.LastIndexOf('[') + 1, s3.LastIndexOf(']') - s3.LastIndexOf('[') - 1) + "_" + s2 + "_" + s3.Substring(0, 4)).ToLower();

            }
        }
        public static string GetTableName(string s1, string s2, string s3, string s4)
        {
            return (GetTableName(s1, s2, s3) + "_" + s4).ToLower();
        }
        public static ArrayList GetRoleResList(string sessionname, string siteid, string roledata)
        {
            DB db = new DB();
            db.Open();
            try
            {
                //string tablename = StringFunction.getTableName(siteid, "role", roledata);
                string tablenameuser = GetTableName(siteid, "role", roledata, "user");
                string tablenameres = GetTableName(siteid, "role", roledata, "res");
                ArrayList al = new ArrayList();
                object _sessionval = session.Get(sessionname);
                string sessionval = _sessionval == null ? "" : _sessionval.ToString();
                string sql = null;
                if (sessionval.StartsWith("[") && sessionval.EndsWith("]"))//[a][b]
                {
                    string subsql = str.D2DD(sessionval).Replace("][", "','").Replace("]", "'").Replace("[", "'");
                    sql = "select distinct(a.resid) from " + tablenameres + " a," + tablenameuser + " b where a.roleid=b.roleid and b.userid in (" + subsql + ")";
                }
                else//常规
                {
                    sql = "select distinct(a.resid) from " + tablenameres + " a," + tablenameuser + " b where a.roleid=b.roleid and b.userid='" + str.D2DD(sessionval) + "'";
                }
                DR rdr = db.OpenRecord(sql);
                while (rdr.Read())
                {
                    al.Add(rdr.GetString(0));
                }
                rdr.Close();
                return al;
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError(ex);
                return new ArrayList();
            }
            finally
            {
                db.Close();
            }
        }
        public static string HavePageRight(HttpContext context, bool ModOpen, bool ViewOpen, string DataStr, string ParaName, string MemBind, string EleCondition, string RoleBindData, string RoleBindSession, string AuthRule, string FlowStat, string SiteID, string AlertInfo)
        {
            if (DataStr == null || DataStr.Equals("")) return null;
            if (ParaName == null || ParaName.Equals("")) return null;
            if (context.Request.Query[ParaName].FirstOrDefault<string>() == null || context.Request.Query[ParaName].FirstOrDefault<string>().Trim().Equals("")) return null;
            if (!ModOpen && !ViewOpen) return null;
            DB db = new DB();
            db.Open();
            try
            {
                string MainTable = null;
                if (DataStr.StartsWith("@")) MainTable = DataStr.Substring(1);
                else
                    MainTable = "ft_" + SiteID + "_f_" + DataStr;
                string fid = context.Request.Query[ParaName].FirstOrDefault<string>().Trim();
                string sql = "select count(*) as ca from " + MainTable + " a where a.fid='" + str.D2DD(fid) + "'";
                string FlowStat_sql = "";
                if (FlowStat != null && !FlowStat.Trim().Equals(""))
                {
                    FlowStat = GetSpecialBase(context, FlowStat.Trim(), SiteID);
                    FlowStat_sql = " and a.flow in (" + str.D2DD(FlowStat) + ")";
                }
                string MemBind_sql = "1=1";
                string EleCondition_sql = "1=1";
                string RoleBindFid_sql = "1=1";
                string RoleBindStat_sql = "1=1";
                if (AuthRule.IndexOf("%m%") >= 0)
                {
                    if (MemBind != null && !MemBind.Trim().Equals(""))
                    {
                        MemBind = GetSpecialBase(context, MemBind.Trim(), SiteID);
                        MemBind_sql = "a.fmem='" + str.D2DD(MemBind) + "'";
                    }
                }
                if (AuthRule.IndexOf("%e%") >= 0)
                {
                    if (EleCondition != null && !EleCondition.Trim().Equals(""))
                    {
                        EleCondition = GetSpecialBase(context, EleCondition.Trim(), SiteID);
                        EleCondition_sql = EleCondition;
                    }
                }
                if (RoleBindData != null && !RoleBindData.Trim().Equals(""))
                {
                    if (AuthRule.IndexOf("%f%") >= 0)
                    {
                        if (RoleBindSession != null && !RoleBindSession.Trim().Equals(""))
                        {
                            object _sessionval = session.Get(RoleBindSession.Trim());
                            string sessionval = _sessionval == null ? "" : _sessionval.ToString();
                            if (ModOpen && !ViewOpen)
                                RoleBindFid_sql = "a.fid in (select distinct(ra.fid) from ds_" + SiteID + "_role_" + RoleBindData + "_fid ra,ds_" + SiteID + "_role_" + RoleBindData + "_user rb where ra.roleid=rb.roleid and rb.userid='" + str.D2DD(sessionval) + "' and ra.ismod=1)";
                            else if (!ModOpen && ViewOpen)
                                RoleBindFid_sql = "a.fid in (select distinct(ra.fid) from ds_" + SiteID + "_role_" + RoleBindData + "_fid ra,ds_" + SiteID + "_role_" + RoleBindData + "_user rb where ra.roleid=rb.roleid and rb.userid='" + str.D2DD(sessionval) + "' and ra.isview=1)";
                            else if (ModOpen && ViewOpen)
                                RoleBindFid_sql = "a.fid in (select distinct(ra.fid) from ds_" + SiteID + "_role_" + RoleBindData + "_fid ra,ds_" + SiteID + "_role_" + RoleBindData + "_user rb where ra.roleid=rb.roleid and rb.userid='" + str.D2DD(sessionval) + "' and ra.ismod=1 and ra.isview=1)";
                        }
                    }
                    if (AuthRule.IndexOf("%s%") >= 0)
                    {
                        if (RoleBindSession != null && !RoleBindSession.Trim().Equals(""))
                        {
                            object _sessionval = session.Get(RoleBindSession.Trim());
                            string sessionval = _sessionval == null ? "" : _sessionval.ToString();
                            if (ModOpen && !ViewOpen)
                                RoleBindStat_sql = "a.flow in (select distinct(ra.stat) from ds_" + SiteID + "_role_" + RoleBindData + "_stat ra,ds_" + SiteID + "_role_" + RoleBindData + "_user rb where ra.roleid=rb.roleid and rb.userid='" + str.D2DD(sessionval) + "' and ra.ismod=1)";
                            else if (!ModOpen && ViewOpen)
                                RoleBindStat_sql = "a.flow in (select distinct(ra.stat) from ds_" + SiteID + "_role_" + RoleBindData + "_stat ra,ds_" + SiteID + "_role_" + RoleBindData + "_user rb where ra.roleid=rb.roleid and rb.userid='" + str.D2DD(sessionval) + "' and ra.isview=1)";
                            else if (ModOpen && ViewOpen)
                                RoleBindStat_sql = "a.flow in (select distinct(ra.stat) from ds_" + SiteID + "_role_" + RoleBindData + "_stat ra,ds_" + SiteID + "_role_" + RoleBindData + "_user rb where ra.roleid=rb.roleid and rb.userid='" + str.D2DD(sessionval) + "' and ra.ismod=1 and ra.isview=1)";
                        }
                    }
                }
                string AuthRule_sql = "";
                if (!AuthRule.Trim().Equals(""))
                    AuthRule_sql = " and (" + AuthRule.Replace("%m%", "(" + MemBind_sql + ")").Replace("%f%", "(" + RoleBindFid_sql + ")").Replace("%s%", "(" + RoleBindStat_sql + ")").Replace("%e%", "(" + EleCondition_sql + ")") + ")";
                sql += FlowStat_sql + AuthRule_sql;
                if (db.GetInt(sql) > 0) return null;
                if (!AlertInfo.Trim().Equals("")) return AlertInfo;
                return "你没有查看或修改该页面的权限";
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError(ex);
                return ex.Message;
            }
            finally
            {
                db.Close();
            }
        }
        public static string PageRightFailedInfo(string AlertInfo, string GoURL)
        {
            string info = "";
            if (AlertInfo.StartsWith("@"))
                info += "<script language='javascript'>" + AlertInfo.Substring(1) + "</script>";
            else
                info += "<script language='javascript'>alert('" + AlertInfo + "');</script>";
            if (!GoURL.Trim().Equals(""))
            {
                info += "<script language='javascript'>location.href='" + GoURL.Trim() + "';</script>";
            }
            return info;
        }
        public static string HaveDeleteRight(HttpContext context, string DataStr, string fid, string MemBind, string EleCondition, string RoleBindData, string RoleBindSession, string AuthRule, string FlowStat, string SiteID, string IdCol = "fid")
        {
            if (DataStr == null || DataStr.Equals("")) return null;
            if (fid == null || fid.Equals("")) return null;
            DB db = new DB();
            db.Open();
            try
            {
                string MainTable = null;
                if (DataStr.StartsWith("@")) MainTable = DataStr.Substring(1);
                else
                    MainTable = "ft_" + SiteID + "_f_" + DataStr;
                string sql = "select count(*) as ca from " + MainTable + " a where a." + IdCol + "='" + str.D2DD(fid) + "'";
                string FlowStat_sql = "";
                if (FlowStat != null && !FlowStat.Trim().Equals(""))
                {
                    FlowStat = GetSpecialBase(context, FlowStat.Trim(), SiteID);
                    FlowStat_sql = " and a.flow in (" + str.D2DD(FlowStat) + ")";
                }
                string MemBind_sql = "1=1";
                string EleCondition_sql = "1=1";
                string RoleBindFid_sql = "1=1";
                string RoleBindStat_sql = "1=1";
                if (AuthRule.IndexOf("%m%") >= 0)
                {
                    if (MemBind != null && !MemBind.Trim().Equals(""))
                    {
                        MemBind = GetSpecialBase(context, MemBind.Trim(), SiteID);
                        MemBind_sql = "a.fmem='" + str.D2DD(MemBind) + "'";
                    }
                }
                if (AuthRule.IndexOf("%e%") >= 0)
                {
                    if (EleCondition != null && !EleCondition.Trim().Equals(""))
                    {
                        EleCondition = GetSpecialBase(context, EleCondition.Trim(), SiteID);
                        EleCondition_sql = EleCondition;
                    }
                }
                if (RoleBindData != null && !RoleBindData.Trim().Equals(""))
                {
                    if (AuthRule.IndexOf("%f%") >= 0)
                    {
                        if (RoleBindSession != null && !RoleBindSession.Trim().Equals(""))
                        {
                            object _sessionval = session.Get(RoleBindSession.Trim());
                            string sessionval = _sessionval == null ? "" : _sessionval.ToString();
                            RoleBindFid_sql = "a." + IdCol + " in (select distinct(ra.fid) from ds_" + SiteID + "_role_" + RoleBindData + "_fid ra,ds_" + SiteID + "_role_" + RoleBindData + "_user rb where ra.roleid=rb.roleid and rb.userid='" + str.D2DD(sessionval) + "' and ra.isdel=1)";
                        }
                    }
                    if (AuthRule.IndexOf("%s%") >= 0)
                    {
                        if (RoleBindSession != null && !RoleBindSession.Trim().Equals(""))
                        {
                            object _sessionval = session.Get(RoleBindSession.Trim());
                            string sessionval = _sessionval == null ? "" : _sessionval.ToString();
                            RoleBindStat_sql = "a.flow in (select distinct(ra.stat) from ds_" + SiteID + "_role_" + RoleBindData + "_stat ra,ds_" + SiteID + "_role_" + RoleBindData + "_user rb where ra.roleid=rb.roleid and rb.userid='" + str.D2DD(sessionval) + "' and ra.isdel=1)";
                        }
                    }
                }
                string AuthRule_sql = "";
                if (!AuthRule.Trim().Equals(""))
                    AuthRule_sql = " and (" + AuthRule.Replace("%m%", "(" + MemBind_sql + ")").Replace("%f%", "(" + RoleBindFid_sql + ")").Replace("%s%", "(" + RoleBindStat_sql + ")").Replace("%e%", "(" + EleCondition_sql + ")") + ")";
                sql += FlowStat_sql + AuthRule_sql;
                if (db.GetInt(sql) > 0) return null;
                return "你没有删除该项的权限";
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError(ex);
                return ex.Message;
            }
            finally
            {
                db.Close();
            }
        }
        public static string HaveCopyRight(HttpContext context, string DataStr, string fid, string MemBind, string EleCondition, string RoleBindData, string RoleBindSession, string AuthRule, string FlowStat, string SiteID)
        {
            if (DataStr == null || DataStr.Equals("")) return null;
            if (fid == null || fid.Equals("")) return null;
            DB db = new DB();
            db.Open();
            try
            {
                string MainTable = null;
                if (DataStr.StartsWith("@")) MainTable = DataStr.Substring(1);
                else
                    MainTable = "ft_" + SiteID + "_f_" + DataStr;
                string sql = "select count(*) as ca from " + MainTable + " a where a.fid='" + str.D2DD(fid) + "'";
                string FlowStat_sql = "";
                if (FlowStat != null && !FlowStat.Trim().Equals(""))
                {
                    FlowStat = GetSpecialBase(context, FlowStat.Trim(), SiteID);
                    FlowStat_sql = " and a.flow in (" + str.D2DD(FlowStat) + ")";
                }
                string MemBind_sql = "1=1";
                string EleCondition_sql = "1=1";
                string RoleBindFid_sql = "1=1";
                string RoleBindStat_sql = "1=1";
                if (AuthRule.IndexOf("%m%") >= 0)
                {
                    if (MemBind != null && !MemBind.Trim().Equals(""))
                    {
                        MemBind = GetSpecialBase(context, MemBind.Trim(), SiteID);
                        MemBind_sql = "a.fmem='" + str.D2DD(MemBind) + "'";
                    }
                }
                if (AuthRule.IndexOf("%e%") >= 0)
                {
                    if (EleCondition != null && !EleCondition.Trim().Equals(""))
                    {
                        EleCondition = GetSpecialBase(context, EleCondition.Trim(), SiteID);
                        EleCondition_sql = EleCondition;
                    }
                }
                if (RoleBindData != null && !RoleBindData.Trim().Equals(""))
                {
                    if (AuthRule.IndexOf("%f%") >= 0)
                    {
                        if (RoleBindSession != null && !RoleBindSession.Trim().Equals(""))
                        {
                            object _sessionval = session.Get(RoleBindSession.Trim());
                            string sessionval = _sessionval == null ? "" : _sessionval.ToString();
                            RoleBindFid_sql = "a.fid in (select distinct(ra.fid) from ds_" + SiteID + "_role_" + RoleBindData + "_fid ra,ds_" + SiteID + "_role_" + RoleBindData + "_user rb where ra.roleid=rb.roleid and rb.userid='" + str.D2DD(sessionval) + "' and (ra.isview=1 or ra.ismod=1))";
                        }
                    }
                    if (AuthRule.IndexOf("%s%") >= 0)
                    {
                        if (RoleBindSession != null && !RoleBindSession.Trim().Equals(""))
                        {
                            object _sessionval = session.Get(RoleBindSession.Trim());
                            string sessionval = _sessionval == null ? "" : _sessionval.ToString();
                            RoleBindStat_sql = "a.flow in (select distinct(ra.stat) from ds_" + SiteID + "_role_" + RoleBindData + "_stat ra,ds_" + SiteID + "_role_" + RoleBindData + "_user rb where ra.roleid=rb.roleid and rb.userid='" + str.D2DD(sessionval) + "' and (ra.isview=1 or ra.ismod=1))";
                        }
                    }
                }
                string AuthRule_sql = "";
                if (!AuthRule.Trim().Equals(""))
                    AuthRule_sql = " and (" + AuthRule.Replace("%m%", "(" + MemBind_sql + ")").Replace("%f%", "(" + RoleBindFid_sql + ")").Replace("%s%", "(" + RoleBindStat_sql + ")").Replace("%e%", "(" + EleCondition_sql + ")") + ")";
                sql += FlowStat_sql + AuthRule_sql;
                if (db.GetInt(sql) > 0) return null;
                return "你没有复制该项的权限";
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError(ex);
                return ex.Message;
            }
            finally
            {
                db.Close();
            }
        }
        public static string HaveFlowRight(HttpContext context, string DataStr, string fid, string MemBind, string EleCondition, string RoleBindData, string RoleBindSession, string AuthRule, string FlowStat, string SiteID)
        {
            if (DataStr == null || DataStr.Equals("")) return null;
            if (fid == null || fid.Equals("")) return null;
            DB db = new DB();
            db.Open();
            try
            {
                string MainTable = null;
                if (DataStr.StartsWith("@")) MainTable = DataStr.Substring(1);
                else
                    MainTable = "ft_" + SiteID + "_f_" + DataStr;
                string sql = "select count(*) as ca from " + MainTable + " a where a.fid='" + str.D2DD(fid) + "'";
                string FlowStat_sql = "";
                if (FlowStat != null && !FlowStat.Trim().Equals(""))
                {
                    FlowStat = GetSpecialBase(context, FlowStat.Trim(), SiteID);
                    FlowStat_sql = " and a.flow in (" + str.D2DD(FlowStat) + ")";
                }
                string MemBind_sql = "1=1";
                string EleCondition_sql = "1=1";
                string RoleBindFid_sql = "1=1";
                string RoleBindStat_sql = "1=1";
                if (AuthRule.IndexOf("%m%") >= 0)
                {
                    if (MemBind != null && !MemBind.Trim().Equals(""))
                    {
                        MemBind = GetSpecialBase(context, MemBind.Trim(), SiteID);
                        MemBind_sql = "a.fmem='" + str.D2DD(MemBind) + "'";
                    }
                }
                if (AuthRule.IndexOf("%e%") >= 0)
                {
                    if (EleCondition != null && !EleCondition.Trim().Equals(""))
                    {
                        EleCondition = GetSpecialBase(context, EleCondition.Trim(), SiteID);
                        EleCondition_sql = EleCondition;
                    }
                }
                if (RoleBindData != null && !RoleBindData.Trim().Equals(""))
                {
                    if (AuthRule.IndexOf("%f%") >= 0)
                    {
                        if (RoleBindSession != null && !RoleBindSession.Trim().Equals(""))
                        {
                            object _sessionval = session.Get(RoleBindSession.Trim());
                            string sessionval = _sessionval == null ? "" : _sessionval.ToString();
                            RoleBindFid_sql = "a.fid in (select distinct(ra.fid) from ds_" + SiteID + "_role_" + RoleBindData + "_fid ra,ds_" + SiteID + "_role_" + RoleBindData + "_user rb where ra.roleid=rb.roleid and rb.userid='" + str.D2DD(sessionval) + "' and (ra.isview=1 or ra.ismod=1))";
                        }
                    }
                    if (AuthRule.IndexOf("%s%") >= 0)
                    {
                        if (RoleBindSession != null && !RoleBindSession.Trim().Equals(""))
                        {
                            object _sessionval = session.Get(RoleBindSession.Trim());
                            string sessionval = _sessionval == null ? "" : _sessionval.ToString();
                            RoleBindStat_sql = "a.flow in (select distinct(ra.stat) from ds_" + SiteID + "_role_" + RoleBindData + "_stat ra,ds_" + SiteID + "_role_" + RoleBindData + "_user rb where ra.roleid=rb.roleid and rb.userid='" + str.D2DD(sessionval) + "' and (ra.isview=1 or ra.ismod=1))";
                        }
                    }
                }
                string AuthRule_sql = "";
                if (!AuthRule.Trim().Equals(""))
                    AuthRule_sql = " and (" + AuthRule.Replace("%m%", "(" + MemBind_sql + ")").Replace("%f%", "(" + RoleBindFid_sql + ")").Replace("%s%", "(" + RoleBindStat_sql + ")").Replace("%e%", "(" + EleCondition_sql + ")") + ")";
                sql += FlowStat_sql + AuthRule_sql;
                if (db.GetInt(sql) > 0) return null;
                return "你没有操作该项流程的权限";
            }
            catch (Exception ex)
            {
                FTFrame.Project.Core.Api.LogError(ex);
                return ex.Message;
            }
            finally
            {
                db.Close();
            }
        }
        public static string GetSpecialBase(HttpContext context, string instr, string siteid)
        {
            return GetSpecialBase(context, instr, siteid, false);
        }
        public static string GetSpecialBase(HttpContext context, string instr, string siteid, bool SaveMode)
        {
            instr = instr.Trim();
            if (siteid != null && !siteid.Equals("")) instr = instr.Replace("@siteid@", siteid);
            instr = instr.Replace("@curdate@", str.GetDateTime());
            return ReplaceSpecial(context, instr, SaveMode);
        }
        public static string ReplaceSpecial(HttpContext context, string SQLS, bool SaveMode)
        {
            Regex r = new Regex(@"@session\[\w*\]|@request\[\w*\]|@requestForm\[\w*\]");
            MatchCollection mc = r.Matches(SQLS);
            foreach (Match m in mc)
            {
                if (SaveMode)
                {
                    SQLS = SQLS.Replace(m.Value, str.D2DD(GetSpecialValue(context, m.Value)));
                }
                else
                {
                    SQLS = SQLS.Replace(m.Value, GetSpecialValue(context, m.Value));
                }
            }
            return SQLS;
            //string sqlv = SQLS;
            //int indexstart = 0;
            //int indexend = 0;
            //int loopcount = 0;
            //string parastr = "";
            //indexstart = sqlv.IndexOf("@session[");
            //loopcount = 0;
            //while (indexstart >= 0 && loopcount < 1000)
            //{
            //    indexend = sqlv.IndexOf("]", indexstart);
            //    parastr = sqlv.Substring(indexstart, indexend - indexstart + 1);
            //    sqlv = sqlv.Replace(parastr, GetSpecialValue(context, parastr).Replace("|", "[#]").Replace("&", "[$]"));

            //    indexstart = sqlv.IndexOf("@session[");
            //    loopcount++;
            //}
            //indexstart = sqlv.IndexOf("@request[");
            //loopcount = 0;
            //while (indexstart >= 0 && loopcount < 1000)
            //{
            //    indexend = sqlv.IndexOf("]", indexstart);
            //    parastr = sqlv.Substring(indexstart, indexend - indexstart + 1);
            //    sqlv = sqlv.Replace(parastr, GetSpecialValue(context, parastr).Replace("|", "[#]").Replace("&", "[$]"));

            //    indexstart = sqlv.IndexOf("@request[");
            //    loopcount++;
            //}
            //indexstart = sqlv.IndexOf("@requestForm[");
            //loopcount = 0;
            //while (indexstart >= 0 && loopcount < 1000)
            //{
            //    indexend = sqlv.IndexOf("]", indexstart);
            //    parastr = sqlv.Substring(indexstart, indexend - indexstart + 1);
            //    sqlv = sqlv.Replace(parastr, GetSpecialValue(context, parastr).Replace("|", "[#]").Replace("&", "[$]"));

            //    indexstart = sqlv.IndexOf("@requestForm[");
            //    loopcount++;
            //}
            //return sqlv;
        }
        private static string GetSpecialValue(HttpContext context, string parastr)
        {
            if (parastr.StartsWith("@session"))
            {
                string indexname = parastr.Substring(parastr.IndexOf('[') + 1, parastr.IndexOf(']') - parastr.IndexOf('[') - 1);
                object val = session.Get(indexname);
                return val == null ? "" : val.ToString();
            }
            else if (parastr.StartsWith("@request"))
            {
                string indexname = parastr.Substring(parastr.IndexOf('[') + 1, parastr.IndexOf(']') - parastr.IndexOf('[') - 1);
                object val = session.Get(indexname);
                return val == null ? "" : val.ToString();
            }
            else if (parastr.StartsWith("@requestForm"))
            {
                string indexname = parastr.Substring(parastr.IndexOf('[') + 1, parastr.IndexOf(']') - parastr.IndexOf('[') - 1);
                object val = context.Request.Form[indexname];
                return val == null ? "" : val.ToString();
            }
            return "";
        }
        public static string SpecialValue(HttpContext context, string inputStr, string SiteID)
        {
            inputStr = inputStr.Trim().Replace("@siteid@", SiteID);
            if (inputStr.StartsWith("@"))
            {
                inputStr = ReplaceSpecial(context, inputStr, false).Trim();
            }
            else
            {
                var dbSet = DBSuit.ReadOnlyConnection(context);
                DB db = new DB(dbSet.DataBaseType,dbSet.ConnString);
                db.Open();
                try
                {
                    inputStr = ReplaceSpecial(context, inputStr, false).Trim();
                    DR r = db.OpenRecord(inputStr);
                    if (r.Read()) inputStr = r.GetString(0);
                    else inputStr = "(null)";
                    r.Close();
                }

                catch (Exception ex)
                {
                    FTFrame.Project.Core.Api.LogError("{Message}:\r\n" + ex.Message + "\r\n{Source}:\r\n" + ex.Source + "\r\n{InnerException}:\r\n" + ex.InnerException + "\r\n{StackTrace}:\r\n" + ex.StackTrace + "\r\n{TargetSite}:\r\n" + ex.TargetSite, "{Exception}");

                    return "(error)";
                }
                finally
                {
                    db.Close();
                }
            }

            return inputStr;
        }
        public static string SQLSelectSafe(string sql)
        {
            string pat = "exec|delete|master|truncate|declare|create|update|insert|drop|alter";
            string[] pats = pat.Split('|');
            foreach (string s in pats)
            {
                if (sql.Trim().IndexOf(s) >= 0)
                {
                    FTFrame.Project.Core.Api.LogDebug(sql, "SQLSelectSafe Alert");
                    return null;
                }
            }
            return sql;
        }
        public static decimal CaculateMath(string s)
        {
            s = s.Replace("(", "").Replace(")", "");
            decimal result = 0;
            string lastop = "+";
            int pa = s.IndexOfAny(new char[] { '+', '-', '*', '/' });
            while (pa > 0)
            {
                string curs = s.Substring(0, pa);
                switch (lastop)
                {
                    case "+": result += decimal.Parse(curs); break;
                    case "-": result -= decimal.Parse(curs); break;
                    case "*": result *= decimal.Parse(curs); break;
                    case "/": result /= decimal.Parse(curs); break;
                }
                lastop = s.Substring(pa, 1);
                s = s.Substring(pa + 1);
                pa = s.IndexOfAny(new char[] { '+', '-', '*', '/' });
            }
            switch (lastop)
            {
                case "+": result += decimal.Parse(s); break;
                case "-": result -= decimal.Parse(s); break;
                case "*": result *= decimal.Parse(s); break;
                case "/": result /= decimal.Parse(s); break;
            }
            return result;
        }
    }
}
