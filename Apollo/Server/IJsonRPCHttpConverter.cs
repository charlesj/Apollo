namespace Apollo.Server
{
    public interface IJsonRPCHttpConverter
    {
        HttpResponse Convert(JsonRPCResponse response);
    }
}