using ApiAggregation.Domain.Models;
using ApiAggregation.Infrastructure.ExternalApis.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAggregation.Infrastructure.ExternalApis
{
    public class FakeExternalApiClient : IExternalApiClient
    {
        public string SourceName => "NewsAPI";

        public int CallCount { get; private set; }

        public Task<IEnumerable<AggregatedItem>> FetchAsync(CancellationToken cancellationToken)
        {
            CallCount++;

            return Task.FromResult<IEnumerable<AggregatedItem>>(new[]
            {
            new AggregatedItem { Title = "Test", Source = SourceName }
        });
        }
    }
}
