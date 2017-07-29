using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Todo
{
    public class AddTodoQueueItem : AuthenticatedCommand
    {
        private readonly ITodoQueueItemDataService dataDataService;
        public TodoQueueItem Item { get; set; }

        public AddTodoQueueItem(
            ILoginService loginService,
            ITodoQueueItemDataService dataDataService) : base(loginService)
        {
            this.dataDataService = dataDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataDataService.AddItem(Item);
            return CommandResult.SuccessfulResult;
        }

        public override async Task<bool> IsValid()
        {
            await Task.CompletedTask;
            return !string.IsNullOrWhiteSpace(Item?.title);
        }
    }
}
