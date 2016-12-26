using Dapper;
using GoldenFarm.Entity;
using GoldenFarm.Entity.Criteria;
using GoldenFarm.Repository;
using GoldenFarm.Web.Controllers;
using GoldenFarm.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GoldenFarm.Web.Areas.Admin.Controllers
{

    [CheckAdmin]
    public class UsersController : BaseController
    {
        private UserRepository ur = new UserRepository();
        private MarketRepository mr = new MarketRepository();
        private ProductRepository pr = new ProductRepository();

        // GET: Admin/User
        public ActionResult Index(UserCriteria uc)
        {
            StringBuilder where = new StringBuilder();
            DynamicParameters parameter = new DynamicParameters();
            where.Append(" Deleted = 0 ");
            if(!string.IsNullOrEmpty(uc.UserId))
            {
                where.Append(" AND Id = @id");
                parameter.Add("id", uc.UserId);
            }
            if(!string.IsNullOrEmpty(uc.Phone))
            {
                where.Append(" AND (Phone = @name OR DisplayName = @name)");
                parameter.Add("name", uc.Phone);
            }
            if (uc.StartDate.HasValue)
            {
                where.Append(" AND CreateTime >= @start");
                parameter.Add("start", uc.StartDate.Value);
            }
            if (uc.EndDate.HasValue)
            {
                where.Append(" AND CreateTime < @end");
                parameter.Add("end", uc.EndDate.Value);
            }
            var criteria = new PageCriteria
            {
                PageSize = PageSize,
                PageIndex = CurrentPageIndex,
                Where = where.ToString(),
                Order = "Deleted ASC, ID DESC",
                Parameter = parameter
            };
            var model = ur.GetPagedData(criteria);
            return View(model);
        }

        public ActionResult Detail(int id)
        {
            var user = ur.Get(id);
            ViewBag.Products = ur.GetProductsByUser(id);
            return View(user);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Detail(User user)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var user = ur.Get(id);
            if (user != null)
            {
                ur.Delete(user);
            }
            return Content("1");
            ///return RedirectToAction("Details", new { id = user.Id });
        }


        public ActionResult Entrust(MarketCriteria criteria)
        {
            var where = new StringBuilder();
            var parameter = new DynamicParameters();
           
            where.Append(" 1 = 1 ");
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
            if (criteria.IsBuy > -1)
            {
                where.Append(" AND e.IsBuy = @buy");
                parameter.Add("buy", criteria.IsBuy);
            }
            var pc = new PageCriteria
            {
                Order = "e.Id DESC",
                Where = where.ToString(),
                Parameter = parameter,
                PageIndex = CurrentPageIndex,
                Table = "Entrust e INNER JOIN Product p on e.ProductId = p.Id", 
                PageSize = PageSize
            };
            var model = new EntrustRepository().GetPagedData<Entrust, Product, Entrust>(pc, (e, p) => { e.Product = p; return e; });
            ViewBag.Products = pr.GetAllProducts().Select(p => new SelectListItem { Text = p.ProductName, Value = p.Id.ToString() });
            return View(model);
        }

        public ActionResult TradeHistory(MarketCriteria criteria)
        {
            var where = new StringBuilder();
            var parameter = new DynamicParameters();

            where.Append(" 1 = 1 ");
            if (criteria.StartDate.HasValue)
            {
                where.Append(" AND t.CreateTime >= @start");
                parameter.Add("start", criteria.StartDate.Value);
            }
            if (criteria.EndDate.HasValue)
            {
                where.Append(" AND t.CreateTime < @end");
                parameter.Add("end", criteria.EndDate.Value);
            }
            if (criteria.ProductId > 0)
            {
                where.Append(" AND t.ProductId = @pid");
                parameter.Add("pid", criteria.ProductId);
            }
            if (criteria.IsBuy > -1)
            {
                where.Append(" AND t.IsBuy = @buy");
                parameter.Add("buy", criteria.IsBuy);
            }
            var pc = new PageCriteria
            {
                Order = "t.Id DESC",
                Where = where.ToString(),
                Parameter = parameter,
                PageIndex = CurrentPageIndex,
                Table = "[Transaction] t INNER JOIN Product p on t.ProductId = p.Id",
                PageSize = PageSize
            };
            var model = new TransactionRepository().GetPagedData<Transaction, Product, Transaction>(pc, (t, p) => { t.Product = p; return t; });
            ViewBag.Products = pr.GetAllProducts().Select(p => new SelectListItem { Text = p.ProductName, Value = p.Id.ToString() });
            return View(model);
        }


        public ActionResult BorrowHistory(UserCriteria criteria)
        {
            StringBuilder where = new StringBuilder();
            DynamicParameters parameter = new DynamicParameters();
            int uid = 0;
            int.TryParse(criteria.UserId, out uid);
            where.Append(" 1=1 ");
            if (uid > 0)
            {
                where.Append(" AND u.UserId = @userId");
                parameter.Add("userId", uid);
            }
            if (criteria.StartDate.HasValue)
            {
                where.Append(" AND u.CreateTime >= @start");
                parameter.Add("start", criteria.StartDate.Value);
            }
            if (criteria.EndDate.HasValue)
            {
                where.Append(" AND u.CreateTime < @end");
                parameter.Add("end", criteria.EndDate.Value);
            }
            var pc = new PageCriteria()
            {
                Table = "UserBorrow u INNER JOIN Product p ON u.ProductId = p.Id",
                Order = "u.Id DESC",
                Where = where.ToString(),
                Parameter = parameter,
                PageIndex = CurrentPageIndex,
                PageSize = PageSize
            };
            var model = new UserBorrowRepository().GetPagedData<UserBorrow, Product, UserBorrow>(pc, (u, p) => { u.Product = p; return u; });
            return View(model);
        }


        public ActionResult GiveHistory(UserCriteria criteria)
        {
            StringBuilder where = new StringBuilder();
            DynamicParameters parameter = new DynamicParameters();
            int uid = 0;
            int.TryParse(criteria.UserId, out uid);
            where.Append(" 1=1 ");
            if (uid > 0)
            {
                where.Append(" AND u.UserId = @userId");
                parameter.Add("userId", uid);
            }
            if (criteria.StartDate.HasValue)
            {
                where.Append(" AND u.CreateTime >= @start");
                parameter.Add("start", criteria.StartDate.Value);
            }
            if (criteria.EndDate.HasValue)
            {
                where.Append(" AND u.CreateTime < @end");
                parameter.Add("end", criteria.EndDate.Value);
            }
            var pc = new PageCriteria()
            {
                Table = "UserGive u INNER JOIN Product p ON u.ProductId = p.Id",
                Order = "u.Id DESC",
                Where = where.ToString(),
                Parameter = parameter,
                PageIndex = CurrentPageIndex,
                PageSize = PageSize
            };
            var model = new UserGiveRepository().GetPagedData<UserGive, Product, UserGive>(pc, (u, p) => { u.Product = p; return u; });
            return View(model);
        }


    }
}