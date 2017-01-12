using System;
using System.Threading.Tasks;
using Apollo.Server;
using Moq;
using Xunit;

namespace Apollo.Tests.Server
{
    public class HttpServerTests : BaseUnitTest<HttpServer>
    {
        [Fact]
        public void AlwaysAddsAccessControlAllowOriginHeader()
        {
            var context = Mocker.GetMock<HttpServer.ITestableHttpListenerContext>();

            ClassUnderTest.ProcessRequest(context.Object);

            context.Verify(c => c.AddHeader("Access-Control-Allow-Origin", "*"), Times.Once());
        }

        [Fact]
        public void AlwaysAddsAllowHeadersHeader()
        {
            var context = Mocker.GetMock<HttpServer.ITestableHttpListenerContext>();

            ClassUnderTest.ProcessRequest(context.Object);

            context.Verify(c =>
                c.AddHeader("Access-Control-Allow-Headers",
                    "X-Requested-With, X-HTTP-Method-Override, Content-Type, Accept"),
                Times.Once());
        }

        [Fact]
        public void AlwaysAddsAllowHeadersMethods()
        {
            var context = Mocker.GetMock<HttpServer.ITestableHttpListenerContext>();

            ClassUnderTest.ProcessRequest(context.Object);

            context.Verify(c => c.AddHeader("Access-Control-Allow-Methods", "POST"), Times.Once());
        }

        [Fact]
        public void ImmediatelyClosesResponseOnHEADorOPTIONS()
        {
            var context = Mocker.GetMock<HttpServer.ITestableHttpListenerContext>();
            context.SetupGet(s => s.HttpMethod).Returns("HEAD");
            ClassUnderTest.ProcessRequest(context.Object);

            context.Verify(c => c.CloseResponse(), Times.Once());
            context.Verify(c => c.GetRequestBody(), Times.Never());
            context.Verify(c => c.WriteResponseBody(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void PassesBodyToJsonRPCProcessorAndSetsResponse()
        {
            var context = Mocker.GetMock<HttpServer.ITestableHttpListenerContext>();
            var body = "body";
            context.SetupGet(s => s.HttpMethod).Returns("POST");
            context.Setup(c => c.GetRequestBody()).Returns(body);

            var response = new HttpResponse();
            response.HttpCode = 200;
            context.SetupSet(c => c.StatusCode = response.HttpCode);

            var processor = Mocker.GetMock<IJsonRpcProcessor>();
            processor.Setup(s => s.Process(body)).Returns(Task.FromResult(response));

            ClassUnderTest.ProcessRequest(context.Object);

            context.VerifySet(c => c.StatusCode = response.HttpCode, Times.Once());
            context.Verify(c => c.WriteResponseBody(response.Body), Times.Once());
            context.Verify(c => c.CloseResponse(), Times.Once());
        }

        [Fact]
        public void Returns503_WhenAddingHeaderThrows()
        {
            var thrownException = new Exception("Yep I Am Exception");
            var context = Mocker.GetMock<HttpServer.ITestableHttpListenerContext>();
            context.Setup(c => c.AddHeader(It.IsAny<string>(), It.IsAny<string>()))
                   .Throws(thrownException);

            ClassUnderTest.ProcessRequest(context.Object);

            var expectedMessage = $"SERVER ERROR: {thrownException.Message}";
            context.VerifySet(c => c.StatusCode = 503, Times.Once());

            context.Verify(c => c.WriteResponseBody(expectedMessage), Times.Once());
            context.Verify(c => c.CloseResponse(), Times.Once());
        }

        public class TestableHttpListenerContextTests
        {
            [Fact]
            public void CanHandlePassedNulls()
            {
                var context = new HttpServer.TestableHttpListenerContext(null);
                Assert.NotNull(context);
            }

            [Fact]
            public void ThrowsProperNullRef_WhenAccessFirstTime()
            {
                var context = new HttpServer.TestableHttpListenerContext(null);
                var exception = Assert.Throws<ArgumentNullException>(() => context.GetRequestBody());
                Assert.Equal("passedContext", exception.ParamName);
                Assert.True(exception.Message.StartsWith("Null HttpListenerContext Encountered at Lazy Evaluation"));
            }
        }
    }
}