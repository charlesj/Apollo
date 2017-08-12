using System.Linq;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.External;
using Apollo.Services;

namespace Apollo.Commands.Feeds
{
    public class AddFeed : AuthenticatedCommand
    {
        private readonly IExternalFeedService feedService;
        private readonly IFeedDataService feedDataService;
        public string Url { get; set; }

        public AddFeed(
            IExternalFeedService feedService,
            IFeedDataService feedDataService,
            ILoginService loginService) : base(loginService)
        {
            this.feedService = feedService;
            this.feedDataService = feedDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var feedDetails = await feedService.GetFeedDetails(Url);
            var feeds = await feedDataService.GetFeeds();
            if (feeds.Any(f => f.url == Url))
            {
                return CommandResult.CreateSuccessResult(new
                {
                    feedDetails,
                    url=Url,
                    message="Already subscribed to feed"
                });
            }

            await feedDataService.AddFeed(feedDetails.Name, Url);
            return CommandResult.CreateSuccessResult(new {feedDetails});
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(Url));
        }
    }
}
