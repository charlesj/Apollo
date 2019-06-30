using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Metrics
{
    public class GetMetrics : AuthenticatedCommand
    {
        private readonly IMetricsDataService metricsDataService;

        public string Category { get; set; }
        public string Name { get; set; }

        public GetMetrics(
            ILoginService loginService,
            IMetricsDataService metricsDataService) : base(loginService)
        {
            this.metricsDataService = metricsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var metrics = await this.metricsDataService.GetMetrics(Category, Name);
            return new CommandResult {ResultStatus = CommandResultType.Success, Result = metrics};
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }

        public override object ExamplePayload()
        {
            return new {category="optional", name="optional" };
        }
    }
}
