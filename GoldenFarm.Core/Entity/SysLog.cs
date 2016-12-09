using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("SysLog")]
    [Serializable]
    public class SysLog : EntityBase
    {
        public string Log { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
