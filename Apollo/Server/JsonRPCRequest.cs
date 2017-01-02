namespace Apollo.Server
{
    public class JsonRPCRequest
    {
        public string Method { get; set; }
        public object Params { get; set; }
        public string Id { get; set; }
    }

    public class JsonRPCResponse
    {
        public string result { get; set; }
        public string id { get; set; }
        public string error { get; set; }
    }
}