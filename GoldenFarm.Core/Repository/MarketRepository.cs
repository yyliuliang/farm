﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoldenFarm.Entity;
using Dapper;
using Dapper.Contrib.Extensions;

namespace GoldenFarm.Repository
{
    public class EntrustRepository : RepositoryBase<Entrust> { }

    public class TransactionRepository : RepositoryBase<Transaction> { }

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

        public IEnumerable<Market> GetLatestMarkets()
        {
            string sql = "SELECT TOP 1 Date FROM Market ORDER BY Date DESC";
            DateTime date = DateTime.Today;
            date = Conn.QuerySingleOrDefault<DateTime>(sql);
            sql = "SELECT * FROM Market m INNER JOIN Product p ON m.ProductId = p.Id WHERE Date=@date";
            return Conn.Query<Market, Product, Market>(sql, mpMapper, new { date = date });
        }


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

        public Market GetTodayProductMarket(int productId)
        {
            DateTime date = DateTime.Today;
            string sql = "SELECT TOP 1 * FROM Market m INNER JOIN Product p ON m.ProductId = p.Id WHERE p.Id=@pid AND Date=@date";
            return Conn.Query<Market, Product, Market>(sql, mpMapper, new { pid = productId, date = date }).FirstOrDefault();
        }

        public IEnumerable<Transaction> GetTodayTransactions(string productCode)
        {
            DateTime date = DateTime.Today;
            string sql = "SELECT * FROM [Transaction] mt INNER JOIN Product p ON mt.ProductId = p.Id WHERE p.productcode=@code AND mt.Date=@date";
            return Conn.Query<Transaction, Product, Transaction>(sql, mtpMapper, new { code = productCode, date = date });
        }
        #endregion


        public IEnumerable<Market> GetProductMarkets(string productCode)
        {
            string sql = "SELECT * FROM Market m INNER JOIN Product p ON m.ProductId = p.Id WHERE p.productcode=@code";
            return Conn.Query<Market, Product, Market>(sql, mpMapper, new { code = productCode });
        }


        public IEnumerable<dynamic> GetTop5Entrusts(int productId, bool buy)
        {
            DateTime date = DateTime.Today;
            DateTime start = DateTime.Parse(date.ToString("yyyy-MM-dd 0:0:0"));
            DateTime end = DateTime.Parse(date.ToString("yyyy-MM-dd 23:59:59"));
            string sql = @"SELECT TOP 5 Price, SUM(Count - DealedCount) Number FROM Entrust 
                           WHERE ProductId = @pid AND Cancelled = 0 AND CreateTime >= @start AND CreateTime <@end AND Status IN (0, 2) AND IsBuy = @buy
                           GROUP BY Price
                           ORDER BY Price DESC";
            return Conn.Query(sql, new { pid = productId, buy = buy, start = start, end = end }).Select(e => new { Price = e.Price, Number = e.Number });
        }

        public IEnumerable<Entrust> GetEntrusts(MarketCriteria criteria)
        {
            string sql = @"SELECT * FROM Entrust e INNER JOIN Product p on e.ProductId = p.Id 
                           WHERE 1 = 1";

            var parameters = new DynamicParameters();

            if (criteria.UserId > 0)
            {
                sql += " AND e.UserId = @userId";
                parameters.Add("userId", criteria.UserId);
            }
            if (criteria.StartDate.HasValue)
            {
                sql += " AND e.CreateTime >= @start";
                parameters.Add("start", criteria.StartDate.Value);
            }

            if (criteria.EndDate.HasValue)
            {
                sql += " AND e.CreateTime < @end";
                parameters.Add("end", criteria.EndDate.Value);
            }

            if (criteria.ProductId > 0)
            {
                sql += " AND e.ProductId = @pid";
                parameters.Add("pid", criteria.ProductId);
            }
            if (criteria.IsBuy > -1)
            {
                sql += " AND e.IsBuy = @buy";
                parameters.Add("buy", criteria.IsBuy);
            }
            if (criteria.Cancelled.HasValue && criteria.Cancelled.Value > -1)
            {
                sql += " AND e.Cancelled = @cancelled";
                parameters.Add("cancelled", criteria.Cancelled);
            }

            sql += " ORDER BY e.Id DESC";
            return Conn.Query<Entrust, Product, Entrust>(sql, (e, p) => { e.Product = p; return e; }, parameters);
        }

        public int PostEntrust(Entrust entrust)
        {
            int dealedCount = 0;
            CreateEntrust(entrust);

            return dealedCount;
        }


        public Entrust GetEntrustById(int id)
        {
            return new EntrustRepository().Get(id);
        }

        public void CancelEntrust(Entrust entrust)
        {
            if (entrust != null)
            {
                if (entrust.Status == 0)
                {
                    entrust.Status = 9;
                }
                else if (entrust.Status == 2)
                {
                    entrust.Status = 3;
                }

                entrust.Cancelled = true;
                new EntrustRepository().Update(entrust);
            }
        }

        public int CreateEntrust(Entrust entrust)
        {
            return new EntrustRepository().Create(entrust);
        }

        public void UpdateEntrust(Entrust entrust)
        {
            Conn.Update<Entrust>(entrust);
        }

        public int CreateTransaction(Transaction transaction)
        {
            return new TransactionRepository().Create(transaction);
        }


        public IEnumerable<Transaction> GetTransactions(MarketCriteria criteria)
        {
            return null;
        }



        public void PrepareMarketsTestData()
        {
            string sql = "TRUNCATE TABLE [Market]";
            Conn.Execute(sql);

            DateTime today = DateTime.Today;
            DateTime start = DateTime.Now.AddMonths(-3);

            int days = (today - start).Days;
            Random r = new Random();
            var products = new ProductRepository().GetAllProducts();
            foreach (var p in products)
            {
                bool isrose = p.ProductCode == "800008";
                double prevClosePrice = 0D;
                for (int i = 0; i < days; i++)
                {
                    var rate = (double)r.Next(0, 10000) / (double)100000;

                    
                    bool tag = DateTime.Now.Ticks % 2 == 0;
                    string date = today.AddDays(-i).ToString("yyyy-MM-dd");

                    var openPrice = 0D;
                    var topPrice = 0D;
                    var bottomPrice = 0D;

                    if (prevClosePrice == 0)
                    {
                        openPrice = r.NextDouble();
                    }
                    else
                    {
                        openPrice = prevClosePrice;
                    }
                    var currentPrice = 0D;
                    var closePrice = 0D;
                    if (tag)
                    {
                        
                        if (isrose)
                        {
                            currentPrice = openPrice * (1 + (double)r.Next(0, 60000) / (double)100000);
                        }
                        else
                        {
                            currentPrice = openPrice * (1 + (double)r.Next(0, 10000) / (double)100000);
                        }
                    }
                    else
                    {
                        if (isrose)
                        {
                            currentPrice = openPrice * (1 - (double)r.Next(0, 60000) / (double)100000);
                        }
                        else
                        {
                            currentPrice = openPrice * (1 - (double)r.Next(0, 10000) / (double)100000);
                        }
                    }

                    if (tag)
                    {
                        if (isrose)
                        {
                            closePrice = openPrice * (1 + (double)r.Next(0, 60000) / (double)100000);
                        }
                        else
                        {
                            closePrice = openPrice * (1 + (double)r.Next(0, 10000) / (double)100000);
                        }
                    }
                    else
                    {
                        if (isrose)
                        {
                            closePrice = openPrice * (1 - (double)r.Next(0, 60000) / (double)100000);
                        }
                        else
                        {
                            closePrice = openPrice * (1 - (double)r.Next(0, 10000) / (double)100000);
                        }
                    }
                    var max = Math.Max(currentPrice, openPrice);
                    var min = Math.Min(currentPrice, openPrice);
                    if (isrose)
                    {
                        topPrice = max * (1 + (double)r.Next(0, 60000) / (double)100000);
                        bottomPrice = min * (1 - (double)r.Next(0, 60000) / (double)100000);
                    }
                    else
                    {
                        topPrice = max * (1 + (double)r.Next(0, 10000) / (double)100000);
                        bottomPrice = min * (1 - (double)r.Next(0, 10000) / (double)100000);
                    }
                    
                    sql = @"INSERT INTO Market( ProductId, CurrentPrice, PrevDayPrice, OpenPrice, ClosePrice, TopPrice, BottomPrice, Volume, [Date])
                            VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, '{8}')";
                    sql = string.Format(sql, p.Id, currentPrice, prevClosePrice == 0 ? openPrice : prevClosePrice, openPrice, closePrice, topPrice, bottomPrice, r.Next(999999), date);
                    Conn.Execute(sql);
                    prevClosePrice = closePrice;
                }
            }
        }

        public void PrepareTransactionsTestData()
        {
            string sql = "TRUNCATE TABLE [Transaction]";
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
                    sql = @"INSERT INTO [Transaction]( TransactionId, ProductId, UserId, IsBuy, [Count], ChargeFee, Price, [Date], CreateTime)
                            VALUES(NEWID(), {0}, 0, 1, {1}, {2}, {3}, '{4}', '{5}')";
                    sql = string.Format(sql, p.Id, r.Next(999999), r.NextDouble(), r.NextDouble(), date, time);
                    Conn.Execute(sql);
                }
            }
        }
    }
}
