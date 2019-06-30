using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace Apollo.CLI.Commands.Meta
{
    public class ChangePasswordCommand : BaseCommand
    {
        public ChangePasswordCommand(CommandLineOptions options) : base(options)
        {
        }

        public override async Task Execute()
        {
            Console.Write("Enter current password: ", false);
            var currentPassword = Console.ReadLineSupressOutput();
            Console.Write("Enter new password: ", false);
            var newPassword = Console.ReadLineSupressOutput();
            Console.Write("Enter new password (verify): ", false);
            var newPasswordVerification = Console.ReadLineSupressOutput();

            var result = await this.Execute("ChangePassword",
                new {currentPassword, newPassword, newPasswordVerification});

            if(result.ResultStatus == 0)
                Console.Green("Successfully changed password");
        }

        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Change your password";
            command.OnExecute(() =>
            {
                options.Command = new ChangePasswordCommand(options);
                return 0;
            });
        }
    }
}
