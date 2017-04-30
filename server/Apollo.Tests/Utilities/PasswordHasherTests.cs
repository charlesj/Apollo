using Apollo.Utilities;
using Xunit;

namespace Apollo.Tests.Utilities
{
    public class PasswordHasherTests : BaseUnitTest<PasswordHasher>
    {
        private const string password = "password";

        [Fact]
        public void CanGenerateHashFromPassword()
        {
            var hash = ClassUnderTest.GenerateHash(password);

            Assert.NotEmpty(hash);
        }

        [Fact]
        public void EveryhashIsDifferent()
        {
            var hash = ClassUnderTest.GenerateHash(password);
            var hash2 = ClassUnderTest.GenerateHash(password);

            Assert.NotEqual(hash, hash2);
        }

        [Fact]
        public void CanVerifySuccessfulHash()
        {
            var hash = ClassUnderTest.GenerateHash(password);
            var valid = ClassUnderTest.CheckHash(hash, password);

            Assert.True(valid);
        }
    }
}