using System.Threading.Tasks;
using Apollo.CommandSystem;
using Microsoft.Extensions.CommandLineUtils;

namespace Apollo.CLI.Commands.Weather
{
    public class UpdateForecastsCommand : BaseCommand
    {
        public string ApiKey { get; set; }

        public UpdateForecastsCommand(CommandLineOptions options) : base(options)
        {
        }

        public override async Task Execute()
        {
            var result = await Execute("UpdateWeatherForecasts", new { ApiKey });
            if (result.ResultStatus == CommandResultType.Success)
            {
                Console.Green("Weather Updated");
            }
            else
            {
                Console.Red("Error updating weather");
            }
        }

        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Updates the weather forecast for the tracked locations";

            var apiKey = command.Option("--apiKey", "The DarkSky api key", CommandOptionType.SingleValue);

            command.OnExecute(() =>
            {
                options.Command = new UpdateForecastsCommand(options)
                {
                    ApiKey = apiKey.Value()
                };
                return 0;
            });
        }
    }
}
