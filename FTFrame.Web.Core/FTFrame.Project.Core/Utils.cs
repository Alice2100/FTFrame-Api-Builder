using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FTFrame.Tool;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Yitter.IdGenerator;

namespace FTFrame.Project.Core.Utils
{
    public class Jwt
    {
        public static string SecretKey = "";// ConfigHelper.GetConfigValue("Jwt:SecretKey");
        public static int ExpiresMinutes = 99;// int.Parse(ConfigHelper.GetConfigValue("Jwt:ExpiresMinutes"));
        public static string Domin = "";// ConfigHelper.GetConfigValue("Jwt:Domin");
        public static string Generate(string userId, string userName, string corpId)
        {
            // 1. 选择加密算法
            var algorithm = SecurityAlgorithms.HmacSha256;

            // 2. 定义需要使用到的Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim("CorpId", corpId),
            };

            // 3. secretKey
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            // 4. 生成Credentials
            var signingCredentials = new SigningCredentials(secretKey, algorithm);

            // 5. 根据以上组件，生成token
            var token = new JwtSecurityToken(
                Domin,    //Issuer
                Domin,  //Audience
                claims,                          //Claims,
                DateTime.Now,                   //notBefore
                DateTime.Now.AddMinutes(ExpiresMinutes),         //expires
                signingCredentials
            );
            // 6. 将token变为string
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
        public static (ClaimsPrincipal claimsPrincipal, string exception) Validate(string jwtToken)
        {
            try
            {
                var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Domin,
                    ValidAudience = Domin,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))
                }, out SecurityToken validatedToken);
                return (claimsPrincipal, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
    }
    public class Str
    {
        private static object _lock = new object();
        public static string GenerateCode(string prefix, string tableName, string filedName, List<string> tempCodeList = null)
        {
            lock (_lock)
            {
                var code = prefix + DateTime.Now.ToString("yyyyMMdd") + new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999);
                using (var db = DBSuit.DBReadOnly())
                {
                    var loop = 0;
                    while (loop < 99 && ((tempCodeList != null && tempCodeList.Contains(code)) || db.GetInt($"select count(*) from {tableName.D0()} where {filedName.D0()}='{code}'") > 0))
                    {
                        loop++;
                        code = prefix + DateTime.Now.ToString("yyyyMMdd") + new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999);
                    }
                    if (loop >= 99) return "CODESAME";
                    else return code;
                }
            }
        }
        public static string FitUrlToStr(string str)
        {
            return str.Replace("_X01X_", ":").Replace("_X02X_", ";").Replace("_X03X_", "/").Replace("_X04X_", "\\").Replace("_X05X_", "?").Replace("_X06X_", "&").Replace("_X07X_", "%").Replace("_X08X_", "=").Replace("_X09X_", "-").Replace("_X10X_", "+").Replace("_X11X_", "#");
        }
        public static string StrToFitUrl(string str)
        {
            return str.Replace(":", "_X01X_").Replace(";", "_X02X_").Replace("/", "_X03X_").Replace("\\", "_X04X_").Replace("?", "_X05X_").Replace("&", "_X06X_").Replace("%", "_X07X_").Replace("=", "_X08X_").Replace("-", "_X09X_").Replace("+", "_X10X_").Replace("#", "_X11X_");
        }
        public static string Encode(object val)
        {
            return Encode(val.ToString());
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encode(string str)
        {
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "78Ujd%rynSDF@#maobb234efwe";

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


            sTemp = "oP0#dinj&odkmmr45du#89dkf0";
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
        public static string Decode(string str)
        {
            SymmetricAlgorithm mobjCryptoService;
            String Key;

            mobjCryptoService = new RijndaelManaged();
            Key = "78Ujd%rynSDF@#maobb234efwe";
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
            sTemp = "oP0#dinj&odkmmr45du#89dkf0";
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

        private static IdGeneratorOptions idGeneratorOptions = null;
        public static long SnowId()
        {
            if (idGeneratorOptions == null)
            {
                ushort workId = 1;
                idGeneratorOptions = new IdGeneratorOptions(workId);
                idGeneratorOptions.WorkerIdBitLength = 6;
                YitIdHelper.SetIdGenerator(idGeneratorOptions);
            }
            return YitIdHelper.NextId();
        }
    }
    public class Net
    {
        public static string HttpPost(string Url, string postDataStr, string contentType = "application/json;charset=utf-8", string headerName = "", string headerValue = "")
        {

            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Method = "POST";
            //req.ContentType = "application/x-www-form-urlencoded";
            req.ContentType = contentType;
            if (headerName != "") req.Headers.Add(headerName, headerValue);

            byte[] data = Encoding.UTF8.GetBytes(postDataStr);

            req.ContentLength = data.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        public static void HttpPostDownload(string Url, string postDataStr, string filename)
        {
            filename = filename.Replace("/", "\\").Replace("\\\\", "\\");
            try
            {
                Directory.CreateDirectory(filename.Substring(0, filename.LastIndexOf('\\')));
            }
            catch (Exception ex)
            {

            }
            if (!Directory.Exists(filename.Substring(0, filename.LastIndexOf('\\')))) return;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            byte[] data = Encoding.UTF8.GetBytes(postDataStr);

            req.ContentLength = data.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }


            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            Stream so = new FileStream(filename, FileMode.Create);
            long totalDownloadedByte = 0;
            byte[] by = new byte[1024];
            int osize = stream.Read(by, 0, (int)by.Length);
            while (osize > 0)
            {
                totalDownloadedByte = osize + totalDownloadedByte;
                //Application.DoEvents();
                so.Write(by, 0, osize);
                osize = stream.Read(by, 0, (int)by.Length);
            }
            so.Close();
            stream.Close();
        }
        public static string HttpPostForm(string strUrl, List<(int Type, string Prop, string Value)> postParaList)
        {
            try
            {
                string responseContent = "";
                var memStream = new MemoryStream();
                var webRequest = (HttpWebRequest)WebRequest.Create(strUrl);
                var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
                var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
                var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");
                memStream.Write(beginBoundary, 0, beginBoundary.Length);
                webRequest.Method = "POST";
                webRequest.Timeout = 10000;
                webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                for (int i = 0; i < postParaList.Count; i++)
                {
                    var temp = postParaList[i];
                    if (temp.Type == 1)
                    {
                        var fileStream = new FileStream(temp.Value, FileMode.Open, FileAccess.Read);
                        // 写入文件
                        const string filePartHeader =
                            "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                            "Content-Type: application/octet-stream\r\n\r\n";
                        var header = string.Format(filePartHeader, temp.Prop, temp.Value);
                        var headerbytes = Encoding.UTF8.GetBytes(header);
                        memStream.Write(headerbytes, 0, headerbytes.Length);
                        var buffer = new byte[1024];
                        int bytesRead; // =0
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            memStream.Write(buffer, 0, bytesRead);
                        }
                        string end = "\r\n";
                        headerbytes = Encoding.UTF8.GetBytes(end);
                        memStream.Write(headerbytes, 0, headerbytes.Length);
                        fileStream.Close();
                    }
                    else if (temp.Type == 0)
                    {
                        var stringKeyHeader = "Content-Disposition: form-data; name=\"{0}\"" +
                                       "\r\n\r\n{1}\r\n";
                        var header = string.Format(stringKeyHeader, temp.Prop, temp.Value);
                        var headerbytes = Encoding.UTF8.GetBytes(header);
                        memStream.Write(headerbytes, 0, headerbytes.Length);
                    }
                    if (i != postParaList.Count - 1)
                        memStream.Write(beginBoundary, 0, beginBoundary.Length);
                    else
                        memStream.Write(endBoundary, 0, endBoundary.Length);
                }
                webRequest.ContentLength = memStream.Length;
                var requestStream = webRequest.GetRequestStream();
                memStream.Position = 0;
                var tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();
                using (HttpWebResponse res = (HttpWebResponse)webRequest.GetResponse())
                {


                    using (Stream resStream = res.GetResponseStream())
                    {
                        byte[] buffer = new byte[1024];
                        int read;
                        while ((read = resStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            responseContent += Encoding.UTF8.GetString(buffer, 0, read);
                        }
                    }
                    res.Close();
                }
                return responseContent;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return null;




        }
        public static string HttpGet(string Url, string postDataStr = "", string contentType = "application/json;charset=utf-8", string headerName = "", string headerValue = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = contentType;
            if (headerName != "") request.Headers.Add(headerName, headerValue);
            string retString = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream myResponseStream = response.GetResponseStream())
                {
                    using (StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8))
                    {
                        retString = myStreamReader.ReadToEnd();
                        myStreamReader.Close();
                        myResponseStream.Close();
                    }
                }
            }
            return retString;
        }

    }
}
