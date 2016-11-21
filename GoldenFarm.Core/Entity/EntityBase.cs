using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace GoldenFarm.Entity
{
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
