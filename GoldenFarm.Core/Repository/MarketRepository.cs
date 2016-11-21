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
        public IEnumerable<Market> GetAll()
        {
            string sql = "SELECT * FROM Market m LEFT JOIN Product p ON m.ProductId = p.Id";
            return Conn.Query<Product, Market, Market>(sql, (product, market) =>
            {
                market.Product = product;
                return market;
            });
        }
    }
}
