using System.Threading.Tasks;
using Apollo.Data;
using Apollo.External;
using Apollo.Services;
using Microsoft.Extensions.CommandLineUtils;

namespace Apollo.CLI.Commands.Feeds
{
    public class TestFeedCommand : BaseCommand
    {
        public string FeedUrl { get; set; }

        public TestFeedCommand(CommandLineOptions options) : base(options)
        {
        }

        public override async Task Execute()
        {
            Logger.Enabled = true;
            Logger.TraceEnabled = true;
            if (string.IsNullOrEmpty(FeedUrl))
            {
                Console.Red("Must pass feed url");
            }

            var feedService = new ExternalFeedService(new UrlFetcher());
            var items = await feedService.GetItems(new Feed{url = FeedUrl});
            Console.Green($"Found {items.Count} items");
            Console.Write(items.ToJson());
        }


        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Test a RSS or Atom feed";

            var feedOption = command.Argument("feed", "The url of the feed");

            command.OnExecute(() =>
            {
                options.Command = new TestFeedCommand(options)
                {
                    FeedUrl = feedOption.Value,
                };

                return 0;
            });
        }
    }
}
