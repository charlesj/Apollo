using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Boards
{
    public class SaveBoard : AuthenticatedCommand
    {
        private readonly IBoardDataService dataService;
        public Board board { get; set; }


        public SaveBoard(ILoginService loginService, IBoardDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var updated = await dataService.SaveBoard(board);
            return CommandResult.CreateSuccessResult(updated);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(board.title) && board.list_order >= 0);
        }

        public override object ExamplePayload()
        {
            return new { board = new {id=0, title="title", list_order=1}};
        }
    }
}
