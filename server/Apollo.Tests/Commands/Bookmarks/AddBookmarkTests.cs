using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Commands.Bookmarks;
using Apollo.CommandSystem;
using Apollo.Data;
using Moq;
using Xunit;

namespace Apollo.Tests.Commands.Bookmarks
{
    public class AddBookmarkTests : BaseUnitTest<SaveBookmark>
    {
        public AddBookmarkTests()
        {
            this.ClassUnderTest.description = "description";
            this.ClassUnderTest.tags = new List<string>{"tag1", "tag2"};
            this.ClassUnderTest.link = "http://example.com";
            this.ClassUnderTest.title = "title";
            this.ClassUnderTest.createdAt = DateTime.Now;
            this.ClassUnderTest.modifiedAt = DateTime.Today;
        }

        public class Execute : AddBookmarkTests
        {
            public Execute()
            {
                this.Mock<IBookmarksDataService>()
                    .Setup(bds => bds.Upsert(It.IsAny<Bookmark>()))
                    .Returns(Task.FromResult(0));
            }

            [Fact]
            public async void ConstructsBookmarkAndInserts()
            {
                await this.ClassUnderTest.Execute();
                
                this.Mock<IBookmarksDataService>()
                    .Verify(bds => bds.Upsert(
                        It.Is<Bookmark>(
                            b => b.title == this.ClassUnderTest.title &&
                                 b.link == this.ClassUnderTest.link &&
                                 b.description == this.ClassUnderTest.description &&
                                 b.tags.All(t => this.ClassUnderTest.tags.Contains(t)) &&
                                 b.created_at == this.ClassUnderTest.createdAt &&
                                 b.modified_at == this.ClassUnderTest.modifiedAt)),
                        Times.Once());
            }

            [Fact]
            public async void ReturnsSuccessResult()
            {
                var result = await this.ClassUnderTest.Execute();
                Assert.Same(CommandResult.SuccessfulResult, result);
            }
        }

        public class IsValid : AddBookmarkTests
        {
            [Fact]
            public async void IsTrueWhenAllAreSet()
            {
                Assert.True(await this.ClassUnderTest.IsValid());
            }

            [Theory]
            [InlineData("")]
            [InlineData("  ")]
            [InlineData("    ")]
            [InlineData("\n")]
            [InlineData("\t")]
            public async void IsFalseWhenTitleIsNullOrWhitespace(string value)
            {
                this.ClassUnderTest.title = value;
                Assert.False(await this.ClassUnderTest.IsValid());
            }

            [Theory]
            [InlineData("")]
            [InlineData("  ")]
            [InlineData("    ")]
            [InlineData("\n")]
            [InlineData("\t")]
            public async void IsFalseWhenLinkIsNullOrWhitespace(string value)
            {
                this.ClassUnderTest.link = value;
                Assert.False(await this.ClassUnderTest.IsValid());
            }
        }
    }
}