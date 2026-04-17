/*FTDP Control,This Code Created By FTDP Deploy Tool
Build By Maobb 2007-6-10
Code Deploy Time is:2020-06-05 09:55:17*/
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace ft_dyvalue
{
public class ftdpcontroltemp
{
        public class TextShow
        {
            public static string Zhi = "Value";//值
            public static string ShuZu = "Array";//数组
            public static string MingCheng = "Name";//名称
            public static string BangDingZiDuan = "Bound Field";//绑定字段
            public static string LeiXing = "Type";//类型
            public static string FilterRule = "Filter Rule";//过滤规则

        }
        private static string setStyle(string name)
{
return setStyleClass(name) + setStyleCssTxt(name);
}
private static string setStyleClass(string name)
{
int i;
for(i=0;i<client.thisStyleObject.Length;i++)
{
	if(((string[])(client.thisStyleObject[i]))[0].Equals(name))
	{
		if(!((string[])(client.thisStyleObject[i]))[1].Equals(""))
		{
			return "class=\"" + ((string[])(client.thisStyleObject[i]))[1] + "\" ";
		}
		return "";
	}
}
return "";
}
private static string setStyleCssTxt(string name)
{
	int i;
	for(i=0;i<client.thisStyleObject.Length;i++)
	{
		if(((string[])(client.thisStyleObject[i]))[0].Equals(name))
	{
		if(!((string[])(client.thisStyleObject[i]))[2].Equals(""))
		{
			return "style=\"" + ((string[])(client.thisStyleObject[i]))[2] + "\" ";
		}
		return "";
	}
}
return "";
}
public static string getStyleClass(string name)
{
int i;
for(i=0;i<client.thisStyleObject.Length;i++)
{
	if(((string[])(client.thisStyleObject[i]))[0].Equals(name))
	{
		if(!((string[])(client.thisStyleObject[i]))[1].Equals(""))
		{
			return ((string[])(client.thisStyleObject[i]))[1];
		}
		return "";
	}
}
return "";
}
public static string getStyleCssTxt(string name)
{
	int i;
	for(i=0;i<client.thisStyleObject.Length;i++)
	{
		if(((string[])(client.thisStyleObject[i]))[0].Equals(name))
	{
		if(!((string[])(client.thisStyleObject[i]))[2].Equals(""))
		{
			return ((string[])(client.thisStyleObject[i]))[2];
		}
		return "";
	}
}
return "";
}
}
	public class func
	{
		public static string getDecode(string str)
		{
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
		public static string LeiXingText(string val)
		{
			switch (val)
			{
				case "0": return "Add";
				case "1": return "Mod";
				default: return "其他";
			}
			return null;
		}
	}
}
