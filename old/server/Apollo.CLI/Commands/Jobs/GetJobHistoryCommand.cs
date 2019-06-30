using System;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace Apollo.CLI.Commands.Jobs
{
    public class GetJobHistoryCommand : BaseCommand
    {
        public int JobId { get; set; }

        public GetJobHistoryCommand(CommandLineOptions options) : base(options)
        {
        }

        public override async Task Execute()
        {
            await Execute("getJobHistory", new { JobId });
        }

        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Gets the execution history of a given job";

            var jobIdArgument = command.Argument("id", "The id of the job");

            command.OnExecute(() =>
            {
                options.Command = new GetJobHistoryCommand(options)
                {
                    JobId = Convert.ToInt32(jobIdArgument.Value)
                };

                return 0;
            });
        }
    }
}
