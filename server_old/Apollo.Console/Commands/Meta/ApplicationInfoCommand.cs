namespace Apollo.Console.Commands
{
    public class ApplicationInfoCommandOptions : CommandOptionsBase<ApplicationInfoCommand>
    {
    }

    public class ApplicationInfoCommand : BaseCommand<ApplicationInfoCommandOptions>
    {
        public ApplicationInfoCommand(ApplicationInfoCommandOptions options) : base(options)
        {
        }

        public override void Execute()
        {
            Execute("applicationinfo", new object());
        }
    }
}