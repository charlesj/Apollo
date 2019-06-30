using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface ITimelineDataService
    {
        Task<TimelineEntry> RecordEntry(string title, string reference_type = null, int? reference_id = null, DateTime? event_time = null);
        Task<IReadOnlyList<TimelineEntry>> GetEntries(DateTime start, DateTime? end);
        Action<int> Callback(string title, string reference_type);
    }

    public class TimelineDataService : BaseDataService, ITimelineDataService
    {
        public TimelineDataService(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<TimelineEntry> RecordEntry(string title, string reference_type = null, int? reference_id = null, DateTime? event_time = null)
        {
            if(event_time == null) event_time = DateTime.UtcNow;

            var id = await InsertAndReturnId("insert into timeline(title, event_time, reference_type, reference_id, " +
                                          "created_at) values (@title, @event_time, @reference_type, " +
                                          "@reference_id, current_timestamp) returning id",
                                       new {title, event_time, reference_type, reference_id });

            var entry = await QueryAsync<TimelineEntry>("select * from timeline where id=@id", new {id});
            return entry.Single();
        }

        public Task<IReadOnlyList<TimelineEntry>> GetEntries(DateTime start, DateTime? end)
        {
            return QueryAsync<TimelineEntry>("select * from timeline where event_time>=@start and " +
                                             "@end is null or event_time<=@end", new {start, end});
        }

        public Action<int> Callback(string title, string reference_type)
        {
            async void callback(int id)
            {
                await RecordEntry(title, reference_type, id);
            }

            return callback;
        }
    }

    public class TimelineEntry
    {
        public int id { get; set; }
        public string title { get; set; }
        public int? reference_id { get; set; }
        public string reference_type { get; set; }
        public DateTime event_time { get; set; }
        public DateTime created_at { get; set; }
    }
}
