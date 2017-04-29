using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Data.ResultModels;

namespace Apollo.Commands.Journal
{
    public class AddJournalEntry : ICommand
    {
        private readonly IJournalDataService journalDataService;

        public string Note { get; set; }

        public AddJournalEntry(IJournalDataService journalDataService)
        {
            this.journalDataService = journalDataService;
        }

        public async Task<CommandResult> Execute()
        {
            await this.journalDataService.CreateJournalEntry(new JournalEntry
            {
                note = this.Note
            });

            return new CommandResult
            {
                ResultStatus = CommandResultType.Success
            };
        }

        public Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(this.Note));
        }

        public Task<bool> Authorize()
        {
            return Task.FromResult(true);
        }
    }
}