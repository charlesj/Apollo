using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Bookmarks
{
    public class DeleteBookmark : AuthenticatedCommand
    {
        private readonly IBookmarksDataService dataService;
        public int id { get; set; }

        public DeleteBookmark(ILoginService loginService, IBookmarksDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.Delete(id);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(id != default(int));
        }
    }
}
