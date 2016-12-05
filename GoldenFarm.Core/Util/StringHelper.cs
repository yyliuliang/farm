using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace GoldenFarm.Util
{
    public class StringHelper
    {
        public static string GenerateRandomCode(int length = 4, bool onlyNumber = false)
        {
            Random r = new Random();
            string s = "";
            for (int j = 0; j < length; j++)
            {
                int i = r.Next(3);
                int ch;
                switch (i)
                {
                    case 1:
                        ch = r.Next(0, 9);
                        s = s + ch.ToString();
                        break;
                    case 2:
                        ch = onlyNumber ? r.Next(0, 9) : r.Next(65, 90);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                    case 3:
                        ch = onlyNumber ? r.Next(0, 9) : r.Next(97, 122);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                    default:
                        ch = onlyNumber ? r.Next(0, 9) : r.Next(97, 122);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                }
                r.NextDouble();
                r.Next(100, 1999);
            }
            return s;
        }


        public static string MD5Hash(string text)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(text, "MD5");
        }
    }
}
