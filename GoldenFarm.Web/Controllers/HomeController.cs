using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoldenFarm.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if(!string.IsNullOrEmpty(Request["ref"]))
            {
                Session["ref"] = Request["ref"];
            }
            return View();
        }
        
    }
}