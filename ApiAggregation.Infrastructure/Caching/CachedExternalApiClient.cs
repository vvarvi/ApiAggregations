using ApiAggregation.Domain.Models;
using ApiAggregation.Infrastructure.Configurations;
using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ApiAggregation.Infrastructure.Caching
{
    public class CachedExternalApiClient : IExternalApiClient
    {
        private readonly IExternalApiClient _externalApiClient;
        private readonly IMemoryCache _cache;
        private readonly ICacheTtlPolicy _ttlPolicy;

        public string SourceName => _externalApiClient.SourceName;

        public CachedExternalApiClient(IExternalApiClient externalApiClient, IMemoryCache cache, ICacheTtlPolicy ttlPolicy)
        {
            _externalApiClient = externalApiClient;
            _cache = cache;
            _ttlPolicy = ttlPolicy;
        }

        public async Task<IEnumerable<AggregatedItem>> FetchAsync(CancellationToken cancellationToken)
        {
            var cacheKey = $"external-api:{SourceName}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<AggregatedItem> cached))
            {
                return cached;
            }

            var result = await _externalApiClient.FetchAsync(cancellationToken);

            _cache.Set(cacheKey, result, _ttlPolicy.GetTtl(SourceName));

            return result;
        }
    }
}
