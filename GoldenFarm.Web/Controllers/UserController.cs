using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoldenFarm.Web.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Reg()
        {
            return View();
        }

        public ActionResult FindPassword()
        {
            return View();
        }

        public ActionResult Safe()
        {
            return View();
        }

        public ActionResult Wallet()
        {
            return View();
        }

        public ActionResult Deposit()
        {
            return View();
        }

        public ActionResult Withdraw()
        {
            return View();
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