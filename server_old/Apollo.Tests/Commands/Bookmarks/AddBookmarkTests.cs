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
    public class AddBookmarkTests : BaseUnitTest<AddBookmark>
    {
        public AddBookmarkTests()
        {
            this.ClassUnderTest.Description = "description";
            this.ClassUnderTest.Tags = new List<string>{"tag1", "tag2"};
            this.ClassUnderTest.Link = "http://example.com";
            this.ClassUnderTest.Title = "title";
            this.ClassUnderTest.CreatedAt = DateTime.Now;
            this.ClassUnderTest.ModifiedAt = DateTime.Today;
        }

        public class Execute : AddBookmarkTests
        {
            public Execute()
            {
                this.Mock<IBookmarksDataService>()
                    .Setup(bds => bds.Insert(It.IsAny<Bookmark>()))
                    .Returns(Task.FromResult(0));
            }

            [Fact]
            public async void ConstructsBookmarkAndInserts()
            {
                await this.ClassUnderTest.Execute();
                
                this.Mock<IBookmarksDataService>()
                    .Verify(bds => bds.Insert(
                        It.Is<Bookmark>(
                            b => b.title == this.ClassUnderTest.Title &&
                                 b.link == this.ClassUnderTest.Link &&
                                 b.description == this.ClassUnderTest.Description &&
                                 b.tags.All(t => this.ClassUnderTest.Tags.Contains(t)) &&
                                 b.created_at == this.ClassUnderTest.CreatedAt &&
                                 b.modified_at == this.ClassUnderTest.ModifiedAt)),
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
                this.ClassUnderTest.Title = value;
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
                this.ClassUnderTest.Link = value;
                Assert.False(await this.ClassUnderTest.IsValid());
            }
        }
    }
}