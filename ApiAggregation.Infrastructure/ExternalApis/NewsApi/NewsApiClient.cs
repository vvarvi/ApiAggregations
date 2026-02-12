using ApiAggregation.Domain.Models;
using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using ApiAggregation.Infrastructure.ExternalApis.GitHubApi;
using ApiAggregation.Infrastructure.ExternalApis.NewsApi.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ApiAggregation.Infrastructure.ExternalApis.NewsApi
{
    public class NewsApiClient : BaseExternalApiClient, IExternalApiClient
    {
        private readonly ILogger<GitHubApiClient> _logger;

        public override string SourceName => "NewsApi";

        public NewsApiClient(HttpClient httpClient, ILogger<GitHubApiClient> logger) : base(httpClient, "NewsApi")
        {
            _logger = logger;
        }

        public override async Task<IEnumerable<AggregatedItem>> FetchAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Calling News Articles API started...");

                var response = await _httpClient.GetAsync("/news", cancellationToken);

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<AggregatedItem>();

                var json = await response.Content.ReadAsStringAsync();

                var dto = JsonSerializer.Deserialize<NewsArticleResponseDTO>(json);

                _logger.LogInformation("News Articles API succeeded...");

                return new[]
                {
                    new AggregatedItem
                    {
                        Title = dto.Title,
                        Source = SourceName
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching news articles");
                return Enumerable.Empty<AggregatedItem>();
            }
        }

        //public NewsApiClient(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}

        //public async Task<IEnumerable<AggregatedItem>> FetchAsync(CancellationToken cancellationToken)
        //{
        //    var response = await _httpClient.GetFromJsonAsync<NewsApiResponse>("/v2/top-headlines?country=us", cancellationToken);

        //    return response?.Articles?.Select(MapToDomain)
        //        ?? Enumerable.Empty<AggregatedItem>();
        //}

        //private AggregatedItem MapToDomain(NewsArticleDTO dto)
        //{
        //    return new AggregatedItem
        //    {
        //        Source = SourceName,
        //        Title = dto.Title,
        //        Url = dto.Url,
        //        Date = DateTime.TryParse(dto.PublishedAt.ToString(), out var date) ? date : DateTime.UtcNow,
        //        Category = "News"
        //    };
        //}
    }
}
