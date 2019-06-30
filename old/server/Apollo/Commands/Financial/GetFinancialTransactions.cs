using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Financial
{
    public class GetFinancialTransactions : AuthenticatedCommand
    {
        private readonly IFinancialAccountDataService dataService;
        public int account_id { get; set; }

        public GetFinancialTransactions(ILoginService loginService, IFinancialAccountDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(await dataService.GetTransactions(account_id));
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(account_id != default(int));
        }

        public override object ExamplePayload()
        {
            return new {account_id= 1};
        }
    }
}
