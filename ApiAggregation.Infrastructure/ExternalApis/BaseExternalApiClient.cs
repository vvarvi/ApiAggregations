
using ApiAggregation.Domain.Models;
using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using System.Net.Http;

namespace ApiAggregation.Infrastructure.ExternalApis
{
    public abstract class BaseExternalApiClient : IExternalApiClient
    {
        protected readonly HttpClient _httpClient;

        public virtual string SourceName { get; }

        protected BaseExternalApiClient(HttpClient httpClient, string sourceName)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            SourceName = sourceName;
        }

        public abstract Task<IEnumerable<AggregatedItem>> FetchAsync(CancellationToken cancellationToken);
    }
}
