using Apollo.Console.Commands;
using Apollo.Console.Commands.Journal;
using Apollo.Console.Commands.Meta;
using CommandLine;
using CommandLine.Text;

namespace Apollo.Console
{
    public class AvailableCommands
    {
        [VerbOption("applicationInfo", HelpText = "Get information about the server.")]
        public ApplicationInfoCommandOptions ApplicationInfoCommandOptions { get; set; }

        [VerbOption("getAllJournalEntries", HelpText = "Gets all the journal entries")]
        public GetAllJournalEntriesCommandOptions GetAllJournalEntriesCommandOptions { get; set; }

        [VerbOption("test", HelpText = "Test command to make sure things are working")]
        public TestCommandOptions TestCommandOptions { get; set; }

        [HelpVerbOption("help")]
        public string GetUsage(string verb)
        {
            return HelpText.AutoBuild(this, verb);
        }
    }
}