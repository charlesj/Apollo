using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface ITodoQueueItemDataService
    {
        Task AddItem(TodoQueueItem item);
        Task<IReadOnlyList<TodoQueueItem>> GetIncompleteItems();
        Task UpdateItem(TodoQueueItem item);
    }

    public class TodoQueueItemDataService : BaseDataService, ITodoQueueItemDataService
    {
        public const string InsertSql = "insert into todo_queue_items(title, link, description, created_at) " +
                                        "values (@title, @link, @description, current_timestamp)";

        public const string GetIncompleteItemsSql = "select * from todo_queue_items where completed_at is null";

        public const string UpdateSql = "update todo_queue_items set " +
                                        "title=@title, " +
                                        "link=@link, " +
                                        "description=@description, " +
                                        "completed_at=@completed_at where id=@id";

        public TodoQueueItemDataService(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task AddItem(TodoQueueItem item)
        {
            await Execute(InsertSql, item);
        }

        public async Task<IReadOnlyList<TodoQueueItem>> GetIncompleteItems()
        {
            return await QueryAsync<TodoQueueItem>(GetIncompleteItemsSql);
        }

        public async Task UpdateItem(TodoQueueItem item)
        {
            await Execute(UpdateSql, item);
        }
    }

    public class TodoQueueItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? completed_at { get; set; }
    }
}
