using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;
using FTFrame;
using FTFrame.DBClient;
using System.Drawing;

namespace FTFrame.Tool
{
    public class Project
    {
        public static string GetLiquid(string TableName, string ColName, string PattenStr, string LockLikeStr)
        {
            string liquid_id = GetLiquidID(TableName, ColName, PattenStr, LockLikeStr);
            int liquid_loop = 0;
            while (!IsLiquidIDOK(TableName, ColName, liquid_id))
            {
                if (liquid_loop > 99)
                {
                    liquid_id = "[error]loop99";
                    break;
                }
                liquid_loop++;
                liquid_id = GetLiquidID(TableName, ColName, PattenStr, LockLikeStr);
            }
            return liquid_id;
        }
        public static string GetLiquid_AddNext(string CurId, string PattenStr)
        {
            PattenStr = PattenStr.Trim();
            DateTime dt = DateTime.Now;
            PattenStr = str.GetDateTimeCustom(dt, PattenStr.Replace("[Y]", "yyyy").Replace("[M]", "MM").Replace("[D]", "dd").Replace("[h]", "HH").Replace("[m]", "mm").Replace("[s]", "ss"));
            Regex r = new Regex(@"\[N\(\d\)\]");
            Match m = r.Match(PattenStr);
            if (!m.Success) return "[error]no[N]";
            string RateN = m.Value;
            int RateI = m.Index;
            int RateLengh = int.Parse(RateN.Substring(RateN.IndexOf('(') + 1, RateN.IndexOf(')') - RateN.IndexOf('(') - 1));
            try
            {
                int MaxID = 1;
                string v = CurId.Substring(RateI, RateLengh);
                if (v != null)
                {
                    if (int.TryParse(v.ToString(), out MaxID))
                    {
                        MaxID += 1;
                    }
                    else
                    {
                        MaxID = 1;
                    }
                }
                if (MaxID >= Math.Pow(10, RateLengh))
                {
                    return "[error]max" + RateLengh;
                }
                string NewID = MaxID.ToString();
                for (int i = 0; i < RateLengh - MaxID.ToString().Length; i++)
                {
                    NewID = "0" + NewID;
                }
                NewID = PattenStr.Replace(RateN, NewID);
                return NewID;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "[error]ex";
            }
        }
        private static string GetLiquidID(string TableName, string ColName, string PattenStr, string LockLikeStr)
        {
            PattenStr = PattenStr.Trim();
            LockLikeStr = LockLikeStr.Trim();
            DateTime dt = DateTime.Now;
            PattenStr = str.GetDateTimeCustom(dt, PattenStr.Replace("[Y]", "yyyy").Replace("[M]", "MM").Replace("[D]", "dd").Replace("[h]", "HH").Replace("[m]", "mm").Replace("[s]", "ss"));
            if (!LockLikeStr.Equals(""))
            {
                LockLikeStr = str.GetDateTimeCustom(dt, LockLikeStr.Replace("[Y]", "yyyy").Replace("[M]", "MM").Replace("[D]", "dd").Replace("[h]", "HH").Replace("[m]", "mm").Replace("[s]", "ss").Replace("%", "&&")).Replace("&&", "%");
            }
            Regex r = new Regex(@"\[N\(\d\)\]");
            Match m = r.Match(PattenStr);
            if (!m.Success) return "[error]no[N]";
            string RateN = m.Value;
            int RateI = m.Index;
            int RateLengh = int.Parse(RateN.Substring(RateN.IndexOf('(') + 1, RateN.IndexOf(')') - RateN.IndexOf('(') - 1));
            DB db = new DB();
            db.Open();
            try
            {
                string sql = null;
                switch (SysConst.DataBaseType)
                {
                    case DataBase.MySql:
                        if (LockLikeStr.Equals(""))
                        {
                            sql = "select SUBSTRING(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  order by  rate desc";
                        }
                        else
                        {
                            sql = "select SUBSTRING(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  where " + str.D2DD(ColName) + " like '" + str.D2DD(LockLikeStr) + "' order by  rate desc";
                        }
                        break;
                    case DataBase.SqlServer:
                        if (LockLikeStr.Equals(""))
                        {
                            sql = "select SUBSTRING(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  order by  rate desc";
                        }
                        else
                        {
                            sql = "select SUBSTRING(" + str.D2DD(ColName) + "," + (RateI + 1) + "," + RateLengh + ") as rate from " + str.D2DD(TableName) + "  where " + str.D2DD(ColName) + " like '" + str.D2DD(LockLikeStr) + "' order by  rate desc";
                        }
                        break;
                }
                int MaxID = 1;
                object v = db.GetObject(sql);
                if (v != null)
                {
                    if (int.TryParse(v.ToString(), out MaxID))
                    {
                        MaxID += 1;
                    }
                    else
                    {
                        MaxID = 1;
                    }
                }
                if (MaxID >= Math.Pow(10, RateLengh))
                {
                    return "[error]max" + RateLengh;
                }
                string NewID = MaxID.ToString();
                for (int i = 0; i < RateLengh - MaxID.ToString().Length; i++)
                {
                    NewID = "0" + NewID;
                }
                NewID = PattenStr.Replace(RateN, NewID);
                return NewID;
            }
            catch
            {
                return "[error]ex";
            }
            finally { db.Close(); }
        }
        private static bool IsLiquidIDOK(string TableName, string ColName, string LiquidID)
        {
            if (LiquidID.StartsWith("[error]")) return true;
            DB db = new DB();
            db.Open();
            try
            {
                string sql = "select count(*) as ca from " + str.D2DD(TableName) + " where " + str.D2DD(ColName) + "='" + str.D2DD(LiquidID) + "'";
                return db.GetInt(sql) == 0;
            }
            catch
            {
                return true;
            }
            finally { db.Close(); }
        }
        public static ArrayList GetUserDepartRealArray(string ids)
        {
            ArrayList UserDeparts = new ArrayList();
            if (ids != null)
            {
                string[] Ids = ids.Split(',');
                foreach (string Id in Ids)
                {
                    string id = GetUserDepartRealID(Id);
                    if (id != null && !id.Equals(""))
                    {
                        if (!UserDeparts.Contains(id)) UserDeparts.Add(id);
                    }
                }
            }
            return UserDeparts;
        }
        public static string GetUserDepartRealID(string Id)
        {
            string id = (Id == null ? null : Id.Trim());
            if (id != null && !id.Equals(""))
            {
                if (id.IndexOf('_') == 1)
                {
                    id = id.Substring(2);
                }
                if (id.Length > 37) id = id.Substring(0, id.LastIndexOf('_'));
            }
            return id;
        }

        public static object[] GetContinent(int pid)
        {
            DB db = new DB();
            db.Open();
            try
            {
                string sql = "select id,name from FD_CONTINENT where pid=" + pid + " order by id";
                ArrayList al = new ArrayList();
                DR dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    al.Add(new string[] { dr.GetInt32(0).ToString(), dr.GetString(1) });
                }
                dr.Close();
                return al.ToArray();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new object[0];
            }
            finally { db.Close(); }
        }
        public static object[] GetCountry(int pid)
        {
            DB db = new DB();
            db.Open();
            try
            {
                string sql = "select id,name from FD_COUNTRY where pid=" + pid + " order by id";
                ArrayList al = new ArrayList();
                DR dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    al.Add(new string[] { dr.GetInt32(0).ToString(), dr.GetString(1) });
                }
                dr.Close();
                return al.ToArray();
            }
            catch (Exception ex) { log.Error(ex); return new object[0]; }
            finally { db.Close(); }
        }
        public static object[] GetProvince(int pid)
        {
            DB db = new DB();
            db.Open();
            try
            {
                string sql = "select id,name from FD_PROVINCE where pid=" + pid + " order by id";
                ArrayList al = new ArrayList();
                DR dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    al.Add(new string[] { dr.GetInt32(0).ToString(), dr.GetString(1) });
                }
                dr.Close();
                return al.ToArray();
            }
            catch (Exception ex) { log.Error(ex); return new object[0]; }
            finally { db.Close(); }
        }
        public static object[] GetCity(int pid)
        {
            DB db = new DB();
            db.Open();
            try
            {
                string sql = "select id,name from FD_CITY where pid=" + pid + " order by id";
                ArrayList al = new ArrayList();
                DR dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    al.Add(new string[] { dr.GetInt32(0).ToString(), dr.GetString(1) });
                }
                dr.Close();
                return al.ToArray();
            }
            catch (Exception ex) { log.Error(ex); return new object[0]; }
            finally { db.Close(); }
        }
        public static void RightUpdate(string ScourceID, string UserIDs, int TypeID)
        {
            DB db = new DB();
            db.Open();
            ST st = db.GetTransaction();
            try
            {
                string sql = "delete from FT_RIGHT where SOURCEID='" + str.D2DD(ScourceID) + "' and TYPEID=" + TypeID + "";
                db.ExecSql(sql, st);
                ArrayList al = GetUserDepartRealArray(UserIDs);
                foreach (string UserID in al)
                {
                    sql = "insert into FT_RIGHT(SOURCEID,USERID,TYPEID)";
                    sql += "values('" + str.D2DD(ScourceID) + "','" + str.D2DD(UserID) + "'," + TypeID + ")";
                    db.ExecSql(sql, st);
                }
                al.Clear();
                al = null;
                st.Commit();
            }
            catch (Exception ex) { st.Rollback(); log.Error(ex); }
            finally
            {
                db.Close();
            }
        }

        public static string RightGet(string ScourceID, int TypeID)
        {
            DB db = new DB();
            db.Open();
            try
            {
                string sql = "select USERID from FT_RIGHT where SOURCEID='" + str.D2DD(ScourceID) + "' and TYPEID=" + TypeID;
                string Restr = "";
                DR dr = db.OpenRecord(sql);
                while (dr.Read())
                {
                    Restr += "," + dr.GetString(0);
                }
                dr.Close();
                if (!Restr.Equals("")) Restr = Restr.Substring(1);
                return Restr;
            }
            catch (Exception ex) { log.Error(ex); return ""; }
            finally
            {
                db.Close();
            }
        }
        public static bool RightHave(string ScourceID, int TypeID)
        {
            if (UserTool.IsAdmin()) return true;
            if (!UserTool.IsLogin()) return false;
            string UserID = UserTool.CurUserID();
            DB db = new DB();
            db.Open();
            try
            {
                string sql = "select count(*) from FT_RIGHT where SOURCEID='" + str.D2DD(ScourceID) + "' and USERID='" + str.D2DD(UserID) + "' and TYPEID=" + TypeID;
                return db.GetInt(sql) > 0;
            }
            catch (Exception ex) { log.Error(ex); return false; }
            finally
            {
                db.Close();
            }
        }
    }
    public class Validate
    {
        //验证码长度
        private static int codeLen = 4;
        //图片清晰度
        private static int fineness = 100;
        //图片宽度
        private static int imgWidth = 90;
        //图片高度
        private static int imgHeight = 29;
        //字体大小
        private static int fontSize = 18;
        //绘制起始坐标
        private static int posX = -2;
        //绘制起始坐标
        private static int posY = 0;
        public Validate()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public Bitmap GetValidateImg()
        {
            //HttpContext.Current.Response.Expires = 0;
            //Response.AddHeader 'Pragma','no-cache';
            //Response.AddHeader 'cache-ctrol','no-cache';

            string validateCode = CreateValidateCode();

            //生成BITMAP图像
            Bitmap bitmap = new Bitmap(imgWidth, imgHeight);

            BgColor = GetColor();
            LineColor = GetColor2();
            //给图像设置干扰
            DisturbBitmap(bitmap);

            //绘制验证码图像
            DrawValidateCode(bitmap, validateCode);
            return bitmap;
        }
        //-----------------------------------------
        //随机生成验证码,并保存到SESSION中
        //-----------------------------------------
        private string CreateValidateCode()
        {
            string validateCode = "";

            //随机数对象
            Random random = new Random();


            for (int i = 0; i < codeLen; i++)
            {
                //26;a - z
                int n = random.Next(26);

                //将数字转换成大写字母
                validateCode += (char)(n + 65);
            }

            //保存验证码
            session.SetString("Validate", validateCode);

            return validateCode;
        }


        //-----------------------------------------
        //为图片设置干扰点
        //-----------------------------------------
        private void DisturbBitmap(Bitmap bitmap)
        {
            // 通过随机数生成
            Random random = new Random();

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    if (random.Next(100) <= fineness)
                        bitmap.SetPixel(i, j, BgColor);
                }
            }




        }
        private Color BgColor = Color.Black;
        private Color LineColor = Color.Black;
        private Color[] c = { Color.LightYellow, Color.LightCyan, Color.AliceBlue, Color.LightGreen, Color.White };
        private Color[] c2 = { Color.Red, Color.DarkBlue, Color.DarkGreen, Color.Brown };
        private string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
        private FontStyle[] fs = { FontStyle.Bold, FontStyle.Italic, FontStyle.Regular, FontStyle.Bold | FontStyle.Italic };
        private Color GetColor()
        {
            Random random = new Random();
            return c[random.Next(c.Length)];
        }
        private Color lastcolor = Color.Empty;
        private string lastfont = "";
        private FontStyle lastStyle = FontStyle.Strikeout;
        private Color GetColor2()
        {
            Random random = new Random();
            Color cc = c2[random.Next(c2.Length)];
            while (cc.Equals(lastcolor))
            {
                cc = c2[random.Next(c2.Length)];
            }
            lastcolor = cc;
            return cc;
        }
        private string GetFont()
        {
            Random random = new Random();
            string cc = font[random.Next(font.Length)];
            while (cc.Equals(lastfont))
            {
                cc = font[random.Next(font.Length)];
            }
            lastfont = cc;
            return cc;
        }
        private FontStyle GetFontStyle()
        {
            Random random = new Random();
            FontStyle cc = fs[random.Next(fs.Length)];
            while (cc.Equals(lastStyle))
            {
                cc = fs[random.Next(fs.Length)];
            }
            lastStyle = cc;
            return cc;
        }
        //-----------------------------------------
        //绘制验证码图片
        //-----------------------------------------
        private void DrawValidateCode(Bitmap bitmap, string validateCode)
        {
            //随机获取颜色,随机字体,随机字体样式
            Random random = new Random();
            Random random1 = new Random();
            Random random2 = new Random();
            Random rand = new Random();

            Font f = new Font(font[random1.Next(font.Length)], fontSize, fs[random2.Next(fs.Length)]);
            Brush b = new SolidBrush(c2[random.Next(c2.Length)]);




            //获取绘制器对象
            Graphics g = Graphics.FromImage(bitmap);

            //绘制验证码图像
            g.DrawString(validateCode[0].ToString(), f, b, posX, posY);

            f = new Font(font[random1.Next(font.Length)], fontSize, fs[random2.Next(fs.Length)]);
            b = new SolidBrush(c2[random.Next(c2.Length)]);
            g.DrawString(validateCode[1].ToString(), f, b, posX + imgWidth / 4, posY);

            f = new Font(font[random1.Next(font.Length)], fontSize, fs[random2.Next(fs.Length)]);
            b = new SolidBrush(c2[random.Next(c2.Length)]);
            g.DrawString(validateCode[2].ToString(), f, b, posX + imgWidth / 2, posY);

            f = new Font(font[random1.Next(font.Length)], fontSize, fs[random2.Next(fs.Length)]);
            b = new SolidBrush(c2[random.Next(c2.Length)]);
            g.DrawString(validateCode[3].ToString(), f, b, posX + (imgWidth * 3) / 4, posY);
            //画图片的背景噪音线
            for (int i = 0; i < 16; i++)
            {
                int x1 = rand.Next(bitmap.Width);
                int x2 = rand.Next(bitmap.Width);
                int y1 = rand.Next(bitmap.Height);
                int y2 = rand.Next(bitmap.Height);

                g.DrawLine(new Pen(LineColor), x1, y1, x2, y2);
            }
        }
    }
}
