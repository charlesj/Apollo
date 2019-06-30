using System;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.External.Coinbase;
using Apollo.Services;

namespace Apollo.Commands.Financial
{
    public class UpdateCryptoPrices : AuthenticatedCommand
    {
        private readonly IFinancialAssetPriceDataService dataService;
        private readonly IGdaxClient gdaxClient;

        public UpdateCryptoPrices(
            IFinancialAssetPriceDataService dataService,
            IGdaxClient gdaxClient,
            ILoginService loginService) : base(loginService)
        {
            this.dataService = dataService;
            this.gdaxClient = gdaxClient;
        }

        public override async Task<CommandResult> Execute()
        {
            var products = new[] {"btc", "eth", "ltc"};
            foreach (var product in products)
            {
                var price = await gdaxClient.GetProductTicker($"{product}-usd");
                await dataService.RecordPrice(product, decimal.Parse(price.Price), "gdax", DateTimeOffset.UtcNow);
            }

            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
