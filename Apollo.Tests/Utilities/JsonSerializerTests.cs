using Apollo.Utilities;
using Xunit;

namespace Apollo.Tests.Utilities
{
    public class JsonSerializerTests : BaseUnitTest<JsonSerializer>
    {
        public class Serialize : JsonSerializerTests
        {
            [Fact]
            public void SerializeSimple()
            {
                var obj = new object();
                var result = ClassUnderTest.Serialize(obj);
                Assert.Equal("{}", result);
            }

            [Fact]
            public void SerializeComplex()
            {
                var obj = new {test = "yes"};
                var result = ClassUnderTest.Serialize(obj);
                Assert.Equal("{\"test\":\"yes\"}", result);
            }

            [Fact]
            public void SerializeIndent()
            {
                var obj = new {test = "yes"};
                var result = ClassUnderTest.Serialize(obj, true);
                Assert.Equal("{\n  \"test\": \"yes\"\n}", result);
            }
        }

        public class Deserialize : JsonSerializerTests
        {
            [Fact]
            public void SimpleObject()
            {
                var result = ClassUnderTest.Deserialize<object>("{}");
                Assert.NotNull(result);
            }
        }
    }
}