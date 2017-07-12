using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface ITodoItemDataService
    {
        Task AddItem(string title);
        Task UpdateItem(TodoItem item);
        Task<IReadOnlyList<TodoItem>> GetIncompleteItems();
        Task<IReadOnlyList<TodoItem>> GetRecentlyCompletedItems();
    }

    public class TodoItemDataService: BaseDataService, ITodoItemDataService
    {
        public const string IncompleteItemsQuery = "select * from todo_items where completed_at is null";

        public const string RecententlyCompletedItemsQuery =
            "select * from todo_items where " +
            "completed_at > (current_timestamp - interval '1 week')";

        public const string InsertItemQuery =
            "insert into todo_items(title, created_at) values (@title, current_timestamp)";

        public const string UpdateItemQuery =
            "update todo_items set title=@title, completed_at=@completed_at where id=@id";

        public TodoItemDataService(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task AddItem(string title)
        {
            await Execute(InsertItemQuery, new {title});
        }

        public async Task UpdateItem(TodoItem item)
        {
            await Execute(UpdateItemQuery, item);
        }

        public async Task<IReadOnlyList<TodoItem>> GetIncompleteItems()
        {
            return await QueryAsync<TodoItem>(IncompleteItemsQuery);
        }

        public async Task<IReadOnlyList<TodoItem>> GetRecentlyCompletedItems()
        {
            return await QueryAsync<TodoItem>(RecententlyCompletedItemsQuery);
        }
    }

    public class TodoItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? completed_at { get; set; }
    }
}
