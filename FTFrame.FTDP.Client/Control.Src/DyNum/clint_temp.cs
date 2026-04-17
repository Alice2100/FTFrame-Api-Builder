/*FTDP Control,This Code Created By FTDP Deploy Tool
Build By Maobb 2007-6-10
Code Deploy Time is:2020-06-05 09:49:47*/
using System;
namespace ft_dynum
{
    public class ftdpcontroltemp
    {
        private static string setStyle(string name)
        {
            return setStyleClass(name) + setStyleCssTxt(name);
        }
        private static string setStyleClass(string name)
        {
            int i;
            for (i = 0; i < client.thisStyleObject.Length; i++)
            {
                if (((string[])(client.thisStyleObject[i]))[0].Equals(name))
                {
                    if (!((string[])(client.thisStyleObject[i]))[1].Equals(""))
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
            for (i = 0; i < client.thisStyleObject.Length; i++)
            {
                if (((string[])(client.thisStyleObject[i]))[0].Equals(name))
                {
                    if (!((string[])(client.thisStyleObject[i]))[2].Equals(""))
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
            for (i = 0; i < client.thisStyleObject.Length; i++)
            {
                if (((string[])(client.thisStyleObject[i]))[0].Equals(name))
                {
                    if (!((string[])(client.thisStyleObject[i]))[1].Equals(""))
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
            for (i = 0; i < client.thisStyleObject.Length; i++)
            {
                if (((string[])(client.thisStyleObject[i]))[0].Equals(name))
                {
                    if (!((string[])(client.thisStyleObject[i]))[2].Equals(""))
                    {
                        return ((string[])(client.thisStyleObject[i]))[2];
                    }
                    return "";
                }
            }
            return "";
        }
        public static string input(string p0, string p1, string p2, string p3, string p4, string p5, string p6)
        {
            return ("<input type=hidden name=\"ftform_liquid_" + p0 + "\" id=\"ftform_liquid_" + p1 + "\" value=\"" + p2 + " &&" + p3 + "&&" + p4 + "&&" + p5 + "&&" + p6 + "\"/>").Trim();
        }
        public static string ClientView(string p0)
        {
            return ("<img src=\"" + p0 + "\" border=\"0\"/>").Trim();
        }
    }
}
