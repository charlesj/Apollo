using System;

namespace Apollo.Console
{
    public abstract class CommandOptionsBase<TCommand> : SharedCommandOptions, ICommandOptions where TCommand : ICommand
    {
        public Type GetCommandType()
        {
            return typeof(TCommand);
        }
    }
}