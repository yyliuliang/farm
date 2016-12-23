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
    public class AdminController : BaseController
    {
        private NewsRepository nr = new NewsRepository();

        public ActionResult Index()
        {
            return View();
        }

        #region News
        public ActionResult News(NewsCriteria criteria)
        {
            StringBuilder where = new StringBuilder();
            DynamicParameters parameter = new DynamicParameters();
            where.Append(" n.Deleted = 0 ");
            if (criteria.CategoryId > 0)
            {
                where.Append(" AND n.CategoryId = @cid");
                parameter.Add("cid", criteria.CategoryId);
            }
            if(!string.IsNullOrEmpty(criteria.Title))
            {
                where.Append(" AND n.Title LIKE @title");
                parameter.Add("title", "%" + criteria.Title + "%");
            }
            if(criteria.StartDate.HasValue)
            {
                where.Append(" AND n.CreateTime >= @start");
                parameter.Add("start", criteria.StartDate.Value);
            }
            if (criteria.EndDate.HasValue)
            {
                where.Append(" AND n.CreateTime < @end");
                parameter.Add("end", criteria.EndDate.Value);
            }
            var pc = new PageCriteria
            {
                Table = "News n INNER JOIN NewsCategory c ON n.CategoryId = c.Id",
                Where = where.ToString(),
                PageIndex = CurrentPageIndex,
                Order = "n.Id DESC",
                PageSize = PageSize,
                Parameter = parameter
            };
            var model = nr.GetPagedData<News, NewsCategory, News>(pc, (n, c) => { n.Category = c; return n; });
            ViewBag.NewsCategories = nr.GetAllCategories().Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title });
            return View(model);
        }

        public ActionResult NewsDetail(int id)
        {
            var news = nr.Get(id) ?? new News();
            ViewBag.NewsCategories = nr.GetAllCategories().Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title, Selected = c.Id == news.CategoryId });
            return View(news);
        }

        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult NewsDetail(News news)
        {
            News origin = nr.Get(news.Id);

            if (origin == null)
            {
                origin = new News();
                origin.Creator = CurrentUser.Id;
                origin.CreateTime = DateTime.Now;
            }
            origin.Title = news.Title;
            origin.CategoryId = news.CategoryId;
            origin.PreviewContent = news.PreviewContent;
            origin.Content = news.Content;
            origin.Author = news.Author;
            origin.Source = news.Source;
            if(origin.Id > 0)
            {
                nr.Update(origin);
            }
            else
            {
                nr.Create(origin);
            }
            return RedirectToAction("News", new { id = origin.Id });
        }

        public ActionResult CreateNews()
        {
            var news = new News();
            ViewBag.NewsCategories = nr.GetAllCategories().Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title, Selected = c.Id == news.CategoryId });
            return View("NewsDetail", news);
        }

        [HttpPost]
        public ActionResult DeleteNews(int id)
        {
            var news = nr.Get(id);
            if(news != null)
            {
                news.Deleted = true;
                nr.Update(news);
            }
            return Content("1");
        }
        #endregion

    }
}