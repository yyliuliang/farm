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
    /*
    public class Repository
    {
        protected readonly IDbConnection Conn = null;

        public RepositoryBase()
        {
            Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["farm"].ConnectionString);
        }

        public TEntity Get<TEntity>(int id) where TEntity : EntityBase
        {
            return Conn.Get<TEntity>(id);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            Conn.Delete(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            Conn.Update(entity);
        }

        public IEnumerable<TEntity> GetAll<TEntity>(bool includeDeleted) where TEntity : EntityBase
        {
            if (!includeDeleted)
            {
                Type type = typeof(TEntity);
                var p = type.GetProperty("Deleted");
                if (p == null)
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

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : EntityBase
        {
            return GetAll<TEntity>(false);
        }

        public PageViewData<TEntity> GetPagedData<TEntity>(PageCriteria criteria) where TEntity : EntityBase
        {
            var data = new PageViewData<TEntity>();
            string table = string.IsNullOrEmpty(criteria.Table) ? typeof(TEntity).GetTableName() : criteria.Table;
            string fields = string.IsNullOrEmpty(criteria.Fields) ? "*" : criteria.Fields;
            string sql = @"SELECT {0} FROM {1} WHERE {2} ORDER BY {3} OFFSET ({4} * ({5}-1)) ROW FETCH NEXT {4} ROWS ONLY";
            sql = string.Format(sql, fields, table, criteria.Where, criteria.Order, criteria.PageSize, criteria.PageIndex);
            data.Items = Conn.Query<TEntity>(sql, criteria.Parameter);
            sql = "SELECT COUNT(1) FROM {0} WHERE {1}";
            sql = string.Format(sql, table, criteria.Where);
            data.TotalCount = Conn.QueryFirstOrDefault<int>(sql, criteria.Parameter);
            data.PageIndex = criteria.PageIndex;
            data.PageSize = criteria.PageSize;
            return data;
        }



        public int Create<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            return (int)Conn.Insert(entity);
        }
    }
    */

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
            string table = string.IsNullOrEmpty(criteria.Table) ? typeof(TEntity).GetTableName() : criteria.Table;
            string fields = string.IsNullOrEmpty(criteria.Fields) ? "*" : criteria.Fields;
            string sql = @"SELECT {0} FROM {1} WHERE {2} ORDER BY {3} OFFSET ({4} * ({5}-1)) ROW FETCH NEXT {4} ROWS ONLY";
            sql = string.Format(sql, fields, table, string.IsNullOrEmpty(criteria.Where) ? " 1=1 " : criteria.Where, string.IsNullOrEmpty(criteria.Order) ? "Id DESC" : criteria.Order, criteria.PageSize, criteria.PageIndex);
            data.Items = Conn.Query<TEntity>(sql, criteria.Parameter);
            sql = "SELECT COUNT(1) FROM {0} WHERE {1}";
            sql = string.Format(sql, table, string.IsNullOrEmpty(criteria.Where) ? " 1=1 " : criteria.Where);
            data.TotalCount = Conn.QueryFirstOrDefault<int>(sql, criteria.Parameter);
            data.PageIndex = criteria.PageIndex;
            data.PageSize = criteria.PageSize;
            return data;
        }

        public PageViewData<TEntity> GetPagedData<TFirst, TSecond, TReturn>(PageCriteria criteria, Func<TFirst, TSecond, TReturn> func) where TReturn : EntityBase
        {
            var data = new PageViewData<TEntity>();
            string table = string.IsNullOrEmpty(criteria.Table) ? typeof(TEntity).GetTableName() : criteria.Table;
            string fields = string.IsNullOrEmpty(criteria.Fields) ? "*" : criteria.Fields;
            string sql = @"SELECT {0} FROM {1} WHERE {2} ORDER BY {3} OFFSET ({4} * ({5}-1)) ROW FETCH NEXT {4} ROWS ONLY";
            sql = string.Format(sql, fields, table, string.IsNullOrEmpty(criteria.Where) ? " 1=1 " : criteria.Where, string.IsNullOrEmpty(criteria.Order) ? "Id DESC" : criteria.Order, criteria.PageSize, criteria.PageIndex);
            data.Items = (IEnumerable<TEntity>)Conn.Query<TFirst, TSecond, TReturn>(sql, func, criteria.Parameter);
            sql = "SELECT COUNT(1) FROM {0} WHERE {1}";
            sql = string.Format(sql, table, string.IsNullOrEmpty(criteria.Where) ? " 1=1 " : criteria.Where);
            data.TotalCount = Conn.QueryFirstOrDefault<int>(sql, criteria.Parameter);
            data.PageIndex = criteria.PageIndex;
            data.PageSize = criteria.PageSize;
            return data;
        }



        public int Create(TEntity entity)
        {
            return (int)Conn.Insert(entity);
        }

    }
}
