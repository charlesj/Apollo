using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Journal
{
    public class GetAllJournalEntries : AuthenticatedCommand
    {
        private readonly IJournalDataService journalDataService;

        public GetAllJournalEntries(IJournalDataService journalDataService, ILoginService loginService) : base(loginService)
        {
            this.journalDataService = journalDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var entries = await journalDataService.GetAllJournalEntries();
            return new CommandResult {ResultStatus = CommandResultType.Success, Result = entries};
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}