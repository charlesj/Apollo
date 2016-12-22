using Apollo.Commands;
using Apollo.Server;
using Xunit;

namespace Apollo.Tests.Server
{
    public class JsonRPCProcessorTests : BaseUnitTest<JsonRPCProcessor>
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        [InlineData(null)]
        public async void EmptyBody_Returns400Result(string requestBody)
        {
            var result = await ClassUnderTest.Process(requestBody);
            Assert.Equal(400, result.HttpCode);
            Assert.Null(result.Body);
        }

        [Fact]
        public async void UnparsableRequest_Returns400Result()
        {
            var request = "notempty";
            var parser = this.Mocker.GetMock<IJsonRPCRequestParser>();
            var parserResult = new JsonRPCParserResult();

            parser.Setup(p => p.Parse(request)).Returns(parserResult);

            var result = await ClassUnderTest.Process(request);
            Assert.Equal(400, result.HttpCode);
            Assert.Equal("Could not parse RPC Request", result.Body);
        }

        [Fact]
        public async void UnmatchedMethod_Returns404()
        {
            var request = "notempty";
            var parser = this.Mocker.GetMock<IJsonRPCRequestParser>();
            var parserResult = new JsonRPCParserResult{Success = true, Request = new JsonRPCRequest{ Method = "method"}};
            parser.Setup(p => p.Parse(request)).Returns(parserResult);

            var commandLocator = this.Mocker.GetMock<ICommandLocator>();
            commandLocator.Setup(l => l.Locate(parserResult.Request.Method))
                .Returns((ICommand)null);

            var result = await ClassUnderTest.Process(request);
            Assert.Equal(404, result.HttpCode);
            Assert.Equal("Could not locate requested method", result.Body);
        }
    }
}