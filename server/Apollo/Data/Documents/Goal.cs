using System;

namespace Apollo.Data.Documents
{
    public class Goal : IDocument
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string MetricName { get; set; }
        public Decimal TargetValue { get; set; }
        public bool Completed { get; set; }
        public DateTimeOffset CompletedAt { get; set; }

        public string Id => $"goal:{Slug}";
    }
}
