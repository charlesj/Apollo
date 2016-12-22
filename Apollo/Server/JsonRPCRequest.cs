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

    public class HttpResponse
    {
        public string Body { get; set; }
        public int HttpCode { get; set; }

        public static HttpResponse BadRequest(string message = null)
        {
            return new HttpResponse {Body = message, HttpCode = 400};
        }

        public static HttpResponse NotFound(string message)
        {
            return new HttpResponse {Body = message, HttpCode = 404};
        }
    }
}