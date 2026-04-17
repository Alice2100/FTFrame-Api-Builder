using System;
using System.ComponentModel;
namespace FTDPClient.classes
{
	/// <summary>
	/// Licence 的摘要说明。
	/// </summary>
	public class Licence:License
	{
		private Type type=null;

		public Licence(Type type)
		{
			this.type=type;
		}
		public override string LicenseKey
		{
			get
			{
				return "0BBD259A-5A17-4ae5-A260-D7FFAF414412";
			}
		}
		public override void Dispose()
		{

		}

	}
}
