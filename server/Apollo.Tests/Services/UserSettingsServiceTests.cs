using System;
using System.Threading.Tasks;
using Apollo.Data;
using Apollo.Services;
using Apollo.Utilities;
using Moq;
using Xunit;

namespace Apollo.Tests.Services
{
    public class UserSettingsServiceTests : BaseUnitTest<UserSettingsService>
    {
        private const string propName = "prop";
        private const string serializedValue = "serialized";

        public class SetSetting : UserSettingsServiceTests
        {
            public SetSetting()
            {
                Mock<IJsonSerializer>()
                    .Setup(v => v.Serialize(It.IsAny<UserSettingsService.Wrapper<string>>(), It.IsAny<bool>()))
                    .Returns(serializedValue);

                Mock<IUserSettignsDataService>()
                    .Setup(u => u.UpdateSetting(It.IsAny<UserSetting>()))
                    .Returns(Task.FromResult(0));
            }

            [Fact]
            public async void BubblesExceptionWhenSerializerThrows()
            {
                var settingValue = "yep";
                Mock<IJsonSerializer>()
                    .Setup(v => v.Serialize(It.IsAny<UserSettingsService.Wrapper<string>>(), It.IsAny<bool>()))
                    .Throws<Exception>();

                await Assert.ThrowsAsync<Exception>(async () =>
                    await this.ClassUnderTest.SetSetting<string>(propName, settingValue));
            }

            [Fact]
            public async void SavesNewsSettingValueInDatabase()
            {
                await this.ClassUnderTest.SetSetting(propName, "whatever");

                Mock<IUserSettignsDataService>()
                    .Verify(u => u.UpdateSetting(
                        It.Is<UserSetting>(us =>
                            us.name == propName
                            && us.value == serializedValue)), Times.Once());
            }
        }

        public class GetSettings : UserSettingsServiceTests
        {
            [Fact]
            public async void ReturnsTheDeserializedValue()
            {
                Mock<IUserSettignsDataService>()
                    .Setup(u => u.GetUserSetting(propName))
                    .Returns(Task.FromResult(new UserSetting
                    {
                        name = propName,
                        value = serializedValue
                    }));

                var realValue = "real value";
                var returnWrapped = new UserSettingsService.Wrapper<string>(realValue);
                Mock<IJsonSerializer>()
                    .Setup(j => j.Deserialize<UserSettingsService.Wrapper<string>>(serializedValue))
                    .Returns(returnWrapped);

                var result = await this.ClassUnderTest.GetSetting<string>(propName);

                Assert.Same(realValue, result);
            }
        }
    }
}
