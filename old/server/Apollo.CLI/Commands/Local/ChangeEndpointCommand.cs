using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Newtonsoft.Json;

namespace Apollo.CLI.Commands.Local
{
    public class ChangeEndpointCommand : BaseCommand
    {
        public string NewEndpoint { get; set; }

        public ChangeEndpointCommand(CommandLineOptions options) : base(options)
        {
        }

        public override Task Execute()
        {
            if (string.IsNullOrWhiteSpace(NewEndpoint))
            {
                Console.Red("You must include the endpoint to switch to");
                return Task.CompletedTask;
            }

            var reader = new ConfigurationReader();
            var config = reader.GetConfiguration();
            config.Endpoint = NewEndpoint;
            reader.UpdateConfiguration(config);

            var newConfiguration = reader.GetConfiguration();
            Console.Green(JsonConvert.SerializeObject(newConfiguration, Formatting.Indented));

            return Task.CompletedTask;
        }

        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Change server endpoint";

            var endpointArgument = command.Argument("endpoint", "The endpoint URL. Example: http://192.168.142.10/api");

            command.OnExecute(() =>
            {
                options.Command = new ChangeEndpointCommand(options)
                {
                    NewEndpoint = endpointArgument.Value
                };

                return 0;
            });
        }
    }
}
