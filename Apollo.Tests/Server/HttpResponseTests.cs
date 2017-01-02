using Apollo.Server;
using Xunit;

namespace Apollo.Tests.Server
{
    public class HttpResponseTests
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

        [Fact]
        public void ServerError_WithMessage()
        {
            var message = "Hello";
            var response = HttpResponse.ServerError(message);
            Assert.Equal(message, response.Body);
            Assert.Equal(503, response.HttpCode);
        }

        [Fact]
        public void Success_WithMessage()
        {
            var message = "Hello";
            var response = HttpResponse.Success(message);
            Assert.Equal(message, response.Body);
            Assert.Equal(200, response.HttpCode);
        }
    }
}