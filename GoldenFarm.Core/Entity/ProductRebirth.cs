using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("ProductRebirth")]
    public class ProductRebirth : EntityBase
    {
        public int ProductId { get; set; }

        [Write(false)]
        public virtual Product Product {get;set;}

        public int UserId { get; set; }

        public int RebirthCount { get; set; }

        public int SeedCount { get; set; }
        
        public decimal ChargeFee { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
