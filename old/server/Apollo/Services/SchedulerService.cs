using System;
using System.Collections.Generic;
using System.Linq;

namespace Apollo.Services
{
    public interface ISchedulerService
    {
        IEnumerable<DateTimeOffset> GenerateEvents(Schedule schedule);
        DateTimeOffset? GetNextEvent(Schedule schedule, DateTime? jobLastExecutedAt);
    }

    public class SchedulerService : ISchedulerService
    {
        public IEnumerable<DateTimeOffset> GenerateEvents(Schedule schedule)
        {
            var curr = schedule.start;
            var generatedCount = 0;
            var interval = GenerateInterval(schedule);

            while (schedule.repeat_count == null || generatedCount++ < schedule.repeat_count.Value)
            {
                yield return curr;
                curr = curr.Add(interval);
            }
        }

        public DateTimeOffset? GetNextEvent(Schedule schedule, DateTime? jobLastExecutedAt)
        {
            if (jobLastExecutedAt == null)
                return GenerateEvents(schedule).Take(1).Single();

            var lastExecuted = new DateTimeOffset(jobLastExecutedAt.Value, TimeSpan.Zero);
            var nextEvent = GenerateEvents(schedule).Where(dt => dt > lastExecuted).Take(1).SingleOrDefault();
            if (nextEvent == default(DateTimeOffset))
                return null;
            return nextEvent;
        }

        public TimeSpan GenerateInterval(Schedule schedule)
        {
            if (schedule.daily)
            {
                return TimeSpan.FromDays(1);
            }

            if (schedule.hourly)
            {
                return TimeSpan.FromHours(1);
            }

            if (schedule.minutely)
            {
                return TimeSpan.FromMinutes(1);
            }

            throw new Exception("Invalid Job Schedule");
        }
    }

    public class Schedule
    {
        public DateTime start;
        public bool minutely { get; set; }
        public bool hourly { get; set; }
        public bool daily { get; set; }
        public int? repeat_count { get; set; }
    }
}
