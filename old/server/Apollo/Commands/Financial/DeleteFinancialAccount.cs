using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Financial
{
    public class DeleteFinancialAccount : AuthenticatedCommand
    {
        private readonly IFinancialAccountDataService dataService;
        public int account_id { get; set; }

        public DeleteFinancialAccount(ILoginService loginService, IFinancialAccountDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.DeleteAccount(account_id);
            return CommandResult.SuccessfulResult;
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
