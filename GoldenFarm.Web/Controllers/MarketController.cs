using GoldenFarm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoldenFarm.Web.Controllers
{
    public class MarketController : BaseController
    {
        // GET: Market
        public ActionResult Index()
        {
            var markets = new List<ProductMarket>();
            return View(markets);
        }

        public ActionResult Detail()
        {
            return View();
        }
    }
}