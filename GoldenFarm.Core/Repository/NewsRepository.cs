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
    public class NewsRepository : RepositoryBase<News>
    {
        Func<News, NewsCategory, News> ncMapper = (news, category) =>
        {
            news.Category = category;
            return news;
        };

        public IEnumerable<NewsCategory> GetAllCategories()
        {         
            return Conn.GetAll<NewsCategory>();
        }            

        public IEnumerable<News> GetNewsByCategory(string category)
        {
            if (!string.IsNullOrEmpty(category))
            {
                string sql = "SELECT * FROM News n INNER JOIN NewsCategory c ON n.CategoryId = c.Id WHERE c.Name = @name AND n.Deleted = 0";
                return Conn.Query<News, NewsCategory, News>(sql, ncMapper, new { name = category });
            }
            else
            {
                string sql = "SELECT * FROM News n INNER JOIN NewsCategory c ON n.CategoryId = c.Id WHERE n.Deleted = 0";
                return Conn.Query<News, NewsCategory, News>(sql, ncMapper);
            }
        }


        public NewsCategory GetNewsCategory(string category)
        {
            string sql = "SELECT * FROM NewsCategory WHERE Name = @name";
            return Conn.QueryFirstOrDefault<NewsCategory>(sql, new { name = category });
        }

        public IEnumerable<News> GetAllNews()
        {
            string sql = "SELECT * FROM News WHERE Deleted = 0";
            return Conn.Query<News>(sql);
        }

        
    }


}
