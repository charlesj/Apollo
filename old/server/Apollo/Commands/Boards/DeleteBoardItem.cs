using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Boards
{
    public class DeleteBoardItem : AuthenticatedCommand
    {
        private readonly IBoardDataService dataService;
        public int id { get; set; }

        public DeleteBoardItem(ILoginService loginService, IBoardDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.DeleteBoardItem(id);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }

        public override object ExamplePayload()
        {
            return new { id=0};
        }
    }
}
