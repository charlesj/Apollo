using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Server;
using Moq;
using Xunit;

namespace Apollo.Tests.Server
{
    public class JsonRpcCommandTranslatorTests : BaseUnitTest<JsonRpcCommandTranslator>
    {
        private ICommand command;
        private JsonRpcRequest request;
        private CommandResult goodResult;
        private HttpClientInfo clientInfo = new HttpClientInfo();

        public JsonRpcCommandTranslatorTests()
        {
            command = new SimpleCommand();
            request = new JsonRpcRequest
            {
                Id = "42",
                Method = "yes this is method"
            };

            goodResult = new CommandResult
            {
                Elapsed = 0,
                Result = new object(),
                ResultStatus = CommandResultType.Success
            };
        }

        [Fact]
        public async void CallsCommandProcessor_WithParamters()
        {
            request.Params = new object();
            var processor = Mock<ICommandProcessor>();
            processor.Setup(r => r.Process(command, request.Params))
                .Returns(Task.FromResult(goodResult));
            await ClassUnderTest.ExecuteCommand(command, request, clientInfo);

            processor.Verify(p => p.Process(command, request.Params), Times.Once());
        }

        [Fact]
        public async void ResponseId_MustMatch()
        {
            request.Params = new object();
            var processor = Mock<ICommandProcessor>();
            processor.Setup(r => r.Process(command, request.Params))
                .Returns(Task.FromResult(goodResult));
            var result = await ClassUnderTest.ExecuteCommand(command, request, clientInfo);

            Assert.Equal(request.Id, result.id);
        }

        [Fact]
        public async void Result_IsNonSerializedCommandResult()
        {
            request.Params = new object();
            var processor = Mock<ICommandProcessor>();
            processor.Setup(r => r.Process(command, request.Params))
                .Returns(Task.FromResult(goodResult));

            var result = await ClassUnderTest.ExecuteCommand(command, request, clientInfo);

            Assert.Same(goodResult, result.result);
        }

        [Fact]
        public async void ErrorIsEmpty_WhenSuccessResult()
        {
            request.Params = new object();
            var processor = Mock<ICommandProcessor>();
            processor.Setup(r => r.Process(command, request.Params))
                .Returns(Task.FromResult(goodResult));

            var result = await ClassUnderTest.ExecuteCommand(command, request, clientInfo);

            Assert.Null(result.error);
        }

        [Theory]
        [InlineData(CommandResultType.Error)]
        [InlineData(CommandResultType.Invalid)]
        [InlineData(CommandResultType.Unauthorized)]
        public async void ErrorIsExceptionMessage_WhenErrorResult(CommandResultType type)
        {
            request.Params = new object();
            var expected = "expected";
            var badResult = new CommandResult
            {
                ResultStatus = type,
                ErrorMessage = expected
            };

            var processor = Mock<ICommandProcessor>();
            processor.Setup(r => r.Process(command, request.Params))
                .Returns(Task.FromResult(badResult));

            var result = await ClassUnderTest.ExecuteCommand(command, request, clientInfo);
            Assert.Equal(expected, result.error);
        }

        [Fact]
        public async void HydratesCommandClientInformationFromHttpClientInfo()
        {
            request.Params = new object();
            var processor = Mock<ICommandProcessor>();
            processor.Setup(r => r.Process(command, request.Params))
                .Returns(Task.FromResult(goodResult));

            clientInfo.Agent = "YES I AM AGENT";
            clientInfo.IpAddress = "11.11.11.11";

            await ClassUnderTest.ExecuteCommand(command, request, clientInfo);

            processor.Verify(p =>
                p.Process(
                    It.Is<SimpleCommand>(c => c.ClientIpAddress == clientInfo.IpAddress
                                         && c.ClientUserAgent == clientInfo.Agent),
                request.Params), Times.Once());
        }

        private class SimpleCommand : CommandBase
        {
            public override Task<CommandResult> Execute()
            {
                throw new System.NotImplementedException();
            }

            public override Task<bool> IsValid()
            {
                throw new System.NotImplementedException();
            }

            public override Task<bool> Authorize()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
