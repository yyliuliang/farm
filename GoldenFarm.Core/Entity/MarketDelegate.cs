using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Serializable]
    public class MarketDelegate : EntityBase
    {
        public int UserId { get; set; }
        
        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public bool Buy { get; set; }

        /// <summary>
        /// 0: 未成交   1:已成交
        /// </summary>
        public int Status { get; set; }

        public bool Cancelled { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
