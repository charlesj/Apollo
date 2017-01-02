using System;

namespace Apollo.Commands
{
    public interface ICommandLocator
    {
        ICommand Locate(string commandName);
    }
}