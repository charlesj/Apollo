using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Jobs
{
    public class CancelJob : AuthenticatedCommand
    {
        private readonly IJobsDataService jobsDataService;

        public int JobId { get; set; }

        public CancelJob(IJobsDataService jobsDataService, ILoginService loginService) : base(loginService)
        {
            this.jobsDataService = jobsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await jobsDataService.CancelJob(JobId);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(JobId > 0);
        }
    }
}
