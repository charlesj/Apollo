namespace Apollo.Server
{
    public interface ITestableHttpContext
    {
        string HttpMethod { get; }
        int StatusCode { get; set; }
        string GetRequestBody();
        void WriteResponseBody(string body);
        void AddHeader(string headerName, string value);
        void CloseResponse();
        HttpClientInfo GetClientInfo();
    }
}