using GoldenFarm.Entity;
using GoldenFarm.Repository;
using GoldenFarm.Web.Controllers;
using GoldenFarm.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoldenFarm.Web.Areas.Admin.Controllers
{

    [CheckAdmin]
    public class MarketsController : BaseController
    {
        private MarketRepository mr = new MarketRepository();
        public ActionResult RebirthHistory()
        {
            var pc = new PageCriteria()
            {
                Table = "ProductRebirth r INNER JOIN Product p ON r.ProductId = p.Id",
                Order = "r.Id DESC",
                PageIndex = CurrentPageIndex,
                PageSize = PageSize
            };
            var model = new ProductRebirthRepository().GetPagedData<ProductRebirth, Product, ProductRebirth>(pc, (r, p) => { r.Product = p; return r; });
            return View(model);
        }
    }
}