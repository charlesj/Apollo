namespace Apollo.Console.Commands
{
    public abstract class BaseCommand<TCommandOptions> : ICommand where TCommandOptions : SharedCommandOptions
    {
        public BaseCommand(TCommandOptions options)
        {
            Options = options;
            CommandInvoker = new ServerCommandInvoker();
        }

        protected ServerCommandInvoker CommandInvoker { get; private set; }
        public TCommandOptions Options { get; }

        public abstract void Execute();

        protected void Execute(string commandName, object parameters)
        {
            CommandInvoker.Execute(commandName, parameters, Options);
        }
    }
}