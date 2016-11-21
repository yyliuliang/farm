using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoldenFarm.Entity;
using GoldenFarm.Repository;

namespace GoldenFarm.Web.Controllers
{
    public class MarketController : BaseController
    {
        // GET: Market
        public ActionResult Index()
        {
            using (var r = new MarketRepository())
            {
                var markets = r.GetAll();
                return View(markets);
            }
        }

        public ActionResult Detail()
        {
            return View();
        }
    }
}