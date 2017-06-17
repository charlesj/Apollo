using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apollo.Data;
using Moq;
using Xunit;

namespace Apollo.Tests.Data
{
    public class MetricsDataServiceTests : BaseDataUnitTest<MetricsDataService>
    {
        private string category = "category";
        private string name = "name";
        private float value = 1.2f;

        public class InsertMetric : MetricsDataServiceTests
        {
            [Fact]
            public async void RunsExpectedSql()
            {
                await this.ClassUnderTest.InsertMetric(category, name, value);

                connection.Verify(c =>
                    c.Execute(It.Is<string>(
                            q => q == MetricsDataService.InsertSql),
                        It.Is<object>(o => 
                            GetAnonymousString(o, "category") == category && 
                            GetAnonymousString(o, "name") == name &&
                            Math.Abs(GetAnon<float>(o, "value") - value) < .1f)
                    ), Times.Once());
            }
        }

        public class GetMetrics : MetricsDataServiceTests
        {
            private IEnumerable<Metric> validResult = new List<Metric>();

            public GetMetrics()
            {
                connection.Setup(c => c.QueryAsync<Metric>(It.IsAny<string>(), It.IsAny<object>()))
                    .Returns(Task.FromResult(validResult));
            }
            
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("   ")]
            public async void QueriesForNameWhenCategoryNullOrEmpty(string cat)
            {
                await this.ClassUnderTest.GetMetrics(cat, name);
                
                connection.Verify(c => 
                    c.QueryAsync<Metric>(It.Is<string>(
                        q => q == MetricsDataService.NameSelectSql), 
                        It.Is<object>(o => 
                            GetAnonymousString(o, "category") == cat &&
                            GetAnonymousString(o, "name") == name)),
                    Times.Once());
            }
            
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("   ")]
            public async void QueriesForNameWhenNameNullOrEmpty(string n)
            {
                await this.ClassUnderTest.GetMetrics(category, n);
                
                connection.Verify(c => 
                        c.QueryAsync<Metric>(It.Is<string>(
                                q => q == MetricsDataService.CategorySelectSql), 
                            It.Is<object>(o => 
                                GetAnonymousString(o, "category") == category &&
                                GetAnonymousString(o, "name") == n)),
                    Times.Once());
            }
            
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("   ")]
            public async void QueriesForAllWhenBothEmpty(string both)
            {
                await this.ClassUnderTest.GetMetrics(both, both);

                connection.Verify(c => 
                        c.QueryAsync<Metric>(It.Is<string>(
                                q => q == MetricsDataService.AllSelectSql), 
                            It.Is<object>(o => 
                                GetAnonymousString(o, "category") == both &&
                                GetAnonymousString(o, "name") == both)),
                    Times.Once());
            }
            
            [Fact]
            public async void LimitsByNameAndCategoryWhenDefined()
            {
                await this.ClassUnderTest.GetMetrics(category, name);

                connection.Verify(c => 
                        c.QueryAsync<Metric>(It.Is<string>(
                                q => q == MetricsDataService.BothSelectSql), 
                            It.Is<object>(o => 
                                GetAnonymousString(o, "category") == category &&
                                GetAnonymousString(o, "name") == name)),
                    Times.Once());
            }
        }
    }
}