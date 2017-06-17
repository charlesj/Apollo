using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Metrics
{
    public class AddMetric : AuthenticatedCommand
    {
        private readonly IMetricsDataService metricsDataService;
        
        public string Category { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
        
        public AddMetric(
            ILoginService loginService,
            IMetricsDataService metricsDataService
        ) : base(loginService)
        {
            this.metricsDataService = metricsDataService;
            this.Value = default(float);
        }

        public override async Task<CommandResult> Execute()
        {
            await this.metricsDataService.InsertMetric(Category, Name, Value);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            if (string.IsNullOrWhiteSpace(Category) ||
                string.IsNullOrWhiteSpace(Name))
                return Task.FromResult(false);
            return Task.FromResult(true);
        }
    }
}