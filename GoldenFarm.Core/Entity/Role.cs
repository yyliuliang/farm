using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("Role")]
    [Serializable]
    public class Role : EntityBase
    {
        public string RoleName { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
