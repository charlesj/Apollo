using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Boards
{
    public class GetBoards : AuthenticatedCommand
    {
        private readonly IBoardDataService dataService;

        public GetBoards(ILoginService loginService, IBoardDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(await dataService.GetBoards());
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
