using System;
using System.Threading.Tasks;

namespace Apollo.Utilities
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
        Task Delay(int milliseconds);
    }

    public class Clock : IClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

        public async Task Delay(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }
    }
}