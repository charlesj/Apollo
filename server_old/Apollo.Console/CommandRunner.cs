using CommandLine;
using CommandLine.Text;
using SimpleInjector;

namespace Apollo.Console
{
    public static class CommandRunner
    {
        public static void Run<TOptions>(string[] args) where TOptions : new()
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine(HelpText.AutoBuild(new TOptions(), null));
                return;
            }

            ICommandOptions options = null;
            if (Parser.Default.ParseArguments(args, new TOptions(), (v, so) =>
            {
                options = (ICommandOptions)so;
            }))
            {
                var container = new Container();
                container.Register(options.GetType(), () => options, Lifestyle.Singleton);

                var command = (ICommand)container.GetInstance(options.GetCommandType());
                command.Execute();
            }
        }
    }
}