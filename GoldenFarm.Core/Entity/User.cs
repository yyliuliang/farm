using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GoldenFarm.Entity
{
    [Serializable]
    [Table("[User]")]
    public class User : EntityBase
    {
       
        public Guid UserGuid { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        
        public string Phone { get; set; }

        public string Password { get; set; }

        public string Avatar { get; set; }

        public string LastLoginIP { get; set; }

        public DateTime LastLoginTime { get; set; }

        public DateTime CreateTime { get; set; }

        public decimal TotalScore { get; set; }

        public decimal FrozenScore { get; set; }

        public string IdNum { get; set; }

        public bool SmsGiveSwitch { get; set; }

        public int RefUserId { get; set; }

        public string RefUserPath { get; set; }

        [Write(false)]
        public virtual UserBankAccount BankAccount { get; set; }

        public bool Deleted { get; set; }
        
        public bool IsAdmin { get; set; }
    }
}
