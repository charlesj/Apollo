using System;
using Apollo.Server;
using Apollo.Utilities;
using Newtonsoft.Json;
using Xunit;

namespace Apollo.Tests.Server
{
    public class JsonRpcRequestParserTests : BaseUnitTest<JsonRpcRequestParser>
    {
        [Fact]
        public void ReturnsSuccessFalse_WhenSerializerThrowsException()
        {
            var request = string.Empty;
            var serializer = Mocker.GetMock<IJsonSerializer>();
            serializer.Setup(s => s.Deserialize<JsonRpcRequest>(request))
                .Throws<Exception>();

            var result = ClassUnderTest.Parse(request);
            Assert.False(result.Success);
        }

        [Fact]
        public void ReturnsTrue_WhenAllIsGood()
        {
            var request = string.Empty;
            var serializer = Mocker.GetMock<IJsonSerializer>();
            var expected = new JsonRpcRequest();
            serializer.Setup(s => s.Deserialize<JsonRpcRequest>(request))
                .Returns(expected);

            var result = ClassUnderTest.Parse(request);
            Assert.True(result.Success);
            Assert.Same(expected, result.Request);
        }

        public class IntegrationTests
        {
            private IJsonRpcRequestParser classUnderTest;

            public IntegrationTests()
            {
                classUnderTest = new JsonRpcRequestParser(new ApolloJsonSerializer());
            }

            [Fact]
            public void ReturnsProperlyTrackedData()
            {
                var expected = new JsonRpcRequest
                {
                    Id = "1",
                    Method = "what",
                    Params = new {parameter = "yes"}
                };

                var body = JsonConvert.SerializeObject(expected);

                var result = classUnderTest.Parse(body);
                Assert.True(result.Success);
                Assert.Equal(expected.Id, result.Request.Id);
                Assert.Equal(expected.Method, result.Request.Method);

                var p = (dynamic) expected.Params;
                Assert.Equal("yes", p.parameter);
            }

            [Fact]
            public void ReturnsProperlyTrackedData_LowerCase()
            {
                var expected = new JsonRpcRequest
                {
                    Id = "1",
                    Method = "what",
                    Params = new {parameter = "yes"}
                };

                var body = JsonConvert.SerializeObject(expected).ToLowerInvariant();

                var result = classUnderTest.Parse(body);
                Assert.False(body.Contains("Id"));
                Assert.True(result.Success);
                Assert.Equal(expected.Id, result.Request.Id);
                Assert.Equal(expected.Method, result.Request.Method);

                var p = (dynamic) expected.Params;
                Assert.Equal("yes", p.parameter);
            }

            [Fact]
            public void BadRequest_NotEvenJson()
            {
                var body = "wokka wokka";

                var result = classUnderTest.Parse(body);
                Assert.False(result.Success);
            }

            [Fact]
            public void BadRequest_WhenNoId()
            {
                var expected = new
                {
                    Method = "what",
                    Params = new {parameter = "yes"}
                };

                var body = JsonConvert.SerializeObject(expected).ToLowerInvariant();
                var result = classUnderTest.Parse(body);
                Assert.False(result.Success);
            }

            [Fact]
            public void BadRequest_WhenNoMethod()
            {
                var expected = new
                {
                    id = "what",
                    Params = new {parameter = "yes"}
                };

                var body = JsonConvert.SerializeObject(expected).ToLowerInvariant();
                var result = classUnderTest.Parse(body);
                Assert.False(result.Success);
            }
        }
    }
}