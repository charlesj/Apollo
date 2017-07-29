using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Todo
{
    public class GetTodoQueueItems : AuthenticatedCommand
    {
        private readonly ITodoQueueItemDataService dataService;

        public GetTodoQueueItems(ILoginService loginService, ITodoQueueItemDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var items = await dataService.GetIncompleteItems();
            return CommandResult.CreateSuccessResult(items);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
