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
using System.Linq.Expressions;

namespace GoldenFarm.Repository
{
    public abstract class RepositoryBase<TEntity> where TEntity : class
    {
        protected readonly IDbConnection Conn = null;

        public RepositoryBase()
        {
            Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["farm"].ConnectionString);
        }
        
        public TEntity Get(int id)
        {
            return Conn.Get<TEntity>(id);
        }

        public void Delete(TEntity entity)
        {
            Conn.Delete(entity);
        }

        public void Update(TEntity entity)
        {
            Conn.Update(entity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Conn.GetAll<TEntity>();
        }

        public IEnumerable<TEntity> GetPagedData(PageCriteria criterial)
        {
            //  return Conn.get
            return null;
        }

        public void Create(TEntity entity)
        {
            Conn.Insert(entity);
        }

    }
}
