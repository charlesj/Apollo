using Apollo.Utilities;
using CommandLine;

namespace Apollo.Console.Commands.Local
{
    public class GetPasswordHashCommandOptions : CommandOptionsBase<GetPasswordHashCommand>
    {
    }

    public class GetPasswordHashCommand : BaseCommand<GetPasswordHashCommandOptions>
    {

        public GetPasswordHashCommand(GetPasswordHashCommandOptions options) : base(options)
        {
        }

        public override void Execute()
        {
            var hasher = new PasswordHasher();
            Console.Write("Enter password > ", false);
            var password = Console.ReadLineSupressOutput();
            Console.Write("Enter password (verify) > ", false);
            var passwordVerify = Console.ReadLineSupressOutput();

            if (password == passwordVerify)
            {
                Console.Write("==================================================\n");
                Console.Green(hasher.GenerateHash(password));
                Console.Write("\n==================================================");
            }
            else
                Console.Red("Passwords did not match");
        }
    }
}