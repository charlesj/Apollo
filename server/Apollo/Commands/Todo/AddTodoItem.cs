using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Todo
{
    public class AddTodoItem : AuthenticatedCommand
    {
        private readonly ITodoItemDataService todoItemDataService;
        public string Title { get; set; }

        public AddTodoItem(ILoginService loginService, ITodoItemDataService todoItemDataService) : base(loginService)
        {
            this.todoItemDataService = todoItemDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await this.todoItemDataService.AddItem(Title);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            var valid = !string.IsNullOrWhiteSpace(Title);
            return Task.FromResult(valid);
        }
    }
}
