using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Boards
{
    public class UpdateBoard : AuthenticatedCommand
    {
        private readonly IBoardDataService dataService;
        public int id { get; set; }
        public string title { get; set; }
        public int list_order { get; set; }

        public UpdateBoard(ILoginService loginService, IBoardDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.UpdateBoard(new Board {id=id, title = title, list_order = list_order});
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(title) && id > 0 && list_order >= 0);
        }

        public override object ExamplePayload()
        {
            return new { id=0, title, list_order=1};
        }
    }
}
