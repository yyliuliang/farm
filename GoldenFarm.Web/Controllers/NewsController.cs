using GoldenFarm.Repository;
using GoldenFarm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoldenFarm.Web.Controllers
{
    public class NewsController : BaseController
    {
        private NewsRepository nr = new NewsRepository();
        // GET: News
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int id)
        {
            var news = nr.Get(id);
            ViewBag.Cid = news.CategoryId;
            news.ReadCount++;
            nr.Update(news);
            return View(news);
        }

        [ChildActionOnly]
        public ActionResult SideMenu()
        {
            var categories = nr.GetAllCategories();
            return PartialView("_NewsSideMenu", categories);
        }

        public ActionResult List(string category)
        {
            //var news = nr.GetNewsByCategory(category);
            //if(news!=null && news.Any() && news.First().Category != null)
            //{
            //    ViewBag.Cid = news.First().CategoryId;
            //}

           
            var criteria = new PageCriteria
            {
                PageSize = PageSize,
                PageIndex = CurrentPageIndex,
                Where = "Deleted = 0",
                Order = "ID DESC"
            };
            var model = nr.GetPagedData(criteria);           
            return View(model);
        }
    }
}