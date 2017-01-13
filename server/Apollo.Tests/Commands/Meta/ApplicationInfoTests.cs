using Apollo.Commands.Meta;
using Xunit;

namespace Apollo.Tests.Commands.Meta
{
    public class ApplicationInfoTests : BaseUnitTest<ApplicationInfo>
    {
        [Fact]
        public async void PassesInfoFromVersion()
        {
            var result = await ClassUnderTest.Execute();

            Assert.Null(result.ErrorMessage);
            var simple = Assert.IsType<ApplicationInfoResult>(result.Result);
            Assert.Equal(Apollo.Version, simple.version);
            Assert.Equal(Apollo.CompiledOn, simple.compiledOn);
            Assert.Equal(Apollo.CommitHash, simple.commitHash);
        }
    }
}