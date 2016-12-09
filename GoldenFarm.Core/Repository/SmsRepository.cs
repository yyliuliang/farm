using GoldenFarm.Entity;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Repository
{
    public class SmsRepository : RepositoryBase<Sms>
    {
        int expired = 60; //60 minutes
        public bool CheckSms(string phone, string code, string category = "Common")
        {
            string sql = @"SELECT TOP 1 * 
                           FROM Sms 
                           WHERE Phone = @phone AND Category = @category AND Code = @code AND DATEDIFF(MINUTE, CreateTime, GETDATE()) <= @minute 
                           ORDER BY Id DESC";
            var sms = Conn.QuerySingleOrDefault<Sms>(sql, new { phone = phone, category = category, code = code, minute = expired });
            return sms != null;
        }
    }
}
