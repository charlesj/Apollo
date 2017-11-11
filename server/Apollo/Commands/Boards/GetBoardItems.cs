using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Boards
{
    public class GetBoardItems : AuthenticatedCommand
    {
        private readonly IBoardDataService dataService;

        public int board_id { get; set; }

        public GetBoardItems(ILoginService loginService, IBoardDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(await dataService.GetBoardItems(board_id));
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(board_id > 0);
        }
    }
}
