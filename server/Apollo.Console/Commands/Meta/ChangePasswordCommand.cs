namespace Apollo.Console.Commands.Meta
{
    public class ChangePasswordCommandOptions : CommandOptionsBase<ChangePasswordCommand>
    {

    }

    public class ChangePasswordCommand : BaseCommand<ChangePasswordCommandOptions>
    {
        public ChangePasswordCommand(ChangePasswordCommandOptions options) : base(options)
        {
        }

        public override void Execute()
        {
            Console.Write("Enter current password > ", false);
            var currentPassword = Console.ReadLineSupressOutput();
            Console.Write("Enter new password > ", false);
            var newPassword = Console.ReadLineSupressOutput();
            Console.Write("Enter new password (verify) > ", false);
            var newPasswordVerification = Console.ReadLineSupressOutput();

            var result = this.Execute("ChangePassword",
                new {currentPassword, newPassword, newPasswordVerification, token = Options.LoginToken});

            if(result.ResultStatus == 0)
                Console.Green("Successfully changed password");
        }
    }
}