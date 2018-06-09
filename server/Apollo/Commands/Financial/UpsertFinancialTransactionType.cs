using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Financial
{
    public class UpsertFinancialTransactionType : AuthenticatedCommand
    {
        private readonly IFinancialAccountDataService dataService;
        public FinancialTransactionType TransactionType { get; set; }

        public UpsertFinancialTransactionType(ILoginService loginService, IFinancialAccountDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(await dataService.UpsertTransactionType(TransactionType));
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(TransactionType.name) &&
                                   !string.IsNullOrWhiteSpace(TransactionType.description));
        }

        public override object ExamplePayload()
        {
            return new {transactionType= new FinancialTransactionType()};
        }
    }
}
