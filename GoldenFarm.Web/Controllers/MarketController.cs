using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoldenFarm.Entity;
using GoldenFarm.Repository;
using GoldenFarm.Web.Models;
using GoldenFarm.Web.Filter;

namespace GoldenFarm.Web.Controllers
{
    public class MarketController : BaseController
    {
        MarketRepository mr = new MarketRepository();
        ProductRepository pr = new ProductRepository();
        UserRepository ur = new UserRepository();

        // GET: Market
        public ActionResult Index()
        {
            var markets = mr.GeTodayMarkets();
            return View(markets);

        }

        [CheckLogin]
        public ActionResult Detail(string id)
        {
            var model = new MarketDetailViewModel();
            model.MarketDetail = mr.GetTodayProductMarket(id);
            model.Products = pr.GetAllProducts();
            return View(model);
        }

        [HttpPost]
        public ActionResult Minute(string id)
        {
            var currentMarket = mr.GetTodayProductMarket(id);
            var transactions = mr.GetTodayTransactions(id).OrderBy(t => t.CreateTime.ToTimestamp());
            var data = new
            {
                SysDT = DateTime.Now.ToTimestamp(),
                TimeShare = from t in transactions
                            select new
                            {
                                Time = t.CreateTime.ToTimestamp(),
                                Price = t.Price,
                                Volume = t.Count
                            },
                MarketInfo = new
                {
                    OpenPrice = currentMarket.OpenPrice,
                    LimitUp = transactions.Max(m => m.Price),
                    LimitDown = transactions.Min(m => m.Price),
                }

            };
            return Json(new { data = data });
        }

        [HttpPost]
        public ActionResult Candle(string id)
        {
            var markets = mr.GetProductMarkets(id).OrderBy(m => m.Date.ToTimestamp());
            var currentMarket = mr.GetTodayProductMarket(id);
            var data = new
            {
                KLineList = from m in markets
                            select new
                            {
                                Date = m.Date.ToTimestamp(),
                                OpeningPrice = m.OpenPrice,
                                ClosingPrice = m.ClosePrice,
                                HighestPrice = m.TopPrice,
                                LowestPrice = m.BottomPrice,
                                Volume = m.Volume,
                                ChangePrice = m.Raised,
                                ChangeRate = m.RaisedRate
                            },
                MarketInfo = new
                {
                    OpenPrice = currentMarket.OpenPrice,
                    LimitUp = markets.Max(m => m.TopPrice),
                    LimitDown = markets.Min(m => m.BottomPrice),
                }
            };
            return Json(new { data = data });
        }


        public ActionResult TradeCenter()
        {
            var markets = mr.GeTodayMarkets();
            return View(markets);
        }

        public ActionResult Borrow()
        {
            return View(CurrentUser);
        }

        public ActionResult Rebirth()
        {
            var products = ur.GetProductsByUser(CurrentUser.Id);
            return View(products);
        }


        public ActionResult RebirthHistory()
        {
            var history = ur.GetRebirthHistoryByUser(CurrentUser.Id);
            return View(history);
        }


        public ActionResult __PrepareTestData()
        {
            mr.PrepareMarketsTestData();
            mr.PrepareTransactionsTestData();
            return Content("ok");
        }
    }
}