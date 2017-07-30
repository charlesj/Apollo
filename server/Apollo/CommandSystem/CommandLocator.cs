using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Apollo.ServiceLocator;

namespace Apollo.CommandSystem
{
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
            Logger.Trace($"Looking for command {commandName}");
            var type = typeof(ICommand);
            var commandType = type.GetTypeInfo().Assembly.GetTypes()
                .Where(p => type.IsAssignableFrom(p))
                .SingleOrDefault(t => commandName == t.Name.ToLowerInvariant());

            Logger.Trace("finished searching for command", commandType);
            if (commandType == null)
                return null;

            var commandInstance = (ICommand)serviceLocator.Get(commandType);
            Logger.Trace("Built instance of command");
            return commandInstance;
        }

        public IReadOnlyList<Type> GetAllAvailableCommands()
        {
            var type = typeof(ICommand);
            var types = type.GetTypeInfo().Assembly.GetTypes()
                .Where(p => type.IsAssignableFrom(p));

            return types.ToList();
        }
    }
}
