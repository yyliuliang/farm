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
    public class CommonController : BaseController
    {

        public const string CaptchaImageText = "CaptchaImageText";
        // GET: Common
        public ActionResult VerificationImgForm()
        {
            string code = StringHelper.GenerateRandomCode();            
            RandomImage ci = new RandomImage(code, 240, 60);
            TempData[CaptchaImageText] = code;
            using (MemoryStream ms = new MemoryStream())
            {
                ci.Image.Save(ms, ImageFormat.Jpeg);
                ci.Dispose();

                return File(ms.ToArray(), "image/jpeg");
            }
        }
    }
}