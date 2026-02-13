using ApiAggregation.Domain.Models;
using ApiAggregation.Infrastructure.Caching;
using ApiAggregation.Infrastructure.ExternalApis;
using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace ApiAggregation.Tests.Caching
{
    public class CachedExternalApiClientTests:IExternalApiClient
    {
        [Fact]
        public async Task Returns_Cached_Result_On_Second_Call()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var fakeClient = new FakeExternalApiClient();

            var ttlPolicy = new FakeTtlPolicy();

            var cachedClient = new CachedExternalApiClient(
                fakeClient,
                memoryCache,
                ttlPolicy);

            // First call → hits API
            var first = await cachedClient.FetchAsync(CancellationToken.None);

            // Second call → should use cache
            var second = await cachedClient.FetchAsync(CancellationToken.None);

            Assert.Equal(1, fakeClient.CallCount);
            Assert.Single(first);
            Assert.Single(second);
        }
    }
}
