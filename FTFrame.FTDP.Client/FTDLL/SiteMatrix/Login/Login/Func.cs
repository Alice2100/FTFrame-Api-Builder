using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;
namespace Login
{
    public class Func
    {
        public static string GetEncode(object str)
        {
            return GetEncode(str.ToString());
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetEncode(string str)
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
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetDecode(string str)
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
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetEncode_L2(string str)
        {
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "^#bgjodlp$332xbx34efwe1";

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


            sTemp = "kfo$6fjf7&krn%339dNfd";
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
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetDecode_L2(string str)
        {
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "^#bgjodlp$332xbx34efwe1";
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
            sTemp = "kfo$6fjf7&krn%339dNfd";
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

    }
}
