using GoldenFarm.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoldenFarm.Web.Filter
{
    public class CheckLogin : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = (BaseController)filterContext.Controller;
            if(controller.CurrentUser == null)
            {
                string path = filterContext.HttpContext.Request.Url.LocalPath;
                if (path.Equals("/User/Login", StringComparison.OrdinalIgnoreCase) || path.Equals("/", StringComparison.OrdinalIgnoreCase))
                {
                    filterContext.HttpContext.Response.Redirect("/User/Login");
                }
                else
                    filterContext.HttpContext.Response.Redirect("/User/Login?ReturnUrl=" + path);
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}