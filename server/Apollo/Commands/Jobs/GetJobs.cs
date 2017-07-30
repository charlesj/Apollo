using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Jobs
{
    public class GetJobs : AuthenticatedCommand
    {
        public bool Expired { get; set; }

        private readonly IJobsDataService jobsDataService;

        public GetJobs(IJobsDataService jobsDataService, ILoginService loginService) : base(loginService)
        {
            this.jobsDataService = jobsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            if (Expired)
            {
                return CommandResult.CreateSuccessResult(await jobsDataService.GetExpiredJobs());
            }

            return CommandResult.CreateSuccessResult(await jobsDataService.GetActiveJobs());
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
