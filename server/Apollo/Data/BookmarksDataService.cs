using System;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface IBookmarksDataService
    {
        Task Insert(Bookmark bookmark);
    }
    
    public class BookmarksDataService : BaseDataService, IBookmarksDataService
    {
        public const string InsertSql = "insert into bookmarks" +
                                        "(title, link, description, tags, created_at, modified_at)" +
                                        "values (@title, @link, @description, @tags, " +
                                        "@created_at, @modified_at)";
        
        public BookmarksDataService(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task Insert(Bookmark bookmark)
        {
            if(bookmark.created_at == default(DateTime))
                bookmark.created_at = DateTime.UtcNow;
            if(bookmark.modified_at == default(DateTime))
                bookmark.modified_at = DateTime.UtcNow;
            
            using (var conn = await connectionFactory.GetConnection())
            {
                conn.Execute(InsertSql, bookmark);
            }
        }
    }

    public class Bookmark
    {
        public int id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public string[] tags { get; set; }
        public DateTime created_at { get; set; }
        public DateTime modified_at { get; set; }
    }
}