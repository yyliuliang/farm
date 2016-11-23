using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Serializable]
    public class MarketTransaction : EntityBase
    {
        public Guid TransactionId { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int BuyerId { get; set; }

        public int SellerId { get; set; }

        public int Volume { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
