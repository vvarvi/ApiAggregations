using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAggregation.Infrastructure.Performance
{
    public class PerformanceMetric
    {
        public string SourceName { get; set; } = default!;
        public TimeSpan Duration { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
