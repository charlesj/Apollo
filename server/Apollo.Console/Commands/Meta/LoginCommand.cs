using CommandLine;

namespace Apollo.Console.Commands.Meta
{
    public class LoginCommandOptions : CommandOptionsBase<LoginCommand>
    {
        [Option('p', "password", HelpText = "The Password to use to login", Required = true)]
        public string Password { get; set; }
    }

    public class LoginCommand : BaseCommand<LoginCommandOptions>
    {
        private ConfigurationReader configurationReader;

        public LoginCommand(LoginCommandOptions options) : base(options)
        {
            this.configurationReader = new ConfigurationReader();
            
        }

        public override void Execute()
        {
            var result = Execute("Login", new {password = this.Options.Password});
            if (result == null && result.token != null)
            {
                System.Console.WriteLine("Could not login");
                return;
            }

            var configuration = configurationReader.GetConfiguration();
            System.Console.WriteLine($"Setting new token {result.token}");
            configuration.LoginToken = result.token;

            configurationReader.UpdateConfiguration(configuration);
        }


    }
}