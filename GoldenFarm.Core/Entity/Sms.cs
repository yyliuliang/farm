using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("Sms")]
    [Serializable]
    public class Sms : EntityBase
    {
        public string Phone { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }


        public int Sender { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
