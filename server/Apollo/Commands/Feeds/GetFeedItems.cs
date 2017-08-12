using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Feeds
{
    public class GetFeedItems : AuthenticatedCommand
    {
        private readonly IFeedDataService feedDataService;
        public int FeedId { get; set; }

        public GetFeedItems(IFeedDataService feedDataService,ILoginService loginService) : base(loginService)
        {
            this.feedDataService = feedDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var items = await feedDataService.GetItems(FeedId);
            return CommandResult.CreateSuccessResult(items);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(FeedId != 0);
        }
    }
}
