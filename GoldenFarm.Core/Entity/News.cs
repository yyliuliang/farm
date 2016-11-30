using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("News")]
    [Serializable]
    public class News : EntityBase
    {
        public string Title  { get; set; }

        public string Source { get; set; }

        public string Author { get; set; }

        public int ReadCount { get; set; }

        public string Content { get; set; }

        public DateTime Create { get; set; }
    }
}
