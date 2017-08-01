using System;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Jobs
{
    public class AddJob : AuthenticatedCommand
    {
        private readonly ICommandLocator commandLocator;
        private readonly ICommandHydrator commandHydrator;
        private readonly IJobsDataService jobsDataService;

        public string CommandName { get; set; }
        public object Parameters { get; set; }
        public Schedule Schedule { get; set; }

        public AddJob(
            ICommandLocator commandLocator,
            ICommandHydrator commandHydrator,
            IJobsDataService jobsDataService,
            ILoginService loginService) : base(loginService)
        {
            this.commandLocator = commandLocator;
            this.commandHydrator = commandHydrator;
            this.jobsDataService = jobsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            //Logger.Info("Scheduling new Job", new {CommandName, Parameters, Schedule});
            await jobsDataService.AddJob(CommandName, Parameters, Schedule);
            return CommandResult.SuccessfulResult;
        }

        public override async Task<bool> IsValid()
        {
            var command = commandLocator.Locate(CommandName);
            var validCommandName = command != null;
            commandHydrator.Hydrate(ref command, Parameters);
            var validParameters = await command.IsValid();
            var validSchedule = Schedule != null &&
                                Schedule.start != default(DateTime) &&
                                (Schedule.repeat_count == null || Schedule.repeat_count > 0);

            return validCommandName && validParameters && validSchedule;
        }
    }
}
