using System.Threading.Tasks;
using Apollo.Commands.Metrics;
using Apollo.CommandSystem;
using Apollo.Data;
using Moq;
using Xunit;

namespace Apollo.Tests.Commands.Metrics
{
    public class AddMetricTests : BaseUnitTest<AddMetric>
    {
        public AddMetricTests()
        {
            this.ClassUnderTest.category = "category";
            this.ClassUnderTest.name = "name";
            this.ClassUnderTest.value = 1.0m;
        }

        public class IsValid : AddMetricTests
        {
            [Fact]
            public async void RequiresCategory()
            {
                this.ClassUnderTest.category = string.Empty;
                Assert.False(await this.ClassUnderTest.IsValid());
            }

            [Fact]
            public async void RequiresName()
            {
                this.ClassUnderTest.name = string.Empty;
                Assert.False(await this.ClassUnderTest.IsValid());
            }

            [Fact]
            public async void BothCategoryAndNameReturnTrue()
            {
                Assert.True(await this.ClassUnderTest.IsValid());
            }
        }

        public class Execute : AddMetricTests
        {
            public Execute()
            {
                this.Mock<IMetricsDataService>()
                    .Setup(m => m.InsertMetric(It.IsAny<string>(),
                                               It.IsAny<string>(),
                                               It.IsAny<decimal>()))
                    .Returns(Task.FromResult(0));
            }

            [Fact]
            public async void CallsDataService()
            {
                await this.ClassUnderTest.Execute();
                this.Mock<IMetricsDataService>()
                    .Verify(mds =>
                        mds.InsertMetric(
                            this.ClassUnderTest.category,
                            this.ClassUnderTest.name,
                            this.ClassUnderTest.value),
                        Times.Once());
            }

            [Fact]
            public async void SuccessReturnsDefaultSuccessResult()
            {
                var result = await this.ClassUnderTest.Execute();
                Assert.Same(CommandResult.SuccessfulResult, result);
            }
        }
    }
}
