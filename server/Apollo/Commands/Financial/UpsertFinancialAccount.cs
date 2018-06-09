using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Financial
{
    public class UpsertFinancialAccount : AuthenticatedCommand
    {
        private readonly IFinancialAccountDataService dataService;
        public FinancialAccount Account { get; set; }

        public UpsertFinancialAccount(ILoginService loginService, IFinancialAccountDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(await dataService.UpsertAccount(Account));
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(Account.name) &&
                                   !string.IsNullOrWhiteSpace(Account.type) &&
                                   !string.IsNullOrWhiteSpace(Account.description));
        }

        public override object ExamplePayload()
        {
            return new {account= new FinancialAccount()};
        }
    }
}
