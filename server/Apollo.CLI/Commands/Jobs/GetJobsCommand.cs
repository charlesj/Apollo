using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace Apollo.CLI.Commands.Jobs
{
    public class GetJobsCommand : BaseCommand
    {
        public GetJobsCommand(CommandLineOptions options) : base(options)
        {
        }

        public override async Task Execute()
        {
            await Execute("getJobs", new { });
        }

        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Gets the active jobs on the server";


            command.OnExecute(() =>
            {
                options.Command = new GetJobsCommand(options);

                return 0;
            });
        }
    }
}
