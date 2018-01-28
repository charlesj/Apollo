using System.Linq;
﻿using System.Threading.Tasks;
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
            var activeJobs = await jobsDataService.GetActiveJobs();

            if (Expired)
            {
                var expiredJobs = await jobsDataService.GetExpiredJobs();
                return CommandResult.CreateSuccessResult(activeJobs.Concat(expiredJobs));
            }

            return CommandResult.CreateSuccessResult(activeJobs);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }

        public override object ExamplePayload()
        {
            return new { Expired="optional" };
        }
    }
}
