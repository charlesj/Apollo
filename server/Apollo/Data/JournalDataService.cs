using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface IJournalDataService
    {
        Task<IReadOnlyList<JournalEntry>> GetJournalEntries(int start);
        Task<JournalEntry> CreateJournalEntry(JournalEntry entry);
        Task<int> GetRecentEntryCount();
        Task<int> GetCount();
    }

    public class JournalDataService : BaseDataService, IJournalDataService
    {
        public const string SelectAllQuery = "select * from journal order by id desc limit 100 offset @start";
        public const string RecentCountSql = "select count(*) from journal where created_at>@created_at;";
        public const string CountSql = "select count(*) from journal;";
        public const string SingleSql = "select * from journal where id=@id";

        public const string InsertSql = "insert into journal(note, tags, created_at) " +
                                        "values (@note,@tags,current_timestamp) returning id";

        public JournalDataService(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<IReadOnlyList<JournalEntry>> GetJournalEntries(int start)
        {
            return await QueryAsync<JournalEntry>(SelectAllQuery, new { start });

        }

        public async Task<JournalEntry> CreateJournalEntry(JournalEntry entry)
        {
            var id = await InsertAndReturnId(InsertSql, entry);
            return (await QueryAsync<JournalEntry>(SingleSql, new {id})).Single();
        }

        public async Task<int> GetRecentEntryCount()
        {
            return (await QueryAsync<CountResult>(RecentCountSql, new { created_at = DateTime.Now.AddDays(-7)})).Single().count;
        }

        public async Task<int> GetCount()
        {
            return (await QueryAsync<CountResult>(CountSql)).Single().count;
        }
    }

    public class JournalEntry
    {
        public int id { get; set; }
        public string note { get; set; }
        public DateTime created_at { get; set; }
        public string[] tags { get; set; }
    }
}
