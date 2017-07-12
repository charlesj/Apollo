using System.Collections.Generic;
using System.Threading.Tasks;
using Apollo.Data;
using Moq;
using Xunit;

namespace Apollo.Tests.Data
{
    public class BookmarksDataServiceTests : BaseDataUnitTest<BookmarksDataService>
    {
        public class Insert : BookmarksDataServiceTests
        {
            private Bookmark bookmark = new Bookmark();

            [Fact]
            public async void InsertsWithExpectedSqlAndObject()
            {
                await ClassUnderTest.Insert(bookmark);

                this.connection.Verify( c => c.Execute(
                    It.Is<string>(s => s == BookmarksDataService.InsertSql),
                    It.Is<object>(o => o == bookmark)));
            }
        }

        public class GetTotal : BookmarksDataServiceTests
        {
            private IEnumerable<CountResult> validResult = new List<CountResult>{ new CountResult{ count = 1}};
            public GetTotal()
            {
                this.connection.Setup(c => c.QueryAsync<CountResult>(It.IsAny<string>(), It.IsAny<object>()))
                    .Returns(Task.FromResult(validResult));
            }

            [Fact]
            public async void ExpectedQuery()
            {
                await ClassUnderTest.GetTotal();

                this.connection.Verify(c =>
                    c.QueryAsync<CountResult>(
                        BookmarksDataService.CountSql,
                        It.IsAny<object>()), Times.Once());
            }
        }

        public class GetPage : BookmarksDataServiceTests
        {
            private IEnumerable<Bookmark> validResult = new List<Bookmark>();

            public GetPage()
            {
                this.connection.Setup(c => c.QueryAsync<Bookmark>(It.IsAny<string>(), It.IsAny<object>()))
                    .Returns(Task.FromResult(validResult));
            }

            [Fact]
            public async void ExpectedSqlWhenNoUrl()
            {
                await ClassUnderTest.Get(42, null);

                this.connection.Verify(c => c.QueryAsync<Bookmark>(BookmarksDataService.BasicGetSql,
                    It.Is<object>(o => GetAnon<int>(o, "start") == 42)));
            }

            [Fact]
            public async void ExpectedSqlWithUrl()
            {
                await ClassUnderTest.Get(42, "url");

                this.connection.Verify(c => c.QueryAsync<Bookmark>(BookmarksDataService.UrlGetSql,
                    It.Is<object>(o => GetAnon<string>(o, "link") == "url")));
            }
        }
    }
}
