using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Commands.Todo;
using Apollo.Data;
using Xunit;

namespace Apollo.Tests.Commands.Todo
{
    public class GetTodoItemsTests : BaseUnitTest<GetTodoItems>
    {
        public class Execute : GetTodoItemsTests
        {
            List<TodoItem> incompleteItems = new List<TodoItem>{ TestData.TodoItems.TodoItem() };
            List<TodoItem> recentlyCompletedItems = new List<TodoItem>
            {
                TestData.TodoItems.TodoItem(completed_at: DateTime.Now.AddHours(-1))
            };

            public Execute()
            {
                Mock<ITodoItemDataService>()
                    .Setup(t => t.GetIncompleteItems())
                    .Returns(Task.FromResult((IReadOnlyList<TodoItem>) incompleteItems));

                Mock<ITodoItemDataService>()
                    .Setup(t => t.GetRecentlyCompletedItems())
                    .Returns(Task.FromResult((IReadOnlyList<TodoItem>) recentlyCompletedItems));
            }

            [Fact]
            public async void GetsAllIncompleteItems()
            {
                var result = await ClassUnderTest.Execute();

                var resultData = (IEnumerable<TodoItem>) result.Result;
                Assert.True(incompleteItems.All(ii => resultData.Contains(ii)));
            }

            [Fact]
            public async void IncludesRecentlyCompletedItems()
            {
                var result = await ClassUnderTest.Execute();

                var resultData = (IEnumerable<TodoItem>) result.Result;
                Assert.True(recentlyCompletedItems.All(ii => resultData.Contains(ii)));
            }
        }

        public class IsValid : GetTodoItemsTests
        {
            [Fact]
            public async void AlwaysTrue()
            {
                Assert.True(await ClassUnderTest.IsValid());
            }
        }
    }
}
