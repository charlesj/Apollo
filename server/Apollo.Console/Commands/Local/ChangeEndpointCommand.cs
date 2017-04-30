using CommandLine;

namespace Apollo.Console.Commands.Local
{
    public class ChangeEndpointCommandOptions : CommandOptionsBase<ChangeEndpointCommand>
    {
        [Option('e', "Endpoint", HelpText = "The New Endpoint", Required = true)]
        public string NewEndpoint { get; set; }
    }

    public class ChangeEndpointCommand : ICommand
    {
        private readonly ChangeEndpointCommandOptions options;

        public ChangeEndpointCommand(ChangeEndpointCommandOptions options)
        {
            this.options = options;
        }

        public void Execute()
        {
            var reader = new ConfigurationReader();
            var config = reader.GetConfiguration();
            config.Endpoint = this.options.NewEndpoint;
            reader.UpdateConfiguration(config);
            System.Console.WriteLine($"New Apollo Enpdoint: {config.Endpoint}");
        }
    }
}