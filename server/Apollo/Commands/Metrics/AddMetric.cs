using System;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Metrics
{
    public class AddMetric : AuthenticatedCommand
    {
        private readonly IMetricsDataService metricsDataService;

        public string category { get; set; }
        public string name { get; set; }
        public decimal value { get; set; }

        public AddMetric(
            ILoginService loginService,
            IMetricsDataService metricsDataService
        ) : base(loginService)
        {
            Logger.Trace("Created AddMetric Instance");
            this.metricsDataService = metricsDataService;
            this.value = default(decimal);
        }

        public override async Task<CommandResult> Execute()
        {
            Logger.Trace("Began Executing AddMetric");
            await this.metricsDataService.InsertMetric(category, name, value);
            Logger.Trace("Left Data Service");
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            if (string.IsNullOrWhiteSpace(category) ||
                string.IsNullOrWhiteSpace(name))
                return Task.FromResult(false);
            return Task.FromResult(true);
        }

        public override object ExamplePayload()
        {
            return new { category, name, value};
        }
    }
}
