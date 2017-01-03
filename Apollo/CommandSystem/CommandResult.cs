namespace Apollo.CommandSystem
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
    }
}