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
        private const string ipAddress = "ipAddress";
        private const string userAgent = "userAgent";

        public LoginServiceTests()
        {
            this.Mocker.GetMock<IUserSettingsService>()
                .Setup(l => l.GetSetting<string>(Constants.UserSettings.PasswordHash))
                .Returns(Task.FromResult(hash));

            this.Mocker.GetMock<IPasswordHasher>()
                .Setup(p => p.CheckHash(hash, password))
                .Returns(true);

            this.Mocker.GetMock<ILoginSessionDataService>()
                .Setup(r => r.UpdateLastSeen(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(0));

            this.Mocker.GetMock<ILoginSessionDataService>()
                .Setup(r => r.CreateSession(It.IsAny<string>()))
                .Returns(Task.FromResult(0));
        }

        [Fact]
        public async void ReadsHashFromUserSettings()
        {
            await this.ClassUnderTest.Authenticate(password);

            this.Mocker.GetMock<IUserSettingsService>()
                .Verify(u => u.GetSetting<string>(Constants.UserSettings.PasswordHash), Times.Once());
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
                .Setup(l => l.GetAllActiveSessions())
                .Returns(Task.FromResult(sessions));

            var result = await this.ClassUnderTest.ValidateToken(token, ipAddress, userAgent);

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
                .Setup(l => l.GetAllActiveSessions())
                .Returns(Task.FromResult(sessions));

            var result = await this.ClassUnderTest.ValidateToken(token, ipAddress, userAgent);

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
                .Setup(l => l.GetAllActiveSessions())
                .Returns(Task.FromResult(sessions));

            await this.ClassUnderTest.ValidateToken(token, ipAddress, userAgent);

            this.Mocker.GetMock<ILoginSessionDataService>()
                .Verify(l => l.UpdateLastSeen(token, ipAddress, userAgent), Times.Once());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void CheckPasswordReturnsTrueIfMatch(bool expected)
        {
            Mock<IUserSettingsService>()
                .Setup(u => u.GetSetting<string>(Constants.UserSettings.PasswordHash))
                .Returns(Task.FromResult(hash));

            Mock<IPasswordHasher>()
                .Setup(p => p.CheckHash(hash, password))
                .Returns(expected);

            var result = await this.ClassUnderTest.CheckPassword(password);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async void RevokeTokenCallsLoginDataService()
        {
            this.Mocker.GetMock<ILoginSessionDataService>()
                .Setup(l => l.RevokeSession(It.IsAny<string>()))
                .Returns(Task.FromResult(string.Empty));

            await this.ClassUnderTest.RevokeToken(token);

            this.Mocker.GetMock<ILoginSessionDataService>()
                .Verify(l => l.RevokeSession(token), Times.Once());
        }
    }
}
