using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Financial
{
    public class DeleteFinancialTransaction : AuthenticatedCommand
    {
        private readonly IFinancialAccountDataService dataService;
        public int transaction_id { get; set; }

        public DeleteFinancialTransaction(ILoginService loginService, IFinancialAccountDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.DeleteTransaction(transaction_id);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(transaction_id != default(int));
        }

        public override object ExamplePayload()
        {
            return new {transaction_id= 1};
        }
    }
}
