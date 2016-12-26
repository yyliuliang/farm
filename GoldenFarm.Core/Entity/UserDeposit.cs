using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Serializable]
    [Table("UserDeposit")]
    public class UserDeposit : EntityBase
    {
        public int UserId { get; set; }

        public string FlowNum { get; set; }

        public decimal Amount { get; set; }

        public string Gateway { get; set; }
        
        public string GatewayOrderNum { get; set; }


        public string Params { get; set; }
        
        public int Status { get; set; }

        public string IP { get; set; }
        
        public DateTime CreateTime { get; set; }
    }
}
