using System.Text;
using Apollo.Utilities;
using Xunit;

namespace Apollo.Tests.Utilities
{
    public class Base64ConverterTests : BaseUnitTest<Base64Converter>
    {
        private string messageEncoded = "VGhpcyBpcyBteSBtZXNzYWdl";
        private byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        const string message = "This is my message";

        [Fact]
        public void CanGetEncodingFromBytes()
        {
            var actual = this.ClassUnderTest.Encode(messageBytes);
            Assert.Equal(messageEncoded, actual);
        }

        [Fact]
        public void CanGetEncodingFromStringAssumingUTF8()
        {
            var actual = this.ClassUnderTest.Encode(message);
            Assert.Equal(messageEncoded, actual);
        }

        [Fact]
        public void CanGetBytesFromBase64String()
        {
            var actual = this.ClassUnderTest.Decode(messageEncoded);
            Assert.Equal(messageBytes, actual);
        }

        [Fact]
        public void CanGetStringFromBase64String()
        {
            var actual = this.ClassUnderTest.DecodeString(messageEncoded);
            Assert.Equal(message, actual);
        }
    }
}