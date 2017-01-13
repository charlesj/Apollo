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
                var results = await connection.QueryAsync<JournalEntry>("select * from journal");
                return results.ToList();
            }
        }
    }
}