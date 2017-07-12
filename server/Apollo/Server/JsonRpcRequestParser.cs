using System;
using Apollo.Utilities;

namespace Apollo.Server
{
    public class JsonRpcRequestParser : IJsonRpcRequestParser
    {
        private readonly IJsonSerializer serializer;

        public JsonRpcRequestParser(IJsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        public JsonRpcParserResult Parse(string body)
        {
            try
            {
                var deserialized = serializer.Deserialize<JsonRpcRequest>(body);
                if(string.IsNullOrEmpty(deserialized.Id)
                   || string.IsNullOrEmpty(deserialized.Method))
                    return new JsonRpcParserResult {Success = false};

                return new JsonRpcParserResult {Success = true, Request = deserialized};
            }
            catch (Exception)
            {
                return new JsonRpcParserResult {Success = false};
            }
        }
    }
}