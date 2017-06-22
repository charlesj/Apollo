using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface IBookmarksDataService
    {
        Task Insert(Bookmark bookmark);
        Task<int> GetTotal();
        Task<IReadOnlyList<Bookmark>> Get(int start, string link);
    }

    public class BookmarksDataService : BaseDataService, IBookmarksDataService
    {
        public const string InsertSql = "insert into bookmarks" +
                                        "(title, link, description, tags, created_at, modified_at)" +
                                        "values (@title, @link, @description, @tags, " +
                                        "@created_at, @modified_at)";

        public const string CountSql = "select count(*) from bookmarks;";

        public const string BasicGetSql = "select * from bookmarks order by id desc limit 100 offset @start";

        public const string UrlGetSql = "select * from bookmarks where link=@link order by id desc";

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

        public async Task<int> GetTotal()
        {
            using (var conn = await connectionFactory.GetConnection())
            {
                return (await conn.QueryAsync<CountResult>(CountSql, null)).Single().count;
            }
        }

        public async Task<IReadOnlyList<Bookmark>> Get(int start, string link)
        {
            using (var conn = await connectionFactory.GetConnection())
            {
                if(!string.IsNullOrWhiteSpace(link))
                    return (await conn.QueryAsync<Bookmark>(UrlGetSql, new {link})).ToList();
                return (await conn.QueryAsync<Bookmark>(BasicGetSql, new {start})).ToList();
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
