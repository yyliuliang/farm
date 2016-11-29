using GoldenFarm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace GoldenFarm.Repository
{
    public class UserRepository : RepositoryBase<User>
    {

        public bool UserExists(string username)
        {
            string sql = "SELECT TOP 1 * FROM User WHERE UserName = @name";
            return Conn.QueryFirst<User>(sql, new { name = username }) != null;
        }


        public bool Login(string username, string password)
        {
            string sql = "SELECT TOP 1 * FROM User WHERE UserName = @name AND Password = @password";
            return Conn.QueryFirst<User>(sql, new { name = username, password = password }) != null;
        }

    }
}
