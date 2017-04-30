using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Data.ResultModels;
using Dapper;

namespace Apollo.Data
{
    public interface IJournalDataService
    {
        Task<IReadOnlyList<JournalEntry>> GetAllJournalEntries();
        Task CreateJournalEntry(JournalEntry entry);
    }

    public class JournalDataService : IJournalDataService
    {
        private readonly IDbConnectionFactory connectionFactory;

        public JournalDataService(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IReadOnlyList<JournalEntry>> GetAllJournalEntries()
        {
            using (var connection = await connectionFactory.GetConnection())
            {
                var query = "select * from journal order by id desc";
                var results = await connection
                    .QueryAsync<JournalEntry>(query);
                return results.ToList();
            }
        }

        public async Task CreateJournalEntry(JournalEntry entry)
        {
            using (var connection = await connectionFactory.GetConnection())
            {
                connection.Execute(@"
                    insert into journal(note, created_at)
                    values (@note, current_timestamp)",
                    new {note = entry.note});
            }
        }
    }
}