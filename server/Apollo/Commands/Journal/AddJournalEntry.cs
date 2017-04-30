using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Data.ResultModels;
using Apollo.Services;

namespace Apollo.Commands.Journal
{
    public class AddJournalEntry : AuthenticatedCommand
    {
        private readonly IJournalDataService journalDataService;

        public string Note { get; set; }

        public AddJournalEntry(IJournalDataService journalDataService, ILoginService loginService) : base(loginService)
        {
            this.journalDataService = journalDataService;
        }

        public override async Task<CommandResult> Execute()
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

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(this.Note));
        }
    }
}