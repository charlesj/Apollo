namespace Apollo.CommandSystem
{
    public interface ICommandProcessor
    {
        CommandResult Process(ICommand command);
    }
}