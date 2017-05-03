using System;
using System.Collections.Generic;
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
        }


        public class GetUserSetting : UserSettingsDataServiceTests
        {

            public GetUserSetting()
            {
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
                    .Returns(Task.FromResult((IEnumerable<UserSetting>) new List<UserSetting>()));

                var exception =
                    await Assert.ThrowsAsync<InvalidOperationException>(
                        async () => await this.ClassUnderTest.GetUserSetting(settingName));

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

                var exception =
                    await Assert.ThrowsAsync<InvalidOperationException>(
                        async () => await this.ClassUnderTest.GetUserSetting(settingName));

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

        public class UpsertSetting : UserSettingsDataServiceTests
        {
            private const string newValue = "new value";

            [Fact]
            public async void MakesExpectedQuery()
            {
                await this.ClassUnderTest.UpdateSetting(new UserSetting
                {
                    name = settingName,
                    value = newValue
                });

                connection.Verify(c =>
                    c.Execute(It.Is<string>(
                            q => q == "update user_settings set value=@newValue, updated_at=current_timestamp where name=@name"),
                        It.Is<object>(o => GetAnonymousString(o, "name") == settingName && GetAnonymousString(o, "newValue") == newValue)
                ), Times.Once());
            }
        }
    }
}