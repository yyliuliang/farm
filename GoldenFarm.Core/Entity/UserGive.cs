using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("UserGive")]
    [Serializable]
    public class UserGive : EntityBase
    {
        public int UserId { get; set; }


        public int ReceiverId { get; set; }


        [Write(false)]
        public virtual User Receiver { get; set; }


        public int ProductId { get; set; }

        [Write(false)]
        public virtual Product Product { get; set; }


        public int Count { get; set; }


        public int Status { get; set; }
        

        public decimal ChargeFee { get; set; }
        

        public DateTime CreateTime { get; set; }
    }
}
