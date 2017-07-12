using System.Runtime.Remoting;
using System.Threading.Tasks;
using Apollo.Commands.UserSettings;
using Apollo.CommandSystem;
using Apollo.Services;
using Apollo.Utilities;
using Moq;
using Xunit;

namespace Apollo.Tests.Commands.UserSettings
{
    public class ChangePasswordTests : BaseUnitTest<ChangePassword>
    {
        private const string newHash = "hash";

        public ChangePasswordTests()
        {
            this.ClassUnderTest.NewPassword = "one";
            Mock<ILoginService>()
                .Setup(l => l.CheckPassword(this.ClassUnderTest.CurrentPassword))
                .Returns(Task.FromResult(true));

            Mock<IPasswordHasher>()
                .Setup(p => p.GenerateHash(It.IsAny<string>()))
                .Returns(newHash);

            Mock<IUserSettingsService>()
                .Setup(u => u.SetSetting<string>(Constants.UserSettings.PasswordHash, newHash))
                .Returns(Task.FromResult(0));
        }

        [Fact]
        public async void InvalidIfCurrentPasswordWrong()
        {
            Mock<ILoginService>()
                .Setup(l => l.CheckPassword(this.ClassUnderTest.CurrentPassword))
                .Returns(Task.FromResult(false));

            var result  = await this.ClassUnderTest.IsValid();

            Assert.False(result);
        }

        [Fact]
        public async void InvalidIfNewPasswordsDiffer()
        {
            this.ClassUnderTest.NewPasswordVerification = "two";
            var result  = await this.ClassUnderTest.IsValid();

            Assert.False(result);
        }
        [Fact]
        public async void InvalidIfNewPasswordsMatch()
        {
            this.ClassUnderTest.NewPasswordVerification = "one";
            var result  = await this.ClassUnderTest.IsValid();

            Assert.True(result);
        }

        [Fact]
        public async void GeneratesNewHashFromNewPassword()
        {
            await this.ClassUnderTest.Execute();

            Mock<IPasswordHasher>()
                .Verify(p => p.GenerateHash(this.ClassUnderTest.NewPassword), Times.Once());
        }

        [Fact]
        public async void UpdatesUserSettingToNewHash()
        {
            await this.ClassUnderTest.Execute();

            Mock<IUserSettingsService>()
                .Verify(u => u.SetSetting<string>(Constants.UserSettings.PasswordHash, newHash));
        }

        [Fact]
        public async void ReturnsSuccessResult()
        {
            var result = await this.ClassUnderTest.Execute();

            Assert.Same(CommandResult.SuccessfulResult, result);
        }

        [Fact]
        public async void InvalidTokenIsUnauthorized()
        {
            Mock<ILoginService>().Setup(l => l.ValidateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var result = await this.ClassUnderTest.Authorize();

            Assert.False(result);
        }

        [Fact]
        public async void ValidTokenIsAuthorized()
        {
            Mock<ILoginService>().Setup(l => l.ValidateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            var result = await this.ClassUnderTest.Authorize();

            Assert.True(result);
        }
    }
}
