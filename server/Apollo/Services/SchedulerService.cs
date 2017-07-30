using System;
using System.Collections.Generic;

namespace Apollo.Services
{
    public interface ISchedulerService
    {
        IEnumerable<DateTimeOffset> GenerateEvents(Schedule schedule, DateTimeOffset start);
    }

    public class SchedulerService
    {
        public IEnumerable<DateTimeOffset> GenerateEvents(Schedule schedule)
        {
            var curr = schedule.start;
            var generatedCount = 0;
            var interval = GenerateInterval(schedule);
            while (schedule.repeat_count == null || generatedCount++ < schedule.repeat_count.Value)
            {
                curr = curr.Add(interval);
                yield return curr;
            }
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

            return TimeSpan.MaxValue;
        }
    }

    public class Schedule
    {
        public DateTime start;
        public bool hourly { get; set; }
        public bool daily { get; set; }
        public int? repeat_count { get; set; }
    }
}
