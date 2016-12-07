using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoldenFarm.Entity;
using Dapper;
using Dapper.Contrib.Extensions;

namespace GoldenFarm.Repository
{
    public class MarketRepository : RepositoryBase<Market>
    {
        Func<Market, Product, Market> mpMapper = (market, product) =>
        {
            market.Product = product;
            return market;
        };

        Func<Transaction, Product, Transaction> mtpMapper = (transaction, product) =>
        {
            transaction.Product = product;
            return transaction;
        };


        #region 今日数据
        public IEnumerable<Market> GeTodayMarkets()
        {
            DateTime date = DateTime.Today;
            string sql = "SELECT * FROM Market m INNER JOIN Product p ON m.ProductId = p.Id WHERE Date=@date";
            return Conn.Query<Market, Product, Market>(sql, mpMapper, new { date = date });
        }

        public Market GetTodayProductMarket(string productCode)
        {
            DateTime date = DateTime.Today;
            string sql = "SELECT TOP 1 * FROM Market m INNER JOIN Product p ON m.ProductId = p.Id WHERE p.productcode=@code AND Date=@date";
            return Conn.Query<Market, Product, Market>(sql, mpMapper, new { code = productCode, date = date }).FirstOrDefault();
        }

        public IEnumerable<Transaction> GetTodayTransactions(string productCode)
        {
            DateTime date = DateTime.Today;
            string sql = "SELECT * FROM MarketTransaction mt INNER JOIN Product p ON mt.ProductId = p.Id WHERE p.productcode=@code AND mt.Date=@date";
            return Conn.Query<Transaction, Product, Transaction>(sql, mtpMapper, new { code = productCode, date = date });
        }
        #endregion


        public IEnumerable<Market> GetProductMarkets(string productCode)
        {
            string sql = "SELECT * FROM Market m INNER JOIN Product p ON m.ProductId = p.Id WHERE p.productcode=@code";
            return Conn.Query<Market, Product, Market>(sql, mpMapper, new { code = productCode});
        }



        public IEnumerable<Entrust> GetEntrusts(MarketCriteria criteria)
        {
            string sql = @"SELECT * FROM Entrust e INNER JOIN Product p on e.ProductId = p.Id 
                           WHERE e.UserId = @userId AND e.CreateTime BETWEEN @start AND @end";

            var parameters = new DynamicParameters();
            parameters.Add("userId", criteria.UserId);
            parameters.Add("start", criteria.StartDate);
            parameters.Add("end", criteria.EndDate);
            if (criteria.ProductId > 0)
            {
                sql += " AND e.ProductId = @pid";
                parameters.Add("pid", criteria.ProductId);
            }
            if(criteria.IsBuy > -1)
            {
                sql += " AND e.IsBuy = @buy";
                parameters.Add("buy", criteria.IsBuy);
            }

            return Conn.Query<Entrust, Product, Entrust>(sql, (e, p) => { e.Product = p; return e; }, parameters);
        }

        

        public void PrepareMarketsTestData()
        {
            string sql = "TRUNCATE TABLE [Market]";
            Conn.Execute(sql);

            DateTime today = DateTime.Today;
            DateTime start = DateTime.Now.AddMonths(-3);
            int days = (today - start).Days;
            Random r = new Random();
            for(int i=0; i < days; i++ )
            {
                string date = today.AddDays(-i).ToString("yyyy-MM-dd");
                var products = new ProductRepository().GetAllProducts();

                foreach(var p in products)
                {
                    sql = @"INSERT INTO Market( ProductId, CurrentPrice, PrevDayPrice, OpenPrice, ClosePrice, TopPrice, BottomPrice, Volume, [Date])
                            VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, '{8}')";
                    sql = string.Format(sql, p.Id, r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble(), r.Next(999999), date);
                    Conn.Execute(sql);
                }
            }
        }

        public void PrepareTransactionsTestData()
        {
            string sql = "TRUNCATE TABLE [MarketTransaction]";
            Conn.Execute(sql);

            DateTime now = DateTime.Now;
            DateTime start = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd 9:0:0"));
            int minutes = (int)(now - start).TotalMinutes;
            string date = now.ToString("yyyy-MM-dd");
            Random r = new Random();
            for (int i = 0; i < minutes; i++)
            {
                string time = start.AddMinutes(i).ToString("yyyy-MM-dd HH:mm:ss");
                
                var products = new ProductRepository().GetAllProducts();

                foreach (var p in products)
                {
                    sql = @"INSERT INTO MarketTransaction( TransactionId, ProductId, BuyerId, SellerId, Volume, Price, [Date], CreateTime)
                            VALUES(NEWID(), {0}, 0, 0, {1}, {2}, '{3}', '{4}')";
                    sql = string.Format(sql, p.Id, r.Next(999999), r.NextDouble(), date, time);
                    Conn.Execute(sql);
                }
            }
        }
    }
}
