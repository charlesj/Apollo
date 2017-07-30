using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Jobs
{
    public class GetActiveJobs : AuthenticatedCommand
    {
        private readonly IJobsDataService jobsDataService;

        public GetActiveJobs(IJobsDataService jobsDataService, ILoginService loginService) : base(loginService)
        {
            this.jobsDataService = jobsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var activeJobs = await jobsDataService.GetActiveJobs();
            return CommandResult.CreateSuccessResult(activeJobs);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
