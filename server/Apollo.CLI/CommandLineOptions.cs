using Apollo.CLI.Commands;
using Microsoft.Extensions.CommandLineUtils;

namespace Apollo.CLI
{
    public class CommandLineOptions
    {
        public ICommand Command { get; set; }
        public bool Verbose { get; set; }
        public bool ShowRequest { get; set; }
        public bool FullResults { get; set; }
        public string Endpoint { get; set; }
        public string LoginToken { get; set; }
        public bool SuppressOutput { get; set; }

        public static CommandLineOptions Parse(string[] args)
        {
            var options = new CommandLineOptions();
            var configurationReader = new ConfigurationReader();
            var config = configurationReader.GetConfiguration();
            options.Endpoint = config.Endpoint;
            options.LoginToken = config.Endpoint;

            var app = new CommandLineApplication
            {
                Name = "apollo",
                FullName = "Apollo CommandLine Interface"
            };

            app.HelpOption("-h|--help");

            var verboseSwitch = app.Option("--verbose", "Verbose Output", CommandOptionType.NoValue);
            var requestSwitch =
                app.Option("--showRequest", "Write request object to stdout", CommandOptionType.NoValue);
            var fullResultSwitch = app.Option("--fullResults", "Display full result object and not just command result",
                CommandOptionType.NoValue);

            RootCommand.Configure(app, options);
            var result = app.Execute(args);
            if (result != 0)
            {
                return null;
            }

            options.Verbose = verboseSwitch.HasValue();
            options.ShowRequest = requestSwitch.HasValue();
            options.FullResults = fullResultSwitch.HasValue();
            return options;
        }


    }
}
