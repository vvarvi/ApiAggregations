
using ApiAggregation.Domain.Models;
using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using ApiAggregation.Infrastructure.Performance;
using System.Net.Http;

namespace ApiAggregation.Infrastructure.ExternalApis
{
    public abstract class BaseExternalApiClient : IExternalApiClient
    {
        protected readonly HttpClient _httpClient;

        private readonly IApiPerformanceTracker _metrics;

        public virtual string SourceName { get; }

        protected BaseExternalApiClient(HttpClient httpClient, string sourceName, IApiPerformanceTracker metrics)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            SourceName = sourceName;
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));                  
        }

        public abstract Task<IEnumerable<AggregatedItem>> FetchAsync(CancellationToken cancellationToken);
    }
}
