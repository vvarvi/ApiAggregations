using System;
using System.Collections.Concurrent;

namespace ApiAggregation.Infrastructure.Performance
{
    public class InMemoryApiPerformanceTracker : IApiPerformanceTracker
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<(DateTime Timestamp, double ResponseTime)>> _apiResponseTimes;

        public InMemoryApiPerformanceTracker()
        {
            _apiResponseTimes = new ConcurrentDictionary<string, ConcurrentQueue<(DateTime, double)>>();
        }

        public void RecordResponseTime(string apiName, double responseTimeMs)
        {
            var queue = _apiResponseTimes.GetOrAdd(apiName, _ => new ConcurrentQueue<(DateTime, double)>());
            queue.Enqueue((DateTime.UtcNow, responseTimeMs));
        }

        public double GetRollingAverage(string apiName, TimeSpan window)
        {
            if (!_apiResponseTimes.TryGetValue(apiName, out var queue))
                return 0;

            // Remove old entries outside the rolling window
            while (queue.TryPeek(out var entry) && entry.Timestamp < DateTime.UtcNow - window)
            {
                queue.TryDequeue(out _);
            }

            if (!queue.Any())
                return 0;

            return queue.Average(x => x.ResponseTime);
        }

        public IReadOnlyDictionary<string, double> GetAllRollingAverages(TimeSpan window)
        {
            return _apiResponseTimes.Keys
                .ToDictionary(api => api, api => GetRollingAverage(api, window));
        }
    }
}
