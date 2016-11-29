using GoldenFarm.Util;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoldenFarm.Web.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult VerificationImgForm()
        {
            this.Session["CaptchaImageText"] = GenerateRandomCode();
            // Create a CAPTCHA image using the text stored in the Session object.
            RandomImage ci = new RandomImage(this.Session
                ["CaptchaImageText"].ToString(), 300, 75);
            // Change the response headers to output a JPEG image.
            //this.Response.Clear();
            //this.Response.ContentType = "image/jpeg";
            // Write the image to the response stream in JPEG format.
            MemoryStream ms = new MemoryStream();
            ci.Image.Save(ms, ImageFormat.Jpeg);
            // Dispose of the CAPTCHA image object.
           // ci.Dispose();

            return File(ms.ToArray(), "image/jpeg");
        }


        private string GenerateRandomCode()
        {
            Random r = new Random();
            string s = "";
            for (int j = 0; j < 4; j++)
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
                        ch = r.Next(65, 90);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                    case 3:
                        ch = r.Next(97, 122);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                    default:
                        ch = r.Next(97, 122);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                }
                r.NextDouble();
                r.Next(100, 1999);
            }
            return s;
        }
    }
}