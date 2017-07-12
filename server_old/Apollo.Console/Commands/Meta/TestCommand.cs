using CommandLine;

namespace Apollo.Console.Commands.Meta
{
    public class TestCommandOptions : CommandOptionsBase<TestCommand>
    {
        [Option('n', "name", HelpText = "Who to name", Required = false)]
        public string Name { get; set; }
    }

    public class TestCommand : BaseCommand<TestCommandOptions>, ICommand
    {
        public TestCommand(TestCommandOptions options) : base(options)
        {
        }

        public override void Execute()
        {
            Execute("TestCommand", new object());
        }
    }
}