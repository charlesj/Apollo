using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Feeds
{
    public class MarkItemAsRead : AuthenticatedCommand
    {
        private readonly IFeedDataService dataService;
        public int itemId { get; set; }

        public MarkItemAsRead(IFeedDataService dataService, ILoginService loginService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.MarkItemRead(itemId);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(itemId > 0);
        }
    }
}
