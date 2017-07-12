using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Todo
{
    public class UpdateTodoItem : AuthenticatedCommand
    {
        private readonly ITodoItemDataService todoItemDataService;

        public TodoItem Item { get; set; }

        public UpdateTodoItem(
            ITodoItemDataService todoItemDataService,
            ILoginService loginService) : base(loginService)
        {
            this.todoItemDataService = todoItemDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await todoItemDataService.UpdateItem(Item);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            var isValid = true;
            if (Item == null)
                isValid = false;
            if (Item?.id == default(int))
                isValid = false;
            if (string.IsNullOrWhiteSpace(Item?.title))
                isValid = false;
            return Task.FromResult(isValid);
        }
    }
}
