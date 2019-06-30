using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Apollo.External.SyndicationToolbox;
using Apollo.Services;
using Apollo.Utilities;
using Feed = Apollo.Data.Feed;

namespace Apollo.External
{
    public interface IExternalFeedService
    {
        Task<FeedDetails> GetFeedDetails(string url);
        Task<IReadOnlyList<ExternalFeedItem>> GetItems(Feed feed);
    }

    public interface IFeedReader
    {
        Task<FeedDetails> GetFeedDetails(string content);
        Task<IReadOnlyList<ExternalFeedItem>> GetItems(string content);
    }

    public class ExternalFeedService : IExternalFeedService
    {
        private readonly IUrlFetcher urlFetcher;

        public ExternalFeedService(IUrlFetcher urlFetcher)
        {
            this.urlFetcher = urlFetcher;
        }

        public async Task<FeedDetails> GetFeedDetails(string url)
        {
            var content = await urlFetcher.Get(url);
            var feedParser = FeedParser.Create(content);
            var parsed = feedParser.Parse();
            return new FeedDetails
            {
                Name = parsed.Name
            };
        }

        public async Task<IReadOnlyList<ExternalFeedItem>> GetItems(Feed feed)
        {
            try
            {
                var content = await urlFetcher.Get(feed.url);
                var feedParser = FeedParser.Create(content);
                var parsed = feedParser.Parse();
                return parsed.Articles.Select(a => new ExternalFeedItem
                {
                    Title = a.Title,
                    Content = a.Content,
                    Link = a.WebUri,
                    PublishDate = a.Published
                }).ToList();
            }
            catch (Exception exception)
            {
                Logger.Error("Error attempting to fetch feed items", new {feed, exception.Message});
                return null;
            }
        }
    }

    public class FeedDetails
    {
        public string Name { get; set; }
    }

    public class ExternalFeedItem
    {
        public string Content { get; set; }
        public string Link { get; set; }
        public DateTime PublishDate { get; set; }
        public string Title { get; set; }
    }


    public class FeedException : Exception
    {
        public FeedException(string message) : base(message)
        {
        }
    }
}
