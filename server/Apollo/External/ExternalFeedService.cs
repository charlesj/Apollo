using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Apollo.Data;
using Apollo.Services;
using Apollo.Utilities;

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
            var feedType = DetectFeedType(content);
            if (feedType == FeedType.Unknown)
            {
                throw new FeedException("Cannot determine feedtype");
            }

            var reader = GetFeedReader(feedType);

            return await reader.GetFeedDetails(content);
        }

        public async Task<IReadOnlyList<ExternalFeedItem>> GetItems(Feed feed)
        {
            try
            {
                var content = await urlFetcher.Get(feed.url);
                var feedType = DetectFeedType(content);
                if (feedType == FeedType.Unknown)
                {
                    throw new FeedException("Cannot determine feedtype");
                }

                var reader = GetFeedReader(feedType);
                return await reader.GetItems(content);
            }
            catch (Exception exception)
            {
                Logger.Error("Error attempting to fetch feed items", new {feed, exception.Message});
                return null;
            }
        }

        private IFeedReader GetFeedReader(FeedType type)
        {
            switch (type)
            {
                case FeedType.Atom:
                    return new AtomFeedReader();
                case FeedType.RSS:
                    return new RssFeedReader();
                default:
                    return null;
            }
        }

        public FeedType DetectFeedType(string feed)
        {
            if(feed.Contains("<rss"))
            {
                return FeedType.RSS;
            }

            if (feed.Contains("<feed "))
            {
                return FeedType.Atom;
            }

            return FeedType.Unknown;
        }
    }

    public enum FeedType
    {
        RSS,
        Atom,
        Unknown
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

    public class AtomFeedReader : IFeedReader
    {
        public Task<FeedDetails> GetFeedDetails(string content)
        {
            var document = XDocument.Parse(content);
            if (document.Root == null)
            {
                Logger.Error("Could not parse Atom Feed", new {content});
                throw new FeedException("Could not parse atom feed");
            }

            var title = document.Root.Descendants().First(d => d.Name.LocalName == "title").Value;
            return Task.FromResult(new FeedDetails {Name = title});
        }

        public Task<IReadOnlyList<ExternalFeedItem>> GetItems(string content)
        {
            Logger.Trace("Attempting to extract atom items", new {content});
            var document = XDocument.Parse(content);
            var items = document.Root.Elements().Where(i => i.Name.LocalName == "entry").Select(item => new ExternalFeedItem
            {
                Content = item.Elements().First(i => i.Name.LocalName == "content").Value,
                Link = item.Elements().First(i => i.Name.LocalName == "link").Attribute("href")?.Value,
                PublishDate = item.Elements().First(i => i.Name.LocalName == "published").Value.ToDateTime(),
                Title = item.Elements().First(i => i.Name.LocalName == "title").Value
            });

            return Task.FromResult((IReadOnlyList<ExternalFeedItem>)items.ToList());
        }
    }

    public class RssFeedReader : IFeedReader
    {
        public Task<FeedDetails> GetFeedDetails(string content)
        {
            var document = XDocument.Parse(content);
            if (document.Root == null)
            {
                Logger.Error("Could not parse RSS Feed", new {content});
                throw new FeedException("Could not parse RSS feed");
            }

            var title = document.Root.Descendants()
                .First(d => d.Name.LocalName == "channel")
                .Descendants()
                .First(i => i.Name.LocalName == "title").Value;
            return Task.FromResult(new FeedDetails {Name = title});
        }

        public Task<IReadOnlyList<ExternalFeedItem>> GetItems(string content)
        {
            Logger.Trace("Attempting to extract rss items", new {content});
            var document = XDocument.Parse(content);

            var items = document.Root.Descendants()
                .First(i => i.Name.LocalName == "channel").Elements()
                .Where(i => i.Name.LocalName == "item").Select(item =>
            {
                Logger.Trace($"Processing item", item);
                try
                {
                    var description = item.Elements().FirstOrDefault(i => i.Name.LocalName == "description")?.Value;
                    if (description == null)
                    {
                        // holy carp this is stupid.
                        description = item.Elements().FirstOrDefault(i => i.Name.LocalName == "encoded")?.Value;
                    }

                    var extractedItem = new ExternalFeedItem
                    {
                        Content = description,
                        Link = item.Elements().First(i => i.Name.LocalName == "link").Value,
                        PublishDate = item.Elements().First(i => i.Name.LocalName == "pubDate").Value.ToDateTime(),
                        Title = item.Elements().First(i => i.Name.LocalName == "title").Value
                    };
                    return extractedItem;
                }
                catch (Exception e)
                {
                    Logger.Error("Could not extract item", new{e.Message, item});
                    return null;
                }
            });

            return Task.FromResult((IReadOnlyList<ExternalFeedItem>)items.ToList());
        }
    }

    public class FeedException : Exception
    {
        public FeedException(string message) : base(message)
        {
        }
    }
}
