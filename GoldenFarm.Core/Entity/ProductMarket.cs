using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Serializable]
    public class ProductMarket : EntityBase
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal PrevDayPrice { get; set; }

        public decimal OpenPrice { get; set; }

        public decimal ClosePrice { get; set; }

        public decimal TopPrice { get; set; }

        public decimal BottomPrice { get; set; }

        public int DealedTotalCount { get; set; }

        public DateTime Date { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
