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
        Task<int> GetRecentCount();
    }

    public class BookmarksDataService : BaseDataService, IBookmarksDataService
    {
        public const string InsertSql = "insert into bookmarks" +
                                        "(title, link, description, tags, created_at, modified_at)" +
                                        "values (@title, @link, @description, @tags, " +
                                        "@created_at, @modified_at)";

        public const string CountSql = "select count(*) from bookmarks;";

        public const string RecentCountSql = "select count(*) from bookmarks where created_at>@created_at;";

        public const string BasicGetSql = "select * from bookmarks order by id desc limit 100 offset @start";

        public const string UrlGetSql = "select * from bookmarks where link=@link order by id desc";

        public BookmarksDataService(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task Insert(Bookmark bookmark)
        {
            if(bookmark.created_at == default(DateTime))
                bookmark.created_at = DateTime.UtcNow;
            if(bookmark.modified_at == default(DateTime))
                bookmark.modified_at = DateTime.UtcNow;

            await Execute(InsertSql, bookmark);
        }

        public async Task<int> GetTotal()
        {
            return (await QueryAsync<CountResult>(CountSql, null)).Single().count;
        }

        public async Task<IReadOnlyList<Bookmark>> Get(int start, string link)
        {
            if(!string.IsNullOrWhiteSpace(link))
                return await QueryAsync<Bookmark>(UrlGetSql, new {link});
            return await QueryAsync<Bookmark>(BasicGetSql, new {start});
        }

        public async Task<int> GetRecentCount()
        {
            return (await QueryAsync<CountResult>(RecentCountSql, new { created_at = DateTime.Now.AddDays(-7)})).Single().count;
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
