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

        public int CategoryId { get; set; }

        public string PreviewContent { get; set; }

        [Write(false)]
        public virtual NewsCategory Category { get; set; }

        public string Source { get; set; }

        public string Author { get; set; }

        public int ReadCount { get; set; }

        public string Content { get; set; }

        public int Creator { get; set; }

        public bool Deleted { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
