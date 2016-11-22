using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GoldenFarm.Entity;

namespace GoldenFarm.Web.Models
{
    public class MarketDetailViewModel
    {
        
        public Market MarketDetail { get; set; }

        public IEnumerable<Product> Products { get; set; }

    }
}