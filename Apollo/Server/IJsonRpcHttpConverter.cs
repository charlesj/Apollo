namespace Apollo.Server
{
    public interface IJsonRpcHttpConverter
    {
        HttpResponse Convert(JsonRpcResponse response);
    }

    public class JsonRpcHttpConverter : IJsonRpcHttpConverter
    {
        public HttpResponse Convert(JsonRpcResponse response)
        {
            throw new System.NotImplementedException();
        }
    }
}