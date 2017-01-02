using System;

namespace Apollo.Server
{
    public interface IJsonRpcRequestParser
    {
        JsonRpcParserResult Parse(string body);
    }
}