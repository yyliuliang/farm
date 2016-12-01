using GoldenFarm.Entity;
using GoldenFarm.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace GoldenFarm.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        Cache cache = HttpRuntime.Cache;
        private UserRepository ur = new UserRepository();

        public int CurrentPageIndex
        {
            get
            {
                int page = 1;
                if(!string.IsNullOrEmpty(Request["page"]))
                {
                    int.TryParse(Request["page"], out page);                    
                }
                return page;
            }
        }

        public int PageSize
        {
            get
            {
                return 20;
            }
        }

        [NonAction]
        protected void _Logout()
        {
            cache.Remove(LoginUserGuid.ToString());
            Response.Cookies["gyuid"].Value = null;
        }

        [NonAction]
        protected void _Login(User user, bool rememberMe)
        {
            DateTime date = rememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddHours(24);
            Response.Cookies["gyuid"].Value = user.UserGuid.ToString();
            Response.Cookies["gyuid"].Expires = date;
        }

        public User CurrentUser
        {
            get
            {
                User user = null;
                if(LoginUserGuid != Guid.Empty)
                {
                    if (cache[LoginUserGuid.ToString()] != null)
                    {
                        user = (User)cache[LoginUserGuid.ToString()];
                    }
                    if (user == null)
                    {
                        user = ur.GetByUserGuid(LoginUserGuid);
                        if (user != null)
                        {
                            cache.Insert(LoginUserGuid.ToString(), user);
                        }
                    }
                }
                
                return user;
            }
        }

        public Guid LoginUserGuid
        {
            get
            {
                if (Request.Cookies["gyuid"]!= null && !string.IsNullOrEmpty(Request.Cookies["gyuid"].Value))
                {
                    return new Guid(Request.Cookies["gyuid"].Value);
                }
                return Guid.Empty;
            }
        }
    }
}