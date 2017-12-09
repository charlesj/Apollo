using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Boards
{
    public class AddBoard : AuthenticatedCommand
    {
        private readonly IBoardDataService dataService;
        public string title { get; set; }
        public int list_order { get; set; }

        public AddBoard(ILoginService loginService, IBoardDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.AddBoard(new Board {title = title, list_order = list_order});
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(title) && list_order >= 0);
        }

        public override object ExamplePayload()
        {
            return new {title = "board title", list_order = 1};
        }
    }
}
