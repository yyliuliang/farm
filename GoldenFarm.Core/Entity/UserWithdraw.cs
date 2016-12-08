using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("UserWithdraw")]
    [Serializable]
    public class UserWithdraw : EntityBase
    {
        public int UserId { get; set; }

        public string Bank { get; set; }

        public string AccountNum { get; set; }
        
        public string AccountName { get; set; }

        public decimal Amount { get; set; }


        public decimal ChargeFee { get; set; }


        public int Status { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
