using System;
using System.Collections.Generic;

namespace Apollo.CommandSystem
{
    public interface ICommandLocator
    {
        ICommand Locate(string commandName);
        IReadOnlyList<Type> GetAllAvailableCommands();
    }
}
