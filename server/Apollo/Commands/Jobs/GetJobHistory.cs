using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Jobs
{
    public class GetJobHistory : AuthenticatedCommand
    {
        private readonly IJobsDataService jobsDataService;
        public int JobId { get; set; }

        public GetJobHistory(IJobsDataService jobsDataService,ILoginService loginService) : base(loginService)
        {
            this.jobsDataService = jobsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var history = await jobsDataService.GetJobHistory(JobId);
            return CommandResult.CreateSuccessResult(history);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(JobId > 0);
        }
    }
}
