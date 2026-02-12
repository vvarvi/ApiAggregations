using ApiAggregation.Domain.Models;
using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using ApiAggregation.Infrastructure.Observability.Logging.Correlation;
using Microsoft.Extensions.Logging;

namespace ApiAggregation.Infrastructure.ExternalApis.GitHubApi
{
    public class GitHubApiClient : BaseExternalApiClient, IExternalApiClient
    {
        private readonly ILogger<GitHubApiClient> _logger;

        private readonly ICorrelationIdAccessor _correlationAccessor;
        public override string SourceName => "GitHubAPI";

        public GitHubApiClient(HttpClient httpClient, ILogger<GitHubApiClient> logger, ICorrelationIdAccessor correlationAccessor) : base(httpClient, "GitHubAPI")
        {
            _logger = logger;
            _correlationAccessor = correlationAccessor;
        }

        public override async Task<IEnumerable<AggregatedItem>> FetchAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Calling GitHub API started. CorrelationId: {CorrelationId}", _correlationAccessor.CorrelationId);

                var response = await _httpClient.GetAsync("/github/repos", cancellationToken);

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<AggregatedItem>();

                _logger.LogInformation("GitHub API succeeded...");

                return new[]
                {
                    new AggregatedItem
                    {
                        Title = "GitHub Repo Example",
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
