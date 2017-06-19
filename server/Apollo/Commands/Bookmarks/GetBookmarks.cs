using System.Collections.Generic;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Bookmarks
{
    public class GetBookmarks : AuthenticatedCommand
    {
        private readonly IBookmarksDataService bookmarksDataService;
        public int Start { get; set; }

        public GetBookmarks(ILoginService loginService, IBookmarksDataService bookmarksDataService) : base(loginService)
        {
            this.bookmarksDataService = bookmarksDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var result = new GetBookmarksResult();
            result.total = await bookmarksDataService.GetTotal();
            result.bookmarks = await bookmarksDataService.GetPage(Start);
            return CommandResult.CreateSuccessResult(result);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }

        public class GetBookmarksResult
        {
            public int total { get; set; }
            public IReadOnlyList<Bookmark> bookmarks { get; set; }
        }
    }
}