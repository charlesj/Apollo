using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Log
{
    public class GetJournalEntries : AuthenticatedCommand
    {
        private readonly IJournalDataService journalDataService;

        public int start { get; set; }

        public GetJournalEntries(IJournalDataService journalDataService, ILoginService loginService) : base(loginService)
        {
            this.journalDataService = journalDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var entries = await journalDataService.GetJournalEntries(start);
            var total = await journalDataService.GetCount();
            return CommandResult.CreateSuccessResult(new {entries, total});
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
