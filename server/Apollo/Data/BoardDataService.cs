using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface IBoardDataService
    {
        Task AddBoard(Board board);
        Task<IReadOnlyList<Board>> GetBoards();
        Task UpdateBoard(Board board);
        Task AddItem(BoardItem item);
        Task<IReadOnlyList<BoardItem>> GetBoardItems(int id);
        Task UpdateItem(BoardItem item);
    }

    public class BoardDataService : BaseDataService, IBoardDataService
    {
        public const string InsertBoardSql = "insert into boards(title, list_order, created_at) " +
                                             "values (@title, @list_order, current_timestamp)";

        public const string GetBoardsSql = "select * from boards order by list_order desc";

        public const string UpdateBoardSql = "update boards set title=@title, list_order=@list_order where id=@id";

        public const string InsertItemSql = "insert into board_items(board_id, title, link, description, created_at) " +
                                        "values (@board_id, @title, @link, @description, current_timestamp)";

        public const string GetBoardItemsSql = "select * from board_items where board_id=@id";

        public const string UpdateItemSql = "update board_items set " +
                                        "board_id=@board_id, " +
                                        "title=@title, " +
                                        "link=@link, " +
                                        "description=@description, " +
                                        "completed_at=@completed_at where id=@id";

        public BoardDataService(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task AddBoard(Board board)
        {
            await Execute(InsertBoardSql, board);
        }

        public async Task<IReadOnlyList<Board>> GetBoards()
        {
            return await QueryAsync<Board>(GetBoardsSql);
        }

        public async Task UpdateBoard(Board board)
        {
            await Execute(UpdateBoardSql, board);
        }

        public async Task AddItem(BoardItem item)
        {
            await Execute(InsertItemSql, item);
        }

        public async Task<IReadOnlyList<BoardItem>> GetBoardItems(int id)
        {
            return await QueryAsync<BoardItem>(GetBoardItemsSql, new { id });
        }

        public async Task UpdateItem(BoardItem item)
        {
            await Execute(UpdateItemSql, item);
        }
    }

    public class Board
    {
        public int id { get; set; }
        public string title { get; set; }
        public int list_order { get; set; }
        public DateTime created_at { get; set; }
    }

    public class BoardItem
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
