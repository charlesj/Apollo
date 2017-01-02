namespace Apollo.CommandSystem
{
    public class CommandResult
    {
        public CommandResultType ResultStatus { get; set; }
        public object Result { get; set; }
        public int Elapsed { get; set; }
    }
}