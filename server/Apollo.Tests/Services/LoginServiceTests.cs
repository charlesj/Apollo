using System.Collections.Generic;
using System.Threading.Tasks;
using Apollo.Data;
using Apollo.Services;
using Apollo.Utilities;
using Moq;
using Xunit;

namespace Apollo.Tests.Services
{
    public class LoginServiceTests : BaseUnitTest<LoginService>
    {
        private const string password = "password";
        private const string hash = "hash";
        private const string token = "token";

        public LoginServiceTests()
        {
            this.Mocker.GetMock<IConfiguration>()
                .Setup(l => l.LoginPasswordHash())
                .Returns(hash);

            this.Mocker.GetMock<IPasswordHasher>()
                .Setup(p => p.CheckHash(hash, password))
                .Returns(true);

            this.Mocker.GetMock<ILoginSessionDataService>()
                .Setup(r => r.UpdateLastSeen(It.IsAny<string>()))
                .Returns(Task.FromResult(0));
        }

        [Fact]
        public async void ReturnsNullWhenPasswordDoesntMatch()
        {
            this.Mocker.GetMock<IPasswordHasher>()
                .Setup(p => p.CheckHash(hash, password))
                .Returns(false);

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
        public async void StoresTokenInDb()
        {
            var result = await this.ClassUnderTest.Authenticate(password);

            this.Mocker.GetMock<ILoginSessionDataService>()
                .Verify(l => l.CreateSession(result), Times.Once());
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
            IReadOnlyList<LoginSession> sessions = new List<LoginSession>
            {
                new LoginSession
                {
                    token = token
                }
            };

            this.Mocker.GetMock<ILoginSessionDataService>()
                .Setup(l => l.GetAllSessions())
                .Returns(Task.FromResult(sessions));

            var result = await this.ClassUnderTest.ValidateToken(token);

            Assert.True(result);
        }

        [Fact]
        public async void ReturnsFalseIfTokenNotAvailable()
        {
            IReadOnlyList<LoginSession> sessions = new List<LoginSession>
            {
                new LoginSession
                {
                    token = "Something else"
                }
            };

            this.Mocker.GetMock<ILoginSessionDataService>()
                .Setup(l => l.GetAllSessions())
                .Returns(Task.FromResult(sessions));

            var result = await this.ClassUnderTest.ValidateToken(token);

            Assert.False(result);
        }

        [Fact]
        public async void UpdatesLastSeenIfValidToken()
        {
            IReadOnlyList<LoginSession> sessions = new List<LoginSession>
            {
                new LoginSession
                {
                    token = token
                }
            };

            this.Mocker.GetMock<ILoginSessionDataService>()
                .Setup(l => l.GetAllSessions())
                .Returns(Task.FromResult(sessions));

            await this.ClassUnderTest.ValidateToken(token);

            this.Mocker.GetMock<ILoginSessionDataService>()
                .Verify(l => l.UpdateLastSeen(token), Times.Once());
        }
    }
}