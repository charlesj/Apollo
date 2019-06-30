using System.Threading.Tasks;
using Apollo.Commands;
using Apollo.CommandSystem;
using Apollo.Services;
using Moq;
using Xunit;

namespace Apollo.Tests.Commands
{
    public class AuthenticatedCommandTests : BaseUnitTest<AuthenticatedCommandTests.TestableAuthenticatedCommand>
    {
        public AuthenticatedCommandTests()
        {
            var loginServiceMock = this.Mock<ILoginService>();
            loginServiceMock.Setup(l => l.ValidateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            this.ClassUnderTest.Token = "Token";
            this.ClassUnderTest.ClientIpAddress = "ipAddress";
            this.ClassUnderTest.ClientUserAgent = "userAgent";
        }

        [Fact]
        public async void AuthenticateCallsLoginService()
        {
            var result = await this.ClassUnderTest.Authorize();

            this.Mock<ILoginService>()
                .Verify(l =>
                    l.ValidateToken(
                        this.ClassUnderTest.Token,
                        this.ClassUnderTest.ClientIpAddress,
                        this.ClassUnderTest.ClientUserAgent), Times.Once());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void ReturnsLoginValidateServiceResults(bool expectedResult)
        {
            var loginServiceMock = this.Mock<ILoginService>();
            loginServiceMock.Setup(l => l.ValidateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(expectedResult));

            var result = await this.ClassUnderTest.Authorize();

            Assert.Equal(expectedResult, result);
        }

        public class TestableAuthenticatedCommand : AuthenticatedCommand
        {
            public TestableAuthenticatedCommand(ILoginService loginService) : base(loginService)
            {
            }

            public override Task<CommandResult> Execute()
            {
                throw new System.NotImplementedException();
            }

            public override Task<bool> IsValid()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
