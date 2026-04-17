using System;
using System.Data;
using System.Xml;
using System.IO;
using System.Diagnostics;
namespace DeployControl
{
	class Deploy
	{
		public static string warename="";
		public static string wareversion="";
		public static string control_company="";
		public static string control_copyright="";
		public static string indexpath="";
		public static string indexfile="";
		public static string allstring="";
		public static string outputpath="";
		public static string forexml="";
		public static string backxml="";
		public static string sqlxml="";
		public static string tempxml="";
		public static string CSCode="";
		public static XmlDocument xmldoc = new XmlDocument();
		public static bool forcss=false;
		public static string forcssstr="";
        public static StreamWriter logWriter;
        private static bool DeleteTemp = false;
        private static System.Text.Encoding NowEncoding=System.Text.Encoding.Default;
        public static string waredirpath = "";
		static void Main(string[] args)
		{
            START:
			try
			{
			    File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "//log");
                logWriter = File.CreateText(System.AppDomain.CurrentDomain.BaseDirectory + "//log");

				string warexmlpath;
				string waredeploy;
				string libpath;
				string outputpath2;
				string framepath;
				string compiletype;
				//string antpath;
				string d4sln;
				string d4csproj;
				string d4csproj2;
				while(warename.Equals(""))
				{
                    O("**************Enter Control Name**************");
					Console.OpenStandardInput();
					warename=Console.ReadLine().Trim().ToLower();
				}
                if(warename.EndsWith("#"))
                {
                    DeleteTemp = true;
                    warename = warename.Substring(0, warename.Length - 1);
                }



				xmldoc.Load("deploy.config");
				warexmlpath=xmldoc.SelectSingleNode("//configuration/controlxmlpath").InnerText;
				waredeploy=xmldoc.SelectSingleNode("//configuration/controldeploypath").InnerText;
				libpath=xmldoc.SelectSingleNode("//configuration/libpath").InnerText;
				outputpath2=xmldoc.SelectSingleNode("//configuration/outputpath").InnerText;
				framepath=xmldoc.SelectSingleNode("//configuration/framepath").InnerText;
				compiletype=xmldoc.SelectSingleNode("//configuration/compiletype").InnerText;
				//antpath=xmldoc.SelectSingleNode("//configuration/antpath").InnerText;
				L("Controlxmlpath is:" + warexmlpath);
				L("Controldeploy is:" + waredeploy);
				L("libpath is:" + libpath);
				L("outputpath is:" + outputpath2);
				L("framepath is:" + framepath);
				L("compiletype is:" + compiletype);
				//L("antpath is:" + antpath + "\r\n");

                
                if (warename.IndexOf("|") >= 0)
                {
                    waredirpath = warename.Split('|')[0];
                    indexpath = warexmlpath + waredirpath + @"\";
                    warename = warename.Split('|')[1];
                }
                else
                {
                    waredirpath = warename;
                    indexpath = warexmlpath + warename + @"\";
                }


				d4sln=xmldoc.SelectSingleNode("//configuration/dssln").InnerText;
                d4csproj = xmldoc.SelectSingleNode("//configuration/dscsproj").InnerText.Replace("{congtrolname}", warename).Replace("{OutputPath}", waredeploy + waredirpath.ToLower() + "\\bin\\release\\").Replace("{FramePath}", framepath).Replace("{LibPath}", libpath);
                d4csproj2 = xmldoc.SelectSingleNode("//configuration/dscsproj2").InnerText.Replace("{congtrolname}", warename).Replace("{OutputPath}", waredeploy + waredirpath.ToLower() + "\\bin\\release\\").Replace("{FramePath}", framepath).Replace("{LibPath}", libpath);


                

				indexfile=indexpath + "index.xml";
				if(!File.Exists(indexfile))
				{
					L("the file not exists:" + indexfile);
					return;
				}
				/*
						L("indexfile is:" + indexfile);
						FileInfo fi = new FileInfo(indexfile);
						fi.OpenRead();
						StreamReader sr=fi.OpenText();
						allstring=sr.ReadToEnd();
						sr.Close();
				*/		L("Indexfile is:" + indexfile);
				File.Copy(indexfile,outputpath2 + @"\" + warename + ".aspx",true);
				XmlDocument aspx=new XmlDocument();
				aspx.Load(outputpath2 + @"\" + warename + ".aspx");
				//XmlDocument temp=new XmlDocument();
				//temp.LoadXml("<params><foreparts></foreparts><backparts></backparts></params>");
				//aspx.LastChild.AppendChild(temp.LastChild);
				//aspx.LastChild.AppendChild(aspx.CreateNode(XmlNodeType.Element,"aaa","bbb"));
				//aspx.LoadXml(aspx.FirstChild.OuterXml + "<configuration>" + aspx.LastChild.InnerXml +"<params><foreparts></foreparts><backparts></backparts></params></configuration>");
				//aspx.Save(wareindex + @"\" + warename + ".aspx");
				xmldoc.Load(indexfile);
				L("Read index.xml ok !");
				string warexml=xmldoc.SelectSingleNode("//configuration/config").OuterXml;
				warename=xmldoc.SelectSingleNode("//configuration/config/control_name").InnerText;
				L("the ControlName from XML File is:" + warename);
				wareversion=xmldoc.SelectSingleNode("//configuration/config/control_version").InnerText;
				L("the ControlVersion from XML File is:" + wareversion);
				control_company=xmldoc.SelectSingleNode("//configuration/config/control_company").InnerText;
				L("the ControlCompany from XML File is:" + control_company);
				control_copyright=xmldoc.SelectSingleNode("//configuration/config/control_copyright").InnerText;
				L("the ControlCopyright from XML File is:" + control_copyright);
				forexml=xmldoc.SelectSingleNode("//configuration/forecsxmlfile").InnerText;
				backxml=xmldoc.SelectSingleNode("//configuration/backcsxmlfile").InnerText;
				sqlxml=xmldoc.SelectSingleNode("//configuration/sqlcsxmlfile").InnerText;
				tempxml=xmldoc.SelectSingleNode("//configuration/tempcsxmlfile").InnerText;
                //other files add 2009-6-17 maobinbin
                string appendfilexml = "";
                if (xmldoc.SelectSingleNode("//configuration/appendfiles") != null)
                {
                    appendfilexml = xmldoc.SelectSingleNode("//configuration/appendfiles").InnerXml;
                }
				//AssemblyInfo start
				string s="";
                s = s + "/*FTDP Control,This Code Created By FTDP Deploy Tool\r\n";
                s = s + "Build By Maobb 2007-6-10\r\n";
                s = s + "Code Deploy Time is:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "*/\r\n";
				s=s+"using System.Reflection;\r\n";
				s=s+"using System.Runtime.CompilerServices;\r\n";
				s=s+"[assembly: AssemblyTitle(\"FT." + warename + "\")]\r\n";
				s=s+"[assembly: AssemblyDescription(\"" + control_company + "." + warename + "\")]\r\n";
				s=s+"[assembly: AssemblyConfiguration(\"\")]\r\n";
				s=s+"[assembly: AssemblyCompany(\"" + control_company + "\")]\r\n";
				s=s+"[assembly: AssemblyProduct(\"" + warename + "\")]\r\n";
                s = s + "[assembly: AssemblyCopyright(\"" + control_copyright + "{Build by FTDP ControlDeploy Tool}\")]\r\n";
				s=s+"[assembly: AssemblyTrademark(\"\")]\r\n";
				s=s+"[assembly: AssemblyCulture(\"\")]\r\n";
				s=s+"[assembly: AssemblyVersion(\"" + wareversion + "\")]\r\n";
				s=s+"[assembly: AssemblyDelaySign(false)]\r\n";
				s=s+"[assembly: AssemblyKeyFile(\"\")]\r\n";
				s=s+"[assembly: AssemblyKeyName(\"\")]\r\n";
				//AssemblyInfo end
                outputpath = waredeploy + waredirpath.ToLower();
				L("control deploy path is:" + outputpath);
				if(!Directory.Exists(outputpath))
				{
					L("control deploy path Directory not found ,created");
					Directory.CreateDirectory(outputpath);
				}
				L("Start Write AssemblyInfo.cs");
				if(File.Exists(outputpath + @"\AssemblyInfo.cs"))
				{
					L("AssemblyInfo.cs Exist,now Delete");
					File.Delete(outputpath + @"\AssemblyInfo.cs");
				}
                StreamWriter sw = new StreamWriter(outputpath + @"\AssemblyInfo.cs", false, NowEncoding);
				//StreamWriter sw=File.CreateText(outputpath + @"\AssemblyInfo.cs");
				//sw.Close();
				L("AssemblyInfo.cs Created");
				//FileInfo fi = new FileInfo(outputpath + @"\AssemblyInfo.cs");
                //sw = new StreamWriter();
				//sw.Write;
				sw.Write(s);
				sw.Flush();
				sw.Close();
				L("AssemblyInfo.cs Write OK !");
				L("End Write AssemblyInfo.cs\r\n");

				L("Start Enter sql.cs");
				xmldoc.Load(indexpath + sqlxml);
				s="";
                s = s + "/*FTDP Control,This Code Created By FTDP Deploy Tool\r\n";
				s=s+"Build By Maobb 2007-6-10\r\n";
                s = s + "Code Deploy Time is:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "*/\r\n";
				s=s+"using System;\r\n";
                s = s + "using FTDP;\r\n";
                s = s + "using FTDP.Tool;\r\n";
                s = s + "using System.Text.RegularExpressions;\r\n";
				s=s+"namespace FT." + warename + "\r\n";
				s=s+"{\r\n";
				s=s+"public class ftdpcontrolsql\r\n";
				s=s+"{\r\n";
				//LoopNode start
				XmlNodeList nodes=xmldoc.SelectNodes("//configuration/sql");
				XmlNode node;
				string h1;
				string h2;
				int i;
				int j;
				string[] type;
				string[] sqlstr;
				for(i=0;i<nodes.Count;i++)
				{
                    node = nodes.Item(i);
				    string[] paras=node.SelectSingleNode("param").InnerText.Trim().Split(',');
                    string DefaultSql=null;
                    if (node.SelectSingleNode("Default") != null)
                        DefaultSql = node.SelectSingleNode("Default").InnerText.Trim();
                    else
                        DefaultSql = node.SelectSingleNode("sqlstr").InnerText.Trim();
                    string caseStr = "";
                    bool HasAnyDBDefind = false;
				    foreach(XmlNode dbnode in node.ChildNodes)
				    {
                        if (!dbnode.Name.Equals("param") && !dbnode.Name.Equals("Default") && !dbnode.Name.Equals("sqlstr"))
				        {
                            HasAnyDBDefind = true;
                            s += "public static string " + node.Attributes.GetNamedItem("name").Value + "_For_" + dbnode.Name + "(" + GetSqlParas(paras) + ")\r\n{\r\n";

                            s = s + "return (\"" + ChangeSql(paras, dbnode.InnerText.Trim().Equals("") ? DefaultSql : dbnode.InnerText.Trim()) + "\").Trim();\r\n}\r\n";
                            caseStr += "case DataBaseType." + dbnode.Name + ": return " + node.Attributes.GetNamedItem("name").Value + "_For_" + dbnode.Name + "(" + node.SelectSingleNode("param").InnerText.Trim() + ");\r\n";
				        }
				    }
                    if (!HasAnyDBDefind)
                    {
                        string[] _type = paras;
                        string _h1 = "";
                        for (int _j = 0; _j < _type.Length; _j++)
                        {
                            if (_type[_j].Equals("s"))
                            {
                                _h1 = _h1 + "string p" + _j;
                            }
                            if (_type[_j].Equals("i"))
                            {
                                _h1 = _h1 + "int p" + _j;
                            }
                            if (_type[_j].Equals("d"))
                            {
                                _h1 = _h1 + "DateTime p" + _j;
                            }
                            if (_type[_j].Equals("o"))
                            {
                                _h1 = _h1 + "object p" + _j;
                            }
                            if (_j < _type.Length - 1)
                            {
                                _h1 = _h1 + ",";
                            }
                        }
                        string[] _sqlstr =DefaultSql.Split('@');
                        string _h2 = "";
                        for (int _j = 0; _j < _sqlstr.Length; _j++)
                        {
                            _h2 = _h2 + "\"" + _sqlstr[_j] + "\"";
                            if (_j < _sqlstr.Length - 1)
                            {
                                _h2 = _h2 + " + p" + _j + " + ";
                            }
                        }
                        _h2 = _h2.Replace("####$$$$", "@");
                        s = s + "public static string " + node.Attributes.GetNamedItem("name").Value + "(" + _h1 + ")\r\n{\r\n";
                        s = s + "return (" + _h2 + ").Trim();\r\n}\r\n";
                        //s += "public static string " + node.Attributes.GetNamedItem("name").Value + "_For_MySql(" + GetSqlParas(paras) + ")\r\n{\r\n";

                        //s = s + "return (\"" + ChangeSql(paras, DefaultSql) + "\").Trim();\r\n}\r\n";
                        //caseStr += "case DataBaseType.MySql: return " + node.Attributes.GetNamedItem("name").Value + "_For_MySql(" + node.SelectSingleNode("param").InnerText.Trim() + ");\r\n";
                    }
                    else
                    {
                        s += "public static string " + node.Attributes.GetNamedItem("name").Value + "(" + GetSqlParas(paras) + ")\r\n{\r\n";
                        s += "switch(FTDP.ConstStr.DBType)\r\n{" + caseStr + "}\r\nreturn \"no this dbtype sql defined on this control!\";}\r\n";
                    }
				}
				//LoopNode end
				s=s+"}\r\n";
				s=s+"}\r\n";
				L("Start Write sql.cs");
				if(File.Exists(outputpath + @"\sql.cs"))
				{
					L("sql.cs Exist,now Delete");
					File.Delete(outputpath + @"\sql.cs");
				}
                sw = new StreamWriter(outputpath + @"\sql.cs", false, NowEncoding);
                //sw=File.CreateText(outputpath + @"\sql.cs");
                //sw.Close();
                //L("sql.cs Created");
                //FileInfo fi;
                //fi = new FileInfo(outputpath + @"\sql.cs");
                //sw = fi.AppendText();
				sw.Write(s);
				sw.Flush();
				sw.Close();
				L("sql.cs Write OK !");
				L("End Write sql.cs");
				L("End  sql.cs\r\n");

				L("Start Enter temp.cs");
				xmldoc.Load(indexpath + tempxml);
				s="";
                s = s + "/*FTDP Control,This Code Created By FTDP Deploy Tool\r\n";
				s=s+"Build By Maobb 2005-7-21\r\n";
                s = s + "Code Deploy Time is:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "*/\r\n";
				s=s+"using System;\r\n";
				s=s+"using FT." + warename + ".Fore;\r\n";
				s=s+"namespace FT." + warename + "\r\n";
				s=s+"{\r\n";
                s = s + "public class ftdpcontroltemp\r\n";
				s=s+"{\r\n";
				s=s+"private static string setStyle(string name)\r\n";
	s=s+"{\r\n";
	s=s+"return setStyleClass(name) + setStyleCssTxt(name);\r\n";
	s=s+"}\r\n";
    s = s + "private static string setStyleClass(string name)\r\n";
	s=s+"{\r\n";
		s=s+"int i;\r\n";
		s=s+"for(i=0;i<client.thisStyleObject.Length;i++)\r\n";
		s=s+"{\r\n";
		s=s+"	if(((string[])(client.thisStyleObject[i]))[0].Equals(name))\r\n";
		s=s+"	{\r\n";
		s=s+"		if(!((string[])(client.thisStyleObject[i]))[1].Equals(\"\"))\r\n";
		s=s+"		{\r\n";
		s=s+"			return \"class=\\\"\" + ((string[])(client.thisStyleObject[i]))[1] + \"\\\" \";\r\n";
		s=s+"		}\r\n";
		s=s+"		return \"\";\r\n";
		s=s+"	}\r\n";
		s=s+"}\r\n";
		s=s+"return \"\";\r\n";
	s=s+"}\r\n";
    s = s + "private static string setStyleCssTxt(string name)\r\n";
	s=s+"{\r\n";
	s=s+"	int i;\r\n";
	s=s+"	for(i=0;i<client.thisStyleObject.Length;i++)\r\n";
	s=s+"	{\r\n";
	s=s+"		if(((string[])(client.thisStyleObject[i]))[0].Equals(name))\r\n";
		s=s+"	{\r\n";
		s=s+"		if(!((string[])(client.thisStyleObject[i]))[2].Equals(\"\"))\r\n";
		s=s+"		{\r\n";
		s=s+"			return \"style=\\\"\" + ((string[])(client.thisStyleObject[i]))[2] + \"\\\" \";\r\n";
		s=s+"		}\r\n";
		s=s+"		return \"\";\r\n";
		s=s+"	}\r\n";
		s=s+"}\r\n";
		s=s+"return \"\";\r\n";
	s=s+"}\r\n";
    s = s + "public static string getStyleClass(string name)\r\n";
    s = s + "{\r\n";
    s = s + "int i;\r\n";
    s = s + "for(i=0;i<client.thisStyleObject.Length;i++)\r\n";
    s = s + "{\r\n";
    s = s + "	if(((string[])(client.thisStyleObject[i]))[0].Equals(name))\r\n";
    s = s + "	{\r\n";
    s = s + "		if(!((string[])(client.thisStyleObject[i]))[1].Equals(\"\"))\r\n";
    s = s + "		{\r\n";
    s = s + "			return ((string[])(client.thisStyleObject[i]))[1];\r\n";
    s = s + "		}\r\n";
    s = s + "		return \"\";\r\n";
    s = s + "	}\r\n";
    s = s + "}\r\n";
    s = s + "return \"\";\r\n";
    s = s + "}\r\n";
    s = s + "public static string getStyleCssTxt(string name)\r\n";
    s = s + "{\r\n";
    s = s + "	int i;\r\n";
    s = s + "	for(i=0;i<client.thisStyleObject.Length;i++)\r\n";
    s = s + "	{\r\n";
    s = s + "		if(((string[])(client.thisStyleObject[i]))[0].Equals(name))\r\n";
    s = s + "	{\r\n";
    s = s + "		if(!((string[])(client.thisStyleObject[i]))[2].Equals(\"\"))\r\n";
    s = s + "		{\r\n";
    s = s + "			return ((string[])(client.thisStyleObject[i]))[2];\r\n";
    s = s + "		}\r\n";
    s = s + "		return \"\";\r\n";
    s = s + "	}\r\n";
    s = s + "}\r\n";
    s = s + "return \"\";\r\n";
    s = s + "}\r\n";
				//LoopNode start
				nodes=xmldoc.SelectNodes("//configuration/temp");
				for(i=0;i<nodes.Count;i++)
				{
					node=nodes.Item(i);
					h1=node.FirstChild.InnerText;
					h2=" " + node.LastChild.InnerText.Replace("\"","\\\"").Replace("#@","####$$$$").Replace("\r\n"," ").Replace("\n"," ") + " ";
					string h="";
					string l="";
					string c=h2;
					while(c.IndexOf("setstyle=\\\"%{")>=0)
					{
						h=c.Substring(c.IndexOf("setstyle=\\\"%{"),c.IndexOf("}%")-c.IndexOf("setstyle=\\\"%{")+3);
						l=h.Substring(h.IndexOf("["),h.Length-h.IndexOf("["));
						l=h.Replace(l,"").Replace("setstyle=\\\"%{","%%%%") + "\") + ";
						c=c.Replace(h,l);
					}
					h2=c;
					type=h1.Split(',');
					h1="";
					for(j=0;j<type.Length;j++)
					{
						if(type[j].Equals("s"))
						{
							h1=h1+"string p" + j;
						}
						if(type[j].Equals("i"))
						{
							h1=h1+"int p" + j;
						}
						if(type[j].Equals("d"))
						{
							h1=h1+"DateTime p" + j;
						}
						if(type[j].Equals("o"))
						{
							h1=h1+"object p" + j;
						}
						if(j<type.Length-1)
						{
							h1=h1 + ",";
						}
					}
					L(node.Attributes.GetNamedItem("name").Value + ":" + h1);
					sqlstr=h2.Split('@');
					h2="";
					for(j=0;j<sqlstr.Length;j++)
					{
						h2=h2+"\"" + sqlstr[j] + "\"";
						h2=h2.Replace("%%%%","\" + setStyle(\"");
						if(j<sqlstr.Length-1)
						{
							h2=h2+" + p" + j + " + ";
						}
					}
					h2=h2.Replace("####$$$$","@");
					L("Temp:" + node.Attributes.GetNamedItem("name").Value + " OK!");
					s=s+"public static string " + node.Attributes.GetNamedItem("name").Value + "(" + h1 + ")\r\n{\r\n";
					s=s+"return (" + h2 + ").Trim();\r\n}\r\n";
				}
				//LoopNode end
				s=s+"}\r\n";
				s=s+"}\r\n";
				L("Start Write temp.cs");
				if(File.Exists(outputpath + @"\temp.cs"))
				{
					L("temp.cs Exist,now Delete");
					File.Delete(outputpath + @"\temp.cs");
				}
                sw = new StreamWriter(outputpath + @"\temp.cs", false, NowEncoding);
                //sw=File.CreateText(outputpath + @"\temp.cs");
                //sw.Close();
                //L("temp.cs Created");
                //fi = new FileInfo(outputpath + @"\temp.cs");
                //sw = fi.AppendText();
				sw.Write(s);
				sw.Flush();
				sw.Close();
				L("temp.cs Write OK !");
				L("End Write temp.cs");
				L("End  temp.cs\r\n");

				L("Start Enter clint_temp.cs");
				xmldoc.Load(indexpath + tempxml);
				s="";
                s = s + "/*FTDP Control,This Code Created By FTDP Deploy Tool\r\n";
                s = s + "Build By Maobb 2007-6-10\r\n";
                s = s + "Code Deploy Time is:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "*/\r\n";
				s=s+"using System;\r\n";
				s=s+"namespace ft_" + warename.ToLower() + "\r\n";
				s=s+"{\r\n";
                s = s + "public class ftdpcontroltemp\r\n";
				s=s+"{\r\n";
				s=s+"private static string setStyle(string name)\r\n";
				s=s+"{\r\n";
				s=s+"return setStyleClass(name) + setStyleCssTxt(name);\r\n";
				s=s+"}\r\n";
                s = s + "private static string setStyleClass(string name)\r\n";
				s=s+"{\r\n";
				s=s+"int i;\r\n";
				s=s+"for(i=0;i<client.thisStyleObject.Length;i++)\r\n";
				s=s+"{\r\n";
				s=s+"	if(((string[])(client.thisStyleObject[i]))[0].Equals(name))\r\n";
				s=s+"	{\r\n";
				s=s+"		if(!((string[])(client.thisStyleObject[i]))[1].Equals(\"\"))\r\n";
				s=s+"		{\r\n";
				s=s+"			return \"class=\\\"\" + ((string[])(client.thisStyleObject[i]))[1] + \"\\\" \";\r\n";
				s=s+"		}\r\n";
				s=s+"		return \"\";\r\n";
				s=s+"	}\r\n";
				s=s+"}\r\n";
				s=s+"return \"\";\r\n";
				s=s+"}\r\n";
                s = s + "private static string setStyleCssTxt(string name)\r\n";
				s=s+"{\r\n";
				s=s+"	int i;\r\n";
				s=s+"	for(i=0;i<client.thisStyleObject.Length;i++)\r\n";
				s=s+"	{\r\n";
				s=s+"		if(((string[])(client.thisStyleObject[i]))[0].Equals(name))\r\n";
				s=s+"	{\r\n";
				s=s+"		if(!((string[])(client.thisStyleObject[i]))[2].Equals(\"\"))\r\n";
				s=s+"		{\r\n";
				s=s+"			return \"style=\\\"\" + ((string[])(client.thisStyleObject[i]))[2] + \"\\\" \";\r\n";
				s=s+"		}\r\n";
				s=s+"		return \"\";\r\n";
				s=s+"	}\r\n";
				s=s+"}\r\n";
				s=s+"return \"\";\r\n";
				s=s+"}\r\n";
                s = s + "public static string getStyleClass(string name)\r\n";
                s = s + "{\r\n";
                s = s + "int i;\r\n";
                s = s + "for(i=0;i<client.thisStyleObject.Length;i++)\r\n";
                s = s + "{\r\n";
                s = s + "	if(((string[])(client.thisStyleObject[i]))[0].Equals(name))\r\n";
                s = s + "	{\r\n";
                s = s + "		if(!((string[])(client.thisStyleObject[i]))[1].Equals(\"\"))\r\n";
                s = s + "		{\r\n";
                s = s + "			return ((string[])(client.thisStyleObject[i]))[1];\r\n";
                s = s + "		}\r\n";
                s = s + "		return \"\";\r\n";
                s = s + "	}\r\n";
                s = s + "}\r\n";
                s = s + "return \"\";\r\n";
                s = s + "}\r\n";
                s = s + "public static string getStyleCssTxt(string name)\r\n";
                s = s + "{\r\n";
                s = s + "	int i;\r\n";
                s = s + "	for(i=0;i<client.thisStyleObject.Length;i++)\r\n";
                s = s + "	{\r\n";
                s = s + "		if(((string[])(client.thisStyleObject[i]))[0].Equals(name))\r\n";
                s = s + "	{\r\n";
                s = s + "		if(!((string[])(client.thisStyleObject[i]))[2].Equals(\"\"))\r\n";
                s = s + "		{\r\n";
                s = s + "			return ((string[])(client.thisStyleObject[i]))[2];\r\n";
                s = s + "		}\r\n";
                s = s + "		return \"\";\r\n";
                s = s + "	}\r\n";
                s = s + "}\r\n";
                s = s + "return \"\";\r\n";
                s = s + "}\r\n";
				//LoopNode start
				nodes=xmldoc.SelectNodes("//configuration/temp[@type='fore']");
				for(i=0;i<nodes.Count;i++)
				{
					node=nodes.Item(i);
					h1=node.FirstChild.InnerText;
					h2=" " + node.LastChild.InnerText.Replace("\"","\\\"").Replace("#@","####$$$$").Replace("\r\n"," ").Replace("\n"," ") + " ";
					string h="";
					string l="";
					string c=h2;
					while(c.IndexOf("setstyle=\\\"%{")>=0)
					{
						
						h=c.Substring(c.IndexOf("setstyle=\\\"%{"),c.IndexOf("}%")-c.IndexOf("setstyle=\\\"%{")+3);
						//string hss=h.Replace("setstyle=\\\"%{","alice_").Replace("}%","").Replace("\\","");
						l=h.Substring(h.IndexOf("["),h.Length-h.IndexOf("["));
						//l=h.Replace(l,"").Replace("setstyle=\\\"%{","styleset=\\\"" + hss + "\\\" %%%%") + "\") + ";
						l=h.Replace(l,"").Replace("setstyle=\\\"%{","%%%%") + "\") + ";
						c=c.Replace(h,l);
					}
					h2=c;
					type=h1.Split(',');
					h1="";
					for(j=0;j<type.Length;j++)
					{
						if(type[j].Equals("s"))
						{
							h1=h1+"string p" + j;
						}
						if(type[j].Equals("i"))
						{
							h1=h1+"int p" + j;
						}
						if(type[j].Equals("d"))
						{
							h1=h1+"DateTime p" + j;
						}
						if(type[j].Equals("o"))
						{
							h1=h1+"object p" + j;
						}
						if(j<type.Length-1)
						{
							h1=h1 + ",";
						}
					}
					L(node.Attributes.GetNamedItem("name").Value + ":" + h1);
					sqlstr=h2.Split('@');
					h2="";
					for(j=0;j<sqlstr.Length;j++)
					{
						h2=h2+"\"" + sqlstr[j] + "\"";
						h2=h2.Replace("%%%%","\" + setStyle(\"");
						if(j<sqlstr.Length-1)
						{
							h2=h2+" + p" + j + " + ";
						}
					}
					h2=h2.Replace("####$$$$","@");
					L("clint_Temp:" + node.Attributes.GetNamedItem("name").Value + " OK!");
                    s = s + "public static string " + node.Attributes.GetNamedItem("name").Value + "(" + h1 + ")\r\n{\r\n";
					//setstyle
					s=s+"return (" + h2 + ").Trim();\r\n}\r\n";
				}
				//LoopNode end
				s=s+"}\r\n";
				s=s+"}\r\n";
				L("Start Write clint_temp.cs");
				if(File.Exists(outputpath + @"\clint_temp.cs"))
				{
					L("clint_temp.cs Exist,now Delete");
					File.Delete(outputpath + @"\clint_temp.cs");
				}
                sw = new StreamWriter(outputpath + @"\clint_temp.cs", false, NowEncoding);
                //sw=File.CreateText(outputpath + @"\clint_temp.cs");
                //sw.Close();
                //L("clint_temp.cs Created");
                //fi = new FileInfo(outputpath + @"\clint_temp.cs");
                //sw = fi.AppendText();
				sw.Write(s);
				sw.Flush();
				sw.Close();
				L("clint_temp.cs Write OK !");
				L("End Write clint_temp.cs");
				L("End  clint_temp.cs\r\n");

				L("Start Enter back.cs");
				s="";
                s = s + "/*FTDP Control,This Code Created By FTDP Deploy Tool\r\n";
                s = s + "Build By Maobb 2007-6-10\r\n";
                s = s + "Code Deploy Time is:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "*/\r\n";
				s=s+"using System;\r\n";
				s=s+"using System.Web;\r\n";
				s=s+"using System.Web.UI;\r\n";
				s=s+"using System.Web.UI.WebControls;\r\n";
				s=s+"using System.Data;\r\n";
                s = s + "using System.Collections;\r\n";
				s=s+"using System.Xml;\r\n";
                s = s + "using FTDP;\r\n";
                s = s + "using FTDP.Tool;\r\n";
                s = s + "using FTDP.DBClient;\r\n";
                s = s + "using FTDP.Page;\r\n";
                s = s + "using System.Text.RegularExpressions;\r\n";
				s=s+"namespace FT." + warename + ".Back\r\n";
				s=s+"{\r\n";
				xmldoc.Load(indexpath + backxml);
				nodes=xmldoc.SelectNodes("//configuration/part");
				string forpara="";
				forpara+="<params><backparts>";
				for(i=0;i<nodes.Count;i++)
				{
                    node = nodes.Item(i);
                    if (node.Attributes.GetNamedItem("listpage") != null && node.Attributes.GetNamedItem("listpage").Value.Equals("true"))
                    {
                        forpara += node.OuterXml;
                    }
                    else
                    {
                        forpara += "<part name=\"" + node.Attributes.GetNamedItem("name").Value + "\">";
                        L("Start Convert to C# Back Code:Part=" + node.Attributes.GetNamedItem("name").Value);
                        s = s + "public class " + node.Attributes.GetNamedItem("name").Value + " : Control, INamingContainer\r\n";
                        s = s + "{\r\n";
                        s = s + "public string SiteID;\r\n";
                        s = s + "public string ControlName;\r\n";
                        s = s + "public string ControlID;\r\n";
                        //s = s + "public string PartID;\r\n";
                        s = s + "public string DataSource;\r\n";
                        s = s + "public string SetStyle;\r\n";
                        s = s + "public string ControlPara;\r\n";
                        forpara += node.SelectSingleNode("public_params").InnerXml;
                        forpara += "</part>";
                        //XmlNodeList podes=node.SelectNodes("//part[@name='" + node.Attributes.GetNamedItem("name").Value + "']/public_params/param");
                        XmlNodeList podes = node.SelectNodes("public_params/param");
                        //XmlNode x=new XmlNode();
                        //aspx.LastChild.AppendChild();
                        XmlNode pode;
                        for (j = 0; j < podes.Count; j++)
                        {
                            pode = podes.Item(j);
                            s = s + "public " + pode.SelectSingleNode("type").InnerText + " " + pode.SelectSingleNode("name").InnerText + ";\r\n";
                        }
                        s = s + "protected override void Render(HtmlTextWriter output)\r\n";
                        s = s + "{\r\n";
                        s = s + "ControlPara=ControlPara.Replace(\"{dsqt}\",\"\\\"\");\r\n";
                        for (j = 0; j < podes.Count; j++)
                        {
                            pode = podes.Item(j);
                            if (pode.SelectSingleNode("type").InnerText.Trim().ToLower().Equals("string"))
                            {
                                s = s + pode.SelectSingleNode("name").InnerText + "=" + pode.SelectSingleNode("name").InnerText + ".Replace(\"{dsqt}\",\"\\\"\");\r\n";
                            }
                        }
                        CSCode = "";
                        X(node.SelectSingleNode("//part[@name='" + node.Attributes.GetNamedItem("name").Value + "']/logic_code"));
                        s = s + CSCode;
                        s = s + "}\r\n";
                        s = s + "}\r\n";
                    }
				}
				forpara+="</backparts><foreparts>";
				s=s+"}\r\n";
				L("End Convert to C# Code");
				L("Start Write back.cs");
				if(File.Exists(outputpath + @"\back.cs"))
				{
					L("back.cs Exist,now Delete");
					File.Delete(outputpath + @"\back.cs");
				}
                sw = new StreamWriter(outputpath + @"\back.cs", false, NowEncoding);
                //sw=File.CreateText(outputpath + @"\back.cs");
                //sw.Close();
                //L("back.cs Created");
                //fi = new FileInfo(outputpath + @"\back.cs");
                //sw = fi.AppendText();
				sw.Write(s);
				sw.Flush();
				sw.Close();
				L("back.cs Write OK !");
				L("End Write back.cs");
				L("End  back.cs\r\n");

				L("Start Enter Fore.cs");
				s="";
                s = s + "/*FTDP Control,This Code Created By FTDP Deploy Tool\r\n";
                s = s + "Build By Maobb 2007-6-10\r\n";
                s = s + "Code Deploy Time is:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "*/\r\n";
				s=s+"using System;\r\n";
				s=s+"using System.Web;\r\n";
				s=s+"using System.Web.UI;\r\n";
				s=s+"using System.Web.UI.WebControls;\r\n";
				s=s+"using System.Data;\r\n";
				s=s+"using System.Xml;\r\n";
                s = s + "using System.Collections;\r\n";
                s = s + "using FTDP;\r\n";
                s = s + "using FTDP.Tool;\r\n";
                s = s + "using FTDP.DBClient;\r\n";
                s = s + "using FTDP.Page;\r\n";
                s = s + "using System.Text.RegularExpressions;\r\n";
				s=s+"namespace FT." + warename + ".Fore\r\n";
				s=s+"{\r\n";
				s=s+"public class client\r\n";
				s=s+"{\r\n";
				s=s+"public static string thisControlID=\"\";\r\n";
				s=s+"public static object[] thisStyleObject;\r\n";
				s=s+"}\r\n";
				xmldoc.Load(indexpath + forexml);
				nodes=xmldoc.SelectNodes("//configuration/part");
				for(i=0;i<nodes.Count;i++)
				{
					node=nodes.Item(i);
					forpara+="<part name=\"" + node.Attributes.GetNamedItem("name").Value + "\">";
					L("Start Convert to C# Fore Code:Part=" + node.Attributes.GetNamedItem("name").Value);
					s=s+"public class " + node.Attributes.GetNamedItem("name").Value + " : Control, INamingContainer\r\n";
					s=s+"{\r\n";
					s=s+"public string SiteID;\r\n";
					s=s+"public string ControlName;\r\n";
					s=s+"public string ControlID;\r\n";
                    s = s + "public string PartID;\r\n";
					s=s+"public string DataSource;\r\n";
					s=s+"public string SetStyle;\r\n";
                    s = s + "public string ControlPara;\r\n";
					//XmlNodeList podes=node.SelectNodes("//part[@name='" + node.Attributes.GetNamedItem("name").Value + "']/public_params/param");
					XmlNodeList podes=node.SelectNodes("public_params/param");
					XmlNode pode;
					for(j=0;j<podes.Count;j++)
					{
						pode=podes.Item(j);
						s=s+"public " + pode.SelectSingleNode("type").InnerText + " " + pode.SelectSingleNode("name").InnerText + ";\r\n";
						if(pode.SelectSingleNode("class").InnerText.StartsWith("system"))
						{
						forpara+=pode.OuterXml;
						}
					}
					forpara+="</part>";
					s=s+"protected override void Render(HtmlTextWriter output)\r\n";
					s=s+"{\r\n";
                    s = s + "ControlPara=ControlPara.Replace(\"{dsqt}\",\"\\\"\");\r\n";
					for(j=0;j<podes.Count;j++)
					{
						pode=podes.Item(j);
						if(pode.SelectSingleNode("type").InnerText.Trim().ToLower().Equals("string"))
						{
						s=s+ pode.SelectSingleNode("name").InnerText + "=" + pode.SelectSingleNode("name").InnerText + ".Replace(\"{dsqt}\",\"\\\"\");\r\n";
						}
					}
					s=s+"client.thisControlID=ControlID;\r\n";
					s=s+"string[] StyleStringArray=SetStyle.Split('{');\r\n";
					s=s+"int StyleStringArrayi;\r\n";
					s=s+"int StyleStringArrayLength=StyleStringArray.Length;\r\n";
					s=s+"client.thisStyleObject=new object[StyleStringArrayLength];\r\n";
					s=s+"for(StyleStringArrayi=0;StyleStringArrayi<StyleStringArrayLength;StyleStringArrayi++)\r\n";
					s=s+"{\r\n";
					s=s+"	string[] StyleStringArrayOne=StyleStringArray[StyleStringArrayi].Split('}');\r\n";
					s=s+"	client.thisStyleObject[StyleStringArrayi]=StyleStringArrayOne;\r\n";
					s=s+"}\r\n";
					CSCode="";
					X(node.SelectSingleNode("//part[@name='" + node.Attributes.GetNamedItem("name").Value + "']/logic_code"));
					s=s+CSCode;
					s=s+"}\r\n";
					s=s+"}\r\n";
				}
				forpara+="</foreparts></params>";
				L("Control index xml file start writing......");
				aspx.LoadXml(aspx.FirstChild.OuterXml + "<configuration>" + aspx.LastChild.InnerXml + forpara.Replace("param>","para>") + "</configuration>");
				aspx.Save(outputpath2 + @"\" + warename + ".dll");
				s=s+"}\r\n";
				L("End Convert to C# Code");
				L("Start Write fore.cs");
				if(File.Exists(outputpath + @"\fore.cs"))
				{
					L("fore.cs Exist,now Delete");
					File.Delete(outputpath + @"\fore.cs");
				}
                sw = new StreamWriter(outputpath + @"\fore.cs", false, NowEncoding);
                //sw=File.CreateText(outputpath + @"\fore.cs");
                //sw.Close();
                //L("fore.cs Created");
                //fi = new FileInfo(outputpath + @"\fore.cs");
                //sw = fi.AppendText();
				sw.Write(s);
				sw.Flush();
				sw.Close();
				L("fore.cs Write OK !");
				L("End Write fore.cs");
				L("End  fore.cs\r\n");

				L("Start Enter clint_fore.cs");
				s="";
                s = s + "/*FTDP Control,This Code Created By FTDP Deploy Tool\r\n";
                s = s + "Build By Maobb 2007-6-10\r\n";
                s = s + "Code Deploy Time is:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "*/\r\n";
				s=s+"using System;\r\n";
				s=s+"using System.Web;\r\n";
				s=s+"using System.Web.UI;\r\n";
				s=s+"using System.Web.UI.WebControls;\r\n";
				s=s+"using System.Data;\r\n";
                s = s + "using System.Collections;\r\n";
				s=s+"namespace ft_" + warename.ToLower() + "\r\n";
				s=s+"{\r\n";
				s=s+"public class client\r\n";
				s=s+"{\r\n";
				s=s+"public static string thisControlID=\"\";\r\n";
				s=s+"public static object[] thisStyleObject;\r\n";
				s=s+"public string ControlXML()\r\n";
				s=s+"{\r\n";
				s=s+"return \"" + warexml.Replace("\"","\\\"").Replace("\r\n","") + "\";\r\n";
				s=s+"}\r\n";
				s=s+"public string DeployTime()\r\n";
				s=s+"{\r\n";
                s = s + "return \"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "\";\r\n";
				s=s+"}\r\n";
				s=s+"}\r\n";
				xmldoc.Load(indexpath + forexml);
				nodes=xmldoc.SelectNodes("//configuration/part");
				for(i=0;i<nodes.Count;i++)
				{
					node=nodes.Item(i);
					L("Start Convert to C# Fore_Clint Code:Part=" + node.Attributes.GetNamedItem("name").Value);
					s=s+"public class " + node.Attributes.GetNamedItem("name").Value + " : Control, INamingContainer\r\n";
					s=s+"{\r\n";
					CSCode="";
					forcssstr="<styles>";
					forcss=true;
					X(node.SelectSingleNode("//part[@name='" + node.Attributes.GetNamedItem("name").Value + "']/client_show"));
					forcss=false;
					forcssstr+="</styles>";
					string xmla="<partxml>" + node.SelectSingleNode("//part[@name='" + node.Attributes.GetNamedItem("name").Value + "']/public_params").OuterXml.Replace("\"","\\\"").Replace("\r\r\n","") + forcssstr + "</partxml>";
					string xml0="";
					string xml1="";
					string xml2="";
					string xml3="";
					string xml4="";
					string xml5="";
					string xml6="";
					string xml7="";
					string xml8="";
					string xml9="";
                    string xml10 = "";
                    string xml11 = "";
                    string xml12 = "";
                    string xml13 = "";
                    string xml14 = "";
                    string xml15 = "";
                    string xml16 = "";
                    string xml17 = "";
                    string xml18 = "";
                    string xml19 = "";
					if(xmla.Length>1800)
					{
						xml0=xmla.Substring(0,1800);
					}
					else
					{
						xml0=xmla.Substring(0);
					}
					if(xmla.Length>1800*2)
					{
						xml1=xmla.Substring(1800,1800);
					}
					else
					{
						if(xmla.Length>1800)
						{
							xml1=xmla.Substring(1800);
						}
					}
					if(xmla.Length>1800*3)
					{
						xml2=xmla.Substring(1800*2,1800);
					}
					else
					{
						if(xmla.Length>1800*2)
						{
							xml2=xmla.Substring(1800*2);
						}
					}
					if(xmla.Length>1800*4)
					{
						xml3=xmla.Substring(1800*3,1800);
					}
					else
					{
						if(xmla.Length>1800*3)
						{
							xml3=xmla.Substring(1800*3);
						}
					}
					if(xmla.Length>1800*5)
					{
						xml4=xmla.Substring(1800*4,1800);
					}
					else
					{
						if(xmla.Length>1800*4)
						{
							xml4=xmla.Substring(1800*4);
						}
					}
					if(xmla.Length>1800*6)
					{
						xml5=xmla.Substring(1800*5,1800);
					}
					else
					{
						if(xmla.Length>1800*5)
						{
							xml5=xmla.Substring(1800*5);
						}
					}
					if(xmla.Length>1800*7)
					{
						xml6=xmla.Substring(1800*6,1800);
					}
					else
					{
						if(xmla.Length>1800*6)
						{
							xml6=xmla.Substring(1800*6);
						}
					}
					if(xmla.Length>1800*8)
					{
						xml7=xmla.Substring(1800*7,1800);
					}
					else
					{
						if(xmla.Length>1800*7)
						{
							xml7=xmla.Substring(1800*7);
						}
					}
					if(xmla.Length>1800*9)
					{
						xml8=xmla.Substring(1800*8,1800);
					}
					else
					{
						if(xmla.Length>1800*8)
						{
							xml8=xmla.Substring(1800*8);
						}
					}
					if(xmla.Length>1800*10)
					{
						xml9=xmla.Substring(1800*9,1800);
					}
					else
					{
						if(xmla.Length>1800*9)
						{
							xml9=xmla.Substring(1800*9);
						}
					}
                    if (xmla.Length > 1800 * 11)
                    {
                        xml10 = xmla.Substring(1800 * 10, 1800);
                    }
                    else
                    {
                        if (xmla.Length > 1800 * 10)
                        {
                            xml10 = xmla.Substring(1800 * 10);
                        }
                    }
                    if (xmla.Length > 1800 * 12)
                    {
                        xml11 = xmla.Substring(1800 * 11, 1800);
                    }
                    else
                    {
                        if (xmla.Length > 1800 * 11)
                        {
                            xml11 = xmla.Substring(1800 * 11);
                        }
                    }
                    if (xmla.Length > 1800 * 13)
                    {
                        xml12 = xmla.Substring(1800 * 12, 1800);
                    }
                    else
                    {
                        if (xmla.Length > 1800 * 12)
                        {
                            xml12 = xmla.Substring(1800 * 12);
                        }
                    }
                    if (xmla.Length > 1800 * 14)
                    {
                        xml13 = xmla.Substring(1800 * 13, 1800);
                    }
                    else
                    {
                        if (xmla.Length > 1800 * 13)
                        {
                            xml13 = xmla.Substring(1800 * 13);
                        }
                    }
                    if (xmla.Length > 1800 * 15)
                    {
                        xml14 = xmla.Substring(1800 * 14, 1800);
                    }
                    else
                    {
                        if (xmla.Length > 1800 * 14)
                        {
                            xml14 = xmla.Substring(1800 * 14);
                        }
                    }
                    if (xmla.Length > 1800 * 16)
                    {
                        xml15 = xmla.Substring(1800 * 15, 1800);
                    }
                    else
                    {
                        if (xmla.Length > 1800 * 15)
                        {
                            xml15 = xmla.Substring(1800 * 15);
                        }
                    }
                    if (xmla.Length > 1800 * 17)
                    {
                        xml16 = xmla.Substring(1800 * 16, 1800);
                    }
                    else
                    {
                        if (xmla.Length > 1800 * 16)
                        {
                            xml16 = xmla.Substring(1800 * 16);
                        }
                    }
                    if (xmla.Length > 1800 * 18)
                    {
                        xml17 = xmla.Substring(1800 * 17, 1800);
                    }
                    else
                    {
                        if (xmla.Length > 1800 * 17)
                        {
                            xml17 = xmla.Substring(1800 * 17);
                        }
                    }
                    if (xmla.Length > 1800 * 19)
                    {
                        xml18 = xmla.Substring(1800 * 18, 1800);
                    }
                    else
                    {
                        if (xmla.Length > 1800 * 18)
                        {
                            xml18 = xmla.Substring(1800 * 18);
                        }
                    }
                    if (xmla.Length > 1800 * 20)
                    {
                        xml19 = xmla.Substring(1800 * 19, 1800);
                    }
                    else
                    {
                        if (xmla.Length > 1800 * 19)
                        {
                            xml19 = xmla.Substring(1800 * 19);
                        }
                    }
					s=s+"public static string PartXML0=" + adaperQut(xml0) + ";\r\n";
					s=s+"public static string PartXML1=" + adaperQut(xml1) + ";\r\n";
					s=s+"public static string PartXML2=" + adaperQut(xml2) + ";\r\n";
					s=s+"public static string PartXML3=" + adaperQut(xml3) + ";\r\n";
					s=s+"public static string PartXML4=" + adaperQut(xml4) + ";\r\n";
					s=s+"public static string PartXML5=" + adaperQut(xml5) + ";\r\n";
					s=s+"public static string PartXML6=" + adaperQut(xml6) + ";\r\n";
					s=s+"public static string PartXML7=" + adaperQut(xml7) + ";\r\n";
					s=s+"public static string PartXML8=" + adaperQut(xml8) + ";\r\n";
					s=s+"public static string PartXML9=" + adaperQut(xml9) + ";\r\n";
                    s = s + "public static string PartXML10=" + adaperQut(xml10) + ";\r\n";
                    s = s + "public static string PartXML11=" + adaperQut(xml11) + ";\r\n";
                    s = s + "public static string PartXML12=" + adaperQut(xml12) + ";\r\n";
                    s = s + "public static string PartXML13=" + adaperQut(xml13) + ";\r\n";
                    s = s + "public static string PartXML14=" + adaperQut(xml14) + ";\r\n";
                    s = s + "public static string PartXML15=" + adaperQut(xml15) + ";\r\n";
                    s = s + "public static string PartXML16=" + adaperQut(xml16) + ";\r\n";
                    s = s + "public static string PartXML17=" + adaperQut(xml17) + ";\r\n";
                    s = s + "public static string PartXML18=" + adaperQut(xml18) + ";\r\n";
                    s = s + "public static string PartXML19=" + adaperQut(xml19) + ";\r\n";
                    s = s + "public static string PartXML=PartXML0+PartXML1+PartXML2+PartXML3+PartXML4+PartXML5+PartXML6+PartXML7+PartXML8+PartXML9+PartXML10+PartXML11+PartXML12+PartXML13+PartXML14+PartXML15+PartXML16+PartXML17+PartXML18+PartXML19;\r\n";
					s=s+"public string getPartXml()\r\n";
					s=s+"{\r\n";
					s=s+"return PartXML;\r\n";
					s=s+"}\r\n";
					XmlNodeList podes=node.SelectNodes("public_params/param");
					XmlNode pode;
					s+="public string getHtml(string theControlID,string SetStyle,";
					for(j=0;j<podes.Count;j++)
					{
						pode=podes.Item(j);
						if(!pode.SelectSingleNode("class").InnerText.StartsWith("system"))
						{
							s=s+pode.SelectSingleNode("type").InnerText + " " + pode.SelectSingleNode("name").InnerText + ",";
						}
					}
					if(s.EndsWith(",")){
					s=s.Substring(0,s.Length-1);
					}
					s+=")\r\n";
					s=s+"{\r\n";
					s=s+"client.thisControlID=theControlID;\r\n";
					s=s+"string[] StyleStringArray=SetStyle.Split('{');\r\n";
					s=s+"int StyleStringArrayi;\r\n";
					s=s+"int StyleStringArrayLength=StyleStringArray.Length;\r\n";
					s=s+"client.thisStyleObject=new object[StyleStringArrayLength];\r\n";
					s=s+"for(StyleStringArrayi=0;StyleStringArrayi<StyleStringArrayLength;StyleStringArrayi++)\r\n";
					s=s+"{\r\n";
					s=s+"	string[] StyleStringArrayOne=StyleStringArray[StyleStringArrayi].Split('}');\r\n";
					s=s+"	client.thisStyleObject[StyleStringArrayi]=StyleStringArrayOne;\r\n";
					s=s+"}\r\n";
					CSCode="";
					X(node.SelectSingleNode("//part[@name='" + node.Attributes.GetNamedItem("name").Value + "']/client_show"));
					s=s+CSCode;
					s=s+"}\r\n";
					s=s+"}\r\n";
				}
				s=s+"}\r\n";
				L("End Convert to C# Code");
				L("Start Write clint_fore.cs");
				if(File.Exists(outputpath + @"\clint_fore.cs"))
				{
					L("fore.cs Exist,now Delete");
					File.Delete(outputpath + @"\clint_fore.cs");
				}
                sw = new StreamWriter(outputpath + @"\clint_fore.cs", false, NowEncoding);
                //sw=File.CreateText(outputpath + @"\clint_fore.cs");
                //sw.Close();
                //L("clint_fore.cs Created");
                //fi = new FileInfo(outputpath + @"\clint_fore.cs");
                //sw = fi.AppendText();
				sw.Write(s);
				sw.Flush();
				sw.Close();
				L("clint_fore.cs Write OK !");
				L("End Write clint_fore.cs");
				L("End  clint_fore.cs\r\n");

				L("Start Write ds.sln");
				if(File.Exists(outputpath + @"\ds.sln"))
				{
					L("ds.sln Exist,now Delete");
					File.Delete(outputpath + @"\ds.sln");
				}
                sw = new StreamWriter(outputpath + @"\ds.sln", false, NowEncoding);
                //sw=File.CreateText(outputpath + @"\ds.sln");
                //sw.Close();
                //L("ds.sln Created");
                //fi = new FileInfo(outputpath + @"\ds.sln");
                //sw = fi.AppendText();
				s="";
                s = s + "Microsoft Visual Studio Solution File, Format Version 11.00\r\n";
                s += "# Visual Studio 2010\r\n";
				s=s+"Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"ds\", \"ds.csproj\", \"{9DE6000B-A0D2-455A-A5BC-AF8DE33D5893}\"\r\n";
				//s=s+"ProjectSection(ProjectDependencies) = postProject\r\n";
				//s=s+"EndProjectSection\r\n";
				s=s+"EndProject\r\n";
				s=s+d4sln;
				sw.Write(s);
				sw.Flush();
				sw.Close();
				L("ds.sln Write OK !");
				L("End Write ds.sln");

				L("Start Write clint_ds.sln");
				if(File.Exists(outputpath + @"\clint_ds.sln"))
				{
					L("clint_ds.sln Exist,now Delete");
					File.Delete(outputpath + @"\clint_ds.sln");
				}
                sw = new StreamWriter(outputpath + @"\clint_ds.sln", false, NowEncoding);
                //sw=File.CreateText(outputpath + @"\clint_ds.sln");
                //sw.Close();
                //L("clint_ds.sln Created");
                //fi = new FileInfo(outputpath + @"\clint_ds.sln");
                //sw = fi.AppendText();
				s="";
                s = s + "Microsoft Visual Studio Solution File, Format Version 11.00\r\n";
                s += "# Visual Studio 2010\r\n";
				s=s+"Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"clint_ds\", \"clint_ds.csproj\", \"{9DE6000B-A0D2-455A-A5BC-AF8DE33D5893}\"\r\n";
				//s=s+"ProjectSection(ProjectDependencies) = postProject\r\n";
				//s=s+"EndProjectSection\r\n";
				s=s+"EndProject\r\n";
				s=s+d4sln;
				sw.Write(s);
				sw.Flush();
				sw.Close();
				L("clint_ds.sln Write OK !");
				L("End Write clint_ds.sln");

				L("Start Write ds.csproj");
				if(File.Exists(outputpath + @"\ds.csproj"))
				{
					L("ds.sln Exist,now Delete");
					File.Delete(outputpath + @"\ds.csproj");
				}
                sw = new StreamWriter(outputpath + @"\ds.csproj", false, NowEncoding);
                //sw=File.CreateText(outputpath + @"\ds.csproj");
                //sw.Close();
                //L("ds.sln Created");
                //fi = new FileInfo(outputpath + @"\ds.csproj");
                //sw = fi.AppendText();
                //s="";
                //s=s+"<VisualStudioProject>\r\n";
                //s=s+"<CSHARP\r\n";
                //s=s+"ProjectType = \"Local\"\r\n";
                //s=s+"ProductVersion = \"7.10.3077\"\r\n";
                //s=s+"SchemaVersion = \"2.0\"\r\n";
                //s=s+"ProjectGuid = \"{9DE6000B-A0D2-455A-A5BC-AF8DE33D5893}\"\r\n";
                //s=s+">\r\n";
                //s=s+"<Build>\r\n";
                //s=s+"<Settings\r\n";
                //s=s+"ApplicationIcon = \"\"\r\n";
                //s=s+"AssemblyKeyContainerName = \"\"\r\n";
                //s=s+"AssemblyName = \"DS." + warename + "\"\r\n";
                //s=s+"AssemblyOriginatorKeyFile = \"\"\r\n";
                //s=s+"DefaultClientScript = \"JScript\"\r\n";
                //s=s+"DefaultHTMLPageLayout = \"Grid\"\r\n";
                //s=s+"DefaultTargetSchema = \"IE50\"\r\n";
                //s=s+"DelaySign = \"false\"\r\n";
                //s=s+"OutputType = \"Library\"\r\n";
                //s=s+"PreBuildEvent = \"\"\r\n";
                //s=s+"PostBuildEvent = \"\"\r\n";
                //s=s+"RootNamespace = \"DS." + warename + "\"\r\n";
                //s=s+"RunPostBuildEvent = \"OnBuildSuccess\"\r\n";
                //s=s+"StartupObject = \"\"\r\n";
                //s=s+">\r\n";
                //add by maobb 2009-6-17
                d4csproj = d4csproj.Replace("{AppendFiles}", appendfilexml);
				s=d4csproj;
				sw.Write(s);
				sw.Flush();
				sw.Close();
				L("ds.csproj Write OK !");
				L("End Write ds.csproj");

				L("Start Write clint_ds.csproj");
				if(File.Exists(outputpath + @"\clint_ds.csproj"))
				{
					L("clint_ds.sln Exist,now Delete");
					File.Delete(outputpath + @"\clint_ds.csproj");
				}
                sw = new StreamWriter(outputpath + @"\clint_ds.csproj", false, NowEncoding);
                //sw=File.CreateText(outputpath + @"\clint_ds.csproj");
                //sw.Close();
                //L("clint_ds.csproj Created");
                //fi = new FileInfo(outputpath + @"\clint_ds.csproj");
                //sw = fi.AppendText();
                //s="";
                //s=s+"<VisualStudioProject>\r\n";
                //s=s+"<CSHARP\r\n";
                //s=s+"ProjectType = \"Local\"\r\n";
                //s=s+"ProductVersion = \"7.10.3077\"\r\n";
                //s=s+"SchemaVersion = \"2.0\"\r\n";
                //s=s+"ProjectGuid = \"{9DE6000B-A0D2-455A-A5BC-AF8DE33D5893}\"\r\n";
                //s=s+">\r\n";
                //s=s+"<Build>\r\n";
                //s=s+"<Settings\r\n";
                //s=s+"ApplicationIcon = \"\"\r\n";
                //s=s+"AssemblyKeyContainerName = \"\"\r\n";
                //s=s+"AssemblyName = \"ds_" + warename.ToLower() + "\"\r\n";
                //s=s+"AssemblyOriginatorKeyFile = \"\"\r\n";
                //s=s+"DefaultClientScript = \"JScript\"\r\n";
                //s=s+"DefaultHTMLPageLayout = \"Grid\"\r\n";
                //s=s+"DefaultTargetSchema = \"IE50\"\r\n";
                //s=s+"DelaySign = \"false\"\r\n";
                //s=s+"OutputType = \"Library\"\r\n";
                //s=s+"PreBuildEvent = \"\"\r\n";
                //s=s+"PostBuildEvent = \"\"\r\n";
                //s=s+"RootNamespace = \"ds_" + warename.ToLower() + "\"\r\n";
                //s=s+"RunPostBuildEvent = \"OnBuildSuccess\"\r\n";
                //s=s+"StartupObject = \"\"\r\n";
                //s=s+">\r\n";
				s=d4csproj2;
				sw.Write(s);
				sw.Flush();
				sw.Close();
				L("clint_ds.csproj Write OK !");
				L("End Write clint_ds.csproj");

                //add by maobb 2009-6-17
                L("Start Append Compile Files");
                DirectoryInfo ap_di=new DirectoryInfo(indexpath + "\\" + warename + ".res");
                if(ap_di.Exists)
                {
                    FileInfo[] ap_files=ap_di.GetFiles();
                    foreach (FileInfo ap_file in ap_files)
                    {
                        if (File.Exists(outputpath + "\\" + ap_file.Name))
                        {
                            FileInfo fi = new FileInfo(outputpath + "\\" + ap_file.Name);
                            if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;
                            File.Delete(outputpath + "\\" + ap_file.Name);
                        } 
                        ap_file.CopyTo(outputpath + "\\" + ap_file.Name, true);
                        L("Copy " + ap_file.Name + " OK!");
                    }
                }
                L("End Append Compile Files");
                //L("Start Write cs.build for ant");
                //if(File.Exists(outputpath + @"\cs.build"))
                //{
                //    L("cs.build Exist,now Delete");
                //    File.Delete(outputpath + @"\cs.build");
                //}
                //sw = new StreamWriter(outputpath + @"\cs.build", false, NowEncoding);
                ////sw=File.CreateText(outputpath + @"\cs.build");
                ////sw.Close();
                ////L("cs.build Created");
                ////fi = new FileInfo(outputpath + @"\cs.build");
                ////sw = fi.AppendText();
                //s="<project name=\"Solution Build Example\" default=\"rebuild\">\r\n";
                //s+="<property name=\"configuration\" value=\"release\"/>\r\n";
                //s+="<target name=\"clean\" description=\"Delete all previously compiled binaries.\">\r\n";
                //s+="<delete><fileset><include name=\"**/bin/**\" />";
                //s+="<include name=\"**/obj/**\" /> <include name=\"**/*.suo\" /><include name=\"**/*.user\" />";
                //s+="</fileset></delete></target>\r\n";
                //s+="<target name=\"build\" description=\"Build all targets.\">\r\n";
                //s+="<call target=\"build.d4\"/>\r\n";
                //s+="<call target=\"build.d4_client\"/>\r\n</target>\r\n";
                //s+="<target name=\"rebuild\" depends=\"clean, build\" />\r\n";
                //s+="<target name=\"build.d4\">\r\n<solution configuration=\"${configuration}\" solutionfile=\"d4.sln\" />\r\n";
                //s+="<property name=\"expected.output\" value=\"bin/${configuration}/D4." + warename + ".dll\"/>\r\n";
                //s+="<fail unless=\"${file::exists(expected.output)}\">Output file doesn't exist in ${expected.output}</fail>\r\n</target>\r\n";
                //s+="<target name=\"build.d4_client\">\r\n";
                //s+="<solution configuration=\"${configuration}\" solutionfile=\"clint_d4.sln\" />\r\n";
                //s+="<property name=\"expected.output\" value=\"bin/${configuration}/d4_" + warename.ToLower() + ".dll\"/>\r\n";
                //s+=" <fail unless=\"${file::exists(expected.output)}\">Output file doesn't exist in ${expected.output}</fail>";
                //s+=" </target>\r\n";
                //s+="</project>";
                //sw.Write(s);
                //sw.Flush();
                //sw.Close();
                //L("End Write cs.build for ant");
				L("******************************************************");
				L("******************************************************");
				L("Compiling...");
				//return;

                L(ExeCommand("\"" + framepath + "\\msbuild.exe\" \"" + waredeploy + waredirpath.ToLower() + "\\ds.sln\" /t:Rebuild /p:Configuration=Release /noconlog"));
                L(ExeCommand("\"" + framepath + "\\msbuild.exe\" \"" + waredeploy + waredirpath.ToLower() + "\\clint_ds.sln\" /t:Rebuild /p:Configuration=Release /noconlog"));
				//L(ExeCommand(antpath + "\\NAnt rebuild -buildfile:" + waredeploy + warename.ToLower() + "\\cs.build"));

				//L("Compiling...Client...");
				//L(ExeCommand("\"" + devenv + "\" \"" + waredeploy + warename.ToLower() + "\\clint_ds.sln\" /build \"" + compiletype + "\""));
				if(!File.Exists(outputpath + @"\bin\release\FT." + warename + ".dll"))
				{
					L("File: " + outputpath + @"\bin\release\FT." + warename + ".dll not exists,return!");
                    goto START;
					return;
				}
				if(!File.Exists(outputpath + @"\bin\release\ft_" + warename.ToLower() + ".dll"))
				{
					L("File: " + outputpath + @"\bin\release\ft_" + warename.ToLower() + ".dll not exists,return!");
                    goto START;
					return;
				}
				if(!File.Exists(outputpath2 + @"\" + warename.ToLower() + ".dll"))
				{
					L("File: " + outputpath2 + @"\" + warename.ToLower() + ".dll not exists,return!");
                    goto START;
					return;
				}
				File.Delete(outputpath2 + @"\FT." + warename + ".dll");
				File.Delete(outputpath2 + @"\ft_" + warename.ToLower() + ".dll");
				File.Copy(outputpath + @"\bin\release\FT." + warename.ToLower() + ".dll",outputpath2 + @"\FT." + warename + ".dll",true);
				File.Copy(outputpath + @"\bin\release\ft_" + warename.ToLower() + ".dll",outputpath2 + @"\ft_" + warename.ToLower() + ".dll",true);
				File.Delete(outputpath2 + @"\" + warename.ToLower() + ".control");
				string[] comstr=new string[3];
				comstr[0]=outputpath2 + @"\" + warename.ToLower() + ".dll";
				comstr[1]=outputpath2 + @"\FT." + warename + ".dll";
				comstr[2]=outputpath2 + @"\ft_" + warename.ToLower() + ".dll";
				L("******************************************************");
				L("******************************************************");
				L("Writing Control File ...");
				L("******************************************************");
				//new Compression.ZipClass().ZipFileMain(@"F:\bk\bk2005-7-5\Server_Deploy_Doc\output",outputpath2 + @"\" + warename.ToLower() + ".control");
				//return;
				new Compression.ZipClass().ZipFiles(comstr,outputpath2 + @"\" + warename.ToLower() + ".control",indexpath + warename.ToLower() + ".res");
				L("******************************************************");
				L("Control File Write OK!");
				L("******************************************************");
				L("Control File Path:");
				L(outputpath2 + @"\" + warename.ToLower() + ".control");
				L("******************************************************");
				L("******************************************************");
				File.Delete(outputpath2 + @"\FT." + warename + ".dll");
				File.Delete(outputpath2 + @"\ft_" + warename.ToLower() + ".dll");
				File.Delete(outputpath2 + @"\" + warename.ToLower() + ".dll");
				File.Delete(outputpath2 + @"\" + warename.ToLower() + ".aspx");
	            
				
			}
			catch(Exception ex)
			{
			L(ex.Message);
			}
			finally
			{
                try
                {

                    if (DeleteTemp) Directory.Delete(outputpath, true);
                    
                }
                catch { }
				O("\n**********Press Enter to Start Control Deploy**************");
				Console.OpenStandardInput();
				warename=Console.ReadLine().Trim().ToLower();
				L("\nDeploy End");
                logWriter.Flush();
                logWriter.Close();
			}
            goto START;
			/*System.Console.WriteLine("测试1: 直接等待 n = 199 次循环");
			Wait(199);

			System.Console.WriteLine("\n\n测试2: 等待结束条件: b == true");
			// 主程序 开始
			b = false;
			new System.Threading.Thread(new System.Threading.ThreadStart(DoWait)).Start(); //监视线程: 显示滚动计数器
			//以下是耗时的主程序
			System.Threading.Thread.Sleep(5 * 1000); //主程序耗时 5 秒
			b = true; //主程序 结束
			System.Console.WriteLine("\n主程序耗时 5 秒");
			*/
		}
		private static void X(XmlNode x)
		{
            try
            {
                if (x.Name.ToLower().Equals("controlauthorization"))
                {
                    M("if(!new ComFunction().IsControlAuthorization(SiteID,ControlID,RoleType))");
                    M("{");
                    M("output.Write(Const.NotControlAuthorization);");
                    M("return;");
                    M("}");
                }
                if (x.Name.ToLower().Equals("exception2log"))
                {
                    M("log.Exception(ex);");
                    //M("log.error(\"{Message}:\\r\\n\" + ex.Message + \"\\r\\n{Source}:\\r\\n\" + ex.Source + \"\\r\\n{InnerException}:\\r\\n\" + ex.InnerException + \"\\r\\n{StackTrace}:\\r\\n\" + ex.StackTrace + \"\\r\\n{TargetSite}:\\r\\n\" +ex.TargetSite,\"{Exception}\");");
                }
                if (x.Name.ToLower().Equals("publishedauthorization"))
                {
                    M("if(new ComFunction().BeenPublished(" + x.Attributes.GetNamedItem("name").Value + "))");
                    M("{");
                    M("output.Write(Const.BeenPublishedNotModString);");
                    M("return;");
                    M("}");
                }
                if (x.Name.ToLower().Equals("dbstart"))   
                {
                    M("DB " + x.Attributes.GetNamedItem("name").Value + "=new DB();\n" + x.Attributes.GetNamedItem("name").Value + ".Open();");
                }
                if (x.Name.ToLower().Equals("rdrclose"))
                {
                    M(x.Attributes.GetNamedItem("name").Value + ".Close();\n");
                }
                if (x.Name.ToLower().Equals("sessionsettimeout"))
                {
                    M("Session.setTimeout(" + Unit(SC(x.Attributes.GetNamedItem("value").Value), true) + ");");
                }
                if (x.Name.ToLower().Equals("sessionremove"))
                {
                    M("Session.Remove(" + Unit(SC(x.Attributes.GetNamedItem("name").Value)) + ");");
                }
                if (x.Name.ToLower().Equals("sessionadd"))
                {
                    M("Session.Add(" + Unit(SC(x.Attributes.GetNamedItem("name").Value)) + "," + Unit(SC(x.Attributes.GetNamedItem("value").Value)) + ");");
                }
                if (x.Name.ToLower().Equals("write"))
                {
                    M("output.Write(" + Unit(SC(x.Attributes.GetNamedItem("value").Value)) + ");");
                }
                if (x.Name.ToLower().Equals("writeln"))
                {
                    M("output.Write(" + Unit(SC(x.Attributes.GetNamedItem("value").Value)) + "+\"\\n\"" + ");");
                }
                if (x.Name.ToLower().Equals("debug"))
                {
                    M("log.debug(" + Unit(SC(x.Attributes.GetNamedItem("value").Value)) + ");");
                }
                if (x.Name.ToLower().Equals("info"))
                {
                    M("log.info(" + Unit(SC(x.Attributes.GetNamedItem("value").Value)) + ");");
                }
                if (x.Name.ToLower().Equals("error"))
                {
                    M("log.error(" + Unit(SC(x.Attributes.GetNamedItem("value").Value)) + ");");
                }
                if (x.Name.ToLower().Equals("goto"))
                {
                    M("goto " + x.Attributes.GetNamedItem("name").Value + ";");
                }
                if (x.Name.ToLower().Equals("print"))
                {
                    L(Unit(SC(x.Attributes.GetNamedItem("value").Value)));
                }
                if (x.Name.ToLower().Equals("label"))
                {
                    M(x.Attributes.GetNamedItem("name").Value + ":");
                }
                if (x.Name.ToLower().Equals("return"))
                {
                    M("return " + Unit(SC(x.Attributes.GetNamedItem("value").Value)) + ";");
                }
                if (x.Name.ToLower().Equals("define"))
                {
                    if (x.Attributes.GetNamedItem("value") == null)
                    {
                        M(x.Attributes.GetNamedItem("type").Value + " " + x.Attributes.GetNamedItem("name").Value + ";");
                    }
                    else
                    {
                        if (x.Attributes.GetNamedItem("type").Value.Equals("int"))
                        {
                            M(x.Attributes.GetNamedItem("type").Value + " " + x.Attributes.GetNamedItem("name").Value + "=" + Unit(x.Attributes.GetNamedItem("value").Value, true) + ";");
                        }
                        else
                        {
                            M(x.Attributes.GetNamedItem("type").Value + " " + x.Attributes.GetNamedItem("name").Value + "=" + Unit(SC(x.Attributes.GetNamedItem("value").Value)) + ";");
                        }
                    }
                }
                if (x.Name.ToLower().Equals("send"))
                {
                    O("Get " + x.Attributes.GetNamedItem("name").Value + " Type:");
                    L("send " + x.Attributes.GetNamedItem("name").Value + " type:" + xmldoc.SelectSingleNode("//*[define]/define[@name='" + x.Attributes.GetNamedItem("name").Value + "']").Attributes.GetNamedItem("type").Value);
                    if (xmldoc.SelectSingleNode("//*[define]/define[@name='" + x.Attributes.GetNamedItem("name").Value + "']").Attributes.GetNamedItem("type").Value.Equals("int"))
                    {
                        M(x.Attributes.GetNamedItem("name").Value + "=" + Unit(x.Attributes.GetNamedItem("value").Value, true) + ";");
                    }
                    else
                    {
                        M(x.Attributes.GetNamedItem("name").Value + "=" + Unit(SC(x.Attributes.GetNamedItem("value").Value)) + ";");
                    }
                }
                if (x.Name.ToLower().Equals("execsql"))
                {
                    M(x.Attributes.GetNamedItem("name").Value + ".execSql(" + Unit(SC(x.Attributes.GetNamedItem("value").Value)) + ");");
                }
                if (x.Name.ToLower().Equals("dbexecsql"))
                {
                    M("DB.ExecSql(" + Unit(SC(x.Attributes.GetNamedItem("value").Value)) + ");");
                }
                if (x.Name.ToLower().Equals("dbend"))
                {
                    M(x.Attributes.GetNamedItem("name").Value + ".Close();");
                }
                if (x.Name.ToLower().Equals("try"))
                {
                    M("try{");
                }
                if (x.Name.ToLower().Equals("catch"))
                {
                    M("catch(Exception ex){");
                }
                if (x.Name.ToLower().Equals("finally"))
                {
                    M("finally{");
                }
                if (x.Name.ToLower().Equals("if"))
                {
                    M("if(" + SC(x.Attributes.GetNamedItem("value").Value) + "){");
                }
                if (x.Name.ToLower().Equals("else"))
                {
                    M("else{");
                }
                if (x.Name.ToLower().Equals("while"))
                {
                    M("while(" + SC(x.Attributes.GetNamedItem("value").Value) + "){");
                }
                if (x.Name.ToLower().Equals("for"))
                {
                    M("for(" + SC(x.Attributes.GetNamedItem("value").Value) + "){");
                }
                if (x.Name.ToLower().Equals("code"))
                {
                    M(x.InnerText);
                }
                if (x.HasChildNodes)
                {
                    XmlNodeList XL = x.ChildNodes;
                    int i;
                    for (i = 0; i < XL.Count; i++)
                    {
                        X(XL.Item(i));
                    }
                }
                if (x.Name.ToLower().Equals("try"))
                {
                    M("}");
                }
                if (x.Name.ToLower().Equals("catch"))
                {
                    M("}");
                }
                if (x.Name.ToLower().Equals("finally"))
                {
                    M("}");
                }
                if (x.Name.ToLower().Equals("if"))
                {
                    M("}");
                }
                if (x.Name.ToLower().Equals("else"))
                {
                    M("}");
                }
                if (x.Name.ToLower().Equals("while"))
                {
                    M("}");
                }
                if (x.Name.ToLower().Equals("for"))
                {
                    M("}");
                }
            }
		    catch(Exception exx)
		    {
                L("***********************Exception occured!*************************");
                L(exx.Message);
                L(x.OuterXml);
                L("******************************************************************");
		    }
		}
	    //多参数问题：解决$a,$b,$c,@xx 变成 a,b,c,xx()。
	    private static string GetUnionUnit(string s)
	    {
            string[] ps=s.Split(',');
            string returnstr = "";
	        foreach(string p in ps)
	        {
                returnstr += "," + Unit(p);
	        }
            if (returnstr.Length > 0) return returnstr.Substring(1);
            return returnstr;
	    }
		private static string Unit(string s)
		{
		string rt="";
		s=s.Replace("^#","%%%%").Replace("\"","\\\"");
		string[] sp=s.Split('#');
		int i;
			for(i=0;i<sp.Length;i++)
			{
				if(sp[i].StartsWith("$"))
				{
					rt=rt+sp[i].Substring(1,sp[i].Length-1);
				}
				else if(sp[i].StartsWith("@"))
				{
					rt=rt+getCode(sp[i].Substring(1,sp[i].Length-1));
				}
				else
				{
					rt=rt+"\"" + sp[i] + "\"";
				}
				if(i<sp.Length-1)
				{
					rt=rt+" + ";
				}
			}
			rt=rt.Replace("%%%%","#").Replace("^@","@").Replace("^~","~").Replace("^$","$");
			return rt;

		}
		private static string Unit(string s,bool b)
		{
			string rt="";
			if(b==true)
			{
				string[] sp=s.Split('#');
				int i;
				for(i=0;i<sp.Length;i++)
				{
					if(sp[i].StartsWith("$"))
					{
						rt=rt+sp[i].Substring(1,sp[i].Length-1);
					}
					else if(sp[i].StartsWith("@"))
					{
						rt=rt+getCode(sp[i].Substring(1,sp[i].Length-1));
					}
					else
					{
						rt=rt+ sp[i];
					}
					if(i<sp.Length-1)
					{
						rt=rt+" + ";
					}
				}
			}
			return rt;
		}
		private static string getCode(string s)
		{
			string r="";
			if(s.StartsWith("getTableName"))
			{
				if(s.IndexOf("[")<0)
				{
					r="str.GetTableName(SiteID,ControlName,DataSource)";
				}
				else
				{
					r="str.GetTableName(SiteID,ControlName,DataSource,\"" + s.Replace("getTableName[","").Replace("]","") + "\")";
				}
			}
            if (s.StartsWith("getEditor"))
            {
                string[] editorvalues = s.Substring(s.IndexOf("[") + 1, s.IndexOf("]") - s.IndexOf("[") - 1).Split(',');
                r = "\"<input type=\\\"hidden\\\" id=\\\"" + editorvalues[0] + "\\\" name=\\\"" + editorvalues[0] + "\\\" value=\\\"\" + " + editorvalues[4].Substring(1) + ".Replace(\"&\", \"&amp;\").Replace(\"<\", \"&lt;\").Replace(\">\", \"&gt;\").Replace(\"\\\"\", \"&quot;\") + \"\\\"/>";
                r += "<input type=\\\"hidden\\\" id=\\\"" + editorvalues[0] + "___Config\\\" value=\\\"\\\" />";
                r += "<iframe id=\\\"" + editorvalues[0] + "___Frame\\\" src=\\\"/dscommon/editor/fckeditor.html?InstanceName=" + editorvalues[0] + "&amp;Toolbar=" + editorvalues[3] + "\\\" width=\\\"" + editorvalues[1] + "\\\" height=\\\"" + editorvalues[2] + "\\\" frameborder=\\\"no\\\" scrolling=\\\"no\\\"></iframe>\"";
            }
            if (s.StartsWith("getResourcePath"))
            {
                r = "System.AppDomain.CurrentDomain.BaseDirectory + \"\\\\lib\\\\" + warename.ToLower() + ".res\\\\\"";
            }
			if(s.StartsWith("requestForm"))
			{
				r="this.Context.Request.Form[\"" + s.Substring("requestForm[".Length,s.Length-"requestForm[".Length-1) + "\"]";
			}
			if(s.StartsWith("Request"))
			{
				r="this.Context.Request[\"" + s.Substring("Request[".Length,s.Length-"Request[".Length-1) + "\"]";
			}
            if (s.StartsWith("request") && !s.StartsWith("requestForm"))
            {
                r = "this.Context.Request[\"" + s.Substring("request[".Length, s.Length - "request[".Length - 1) + "\"]";
            }
			if(s.StartsWith("callTemp"))
			{
                r = "ftdpcontroltemp." + s.Split('@')[1] + s.Split('@')[0].Replace("callTemp[", "(").Replace("]", ")");
				if(forcss)
				{
					XmlDocument cssdoc=new XmlDocument();
					cssdoc.Load(indexpath + tempxml);
					string c=cssdoc.SelectSingleNode("//configuration/temp[@name='" + s.Split('@')[1] + "']/tempstr").InnerText;
					string h="";
					string l="";
					while(c.IndexOf("setstyle=\"%{")>=0)
					{
						h=c.Substring(c.IndexOf("%{"),c.IndexOf("}%")-c.IndexOf("%{")+2);
						l=h.Substring(h.IndexOf("["),h.Length-h.IndexOf("["));
						l=h.Replace(l,"").Replace("%{","");
						c=c.Replace(h,l);
						if(forcssstr.IndexOf("name=\\\"" + l + "\\\"")<0)
						{
							string _caption=h.Substring(h.IndexOf("[")+1,h.IndexOf("|||")-h.IndexOf("[")-1);
							string _description=h.Substring(h.IndexOf("|||")+3,h.IndexOf("]")-h.IndexOf("|||")-3);
							forcssstr+="<style name=\\\"" + l + "\\\" description=\\\"" + _description + "\\\" caption=\\\"" + _caption + "\\\"";
							h=h.Replace("[" + _caption + "|||" + _description + "]","");
							string _classdefault=h.Substring(h.IndexOf("[")+1,h.IndexOf("|||")-h.IndexOf("[")-1);
							string _styledefault=h.Substring(h.IndexOf("|||")+3,h.IndexOf("]")-h.IndexOf("|||")-3);
							forcssstr+=" class=\\\"" + _classdefault + "\\\" csstext=\\\"" + _styledefault + "\\\"/>";
						}
					}
				}
			}
			if(s.StartsWith("getDateTime"))
			{
                r = "DateTime.Now";
			}
			if(s.StartsWith("dot2DotDot"))
			{
                r = "str.D2DD(" + s.Substring("dot2DotDot[".Length, s.Length - "dot2DotDot[".Length - 1) + ")";
			}
			if(s.StartsWith("getSqlString"))
			{
                r = "ftdpcontrolsql." + s.Split('@')[1] + s.Split('@')[0].Replace("getSqlString[", "(").Replace("]", ")");
			}
            if (s.StartsWith("getControlPara"))
            {
                r = "ControlPara.Split(';')" + s.Replace("getControlPara","");
            }
			if(s.StartsWith("execSql"))
			{
				r=s.Split(',')[0].Replace("execSql[","")+".execSql(" + Unit(s.Split(',')[1].Replace("]","")) + ")";
			}
            if (s.StartsWith("DBExecSql"))
            {
                r = "DB.ExecSql(" + Unit(s.Split('[')[1].Replace("]", "")) + ")";
            }
            if (s.StartsWith("dbExecSql"))
            {
                r = "DB.ExecSql(" + Unit(s.Split('[')[1].Replace("]", "")) + ")";
            }
            if (s.StartsWith("DBGetDataSet"))
            {
                r = "DB.GetDataSet(" + GetUnionUnit(s.Split('[')[1].Replace("]", "")) + ")";
            }
            if (s.StartsWith("dbGetDataSet"))
            {
                r = "DB.GetDataSet(" + GetUnionUnit(s.Split('[')[1].Replace("]", "")) + ")";
            }
            if (s.StartsWith("DBGetDataTable"))
            {
                r = "DB.GetDataTable(" + Unit(s.Split('[')[1].Replace("]", "")) + ")";
            }
            if (s.StartsWith("dbGetDataTable"))
            {
                r = "DB.GetDataTable(" + Unit(s.Split('[')[1].Replace("]", "")) + ")";
            }
			if(s.StartsWith("dbGetString"))
			{
				r=s.Split(',')[0].Replace("dbGetString[","")+".GetString(" + Unit(s.Split(',')[1].Replace("]","")) + ")";
			}
			if(s.StartsWith("dbGetInteger"))
			{
				r=s.Split(',')[0].Replace("dbGetInteger[","")+".GetCount(" + Unit(s.Split(',')[1].Replace("]","")) + ")";
			}
			if(s.StartsWith("getString"))
			{
				r=s.Split(',')[0].Replace("getString[","")+".GetString(" + Unit(s.Split(',')[1].Replace("]","")) + ")";
			}
            if (s.StartsWith("getInt32"))
            {
                r = s.Split(',')[0].Replace("getInt32[", "") + ".GetInt32(" + Unit(s.Split(',')[1].Replace("]", "")) + ")";
            }
            if (s.StartsWith("getValue"))
            {
                r = s.Split(',')[0].Replace("getValue[", "") + ".GetValue(" + Unit(s.Split(',')[1].Replace("]", "")) + ")";
            }
			if(s.StartsWith("getRecord"))
			{
				r=s.Split(',')[0].Replace("getRecord[","")+".OpenRecord(" + Unit(s.Split(',')[1].Replace("]","")) + ")";
			}
			if(s.StartsWith("intParse"))
			{
				r="int.Parse(" + Unit(s.Split('[')[1].Replace("]","")) + ")";
			}
			if(s.StartsWith("Exception"))
			{
				r="ex.Message";
			}
            if (s.StartsWith("exception"))
            {
                r = "ex.Message";
            }
			if(s.StartsWith("getSession"))
			{
				r="Session.Get(" + Unit(s.Split('[')[1].Replace("]","")) + ")";
			}
			if(s.StartsWith("javaScript"))
			{
				r="str.JavascriptLabel(" + Unit(s.Split('[')[1].Replace("]","")) + ")";
			}
			if(s.StartsWith("Break"))
			{
				r="\"\\n\"";
			}
            if (s.StartsWith("break"))
            {
                r = "\"\\n\"";
            }
            if (s.StartsWith("getStyleClassName"))
            {
                r = "ftdpcontroltemp.getStyleClass(" + Unit(s.Split('[')[1].Replace("]", "")) + ")";
            }
            if (s.StartsWith("getStyleCssText"))
            {
                r = "ftdpcontroltemp.getStyleCssTxt(" + Unit(s.Split('[')[1].Replace("]", "")) + ")";
            }
			return r;
		}
		private static void M(string s)
		{
		CSCode=CSCode + s + "\n";
		}
		private static string SC(string s)
		{
		return s.Replace("@'","\"").Replace("@[","<").Replace("@]",">").Replace("\\","\\\\");
		}
		private static void O(object o)
		{
		Console.Write(o);
        logWriter.WriteLine(o.ToString());
		}
		private static void L(object o)
		{
			Console.WriteLine(o);
		    logWriter.WriteLine(o.ToString());
		}
	    private static string ChangeSql(string[] paras,string sql)
	    {
	        string rsql=sql.Replace("\"","\\\"").Replace("#@","####$$$$");
	        foreach(string para in paras)
	        {
                if(para.StartsWith("d"))
                    rsql = rsql.Replace("@" + para + "@", "\"+str.GetDateTime(" + para + ")+\"");
                else
                    rsql=rsql.Replace("@" + para + "@", "\"+" + para + "+\"");
	        }
            rsql = rsql.Replace("####$$$$", "@");
            return rsql;
	    }
	    private static string GetSqlParas(string[] paras)
	    {
	        string s="";
	        for(int i=0;i<paras.Length;i++)
	        {
	            switch(paras[i].Substring(0,1))
	            {
                    case "s": s += "string " + paras[i]; break;
                    case "i": s += "int " + paras[i]; break;
                    case "d": s += "DateTime " + paras[i]; break;
                    case "o": s += "object " + paras[i]; break;
	            }
	            if(i<paras.Length-1)
	            {
                    s += ",";
	            }
	        }
            return s;
	    }
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

			catch(Exception e)

			{

				strOutput = e.Message;

			}

			return strOutput;

		}
		private static string adaperQut(string xml0)
		{
		//\"

		//if(xml0.EndsWith("\""))xml0="\\" + xml0;
		//if(!xml0.StartsWith("\""))xml0="\"" + xml0;
		if(xml0.EndsWith("\\"))xml0=xml0 + "\"";
		if(xml0.StartsWith("\""))xml0=xml0.Substring(1);
		//if(!xml0.EndsWith("\""))xml0=xml0 + "\"";
		xml0="\"" + xml0 + "\"";
		return xml0;
		}


	}
}
