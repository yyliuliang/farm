using GoldenFarm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoldenFarm.Web.Controllers
{
    public abstract class BaseController : Controller
    {
       public User CurrentUser
        {
            get
            {
                return new User();
            }
        }
    }
}