using CommandLine;

namespace Apollo.Console.Commands.Meta
{
    public class LoginCommandOptions : CommandOptionsBase<LoginCommand>
    {
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
            Console.Write("Enter password > ", false);
            var password = Console.ReadLineSupressOutput();
            var result = Execute("Login", new { password });
            var loginToken = result?.Result?.token;
            if (result == null && loginToken != null)
            {
                Console.Red("Could not login");
                return;
            }

            var configuration = configurationReader.GetConfiguration();
            Console.Green($"Setting new token {loginToken}");
            configuration.LoginToken = loginToken;

            configurationReader.UpdateConfiguration(configuration);
        }


    }
}