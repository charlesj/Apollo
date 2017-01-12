using System;

namespace Apollo.Console
{
    internal interface ICommandOptions
    {
        bool FullResults { get; }
        Type GetCommandType();
    }
}