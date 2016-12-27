using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("[Transaction]")]
    [Serializable]
    public class Transaction : EntityBase
    {
        public Guid TransactionId { get; set; }

        public int ProductId { get; set; }

        [Write(false)]
        public virtual Product Product { get; set; }

        public int UserId { get; set; }

        public bool IsBuy { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }


        public decimal ChargeFee { get; set; }

        public DateTime Date { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
