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

        public User GetByUserGuid(Guid id)
        {
            string sql = "SELECT TOP 1 * FROM [User] WHERE UserGuid = @id AND Deleted=0";
            return Conn.QueryFirstOrDefault<User>(sql, new { id = id });
        }

        public bool UserExists(string username)
        {
            string sql = "SELECT TOP 1 * FROM [User] WHERE (UserName = @name OR Phone = @name) AND Deleted = 0";
            return Conn.QueryFirstOrDefault<User>(sql, new { name = username }) != null;
        }


        public User Login(string username, string password)
        {
            string sql = "SELECT TOP 1 * FROM [User] WHERE (UserName = @name OR Phone = @name) AND [Password] = @password AND Deleted = 0";
            return Conn.QueryFirstOrDefault<User>(sql, new { name = username, password = password });
        }



        #region product

        public IEnumerable<UserProduct> GetProductsByUser(int id)
        {
            string sql = "SELECT * FROM [User] u INNER JOIN UserProduct p on u.Id = p.UserId WHERE u.Id = @id";
            return Conn.Query<UserProduct, Product, UserProduct>(sql, (u, p) => { u.Product = p; return u; }, new { id = id });
        }

        #endregion

    }
}
