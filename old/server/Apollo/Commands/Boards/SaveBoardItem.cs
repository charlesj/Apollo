using System;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Boards
{
    public class SaveBoardItem : AuthenticatedCommand
    {
        private readonly IBoardDataService dataService;

        public int id { get; set; }
        public int board_id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public DateTime? completed_at { get; set; }

        public SaveBoardItem(ILoginService loginService, IBoardDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var updated = await dataService.SaveItem(new BoardItem
            {
                id = id,
                board_id = board_id,
                title = title,
                link = link,
                description = description,
                completed_at = completed_at
            });

            return CommandResult.CreateSuccessResult(updated);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(title) && link != null && description != null && board_id > 0);
        }

        public override object ExamplePayload()
        {
            return new { id=0, board_id=0, title, link, description, completed_at};
        }
    }
}
