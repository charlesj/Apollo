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
                    .Setup(v => v.Serialize(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(serializedValue);

                Mock<IUserSettignsDataService>()
                    .Setup(u => u.UpsertSetting(It.IsAny<UserSetting>()))
                    .Returns(Task.FromResult(0));
            }

            [Fact]
            public async void BubblesExceptionWhenSerializerThrows()
            {
                var settingValue = "yep";
                Mock<IJsonSerializer>()
                    .Setup(v => v.Serialize(settingValue, It.IsAny<bool>()))
                    .Throws<Exception>();

                await Assert.ThrowsAsync<Exception>(async () =>
                    await this.ClassUnderTest.SetSetting<string>(propName, settingValue));
            }

            [Fact]
            public async void SavesNewsSettingValueInDatabase()
            {
                await this.ClassUnderTest.SetSetting(propName, "whatever");

                Mock<IUserSettignsDataService>()
                    .Verify(u => u.UpsertSetting(
                        It.Is<UserSetting>(us =>
                            us.Name == propName
                            && us.Value == serializedValue)), Times.Once());
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
                        Name = propName,
                        Value = serializedValue
                    }));

                var realValue = "real value";
                Mock<IJsonSerializer>()
                    .Setup(j => j.Deserialize<string>(serializedValue))
                    .Returns(realValue);

                var result = await this.ClassUnderTest.GetSetting<string>(propName);

                Assert.Same(realValue, result);
            }
        }
    }
}