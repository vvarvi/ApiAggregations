using ApiAggregation.Infrastructure.Caching;
using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ApiAggregation.Infrastructure.ExternalApis
{
    public class CachedExternalApiClient
    {
        private readonly ICacheTtlPolicy _ttlPolicy;

        private readonly IExternalApiClient _inner;

        private readonly IMemoryCache _cache;

        public CachedExternalApiClient(IExternalApiClient inner, IMemoryCache cache, ICacheTtlPolicy ttlPolicy)
        {
            _inner = inner;
            _cache = cache;
            _ttlPolicy = ttlPolicy;
        }
    }
}
