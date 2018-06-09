using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Financial
{
    public class DeleteFinancialTransactionType : AuthenticatedCommand
    {
        private readonly IFinancialAccountDataService dataService;
        public int transaction_type_id { get; set; }

        public DeleteFinancialTransactionType(ILoginService loginService, IFinancialAccountDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.DeleteTransactionType(transaction_type_id);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(transaction_type_id != default(int));
        }

        public override object ExamplePayload()
        {
            return new {transaction_type_id= 1};
        }
    }
}
