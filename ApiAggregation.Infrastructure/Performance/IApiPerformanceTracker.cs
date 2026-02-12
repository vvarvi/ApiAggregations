using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAggregation.Infrastructure.Performance
{
    public interface IApiPerformanceTracker
    {
        void RecordResponseTime(string apiName, double responseTimeMs);
        double GetRollingAverage(string apiName, TimeSpan window);
        IReadOnlyDictionary<string, double> GetAllRollingAverages(TimeSpan window);
    }
}
