using Apollo.Utilities;

namespace Apollo.Server
{
    public interface IJsonRpcHttpConverter
    {
        HttpResponse Convert(JsonRpcResponse response);
    }

    public class JsonRpcHttpConverter : IJsonRpcHttpConverter
    {
        private readonly IJsonSerializer serializer;

        public JsonRpcHttpConverter(IJsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        public HttpResponse Convert(JsonRpcResponse response)
        {
            if (response == null)
                return HttpResponse.ServerError("Null Command Result");

            var body = serializer.Serialize(response, true);

            if (!string.IsNullOrWhiteSpace(response.error))
            {
                return HttpResponse.ServerError(body);
            }

            return HttpResponse.Success(body);
        }
    }
}