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
        Func<Market, Product, Market> mapper = (market, product) =>
        {
            market.Product = product;
            return market;
        };

        public IEnumerable<Market> GeTodayMarkets()
        {
            DateTime date = DateTime.Today;
            string sql = "SELECT * FROM Market m INNER JOIN Product p ON m.ProductId = p.Id WHERE Date=@date";
            return Conn.Query<Market, Product, Market>(sql, mapper, new { date = date });
        }

        public Market GetTodayMarket(int productId)
        {
            DateTime date = DateTime.Today;
            string sql = "SELECT TOP 1 * FROM Market m INNER JOIN Product p ON m.ProductId = p.Id WHERE m.productid=@pid AND Date=@date";
            return Conn.Query<Market, Product, Market>(sql, mapper, new { pid = productId, date = date }).FirstOrDefault();

        }
    }
}
