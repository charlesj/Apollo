using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Todo
{
    public class UpdateTodoQueueItem : AuthenticatedCommand
    {
        private readonly ITodoQueueItemDataService dataService;
        public TodoQueueItem Item { get; set; }

        public UpdateTodoQueueItem(ILoginService loginService, ITodoQueueItemDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.UpdateItem(Item);
            return CommandResult.SuccessfulResult;
        }

        public override async Task<bool> IsValid()
        {
            await Task.CompletedTask;
            return !string.IsNullOrWhiteSpace(Item?.title) &&
                Item?.id > 0;
        }
    }
}
