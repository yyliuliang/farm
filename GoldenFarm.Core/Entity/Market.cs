using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace GoldenFarm.Entity
{

    [Serializable]
    public class Market : EntityBase
    {
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
        
        public decimal CurrentPrice { get; set; }

        public decimal PrevDayPrice { get; set; }

        public decimal OpenPrice { get; set; }

        public decimal ClosePrice { get; set; }

        public decimal TopPrice { get; set; }

        public decimal BottomPrice { get; set; }

        public int Volume { get; set; }

        public DateTime Date { get; set; }

        public DateTime CreateTime { get; set; }

        
        [Write(false)]
        public decimal Raised
        {
            get
            {
                return CurrentPrice - PrevDayPrice;
            }
        }

        [Write(false)]
        public string RaisedRate
        {
            get
            {
                if (OpenPrice == 0) return "100";
                return ((Raised / OpenPrice) * 100).ToString("f2");
            }
        }

    }
}
