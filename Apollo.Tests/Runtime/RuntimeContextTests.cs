using Apollo.Runtime;
using Xunit;

namespace Apollo.Tests.Runtime
{
    public class RuntimeContextTests
    {
        [Fact]
        public void DefaultsToNotEnding()
        {
            var semaphore = new RuntimeContext();
            Assert.False(semaphore.Ending);
        }

        [Fact]
        public void CallingEnd_SetsEndingToTrue()
        {
            var semaphore = new RuntimeContext();
            semaphore.End();

            Assert.True(semaphore.Ending);
        }

        [Fact]
        public void CallingStartFrame_IncrementsCounter()
        {

        }
    }
}