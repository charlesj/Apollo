using System;
using Apollo.Utilities;
using Moq;
using Xunit;

namespace Apollo.Tests
{
    public class ConfigurationTests : BaseUnitTest<Configuration>
    {
        [Fact]
        public void CanInstantiate_WhenEnvironmentReaderThrows()
        {
            var reader = Mocker.GetMock<IEnvironmentReader>();
            reader.Setup(r => r.Read(It.IsAny<string>())).Throws<Exception>();

            var c = new Configuration(reader.Object);

            Assert.NotNull(c);
        }

        [Fact]
        public void DatabaseName_ReturnsEnvVarValue()
        {
            var reader = Mocker.GetMock<IEnvironmentReader>();
            var expected = "DatabaseName";
            reader.Setup(r => r.Read(Constants.EnvironmentalVars.DatabaseName)).Returns(expected);

            var actual = ClassUnderTest.DatabaseName();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DatabaseServer_ReturnsEnvVarValue()
        {
            var reader = Mocker.GetMock<IEnvironmentReader>();
            var expected = "Server";
            reader.Setup(r => r.Read(Constants.EnvironmentalVars.DatabaseServer)).Returns(expected);

            var actual = ClassUnderTest.DatabaseServer();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DatabaseUsername_ReturnsEnvVarValue()
        {
            var reader = Mocker.GetMock<IEnvironmentReader>();
            var expected = "Username";
            reader.Setup(r => r.Read(Constants.EnvironmentalVars.DatabaseUsername)).Returns(expected);

            var actual = ClassUnderTest.DatabaseUsername();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DatabasePassword_ReturnsEnvVarValue()
        {
            var reader = Mocker.GetMock<IEnvironmentReader>();
            var expected = "Password";
            reader.Setup(r => r.Read(Constants.EnvironmentalVars.DatabasePassword)).Returns(expected);

            var actual = ClassUnderTest.DatabasePassword();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LoginPassword_ResturnsEnvVarValue()
        {
            var reader = Mocker.GetMock<IEnvironmentReader>();
            var expected = "Password";
            reader.Setup(r => r.Read(Constants.EnvironmentalVars.LoginPassword)).Returns(expected);

            var actual = ClassUnderTest.LoginPassword();

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void IsValid_ReturnsFalse_WhenGivenANull()
        {
            var reader = Mocker.GetMock<IEnvironmentReader>();
            reader.Setup(r => r.Read(It.IsAny<string>())).Returns((string)null);
            Assert.False(ClassUnderTest.IsValid());
        }

        [Fact]
        public void IsValid_ReturnsFalse_WhenExceptionIsThrown()
        {
            var reader = Mocker.GetMock<IEnvironmentReader>();
            reader.Setup(r => r.Read(It.IsAny<string>())).Throws<Exception>();
            Assert.False(ClassUnderTest.IsValid());
        }

        [Fact]
        public void ReturnsTrue_WhenAllIsWell()
        {
            var reader = Mocker.GetMock<IEnvironmentReader>();
            reader.Setup(r => r.Read(It.IsAny<string>())).Returns("notnull");

            Assert.True(ClassUnderTest.IsValid());
        }
    }
}