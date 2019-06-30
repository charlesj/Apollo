using Apollo.CommandSystem;

namespace Apollo.CLI
{
    public class ServerResponse
    {
        public string id { get; set; }
        public string error { get; set; }
        public ServerCommandResult result { get; set; }
    }

    public class ServerCommandResult
    {
        public CommandResultType ResultStatus { get; set; }
        public dynamic Result { get; set; }
        public long Elapsed { get; set; }
        public string ErrorMessage { get; set; }
    }
}
