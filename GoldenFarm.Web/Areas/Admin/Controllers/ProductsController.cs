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
            Product origin = pr.Get(product.Id);

            if (origin == null)
            {
                origin = new Product();
                origin.CreateTime = DateTime.Now;
            }
            origin.ProductName = product.ProductName;
            origin.ProductCode = product.ProductCode;
            origin.Description = product.Description;

            if (origin.Id > 0)
            {
                pr.Update(origin);
            }
            else
            {
                pr.Create(origin);
            }
            return RedirectToAction("Index");
        }
    }
}