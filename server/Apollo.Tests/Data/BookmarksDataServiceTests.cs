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
    }
}