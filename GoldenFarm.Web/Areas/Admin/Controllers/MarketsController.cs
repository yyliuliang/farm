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
    public class MarketsController : BaseController
    {
        private MarketRepository mr = new MarketRepository();
        public ActionResult RebirthHistory(UserCriteria criteria)
        {
            StringBuilder where = new StringBuilder();
            DynamicParameters parameter = new DynamicParameters();
            int uid = 0;
            int.TryParse(criteria.UserId, out uid);
            where.Append(" 1=1 ");
            if(uid > 0)
            {
                where.Append(" AND r.UserId = @userId");
                parameter.Add("userId", uid);
            }
            if(criteria.StartDate.HasValue)
            {
                where.Append(" AND r.CreateTime >= @start");
                parameter.Add("start", criteria.StartDate.Value);
            }
            if (criteria.EndDate.HasValue)
            {
                where.Append(" AND r.CreateTime < @end");
                parameter.Add("end", criteria.EndDate.Value);
            }
            var pc = new PageCriteria()
            {
                Table = "ProductRebirth r INNER JOIN Product p ON r.ProductId = p.Id",
                Order = "r.Id DESC",
                Where = where.ToString(),
                Parameter = parameter,
                PageIndex = CurrentPageIndex,
                PageSize = PageSize
            };
            var model = new ProductRebirthRepository().GetPagedData<ProductRebirth, Product, ProductRebirth>(pc, (r, p) => { r.Product = p; return r; });
            return View(model);
        }
    }
}