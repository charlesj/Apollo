using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Financial
{
    public class GetHistoricalAssetPrices : AuthenticatedCommand
    {
        private readonly IFinancialAssetPriceDataService dataService;
        public string Symbol { get; set; }

        public GetHistoricalAssetPrices(IFinancialAssetPriceDataService dataService, ILoginService loginService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(await dataService.GetHistoricalPrices(Symbol));
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(Symbol));
        }

        public override object ExamplePayload()
        {
            return new { Symbol };
        }
    }
}
