using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GoldenFarm.Entity
{

    public class User : EntityBase
    {
        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }

        public string Avatar { get; set; }

        public string LastLoginIP { get; set; }

        public DateTime LastLoginTime { get; set; }

        public DateTime CreateTime { get; set; }

        public bool Deleted { get; set; }
    }
}
