using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Financial
{
    public class GetFinancialAccounts : AuthenticatedCommand
    {
        private readonly IFinancialAccountDataService dataService;

        public GetFinancialAccounts(ILoginService loginService, IFinancialAccountDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var accounts = await dataService.GetAccounts();
            return CommandResult.CreateSuccessResult(accounts);
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
