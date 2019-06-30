using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace Apollo.CLI.Commands.Feeds
{
    public class AddFeedCommand : BaseCommand
    {
        public string FeedUrl { get; set; }

        public AddFeedCommand(CommandLineOptions options) : base(options)
        {
        }

        public override async Task Execute()
        {
            if (string.IsNullOrEmpty(FeedUrl))
            {
                Console.Red("Must pass feed url");
            }

            await Execute("addFeed", new {url = FeedUrl});
        }


        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Subscribes to a new RSS or Atom feed";

            var feedOption = command.Argument("feed", "The url of the feed");

            command.OnExecute(() =>
            {
                options.Command = new AddFeedCommand(options)
                {
                    FeedUrl = feedOption.Value,
                };

                return 0;
            });
        }
    }
}
