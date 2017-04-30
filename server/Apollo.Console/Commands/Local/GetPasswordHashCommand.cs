using Apollo.Utilities;
using CommandLine;

namespace Apollo.Console.Commands.Local
{
    public class GetPasswordHashCommandOptions : CommandOptionsBase<GetPasswordHashCommand>
    {
        [Option('p', "password", HelpText = "the password to hash", Required = true)]
        public string Password { get; set; }
    }

    public class GetPasswordHashCommand : ICommand
    {
        private readonly GetPasswordHashCommandOptions options;

        public GetPasswordHashCommand(GetPasswordHashCommandOptions options)
        {
            this.options = options;
        }

        public void Execute()
        {
            var hasher = new PasswordHasher();
            System.Console.WriteLine(hasher.GenerateHash(options.Password));
        }
    }
}