using System;

namespace Apollo.Data.Documents
{
    public class Goal : IDocument
    {
        public string slug { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTimeOffset startDate { get; set; }
        public DateTimeOffset endDate { get; set; }
        public string metricName { get; set; }
        public Decimal targetValue { get; set; }
        public bool completed { get; set; }
        public DateTimeOffset completedAt { get; set; }
        public bool featured { get; set; }

        public string Id => $"goal:{slug}";
    }
}
