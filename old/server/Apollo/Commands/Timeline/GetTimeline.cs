using System;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Timeline
{
    public class GetTimeline : AuthenticatedCommand
    {
        private readonly ITimelineDataService tds;
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public GetTimeline(ILoginService loginService, ITimelineDataService tds) : base(loginService)
        {
            this.tds = tds;
        }

        public override async Task<CommandResult> Execute()
        {
            if(Start == null) Start = DateTime.UtcNow.AddMonths(-1);
            var entries = await tds.GetEntries(Start.Value, End);
            return CommandResult.CreateSuccessResult(entries);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
