using System.Threading.Tasks;
using Apollo.Runtime;
using Apollo.Utilities;
using Moq;
using Xunit;

namespace Apollo.Tests.Runtime
{
    public class SimpleLoopRuntimeTests : BaseUnitTest<SimpleLoopRuntime>
    {
        public SimpleLoopRuntimeTests()
        {
            var clockMock = Mock<IClock>();
            clockMock.Setup(d => d.Delay(It.IsAny<int>())).Returns(Task.FromResult(0));
        }

        [Fact]
        public async void LoopsUntilCancelled()
        {
            var contextMock = Mock<IRuntimeContext>();
            contextMock.Setup(rc => rc.Ending).ReturnsInOrder(false, false, true);

            await ClassUnderTest.Run();

            contextMock.Verify(ctx => ctx.StartFrame(), Times.Exactly(2));
        }

        [Fact]
        public async void CallsShutdownComplete_WhenDone()
        {
            var contextMock = Mock<IRuntimeContext>();
            contextMock.Setup(rc => rc.Ending).ReturnsInOrder(false, true);

            await ClassUnderTest.Run();

            contextMock.Verify(ctx => ctx.CompleteShutdown(), Times.Exactly(1));
        }

        [Fact]
        public async void DelaysBetweenLoops()
        {
            var contextMock = Mock<IRuntimeContext>();
            contextMock.Setup(rc => rc.Ending).ReturnsInOrder(false, true);

            await ClassUnderTest.Run();

            var clockMock = Mock<IClock>();
            clockMock.Verify(ctx => ctx.Delay(10), Times.Exactly(1));
        }
    }
}
