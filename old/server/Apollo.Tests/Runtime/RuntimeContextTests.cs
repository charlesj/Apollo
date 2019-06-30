using Apollo.Runtime;
using Xunit;

namespace Apollo.Tests.Runtime
{
    public class RuntimeContextTests
    {
        [Fact]
        public void DefaultsToNotEnding()
        {
            var context = new RuntimeContext();
            Assert.False(context.Ending);
        }

        [Fact]
        public void CallingEnd_SetsEndingToTrue()
        {
            var context = new RuntimeContext();
            context.End();

            Assert.True(context.Ending);
        }

        [Fact]
        public void CallingStartFrame_IncrementsCounter()
        {
            var ctx = new RuntimeContext();
            Assert.Equal(0, ctx.FrameNumber);
            ctx.StartFrame();
            Assert.Equal(1, ctx.FrameNumber);
        }

        [Fact]
        public void CallingCompleteShutdown_SetsEndedToTrue()
        {
            var ctx = new RuntimeContext();
            Assert.False(ctx.Ended);

            ctx.CompleteShutdown();
            Assert.True(ctx.Ended);
        }
    }
}
