using System;
using Apollo.Server;
using Xunit;

namespace Apollo.Tests.Server
{
    public class TestableHttpListenerContextTests
    {
        [Fact]
        public void CanHandlePassedNulls()
        {
            var context = new TestableHttpListenerContext(null);
            Assert.NotNull(context);
        }

        [Fact]
        public void ThrowsProperNullRef_WhenAccessFirstTime()
        {
            var context = new TestableHttpListenerContext(null);
            var exception = Assert.Throws<ArgumentNullException>(() => context.GetRequestBody());
            Assert.Equal("passedContext", exception.ParamName);
            Assert.True(exception.Message.StartsWith("Null HttpListenerContext Encountered at Lazy Evaluation"));
        }
    }
}