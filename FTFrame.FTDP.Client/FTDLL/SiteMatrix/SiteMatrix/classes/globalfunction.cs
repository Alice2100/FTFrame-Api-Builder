using System;
using System.Diagnostics;
using System.Windows.Forms;
using SiteMatrix.forms;
using System.IO;
using SiteMatrix.consts;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace SiteMatrix.functions
{
    /// <summary>
    /// globalfunction 的摘要说明。
    /// </summary>
    public class MsgBox
    {
        public MsgBox()
        {
        }
        public MsgBox(string text)
        {
            MessageBox.Show(text);
        }
        public static DialogResult msg(string text, string caption)
        {
            return MessageBox.Show(text, caption);
        }
        public static DialogResult msg(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(text, caption, buttons, icon);
        }
        public static DialogResult msg(string text, string caption, MessageBoxButtons buttons)
        {
            return MessageBox.Show(text, caption, buttons);
        }
        public static DialogResult msg(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return MessageBox.Show(text, caption, buttons, icon, defaultButton);
        }
        public static DialogResult msg(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return MessageBox.Show(text, caption, buttons, icon, defaultButton, options);
        }
        public static void Information(string text)
        {
            MessageBox.Show(text, res._globalfunctions.GetString("c1"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void Warning(string text)
        {
            MessageBox.Show(text, res._globalfunctions.GetString("c2"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            log.Debug(text, "Message Warning");
        }
        public static void Error(string text)
        {
            MessageBox.Show(text, res._globalfunctions.GetString("c3"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            log.Debug(text, "Message Error");
        }
        public static DialogResult OKCancel(string text)
        {
            return MessageBox.Show(text,  res._globalfunctions.GetString("c4"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }
        public static DialogResult YesNo(string text)
        {
            return MessageBox.Show(text, res._globalfunctions.GetString("c4"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        public static DialogResult YesNoCancel(string text)
        {
            return MessageBox.Show(text, res._globalfunctions.GetString("c4"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }
        public static DialogResult RetryCancel(string text)
        {
            return MessageBox.Show(text, res._globalfunctions.GetString("c4"), MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
        }
        public static DialogResult AbortRetryIgnore(string text)
        {
            return MessageBox.Show(text, res._globalfunctions.GetString("c4"), MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question);
        }
    }
    public class error
    {
        public error()
        { }
        public error(Exception ex)
        {
            ErrorReport er = new ErrorReport();
            er.errorString = "{Message}:\r\n" + ex.Message + "\r\n{Source}:\r\n" + ex.Source + "\r\n{InnerException}:\r\n" + ex.InnerException + "\r\n{StackTrace}:\r\n" + ex.StackTrace + "\r\n{TargetSite}:\r\n" + ex.TargetSite;
            er.errorView = "{Message}:\r\n" + ex.Message + "\r\n{Source}:\r\n" + ex.Source + "\r\n{InnerException}:\r\n" + ex.InnerException + "\r\n{StackTrace}:\r\n" + ex.StackTrace + "\r\n{TargetSite}:\r\n" + ex.TargetSite;
            log.Error(er.errorString, "from class error");
            er.ShowDialog();
        }
    }
    public class file
    {
        public file()
        { }
        public file(string filename)
        {
            try
            {
                File.CreateText(filename);
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static FileStream Create(string filename)
        {
            try
            {
                return File.Create(filename);
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static void Copy(string sourceFileName, string destFileName)
        {
            try
            {
                File.Copy(sourceFileName, destFileName);
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            try
            {
                File.Copy(sourceFileName, destFileName, overwrite);
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static void AppenText(string FileName, string appenText)
        {
            try
            {
                FileInfo fi = new FileInfo(FileName);
                StreamWriter sw = fi.AppendText();
                sw.Write(appenText);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static void CreateText(string FileName, string createText)
        {
            try
            {
                FileInfo fi = new FileInfo(FileName);
                StreamWriter sw = fi.CreateText();
                sw.Write(createText);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static void Move(string sourceFileName, string destFileName)
        {
            try
            {
                File.Move(sourceFileName, destFileName);
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static string getFileText(string filename, System.Text.Encoding Encode)
        {
            try
            {
                //FileInfo fi = new FileInfo(filename);
                //fi.OpenRead();
                StreamReader sr = new StreamReader(filename, Encode);
                string rs = sr.ReadToEnd();
                sr.Close();
                return rs;

            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static FileStream Open(string filename, FileMode mode)
        {
            try
            {
                return File.Open(filename, mode);
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static FileStream Open(string filename, FileMode mode, FileAccess access)
        {
            try
            {
                return File.Open(filename, mode, access);
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static FileStream Open(string filename, FileMode mode, FileAccess access, FileShare share)
        {
            try
            {
                return File.Open(filename, mode, access, share);
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static FileStream OpenRead(string filename)
        {
            try
            {
                return File.OpenRead(filename);
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static StreamReader OpenText(string filename)
        {
            try
            {
                return File.OpenText(filename);
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static FileStream OpenWrite(string filename)
        {
            try
            {
                return File.OpenWrite(filename);
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static bool Exists(string filename)
        {
            return File.Exists(filename);
        }
        public static void Delete(string filename)
        {
            File.Delete(filename);
        }
    }
    public class dir
    {
        public dir()
        { }
        public dir(string dirpath)
        {
            try
            {
                Directory.CreateDirectory(dirpath);
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static void Delete(string dirpath)
        {
            try
            {
                Directory.Delete(dirpath);
            }
            catch 
            {
            }
        }
        public static void Delete(string dirpath, bool recursive)
        {
            try
            {
                Directory.Delete(dirpath, recursive);
            }
            catch
            {
            }
        }
        public static bool Exists(string dirpath)
        {
            return Directory.Exists(dirpath);
        }
        public static void Move(string sourceDirName, string destDirName)
        {
            Directory.Move(sourceDirName, destDirName);
        }
        /// <SUMMARY>
        /// Copy a Directory, SubDirectories and Files Given a Source and  
        /// Destination DirectoryInfo Object, Given a SubDirectory Filter
        /// and a File Filter.
        /// IMPORTANT: The search strings for SubDirectories and Files applies 
        /// to every Folder and File within the Source Directory.
        /// </SUMMARY>
        /// <PARAM name="SourceDirectory">A DirectoryInfo Object Pointing 
        /// to the Source Directory</PARAM>
        /// <PARAM name="DestinationDirectory">A DirectoryInfo Object Pointing 
        /// to the Destination Directory</PARAM>
        /// <PARAM name="SourceDirectoryFilter">Search String on  
        ///   SubDirectories (Example: "System*" will return all subdirectories
        ///   starting with "System")</PARAM>
        /// <PARAM name="SourceFileFilter">File Filter: Standard DOS-Style Format 
        ///    (Examples: "*.txt" or "*.exe")</PARAM>
        /// <PARAM name="Overwrite">Whether or not to Overwrite Copied Files in the
        ///     Destination Directory</PARAM>
        public static void Copy(DirectoryInfo SourceDirectory,
            DirectoryInfo DestinationDirectory, string SourceDirectoryFilter,
            string SourceFileFilter, bool Overwrite)
        {
            DirectoryInfo[] SourceSubDirectories;
            FileInfo[] SourceFiles;

            //Check for File Filter
            if (SourceFileFilter != null)
                SourceFiles = SourceDirectory.GetFiles(SourceFileFilter.Trim());
            else
                SourceFiles = SourceDirectory.GetFiles();

            //Check for Folder Filter
            if (SourceDirectoryFilter != null)
                SourceSubDirectories = SourceDirectory.GetDirectories(
                    SourceDirectoryFilter.Trim());
            else
                SourceSubDirectories = SourceDirectory.GetDirectories();

            //Create the Destination Directory
            if (!DestinationDirectory.Exists) DestinationDirectory.Create();

            //Recursively Copy Every SubDirectory and it's 
            //Contents (according to folder filter)
            foreach (DirectoryInfo SourceSubDirectory in SourceSubDirectories)
                Copy(SourceSubDirectory, new DirectoryInfo(
                    DestinationDirectory.FullName + @"\" + SourceSubDirectory.Name),
                    SourceDirectoryFilter, SourceFileFilter, Overwrite);

            //Copy Every File to Destination Directory (according to file filter)
            foreach (FileInfo SourceFile in SourceFiles)
                SourceFile.CopyTo(DestinationDirectory.FullName +
                    @"\" + SourceFile.Name, Overwrite);
        }
        public static void Copy(DirectoryInfo SourceDirectory,
            DirectoryInfo DestinationDirectory, System.Windows.Forms.Label stat, bool Overwrite)
        {
            DirectoryInfo[] SourceSubDirectories;
            FileInfo[] SourceFiles;

            //Check for File Filter

            SourceFiles = SourceDirectory.GetFiles();

            //Check for Folder Filter

            SourceSubDirectories = SourceDirectory.GetDirectories();

            //Create the Destination Directory
            if (!DestinationDirectory.Exists) DestinationDirectory.Create();

            //Recursively Copy Every SubDirectory and it's 
            //Contents (according to folder filter)
            foreach (DirectoryInfo SourceSubDirectory in SourceSubDirectories)
                Copy(SourceSubDirectory, new DirectoryInfo(
                    DestinationDirectory.FullName + @"\" + SourceSubDirectory.Name),
                    stat, Overwrite);

            //Copy Every File to Destination Directory (according to file filter)
            foreach (FileInfo SourceFile in SourceFiles)
            {
                System.Windows.Forms.Application.DoEvents();
                stat.Text = "Copying site file " + SourceFile.Name;
                System.Windows.Forms.Application.DoEvents();

                SourceFile.CopyTo(DestinationDirectory.FullName +
                    @"\" + SourceFile.Name, Overwrite);
            }
        }
        public static DirectoryInfo CreateDirectory(string DirName)
        {
            return Directory.CreateDirectory(DirName);
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class control
    {
        public static string GetSharedCaptionByIndex(string i)
        {
            switch (i)
                {
                    case "0": return res._globalfunctions.GetString("s1");
                    case "1": return res._globalfunctions.GetString("s2");
                    case "2": return res._globalfunctions.GetString("s3");
                }
                if (i.Equals(res._globalfunctions.GetString("s1"))) return "0";
                if (i.Equals(res._globalfunctions.GetString("s2"))) return "1";
                if (i.Equals(res._globalfunctions.GetString("s3"))) return "2";
                return "(Not Select)";
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class tree
    {
        public enum NodeType
        {
            Root=0,Directory=1,Page=2,Control=3,Instance=4,Part=5,Unknown=6
        }
        public static NodeType getNodeType(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            switch(getTypeFromID(id))
            {
                case "root":
                    return NodeType.Root;
                case "drct":
                    return NodeType.Directory;
                case "page":
                    return NodeType.Page;
                case "comp":
                    return NodeType.Control;
                case "ctrl":
                    return NodeType.Instance;
                case "part":
                    return NodeType.Part;
            }
            return NodeType.Unknown;
        }
        public static NodeType getNodeType(TreeNode nd)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            switch (getTypeFromID(nd))
            {
                case "root":
                    return NodeType.Root;
                case "drct":
                    return NodeType.Directory;
                case "page":
                    return NodeType.Page;
                case "comp":
                    return NodeType.Control;
                case "ctrl":
                    return NodeType.Instance;
                case "part":
                    return NodeType.Part;
            }
            return NodeType.Unknown;
        }
        public static string getTypeFromID(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                return id.Substring(id.Length - 4);
            }
            catch (Exception e)
            {
                new error(e);
                return "unknown";
            }
        }
        public static string getTypeFromID(TreeNode nd)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                string id = ((string[])nd.Tag)[2];
                return id.Substring(id.Length - 4);
            }
            catch (Exception e)
            {
                new error(e);
                return "unknown";
            }
        }
        public static string getID(TreeNode nd)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                return ((string[])nd.Tag)[2];
            }
            catch (Exception e)
            {
                new error(e);
                return "unknown";
            }
        }
        public static TreeNode getTreeNodeByID(string id,TreeView tv)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                tree.NodeGetByValue = null;
                tree.getNodebyValue(tv.Nodes, 2, id);
                return tree.NodeGetByValue;
            }
            catch (Exception ex)
            {
                new error(ex);
                return null;
            }
        }
        public static TreeNode getSiteNodeByID(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                tree.NodeGetByValue = null;
                tree.getNodebyValue(globalConst.MdiForm.SiteTree.Nodes, 2, id);
                return tree.NodeGetByValue;
            }
            catch (Exception ex)
            {
                new error(ex);
                return null;
            }
        }
        public static TreeNode getCtrlNodeByID(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                tree.NodeGetByValue = null;
                tree.getNodebyValue(globalConst.MdiForm.ControlTree.Nodes, 2, id);
                return tree.NodeGetByValue;
            }
            catch (Exception ex)
            {
                new error(ex);
                return null;
            }
        }
        public static TreeNode NodeGetByValue;
        public static void getNodebyValue(TreeNodeCollection nds, int tagI, string _value)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                int i;
                for (i = 0; i < nds.Count; i++)
                {
                    if (_value.Equals(((string[])nds[i].Tag)[tagI]))
                    {
                        NodeGetByValue = nds[i];
                        return;
                    }
                    getNodebyValue(nds[i].Nodes, tagI, _value);
                }
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static string getPath(TreeNode nd)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                string path = ((string[])nd.Tag)[0];
                string id = ((string[])nd.Tag)[2];
                if (id.Equals("root")) return ("");
                else
                    return getPath(nd.Parent) + @"\" + path;
            }
            catch (Exception e)
            {
                new error(e);
                return "unknown";
            }
        }
        public static string getSubPath(TreeNode nd)
        {
        System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                string id = ((string[])nd.Tag)[2];
                if (id.Equals("root")) return ("");
                else
                    return getSubPath(nd.Parent) + @"../";
            }
            catch (Exception e)
            {
                new error(e);
                return "";
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class rdm
    {
        public static string getDataSourceID()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            Random r = new Random();
            string s = "";
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            return s;
        }
       
        public static string getCombID()
        {
            return GenerateComb().ToString().Replace("-", "_");
        }
        public static string getID()
        {
            //"g5pi3_ps1fi_hy2d_223552";
            return System.Guid.NewGuid().ToString().Replace("-","_");
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            Random r = new Random();
            string s = "";
            s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            s += "_";

            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();

            s += "_";

            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            if (r.Next(1, 36) > 10)
                s += System.Convert.ToChar(r.Next(97, 122)).ToString();
            else
                s += r.Next(0, 9).ToString();
            s += "_";
            DateTime d = DateTime.Now;
            string s1 = d.Hour.ToString();
            string s2 = d.Minute.ToString();
            string s3 = d.Second.ToString();
            string f = (s1.Length == 1 ? s1 = "0" + s1 : s1 = s1) + (s2.Length == 1 ? s2 = "0" + s2 : s2 = s2) + (s3.Length == 1 ? s3 = "0" + s3 : s3 = s3);
            s += f;
            return s;
        }
/**//// <summary>
/**//// Generate a new <see cref="Guid"/> using the comb algorithm.
/**//// </summary>
private static Guid GenerateComb()
{
    byte[ ] guidArray = Guid.NewGuid().ToByteArray();

    DateTime baseDate = new DateTime( 1900, 1, 1 );
    DateTime now = DateTime.Now;

 // Get the days and milliseconds which will be used to build 
//the byte string 
    TimeSpan days = new TimeSpan( now.Ticks - baseDate.Ticks );
    TimeSpan msecs = now.TimeOfDay;

    // Convert to a byte array   
    // Note that SQL Server is accurate to 1/300th of a 
   // millisecond so we divide by 3.333333 
    byte[ ] daysArray = BitConverter.GetBytes( days.Days );
    byte[ ] msecsArray = BitConverter.GetBytes( ( long )  ( msecs.TotalMilliseconds/3.333333 ) );

    // Reverse the bytes to match SQL Servers ordering 
    Array.Reverse( daysArray );
    Array.Reverse( msecsArray );

    // Copy the bytes into the guid 
    Array.Copy( daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2 );
    Array.Copy( msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4 );
    return new Guid( guidArray );	
}
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class log
    {
        public static void Info(string info)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            if (globalConst.LogLevel > 2)
            {
                string path = globalConst.LogOutputPath + @"\dssmlog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path)) { }
                }
                FileInfo fi = new FileInfo(path);
                StreamWriter sw = fi.AppendText();
                sw.WriteLine("[DSSM]Info[" + DateTime.Now + "]:" + info);
                sw.Flush();
                sw.Close();
            }
        }
        public static void Info(string info, string other)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            if (globalConst.LogLevel > 2)
            {
                string path = globalConst.LogOutputPath + @"\dssmlog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path)) { }
                }
                FileInfo fi = new FileInfo(path);
                StreamWriter sw = fi.AppendText();
                sw.WriteLine("[DSSM]Info[" + DateTime.Now + "]{" + other + "}:" + info);
                sw.Flush();
                sw.Close();
            }
        }
        public static void Debug(string debug)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            if (globalConst.LogLevel > 1)
            {
                string path = globalConst.LogOutputPath + @"\dssmlog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path)) { }
                }
                FileInfo fi = new FileInfo(path);
                StreamWriter sw = fi.AppendText();
                sw.WriteLine("[DSSM]Debug[" + DateTime.Now + "]:" + debug);
                sw.Flush();
                sw.Close();
            }
        }
        public static void Debug(string debug, string other)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            if (globalConst.LogLevel > 1)
            {
                string path = globalConst.LogOutputPath + @"\dssmlog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path)) { }
                }
                FileInfo fi = new FileInfo(path);
                StreamWriter sw = fi.AppendText();
                sw.WriteLine("[DSSM]Debug[" + DateTime.Now + "]{" + other + "}:" + debug);
                sw.Flush();
                sw.Close();
            }
        }
        public static void Error(string error)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            if (globalConst.LogLevel > 0)
            {
                string path = globalConst.LogOutputPath + @"\dssmlog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path)) { }
                }
                FileInfo fi = new FileInfo(path);
                StreamWriter sw = fi.AppendText();
                sw.WriteLine("[DSSM]Error[" + DateTime.Now + "]:" + error);
                sw.Flush();
                sw.Close();
            }
        }
        public static void Error(string error, string other)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            if (globalConst.LogLevel > 0)
            {
                string path = globalConst.LogOutputPath + @"\dssmlog_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path)) { }
                }
                FileInfo fi = new FileInfo(path);
                StreamWriter sw = fi.AppendText();
                sw.WriteLine("[DSSM]Error[" + DateTime.Now + "]{" + other + "}:" + error);
                sw.Flush();
                sw.Close();
            }
        }

    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class str
    {
        public static bool IsFormEleTag(string tagName,string type)
        {
            if (tagName == null) return false;
            if (type != null && (type.ToLower().Equals("button") || type.ToLower().Equals("submit"))) return false;
            tagName = tagName.ToLower();
            return "|input|select|textarea|label|span".IndexOf("|" + tagName + "|") >= 0;
        }
         public static bool IsNatural(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9_]+$");
            return reg1.IsMatch(str);
        } 
        public static void StatusClear()
        {
            Application.DoEvents();
            globalConst.MdiForm.MainStatus.Text = "";
            Application.DoEvents();
        }
        public static void ShowStatus(string s)
        {
            Application.DoEvents();
            globalConst.MdiForm.MainStatus.Text = s;
            Application.DoEvents();
        }
        public static string Dot2DotDot(string s)
        {
            if (s == null) return "";
            return s.Replace("'", "''");
        }
        public static string EncodingConvert(string ins, Encoding sce, Encoding des)
        {
            try
            {
                byte[] unicodeBytes = sce.GetBytes(ins);

                // Perform the conversion from one encoding to the other.
                byte[] beBytes = Encoding.Convert(sce, des, unicodeBytes);

                // Convert the new byte[] into a char[] and then into a string.
                // This is a slightly different approach to converting to illustrate
                // the use of GetCharCount/GetChars.
                char[] beChars = new char[des.GetCharCount(beBytes, 0, beBytes.Length)];
                des.GetChars(beBytes, 0, beBytes.Length, beChars, 0);
                return new string(beChars);
            }
            catch (Exception ex)
            {
                new error(ex);
                return "";
            }
        }
        public static bool AuthOK(string url)
        {
            return true;
            return DateTime.Now < DateTime.Parse("2018-12-12") && url.IndexOf("jiulifintech.com") >= 0;
        }
        public static string getEncode(string str)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "^#@$FVSD#$%SDF@#maobb234efwe";

            String sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);

            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength);
            }
            byte[] kkkk = ASCIIEncoding.ASCII.GetBytes(sTemp);


            sTemp = "Dmaobbasfui23497#$ASasdkf0";
            mobjCryptoService.GenerateIV();
            bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength);
            }
            byte[] vvvv = ASCIIEncoding.ASCII.GetBytes(sTemp);

            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(str.ToString());
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = kkkk;
            mobjCryptoService.IV = vvvv;
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }
        public static string getDecode(string str)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "^#@$FVSD#$%SDF@#maobb234efwe";
            String sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);
            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength);
            }

            byte[] kkkk = ASCIIEncoding.ASCII.GetBytes(sTemp);
            sTemp = "Dmaobbasfui23497#$ASasdkf0";
            mobjCryptoService.GenerateIV();
            bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength);
            }
            byte[] vvvv = ASCIIEncoding.ASCII.GetBytes(sTemp);

            byte[] bytIn = Convert.FromBase64String(str);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = kkkk;
            mobjCryptoService.IV = vvvv;
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
        public static string getEncode2(string str)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "hkNndjhgdDF@#maobb234efwe";

            String sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);

            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength);
            }
            byte[] kkkk = ASCIIEncoding.ASCII.GetBytes(sTemp);


            sTemp = "hidnahfkandi23497#$ASasdkf0";
            mobjCryptoService.GenerateIV();
            bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength);
            }
            byte[] vvvv = ASCIIEncoding.ASCII.GetBytes(sTemp);

            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(str.ToString());
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = kkkk;
            mobjCryptoService.IV = vvvv;
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }
        public static string getDecode2(string str)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "hkNndjhgdDF@#maobb234efwe";
            String sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);
            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength);
            }

            byte[] kkkk = ASCIIEncoding.ASCII.GetBytes(sTemp);
            sTemp = "hidnahfkandi23497#$ASasdkf0";
            mobjCryptoService.GenerateIV();
            bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength);
            }
            byte[] vvvv = ASCIIEncoding.ASCII.GetBytes(sTemp);

            byte[] bytIn = Convert.FromBase64String(str);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = kkkk;
            mobjCryptoService.IV = vvvv;
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
        public static string getMD5(string str)
        {
            byte[] sor = Encoding.UTF8.GetBytes(str);
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(sor);
            StringBuilder strbul = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

            }
            return strbul.ToString();
        }
        public static string GetSplitValue(string para,int index)
        {
            string stri = "";
            if (para != null && !para.Equals(""))
            {
                para = str.getDecode(para);
                para = para.Replace(";", "&&&").Replace("|||", ";");
                if (para.Split(';').Length < index + 1) return "";
                stri = para.Split(';')[index].Replace("&&&", ";").Trim();
            }
            return stri;
        }
        public static string SetSplitValue(string para,int paracount, int index,string newvalue)
        {
            if (para == null || para.Equals(""))
            {
                para = "";
                for (int i = 0; i < paracount; i++)
                {
                    if (i < paracount - 1) para += "|||";
                }
                para = str.getEncode(para);
            }
            para = str.getDecode(para);
            para = para.Replace(";", "&&&").Replace("|||", ";");
            for (int i = 0; i < paracount - para.Split(';').Length; i++)
            {
                para += ";";
            }
            string[] parastrs = para.Split(';');
            string newstring = "";
            for (int i = 0; i < parastrs.Length; i++)
            {
                if (i != index)
                {
                    newstring += parastrs[i].Replace("&&&", ";").Trim();
                }
                else
                {
                    newstring += newvalue;
                }
                if (i < parastrs.Length - 1) newstring += "|||";
            }
            return str.getEncode(newstring);
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class form
    {
        public static Editor getEditorByURL(string url)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                foreach (Form fm in globalConst.MdiForm.MdiChildren)
                {
                    if (fm.Name.Equals("Editor"))
                    {
                        if (((Editor)fm).thisUrl.ToLower().Equals(url.ToLower()))
                        {
                            return ((Editor)fm);
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static Editor getEditor(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                foreach (Form fm in globalConst.MdiForm.MdiChildren)
                {
                    if (fm.Name.Equals("Editor"))
                    {
                        if (((Editor)fm).thisID.Equals(id))
                        {
                            return ((Editor)fm);
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static void OutEdited()
        {
            try
            {
                foreach (Form fm in globalConst.MdiForm.MdiChildren)
                {
                    if (fm.Name.Equals("Editor"))
                    {
                        Editor ed = (Editor)fm;
                        if (!ed.isFreeFile)
                        {
                            FileInfo fio = new FileInfo(ed.thisUrl);
                            if (!fio.LastWriteTime.Equals(ed.UrlLastModTime))
                            {
                                str.ShowStatus("正在更新外部编辑内容   " + ed.Name);
                                ed.OutEdited();
                                ed.UrlLastModTime = fio.LastWriteTime;
                                str.StatusClear();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static List<Editor> getEditors()
        {
            try
            {
                List<Editor> list = new List<Editor>();
                foreach (Form fm in globalConst.MdiForm.MdiChildren)
                {
                    if (fm.Name.Equals("Editor"))
                    {
                        list.Add((Editor)fm);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                new error(e);
                return new List<Editor>();
            }
        }
        public static bool doActive(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                foreach (Form fm in globalConst.MdiForm.MdiChildren)
                {
                    if (fm.Name.Equals("Editor"))
                    {
                        if (((Editor)fm).thisID.Equals(id))
                        {
                            fm.Activate();
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                new error(e);
                return false;
            }
        }
        public static void addFreeFileEditor(string url, string title, string id, string name,string text,bool IsFreeSaved)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                url = url.Replace(@"\\", @"\");
                Editor edr = getEditorByURL(url);
                if (edr == null)
                {
                    Editor ed = new Editor();
                    ed.MdiParent = globalConst.MdiForm;
                    ed.thisUrl = url;
                    ed.thisTitle = title;
                    ed.thisID = id;
                    ed.thisName = name;
                    if (text.StartsWith("Static"))
                    {
                        ed.Text = "Free Page *";
                    }
                    else
                    {
                        ed.Text = text + " - " + url;
                    }
                    ed.isFreeFile = true;
                    ed.isFreeFileSaved = IsFreeSaved;
                    ed.Show();
                }
                else
                {
                    edr.Activate();
                }
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static Editor addEditor(string url, string title, string id, string name,int pagetype)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                Editor ed = new Editor();
                ed.MdiParent = globalConst.MdiForm;
                ed.thisUrl = url;
                ed.thisTitle = title;
                ed.thisID = id;
                ed.thisName = name;
                ed.Text = ed.thisTitle;
                ed.isFreeFile = false;
                ed.pagetype = pagetype;
                ed.Show();
                return ed;
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static void UpdateFileOpend(string EditorID, bool IsOpened)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                //init all
                if (EditorID == null)
                {
                    globalConst.MdiForm.FileOpend.Nodes.Clear();
                    TreeNode tn=new TreeNode(res._globalfunctions.GetString("c5"),22,22);
                    tn.Tag="root";
                    globalConst.MdiForm.FileOpend.Nodes.Add(tn);
                    foreach (Form fm in globalConst.MdiForm.MdiChildren)
                    {
                        if (fm.Name.Equals("Editor"))
                        {
                            UpdateFileOpend(((Editor)fm).thisID, true);
                        }
                    }
                }
                else
                {
                    if (IsOpened)
                    {
                        TreeNode cnd = null;
                        foreach (TreeNode nd in globalConst.MdiForm.FileOpend.Nodes[0].Nodes)
                        {
                            if (nd.Tag.ToString().Equals(EditorID))
                            {
                                cnd = nd;
                                break;
                            }
                        }
                        Editor edr = getEditor(EditorID);

                        TreeNode tn;
                        if (edr.isFreeFile)
                        {
                            if (edr.Text.EndsWith("*"))
                                tn = new TreeNode(edr.Text, 8, 8);
                            else
                            {
                                string edrText = edr.Text;
                                edrText.Replace("/", "\\");
                                if (edrText.LastIndexOf('\\') > 0)
                                    tn = new TreeNode(edrText.Substring(edrText.LastIndexOf('\\') + 1), 8, 8);
                                else
                                    tn = new TreeNode(edrText, 8, 8);
                            }
                        }
                        else
                        {
                            switch (edr.pagetype)
                            {
                                case 0:
                                    tn = new TreeNode(edr.Text, 19, 19);
                                    break;
                                case 1:
                                    tn = new TreeNode(edr.Text, 26, 26);
                                    break;
                                case 2:
                                    tn = new TreeNode(edr.Text, 27, 27);
                                    break;
                                default:
                                    tn = new TreeNode(edr.Text, 19, 19);
                                    break;
                            }
                            
                        }
                        tn.Tag = EditorID;
                        if (cnd == null)
                            globalConst.MdiForm.FileOpend.Nodes[0].Nodes.Add(tn);
                        else
                        {
                            cnd.Text = tn.Text;
                            cnd.Tag = tn.Tag;
                        }
                        globalConst.MdiForm.FileOpend.Nodes[0].ExpandAll();
                    }
                    else
                    {
                        foreach (TreeNode nd in globalConst.MdiForm.FileOpend.Nodes[0].Nodes)
                        {
                            if (nd.Tag.ToString().Equals(EditorID))
                            {
                                nd.Remove();
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static void closeEditor(string id)
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                int i;
                Form[] fms = globalConst.MdiForm.MdiChildren;
                for (i = 0; i < fms.Length; i++)
                {
                    if (fms[i].Name.Equals("Editor"))
                    {
                        Editor edr = (Editor)fms[i];
                        if (edr.thisID.Equals(id))
                        {
                            edr.Close();
                            fms = null;
                            return;
                        }
                    }
                }
                fms = null;
            }
            catch (Exception e)
            {
                new error(e);
            }
        }
        public static Editor getEditor()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                if (globalConst.MdiForm == null) return null;
                if (globalConst.MdiForm.ActiveMdiChild != null)
                {
                    if (globalConst.MdiForm.ActiveMdiChild.Name.Equals("Editor"))
                        return (Editor)globalConst.MdiForm.ActiveMdiChild;
                }
                if (globalConst.curActiveForm == null) return null;
                if (globalConst.curActiveForm.Name.Equals("Editor"))
                {
                    int i;
                    Editor er = (Editor)globalConst.curActiveForm;
                    Form[] fms = globalConst.MdiForm.MdiChildren;
                    for (i = 0; i < fms.Length; i++)
                    {
                        if (fms[i].Name.Equals("Editor"))
                        {
                            Editor edr = (Editor)fms[i];
                            if (edr.thisID.Equals(er.thisID))
                            {
                                return edr;
                            }
                        }
                    }
                    fms = null;
                }
                return null;
            }
            catch (Exception e)
            {
                new error(e);
                return null;
            }
        }
        public static bool IsEditorCount0()
        {
            if (globalConst.MdiForm.MdiChildren.Length > 1) return false;
            if (globalConst.MdiForm.MdiChildren.Length == 0) return true;
            if (globalConst.MdiForm.MdiChildren.Length == 1)
            {
                if (((Editor)globalConst.MdiForm.MdiChildren[0]).IsClosing) return true;
                else
                    return false;
            }
            return false;
        }
        public static int getEditorCount()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
            try
            {
                int j = 0;
                int i;
                Form[] fms = globalConst.MdiForm.MdiChildren;
                for (i = 0; i < fms.Length; i++)
                {
                    if (fms[i].Name.Equals("Editor"))
                    {
                        if (!((Editor)fms[i]).IsClosing)
                        {
                            j++;
                        }
                    }
                }
                fms = null;
                return j;
            }
            catch (Exception e)
            {
                new error(e);
                return 0;
            }
        }
    }
    public class sheel
    {
        public static string ExeCommand(string commandText)
        {

            Process p = new Process();

            p.StartInfo.FileName = "cmd.exe";

            p.StartInfo.UseShellExecute = false;

            p.StartInfo.RedirectStandardInput = true;

            p.StartInfo.RedirectStandardOutput = true;

            p.StartInfo.RedirectStandardError = true;

            p.StartInfo.CreateNoWindow = true;

            string strOutput = null;

            try
            {

                p.Start();

                p.StandardInput.WriteLine(commandText);

                p.StandardInput.WriteLine("exit");

                strOutput = p.StandardOutput.ReadToEnd();

                p.WaitForExit();

                p.Close();

            }

            catch (Exception e)
            {

                strOutput = e.Message;

            }

            return strOutput;

        }
        public static void ExeSheel(string sheelName)
        {
            try
            {
                Process p = new Process();

                p.StartInfo.FileName = sheelName;

                p.StartInfo.UseShellExecute = true;

                //p.StartInfo.RedirectStandardInput = true;

                //p.StartInfo.RedirectStandardOutput = true;

                //p.StartInfo.RedirectStandardError = true;

                //p.StartInfo.CreateNoWindow = true;

                //string strOutput = null;


                p.Start();

                //p.StandardInput.WriteLine(commandText);

                //p.StandardInput.WriteLine("exit");

                //strOutput = p.StandardOutput.ReadToEnd();

                //p.WaitForExit();

                //p.Close();
            }
            catch(Exception ex)
            {
                MsgBox.Warning(ex.Message);
            }

        }
    }
}
