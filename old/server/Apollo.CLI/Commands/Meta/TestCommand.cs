using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace Apollo.CLI.Commands.Meta
{
    public class TestCommand : BaseCommand
    {
        public TestCommand(CommandLineOptions options) : base(options)
        {
        }

        public override async Task Execute()
        {
            Options.SuppressOutput = true;
            var result = await Execute("testCommand", new object());
            Console.Green($"{result.Result.test}");
        }

        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Test connectivity to the server";
            command.OnExecute(() =>
            {
                options.Command = new TestCommand(options);
                return 0;
            });
        }
    }
}
