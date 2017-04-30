using CommandLine;

namespace Apollo.Console.Commands.Journal
{
    public class AddJournalEntryCommandOptions : CommandOptionsBase<AddJournalEntryCommand>
    {
        [Option('n', "note", HelpText = "content of the note", Required = true)]
        public string Note { get; set; }
    }

    public class AddJournalEntryCommand : BaseCommand<AddJournalEntryCommandOptions>
    {
        public AddJournalEntryCommand(AddJournalEntryCommandOptions options) : base(options)
        {
        }

        public override void Execute()
        {
            Execute("AddJournalEntry", new
            {
                token = Options.LoginToken,
                note = Options.Note
            });
        }
    }
}