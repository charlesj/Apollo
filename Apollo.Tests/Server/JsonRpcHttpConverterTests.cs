using Apollo.Server;
using Apollo.Utilities;
using Xunit;

namespace Apollo.Tests.Server
{
    public class JsonRpcHttpConverterTests : BaseUnitTest<JsonRpcHttpConverter>
    {
        [Fact]
        public void NullJsonResponse_Returns503()
        {
            var result = ClassUnderTest.Convert(null);

            Assert.Equal(503, result.HttpCode);
            Assert.Equal("Null Command Result", result.Body);
        }

        [Fact]
        public void Returns503WithSerializedResponse_WhenErrorIsNotNull()
        {
            var jsonResult = new JsonRpcResponse{ error = "error"};
            var serializer = Mocker.GetMock<IJsonSerializer>();
            var serialized = "serialized";
            serializer.Setup(s => s.Serialize(jsonResult))
                .Returns(serialized);
            var result = ClassUnderTest.Convert(jsonResult);

            Assert.Equal(503, result.HttpCode);
            Assert.Equal(serialized, result.Body);
        }

        [Fact]
        public void Returns200WithSerializedResponse_WhenErrorIsNull()
        {
            var jsonResult = new JsonRpcResponse{ result = "yes i am result"};
            var serializer = Mocker.GetMock<IJsonSerializer>();
            var serialized = "serialized";
            serializer.Setup(s => s.Serialize(jsonResult))
                .Returns(serialized);
            var result = ClassUnderTest.Convert(jsonResult);

            Assert.Equal(200, result.HttpCode);
            Assert.Equal(serialized, result.Body);
        }
    }
}