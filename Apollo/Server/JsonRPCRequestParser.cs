namespace Apollo.Server
{
    public interface IJsonRPCRequestParser
    {
        JsonRPCParserResult Parse(string body);
    }

    public class JsonRPCParserResult
    {
        public bool Success { get; set; }
        public JsonRPCRequest Request { get; set; }
    }

    public class JsonRPCRequestParser : IJsonRPCRequestParser
    {
        public JsonRPCParserResult Parse(string body)
        {
            throw new System.NotImplementedException();
        }
    }
}