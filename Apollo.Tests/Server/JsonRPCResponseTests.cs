using Apollo.Server;
using Xunit;

namespace Apollo.Tests.Server
{
    public class JsonRPCResponseTests
    {
        [Fact]
        public void BadRequest()
        {
            var response = HttpResponse.BadRequest();
            Assert.Null(response.Body);
            Assert.Equal(400, response.HttpCode);
        }

        [Fact]
        public void BadRequest_WithMessage()
        {
            var message = "Hello";
            var response = HttpResponse.BadRequest(message);
            Assert.Equal(message, response.Body);
            Assert.Equal(400, response.HttpCode);
        }

        [Fact]
        public void NotFound_WithMessage()
        {
            var message = "Hello";
            var response = HttpResponse.NotFound(message);
            Assert.Equal(message, response.Body);
            Assert.Equal(404, response.HttpCode);
        }
    }
}