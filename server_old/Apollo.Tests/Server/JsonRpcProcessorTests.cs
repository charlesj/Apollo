using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Server;
using Xunit;

namespace Apollo.Tests.Server
{
    public class JsonRpcProcessorTests : BaseUnitTest<JsonRpcProcessor>
    {
        private HttpClientInfo clientInfo = new HttpClientInfo();

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        [InlineData(null)]
        public async void EmptyBody_Returns400Result(string requestBody)
        {
            var result = await ClassUnderTest.Process(requestBody, clientInfo);
            Assert.Equal(400, result.HttpCode);
            Assert.Null(result.Body);
        }

        [Fact]
        public async void UnparsableRequest_Returns400Result()
        {
            var request = "notempty";
            var parser = this.Mocker.GetMock<IJsonRpcRequestParser>();
            var parserResult = new JsonRpcParserResult();

            parser.Setup(p => p.Parse(request)).Returns(parserResult);

            var result = await ClassUnderTest.Process(request, clientInfo);
            Assert.Equal(400, result.HttpCode);
            Assert.Equal("Could not parse RPC Request", result.Body);
        }

        [Fact]
        public async void UnmatchedMethod_Returns404()
        {
            var request = "notempty";
            var parser = this.Mocker.GetMock<IJsonRpcRequestParser>();
            var parserResult = new JsonRpcParserResult{Success = true, Request = new JsonRpcRequest{ Method = "method"}};
            parser.Setup(p => p.Parse(request)).Returns(parserResult);

            var commandLocator = this.Mocker.GetMock<ICommandLocator>();
            commandLocator.Setup(l => l.Locate(parserResult.Request.Method))
                .Returns((ICommand)null);

            var result = await ClassUnderTest.Process(request, clientInfo);
            Assert.Equal(404, result.HttpCode);
            Assert.Equal("Could not locate requested method", result.Body);
        }

        [Fact]
        public async void PassesExecutionToTranslator_WhenCommandIsLocated()
        {
            var request = "notempty";
            var parser = this.Mocker.GetMock<IJsonRpcRequestParser>();
            var parserResult = new JsonRpcParserResult{Success = true, Request = new JsonRpcRequest{ Method = "method"}};
            parser.Setup(p => p.Parse(request)).Returns(parserResult);

            var mockedCommand = this.Mocker.GetMock<ICommand>();
            var commandLocator = this.Mocker.GetMock<ICommandLocator>();
            commandLocator.Setup(l => l.Locate(parserResult.Request.Method))
                .Returns(mockedCommand.Object);
            var translator = this.Mocker.GetMock<IJsonRpcCommandTranslator>();
            translator.Setup(t => t.ExecuteCommand(mockedCommand.Object, parserResult.Request, clientInfo))
                .Returns(Task.FromResult(new JsonRpcResponse()));
            await ClassUnderTest.Process(request, clientInfo);

            translator.Verify(t => t.ExecuteCommand(mockedCommand.Object, parserResult.Request, clientInfo));
        }

        [Fact]
        public async void ConvertsJsonRPCtoHttpResponse_usingconvert()
        {
            var request = "notempty";
            var parser = this.Mocker.GetMock<IJsonRpcRequestParser>();
            var parserResult = new JsonRpcParserResult{Success = true, Request = new JsonRpcRequest{ Method = "method"}};
            parser.Setup(p => p.Parse(request)).Returns(parserResult);

            var mockedCommand = this.Mocker.GetMock<ICommand>();
            var commandLocator = this.Mocker.GetMock<ICommandLocator>();
            commandLocator.Setup(l => l.Locate(parserResult.Request.Method))
                .Returns(mockedCommand.Object);

            var translator = this.Mocker.GetMock<IJsonRpcCommandTranslator>();
            var jsonResponse = new JsonRpcResponse();
            translator.Setup(t => t.ExecuteCommand(mockedCommand.Object, parserResult.Request, clientInfo))
                .Returns(Task.FromResult(jsonResponse));

            var converter = this.Mocker.GetMock<IJsonRpcHttpConverter>();
            var httpResponse = new HttpResponse();
            converter.Setup(c => c.Convert(jsonResponse))
                .Returns(httpResponse);

            var result = await ClassUnderTest.Process(request, clientInfo);
            Assert.Same(httpResponse, result);
        }
    }
}
