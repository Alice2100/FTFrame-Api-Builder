using System;
using System.Resources;
using System.Reflection;
namespace FTDPClient.Resource
{
	/// <summary>
	/// ResClass 的摘要说明。
	/// </summary>
	/// 
	public class rm
	{
		private ResourceManager resm;
		private string rname;
		public rm(string resname)
		{
			rname=resname;
			resm=new ResourceManager(resname,Assembly.GetExecutingAssembly());
		}
		public  object GetObject(string s)
		{
			try
			{
				return resm.GetObject(s);
			}
			catch
			{
				return "!!!" + s;// + "!!!" + rname;
			}	
		}
		public string str(string s,string defaultS=null)
		{
			return GetString(s, defaultS);
		}
		public  string GetString(string s, string defaultS = null)
		{
			try
			{
				return resm.GetString(s);
			}
			catch
			{
                return defaultS==null? ("!!!" + s): defaultS;// + "!!!" + rname;
			}	
		}
	}
}
