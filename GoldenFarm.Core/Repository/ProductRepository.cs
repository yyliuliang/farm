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
    public class ProductRepository : RepositoryBase<Product>
    {
        public IEnumerable<Product> GetAllProducts()
        {
            string sql = "SELECT * FROM Product WHERE Deleted=0";
            return Conn.Query<Product>(sql);
        }
    }
}
