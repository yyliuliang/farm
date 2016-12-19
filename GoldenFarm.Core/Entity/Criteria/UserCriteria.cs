using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity.Criteria
{
    public class UserCriteria
    {
        public string UserId { get; set; }

        public string Phone { get; set; }

        public string DisplayName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
