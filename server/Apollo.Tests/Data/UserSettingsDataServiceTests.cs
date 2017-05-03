using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Apollo.Data;
using Moq;
using Xunit;

namespace Apollo.Tests.Data
{
    public class UserSettingsDataServiceTests : BaseUnitTest<UserSettingsDataService>
    {
        private const string settingName = "setting";
        private Mock<ITestableDbConnection> connection;

        public UserSettingsDataServiceTests()
        {
            connection = Mock<ITestableDbConnection>();

            Mock<IDbConnectionFactory>()
                .Setup(s => s.GetConnection())
                .Returns(Task.FromResult(connection.Object));

            IEnumerable<UserSetting> queryResults = new List<UserSetting>
            {
                new UserSetting()
            };

            connection.Setup(c => c.QueryAsync<UserSetting>(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.FromResult(queryResults));
        }

        [Fact]
        public async void UsesExpectedQuery()
        {
            await this.ClassUnderTest.GetUserSetting(settingName);

            connection
                .Verify(c =>
                    c.QueryAsync<UserSetting>(
                        It.Is<string>(s => s == "select * from user_settings where name=@name"),
                        It.Is<object>(o => GetAnonymousString(o, "name") == settingName)
                    )
                );
        }

        [Fact]
        public async void ThrowsExceptionIfNoResults()
        {
            connection.Setup(c => c.QueryAsync<UserSetting>(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.FromResult((IEnumerable<UserSetting>)new List<UserSetting>()));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await this.ClassUnderTest.GetUserSetting(settingName));

            Assert.Equal("Could not find user setting 'setting'", exception.Message);
        }

        [Fact]
        public async void ThrowsExceptionIfMultipleResults()
        {
            IEnumerable<UserSetting> queryResults = new List<UserSetting>
            {
                new UserSetting(),
                new UserSetting()
            };

            connection.Setup(c => c.QueryAsync<UserSetting>(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.FromResult(queryResults));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await this.ClassUnderTest.GetUserSetting(settingName));

            Assert.Equal("Multiple values for 'setting' - fix the database", exception.Message);
        }

        [Fact]
        public async void ReturnsTheOnlyResult()
        {
            var expected = new UserSetting();
            IEnumerable<UserSetting> queryResults = new List<UserSetting>
            {
                expected
            };

            connection.Setup(c => c.QueryAsync<UserSetting>(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.FromResult(queryResults));

            var actual = await ClassUnderTest.GetUserSetting(settingName);

            Assert.Same(expected, actual);
        }
    }
}