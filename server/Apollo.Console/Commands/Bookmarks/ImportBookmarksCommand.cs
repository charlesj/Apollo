using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Apollo.Utilities;
using CommandLine;
using Newtonsoft.Json;

namespace Apollo.Console.Commands.Bookmarks
{
    public class ImportBookmarksCommandOptions : CommandOptionsBase<ImportBookmarksCommand>
    {
        [Option('f', "file", HelpText = "path to the bookmark.json", Required = true)]
        public string FilePath { get; set; }
    }
    
    public class ImportBookmarksCommand : BaseCommand<ImportBookmarksCommandOptions>
    {
        public ImportBookmarksCommand(ImportBookmarksCommandOptions options) : base(options)
        {
        }

        public override void Execute()
        {
            var json = string.Join(Environment.NewLine, File.ReadAllLines(Options.FilePath));
            var bookmarks = JsonConvert.DeserializeObject<List<Bookmark>>(json);
            foreach (var bookmark in bookmarks)
            {
                Console.Yellow($"Importing bookmark {bookmark.url}...", false);

                var result = Execute("addBookmark", new
                {
                    bookmark.title,
                    link = bookmark.url,
                    bookmark.tags,
                    description = bookmark.comment,
                    createdAt = bookmark.add_date,
                    modifiedAt =  bookmark.add_date
                });

                if (result?.ErrorMessage != null)
                {
                    Console.Red((string)result.ErrorMessage);
                    continue;
                }
                
                
                Console.Green("success");
            }
        }

        public class Bookmark
        {
            public string[] tags { get; set; }
            public string url { get; set; }
            public string add_date { get; set; }
            public string title { get; set; }
            public string comment { get; set; }
        }
    }
}