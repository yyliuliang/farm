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
    public class UserBorrowRepository : RepositoryBase<UserBorrow> { }

    public class UserGiveRepository : RepositoryBase<UserGive> { }

    public class UserScoreRepository : RepositoryBase<UserScore> { }

    public class UserDepositRepository : RepositoryBase<UserDeposit> { }


    public class UserRepository : RepositoryBase<User>
    {

        public User GetByUserGuid(Guid id)
        {
            string sql = "SELECT TOP 1 * FROM [User] u LEFT JOIN [UserBankAccount] uba ON u.Id = uba.UserId WHERE u.UserGuid = @id AND Deleted=0";
            var r = Conn.Query<User, UserBankAccount, User>(sql, (u, b) => { u.BankAccount = b; return u; }, new { id = id });
            return r != null && r.Any() ? r.FirstOrDefault() : null;
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

        public IEnumerable<UserProduct> GetProductsByUser(int userId)
        {
            string sql = "SELECT * FROM UserProduct u INNER JOIN Product p on p.Id = u.ProductId WHERE u.UserId = @userId";
            return Conn.Query<UserProduct, Product, UserProduct>(sql, (u, p) => { u.Product = p; return u; }, new { userId = userId });
        }
        

        public UserProduct GetProductByUser(int productId, int userId)
        {
            string sql = "SELECT TOP 1 * FROM Product p INNER JOIN UserProduct u on p.Id = u.ProductId WHERE u.UserId = @userId AND p.Id = @pid";
            var r = Conn.Query<UserProduct, Product, UserProduct>(sql, (u, p) => { u.Product = p; return u; }, new { userId = userId, pid = productId });
            return r.FirstOrDefault();
        }

        public void UpdateUserProduct(UserProduct up)
        {
            Conn.Update<UserProduct>(up);
        }

        public int CreateUserProduct(UserProduct up)
        {
            return (int)Conn.Insert(up);
        }

        #endregion


        public int GetDirectRefUsersCount(int userId)
        {
            string sql = "SELECT COUNT(1) FROM [User] WHERE Deleted = 0 AND RefUserId = @userId AND RefUserId != Id";
            return Conn.QuerySingle<int>(sql, new { userId = userId });
        }

        public int GetIndirectRefUsersCount(int userId)
        {
            string sql = "SELECT COUNT(1) FROM [User] WHERE Deleted = 0 AND RefUserPath like @userId AND RefUserId != Id";
            return Conn.QuerySingle<int>(sql, new { userId = userId + "%" });
        }

        public IEnumerable<User> GetRefUsers(int userId)
        {
            string sql = "SELECT * FROM [User] WHERE Deleted = 0 AND RefUserPath like @userId AND RefUserId != Id";
            return Conn.Query<User>(sql, new { userId = userId + "%" });
        }

        public void CreateUserScore(UserScore score)
        {
            new UserScoreRepository().Create(score);
        }


        public IEnumerable<UserScore> GetRefUserRewardScores(UserScoreCriteria criteria)
        {
            string sql = @"SELECT * FROM UserScore us INNER JOIN [User] u on us.UserId = u.Id 
                                WHERE TypeId IN (4, 10, 20) AND us.UserPath LIKE @path AND us.CreateTime BETWEEN @start AND @end";

            return Conn.Query<UserScore, User, UserScore>(sql, (us, u) => { us.User = u; return us; }, new { path = criteria.RefUserId + "%", start = criteria.StartDate, end = criteria.EndDate });
        }

        
        public IEnumerable<UserScore> GetUserRewardScores(UserScoreCriteria criteria)
        {
            string sql = @"SELECT * FROM UserScore us INNER JOIN [User] u on us.UserId = u.Id 
                                WHERE TypeId IN (4, 10, 20) AND us.UserId = @userId AND us.CreateTime BETWEEN @start AND @end";

            return Conn.Query<UserScore, User, UserScore>(sql, (us, u) => { us.User = u; return us; }, new { userId = criteria.RefUserId, start = criteria.StartDate, end = criteria.EndDate });
        }


        public IEnumerable<UserScore> GetScoresByUser(UserScoreCriteria criteria)
        {
            string sql = @"SELECT * FROM UserScore us INNER JOIN [User] u on us.UserId = u.Id 
                                WHERE us.UserId = @userId AND us.CreateTime BETWEEN @start AND @end";

            return Conn.Query<UserScore, User, UserScore>(sql, (us, u) => { us.User = u; return us; }, new { userId = criteria.RefUserId, start = criteria.StartDate, end = criteria.EndDate });
        }


        public IEnumerable<ProductRebirth> GetRebirthHistoryByUser(int userId)
        {
            string sql = "SELECT * FROM ProductRebirth r INNER JOIN Product p on r.ProductId = p.Id WHERE r.UserId = @userId";
            return Conn.Query<ProductRebirth, Product, ProductRebirth>(sql, (r, p) => { r.Product = p; return r; }, new { userId = userId });
        }


        public IEnumerable<UserBorrow> GetBorrowHistoryByUser(int userId)
        {
            string sql = "SELECT * FROM UserBorrow r INNER JOIN Product p on r.ProductId = p.Id WHERE r.UserId = @userId";
            return Conn.Query<UserBorrow, Product, UserBorrow>(sql, (r, p) => { r.Product = p; return r; }, new { userId = userId });
        }


        public IEnumerable<UserGive> GetGiveHistoryByUser(int userId)
        {
            string sql = "SELECT * FROM UserGive g INNER JOIN Product p on g.ProductId = p.Id INNER JOIN [User] u on g.ReceiverId = u.Id  WHERE g.UserId = @userId";
            return Conn.Query<UserGive, Product, User, UserGive>(sql, (g, p, u) => { g.Product = p; g.Receiver = u; return g; }, new { userId = userId });
        }

        public IEnumerable<UserWithdraw> GetWithdrawHistoryByUser(int userId)
        {
            string sql = "SELECT * FROM UserWithdraw WHERE UserId = @userId";
            return Conn.Query<UserWithdraw>(sql, new { userId = userId });
        }


        public void CreateDeposity(UserDeposit deposit)
        {
            new UserDepositRepository().Create(deposit);
        }

        public IEnumerable<UserDeposit> GetDepositsByUser(int userId)
        {
            string sql = "SELECT * FROM UserDeposit WHERE UserId = @userId";
            return Conn.Query<UserDeposit>(sql, new { userId = userId });
        }
    }
}
