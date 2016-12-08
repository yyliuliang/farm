using GoldenFarm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Repository
{
    public class SmsRepository : RepositoryBase<Sms>
    {
        int expired = 10; //10 minutes
        public bool CheckSms(string phone, string code, string category = "Common")
        {

            return true;
        }
    }
}
