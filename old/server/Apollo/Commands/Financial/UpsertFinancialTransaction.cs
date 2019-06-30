using System;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Financial
{
    public class UpsertFinancialTransaction : AuthenticatedCommand
    {
        private readonly IFinancialAccountDataService dataService;
        public FinancialTransaction Transaction { get; set; }

        public UpsertFinancialTransaction(ILoginService loginService, IFinancialAccountDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(await dataService.UpsertTransaction(Transaction));
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(
                Transaction.account_id != default(int) &&
                Transaction.tags.Length > 0 &&
                Transaction.occurred_at != default(DateTime) &&
                !string.IsNullOrWhiteSpace(Transaction.name) &&
                !string.IsNullOrWhiteSpace(Transaction.notes));
        }

        public override object ExamplePayload()
        {
            return new {transaction= new FinancialTransaction()};
        }
    }
}
