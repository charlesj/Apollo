using System.Threading.Tasks;
using Apollo.Commands;
using Apollo.CommandSystem;
using Apollo.Services;
using Moq;
using Xunit;

namespace Apollo.Tests.Commands
{
    public class LoginTests : BaseUnitTest<Login>
    {
        public LoginTests()
        {
            var loginMock = this.Mocker.GetMock<ILoginService>();
            loginMock.Setup(l => l.Authenticate(It.IsAny<string>()))
                .Returns(Task.FromResult(string.Empty));
        }
        [Theory]
        [InlineData("", false)]
        [InlineData("  ", false)]
        [InlineData("    ", false)]
        [InlineData("\n", false)]
        [InlineData("\t", false)]
        [InlineData("password", true)]
        public async void ValidatesNoteContent(string password, bool expectedValid)
        {
            this.ClassUnderTest.Password = password;
            var isValid = await this.ClassUnderTest.IsValid();
            Assert.Equal(expectedValid, isValid);
        }

        [Fact]
        public async void ChecksLoginServiceUsingPassword()
        {
            var password = "YES THIS IS PASSWORD";
            this.ClassUnderTest.Password = password;
            await this.ClassUnderTest.Execute();

            this.Mocker.GetMock<ILoginService>()
                .Verify(l => l.Authenticate(It.Is<string>(s => s == password)));
        }

        [Fact]
        public async void ReturnsUnsuccessfulIfLoginServerReturnsEmptyString()
        {
            var result = await this.ClassUnderTest.Execute();

            Assert.Equal(CommandResultType.Error, result.ResultStatus);
        }

        [Fact]
        public async void ReturnsTokenFromLoginServiceIfNotEmpty()
        {
            var token = "I AM TOKEN";
            var loginMock = this.Mocker.GetMock<ILoginService>();
            loginMock.Setup(l => l.Authenticate(It.IsAny<string>()))
                .Returns(Task.FromResult(token));

            var result = await this.ClassUnderTest.Execute();
            var dresult = (Login.LoginResult) result.Result;

            Assert.Equal(token, dresult.token);
        }

        [Fact]
        public async void AuthorizeReturnsTrue()
        {
            var result = await ClassUnderTest.Authorize();

            Assert.True(result);
        }
    }
}