using System;
using System.ComponentModel;
namespace SiteMatrix.classes
{
	/// <summary>
	/// LicenceProvider 的摘要说明。
	/// </summary>
	public class LicenceProvider:LicenseProvider
	{
		public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
		{
            if ("FTDP".Equals(SiteMatrix.consts.globalConst.ProductName) && "Maobinbin".Equals(SiteMatrix.consts.globalConst.CompanyName))
			{
				return new SiteMatrix.classes.Licence(type);
			}
			throw new LicenseException(type, instance, "Your license is invalid.\nYou can get Developer License from Syslive!");
		}

	}
}
