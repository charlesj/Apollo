using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface INotebookDataService
    {
        Task CreateNote(string name, string body);
        Task<Note> GetNote(int id);
        Task<IReadOnlyList<Note>> GetNoteListing();
        Task<Note> UpsertNote(Note note);
        Task DeleteNote(int id);
        Task<int> GetNoteCount();
    }

    public class NotebookDataService : BaseDataService, INotebookDataService
    {
        private readonly ITimelineDataService timelineDataService;

        public const string CreateNoteSql = "insert into notes(name,body,created_at,modified_at) values " +
                                            "(@name, @body, current_timestamp, current_timestamp) returning id";

        public const string CountSql = "select count(*) from notes";

        public const string GetNoteSql = "select * from notes where id=@id";

        public const string GetNoteListingSql = "select * from notes " +
                                                "order by modified_at desc";

        public const string UpdateNoteSql = "update notes set name=@name, body=@body, modified_at=current_timestamp " +
                                            "where id=@id";

        public const string DeleteNoteSql = "delete from notes where id=@id";

        public NotebookDataService(IConnectionFactory connectionFactory, ITimelineDataService timelineDataService) : base(connectionFactory)
        {
            this.timelineDataService = timelineDataService;
        }

        public Task CreateNote(string name, string body)
        {
            return Execute(CreateNoteSql, new {name, body});
        }

        public async Task<Note> GetNote(int id)
        {
            var results = await QueryAsync<Note>(GetNoteSql, new {id});
            return results.Single();
        }

        public Task<IReadOnlyList<Note>> GetNoteListing()
        {
            return QueryAsync<Note>(GetNoteListingSql);
        }

        public async Task<Note> UpsertNote(Note note)
        {
            var insertCallback =
                timelineDataService.Callback($"Created Note {note.name}", Constants.TimelineReferences.Note);

            var updateCallback =
                timelineDataService.Callback($"Edited Note {note.name}", Constants.TimelineReferences.Note);

            return await Upsert(CreateNoteSql, UpdateNoteSql, GetNoteSql, note, insertCallback, updateCallback);
        }

        public Task DeleteNote(int id)
        {
            return Execute(DeleteNoteSql, new {id});
        }

        public async Task<int> GetNoteCount()
        {
            return (await QueryAsync<CountResult>(CountSql, null)).Single().count;
        }
    }

    public class Note : ITableModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string body { get; set; }
        public DateTime created_at { get; set; }
        public DateTime modified_at { get; set; }
    }
}
