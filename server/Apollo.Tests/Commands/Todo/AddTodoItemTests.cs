using System.Threading.Tasks;
using Apollo.Commands.Todo;
using Apollo.CommandSystem;
using Apollo.Data;
using Moq;
using Xunit;

namespace Apollo.Tests.Commands.Todo
{
    public class AddTodoItemTests : BaseUnitTest<AddTodoItem>
    {
        public class Execute : AddTodoItemTests
        {
            private const string title = "title";
            public Execute()
            {
                this.ClassUnderTest.Title = title;
                Mock<ITodoItemDataService>().Setup(t => t.AddItem(It.IsAny<string>()))
                    .Returns(Task.FromResult(0));
            }

            [Fact]
            public async void CallsDataService()
            {
                await ClassUnderTest.Execute();

                Mock<ITodoItemDataService>()
                    .Verify(t => t.AddItem(title), Times.Once());
            }

            [Fact]
            public async void ReturnsSuccessResult()
            {
                var result = await ClassUnderTest.Execute();
                Assert.Same(CommandResult.SuccessfulResult, result);
            }
        }

        public class IsValid : AddTodoItemTests
        {
            [Theory]
            [InlineData("", false)]
            [InlineData("  ", false)]
            [InlineData("    ", false)]
            [InlineData("\n", false)]
            [InlineData("\t", false)]
            [InlineData("content", true)]
            public async void ChecksTitleForNonEmptyValue(string title, bool expected)
            {
                this.ClassUnderTest.Title = title;
                Assert.Equal(expected, await ClassUnderTest.IsValid());
            }
        }
    }
}
