using Microsoft.Extensions.Hosting;

namespace ApiAggregation.Infrastructure.Performance
{
    public class PerformanceLoggingHostedService : BackgroundService
    {
        private readonly PerformanceAnalyzerHostedService _analyzer;

        public PerformanceLoggingHostedService(PerformanceAnalyzerHostedService analyzer)
        {
            _analyzer = analyzer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _analyzer.Analyze("WeatherAPI");
                _analyzer.Analyze("NewsAPI");
                _analyzer.Analyze("GitHubAPI");

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }

}
