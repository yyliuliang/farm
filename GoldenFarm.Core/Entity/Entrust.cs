using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("Entrust")]
    [Serializable]
    public class Entrust : EntityBase
    {
        public int UserId { get; set; }
        
        public int ProductId { get; set; }

        [Write(false)]
        public virtual Product Product { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public int DealedCount { get; set; }

        public bool IsBuy { get; set; }

        /// <summary>
        /// 0: 未成交   1:已成交  2: 部分成交   3: 部分撤销
        /// </summary>
        public int Status { get; set; }

        public bool Cancelled { get; set; }

        public DateTime CreateTime { get; set; }

        [Write(false)]
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case 0: return "未成交";
                    case 1: return "已成交";
                    case 2: return "部分成交";
                    case 3: return "部分撤销";
                    default: return "未知";
                }
            }
        }
    }


}
