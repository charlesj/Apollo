using System.Collections.Generic;
using System.Threading.Tasks;
using Apollo.Commands.Bookmarks;
using Apollo.Data;
using Moq;
using Xunit;

namespace Apollo.Tests.Commands.Bookmarks
{
    public class GetBookmarksTests : BaseUnitTest<GetBookmarks>
    {
        public class Execute : GetBookmarksTests
        {
            IReadOnlyList<Bookmark> results = new List<Bookmark>();
            public Execute()
            {
                this.Mock<IBookmarksDataService>()
                    .Setup(bds => bds.GetTotal())
                    .Returns(Task.FromResult(42));

                this.Mock<IBookmarksDataService>()
                    .Setup(bds => bds.GetPage(It.IsAny<int>()))
                    .Returns(Task.FromResult(results));
            }

            [Fact]
            public async void SetsTotalFromBookmarkService()
            {
                var result = await ClassUnderTest.Execute();
                var dres = (GetBookmarks.GetBookmarksResult) result.Result;
                Assert.Equal(42, dres.total);
            }
            
            [Fact]
            public async void SetsResultsFromBookmarkService()
            {
                var result = await ClassUnderTest.Execute();
                var dres = (GetBookmarks.GetBookmarksResult) result.Result;
                Assert.Same(results, dres.bookmarks);
            }
        }
        
        public class IsValid : GetBookmarksTests
        {
            [Fact]
            public async void AlwaysTrue()
            {
                Assert.True(await ClassUnderTest.IsValid());
            }
        }
    }
}