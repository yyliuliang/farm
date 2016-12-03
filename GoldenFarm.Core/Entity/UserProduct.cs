using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("UserProduct")]
    [Serializable]
    public class UserProduct : EntityBase
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }
                
        [Write(false)]
        public virtual Product Product { get; set; }

        public int TotalCount { get; set; }

        public int FrozenCount { get; set; }
        
        public DateTime UpdateTime { get; set; }

        public DateTime CreateTime {get;set;}


    }
}
