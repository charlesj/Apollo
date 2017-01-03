using Apollo.Commands.Meta;
using Apollo.CommandSystem;
using Apollo.ServiceLocator;
using Xunit;

namespace Apollo.Tests.CommandSystem
{
    public class CommandLocatorIntegrationTests
    {
        private ICommandLocator classUnderTest;

        public CommandLocatorIntegrationTests()
        {
            classUnderTest = new CommandLocator(new SimpleInjectorServiceLocator());
        }

        [Fact]
        public void ANonExistingCommandName_ReturnsNull()
        {
            Assert.Null(classUnderTest.Locate("Wokka wokka"));
        }

        [Fact]
        public void ReturnsInstanceOfCommand_WhenFound()
        {
            var command = classUnderTest.Locate("testCommand");
            Assert.IsType<TestCommand>(command);
        }
    }
}