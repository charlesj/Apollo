using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using Apollo.Commands.Metrics;
using Apollo.Data;
using Moq;
using Xunit;

namespace Apollo.Tests.Commands.Metrics
{
    public class GetMetricsTests : BaseUnitTest<GetMetrics>
    {
        public GetMetricsTests()
        {
            this.ClassUnderTest.Category = "category";
            this.ClassUnderTest.Name = "name";
        }

        public class Exectute : GetMetricsTests
        {
            private IReadOnlyList<Metric> validResult = new List<Metric>();
            public Exectute()
            {
                this.Mock<IMetricsDataService>()
                    .Setup(mds => mds.GetMetrics(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(validResult));
            }

            [Fact]
            public async void PassesCategoryAndNameToDataService()
            {
                await ClassUnderTest.Execute();
                this.Mock<IMetricsDataService>()
                    .Verify(mds => mds.GetMetrics("category", "name"), Times.Once());
            }
            
            [Fact]
            public async void ReturnsResultsFromDataService()
            {
                var result = await ClassUnderTest.Execute();
                Assert.Same(validResult, result.Result);
            }
        }
        
        public class IsValid : GetMetricsTests
        {
            [Fact]
            public async void ReturnsTrueIfNoPropsSet()
            {
                this.ClassUnderTest.Category = null;
                this.ClassUnderTest.Name = null;
                
                Assert.True(await this.ClassUnderTest.IsValid());
            }

            [Fact]
            public async void ReturnsTrueIfCategoryNull()
            {
                this.ClassUnderTest.Category = null;
                Assert.True(await this.ClassUnderTest.IsValid());
            }

            [Fact]
            public async void ReturnsTrueIfNameNull()
            {
                this.ClassUnderTest.Name = null;
                Assert.True(await this.ClassUnderTest.IsValid());
            }
        }
    }
}