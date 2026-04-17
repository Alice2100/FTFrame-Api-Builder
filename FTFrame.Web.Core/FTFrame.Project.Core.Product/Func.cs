using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using System.Linq;
using System.IO;
using System.Xml;
using System.Collections;
using CoreHttp = Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Drawing;
using System.Net;

namespace FTFrame.Project.Core
{
    public class ComFunc
    {
        /// <summary>
        /// 得到设置的值
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="SetKey"></param>
        /// <returns></returns>
        public static string GetSetValue(string AppName, string SetKey)
        {
            using (DB userDB = new DB())
            {
                userDB.Open();
                string sql = "select SetVal from TB_SET where AppName='" + str.D2DD(AppName) + "' and SetKey='" + str.D2DD(SetKey) + "'";
                string SetVal = userDB.GetString(sql);
                if (SetVal != null) return SetVal;
                sql = "select DefaultVal from TB_Set_Config where AppName='" + str.D2DD(AppName) + "' and SetKey='" + str.D2DD(SetKey) + "'";
                return userDB.GetString(sql);
            }
        }
        /// <summary>
        /// 得到字典options html，Col的值可为DicId,DicCode和DicName，根据不同需要灵活组合
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="SetKey"></param>
        /// <param name="ValueCol"></param>
        /// <param name="TextCol"></param>
        /// <returns></returns>
        public static string GetDicOptions(string AppName, string SetKey, string ValueCol, string TextCol)
        {

            using (DB userDB = new DB())
            {
                userDB.Open();
                string s = "";
                string sql = "select " + str.GetSafeCodeRemoveAll(ValueCol) + ", " + str.GetSafeCodeRemoveAll(TextCol) + " from TB_DIC where AppName='" + str.D2DD(AppName) + "' and SetKey='" + str.D2DD(SetKey) + "' order by Rank";
                using (DR dr = new DR(userDB, sql))
                {
                    while (dr.Read())
                    {
                        s += "<option value='" + dr.GetStringNoNULL(0) + "'>" + dr.GetStringNoNULL(1) + "</option>";
                    }
                }
                return s;
            }
        }


        /// <summary>
        /// 得到字典取值Sql，Col的值可为DicId,DicCode和DicName，根据不同需要灵活组合
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="SetKey"></param>
        /// <param name="ValueCol"></param>
        /// <param name="TextCol"></param>
        /// <returns></returns>
        public static string GetDicSql(string ComName, string SetKey, string ValueCol, string TextCol)
        {
            return "select " + str.GetSafeCodeRemoveAll(ValueCol) + ", " + str.GetSafeCodeRemoveAll(TextCol) + " from TB_DIC where AppName='" + str.D2DD(ComName) + "' and SetKey='" + str.D2DD(SetKey) + "' order by Rank";
        }
    }
    public class Func
    {
        public static string escape(string str)
        {
            return Uri.EscapeDataString(str);
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in str)
                {
                    sb.Append((Char.IsLetterOrDigit(c)
                    || c == '-' || c == '_' || c == '\\'
                    || c == '/' || c == '.') ? c.ToString() : Uri.HexEscape(c));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                log.Error("str is " + str);
                log.Error(ex);
                throw ex;
            }
        }
        /// <summary>
        /// 生成压缩图片
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public static string ReducedImage(string imgpath, int Width, int Height)
        {

            try
            {

                if (imgpath == null || imgpath == "") return null;
                string filename = AppDomain.CurrentDomain.BaseDirectory + imgpath.Replace("/", "\\");
                if (filename.EndsWith("_rdced_.jpeg")) return null;
                Image img = Image.FromFile(filename);
                if (img.Width <= Width && img.Height <= Height) return imgpath;
                Bitmap b = new Bitmap(Width, Height);
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                g.DrawImage(img, new Rectangle(0, 0, Width, Height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                g.Dispose();
                string filename2 = filename + "_rdced_.jpeg";
                filename2 = filename2.Replace(",", "");
                b.Save(filename2, System.Drawing.Imaging.ImageFormat.Jpeg);

                b.Dispose();
                b = null;

                return imgpath.Replace(",", "") + "_rdced_.jpeg";
            }
            catch (Exception e)
            {
                log.Error(e);
                return null;
            }
        }
        public static string TurnPageHTML_LayUI(string[] para)
        {
            string PartId = para[0];//片段ID
            int CountAll = int.Parse(para[1]);//总数量
            int PageSize = int.Parse(para[2]);//每页数量
            int PageCount = int.Parse(para[3]);//总页数
            int CurPage = int.Parse(para[4]);//当前页码
            string s = "";
            s += "<div class=\"layui-box layui-laypage layui-laypage-default\" id=\"layui-laypage-18\">";
            if (CountAll == 0)
            {
                s += "<span class=\"layui-laypage-count\">记录数为 0</span>";
            }
            else
            {
                if (CurPage <= 1)
                {
                    s += "<a href=\"javascript:;\" class=\"layui-laypage-prev\" data-page=\"0\"><i class=\"layui-icon\"></i></a>";
                }
                else
                {
                    s += "<a href=\"javascript:load_" + PartId + "_part(" + (CurPage - 1) + ");\" class=\"layui-laypage-prev\" data-page=\"" + (CurPage - 1) + "\">" + (CurPage - 1) + "</a>";
                }
                for (int i = Math.Max(1, CurPage - 4); i <= Math.Min(PageCount, CurPage + 4); i++)
                {
                    if (i == CurPage)
                    {
                        s += "<span class=\"layui-laypage-curr\"><em class=\"layui-laypage-em\"></em><em>1</em></span>";
                    }
                    else
                    {
                        s += "<a href=\"javascript:load_" + PartId + "_part(" + i + ");\" data-page=\"2\">2</a>";
                    }
                }
                if (CurPage < PageCount)
                {
                    s += "<a href=\"javascript:load_" + PartId + "_part(" + PageCount + ");;\" class=\"layui-laypage-last\" title=\"尾页\" data-page=\"" + PageCount + "\">" + PageCount + "</a>";
                }
                else
                {//layui-disabled
                    s += "<a href=\"javascript:;\" class=\"layui-laypage-last\" data-page=\"0\"><i class=\"layui-icon\"></i></a>";
                }
                s += "<span class=\"layui-laypage-count\">共 " + CountAll + " 条</span>";
            }
            s += "<input type=hidden id=\"t_" + PartId + "_part\" value='" + CurPage + "'/>";
            s += "</div>";
            return s;
        }
        public static string TurnPageHTML_Vue(string[] para)
        {
            string PartId = para[0];//片段ID
            int CountAll = int.Parse(para[1]);//总数量
            int PageSize = int.Parse(para[2]);//每页数量
            int PageCount = int.Parse(para[3]);//总页数
            int CurPage = int.Parse(para[4]);//当前页码
            string s = "";
            s += "<div class=\"el-pagination is-background\">";
            if (CountAll == 0)
            {
                //s += "<li class=\"number\">记录数为 0</li>";
                s += "<button type=\"button\" disabled=\"disabled\" class=\"btn-prev\" style='padding-left:12px;padding-right:12px'>The number of records is 0</button>";
            }
            else
            {
                if (CurPage > 1)
                {
                    s += "<button type=\"button\" class=\"btn-prev\" onclick=\"load_" + PartId + "_part(" + (CurPage - 1) + ")\"><i class=\"el-icon el-icon-arrow-left\"></i></button>";
                }
                else
                {
                    s += "<button type=\"button\" disabled=\"disabled\" class=\"btn-prev\"><i class=\"el-icon el-icon-arrow-left\"></i></button>";
                }
                s += "<ul class=\"el-pager\">";
                for (int i = Math.Max(1, CurPage - 4); i <= Math.Min(PageCount, CurPage + 4); i++)
                {
                    s += "<li class=\"number " + (CurPage == i ? " active" : "") + "\" onclick=\"load_" + PartId + "_part(" + i + ")\">" + i + "</li>";
                }
                s += "</ul>";
                if (CurPage < PageCount)
                {
                    s += "<button type=\"button\" class=\"btn-next\" onclick=\"load_" + PartId + "_part(" + (CurPage + 1) + ")\"><i class=\"el-icon el-icon-arrow-right\"></i></button>";
                }
                else
                {
                    s += "<button type=\"button\" disabled=\"disabled\"  class=\"btn-next\"><i class=\"el-icon el-icon-arrow-right\"></i></button>";
                }
                s += "<span style=\"line-height: 30px; font-weight:normal\"><b>" + CountAll + "</b> records in total</span>";
                //s += "  每页显示";
                //s+= "<select id=\"ts_"+ PartId + "_part\" onchange=\"dl_search('" + PartId + "_part')\" class=\"_sel\"><option value=\" - 1\">默认</option><option value=\"20\">20</option><option value=\"30\">30</option><option value=\"50\">50</option><option value=\"100\">100</option><option value=\"200\">200</option><option value=\"500\">500</option><option value=\"1000\">1000</option><!--option value=\"0\">全部</option--></select>";
                //s += "条";
                //s += "<em class=\"xgt_meitu_nav_total\">共<b>" + CountAll + "</b>结果</em>";
                s += "<input type=hidden id=\"t_" + PartId + "_part\" value='" + CurPage + "'/>";
                s += "<input type=hidden id=\"ts_" + PartId + "_part\" value='-1'/>";
            }
            s += "</div>";
            return s;
        }
        public static string TurnPageHTML(string[] para)
        {
            string PartId = para[0];//片段ID
            int CountAll = int.Parse(para[1]);//总数量
            int PageSize = int.Parse(para[2]);//每页数量
            int PageCount = int.Parse(para[3]);//总页数
            int CurPage = int.Parse(para[4]);//当前页码
            string s = "";
            s += "<nav aria-label=\"Page navigation\" class=\"text-center\">";
            s += "<ul class=\"pagination\">";
            if (CountAll == 0)
            {
                s += "<li><a>记录数为 0</a></li>";

            }
            else
            {
                if (CurPage > 1)
                {
                    s += "<li><a href=\"javascript:void(0)\" onclick=\"load_" + PartId + "_part(" + (CurPage - 1) + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">上一页</span></a></li>";
                }
                for (int i = Math.Max(1, CurPage - 4); i <= Math.Min(PageCount, CurPage + 4); i++)
                {
                    s += "<li" + (CurPage == i ? " class=\"active\"" : "") + "><a href=\"javascript:void(0)\" onclick=\"load_" + PartId + "_part(" + i + ")\">" + i + "</a></li>";
                }
                if (CurPage < PageCount)
                {
                    s += "<li><a href=\"javascript:void(0)\" onclick=\"load_" + PartId + "_part(" + (CurPage + 1) + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">下一页</span></a></li>";
                }
                s += "<em class=\"xgt_meitu_nav_total\">共<b>" + CountAll + "</b>结果</em>";

            }
            s += "</ul>";
            s += "</nav>";
            s += "<input type=hidden id=\"t_" + PartId + "_part\" value='" + CurPage + "'/>";
            s += "<div class=\"el-pagination is-background\"><button type=\"button\" disabled=\"disabled\" class=\"btn-prev\"><i class=\"el-icon el-icon-arrow-left\"></i></button><ul class=\"el-pager\"><li class=\"number active\">1</li><!----><li class=\"number\">2</li><li class=\"number\">3</li><li class=\"number\">4</li><li class=\"number\">5</li><li class=\"number\">6</li><li class=\"el-icon more btn-quicknext el-icon-more\"></li><li class=\"number\">100</li></ul></div>";
            s += "<div id=app2><el-pagination ";
            s += "\r\n background ";
            s += "\r\n  layout =\"prev, pager, next\" ";
            s += "\r\n  :total=\"1000\" > ";
            s += "\r\n </el-pagination></div> ";
            return s;
        }
        /// <summary>
        /// 上传的图片保存，返回图片路径
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /*
        public static string ImageSave(HttpPostedFile file)
        {
            if (file == null || file.FileName == null || file.FileName.IndexOf('.') < 0) return null;
            if (!IsPicture(file.FileName)) return null;
            if (file.ContentLength > 10 * 1024 * 1024) return null;
            string extName = file.FileName.Substring(file.FileName.LastIndexOf('.'));
            string subPath = @"\_ftfiles\busi\" + DateTime.Now.ToString("yyyy_MM");
            string SavePath = SysConst.RootPath + subPath;
            if (!Directory.Exists(SavePath)) Directory.CreateDirectory(SavePath);
            string newFilename = str.GetCombID() + extName;
            file.SaveAs(SavePath + "\\" + newFilename);
            return subPath.Replace("\\", "/") + "/" + newFilename;
        }*/
        // 判断文件是否是图片 
        public static bool IsPicture(string fileName)
        {
            string strFilter = ".jpeg|.gif|.jpg|.png|.bmp|.pic|.tiff|.ico|.iff|.lbm|.mag|.mac|.mpt|.opt|";
            char[] separtor = { '|' };
            string[] tempFileds = StringSplit(strFilter, separtor);
            foreach (string str in tempFileds)
            {
                if (str.ToUpper() == fileName.Substring(fileName.LastIndexOf("."), fileName.Length - fileName.LastIndexOf(".")).ToUpper()) { return true; }
            }
            return false;
        }
        public static int NewMessageCount()
        {
            string UserID = UserTool.CurUserID();
            if (UserID == null) return 0;
            string sql = "select count(*) from TB_MESSAGE where STAT=1 and USERID='" + str.D2DD(UserID) + "' and ISREAD=0";
            DB db = new DB();
            db.Open();
            try
            {
                return db.GetInt(sql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return 0;
            }
            finally
            {
                db.Close();
            }
        }
        // 通过字符串，分隔符返回string[]数组 
        private static string[] StringSplit(string s, char[] separtor)
        {
            string[] tempFileds = s.Trim().Split(separtor); return tempFileds;
        }
        /// <summary>
        /// Post数据到指定URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string postData)
        {
            try
            {
                System.Net.WebRequest wRequest = System.Net.WebRequest.Create(url);
                //wRequest.ContentType = "text/html; charset=UTF-8";
                var data = Encoding.UTF8.GetBytes(postData);
                wRequest.Method = "POST";
                wRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                wRequest.ContentLength = data.Length;
                using (var stream = wRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)wRequest.GetResponse();
                System.IO.Stream respStream = response.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.Message;
            }
        }
        /// <summary>
        /// Get Http数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGet(string Url)
        {
            return HttpGet(Url, Encoding.UTF8);
        }
        public static string HttpGet(string Url, Encoding dataEncode)
        {
            string ret = "";
            try
            {
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(Url));
                webReq.Method = "GET";
                webReq.ContentType = "application/x-www-form-urlencoded";

                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), dataEncode);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 写文字到指定文件
        /// </summary>
        /// <param name="FullFileName"></param>
        /// <param name="FileContent"></param>
        /// <param name="dataEncode"></param>
        /// <returns></returns>
        public static string SaveToFile(string FullFileName, string FileContent, Encoding dataEncode)
        {
            try
            {
                FullFileName = FullFileName.Replace("/", @"\");
                string BaseDir = FullFileName.Substring(0, FullFileName.LastIndexOf(@"\"));
                if (!Directory.Exists(BaseDir))
                {
                    Directory.CreateDirectory(BaseDir);
                }
                if (File.Exists(FullFileName)) File.Delete(FullFileName);

                using (FileStream fs = new FileStream(BaseDir, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs, dataEncode))
                    {
                        sw.WriteLine(FileContent);
                        sw.Close();
                    }
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }
    }

}
