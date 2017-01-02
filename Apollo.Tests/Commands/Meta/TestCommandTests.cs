using Apollo.Commands.Meta;
using Apollo.CommandSystem;
using Xunit;

namespace Apollo.Tests.Commands.Meta
{
    public class TestCommandTests : BaseUnitTest<TestCommand>
    {
        [Fact]
        public async void ValidateIsAlwaysTrue()
        {
            Assert.True(await ClassUnderTest.Validate());
        }

        [Fact]
        public async void AuthorizeIsAlwaysTrue()
        {
            Assert.True(await ClassUnderTest.Authorize());
        }

        [Fact]
        public async void ExecuteReturnsSuccessResult()
        {
            var result = await ClassUnderTest.Execute();
            Assert.Equal(CommandResultType.Success, result.ResultStatus);
        }
    }
}