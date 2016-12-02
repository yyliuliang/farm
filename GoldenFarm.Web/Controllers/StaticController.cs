using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoldenFarm.Web.Controllers
{
    public class StaticController : BaseController
    {

        #region about
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Join()
        {
            return View();
        }

        public ActionResult Agreement()
        {
            return View();
        }

        public ActionResult Borrow()
        {
            return View();
        }

        public ActionResult Law()
        {
            return View();
        }

        public ActionResult Charge()
        {
            return View();
        }
        #endregion

        #region faq
        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult Guide()
        {
            return View();
        }

        public ActionResult Service()
        {
            return View();
        }
        #endregion

        #region strategy

        public ActionResult Farm()
        {
            return View();
        }

        public ActionResult Fruit()
        {
            return View();
        }

        public ActionResult Land()
        {
            return View();
        }

        public ActionResult Prop()
        {
            return View();
        }

        public ActionResult Joss()
        {
            return View();
        }

        public ActionResult FaqForFarm()
        {
            return View();
        }

        public ActionResult GuideForFarm()
        {
            return View();
        }

        public ActionResult Video()
        {
            return View();
        }
        #endregion
    }
}