using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    public class ResourceKeyValue : EntityBase
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
