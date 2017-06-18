using System.Collections.Generic;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Bookmarks
{
    public class AddBookmark : AuthenticatedCommand
    {
        private readonly IBookmarksDataService bookmarksDataService;
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
        
        public AddBookmark(ILoginService loginService, IBookmarksDataService bookmarksDataService) : base(loginService)
        {
            this.bookmarksDataService = bookmarksDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var bookmark = new Bookmark
            {
                title = Title,
                link = Link,
                description = Description,
                tags = Tags.ToArray()
            };

            await this.bookmarksDataService.Insert(bookmark);
            
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            if (string.IsNullOrWhiteSpace(Title))
                return Task.FromResult(false);
            if (string.IsNullOrWhiteSpace(Link))
                return Task.FromResult(false);
            return Task.FromResult(true);
        }
    }
}