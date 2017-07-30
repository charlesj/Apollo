using System;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Newtonsoft.Json;

namespace Apollo.CLI.Commands.Jobs
{
    public class AddJobCommand : BaseCommand
    {
        public string CommandName { get; set; }
        public string ParametersJson { get; set; }
        public int RepeatCount { get; set; }
        public bool Hourly { get; set; }
        public bool Daily { get; set; }

        public AddJobCommand(CommandLineOptions options) : base(options)
        {
        }

        public override async Task Execute()
        {
            var jobParameters = new object();
            if (string.IsNullOrEmpty(CommandName))
            {
                Console.Red("must set command name");
                return;
            }
            if (!string.IsNullOrWhiteSpace(ParametersJson))
                jobParameters = JsonConvert.DeserializeObject<object>(ParametersJson);

            var parameters = new
            {
                CommandName,
                parameters = jobParameters,
                Schedule = new
                {
                    start = DateTime.Now,
                    repeat_count = RepeatCount,
                    hourly = Hourly
                }
            };

            await Execute("addJob", parameters);
            Console.Green("Job Scheduled");
        }

        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Gets the execution history of a given job";

            var commandName = command.Option("--name", "The name of the command to run", CommandOptionType.SingleValue);
            var parameters = command.Option("-p", "Parameters json", CommandOptionType.SingleValue);
            var repeatOption = command.Option("-r", "The number of times to repeat", CommandOptionType.SingleValue);
            var hourlyOption = command.Option("--hourly", "whether to repeat hourly", CommandOptionType.NoValue);

            command.OnExecute(() =>
            {
                options.Command = new AddJobCommand(options)
                {
                    CommandName = commandName.Value(),
                    ParametersJson = parameters.Value(),
                    RepeatCount = Convert.ToInt32(repeatOption.Value()),
                    Hourly = hourlyOption.HasValue()
                };

                return 0;
            });
        }
    }
}
