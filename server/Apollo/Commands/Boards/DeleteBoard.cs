using System.Linq;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Boards
{
    public class DeleteBoard : AuthenticatedCommand
    {
        private readonly IBoardDataService dataService;
        public int id { get; set; }

        public DeleteBoard(ILoginService loginService, IBoardDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.DeleteBoard(id);
            return CommandResult.SuccessfulResult;
        }

        public override async Task<bool> IsValid()
        {
            var items = await dataService.GetBoardItems(id);
            return !items.Any();
        }

        public override object ExamplePayload()
        {
            return new { id=0};
        }
    }
}
