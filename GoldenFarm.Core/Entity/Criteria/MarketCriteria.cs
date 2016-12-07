using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    public class MarketCriteria
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }


        public int IsBuy { get; set; }


        public DateTime? StartDate { get; set; }


        public DateTime? EndDate { get; set; }
    }
}
