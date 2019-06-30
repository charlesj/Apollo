using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.External;
using Apollo.Services;

namespace Apollo.Commands.Feeds
{
    public class CheckFeedsForUpdates : AuthenticatedCommand
    {
        private readonly IExternalFeedService feedService;
        private readonly IFeedDataService feedDataService;

        public CheckFeedsForUpdates(
            IExternalFeedService feedService,
            IFeedDataService feedDataService,
            ILoginService loginService) : base(loginService)
        {
            this.feedService = feedService;
            this.feedDataService = feedDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var feeds = await feedDataService.GetFeeds();
            var results = new List<Tuple<Feed, bool>>();
            foreach (var feed in feeds)
            {
                var result = await ProcessFeed(feed);
                results.Add(Tuple.Create(feed, result));
            }

            return CommandResult.CreateSuccessResult(results);
        }

        private async Task<bool> ProcessFeed(Feed feed)
        {
            try
            {
                Logger.Trace($"Processing feed {feed.name}");
                await feedDataService.UpdateLastAttempt(feed.id);
                var items = await feedService.GetItems(feed);
                if (items != null)
                {
                    Logger.Trace("Found items", new {feed, items});
                    foreach (var item in items)
                    {
                        if(item == null) continue;

                        var existingItems = await feedDataService.CheckItemUrl(item.Link);
                        if (!existingItems.Any())
                        {
                            await feedDataService.AddItem(feed.id, item.Title, item.Link, item.Content, item.PublishDate);
                        }
                    }

                    await feedDataService.MarkUpdated(feed.id);
                    Logger.Trace($"Finished processing feed {feed.name}");
                }
                else
                {
                    Logger.Error("Error getting items for feed", feed);
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Logger.Error("Error getting items for feed", new{feed, e.Message});
                return false;
            }
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
