using System.IO;
using System.Linq;
﻿using System.Threading.Tasks;
using System.Xml.Linq;
using Apollo.Data;
using Apollo.External;
using Apollo.Services;
using Microsoft.Extensions.CommandLineUtils;

namespace Apollo.CLI.Commands.Feeds
{
    public class ImportFeedsCommand : BaseCommand
    {
        public string FeedUrl { get; set; }

        public ImportFeedsCommand(CommandLineOptions options) : base(options)
        {
        }

        public override async Task Execute()
        {
            Logger.Enabled = true;
            //Logger.TraceEnabled = true;
            var feedService = new ExternalFeedService(new UrlFetcher());

            var content = string.Join("", File.ReadAllLines("/home/josh/feeds_formatted.xml"));
            var document = XDocument.Parse(content);
            var outlineElements = document.Root.Elements()
                                          .Where(i => i.Name.LocalName=="body").Elements()
                                          .Where(i => i.Name.LocalName=="outline").ToList();

            Console.Green($"Found {outlineElements.Count} feeds to import");
            foreach (var outlineElement in outlineElements)
            {
                var rssUrl = outlineElement.Attribute("xmlUrl")?.Value;
                Console.Write($"Attempting to subscribe to {rssUrl}: ");
                await Execute("addFeed", new {url = rssUrl});
                Console.Green("Done");
            }
            //var items = document.Root.Elements().Where(i => i.Name.LocalName == "entry").Select(item => new ExternalFeedItem
            //{
            //    Content = item.Elements().First(i => i.Name.LocalName == "content").Value,
            //    Link = item.Elements().First(i => i.Name.LocalName == "link").Attribute("href")?.Value,
            //    PublishDate = item.Elements().First(i => i.Name.LocalName == "published").Value.ToDateTime(),
            //    Title = item.Elements().First(i => i.Name.LocalName == "title").Value
            //});
            //var items = await feedService.GetItems(new Feed{url = FeedUrl});
            //Console.Green($"Found {items.Count} items");
            //Console.Write(items.ToJson());
        }


        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Test a RSS or Atom feed";



            command.OnExecute(() =>
            {
                options.Command = new ImportFeedsCommand(options);
                return 0;
            });
        }
    }
}
