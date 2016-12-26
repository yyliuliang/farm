using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Table("UserScore")]
    [Serializable]
    public class UserScore : EntityBase
    {
        public int UserId { get; set; }


        [Write(false)]
        public virtual User User { get; set; }

        
        public string UserPath { get; set; }


        public int TypeId { get; set; }


        public string Num { get; set; }


        public decimal Score { get; set; }


        public decimal ChargeFee { get; set; }


        public int Status { get; set; }


        public string Description { get; set; }


        public DateTime CreateTime { get; set; }

    }

    public enum ScoreType
    {
        充值 = 2,
        提现 = 5,
        购买 = 3,
        出售 = 7,
        推广奖励_交易 = 4,
        推广奖励_游戏 = 10,
        推广奖励_首充 = 20,
        机构奖励_交易 = 14,
        机构奖励_游戏 = 15,
        机构奖励_首充 = 21,
        机构入驻费 = 17,
        机构入驻退费 = 18,
        钻石宝箱 = 6,
        果实重生 = 8,
        游戏消费 = 9,
        果实租借 = 11,
        果实归还 = 12,
        补充保证金 = 13,
        系统奖励 = 16,
        赠送手续费 = 19
    }
}
