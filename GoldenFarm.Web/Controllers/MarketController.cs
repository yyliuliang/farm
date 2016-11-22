using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoldenFarm.Entity;
using GoldenFarm.Repository;
using GoldenFarm.Web.Models;

namespace GoldenFarm.Web.Controllers
{
    public class MarketController : BaseController
    {
        MarketRepository mr = new MarketRepository();
        ProductRepository pr = new ProductRepository();

        // GET: Market
        public ActionResult Index()
        {
            var markets = mr.GeTodayMarkets();
            return View(markets);

        }

        public ActionResult Detail(int id)
        {
            var model = new MarketDetailViewModel();
            model.MarketDetail = mr.GetTodayMarket(id);
            model.Products = pr.GetAllProducts();
            return View(model);

        }
    }
}