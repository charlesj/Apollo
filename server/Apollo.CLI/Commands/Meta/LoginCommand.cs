using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace Apollo.CLI.Commands.Meta
{
    public class LoginCommand : BaseCommand
    {
        public LoginCommand(CommandLineOptions options) : base(options)
        {
        }

        public override async Task Execute()
        {
            var configurationReader = new ConfigurationReader();
            Console.Write("Enter password: ", false);
            var password = Console.ReadLineSupressOutput();
            var result = await Execute("Login", new { password });
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

        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Login to server and retrieve login token";
            command.OnExecute(() =>
            {
                options.Command = new LoginCommand(options);
                return 0;
            });
        }
    }
}
