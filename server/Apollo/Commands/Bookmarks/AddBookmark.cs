using System;
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
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public List<string> tags { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime modifiedAt { get; set; }

        public AddBookmark(ILoginService loginService, IBookmarksDataService bookmarksDataService) : base(loginService)
        {
            this.bookmarksDataService = bookmarksDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var bookmark = new Bookmark
            {
                title = title,
                link = link,
                description = description,
                tags = tags.ToArray(),
                created_at = createdAt,
                modified_at = modifiedAt
            };

            await this.bookmarksDataService.Insert(bookmark);

            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            if (string.IsNullOrWhiteSpace(title))
                return Task.FromResult(false);
            if (string.IsNullOrWhiteSpace(link))
                return Task.FromResult(false);
            return Task.FromResult(true);
        }

        public override object ExamplePayload()
        {
            return new { title, link, description, tags=new[] {"tag1", "tag2"}, createdAt, modifiedAt};
        }
    }
}
