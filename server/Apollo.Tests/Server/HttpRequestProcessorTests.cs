using System;
using System.Threading.Tasks;
using Apollo.Server;
using Moq;
using Xunit;

namespace Apollo.Tests.Server
{
    public class HttpRequestProcessorTests : BaseUnitTest<HttpRequestProcessor>
    {
        [Fact]
        public async void AlwaysAddsAccessControlAllowOriginHeader()
        {
            var context = Mock<ITestableHttpContext>();

            await ClassUnderTest.Process(context.Object);

            context.Verify(c => c.AddHeader("Access-Control-Allow-Origin", "*"), Times.Once());
        }

        [Fact]
        public async void AlwaysAddsAllowHeadersHeader()
        {
            var context = Mock<ITestableHttpContext>();

            await ClassUnderTest.Process(context.Object);
            context.Verify(c =>
                c.AddHeader("Access-Control-Allow-Headers",
                    "X-Requested-With, X-HTTP-Method-Override, Content-Type, Accept"),
                Times.Once());
        }

        [Fact]
        public async void AlwaysAddsAllowHeadersMethods()
        {
            var context = Mock<ITestableHttpContext>();

            await ClassUnderTest.Process(context.Object);

            context.Verify(c => c.AddHeader("Access-Control-Allow-Methods", "POST"), Times.Once());
        }

        [Fact]
        public async void ImmediatelyClosesResponseOnHEADorOPTIONS()
        {
            var context = Mock<ITestableHttpContext>();
            context.SetupGet(s => s.HttpMethod).Returns("HEAD");
            await ClassUnderTest.Process(context.Object);

            context.Verify(c => c.CloseResponse(), Times.Once());
            context.Verify(c => c.GetRequestBody(), Times.Never());
            context.Verify(c => c.WriteResponseBody(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async void PassesBodyToJsonRPCProcessorAndSetsResponse()
        {
            var context = Mock<ITestableHttpContext>();
            var body = "body";
            var clientInfo = new HttpClientInfo();

            context.SetupGet(s => s.HttpMethod).Returns("POST");
            context.Setup(c => c.GetRequestBody()).Returns(body);
            context.Setup(c => c.GetClientInfo()).Returns(clientInfo);

            var response = new HttpResponse();
            response.HttpCode = 200;
            context.SetupSet(c => c.StatusCode = response.HttpCode);

            var processor = Mock<IJsonRpcProcessor>();
            processor.Setup(s => s.Process(body, clientInfo)).Returns(Task.FromResult(response));

            await ClassUnderTest.Process(context.Object);

            context.VerifySet(c => c.StatusCode = response.HttpCode, Times.Once());
            context.Verify(c => c.WriteResponseBody(response.Body), Times.Once());
            context.Verify(c => c.CloseResponse(), Times.Once());
            processor.Verify(p => p.Process(body, clientInfo), Times.Once());
        }

        [Fact]
        public async void Returns503_WhenAddingHeaderThrows()
        {
            var thrownException = new Exception("Yep I Am Exception");
            var context = Mock<ITestableHttpContext>();
            context.Setup(c => c.AddHeader(It.IsAny<string>(), It.IsAny<string>()))
                   .Throws(thrownException);

            await ClassUnderTest.Process(context.Object);

            var expectedMessage = $"SERVER ERROR: {thrownException.Message}";
            context.VerifySet(c => c.StatusCode = 503, Times.Once());

            context.Verify(c => c.WriteResponseBody(expectedMessage), Times.Once());
            context.Verify(c => c.CloseResponse(), Times.Once());
        }

        [Fact]
        public async void TellsWorldWhatPowersIt()
        {
            var context = Mock<ITestableHttpContext>();

            await ClassUnderTest.Process(context.Object);

            context.Verify(c => c.AddHeader("X-Powered-By", $"Apollo v{Apollo.Version}"), Times.Once());
        }
    }
}
