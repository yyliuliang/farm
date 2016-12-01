using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace GoldenFarm.Web
{
    public class GFPagerOptions : PagerOptions
    {
        public GFPagerOptions()
        {
            this.AlwaysShowFirstLastPageNumber = false;
            this.CssClass = "pager";
            this.ShowPrevNext = false;
            this.ShowNumericPagerItems = true;
            this.ShowFirstLast = false;
            this.AutoHide = true;
            this.PageIndexParameterName = "page";
            this.AutoHide = false;
            this.CurrentPagerItemTemplate = "<a class=current>{0}</a>";
        }
    }
}