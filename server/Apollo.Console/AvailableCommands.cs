using Apollo.Console.Commands;
using Apollo.Console.Commands.Journal;
using Apollo.Console.Commands.Meta;
using CommandLine;
using CommandLine.Text;

namespace Apollo.Console
{
    public class AvailableCommands
    {
        // meta
        [VerbOption("applicationInfo", HelpText = "Get information about the server.")]
        public ApplicationInfoCommandOptions ApplicationInfoCommandOptions { get; set; }

        [VerbOption("test", HelpText = "Test command to make sure things are working")]
        public TestCommandOptions TestCommandOptions { get; set; }

        // journal
        [VerbOption("getAllJournalEntries", HelpText = "Gets all the journal entries")]
        public GetAllJournalEntriesCommandOptions GetAllJournalEntriesCommandOptions { get; set; }

        [VerbOption("addJournalEntry", HelpText = "Adds a new journal entury")]
        public AddJournalEntryCommandOptions AddJournalEntryCommandOptions { get; set; }


        [HelpVerbOption("help")]
        public string GetUsage(string verb)
        {
            return HelpText.AutoBuild(this, verb);
        }
    }
}