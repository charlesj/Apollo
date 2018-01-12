using System.Collections.Generic;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Log
{
    public class AddJournalEntry : AuthenticatedCommand
    {
        private readonly IJournalDataService journalDataService;

        public string Note { get; set; }
        public List<string> Tags { get; set; }

        public AddJournalEntry(IJournalDataService journalDataService, ILoginService loginService) : base(loginService)
        {
            this.journalDataService = journalDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var note = await this.journalDataService.CreateJournalEntry(new JournalEntry
            {
                note = this.Note,
                tags = this.Tags?.ToArray()
            });

            return CommandResult.CreateSuccessResult(note);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(this.Note));
        }

        public override object ExamplePayload()
        {
            return new { Note, Tags=new List<string>{"tag1", "tag2"}};
        }
    }
}
