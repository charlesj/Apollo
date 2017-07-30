using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Apollo.CommandSystem
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly ICommandHydrator hydrator;

        public CommandProcessor(ICommandHydrator hydrator)
        {
            this.hydrator = hydrator;
        }

        public async Task<CommandResult> Process(ICommand command, object parameters)
        {
            var stopWatch = Stopwatch.StartNew();

            Logger.Trace("Hydrating command");
            hydrator.Hydrate(ref command, parameters);
            Logger.Trace("Hydrated command", command);
            Logger.Trace("Validating command");
            var result = await ValidateCommand(command);
            Logger.Trace("Validated command", result);

            if (result == null)
            {
                Logger.Trace("Authorizing command");
                result = await AuthorizeCommand(command);
                Logger.Trace("Authorized command", result);
            }
            Logger.Trace("Result", result);

            if (result == null)
            {
                Logger.Trace("Executing Command");
                result = await ExecuteCommand(command);
                Logger.Trace("Executed Command", result);
            }

            result.Elapsed = stopWatch.ElapsedMilliseconds;
            return result;
        }

        private async Task<CommandResult> ExecuteCommand(ICommand command)
        {
            try
            {
                var result = await command.Execute();
                return result;
            }
            catch (Exception exception)
            {
                return new CommandResult
                {
                    ErrorMessage = exception.Message,
                    Result = command,
                    ResultStatus = CommandResultType.Error
                };
            }
        }

        private async Task<CommandResult> AuthorizeCommand(ICommand command)
        {
            var result = new CommandResult();
            try
            {
                if (!await command.Authorize())
                {
                    Logger.Trace("Unauthorized Command");
                    result.ErrorMessage = "Unauthorized Command";
                    result.ResultStatus = CommandResultType.Unauthorized;
                    result.Result = command;
                    return result;
                }
                Logger.Trace("Command Authorized");
                return null;
            }
            catch (Exception exception)
            {
                Logger.Error($"Command Authorization Exception {exception.Message}");
                result.ErrorMessage = exception.Message;
                result.ResultStatus = CommandResultType.Unauthorized;
                result.Result = new {};
            }

            return result;
        }

        private async Task<CommandResult> ValidateCommand(ICommand command)
        {
            var result = new CommandResult();
            try
            {
                if (!await command.IsValid())
                {
                    result.ErrorMessage = "Invalid Command";
                    result.ResultStatus = CommandResultType.Invalid;
                    result.Result = command;
                    return result;
                }

                return null;
            }
            catch (Exception exception)
            {
                result.ErrorMessage = exception.Message;
                result.ResultStatus = CommandResultType.Invalid;
                result.Result = command;
            }

            return result;
        }
    }
}
