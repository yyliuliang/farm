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
    public class ProductsController : BaseController
    {
        private ProductRepository pr = new ProductRepository();

        // GET: Admin/Product
        public ActionResult Index()
        {
            var products = pr.GetAllProducts();
            return View(products);
        }

        public ActionResult Detail(int id)
        {
            var product = pr.Get(id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Detail(Product product)
        {
            return RedirectToAction("Detail", new { id = product.Id });
        }
    }
}