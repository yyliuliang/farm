using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("Transaction")]
    [Serializable]
    public class Transaction : EntityBase
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
