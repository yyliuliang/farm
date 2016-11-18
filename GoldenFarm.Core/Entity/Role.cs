using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Serializable]
    public class Role : EntityBase
    {
        public string RoleName { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
