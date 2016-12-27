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

                        #region 处理交易
                        var saleEntrusts = mr.GetEntrusts(new MarketCriteria { IsBuy = 0, ProductId = pid, Cancelled = 0, StartDate = start, EndDate = end })
                            .Where(e => e.Price <= price).OrderBy(e => e.Price).ThenByDescending(e => e.Id);

                        if (saleEntrusts != null && saleEntrusts.Any())
                        {
                            bool dealed = false;
                            foreach (var e in saleEntrusts)
                            {
                                if (dealed) break;
                                decimal amount = 0M;
                                Guid tid = Guid.NewGuid();
                                int acutalCount = 0;
                                // 80,  100 - 30 or 0
                                if ((e.Count - e.DealedCount) >= (entrust.Count - entrust.DealedCount)) //此笔交易可完成
                                {
                                    acutalCount = entrust.Count - entrust.DealedCount;
                                    amount = (acutalCount) * e.Price;
                                    e.DealedAmount += (acutalCount) * e.Price;
                                    dealedAmount += e.DealedAmount;
                                    e.DealedCount += (acutalCount);
                                    entrust.DealedAmount += (acutalCount) * e.Price;
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
                                    dealed = true;
                                   // break;
                                }
                                else //需更多笔交易
                                {
                                    acutalCount = e.Count - e.DealedCount;
                                    amount = (acutalCount) * e.Price;
                                    entrust.DealedAmount += acutalCount * e.Price;
                                    dealedAmount += acutalCount * e.Price;
                                    entrust.DealedCount += acutalCount;
                                    e.DealedAmount += acutalCount * e.Price;
                                    e.DealedCount = e.Count;

                                    e.Status = 1;
                                    entrust.Status = 2;

                                    mr.UpdateEntrust(e);
                                }
                                var user = ur.Get(e.UserId);
                                if (user != null)
                                {
                                    user.TotalScore += amount;
                                    ur.Update(user);
                                    var uup = ur.GetProductByUser(e.ProductId, e.UserId);
                                    uup.FrozenCount -= acutalCount;
                                    uup.TotalCount -= acutalCount;

                                    ur.UpdateUserProduct(uup);

                                    var score = new UserScore
                                    {
                                        TypeId = (int)ScoreType.出售,
                                        UserId = user.Id,
                                        ChargeFee = 0,
                                        Score = amount,
                                        CreateTime = DateTime.Now,
                                        Status = 1,
                                        UserPath = user.RefUserPath

                                    };
                                    new UserScoreRepository().Create(score);
                                }
                                

                                var tbuy = new Transaction
                                {
                                    Count = acutalCount,
                                    IsBuy = true,
                                    UserId = entrust.UserId,
                                    CreateTime = DateTime.Now,
                                    Date = DateTime.Today,
                                    Price = e.Price,
                                    ProductId = e.ProductId,
                                    ChargeFee = 0,
                                    TransactionId = tid
                                };
                                var tsell = new Transaction
                                {
                                    Count = acutalCount,
                                    IsBuy = false,
                                    UserId = e.UserId,
                                    CreateTime = DateTime.Now,
                                    Date = DateTime.Today,
                                    Price = e.Price,
                                    ProductId = e.ProductId,
                                    ChargeFee = 0,
                                    TransactionId = tid
                                };
                                mr.CreateTransaction(tbuy);
                                mr.CreateTransaction(tsell);
                            }
                            mr.UpdateEntrust(entrust);
                        }
                        if (entrust.Status == 1)
                        {
                            CurrentUser.FrozenScore -= (count * price);
                        }
                        else
                        {
                            CurrentUser.FrozenScore -= dealedAmount;
                        }
                        var up = ur.GetProductByUser(entrust.ProductId, CurrentUser.Id);
                        if (up == null)
                        {
                            up = new UserProduct
                            {
                                ProductId = entrust.ProductId,
                                FrozenCount = 0,
                                UserId = CurrentUser.Id,
                                TotalCount = entrust.DealedCount,
                                CreateTime = DateTime.Now,
                                UpdateTime = DateTime.Now
                            };
                            ur.CreateUserProduct(up);
                        }
                        else
                        {
                            up.TotalCount += entrust.DealedCount;
                            up.UpdateTime = DateTime.Now;
                            ur.UpdateUserProduct(up);
                        }
                        #endregion

                        CurrentUser.TotalScore -= dealedAmount;
                        ur.Update(CurrentUser);
                        RefreshCurrentUser();

                        if (dealedAmount > 0)
                        {
                            var score = new UserScore
                            {
                                TypeId = (int)ScoreType.购买,
                                UserId = CurrentUser.Id,
                                ChargeFee = 0,
                                Score = dealedAmount,
                                CreateTime = DateTime.Now,
                                Status = 1,
                                UserPath = CurrentUser.RefUserPath
                            };
                            new UserScoreRepository().Create(score);
                        }
                    }
                    break;
                case "SALE":
                    {
                        int count = int.Parse(Request.Form["SaleCount"]);
                        int pid = int.Parse(Request.Form["pid"]);
                        decimal price = decimal.Parse(Request.Form["SalePrice"]);
                        int dealedCount = 0;
                        decimal dealedAmount = 0M;
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

                        int eid = mr.CreateEntrust(entrust);

                        #region 处理交易

                        var buyEntrusts = mr.GetEntrusts(new MarketCriteria { IsBuy = 1, ProductId = pid, Cancelled = 0, StartDate = start, EndDate = end })
                            .Where(e => e.Price >= price).OrderByDescending(e => e.Price).ThenByDescending(e => e.Id);

                        if (buyEntrusts != null && buyEntrusts.Any())
                        {
                            bool dealed = false;
                            foreach (var e in buyEntrusts)
                            {
                                if (dealed) break;
                                decimal amount = 0M;
                                Guid tid = Guid.NewGuid();
                                int acutalCount = 0;
                                // 80,  100 - 30 or 0
                                if ((e.Count - e.DealedCount) >= (entrust.Count - entrust.DealedCount)) //此笔交易可完成
                                {
                                    acutalCount = entrust.Count - entrust.DealedCount;
                                    amount = (acutalCount) * e.Price;
                                    e.DealedAmount += (acutalCount) * e.Price;
                                    dealedCount += acutalCount;
                                    e.DealedCount += (acutalCount);
                                    entrust.DealedAmount += (acutalCount) * e.Price;
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
                                    dealedAmount += amount;
                                    mr.UpdateEntrust(e);
                                    dealed = true;
                                    //break;
                                }
                                else //需更多笔交易
                                {
                                    acutalCount = e.Count - e.DealedCount;
                                    amount = (acutalCount) * e.Price;
                                    entrust.DealedAmount += acutalCount * e.Price;
                                    dealedCount += acutalCount;
                                    entrust.DealedCount += acutalCount;
                                    e.DealedAmount += acutalCount * e.Price;
                                    e.DealedCount = e.Count;

                                    e.Status = 1;
                                    entrust.Status = 2;

                                    mr.UpdateEntrust(e);
                                    dealedAmount += amount;
                                }
                                var user = ur.Get(e.UserId);
                                if (user != null)
                                {
                                    user.FrozenScore -= amount;
                                    user.TotalScore -= amount;
                                    ur.Update(user);
                                    var uup = ur.GetProductByUser(e.ProductId, e.UserId);
                                    //uup.FrozenCount -= acutalCount;
                                    uup.TotalCount += acutalCount;
                                    ur.UpdateUserProduct(uup);

                                    var score = new UserScore
                                    {
                                        TypeId = (int)ScoreType.购买,
                                        UserId = user.Id,
                                        ChargeFee = 0,
                                        Score = amount,
                                        CreateTime = DateTime.Now,
                                        Status = 1,
                                        UserPath = user.RefUserPath

                                    };
                                    new UserScoreRepository().Create(score);
                                }
                                var tbuy = new Transaction
                                {
                                    Count = acutalCount,
                                    IsBuy = true,
                                    UserId = e.UserId,
                                    CreateTime = DateTime.Now,
                                    Date = DateTime.Today,
                                    Price = e.Price,
                                    ProductId = e.ProductId,
                                    ChargeFee = 0,
                                    TransactionId = tid
                                };
                                var tsell = new Transaction
                                {
                                    Count = acutalCount,
                                    IsBuy = false,
                                    UserId = entrust.UserId,
                                    CreateTime = DateTime.Now,
                                    Date = DateTime.Today,
                                    Price = e.Price,
                                    ProductId = e.ProductId,
                                    ChargeFee = 0,
                                    TransactionId = tid
                                };
                                mr.CreateTransaction(tbuy);
                                mr.CreateTransaction(tsell);
                            }
                            mr.UpdateEntrust(entrust);
                        }
                        
                        #endregion
                        if (dealedCount > 0)
                        {
                            var score = new UserScore
                            {
                                TypeId = (int)ScoreType.出售,
                                UserId = CurrentUser.Id,
                                ChargeFee = 0,
                                Score = dealedAmount,
                                CreateTime = DateTime.Now,
                                Status = 1,
                                UserPath = CurrentUser.RefUserPath

                            };
                            new UserScoreRepository().Create(score);
                            up.FrozenCount -= dealedCount;
                            up.TotalCount -= dealedCount;
                            ur.UpdateUserProduct(up);
                            ur.Update(CurrentUser);
                            RefreshCurrentUser();
                        }
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
                        if (Request.IsAjaxRequest())
                        {
                            return Content("1");
                        }
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
            ViewBag.Products = mr.GetLatestMarkets().Where(m => m.ProductId != 800001);
            return View(CurrentUser);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Borrow(UserBorrow borrow)
        {
            borrow.UserId = CurrentUser.Id;
            borrow.CreateTime = DateTime.Now;
            borrow.Bail = borrow.BorrowCount * borrow.Price * 2;
            borrow.DailyInterest = 0.02M;
            borrow.Deadline = DateTime.Today.AddMonths(1);

            //创建租借记录
            new UserBorrowRepository().Create(borrow);

            //修改当前用户的冻结金额
            CurrentUser.FrozenScore += borrow.Bail;
            ur.Update(CurrentUser);
            RefreshCurrentUser();

            //修改用户的产品数量
            var up = ur.GetProductByUser(borrow.ProductId, CurrentUser.Id);
            if (up != null)
            {
                up.TotalCount += borrow.BorrowCount;
                up.UpdateTime = DateTime.Now;
                ur.UpdateUserProduct(up);
            }
            else
            {
                up = new UserProduct
                {
                    ProductId = borrow.ProductId,
                    UserId = CurrentUser.Id,
                    TotalCount = borrow.BorrowCount,
                    FrozenCount = 0,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                ur.CreateUserProduct(up);
            }

            var score = new UserScore
            {
                UserId = CurrentUser.Id,
                Score = borrow.Bail,
                Status = 1,
                TypeId = (int)ScoreType.果实租借,
                CreateTime = DateTime.Now,
                ChargeFee = 0
            };
            new UserScoreRepository().Create(score);

            return RedirectToAction("BorrowHistory", "User");
        }

        public ActionResult Rebirth()
        {
            var products = ur.GetProductsByUser(CurrentUser.Id).Where(p => p.TotalCount > 0 && p.ProductId != 800008 && p.ProductId != 800001); //玫瑰不能重生
            return View(products);
        }

        [HttpPost]
        public ActionResult Rebirth(int pid, int count)
        {
            var product = ur.GetProductByUser(pid, CurrentUser.Id);
            int seedId = 800001;
            if (product != null)
            {
                product.TotalCount -= count;
                product.UpdateTime = DateTime.Now;
                ur.UpdateUserProduct(product);

                var seed = ur.GetProductByUser(seedId, CurrentUser.Id);
                if (seed == null || seed.Id != seedId)
                {
                    seed = new UserProduct()
                    {
                        ProductId = seedId,
                        TotalCount = count,
                        FrozenCount = 0,
                        UserId = CurrentUser.Id,
                        UpdateTime = DateTime.Now,
                        CreateTime = DateTime.Now
                    };
                    ur.CreateUserProduct(seed);
                }
                else
                {
                    seed.TotalCount += count;
                    seed.UpdateTime = DateTime.Now;
                    ur.UpdateUserProduct(seed);
                }
                var pr = new ProductRebirth
                {
                    ProductId = pid,
                    UserId = CurrentUser.Id,
                    RebirthCount = count,
                    SeedCount = count,
                    ChargeFee = 0,
                    CreateTime = DateTime.Now
                };
                new ProductRebirthRepository().Create(pr);
                var m = mr.GetTodayProductMarket(pid);
                var score = new UserScore
                {
                    TypeId = (int)ScoreType.果实重生,
                    UserId = CurrentUser.Id,
                    Status = 1,
                    ChargeFee = 0,
                    CreateTime = DateTime.Now,
                    Score = m.CurrentPrice * count
                };
                new UserScoreRepository().Create(score);
            }

            return Content("1");
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