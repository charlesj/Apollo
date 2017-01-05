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

            hydrator.Hydrate(ref command, parameters);

            var result = await ValidateCommand(command);

            if(result == null)
                result = await AuthorizeCommand(command);

            if(result == null)
                result = await ExecuteCommand(command);

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
                    Result = new {command, exception},
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
                    result.ErrorMessage = "Unauthorized Command";
                    result.ResultStatus = CommandResultType.Unauthorized;
                    result.Result = command;
                    return result;
                }

                return null;
            }
            catch (Exception exception)
            {
                result.ErrorMessage = exception.Message;
                result.ResultStatus = CommandResultType.Unauthorized;
                result.Result = new {command, exception};
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
                result.Result = new {command, exception};
            }

            return result;
        }
    }
}