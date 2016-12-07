using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("UserBorrow")]
    public class UserBorrow : EntityBase
    {
        public int UserId { get; set; }
        
        public int ProductId { get; set; }

        [Write(false)]
        public virtual Product Product { get; set; }
                
        public int BorrowCount { get; set; }
        
        public decimal DailyInterest { get; set; }

        public decimal Bail { get; set; }

        public int Status { get; set; }

        public DateTime? ReturnTime { get; set; }

        public DateTime Deadline { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
