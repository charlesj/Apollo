using System;
using System.Threading.Tasks;
using Apollo.CLI.Commands.Local;
using Apollo.CLI.Commands.Meta;
using Microsoft.Extensions.CommandLineUtils;

namespace Apollo.CLI.Commands
{
    public class RootCommand : ICommand
    {
        private readonly CommandLineApplication app;

        public RootCommand(CommandLineApplication app)
        {
            this.app = app;
        }

        public Task Execute()
        {
            app.ShowHelp();
            return Task.FromResult(0);
        }

        public static void Configure(CommandLineApplication app, CommandLineOptions options)
        {
            app.Command("test", c => TestCommand.Configure(c, options));
            app.Command("serverInfo", c => ApplicationInfoCommand.Configure(c, options));
            app.Command("displayConfig", c => DisplayConfigurationCommand.Configure(c, options));
            app.Command("changeEndpoint", c => ChangeEndpointCommand.Configure(c, options));

            app.OnExecute(() =>
            {
                options.Command = new RootCommand(app);
                return 0;
            });
        }
    }
}
