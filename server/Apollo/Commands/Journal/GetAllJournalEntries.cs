using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;

namespace Apollo.Commands.Journal
{
    public class GetAllJournalEntries : ICommand
    {
        private readonly IJournalDataService journalDataService;

        public GetAllJournalEntries(IJournalDataService journalDataService)
        {
            this.journalDataService = journalDataService;
        }

        public async Task<CommandResult> Execute()
        {
            var entries = await journalDataService.GetAllJournalEntries();
            return new CommandResult {ResultStatus = CommandResultType.Success, Result = entries};
        }

        public Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }

        public Task<bool> Authorize()
        {
            return Task.FromResult(true);
        }
    }
}