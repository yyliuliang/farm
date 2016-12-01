using GoldenFarm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm
{
    public class PageViewData<TEntity> where TEntity : EntityBase
    {
        public int TotalCount { get; set; }

        public IEnumerable<TEntity> Items { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
