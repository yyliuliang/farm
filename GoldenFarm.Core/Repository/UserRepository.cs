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
            return Conn.QuerySingleOrDefault<User>(sql, new { name = username }) != null;
        }


        public User Login(string username, string password)
        {
            string sql = @"SELECT TOP 1 * FROM [User] u LEFT JOIN [UserBankAccount] uba ON u.Id = uba.UserId
                           WHERE (UserName = @name OR Phone = @name) AND [Password] = @password AND Deleted = 0 ";
            var r = Conn.Query<User, UserBankAccount, User>(sql, (u, b) => { u.BankAccount = b; return u; }, new { name = username, password = password });
            return r != null && r.Any() ? r.FirstOrDefault() : null;
        }


        #region bank account

        public UserBankAccount GetBankAccount(int userId)
        {
            string sql = "SELECT TOP 1 * FROM [UserBankAccount] WHERE UserId = @userId";
            return Conn.QuerySingleOrDefault<UserBankAccount>(sql, new { userId = userId });
        }

        public void SaveBankAccount(UserBankAccount bankAccount)
        {
            if (bankAccount.Id == 0)
            {
                Conn.Insert<UserBankAccount>(bankAccount);
            }
            else
            {
                Conn.Update<UserBankAccount>(bankAccount);
            }
        }

        #endregion


        #region product

        public IEnumerable<UserProduct> GetProductsByUser(int id)
        {
            string sql = "SELECT * FROM [User] u INNER JOIN UserProduct p on u.Id = p.UserId WHERE u.Id = @id";
            return Conn.Query<UserProduct, Product, UserProduct>(sql, (u, p) => { u.Product = p; return u; }, new { id = id });
        }

        #endregion


        public int GetDirectRefUsersCount(int userId)
        {
            string sql = "SELECT COUNT(1) FROM [User] WHERE Deleted = 0 AND RefUserId = @userId";
            return Conn.QuerySingle<int>(sql, new { userId = userId });
        }

        public int GetIndirectRefUsersCount(int userId)
        {
            string sql = "SELECT COUNT(1) FROM [User] WHERE Deleted = 0 AND RefUserPath like @userId";
            return Conn.QuerySingle<int>(sql, new { userId = userId + "%" });
        }

        public IEnumerable<User> GetRefUsers(int userId)
        {
            string sql = "SELECT * FROM [User] WHERE Deleted = 0 AND RefUserPath like @userId";
            return Conn.Query<User>(sql, new { userId = userId + "%" });
        }

        public IEnumerable<UserScore> GetRefUserScores(UserScoreCriteria criteria)
        {
            string sql = @"SELECT * FROM UserScore us INNER JOIN [User] u on us.UserId = u.Id 
                                WHERE TypeId IN (4, 10, 20) AND us.UserPath LIKE @path AND us.CreateTime BETWEEN @start AND @end";

            return Conn.Query<UserScore, User, UserScore>(sql, (us, u) => { us.User = u; return us; }, new { path = criteria.RefUserId + "%", start = criteria.StartDate, end = criteria.EndDate });
        }

        public IEnumerable<UserScore> GetUserScores(UserScoreCriteria criteria)
        {
            string sql = @"SELECT * FROM UserScore us INNER JOIN [User] u on us.UserId = u.Id 
                                WHERE TypeId IN (4, 10, 20) AND us.UserId = @userId AND us.CreateTime BETWEEN @start AND @end";

            return Conn.Query<UserScore, User, UserScore>(sql, (us, u) => { us.User = u; return us; }, new { userId = criteria.RefUserId, start = criteria.StartDate, end = criteria.EndDate });
        }

    }
}
