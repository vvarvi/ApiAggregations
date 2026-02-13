using ApiAggregation.Domain.Models;
using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using ApiAggregation.Infrastructure.ExternalApis.GitHubApi.Models;
using ApiAggregation.Infrastructure.ExternalApis.NewsApi.Models;
using ApiAggregation.Infrastructure.Observability.Logging.Correlation;
using ApiAggregation.Infrastructure.Performance;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ApiAggregation.Infrastructure.ExternalApis.GitHubApi
{
    public class GitHubApiClient : BaseExternalApiClient, IExternalApiClient
    {
        private readonly ILogger<GitHubApiClient> _logger;

        private readonly ApiConfig _config;

        private readonly IApiPerformanceTracker _metrics;

        private readonly ICorrelationIdAccessor _correlationAccessor;

        public override string SourceName => "GitHubAPI";

        public GitHubApiClient(HttpClient httpClient, IOptions<ExternalApiOptions> options, ILogger<GitHubApiClient> logger, ICorrelationIdAccessor correlationAccessor, IApiPerformanceTracker metrics) : base(httpClient, "GitHubAPI", metrics)
        {
            _logger = logger;
            _correlationAccessor = correlationAccessor;
            _metrics = metrics;
            _config = options.Value.GitHub;
            _httpClient.BaseAddress = new Uri(_config.BaseUrl);
        }

        public override async Task<IEnumerable<AggregatedItem>> FetchAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Calling GitHub API started. CorrelationId: {CorrelationId}", _correlationAccessor.CorrelationId);

                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("ApiAggregationApp");

                var response = await _httpClient.GetAsync("/users/dotnet/repos", cancellationToken);

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<AggregatedItem>();

                var json = await response.Content.ReadAsStringAsync(cancellationToken);

                var dto = JsonSerializer.Deserialize<GitHubResponseDTO>(json);


                _logger.LogInformation("GitHub API succeeded...");

                return new[]
                {
                    new AggregatedItem
                    {
                        Title = dto.Name,
                        Source = SourceName
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calling GitHub API.");
                return Enumerable.Empty<AggregatedItem>();
            }

            //public GitHubApiClient(HttpClient httpClient)
            //{
            //    _httpClient = httpClient;
            //}

            //public async Task<IEnumerable<AggregatedItem>> FetchAsync(CancellationToken cancellationToken)
            //{
            //    var repos = await _httpClient.GetFromJsonAsync<List<GitHubResponseDTO>>("/users/dotnet/repos", cancellationToken);

            //    return repos?.Select(r => new AggregatedItem
            //    {
            //        Source = SourceName,
            //        Title = r.Name,
            //        Url = r.HtmlUrl,
            //        Category = "GitHub",
            //        Date = DateTime.UtcNow
            //    }) ?? Enumerable.Empty<AggregatedItem>();
            //}
        }
    }
}
