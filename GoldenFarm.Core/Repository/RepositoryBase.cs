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
using GoldenFarm.Entity;

namespace GoldenFarm.Repository
{
    public abstract class RepositoryBase<TEntity> where TEntity : EntityBase
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

        public IEnumerable<TEntity> GetAll(bool includeDeleted)
        {            
            if (!includeDeleted)
            {
                Type type = typeof(TEntity);
                var p = type.GetProperty("Deleted");
                if(p == null)
                {
                    return Conn.GetAll<TEntity>();
                }
                else
                {
                    string sql = "SELECT * FROM {0} WHERE Deleted = 0";                   
                    string table = type.GetTableName();
                    sql = string.Format(sql, table);
                }                
            }
            return Conn.GetAll<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return GetAll(false);
        }

        public PageViewData<TEntity> GetPagedData(PageCriteria criteria)
        {
            var data = new PageViewData<TEntity>();
            string table = typeof(TEntity).GetTableName();
            string sql = @"SELECT * FROM {0} WHERE {1} ORDER BY {2} OFFSET ({3} * ({4}-1)) ROW FETCH NEXT {3} ROWS ONLY";
            sql = string.Format(sql, table, criteria.Where, criteria.Order, criteria.PageSize, criteria.PageIndex);
            data.Items = Conn.Query<TEntity>(sql);
            sql = "SELECT COUNT(1) FROM {0} WHERE {1}";
            sql = string.Format(sql, table, criteria.Where);
            data.TotalCount = Conn.QueryFirstOrDefault<int>(sql);
            return data;
        }

        

        public void Create(TEntity entity)
        {
            Conn.Insert(entity);
        }

    }
}
