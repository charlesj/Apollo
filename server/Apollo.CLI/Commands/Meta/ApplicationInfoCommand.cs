using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace Apollo.CLI.Commands.Meta
{
    public class ApplicationInfoCommand : BaseCommand
    {
        public ApplicationInfoCommand(CommandLineOptions options) : base(options)
        {
        }

        public override async Task Execute()
        {
            await Execute("applicationinfo", new object());
        }

        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Get information about the server";
            command.OnExecute(() =>
            {
                options.Command = new ApplicationInfoCommand(options);
                return 0;
            });
        }
    }
}
