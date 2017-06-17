﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface IJournalDataService
    {
        Task<IReadOnlyList<JournalEntry>> GetAllJournalEntries();
        Task CreateJournalEntry(JournalEntry entry);
    }

    public class JournalDataService : BaseDataService, IJournalDataService
    {
        public JournalDataService(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
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
                connection.Execute($@"
                    insert into journal(note, tags, created_at)
                    values (@note,@tags,current_timestamp)",
                    new {entry.note, entry.tags});
            }
        }
    }
    
    public class JournalEntry
    {
        public int id { get; set; }
        public string note { get; set; }
        public DateTime     created_at { get; set; }
        public string[] tags { get; set; }
    }
}