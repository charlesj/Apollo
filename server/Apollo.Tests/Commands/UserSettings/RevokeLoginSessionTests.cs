using System.Threading.Tasks;
using Apollo.Commands.UserSettings;
using Apollo.Services;
using Moq;
using Xunit;

namespace Apollo.Tests.Commands.UserSettings
{
    public class RevokeLoginSessionTests : BaseUnitTest<RevokeLoginSession>
    {
        [Theory]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("token", true)]
        public async void ValidateLooksForTokenToRevoke(string token, bool expectedValue)
        {
            this.ClassUnderTest.TokenToRevoke = token;
            var result = await this.ClassUnderTest.IsValid();

            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public async void Exectute_CallsLoginServiceToRevokePassedToken()
        {
            this.Mock<ILoginService>().Setup(l => l.RevokeToken(It.IsAny<string>()))
                .Returns(Task.FromResult(0));
            
            this.ClassUnderTest.TokenToRevoke = "token";
            var result = await this.ClassUnderTest.Execute();
            
            this.Mock<ILoginService>()
                .Verify(l => l.RevokeToken(It.Is<string>(t => t == this.ClassUnderTest.TokenToRevoke)));
        }
    }
}