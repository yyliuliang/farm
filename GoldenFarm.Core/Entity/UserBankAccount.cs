using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("UserBankAccount")]
    [Serializable]
    public class UserBankAccount : EntityBase
    {
        public int UserId { get; set; }

        //[Write(false)]
        //public virtual User User { get; set; }

        public string Bank { get; set; }

        public string AccountNum { get; set; }

        public string AccountName { get; set; }


        public DateTime CreateTime { get; set; }

    }
}
