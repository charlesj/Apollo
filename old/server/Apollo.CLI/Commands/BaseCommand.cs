using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Utilities;

namespace Apollo.CLI.Commands
{
    public interface ICommand
    {
        Task Execute();
    }

    public abstract class BaseCommand : ICommand
    {
        protected CommandLineOptions Options { get; }
        protected ConsoleNotShitty Console { get; }
        protected ServerCommandInvoker Invoker { get; }

        protected BaseCommand(CommandLineOptions options)
        {
            Options = options;
            Console = new ConsoleNotShitty();
            Invoker = new ServerCommandInvoker();
        }

        public abstract Task Execute();

        public Task<ServerCommandResult> Execute(string command, object parameters)
        {
            return Invoker.Execute(command, parameters, Options);
        }
    }
}
