using GoldenFarm.Repository;
using GoldenFarm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GoldenFarm.Web.Models;

namespace GoldenFarm.Web.Controllers
{
    public class UserController : BaseController
    {
        private UserRepository ur = new UserRepository();
        // GET: User
        public ActionResult Index()
        {
            var model = new UserIndexViewModel();
            model.User = CurrentUser;
            model.UserProducts = ur.GetProductsByUser(CurrentUser.Id);
            return View(model);
        }

        public ActionResult UserExists()
        {
            string username = Request["username"];
            return Content(ur.UserExists(username) ? "1" : "0");
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult Logout()
        {
            _Logout();
            return RedirectToAction("Login");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(string phone, string password, string vcode, bool rememberMe = false)
        {
            if(string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("login", "请填写手机和密码");
                return View();
            }
            string code = (string)TempData[CommonController.CaptchaImageText];
            if (string.Compare(vcode, code, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                ModelState.AddModelError("login", "验证码错误");
                return View();
            }
            password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
            var user = ur.Login(phone, password);
            if(user == null)
            {
                ModelState.AddModelError("login", "用户名或密码错误");
                return View();
            }
            rememberMe = Request.Form["rememberme"] == "1";
            _Login(user, rememberMe);
            string returnUrl = Request.Form["returnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }
        

        public ActionResult Reg()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Reg(string phone, string password, string passwordc, string vcode)
        {
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("reg", "请填写手机和密码");
                return View();
            }

            if(password != passwordc)
            {
                ModelState.AddModelError("reg", "请确认密码");
                return View();
            }

            string code = (string)TempData[CommonController.CaptchaImageText];
            if (string.Compare(vcode, code, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                ModelState.AddModelError("reg", "验证码错误");
                return View();
            }

            if(ur.UserExists(phone))
            {
                ModelState.AddModelError("reg", "此号码系统中已存在");
                return View();
            }

            Entity.User user = new Entity.User();
            user.UserGuid = Guid.NewGuid();
            user.Phone = phone;
            user.UserName = user.Phone;
            user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
            user.LastLoginIP = Request.UserIP();
            user.LastLoginTime = DateTime.Now;
            user.CreateTime = DateTime.Now;
            ur.Create(user);
            return RedirectToAction("Index");
        }

        public ActionResult ChangePwd()
        {
            return View();
        }

        public ActionResult FindPassword()
        {
            return View();
        }

        public ActionResult Safe()
        {
            return View(CurrentUser);
        }

        public ActionResult VerifyMobile()
        {
            return View(CurrentUser);
        }


        public ActionResult GiveSwitch()
        {
            return View(CurrentUser);
        }


        public ActionResult Wallet()
        {
            return View(CurrentUser);
        }

        public ActionResult Deposit()
        {
            return View(CurrentUser);
        }

        public ActionResult Withdraw()
        {
            return View(CurrentUser);
        }

        public ActionResult FinanceDetail()
        {
            return View();
        }

        public ActionResult Entrust()
        {
            return View();
        }

        public ActionResult TradeHistory()
        {
            return View();
        }

        public ActionResult BorrowHistory()
        {
            return View();
        }

        public ActionResult Give()
        {
            return View();
        }

        public ActionResult GiveHistory()
        {
            return View();
        }


        public ActionResult Certificated()
        {
            return View();
        }

        public ActionResult BindBankCard()
        {
            return View();
        }

        #region 推广中心

        public ActionResult PopLink()
        {
            return View();
        }

        public ActionResult PopDetail()
        {
            return View();
        }

        public ActionResult PopReward()
        {
            return View();
        }

        public ActionResult FillReferId()
        {
            return View();
        }
        #endregion

    }
}