using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Util
{
    public class SmsHelper
    {
        static string url = ConfigurationManager.AppSettings["WebReference.Service.PostUrl"];
        static string user = ConfigurationManager.AppSettings["WebReference.Service.User"];
        static string pwd = ConfigurationManager.AppSettings["WebReference.Service.Pwd"];

        public static bool SendSms(string phone, string message)
        {
            string postStrTpl = "un={0}&pw={1}&phone={2}&msg={3}&rd=1";

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postData = encoding.GetBytes(string.Format(postStrTpl, user, pwd, phone, message));
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            myRequest.KeepAlive = false;
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = postData.Length;
            myRequest.Timeout = 5000;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(postData, 0, postData.Length);
            newStream.Flush();
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            if (myResponse.StatusCode == HttpStatusCode.OK)
            {
                myResponse.Close();
                myRequest.Abort();
                return true;
            }
            else
            {
                myRequest.Abort();
                myResponse.Close();
                return false;
            }
        }

    }
}
