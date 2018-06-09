using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Financial
{
    public class GetFinancialTransactionTypes : AuthenticatedCommand
    {
        private readonly IFinancialAccountDataService dataService;

        public GetFinancialTransactionTypes(ILoginService loginService, IFinancialAccountDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(await dataService.GetTransactionTypes());
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }

        public override object ExamplePayload()
        {
            return new {};
        }
    }
}
