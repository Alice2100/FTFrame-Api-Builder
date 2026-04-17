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
using System.Collections;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Linq;
using Microsoft.Data.Sqlite;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Math;
using System.Diagnostics;

namespace FTFrame.Server.Core
{
    public class PublishUtil
    {
        public static string JavaRootPath = ConfigHelper.GetConfigValue("Java:RootPath");
        public static string JavaJarName = ConfigHelper.GetConfigValue("Java:JarName");
        public static string JavaPort = ConfigHelper.GetConfigValue("Java:Port");
        public static string JAVA_HOME = ConfigHelper.GetConfigValue("Java:JAVA_HOME");
        public static string MAVEN_HOME = ConfigHelper.GetConfigValue("Java:MAVEN_HOME");
        public static string JavaBaseUrl = ConfigHelper.GetConfigValue("Java:BaseUrl");
        public static void SingleFileSiteBakSave(string caption, DateTime pubDateNow, string pageid, string directPath, string filename)
        {
            try
            {
                caption = caption.Replace("/", "_").Replace("\\", "_").Replace(" ", "");
                caption = caption.Replace("&", "").Replace("|", "").Replace("?", "").Replace("*", "").Replace(">", "").Replace("<", "");
                StringBuilder rBuilder = new StringBuilder(caption);
                foreach (char rInvalidChar in Path.GetInvalidFileNameChars()) rBuilder.Replace(rInvalidChar.ToString(), string.Empty);
                caption = rBuilder.ToString();
                //int index = caption.IndexOf('_');
                //if(index>0)
                //{
                //    string uCaption = str.ToUnicodeString(caption.Substring(0, index)).Replace("\\", "");
                //    caption = uCaption + caption.Substring(index);
                //}
                string deploypath = System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + @"_ftpub";
                if (File.Exists(deploypath + Path.DirectorySeparatorChar + @"site.fthidden"))
                {
                    //File.Move(deploypath + @"\site.fthidden", deploypath + @"\site_" + caption + ".fthidden");
                    string newsitefilename = str.GetDateTime(pubDateNow);
                    newsitefilename = newsitefilename.Replace(" ", "").Replace(":", "").Replace("-", "");
                    if (pageid != null)
                    {
                        Directory.CreateDirectory(SysConst.SiteBak + Path.DirectorySeparatorChar + pageid);
                        string filepath = SysConst.SiteBak + Path.DirectorySeparatorChar + pageid + Path.DirectorySeparatorChar + newsitefilename + "_" + caption + ".site";
                        File.Move(deploypath + Path.DirectorySeparatorChar + @"site.fthidden", filepath);
                        //if (File.Exists(filepath)) new FileInfo(filepath).LastWriteTime = pubDateNow;
                        string dirPath = SysConst.RootPath + "/Pages/" + directPath;
                        dirPath = dirPath.Replace("/", Path.DirectorySeparatorChar.ToString());
                        var pubPageFileName = (dirPath + Path.DirectorySeparatorChar + filename).Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString()) + ".cshtml";
                        if (File.Exists(pubPageFileName))
                        {
                            string dirPathBak = SysConst.SiteBak + Path.DirectorySeparatorChar + pageid + Path.DirectorySeparatorChar + newsitefilename + "_" + caption + Path.DirectorySeparatorChar + directPath;
                            dirPathBak = dirPathBak.Replace("/", Path.DirectorySeparatorChar.ToString());
                            Directory.CreateDirectory(dirPathBak.Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString()));
                            var pubPageFileNameBak = (dirPathBak + Path.DirectorySeparatorChar + filename).Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString()) + ".cshtml";
                            File.Copy(pubPageFileName, pubPageFileNameBak);
                        }
                        string rootPath = JavaRootPath;
                        if (!string.IsNullOrWhiteSpace(rootPath))
                        {
                            rootPath = Path.GetFullPath(rootPath);
                            string pubPath = rootPath + "/project/src/main/java";
                            var pathArrays = directPath.Split(new string[] { "/", "\\" }, StringSplitOptions.RemoveEmptyEntries);
                            pubPath += Path.DirectorySeparatorChar + "com" + Path.DirectorySeparatorChar + "ftframe" + Path.DirectorySeparatorChar + "api";
                            foreach (var path in pathArrays)
                            {
                                pubPath += Path.DirectorySeparatorChar + path.Replace("-", "_").Replace(" ", "");
                            }
                            pubPath = pubPath.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString());
                            var pubFileName = pubPath + Path.DirectorySeparatorChar + filename.Replace("-", "_").Replace(" ", "") + ".java";
                            if (File.Exists(pubFileName))
                            {
                                string dirPathBak = SysConst.SiteBak + Path.DirectorySeparatorChar + pageid + Path.DirectorySeparatorChar + newsitefilename + "_" + caption;
                                dirPathBak += Path.DirectorySeparatorChar + "_java" + Path.DirectorySeparatorChar + "com" + Path.DirectorySeparatorChar + "ftframe" + Path.DirectorySeparatorChar + "api";
                                foreach (var path in pathArrays)
                                {
                                    dirPathBak += Path.DirectorySeparatorChar + path.Replace("-", "_").Replace(" ", "");
                                }
                                dirPathBak = dirPathBak.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString());
                                Directory.CreateDirectory(dirPathBak);
                                var pubPageFileNameBak = (dirPathBak + Path.DirectorySeparatorChar + filename.Replace("-", "_").Replace(" ", "")).Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString()) + ".java";
                                File.Copy(pubPageFileName, pubPageFileNameBak);
                            }
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(SysConst.SiteBak);
                        string filepath = SysConst.SiteBak + Path.DirectorySeparatorChar + newsitefilename + "_" + caption + ".site";
                        File.Move(deploypath + Path.DirectorySeparatorChar + @"site.fthidden", filepath);
                        //if (File.Exists(filepath)) new FileInfo(filepath).LastWriteTime = pubDateNow;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        public static void CodeSetUpdate(SqliteConnection conn, string QianMing)
        {
            string sql;
            using (DB db = new DB(SysConst.ConnectionStr_FTDP))
            {
                db.Open();
                sql = "delete from ft_ftdp_codeset where devuser='" + str.D2DD(QianMing) + "'";
                db.ExecSql(sql);
                sql = "select * from codeset where devuser='" + str.D2DD(QianMing) + "'";
                SqliteCommand command = new SqliteCommand(sql, conn);
                using (SqliteDataReader rdrSrc = command.ExecuteReader())
                {
                    while (rdrSrc.Read())
                    {
                        int id = rdrSrc.GetInt32(rdrSrc.GetOrdinal("id"));
                        string devuser = (rdrSrc.GetValue(rdrSrc.GetOrdinal("devuser")) ?? "").ToString();
                        string dllname = rdrSrc.GetValue(rdrSrc.GetOrdinal("dllname")).ToString();
                        string codekey = rdrSrc.GetValue(rdrSrc.GetOrdinal("codekey")).ToString();
                        string codeval = rdrSrc.GetValue(rdrSrc.GetOrdinal("codeval")).ToString();
                        string returntype = rdrSrc.GetValue(rdrSrc.GetOrdinal("returntype")).ToString();
                        string mimo = (rdrSrc.GetValue(rdrSrc.GetOrdinal("mimo")) ?? "").ToString();
                        DateTime modtime = rdrSrc.GetDateTime(rdrSrc.GetOrdinal("modtime"));
                        sql = "insert into ft_ftdp_codeset(fid,devuser,dllname,codekey,codeval,returntype,modtime,mimo)values('" + str.GetCombID() + "','" + str.D2DD(devuser) + "','" + str.D2DD(dllname) + "','" + str.D2DD(codekey) + "','" + str.D2DD(codeval) + "','" + str.D2DD(returntype) + "','" + modtime.ToString("yyyy-MM-dd HH:mm:ss") + "','" + str.D2DD(mimo) + "')";
                        db.ExecSql(sql);
                    }
                }
                command.Dispose();
            }
        }
        public static string CodeCompile(SqliteConnection conn, string QianMing)
        {
            List<string> DllList = new List<string>();
            Dictionary<string, string[]> SetList = new Dictionary<string, string[]>();
            string sql;
            using (DB db = new DB(SysConst.ConnectionStr_FTDP))
            {
                db.Open();
                sql = "delete from ft_ftdp_codeset where devuser='" + str.D2DD(QianMing) + "'";
                db.ExecSql(sql);
                sql = "select * from codeset where devuser='" + str.D2DD(QianMing) + "'";
                SqliteCommand command = new SqliteCommand(sql, conn);
                using (SqliteDataReader rdrSrc = command.ExecuteReader())
                {
                    while (rdrSrc.Read())
                    {
                        int id = rdrSrc.GetInt32(rdrSrc.GetOrdinal("id"));
                        string devuser = (rdrSrc.GetValue(rdrSrc.GetOrdinal("devuser")) ?? "").ToString();
                        string dllname = rdrSrc.GetValue(rdrSrc.GetOrdinal("dllname")).ToString();
                        string codekey = rdrSrc.GetValue(rdrSrc.GetOrdinal("codekey")).ToString();
                        string codeval = rdrSrc.GetValue(rdrSrc.GetOrdinal("codeval")).ToString();
                        string returntype = rdrSrc.GetValue(rdrSrc.GetOrdinal("returntype")).ToString();
                        string mimo = (rdrSrc.GetValue(rdrSrc.GetOrdinal("mimo")) ?? "").ToString();
                        DateTime modtime = rdrSrc.GetDateTime(rdrSrc.GetOrdinal("modtime"));
                        sql = "insert into ft_ftdp_codeset(fid,devuser,dllname,codekey,codeval,returntype,modtime,mimo)values('" + str.GetCombID() + "','" + str.D2DD(devuser) + "','" + str.D2DD(dllname) + "','" + str.D2DD(codekey) + "','" + str.D2DD(codeval) + "','" + str.D2DD(returntype) + "','" + modtime.ToString("yyyy-MM-dd HH:mm:ss") + "','" + str.D2DD(mimo) + "')";
                        db.ExecSql(sql);
                    }
                }
                command.Dispose();
                sql = "select dllname,codekey,codeval,returntype from ft_ftdp_codeset";
                using (DR dr = db.OpenRecord(sql))
                {
                    while (dr.Read())
                    {
                        string dllname = dr.GetString(0).Trim();//FTFrame.Base
                        if (string.IsNullOrWhiteSpace(dllname)) continue;
                        string codekey = dr.GetString(1).Trim();//FT.Com.WorkFlow.Busi.AfterAdd
                        string codeval = dr.GetString(2).Trim();//FT.Com.WorkFlow.Busi.AfterAdd(String,HttpContext)    String,Int16,Int32,Int64,Decimal,Single,Double,Boolean,HttpContext
                        string returntype = dr.GetString(3).Trim();//String  Void  List`1  Dictrinary`2
                        if (!DllList.Contains(dllname)) DllList.Add(dllname);
                        if (!SetList.ContainsKey(codekey))
                        {
                            SetList.Add(codekey, new string[] { codeval, returntype });
                        }
                    }
                }
            }
            var assemblyName = "FTFrame.Dynamic.Core.dll";
            List<MetadataReference> Assemblys = new List<MetadataReference>();
            Assemblys.Add(MetadataReference.CreateFromFile(SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "FTFrame.Core.dll"));
            Assemblys.Add(MetadataReference.CreateFromFile(SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "Pages" + Path.DirectorySeparatorChar + "_ft" + Path.DirectorySeparatorChar + "_compile" + Path.DirectorySeparatorChar + "System.Runtime.dll"));
            Assemblys.Add(MetadataReference.CreateFromFile(SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "Pages" + Path.DirectorySeparatorChar + "_ft" + Path.DirectorySeparatorChar + "_compile" + Path.DirectorySeparatorChar + "System.Collections.dll"));
            //Assemblys.Add(MetadataReference.CreateFromFile(typeof(Object).Assembly.Location));
            //Assemblys.Add(MetadataReference.CreateFromFile(typeof(String).Assembly.Location));
            //Assemblys.Add(MetadataReference.CreateFromFile(typeof(HttpContext).Assembly.Location));
            Assemblys.Add(MetadataReference.CreateFromFile(SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "Pages" + Path.DirectorySeparatorChar + "_ft" + Path.DirectorySeparatorChar + "_compile" + Path.DirectorySeparatorChar + "Microsoft.AspNetCore.Http.Abstractions.dll"));
            Assemblys.Add(MetadataReference.CreateFromFile(SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "Pages" + Path.DirectorySeparatorChar + "_ft" + Path.DirectorySeparatorChar + "_compile" + Path.DirectorySeparatorChar + "System.Web.dll"));
            //Assemblys.Add(MetadataReference.CreateFromFile(Assembly.Load("System").Location));
            //Assemblys.Add(MetadataReference.CreateFromFile(Assembly.Load("System.Web").Location));
            //Assemblys.Add(MetadataReference.CreateFromFile(Assembly.Load("System.Collections").Location));
            //Assemblys.Add(MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location)); 
            foreach (string dllname in DllList)
            {
                Assemblys.Add(MetadataReference.CreateFromFile(SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "" + dllname + ".dll"));
            }
            string codeText = CodeCompileGenerateCode(SetList);
            var syntaxTree = CSharpSyntaxTree.ParseText(codeText);
            var compilation = CSharpCompilation.Create(assemblyName, new[] { syntaxTree },
                   options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
               .AddReferences(Assemblys);
            // 编译到文件中。
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
            {

                timer.Enabled = false;
                if (!string.IsNullOrEmpty(SysConst.DllCompilePath) && !Directory.Exists(SysConst.DllCompilePath))
                {
                    Directory.CreateDirectory(SysConst.DllCompilePath);
                }
                using (var ms = new FileStream((string.IsNullOrEmpty(SysConst.DllCompilePath) ? (SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "Pages" + Path.DirectorySeparatorChar + "_ft" + Path.DirectorySeparatorChar + "_compile") : SysConst.DllCompilePath) + @"" + Path.DirectorySeparatorChar + "" + "FTFrame.Dynamic.Core.dll", FileMode.Create))
                {
                    var result = compilation.Emit(ms);
                    if (result.Success)
                    {
                    }
                    else
                    {
                        foreach (var v in result.Diagnostics)
                        {
                            log.Error(v.ToString());
                        }
                        //return "编译错误，详见日志";
                    }
                }
                //if (System.IO.File.Exists(SysConst.RootPath + "\\FTFrame.Dynamic.Core.dll"))
                //{
                //    using (var stream = System.IO.File.OpenRead(SysConst.RootPath + "\\FTFrame.Dynamic.Core.dll"))
                //    {
                //        System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(stream);
                //    }
                //}
                timer.Dispose();
            };
            timer.Interval = 1000;
            timer.Enabled = true;

            Assemblys.Clear();
            using (FileStream fs = new FileStream(SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "Pages" + Path.DirectorySeparatorChar + "_ft" + Path.DirectorySeparatorChar + "_compile" + Path.DirectorySeparatorChar + "FTFrame.Dynamic.Core.cs", FileMode.Create))
            {
                byte[] myByte = Encoding.UTF8.GetBytes(codeText);
                fs.Write(myByte, 0, myByte.Length);
                fs.Flush();
            }
            return null;
        }


        public static string CodeCompile_Old(SqliteConnection conn, string QianMing)
        {
            List<string> DllList = new List<string>();
            Dictionary<string, string[]> SetList = new Dictionary<string, string[]>();
            string sql;
            using (DB db = new DB(SysConst.ConnectionStr_FTDP))
            {
                db.Open();
                sql = "delete from ft_ftdp_codeset where devuser='" + str.D2DD(QianMing) + "'";
                db.ExecSql(sql);
                sql = "select * from codeset where devuser='" + str.D2DD(QianMing) + "'";
                SqliteCommand command = new SqliteCommand(sql, conn);
                using (SqliteDataReader rdrSrc = command.ExecuteReader())
                {
                    while (rdrSrc.Read())
                    {
                        int id = rdrSrc.GetInt32(rdrSrc.GetOrdinal("id"));
                        string devuser = (rdrSrc.GetValue(rdrSrc.GetOrdinal("devuser")) ?? "").ToString();
                        string dllname = rdrSrc.GetValue(rdrSrc.GetOrdinal("dllname")).ToString();
                        string codekey = rdrSrc.GetValue(rdrSrc.GetOrdinal("codekey")).ToString();
                        string codeval = rdrSrc.GetValue(rdrSrc.GetOrdinal("codeval")).ToString();
                        string returntype = rdrSrc.GetValue(rdrSrc.GetOrdinal("returntype")).ToString();
                        string mimo = (rdrSrc.GetValue(rdrSrc.GetOrdinal("mimo")) ?? "").ToString();
                        DateTime modtime = rdrSrc.GetDateTime(rdrSrc.GetOrdinal("modtime"));
                        sql = "insert into ft_ftdp_codeset(fid,devuser,dllname,codekey,codeval,returntype,modtime,mimo)values('" + str.GetCombID() + "','" + str.D2DD(devuser) + "','" + str.D2DD(dllname) + "','" + str.D2DD(codekey) + "','" + str.D2DD(codeval) + "','" + str.D2DD(returntype) + "','" + modtime.ToString("yyyy-MM-dd HH:mm:ss") + "','" + str.D2DD(mimo) + "')";
                        db.ExecSql(sql);
                    }
                }
                command.Dispose();
                sql = "select dllname,codekey,codeval,returntype from ft_ftdp_codeset";
                using (DR dr = db.OpenRecord(sql))
                {
                    while (dr.Read())
                    {
                        string dllname = dr.GetString(0).Trim();//FTFrame.Base
                        string codekey = dr.GetString(1).Trim();//FT.Com.WorkFlow.Busi.AfterAdd
                        string codeval = dr.GetString(2).Trim();//FT.Com.WorkFlow.Busi.AfterAdd(String,HttpContext)    String,Int16,Int32,Int64,Decimal,Single,Double,Boolean,HttpContext
                        string returntype = dr.GetString(3).Trim();//String  Void
                        if (!DllList.Contains(dllname)) DllList.Add(dllname);
                        if (!SetList.ContainsKey(codekey))
                        {
                            SetList.Add(codekey, new string[] { codeval, returntype });
                        }
                    }
                }
            }
            CSharpCodeProvider complier = new CSharpCodeProvider();
            //ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();
            CompilerParameters options = new CompilerParameters();
            options.ReferencedAssemblies.Add("System.dll");
            options.ReferencedAssemblies.Add("System.Web.dll");
            options.ReferencedAssemblies.Add("System.Net.Http.dll");
            options.ReferencedAssemblies.Add("System.Data.dll");
            options.ReferencedAssemblies.Add(SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "FTFrame.Core.dll");
            options.GenerateExecutable = false;
            options.GenerateInMemory = false;
            options.OutputAssembly = SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "FTFrame.Project.Dynamic.dll";
            options.IncludeDebugInformation = false;
            foreach (string dllname in DllList)
            {
                options.ReferencedAssemblies.Add(SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "" + dllname + ".dll");
            }
            CompilerResults cr = complier.CompileAssemblyFromSource(options, CodeCompileGenerateCode(SetList));
            if (cr.Errors.HasErrors)
            {
                foreach (CompilerError err in cr.Errors)
                {
                    log.Error(err.ErrorText);
                }
                return ("动态编译错误，详见日志");
            }
            return null;
        }
        private static string CodeCompileGenerateCode(Dictionary<string, string[]> SetList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;");
            sb.Append("using System.Collections.Generic;");
            sb.Append("using System.Text;");
            sb.Append("using System.Threading.Tasks;");
            sb.Append("using System.Web;");
            sb.Append("using Microsoft.AspNetCore.Http;");
            sb.Append("namespace FTFrame.Dynamic.Core");
            sb.Append("{");
            sb.Append(" public class Code  ");
            sb.Append(" {  ");
            sb.Append(" public static object Get(string code, string[] para, HttpContext Context)  ");
            sb.Append(" {  ");
            sb.Append(Environment.NewLine);
            sb.Append("object _dynamicObjectValue=\"(nocode)\";");
            sb.Append(Environment.NewLine);
            sb.Append("switch(code)");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            foreach (string key in SetList.Keys)
            {
                sb.Append("case \"" + key + "\":");
                //sb.Append("if(string.Compare(code,\"" + key + "\", StringComparison.Ordinal)==0){\r\n");
                int lastLeft = SetList[key][0].LastIndexOf('(');
                string item0 = SetList[key][0].Substring(0, lastLeft);
                string item1 = SetList[key][0].Substring(lastLeft + 1);
                //string[] item = SetList[key][0].Split('(');
                string[] item2 = item1.Replace(")", "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                string funcName = item0 + "(";
                for (int i = 0; i < item2.Length; i++)
                {//String,Int16,Int32,Int64,Decimal,Single,Double,Boolean,HttpContext
                    if (i > 0) funcName += ",";
                    switch (item2[i])
                    {
                        case "String":
                            funcName += "para[" + i + "]"; break;
                        case "Int16":
                            funcName += "int.Parse(para[" + i + "])"; break;
                        case "Int32":
                            funcName += "int.Parse(para[" + i + "])"; break;
                        case "Int64":
                            funcName += "long.Parse(para[" + i + "])"; break;
                        case "Decimal":
                            funcName += "decimal.Parse(para[" + i + "])"; break;
                        case "Single":
                            funcName += "float.Parse(para[" + i + "])"; break;
                        case "Double":
                            funcName += "double.Parse(para[" + i + "])"; break;
                        case "Boolean":
                            funcName += "bool.Parse(para[" + i + "])"; break;
                        case "HttpContext":
                            funcName += "Context"; break;
                    }
                }
                funcName += ")";
                if (SetList[key][1] == "Void")
                {

                    sb.Append(funcName + "; ");
                    sb.Append("_dynamicObjectValue = null;break;");
                }
                else//else if (SetList[key][1] == "String")
                {

                    sb.Append("_dynamicObjectValue = " + funcName + ";break;");
                }
                sb.Append(Environment.NewLine);
                //sb.Append("}");
            }
            sb.Append("}");
            sb.Append(Environment.NewLine);
            sb.Append("  return _dynamicObjectValue; ");
            sb.Append("   ");
            sb.Append("   ");
            sb.Append("  } ");
            sb.Append("  } ");
            sb.Append("}");

            string code = sb.ToString();
            return code;
        }
        private static string CodeCompileApi_Old(string DllName, StringBuilder sb)
        {
            CSharpCodeProvider complier = new CSharpCodeProvider();
            //ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();
            CompilerParameters options = new CompilerParameters();
            options.ReferencedAssemblies.Add("System.dll");
            options.ReferencedAssemblies.Add("System.Web.dll");
            options.ReferencedAssemblies.Add("System.Net.Http.dll");
            options.ReferencedAssemblies.Add("System.Data.dll");
            options.ReferencedAssemblies.Add(SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "FTFrame.Project.Core.dll");
            options.ReferencedAssemblies.Add(SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "FTFrame.Server.Core.dll");
            options.GenerateExecutable = false;
            options.GenerateInMemory = false;
            options.OutputAssembly = SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "" + DllName;
            options.IncludeDebugInformation = false;
            CompilerResults cr = complier.CompileAssemblyFromSource(options, sb.ToString());
            if (cr.Errors.HasErrors)
            {
                foreach (CompilerError err in cr.Errors)
                {
                    log.Error(err.ErrorText);
                }
                return ("Api动态编译错误，详见日志");
            }
            return null;
        }
        private static string CodeCompileApi(string DllName, StringBuilder sb)
        {
            //var syntaxTree = CSharpSyntaxTree.ParseText(sb.ToString());
            //var compilation = CSharpCompilation.Create(DllName, new[] { syntaxTree },
            //        options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            //var result = compilation.Emit(ms);
            // 指定编译选项。
            //foreach(var a in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    log.Error(a.FullName,a.GetName().Name);
            //}
            //log.Error(Assembly.Load("System").Location);
            //log.Error(Assembly.Load("System.Web").Location);
            var assemblyName = DllName;
            var syntaxTree = CSharpSyntaxTree.ParseText(sb.ToString());
            var compilation = CSharpCompilation.Create(assemblyName, new[] { syntaxTree },
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                //    .AddReferences(AppDomain.CurrentDomain.GetAssemblies().Select(x => MetadataReference.CreateFromFile(x.Location)));
                .AddReferences(
                    MetadataReference.CreateFromFile(SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "FTFrame.Project.Core.dll"),
                    MetadataReference.CreateFromFile(SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "FTFrame.Server.Core.dll"),
                    MetadataReference.CreateFromFile(typeof(Object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(String).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(HttpContext).Assembly.Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System").Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Web").Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Collections").Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location)

                //MetadataReference.CreateFromFile("System.dll"),
                //MetadataReference.CreateFromFile("System.Web.dll"),
                //MetadataReference.CreateFromFile("System.Net.Http.dll")
                // 这算是偷懒了吗？我把 .NET Core 运行时用到的那些引用都加入到引用了。
                // 加入引用是必要的，不然连 object 类型都是没有的，肯定编译不通过。
                //AppDomain.CurrentDomain.GetAssemblies().Select(x => MetadataReference.CreateFromFile(x.Location))
                );

            // 编译到文件中。
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
            {

                timer.Enabled = false;
                using (var ms = new FileStream(SysConst.RootPath + @"" + Path.DirectorySeparatorChar + "" + DllName, FileMode.Create))
                {
                    var result = compilation.Emit(ms);

                    if (result.Success)
                    {
                        //return null;
                    }
                    else
                    {
                        foreach (var v in result.Diagnostics)
                        {
                            log.Error(v.ToString());
                        }
                        //return "Error";
                    }
                }
                timer.Dispose();
            };
            timer.Interval = 1000;
            timer.Enabled = true;

            return null;
        }
        public static void ApiPagePublish(SqliteConnection conn, SqliteConnection conn2, string pageid, string pagecaption, string directPath, string filename, string QianMing)
        {
            if (SysConst.IsApiCompile) ApiPagePublish_Compile(conn, conn2, pageid, pagecaption, directPath, filename, QianMing);
            else ApiPagePublish_NoCompile(conn, conn2, pageid, pagecaption, directPath, filename, QianMing);
            ApiPagePublish_Java(conn, conn2, pageid, pagecaption, directPath, filename, QianMing);
        }
        private static void DelApiPageOther(string subPath)
        {
            string PtApiPath = (subPath + "?");
            PtApiPath = Api.ApiPathReplace(PtApiPath);
            using (DB db = new DB(SysConst.ConnectionStr_FTDP))
            {
                db.Open();
                string sql = "delete from ft_ftdp_apidoc where ApiPath like '" + PtApiPath + "%'";
                db.ExecSql(sql);
                sql = "delete from ft_ftdp_apiset where ApiPath like '" + PtApiPath + "%'";
                db.ExecSql(sql);
                if (PtApiPath.StartsWith("/")) PtApiPath = PtApiPath.Substring(1);
                PtApiPath = PtApiPath.Replace("?", "");
                sql = "delete from ft_ftdp_route where route ='" + PtApiPath + "'";
                db.ExecSql(sql);
            }
        }
        private static void ApiPagePublish_Compile(SqliteConnection conn, SqliteConnection conn2, string pageid, string pagecaption, string directPath, string filename, string QianMing)
        {
            string dirPath = SysConst.RootPath + "/Pages/" + directPath;
            dirPath = dirPath.Replace("/", Path.DirectorySeparatorChar.ToString());
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            string subPath = (directPath + filename).Replace(Path.DirectorySeparatorChar.ToString(), "/");
            DelApiPageOther(subPath);
            string nameSpace = "FTFrame.Api." + subPath.Replace("/", "_").Replace(" ", "").Replace(".", "");
            string sql = "select a.id,a.partxml,b.name from parts a,controls b,part_in_page c where a.id=c.partid and c.pageid='" + str.D2DD(pageid) + "' and a.controlid=b.id and ";
            sql += "((b.name='list' and a.name='List') or (b.name='dyvalue' and a.name='Interface') or (b.name='dataop' and a.name='Interface'))";
            string AspxCode = "@page" + Environment.NewLine + "@{Layout = null;}" + Environment.NewLine + "@{" + nameSpace + ".Interface.Do(HttpContext); }";
            StringBuilder sb = new StringBuilder();
            #region 生成c#代码
            sb.Append("using System;");
            sb.Append("using System.Collections.Generic;");
            sb.Append("using System.Text;");
            sb.Append("using System.Threading.Tasks;");
            sb.Append("using System.Web;");
            sb.Append("using Microsoft.AspNetCore.Http;");
            sb.Append("namespace " + nameSpace);
            sb.Append("{");
            sb.Append(" public class Interface  ");
            sb.Append(" {  ");
            sb.Append(" public static void Do(HttpContext Context)  ");
            sb.Append(" {  ");
            //sb.Append(" if (!FTFrame.Project.Core.ProjectFilter.Page(Context)) return;  ");
            sb.Append("Context.Response.ContentType = \"application/Json;charset=utf-8\";");
            sb.Append("string[] paras=FTFrame.Project.Core.Api.Paras(Context);");
            sb.Append("if(paras==null)");
            sb.Append("{");
            sb.Append("Context.Response.WriteAsync(\"{\\\"code\\\":203,\\\"" + Project.Core.Api.MessageStr + "\\\":\\\"no path\\\"}\");");
            sb.Append("return;");
            sb.Append("}");
            sb.Append("string json=\"\";");
            sb.Append("string key=paras[0];");
            sb.Append("if(0>1){}");
            #endregion
            using (DB db = new DB(SysConst.ConnectionStr_FTDP))
            {
                db.Open();
                SqliteCommand command = new SqliteCommand(sql, conn);
                using (SqliteDataReader rdrSrc = command.ExecuteReader())
                {
                    while (rdrSrc.Read())
                    {
                        string partid = rdrSrc.GetString(rdrSrc.GetOrdinal("id"));
                        string partxml = rdrSrc.GetString(rdrSrc.GetOrdinal("partxml"));
                        string controlname = rdrSrc.GetString(rdrSrc.GetOrdinal("name"));
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(partxml);
                        if (controlname == "list")
                        {
                            string ApiSet = getPartParamValue(doc, "APISet");
                            string OrderBy = getPartParamValue(doc, "OrderBy");
                            string CusSQL = Sql.GetSqlForRemoveSameCols(getPartParamValue(doc, "CusSQL"));
                            string CusSQLCount = Sql.GetSqlForRemoveSameCols(getPartParamValue(doc, "CusSQLHalf"));
                            string RowAll = getPartParamValue(doc, "RowAll");
                            string SearchDefine = getPartParamValue(doc, "SearchDefine");
                            string BlockDataDefine = getPartParamValue(doc, "BlockDataDefine");
                            string CusCondition = getPartParamValue(doc, "CusCondition");
                            string CacuRowData = getPartParamValue(doc, "CacuRowData");
                            string CustomConnection = getPartParamValue(doc, "CustomConnection");
                            string MainTable = getPartParamValue(doc, "MainTable");
                            string NumsPerPage = getPartParamValue(doc, "NumsPerPage");
                            string ExecBefore = getPartParamValue(doc, "ExecBefore");
                            string ExecAfter = getPartParamValue(doc, "ExecAfter");

                            if (!string.IsNullOrWhiteSpace(ApiSet))
                            {
                                string[] items = ApiSet.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string item in items)
                                {
                                    string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                                    string InputType = "form";
                                    if (colcfg.Length > 5) InputType = colcfg[5];
                                    string KvSet = "";
                                    if (colcfg.Length > 6) KvSet = colcfg[6];
                                    string ApiName = colcfg[0].Trim();
                                    string ApiPath = ApiName.StartsWith("/") ? ApiName : (subPath + "?" + ApiName);
                                    ApiPath = Api.ApiPathReplace(ApiPath);
                                    string KeyName = "";
                                    string Col1ReqName = "";
                                    string Col2ReqName = "";
                                    string Col3ReqName = "";
                                    string Col1Name = "";
                                    string Col2Name = "";
                                    string Col3Name = "";
                                    string[] keys = colcfg[2].Split(new string[] { "," }, StringSplitOptions.None);
                                    int StatCount = 0;
                                    for (int i = 0; i < keys.Length; i++)
                                    {
                                        if (keys[i] == "orderBy")
                                        { }
                                        else if (keys[i] == "orderType")
                                        { }
                                        else if (keys[i] == "schText")
                                        { }
                                        else if (keys[i] == "schStrict")
                                        { }
                                        else if (keys[i] == "schAdv")
                                        { }
                                        else if (keys[i] == "pageSize")
                                        { }
                                        else if (keys[i] == "numType")
                                        { }
                                        else if (keys[i] == "pageNum")
                                        { }
                                        else if (keys[i].StartsWith("keyValue"))
                                        {
                                            KeyName = keys[i].Substring(keys[i].IndexOf('.') + 1);
                                        }
                                        else
                                        {
                                            string[] statItem = keys[i].Split('.');
                                            if (StatCount == 0)
                                            {
                                                Col1ReqName = statItem[0];
                                                Col1Name = statItem[1];
                                            }
                                            else if (StatCount == 1)
                                            {
                                                Col2ReqName = statItem[0];
                                                Col2Name = statItem[1];
                                            }
                                            else if (StatCount == 2)
                                            {
                                                Col3ReqName = statItem[0];
                                                Col3Name = statItem[1];
                                            }
                                            StatCount++;
                                        }
                                    }
                                    sql = "delete from ft_ftdp_apiset where ApiPath='" + str.D2DD(ApiPath) + "'";
                                    db.ExecSql(sql);
                                    sql = "insert into ft_ftdp_apiset(ApiPath,PartID,PageID,ApiType,CustomConnection,Set_List_Order,Set_List_Sql,Set_List_SqlCount,Set_List_CusCondition,Set_List_RowAll)";
                                    sql += "values(@ApiPath,@PartID,@PageID,@ApiType,@CustomConnection,@Set_List_Order,@Set_List_Sql,@Set_List_SqlCount,@Set_List_CusCondition,@Set_List_RowAll)";
                                    db.ExecSql(sql, new PR[] {
                                    new PR("@ApiPath",ApiPath),
                                    new PR("@PartID",partid),
                                    new PR("@PageID",pageid),
                                    new PR("@ApiType","List"),
                                    new PR("@CustomConnection",CustomConnection),
                                    new PR("@Set_List_Order",OrderBy),
                                    new PR("@Set_List_Sql",CusSQL),
                                    new PR("@Set_List_SqlCount",CusSQLCount),
                                    new PR("@Set_List_CusCondition",CusCondition),
                                    new PR("@Set_List_RowAll",RowAll)
                                    });
                                    ApiDoc(db, "List", ApiPath, "fid", pagecaption, colcfg, RowAll, null, null, QianMing, pageid);
                                    #region 生成c#代码
                                    sb.Append("else if(key==\"" + ApiName + "\"){");
                                    sb.Append("if(!FTFrame.Project.Core.Api.Auth(\"List\",\"" + subPath + "\",paras,Context))json=FTFrame.Project.Core.Api.AuthFailedJson(\"List\",\"" + subPath + "\",paras,Context);");
                                    sb.Append("else{");
                                    sb.Append("Dictionary<string,string> setDic=new Dictionary<string,string>();");
                                    sb.Append("setDic.Add(\"partid\",\"" + partid + "\");");
                                    sb.Append("setDic.Add(\"Order\",\"" + OrderBy + "\");");
                                    sb.Append("setDic.Add(\"schdefine\",\"" + SearchDefine + "\");");
                                    sb.Append("setDic.Add(\"SiteID\",\"ftdp\");");
                                    sb.Append("setDic.Add(\"InputType\",\"" + InputType + "\");");
                                    sb.Append("setDic.Add(\"sql\",\"" + CusSQL.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n") + "\");");
                                    sb.Append("setDic.Add(\"sqlCount\",\"" + CusSQLCount.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n") + "\");");
                                    sb.Append("setDic.Add(\"RowAll\",\"" + RowAll.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n") + "\");");
                                    sb.Append("setDic.Add(\"Consts\",\"################################################\");");
                                    sb.Append("setDic.Add(\"BlockDataDefine\",\"" + BlockDataDefine + "\");");
                                    sb.Append("setDic.Add(\"UserCusCdn\",\"" + CusCondition + "\");");
                                    sb.Append("setDic.Add(\"CacuRowData\",\"" + CacuRowData + "\");");
                                    sb.Append("setDic.Add(\"CustomConnection\",\"" + CustomConnection + "\");");
                                    sb.Append("setDic.Add(\"MainTable\",\"" + MainTable + "\");");
                                    sb.Append("setDic.Add(\"NumsPerPage\",\"" + NumsPerPage + "\");");
                                    sb.Append("setDic.Add(\"KeyName\",\"" + KeyName + "\");");
                                    sb.Append("setDic.Add(\"Col1ReqName\",\"" + Col1ReqName + "\");");
                                    sb.Append("setDic.Add(\"Col2ReqName\",\"" + Col2ReqName + "\");");
                                    sb.Append("setDic.Add(\"Col3ReqName\",\"" + Col3ReqName + "\");");
                                    sb.Append("setDic.Add(\"Col1Name\",\"" + Col1Name + "\");");
                                    sb.Append("setDic.Add(\"Col2Name\",\"" + Col2Name + "\");");
                                    sb.Append("setDic.Add(\"Col3Name\",\"" + Col3Name + "\");");
                                    sb.Append("setDic.Add(\"ExecBefore\",\"" + ExecBefore.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    sb.Append("setDic.Add(\"ExecAfter\",\"" + ExecAfter.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    sb.Append("setDic.Add(\"KeyValueSet\",\"" + KvSet + "\");");
                                    sb.Append("json=FTFrame.Server.Core.Api.Json(\"List\",Context,setDic,paras,false);");
                                    sb.Append("setDic.Clear(); setDic = null;");
                                    sb.Append("}");
                                    sb.Append("}");
                                    #endregion

                                }
                            }
                        }
                        else if (controlname == "dyvalue")
                        {
                            string ApiSet = getPartParamValue(doc, "APISet");
                            string DefaultFID = getPartParamValue(doc, "DefaultFID");
                            string Define = getPartParamValue(doc, "Define");
                            string FidCol = getPartParamValue(doc, "FidCol");
                            int OpDefaultCol = int.Parse(getPartParamValue(doc, "OpDefaultCol"));
                            string ExecBefore = getPartParamValue(doc, "ExecBefore");
                            string ExecAfter = getPartParamValue(doc, "ExecAfter");
                            string CustomConnection = getPartParamValue(doc, "CustomConnection");

                            if (!string.IsNullOrWhiteSpace(ApiSet))
                            {
                                string[] items = ApiSet.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string item in items)
                                {
                                    string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                                    string ApiName = colcfg[0].Trim();
                                    string ApiPath = ApiName.StartsWith("/") ? ApiName : (subPath + "?" + ApiName);
                                    ApiPath = Api.ApiPathReplace(ApiPath);
                                    //string[] keys = colcfg[2].Split(new string[] { "," }, StringSplitOptions.None);
                                    sql = "delete from ft_ftdp_apiset where ApiPath='" + str.D2DD(ApiPath) + "'";
                                    db.ExecSql(sql);
                                    sql = "insert into ft_ftdp_apiset(ApiPath,PartID,PageID,ApiType,CustomConnection,Set_DyValue_DefineStr,Set_DyValue_ApiDefine,Set_DyValue_DefaultFID,Set_DyValue_FidCol,Set_DyValue_OpDefaultCol)";
                                    sql += "values(@ApiPath,@PartID,@PageID,@ApiType,@CustomConnection,@Set_DyValue_DefineStr,@Set_DyValue_ApiDefine,@Set_DyValue_DefaultFID,@Set_DyValue_FidCol,@Set_DyValue_OpDefaultCol)";
                                    db.ExecSql(sql, new PR[] {
                                    new PR("@ApiPath",ApiPath),
                                    new PR("@PartID",partid),
                                    new PR("@PageID",pageid),
                                    new PR("@ApiType","DyValue"),
                                    new PR("@CustomConnection",CustomConnection),
                                    new PR("@Set_DyValue_DefineStr",Define),
                                    new PR("@Set_DyValue_ApiDefine",item),
                                    new PR("@Set_DyValue_DefaultFID",DefaultFID),
                                    new PR("@Set_DyValue_FidCol",FidCol),
                                    new PR("@Set_DyValue_OpDefaultCol",OpDefaultCol)
                                    });
                                    ApiDoc(db, "DyValue", ApiPath, string.IsNullOrEmpty(FidCol) ? "fid" : FidCol, pagecaption, colcfg, null, ApiSet, null, QianMing, pageid);
                                    #region 生成c#代码
                                    sb.Append("else if(key==\"" + ApiName + "\"){");
                                    sb.Append("if(!FTFrame.Project.Core.Api.Auth(\"DyValue\",\"" + subPath + "\",paras,Context))json=FTFrame.Project.Core.Api.AuthFailedJson(\"DyValue\",\"" + subPath + "\",paras,Context);");
                                    sb.Append("else{");
                                    sb.Append("Dictionary<string,string> setDic=new Dictionary<string,string>();");
                                    sb.Append("setDic.Add(\"partid\",\"" + partid + "\");");
                                    sb.Append("setDic.Add(\"DefaultFID\",\"" + DefaultFID + "\");");
                                    sb.Append("setDic.Add(\"DefineStr\",\"" + Define.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n") + "\");");
                                    sb.Append("setDic.Add(\"SiteID\",\"ftdp\");");
                                    sb.Append("setDic.Add(\"ApiDefine\",\"" + item.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n") + "\");");
                                    sb.Append("setDic.Add(\"FidCol\",\"" + FidCol + "\");");
                                    sb.Append("setDic.Add(\"OpDefaultCol\",\"" + OpDefaultCol + "\");");
                                    sb.Append("setDic.Add(\"ExecBefore\",\"" + ExecBefore.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    sb.Append("setDic.Add(\"ExecAfter\",\"" + ExecAfter.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    sb.Append("setDic.Add(\"CustomConnection\",\"" + CustomConnection.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    sb.Append("json=FTFrame.Server.Core.Api.Json(\"DyValue\",Context,setDic,paras,false);");
                                    sb.Append("setDic.Clear(); setDic = null;");
                                    sb.Append("}");
                                    sb.Append("}");
                                    #endregion

                                }
                            }
                        }
                        else if (controlname == "dataop")
                        {
                            string ApiDefine = getPartParamValue(doc, "APISet");
                            string defaultfid = getPartParamValue(doc, "DefaultFID");
                            string DefineStr = getPartParamValue(doc, "Define");
                            string opid = getPartParamValue(doc, "OPID");
                            string jssuc = getPartParamValue(doc, "JSSuccess");
                            string codebefore = getPartParamValue(doc, "CodeBefore");
                            string codeafter = getPartParamValue(doc, "CodeAfter");
                            string cdnsql = getPartParamValue(doc, "OPContidionSql");
                            string cdncode = getPartParamValue(doc, "OPContidionCode");
                            string cdnjs = getPartParamValue(doc, "OPContidionJs");
                            string execsqlbefore = getPartParamValue(doc, "BeforeSql");
                            string execsqlafter = getPartParamValue(doc, "AfterSql");
                            string FidCol = getPartParamValue(doc, "FidCol");
                            int OpDefaultCol = int.Parse(getPartParamValue(doc, "OpDefaultCol"));
                            string CustomConnection = getPartParamValue(doc, "CustomConnection");

                            Dictionary<string, string> JsonSet = new Dictionary<string, string>();
                            string[] rows = DefineStr.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < rows.Length; i++)
                            {
                                string[] item = rows[i].Split(new string[] { "##" }, StringSplitOptions.None);
                                string name = item[1].Trim();
                                string json = null;
                                if (item.Length >= 9) json = item[8].Trim();
                                if (!string.IsNullOrEmpty(json) && json != "[]")
                                {
                                    if (!JsonSet.ContainsKey(name)) JsonSet.Add(name, json.Replace("$#$#$", "##"));
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(ApiDefine))
                            {
                                string[] items = ApiDefine.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string item in items)
                                {
                                    string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                                    string InputType = "form";
                                    if (colcfg.Length > 5) InputType = colcfg[5];
                                    string ApiName = colcfg[0].Trim();
                                    string ApiPath = ApiName.StartsWith("/") ? ApiName : (subPath + "?" + ApiName);
                                    ApiPath = Api.ApiPathReplace(ApiPath);
                                    Dictionary<string, string> Set_Dic = new Dictionary<string, string>();
                                    Set_Dic.Add("partid", partid);
                                    Set_Dic.Add("ApiDefine", item.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    Set_Dic.Add("defaultfid", defaultfid);
                                    Set_Dic.Add("SiteID", "ftdp");
                                    Set_Dic.Add("InputType", InputType);
                                    Set_Dic.Add("DefineStr", DefineStr.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    Set_Dic.Add("opid", opid);
                                    Set_Dic.Add("jssuc", jssuc.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    Set_Dic.Add("codebefore", codebefore);
                                    Set_Dic.Add("codeafter", codeafter);
                                    Set_Dic.Add("cdnsql", cdnsql.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    Set_Dic.Add("cdncode", cdncode);
                                    Set_Dic.Add("cdnjs", cdnjs.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    Set_Dic.Add("execsqlbefore", execsqlbefore.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    Set_Dic.Add("execsqlafter", execsqlafter.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    Set_Dic.Add("FidCol", FidCol);
                                    Set_Dic.Add("OpDefaultCol", OpDefaultCol.ToString());
                                    Set_Dic.Add("CustomConnection", CustomConnection.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    sql = "delete from ft_ftdp_apiset where ApiPath='" + str.D2DD(ApiPath) + "'";
                                    db.ExecSql(sql);
                                    sql = "insert into ft_ftdp_apiset(ApiPath,PartID,PageID,ApiType,CustomConnection,Set_DataOP_Dic)";
                                    sql += "values(@ApiPath,@PartID,@PageID,@ApiType,@CustomConnection,@Set_DataOP_Dic)";
                                    db.ExecSql(sql, new PR[] {
                                    new PR("@ApiPath",ApiPath),
                                    new PR("@PartID",partid),
                                    new PR("@PageID",pageid),
                                    new PR("@ApiType","DataOP"),
                                    new PR("@CustomConnection",CustomConnection),
                                    new PR("@Set_DataOP_Dic",func.DicToStr(Set_Dic))
                                    });
                                    ApiDoc(db, "DataOP", ApiPath, string.IsNullOrEmpty(FidCol) ? "fid" : FidCol, pagecaption, colcfg, null, null, ApiDefine, QianMing, pageid, JsonSet);
                                    #region 生成c#代码
                                    sb.Append("else if(key==\"" + ApiName + "\"){");
                                    sb.Append("if(!FTFrame.Project.Core.Api.Auth(\"DataOP\",\"" + subPath + "\",paras,Context))json=FTFrame.Project.Core.Api.AuthFailedJson(\"DataOP\",\"" + subPath + "\",paras,Context);");
                                    sb.Append("else{");
                                    sb.Append("Dictionary<string,string> setDic=new Dictionary<string,string>();");
                                    sb.Append("setDic.Add(\"partid\",\"" + Set_Dic["partid"] + "\");");
                                    sb.Append("setDic.Add(\"ApiDefine\",\"" + Set_Dic["ApiDefine"] + "\");");
                                    sb.Append("setDic.Add(\"defaultfid\",\"" + Set_Dic["defaultfid"] + "\");");
                                    sb.Append("setDic.Add(\"SiteID\",\"" + Set_Dic["SiteID"] + "\");");
                                    sb.Append("setDic.Add(\"InputType\",\"" + Set_Dic["InputType"] + "\");");
                                    sb.Append("setDic.Add(\"DefineStr\",\"" + Set_Dic["DefineStr"] + "\");");
                                    sb.Append("setDic.Add(\"opid\",\"" + Set_Dic["opid"] + "\");");
                                    sb.Append("setDic.Add(\"jssuc\",\"" + Set_Dic["jssuc"] + "\");");
                                    sb.Append("setDic.Add(\"codebefore\",\"" + Set_Dic["codebefore"] + "\");");
                                    sb.Append("setDic.Add(\"codeafter\",\"" + Set_Dic["codeafter"] + "\");");
                                    sb.Append("setDic.Add(\"cdnsql\",\"" + Set_Dic["cdnsql"] + "\");");
                                    sb.Append("setDic.Add(\"cdncode\",\"" + Set_Dic["cdncode"] + "\");");
                                    sb.Append("setDic.Add(\"cdnjs\",\"" + Set_Dic["cdnjs"] + "\");");
                                    sb.Append("setDic.Add(\"execsqlbefore\",\"" + Set_Dic["execsqlbefore"] + "\");");
                                    sb.Append("setDic.Add(\"execsqlafter\",\"" + Set_Dic["execsqlafter"] + "\");");
                                    sb.Append("setDic.Add(\"FidCol\",\"" + Set_Dic["FidCol"] + "\");");
                                    sb.Append("setDic.Add(\"OpDefaultCol\",\"" + Set_Dic["OpDefaultCol"] + "\");");
                                    sb.Append("setDic.Add(\"CustomConnection\",\"" + Set_Dic["CustomConnection"] + "\");");
                                    sb.Append("json=FTFrame.Server.Core.Api.Json(\"DataOP\",Context,setDic,paras,false);");
                                    sb.Append("setDic.Clear(); setDic = null;");
                                    sb.Append("}");
                                    sb.Append("}");
                                    Set_Dic.Clear();
                                    #endregion

                                }
                            }
                        }
                    }
                }
                command.Dispose();
            }
            #region 生成c#代码
            sb.Append("else");
            sb.Append("{");
            sb.Append("json=\"{\\\"code\\\":203,\\\"" + Project.Core.Api.MessageStr + "\\\":\\\"wrong key\\\"}\";");
            sb.Append("}");
            sb.Append("Context.Response.WriteAsync(json);");
            sb.Append("return;");
            sb.Append("}}}");
            #endregion

            string compileResult = CodeCompileApi(nameSpace + ".dll", sb);
            if (compileResult == null)
            {
                if (File.Exists(dirPath + Path.DirectorySeparatorChar + filename + ".cshtml"))
                {
                    File.Delete(dirPath + Path.DirectorySeparatorChar + filename + ".cshtml");
                }
                using (StreamWriter sr = new StreamWriter(dirPath + Path.DirectorySeparatorChar + filename + ".cshtml", false, new System.Text.UTF8Encoding(true)))
                {
                    sr.Write(AspxCode);
                }
                string route = subPath;
                if (route.StartsWith("/")) route = route.Substring(1);
                Api.MapPageRouteAdd(route, pageid);
            }
        }
        private static void ApiPagePublish_NoCompile(SqliteConnection conn, SqliteConnection conn2, string pageid, string pagecaption, string directPath, string filename, string QianMing)
        {
            string dirPath = SysConst.RootPath + "/Pages/" + directPath;
            dirPath = dirPath.Replace("/", Path.DirectorySeparatorChar.ToString());
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            string subPath = (directPath + filename).Replace(Path.DirectorySeparatorChar.ToString(), "/");
            DelApiPageOther(subPath);
            string sql = "select a.id,a.partxml,b.name from parts a,controls b,part_in_page c where a.id=c.partid and c.pageid='" + str.D2DD(pageid) + "' and a.controlid=b.id and ";
            sql += "((b.name='list' and a.name='List') or (b.name='dyvalue' and a.name='Interface') or (b.name='dataop' and a.name='Interface'))";
            StringBuilder sb = new StringBuilder();
            #region 生成c#代码
            sb.Append("@page");
            sb.Append(Environment.NewLine);
            var dirs = subPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var nameSpace = "ftdp.Pages";
            foreach (var dir in dirs)
            {
                nameSpace += "._" + dir.Replace("-", "_").Replace(" ", "");
            }
            sb.Append("@namespace " + nameSpace);
            sb.Append(Environment.NewLine);
            sb.Append("@using FTFrame;");
            sb.Append(Environment.NewLine);
            sb.Append("@using FTFrame.Tool;");
            sb.Append(Environment.NewLine);
            sb.Append("@using FTFrame.DBClient;");
            sb.Append(Environment.NewLine);
            sb.Append("@using FTFrame.Project.Core;");
            sb.Append(Environment.NewLine);
            sb.Append("@using Microsoft.AspNetCore.Http.Extensions;");
            sb.Append(Environment.NewLine);
            sb.Append("@{Layout = null;}");
            sb.Append(Environment.NewLine);
            //sb.Append("@{if (!ProjectFilter.Page(HttpContext)) return;}");
            //sb.Append(Environment.NewLine);
            sb.Append("@{");
            sb.Append(Environment.NewLine);
            sb.Append("Response.ContentType = \"application/Json;charset=utf-8\";");
            sb.Append(Environment.NewLine);
            sb.Append("string[] paras=FTFrame.Project.Core.Api.Paras(HttpContext);");
            sb.Append(Environment.NewLine);
            sb.Append("if(paras==null)");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("Output.Write(\"{\\\"code\\\":203,\\\"" + Project.Core.Api.MessageStr + "\\\":\\\"no path\\\"}\");");
            sb.Append(Environment.NewLine);
            sb.Append("return;");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            sb.Append(Environment.NewLine);
            sb.Append("string json=\"\";");
            sb.Append(Environment.NewLine);
            sb.Append("string key=paras[0];");
            sb.Append(Environment.NewLine);
            sb.Append("Dictionary<string, object> reqDic = new Dictionary<string, object>();");
            sb.Append(Environment.NewLine);
            sb.Append("reqDic.Add(\"_userinfo_\",new FTFrame.Project.Core.LoginInfo(HttpContext));");
            sb.Append(Environment.NewLine);
            sb.Append("if(0>1){}");
            sb.Append(Environment.NewLine);
            #endregion
            using (DB db = new DB(SysConst.ConnectionStr_FTDP))
            {
                db.Open();
                SqliteCommand command = new SqliteCommand(sql, conn);
                using (SqliteDataReader rdrSrc = command.ExecuteReader())
                {
                    while (rdrSrc.Read())
                    {
                        string partid = rdrSrc.GetString(rdrSrc.GetOrdinal("id"));
                        string partxml = rdrSrc.GetString(rdrSrc.GetOrdinal("partxml"));
                        string controlname = rdrSrc.GetString(rdrSrc.GetOrdinal("name"));
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(partxml);
                        if (controlname == "list")
                        {
                            string ApiSet = getPartParamValue(doc, "APISet");
                            string OrderBy = getPartParamValue(doc, "OrderBy");
                            string CusSQL = Sql.GetSqlForRemoveSameCols(getPartParamValue(doc, "CusSQL"));
                            string CusSQLCount = Sql.GetSqlForRemoveSameCols(getPartParamValue(doc, "CusSQLHalf"));
                            string RowAll = getPartParamValue(doc, "RowAll");
                            string SearchDefine = getPartParamValue(doc, "SearchDefine");
                            string BlockDataDefine = getPartParamValue(doc, "BlockDataDefine");
                            string CusCondition = getPartParamValue(doc, "CusCondition");
                            string CacuRowData = getPartParamValue(doc, "CacuRowData");
                            string CustomConnection = getPartParamValue(doc, "CustomConnection");
                            string MainTable = getPartParamValue(doc, "MainTable");
                            string NumsPerPage = getPartParamValue(doc, "NumsPerPage");
                            string ExecBefore = getPartParamValue(doc, "ExecBefore");
                            string ExecAfter = getPartParamValue(doc, "ExecAfter");

                            if (!string.IsNullOrWhiteSpace(ApiSet))
                            {
                                string[] items = ApiSet.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string item in items)
                                {
                                    string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                                    string InputType = "form";
                                    if (colcfg.Length > 5) InputType = colcfg[5];
                                    string KvSet = "";
                                    if (colcfg.Length > 6) KvSet = colcfg[6];
                                    string ApiName = colcfg[0].Trim();
                                    string ApiPath = ApiName.StartsWith("/") ? ApiName : (subPath + "?" + ApiName);
                                    ApiPath = Api.ApiPathReplace(ApiPath);
                                    string KeyName = "";
                                    string Col1ReqName = "";
                                    string Col2ReqName = "";
                                    string Col3ReqName = "";
                                    string Col1Name = "";
                                    string Col2Name = "";
                                    string Col3Name = "";
                                    string[] keys = colcfg[2].Split(new string[] { "," }, StringSplitOptions.None);
                                    int StatCount = 0;
                                    for (int i = 0; i < keys.Length; i++)
                                    {
                                        if (keys[i] == "orderBy")
                                        { }
                                        else if (keys[i] == "orderType")
                                        { }
                                        else if (keys[i] == "schText")
                                        { }
                                        else if (keys[i] == "schStrict")
                                        { }
                                        else if (keys[i] == "schAdv")
                                        { }
                                        else if (keys[i] == "pageSize")
                                        { }
                                        else if (keys[i] == "numType")
                                        { }
                                        else if (keys[i] == "pageNum")
                                        { }
                                        else if (keys[i].StartsWith("keyValue"))
                                        {
                                            KeyName = keys[i].Substring(keys[i].IndexOf('.') + 1);
                                        }
                                        else
                                        {
                                            string[] statItem = keys[i].Split('.');
                                            if (StatCount == 0)
                                            {
                                                Col1ReqName = statItem[0];
                                                Col1Name = statItem[1];
                                            }
                                            else if (StatCount == 1)
                                            {
                                                Col2ReqName = statItem[0];
                                                Col2Name = statItem[1];
                                            }
                                            else if (StatCount == 2)
                                            {
                                                Col3ReqName = statItem[0];
                                                Col3Name = statItem[1];
                                            }
                                            StatCount++;
                                        }
                                    }
                                    sql = "delete from ft_ftdp_apiset where ApiPath='" + str.D2DD(ApiPath) + "'";
                                    db.ExecSql(sql);
                                    sql = "insert into ft_ftdp_apiset(ApiPath,PartID,PageID,ApiType,CustomConnection,Set_List_Order,Set_List_Sql,Set_List_SqlCount,Set_List_CusCondition,Set_List_RowAll)";
                                    sql += "values(@ApiPath,@PartID,@PageID,@ApiType,@CustomConnection,@Set_List_Order,@Set_List_Sql,@Set_List_SqlCount,@Set_List_CusCondition,@Set_List_RowAll)";
                                    db.ExecSql(sql, new PR[] {
                                    new PR("@ApiPath",ApiPath),
                                    new PR("@PartID",partid),
                                    new PR("@PageID",pageid),
                                    new PR("@ApiType","List"),
                                    new PR("@CustomConnection",CustomConnection),
                                    new PR("@Set_List_Order",OrderBy),
                                    new PR("@Set_List_Sql",CusSQL),
                                    new PR("@Set_List_SqlCount",CusSQLCount),
                                    new PR("@Set_List_CusCondition",CusCondition),
                                    new PR("@Set_List_RowAll",RowAll)
                                    });
                                    ApiDoc(db, "List", ApiPath, "fid", pagecaption, colcfg, RowAll, null, null, QianMing, pageid);
                                    #region 生成c#代码
                                    sb.Append("else if(key==\"" + ApiName + "\"){");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("if(!FTFrame.Project.Core.Api.Auth(\"List\",\"" + subPath + "\",paras,HttpContext,reqDic))json=FTFrame.Project.Core.Api.AuthFailedJson(\"List\",\"" + subPath + "\",paras,HttpContext);");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("else{");
                                    sb.Append(Environment.NewLine);
                                    sb.AppendLine("Dictionary<string,string> setDic=new Dictionary<string,string>(){");
                                    sb.AppendLine("{\"partid\",\"" + partid + "\"},");
                                    sb.AppendLine("{\"Order\",\"" + OrderBy + "\"},");
                                    sb.AppendLine("{\"schdefine\",\"" + SearchDefine + "\"},");
                                    sb.AppendLine("{\"SiteID\",\"ftdp\"},");
                                    sb.AppendLine("{\"InputType\",\"" + InputType + "\"},");
                                    sb.AppendLine("{\"sql\",\"" + CusSQL.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\"},");
                                    sb.AppendLine("{\"sqlCount\",\"" + CusSQLCount.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\"},");
                                    sb.AppendLine("{\"RowAll\",\"" + RowAll.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\"},");
                                    sb.AppendLine("{\"Consts\",\"################################################\"},");
                                    sb.AppendLine("{\"BlockDataDefine\",\"" + BlockDataDefine.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\"},");
                                    sb.AppendLine("{\"UserCusCdn\",\"" + CusCondition.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\"},");
                                    sb.AppendLine("{\"CacuRowData\",\"" + CacuRowData.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\"},");
                                    sb.AppendLine("{\"CustomConnection\",\"" + CustomConnection.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\"},");
                                    sb.AppendLine("{\"MainTable\",\"" + MainTable + "\"},");
                                    sb.AppendLine("{\"NumsPerPage\",\"" + NumsPerPage + "\"},");
                                    sb.AppendLine("{\"KeyName\",\"" + KeyName + "\"},");
                                    sb.AppendLine("{\"Col1ReqName\",\"" + Col1ReqName + "\"},");
                                    sb.AppendLine("{\"Col2ReqName\",\"" + Col2ReqName + "\"},");
                                    sb.AppendLine("{\"Col3ReqName\",\"" + Col3ReqName + "\"},");
                                    sb.AppendLine("{\"Col1Name\",\"" + Col1Name + "\"},");
                                    sb.AppendLine("{\"Col2Name\",\"" + Col2Name + "\"},");
                                    sb.AppendLine("{\"Col3Name\",\"" + Col3Name + "\"},");
                                    sb.AppendLine("{\"ExecBefore\",\"" + ExecBefore.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\"},");
                                    sb.AppendLine("{\"ExecAfter\",\"" + ExecAfter.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\"},");
                                    sb.AppendLine("{\"KeyValueSet\",\"" + KvSet + "\"},");
                                    sb.AppendLine("};");
                                    //sb.Append("Dictionary<string,string> setDic=new Dictionary<string,string>();");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"partid\",\"" + partid + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"Order\",\"" + OrderBy + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"schdefine\",\"" + SearchDefine + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"SiteID\",\"ftdp\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"InputType\",\"" + InputType + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"sql\",\"" + CusSQL.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"","'") + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"sqlCount\",\"" + CusSQLCount.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"RowAll\",\"" + RowAll + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"Consts\",\"################################################\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"BlockDataDefine\",\"" + BlockDataDefine.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"UserCusCdn\",\"" + CusCondition.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"CacuRowData\",\"" + CacuRowData.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"CustomConnection\",\"" + CustomConnection.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"MainTable\",\"" + MainTable + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"NumsPerPage\",\"" + NumsPerPage + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"KeyName\",\"" + KeyName + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"Col1ReqName\",\"" + Col1ReqName + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"Col2ReqName\",\"" + Col2ReqName + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"Col3ReqName\",\"" + Col3ReqName + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"Col1Name\",\"" + Col1Name + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"Col2Name\",\"" + Col2Name + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"Col3Name\",\"" + Col3Name + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"ExecBefore\",\"" + ExecBefore.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"ExecAfter\",\"" + ExecAfter.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"KeyValueSet\",\"" + KvSet + "\");");
                                    //sb.Append(Environment.NewLine);
                                    sb.Append("json=FTFrame.Server.Core.Api.Json(\"List\",HttpContext,setDic,paras,false,null,0,\"\",reqDic);");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("setDic.Clear(); setDic = null;");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("}");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("}");
                                    #endregion

                                }
                            }
                        }
                        else if (controlname == "dyvalue")
                        {
                            string ApiSet = getPartParamValue(doc, "APISet");
                            string DefaultFID = getPartParamValue(doc, "DefaultFID");
                            string Define = getPartParamValue(doc, "Define");
                            string FidCol = getPartParamValue(doc, "FidCol");
                            int OpDefaultCol = int.Parse(getPartParamValue(doc, "OpDefaultCol"));
                            string ExecBefore = getPartParamValue(doc, "ExecBefore");
                            string ExecAfter = getPartParamValue(doc, "ExecAfter");
                            string CustomConnection = getPartParamValue(doc, "CustomConnection");

                            if (!string.IsNullOrWhiteSpace(ApiSet))
                            {
                                string[] items = ApiSet.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string item in items)
                                {
                                    string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                                    string ApiName = colcfg[0].Trim();
                                    string ApiPath = ApiName.StartsWith("/") ? ApiName : (subPath + "?" + ApiName);
                                    ApiPath = Api.ApiPathReplace(ApiPath);
                                    //string[] keys = colcfg[2].Split(new string[] { "," }, StringSplitOptions.None);
                                    sql = "delete from ft_ftdp_apiset where ApiPath='" + str.D2DD(ApiPath) + "'";
                                    db.ExecSql(sql);
                                    sql = "insert into ft_ftdp_apiset(ApiPath,PartID,PageID,ApiType,CustomConnection,Set_DyValue_DefineStr,Set_DyValue_ApiDefine,Set_DyValue_DefaultFID,Set_DyValue_FidCol,Set_DyValue_OpDefaultCol)";
                                    sql += "values(@ApiPath,@PartID,@PageID,@ApiType,@CustomConnection,@Set_DyValue_DefineStr,@Set_DyValue_ApiDefine,@Set_DyValue_DefaultFID,@Set_DyValue_FidCol,@Set_DyValue_OpDefaultCol)";
                                    db.ExecSql(sql, new PR[] {
                                    new PR("@ApiPath",ApiPath),
                                    new PR("@PartID",partid),
                                    new PR("@PageID",pageid),
                                    new PR("@ApiType","DyValue"),
                                    new PR("@CustomConnection",CustomConnection),
                                    new PR("@Set_DyValue_DefineStr",Define),
                                    new PR("@Set_DyValue_ApiDefine",item),
                                    new PR("@Set_DyValue_DefaultFID",DefaultFID),
                                    new PR("@Set_DyValue_FidCol",FidCol),
                                    new PR("@Set_DyValue_OpDefaultCol",OpDefaultCol)
                                    });
                                    ApiDoc(db, "DyValue", ApiPath, string.IsNullOrEmpty(FidCol) ? "fid" : FidCol, pagecaption, colcfg, null, ApiSet, null, QianMing, pageid);
                                    #region 生成c#代码
                                    sb.Append("else if(key==\"" + ApiName + "\"){");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("if(!FTFrame.Project.Core.Api.Auth(\"DyValue\",\"" + subPath + "\",paras,HttpContext,reqDic))json=FTFrame.Project.Core.Api.AuthFailedJson(\"DyValue\",\"" + subPath + "\",paras,HttpContext);");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("else{");
                                    sb.Append(Environment.NewLine);
                                    sb.AppendLine("Dictionary<string,string> setDic=new Dictionary<string,string>(){");
                                    sb.AppendLine("{\"partid\",\"" + partid + "\"},");
                                    sb.AppendLine("{\"DefaultFID\",\"" + DefaultFID + "\"},");
                                    sb.AppendLine("{\"DefineStr\",\"" + Define.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\"},");
                                    sb.AppendLine("{\"SiteID\",\"ftdp\"},");
                                    sb.AppendLine("{\"ApiDefine\",\"" + item.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\"},");
                                    sb.AppendLine("{\"FidCol\",\"" + FidCol + "\"},");
                                    sb.AppendLine("{\"OpDefaultCol\",\"" + OpDefaultCol + "\"},");
                                    sb.AppendLine("{\"ExecBefore\",\"" + ExecBefore.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\"},");
                                    sb.AppendLine("{\"ExecAfter\",\"" + ExecAfter.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\"},");
                                    sb.AppendLine("{\"CustomConnection\",\"" + CustomConnection.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\"},");
                                    sb.AppendLine("};");
                                    //sb.Append("Dictionary<string,string> setDic=new Dictionary<string,string>();");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"partid\",\"" + partid + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"DefaultFID\",\"" + DefaultFID + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"DefineStr\",\"" + Define.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"SiteID\",\"ftdp\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"ApiDefine\",\"" + item.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"FidCol\",\"" + FidCol + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"OpDefaultCol\",\"" + OpDefaultCol + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"ExecBefore\",\"" + ExecBefore.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"ExecAfter\",\"" + ExecAfter.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"CustomConnection\",\"" + CustomConnection.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'") + "\");");
                                    //sb.Append(Environment.NewLine);
                                    sb.Append("json=FTFrame.Server.Core.Api.Json(\"DyValue\",HttpContext,setDic,paras,false,null,0,\"\",reqDic);");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("setDic.Clear(); setDic = null;");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("}");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("}");
                                    #endregion

                                }
                            }
                        }
                        else if (controlname == "dataop")
                        {
                            string ApiDefine = getPartParamValue(doc, "APISet");
                            string defaultfid = getPartParamValue(doc, "DefaultFID");
                            string DefineStr = getPartParamValue(doc, "Define");
                            string opid = getPartParamValue(doc, "OPID");
                            string jssuc = getPartParamValue(doc, "JSSuccess");
                            string codebefore = getPartParamValue(doc, "CodeBefore");
                            string codeafter = getPartParamValue(doc, "CodeAfter");
                            string cdnsql = getPartParamValue(doc, "OPContidionSql");
                            string cdncode = getPartParamValue(doc, "OPContidionCode");
                            string cdnjs = getPartParamValue(doc, "OPContidionJs");
                            string execsqlbefore = getPartParamValue(doc, "BeforeSql");
                            string execsqlafter = getPartParamValue(doc, "AfterSql");
                            string FidCol = getPartParamValue(doc, "FidCol");
                            int OpDefaultCol = int.Parse(getPartParamValue(doc, "OpDefaultCol"));
                            string CustomConnection = getPartParamValue(doc, "CustomConnection");
                            Dictionary<string, string> JsonSet = new Dictionary<string, string>();
                            string[] rows = DefineStr.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < rows.Length; i++)
                            {
                                string[] item = rows[i].Split(new string[] { "##" }, StringSplitOptions.None);
                                string name = item[1].Trim();
                                string json = null;
                                if (item.Length >= 9) json = item[8].Trim();
                                if (!string.IsNullOrEmpty(json) && json != "[]")
                                {
                                    if (!JsonSet.ContainsKey(name)) JsonSet.Add(name, json.Replace("$#$#$", "##"));
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(ApiDefine))
                            {
                                string[] items = ApiDefine.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string item in items)
                                {
                                    string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                                    string InputType = "form";
                                    if (colcfg.Length > 5) InputType = colcfg[5];
                                    string ApiName = colcfg[0].Trim();
                                    string ApiPath = ApiName.StartsWith("/") ? ApiName : (subPath + "?" + ApiName);
                                    ApiPath = Api.ApiPathReplace(ApiPath);
                                    Dictionary<string, string> Set_Dic = new Dictionary<string, string>();
                                    Set_Dic.Add("partid", partid);
                                    Set_Dic.Add("ApiDefine", item.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    Set_Dic.Add("defaultfid", defaultfid);
                                    Set_Dic.Add("SiteID", "ftdp");
                                    Set_Dic.Add("InputType", InputType);
                                    Set_Dic.Add("DefineStr", DefineStr.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    Set_Dic.Add("opid", opid);
                                    Set_Dic.Add("jssuc", jssuc.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    Set_Dic.Add("codebefore", codebefore);
                                    Set_Dic.Add("codeafter", codeafter);
                                    Set_Dic.Add("cdnsql", cdnsql.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    Set_Dic.Add("cdncode", cdncode);
                                    Set_Dic.Add("cdnjs", cdnjs.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    Set_Dic.Add("execsqlbefore", execsqlbefore.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    Set_Dic.Add("execsqlafter", execsqlafter.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    Set_Dic.Add("FidCol", FidCol);
                                    Set_Dic.Add("OpDefaultCol", OpDefaultCol.ToString());
                                    Set_Dic.Add("CustomConnection", CustomConnection.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\r\\n").Replace("\"", "'"));
                                    sql = "delete from ft_ftdp_apiset where ApiPath='" + str.D2DD(ApiPath) + "'";
                                    db.ExecSql(sql);
                                    sql = "insert into ft_ftdp_apiset(ApiPath,PartID,PageID,ApiType,CustomConnection,Set_DataOP_Dic)";
                                    sql += "values(@ApiPath,@PartID,@PageID,@ApiType,@CustomConnection,@Set_DataOP_Dic)";
                                    db.ExecSql(sql, new PR[] {
                                    new PR("@ApiPath",ApiPath),
                                    new PR("@PartID",partid),
                                    new PR("@PageID",pageid),
                                    new PR("@ApiType","DataOP"),
                                    new PR("@CustomConnection",CustomConnection),
                                    new PR("@Set_DataOP_Dic",func.DicToStr(Set_Dic))
                                    });
                                    ApiDoc(db, "DataOP", ApiPath, string.IsNullOrEmpty(FidCol) ? "fid" : FidCol, pagecaption, colcfg, null, null, ApiDefine, QianMing, pageid, JsonSet);
                                    #region 生成c#代码
                                    sb.Append("else if(key==\"" + ApiName + "\"){");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("if(!FTFrame.Project.Core.Api.Auth(\"DataOP\",\"" + subPath + "\",paras,HttpContext,reqDic))json=FTFrame.Project.Core.Api.AuthFailedJson(\"DataOP\",\"" + subPath + "\",paras,HttpContext);");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("else{");
                                    sb.Append(Environment.NewLine);
                                    sb.AppendLine("Dictionary<string,string> setDic=new Dictionary<string,string>(){");
                                    sb.AppendLine("{\"partid\",\"" + Set_Dic["partid"] + "\"},");
                                    sb.AppendLine("{\"ApiDefine\",\"" + Set_Dic["ApiDefine"] + "\"},");
                                    sb.AppendLine("{\"defaultfid\",\"" + Set_Dic["defaultfid"] + "\"},");
                                    sb.AppendLine("{\"SiteID\",\"" + Set_Dic["SiteID"] + "\"},");
                                    sb.AppendLine("{\"InputType\",\"" + Set_Dic["InputType"] + "\"},");
                                    sb.AppendLine("{\"DefineStr\",\"" + Set_Dic["DefineStr"] + "\"},");
                                    sb.AppendLine("{\"opid\",\"" + Set_Dic["opid"] + "\"},");
                                    sb.AppendLine("{\"jssuc\",\"" + Set_Dic["jssuc"] + "\"},");
                                    sb.AppendLine("{\"codebefore\",\"" + Set_Dic["codebefore"] + "\"},");
                                    sb.AppendLine("{\"codeafter\",\"" + Set_Dic["codeafter"] + "\"},");
                                    sb.AppendLine("{\"cdnsql\",\"" + Set_Dic["cdnsql"] + "\"},");
                                    sb.AppendLine("{\"cdncode\",\"" + Set_Dic["cdncode"] + "\"},");
                                    sb.AppendLine("{\"cdnjs\",\"" + Set_Dic["cdnjs"] + "\"},");
                                    sb.AppendLine("{\"execsqlbefore\",\"" + Set_Dic["execsqlbefore"] + "\"},");
                                    sb.AppendLine("{\"execsqlafter\",\"" + Set_Dic["execsqlafter"] + "\"},");
                                    sb.AppendLine("{\"FidCol\",\"" + Set_Dic["FidCol"] + "\"},");
                                    sb.AppendLine("{\"OpDefaultCol\",\"" + Set_Dic["OpDefaultCol"] + "\"},");
                                    sb.AppendLine("{\"CustomConnection\",\"" + Set_Dic["CustomConnection"] + "\"},");
                                    sb.AppendLine("};");
                                    //sb.Append("Dictionary<string,string> setDic=new Dictionary<string,string>();");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"partid\",\"" + Set_Dic["partid"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"ApiDefine\",\"" + Set_Dic["ApiDefine"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"defaultfid\",\"" + Set_Dic["defaultfid"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"SiteID\",\"" + Set_Dic["SiteID"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"InputType\",\"" + Set_Dic["InputType"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"DefineStr\",\"" + Set_Dic["DefineStr"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"opid\",\"" + Set_Dic["opid"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"jssuc\",\"" + Set_Dic["jssuc"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"codebefore\",\"" + Set_Dic["codebefore"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"codeafter\",\"" + Set_Dic["codeafter"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"cdnsql\",\"" + Set_Dic["cdnsql"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"cdncode\",\"" + Set_Dic["cdncode"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"cdnjs\",\"" + Set_Dic["cdnjs"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"execsqlbefore\",\"" + Set_Dic["execsqlbefore"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"execsqlafter\",\"" + Set_Dic["execsqlafter"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"FidCol\",\"" + Set_Dic["FidCol"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"OpDefaultCol\",\"" + Set_Dic["OpDefaultCol"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    //sb.Append("setDic.Add(\"CustomConnection\",\"" + Set_Dic["CustomConnection"] + "\");");
                                    //sb.Append(Environment.NewLine);
                                    sb.Append("json=FTFrame.Server.Core.Api.Json(\"DataOP\",HttpContext,setDic,paras,false,null,0,\"\",reqDic);");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("setDic.Clear(); setDic = null;");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("}");
                                    sb.Append(Environment.NewLine);
                                    sb.Append("}");
                                    Set_Dic.Clear();
                                    #endregion

                                }
                            }
                        }
                    }
                }
                command.Dispose();
            }
            #region 生成c#代码
            sb.Append("else");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("json=\"{\\\"code\\\":203,\\\"" + Project.Core.Api.MessageStr + "\\\":\\\"wrong key\\\"}\";");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            sb.Append(Environment.NewLine);
            sb.Append("reqDic.Clear();");
            sb.Append(Environment.NewLine);
            sb.Append("reqDic = null;");
            sb.Append(Environment.NewLine);
            sb.Append("Output.Write(json);");
            sb.Append(Environment.NewLine);
            sb.Append("return;");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            sb.Append(Environment.NewLine);
            #endregion
            if (File.Exists(dirPath + Path.DirectorySeparatorChar + filename + ".cshtml"))
            {
                File.Delete(dirPath + Path.DirectorySeparatorChar + filename + ".cshtml");
            }
            using (StreamWriter sr = new StreamWriter(dirPath + Path.DirectorySeparatorChar + filename + ".cshtml", false, new System.Text.UTF8Encoding(true)))
            {
                sr.Write(sb.ToString());
            }

            string route = subPath;
            if (route.StartsWith("/")) route = route.Substring(1);
            Api.MapPageRouteAdd(route, pageid);
        }
        private static void ApiPagePublish_Java(SqliteConnection conn, SqliteConnection conn2, string pageid, string pagecaption, string directPath, string filename, string QianMing)
        {
            string rootPath = JavaRootPath;
            if (string.IsNullOrWhiteSpace(rootPath)) return;
            rootPath = Path.GetFullPath(rootPath);
            string pubPath = rootPath + "/project/src/main/java";
            var pathArrays = directPath.Split(new string[] { "/", "\\" }, StringSplitOptions.RemoveEmptyEntries);
            pubPath += Path.DirectorySeparatorChar + "com" + Path.DirectorySeparatorChar + "ftframe" + Path.DirectorySeparatorChar + "api";
            foreach (var path in pathArrays)
            {
                pubPath += Path.DirectorySeparatorChar + path.Replace("-", "_").Replace(" ", "");
            }
            pubPath = pubPath.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString());
            if (!Directory.Exists(pubPath)) Directory.CreateDirectory(pubPath);
            string subPath = (directPath + filename).Replace(Path.DirectorySeparatorChar.ToString(), "/");
            //DelApiPageOther(subPath);
            string sql = "select a.id,a.partxml,b.name from parts a,controls b,part_in_page c where a.id=c.partid and c.pageid='" + str.D2DD(pageid) + "' and a.controlid=b.id and ";
            sql += "((b.name='list' and a.name='List') or (b.name='dyvalue' and a.name='Interface') or (b.name='dataop' and a.name='Interface'))";
            var oriFilename = filename;
            filename = filename.Replace("-", "_").Replace(" ", "");
            StringBuilder sb = new StringBuilder();
            #region 生成java代码
            var packageString = "com.ftframe.api";
            string apiPath = "";
            foreach (var path in pathArrays)
            {
                packageString += "." + path.Replace("-", "_").Replace(" ", "");
                //apiPath += "/" + path.Replace("-", "_").Replace(" ", "");
                apiPath += "/" + path.Replace(" ", "");
            }
            apiPath += "/" + oriFilename;
            if(packageString.EndsWith("."+ filename) || packageString == filename)
            {
                filename += "_PACKNAMESAME";
            }
            sb.AppendLine("package " + packageString + ";");
            sb.AppendLine("import javax.servlet.http.HttpServletRequest;");
            sb.AppendLine("import javax.servlet.http.HttpServletResponse;");
            sb.AppendLine("import com.ftframe.project.*;");
            sb.AppendLine("import com.ftframe.server.*;");
            sb.AppendLine("import org.springframework.web.bind.annotation.RequestMapping;");
            sb.AppendLine("import org.springframework.web.bind.annotation.RequestMethod;");
            sb.AppendLine("import org.springframework.web.bind.annotation.RestController;");
            sb.AppendLine("import org.springframework.web.context.request.RequestContextHolder;");
            sb.AppendLine("import org.springframework.web.context.request.ServletRequestAttributes;");
            sb.AppendLine("import java.util.HashMap;");
            sb.AppendLine("@RestController");
            sb.AppendLine("public class " + filename + " {");
            SBA(sb, 1, "@RequestMapping(value = \"" + apiPath + "\", method = {RequestMethod.POST, RequestMethod.GET}, produces = \"application/json;charset=UTF-8\")");
            SBA(sb, 1, "String singleMethod() {");
            SBA(sb, 2, "ServletRequestAttributes servletRequestAttributes = (ServletRequestAttributes) RequestContextHolder.getRequestAttributes();");
            SBA(sb, 2, "HttpServletRequest request = servletRequestAttributes.getRequest();");
            SBA(sb, 2, "HttpServletResponse response = servletRequestAttributes.getResponse();");
            SBA(sb, 2, "String[] paras = ProApi.Paras(request, response);");
            SBA(sb, 2, "if (paras == null || paras.length == 0) {");
            SBA(sb, 3, "return \"{\\\"code\\\":203,\\\"msg\\\":\\\"no path\\\"}\";");
            SBA(sb, 2, "}");
            SBA(sb, 2, "String json = \"\";");
            SBA(sb, 2, "String key = paras[0];");
            SBA(sb, 2, "HashMap<String, Object> reqMap = new HashMap(){");
            SBA(sb, 3, "{");
            SBA(sb, 4, "put(\"_userinfo_\", User.GetUserInfo(request, response));");
            SBA(sb, 3, "}");
            SBA(sb, 2, "};");
            SBA(sb, 2, "if (false) {");
            using (DB db = new DB(SysConst.ConnectionStr_FTDP))
            {
                db.Open();
                SqliteCommand command = new SqliteCommand(sql, conn);
                using (SqliteDataReader rdrSrc = command.ExecuteReader())
                {
                    while (rdrSrc.Read())
                    {
                        string partid = rdrSrc.GetString(rdrSrc.GetOrdinal("id"));
                        string partxml = rdrSrc.GetString(rdrSrc.GetOrdinal("partxml"));
                        string controlname = rdrSrc.GetString(rdrSrc.GetOrdinal("name"));
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(partxml);
                        if (controlname == "list")
                        {
                            string ApiSet = getPartParamValue(doc, "APISet");
                            string OrderBy = getPartParamValue(doc, "OrderBy");
                            string CusSQL = Sql.GetSqlForRemoveSameCols(getPartParamValue(doc, "CusSQL"));
                            string CusSQLCount = Sql.GetSqlForRemoveSameCols(getPartParamValue(doc, "CusSQLHalf"));
                            string RowAll = getPartParamValue(doc, "RowAll");
                            string SearchDefine = getPartParamValue(doc, "SearchDefine");
                            string BlockDataDefine = getPartParamValue(doc, "BlockDataDefine");
                            string CusCondition = getPartParamValue(doc, "CusCondition");
                            string CacuRowData = getPartParamValue(doc, "CacuRowData");
                            string CustomConnection = getPartParamValue(doc, "CustomConnection");
                            string MainTable = getPartParamValue(doc, "MainTable");
                            string NumsPerPage = getPartParamValue(doc, "NumsPerPage");
                            string ExecBefore = getPartParamValue(doc, "ExecBefore");
                            string ExecAfter = getPartParamValue(doc, "ExecAfter");

                            if (!string.IsNullOrWhiteSpace(ApiSet))
                            {
                                string[] items = ApiSet.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string item in items)
                                {
                                    string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                                    string InputType = "form";
                                    if (colcfg.Length > 5) InputType = colcfg[5];
                                    string KvSet = "";
                                    if (colcfg.Length > 6) KvSet = colcfg[6];
                                    string ApiName = colcfg[0].Trim();
                                    string ApiPath = ApiName.StartsWith("/") ? ApiName : (subPath + "?" + ApiName);
                                    ApiPath = Api.ApiPathReplace(ApiPath);
                                    string KeyName = "";
                                    string Col1ReqName = "";
                                    string Col2ReqName = "";
                                    string Col3ReqName = "";
                                    string Col1Name = "";
                                    string Col2Name = "";
                                    string Col3Name = "";
                                    string[] keys = colcfg[2].Split(new string[] { "," }, StringSplitOptions.None);
                                    int StatCount = 0;
                                    for (int i = 0; i < keys.Length; i++)
                                    {
                                        if (keys[i] == "orderBy")
                                        { }
                                        else if (keys[i] == "orderType")
                                        { }
                                        else if (keys[i] == "schText")
                                        { }
                                        else if (keys[i] == "schStrict")
                                        { }
                                        else if (keys[i] == "schAdv")
                                        { }
                                        else if (keys[i] == "pageSize")
                                        { }
                                        else if (keys[i] == "numType")
                                        { }
                                        else if (keys[i] == "pageNum")
                                        { }
                                        else if (keys[i].StartsWith("keyValue"))
                                        {
                                            KeyName = keys[i].Substring(keys[i].IndexOf('.') + 1);
                                        }
                                        else
                                        {
                                            string[] statItem = keys[i].Split('.');
                                            if (StatCount == 0)
                                            {
                                                Col1ReqName = statItem[0];
                                                Col1Name = statItem[1];
                                            }
                                            else if (StatCount == 1)
                                            {
                                                Col2ReqName = statItem[0];
                                                Col2Name = statItem[1];
                                            }
                                            else if (StatCount == 2)
                                            {
                                                Col3ReqName = statItem[0];
                                                Col3Name = statItem[1];
                                            }
                                            StatCount++;
                                        }
                                    }
                                    #region 生成java代码
                                    SBA(sb, 2, "} else if (key.equals(\"" + ApiName + "\")) {");
                                    SBA(sb, 3, "if (!ProApi.Auth(\"List\", \"" + subPath + "\", paras, request, response,reqMap))");
                                    SBA(sb, 4, "json = ProApi.AuthFailedJson(\"List\", \"" + subPath + "\", paras, request, response,reqMap);");
                                    SBA(sb, 3, "else {");
                                    SBA(sb, 4, "HashMap<String, Object> setMap = new HashMap<String, Object>() {");
                                    SBA(sb, 5, "{");
                                    SBA(sb, 6, "put(\"partid\", \"" + partid + "\");");
                                    SBA(sb, 6, "put(\"SiteID\", \"ftdp\");");
                                    SBA(sb, 6, "put(\"Order\", \"" + OrderBy + "\");");
                                    SBA(sb, 6, "put(\"schdefine\", \"" + SearchDefine + "\");");
                                    SBA(sb, 6, "put(\"InputType\", \"" + InputType + "\");");
                                    SBA(sb, 6, "put(\"sql\", \"" + CusSQL.NoWrap() + "\");");
                                    SBA(sb, 6, "put(\"sqlCount\", \"" + CusSQLCount.NoWrap() + "\");");
                                    SBA(sb, 6, "put(\"RowAll\",");
                                    SBA(sb, 7, "new String[][]{");
                                    string[] rows = RowAll.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (string rowstr in rows)
                                    {
                                        SBA(sb, 8, "{");
                                        string row = rowstr.Substring(0, rowstr.IndexOf("&&&")).Trim();
                                        string[] rowcols = row.Split('#');
                                        string title = rowcols[0];
                                        string data = str.GetDecode(rowcols[1]);
                                        string align = rowcols[2];
                                        string ordercol = rowcols[3];
                                        string clickjs = rowcols[4];
                                        string caption = rowcols[5];
                                        string saveto = rowcols[6];
                                        int isshow = ((rowcols.Length > 7 && rowcols[7].Equals("1")) ? 1 : 0);
                                        string opencdn = rowstr.Substring(rowstr.IndexOf("&&&") + 3).Trim();
                                        SBA(sb, 9, "\"" + title.NoDotNoWrap() + "\", \"" + data.NoWrap() + "\", \"" + align.NoDotNoWrap() + "\", \"" + ordercol.NoDotNoWrap() + "\", \"" + clickjs.NoDotNoWrap() + "\", \"" + caption.NoWrap() + "\", \"" + saveto.NoDotNoWrap() + "\", \"" + isshow + "\", \"" + opencdn.NoWrap() + "\"");
                                        SBA(sb, 8, "},");
                                    }
                                    SBA(sb, 7, "}");
                                    SBA(sb, 6, ");");
                                    SBA(sb, 6, "put(\"Consts\", \"################################################\");");
                                    SBA(sb, 6, "put(\"BlockDataDefine\", \"" + BlockDataDefine.NoWrap() + "\");");
                                    SBA(sb, 6, "put(\"UserCusCdn\", \"" + CusCondition.NoWrap() + "\");");
                                    SBA(sb, 6, "put(\"CacuRowData\", \"" + CacuRowData.NoWrap() + "\");");
                                    SBA(sb, 6, "put(\"CustomConnection\", \"" + CustomConnection.NoWrap() + "\");");
                                    SBA(sb, 6, "put(\"MainTable\", \"" + MainTable + "\");");
                                    SBA(sb, 6, "put(\"NumsPerPage\", \"" + NumsPerPage + "\");");
                                    SBA(sb, 6, "put(\"KeyName\", \"" + KeyName + "\");");
                                    SBA(sb, 6, "put(\"Col1ReqName\", \"" + Col1ReqName + "\");");
                                    SBA(sb, 6, "put(\"Col2ReqName\", \"" + Col2ReqName + "\");");
                                    SBA(sb, 6, "put(\"Col3ReqName\", \"" + Col3ReqName + "\");");
                                    SBA(sb, 6, "put(\"Col1Name\", \"" + Col1Name + "\");");
                                    SBA(sb, 6, "put(\"Col2Name\", \"" + Col2Name + "\");");
                                    SBA(sb, 6, "put(\"Col3Name\", \"" + Col3Name + "\");");
                                    SBA(sb, 6, "put(\"ExecBefore\", \"" + ExecBefore.NoWrap() + "\");");
                                    SBA(sb, 6, "put(\"ExecAfter\", \"" + ExecAfter.NoWrap() + "\");");
                                    SBA(sb, 6, "put(\"KeyValueSet\",");
                                    SBA(sb, 7, "new HashMap<String, String>() {");
                                    SBA(sb, 8, "{");
                                    var kvSetDic = Api.KeyValueSetDic(KvSet);
                                    foreach (var ksItem in kvSetDic)
                                    {
                                        SBA(sb, 9, "put(\"" + ksItem.Key + "\", \"" + ksItem.Value.NoWrap() + "\");");
                                    }
                                    kvSetDic.Clear();
                                    SBA(sb, 8, "}");
                                    SBA(sb, 7, "}");
                                    SBA(sb, 6, ");");
                                    SBA(sb, 5, "}");
                                    SBA(sb, 4, "};");
                                    SBA(sb, 4, "json = Api.Json(\"List\", paras, setMap, reqMap, request, response, false, null, 0, \"\");");
                                    SBA(sb, 4, "setMap.clear();");
                                    SBA(sb, 4, "setMap = null;");
                                    SBA(sb, 3, "}");
                                    #endregion

                                }
                            }
                        }
                        else if (controlname == "dyvalue")
                        {
                            string ApiSet = getPartParamValue(doc, "APISet");
                            string DefaultFID = getPartParamValue(doc, "DefaultFID");
                            string Define = getPartParamValue(doc, "Define");
                            string FidCol = getPartParamValue(doc, "FidCol");
                            int OpDefaultCol = int.Parse(getPartParamValue(doc, "OpDefaultCol"));
                            string ExecBefore = getPartParamValue(doc, "ExecBefore");
                            string ExecAfter = getPartParamValue(doc, "ExecAfter");
                            string CustomConnection = getPartParamValue(doc, "CustomConnection");

                            if (!string.IsNullOrWhiteSpace(ApiSet))
                            {
                                string[] items = ApiSet.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string item in items)
                                {
                                    string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                                    string ApiName = colcfg[0].Trim();
                                    string ApiPath = ApiName.StartsWith("/") ? ApiName : (subPath + "?" + ApiName);
                                    ApiPath = Api.ApiPathReplace(ApiPath);
                                    #region 生成java代码
                                    SBA(sb, 2, "} else if (key.equals(\"" + ApiName + "\")) {");
                                    SBA(sb, 3, "if (!ProApi.Auth(\"DyValue\", \"" + subPath + "\", paras, request, response,reqMap))");
                                    SBA(sb, 4, "json = ProApi.AuthFailedJson(\"DyValue\", \"" + subPath + "\", paras, request, response,reqMap);");
                                    SBA(sb, 3, "else {");
                                    SBA(sb, 4, "HashMap<String, Object> setMap = new HashMap<String, Object>() {");
                                    SBA(sb, 5, "{");
                                    SBA(sb, 6, "put(\"partid\", \"" + partid + "\");");
                                    SBA(sb, 6, "put(\"SiteID\", \"ftdp\");");
                                    SBA(sb, 6, "put(\"DefaultFID\", \"" + DefaultFID.NoDotNoWrap() + "\");");
                                    SBA(sb, 6, "put(\"DefineStr\",");
                                    SBA(sb, 7, "new String[][]{");
                                    string[] rowsDefine = Define.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (string rowstr in rowsDefine)
                                    {
                                        SBA(sb, 8, "{");
                                        string[] rowcols = rowstr.Split(new string[] { "##" }, StringSplitOptions.None);
                                        string caption = rowcols[0];
                                        string id = rowcols[1].Trim();
                                        string tabletag = str.GetDecode(rowcols[2].Trim());
                                        string fid = rowcols[3].Trim();
                                        string isasync = rowcols[4];
                                        int isdy = int.Parse(rowcols[5]);
                                        int isdim = int.Parse(rowcols[6]);
                                        string adv = str.GetDecode(rowcols[7].Trim());
                                        SBA(sb, 9, "\"" + caption.NoDotNoWrap() + "\", \"" + id.NoDotNoWrap() + "\", \"" + tabletag.NoWrap() + "\", \"" + fid.NoWrap() + "\", \"" + isasync.NoDotNoWrap() + "\", \"" + isdy + "\", \"" + isdim + "\", \"" + adv.NoWrap() + "\"");
                                        SBA(sb, 8, "},");
                                    }
                                    SBA(sb, 7, "}");
                                    SBA(sb, 6, ");");
                                    SBA(sb, 6, "put(\"ApiDefine\",");
                                    SBA(sb, 7, "new Object[]{");
                                    string ApiDesc = colcfg[1].Trim();
                                    SBA(sb, 8, "\"" + ApiName + "\", \"" + ApiDesc.NoDotNoWrap() + "\",");
                                    SBA(sb, 8, "new String[][]{");
                                    string[] apiKeys = colcfg[2].Split(new string[] { "[#]" }, StringSplitOptions.None);
                                    string[] colKeys = colcfg[3].Split(new string[] { "[#]" }, StringSplitOptions.None);
                                    string[] colDesc = colcfg[4].Split(new string[] { "[#]" }, StringSplitOptions.None);
                                    string[] linkKeys = colcfg[5].Split(new string[] { "[#]" }, StringSplitOptions.None);
                                    for (var i = 0; i < apiKeys.Length; i++)
                                    {
                                        SBA(sb, 9, "{");
                                        SBA(sb, 10, "\"" + apiKeys[i].NoDotNoWrap() + "\", \"" + colKeys[i].NoDotNoWrap() + "\", \"" + colDesc[i].NoDotNoWrap() + "\", \"" + linkKeys[i].NoWrap() + "\"");
                                        SBA(sb, 9, "},");
                                    }
                                    SBA(sb, 8, "}");
                                    SBA(sb, 7, "}");
                                    SBA(sb, 6, ");");
                                    SBA(sb, 6, "put(\"FidCol\", \"" + FidCol.NoDotNoWrap() + "\");");
                                    SBA(sb, 6, "put(\"OpDefaultCol\", \"" + OpDefaultCol + "\");");
                                    SBA(sb, 6, "put(\"ExecBefore\", \"" + ExecBefore.NoWrap() + "\");");
                                    SBA(sb, 6, "put(\"ExecAfter\", \"" + ExecAfter.NoWrap() + "\");");
                                    SBA(sb, 6, "put(\"CustomConnection\", \"" + CustomConnection.NoWrap() + "\");");
                                    SBA(sb, 5, "}");
                                    SBA(sb, 4, "};");
                                    SBA(sb, 4, "json = Api.Json(\"DyValue\", paras, setMap, reqMap, request, response, false, null, 0, \"\");");
                                    SBA(sb, 4, "setMap.clear();");
                                    SBA(sb, 4, "setMap = null;");
                                    SBA(sb, 3, "}");
                                    #endregion

                                }
                            }
                        }
                        else if (controlname == "dataop")
                        {
                            string ApiDefine = getPartParamValue(doc, "APISet");
                            string defaultfid = getPartParamValue(doc, "DefaultFID");
                            string DefineStr = getPartParamValue(doc, "Define");
                            string opid = getPartParamValue(doc, "OPID");
                            string jssuc = getPartParamValue(doc, "JSSuccess");
                            string codebefore = getPartParamValue(doc, "CodeBefore");
                            string codeafter = getPartParamValue(doc, "CodeAfter");
                            string cdnsql = getPartParamValue(doc, "OPContidionSql");
                            string cdncode = getPartParamValue(doc, "OPContidionCode");
                            string cdnjs = getPartParamValue(doc, "OPContidionJs");
                            string execsqlbefore = getPartParamValue(doc, "BeforeSql");
                            string execsqlafter = getPartParamValue(doc, "AfterSql");
                            string FidCol = getPartParamValue(doc, "FidCol");
                            int OpDefaultCol = int.Parse(getPartParamValue(doc, "OpDefaultCol"));
                            string CustomConnection = getPartParamValue(doc, "CustomConnection");
                            Dictionary<string, string> JsonSet = new Dictionary<string, string>();
                            string[] rows = DefineStr.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < rows.Length; i++)
                            {
                                string[] item = rows[i].Split(new string[] { "##" }, StringSplitOptions.None);
                                string name = item[1].Trim();
                                string json = null;
                                if (item.Length >= 9) json = item[8].Trim();
                                if (!string.IsNullOrEmpty(json) && json != "[]")
                                {
                                    if (!JsonSet.ContainsKey(name)) JsonSet.Add(name, json.Replace("$#$#$", "##"));
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(ApiDefine))
                            {
                                string[] items = ApiDefine.Split(new string[] { "{$$}" }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string item in items)
                                {
                                    string[] colcfg = item.Split(new string[] { "[##]" }, StringSplitOptions.None);
                                    string InputType = "form";
                                    if (colcfg.Length > 5) InputType = colcfg[5];
                                    string ApiName = colcfg[0].Trim();
                                    string ApiPath = ApiName.StartsWith("/") ? ApiName : (subPath + "?" + ApiName);
                                    ApiPath = Api.ApiPathReplace(ApiPath);
                                    Dictionary<string, string> Set_Dic = new Dictionary<string, string>();
                                    Set_Dic.Add("partid", partid);
                                    Set_Dic.Add("ApiDefine", item.NoWrap());
                                    Set_Dic.Add("defaultfid", defaultfid);
                                    Set_Dic.Add("SiteID", "ftdp");
                                    Set_Dic.Add("InputType", InputType);
                                    Set_Dic.Add("DefineStr", DefineStr.NoWrap());
                                    Set_Dic.Add("opid", opid);
                                    Set_Dic.Add("jssuc", jssuc.NoWrap());
                                    Set_Dic.Add("codebefore", codebefore);
                                    Set_Dic.Add("codeafter", codeafter);
                                    Set_Dic.Add("cdnsql", cdnsql.NoWrap());
                                    Set_Dic.Add("cdncode", cdncode);
                                    Set_Dic.Add("cdnjs", cdnjs.NoWrap());
                                    Set_Dic.Add("execsqlbefore", execsqlbefore.NoWrap());
                                    Set_Dic.Add("execsqlafter", execsqlafter.NoWrap());
                                    Set_Dic.Add("FidCol", FidCol);
                                    Set_Dic.Add("OpDefaultCol", OpDefaultCol.ToString());
                                    Set_Dic.Add("CustomConnection", CustomConnection.NoWrap());
                                    #region 生成java代码
                                    SBA(sb, 2, "} else if (key.equals(\"" + ApiName + "\")) {");
                                    SBA(sb, 3, "if (!ProApi.Auth(\"DataOP\", \"" + subPath + "\", paras, request, response,reqMap))");
                                    SBA(sb, 4, "json = ProApi.AuthFailedJson(\"DataOP\", \"" + subPath + "\", paras, request, response,reqMap);");
                                    SBA(sb, 3, "else {");
                                    SBA(sb, 4, "HashMap<String, Object> setMap = new HashMap<String, Object>() {");
                                    SBA(sb, 5, "{");
                                    SBA(sb, 6, "put(\"partid\", \"" + Set_Dic["partid"] + "\");");
                                    SBA(sb, 6, "put(\"SiteID\", \"ftdp\");");
                                    SBA(sb, 6, "put(\"defaultfid\", \"" + Set_Dic["defaultfid"] + "\");");
                                    SBA(sb, 6, "put(\"InputType\", \"" + Set_Dic["InputType"] + "\");");
                                    SBA(sb, 6, "put(\"DefineStr\",");
                                    SBA(sb, 7, "new String[][]{");
                                    string[] rowsDefine = Set_Dic["DefineStr"].Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (string rowstr in rowsDefine)
                                    {
                                        SBA(sb, 8, "{");
                                        string[] rowcols = rowstr.Split(new string[] { "##" }, StringSplitOptions.None);
                                        string caption = rowcols[0].Trim();
                                        string name = rowcols[1].Trim();
                                        string tabletag = str.GetDecode(rowcols[2].Trim());
                                        string optype = str.GetDecode(rowcols[3].Trim());
                                        string jiaoyan = rowcols[4].Trim();
                                        string fid = rowcols[5].Trim();
                                        string idValue = rowcols[6].Trim();
                                        string advset = str.GetDecode(rowcols[7].Trim());
                                        string json = (rowcols.Length >= 9) ? rowcols[8].Trim() : "";
                                        SBA(sb, 9, "\"" + caption.NoDotNoWrap() + "\", \"" + name.NoDotNoWrap() + "\", \"" + tabletag.NoDotNoWrap() + "\", \"" + optype + "\", \"" + jiaoyan.NoWrap() + "\", \"" + fid.NoWrap() + "\", \"" + idValue.NoDotNoWrap() + "\", \"" + advset.NoWrap() + "\", \"" + json.NoWrap() + "\"");
                                        SBA(sb, 8, "},");
                                    }
                                    SBA(sb, 7, "}");
                                    SBA(sb, 6, ");");
                                    SBA(sb, 6, "put(\"ApiDefine\",");
                                    SBA(sb, 7, "new Object[]{");
                                    string ApiDesc = colcfg[1].Trim();
                                    SBA(sb, 8, "\"" + ApiName + "\", \"" + ApiDesc.NoDotNoWrap() + "\",");
                                    SBA(sb, 8, "new String[][]{");
                                    string[] apiKeys = colcfg[2].Split(new string[] { "[#]" }, StringSplitOptions.None);
                                    string[] colKeys = colcfg[3].Split(new string[] { "[#]" }, StringSplitOptions.None);
                                    string[] colDesc = colcfg[4].Split(new string[] { "[#]" }, StringSplitOptions.None);
                                    for (var i = 0; i < apiKeys.Length; i++)
                                    {
                                        SBA(sb, 9, "{");
                                        SBA(sb, 10, "\"" + apiKeys[i].NoDotNoWrap() + "\", \"" + colKeys[i].NoDotNoWrap() + "\", \"" + colDesc[i].NoDotNoWrap() + "\"");
                                        SBA(sb, 9, "},");
                                    }
                                    SBA(sb, 8, "}");
                                    SBA(sb, 7, "}");
                                    SBA(sb, 6, ");");
                                    SBA(sb, 6, "put(\"opid\", \"" + Set_Dic["opid"] + "\");");
                                    SBA(sb, 6, "put(\"jssuc\", \"" + Set_Dic["jssuc"] + "\");");
                                    SBA(sb, 6, "put(\"codebefore\", \"" + Set_Dic["codebefore"] + "\");");
                                    SBA(sb, 6, "put(\"codeafter\", \"" + Set_Dic["codeafter"] + "\");");
                                    SBA(sb, 6, "put(\"cdnsql\", \"" + Set_Dic["cdnsql"] + "\");");
                                    SBA(sb, 6, "put(\"cdncode\", \"" + Set_Dic["cdncode"] + "\");");
                                    SBA(sb, 6, "put(\"cdnjs\", \"" + Set_Dic["cdnjs"] + "\");");
                                    SBA(sb, 6, "put(\"execsqlbefore\", \"" + Set_Dic["execsqlbefore"] + "\");");
                                    SBA(sb, 6, "put(\"execsqlafter\", \"" + Set_Dic["execsqlafter"] + "\");");
                                    SBA(sb, 6, "put(\"FidCol\", \"" + Set_Dic["FidCol"] + "\");");
                                    SBA(sb, 6, "put(\"OpDefaultCol\", \"" + Set_Dic["OpDefaultCol"] + "\");");
                                    SBA(sb, 6, "put(\"CustomConnection\", \"" + Set_Dic["CustomConnection"] + "\");");
                                    SBA(sb, 5, "}");
                                    SBA(sb, 4, "};");
                                    SBA(sb, 4, "json = Api.Json(\"DataOP\", paras, setMap, reqMap, request, response, false, null, 0, \"\");");
                                    SBA(sb, 4, "setMap.clear();");
                                    SBA(sb, 4, "setMap = null;");
                                    SBA(sb, 3, "}");
                                    #endregion
                                    Set_Dic.Clear();
                                }
                            }
                        }
                    }
                }
                command.Dispose();
            }
            SBA(sb, 2, "} else {");
            SBA(sb, 3, "json = \"{\\\"code\\\":203,\\\"msg\\\":\\\"wrong key\\\"}\";");
            SBA(sb, 2, "}");
            SBA(sb, 2, "return json;");
            SBA(sb, 1, "}");
            sb.AppendLine("}");
            #endregion
            if (File.Exists(pubPath + Path.DirectorySeparatorChar + filename + ".java"))
            {
                File.Delete(pubPath + Path.DirectorySeparatorChar + filename + ".java");
            }
            using (StreamWriter sr = new StreamWriter(pubPath + Path.DirectorySeparatorChar + filename + ".java", false, new System.Text.UTF8Encoding(false)))
            {
                sr.Write(sb.ToString());
            }
            if (!string.IsNullOrWhiteSpace(MAVEN_HOME))
            {
                new Task(() => ApiPagePublish_Java_Server_Do()).Start();
            }
        }
        private static bool ApiPagePublish_Java_Server_Do_Runing = false;
        private static void ApiPagePublish_Java_Server_Do()
        {
            try
            {
                if (ApiPagePublish_Java_Server_Do_Runing)
                {
                    log.Debug("ApiPagePublish_Java_Server_Do_Runing");
                    return;
                }
                ApiPagePublish_Java_Server_Do_Runing = true;
                string port = JavaPort;
                string javaSitePath = JavaRootPath;
                javaSitePath = Path.GetFullPath(javaSitePath);
                string javaJarName = JavaJarName;
                string javaJarFile = javaSitePath + "/project/target/" + javaJarName;
                

                var cmdMaven = "export JAVA_HOME=\""+JAVA_HOME+ "\";cd "+ javaSitePath + "/project;" + MAVEN_HOME + "/bin/mvn clean package";
                log.Debug(cmdMaven, "Maven");
                func.ExecuteCommandBash(cmdMaven);

                //string netstatStr= func.ExecuteCommandFile("sh", "netstat.sh :::"+ port, javaSitePath+"/shell", 60000);
                string netstatStr = func.ExecuteCommandBash("netstat -tunlp | grep :::" + port);
                log.Debug(netstatStr, "NetStat");
                int findPortIndex = netstatStr.IndexOf(":::" + port);
                int PID = 0;
                if (findPortIndex > 0)
                {
                    string[] lines = netstatStr.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < lines.Length && PID == 0; i++)
                    {
                        string[] items = lines[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        bool ok1 = false;
                        for (int j = 0; j < items.Length; j++)
                        {
                            if (items[j].EndsWith(":::" + port)) ok1 = true;
                            if (ok1 && items[j].EndsWith("/java"))
                            {
                                PID = int.Parse(items[j].Split('/')[0]);
                                break;
                            }
                        }
                    }
                }
                if (PID > 0)
                {
                    log.Debug("kill -9 " + PID, "Kill PID");
                    func.ExecuteCommandFile("kill", "-9 " + PID, javaSitePath, 60000);
                }
                else
                {
                    log.Debug("No Pid Find", "Kill PID");
                }


                log.Debug("nohup java -jar " + javaJarFile + " --server.port=" + port);
                //func.ExecuteCommandFile("sh", "java.sh " + javaJarFile + " " + port, javaSitePath + "/shell", 60000);
                //执行nohup 会暂时停在这里
                ApiPagePublish_Java_Server_Do_Runing = false;
                log.Debug(ApiPagePublish_Java_Server_Do_Runing.ToString(), "Server_Do_Runing");
                var javaCmdReturn=func.ExecuteCommandBash("nohup java -jar " + javaJarFile + "  --server.port=" + port + "  &",5000);
                log.Debug(javaCmdReturn, "JavaCmdReturn");

                netstatStr = func.ExecuteCommandBash("netstat -tunlp | grep :::" + port);
                log.Debug(netstatStr, "NetStat");

                ApiPagePublish_Java_Server_Do_Runing = false;
                log.Debug(ApiPagePublish_Java_Server_Do_Runing.ToString(), "Server_Do_Runing");

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                ApiPagePublish_Java_Server_Do_Runing = false;
            }
        }
        private static void ApiDocSon(DB db, string ApiPath, StringBuilder sb, int PassLeft)
        {
            string apisql = "select * from ft_ftdp_apiset where ApiPath='" + str.D2DD(ApiPath) + "'";
            string ApiType = "";
            string RowAll = null, ApiDefine = null;
            using (DR dr = db.OpenRecord(apisql))
            {
                if (dr.Read())
                {
                    ApiType = dr.GetString("ApiType");
                    RowAll = dr.GetStringNoNULL("Set_List_RowAll");
                    ApiDefine = dr.GetStringNoNULL("Set_DyValue_ApiDefine");
                }
            }
            if (ApiType == "List")
            {
                sb.Append(HtmlDiv(PassLeft + 2, "{\"list\": ["));
                sb.Append(HtmlDiv(PassLeft + 3, "{"));
                string[] rows = RowAll.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                sb.Append(HtmlDiv(PassLeft + 4, "\"fid\": \"referenced primary key value\","));
                foreach (string rowstr in rows)
                {
                    string row = rowstr.Substring(0, rowstr.IndexOf("&&&")).Trim();
                    string[] rowcols = row.Split('#');
                    if (rowcols[7] == "0")//该列显示
                    {
                        string colname = rowcols[0];
                        string coldesc = rowcols[5];
                        string rowdata = str.GetDecode(rowcols[1]);
                        if (!rowdata.StartsWith("@api_"))
                        {
                            if (colname.ToLower() != "fid") sb.Append(HtmlDiv(PassLeft + 4, "\"" + colname + "\": \"" + coldesc + "\","));
                        }
                        else
                        {
                            string SonApiPath = rowdata.Substring(5);
                            int Sonindex = SonApiPath.IndexOf("?");
                            if (Sonindex > 0)
                            {
                                string[] Sonnewparas = SonApiPath.Substring(Sonindex + 1).Split('/');
                                string Sonkey = Sonnewparas[0];
                                SonApiPath = SonApiPath.Substring(0, Sonindex) + "?" + Sonkey;
                                sb.Append(HtmlDiv(PassLeft + 4, "\"" + colname + "\": /*" + coldesc + "*/"));
                                ApiDocSon(db, SonApiPath, sb, PassLeft + 4);
                                sb.Append(HtmlDiv(PassLeft + 4, ","));
                            }
                        }
                    }
                }
                sb.Append(HtmlDiv(PassLeft + 3, " } ],"));
                sb.Append(HtmlDiv(PassLeft + 3, "\"cacu\": \"Calculate Row\","));
                sb.Append(HtmlDiv(PassLeft + 3, "\"page\": {"));
                sb.Append(HtmlDiv(PassLeft + 4, "\"count\": Total,"));
                sb.Append(HtmlDiv(PassLeft + 4, "\"pageSize\": Items Per Page,"));
                sb.Append(HtmlDiv(PassLeft + 4, "\"pageNum\": Current Page,"));
                sb.Append(HtmlDiv(PassLeft + 4, "\"pageCount\": Page Count"));
                sb.Append(HtmlDiv(PassLeft + 2, "}"));
                sb.Append(HtmlDiv(PassLeft + 2, "}"));
            }
            else if (ApiType == "DyValue")
            {
                string[] apiItem = ApiDefine.Split(new string[] { "[##]" }, StringSplitOptions.None);
                string[] apiKeys = apiItem[2].Split(new string[] { "[#]" }, StringSplitOptions.None);
                string[] shuomings = apiItem[4].Split(new string[] { "[#]" }, StringSplitOptions.None);
                string[] linkKeys = apiItem[5].Split(new string[] { "[#]" }, StringSplitOptions.None);
                sb.Append(HtmlDiv(PassLeft + 3, "{\"detail\": {"));
                for (int i = 0; i < apiKeys.Length; i++)
                {
                    if (apiKeys[i].IndexOf(',') >= 0)//动态新增或维表
                    {
                        string[] ItemKeys = apiKeys[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        string[] ItemDescs = shuomings[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        sb.Append(HtmlDiv(PassLeft + 4, "\"" + apiKeys[i].Substring(0, apiKeys[i].IndexOf(',')) + "\": /*" + (ItemDescs.Length > 0 ? ItemDescs[0] : "") + "*/[{"));
                        for (int _i = 1; _i < ItemKeys.Length; _i++)
                        {
                            sb.Append(HtmlDiv(PassLeft + 6, "\"" + ItemKeys[_i] + "\":\"" + (ItemDescs.Length > _i ? ItemDescs[_i] : "") + "\","));
                        }
                        sb.Append(HtmlDiv(PassLeft + 4, "}],"));
                    }
                    else if (!linkKeys[i].StartsWith("@api_"))
                    {
                        sb.Append(HtmlDiv(PassLeft + 4, "\"" + apiKeys[i] + "\": \"" + shuomings[i] + "\","));
                    }
                    else
                    {
                        string SonApiPath = linkKeys[i].Substring(5);
                        int Sonindex = SonApiPath.IndexOf("?");
                        if (Sonindex > 0)
                        {
                            string[] Sonnewparas = SonApiPath.Substring(Sonindex + 1).Split('/');
                            string Sonkey = Sonnewparas[0];
                            SonApiPath = SonApiPath.Substring(0, Sonindex) + "?" + Sonkey;
                            sb.Append(HtmlDiv(PassLeft + 4, "\"" + apiKeys[i] + "\": /*" + shuomings[i] + "*/"));
                            ApiDocSon(db, SonApiPath, sb, PassLeft + 4);
                            sb.Append(HtmlDiv(PassLeft + 4, ","));
                        }
                    }
                }
                sb.Append(HtmlDiv(PassLeft + 3, "}}"));
            }
        }
        private static void ApiDoc(DB db, string ApiType, string ApiPath, string FidCol, string Caption, string[] ApiItem, string RowSet, string DyValueApiSet, string DataOPApiSet, string DevUser, string pageid, Dictionary<string, string> JsonSet = null)
        {
            if (ApiType == "List")
            {
                string Mimo = ApiItem[1];
                bool JsonInput = ApiItem.Length > 5 && ApiItem[5] == "json";
                string sql = "select Fid from ft_ftdp_apidoc where ApiPath='" + str.D2DD(ApiPath) + "'";
                string Fid = db.GetString(sql) ?? str.GetCombID();
                StringBuilder sb = new StringBuilder();
                StringBuilder sbP = new StringBuilder();
                if (JsonInput) sb.Append(HtmlDiv(0, "{"));
                else sb.Append("<table width='100%' border=0 cellspacing=0><tr><th>Key</th><th>Reference Value</th><th>Note</th></tr>");
                sbP.Append("{\"info\": {\"_postman_id\": \"" + Fid + "\",\"name\": \"" + Mimo.Replace("\"", "") + "\",\"schema\": \"https://schema.getpostman.com/json/collection/v2.1.0/collection.json\"},");
                sbP.Append("\"item\": [{\"name\": \"Data List\",\"id\": \"" + str.GetCombID() + "\",\"protocolProfileBehavior\": {\"disabledSystemHeaders\": {\"user-agent\": true}},\"request\": {\"method\": \"POST\",\"header\": [{\"key\": \"User-Agent\",\"value\": \"FTDP\",\"type\": \"text\"},{\"key\":\"token\",\"value\":\"ftdp\",\"type\":\"text\"},{\"key\":\"Authorization\",\"value\":\"ftdp\",\"type\":\"text\"}],\"body\": ");
                if (JsonInput) sbP.Append("{\"mode\": \"raw\",\"raw\": \"{\\r\\n ");
                else sbP.Append("{\"mode\": \"formdata\",\"formdata\": [");
                string[] keys = ApiItem[2].Split(new string[] { "," }, StringSplitOptions.None);
                string[] cankaos = ApiItem[3].Split(new string[] { "[#]" }, StringSplitOptions.None);
                string[] shuomings = ApiItem[4].Split(new string[] { "[#]" }, StringSplitOptions.None);
                int StatCount = 0;
                for (int i = 0; i < keys.Length; i++)
                {
                    if (i > 0 && !JsonInput) sbP.Append(",");
                    if (keys[i] == "orderBy")
                    {
                        if (JsonInput) sb.Append(HtmlDiv(1, "\"orderBy\":\"" + cankaos[i] + "\", " + shuomings[i]));
                        else sb.Append("<tr><td>orderBy</td><td>" + cankaos[i] + "</td><td>" + shuomings[i] + "</td></tr>");
                        if (JsonInput) sbP.Append(" \\\"orderBy\\\": \\\"" + cankaos[i] + "\\\",/* " + shuomings[i].Replace("\"", "") + " */\\r\\n ");
                        else sbP.Append("{\"key\": \"" + keys[i] + "\",\"value\": \"" + cankaos[i] + "\",\"description\": \"" + shuomings[i].Replace("\"", "") + "\",\"type\": \"text\"}");
                    }
                    else if (keys[i] == "orderType")
                    {
                        if (JsonInput) sb.Append(HtmlDiv(1, "\"orderType\":\"" + cankaos[i] + "\", " + shuomings[i]));
                        else sb.Append("<tr><td>orderType</td><td>" + cankaos[i] + "</td><td>" + shuomings[i] + "</td></tr>");
                        if (JsonInput) sbP.Append(" \\\"orderType\\\": \\\"" + cankaos[i] + "\\\",/* " + shuomings[i].Replace("\"", "") + " */\\r\\n ");
                        else sbP.Append("{\"key\": \"" + keys[i] + "\",\"value\": \"" + cankaos[i] + "\",\"description\": \"" + shuomings[i].Replace("\"", "") + "\",\"type\": \"text\"}");
                    }
                    else if (keys[i] == "schText")
                    {
                        if (JsonInput) sb.Append(HtmlDiv(1, "\"schText\":\"" + cankaos[i] + "\", " + shuomings[i]));
                        else sb.Append("<tr><td>schText</td><td>" + cankaos[i] + "</td><td>" + shuomings[i] + "</td></tr>");
                        if (JsonInput) sbP.Append(" \\\"schText\\\": \\\"" + cankaos[i] + "\\\",/* " + shuomings[i].Replace("\"", "") + " */\\r\\n ");
                        else sbP.Append("{\"key\": \"" + keys[i] + "\",\"value\": \"" + cankaos[i] + "\",\"description\": \"" + shuomings[i].Replace("\"", "") + "\",\"type\": \"text\"}");
                    }
                    else if (keys[i] == "schStrict")
                    {
                        if (JsonInput) sb.Append(HtmlDiv(1, "\"schStrict\":\"" + cankaos[i] + "\", " + shuomings[i]));
                        else sb.Append("<tr><td>schStrict</td><td>" + cankaos[i] + "</td><td>" + shuomings[i] + "</td></tr>");
                        if (JsonInput) sbP.Append(" \\\"schStrict\\\": \\\"" + cankaos[i] + "\\\",/* " + shuomings[i].Replace("\"", "") + " */\\r\\n ");
                        else sbP.Append("{\"key\": \"" + keys[i] + "\",\"value\": \"" + cankaos[i] + "\",\"description\": \"" + shuomings[i].Replace("\"", "") + "\",\"type\": \"text\"}");
                    }
                    else if (keys[i] == "schAdv")
                    {
                        if (JsonInput) sb.Append(HtmlDiv(1, "\"schAdv\":\"" + cankaos[i] + "\", " + shuomings[i]));
                        else sb.Append("<tr><td>schAdv</td><td>" + cankaos[i] + "</td><td>" + shuomings[i] + "</td></tr>");
                        if (JsonInput) sbP.Append(" \\\"schAdv\\\": \\\"" + cankaos[i] + "\\\",/* " + shuomings[i].Replace("\"", "") + " */\\r\\n ");
                        else sbP.Append("{\"key\": \"" + keys[i] + "\",\"value\": \"" + cankaos[i] + "\",\"description\": \"" + shuomings[i].Replace("\"", "") + "\",\"type\": \"text\"}");
                    }
                    else if (keys[i] == "pageSize")
                    {
                        if (JsonInput) sb.Append(HtmlDiv(1, "\"pageSize\":\"" + cankaos[i] + "\", " + shuomings[i]));
                        else sb.Append("<tr><td>pageSize</td><td>" + cankaos[i] + "</td><td>" + shuomings[i] + "</td></tr>");
                        if (JsonInput) sbP.Append(" \\\"pageSize\\\": \\\"" + cankaos[i] + "\\\",/* " + shuomings[i].Replace("\"", "") + " */\\r\\n ");
                        else sbP.Append("{\"key\": \"" + keys[i] + "\",\"value\": \"" + cankaos[i] + "\",\"description\": \"" + shuomings[i].Replace("\"", "") + "\",\"type\": \"text\"}");
                    }
                    else if (keys[i] == "numType")
                    {
                        if (JsonInput) sb.Append(HtmlDiv(1, "\"numType\":\"" + cankaos[i] + "\", " + shuomings[i]));
                        else sb.Append("<tr><td>numType</td><td>" + cankaos[i] + "</td><td>" + shuomings[i] + "</td></tr>");
                        if (JsonInput) sbP.Append(" \\\"numType\\\": \\\"" + cankaos[i] + "\\\",/* " + shuomings[i].Replace("\"", "") + " */\\r\\n ");
                        else sbP.Append("{\"key\": \"" + keys[i] + "\",\"value\": \"" + cankaos[i] + "\",\"description\": \"" + shuomings[i].Replace("\"", "") + "\",\"type\": \"text\"}");
                    }
                    else if (keys[i] == "pageNum")
                    {
                        if (JsonInput) sb.Append(HtmlDiv(1, "\"pageNum\":\"" + cankaos[i] + "\", " + shuomings[i]));
                        else sb.Append("<tr><td>pageNum</td><td>" + cankaos[i] + "</td><td>" + shuomings[i] + "</td></tr>");
                        if (JsonInput) sbP.Append(" \\\"pageNum\\\": \\\"" + cankaos[i] + "\\\",/* " + shuomings[i].Replace("\"", "") + " */\\r\\n ");
                        else sbP.Append("{\"key\": \"" + keys[i] + "\",\"value\": \"" + cankaos[i] + "\",\"description\": \"" + shuomings[i].Replace("\"", "") + "\",\"type\": \"text\"}");
                    }
                    else if (keys[i].StartsWith("keyValue"))
                    {
                        if (JsonInput) sb.Append(HtmlDiv(1, "\"keyValue\":<font style='color:red'>\"" + keys[i].Substring(keys[i].IndexOf('.') + 1) + "\"</font>, " + shuomings[i]));
                        else sb.Append("<tr><td>keyValue</td><td style='color:red'>" + keys[i].Substring(keys[i].IndexOf('.') + 1) + "</td><td>" + shuomings[i] + "</td></tr>");
                        if (JsonInput) sbP.Append(" \\\"keyValue\\\": \\\"\\\",/* " + shuomings[i].Replace("\"", "") + " */\\r\\n ");
                        else sbP.Append("{\"key\": \"keyValue\",\"value\": \"\",\"description\": \"" + shuomings[i].Replace("\"", "") + "\",\"type\": \"text\"}");
                    }
                    else
                    {
                        string[] statItem = keys[i].Split('.');
                        if (StatCount == 0)
                        {
                            if (JsonInput) sb.Append(HtmlDiv(1, "\"" + statItem[0] + "\":<font style='color:red'>\"" + statItem[1] + "\"</font>, " + shuomings[i]));
                            else sb.Append("<tr><td>" + statItem[0] + "</td><td style='color:red'>" + statItem[1] + "</td><td>" + shuomings[i] + "</td></tr>");
                            if (JsonInput) sbP.Append(" \\\"" + statItem[0] + "\\\": \\\"\\\",/* " + shuomings[i].Replace("\"", "") + " */\\r\\n ");
                            else sbP.Append("{\"key\": \"" + statItem[0] + "\",\"value\": \"\",\"description\": \"" + shuomings[i].Replace("\"", "") + "\",\"type\": \"text\"}");
                        }
                        else if (StatCount == 1)
                        {
                            if (JsonInput) sb.Append(HtmlDiv(1, "\"" + statItem[0] + "\":<font style='color:red'>\"" + statItem[1] + "\"</font>, " + shuomings[i]));
                            else sb.Append("<tr><td>" + statItem[0] + "</td><td style='color:red'>" + statItem[1] + "</td><td>" + shuomings[i] + "</td></tr>");
                            if (JsonInput) sbP.Append(" \\\"" + statItem[0] + "\\\": \\\"\\\",/* " + shuomings[i].Replace("\"", "") + " */\\r\\n ");
                            else sbP.Append("{\"key\": \"" + statItem[0] + "\",\"value\": \"\",\"description\": \"" + shuomings[i].Replace("\"", "") + "\",\"type\": \"text\"}");
                        }
                        else if (StatCount == 2)
                        {
                            if (JsonInput) sb.Append(HtmlDiv(1, "\"" + statItem[0] + "\":<font style='color:red'>\"" + statItem[1] + "\"</font>, " + shuomings[i]));
                            else sb.Append("<tr><td>" + statItem[0] + "</td><td style='color:red'>" + statItem[1] + "</td><td>" + shuomings[i] + "</td></tr>");
                            if (JsonInput) sbP.Append(" \\\"" + statItem[0] + "\\\": \\\"\\\",/* " + shuomings[i].Replace("\"", "") + " */\\r\\n ");
                            else sbP.Append("{\"key\": \"" + statItem[0] + "\",\"value\": \"\",\"description\": \"" + shuomings[i].Replace("\"", "") + "\",\"type\": \"text\"}");
                        }
                        StatCount++;
                    }
                }
                if (JsonInput) sb.Append(HtmlDiv(0, "}"));
                else sb.Append("</table>");
                if (JsonInput) sbP.Append("}\", \"options\": { \"raw\": { \"language\": \"json\"} } }, ");
                else sbP.Append("]},");
                sbP.Append("\"url\": {");
                sbP.Append("\"raw\": \"[[[Proto]]]://[[[Host]]]" + ApiPath + "\",");///yanshi/10/?GetList_add
                string query = ApiPath.Contains('?') ? ApiPath.Split('?')[1] : "";
                string[] pathitem = ApiPath.Split('?')[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                sbP.Append("\"protocol\": \"[[[Proto]]]\",");
                sbP.Append("\"host\": [\"[[[Host]]]\"],");
                sbP.Append("\"path\": [");
                for (int i = 0; i < pathitem.Length; i++)
                {
                    if (i > 0) sbP.Append(",");
                    sbP.Append("\"" + pathitem[i] + "\"");
                }
                //sbP.Append("\"\"");
                sbP.Append("]");
                if (!string.IsNullOrEmpty(query))
                {

                    sbP.Append(",\"query\": [{");
                    sbP.Append("\"key\": \"" + query + "\",");
                    sbP.Append("\"value\": null");
                    sbP.Append("}]");
                }
                sbP.Append("}},\"response\": []}],\"protocolProfileBehavior\": {}}");
                StringBuilder sbDesc = new StringBuilder();
                string InputDesc = sb.ToString();
                string PostManJson = sbP.ToString();
                sb = new StringBuilder();
                if (true || StatCount == 0)
                {
                    sb.Append(HtmlDiv(0, "{"));
                    sb.Append(HtmlDiv(1, "\"code\": 200,"));
                    sb.Append(HtmlDiv(1, "\"" + Project.Core.Api.MessageStr + "\":\"success\","));
                    sb.Append(HtmlDiv(1, "\"data\": {"));
                    sb.Append(HtmlDiv(2, "\"list\": ["));
                    sb.Append(HtmlDiv(3, "{"));
                    string[] rows = RowSet.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                    sb.Append(HtmlDiv(4, "\"" + FidCol + "\": \"Primary Key Value\","));
                    sbDesc.Append(FidCol + "{::}" + "Primary Key Value" + "{;;}");
                    foreach (string rowstr in rows)
                    {
                        string row = rowstr.Substring(0, rowstr.IndexOf("&&&")).Trim();
                        string[] rowcols = row.Split('#');
                        if (rowcols[7] == "0")//该列显示
                        {
                            string colname = rowcols[0];
                            string coldesc = rowcols[5];
                            string rowdata = str.GetDecode(rowcols[1]);
                            if (!rowdata.StartsWith("@api_"))
                            {
                                if (colname.ToLower() != FidCol) sb.Append(HtmlDiv(4, "\"" + colname + "\": \"" + coldesc + "\","));
                                sbDesc.Append(colname + "{::}" + coldesc + "{;;}");
                            }
                            else
                            {
                                string SonApiPath = rowdata.Substring(5);
                                int Sonindex = SonApiPath.IndexOf("?");
                                if (Sonindex > 0)
                                {
                                    string[] Sonnewparas = SonApiPath.Substring(Sonindex + 1).Split('/');
                                    string Sonkey = Sonnewparas[0];
                                    SonApiPath = SonApiPath.Substring(0, Sonindex) + "?" + Sonkey;
                                    sb.Append(HtmlDiv(4, "\"" + colname + "\": /*" + coldesc + "*/"));
                                    ApiDocSon(db, SonApiPath, sb, 4);
                                    sb.Append(HtmlDiv(4, ","));
                                }
                            }
                        }
                    }
                    sb.Append(HtmlDiv(3, " } ],"));
                    sb.Append(HtmlDiv(3, "\"cacu\": \"Calculate Row\","));
                    sb.Append(HtmlDiv(3, "\"page\": {"));
                    sb.Append(HtmlDiv(4, "\"count\": Total,"));
                    sb.Append(HtmlDiv(4, "\"pageSize\": Items Per Page,"));
                    sb.Append(HtmlDiv(4, "\"pageNum\": Current Page,"));
                    sb.Append(HtmlDiv(4, "\"pageCount\": Page Count"));
                    sb.Append(HtmlDiv(2, "}"));
                    sb.Append(HtmlDiv(1, "}"));
                    sb.Append(HtmlDiv(0, "}"));
                }
                else
                {
                    sb.Append(HtmlDiv(0, "{"));
                    sb.Append(HtmlDiv(1, "\"code\": 200,"));
                    sb.Append(HtmlDiv(1, "\"" + Project.Core.Api.MessageStr + "\":\"success\","));
                    sb.Append(HtmlDiv(1, "\"data\": {"));
                    sb.Append(HtmlDiv(2, "\"rowsAffected\":1"));
                    sb.Append(HtmlDiv(1, "}"));
                    sb.Append(HtmlDiv(0, "}"));
                }
                string OutDesc = sb.ToString();
                sql = "delete from ft_ftdp_apidoc where ApiPath='" + str.D2DD(ApiPath) + "'";
                db.ExecSql(sql);
                sql = "insert into ft_ftdp_apidoc(Fid,ApiPath,PageID,PageCaption,DevUser,ApiType,Mimo,InputDesc,OutputDesc,ModTime,PostManJson,KeyDesc)";
                sql += "values(@Fid,@ApiPath,@PageID,@PageCaption,@DevUser,@ApiType,@Mimo,@InputDesc,@OutputDesc,@ModTime,@PostManJson,@KeyDesc)";
                db.ExecSql(sql, new PR[] {
                                    new PR("@Fid",Fid),
                                    new PR("@ApiPath",ApiPath),
                                    new PR("@PageID",pageid),
                                    new PR("@PageCaption",Caption),
                                    new PR("@DevUser",DevUser),
                                    new PR("@ApiType","List"),
                                    new PR("@Mimo",Mimo),
                                    new PR("@InputDesc",InputDesc),
                                    new PR("@OutputDesc",OutDesc),
                                    new PR("@ModTime",DateTime.Now),
                                    new PR("@PostManJson",PostManJson),
                                    new PR("@KeyDesc",sbDesc.ToString()),
                                    });
            }
            else if (ApiType == "DyValue")
            {
                string Mimo = ApiItem[1];
                string sql = "select Fid from ft_ftdp_apidoc where ApiPath='" + str.D2DD(ApiPath) + "'";
                string Fid = db.GetString(sql) ?? str.GetCombID();
                StringBuilder sb = new StringBuilder();
                StringBuilder sbP = new StringBuilder();
                sb.Append("<table width='100%' border=0 cellspacing=0><tr><td>Data Getting,after the Api address, parameters are passed through slashes, corresponding to the parameter sequence in the settings</td></tr>");
                sbP.Append("{\"info\": {\"_postman_id\": \"" + Fid + "\",\"name\": \"" + Mimo.Replace("\"", "") + "\",\"schema\": \"https://schema.getpostman.com/json/collection/v2.1.0/collection.json\"},");
                sbP.Append("\"item\": [{\"name\": \"Data Getting\",\"id\": \"" + str.GetCombID() + "\",\"protocolProfileBehavior\": {\"disabledSystemHeaders\": {\"user-agent\": true}},\"request\": {\"method\": \"GET\",\"header\": [{\"key\": \"User-Agent\",\"value\": \"FTDP\",\"type\": \"text\"},{\"key\":\"token\",\"value\":\"ftdp\",\"type\":\"text\"},{\"key\":\"Authorization\",\"value\":\"ftdp\",\"type\":\"text\"}],\"body\": {\"mode\": \"formdata\",\"formdata\": [");
                string[] keys = ApiItem[2].Split(new string[] { "[#]" }, StringSplitOptions.None);
                string[] shuomings = ApiItem[4].Split(new string[] { "[#]" }, StringSplitOptions.None);
                string[] linkKeys = ApiItem[5].Split(new string[] { "[#]" }, StringSplitOptions.None);

                sb.Append("</table>");
                sbP.Append("]},");
                sbP.Append("\"url\": {");
                sbP.Append("\"raw\": \"[[[Proto]]]://[[[Host]]]" + ApiPath + "\",");///yanshi/10/?GetList_add
                string query = ApiPath.Contains('?') ? ApiPath.Split('?')[1] : "";
                string[] pathitem = ApiPath.Split('?')[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                sbP.Append("\"protocol\": \"[[[Proto]]]\",");
                sbP.Append("\"host\": [\"[[[Host]]]\"],");
                sbP.Append("\"path\": [");
                for (var i = 0; i < pathitem.Length; i++)
                {
                    if (i > 0) sbP.Append(",");
                    sbP.Append("\"" + pathitem[i] + "\"");
                }
                sbP.Append("]");
                if (!string.IsNullOrEmpty(query))
                {

                    sbP.Append(",\"query\": [{");
                    sbP.Append("\"key\": \"" + query + "\",");
                    sbP.Append("\"value\": null");
                    sbP.Append("}]");
                }
                sbP.Append("}},\"response\": []}],\"protocolProfileBehavior\": {}}");
                StringBuilder sbDesc = new StringBuilder();
                string InputDesc = sb.ToString();
                string PostManJson = sbP.ToString();
                sb = new StringBuilder();
                sb.Append(HtmlDiv(0, "{"));
                sb.Append(HtmlDiv(1, "\"code\": 200,"));
                sb.Append(HtmlDiv(1, "\"" + Project.Core.Api.MessageStr + "\":\"success\","));
                sb.Append(HtmlDiv(1, "\"data\": {"));
                sb.Append(HtmlDiv(2, "\"detail\": "));
                sb.Append(HtmlDiv(3, "{"));
                for (int i = 0; i < keys.Length; i++)
                {
                    if (keys[i].IndexOf(',') >= 0)//动态新增或维表
                    {
                        string[] ItemKeys = keys[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        string[] ItemDescs = shuomings[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        sb.Append(HtmlDiv(4, "\"" + keys[i].Substring(0, keys[i].IndexOf(',')) + "\": /*" + (ItemDescs.Length > 0 ? ItemDescs[0] : "") + "*/[{"));
                        for (int _i = 1; _i < ItemKeys.Length; _i++)
                        {
                            sb.Append(HtmlDiv(6, "\"" + ItemKeys[_i] + "\":\"" + (ItemDescs.Length > _i ? ItemDescs[_i] : "") + "\","));
                        }
                        sb.Append(HtmlDiv(4, "}],"));
                    }
                    else if (!linkKeys[i].StartsWith("@api_"))
                    {
                        sb.Append(HtmlDiv(4, "\"" + keys[i] + "\": \"" + shuomings[i] + "\","));
                        sbDesc.Append(keys[i] + "{::}" + shuomings[i] + "{;;}");
                    }
                    else
                    {
                        string SonApiPath = linkKeys[i].Substring(5);
                        int Sonindex = SonApiPath.IndexOf("?");
                        if (Sonindex > 0)
                        {
                            string[] Sonnewparas = SonApiPath.Substring(Sonindex + 1).Split('/');
                            string Sonkey = Sonnewparas[0];
                            SonApiPath = SonApiPath.Substring(0, Sonindex) + "?" + Sonkey;
                            sb.Append(HtmlDiv(4, "\"" + keys[i] + "\": /*" + shuomings[i] + "*/"));
                            ApiDocSon(db, SonApiPath, sb, 4);
                            sb.Append(HtmlDiv(4, ","));
                            sbDesc.Append(keys[i] + "{::}" + shuomings[i] + "{;;}");
                        }
                    }
                }
                sb.Append(HtmlDiv(3, " } "));
                sb.Append(HtmlDiv(1, "}"));
                sb.Append(HtmlDiv(0, "}"));
                string OutDesc = sb.ToString();
                sql = "delete from ft_ftdp_apidoc where ApiPath='" + str.D2DD(ApiPath) + "'";
                db.ExecSql(sql);
                sql = "insert into ft_ftdp_apidoc(Fid,ApiPath,PageID,PageCaption,DevUser,ApiType,Mimo,InputDesc,OutputDesc,ModTime,PostManJson,KeyDesc)";
                sql += "values(@Fid,@ApiPath,@PageID,@PageCaption,@DevUser,@ApiType,@Mimo,@InputDesc,@OutputDesc,@ModTime,@PostManJson,@KeyDesc)";
                db.ExecSql(sql, new PR[] {
                                    new PR("@Fid",Fid),
                                    new PR("@ApiPath",ApiPath),
                                    new PR("@PageID",pageid),
                                    new PR("@PageCaption",Caption),
                                    new PR("@DevUser",DevUser),
                                    new PR("@ApiType","DyValue"),
                                    new PR("@Mimo",Mimo),
                                    new PR("@InputDesc",InputDesc),
                                    new PR("@OutputDesc",OutDesc),
                                    new PR("@ModTime",DateTime.Now),
                                    new PR("@PostManJson",PostManJson),
                                    new PR("@KeyDesc",sbDesc.ToString()),
                                    });
            }
            else if (ApiType == "DataOP")
            {
                string Mimo = ApiItem[1];
                bool JsonInput = ApiItem.Length > 5 && ApiItem[5] == "json";
                string sql = "select Fid from ft_ftdp_apidoc where ApiPath='" + str.D2DD(ApiPath) + "'";
                string Fid = db.GetString(sql) ?? str.GetCombID();
                StringBuilder sb = new StringBuilder();
                StringBuilder sbP = new StringBuilder();
                StringBuilder sbDesc = new StringBuilder();
                if (JsonInput) sb.Append(HtmlDiv(0, "{"));
                else sb.Append("<table width='100%' border=0 cellspacing=0><tr><th>Key</th><th>Note</th></tr>");
                sbP.Append("{\"info\": {\"_postman_id\": \"" + Fid + "\",\"name\": \"" + Mimo.Replace("\"", "") + "\",\"schema\": \"https://schema.getpostman.com/json/collection/v2.1.0/collection.json\"},");
                sbP.Append("\"item\": [{\"name\": \"Data Operation\",\"id\": \"" + str.GetCombID() + "\",\"protocolProfileBehavior\": {\"disabledSystemHeaders\": {\"user-agent\": true}},\"request\": {\"method\": \"POST\",\"header\": [{\"key\": \"User-Agent\",\"value\": \"FTDP\",\"type\": \"text\"},{\"key\":\"token\",\"value\":\"ftdp\",\"type\":\"text\"},{\"key\":\"Authorization\",\"value\":\"ftdp\",\"type\":\"text\"}],\"body\": ");
                if (JsonInput) sbP.Append("{\"mode\": \"raw\",\"raw\": \"{\\r\\n ");
                else sbP.Append("{\"mode\": \"formdata\",\"formdata\": [");
                string[] keys = ApiItem[2].Split(new string[] { "[#]" }, StringSplitOptions.None);
                string[] shuomings = ApiItem[4].Split(new string[] { "[#]" }, StringSplitOptions.None);
                if (JsonInput) sb.Append(HtmlDiv(1, "\"" + FidCol + "\":\"\", Optional. Post or the first param of query,bound to @p1@"));
                else sb.Append("<tr><td>" + FidCol + "</td><td>Optional. Post or the first param of query,bound to @p1@</td></tr>");
                if (JsonInput) sbP.Append(" \\\"" + FidCol + "\\\": \\\"\\\",/* Optional. Post or the first param of query,bound to @p1@ */\\r\\n ");
                else sbP.Append("{\"key\": \"" + FidCol + "\",\"value\": \"\",\"description\": \"Optional. Post or the first param of query,bound to @p1@\",\"type\": \"text\"}");
                for (int i = 0; i < keys.Length; i++)
                {
                    if (keys[i].Equals("") || keys[i].StartsWith("_")) continue;
                    if (i > -1 && !JsonInput) sbP.Append(",");
                    if (JsonSet != null && JsonSet.ContainsKey(keys[i]))
                    {
                        StringBuilder sbj = new StringBuilder();
                        StringBuilder sbjP = new StringBuilder();
                        JArray ja = JArray.Parse(JsonSet[keys[i]]);
                        DocJson(ja, sbj, sbjP, 0);
                        ja.Clear();
                        ja = null;
                        if (JsonInput)
                        {
                            sb.Append(HtmlDiv(1, "\"" + keys[i] + "\": <font style='color:blue'>" + shuomings[i] + "</font>"));
                            sb.Append(HtmlDiv(4, "<font style='color:blue'>" + sbj.ToString() + "</font>"));
                        }
                        else sb.Append("<tr><td>" + keys[i] + "</td><td style='color:blue'><font style='color:black'>" + shuomings[i] + "</font><br>" + sbj.ToString() + "</td></tr>");

                        if (JsonInput) sbP.Append(" \\\"" + keys[i] + "\\\": /* " + shuomings[i].Replace("\"", "") + " */\\r\\n" + sbjP.ToString() + ",\\r\\n ");
                        else sbP.Append("{\"key\": \"" + keys[i] + "\",\"value\": \"[]\",\"description\": \"" + shuomings[i].Replace("\"", "") + "\",\"type\": \"text\"}");
                    }
                    else
                    {
                        if (JsonInput) sb.Append(HtmlDiv(1, "\"" + keys[i] + "\":\"\", " + shuomings[i] + ""));
                        else sb.Append("<tr><td>" + keys[i] + "</td><td>" + shuomings[i] + "</td></tr>");
                        if (JsonInput) sbP.Append(" \\\"" + keys[i] + "\\\": \\\"\\\",/* " + shuomings[i].Replace("\"", "") + " */\\r\\n ");
                        else sbP.Append("{\"key\": \"" + keys[i] + "\",\"value\": \"\",\"description\": \"" + shuomings[i].Replace("\"", "") + "\",\"type\": \"text\"}");


                    }
                    sbDesc.Append(keys[i] + "{::}" + shuomings[i] + "{;;}");
                }
                if (JsonInput) sb.Append(HtmlDiv(0, "}"));
                else sb.Append("</table>");
                if (JsonInput) sbP.Append("}\", \"options\": { \"raw\": { \"language\": \"json\"} } }, ");
                else sbP.Append("]},");
                sbP.Append("\"url\": {");
                sbP.Append("\"raw\": \"[[[Proto]]]://[[[Host]]]" + ApiPath + "\",");///yanshi/10/?GetList_add
                string query = ApiPath.Contains('?') ? ApiPath.Split('?')[1] : "";
                string[] pathitem = ApiPath.Split('?')[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                sbP.Append("\"protocol\": \"[[[Proto]]]\",");
                sbP.Append("\"host\": [\"[[[Host]]]\"],");
                sbP.Append("\"path\": [");
                for (var i = 0; i < pathitem.Length; i++)
                {
                    if (i > 0) sbP.Append(",");
                    sbP.Append("\"" + pathitem[i] + "\"");
                }
                sbP.Append("]");
                if (!string.IsNullOrEmpty(query))
                {

                    sbP.Append(",\"query\": [{");
                    sbP.Append("\"key\": \"" + query + "\",");
                    sbP.Append("\"value\": null");
                    sbP.Append("}]");
                }
                sbP.Append("}},\"response\": []}],\"protocolProfileBehavior\": {}}");
                string InputDesc = sb.ToString();
                string PostManJson = sbP.ToString();
                sb = new StringBuilder();
                sb.Append(HtmlDiv(0, "{"));
                sb.Append(HtmlDiv(1, "\"code\": 200,"));
                sb.Append(HtmlDiv(1, "\"" + Project.Core.Api.MessageStr + "\":\"success\","));
                sb.Append(HtmlDiv(1, "\"tip\":\"Operation successful\","));
                sb.Append(HtmlDiv(1, "\"data\": {"));
                sb.Append(HtmlDiv(2, "\"newId\": \"526f9e09_93e0_46cd_8e14_267a8046c8bc\""));
                sb.Append(HtmlDiv(1, "}"));
                sb.Append(HtmlDiv(0, "}"));
                string OutDesc = sb.ToString();
                sql = "delete from ft_ftdp_apidoc where ApiPath='" + str.D2DD(ApiPath) + "'";
                db.ExecSql(sql);
                sql = "insert into ft_ftdp_apidoc(Fid,ApiPath,PageID,PageCaption,DevUser,ApiType,Mimo,InputDesc,OutputDesc,ModTime,PostManJson,KeyDesc)";
                sql += "values(@Fid,@ApiPath,@PageID,@PageCaption,@DevUser,@ApiType,@Mimo,@InputDesc,@OutputDesc,@ModTime,@PostManJson,@KeyDesc)";
                db.ExecSql(sql, new PR[] {
                                    new PR("@Fid",Fid),
                                    new PR("@ApiPath",ApiPath),
                                    new PR("@PageID",pageid),
                                    new PR("@PageCaption",Caption),
                                    new PR("@DevUser",DevUser),
                                    new PR("@ApiType","DataOP"),
                                    new PR("@Mimo",Mimo),
                                    new PR("@InputDesc",InputDesc),
                                    new PR("@OutputDesc",OutDesc),
                                    new PR("@ModTime",DateTime.Now),
                                    new PR("@PostManJson",PostManJson),
                                    new PR("@KeyDesc",sbDesc.ToString()),
                                    });
            }
        }
        private static void DocJson(JArray Ja, StringBuilder Sb, StringBuilder SbP, int Left)
        {
            Sb.Append(HtmlDiv(Left, "[{"));
            SbP.Append("          [{");
            List<JToken> list = Ja.ToList<JToken>();
            foreach (JToken jt in list)
            {
                if (jt.Type.ToString() == "Object")
                {
                    List<JToken> list1 = ((JObject)jt).ToList<JToken>();
                    string item = ((JProperty)list1[0]).Value.ToString();
                    string[] row = item.Split(new string[] { "##" }, StringSplitOptions.None);
                    string caption = row[0].Trim();
                    string name = row[1].Trim();
                    if (name.Equals("") || name.StartsWith("_")) continue;
                    int special = int.Parse(row[5].Trim());
                    if (string.IsNullOrWhiteSpace(caption)) caption = name;
                    string tip = "";
                    if (special == 1) tip = "<font color=red>Primary Key,mandatory when update operation</font>";
                    if (special != 2)
                    {
                        if (((JProperty)list1[1]).Value.Type.ToString() != "Array" || ((JProperty)list1[1]).Value.ToString() == "[]")
                        {
                            Sb.Append(HtmlDiv(Left + 1, "\"" + name + "\":\"" + caption + "\"," + tip));
                            SbP.Append("\\r\\n            " + "\\\"" + name + "\\\":\\\"" + caption + "\\\",/* " + tip + " */");
                        }
                        else
                        {
                            Sb.Append(HtmlDiv(Left + 1, "\"" + name + "\":"));
                            SbP.Append("\\r\\n            \\\"" + name + "\\\":");
                            DocJson(((JArray)((JProperty)list1[1]).Value), Sb, SbP, Left + 2);
                            Sb.Append(HtmlDiv(Left + 2, ","));
                            SbP.Append("\\r\\n            ,");
                        }
                    }
                }
            }
            Sb.Append(HtmlDiv(Left, "}]"));
            SbP.Append("\\r\\n            }]");
        }
        private static string HtmlDiv(int Left, string str)
        {
            return "<div style='padding-left:" + (20 * Left) + "px'>" + str + "</div>";
        }
        private static void SBA(StringBuilder sb, int tabCount, string str, bool isAppendLine = true)
        {
            var s = "";
            for (int i = 0; i < tabCount; i++)
            {
                s += "\t";
            }
            if (isAppendLine) sb.AppendLine(s + str);
            else sb.Append(s + str);
        }
        public static string getPartParamValue(XmlDocument doc, string ParaName)
        {
            XmlNodeList nodes = doc.SelectNodes("partxml/param");
            foreach (XmlNode node in nodes)
            {
                if (node.SelectSingleNode("name").InnerText == ParaName)
                {
                    return node.SelectSingleNode("value").InnerText;
                }
            }
            return "";
        }
    }
}
