using System;
using System.Linq;
using Apollo.ServiceLocator;

namespace Apollo.CommandSystem
{
    public interface ICommandLocator
    {
        ICommand Locate(string commandName);
    }

    public class CommandLocator : ICommandLocator
    {
        private readonly IServiceLocator serviceLocator;

        public CommandLocator(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public ICommand Locate(string commandName)
        {
            commandName = commandName.ToLowerInvariant();
            var type = typeof(ICommand);
            var commandType = type.Assembly.GetTypes()
                .Where(p => type.IsAssignableFrom(p))
                .SingleOrDefault(t => commandName == t.Name.ToLowerInvariant());

            if (commandType == null)
                return null;

            return (ICommand)serviceLocator.Get(commandType);
        }
    }
}