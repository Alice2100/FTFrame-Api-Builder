using System;
using System.Resources;
using System.Reflection;
namespace SiteMatrix.Resource
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
				return s + "NotExsitsIn" + rname;
			}	
		}
		public  string GetString(string s)
		{
			try
			{
				return resm.GetString(s);
			}
			catch
			{
                return s + "NotExsitsIn" + rname;
			}	
		}
	}
}
