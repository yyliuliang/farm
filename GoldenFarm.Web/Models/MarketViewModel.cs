using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GoldenFarm.Entity;

namespace GoldenFarm.Web.Models
{
    public class MarketDetailViewModel
    {
        public User User { get; set; }
        
        public Market MarketDetail { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<UserProduct> UserProducts { get; set; }


        public IEnumerable<Entrust> CurrentEntrusts { get; set; }


        public IEnumerable<Entrust> HistoryEntrusts { get; set; }

    }
}