using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Newtonsoft.Json;

namespace Apollo.CLI.Commands.Local
{
    public class DisplayConfigurationCommand : BaseCommand
    {
        public DisplayConfigurationCommand(CommandLineOptions options) : base(options)
        {
        }

        public override Task Execute()
        {
            var configuration = new ConfigurationReader();
            Console.Green(JsonConvert.SerializeObject(configuration.GetConfiguration(), Formatting.Indented));
            return Task.CompletedTask;
        }

        public static void Configure(CommandLineApplication app, CommandLineOptions options)
        {
            app.Description = "Display local configuration";

            app.OnExecute(() =>
            {
                options.Command = new DisplayConfigurationCommand(options);
                return 0;
            });
        }
    }
}
