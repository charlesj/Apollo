using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Apollo.Commands.UserSettings;
using Apollo.Data;
using Xunit;

namespace Apollo.Tests.Commands.UserSettings
{
    public class GetAllActiveLoginSessionsTests : BaseUnitTest<GetAllActiveLoginSessions>
    {
        [Fact]
        public async void ValidateAlwaysReturnsTrue()
        {
            var valid = await this.ClassUnderTest.IsValid();
            Assert.True(valid);
        }

        [Fact]
        public async void ExecuteReturnsActiveSessions()
        {
            IReadOnlyList<LoginSession> sessions = new List<LoginSession>
            {
                new LoginSession
                {
                    token = "token"
                }
            };

            Mock<ILoginSessionDataService>()
                .Setup(l => l.GetAllActiveSessions())
                .Returns(Task.FromResult(sessions));

            var result = await this.ClassUnderTest.Execute();

            Assert.Same(sessions, result.Result);
        }
    }
}
