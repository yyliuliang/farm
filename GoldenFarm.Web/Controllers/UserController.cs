using GoldenFarm.Repository;
using GoldenFarm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GoldenFarm.Web.Models;
using System.IO;
using GoldenFarm.Util;
using GoldenFarm.Web.Filter;
using Dapper;
using System.Text;

namespace GoldenFarm.Web.Controllers
{
    public class UserController : BaseController
    {
        private UserRepository ur = new UserRepository();
        private MarketRepository mr = new MarketRepository();
        private ProductRepository pr = new ProductRepository();
        private SmsRepository sr = new SmsRepository();


        public ActionResult PaymentSuccess()
        {
            return View();
        }

        [CheckLogin]
        public ActionResult Index()
        {
            var model = new UserIndexViewModel();
            model.User = CurrentUser;
            model.UserProducts = ur.GetProductsByUser(CurrentUser.Id).Where(p => p.TotalCount > 0);
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
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
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
            if (user == null)
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
        public ActionResult Reg(string phone, string password, string passwordc, string vcode, string mcode, int? refCode)
        {
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("reg", "请填写手机和密码");
                return View();
            }

            if (password != passwordc)
            {
                ModelState.AddModelError("reg", "请确认密码");
                return View();
            }

            //string code = (string)TempData[CommonController.CaptchaImageText];
            //if (string.Compare(vcode, code, StringComparison.InvariantCultureIgnoreCase) != 0)
            //{
            //    ModelState.AddModelError("reg", "验证码错误");
            //    return View();
            //}

            if (!sr.CheckSms(phone, mcode, "Reg"))
            {
                ModelState.AddModelError("reg", "短信验证码错误");
                return View();
            }

            if (ur.UserExists(phone))
            {
                ModelState.AddModelError("reg", "此号码系统中已存在");
                return View();
            }

            var user = new User();
            User refUser = null;
            string refPath = string.Empty;
            user.UserGuid = Guid.NewGuid();
            user.Phone = phone;
            user.UserName = user.Phone;
            user.Password = password.MD5Hash();
            user.LastLoginIP = Request.UserIP();
            user.LastLoginTime = DateTime.Now;
            user.CreateTime = DateTime.Now;
            if (refCode.HasValue)
            {
                refUser = ur.Get(refCode.Value);
                if (refUser != null)
                {
                    user.RefUserId = refCode.Value;
                    refPath = refUser.RefUserPath;
                }
            }

            int uid = ur.Create(user);
            if (refUser != null)
            {
                user.RefUserPath = refPath + ";" + uid;
            }
            //else
            //{
            //    user.RefUserId = uid;
            //    user.RefUserPath = uid + ";";
            //}
            ur.Update(user);
            _Login(user, false);

            string sms = "恭喜您注册成功农场物语账号，祝您开心玩游戏，天天赚大钱。";
            SmsHelper.SendSms(user.Phone, sms);
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
            if (string.IsNullOrEmpty(oldpassword) || string.IsNullOrEmpty(newpassword) || string.IsNullOrEmpty(newpasswordc))
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

        [HttpPost]
        public ActionResult UploadPortrait(HttpPostedFileBase image)
        {
            string filename = Path.Combine(Server.MapPath("/Content/portrait/"), CurrentUser.Id.ToString() + ".jpg");
            image.SaveAs(filename);
            CurrentUser.Avatar = string.Format("/Content/portrait/{0}.jpg", CurrentUser.Id);
            ur.Update(CurrentUser);
            RefreshCurrentUser();
            return Json(new { success = true, image = CurrentUser.Avatar });
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GiveSwitch(string mcode)
        {
            if (!CurrentUser.SmsGiveSwitch)
            {
                if (!sr.CheckSms(CurrentUser.Phone, mcode))
                {
                    ModelState.AddModelError("code", "请确认短信验证码");
                    return View(CurrentUser);
                }
            }
            CurrentUser.SmsGiveSwitch = !CurrentUser.SmsGiveSwitch;
            ur.Update(CurrentUser);
            RefreshCurrentUser();
            return View(CurrentUser);
        }


        public ActionResult Wallet()
        {
            return View(CurrentUser);
        }

        public ActionResult Deposit()
        {
            var deposits = ur.GetDepositsByUser(CurrentUser.Id);
            return View(deposits);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Deposit(string gateway, decimal amount)
        {
            string url = string.Empty;
            var deposit = new UserDeposit();
            deposit.UserId = CurrentUser.Id;
            deposit.IP = Request.UserIP();
            deposit.Amount = amount;
            deposit.Gateway = gateway;
            deposit.FlowNum = DateTime.Now.ToString("yyyyMMdd") + CurrentUser.Id + DateTime.Now.Ticks;
            if (gateway == "alipay")
            {
                url = PaymentHelper.Alipay(deposit);
            }
            else if (gateway == "wechatpay")
            {
                url = PaymentHelper.Weixin(deposit);
            }
            if (string.IsNullOrEmpty(url))
            {
                return View();
            }
            return Redirect(url);
        }

        public ActionResult Withdraw()
        {
            var model = new UserWithdrawViewModel();
            model.User = CurrentUser;
            model.Withdraws = ur.GetWithdrawHistoryByUser(CurrentUser.Id);
            return View(model);
        }

        public ActionResult FinanceDetail(UserScoreCriteria criteria)
        {
            IEnumerable<UserScore> scores = null;
            criteria.RefUserId = CurrentUser.Id;
            decimal total = 0.00000M;
            if (!criteria.StartDate.HasValue)
            {
                criteria.StartDate = DateTime.Now.AddYears(-5);
            }

            if (!criteria.EndDate.HasValue)
            {
                criteria.EndDate = DateTime.Now.AddYears(5);
            }
            scores = ur.GetScoresByUser(criteria);

            if (criteria.TypeId > 0)
            {
                scores = scores.Where(s => s.TypeId == criteria.TypeId);
            }
            if (scores != null && scores.Any())
            {
                total = scores.Sum(s => s.Score);
            }
            ViewBag.TotalScore = total;
            return View(scores);
        }

        public ActionResult Entrust(MarketCriteria criteria)
        {
            var parameter = new DynamicParameters();
            var where = new StringBuilder();
            criteria.UserId = CurrentUser.Id;
            where.Append(" e.UserId = @userId ");
            parameter.Add("userId", CurrentUser.Id);
            if (criteria.StartDate.HasValue)
            {
                where.Append(" AND e.CreateTime >= @start");
                parameter.Add("start", criteria.StartDate.Value);
            }
            if (criteria.EndDate.HasValue)
            {
                where.Append(" AND e.CreateTime < @end");
                parameter.Add("end", criteria.EndDate.Value);
            }
            if(criteria.ProductId > 0)
            {
                where.Append(" AND e.ProductId = @pid");
                parameter.Add("pid", criteria.ProductId);
            }
            if(criteria.IsBuy.HasValue && criteria.IsBuy >-1)
            {
                where.Append(" AND e.IsBuy = @buy");
                parameter.Add("buy", criteria.IsBuy);
            }
            var pc = new PageCriteria
            {
                Where = where.ToString(),
                PageIndex = CurrentPageIndex,
                PageSize = PageSize,
                Parameter = parameter,
                Table = "Entrust e INNER JOIN Product p ON e.ProductId = p.Id",
                Order = "e.Id DESC"
            };

            //var entrusts = mr.GetEntrusts(criteria);
            var model = new EntrustRepository().GetPagedData<Entrust, Product, Entrust>(pc, (e, p) => { e.Product = p; return e; });
            ViewBag.Products = pr.GetAllProducts().Select(p => new SelectListItem { Text = p.ProductName, Value = p.Id.ToString() });
            return View(model);
        }

        public ActionResult TradeHistory(MarketCriteria criteria)
        {
            var parameter = new DynamicParameters();
            var where = new StringBuilder();
            criteria.UserId = CurrentUser.Id;
            where.Append(" e.UserId = @userId ");
            parameter.Add("userId", CurrentUser.Id);
            if (criteria.StartDate.HasValue)
            {
                where.Append(" AND e.CreateTime >= @start");
                parameter.Add("start", criteria.StartDate.Value);
            }
            if (criteria.EndDate.HasValue)
            {
                where.Append(" AND e.CreateTime < @end");
                parameter.Add("end", criteria.EndDate.Value);
            }
            if (criteria.ProductId > 0)
            {
                where.Append(" AND e.ProductId = @pid");
                parameter.Add("pid", criteria.ProductId);
            }
            if (criteria.IsBuy.HasValue && criteria.IsBuy > -1)
            {
                where.Append(" AND e.IsBuy = @buy");
                parameter.Add("buy", criteria.IsBuy);
            }
            var pc = new PageCriteria
            {
                Where = where.ToString(),
                PageIndex = CurrentPageIndex,
                PageSize = PageSize,
                Parameter = parameter,
                Table = "[Transaction] e INNER JOIN Product p ON e.ProductId = p.Id",
                Order = "e.Id DESC"
            };
            
            var model = new TransactionRepository().GetPagedData<Transaction, Product, Transaction>(pc, (t, p) => { t.Product = p; return t; });
            ViewBag.Products = pr.GetAllProducts().Select(p => new SelectListItem { Text = p.ProductName, Value = p.Id.ToString() });
            return View(model);
        }

        public ActionResult BorrowHistory()
        {
            var history = ur.GetBorrowHistoryByUser(CurrentUser.Id);
            return View(history);
        }

        [HttpPost]
        public ActionResult ReturnBorrow(int id)
        {
            var ubr = new UserBorrowRepository();
            var borrow = ubr.Get(id);
            if(borrow!=null)
            {
                var up = ur.GetProductByUser(borrow.ProductId, CurrentUser.Id);
                if(up == null || (up.TotalCount - up.FrozenCount) < borrow.BorrowCount)
                {
                    return Content("没有足够的产品可供归还");
                }
                borrow.Status = 1;
                borrow.ReturnTime = DateTime.Now;
                ubr.Update(borrow);
                //利息
                var interests = borrow.DailyInterest * borrow.BorrowCount * borrow.Price * (int)(DateTime.Today - borrow.CreateTime).TotalDays;

                CurrentUser.FrozenScore -= borrow.Bail;
                CurrentUser.TotalScore -= interests;
                ur.Update(CurrentUser);
                RefreshCurrentUser();

                up.TotalCount -= borrow.BorrowCount;
                up.UpdateTime = DateTime.Now;
                ur.UpdateUserProduct(up);
            }

            return Content("1");
        }

        public ActionResult Give()
        {
            ViewBag.Products = ur.GetProductsByUser(CurrentUser.Id).Where(p => (p.TotalCount - p.FrozenCount) > 0);
            return View(CurrentUser);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Give(UserGive give, string mcode)
        {
            var user = ur.Get(give.ReceiverId);
            if(user == null)
            {
                if(!ur.UserExists(Request.Form["ReceiverId"]))
                {
                    ModelState.AddModelError("give", "请确认接收对象");
                    return RedirectToAction("Give");
                }
            }

            if(!new SmsRepository().CheckSms(CurrentUser.Phone, mcode, "GIVE"))
            {
                ModelState.AddModelError("give", "请确认短信验证码");
                return RedirectToAction("Give");
            }

            var up = ur.GetProductByUser(give.ProductId, CurrentUser.Id);
            if(up == null || (up.TotalCount - up.FrozenCount) < give.Count)
            {
                ModelState.AddModelError("give", "请确认赠送数量");
                return RedirectToAction("Give");
            }

            up.TotalCount -= give.Count;
            up.UpdateTime = DateTime.Now;
            ur.UpdateUserProduct(up);

            var upTo = ur.GetProductByUser(give.ProductId, give.ReceiverId);
            if(upTo != null)
            {
                upTo.TotalCount += give.Count;
                upTo.UpdateTime = DateTime.Now;
            }
            else
            {
                upTo = new UserProduct
                {
                    ProductId = give.ProductId,
                    FrozenCount = 0,
                    TotalCount = give.Count,
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UserId = give.ReceiverId
                };
                ur.CreateUserProduct(upTo);
            }

            var m = mr.GetTodayProductMarket(give.ProductId);
            if (m != null)
            {
                var fee = m.CurrentPrice * give.Count;
                give.ChargeFee = fee;
                user.TotalScore -= fee;
                
                ur.Update(user);
            }
            give.CreateTime = DateTime.Now;
            give.Status = 1;
            give.UserId = CurrentUser.Id;
            new UserGiveRepository().Create(give);
            return RedirectToAction("GiveHistory");
        }

        public ActionResult GiveHistory()
        {
            var history = ur.GetGiveHistoryByUser(CurrentUser.Id);
            return View(history);
        }


        public ActionResult Certificated()
        {
            return View(CurrentUser);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Certificated(string name, string idnum)
        {
            if (string.IsNullOrEmpty(name))
            {
                ModelState.AddModelError("cert", "请输入姓名");
                return View(CurrentUser);
            }

            if (string.IsNullOrEmpty(idnum))
            {
                ModelState.AddModelError("cert", "请输入身份证号");
                return View(CurrentUser);
            }

            CurrentUser.DisplayName = name;
            CurrentUser.IdNum = idnum;
            ur.Update(CurrentUser);
            RefreshCurrentUser();
            return View(CurrentUser);
        }

        public ActionResult BindBankCard()
        {
            var bankAccount = CurrentUser.BankAccount;//ur.GetBankAccount(CurrentUser.Id);
            ViewBag.Name = CurrentUser.DisplayName;
            return View(bankAccount);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult BindBankCard(string bank, string accountNum)
        {
            var bankAccount = CurrentUser.BankAccount; //ur.GetBankAccount(CurrentUser.Id);
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
            RefreshCurrentUser();
            return RedirectToAction("BindBankCard");
        }

        #region 推广中心

        public ActionResult PopLink()
        {
            return View(CurrentUser);
        }

        public ActionResult PopDetail()
        {
            int uid = CurrentUser.Id;
            ViewBag.UserId = uid;
            var model = new PopDetailViewModel();
            model.DirectRefUsersCount = ur.GetDirectRefUsersCount(uid);
            model.IndirectRefUsersCount = ur.GetIndirectRefUsersCount(uid);
            model.RefUsers = ur.GetRefUsers(uid);
            return View(model);
        }

        public ActionResult PopReward(UserScoreCriteria criteria)
        {
            IEnumerable<UserScore> scores = null;
            decimal total = 0.00000M;
            if (!criteria.StartDate.HasValue)
            {
                criteria.StartDate = DateTime.Now.AddYears(-5);
            }

            if (!criteria.EndDate.HasValue)
            {
                criteria.EndDate = DateTime.Now.AddYears(5);
            }
            if (criteria.RefUserId > 0)
            {
                scores = ur.GetUserRewardScores(criteria);
            }
            else
            {
                criteria.RefUserId = CurrentUser.Id;
                scores = ur.GetRefUserRewardScores(criteria);
            }
            if (criteria.TypeId > 0)
            {
                scores = scores.Where(s => s.TypeId == criteria.TypeId);
            }
            if (scores != null && scores.Any())
            {
                total = scores.Sum(s => s.Score);
            }
            ViewBag.TotalScore = total;
            return View(scores);
        }

        public ActionResult FillReferId()
        {
            return View(CurrentUser);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult FillReferId(int referId)
        {
            var user = ur.Get(referId);
            if (user == null)
            {
                ModelState.AddModelError("refid", "推广码错误");
                return View(CurrentUser);
            }

            CurrentUser.RefUserId = referId;
            ur.Update(CurrentUser);
            return View(CurrentUser);
        }
        #endregion

    }
}