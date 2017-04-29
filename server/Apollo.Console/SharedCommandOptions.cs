using CommandLine;

namespace Apollo.Console
{
    public class SharedCommandOptions
    {
        public SharedCommandOptions()
        {
            this.Endpoint = "http://192.168.142.10/api";
        }

        [Option("fullResults", Required = false, HelpText = "Whether to display the full results, or just the result object")]
        public bool FullResults { get; set; }

        [Option("showRequest", Required = false, HelpText = "Whether to write the request object to the console")]
        public bool ShowRequest { get; set; }

        [Option("endpoint", Required = false, HelpText = "Override the default endpoint")]
        public string Endpoint { get; set; }
    }
}
