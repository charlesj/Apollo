using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface IBoardDataService
    {
        Task<IReadOnlyList<Board>> GetBoards();
        Task<Board> SaveBoard(Board board);
        Task DeleteBoard(int id);
        Task<IReadOnlyList<BoardItem>> GetBoardItems(int id);
        Task<BoardItem> SaveItem(BoardItem item);
        Task DeleteBoardItem(int id);
        Task<int> GetBoardCount();
        Task<int> GetRecentlyAddedItemCount();
        Task<int> GetIncompleteItemCount();
        Task<int> GetRecentlyCompletedItemCount();
    }

    public class BoardDataService : BaseDataService, IBoardDataService
    {
        public const string InsertBoardSql = "insert into boards(title, list_order, created_at) " +
                                             "values (@title, @list_order, current_timestamp) returning id";

        public const string GetBoardsSql = "select * from boards order by list_order asc";

        public const string GetBoardSql = "select * from boards where id=@id";

        public const string UpdateBoardSql = "update boards set title=@title, list_order=@list_order where id=@id";

        public const string DeleteBoardSql = "delete from boards where id=@id";

        public const string InsertItemSql = "insert into board_items(board_id, title, link, description, created_at) " +
                                        "values (@board_id, @title, @link, @description, current_timestamp) returning id";

        public const string SingleItemSql = "select * from board_items where id=@id";

        public const string GetBoardItemsSql = "select * from board_items where board_id=@id";

        public const string UpdateItemSql = "update board_items set " +
                                        "board_id=@board_id, " +
                                        "title=@title, " +
                                        "link=@link, " +
                                        "description=@description, " +
                                        "completed_at=@completed_at where id=@id";

        public const string DeleteItemSql = "delete from board_items where id=@id";

        public const string BoardCountSql = "select count(*) from boards";

        public const string IncompleteBoardItemCountSql = "select count(*) from board_items where completed_at is null";

        public const string RecentlyAddedBoardItems = "select count(*) from board_items where created_at>@created_at";

        public const string RecentlyCompletedBoardItems =
            "select count(*) from board_items where completed_at>@completed_at";

        public BoardDataService(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<int> GetBoardCount()
        {
            return (await QueryAsync<CountResult>(BoardCountSql, null)).Single().count;
        }

        public async Task<int> GetRecentlyAddedItemCount()
        {
            return (await QueryAsync<CountResult>(RecentlyAddedBoardItems, new { created_at = DateTime.Now.AddDays(-7)})).Single().count;
        }

        public async Task<int> GetIncompleteItemCount()
        {
            return (await QueryAsync<CountResult>(IncompleteBoardItemCountSql, null)).Single().count;
        }

        public async Task<int> GetRecentlyCompletedItemCount()
        {
            return (await QueryAsync<CountResult>(RecentlyCompletedBoardItems, new { completed_at = DateTime.Now.AddDays(-7)})).Single().count;
        }

        public async Task AddBoard(Board board)
        {
            await Execute(InsertBoardSql, board);
        }

        public async Task<IReadOnlyList<Board>> GetBoards()
        {
            return await QueryAsync<Board>(GetBoardsSql);
        }

        public async Task<Board> SaveBoard(Board board)
        {
            return await Upsert(InsertBoardSql, UpdateBoardSql, GetBoardSql, board);
        }

        public async Task DeleteBoard(int id)
        {
            await Execute(DeleteBoardSql, new {id});
        }

        public async Task<IReadOnlyList<BoardItem>> GetBoardItems(int id)
        {
            return await QueryAsync<BoardItem>(GetBoardItemsSql, new {id});
        }

        public async Task<BoardItem> SaveItem(BoardItem item)
        {
            return await Upsert(InsertItemSql, UpdateItemSql, SingleItemSql, item);
        }

        public async Task DeleteBoardItem(int id)
        {
            await Execute(DeleteItemSql, new {id});
        }
    }

    public class Board : ITableModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public int list_order { get; set; }
        public DateTime created_at { get; set; }
    }

    public class BoardItem : ITableModel
    {
        public int id { get; set; }
        public int board_id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? completed_at { get; set; }
    }
}
