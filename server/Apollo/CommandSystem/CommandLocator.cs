using System.Diagnostics;
using System.Linq;
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
            TraceLogger.Trace($"Looking for command {commandName}");
            var type = typeof(ICommand);
            var commandType = type.Assembly.GetTypes()
                .Where(p => type.IsAssignableFrom(p))
                .SingleOrDefault(t => commandName == t.Name.ToLowerInvariant());
            
            TraceLogger.Trace("finished searching for command", commandType);
            if (commandType == null)
                return null;
            
            var commandInstance = (ICommand)serviceLocator.Get(commandType);
            TraceLogger.Trace("Built instance of command");
            return commandInstance;
        }
    }
}