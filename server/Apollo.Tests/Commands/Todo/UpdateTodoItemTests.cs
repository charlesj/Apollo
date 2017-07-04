using System.Threading.Tasks;
using Apollo.Commands.Todo;
using Apollo.CommandSystem;
using Apollo.Data;
using Moq;
using Xunit;

namespace Apollo.Tests.Commands.Todo
{
    public class UpdateTodoItemTests : BaseUnitTest<UpdateTodoItem>
    {
        public class Execute : UpdateTodoItemTests
        {
            public Execute()
            {
                ClassUnderTest.Item = TestData.TodoItems.TodoItem();
                Mock<ITodoItemDataService>()
                    .Setup(t => t.UpdateItem(It.IsAny<TodoItem>()))
                    .Returns(Task.FromResult(0));
            }

            [Fact]
            public async void CallsDataServerWithItem()
            {
                await ClassUnderTest.Execute();

                Mock<ITodoItemDataService>()
                    .Verify(t => t.UpdateItem(ClassUnderTest.Item), Times.Once());
            }

            [Fact]
            public async void ReturnsSuccessResult()
            {
                var result = await ClassUnderTest.Execute();
                Assert.Same(CommandResult.SuccessfulResult, result);
            }
        }

        public class IsValid : UpdateTodoItemTests
        {
            [Fact]
            public async void ReturnsFalseIfItemIsNull()
            {
                Assert.Null(ClassUnderTest.Item);
                var result = await ClassUnderTest.IsValid();
                Assert.False(result);
            }

            [Fact]
            public async void ReturnsTrueIfItemNotNull()
            {
                ClassUnderTest.Item = TestData.TodoItems.TodoItem();
                Assert.True(await ClassUnderTest.IsValid());
            }

            [Fact]
            public async void ReturnsFalseIfItemIdIsDefault()
            {
                ClassUnderTest.Item = TestData.TodoItems.TodoItem();
                ClassUnderTest.Item.id = default(int);

                Assert.False(await ClassUnderTest.IsValid());
            }

            [Theory]
            [InlineData("", false)]
            [InlineData("  ", false)]
            [InlineData("    ", false)]
            [InlineData("\n", false)]
            [InlineData("\t", false)]
            [InlineData("content", true)]
            public async void ReturnsFalseIfItemTitleIsNull(string title, bool expected)
            {
                ClassUnderTest.Item = TestData.TodoItems.TodoItem();
                ClassUnderTest.Item.title = title;

                Assert.Equal(expected, await ClassUnderTest.IsValid());
            }
        }
    }
}
