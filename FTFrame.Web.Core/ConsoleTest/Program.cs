using System;
using System.Collections.Generic;
using FTFrame.DBClient;
using System.Linq;
using ConsoleTest.Busi;

namespace ConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var jh = new JH();
            jh.Busi_Test();
            return;
            Console.WriteLine("Hello World!");
            Auth.Access_Token = "123";
            Console.WriteLine(Api.AccountGroup2);
            Auth.Access_Token = "456";
            Console.WriteLine(Api.AccountGroup2);
            return;
            using DB db = new DB(false);
            List<string> list = new List<string>() {
               DateTime.Now.ToString("yyyyMM"),
               DateTime.Now.AddMonths(-1).ToString("yyyyMM")
            };
            var l2 = list.ToArray();
            // var list2 = db.SelectList<testObj>(r=> l2.Contains(r.name) && r.id==1,r=>r.OrderByDescending(s=>s.id));
        }
    }
    public class testObj
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class Auth
    {
        public static string Access_Token = null;
    }
    public struct Api
    {
        public static Func<string> AccountGroup = () => ("https://api.kingdee.com/jdy/sys/accountGroup?access_token=" + Auth.Access_Token);
        public static string AccountGroup2 { get { return "https://api.kingdee.com/jdy/sys/accountGroup?access_token=" + Auth.Access_Token; } }
    }
}
