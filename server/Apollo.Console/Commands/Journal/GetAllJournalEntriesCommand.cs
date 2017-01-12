namespace Apollo.Console.Commands.Journal
{
    public class GetAllJournalEntriesCommandOptions : CommandOptionsBase<GetAllJournalEntriesCommand>
    {
    }

    public class GetAllJournalEntriesCommand : BaseCommand<GetAllJournalEntriesCommandOptions>
    {
        public GetAllJournalEntriesCommand(GetAllJournalEntriesCommandOptions options) : base(options)
        {
        }

        public override void Execute()
        {
            Execute("GetAllJournalEntries", new object());
        }
    }
}