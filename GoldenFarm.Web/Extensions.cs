using GoldenFarm.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoldenFarm.Web
{
    public static class Extensions
    {

        public static bool IsLogined(this HtmlHelper helper)
        {
            var controller = (BaseController)helper.ViewContext.Controller;
            return controller.CurrentUser != null;
        }
    }
}