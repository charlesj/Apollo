using System;

namespace Apollo.Data.ResultModels
{
    public class JournalEntry
    {
        public int id { get; set; }
        public string note { get; set; }
        public DateTime created_at { get; set; }
    }
}