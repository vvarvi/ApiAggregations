using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;

namespace ApiAggregation.Infrastructure.Performance
{
    public class PerformanceAnalyzerHostedService : BackgroundService
    {
        private readonly ILogger<PerformanceAnalyzerHostedService> _logger;
        private readonly IApiPerformanceTracker _tracker;
        private readonly Meter _meter;
        private readonly Counter<long> _anomalyCounter;
        private readonly TimeSpan _rollingWindow = TimeSpan.FromMinutes(5);

        public PerformanceAnalyzerHostedService(
            ILogger<PerformanceAnalyzerHostedService> logger,
            IApiPerformanceTracker tracker)
        {
            _logger = logger;
            _tracker = tracker;

            _meter = new Meter("ApiPerformanceMetrics", "1.0.0");
            _anomalyCounter = _meter.CreateCounter<long>("api_anomalies_total");
        }

        public void Analyze(string source)
        {
            var baseline = _tracker.GetMetrics(source, TimeSpan.FromMinutes(30));
            var recent = _tracker.GetMetrics(source, TimeSpan.FromMinutes(5));

            if (!baseline.Any() || !recent.Any())
                return;

            var baselineAvg = baseline.Average(m => m.Duration.TotalMilliseconds);
            var recentAvg = recent.Average(m => m.Duration.TotalMilliseconds);

            if (recentAvg > baselineAvg * 1.5)
            {
                _logger.LogWarning(
                    "PERFORMANCE ANOMALY: {Source} degraded. Baseline={Baseline}ms Recent={Recent}ms",
                    source, baselineAvg, recentAvg);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Performance Analyzer Hosted Service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    AnalyzePerformance();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Performance Analyzer");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private void AnalyzePerformance()
        {
            var averages = _tracker.GetAllRollingAverages(_rollingWindow);

            foreach (var (api, rollingAvg) in averages)
            {
                if (rollingAvg == 0) continue;

                // Compute overall average (for simplicity: using same rolling avg as baseline)
                var overallAvg = rollingAvg;

                // Detect spike >50%
                if (rollingAvg > 1.5 * overallAvg)
                {
                    _logger.LogWarning(
                        "Performance spike detected for {Api}. Rolling Avg: {RollingAvg:F2}ms",
                        api, rollingAvg);

                    //_anomalyCounter.Add((1, new("api_name", api));
                }

                // Optional: OpenTelemetry gauge
                _meter.CreateObservableGauge("api_response_time_ms", () => rollingAvg, unit: "ms");
            }
        }
    }
}
