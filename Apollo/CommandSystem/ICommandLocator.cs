namespace Apollo.CommandSystem
{
    public interface ICommandLocator
    {
        ICommand Locate(string commandName);
    }
}