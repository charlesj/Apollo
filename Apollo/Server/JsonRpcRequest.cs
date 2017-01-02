namespace Apollo.Server
{
    public class JsonRpcRequest
    {
        public string Method { get; set; }
        public object Params { get; set; }
        public string Id { get; set; }
    }

    public class JsonRpcResponse
    {
        public string result { get; set; }
        public string id { get; set; }
        public string error { get; set; }
    }
}