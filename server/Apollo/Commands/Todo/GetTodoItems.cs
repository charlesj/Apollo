using System.Linq;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Todo
{
    public class GetTodoItems : AuthenticatedCommand
    {
        private readonly ITodoItemDataService todoItemDataService;

        public GetTodoItems(ILoginService loginService, ITodoItemDataService todoItemDataService) : base(loginService)
        {
            this.todoItemDataService = todoItemDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var incompleteItems = await todoItemDataService.GetIncompleteItems();
            var recentlyCompletedItems = await todoItemDataService.GetRecentlyCompletedItems();
            var all = incompleteItems.Concat(recentlyCompletedItems);
            return CommandResult.CreateSuccessResult(all);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
