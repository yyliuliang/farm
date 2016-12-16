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
    public class UsersController : BaseController
    {
        private UserRepository ur = new UserRepository();
        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int id)
        {
            var user = ur.Get(id);
            return View(user);
        }


        [HttpPost]
        public ActionResult Details(User user)
        {
            return RedirectToAction("Details", new { id = user.Id });
        }
    }
}