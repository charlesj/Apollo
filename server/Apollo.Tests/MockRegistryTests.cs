using Moq;
using Xunit;

namespace Apollo.Tests
{
    public class MockRegistryTests
    {
        private readonly MockRegistry ClassUnderTest;

        public MockRegistryTests()
        {
            this.ClassUnderTest = new MockRegistry();
        }

        [Fact]
        public void CanGetUsingGeneric()
        {
            var mock = this.ClassUnderTest.Get<ITestInterface>();
            Assert.IsType<Mock<ITestInterface>>(mock);
        }

        [Fact]
        public void CanGetAType()
        {
            this.ClassUnderTest.Get(typeof(ITestInterface));
        }

        [Fact]
        public void ReturnedObject_CanBeCastToRequestedType()
        {
            var constructed = this.ClassUnderTest.Get(typeof(ITestInterface));
            Assert.IsAssignableFrom(typeof(ITestInterface), constructed);
        }

        [Fact]
        public void RequestingSameType_ReturnsSameObject()
        {
            var first = this.ClassUnderTest.Get(typeof(ITestInterface));
            var second = this.ClassUnderTest.Get(typeof(ITestInterface));
            Assert.Same(first, second);
        }

        public interface ITestInterface
        {

        }
    }
}
