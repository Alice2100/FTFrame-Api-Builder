using System;
using System.ComponentModel;
namespace FTDPClient.classes
{
	/// <summary>
	/// LicenceProvider 的摘要说明。
	/// </summary>
	public class LicenceProvider:LicenseProvider
	{
		public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
		{
            if ("FTDP".Equals(FTDPClient.consts.globalConst.ProductName) && "Maobinbin".Equals(FTDPClient.consts.globalConst.CompanyName))
			{
				return new FTDPClient.classes.Licence(type);
			}
			throw new LicenseException(type, instance, "Your license is invalid.\nYou can get Developer License from Syslive!");
		}

	}
}
