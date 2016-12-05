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
            password = password.MD5Hash();
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
        public ActionResult Reg(string phone, string password, string passwordc, string vcode, int refuid)
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
            user.Password = password.MD5Hash();
            user.LastLoginIP = Request.UserIP();
            user.LastLoginTime = DateTime.Now;
            user.CreateTime = DateTime.Now;
            if(refuid > 0)
            {
                user.RefUserId = refuid;
            }
            ur.Create(user);
            return RedirectToAction("Index");
        }

        public ActionResult ChangePwd()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangePwd(string oldpassword, string newpassword, string newpasswordc)
        {
            if(string.IsNullOrEmpty(oldpassword) || string.IsNullOrEmpty(newpassword) || string.IsNullOrEmpty(newpasswordc))
            {
                ModelState.AddModelError("changepwd", "请填写密码");
                return View();
            }

            if (newpassword != newpasswordc)
            {
                ModelState.AddModelError("changepwd", "请确认新密码");
                return View();
            }

            if (CurrentUser.Password != oldpassword.MD5Hash())
            {
                ModelState.AddModelError("changepwd", "请确认原密码");
                return View();
            }

            CurrentUser.Password = newpassword.MD5Hash();
            ur.Update(CurrentUser);

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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult VerifyMobile(string phone, string vcode, string mcode)
        {
            string code = (string)TempData[CommonController.CaptchaImageText];
            if (string.Compare(vcode, code, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                ModelState.AddModelError("vm", "验证码错误");
                return View();
            }
            return View();
        }


        public ActionResult ChangeMobile()
        {
            return View(CurrentUser);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangeMobile(string phone, string vcode, string mcode)
        {
            return View();
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
            return View(CurrentUser);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Certificated(string name, string idnum)
        {
            if(string.IsNullOrEmpty(name))
            {
                ModelState.AddModelError("cert", "请输入姓名");
                return View();
            }

            if(string.IsNullOrEmpty(idnum))
            {
                ModelState.AddModelError("cert", "请输入身份证号");
                return View();
            }

            CurrentUser.DisplayName = name;
            CurrentUser.IdNum = idnum;
            ur.Update(CurrentUser);
            return View(CurrentUser);
        }

        public ActionResult BindBankCard()
        {
            var bankAccount = ur.GetBankAccount(CurrentUser.Id);
            ViewBag.Name = CurrentUser.DisplayName;
            return View(bankAccount);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult BindBankCard(string bank, string accountNum)
        {
            var bankAccount = ur.GetBankAccount(CurrentUser.Id);
            if (string.IsNullOrEmpty(bank) || string.IsNullOrEmpty(accountNum))
            {
                ModelState.AddModelError("bank", "请输入银行和卡号");
                return View(bankAccount);
            }
            
            if (bankAccount == null)
            {
                bankAccount = new UserBankAccount();
                bankAccount.UserId = CurrentUser.Id;
                bankAccount.CreateTime = DateTime.Now;                
            }

            bankAccount.Bank = bank;
            bankAccount.AccountNum = accountNum;
            bankAccount.AccountName = CurrentUser.DisplayName;
            
            ur.SaveBankAccount(bankAccount);
            return RedirectToAction("BindBankCard");
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