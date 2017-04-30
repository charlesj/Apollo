using Apollo.Services;
using Xunit;

namespace Apollo.Tests.Services
{
    public class LoginServiceTests : BaseUnitTest<LoginService>
    {
        private const string password = "password";

        public LoginServiceTests()
        {
            this.Mocker.GetMock<IConfiguration>()
                .Setup(l => l.LoginPassword())
                .Returns(password);
        }

        [Fact]
        public async void ReturnsNullWhenPasswordDoesntMatch()
        {
            this.Mocker.GetMock<IConfiguration>()
                .Setup(l => l.LoginPassword())
                .Returns("something");

            var result = await this.ClassUnderTest.Authenticate(null);

            Assert.Null(result);
        }

        [Fact]
        public async void ReturnsTokenWhenPasswordMatches()
        {
            var result = await this.ClassUnderTest.Authenticate(password);

            Assert.NotNull(result);
        }

        [Fact]
        public async void CallingAuthenticateAgainReturnsDifferentToken()
        {
            var result = await this.ClassUnderTest.Authenticate(password);
            var result2 = await this.ClassUnderTest.Authenticate(password);

            Assert.NotEqual(result, result2);
        }

        [Fact]
        public async void CanValidateTokenAfterAuthorization()
        {
            var token = await this.ClassUnderTest.Authenticate(password);
            var result = await this.ClassUnderTest.ValidateToken(token);

            Assert.True(result);
        }
    }
}