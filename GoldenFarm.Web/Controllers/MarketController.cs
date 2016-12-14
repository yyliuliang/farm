﻿using System;
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
            var entrusts = mr.GetEntrusts(new MarketCriteria { UserId = CurrentUser.Id });
            model.MarketDetail = mr.GetTodayProductMarket(id);
            model.Products = pr.GetAllProducts();
            model.User = CurrentUser;
            model.UserProduct = ur.GetProductsByUser(CurrentUser.Id).FirstOrDefault(p => p.Product.ProductCode == id);
            model.CurrentEntrusts = entrusts.Where(e => !e.Cancelled);
            model.HistoryEntrusts = entrusts.Where(e => e.Cancelled);
            return View(model);
        }

        [CheckLogin]
        [HttpPost]
        public ActionResult Detail(string id, string submitType)
        {
            bool buy = false;
            DateTime start = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd 0:0:0"));
            DateTime end = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd 23:59:59"));

            switch (submitType)
            {
                case "BUY":
                    {
                        buy = true;
                        int count = int.Parse(Request.Form["BuyCount"]);
                        int pid = int.Parse(Request.Form["pid"]);
                        decimal price = decimal.Parse(Request.Form["BuyPrice"]);
                        var entrust = new Entrust()
                        {
                            IsBuy = buy,
                            Price = price,
                            Count = count,
                            DealedCount = 0,
                            UserId = CurrentUser.Id,
                            ProductId = pid,
                            CreateTime = DateTime.Now
                        };
                        
                        
                        int eid = mr.CreateEntrust(entrust);
                        decimal dealedAmount = 0M;

                        //冻结用户委托金额
                        CurrentUser.FrozenScore += (count * price);

                        var saleEntrusts = mr.GetEntrusts(new MarketCriteria { IsBuy = 0, ProductId = pid, Cancelled = 0, StartDate = start, EndDate = end })
                            .Where(e => e.Price <= price).OrderBy(e => e.Price).ThenByDescending(e => e.Id);

                        if (saleEntrusts != null && saleEntrusts.Any())
                        {
                            foreach(var e in saleEntrusts)
                            {
                                decimal amount = 0M;
                                // 80,  100 - 30 or 0
                                if((e.Count - e.DealedCount) >= (entrust.Count - entrust.DealedCount)) //此笔交易可完成
                                {
                                    amount = (entrust.Count - entrust.DealedCount) * e.Price;
                                    e.DealedAmount += (entrust.Count - entrust.DealedCount) * e.Price;
                                    dealedAmount += e.DealedAmount;
                                    e.DealedCount += (entrust.Count - entrust.DealedCount);
                                    entrust.DealedAmount += (entrust.Count - entrust.DealedCount) * e.Price;
                                    entrust.DealedCount = entrust.Count;                                    
                                    entrust.Status = 1;
                                    if (e.DealedCount == e.Count)
                                    {
                                        e.Status = 1;                                        
                                    }
                                    else
                                    {
                                        e.Status = 2;
                                    }

                                    mr.UpdateEntrust(e);
                                    break;
                                }
                                else //需更多笔交易
                                {
                                    amount = (e.Count - e.DealedCount) * e.Price;
                                    entrust.DealedAmount += (e.Count - e.DealedCount) * e.Price;
                                    dealedAmount += (e.Count - e.DealedCount) * e.Price;
                                    entrust.DealedCount += (e.Count - e.DealedCount);
                                    e.DealedAmount += (e.Count - e.DealedCount) * e.Price;
                                    e.DealedCount = e.Count;

                                    e.Status = 1;
                                    entrust.Status = 2;

                                    mr.UpdateEntrust(e);
                                }
                                var user = ur.Get(e.UserId);
                                if(user != null)
                                {
                                    user.TotalScore += amount;
                                    ur.Update(user);
                                }
                                var t1 = new Transaction
                                {

                                };
                                var t2 = new Transaction
                                {

                                };
                                mr.CreateTransaction(t1);
                                mr.CreateTransaction(t2);
                            }
                            mr.UpdateEntrust(entrust);
                        }
                        if(entrust.Status == 1)
                        {
                            CurrentUser.FrozenScore -= (count * price);
                        }
                        else
                        {
                            CurrentUser.FrozenScore -= dealedAmount;
                        }

                        CurrentUser.TotalScore -= dealedAmount;
                       
                        ur.Update(CurrentUser);
                        RefreshCurrentUser();
                    }
                    break;
                case "SALE":
                    {
                        int count = int.Parse(Request.Form["SaleCount"]);
                        int pid = int.Parse(Request.Form["pid"]);
                        decimal price = decimal.Parse(Request.Form["SalePrice"]);
                        var entrust = new Entrust()
                        {
                            IsBuy = buy,
                            Price = price,
                            Count = count,
                            DealedCount = 0,
                            UserId = CurrentUser.Id,
                            ProductId = pid,
                            CreateTime = DateTime.Now
                        };
                        //冻结用户委托产品
                        var up = ur.GetProductsByUser(CurrentUser.Id).First(p => p.Product.ProductCode == id);
                        up.FrozenCount += count;
                        ur.UpdateUserProduct(up);
                        int eid = mr.CreateEntrust(entrust);
                    }
                    break;
                case "REVOKE":
                    {
                        int eid = int.Parse(Request.Form["eid"]);
                        var e = mr.GetEntrustById(eid);
                        //解冻用户委托金额
                        if (e.IsBuy)
                        {
                            CurrentUser.FrozenScore -= (e.Count - e.DealedCount) * e.Price;
                            ur.Update(CurrentUser);
                            RefreshCurrentUser();
                        }
                        //冻结用户委托产品
                        else
                        {
                            var up = ur.GetProductsByUser(CurrentUser.Id).First(p => p.ProductId == e.ProductId);
                            up.FrozenCount += (e.Count - e.DealedCount);
                            ur.UpdateUserProduct(up);
                        }
                        mr.CancelEntrust(e);
                    }
                    break;
                default: break;
            }

            return RedirectToAction("Detail", new { id = id });
        }

        [CheckLogin]
        [HttpPost]
        public ActionResult FlushEntrust(string id)
        {
            int pid = int.Parse(Request.Form["pid"]);
            var market = mr.GetTodayProductMarket(id);
            //var buylist = mr.GetTop5Entrusts(pid, true);
            //var salelist = mr.GetTop5Entrusts(pid, false);

            return Json(new
            {
                MarketInfo = new
                {
                    ProductId = market.ProductId,
                    ProductName = market.Product.ProductName,
                    ProductCode = id,
                    IncreaseRate = market.RaisedRate,
                    Increase = market.Raised,
                    Price = market.CurrentPrice,
                    OpenPrice = market.OpenPrice,
                    HighestPrice = market.TopPrice,
                    LowestPrice = market.BottomPrice,
                    LimitUp = market.PrevDayPrice * 1.1M,
                    LimitDown = market.PrevDayPrice * 0.9M,
                    Volume = market.Volume
                },
                BuyList = mr.GetTop5Entrusts(pid, true),
                SaleList = mr.GetTop5Entrusts(pid, false)
            });
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

        [CheckLogin]
        [HttpPost]
        public ActionResult GetMyEntrusts()
        {
            return View();
        }

        [CheckLogin]
        [HttpPost]
        public ActionResult PostEntrust(Entrust entrust)
        {
            return View();
        }


        public ActionResult __PrepareTestData()
        {
            mr.PrepareMarketsTestData();
            mr.PrepareTransactionsTestData();
            return Content("ok");
        }
    }
}