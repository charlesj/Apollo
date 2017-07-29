﻿namespace Apollo.CommandSystem
{
    public class CommandResult
    {
        public CommandResult()
        {
            Elapsed = -1;
        }

        public CommandResultType ResultStatus { get; set; }
        public object Result { get; set; }
        public long Elapsed { get; set; }
        public string ErrorMessage { get; set; }

        public static CommandResult SuccessfulResult = new CommandResult() {ResultStatus = CommandResultType.Success};

        public static CommandResult CreateSuccessResult(object result)
        {
            return new CommandResult {ResultStatus = CommandResultType.Success, Result = result};
        }
    }
}