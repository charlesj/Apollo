namespace Apollo.Commands
{
    public interface ICommandProcessor
    {
        CommandResult Process(ICommand command);
    }
}