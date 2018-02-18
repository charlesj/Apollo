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
            var updatedItem = await dataService.MarkItemRead(itemId);
            return CommandResult.CreateSuccessResult(updatedItem);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(itemId > 0);
        }

        public override object ExamplePayload()
        {
            return new { itemId };
        }
    }
}
