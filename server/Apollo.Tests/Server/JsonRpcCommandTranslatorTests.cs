using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Server;
using Apollo.Utilities;
using Moq;
using Xunit;

namespace Apollo.Tests.Server
{
    public class JsonRpcCommandTranslatorTests : BaseUnitTest<JsonRpcCommandTranslator>
    {
        private ICommand command;
        private JsonRpcRequest request;
        private CommandResult goodResult;

        public JsonRpcCommandTranslatorTests()
        {
            command = Mocker.GetMock<ICommand>().Object;
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
            var processor = Mocker.GetMock<ICommandProcessor>();
            processor.Setup(r => r.Process(command, request.Params))
                .Returns(Task.FromResult(goodResult));
            await ClassUnderTest.ExecuteCommand(command, request);

            processor.Verify(p => p.Process(command, request.Params), Times.Once());
        }

        [Fact]
        public async void ResponseId_MustMatch()
        {
            request.Params = new object();
            var processor = Mocker.GetMock<ICommandProcessor>();
            processor.Setup(r => r.Process(command, request.Params))
                .Returns(Task.FromResult(goodResult));
            var result = await ClassUnderTest.ExecuteCommand(command, request);

            Assert.Equal(request.Id, result.id);
        }

        [Fact]
        public async void Result_IsNonSerializedCommandResult()
        {
            request.Params = new object();
            var processor = Mocker.GetMock<ICommandProcessor>();
            processor.Setup(r => r.Process(command, request.Params))
                .Returns(Task.FromResult(goodResult));

            var result = await ClassUnderTest.ExecuteCommand(command, request);

            Assert.Same(goodResult, result.result);
        }

        [Fact]
        public async void ErrorIsEmpty_WhenSuccessResult()
        {
            request.Params = new object();
            var processor = Mocker.GetMock<ICommandProcessor>();
            processor.Setup(r => r.Process(command, request.Params))
                .Returns(Task.FromResult(goodResult));

            var result = await ClassUnderTest.ExecuteCommand(command, request);

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

            var processor = Mocker.GetMock<ICommandProcessor>();
            processor.Setup(r => r.Process(command, request.Params))
                .Returns(Task.FromResult(badResult));

            var result = await ClassUnderTest.ExecuteCommand(command, request);

            Assert.Equal(expected, result.error);
        }
    }
}