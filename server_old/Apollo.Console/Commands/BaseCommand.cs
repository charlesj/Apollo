using Apollo.Utilities;

namespace Apollo.Console.Commands
{
    public abstract class BaseCommand<TCommandOptions> : ICommand where TCommandOptions : SharedCommandOptions
    {
        public BaseCommand(TCommandOptions options)
        {
            Options = options;
            CommandInvoker = new ServerCommandInvoker();
            Console = new ConsoleNotShitty();
        }

        protected ServerCommandInvoker CommandInvoker { get; private set; }
        protected ConsoleNotShitty Console { get; private set; }
        public TCommandOptions Options { get; }

        public abstract void Execute();

        protected dynamic Execute(string commandName, object parameters)
        {
            return CommandInvoker.Execute(commandName, parameters, Options);
        }
    }
}