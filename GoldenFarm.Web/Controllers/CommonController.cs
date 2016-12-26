using GoldenFarm.Entity;
using GoldenFarm.Repository;
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
        private SmsRepository sr = new SmsRepository();

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


        public ActionResult GetSMS(string category, string phone, string vcode)
        {
            if(string.IsNullOrEmpty(category))
            {
                category = "Common";
            }
            if(string.Compare(vcode, (string)TempData[CaptchaImageText], StringComparison.InvariantCultureIgnoreCase)!=0)
            {
                return Json(new { result = false, msg = "请确认验证码" });
            }
            string code = StringHelper.GenerateRandomCode(6, true);
            string msg = @"尊敬的用户：您本次的验证码为 {0}，如非本人操作，请勿转告他人。";
            msg = string.Format(msg, code);
            SmsHelper.SendSms(phone, msg);
            Sms sms = new Sms
            {
                Phone = phone,
                Code = code,
                Message = msg,
                Sender = (CurrentUser != null) ? CurrentUser.Id : 0,
                Category = category,
                CreateTime = DateTime.Now
            };
            sr.Create(sms);
            return Json(new { result = true, msg = string.Empty });
        }


        
    }
}