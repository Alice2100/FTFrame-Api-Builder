/// <summary>
 /// 压缩文件
 /// </summary>modify by alice.maobb
 ///  /// <summary>
 /// 解压文件
 /// </summary>
using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.GZip;

namespace FTDPClient.Compression
{
[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class ZipSite
	{
		private ZipOutputStream s;
		private Crc32 crc;
		public ZipSite(string ZipFileName)
		{
			ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = System.Text.Encoding.UTF8.CodePage;
			crc = new Crc32();
		s = new ZipOutputStream(File.Create(ZipFileName));
		s.SetLevel(6); // 0 - store only to 9 - means best compression
		}
		public void ZipEnd()
		{
			s.Finish();
			s.Close();
		}
		public void ZipFilesAndFolders(string[] filenames,string[] folders,string headfolder)
		{			
  
			foreach (string file in filenames) 
			{
                if (file==null||file.Equals("")) continue;
				//打开压缩文件
				FileStream fs = File.OpenRead(file);
   
				FileInfo fi=new FileInfo(file);
   
				byte[] buffer = new byte[fs.Length];
				fs.Read(buffer, 0, buffer.Length);
				ZipEntry entry = new ZipEntry(headfolder + fi.Name);
				
   
				//entry.DateTime = DateTime.Now;
				entry.DateTime = fi.LastWriteTime;
				fi=null;
   
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
   
				entry.Crc  = crc.Value;
   
				s.PutNextEntry(entry);
   
				s.Write(buffer, 0, buffer.Length);
   
			}
			foreach(string resfolder in folders)
			{

				if(Directory.Exists(resfolder))
				{
					DirectoryInfo dio=new DirectoryInfo(resfolder);
			
					string[] filenamessub = Directory.GetFiles(resfolder);
					string[] foldernamessub=Directory.GetDirectories(resfolder);

					string headfolder2=headfolder + dio.Name + @"\";
					ZipFilesAndFolders(filenamessub,foldernamessub,headfolder2);
				}
			}
  
			
		}
	}
	[System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
	public class ZipClass
	{
 
		public ZipClass()
		{
			ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = System.Text.Encoding.UTF8.CodePage;
			//ZipStrings.CodePage = System.Text.Encoding.UTF8.CodePage;
		}
		public void ZipFile(string FileToZip, string ZipedFile ,int CompressionLevel, int BlockSize)
		{
			//如果文件没有找到，则报错
			if (! System.IO.File.Exists(FileToZip)) 
			{
				throw new System.IO.FileNotFoundException("The specified file " + FileToZip + " could not be found. Zipping aborderd");
			}
  
			System.IO.FileStream StreamToZip = new System.IO.FileStream(FileToZip,System.IO.FileMode.Open , System.IO.FileAccess.Read);
			System.IO.FileStream ZipFile = System.IO.File.Create(ZipedFile);
			ZipOutputStream ZipStream = new ZipOutputStream(ZipFile);
			ZipEntry ZipEntry = new ZipEntry("ZippedFile");
			ZipStream.PutNextEntry(ZipEntry);
			ZipStream.SetLevel(CompressionLevel);
			byte[] buffer = new byte[BlockSize];
			System.Int32 size =StreamToZip.Read(buffer,0,buffer.Length);
			ZipStream.Write(buffer,0,size);
			try 
			{
				while (size < StreamToZip.Length) 
				{
					int sizeRead =StreamToZip.Read(buffer,0,buffer.Length);
					ZipStream.Write(buffer,0,sizeRead);
					size += sizeRead;
				}
			} 
			catch(System.Exception ex)
			{
				throw ex;
			}
			ZipStream.Finish();
			ZipStream.Close();
			StreamToZip.Close();
		}
 
		public void ZipFileMain(string inputpath,string zipfile)
		{
			string[] filenames = Directory.GetFiles(inputpath);
  
			Crc32 crc = new Crc32();
			ZipOutputStream s = new ZipOutputStream(File.Create(zipfile));
  
			s.SetLevel(6); // 0 - store only to 9 - means best compression
  
			foreach (string file in filenames) 
			{
				//打开压缩文件
				FileStream fs = File.OpenRead(file);
				FileInfo fi=new FileInfo(file);
   
				byte[] buffer = new byte[fs.Length];
				fs.Read(buffer, 0, buffer.Length);
				ZipEntry entry = new ZipEntry(fi.Name);
				fi=null;
   
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
   
				entry.Crc  = crc.Value;
   
				s.PutNextEntry(entry);
   
				s.Write(buffer, 0, buffer.Length);
   
			}
  
			s.Finish();
			s.Close();
		}
		public void ZipFiles(string[] filenames,string zipfile)
		{
  
			Crc32 crc = new Crc32();
			ZipOutputStream s = new ZipOutputStream(File.Create(zipfile));
  
			s.SetLevel(6); // 0 - store only to 9 - means best compression
  
			foreach (string file in filenames) 
			{
				//打开压缩文件
				FileStream fs = File.OpenRead(file);
   
				FileInfo fi=new FileInfo(file);
   
				byte[] buffer = new byte[fs.Length];
				fs.Read(buffer, 0, buffer.Length);
				ZipEntry entry = new ZipEntry(fi.Name);
				fi=null;
   
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
   
				entry.Crc  = crc.Value;
   
				s.PutNextEntry(entry);
   
				s.Write(buffer, 0, buffer.Length);
   
			}
  
			s.Finish();
			s.Close();
		}
		public void UnZip(string zipfile,string ouputpath)
		{
			ZipInputStream s = new ZipInputStream(File.OpenRead(zipfile));
  
			ZipEntry theEntry;
			while ((theEntry = s.GetNextEntry()) != null) 
			{
   
				string directoryName = Path.GetDirectoryName(ouputpath);
				string fileName      = Path.GetFileName(theEntry.Name);
				DateTime dt=theEntry.DateTime;
   
				//生成解压目录
				//Directory.CreateDirectory(directoryName);
                if (fileName != String.Empty)
                {
                    try
                    {
                        //解压文件到指定的目录
                        string filename = ouputpath + theEntry.Name;

                        Directory.CreateDirectory(Path.GetDirectoryName(filename));
                        File.Delete(filename);
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
                    catch
                    {
                        continue;
                    }
                }
			}
			s.Close();
		}

	}
}