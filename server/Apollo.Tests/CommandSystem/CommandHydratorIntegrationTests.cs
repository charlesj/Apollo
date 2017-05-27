using System;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Xunit;

namespace Apollo.Tests.CommandSystem
{
    public class CommandHydratorIntegrationTests
    {
        private ICommandHydrator classUnderTest;

        public CommandHydratorIntegrationTests()
        {
            classUnderTest = new CommandHydrator();
        }

        [Fact]
        public void CommandWithNoParamaters_AndEmptyParameters()
        {
            ICommand command = new EmptyCommand();
            classUnderTest.Hydrate(ref command, new object());
        }

        [Fact]
        public void CommandWithSimpleProperties_AndEmptyParameters()
        {
            ICommand command = new SimpleCommand();
            classUnderTest.Hydrate(ref command, new object());
        }

        [Fact]
        public void CommandWithSimpleProperties_AndGoodParameters()
        {
            ICommand command = new SimpleCommand();
            var expectedCount = 1;
            var name = "hello";
            var parameters = new
            {
                count = expectedCount,
                name,
                startTime = "1983-02-13T00:47:11+07:00"
            };

            classUnderTest.Hydrate(ref command, parameters);

            var sc = command as SimpleCommand;
            Assert.Equal(expectedCount, sc.Count);
            Assert.Equal(name, sc.Name);
            Assert.Equal(1983, sc.StartTime.Year);
            Assert.Equal(2, sc.StartTime.Month);
            Assert.Equal(13, sc.StartTime.Day);
        }

        private class SimpleCommand : CommandBase
        {
            public int Count { get; set; }
            public string Name { get; set; }
            public DateTimeOffset StartTime { get; set; }
            public override Task<CommandResult> Execute()
            {
                throw new System.NotImplementedException();
            }

            public override Task<bool> IsValid()
            {
                throw new System.NotImplementedException();
            }

            public override Task<bool> Authorize()
            {
                throw new System.NotImplementedException();
            }
        }

        private class EmptyCommand : CommandBase
        {
            public override Task<CommandResult> Execute()
            {
                throw new System.NotImplementedException();
            }

            public override Task<bool> IsValid()
            {
                throw new System.NotImplementedException();
            }

            public override Task<bool> Authorize()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}