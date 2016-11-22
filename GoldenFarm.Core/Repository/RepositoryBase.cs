using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;

namespace GoldenFarm.Repository
{
    public abstract class RepositoryBase<TEntity> where TEntity : class
    {
        protected readonly IDbConnection Conn = null;

        public RepositoryBase()
        {
            Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["farm"].ConnectionString);
        }
        //public virtual void Dispose()
        //{
        //    if(Conn != null && Conn.State != ConnectionState.Closed)
        //    {
        //        Conn.Close();
        //        Conn.Dispose();
        //    }
        //}

        //public virtual TEntity GetById(int id)
        //{
        //    return Conn.Get<TEntity>(id);
        //}

        
        //public IEnumerable<TEntity> GetAll()
        //{
        //    return Conn.GetAll<TEntity>();
        //}
    }
}
