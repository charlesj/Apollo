using System;
using System.Threading.Tasks;

namespace Apollo.Commands
{
    public interface ICommandLocator
    {
        ICommand Locate(string commandName);
    }

    public interface ICommand
    {
        Task Execute();
    }
}