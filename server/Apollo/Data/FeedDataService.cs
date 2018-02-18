using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Apollo.Data
{
    public interface IFeedDataService
    {
        Task AddFeed(string name, string url);
        Task<IReadOnlyList<Feed>> GetFeeds();
        Task UpdateLastAttempt(int feedId);
        Task MarkUpdated(int feedId);
        Task<IReadOnlyList<FeedItem>> CheckItemUrl(string itemLink);
        Task AddItem(int feedId, string title, string url, string body, DateTime publishDate);
        Task<IReadOnlyList<FeedItem>> GetItems(int feedId);
        Task<FeedItem> MarkItemRead(int itemId);
    }

    public class FeedDataService : BaseDataService, IFeedDataService
    {
        private readonly ITimelineDataService tds;

        public const string AddFeedSql = "insert into feeds(name, url, created_at, last_updated_at, last_attempted) " +
                                         "values (@name, @url, current_timestamp, current_timestamp, current_timestamp) " +
                                         "returning id";

        public const string GetFeedsSql = "select feeds.*, " +
                                          "(select count(*) from feed_items where " +
                                          "feeds.id=feed_items.feed_id and feed_items.read_at is null) " +
                                          "as unread_count from feeds order by feeds.name";

        public const string UpdateLastAttemptSql = "update feeds set last_attempted=current_timestamp " +
                                                   "where id=@feedId";

        public const string MarkUpdatedSql = "update feeds set last_updated_at=current_timestamp " +
                                                   "where id=@feedId";

        public const string CheckItemUrlSql = "select * from feed_items where url=@url";

        public const string AddItemSql = "insert into feed_items(feed_id, title, url, body, created_at, published_at) " +
                                         "values (@feedId, @title, @url, @body, current_timestamp, @publishDate)";

        public const string GetAllUnreadItemsSql = "select feed_items.*, feeds.name as feed_name from feed_items " +
                                                   "join feeds on feed_items.feed_id = feeds.id " +
                                                   "where read_at is null " +
                                                   "order by published_at limit 100";

        public const string GetFeedUnreadItemsSql = "select feed_items.*, feeds.name as feed_name from feed_items " +
                                                    "join feeds on feed_items.feed_id = feeds.id " +
                                                    "where read_at is null " +
                                                    "and feed_id=@feedId order by published_at limit 100";

        public const string MarkItemReadSql = "update feed_items set read_at=current_timestamp " +
                                              "where id=@feedItemId";

        public FeedDataService(IConnectionFactory connectionFactory, ITimelineDataService tds) : base(connectionFactory)
        {
            this.tds = tds;
        }

        public async Task AddFeed(string name, string url)
        {

            var id = await InsertAndReturnId(AddFeedSql, new {name, url});
            await tds.RecordEntry($"Added feed {name}", Constants.TimelineReferences.Feed, id);
        }

        public Task<IReadOnlyList<Feed>> GetFeeds()
        {
            return QueryAsync<Feed>(GetFeedsSql);
        }

        public Task UpdateLastAttempt(int feedId)
        {
            return Execute(UpdateLastAttemptSql, new {feedId});
        }

        public Task MarkUpdated(int feedId)
        {
            return Execute(MarkUpdatedSql, new {feedId});
        }

        public Task<IReadOnlyList<FeedItem>> CheckItemUrl(string url)
        {
            return QueryAsync<FeedItem>(CheckItemUrlSql, new {url});
        }

        public Task AddItem(int feedId, string title, string url, string body, DateTime publishDate)
        {
            if (string.IsNullOrWhiteSpace(title))
                title = "Untitled assumed by Apollo";
            if (title.Length > 255)
                title = title.Substring(0, 251) + "...";
            if (string.IsNullOrWhiteSpace(body))
                body = "<h3>No Content</h3>";
            if (string.IsNullOrEmpty(url))
                return Task.CompletedTask;
            return Execute(AddItemSql, new {feedId, title, url, body, publishDate});
        }

        public Task<IReadOnlyList<FeedItem>> GetItems(int feedId)
        {
            if (feedId == -1)
            {
                return QueryAsync<FeedItem>(GetAllUnreadItemsSql);
            }

            return QueryAsync<FeedItem>(GetFeedUnreadItemsSql, new {feedId});
        }

        public async Task<FeedItem> MarkItemRead(int feedItemId)
        {
            await Execute(MarkItemReadSql, new {feedItemId});
            var updated = await QueryAsync<FeedItem>("select * from feed_items where id=@feedItemId", new { feedItemId });
            return updated.Single();
        }
    }

    public class Feed
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public DateTime created_at { get; set; }
        public DateTime last_updated_at { get; set; }
        public DateTime last_attempted { get; set; }
        public int unread_count { get; set; }
    }

    public class FeedItem
    {
        public int id { get; set; }
        public int feed_id { get; set; }
        public string feed_name { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string body { get; set; }
        public DateTime created_at { get; set; }
        public DateTime published_at { get; set; }
        public DateTime? read_at { get; set; }
    }
}
