using System;
using System.Linq;
using Apollo.Services;
using Xunit;

namespace Apollo.Tests.Services
{
    public class ScheduleServiceTests : BaseUnitTest<SchedulerService>
    {
        private DateTime beginTime = new DateTime(1983, 2, 13, 4, 2, 0);

        public class GenerateNextEvents : ScheduleServiceTests
        {
            [Fact]
            public void StartsWithStartTime()
            {
                var schedule = new Schedule {hourly = true, start = beginTime};
                var next = ClassUnderTest.GenerateEvents(schedule).Take(1);
                Assert.Collection(next,
                    e => Assert.Equal(beginTime, e));
            }

            [Fact]
            public void CanGenerateHourlySchedule()
            {
                var schedule = new Schedule {hourly = true, start = beginTime};
                var next = ClassUnderTest.GenerateEvents(schedule).Take(4);
                Assert.Collection(next,
                    e => Assert.Equal(beginTime, e),
                    e => Assert.Equal(beginTime.AddHours(1), e),
                    e => Assert.Equal(beginTime.AddHours(2), e),
                    e => Assert.Equal(beginTime.AddHours(3), e));
            }

            [Fact]
            public void RepeatCountRespected()
            {
                var schedule = new Schedule {daily = true, repeat_count = 1, start = beginTime};
                var next = ClassUnderTest.GenerateEvents(schedule).Take(3);
                Assert.Collection(next,
                    e => Assert.Equal(beginTime, e));
            }
        }

        public class GenerateInterval : ScheduleServiceTests
        {
            [Fact]
            public void HourTimespan()
            {
                var schedule = new Schedule {hourly = true};
                var interval = ClassUnderTest.GenerateInterval(schedule);
                Assert.Equal(TimeSpan.FromHours(1), interval);
            }

            [Fact]
            public void DailyTimespan()
            {
                var schedule = new Schedule {daily = true};
                var interval = ClassUnderTest.GenerateInterval(schedule);
                Assert.Equal(TimeSpan.FromDays(1), interval);
            }
        }
    }
}
