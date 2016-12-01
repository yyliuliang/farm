using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("NewsCategory")]
    [Serializable]
    public class NewsCategory : EntityBase
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
