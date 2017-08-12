using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Feeds
{
    public class GetFeeds : AuthenticatedCommand
    {
        private readonly IFeedDataService dataService;

        public GetFeeds(IFeedDataService dataService, ILoginService loginService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var feeds = await dataService.GetFeeds();
            return CommandResult.CreateSuccessResult(feeds);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
