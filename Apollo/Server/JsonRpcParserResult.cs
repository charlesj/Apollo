namespace Apollo.Server
{
    public class JsonRpcParserResult
    {
        public bool Success { get; set; }
        public JsonRpcRequest Request { get; set; }
    }
}