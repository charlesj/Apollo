using System.Collections.Concurrent;

namespace Apollo.Utilities
{
    public static class CircuitBreakers
    {
        private static ConcurrentDictionary<string, CircuitBreakerStatus> statuses = new ConcurrentDictionary<string, CircuitBreakerStatus>();

        public static void RecordBreak(string breakerIdentifier)
        {
            if (!statuses.ContainsKey(breakerIdentifier))
            {
                statuses.TryAdd(breakerIdentifier, new CircuitBreakerStatus());
            }

            statuses[breakerIdentifier].FailureCount++;

            if (statuses[breakerIdentifier].FailureCount > 5)
            {
                statuses[breakerIdentifier].Broken = true;
            }
        }

        public static bool IsBroken(string breakerIdentifier)
        {
            return statuses.ContainsKey(breakerIdentifier) && statuses[breakerIdentifier].Broken;
        }
    }

    public class CircuitBreakerStatus
    {
        public bool Broken { get; set; }
        public int FailureCount { get; set; }
    }
}
