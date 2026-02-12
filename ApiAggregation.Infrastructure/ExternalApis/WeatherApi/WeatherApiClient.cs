using ApiAggregation.Domain.Models;
using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using ApiAggregation.Infrastructure.ExternalApis.GitHubApi;
using ApiAggregation.Infrastructure.ExternalApis.WeatherApi.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ApiAggregation.Infrastructure.ExternalApis.WeatherApi
{
    public class WeatherApiClient : BaseExternalApiClient, IExternalApiClient
    {
        private readonly ILogger<GitHubApiClient> _logger;

        public override string SourceName => "WeatherApi";

        public WeatherApiClient(HttpClient httpClient, ILogger<GitHubApiClient> logger) : base(httpClient, "WeatherApi")
        {
            _logger = logger;
        }

        //API Client Example με Resilience
        //public async Task<IEnumerable<AggregatedItem>> FetchAsync(CancellationToken cancellationToken)
        //{
        //    var response = await _http.GetAsync("/weather", cancellationToken);

        //    if (!response.IsSuccessStatusCode)
        //        return Enumerable.Empty<AggregatedItem>();

        //    return Parse(await response.Content.ReadAsStringAsync());
        //}

        public override async Task<IEnumerable<AggregatedItem>> FetchAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Calling Weather API started...");

                var response = await _httpClient.GetAsync("/weather", cancellationToken);

                if (response == null) return Enumerable.Empty<AggregatedItem>();

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<AggregatedItem>();

                var json = await response.Content.ReadAsStringAsync();

                var dto = JsonSerializer.Deserialize<WeatherApiResponseDTO>(json);

                _logger.LogInformation("Weather API succeeded...");

                return new[]
                {
                    new AggregatedItem
                    {
                        Source = SourceName,
                        Title = $"Weather in ",
                        Category = "Weather",
                        Date = DateTime.UtcNow,
                        Url = "https://openweathermap.org"
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calling Weather API.");
                return Enumerable.Empty<AggregatedItem>();
            }
        }
    }

    public class WeatherResponse
    {
        public string Name { get; set; }
        public MainInfo Main { get; set; }
    }

    public class MainInfo
    {
    }
}
