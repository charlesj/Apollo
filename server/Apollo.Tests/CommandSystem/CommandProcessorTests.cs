using System;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Moq;
using Xunit;

namespace Apollo.Tests.CommandSystem
{
    public class CommandProcessorTests : BaseUnitTest<CommandProcessor>
    {
        private ICommand command;
        private object parameters;

        public CommandProcessorTests()
        {
            command = Mock<ICommand>().Object;
            parameters = new object();
        }

        [Fact]
        public async void HydratesCommand()
        {
            var hydrator = Mock<ICommandHydrator>();

            await ClassUnderTest.Process(command, parameters);

            hydrator.Verify(h => h.Hydrate(ref command, parameters), Times.Once());
        }

        [Fact]
        public async void ReturnsInvalidResult_WhenValidationReturnsFalse()
        {
            var commandMock = Mock<ICommand>();
            commandMock.Setup(c => c.IsValid()).Returns(Task.FromResult(false));

            var result = await ClassUnderTest.Process(command, parameters);
            Assert.Equal(CommandResultType.Invalid, result.ResultStatus);
            Assert.Equal("Invalid Command", result.ErrorMessage);
            Assert.Same(command, result.Result);
        }

        [Fact]
        public async void ReturnsInvalidResult_WhenValidationThrowsException()
        {
            var commandMock = Mock<ICommand>();
            var expectedMessage = "Message";
            var exception = new Exception(expectedMessage);
            commandMock.Setup(c => c.IsValid()).Throws(exception);

            var result = await ClassUnderTest.Process(command, parameters);
            Assert.Equal(CommandResultType.Invalid, result.ResultStatus);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void ReturnsUnauthorized_WhenAuthoriszeReturnsFalse()
        {
            var commandMock = Mock<ICommand>();
            commandMock.Setup(c => c.IsValid()).Returns(Task.FromResult(true));
            commandMock.Setup(c => c.Authorize()).Returns(Task.FromResult(false));

            var result = await ClassUnderTest.Process(command, parameters);
            Assert.Equal(CommandResultType.Unauthorized, result.ResultStatus);
            Assert.Equal("Unauthorized Command", result.ErrorMessage);
            Assert.Same(command, result.Result);
        }

        [Fact]
        public async void ReturnsUnauthorizedResult_WhenAuthorizationThrowsException()
        {
            var commandMock = Mock<ICommand>();
            var expectedMessage = "Message";
            var exception = new Exception(expectedMessage);
            commandMock.Setup(c => c.IsValid()).Returns(Task.FromResult(true));
            commandMock.Setup(c => c.Authorize()).Throws(exception);

            var result = await ClassUnderTest.Process(command, parameters);
            Assert.Equal(CommandResultType.Unauthorized, result.ResultStatus);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void ReturnsErrorResult_WhenExecuteThrowsException()
        {
            var commandMock = Mock<ICommand>();
            var expectedMessage = "Message";
            var exception = new Exception(expectedMessage);
            commandMock.Setup(c => c.IsValid()).Returns(Task.FromResult(true));
            commandMock.Setup(c => c.Authorize()).Returns(Task.FromResult(true));

            commandMock.Setup(c => c.Execute()).Throws(exception);

            var result = await ClassUnderTest.Process(command, parameters);
            Assert.Equal(CommandResultType.Error, result.ResultStatus);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void ReturnsExecuteResult_WhenExecuteDoesNotThrow()
        {
            var commandMock = Mock<ICommand>();
            commandMock.Setup(c => c.IsValid()).Returns(Task.FromResult(true));
            commandMock.Setup(c => c.Authorize()).Returns(Task.FromResult(true));

            var commandResult = new CommandResult();
            commandMock.Setup(c => c.Execute()).Returns(Task.FromResult(commandResult));

            var result = await ClassUnderTest.Process(command, parameters);
            Assert.Same(commandResult, result);
        }

        [Fact]
        public async void IncludesExecutionTime()
        {
            var commandMock = Mock<ICommand>();
            commandMock.Setup(c => c.IsValid()).Returns(Task.FromResult(true));
            commandMock.Setup(c => c.Authorize()).Returns(Task.FromResult(true));

            var commandResult = new CommandResult();
            commandMock.Setup(c => c.Execute()).Returns(Task.FromResult(commandResult));

            var result = await ClassUnderTest.Process(command, parameters);

            Assert.True(result.Elapsed > -1);
        }
    }
}
